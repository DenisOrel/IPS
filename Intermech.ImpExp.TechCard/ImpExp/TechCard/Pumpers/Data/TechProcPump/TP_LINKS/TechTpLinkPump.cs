// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_LINKS.TechTpLinkPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Common.TechCardSettings;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Data.Obj2Link;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.Interfaces.TechCard;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_LINKS;

[TaskDescription("Инициализация данных для перекачки - Связи с объектами сквозного ТП", "Перекачка данных - Связи с объектами сквозного ТП")]
internal class TechTpLinkPump(PluginClass plugin) : TechLinkBasePump(plugin)
{
  private readonly Guid _guid = new Guid("{75EEB2E1-EF3E-4DEB-82B0-DD1B03D33D2A}");

  protected override Guid GUID => this._guid;

  protected override void InitData()
  {
    this._relTypeGuid = TechCardConsts.RelTypes.TechThroughtTPRelationGuid;
    this._categoryA = ImportingCategory.None;
    this._categoryB = ImportingCategory.None;
    this._fieldAName = "F_SOURCE_KEY";
    this._fieldBName = "F_TARGET_KEY";
    this._tableName = "TP_LINKS";
    this._recType = "Связи с объектами сквозного ТП";
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechTPLinks;

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return this.GetLinkTypeCategories();
  }

  private ImportingCategory[] GetLinkTypeCategories()
  {
    return new ImportingCategory[2]
    {
      ImportingCategory.TechOperation,
      this.GetTechCategory()
    };
  }

  private ImportingCategory GetImportingCategoryByRecordTypeId(int recordTypeId)
  {
    if (recordTypeId != 0)
    {
      if (recordTypeId == 1)
        return ImportingCategory.TechOperation;
      this.plugin.appManager.AddWarningMessage("Неизвестный идентификатор типа: " + (object) recordTypeId);
    }
    return ImportingCategory.None;
  }

  protected override ImportingCategory GetACategory(TechObjectRecordBase record)
  {
    return this.GetImportingCategoryByRecordTypeId(Convert.ToInt32(record.Fields["F_SOURCE_TYPE"]));
  }

  protected override ImportingCategory GetBCategory(TechObjectRecordBase record)
  {
    return this.GetImportingCategoryByRecordTypeId(Convert.ToInt32(record.Fields["F_TARGET_TYPE"]));
  }

  protected override TechObjectRecord GetTpObjRec() => (TechObjectRecord) new TechTPLinkObject();

  protected override TechDataSource GetDataSource()
  {
    if (this._dataSource == null)
    {
      TechDataBuilderSimple<TechPumpBase> dataBuilder = new TechDataBuilderSimple<TechPumpBase>((TechPumpBase) this);
      dataBuilder.PumpModeCondFunc = (Func<string, string, string>) ((condField, dopType) =>
      {
        string dataSource = string.Empty;
        string str = TechSettingsHelper.PumpDataType.HasFlag((Enum) TechPumpDataType.TechProc) ? TechDataBuilder<PumpClass>.GetPumpModeCond("F_DOC_TCKEY", -2) : string.Empty;
        if (str != string.Empty)
          dataSource = str;
        return dataSource;
      });
      this._dataSource = new TechDataSource((ITechDataBuilder) dataBuilder);
    }
    return this._dataSource;
  }

  protected override long GetNewKeyB(TechObjectRecordBase record, int imObjAId)
  {
    TechDiffTag techDiffTagByOldKey = this.GetTechDiffTagByOldKey(this.GetACategory(record), (object) this.ConvertFieldAValue(imObjAId));
    List<Obj2LinkInfoObject> source;
    if (techDiffTagByOldKey != null && !techDiffTagByOldKey.IsCloneListEmpty && TechPumpData.TechObjects.Tp2LinkList.TryGetValue((long) Convert.ToInt32(record.Fields["F_DOC_TCKEY"]), out source))
    {
      int artTcKey = source.First<Obj2LinkInfoObject>().ArtTcKey;
      long newKeyB;
      if (techDiffTagByOldKey.CloneList.TryGetValue(artTcKey, out newKeyB))
        return newKeyB;
    }
    return base.GetNewKeyB(record, imObjAId);
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
    parmList.AddAttribute(this._atSortAttr, (object) int32);
  }

  protected override void CheckBaseRecords()
  {
  }
}
