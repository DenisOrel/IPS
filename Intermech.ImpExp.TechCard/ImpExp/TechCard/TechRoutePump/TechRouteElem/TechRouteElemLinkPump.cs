// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechRoutePump.TechRouteElem.TechRouteElemLinkPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.Interfaces;
using Intermech.Interfaces.TechCard;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechRoutePump.TechRouteElem;

[TaskDescription("Инициализация данных для перекачки - Связь элементов расцеховки с шаблонами", "Перекачка данных - Связь элементов расцеховки с шаблонами")]
internal class TechRouteElemLinkPump(PluginClass plugin) : TechLinkBasePump(plugin)
{
  private readonly Guid _guid = new Guid("{2190BEF0-D95D-4FDA-8075-0E9D4F004288}");

  protected override Guid GUID => this._guid;

  protected override ImportingCategory GetTechCategory()
  {
    return ImportingCategory.LinksTechRouteElemLinkPump;
  }

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new List<ImportingCategory>((IEnumerable<ImportingCategory>) base.GetCategoriesByNeed2CreateTechRel())
    {
      ImportingCategory.TechRouteElemUniqueCache
    }.ToArray();
  }

  protected override void InitData()
  {
    this._relTypeGuid = TechCardConsts.RelTypes.TechRelationGuid;
    this._categoryA = ImportingCategory.TechRouteTemplate;
    this._categoryB = ImportingCategory.TechRouteElem;
    this._fieldAName = "F_TEMPLATE_ID";
    this._fieldBName = "F_KEY";
    this._tableName = "TC_NROUTE_STRINGS";
  }

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new TechObjectRecordDynamic();
  }

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

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    return new List<TechRelParam>();
  }

  public override void Exam() => base.Exam();

  public override void Pump() => base.Pump();

  public override bool CheckObjTypeOrParamType(string entCode, Guid attrGuid) => false;

  public override bool CheckObjLinkOrParamType(string entCode, Guid attrGuid) => false;

  public override void FillRecordParamsFixed(TechObjectRecord record, TechParamList parmList)
  {
  }

  protected override void CheckBaseRecords()
  {
  }

  protected override long GetNewKeyA(TechObjectRecordBase record, int imObjAId)
  {
    return ImportingDataHelper.Instance.GetNewKey(this._import_data_main, this.GetACategory(record), (object) this.ConvertFieldAValue(imObjAId));
  }

  protected override RelationRecord CreateTechRel(
    long ipsObjectAId,
    long ipsObjectBId,
    int relTypeId)
  {
    return base.CreateTechRel(ipsObjectAId, ipsObjectBId, relTypeId);
  }
}
