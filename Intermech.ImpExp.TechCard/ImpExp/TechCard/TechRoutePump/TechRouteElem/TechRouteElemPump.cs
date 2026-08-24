// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechRoutePump.TechRouteElem.TechRouteElemPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Common.TechCardSettings;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRoute_Links.TechRouteElem;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.ImpExp.TechCard.TechRoutePump.Common;
using Intermech.Interfaces;
using Intermech.Interfaces.TechCard;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechRoutePump.TechRouteElem;

[TaskType(PumperType.Standard)]
[TaskDescription("Инициализация данных для перекачки - элементы расцеховки", "Перекачка данных - элементы расцеховки")]
internal class TechRouteElemPump(PluginClass plugin) : TechRouteCommonPump(plugin)
{
  private readonly Guid _guid = new Guid("{AEE65910-63CF-416e-8D76-A955FBF96FB6}");
  private int _otRouteTemplateTypeId;
  private int _otRouteTypeId;
  private IAttributeTypeItem _atWorkTypeAttr;
  private IAttributeTypeItem _atCehAttr;
  private IAttributeTypeItem _atAreaAttr;
  private IAttributeTypeItem _atCaptionAttr;
  private IAttributeTypeItem _attObjectPrototype;
  private readonly Dictionary<int, int> _template2RouteCache = new Dictionary<int, int>();

  protected override void InitData()
  {
    base.InitData();
    this._sortFieldName = "F_ORDER";
    this._recType = "X";
    this._recTypeID = 22;
    this._tableName = "TC_NROUTE_STRINGS";
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
      IObjectTypeItem byGuid1 = imdi.ObjectTypes.GetByGuid(TechCardConsts.ObjectTypes.TemplRouteBaseGUID);
      if (byGuid1 != null)
        this._otRouteTemplateTypeId = byGuid1.ID;
      IObjectTypeItem byGuid2 = imdi.ObjectTypes.GetByGuid(TechCardConsts.ObjectTypes.CehRouteGUID);
      if (byGuid2 != null)
        this._otRouteTypeId = byGuid2.ID;
      IAttributeTypeItem byGuid3 = imdi.AttributeTypes.GetByGuid(TechCardConsts.AttributeTypes.WorkTypeAttrGuid);
      if (byGuid3 != null)
        this._atWorkTypeAttr = byGuid3;
      IAttributeTypeItem byGuid4 = imdi.AttributeTypes.GetByGuid(TechCardConsts.AttributeTypes.CehRouteAttrGUID);
      if (byGuid4 != null)
        this._atCehAttr = byGuid4;
      IAttributeTypeItem byGuid5 = imdi.AttributeTypes.GetByGuid(TechCardConsts.AttributeTypes.AreaRouteAttrGUID);
      if (byGuid5 != null)
        this._atAreaAttr = byGuid5;
      IAttributeTypeItem byGuid6 = imdi.AttributeTypes.GetByGuid(TechCardConsts.AttributeTypes.ElemRouteCaptionAttrGuid);
      if (byGuid6 != null)
        this._atCaptionAttr = byGuid6;
      IAttributeTypeItem byGuid7 = imdi.AttributeTypes.GetByGuid(new Guid("cadd9668-306c-11d8-b4e9-00304f19f545"));
      if (byGuid7 != null)
        this._attObjectPrototype = byGuid7;
      base.LoadMetaData4Pump();
    }
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechRouteElem;

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new ImportingCategory[3]
    {
      ImportingCategory.TechRouteTemplate,
      ImportingCategory.TechRouteElemUniqueCache,
      ImportingCategory.TechRoute
    };
  }

  protected override ImportingCategory[] GetCategoriesByNeed2FillTechObject()
  {
    return new ImportingCategory[5]
    {
      ImportingCategory.TechCeh,
      ImportingCategory.TechArea,
      ImportingCategory.TechWorkTypes,
      ImportingCategory.TechRouteElemUniqueCache,
      ImportingCategory.TechRouteElemTemplate
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

  public override void FillObjectParams(
    TechObjectRecord record,
    TechParamList parmList,
    ObjectRecord objectRec)
  {
    base.FillObjectParams(record, parmList, objectRec);
  }

  protected override ObjectRecord CreateTechObject(TechObjectRecord recBase)
  {
    return base.CreateTechObject(recBase);
  }

  protected override void CheckBaseRecords()
  {
  }

  protected override string GetUniqueRecordHash(TechObjectRecordBase recBase) => (string) null;

  protected override TechDataSource GetDataSource()
  {
    if (this._dataSource == null)
    {
      TechDataBuilderSimple<TechPumpBase> dataBuilder = new TechDataBuilderSimple<TechPumpBase>((TechPumpBase) this);
      dataBuilder.PumpModeCondFunc = (Func<string, string, string>) ((condField, dopType) =>
      {
        int recTypeId = 122;
        return TechDataBuilder<PumpClass>.GetPumpModeCond(condField, recTypeId);
      });
      this._dataSource = new TechDataSource((ITechDataBuilder) dataBuilder);
    }
    return this._dataSource;
  }

  protected override TechObjectRecord GetTpObjRec() => (TechObjectRecord) new TechRouteElemObject();

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    List<TechRelParam> links = new List<TechRelParam>(1);
    if (TechSettingsHelper.IgnoreRouteTemplates)
      this.LinkWithRoute(recBase, ipsObjId, links);
    else
      this.LinkWithTemplate(recBase, ipsObjId, links);
    return links;
  }

  private void LoadTemplate2RouteCache()
  {
    this._template2RouteCache.Clear();
    try
    {
      IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand();
      string str = TechDataBuilder<PumpClass>.GetPumpModeCond("F_KEY", 2);
      if (!string.IsNullOrEmpty(str))
        str = " where " + str;
      command.CommandText = "select F_KEY, F_TMP_OBRABOTKI, F_TMP_SBORKI from TC_NROUTES " + str;
      using (IDataReader dataReader = command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
      {
        int ordinal1 = dataReader.GetOrdinal("F_TMP_OBRABOTKI");
        int ordinal2 = dataReader.GetOrdinal("F_TMP_SBORKI");
        int ordinal3 = dataReader.GetOrdinal("F_KEY");
        while (dataReader.Read())
        {
          int int32_1 = dataReader.IsDBNull(ordinal1) ? 0 : BasePumpHelper.ToInt32(dataReader[ordinal1]);
          if (int32_1 > 0)
            this._template2RouteCache[int32_1] = BasePumpHelper.ToInt32(dataReader[ordinal3]);
          int int32_2 = dataReader.IsDBNull(ordinal1) ? 0 : BasePumpHelper.ToInt32(dataReader[ordinal2]);
          if (int32_2 > 0)
            this._template2RouteCache[int32_2] = BasePumpHelper.ToInt32(dataReader[ordinal3]);
        }
        dataReader.Close();
      }
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage("Невозможно прочитать кэш шаблонов к расцеховочным маршрутам: " + ex.Message);
      if (!(ex is OutOfMemoryException))
        return;
      throw;
    }
  }

  private void LinkWithRoute(
    TechObjectRecordBase elem,
    long ipsObjIdElem,
    List<TechRelParam> links)
  {
    int key;
    if (!this._template2RouteCache.TryGetValue(Convert.ToInt32(elem.Fields["F_TEMPLATE_ID"]), out key))
      return;
    long newKey = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechRoute, (object) key);
    if (newKey == 0L)
      return;
    links.Add(new TechRelParam(newKey, ipsObjIdElem, this._relTechRelationID, this._otRouteTypeId, this.objTypeID)
    {
      Sort = links.Count
    });
  }

  private void LinkWithTemplate(
    TechObjectRecordBase elem,
    long ipsObjIdElem,
    List<TechRelParam> links)
  {
    int int32 = Convert.ToInt32(elem.Fields["F_TEMPLATE_ID"]);
    if (!this.IsCloneRecord(elem))
    {
      long newKey = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechRouteTemplate, (object) int32);
      if (newKey == 0L)
        return;
      TechRelParam techRelParam = new TechRelParam(newKey, ipsObjIdElem, this._relTechRelationID, this._otRouteTemplateTypeId, this.objTypeID);
      links.Add(techRelParam);
    }
    else
    {
      TechRelParam techRelParam = this.AddRelationByObject(ImportingCategory.TechRouteTemplate, (object) int32, this._relTechRelationID, elem, ipsObjIdElem, this._otRouteTemplateTypeId, this.objTypeID);
      if (techRelParam == null)
        return;
      links.Add(techRelParam);
    }
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
      }
    }
    objRecord.Caption = $"{caption1} {caption2} - {caption3}".Truncate(Intermech.Consts.MaxStringSize - 2);
    if (this._atCaptionAttr != null)
      this._techParmList.AddAttribute(this._atCaptionAttr, (object) $"{caption1} {caption2} - {caption3}");
    if (this._atNaimAttrType != null)
      this._techParmList.AddAttribute(this._atNaimAttrType, (object) $"{caption1} {caption2} - {caption3}");
    if (this._atObozAttrType != null)
      this._techParmList.AddAttribute(this._atObozAttrType, (object) $"{caption1} {caption2} - {caption3}");
    if (this._attObjectPrototype != null)
    {
      DictionaryValue dictionaryValue = this._import_data_main.GetValue(ImportingCategory.TechRouteElemTemplate, (object) record.Key);
      if (dictionaryValue != null && dictionaryValue.NewObjectID != -1L)
        this._techParmList.AddAttribute(this._attObjectPrototype, (object) dictionaryValue.NewObjectID, dictionaryValue.Caption);
    }
    base.FillTechObject(objRecord, record);
  }

  public override void Exam() => base.Exam();

  public override void Pump()
  {
    base.LoadMetaData4Pump();
    if (TechSettingsHelper.IgnoreRouteTemplates)
      this.LoadTemplate2RouteCache();
    base.Pump();
  }

  protected override Guid GUID => this._guid;
}
