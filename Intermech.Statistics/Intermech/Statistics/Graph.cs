// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.Graph
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Statistics;

public class Graph
{
  public string Caption { get; }

  public List<DataPoint> Points { get; }

  public Graph(string caption, List<DataPoint> points)
  {
    this.Caption = caption;
    this.Points = points;
  }
}
