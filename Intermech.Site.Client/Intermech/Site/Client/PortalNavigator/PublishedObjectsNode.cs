// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PublishedObjectsNode
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Navigator.DB;
using Intermech.Navigator.Parts;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class PublishedObjectsNode : PublishTypeNode
{
  private List<long> _objectIDs;

  public PublishedObjectsNode(List<long> objectIDs, int objectType)
    : base(objectType)
  {
    this._objectIDs = objectIDs;
  }

  protected override List<PartSlot> CreateNonFolderSlots()
  {
    if (this.viewPart == null)
      this.viewPart = (INodePart) new ContainsPart(this.Services, (IConditionsProvider) new ObjectsListConditionsProvider(this._objectIDs), this.typeID);
    return this.SlotsFromSinglePart(this.viewPart);
  }

  protected override List<PartSlot> CreateFolderSlots() => (List<PartSlot>) null;
}
