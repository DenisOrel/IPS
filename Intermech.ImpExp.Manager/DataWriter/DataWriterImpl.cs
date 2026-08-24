// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.DataWriterImpl
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.Interface.PumpStatistics;
using Intermech.ImpExp.Manager.CommonData;
using Intermech.ImpExp.Manager.DataWriter.Decorators;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter;

internal sealed class DataWriterImpl : IDataWriter
{
  private Dictionary<int, int> objectsTypeLcSteps = new Dictionary<int, int>();
  private Dictionary<int, int> lcStepLevelID = new Dictionary<int, int>();

  public MetadataInfo metadataInfo
  {
    get => ServicesManager.GetService(typeof (IMetadataInfo)) as MetadataInfo;
  }

  public DataWriterImpl(IAppManager manager) => this.AppManager = manager;

  public IAppManager AppManager { get; }

  public IUserSession GetUserSession() => this.metadataInfo.UserSession;

  internal IImportedObjectInfo addObject(ObjectRecord objRec)
  {
    return this.addObject(objRec, (AttributeRecord[]) null);
  }

  public long[] ImportSequrity(SecurityRecord[] importingRecs)
  {
    return this.metadataInfo.dbImporter.ImportSequrity(importingRecs);
  }

  internal IImportedObjectInfo addObject(ObjectRecord objRec, AttributeRecord[] objAttrs)
  {
    ImportingObject importingObject = new ImportingObject(objRec);
    IImportedObjectList importedObjectList = this.CreateImportedObjectList(0);
    importedObjectList.AddObject(objRec);
    if (objAttrs != null)
    {
      foreach (AttributeRecord objAttr in objAttrs)
        importedObjectList.AddAttribute(objAttr);
    }
    AttributesHelper.AddObligatoryObjectAttributes(this.metadataInfo.UserSession, importedObjectList);
    importedObjectList.Import();
    return (IImportedObjectInfo) new ImportedObjectInfo(importedObjectList.Items[0].Object.Object_id, importedObjectList.Items[0].Object.Id);
  }

  internal long addRelation(ImportingRelation importingRelation)
  {
    IImportedRelationList importedRelationList = this.CreateImportedRelationList(0);
    importedRelationList.AddRelation(importingRelation.Relation);
    if (importingRelation.Attributes != null)
    {
      foreach (AttributeRecord attribute in importingRelation.Attributes)
        importedRelationList.AddAttribute(attribute);
    }
    AttributesHelper.AddObligatoryRelationAttributes((IDataWriter) this, importedRelationList);
    importedRelationList.Import();
    return importedRelationList.Items[0].Relation.PrjLinkId;
  }

  public long AddSysObject(Guid sysObjGuid)
  {
    long objectID = -1;
    IDBObject dbObject = this.GetUserSession().GetObject(sysObjGuid);
    if (dbObject != null)
      objectID = dbObject.ObjectID;
    if (this.metadataInfo.ImportedObjects.GetInfo(objectID) == null)
      this.metadataInfo.ImportedObjects.AddValue(dbObject.ObjectID, dbObject.ID, dbObject.ObjectType, dbObject.ObjectGUID, dbObject.GUID);
    return objectID;
  }

  public long AddObject(int objType, int owner, string caption)
  {
    int lcStep = 0;
    int versionId = 0;
    int objVerType = 0;
    int lewelId = 0;
    DateTime universalTime = DateTime.Now.ToUniversalTime();
    DateTime createDate = universalTime;
    string caption1 = caption;
    return this.AddObject(objType, owner, lcStep, versionId, 0, objVerType, universalTime, lewelId, createDate, caption1);
  }

  public long AddObject(int objType, int owner, string caption, Guid objectGuid)
  {
    int lcStep = 0;
    int versionId = 0;
    int objVerType = 0;
    int lewelId = 0;
    DateTime universalTime = DateTime.Now.ToUniversalTime();
    DateTime createDate = universalTime;
    string caption1 = caption;
    if (objectGuid == Guid.Empty)
      objectGuid = this.metadataInfo.NewPumpGuid();
    return this.AddObject(objType, owner, lcStep, versionId, 0, objVerType, universalTime, lewelId, createDate, caption1, objectGuid);
  }

  public long AddObject(int objType, int owner) => this.AddObject(objType, owner, string.Empty);

  internal int getObjTypeLcStep(int objectTypeID)
  {
    if (!this.objectsTypeLcSteps.ContainsKey(objectTypeID))
    {
      IUserSession userSession = this.GetUserSession();
      if (userSession != null)
      {
        IDBLifecycleStepCollection lifecycleStepCollection = userSession.GetLifecycleStepCollection(objectTypeID);
        int firstStep = lifecycleStepCollection != null ? lifecycleStepCollection.GetFirstStep() : 0;
        this.objectsTypeLcSteps.Add(objectTypeID, firstStep);
      }
    }
    return this.objectsTypeLcSteps[objectTypeID];
  }

  internal int getLcStepLevelID(int lcStepID)
  {
    if (!this.lcStepLevelID.ContainsKey(lcStepID))
    {
      IUserSession userSession = this.GetUserSession();
      if (userSession != null)
      {
        IDBLifecycleStep lifecycleStep = userSession.GetLifecycleStep(lcStepID);
        int levelId = lifecycleStep != null ? lifecycleStep.LevelID : 0;
        this.lcStepLevelID.Add(lcStepID, levelId);
      }
    }
    return this.lcStepLevelID[lcStepID];
  }

  public long AddObject(
    int objType,
    int owner,
    int lcStep,
    int versionId,
    int userId,
    int objVerType,
    DateTime modifDate,
    int lewelId,
    DateTime createDate,
    string caption)
  {
    return this.AddObject(objType, owner, lcStep, versionId, userId, objVerType, modifDate, lewelId, createDate, caption, this.metadataInfo.NewPumpGuid());
  }

  private long AddObject(
    int objType,
    int owner,
    int lcStep,
    int versionId,
    int userId,
    int objVerType,
    DateTime modifDate,
    int lewelId,
    DateTime createDate,
    string caption,
    Guid objectGuid)
  {
    ObjectRecord objRec = new ObjectRecord();
    objRec.Object_id = -1L;
    objRec.ObjectGuid = (object) objectGuid;
    objRec.Id = -1L;
    objRec.IdGuid = (object) this.metadataInfo.NewPumpGuid();
    objRec.Lc_step = lcStep;
    objRec.VersionId = 0;
    if (userId != 0)
    {
      DictionaryValue dictionaryValue = this.metadataInfo.ImportedUsers.GetValue(userId);
      objRec.ChkoutBy = dictionaryValue == null ? this.metadataInfo.UserID : dictionaryValue.NewObjectID;
      objRec.ChkoutGuid = (object) (dictionaryValue == null ? this.metadataInfo.UserGUID : (dictionaryValue.Tag as UserTag).Guid);
    }
    else
    {
      objRec.ChkoutBy = this.metadataInfo.UserID;
      objRec.ChkoutGuid = (object) this.metadataInfo.UserGUID;
    }
    objRec.ObjectVerType = 0;
    objRec.ObjectType = objType;
    if (owner != 0)
    {
      DictionaryValue dictionaryValue = this.metadataInfo.ImportedUsers.GetValue(owner);
      objRec.OwnerId = dictionaryValue == null ? this.metadataInfo.UserID : dictionaryValue.NewObjectID;
      objRec.OwnerGuid = (object) (dictionaryValue == null ? this.metadataInfo.UserGUID : (dictionaryValue.Tag as UserTag).Guid);
    }
    else
    {
      objRec.OwnerId = this.metadataInfo.UserID;
      objRec.OwnerGuid = (object) this.metadataInfo.UserGUID;
    }
    objRec.ModifyDate = modifDate;
    objRec.LevelId = lewelId;
    objRec.ObjCreate = createDate;
    objRec.Caption = caption;
    if (objRec.Lc_step == 0)
      objRec.Lc_step = this.getObjTypeLcStep(objType);
    if (objRec.LevelId == 0)
      objRec.LevelId = this.getLcStepLevelID(objRec.Lc_step);
    IImportedObjectInfo importedObjectInfo = this.addObject(objRec);
    objRec.Id = importedObjectInfo.ID;
    objRec.Object_id = importedObjectInfo.ObjectID;
    return objRec.Object_id;
  }

  private int addAttr(AttributeRecord attrRecord)
  {
    IImportedObjectList importedObjectList = this.CreateImportedObjectList();
    if (attrRecord.AttributableId == 0L || attrRecord.AttributableId == -1L)
      throw new Exception("Не указан идентификатор объекта");
    importedObjectList.UseObject(attrRecord.AttributableId);
    importedObjectList.AddAttribute(attrRecord);
    importedObjectList.Import();
    return attrRecord.AttributeId;
  }

  internal AttributeRecord getNewAttributeRecord(int attrType)
  {
    return this.getNewAttributeRecord(0L, attrType, 0);
  }

  internal AttributeRecord getNewAttributeRecord(long objId, int attrType)
  {
    return this.getNewAttributeRecord(objId, attrType, 0);
  }

  internal AttributeRecord getNewAttributeRecord(long objId, int attrType, int numInList)
  {
    return new AttributeRecord()
    {
      AttributeId = attrType,
      AttributableId = objId,
      InlistId = numInList,
      IntegerValue = (object) null,
      IntegerGuid = (object) null,
      DoubleValue = (object) null,
      DoubleGuid = (object) null,
      StringValue = (object) null,
      DateValue = (object) null
    };
  }

  public AttributeRecord CreateAttributeRecord(
    int attrType,
    AttrValueType attrValtype,
    object attrVal,
    int numInList)
  {
    AttributeRecord newAttributeRecord = this.getNewAttributeRecord((long) attrType, numInList);
    switch (attrValtype)
    {
      case AttrValueType.stringVal:
        newAttributeRecord.StringValue = attrVal;
        break;
      case AttrValueType.integerVal:
        newAttributeRecord.IntegerValue = attrVal;
        break;
      case AttrValueType.doubleVal:
        newAttributeRecord.DoubleValue = attrVal;
        break;
      case AttrValueType.datetimeVal:
        newAttributeRecord.DateValue = attrVal;
        break;
    }
    return newAttributeRecord;
  }

  public AttributeRecord CreateAttributeRecordNull(int attrType)
  {
    return this.getNewAttributeRecord(attrType);
  }

  public AttributeRecord CreateAttributeRecordStr(int attrType, string value)
  {
    AttributeRecord attributeRecordNull = this.CreateAttributeRecordNull(attrType);
    attributeRecordNull.StringValue = (object) value;
    return attributeRecordNull;
  }

  public AttributeRecord CreateAttributeRecordInt(int attrType, long value)
  {
    AttributeRecord attributeRecordNull = this.CreateAttributeRecordNull(attrType);
    attributeRecordNull.IntegerValue = (object) value;
    return attributeRecordNull;
  }

  public AttributeRecord CreateAttributeRecordDouble(int attrType, double value)
  {
    AttributeRecord attributeRecordNull = this.CreateAttributeRecordNull(attrType);
    attributeRecordNull.DoubleValue = (object) value;
    return attributeRecordNull;
  }

  public AttributeRecord CreateAttributeRecordDate(int attrType, DateTime value)
  {
    AttributeRecord attributeRecordNull = this.CreateAttributeRecordNull(attrType);
    attributeRecordNull.DateValue = (object) value;
    return attributeRecordNull;
  }

  public AttributeRecord CreateAttributeRecordLink(int attrType, long value, string caption)
  {
    AttributeRecord attributeRecordNull = this.CreateAttributeRecordNull(attrType);
    attributeRecordNull.IntegerValue = (object) value;
    attributeRecordNull.StringValue = (object) caption;
    return attributeRecordNull;
  }

  public AttributeRecord CreateAttributeRecordMeasure(
    int attrType,
    double value,
    long measureID,
    string strValue)
  {
    AttributeRecord attributeRecordNull = this.CreateAttributeRecordNull(attrType);
    attributeRecordNull.DoubleValue = (object) value;
    attributeRecordNull.IntegerValue = (object) measureID;
    attributeRecordNull.StringValue = (object) strValue;
    return attributeRecordNull;
  }

  public AttributeRecord CreateAttributeRecordBlob(
    int attrType,
    string filePath,
    long fileSize,
    string fileNote,
    ArcMethods arcMethod)
  {
    AttributeRecord attributeRecordNull = this.CreateAttributeRecordNull(attrType);
    attributeRecordNull.Path2File = filePath;
    attributeRecordNull.FileSize = (object) fileSize;
    attributeRecordNull.FileNote = (object) fileNote;
    attributeRecordNull.StringValue = (object) fileNote;
    attributeRecordNull.ArcMethod = (object) (int) arcMethod;
    return attributeRecordNull;
  }

  public int AddAttribute(
    long objId,
    int attrType,
    AttrValueType attrValtype,
    object attrVal,
    int numInList)
  {
    AttributeRecord newAttributeRecord = this.getNewAttributeRecord(objId, attrType, numInList);
    switch (attrValtype)
    {
      case AttrValueType.stringVal:
        newAttributeRecord.StringValue = attrVal;
        break;
      case AttrValueType.integerVal:
        newAttributeRecord.IntegerValue = attrVal;
        break;
      case AttrValueType.doubleVal:
        newAttributeRecord.DoubleValue = attrVal;
        break;
      case AttrValueType.datetimeVal:
        newAttributeRecord.DateValue = attrVal;
        break;
    }
    return this.addAttr(newAttributeRecord);
  }

  public int AddAttributeNull(long objId, int attrType)
  {
    return this.addAttr(this.getNewAttributeRecord(objId, attrType));
  }

  public int AddAttributeStr(long objId, int attrType, string value)
  {
    AttributeRecord newAttributeRecord = this.getNewAttributeRecord(objId, attrType);
    newAttributeRecord.StringValue = (object) value;
    return this.addAttr(newAttributeRecord);
  }

  public int AddAttributeInt(long objId, int attrType, long value)
  {
    AttributeRecord newAttributeRecord = this.getNewAttributeRecord(objId, attrType);
    newAttributeRecord.IntegerValue = (object) value;
    return this.addAttr(newAttributeRecord);
  }

  public int AddAttributeDouble(long objId, int attrType, double value)
  {
    AttributeRecord newAttributeRecord = this.getNewAttributeRecord(objId, attrType);
    newAttributeRecord.DoubleValue = (object) value;
    return this.addAttr(newAttributeRecord);
  }

  public int AddAttributeDate(long objId, int attrType, DateTime value)
  {
    AttributeRecord newAttributeRecord = this.getNewAttributeRecord(objId, attrType);
    newAttributeRecord.DateValue = (object) value;
    return this.addAttr(newAttributeRecord);
  }

  public int AddAttributeLink(long objId, int attrType, long value, string caption)
  {
    AttributeRecord newAttributeRecord = this.getNewAttributeRecord(objId, attrType);
    newAttributeRecord.IntegerValue = (object) value;
    newAttributeRecord.StringValue = (object) caption;
    return this.addAttr(newAttributeRecord);
  }

  public int AddAttributeMeasure(
    long objId,
    int attrType,
    double value,
    long measureID,
    string strValue)
  {
    AttributeRecord newAttributeRecord = this.getNewAttributeRecord(objId, attrType);
    newAttributeRecord.DoubleValue = (object) value;
    newAttributeRecord.IntegerValue = (object) measureID;
    newAttributeRecord.StringValue = (object) strValue;
    return this.addAttr(newAttributeRecord);
  }

  public int AddAttributeBlob(
    long objId,
    int attrType,
    string filePath,
    long fileSize,
    string fileNote,
    ArcMethods arcMethod)
  {
    AttributeRecord newAttributeRecord = this.getNewAttributeRecord(objId, attrType);
    newAttributeRecord.Path2File = filePath;
    newAttributeRecord.FileSize = (object) fileSize;
    newAttributeRecord.FileNote = (object) fileNote;
    newAttributeRecord.StringValue = (object) fileNote;
    newAttributeRecord.ArcMethod = (object) (int) arcMethod;
    return this.addAttr(newAttributeRecord);
  }

  public long AddRelationFromID(long projId, long partId, int relType, DateTime crtDate)
  {
    this.GetUserSession();
    RelationRecord rel = new RelationRecord(0L, (object) this.metadataInfo.NewPumpGuid(), (object) projId, (object) partId, relType, (object) crtDate, 0L);
    IImportedRelationList importedRelationList = this.CreateImportedRelationList(0);
    importedRelationList.AddRelation(rel);
    importedRelationList.Import();
    return importedRelationList.Items[0].Relation.PrjLinkId;
  }

  public long AddRelation(long projId, long partId, int relType)
  {
    return this.AddRelation(projId, partId, relType, DateTime.Now.ToUniversalTime());
  }

  public long AddRelation(long projId, long partId, int relType, DateTime crtDate)
  {
    long id = this.metadataInfo.ImportedObjects.GetID(partId);
    if (id == 0L)
      throw new Exception($"Среди закаченных объектов не найден объект {partId}, который является участником создаваемой связи.");
    return this.AddRelationFromID(projId, id, relType, crtDate);
  }

  private bool addRelationAttribyte(AttributeRecord relationAttributeRecord)
  {
    IImportedRelationList importedRelationList = this.CreateImportedRelationList(0);
    if (relationAttributeRecord.AttributableId == 0L || relationAttributeRecord.AttributableId == -1L)
      throw new Exception("Не указан мдентификатор связи");
    importedRelationList.UseRelation(relationAttributeRecord.AttributableId);
    importedRelationList.AddAttribute(relationAttributeRecord);
    importedRelationList.Import();
    return importedRelationList.Items[0].Relation.PrjLinkId != 0L;
  }

  internal AttributeRecord getNewRelationAttributeRecord(long relId, int attrTypeId, int inlist)
  {
    return new AttributeRecord()
    {
      AttributeId = attrTypeId,
      AttributableId = relId,
      InlistId = inlist,
      IntegerValue = (object) null,
      IntegerGuid = (object) null,
      DoubleValue = (object) null,
      DoubleGuid = (object) null,
      StringValue = (object) null,
      DateValue = (object) null
    };
  }

  internal AttributeRecord getNewRelationAttributeRecord(long relId, int attrTypeId)
  {
    return this.getNewRelationAttributeRecord(relId, attrTypeId, 0);
  }

  public bool AddRelationAttribyte(
    long relId,
    int attrTypeId,
    int numInList,
    AttrValueType attrValtype,
    object attrVal)
  {
    AttributeRecord relationAttributeRecord = this.getNewRelationAttributeRecord(relId, attrTypeId);
    relationAttributeRecord.InlistId = numInList;
    switch (attrValtype)
    {
      case AttrValueType.stringVal:
        relationAttributeRecord.StringValue = attrVal;
        break;
      case AttrValueType.integerVal:
        relationAttributeRecord.IntegerValue = attrVal;
        break;
      case AttrValueType.doubleVal:
        relationAttributeRecord.DoubleValue = attrVal;
        break;
      case AttrValueType.datetimeVal:
        relationAttributeRecord.DateValue = attrVal;
        break;
    }
    return this.addRelationAttribyte(relationAttributeRecord);
  }

  public bool AddRelationAttribyteStr(long relId, int attrTypeId, string value)
  {
    AttributeRecord relationAttributeRecord = this.getNewRelationAttributeRecord(relId, attrTypeId);
    relationAttributeRecord.StringValue = (object) value;
    return this.addRelationAttribyte(relationAttributeRecord);
  }

  public bool AddRelationAttribyteInt(long relId, int attrTypeId, long value)
  {
    AttributeRecord relationAttributeRecord = this.getNewRelationAttributeRecord(relId, attrTypeId);
    relationAttributeRecord.IntegerValue = (object) value;
    return this.addRelationAttribyte(relationAttributeRecord);
  }

  public bool AddRelationAttribyteDbl(long relId, int attrTypeId, double value)
  {
    AttributeRecord relationAttributeRecord = this.getNewRelationAttributeRecord(relId, attrTypeId);
    relationAttributeRecord.DoubleValue = (object) value;
    return this.addRelationAttribyte(relationAttributeRecord);
  }

  public bool AddRelationAttribyteDate(long relId, int attrTypeId, DateTime value)
  {
    AttributeRecord relationAttributeRecord = this.getNewRelationAttributeRecord(relId, attrTypeId);
    relationAttributeRecord.DateValue = (object) value;
    return this.addRelationAttribyte(relationAttributeRecord);
  }

  public bool AddRelationAttribyteLink(long relId, int attrTypeId, long value, string caption)
  {
    AttributeRecord relationAttributeRecord = this.getNewRelationAttributeRecord(relId, attrTypeId);
    relationAttributeRecord.IntegerValue = (object) value;
    relationAttributeRecord.StringValue = (object) caption;
    return this.addRelationAttribyte(relationAttributeRecord);
  }

  public bool AddRelationAttribyteMeasure(
    long relId,
    int attrTypeId,
    double value,
    long measureID,
    string strValue)
  {
    AttributeRecord relationAttributeRecord = this.getNewRelationAttributeRecord(relId, attrTypeId);
    relationAttributeRecord.DoubleValue = (object) value;
    relationAttributeRecord.IntegerValue = (object) measureID;
    relationAttributeRecord.StringValue = (object) strValue;
    return this.addRelationAttribyte(relationAttributeRecord);
  }

  public bool AddRelationAttribyteBlob(
    long relId,
    int attrTypeId,
    string filePath,
    long fileSize,
    string fileNote,
    ArcMethods arcMethod)
  {
    AttributeRecord relationAttributeRecord = this.getNewRelationAttributeRecord(relId, attrTypeId);
    relationAttributeRecord.Path2File = filePath;
    relationAttributeRecord.FileSize = (object) fileSize;
    relationAttributeRecord.FileNote = (object) fileNote;
    relationAttributeRecord.StringValue = (object) fileNote;
    relationAttributeRecord.ArcMethod = (object) (int) arcMethod;
    return this.addRelationAttribyte(relationAttributeRecord);
  }

  public IImportedObjectList CreateImportedObjectList()
  {
    return (IImportedObjectList) new ImportedObjectList(this);
  }

  public IImportedObjectList CreateImportedObjectList(int packetSize)
  {
    return (IImportedObjectList) new ImportedObjectList(this, packetSize);
  }

  public IImportedObjectList CreateImportedObjectListWithStatistics(Guid ownerGuid)
  {
    Task<Intermech.ImpExp.Interface.PumpStatistics.PumpStatistics> task = ApplicationServices.Container.GetService<PumpStatisticsService>().LoadAsync(ownerGuid);
    task.Wait();
    return (IImportedObjectList) new ImportedObjectListWithStatisticsDecorator((IImportedObjectList) new ImportedObjectList(this), task.Result);
  }

  public IImportedObjectList CreateImportedObjectListWithStatistics(Guid ownerGuid, int packetSize)
  {
    Task<Intermech.ImpExp.Interface.PumpStatistics.PumpStatistics> task = ApplicationServices.Container.GetService<PumpStatisticsService>().LoadAsync(ownerGuid);
    task.Wait();
    return (IImportedObjectList) new ImportedObjectListWithStatisticsDecorator((IImportedObjectList) new ImportedObjectList(this, packetSize), task.Result);
  }

  public IImportedRelationList CreateImportedRelationListWithStatistics(Guid ownerGuid)
  {
    Task<Intermech.ImpExp.Interface.PumpStatistics.PumpStatistics> task = ApplicationServices.Container.GetService<PumpStatisticsService>().LoadAsync(ownerGuid);
    task.Wait();
    return (IImportedRelationList) new ImportedRelationListWithStatisticsDecorator((IImportedRelationList) new ImportedRelationList(this), task.Result);
  }

  public IImportedRelationList CreateImportedRelationListWithStatistics(
    Guid ownerGuid,
    int packetSize)
  {
    Task<Intermech.ImpExp.Interface.PumpStatistics.PumpStatistics> task = ApplicationServices.Container.GetService<PumpStatisticsService>().LoadAsync(ownerGuid);
    task.Wait();
    return (IImportedRelationList) new ImportedRelationListWithStatisticsDecorator((IImportedRelationList) new ImportedRelationList(this, packetSize), task.Result);
  }

  public IImportedRelationList CreateImportedRelationList()
  {
    return (IImportedRelationList) new ImportedRelationList(this);
  }

  public IImportedRelationList CreateImportedRelationList(int packetSize)
  {
    return (IImportedRelationList) new ImportedRelationList(this, packetSize);
  }

  public bool SetVersionsTree(DataTable treeTable)
  {
    return this.metadataInfo.dbImporter.SetVersionsTree(treeTable);
  }

  public bool IncludeObjectIntoSelection(long selectionID, string key, long objectID, long id)
  {
    return this.metadataInfo.dbImporter.IncludeObjectIntoSelection(selectionID, key, objectID, id);
  }

  public IFoldersFilter GetFoldersFilterPumper() => (IFoldersFilter) new FoldersFilter();
}
