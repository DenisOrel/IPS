// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.TP_ART.TechArtPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.ImpExp.TechCard.TechProcPump.Common.TechProcessPump;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.TP_ART;

[TaskDescription("Инициализация данных для перекачки - Состав Изделия Techcard", "Перекачка данных - Состав Изделия Techcard")]
internal class TechArtPump(PluginClass plugin) : TechPumpBase(plugin)
{
  private readonly Guid _guid = new Guid("{C758FC63-CFB0-48cf-9F51-8A75E0D0DA9A}");
  protected int _otKomplArtID = -1;
  protected int _otSobArtID = -1;
  protected IAttributeTypeItem _atObjectLinktAttr;

  protected override void InitData()
  {
    this._sortFieldName = "F_ORDER";
    this._recType = "N";
    this._recTypeID = 13;
    this._tableName = "TP_ART";
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
      IObjectTypeItem byGuid1 = imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otKomplArt);
      if (byGuid1 != null)
        this._otKomplArtID = byGuid1.ID;
      IObjectTypeItem byGuid2 = imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otSobArt);
      if (byGuid2 != null)
        this._otSobArtID = byGuid2.ID;
      IAttributeTypeItem byGuid3 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atObjectLinktAttrGuid);
      if (byGuid3 != null)
        this._atObjectLinktAttr = byGuid3;
      base.LoadMetaData4Pump();
    }
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechArticlesPump;

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new ImportingCategory[4]
    {
      ImportingCategory.TechOperation,
      ImportingCategory.TechPerehPump,
      ImportingCategory.TechProcessPump,
      ImportingCategory.TechArtCompositionKeys
    };
  }

  protected override ImportingCategory[] GetCategoriesByNeed2FillTechObject()
  {
    return new ImportingCategory[2]
    {
      ImportingCategory.Articles,
      ImportingCategory.TechArtCompositionKeys
    };
  }

  protected override void CheckBaseRecords()
  {
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
    return (TechObjectRecord) new TechObjectRecordDynamic("TP_ART");
  }

  protected override int GetObjectType(TechObjectRecordBase record)
  {
    object entityValue = this._techParmList.GetEntityValue("PrUz");
    return entityValue != null ? (entityValue.ToString().Equals("2") ? this._otKomplArtID : this._otSobArtID) : (this.GetParentArtCompositionId(this._techParmList) != 0 || Convert.ToInt32(record.Fields["F_ORDER"]) != 0 && this.GetArtCompositionUnicalId(this._techParmList) == 0 ? this._otKomplArtID : this._otSobArtID);
  }

  private int GetParentArtCompositionId(TechParamList paramList)
  {
    int artCompositionId = 0;
    if (paramList == null)
      return artCompositionId;
    object entityValue = paramList.GetEntityValue("OwID");
    if (entityValue != null)
      artCompositionId = Convert.ToInt32(entityValue);
    return artCompositionId;
  }

  private int GetArtCompositionUnicalId(TechParamList paramList)
  {
    int compositionUnicalId = 0;
    if (paramList == null)
      return compositionUnicalId;
    object entityValue = paramList.GetEntityValue("ArID");
    if (entityValue != null)
      compositionUnicalId = Convert.ToInt32(entityValue);
    return compositionUnicalId;
  }

  private int GetCurentImbLinkKey(int iTpKey, int iOperKey, int iPerehKey)
  {
    if (iPerehKey != 0)
      return iPerehKey;
    return iOperKey != 0 ? iOperKey : iTpKey;
  }

  private string CreateUnicalStringArtComKey(int artId, int artIdEntKey, int parentKey)
  {
    return $"{artId}_{artIdEntKey}_{parentKey}";
  }

  protected override ObjectRecord CreateTechObject(TechObjectRecord record)
  {
    ObjectRecord objectRec = this._impObjList.AddObject(this.GetObjectType((TechObjectRecordBase) record), 0);
    int int32_1 = Convert.ToInt32(record.Fields["F_DOCTCKEY"]);
    int int32_2 = Convert.ToInt32(record.Fields["F_OPERKEY"]);
    int int32_3 = Convert.ToInt32(record.Fields["F_PEREHKEY"]);
    int int32_4 = Convert.ToInt32(record.Fields["F_ORDER"]);
    int curentImbLinkKey = this.GetCurentImbLinkKey(int32_1, int32_2, int32_3);
    int artCompositionId = this.GetParentArtCompositionId(this._techParmList);
    bool flag = false;
    if (artCompositionId == 0)
    {
      int compositionUnicalId = this.GetArtCompositionUnicalId(this._techParmList);
      if (compositionUnicalId != 0 || int32_4 == 0)
      {
        string unicalStringArtComKey = this.CreateUnicalStringArtComKey(record.diff_ArtTcKey, compositionUnicalId, curentImbLinkKey);
        if (this._import_data_main.GetValue(ImportingCategory.TechArtCompositionKeys, (object) unicalStringArtComKey) == null)
          this._import_data_main.AddValue(ImportingCategory.TechArtCompositionKeys, (object) unicalStringArtComKey, (long) record.Key);
        flag = true;
      }
    }
    if (!flag)
    {
      string unicalStringArtComKey = this.CreateUnicalStringArtComKey(record.diff_ArtTcKey, artCompositionId, curentImbLinkKey);
      record.SetFieldValue("F_OWNER_KEY", (object) unicalStringArtComKey);
    }
    int num = this._impObjList.Items.Count - 1;
    this._techBaseImportList.Add((TechObjectRecordBase) record, num);
    this.FillTechObject(objectRec, record);
    return objectRec;
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (objRecord == null || record == null || record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record.RecMode != TechObjectRecord.PumpMode.ObjectOnly)
      return;
    int int32 = Convert.ToInt32(record.Fields["F_ARTTCKEY"]);
    string empty1 = string.Empty;
    object entityValue1 = this._techParmList.GetEntityValue("Ндет");
    if (entityValue1 != null)
      empty1 = entityValue1.ToString();
    string empty2 = string.Empty;
    object entityValue2 = this._techParmList.GetEntityValue("Одет");
    if (entityValue2 != null)
      empty2 = entityValue2.ToString();
    if (int32 != 0 && this._atObjectLinktAttr != null)
    {
      (long ObjId, long ObjVerId, string Caption) articleInfoByKey = this.GetArticleInfoByKey(int32);
      if (articleInfoByKey.ObjVerId != 0L)
        this._techParmList.AddAttribute(this._atObjectLinktAttr, (object) articleInfoByKey.ObjVerId, articleInfoByKey.Caption);
    }
    if (!empty1.Equals(string.Empty) || !empty2.Equals(string.Empty))
      objRecord.Caption = $"{empty2}({empty1})".Truncate(Consts.MaxStringSize - 2);
    base.FillTechObject(objRecord, record);
  }

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    List<TechRelParam> techRelList = new List<TechRelParam>();
    int relTechRelationId = this._relTechRelationID;
    int num1 = Convert.ToInt32(recBase.Fields["F_DOCTCKEY"]);
    int num2 = Convert.ToInt32(recBase.Fields["F_OPERKEY"]);
    int num3 = Convert.ToInt32(recBase.Fields["F_PEREHKEY"]);
    string str = "0";
    object obj1;
    if (recBase.Fields.TryGetValue("F_OWNER_KEY", out obj1))
      str = obj1.ToString();
    int objectType = this.GetObjectType(recBase);
    bool flag = true;
    string oldKey = str;
    if (!oldKey.Equals(string.Empty))
    {
      long newKey1 = this._import_data_main.GetNewKey(ImportingCategory.TechArtCompositionKeys, (object) oldKey);
      if (newKey1 != 0L)
      {
        long newKey2 = this._import_data_main.GetNewKey(this.GetTechCategory(), (object) newKey1);
        if (newKey2 != 0L)
        {
          TechRelParam techRelParam = new TechRelParam(newKey2, ipsObjId, relTechRelationId, objectType, objectType);
          techRelList.Add(techRelParam);
          flag = false;
        }
      }
    }
    if (flag)
    {
      if (num3 < 0)
        num3 = 0;
      if (num2 < 0)
        num2 = 0;
      if (num1 < 0)
        num1 = 0;
      if (num3 != 0 && num2 != 0)
      {
        if (this.IsCloneRecord(recBase))
        {
          TechRelParam techRelParam = this.AddRelationByObject(ImportingCategory.TechPerehPump, (object) num3, relTechRelationId, recBase, ipsObjId, this._otPerehTypeID, objectType);
          if (techRelParam != null)
            techRelList.Add(techRelParam);
        }
        else
        {
          long newKey = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechPerehPump, (object) num3);
          if (newKey != 0L)
          {
            TechRelParam techRelParam = new TechRelParam(newKey, ipsObjId, relTechRelationId, this._otPerehTypeID, objectType);
            techRelList.Add(techRelParam);
          }
        }
      }
      else if (num2 != 0)
      {
        if (this.IsCloneRecord(recBase))
        {
          TechRelParam techRelParam = this.AddRelationByObject(ImportingCategory.TechOperation, (object) num2, relTechRelationId, recBase, ipsObjId, this._otOperTypeID, objectType);
          if (techRelParam != null)
            techRelList.Add(techRelParam);
        }
        else
        {
          long newKey = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechOperation, (object) num2);
          if (newKey != 0L)
          {
            TechRelParam techRelParam = new TechRelParam(newKey, ipsObjId, relTechRelationId, this._otOperTypeID, objectType);
            techRelList.Add(techRelParam);
          }
        }
      }
      else if (num1 != 0)
      {
        DictionaryValue dictionaryValue = ImportingDataHelper.Instance.GetValue(this._import_data_main, ImportingCategory.TechProcessPump, (object) num1, false);
        int result = -1;
        if (dictionaryValue != null && dictionaryValue.Tag is TechRecordObjectTag)
        {
          object obj2 = ((TechRecordObjectTag) dictionaryValue.Tag).Object;
          if (obj2 is TechProcCacheInfo techProcCacheInfo)
            result = techProcCacheInfo.ObjTypeId;
          else
            int.TryParse(obj2.ToString(), out result);
        }
        if (this.IsCloneRecord(recBase))
        {
          TechRelParam techRelParam = this.AddRelationByObject(ImportingCategory.TechProcessPump, (object) num1, relTechRelationId, recBase, ipsObjId, result, objectType);
          if (techRelParam != null)
            techRelList.Add(techRelParam);
        }
        else
        {
          long newObjectId = dictionaryValue != null ? dictionaryValue.NewObjectID : 0L;
          if (newObjectId != 0L)
          {
            TechRelParam techRelParam = new TechRelParam(newObjectId, ipsObjId, relTechRelationId, result, objectType);
            techRelList.Add(techRelParam);
          }
        }
      }
    }
    return techRelList;
  }

  public override void Exam() => base.Exam();

  public override void Pump() => base.Pump();

  protected override Guid GUID => this._guid;
}
