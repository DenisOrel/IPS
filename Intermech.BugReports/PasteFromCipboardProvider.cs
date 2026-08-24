// Decompiled with JetBrains decompiler
// Type: Intermech.BugReports.PasteFromCipboardProvider
// Assembly: Intermech.BugReports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 16F80F46-2B9D-4747-9BFD-4CC209192F4E
// Assembly location: D:\IPS\Client\Intermech.BugReports.dll

using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.BugReports;

internal class PasteFromCipboardProvider : ICommandsProvider
{
  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    if (items.Count == 1)
    {
      if (((viewServices.GetService(typeof (IViewState)) is IViewState service ? (long) service.ViewState : 0L) & 2L) == 0L & (Bitmap) Clipboard.GetDataObject().GetData(DataFormats.Bitmap) != null)
      {
        CommandsInfo groupCommands = new CommandsInfo();
        groupCommands.Add("PasteFromClipboard", new CommandInfo(0, new ClickEventHandler(PasteFromClipboardMenuCommands.Paste)));
        return groupCommands;
      }
    }
    return CommandsInfo.Empty;
  }
}
