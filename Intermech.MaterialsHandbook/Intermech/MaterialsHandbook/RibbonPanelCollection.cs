// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.RibbonPanelCollection
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace Intermech.MaterialsHandbook;

public sealed class RibbonPanelCollection : List<RibbonPanel>
{
  private RibbonTab _ownerTab;

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public Ribbon Owner
  {
    get => this._ownerTab.Owner;
    set
    {
      foreach (RibbonPanel ribbonPanel in (List<RibbonPanel>) this)
        ribbonPanel.Owner = value;
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public RibbonTab OwnerTab
  {
    get => this._ownerTab;
    set
    {
      this._ownerTab = value;
      foreach (RibbonPanel ribbonPanel in (List<RibbonPanel>) this)
        ribbonPanel.OwnerTab = value;
    }
  }

  public RibbonPanelCollection(RibbonTab ownerTab) => this._ownerTab = ownerTab;

  public new void Add(RibbonPanel item)
  {
    item.Owner = this.Owner;
    item.OwnerTab = this.OwnerTab;
    base.Add(item);
  }

  public new void AddRange(IEnumerable<RibbonPanel> items)
  {
    foreach (RibbonPanel ribbonPanel in items)
    {
      ribbonPanel.Owner = this.Owner;
      ribbonPanel.OwnerTab = this.OwnerTab;
    }
    base.AddRange(items);
  }

  public new void Insert(int index, RibbonPanel item)
  {
    item.Owner = this.Owner;
    item.OwnerTab = this.OwnerTab;
    base.Insert(index, item);
  }
}
