// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Client.NavigatorCommandProvider
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Commands;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using Intermech.Scripting.Common.DesignTime;
using Intermech.Scripting.Projects.DBScripts;
using Intermech.Scripting.Services;
using System;

#nullable disable
namespace Intermech.Scripting.Client;

internal sealed class NavigatorCommandProvider : ICommandsProvider
{
  private IScriptPadService ideService;
  private DBScriptRepository dbScriptRepository;

  public NavigatorCommandProvider(
    IScriptPadService ideService,
    DBScriptRepository dbScriptRepository)
  {
    if (ideService == null)
      throw new ArgumentNullException(nameof (ideService));
    if (dbScriptRepository == null)
      throw new ArgumentNullException(nameof (dbScriptRepository));
    this.ideService = ideService;
    this.dbScriptRepository = dbScriptRepository;
  }

  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    if (viewServices == null)
      throw new ArgumentNullException(nameof (viewServices));
    if (items.Count == 0)
      return CommandsInfo.Empty;
    CommandsInfo mergedCommands = new CommandsInfo();
    mergedCommands.Add("OpenDocument", new CommandInfo(0, new ClickEventHandler(this.OnOpenScriptInIDE)));
    mergedCommands.Add("ViewDocument", new CommandInfo(0, new ClickEventHandler(this.OnViewScriptInIDE)));
    mergedCommands.Add("EditDocument", new CommandInfo(0, new ClickEventHandler(this.OnEditScriptInIDE)));
    return mergedCommands;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    if (viewServices == null)
      throw new ArgumentNullException(nameof (viewServices));
    return CommandsInfo.Empty;
  }

  private void OnOpenScriptInIDE(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    for (int index = 0; index < items.Count; ++index)
    {
      long objectId = ((IDBTypedObjectID) items.GetItemData(index, typeof (IDBTypedObjectID))).ObjectID;
      bool initializeWhenEmpty = this.CanEditScript(objectId);
      this.ideService.OpenScriptInIDEWindow((ScriptProject) this.ideService.GetScriptProject(objectId, initializeWhenEmpty), new OpenInScriptPadParameters()
      {
        ReadOnlyMode = !initializeWhenEmpty
      });
    }
  }

  private void OnViewScriptInIDE(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    for (int index = 0; index < items.Count; ++index)
      this.ideService.OpenScriptInIDEWindow((ScriptProject) this.ideService.GetScriptProject(((IDBTypedObjectID) items.GetItemData(index, typeof (IDBTypedObjectID))).ObjectID), new OpenInScriptPadParameters()
      {
        ReadOnlyMode = true
      });
  }

  private void OnEditScriptInIDE(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    for (int index = 0; index < items.Count; ++index)
    {
      long num = ((IDBTypedObjectID) items.GetItemData(index, typeof (IDBTypedObjectID))).ObjectID;
      if (!this.CanEditScript(num))
      {
        ObjectCopyCommand checkoutCommand = ObjectCommandFactory.CreateCheckoutCommand(true);
        checkoutCommand.ObjectId = num;
        checkoutCommand.Execute();
        num = checkoutCommand.NewObjectId;
      }
      this.ideService.OpenScriptInIDEWindow((ScriptProject) this.ideService.GetScriptProject(num, true), new OpenInScriptPadParameters()
      {
        ReadOnlyMode = false
      });
    }
  }

  private bool CanEditScript(long objectId)
  {
    if (objectId < 0L)
      return true;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (sessionKeeper.Session.GetObject(objectId, false).ObjectModifyMode == ObjectModifyModes.InBase)
        return true;
    }
    return false;
  }
}
