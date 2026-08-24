// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PacketTypeNode
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using Intermech.Navigator.Selections;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal sealed class PacketTypeNode(int typeID) : PublishTypeNode(typeID)
{
  protected override List<PartSlot> CreateNonFolderSlots()
  {
    if (this.viewPart == null)
      this.viewPart = (INodePart) new PacketsPart(this.Services, this.typeID);
    return this.SlotsFromSinglePart(this.viewPart);
  }

  public override NodeColumnCollection GetDefaultColumns(ContentType content)
  {
    return content == ContentType.NonFolders ? Helper.GetPublishedPacketColumns() : base.GetDefaultColumns(content);
  }

  protected override ITopBinding GetBinding() => (ITopBinding) new PacketBinding(this.typeID);
}
