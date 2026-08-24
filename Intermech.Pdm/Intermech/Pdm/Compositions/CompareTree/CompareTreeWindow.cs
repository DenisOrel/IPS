// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.CompareTreeWindow
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Infralution.Controls.VirtualTree;
using Intermech.Bars;
using Intermech.Client.Core;
using Intermech.DataFormats;
using Intermech.Docking;
using Intermech.Document.Client;
using Intermech.Document.Model;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Document;
using Intermech.Interfaces.Pdm;
using Intermech.IO;
using Intermech.Kernel.Search;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Queries;
using Intermech.Pdm.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal class CompareTreeWindow : 
  FIltratedDocControl,
  ICommandTarget,
  IFiltrationClass,
  IFiltrationRuleClass,
  ICompareTreeWindowRestoreData,
  ICompareTreeWindowSettings
{
  private AdvancedServiceContainer _services;
  private RecordMapping _mapping;
  private ColumnsConfiguration _columnsConfiguration;
  private static Guid _controlGuid = new Guid("{FB004EDB-01A6-46AD-A3CC-41EEC08539D2}");
  private BackgroundCompositionReader _reader;
  private TreeViewFiltrationPanels _treeViewFiltrationPanelsLeft;
  private TreeViewFiltrationPanels _treeViewFiltrationPanelsRight;
  private TreeStateSyncronyzer _stateSyncronyzerTreeViews;
  private TreeStateSyncronyzer _stateSyncronyzerObjectAttributes;
  private TreeStateSyncronyzer _stateSyncronyzerRelationAttributes;
  private TabStateSyncronyzer _tabStateSyncronyzer;
  private bool _executing;
  private Guid _currentRuleID = Guid.Empty;
  private CompositionItem _rootItemLeft;
  private CompositionItem _rootItemRight;
  private INamedImageList _namedImageList;
  private bool _attributePanelShown;
  private IContainer components;
  private ContainerBarClientPanel containerBarClientPanel1;
  private StatusStrip statusStrip1;
  private iGCellStyle iGrid2DefaultCellStyle;
  private iGColHdrStyle iGrid2DefaultColHdrStyle;
  private iGCellStyle iGrid2RowTextColCellStyle;
  private SplitContainer splitContainer1;
  private ToolStripStatusLabel toolStripStatusLabel1;
  private ToolStripStatusLabel toolStripStatusLabel2;
  private ToolStripStatusLabel toolStripStatusLabel3;
  private CompareTreeView virtualTreeView1;
  private CompareTreeView virtualTreeView2;
  private SplitContainer splitContainer2;
  private SplitContainer splitContainer3;
  private Column column1;
  private Column column2;
  private AttributesControl attributesControlLeft;
  private AttributesControl attributesControlRigth;
  private ButtonItem buttonItem5;
  private BarManager barManager1;
  private ToolBarContainer leftBarDock;
  private ToolBarContainer rightBarDock;
  private ToolBarContainer bottomBarDock;
  private ToolBarContainer topBarDock;
  private Intermech.Bars.ToolBar tbMain;
  private ComboBoxItem cbRules;
  private ButtonItem biAction;
  private ToolBarContainer toolBarContainer1;
  private ToolBarContainer toolBarContainer2;
  private Panel panel1;
  private Intermech.Bars.ToolBar _filterToolbar;
  private ComboBoxItem cbFiltrationRule;
  private ButtonItem btRuleVariant;
  private ButtonItem btRuleBrowser;
  private ButtonItem btRuleHint;
  private ButtonItem useStoredExplicitPartVersionIDButtonItem;
  private Intermech.Bars.ToolBar toolBarEditingContexts;
  private LabelItem labelContext;
  private DropDownMenuItem contextsList;
  internal ButtonItem buttonEditingContextsBrowse;
  private Intermech.Bars.ToolBar _filterToolbar2;
  private ComboBoxItem cbFiltrationRule2;
  private ButtonItem btRuleVariant2;
  private ButtonItem btRuleBrowser2;
  private ButtonItem btRuleHint2;
  private ButtonItem useStoredExplicitPartVersionIDButtonItem2;
  private Intermech.Bars.ToolBar toolBarEditingContexts2;
  private LabelItem labelItem1;
  private DropDownMenuItem contextsList2;
  internal ButtonItem buttonEditingContextsBrowse2;
  private Panel panel2;
  private ButtonItem bRecursive;
  private ButtonItem biEditRule;
  private ButtonItem bRefresh;
  private Intermech.Bars.ToolBar tbAttributes;
  private ButtonItem biShowAttributes;
  private ButtonItem biAllAttributes;
  private ButtonItem bExchange;
  private ButtonItem biSortAZAndState;
  private ButtonItem biSortAZ;
  private ButtonItem bReport;
  private ButtonItem biAddRule;

  public CompareTreeWindow(long item1, long item2, Guid ruleID, bool recursive)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      RootItemFactory rootItemFactory = new RootItemFactory();
      this.Initialize(sessionKeeper.Session, rootItemFactory.GetItem(sessionKeeper.Session, item1), rootItemFactory.GetItem(sessionKeeper.Session, item2), new Guid?(ruleID), recursive);
    }
  }

  public CompareTreeWindow(IUserSession session, IDBTypedObjectID item1, IDBTypedObjectID item2)
  {
    RootItemFactory rootItemFactory = new RootItemFactory();
    Guid? ruleID;
    bool recursive;
    RuleToObjectTypeSettings.GetSettings(item1.ObjectType, out ruleID, out recursive);
    this.Initialize(session, rootItemFactory.GetItem(session, item1), rootItemFactory.GetItem(session, item2), ruleID, recursive);
    HybridDictionary settings = new HybridDictionary(0, true);
    if (!FormStorage.LoadLayout((Control) this, (IDictionary) settings))
      return;
    CompareTreeWindowSettings.SetSettings(settings, (ICompareTreeWindowSettings) this);
  }

  private void SetItemOrder(
    CompositionItem item1,
    CompositionItem item2,
    out CompositionItem itemLeft,
    out CompositionItem itemRight)
  {
    if (item1.ID == item2.ID && (item1.Version > item2.Version || item1.Version == item2.Version && item1.ObjectID < 0L && item2.ObjectID > 0L))
    {
      itemLeft = item2;
      itemRight = item1;
    }
    else
    {
      itemLeft = item1;
      itemRight = item2;
    }
  }

  private void Initialize(
    IUserSession session,
    CompositionItem item1,
    CompositionItem item2,
    Guid? ruleID = null,
    bool recursive = false)
  {
    this.SetItemOrder(item1, item2, out this._rootItemLeft, out this._rootItemRight);
    this._namedImageList = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
    this._columnsConfiguration = new ColumnsConfiguration(this._rootItemLeft.ObjectTypeID);
    this._services = new AdvancedServiceContainer();
    this._services.AddService(typeof (IFiltrationClass), (object) this.filtrationClass);
    this._services.AddService(typeof (IFiltrationService), (object) this.filtrationService);
    this._services.AddService(typeof (IViewState), (object) new ViewStateService(ViewStateFlags.ReadOnly));
    this.InitializeComponent();
    this.InitializeControls(session, ruleID);
    this.tbMain.ImageList = this._namedImageList.ImageList;
    this.biAction.ImageIndex = this._namedImageList.ImageIndex("imgStart");
    this.bRefresh.ImageIndex = this._namedImageList.ImageIndex("imgRefresh");
    this.biEditRule.ImageIndex = this._namedImageList.ImageIndex("imgRedPencil");
    this.bRecursive.ImageIndex = this._namedImageList.ImageIndex("imgExpandComposition");
    this.biShowAttributes.ImageIndex = this._namedImageList.ImageIndex("imgProp");
    this.biAllAttributes.ImageIndex = this._namedImageList.ImageIndex("imgPrintPreview");
    this.bExchange.ImageIndex = this._namedImageList.ImageIndex("imgExchange");
    this.bReport.ImageIndex = this._namedImageList.ImageIndex("imgReport");
    this.biAddRule.ImageIndex = this._namedImageList.ImageIndex("imgNewCompareRule");
    this.tbAttributes.ImageList = this._namedImageList.ImageList;
    this.toolBarEditingContexts.ImageList = this._namedImageList.ImageList;
    this.toolBarEditingContexts2.ImageList = this._namedImageList.ImageList;
    this.virtualTreeView1.Services = (IServiceProvider) this._services;
    this.virtualTreeView2.Services = (IServiceProvider) this._services;
    this.virtualTreeView1.SelectionChanged += new EventHandler(this.VirtualTreeView1_SelectionChanged);
    this.virtualTreeView2.SelectionChanged += new EventHandler(this.VirtualTreeView2_SelectionChanged);
    this._reader = new BackgroundCompositionReader();
    this._reader.CompositionReaderChangeStateEvent += new CompositionReaderChangeStateDelegate(this.CompositionReaderChangeStateEvent);
    if (ServicesManager.GetService(typeof (INamedImageList)) is INamedImageList service)
    {
      this.TabImageIndex = service.ImageIndex("imgCompCompare");
      this.ShowImageInDocumentTab = true;
    }
    this.bRecursive.Checked = recursive;
    this.Name = PDMPluginConsts.menuTreeCompare;
    this.Guid = CompareTreeWindow._controlGuid;
    this.SetColumns(false);
  }

  private void CompositionReaderChangeStateEvent(
    object sender,
    CompositionReaderChangeStateEventArgs e)
  {
    try
    {
      this.Invoke((Delegate) new CompositionReaderChangeStateDelegate(this.OnCompositionReaderChangeStateEvent), sender, (object) e);
    }
    catch
    {
    }
  }

  private void OnCompositionReaderChangeStateEvent(
    object sender,
    CompositionReaderChangeStateEventArgs e)
  {
    switch (e.State)
    {
      case BackgroundState.Empty:
        this.ClearGrids();
        break;
      case BackgroundState.Error:
        this.ClearGrids();
        int num = (int) MessageBox.Show(e.ErrorException.Message);
        break;
      case BackgroundState.Reading:
        this.ClearGrids();
        this.SetEnabledControls(false);
        this.biAction.ImageIndex = this._namedImageList.ImageIndex("imgStop2");
        this.biAction.ToolTipText = "Остановить выполнение сравнения";
        break;
      case BackgroundState.Fill:
        this.virtualTreeView1.DataSource = (object) e.Result[0];
        this.virtualTreeView2.DataSource = (object) e.Result[1];
        this.UpdateRows(false);
        break;
    }
    if (e.State == BackgroundState.Reading)
      return;
    this._executing = false;
    this.SetEnabledControls(true);
    this.biAction.ImageIndex = this._namedImageList.ImageIndex("imgStart");
    this.biAction.ToolTipText = "Выполнить сравнение";
  }

  private void UpdateRows(bool reloadChildren)
  {
    this.virtualTreeView1.UpdateRows(reloadChildren);
    this.virtualTreeView2.UpdateRows(reloadChildren);
    this.virtualTreeView1.SelectedRow = this.virtualTreeView1.RootRow;
    this.virtualTreeView2.SelectedRow = this.virtualTreeView2.RootRow;
  }

  private void RefreshCompareRules(Guid? selectedRule)
  {
    this.cbRules.Items.Clear();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      List<int> objectTypes = new List<int>()
      {
        this._rootItemLeft.ObjectTypeID
      };
      if (this._rootItemLeft.ObjectTypeID != this._rootItemRight.ObjectTypeID)
        objectTypes.Add(this._rootItemRight.ObjectTypeID);
      CompareRuleComboItemsList ruleComboItemsList = CompareRuleComboItemsList.Load(sessionKeeper.Session, objectTypes);
      if (ruleComboItemsList.Count == 0)
        throw new Exception("Отсутствуют доступные правила сравнения! Сравнение составов без правила невозможно.");
      int num = 0;
      if (selectedRule.HasValue)
      {
        Guid? nullable = selectedRule;
        Guid empty = Guid.Empty;
        if ((nullable.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() != empty ? 1 : 0) : 0) : 1) != 0)
        {
          for (int index = 0; index < ruleComboItemsList.Count; ++index)
          {
            if (ruleComboItemsList[index].RuleID.Equals((object) selectedRule))
              num = index;
            this.cbRules.Items.Add((object) ruleComboItemsList[index]);
          }
          goto label_13;
        }
      }
      this.cbRules.Items.AddRange((object[]) ruleComboItemsList.ToArray());
label_13:
      this.cbRules.ComboBox.SelectedIndex = num;
    }
  }

  private void InitializeControls(IUserSession session, Guid? ruleID)
  {
    this.virtualTreeView1.Tag = (object) 1;
    this.virtualTreeView2.Tag = (object) 2;
    this.RebuildColumns();
    this.virtualTreeView1.DataSource = (object) this._rootItemLeft;
    this.virtualTreeView2.DataSource = (object) this._rootItemRight;
    this.UpdateRows(false);
    this.splitContainer2.Panel2Collapsed = true;
    this.RefreshCompareRules(ruleID);
    this._stateSyncronyzerTreeViews = new TreeStateSyncronyzer((Intermech.VirtualTreeView.VirtualTreeView) this.virtualTreeView1, (Intermech.VirtualTreeView.VirtualTreeView) this.virtualTreeView2, this.virtualTreeView1.VScrollBar, this.virtualTreeView2.VScrollBar);
    this._stateSyncronyzerTreeViews.TreeRowExpandedEvent += new TreeRowExpanded(this.TreeRowExpandedEvent);
    this._treeViewFiltrationPanelsLeft = new TreeViewFiltrationPanels();
    this._treeViewFiltrationPanelsLeft.Create(this._filterToolbar, this.cbFiltrationRule, this.btRuleBrowser, this.btRuleVariant, this.btRuleHint, this.useStoredExplicitPartVersionIDButtonItem, this.buttonEditingContextsBrowse, this.contextsList, this.toolBarContainer1);
    this._treeViewFiltrationPanelsRight = new TreeViewFiltrationPanels();
    this._treeViewFiltrationPanelsRight.Create(this._filterToolbar2, this.cbFiltrationRule2, this.btRuleBrowser2, this.btRuleVariant2, this.btRuleHint2, this.useStoredExplicitPartVersionIDButtonItem2, this.buttonEditingContextsBrowse2, this.contextsList2, this.toolBarContainer2);
    this._stateSyncronyzerObjectAttributes = new TreeStateSyncronyzer((Intermech.VirtualTreeView.VirtualTreeView) this.attributesControlLeft.ControlObjectAttributes, (Intermech.VirtualTreeView.VirtualTreeView) this.attributesControlRigth.ControlObjectAttributes, this.attributesControlLeft.ControlObjectAttributes.VScrollBar, this.attributesControlRigth.ControlObjectAttributes.VScrollBar);
    this._stateSyncronyzerRelationAttributes = new TreeStateSyncronyzer((Intermech.VirtualTreeView.VirtualTreeView) this.attributesControlLeft.ControlRelationAttributes, (Intermech.VirtualTreeView.VirtualTreeView) this.attributesControlRigth.ControlRelationAttributes, this.attributesControlLeft.ControlRelationAttributes.VScrollBar, this.attributesControlRigth.ControlRelationAttributes.VScrollBar);
    this._tabStateSyncronyzer = new TabStateSyncronyzer(this.attributesControlLeft.TabControl, this.attributesControlRigth.TabControl);
  }

  private void TreeRowExpandedEvent(object sender, TreeRowExpandedEventArgs e)
  {
    if (!(e.Row1.Item is CompositionItem compositionItem1) || !(e.Row2.Item is CompositionItem compositionItem2) || compositionItem1.Handled || e.Row1.ParentRow == null || compositionItem1.Count == 0)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      AttributesCompareFacade attributesCompareFacade = new AttributesCompareFacade(this._currentRuleID);
      this.Cursor = Cursors.WaitCursor;
      try
      {
        bool checkAttributesExists = ((ICompareTreeSettingsService) ServicesManager.GetService(typeof (ICompareTreeSettingsService))).CheckExistsAttributes(this._currentRuleID);
        attributesCompareFacade.CompareChildItems(sessionKeeper.Session, compositionItem1, compositionItem2, checkAttributesExists);
        compositionItem1.Handled = true;
        compositionItem2.Handled = true;
      }
      finally
      {
        this.Cursor = Cursors.Default;
      }
    }
  }

  protected override void OnClosing(CancelEventArgs e)
  {
    this._columnsConfiguration.SaveColumns((Intermech.VirtualTreeView.VirtualTreeView) this.virtualTreeView1, (Intermech.VirtualTreeView.VirtualTreeView) this.virtualTreeView2);
    this.Do_DeleteFiltrationSettings();
    IAdditionalCompositionFiltrationService service = ServicesManager.GetService(typeof (IAdditionalCompositionFiltrationService)) as IAdditionalCompositionFiltrationService;
    service.OnToolBarClosed(this._treeViewFiltrationPanelsLeft.AddCommandGuid);
    service.OnToolBarClosed(this._treeViewFiltrationPanelsRight.AddCommandGuid);
  }

  private void SetEnabledControls(bool enable)
  {
    this.splitContainer2.Enabled = enable;
    this.cbRules.Enabled = enable;
    this.tbAttributes.Enabled = enable;
    this.bRecursive.Enabled = enable;
    this.biEditRule.Enabled = enable;
    this.bRefresh.Enabled = enable;
    this.bExchange.Enabled = enable;
  }

  private void ClearGrids()
  {
    if (this.virtualTreeView1.DataSource is CompositionItem dataSource1)
    {
      dataSource1.Clear();
      dataSource1.CompositionItemFlag = CompositionItemFlags.Equal;
    }
    if (this.virtualTreeView2.DataSource is CompositionItem dataSource2)
    {
      dataSource2.Clear();
      dataSource2.CompositionItemFlag = CompositionItemFlags.Equal;
    }
    this.UpdateRows(true);
  }

  private void SetColumns(bool rebuild)
  {
    this.virtualTreeView1.Columns.Clear();
    this.virtualTreeView2.Columns.Clear();
    if (rebuild)
      this.RebuildColumns();
    List<NodeColumnID> columns = this.Columns;
    foreach (NodeColumnID columnID in columns)
      this.AddTreeViewColumn((Intermech.VirtualTreeView.VirtualTreeView) this.virtualTreeView1, columnID);
    foreach (NodeColumnID columnID in columns)
      this.AddTreeViewColumn((Intermech.VirtualTreeView.VirtualTreeView) this.virtualTreeView2, columnID);
  }

  private void AddTreeViewColumn(Intermech.VirtualTreeView.VirtualTreeView view, NodeColumnID columnID)
  {
    int attributeId = columnID.AttributeID;
    IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(attributeId);
    string name = attributeType.Name;
    AttributesTypeHelper.GetTypeOfAttributeValue(attributeType.FieldType == FieldTypes.ftSystem ? attributeType.RealFieldType : attributeType.FieldType);
    Column column = new Column()
    {
      Caption = name,
      Name = attributeId.ToString(),
      Sortable = false,
      Width = this._columnsConfiguration.GetColumnWidth(attributeId, (int) view.Tag)
    };
    column.CellStyle.HorzAlignment = StringAlignment.Near;
    view.Columns.Add(column);
  }

  private void RebuildColumns()
  {
    this._mapping = new RecordMapping();
    IColumnSchemes service = (IColumnSchemes) ServicesManager.GetService(typeof (IColumnSchemes));
    foreach (NodeColumnID column in this.Columns)
    {
      if (column.AttributeID < 0)
        this.AddColumn(service, Intermech.Navigator.Consts.ObjectObligatoryColumnSchemeGuid, column);
      else
        this.AddColumn(service, Intermech.Navigator.Consts.ObjectColumnSchemeGuid, column);
    }
  }

  private void AddColumn(IColumnSchemes schemes, Guid schemeGuid, NodeColumnID columnID)
  {
    object columnID1 = columnID.AttributeID >= 0 ? (object) columnID.AttributeID : (object) (ObligatoryObjectAttributes) columnID.AttributeID;
    NodeColumn column = schemes.CreateColumn(schemeGuid, columnID1, NodeColumnSortOrder.None, 0);
    INodeColumnTransform defaultTransform = schemes.GetDefaultTransform(schemeGuid, (object) columnID.AttributeID);
    this._mapping.RegisterColumn(column, (object) columnID, defaultTransform);
  }

  private List<NodeColumnID> Columns
  {
    get
    {
      return new List<NodeColumnID>()
      {
        new NodeColumnID((object) ObligatoryObjectAttributes.CAPTION, AttributeSourceTypes.Object),
        new NodeColumnID((object) ControlsHelper.AttributeChangesID, AttributeSourceTypes.Object),
        new NodeColumnID((object) ObligatoryObjectAttributes.F_CHKOUT_BY, AttributeSourceTypes.Object),
        new NodeColumnID((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object),
        new NodeColumnID((object) ObligatoryObjectAttributes.F_OWNER_ID, AttributeSourceTypes.Object),
        new NodeColumnID((object) ObligatoryObjectAttributes.F_LEVEL_ID, AttributeSourceTypes.Object),
        new NodeColumnID((object) ObligatoryObjectAttributes.F_PROJECT_ID, AttributeSourceTypes.Object)
      };
    }
  }

  public override string Text => "Дерево сравнения";

  public long LeftItemID => this._rootItemLeft.ObjectID;

  public long RightItemID => this._rootItemRight.ObjectID;

  public Guid RuleID => ((CompareRuleComboItem) this.cbRules.ComboBox.SelectedItem).RuleID;

  public bool Recursive => this.bRecursive.Checked;

  private void biAction_Click(object sender, EventArgs e)
  {
    this._executing = !this._executing;
    if (this._executing)
    {
      this._currentRuleID = ((CompareRuleComboItem) this.cbRules.ComboBox.SelectedItem).RuleID;
      this._reader.Start(new BackgroundCompositionReaderArgs(this._currentRuleID, (CompositionItem) this.virtualTreeView1.DataSource, this._treeViewFiltrationPanelsLeft.CompositionFiltrationSettings, (CompositionItem) this.virtualTreeView2.DataSource, this._treeViewFiltrationPanelsRight.CompositionFiltrationSettings, this.bRecursive.Checked));
    }
    else
      this._reader.Stop();
  }

  public bool Execute(ICommandState commandState) => false;

  public bool QueryStatus(ICommandState commandState) => false;

  protected override string GetPersistString()
  {
    return CompareTreeWindowRestore.GetPersistString((ICompareTreeWindowRestoreData) this);
  }

  public static DockControl RestoreWindowCallback(Guid guid, string persistString)
  {
    return !(guid != CompareTreeWindow._controlGuid) ? (DockControl) CompareTreeWindowRestore.RestoreWindow(persistString) : (DockControl) null;
  }

  private void bRecursive_Click(object sender, EventArgs e)
  {
    this.bRecursive.Checked = !this.bRecursive.Checked;
  }

  private void biEditRule_Click(object sender, EventArgs e)
  {
    CompareRuleComboItem selectedItem = (CompareRuleComboItem) this.cbRules.ComboBox.SelectedItem;
    if (selectedItem.IsVirtual)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(selectedItem.RuleID);
      int num = (int) PropertiesWindow.Execute(string.Empty, string.Empty, objectInfo.ObjectID);
      if (selectedItem.Name.Equals(objectInfo.Caption))
        return;
      this.RefreshCompareRules(new Guid?(selectedItem.RuleID));
    }
  }

  private void BiAddRule_Click(object sender, EventArgs e)
  {
    long objectByTypeDialog = (ServicesManager.GetService(typeof (IObjectCreatorService)) as IObjectCreatorService).CreateObjectByTypeDialog(new Guid[2]
    {
      PDMHelper.objtypeCommonCompositionRules,
      PDMHelper.objtypePersonalCompositionRules
    });
    switch (objectByTypeDialog)
    {
      case -1:
        break;
      case 0:
        break;
      default:
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          this.RefreshCompareRules(new Guid?(sessionKeeper.Session.GetObjectInfo(objectByTypeDialog).VersionGuid));
          break;
        }
    }
  }

  private void biShowAttributes_Click(object sender, EventArgs e)
  {
    this.biShowAttributes.Checked = !this.biShowAttributes.Checked;
    this.splitContainer2.Panel2Collapsed = !this.biShowAttributes.Checked;
    if (!this._attributePanelShown)
    {
      this.attributesControlLeft.ResizeColumns();
      this.attributesControlRigth.ResizeColumns();
      this._attributePanelShown = true;
    }
    this.RefreshAttributes();
  }

  private void biAllAttributes_Click(object sender, EventArgs e)
  {
    this.biAllAttributes.Checked = !this.biAllAttributes.Checked;
    this.OnSelectionChanged(this.virtualTreeView1, this.attributesControlLeft);
    this.OnSelectionChanged(this.virtualTreeView2, this.attributesControlRigth);
  }

  private void RefreshAttributes()
  {
    if (!this.biShowAttributes.Checked)
      return;
    this.VirtualTreeView2_SelectionChanged((object) this, (EventArgs) null);
    this.VirtualTreeView1_SelectionChanged((object) this, (EventArgs) null);
  }

  private void VirtualTreeView2_SelectionChanged(object sender, EventArgs e)
  {
    this.OnSelectionChanged(this.virtualTreeView2, this.attributesControlRigth);
  }

  private void VirtualTreeView1_SelectionChanged(object sender, EventArgs e)
  {
    this.OnSelectionChanged(this.virtualTreeView1, this.attributesControlLeft);
  }

  private void OnSelectionChanged(
    CompareTreeView compareTreeView,
    AttributesControl attributesControl)
  {
    CompositionItem selectedItem = (CompositionItem) compareTreeView.SelectedItem;
    bool isRoot = selectedItem != null && !selectedItem.Empty && selectedItem.PrjLinkID == 0L;
    attributesControl.RefreshAttributes(selectedItem, this.biAllAttributes.Checked, this.biSortAZAndState.Checked, isRoot);
  }

  private void bExchange_Click(object sender, EventArgs e)
  {
    CompositionItem dataSource = (CompositionItem) this.virtualTreeView1.DataSource;
    this.virtualTreeView1.DataSource = (object) (CompositionItem) this.virtualTreeView2.DataSource;
    this.virtualTreeView2.DataSource = (object) dataSource;
    this.ClearGrids();
  }

  private void biSort_Click(object sender, EventArgs e)
  {
    this.biSortAZAndState.Checked = !this.biSortAZAndState.Checked;
    this.biSortAZ.Checked = !this.biSortAZ.Checked;
    this.OnSelectionChanged(this.virtualTreeView1, this.attributesControlLeft);
    this.OnSelectionChanged(this.virtualTreeView2, this.attributesControlRigth);
  }

  private void cbRules_SelectedValueChanged(object sender, EventArgs e)
  {
    this.biEditRule.Enabled = !((CompareRuleComboItem) this.cbRules.ComboBox.SelectedItem).IsVirtual;
  }

  private void bReport_Click(object sender, EventArgs e)
  {
    object[] objArray = Intermech.Navigator.SelectionWindow.Select("Выберите скрипт формирования отчета", (IDescriptor) new Intermech.Navigator.DBObjectTypes.Descriptor(MetaDataHelper.GetObjectTypeID("cadd9b67-306c-11d8-b4e9-00304f19f545")), typeof (IDBTypedObjectID), SelectionOptions.Default | SelectionOptions.DisableMultiselect);
    if (objArray == null || objArray.Length == 0)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(((IDBTypedObjectID) objArray[0]).ObjectID);
      IDBAttribute attributeByGuid1 = dbObject.GetAttributeByGuid(new Guid("cad00366-306c-11d8-b4e9-00304f19f545"));
      if (attributeByGuid1 == null || attributeByGuid1.IsNull)
      {
        int num1 = (int) MessageBox.Show("Не найден текст сценария!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string code = (string) attributeByGuid1.Value;
        IDBAttribute attributeByGuid2 = dbObject.GetAttributeByGuid(new Guid("cad00071-306c-11d8-b4e9-00304f19f545"));
        if (attributeByGuid2 == null || attributeByGuid2.AsInteger == 0L)
        {
          int num2 = (int) MessageBox.Show("Не указан шаблон документа для сценария!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          long asInteger = attributeByGuid2.AsInteger;
          IDBObject templateObject = sessionKeeper.Session.GetObject(asInteger, true);
          ImDocumentData template = (ImDocumentData) null;
          Stream stream = this.LoadXMLFromObject(templateObject, sessionKeeper.Session.IdentHelper.FileAttributeID);
          if (stream != null)
            template = ImDocumentData.LoadFromXml(stream);
          ImDocument documentFromTemplate = (ImDocument) ImDocumentData.CreateDocumentFromTemplate(template);
          string str = ScriptExecHelper.IsolatedExecScript(code, CSharpScriptInvocationOptions.Default, (object) sessionKeeper.Session, (object) documentFromTemplate, (object) (CompositionItem) this.virtualTreeView1.DataSource, (object) (CompositionItem) this.virtualTreeView2.DataSource);
          if (string.IsNullOrEmpty(str))
          {
            documentFromTemplate.UpdateLayout(0, true, false);
            DocumentEditorPlugin.Instance.OpenImDocument(documentFromTemplate);
          }
          else
          {
            int num3 = (int) MessageBox.Show($"Ошибка при выполнении сценария: {str}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
      }
    }
  }

  protected Stream LoadXMLFromObject(IDBObject templateObject, int fileAttributeID)
  {
    IDBAttribute attributeById = templateObject.GetAttributeByID(fileAttributeID);
    if (attributeById != null)
    {
      ImChunkedStream outStream = new ImChunkedStream();
      IBlobReader blobReader = attributeById as IBlobReader;
      BlobInformation blobInformation = blobReader.OpenBlob(0);
      try
      {
        if (blobInformation.RealFileSize > 0L)
        {
          if (blobInformation.ArcMethod == ArcMethods.ZLibPacked)
          {
            IPackedStream service = ServiceUtils.GetService<IPackedStream>((object) ApplicationServices.Container, true);
            using (MemoryStream inStream = new MemoryStream(blobReader.ReadDataBlock()))
            {
              inStream.Position = 0L;
              service.UnpackStream((Stream) outStream, (Stream) inStream);
            }
          }
          else
            outStream.Write(blobReader.ReadDataBlock(), 0, Convert.ToInt32(blobInformation.PackedFileSize));
          outStream.Position = 0L;
          return (Stream) outStream;
        }
      }
      finally
      {
        blobReader.CloseBlob();
      }
    }
    return (Stream) null;
  }

  private void Refresh_Click(object sender, EventArgs e)
  {
    this.RefreshCompareRules(new Guid?(this.cbRules.ComboBox.SelectedItem is CompareRuleComboItem selectedItem ? selectedItem.RuleID : Guid.Empty));
  }

  public override void OnClosed(EventArgs e)
  {
    RuleToObjectTypeSettings.SetSettings(this._rootItemLeft.ObjectTypeID, this.RuleID, this.Recursive);
    base.OnClosed(e);
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
    this.containerBarClientPanel1 = new ContainerBarClientPanel();
    this.statusStrip1 = new StatusStrip();
    this.toolStripStatusLabel1 = new ToolStripStatusLabel();
    this.toolStripStatusLabel2 = new ToolStripStatusLabel();
    this.toolStripStatusLabel3 = new ToolStripStatusLabel();
    this.iGrid2DefaultCellStyle = new iGCellStyle(true);
    this.iGrid2DefaultColHdrStyle = new iGColHdrStyle(true);
    this.iGrid2RowTextColCellStyle = new iGCellStyle(true);
    this.splitContainer1 = new SplitContainer();
    this.panel1 = new Panel();
    this.virtualTreeView1 = new CompareTreeView();
    this.toolBarContainer1 = new ToolBarContainer();
    this._filterToolbar = new Intermech.Bars.ToolBar();
    this.cbFiltrationRule = new ComboBoxItem();
    this.btRuleVariant = new ButtonItem();
    this.btRuleBrowser = new ButtonItem();
    this.btRuleHint = new ButtonItem();
    this.useStoredExplicitPartVersionIDButtonItem = new ButtonItem();
    this.toolBarEditingContexts = new Intermech.Bars.ToolBar();
    this.labelContext = new LabelItem();
    this.contextsList = new DropDownMenuItem();
    this.buttonEditingContextsBrowse = new ButtonItem();
    this.barManager1 = new BarManager();
    this.panel2 = new Panel();
    this.virtualTreeView2 = new CompareTreeView();
    this.toolBarContainer2 = new ToolBarContainer();
    this._filterToolbar2 = new Intermech.Bars.ToolBar();
    this.cbFiltrationRule2 = new ComboBoxItem();
    this.btRuleVariant2 = new ButtonItem();
    this.btRuleBrowser2 = new ButtonItem();
    this.btRuleHint2 = new ButtonItem();
    this.useStoredExplicitPartVersionIDButtonItem2 = new ButtonItem();
    this.toolBarEditingContexts2 = new Intermech.Bars.ToolBar();
    this.labelItem1 = new LabelItem();
    this.contextsList2 = new DropDownMenuItem();
    this.buttonEditingContextsBrowse2 = new ButtonItem();
    this.splitContainer2 = new SplitContainer();
    this.splitContainer3 = new SplitContainer();
    this.attributesControlLeft = new AttributesControl();
    this.attributesControlRigth = new AttributesControl();
    this.column1 = new Column();
    this.column2 = new Column();
    this.buttonItem5 = new ButtonItem();
    this.leftBarDock = new ToolBarContainer();
    this.rightBarDock = new ToolBarContainer();
    this.bottomBarDock = new ToolBarContainer();
    this.topBarDock = new ToolBarContainer();
    this.tbMain = new Intermech.Bars.ToolBar();
    this.cbRules = new ComboBoxItem();
    this.biEditRule = new ButtonItem();
    this.bRefresh = new ButtonItem();
    this.bReport = new ButtonItem();
    this.biAction = new ButtonItem();
    this.bRecursive = new ButtonItem();
    this.bExchange = new ButtonItem();
    this.tbAttributes = new Intermech.Bars.ToolBar();
    this.biShowAttributes = new ButtonItem();
    this.biAllAttributes = new ButtonItem();
    this.biSortAZAndState = new ButtonItem();
    this.biSortAZ = new ButtonItem();
    this.biAddRule = new ButtonItem();
    this.statusStrip1.SuspendLayout();
    this.splitContainer1.BeginInit();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.Panel2.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    this.panel1.SuspendLayout();
    this.virtualTreeView1.BeginInit();
    this.toolBarContainer1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.virtualTreeView2.BeginInit();
    this.toolBarContainer2.SuspendLayout();
    this.splitContainer2.BeginInit();
    this.splitContainer2.Panel1.SuspendLayout();
    this.splitContainer2.Panel2.SuspendLayout();
    this.splitContainer2.SuspendLayout();
    this.splitContainer3.BeginInit();
    this.splitContainer3.Panel1.SuspendLayout();
    this.splitContainer3.Panel2.SuspendLayout();
    this.splitContainer3.SuspendLayout();
    this.topBarDock.SuspendLayout();
    this.SuspendLayout();
    this.containerBarClientPanel1.Location = new Point(2, 27);
    this.containerBarClientPanel1.Name = "containerBarClientPanel1";
    this.containerBarClientPanel1.Size = new Size(196, (int) byte.MaxValue);
    this.containerBarClientPanel1.TabIndex = 0;
    this.statusStrip1.Items.AddRange(new ToolStripItem[3]
    {
      (ToolStripItem) this.toolStripStatusLabel1,
      (ToolStripItem) this.toolStripStatusLabel2,
      (ToolStripItem) this.toolStripStatusLabel3
    });
    this.statusStrip1.Location = new Point(0, 689);
    this.statusStrip1.Name = "statusStrip1";
    this.statusStrip1.Size = new Size(1068, 22);
    this.statusStrip1.SizingGrip = false;
    this.statusStrip1.TabIndex = 1;
    this.statusStrip1.Text = "statusStrip1";
    this.toolStripStatusLabel1.BackColor = Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 192 /*0xC0*/);
    this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
    this.toolStripStatusLabel1.Size = new Size(46, 17);
    this.toolStripStatusLabel1.Text = "Удален";
    this.toolStripStatusLabel2.ActiveLinkColor = Color.Red;
    this.toolStripStatusLabel2.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192 /*0xC0*/);
    this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
    this.toolStripStatusLabel2.Size = new Size(56, 17);
    this.toolStripStatusLabel2.Text = "Изменен";
    this.toolStripStatusLabel3.BackColor = Color.FromArgb(192 /*0xC0*/, (int) byte.MaxValue, 192 /*0xC0*/);
    this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
    this.toolStripStatusLabel3.Size = new Size(61, 17);
    this.toolStripStatusLabel3.Text = "Добавлен";
    this.splitContainer1.Dock = DockStyle.Fill;
    this.splitContainer1.IsSplitterFixed = true;
    this.splitContainer1.Location = new Point(0, 0);
    this.splitContainer1.Name = "splitContainer1";
    this.splitContainer1.Panel1.Controls.Add((Control) this.panel1);
    this.splitContainer1.Panel1.Controls.Add((Control) this.toolBarContainer1);
    this.splitContainer1.Panel2.Controls.Add((Control) this.panel2);
    this.splitContainer1.Panel2.Controls.Add((Control) this.toolBarContainer2);
    this.splitContainer1.Size = new Size(1068, 439);
    this.splitContainer1.SplitterDistance = 531;
    this.splitContainer1.TabIndex = 5;
    this.panel1.Controls.Add((Control) this.virtualTreeView1);
    this.panel1.Dock = DockStyle.Fill;
    this.panel1.Location = new Point(0, 50);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(531, 389);
    this.panel1.TabIndex = 13;
    this.virtualTreeView1.AllowDrop = true;
    this.virtualTreeView1.AllowMultiSelect = false;
    this.virtualTreeView1.Control = (object) null;
    this.virtualTreeView1.DisableHeaderContextMenu = true;
    this.virtualTreeView1.Dock = DockStyle.Fill;
    this.virtualTreeView1.ImageList = (ImageList) null;
    this.virtualTreeView1.Location = new Point(0, 0);
    this.virtualTreeView1.Name = "virtualTreeView1";
    this.virtualTreeView1.SelectedItems = (ISelectedItems) null;
    this.virtualTreeView1.Services = (IServiceProvider) null;
    this.virtualTreeView1.Size = new Size(531, 389);
    this.virtualTreeView1.TabIndex = 3;
    this.toolBarContainer1.Controls.Add((Control) this._filterToolbar);
    this.toolBarContainer1.Controls.Add((Control) this.toolBarEditingContexts);
    this.toolBarContainer1.Dock = DockStyle.Top;
    this.toolBarContainer1.Guid = new Guid("d6f04465-9e6c-4b05-a4d6-896a689963fe");
    this.toolBarContainer1.Location = new Point(0, 0);
    this.toolBarContainer1.Manager = this.barManager1;
    this.toolBarContainer1.Name = "toolBarContainer1";
    this.toolBarContainer1.Size = new Size(531, 50);
    this.toolBarContainer1.TabIndex = 11;
    this.toolBarContainer1.Text = "BarDock";
    this._filterToolbar.AllowVerticalDock = false;
    this._filterToolbar.DockLine = 1;
    this._filterToolbar.FullMenus = true;
    this._filterToolbar.Guid = new Guid("7b9a8adc-5be9-42fb-a7e2-91052e0fcbd9");
    this._filterToolbar.Hidden = false;
    this._filterToolbar.Items.AddRange(new ToolbarItemBase[5]
    {
      (ToolbarItemBase) this.cbFiltrationRule,
      (ToolbarItemBase) this.btRuleVariant,
      (ToolbarItemBase) this.btRuleBrowser,
      (ToolbarItemBase) this.btRuleHint,
      (ToolbarItemBase) this.useStoredExplicitPartVersionIDButtonItem
    });
    this._filterToolbar.Location = new Point(2, 0);
    this._filterToolbar.MinimumFloatingSize = new Size(250, 30);
    this._filterToolbar.Name = "_filterToolbar";
    this._filterToolbar.Size = new Size(489, 26);
    this._filterToolbar.StretchItem = (ToolbarItemBase) this.cbFiltrationRule;
    this._filterToolbar.TabIndex = 7;
    this._filterToolbar.Text = "Фильтрация состава";
    this.cbFiltrationRule.CommandName = "cbFiltrationRule";
    this.cbFiltrationRule.DefaultText = "<Выберите правило подбора версий>";
    this.cbFiltrationRule.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbFiltrationRule.Locked = true;
    this.cbFiltrationRule.MinimumControlWidth = 100;
    this.cbFiltrationRule.MinimumSize = 356;
    this.cbFiltrationRule.Padding.Bottom = 0;
    this.cbFiltrationRule.Padding.Left = 1;
    this.cbFiltrationRule.Padding.Right = 1;
    this.cbFiltrationRule.Padding.Top = 0;
    this.cbFiltrationRule.Stretch = true;
    this.cbFiltrationRule.Text = "Правило:";
    this.cbFiltrationRule.ToolTipText = "Правило:";
    this.btRuleVariant.CommandName = "RuleVariant";
    this.btRuleVariant.ToolTipText = "Выбрать вариант значений для текущего правила подбора версий";
    this.btRuleBrowser.CommandName = "RuleBrowser";
    this.btRuleBrowser.ToolTipText = "Выбрать правило подбора версий";
    this.btRuleHint.BeginGroup = true;
    this.btRuleHint.CommandName = "RuleHint";
    this.btRuleHint.Locked = true;
    this.btRuleHint.ShowText = true;
    this.useStoredExplicitPartVersionIDButtonItem.BeginGroup = true;
    this.useStoredExplicitPartVersionIDButtonItem.CommandName = "useStoredExplicitPartVersionIDButtonItem";
    this.useStoredExplicitPartVersionIDButtonItem.ImageIndex = 2;
    this.useStoredExplicitPartVersionIDButtonItem.ToolTipText = "Режим 'По информации о конкретизации'";
    this.toolBarEditingContexts.AllowVerticalDock = false;
    this.toolBarEditingContexts.DockLine = 2;
    this.toolBarEditingContexts.FullMenus = true;
    this.toolBarEditingContexts.Guid = new Guid("7e41d6d7-f8e4-4809-b69a-09b9706dffef");
    this.toolBarEditingContexts.Hidden = false;
    this.toolBarEditingContexts.Items.AddRange(new ToolbarItemBase[3]
    {
      (ToolbarItemBase) this.labelContext,
      (ToolbarItemBase) this.contextsList,
      (ToolbarItemBase) this.buttonEditingContextsBrowse
    });
    this.toolBarEditingContexts.Location = new Point(2, 26);
    this.toolBarEditingContexts.Name = "toolBarEditingContexts";
    this.toolBarEditingContexts.Size = new Size(281, 24);
    this.toolBarEditingContexts.TabIndex = 26;
    this.toolBarEditingContexts.Text = "Текущий контекст редактирования";
    this.labelContext.CommandName = "labelContext";
    this.labelContext.Importance = ToolBarItemImportance.Highest;
    this.labelContext.Text = "Контекст редактирования:";
    this.labelContext.ToolTipText = "Текущий контекст редактирования";
    this.contextsList.CommandName = "contextsList";
    this.contextsList.Importance = ToolBarItemImportance.Highest;
    this.contextsList.MinimumSize = 64 /*0x40*/;
    this.contextsList.ShowText = true;
    this.buttonEditingContextsBrowse.CommandName = "buttonEditingContextsBrowse";
    this.buttonEditingContextsBrowse.ToolTipText = "Выбрать контекст редактирования и сделать его активным";
    this.barManager1.OwnerForm = (Form) null;
    this.panel2.Controls.Add((Control) this.virtualTreeView2);
    this.panel2.Dock = DockStyle.Fill;
    this.panel2.Location = new Point(0, 50);
    this.panel2.Name = "panel2";
    this.panel2.Size = new Size(533, 389);
    this.panel2.TabIndex = 12;
    this.virtualTreeView2.AllowDrop = true;
    this.virtualTreeView2.AllowMultiSelect = false;
    this.virtualTreeView2.Control = (object) null;
    this.virtualTreeView2.DisableHeaderContextMenu = true;
    this.virtualTreeView2.Dock = DockStyle.Fill;
    this.virtualTreeView2.ImageList = (ImageList) null;
    this.virtualTreeView2.Location = new Point(0, 0);
    this.virtualTreeView2.Name = "virtualTreeView2";
    this.virtualTreeView2.SelectedItems = (ISelectedItems) null;
    this.virtualTreeView2.Services = (IServiceProvider) null;
    this.virtualTreeView2.Size = new Size(533, 389);
    this.virtualTreeView2.TabIndex = 5;
    this.toolBarContainer2.Controls.Add((Control) this._filterToolbar2);
    this.toolBarContainer2.Controls.Add((Control) this.toolBarEditingContexts2);
    this.toolBarContainer2.Dock = DockStyle.Top;
    this.toolBarContainer2.Guid = new Guid("d6f04465-9e6c-4b05-a4d6-896a689963fe");
    this.toolBarContainer2.Location = new Point(0, 0);
    this.toolBarContainer2.Manager = this.barManager1;
    this.toolBarContainer2.Name = "toolBarContainer2";
    this.toolBarContainer2.Size = new Size(533, 50);
    this.toolBarContainer2.TabIndex = 11;
    this.toolBarContainer2.Text = "BarDock";
    this._filterToolbar2.AllowVerticalDock = false;
    this._filterToolbar2.DockLine = 1;
    this._filterToolbar2.FullMenus = true;
    this._filterToolbar2.Guid = new Guid("7b9a8adc-5be9-42fb-a7e2-91052e0fcbd9");
    this._filterToolbar2.Hidden = false;
    this._filterToolbar2.Items.AddRange(new ToolbarItemBase[5]
    {
      (ToolbarItemBase) this.cbFiltrationRule2,
      (ToolbarItemBase) this.btRuleVariant2,
      (ToolbarItemBase) this.btRuleBrowser2,
      (ToolbarItemBase) this.btRuleHint2,
      (ToolbarItemBase) this.useStoredExplicitPartVersionIDButtonItem2
    });
    this._filterToolbar2.Location = new Point(2, 0);
    this._filterToolbar2.MinimumFloatingSize = new Size(250, 30);
    this._filterToolbar2.Name = "_filterToolbar2";
    this._filterToolbar2.Size = new Size(489, 26);
    this._filterToolbar2.StretchItem = (ToolbarItemBase) this.cbFiltrationRule2;
    this._filterToolbar2.TabIndex = 27;
    this._filterToolbar2.Text = "Фильтрация состава";
    this.cbFiltrationRule2.CommandName = "cbFiltrationRule2";
    this.cbFiltrationRule2.DefaultText = "<Выберите правило подбора версий>";
    this.cbFiltrationRule2.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbFiltrationRule2.Locked = true;
    this.cbFiltrationRule2.MinimumControlWidth = 100;
    this.cbFiltrationRule2.MinimumSize = 356;
    this.cbFiltrationRule2.Padding.Bottom = 0;
    this.cbFiltrationRule2.Padding.Left = 1;
    this.cbFiltrationRule2.Padding.Right = 1;
    this.cbFiltrationRule2.Padding.Top = 0;
    this.cbFiltrationRule2.Stretch = true;
    this.cbFiltrationRule2.Text = "Правило:";
    this.cbFiltrationRule2.ToolTipText = "Правило:";
    this.btRuleVariant2.CommandName = "RuleVariant2";
    this.btRuleVariant2.ToolTipText = "Выбрать вариант значений для текущего правила подбора версий";
    this.btRuleBrowser2.CommandName = "RuleBrowser2";
    this.btRuleBrowser2.ToolTipText = "Выбрать правило подбора версий";
    this.btRuleHint2.BeginGroup = true;
    this.btRuleHint2.CommandName = "RuleHint2";
    this.btRuleHint2.Locked = true;
    this.btRuleHint2.ShowText = true;
    this.useStoredExplicitPartVersionIDButtonItem2.BeginGroup = true;
    this.useStoredExplicitPartVersionIDButtonItem2.CommandName = "useStoredExplicitPartVersionIDButtonItem2";
    this.useStoredExplicitPartVersionIDButtonItem2.ImageIndex = 2;
    this.useStoredExplicitPartVersionIDButtonItem2.ToolTipText = "Режим 'По информации о конкретизации'";
    this.toolBarEditingContexts2.AllowVerticalDock = false;
    this.toolBarEditingContexts2.DockLine = 2;
    this.toolBarEditingContexts2.FullMenus = true;
    this.toolBarEditingContexts2.Guid = new Guid("7e41d6d7-f8e4-4809-b69a-09b9706dffef");
    this.toolBarEditingContexts2.Hidden = false;
    this.toolBarEditingContexts2.Items.AddRange(new ToolbarItemBase[3]
    {
      (ToolbarItemBase) this.labelItem1,
      (ToolbarItemBase) this.contextsList2,
      (ToolbarItemBase) this.buttonEditingContextsBrowse2
    });
    this.toolBarEditingContexts2.Location = new Point(2, 26);
    this.toolBarEditingContexts2.Name = "toolBarEditingContexts2";
    this.toolBarEditingContexts2.Size = new Size(281, 24);
    this.toolBarEditingContexts2.TabIndex = 28;
    this.toolBarEditingContexts2.Text = "Текущий контекст редактирования";
    this.labelItem1.CommandName = "labelContext";
    this.labelItem1.Importance = ToolBarItemImportance.Highest;
    this.labelItem1.Text = "Контекст редактирования:";
    this.labelItem1.ToolTipText = "Текущий контекст редактирования";
    this.contextsList2.CommandName = "contextsList";
    this.contextsList2.Importance = ToolBarItemImportance.Highest;
    this.contextsList2.MinimumSize = 64 /*0x40*/;
    this.contextsList2.ShowText = true;
    this.buttonEditingContextsBrowse2.CommandName = "buttonEditingContextsBrowse";
    this.buttonEditingContextsBrowse2.ToolTipText = "Выбрать контекст редактирования и сделать его активным";
    this.splitContainer2.Dock = DockStyle.Fill;
    this.splitContainer2.Location = new Point(0, 26);
    this.splitContainer2.Name = "splitContainer2";
    this.splitContainer2.Orientation = Orientation.Horizontal;
    this.splitContainer2.Panel1.Controls.Add((Control) this.splitContainer1);
    this.splitContainer2.Panel2.Controls.Add((Control) this.splitContainer3);
    this.splitContainer2.Size = new Size(1068, 663);
    this.splitContainer2.SplitterDistance = 439;
    this.splitContainer2.TabIndex = 6;
    this.splitContainer3.Dock = DockStyle.Fill;
    this.splitContainer3.Location = new Point(0, 0);
    this.splitContainer3.Name = "splitContainer3";
    this.splitContainer3.Panel1.Controls.Add((Control) this.attributesControlLeft);
    this.splitContainer3.Panel2.Controls.Add((Control) this.attributesControlRigth);
    this.splitContainer3.Size = new Size(1068, 220);
    this.splitContainer3.SplitterDistance = 530;
    this.splitContainer3.TabIndex = 2;
    this.attributesControlLeft.Dock = DockStyle.Fill;
    this.attributesControlLeft.Location = new Point(0, 0);
    this.attributesControlLeft.Name = "attributesControlLeft";
    this.attributesControlLeft.Size = new Size(530, 220);
    this.attributesControlLeft.TabIndex = 0;
    this.attributesControlRigth.Dock = DockStyle.Fill;
    this.attributesControlRigth.Location = new Point(0, 0);
    this.attributesControlRigth.Name = "attributesControlRigth";
    this.attributesControlRigth.Size = new Size(534, 220);
    this.attributesControlRigth.TabIndex = 0;
    this.column1.Caption = (string) null;
    this.column1.Name = "column1";
    this.column2.Caption = (string) null;
    this.column2.Name = "column2";
    this.buttonItem5.CommandName = "buttonItem5";
    this.leftBarDock.Dock = DockStyle.Left;
    this.leftBarDock.Guid = new Guid("6283eb8a-627e-488c-bc89-55dea83ab3fc");
    this.leftBarDock.Location = new Point(0, 26);
    this.leftBarDock.Manager = this.barManager1;
    this.leftBarDock.Name = "leftBarDock";
    this.leftBarDock.Size = new Size(0, 685);
    this.leftBarDock.TabIndex = 7;
    this.leftBarDock.Text = "BarDock";
    this.rightBarDock.Dock = DockStyle.Right;
    this.rightBarDock.Guid = new Guid("195a95df-bc3f-4376-9eb7-2ef709bbefed");
    this.rightBarDock.Location = new Point(1068, 26);
    this.rightBarDock.Manager = this.barManager1;
    this.rightBarDock.Name = "rightBarDock";
    this.rightBarDock.Size = new Size(0, 685);
    this.rightBarDock.TabIndex = 8;
    this.rightBarDock.Text = "BarDock";
    this.bottomBarDock.Dock = DockStyle.Bottom;
    this.bottomBarDock.Guid = new Guid("b00db4d5-76de-4221-944e-19c6267a0d56");
    this.bottomBarDock.Location = new Point(0, 711);
    this.bottomBarDock.Manager = this.barManager1;
    this.bottomBarDock.Name = "bottomBarDock";
    this.bottomBarDock.Size = new Size(1068, 0);
    this.bottomBarDock.TabIndex = 9;
    this.bottomBarDock.Text = "BarDock";
    this.topBarDock.Controls.Add((Control) this.tbMain);
    this.topBarDock.Controls.Add((Control) this.tbAttributes);
    this.topBarDock.Dock = DockStyle.Top;
    this.topBarDock.Guid = new Guid("d6f04465-9e6c-4b05-a4d6-896a689963fe");
    this.topBarDock.Location = new Point(0, 0);
    this.topBarDock.Manager = this.barManager1;
    this.topBarDock.Name = "topBarDock";
    this.topBarDock.Size = new Size(1068, 26);
    this.topBarDock.TabIndex = 10;
    this.topBarDock.Text = "BarDock";
    this.tbMain.DockLine = 1;
    this.tbMain.FullMenus = true;
    this.tbMain.Guid = new Guid("467b0c6c-08c9-4165-81da-88000c4dc4a1");
    this.tbMain.Hidden = false;
    this.tbMain.Items.AddRange(new ToolbarItemBase[8]
    {
      (ToolbarItemBase) this.cbRules,
      (ToolbarItemBase) this.biEditRule,
      (ToolbarItemBase) this.biAddRule,
      (ToolbarItemBase) this.bRefresh,
      (ToolbarItemBase) this.bReport,
      (ToolbarItemBase) this.biAction,
      (ToolbarItemBase) this.bRecursive,
      (ToolbarItemBase) this.bExchange
    });
    this.tbMain.Location = new Point(2, 0);
    this.tbMain.Name = "tbMain";
    this.tbMain.Size = new Size(452, 26);
    this.tbMain.TabIndex = 1;
    this.tbMain.Text = "Основные настройки сравнения";
    this.cbRules.CommandName = "cbRules";
    this.cbRules.ToolTipText = "Список доступных правил сравнения";
    this.cbRules.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbRules.MinimumControlWidth = 250;
    this.cbRules.Padding.Bottom = 0;
    this.cbRules.Padding.Left = 1;
    this.cbRules.Padding.Right = 1;
    this.cbRules.Padding.Top = 0;
    this.cbRules.SelectedValueChanged += new EventHandler(this.cbRules_SelectedValueChanged);
    this.biEditRule.CommandName = "biEditRule";
    this.biEditRule.ToolTipText = "Редактировать правило";
    this.biEditRule.Click += new EventHandler(this.biEditRule_Click);
    this.bRefresh.CommandName = "bRefresh";
    this.bRefresh.ToolTipText = "Обновить список правил";
    this.bRefresh.Click += new EventHandler(this.Refresh_Click);
    this.bReport.BeginGroup = true;
    this.bReport.CommandName = "bReport";
    this.bReport.ToolTipText = "Генерировать отчет";
    this.bReport.Click += new EventHandler(this.bReport_Click);
    this.biAction.CommandName = "biAction";
    this.biAction.ToolTipText = "Выполнить сравнение";
    this.biAction.Click += new EventHandler(this.biAction_Click);
    this.bRecursive.BeginGroup = true;
    this.bRecursive.CommandName = "bRecursive";
    this.bRecursive.ToolTipText = "Развернутый состав";
    this.bRecursive.Click += new EventHandler(this.bRecursive_Click);
    this.bExchange.CommandName = "bExchange";
    this.bExchange.ToolTipText = "Поменять местами сравниваемые  объекты ";
    this.bExchange.Click += new EventHandler(this.bExchange_Click);
    this.tbAttributes.DockLine = 1;
    this.tbAttributes.DockOffset = 1;
    this.tbAttributes.FullMenus = true;
    this.tbAttributes.Guid = new Guid("54420f0c-4ebd-46ee-9c7d-cb54e9a7ae7d");
    this.tbAttributes.Hidden = false;
    this.tbAttributes.Items.AddRange(new ToolbarItemBase[4]
    {
      (ToolbarItemBase) this.biShowAttributes,
      (ToolbarItemBase) this.biAllAttributes,
      (ToolbarItemBase) this.biSortAZAndState,
      (ToolbarItemBase) this.biSortAZ
    });
    this.tbAttributes.Location = new Point(456, 0);
    this.tbAttributes.Name = "tbAttributes";
    this.tbAttributes.Size = new Size(130, 26);
    this.tbAttributes.TabIndex = 2;
    this.tbAttributes.Text = "Панели сравнения атрибутов";
    this.biShowAttributes.CommandName = "buttonItem1";
    this.biShowAttributes.ToolTipText = "Отобразить панели с атрибутами";
    this.biShowAttributes.Click += new EventHandler(this.biShowAttributes_Click);
    this.biAllAttributes.BeginGroup = true;
    this.biAllAttributes.CommandName = "biAllAttributes";
    this.biAllAttributes.ToolTipText = "Зачитывать все атрибуты объектов и связей";
    this.biAllAttributes.Click += new EventHandler(this.biAllAttributes_Click);
    this.biSortAZAndState.BeginGroup = true;
    this.biSortAZAndState.Checked = true;
    this.biSortAZAndState.CommandName = "buttonItem1";
    this.biSortAZAndState.Image = (Image) Resources.data_sort_icon1;
    this.biSortAZAndState.ToolTipText = "Сортировка атрибутов в алфавитном порядке, помеченные цветом атрибуты в начале списка";
    this.biSortAZAndState.Click += new EventHandler(this.biSort_Click);
    this.biSortAZ.CommandName = "buttonItem2";
    this.biSortAZ.Image = (Image) Resources.sort_columns_icon1;
    this.biSortAZ.ToolTipText = "Сортировка атрибутов в алфавитном порядке";
    this.biSortAZ.Click += new EventHandler(this.biSort_Click);
    this.biAddRule.CommandName = "biAddRule";
    this.biAddRule.Click += new EventHandler(this.BiAddRule_Click);
    this.biAddRule.ToolTipText = "Новое правило сравнения";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.splitContainer2);
    this.Controls.Add((Control) this.statusStrip1);
    this.Controls.Add((Control) this.leftBarDock);
    this.Controls.Add((Control) this.rightBarDock);
    this.Controls.Add((Control) this.bottomBarDock);
    this.Controls.Add((Control) this.topBarDock);
    this.Name = nameof (CompareTreeWindow);
    this.Size = new Size(1068, 711);
    this.statusStrip1.ResumeLayout(false);
    this.statusStrip1.PerformLayout();
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.Panel2.ResumeLayout(false);
    this.splitContainer1.EndInit();
    this.splitContainer1.ResumeLayout(false);
    this.panel1.ResumeLayout(false);
    this.virtualTreeView1.EndInit();
    this.toolBarContainer1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.virtualTreeView2.EndInit();
    this.toolBarContainer2.ResumeLayout(false);
    this.splitContainer2.Panel1.ResumeLayout(false);
    this.splitContainer2.Panel2.ResumeLayout(false);
    this.splitContainer2.EndInit();
    this.splitContainer2.ResumeLayout(false);
    this.splitContainer3.Panel1.ResumeLayout(false);
    this.splitContainer3.Panel2.ResumeLayout(false);
    this.splitContainer3.EndInit();
    this.splitContainer3.ResumeLayout(false);
    this.topBarDock.ResumeLayout(false);
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  private delegate void SetEnabledHandler(Control sender, bool enable);

  private delegate void ThrowExceptionHandler(Control sender, Exception ex);

  private delegate void SetButtonItemHandler(bool start);
}
