// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.ItemsSeries
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections;
using System.Linq;

#nullable disable
namespace OxyPlot.Series;

public abstract class ItemsSeries : OxyPlot.Series.Series
{
  [CodeGeneration(false)]
  public IEnumerable ItemsSource { get; set; }

  protected internal override void UpdateValidData()
  {
  }

  protected static object GetItem(IEnumerable itemsSource, int index)
  {
    if (itemsSource == null || index < 0)
      return (object) null;
    if (itemsSource is IList list)
      return index < list.Count && index >= 0 ? list[index] : (object) null;
    int i = 0;
    return itemsSource.Cast<object>().FirstOrDefault<object>((Func<object, bool>) (item => i++ == index));
  }

  protected virtual object GetItem(int i) => ItemsSeries.GetItem(this.ItemsSource, i);
}
