// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.LoadCache.TechCache
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common.ArcArtPump;
using Intermech.ImpExp.TechCard.Pumpers;
using Intermech.ImpExp.TechCard.Pumpers.Data.Obj2Link;
using Intermech.ImpExp.TechCard.Pumpers.Data.Tp2LinkPump;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.ArcArtPump;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.TC_Configs;
using Intermech.ImpExp.TechCard.TechProcPump;
using Intermech.ImpExp.TechCard.TechTypes;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common.LoadCache;

internal static class TechCache
{
  public static bool isResumeMode;
  public static SavePoint SavePoint;

  public static bool ReadAllLists(ISavePoint savePoint)
  {
    bool flag = true;
    SavePoint savePoint1 = savePoint?.GetSavePoint();
    List<Guid> guidList = savePoint1?.PumpCompleted != null ? savePoint1.PumpCompleted : new List<Guid>();
    if (!TechCache.ReadOneList(TechCache.CategoryList.TechEntFixList))
      flag = false;
    if (!TechCache.ReadOneList(TechCache.CategoryList.EntTypeList))
      flag = false;
    bool obligated1 = guidList.Contains(ArtArtsPump.ClassGuid);
    if (!TechCache.ReadOneList(TechCache.CategoryList.ArcArtList, obligated1))
      flag = !obligated1;
    bool obligated2 = guidList.Contains(Tp2Obj2LinkPump.ClassGuid);
    if (!TechCache.ReadOneList(TechCache.CategoryList.TechTp2ObjLinkList, obligated2))
      flag = !obligated2;
    if (!TechCache.ReadOneList(TechCache.CategoryList.TechTypeList))
      flag = false;
    if (!TechCache.ReadOneList(TechCache.CategoryList.EnityRefDirectoreList))
      flag = false;
    if (!TechCache.ReadOneList(TechCache.CategoryList.TechImTablesList))
      flag = false;
    if (!TechCache.ReadOneList(TechCache.CategoryList.TechImFieldsList))
      flag = false;
    if (!TechCache.ReadOneList(TechCache.CategoryList.Code2AttributeGuid))
      flag = false;
    if (!TechCache.ReadOneList(TechCache.CategoryList.EntTypeList))
      flag = false;
    if (!TechCache.ReadOneList(TechCache.CategoryList.ProductionsList))
      flag = false;
    if (!TechCache.ReadOneList(TechCache.CategoryList.TechConfigData))
      flag = false;
    if (!TechCache.ReadOneList(TechCache.CategoryList.EntitiesList))
      flag = false;
    return flag;
  }

  public static void WriteOneList(TechCache.CategoryList en, object obj)
  {
    ICache cacheService = TechCache.GetCacheService();
    if (cacheService == null)
      return;
    cacheService.DeleteCache((ImportingCategory) en);
    IImportingData cache = cacheService.GetCache((ImportingCategory) en);
    try
    {
      if (cache.GetNewKey((ImportingCategory) en, (object) 1) == 1L)
        return;
      TechObjectTag tag = new TechObjectTag(obj);
      cache.AddValue((ImportingCategory) en, (object) 1, 1L, (ITagImportObject) tag);
    }
    finally
    {
      cacheService.ReleaseCache((ImportingCategory) en);
    }
  }

  public static bool ReadOneList(TechCache.CategoryList en) => TechCache.ReadOneList(en, true);

  public static bool ReadOneList(TechCache.CategoryList en, bool obligated)
  {
    object cacheData;
    bool flag = TechCache.ReadOneList(en, out cacheData, obligated);
    if (!flag || cacheData == null)
      return flag;
    switch ((ImportingCategory) en)
    {
      case ImportingCategory.Tech_C_EntFixList:
        TechPumpData.EntFixList = (TechEntFixList) cacheData;
        break;
      case ImportingCategory.Tech_C_EntTypeList:
        TechPumpData.EntTypeList = (EntityTypeList) cacheData;
        break;
      case ImportingCategory.Tech_C_TechImTablesList:
        TechPumpData.Tables.ImTablesData = (ImTableInfoCache) cacheData;
        break;
      case ImportingCategory.Tech_C_ArcArtList:
        TechPumpData.TechObjects.ArcArtList = (Dictionary<long, ArcArtsObject>) cacheData;
        break;
      case ImportingCategory.Tech_C_Code2AttributeGuid:
        TechPumpData.Entities.Code2AttributeGuid = (Dictionary<string, Guid>) cacheData;
        break;
      case ImportingCategory.Tech_C_EnityRefDirectoreList:
        TechPumpData.Entities._entityRefDataList = (EntityReferenceList) cacheData;
        break;
      case ImportingCategory.Tech_C_TechTypeList:
        TechPumpData.TechType._techTypeList = (TechTypeList) cacheData;
        break;
      case ImportingCategory.Tech_C_ProductionList:
        TechPumpData.Production._productions = cacheData as Dictionary<int, IpsProductionObj>;
        break;
      case ImportingCategory.Tech_C_EntitiesList:
        TechPumpData.Entities._entitiesList = (Dictionary<string, Entity>) cacheData;
        break;
      case ImportingCategory.Tech_C_TechImFieldsList:
        TechPumpData.Tables.ImFieldsData = (ImFieldInfoCache) cacheData;
        break;
      case ImportingCategory.Tech_C_TechConfigs:
        TechPumpData.Configs.Cache = (TechConfigCache) cacheData;
        break;
      case ImportingCategory.Tech_C_Tp2ObjLinkList:
        TechPumpData.TechObjects.Tp2LinkList = (Dictionary<long, List<Obj2LinkInfoObject>>) cacheData;
        break;
    }
    return true;
  }

  public static bool ReadOneList(TechCache.CategoryList en, out object cacheData, bool obligated)
  {
    bool flag = true;
    cacheData = (object) null;
    ICache cacheService = TechCache.GetCacheService();
    if (cacheService == null)
      return false;
    IImportingData cache = cacheService.GetCache((ImportingCategory) en);
    try
    {
      if (cache.GetTag((ImportingCategory) en, (object) 1) is TechObjectTag tag)
      {
        cacheData = tag.Object;
      }
      else
      {
        if (obligated)
          TechcardConsts.Plugin.appManager.AddErrorMessage("Невозможно прочитать кэш предыдущей закачки: " + (object) en);
        flag = false;
      }
    }
    catch (Exception ex)
    {
      TechcardConsts.Plugin.appManager.AddWarningMessage($"Невозможно прочитать кэш {en.ToString()}: {ex.Message}");
      flag = false;
    }
    finally
    {
      cacheService.ReleaseCache((ImportingCategory) en);
    }
    return flag;
  }

  public static ICache GetCacheService()
  {
    if (ServicesManager.GetService(typeof (ICache)) is ICache service)
      return service;
    TechcardConsts.Plugin.appManager.AddWarningMessage($"Служба {typeof (ICache)} не найдена");
    return service;
  }

  public static void IsExistImbaseCaches()
  {
    ICache cacheService = TechCache.GetCacheService();
    if (cacheService == null)
      return;
    foreach (ImportingCategory category in TechcardConsts.TechCacheConsts.GetAllIMBASECategory())
    {
      if (!cacheService.Exist(category))
        TechcardConsts.Plugin.appManager.AddWarningMessage($"Кэш {category.ToString()} не найден");
    }
  }

  public static void IsExistSearchCaches()
  {
    ICache cacheService = TechCache.GetCacheService();
    if (cacheService == null)
      return;
    foreach (ImportingCategory category in TechcardConsts.TechCacheConsts.GetAllSEARCHCategory())
    {
      if (!cacheService.Exist(category))
        TechcardConsts.Plugin.appManager.AddWarningMessage($"кэш {category} не найден");
    }
  }

  public enum CategoryList
  {
    TechEntFixList = 61, // 0x0000003D
    EntTypeList = 62, // 0x0000003E
    TechImTablesList = 64, // 0x00000040
    ArcArtList = 65, // 0x00000041
    EntityRefList = 66, // 0x00000042
    Code2AttributeGuid = 67, // 0x00000043
    EnityRefDirectoreList = 68, // 0x00000044
    TechTypeList = 69, // 0x00000045
    ProductionsList = 71, // 0x00000047
    MasterAtrList = 73, // 0x00000049
    EntitiesList = 74, // 0x0000004A
    EntitiesListOrg = 156, // 0x0000009C
    TechImFieldsList = 174, // 0x000000AE
    TechAttributes2Exclude = 177, // 0x000000B1
    TechConfigData = 178, // 0x000000B2
    TechTp2ObjLinkList = 197, // 0x000000C5
  }
}
