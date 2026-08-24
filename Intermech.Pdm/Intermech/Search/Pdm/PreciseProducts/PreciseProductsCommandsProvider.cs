// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.PreciseProducts.PreciseProductsCommandsProvider
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Client;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Controls;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.Search.Pdm.PreciseProducts;

public sealed class PreciseProductsCommandsProvider : ICommandsProvider
{
  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    if (viewServices == null)
      throw new ArgumentNullException(nameof (viewServices));
    CommandsInfo groupCommands = new CommandsInfo();
    long relationID = 0;
    long productVersionID = 0;
    if (this.CheckParamsForCreatePreciseProduct(items, viewServices, out relationID, out productVersionID))
      groupCommands.Add("CreatePreciseProduct", new CommandInfo(0, new ClickEventHandler(this.CreatePreciseProduct)));
    return groupCommands;
  }

  private void CreatePreciseProduct(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    if (viewServices == null)
      throw new ArgumentNullException(nameof (viewServices));
    long relationID = 0;
    long productVersionID = 0;
    if (!this.CheckParamsForCreatePreciseProduct(items, viewServices, out relationID, out productVersionID))
      throw new ArgumentException();
    if (!(ServicesManager.GetService(typeof (IPreciseProductsClientService)) is IPreciseProductsClientService service))
      return;
    service.CreatePreciseProduct(relationID, productVersionID);
  }

  private bool CheckParamsForCreatePreciseProduct(
    ISelectedItems selectedItems,
    IServiceProvider serviceProvider,
    out long relationID,
    out long productVersionID)
  {
    relationID = 0L;
    productVersionID = 0L;
    if (!(serviceProvider.GetService(typeof (IViewState)) is IViewState service) || service.ViewState != ViewStateFlags.NodeInTree || !(selectedItems.GetItemData(0, typeof (NavigatorTreeNode)) is NavigatorTreeNode itemData))
      return false;
    NavigatorTreeNode parent = itemData.Parent;
    if (parent == null)
      return false;
    NodeID nodeId1 = itemData.NodeID as NodeID;
    NodeID nodeId2 = parent.NodeID as NodeID;
    if (nodeId1 == null || nodeId2 == null)
      return false;
    CommandsInfo commandsInfo = new CommandsInfo();
    if (!PreciseProductsHelper.IsObjectTypeSuitableForCreatePreciseProduct(nodeId1.ObjectTypeID) || nodeId2.ObjectTypeID != PreciseProductsConstants.OrderObjectTypeID && nodeId2.ObjectTypeID != PreciseProductsConstants.ComplementObjectTypeID)
      return false;
    relationID = nodeId1.PrjLinkID;
    productVersionID = nodeId1.ObjectID;
    return true;
  }
}
