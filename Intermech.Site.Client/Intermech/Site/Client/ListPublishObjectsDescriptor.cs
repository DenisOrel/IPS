// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.ListPublishObjectsDescriptor
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Persistence;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Site.Client;

internal sealed class ListPublishObjectsDescriptor : DictDescriptor
{
  public ListPublishObjectsDescriptor(PersistentState state)
    : base(state)
  {
  }

  public ListPublishObjectsDescriptor(
    int categoryID,
    int typeID,
    string caption,
    Dictionary<int, List<long>> objectIDs)
    : base(categoryID, typeID, caption, objectIDs)
  {
  }

  public override INode GetChild(INodeID nodeID)
  {
    return (INode) new ListPublishObjectsNode(this._objectIDs);
  }
}
