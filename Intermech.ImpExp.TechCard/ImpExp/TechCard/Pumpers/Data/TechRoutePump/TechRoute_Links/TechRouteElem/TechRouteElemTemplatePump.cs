// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRoute_Links.TechRouteElem.TechRouteElemTemplatePump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.ImpExp.TechCard.TechRoutePump.Common;
using Intermech.Interfaces;
using Intermech.Interfaces.TechCard;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRoute_Links.TechRouteElem;

[TaskType(PumperType.MetaData)]
[TaskDescription("Инициализация данных для перекачки - шаблоны элементов расцеховки", "Перекачка данных - шаблоны элементов расцеховки")]
internal class TechRouteElemTemplatePump(PluginClass plugin) : TechRouteCommonPump(plugin)
{
  private readonly Guid _guid = new Guid("{9D49B096-EE29-4261-B883-D168A7E2635E}");
  private IAttributeTypeItem _atWorkTypeAttr;
  private IAttributeTypeItem _atCehAttr;
  private IAttributeTypeItem _atAreaAttr;
  private IAttributeTypeItem _atTechCehCodeAttr;
  private IAttributeTypeItem _atTechWorkAreaCodeAttr;
  private IAttributeTypeItem _atCaptionAttr;

  protected override void InitData()
  {
    base.InitData();
    this._sortFieldName = "F_ORDER";
    this._recType = "X";
    this._recTypeID = 22;
    this._tableName = "TC_NROUTE_STRINGS";
    this.objTypeID = (this.plugin.Imdi.ObjectTypes.GetByGuid(TechCardConsts.ObjectTypes.ElemRouteTemplateGuid) ?? throw new Exception("Тип объекта \"Шаблон расцеховочного элемента\" не найден")).ID;
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
      IAttributeTypeItem byGuid1 = imdi.AttributeTypes.GetByGuid(TechCardConsts.AttributeTypes.WorkTypeAttrGuid);
      if (byGuid1 != null)
        this._atWorkTypeAttr = byGuid1;
      IAttributeTypeItem byGuid2 = imdi.AttributeTypes.GetByGuid(TechCardConsts.AttributeTypes.CehRouteAttrGUID);
      if (byGuid2 != null)
        this._atCehAttr = byGuid2;
      IAttributeTypeItem byGuid3 = imdi.AttributeTypes.GetByGuid(TechCardConsts.AttributeTypes.AreaRouteAttrGUID);
      if (byGuid3 != null)
        this._atAreaAttr = byGuid3;
      IAttributeTypeItem byGuid4 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atTechCehCodeAttrGuid);
      if (byGuid4 != null)
        this._atTechCehCodeAttr = byGuid4;
      IAttributeTypeItem byGuid5 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atTechWorkAreaCodeAttrGuid);
      if (byGuid5 != null)
        this._atTechWorkAreaCodeAttr = byGuid5;
      IAttributeTypeItem byGuid6 = imdi.AttributeTypes.GetByGuid(TechCardConsts.AttributeTypes.ElemRouteCaptionAttrGuid);
      if (byGuid6 != null)
        this._atCaptionAttr = byGuid6;
      base.LoadMetaData4Pump();
    }
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechRouteElemTemplate;

  protected override ImportingCategory GetTechUniqueCategory()
  {
    return ImportingCategory.TechRouteElemTemplateUniqueCache;
  }

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new ImportingCategory[2]
    {
      ImportingCategory.TechRouteTemplate,
      ImportingCategory.TechRouteElemTemplateUniqueCache
    };
  }

  protected override ImportingCategory[] GetCategoriesByNeed2FillTechObject()
  {
    return new ImportingCategory[4]
    {
      ImportingCategory.TechCeh,
      ImportingCategory.TechArea,
      ImportingCategory.TechWorkTypes,
      ImportingCategory.TechRouteElemTemplateUniqueCache
    };
  }

  protected override void CheckBaseRecords()
  {
  }

  protected override void AddValue2Cache(
    object oldKey,
    long newKey,
    TechObjectRecordBase recBase,
    TechParamList recParmList)
  {
    base.AddValue2Cache(oldKey, newKey, recBase, recParmList);
  }

  protected override string GetUniqueRecordHash(TechObjectRecordBase recBase)
  {
    return $"{Convert.ToInt32(recBase.Fields["F_VID_ID"])}_{Convert.ToInt32(recBase.Fields["F_CEH_ID"])}";
  }

  protected override TechDataSource GetDataSource()
  {
    if (this._dataSource == null)
    {
      TechDataBuilderSimple<TechPumpBase> dataBuilder = new TechDataBuilderSimple<TechPumpBase>((TechPumpBase) this);
      dataBuilder.PumpModeCondFunc = (Func<string, string, string>) ((condField, dopType) => string.Empty);
      this._dataSource = new TechDataSource((ITechDataBuilder) dataBuilder);
    }
    return this._dataSource;
  }

  protected override TechObjectRecord GetTpObjRec() => (TechObjectRecord) new TechRouteElemObject();

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    return new List<TechRelParam>(0);
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (objRecord == null || record == null || record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record.RecMode != TechObjectRecord.PumpMode.ObjectOnly)
      return;
    int int32_1 = Convert.ToInt32(record.Fields["F_CEH_ID"]);
    int int32_2 = Convert.ToInt32(record.Fields["F_VID_ID"]);
    string caption1 = string.Empty;
    string caption2 = string.Empty;
    string caption3 = string.Empty;
    if (this._atWorkTypeAttr != null)
    {
      DictionaryValue dictionaryValue = this._import_data_main.GetValue(ImportingCategory.TechWorkTypes, (object) int32_2);
      if (dictionaryValue != null && dictionaryValue.NewObjectID != -1L)
      {
        caption3 = dictionaryValue.Caption;
        this._techParmList.AddAttribute(this._atWorkTypeAttr, (object) dictionaryValue.NewObjectID, caption3);
      }
    }
    if (this._atCehAttr != null)
    {
      DictionaryValue dictionaryValue = this._import_data_main.GetValue(ImportingCategory.TechCeh, (object) int32_1);
      if (dictionaryValue != null && dictionaryValue.NewObjectID != -1L)
      {
        caption1 = dictionaryValue.Caption;
        this._techParmList.AddAttribute(this._atCehAttr, (object) dictionaryValue.NewObjectID, caption1);
        ITagImportObject tag;
        if (this._atTechCehCodeAttr != null && (tag = dictionaryValue.Tag) is TechObjectTag)
        {
          string str = Convert.ToString(((TechObjectTag) tag).Object);
          if (!string.IsNullOrEmpty(str))
            this._techParmList.AddAttribute(this._atTechCehCodeAttr, (object) str);
        }
      }
    }
    if (this._atLastLevelSeek != null)
      this._techParmList.AddAttribute(this._atLastLevelSeek, (object) (long) this.GetFirstStepLifecycle());
    if (this._atAreaAttr != null)
    {
      DictionaryValue dictionaryValue = this._import_data_main.GetValue(ImportingCategory.TechArea, (object) int32_1);
      if (dictionaryValue != null && dictionaryValue.NewObjectID != -1L)
      {
        caption2 = dictionaryValue.Caption;
        this._techParmList.AddAttribute(this._atAreaAttr, (object) dictionaryValue.NewObjectID, caption2);
        ITagImportObject tag;
        if (this._atTechWorkAreaCodeAttr != null && (tag = dictionaryValue.Tag) is TechObjectTag)
        {
          string str = Convert.ToString(((TechObjectTag) tag).Object);
          if (!string.IsNullOrEmpty(str))
            this._techParmList.AddAttribute(this._atTechWorkAreaCodeAttr, (object) str);
        }
      }
    }
    objRecord.Caption = $"{caption1} {caption2} - {caption3}".Truncate(Intermech.Consts.MaxStringSize - 2);
    if (this._atCaptionAttr != null)
      this._techParmList.AddAttribute(this._atCaptionAttr, (object) $"{caption1} {caption2} - {caption3}");
    if (this._atNaimAttrType != null)
      this._techParmList.AddAttribute(this._atNaimAttrType, (object) $"{caption1} {caption2} - {caption3}");
    if (this._atObozAttrType != null)
      this._techParmList.AddAttribute(this._atObozAttrType, (object) $"{caption1} {caption2} - {caption3}");
    base.FillTechObject(objRecord, record);
  }

  public override void Exam() => base.Exam();

  public override void Pump()
  {
    base.LoadMetaData4Pump();
    base.Pump();
  }

  protected override Guid GUID => this._guid;
}
