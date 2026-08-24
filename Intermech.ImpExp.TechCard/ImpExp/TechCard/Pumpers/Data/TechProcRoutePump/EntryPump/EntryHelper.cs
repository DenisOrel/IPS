// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.EntryPump.EntryHelper
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.ZPCEntryPump;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.EntryPump;

internal class EntryHelper
{
  public static string GenerateUniqueRecordKey(TechObjectRecordBase recBase)
  {
    switch (recBase)
    {
      case ProcRouteObject procRouteObject:
        object[] objArray1 = new object[4]
        {
          (object) procRouteObject.EntryInfo.ArtTcKey,
          null,
          null,
          null
        };
        EntryInfo entryInfo1 = procRouteObject.EntryInfo;
        objArray1[1] = (object) entryInfo1.SbArtTcKey;
        entryInfo1 = procRouteObject.EntryInfo;
        objArray1[2] = (object) entryInfo1.ZakArtTcKey;
        objArray1[3] = (object) procRouteObject.RouteId.ToString();
        return string.Format("{0}_{1}_{2}_{3}", objArray1);
      case ZPCProcRouteEntryObject routeEntryObject1:
        object[] objArray2 = new object[5]
        {
          (object) routeEntryObject1.EntryInfo.ArtTcKey,
          null,
          null,
          null,
          null
        };
        EntryInfo entryInfo2 = routeEntryObject1.EntryInfo;
        objArray2[1] = (object) entryInfo2.SbArtTcKey;
        entryInfo2 = routeEntryObject1.EntryInfo;
        objArray2[2] = (object) entryInfo2.ZakArtTcKey;
        objArray2[3] = (object) routeEntryObject1.ExitSbPrjLinkId;
        objArray2[4] = (object) routeEntryObject1.ArtIpsObjId;
        return string.Format("{0}_{1}_{2}_{3}_{4}", objArray2);
      case ProcRouteEntryObject routeEntryObject2:
        return $"{routeEntryObject2.EntryInfo.ArtTcKey}_{routeEntryObject2.EntryInfo.SbArtTcKey}_{routeEntryObject2.EntryInfo.ZakArtTcKey}";
      default:
        return EntryHelper.GenerateTcEntryKey(recBase);
    }
  }

  public static string GenerateTcEntryKey(TechObjectRecordBase recBase)
  {
    return $"{Convert.ToInt32(recBase.Fields["F_ART_TCKEY"])}_{Convert.ToInt32(recBase.Fields["F_PROJ_TCKEY"])}_{Convert.ToInt32(recBase.Fields["F_ZAK_TCKEY"])}";
  }

  public static string GenerateTcEntryKey(EntryInfo entryInfo)
  {
    return $"{entryInfo.ArtTcKey}_{entryInfo.SbArtTcKey}_{entryInfo.ZakArtTcKey}";
  }

  public static bool IsDefaultRouteEntry(TechObjectRecordBase record)
  {
    return record is ProcRouteEntryObject routeEntryObject && routeEntryObject.EntryInfo.SbArtTcKey == routeEntryObject.EntryInfo.ZakArtTcKey && routeEntryObject.EntryInfo.ZakArtTcKey == 0;
  }
}
