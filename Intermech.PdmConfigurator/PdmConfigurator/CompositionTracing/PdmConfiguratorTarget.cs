// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.CompositionTracing.PdmConfiguratorTarget
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.Bars;
using Intermech.Docking;
using Intermech.Interfaces.Client;
using Intermech.Navigator.Controls;

#nullable disable
namespace Intermech.PdmConfigurator.CompositionTracing;

public sealed class PdmConfiguratorTarget : ICommandTarget
{
  private ICurrentNavWindow _currentNavWindow;
  private DockManager _dockManager;

  public PdmConfiguratorTarget()
  {
    this._dockManager = ServicesManager.GetService(typeof (DockManager)) as DockManager;
    this._currentNavWindow = ServicesManager.GetService(typeof (ICurrentNavWindow)) as ICurrentNavWindow;
  }

  public bool Execute(ICommandState commandState) => false;

  public bool QueryStatus(ICommandState commandState)
  {
    string commandName = commandState.CommandName;
    if (commandName == string.Empty || !(commandName == "CompositionTracing") && !(commandName == "TracingProcess"))
      return false;
    NavWindowBase activeDocument = this._dockManager.ActiveDocument as NavWindowBase;
    commandState.Enabled = activeDocument != null && activeDocument.TreeView != null;
    return true;
  }
}
