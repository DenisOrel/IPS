// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PublishCompositionDescriptor
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces.WebPortal;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Persistence;
using Intermech.Navigator.VirtualNodes;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Site.Client;

internal class PublishCompositionDescriptor : HiveDescriptor
{
  private List<PublishCompositionObject> _publishObjects;

  public PublishCompositionDescriptor(PersistentState state)
    : base(state)
  {
  }

  public PublishCompositionDescriptor(List<PublishCompositionObject> objectIDs)
    : base(SiteClientConsts.CategoryRootListPublishObjects, 0, "Публикуемые объекты")
  {
    this._publishObjects = objectIDs;
  }

  public override INode GetChild(INodeID nodeID)
  {
    return (INode) new PublishCompositionNode(this._publishObjects);
  }
}
