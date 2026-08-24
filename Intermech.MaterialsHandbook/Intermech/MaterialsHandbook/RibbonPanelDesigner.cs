// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.RibbonPanelDesigner
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

#nullable disable
namespace Intermech.MaterialsHandbook;

internal class RibbonPanelDesigner : RibbonElementWithItemCollectionDesigner
{
  public override RibbonItemCollection Collection
  {
    get
    {
      return !(this.Component is RibbonPanel component) ? (RibbonItemCollection) null : component.Items;
    }
  }

  public override Ribbon Ribbon
  {
    get => !(this.Component is RibbonPanel component) ? (Ribbon) null : component.Owner;
  }
}
