// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.AutoPublishListDescriptor
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Navigator.Interfaces;
using Intermech.Navigator.VirtualNodes;

#nullable disable
namespace Intermech.Site.Client;

internal sealed class AutoPublishListDescriptor : HiveDescriptor
{
  public AutoPublishListDescriptor()
    : base(Intermech.Navigator.Consts.CategoryVersionsObjectNode, 0, SiteClientConsts.AutoPublishOblectsListCaption)
  {
  }

  public override INode GetChild(INodeID nodeID) => (INode) new AutoPublishListNode();
}
