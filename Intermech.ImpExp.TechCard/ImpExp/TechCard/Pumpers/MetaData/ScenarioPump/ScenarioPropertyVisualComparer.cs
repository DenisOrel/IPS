// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.ScenarioPump.ScenarioPropertyVisualComparer
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.ScenarioPump;

internal class ScenarioPropertyVisualComparer : IComparer<ScenarioProperty>, IComparer
{
  public int Compare(ScenarioProperty x, ScenarioProperty y)
  {
    if (x == null)
      throw new ArgumentNullException(nameof (x));
    if (y == null)
      throw new ArgumentNullException(nameof (y));
    if (x == y)
      return 0;
    int num1 = x.VidDet.CompareTo(y.VidDet);
    if (num1 != 0)
      return num1;
    int num2 = x.VidZag.CompareTo(y.VidZag);
    if (num2 != 0)
      return num2;
    int num3 = x.IsReCountButton.CompareTo(y.IsReCountButton);
    return num3 != 0 ? num3 : x.SlideId.CompareTo(y.SlideId);
  }

  public int Compare(object x, object y)
  {
    return this.Compare(x as ScenarioProperty, y as ScenarioProperty);
  }
}
