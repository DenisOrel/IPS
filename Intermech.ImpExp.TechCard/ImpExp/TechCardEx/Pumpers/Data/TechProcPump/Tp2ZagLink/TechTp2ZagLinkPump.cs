// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCardEx.Pumpers.Data.TechProcPump.Tp2ZagLink.TechTp2ZagLinkPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.Tp2ZagLink;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.Interfaces.TechCard;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCardEx.Pumpers.Data.TechProcPump.Tp2ZagLink;

[TaskDescription("Инициализация данных для перекачки - Связь техпроцесса с заготовкой", "Перекачка данных - Связь техпроцесса с заготовкой")]
internal class TechTp2ZagLinkPump(PluginClass plugin) : TechLinkBasePump(plugin)
{
  private readonly Guid _guid = new Guid("{FE41DBF7-E560-4F75-8227-9A890EF0221E}");

  protected override Guid GUID => this._guid;

  protected override void InitData()
  {
    this._relTypeGuid = TechCardConsts.RelTypes.TechRelationGuid;
    this._categoryA = ImportingCategory.TechProcessPump;
    this._categoryB = ImportingCategory.TechZagot;
    this._fieldAName = "F_DOCTCKEY";
    this._fieldBName = "F_ZAGOTKEY";
    this._tableName = "TP_DOC_ZAG";
    this._recType = "Связь техпроцесса с заготовкой";
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechProc2ZagLinks;

  protected override long GetNewKeyA(TechObjectRecordBase record, int imObjAId)
  {
    TechDiffTag techDiffTagByOldKey = this.GetTechDiffTagByOldKey(this.GetACategory(record), (object) this.ConvertFieldAValue(imObjAId));
    long num;
    return techDiffTagByOldKey != null && !techDiffTagByOldKey.IsCloneListEmpty && techDiffTagByOldKey.CloneList.TryGetValue(Convert.ToInt32(record.Fields["F_ART_TCKEY"]), out num) ? num : base.GetNewKeyA(record, imObjAId);
  }

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new TechTp2ZagLinkObject();
  }

  protected override TechDataSource GetDataSource()
  {
    if (this._dataSource == null)
      this._dataSource = new TechDataSource((ITechDataBuilder) new TechTP2ZagLinkDataBuilder<TechPumpBase>((TechPumpBase) this));
    return this._dataSource;
  }

  public override void Exam() => base.Exam();

  public override void Pump() => base.Pump();

  public override bool CheckObjTypeOrParamType(string entCode, Guid attrGuid) => false;

  public override bool CheckObjLinkOrParamType(string entCode, Guid attrGuid) => false;

  public override void FillRecordParamsFixed(TechObjectRecord record, TechParamList parmList)
  {
    int int32 = Convert.ToInt32(record.Fields["F_ORDER"]);
    if (this._atSortAttr == null)
      return;
    parmList.AddAttribute(this._atTechTypeKeyAttr, (object) int32);
  }

  protected override void CheckBaseRecords()
  {
  }
}
