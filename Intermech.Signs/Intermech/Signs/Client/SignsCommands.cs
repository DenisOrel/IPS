// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.SignsCommands
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Localization;
using Intermech.Protection;
using Intermech.Signs.CryptoAPI;
using Intermech.Signs.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Signs.Client;

/// <summary>Реализация команд подписания</summary>
public class SignsCommands
{
  /// <summary>Команда "Подписать"</summary>
  /// <param name="typedObjectIDs">Подписываемые объекты</param>
  public static bool SignUpCommand(List<IDBTypedObjectID> typedObjectIDs)
  {
    SignCollection infoForSigning = new SignCollection();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      infoForSigning.UserID = sessionKeeper.Session.UserID;
      UserRankInformation[] rankInfo;
      string resolutionString;
      if (SignsCache.Sign(typedObjectIDs, SignsCache.UserSignsCard, out rankInfo, out resolutionString))
      {
        infoForSigning.Resolution = resolutionString;
        foreach (IDBTypedObjectID typedObjectId in typedObjectIDs)
        {
          if (!infoForSigning.ListOfIDs.Contains(typedObjectId.ObjectID))
            infoForSigning.ListOfIDs.Add(typedObjectId.ObjectID);
        }
        ISignsService customService = sessionKeeper.Session.GetCustomService(typeof (ISignsService)) as ISignsService;
        SignedInfo info = new SignedInfo();
        foreach (UserRankInformation userRankInformation in rankInfo)
        {
          infoForSigning.RankID = userRankInformation.RankID;
          infoForSigning.ListOfGraphs = userRankInformation.Graphs;
          SignsCommands.AddRelationsInfo(customService.Sign(infoForSigning, sessionKeeper.Session.SessionGUID, SignsHolder.SignObjectTypeID), sessionKeeper.Session, info);
        }
        (ServicesManager.GetService(typeof (INotificationService)) as INotificationService).FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsCreated", (IList<long>) info.RelationIDs, (IList<long>) info.ProjIDs, (IList<int>) null, (IList<int>) info.RelTypeIDs));
        return true;
      }
    }
    return false;
  }

  /// <summary>Команда "Подписать как"</summary>
  /// <param name="typedObjectIDs"></param>
  public static void SignAsCommand(List<IDBTypedObjectID> typedObjectIDs)
  {
    SignCollection infoForSigning = new SignCollection();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      using (SelectUser selectUser = new SelectUser())
      {
        if (!selectUser.ShowDialog().Equals((object) DialogResult.OK))
          return;
        infoForSigning.UserName = selectUser.UserName;
        infoForSigning.Password = selectUser.Password;
      }
      infoForSigning.UserID = SignsCache.GetUserIdByUserName(infoForSigning.UserName);
      RoleProperties[] rolesList = sessionKeeper.Session.GetRolesList(infoForSigning.UserID);
      if (rolesList.Length == 0)
        throw new Exception($"Пользователь {infoForSigning.UserName} должен иметь хотя бы одну назначенную роль");
      IMServerService service = ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService;
      IUserSession session = service.ServerObject.CreateSession();
      session.Login(infoForSigning.UserName, new PswPackage(infoForSigning.Password, service.ServerObject.CryptMethod), sessionKeeper.Session.ComputerName, sessionKeeper.Session.TimeZoneOffset, rolesList[0].RoleID, "SignAsSession");
      try
      {
        SignsCard card = SignsCache.LoadUserGraphInfo(infoForSigning);
        UserRankInformation[] rankInfo;
        string resolutionString;
        if (!SignsCache.Sign(typedObjectIDs, card, out rankInfo, out resolutionString))
          return;
        foreach (IDBTypedObjectID typedObjectId in typedObjectIDs)
        {
          if (!infoForSigning.ListOfIDs.Contains(typedObjectId.ObjectID))
            infoForSigning.ListOfIDs.Add(typedObjectId.ObjectID);
        }
        infoForSigning.Resolution = resolutionString;
        SignedInfo info = new SignedInfo();
        ISignsService customService = sessionKeeper.Session.GetCustomService(typeof (ISignsService)) as ISignsService;
        string password = infoForSigning.Password;
        infoForSigning.Password = string.Empty;
        try
        {
          foreach (UserRankInformation userRankInformation in rankInfo)
          {
            infoForSigning.RankID = userRankInformation.RankID;
            infoForSigning.ListOfGraphs = userRankInformation.Graphs;
            SignsCommands.AddRelationsInfo(customService.SignAs(infoForSigning, session.SessionGUID), session, info);
          }
        }
        finally
        {
          infoForSigning.Password = password;
        }
        (ServicesManager.GetService(typeof (INotificationService)) as INotificationService).FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsCreated", (IList<long>) info.RelationIDs, (IList<long>) info.ProjIDs, (IList<int>) null, (IList<int>) info.RelTypeIDs));
      }
      finally
      {
        session.Logout("SignAsSession");
      }
    }
  }

  /// <summary>Команда "ЭЦП"</summary>
  /// <param name="typedObjectIDs"></param>
  public static void CryptoSignUp(List<IDBTypedObjectID> typedObjectIDs)
  {
    UserRankInformation[] rankInfo = (UserRankInformation[]) null;
    SignCollection infoForSigning = new SignCollection();
    string resolutionString;
    if (!SignsCache.Sign(typedObjectIDs, SignsCache.UserSignsCard, out rankInfo, out resolutionString))
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IUserSession session = sessionKeeper.Session;
      ISignsService customService = session.GetCustomService(typeof (ISignsService)) as ISignsService;
      infoForSigning.UserID = session.UserID;
      infoForSigning.Resolution = resolutionString;
      OpenKeysCollection collection = new OpenKeysCollection();
      if (!SignsHolder.CertificateSigningOnlyMode)
      {
        IDBAttribute objectAttributeById = session.GetObjectAttributeByID(session.UserID, SignsHolder.OpenKeysAttrTypeID);
        for (int index = 0; index < objectAttributeById.ValuesCount; ++index)
        {
          objectAttributeById.Index = index;
          string asString = objectAttributeById.AsString;
          if (!asString.Equals(string.Empty))
            collection.Add((object) new OpenKey(asString));
        }
      }
      OpenKey openKey = (OpenKey) null;
      X509Certificate2 x509Certificate2 = (X509Certificate2) null;
      if (collection.Count > 0 && !SignsHolder.CertificateSigningOnlyMode)
      {
        using (SelectOpenKey selectOpenKey = new SelectOpenKey(collection))
        {
          if (!selectOpenKey.ShowDialog().Equals((object) DialogResult.OK))
            return;
          switch (selectOpenKey.ValueType)
          {
            case SelectOpenKeyValueType.OpenKey:
              openKey = selectOpenKey.Value as OpenKey;
              break;
            case SelectOpenKeyValueType.Certificate:
              x509Certificate2 = selectOpenKey.Value as X509Certificate2;
              break;
          }
        }
      }
      else
      {
        X509Certificate2Collection possibleCertificates = CertProcs.GetPossibleCertificates(session);
        if (possibleCertificates != null && possibleCertificates.Count > 0)
        {
          x509Certificate2 = CertProcs.SelectCertificate(possibleCertificates, LocalizationHolder.rm.GetString("Signs_86"), LocalizationHolder.rm.GetString("Signs_87"));
          if (x509Certificate2 == null)
            return;
          CertProcs.GetCertInfo(x509Certificate2, true);
        }
        else
        {
          int num = (int) MessageBox.Show(LocalizationHolder.rm.GetString("NoCertsFound"), MessageDialogs.msgInformation, MessageBoxButtons.OK);
          return;
        }
      }
      SignedInfo info = new SignedInfo();
      foreach (UserRankInformation userRankInformation in rankInfo)
      {
        infoForSigning.RankID = userRankInformation.RankID;
        infoForSigning.ListOfGraphs = userRankInformation.Graphs;
        foreach (IDBTypedObjectID typedObjectId in typedObjectIDs)
        {
          infoForSigning.ListOfIDs.Clear();
          infoForSigning.ListOfIDs.Add(typedObjectId.ObjectID);
          byte[] pkcs7 = (byte[]) null;
          IDBObject dbObject1 = session.GetObject(typedObjectId.ObjectID);
          int num1 = dbObject1.GetHashVersion();
          int num2 = num1;
          HashContent hashContent = new HashContent();
          AttributeValues attributeValues = new AttributeValues(SignsHolder.OpenKeysAttrTypeID);
          if (HashProcs.SimpleVersion(num1) < 4)
          {
            byte[] objectHashData = SignsCommands.GetObjectHashData(dbObject1, num1, x509Certificate2, true, (IHashContent) hashContent);
            if (openKey != null)
            {
              IDBObject dbObject2 = session.GetObject(openKey.ProviderGuid);
              string asString = dbObject2.GetAttributeByGuid(SignsHolder.NaimAttrTypeGuid).AsString;
              int int32_1 = Convert.ToInt32(dbObject2.GetAttributeByGuid(SignsHolder.CryptoTypeAttrTypeGuid).AsInteger);
              int int32_2 = Convert.ToInt32(dbObject2.GetAttributeByGuid(SignsHolder.CryptoAlgIDAttrTypeGuid).AsInteger);
              using (CapiProvider capiProvider = new CapiProvider(new CspParameters(int32_1, asString, openKey.ConteinerName)
              {
                KeyNumber = openKey.KeyType == OpenKeyType.Extended ? 2 : 1,
                Flags = CspProviderFlags.UseExistingKey
              }))
                pkcs7 = capiProvider.SignObjectHash(objectHashData, int32_2);
              openKey.KeyType = OpenKeyType.Extended;
              attributeValues.Values = new object[1]
              {
                (object) openKey.ToString()
              };
            }
            else
            {
              if (x509Certificate2 == null)
                return;
              X509ChainStatus[] verifyChainResult = (X509ChainStatus[]) null;
              SignResult signResult = SignsProcs.SignMsg(objectHashData, x509Certificate2, out pkcs7, SignsHolder.DoRevocation, out verifyChainResult);
              if (signResult != SignResult.OK)
              {
                if (!SignsHolder.DoRevocation || signResult != SignResult.NotVerified)
                  return;
                foreach (X509ChainStatus x509ChainStatus in verifyChainResult)
                  session.EventLog.AddToTrace($"{x509ChainStatus.StatusInformation} ({x509Certificate2.Subject}; {x509Certificate2.Issuer})", Consts.traceAlways, string.Empty);
                return;
              }
              attributeValues.Values = new object[1]
              {
                (object) string.Empty
              };
            }
          }
          else
          {
            string tempFileName1 = ClientContext.FileVault.TempArea.GetTempFileName();
            string tempFileName2 = ClientContext.FileVault.TempArea.GetTempFileName();
            try
            {
              using (FileStream fileStream = new FileStream(tempFileName1, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
              {
                num1 = HashProcs.ExtractSignInfo((Stream) fileStream, dbObject1, num1, true, (IHashContent) hashContent);
                if (x509Certificate2 == null)
                  return;
                StreamCms streamCms = new StreamCms();
                try
                {
                  fileStream.Position = 0L;
                  using (FileStream outFile = new FileStream(tempFileName2, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                    streamCms.Encode(x509Certificate2, (Stream) fileStream, (Stream) outFile, true);
                  using (FileStream input = new FileStream(tempFileName2, FileMode.Open, FileAccess.Read, FileShare.None))
                  {
                    using (BinaryReader binaryReader = new BinaryReader((Stream) input))
                      pkcs7 = binaryReader.ReadBytes((int) input.Length);
                  }
                }
                catch (Exception ex)
                {
                  session.EventLog.AddToTrace($"{ex.Message} ({x509Certificate2.Subject}; {x509Certificate2.Issuer})", Consts.traceAlways, string.Empty);
                  return;
                }
                attributeValues.Values = new object[1]
                {
                  (object) string.Empty
                };
              }
            }
            finally
            {
              try
              {
                File.Delete(tempFileName1);
              }
              catch
              {
              }
              try
              {
                File.Delete(tempFileName2);
              }
              catch
              {
              }
            }
          }
          Dictionary<long, List<long>> dict = customService.Sign(infoForSigning, session.SessionGUID, SignsHolder.CryptoSignObjectTypeID);
          SignsCommands.AddRelationsInfo(dict, sessionKeeper.Session, info);
          using (MemoryStream memoryStream1 = new MemoryStream(pkcs7))
          {
            using (MemoryStream memoryStream2 = new MemoryStream())
            {
              hashContent.Save((Stream) memoryStream2);
              foreach (long objectID in dict[typedObjectId.ObjectID])
              {
                DateTime now = DateTime.Now;
                IDBObject dbObject3 = session.GetObject(objectID);
                if (num1 != num2)
                  dbObject3.GetAttributeByID(SignsHolder.SignVersionAttrTypeID).Value = (object) num1;
                IDBAttribute attributeById1 = dbObject3.GetAttributeByID(SignsHolder.EDSAttrTypeID);
                BlobInformation blobInformation = new BlobInformation(memoryStream1.Length, 0L, now, "ElectronicDigitalSign", ArcMethods.ZLibPacked, string.Empty);
                BlobInformation aBlobInformation1 = blobInformation;
                MemoryStream aSourceStream1 = memoryStream1;
                new BlobProcWriter(attributeById1, 0, aBlobInformation1, (Stream) aSourceStream1, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).WriteData();
                IDBAttribute attributeById2 = dbObject3.GetAttributeByID(SignsHolder.SignDataSequenceTypeID);
                blobInformation = new BlobInformation(memoryStream2.Length, 0L, now, "ElectronicDigitalHashContent", ArcMethods.ZLibPacked, string.Empty);
                BlobInformation aBlobInformation2 = blobInformation;
                MemoryStream aSourceStream2 = memoryStream2;
                new BlobProcWriter(attributeById2, 0, aBlobInformation2, (Stream) aSourceStream2, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).WriteData();
                if (HashProcs.SimpleVersion(num1) < 4)
                  dbObject3.SetAttributesValues(new AttributeValues[1]
                  {
                    attributeValues
                  });
              }
            }
          }
        }
      }
      (ServicesManager.GetService(typeof (INotificationService)) as INotificationService).FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsCreated", (IList<long>) info.RelationIDs, (IList<long>) info.ProjIDs, (IList<int>) null, (IList<int>) info.RelTypeIDs));
    }
  }

  private static void AddRelationsInfo(
    Dictionary<long, List<long>> dict,
    IUserSession session,
    SignedInfo info)
  {
    foreach (KeyValuePair<long, List<long>> keyValuePair in dict)
    {
      foreach (long partID in keyValuePair.Value)
      {
        IDBRelation relation = session.GetRelation(keyValuePair.Key, partID, SignsHolder.SignRelationTypeID, true);
        if (relation != null)
        {
          info.RelationIDs.Add(relation.RelationID);
          info.ProjIDs.Add(keyValuePair.Key);
          info.RelTypeIDs.Add(SignsHolder.SignRelationTypeID);
        }
      }
    }
  }

  /// <summary>
  /// Получения данных для подписывания : старая функция для криптоподписей ниже HashVersionsCrypto.Version4
  /// Используется только для поддержки проверки старых версий подписи.
  /// </summary>
  /// <param name="dbObject">Объект для получения данных</param>
  /// <param name="versionID">Версия подписи</param>
  /// <returns></returns>
  private static byte[] GetObjectHashData(
    IDBObject dbObject,
    int versionID,
    X509Certificate2 certificate,
    bool setContent,
    IHashContent hashContent)
  {
    X509Certificate2 certificate1 = new X509Certificate2(certificate.Export(X509ContentType.Cert));
    return new UnicodeEncoding().GetBytes(dbObject.GetHashFile(versionID, certificate1, setContent, hashContent));
  }
}
