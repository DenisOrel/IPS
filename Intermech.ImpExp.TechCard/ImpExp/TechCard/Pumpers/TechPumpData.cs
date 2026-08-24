// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.TechPumpData
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Common.ArcArtPump;
using Intermech.ImpExp.TechCard.Pumpers.Data.Obj2Link;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.TC_Configs;
using Intermech.ImpExp.TechCard.TechProcPump;
using Intermech.ImpExp.TechCard.TechTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers;

public static class TechPumpData
{
  internal static TechEntFixList EntFixList;
  internal static EntityTypeList EntTypeList;

  public static class Entities
  {
    internal static EntityReferenceList _entityRefDataList = new EntityReferenceList();
    internal static Dictionary<string, Entity> _entitiesList = new Dictionary<string, Entity>();
    internal static Dictionary<string, Guid> Code2AttributeGuid = new Dictionary<string, Guid>();
    public static EntityProductionList EntityProductionList = new EntityProductionList();

    public static EntityReferenceList EntityRefDataList
    {
      [DebuggerStepThrough] get => TechPumpData.Entities._entityRefDataList;
    }

    public static Dictionary<string, Entity> EntitiesList
    {
      [DebuggerStepThrough] get => TechPumpData.Entities._entitiesList;
    }
  }

  public static class TechType
  {
    internal static TechTypeList _techTypeList = new TechTypeList();

    public static TechTypeList TechTypeList
    {
      [DebuggerStepThrough] get => TechPumpData.TechType._techTypeList;
    }
  }

  public static class Production
  {
    internal static Dictionary<int, IpsProductionObj> _productions = new Dictionary<int, IpsProductionObj>();

    internal static Dictionary<int, IpsProductionObj> Productions
    {
      [DebuggerStepThrough] get => TechPumpData.Production._productions;
    }
  }

  internal static class TechObjects
  {
    public static Dictionary<long, ArcArtsObject> ArcArtList;
    public static Dictionary<long, List<Obj2LinkInfoObject>> Tp2LinkList;
  }

  internal static class Tables
  {
    public static ImTableInfoCache ImTablesData;
    public static ImFieldInfoCache ImFieldsData;
  }

  internal static class Configs
  {
    private static readonly Dictionary<int, int> MaxDigitCache = new Dictionary<int, int>();
    public const int CidDigAfterDefault = 88;
    public const int CidIzwTypes = 124;
    public const int cidWorkWithArtVers = 171;
    public static TechConfigCache Cache = (TechConfigCache) null;

    public static int MaxDigitsAfter(int productionId)
    {
      int result;
      if (TechPumpData.Configs.MaxDigitCache.TryGetValue(productionId, out result))
        return result;
      if (productionId == 0)
        productionId = 19;
      if (!int.TryParse(TechPumpData.Configs.Cache.GetCustomConfigById(88, productionId, out bool _), out result))
        result = 9;
      TechPumpData.Configs.MaxDigitCache[productionId] = result;
      return result;
    }

    public static bool WorkWithArtVers
    {
      get
      {
        int result;
        return int.TryParse(TechPumpData.Configs.Cache.GetCustomConfigById(88, 0, out bool _), out result) && Convert.ToBoolean(result);
      }
    }
  }
}
