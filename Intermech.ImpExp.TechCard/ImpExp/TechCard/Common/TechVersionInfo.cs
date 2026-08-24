// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.TechVersionInfo
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

#nullable disable
namespace Intermech.ImpExp.TechCard.Common;

public static class TechVersionInfo
{
  public static class DataBase
  {
    public const int MinVersion = 891;
    public const int MaxVersion = 2147483647 /*0x7FFFFFFF*/;
    public static int CurrentVersion;
  }

  public static class Program
  {
    public const int MinVersion = 9;
    public const int MaxVersion = 9;
    public const int MinRevision = 8;
    public const int MaxRevision = 8;
    public static int CurrentVersion;
    public static int CurrentRevision;
    internal const string RevisionParam = "TRUNID";
  }
}
