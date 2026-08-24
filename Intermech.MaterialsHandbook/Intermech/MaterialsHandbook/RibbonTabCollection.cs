// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.RibbonTabCollection
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace Intermech.MaterialsHandbook;

public sealed class RibbonTabCollection : List<RibbonTab>
{
  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public Ribbon Owner { get; set; }

  internal RibbonTabCollection(Ribbon owner) => this.Owner = owner;

  public new void Add(RibbonTab item)
  {
    item.Owner = this.Owner;
    base.Add(item);
    this.Owner.OnRegionsChanged();
  }

  public new void AddRange(IEnumerable<RibbonTab> items)
  {
    foreach (RibbonTab ribbonTab in items)
      ribbonTab.Owner = this.Owner;
    base.AddRange(items);
    this.Owner.OnRegionsChanged();
  }

  public new void Insert(int index, RibbonTab item)
  {
    item.Owner = this.Owner;
    base.Insert(index, item);
    this.Owner.OnRegionsChanged();
  }

  public void Remove(RibbonTab context)
  {
    base.Remove(context);
    this.Owner.OnRegionsChanged();
  }

  public new int RemoveAll(Predicate<RibbonTab> predicate)
  {
    throw new ApplicationException("RibbonTabCollection.RemoveAll function is not supported");
  }

  public new void RemoveAt(int index)
  {
    base.RemoveAt(index);
    this.Owner.OnRegionsChanged();
  }

  public new void RemoveRange(int index, int count)
  {
    base.RemoveRange(index, count);
    this.Owner.OnRegionsChanged();
  }
}
