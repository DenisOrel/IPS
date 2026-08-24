// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.ExcludeAllDeletedCommand
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.DataFormats;
using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Kernel.Search;
using Intermech.Navigator.Controls;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Search.UI;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MRP2;

internal class ExcludeAllDeletedCommand
{
  /// <summary>
  /// Команда для удаления из состава ПВ всех исключенных позиций (отмеченных как удаленные)
  /// </summary>
  /// <param name="items"></param>
  /// <param name="viewServices"></param>
  /// <param name="additionalInfo"></param>
  internal static void Handler(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    INodeID nodeId1 = items.GetItemData(0, typeof (NavigatorTreeNode)) is NavigatorTreeNode itemData1 ? items.GetItemID(0) : throw new ApplicationException("Эту команду можно выполнить только в отдельном окне в контексте состава производственной ведомости");
    IDBObjectID itemData2 = items.GetItemData(0, typeof (IDBObjectID)) as IDBObjectID;
    if (nodeId1.CategoryID != 1 || !MetaDataHelper.IsObjectTypeChildOf(nodeId1.TypeID, MRP2Consts.objtypeIdProductionLists) && !MetaDataHelper.IsObjectTypeChildOf(nodeId1.TypeID, MRP2Consts.objtypeIdProductionCopy) || MessageBox.Show("Удалить из состава текущего элемента все позиции отмеченные как исключенные?", "Подтверждение", MessageBoxButtons.YesNo) != DialogResult.Yes)
      return;
    using (SessionKeeper sk = new SessionKeeper())
    {
      if (itemData1.NodeID is NodeID nodeId2 && nodeId2.CheckedOutBy == 0L)
        sk.Session.GetObject(nodeId2.ObjectID).CheckEdit();
      ExcludeAllDeletedCommand.CheckOutAllParents(itemData1, sk);
      DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[1]
      {
        new ConditionStructure(MRP2Consts.attrIdDeleteTag, RelationalOperators.Equal, (object) true, LogicalOperators.AND, 0, false)
      }, new ColumnDescriptor[1]
      {
        new ColumnDescriptor((object) ObligatoryObjectAttributes.F_PRJLINK_ID, AttributeSourceTypes.Relation, ColumnContents.Value, ColumnNameMapping.Index, SortOrders.NONE, 0)
      });
      DataTable dataTable = sk.Session.GetRelationCollection(MRP2Consts.reltypeIdProductComposition).ConsistFrom(paramSet, itemData2.Value);
      sk.Session.StartLogHistory();
      try
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        {
          long int64 = Convert.ToInt64(row[0]);
          sk.Session.GetRelation(int64, false)?.Delete(0L);
        }
        foreach (DataRow row in (InternalDataCollectionBase) sk.Session.GetRelationCollection(MRP2Consts.reltypeIdDocumentComposition).ConsistFrom(paramSet, itemData2.Value).Rows)
        {
          long int64 = Convert.ToInt64(row[0]);
          sk.Session.GetRelation(int64, false)?.Delete(0L);
        }
        NotificationHelper.Notify((object) null, sk.Session.GetModificationsHistoryList());
      }
      finally
      {
        sk.Session.StopLogHistory();
      }
      itemData1.Tree.RefreshNode(itemData1);
    }
  }

  private static void CheckOutAllParents(NavigatorTreeNode tNode, SessionKeeper sk)
  {
    NavigatorTreeNode navigatorTreeNode = tNode.ChildsEnumeration((System.Func<NavigatorTreeNode, bool>) (n =>
    {
      IDBRelation relation = sk.Session.GetRelation(n.GetData<IDBRelationID>(n.NodeID).Value, false);
      if (relation == null)
        return false;
      IDBAttribute attributeById = relation.GetAttributeByID(MRP2Consts.attrIdDeleteTag);
      return attributeById != null && attributeById.AsBoolean;
    }), (System.Func<NavigatorTreeNode, bool>) (n => true), true).FirstOrDefault<NavigatorTreeNode>((System.Func<NavigatorTreeNode, bool>) (n => (n.Parent.NodeID as NodeID).CheckedOutBy != sk.Session.UserID));
    if (navigatorTreeNode == null)
      return;
    CheckOutCommand.CheckOutTreeNode(sk.Session, navigatorTreeNode.Parent, out string[] _);
    tNode.Tree.RefreshNode(tNode);
    ExcludeAllDeletedCommand.CheckOutAllParents(tNode, sk);
  }
}
