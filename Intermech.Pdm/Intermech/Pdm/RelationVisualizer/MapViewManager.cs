// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.MapViewManager
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Map;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

[Serializable]
public class MapViewManager(MapView v) : MapToolManager(v)
{
  private IServiceContainer _services;
  private INotificationService _notificationService;
  private IHotKeysManager _hotKeysManager;
  private readonly IIODispatcher _ioDispatcher = (IIODispatcher) new IODispatcher();
  private AdvancedServiceContainer _serviceContainer;

  public void InitializeServices()
  {
    this._hotKeysManager = ServicesManager.GetService(typeof (IHotKeysManager)) as IHotKeysManager;
    this._services = (IServiceContainer) new ServiceContainer();
    this._services.AddService(typeof (IViewState), (object) new ViewStateService());
    if (this._notificationService != null)
      this._services.AddService(typeof (INotificationService), (object) this._notificationService);
    ObjectsSelectionOptionsHolder serviceInstance = new ObjectsSelectionOptionsHolder(ObjectsSelectionOptions.ShowAllModifications);
    this._serviceContainer = new AdvancedServiceContainer();
    this._serviceContainer.AddService(typeof (ObjectsSelectionOptionsHolder), (object) serviceInstance);
  }

  protected bool VisViewKeyDown(MapInputEventArgs args)
  {
    bool control = args.Control;
    Keys key = args.Key;
    if (key == Keys.Delete)
    {
      this.View.EditDelete();
      return true;
    }
    if (control && key == Keys.A)
    {
      this.View.SelectAll();
      return true;
    }
    if (control && key == Keys.C)
    {
      this.View.EditCopy();
      return true;
    }
    if (control && key == Keys.X)
    {
      this.View.EditCut();
      return true;
    }
    if (control && key == Keys.V)
    {
      this.View.EditPaste();
      return true;
    }
    switch (key)
    {
      case Keys.Prior:
        if (args.Shift)
          this.View.ScrollPage(-1f, 0.0f);
        else
          this.View.ScrollPage(0.0f, -1f);
        return true;
      case Keys.Next:
        if (args.Shift)
          this.View.ScrollPage(1f, 0.0f);
        else
          this.View.ScrollPage(0.0f, 1f);
        return true;
      case Keys.End:
        RectangleF documentBounds1 = this.View.ComputeDocumentBounds();
        SizeF docExtentSize = this.View.DocExtentSize;
        PointF pointF = control ? new PointF(documentBounds1.X + documentBounds1.Width - docExtentSize.Width, documentBounds1.Y + documentBounds1.Height - docExtentSize.Height) : new PointF(documentBounds1.X + documentBounds1.Width - docExtentSize.Width, this.View.DocPosition.Y);
        this.View.DocPosition = new PointF(Math.Max(0.0f, pointF.X), Math.Max(0.0f, pointF.Y));
        return true;
      case Keys.Home:
        RectangleF documentBounds2 = this.View.ComputeDocumentBounds();
        this.View.DocPosition = control ? new PointF(documentBounds2.X, documentBounds2.Y) : new PointF(documentBounds2.X, this.View.DocPosition.Y);
        return true;
      case Keys.F2:
        this.View.EditEdit();
        return true;
      default:
        if (control && key == Keys.Z)
        {
          this.View.Undo();
          return true;
        }
        if (control && key == Keys.Y)
        {
          this.View.Redo();
          return true;
        }
        if (key != Keys.Escape)
          return false;
        if (this.View.CanSelectObjects())
          this.Selection.Clear();
        base.DoKeyDown();
        return true;
    }
  }

  public override void DoKeyDown()
  {
    MapInputEventArgs lastInput = this.LastInput;
    if (this.VisViewKeyDown(lastInput) || !(this.View.Selection.Primary is VisNode primary))
      return;
    List<IHotKeysCommand> commands = this._hotKeysManager[this.CombineKeys(lastInput)];
    if (commands == null || commands.Count <= 0)
      return;
    ISelectedItems items = ObjectExtensions.GetItems(new long[1]
    {
      primary.ObjId
    }, (IServiceProvider) this._serviceContainer);
    this.ExecuteMenuCommand(commands, items);
  }

  internal Keys CombineKeys(MapInputEventArgs args)
  {
    Keys key = args.Key;
    if (args.Alt)
      key |= Keys.Alt;
    if (args.Control)
      key |= Keys.Control;
    if (args.Shift)
      key |= Keys.Shift;
    return key;
  }

  private bool ExecuteMenuCommand(List<IHotKeysCommand> commands, ISelectedItems items)
  {
    if (commands == null || commands.Count == 0 || items == null)
      return false;
    CommandsTable commandsTable = Intermech.Navigator.ContextMenu.Services.GetCommandsTable(items, (IServiceProvider) this._services, false);
    string commandName = string.Empty;
    for (int index = 0; index < commands.Count; ++index)
    {
      if (commandsTable.Contains(commands[index].Command))
      {
        commandName = commands[index].Command;
        break;
      }
    }
    if (commandName == string.Empty)
      return false;
    Intermech.Navigator.ContextMenu.Services.InvokeCommand(commandName, commandsTable, (IServiceProvider) this._services);
    return true;
  }
}
