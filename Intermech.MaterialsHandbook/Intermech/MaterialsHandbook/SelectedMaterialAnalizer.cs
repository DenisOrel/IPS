// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.SelectedMaterialAnalizer
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.DataFormats;
using Intermech.Imbase;
using Intermech.Imbase.Views;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.MaterialsHandbook;

internal class SelectedMaterialAnalizer : SelectedItemsAnalyzer
{
  public override SelectedItemsAnalyzerResult Analyze(
    ISelectionWindow sender,
    ISelectedItemsHost itemsHost)
  {
    SelectedItemsAnalyzerResult itemsAnalyzerResult = SelectedItemsAnalyzerResult.Disabled;
    if (base.Analyze(sender, itemsHost) == SelectedItemsAnalyzerResult.Enabled)
    {
      if (itemsHost.SelectedItems is IMHView.IMHSelectedItems selectedItems)
      {
        if (selectedItems.Selectable && selectedItems.GetItemData(0, (Type) null) is IMHMaterialRecordID itemData1)
          itemsAnalyzerResult = (itemData1.ID == 0L ? 0 : (itemData1.Value > -1L ? 1 : 0)) != 0 ? SelectedItemsAnalyzerResult.Enabled : SelectedItemsAnalyzerResult.Disabled;
      }
      else if (itemsHost.SelectedItems.GetItemData(0, typeof (IImbaseTableRecordID)) is IImbaseTableRecordID)
      {
        if (!(itemsHost is ImbaseTableView imbaseTableView))
          return itemsAnalyzerResult;
        itemsAnalyzerResult = imbaseTableView.TblView.DisabledRecord() ? SelectedItemsAnalyzerResult.Disabled : SelectedItemsAnalyzerResult.Enabled;
      }
      else if (itemsHost.SelectedItems.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData2)
      {
        if (itemData2.ObjectType == Intermech.Imbase.Consts.ImbaseTableRefTypeID || itemData2.ObjectType == Intermech.Imbase.Consts.ImbaseCatalogTypeID || itemData2.ObjectType == Intermech.Imbase.Consts.ImbaseFavoritesTypeID)
          itemsAnalyzerResult = SelectedItemsAnalyzerResult.Disabled;
        else if (itemData2.ObjectType == Intermech.Imbase.Consts.ImbaseFolderTypeID)
        {
          NavigatorTreeView tree = sender.Tree;
          if (tree != null)
          {
            NodeIDPath parentPath = itemsHost.SelectedItems.GetParentPath(0);
            INodeID itemId = itemsHost.SelectedItems.GetItemID(0);
            if (parentPath != null && itemId != null)
            {
              NodeIDPath nodeIDPath = new NodeIDPath(parentPath, itemId);
              itemsAnalyzerResult = SelectedItemsAnalyzerResult.Disabled;
              NavigatorTreeNode lastNode = (NavigatorTreeNode) null;
              if (!tree.TryFind(nodeIDPath, out lastNode))
                lastNode = (NavigatorTreeNode) null;
              if (lastNode != null && !lastNode.HasChildren)
              {
                if (!lastNode.Full)
                  tree.RefreshNode(lastNode);
                if (!lastNode.HasChildren && (!(lastNode.Handler is FolderNode handler) || handler.TableRefIDs == null || handler.TableRefIDs.Count == 0))
                  itemsAnalyzerResult = SelectedItemsAnalyzerResult.Enabled;
              }
            }
          }
        }
        else
          itemsAnalyzerResult = itemData2.ObjectType > 0 ? SelectedItemsAnalyzerResult.Enabled : SelectedItemsAnalyzerResult.Disabled;
      }
    }
    return itemsAnalyzerResult;
  }
}
