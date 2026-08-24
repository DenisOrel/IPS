// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.TaskNode
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Navigator.DBObjectTypes;
using Intermech.Navigator.Parts;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

public class TaskNode(int objTypeID, AccessRights accessRights) : ObjectTypeNode(objTypeID, accessRights)
{
  protected override List<PartSlot> CreateNonFolderSlots()
  {
    return this.SlotsFromSinglePart((INodePart) new TaskPart(this.ObjTypeID, this.Services));
  }
}
