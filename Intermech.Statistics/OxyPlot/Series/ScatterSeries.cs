// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.ScatterSeries
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Series;

public class ScatterSeries : ScatterSeries<ScatterPoint>
{
  protected override void UpdateFromDataFields()
  {
    OxyPlot.ListBuilder<ScatterPoint> listBuilder = new OxyPlot.ListBuilder<ScatterPoint>();
    listBuilder.Add<double>(this.DataFieldX, double.NaN);
    listBuilder.Add<double>(this.DataFieldY, double.NaN);
    listBuilder.Add<double>(this.DataFieldSize, double.NaN);
    listBuilder.Add<double>(this.DataFieldValue, double.NaN);
    listBuilder.Add<object>(this.DataFieldTag, (object) null);
    listBuilder.FillT((IList<ScatterPoint>) this.ItemsSourcePoints, this.ItemsSource, (Func<IList<object>, ScatterPoint>) (args => new ScatterPoint(Convert.ToDouble(args[0]), Convert.ToDouble(args[1]), Convert.ToDouble(args[2]), Convert.ToDouble(args[3]), args[4])));
  }
}
