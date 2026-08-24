// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.TP_OB.TechOutfitInvNomLinksPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.Interfaces.TechCard;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.TP_OB;

[TaskDescription("Инициализация данных для перекачки - Связь Операций с Оборудованием", "Перекачка данных - Связь Операций с Оборудованием")]
internal class TechOutfitInvNomLinksPump(PluginClass plugin) : TechLinkBasePump(plugin)
{
  private readonly Guid _guid = new Guid("{F4988E7C-475C-4f4d-9845-50B2DBBEA67E}");

  protected override void InitData()
  {
    this._relTypeGuid = TechCardConsts.RelTypes.TechRelationGuid;
    this._categoryA = ImportingCategory.TechOperation;
    this._categoryB = ImportingCategory.TechInvNomPump;
    this._fieldAName = "F_OPERKEY";
    this._fieldBName = "F_INVNOM";
    this._tableName = "TP_OB";
  }

  protected override Guid GUID => this._guid;

  protected override ImportingCategory GetTechCategory()
  {
    return ImportingCategory.TechOutfilt_InvNomLinks;
  }

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new List<ImportingCategory>((IEnumerable<ImportingCategory>) base.GetCategoriesByNeed2CreateTechRel())
    {
      ImportingCategory.TechOutfitPump
    }.ToArray();
  }

  protected override long GetNewKeyB(TechObjectRecordBase record, int imObjBId)
  {
    long newKey = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, this.GetBCategory(record), (object) this.ConvertFieldBValue(imObjBId), false);
    return newKey != 0L ? newKey : ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechOutfitPump, (object) record.Key);
  }

  protected override void CheckBaseRecords()
  {
  }

  protected override string GetRecordPumpMode(TechObjectRecord record)
  {
    if (Convert.ToInt32(record.Fields["F_INVNOM"]) == 0)
    {
      record.RecMode = TechObjectRecord.PumpMode.NotPump;
      return string.Empty;
    }
    record.RecMode = TechObjectRecord.PumpMode.ObjectAndLinks;
    return string.Empty;
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

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new TechObjectRecordDynamic();
  }

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    return base.CreateTechRelList(recBase, ipsObjId);
  }

  public override void Exam() => base.Exam();

  public override void Pump() => base.Pump();
}
