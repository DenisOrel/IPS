// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.EditDocumentCommand
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Commands;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.MRP2;

/// <summary>команда меню редактировать</summary>
internal class EditDocumentCommand
{
  internal static void EditDocumentHandler(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items == null || items.Count == 0)
      return;
    ObjectCopyCommand checkoutCommand = ObjectCommandFactory.CreateCheckoutCommand(true);
    for (int index = 0; index < items.Count; ++index)
    {
      if (items.GetItemData(index, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData && MetaDataHelper.IsObjectTypeChildOf(itemData.ObjectType, MRP2Consts.objtypeIdProductionLists))
      {
        checkoutCommand.ObjectId = itemData.ObjectID;
        checkoutCommand.Execute();
        Intermech.Navigator.ContextMenu.Services.InvokeCommand("OpenInNewWindow", Intermech.Navigator.ContextMenu.Services.GetCommandsTable(Intermech.Navigator.ContextMenu.Services.GetItems(checkoutCommand.NewObjectId), viewServices, false), viewServices);
      }
    }
  }

  internal static void EditDocumentInTreeHandler(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items == null || items.Count == 0)
      return;
    ObjectCopyCommand checkoutCommand = ObjectCommandFactory.CreateCheckoutCommand(true);
    for (int index = 0; index < items.Count; ++index)
    {
      if (items.GetItemData(index, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData1 && MetaDataHelper.IsObjectTypeChildOf(itemData1.ObjectType, MRP2Consts.objtypeIdProductionLists))
      {
        checkoutCommand.ObjectId = itemData1.ObjectID;
        checkoutCommand.Execute();
        if (items.GetItemData(0, typeof (NavigatorTreeNode)) is NavigatorTreeNode itemData && itemData.Level != 1)
          Intermech.Navigator.ContextMenu.Services.InvokeCommand("OpenInNewWindow", Intermech.Navigator.ContextMenu.Services.GetCommandsTable(Intermech.Navigator.ContextMenu.Services.GetItems(checkoutCommand.NewObjectId), viewServices, false), viewServices);
      }
    }
  }
}
