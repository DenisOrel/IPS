// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.TechExpPump.Common.TechExpKeyConverter
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.TechExpPump.Common;

public static class TechExpKeyConverter
{
  private const int ExpObjDelta = 1000000;

  public static TechExpKey ConvertTo(long expertObjectKey, long recordId = 0)
  {
    return new TechExpKey(expertObjectKey * 1000000L + recordId);
  }

  public static bool ConvertFrom(
    TechExpKey techExpKey,
    out long expertObjectKey,
    out long recordId)
  {
    expertObjectKey = techExpKey.Value / 1000000L;
    recordId = techExpKey.Value - expertObjectKey * 1000000L;
    return true;
  }

  public static bool ConvertFrom(TechExpKey techExpKey, out long expertObjectKey)
  {
    return TechExpKeyConverter.ConvertFrom(techExpKey, out expertObjectKey, out long _);
  }
}
