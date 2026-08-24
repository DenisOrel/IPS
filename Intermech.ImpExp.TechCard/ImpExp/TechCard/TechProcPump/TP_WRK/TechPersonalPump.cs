// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.TP_WRK.TechPersonalPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.TP_WRK;

[TaskDescription("Инициализация данных для перекачки - Персонал", "Перекачка данных - Персонал")]
internal class TechPersonalPump(PluginClass plugin) : TechBaseUniquePump(plugin)
{
  protected Guid _guid = new Guid("{C75DDB63-CFB0-48cf-9F51-8A40DDD0DA9A}");

  protected override Guid GUID => this._guid;

  protected override void InitData()
  {
    base.InitData();
    this._recType = "I";
    this._recTypeID = 9;
    this._tableName = "TP_WRK";
    this._dopTypes.Add("S");
    this._dopTypes.Add("I");
    this._dopTypes.Add("F");
    this._dopTypes.Add("D");
    this._sortFieldName = "F_ORDER";
  }

  protected override void LoadMetaData4Pump()
  {
    if (this.plugin.Imdi == null)
      this.plugin.appManager.AddErrorMessage("Ошибка получения кэша метаданных");
    else
      base.LoadMetaData4Pump();
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechPersonalPump;

  protected override ImportingCategory GetTechUniqueCategory()
  {
    return ImportingCategory.TechPersonalUniquePump;
  }

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new ImportingCategory[3]
    {
      ImportingCategory.TechOperation,
      ImportingCategory.TechPerehPump,
      ImportingCategory.TechProcessPump
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

  protected override string GetRecordRecKey(TechObjectRecordBase record)
  {
    return Convert.ToString(this._techParmList.GetEntityValue("risp"));
  }

  protected override string GetUniqueRecordHash(TechObjectRecordBase record)
  {
    if (!(record is TechObjectRecordUniqueDynamic recordUniqueDynamic))
      return base.GetUniqueRecordHash(record);
    if (!string.IsNullOrEmpty(recordUniqueDynamic.UniqueRecordHash))
      return recordUniqueDynamic.UniqueRecordHash;
    string uniqueRecordHash = $"{base.GetUniqueRecordHash(record)}:{this.NormalizeEntityValue(this._techParmList.GetEntityValue("NIsp"))}";
    if (this._techParmList.Count != 0)
      recordUniqueDynamic.UniqueRecordHash = uniqueRecordHash;
    return uniqueRecordHash;
  }

  protected override string GetTechcardObjectCompareIndex()
  {
    return ImbaseObjectNameParser.ParseCompositeObjName(Convert.ToString(this._techParmList.GetEntityValue("NIsp"))).ObjectName.Truncate(Consts.MaxStringSize - 2);
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (objRecord == null || record == null || record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record.RecMode != TechObjectRecord.PumpMode.ObjectOnly)
      return;
    object entityValue = this._techParmList.GetEntityValue("NIsp");
    if (entityValue != null)
      objRecord.Caption = entityValue.ToString().Truncate(Consts.MaxStringSize - 2);
    base.FillTechObject(objRecord, record);
  }

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new TechObjectRecordUniqueDynamic("TP_WRK");
  }

  public override void Exam() => base.Exam();

  public override void Pump() => base.Pump();
}
