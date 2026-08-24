// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.TP_MAT.TechMaterialPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectData;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_MAT;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_MAT.MaterialGroupSubstitute;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.TP_MAT;

[TaskDescription("Инициализация данных для перекачки - Материалы", "Перекачка данных - Материалы")]
internal class TechMaterialPump(PluginClass plugin) : TechBaseUniquePump(plugin)
{
  private readonly Guid _guid = new Guid("{C758FB77-CFB0-48cf-9F51-8A45EDD0DA9A}");
  private int _objOperationsTypeIpsId = -1;
  private int _objMaterialsTypeIpsId = -1;
  private int _objTpTypeIpsId = -1;
  private int _objArtsTypeIpsId = -1;
  private int _objPerTypeIpsId = -1;
  private int _objDopPrTypeIpsId = -1;
  private int _objGrMatTypeIpsId = -1;
  private IImportedObjectList _localObjList;
  private IAttributeTypeItem _atSubstitutesGroupNo;
  private IAttributeTypeItem _atSubstituteInGroup;
  private IAttributeTypeItem _atSubstituteGroupName;
  private IAttributeTypeItem _atSubstituteName;

  protected override void InitData()
  {
    this._sortFieldName = "F_ORDER";
    this._recType = "M";
    this._recTypeID = 12;
    this._tableName = "TP_MAT";
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
      this._objOperationsTypeIpsId = TechPumpData.TechType.TechTypeList.GetObjTypeId(TechcardConsts.TpRecordType.Oper);
      this._objMaterialsTypeIpsId = TechPumpData.TechType.TechTypeList.GetObjTypeId(TechcardConsts.TpRecordType.MaterialAdd);
      this._objTpTypeIpsId = TechPumpData.TechType.TechTypeList.GetObjTypeId(TechcardConsts.TpRecordType.Passport);
      this._objArtsTypeIpsId = TechcardConsts.Plugin.Imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otProductObjTypeGuid).ID;
      this._objPerTypeIpsId = TechPumpData.TechType.TechTypeList.GetObjTypeId(TechcardConsts.TpRecordType.Perehod);
      this._objDopPrTypeIpsId = TechPumpData.TechType.TechTypeList.GetObjTypeId(TechcardConsts.TpRecordType.DopPriem);
      this._objGrMatTypeIpsId = TechPumpData.TechType.TechTypeList.GetObjTypeId(TechcardConsts.TpRecordType.MaterialGroup);
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

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechMatRawData;

  protected override string GetUniqueRecordHash(TechObjectRecordBase record)
  {
    if (!(record is TechObjectRecordUniqueDynamic recordUniqueDynamic))
      return base.GetUniqueRecordHash(record);
    if (!string.IsNullOrEmpty(recordUniqueDynamic.UniqueRecordHash))
      return recordUniqueDynamic.UniqueRecordHash;
    string uniqueRecordHash = $"{base.GetUniqueRecordHash(record)}:{this.NormalizeEntityValue(this._techParmList.GetEntityValue("Нвсм"))}:{this.NormalizeEntityValue(this._techParmList.GetEntityValue("Овсм"))}";
    if (this._techParmList.Count != 0)
      recordUniqueDynamic.UniqueRecordHash = uniqueRecordHash;
    return uniqueRecordHash;
  }

  protected override string GetRecordRecKey(TechObjectRecordBase record)
  {
    return Convert.ToString(this._techParmList.GetEntityValue("VMRc"));
  }

  protected override ImportingCategory GetTechUniqueCategory()
  {
    return ImportingCategory.TechMatUniquePump;
  }

  protected override ImportingCategory[] GetCategoriesByNeed2FillTechObject()
  {
    return new List<ImportingCategory>((IEnumerable<ImportingCategory>) base.GetCategoriesByNeed2FillTechObject())
    {
      ImportingCategory.Users
    }.ToArray();
  }

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return this.GetMaterialLinkTypes();
  }

  private ImportingCategory[] GetMaterialLinkTypes()
  {
    return new ImportingCategory[8]
    {
      ImportingCategory.TechMatGrPump,
      ImportingCategory.TechMatPump,
      ImportingCategory.TechOperation,
      ImportingCategory.TechPerehPump,
      ImportingCategory.TechProcessPump,
      ImportingCategory.TechMaterialPostLinks,
      ImportingCategory.TechMaterialGroupReplaceableCache,
      ImportingCategory.TechMaterialGroupSubstituteCache
    };
  }

  private ImportingCategory GetParentCacheCategory(int parentType)
  {
    switch (parentType)
    {
      case 0:
        return ImportingCategory.None;
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
        this.plugin.appManager.AddWarningMessage("Неизвестный идентификатор типа: " + (object) parentType);
        goto case 0;
    }
  }

  protected override void CheckBaseRecords()
  {
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (objRecord == null || record == null || record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record.RecMode != TechObjectRecord.PumpMode.ObjectOnly)
      return;
    int int32 = Convert.ToInt32(record.Fields["F_USERID"]);
    DateTime dateTime = Convert.ToDateTime(record.Fields["F_DATE"]);
    DictionaryValue userInfoBySearchId = this.GetUserInfoBySearchId(int32);
    if (userInfoBySearchId != null)
    {
      objRecord.OwnerId = userInfoBySearchId.NewObjectID;
      if (userInfoBySearchId.Tag is UserTag tag)
        objRecord.OwnerGuid = (object) tag.Guid;
    }
    if (dateTime != DateTime.MinValue)
      objRecord.ObjCreate = dateTime.ToUniversalTime();
    object entityValue = this._techParmList.GetEntityValue("Овсм");
    if (entityValue != null)
      objRecord.Caption = entityValue.ToString().Truncate(Consts.MaxStringSize - 2);
    base.FillTechObject(objRecord, record);
  }

  protected override void FillObjectObligatoryAttributes(TechObjectRecord record)
  {
    if (record.RecMode == TechObjectRecord.PumpMode.LinkOnly)
      return;
    base.FillObjectObligatoryAttributes(record);
  }

  public override void FillLinkParams(
    TechObjectRecordBase recBase,
    TechRelParam relRecord,
    TechParamList paramList)
  {
    if (this._import_data_main.GetValue(ImportingCategory.TechMaterialGroupReplaceableCache, (object) TechMaterialGroupSubstituteCacheInfo.GetObjectCacheCode(recBase.Key, this._recTypeID)) != null)
    {
      paramList.AddAttribute(this._atSubstitutesGroupNo, (object) recBase.Key, (string) null, EntitySetting.AttributeBelongs.ToLink);
      paramList.AddAttribute(this._atSubstituteInGroup, (object) 0, (string) null, EntitySetting.AttributeBelongs.ToLink);
      paramList.AddAttribute(this._atSubstituteGroupName, (object) recBase.Key, (string) null, EntitySetting.AttributeBelongs.ToLink);
      paramList.AddAttribute(this._atSubstituteName, (object) $"{recBase.Key}.0", (string) null, EntitySetting.AttributeBelongs.ToLink);
    }
    else
    {
      int int32_1 = Convert.ToInt32(recBase.Fields["F_PARENTKEY_1"]);
      int int32_2 = Convert.ToInt32(recBase.Fields["F_PARENTTYPE_1"]);
      if (int32_2 == 24)
      {
        DictionaryValue dictionaryValue = this._import_data_main.GetValue(ImportingCategory.TechMaterialGroupSubstituteCache, (object) TechMaterialGroupSubstituteCacheInfo.GetObjectCacheCode(int32_1, int32_2));
        TechMaterialGroupSubstituteCacheInfo substituteCacheInfo = (TechMaterialGroupSubstituteCacheInfo) null;
        if (dictionaryValue != null)
          substituteCacheInfo = ((TechObjectTag) dictionaryValue.Tag).Object as TechMaterialGroupSubstituteCacheInfo;
        if (substituteCacheInfo != null)
        {
          paramList.AddAttribute(this._atSubstitutesGroupNo, (object) substituteCacheInfo.ReplaceableObjectKey, (string) null, EntitySetting.AttributeBelongs.ToLink);
          paramList.AddAttribute(this._atSubstituteInGroup, (object) (substituteCacheInfo.Order + 1), (string) null, EntitySetting.AttributeBelongs.ToLink);
          paramList.AddAttribute(this._atSubstituteGroupName, (object) substituteCacheInfo.ReplaceableObjectKey, (string) null, EntitySetting.AttributeBelongs.ToLink);
          paramList.AddAttribute(this._atSubstituteName, (object) $"{substituteCacheInfo.ReplaceableObjectKey}.{substituteCacheInfo.Order + 1}", (string) null, EntitySetting.AttributeBelongs.ToLink);
        }
      }
    }
    base.FillLinkParams(recBase, relRecord, paramList);
  }

  protected override void AddValue2Cache(
    object oldKey,
    long newKey,
    TechObjectRecordBase recBase,
    TechParamList recParmList)
  {
    base.AddValue2Cache(oldKey, newKey, recBase, recParmList);
    if (Convert.ToInt32(recBase.Fields["F_PARENTKEY_2"]) == 0 || this._import_data_main.GetValue(ImportingCategory.TechMaterialGroupReplaceableCache, (object) TechMaterialGroupSubstituteCacheInfo.GetObjectCacheCode(recBase.Key, this._recTypeID)) != null)
    {
      this._import_data_main.AddValue(ImportingCategory.TechMatPump, oldKey, newKey);
    }
    else
    {
      ObjectRecord objectRecord = this._localObjList.AddObject(this._objGrMatTypeIpsId, 0, "Группа материалов");
      objectRecord.ObjCreate = DateTime.Now.ToUniversalTime();
      objectRecord.ModifyDate = DateTime.Now;
      objectRecord.IsBaseVersion = true;
      ImportingObject importingObject = this._localObjList.Items[this._localObjList.Items.Count - 1];
      this._localObjList.Import();
      this._import_data_main.AddValue(ImportingCategory.TechMatPump, oldKey, importingObject.Object.Object_id);
    }
  }

  protected override TechObjectRecord GetTpObjRec() => (TechObjectRecord) new TechMaterialObject();

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    int key = recBase.Key;
    int num1 = Convert.ToInt32(recBase.Fields["F_PARENTKEY_1"]);
    int num2 = Convert.ToInt32(recBase.Fields["F_PARENTTYPE_1"]);
    int int32 = Convert.ToInt32(recBase.Fields["F_PARENTKEY_2"]);
    int relTechRelationId = this._relTechRelationID;
    List<TechRelParam> techRelList = new List<TechRelParam>();
    if (num2 == 0 || num1 == 0)
      return techRelList;
    if (num2 == 24)
    {
      DictionaryValue dictionaryValue = this._import_data_main.GetValue(ImportingCategory.TechMaterialGroupSubstituteCache, (object) TechMaterialGroupSubstituteCacheInfo.GetObjectCacheCode(num1, num2));
      TechMaterialGroupSubstituteCacheInfo substituteCacheInfo = (TechMaterialGroupSubstituteCacheInfo) null;
      if (dictionaryValue != null)
        substituteCacheInfo = ((TechObjectTag) dictionaryValue.Tag).Object as TechMaterialGroupSubstituteCacheInfo;
      if (substituteCacheInfo != null)
      {
        num2 = substituteCacheInfo.ReplaceableParentType;
        num1 = substituteCacheInfo.ReplaceableParentKey;
      }
    }
    ImportingCategory parentCacheCategory = this.GetParentCacheCategory(num2);
    int ipsObjTypeB = -1;
    switch (parentCacheCategory)
    {
      case ImportingCategory.Articles:
        ipsObjTypeB = this._objArtsTypeIpsId;
        break;
      case ImportingCategory.TechOperation:
        ipsObjTypeB = this._objOperationsTypeIpsId;
        break;
      case ImportingCategory.TechProcessPump:
        ipsObjTypeB = this._objTpTypeIpsId;
        break;
      case ImportingCategory.TechPerehPump:
        ipsObjTypeB = this._objPerTypeIpsId;
        break;
      case ImportingCategory.TechMatPump:
        ipsObjTypeB = !TechCardPlugin.Configuration.SkipMaterialComposition ? this._objMaterialsTypeIpsId : -1;
        break;
      case ImportingCategory.TechMatGrPump:
        ipsObjTypeB = this._objGrMatTypeIpsId;
        break;
      case ImportingCategory.TechAddMovement:
        ipsObjTypeB = this._objDopPrTypeIpsId;
        break;
    }
    if (ipsObjTypeB == -1)
      return techRelList;
    long newKey1 = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, parentCacheCategory, (object) num1, false);
    if (newKey1 == 0L)
    {
      if (!this.IsCloneRecord(recBase))
        this._import_data_main.AddValue(ImportingCategory.TechMaterialPostLinks, (object) TechMaterialLinksPump.GenerateMatLinkKey(this._recTypeID, key, num2, num1), 1L);
    }
    else if (this.IsCloneRecord(recBase))
    {
      TechRelParam techRelParam = this.AddRelationByObject(parentCacheCategory, (object) num1, relTechRelationId, recBase, ipsObjId, ipsObjTypeB, this.objTypeID);
      if (techRelParam != null)
        techRelList.Add(techRelParam);
    }
    else if (int32 != 0 && this._import_data_main.GetValue(ImportingCategory.TechMaterialGroupReplaceableCache, (object) TechMaterialGroupSubstituteCacheInfo.GetObjectCacheCode(recBase.Key, this._recTypeID)) == null)
    {
      long newKey2 = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechMatPump, (object) key);
      if (newKey2 != 0L)
      {
        TechRelParam techRelParam1 = new TechRelParam(newKey1, newKey2, relTechRelationId, ipsObjTypeB, this._objGrMatTypeIpsId);
        techRelList.Add(techRelParam1);
        TechRelParam techRelParam2 = new TechRelParam(newKey2, ipsObjId, relTechRelationId, this._objGrMatTypeIpsId, this.objTypeID);
        techRelList.Add(techRelParam2);
      }
    }
    else
    {
      long num3 = 0;
      string str = string.Empty;
      if (num2 == 12)
      {
        str = TechMaterialLinksPump.GenerateIpsMatLinkKey(newKey1, ipsObjId, relTechRelationId);
        num3 = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechMaterialPostLinks, (object) str, false);
      }
      if (num3 == 0L)
      {
        TechRelParam techRelParam = new TechRelParam(newKey1, ipsObjId, relTechRelationId, ipsObjTypeB, this.objTypeID);
        techRelList.Add(techRelParam);
        if (num2 == 12)
          this._import_data_main.AddValue(ImportingCategory.TechMaterialPostLinks, (object) str, 1L);
      }
    }
    return techRelList;
  }

  protected override BaseTechObjInfo GetObjectInfoFromIps(string objName)
  {
    MaterialInfo material = this.plugin.Imdi.Materials.GetMaterial(objName, -1);
    if (material == null)
      return (BaseTechObjInfo) null;
    return new BaseTechObjInfo()
    {
      IpsObjVerId = material.ObjectID,
      CompareIndex = material.Caption
    };
  }

  protected override IEnumerable<int> GetIpsAttributesToCompareObjects()
  {
    return (IEnumerable<int>) new int[1]
    {
      MetaDataHelper.GetAttributeID((object) "cad00020-306c-11d8-b4e9-00304f19f545")
    };
  }

  protected override string GetTechcardObjectCompareIndex()
  {
    return ImbaseObjectNameParser.ParseCompositeObjName(Convert.ToString(this._techParmList.GetEntityValue("Овсм"))).ObjectName.Truncate(Consts.MaxStringSize - 2);
  }

  public override void Exam() => base.Exam();

  public override void Pump()
  {
    this._localObjList = this.plugin.Idw.CreateImportedObjectListWithStatistics(this.GUID);
    this._localObjList.PacketSize = 1;
    try
    {
      base.Pump();
    }
    finally
    {
      this._localObjList = (IImportedObjectList) null;
    }
  }

  protected override TechDataSource GetDataSource()
  {
    if (this._dataSource == null)
    {
      TechMaterialDataBuilder<TechBaseUniquePump> dataBuilder = new TechMaterialDataBuilder<TechBaseUniquePump>((TechBaseUniquePump) this);
      dataBuilder.PumpModeCondFunc = (Func<string, string, string>) ((condField, dopType) =>
      {
        string condField1 = dopType == "" ? " A.F_DOCTCKEY" : "F_TCKEY";
        string str = TechDataBuilder<PumpClass>.GetPumpModeCond(condField1, -2);
        string pumpModeCond = TechDataBuilder<PumpClass>.GetPumpModeCond(dopType == "" ? " A.F_SETKEY" : " B.F_SETKEY", 4);
        if (str == " 1 = 2 ")
          str = pumpModeCond == string.Empty ? $" {condField1} = 0 " : string.Empty;
        if (str == string.Empty)
          return pumpModeCond;
        return !(pumpModeCond != string.Empty) ? str : $"( {str} OR {pumpModeCond})";
      });
      this._dataSource = new TechDataSource((ITechDataBuilder) dataBuilder);
    }
    return this._dataSource;
  }

  protected override Guid GUID => this._guid;
}
