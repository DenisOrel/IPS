// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareCompositionView
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal class CompareCompositionView : ChildrenView
{
  private int _imageIndex = -1;
  private bool _queryBegining;
  private bool _reloadFromSelf;
  private int _objectType = -1;
  private CompareSettingsControl _settings;
  private string _stateStreamPrefix = nameof (CompareCompositionView);
  private bool _initializedSettings;
  private IContainer components;
  private ToolStripStatusLabel toolStripStatusLabel1;
  private ToolStripStatusLabel toolStripStatusLabel2;
  private ToolStripStatusLabel toolStripStatusLabel3;

  public CompareCompositionView()
  {
    this.InitializeComponent();
    INamedImageList service = (INamedImageList) ServicesManager.GetService(typeof (INamedImageList));
    if (service != null)
      this._imageIndex = service.ImageIndex("imgCompCompare");
    this.DisableColumnsGrouping = true;
    this._readNextToolStripDropDownButton.Enabled = false;
    this._readNextToolStripDropDownButton.Visible = false;
    this._readAllToolStripDropDownButton.Enabled = false;
    this._readAllToolStripDropDownButton.Visible = false;
    this._refreshButtonItem.Visible = false;
    this._manualSortingSetupButtonItem.Visible = false;
    this._toggleManualSortingButtonItem.Visible = false;
    this._editingModeButtonItem.Visible = false;
  }

  public override string StateStreamPrefix
  {
    [DebuggerStepThrough] get => this._stateStreamPrefix;
    set
    {
    }
  }

  public override void Initialize(ISelectedItems items, IServiceProvider provider)
  {
    if (this._reloadFromSelf)
    {
      this._reloadFromSelf = false;
    }
    else
    {
      this._objectType = ((IDBTypedObjectID) items.GetItemData(0, typeof (IDBTypedObjectID))).ObjectType;
      base.Initialize(items, provider);
    }
  }

  private void InitSettings()
  {
    if (this._initializedSettings)
      return;
    ICompareObjectNode node = (ICompareObjectNode) this.Node;
    ((BaseBackgroundReader) node.Reader).StateChangedEvent += new StateChanged(this.ReaderStateChanged);
    this.SetDefaultCompareRelTypes(node);
    this._settings = new CompareSettingsControl(this._toolBar, this._statusStrip, node.Info, true);
    this._settings.SettingsButtonClickEvent += new SettingsButtonClick(this.OnSettingsButtonClick);
    this._settings.RelTypesChangedEvent += new RelTypesChanged(this.OnRelTypesChanged);
    this._initializedSettings = true;
  }

  public override void Activate(IView previousView)
  {
    ICompareObjectNode node = (ICompareObjectNode) this.Node;
    node.FromCompareView = true;
    base.Activate(previousView);
    this.InitSettings();
    if (((ICompareObjectNode) this.Node).Info.Result != null && ((ICompareObjectNode) this.Node).Info.Result.Count != 0 || !node.RefreshColumns[node.ObjectID])
      return;
    this.SetColumns(false);
    node.RefreshColumns[node.ObjectID] = false;
  }

  public override void Deactivate(IView nextView)
  {
    ((ICompareObjectNode) this.Node).FromCompareView = false;
    this.SaveConfig();
    base.Deactivate(nextView);
  }

  public override int ImageIndex => this._imageIndex;

  public override string Caption => PDMPluginConsts.CompareObjectComposition;

  private void SetNeedRefreshColumns()
  {
    ICompareObjectNode node = (ICompareObjectNode) this.Node;
    for (int index = 0; index < node.CompareObjects.Count; ++index)
      node.RefreshColumns[node.CompareObjects[index].Item1] = node.CompareObjects[index].Item1 != node.ObjectID;
  }

  private void ResetColumns()
  {
    ICompareObjectNode node = (ICompareObjectNode) this.Node;
    this.SetColumns(false);
    this.SetNeedRefreshColumns();
    node.ClearResult();
  }

  public override void ResetColumnsCommand(
    ISelectedItems selectedItems,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    this.ResetColumns();
  }

  protected override INode GetNode() => base.GetNode();

  private void ClearTableData()
  {
    ICompareObjectNode node = (ICompareObjectNode) this.Node;
    if (node.Reader != null && node.Reader.QueryResult != null)
      node.Reader.QueryResult.Rows.Clear();
    if (node.Info.Result != null)
      node.Info.Result.Clear();
    this.ClearData();
  }

  private void SetColumns(bool fromConfig, bool onActivate = false)
  {
    this.ClearTableData();
    ICompareObjectNode node = (ICompareObjectNode) this.Node;
    this._reloadFromSelf = true;
    try
    {
      if (fromConfig)
        this.GridLoadState((Stream) null);
      else
        this.SetColumns(node.GetDefaultColumns(ContentType.NonFolders), false);
    }
    finally
    {
      this._reloadFromSelf = false;
    }
  }

  private void ReaderStateChanged(object sender, StateChangedEventArgs arg)
  {
    try
    {
      ICompareObjectNode node = (ICompareObjectNode) this.Node;
      BackgroundReaderComparer reader = (BackgroundReaderComparer) node.Reader;
      switch (arg.State)
      {
        case BackgroundState.Error:
          this.SetEnabledControls(true);
          this._toolBar.Invoke((Delegate) new CompareCompositionView.SetButtonItemHandler(this.SetButtonItem), (object) true);
          reader.State = BackgroundState.Empty;
          break;
        case BackgroundState.Reading:
          this.SetEnabledControls(false);
          this._toolBar.Invoke((Delegate) new CompareCompositionView.SetButtonItemHandler(this.SetButtonItem), (object) false);
          this._queryBegining = true;
          break;
        case BackgroundState.Fill:
          this.SetEnabledControls(true);
          this._toolBar.Invoke((Delegate) new CompareCompositionView.SetButtonItemHandler(this.SetButtonItem), (object) true);
          break;
        case BackgroundState.PartComplete:
          if (node.Info.Result == null)
            node.ClearResult();
          if (node.Info.Result.ContainsKey(arg.PartIdCompleted))
          {
            node.Info.Result[arg.PartIdCompleted] = node.Reader.QueryResult;
            break;
          }
          node.Info.Result.Add(arg.PartIdCompleted, node.Reader.QueryResult);
          break;
      }
    }
    catch (Exception ex)
    {
      ExceptionHelper.ExceptionService.ShowException(ex);
    }
  }

  private void SetEnabled(System.Windows.Forms.Control sender, bool enable)
  {
    if (sender == this._toolBar)
      this._embeddedViewsDropDownMenuItem.Enabled = enable;
    else
      sender.Enabled = enable;
  }

  private void SetEnabledControls(bool enable)
  {
    this._grid.Invoke((Delegate) new CompareCompositionView.SetEnabledHandler(this.SetEnabled), (object) this._grid, (object) enable);
    this._toolBar.Invoke((Delegate) new CompareCompositionView.SetEnabledHandler(this.SetEnabled), (object) this._toolBar, (object) enable);
  }

  private void SetButtonItem(bool start)
  {
    this._settings.SetStartButton(start);
    this._settings.SetCircleThread(!start);
    if (!start)
      return;
    this._queryBegining = false;
    this._dataAdapter.ClearRows();
    this.ReloadItems();
  }

  public override void SetColumnsCommand(
    ISelectedItems selectedItems,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    ((ICompareObjectNode) this.Node).ClearResult();
    base.SetColumnsCommand(selectedItems, viewServices, additionalInfo);
  }

  private void OnRelTypesChanged(object sender, EventArgs e)
  {
    this.SaveConfig();
    this.SetColumns(false);
    this.SetNeedRefreshColumns();
    ((ICompareObjectNode) this.Node).Info.Result = (Dictionary<long, DataTable>) null;
  }

  private void OnSettingsButtonClick(object sender, SettingsButtonClickEventArgs e)
  {
    ICompareObjectNode node = (ICompareObjectNode) this.Node;
    switch (e.ButtonCommand)
    {
      case SettingsButtonCommands.Recursive:
        node.Info.Recursive = !e.ButtonChecked;
        node.Info.Result = (Dictionary<long, DataTable>) null;
        this.ClearTableData();
        break;
      case SettingsButtonCommands.Differences:
        node.Info.CompositionMode = CompositionModes.Differences;
        this.ReloadItems();
        break;
      case SettingsButtonCommands.Compatibility:
        node.Info.CompositionMode = CompositionModes.Compatibility;
        this.ReloadItems();
        break;
      case SettingsButtonCommands.Composition:
        node.Info.CompositionMode = CompositionModes.Composition;
        this.ReloadItems();
        break;
      case SettingsButtonCommands.Start:
        this.OnStartClick(node);
        break;
      case SettingsButtonCommands.ResetColumns:
        this.ResetColumns();
        break;
    }
  }

  private void OnStartClick(ICompareObjectNode node)
  {
    BackgroundReaderComparer reader = (BackgroundReaderComparer) node.Reader;
    if (!this._queryBegining)
    {
      node.ClearResult();
      node.FromCompareView = true;
      using (new FixEditingContext())
        this.ReloadItems();
    }
    else
    {
      reader?.Cancel();
      reader.State = BackgroundState.Empty;
    }
  }

  private void RefreshData() => this.ReloadItems();

  private void SaveConfig()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      ICompareObjectNode node = (ICompareObjectNode) this.Node;
      CompareSettings.Write(sessionKeeper.Session, this._objectType, node.Info.RelationTypes, node.Info.CompareAttributes);
    }
  }

  private void SetDefaultCompareRelTypes(ICompareObjectNode node)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      Dictionary<int, bool> relationTypes;
      List<int> compareAttributes;
      CompareSettings.Read(sessionKeeper.Session, this._objectType, out relationTypes, out compareAttributes);
      foreach (KeyValuePair<int, bool> relationType in node.Info.RelationTypes)
      {
        if (!relationTypes.ContainsKey(relationType.Key))
          relationTypes.Add(relationType.Key, false);
      }
      node.Info.RelationTypes = relationTypes;
      node.Info.CompareAttributes = compareAttributes;
    }
  }

  protected override void CustomDrawCellBackground(object sender, iGCustomDrawCellEventArgs e)
  {
    iGRow row = e.RowIndex >= 0 ? this._grid.Rows[e.RowIndex] : (iGRow) null;
    Intermech.Navigator.DBObjects.NodeID nodeIdForRow = row != null ? this.GetNodeIDForRow(row) as Intermech.Navigator.DBObjects.NodeID : (Intermech.Navigator.DBObjects.NodeID) null;
    ICompareObjectNode node = (ICompareObjectNode) this.Node;
    if (node.CurrentDifferences == null || node.CurrentDifferences.Differences == null)
    {
      base.CustomDrawCellBackground(sender, e);
    }
    else
    {
      NodeColumn tag = (NodeColumn) this._grid.Cols[e.ColIndex].Tag;
      if (tag == null)
      {
        base.CustomDrawCellBackground(sender, e);
      }
      else
      {
        List<int> intList = (List<int>) null;
        if (node.CurrentDifferences.Differences.TryGetValue(nodeIdForRow.ObjectID, out intList) && intList != null && (tag.ID is int || tag.ID is ObligatoryObjectAttributes) && intList.Contains((int) tag.ID))
        {
          Rectangle bounds = e.Bounds;
          Brush brush = (Brush) new SolidBrush(Color.CornflowerBlue);
          try
          {
            e.Graphics.FillRectangle(brush, bounds);
          }
          finally
          {
            brush.Dispose();
          }
        }
        else
          base.CustomDrawCellBackground(sender, e);
      }
    }
  }

  protected override void GridDynamicFont(object sender, iGDynamicFontEventArgs e)
  {
    base.GridDynamicFont(sender, e);
    Intermech.Navigator.DBObjects.NodeID nodeIdForRow = (e.RowIndex >= 0 ? this._grid.Rows[e.RowIndex] : (iGRow) null) != null ? this.GetNodeIDForRow(e.RowIndex) as Intermech.Navigator.DBObjects.NodeID : (Intermech.Navigator.DBObjects.NodeID) null;
    ICompareObjectNode node = (ICompareObjectNode) this.Node;
    if (nodeIdForRow == null || node.CurrentDifferences == null || node.CurrentDifferences == null || node.CurrentDifferences.Differences == null || !node.CurrentDifferences.Differences.ContainsKey(nodeIdForRow.ObjectID))
      return;
    e.Font = new Font(e.Font, FontStyle.Bold);
  }

  protected override void RefreshViewCommand(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    ((ICompareObjectNode) this.Node).ClearResult();
    base.RefreshViewCommand(items, viewServices, additionalInfo);
  }

  protected override bool Eof => true;

  protected override void GridReloadIfNeed()
  {
    base.GridReloadIfNeed();
    this.GridSaveState((Stream) null);
  }

  protected override void Dispose(bool disposing)
  {
    ICompareObjectNode node = (ICompareObjectNode) this.Node;
    BackgroundReaderComparer reader = node != null ? (BackgroundReaderComparer) node.Reader : (BackgroundReaderComparer) null;
    if (this._queryBegining)
    {
      reader?.Cancel();
      this._settings.Dispose();
    }
    if (reader != null)
    {
      BackgroundReaderComparer backgroundReaderComparer = reader;
      backgroundReaderComparer.StateChangedEvent = backgroundReaderComparer.StateChangedEvent - new StateChanged(this.ReaderStateChanged);
    }
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (CompareCompositionView));
    this.toolStripStatusLabel1 = new ToolStripStatusLabel();
    this.toolStripStatusLabel2 = new ToolStripStatusLabel();
    this.toolStripStatusLabel3 = new ToolStripStatusLabel();
    ((ISupportInitialize) this._grid).BeginInit();
    ((ISupportInitialize) this._pictureBox).BeginInit();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this._toolBar, "_toolBar");
    this._grid.DefaultAutoGroupRow.Height = 21;
    this._grid.FrozenArea.ColCount = 1;
    this._grid.FrozenArea.SortFrozenRows = true;
    this._grid.GroupBox.BackColor = SystemColors.AppWorkspace;
    this._grid.GroupBox.HintBackColor = SystemColors.AppWorkspace;
    this._grid.GroupBox.HintForeColor = SystemColors.ControlText;
    this._grid.GroupBox.Text = componentResourceManager.GetString("_grid.GroupBox.Text");
    this._grid.GroupBox.Visible = true;
    this._grid.Header.AutoHeightFlags = iGHdrAutoHeightFlags.OnAddCol | iGHdrAutoHeightFlags.OnRemoveCol | iGHdrAutoHeightFlags.OnShowCol | iGHdrAutoHeightFlags.OnContentsChange | iGHdrAutoHeightFlags.OnThemeChange | iGHdrAutoHeightFlags.OnResizeCol;
    this._grid.Header.Height = (int) componentResourceManager.GetObject("_grid.Header.Height");
    this._grid.LayoutObject.Flags = iGLayoutFlags.Grouping | iGLayoutFlags.Sorting | iGLayoutFlags.ColVisibility | iGLayoutFlags.ColWidth | iGLayoutFlags.ColOrder;
    componentResourceManager.ApplyResources((object) this._grid, "_grid");
    componentResourceManager.ApplyResources((object) this._pageViewsManager, "_pageViewsManager");
    this._filtersComboBoxItem.Locked = false;
    this._filtersComboBoxItem.Padding.Bottom = 0;
    this._filtersComboBoxItem.Padding.Left = 1;
    this._filtersComboBoxItem.Padding.Right = 1;
    this._filtersComboBoxItem.Padding.Top = 0;
    componentResourceManager.ApplyResources((object) this._gridHeaderMenuBar, "_gridHeaderMenuBar");
    componentResourceManager.ApplyResources((object) this._pictureBox, "_pictureBox");
    this._editingModeButtonItem.Visible = false;
    this.buttonHeightSet.Padding.Bottom = 0;
    this.buttonHeightSet.Padding.Left = 0;
    this.buttonHeightSet.Padding.Right = 0;
    this.buttonHeightSet.Padding.Top = 0;
    this.toolStripStatusLabel1.BackColor = Color.SkyBlue;
    this.toolStripStatusLabel1.BorderSides = ToolStripStatusLabelBorderSides.All;
    componentResourceManager.ApplyResources((object) this.toolStripStatusLabel1, "toolStripStatusLabel1");
    this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
    this.toolStripStatusLabel2.BackColor = SystemColors.Window;
    this.toolStripStatusLabel2.BorderSides = ToolStripStatusLabelBorderSides.All;
    componentResourceManager.ApplyResources((object) this.toolStripStatusLabel2, "toolStripStatusLabel2");
    this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
    this.toolStripStatusLabel3.BackColor = SystemColors.Window;
    this.toolStripStatusLabel3.BorderSides = ToolStripStatusLabelBorderSides.All;
    this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
    componentResourceManager.ApplyResources((object) this.toolStripStatusLabel3, "toolStripStatusLabel3");
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Name = nameof (CompareCompositionView);
    ((ISupportInitialize) this._grid).EndInit();
    ((ISupportInitialize) this._pictureBox).EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  private delegate void SetEnabledHandler(System.Windows.Forms.Control sender, bool enable);

  private delegate void SetButtonItemHandler(bool start);
}
