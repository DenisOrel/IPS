// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.ObjectPublisher
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Server.WebPortal;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel.Search;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;

#nullable disable
namespace Intermech.Portal.Server;

internal class ObjectPublisher : UnitPublisher
{
  private IDBImporter _importer;

  public ObjectPublisher(
    IUserSession session,
    TransferedObject unit,
    XmlDocument xmlDocument,
    XmlNode rootNode,
    SiteInfo info,
    IDBImporter importer)
    : base(session, unit, xmlDocument, rootNode, info, 1)
  {
    if (rootNode == null)
      throw new ArgumentNullException(nameof (rootNode));
    this._importer = importer;
  }

  public override Guid Publish(
    IDBObjectCollection publishObjects,
    string enabledSites,
    GroupPublishItem item,
    PackAnalyzInfo packAnalyzInfo,
    PublishCaches caches,
    IDBRelationCollection relCollection,
    IDBRelationType relTypePublish)
  {
    ObjectInfo objectAttributes = AttributesFile.GetObjectAttributes(this.rootNode);
    AnalyzObjectInfo analyzObjectInfo = packAnalyzInfo.AnalyzObjectInfo[objectAttributes.ObjectGuid];
    if (TraceLog.Enabled)
    {
      TraceLog.Write($"...ObjectPublisher: ObjectGuid={objectAttributes.ObjectGuid} Guid={objectAttributes.Guid} Caption={objectAttributes.Caption} LinkedGuid={objectAttributes.LinkedGuid} ParentGuid={objectAttributes.ParentGuid} ObjTypeName={objectAttributes.ObjTypeName} ObjectTypeGuid={objectAttributes.ObjectTypeGuid}");
      TraceLog.Write($"...AnalyzObjectInfo: ID={analyzObjectInfo.ID} PublishEnable={analyzObjectInfo.PublishEnable} Deleted={analyzObjectInfo.Deleted} InComposition={analyzObjectInfo.InComposition} WithComposition={analyzObjectInfo.WithComposition}");
      TraceLog.Write($"...unit category={this.unit.Category}");
    }
    bool isNewObject = analyzObjectInfo.ID == 0L;
    IDBObjectType objectType = this.session.GetObjectType(objectAttributes.PublishObjectType == Guid.Empty ? PortalConsts.objtypePublishObjects : objectAttributes.PublishObjectType, true);
    IDBObject dbObject = (IDBObject) null;
    try
    {
      if (analyzObjectInfo.Deleted)
      {
        dbObject = this.session.GetObject(analyzObjectInfo.ID, true);
        dbObject.LCStep = this.session.GetLCSchema(objectType.SchemaID, true).GetStepsCollection().GetFirstStep();
        this.SetBaseVersion(dbObject, dbObject.IsBaseVersion);
        this.AddEvent(analyzObjectInfo.ID, this.LogName(objectAttributes), "Объект восстановлен");
        if (TraceLog.Enabled)
          TraceLog.Write("...object recovery");
      }
      if (!analyzObjectInfo.PublishEnable)
      {
        this.AddEvent(analyzObjectInfo.ID, this.LogName(objectAttributes), "Обновление запрещено");
        if (TraceLog.Enabled)
          TraceLog.Write("...object update denied");
        return analyzObjectInfo.GUID;
      }
      if (dbObject == null && analyzObjectInfo.ID != 0L)
        dbObject = this.session.GetObject(analyzObjectInfo.ID, true);
      IDBObject dbParent = (IDBObject) null;
      if (dbObject == null)
        dbObject = this.Create(this.session, objectType, publishObjects, objectAttributes, out dbParent);
      if (isNewObject)
      {
        if (TraceLog.Enabled)
          TraceLog.Write("...new object created");
        PublishHelper.SetSiteCodes(dbObject, this.unit.Tag as ObjectTag, this.unit.Category, enabledSites, this.info.Code.ToString(), packAnalyzInfo.SiteForUpdate, packAnalyzInfo.IsAutoTransfer, isNewObject, analyzObjectInfo.InComposition);
        PublishHelper.AddAtribute((IDBAttributable) dbObject, IDHelper.AttributePublishInCompositionID, (object) analyzObjectInfo.InComposition);
        if (objectAttributes.ObjectTypeGuid != Guid.Empty)
          PublishHelper.AddAtribute((IDBAttributable) dbObject, IDHelper.AttributeObjectTypeGuidID, (object) objectAttributes.ObjectTypeGuid);
        else
          PublishHelper.AddAtribute((IDBAttributable) dbObject, IDHelper.AttributeObjectTypeGuidID);
        PublishHelper.AddAtribute((IDBAttributable) dbObject, IDHelper.AttributeObjTypeNameID, (object) objectAttributes.ObjTypeName);
        PublishHelper.AddAtribute((IDBAttributable) dbObject, IDHelper.AttributePublishObjectGuidID, (object) objectAttributes.ObjectGuid);
        PublishHelper.AddAtribute((IDBAttributable) dbObject, IDHelper.AttributePublishGuidID, (object) objectAttributes.Guid);
        PublishHelper.AddAtribute((IDBAttributable) dbObject, IDHelper.AttributeRootTypePublishObjectID, (object) (int) (this.unit.Tag as ObjectTag).RootType);
        if (!packAnalyzInfo.IsAutoTransfer && dbParent != null)
        {
          IDBAttribute attributeById = dbParent.GetAttributeByID(IDHelper.AttributeCopyKeepersID);
          if (attributeById != null && !attributeById.IsNull && attributeById.AsString != string.Empty)
          {
            PublishHelper.AddAtribute((IDBAttributable) dbObject, IDHelper.AttributeSitesForUpdateID, (object) attributeById.AsString);
            if (TraceLog.Enabled)
              TraceLog.Write($"...add AttributeSitesForUpdate={attributeById.AsString}");
          }
        }
        dbObject.CommitCreation(true);
      }
      else
      {
        if (TraceLog.Enabled)
          TraceLog.Write("...start update object");
        PublishHelper.SetSiteCodes(dbObject, this.unit.Tag as ObjectTag, this.unit.Category, enabledSites, this.info.Code.ToString(), packAnalyzInfo.SiteForUpdate, packAnalyzInfo.IsAutoTransfer, isNewObject, analyzObjectInfo.InComposition);
        IDBAttribute attributeById1 = dbObject.GetAttributeByID(IDHelper.AttributePublishInCompositionID);
        if (attributeById1 == null)
          dbObject.Attributes.AddAttribute(IDHelper.AttributePublishInCompositionID, false, new object[1]
          {
            (object) analyzObjectInfo.InComposition
          });
        else if (attributeById1.AsBoolean && !analyzObjectInfo.InComposition)
          attributeById1.AsBoolean = false;
        IDBAttribute attributeById2 = dbObject.GetAttributeByID(IDHelper.AttributeCopyKeepersID);
        if (!packAnalyzInfo.IsAutoTransfer && attributeById2 != null && attributeById2.AsString.Length > 0)
        {
          string str = attributeById2.AsString.Replace(Convert.ToString(this.info.Code), string.Empty);
          if (str.Length > 0)
          {
            PublishHelper.AddAtribute((IDBAttributable) dbObject, IDHelper.AttributeSitesForUpdateID, (object) str);
            if (TraceLog.Enabled)
              TraceLog.Write($"...add AttributeSitesForUpdate={str}");
          }
        }
      }
      if (objectAttributes.VerCode >= 0)
        PublishHelper.AddAtribute((IDBAttributable) dbObject, IDHelper.AttributeVerCodeID, (object) objectAttributes.VerCode);
      if (objectAttributes.LinkedGuid != Guid.Empty)
      {
        PublishHelper.AddAtribute((IDBAttributable) dbObject, IDHelper.AttributeLinkedGuidID, (object) objectAttributes.LinkedGuid);
        this.AddEvent(ActionType.EditProperties, analyzObjectInfo.ID, this.LogName(objectAttributes), $"Установлен GUID связанного объекта {objectAttributes.LinkedGuid}");
        if (TraceLog.Enabled)
          TraceLog.Write($"...add AttributeLinkedGuid={objectAttributes.LinkedGuid}");
      }
      else
      {
        IDBAttribute attributeById = dbObject.GetAttributeByID(IDHelper.AttributeLinkedGuidID);
        if (attributeById != null && !isNewObject && attributeById.AsString != string.Empty)
        {
          this.AddEvent(ActionType.EditProperties, analyzObjectInfo.ID, this.LogName(objectAttributes), $"Удален GUID связанного объекта {attributeById.AsString}");
          attributeById.Delete(0L);
          if (TraceLog.Enabled)
            TraceLog.Write("...remove AttributeLinkedGuid");
        }
      }
      if (objectType.CaptionAttribute == 0 && objectAttributes.Caption != dbObject.Caption)
        dbObject.Caption = objectAttributes.Caption;
      if (objectAttributes.BaseVersion && !dbObject.IsBaseVersion)
      {
        IDBObject objectBaseVersionById = this.session.GetObjectBaseVersionByID(dbObject.ID, false);
        if (objectBaseVersionById != null)
          this.SetBaseVersion(objectBaseVersionById, false);
        dbObject.MakeBaseVersion();
        if (TraceLog.Enabled)
          TraceLog.Write("...set new BaseVersion");
      }
      List<Guid> currentLinks = new List<Guid>();
      this.ParceXMLIntoAttributable(this.session, (IDBAttributable) dbObject, (IDBAttributableType) objectType, this.xmlDocument, this.rootNode, this.directoryName, caches.ImportedObjectsIDs, ref currentLinks);
      if (currentLinks.Count > 0)
        caches.ObjectsWithLinks.Add(dbObject.ObjectID);
      this.SetLinksAttribute(this.session, (IDBAttributable) dbObject, currentLinks);
      if (dbParent != null)
        this.CorrectCompositionRelations(relCollection, dbObject, objectAttributes);
      if (isNewObject)
      {
        if (dbParent != null)
          this.AddEvent(Math.Abs(dbObject.ObjectID), this.LogName(objectAttributes), $"Выпущена версия объекта {dbParent.ID}");
        else
          this.AddEvent(Math.Abs(dbObject.ObjectID), this.LogName(objectAttributes), "Создан новый объект");
      }
      else
        this.AddEvent(Math.Abs(dbObject.ObjectID), this.LogName(objectAttributes), "Обновлена версия существующего объекта");
      return dbObject.ObjectGUID;
    }
    finally
    {
      long partObjectID = 0;
      if (dbObject == null && analyzObjectInfo.ID != 0L)
        partObjectID = analyzObjectInfo.ID;
      else if (dbObject != null)
        partObjectID = dbObject.ObjectID;
      if (partObjectID != 0L)
      {
        caches.ImportedObjectsIDs.Add(objectAttributes.ObjectGuid, partObjectID);
        if (isNewObject)
          analyzObjectInfo.ID = partObjectID;
        if (this.unit.Category == TransferedObjectCategory.Object)
          caches.Objects.Add(partObjectID);
        if (item != null)
          this.session.GetRelationCollection(this.session.IdentHelper.SimpleRelationTypeID).Create(item.DBObject.ObjectID, partObjectID, DateTime.Now);
      }
    }
  }

  private void CorrectCompositionRelations(
    IDBRelationCollection relCollection,
    IDBObject dbUnit,
    ObjectInfo oi)
  {
    try
    {
      DataTable dataTable = relCollection.ConsistFrom(new DBRecordSetParams((ConditionStructure[]) null, new object[1]
      {
        (object) -20
      }), dbUnit.ObjectID);
      if (dataTable == null || dataTable.Rows.Count <= 0)
        return;
      for (int index1 = 0; index1 < dataTable.Rows.Count; ++index1)
      {
        IDBRelation relation = this.session.GetRelation(Convert.ToInt64(dataTable.Rows[index1][0]));
        if (TraceLog.Enabled)
          TraceLog.Write($"...relation {relation.GUID} correct");
        IDBAttribute attributeByGuid = relation.GetAttributeByGuid(PortalServerConsts.attributeFile);
        if (attributeByGuid != null && attributeByGuid.ValuesCount > 0)
        {
          for (int index2 = 0; index2 < attributeByGuid.ValuesCount; ++index2)
          {
            attributeByGuid.Index = index2;
            if (!(attributeByGuid.AsString != PortalConsts.AttributesXmlFileName))
            {
              IPackedStream service = ServiceUtils.GetService<IPackedStream>((object) ApplicationServices.Container, true);
              MemoryStream memoryStream1 = new MemoryStream();
              try
              {
                IBlobReader blobReader = attributeByGuid as IBlobReader;
                BlobInformation blobInformation = blobReader.OpenBlob(0);
                MemoryStream memoryStream2 = new MemoryStream();
                BlobInformation blobInfo = new BlobInformation();
                try
                {
                  byte[] buffer = blobReader.ReadDataBlock(0);
                  if (buffer != null)
                    memoryStream1.Write(buffer, 0, buffer.Length);
                  XmlDocument xmlDocument = new XmlDocument();
                  if (blobInformation.ArcMethod == ArcMethods.ZLibPacked)
                  {
                    service.UnpackStream((Stream) memoryStream2, (Stream) memoryStream1);
                    memoryStream2.Position = 0L;
                    xmlDocument.Load((Stream) memoryStream2);
                  }
                  else
                  {
                    memoryStream1.Position = 0L;
                    xmlDocument.Load((Stream) memoryStream1);
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
                  if (xmlNode == null)
                    throw new Exception(LocalizationHolder.rm.GetString("PortalServer_35"));
                  bool flag = false;
                  for (int i = 0; i < xmlNode.ChildNodes.Count; ++i)
                  {
                    if (xmlNode.ChildNodes[i].Name == PortalConsts.XmlNodeSysAttribute && xmlNode.ChildNodes[i].Attributes["F_PROJECT_GUID"] != null)
                    {
                      xmlNode.ChildNodes[i].Attributes["F_PROJECT_GUID"].Value = oi.ObjectGuid.ToString();
                      flag = true;
                      break;
                    }
                  }
                  if (!flag)
                    throw new Exception(LocalizationHolder.rm.GetString("PortalServer_36"));
                  memoryStream2.Position = 0L;
                  xmlDocument.Save((Stream) memoryStream2);
                  service.PackStream((Stream) memoryStream1, (Stream) memoryStream2, 9);
                  blobInfo.RealFileSize = memoryStream2.Length;
                }
                finally
                {
                  memoryStream2.Close();
                  blobReader.CloseBlob();
                }
                blobInfo.ModifyDate = DateTime.Now;
                blobInfo.ArcMethod = ArcMethods.ZLibPacked;
                blobInfo.PackedFileSize = memoryStream1.Length;
                blobInfo.Note = string.Empty;
                blobInfo.FileName = PortalConsts.AttributesXmlFileName;
                IBlobWriter blobWriter = attributeByGuid as IBlobWriter;
                blobWriter.OpenBlob(blobInfo, false);
                blobWriter.WriteDataBlock(memoryStream1.ToArray());
              }
              finally
              {
                memoryStream1.Close();
              }
            }
          }
        }
      }
    }
    catch (Exception ex)
    {
      throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_37"), (object) dbUnit.NameInMessages, (object) ex.Message));
    }
  }

  private void SetBaseVersion(IDBObject publishObject, bool baseVersion)
  {
    IPackedStream service = ServiceUtils.GetService<IPackedStream>((object) ApplicationServices.Container, true);
    IDBAttribute attributeByGuid = publishObject.GetAttributeByGuid(PortalServerConsts.attributeFile, true);
    for (int index = 0; index < attributeByGuid.ValuesCount; ++index)
    {
      attributeByGuid.Index = index;
      if ((attributeByGuid as IBlobReader).OpenBlob(-1).FileName == PortalConsts.AttributesXmlFileName)
      {
        BlobInformation blobInfo = (attributeByGuid as IBlobReader).OpenBlob(0);
        XmlDocument xmlDocument = new XmlDocument();
        using (MemoryStream inStream = new MemoryStream((attributeByGuid as IBlobReader).ReadDataBlock()))
        {
          (attributeByGuid as IBlobReader).CloseBlob();
          using (MemoryStream memoryStream = new MemoryStream())
          {
            service.UnpackStream((Stream) memoryStream, (Stream) inStream);
            memoryStream.Position = 0L;
            xmlDocument.Load((Stream) memoryStream);
          }
        }
        XmlNode xmlNode = xmlDocument.SelectSingleNode("//" + PortalConsts.XmlNodeSysAttribute);
        if (xmlNode.Attributes["F_BASE_VERSION"] != null && Convert.ToInt32(xmlNode.Attributes["F_BASE_VERSION"].Value) == 1)
        {
          string str = baseVersion ? "1" : "0";
          if (xmlNode.Attributes["F_BASE_VERSION"].Value != str)
          {
            xmlNode.Attributes["F_BASE_VERSION"].Value = str;
            using (MemoryStream memoryStream = new MemoryStream())
            {
              xmlDocument.Save((Stream) memoryStream);
              memoryStream.Position = 0L;
              blobInfo.RealFileSize = memoryStream.Length;
              using (MemoryStream outStream = new MemoryStream())
              {
                service.PackStream((Stream) outStream, (Stream) memoryStream, 9);
                blobInfo.PackedFileSize = outStream.Length;
                if ((attributeByGuid as IBlobWriter).OpenBlob(blobInfo, false))
                  (attributeByGuid as IBlobWriter).WriteDataBlock(outStream.ToArray());
              }
            }
          }
        }
      }
    }
    if (!TraceLog.Enabled)
      return;
    TraceLog.Write($"...current BaseVersion={publishObject.ObjectID} correct");
  }

  private IDBObject Create(
    IUserSession session,
    IDBObjectType publishObjectType,
    IDBObjectCollection publishObjects,
    ObjectInfo oi,
    out IDBObject dbParent)
  {
    dbParent = (IDBObject) null;
    if (oi.ParentGuid != Guid.Empty)
    {
      DataTable dataTable = publishObjects.Select(new DBRecordSetParams(new ConditionStructure[1]
      {
        new ConditionStructure(PortalConsts.attributePublishObjectGUID, RelationalOperators.Equal, (object) oi.ParentGuid, LogicalOperators.AND, 0)
      }, new object[1]{ (object) -2 }));
      if (dataTable.Rows.Count > 0)
        dbParent = session.GetObject(Convert.ToInt64(dataTable.Rows[0][0]), true);
    }
    if (dbParent == null)
    {
      DataTable dataTable = publishObjects.Select(new DBRecordSetParams(new ConditionStructure[1]
      {
        new ConditionStructure(PortalServerConsts.attributePublishGUID, RelationalOperators.Equal, (object) oi.Guid, LogicalOperators.AND, 0)
      }, new ColumnDescriptor[1]
      {
        new ColumnDescriptor((object) -2, SortOrders.DESC, 0)
      }, recordCount: 1));
      if (dataTable.Rows.Count > 0)
        dbParent = session.GetObject(Convert.ToInt64(dataTable.Rows[0][0]), true);
    }
    IDBObject version;
    if (dbParent != null)
    {
      version = session.GetObjectCollection(dbParent.ObjectType).CreateVersion(dbParent.ObjectID);
      if (TraceLog.Enabled)
        TraceLog.Write($"...create version from parent {dbParent.ObjectID}");
    }
    else
    {
      version = session.GetObjectCollection(publishObjectType.ObjectType).Create();
      if (TraceLog.Enabled)
        TraceLog.Write("...created new version");
    }
    return version;
  }

  private string LogName(ObjectInfo objectInfo)
  {
    return $"{objectInfo.Caption} ({objectInfo.ObjectGuid})";
  }
}
