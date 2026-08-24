// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.TechBaseUniquePump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectData;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.ImpExp.TechCard.TechProcPump.Common.TechProcessPump;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.TechCard;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;

internal abstract class TechBaseUniquePump(PluginClass plugin) : TechPumpBase(plugin)
{
  private int _lastRecKey;
  private int _lastTblKey;
  private string _fldRecKey = "F_RECKEY";
  private string _imbaseCodeEntity;
  private readonly IDictionary<string, BaseTechObjInfo> _existsObjName2ObjectCache = (IDictionary<string, BaseTechObjInfo>) new Dictionary<string, BaseTechObjInfo>();
  private readonly ISet<long> _existsObjIdCache = (ISet<long>) new HashSet<long>();
  protected string _fldTblKey = "F_TBLKEY";

  private void DoLoadImbaseObjectsFromIps(IUserSession session)
  {
    if (this.objTypeID == -1)
      return;
    IDBObjectCollection objectCollection = session.GetObjectCollection(this.objTypeID);
    IEnumerable<int> toCompareObjects = this.GetIpsAttributesToCompareObjects();
    if (toCompareObjects == null)
      return;
    ColumnDescriptor[] first = new ColumnDescriptor[4]
    {
      new ColumnDescriptor((object) -2, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.FieldName, SortOrders.NONE, 0),
      new ColumnDescriptor((object) -3, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.FieldName, SortOrders.NONE, 0),
      new ColumnDescriptor((object) -12, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.FieldName, SortOrders.NONE, 0),
      new ColumnDescriptor((object) -18, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.FieldName, SortOrders.NONE, 0)
    };
    int length = first.Length;
    ColumnDescriptor[] array = ((IEnumerable<ColumnDescriptor>) first).Concat<ColumnDescriptor>(toCompareObjects.Select<int, ColumnDescriptor>((System.Func<int, ColumnDescriptor>) (attrId => new ColumnDescriptor((object) attrId, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Guid, SortOrders.NONE, 0)))).ToArray<ColumnDescriptor>();
    if (array.Length <= length)
      return;
    DBRecordSetParams paramSet = new DBRecordSetParams((ConditionStructure[]) null, array);
    DataTable dataTable = objectCollection.Select(paramSet);
    if (dataTable == null)
      return;
    foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
    {
      int int32Value = DataSetProcessor.GetInt32Value(row[0], 0);
      if (int32Value != 0)
      {
        string objectCompareIndex = this.GetIpsObjectCompareIndex(((IEnumerable<object>) row.ItemArray).Skip<object>(length).Select<object, string>((Func<object, int, string>) ((fieldValue, idx) => DataSetProcessor.GetStringValue(fieldValue, string.Empty))));
        if (!string.IsNullOrEmpty(objectCompareIndex))
        {
          BaseTechObjInfo baseTechObjInfo = new BaseTechObjInfo()
          {
            IpsObjVerId = (long) int32Value,
            CompareIndex = objectCompareIndex
          };
          this._existsObjIdCache.Add((long) int32Value);
          this._existsObjName2ObjectCache[objectCompareIndex] = baseTechObjInfo;
          if (this.plugin.Imdi.ImportedObjects.GetInfo((long) int32Value) == null)
          {
            string str1 = Convert.ToString(row[2]);
            string str2 = Convert.ToString(row[3]);
            this.plugin.Imdi.ImportedObjects.AddValue((long) int32Value, Convert.ToInt64(row[1]), Convert.ToInt32(this.objTypeID), GuidHelper.IsGuid(str1) ? new Guid(str1) : Guid.Empty, GuidHelper.IsGuid(str2) ? new Guid(str2) : Guid.Empty);
          }
        }
      }
    }
  }

  protected override void LoadMetaData4Pump()
  {
    if (this.plugin.Imdi == null)
    {
      this.plugin.appManager.AddErrorMessage("Ошибка получения кэша метаданных");
    }
    else
    {
      foreach (Entity entity in TechPumpData.Entities.EntitiesList.Values)
      {
        if (entity.RecordID == this.RecTypeID && entity.EntityReference != null && entity.EntityReference.Field == -2)
        {
          this._imbaseCodeEntity = entity.Code;
          break;
        }
      }
      base.LoadMetaData4Pump();
    }
  }

  protected override string GetRecordPumpMode(TechObjectRecord record)
  {
    int int32_1 = Convert.ToInt32(record.Fields[this._fldRecKey]);
    int int32_2 = Convert.ToInt32(record.Fields[this._fldTblKey]);
    if (int32_1 < this._lastRecKey || int32_1 == this._lastRecKey && int32_2 < this._lastTblKey)
    {
      record.RecMode = TechObjectRecord.PumpMode.NotPump;
      return string.Empty;
    }
    long lastObjId = this._lastObjID;
    try
    {
      if (int32_1 != this._lastRecKey || int32_2 != this._lastTblKey)
        this._lastObjID = 0L;
      return base.GetRecordPumpMode(record);
    }
    finally
    {
      this._lastObjID = lastObjId;
    }
  }

  protected abstract string GetRecordRecKey(TechObjectRecordBase record);

  protected override string GetUniqueRecordHash(TechObjectRecordBase record)
  {
    return $"{this.GetRecordRecKey(record)}#{Convert.ToInt32(record.Fields[this._fldRecKey])}#{Convert.ToInt32(record.Fields[this._fldTblKey])}#";
  }

  protected override void PumpDiffRec(TechObjectRecord record)
  {
    switch (record.RecMode)
    {
      case TechObjectRecord.PumpMode.ObjectAndLinks:
      case TechObjectRecord.PumpMode.LinkOnly:
        base.PumpDiffRec(record);
        break;
    }
  }

  protected override void PumpBaseRec(TechObjectRecord record)
  {
    long lastObjId = this._lastObjID;
    try
    {
      if (this._lastObjID != 0L)
      {
        int int32_1 = Convert.ToInt32(record.Fields[this._fldRecKey]);
        int int32_2 = Convert.ToInt32(record.Fields[this._fldTblKey]);
        if (this._lastRecKey != int32_1 || this._lastTblKey != int32_2)
          this._lastObjID = 0L;
      }
      base.PumpBaseRec(record);
      if (this.IsCloneRecord((TechObjectRecordBase) record))
        return;
      switch (record.RecMode)
      {
        case TechObjectRecord.PumpMode.ObjectAndLinks:
          record.RecMode = TechObjectRecord.PumpMode.LinkOnly;
          break;
        case TechObjectRecord.PumpMode.ObjectOnly:
          record.RecMode = TechObjectRecord.PumpMode.NotPump;
          break;
      }
    }
    finally
    {
      this._lastObjID = lastObjId;
    }
  }

  protected override void AfterPumpCloneRec(TechObjectRecord record, TechObjectRecord recordClone)
  {
    base.AfterPumpCloneRec(record, recordClone);
    switch (record.RecMode)
    {
      case TechObjectRecord.PumpMode.ObjectAndLinks:
        record.RecMode = TechObjectRecord.PumpMode.LinkOnly;
        break;
      case TechObjectRecord.PumpMode.ObjectOnly:
        record.RecMode = TechObjectRecord.PumpMode.NotPump;
        break;
    }
  }

  protected override void FillObjectObligatoryAttributes(TechObjectRecord record)
  {
    if (record.RecMode == TechObjectRecord.PumpMode.LinkOnly)
      return;
    base.FillObjectObligatoryAttributes(record);
  }

  public override void FillRecordParamsFixed(TechObjectRecord record, TechParamList paramList)
  {
    base.FillRecordParamsFixed(record, paramList);
    string str = !string.IsNullOrEmpty(this._imbaseCodeEntity) ? Convert.ToString(paramList.GetEntityValue(this._imbaseCodeEntity)) : string.Empty;
    if (string.IsNullOrEmpty(str) || ImbaseImportHelper.ImbaseKeyHandler(this._impObjList, MetaDataHelper.GetAttributeID((object) TechCardConsts.AttributeTypes.ImbaseObjectAttrGuid), MetaDataHelper.GetAttributeID((object) TechCardConsts.AttributeTypes.ImbaseCodeAttrGuid), Convert.ToString(str), this._import_data_main))
      return;
    string message = $"Код Imbase (Code = \"{str}\") не найден в кэше Imbase";
    TechcardConsts.Plugin.appManager.AddNewWarningMessage(message);
  }

  protected override ObjectRecord CreateTechObject(TechObjectRecord record)
  {
    if (record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks)
      return base.CreateTechObject(record);
    string objectCompareIndex = this.GetTechcardObjectCompareIndex();
    if (objectCompareIndex == null)
      return base.CreateTechObject(record);
    long newKey = 0;
    BaseTechObjInfo objectInfoFromIps = this.GetObjectInfoFromIps(objectCompareIndex);
    if (objectInfoFromIps != null && this.plugin.Imdi.ImportedObjects.GetInfo(objectInfoFromIps.IpsObjVerId) != null)
      newKey = objectInfoFromIps.IpsObjVerId;
    if (TechCardPlugin.Configuration.UniqueObjectsLookupIpsPumpMode)
    {
      if (newKey != 0L && !this._existsObjIdCache.Contains(newKey))
        newKey = 0L;
      if (newKey == 0L && this._existsObjName2ObjectCache.TryGetValue(objectCompareIndex, out objectInfoFromIps))
        newKey = objectInfoFromIps.IpsObjVerId;
    }
    if (newKey == 0L)
      return base.CreateTechObject(record);
    record.RecMode = TechObjectRecord.PumpMode.LinkOnly;
    string uniqueRecordHash = this.GetUniqueRecordHash((TechObjectRecordBase) record);
    if (!string.IsNullOrEmpty(uniqueRecordHash))
      this._import_data_main.AddValue(this.GetTechUniqueCategory(), (object) uniqueRecordHash, newKey, string.Empty);
    return base.CreateTechObject(record);
  }

  protected virtual BaseTechObjInfo GetObjectInfoFromIps(string objName) => (BaseTechObjInfo) null;

  protected virtual IEnumerable<int> GetIpsAttributesToCompareObjects()
  {
    return (IEnumerable<int>) new int[1]
    {
      MetaDataHelper.GetAttributeID((object) "cad00047-306c-11d8-b4e9-00304f19f545")
    };
  }

  protected virtual string GetIpsObjectCompareIndex(IEnumerable<string> ipsObjAttrValues)
  {
    string sourceImbaseObjName = ipsObjAttrValues.FirstOrDefault<string>();
    return sourceImbaseObjName != null ? ImbaseObjectNameParser.ParseCompositeObjName(sourceImbaseObjName).ObjectName : string.Empty;
  }

  protected abstract string GetTechcardObjectCompareIndex();

  protected override ImportingCategory[] GetCategoriesByNeed2FillTechObject()
  {
    return new ImportingCategory[3]
    {
      ImportingCategory.ImbaseFolders,
      ImportingCategory.TechCeh,
      ImportingCategory.ImbaseTableLinksKeyToObjectID
    };
  }

  protected override TechDataSource GetDataSource()
  {
    if (this._dataSource == null)
      this._dataSource = new TechDataSource((ITechDataBuilder) new TechBaseUniqueDataBuilder<TechBaseUniquePump>(this));
    return this._dataSource;
  }

  protected override void PumpLoadSubDataRec(
    string dopType,
    IDataReader reader,
    TechObjectRecordBase record)
  {
    if (reader == null || record == null)
      return;
    base.PumpLoadSubDataRec(dopType, reader, record);
    if (record.ExFields == null)
      record.ExFields = (IDictionary<string, object>) new Dictionary<string, object>();
    int ordinal1 = reader.GetOrdinal(this._fldRecKey);
    record.ExFields.Add(this._fldRecKey, (object) (ordinal1 != -1 ? BasePumpHelper.ToInt32(reader[ordinal1]) : 0));
    int ordinal2 = reader.GetOrdinal(this._fldTblKey);
    record.ExFields.Add(this._fldTblKey, (object) (ordinal2 != -1 ? BasePumpHelper.ToInt32(reader[ordinal2]) : 0));
  }

  protected override void PumpLoadData()
  {
    if (TechCardPlugin.Configuration.UniqueObjectsLookupIpsPumpMode)
      this.DoLoadImbaseObjectsFromIps(this.Plugin.Imdi.UserSession);
    base.PumpLoadData();
  }

  protected override bool PumpLoadSubData_Loaded(
    string dopType,
    TechObjectRecordBase recBase,
    TechObjectRecordSub dopRecord)
  {
    int int32_1 = Convert.ToInt32(recBase.Fields[this._fldRecKey]);
    int int32_2 = Convert.ToInt32(dopRecord.ExFields[this._fldRecKey]);
    if (int32_2 > int32_1)
      return true;
    if (int32_2 != int32_1)
      return false;
    int int32_3 = Convert.ToInt32(recBase.Fields[this._fldTblKey]);
    int int32_4 = Convert.ToInt32(dopRecord.ExFields[this._fldTblKey]);
    if (int32_4 > int32_3)
      return true;
    return int32_4 == int32_3 && dopRecord.ParentKey > recBase.Key;
  }

  protected override void AnalyzeStoppedData()
  {
    base.AnalyzeStoppedData();
    if (this._lastObjID == 0L)
      return;
    long lastObjId = this._lastObjID;
    if (!(ServicesManager.GetService(typeof (ICache)) is ICache service))
      return;
    IImportingData cache = service.GetCache(this.GetTechUniqueCategory());
    if (cache == null)
      return;
    try
    {
      IDictionary<object, DictionaryValue> category = (IDictionary<object, DictionaryValue>) cache.GetCategory(this.GetTechUniqueCategory());
      string empty = string.Empty;
      foreach (KeyValuePair<object, DictionaryValue> keyValuePair in (IEnumerable<KeyValuePair<object, DictionaryValue>>) category)
      {
        if (keyValuePair.Value.NewObjectID == lastObjId)
        {
          empty = keyValuePair.Key.ToString();
          break;
        }
      }
      if (empty == string.Empty)
      {
        long num = 0;
        foreach (KeyValuePair<object, DictionaryValue> keyValuePair in (IEnumerable<KeyValuePair<object, DictionaryValue>>) category)
        {
          if (keyValuePair.Value.NewObjectID > num)
          {
            num = keyValuePair.Value.NewObjectID;
            empty = keyValuePair.Key.ToString();
          }
        }
      }
      if (empty == string.Empty)
        return;
      string[] strArray = empty.Split('#');
      int result1;
      int result2;
      if (strArray.Length <= 2 || !int.TryParse(strArray[1], out result1) || !int.TryParse(strArray[1], out result2))
        return;
      this._lastRecKey = result1;
      this._lastTblKey = result2;
    }
    finally
    {
      service.ReleaseCache(this.GetTechUniqueCategory());
    }
  }

  protected override void AddValue2Cache(
    object oldKey,
    long newKey,
    TechObjectRecordBase recBase,
    TechParamList recParmList)
  {
    base.AddValue2Cache(oldKey, newKey, recBase, recParmList);
  }

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    List<TechRelParam> relParmList = new List<TechRelParam>();
    if (recBase == null || recBase is TechObjectRecord techObjectRecord && techObjectRecord.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && techObjectRecord.RecMode != TechObjectRecord.PumpMode.LinkOnly)
      return relParmList;
    foreach (TechObjectRecordBase objectRecordBase in new List<TechObjectRecordBase>()
    {
      recBase
    })
    {
      int int32_1 = Convert.ToInt32(objectRecordBase.Fields["F_DOCTCKEY"]);
      int int32_2 = Convert.ToInt32(objectRecordBase.Fields["F_OPERKEY"]);
      int int32_3 = objectRecordBase.FieldExist("F_PEREHKEY") ? Convert.ToInt32(objectRecordBase.Fields["F_PEREHKEY"]) : 0;
      if (this.CreateTechCustomRelations(objectRecordBase, ipsObjId, relParmList))
      {
        int result = -1;
        int oldKey;
        long ipsObjectB;
        ImportingCategory category;
        if (int32_3 != 0 && int32_2 != 0)
        {
          oldKey = int32_3;
          ipsObjectB = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechPerehPump, (object) int32_3);
          result = this._otPerehTypeID;
          category = ImportingCategory.TechPerehPump;
        }
        else if (int32_2 != 0)
        {
          oldKey = int32_2;
          ipsObjectB = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechOperation, (object) int32_2);
          result = this._otOperTypeID;
          category = ImportingCategory.TechOperation;
        }
        else
        {
          DictionaryValue dictionaryValue = ImportingDataHelper.Instance.GetValue(this._import_data_main, ImportingCategory.TechProcessPump, (object) int32_1);
          oldKey = int32_1;
          ipsObjectB = dictionaryValue != null ? dictionaryValue.NewObjectID : 0L;
          category = ImportingCategory.TechProcessPump;
          if (dictionaryValue != null && dictionaryValue.Tag is TechRecordObjectTag)
          {
            object obj = ((TechRecordObjectTag) dictionaryValue.Tag).Object;
            if (obj is TechProcCacheInfo techProcCacheInfo)
              result = techProcCacheInfo.ObjTypeId;
            else
              int.TryParse(obj.ToString(), out result);
          }
        }
        if (ipsObjectB != 0L)
        {
          if (!this.IsCloneRecord(objectRecordBase))
          {
            if (ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechOperation, (object) int32_2) != 0L)
            {
              TechRelParam techRelParam = new TechRelParam(ipsObjectB, ipsObjId, this._relTechRelationID, result, this.objTypeID);
              relParmList.Add(techRelParam);
            }
          }
          else
          {
            TechRelParam techRelParam = this.AddRelationByObject(category, (object) oldKey, this._relTechRelationID, objectRecordBase, ipsObjId, result, this.objTypeID);
            if (techRelParam != null)
              relParmList.Add(techRelParam);
          }
        }
      }
    }
    return relParmList;
  }

  protected virtual bool CreateTechCustomRelations(
    TechObjectRecordBase recBase,
    long ipsObjId,
    List<TechRelParam> relParmList)
  {
    return true;
  }

  internal int LastRecKey => this._lastRecKey;

  internal string FldRecKey => this._fldRecKey;

  internal string FldTblKey => this._fldTblKey;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal string NormalizeEntityValue(object entityValue)
  {
    return Convert.ToString(entityValue).ToUpperInvariant().Replace(" ", "");
  }
}
