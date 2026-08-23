// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.CertProcs
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

#nullable disable
namespace Intermech.Signs.Interfaces;

public class CertProcs
{
  public const string MY = "MY";

  /// <summary>
  /// Возвращает выборку сертификатов, которые можно использовать для подписи.
  /// выборка выполняется из всех сертификатов, находящихся в личном хранилище сертификатов и
  /// использующих криптопровайдеры, зарегистрированные в системе
  /// </summary>
  /// <param name="session"></param>
  /// <returns></returns>
  public static X509Certificate2Collection GetPossibleCertificates(IUserSession session)
  {
    List<X509Certificate2> certList = new List<X509Certificate2>();
    X509Store x509Store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
    x509Store.Open(OpenFlags.ReadOnly);
    CertProcs.FilterCertificates(session, x509Store.Certificates, certList, SignsHolder.DoRevocation);
    return new X509Certificate2Collection(certList.ToArray());
  }

  /// <summary>
  /// отфильтровать из всей коллекции сертификатов те из них,
  /// которые поддерживают криптопровайдеры, зарегистрированные в системе, и являются действующими
  /// </summary>
  /// <param name="x509Certificate2Collection"></param>
  /// <param name="certList"></param>
  /// <param name="verifyCert">проверить сертификаты через сервер отзыва сертификатов</param>
  private static void FilterCertificates(
    IUserSession session,
    X509Certificate2Collection x509Certificate2Collection,
    List<X509Certificate2> certList,
    bool verifyCert)
  {
    DateTime now = DateTime.Now;
    string str = "timestamp_" + now.ToLongTimeString();
    if (SignsHolder.SignsDeveloperMode)
      CertProcs.WriteToEventLog(session.EventLog, $"{str}> сертификатов выбрано из контейнера: {x509Certificate2Collection.Count.ToString()}");
    certList.Clear();
    foreach (X509Certificate2 x509Certificate2 in x509Certificate2Collection)
    {
      if (x509Certificate2.HasPrivateKey)
      {
        DateTime notBefore = x509Certificate2.NotBefore;
        DateTime notAfter = x509Certificate2.NotAfter;
        if (!(now < notBefore))
        {
          if (!(now > notAfter))
          {
            try
            {
              if (verifyCert)
              {
                X509ChainStatus[] chStatus = (X509ChainStatus[]) null;
                if (CertProcs.GetX509VerifyResultsV4(x509Certificate2, false, out chStatus))
                {
                  if (SignsHolder.SignsDeveloperMode)
                    CertProcs.WriteToEventLog(session.EventLog, $"{str}> > сертификат добавлен в выборку - прошёл проверку ({x509Certificate2.Subject}; {x509Certificate2.Issuer})");
                  certList.Add(x509Certificate2);
                  continue;
                }
                if (SignsHolder.SignsDeveloperMode)
                  CertProcs.WriteToEventLog(session.EventLog, $"{str}> сертификат исключен из выборки - не прошёл проверку, подробности ниже ({x509Certificate2.Subject}; {x509Certificate2.Issuer})");
                foreach (X509ChainStatus x509ChainStatus in chStatus)
                  CertProcs.WriteToEventLog(session.EventLog, $"{str}> {x509ChainStatus.StatusInformation} ({x509Certificate2.Subject}; {x509Certificate2.Issuer})");
                continue;
              }
              if (SignsHolder.SignsDeveloperMode)
                CertProcs.WriteToEventLog(session.EventLog, $"{str}> сертификат добавлен в выборку - проверка не выполнялась ({x509Certificate2.Subject}; {x509Certificate2.Issuer})");
              certList.Add(x509Certificate2);
              continue;
            }
            catch (CryptographicException ex)
            {
              if (SignsHolder.SignsDeveloperMode)
              {
                CertProcs.WriteToEventLog(session.EventLog, $"{str}> сертификат исключен из выборки - исключение {ex.Message} ({x509Certificate2.Subject}; {x509Certificate2.Issuer})");
                continue;
              }
              continue;
            }
          }
        }
        if (SignsHolder.SignsDeveloperMode)
          CertProcs.WriteToEventLog(session.EventLog, $"{str}> сертификат исключен из выборки - не пройдена проверка на сроки действия [{notBefore.ToShortDateString()}..{notAfter.ToShortDateString()}]({x509Certificate2.Subject}; {x509Certificate2.Issuer})");
      }
      else if (SignsHolder.SignsDeveloperMode)
        CertProcs.WriteToEventLog(session.EventLog, $"{str}> сертификат исключен из выборки - отсутствует закрытый ключ для ({x509Certificate2.Subject}; {x509Certificate2.Issuer})");
    }
  }

  /// <summary>
  /// Проверяет сертификат и определяет, надо ли писать предупреждение про подписывание этим сертификатом
  /// </summary>
  /// <param name="cert"></param>
  /// <param name="writeToLog"></param>
  /// <returns> == null не все гладко - то ли нет закрытого ключа, то ли не читается информация из него</returns>
  public static CspKeyContainerInfo GetCertInfo(X509Certificate2 cert, bool writeToLog)
  {
    if (cert != null)
    {
      if (cert.HasPrivateKey)
      {
        try
        {
          return (cert.PrivateKey as ICspAsymmetricAlgorithm).CspKeyContainerInfo;
        }
        catch
        {
        }
        if (writeToLog)
        {
          using (SessionKeeper sessionKeeper = new SessionKeeper())
            CertProcs.WriteToEventLog(sessionKeeper.Session.EventLog, $"> предупреждение: криптопровайдер не поддерживает некоторые возможности CryptoAPI: ICspAsymmetricAlgorithm ({cert.Subject}; {cert.Issuer})");
        }
      }
    }
    return (CspKeyContainerInfo) null;
  }

  private static void WriteToEventLog(IEventLog log, string s)
  {
    string str = $"Модуль подписей{(!SignsHolder.SignsDeveloperMode ? string.Empty : " (режим разработчика)")}: ";
    log.AddToTrace(str + s, Consts.traceAlways, string.Empty);
  }

  /// <summary>
  /// Проверка цепочки сертификатов через сервер отзыва сертификатов, имеет смысл вызывать если !cert.Verify()
  /// </summary>
  /// <param name="cert">сертификат для разбора</param>
  /// <param name="chStatus">статус проверки цепочки сертификатов</param>
  /// <returns>валидный или нет</returns>
  [Obsolete]
  public static bool GetX509VerifyResults(X509Certificate2 cert, out X509ChainStatus[] chStatus)
  {
    X509Chain x509Chain = new X509Chain();
    x509Chain.ChainPolicy.RevocationMode = SignsHolder.RevocationMode;
    if (true)
      x509Chain.ChainPolicy.VerificationFlags = X509VerificationFlags.IgnoreNotTimeValid;
    int num = x509Chain.Build(cert) ? 1 : 0;
    chStatus = x509Chain.ChainStatus;
    return num != 0;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="cert"></param>
  /// <param name="whenCheckSign">true на этапе проверки подписи - не учитывается, что срок действия сертификата прошёл, false на этапе выбора сертификата </param>
  /// <param name="chStatus"></param>
  /// <returns></returns>
  public static bool GetX509VerifyResultsV4(
    X509Certificate2 cert,
    bool whenCheckSign,
    out X509ChainStatus[] chStatus)
  {
    X509Chain x509Chain = new X509Chain()
    {
      ChainPolicy = {
        RevocationFlag = X509RevocationFlag.EntireChain,
        RevocationMode = SignsHolder.RevocationMode,
        VerificationFlags = whenCheckSign ? X509VerificationFlags.IgnoreNotTimeValid : X509VerificationFlags.NoFlag,
        VerificationTime = DateTime.Now,
        UrlRetrievalTimeout = new TimeSpan(0, 0, 30)
      }
    };
    bool x509VerifyResultsV4 = x509Chain.Build(cert);
    chStatus = x509Chain.ChainStatus;
    if (x509Chain.ChainStatus.Length != 0 && ((IEnumerable<X509ChainStatus>) x509Chain.ChainStatus).Any<X509ChainStatus>((Func<X509ChainStatus, bool>) (status => status.Status == X509ChainStatusFlags.NotTimeValid || status.Status == X509ChainStatusFlags.Revoked || status.Status == X509ChainStatusFlags.UntrustedRoot)))
      x509VerifyResultsV4 = false;
    return x509VerifyResultsV4;
  }

  /// <summary>Выбрать один сертификат из списка</summary>
  /// <param name="certificates"></param>
  /// <param name="title"></param>
  /// <param name="message"></param>
  /// <returns></returns>
  public static X509Certificate2 SelectCertificate(
    X509Certificate2Collection certificates,
    string title,
    string message)
  {
    X509Certificate2 x509Certificate2 = (X509Certificate2) null;
    X509Certificate2Collection certificate2Collection = X509Certificate2UI.SelectFromCollection(certificates, title, message, X509SelectionFlag.SingleSelection);
    if (certificate2Collection.Count > 0)
      x509Certificate2 = certificate2Collection[0];
    return x509Certificate2;
  }

  /// <summary>
  /// Класс, описывающий зарегистрированный в системе криптопровайдер
  /// </summary>
  public struct CryptoProvider(string name, int cryptoTypeId, int cryptoAlgId)
  {
    public string Name = name;
    public int CryptoTypeId = cryptoTypeId;
    public int CryptoAlgId = cryptoAlgId;
  }
}
