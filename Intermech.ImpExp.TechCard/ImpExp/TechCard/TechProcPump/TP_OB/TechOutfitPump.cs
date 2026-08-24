// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.TP_OB.TechOutfitPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.Interfaces;
using Intermech.Interfaces.TechCard;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.TP_OB;

[TaskDescription("Инициализация данных для перекачки - Оборудование", "Перекачка данных - Оборудование")]
internal class TechOutfitPump(PluginClass plugin) : TechBaseUniquePump(plugin)
{
  private readonly Guid _guid = new Guid("{6251F47B-225D-4353-85D2-C542DD80E4CA}");

  protected override void InitData()
  {
    base.InitData();
    this._fldTblKey = "F_INVNOM";
    this._sortFieldName = "F_ORDER";
    this._recType = "B";
    this._recTypeID = 2;
    this._tableName = "TP_OB";
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
      IObjectTypeItem byGuid1 = imdi.ObjectTypes.GetByGuid(TechCardConsts.ObjectTypes.PerehodGUID);
      if (byGuid1 != null)
        this._otPerehTypeID = byGuid1.ID;
      IObjectTypeItem byGuid2 = imdi.ObjectTypes.GetByGuid(TechCardConsts.ObjectTypes.OperaciyaGUID);
      if (byGuid2 != null)
        this._otOperTypeID = byGuid2.ID;
      base.LoadMetaData4Pump();
    }
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechOutfitPump;

  protected override ImportingCategory GetTechUniqueCategory()
  {
    return ImportingCategory.TechOutfitUniquePump;
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

  protected override void CheckBaseRecords()
  {
  }

  protected override string GetRecordPumpMode(TechObjectRecord record)
  {
    return base.GetRecordPumpMode(record);
  }

  protected override string GetRecordRecKey(TechObjectRecordBase record) => "0";

  protected override string GetUniqueRecordHash(TechObjectRecordBase record)
  {
    if (!(record is TechObjectRecordUniqueDynamic recordUniqueDynamic))
      return base.GetUniqueRecordHash(record);
    if (!string.IsNullOrEmpty(recordUniqueDynamic.UniqueRecordHash))
      return recordUniqueDynamic.UniqueRecordHash;
    string uniqueRecordHash = $"{base.GetUniqueRecordHash(record)}:{this.NormalizeEntityValue(this._techParmList.GetEntityValue("МОД"))}:{this.NormalizeEntityValue(this._techParmList.GetEntityValue("ОБОР"))}";
    if (this._techParmList.Count != 0)
      recordUniqueDynamic.UniqueRecordHash = uniqueRecordHash;
    return uniqueRecordHash;
  }

  protected override string GetTechcardObjectCompareIndex()
  {
    string objectName = ImbaseObjectNameParser.ParseCompositeObjName(Convert.ToString(this._techParmList.GetEntityValue("МОД"))).ObjectName;
    if (string.IsNullOrEmpty(objectName))
    {
      string sourceImbaseObjName = Convert.ToString(this._techParmList.GetEntityValue("ОБОР"));
      if (!string.IsNullOrEmpty(sourceImbaseObjName))
        objectName = ImbaseObjectNameParser.ParseCompositeObjName(sourceImbaseObjName).ObjectName;
    }
    return objectName.Truncate(Intermech.Consts.MaxStringSize - 2);
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (objRecord == null || record == null || record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record.RecMode != TechObjectRecord.PumpMode.ObjectOnly)
      return;
    DateTime dateTime = Convert.ToDateTime(record.Fields["F_DATE"]);
    if (dateTime != DateTime.MinValue)
      objRecord.ObjCreate = dateTime.ToUniversalTime();
    string str = Convert.ToString(this._techParmList.GetEntityValue("МОД"));
    if (string.IsNullOrEmpty(str))
      str = Convert.ToString(this._techParmList.GetEntityValue("ОБОР"));
    if (!string.IsNullOrEmpty(str))
      objRecord.Caption = str.Truncate(Intermech.Consts.MaxStringSize - 2);
    base.FillTechObject(objRecord, record);
  }

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new TechObjectRecordUniqueDynamic("TP_OB");
  }

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    return Convert.ToInt32(recBase.Fields["F_INVNOM"]) != 0 ? new List<TechRelParam>() : base.CreateTechRelList(recBase, ipsObjId);
  }

  public override void Exam() => base.Exam();

  public override void Pump() => base.Pump();

  protected override Guid GUID => this._guid;
}
