// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.CompareTreeWindowSettings
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System.Collections.Specialized;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal class CompareTreeWindowSettings
{
  public static HybridDictionary GetSettings(ICompareTreeWindowSettings compareTreeWindow)
  {
    return new HybridDictionary(0, true);
  }

  public static void SetSettings(
    HybridDictionary settings,
    ICompareTreeWindowSettings compareTreeWindow)
  {
  }
}
