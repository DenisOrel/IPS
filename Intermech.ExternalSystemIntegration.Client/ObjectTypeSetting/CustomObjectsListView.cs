// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.ObjectTypeSetting.CustomObjectsListView
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using Intermech.Bars;
using Intermech.Navigator.ContextMenu.Extensions;
using Intermech.Navigator.Controls;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client.ObjectTypeSetting;

public class CustomObjectsListView : ObjectsViewBase, ICommandsFilter
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
  private ButtonItem buttonItemAdd;

  public event EventHandler AddButtonClick
  {
    add => this.buttonItemAdd.Click += value;
    remove => this.buttonItemAdd.Click -= value;
  }

  public CustomObjectsListView()
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

  public override void Deactivate(IView nextView)
  {
    this._dataLoaded = false;
    base.Deactivate(nextView);
  }

  public void FilterCommands(
    ISelectedItems items,
    IEnumerable<CommandAndVisibleStatus> commandWithVisibleStatuses)
  {
    foreach (CommandAndVisibleStatus andVisibleStatus in commandWithVisibleStatuses.Where<CommandAndVisibleStatus>((Func<CommandAndVisibleStatus, bool>) (commandAndStatus => !CustomObjectsListView._supportedCommands.Contains(commandAndStatus.Name))))
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (CustomObjectsListView));
    this.buttonItemAdd = new ButtonItem();
    ((ISupportInitialize) this._grid).BeginInit();
    ((ISupportInitialize) this._pictureBox).BeginInit();
    this.SuspendLayout();
    this._toolBar.Items.AddRange(new ToolbarItemBase[1]
    {
      (ToolbarItemBase) this.buttonItemAdd
    });
    this._grid.DefaultAutoGroupRow.Height = 21;
    this._grid.FrozenArea.ColCount = 1;
    this._grid.FrozenArea.SortFrozenRows = true;
    this._grid.GroupBox.BackColor = SystemColors.AppWorkspace;
    this._grid.GroupBox.HintBackColor = SystemColors.AppWorkspace;
    this._grid.GroupBox.HintForeColor = SystemColors.ControlText;
    this._grid.GroupBox.Text = "Перетащите заголовок колонки в эту область для группировки по значениям этой колонки";
    this._grid.GroupBox.Visible = true;
    this._grid.Header.AutoHeightFlags = iGHdrAutoHeightFlags.OnAddCol | iGHdrAutoHeightFlags.OnRemoveCol | iGHdrAutoHeightFlags.OnShowCol | iGHdrAutoHeightFlags.OnContentsChange | iGHdrAutoHeightFlags.OnThemeChange | iGHdrAutoHeightFlags.OnResizeCol;
    this._grid.Header.Height = 20;
    this._grid.LayoutObject.Flags = iGLayoutFlags.Grouping | iGLayoutFlags.Sorting | iGLayoutFlags.ColVisibility | iGLayoutFlags.ColWidth | iGLayoutFlags.ColOrder;
    this._grid.Size = new Size(1151, 160 /*0xA0*/);
    this.buttonHeightSet.Padding.Bottom = 0;
    this.buttonHeightSet.Padding.Left = 0;
    this.buttonHeightSet.Padding.Right = 0;
    this.buttonHeightSet.Padding.Top = 0;
    this._filtersComboBoxItem.Padding.Bottom = 0;
    this._filtersComboBoxItem.Padding.Left = 1;
    this._filtersComboBoxItem.Padding.Right = 1;
    this._filtersComboBoxItem.Padding.Top = 0;
    this.buttonItemAdd.BeginGroup = true;
    this.buttonItemAdd.CommandName = "buttonItemAdd";
    this.buttonItemAdd.Image = (Image) componentResourceManager.GetObject("buttonItemAdd.Image");
    this.buttonItemAdd.ToolTipText = "Добавить";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Name = nameof (CustomObjectsListView);
    ((ISupportInitialize) this._grid).EndInit();
    ((ISupportInitialize) this._pictureBox).EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
