// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.TP_MAT.TechMaterialLinksPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_MAT;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_MAT.MaterialGroupSubstitute;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.TP_MAT;

[TaskDescription("Инициализация данных для перекачки - Связь материалами", "Перекачка данных - Создание связей с материалами")]
internal class TechMaterialLinksPump(PluginClass plugin) : TechLinkBasePump(plugin)
{
  private IAttributeTypeItem _atSubstitutesGroupNo;
  private IAttributeTypeItem _atSubstituteInGroup;
  private IAttributeTypeItem _atSubstituteGroupName;
  private IAttributeTypeItem _atSubstituteName;
  private readonly Guid _guid = new Guid("{DA189F29-4F01-4260-A22D-3A3110FFAF65}");

  protected override void InitData()
  {
    this._relTypeGuid = TechcardConsts.TypeConsts.rtTechRelationGuid;
    this._categoryA = ImportingCategory.None;
    this._categoryB = ImportingCategory.None;
    this._fieldAName = "F_PARENTKEY";
    this._fieldBName = "F_CHILDKEY";
    this._tableName = "TP_MAT_LINKS";
    this._recType = "Связь с заготовкой";
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
      IAttributeTypeItem byGuid1 = imdi.AttributeTypes.GetByGuid(new Guid("cad001c0-306c-11d8-b4e9-00304f19f545"));
      if (byGuid1 != null)
        this._atSubstitutesGroupNo = byGuid1;
      IAttributeTypeItem byGuid2 = imdi.AttributeTypes.GetByGuid(new Guid("cad001c1-306c-11d8-b4e9-00304f19f545"));
      if (byGuid2 != null)
        this._atSubstituteInGroup = byGuid2;
      IAttributeTypeItem byGuid3 = imdi.AttributeTypes.GetByGuid(new Guid("cad00817-306c-11d8-b4e9-00304f19f545"));
      if (byGuid3 != null)
        this._atSubstituteGroupName = byGuid3;
      IAttributeTypeItem byGuid4 = imdi.AttributeTypes.GetByGuid(new Guid("cad00818-306c-11d8-b4e9-00304f19f545"));
      if (byGuid4 != null)
        this._atSubstituteName = byGuid4;
      base.LoadMetaData4Pump();
    }
  }

  protected override Guid GUID => this._guid;

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return this.GetMaterialLinkTypes();
  }

  protected override ImportingCategory GetACategory(TechObjectRecordBase record)
  {
    int int32 = Convert.ToInt32(record.Fields["F_PARENTTYPE"]);
    switch (int32)
    {
      case 0:
        return base.GetACategory(record);
      case 1:
        return ImportingCategory.TechOperation;
      case 5:
        return ImportingCategory.TechAddMovement;
      case 12:
        return ImportingCategory.TechMatPump;
      case 14:
        return ImportingCategory.TechPerehPump;
      case 15:
        return ImportingCategory.TechProcessPump;
      case 17:
        return ImportingCategory.None;
      case 24:
        return ImportingCategory.TechMatGrPump;
      default:
        this.plugin.appManager.AddWarningMessage("Неизвестный идентификатор типа: " + (object) int32);
        goto case 0;
    }
  }

  protected override ImportingCategory GetBCategory(TechObjectRecordBase record)
  {
    int int32 = Convert.ToInt32(record.Fields["F_CHILDTYPE"]);
    switch (int32)
    {
      case 0:
        return base.GetBCategory(record);
      case 12:
        return ImportingCategory.TechMatPump;
      case 24:
        return ImportingCategory.TechMatGrPump;
      default:
        this.plugin.appManager.AddWarningMessage("Неизвестный идентификатор типа: " + (object) int32);
        goto case 0;
    }
  }

  protected override ImportingCategory GetTechCategory()
  {
    return ImportingCategory.LinksTechMaterialsPump;
  }

  private ImportingCategory[] GetMaterialLinkTypes()
  {
    return new ImportingCategory[9]
    {
      ImportingCategory.TechMatGrPump,
      ImportingCategory.TechMatPump,
      ImportingCategory.TechOperation,
      ImportingCategory.TechPerehPump,
      ImportingCategory.TechProcessPump,
      this.GetTechCategory(),
      ImportingCategory.TechMaterialPostLinks,
      ImportingCategory.TechMaterialGroupReplaceableCache,
      ImportingCategory.TechMaterialGroupSubstituteCache
    };
  }

  protected override string GetRecordPumpMode(TechObjectRecord record)
  {
    int int32_1 = Convert.ToInt32(record.Fields["F_CHILDTYPE"]);
    int int32_2 = Convert.ToInt32(record.Fields["F_CHILDKEY"]);
    int int32_3 = Convert.ToInt32(record.Fields["F_PARENTTYPE"]);
    int int32_4 = Convert.ToInt32(record.Fields["F_PARENTKEY"]);
    if (ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechMaterialPostLinks, (object) TechMaterialLinksPump.GenerateMatLinkKey(int32_1, int32_2, int32_3, int32_4), false) == 0L)
    {
      record.RecMode = TechObjectRecord.PumpMode.NotPump;
      return string.Empty;
    }
    if (TechCardPlugin.Configuration.SkipMaterialComposition && int32_1 == 12 && int32_3 == 12)
    {
      record.RecMode = TechObjectRecord.PumpMode.NotPump;
      return string.Empty;
    }
    if (this._import_data_main.GetValue(ImportingCategory.TechMaterialGroupSubstituteCache, (object) TechMaterialGroupSubstituteCacheInfo.GetObjectCacheCode(int32_2, int32_1)) != null)
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
      dataBuilder.PumpModeCondFunc = (Func<string, string, string>) ((condField, dopType) =>
      {
        string pumpModeCond1 = TechDataBuilder<PumpClass>.GetPumpModeCond("F_TCKEY", -2);
        string pumpModeCond2 = TechDataBuilder<PumpClass>.GetPumpModeCond("F_SETKEY", 4);
        if (pumpModeCond1 == string.Empty)
          return pumpModeCond2;
        return !(pumpModeCond2 != string.Empty) ? pumpModeCond1 : $"( {pumpModeCond1} OR {pumpModeCond2})";
      });
      this._dataSource = new TechDataSource((ITechDataBuilder) dataBuilder);
    }
    return this._dataSource;
  }

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new TechMaterialLinkObject();
  }

  protected override void CheckBaseRecords()
  {
  }

  protected override int CreateRelations(
    string pumpBegin,
    int relTypeId,
    int i,
    int recCount,
    TechObjectRecordBase record)
  {
    if (record == null)
      return i;
    int int32_1 = Convert.ToInt32(record.Fields["F_CHILDTYPE"]);
    int int32_2 = Convert.ToInt32(record.Fields["F_CHILDKEY"]);
    int int32_3 = Convert.ToInt32(record.Fields["F_PARENTTYPE"]);
    int int32_4 = Convert.ToInt32(record.Fields["F_PARENTKEY"]);
    TechMaterialGroupSubstituteCacheInfo substituteCacheInfo = (TechMaterialGroupSubstituteCacheInfo) null;
    if (int32_3 == 24)
    {
      DictionaryValue dictionaryValue = this._import_data_main.GetValue(ImportingCategory.TechMaterialGroupSubstituteCache, (object) TechMaterialGroupSubstituteCacheInfo.GetObjectCacheCode(int32_4, int32_3));
      if (dictionaryValue != null)
        substituteCacheInfo = ((TechObjectTag) dictionaryValue.Tag).Object as TechMaterialGroupSubstituteCacheInfo;
      if (substituteCacheInfo != null)
      {
        record.Fields["F_PARENTTYPE"] = (object) substituteCacheInfo.ReplaceableParentType;
        record.Fields["F_PARENTKEY"] = (object) substituteCacheInfo.ReplaceableParentKey;
      }
    }
    int relations = base.CreateRelations(pumpBegin, relTypeId, i, recCount, record);
    if (i == relations)
      return relations;
    if (this._import_data_main.GetValue(ImportingCategory.TechMaterialGroupReplaceableCache, (object) TechMaterialGroupSubstituteCacheInfo.GetObjectCacheCode(int32_2, int32_1)) != null)
    {
      this._impRelList.AddAttribute(this._atSubstitutesGroupNo.ID, (AttrValueType) this._atSubstitutesGroupNo.AttrValueType, (object) int32_2, 0);
      this._impRelList.AddAttribute(this._atSubstituteInGroup.ID, (AttrValueType) this._atSubstituteInGroup.AttrValueType, (object) 0, 0);
      this._impRelList.AddAttribute(this._atSubstituteGroupName.ID, (AttrValueType) this._atSubstituteGroupName.AttrValueType, (object) int32_2, 0);
      this._impRelList.AddAttribute(this._atSubstituteName.ID, (AttrValueType) this._atSubstituteName.AttrValueType, (object) $"{int32_2}.0", 0);
    }
    else if (substituteCacheInfo != null)
    {
      this._impRelList.AddAttribute(this._atSubstitutesGroupNo.ID, (AttrValueType) this._atSubstitutesGroupNo.AttrValueType, (object) substituteCacheInfo.ReplaceableObjectKey, 0);
      this._impRelList.AddAttribute(this._atSubstituteInGroup.ID, (AttrValueType) this._atSubstituteInGroup.AttrValueType, (object) (substituteCacheInfo.Order + 1), 0);
      this._impRelList.AddAttribute(this._atSubstituteGroupName.ID, (AttrValueType) this._atSubstituteGroupName.AttrValueType, (object) substituteCacheInfo.ReplaceableObjectKey, 0);
      this._impRelList.AddAttribute(this._atSubstituteName.ID, (AttrValueType) this._atSubstituteName.AttrValueType, (object) $"{substituteCacheInfo.ReplaceableObjectKey}.{substituteCacheInfo.Order + 1}", 0);
    }
    return relations;
  }

  public override void Exam() => base.Exam();

  public override void Pump() => base.Pump();

  internal static string GenerateMatLinkKey(int typeA, int keyA, int typeB, int keyB)
  {
    long num1 = TechcardConsts.Utils.CodeHashCode(typeA, keyA);
    long num2 = TechcardConsts.Utils.CodeHashCode(typeB, keyB);
    if (num1 < num2)
    {
      long num3 = num2;
      num2 = num1;
      num1 = num3;
    }
    return $"{num1}_{num2}";
  }

  internal static string GenerateIpsMatLinkKey(long keyA, long keyB, int relType = -1)
  {
    return $"IPS_{keyA}_{keyB}_{relType}";
  }
}
