// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.TP_PER.TechPerehodPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_OPER;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_PER;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.Interfaces;
using Intermech.Pools;
using Intermech.Text;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.TP_PER;

[TaskDescription("Инициализация данных для перекачки - Переходы", "Перекачка данных - Переходы")]
internal class TechPerehodPump(PluginClass plugin) : TechPumpBase(plugin)
{
  private readonly Guid _guid = new Guid("{1251F47B-225D-4DDD-8522-C542D345E4CA}");
  protected IAttributeTypeItem _atNonNumerationFlag;

  protected override void InitData()
  {
    this._sortFieldName = "F_ORDER";
    this._recType = "O";
    this._recTypeID = 14;
    this._tableName = "TP_PER";
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
      IAttributeTypeItem byGuid = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atNonNumerateFlagAttrGuid);
      if (byGuid != null)
        this._atNonNumerationFlag = byGuid;
      base.LoadMetaData4Pump();
    }
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechPerehPump;

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new ImportingCategory[1]
    {
      ImportingCategory.TechOperation
    };
  }

  protected override ImportingCategory[] GetCategoriesByNeed2FillTechObject()
  {
    return new ImportingCategory[2]
    {
      ImportingCategory.ImbaseFolders,
      ImportingCategory.TechCeh
    };
  }

  public override bool CheckObjTypeOrParamType(string entCode, Guid attrGuid)
  {
    return base.CheckObjTypeOrParamType(entCode, attrGuid);
  }

  public override bool CheckObjLinkOrParamType(string entCode, Guid attrGuid)
  {
    return base.CheckObjLinkOrParamType(entCode, attrGuid);
  }

  protected override void CheckBaseRecords()
  {
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (objRecord == null || record == null || record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record.RecMode != TechObjectRecord.PumpMode.ObjectOnly)
      return;
    int int32_1 = Convert.ToInt32(record.Fields["F_NUMBER"]);
    string empty = int32_1.ToString();
    int int32_2 = Convert.ToInt32(record.Fields["F_OPERKEY"]);
    DateTime dateTime = Convert.ToDateTime(record.Fields["F_DATE"]);
    int int32_3 = Convert.ToInt32(record.Fields["F_FLAGS"]);
    if (int32_3 == 0)
    {
      empty = string.Empty;
      this._techParmList.AddOrUpdateEntity("Nпер", (object) empty);
    }
    else if (int32_1 <= 0)
    {
      ITechParamEntity entity = this._techParmList.GetEntity("Nпер");
      if (entity != null)
      {
        if (empty == Convert.ToString(entity.Value))
        {
          this._techParmList.Remove((ITechParamBase) entity);
          entity = this._techParmList.GetEntity("Nпер");
        }
        if (entity != null)
          empty = Convert.ToString(entity.Value);
      }
    }
    if (this._atLastLevelSeek != null)
      this._techParmList.AddAttribute(this._atLastLevelSeek, (object) (long) this.GetFirstStepLifecycle());
    if (dateTime != DateTime.MinValue)
      objRecord.ObjCreate = dateTime.ToUniversalTime();
    if (int32_3 == 0 && this._atNonNumerationFlag != null)
      this._techParmList.AddAttribute(this._atNonNumerationFlag, (object) true);
    ITechParamEntity entity1 = this._techParmList.GetEntity("Тепр");
    string str = string.Empty;
    if (entity1 != null && this._entityConverter != null)
    {
      ITechParamAttribute techParamAttribute = this._entityConverter.Convert((TechObjectRecordBase) record, this._techParmList, entity1, this.GetEntityByCode(entity1.Code));
      str = techParamAttribute != null ? Convert.ToString(techParamAttribute.Value).Truncate(Consts.MaxStringSize - 2) : string.Empty;
    }
    using (ObjectPoolScope<StringBuilder> objectPoolScope = TextServices.StringBuilderPool.Allocate(4096 /*0x1000*/))
    {
      StringBuilder stringBuilder = objectPoolScope.Object;
      stringBuilder.Append(empty);
      stringBuilder.Append(". ");
      stringBuilder.Append(str);
      stringBuilder.Truncate(Consts.MaxStringSize - 2);
      objRecord.Caption = stringBuilder.ToString();
    }
    if (this._import_data_main.GetValue(ImportingCategory.TechOperation, (object) int32_2)?.Tag is TechRecordObjectTag tag && tag.Object is TechOperationCacheInfo operationCacheInfo)
    {
      if (operationCacheInfo.OwnerGuid != Guid.Empty)
        objRecord.OwnerGuid = (object) operationCacheInfo.OwnerGuid;
      if (operationCacheInfo.OwnerId != 0L)
        objRecord.OwnerId = operationCacheInfo.OwnerId;
    }
    base.FillTechObject(objRecord, record);
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

  protected override TechObjectRecord GetTpObjRec() => (TechObjectRecord) new TechPerehodObject();

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    long int32 = (long) Convert.ToInt32(recBase.Fields["F_OPERKEY"]);
    List<TechRelParam> techRelList = new List<TechRelParam>();
    int relTechRelationId = this._relTechRelationID;
    if (!this.IsCloneRecord(recBase))
    {
      long newKey = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechOperation, (object) int32);
      if (newKey != 0L)
      {
        TechRelParam techRelParam = new TechRelParam(newKey, ipsObjId, relTechRelationId, this._otOperTypeID, this.objTypeID);
        techRelList.Add(techRelParam);
      }
    }
    else
    {
      TechRelParam techRelParam = this.AddRelationByObject(ImportingCategory.TechOperation, (object) int32, relTechRelationId, recBase, ipsObjId, this._otOperTypeID, this.objTypeID);
      if (techRelParam != null)
        techRelList.Add(techRelParam);
    }
    return techRelList;
  }

  public override void Exam() => base.Exam();

  public override void Pump() => base.Pump();

  protected override Guid GUID => this._guid;
}
