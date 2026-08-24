// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.ObjectListView
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using Intermech.Navigator.VirtualNodes;
using Intermech.Site.Client.PortalNavigator;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Site.Client;

internal class ObjectListView
{
  protected ObjectsListControl control;
  protected IPortalMetadata metadata;

  public ObjectListView(ObjectsListControl viewObjectsList)
  {
    this.control = viewObjectsList;
    this.metadata = (IPortalMetadata) ServicesManager.GetService(typeof (IPortalMetadata));
  }

  public void InitView(IServiceProvider viewServices, List<long> objectIDs, List<int> types)
  {
    this.InitControl(viewServices, objectIDs, types);
  }

  public void InitView(
    ISelectedItems items,
    IServiceProvider viewServices,
    out List<long> objectIDs)
  {
    List<int> types = new List<int>();
    objectIDs = new List<long>();
    for (int index = 0; index < items.Count; ++index)
    {
      if (items.GetItemData(index, typeof (IPublishTypedID)) is IPublishTypedID itemData && !objectIDs.Contains(itemData.ObjectID))
      {
        objectIDs.Add(itemData.ObjectID);
        if (types.IndexOf(itemData.TypeID) < 0)
          types.Add(itemData.TypeID);
      }
    }
    this.InitControl(viewServices, objectIDs, types);
  }

  protected void InitControl(IServiceProvider viewServices, List<long> objectIDs)
  {
    this.InitControl(viewServices, objectIDs, (List<int>) null);
  }

  protected void InitControl(IServiceProvider viewServices, List<long> objectIDs, List<int> types)
  {
    int onlyType = this.GetOnlyType(types);
    this.control.Initialize((IDescriptor) this.GetListDescriptor(viewServices, objectIDs, onlyType), viewServices);
    this.control.SetColumns(this.GetColumns(), false);
    this.control.Activate((IView) null);
  }

  protected virtual NodeColumnCollection GetColumns() => Intermech.Site.Client.PortalNavigator.Helper.GetPublishedObjectColumns();

  protected virtual HiveDescriptor GetListDescriptor(
    IServiceProvider viewServices,
    List<long> objectIDs,
    int typeID)
  {
    return (HiveDescriptor) new PublishedObjectsDescriptor(objectIDs, typeID);
  }

  private int GetOnlyType(List<int> types)
  {
    return types == null || types.Count != 1 ? ((IPortalMetadata) ServicesManager.GetService(typeof (IPortalMetadata))).GetPublishObjectType(PortalConsts.objtypePublishObjects).ID : types[0];
  }
}
