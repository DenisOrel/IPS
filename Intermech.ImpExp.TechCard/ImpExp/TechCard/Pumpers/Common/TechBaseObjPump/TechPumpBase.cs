// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.TechPumpBase
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.Interface.SafeDataProxy;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Common.ArcArtPump;
using Intermech.ImpExp.TechCard.Common.LoadCache;
using Intermech.ImpExp.TechCard.Pumpers.Common.DataCache;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectData;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechParam;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.Strategy;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.Strategy.Convert;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump;
using Intermech.ImpExp.TechCard.TechProcPump;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.ImpExp.TechCard.TechProcPump.Common.TechDiff;
using Intermech.ImpExp.TechCard.TechTypes;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Pools;
using Intermech.Text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;

[TaskDescription("Инициализация данных для перекачки - базовый класс", "Перекачка данных - базовый класс")]
internal abstract class TechPumpBase : PumpClass
{
  internal static Guid cnt_Common_Folder_Guid = new Guid();
  internal TechEntityConverter _entityConverter;
  private TechAttributeWriter _attributeWriter;
  protected string _recType;
  protected int _recTypeID;
  protected string _tableName;
  protected long _lastObjID;
  protected TechDataSource _dataSource;
  protected int _imTableCode;
  protected List<string> _dopTypes;
  protected string _sortFieldName = string.Empty;
  protected IAttributeTypeItem _atTechLinkAtRelGTPRelation;
  protected IAttributeTypeItem _atTechTypeKeyAttr;
  protected IAttributeTypeItem _atTechArtAttr;
  protected IAttributeTypeItem _atSortAttr;
  protected IAttributeTypeItem _atLastLevelSeek;
  protected IAttributeTypeItem _atNaimAttrType;
  protected IAttributeTypeItem _atObozAttrType;
  protected IAttributeTypeItem _atImbaseKeyAttr;
  protected int _otPerehTypeID = -1;
  protected int _otOperTypeID = -1;
  protected int _relTechRelationID = -1;
  protected int _relTechGTPRelationID = -1;
  protected bool _allowDiffObjects;
  protected bool _isObjectTypeIsSuspended;
  protected Dictionary<int, TechDiffRec> _diffObjectsList;
  protected int objTypeID = -1;
  protected bool objAnyAttr;
  protected bool relAnyAttr;
  protected int captionLength = -1;
  protected List<Guid> objAttrList;
  protected List<Guid> relAttrList;
  private int _lcFirstStepId = -1;
  protected Entity entRefImbaseKey;
  protected Entity entRefImRecKey;
  protected Entity entRefImTblKey;
  protected Dictionary<TechObjectRecordBase, TechParamList> tpObjRecList;
  protected TechParamList _techParmList;
  protected EntityTypeRec entTypeRec;
  protected TechObjectDataSubCache _techDataRecCache;
  protected IImportedObjectList _impObjList;
  protected IImportedRelationList _impRelList;
  protected Dictionary<TechObjectRecordBase, int> _techBaseImportList;
  protected Dictionary<int, Dictionary<int, long>> _curArt2ObjectIdList = new Dictionary<int, Dictionary<int, long>>();
  protected Dictionary<string, TechObjectRecordSub> _dopRecLastList = new Dictionary<string, TechObjectRecordSub>();
  protected List<TechObjectRelationInfo> relListPairs = new List<TechObjectRelationInfo>();
  protected internal IImportingData _import_data_main;
  protected internal IImportingData _import_data_imbase;
  private static readonly Dictionary<int, DictionaryValue> UserList = new Dictionary<int, DictionaryValue>();
  public bool ForceObjAttrWriteMode = true;
  public bool ForceLinkAttrWriteMode = true;

  protected virtual TechDataSource GetDataSource()
  {
    if (this._dataSource == null)
      this._dataSource = new TechDataSource((ITechDataBuilder) new TechDataBuilderSimple<TechPumpBase>(this));
    return this._dataSource;
  }

  protected void DoHandleImportObjectsException(Exception exception)
  {
    if (!exception.Data.Contains((object) "DbImportException"))
      return;
    for (int index = 0; index < this._impObjList.Items.Count; ++index)
      this._impObjList.Items[index].Object = (ObjectRecord) null;
    this.TechBase_AfterImportObjectEvent((object) this._impObjList, EventArgs.Empty);
  }

  protected void DoHandleImportRelationsException(Exception exception)
  {
    if (!exception.Data.Contains((object) "DbImportException"))
      return;
    this.impRelList_AfterImportEvent((object) this._impRelList, EventArgs.Empty);
  }

  protected override int GetRecordsCount(string sqlText)
  {
    using (IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand())
    {
      command.CommandTimeout = 0;
      command.CommandText = sqlText.ToUpper();
      try
      {
        return System.Convert.ToInt32(command.ExecuteScalar());
      }
      finally
      {
        if (TechcardConsts.ConnectionManager.DataBaseType == "IntermechConnection.MsSQL")
          command.Connection.Close();
      }
    }
  }

  protected override int GetTableRecordsCount(string tableName)
  {
    using (IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand())
    {
      command.CommandTimeout = 0;
      command.CommandText = "SELECT COUNT(*) FROM " + tableName.ToUpper();
      try
      {
        return System.Convert.ToInt32(command.ExecuteScalar());
      }
      finally
      {
        if (TechcardConsts.ConnectionManager.DataBaseType == "IntermechConnection.MsSQL")
          command.Connection.Close();
      }
    }
  }

  protected override bool TableExists(string tableName)
  {
    return TechcardConsts.ConnectionManager.IsTableExists(tableName);
  }

  protected override IDataReader GetBehaviorDataReader(
    string tableName,
    string tableColumns,
    CommandBehavior commandBehavior)
  {
    if (!this.TableExists(tableName))
      return (IDataReader) null;
    IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand();
    command.CommandTimeout = 0;
    command.CommandText = $"SELECT {tableColumns} FROM {tableName.ToUpper()}";
    if (commandBehavior == CommandBehavior.SchemaOnly)
      command.CommandText += " WHERE 1=0";
    return command.ExecuteReader(commandBehavior);
  }

  protected override IDataReader GetDataReader(string sqlText, CommandBehavior commandBehavior)
  {
    IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand();
    command.CommandText = sqlText;
    command.CommandTimeout = 0;
    return command.ExecuteReader(commandBehavior);
  }

  protected override IDataReader GetDefaultDataReader(string tableName, string tableColumns)
  {
    return this.GetBehaviorDataReader(tableName, tableColumns, TechcardConsts.ConnectionManager.CommandBehavior);
  }

  protected override IDataReader GetDataReader(string sqlText)
  {
    return this.GetDataReader(sqlText, TechcardConsts.ConnectionManager.CommandBehavior);
  }

  protected override IDataReader GetDefaultDataReader(string tableName)
  {
    return this.GetDefaultDataReader(tableName, "*");
  }

  public static long GenBaseTechObjectsVersionsCacheKey(int objKey, LinkedObjectType objType)
  {
    return ((long) objKey << 32 /*0x20*/) + (long) System.Convert.ToInt32((object) objType);
  }

  public int GetSqlRecordsCount(string sqlText) => this.GetRecordsCount(sqlText);

  public IDataReader GetCustomDataReader(string sqlText) => this.GetDataReader(sqlText);

  public long LastObjID => this._lastObjID;

  public string RecType => this._recType;

  public int RecTypeID => this._recTypeID;

  public string TableName => this._tableName;

  public virtual long GetLastObjectForType(IUserSession session, int objectTypeId, long objectId)
  {
    long lastObjectForType = 0;
    if (session == null)
      return lastObjectForType;
    try
    {
      Guid techTypeKeyAttrGuid = TechcardConsts.TypeConsts.atTechTypeKeyAttrGuid;
      if (session.GetAttributeType(techTypeKeyAttrGuid, false) == null || session.GetObjectType(this.objTypeID, false) == null)
        return lastObjectForType;
      long num = this.GetLastObjectIdFromObjectCache(session, objectTypeId, 100);
      if (Consts.IsUndefinedObjectId(num))
        return lastObjectForType;
      this.plugin.appManager.AddInfoMessage($"В кэше объектов IPS найден объект ObjectId = {num}");
      if (this.GetCategoriesByNeed2CreateTechRel().Length != 0)
      {
        num = this.GetLastObjectIdFromRelationCache(session, objectTypeId, 100);
        if (!Consts.IsUndefinedObjectId(num))
          this.plugin.appManager.AddInfoMessage($"В кэше связей IPS найден объект ObjectId = {num}");
      }
      if (Consts.IsUndefinedObjectId(num))
        return lastObjectForType;
      IDBAttribute attributeByGuid = session.GetObject(num, false)?.GetAttributeByGuid(techTypeKeyAttrGuid, false);
      if (attributeByGuid != null && !attributeByGuid.IsNull)
      {
        lastObjectForType = attributeByGuid.AsInteger;
        if (lastObjectForType < 0L)
          lastObjectForType = 0L;
        if (lastObjectForType > 0L)
        {
          int recKey;
          TechcardConsts.Utils.DecodeHashCode(lastObjectForType, out int _, out recKey);
          lastObjectForType = (long) recKey;
        }
      }
      if (!Consts.IsUndefinedObjectId(lastObjectForType))
        this.plugin.appManager.AddInfoMessage($"В кэше IPS последний импортированный объект для записи Key = {lastObjectForType}");
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage("Невозможно получить список объектов по его типу:" + (object) ex);
      if (!(ex is OutOfMemoryException))
        return lastObjectForType;
      throw;
    }
    return lastObjectForType;
  }

  private long GetLastObjectIdFromRelationCache(
    IUserSession session,
    int objectTypeId,
    int recordCount)
  {
    long fromRelationCache = 0;
    if (this.plugin.Imdi.ImportedRelations?.Dictionary == null)
      return fromRelationCache;
    List<long> longList = new List<long>(this.plugin.Imdi.ImportedRelations.Dictionary.Count);
    longList.AddRange(this.plugin.Imdi.ImportedRelations.Dictionary.Select<KeyValuePair<object, DictionaryValue>, long>((System.Func<KeyValuePair<object, DictionaryValue>, long>) (item => System.Convert.ToInt64(item.Key))));
    longList.Sort();
    for (int index = longList.Count - 1; index > Math.Max(0, longList.Count - recordCount); --index)
    {
      long aRelationID = longList[index];
      if (aRelationID != 0L)
      {
        IDBRelation relation = session.GetRelation(aRelationID, false);
        IDBObject objectById = relation != null ? session.GetObjectByID(relation.PartID, false) : (IDBObject) null;
        if (objectById != null && objectById.ObjectType == objectTypeId)
        {
          fromRelationCache = objectById.ObjectID;
          break;
        }
      }
      else
        break;
    }
    return fromRelationCache;
  }

  private long GetLastObjectIdFromObjectCache(
    IUserSession session,
    int objectTypeId,
    int recordCount)
  {
    long idFromObjectCache = 0;
    if (this.plugin.Imdi.ImportedObjects?.Dictionary == null)
      return idFromObjectCache;
    List<long> longList = new List<long>(this.plugin.Imdi.ImportedObjects.Dictionary.Count);
    longList.AddRange(this.plugin.Imdi.ImportedObjects.Dictionary.Select<KeyValuePair<object, DictionaryValue>, long>((System.Func<KeyValuePair<object, DictionaryValue>, long>) (item => System.Convert.ToInt64(item.Key))));
    longList.Sort();
    for (int index = longList.Count - 1; index >= Math.Max(0, longList.Count - recordCount); --index)
    {
      long objectID = longList[index];
      int objectTypeId1 = this.plugin.Imdi.ImportedObjects.GetObjectTypeID(objectID);
      if (objectTypeId1 == -1)
        objectTypeId1 = session.GetObjectInfo(objectID).ObjectTypeID;
      if (objectTypeId1 == objectTypeId)
      {
        idFromObjectCache = objectID;
        break;
      }
    }
    return idFromObjectCache;
  }

  protected void LoadImportingCategoryData()
  {
    this._import_data_main = ImportingCategoryDataCache.Instance.GetCache(ImportingCategoryBuilder.DiffCategories(this.GetCategoriesByNeed2CreateTechRel(), this.GetCategoriesByNeed2FillTechObject(), new ImportingCategory[4]
    {
      this.GetTechCategory(),
      this.GetTechObjectExCategory(),
      this.GetTechUniqueCategory(),
      ImportingCategory.TechSuspendedAtr
    }));
    if (this._import_data_main != null)
      this._import_data_main = (IImportingData) new SafeImportingDataProxy(this._import_data_main, (ISafeProxyErrorHandler) new ImpExpErrorHandler(this.plugin.appManager));
    this._import_data_imbase = ImportingCategoryDataCache.Instance.GetCache(new ImportingCategory[2]
    {
      ImportingCategory.TechCeh,
      ImportingCategory.ImbaseFolders
    });
  }

  protected abstract ImportingCategory GetTechCategory();

  protected virtual ImportingCategory GetTechUniqueCategory() => ImportingCategory.None;

  protected virtual ImportingCategory GetTechObjectExCategory() => ImportingCategory.None;

  protected virtual ImportingCategory[] GetCategoriesByNeed2FillTechObject()
  {
    return new ImportingCategory[0];
  }

  protected virtual ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new ImportingCategory[0];
  }

  protected virtual void LoadMetaData4StoppedPump()
  {
    this.entTypeRec = TechPumpData.EntTypeList.GetRecByType(this._recTypeID);
  }

  protected virtual void LoadMetaData4Pump()
  {
    if (this.objTypeID == -1)
      this.objTypeID = TechPumpData.TechType.TechTypeList.GetObjTypeId(this._recTypeID);
    TechTypeInfo typeRecByRecordId = TechPumpData.TechType.TechTypeList.GetTypeRecByRecordId(this._recTypeID);
    ImTableInfo tableInfo = typeRecByRecordId != null ? TechPumpData.Tables.ImTablesData.GetTableInfo((TechcardConsts.imTablesConsts) typeRecByRecordId.PredefID) : (ImTableInfo) null;
    if (tableInfo != null)
      this._imTableCode = tableInfo.TableKey;
    IMetadataInfo imdi = this.plugin.Imdi;
    if (imdi == null)
    {
      this.plugin.appManager.AddErrorMessage("Ошибка получения кэша метаданных");
    }
    else
    {
      IAttributeTypeItem byGuid1 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atTechTypeKeyAttrGuid);
      if (byGuid1 != null)
        this._atTechTypeKeyAttr = byGuid1;
      IAttributeTypeItem byGuid2 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atTechArtAtrGuid);
      if (byGuid2 != null)
        this._atTechArtAttr = byGuid2;
      IAttributeTypeItem byGuid3 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atLA_SortAttrGuid);
      if (byGuid3 != null)
        this._atSortAttr = byGuid3;
      IAttributeTypeItem byGuid4 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atLastLevelSeek);
      if (byGuid4 != null)
        this._atLastLevelSeek = byGuid4;
      IAttributeTypeItem byGuid5 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atNaimAttrTypeGuid);
      if (byGuid5 != null)
        this._atNaimAttrType = byGuid5;
      IAttributeTypeItem byGuid6 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atObozAttrTypeGuid);
      if (byGuid6 != null)
        this._atObozAttrType = byGuid6;
      IAttributeTypeItem byGuid7 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atImbaseKeyAttrGuid);
      if (byGuid7 != null)
        this._atImbaseKeyAttr = byGuid7;
      IAttributeTypeItem byGuid8 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atTechProcGroupRelAttrGUID);
      if (byGuid8 != null)
        this._atTechLinkAtRelGTPRelation = byGuid8;
      IRelationTypeItem byGuid9 = this.plugin.Imdi.RelationTypes.GetByGuid(TechcardConsts.TypeConsts.rtTechRelationGuid);
      if (byGuid9 != null)
        this._relTechRelationID = byGuid9.ID;
      IRelationTypeItem byGuid10 = this.plugin.Imdi.RelationTypes.GetByGuid(TechcardConsts.TypeConsts.rtTechGTPRelationGuid);
      if (byGuid10 != null)
        this._relTechGTPRelationID = byGuid10.ID;
      IObjectTypeItem byGuid11 = imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otTechPerehodObjTypeGuid);
      if (byGuid11 != null)
        this._otPerehTypeID = byGuid11.ID;
      IObjectTypeItem byGuid12 = imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otOperationObjTypeGuid);
      if (byGuid12 != null)
        this._otOperTypeID = byGuid12.ID;
      this.LoadMetaData4ObjAttrs();
      this.LoadMetaData4RelAttrs();
    }
  }

  protected virtual void LoadEntityMetaData()
  {
    if (this.entTypeRec == null)
      return;
    foreach (Entity entity in this.entTypeRec.CodeList.Values)
    {
      if (entity != null && entity.EntityReference != null && entity.EntityReference.Field == -2)
      {
        this.entRefImbaseKey = entity;
        break;
      }
    }
    foreach (Entity entity in this.entTypeRec.CodeList.Values)
    {
      if (entity != null && entity.EntityReference != null && entity.EntityReference.Field == -3)
      {
        this.entRefImRecKey = entity;
        break;
      }
    }
    foreach (Entity entity in this.entTypeRec.CodeList.Values)
    {
      if (entity != null && entity.EntityReference != null && entity.EntityReference.Field == -4)
      {
        this.entRefImTblKey = entity;
        break;
      }
    }
  }

  protected virtual void LoadMetaData4ObjAttrs()
  {
    if (this.objTypeID == -1)
      return;
    IMetadataInfo imdi = this.plugin.Imdi;
    if (imdi == null)
    {
      this.plugin.appManager.AddErrorMessage("Ошибка получения метаданных");
    }
    else
    {
      IObjectTypeItem objectTypeItem = imdi.ObjectTypes.GetByID(this.objTypeID);
      if (objectTypeItem != null)
      {
        this.objAnyAttr = objectTypeItem.AnyAttribute;
        int captionAttributeId = objectTypeItem.CaptionAttributeID;
        if (captionAttributeId != 0)
        {
          IAttributeTypeItem byId = imdi.AttributeTypes.GetByID(captionAttributeId);
          if (byId != null)
            this.captionLength = byId.MaxSize;
        }
        if (this.objAnyAttr)
          return;
        for (; objectTypeItem != null; objectTypeItem = imdi.ObjectTypes.GetByGuid(objectTypeItem.ParentID))
        {
          foreach (int attrTypeId in objectTypeItem.AttrTypeIDs)
          {
            IAttributeTypeItem byId = imdi.AttributeTypes.GetByID(attrTypeId);
            if (byId != null)
              this.objAttrList.Add(byId.GUID);
          }
          if (objectTypeItem.ParentID == Guid.Empty)
            break;
        }
      }
      else
        this.plugin.appManager.AddErrorMessage($"Тип атрибута №{this.objTypeID} не найден!");
    }
  }

  protected virtual void LoadMetaData4RelAttrs()
  {
    IMetadataInfo imdi = this.plugin.Imdi;
    IRelationTypeItem byGuid = imdi.RelationTypes.GetByGuid(TechcardConsts.TypeConsts.rtTechRelationGuid);
    if (byGuid == null)
      return;
    IDBRelationType relationType = imdi.UserSession.GetRelationType(byGuid.ID);
    if (relationType != null)
      this.relAnyAttr = relationType.AnyAttributes;
    if (this.relAnyAttr)
      return;
    foreach (int attrTypeId in byGuid.AttrTypeIDs)
    {
      IAttributeTypeItem byId = imdi.AttributeTypes.GetByID(attrTypeId);
      if (byId != null)
        this.relAttrList.Add(byId.GUID);
    }
  }

  public virtual bool CheckObjTypeOrParamType(string entCode, Guid attrGuid)
  {
    bool flag = this.objAnyAttr || this.ForceObjAttrWriteMode;
    if (!flag && attrGuid != Guid.Empty)
      flag = this.objAttrList.Contains(attrGuid);
    return flag;
  }

  public virtual bool CheckObjLinkOrParamType(string entCode, Guid attrGuid)
  {
    bool flag = this.relAnyAttr || this.ForceLinkAttrWriteMode;
    if (!flag && attrGuid != Guid.Empty)
      flag = this.relAttrList.Contains(attrGuid);
    return flag;
  }

  protected virtual void ExamData()
  {
    this.ExamSubData(string.Empty);
    foreach (string dopType in this._dopTypes)
      this.ExamSubData(dopType);
  }

  protected virtual void ExamSubData(string dopType)
  {
    string str1 = this.GetType().ToString();
    if (this.TableName == string.Empty)
    {
      this.plugin.appManager.AddWarningMessage(str1 + ": Имя таблицы не задано");
    }
    else
    {
      string str2 = dopType != string.Empty ? $"{this.TableName}_{dopType}" : this.TableName;
      if (!this.TableExists(str2))
        this.plugin.appManager.AddWarningMessage(str1 + ": Таблица '' не найдена.");
      if (!TechCardPlugin.Configuration.CheckDopTablesDuplicates)
        return;
      this.CheckDopTableDuplicates(str2);
    }
  }

  private void CheckDopTableDuplicates(string dopTypeTable)
  {
    if (dopTypeTable.Length < 2)
      return;
    if (!((IEnumerable<string>) new string[4]
    {
      "_D",
      "_I",
      "_F",
      "_S"
    }).Contains<string>(dopTypeTable.Substring(dopTypeTable.Length - 2, 2).ToUpperInvariant()))
      return;
    string str1 = $"select count(*) from ({(!dopTypeTable.EndsWith("_D") ? (!dopTypeTable.StartsWith("TP_MAT") ? $"select F_PARENTKEY, F_ROW from {dopTypeTable} group by F_PARENTKEY, F_ROW having count(*) > 1" : $"select F_PARENTKEY, F_ROW, F_SETKEY from {dopTypeTable} group by F_PARENTKEY, F_ROW, F_SETKEY having count(*) > 1") : (!dopTypeTable.StartsWith("TP_MAT") ? $"select F_PARENTKEY, F_ENTITY, F_ROW from {dopTypeTable} group by F_PARENTKEY, F_ENTITY, F_ROW having count(*) > 1" : $"select F_PARENTKEY, F_ENTITY, F_ROW, F_SETKEY from {dopTypeTable} group by F_PARENTKEY, F_ENTITY, F_ROW, F_SETKEY having count(*) > 1"))}) tbl";
    IDbCommand command = this.plugin.idb.CreateCommand();
    command.CommandText = str1;
    try
    {
      if (System.Convert.ToInt64(command.ExecuteScalar()) <= 0L)
        return;
      string str2 = $"В базе Imbase в таблице {dopTypeTable} обнаружены не уникальные записи!{Environment.NewLine}Рекомендуется перед импортом запустить программу обновления БД Techcard TCPatch.exe для удаления дубликатов. Прервать импорт ?";
      this.plugin.appManager.AddWarningMessage(str2);
      string caption = "Внимание";
      if (MessageBox.Show(str2, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
        Application.Exit();
      else
        this.plugin.appManager.AddWarningMessage("Продолжение миграции...");
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddErrorMessage($"Ошибка при проверке данных дополнительных таблиц на наличие дубликатов:{Environment.NewLine}{ex.Message}{Environment.NewLine}{str1}");
      throw;
    }
  }

  protected virtual void AnalyzeStoppedData()
  {
    this._lastObjID = 0L;
    if (!(ServicesManager.GetService(typeof (ICache)) is ICache service))
      return;
    IImportingData cache = service.GetCache(this.GetTechCategory());
    if (cache == null)
      return;
    try
    {
      IDictionary<object, DictionaryValue> category = (IDictionary<object, DictionaryValue>) cache.GetCategory(this.GetTechCategory());
      foreach (object key in (IEnumerable<object>) category.Keys)
      {
        if (key is long val2)
          this._lastObjID = Math.Max(this._lastObjID, val2);
      }
      if (this.CheckCount <= 1)
        return;
      IUserSession userSession = this.plugin.Idw.GetUserSession();
      long objectId = 0;
      DictionaryValue dictionaryValue;
      if (category.TryGetValue((object) this._lastObjID, out dictionaryValue))
        objectId = dictionaryValue.NewObjectID;
      if (objectId == 0L)
        return;
      long lastObjectForType = this.GetLastObjectForType(userSession, this.objTypeID, objectId);
      if (lastObjectForType > 0L)
      {
        this._lastObjID = lastObjectForType;
      }
      else
      {
        List<long> list = category.Select<KeyValuePair<object, DictionaryValue>, long>((System.Func<KeyValuePair<object, DictionaryValue>, long>) (item => item.Value.NewObjectID)).ToList<long>();
        list.Sort();
        if (list.Count <= this.CheckCount)
        {
          this._lastObjID = 0L;
        }
        else
        {
          long lastPacketDbObjectId = list[list.Count - this.CheckCount];
          if (!(category.FirstOrDefault<KeyValuePair<object, DictionaryValue>>((System.Func<KeyValuePair<object, DictionaryValue>, bool>) (item => item.Value.NewObjectID == lastPacketDbObjectId)).Key is long key) || key >= this._lastObjID)
            return;
          this._lastObjID = key;
          this.plugin.appManager.AddInfoMessage($"В кэше миграции для предыдущего пакета найдена запись с Key = {this._lastObjID}");
        }
      }
    }
    finally
    {
      service.ReleaseCache(this.GetTechCategory());
    }
  }

  protected abstract void CheckBaseRecords();

  protected virtual bool CheckRecordLessThenLastKey(TechObjectRecord record)
  {
    return (long) record.baseKey < this._lastObjID;
  }

  protected virtual string GetRecordPumpMode(TechObjectRecord record)
  {
    if (this.CheckRecordLessThenLastKey(record))
    {
      record.RecMode = TechObjectRecord.PumpMode.NotPump;
      return string.Empty;
    }
    record.RecMode = TechObjectRecord.PumpMode.ObjectAndLinks;
    return string.Empty;
  }

  protected virtual string GetRecordWithParamsPumpMode(TechObjectRecord record)
  {
    string uniqueRecordHash = this.GetUniqueRecordHash((TechObjectRecordBase) record);
    if (string.IsNullOrEmpty(uniqueRecordHash) || this._import_data_main.GetValue(this.GetTechUniqueCategory(), (object) uniqueRecordHash) == null)
      return string.Empty;
    switch (record.RecMode)
    {
      case TechObjectRecord.PumpMode.ObjectAndLinks:
      case TechObjectRecord.PumpMode.LinkOnly:
        record.RecMode = TechObjectRecord.PumpMode.LinkOnly;
        break;
      case TechObjectRecord.PumpMode.ObjectOnly:
        record.RecMode = TechObjectRecord.PumpMode.NotPump;
        break;
    }
    return string.Empty;
  }

  protected virtual string GetUniqueRecordHash(TechObjectRecordBase record) => (string) null;

  protected virtual int GetObjectType(TechObjectRecordBase record) => this.objTypeID;

  protected virtual ObjectRecord CreateTechObject(TechObjectRecord record)
  {
    ObjectRecord objectRec = (ObjectRecord) null;
    string uniqueRecordHash = this.GetUniqueRecordHash((TechObjectRecordBase) record);
    TechObjectRecord.PumpMode pumpMode = record.RecMode;
    if (uniqueRecordHash == null && pumpMode == TechObjectRecord.PumpMode.LinkOnly)
      pumpMode = TechObjectRecord.PumpMode.ObjectAndLinks;
    switch (pumpMode)
    {
      case TechObjectRecord.PumpMode.ObjectAndLinks:
      case TechObjectRecord.PumpMode.ObjectOnly:
        objectRec = this._impObjList.AddObject(this.GetObjectType((TechObjectRecordBase) record), 0);
        if (!string.IsNullOrEmpty(uniqueRecordHash))
        {
          this._import_data_main.AddValue(this.GetTechUniqueCategory(), (object) uniqueRecordHash, objectRec.Object_id, objectRec.ObjectGuid.ToString());
          break;
        }
        break;
      case TechObjectRecord.PumpMode.LinkOnly:
        Guid objectGuid1 = Guid.Empty;
        DictionaryValue dictionaryValue = this._import_data_main.GetValue(this.GetTechUniqueCategory(), (object) uniqueRecordHash);
        long num = dictionaryValue != null ? dictionaryValue.NewObjectID : 0L;
        if (dictionaryValue != null)
        {
          if (num != 0L)
            dictionaryValue.Caption = (string) null;
          objectGuid1 = dictionaryValue.Caption == null || !GuidHelper.IsGuid(dictionaryValue.Caption) ? Guid.Empty : new Guid(dictionaryValue.Caption);
        }
        try
        {
          if (!(objectGuid1 == Guid.Empty))
          {
            if (this._impObjList.Items.UseObject(objectGuid1))
              goto label_28;
          }
          if (objectGuid1 != Guid.Empty)
          {
            string str = objectGuid1.ToString();
            foreach (KeyValuePair<object, DictionaryValue> keyValuePair in this.plugin.Imdi.ImportedObjects.Dictionary)
            {
              if (!(keyValuePair.Value.Caption != str))
              {
                num = (long) keyValuePair.Key;
                break;
              }
            }
          }
          if (num != 0L)
          {
            this._impObjList.UseObject(num);
            if (dictionaryValue != null)
            {
              if (!this._import_data_main.SetNewKey(this.GetTechUniqueCategory(), (object) uniqueRecordHash, num))
                dictionaryValue.NewObjectID = num;
            }
          }
          else
          {
            objectRec = this._impObjList.AddObject(this.GetObjectType((TechObjectRecordBase) record), 0);
            objectGuid1 = new Guid(objectRec.ObjectGuid.ToString());
            if (dictionaryValue != null)
              dictionaryValue.Caption = objectGuid1.ToString();
          }
        }
        catch (Exception ex)
        {
          this.RemoveBaseRec((TechObjectRecordBase) record);
          this.plugin.appManager.AddWarningMessage(string.Format("Невозможно добавить / модифицировать существующий объект ТП (typeID={3}) \"{0}\" по причине: {1}{2}", dictionaryValue != null ? (object) dictionaryValue.Caption : (object) string.Empty, (object) ex.Message, (object) (Environment.NewLine + ex.StackTrace), (object) this._recTypeID));
          if (ex is OutOfMemoryException)
            throw;
          this.DoHandleImportObjectsException(ex);
          return (ObjectRecord) null;
        }
label_28:
        if (objectRec == null)
        {
          object obj = (object) Guid.Empty;
          if (num != 0L)
            obj = (object) this.plugin.Imdi.ImportedObjects.GetGUID(num);
          else if (objectGuid1 != Guid.Empty)
          {
            int currentIndex = this._impObjList.Items.CurrentIndex;
            obj = currentIndex < 0 || !(this._impObjList.Items[currentIndex].Object.ObjectGuid is Guid objectGuid2) || !(objectGuid2 == objectGuid1) ? obj : this._impObjList.Items[currentIndex].Object.IdGuid;
          }
          if (obj is Guid guid && guid == Guid.Empty)
            obj = (object) Guid.NewGuid();
          objectRec = new ObjectRecord()
          {
            ObjectType = this.GetObjectType((TechObjectRecordBase) record),
            IdGuid = obj
          };
        }
        objectRec.ObjectGuid = (object) objectGuid1;
        objectRec.Object_id = num;
        break;
    }
    int currentIndex1 = this._impObjList.Items.CurrentIndex;
    this._techBaseImportList.Add((TechObjectRecordBase) record, currentIndex1);
    this.FillTechObject(objectRec, record);
    return objectRec;
  }

  protected virtual void FillTechObject(ObjectRecord objectRec, TechObjectRecord record)
  {
    if (objectRec == null || record == null || record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record.RecMode != TechObjectRecord.PumpMode.ObjectOnly)
      return;
    objectRec.Lc_step = this.GetFirstStepLifecycle();
    objectRec.IsBaseVersion = objectRec.ObjectVerType == 0;
    if (this.captionLength > 0 && this.captionLength < objectRec.Caption.Length)
      objectRec.Caption = objectRec.Caption.Remove(this.captionLength);
    if (this._atImbaseKeyAttr == null || this._imTableCode == 0 || this.entRefImbaseKey == null || this.entRefImRecKey == null || this.entRefImTblKey == null || this._techParmList.GetAttribute(this._atImbaseKeyAttr.ID) != null)
      return;
    object obj = this._techParmList.GetEntityValue(this.entRefImbaseKey.Code);
    if (obj == null || obj.Equals((object) string.Empty))
    {
      object entityValue1 = this._techParmList.GetEntityValue(this.entRefImRecKey.Code);
      if (entityValue1 != null && !entityValue1.Equals((object) string.Empty) && !entityValue1.Equals((object) DBNull.Value) && !entityValue1.Equals((object) 0))
      {
        object entityValue2 = this._techParmList.GetEntityValue(this.entRefImTblKey.Code);
        if (entityValue2 == null || entityValue2.Equals((object) string.Empty) || entityValue2.Equals((object) DBNull.Value) || entityValue2.Equals((object) 0))
          record.Fields.TryGetValue("F_TBLKEY", out entityValue2);
        int result1 = 0;
        int result2;
        int.TryParse(entityValue1.ToString(), out result2);
        if (entityValue2 != null)
          int.TryParse(entityValue2.ToString(), out result1);
        if (result2 != 0 || result1 != 0)
          obj = (object) TechcardConsts.Utils.GetImbaseKey(this._imTableCode, result2, result1);
      }
    }
    if (obj == null || obj.Equals((object) string.Empty))
      return;
    this._techParmList.AddAttribute(this._atImbaseKeyAttr, obj);
  }

  protected virtual void FillObjectObligatoryAttributes(TechObjectRecord record)
  {
    int objectIndex;
    if (!this._techBaseImportList.TryGetValue((TechObjectRecordBase) record, out objectIndex))
      this.plugin.appManager.AddWarningMessage("Не найден в кэше индекс для записи Key=" + (object) record.Key);
    else
      AttributesHelper.AddObligatoryObjectAttributes(this.plugin.Imdi.UserSession, this._impObjList, objectIndex);
  }

  protected virtual List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    return new List<TechRelParam>();
  }

  protected void CreateRelationByObjectAtEnt(ImportingObject importingObject, int relTypeId)
  {
    if (importingObject == null)
      throw new ArgumentNullException(nameof (importingObject));
    Guid objectGuid = (Guid) importingObject.Object.ObjectGuid;
    long objectId = importingObject.Object.Object_id;
    List<int> intList = new List<int>();
    int num1 = 0;
    int num2 = -1;
    for (int index1 = 0; index1 < this.relListPairs.Count; ++index1)
    {
      TechObjectRelationInfo relListPair1 = this.relListPairs[index1];
      if ((Guid) relListPair1.ObjEntRec.ObjectGuid == objectGuid && relListPair1.ObjEntRecID == 0L)
      {
        relListPair1.ObjEntRecID = objectId;
        num1 = relListPair1.Record.baseKey;
        num2 = index1;
      }
      if ((Guid) relListPair1.ObjRec.ObjectGuid == objectGuid && relListPair1.ObjRecID == 0L)
      {
        relListPair1.ObjRecID = objectId;
        num1 = relListPair1.Record.baseKey;
        num2 = index1;
      }
      if (relListPair1.ObjRecID != 0L && relListPair1.ObjEntRecID != 0L && !this.IsCloneRecord((TechObjectRecordBase) relListPair1.Record))
      {
        RelationRecord relationRecord1 = this._impRelList.AddRelation(relListPair1.ObjRecID, relListPair1.ObjEntRecID, relTypeId);
        if (relListPair1.ObjRec != null && relListPair1.ObjEntRec != null)
          this.FillLinkSortParam(new TechRelParam(relListPair1.ObjRecID, relListPair1.ObjEntRecID, relTypeId, relListPair1.ObjRec.ObjectType, relListPair1.ObjEntRec.ObjectType)
          {
            RelRec = relationRecord1
          }, (TechObjectRecordBase) relListPair1.Record);
        this.FillLinkObligatoryAttributes();
        intList.Add(index1);
        for (int index2 = index1 - 1; index2 >= 0; --index2)
        {
          TechObjectRelationInfo relListPair2 = this.relListPairs[index2];
          if (relListPair2.Record.baseKey == relListPair1.Record.baseKey)
          {
            if (this.IsCloneRecord((TechObjectRecordBase) relListPair2.Record) && relListPair2.ObjEntRec.ObjectType == relListPair1.ObjEntRec.ObjectType)
            {
              RelationRecord relationRecord2 = this._impRelList.AddRelation(relListPair2.ObjRecID, relListPair2.ObjEntRecID, relTypeId);
              if ((Guid) relationRecord1.PrjLinkGuid != Guid.Empty)
                this._impRelList.AddAttributeStr(this._atTechLinkAtRelGTPRelation.ID, relationRecord1.PrjLinkGuid.ToString());
              if (relListPair2.ObjRec != null && relListPair2.ObjEntRec != null)
                this.FillLinkSortParam(new TechRelParam(relListPair2.ObjRecID, relListPair2.ObjEntRecID, relTypeId, relListPair2.ObjRec.ObjectType, relListPair2.ObjEntRec.ObjectType)
                {
                  RelRec = relationRecord2
                }, (TechObjectRecordBase) relListPair2.Record);
              this.FillLinkObligatoryAttributes();
              intList.Add(index2);
            }
          }
          else
            break;
        }
      }
    }
    if (num1 > 0)
    {
      for (int index = num2 - 1; index >= 0; --index)
      {
        if (!intList.Contains(index))
        {
          TechObjectRelationInfo relListPair = this.relListPairs[index];
          if (this.IsCloneRecord((TechObjectRecordBase) relListPair.Record) && relListPair.Record.baseKey < num1 && relListPair.ObjRecID != 0L && relListPair.ObjEntRecID != 0L)
          {
            RelationRecord relationRecord = this._impRelList.AddRelation(relListPair.ObjRecID, relListPair.ObjEntRecID, relTypeId);
            if (relListPair.ObjRec != null && relListPair.ObjEntRec != null)
              this.FillLinkSortParam(new TechRelParam(relListPair.ObjRecID, relListPair.ObjEntRecID, relTypeId, relListPair.ObjRec.ObjectType, relListPair.ObjEntRec.ObjectType)
              {
                RelRec = relationRecord
              }, (TechObjectRecordBase) relListPair.Record);
            this.FillLinkObligatoryAttributes();
            intList.Add(index);
          }
        }
      }
    }
    if (intList.Count <= 0)
      return;
    intList.Sort();
    for (int index = intList.Count - 1; index >= 0; --index)
      this.relListPairs.RemoveAt(intList[index]);
  }

  protected virtual long AddHorizontalRelation(
    long parentRelId,
    long childRelId,
    Guid relationGuid)
  {
    return -1;
  }

  protected virtual TechRelParam AddRelationByObject(
    ImportingCategory category,
    object oldKey,
    int relationTypeId,
    TechObjectRecordBase recBase,
    long ipsObjId,
    int ipsObjTypeB,
    int ipsObjTypeA)
  {
    return this.AddRelationByObject(category, oldKey, relationTypeId, recBase, ipsObjId, recBase.diff_ArtTcKey, ipsObjTypeB, ipsObjTypeA);
  }

  protected virtual TechDiffTag GetTechDiffTagByOldKey(ImportingCategory category, object oldKey)
  {
    TechDiffTag techDiffTagByOldKey = (TechDiffTag) null;
    if (this._import_data_main.GetTag(category, oldKey) is TechRecordObjectTag tag)
      techDiffTagByOldKey = tag.TechDiffTag;
    return techDiffTagByOldKey;
  }

  protected virtual TechRelParam AddRelationByObject(
    ImportingCategory category,
    object oldKey,
    int relationTypeId,
    TechObjectRecordBase recBase,
    long ipsObjId,
    int artTcKey,
    int ipsObjTypeB,
    int ipsObjTypeA)
  {
    TechDiffTag techDiffTagByOldKey = this.GetTechDiffTagByOldKey(category, oldKey);
    if (techDiffTagByOldKey == null || techDiffTagByOldKey.IsCloneListEmpty)
      return (TechRelParam) null;
    long num;
    if (!techDiffTagByOldKey.CloneList.TryGetValue(artTcKey, out num))
      return (TechRelParam) null;
    RelationRecord relationRecord = this._impRelList.AddRelation(num, ipsObjId, relationTypeId);
    return new TechRelParam(num, ipsObjId, relationTypeId, ipsObjTypeB, ipsObjTypeA)
    {
      RelRec = relationRecord
    };
  }

  protected virtual int GetCurentObjectLifeC(TechObjectRecordBase record) => -1;

  protected int GetFirstStepLifecycle()
  {
    if (this._lcFirstStepId == -1)
    {
      if (this.objTypeID == -1)
      {
        this._lcFirstStepId = -1;
        return this._lcFirstStepId;
      }
      this._lcFirstStepId = this.plugin.Idw.GetUserSession().GetLifecycleStepCollection(this.objTypeID).GetFirstStep();
    }
    return this._lcFirstStepId;
  }

  protected bool IsCloneRecord(TechObjectRecordBase baseRecord) => baseRecord.diff_ArtTcKey != 0;

  protected abstract TechObjectRecord GetTpObjRec();

  protected virtual TechObjectRecordSub GetTpObjRecDop(string dopType)
  {
    return TechObjectRecordSubFactory.Create(dopType);
  }

  protected virtual void AddMessageIfRecordNotPump(TechObjectRecord record, string reason)
  {
    if (record.RecMode != TechObjectRecord.PumpMode.NotPump && record.RecMode != TechObjectRecord.PumpMode.Unknown || !(reason != string.Empty))
      return;
    this.plugin.appManager.AddWarningMessage($"Запись {(object) record.Key} из таблицы {record.TableName} не качается по причине: {reason}");
  }

  protected virtual void AddObject2Cache(ImportingObject itemImpObject)
  {
    if (itemImpObject == null)
      throw new ArgumentNullException(nameof (itemImpObject));
    if (itemImpObject.Object == null || itemImpObject.Object.Object_id == 0L)
      return;
    ImportingCategory objectExCategory = this.GetTechObjectExCategory();
    if (objectExCategory == ImportingCategory.None)
      return;
    this._import_data_main.AddValue(objectExCategory, (object) itemImpObject.Object.Object_id, itemImpObject.Object.ModifyDate.Ticks);
  }

  protected virtual void AddValue4Clone2Cache(
    IImportingData masterImport,
    object oldKey,
    long newKey,
    TechObjectRecordBase recBase)
  {
    masterImport.AddValue(this.GetTechCategory(), oldKey, newKey);
  }

  protected virtual void AddValue2Cache(
    object oldKey,
    long newKey,
    TechObjectRecordBase recBase,
    TechParamList recParmList)
  {
    IImportingData importDataMain = this._import_data_main;
    string uniqueRecordHash = this.GetUniqueRecordHash(recBase);
    if (!string.IsNullOrEmpty(uniqueRecordHash))
    {
      DictionaryValue dictionaryValue = this._import_data_main.GetValue(this.GetTechUniqueCategory(), (object) uniqueRecordHash);
      if (dictionaryValue != null)
      {
        if (!this._import_data_main.SetNewKey(this.GetTechUniqueCategory(), (object) uniqueRecordHash, newKey))
          dictionaryValue.NewObjectID = newKey;
        dictionaryValue.Caption = (string) null;
      }
    }
    if (this.IsCloneRecord(recBase))
    {
      int diffArtTcKey = recBase.diff_ArtTcKey;
      if (diffArtTcKey != 0)
      {
        Dictionary<int, long> dictionary;
        if (!this._curArt2ObjectIdList.TryGetValue(recBase.baseKey, out dictionary))
        {
          dictionary = new Dictionary<int, long>();
          this._curArt2ObjectIdList.Add(recBase.baseKey, dictionary);
        }
        dictionary?.Add(diffArtTcKey, newKey);
      }
      this.AddValue4Clone2Cache(importDataMain, oldKey, newKey, recBase);
    }
    else
    {
      TechRecordObjectTag tag = this.GetTagValue4Cache(oldKey, newKey, recBase);
      Dictionary<int, long> dictionary;
      if (!this._curArt2ObjectIdList.TryGetValue(recBase.baseKey, out dictionary))
      {
        DictionaryValue dictionaryValue = importDataMain.GetValue(this.GetTechCategory(), oldKey);
        if (dictionaryValue == null)
        {
          if (tag != null)
            importDataMain.AddValue(this.GetTechCategory(), oldKey, newKey, (ITagImportObject) tag);
          else
            importDataMain.AddValue(this.GetTechCategory(), oldKey, newKey);
        }
        else
        {
          if (dictionaryValue.NewObjectID != newKey)
            importDataMain.SetNewKey(this.GetTechCategory(), oldKey, newKey);
          dictionaryValue.NewObjectID = newKey;
          dictionaryValue.Tag = (ITagImportObject) tag;
        }
      }
      else
      {
        foreach (KeyValuePair<int, long> keyValuePair in dictionary)
          this.AddHorizontalRelation(newKey, keyValuePair.Value, Guid.Empty);
        if (tag == null)
          tag = new TechRecordObjectTag((object) null);
        tag.TechDiffTag = new TechDiffTag()
        {
          CloneList = dictionary
        };
        importDataMain.AddValue(this.GetTechCategory(), oldKey, newKey, (ITagImportObject) tag);
        this._curArt2ObjectIdList.Remove(recBase.baseKey);
      }
    }
  }

  protected virtual TechRecordObjectTag GetTagValue4Cache(
    object oldKey,
    long newKey,
    TechObjectRecordBase recBase)
  {
    return (TechRecordObjectTag) null;
  }

  public virtual void LoadTechParams(TechObjectRecord record)
  {
    this._techParmList.Clear();
    if (record == null)
      return;
    this.LoadTechBaseParams(record);
    foreach (string dopType in this._dopTypes)
    {
      if (!(dopType == "D"))
        this.LoadTechSubParams((TechObjectRecordBase) record, dopType);
    }
    if (this._dopTypes.Contains("D"))
      this.LoadTechSubParams((TechObjectRecordBase) record, "D");
    this.LoadTechDiffParams(record);
  }

  protected virtual void LoadTechBaseParams(TechObjectRecord record)
  {
    if (record == null)
      return;
    foreach (string key in (IEnumerable<string>) record.Fields.Keys)
    {
      string entity = TechPumpData.EntFixList.GetEntity(this.TableName, key);
      if (TechcardConsts.TechcardCommon.Code2AttributeGuid.ContainsKey(entity))
      {
        object fieldValue = record.GetFieldValue(key);
        this._techParmList.AddEntity(entity, fieldValue);
      }
    }
  }

  protected virtual void LoadTechSubParams(TechObjectRecordBase recBase, string dopType)
  {
    if (recBase == null || this._techDataRecCache == null)
      return;
    List<TechObjectDataSub> techRecs = this._techDataRecCache.GetTechRecs(dopType, recBase.baseKey);
    if (techRecs == null)
      return;
    if (dopType.Contains("D"))
    {
      this.LoadTechSubParams_D(techRecs);
    }
    else
    {
      foreach (TechObjectDataSub techDataRec in techRecs)
        this.LoadTechSubParamsCustom(techDataRec, dopType);
    }
  }

  protected void LoadTechSubParams_D(List<TechObjectDataSub> techDataRecList)
  {
    if (techDataRecList == null || techDataRecList.Count == 0)
      return;
    using (ObjectPoolScope<StringBuilder> objectPoolScope = TextServices.StringBuilderPool.Allocate(2048 /*0x0800*/))
    {
      foreach (TechObjectDataSub techDataRec in techDataRecList)
      {
        string code = string.Empty;
        objectPoolScope.Object.Clear();
        foreach (TechObjectRecordSub_D record in techDataRec.Records)
        {
          if (record.Value is string str)
          {
            int length = str.Length;
            if (length > 0 && str[length - 1] == '$')
              record.Value = (object) str.Remove(length - 1);
          }
          if (code != record.Entity)
          {
            if (objectPoolScope.Object.Length != 0)
            {
              this._techParmList.AddEntity(code, (object) objectPoolScope.Object.ToString());
              objectPoolScope.Object.Clear();
            }
            code = record.Entity;
            objectPoolScope.Object.Append(record.Value);
          }
          else
            objectPoolScope.Object.Append(record.Value);
        }
        if (objectPoolScope.Object.Length != 0)
          this._techParmList.AddEntity(code, (object) objectPoolScope.Object.ToString());
      }
    }
  }

  protected virtual void LoadTechSubParamsCustom(TechObjectDataSub techDataRec, string dopType)
  {
    if (techDataRec == null || dopType != "I" && dopType != "F" && dopType != "S" && !dopType.Contains("_I") && !dopType.Contains("_F") && !dopType.Contains("_S"))
      return;
    Dictionary<int, Entity> entListByDopType = this.entTypeRec.GetEntListByDopType(dopType);
    foreach (TechObjectRecordSub record in techDataRec.Records)
    {
      int row = record.Row;
      foreach (string key1 in (IEnumerable<string>) record.Fields.Keys)
      {
        if (!key1.Equals(string.Empty))
        {
          object fieldValue = record.GetFieldValue(key1);
          if (fieldValue != null)
          {
            string str = key1.Remove(0, 1);
            int key2 = 10 * (row - 1) + System.Convert.ToInt32(str);
            Entity entity;
            if (entListByDopType.TryGetValue(key2, out entity) && entity != null)
              this._techParmList.AddEntity(entity.Code, fieldValue);
          }
        }
      }
    }
  }

  protected virtual void LoadTechDiffParams(TechObjectRecord record)
  {
    if (record == null || !this._allowDiffObjects || !this.IsCloneRecord((TechObjectRecordBase) record))
      return;
    List<TechDiffElement> techDiffElementList = (List<TechDiffElement>) null;
    TechDiffRec techDiffRec;
    if (TechDiffCache.DiffRecList.TryGetValue(record.baseKey, out techDiffRec))
      techDiffElementList = techDiffRec.Diff;
    if (techDiffElementList == null)
      return;
    foreach (TechDiffElement techDiffElement in techDiffElementList)
    {
      if (techDiffElement.ArtTcKey == record.diff_ArtTcKey && !techDiffElement.Entity.Equals(string.Empty))
      {
        string entity = techDiffElement.Entity;
        object obj = (object) null;
        switch (techDiffElement.EntType)
        {
          case 0:
            obj = (object) techDiffElement.StrValue;
            break;
          case 1:
            obj = (object) techDiffElement.NumValue;
            break;
        }
        this._techParmList.AddOrUpdateEntity(entity, obj);
      }
    }
  }

  protected virtual void AddAttribute2ImpObjectList(ITechParamAttribute techAttribute)
  {
    if (techAttribute != null && techAttribute.AttributeType == null)
    {
      this.plugin.appManager.AddInfoMessage("Атрибут не настроен!");
    }
    else
    {
      if (this._impObjList == null)
        return;
      this._attributeWriter.Write((IImportedAttributeList) this._impObjList, techAttribute);
    }
  }

  protected virtual void AddAttribute2ImpRelationList(ITechParamAttribute techAttribute)
  {
    if (techAttribute != null && techAttribute.AttributeType == null)
    {
      this.plugin.appManager.AddInfoMessage("Атрибут не настроен!");
    }
    else
    {
      if (this._impRelList == null)
        return;
      this._attributeWriter.Write((IImportedAttributeList) this._impRelList, techAttribute);
    }
  }

  protected virtual void PumpLoadData()
  {
    this.plugin.appManager.AddInfoMessage("Формирования источника даных");
    TechDataSource dataSource = this.GetDataSource();
    try
    {
      this.plugin.appManager.AddInfoMessage("Получение DataReader");
      TechDataReaderInfo dataReaderInfo1 = dataSource.GetDataReaderInfo(string.Empty);
      if (dataReaderInfo1 == null)
        this.plugin.appManager.AddErrorMessage($"DataReader для типа {this._recType} не найден.");
      else if (ServiceUtils.GetService<ICache>((object) ServicesManager.ServiceContainer, false) == null)
      {
        this.plugin.appManager.AddWarningMessage($"Интерфейс для работы с закэшированными импортированными данными для \"{(object) this.GetTechCategory()}\" не получен");
      }
      else
      {
        this.plugin.appManager.AddInfoMessage($"Количество основных записей источника данных: {dataReaderInfo1.RecordCount}");
        this.plugin.appManager.AddInfoMessage("Инициализация полей схемы данных");
        this.GetTpObjRec().ParseSchema((IDictionary<string, int>) this.GetTableColumns(dataReaderInfo1.DataReader));
        foreach (string dopType in this._dopTypes)
        {
          TechDataReaderInfo dataReaderInfo2 = dataSource.GetDataReaderInfo(dopType);
          if (dataReaderInfo2 != null)
            this.GetTpObjRecDop(dopType).ParseSchema((IDictionary<string, int>) this.GetTableColumns(dataReaderInfo2.DataReader));
        }
        int val1 = 0;
        this.plugin.appManager.AddInfoMessage("Вызов DataReader");
        IDataReader dataReader = dataReaderInfo1.DataReader;
        this.plugin.appManager.AddInfoMessage("Обработка записей DataReader");
        while (dataReader.Read())
        {
          TechObjectRecord tpObjRec = this.GetTpObjRec();
          if (tpObjRec != null)
          {
            this.PumpLoadDataRec(dataReader, tpObjRec);
            string reason = this.GetRecordPumpMode(tpObjRec);
            if (tpObjRec.RecMode != TechObjectRecord.PumpMode.NotPump && tpObjRec.RecMode != TechObjectRecord.PumpMode.Unknown)
            {
              foreach (string dopType in this._dopTypes)
                this.PumpLoadSubData((TechObjectRecordBase) tpObjRec, dopType);
              this._techParmList = new TechParamList();
              this.LoadTechParams(tpObjRec);
              reason = this.GetRecordWithParamsPumpMode(tpObjRec);
            }
            if (tpObjRec.RecMode != TechObjectRecord.PumpMode.NotPump)
            {
              if (tpObjRec.RecMode != TechObjectRecord.PumpMode.Unknown)
              {
                try
                {
                  TechParamList techParmList = this._techParmList;
                  this.PumpDiffRec(tpObjRec);
                  this._techParmList = techParmList;
                  this.PumpBaseRec(tpObjRec);
                  goto label_25;
                }
                finally
                {
                  this._techDataRecCache.RemoveTechRecs(tpObjRec.Key);
                }
              }
            }
            this.AddMessageIfRecordNotPump(tpObjRec, reason);
          }
label_25:
          ++val1;
          if (val1 % this.CheckCount == 0 || val1 == dataReaderInfo1.RecordCount)
            this.PumpCheckPoint($"Закачка типов записей ТП ({val1} из {dataReaderInfo1.RecordCount})", this.CalculatePercent(dataReaderInfo1.RecordCount, Math.Min(val1, dataReaderInfo1.RecordCount), 0, 100));
        }
        this.ExamCheckPoint($"Закачка данных для элементов ТП: {this._recType} успешно завершена", 100);
      }
    }
    finally
    {
      dataSource.Close();
    }
  }

  protected void PumpLoadDataRec(IDataReader dataReader, TechObjectRecord record)
  {
    if (dataReader == null || record == null)
      return;
    record.Parse(dataReader);
  }

  protected virtual void PumpLoadSubData(TechObjectRecordBase recordBase, string dopType)
  {
    this.ExamCheckPoint($"Инициализация загрузки типа данных: {dopType} для элементов ТП: {this._recType}", 0);
    TechDataReaderInfo dataReaderInfo = this.GetDataSource().GetDataReaderInfo(dopType);
    if (dataReaderInfo == null)
    {
      this.plugin.appManager.AddWarningMessage($"DataReader для типа {this._recType}:{dopType} не найден.");
    }
    else
    {
      try
      {
        IDataReader dataReader = dataReaderInfo.DataReader;
        int val1 = 0;
        TechObjectRecordSub dopRecord;
        if (this._dopRecLastList.TryGetValue(dopType, out dopRecord) && dopRecord != null && this.PumpLoadSubData_Loaded(dopType, recordBase, dopRecord))
          return;
        while (dataReader.Read())
        {
          TechObjectRecordSub tpObjRecDop = this.GetTpObjRecDop(dopType);
          if (tpObjRecDop == null)
            break;
          this.PumpLoadSubDataRec(dopType, dataReader, (TechObjectRecordBase) tpObjRecDop);
          this._techDataRecCache.AddTechDataRec(dopType, tpObjRecDop);
          this._dopRecLastList[dopType] = tpObjRecDop;
          ++val1;
          if (val1 % this.CheckCount == 0 || val1 == dataReaderInfo.RecordCount)
            this.ExamCheckPoint($"Считывание типов записей ТП ({val1} из {dataReaderInfo.RecordCount})", this.CalculatePercent(dataReaderInfo.RecordCount, Math.Min(val1, dataReaderInfo.RecordCount), 1, 10));
          if (this.PumpLoadSubData_Loaded(dopType, recordBase, tpObjRecDop))
            break;
        }
      }
      finally
      {
        this.ExamCheckPoint($"Загрузка типа данных: {dopType} для элементов ТП: {this._recType} успешно завершена", 100);
      }
    }
  }

  protected virtual void PumpLoadSubDataRec(
    string dopType,
    IDataReader dataReader,
    TechObjectRecordBase record)
  {
    if (dataReader == null || record == null)
      return;
    record.Parse(dataReader);
  }

  protected virtual bool PumpLoadSubData_Loaded(
    string dopType,
    TechObjectRecordBase recBase,
    TechObjectRecordSub dopRecord)
  {
    return dopRecord.ParentKey > recBase.Key;
  }

  protected virtual void PumpDiffRec(TechObjectRecord record)
  {
    if (!this._allowDiffObjects)
      return;
    int key = record.Key;
    TechDiffRec techDiffRec;
    if (!TechDiffCache.DiffRecList.TryGetValue(key, out techDiffRec) || techDiffRec == null)
      return;
    List<int> intList = new List<int>();
    foreach (TechDiffElement techDiffElement in techDiffRec.Diff)
    {
      if (!intList.Contains(techDiffElement.ArtTcKey))
      {
        TechObjectRecord cloneRecord = this.CreateCloneRecord(record);
        cloneRecord.Key = -techDiffElement.Key;
        cloneRecord.diff_ArtTcKey = techDiffElement.ArtTcKey;
        this.BeforePumpCloneRec(record, cloneRecord);
        this._techParmList = new TechParamList();
        this.LoadTechParams(cloneRecord);
        this.PumpBaseRec(cloneRecord);
        this.AfterPumpCloneRec(record, cloneRecord);
        intList.Add(techDiffElement.ArtTcKey);
      }
    }
  }

  protected virtual void PumpBaseRec(TechObjectRecord record)
  {
    bool flag = true;
    try
    {
      if (this.CheckRecordLessThenLastKey(record) || this._import_data_main.GetValue(this.GetTechCategory(), (object) record.Key) != null)
        return;
      flag = false;
      ObjectRecord techObject = this.CreateTechObject(record);
      this.tpObjRecList[(TechObjectRecordBase) record] = this._techParmList;
      this.FillObjectParams(record, this.tpObjRecList[(TechObjectRecordBase) record], techObject);
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage($"Ошибка обработки записи \"{record.Key}\" таблицы \"{record.TableName}\": {ex.Message}{Environment.NewLine + ex.StackTrace}");
      if (ex is OutOfMemoryException)
        throw;
      this.DoHandleImportObjectsException(ex);
    }
    finally
    {
      if (flag)
        this.tpObjRecList.Remove((TechObjectRecordBase) record);
    }
  }

  protected virtual void PumpLoadTechDiffData()
  {
    if (TechDiffCache.DiffPumper != null)
    {
      TechDiffCache.DiffPumper.LoadDiffData((int) this._lastObjID, this._recTypeID);
      this._allowDiffObjects = TechDiffCache.DiffRecList.Count > 0;
    }
    else
      this._allowDiffObjects = false;
  }

  protected TechObjectRecord CreateCloneRecord(TechObjectRecord record)
  {
    TechObjectRecord tpObjRec = this.GetTpObjRec();
    tpObjRec.Assign((object) record);
    return tpObjRec;
  }

  protected virtual void BeforePumpCloneRec(TechObjectRecord record, TechObjectRecord recordClone)
  {
  }

  protected virtual void AfterPumpCloneRec(TechObjectRecord record, TechObjectRecord recordClone)
  {
  }

  protected virtual void RemoveBaseRec(TechObjectRecordBase recBase)
  {
    if (recBase == null)
      return;
    this.tpObjRecList.Remove(recBase);
    this._techBaseImportList.Remove(recBase);
  }

  protected virtual void TechBase_AfterImportObjectEvent(object sender, EventArgs e)
  {
    if (!(sender is IImportedObjectList importedObjectList))
      return;
    if (this._import_data_main == null)
      return;
    try
    {
      List<TechObjectRecordBase> objectRecordBaseList1 = new List<TechObjectRecordBase>(this._techBaseImportList.Count);
      Dictionary<TechObjectRecordBase, long> dictionary1 = new Dictionary<TechObjectRecordBase, long>(importedObjectList.Items.Count);
      for (int index = 0; index < importedObjectList.Items.Count; ++index)
      {
        objectRecordBaseList1.Clear();
        foreach (KeyValuePair<TechObjectRecordBase, int> techBaseImport in this._techBaseImportList)
        {
          if (techBaseImport.Value == index)
            objectRecordBaseList1.Add(techBaseImport.Key);
        }
        Exception importError = importedObjectList.GetImportError(index);
        if (importError != null)
          this.plugin.appManager.AddWarningMessage($"Объект(ы) идентичные записи {string.Join(",", Array.ConvertAll<TechObjectRecordBase, string>(objectRecordBaseList1.ToArray(), new Converter<TechObjectRecordBase, string>(System.Convert.ToString)))} из таблицы {this._tableName} не импортирован, по причине: {importError.Message}");
        ImportingObject importingObject = importedObjectList.Items[index];
        if (importingObject != null && importingObject.Object != null)
        {
          if (objectRecordBaseList1.Count != 0)
          {
            long objectId = importingObject.Object.Object_id;
            if (objectId != 0L)
            {
              foreach (TechObjectRecordBase key in objectRecordBaseList1)
                dictionary1.Add(key, objectId);
            }
          }
          if (importingObject.Object.ObjectGuid != null)
          {
            this.CreateRelationByObjectAtEnt(importingObject, this._relTechRelationID);
            this.AddObject2Cache(importingObject);
          }
        }
      }
      List<TechObjectRecordBase> objectRecordBaseList2 = new List<TechObjectRecordBase>(this.tpObjRecList.Count);
      for (int index = 0; index < 2; ++index)
      {
        foreach (KeyValuePair<TechObjectRecordBase, TechParamList> tpObjRec in this.tpObjRecList)
        {
          TechObjectRecordBase key = tpObjRec.Key;
          try
          {
            if (this.IsCloneRecord(key))
            {
              if (index == 1)
                continue;
            }
            else if (index == 0)
              continue;
            if (tpObjRec.Value != null)
            {
              long newKey;
              if (!dictionary1.TryGetValue(key, out newKey))
              {
                objectRecordBaseList2.Add(key);
              }
              else
              {
                this._techBaseImportList.Remove(key);
                objectRecordBaseList2.Add(key);
                this.AddValue2Cache((object) key.Key, newKey, key, tpObjRec.Value);
              }
            }
          }
          catch (Exception ex)
          {
            this.plugin.appManager.AddWarningMessage($"Ошибка импорта записи пакета  (Таблица - {key.TableName}, Key - {key.Key}): {ex.Message}{Environment.NewLine + ex.StackTrace}");
            if (ex is OutOfMemoryException)
              throw;
          }
        }
      }
      Dictionary<TechObjectRecordBase, List<TechRelParam>> dictionary2 = new Dictionary<TechObjectRecordBase, List<TechRelParam>>();
      for (int index1 = 0; index1 < 2; ++index1)
      {
        foreach (KeyValuePair<TechObjectRecordBase, TechParamList> tpObjRec in this.tpObjRecList)
        {
          TechObjectRecordBase key = tpObjRec.Key;
          try
          {
            if (this.IsCloneRecord(key))
            {
              if (index1 == 0)
                continue;
            }
            else if (index1 == 1)
              continue;
            if (tpObjRec.Value != null)
            {
              long num;
              if (dictionary1.TryGetValue(key, out num))
              {
                this.SaveLinkSuspendedParams(num, tpObjRec.Value);
                List<TechRelParam> techRelList = this.CreateTechRelList(key, num);
                for (int index2 = 0; index2 < techRelList.Count; ++index2)
                {
                  TechRelParam techRelParam = techRelList[index2];
                  if (techRelParam != null)
                  {
                    if (techRelParam.RelRec == null)
                    {
                      RelationRecord relationRecord = this._impRelList.AddRelation(techRelParam.IpsObjectBid, techRelParam.IpsObjectAid, techRelParam.RelType);
                      techRelParam.RelRec = relationRecord;
                    }
                    TechParamList parmList = tpObjRec.Value;
                    if (parmList != null)
                      this.FillLinkParams(key, techRelParam, parmList);
                    this.FillLinkSortParam(techRelParam, key);
                    this.FillLinkSuspendedParams(key, num, parmList, techRelParam);
                    List<TechRelParam> techRelParamList;
                    if (index1 == 1 && dictionary2.TryGetValue(key, out techRelParamList) && index2 <= techRelParamList.Count)
                    {
                      Guid prjLinkGuid = (Guid) techRelParamList[index2].RelRec.PrjLinkGuid;
                      if (prjLinkGuid != Guid.Empty && this._atTechLinkAtRelGTPRelation != null)
                        this._impRelList.AddAttributeStr(this._atTechLinkAtRelGTPRelation.ID, prjLinkGuid.ToString());
                    }
                    this.FillLinkObligatoryAttributes();
                  }
                }
                if (index1 == 0)
                {
                  if (techRelList.Count != 0)
                    dictionary2.Add(key, techRelList);
                }
              }
            }
          }
          catch (Exception ex)
          {
            this.plugin.appManager.AddWarningMessage($"Ошибка создания связей для записи пакета (Таблица - {key.TableName}, Key - {key.Key}): {ex.Message}{Environment.NewLine + ex.StackTrace}");
            if (ex is OutOfMemoryException)
              throw;
            this.DoHandleImportRelationsException(ex);
          }
        }
      }
      foreach (TechObjectRecordBase recBase in objectRecordBaseList2)
        this.RemoveBaseRec(recBase);
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage($"Ошибка импорта текщего пакета: {ex.Message}");
      if (!(ex is OutOfMemoryException))
        return;
      throw;
    }
  }

  protected virtual void impRelList_AfterImportEvent(object sender, EventArgs e)
  {
  }

  protected Dictionary<string, Guid> GetAttributeGuidDictionary(ISaveSettings ss)
  {
    Dictionary<string, Guid> attributeGuidDictionary = new Dictionary<string, Guid>();
    if (ss == null)
    {
      this.plugin.appManager.AddWarningMessage("Невозможно получить список атрибутов на поля IMBASE т.к. интерфейс кэша сохраненных настроек не найден. Это может привести к невозможности закачки анкет правил автоподбора");
      return attributeGuidDictionary;
    }
    try
    {
      string settingsName = "IMBASEFIELD";
      Dictionary<string, SaveSettingsAttribute[]> settings = ss.GetSettings(settingsName);
      if (settings == null)
        throw new Exception("В списке сохраненных настроек не найдены правила перекачки полей IMBASE в атрибуты IPS");
      foreach (KeyValuePair<string, SaveSettingsAttribute[]> keyValuePair in settings)
      {
        Guid guid = new Guid(keyValuePair.Value[0].AttributeValue);
        attributeGuidDictionary.Add(keyValuePair.Key, guid);
      }
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage($"Невозможно получить список атрибутов на поля IMBASE от сервиса сохраненных настроек. Это может привести к невозможности закачки анкет правил автоподбора. Ошибка: {ex.Message}");
      if (ex is OutOfMemoryException)
        throw;
    }
    return attributeGuidDictionary;
  }

  protected virtual void ClearTmpData() => this.DeleteTmpFiles();

  protected virtual void ReleasePumpData()
  {
    this.ClearTmpData();
    this._dataSource = (TechDataSource) null;
    this._curArt2ObjectIdList.Clear();
    this._curArt2ObjectIdList = (Dictionary<int, Dictionary<int, long>>) null;
    this._dopTypes.Clear();
    this._dopTypes = (List<string>) null;
    this.tpObjRecList.Clear();
    this.tpObjRecList = (Dictionary<TechObjectRecordBase, TechParamList>) null;
    this._techParmList.Clear();
    this._techParmList = (TechParamList) null;
    this.entTypeRec = (EntityTypeRec) null;
    this._techDataRecCache.Clear();
    this._techDataRecCache = (TechObjectDataSubCache) null;
    this._impObjList = (IImportedObjectList) null;
    this._impRelList = (IImportedRelationList) null;
    this._techBaseImportList.Clear();
    this._techBaseImportList = (Dictionary<TechObjectRecordBase, int>) null;
    this.objAttrList.Clear();
    this.objAttrList = (List<Guid>) null;
    this.relAttrList.Clear();
    this.relAttrList = (List<Guid>) null;
    this.relListPairs.Clear();
    this.relListPairs = (List<TechObjectRelationInfo>) null;
    this._entityConverter = (TechEntityConverter) null;
    this._attributeWriter = (TechAttributeWriter) null;
    ImportingCategoryDataCache.Instance.ClearCaches();
  }

  protected virtual void DoAfterPump()
  {
  }

  protected virtual void InitData()
  {
    this._recType = string.Empty;
    this._tableName = string.Empty;
  }

  protected string GetTmpFileName() => TechUtils.File.GetTmpFileName(this.GUID);

  private void DeleteTmpFiles()
  {
    TechUtils.File.DeleteTmpFiles(this.GUID);
    TechUtils.File.DeleteTmpFiles(TechPumpBase.cnt_Common_Folder_Guid);
  }

  protected TechPumpBase(PluginClass plugin)
    : base(plugin)
  {
    this.taskExam.Repumpble = true;
    this.taskPump.Repumpble = true;
    this._entityConverter = new TechEntityConverter((PumpClass) this, new ObjectForAttributeFieldTypeService<TechEntityConvertStrategy>(new ObjectForAttributeFieldTypeFactory<TechEntityConvertStrategy>()));
    this._attributeWriter = new TechAttributeWriter((PumpClass) this, new ObjectForAttributeFieldTypeService<TechAttributeWriteStrategy>(new ObjectForAttributeFieldTypeFactory<TechAttributeWriteStrategy>()));
    this._dopTypes = new List<string>();
    this.tpObjRecList = new Dictionary<TechObjectRecordBase, TechParamList>();
    this._techDataRecCache = new TechObjectDataSubCache();
    this._techParmList = new TechParamList();
    this.objAttrList = new List<Guid>();
    this.relAttrList = new List<Guid>();
    this._techBaseImportList = new Dictionary<TechObjectRecordBase, int>();
    this.InitData();
    this.CheckBaseRecords();
  }

  public int CheckCount
  {
    get
    {
      return !(ServicesManager.GetService(typeof (IConfigurationService)) is IConfigurationService service) ? 1 : service.Configuration.PacketSize;
    }
  }

  public virtual void FillObjectParams(
    TechObjectRecord record,
    TechParamList paramList,
    ObjectRecord objectRec)
  {
    if (objectRec != null)
      this.FillRecordParamsFixed(record, paramList);
    this.FillRecordParams2Attribute(record, paramList, objectRec);
    this.FillObjectObligatoryAttributes(record);
    this.FillRecordParams2NewObject(record, paramList, objectRec);
  }

  protected void FillRecordParams2Attribute(
    TechObjectRecord record,
    TechParamList parmList,
    ObjectRecord objectRec)
  {
    if (parmList == null || parmList.Count == 0 || objectRec == null || record.RecMode != TechObjectRecord.PumpMode.ObjectOnly && record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks)
      return;
    foreach (ITechParamBase parm in (List<ITechParamBase>) parmList)
    {
      try
      {
        switch (parm.GetTechParamType())
        {
          case Intermech.ImpExp.TechCard.TechProcPump.Common.TechParamType.Attribute:
            ITechParamAttribute techAttribute1 = parm as ITechParamAttribute;
            if (techAttribute1.AttributeBelongs != EntitySetting.AttributeBelongs.ToObject)
            {
              if (techAttribute1.AttributeBelongs != EntitySetting.AttributeBelongs.ToLinkAndObject)
                continue;
            }
            this.AddAttribute2ImpObjectList(techAttribute1);
            continue;
          case Intermech.ImpExp.TechCard.TechProcPump.Common.TechParamType.Entity:
            ITechParamEntity techEntity = parm as ITechParamEntity;
            Guid attrGuid;
            if (TechcardConsts.TechcardCommon.Code2AttributeGuid.TryGetValue(techEntity.Code, out attrGuid))
            {
              if (!(attrGuid == Guid.Empty))
              {
                Entity entityByCode = this.GetEntityByCode(techEntity.Code);
                if (entityByCode != null)
                {
                  if (!(entityByCode.Settings.ObjectType != Guid.Empty))
                  {
                    if (!this.CheckObjTypeOrParamType(techEntity.Code, attrGuid))
                    {
                      this.FillObjСhangedOrParamType(record, parm);
                      continue;
                    }
                    if (!(attrGuid == Guid.Empty))
                    {
                      if (entityByCode.Settings.AttributeBelong != EntitySetting.AttributeBelongs.ToLinkAndObject)
                      {
                        if (entityByCode.Settings.AttributeBelong != EntitySetting.AttributeBelongs.ToObject)
                          continue;
                      }
                      ITechParamAttribute techAttribute2 = this._entityConverter.Convert((TechObjectRecordBase) record, this._techParmList, techEntity, entityByCode);
                      if (techAttribute2 != null)
                      {
                        this.AddAttribute2ImpObjectList(techAttribute2);
                        continue;
                      }
                      continue;
                    }
                    continue;
                  }
                  continue;
                }
                continue;
              }
              continue;
            }
            continue;
          default:
            continue;
        }
      }
      catch (Exception ex)
      {
        this.plugin.appManager.AddWarningMessage($"Невозможно добавить атрибут {parm.ToString()} по причине: {ex.Message}");
        if (ex is OutOfMemoryException)
          throw;
      }
    }
  }

  protected Entity GetEntityByCode(string entityCode)
  {
    Entity entityByCode;
    if (this.entTypeRec == null || !this.entTypeRec.CodeList.TryGetValue(entityCode, out entityByCode))
      TechPumpData.Entities.EntitiesList.TryGetValue(entityCode, out entityByCode);
    return entityByCode;
  }

  public virtual void FillRecordParamsFixed(TechObjectRecord record, TechParamList parmList)
  {
    if (record == null || parmList == null)
      return;
    if (this._atTechTypeKeyAttr != null)
      parmList.AddAttribute(this._atTechTypeKeyAttr, (object) record.Key);
    if (this._atTechArtAttr == null || record.diff_ArtTcKey <= 0)
      return;
    parmList.AddAttribute(this._atTechArtAttr, (object) record.diff_ArtTcKey);
  }

  public virtual void FillObjСhangedOrParamType(TechObjectRecord record, ITechParamBase techParm)
  {
  }

  private void FillObjectRecord4Ent2Obj(
    TechObjectRecord record,
    TechParamList techParmList,
    ObjectRecord entObjRec)
  {
    entObjRec.IsBaseVersion = true;
    foreach (ITechParamBase techParm in (List<ITechParamBase>) techParmList)
    {
      if (techParm is ITechParamAttribute techAttribute && (techAttribute.AttributeType == this._atTechTypeKeyAttr || techAttribute.AttributeType == this._atTechArtAttr))
        this.AddAttribute2ImpObjectList(techAttribute);
    }
  }

  protected virtual void FillRecordParams2NewObject(
    TechObjectRecord record,
    TechParamList techParmList,
    ObjectRecord objectRec)
  {
    if (techParmList == null || techParmList.Count == 0)
      return;
    Dictionary<(int, string), List<ITechParamBase>> dictionary = new Dictionary<(int, string), List<ITechParamBase>>();
    foreach (ITechParamBase techParm in (List<ITechParamBase>) techParmList)
    {
      if (techParm.GetTechParamType() != Intermech.ImpExp.TechCard.TechProcPump.Common.TechParamType.Attribute)
      {
        if (techParm is ITechParamEntity techParamEntity)
        {
          Entity entityByCode = this.GetEntityByCode(techParamEntity.Code);
          if (entityByCode != null && !(entityByCode.Settings.ObjectType == Guid.Empty) && entityByCode.PumpToAttrType != null)
          {
            int objectTypeId = entityByCode.Settings.ObjectTypeID;
            if (objectTypeId == -1)
              this.plugin.appManager.AddWarningMessage($"тип объекта {entityByCode.Settings.ObjectType} не найден в метаданных");
            (int, string) key = (objectTypeId, entityByCode.Settings.JoinGroupId);
            List<ITechParamBase> techParamBaseList;
            if (!dictionary.TryGetValue(key, out techParamBaseList))
            {
              techParamBaseList = new List<ITechParamBase>();
              dictionary.Add(key, techParamBaseList);
            }
            techParamBaseList?.Add(techParm);
          }
        }
      }
    }
    if (!this.IsCloneRecord((TechObjectRecordBase) record) && dictionary.Count == 0)
    {
      foreach (TechObjectRelationInfo relListPair in this.relListPairs)
      {
        if (relListPair.Record.baseKey == record.baseKey)
        {
          (int, string) key = (relListPair.ObjEntRec.ObjectType, relListPair.JoinGroupId);
          if (!dictionary.ContainsKey(key))
            dictionary.Add(key, new List<ITechParamBase>());
        }
      }
    }
    HashSet<int> intSet = new HashSet<int>();
    foreach (KeyValuePair<(int, string), List<ITechParamBase>> keyValuePair in dictionary)
    {
      List<ITechParamBase> techParamBaseList = keyValuePair.Value;
      while (techParamBaseList.Count != 0)
      {
        intSet.Clear();
        ObjectRecord objectRecord = this._impObjList.AddObject(keyValuePair.Key.Item1, 0);
        if (objectRecord != null)
        {
          for (int index = techParamBaseList.Count - 1; index >= 0; --index)
          {
            bool flag = true;
            ITechParamBase techParamBase = techParamBaseList[index];
            try
            {
              switch (techParamBase.GetTechParamType())
              {
                case Intermech.ImpExp.TechCard.TechProcPump.Common.TechParamType.Attribute:
                  if (techParamBase is ITechParamAttribute techAttribute1)
                  {
                    if (!intSet.Contains(techAttribute1.AttributeType.ID))
                    {
                      this.AddAttribute2ImpObjectList(techAttribute1);
                      intSet.Add(techAttribute1.AttributeType.ID);
                      continue;
                    }
                    flag = false;
                    continue;
                  }
                  continue;
                case Intermech.ImpExp.TechCard.TechProcPump.Common.TechParamType.Entity:
                  if (techParamBase is ITechParamEntity techEntity)
                  {
                    Entity entityByCode = this.GetEntityByCode(techEntity.Code);
                    if (entityByCode != null)
                    {
                      if (entityByCode.Settings != null)
                      {
                        if (entityByCode.Settings.Properties != null)
                        {
                          ITechParamAttribute techAttribute2 = this._entityConverter.Convert((TechObjectRecordBase) record, this._techParmList, techEntity, entityByCode);
                          if (techAttribute2 != null)
                          {
                            if (!intSet.Contains(techAttribute2.AttributeType.ID))
                            {
                              this.AddAttribute2ImpObjectList(techAttribute2);
                              intSet.Add(techAttribute2.AttributeType.ID);
                              continue;
                            }
                            flag = false;
                            continue;
                          }
                          continue;
                        }
                        continue;
                      }
                      continue;
                    }
                    continue;
                  }
                  continue;
                default:
                  continue;
              }
            }
            finally
            {
              if (flag)
                techParamBaseList.RemoveAt(index);
            }
          }
          this.FillObjectRecord4Ent2Obj(record, techParmList, objectRecord);
          AttributesHelper.AddObligatoryObjectAttributes(this.plugin.Idw.GetUserSession(), this._impObjList);
          if (objectRec != null)
          {
            TechObjectRelationInfo objectRelationInfo = new TechObjectRelationInfo(objectRec, objectRecord, record, keyValuePair.Key.Item2);
            if (objectRec.Object_id != 0L)
              objectRelationInfo.ObjRecID = objectRec.Object_id;
            this.relListPairs.Add(objectRelationInfo);
          }
        }
        else
          break;
      }
    }
  }

  public virtual void FillLinkParams(
    TechObjectRecordBase recBase,
    TechRelParam relationParam,
    TechParamList parmList)
  {
    this.FillLinkParams(recBase, relationParam.RelRec, parmList, this.entTypeRec != null ? this.entTypeRec.CodeList : (Dictionary<string, Entity>) null);
  }

  public virtual void FillLinkParams(
    TechObjectRecordBase recBase,
    RelationRecord relRecord,
    TechParamList parmList,
    Dictionary<string, Entity> codeList)
  {
    if (relRecord == null || relRecord.RelationType == -1)
    {
      if (parmList.Count <= 0)
        return;
      this.plugin.appManager.AddInfoMessage($"для типа {this._recType} запись {recBase.Key} не найдена связь");
    }
    else
    {
      foreach (ITechParamBase parm in (List<ITechParamBase>) parmList)
      {
        switch (parm.GetTechParamType())
        {
          case Intermech.ImpExp.TechCard.TechProcPump.Common.TechParamType.Attribute:
            ITechParamAttribute techAttribute1 = parm as ITechParamAttribute;
            if (techAttribute1.AttributeBelongs == EntitySetting.AttributeBelongs.ToLink || techAttribute1.AttributeBelongs == EntitySetting.AttributeBelongs.ToLinkAndObject)
            {
              this.AddAttribute2ImpRelationList(techAttribute1);
              continue;
            }
            continue;
          case Intermech.ImpExp.TechCard.TechProcPump.Common.TechParamType.Entity:
            ITechParamEntity techEntity = parm as ITechParamEntity;
            Guid attrGuid;
            if (TechcardConsts.TechcardCommon.Code2AttributeGuid.TryGetValue(techEntity.Code, out attrGuid) && !(attrGuid == Guid.Empty))
            {
              if (!this.CheckObjLinkOrParamType(techEntity.Code, attrGuid))
              {
                this.FillLinkChangedOrParamType(recBase, relRecord, parm);
                continue;
              }
              Entity entityByCode = this.GetEntityByCode(techEntity.Code);
              if (entityByCode != null && entityByCode.Settings.AttributeBelong != EntitySetting.AttributeBelongs.ToObject && entityByCode.PumpToAttrTypeID != 0)
              {
                ITechParamAttribute techAttribute2 = this._entityConverter.Convert(recBase, this._techParmList, techEntity, entityByCode);
                if (techAttribute2 != null)
                {
                  this.AddAttribute2ImpRelationList(techAttribute2);
                  continue;
                }
                continue;
              }
              continue;
            }
            continue;
          default:
            continue;
        }
      }
    }
  }

  public virtual void FillLinkChangedOrParamType(
    TechObjectRecordBase recBase,
    RelationRecord rel,
    ITechParamBase techParm)
  {
  }

  public virtual void FillLinkSuspendedParams(
    TechObjectRecordBase recBase,
    long ipsObjectId,
    TechParamList parmList,
    TechRelParam relParam)
  {
    IImportingData importDataMain = this._import_data_main;
    if (importDataMain == null)
      return;
    long ipsObjectBid = relParam.IpsObjectBid;
    if (ipsObjectBid != ipsObjectId)
      return;
    RelationRecord relRec = relParam.RelRec;
    if (!(importDataMain.GetTag(ImportingCategory.TechSuspendedAtr, (object) ipsObjectBid) is TechObjectTag tag) || !(tag.Object is TechParamList parmList1))
      return;
    Dictionary<string, Entity> entitiesList = TechPumpData.Entities.EntitiesList;
    this.FillLinkParams(recBase, relRec, parmList1, entitiesList);
    tag.Object = (object) null;
  }

  protected virtual bool FillLinkSortParam(TechRelParam relParam, TechObjectRecordBase recBase)
  {
    if (relParam == null || relParam.RelRec == null || relParam.RelRec.RelationType == -1 || recBase == null)
      return false;
    int id = this._atSortAttr != null ? this._atSortAttr.ID : 0;
    if (id == 0)
      return false;
    long num = 0;
    IUserSession userSession = this.plugin.Imdi?.UserSession;
    if (userSession != null)
    {
      CompositionSortingRuleProc.TechChildRelationType childSortingRule = CompositionSortingRuleProc.GetChildSortingRule(relParam.IpsObjTypeB, relParam.RelType, userSession);
      if (childSortingRule != null && !childSortingRule.ChildObjCache.TryGetValue(relParam.IpsObjTypeA, out num))
      {
        int parentObjectType = childSortingRule.ChildRelType.GetNearestBaseParentObjectType(relParam.IpsObjTypeA);
        ChildObjectType childObjectType1 = childSortingRule.ChildRelType[parentObjectType];
        if (childObjectType1 != null)
        {
          num = childObjectType1.StartSortingValue;
        }
        else
        {
          long val1 = 0;
          foreach (ChildObjectType childObjectType2 in childSortingRule.ChildRelType.ChildObjectTypes)
          {
            if (childObjectType2 != null)
              val1 = Math.Max(val1, childObjectType2.StartSortingValue);
          }
          num = val1 + 1000000000L;
        }
        num += 20000000000L * (long) childSortingRule.ChildRelTypeIdx;
        childSortingRule.ChildObjCache.Add(relParam.IpsObjTypeA, num);
      }
    }
    long intValue = 0;
    object objValue;
    if (this._sortFieldName == string.Empty || !recBase.Fields.TryGetValue(this._sortFieldName, out objValue))
    {
      if (recBase.Fields.TryGetValue("F_KEY", out objValue))
      {
        if (!DataConvertor.IsEmptyValue(objValue) && !DataConvertor.ConvertObjToInt(objValue, out intValue))
        {
          this.plugin.appManager.AddNewWarningMessage($"Ошибка преобразования атрибута связи \"{objValue}\" в целочисленное значение");
          return false;
        }
        intValue %= 100000L;
      }
    }
    else if (!DataConvertor.IsEmptyValue(objValue) && !DataConvertor.ConvertObjToInt(objValue, out intValue))
    {
      this.plugin.appManager.AddNewWarningMessage($"Ошибка преобразования атрибута связи \"{objValue}\" в целочисленное значение");
      return false;
    }
    intValue = num + (intValue + (long) relParam.Sort + 1L) * 1000000L;
    this._impRelList.AddAttributeInt(id, intValue);
    return true;
  }

  protected virtual void FillLinkObligatoryAttributes()
  {
    AttributesHelper.AddObligatoryRelationAttributes(this.plugin.Idw, this._impRelList);
  }

  public virtual void SaveLinkSuspendedParams(long ipsObjectId, TechParamList parmList)
  {
    if (!this._isObjectTypeIsSuspended)
      return;
    IImportingData importDataMain = this._import_data_main;
    if (importDataMain == null || parmList.Count == 0)
      return;
    TechParamList techObject = new TechParamList();
    foreach (ITechParamBase parm in (List<ITechParamBase>) parmList)
    {
      switch (parm.GetTechParamType())
      {
        case Intermech.ImpExp.TechCard.TechProcPump.Common.TechParamType.Entity:
          Entity entity;
          if (parm is ITechParamEntity techParamEntity && this.entTypeRec.CodeList.TryGetValue(techParamEntity.Code, out entity) && entity.Settings.AttributeBelong != EntitySetting.AttributeBelongs.ToObject)
          {
            techObject.Add(parm);
            continue;
          }
          continue;
        default:
          continue;
      }
    }
    if (techObject.Count == 0)
      return;
    TechObjectTag tag = new TechObjectTag((object) techObject);
    importDataMain.AddValue(ImportingCategory.TechSuspendedAtr, (object) ipsObjectId, ipsObjectId, (ITagImportObject) tag);
  }

  public virtual DictionaryValue GetUserInfoBySearchId(int searchUserId)
  {
    switch (searchUserId)
    {
      case 0:
        searchUserId = -2;
        break;
    }
    DictionaryValue userInfoBySearchId;
    if (TechPumpBase.UserList.TryGetValue(searchUserId, out userInfoBySearchId))
      return userInfoBySearchId;
    try
    {
      if (this._import_data_main == null)
        return (DictionaryValue) null;
      userInfoBySearchId = this.plugin.Imdi.ImportedUsers.GetValue(searchUserId);
      TechPumpBase.UserList.Add(searchUserId, userInfoBySearchId);
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage(string.Format("Ошибка при получении данных записи пользователя (SEARCHID={1}): {0}", (object) ex.Message, (object) searchUserId));
      if (ex is OutOfMemoryException)
        throw;
    }
    return userInfoBySearchId;
  }

  public virtual (long ObjId, long ObjVerId, string Caption) GetArticleInfoByKey(int artTcKey)
  {
    (long, long, string) articleInfoByKey = (0L, 0L, string.Empty);
    if (artTcKey == 0)
      return articleInfoByKey;
    ArcArtsObject arcArtsObject;
    if (!TechPumpData.TechObjects.ArcArtList.TryGetValue((long) artTcKey, out arcArtsObject))
    {
      string Message = $"Изделие c TCKEY ={artTcKey} не найдено";
      TechcardConsts.Plugin.appManager.AddWarningMessage(Message);
      return articleInfoByKey;
    }
    (long ObjId, long ObjVerId, string Caption) articleInfoById = this.GetArticleInfoById(arcArtsObject.ArtId, arcArtsObject.ArtVer);
    if (articleInfoById.ObjId != 0L && string.IsNullOrEmpty(articleInfoById.Caption))
      articleInfoById.Caption = arcArtsObject.Caption;
    if (articleInfoById.ObjId == 0L && arcArtsObject.PortalVerGuid != Guid.Empty)
    {
      IDBObject dbObject = this.plugin.Idw.GetUserSession().GetObject(arcArtsObject.PortalVerGuid, false);
      if (dbObject != null)
      {
        articleInfoById.ObjId = Math.Abs(dbObject.ObjectID);
        articleInfoById.Caption = dbObject.Caption;
      }
    }
    return articleInfoById;
  }

  public virtual (long ObjId, long ObjVerId, string Caption) GetArticleInfoById(
    int artId,
    int artVer)
  {
    (long, long, string) articleInfoById = (0L, 0L, string.Empty);
    DictionaryValue dictionaryValue = this._import_data_main.GetValue(ImportingCategory.Articles, (object) artId);
    if (dictionaryValue == null)
      return articleInfoById;
    articleInfoById.Item3 = dictionaryValue.Caption;
    articleInfoById.Item1 = dictionaryValue.NewObjectID;
    if (!(dictionaryValue.Tag is ArticleTag tag) || tag.Versions == null)
      return articleInfoById;
    if (artVer == -1)
      artVer = tag.VersionID;
    long num;
    articleInfoById.Item2 = tag.Versions.TryGetValue(artVer, out num) ? num : articleInfoById.Item1;
    return articleInfoById;
  }

  public override void Exam()
  {
    this.entTypeRec = TechPumpData.EntTypeList.GetRecByType(this._recTypeID);
    this.ExamData();
    this.ExamCheckPoint($"Подготовка к закачке для элементов ТП: {this._recType} успешно завершена", 100);
  }

  public override void Pump()
  {
    this.plugin.appManager.AddInfoMessage("Загрузка метаданных ");
    this.LoadMetaData4Pump();
    this.plugin.appManager.AddInfoMessage("Загрузка кэшей импортированных данных ");
    this.LoadImportingCategoryData();
    if (this.objTypeID == -1)
    {
      this.plugin.appManager.AddInfoMessage($"Тип записи ТП {this._recType} пропущен");
    }
    else
    {
      if (TechCache.isResumeMode || this.IsMetadataPumper)
      {
        this.plugin.appManager.AddInfoMessage("Загрузка информации о предыдущем импорте ");
        this.LoadMetaData4StoppedPump();
        SavePoint savePoint = TechCache.SavePoint;
        if (savePoint != null && savePoint.PumpGuid == this.GUID && !savePoint.RePumpMode)
          this.AnalyzeStoppedData();
      }
      this.plugin.appManager.AddInfoMessage("Загрузка служебной информации о понятиях");
      this.LoadEntityMetaData();
      IDataWriter idw = this.plugin.Idw;
      this._impRelList = idw.CreateImportedRelationListWithStatistics(this.GUID);
      this._impObjList = idw.CreateImportedObjectListWithStatistics(this.GUID);
      this._impObjList.NewObjectsOnlyInList = false;
      this._impObjList.AfterImportEvent += new AfterImportEventDelegate(this.TechBase_AfterImportObjectEvent);
      this._impRelList.AfterImportEvent += new AfterImportEventDelegate(this.impRelList_AfterImportEvent);
      this.PumpCheckPoint("Закачка для элементов ТП: " + this._recType, 0);
      this.plugin.appManager.AddInfoMessage("Закачка для элементов ТП: " + this._recType);
      try
      {
        this.plugin.appManager.AddInfoMessage("Загрузка данных для ГТП / ТТП");
        this.PumpLoadTechDiffData();
        this.plugin.appManager.AddInfoMessage("Инициализация импорта данных");
        this.PumpLoadData();
        this._impObjList.Import();
        this._impRelList.Import();
        this.plugin.appManager.AddInfoMessage("Завершение импорта данных");
        this.DoAfterPump();
      }
      catch (Exception ex)
      {
        this.plugin.appManager.AddWarningMessage("Ошибка перекачки: " + ex.Message);
        this.plugin.appManager.AddWarningMessage("Call stack: " + ex.StackTrace);
        if (!(ex is OutOfMemoryException))
          return;
        throw;
      }
      finally
      {
        this.ReleasePumpData();
        this.PumpCheckPoint($"Закачка данных для элементов ТП: {this._recType} успешно завершена", 100);
        this.plugin.appManager.AddInfoMessage($"Закачка для элементов ТП: {this._recType} завершена");
      }
    }
  }

  public override string ToString() => "Тип записи: " + this._recType;
}
