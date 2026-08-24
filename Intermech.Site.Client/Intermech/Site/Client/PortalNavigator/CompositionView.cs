// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.CompositionView
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Localization;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.VirtualNodes;
using System;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class CompositionView : PublishedObjectsView
{
  private int _imageIndex = -1;
  private int _viewIndex = 1;
  private string _viewName = LocalizationHolder.rm.GetString("Site.Client_8");

  public CompositionView()
  {
    INamedImageList service = (INamedImageList) ServicesManager.GetService(typeof (INamedImageList));
    if (service != null)
      this._imageIndex = service.ImageIndex("imgContains");
    this.Options |= ChildrenViewOptions.DisablePathProcessing;
  }

  public override void Initialize(ISelectedItems items, IServiceProvider provider)
  {
    INodeID itemId = items.GetItemID(0);
    if (itemId == null || !(itemId is PublishedObjectNodeID))
      return;
    base.Initialize(items, provider);
  }

  public override string Caption => this._viewName;

  public override int OrderID => this._viewIndex;

  public override int ImageIndex => this._imageIndex;

  protected override IDescriptor GetEmptyPathDescriptor()
  {
    return (IDescriptor) new HiveDescriptor(SiteClientConsts.CategoryContains, 0, "ContainsNodeDescriptor");
  }
}
