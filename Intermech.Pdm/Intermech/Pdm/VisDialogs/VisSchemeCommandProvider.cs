// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.VisDialogs.VisSchemeCommandProvider
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.DataFormats;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.Pdm.VisDialogs;

internal class VisSchemeCommandProvider : ICommandsProvider
{
  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    ViewStateFlags viewStateFlags = viewServices.GetService(typeof (IViewState)) is IViewState service ? service.ViewState : ViewStateFlags.None;
    if ((viewStateFlags & ViewStateFlags.InDialog) != ViewStateFlags.None || (viewStateFlags & ViewStateFlags.ReadOnly) != ViewStateFlags.None || items.Count != 1)
      return CommandsInfo.Empty;
    CommandsInfo mergedCommands = new CommandsInfo();
    mergedCommands.Add("EditDocument", new CommandInfo(0, new ClickEventHandler(VisSchemeCommandProvider.VisSchemeEditDocumentCommand)));
    return mergedCommands;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }

  public static void VisSchemeEditDocumentCommand(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (!(items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData))
      return;
    VisSchemeEditor visSchemeEditor = new VisSchemeEditor();
    visSchemeEditor.SchemeID = itemData.ObjectID;
    visSchemeEditor.LoadObjectData(0);
    int num = (int) visSchemeEditor.ShowDialog();
  }
}
