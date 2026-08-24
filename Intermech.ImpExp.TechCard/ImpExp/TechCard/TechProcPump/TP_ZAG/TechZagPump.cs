// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.TP_ZAG.TechZagPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Common.ArcArtPump;
using Intermech.ImpExp.TechCard.Pumpers;
using Intermech.ImpExp.TechCard.Pumpers.Common.DataCache;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_ZAG;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.Interfaces;
using Intermech.Interfaces.TechCard;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.TP_ZAG;

[TaskDescription("Инициализация данных для перекачки - Заготовки", "Перекачка данных - Заготовки")]
internal class TechZagPump(PluginClass plugin) : TechPumpBase(plugin)
{
  private Dictionary<int, List<int>> _parent2KeyList = new Dictionary<int, List<int>>();
  private Dictionary<string, long> _vidZagotCache = new Dictionary<string, long>();
  private Dictionary<int, List<int>> _art2RecordsCache = new Dictionary<int, List<int>>();
  private Guid _guid = new Guid("{6711F47B-FF0D-4313-84D2-CC422380E4C9}");
  protected int _otZagotGroupTypeID = -1;
  private IAttributeTypeItem _atZSearchObjectAttr;
  private IAttributeTypeItem _atGtpContextAttr;

  private void ReadParentObjectKeys()
  {
    this._parent2KeyList.Clear();
    try
    {
      IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand();
      command.CommandText = string.Format("SELECT \r\n                                                    {0},\r\n                                                    {2}     \r\n                                                  FROM \r\n                                                    {1} \r\n                                                  WHERE \r\n                                                    {0} <> 0 \r\n                                                  ORDER BY {0},{2}", (object) "F_PARENTKEY", (object) "TP_ZAG", (object) "F_KEY");
      using (IDataReader dataReader = command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
      {
        int ordinal1 = dataReader.GetOrdinal("F_PARENTKEY");
        int ordinal2 = dataReader.GetOrdinal("F_KEY");
        int num = 0;
        List<int> intList = (List<int>) null;
        while (dataReader.Read())
        {
          int int32_1 = dataReader.IsDBNull(ordinal1) ? 0 : BasePumpHelper.ToInt32(dataReader[ordinal1]);
          int int32_2 = dataReader.IsDBNull(ordinal2) ? 0 : BasePumpHelper.ToInt32(dataReader[ordinal2]);
          if (int32_1 != 0)
          {
            if (num != int32_1)
            {
              intList = new List<int>() { int32_1 };
              this._parent2KeyList.Add(int32_1, intList);
              num = int32_1;
            }
            if (int32_1 != int32_2 && intList != null)
              intList.Add(int32_2);
          }
        }
        dataReader.Close();
      }
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage("Невозможно получить перечень родительских идентификаторов версий заготовок: " + ex.Message);
      if (!(ex is OutOfMemoryException))
        return;
      throw;
    }
  }

  private int GetRecordVarNumber(int artKey, int recordKey)
  {
    List<int> intList;
    if (!this._art2RecordsCache.TryGetValue(artKey, out intList))
    {
      intList = new List<int>();
      this._art2RecordsCache.Add(artKey, intList);
    }
    int num1 = intList.IndexOf(recordKey);
    if (num1 == -1)
    {
      num1 = intList.Count;
      intList.Add(recordKey);
    }
    int num2;
    return num2 = num1 + 1;
  }

  private int GetRecordVersionNo(int parentKey, int recordKey, out int prevRecordKey)
  {
    int recordVersionNo1 = -1;
    prevRecordKey = -1;
    List<int> intList;
    if (!this._parent2KeyList.TryGetValue(parentKey, out intList))
      return recordVersionNo1;
    int recordVersionNo2 = intList.IndexOf(recordKey);
    if (recordVersionNo2 == -1 || recordVersionNo2 <= 0)
      return recordVersionNo2;
    prevRecordKey = intList[recordVersionNo2 - 1];
    return recordVersionNo2;
  }

  private EntityTypeRec GetEntityTypeRec()
  {
    this.entTypeRec = TechPumpData.EntTypeList.GetRecByType(23);
    EntityTypeRec recByType = TechPumpData.EntTypeList.GetRecByType(17);
    if (recByType != null)
    {
      foreach (Entity entity in recByType.CodeList.Values)
        this.entTypeRec.AddEntity(entity);
      foreach (KeyValuePair<string, Dictionary<int, Entity>> dopType in recByType.DopTypeList)
      {
        string key = $"{"S"}_{dopType.Key}";
        if (this.entTypeRec.DopTypeList.ContainsKey(key))
          this.entTypeRec.DopTypeList[key] = dopType.Value;
        else
          this.entTypeRec.DopTypeList.Add(key, dopType.Value);
      }
    }
    return this.entTypeRec;
  }

  protected override void InitData()
  {
    this._sortFieldName = "F_ORDER";
    this._recType = "Z";
    this._recTypeID = 23;
    this._tableName = "TP_ZAG";
    this._dopTypes.Add("I");
    this._dopTypes.Add("F");
    this._dopTypes.Add("S");
    this._dopTypes.Add("D");
    this._dopTypes.Add("S_I");
    this._dopTypes.Add("S_F");
    this._dopTypes.Add("S_S");
    this._dopTypes.Add("S_D");
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechZagot;

  protected virtual ImportingCategory GetTechParentCategory() => ImportingCategory.TechZagotParents;

  protected override ImportingCategory[] GetCategoriesByNeed2FillTechObject()
  {
    return new ImportingCategory[9]
    {
      ImportingCategory.Articles,
      ImportingCategory.ImbaseFolders,
      ImportingCategory.TechCeh,
      ImportingCategory.ImbaseTableLinksKeyToObjectID,
      ImportingCategory.Users,
      ImportingCategory.TechVidZagPump,
      ImportingCategory.TechVidIzdPump,
      ImportingCategory.BaseTechObjectsVersionsCache,
      this.GetTechParentCategory()
    };
  }

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new ImportingCategory[1]
    {
      ImportingCategory.TechZagotGroupUniquePump
    };
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
      IObjectTypeItem byGuid1 = imdi.ObjectTypes.GetByGuid(TechCardConsts.ObjectTypes.ZagotGroupGUID);
      if (byGuid1 != null)
        this._otZagotGroupTypeID = byGuid1.ID;
      IAttributeTypeItem byGuid2 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atZSearchObjectAttrGuid);
      if (byGuid2 != null)
        this._atZSearchObjectAttr = byGuid2;
      IAttributeTypeItem byGuid3 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atGtpContextAttrGUID);
      if (byGuid3 != null)
        this._atGtpContextAttr = byGuid3;
      IImportingData cache = ImportingCategoryDataCache.Instance.GetCache(new ImportingCategory[1]
      {
        ImportingCategory.TechVidZagPump
      });
      try
      {
        foreach (DictionaryValue dictionaryValue in cache.GetCategory(ImportingCategory.TechVidZagPump).Values)
        {
          long num;
          if (!this._vidZagotCache.TryGetValue(dictionaryValue.Caption, out num))
            this._vidZagotCache.Add(dictionaryValue.Caption, dictionaryValue.NewObjectID);
          else if (dictionaryValue.NewObjectID < num)
            this._vidZagotCache[dictionaryValue.Caption] = dictionaryValue.NewObjectID;
        }
      }
      finally
      {
        ImportingCategoryDataCache.Instance.FreeCache(new ImportingCategory[1]
        {
          ImportingCategory.TechVidZagPump
        });
      }
      base.LoadMetaData4Pump();
    }
  }

  protected override void LoadMetaData4StoppedPump() => this.entTypeRec = this.GetEntityTypeRec();

  protected override TechDataSource GetDataSource()
  {
    if (this._dataSource == null)
    {
      TechZagDataBuilder<TechPumpBase> dataBuilder = new TechZagDataBuilder<TechPumpBase>((TechPumpBase) this);
      dataBuilder.PumpModeCondFunc = (Func<string, string, string>) ((condField, dopType) => TechDataBuilder<PumpClass>.GetPumpModeCond(condField, 3));
      this._dataSource = new TechDataSource((ITechDataBuilder) dataBuilder);
    }
    return this._dataSource;
  }

  protected override void CheckBaseRecords()
  {
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (this._impObjList == null)
      return;
    int key = record.Key;
    int int32_1 = Convert.ToInt32(record.Fields["F_PARENTKEY"]);
    int newKey = Convert.ToInt32(record.Fields["F_VERSION"]);
    int int32_2 = Convert.ToInt32(record.Fields["F_STATUS"]);
    string str1 = Convert.ToString(record.Fields["F_DESCR"]);
    Convert.ToInt32(record.Fields["F_CTLKEY"]);
    int int32_3 = Convert.ToInt32(record.Fields["F_ZAGARTKEY"]);
    int int32_4 = Convert.ToInt32(record.Fields["F_USER_CREATOR"]);
    long int32_5 = (long) Convert.ToInt32(record.Fields["F_GROUPZAG_KEY"]);
    objRecord.ObjectVerType = 0;
    string caption = str1;
    if (caption == string.Empty)
    {
      int int32_6 = Convert.ToInt32(record.Fields["F_ARTTCKEY"]);
      this.GetArticleInfoByKey(int32_6);
      ArcArtsObject arcArtsObject;
      if (TechPumpData.TechObjects.ArcArtList.TryGetValue((long) int32_6, out arcArtsObject))
      {
        int recordKey = int32_1 != 0 ? int32_1 : key;
        int recordVarNumber = this.GetRecordVarNumber(int32_6, recordKey);
        caption = $"{arcArtsObject.Designation} {recordVarNumber} ЗАГ";
      }
    }
    if (caption == string.Empty)
    {
      ITechParamBase entity = (ITechParamBase) this._techParmList.GetEntity("SORT");
      if (entity != null && entity.Value is string)
        caption = entity.Value.ToString();
    }
    if (caption == string.Empty)
    {
      ITechParamBase entity = (ITechParamBase) this._techParmList.GetEntity("Ммтр");
      if (entity != null && entity.Value is string)
        caption = entity.Value.ToString();
    }
    if (str1 == string.Empty && str1 != caption)
    {
      record.Fields["F_DESCR"] = (object) caption;
      this._techParmList.AddOrUpdateEntity("%ZDS", (object) caption, true, caption);
    }
    objRecord.Caption = caption.Truncate(Intermech.Consts.MaxStringSize - 2);
    if (this._parent2KeyList.ContainsKey(key))
    {
      if (this._import_data_main.GetValue(this.GetTechParentCategory(), (object) key) != null)
        this._import_data_main.ClearValue((int) this.GetTechParentCategory(), (object) key);
      this._import_data_main.AddValue(this.GetTechParentCategory(), (object) key, 0L, Convert.ToString(objRecord.IdGuid));
    }
    if (int32_1 != 0)
    {
      DictionaryValue dictionaryValue = this._import_data_main.GetValue(this.GetTechParentCategory(), (object) int32_1);
      if (dictionaryValue != null && !string.IsNullOrEmpty(dictionaryValue.Caption))
      {
        Guid guid = Guid.Empty;
        try
        {
          guid = new Guid(dictionaryValue.Caption);
        }
        catch
        {
        }
        if (!guid.Equals(Guid.Empty))
        {
          objRecord.Id = 0L;
          objRecord.IdGuid = (object) guid;
          objRecord.ObjectVerType = 1;
          int prevRecordKey;
          newKey = this.GetRecordVersionNo(int32_1, key, out prevRecordKey);
          objRecord.ParentVersionNo = newKey - 1;
          objRecord.ParentVersionId = this._import_data_main.GetNewKey(this.GetTechCategory(), (object) prevRecordKey);
        }
      }
    }
    switch (int32_2)
    {
      case -1:
        objRecord.ChkoutGuid = (object) null;
        objRecord.ChkoutBy = 0L;
        break;
    }
    if (int32_3 != 0 && this._atZSearchObjectAttr != null)
    {
      (long ObjId, long ObjVerId, string Caption) articleInfoByKey = this.GetArticleInfoByKey(int32_3);
      if (articleInfoByKey.ObjVerId != 0L)
        this._techParmList.AddAttribute(this._atZSearchObjectAttr, (object) articleInfoByKey.ObjVerId, articleInfoByKey.Caption);
    }
    objRecord.VersionId = newKey;
    DictionaryValue userInfoBySearchId = this.GetUserInfoBySearchId(int32_4);
    if (userInfoBySearchId != null)
    {
      objRecord.OwnerId = userInfoBySearchId.NewObjectID;
      if (userInfoBySearchId.Tag is UserTag tag)
        objRecord.OwnerGuid = (object) tag.Guid;
    }
    long num;
    if (this._techParmList.GetEntity("ПЗГ")?.Value is string str2 && this._vidZagotCache.TryGetValue(str2, out num))
    {
      if (TechCardPlugin.Configuration.ZagotLink2ImbasePumpMode)
      {
        ITechParamEntity entity = this._techParmList.GetEntity("КЗГ");
        ITechParamAttribute techParamAttribute = this._entityConverter.Convert((TechObjectRecordBase) record, this._techParmList, entity, this.GetEntityByCode(entity.Code));
        if (techParamAttribute != null)
          this._techParmList.AddOrUpdateEntity("ВЗГ", techParamAttribute.Value, true, string.IsNullOrEmpty(techParamAttribute.Caption) ? str2 : techParamAttribute.Caption);
      }
      else
        this._techParmList.AddOrUpdateEntity("ВЗГ", (object) num, true, str2);
    }
    ITechParamEntity entity1 = this._techParmList.GetEntity("ВИД");
    int result;
    if (entity1 != null && int.TryParse(Convert.ToString(entity1.Value), out result))
    {
      TechPumpData.Entities.EntitiesList["ВИД"].IsMasterAttr = false;
      if (this.entTypeRec != null && this.entTypeRec.CodeList.ContainsKey("ВИД"))
        this.entTypeRec.CodeList["ВИД"].IsMasterAttr = false;
      DictionaryValue dictionaryValue = this._import_data_main.GetValue(ImportingCategory.TechVidIzdPump, (object) result);
      if (dictionaryValue != null)
      {
        if (TechCardPlugin.Configuration.ZagotLink2ImbasePumpMode)
        {
          ITechParamAttribute techParamAttribute = this._entityConverter.Convert((TechObjectRecordBase) record, this._techParmList, entity1, this.GetEntityByCode(entity1.Code));
          if (techParamAttribute != null)
            this._techParmList.AddOrUpdateEntity("ВИД", techParamAttribute.Value, true, dictionaryValue.Caption);
        }
        else
          this._techParmList.AddOrUpdateEntity("ВИД", (object) dictionaryValue.NewObjectID, true, dictionaryValue.Caption);
      }
    }
    if (int32_5 != 0L && this._atGtpContextAttr != null)
      this._techParmList.AddAttribute(this._atGtpContextAttr, (object) true);
    base.FillTechObject(objRecord, record);
    objRecord.IsBaseVersion = int32_2 == 0;
    if (!objRecord.IsBaseVersion)
      return;
    long oldKey = TechPumpBase.GenBaseTechObjectsVersionsCacheKey(record.Key, LinkedObjectType.Zagot);
    if (this._import_data_main.GetValue(ImportingCategory.BaseTechObjectsVersionsCache, (object) oldKey) != null)
      return;
    this._import_data_main.AddValue(ImportingCategory.BaseTechObjectsVersionsCache, (object) oldKey, (long) newKey);
  }

  protected override TechObjectRecord GetTpObjRec() => (TechObjectRecord) new TechZagRecord();

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    List<TechRelParam> techRelList = new List<TechRelParam>();
    long int32 = (long) Convert.ToInt32(recBase.Fields["F_GROUPZAG_KEY"]);
    if (int32 == 0L)
      return techRelList;
    int techGtpRelationId = this._relTechGTPRelationID;
    long newKey = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechZagotGroupUniquePump, (object) int32);
    if (newKey != 0L)
    {
      TechRelParam techRelParam = new TechRelParam(newKey, ipsObjId, techGtpRelationId, this._otZagotGroupTypeID, this.objTypeID);
      techRelList.Add(techRelParam);
    }
    return techRelList;
  }

  public override void Exam()
  {
    this.entTypeRec = this.GetEntityTypeRec();
    base.Exam();
  }

  protected override void ExamSubData(string dopType)
  {
    switch (dopType)
    {
      case "S_I":
        break;
      case "S_F":
        break;
      case "S_S":
        break;
      case "S_D":
        break;
      default:
        base.ExamSubData(dopType);
        break;
    }
  }

  public override void Pump()
  {
    if (TechCardPlugin.Configuration.ZagotLink2ImbasePumpMode)
      this.plugin.appManager.AddWarningMessage("Включен режим привязки заготовки к справочникам Imbase!");
    this.ReadParentObjectKeys();
    base.Pump();
  }

  protected override void ReleasePumpData()
  {
    base.ReleasePumpData();
    this._parent2KeyList.Clear();
    this._parent2KeyList = (Dictionary<int, List<int>>) null;
    this._vidZagotCache.Clear();
    this._vidZagotCache = (Dictionary<string, long>) null;
    this._art2RecordsCache.Clear();
    this._art2RecordsCache = (Dictionary<int, List<int>>) null;
  }

  protected override Guid GUID => this._guid;
}
