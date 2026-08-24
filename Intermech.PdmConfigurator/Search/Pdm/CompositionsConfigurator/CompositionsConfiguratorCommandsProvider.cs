// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.CompositionsConfigurator.CompositionsConfiguratorCommandsProvider
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Controls;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Search.UI;
using Intermech.Search.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Pdm.CompositionsConfigurator;

public sealed class CompositionsConfiguratorCommandsProvider : ICommandsProvider
{
  private LazyService<ICompositionsConfiguratorClientService> _compositionsConfiguratorClientService = new LazyService<ICompositionsConfiguratorClientService>();

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
    IDBRelationID relationID = (IDBRelationID) null;
    if (this.CheckParamsForCopyApplicationConditions(items, viewServices, out relationID))
      groupCommands.Add("CompositionsConfigurator.CopyApplicationConditions", new CommandInfo(-1, new ClickEventHandler(this.CopyApplicationConditions)));
    IDBRelationID[] relationIds = (IDBRelationID[]) null;
    if (this.CheckParamsForPasteApplicationConditions(items, viewServices, out relationIds))
      groupCommands.Add("CompositionsConfigurator.PasteApplicationConditions", new CommandInfo(-1, new ClickEventHandler(this.PasteApplicationConditions)));
    NodeID[] nodeIds = (NodeID[]) null;
    if (this.CheckParamsForPasteApplicationConditionsToAllInstances(items, viewServices, out nodeIds))
      groupCommands.Add("CompositionsConfigurator.PasteApplicationConditionsToAllInstances", new CommandInfo(-1, new ClickEventHandler(this.PasteApplicationConditionsToAllInstances)));
    return groupCommands;
  }

  private void CopyApplicationConditions(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    if (viewServices == null)
      throw new ArgumentNullException(nameof (viewServices));
    IDBRelationID relationID = (IDBRelationID) null;
    if (!this.CheckParamsForCopyApplicationConditions(items, viewServices, out relationID))
      throw new ArgumentException();
    this._compositionsConfiguratorClientService.Value.CopyApplicationConditionsToClipboard(relationID.Value);
  }

  private void PasteApplicationConditions(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    if (viewServices == null)
      throw new ArgumentNullException(nameof (viewServices));
    IDBRelationID[] relationIds = (IDBRelationID[]) null;
    if (!this.CheckParamsForPasteApplicationConditions(items, viewServices, out relationIds))
      throw new ArgumentException();
    if (relationIds.Length == 0)
      return;
    long projId = relationIds[0].ProjID;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(projId);
      if (dbObject.ObjectModifyMode == ObjectModifyModes.Checkout)
      {
        if (dbObject.CheckoutBy != sessionKeeper.Session.UserID)
        {
          int num = (int) MessageBox.Show($"Объект #{projId} '{dbObject.Caption}' не взят на изменение. Перед выполнением команды необходимо взять объект на изменение.", "Intermech Professional Solution", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return;
        }
      }
    }
    this._compositionsConfiguratorClientService.Value.PasteApplicationConditionsFromClipboard(((IEnumerable<IDBRelationID>) relationIds).Select<IDBRelationID, long>((Func<IDBRelationID, long>) (o => o.Value)));
    NavigatorTreeView service1 = viewServices.GetService(typeof (NavigatorTreeView)) as NavigatorTreeView;
    PageViewsManager service2 = viewServices.GetService(typeof (PageViewsManager)) as PageViewsManager;
    if (service1 == null || service1.SelectedItems == null || service2 == null)
      return;
    service2.UpdateViews(service1.SelectedItems, true);
  }

  private void PasteApplicationConditionsToAllInstances(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    if (viewServices == null)
      throw new ArgumentNullException(nameof (viewServices));
    NodeID[] nodeIds = (NodeID[]) null;
    if (!this.CheckParamsForPasteApplicationConditionsToAllInstances(items, viewServices, out nodeIds))
      throw new ArgumentException();
    if (nodeIds.Length == 0)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      using (NotificationContext.Create(sessionKeeper.Session))
        ((ICompositionConfiguratorServerService) sessionKeeper.Session.GetCustomService(typeof (ICompositionConfiguratorServerService))).CopyApplicationConditionsToAllInstances(sessionKeeper.Session.SessionGUID, ((IEnumerable<NodeID>) nodeIds).Select<NodeID, Tuple<long, long, long>>((Func<NodeID, Tuple<long, long, long>>) (o => new Tuple<long, long, long>(o.ProjID, o.PrjLinkID, o.ID))).ToArray<Tuple<long, long, long>>());
    }
    int num = (int) MessageBox.Show("Вставка условий применения успешно завершена.", "Intermech Professional Solution", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
  }

  private bool CheckParamsForCopyApplicationConditions(
    ISelectedItems selectedItems,
    IServiceProvider serviceProvider,
    out IDBRelationID relationID)
  {
    relationID = (IDBRelationID) null;
    IDBTypedObjectID typedObjectID = (IDBTypedObjectID) null;
    return this.IsEnabledPdmConfigurator() && SelectedItemsHelper.TryGetSingleTypedObjectIDWithObjectVersionIDAndObjectTypeID(selectedItems, out typedObjectID) && SelectedItemsHelper.TryGetSingleRelationIDWithRelationIDAndRelationTypeID(selectedItems, out relationID) && CompositionsConfiguratorHelper.IsConfigurableRelationTypeID(relationID.RelationType);
  }

  private bool CheckParamsForPasteApplicationConditions(
    ISelectedItems selectedItems,
    IServiceProvider serviceProvider,
    out IDBRelationID[] relationIds)
  {
    relationIds = (IDBRelationID[]) null;
    IDBTypedObjectID[] typedObjectIds = (IDBTypedObjectID[]) null;
    return this.IsEnabledPdmConfigurator() && SelectedItemsHelper.TryGetTypedObjectIdsWithObjectVersionIdsAndObjectTypeIds(selectedItems, out typedObjectIds) && SelectedItemsHelper.TryGetRelationIdsWithRelationIdsAndRelationTypeIdsAndCommonNotUnknownProjectID(selectedItems, out relationIds) && CompositionsConfiguratorHelper.IsAllConfigurableRelationTypeIds(((IEnumerable<IDBRelationID>) relationIds).Select<IDBRelationID, int>((Func<IDBRelationID, int>) (o => o.RelationType)));
  }

  private bool CheckParamsForPasteApplicationConditionsToAllInstances(
    ISelectedItems selectedItems,
    IServiceProvider serviceProvider,
    out NodeID[] nodeIds)
  {
    nodeIds = (NodeID[]) null;
    return this.IsEnabledPdmConfigurator() && SelectedItemsHelper.TryGetObjectNodeIdsWithObjectVersionIDAndObjectTypeID(selectedItems, out nodeIds) && !RelationHelper.IsAnyUnknownRelationID(((IEnumerable<NodeID>) nodeIds).Select<NodeID, long>((Func<NodeID, long>) (o => o.PrjLinkID))) && !RelationTypeHelper.IsAnyUnknownRelationTypeID(((IEnumerable<NodeID>) nodeIds).Select<NodeID, int>((Func<NodeID, int>) (o => o.RelationTypeID))) && CompositionsConfiguratorHelper.IsAllConfigurableRelationTypeIds(((IEnumerable<NodeID>) nodeIds).Select<NodeID, int>((Func<NodeID, int>) (o => o.RelationTypeID)));
  }

  private bool IsEnabledPdmConfigurator()
  {
    return ServicesManager.GetService(typeof (ICurrentUserAndRole)) is ICurrentUserAndRole service && service.EnabledPdmConfigurator;
  }
}
