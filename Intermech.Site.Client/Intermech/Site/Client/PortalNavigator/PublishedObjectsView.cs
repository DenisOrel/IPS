// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PublishedObjectsView
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Bars;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.Localization;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class PublishedObjectsView : ChildrenView
{
  private int _objectTypeID = -1;
  private int _imageIndex = -1;
  private string _caption = LocalizationHolder.rm.GetString("Site.Client_36");

  public PublishedObjectsView() => this.DisableFiltration = true;

  public override string Caption => this._caption;

  public override int ImageIndex => this._imageIndex;

  public override ContentType ViewContentType => ContentType.NonFolders;

  public override void Initialize(ISelectedItems items, IServiceProvider provider)
  {
    INodeID itemId = items.GetItemID(0);
    if (itemId != null && itemId is PublishTypeNodeID)
    {
      this._objectTypeID = itemId.TypeID;
      if (this._objectTypeID != -1)
      {
        IPortalMetadata service = (IPortalMetadata) ServicesManager.GetService(typeof (IPortalMetadata));
        if (service != null)
        {
          PortalObjectType publishObjectType = service.GetPublishObjectType(this._objectTypeID);
          if (publishObjectType != null)
            this._caption = publishObjectType.Name;
        }
      }
    }
    base.Initialize(items, provider);
  }

  public override bool QueryStatus(ICommandState commandState)
  {
    int num = base.QueryStatus(commandState) ? 1 : 0;
    if (!(commandState.CommandName == "ParametersCard"))
      return num != 0;
    commandState.Enabled = false;
    return num != 0;
  }

  private void InitializeComponent()
  {
    ((ISupportInitialize) this._grid).BeginInit();
    this.SuspendLayout();
    this._grid.DefaultAutoGroupRow.Height = 21;
    this._grid.FrozenArea.ColCount = 1;
    this._grid.FrozenArea.SortFrozenRows = true;
    this._grid.GroupBox.BackColor = SystemColors.AppWorkspace;
    this._grid.GroupBox.HintBackColor = SystemColors.AppWorkspace;
    this._grid.GroupBox.HintForeColor = SystemColors.ControlText;
    this._grid.GroupBox.Text = LocalizationHolder.rm.GetString("Site.Client_38");
    this._grid.GroupBox.Visible = true;
    this._grid.Header.AutoHeightFlags = iGHdrAutoHeightFlags.OnAddCol | iGHdrAutoHeightFlags.OnRemoveCol | iGHdrAutoHeightFlags.OnShowCol | iGHdrAutoHeightFlags.OnContentsChange | iGHdrAutoHeightFlags.OnThemeChange | iGHdrAutoHeightFlags.OnResizeCol;
    this._grid.Header.Height = 19;
    this._grid.LayoutObject.Flags = iGLayoutFlags.Grouping | iGLayoutFlags.Sorting | iGLayoutFlags.ColVisibility | iGLayoutFlags.ColWidth | iGLayoutFlags.ColOrder;
    this._grid.Size = new Size(602, 130);
    this.buttonHeightSet.Padding.Bottom = 0;
    this.buttonHeightSet.Padding.Left = 0;
    this.buttonHeightSet.Padding.Right = 0;
    this.buttonHeightSet.Padding.Top = 0;
    this._filtersComboBoxItem.Enabled = false;
    this._filtersComboBoxItem.Padding.Bottom = 0;
    this._filtersComboBoxItem.Padding.Left = 1;
    this._filtersComboBoxItem.Padding.Right = 1;
    this._filtersComboBoxItem.Padding.Top = 0;
    this._currentVersionsRuleButtonItem.Enabled = false;
    this.DisableFiltration = true;
    this.Name = "PublishObjectsView";
    ((ISupportInitialize) this._grid).EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
