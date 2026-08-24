// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.BarSeriesBase`1
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OxyPlot.Series;

public abstract class BarSeriesBase<T> : BarSeriesBase where T : BarItemBase, new()
{
  private bool ownsItemsSourceItems;

  protected BarSeriesBase() => this.Items = new List<T>();

  public List<T> Items { get; private set; }

  protected List<T> ActualItems => this.ItemsSource == null ? this.Items : this.ItemsSourceItems;

  protected List<T> ItemsSourceItems { get; set; }

  protected internal override IList<CategorizedItem> GetItems()
  {
    return (IList<CategorizedItem>) this.ActualItems.Cast<CategorizedItem>().ToList<CategorizedItem>();
  }

  protected internal override void UpdateData()
  {
    if (this.ItemsSource == null)
      return;
    if (this.ItemsSource is List<T> itemsSource)
    {
      this.ItemsSourceItems = itemsSource;
      this.ownsItemsSourceItems = false;
    }
    else
    {
      this.ClearItemsSourceItems();
      if (this.ValueField == null)
        this.ItemsSourceItems.AddRange(this.ItemsSource.OfType<T>());
      else
        this.UpdateFromDataFields();
    }
  }

  protected abstract void UpdateFromDataFields();

  protected override object GetItem(int i)
  {
    return this.ItemsSource != null || this.ActualItems == null || this.ActualItems.Count == 0 ? base.GetItem(i) : (object) this.ActualItems[i];
  }

  private void ClearItemsSourceItems()
  {
    if (!this.ownsItemsSourceItems || this.ItemsSourceItems == null)
      this.ItemsSourceItems = new List<T>();
    else
      this.ItemsSourceItems.Clear();
    this.ownsItemsSourceItems = true;
  }
}
