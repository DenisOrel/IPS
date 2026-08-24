// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.CompositionTracing.CompositionTracingView
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.Bars;
using Intermech.Controls;
using Intermech.DataFormats;
using Intermech.Docking;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.PdmConfigurator;
using Intermech.Localization;
using Intermech.Navigator.Controls;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.PdmConfigurator.CompositionTracing;

public sealed class CompositionTracingView : DockControl
{
  public static readonly Guid ViewGuid = new Guid("c34700ef-1d61-414b-b827-20144469e9a9");
  private object _syncObject = new object();
  private PdmCompositionBrowserJobStatus _pdmCompositionBrowserJobStatus;
  private Thread _thread;
  private Guid _browserID;
  private DockManager _dockManager;
  private ICategoryTypeIconService _categoryTypeIconService;
  private Dictionary<string, int> _colWidths = new Dictionary<string, int>();
  private NavigatorTreeView _navigatorTreeView;
  private NodeIDPath _selectedNodeIDPath;
  private RelationPath _rootRelationPath;
  private IFiltrationService _filtrationService;
  private ICommandManager _commandManager;
  private IContainer components;
  private Intermech.Bars.ToolBar tbTracing;
  private iGrid _grid;
  private Panel panel1;
  private CheckBox _tracingCheckBox;
  private CheckBox _errorsCheckBox;
  private Label lbFilter;
  private ImageList ilTracing;
  private ButtonItem _processButtonItem;
  private ImageList ilState;
  private ButtonItem _clearButtonItem;
  private Label _processedObjectsCountLabel;

  public CompositionTracingView()
  {
    this.InitializeComponent();
    if (!(ServicesManager.GetService(typeof (IGuidMapper)) is IGuidMapper))
      return;
    this._dockManager = ServicesManager.GetService(typeof (DockManager)) as DockManager;
    this._categoryTypeIconService = ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
    this._filtrationService = ServicesManager.GetService(typeof (IFiltrationService)) as IFiltrationService;
    this.PrepareGridsColumns();
    this.UpdateControls();
  }

  private void ProcessButtonItem_Click(object sender, EventArgs e)
  {
    if (this._pdmCompositionBrowserJobStatus == null || this._pdmCompositionBrowserJobStatus.Progress != PdmCompositionBrowserJobProgress.Working)
    {
      this._processButtonItem.ImageIndex = 0;
      this._processButtonItem.ToolTipText = LocalizationHolder.rm.GetString("PdmConfigurator_9");
      this.StartThread();
    }
    else
    {
      this._processButtonItem.ImageIndex = 1;
      this._processButtonItem.ToolTipText = LocalizationHolder.rm.GetString("PdmConfigurator_6");
      this.Cancel();
    }
  }

  private void ClearButtonItem_Click(object sender, EventArgs e)
  {
    this._grid.Rows.Clear();
    this.UpdateControls();
  }

  private void Grid_CellClick(object sender, iGCellClickEventArgs e)
  {
    RelationPath relationPath = this._grid.Cells[e.RowIndex, "RELATION_PATH"].Value as RelationPath;
    NodeIDPath nodeIDPath = this._selectedNodeIDPath.Clone() as NodeIDPath;
    List<SimpleRelationPair> simpleRelationPairList = new List<SimpleRelationPair>();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      for (int index = 0; index < relationPath.Items.Count; ++index)
      {
        SimpleRelationPair simpleRelationPair = relationPath.Items[index];
        if (simpleRelationPair.F_PRJLINK_ID != 0L && !this._rootRelationPath.Items.Contains(simpleRelationPair) && MetaDataHelper.IsPdmPartiallyConfigurableRelationType(simpleRelationPair.F_RELATION_TYPE))
        {
          IDBObject dbObject = sessionKeeper.Session.GetObject(simpleRelationPair.F_PART_ID);
          NodeID NodeID = new NodeID(dbObject.ObjectType, dbObject.ObjectID, dbObject.ID, dbObject.CheckoutBy, simpleRelationPair.F_PRJLINK_ID, dbObject.LCStep, dbObject.Caption, simpleRelationPair.F_RELATION_TYPE, dbObject.OwnerID, 0L, ObjectFiltrationState.fsNotRequired, (long) dbObject.VersionID, 0L, string.Empty, 0L, Guid.Empty, dbObject.ModificationID);
          nodeIDPath.Add((INodeID) NodeID);
        }
      }
    }
    this._navigatorTreeView.TryBrowse(nodeIDPath);
  }

  private void Grid_ColWidthChanging(object sender, iGColWidthEventArgs e)
  {
    this._colWidths[this._grid.Cols[e.ColIndex].Key] = e.Width;
    this.CorrectColsWidth();
  }

  private void Grid_ColWidthEndChange(object sender, iGColWidthEventArgs e)
  {
    this._colWidths[this._grid.Cols[e.ColIndex].Key] = e.Width;
    this.CorrectColsWidth();
  }

  private void Grid_Resize(object sender, EventArgs e) => this.CorrectColsWidth();

  private void ErrorsCheckBox_CheckedChanged(object sender, EventArgs e)
  {
    this.ChangeFilter(this._errorsCheckBox.Checked, CompositionTracingView.MessageType.Error);
  }

  private void TracingCheckBox_CheckedChanged(object sender, EventArgs e)
  {
    this.ChangeFilter(this._tracingCheckBox.Checked, CompositionTracingView.MessageType.Trace);
  }

  private void PrepareGridsColumns()
  {
    iGCellStyle iGcellStyle1 = new iGCellStyle(true);
    iGcellStyle1.ImageAlign = iGContentAlignment.TopLeft;
    iGcellStyle1.TextAlign = iGContentAlignment.TopLeft;
    iGcellStyle1.ReadOnly = iGBool.True;
    iGcellStyle1.ImageList = this.ilState;
    iGCellStyle iGcellStyle2 = new iGCellStyle(true);
    iGcellStyle2.TextAlign = iGContentAlignment.TopLeft;
    iGcellStyle2.ReadOnly = iGBool.True;
    iGCellStyle iGcellStyle3 = new iGCellStyle(true);
    iGcellStyle3.ImageAlign = iGContentAlignment.MiddleLeft;
    iGcellStyle3.ReadOnly = iGBool.True;
    iGcellStyle3.ImageList = this._categoryTypeIconService.ImageList;
    if (this._colWidths.Count == 0)
      this._colWidths = new Dictionary<string, int>()
      {
        {
          "OBJECT_TYPE_ICON",
          32 /*0x20*/
        },
        {
          "OBJECT_ID",
          50
        },
        {
          "RELATION_ID",
          50
        },
        {
          "OBJECT_CAPTION",
          200
        },
        {
          "MESSAGE",
          200
        },
        {
          "RELATION_PATH",
          0
        },
        {
          "TRACE",
          0
        }
      };
    iGCol col1 = this._grid.Cols["OBJECT_TYPE_ICON"];
    iGCol iGcol1 = this._grid.Cols["OBJECT_TYPE_ICON"] ?? this._grid.Cols.Add(new iGColPattern(this._colWidths["OBJECT_TYPE_ICON"], true, true, 32 /*0x20*/, 32 /*0x20*/, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) string.Empty, "OBJECT_TYPE_ICON", -1, (object) null, (object) null, -1));
    iGcol1.Width = this._colWidths["OBJECT_TYPE_ICON"];
    iGcol1.CellStyle = iGcellStyle3;
    iGCol col2 = this._grid.Cols["OBJECT_ID"];
    iGCol iGcol2 = this._grid.Cols["OBJECT_ID"] ?? this._grid.Cols.Add(new iGColPattern(this._colWidths["OBJECT_ID"], true, true, 50, -1, true, false, false, iGSortType.ByValue, iGSortOrder.Ascending, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_1"), "OBJECT_ID", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol2.CellStyle = iGcellStyle2;
    iGcol2.Width = this._colWidths["OBJECT_ID"];
    iGCol col3 = this._grid.Cols["RELATION_ID"];
    iGCol iGcol3 = this._grid.Cols["RELATION_ID"] ?? this._grid.Cols.Add(new iGColPattern(this._colWidths["RELATION_ID"], true, true, 50, -1, true, false, false, iGSortType.ByValue, iGSortOrder.Ascending, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_2"), "RELATION_ID", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol3.CellStyle = iGcellStyle2;
    iGcol3.Width = this._colWidths["RELATION_ID"];
    iGCol col4 = this._grid.Cols["OBJECT_CAPTION"];
    iGCol iGcol4 = this._grid.Cols["OBJECT_CAPTION"] ?? this._grid.Cols.Add(new iGColPattern(this._colWidths["OBJECT_CAPTION"], true, true, 200, -1, true, false, false, iGSortType.ByValue, iGSortOrder.Ascending, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_3"), "OBJECT_CAPTION", -1, (object) null, (object) null, -1));
    iGcol4.Width = this._colWidths["OBJECT_CAPTION"];
    iGcol4.CellStyle = iGcellStyle2;
    iGCol col5 = this._grid.Cols["MESSAGE"];
    iGCol iGcol5 = this._grid.Cols["MESSAGE"] ?? this._grid.Cols.Add(new iGColPattern(this._colWidths["MESSAGE"], true, true, 200, -1, true, false, false, iGSortType.ByValue, iGSortOrder.Ascending, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_4"), "MESSAGE", -1, (object) null, (object) null, -1));
    iGcol5.Width = this._colWidths["MESSAGE"];
    iGcol5.CellStyle = iGcellStyle1;
    iGCol col6 = this._grid.Cols["RELATION_PATH"];
    (this._grid.Cols["RELATION_PATH"] ?? this._grid.Cols.Add(new iGColPattern(this._colWidths["RELATION_PATH"], false, false, 0, 0, true, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) string.Empty, "RELATION_PATH", -1, (object) null, (object) null, -1))).Width = this._colWidths["RELATION_PATH"];
    iGCol col7 = this._grid.Cols["TRACE"];
    (this._grid.Cols["TRACE"] ?? this._grid.Cols.Add(new iGColPattern(this._colWidths["TRACE"], false, false, 0, 0, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) string.Empty, "TRACE", -1, (object) null, (object) null, -1))).Width = this._colWidths["TRACE"];
    this.CorrectColsWidth();
  }

  private void CorrectColsWidth()
  {
    if (this._grid.AutoResizeCols || this._colWidths.Count == 0)
      return;
    int num = this._grid.ClientRectangle.Width - 30 - this._colWidths["OBJECT_TYPE_ICON"] - this._colWidths["OBJECT_ID"] - this._colWidths["RELATION_ID"] - this._colWidths["OBJECT_CAPTION"];
    if (this._grid.Cols.Count == 0)
      return;
    this._grid.Cols["OBJECT_CAPTION"].Width = this._colWidths["OBJECT_CAPTION"];
    this._grid.Cols["OBJECT_ID"].Width = this._colWidths["OBJECT_ID"];
    this._grid.Cols["RELATION_ID"].Width = this._colWidths["RELATION_ID"];
    if (num > 200)
      this._grid.Cols["MESSAGE"].Width = this._colWidths["MESSAGE"] = num;
    else
      this._grid.Cols["MESSAGE"].Width = this._colWidths["MESSAGE"];
  }

  private void LoadCompositionInfo()
  {
    if (this._pdmCompositionBrowserJobStatus == null)
      return;
    SortedDictionary<RelationPath, TraceEntry> items = this._pdmCompositionBrowserJobStatus.Trace.Items;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      foreach (RelationPath key in items.Keys)
      {
        TraceEntry traceEntry = items[key];
        if (traceEntry.Flags != PdmConfiguratorResult.Unknown)
        {
          iGRow iGrow = this._grid.Rows.Add();
          iGrow.Visible = traceEntry.Flags == PdmConfiguratorResult.True || traceEntry.Flags == PdmConfiguratorResult.False ? this._tracingCheckBox.Checked : this._errorsCheckBox.Checked;
          iGrow.Cells["MESSAGE"].Value = traceEntry.Message != string.Empty ? (object) traceEntry.Message : (object) LocalizationHolder.rm.GetString("PdmConfigurator_5");
          iGrow.Cells["MESSAGE"].ImageIndex = traceEntry.Flags != PdmConfiguratorResult.Incompatibles ? (traceEntry.Flags != PdmConfiguratorResult.ContextNotFound ? (traceEntry.Flags != PdmConfiguratorResult.Exception ? (traceEntry.Flags == PdmConfiguratorResult.OptionNotFound || traceEntry.Flags == PdmConfiguratorResult.ConflictOptionNotFound || traceEntry.Flags == PdmConfiguratorResult.ApplOptionNotFound ? 3 : (traceEntry.Flags == PdmConfiguratorResult.OptionValueNotFound || traceEntry.Flags == PdmConfiguratorResult.ConflictOptionValueNotFound || traceEntry.Flags == PdmConfiguratorResult.ApplOptionValueNotFound ? 4 : (traceEntry.Flags != PdmConfiguratorResult.True ? 6 : 5))) : 2) : 1) : 0;
          iGrow.Cells["RELATION_PATH"].Value = (object) key;
          iGrow.Cells["TRACE"].Value = (object) traceEntry;
          long fPartId = key.Items[key.Items.Count - 1].F_PART_ID;
          int fObjectType = key.Items[key.Items.Count - 1].F_OBJECT_TYPE;
          iGrow.Cells["RELATION_ID"].Value = (object) key.Items[key.Items.Count - 1].F_PRJLINK_ID;
          iGrow.Cells["OBJECT_TYPE_ICON"].ImageIndex = this._categoryTypeIconService.IndexOf(4, fObjectType);
          IDBObject dbObject = sessionKeeper.Session.GetObject(fPartId);
          iGrow.Cells["OBJECT_CAPTION"].Value = (object) dbObject.Caption;
          iGrow.Cells["OBJECT_ID"].Value = (object) dbObject.ObjectID;
        }
      }
    }
  }

  private void StartThread()
  {
    this.StopThread();
    this._grid.Rows.Clear();
    using (FixEditingContext fixEditingContext = new FixEditingContext())
    {
      this._thread = new Thread(fixEditingContext.SendEditingContextToThread(new ThreadStart(this.ThreadMethod)));
      this._thread.IsBackground = true;
      this._thread.Name = "PdmConfigurator.CompositionTracing";
      this._processedObjectsCountLabel.Text = "Обработано объетков: 0";
      this.UpdateControls();
      this._thread.Start();
    }
  }

  private void StopThread()
  {
    if (this._thread != null)
      this._thread.Abort();
    this._thread = (Thread) null;
  }

  private void ThreadMethod()
  {
    if (!(this._dockManager.ActiveDocument is NavWindowBase navWindowBase1))
      navWindowBase1 = ((IEnumerable<DockControl>) this._dockManager.GetDockControls()).Where<DockControl>((Func<DockControl, bool>) (dockControl => dockControl is NavWindowBase)).Cast<NavWindowBase>().FirstOrDefault<NavWindowBase>((Func<NavWindowBase, bool>) (navWindowBase => navWindowBase.IsOpen));
    if (navWindowBase1 != null && navWindowBase1.TreeView != null)
    {
      this._navigatorTreeView = navWindowBase1.TreeView;
      this._selectedNodeIDPath = this._navigatorTreeView.GetNodeIDPath(this._navigatorTreeView.FocusedNode);
      ISelectedItems selectedItems = this._navigatorTreeView.SelectedItems;
      this._rootRelationPath = NavigatorTreeViewHelper.GetCompositionNodePath(this._navigatorTreeView.FocusedNode);
      if (selectedItems != null && selectedItems.Count == 1)
        this.Browse(selectedItems);
    }
    this._thread = (Thread) null;
    this.LoadCompositionInfo();
    this._processButtonItem.ImageIndex = 1;
    this._processButtonItem.ToolTipText = LocalizationHolder.rm.GetString("PdmConfigurator_6");
    this.UpdateControls();
  }

  private void Browse(ISelectedItems items)
  {
    lock (this._syncObject)
      this._pdmCompositionBrowserJobStatus = (PdmCompositionBrowserJobStatus) null;
    IDBTypedObjectID itemData1 = items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
    IDBRelationID itemData2 = items.GetItemData(0, typeof (IDBRelationID)) as IDBRelationID;
    if (itemData1 == null || itemData2 == null || !MetaDataHelper.IsPdmRootObjectType(itemData1.ObjectType) && !MetaDataHelper.IsPdmConfigurableObjectType(itemData1.ObjectType))
    {
      int num = (int) IMMessageBox.Show(LocalizationHolder.rm.GetString("PdmConfigurator_7"), LocalizationHolder.rm.GetString("PdmConfigurator_8"), MessageBoxButtons.OK, IMMessageBoxImage.Information);
    }
    else
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        if (!(sessionKeeper.Session.GetCustomService(typeof (IPdmConfiguratorService)) is IPdmConfiguratorService customService))
        {
          lock (this._syncObject)
            this._pdmCompositionBrowserJobStatus = (PdmCompositionBrowserJobStatus) null;
        }
        else
        {
          CompositionObjects objs = new CompositionObjects();
          IDBObject dbObject1 = sessionKeeper.Session.GetObject(itemData1.ObjectID);
          IDBObject dbObject2;
          if (this._rootRelationPath.Items.Count == 0)
          {
            dbObject2 = dbObject1;
          }
          else
          {
            IDBRelation relation = sessionKeeper.Session.GetRelation(this._rootRelationPath.Items[0].F_PRJLINK_ID);
            dbObject2 = sessionKeeper.Session.GetObject(relation.ProjID);
          }
          RelationPair rootObject = new RelationPair(sessionKeeper.Session.ClientConnectionID, dbObject2.ObjectID, dbObject2.ObjectType, 0L, sessionKeeper.Session.UserID, dbObject2.ObjectID, -1, dbObject2.ObjectType);
          CompositionObject compositionObject = new CompositionObject(dbObject1.ID, dbObject1.ObjectID, dbObject1.ObjectType, dbObject1.LCStep, dbObject1.OwnerID, dbObject1.CheckoutBy, dbObject1.Caption, (long) dbObject1.VersionID, dbObject1.ModificationID, Convert.ToInt64(dbObject1.IsBaseVersion), ObjectVersionDescriptionOptions.None, (CompositionObjects) null, itemData2.Value, itemData2.PartID, itemData2.RelationType, string.Empty);
          objs.Add(compositionObject);
          this._browserID = customService.Browse(sessionKeeper.Session.SessionGUID, rootObject, this._rootRelationPath, objs, new PdmCompositionBrowserEventArgs(-1, this._filtrationService.FiltrationServiceOwnerID, (VersionsRule) null, (HybridDictionary) null, true));
          PdmCompositionBrowserJobStatus tempStatus = (PdmCompositionBrowserJobStatus) null;
          while (!(this._browserID == Guid.Empty))
          {
            tempStatus = customService.QueryBrowserStatus(this._browserID);
            lock (this._syncObject)
              this._pdmCompositionBrowserJobStatus = tempStatus;
            if (tempStatus == null)
              break;
            this._processedObjectsCountLabel.Invoke((Delegate) (() =>
            {
              if (tempStatus.Trace == null)
                return;
              this._processedObjectsCountLabel.Text = $"Обработано объетков: {tempStatus.Trace.Items.Count}";
            }));
            if (tempStatus.Progress != PdmCompositionBrowserJobProgress.NotStarted && tempStatus.Progress != PdmCompositionBrowserJobProgress.Working)
              break;
            Thread.Sleep(1000);
          }
        }
      }
    }
  }

  private void Cancel()
  {
    if (this._browserID == Guid.Empty)
      return;
    lock (this._syncObject)
    {
      this.StopThread();
      if ((ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService).GetCustomService(typeof (IPdmConfiguratorService)) is IPdmConfiguratorService customService)
        customService.CancelBrowse(this._browserID);
      this._pdmCompositionBrowserJobStatus = (PdmCompositionBrowserJobStatus) null;
      this._browserID = Guid.Empty;
      this.UpdateControls();
    }
  }

  private void ChangeFilter(bool visible, CompositionTracingView.MessageType type)
  {
    foreach (iGRow row in (IEnumerable) this._grid.Rows)
    {
      TraceEntry traceEntry = row.Cells["TRACE"].Value as TraceEntry;
      if (type == CompositionTracingView.MessageType.Trace && (traceEntry.Flags == PdmConfiguratorResult.False || traceEntry.Flags == PdmConfiguratorResult.True))
        row.Visible = visible;
      else if (type == CompositionTracingView.MessageType.Error && traceEntry.Flags != PdmConfiguratorResult.False && traceEntry.Flags != PdmConfiguratorResult.True)
        row.Visible = visible;
    }
  }

  private void UpdateControls()
  {
    this._clearButtonItem.Enabled = this._grid.Rows.Count > 0;
    this._processedObjectsCountLabel.Visible = this._thread != null;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (CompositionTracingView));
    this.tbTracing = new Intermech.Bars.ToolBar();
    this.ilTracing = new ImageList(this.components);
    this._processButtonItem = new ButtonItem();
    this._clearButtonItem = new ButtonItem();
    this._grid = new iGrid();
    this.panel1 = new Panel();
    this._processedObjectsCountLabel = new Label();
    this._tracingCheckBox = new CheckBox();
    this._errorsCheckBox = new CheckBox();
    this.lbFilter = new Label();
    this.ilState = new ImageList(this.components);
    ((ISupportInitialize) this._grid).BeginInit();
    this.panel1.SuspendLayout();
    this.SuspendLayout();
    this.tbTracing.FullMenus = true;
    this.tbTracing.Guid = new Guid("37056402-c6d1-47d4-be0f-e941c1a06e55");
    this.tbTracing.Hidden = false;
    this.tbTracing.ImageList = this.ilTracing;
    this.tbTracing.Items.AddRange(new ToolbarItemBase[2]
    {
      (ToolbarItemBase) this._processButtonItem,
      (ToolbarItemBase) this._clearButtonItem
    });
    componentResourceManager.ApplyResources((object) this.tbTracing, "tbTracing");
    this.tbTracing.Name = "tbTracing";
    this.tbTracing.Tag = (object) "";
    this.ilTracing.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("ilTracing.ImageStream");
    this.ilTracing.TransparentColor = Color.Transparent;
    this.ilTracing.Images.SetKeyName(0, "gear_stop.png");
    this.ilTracing.Images.SetKeyName(1, "gear_run.png");
    this.ilTracing.Images.SetKeyName(2, "document.png");
    componentResourceManager.ApplyResources((object) this._processButtonItem, "_processButtonItem");
    this._processButtonItem.ImageIndex = 1;
    this._processButtonItem.Click += new EventHandler(this.ProcessButtonItem_Click);
    this._clearButtonItem.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this._clearButtonItem, "_clearButtonItem");
    this._clearButtonItem.ImageIndex = 2;
    this._clearButtonItem.Click += new EventHandler(this.ClearButtonItem_Click);
    this._grid.BackColorEvenRows = Color.WhiteSmoke;
    this._grid.DefaultRow.Height = (int) componentResourceManager.GetObject("resource.Height");
    this._grid.DefaultRow.NormalCellHeight = (int) componentResourceManager.GetObject("resource.NormalCellHeight");
    componentResourceManager.ApplyResources((object) this._grid, "_grid");
    this._grid.Header.Height = (int) componentResourceManager.GetObject("_grid.Header.Height");
    this._grid.HighlightBackColorNoFocus = SystemColors.ControlLight;
    this._grid.HotTracking = false;
    this._grid.LayoutObject.Flags = iGLayoutFlags.Sorting | iGLayoutFlags.ColVisibility | iGLayoutFlags.ColWidth | iGLayoutFlags.ColOrder;
    this._grid.Name = "_grid";
    this._grid.ReadOnly = true;
    this._grid.RowMode = true;
    this._grid.VScrollBar.Visibility = iGScrollBarVisibility.Always;
    this._grid.CellClick += new iGCellClickEventHandler(this.Grid_CellClick);
    this._grid.ColWidthEndChange += new iGColWidthEventHandler(this.Grid_ColWidthEndChange);
    this._grid.ColWidthChanging += new iGColWidthEventHandler(this.Grid_ColWidthChanging);
    this._grid.Resize += new EventHandler(this.Grid_Resize);
    this.panel1.Controls.Add((Control) this._processedObjectsCountLabel);
    this.panel1.Controls.Add((Control) this._tracingCheckBox);
    this.panel1.Controls.Add((Control) this._errorsCheckBox);
    this.panel1.Controls.Add((Control) this.lbFilter);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    componentResourceManager.ApplyResources((object) this._processedObjectsCountLabel, "_processedObjectsCountLabel");
    this._processedObjectsCountLabel.Name = "_processedObjectsCountLabel";
    componentResourceManager.ApplyResources((object) this._tracingCheckBox, "_tracingCheckBox");
    this._tracingCheckBox.Checked = true;
    this._tracingCheckBox.CheckState = CheckState.Checked;
    this._tracingCheckBox.Name = "_tracingCheckBox";
    this._tracingCheckBox.UseVisualStyleBackColor = true;
    this._tracingCheckBox.CheckedChanged += new EventHandler(this.TracingCheckBox_CheckedChanged);
    componentResourceManager.ApplyResources((object) this._errorsCheckBox, "_errorsCheckBox");
    this._errorsCheckBox.Checked = true;
    this._errorsCheckBox.CheckState = CheckState.Checked;
    this._errorsCheckBox.Name = "_errorsCheckBox";
    this._errorsCheckBox.UseVisualStyleBackColor = true;
    this._errorsCheckBox.CheckedChanged += new EventHandler(this.ErrorsCheckBox_CheckedChanged);
    componentResourceManager.ApplyResources((object) this.lbFilter, "lbFilter");
    this.lbFilter.Name = "lbFilter";
    this.ilState.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("ilState.ImageStream");
    this.ilState.TransparentColor = Color.Transparent;
    this.ilState.Images.SetKeyName(0, "pcsIncompatibilities.ico");
    this.ilState.Images.SetKeyName(1, "pcsContextNotFound.ico");
    this.ilState.Images.SetKeyName(2, "pcsException.ico");
    this.ilState.Images.SetKeyName(3, "pcsOptionNotFound.ico");
    this.ilState.Images.SetKeyName(4, "pcsOptionValueNotFound.ico");
    this.ilState.Images.SetKeyName(5, "pcsConfigured.ico");
    this.ilState.Images.SetKeyName(6, "pcsNone.ico");
    this.ilState.Images.SetKeyName(7, "gear_information.png");
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this._grid);
    this.Controls.Add((Control) this.tbTracing);
    this.Controls.Add((Control) this.panel1);
    this.Guid = new Guid("c34700ef-1d61-414b-b827-20144469e9a9");
    this.HideOnClose = true;
    this.Name = nameof (CompositionTracingView);
    this.ShowHint = DockState.DockBottomAutoHide;
    ((ISupportInitialize) this._grid).EndInit();
    this.panel1.ResumeLayout(false);
    this.panel1.PerformLayout();
    this.ResumeLayout(false);
  }

  private enum MessageType
  {
    Error,
    Trace,
  }
}
