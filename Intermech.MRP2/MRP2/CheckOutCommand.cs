// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.CheckOutCommand
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Client.Core.FormDesigner.Controls;
using Intermech.Commands;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Navigator.Controls;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MRP2;

internal class CheckOutCommand
{
  /// <summary>
  /// При взятии на редактирование производственных копий,
  /// автоматом делается следующее: для производвтсвенной копии ПК1 создается новая производственная копия объекта ПК1(1), которая копирует св-ва и состав первого уровня от "прототипа"
  /// если ПК1 входит входит в ПК2 и она не взята на редактирование, то для ПК2 также создается новая копия ПК2(1),
  /// в которой связь с ПК1 заменяется на связь ПК1(1),
  /// далее если ПК2 входит в производственную ведомость ПВ, то ПВ также берется на редактирование (если не взята) и связь ПВ-&gt;ПК2 меняется на ПВ-&gt;ПК2(1)
  /// </summary>
  /// <param name="items"></param>
  /// <param name="viewServices"></param>
  /// <param name="additionalInfo"></param>
  public static void Handler(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items == null || items.Count == 0)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      NavigatorTreeView service1 = viewServices.GetService(typeof (NavigatorTreeView)) as NavigatorTreeView;
      string[] addr;
      if (items.GetItemData(0, typeof (NavigatorTreeNode)) is NavigatorTreeNode itemData)
      {
        CheckOutCommand.CheckOutTreeNode(sessionKeeper.Session, itemData, out addr);
      }
      else
      {
        ChildrenView service2 = viewServices.GetService(typeof (ChildrenView)) as ChildrenView;
        if (service1 != null && service1.RootNodeID is NodeID rootNodeId1 && MetaDataHelper.IsObjectTypeChildOf(rootNodeId1.ObjectTypeID, MRP2Consts.objtypeIdProductionLists) && service2 != null && service2.GetType() == typeof (CompositionView))
        {
          CheckOutCommand.CheckOutTreeNode(sessionKeeper.Session, service1.FocusedNode, items, out addr);
        }
        else
        {
          DesForm service3 = viewServices.GetService(typeof (DesForm)) as DesForm;
          NodeID itemId = items.GetItemID(0) as NodeID;
          NodeID nodeId = service1?.FocusedNode?.NodeID as NodeID;
          if (service1 == null || !(service1.RootNodeID is NodeID rootNodeId) || !MetaDataHelper.IsObjectTypeChildOf(rootNodeId.ObjectTypeID, MRP2Consts.objtypeIdProductionLists) || service3 == null || nodeId == null || itemId == null || nodeId.ObjectID != itemId.ObjectID)
            throw new NotificationException("Эту команду можно выполнить только в отдельном окне в контексте состава производственной ведомости");
          CheckOutCommand.CheckOutTreeNode(sessionKeeper.Session, service1.FocusedNode, out addr);
        }
      }
      string path = string.Join("\\", addr);
      service1.RefreshNode(service1.RootNode);
      service1.Browse(path);
      if (!(viewServices.GetService(typeof (INavigatorTreeViewContextMenuHelper)) is INavigatorTreeViewContextMenuHelper service4))
        return;
      service4.CanRestoreFocusedNode = false;
    }
  }

  internal static void CheckOutTreeNode(
    IUserSession session,
    NavigatorTreeNode node,
    ISelectedItems items,
    out string[] addr)
  {
    INodeID[] path = node.GetPath();
    addr = new string[path.Length];
    if (!(path[0] is NodeID nodeId1) || !MetaDataHelper.IsObjectTypeChildOf(nodeId1.ObjectTypeID, MRP2Consts.objtypeIdProductionLists))
      throw new NotificationException("Эту команду можно выполнить только в отдельном окне в контексте состава производственной ведомости");
    CheckOutCommand.CheckEditPath(session, path, addr);
    List<long> newObjectIDs = new List<long>();
    List<Guid> relGuidsList = new List<Guid>();
    for (int index = 0; index < items.Count; ++index)
    {
      if (items.GetItemData(index, typeof (IDBRelationID)) is IDBRelationID itemData)
      {
        IDBCheckedOutByID itemData1 = items.GetItemData(index, typeof (IDBCheckedOutByID)) as IDBCheckedOutByID;
        IDBTypedObjectID itemData2 = items.GetItemData(index, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
        if (itemData1 != null && itemData2 != null && itemData1.CheckedOutBy == 0L)
        {
          relGuidsList.Add(itemData.RelGuid);
          long withReplacedPart = MRP2Service.CreateProductionCopyWithReplacedPart(session, itemData1.ObjectID, itemData2.ObjectType, Guid.Empty, 0L, true, out Guid _, out Dictionary<Guid, Guid> _);
          newObjectIDs.Add(withReplacedPart);
        }
      }
    }
    MRP2Service.DisabledEvents = true;
    try
    {
      for (int index = path.Length - 1; index >= 0; --index)
      {
        if (path[index] is NodeID _)
        {
          NodeID nodeId2 = path[index] as NodeID;
          if (nodeId2.CheckedOutBy != session.UserID)
          {
            if (index == 0)
            {
              ObjectCopyCommand checkoutCommand = ObjectCommandFactory.CreateCheckoutCommand(true);
              checkoutCommand.ObjectId = nodeId2.ObjectID;
              checkoutCommand.Execute();
              _do_replace_link(checkoutCommand.NewObjectId);
              addr[index] = nodeId2.ObjectID.ToString();
            }
            else
            {
              long productionCopy = MRP2Service.CreateProductionCopy(session, nodeId2.ObjectID, nodeId2.ObjectTypeID, relGuidsList, newObjectIDs, true, out List<Guid> _);
              newObjectIDs.Clear();
              newObjectIDs.Add(productionCopy);
              relGuidsList.Clear();
              relGuidsList.Add(nodeId2.RelGuid);
              addr[index] = productionCopy.ToString();
            }
          }
          else
          {
            _do_replace_link(nodeId2.ObjectID);
            addr[index] = nodeId2.ObjectID.ToString();
            break;
          }
        }
      }
    }
    finally
    {
      MRP2Service.DisabledEvents = false;
    }

    void _do_replace_link(long projObjID)
    {
      for (int index = 0; index < newObjectIDs.Count; ++index)
        MRP2Service.ReplaceLink(session, projObjID, relGuidsList[index], newObjectIDs[index]);
    }
  }

  internal static void CheckOutTreeNode(
    IUserSession session,
    NavigatorTreeNode node,
    out string[] addr)
  {
    INodeID[] path = node.GetPath();
    addr = new string[path.Length];
    if (!(path[0] is NodeID nodeId1) || !MetaDataHelper.IsObjectTypeChildOf(nodeId1.ObjectTypeID, MRP2Consts.objtypeIdProductionLists))
      throw new NotificationException("Эту команду можно выполнить только в отдельном окне в контексте состава производственной ведомости");
    CheckOutCommand.CheckEditPath(session, path, addr);
    Guid replacedRelation = Guid.Empty;
    long num = 0;
    MRP2Service.DisabledEvents = true;
    try
    {
      for (int index = path.Length - 1; index >= 0; --index)
      {
        if (path[index] is NodeID _)
        {
          NodeID nodeId2 = path[index] as NodeID;
          if (nodeId2.CheckedOutBy != session.UserID)
          {
            if (index == 0)
            {
              ObjectCopyCommand checkoutCommand = ObjectCommandFactory.CreateCheckoutCommand(true);
              checkoutCommand.ObjectId = nodeId2.ObjectID;
              checkoutCommand.Execute();
              MRP2Service.ReplaceLink(session, checkoutCommand.NewObjectId, replacedRelation, num);
              addr[index] = nodeId2.ObjectID.ToString();
            }
            else
            {
              num = MRP2Service.CreateProductionCopyWithReplacedPart(session, nodeId2.ObjectID, nodeId2.ObjectTypeID, replacedRelation, num, true, out Guid _, out Dictionary<Guid, Guid> _);
              replacedRelation = nodeId2.RelGuid;
              addr[index] = num.ToString();
            }
          }
          else
          {
            MRP2Service.ReplaceLink(session, nodeId2.ObjectID, replacedRelation, num);
            addr[index] = nodeId2.ObjectID.ToString();
            break;
          }
        }
      }
    }
    finally
    {
      MRP2Service.DisabledEvents = false;
    }
  }

  private static void CheckEditPath(IUserSession session, INodeID[] path, string[] addr)
  {
    for (int index = path.Length - 1; index >= 0; --index)
    {
      if (path[index] is NodeID nodeId)
      {
        if (addr != null)
          addr[index] = nodeId.ObjectID.ToString();
        if (nodeId.CheckedOutBy != 0L && nodeId.CheckedOutBy != session.UserID)
          session.GetObject(nodeId.ObjectID).CheckEdit();
      }
    }
  }
}
