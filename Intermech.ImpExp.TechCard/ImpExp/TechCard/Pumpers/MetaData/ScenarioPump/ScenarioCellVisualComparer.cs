// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.ScenarioPump.ScenarioCellVisualComparer
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.ScenarioPump;

internal class ScenarioCellVisualComparer : IComparer<ScenarioCell>, IComparer
{
  public int Compare(ScenarioCell x, ScenarioCell y)
  {
    if (x == y)
      return 0;
    if (x == null)
      return 1;
    if (y == null)
      return -1;
    int num1 = x.Type.CompareTo((object) y.Type);
    if (num1 != 0)
      return num1;
    int num2 = string.CompareOrdinal(x.Value, y.Value);
    return num2 != 0 ? num2 : x.IsReCountButton.CompareTo(y.IsReCountButton);
  }

  public int Compare(object x, object y) => this.Compare(x as ScenarioCell, y as ScenarioCell);
}
