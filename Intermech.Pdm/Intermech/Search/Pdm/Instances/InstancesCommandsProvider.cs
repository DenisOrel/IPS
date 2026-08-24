// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.Instances.InstancesCommandsProvider
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.DataFormats;
using Intermech.Interfaces.Pdm;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.Search.Pdm.Instances;

public sealed class InstancesCommandsProvider : ICommandsProvider
{
  private LazyService<IInstancesClientService> _instancesClientService = new LazyService<IInstancesClientService>();

  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    if (viewServices == null)
      throw new ArgumentNullException(nameof (viewServices));
    return CommandsInfo.Empty;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    if (viewServices == null)
      throw new ArgumentNullException(nameof (viewServices));
    CommandsInfo groupCommands = new CommandsInfo();
    IDBTypedObjectID typedObjectID = (IDBTypedObjectID) null;
    if (this.CheckSelectedItemsForCreateInstances(items, out typedObjectID))
      groupCommands.Add("Create.Instances", new CommandInfo(-1, new ClickEventHandler(this.CreateInstances)));
    return groupCommands;
  }

  private void CreateInstances(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    IDBTypedObjectID typedObjectID = (IDBTypedObjectID) null;
    if (!this.CheckSelectedItemsForCreateInstances(items, out typedObjectID))
      throw new ArgumentException();
    this._instancesClientService.Value.CreateInstances(typedObjectID.ObjectID);
  }

  private bool CheckSelectedItemsForCreateInstances(
    ISelectedItems selectedItems,
    out IDBTypedObjectID typedObjectID)
  {
    typedObjectID = (IDBTypedObjectID) null;
    return selectedItems.GetItemID(0).CategoryID == 1 && SelectedItemsHelper.TryGetSingleTypedObjectIDWithObjectVersionIDAndObjectTypeID(selectedItems, out typedObjectID) && InstancesHelper.CheckObjectTypeForCreateInstances(typedObjectID.ObjectType);
  }
}
