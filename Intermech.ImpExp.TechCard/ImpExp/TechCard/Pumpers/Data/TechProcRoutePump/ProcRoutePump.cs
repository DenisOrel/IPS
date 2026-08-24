// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.ProcRoutePump
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
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.EntryPump;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.Interfaces;
using Intermech.Interfaces.TechCard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump;

[TaskDescription("Инициализация данных для перекачки - Маршрут обработки", "Перекачка данных - Маршрут обработки")]
internal class ProcRoutePump(PluginClass plugin) : TechPumpBase(plugin)
{
  private IDictionary<string, ProcRoutesTag> _procRouteCache = (IDictionary<string, ProcRoutesTag>) new Dictionary<string, ProcRoutesTag>();
  private int _articleTypeId = -1;
  protected Guid _guid = new Guid("{F6EF8537-014D-45e2-B9DC-295830A01338}");
  protected IAttributeTypeItem _atProcRouteDefaultAttr;

  private void LoadRoutesCacheData()
  {
    this._procRouteCache.Clear();
    foreach (KeyValuePair<object, DictionaryValue> keyValuePair in this._import_data_main.GetCategory(ImportingCategory.TechManufacturingRoutingEntry))
    {
      if (keyValuePair.Value.Tag is ProcRoutesTag tag1)
      {
        this._procRouteCache.Add(keyValuePair.Key.ToString(), tag1);
        foreach (ProcRouteEntryTag procRouteEntryTag in tag1.Entries.Values)
        {
          string oldKey = $"{keyValuePair.Key}_{procRouteEntryTag.ProcRouteId}";
          if (this._import_data_main.GetTag(this.GetTechCategory(), (object) oldKey) is ProcRouteEntryTag tag)
            procRouteEntryTag.IpsProcRouteId = tag.IpsProcRouteId;
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
      IAttributeTypeItem byGuid1 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atProcRouteDefaultAttrGuid);
      if (byGuid1 != null)
        this._atProcRouteDefaultAttr = byGuid1;
      IAttributeTypeItem byGuid2 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atLastLevelSeek);
      if (byGuid2 != null)
        this._atLastLevelSeek = byGuid2;
      base.LoadMetaData4Pump();
    }
  }

  protected override Guid GUID => this._guid;

  protected override TechDataSource GetDataSource()
  {
    if (this._dataSource == null)
    {
      ProcRouteDataBuilder<TechPumpBase> dataBuilder = new ProcRouteDataBuilder<TechPumpBase>((TechPumpBase) this);
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

  protected override ImportingCategory GetTechCategory()
  {
    return ImportingCategory.TechManufacturingRouting;
  }

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new ImportingCategory[1]
    {
      ImportingCategory.Articles
    };
  }

  protected override ImportingCategory[] GetCategoriesByNeed2FillTechObject()
  {
    return new ImportingCategory[3]
    {
      ImportingCategory.Articles,
      ImportingCategory.TechManufacturingRoutingEntry,
      ImportingCategory.BaseTechObjectsVersionsCache
    };
  }

  protected override void InitData()
  {
    this.objTypeID = -1;
    if (this.plugin.Imdi.ObjectTypes.ExistsByGuid(TechcardConsts.TypeConsts.otTechRoute2AtrObjectTypeGuid))
      this.objTypeID = this.plugin.Imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otTechRoute2AtrObjectTypeGuid).ID;
    this._tableName = "TC_OBJ2LINK";
    this._recType = "Маршрут обработки";
    this._articleTypeId = this.plugin.Imdi.ObjectTypes.GetByGuid(TechCardConsts.ObjectTypes.ArticleBaseGUID).ID;
  }

  public override bool CheckObjTypeOrParamType(string entCode, Guid attrGuid) => false;

  public override bool CheckObjLinkOrParamType(string entCode, Guid attrGuid) => false;

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (objRecord == null || record == null || record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record.RecMode != TechObjectRecord.PumpMode.ObjectOnly || record.diff_ArtTcKey != 0)
      return;
    string str1 = Convert.ToString(record.Fields["F_NAME"]);
    string str2 = Convert.ToString(record.Fields["F_DESIGNATION"]);
    bool boolean = Convert.ToBoolean(record.Fields["F_ISDEFAULT"]);
    if (str1 != string.Empty && str2 != string.Empty)
      objRecord.Caption = $"{str2} ({str1})".Truncate(Intermech.Consts.MaxStringSize - 2);
    else if (str1 != string.Empty)
      objRecord.Caption = $"({str1})".Truncate(Intermech.Consts.MaxStringSize - 2);
    else if (str2 != string.Empty)
      objRecord.Caption = (str2 ?? "").Truncate(Intermech.Consts.MaxStringSize - 2);
    if (this._atProcRouteDefaultAttr != null)
    {
      string str3 = "";
      if (boolean)
        str3 = "По умолчанию";
      this._techParmList.AddAttribute(this._atProcRouteDefaultAttr, (object) str3);
    }
    if (this._atLastLevelSeek != null)
      this._techParmList.AddAttribute(this._atLastLevelSeek, (object) (long) this.GetFirstStepLifecycle());
    if (str1 != string.Empty && this._atNaimAttrType != null)
      this._techParmList.AddAttribute(this._atNaimAttrType, (object) str1);
    if (str2 != string.Empty && this._atObozAttrType != null)
      this._techParmList.AddAttribute(this._atObozAttrType, (object) str2);
    base.FillTechObject(objRecord, record);
  }

  protected override void ReleasePumpData()
  {
    this._procRouteCache.Clear();
    this._procRouteCache = (IDictionary<string, ProcRoutesTag>) null;
    base.ReleasePumpData();
  }

  protected override void CheckBaseRecords()
  {
  }

  protected override string GetRecordPumpMode(TechObjectRecord record)
  {
    if (!(record is ProcRouteObject procRouteObject))
      return string.Empty;
    if (TechSettingsHelper.PumpLinksOnlyWithActual && this._import_data_main.GetValue(ImportingCategory.BaseTechObjectsVersionsCache, (object) TechPumpBase.GenBaseTechObjectsVersionsCacheKey(procRouteObject.LinkedObj.ObjKeyKey, procRouteObject.LinkedObj.ObjType)) == null)
    {
      record.RecMode = TechObjectRecord.PumpMode.NotPump;
      return string.Empty;
    }
    string tcEntryKey = EntryHelper.GenerateTcEntryKey((TechObjectRecordBase) procRouteObject);
    if (EntryHelper.IsDefaultRouteEntry((TechObjectRecordBase) procRouteObject))
    {
      if (this._procRouteCache.ContainsKey(tcEntryKey))
      {
        procRouteObject.RecMode = TechObjectRecord.PumpMode.NotPump;
        return string.Empty;
      }
      Guid guid = Guid.NewGuid();
      ProcRoutesTag procRoutesTag = new ProcRoutesTag();
      ProcRouteEntryTag procRouteEntryTag = new ProcRouteEntryTag(guid);
      (_, procRouteEntryTag.IpsOwnerObjId, _) = this.GetArticleInfoByKey(procRouteObject.EntryInfo.ArtTcKey);
      procRouteEntryTag.IpsOwnerObjTypeId = this._articleTypeId;
      procRoutesTag.Entries.Add(guid, procRouteEntryTag);
      this._procRouteCache.Add(tcEntryKey, procRoutesTag);
      procRouteObject.RecMode = TechObjectRecord.PumpMode.ObjectAndLinks;
      return string.Empty;
    }
    ProcRoutesTag procRoutesTag1;
    if (!this._procRouteCache.TryGetValue(tcEntryKey, out procRoutesTag1) || procRoutesTag1.Processed)
    {
      procRouteObject.RecMode = TechObjectRecord.PumpMode.NotPump;
      return string.Empty;
    }
    if (procRoutesTag1.Entries.Values.Any<ProcRouteEntryTag>((Func<ProcRouteEntryTag, int, bool>) ((procRoute, idx) => procRoute.IpsProcRouteId == 0L)))
    {
      procRouteObject.RecMode = TechObjectRecord.PumpMode.ObjectAndLinks;
      procRoutesTag1.Processed = true;
      return string.Empty;
    }
    procRouteObject.RecMode = TechObjectRecord.PumpMode.NotPump;
    return string.Empty;
  }

  protected override TechObjectRecord GetTpObjRec() => (TechObjectRecord) new ProcRouteObject();

  protected override void AddValue2Cache(
    object oldKey,
    long newKey,
    TechObjectRecordBase recBase,
    TechParamList recParmList)
  {
    ProcRoutesTag procRoutesTag;
    ProcRouteEntryTag tag;
    if (!(recBase is ProcRouteObject recBase1) || recBase1.RouteId == Guid.Empty || !this._procRouteCache.TryGetValue(EntryHelper.GenerateTcEntryKey((TechObjectRecordBase) recBase1), out procRoutesTag) || !procRoutesTag.Entries.TryGetValue(recBase1.RouteId, out tag))
      return;
    tag.IpsProcRouteId = newKey;
    string uniqueRecordKey = EntryHelper.GenerateUniqueRecordKey((TechObjectRecordBase) recBase1);
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

  public override void Pump() => base.Pump();

  protected override void AnalyzeStoppedData()
  {
  }

  protected override void PumpBaseRec(TechObjectRecord record)
  {
    ProcRoutesTag procRoutesTag;
    if (!(record is ProcRouteObject procRouteObject1) || !this._procRouteCache.TryGetValue(EntryHelper.GenerateTcEntryKey((TechObjectRecordBase) procRouteObject1), out procRoutesTag))
      return;
    foreach (ProcRouteEntryTag procRouteEntryTag in procRoutesTag.Entries.Values)
    {
      if (procRouteEntryTag.IpsProcRouteId == 0L)
      {
        ProcRouteObject procRouteObject2 = new ProcRouteObject();
        try
        {
          procRouteObject2.Assign((object) procRouteObject1);
          procRouteObject2.RouteId = procRouteEntryTag.ProcRouteId;
          this._techParmList.Clear();
          ObjectRecord techObject = this.CreateTechObject((TechObjectRecord) procRouteObject2);
          procRouteObject2.ParamList.Clear();
          if (this._techParmList.Count > 0)
            procRouteObject2.ParamList.AddRange((IEnumerable<ITechParamBase>) this._techParmList);
          this.tpObjRecList[(TechObjectRecordBase) procRouteObject2] = procRouteObject2.ParamList;
          this.FillObjectParams((TechObjectRecord) procRouteObject2, procRouteObject2.ParamList, techObject);
        }
        catch (Exception ex)
        {
          this.plugin.appManager.AddWarningMessage($"Ошибка обработки записи \"{procRouteObject1.Key}\" таблицы \"{procRouteObject1.TableName}\": {ex.Message}{Environment.NewLine + ex.StackTrace}");
          if (ex is OutOfMemoryException)
            throw;
          this.DoHandleImportObjectsException(ex);
        }
      }
    }
  }

  protected override void PumpDiffRec(TechObjectRecord record)
  {
  }

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    List<TechRelParam> result = new List<TechRelParam>();
    ProcRoutesTag procRoutesTag;
    ProcRouteEntryTag route;
    if (!(recBase is ProcRouteObject procRouteObject) || procRouteObject.RecMode == TechObjectRecord.PumpMode.ObjectOnly || procRouteObject.RecMode == TechObjectRecord.PumpMode.NotPump || !this._procRouteCache.TryGetValue(EntryHelper.GenerateTcEntryKey(recBase), out procRoutesTag) || !procRoutesTag.Entries.TryGetValue(procRouteObject.RouteId, out route))
      return result;
    if (route.IpsOwnerObjId != 0L)
    {
      TechRelParam techRelParam = new TechRelParam(route.IpsOwnerObjId, ipsObjId, this._relTechRelationID, route.IpsOwnerObjTypeId, this.objTypeID);
      result.Add(techRelParam);
      if (!TechPumpData.Configs.WorkWithArtVers)
        this.plugin.Imdi.UserSession.GetAllObjectVersionsList(route.IpsOwnerObjId, false, true, true)?.ForEach((Action<long>) (artObjVerId =>
        {
          if (route.IpsOwnerObjId == artObjVerId)
            return;
          result.Add(new TechRelParam(artObjVerId, ipsObjId, this._relTechRelationID, route.IpsOwnerObjTypeId, this.objTypeID));
        }));
    }
    else
      this.plugin.appManager.AddWarningMessage($"Родительский объект для записи Key = {recBase.Key} не найден!");
    if (!this.plugin.Imdi.ObjectTypes.ExistsByGuid(TechcardConsts.TypeConsts.otProcRoutingEntryGUID))
      return result;
    int id = this.plugin.Imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otProcRoutingEntryGUID).ID;
    foreach (long entry in (IEnumerable<long>) route.Entries)
    {
      TechRelParam techRelParam = new TechRelParam(ipsObjId, entry, this._relTechRelationID, this.objTypeID, id);
      result.Add(techRelParam);
    }
    return result;
  }

  protected override void PumpLoadData()
  {
    this.LoadRoutesCacheData();
    base.PumpLoadData();
  }
}
