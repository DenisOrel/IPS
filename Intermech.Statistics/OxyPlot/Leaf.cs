// Decompiled with JetBrains decompiler
// Type: OxyPlot.Leaf
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;

#nullable disable
namespace OxyPlot;

internal class Leaf : Node
{
  public Leaf(int symbol)
  {
    this.Symbol = symbol >= 0 ? symbol : throw new ArgumentException("Illegal symbol value", nameof (symbol));
  }

  public int Symbol { get; private set; }
}
