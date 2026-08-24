// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.DocumentsMenuProvider
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.MRP2;

internal class DocumentsMenuProvider : ICommandsProvider
{
  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    if (!(items.GetParentData(0, typeof (IDBObjectTypeID)) is IDBObjectTypeID parentData) || !MetaDataHelper.IsObjectTypeChildOf(parentData.Value, MRP2Consts.objtypeIdProductionObjects))
      return CommandsInfo.Empty;
    CommandsInfo mergedCommands = new CommandsInfo();
    mergedCommands.Add("MRP2.HideDeleted", new CommandInfo(0, new ClickEventHandler(HideDeletePositionsCommand.Handler), !HideDeletePositionsCommand.checkedState ? new ContextMenuItemState(ContextMenuCheckState.Unchecked) : new ContextMenuItemState(ContextMenuCheckState.Checked)));
    mergedCommands.Add("Exclude", new CommandInfo(0, new ClickEventHandler(ExcludeSostavCommand.Handler)));
    mergedCommands.Add("MRP2.ReplaceVersion", new CommandInfo(0, new ClickEventHandler(ReplaceVersionCommand.DocHandler)));
    return mergedCommands;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }
}
