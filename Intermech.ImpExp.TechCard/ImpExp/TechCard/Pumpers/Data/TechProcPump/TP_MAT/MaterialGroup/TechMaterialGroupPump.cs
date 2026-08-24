// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_MAT.MaterialGroup.TechMaterialGroupPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_MAT.MaterialGroupSubstitute;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.ImpExp.TechCard.TechProcPump.TP_MAT;
using Intermech.Interfaces;
using Intermech.Interfaces.TechCard;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_MAT.MaterialGroup;

[TaskDescription("Инициализация данных для перекачки - Группы материалов", "Перекачка данных - Группы материалов")]
internal class TechMaterialGroupPump(PluginClass plugin) : TechPumpBase(plugin)
{
  private IAttributeTypeItem _atSubstitutesGroupNo;
  private IAttributeTypeItem _atSubstituteInGroup;
  private IAttributeTypeItem _atSubstituteGroupName;
  private IAttributeTypeItem _atSubstituteName;
  private int _objOperationsTypeIpsId = -1;
  private int _objMaterialsTypeIpsId = -1;
  private int _objTpTypeIpsId = -1;
  private int _objPerTypeIpsId = -1;
  private int _objDopPrTypeIpsId = -1;
  private int _objGrMatTypeIpsId = -1;
  private readonly Guid _guid = new Guid("{C758FB88-CFB0-488f-9F51-8A45EDD0DA9A}");

  protected override void InitData()
  {
    this._recType = "1";
    this._recTypeID = 24;
    this._tableName = "TP_MAT_GR";
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
      this._objOperationsTypeIpsId = TechPumpData.TechType.TechTypeList.GetObjTypeId(TechcardConsts.TpRecordType.Oper);
      this._objMaterialsTypeIpsId = TechPumpData.TechType.TechTypeList.GetObjTypeId(TechcardConsts.TpRecordType.MaterialAdd);
      this._objTpTypeIpsId = TechPumpData.TechType.TechTypeList.GetObjTypeId(TechcardConsts.TpRecordType.Passport);
      this._objPerTypeIpsId = TechPumpData.TechType.TechTypeList.GetObjTypeId(TechcardConsts.TpRecordType.Perehod);
      this._objDopPrTypeIpsId = TechPumpData.TechType.TechTypeList.GetObjTypeId(TechcardConsts.TpRecordType.DopPriem);
      this._objGrMatTypeIpsId = TechPumpData.TechType.TechTypeList.GetObjTypeId(TechcardConsts.TpRecordType.MaterialGroup);
      IAttributeTypeItem byGuid1 = imdi.AttributeTypes.GetByGuid(new Guid("cad001c0-306c-11d8-b4e9-00304f19f545"));
      if (byGuid1 != null)
        this._atSubstitutesGroupNo = byGuid1;
      IAttributeTypeItem byGuid2 = imdi.AttributeTypes.GetByGuid(new Guid("cad001c1-306c-11d8-b4e9-00304f19f545"));
      if (byGuid2 != null)
        this._atSubstituteInGroup = byGuid2;
      IAttributeTypeItem byGuid3 = imdi.AttributeTypes.GetByGuid(new Guid("cad00817-306c-11d8-b4e9-00304f19f545"));
      if (byGuid3 != null)
        this._atSubstituteGroupName = byGuid3;
      IAttributeTypeItem byGuid4 = imdi.AttributeTypes.GetByGuid(new Guid("cad00818-306c-11d8-b4e9-00304f19f545"));
      if (byGuid4 != null)
        this._atSubstituteName = byGuid4;
      base.LoadMetaData4Pump();
    }
  }

  protected override Guid GUID => this._guid;

  protected override ImportingCategory[] GetCategoriesByNeed2FillTechObject()
  {
    return new ImportingCategory[3]
    {
      ImportingCategory.TechMatGroupParent,
      ImportingCategory.Users,
      ImportingCategory.BaseTechObjectsVersionsCache
    };
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechMatGrPump;

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return this.GetMaterialLinkTypes();
  }

  private ImportingCategory[] GetMaterialLinkTypes()
  {
    return new ImportingCategory[8]
    {
      ImportingCategory.TechManufacturingRouting,
      ImportingCategory.TechMatPump,
      ImportingCategory.TechOperation,
      ImportingCategory.TechPerehPump,
      ImportingCategory.TechProcessPump,
      ImportingCategory.TechMaterialPostLinks,
      ImportingCategory.TechMaterialGroupSubstituteCache,
      ImportingCategory.TechMaterialGroupReplaceableCache
    };
  }

  private ImportingCategory GetParentCacheCategory(int parentType)
  {
    switch (parentType)
    {
      case 0:
        return ImportingCategory.None;
      case 1:
        return ImportingCategory.TechOperation;
      case 5:
        return ImportingCategory.TechAddMovement;
      case 12:
        return ImportingCategory.TechMatPump;
      case 14:
        return ImportingCategory.TechPerehPump;
      case 15:
        return ImportingCategory.TechProcessPump;
      case 17:
        return ImportingCategory.None;
      case 24:
        return ImportingCategory.TechMatGrPump;
      default:
        this.plugin.appManager.AddWarningMessage("Неизвестный идентификатор типа: " + parentType.ToString());
        goto case 0;
    }
  }

  protected override string GetRecordPumpMode(TechObjectRecord record)
  {
    string recordPumpMode = base.GetRecordPumpMode(record);
    if (record.RecMode == TechObjectRecord.PumpMode.NotPump || record.RecMode == TechObjectRecord.PumpMode.Unknown || this._import_data_main.GetValue(ImportingCategory.TechMaterialGroupSubstituteCache, (object) TechMaterialGroupSubstituteCacheInfo.GetObjectCacheCode(record.Key, this._recTypeID)) == null)
      return recordPumpMode;
    record.RecMode = TechObjectRecord.PumpMode.NotPump;
    return recordPumpMode;
  }

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new TechMaterialGroupObject();
  }

  protected override void CheckBaseRecords()
  {
  }

  protected override TechDataSource GetDataSource()
  {
    if (this._dataSource == null)
    {
      TechMaterialGroupDataBuilder<TechPumpBase> dataBuilder = new TechMaterialGroupDataBuilder<TechPumpBase>((TechPumpBase) this);
      dataBuilder.PumpModeCondFunc = (Func<string, string, string>) ((condField, dopType) =>
      {
        string pumpModeCond1 = TechDataBuilder<PumpClass>.GetPumpModeCond(dopType == "" ? "A.F_DOCTCKEY" : "F_TCKEY", -2);
        string pumpModeCond2 = TechDataBuilder<PumpClass>.GetPumpModeCond(dopType == "" ? "A.F_SETKEY" : "F_SETKEY", 4);
        if (pumpModeCond1 == string.Empty)
          return pumpModeCond2;
        return !(pumpModeCond2 != string.Empty) ? pumpModeCond1 : $"( {pumpModeCond1} OR {pumpModeCond2})";
      });
      this._dataSource = new TechDataSource((ITechDataBuilder) dataBuilder);
    }
    return base.GetDataSource();
  }

  protected override int GetObjectType(TechObjectRecordBase record)
  {
    object objValue;
    if (!(record is TechMaterialGroupObject materialGroupObject) || !materialGroupObject.Fields.TryGetValue("F_GROUP_TYPE", out objValue))
      return this.objTypeID;
    int intValue;
    DataConvertor.ConvertObjToInt(objValue, out intValue);
    switch (intValue)
    {
      case 1:
      case 4:
        return this._objGrMatTypeIpsId;
      case 2:
        return TechCardConsts.ObjectTypes.MaterialGroupID;
      case 3:
        return TechCardConsts.ObjectTypes.MaterialSetID;
      default:
        return this.objTypeID;
    }
  }

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    int key = recBase.Key;
    int int32_1 = Convert.ToInt32(recBase.Fields["F_PARENTKEY_1"]);
    int int32_2 = Convert.ToInt32(recBase.Fields["F_PARENTTYPE_1"]);
    int relTechRelationId = this._relTechRelationID;
    List<TechRelParam> techRelList = new List<TechRelParam>();
    if (int32_1 == 0)
      return techRelList;
    ImportingCategory parentCacheCategory = this.GetParentCacheCategory(int32_2);
    int ipsObjTypeB = -1;
    switch (parentCacheCategory)
    {
      case ImportingCategory.TechOperation:
        ipsObjTypeB = this._objOperationsTypeIpsId;
        break;
      case ImportingCategory.TechProcessPump:
        ipsObjTypeB = this._objTpTypeIpsId;
        break;
      case ImportingCategory.TechPerehPump:
        ipsObjTypeB = this._objPerTypeIpsId;
        break;
      case ImportingCategory.TechMatPump:
        ipsObjTypeB = this._objMaterialsTypeIpsId;
        break;
      case ImportingCategory.TechMatGrPump:
        ipsObjTypeB = this._objGrMatTypeIpsId;
        break;
      case ImportingCategory.TechAddMovement:
        ipsObjTypeB = this._objDopPrTypeIpsId;
        break;
    }
    if (ipsObjTypeB != 0)
    {
      long newKey = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, parentCacheCategory, (object) int32_1.ToString(), false);
      if (newKey == 0L)
      {
        if (!this.IsCloneRecord(recBase))
          this._import_data_main.AddValue(ImportingCategory.TechMaterialPostLinks, (object) TechMaterialLinksPump.GenerateMatLinkKey(this._recTypeID, key, int32_2, int32_1), 1L);
      }
      else if (this.IsCloneRecord(recBase))
      {
        TechRelParam techRelParam = this.AddRelationByObject(parentCacheCategory, (object) int32_1, relTechRelationId, recBase, ipsObjId, ipsObjTypeB, this.objTypeID);
        if (techRelParam != null)
          techRelList.Add(techRelParam);
      }
      else
      {
        TechRelParam techRelParam = new TechRelParam(newKey, ipsObjId, relTechRelationId, ipsObjTypeB, this.objTypeID);
        techRelList.Add(techRelParam);
      }
    }
    return techRelList;
  }

  protected override void PumpLoadSubData(TechObjectRecordBase recBase, string dopType)
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
        TechObjectRecordSub_D2 objectRecordSubD2 = new TechObjectRecordSub_D2();
        IDataReader dataReader = dataReaderInfo.DataReader;
        Dictionary<string, int> tableColumns = this.GetTableColumns(dataReader);
        objectRecordSubD2.ParseSchema((IDictionary<string, int>) tableColumns);
        int index = 0;
        while (dataReader.Read())
        {
          TechObjectRecordSub_D objectRecordSubD = (TechObjectRecordSub_D) new TechObjectRecordSub_D2();
          this.PumpLoadSubDataRec(dopType, dataReader, (TechObjectRecordBase) objectRecordSubD);
          this._techDataRecCache.AddTechDataRec(dopType, (TechObjectRecordSub) objectRecordSubD);
          ++index;
          if (index % this.CheckCount == 0 || index == dataReaderInfo.RecordCount)
            this.ExamCheckPoint($"Считывание типов записей ТП ({index} из {dataReaderInfo.RecordCount})", this.CalculatePercent(dataReaderInfo.RecordCount, index, 1, 10));
          if (objectRecordSubD.ParentKey != recBase.Key)
            break;
        }
      }
      finally
      {
        this.ExamCheckPoint($"Загрузка типа данных: {dopType} для элементов ТП: {this._recType} успешно завершена", 100);
      }
    }
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (objRecord == null || record == null || record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record.RecMode != TechObjectRecord.PumpMode.ObjectOnly)
      return;
    DateTime dateTime = Convert.ToDateTime(record.Fields["F_DATE"]);
    string str = Convert.ToString(record.Fields["F_NAME"]);
    int int32_1 = Convert.ToInt32(record.Fields["F_USERID"]);
    int result;
    int.TryParse(Convert.ToString(this._techParmList.GetEntityValue("%GV")), out result);
    int int32_2 = Convert.ToInt32(record.Fields["F_PARENTKEY"]);
    int int32_3 = Convert.ToInt32(record.Fields["F_OWNER"]);
    int int32_4 = Convert.ToInt32(record.Fields["F_STATUS"]);
    objRecord.Caption = str.Truncate(Intermech.Consts.MaxStringSize - 2);
    if (this._import_data_main.GetValue(ImportingCategory.TechMatGroupParent, (object) record.Key) == null)
      this._import_data_main.AddValue(ImportingCategory.TechMatGroupParent, (object) record.Key, (long) result, objRecord.IdGuid.ToString());
    if (int32_2 != 0 && int32_2 != record.Key)
    {
      DictionaryValue dictionaryValue1 = this._import_data_main.GetValue(ImportingCategory.TechMatGroupParent, (object) int32_2);
      if (dictionaryValue1 != null && !string.IsNullOrEmpty(dictionaryValue1.Caption))
      {
        Guid guid = GuidHelper.IsGuid(dictionaryValue1.Caption) ? new Guid(dictionaryValue1.Caption) : Guid.Empty;
        if (!guid.Equals(Guid.Empty))
        {
          objRecord.Id = 0L;
          objRecord.IdGuid = (object) guid;
          DictionaryValue dictionaryValue2 = this._import_data_main.GetValue(ImportingCategory.TechMatGroupParent, (object) int32_3);
          objRecord.ParentVersionNo = dictionaryValue2 != null ? (int) dictionaryValue2.NewObjectID : result - 1;
          objRecord.ParentVersionId = this._import_data_main.GetNewKey(this.GetTechCategory(), (object) int32_3);
        }
      }
    }
    objRecord.VersionId = result;
    objRecord.ObjectVerType = int32_4 >= 0 ? 0 : 1;
    DictionaryValue userInfoBySearchId = this.GetUserInfoBySearchId(int32_1);
    if (userInfoBySearchId != null)
    {
      objRecord.OwnerId = userInfoBySearchId.NewObjectID;
      if (userInfoBySearchId.Tag is UserTag tag)
        objRecord.OwnerGuid = (object) tag.Guid;
    }
    if (dateTime != DateTime.MinValue)
      objRecord.ObjCreate = dateTime.ToUniversalTime();
    base.FillTechObject(objRecord, record);
    if (!objRecord.IsBaseVersion)
      return;
    long oldKey = TechPumpBase.GenBaseTechObjectsVersionsCacheKey(record.Key, LinkedObjectType.MatGroup);
    if (this._import_data_main.GetValue(ImportingCategory.BaseTechObjectsVersionsCache, (object) oldKey) != null)
      return;
    this._import_data_main.AddValue(ImportingCategory.BaseTechObjectsVersionsCache, (object) oldKey, (long) result);
  }

  public override void FillLinkParams(
    TechObjectRecordBase recBase,
    TechRelParam relRecord,
    TechParamList paramList)
  {
    if (this._import_data_main.GetValue(ImportingCategory.TechMaterialGroupReplaceableCache, (object) TechMaterialGroupSubstituteCacheInfo.GetObjectCacheCode(recBase.Key, this._recTypeID)) != null)
    {
      paramList.AddAttribute(this._atSubstitutesGroupNo, (object) recBase.Key, (string) null, EntitySetting.AttributeBelongs.ToLink);
      paramList.AddAttribute(this._atSubstituteInGroup, (object) 0, (string) null, EntitySetting.AttributeBelongs.ToLink);
      paramList.AddAttribute(this._atSubstituteGroupName, (object) recBase.Key, (string) null, EntitySetting.AttributeBelongs.ToLink);
      paramList.AddAttribute(this._atSubstituteName, (object) $"{recBase.Key}.0", (string) null, EntitySetting.AttributeBelongs.ToLink);
    }
    base.FillLinkParams(recBase, relRecord, paramList);
  }

  public override void Exam() => base.Exam();

  public override void Pump() => base.Pump();
}
