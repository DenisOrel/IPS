// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.ScenarioPump.ScenarioVisualComparer
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.ScenarioPump;

internal class ScenarioVisualComparer : IComparer<Scenario>, IComparer
{
  private readonly IComparer _cellComparer = (IComparer) new ScenarioCellVisualComparer();
  private readonly IComparer _propertyComparer = (IComparer) new ScenarioPropertyVisualComparer();

  public int Compare(Scenario x, Scenario y)
  {
    if (x == null)
      throw new ArgumentNullException(nameof (x));
    if (y == null)
      throw new ArgumentNullException(nameof (y));
    if (x == y)
      return 0;
    int num1 = x.Kind.CompareTo((object) y.Kind);
    if (num1 != 0)
      return num1;
    int num2 = x.ColCount.CompareTo(y.ColCount);
    if (num2 != 0)
      return num2;
    int num3 = x.RowCount.CompareTo(y.RowCount);
    if (num3 != 0)
      return num3;
    int num4 = this._propertyComparer.Compare((object) x.Property, (object) y.Property);
    if (num4 != 0)
      return num4;
    for (int index1 = 0; index1 < x.ColCount; ++index1)
    {
      for (int index2 = 0; index2 < x.RowCount; ++index2)
      {
        int num5 = this._cellComparer.Compare((object) x.Cells[index1, index2], (object) y.Cells[index1, index2]);
        if (num5 != 0)
          return num5;
      }
    }
    return 0;
  }

  public int Compare(object x, object y) => this.Compare(x as Scenario, y as Scenario);
}
