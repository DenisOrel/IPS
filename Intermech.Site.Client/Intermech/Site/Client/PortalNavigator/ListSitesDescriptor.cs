// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.ListSitesDescriptor
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.VirtualNodes;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class ListSitesDescriptor : HiveDescriptor
{
  public ListSitesDescriptor()
    : base(SiteClientConsts.CategoryListSites, 0, string.Empty)
  {
    this._caption = MetaDataHelper.GetObjectTypeName(PortalConsts.objtypeSites);
  }

  public override INode GetChild(INodeID nodeID) => (INode) new ListSitesNode();
}
