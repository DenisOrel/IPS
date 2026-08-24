// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.AutoPublishListNode
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.Navigator;
using Intermech.Navigator.DB;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using Intermech.Navigator.Selections;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Site.Client;

internal sealed class AutoPublishListNode : ObjectsDictNode
{
  public AutoPublishListNode()
    : base((Dictionary<int, List<long>>) null, false)
  {
    this._objectIDs = this.GetObjects();
  }

  private Dictionary<int, List<long>> GetObjects()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(PortalConsts.selectionAutoPublish);
      return (sessionKeeper.Session.GetCustomService(typeof (ISelectionsService)) as ISelectionsService).IncludedObjects((object) sessionKeeper.Session.SessionGUID, objectInfo.ObjectID);
    }
  }

  protected override List<PartSlot> CreateFolderSlots()
  {
    return new List<PartSlot>()
    {
      new PartSlot(Intermech.Navigator.Selections.Consts.SelectionsPartGuid, (INodePart) new DescriptorsPart(new DescriptorCollection()
      {
        {
          Intermech.Navigator.Selections.Consts.SelectionsDescriptorGuid,
          (IDescriptor) new HiveDescriptor(Intermech.Navigator.Selections.Consts.SelectionTypeID, (ITopBinding) new AutoPublishListBinding(this, this._objectIDs))
        }
      }, false))
    };
  }

  protected override List<PartSlot> CreateNonFolderSlots() => base.CreateNonFolderSlots();

  public new List<PartSlot> CreateNonFolderSlots(IConditionsProvider conditionProvider)
  {
    return base.CreateNonFolderSlots(conditionProvider);
  }

  public override INodeQuery GetQuery(ContentType content)
  {
    return (content & ContentType.Folders) > ContentType.None && (content & ContentType.NonFolders) > ContentType.None ? base.GetQuery(ContentType.NonFolders) : base.GetQuery(content);
  }
}
