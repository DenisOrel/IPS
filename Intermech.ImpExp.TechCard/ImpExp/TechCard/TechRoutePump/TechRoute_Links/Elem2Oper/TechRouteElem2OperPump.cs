// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechRoutePump.TechRoute_Links.Elem2Oper.TechRouteElem2OperPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRoute_Links.Elem2Oper;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.Interfaces;
using Intermech.Interfaces.TechCard;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechRoutePump.TechRoute_Links.Elem2Oper;

[TaskDescription("Инициализация данных для перекачки - Связь расцеховок с техпроцессами", "Перекачка данных - Связь расцеховок с техпроцессами")]
internal class TechRouteElem2OperPump(PluginClass plugin) : TechLinkBasePump(plugin)
{
  private readonly Guid _guid = new Guid("{B43F06A2-F13D-40fd-866D-9D01202D8F55}");
  private readonly HashSet<Tuple<long, long>> _importedObjectsList = new HashSet<Tuple<long, long>>();

  protected override Guid GUID => this._guid;

  protected override ImportingCategory GetTechCategory()
  {
    return ImportingCategory.LinksTechRouteElem2OperPump;
  }

  protected override void InitData()
  {
    this._relTypeGuid = TechCardConsts.RelTypes.TechRouteRelationGuid;
    this._categoryA = ImportingCategory.TechCehZahodPump;
    this._categoryB = ImportingCategory.TechRouteElem;
    this._fieldAName = "F_OPER_KEY";
    this._fieldBName = "F_STRING_ID";
    this._tableName = "TC_NROUTE_TPOPER";
  }

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new TechObjectRecordDynamic();
  }

  protected override TechDataSource GetDataSource()
  {
    if (this._dataSource == null)
    {
      TechRouteElem2OperDataBuilder<TechPumpBase> dataBuilder = new TechRouteElem2OperDataBuilder<TechPumpBase>((TechPumpBase) this);
      dataBuilder.PumpModeCondFunc = (Func<string, string, string>) ((condField, dopType) => string.Empty);
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
    long newKey = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, this.GetACategory(record), (object) this.ConvertFieldAValue(imObjAId), false);
    if (newKey < 0L)
      newKey = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, this.GetACategory(record), (object) -newKey);
    return newKey;
  }

  protected override RelationRecord CreateTechRel(
    long ipsObjectAId,
    long ipsObjectBId,
    int relTypeId)
  {
    Tuple<long, long> tuple = new Tuple<long, long>(ipsObjectAId, ipsObjectBId);
    if (this._importedObjectsList.Contains(tuple))
      return (RelationRecord) null;
    this._importedObjectsList.Add(tuple);
    return base.CreateTechRel(ipsObjectAId, ipsObjectBId, relTypeId);
  }

  protected override void ClearTmpData()
  {
    this._importedObjectsList.Clear();
    base.ClearTmpData();
  }
}
