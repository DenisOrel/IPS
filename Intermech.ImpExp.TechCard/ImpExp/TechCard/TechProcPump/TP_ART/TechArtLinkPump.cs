// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.TP_ART.TechArtLinkPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.TP_ART;

[TaskDescription("Инициализация данных для перекачки - Создание связей с Составом изделия", "Перекачка данных - Создание связей с Составом изделия")]
internal class TechArtLinkPump(PluginClass plugin) : TechLinkBasePump(plugin)
{
  private readonly Guid _guid = new Guid("{D449ACCB-16D6-4848-BBD4-24DF36BA28FA}");

  protected override Guid GUID => this._guid;

  protected override void InitData()
  {
    this._relTypeGuid = TechcardConsts.TypeConsts.rtTechRelationGuid;
    this._categoryA = ImportingCategory.None;
    this._categoryB = ImportingCategory.None;
    this._tableName = "TP_ART";
    this._recType = "Связь Состава изделия";
  }

  protected override long GetNewKeyB(TechObjectRecordBase record, int imObjBId)
  {
    return this.GetArticleInfoByKey(imObjBId).ObjVerId;
  }

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new ImportingCategory[4]
    {
      ImportingCategory.TechProcessPump,
      ImportingCategory.TechOperation,
      ImportingCategory.TechPerehPump,
      ImportingCategory.Articles
    };
  }

  protected override ImportingCategory GetACategory(TechObjectRecordBase record)
  {
    int int32 = Convert.ToInt32(record.Fields["F_OPERKEY"]);
    if (Convert.ToInt32(record.Fields["F_PEREHKEY"]) != 0)
      return ImportingCategory.TechPerehPump;
    return int32 != 0 ? ImportingCategory.TechOperation : ImportingCategory.TechProcessPump;
  }

  protected override ImportingCategory GetBCategory(TechObjectRecordBase record)
  {
    return ImportingCategory.Articles;
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechArtComLinks;

  protected override string GetFieldNameA(TechObjectRecordBase record)
  {
    int int32 = Convert.ToInt32(record.Fields["F_OPERKEY"]);
    if (Convert.ToInt32(record.Fields["F_PEREHKEY"]) != 0)
      return "F_PEREHKEY";
    return int32 != 0 ? "F_OPERKEY" : "F_DOCTCKEY";
  }

  protected override string GetFieldNameB(TechObjectRecordBase record) => "F_ARTTCKEY";

  protected override string GetRecordPumpMode(TechObjectRecord record)
  {
    switch (Convert.ToInt32(record.Fields["F_KIND"]))
    {
      case 4:
        record.RecMode = TechObjectRecord.PumpMode.ObjectAndLinks;
        return string.Empty;
      default:
        record.RecMode = TechObjectRecord.PumpMode.LinkOnly;
        return string.Empty;
    }
  }

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new TechObjectRecordDynamic("TP_ART");
  }

  protected override void CheckBaseRecords()
  {
  }

  public override void Exam() => base.Exam();

  public override void Pump() => base.Pump();
}
