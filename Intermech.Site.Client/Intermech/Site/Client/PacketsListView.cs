// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PacketsListView
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Navigator.Interfaces;
using Intermech.Navigator.VirtualNodes;
using Intermech.Site.Client.PortalNavigator;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Site.Client;

internal sealed class PacketsListView(ObjectsListControl viewObjectsList) : ObjectListView(viewObjectsList)
{
  protected override NodeColumnCollection GetColumns() => Intermech.Site.Client.PortalNavigator.Helper.GetPublishedPacketColumns();

  protected override HiveDescriptor GetListDescriptor(
    IServiceProvider viewServices,
    List<long> objectIDs,
    int typeID)
  {
    return (HiveDescriptor) new PacketsListDescriptor(objectIDs, typeID);
  }
}
