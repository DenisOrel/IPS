// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.TP_OPER.TechOperPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Data.Obj2Link;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_OPER;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.ImpExp.TechCard.TechProcPump.Common.TechProcessPump;
using Intermech.Interfaces;
using Intermech.Interfaces.TechCard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.TP_OPER;

[TaskDescription("Инициализация данных для перекачки - Операции", "Перекачка данных - Операции")]
internal class TechOperPump(PluginClass plugin) : TechPumpBase(plugin)
{
  private readonly HashSet<int> _operWithOperLinkCache = new HashSet<int>();
  private readonly Guid _guid = new Guid("{6751F47B-FF5D-4353-85D2-C5422380E4C9}");
  protected int _otCehCahodTypeID = -1;
  protected IAttributeTypeItem _atProductionAttrType;

  private void LoadOperWithOperLinkCache()
  {
    try
    {
      using (IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand())
      {
        string str = string.Empty;
        if (this._lastObjID != 0L)
          str = $"AND {"F_SOURCE_KEY"} > {this._lastObjID}";
        string pumpModeCond = TechDataBuilder<PumpClass>.GetPumpModeCond("F_DOC_TCKEY", -2);
        if (pumpModeCond != string.Empty)
          str = $"{str} AND {pumpModeCond}";
        command.CommandText = $"SELECT DISTINCT\r\n                                                        {"F_SOURCE_KEY"} \r\n                                                      FROM \r\n                                                        {"TP_LINKS"}\r\n                                                      WHERE   \r\n                                                        {"F_SOURCE_TYPE"} = 1 AND \r\n                                                        {"F_TARGET_TYPE"} = 1        \r\n                                                        {str} ";
        using (IDataReader dataReader = command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
        {
          int ordinal = dataReader.GetOrdinal("F_SOURCE_KEY");
          while (dataReader.Read())
          {
            int int32 = dataReader.IsDBNull(ordinal) ? 0 : BasePumpHelper.ToInt32(dataReader[ordinal]);
            if (int32 != 0)
              this._operWithOperLinkCache.Add(int32);
          }
          dataReader.Close();
        }
      }
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage(string.Format("Невозможно прочитать информацию о связях операций с операциями сквозного ТП(таблица {1}): {0}", (object) ex.Message, (object) "TP_LINKS"));
      if (!(ex is OutOfMemoryException))
        return;
      throw;
    }
  }

  protected override void InitData()
  {
    this._sortFieldName = "F_ORDER";
    this._recType = "A";
    this._recTypeID = 1;
    this._tableName = "TP_OPER";
    this._dopTypes.Add("S");
    this._dopTypes.Add("I");
    this._dopTypes.Add("F");
    this._dopTypes.Add("D");
  }

  protected override void LoadMetaData4Pump()
  {
    IMetadataInfo imdi = this.plugin.Imdi;
    if (imdi == null)
    {
      this.plugin.appManager.AddErrorMessage("Ошибка получения кэша метаданных");
    }
    else
    {
      IObjectTypeItem byGuid1 = imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otTechCehZahodObjTypeGuid);
      if (byGuid1 != null)
        this._otCehCahodTypeID = byGuid1.ID;
      IAttributeTypeItem byGuid2 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atProductionAttrTypeGuid);
      if (byGuid2 != null)
        this._atProductionAttrType = byGuid2;
      base.LoadMetaData4Pump();
    }
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechOperation;

  protected override ImportingCategory GetTechObjectExCategory()
  {
    return ImportingCategory.TechObjectExtInfo;
  }

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new ImportingCategory[3]
    {
      ImportingCategory.TechCehZahodPump,
      ImportingCategory.TechParentParametors,
      ImportingCategory.TechProcessPump
    };
  }

  protected override ImportingCategory[] GetCategoriesByNeed2FillTechObject()
  {
    return new ImportingCategory[4]
    {
      ImportingCategory.TechProcessPump,
      ImportingCategory.ImbaseFolders,
      ImportingCategory.TechCeh,
      ImportingCategory.Users
    };
  }

  protected override void CheckBaseRecords()
  {
  }

  public override bool CheckObjTypeOrParamType(string entCode, Guid attrGuid)
  {
    return base.CheckObjTypeOrParamType(entCode, attrGuid);
  }

  public override bool CheckObjLinkOrParamType(string entCode, Guid attrGuid)
  {
    return base.CheckObjLinkOrParamType(entCode, attrGuid);
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (objRecord == null || record == null || record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record.RecMode != TechObjectRecord.PumpMode.ObjectOnly)
      return;
    int int32 = Convert.ToInt32(record.Fields["F_DOCTCKEY"]);
    int key = Convert.ToInt32(record.Fields["F_PRODUCTION"]);
    string str = Convert.ToString(record.Fields["F_NUMBER"]);
    int searchUserId = Convert.ToInt32(record.Fields["F_USER"]);
    DateTime dateTime = Convert.ToDateTime(record.Fields["F_DATE"]);
    string empty = string.Empty;
    if (this._techParmList != null)
    {
      object entityValue = this._techParmList.GetEntityValue("ОПЕР");
      if (entityValue != null)
        empty = entityValue.ToString();
    }
    TechProcCacheInfo techProcCacheInfo = (TechProcCacheInfo) null;
    if (this._import_data_main.GetValue(ImportingCategory.TechProcessPump, (object) int32)?.Tag is TechRecordObjectTag tag1)
      techProcCacheInfo = tag1.Object as TechProcCacheInfo;
    objRecord.Caption = $"{str}  {empty}".Truncate(Intermech.Consts.MaxStringSize - 2);
    if (this._atLastLevelSeek != null)
      this._techParmList.AddAttribute(this._atLastLevelSeek, (object) (long) this.GetFirstStepLifecycle());
    if (searchUserId == 0 && techProcCacheInfo != null)
      searchUserId = techProcCacheInfo.UserCode;
    DictionaryValue userInfoBySearchId = this.GetUserInfoBySearchId(searchUserId);
    if (userInfoBySearchId != null)
    {
      objRecord.OwnerId = userInfoBySearchId.NewObjectID;
      if (userInfoBySearchId.Tag is UserTag tag2)
        objRecord.OwnerGuid = (object) tag2.Guid;
    }
    if (dateTime != DateTime.MinValue)
      objRecord.ObjCreate = dateTime.ToUniversalTime();
    if (this._atProductionAttrType != null)
    {
      if (key == 0 && techProcCacheInfo != null)
        key = techProcCacheInfo.ProductionCode;
      IpsProductionObj ipsProductionObj;
      if (key != 0 && TechPumpData.Production.Productions.TryGetValue(key, out ipsProductionObj))
      {
        if (ipsProductionObj == null)
          this.plugin.appManager.AddWarningMessage($"Вид производства № {(object) key} не найден");
        else
          this._techParmList.AddAttribute(this._atProductionAttrType, (object) ipsProductionObj.ObjID, ipsProductionObj.ProdInfo.Name);
      }
    }
    base.FillTechObject(objRecord, record);
  }

  protected override TechRecordObjectTag GetTagValue4Cache(
    object oldKey,
    long newKey,
    TechObjectRecordBase recBase)
  {
    int searchUserId = Convert.ToInt32(recBase.Fields["F_USER"]);
    if (searchUserId == 0)
    {
      int int32 = Convert.ToInt32(recBase.Fields["F_DOCTCKEY"]);
      TechProcCacheInfo techProcCacheInfo = (TechProcCacheInfo) null;
      if (this._import_data_main.GetValue(ImportingCategory.TechProcessPump, (object) int32)?.Tag is TechRecordObjectTag tag)
        techProcCacheInfo = tag.Object as TechProcCacheInfo;
      if (techProcCacheInfo != null)
        searchUserId = techProcCacheInfo.UserCode;
    }
    DictionaryValue userInfoBySearchId = this.GetUserInfoBySearchId(searchUserId);
    if (userInfoBySearchId == null)
      return (TechRecordObjectTag) null;
    TechOperationCacheInfo techObject = new TechOperationCacheInfo();
    techObject.OwnerId = userInfoBySearchId.NewObjectID;
    if (userInfoBySearchId.Tag is UserTag tag1)
      techObject.OwnerGuid = tag1.Guid;
    return new TechRecordObjectTag((object) techObject);
  }

  protected override TechDataSource GetDataSource()
  {
    if (this._dataSource == null)
    {
      TechDataBuilderSimple<TechPumpBase> dataBuilder = new TechDataBuilderSimple<TechPumpBase>((TechPumpBase) this);
      dataBuilder.PumpModeCondFunc = (Func<string, string, string>) ((condField, dopType) => TechDataBuilder<PumpClass>.GetPumpModeCond(dopType == "" ? "F_DOCTCKEY" : "F_TCKEY", -2));
      this._dataSource = new TechDataSource((ITechDataBuilder) dataBuilder);
    }
    return this._dataSource;
  }

  protected override TechObjectRecord GetTpObjRec() => (TechObjectRecord) new TechOperationObject();

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    int num1 = recBase.baseKey;
    List<TechRelParam> techRelList = new List<TechRelParam>();
    long newKey = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechCehZahodPump, (object) num1);
    int relTechRelationId = this._relTechRelationID;
    if (this.IsCloneRecord(recBase))
    {
      if (newKey < 0L)
        num1 = Convert.ToInt32(-newKey);
      TechRelParam techRelParam = this.AddRelationByObject(ImportingCategory.TechCehZahodPump, (object) num1, relTechRelationId, recBase, ipsObjId, this._otCehCahodTypeID, this.objTypeID);
      if (techRelParam != null)
        techRelList.Add(techRelParam);
    }
    else
    {
      if (newKey < 0L)
        newKey = this._import_data_main.GetNewKey(ImportingCategory.TechCehZahodPump, (object) -newKey);
      if (newKey != 0L)
      {
        TechRelParam techRelParam = new TechRelParam(newKey, ipsObjId, relTechRelationId, this._otCehCahodTypeID, this.objTypeID);
        techRelList.Add(techRelParam);
      }
    }
    if (!this.IsCloneRecord(recBase))
    {
      int int32 = Convert.ToInt32(recBase.Fields["F_TPKEY"]);
      if (int32 != 0 && !this._operWithOperLinkCache.Contains(recBase.Key))
      {
        int throughtTpRelationId = TechCardConsts.RelTypes.TechThroughtTPRelationID;
        DictionaryValue dictionaryValue = this._import_data_main.GetValue(ImportingCategory.TechProcessPump, (object) int32);
        int result = -1;
        long ipsObjectA = 0;
        if (dictionaryValue != null && dictionaryValue.Tag is TechRecordObjectTag)
        {
          ipsObjectA = dictionaryValue.NewObjectID;
          TechRecordObjectTag tag = (TechRecordObjectTag) dictionaryValue.Tag;
          object obj = tag.Object;
          if (obj is TechProcCacheInfo techProcCacheInfo)
            result = techProcCacheInfo.ObjTypeId;
          else
            int.TryParse(obj.ToString(), out result);
          TechDiffTag techDiffTag = tag.TechDiffTag;
          if (techDiffTag != null && !techDiffTag.IsCloneListEmpty)
          {
            ipsObjectA = 0L;
            List<Obj2LinkInfoObject> source;
            if (TechPumpData.TechObjects.Tp2LinkList.TryGetValue((long) Convert.ToInt32(recBase.Fields["F_DOCTCKEY"]), out source))
            {
              int artTcKey = source.First<Obj2LinkInfoObject>().ArtTcKey;
              long num2;
              if (techDiffTag.CloneList.TryGetValue(artTcKey, out num2))
                ipsObjectA = num2;
            }
          }
        }
        if (ipsObjectA != 0L)
        {
          TechRelParam techRelParam = new TechRelParam(ipsObjId, ipsObjectA, throughtTpRelationId, this.objTypeID, result);
          techRelList.Add(techRelParam);
        }
      }
    }
    return techRelList;
  }

  protected override void PumpLoadData()
  {
    this.LoadOperWithOperLinkCache();
    base.PumpLoadData();
  }

  public override void Exam() => base.Exam();

  public override void Pump() => base.Pump();

  protected override void ClearTmpData() => base.ClearTmpData();

  protected override Guid GUID => this._guid;
}
