// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PortalRootNode
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces.Client;
using Intermech.Navigator;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class PortalRootNode : CompositeNode, INodeNotifications
{
  private bool _isAdmin;
  internal static List<IDescriptor> _descrs = new List<IDescriptor>();

  public PortalRootNode(bool isAdmin) => this._isAdmin = isAdmin;

  protected override List<PartSlot> CreateFolderSlots()
  {
    DescriptorCollection descriptors = new DescriptorCollection();
    for (int index = 0; index < RootNodeChildren.Descriptors.Count; ++index)
      descriptors.Add(RootNodeChildren.Descriptors[index]);
    if (this._isAdmin)
    {
      for (int index = 0; index < RootNodeChildren.AdminDescriptors.Count; ++index)
        descriptors.Add(RootNodeChildren.AdminDescriptors[index]);
    }
    return this.SlotsFromSinglePart((INodePart) new DescriptorsPart(descriptors, false));
  }

  public override INode GetChild(INodeID nodeID)
  {
    switch (nodeID)
    {
      case PacketTypeNodeID _:
        return base.GetChild(nodeID);
      case PublishTypeNodeID _:
        return (INode) new PublishTypeNode(((PublishTypeNodeID) nodeID).id);
      default:
        return base.GetChild(nodeID);
    }
  }

  public ProcessResult Process(NotificationEventArgs e, object AdditionalInfo)
  {
    return ProcessResult.None;
  }
}
