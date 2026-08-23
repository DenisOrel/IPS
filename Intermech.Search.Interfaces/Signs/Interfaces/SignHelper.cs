// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.SignHelper
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using Intermech.Interfaces;
using System;
using System.Security.Cryptography.X509Certificates;

#nullable disable
namespace Intermech.Signs.Interfaces;

/// <summary>
/// 
/// </summary>
public class SignHelper
{
  /// <summary>Тип объекта "Подпись с криптозащитой"</summary>
  private static readonly Guid cryptoSignObjectTypeGuid = new Guid("cad00138-306c-11d8-b4e9-00304f19f545");
  private static int _cryptoSignObjectTypeId = 0;

  /// <summary>Версия подписи</summary>
  /// <param name="algVersion">Версия алгоритма</param>
  /// <returns></returns>
  [Obsolete("Версия алгоритма не указывает на переносимость или не переносимость подписи")]
  public static SignVersions TranslateVersion(int algVersion)
  {
    return algVersion != 0 ? SignVersions.Portable : SignVersions.Unbearable;
  }

  private static int GetCryptoSignObjectTypeId(IUserSession session)
  {
    if (SignHelper._cryptoSignObjectTypeId == 0)
      SignHelper._cryptoSignObjectTypeId = session.GetObjectType(SignHelper.cryptoSignObjectTypeGuid).ObjectType;
    return SignHelper._cryptoSignObjectTypeId;
  }

  /// <summary>Статус подписи</summary>
  /// <param name="session">Пользовательская сессия</param>
  /// <param name="objectID">Идентификатор версии подписываемого объекта</param>
  /// <param name="signObjectID">Идентификатор версии подписи</param>
  /// <param name="signObjectType">Идентификатор типа подписи</param>
  /// <param name="objectModifyDate">Дата модификации подписываемого объекта: для внутренней подписи; для криптоподписей не имеет значения</param>
  /// <param name="signDate">Дата подписи: для внутренней подписи; для криптоподписей не имеет значения</param>
  /// <param name="certificates">сертификаты подписанта, если есть</param>
  /// <returns></returns>
  public static SignStatuses TranslateStatus(
    IUserSession session,
    long objectID,
    long signObjectID,
    int signObjectType,
    DateTime objectModifyDate,
    DateTime signDate,
    out X509Certificate2Collection certificates)
  {
    return SignHelper.TranslateStatus(session, objectID, signObjectID, signObjectType, objectModifyDate, signDate, SignHelper.GetCryptoSignObjectTypeId(session), out certificates);
  }

  /// <summary>Статус подписи</summary>
  /// <param name="session">Пользовательская сессия</param>
  /// <param name="objectID">Идентификатор версии подписываемого объекта</param>
  /// <param name="signObjectID">Идентификатор версии подписи</param>
  /// <param name="signObjectType">Идентификатор типа подписи</param>
  /// <param name="objectModifyDate">Дата модификации подписываемого объекта: для внутренней подписи; для криптоподписей не имеет значения</param>
  /// <param name="signDate">Дата подписи: для внутренней подписи; для криптоподписей не имеет значения</param>
  /// <param name="cryptoSignObjectTypeID">Идентификатор типа объектов "Подпись с криптозащитой"</param>
  /// <param name="certificates">сертификаты подписанта, если есть</param>
  /// <returns></returns>
  [Obsolete("Следует использовать функцию без параметра \"int cryptoSignObjectTypeID\"", false)]
  public static SignStatuses TranslateStatus(
    IUserSession session,
    long objectID,
    long signObjectID,
    int signObjectType,
    DateTime objectModifyDate,
    DateTime signDate,
    int cryptoSignObjectTypeID,
    out X509Certificate2Collection certificates)
  {
    certificates = (X509Certificate2Collection) null;
    ISignsService customService = session.GetCustomService(typeof (ISignsService)) as ISignsService;
    byte[] rawData = (byte[]) null;
    long objectID1 = objectID;
    long signObjectID1 = signObjectID;
    Guid sessionGuid = session.SessionGUID;
    ref byte[] local = ref rawData;
    bool flag = customService.CheckHashCode(objectID1, signObjectID1, sessionGuid, out local);
    if (rawData != null)
    {
      certificates = new X509Certificate2Collection();
      certificates.Import(rawData);
    }
    if (signObjectType.Equals(cryptoSignObjectTypeID))
      return !flag ? SignStatuses.CryptoSignOutOfDate : SignStatuses.CryptoSignActual;
    if (!flag)
      return SignStatuses.SignIncorrect;
    return !DateTimeHelper.EqualsTruncateToSeconds(objectModifyDate, signDate) ? SignStatuses.SignOutOfDate : SignStatuses.SignActual;
  }

  /// <summary>Статус подписи</summary>
  /// <param name="session">Пользовательская сессия</param>
  /// <param name="objectID">Идентификатор версии подписываемого объекта</param>
  /// <param name="signObjectID">Идентификатор версии подписи</param>
  /// <param name="signObjectType">Идентификатор типа подписи</param>
  /// <param name="objectModifyDate">Дата модификации подписываемого объекта (атрибут "Дата модификации содержимого объекта" от подписанного объекта): для внутренней подписи; для криптоподписей не имеет значения</param>
  /// <param name="signDate">Дата подписи (атрибут "Дата модификации содержимого объекта" от объекта подписи): для внутренней подписи; для криптоподписей не имеет значения</param>
  /// <returns></returns>
  public static SignStatuses TranslateStatus(
    IUserSession session,
    long objectID,
    long signObjectID,
    int signObjectType,
    DateTime objectModifyDate,
    DateTime signDate)
  {
    return SignHelper.TranslateStatus(session, objectID, signObjectID, signObjectType, objectModifyDate, signDate, SignHelper.GetCryptoSignObjectTypeId(session));
  }

  /// <summary>Статус подписи</summary>
  /// <param name="session">Пользовательская сессия</param>
  /// <param name="objectID">Идентификатор версии подписываемого объекта</param>
  /// <param name="signObjectID">Идентификатор версии подписи</param>
  /// <param name="signObjectType">Идентификатор типа подписи</param>
  /// <param name="objectModifyDate">Дата модификации подписываемого объекта (атрибут "Дата модификации содержимого объекта" от подписанного объекта): для внутренней подписи; для криптоподписей не имеет значения</param>
  /// <param name="signDate">Дата подписи (атрибут "Дата модификации содержимого объекта" от объекта подписи): для внутренней подписи; для криптоподписей не имеет значения</param>
  /// <param name="cryptoSignObjectTypeID">Идентификатор типа объектов "Подпись с криптозащитой"</param>
  /// <returns></returns>
  [Obsolete("Следует использовать функцию без параметра \"int cryptoSignObjectTypeID\"", false)]
  public static SignStatuses TranslateStatus(
    IUserSession session,
    long objectID,
    long signObjectID,
    int signObjectType,
    DateTime objectModifyDate,
    DateTime signDate,
    int cryptoSignObjectTypeID)
  {
    X509Certificate2Collection certificates = (X509Certificate2Collection) null;
    return SignHelper.TranslateStatus(session, objectID, signObjectID, signObjectType, objectModifyDate, signDate, cryptoSignObjectTypeID, out certificates);
  }
}
