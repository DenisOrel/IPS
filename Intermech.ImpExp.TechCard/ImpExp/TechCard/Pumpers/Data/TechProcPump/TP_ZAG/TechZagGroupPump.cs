// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_ZAG.TechZagGroupPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.ImpExp.TechCard.TechProcPump.TP_ZAG;
using Intermech.Interfaces.TechCard;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_ZAG;

[TaskDescription("Инициализация данных для перекачки - Групповые заготовки", "Перекачка данных - групповые заготовки")]
internal class TechZagGroupPump(PluginClass plugin) : TechZagPump(plugin)
{
  private readonly Guid _guid = new Guid("{11C25233-3F16-4B58-9CEF-083BB1312A09}");

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechZagotGroup;

  protected override ImportingCategory GetTechParentCategory()
  {
    return ImportingCategory.TechZagotGroupParents;
  }

  protected override ImportingCategory GetTechUniqueCategory()
  {
    return ImportingCategory.TechZagotGroupUniquePump;
  }

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new ImportingCategory[0];
  }

  protected override string GetRecordPumpMode(TechObjectRecord record)
  {
    if (Convert.ToInt32(record.Fields["F_GROUPZAG_KEY"]) != 0)
      return base.GetRecordPumpMode(record);
    record.RecMode = TechObjectRecord.PumpMode.NotPump;
    return string.Empty;
  }

  protected override string GetUniqueRecordHash(TechObjectRecordBase record)
  {
    return Convert.ToInt32(record.Fields["F_GROUPZAG_KEY"]).ToString();
  }

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    return new List<TechRelParam>();
  }

  protected override TechDataSource GetDataSource() => base.GetDataSource();

  protected override void LoadMetaData4Pump()
  {
    IMetadataInfo imdi = this.plugin.Imdi;
    if (imdi == null)
    {
      this.plugin.appManager.AddErrorMessage("Ошибка получения кэша метаданных");
    }
    else
    {
      IObjectTypeItem byGuid = imdi.ObjectTypes.GetByGuid(TechCardConsts.ObjectTypes.ZagotGroupGUID);
      if (byGuid == null)
      {
        this.plugin.appManager.AddErrorMessage("Тип объектов - \"Групповая заготовка\" не найден");
      }
      else
      {
        this.objTypeID = byGuid.ID;
        base.LoadMetaData4Pump();
      }
    }
  }

  protected override Guid GUID => this._guid;
}
