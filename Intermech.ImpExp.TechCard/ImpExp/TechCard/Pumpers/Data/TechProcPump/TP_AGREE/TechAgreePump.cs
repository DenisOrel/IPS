// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_AGREE.TechAgreePump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.Interfaces;
using Intermech.Signs;
using Intermech.Signs.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_AGREE;

[TaskDescription("Инициализация данных для перекачки - Согласование объектов ТП", "Перекачка данных - Согласование объектов ТП")]
internal class TechAgreePump(PluginClass plugin) : TechPumpBase(plugin)
{
  private readonly Guid _guid = new Guid("{C089616B-B6A5-45BA-A5D3-1BA06569D51F}");
  protected int rtSignedTypeId;
  protected IAttributeTypeItem _atArchiveCopyType;
  protected IAttributeTypeItem _atSignVersionType;
  protected IAttributeTypeItem _atSignDateType;
  protected IAttributeTypeItem _atSignGraphType;
  protected IAttributeTypeItem _atModifyDataType;
  protected IAttributeTypeItem _atRankType;
  protected IAttributeTypeItem _atSignedUpByUserType;
  protected IAttributeTypeItem _atProtectionType;

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new TechObjectRecordDynamic("TP_AGREE");
  }

  protected override TechDataSource GetDataSource()
  {
    if (this._dataSource == null)
    {
      TechDataBuilderSimple<TechPumpBase> dataBuilder = new TechDataBuilderSimple<TechPumpBase>((TechPumpBase) this);
      dataBuilder.PumpModeCondFunc = (Func<string, string, string>) ((condField, dopType) => TechDataBuilder<PumpClass>.GetPumpModeCond("F_TPKEY", -2));
      this._dataSource = new TechDataSource((ITechDataBuilder) dataBuilder);
    }
    return this._dataSource;
  }

  protected override string GetRecordPumpMode(TechObjectRecord record)
  {
    if (this._import_data_main.GetValue(ImportingCategory.TechProcessPump, (object) Convert.ToInt32(record.Fields["F_TPKEY"])) != null)
      return base.GetRecordPumpMode(record);
    record.RecMode = TechObjectRecord.PumpMode.NotPump;
    return string.Empty;
  }

  protected override void CheckBaseRecords()
  {
  }

  protected ImportingCategory GetParentObjectCategory(TechObjectRecordBase record)
  {
    int int32 = Convert.ToInt32(record.Fields["F_OBJTYPE"]);
    switch (int32)
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
        this.plugin.appManager.AddWarningMessage("Неизвестный идентификатор типа: " + (object) int32);
        goto case 0;
    }
  }

  protected int GetParentObjectType(TechObjectRecordBase record)
  {
    return TechPumpData.TechType.TechTypeList.GetObjTypeId(Convert.ToInt32(record.Fields["F_OBJTYPE"]));
  }

  protected virtual long GetParentObjectId(TechObjectRecordBase recBase)
  {
    int int32_1 = Convert.ToInt32(recBase.Fields["F_OBJTYPE"]);
    int int32_2 = Convert.ToInt32(recBase.Fields["F_OBJID"]);
    int int32_3 = Convert.ToInt32(recBase.Fields["F_ARTKEY"]);
    ImportingCategory parentObjectCategory = this.GetParentObjectCategory(recBase);
    if (parentObjectCategory == ImportingCategory.None)
      return 0;
    DictionaryValue dictValue = this._import_data_main.GetValue(parentObjectCategory, (object) int32_2);
    if (dictValue == null)
    {
      this.plugin.appManager.AddWarningMessage($"Объект с F_OBJKEY='{int32_2}' F_OBJTYPE='{int32_1}' не найден в кэше закаченных объектов");
      return 0;
    }
    if (int32_3 == 0)
      return dictValue.NewObjectID;
    long parentObjectId;
    if (TechDiffTag.GetDiffTag(dictValue).CloneList.TryGetValue(int32_3, out parentObjectId))
      return parentObjectId;
    this.plugin.appManager.AddWarningMessage($"Объект с F_OBJKEY='{int32_2}' F_OBJTYPE='{int32_1}' F_ARTKEY='{int32_3}' не найден в кэше закаченных объектов");
    return 0;
  }

  protected virtual bool GetParentObjectModificationDate(
    long objectId,
    out DateTime modificationDate)
  {
    modificationDate = DateTime.UtcNow;
    DictionaryValue dictionaryValue = this._import_data_main.GetValue(ImportingCategory.TechObjectExtInfo, (object) objectId);
    if (dictionaryValue == null)
      return false;
    modificationDate = new DateTime(dictionaryValue.NewObjectID);
    return true;
  }

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    List<TechRelParam> techRelList = new List<TechRelParam>();
    long parentObjectId = this.GetParentObjectId(recBase);
    if (parentObjectId == 0L)
      return techRelList;
    RelationRecord relationRecord = this._impRelList.AddRelation(parentObjectId, ipsObjId, this.rtSignedTypeId);
    techRelList.Add(new TechRelParam(parentObjectId, ipsObjId, this.rtSignedTypeId, this.GetParentObjectType(recBase), this.objTypeID)
    {
      RelRec = relationRecord
    });
    return techRelList;
  }

  protected override ObjectRecord CreateTechObject(TechObjectRecord record)
  {
    return base.CreateTechObject(record);
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (objRecord == null || record == null || record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record.RecMode != TechObjectRecord.PumpMode.ObjectOnly)
      return;
    int int32_1 = Convert.ToInt32(record.Fields["F_USERID"]);
    int int32_2 = Convert.ToInt32(record.Fields["F_RANKID"]);
    int int32_3 = Convert.ToInt32(record.Fields["F_STATUS"]);
    DateTime dateOfSign = Convert.ToDateTime(record.Fields["F_DATE"]);
    object fldvalue = (object) dateOfSign;
    BasePumpHelper.FixDateTimeField(ref fldvalue);
    if (fldvalue != null)
      dateOfSign = (DateTime) fldvalue;
    DateTime modificationDate = DateTime.UtcNow;
    long parentObjectId = this.GetParentObjectId((TechObjectRecordBase) record);
    if (parentObjectId != 0L)
      this.GetParentObjectModificationDate(parentObjectId, out modificationDate);
    objRecord.ObjCreate = dateOfSign;
    this._techParmList.AddAttribute(this._atArchiveCopyType, (object) 1);
    this._techParmList.AddAttribute(this._atSignVersionType, (object) 1);
    this._techParmList.AddAttribute(this._atSignDateType, (object) dateOfSign);
    char rankCode = BasePumpHelper.GetRankCode(int32_2);
    DictionaryValue dictionaryValue = BasePumpHelper.RanksCache != null ? BasePumpHelper.RanksCache.GetValue((object) rankCode) : (DictionaryValue) null;
    if (dictionaryValue != null)
    {
      this._techParmList.AddAttribute(this._atSignGraphType, (object) dictionaryValue.Caption);
      this._techParmList.AddAttribute(this._atRankType, (object) dictionaryValue.NewObjectID, BasePumpHelper.GetNewRankCaption(dictionaryValue.NewObjectID));
    }
    if (int32_3 != 0)
      modificationDate = dateOfSign.AddDays(-1.0);
    this._techParmList.AddAttribute(this._atModifyDataType, (object) modificationDate);
    DictionaryValue userInfoBySearchId = this.GetUserInfoBySearchId(int32_1);
    if (userInfoBySearchId == null)
      return;
    this._techParmList.AddAttribute(this._atSignedUpByUserType, (object) userInfoBySearchId.NewObjectID, userInfoBySearchId.Caption);
    if (!(userInfoBySearchId.Tag is UserTag tag) || dictionaryValue == null)
      return;
    this._techParmList.AddAttribute(this._atProtectionType, (object) Convert.ToBase64String(HashPack.CalcHash(new HashPack(dictionaryValue.Caption, tag.Guid, modificationDate, dateOfSign, string.Empty).Pack())));
  }

  protected override Guid GUID => this._guid;

  protected override void InitData()
  {
    this._sortFieldName = "F_ORDER";
    this._recType = "Согласование объектов ТП";
    this._tableName = "TP_AGREE";
    if (!this.plugin.Imdi.ObjectTypes.ExistsByGuid(TechcardConsts.TypeConsts.otSignGuid))
      return;
    this.objTypeID = this.plugin.Imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otSignGuid).ID;
  }

  protected override void LoadMetaData4Pump()
  {
    IAttributeTypeItem byGuid1 = this.plugin.Imdi.AttributeTypes.GetByGuid(SignsHolder.InArchiveAttrTypeGuid);
    if (byGuid1 != null)
      this._atArchiveCopyType = byGuid1;
    IAttributeTypeItem byGuid2 = this.plugin.Imdi.AttributeTypes.GetByGuid(SignsHolder.SignVersionAttrTypeGuid);
    if (byGuid2 != null)
      this._atSignVersionType = byGuid2;
    IAttributeTypeItem byGuid3 = this.plugin.Imdi.AttributeTypes.GetByGuid(SignsHolder.DateOfSignatureGuid);
    if (byGuid3 != null)
      this._atSignDateType = byGuid3;
    IAttributeTypeItem byGuid4 = this.plugin.Imdi.AttributeTypes.GetByGuid(SignsHolder.GraphAttrTypeGuid);
    if (byGuid4 != null)
      this._atSignGraphType = byGuid4;
    IAttributeTypeItem byGuid5 = this.plugin.Imdi.AttributeTypes.GetByGuid(SignsHolder.ModifyDateAttrTypeGuid);
    if (byGuid5 != null)
      this._atModifyDataType = byGuid5;
    IAttributeTypeItem byGuid6 = this.plugin.Imdi.AttributeTypes.GetByGuid(SignsHolder.RankAttrTypeGuid);
    if (byGuid6 != null)
      this._atRankType = byGuid6;
    IAttributeTypeItem byGuid7 = this.plugin.Imdi.AttributeTypes.GetByGuid(SignsHolder.SignUpAttrTypeGuid);
    if (byGuid7 != null)
      this._atSignedUpByUserType = byGuid7;
    IAttributeTypeItem byGuid8 = this.plugin.Imdi.AttributeTypes.GetByGuid(SignsHolder.HashProtectionAttrTypeGuid);
    if (byGuid8 != null)
      this._atProtectionType = byGuid8;
    IRelationTypeItem byGuid9 = this.plugin.Imdi.RelationTypes.GetByGuid(TechcardConsts.TypeConsts.rtSignGuid);
    if (byGuid9 != null)
      this.rtSignedTypeId = byGuid9.ID;
    base.LoadMetaData4Pump();
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechAgree;

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new ImportingCategory[3]
    {
      ImportingCategory.TechProcessPump,
      ImportingCategory.TechOperation,
      ImportingCategory.TechPerehPump
    };
  }

  protected override ImportingCategory[] GetCategoriesByNeed2FillTechObject()
  {
    return new ImportingCategory[3]
    {
      ImportingCategory.TechProcessPump,
      ImportingCategory.Users,
      ImportingCategory.TechObjectExtInfo
    };
  }

  public override void Exam()
  {
    if (BasePumpHelper.RanksCache == null)
      TechcardConsts.Plugin.appManager.AddErrorMessage("Список должностей Search не найден. Проверьте, загружен ли модуль Intermech.ImpExp.SearchData");
    else
      base.Exam();
  }

  protected override void ReleasePumpData() => base.ReleasePumpData();
}
