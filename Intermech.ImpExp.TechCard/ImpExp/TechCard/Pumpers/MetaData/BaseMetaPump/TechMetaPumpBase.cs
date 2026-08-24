// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.BaseMetaPump.TechMetaPumpBase
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Expert;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.Interfaces;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.BaseMetaPump;

[TaskDescription("Инициализация метаданных для перекачки - базовый класс", "Перекачка метаданных - базовый класс")]
[TaskType(PumperType.MetaData)]
internal abstract class TechMetaPumpBase : TechPumpBase
{
  protected Dictionary<string, TechMetaPumpBase.DbMetaItem> _dbMetaCache = new Dictionary<string, TechMetaPumpBase.DbMetaItem>();

  protected virtual bool LoadDbMetaObjects()
  {
    if (this.objTypeID == -1)
      return false;
    IUserSession userSession = this.plugin.Idw.GetUserSession();
    if (userSession == null)
      return false;
    DataTable objectData = DataHelper.GetObjectData(this.objTypeID, userSession, (IEnumerable<ConditionStructure>) null, (IEnumerable<ColumnDescriptor>) this.GetDbMetaColumns().ToArray());
    if (objectData == null)
      return false;
    foreach (DataRow row in (InternalDataCollectionBase) objectData.Rows)
    {
      if (row != null)
        this.DbMetaObjectParse(row);
    }
    return true;
  }

  protected virtual List<ColumnDescriptor> GetDbMetaColumns()
  {
    List<ColumnDescriptor> dbMetaColumns = new List<ColumnDescriptor>(2);
    dbMetaColumns.Add(new ColumnDescriptor((object) -2, SortOrders.DESC, 2));
    int id1 = this.plugin.Imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atNaimAttrTypeGuid).ID;
    int id2 = this.plugin.Imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atObozAttrTypeGuid).ID;
    dbMetaColumns.Add(new ColumnDescriptor((object) id1, SortOrders.ASC, 0));
    dbMetaColumns.Add(new ColumnDescriptor((object) id2, SortOrders.ASC, 1));
    dbMetaColumns.Add(new ColumnDescriptor((object) -7, SortOrders.NONE, 0));
    return dbMetaColumns;
  }

  protected virtual void DbMetaObjectParse(DataRow dataRow)
  {
    if (dataRow == null)
      return;
    string str = dataRow[1] != DBNull.Value ? dataRow[1].ToString() : string.Empty;
    long int64 = dataRow[0] != DBNull.Value ? Convert.ToInt64(dataRow[0]) : 0L;
    int num = dataRow[3] != DBNull.Value ? Convert.ToInt32(dataRow[3]) : -1;
    if (str == string.Empty || int64 == 0L || num == -1)
      return;
    string key = $"({num})_{str}";
    TechMetaPumpBase.DbMetaItem dbMetaItem;
    if (this._dbMetaCache.TryGetValue(key, out dbMetaItem))
      dbMetaItem.ObjectId = int64;
    else
      this._dbMetaCache.Add(key, new TechMetaPumpBase.DbMetaItem(int64));
  }

  protected virtual string GetDBMetaRecordHash(TechObjectRecord record)
  {
    return record == null ? string.Empty : $"({this.objTypeID})_{Convert.ToString(record.Fields["F_NAME"])}";
  }

  protected TechMetaPumpBase(PluginClass plugin)
    : base(plugin)
  {
    this.taskExam.Repumpble = false;
    this.taskPump.Repumpble = false;
  }

  protected override string GetRecordPumpMode(TechObjectRecord record)
  {
    string recordPumpMode = base.GetRecordPumpMode(record);
    return record.RecMode == TechObjectRecord.PumpMode.NotPump || record.RecMode == TechObjectRecord.PumpMode.Unknown ? recordPumpMode : string.Empty;
  }

  public override long GetLastObjectForType(IUserSession session, int objectTypeId, long objectId)
  {
    long lastObjectForType = 0;
    if (session == null)
      return lastObjectForType;
    try
    {
      Guid techTypeKeyAttrGuid = TechcardConsts.TypeConsts.atTechTypeKeyAttrGuid;
      IDBAttributeType attributeType = session.GetAttributeType(techTypeKeyAttrGuid, false);
      if (attributeType == null || session.GetObjectType(this.objTypeID, false) == null)
        return lastObjectForType;
      IDBObjectCollection objectCollection = session.GetObjectCollection(objectTypeId);
      ColumnDescriptor[] columns = new ColumnDescriptor[1]
      {
        new ColumnDescriptor((object) techTypeKeyAttrGuid, AttributeSourceTypes.Auto, ColumnContents.Text, ColumnNameMapping.Guid, SortOrders.DESC, 1)
      };
      DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[1]
      {
        new ConditionStructure(attributeType.AttributeID, RelationalOperators.GreaterOrEqual, (object) objectId, LogicalOperators.NONE, 0, false)
      }, columns, recordCount: 1);
      DataTable dataTable = objectCollection.Select(paramSet);
      if (dataTable != null && dataTable.Rows.Count > 0)
      {
        lastObjectForType = DataSetProcessor.GetInt64Value(dataTable.Rows[0], 0, 0L);
        int recKey;
        TechcardConsts.Utils.DecodeHashCode(lastObjectForType, out int _, out recKey);
        lastObjectForType = (long) recKey;
      }
      if (!Intermech.Consts.IsUndefinedObjectId(lastObjectForType))
        this.plugin.appManager.AddInfoMessage($"В базе IPS последний импортированный объект для записи Key = {lastObjectForType}");
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

  protected override string GetRecordWithParamsPumpMode(TechObjectRecord record)
  {
    string empty = string.Empty;
    string dbMetaRecordHash = this.GetDBMetaRecordHash(record);
    TechMetaPumpBase.DbMetaItem dbMetaItem;
    if (dbMetaRecordHash == string.Empty || !this._dbMetaCache.TryGetValue(dbMetaRecordHash, out dbMetaItem))
      return empty;
    this.plugin.appManager.AddInfoMessage($"Запись F_KEY = {record.Key} исключена из импорта. В базе IPS найден соответствующий объект ( ObjectId = {dbMetaItem.ObjectId} ).");
    if (!dbMetaItem.Proceeded)
    {
      this.AddValue2Cache((object) record.Key, dbMetaItem.ObjectId, (TechObjectRecordBase) record, this._techParmList);
      dbMetaItem.Proceeded = true;
    }
    record.RecMode = TechObjectRecord.PumpMode.NotPump;
    return empty;
  }

  protected override void PumpLoadData()
  {
    this.LoadDbMetaObjects();
    base.PumpLoadData();
  }

  protected override void ReleasePumpData()
  {
    base.ReleasePumpData();
    this._dbMetaCache.Clear();
    this._dbMetaCache = (Dictionary<string, TechMetaPumpBase.DbMetaItem>) null;
  }

  internal class DbMetaItem
  {
    protected long _objectID;
    protected bool _proceeded;

    public DbMetaItem(long objectId) => this._objectID = objectId;

    public long ObjectId
    {
      [DebuggerStepThrough] get => this._objectID;
      set => this._objectID = value;
    }

    public bool Proceeded
    {
      [DebuggerStepThrough] get => this._proceeded;
      [DebuggerStepThrough] set => this._proceeded = value;
    }
  }
}
