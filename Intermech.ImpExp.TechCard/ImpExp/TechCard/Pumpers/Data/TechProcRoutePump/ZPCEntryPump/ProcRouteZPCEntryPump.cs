// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.ZPCEntryPump.ProcRouteZPCEntryPump
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
using Intermech.MRP2;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.ZPCEntryPump;

[TaskDescription("Инициализация данных для перекачки - входимостей в маршрут обработки для производственных заказов", "Перекачка данных - Входимости в маршрут обработки для производственных заказов")]
internal class ProcRouteZPCEntryPump(PluginClass plugin) : TechPumpBase(plugin)
{
  private Dictionary<EntryInfo, EntryContent> _TC_fwdCache = new Dictionary<EntryInfo, EntryContent>();
  private Dictionary<EntryInfo, ISet<ZPCEntryInfo>> _S4_fwdCache = new Dictionary<EntryInfo, ISet<ZPCEntryInfo>>();
  private Dictionary<EntryContent, ArtProdCopies> _bkwdCache = new Dictionary<EntryContent, ArtProdCopies>();
  private ISet<long> _zaks = (ISet<long>) new HashSet<long>();
  protected int _otArticleBaseTypeID = -1;
  protected IAttributeTypeItem _atMemberOfAssemblyCopyAttr;
  protected IAttributeTypeItem _atMemberOfExitAssemblyAttr;
  protected IAttributeTypeItem _atMemberOfProductionReportVersion;
  protected IAttributeTypeItem _atMemberOfProductionReportObject;
  protected IAttributeTypeItem _atNotes;
  private int _articlePCTypeId = -1;

  private ImportingCategory GetTempCategory()
  {
    return ImportingCategory.TechManufacturingRoutingEntryTemp;
  }

  private ArtProdCopies FindFullEntryInfoInCache(EntryInfo entryInfo)
  {
    EntryContent key;
    if (!this._TC_fwdCache.TryGetValue(entryInfo, out key))
      return (ArtProdCopies) null;
    ArtProdCopies artProdCopies;
    return !this._bkwdCache.TryGetValue(key, out artProdCopies) ? (ArtProdCopies) null : artProdCopies;
  }

  private void FillZaksCache()
  {
    using (IDataReader dataReader = this.GetDataReader("SELECT a.ART_ID FROM ARTICLES a WHERE " + $"a.{"SECTION_ID"} = {99999990}", ConnStrType.Search))
    {
      int tableColumn = this.GetTableColumns(dataReader)["ART_ID"];
      while (dataReader.Read())
      {
        int int32Value = DataSetProcessor.GetInt32Value(dataReader[tableColumn], 0);
        if (int32Value != 0)
          this._zaks.Add((long) int32Value);
      }
    }
  }

  private void FillFwdTCCache()
  {
    using (IDataReader dataReader = this.GetDataReader("SELECT a.* from TC_OBJ2LINK a  where a.F_ZAK_TCKEY>0 order by a.F_KEY"))
    {
      Dictionary<string, int> tableColumns = this.GetTableColumns(dataReader);
      int i1 = tableColumns["F_ART_TCKEY"];
      int i2 = tableColumns["F_ZAK_TCKEY"];
      int i3 = tableColumns["F_PROJ_TCKEY"];
      int i4 = tableColumns["F_ART_ID"];
      int i5 = tableColumns["F_ZAK_ID"];
      int i6 = tableColumns["F_PROJ_ID"];
      int i7 = tableColumns["F_OBJ_KEY"];
      int i8 = tableColumns["F_OBJ_TYPE"];
      while (dataReader.Read())
      {
        int int32Value1 = DataSetProcessor.GetInt32Value(dataReader[i5], 0);
        if (this._zaks.Contains((long) int32Value1))
        {
          int int32_1 = BasePumpHelper.ToInt32(dataReader[i1]);
          int int32_2 = BasePumpHelper.ToInt32(dataReader[i3]);
          int int32_3 = BasePumpHelper.ToInt32(dataReader[i2]);
          int int32Value2 = DataSetProcessor.GetInt32Value(dataReader[i4], 0);
          int int32Value3 = DataSetProcessor.GetInt32Value(dataReader[i6], 0);
          EntryInfo key = new EntryInfo(int32_1, int32Value2, int32_2, int32Value3, int32_3, int32Value1);
          EntryContent entryContent;
          if (!this._TC_fwdCache.TryGetValue(key, out entryContent))
          {
            entryContent = new EntryContent();
            this._TC_fwdCache.Add(key, entryContent);
          }
          LinkedObjDescr linkedObjDescr = new LinkedObjDescr(BasePumpHelper.ToInt32(dataReader[i7]), (LinkedObjectType) BasePumpHelper.ToInt32(dataReader[i8]));
          if (!entryContent.Content.Contains(linkedObjDescr))
            entryContent.Content.Add(linkedObjDescr);
        }
      }
    }
  }

  private void FillFwdS4Cache()
  {
    this.plugin.appManager.AddInfoMessage("Начало формирования прямого кэша входимостей");
    string key1 = "SB_PART_AID";
    string key2 = "SB_PRJLINK_ID";
    string key3 = "EXIT_SB_PRJLINK_ID";
    using (IDataReader dataReader = this.GetDataReader($"select z.PART_AID,z.PRJLINK_ID,z_sb.PART_AID {key1},z_sb.PRJLINK_ID {key2},z.ZAKAZ_ID,(SELECT max(zz.PRJLINK_ID)   FROM ZPC zz   WHERE zz.PARENT_ZREC_ID = 100     AND zz.ZAKAZ_ID = z.ZAKAZ_ID     AND z.Z_VER = zz.Z_VER     AND (z.ZREC_ID - zz.ZREC_ID > 0)     AND (z.PARENT_ZREC_ID >= ZZ.ZREC_ID)) {key3} FROM ZPC z   LEFT JOIN ZPC z_sb ON     z.ZAKAZ_ID = z_sb.ZAKAZ_ID     AND z.Z_VER = z_sb.Z_VER     AND z.PARENT_ZREC_ID = z_sb.ZREC_ID where    Z.OPCODE IN ({$"{0},"}{$"{1},"}{$"{3},"}{$"{4}"})", ConnStrType.Search))
    {
      Dictionary<string, int> tableColumns = this.GetTableColumns(dataReader);
      int i1 = tableColumns["PART_AID"];
      int i2 = tableColumns["PRJLINK_ID"];
      int i3 = tableColumns[key1];
      int i4 = tableColumns[key2];
      int i5 = tableColumns["ZAKAZ_ID"];
      int i6 = tableColumns[key3];
      while (dataReader.Read())
      {
        int int32Value1 = DataSetProcessor.GetInt32Value(dataReader[i5], 0);
        if (int32Value1 <= 0 || this._zaks.Contains((long) int32Value1))
        {
          int int32Value2 = DataSetProcessor.GetInt32Value(dataReader[i1], 0);
          int int32Value3 = DataSetProcessor.GetInt32Value(dataReader[i2], 0);
          int int32Value4 = DataSetProcessor.GetInt32Value(dataReader[i3], 0);
          int int32Value5 = DataSetProcessor.GetInt32Value(dataReader[i4], 0);
          int int32Value6 = DataSetProcessor.GetInt32Value(dataReader[i6], 0);
          EntryInfo key4 = new EntryInfo(0, int32Value2, 0, 0, 0, int32Value1);
          ISet<ZPCEntryInfo> zpcEntryInfoSet;
          if (!this._S4_fwdCache.TryGetValue(key4, out zpcEntryInfoSet))
          {
            zpcEntryInfoSet = (ISet<ZPCEntryInfo>) new HashSet<ZPCEntryInfo>();
            this._S4_fwdCache.Add(key4, zpcEntryInfoSet);
          }
          ZPCEntryInfo zpcEntryInfo = new ZPCEntryInfo(int32Value2, int32Value3, int32Value4, int32Value5, int32Value1, int32Value6);
          if (!zpcEntryInfoSet.Contains(zpcEntryInfo))
            zpcEntryInfoSet.Add(zpcEntryInfo);
        }
      }
    }
    this.plugin.appManager.AddInfoMessage("Окончание формирования прямого кэша входимостей");
  }

  private void FillBkwdCache()
  {
    foreach (KeyValuePair<EntryInfo, EntryContent> keyValuePair in this._TC_fwdCache)
    {
      ArtProdCopies artProdCopies;
      if (!this._bkwdCache.TryGetValue(keyValuePair.Value, out artProdCopies))
      {
        artProdCopies = new ArtProdCopies();
        this._bkwdCache.Add(keyValuePair.Value, artProdCopies);
      }
      ISet<ZPCEntryInfo> zpcEntryInfoSet;
      if (this._S4_fwdCache.TryGetValue(new EntryInfo(0, keyValuePair.Key.ArtId, 0, 0, 0, keyValuePair.Key.ZakArtId), out zpcEntryInfoSet))
      {
        foreach (ZPCEntryInfo zpcEntryInfo in (IEnumerable<ZPCEntryInfo>) zpcEntryInfoSet)
        {
          int artId1 = zpcEntryInfo.ArtId;
          EntryInfo key = keyValuePair.Key;
          int artId2 = key.ArtId;
          if (artId1 == artId2)
          {
            int sbId = zpcEntryInfo.SbId;
            key = keyValuePair.Key;
            int sbArtId1 = key.SbArtId;
            if (sbId == sbArtId1)
            {
              int zakId = zpcEntryInfo.ZakId;
              key = keyValuePair.Key;
              int zakArtId1 = key.ZakArtId;
              if (zakId == zakArtId1)
              {
                (long IpsObjId, Guid ProdCopyId) infoByZpcPrjLinkId = this.GetArticleInfoByZpcPrjLinkId(zpcEntryInfo.ArtPrjLinkId);
                if (infoByZpcPrjLinkId.IpsObjId != 0L)
                {
                  ZPCProcRouteInfo zpcProcRouteInfo;
                  if (!artProdCopies.ProcRoutes.TryGetValue(infoByZpcPrjLinkId.IpsObjId, out zpcProcRouteInfo))
                  {
                    zpcProcRouteInfo = new ZPCProcRouteInfo();
                    artProdCopies.ProcRoutes.Add(infoByZpcPrjLinkId.IpsObjId, zpcProcRouteInfo);
                  }
                  IDictionary<int, ZPCEntryInfoEx> zakInfo1 = zpcProcRouteInfo.ZakInfo;
                  key = keyValuePair.Key;
                  int zakArtTcKey1 = key.ZakArtTcKey;
                  ZPCEntryInfoEx zpcEntryInfoEx1;
                  ref ZPCEntryInfoEx local1 = ref zpcEntryInfoEx1;
                  if (!zakInfo1.TryGetValue(zakArtTcKey1, out local1))
                  {
                    zpcEntryInfoEx1 = new ZPCEntryInfoEx();
                    IDictionary<int, ZPCEntryInfoEx> zakInfo2 = zpcProcRouteInfo.ZakInfo;
                    key = keyValuePair.Key;
                    int zakArtTcKey2 = key.ZakArtTcKey;
                    ZPCEntryInfoEx zpcEntryInfoEx2 = zpcEntryInfoEx1;
                    zakInfo2.Add(zakArtTcKey2, zpcEntryInfoEx2);
                  }
                  ZPCSbList zpcSbList;
                  if (!zpcEntryInfoEx1.ExitSbsPrjLinkIds.TryGetValue(zpcEntryInfo.ExitPrjLinkId, out zpcSbList))
                  {
                    zpcSbList = new ZPCSbList();
                    zpcEntryInfoEx1.ExitSbsPrjLinkIds.Add(zpcEntryInfo.ExitPrjLinkId, zpcSbList);
                  }
                  string oldKey = $"{EntryHelper.GenerateTcEntryKey(keyValuePair.Key)}_{zpcEntryInfo.ExitPrjLinkId}_{infoByZpcPrjLinkId.IpsObjId}";
                  DictionaryValue dictionaryValue = this._import_data_main.GetValue(this.GetTempCategory(), (object) oldKey);
                  if (dictionaryValue != null)
                    zpcSbList.IpsObjId = dictionaryValue.NewObjectID;
                  if (!zpcSbList.Contains(zpcEntryInfo))
                  {
                    EntryInfo entryInfo;
                    ref EntryInfo local2 = ref entryInfo;
                    key = keyValuePair.Key;
                    int artTcKey = key.ArtTcKey;
                    key = keyValuePair.Key;
                    int artId3 = key.ArtId;
                    key = keyValuePair.Key;
                    int sbArtTcKey = key.SbArtTcKey;
                    key = keyValuePair.Key;
                    int sbArtId2 = key.SbArtId;
                    key = keyValuePair.Key;
                    int zakArtTcKey3 = key.ZakArtTcKey;
                    key = keyValuePair.Key;
                    int zakArtId2 = key.ZakArtId;
                    local2 = new EntryInfo(artTcKey, artId3, sbArtTcKey, sbArtId2, zakArtTcKey3, zakArtId2);
                    zpcEntryInfo.TcEntryInfo = entryInfo;
                    zpcSbList.Add(zpcEntryInfo);
                  }
                }
              }
            }
          }
        }
      }
    }
  }

  public (long IpsObjId, Guid ProdCopyId) GetArticleInfoByZpcPrjLinkId(int prjLinkId)
  {
    (long, Guid) infoByZpcPrjLinkId = (0L, Guid.Empty);
    DictionaryValue dictionaryValue = this._import_data_main.GetValue(ImportingCategory.ProductionListsObjects, (object) prjLinkId);
    if (dictionaryValue == null)
      return infoByZpcPrjLinkId;
    infoByZpcPrjLinkId.Item1 = dictionaryValue.NewObjectID;
    infoByZpcPrjLinkId.Item2 = this.GetProdCopyId(infoByZpcPrjLinkId.Item1);
    return infoByZpcPrjLinkId;
  }

  public Guid GetProdCopyId(long objId)
  {
    DictionaryValue dictionaryValue = this._import_data_main.GetValue(ImportingCategory.ProductionСopyIDCache, (object) objId);
    if (dictionaryValue != null)
    {
      Guid result;
      if (Guid.TryParse(dictionaryValue.Caption, out result))
        return result;
    }
    else
    {
      IDBAttribute objectAttributeById = this.plugin.Imdi.UserSession.GetObjectAttributeByID(objId, MRP2Consts.attrIdPKDSE_Id);
      Guid result;
      if (objectAttributeById != null && objectAttributeById.Value != null && Guid.TryParse(objectAttributeById.AsString, out result))
        return result;
    }
    return Guid.Empty;
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
      IAttributeTypeItem byGuid1 = imdi.AttributeTypes.GetByGuid(TechCardConsts.AttributeTypes.MemberOfAssemblyCopyAttrGUID);
      if (byGuid1 != null)
        this._atMemberOfAssemblyCopyAttr = byGuid1;
      IAttributeTypeItem byGuid2 = imdi.AttributeTypes.GetByGuid(TechCardConsts.AttributeTypes.MemberOfExitAssemblyAttrGUID);
      if (byGuid2 != null)
        this._atMemberOfExitAssemblyAttr = byGuid2;
      IAttributeTypeItem byGuid3 = imdi.AttributeTypes.GetByGuid(TechCardConsts.AttributeTypes.MemberOfProductionReportVersionAttrGUID);
      if (byGuid3 != null)
        this._atMemberOfProductionReportVersion = byGuid3;
      IAttributeTypeItem byGuid4 = imdi.AttributeTypes.GetByGuid(TechCardConsts.AttributeTypes.MemberOfProductionReportObjectAttrGUID);
      if (byGuid4 != null)
        this._atMemberOfProductionReportObject = byGuid4;
      IAttributeTypeItem byGuid5 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atLastLevelSeek);
      if (byGuid5 != null)
        this._atLastLevelSeek = byGuid5;
      IAttributeTypeItem byGuid6 = imdi.AttributeTypes.GetByGuid(new Guid("cad00021-306c-11d8-b4e9-00304f19f545"));
      if (byGuid6 != null)
        this._atNotes = byGuid6;
      IObjectTypeItem byGuid7 = imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otProductObjTypeGuid);
      if (byGuid7 != null)
        this._otArticleBaseTypeID = byGuid7.ID;
      base.LoadMetaData4Pump();
    }
  }

  protected override Guid GUID { get; } = new Guid("{0157DB15-18D1-49E0-94C2-2C84E2D4A683}");

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
        string str1 = "F_ZAK_TCKEY>0";
        string str2 = TechSettingsHelper.PumpMode == TechPumpMode.tpmProdZakList ? TechDataBuilder<PumpClass>.GetPumpModeCond("F_ZAK_TCKEY", Convert.ToInt32((object) Intermech.ImpExp.TechCard.Common.DataManager.DataManager.ObjDataType.odtProdZakKey)) : str1;
        if (str2.IsEmpty())
          return pumpModeCond;
        return $"({pumpModeCond} and {str2})";
      });
      this._dataSource = new TechDataSource((ITechDataBuilder) dataBuilder);
    }
    return this._dataSource;
  }

  protected override void PumpLoadData()
  {
    this.FillZaksCache();
    this.FillFwdTCCache();
    this.FillFwdS4Cache();
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
    return new ImportingCategory[5]
    {
      ImportingCategory.Articles,
      ImportingCategory.ProductionListsObjects,
      ImportingCategory.ProductionСopyIDCache,
      ImportingCategory.BaseTechObjectsVersionsCache,
      this.GetTempCategory()
    };
  }

  protected override void InitData()
  {
    this.objTypeID = -1;
    if (this.plugin.Imdi.ObjectTypes.ExistsByGuid(TechcardConsts.TypeConsts.otProcRoutingEntryGUID))
      this.objTypeID = this.plugin.Imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otProcRoutingEntryGUID).ID;
    this._tableName = "TC_OBJ2LINK";
    this._recType = "Входимость в маршрут обработки";
    this._articlePCTypeId = this.plugin.Imdi.ObjectTypes.GetByGuid(TechCardConsts.ObjectTypes.ArticleCopyBaseGUID).ID;
  }

  public override bool CheckObjTypeOrParamType(string entCode, Guid attrGuid) => false;

  public override bool CheckObjLinkOrParamType(string entCode, Guid attrGuid) => false;

  protected override void PumpBaseRec(TechObjectRecord record)
  {
    if (!(record is ZPCProcRouteEntryObject source))
      return;
    ArtProdCopies entryInfoInCache = this.FindFullEntryInfoInCache(source.EntryInfo);
    if (entryInfoInCache == null)
      return;
    foreach (long key in entryInfoInCache.ProcRoutes.Keys)
    {
      foreach (ZPCEntryInfoEx zpcEntryInfoEx in (IEnumerable<ZPCEntryInfoEx>) entryInfoInCache.ProcRoutes[key].ZakInfo.Values)
      {
        if (!zpcEntryInfoEx.Processed)
        {
          HashSet<Guid> guidSet = new HashSet<Guid>();
          foreach (KeyValuePair<int, ZPCSbList> exitSbsPrjLinkId in zpcEntryInfoEx.ExitSbsPrjLinkIds)
          {
            if (exitSbsPrjLinkId.Value.IpsObjId == 0L)
            {
              (long, Guid) valueTuple = exitSbsPrjLinkId.Key != 0 ? this.GetArticleInfoByZpcPrjLinkId(exitSbsPrjLinkId.Key) : (0L, Guid.NewGuid());
              if (valueTuple.Item2 == Guid.Empty)
                valueTuple.Item2 = Guid.NewGuid();
              if (!guidSet.Contains(valueTuple.Item2))
              {
                guidSet.Add(valueTuple.Item2);
                ZPCProcRouteEntryObject routeEntryObject = new ZPCProcRouteEntryObject(key, exitSbsPrjLinkId.Key);
                try
                {
                  ZPCEntryInfo zpcEntryInfo = exitSbsPrjLinkId.Value.FirstOrDefault<ZPCEntryInfo>();
                  if (zpcEntryInfo == null)
                    return;
                  EntryInfo entryInfo;
                  ref EntryInfo local = ref entryInfo;
                  EntryInfo tcEntryInfo = zpcEntryInfo.TcEntryInfo;
                  int artTcKey = tcEntryInfo.ArtTcKey;
                  tcEntryInfo = zpcEntryInfo.TcEntryInfo;
                  int artId = tcEntryInfo.ArtId;
                  tcEntryInfo = zpcEntryInfo.TcEntryInfo;
                  int sbArtTcKey = tcEntryInfo.SbArtTcKey;
                  tcEntryInfo = zpcEntryInfo.TcEntryInfo;
                  int sbArtId = tcEntryInfo.SbArtId;
                  tcEntryInfo = zpcEntryInfo.TcEntryInfo;
                  int zakArtTcKey = tcEntryInfo.ZakArtTcKey;
                  tcEntryInfo = zpcEntryInfo.TcEntryInfo;
                  int zakArtId = tcEntryInfo.ZakArtId;
                  local = new EntryInfo(artTcKey, artId, sbArtTcKey, sbArtId, zakArtTcKey, zakArtId);
                  routeEntryObject.AssignTo((ProcRouteEntryObject) source, entryInfo);
                  this._techParmList.Clear();
                  ObjectRecord techObject = this.CreateTechObject((TechObjectRecord) routeEntryObject);
                  routeEntryObject.ParamList.Clear();
                  if (this._techParmList.Count > 0)
                    routeEntryObject.ParamList.AddRange((IEnumerable<ITechParamBase>) this._techParmList);
                  this.tpObjRecList[(TechObjectRecordBase) routeEntryObject] = routeEntryObject.ParamList;
                  this.FillObjectParams((TechObjectRecord) routeEntryObject, routeEntryObject.ParamList, techObject);
                }
                catch (Exception ex)
                {
                  this.plugin.appManager.AddWarningMessage($"Ошибка обработки записи \"{source.Key}\" таблицы \"{source.TableName}\": {ex.Message}{Environment.NewLine + ex.StackTrace}");
                  if (ex is OutOfMemoryException)
                    throw;
                  this.DoHandleImportObjectsException(ex);
                }
              }
            }
          }
        }
      }
    }
    this.SetProcessed(source.EntryInfo, true);
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (!(record is ZPCProcRouteEntryObject record1) || objRecord == null || record1.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record1.RecMode != TechObjectRecord.PumpMode.ObjectOnly || record1.diff_ArtTcKey != 0)
      return;
    ArtProdCopies entryInfoInCache = this.FindFullEntryInfoInCache(record1.EntryInfo);
    ZPCProcRouteInfo zpcProcRouteInfo;
    if (entryInfoInCache == null || !entryInfoInCache.ProcRoutes.TryGetValue(record1.ArtIpsObjId, out zpcProcRouteInfo))
      return;
    EntryInfo entryInfo = record1.EntryInfo;
    (long ObjId, long ObjVerId, string Caption) articleInfoByKey = this.GetArticleInfoByKey(entryInfo.ZakArtTcKey);
    if (this._atMemberOfProductionReportObject != null && articleInfoByKey.ObjId != 0L)
      this._techParmList.AddAttribute(this._atMemberOfProductionReportObject, (object) articleInfoByKey.ObjId, articleInfoByKey.Caption);
    if (this._atMemberOfAssemblyCopyAttr != null)
    {
      int num = 0;
      IDictionary<int, ZPCEntryInfoEx> zakInfo = zpcProcRouteInfo.ZakInfo;
      entryInfo = record1.EntryInfo;
      int zakArtTcKey = entryInfo.ZakArtTcKey;
      ZPCEntryInfoEx zpcEntryInfoEx;
      ref ZPCEntryInfoEx local = ref zpcEntryInfoEx;
      if (zakInfo.TryGetValue(zakArtTcKey, out local))
      {
        if (record1.ExitSbPrjLinkId > 0)
        {
          ZPCSbList zpcSbList;
          if (zpcEntryInfoEx.ExitSbsPrjLinkIds.TryGetValue(record1.ExitSbPrjLinkId, out zpcSbList))
          {
            foreach (ZPCEntryInfo zpcEntryInfo in (List<ZPCEntryInfo>) zpcSbList)
            {
              Guid prodCopyId = this.GetArticleInfoByZpcPrjLinkId(zpcEntryInfo.SbPrjLinkId).ProdCopyId;
              if (prodCopyId != Guid.Empty && this._techParmList.AddAttribute(this._atMemberOfAssemblyCopyAttr, (object) prodCopyId) is TechParamAttribute techParamAttribute)
              {
                techParamAttribute.Index = num;
                ++num;
              }
            }
          }
        }
        else
        {
          Guid prodCopyId = this.GetProdCopyId(record1.ArtIpsObjId);
          if (prodCopyId != Guid.Empty)
            this._techParmList.AddAttribute(this._atMemberOfAssemblyCopyAttr, (object) prodCopyId);
        }
      }
    }
    if (this._atMemberOfExitAssemblyAttr != null)
    {
      (long, Guid) valueTuple = record1.ExitSbPrjLinkId != 0 ? this.GetArticleInfoByZpcPrjLinkId(record1.ExitSbPrjLinkId) : (record1.ArtIpsObjId, this.GetProdCopyId(record1.ArtIpsObjId));
      if (valueTuple.Item2 != Guid.Empty)
        this._techParmList.AddAttribute(this._atMemberOfExitAssemblyAttr, (object) valueTuple.Item2);
    }
    if (this._atLastLevelSeek != null)
      this._techParmList.AddAttribute(this._atLastLevelSeek, (object) (long) this.GetFirstStepLifecycle());
    base.FillTechObject(objRecord, (TechObjectRecord) record1);
  }

  protected override void CheckBaseRecords()
  {
  }

  private bool IsProcessed(EntryInfo entryInfo)
  {
    ArtProdCopies entryInfoInCache = this.FindFullEntryInfoInCache(entryInfo);
    if (entryInfoInCache == null)
      return true;
    bool flag = true;
    foreach (KeyValuePair<long, ZPCProcRouteInfo> procRoute in entryInfoInCache.ProcRoutes)
    {
      foreach (KeyValuePair<int, ZPCEntryInfoEx> keyValuePair in (IEnumerable<KeyValuePair<int, ZPCEntryInfoEx>>) procRoute.Value.ZakInfo)
      {
        flag = flag && keyValuePair.Value.Processed;
        if (!flag)
          break;
      }
    }
    return flag;
  }

  private void SetProcessed(EntryInfo entryInfo, bool processed)
  {
    ArtProdCopies entryInfoInCache = this.FindFullEntryInfoInCache(entryInfo);
    if (entryInfoInCache == null)
      return;
    foreach (KeyValuePair<long, ZPCProcRouteInfo> procRoute in entryInfoInCache.ProcRoutes)
    {
      foreach (KeyValuePair<int, ZPCEntryInfoEx> keyValuePair in (IEnumerable<KeyValuePair<int, ZPCEntryInfoEx>>) procRoute.Value.ZakInfo)
        keyValuePair.Value.Processed = processed;
    }
  }

  protected override string GetRecordPumpMode(TechObjectRecord record)
  {
    if (!(record is ZPCProcRouteEntryObject routeEntryObject))
    {
      record.RecMode = TechObjectRecord.PumpMode.NotPump;
      return string.Empty;
    }
    if (TechSettingsHelper.PumpLinksOnlyWithActual && this._import_data_main.GetValue(ImportingCategory.BaseTechObjectsVersionsCache, (object) TechPumpBase.GenBaseTechObjectsVersionsCacheKey(routeEntryObject.LinkedObj.ObjKeyKey, routeEntryObject.LinkedObj.ObjType)) == null)
    {
      routeEntryObject.RecMode = TechObjectRecord.PumpMode.NotPump;
      return string.Empty;
    }
    ArtProdCopies entryInfoInCache = this.FindFullEntryInfoInCache(routeEntryObject.EntryInfo);
    if (entryInfoInCache == null)
    {
      record.RecMode = TechObjectRecord.PumpMode.NotPump;
      return string.Empty;
    }
    if (entryInfoInCache.Processed)
    {
      record.RecMode = TechObjectRecord.PumpMode.NotPump;
      return string.Empty;
    }
    if (entryInfoInCache.ProcRoutes.Values.All<ZPCProcRouteInfo>((System.Func<ZPCProcRouteInfo, bool>) (routeInfo => routeInfo.ZakInfo.Values.All<ZPCEntryInfoEx>((System.Func<ZPCEntryInfoEx, bool>) (zakInfo => zakInfo.ExitSbsPrjLinkIds.Values.All<ZPCSbList>((System.Func<ZPCSbList, bool>) (entryInfo => entryInfo.IpsObjId != 0L)))))))
    {
      entryInfoInCache.Processed = true;
      record.RecMode = TechObjectRecord.PumpMode.NotPump;
    }
    entryInfoInCache.Processed = true;
    record.RecMode = TechObjectRecord.PumpMode.ObjectAndLinks;
    return string.Empty;
  }

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new ZPCProcRouteEntryObject(0L, 0);
  }

  protected override void AddValue2Cache(
    object oldKey,
    long newKey,
    TechObjectRecordBase recBase,
    TechParamList recParmList)
  {
    if (!(recBase is ZPCProcRouteEntryObject routeEntryObject))
      return;
    ArtProdCopies entryInfoInCache = this.FindFullEntryInfoInCache(routeEntryObject.EntryInfo);
    ZPCProcRouteInfo zpcProcRouteInfo;
    if (entryInfoInCache == null || !entryInfoInCache.ProcRoutes.TryGetValue(routeEntryObject.ArtIpsObjId, out zpcProcRouteInfo))
      return;
    string uniqueRecordKey = EntryHelper.GenerateUniqueRecordKey(recBase);
    ZPCEntryInfoEx zpcEntryInfoEx;
    ZPCSbList zpcSbList;
    if (!zpcProcRouteInfo.ZakInfo.TryGetValue(routeEntryObject.EntryInfo.ZakArtTcKey, out zpcEntryInfoEx) || !zpcEntryInfoEx.ExitSbsPrjLinkIds.TryGetValue(routeEntryObject.ExitSbPrjLinkId, out zpcSbList))
      return;
    zpcSbList.IpsObjId = newKey;
    this._import_data_main.AddValue(this.GetTempCategory(), (object) uniqueRecordKey, newKey, zpcProcRouteInfo.RouteProcId.ToString());
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

  protected override void DoAfterPump()
  {
    Dictionary<object, DictionaryValue> category1 = this._import_data_main.GetCategory(this.GetTechCategory());
    if (category1 == null)
      return;
    category1.Clear();
    Dictionary<object, DictionaryValue> category2 = this._import_data_main.GetCategory(this.GetTempCategory());
    Dictionary<string, ProcRoutesTag> dictionary = new Dictionary<string, ProcRoutesTag>();
    foreach (object key1 in category2.Keys)
    {
      DictionaryValue dictionaryValue = category2[key1];
      string[] source = key1.ToString().Split('_');
      string key2 = string.Join("_", source, 0, 3);
      long result1;
      Guid result2;
      if (long.TryParse(((IEnumerable<string>) source).Last<string>(), out result1) && Guid.TryParse(dictionaryValue.Caption, out result2))
      {
        ProcRoutesTag procRoutesTag;
        if (!dictionary.TryGetValue(key2, out procRoutesTag))
        {
          procRoutesTag = new ProcRoutesTag();
          dictionary.Add(key2, procRoutesTag);
        }
        ProcRouteEntryTag procRouteEntryTag;
        if (!procRoutesTag.Entries.TryGetValue(result2, out procRouteEntryTag))
        {
          procRouteEntryTag = new ProcRouteEntryTag(result2)
          {
            IpsOwnerObjId = result1,
            IpsOwnerObjTypeId = this._articlePCTypeId
          };
          procRoutesTag.Entries.Add(procRouteEntryTag.ProcRouteId, procRouteEntryTag);
        }
        if (!procRouteEntryTag.Entries.Contains(dictionaryValue.NewObjectID))
          procRouteEntryTag.Entries.Add(dictionaryValue.NewObjectID);
      }
    }
    foreach (KeyValuePair<string, ProcRoutesTag> keyValuePair in dictionary)
    {
      if (this._import_data_main.GetValue(this.GetTechCategory(), (object) keyValuePair.Key) == null)
        this._import_data_main.AddValue(this.GetTechCategory(), (object) keyValuePair.Key, 0L, (ITagImportObject) keyValuePair.Value);
      else
        this.plugin.appManager.AddInfoMessage("Записи о входимости в ПЗ уже импортировались ранее: " + keyValuePair.Key);
    }
    base.DoAfterPump();
  }
}
