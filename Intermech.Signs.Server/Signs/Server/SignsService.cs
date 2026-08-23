// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Server.SignsService
// Assembly: Intermech.Signs.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3ABC2C25-9F6B-4AA7-A176-FCB28F816B8C
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Signs.Server.dll

using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Interfaces.Briefcase;
using Intermech.Interfaces.Server;
using Intermech.Kernel;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Remoting.Sponsors;
using Intermech.Signs.CryptoAPI;
using Intermech.Signs.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;

#nullable disable
namespace Intermech.Signs.Server;

internal class SignsService : LongLifeObject, ISignsService
{
  public bool CheckHashCode(long objectID, long signObjectID, Guid sessionGuid)
  {
    byte[] certificatesRawData = (byte[]) null;
    return this.CheckHashCode(objectID, signObjectID, sessionGuid, out certificatesRawData);
  }

  public bool CheckHashCode(
    long objectID,
    long signObjectID,
    Guid sessionGuid,
    out byte[] certificatesRawData)
  {
    certificatesRawData = (byte[]) null;
    IUserSession sessionById = UserSession.GetSessionByID(sessionGuid);
    IDBObject signObject = sessionById.GetObject(signObjectID);
    if (signObject.ObjectType == SignsHolder.SignObjectTypeID)
    {
      byte[] hash2 = HashPack.CalcHash(signObject);
      IDBAttribute attributeById = signObject.GetAttributeByID(SignsHolder.HashProtectionAttrTypeID);
      if (attributeById != null && attributeById.AsString != string.Empty)
        return HashPack.CompareHash(Convert.FromBase64String(attributeById.AsString), hash2);
    }
    else
    {
      IDBAttribute attributeById1 = signObject.GetAttributeByID(SignsHolder.EDSAttrTypeID);
      if (attributeById1 is IBlobReader blobReader && blobReader.OpenBlob(-1).RealFileSize > 0L)
      {
        byte[] numArray = (byte[]) null;
        using (MemoryStream aDestStream = new MemoryStream())
        {
          new BlobProcReader(attributeById1, 0, (Stream) aDestStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).ReadData(sessionById);
          aDestStream.Position = 0L;
          numArray = aDestStream.ToArray();
        }
        if (numArray != null)
        {
          IDBObject idbObject = sessionById.GetObject(objectID);
          int int32_1 = Convert.ToInt32(signObject.GetAttributeByID(SignsHolder.SignVersionAttrTypeID).AsInteger);
          if (HashProcs.SimpleVersion(int32_1) < 4)
          {
            byte[] bytes = new UnicodeEncoding().GetBytes(idbObject.GetHashFile(int32_1, (X509Certificate2) null, false, (IHashContent) null));
            OpenKeysCollection openKeysCollection = new OpenKeysCollection();
            foreach (object obj in signObject.GetAttributeByID(SignsHolder.OpenKeysAttrTypeID).Values)
            {
              if (obj != null && obj.GetType().Equals(typeof (string)) && !string.IsNullOrEmpty(obj.ToString()))
                openKeysCollection.Add((object) new OpenKey(Convert.ToString(obj)));
            }
            if (openKeysCollection.Count == 0)
            {
              string errorMessage = string.Empty;
              X509Certificate2Collection certificates = (X509Certificate2Collection) null;
              int num = (int) SignsProcs.VerifyMsg(numArray, bytes, SignsHolder.DoRevocation, out certificates, out errorMessage);
              if (certificates != null)
                certificatesRawData = certificates.Export(X509ContentType.SerializedCert);
              if (num == 1 && errorMessage != string.Empty)
                sessionById.EventLog.AddToTrace(errorMessage, Consts.traceAlways, string.Empty);
              return num == 0;
            }
            foreach (OpenKey openKey in (ArrayList) openKeysCollection)
            {
              IDBObject dbObject = sessionById.GetObject(openKey.ProviderGuid);
              string asString = dbObject.GetAttributeByGuid(SignsHolder.NaimAttrTypeGuid).AsString;
              int int32_2 = Convert.ToInt32(dbObject.GetAttributeByGuid(SignsHolder.CryptoTypeAttrTypeGuid).AsInteger);
              int int32_3 = Convert.ToInt32(dbObject.GetAttributeByGuid(SignsHolder.CryptoAlgIDAttrTypeGuid).AsInteger);
              CspParameters csp = new CspParameters(int32_2, asString, string.Empty);
              csp.Flags = CspProviderFlags.UseExistingKey;
              bool flag = true;
              using (CapiProvider capiProvider = new CapiProvider(csp))
              {
                if (openKey.KeyType == OpenKeyType.Simple)
                  Array.Reverse((Array) numArray);
                flag = capiProvider.VerifyObjectSign(numArray, bytes, Convert.FromBase64String(openKey.Key), int32_3);
              }
              if (flag)
                return true;
            }
          }
          else
          {
            SignResult signResult = SignResult.NotVerified;
            string EventStr = string.Empty;
            SignedCms signedCms = new SignedCms();
            signedCms.Decode(numArray);
            X509Certificate2Collection certificates = signedCms.Certificates;
            if (certificates != null)
              certificatesRawData = certificates.Export(X509ContentType.SerializedCert);
            if (SignsHolder.DoRevocation)
            {
              if (certificatesRawData == null)
                throw new KernelException(string.Format(LocalizationHolder.rm.GetString("CertNotValid"), (object) ""));
              X509Certificate2Collection certificate2Collection = new X509Certificate2Collection();
              certificate2Collection.Import(certificatesRawData);
              X509Certificate2 cert = certificate2Collection.Count > 0 ? certificate2Collection[0] : throw new KernelException(string.Format(LocalizationHolder.rm.GetString("CertNotValid"), (object) ""));
              X509ChainStatus[] chStatus = (X509ChainStatus[]) null;
              if (!CertProcs.GetX509VerifyResultsV4(cert, true, out chStatus))
              {
                foreach (X509ChainStatus x509ChainStatus in chStatus)
                  sessionById.EventLog.AddToTrace($"{x509ChainStatus.StatusInformation} ({cert.Subject}; {cert.Issuer})", Consts.traceAlways, string.Empty);
                return false;
              }
            }
            IHashContent hashContent = (IHashContent) new HashContent();
            IDBAttribute attributeById2 = signObject.GetAttributeByID(SignsHolder.SignDataSequenceTypeID);
            if (attributeById2 == null)
            {
              IDBAttributeType attributeType = sessionById.GetAttributeType(SignsHolder.SignDataSequenceTypeID);
              throw new KernelException(string.Format(LocalizationHolder.rm.GetString("Signs.Server_8"), (object) attributeType.Name, (object) idbObject.NameInMessages));
            }
            using (MemoryStream aDestStream = new MemoryStream())
            {
              new BlobProcReader(attributeById2, 0, (Stream) aDestStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).ReadData(sessionById);
              hashContent.Load((Stream) aDestStream);
            }
            TemporaryStorage temporaryStorage = new TemporaryStorage();
            using (new RemoteLock((object) idbObject))
            {
              string fullFileName = temporaryStorage.GetFullFileName(Guid.NewGuid().ToString());
              try
              {
                using (Stream stream = (Stream) new FileStream(fullFileName, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                  using (MemoryStream signFile = new MemoryStream(numArray))
                  {
                    using (MemoryStream outFile = new MemoryStream())
                    {
                      HashProcs.ExtractSignInfo(stream, idbObject, int32_1, false, hashContent);
                      StreamCms streamCms = new StreamCms();
                      try
                      {
                        stream.Position = 0L;
                        streamCms.Decode(stream, (Stream) signFile, (Stream) outFile, true, out X509Certificate2 _);
                        signResult = SignResult.OK;
                      }
                      catch (Exception ex)
                      {
                        EventStr = string.Format(LocalizationHolder.rm.GetString("SignVerifyResult"), (object) idbObject.Caption, (object) ex.Message);
                      }
                    }
                  }
                }
              }
              finally
              {
                try
                {
                  File.Delete(fullFileName);
                }
                catch
                {
                }
              }
            }
            if (signResult == SignResult.NotVerified && EventStr != string.Empty)
              sessionById.EventLog.AddToTrace(EventStr, Consts.traceAlways, string.Empty);
            return signResult == SignResult.OK;
          }
        }
      }
    }
    return false;
  }

  public string PatchSignGraphsForAllArchives(
    Dictionary<string, string> substitutes,
    Guid sessionGuid)
  {
    IUserSession sessionById = UserSession.GetSessionByID(sessionGuid);
    List<long> longList = sessionById.IsAdmin ? this.GetArchivesIds(sessionById) : throw new Exception(LocalizationHolder.rm.GetString("NotAdminError"));
    int num = 0;
    StringBuilder stringBuilder = new StringBuilder(string.Empty);
    foreach (long archiveId in longList)
    {
      int substitutesNumber = 0;
      string str = this.ReplaceGraphsForArchive(archiveId, substitutes, ref substitutesNumber, sessionById);
      if (string.IsNullOrEmpty(str))
        num += substitutesNumber;
      else
        stringBuilder.AppendLine(str);
    }
    string str1 = string.Format(LocalizationHolder.rm.GetString("ArchivePatchResult"), (object) num);
    if (!string.IsNullOrEmpty(stringBuilder.ToString()))
      str1 = str1 + LocalizationHolder.rm.GetString("PatchError") + stringBuilder.ToString();
    return str1;
  }

  private string ReplaceGraphsForArchive(
    long archiveId,
    Dictionary<string, string> substitutes,
    ref int substitutesNumber,
    IUserSession session)
  {
    try
    {
      IDBAttribute attributeById = session.GetObject(archiveId).GetAttributeByID(SignsHolder.SignsSetupAttrTypeID);
      if (attributeById == null)
        return string.Empty;
      GraphsSet graphsSet = new GraphsSet();
      using (MemoryStream memoryStream = new MemoryStream())
      {
        new BlobProcReader(attributeById, 0, (Stream) memoryStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).ReadData(session);
        if (memoryStream.Length > 0L)
          graphsSet = GraphsSet.Load((Stream) memoryStream);
      }
      substitutesNumber = graphsSet.DoSubstitutes(substitutes);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        graphsSet.Save((Stream) memoryStream);
        BlobInformation aBlobInformation = new BlobInformation(memoryStream.Length, 0L, DateTime.Now, "signs.xml", ArcMethods.ZLibPacked, string.Empty);
        new BlobProcWriter(attributeById, 0, aBlobInformation, (Stream) memoryStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).WriteData(session);
      }
    }
    catch (Exception ex)
    {
      return string.Format(LocalizationHolder.rm.GetString("ArchivePatchError"), (object) archiveId);
    }
    return string.Empty;
  }

  private List<long> GetArchivesIds(IUserSession session)
  {
    DataTable dataTable = session.GetObjectCollection(new Guid("cad0011e-306c-11d8-b4e9-00304f19f545")).Select(new DBRecordSetParams((ConditionStructure[]) null, new ColumnDescriptor[1]
    {
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0)
    }));
    List<long> collection = new List<long>();
    if (dataTable != null && dataTable.Rows.Count > 0)
    {
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        long int64 = Convert.ToInt64(row[0]);
        collection.SafeAdd<long>(int64);
      }
    }
    return collection;
  }

  public string PatchSignGraphsForLCStepsAndLCLevels(
    Dictionary<string, string> substitutes,
    Guid sessionGuid)
  {
    IUserSession sessionById = UserSession.GetSessionByID(sessionGuid);
    IContainerService containerService = sessionById.IsAdmin ? sessionById.GetCustomService(typeof (IContainerService)) as IContainerService : throw new Exception(LocalizationHolder.rm.GetString("NotAdminError"));
    int commonSubstitutesNumber = 0;
    string str1 = string.Empty + this.PatchLCLevels(substitutes, sessionById, containerService, ref commonSubstitutesNumber) + this.PatchLCSchemas(substitutes, sessionById, containerService, ref commonSubstitutesNumber) + this.PatchLCSchemasForObjectTypes(substitutes, sessionById, containerService, ref commonSubstitutesNumber);
    string str2 = string.Format(LocalizationHolder.rm.GetString("LCStepAndLevelPatchResult"), (object) commonSubstitutesNumber);
    if (!string.IsNullOrEmpty(str1))
      str2 = str2 + LocalizationHolder.rm.GetString("PatchError") + str1;
    return str2;
  }

  private string PatchLCSchemasForObjectTypes(
    Dictionary<string, string> substitutes,
    IUserSession session,
    IContainerService containerService,
    ref int commonSubstitutesNumber)
  {
    List<IMSObjectType> objectTypesList = MetaDataHelper.GetObjectTypesList();
    StringBuilder stringBuilder = new StringBuilder();
    foreach (IMSObjectType objectType in objectTypesList)
    {
      int schemaId = objectType.SchemaID;
      DataTable table = session.GetLifecycleStepCollection(schemaId, objectType.ObjectTypeID).GetSchema().Tables["IMS_LC_STEPS"];
      List<int> intList = new List<int>();
      if (table != null && table.Rows.Count > 0)
      {
        foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
          intList.Add(Convert.ToInt32(row["F_LC_STEP"]));
      }
      foreach (int stepId in intList)
      {
        int substitutesNumber = 0;
        string str = this.ReplaceGraphsForStepsForObjectTypes(stepId, objectType, containerService, substitutes, ref substitutesNumber, session);
        if (string.IsNullOrEmpty(str))
          commonSubstitutesNumber += substitutesNumber;
        else
          stringBuilder.AppendLine(str);
      }
    }
    return stringBuilder.ToString();
  }

  private string PatchLCSchemas(
    Dictionary<string, string> substitutes,
    IUserSession session,
    IContainerService containerService,
    ref int commonSubstitutesNumber)
  {
    DataTable dataTable = (session.GetLCSchemaCollection() as IDBCollection).Select(string.Empty);
    List<int> intList1 = new List<int>();
    if (dataTable != null && dataTable.Rows.Count > 0)
    {
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        int int32 = Convert.ToInt32(row["F_SCHEMA_ID"]);
        intList1.Add(int32);
      }
    }
    StringBuilder stringBuilder = new StringBuilder();
    foreach (int schemaID in intList1)
    {
      DataTable table = session.GetLCSchema(schemaID).GetStepsCollection().GetSchema().Tables["IMS_LC_STEPS"];
      List<int> intList2 = new List<int>();
      if (table != null && table.Rows.Count > 0)
      {
        foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
          intList2.Add(Convert.ToInt32(row["F_LC_STEP"]));
      }
      foreach (int stepId in intList2)
      {
        int substitutesNumber = 0;
        string str = this.ReplaceGraphsForSteps(stepId, containerService, substitutes, ref substitutesNumber, session);
        if (string.IsNullOrEmpty(str))
          commonSubstitutesNumber += substitutesNumber;
        else
          stringBuilder.AppendLine(str);
      }
    }
    return stringBuilder.ToString();
  }

  private string PatchLCLevels(
    Dictionary<string, string> substitutes,
    IUserSession session,
    IContainerService containerService,
    ref int commonSubstitutesNumber)
  {
    List<int> lcLevelIds = this.GetLCLevelIDs(session);
    StringBuilder stringBuilder = new StringBuilder();
    foreach (int levelId in lcLevelIds)
    {
      int substitutesNumber = 0;
      string str = this.ReplaceGraphsForLCLevels(levelId, containerService, substitutes, ref substitutesNumber, session);
      if (string.IsNullOrEmpty(str))
        commonSubstitutesNumber += substitutesNumber;
      else
        stringBuilder.AppendLine(str);
    }
    return stringBuilder.ToString();
  }

  private string ReplaceGraphsForSteps(
    int stepId,
    IContainerService containerService,
    Dictionary<string, string> substitutes,
    ref int substitutesNumber,
    IUserSession session)
  {
    try
    {
      IDBObject containerForLcStep = containerService.GetContainerForLCStep((object) session.SessionGUID, stepId);
      if (containerForLcStep == null)
        return string.Empty;
      IDBAttribute attributeById = containerForLcStep.GetAttributeByID(SignsHolder.SignsSetupAttrTypeID);
      if (attributeById == null)
        return string.Empty;
      GraphsSet graphsSet = new GraphsSet();
      using (MemoryStream memoryStream = new MemoryStream())
      {
        new BlobProcReader(attributeById, 0, (Stream) memoryStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).ReadData(session);
        if (memoryStream.Length > 0L)
          graphsSet = GraphsSet.Load((Stream) memoryStream);
      }
      substitutesNumber = graphsSet.DoSubstitutes(substitutes);
      if (substitutesNumber == 0)
        return string.Empty;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        graphsSet.Save((Stream) memoryStream);
        BlobInformation aBlobInformation = new BlobInformation(memoryStream.Length, 0L, DateTime.Now, "sings.xml", ArcMethods.ZLibPacked, string.Empty);
        new BlobProcWriter(attributeById, 0, aBlobInformation, (Stream) memoryStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).WriteData(session);
      }
    }
    catch (Exception ex)
    {
      return string.Format(LocalizationHolder.rm.GetString("LCStepPatchError"), (object) stepId);
    }
    return string.Empty;
  }

  private string ReplaceGraphsForStepsForObjectTypes(
    int stepId,
    IMSObjectType objectType,
    IContainerService containerService,
    Dictionary<string, string> substitutes,
    ref int substitutesNumber,
    IUserSession session)
  {
    try
    {
      IDBObject lcStepObjectType = containerService.GetContainerForLCStepObjectType((object) session.SessionGUID, stepId, objectType.ObjectTypeID);
      if (lcStepObjectType == null)
        return string.Empty;
      IDBAttribute attributeById = lcStepObjectType.GetAttributeByID(SignsHolder.SignsSetupAttrTypeID);
      if (attributeById == null)
        return string.Empty;
      GraphsSet graphsSet = new GraphsSet();
      using (MemoryStream memoryStream = new MemoryStream())
      {
        new BlobProcReader(attributeById, 0, (Stream) memoryStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).ReadData(session);
        if (memoryStream.Length > 0L)
          graphsSet = GraphsSet.Load((Stream) memoryStream);
      }
      substitutesNumber = graphsSet.DoSubstitutes(substitutes);
      if (substitutesNumber == 0)
        return string.Empty;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        graphsSet.Save((Stream) memoryStream);
        BlobInformation aBlobInformation = new BlobInformation(memoryStream.Length, 0L, DateTime.Now, "sings.xml", ArcMethods.ZLibPacked, string.Empty);
        new BlobProcWriter(attributeById, 0, aBlobInformation, (Stream) memoryStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).WriteData(session);
      }
    }
    catch (Exception ex)
    {
      return string.Format(LocalizationHolder.rm.GetString("LCStepPatchError"), (object) stepId, (object) objectType.ObjectName);
    }
    return string.Empty;
  }

  private string ReplaceGraphsForLCLevels(
    int levelId,
    IContainerService containerService,
    Dictionary<string, string> substitutes,
    ref int substitutesNumber,
    IUserSession session)
  {
    try
    {
      IDBObject containerForLcLevel = containerService.GetContainerForLCLevel((object) session.SessionGUID, levelId);
      if (containerForLcLevel == null)
        return string.Empty;
      IDBAttribute attributeById = containerForLcLevel.GetAttributeByID(SignsHolder.SignsSetupAttrTypeID);
      if (attributeById == null)
        return string.Empty;
      GraphsSet graphsSet = new GraphsSet();
      using (MemoryStream memoryStream = new MemoryStream())
      {
        new BlobProcReader(attributeById, 0, (Stream) memoryStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).ReadData(session);
        if (memoryStream.Length > 0L)
          graphsSet = GraphsSet.Load((Stream) memoryStream);
      }
      substitutesNumber = graphsSet.DoSubstitutes(substitutes);
      if (substitutesNumber == 0)
        return string.Empty;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        graphsSet.Save((Stream) memoryStream);
        BlobInformation aBlobInformation = new BlobInformation(memoryStream.Length, 0L, DateTime.Now, "sings.xml", ArcMethods.ZLibPacked, string.Empty);
        new BlobProcWriter(attributeById, 0, aBlobInformation, (Stream) memoryStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).WriteData(session);
      }
    }
    catch (Exception ex)
    {
      return string.Format(LocalizationHolder.rm.GetString("LCLevelPatchError"), (object) levelId);
    }
    return string.Empty;
  }

  private List<int> GetLCLevelIDs(IUserSession session)
  {
    List<int> lcLevelIds = new List<int>();
    IDBLifecycleLevelCollection lifecycleLevelCollection = session.GetLifecycleLevelCollection();
    if (lifecycleLevelCollection != null)
    {
      DataTable dataTable = lifecycleLevelCollection.Select(string.Empty);
      if (dataTable != null)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        {
          int int32 = Convert.ToInt32(row["F_LEVEL_ID"]);
          lcLevelIds.Add(int32);
        }
      }
    }
    return lcLevelIds;
  }

  public string PatchSignGraphsForRanks(Dictionary<string, string> substitutes, Guid sessionGuid)
  {
    IUserSession sessionById = UserSession.GetSessionByID(sessionGuid);
    List<long> longList = sessionById.IsAdmin ? this.GetRanksIds(sessionById) : throw new Exception(LocalizationHolder.rm.GetString("NotAdminError"));
    int num = 0;
    StringBuilder stringBuilder = new StringBuilder();
    foreach (long rankId in longList)
    {
      int substitutesNumber = 0;
      string str = this.ReplaceGraphsForRank(rankId, substitutes, ref substitutesNumber, sessionById);
      if (string.IsNullOrWhiteSpace(str))
        num += substitutesNumber;
      else
        stringBuilder.AppendLine(str);
    }
    string str1 = string.Format(LocalizationHolder.rm.GetString("RanksPatchResult"), (object) num);
    if (!string.IsNullOrEmpty(stringBuilder.ToString()))
      str1 = str1 + LocalizationHolder.rm.GetString("PatchError") + stringBuilder.ToString();
    return str1;
  }

  private string ReplaceGraphsForRank(
    long rankId,
    Dictionary<string, string> substitutes,
    ref int substitutesNumber,
    IUserSession session)
  {
    try
    {
      IDBAttribute attributeById = session.GetObject(rankId).GetAttributeByID(SignsHolder.SignsSetupAttrTypeID);
      if (attributeById == null)
        return string.Empty;
      Graphs4Type graphs4Type = (Graphs4Type) null;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        new BlobProcReader(attributeById, 0, (Stream) memoryStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).ReadData(session);
        if (memoryStream.Length > 0L)
          graphs4Type = new Graphs4Type((Stream) memoryStream, SignsServerCache.PossibleGraphs);
      }
      if (graphs4Type == null)
        return string.Empty;
      substitutesNumber = graphs4Type.DoSubstitutes(substitutes);
      if (substitutesNumber == 0)
        return string.Empty;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        graphs4Type.Save((Stream) memoryStream, session);
        BlobInformation aBlobInformation = new BlobInformation(memoryStream.Length, 0L, DateTime.Now, "signs.xml", ArcMethods.ZLibPacked, string.Empty);
        new BlobProcWriter(attributeById, 0, aBlobInformation, (Stream) memoryStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).WriteData(session);
      }
    }
    catch (Exception ex)
    {
      return string.Format(LocalizationHolder.rm.GetString("RankPatchError"), (object) rankId);
    }
    return string.Empty;
  }

  private List<long> GetRanksIds(IUserSession session)
  {
    DataTable dataTable = session.GetObjectCollection(new Guid("cad00147-306c-11d8-b4e9-00304f19f545")).Select(new DBRecordSetParams((ConditionStructure[]) null, new ColumnDescriptor[1]
    {
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0)
    }));
    List<long> collection = new List<long>();
    if (dataTable != null && dataTable.Rows.Count > 0)
    {
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        long int64 = Convert.ToInt64(row[0]);
        collection.SafeAdd<long>(int64);
      }
    }
    return collection;
  }

  public bool CheckSignsEx(long[] objectIDs, Guid sessionGuid, bool useStrongCheck4All)
  {
    IUserSession sessionById = UserSession.GetSessionByID(sessionGuid);
    return this.CheckSignsEx(objectIDs, sessionGuid, sessionById.UserID, useStrongCheck4All);
  }

  public bool CheckSignsEx(
    long[] objectIDs,
    Guid sessionGuid,
    long userID,
    bool useStrongCheck4All)
  {
    IUserSession sessionById = UserSession.GetSessionByID(sessionGuid);
    bool flag = true;
    foreach (long objectId in objectIDs)
    {
      flag &= this.FastCheck4Object(objectId, sessionById, userID, useStrongCheck4All);
      if (!flag)
        break;
    }
    return flag;
  }

  private bool FastCheck4Object(
    long objectID,
    IUserSession session,
    long userID,
    bool strongCheck)
  {
    SignsProviderList signedGraphs = this.GetSignedGraphs(objectID, userID, session);
    if (!strongCheck)
      return signedGraphs.Count > 0;
    IDBAttribute attributeById = session.GetObject(objectID).GetAttributeByID(SignsHolder.ModifyDateAttrTypeID);
    if (attributeById != null)
    {
      DateTime asDateTime = attributeById.AsDateTime;
      foreach (SignsProvider signsProvider in signedGraphs)
      {
        if (DateTimeHelper.EqualsTruncateToSeconds(signsProvider.ModifyDate, asDateTime) && signsProvider.ErrorCode.Equals((object) SignsErrors.NoError))
          return true;
      }
    }
    return false;
  }

  internal GraphsSet LoadArchiveGraphs(long archiveID, IUserSession session)
  {
    GraphsSet graphsSet = (GraphsSet) null;
    using (MemoryStream source = new MemoryStream(SignsServerCache.GetSignsSetup(session, archiveID) ?? new byte[0]))
    {
      if (source.Length > 0L)
        graphsSet = GraphsSet.Load((Stream) source);
    }
    return graphsSet;
  }

  public bool CheckSigns(
    long[] objectIDs,
    long archiveID,
    GraphsSet gSet,
    Guid sessionGuid,
    bool raiseException,
    bool change)
  {
    string errorMessage = (string) null;
    object[] additionalInfo = (object[]) null;
    return this.CheckSigns(objectIDs, archiveID, gSet, sessionGuid, raiseException, change, out errorMessage, out additionalInfo);
  }

  public bool CheckSigns(
    long[] objectIDs,
    long archiveID,
    GraphsSet gSet,
    Guid sessionGuid,
    bool raiseException,
    bool change,
    out string errorMessage,
    out object[] additionalInfo)
  {
    bool flag = false;
    errorMessage = (string) null;
    additionalInfo = (object[]) null;
    if (objectIDs.Length.Equals(0))
      return true;
    IUserSession sessionById = UserSession.GetSessionByID(sessionGuid);
    try
    {
      SignsServerCache.LoadPossibleGraphs(sessionById);
      if (gSet == null)
      {
        using (MemoryStream source = new MemoryStream(SignsServerCache.GetSignsSetup(sessionById, archiveID) ?? new byte[0]))
        {
          if (source.Length > 0L)
            gSet = GraphsSet.Load((Stream) source);
          else
            flag = true;
        }
      }
      if (gSet != null)
        flag = this.CheckSignsForArchive(objectIDs, gSet, sessionById, change, out errorMessage, out additionalInfo);
    }
    catch (Exception ex)
    {
      flag = false;
      if (raiseException)
        throw ex;
    }
    return flag;
  }

  public bool CheckSigns(long[] objectIDs, GraphsSet gSet, Guid sessionGuid, bool raiseException)
  {
    string errorMessage = (string) null;
    object[] additionalInfo = (object[]) null;
    return this.CheckSigns(objectIDs, gSet, sessionGuid, raiseException, out errorMessage, out additionalInfo);
  }

  public bool CheckSigns(
    long[] objectIDs,
    GraphsSet gSet,
    Guid sessionGuid,
    bool raiseException,
    out string errorMessage,
    out object[] additionalInfo)
  {
    return this.CheckSigns(objectIDs, gSet, -1L, sessionGuid, raiseException, out errorMessage, out additionalInfo);
  }

  public bool CheckSigns(
    long[] objectIDs,
    GraphsSet gSet,
    long userID,
    Guid sessionGuid,
    bool raiseException)
  {
    string errorMessage = (string) null;
    object[] additionalInfo = (object[]) null;
    return this.CheckSigns(objectIDs, gSet, userID, sessionGuid, raiseException, out errorMessage, out additionalInfo);
  }

  public bool CheckSigns(
    long[] objectIDs,
    GraphsSet gSet,
    long userID,
    Guid sessionGuid,
    bool raiseException,
    out string errorMessage,
    out object[] additionalInfo)
  {
    errorMessage = (string) null;
    additionalInfo = (object[]) null;
    IUserSession sessionById = UserSession.GetSessionByID(sessionGuid);
    bool flag;
    try
    {
      SignsServerCache.LoadPossibleGraphs(sessionById);
      flag = this.CheckSignsForDocType(objectIDs, gSet, userID, sessionById, false, out errorMessage, out additionalInfo);
    }
    catch (Exception ex)
    {
      flag = false;
      if (raiseException)
        throw ex;
    }
    return flag;
  }

  private SignsProviderList GetSignedGraphs(long objectID, IUserSession session)
  {
    return this.GetSignedGraphs(objectID, -1L, session);
  }

  public bool HasSignedGraphs(long objectID, IUserSession session)
  {
    return this.HasSignedGraphs(objectID, -1L, session);
  }

  public bool HasSignedGraphs(long objectID, long userID, IUserSession session)
  {
    return this.GetSignedGraphs(objectID, userID, session).Count > 0;
  }

  private SignsProviderList GetSignedGraphs(long objectID, long userID, IUserSession session)
  {
    IDBRelationCollection relationCollection1 = session.GetRelationCollection(SignsHolder.SignRelationTypeID);
    relationCollection1.ObjectTypeID = SignsHolder.SignObjectTypeID;
    ConditionStructure[] conditions = new ConditionStructure[0];
    if (!userID.Equals(-1L))
      conditions = new ConditionStructure[1]
      {
        new ConditionStructure(SignsHolder.SignUpAttrTypeID, RelationalOperators.Equal, (object) userID, (object) null, LogicalOperators.NONE, 0, false, AttributeSourceTypes.Auto, ColumnContents.ID)
        {
          AttributeSource = AttributeSourceTypes.Object,
          Content = ColumnContents.ID
        }
      };
    ColumnDescriptor[] columns = new ColumnDescriptor[3]
    {
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) SignsHolder.GraphAttrTypeID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) SignsHolder.ModifyDateAttrTypeID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0)
    };
    DBRecordSetParams paramSet = new DBRecordSetParams(conditions, columns);
    DataTable dataTable = relationCollection1.ConsistFrom(paramSet, objectID);
    IDBRelationCollection relationCollection2 = session.GetRelationCollection(SignsHolder.SignRelationTypeID);
    relationCollection2.ObjectTypeID = SignsHolder.CryptoSignObjectTypeID;
    DataTable table = relationCollection2.ConsistFrom(paramSet, objectID);
    dataTable.Merge(table);
    SignsProviderList signedGraphs = new SignsProviderList();
    if (dataTable.Rows.Count > 0)
    {
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        long int64 = Convert.ToInt64(row[0]);
        string graphValue = Convert.ToString(row[1]);
        DateTime dateTime = Convert.ToDateTime(row[2]);
        SignsProvider signsProvider = new SignsProvider(int64, graphValue, dateTime);
        if (!this.CheckHashCode(objectID, int64, session.SessionGUID))
          signsProvider.ErrorCode = SignsErrors.BadHashcode;
        signedGraphs.Add(signsProvider);
      }
    }
    return signedGraphs;
  }

  private bool CompareGraphs(
    long objectID,
    GraphsSet gSet,
    long userID,
    IUserSession session,
    out SignsProviderList errorSignedGraph)
  {
    bool flag = false;
    errorSignedGraph = new SignsProviderList();
    if (gSet.Count == 0)
      return true;
    SignsProviderList signedGraphs = this.GetSignedGraphs(objectID, userID, session);
    foreach (string g1 in gSet)
    {
      GraphsCollection g2 = gSet[g1];
      if (g2.Count != 0)
      {
        flag = true;
        foreach (GraphClass graphClass in g2)
        {
          if (!(SignsServerCache.GetGraphDescr(graphClass.Value) != string.Empty))
            throw new ArgumentException(string.Format(LocalizationHolder.rm.GetString("Signs.Server_9"), (object) graphClass.Value, (object) MetaDataHelper.GetAttributeTypeName(SignsHolder.GraphAttrTypeID)));
          SignsProvider itemWithGraphValue = signedGraphs.GetItemWithGraphValue(graphClass.Value, true);
          if (itemWithGraphValue == null)
          {
            errorSignedGraph.Add(new SignsProvider(-1L, graphClass.Value, DateTime.Now)
            {
              ErrorCode = SignsErrors.NoneSignature
            });
            flag = false;
          }
          else
          {
            if (itemWithGraphValue.ErrorCode != SignsErrors.BadHashcode && graphClass.StrongCheck && !DateTimeHelper.EqualsTruncateToSeconds(session.GetObject(objectID).GetAttributeByID(SignsHolder.ModifyDateAttrTypeID).AsDateTime, itemWithGraphValue.ModifyDate))
            {
              itemWithGraphValue.ErrorCode = SignsErrors.OldSignature;
              flag = false;
            }
            if (itemWithGraphValue.ErrorCode != SignsErrors.NoError)
            {
              errorSignedGraph.Add(itemWithGraphValue);
              flag = false;
            }
          }
        }
        if (flag)
          break;
      }
    }
    return flag;
  }

  internal GraphsSet LoadGraphsSet(Guid objectType, Guid step, IUserSession session)
  {
    GraphsSet graphsSet = new GraphsSet();
    IDBObjectCollection objectCollection = session.GetObjectCollection(SignsHolder.ContainerObjectTypeID);
    string conditionValue = step.ToString() + objectType.ToString();
    string g = "cad00922-306c-11d8-b4e9-00304f19f545";
    ConditionStructure conditionStructure = new ConditionStructure(SignsHolder.LCStepForSignsAttrTypeGuid, RelationalOperators.Equal, (object) step, LogicalOperators.NONE, 0);
    DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(new Guid(g), RelationalOperators.Equal, (object) conditionValue, LogicalOperators.NONE, 0)
    }, new object[1]
    {
      (object) ObligatoryObjectAttributes.F_OBJECT_ID
    });
    paramSet.RecordCount = 1;
    DataTable dataTable1 = objectCollection.Select(paramSet);
    if (dataTable1 != null && dataTable1.Rows.Count.Equals(1))
    {
      long int64 = Convert.ToInt64(dataTable1.Rows[0][0]);
      byte[] signsSetup = SignsServerCache.GetSignsSetup(session, int64);
      if (signsSetup != null && signsSetup.Length != 0)
      {
        using (MemoryStream source = new MemoryStream(signsSetup))
          graphsSet = GraphsSet.Load((Stream) source);
      }
    }
    if (graphsSet.Count == 0)
    {
      paramSet = new DBRecordSetParams(new ConditionStructure[1]
      {
        conditionStructure
      }, new object[1]
      {
        (object) ObligatoryObjectAttributes.F_OBJECT_ID
      });
      paramSet.RecordCount = 1;
      DataTable dataTable2 = objectCollection.Select(paramSet);
      if (dataTable2 != null && dataTable2.Rows.Count.Equals(1))
      {
        long int64 = Convert.ToInt64(dataTable2.Rows[0][0]);
        byte[] signsSetup = SignsServerCache.GetSignsSetup(session, int64);
        if (signsSetup != null && signsSetup.Length != 0)
        {
          using (MemoryStream source = new MemoryStream(signsSetup))
            graphsSet = GraphsSet.Load((Stream) source);
        }
      }
    }
    return graphsSet;
  }

  internal GraphsSet LoadGraphsSet(Guid level, IUserSession session)
  {
    GraphsSet graphsSet = new GraphsSet();
    DataTable dataTable = session.GetObjectCollection(SignsHolder.ContainerObjectTypeID).Select(new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(SignsHolder.LCLevelForSignsAttrTypeID, RelationalOperators.Equal, (object) level, LogicalOperators.NONE, 0, false)
    }, new object[1]
    {
      (object) ObligatoryObjectAttributes.F_OBJECT_ID
    })
    {
      RecordCount = 1
    });
    if (dataTable != null && dataTable.Rows.Count.Equals(1))
    {
      long int64 = Convert.ToInt64(dataTable.Rows[0][0]);
      byte[] signsSetup = SignsServerCache.GetSignsSetup(session, int64);
      if (signsSetup != null && signsSetup.Length != 0)
      {
        using (MemoryStream source = new MemoryStream(signsSetup))
          graphsSet = GraphsSet.Load((Stream) source);
      }
    }
    return graphsSet;
  }

  public bool CheckSignsForNextStep(
    IDBObject[] objects,
    IUserSession session,
    IDBLifecycleStep customStep,
    out string errorMessage,
    out object[] additionalInfo)
  {
    bool flag = true;
    errorMessage = (string) null;
    additionalInfo = (object[]) null;
    Hashtable table = new Hashtable();
    SignsServerCache.LoadPossibleGraphs(session);
    foreach (IDBObject dbObject in objects)
    {
      if (dbObject != null && SignsServerCache.HasSignApp(dbObject.ObjectType))
      {
        Guid guid1 = MetaDataHelper.GetObjectType(dbObject.ObjectType).Guid;
        Guid stepGuid = customStep.Properties.StepGuid;
        GraphsSet graphsSet = SignsServerCache.GetGraphsSetForObjectType(guid1, stepGuid);
        if (graphsSet == null)
        {
          graphsSet = this.LoadGraphsSet(guid1, stepGuid, session);
          SignsServerCache.AddObjectType(guid1, stepGuid, graphsSet);
        }
        if (graphsSet.Count == 0)
        {
          Guid guid2 = (session.GetLifecycleLevel(customStep.LevelID) as IDBGuid).GUID;
          graphsSet = SignsServerCache.GetGraphsSetForLevel(guid2);
          if (graphsSet == null)
          {
            graphsSet = this.LoadGraphsSet(guid2, session);
            SignsServerCache.AddObjectLevel(guid2, graphsSet);
          }
        }
        GraphsSet gSet = GraphsSet.Clone(graphsSet);
        SignsProviderList errorSignedGraph;
        flag = this.CompareGraphs(dbObject.ObjectID, gSet, -1L, session, out errorSignedGraph);
        if (errorSignedGraph.Count > 0)
          table[(object) dbObject.ObjectID] = (object) errorSignedGraph;
        if (!flag)
          break;
      }
    }
    if (table.Count > 0)
      errorMessage = this.CheckErrorMessage(table, session);
    return flag;
  }

  private bool CheckSignsForArchive(
    long[] objectIDs,
    GraphsSet gSet,
    IUserSession session,
    bool change,
    out string errorMessage,
    out object[] additionalInfo)
  {
    bool flag = true;
    errorMessage = (string) null;
    additionalInfo = (object[]) null;
    Hashtable table = new Hashtable();
    foreach (long objectId in objectIDs)
    {
      DBObject dbObject = session.GetObject(objectId) as DBObject;
      if (SignsServerCache.HasSignApp(dbObject.ObjectType))
      {
        Guid guid = (session.GetLifecycleLevel(dbObject.LevelID) as IDBGuid).GUID;
        GraphsSet graphsSet = SignsServerCache.GetGraphsSetForLevel(guid);
        if (graphsSet == null)
        {
          graphsSet = this.LoadGraphsSet(guid, session);
          SignsServerCache.AddObjectLevel(guid, graphsSet);
        }
        gSet.Add(GraphsSet.Clone(graphsSet));
        SignsProviderList errorSignedGraph;
        flag = this.CompareGraphs(objectId, gSet, -1L, session, out errorSignedGraph);
        if (errorSignedGraph.Count > 0)
        {
          if (change)
            throw new KernelException(LocalizationHolder.rm.GetString("Signs.Server_29"));
          table[(object) objectId] = (object) errorSignedGraph;
        }
        if (!flag)
          break;
      }
    }
    if (table.Count > 0)
      errorMessage = this.CheckErrorMessage(table, session);
    return flag;
  }

  private bool CheckSignsForDocType(
    long[] objectIDs,
    GraphsSet gSet,
    long userID,
    IUserSession session,
    bool change,
    out string errorMessage,
    out object[] additionalInfo)
  {
    bool flag = true;
    errorMessage = (string) null;
    additionalInfo = (object[]) null;
    Hashtable hashtable = new Hashtable();
    foreach (long objectId in objectIDs)
    {
      DBObject dbObject = session.GetObject(objectId) as DBObject;
      if (SignsServerCache.HasSignApp(dbObject.ObjectType))
      {
        IDBGuid lifecycleLevel = session.GetLifecycleLevel(dbObject.LevelID) as IDBGuid;
        Guid guid1 = (dbObject.ObjectTypeClass as IDBGuid).GUID;
        Guid stepGuid = dbObject.LCStepObject.Properties.StepGuid;
        Guid guid2 = lifecycleLevel.GUID;
        if (gSet == null)
        {
          gSet = new GraphsSet();
          GraphsSet graphsSet1 = SignsServerCache.GetGraphsSetForLevel(guid2);
          if (graphsSet1 == null)
          {
            graphsSet1 = this.LoadGraphsSet(guid2, session);
            SignsServerCache.AddObjectLevel(guid2, graphsSet1);
          }
          gSet = GraphsSet.Clone(graphsSet1);
          GraphsSet graphsSet2 = SignsServerCache.GetGraphsSetForObjectType(guid1, stepGuid);
          if (graphsSet2 == null)
          {
            graphsSet2 = this.LoadGraphsSet(guid1, stepGuid, session);
            SignsServerCache.AddObjectType(guid1, stepGuid, graphsSet2);
          }
          gSet.Add(GraphsSet.Clone(graphsSet2));
        }
        SignsProviderList errorSignedGraph;
        flag = this.CompareGraphs(objectId, gSet, userID, session, out errorSignedGraph);
        if (errorSignedGraph.Count > 0)
        {
          if (change)
            throw new KernelException(LocalizationHolder.rm.GetString("Signs.Server_29"));
          hashtable[(object) objectId] = (object) errorSignedGraph;
        }
        if (!flag)
          break;
      }
    }
    if (hashtable.Count > 0)
      errorMessage = LocalizationHolder.rm.GetString("Signs.Server_30");
    return flag;
  }

  private string CheckErrorMessage(Hashtable table, IUserSession session)
  {
    string additionalInfo = (string) null;
    return this.CheckErrorMessage(table, session, additionalInfo);
  }

  private string CheckErrorMessage(Hashtable table, IUserSession session, string additionalInfo)
  {
    string str1 = LocalizationHolder.rm.GetString("Signs.Server_10");
    string str2 = LocalizationHolder.rm.GetString("Signs.Server_11");
    string str3 = LocalizationHolder.rm.GetString("Signs.Server_12");
    string format1 = LocalizationHolder.rm.GetString("Signs.Server_14");
    string format2 = LocalizationHolder.rm.GetString("Signs.Server_15");
    string format3 = LocalizationHolder.rm.GetString("Signs.Server_16");
    string format4 = LocalizationHolder.rm.GetString("Signs.Server_17");
    string format5 = LocalizationHolder.rm.GetString("Signs.Server_18");
    string format6 = LocalizationHolder.rm.GetString("Signs.Server_19");
    List<string> stringList1 = new List<string>();
    foreach (DictionaryEntry dictionaryEntry in table)
    {
      long int64 = Convert.ToInt64(dictionaryEntry.Key);
      IDBObject dbObject = session.GetObject(int64, false);
      SignsProviderList signsProviderList = table[(object) int64] as SignsProviderList;
      string str4 = !dbObject.IsCreationMode ? str3 + dbObject.NameInMessages : str3 + MetaDataHelper.GetObjectName(dbObject.ObjectType);
      List<string> stringList2 = new List<string>();
      List<string> stringList3 = new List<string>();
      List<string> stringList4 = new List<string>();
      foreach (SignsProvider signsProvider in signsProviderList)
      {
        string graphDescr = SignsServerCache.GetGraphDescr(signsProvider.GraphValue);
        switch (signsProvider.ErrorCode)
        {
          case SignsErrors.OldSignature:
            if (!stringList3.Contains(graphDescr))
            {
              stringList3.Add($"'{graphDescr}'");
              continue;
            }
            continue;
          case SignsErrors.NoneSignature:
            if (!stringList2.Contains(graphDescr))
            {
              stringList2.Add($"'{graphDescr}'");
              continue;
            }
            continue;
          case SignsErrors.BadHashcode:
            if (!stringList4.Contains(graphDescr))
            {
              stringList4.Add($"'{graphDescr}'");
              continue;
            }
            continue;
          default:
            continue;
        }
      }
      string str5 = string.Join(", ", stringList2.ToArray());
      string str6 = string.Join(", ", stringList3.ToArray());
      string str7 = string.Join(", ", stringList4.ToArray());
      int num = 0;
      int count;
      if (stringList4.Count > 0)
      {
        count = stringList4.Count;
        str4 = !count.Equals(1) ? str4 + string.Format(format4, (object) str7) : str4 + string.Format(format3, (object) str7);
        ++num;
      }
      if (stringList2.Count > 0)
      {
        if (num > 0)
          str4 += ",";
        count = stringList2.Count;
        str4 = !count.Equals(1) ? str4 + string.Format(format2, (object) str5) : str4 + string.Format(format1, (object) str5);
        ++num;
      }
      if (stringList3.Count > 0)
      {
        if (num > 0)
          str4 += ",";
        count = stringList3.Count;
        str4 = !count.Equals(1) ? str4 + string.Format(format6, (object) str6) : str4 + string.Format(format5, (object) str6);
        ++num;
      }
      if (num > 0)
        str4 += ";";
      stringList1.Add(str4);
    }
    if (stringList1.Count > 0)
    {
      string str8 = stringList1[stringList1.Count - 1].TrimEnd(';') + ".";
      stringList1[stringList1.Count - 1] = str8;
    }
    if (additionalInfo != null)
      stringList1.Insert(0, additionalInfo);
    else
      stringList1.Insert(0, str1 + "\r\n");
    if (stringList1.Count > 1)
      stringList1.Insert(1, str2);
    string[] array = stringList1.ToArray();
    return string.Join(string.Empty, array);
  }

  public Dictionary<long, List<long>> SignAs(SignCollection infoForSigning, Guid sessionGuid)
  {
    Dictionary<long, List<long>> dict;
    this.Sign(infoForSigning, sessionGuid, SignsHolder.SignObjectTypeID, out dict);
    return dict;
  }

  public Dictionary<long, List<long>> Sign(
    SignCollection infoForSigning,
    Guid sessionGuid,
    int signTypeID)
  {
    Dictionary<long, List<long>> dict;
    this.Sign(infoForSigning, sessionGuid, signTypeID, out dict);
    return dict;
  }

  public IDBRelation Sign(
    SignCollection infoForSigning,
    Guid sessionGuid,
    int signTypeID,
    out Dictionary<long, List<long>> dict)
  {
    dict = new Dictionary<long, List<long>>();
    IDBRelation dbRelation = (IDBRelation) null;
    UserSession sessionById = UserSession.GetSessionByID(sessionGuid) as UserSession;
    SignsServerCache.LoadPossibleGraphs((IUserSession) sessionById);
    IDBObject dbObject1 = sessionById.GetObject(infoForSigning.UserID);
    if (sessionById.UserID != infoForSigning.UserID)
    {
      IDBObject dbObject2 = sessionById.GetObject(sessionById.UserID);
      throw new KernelException($"Пользователь {dbObject1.Caption} не может подписывать объекты в активной сессии пользователя {dbObject2.Caption}.");
    }
    if ((dbObject1 as DBObject).LevelID == SignsHolder.FiredUserLevelID)
      throw new KernelException($"Пользователь {dbObject1.Caption} не может подписывать объекты, т.к. он уволен.");
    IDBAttribute attributeById1 = dbObject1.GetAttributeByID(UserSession.UserLockedAttributeID);
    if (attributeById1 != null && !attributeById1.IsNull && attributeById1.AsBoolean)
      throw new KernelException($"Пользователь {dbObject1.Caption} не может подписывать объекты, т.к. он заблокирован.");
    IDBAttribute attributeById2 = dbObject1.GetAttributeByID(sessionById.IdentHelper.LoginNameID);
    IDBAttribute attributeByGuid1 = dbObject1.GetAttributeByGuid(SignsHolder.StaffPositionAttrGuid, false);
    bool flag1 = attributeByGuid1 != null;
    string initValue1 = flag1 ? attributeByGuid1.AsString : string.Empty;
    IDBAttribute attributeById3 = dbObject1.GetAttributeByID(SignsHolder.RankAttrTypeID);
    IDBAttribute attributeByGuid2 = dbObject1.GetAttributeByGuid(SignsHolder.FIOInSignAttrTypeGuid, false);
    string asString = dbObject1.GetAttributeByID(SignsHolder.VisibleNameAttrTypeID).AsString;
    if (attributeByGuid2 != null && !string.IsNullOrEmpty(attributeByGuid2.AsString))
      asString = attributeByGuid2.AsString;
    IDBAttribute attributeById4 = (sessionById.GetObject(infoForSigning.RankID, false) ?? throw new KernelException(string.Format(LocalizationHolder.rm.GetString("Signs.Server_32"), (object) infoForSigning.RankID))).GetAttributeByID(SignsHolder.NaimAttrTypeID);
    ArrayList arrayList1 = new ArrayList();
    foreach (object obj in attributeById3.Values)
    {
      if (obj.GetType().Equals(typeof (long)))
      {
        long int64 = Convert.ToInt64(obj);
        if (!arrayList1.Contains((object) int64))
          arrayList1.Add((object) int64);
      }
    }
    if (!arrayList1.Contains((object) infoForSigning.RankID))
      throw new KernelException(string.Format(LocalizationHolder.rm.GetString("Signs.Server_28"), (object) attributeById2.Description, (object) attributeById4.Description));
    using (MemoryStream sourceStream = new MemoryStream(SignsServerCache.GetSignsSetup((IUserSession) sessionById, infoForSigning.RankID) ?? new byte[0]))
    {
      Graphs4Type graphs4Type = sourceStream.Length > 0L ? new Graphs4Type((Stream) sourceStream, (Dictionary<string, string>) null) : throw new KernelException(string.Format(LocalizationHolder.rm.GetString("Signs.Server_27"), (object) attributeById4.Description));
      sessionById.StartTransaction();
      try
      {
        foreach (long listOfId in infoForSigning.ListOfIDs)
        {
          IDBObject dbObject3 = sessionById.GetObject(listOfId, false);
          if (dbObject3 == null)
            throw new KernelException(string.Format(LocalizationHolder.rm.GetString("Signs.Server_33"), (object) listOfId));
          if (listOfId < 0L)
            throw new KernelException(string.Format(LocalizationHolder.rm.GetString("Signs.Server_34"), (object) dbObject3.NameInMessages, (object) dbObject3.ObjectID)).WithRecoveryActions((ErrorRecoveryAction) new OpenIPSObjectRecoveryAction(dbObject3.ObjectID));
          if (!SignsServerCache.HasSignApp(dbObject3.ObjectType))
            throw new KernelException(string.Format(LocalizationHolder.rm.GetString("Signs.Server_22"), (object) MetaDataHelper.GetObjectTypeName(dbObject3.ObjectType)));
          IDBObjectCollection objectCollection = sessionById.GetObjectCollection(-1);
          IDBRelationCollection relationCollection = sessionById.GetRelationCollection(SignsHolder.SignRelationTypeID);
          List<long> longList = new List<long>();
          Graphs4TypeStruct graphs4ObjectType = graphs4Type.GetGraphs4ObjectType((IUserSession) sessionById, dbObject3.ObjectType, true);
          foreach (string listOfGraph in infoForSigning.ListOfGraphs)
          {
            if (!graphs4ObjectType.Graphs.Contains(listOfGraph))
              throw new KernelException(string.Format(LocalizationHolder.rm.GetString("Signs.Server_23"), (object) MetaDataHelper.GetObjectTypeName(dbObject3.ObjectType), (object) SignsServerCache.GetGraphDescr(listOfGraph)));
            bool flag2 = sessionById.Configurations.ReadBool("SIGNS", "CERTIFICATES", "CHECK_COPY_ACTUALITY", false, DBConfigMode.GlobalOnly);
            if (flag2 || SignsHolder.CheckActualSignMadeBySameUser)
            {
              foreach (KeyValuePair<long, DateTime> keyValuePair in SignsService.GetAllSignInGraph(sessionById, listOfId, signTypeID, listOfGraph, infoForSigning.RankID))
              {
                long key = keyValuePair.Key;
                DateTime signDate = keyValuePair.Value;
                DateTime asDateTime1 = dbObject3.GetAttributeByID(SignsHolder.ModifyDateAttrTypeID).AsDateTime;
                switch (SignHelper.TranslateStatus((IUserSession) sessionById, listOfId, key, signTypeID, asDateTime1, signDate))
                {
                  case SignStatuses.CryptoSignActual:
                  case SignStatuses.SignActual:
                    IDBObject dbObject4 = sessionById.GetObject(key, false);
                    DateTime asDateTime2 = dbObject4.GetAttributeByID(SignsHolder.DateOfSignatureID).AsDateTime;
                    string str = string.Empty;
                    long asInteger = dbObject4.GetAttributeByID(SignsHolder.SignUpAttrTypeID).AsInteger;
                    IDBObject dbObject5 = sessionById.GetObject(asInteger, false);
                    if (dbObject5 != null)
                      str = dbObject5.Caption;
                    if (flag2 || str == sessionById.UserName)
                    {
                      string graphDescr = SignsServerCache.GetGraphDescr(listOfGraph);
                      throw new KernelException(string.Format(LocalizationHolder.rm.GetString("Signs.Server_35"), (object) dbObject3.NameInMessages, (object) graphDescr, (object) asDateTime2, (object) attributeById4.Description, (object) str));
                    }
                    continue;
                  default:
                    continue;
                }
              }
            }
            IDBObject signMadeByUser = SignsService.GetSignMadeByUser(sessionById, listOfId, signTypeID, listOfGraph, infoForSigning.RankID, infoForSigning.UserID);
            if (signMadeByUser != null)
            {
              if (listOfId < 0L && signMadeByUser.GetAttributeByID(SignsHolder.InArchiveAttrTypeID).AsBoolean)
              {
                sessionById.GetRelation(listOfId, signMadeByUser.ID).Delete(0L);
                signMadeByUser = objectCollection.Create(signTypeID);
              }
              else if (signMadeByUser.ObjectType == SignsHolder.SignObjectTypeID && signTypeID == SignsHolder.CryptoSignObjectTypeID)
                signMadeByUser.ObjectType = SignsHolder.CryptoSignObjectTypeID;
              else if (signMadeByUser.ObjectType == SignsHolder.CryptoSignObjectTypeID && signTypeID == SignsHolder.SignObjectTypeID)
                signMadeByUser.ObjectType = SignsHolder.SignObjectTypeID;
            }
            else
              signMadeByUser = objectCollection.Create(signTypeID);
            ArrayList arrayList2 = new ArrayList();
            AttributeValues attributeValues1 = new AttributeValues(SignsHolder.ModifyDateAttrTypeID, (dbObject3.GetAttributeByID(SignsHolder.ModifyDateAttrTypeID) ?? throw new KernelException(string.Format(LocalizationHolder.rm.GetString("Signs.Server_24"), (object) MetaDataHelper.GetAttributeTypeName(SignsHolder.ModifyDateAttrTypeID)))).Value);
            arrayList2.Add((object) attributeValues1);
            AttributeValues attributeValues2 = new AttributeValues(SignsHolder.GraphAttrTypeID, (object) listOfGraph);
            arrayList2.Add((object) attributeValues2);
            AttributeValues attributeValues3 = new AttributeValues(SignsHolder.RankAttrTypeID, (object) infoForSigning.RankID);
            arrayList2.Add((object) attributeValues3);
            AttributeValues attributeValues4 = new AttributeValues(SignsHolder.SignUpAttrTypeID, (object) infoForSigning.UserID);
            arrayList2.Add((object) attributeValues4);
            object initValue2 = sessionById.ActingUserID == 0L ? (object) DeleteModesEnum.None : (object) sessionById.ActingUserID;
            AttributeValues attributeValues5 = new AttributeValues(SignsHolder.SignUpIOAttrTypeID, initValue2);
            arrayList2.Add((object) attributeValues5);
            AttributeValues attributeValues6 = new AttributeValues(SignsHolder.InArchiveAttrTypeID, (object) (listOfId > 0L));
            arrayList2.Add((object) attributeValues6);
            long initValue3 = signTypeID == SignsHolder.SignObjectTypeID ? (long) sessionById.AlgorithmVersion : (long) dbObject3.GetHashVersion();
            AttributeValues attributeValues7 = new AttributeValues(SignsHolder.SignVersionAttrTypeID, (object) initValue3);
            arrayList2.Add((object) attributeValues7);
            if (signMadeByUser.GetAttributeByID(SignsHolder.ResolutionAttrTypeID) != null || infoForSigning.Resolution != string.Empty)
            {
              AttributeValues attributeValues8 = new AttributeValues(SignsHolder.ResolutionAttrTypeID, (object) infoForSigning.Resolution);
              arrayList2.Add((object) attributeValues8);
            }
            DateTime now = DateTime.Now;
            DateTime dateTime = now.ToUniversalTime() + signMadeByUser.Session.TimeZoneOffset;
            AttributeValues attributeValues9 = new AttributeValues(SignsHolder.DateOfSignatureID, (object) dateTime.TruncateToSecond());
            arrayList2.Add((object) attributeValues9);
            if (flag1)
            {
              if (sessionById.GetObjectType(signMadeByUser.ObjectType).Attributes.GetAttributeByGUID(SignsHolder.StaffPositionAttrGuid, false) != null)
              {
                AttributeValues attributeValues10 = new AttributeValues(SignsHolder.StaffPositionAttrID, (object) initValue1);
                arrayList2.Add((object) attributeValues10);
              }
            }
            else
            {
              IDBAttribute attributeByGuid3 = signMadeByUser.GetAttributeByGuid(SignsHolder.StaffPositionAttrGuid, false);
              if (attributeByGuid3 != null && attributeByGuid3.AsString != string.Empty)
              {
                AttributeValues attributeValues11 = new AttributeValues(SignsHolder.StaffPositionAttrID, (object) string.Empty);
                arrayList2.Add((object) attributeValues11);
              }
            }
            signMadeByUser.SetAttributesValues(arrayList2.ToArray(typeof (AttributeValues)) as AttributeValues[]);
            if (MetaDataHelper.GetObjectType(signMadeByUser.ObjectType).CaptionAttribute <= 0)
              signMadeByUser.Caption = LocalizationHolder.rm.GetString("Signs.Server_25");
            string signHash = string.Empty;
            if (signTypeID == SignsHolder.SignObjectTypeID)
            {
              IDBAttribute dbAttribute = signMadeByUser.GetAttributeByID(SignsHolder.HashProtectionAttrTypeID) ?? signMadeByUser.Attributes.AddAttribute(SignsHolder.HashProtectionAttrTypeID, true);
              byte[] inArray = HashPack.CalcHash(HashPack.GetHashPack(signMadeByUser));
              dbAttribute.Value = (object) Convert.ToBase64String(inArray);
              signHash = dbAttribute.AsString;
            }
            if (signMadeByUser.IsCreationMode)
            {
              dbRelation = relationCollection.Create(listOfId, signMadeByUser.ObjectID);
              signMadeByUser.CommitCreation(true);
            }
            else
              dbRelation = sessionById.GetRelation(listOfId, signMadeByUser.ID, SignsHolder.SignRelationTypeID);
            this.AddUserInfo(listOfId, listOfGraph, asString, infoForSigning.RankID, now.Date, signHash);
            longList.Add(signMadeByUser.ObjectID);
          }
          dict[listOfId] = longList;
        }
        sessionById.Commit();
      }
      catch (Exception ex)
      {
        sessionById.Rollback();
        throw ex;
      }
    }
    return dbRelation;
  }

  private static IDBObject GetSignMadeByUser(
    UserSession session,
    long signedObjectID,
    int signTypeID,
    string graph,
    long rankID,
    long signUpUserID)
  {
    IDBObject signMadeByUser = (IDBObject) null;
    ConditionStructure conditionStructure1 = new ConditionStructure(SignsHolder.GraphAttrTypeID, RelationalOperators.Equal, (object) graph, LogicalOperators.AND, 0, false);
    conditionStructure1.AttributeSource = AttributeSourceTypes.Object;
    conditionStructure1.Content = ColumnContents.Text;
    ConditionStructure conditionStructure2 = conditionStructure1;
    conditionStructure1 = new ConditionStructure(SignsHolder.RankAttrTypeID, RelationalOperators.Equal, (object) rankID, (object) null, LogicalOperators.AND, 0, false, AttributeSourceTypes.Auto, ColumnContents.ID);
    conditionStructure1.AttributeSource = AttributeSourceTypes.Object;
    conditionStructure1.Content = ColumnContents.ID;
    ConditionStructure conditionStructure3 = conditionStructure1;
    conditionStructure1 = new ConditionStructure(SignsHolder.SignUpAttrTypeID, RelationalOperators.Equal, (object) signUpUserID, (object) null, LogicalOperators.NONE, 0, false, AttributeSourceTypes.Auto, ColumnContents.ID);
    conditionStructure1.AttributeSource = AttributeSourceTypes.Object;
    conditionStructure1.Content = ColumnContents.ID;
    ConditionStructure conditionStructure4 = conditionStructure1;
    DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[3]
    {
      conditionStructure2,
      conditionStructure3,
      conditionStructure4
    }, new object[1]
    {
      (object) ObligatoryObjectAttributes.F_OBJECT_ID
    })
    {
      RecordCount = 1
    };
    IDBRelationCollection relationCollection = session.GetRelationCollection(SignsHolder.SignRelationTypeID);
    relationCollection.ObjectTypeID = signTypeID;
    DataTable dataTable1 = relationCollection.ConsistFrom(paramSet, signedObjectID);
    if (dataTable1 != null && dataTable1.Rows.Count == 1)
    {
      signMadeByUser = session.GetObject(Convert.ToInt64(dataTable1.Rows[0][0]), false);
    }
    else
    {
      relationCollection.ObjectTypeID = signTypeID == SignsHolder.SignObjectTypeID ? SignsHolder.CryptoSignObjectTypeID : SignsHolder.SignObjectTypeID;
      DataTable dataTable2 = relationCollection.ConsistFrom(paramSet, signedObjectID);
      if (dataTable2 != null && dataTable2.Rows.Count == 1)
        signMadeByUser = session.GetObject(Convert.ToInt64(dataTable2.Rows[0][0]), false);
    }
    return signMadeByUser;
  }

  private static Dictionary<long, DateTime> GetAllSignInGraph(
    UserSession session,
    long signedObjectID,
    int signTypeID,
    string graph,
    long rankID)
  {
    Dictionary<long, DateTime> allSignInGraph = new Dictionary<long, DateTime>();
    ConditionStructure conditionStructure1 = new ConditionStructure(SignsHolder.GraphAttrTypeID, RelationalOperators.Equal, (object) graph, LogicalOperators.AND, 0, false);
    conditionStructure1.AttributeSource = AttributeSourceTypes.Object;
    conditionStructure1.Content = ColumnContents.Text;
    ConditionStructure conditionStructure2 = conditionStructure1;
    conditionStructure1 = new ConditionStructure(SignsHolder.RankAttrTypeID, RelationalOperators.Equal, (object) rankID, (object) null, LogicalOperators.AND, 0, false, AttributeSourceTypes.Auto, ColumnContents.ID);
    conditionStructure1.AttributeSource = AttributeSourceTypes.Object;
    conditionStructure1.Content = ColumnContents.ID;
    ConditionStructure conditionStructure3 = conditionStructure1;
    ColumnDescriptor[] columns = new ColumnDescriptor[2]
    {
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object, ColumnContents.ID, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) SignsHolder.ModifyDateAttrTypeID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0)
    };
    DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[2]
    {
      conditionStructure2,
      conditionStructure3
    }, columns);
    IDBRelationCollection relationCollection = session.GetRelationCollection(SignsHolder.SignRelationTypeID);
    relationCollection.ObjectTypeID = signTypeID;
    DataTable dataTable = relationCollection.ConsistFrom(paramSet, signedObjectID);
    relationCollection.ObjectTypeID = signTypeID == SignsHolder.SignObjectTypeID ? SignsHolder.CryptoSignObjectTypeID : SignsHolder.SignObjectTypeID;
    DataTable table = relationCollection.ConsistFrom(paramSet, signedObjectID);
    dataTable.Merge(table);
    if (dataTable.Rows.Count >= 1)
    {
      for (int index = 0; index < dataTable.Rows.Count; ++index)
      {
        long int64 = Convert.ToInt64(dataTable.Rows[index][0]);
        DateTime dateTime = Convert.ToDateTime(dataTable.Rows[index][1]);
        allSignInGraph.Add(int64, dateTime);
      }
    }
    return allSignInGraph;
  }

  public string GetSignHash(long signObjectId, Guid sessionGuid)
  {
    IDBObject signObject = (UserSession.GetSessionByID(sessionGuid) as UserSession).GetObject(signObjectId, false);
    return signObject != null ? Convert.ToBase64String(HashPack.CalcHash(HashPack.GetHashPack(signObject))) : (string) null;
  }

  public void CreateCopySigns(IDBObject createdObject, IUserSession session)
  {
    bool flag = session is UserSession userSession ? session.Configurations.ReadBool("SIGNS", "CERTIFICATES", "COPY_SIGNS_TO_VERSION", false, DBConfigMode.GlobalOnly) : throw new KernelExceptionID(417, (object) "SignsService.CreateCopySigns");
    DataTable signsInfoTable = SignsService.GetSignsInfoTable(createdObject, session);
    if (signsInfoTable == null || signsInfoTable.Rows.Count <= 0)
      return;
    IDBAttribute attributeById = createdObject.GetAttributeByID(SignsHolder.ModifyDateAttrTypeID);
    foreach (DataRow row in (InternalDataCollectionBase) signsInfoTable.Rows)
    {
      userSession.StartTransaction();
      try
      {
        long int64_1 = Convert.ToInt64(row[1]);
        IDBRelation relation = session.GetRelation(int64_1);
        if (!flag || attributeById == null)
        {
          relation.Delete(0L);
          userSession.Commit();
        }
        else
        {
          long int64_2 = Convert.ToInt64(row[0]);
          IDBObject prototype = session.GetObject(int64_2);
          IDBObject signObject = session.GetObjectCollection(prototype.ObjectType).Create(prototype);
          if (signObject is ISignDBObject)
            (signObject as ISignDBObject).CheckGraphsOnCommitCreation = false;
          if (signObject.IsCreationMode)
          {
            relation.ReplacePartObject(signObject.ObjectID);
            signObject.CommitCreation(true);
          }
          if (this.CheckHashCode(createdObject.ObjectID, prototype.ObjectID, session.SessionGUID))
          {
            byte[] inArray = HashPack.CalcHash(signObject);
            signObject.GetAttributeByID(SignsHolder.HashProtectionAttrTypeID).Value = (object) Convert.ToBase64String(inArray);
          }
          userSession.Commit();
        }
      }
      catch
      {
        userSession.Rollback();
        throw;
      }
    }
  }

  private static DataTable GetSignsInfoTable(IDBObject createdObject, IUserSession session)
  {
    IDBRelationCollection relationCollection = session.GetRelationCollection(SignsHolder.SignRelationTypeID);
    relationCollection.ChildObjectTypes = (IList<int>) new List<int>();
    relationCollection.ChildObjectTypes.Add(SignsHolder.SignObjectTypeID);
    relationCollection.ChildObjectTypes.Add(SignsHolder.CryptoSignObjectTypeID);
    DBRecordSetParams paramSet = new DBRecordSetParams((ConditionStructure[]) null, new object[2]
    {
      (object) ObligatoryObjectAttributes.F_OBJECT_ID,
      (object) ObligatoryObjectAttributes.F_PRJLINK_ID
    }, new object[1]
    {
      (object) ObligatoryObjectAttributes.F_PRJLINK_ID
    }, new SortOrders[1]{ SortOrders.ASC });
    return relationCollection.ConsistFrom(paramSet, createdObject.ObjectID);
  }

  public void CleanCache()
  {
    SignsServerCache.LoadObjectTypesForSignRelation();
    SignsServerCache.CleanCaches();
  }

  public byte[] GetRankSignsSetup(long rankID, Guid sessionGuid)
  {
    return SignsServerCache.GetSignsSetup(UserSession.GetSessionByID(sessionGuid), rankID);
  }

  private void AddUserInfo(
    long objectID,
    string graph,
    string userName,
    long rankID,
    DateTime signDate,
    string signHash)
  {
    IUserSession sessionTemporaryClone = (ServerServices.GetService(typeof (IDBTimedEvents)) as IDBTimedEvents).GetSystemSessionTemporaryClone("SignsServer.AddUserInfo");
    try
    {
      IDBObject dbObject = sessionTemporaryClone.GetObject(objectID, false);
      if (dbObject == null)
        return;
      string str = SignsServerCache.GetGraphDescr(graph);
      if (str == string.Empty)
        str = graph;
      IDBAttribute4TypeCollection attributes = sessionTemporaryClone.GetObjectType(dbObject.ObjectType).Attributes;
      DataTable dataTable = attributes.Select("F_ATTRIBUTE_ID");
      if (dataTable != null && dataTable.Rows.Count > 0)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        {
          IDBAttributeType4 attributeById = attributes.GetAttributeByID(Convert.ToInt32(row[0]));
          if (attributeById.Name == str || attributeById.ShortName == str)
          {
            if ((attributeById.Options & AttributeOptions.ModifyInBase) != AttributeOptions.ModifyInBase)
            {
              if (dbObject.ObjectID > 0L)
                break;
            }
            if (attributeById.IsContent)
              return;
            IDBAttribute dbAttribute = dbObject.GetAttributeByID(attributeById.AttributeID) ?? dbObject.Attributes.AddAttribute(attributeById.AttributeID, false);
            if (attributeById.MultipleValued == MultiValueModes.SingleValue)
              dbAttribute.Value = (object) userName;
            else if (attributeById.MultipleValued == MultiValueModes.MultiValues && !new List<object>((IEnumerable<object>) dbAttribute.Values).Contains((object) userName))
            {
              if (string.IsNullOrEmpty(dbAttribute.Values[0].ToString()))
                dbAttribute.Value = (object) userName;
              else
                dbAttribute.AddValue((object) userName);
            }
          }
        }
      }
      int attributeByTypeNameId1 = MetaDataHelper.GetAttributeByTypeNameID(string.Format(LocalizationHolder.rm.GetString("SignDateAttrName"), (object) str));
      if (attributeByTypeNameId1 > 0)
      {
        IDBAttributeType4 attributeById = attributes.GetAttributeByID(attributeByTypeNameId1, false);
        if (attributeById != null && !attributeById.IsContent)
        {
          IDBAttribute dbAttribute = dbObject.GetAttributeByID(attributeById.AttributeID) ?? dbObject.Attributes.AddAttribute(attributeById.AttributeID, false);
          try
          {
            dbAttribute.AsDateTime = signDate;
          }
          catch
          {
          }
        }
      }
      int attributeByTypeNameId2 = MetaDataHelper.GetAttributeByTypeNameID(string.Format(LocalizationHolder.rm.GetString("SignRankAttrName"), (object) str));
      if (attributeByTypeNameId2 > 0)
      {
        IDBAttributeType4 attributeById = attributes.GetAttributeByID(attributeByTypeNameId2, false);
        if (attributeById != null && !attributeById.IsContent)
        {
          IDBAttribute dbAttribute = dbObject.GetAttributeByID(attributeById.AttributeID) ?? dbObject.Attributes.AddAttribute(attributeById.AttributeID, false);
          try
          {
            dbAttribute.AsInteger = rankID;
          }
          catch
          {
          }
        }
      }
      int attributeByTypeNameId3 = MetaDataHelper.GetAttributeByTypeNameID(string.Format(LocalizationHolder.rm.GetString("SignHashAttrName"), (object) str));
      if (attributeByTypeNameId3 <= 0)
        return;
      IDBAttributeType4 attributeById1 = attributes.GetAttributeByID(attributeByTypeNameId3, false);
      if (attributeById1 == null || attributeById1.IsContent)
        return;
      IDBAttribute dbAttribute1 = dbObject.GetAttributeByID(attributeById1.AttributeID) ?? dbObject.Attributes.AddAttribute(attributeById1.AttributeID, false);
      try
      {
        if ((long) signHash.Length > attributeById1.SizeType)
          dbAttribute1.AsString = signHash.Substring(0, Convert.ToInt32(attributeById1.SizeType));
        else
          dbAttribute1.AsString = signHash;
      }
      catch
      {
      }
    }
    finally
    {
      sessionTemporaryClone?.Logout("SignsServer.AddUserInfo");
    }
  }

  internal long UpdateSignsHashesCustom(Guid sessionGuid, bool onlyOldVersion, out string message)
  {
    message = string.Empty;
    UserSession sessionById = UserSession.GetSessionByID(sessionGuid) as UserSession;
    if (!sessionById.IsAdmin)
    {
      message = "Недостаточно прав для выполнения операции";
      return -1;
    }
    long num1 = 0;
    long num2 = 0;
    IDBObjectCollection objectCollection = sessionById.GetObjectCollection(SignsHolder.SignObjectTypeID);
    if (objectCollection != null)
    {
      DBRecordSetParams paramSet = new DBRecordSetParams((ConditionStructure[]) null, new object[1]
      {
        (object) ObligatoryObjectAttributes.F_OBJECT_ID
      });
      foreach (DataRow row in (InternalDataCollectionBase) objectCollection.Select(paramSet).Rows)
      {
        IDBObject signObject = sessionById.GetObject(Convert.ToInt64(row[0]), false);
        if (signObject != null)
        {
          ++num2;
          IDBAttribute attributeById1 = signObject.GetAttributeByID(SignsHolder.SignVersionAttrTypeID);
          if (attributeById1 == null)
          {
            sessionById.EventLog.AddToTrace($"Не найден атрибут {SignsHolder.SignVersionAttrTypeGuid.ToString()} у подписи VerId={signObject.ObjectID}", 0, string.Empty);
          }
          else
          {
            long asInteger = attributeById1.AsInteger;
            if (!onlyOldVersion || onlyOldVersion && asInteger < (long) sessionById.AlgorithmVersion)
            {
              IDBAttribute attributeById2 = signObject.GetAttributeByID(SignsHolder.HashProtectionAttrTypeID);
              if (attributeById2 == null)
              {
                sessionById.EventLog.AddToTrace($"Не найден атрибут {SignsHolder.HashProtectionAttrTypeGuid.ToString()} у подписи VerId={signObject.ObjectID}", 0, string.Empty);
              }
              else
              {
                byte[] inArray;
                try
                {
                  inArray = HashPack.CalcHash(HashPack.GetHashPack(signObject));
                }
                catch (Exception ex)
                {
                  sessionById.EventLog.AddToTrace($"Ошибка во время расчета хэша подписи VerId={signObject.ObjectID}: {ex.Message}{Environment.NewLine}{ex.StackTrace}", 0, string.Empty);
                  continue;
                }
                sessionById.StartTransaction();
                try
                {
                  attributeById1.Value = (object) sessionById.AlgorithmVersion;
                  attributeById2.Value = (object) Convert.ToBase64String(inArray);
                  sessionById.Commit();
                  ++num1;
                }
                catch (Exception ex)
                {
                  sessionById.Rollback();
                  sessionById.EventLog.AddToTrace($"Ошибка во время обновления атрибутов подписи VerId={signObject.ObjectID}: {ex.Message}{Environment.NewLine}{ex.StackTrace}", 0, string.Empty);
                }
              }
            }
          }
        }
      }
    }
    message = $"Всего подписей {num2}, обработано {num1}";
    return num1;
  }

  public long UpdateSignsHashes(Guid sessionGuid, out string message)
  {
    return this.UpdateSignsHashesCustom(sessionGuid, false, out message);
  }

  public void ConvertSignsToLastVersion(Guid sessionGuid)
  {
    string message = string.Empty;
    this.UpdateSignsHashesCustom(sessionGuid, true, out message);
  }

  public List<SignParams> GetObjectSignsParams(long objectId, Guid sessionGuid, bool canShowTime)
  {
    IUserSession sessionById = UserSession.GetSessionByID(sessionGuid);
    IDBRelationCollection relationCollection1 = sessionById.GetRelationCollection(SignsHolder.SignRelationTypeID);
    relationCollection1.ObjectTypeID = SignsHolder.SignObjectTypeID;
    DBRecordSetParams paramSet = new DBRecordSetParams((ConditionStructure[]) null, new ColumnDescriptor[6]
    {
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) SignsHolder.GraphAttrTypeID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) SignsHolder.SignUpAttrTypeID, AttributeSourceTypes.Object, ColumnContents.ID, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) SignsHolder.DateOfSignatureID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) SignsHolder.RankAttrTypeID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) SignsHolder.ModifyDateAttrTypeID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0)
    });
    DataTable dataTable = relationCollection1.ConsistFrom(paramSet, objectId);
    int count1 = dataTable.Rows.Count;
    IDBRelationCollection relationCollection2 = sessionById.GetRelationCollection(SignsHolder.SignRelationTypeID);
    relationCollection2.ObjectTypeID = SignsHolder.CryptoSignObjectTypeID;
    DataTable table = relationCollection2.ConsistFrom(paramSet, objectId);
    dataTable.Merge(table);
    List<SignParams> collection = new List<SignParams>();
    if (dataTable.Rows.Count > 0)
    {
      for (int index1 = 0; index1 < dataTable.Rows.Count; ++index1)
      {
        DataRow row = dataTable.Rows[index1];
        long int64_1 = Convert.ToInt64(row[-2.ToString()]);
        IDBObject dbObject = sessionById.GetObject(int64_1);
        DateTime dateTime1 = Convert.ToDateTime(row[SignsHolder.ModifyDateAttrTypeID.ToString()]);
        DateTime asDateTime = sessionById.GetObject(objectId).GetAttributeByID(SignsHolder.ModifyDateAttrTypeID).AsDateTime;
        X509Certificate2Collection certificates;
        SignStatuses signStatus = SignHelper.TranslateStatus(sessionById, objectId, dbObject.ObjectID, dbObject.ObjectType, asDateTime, dateTime1, SignsHolder.CryptoSignObjectTypeID, out certificates);
        IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(SignsHolder.GraphAttrTypeID);
        object obj = row[SignsHolder.GraphAttrTypeID.ToString()];
        int index2 = attributeType.PossibleValues.IndexOf(obj);
        if (index2 == -1)
          throw new KernelException(string.Format(LocalizationHolder.rm.GetString("Signs.Server_36"), obj));
        string str = Convert.ToString(attributeType.PossibleValuesDescriptions[index2]);
        string signParam1 = SignsService.GetSignParam(SignsHolder.SignGraphNameParam, str);
        string signParam2 = SignsService.GetSignParam(SignsHolder.SignSurnameParam, str);
        long int64_2 = Convert.ToInt64(row[SignsHolder.SignUpAttrTypeID.ToString()]);
        string surnameForSigning = SignsService.GetSurnameForSigning(sessionById, int64_2);
        string signParam3 = SignsService.GetSignParam(SignsHolder.SignDateParam, str);
        DateTime signDate;
        DateTime dateTime2;
        if (canShowTime)
        {
          signDate = Convert.ToDateTime(row[SignsHolder.DateOfSignatureID.ToString()]);
        }
        else
        {
          dateTime2 = Convert.ToDateTime(row[SignsHolder.DateOfSignatureID.ToString()]);
          signDate = dateTime2.Date;
        }
        dateTime2 = Convert.ToDateTime(row[SignsHolder.DateOfSignatureID.ToString()]);
        string signDateAsFormattedString = dateTime2.ToString(SignsHolder.SignDateFormatParam);
        string signParam4 = SignsService.GetSignParam(SignsHolder.SignValueParam, str);
        string rank = Convert.ToString(row[SignsHolder.RankAttrTypeID.ToString()]);
        string signParam5 = SignsService.GetSignParam(SignsHolder.SignRankParam, str);
        string signText = SignsService.GetSignText(signStatus, certificates);
        SignParams signParams1 = new SignParams(signParam2, surnameForSigning, signParam4, signText, signParam3, signDate, signParam1, str, signParam5, rank, int64_1, index1 < count1 ? SignsHolder.SignObjectTypeID : SignsHolder.CryptoSignObjectTypeID, dateTime1, signStatus, signDateAsFormattedString);
        bool flag = signParams1.SignStatus == SignStatuses.CryptoSignOutOfDate || signParams1.SignStatus == SignStatuses.SignOutOfDate || signParams1.SignStatus == SignStatuses.SignIncorrect;
        if (collection.Count == 0)
        {
          collection.Add(signParams1);
        }
        else
        {
          int count2 = collection.Count;
          List<SignParams> signParamsList = new List<SignParams>((IEnumerable<SignParams>) collection);
          for (int index3 = 0; index3 < count2; ++index3)
          {
            SignParams signParams2 = collection[index3];
            string graphName = signParams2.GraphName;
            if (signParams1.GraphName == graphName)
            {
              int num = signParams2.SignStatus == SignStatuses.CryptoSignOutOfDate || signParams2.SignStatus == SignStatuses.SignOutOfDate ? 1 : (signParams2.SignStatus == SignStatuses.SignIncorrect ? 1 : 0);
              if (num != 0 && !flag)
                signParamsList.Remove(signParams2);
              if (num == 0 & flag)
                break;
            }
            if (index3 + 1 == count2)
              signParamsList.Add(signParams1);
          }
          collection = signParamsList;
        }
      }
    }
    return collection;
  }

  private static string GetSurnameForSigning(IUserSession session, long subscrID)
  {
    string surnameForSigning = string.Empty;
    IDBObject dbObject = session.GetObject(subscrID, false);
    if (dbObject != null)
    {
      IDBAttribute attributeById = dbObject.GetAttributeByID(SignsHolder.SurnameForSignAttrTypeID);
      surnameForSigning = attributeById == null || string.IsNullOrWhiteSpace(attributeById.AsString) ? dbObject.Caption : attributeById.AsString;
    }
    return surnameForSigning;
  }

  private static string GetSignText(
    SignStatuses signStatus,
    X509Certificate2Collection certificates)
  {
    string signText = string.Empty;
    switch (signStatus)
    {
      case SignStatuses.CryptoSignOutOfDate:
        signText = SignsHolder.TextForNonActualQualifiedSign;
        break;
      case SignStatuses.CryptoSignActual:
        if (SignsHolder.QualifiedSignDisplayMode == SignsHolder.SignDisplayMode.Text)
        {
          signText = SignsHolder.TextForActualQualifiedSign;
          break;
        }
        if (certificates.Count > 0)
        {
          string publicKeyString = certificates[0].GetPublicKeyString();
          if (publicKeyString != null)
          {
            signText = publicKeyString.Length >= (int) SignsHolder.QualifiedSignKeyLastSymbolsNumber ? publicKeyString.Substring(publicKeyString.Length - (int) SignsHolder.QualifiedSignKeyLastSymbolsNumber) : publicKeyString;
            break;
          }
          break;
        }
        break;
      case SignStatuses.SignOutOfDate:
        signText = SignsHolder.TextForNonActualSimpleSign;
        break;
      case SignStatuses.SignActual:
        signText = SignsHolder.TextForActualSimpleSign;
        break;
      case SignStatuses.SignIncorrect:
        signText = SignsHolder.TextForNonActualSimpleSign;
        break;
    }
    return signText;
  }

  private static string GetSignParam(string param, string signGraph)
  {
    string oldValue = LocalizationHolder.rm.GetString("SignGraph");
    if (SignsHolder.SignSurnameParam.Contains(oldValue))
      param = param.Replace(oldValue, signGraph);
    return param;
  }

  public void SaveSignsParams(Guid sessionGuid)
  {
    IUserSession sessionById = UserSession.GetSessionByID(sessionGuid);
    SignsHolder.DoRevocation = sessionById.Configurations.ReadBool("SIGNS", "CERTIFICATES", "ONLINE_REVOCATION", false, DBConfigMode.GlobalOnly);
    SignsHolder.RevocationMode = sessionById.Configurations.ReadBool("SIGNS", "CERTIFICATES", "ONLINE_MODE_REVOCATION", true, DBConfigMode.GlobalOnly) ? X509RevocationMode.Online : X509RevocationMode.Offline;
    SignsHolder.CertificateSigningOnlyMode = true;
    SignsHolder.CopySignsToVersionMode = sessionById.Configurations.ReadBool("SIGNS", "CERTIFICATES", "COPY_SIGNS_TO_VERSION", false, DBConfigMode.GlobalOnly);
    SignsHolder.СheckExistingCopyActualityMode = sessionById.Configurations.ReadBool("SIGNS", "CERTIFICATES", "CHECK_COPY_ACTUALITY", false, DBConfigMode.GlobalOnly);
    SignsHolder.TextForActualSimpleSign = sessionById.Configurations.ReadString("SIGNS", "SIGNDISPLAYING", "TEXT_FOR_ACTUAL_SIMPLE_SIGN", "<Подп.>", DBConfigMode.GlobalOnly);
    SignsHolder.TextForNonActualSimpleSign = sessionById.Configurations.ReadString("SIGNS", "SIGNDISPLAYING", "TEXT_FOR_NON_ACTUAL_SIMPLE_SIGN", "???", DBConfigMode.GlobalOnly);
    SignsHolder.QualifiedSignDisplayMode = (SignsHolder.SignDisplayMode) sessionById.Configurations.ReadInteger("SIGNS", "SIGNDISPLAYING", "QUALIFIED_SIGN_DISPLAY_MODE", 0L, DBConfigMode.GlobalOnly);
    SignsHolder.TextForActualQualifiedSign = sessionById.Configurations.ReadString("SIGNS", "SIGNDISPLAYING", "TEXT_FOR_ACTUAL_QUALIFIED_SIGN", "<Подп.>", DBConfigMode.GlobalOnly);
    SignsHolder.QualifiedSignKeyLastSymbolsNumber = (uint) sessionById.Configurations.ReadInteger("SIGNS", "SIGNDISPLAYING", "SIGN_KEY_LAST_SYMBOLS_NUMBER", 20L, DBConfigMode.GlobalOnly);
    SignsHolder.TextForNonActualQualifiedSign = sessionById.Configurations.ReadString("SIGNS", "SIGNDISPLAYING", "TEXT_FOR_NON_ACTUAL_QUAL_SIGN", "???", DBConfigMode.GlobalOnly);
    SignsHolder.CheckActualSignMadeBySameUser = sessionById.Configurations.ReadBool("SIGNS", "CERTIFICATES", "CHECK_ACTUAL_SIGN_BY_SAME_USER", false, DBConfigMode.GlobalOnly);
  }

  public void SaveOutputParams(Guid sessionGuid)
  {
    SignsHolder.SignsOutputParametersInit(UserSession.GetSessionByID(sessionGuid));
  }
}
