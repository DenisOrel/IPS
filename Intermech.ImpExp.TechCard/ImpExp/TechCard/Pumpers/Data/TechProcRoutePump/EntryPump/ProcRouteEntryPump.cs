// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.EntryPump.ProcRouteEntryPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Common.TechCardSettings;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.Interfaces;
using Intermech.Interfaces.TechCard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.EntryPump;

[TaskDescription("Инициализация данных для перекачки - входимостей в маршрут обработки", "Перекачка данных - Входимости в маршрут обработки")]
internal class ProcRouteEntryPump(PluginClass plugin) : TechPumpBase(plugin)
{
  private Dictionary<EntryInfo, EntryContent> _fwdCache = new Dictionary<EntryInfo, EntryContent>();
  private Dictionary<EntryContent, ProcRouteInfo> _bkwdCache = new Dictionary<EntryContent, ProcRouteInfo>();
  private ISet<long> _zaksIds = (ISet<long>) new HashSet<long>();
  protected int _otArticleBaseTypeID = -1;
  protected IAttributeTypeItem _atMemberOfSborkaObjectAttr;
  protected IAttributeTypeItem _atMemberOfZakazObjectAttr;
  protected IAttributeTypeItem _atProcRouteDefaultAttr;
  private int _articleTypeId = -1;

  private (EntryInfoEx zakData, Guid routeProcID) FindFullEntryInfoInCache(EntryInfo entryInfo)
  {
    EntryContent key;
    if (!this._fwdCache.TryGetValue(entryInfo, out key))
      return ((EntryInfoEx) null, Guid.Empty);
    ProcRouteInfo procRouteInfo;
    if (!this._bkwdCache.TryGetValue(key, out procRouteInfo))
      return ((EntryInfoEx) null, Guid.Empty);
    EntryInfoEx entryInfoEx;
    return !procRouteInfo.ZakInfo.TryGetValue(entryInfo.ZakArtTcKey, out entryInfoEx) ? ((EntryInfoEx) null, Guid.Empty) : (entryInfoEx, procRouteInfo.RouteProcId);
  }

  private void UpdateRouteProcIdInCache(EntryInfo entryInfo, Guid savedRouteProcId)
  {
    EntryContent key;
    ProcRouteInfo procRouteInfo;
    if (!this._fwdCache.TryGetValue(entryInfo, out key) || !this._bkwdCache.TryGetValue(key, out procRouteInfo))
      return;
    procRouteInfo.RouteProcId = savedRouteProcId;
  }

  private void FillZaksCache()
  {
    using (IDataReader dataReader = this.GetDataReader("SELECT a.ART_ID FROM ARTICLES a WHERE " + $"a.{"SECTION_ID"} = {99999999}", ConnStrType.Search))
    {
      int tableColumn = this.GetTableColumns(dataReader)["ART_ID"];
      while (dataReader.Read())
      {
        int int32Value = DataSetProcessor.GetInt32Value(dataReader[tableColumn], 0);
        if (int32Value != 0)
          this._zaksIds.Add((long) int32Value);
      }
    }
  }

  private void FillFwdCache()
  {
    using (IDataReader customDataReader = this.GetCustomDataReader($" SELECT a.* from {"TC_OBJ2LINK"} a order by a.{"F_KEY"}"))
    {
      Dictionary<string, int> tableColumns = this.GetTableColumns(customDataReader);
      int i1 = tableColumns["F_ART_TCKEY"];
      int i2 = tableColumns["F_ZAK_TCKEY"];
      int i3 = tableColumns["F_PROJ_TCKEY"];
      int i4 = tableColumns["F_ART_ID"];
      int i5 = tableColumns["F_ZAK_ID"];
      int i6 = tableColumns["F_PROJ_ID"];
      int i7 = tableColumns["F_OBJ_KEY"];
      int i8 = tableColumns["F_OBJ_TYPE"];
      while (customDataReader.Read())
      {
        int int32Value1 = DataSetProcessor.GetInt32Value(customDataReader[i5], 0);
        if (int32Value1 <= 0 || this._zaksIds.Contains((long) int32Value1))
        {
          int int32_1 = BasePumpHelper.ToInt32(customDataReader[i1]);
          int int32_2 = BasePumpHelper.ToInt32(customDataReader[i3]);
          int int32_3 = BasePumpHelper.ToInt32(customDataReader[i2]);
          int int32Value2 = DataSetProcessor.GetInt32Value(customDataReader[i4], 0);
          int int32Value3 = DataSetProcessor.GetInt32Value(customDataReader[i6], 0);
          EntryInfo key = new EntryInfo(int32_1, int32Value2, int32_2, int32Value3, int32_3, int32Value1);
          EntryContent entryContent;
          if (!this._fwdCache.TryGetValue(key, out entryContent))
          {
            entryContent = new EntryContent();
            this._fwdCache.Add(key, entryContent);
          }
          LinkedObjDescr linkedObjDescr = new LinkedObjDescr(BasePumpHelper.ToInt32(customDataReader[i7]), (LinkedObjectType) BasePumpHelper.ToInt32(customDataReader[i8]));
          if (!entryContent.Content.Contains(linkedObjDescr))
            entryContent.Content.Add(linkedObjDescr);
        }
      }
    }
  }

  private void FillBkwdCache()
  {
    foreach (KeyValuePair<EntryInfo, EntryContent> keyValuePair in this._fwdCache)
    {
      ProcRouteInfo procRouteInfo;
      if (!this._bkwdCache.TryGetValue(keyValuePair.Value, out procRouteInfo))
      {
        procRouteInfo = new ProcRouteInfo();
        this._bkwdCache.Add(keyValuePair.Value, procRouteInfo);
      }
      EntryInfo key = keyValuePair.Key;
      if (key.SbArtTcKey > 0)
      {
        IDictionary<int, EntryInfoEx> zakInfo1 = procRouteInfo.ZakInfo;
        key = keyValuePair.Key;
        int zakArtTcKey1 = key.ZakArtTcKey;
        EntryInfoEx entryInfoEx1;
        ref EntryInfoEx local = ref entryInfoEx1;
        if (!zakInfo1.TryGetValue(zakArtTcKey1, out local))
        {
          key = keyValuePair.Key;
          entryInfoEx1 = new EntryInfoEx(key.ZakArtTcKey);
          IDictionary<int, EntryInfoEx> zakInfo2 = procRouteInfo.ZakInfo;
          key = keyValuePair.Key;
          int zakArtTcKey2 = key.ZakArtTcKey;
          EntryInfoEx entryInfoEx2 = entryInfoEx1;
          zakInfo2.Add(zakArtTcKey2, entryInfoEx2);
        }
        ISet<int> sbs1 = entryInfoEx1.Sbs;
        key = keyValuePair.Key;
        int sbArtTcKey1 = key.SbArtTcKey;
        if (!sbs1.Contains(sbArtTcKey1))
        {
          ISet<int> sbs2 = entryInfoEx1.Sbs;
          key = keyValuePair.Key;
          int sbArtTcKey2 = key.SbArtTcKey;
          sbs2.Add(sbArtTcKey2);
        }
      }
    }
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
      IAttributeTypeItem byGuid1 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atMemberOfSborkaObjectAttrGUID);
      if (byGuid1 != null)
        this._atMemberOfSborkaObjectAttr = byGuid1;
      IAttributeTypeItem byGuid2 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atMemberOfZakazObjectAttrGUID);
      if (byGuid2 != null)
        this._atMemberOfZakazObjectAttr = byGuid2;
      IAttributeTypeItem byGuid3 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atProcRouteDefaultAttrGuid);
      if (byGuid3 != null)
        this._atProcRouteDefaultAttr = byGuid3;
      IAttributeTypeItem byGuid4 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atLastLevelSeek);
      if (byGuid4 != null)
        this._atLastLevelSeek = byGuid4;
      IObjectTypeItem byGuid5 = imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otProductObjTypeGuid);
      if (byGuid5 != null)
        this._otArticleBaseTypeID = byGuid5.ID;
      base.LoadMetaData4Pump();
    }
  }

  protected override Guid GUID { get; } = new Guid("{3C9A17D1-0B1B-4765-836D-E0798CF13E0F}");

  protected override TechDataSource GetDataSource()
  {
    if (this._dataSource == null)
    {
      EntryDataBuilder<TechPumpBase> dataBuilder = new EntryDataBuilder<TechPumpBase>((TechPumpBase) this);
      dataBuilder.PumpModeCondFunc = (Func<string, string, string>) ((condField, dopType) =>
      {
        if (TechSettingsHelper.PumpDataType == TechPumpDataType.None)
          return " 1 = 2 ";
        string pumpModeCond = TechDataBuilder<PumpClass>.GetPumpModeCond(condField, Convert.ToInt32((object) Intermech.ImpExp.TechCard.Common.DataManager.DataManager.ObjDataType.odtArtKey));
        string str = TechSettingsHelper.PumpMode == TechPumpMode.tpmProdZakList ? TechDataBuilder<PumpClass>.GetPumpModeCond("F_ZAK_TCKEY", Convert.ToInt32((object) Intermech.ImpExp.TechCard.Common.DataManager.DataManager.ObjDataType.odtProdZakKey)) : string.Empty;
        if (str.IsEmpty())
          return pumpModeCond;
        return $"({pumpModeCond} and {str})";
      });
      this._dataSource = new TechDataSource((ITechDataBuilder) dataBuilder);
    }
    return this._dataSource;
  }

  protected override void PumpLoadData()
  {
    this.FillZaksCache();
    this.FillFwdCache();
    this.FillBkwdCache();
    base.PumpLoadData();
  }

  protected override ImportingCategory GetTechCategory()
  {
    return ImportingCategory.TechManufacturingRoutingEntry;
  }

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new ImportingCategory[0];
  }

  protected override ImportingCategory[] GetCategoriesByNeed2FillTechObject()
  {
    return new ImportingCategory[2]
    {
      ImportingCategory.Articles,
      ImportingCategory.BaseTechObjectsVersionsCache
    };
  }

  protected override void InitData()
  {
    this.objTypeID = -1;
    if (this.plugin.Imdi.ObjectTypes.ExistsByGuid(TechcardConsts.TypeConsts.otProcRoutingEntryGUID))
      this.objTypeID = this.plugin.Imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otProcRoutingEntryGUID).ID;
    this._tableName = "TC_OBJ2LINK";
    this._recType = "Входимость в маршрут обработки";
    this._articleTypeId = this.plugin.Imdi.ObjectTypes.GetByGuid(TechCardConsts.ObjectTypes.ArticleBaseGUID).ID;
  }

  public override bool CheckObjTypeOrParamType(string entCode, Guid attrGuid) => false;

  public override bool CheckObjLinkOrParamType(string entCode, Guid attrGuid) => false;

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (!(record is ProcRouteEntryObject record1) || objRecord == null || record1.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record1.RecMode != TechObjectRecord.PumpMode.ObjectOnly)
      return;
    (EntryInfoEx zakData, Guid routeProcID) entryInfoInCache = this.FindFullEntryInfoInCache(record1.EntryInfo);
    if (entryInfoInCache.zakData == null)
      return;
    if (this._atMemberOfZakazObjectAttr != null)
    {
      (long ObjId, long ObjVerId, string Caption) articleInfoByKey = this.GetArticleInfoByKey(entryInfoInCache.zakData.ZakKey);
      if (articleInfoByKey.ObjVerId != 0L)
        this._techParmList.AddAttribute(this._atMemberOfZakazObjectAttr, (object) articleInfoByKey.ObjVerId, articleInfoByKey.Caption);
    }
    if (this._atMemberOfSborkaObjectAttr != null)
    {
      int num = 0;
      foreach (int sb in (IEnumerable<int>) entryInfoInCache.zakData.Sbs)
      {
        (long ObjId, long ObjVerId, string Caption) articleInfoByKey = this.GetArticleInfoByKey(sb);
        if (articleInfoByKey.ObjVerId != 0L && this._techParmList.AddAttribute(this._atMemberOfSborkaObjectAttr, (object) articleInfoByKey.ObjVerId, articleInfoByKey.Caption) is TechParamAttribute techParamAttribute)
        {
          techParamAttribute.Index = num;
          ++num;
        }
      }
    }
    if (this._atLastLevelSeek != null)
      this._techParmList.AddAttribute(this._atLastLevelSeek, (object) (long) this.GetFirstStepLifecycle());
    base.FillTechObject(objRecord, (TechObjectRecord) record1);
  }

  protected override void CheckBaseRecords()
  {
  }

  protected override string GetRecordPumpMode(TechObjectRecord record)
  {
    if (!(record is ProcRouteEntryObject routeEntryObject))
    {
      record.RecMode = TechObjectRecord.PumpMode.NotPump;
      return string.Empty;
    }
    if (EntryHelper.IsDefaultRouteEntry((TechObjectRecordBase) routeEntryObject))
    {
      routeEntryObject.RecMode = TechObjectRecord.PumpMode.NotPump;
      return string.Empty;
    }
    if (TechSettingsHelper.PumpLinksOnlyWithActual && this._import_data_main.GetValue(ImportingCategory.BaseTechObjectsVersionsCache, (object) TechPumpBase.GenBaseTechObjectsVersionsCacheKey(routeEntryObject.LinkedObj.ObjKeyKey, routeEntryObject.LinkedObj.ObjType)) == null)
    {
      routeEntryObject.RecMode = TechObjectRecord.PumpMode.NotPump;
      return string.Empty;
    }
    string uniqueRecordKey = EntryHelper.GenerateUniqueRecordKey((TechObjectRecordBase) routeEntryObject);
    if (this._import_data_main.GetValue(this.GetTechCategory(), (object) uniqueRecordKey)?.Tag is ProcRoutesTag tag)
    {
      foreach (Guid key in tag.Entries.Keys)
      {
        if (!(key == Guid.Empty))
        {
          this.UpdateRouteProcIdInCache(routeEntryObject.EntryInfo, key);
          routeEntryObject.RecMode = TechObjectRecord.PumpMode.NotPump;
          return string.Empty;
        }
      }
    }
    (EntryInfoEx zakData, Guid routeProcID) entryInfoInCache = this.FindFullEntryInfoInCache(routeEntryObject.EntryInfo);
    if (entryInfoInCache.zakData == null)
    {
      routeEntryObject.RecMode = TechObjectRecord.PumpMode.NotPump;
      return string.Empty;
    }
    if (entryInfoInCache.zakData.Processed)
    {
      routeEntryObject.RecMode = TechObjectRecord.PumpMode.NotPump;
      return string.Empty;
    }
    entryInfoInCache.zakData.Processed = true;
    routeEntryObject.RecMode = TechObjectRecord.PumpMode.ObjectAndLinks;
    return string.Empty;
  }

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new ProcRouteEntryObject();
  }

  protected override void AddValue2Cache(
    object oldKey,
    long newKey,
    TechObjectRecordBase recBase,
    TechParamList recParmList)
  {
    if (!(recBase is ProcRouteEntryObject routeEntryObject))
      return;
    (EntryInfoEx zakData, Guid guid) = this.FindFullEntryInfoInCache(routeEntryObject.EntryInfo);
    if (zakData == null)
      return;
    string uniqueRecordKey = EntryHelper.GenerateUniqueRecordKey(recBase);
    if (this._import_data_main.GetTag(this.GetTechCategory(), (object) uniqueRecordKey) is ProcRoutesTag _)
      return;
    (long ObjId, long ObjVerId, string Caption) articleInfoByKey = this.GetArticleInfoByKey(routeEntryObject.EntryInfo.ArtTcKey);
    if (articleInfoByKey.ObjVerId == -1L)
      return;
    ProcRoutesTag tag = new ProcRoutesTag();
    ProcRouteEntryTag procRouteEntryTag = new ProcRouteEntryTag(guid)
    {
      IpsOwnerObjId = articleInfoByKey.ObjVerId,
      IpsOwnerObjTypeId = this._articleTypeId
    };
    tag.Entries.Add(procRouteEntryTag.ProcRouteId, procRouteEntryTag);
    procRouteEntryTag.Entries.Add(newKey);
    this._import_data_main.AddValue(this.GetTechCategory(), (object) uniqueRecordKey, newKey, (ITagImportObject) tag);
  }

  public override void Exam()
  {
    bool flag;
    using (IDataReader customDataReader = this.GetCustomDataReader(string.Format("SELECT \r\n                                   {0}, {1}, {2}, {3}, {4}\r\n                                FROM   \r\n                                   {5}\r\n                                GROUP BY \r\n                                   {0}, {1}, {2}, {3}, {4}\r\n                                HAVING COUNT (*) > 1 ", (object) "F_OBJ_KEY", (object) "F_OBJ_TYPE", (object) "F_ART_TCKEY", (object) "F_PROJ_TCKEY", (object) "F_ZAK_TCKEY", (object) "TC_OBJ2LINK")))
      flag = customDataReader.Read();
    if (flag && MessageBox.Show($"В базе Imbase обнаружены не уникальные привязки к изделиям!{Environment.NewLine}Рекомендуется перед импортом запустить в TechCFG команду 'Администрирование'-'Корректировка привязок к изделиям'. Прервать импорт ?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
      Application.Exit();
    base.Exam();
  }

  protected override void AnalyzeStoppedData()
  {
  }

  protected override void PumpDiffRec(TechObjectRecord record)
  {
  }
}
