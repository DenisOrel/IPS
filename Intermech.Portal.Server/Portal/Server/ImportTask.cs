// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.ImportTask
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Portal;
using Intermech.Interfaces.Server;
using Intermech.Interfaces.WebPortal;
using Intermech.IO;
using Intermech.Kernel;
using Intermech.Kernel.Services.PortalServices;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;

#nullable disable
namespace Intermech.Portal.Server;

internal class ImportTask
{
  private readonly bool _inAnotherThread;

  private ImportTasksDictionary _importTasksDictionary { get; }

  private IUserSession _session { get; }

  private SiteInfo _info { get; }

  private Guid _updateGuid { get; }

  private long[] _rootObjectIDs { get; }

  private string[] _filteredTypes { get; }

  private bool _ownBegin { get; }

  private bool _autoUpdate { get; }

  private int _countLevels { get; }

  private bool _forUpdate { get; }

  private bool _skipNotOwned { get; }

  public ImportTask(
    ImportTasksDictionary importTasksDictionary,
    IUserSession session,
    SiteInfo info,
    Guid updateGuid,
    long[] rootObjectIDs,
    string[] filteredTypes,
    bool ownBegin,
    bool autoUpdate,
    int countLevels,
    bool forUpdate,
    bool skipNotOwned,
    bool inAnotherThread)
  {
    if (rootObjectIDs == null || rootObjectIDs.Length == 0)
      throw new ArgumentException(LocalizationHolder.rm.GetString("PortalServer_10"));
    this._importTasksDictionary = importTasksDictionary;
    this._session = session;
    this._info = info;
    this._updateGuid = updateGuid;
    this._rootObjectIDs = rootObjectIDs;
    this._filteredTypes = filteredTypes;
    this._ownBegin = ownBegin;
    this._autoUpdate = autoUpdate;
    this._countLevels = countLevels;
    this._forUpdate = forUpdate;
    this._skipNotOwned = skipNotOwned;
    this._inAnotherThread = inAnotherThread;
  }

  public void Import()
  {
    bool flag = false;
    List<string> stringList = new List<string>();
    try
    {
      if (this._importTasksDictionary != null)
      {
        this._importTasksDictionary.SetStatus(this._updateGuid, ImportTaskStatuses.Working);
        this._importTasksDictionary.SetPersent(this._updateGuid, 1);
      }
      List<Tuple<long, bool>> objects = new List<Tuple<long, bool>>();
      List<Tuple<Guid, long>> relations = new List<Tuple<Guid, long>>();
      CompositionHelper.GetComposition(this._session, this._rootObjectIDs, this._filteredTypes, objects, relations, this._countLevels);
      if (this._importTasksDictionary != null)
        this._importTasksDictionary.SetPersent(this._updateGuid, 30);
      List<TransferedObject> data = this.MakeImportPacket(this._session, this._info, this._rootObjectIDs, objects, relations, this._ownBegin, this._autoUpdate, this._forUpdate, this._skipNotOwned, stringList, this._countLevels);
      if (TraceLog.Enabled && !this._inAnotherThread)
        TraceLog.Write("Create update");
      if (this._importTasksDictionary != null)
        this._importTasksDictionary.SetPersent(this._updateGuid, 60);
      long[] siteIDs = new long[1]{ this._info.ID };
      string authorID = this._info.Code.ToString();
      new SiteUpdate(data, siteIDs, authorID).SaveIntoBase(this._session, this._updateGuid);
      if (this._importTasksDictionary == null)
        return;
      this._importTasksDictionary.SetStatus(this._updateGuid, ImportTaskStatuses.Completed);
    }
    catch (Exception ex)
    {
      if (this._importTasksDictionary != null)
      {
        this._importTasksDictionary.SetError(this._updateGuid, ex);
        flag = true;
      }
      this.DeleteFiles(this._session, stringList);
      if (this._inAnotherThread)
      {
        string TraceFileName = $"error_{this._updateGuid}_{DateTime.Now.Ticks}.xml";
        this._session.EventLog.AddToTrace(LogException.Create(ex), Intermech.Consts.traceAlways, TraceFileName);
        this._session.EventLog.AddToTrace(string.Format(LocalizationHolder.rm.GetString("PortalServer_11"), (object) this._updateGuid, (object) ex.Message, (object) TraceFileName), Intermech.Consts.traceAlways, string.Empty);
      }
      else
        throw;
    }
    finally
    {
      if (this._inAnotherThread)
        this._session.Logout($"ImportTask_{this._updateGuid}");
      if (!flag && this._importTasksDictionary != null)
        this._importTasksDictionary.RemoveTask(this._updateGuid);
    }
  }

  private void DeleteFiles(IUserSession session, List<string> files)
  {
    if (files == null || files.Count <= 0)
      return;
    foreach (string file in files)
    {
      try
      {
        File.Delete(file);
      }
      catch (Exception ex)
      {
        session.EventLog.AddToTrace($"Ошибка при удалении созданных файлов при возникновении исключения в импорте: {LogException.Create(ex)}", Intermech.Consts.traceAlways, string.Empty);
      }
    }
  }

  private List<TransferedObject> MakeImportPacket(
    IUserSession session,
    SiteInfo info,
    long[] rootObjectIDs,
    List<Tuple<long, bool>> objects,
    List<Tuple<Guid, long>> relations,
    bool ownBegin,
    bool autoUpdate,
    bool forUpdate,
    bool skipNotOwned,
    List<string> createdFiles,
    int countLevels)
  {
    List<TransferedObject> trObjects = new List<TransferedObject>();
    List<long> linkObjects = new List<long>();
    if (TraceLog.Enabled && !this._inAnotherThread)
      TraceLog.Write("ImportedObjects:");
    List<char> charList = (List<char>) null;
    PortalSettings service = (PortalSettings) ServerServices.GetService(typeof (PortalSettings));
    if (!service.SitesSystemTypesIgnore)
    {
      SiteInfo[] sitesFromDb = SiteInfoHelper.GetSitesFromDB(session, info.SystemType);
      charList = new List<char>(sitesFromDb.Length);
      foreach (SiteInfo siteInfo in sitesFromDb)
        charList.Add(siteInfo.Code);
    }
    for (int index = 0; index < objects.Count; ++index)
    {
      IDBObject attributable = session.GetObject(objects[index].Item1);
      if (!service.SitesSystemTypesIgnore)
      {
        char ch = attributable.GetAttributeByGuid(PortalConsts.attributeFirstPublishSite).AsString[0];
        if (!charList.Contains(ch))
          throw new Exception($"{attributable.NameInMessages} не может быть импортирован, так как он был создан в другой системе.");
      }
      if (TraceLog.Enabled && !this._inAnotherThread)
        TraceLog.Write($"ObjectID={attributable.ObjectID} ObjectType={attributable.ObjectType}");
      if (ownBegin)
      {
        if (!ActionsHelper.SetOwner(info, session, attributable, !skipNotOwned))
          ActionsHelper.AddSiteCode(info, attributable);
      }
      else if (autoUpdate | forUpdate)
        ActionsHelper.AddSiteCode(info, attributable);
      IDBAttribute attributeByGuid1 = attributable.GetAttributeByGuid(PortalConsts.attributeSitesForUpdate);
      if (attributeByGuid1 != null && !attributeByGuid1.IsNull)
      {
        string asString = attributeByGuid1.AsString;
        if (asString.IndexOf(info.Code) >= 0)
          attributeByGuid1.AsString = asString.Replace(info.Code.ToString(), string.Empty);
      }
      this.AddLinkIntoImportPaket(session, info, (IDBAttributable) attributable, trObjects, objects, linkObjects, createdFiles);
      string dirName;
      TransferedObject transferedObject = ImportTask.SaveObjectInfoDisk(info, (IDBAttributable) attributable, ChangeType.ctUpdate, TransferedObjectCategory.Object, ownBegin, out dirName);
      ObjectTag tag = (ObjectTag) transferedObject.Tag;
      tag.InComposition = Array.IndexOf<long>(rootObjectIDs, objects[index].Item1) < 0;
      tag.WithComposition = objects[index].Item2;
      trObjects.Add(transferedObject);
      ImportTask.AddToCreatedFiles(createdFiles, transferedObject, dirName);
      IDBAttribute attributeByGuid2 = attributable.GetAttributeByGuid(PortalConsts.attributeImportedSites);
      if (!attributeByGuid2.AsString.Contains(info.Code.ToString()))
        attributeByGuid2.AsString += info.Code.ToString();
      this.AddImportEvent(session, info, 1, attributable.ObjectID, attributable.NameInMessages, string.Empty);
    }
    if (TraceLog.Enabled && !this._inAnotherThread)
      TraceLog.Write("ImportedRelations:");
    foreach (Tuple<Guid, long> relation1 in relations)
    {
      IDBRelation relation = relation1.Item2 != 0L ? session.GetRelation(relation1.Item1, relation1.Item2) : session.GetRelation(relation1.Item1, false);
      if (relation == null)
      {
        if (TraceLog.Enabled && !this._inAnotherThread)
          TraceLog.Write("Не найдена!");
      }
      else
      {
        if (TraceLog.Enabled && !this._inAnotherThread)
          TraceLog.Write($"GUID={relation.GUID} ProjID={relation.ProjID} PartID={relation.PartID}");
        if (objects.Exists((Predicate<Tuple<long, bool>>) (_ => _.Item1.Equals(relation.ProjID))))
        {
          this.AddLinkIntoImportPaket(session, info, (IDBAttributable) relation, trObjects, objects, linkObjects, createdFiles);
          string dirName;
          TransferedObject transferedObject = ImportTask.SaveObjectInfoDisk(info, (IDBAttributable) relation, ChangeType.ctUpdate, TransferedObjectCategory.Relation, false, out dirName);
          trObjects.Add(transferedObject);
          ImportTask.AddToCreatedFiles(createdFiles, transferedObject, dirName);
          this.AddImportEvent(session, info, 5, relation.RelationID, $"Связь {relation.GUID} между {relation.ProjID} и {relation.PartID}", string.Empty);
        }
      }
    }
    return trObjects;
  }

  private static void AddToCreatedFiles(
    List<string> createdFiles,
    TransferedObject obj,
    string dirName)
  {
    if (obj.DataFiles == null || obj.DataFiles.Length == 0)
      return;
    foreach (string dataFile in obj.DataFiles)
      createdFiles.Add(Path.Combine(dirName, dataFile));
  }

  public static TransferedObject SaveObjectInfoDisk(
    SiteInfo info,
    IDBAttributable attributable,
    ChangeType сhangeType,
    TransferedObjectCategory category,
    bool saveEnableSites,
    out string dirName)
  {
    return ImportTask.SaveObjectInfoDisk(info, attributable, сhangeType, category, Guid.NewGuid(), saveEnableSites, out dirName);
  }

  public static TransferedObject SaveObjectInfoDisk(
    SiteInfo info,
    IDBAttributable attributable,
    ChangeType сhangeType,
    TransferedObjectCategory category,
    Guid unitGuid,
    bool saveEnableSites,
    out string dirName)
  {
    TransferedObject transferedObject = new TransferedObject(unitGuid, сhangeType, category, (TransferedObjectTag) null);
    if (category == TransferedObjectCategory.Object || category == TransferedObjectCategory.ObjectLink)
    {
      ObjectTag objectTag = new ObjectTag()
      {
        CreatorCode = attributable.GetAttributeByGuid(PortalConsts.attributeFirstPublishSite).AsString[0]
      };
      string asString = attributable.GetAttributeByGuid(PortalConsts.attributeOwner).AsString;
      if (!string.IsNullOrEmpty(asString))
        objectTag.OwnerCode = new char?(asString[0]);
      IDBAttribute attributeByGuid = attributable.GetAttributeByGuid(PortalConsts.attributeCompositionOwner);
      string str = attributeByGuid != null ? attributeByGuid.AsString : string.Empty;
      if (!string.IsNullOrEmpty(str))
        objectTag.CompositionOwnerCode = new char?(str[0]);
      transferedObject.Tag = (TransferedObjectTag) objectTag;
      if (saveEnableSites)
        objectTag.EnableSites = attributable.GetAttributeByGuid(PortalConsts.attributeEnabledSites).AsString;
    }
    dirName = TempStorage.GetUpdateUnitPath(transferedObject.GUID);
    Directory.CreateDirectory(dirName);
    List<RemarkInfo> remarkInfoList = (List<RemarkInfo>) null;
    if (attributable is IDBObject)
    {
      IDBAttribute attributeByGuid = (attributable as IDBObject).GetAttributeByGuid(PortalConsts.attributeRemarkList);
      if (attributeByGuid != null && attributeByGuid.ValuesCount > 0)
      {
        remarkInfoList = new List<RemarkInfo>(attributeByGuid.ValuesCount);
        for (int index = 0; index < attributeByGuid.ValuesCount; ++index)
        {
          attributeByGuid.Index = index;
          if (!attributeByGuid.IsNull)
          {
            IBlobReader blobReader = attributeByGuid as IBlobReader;
            blobReader.OpenBlob(0);
            try
            {
              using (MemoryStream serializationStream = new MemoryStream(blobReader.ReadDataBlock()))
              {
                RemarkInfo remarkInfo = (RemarkInfo) new BinaryFormatter().Deserialize((Stream) serializationStream);
                if (remarkInfo.EnableSites != string.Empty)
                {
                  if (remarkInfo.EnableSites != null)
                  {
                    if (remarkInfo.EnableSites.IndexOf(info.Code) >= 0)
                      remarkInfoList.Add(remarkInfo);
                  }
                }
              }
            }
            finally
            {
              blobReader.CloseBlob();
            }
          }
        }
      }
    }
    IDBAttribute attributeByGuid1 = attributable.GetAttributeByGuid(PortalServerConsts.attributeFile);
    List<string> dataFiles = new List<string>(attributeByGuid1.ValuesCount);
    if (attributeByGuid1.ValuesCount > 0)
    {
      ServiceUtils.GetService<IPackedStream>((object) ApplicationServices.Container, true);
      for (int index1 = 0; index1 < attributeByGuid1.ValuesCount; ++index1)
      {
        attributeByGuid1.Index = index1;
        IBlobReader bReader = attributeByGuid1 as IBlobReader;
        BlobInformation blobInfo = bReader.OpenBlob(0);
        try
        {
          TempStorage.CheckAndCreateLocDirectory(dirName, blobInfo.FileName);
          string str = Path.Combine(dirName, blobInfo.FileName);
          if (blobInfo.FileName == PortalConsts.AttributesXmlFileName && remarkInfoList != null && remarkInfoList.Count > 0)
          {
            XmlDocument xmlDocument = new XmlDocument();
            using (MemoryStream inStream = new MemoryStream(bReader.ReadDataBlock(0)))
            {
              using (ImChunkedStream imChunkedStream = new ImChunkedStream())
              {
                ZLibStreamHelper.UnpackStream((Stream) inStream, (Stream) imChunkedStream);
                imChunkedStream.Position = 0L;
                xmlDocument.Load((Stream) imChunkedStream);
              }
            }
            XmlNode xmlNode = (XmlNode) null;
            for (int i = 0; i < xmlDocument.ChildNodes.Count; ++i)
            {
              if (xmlDocument.ChildNodes[i].Name == PortalConsts.XmlRootNodeAttributes)
              {
                xmlNode = xmlDocument.ChildNodes[i];
                break;
              }
            }
            for (int index2 = 0; index2 < remarkInfoList.Count; ++index2)
            {
              XmlNode remarkAttributeNode = XMLFileHelper.CreateRemarkAttributeNode(xmlDocument, remarkInfoList[index2]);
              if (remarkInfoList[index2].Values.Count > 0)
              {
                for (int index3 = 0; index3 < remarkInfoList[index2].Values.Count; ++index3)
                {
                  ValueInfo valueInfo = remarkInfoList[index2].Values[index3];
                  XmlNode valueNode = XMLFileHelper.CreateValueNode(xmlDocument, valueInfo.Index);
                  XMLFileHelper.AddStringAttribute(xmlDocument, valueNode, valueInfo.StringValue);
                  XMLFileHelper.AddDateTimeAttribute(xmlDocument, valueNode, valueInfo.DateValue);
                  XMLFileHelper.AddIntegerAttribute(xmlDocument, valueNode, valueInfo.IntValue);
                  XMLFileHelper.AddDoubleAttribute(xmlDocument, valueNode, valueInfo.FloatValue);
                  if (remarkInfoList[index2].FieldType == FieldTypes.ftFile || remarkInfoList[index2].FieldType == FieldTypes.ftMemo || remarkInfoList[index2].FieldType == FieldTypes.ftShortBlob || remarkInfoList[index2].FieldType == FieldTypes.ftBlob)
                  {
                    XMLFileHelper.AddAttribute(xmlDocument, valueNode, "F_ARC_METHOD", Convert.ToString((int) valueInfo.ArcMethod));
                    XMLFileHelper.AddAttribute(xmlDocument, valueNode, "F_FILE", valueInfo.FileName);
                    XMLFileHelper.AddAttribute(xmlDocument, valueNode, "F_FILE_TYPE", Convert.ToString((int) valueInfo.FileType));
                    XMLFileHelper.AddAttribute(xmlDocument, valueNode, "F_FILE_AUTHOR", valueInfo.FileAuthor);
                  }
                  remarkAttributeNode.AppendChild(valueNode);
                }
              }
              xmlNode.AppendChild(remarkAttributeNode);
            }
            using (FileStream outStream = File.Open(str, FileMode.Create, FileAccess.ReadWrite))
            {
              using (ImChunkedStream imChunkedStream = new ImChunkedStream())
              {
                xmlDocument.Save((Stream) imChunkedStream);
                imChunkedStream.Position = 0L;
                ZLibStreamHelper.PackStream((Stream) imChunkedStream, ZLibCompressLevels.LevelMax, (Stream) outStream);
              }
              outStream.Flush();
              outStream.Close();
            }
            dataFiles.Add(blobInfo.FileName);
          }
          else
            ImportTask.WriteFile(str, bReader, blobInfo, dataFiles);
        }
        finally
        {
          bReader.CloseBlob();
        }
      }
    }
    if (remarkInfoList != null && remarkInfoList.Count > 0)
    {
      IDBAttribute attributeByGuid2 = (attributable as IDBObject).GetAttributeByGuid(PortalConsts.attributeRemarkFiles);
      for (int index4 = 0; index4 < remarkInfoList.Count; ++index4)
      {
        switch (remarkInfoList[index4].FieldType)
        {
          case FieldTypes.ftShortBlob:
          case FieldTypes.ftFile:
          case FieldTypes.ftMemo:
          case FieldTypes.ftBlob:
            if (remarkInfoList[index4].Values.Count > 0)
            {
              for (int index5 = 0; index5 < remarkInfoList[index4].Values.Count; ++index5)
              {
                ValueInfo valueInfo = remarkInfoList[index4].Values[index5];
                for (int index6 = 0; index6 < attributeByGuid2.ValuesCount; ++index6)
                {
                  attributeByGuid2.Index = index6;
                  if (attributeByGuid2.AsString.Equals(valueInfo.FileName))
                  {
                    IBlobReader bReader = attributeByGuid2 as IBlobReader;
                    BlobInformation blobInfo = bReader.OpenBlob(0);
                    try
                    {
                      TempStorage.CheckAndCreateLocDirectory(dirName, blobInfo.FileName);
                      ImportTask.WriteFile(Path.Combine(dirName, blobInfo.FileName), bReader, blobInfo, dataFiles);
                      break;
                    }
                    finally
                    {
                      bReader.CloseBlob();
                    }
                  }
                }
              }
              break;
            }
            break;
        }
      }
    }
    transferedObject.DataFiles = dataFiles.ToArray();
    return transferedObject;
  }

  private static void WriteFile(
    string fileName,
    IBlobReader bReader,
    BlobInformation blobInfo,
    List<string> dataFiles)
  {
    FileStream fileStream = File.Open(fileName, FileMode.Create, FileAccess.ReadWrite);
    try
    {
      if (blobInfo.RealFileSize <= 0L)
        return;
      byte[] buffer = bReader.ReadDataBlock(0);
      if (buffer == null)
        return;
      fileStream.Write(buffer, 0, buffer.Length);
    }
    finally
    {
      fileStream.Flush();
      fileStream.Close();
      dataFiles.Add(blobInfo.FileName);
    }
  }

  private void AddImportEvent(
    IUserSession session,
    SiteInfo info,
    int categoryType,
    long categoryID,
    string objectName,
    string note)
  {
    (session as UserSession).EventLogHelper.AddEvent(categoryType == 1 ? categoryID : 0L, categoryType == 5 ? categoryID : 0L, categoryType, categoryID, objectName, note, ActionType.ImportedFromPortal, EventlogRecordType.Information, session.UserID, info.Code.ToString(), session);
  }

  private void AddLinkIntoImportPaket(
    IUserSession session,
    SiteInfo info,
    IDBAttributable attributable,
    List<TransferedObject> trObjects,
    List<Tuple<long, bool>> packetObjects,
    List<long> linkObjects,
    List<string> createdFiles)
  {
    IDBAttribute attrLinks = attributable.GetAttributeByID(IDHelper.AttributePublishLinksID);
    if (attrLinks == null || attrLinks.ValuesCount <= 0)
      return;
    for (int index = 0; index < attrLinks.ValuesCount; ++index)
    {
      attrLinks.Index = index;
      if (!attrLinks.IsNull && attrLinks is IDBObjectLinkAttribute && !packetObjects.Exists((Predicate<Tuple<long, bool>>) (_ => _.Item1.Equals(attrLinks.AsInteger))) && !linkObjects.Contains(attrLinks.AsInteger))
      {
        IDBObject dbObject = (attrLinks as IDBObjectLinkAttribute).DBObject;
        linkObjects.Add(dbObject.ObjectID);
        this.AddLinkIntoImportPaket(session, info, (IDBAttributable) dbObject, trObjects, packetObjects, linkObjects, createdFiles);
        string dirName;
        TransferedObject transferedObject = ImportTask.SaveObjectInfoDisk(info, (IDBAttributable) dbObject, ChangeType.ctUpdate, TransferedObjectCategory.ObjectLink, false, out dirName);
        trObjects.Add(transferedObject);
        ImportTask.AddToCreatedFiles(createdFiles, transferedObject, dirName);
        this.AddImportEvent(session, info, 1, dbObject.ObjectID, $"{dbObject.Caption} ({dbObject.ObjectGUID})", $"Объект импортирован как ссылка из {(attributable is DBObject ? (object) "объекта" : (object) "связи")} {(attributable is DBObject ? ((DBSessionable) attributable).ObjectID : ((DBRelation) attributable).RelationID)}");
      }
    }
  }
}
