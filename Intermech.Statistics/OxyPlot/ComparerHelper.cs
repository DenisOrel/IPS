// Decompiled with JetBrains decompiler
// Type: OxyPlot.ComparerHelper
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot;

public static class ComparerHelper
{
  public static IComparer<T> CreateComparer<T>(Comparison<T> comparison)
  {
    return (IComparer<T>) new ComparerHelper.ComparisonComparer<T>(comparison);
  }

  private class ComparisonComparer<T> : IComparer<T>
  {
    private readonly Comparison<T> comparison;

    public ComparisonComparer(Comparison<T> comparison) => this.comparison = comparison;

    public int Compare(T x, T y) => this.comparison(x, y);
  }
}
