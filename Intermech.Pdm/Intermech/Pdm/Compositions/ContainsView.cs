// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.ContainsView
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.Pdm.Compositions;

[ViewDescriptionProvider(typeof (ContainsView.ContainsViewDescriptionProvider))]
internal sealed class ContainsView : ContainsViewBase
{
  private string _caption = LocalizationHolder.rm.GetString("Pdm_47");
  private int _imageIndex = -1;

  public ContainsView()
    : base(ContainsMode.Contains)
  {
    if (!(ServicesManager.GetService(typeof (INamedImageList)) is INamedImageList service))
      return;
    this._imageIndex = service.ImageIndex("imgContains");
  }

  public override int ImageIndex => this._imageIndex;

  public override string Caption => this._caption;

  public override int OrderID => 25;

  protected override List<Guid> GetPossibleRelationTypes(IUserSession session, int objType)
  {
    List<Guid> relationTypes = new List<Guid>();
    DataTable applicabilitiesList = session.GetRelationsApplicabilityCollection().GetApplicabilitiesList(-1, -1, objType);
    this.FillApplicability(session, applicabilitiesList, ref relationTypes);
    return relationTypes;
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ContainsView));
    ((ISupportInitialize) this._grid).BeginInit();
    ((ISupportInitialize) this._pictureBox).BeginInit();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this._toolBar, "tbViewBar");
    this._toolTip.SetToolTip((System.Windows.Forms.Control) this._toolBar, componentResourceManager.GetString("tbViewBar.ToolTip"));
    componentResourceManager.ApplyResources((object) this._embeddedViewsDropDownMenuItem, "btViewNames");
    componentResourceManager.ApplyResources((object) this._toggleManualSortingButtonItem, "btClearSorting");
    componentResourceManager.ApplyResources((object) this._grid, "grid");
    this._grid.DefaultAutoGroupRow.Height = 21;
    this._grid.DefaultRow.Height = (int) componentResourceManager.GetObject("resource.Height");
    this._grid.DefaultRow.Key = componentResourceManager.GetString("resource.Key");
    this._grid.DefaultRow.NormalCellHeight = (int) componentResourceManager.GetObject("resource.NormalCellHeight");
    this._grid.FrozenArea.ColCount = 1;
    this._grid.FrozenArea.SortFrozenRows = true;
    this._grid.GroupBox.BackColor = SystemColors.AppWorkspace;
    this._grid.GroupBox.HintBackColor = SystemColors.AppWorkspace;
    this._grid.GroupBox.HintForeColor = SystemColors.ControlText;
    this._grid.GroupBox.Text = componentResourceManager.GetString("grid.GroupBox.Text");
    this._grid.GroupBox.Visible = true;
    this._grid.Header.AutoHeightFlags = iGHdrAutoHeightFlags.OnAddCol | iGHdrAutoHeightFlags.OnRemoveCol | iGHdrAutoHeightFlags.OnShowCol | iGHdrAutoHeightFlags.OnContentsChange | iGHdrAutoHeightFlags.OnThemeChange | iGHdrAutoHeightFlags.OnResizeCol;
    this._grid.Header.Height = (int) componentResourceManager.GetObject("grid.Header.Height");
    this._grid.LayoutObject.Flags = iGLayoutFlags.Grouping | iGLayoutFlags.Sorting | iGLayoutFlags.ColVisibility | iGLayoutFlags.ColWidth | iGLayoutFlags.ColOrder;
    this._toolTip.SetToolTip((System.Windows.Forms.Control) this._grid, componentResourceManager.GetString("grid.ToolTip"));
    componentResourceManager.ApplyResources((object) this._collapseAllGroupsButtonItem, "btCollapseAll");
    componentResourceManager.ApplyResources((object) this._expandAllGroupsButtonItem, "btExpandAll");
    componentResourceManager.ApplyResources((object) this._pageViewsManager, "ViewsManager");
    this._toolTip.SetToolTip((System.Windows.Forms.Control) this._pageViewsManager, componentResourceManager.GetString("ViewsManager.ToolTip"));
    componentResourceManager.ApplyResources((object) this.buttonHeightSet, "buttonHeightSet");
    this.buttonHeightSet.Padding.Bottom = 0;
    this.buttonHeightSet.Padding.Left = 0;
    this.buttonHeightSet.Padding.Right = 0;
    this.buttonHeightSet.Padding.Top = 0;
    componentResourceManager.ApplyResources((object) this._filtersComboBoxItem, "listObjectsFiltration");
    this._filtersComboBoxItem.Padding.Bottom = 0;
    this._filtersComboBoxItem.Padding.Left = 1;
    this._filtersComboBoxItem.Padding.Right = 1;
    this._filtersComboBoxItem.Padding.Top = 0;
    componentResourceManager.ApplyResources((object) this._manualSortingSetupButtonItem, "btSetupSorting");
    componentResourceManager.ApplyResources((object) this._toggleGroupingButtonItem, "btClearGrouping");
    componentResourceManager.ApplyResources((object) this._refreshButtonItem, "btRefresh");
    componentResourceManager.ApplyResources((object) this._gridHeaderMenuBar, "menuHeader");
    this._toolTip.SetToolTip((System.Windows.Forms.Control) this._gridHeaderMenuBar, componentResourceManager.GetString("menuHeader.ToolTip"));
    componentResourceManager.ApplyResources((object) this._gridHeaderContextMenuBarItem, "contextMenuHeader");
    componentResourceManager.ApplyResources((object) this._changeGridColumnsMenuButtonItem, "mnpSetupColumns");
    componentResourceManager.ApplyResources((object) this._collapseAllGroupsExpectGroupsWithFocusedItemsButtonItem, "btCollapseAndShow");
    componentResourceManager.ApplyResources((object) this._pictureBox, "pictureView");
    this._toolTip.SetToolTip((System.Windows.Forms.Control) this._pictureBox, componentResourceManager.GetString("pictureView.ToolTip"));
    componentResourceManager.ApplyResources((object) this._currentVersionsRuleButtonItem, "buttonVersionsRule");
    componentResourceManager.ApplyResources((object) this, "$this");
    this.Name = nameof (ContainsView);
    this._toolTip.SetToolTip((System.Windows.Forms.Control) this, componentResourceManager.GetString("$this.ToolTip"));
    ((ISupportInitialize) this._grid).EndInit();
    ((ISupportInitialize) this._pictureBox).EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  private sealed class ContainsViewDescriptionProvider : BaseViewDescriptionProvider
  {
    public override ViewDescription DoGetViewDescription(
      ISelectedItems selectedItems,
      IServiceProvider serviceProvider)
    {
      if (!(serviceProvider.GetService(typeof (INamedImageList)) is INamedImageList service))
        service = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
      INamedImageList namedImageList = service;
      return new ViewDescription()
      {
        Caption = LocalizationHolder.rm.GetString("Pdm_47"),
        ImageIndex = namedImageList != null ? namedImageList.ImageIndex("imgContains") : -1,
        OrderID = 25
      };
    }
  }
}
