// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.SettingObjectsView
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using Intermech.Navigator.ContextMenu.Extensions;
using Intermech.Navigator.Controls;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client;

public class SettingObjectsView : ObjectsViewBase, ICommandsFilter
{
  private static HashSet<string> _supportedCommands = new HashSet<string>((IEnumerable<string>) new string[8]
  {
    "ResetColumns",
    "SetupColumns",
    "ParametersCard",
    "Delete",
    "Refresh",
    "Create",
    "CreateNew",
    "Navigator.CreateObjectType"
  });
  private IContainer components;

  public SettingObjectsView()
  {
    this.InitializeComponent();
    this.OnGetMenuServiceContainer = new ChildrenView.GetMenuServiceContainerDelegate(this.GetMenuServiceContainer);
  }

  private IServiceContainer GetMenuServiceContainer(
    object sender,
    IServiceContainer originalMenuServiceContainer)
  {
    (originalMenuServiceContainer as ServiceContainer).StackLocalContextCommandsFilter((ICommandsFilter) this);
    return originalMenuServiceContainer;
  }

  public void FilterCommands(
    ISelectedItems items,
    IEnumerable<CommandAndVisibleStatus> commandWithVisibleStatuses)
  {
    foreach (CommandAndVisibleStatus andVisibleStatus in commandWithVisibleStatuses.Where<CommandAndVisibleStatus>((Func<CommandAndVisibleStatus, bool>) (commandAndStatus => !SettingObjectsView._supportedCommands.Contains(commandAndStatus.Name))))
      andVisibleStatus.IsVisible = false;
  }

  public override ContentType ViewContentType => ContentType.NonFolders;

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    this.AutoScaleMode = AutoScaleMode.Font;
  }
}
