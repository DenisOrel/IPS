// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.ObjectOptionsEditor
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Infralution.Controls;
using Infralution.Controls.VirtualTree;
using Intermech.Bars;
using Intermech.Client.Core;
using Intermech.Client.Core.Thumbnail;
using Intermech.Controls;
using Intermech.DataFormats;
using Intermech.Docking;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.PdmConfigurator;
using Intermech.Localization;
using Intermech.NavBars;
using Intermech.Navigator;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.PdmConfigurator.Options;
using Intermech.PdmConfigurator.Options.ObjectOptions;
using Intermech.Win32;
using Microsoft.CSharp.RuntimeBinder;
using NJFLib.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.PdmConfigurator;

public sealed class ObjectOptionsEditor : UserControl
{
  private static StringFormat _imageStringFormat = new StringFormat();
  private ObjectVersionDescription _editingObject = new ObjectVersionDescription();
  private bool _isChanged;
  private bool _isInternalChanged;
  private RelationPair _key;
  private RelationPair _parentKey;
  private ObjectOptionsHolder _options = new ObjectOptionsHolder();
  private ObjectOptionsHolder _optionsSource = new ObjectOptionsHolder();
  private ObjectsApplicabilitiesCriterionsCollection _applCollection = new ObjectsApplicabilitiesCriterionsCollection();
  private ObjectsApplicabilitiesCriterionsCollection _applCollectionSource = new ObjectsApplicabilitiesCriterionsCollection();
  private PdmConfiguratorContext _context = new PdmConfiguratorContext((PdmConfiguratorContextsCache) null);
  private PdmConfiguratorContext _contextSource = new PdmConfiguratorContext((PdmConfiguratorContextsCache) null);
  private INamedImageList _images;
  private ICategoryTypeIconService _objtypesIcons;
  private INavGraphicsCache _navGraphicsCache;
  private ICurrentUserAndRole _userRole;
  private IUserNamesCache _userNamesCache;
  private IPicturesCache _cache;
  private IServiceProvider _services;
  private INotificationService _notifications;
  private IFiltrationService _filtrationService;
  private OptionAccessRights _accessRights;
  private PdmContextAccessRights _applsAccessRights;
  private PdmContextAccessRights _contextAccessRights;
  private bool _readOnly;
  internal bool _ignoreNullRelation;
  internal string _pdmCriterion = "";
  internal string _pdmContext = "";
  private bool _inEvents;
  private bool _developerMode;
  private List<OptionObjectDescription> _categories;
  private Dictionary<long, List<OptionHolder>> _optionsHolders = new Dictionary<long, List<OptionHolder>>();
  private static Font _treeBoldFont;
  private static Font _treeBoldUnderFont;
  private static Font _treeFontStriked;
  private NotificationEventHandler _notifyHandler;
  private Row _dropTargetRow;
  private static bool _thumbnailMode = false;
  private static bool _collapsed = true;
  private static bool _hideInvisibles = true;
  private static Dictionary<bool, Dictionary<string, int>> _colWidths = new Dictionary<bool, Dictionary<string, int>>();
  private iGCellStyle cellStyle;
  private iGCellStyle cellStyleBold;
  private iGCellStyle cellCheckBoxEdit;
  private iGCellStyle cellCheckBox;
  private iGCellStyle cellInt64;
  private iGCellStyle cellDouble;
  private iGCellStyle cellDateTime;
  private iGCellStyle cellString;
  private iGCellStyle cellImage;
  private iGCellStyle cellStyleStatus;
  private iGColHdrStyle headerStyle;
  private iGRow prevRow;
  private EventHandler handlerDoDefaultView;
  private EventHandler handlerDoThumbnailsView;
  private IContainer components;
  private HeaderControl headerControl;
  private ImageList imagesTabs;
  private ToolTip toolTip;
  private Column columnMain;
  private MenuBar menuBarTree;
  private ContextMenuBarItem contextMenuBarTree;
  private MenuButtonItem mnpAddOptions;
  private MenuButtonItem mnpDeleteOptions;
  private MenuButtonItem mnpCollapse;
  private MenuButtonItem mnpExpand;
  private MenuButtonItem mnpCard;
  private MenuButtonItem mnpOpenInNewWindow;
  private Panel panelTree;
  private Intermech.Bars.ToolBar toolBarTree;
  private ButtonItem btnAddOptions;
  private ButtonItem btnDeleteOptions;
  private ButtonItem btnCard;
  private ButtonItem btnOpenInNewWindow;
  private ButtonItem btnExpand;
  private ButtonItem btnCollapse;
  private Intermech.VirtualTreeView.VirtualTreeView treeOptions;
  private DropDownMenuItem btnImport;
  private MenuButtonItem btnDefaultImport;
  private MenuButtonItem btnInCompositions;
  private MenuButtonItem btnRecursiveImport;
  private CollapsibleSplitter splitter;
  private iGrid gridValues;
  private Panel panelGrid;
  private ImageList imagesToolbars;
  private MenuButtonItem mnpDefaultImport;
  private MenuButtonItem mnpInCompositions;
  private MenuButtonItem mnpRecursiveImport;
  private MenuButtonItem mnpObligatoryOption;
  protected PageControl tabs;
  private Intermech.Docking.TabPage tabPageIncomps;
  private IncompatibilityEditor incompEditor;
  private Intermech.Docking.TabPage tabPageLinked;
  private LinkedOptionsEditor linkedEditor;
  private Intermech.Docking.TabPage tabPagePicture;
  private PictureBox picture;
  private MenuBar menuBarGridValues;
  private ContextMenuBarItem contextMenuBarGrid;
  private MenuButtonItem mnpDefaultValue;
  private MenuButtonItem mnpGridExpand;
  private MenuButtonItem mnpGridCollapse;
  protected PageControl pagesMain;
  private Intermech.Docking.TabPage pageObjectOptions;
  private CollapsibleSplitter splitterV;
  private Intermech.Bars.ToolBar toolBarGrid;
  private ButtonItem btAdvPanels;
  private DropDownMenuItem btnMode;
  private MenuButtonItem btnDefault;
  private MenuButtonItem btnThumbnails;
  private ButtonItem btnHideInvisibles;
  private ButtonItem btnGridExpand;
  private ButtonItem btnGridCollapse;
  private ButtonItem cbObligatoryOption;
  private LabelItem lbWarning;
  private Intermech.Docking.TabPage pageContext;
  internal ObjectContextEditor contextEditor;
  private Label lbPageContext;
  private Intermech.Docking.TabPage pageAppls;
  internal AppConditionsEditor appEditor;
  private Label labelAppls;
  private Panel panelHint;
  private Label labelWarning;
  private PictureBox pictureHint;
  private Intermech.Docking.TabPage pageCode;
  private ConfigurationCodeEditor codeEditor;
  private MenuButtonItem btnInObjectComposition;
  private MenuButtonItem btnObjectRecursiveImport;
  private SplitContainer _splitContainer;
  private ButtonItem btnExcelReport;

  public ObjectOptionsEditor()
  {
    this.InitializeComponent();
    if (ServicesManager.GetService(typeof (BarManager)) is BarManager service)
    {
      service.RendererChanged += new EventHandler(this.BarManager_RendererChanged);
      this.BarManager_RendererChanged((object) service, EventArgs.Empty);
    }
    if (ServicesManager.GetService(typeof (IGuidMapper)) is IGuidMapper)
      this.Init();
    HelpProvidersClass.SetHelpOptionForControl((Control) this, 1837);
  }

  public event ObjectOptionsEditor.ObjectOptionsChangedEventHandler OnChanged;

  private void RaiseOnChanged()
  {
    if (this.OnChanged == null)
      return;
    this.OnChanged((object) this, new EventArgs());
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public RelationPair Key
  {
    [DebuggerStepThrough] get => this._key;
    set => this._key = value;
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public RelationPair ParentKey
  {
    [DebuggerStepThrough] get => this._parentKey;
    set => this._parentKey = value;
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public bool ReadOnly
  {
    [DebuggerStepThrough] get => this._readOnly;
    set
    {
      this._readOnly = value;
      this.UpdateControls();
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public OptionAccessRights AccessRights
  {
    [DebuggerStepThrough] get => this._accessRights;
    set
    {
      this._accessRights = value;
      this.UpdateControls();
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public IServiceProvider Services
  {
    [DebuggerStepThrough] get => this._services;
    set => this._services = value;
  }

  [Category("Appearance")]
  [Browsable(true)]
  public bool IsChanged
  {
    [DebuggerStepThrough] get => this._isChanged | this._isInternalChanged;
    set
    {
      this._isChanged = value;
      this.RaiseOnChanged();
      this.UpdateControls();
    }
  }

  [Category("Appearance")]
  [Browsable(true)]
  public bool IsInternalChanged
  {
    [DebuggerStepThrough] get => this._isInternalChanged;
    set
    {
      this._isInternalChanged = value;
      this.RaiseOnChanged();
      this.UpdateControls();
    }
  }

  [Category("Appearance")]
  [Browsable(true)]
  public bool DisableHeader
  {
    [DebuggerStepThrough] get => !this.headerControl.Visible;
    set => this.headerControl.Visible = !value;
  }

  public void LoadInfo(IServiceProvider services, RelationPair key, RelationPair parentKey)
  {
    object treeFocusItem = this.GetTreeFocusItem();
    object[] treeSelectedItems = this.GetTreeSelectedItems();
    object[] gridSelectedItems = this.GetGridSelectedItems();
    IncompatibilityEditor.PathPart[] focusPath = this.incompEditor.FocusPath;
    IncompatibilityEditor.PathPart selectedItem = this.linkedEditor.SelectedItem;
    try
    {
      this.Clear();
      if (key == null || key.F_PROJ_ID == 0L || key.Empty)
        return;
      this._services = services;
      this._key = key;
      this._parentKey = parentKey;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        try
        {
          this._context.Services.AddService(typeof (IUserSession), (object) sessionKeeper.Session);
          IDBObject dbObject1 = sessionKeeper.Session.GetObject(this.Key.F_PROJ_ID, false);
          if (dbObject1 == null)
            return;
          this._editingObject.Assign((object) dbObject1);
          this.CheckAccessRights(dbObject1);
          this._options.LoadFromObject((IDBAttributable) dbObject1);
          if (!this.Key.Empty && this.Key.F_PRJLINK_ID != 0L)
          {
            IDBRelation relation = sessionKeeper.Session.GetRelation(this.Key.F_PRJLINK_ID, false);
            if (relation == null)
              return;
            this._applsAccessRights = this.appEditor.CheckAccessRights((IDBAttributable) relation);
            this._contextAccessRights = this.contextEditor.CheckAccessRights((IDBAttributable) relation);
            if (this.ParentKey != null && this.ParentKey.F_PROJ_ID != 0L && !MetaDataHelper.IsObjectTypeChildOf(this.ParentKey.F_OBJECT_TYPE, Intermech.Interfaces.PdmConfigurator.Consts.objtypeComplementsID))
              this._applCollection.LoadFromObject((IDBAttributable) relation);
            if (this.ParentKey != null && !this.ParentKey.Empty)
            {
              IDBObject dbObject2 = sessionKeeper.Session.GetObject(this.ParentKey.F_PROJ_ID, false);
              ObjectOptionsHolder objectOptionsHolder = new ObjectOptionsHolder();
              objectOptionsHolder.LoadFromObject((IDBAttributable) dbObject2);
              this._applCollection.Holder = (object) objectOptionsHolder;
            }
            try
            {
              this._context.Services.AddService(typeof (object), (object) relation);
              this._context.Key = this.Key;
              this._context.ParentKey = this.ParentKey;
              this._context.LoadFromObject((IDBAttributable) relation);
              this._context.ObjectsOptions.Clear();
              this._context.ObjectsOptions.Add(this._options);
              this._context.SyncOptionsList(true);
            }
            finally
            {
              this._context.Services.RemoveService(typeof (object));
            }
          }
          else
          {
            if (!this._ignoreNullRelation)
            {
              this._applsAccessRights = this._contextAccessRights = PdmContextAccessRights.ReadOnly;
              this._contextAccessRights = this.contextEditor.CheckAccessRights((IDBAttributable) dbObject1);
              this._applCollection.Clear();
              this._context.Clear();
            }
            else
            {
              this._applsAccessRights = this._contextAccessRights = PdmContextAccessRights.FullAccess;
              this._applCollection.Assign((object) this._pdmCriterion);
              this._context.ObjectsOptions.Clear();
              this._context.ObjectsOptions.Add(this._options);
              this._context.Assign((object) this._pdmContext);
              this._context.SyncOptionsList(true);
            }
            if (this.Key != null)
            {
              try
              {
                this._context.Services.AddService(typeof (object), (object) dbObject1);
                this._context.Key = this.Key;
                this._context.ParentKey = this.ParentKey;
                this._context.LoadFromObject((IDBAttributable) dbObject1);
                this._context.ObjectsOptions.Clear();
                this._context.ObjectsOptions.Add(this._options);
                if (this._ignoreNullRelation && this._pdmContext != "")
                  this._context.Assign((object) this._pdmContext);
                this._context.SyncOptionsList(true);
                this._context.ContextType = PdmContextType.ContextObject;
              }
              finally
              {
                this._context.Services.RemoveService(typeof (object));
              }
            }
          }
        }
        finally
        {
          this._context.Services.RemoveService(typeof (IUserSession));
        }
      }
      this._context.Key = this.Key;
      this._context.ParentKey = this.ParentKey;
      this.FillEditor(false);
      this.Fix();
    }
    finally
    {
      this.SetTreeFocusItem(treeFocusItem);
      this.SetTreeSelectedItems(treeSelectedItems);
      this.SetGridSelectedItems(gridSelectedItems);
      this.incompEditor.FocusPath = focusPath;
      this.linkedEditor.SelectedItem = selectedItem;
    }
  }

  public void Save()
  {
    if (!this.IsChanged)
      return;
    long objectID = 0;
    long relationID = 0;
    try
    {
      this.Apply();
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        if (this.AccessRights == OptionAccessRights.FullAccess && this.IsInternalChanged)
        {
          IDBObject serviceInstance = sessionKeeper.Session.GetObject(this.Key.F_PROJ_ID, false);
          if (serviceInstance != null)
          {
            this._options.SaveToObject((IDBAttributable) serviceInstance);
            objectID = this.Key.F_PROJ_ID;
          }
          if (this.Key != null && this.Key.F_PRJLINK_ID == 0L && this._contextAccessRights == PdmContextAccessRights.FullAccess)
          {
            if (this.contextEditor.IsChanged)
            {
              try
              {
                this._context.Services.AddService(typeof (IUserSession), (object) sessionKeeper.Session);
                this._context.Services.AddService(typeof (object), (object) serviceInstance);
                this._context.SaveToObject((IDBAttributable) serviceInstance);
              }
              finally
              {
                this._context.Services.RemoveService(typeof (object));
                this._context.Services.RemoveService(typeof (IUserSession));
              }
              relationID = this.Key.F_PRJLINK_ID;
            }
          }
        }
        if (this.Key.F_PRJLINK_ID != 0L && (this._applsAccessRights == PdmContextAccessRights.FullAccess || this._contextAccessRights == PdmContextAccessRights.FullAccess && this.contextEditor.IsChanged))
        {
          IDBRelation relation = sessionKeeper.Session.GetRelation(this.Key.F_PRJLINK_ID, false);
          if (this.ParentKey != null && this.ParentKey.F_PROJ_ID != 0L && !MetaDataHelper.IsObjectTypeChildOf(this.ParentKey.F_OBJECT_TYPE, Intermech.Interfaces.PdmConfigurator.Consts.objtypeComplementsID) && this._applsAccessRights == PdmContextAccessRights.FullAccess && this.appEditor.IsChanged)
          {
            this.appEditor.Save();
            this._applCollection.Assign((object) this.appEditor.PdmCriterionCollection);
            this._applCollection.SaveToObject((IDBAttributable) relation);
            relationID = this.Key.F_PRJLINK_ID;
          }
          if (this._contextAccessRights == PdmContextAccessRights.FullAccess)
          {
            if (this.contextEditor.IsChanged)
            {
              try
              {
                this._context.Services.AddService(typeof (IUserSession), (object) sessionKeeper.Session);
                this._context.Services.AddService(typeof (object), (object) relation);
                this._context.SaveToObject((IDBAttributable) relation);
              }
              finally
              {
                this._context.Services.RemoveService(typeof (object));
                this._context.Services.RemoveService(typeof (IUserSession));
              }
              relationID = this.Key.F_PRJLINK_ID;
            }
          }
        }
        else if (this._ignoreNullRelation)
        {
          if (this.appEditor.IsChanged)
          {
            this.appEditor.Save();
            this._applCollection.Assign((object) this.appEditor.PdmCriterionCollection);
          }
        }
      }
      this.Fix();
    }
    catch (Exception ex)
    {
      if (ex is PdmConfiguratorExeption)
      {
        int num = (int) IMMessageBox.Show(LocalizationHolder.rm.GetString("PdmConfigurator_7"), ex.Message, MessageBoxButtons.OK, IMMessageBoxImage.Information);
        return;
      }
      if (ex.InnerException is PdmConfiguratorExeption)
      {
        int num = (int) IMMessageBox.Show(LocalizationHolder.rm.GetString("PdmConfigurator_7"), ex.InnerException.Message, MessageBoxButtons.OK, IMMessageBoxImage.Information);
        return;
      }
      throw;
    }
    finally
    {
      PdmConfiguratorObjectOptionsCache.ResetExpired();
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        if (sessionKeeper.Session.GetCustomService(typeof (IPdmConfiguratorService)) is IPdmConfiguratorService customService)
          customService.ResetSessionCache((object) sessionKeeper.Session.SessionGUID);
      }
    }
    INotificationService service = ServicesManager.GetService(typeof (INotificationService)) as INotificationService;
    if (relationID != 0L)
      service.FireEvent((object) this, (NotificationEventArgs) new DBRelationsEventArgs("RelationsChanged", relationID));
    if (objectID != 0L)
      service.FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", objectID));
    bool result = true;
    if (this._filtrationService.Filtration.Tags[(object) "{0422E069-0A1D-4235-85E8-C52C3516CFC1}"] != null)
      bool.TryParse(this._filtrationService.Filtration.Tags[(object) "{0422E069-0A1D-4235-85E8-C52C3516CFC1}"].ToString(), out result);
    if (relationID == 0L || result)
      return;
    this._filtrationService.FiltrationApplyUpdates(true);
  }

  public void Clear()
  {
    this._editingObject.Clear();
    this._options.Clear();
    this._optionsHolders.Clear();
    this._applCollection.Clear();
    this._context.Clear();
    this.FillEditor(false);
  }

  public ObjectOptionsEditor.State GetState()
  {
    return new ObjectOptionsEditor.State()
    {
      TreeFocusItem = this.GetTreeFocusItem(),
      TreeSelectedItems = this.GetTreeSelectedItems(),
      GridSelectedItems = this.GetGridSelectedItems(),
      IncompEditorFocusPath = this.incompEditor.GetLiveFocusPath(),
      LinkedEditorSelectedItem = this.linkedEditor.GetLiveSelectedItem()
    };
  }

  public void SetState(ObjectOptionsEditor.State state)
  {
    this.SetTreeFocusItem(state.TreeFocusItem);
    this.SetTreeSelectedItems(state.TreeSelectedItems);
    this.SetGridSelectedItems(state.GridSelectedItems);
    this.incompEditor.FocusPath = state.IncompEditorFocusPath;
    this.linkedEditor.SelectedItem = state.LinkedEditorSelectedItem;
  }

  public void Undo()
  {
    object treeFocusItem = this.GetTreeFocusItem();
    object[] treeSelectedItems = this.GetTreeSelectedItems();
    object[] gridSelectedItems = this.GetGridSelectedItems();
    IncompatibilityEditor.PathPart[] focusPath = this.incompEditor.FocusPath;
    IncompatibilityEditor.PathPart selectedItem = this.linkedEditor.SelectedItem;
    try
    {
      this.linkedEditor.Undo();
      this._options.Assign((object) this._optionsSource);
      this._applCollection.Assign((object) this._applCollectionSource);
      this._context.Assign((object) this._contextSource);
      this.FillEditor(false);
      this.appEditor.IsChanged = false;
      this.contextEditor.IsChanged = false;
      this._isInternalChanged = false;
      this.IsChanged = false;
    }
    finally
    {
      this.SetTreeFocusItem(treeFocusItem);
      this.SetTreeSelectedItems(treeSelectedItems);
      this.SetGridSelectedItems(gridSelectedItems);
      this.incompEditor.FocusPath = focusPath;
      this.linkedEditor.SelectedItem = selectedItem;
    }
  }

  private object[] GetTreeSelectedItems()
  {
    return this.treeOptions.SelectedItems.Cast<object>().ToArray<object>();
  }

  private void SetTreeSelectedItems(object[] items)
  {
    this.treeOptions.SelectedItem = (object) null;
    this.SelectTreeItems(this.treeOptions.RootRow, items);
  }

  private void SelectTreeItems(Row row, object[] items)
  {
    if (((IEnumerable<object>) items).Contains<object>(row.Item))
      row.Selected = true;
    for (int childIndex = 0; childIndex < row.NumChildren; ++childIndex)
      this.SelectTreeItems(row.ChildRowByIndex(childIndex), items);
  }

  private object GetTreeFocusItem()
  {
    return this.treeOptions.FocusRow == null ? (object) null : this.treeOptions.FocusRow.Item;
  }

  private void SetTreeFocusItem(object item)
  {
    this.SetTreeFocusItem(this.treeOptions.RootRow, item);
  }

  private void SetTreeFocusItem(Row row, object item)
  {
    if (item == row.Item)
    {
      this.treeOptions.FocusRow = row;
    }
    else
    {
      for (int childIndex = 0; childIndex < row.NumChildren; ++childIndex)
        this.SetTreeFocusItem(row.ChildRowByIndex(childIndex), item);
    }
  }

  private object[] GetGridSelectedItems()
  {
    List<object> objectList = new List<object>();
    foreach (iGCell selectedCell in this.gridValues.SelectedCells)
    {
      object obj = selectedCell.Row.Cells["TAG"] != null ? selectedCell.Row.Cells["TAG"].Value : (object) null;
      if (obj != null && !objectList.Contains(obj))
        objectList.Add(obj);
    }
    return objectList.ToArray();
  }

  private void SetGridSelectedItems(object[] items)
  {
    OptionValue[] array = items.Cast<OptionValue>().Where<OptionValue>((Func<OptionValue, bool>) (o => o != null)).ToArray<OptionValue>();
    foreach (iGRow row in (IEnumerable) this.gridValues.Rows)
    {
      OptionValue optionValue = (row.Cells["TAG"] != null ? row.Cells["TAG"].Value : (object) null) as OptionValue;
      if (optionValue != null && ((IEnumerable<OptionValue>) array).Any<OptionValue>((Func<OptionValue, bool>) (o => o.ID == optionValue.ID && o.Value == optionValue.Value)))
      {
        foreach (iGCell iGcell in row.Cells.Cast<iGCell>().Reverse<iGCell>())
          iGcell.Selected = true;
      }
    }
  }

  private void BarManager_RendererChanged(object sender, EventArgs e)
  {
    IToolBarRenderer renderer = (sender as BarManager).Renderer;
    this.toolBarTree.Renderer = renderer;
    this.toolBarGrid.Renderer = renderer;
    this.menuBarTree.Renderer = renderer;
    this.menuBarGridValues.Renderer = renderer;
  }

  private void treeOptions_GetChildren(object sender, GetChildrenEventArgs e)
  {
    if (e.Row.Level == 0)
    {
      e.Children = (IList) this._categories;
    }
    else
    {
      if (e.Row.Level != 1)
        return;
      OptionObjectDescription objectDescription = (OptionObjectDescription) e.Row.Item;
      if (!this._optionsHolders.ContainsKey(objectDescription.F_OBJECT_ID))
        return;
      e.Children = (IList) this._optionsHolders[objectDescription.F_OBJECT_ID];
    }
  }

  private void treeOptions_GetRowData(object sender, GetRowDataEventArgs e)
  {
    if (e.Row.Level <= 1)
    {
      e.RowData.IconSize = 32 /*0x20*/;
      e.RowData.ImageList = this._objtypesIcons.ImageList;
      e.RowData.ImageIndex = this._objtypesIcons.IndexOf(4, Intermech.Interfaces.PdmConfigurator.Consts.objtypeOptionsGroupID);
    }
    if (e.Row.Level != 2)
      return;
    e.RowData.IconSize = 32 /*0x20*/;
    e.RowData.ImageList = this._objtypesIcons.ImageList;
    e.RowData.ImageIndex = this._objtypesIcons.IndexOf(4, Intermech.Interfaces.PdmConfigurator.Consts.objtypeOptionID);
  }

  private void treeOptions_GetCellData(object sender, GetCellDataEventArgs e)
  {
    if (e.Row.Level == 0 && e.Column == this.columnMain)
    {
      e.CellData.Value = (object) LocalizationHolder.rm.GetString("PdmConfigurator_51");
      e.CellData.OddStyle = new Style(e.Row.Tree.RowOddStyle, new StyleDelta()
      {
        Font = ObjectOptionsEditor._treeBoldFont
      });
      e.CellData.EvenStyle = new Style(e.Row.Tree.RowEvenStyle, new StyleDelta()
      {
        Font = ObjectOptionsEditor._treeBoldFont
      });
    }
    if (e.Row.Level == 1 && e.Column == this.columnMain)
    {
      ObjectVersionDescription versionDescription = (ObjectVersionDescription) e.Row.Item;
      e.CellData.Value = (object) versionDescription.CAPTION;
      if (versionDescription.F_OBJECT_ID == Intermech.Interfaces.PdmConfigurator.Consts.objectNoCategoryID)
      {
        e.CellData.OddStyle = new Style(e.Row.Tree.RowOddStyle, new StyleDelta()
        {
          Font = ObjectOptionsEditor._treeBoldFont
        });
        e.CellData.EvenStyle = new Style(e.Row.Tree.RowEvenStyle, new StyleDelta()
        {
          Font = ObjectOptionsEditor._treeBoldFont
        });
      }
    }
    if (e.Row.Level != 2)
      return;
    OptionHolder optionHolder = (OptionHolder) e.Row.Item;
    if (e.Column != this.columnMain)
      return;
    e.CellData.Value = (object) optionHolder.OptionCaption;
    if ((optionHolder.OptionFlags & OptionFlags.Obsolete) == OptionFlags.Obsolete && !this._options.VisibleOptionValues.GetObligatoryOption(optionHolder.OptionGuid))
    {
      e.CellData.OddStyle = new Style(e.Row.Tree.RowOddStyle, new StyleDelta()
      {
        ForeColor = Color.Red,
        Font = ObjectOptionsEditor._treeFontStriked
      });
      e.CellData.EvenStyle = new Style(e.Row.Tree.RowEvenStyle, new StyleDelta()
      {
        ForeColor = Color.Red,
        Font = ObjectOptionsEditor._treeFontStriked
      });
    }
    if (!this._options.VisibleOptionValues.GetObligatoryOption(optionHolder.OptionGuid))
      return;
    e.CellData.Value = (object) ("* " + optionHolder.OptionCaption);
    StyleDelta delta1 = new StyleDelta();
    delta1.ForeColor = Color.Blue;
    if ((optionHolder.OptionFlags & OptionFlags.Obsolete) == OptionFlags.Obsolete)
    {
      delta1.ForeColor = Color.Red;
      delta1.Font = ObjectOptionsEditor._treeFontStriked;
    }
    e.CellData.OddStyle = new Style(e.Row.Tree.RowOddStyle, delta1);
    StyleDelta delta2 = new StyleDelta();
    delta2.ForeColor = Color.Blue;
    if ((optionHolder.OptionFlags & OptionFlags.Obsolete) == OptionFlags.Obsolete)
    {
      delta2.ForeColor = Color.Red;
      delta2.Font = ObjectOptionsEditor._treeFontStriked;
    }
    e.CellData.EvenStyle = new Style(e.Row.Tree.RowEvenStyle, delta2);
  }

  private void treeOptions_SelectionChanged(object sender, EventArgs e)
  {
    if (this._inEvents)
      return;
    this.FillGrid();
    this.UpdateControls();
  }

  private void treeOptions_ShowContextMenu(object sender, MouseEventArgs e)
  {
    this.contextMenuBarTree.Show((Control) this.treeOptions, e.Location);
  }

  private void treeOptions_DragEnter(object sender, DragEventArgs e)
  {
    this._dropTargetRow = (Row) null;
    e.Effect = DragDropEffects.None;
    if (!this.treeOptions.AllowDrop)
      return;
    int num = this._options.ObjectID != 0L ? 1 : 0;
    bool flag = !this._readOnly && (this._accessRights & OptionAccessRights.FullAccess) != 0;
    if (num == 0 || !flag || !e.Data.GetDataPresent(typeof (IIOSource)))
      return;
    e.Effect = DragDropEffects.All;
  }

  private void treeOptions_DragOver(object sender, DragEventArgs e)
  {
    e.Effect = DragDropEffects.None;
    if (!this.treeOptions.AllowDrop || this._options.ObjectID == 0L || this._accessRights != OptionAccessRights.FullAccess || !e.Data.GetDataPresent(typeof (IOSource)))
      return;
    int num = this._options.ObjectID != 0L ? 1 : 0;
    bool flag = !this._readOnly && (this._accessRights & OptionAccessRights.FullAccess) != 0;
    if (num == 0 || !flag)
      return;
    e.Effect = DragDropEffects.All;
  }

  private void treeOptions_DragDrop(object sender, DragEventArgs e)
  {
    if (!this.treeOptions.AllowDrop || this._options.ObjectID == 0L || this._accessRights != OptionAccessRights.FullAccess || !e.Data.GetDataPresent(typeof (IOSource)) || !(e.Data.GetData(typeof (IOSource)) is IOSource data) || data.SelectedItems == null || data.SelectedItems.Count == 0)
      return;
    List<long> options = new List<long>();
    for (int index = 0; index < data.SelectedItems.Count; ++index)
    {
      if (data.SelectedItems.GetItemData(index, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData && MetaDataHelper.IsObjectTypeChildOf(itemData.ObjectType, Intermech.Interfaces.PdmConfigurator.Consts.objtypeOptionID) && this._options.Options.IndexOf(itemData.ObjectID) < 0)
        options.Add(itemData.ObjectID);
    }
    this.DoAddOptions((IList<long>) options);
  }

  private void treeOptions_GetAllowedRowDropLocations(
    object sender,
    GetAllowedRowDropLocationsEventArgs e)
  {
    this._dropTargetRow = e.Row;
    e.AllowedDropLocations = this._dropTargetRow != null ? RowDropLocation.OnRow : RowDropLocation.BelowRow;
  }

  private void treeOptions_GetRowDropEffect(object sender, GetRowDropEffectEventArgs e)
  {
    this._dropTargetRow = (Row) null;
    if (!this.treeOptions.AllowDrop || this._accessRights != OptionAccessRights.FullAccess || !e.Data.GetDataPresent(typeof (IOSource)))
      return;
    this._dropTargetRow = e.Row;
    e.DropEffect = DragDropEffects.All;
  }

  private void DoAddOptions(object sender, EventArgs e)
  {
    int num = this._options.ObjectID != 0L ? 1 : 0;
    bool flag = !this._readOnly && (this._accessRights & OptionAccessRights.FullAccess) != 0;
    if (num == 0 || !flag)
      return;
    DescriptorCollection descriptors = new DescriptorCollection();
    descriptors.Add((IDescriptor) new Intermech.Navigator.DBObjectTypes.Descriptor(Intermech.Interfaces.PdmConfigurator.Consts.objtypeOptionID));
    descriptors.Add((IDescriptor) new Intermech.Navigator.DBObjectTypes.Descriptor(MetaDataHelper.GetObjectTypeID(new Guid("cad015af-306c-11d8-b4e9-00304f19f545"))));
    Intermech.Navigator.SelectionWindow.RegisterAnalyze((ISelectedItemsAnalyzer) new TypedObjectsSelectedItemsAnalyzer(Intermech.Interfaces.PdmConfigurator.Consts.objtypeOptionID, true), true);
    object[] objArray = Intermech.Navigator.SelectionWindow.Select(LocalizationHolder.rm.GetString("PdmConfigurator_53"), LocalizationHolder.rm.GetString("PdmConfigurator_54"), (IDescriptor) new Intermech.Navigator.CustomNode.Descriptor(LocalizationHolder.rm.GetString("PdmConfigurator_55"), descriptors), typeof (IDBTypedObjectID), SelectionOptions.Default | SelectionOptions.DisableSelectAbstractTypes);
    if (objArray == null || objArray.Length == 0)
      return;
    List<long> options = new List<long>();
    for (int index = 0; index < objArray.Length; ++index)
    {
      IDBTypedObjectID dbTypedObjectId = (IDBTypedObjectID) objArray[index];
      if (MetaDataHelper.IsObjectTypeChildOf(dbTypedObjectId.ObjectType, Intermech.Interfaces.PdmConfigurator.Consts.objtypeOptionID) && this._options.Options.IndexOf(dbTypedObjectId.ObjectID) < 0)
        options.Add(dbTypedObjectId.ObjectID);
    }
    this.DoAddOptions((IList<long>) options);
    this.FillGrid();
  }

  private void DoDeleteOptions(object sender, EventArgs e)
  {
    int num = this._options.ObjectID != 0L ? 1 : 0;
    bool flag1 = !this._readOnly && (this._accessRights & OptionAccessRights.FullAccess) != 0;
    bool flag2 = this.treeOptions.SelectedRows.Count > 0 && this.treeOptions.RootRow.NumChildren > 0;
    if (num == 0 || !flag1 || !flag2)
      return;
    List<OptionHolder> selectedOptions = this.GetSelectedOptions();
    if (selectedOptions.Count == 0)
      return;
    bool flag3 = false;
    using (new SessionKeeper())
    {
      for (int index = 0; index < selectedOptions.Count; ++index)
      {
        this._options.DeleteOption(selectedOptions[index].OptionObjectID);
        flag3 = true;
      }
    }
    if (flag3)
    {
      this.CollectCategories();
      this.FillTree(false);
      this.IsInternalChanged = true;
    }
    this.FillGrid();
  }

  private void DoCard(object sender, EventArgs e)
  {
    long selectedObject = this.GetSelectedObject();
    if (selectedObject == 0L)
      return;
    int num = (int) PropertiesWindow.Execute(string.Empty, string.Empty, selectedObject, false);
  }

  private void DoOpen(object sender, EventArgs e)
  {
    long selectedObject = this.GetSelectedObject();
    if (selectedObject == 0L)
      return;
    Intermech.Navigator.Utils.OpenNewWindow((IDescriptor) new Intermech.Navigator.DBObjects.Descriptor(selectedObject), this.Services);
  }

  private void DoExpand(object sender, EventArgs e)
  {
    this.treeOptions.RootRow.ExpandChildren(true);
    this.UpdateControls();
  }

  private void DoCollapse(object sender, EventArgs e)
  {
    this.treeOptions.RootRow.CollapseChildren(true);
    this.UpdateControls();
  }

  private void NotificationEventFired(object sender, NotificationEventArgs e)
  {
    if (!(e.EventName == "ObjectsChanged") || !(e is DBObjectsEventArgs objectsEventArgs) || objectsEventArgs.ObjectIDs == null || this._options.Options.Count <= 0)
      return;
    List<long> options = new List<long>();
    bool flag = false;
    for (int index = 0; index < this._options.Options.Count; ++index)
    {
      if (objectsEventArgs.ObjectIDs.IndexOf(this._options.Options[index]) >= 0 || objectsEventArgs.ObjectIDs.IndexOf(-this._options.Options[index]) >= 0)
        options.Add(this._options.Options[index]);
    }
    List<OptionObjectDescription> categoriesList = PdmConfiguratorCache.CacheGetCategoriesList();
    for (int index = 0; index < categoriesList.Count; ++index)
    {
      if (objectsEventArgs.ObjectIDs.IndexOf(categoriesList[index].F_OBJECT_ID) >= 0 || objectsEventArgs.ObjectIDs.IndexOf(-categoriesList[index].F_OBJECT_ID) >= 0)
        flag = true;
      if (flag)
        break;
    }
    if (!(options.Count > 0 | flag))
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (options.Count > 0)
        PdmConfiguratorCache.CacheLoadOptions(sessionKeeper.Session, (IList<long>) options);
      if (flag)
        PdmConfiguratorCache.CacheLoadCategories(sessionKeeper.Session);
      this._options.ClearVisibleOptionsValuesLists(sessionKeeper.Session);
      this._options.FillVisibleOptionsValuesLists();
    }
    this.CollectCategories();
    this.FillTree(true);
  }

  private void DoAdvPanelsShow(object sender, EventArgs e)
  {
    ObjectOptionsEditor._collapsed = !ObjectOptionsEditor._collapsed;
    this._splitContainer.Panel2Collapsed = ObjectOptionsEditor._collapsed;
    this.btAdvPanels.Checked = !ObjectOptionsEditor._collapsed;
  }

  private void gridValues_Resize(object sender, EventArgs e) => this.CorrectGridColsWidth();

  private void IncompEditor_Changed(object sender, EventArgs e)
  {
    this.IsInternalChanged |= this.incompEditor.IsChanged;
  }

  private void DoSwitchOptionObligatory(object sender, EventArgs e)
  {
    if (this._inEvents)
      return;
    int num = this._options.ObjectID != 0L ? 1 : 0;
    bool flag = !this._readOnly && (this._accessRights & OptionAccessRights.FullAccess) != 0;
    if (num == 0 || !flag)
      return;
    OptionHolder selectedOption = this.GetSelectedOption();
    if (selectedOption == null)
      return;
    this._inEvents = true;
    try
    {
      bool obligatory = sender == this.cbObligatoryOption ? this.cbObligatoryOption.Checked : this.mnpObligatoryOption.Checked;
      this._options.VisibleOptionValues.SetObligatoryOption(selectedOption.OptionGuid, obligatory);
      this.cbObligatoryOption.ImageIndex = obligatory ? 8 : 7;
      this.mnpObligatoryOption.ImageIndex = this.cbObligatoryOption.ImageIndex;
      this.cbObligatoryOption.Checked = obligatory;
      this.mnpObligatoryOption.Checked = obligatory;
    }
    finally
    {
      this._inEvents = false;
    }
    this.treeOptions.UpdateRowData(this.treeOptions.SelectedRow);
    this.IsInternalChanged = true;
  }

  private void DoChangeColWidth(object sender, iGColWidthEventArgs e)
  {
    ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode][this.gridValues.Cols[e.ColIndex].Key] = e.Width;
    this.CorrectGridColsWidth();
  }

  private void gridValues_SelectionChanged(object sender, EventArgs e)
  {
    this.FillPanel(this.GetSelectedOptionHolder(), this.GetSelectedOptionValue());
  }

  private void gridValues_ColWidthChanging(object sender, iGColWidthEventArgs e)
  {
    ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode][this.gridValues.Cols[e.ColIndex].Key] = e.Width;
    this.CorrectGridColsWidth();
  }

  private void splitter_VisibleChanged(object sender, EventArgs e)
  {
  }

  private void splitter_SplitterMoved(object sender, SplitterEventArgs e)
  {
  }

  private void DoDefaultView(object sender, EventArgs e)
  {
    if (this._inEvents)
      return;
    ObjectOptionsEditor._thumbnailMode = false;
    this.SetHandlers();
    this.FillGrid();
    this.UpdateControls();
  }

  private void DoThumbnailsView(object sender, EventArgs e)
  {
    if (this._inEvents)
      return;
    ObjectOptionsEditor._thumbnailMode = true;
    this.SetHandlers();
    this.FillGrid();
    this.UpdateControls();
  }

  private void DoShowHideInvisibles(object sender, EventArgs e)
  {
    if (this._inEvents)
      return;
    ObjectOptionsEditor._hideInvisibles = this.btnHideInvisibles.Checked;
    this.FillGrid();
    this.UpdateControls();
  }

  private void DoSetDefaultValue(object sender, EventArgs e)
  {
    if (this._inEvents)
      return;
    int num1 = this._options.ObjectID != 0L ? 1 : 0;
    bool flag1 = !this._readOnly && (this._accessRights & OptionAccessRights.FullAccess) != 0;
    if (num1 == 0 || !flag1)
      return;
    OptionHolder selectedOptionHolder = this.GetSelectedOptionHolder();
    OptionValue selectedOptionValue = this.GetSelectedOptionValue();
    if (selectedOptionHolder == null || selectedOptionValue == null)
      return;
    this._inEvents = true;
    bool flag2 = false;
    try
    {
      bool flag3 = this.mnpDefaultValue.Checked;
      try
      {
        this._options.VisibleOptionValues.SetDefaultOptionValue(selectedOptionHolder.OptionGuid, flag3 ? selectedOptionValue.ID : string.Empty);
        this.mnpDefaultValue.ImageIndex = flag3 ? 8 : 7;
        flag2 = true;
      }
      catch (Exception ex)
      {
        if (ex is PdmConfiguratorExeption)
        {
          int num2 = (int) IMMessageBox.Show(LocalizationHolder.rm.GetString("PdmConfigurator_7"), ex.Message, MessageBoxButtons.OK, IMMessageBoxImage.Information);
        }
        else if (ex.InnerException is PdmConfiguratorExeption)
        {
          int num3 = (int) IMMessageBox.Show(LocalizationHolder.rm.GetString("PdmConfigurator_7"), ex.InnerException.Message, MessageBoxButtons.OK, IMMessageBoxImage.Information);
        }
        else
          throw;
      }
    }
    finally
    {
      this._inEvents = false;
      this.SetCellsStyles();
    }
    this.IsInternalChanged = flag2;
  }

  private void DoExpandGrid(object sender, EventArgs e)
  {
    this.gridValues.PerformAction(iGActions.ExpandAll);
    this.UpdateControls();
  }

  private void DoCollapseGrid(object sender, EventArgs e)
  {
    this.gridValues.PerformAction(iGActions.CollapseAll);
    this.UpdateControls();
  }

  private void contextMenuBarGrid_BeforePopup(object sender, MenuPopupEventArgs e)
  {
    OptionHolder selectedOptionHolder = this.GetSelectedOptionHolder();
    OptionValue selectedOptionValue = this.GetSelectedOptionValue();
    bool flag = selectedOptionHolder != null && selectedOptionValue != null && this._options.VisibleOptionValues.GetDefaultOptionValue(selectedOptionHolder.OptionGuid) == selectedOptionValue.ID;
    this.mnpDefaultValue.ImageIndex = flag ? 8 : 7;
    this.mnpDefaultValue.Checked = flag;
    this.UpdateControls();
  }

  private void gridValues_BeforeCommitEdit(object sender, iGBeforeCommitEditEventArgs e)
  {
    if (this._inEvents)
      return;
    int num1 = this._options.ObjectID != 0L ? 1 : 0;
    bool flag = !this._readOnly && (this._accessRights & OptionAccessRights.FullAccess) != 0;
    if (num1 == 0 || !flag)
      return;
    iGRow row = this.gridValues.Rows[e.RowIndex];
    OptionHolder optionHolder = row.Cells["OPTION"].Value as OptionHolder;
    iGCol col = this.gridValues.Cols[e.ColIndex];
    OptionValue optionValue = row.Cells["TAG"].Value as OptionValue;
    if (optionHolder == null || col.Key != "VISIBLE" || optionValue == null)
      return;
    bool newValue = (bool) e.NewValue;
    try
    {
      if ((optionHolder.OptionFlags & OptionFlags.Obsolete) == OptionFlags.Obsolete)
        throw new PdmConfiguratorExeption(LocalizationHolder.rm.GetString("PdmConfigurator_60"));
      if ((optionValue.Flags & OptionValueFlags.Obsolete) == OptionValueFlags.Obsolete && !newValue && MessageBox.Show(LocalizationHolder.rm.GetString("PdmConfigurator_61"), LocalizationHolder.rm.GetString("PdmConfigurator_62"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
      {
        e.Result = iGEditResult.Cancel;
        return;
      }
      this._options.VisibleOptionValues.SetVisibleOptionValue(optionHolder.OptionGuid, optionValue.ID, newValue);
    }
    catch (Exception ex)
    {
      e.Result = iGEditResult.Cancel;
      int num2 = (int) MessageBox.Show(ex.Message, LocalizationHolder.rm.GetString("PdmConfigurator_63"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      return;
    }
    e.Result = iGEditResult.Commit;
  }

  private void gridValues_AfterCommitEdit(object sender, iGAfterCommitEditEventArgs e)
  {
    iGRow row = this.gridValues.Rows[e.RowIndex];
    this.SetCellsStyle(this.gridValues.Rows[e.RowIndex]);
    row.Visible = ObjectOptionsEditor._hideInvisibles && (bool) row.Cells["VISIBLE"].Value || !ObjectOptionsEditor._hideInvisibles;
    if (!row.Visible && row.Cells[0].Selected)
    {
      this.gridValues.PerformAction(iGActions.DeselectAll);
      this.gridValues.CurRow = (iGRow) null;
      for (int colIndex = 0; colIndex < row.Cells.Count; ++colIndex)
        row.Cells[colIndex].Selected = false;
    }
    OptionValue optionValue = row.Cells["TAG"].Value as OptionValue;
    if ((optionValue.Flags & OptionValueFlags.Obsolete) == OptionValueFlags.Obsolete && !(bool) row.Cells["VISIBLE"].Value)
      this.gridValues.Rows.RemoveAt(row.Index);
    if (row.Cells["OPTION"].Value is OptionHolder optionHolder)
      this._options.VisibleOptionValues.SetVisibleOptionValue(optionHolder.OptionGuid, optionValue.ID, (bool) row.Cells["VISIBLE"].Value);
    if (row.Index >= this.gridValues.Rows.Count || row.Index < this.gridValues.Rows.Count && !row.Visible)
      this.gridValues_SelectionChanged((object) null, (EventArgs) null);
    this.IsInternalChanged = true;
  }

  private void pagesMain_SelectedPageChanging(object sender, PageControlCancelEventArgs e)
  {
    if (this.incompEditor.IsChanged)
      this.incompEditor.Save();
    if (this.linkedEditor.IsChanged)
      this.linkedEditor.Save();
    if (this.codeEditor.IsChanged)
      this.codeEditor.Save();
    if (e.TabPage == this.pageContext)
    {
      PdmConfiguratorContext context = this.contextEditor.Context;
      context.ObjectsOptions.Clear();
      context.ObjectsOptions.Add(this._options);
      context.SyncOptionsList(true);
      this.contextEditor.Context = context;
    }
    else
    {
      if (e.TabPage != this.pageCode)
        return;
      this.FillCodeStructure();
    }
  }

  private void pagesMain_SelectedPageChanged(object sender, EventArgs e)
  {
  }

  private void gridValues_CustomDrawCellGetHeight(
    object sender,
    iGCustomDrawCellGetHeightEventArgs e)
  {
    iGCol col = this.gridValues.Cols[e.ColIndex];
    iGRow row = this.gridValues.Rows[e.RowIndex];
    if (col.Key == "STATUS" || col.Key == "VISIBLE")
    {
      e.Height = this.gridValues.DefaultRow.Height;
    }
    else
    {
      if (!ObjectOptionsEditor._thumbnailMode)
        return;
      e.Height = this.gridValues.DefaultRow.Height;
      if (col.Key != "IMAGE")
        return;
      OptionValue optionValue = row.Cells["TAG"].Value as OptionValue;
      if (row.Cells["TAG"].Value is OptionHolder)
      {
        e.Height = Math.Max(e.Height, 25);
      }
      else
      {
        object picture = optionValue == null || !(optionValue.Image != Guid.Empty) ? (object) null : this._cache.GetPicture(optionValue.Image);
        if (picture == null)
          return;
        Size imageObjectSizeAdv = ThumbnailRenderer.GetImageObjectSizeAdv(picture, row.Cells[e.ColIndex].Bounds);
        if (imageObjectSizeAdv.Height == 0 || imageObjectSizeAdv.Width == 0)
          return;
        double num1 = (double) imageObjectSizeAdv.Width / (double) row.Cells[e.ColIndex].Bounds.Width;
        double num2 = (double) imageObjectSizeAdv.Height / num1;
        e.Height = Math.Max(e.Height, Convert.ToInt32(num2));
      }
    }
  }

  private void gridValues_CustomDrawCellForeground(object sender, iGCustomDrawCellEventArgs e)
  {
    if (!ObjectOptionsEditor._thumbnailMode || this.gridValues.Cols[e.ColIndex].Key != "IMAGE")
      return;
    object picture = !(this.gridValues.Rows[e.RowIndex].Cells["TAG"].Value is OptionValue optionValue) || !(optionValue.Image != Guid.Empty) ? (object) null : this._cache.GetPicture(optionValue.Image);
    if (picture == null)
      return;
    Rectangle imageBounds;
    ref Rectangle local = ref imageBounds;
    Rectangle bounds = e.Bounds;
    int x = bounds.Left + 1;
    bounds = e.Bounds;
    int y = bounds.Top + 1;
    int width = e.Bounds.Width - 2;
    int height = e.Bounds.Height - 2;
    local = new Rectangle(x, y, width, height);
    ThumbnailRenderer.GetImageObjectSizeAdv(picture, imageBounds);
    ThumbnailRenderer.DrawImageObjectAdv(e.Graphics, picture, imageBounds, this.gridValues.Font, ObjectOptionsEditor._imageStringFormat);
  }

  private void tabs_SelectedIndexChanged(object sender, EventArgs e)
  {
    this.FillPanel(this.GetSelectedOptionHolder(), this.GetSelectedOptionValue());
  }

  private void incompEditor_OnChanged(object sender, EventArgs e)
  {
    this.IsInternalChanged |= this.incompEditor.IsChanged;
  }

  private void linkedEditor_OnChanged(object sender, EventArgs e)
  {
    this.IsInternalChanged |= this.linkedEditor.IsChanged;
  }

  private void btnDefaultImport_Click(object sender, EventArgs e)
  {
    this.ImportOptions(PdmAnalyzerFlags.Default);
  }

  private void btnInCompositions_Click(object sender, EventArgs e)
  {
    this.ImportOptions(PdmAnalyzerFlags.InCompositions);
  }

  private void btnRecursiveImport_Click(object sender, EventArgs e)
  {
    this.ImportOptions(PdmAnalyzerFlags.InCompositionsRecursive);
  }

  private void btnInObjectComposition_Click(object sender, EventArgs e)
  {
    this.ImportOptions(PdmAnalyzerFlags.InCompositions, (IDBTypedObjectID) new DBTypedObjectID(this._editingObject.F_OBJECT_TYPE, this._editingObject.F_OBJECT_ID, this._editingObject.F_ID, this._editingObject.CAPTION, this._editingObject.F_OWNER_ID, this._editingObject.F_VERSION_ID, this._editingObject.F_BASE_VERSION, string.Empty, this._editingObject.F_MODIFICATION_ID));
  }

  private void btnObjectRecursiveImport_Click(object sender, EventArgs e)
  {
    this.ImportOptions(PdmAnalyzerFlags.InCompositionsRecursive, (IDBTypedObjectID) new DBTypedObjectID(this._editingObject.F_OBJECT_TYPE, this._editingObject.F_OBJECT_ID, this._editingObject.F_ID, this._editingObject.CAPTION, this._editingObject.F_OWNER_ID, this._editingObject.F_VERSION_ID, this._editingObject.F_BASE_VERSION, string.Empty, this._editingObject.F_MODIFICATION_ID));
  }

  private void pages_SelectedPageChanging(object sender, PageControlCancelEventArgs e)
  {
    if (e.TabIndex != 1)
      return;
    this.FillAppGrid();
  }

  private void tabs_SelectedPageChanging(object sender, PageControlCancelEventArgs e)
  {
  }

  private void DoCellMouseDown(object sender, iGCellMouseDownEventArgs e)
  {
    if (!(sender is iGrid iGrid) || e.Button != MouseButtons.Right)
      return;
    iGRow row = iGrid.Rows[e.RowIndex];
    iGrid.PerformAction(iGActions.DeselectAll);
    this.iGridSelectRowCells(row, true);
    iGrid.CurRow = row;
  }

  private void contextEditor_OnChanged(object sender, EventArgs e)
  {
    this.IsChanged |= this.contextEditor.IsChanged;
    this.IsInternalChanged |= this.contextEditor.IsChanged;
  }

  private void codeEditor_OnChanged(object sender, EventArgs e)
  {
    this.IsInternalChanged |= this.codeEditor.IsChanged;
  }

  private void appEditor_Changed(object sender, EventArgs e)
  {
    this.IsInternalChanged |= this.appEditor.IsChanged;
  }

  private void Init()
  {
    this._images = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
    this._objtypesIcons = ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
    this._navGraphicsCache = ServicesManager.GetService(typeof (INavGraphicsCache)) as INavGraphicsCache;
    this._userRole = ServicesManager.GetService(typeof (ICurrentUserAndRole)) as ICurrentUserAndRole;
    this._userNamesCache = CacheManager.Cache("UserNamesCache") as IUserNamesCache;
    this._cache = ServicesManager.GetService(typeof (IPicturesCache)) as IPicturesCache;
    this._filtrationService = ServicesManager.GetService(typeof (IFiltrationService)) as IFiltrationService;
    this._notifications = ServicesManager.GetService(typeof (INotificationService)) as INotificationService;
    if (this._notifications != null && this._notifyHandler == null)
    {
      this._notifyHandler = new NotificationEventHandler(this.NotificationEventFired);
      this._notifications.Subscribe(this._notifyHandler);
    }
    if (this._images != null)
    {
      this.mnpCard.Image = this._images.ImageList.Images[this._images.ImageIndex("imgCard")];
      this.btnCard.Image = this.mnpCard.Image;
      this.btnOpenInNewWindow.Image = this._images.ImageList.Images[this._images.ImageIndex("imgNavigator")];
      this.mnpOpenInNewWindow.Image = this.btnOpenInNewWindow.Image;
      this._developerMode = (ServicesManager.GetService(typeof (ICurrentUserAndRole)) as ICurrentUserAndRole).DeveloperMode;
    }
    this._isChanged = false;
    this._isInternalChanged = false;
    this._context.ContextsCache = ServicesManager.GetService(typeof (PdmConfiguratorContextsCache)) as PdmConfiguratorContextsCache;
    this._contextSource.ContextsCache = this._context.ContextsCache;
    this._splitContainer.Panel2Collapsed = ObjectOptionsEditor._collapsed;
    bool inEvents = this._inEvents;
    try
    {
      this._inEvents = true;
      this.btnHideInvisibles.Checked = ObjectOptionsEditor._hideInvisibles;
    }
    finally
    {
      this._inEvents = inEvents;
    }
    this.FillGrid();
    this.SetHandlers();
    this.UpdateControls();
  }

  private int GetTypeImageIndex(FieldTypes attrType)
  {
    return this._objtypesIcons == null ? -1 : this._objtypesIcons.IndexOf(3, -1, (object) attrType);
  }

  private void PrepareGridsStyles()
  {
    bool flag1 = this._options.ObjectID != 0L;
    bool flag2 = !this._readOnly && (this._accessRights & OptionAccessRights.FullAccess) != 0;
    if (this.cellStyle == null)
    {
      this.cellStyle = new iGCellStyle(true);
      this.cellStyle.ImageAlign = iGContentAlignment.TopLeft;
      this.cellStyle.TextAlign = iGContentAlignment.TopLeft;
      this.cellStyle.ReadOnly = iGBool.True;
      this.cellStyle.TextFormatFlags = iGStringFormatFlags.WordWrap;
      this.cellStyle.ImageList = this.imagesToolbars;
      this.cellStyleBold = this.cellStyle.Clone();
      this.cellStyleBold.Font = ObjectOptionsEditor._treeBoldFont;
      this.cellCheckBoxEdit = new iGCellStyle(true);
      this.cellCheckBoxEdit.ImageAlign = iGContentAlignment.TopCenter;
      this.cellCheckBoxEdit.TextAlign = iGContentAlignment.TopCenter;
      this.cellCheckBoxEdit.Type = iGCellType.Check;
      this.cellCheckBoxEdit.ValueType = typeof (bool);
      this.cellCheckBoxEdit.SingleClickEdit = iGBool.True;
      this.cellCheckBoxEdit.EmptyStringAs = iGEmptyStringAs.EmptyString;
      this.cellCheckBox = new iGCellStyle(true);
      this.cellCheckBox.ImageAlign = iGContentAlignment.TopCenter;
      this.cellCheckBox.TextAlign = iGContentAlignment.TopCenter;
      this.cellCheckBox.Type = iGCellType.Check;
      this.cellCheckBox.ValueType = typeof (bool);
      this.cellCheckBox.SingleClickEdit = iGBool.False;
      this.cellCheckBox.ReadOnly = iGBool.True;
      this.cellCheckBox.EmptyStringAs = iGEmptyStringAs.EmptyString;
      this.cellInt64 = new iGCellStyle(true);
      this.cellInt64.TextAlign = iGContentAlignment.TopLeft;
      this.cellInt64.ValueType = typeof (long);
      this.cellInt64.SingleClickEdit = iGBool.False;
      this.cellInt64.ReadOnly = iGBool.True;
      this.cellInt64.EmptyStringAs = iGEmptyStringAs.EmptyString;
      this.cellDouble = new iGCellStyle(true);
      this.cellDouble.TextAlign = iGContentAlignment.TopLeft;
      this.cellDouble.ValueType = typeof (string);
      this.cellDouble.SingleClickEdit = iGBool.False;
      this.cellDouble.ReadOnly = iGBool.True;
      this.cellDouble.EmptyStringAs = iGEmptyStringAs.EmptyString;
      this.cellDateTime = new iGCellStyle(true);
      this.cellDateTime.TextAlign = iGContentAlignment.TopLeft;
      this.cellDateTime.ValueType = typeof (string);
      this.cellDateTime.SingleClickEdit = iGBool.False;
      this.cellDateTime.ReadOnly = iGBool.True;
      this.cellDateTime.EmptyStringAs = iGEmptyStringAs.EmptyString;
      this.cellString = new iGCellStyle(true);
      this.cellString.TextAlign = iGContentAlignment.TopLeft;
      this.cellString.ValueType = typeof (string);
      this.cellString.SingleClickEdit = iGBool.False;
      this.cellString.ReadOnly = iGBool.True;
      this.cellString.EmptyStringAs = iGEmptyStringAs.EmptyString;
      this.cellString.TextFormatFlags = iGStringFormatFlags.WordWrap;
      this.cellString.ImageList = this.imagesToolbars;
      this.cellString.ImageAlign = iGContentAlignment.TopLeft;
      this.cellImage = new iGCellStyle(true);
      this.cellImage.TextAlign = iGContentAlignment.TopCenter;
      this.cellImage.ValueType = typeof (Image);
      this.cellImage.SingleClickEdit = iGBool.False;
      this.cellImage.ReadOnly = iGBool.True;
      this.cellImage.EmptyStringAs = iGEmptyStringAs.EmptyString;
      this.cellImage.CustomDrawFlags = iGCustomDrawFlags.Foreground;
      this.cellStyleStatus = this.cellImage.Clone();
      this.cellStyleStatus.ImageAlign = iGContentAlignment.TopCenter;
      this.headerStyle = new iGColHdrStyle(true);
      this.headerStyle.TextAlign = iGContentAlignment.TopLeft;
    }
    this.cellCheckBoxEdit.ReadOnly = flag2 & flag1 ? iGBool.False : iGBool.True;
  }

  private void PrepareGridsColumns()
  {
    long objectId = this._options.ObjectID;
    int num1 = this._readOnly ? 0 : ((this._accessRights & OptionAccessRights.FullAccess) != 0 ? 1 : 0);
    this.gridValues.Header.ImageList = this._objtypesIcons != null ? this._objtypesIcons.ImageList : (ImageList) null;
    this.PrepareGridsStyles();
    if (ObjectOptionsEditor._colWidths.Count == 0)
    {
      Dictionary<string, int> dictionary = new Dictionary<string, int>();
      dictionary.Add("VISIBLE", 64 /*0x40*/);
      dictionary.Add("IMAGE", 64 /*0x40*/);
      dictionary.Add("VALUE", 256 /*0x0100*/);
      dictionary.Add("CODE", 90);
      dictionary.Add("NOTE", 256 /*0x0100*/);
      dictionary.Add("TAG", 0);
      dictionary.Add("OPTION", 0);
      dictionary.Add("ID", 90);
      dictionary.Add("STATUS", 0);
      ObjectOptionsEditor._colWidths[false] = dictionary;
      ObjectOptionsEditor._colWidths[true] = new Dictionary<string, int>((IDictionary<string, int>) dictionary)
      {
        ["IMAGE"] = 90
      };
    }
    FieldTypes selectedOptionDataType = this.GetSelectedOptionDataType();
    iGCol col1 = this.gridValues.Cols["VISIBLE"];
    iGCol iGcol1 = this.gridValues.Cols["VISIBLE"] ?? this.gridValues.Cols.Add(new iGColPattern(Math.Max(64 /*0x40*/, ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["VISIBLE"]), true, true, 64 /*0x40*/, -1, true, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_42"), "VISIBLE", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol1.CellStyle = this.cellCheckBoxEdit;
    iGcol1.Width = ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["VISIBLE"];
    iGcol1.ColHdrStyle = this.headerStyle;
    iGCol col2 = this.gridValues.Cols["IMAGE"];
    iGCol iGcol2 = this.gridValues.Cols["IMAGE"] ?? this.gridValues.Cols.Add(new iGColPattern(Math.Max(36, ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["IMAGE"]), ObjectOptionsEditor._thumbnailMode, true, 36, -1, true, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_38"), "IMAGE", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol2.CellStyle = this.cellImage;
    iGcol2.Width = ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["IMAGE"];
    iGcol2.ColHdrStyle = this.headerStyle;
    iGcol2.Visible = ObjectOptionsEditor._thumbnailMode;
    iGCol col3 = this.gridValues.Cols["VALUE"];
    iGCol iGcol3 = this.gridValues.Cols["VALUE"] ?? this.gridValues.Cols.Add(new iGColPattern(Math.Max(64 /*0x40*/, ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["VALUE"]), true, true, 64 /*0x40*/, -1, true, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_43"), "VALUE", this.GetTypeImageIndex(selectedOptionDataType), (object) string.Empty, (object) string.Empty, -1));
    iGcol3.Width = ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["VALUE"];
    iGcol3.ColHdrStyle = this.headerStyle;
    iGcol3.ImageIndex = this.GetTypeImageIndex(selectedOptionDataType);
    switch (selectedOptionDataType)
    {
      case FieldTypes.ftInteger:
        iGcol3.CellStyle = this.cellInt64;
        break;
      case FieldTypes.ftDouble:
        iGcol3.CellStyle = this.cellDouble;
        break;
      case FieldTypes.ftDateTime:
        iGcol3.CellStyle = this.cellDateTime;
        break;
      case FieldTypes.ftBoolean:
        iGcol3.CellStyle = this.cellCheckBox;
        break;
      default:
        iGcol3.CellStyle = this.cellString;
        break;
    }
    iGCol col4 = this.gridValues.Cols["CODE"];
    iGCol iGcol4 = this.gridValues.Cols["CODE"] ?? this.gridValues.Cols.Add(new iGColPattern(Math.Max(64 /*0x40*/, ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["CODE"]), true, true, 64 /*0x40*/, -1, true, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_44"), "CODE", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol4.CellStyle = this.cellStyleBold;
    iGcol4.Width = ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["CODE"];
    iGcol4.ColHdrStyle = this.headerStyle;
    iGCol col5 = this.gridValues.Cols["NOTE"];
    iGCol iGcol5 = this.gridValues.Cols["NOTE"] ?? this.gridValues.Cols.Add(new iGColPattern(Math.Max(64 /*0x40*/, ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["NOTE"]), true, true, 64 /*0x40*/, -1, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_40"), "NOTE", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol5.CellStyle = this.cellStyle;
    iGcol5.Width = ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["NOTE"];
    iGcol5.ColHdrStyle = this.headerStyle;
    iGCol col6 = this.gridValues.Cols["TAG"];
    (this.gridValues.Cols["TAG"] ?? this.gridValues.Cols.Add(new iGColPattern(ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["TAG"], false, false, 0, 0, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) "", "TAG", -1, (object) null, (object) null, -1))).Width = ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["TAG"];
    iGCol col7 = this.gridValues.Cols["STATUS"];
    iGCol iGcol6 = this.gridValues.Cols["STATUS"] ?? this.gridValues.Cols.Add(new iGColPattern(ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["STATUS"], true, false, 0, -1, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) "", "STATUS", -1, (object) null, (object) null, -1));
    iGcol6.Width = ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["STATUS"];
    iGcol6.CellStyle = this.cellStyleStatus;
    iGCol col8 = this.gridValues.Cols["OPTION"];
    (this.gridValues.Cols["OPTION"] ?? this.gridValues.Cols.Add(new iGColPattern(ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["OPTION"], false, false, 0, 0, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) "", "OPTION", -1, (object) null, (object) null, -1))).Width = ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["OPTION"];
    int num2 = this.gridValues.Cols["ID"] == null ? 1 : 0;
    iGCol iGcol7 = this.gridValues.Cols["ID"] ?? this.gridValues.Cols.Add(new iGColPattern(Math.Max(64 /*0x40*/, ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["ID"]), this._developerMode, true, 64 /*0x40*/, -1, true, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_45"), "ID", -1, (object) string.Empty, (object) string.Empty, -1));
    if (num2 != 0)
      iGcol7.CellStyle = this.cellStyle;
    iGcol7.Width = ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["ID"];
    iGcol7.ColHdrStyle = this.headerStyle;
    iGcol7.Visible = false;
    this.CorrectGridColsWidth();
  }

  private void CorrectGridColsWidth()
  {
    if (this.gridValues.AutoResizeCols || ObjectOptionsEditor._colWidths.Count == 0)
      return;
    int num = this.gridValues.ClientRectangle.Width - 30 - ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["VISIBLE"] - ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["OPTION"] - ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["VALUE"] - ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["CODE"] - ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["TAG"] - ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["OPTION"];
    if (ObjectOptionsEditor._thumbnailMode)
      num -= ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["IMAGE"];
    if (this.gridValues.Cols.Count == 0)
      return;
    this.gridValues.Cols["VISIBLE"].Width = ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["VISIBLE"];
    this.gridValues.Cols["OPTION"].Width = ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["OPTION"];
    this.gridValues.Cols["VALUE"].Width = ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["VALUE"];
    this.gridValues.Cols["CODE"].Width = ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["CODE"];
    this.gridValues.Cols["OPTION"].Width = ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["OPTION"];
    if (num > 32 /*0x20*/)
    {
      ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["NOTE"] = num;
      this.gridValues.Cols["NOTE"].Width = num;
    }
    else
      this.gridValues.Cols["NOTE"].Width = ObjectOptionsEditor._colWidths[ObjectOptionsEditor._thumbnailMode]["NOTE"];
    this.gridValues.Rows.AutoHeight();
  }

  private void CheckAccessRights()
  {
    this._accessRights = OptionAccessRights.ReadOnly;
    if (this._options.ObjectID == 0L)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this.CheckAccessRights(sessionKeeper.Session.GetObject(this._options.ObjectID, false));
  }

  private void CheckAccessRights(IDBObject obj)
  {
    this._accessRights = OptionAccessRights.ReadOnly;
    if (obj == null)
      return;
    if (obj is IDBSecurity dbSecurity && dbSecurity.CheckAccess(ActionType.Edit, true, false))
      this._accessRights = OptionAccessRights.FullAccess;
    ObjectModifyModes objectModifyMode = obj.ObjectModifyMode;
    long checkoutBy = obj.CheckoutBy;
    if (objectModifyMode != ObjectModifyModes.CantModify && (objectModifyMode != ObjectModifyModes.Checkout || checkoutBy == this._userRole.UserID) && (objectModifyMode != ObjectModifyModes.CreateVersion || checkoutBy == this._userRole.UserID))
      return;
    this._accessRights = OptionAccessRights.ReadOnly;
  }

  private void UpdateControls()
  {
    bool flag1 = this._options.ObjectID != 0L;
    bool flag2 = !this._readOnly && (this._accessRights & OptionAccessRights.FullAccess) != 0;
    bool flag3 = this.treeOptions.SelectedRows.Count > 0 && this.treeOptions.RootRow.NumChildren > 0;
    this.btnAddOptions.Enabled = flag2 & flag1;
    this.mnpAddOptions.Enabled = this.btnAddOptions.Enabled;
    this.btnImport.Enabled = this.btnAddOptions.Enabled;
    this.mnpDefaultImport.Enabled = this.btnAddOptions.Enabled;
    this.btnInCompositions.Enabled = this.btnAddOptions.Enabled;
    this.mnpInCompositions.Enabled = this.btnAddOptions.Enabled;
    this.btnRecursiveImport.Enabled = this.btnAddOptions.Enabled;
    this.mnpRecursiveImport.Enabled = this.btnAddOptions.Enabled;
    this.btnDeleteOptions.Enabled = this.btnAddOptions.Enabled & flag3;
    this.mnpDeleteOptions.Enabled = this.btnDeleteOptions.Enabled;
    this.btnCard.Enabled = this.GetSelectedObject() != 0L;
    this.mnpCard.Enabled = this.btnCard.Enabled;
    this.btnOpenInNewWindow.Enabled = this.btnCard.Enabled;
    this.mnpOpenInNewWindow.Enabled = this.btnOpenInNewWindow.Enabled;
    OptionHolder selectedOption = this.GetSelectedOption();
    List<OptionHolder> selectedOptions = this.GetSelectedOptions();
    OptionObjectDescription selectedCategory = this.GetSelectedCategory();
    OptionValue selectedOptionValue = this.GetSelectedOptionValue();
    bool inEvents = this._inEvents;
    try
    {
      this._inEvents = true;
      this.panelHint.Visible = !flag2;
      if (selectedCategory == null && selectedOptions.Count == 0)
      {
        this.gridValues.Rows.Clear();
        this.gridValues.ReadOnly = true;
        this.picture.Enabled = false;
        this.picture.Image = (Image) null;
        this.tabs.Enabled = false;
      }
      else
      {
        this.panelGrid.Enabled = true;
        this.gridValues.ReadOnly = !flag1 || !flag2;
        this.picture.Enabled = true;
        this.tabs.Enabled = true;
      }
      this.cbObligatoryOption.Enabled = flag1 & flag2 && selectedOptions.Count == 1 && selectedOption != null && (selectedOption.OptionFlags & OptionFlags.Obsolete) != OptionFlags.Obsolete;
      this.mnpObligatoryOption.Enabled = this.cbObligatoryOption.Enabled;
      this.lbWarning.Visible = flag1 && selectedOptions.Count == 1 && selectedOption != null && (selectedOption.OptionFlags & OptionFlags.Obsolete) == OptionFlags.Obsolete;
      bool flag4 = this.Key != null && !this.Key.Empty && MetaDataHelper.IsPdmConfigurableRelationType(this.Key.F_RELATION_TYPE);
      bool flag5 = this.Key != null && !this.Key.Empty && (MetaDataHelper.IsPdmConfigurableObjectType(this.Key.F_OBJECT_TYPE) || MetaDataHelper.IsPdmPartiallyConfigurableRelationType(this.Key.F_RELATION_TYPE));
      this.mnpDefaultValue.Enabled = flag1 & flag2 && selectedOptionValue != null;
      this.pageAppls.TabVisible = (flag4 || this._ignoreNullRelation) && this.ParentKey != null && this.ParentKey.F_PROJ_ID != 0L && !MetaDataHelper.IsObjectTypeChildOf(this.ParentKey.F_OBJECT_TYPE, Intermech.Interfaces.PdmConfigurator.Consts.objtypeComplementsID);
      this.pageContext.TabVisible = flag5;
      this.contextEditor.ReadOnly = this._applsAccessRights == PdmContextAccessRights.ReadOnly;
    }
    finally
    {
      this._inEvents = inEvents;
    }
  }

  private iGRow AddOption(OptionHolder option)
  {
    if (option == null)
      return (iGRow) null;
    iGRow iGrow = this.gridValues.Rows.Add();
    iGrow.Level = 0;
    iGrow.Height = 40;
    iGrow.NormalCellHeight = 40;
    for (int colIndex = 0; colIndex < iGrow.Cells.Count; ++colIndex)
    {
      iGrow.Cells[colIndex].ReadOnly = iGBool.True;
      iGrow.Cells[colIndex].Style = this.gridValues.Cols["VALUE"].CellStyle.Clone();
      if ((option.OptionFlags & OptionFlags.Obsolete) == OptionFlags.Obsolete)
        iGrow.Cells[colIndex].BackColor = Color.LavenderBlush;
    }
    iGrow.Cells["VISIBLE"].Style.CustomDrawFlags = iGCustomDrawFlags.Foreground;
    iGrow.Cells["VALUE"].Value = (object) option.OptionCaption;
    iGrow.Cells["VALUE"].Font = ObjectOptionsEditor._treeBoldFont;
    iGrow.Cells["VALUE"].Style.TextAlign = iGContentAlignment.MiddleLeft;
    iGrow.Cells["VALUE"].Flags = iGCellFlags.DisplayText;
    iGrow.TreeButton = iGTreeButtonState.Visible;
    iGrow.Cells["TAG"].Value = (object) option;
    iGrow.Cells["OPTION"].Value = (object) option;
    iGrow.Cells["STATUS"].Value = (object) option;
    if ((option.OptionFlags & OptionFlags.Obsolete) == OptionFlags.Obsolete)
      iGrow.Cells["NOTE"].ImageIndex = 9;
    return iGrow;
  }

  private iGRow AddOptionValue(OptionHolder option, OptionValue value, int level)
  {
    bool visibleOptionValue = this._options.VisibleOptionValues.GetVisibleOptionValue(option.OptionGuid, value.ID);
    bool flag = (value.Flags & OptionValueFlags.Obsolete) == OptionValueFlags.Obsolete;
    iGRow iGrow = this.gridValues.Rows.Add();
    iGrow.Level = level;
    iGrow.Cells["VISIBLE"].Value = (object) visibleOptionValue;
    iGrow.Cells["CODE"].Value = (object) value.Code;
    switch (option.OptionDataType)
    {
      case FieldTypes.ftInteger:
        iGrow.Cells["VALUE"].Value = (object) option.GetAsInt64(value.ID);
        break;
      case FieldTypes.ftDouble:
        iGrow.Cells["VALUE"].Value = (object) option.GetAsDouble(value.ID);
        break;
      case FieldTypes.ftDateTime:
        DateTime asDateTime = option.GetAsDateTime(value.ID);
        iGrow.Cells["VALUE"].Value = (object) asDateTime.ToShortDateString();
        break;
      case FieldTypes.ftBoolean:
        iGrow.Cells["VALUE"].Value = this.gridValues.Cols["VALUE"].CellStyle != this.cellCheckBox ? (option.GetAsBoolean(value.ID) ? (object) "Истина" : (object) "Ложь") : (object) option.GetAsBoolean(value.ID);
        break;
      default:
        iGrow.Cells["VALUE"].Value = (object) option.GetAsString(value.ID);
        break;
    }
    iGrow.Cells["NOTE"].Value = (object) value.Description;
    if (flag)
      iGrow.Cells["NOTE"].ImageIndex = 9;
    iGrow.Cells["TAG"].Value = (object) value;
    iGrow.Cells["OPTION"].Value = (object) option;
    iGrow.Cells["ID"].Value = (object) value.ID;
    int num = flag ? 1 : 0;
    iGrow.Visible = (ObjectOptionsEditor._hideInvisibles & visibleOptionValue || !ObjectOptionsEditor._hideInvisibles) && (visibleOptionValue || !flag);
    return iGrow;
  }

  private void SetCellsStyles()
  {
    this.prevRow = (iGRow) null;
    for (int index = 0; index < this.gridValues.Rows.Count; ++index)
    {
      if (this.gridValues.Rows[index].Level == 0)
        this.prevRow = (iGRow) null;
      this.SetCellsStyle(this.gridValues.Rows[index]);
    }
  }

  private void SetCellsStyle(iGRow row)
  {
    if (row == null)
      return;
    OptionValue optionValue = row.Cells["TAG"].Value as OptionValue;
    OptionHolder optionHolder = row.Cells["OPTION"].Value as OptionHolder;
    if (optionValue == null || optionHolder == null)
      return;
    bool flag1 = (bool) row.Cells["VISIBLE"].Value;
    bool flag2 = this._options.VisibleOptionValues.GetDefaultOptionValue(optionHolder.OptionGuid) == optionValue.ID;
    for (int colIndex = 0; colIndex < row.Cells.Count; ++colIndex)
      row.Cells[colIndex].ForeColor = flag1 ? (!flag2 ? SystemColors.ControlText : Color.Blue) : SystemColors.GrayText;
    if (!row.Visible)
      return;
    if ((optionHolder.OptionFlags & OptionFlags.Obsolete) == OptionFlags.Obsolete || (optionValue.Flags & OptionValueFlags.Obsolete) == OptionValueFlags.Obsolete)
    {
      for (int colIndex = 1; colIndex < row.Cells.Count; ++colIndex)
        row.Cells[colIndex].BackColor = Color.LavenderBlush;
    }
    else
    {
      if (this.prevRow != null && this.prevRow.Cells[1].BackColor == Color.WhiteSmoke)
      {
        for (int colIndex = 1; colIndex < row.Cells.Count; ++colIndex)
          row.Cells[colIndex].BackColor = SystemColors.Window;
      }
      else
      {
        for (int colIndex = 1; colIndex < row.Cells.Count; ++colIndex)
          row.Cells[colIndex].BackColor = Color.WhiteSmoke;
      }
      this.prevRow = row;
    }
  }

  private void FillGrid()
  {
    bool inEvents = this._inEvents;
    List<OptionHolder> source = this.GetSelectedOptions();
    if (source != null)
      source = source.OrderBy<OptionHolder, string>((Func<OptionHolder, string>) (o => o.OptionCaption)).ToList<OptionHolder>();
    try
    {
      this._inEvents = true;
      this.gridValues.Rows.Clear();
      this.PrepareGridsColumns();
      if (source.Count == 0)
        return;
      if (source.Count == 1)
      {
        this.cbObligatoryOption.Checked = this._options.VisibleOptionValues.GetObligatoryOption(source[0].OptionGuid);
        this.mnpObligatoryOption.Checked = this.cbObligatoryOption.Checked;
      }
      else
      {
        this.cbObligatoryOption.Checked = false;
        this.mnpObligatoryOption.Checked = this.cbObligatoryOption.Checked;
      }
      this.cbObligatoryOption.ImageIndex = this.cbObligatoryOption.Checked ? 8 : 7;
      this.mnpObligatoryOption.ImageIndex = this.cbObligatoryOption.ImageIndex;
      int level = source.Count > 1 ? 1 : 0;
      for (int index1 = 0; index1 < source.Count; ++index1)
      {
        OptionHolder option = source[index1];
        if (source.Count > 1)
          this.AddOption(option);
        this.prevRow = (iGRow) null;
        for (int index2 = 0; index2 < option.OptionValues.Count; ++index2)
        {
          OptionValue optionValue = option.OptionValues[index2];
          this.SetCellsStyle(this.AddOptionValue(option, optionValue, level));
        }
      }
      this.gridValues.Rows.AutoHeight();
    }
    finally
    {
      this.prevRow = (iGRow) null;
      this.gridValues_SelectionChanged((object) null, (EventArgs) null);
      this._inEvents = inEvents;
    }
    this.CorrectGridColsWidth();
  }

  private void CollectCategories()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      PdmConfiguratorCache.CacheLoadCategories(sessionKeeper.Session);
    this._categories = PdmConfiguratorCache.CacheGetCategoriesList();
    this._optionsHolders.Clear();
    for (int index = this._categories.Count - 1; index >= 0; --index)
    {
      List<OptionHolder> categoryOptions = this._options.FindCategoryOptions(this._categories[index].F_OBJECT_ID);
      if (categoryOptions.Count == 0)
        this._categories.RemoveAt(index);
      else
        this._optionsHolders[this._categories[index].F_OBJECT_ID] = categoryOptions;
    }
  }

  private void FillEditor(bool checkAccess)
  {
    if (checkAccess)
      this.CheckAccessRights();
    try
    {
      this._inEvents = true;
      if (this._editingObject.F_OBJECT_ID == 0L || string.IsNullOrEmpty(this._editingObject.CAPTION))
        this.headerControl.Text = LocalizationHolder.rm.GetString("PdmConfigurator_46");
      else
        this.headerControl.Text = string.Format(LocalizationHolder.rm.GetString("PdmConfigurator_47"), (object) this._editingObject.CAPTION);
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        if (PdmConfiguratorCache.CategoriesCache.Count == 0)
          PdmConfiguratorCache.CacheLoadCategories(sessionKeeper.Session);
        this._options.LoadOptionsToCache(sessionKeeper.Session);
        this._options.ClearVisibleOptionsValuesLists(sessionKeeper.Session);
      }
      this.CollectCategories();
      this.FillTree(true);
      if (this.treeOptions.RootRow != null)
      {
        this.treeOptions.FocusRow = this.treeOptions.RootRow;
        this.treeOptions.SelectedRow = this.treeOptions.RootRow;
      }
      this.FillGrid();
      this.FillAppGrid();
      this.FillContext();
      this.FillCodeStructure();
    }
    finally
    {
      this._inEvents = false;
    }
    this.UpdateControls();
    this.RaiseOnChanged();
  }

  private void FillPanel(OptionHolder option, OptionValue value)
  {
    this.incompEditor.LoadOptions(this._options, option, value, this._accessRights);
    this.linkedEditor.LoadLinkedOptions(this._options, option, value, this._accessRights);
    if (option == null || value == null)
    {
      this.picture.Image = (Image) null;
    }
    else
    {
      if (value != null)
      {
        StringBuilder stringBuilder = new StringBuilder(" ");
        if (!string.IsNullOrEmpty(value.Code))
          stringBuilder.Append($"[{value.Code}]");
        if (!string.IsNullOrEmpty(value.Value))
        {
          if (stringBuilder.Length > 0)
            stringBuilder.Append(" ");
          stringBuilder.Append(value.Value);
        }
      }
      try
      {
        this.picture.Image = value.Image != Guid.Empty ? this._cache.GetPicture(value.Image) as Image : (Image) null;
      }
      catch
      {
        this.picture.Image = (Image) null;
      }
    }
  }

  private void Fix()
  {
    this._optionsSource.Assign((object) this._options);
    this._applCollectionSource.Assign((object) this._applCollection);
    this._contextSource.Assign((object) this._context);
    this.contextEditor.Fix();
    this.appEditor.IsChanged = false;
    this._isChanged = false;
    this._isInternalChanged = false;
    this.UpdateControls();
    this.RaiseOnChanged();
  }

  private void Apply()
  {
    if (this.incompEditor.IsChanged)
      this.incompEditor.Save();
    if (this.linkedEditor.IsChanged)
      this.linkedEditor.Save();
    if (this.codeEditor.IsChanged)
      this.codeEditor.Save();
    if ((this.contextEditor.IsChanged || this.incompEditor.IsChanged) && this._options.Incompatibilities.Evalute(this.contextEditor.Context) == PdmConfiguratorResult.Incompatibles)
      throw new PdmConfiguratorExeption(LocalizationHolder.rm.GetString("PdmConfigurator_50"));
    this._context.Assign((object) this.contextEditor.Context);
    this._context.ObjectsOptions.Clear();
    this._context.ObjectsOptions.Add(this._options);
    this._context.SyncOptionsList(true);
  }

  private void FillTree(bool resetDatasource)
  {
    if (ObjectOptionsEditor._treeBoldFont == null)
    {
      ObjectOptionsEditor._treeBoldFont = new Font(this.treeOptions.Font, FontStyle.Bold);
      ObjectOptionsEditor._treeBoldUnderFont = new Font(this.treeOptions.Font, FontStyle.Bold | FontStyle.Underline);
      ObjectOptionsEditor._treeFontStriked = new Font(this.treeOptions.Font, FontStyle.Strikeout);
    }
    IList selectedItems = this.treeOptions.SelectedItems;
    if (resetDatasource)
      this.treeOptions.DataSource = (object) this;
    this.treeOptions.UpdateRows(true);
    this.treeOptions.UpdateRowData();
    this.treeOptions.RootRow.ExpandChildren(true);
    this.treeOptions.SelectedRow = (Row) null;
    List<object> objectList = new List<object>();
    foreach (object obj in (IEnumerable) selectedItems)
    {
      OptionObjectDescription optionObjectDescription;
      if ((optionObjectDescription = obj as OptionObjectDescription) != null)
      {
        OptionObjectDescription objectDescription = this._categories.FirstOrDefault<OptionObjectDescription>((Func<OptionObjectDescription, bool>) (o => o.F_OBJECT_ID == optionObjectDescription.F_OBJECT_ID));
        if (objectDescription != null)
          objectList.Add((object) objectDescription);
      }
      else
      {
        OptionHolder optionHolder;
        if ((optionHolder = obj as OptionHolder) != null)
        {
          OptionHolder optionHolder1 = (OptionHolder) null;
          foreach (KeyValuePair<long, List<OptionHolder>> optionsHolder in this._optionsHolders)
          {
            optionHolder1 = optionsHolder.Value.FirstOrDefault<OptionHolder>((Func<OptionHolder, bool>) (o => o.OptionObjectID == optionHolder.OptionObjectID));
            if (optionHolder1 != null)
              break;
          }
          if (optionHolder1 != null)
            objectList.Add((object) optionHolder1);
        }
      }
    }
    foreach (object obj in objectList)
      this.SelectRow(this.treeOptions.RootRow, obj);
    this.UpdateControls();
  }

  private void SelectRow(Row row, object item)
  {
    if (row.Item == item)
    {
      row.Selected = true;
    }
    else
    {
      for (int childIndex = 0; childIndex < row.NumChildren; ++childIndex)
        this.SelectRow(row.ChildRowByIndex(childIndex), item);
    }
  }

  private long GetSelectedObject()
  {
    long selectedObject = 0;
    if (this._options == null || this.treeOptions.SelectedRow == null)
      return selectedObject;
    Row selectedRow = this.treeOptions.SelectedRow;
    if (selectedRow.Level == 1)
    {
      OptionObjectDescription objectDescription = (OptionObjectDescription) selectedRow.Item;
      selectedObject = objectDescription != null ? objectDescription.F_OBJECT_ID : 0L;
    }
    if (selectedRow.Level == 2)
    {
      OptionHolder optionHolder = (OptionHolder) selectedRow.Item;
      selectedObject = optionHolder != null ? optionHolder.OptionObjectID : 0L;
    }
    return selectedObject;
  }

  private OptionObjectDescription GetSelectedCategory()
  {
    OptionObjectDescription selectedCategory = (OptionObjectDescription) null;
    if (this._options == null || this.treeOptions.SelectedRow == null)
      return selectedCategory;
    Row selectedRow = this.treeOptions.SelectedRow;
    if (selectedRow.Level == 1)
      selectedCategory = (OptionObjectDescription) selectedRow.Item;
    return selectedCategory;
  }

  private OptionHolder GetSelectedOption()
  {
    OptionHolder selectedOption = (OptionHolder) null;
    if (this._options == null || this.treeOptions.SelectedRow == null)
      return selectedOption;
    Row selectedRow = this.treeOptions.SelectedRow;
    if (selectedRow.Level == 2)
      selectedOption = (OptionHolder) selectedRow.Item;
    return selectedOption;
  }

  private OptionValue GetSelectedOptionValue()
  {
    iGRow row = this.gridValues.SelectedCells.Count > 0 ? this.gridValues.SelectedCells[0].Row : (iGRow) null;
    return row == null ? (OptionValue) null : row.Cells["TAG"].Value as OptionValue;
  }

  private OptionHolder GetSelectedOptionHolder()
  {
    iGRow row = this.gridValues.SelectedCells.Count <= 0 || !this.gridValues.SelectedCells[0].Selected ? (iGRow) null : this.gridValues.SelectedCells[0].Row;
    return row == null ? (OptionHolder) null : row.Cells["OPTION"].Value as OptionHolder;
  }

  private FieldTypes GetSelectedOptionDataType()
  {
    FieldTypes selectedOptionDataType = FieldTypes.ftString;
    if (this._options == null || this.treeOptions.SelectedRow == null)
      return selectedOptionDataType;
    Row selectedRow = this.treeOptions.SelectedRow;
    if (selectedRow.Level == 2)
    {
      OptionHolder optionHolder = (OptionHolder) selectedRow.Item;
      selectedOptionDataType = optionHolder != null ? optionHolder.OptionDataType : FieldTypes.ftString;
    }
    return selectedOptionDataType;
  }

  private List<OptionHolder> GetSelectedOptions()
  {
    List<OptionHolder> selectedOptions = new List<OptionHolder>();
    bool flag = false;
    for (int index = 0; index < this.treeOptions.SelectedRows.Count; ++index)
    {
      flag = this.treeOptions.SelectedRows[index] == this.treeOptions.RootRow;
      if (flag)
        break;
    }
    if (flag)
    {
      for (int index = 0; index < this._options.Options.Count; ++index)
      {
        OptionHolder option = PdmConfiguratorCache.CacheFindOption(this._options.Options[index]);
        if (option != null)
          selectedOptions.Add(option);
      }
      return selectedOptions;
    }
    for (int index1 = 0; index1 < this.treeOptions.SelectedRows.Count; ++index1)
    {
      Row selectedRow = this.treeOptions.SelectedRows[index1];
      if (selectedRow.Level == 1)
      {
        OptionObjectDescription objectDescription = (OptionObjectDescription) selectedRow.Item;
        List<OptionHolder> categoryOptions = this._options.FindCategoryOptions(objectDescription != null ? objectDescription.F_OBJECT_ID : 0L);
        for (int index2 = 0; index2 < categoryOptions.Count; ++index2)
        {
          if (selectedOptions.IndexOf(categoryOptions[index2]) < 0 && categoryOptions[index2] != null)
            selectedOptions.Add(categoryOptions[index2]);
        }
      }
      else if (selectedRow.Level == 2)
      {
        OptionHolder optionHolder = (OptionHolder) selectedRow.Item;
        if (selectedOptions.IndexOf(optionHolder) < 0 && optionHolder != null)
          selectedOptions.Add(optionHolder);
      }
    }
    return selectedOptions;
  }

  private void DoAddOptions(Intermech.Interfaces.PdmConfigurator.ImportOptions options)
  {
    if (options == null || options.Empty)
      return;
    foreach (KeyValuePair<long, ImportOptionProperties> option1 in options.Options)
    {
      if (this._options.Options.IndexOf(option1.Key) < 0)
      {
        OptionHolder option2 = PdmConfiguratorCache.CacheFindOption(option1.Key);
        if (option2 == null)
        {
          if (IMMessageBox.Show(LocalizationHolder.rm.GetString("PdmConfigurator_56"), string.Format(LocalizationHolder.rm.GetString("PdmConfigurator_57"), (object) option1.Key), MessageBoxButtons.YesNo, IMMessageBoxImage.Question) != DialogResult.Yes)
            return;
        }
        else if ((option2.OptionFlags & OptionFlags.Obsolete) == OptionFlags.Obsolete)
        {
          if (IMMessageBox.Show(LocalizationHolder.rm.GetString("PdmConfigurator_58"), string.Format(LocalizationHolder.rm.GetString("PdmConfigurator_59"), (object) option2.OptionCaption), MessageBoxButtons.YesNo, IMMessageBoxImage.Question) != DialogResult.Yes)
            return;
        }
        else
        {
          this._options.AddOption(option1.Key);
          if (option1.Value.VisibleValues != null && option1.Value.VisibleValues.Count > 0)
            this._options.VisibleOptionValues.Items[option2.OptionGuid] = option1.Value.VisibleValues;
          else if (this._options.VisibleOptionValues.Items.ContainsKey(option2.OptionGuid))
            this._options.VisibleOptionValues.Items.Remove(option2.OptionGuid);
          if (option1.Value.IncompCollection != null && !option1.Value.IncompCollection.Empty)
          {
            foreach (KeyValuePair<OptionValuePair, List<OptionValuePair>> keyValuePair in option1.Value.IncompCollection.LinkedOptions.Items)
              this._options.Incompatibilities.LinkedOptions.Items[keyValuePair.Key] = new List<OptionValuePair>((IEnumerable<OptionValuePair>) keyValuePair.Value);
            option1.Value.IncompCollection.LinkedOptions.Clear();
            if (option1.Value.IncompCollection.FindCriterion(option2.OptionGuid) is PdmCriterion criterion2 && !criterion2.Empty && criterion2.Items.Count != 0)
            {
              if (!(this._options.Incompatibilities.FindCriterion(option2.OptionGuid) is PdmCriterion criterion1))
              {
                PdmCriterion pdmCriterion = this._options.Incompatibilities.AddStubCriterion() as PdmCriterion;
                pdmCriterion.Function = LogicalFunction.Or;
                pdmCriterion.Option = option2.OptionGuid;
                pdmCriterion.Assign((object) criterion2);
              }
              else
              {
                criterion1.Items.Clear();
                criterion1.Assign((object) criterion2);
              }
            }
          }
        }
      }
    }
    if (false)
      return;
    this.CollectCategories();
    this.FillTree(false);
    this.FillGrid();
    this.IsInternalChanged = true;
  }

  private void DoAddOptions(IList<long> options)
  {
    List<long> longList = new List<long>();
    if (options == null || options.Count == 0)
      return;
    for (int index = 0; index < options.Count; ++index)
    {
      if (this._options.Options.IndexOf(options[index]) < 0 && longList.IndexOf(options[index]) < 0)
        longList.Add(options[index]);
    }
    if (longList.Count == 0)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      PdmConfiguratorCache.CacheLoadCategories(sessionKeeper.Session);
      PdmConfiguratorCache.CacheLoadOptions(sessionKeeper.Session, (IList<long>) longList);
    }
    for (int index = longList.Count - 1; index >= 0; --index)
    {
      OptionHolder option = PdmConfiguratorCache.CacheFindOption(longList[index]);
      if (option == null)
      {
        longList.RemoveAt(index);
        if (IMMessageBox.Show(LocalizationHolder.rm.GetString("PdmConfigurator_56"), string.Format(LocalizationHolder.rm.GetString("PdmConfigurator_57"), (object) longList[index]), MessageBoxButtons.YesNo, IMMessageBoxImage.Question) != DialogResult.Yes)
          return;
      }
      else if ((option.OptionFlags & OptionFlags.Obsolete) == OptionFlags.Obsolete)
      {
        longList.RemoveAt(index);
        if (IMMessageBox.Show(LocalizationHolder.rm.GetString("PdmConfigurator_58"), string.Format(LocalizationHolder.rm.GetString("PdmConfigurator_59"), (object) option.OptionCaption), MessageBoxButtons.YesNo, IMMessageBoxImage.Question) != DialogResult.Yes)
          return;
      }
    }
    if (longList.Count == 0)
      return;
    this._options.AddOptions((IList<long>) longList);
    if (false)
      return;
    this.CollectCategories();
    this.FillTree(false);
    this.IsInternalChanged = true;
  }

  private void SetHandlers()
  {
    if (this.handlerDoDefaultView == null)
    {
      this.handlerDoDefaultView = new EventHandler(this.DoDefaultView);
      this.handlerDoThumbnailsView = new EventHandler(this.DoThumbnailsView);
    }
    if (ObjectOptionsEditor._thumbnailMode)
    {
      this.btnMode.Text = this.btnThumbnails.Text;
      this.btnMode.ToolTipText = this.btnThumbnails.ToolTipText;
      this.btnMode.ImageIndex = this.btnThumbnails.ImageIndex;
      this.btnMode.Click -= this.handlerDoThumbnailsView;
      this.btnMode.Click -= this.handlerDoDefaultView;
      this.btnMode.Click += this.handlerDoDefaultView;
      this.btnDefault.Checked = false;
      this.btnThumbnails.Checked = true;
    }
    else
    {
      this.btnMode.Text = this.btnDefault.Text;
      this.btnMode.ToolTipText = this.btnDefault.ToolTipText;
      this.btnMode.ImageIndex = this.btnDefault.ImageIndex;
      this.btnMode.Click -= this.handlerDoDefaultView;
      this.btnMode.Click -= this.handlerDoThumbnailsView;
      this.btnMode.Click += this.handlerDoThumbnailsView;
      this.btnDefault.Checked = true;
      this.btnThumbnails.Checked = false;
    }
  }

  private void ImportOptions(PdmAnalyzerFlags flag, params IDBTypedObjectID[] fromObjects)
  {
    IDBTypedObjectID[] dbTypedObjectIdArray = fromObjects.Length != 0 ? fromObjects : this.SelectObjects(flag);
    if (dbTypedObjectIdArray == null || dbTypedObjectIdArray.Length == 0)
      return;
    PdmAnalyzedOptionObjects analyzedOptionObjects = new PdmAnalyzedOptionObjects();
    for (int index = 0; index < dbTypedObjectIdArray.Length; ++index)
      analyzedOptionObjects.Add(new PdmAnalyzedOptionObject(analyzedOptionObjects, dbTypedObjectIdArray[index].ObjectID)
      {
        ObjectType = dbTypedObjectIdArray[index].ObjectType
      });
    List<long> excludedObjects = new List<long>();
    excludedObjects.Add(this._options.ObjectID);
    IList<long> excludedOptions = (IList<long>) new List<long>((IEnumerable<long>) this._options.Options);
    flag |= PdmAnalyzerFlags.IgnoreObsoleteOptions;
    PdmOptionsAnalyzerJobStatus analyzerJobStatus = ImportAnalyzeForm.Execute(analyzedOptionObjects, flag | PdmAnalyzerFlags.IgnoreObsoleteOptions, excludedObjects, excludedOptions);
    if (analyzerJobStatus == null || analyzerJobStatus.Progress == PdmOptionsAnalyzerJobProgress.Cancelled || analyzerJobStatus.Progress == PdmOptionsAnalyzerJobProgress.Working)
      return;
    if (analyzerJobStatus.Progress == PdmOptionsAnalyzerJobProgress.Error)
    {
      if (analyzerJobStatus.Exception == null)
        return;
      ExceptionHelper.ExceptionService.ShowException(analyzerJobStatus.Exception);
    }
    else
    {
      List<PdmAnalyzedOptionObject> objects = analyzerJobStatus.Items.ExtractObjects();
      if (objects.Count == 0)
      {
        int num = (int) IMMessageBox.Show(LocalizationHolder.rm.GetString("PdmConfigurator_64"), LocalizationHolder.rm.GetString("PdmConfigurator_65"), MessageBoxButtons.OK, IMMessageBoxImage.Information);
      }
      else
      {
        Intermech.Interfaces.PdmConfigurator.ImportOptions options = OptionsImportForm.Execute(objects, excludedOptions);
        if (options == null || options.Empty)
          return;
        this.DoAddOptions(options);
      }
    }
  }

  private IDBTypedObjectID[] SelectObjects(PdmAnalyzerFlags flag)
  {
    DescriptorCollection descriptors = new DescriptorCollection();
    List<int> typeList = new List<int>();
    foreach (IMSObjectType objectTypes in MetaDataHelper.GetObjectTypesList())
    {
      if (MetaDataHelper.IsPdmConfigurableObjectType(objectTypes.ObjectTypeID))
        typeList.Add(objectTypes.ObjectTypeID);
      else if (flag != PdmAnalyzerFlags.Default)
      {
        foreach (int relType in MetaDataHelper.GetApplicabilityRelationTypesID(objectTypes.ObjectTypeID))
        {
          if (MetaDataHelper.IsPdmConfigurableRelationType(relType))
            typeList.Add(objectTypes.ObjectTypeID);
        }
      }
    }
    List<int> enabledObjectTypes = MetaDataHelper.GetTopParentEnabledObjectTypes((IEnumerable<int>) typeList);
    for (int index = 0; index < enabledObjectTypes.Count; ++index)
      descriptors.Add((IDescriptor) new Intermech.Navigator.DBObjectTypes.Descriptor(enabledObjectTypes[index]));
    string description = LocalizationHolder.rm.GetString("PdmConfigurator_66");
    string caption = LocalizationHolder.rm.GetString("PdmConfigurator_67");
    if ((flag & PdmAnalyzerFlags.InCompositions) == PdmAnalyzerFlags.InCompositions)
    {
      description = LocalizationHolder.rm.GetString("PdmConfigurator_68");
      caption = LocalizationHolder.rm.GetString("PdmConfigurator_69");
    }
    if ((flag & PdmAnalyzerFlags.InCompositionsRecursive) == PdmAnalyzerFlags.InCompositionsRecursive)
    {
      description = LocalizationHolder.rm.GetString("PdmConfigurator_70");
      caption = LocalizationHolder.rm.GetString("PdmConfigurator_71");
    }
    SelectionOptions options = SelectionOptions.Default | SelectionOptions.ForceFilterObjectsByRule;
    return Intermech.Navigator.SelectionWindow.Select(caption, description, (IDescriptor) new Intermech.Navigator.CustomNode.Descriptor(Intermech.Navigator.Consts.CategoryCustomNode, 1, caption, descriptors), typeof (IDBTypedObjectID), options) as IDBTypedObjectID[];
  }

  private void iGridSelectRowCells(iGRow row, bool select)
  {
    if (row == null)
      return;
    for (int colIndex = 0; colIndex < row.Cells.Count; ++colIndex)
      row.Cells[colIndex].Selected = select;
  }

  private void FillAppGrid()
  {
    this.appEditor.LoadOptions((IPdmCriterion) this._applCollection, this.ParentKey, this._applsAccessRights);
  }

  private void FillContext()
  {
    this.contextEditor.AccessRights = this._contextAccessRights;
    this.contextEditor.Services = this.Services;
    this.contextEditor.ParentKey = this.ParentKey;
    this.contextEditor.Context = this._context;
  }

  private void FillCodeStructure()
  {
    this.codeEditor.LoadConfigurationCode(this._options, this._editingObject.F_OBJECT_TYPE, this._accessRights);
  }

  private void btnExcelReport_Click(object sender, EventArgs e)
  {
    if (!(ServicesManager.GetService(typeof (ISimpleExcelReports)) is ISimpleExcelReports service))
      return;
    object obj1 = (object) null;
    try
    {
      obj1 = service.GetExcelInstance((object) null, "");
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__0 = CallSite<Func<CallSite, ISimpleExcelReports, object, string, string, string, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "CreateWorkbook", (IEnumerable<Type>) null, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[6]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__0.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__0, service, obj1, "Опции", "Отчет", "", "");
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target1 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p3 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Item", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target2 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p2 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Worksheets", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__1.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__1, obj2);
      object obj4 = target2((CallSite) p2, obj3);
      object obj5 = target1((CallSite) p3, obj4, 1);
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ColumnWidth", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target3 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p5 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__5;
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Columns", (IEnumerable<Type>) null, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj6 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__4.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__4, obj5, "A:A");
      object obj7 = target3((CallSite) p5, obj6, 20);
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ColumnWidth", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target4 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__7.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p7 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__7;
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Columns", (IEnumerable<Type>) null, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj8 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__6.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__6, obj5, "B:B");
      object obj9 = target4((CallSite) p7, obj8, 30);
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ColumnWidth", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target5 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__9.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p9 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__9;
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Columns", (IEnumerable<Type>) null, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj10 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__8.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__8, obj5, "C:C");
      object obj11 = target5((CallSite) p9, obj10, 15);
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ColumnWidth", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target6 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__11.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p11 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__11;
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Columns", (IEnumerable<Type>) null, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj12 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__10.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__10, obj5, "D:D");
      object obj13 = target6((CallSite) p11, obj12, 50);
      int[] numArray1 = new int[2]{ 15851738, 14413794 };
      int num1 = 0;
      int num2 = 1;
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__13 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, int, int, string, object>>.Create(Binder.SetIndex(CSharpBinderFlags.None, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, int, string, object> target7 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__13.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, int, string, object>> p13 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__13;
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Cells", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj14 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__12.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__12, obj5);
      int num3 = num2;
      int num4 = num3 + 1;
      string caption = this._editingObject.CAPTION;
      object obj15 = target7((CallSite) p13, obj14, num3, 1, caption);
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__16 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Color", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target8 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__16.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p16 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__16;
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__15 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Interior", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target9 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__15.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p15 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__15;
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__14 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Range", (IEnumerable<Type>) null, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj16 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__14.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__14, obj5, "A1:D1");
      object obj17 = target9((CallSite) p15, obj16);
      object obj18 = target8((CallSite) p16, obj17, 15064022);
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__18 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__18 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Select", (IEnumerable<Type>) null, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object> target10 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__18.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object>> p18 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__18;
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__17 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__17 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Range", (IEnumerable<Type>) null, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj19 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__17.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__17, obj5, "A1:D1");
      target10((CallSite) p18, obj19);
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__20 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__20 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Merge", (IEnumerable<Type>) null, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object> target11 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__20.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object>> p20 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__20;
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__19 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Selection", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj20 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__19.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__19, obj1);
      target11((CallSite) p20, obj20);
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__22 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__22 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "HorizontalAlignment", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target12 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__22.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p22 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__22;
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__21 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Selection", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj21 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__21.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__21, obj1);
      object obj22 = target12((CallSite) p22, obj21, -4108);
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__24 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__24 = CallSite<Func<CallSite, object, int, int, string, object>>.Create(Binder.SetIndex(CSharpBinderFlags.None, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, int, string, object> target13 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__24.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, int, string, object>> p24 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__24;
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__23 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__23 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Cells", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj23 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__23.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__23, obj5);
      int num5 = num4;
      object obj24 = target13((CallSite) p24, obj23, num5, 1, "Название опции");
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__26 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__26 = CallSite<Func<CallSite, object, int, int, string, object>>.Create(Binder.SetIndex(CSharpBinderFlags.None, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, int, string, object> target14 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__26.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, int, string, object>> p26 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__26;
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__25 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__25 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Cells", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj25 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__25.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__25, obj5);
      int num6 = num4;
      object obj26 = target14((CallSite) p26, obj25, num6, 2, "Значение опции");
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__28 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__28 = CallSite<Func<CallSite, object, int, int, string, object>>.Create(Binder.SetIndex(CSharpBinderFlags.None, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, int, string, object> target15 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__28.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, int, string, object>> p28 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__28;
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__27 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__27 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Cells", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj27 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__27.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__27, obj5);
      int num7 = num4;
      object obj28 = target15((CallSite) p28, obj27, num7, 3, "Шифр опции");
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__30 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__30 = CallSite<Func<CallSite, object, int, int, string, object>>.Create(Binder.SetIndex(CSharpBinderFlags.None, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, int, string, object> target16 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__30.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, int, string, object>> p30 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__30;
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__29 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__29 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Cells", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj29 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__29.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__29, obj5);
      int num8 = num4;
      int num9 = num8 + 1;
      object obj30 = target16((CallSite) p30, obj29, num8, 4, "Примечание");
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__33 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__33 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Color", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target17 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__33.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p33 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__33;
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__32 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__32 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Interior", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target18 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__32.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p32 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__32;
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__31 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__31 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Range", (IEnumerable<Type>) null, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj31 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__31.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__31, obj5, "A2:D2");
      object obj32 = target18((CallSite) p32, obj31);
      int[] numArray2 = numArray1;
      int index1 = num1;
      int num10 = index1 + 1;
      int num11 = numArray2[index1];
      object obj33 = target17((CallSite) p33, obj32, num11);
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__35 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__35 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "HorizontalAlignment", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target19 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__35.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p35 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__35;
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__34 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__34 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Range", (IEnumerable<Type>) null, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj34 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__34.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__34, obj5, "A2");
      object obj35 = target19((CallSite) p35, obj34, -4108);
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__37 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__37 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "HorizontalAlignment", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target20 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__37.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p37 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__37;
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__36 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__36 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Range", (IEnumerable<Type>) null, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj36 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__36.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__36, obj5, "B2");
      object obj37 = target20((CallSite) p37, obj36, -4108);
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__39 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__39 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "HorizontalAlignment", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target21 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__39.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p39 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__39;
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__38 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__38 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Range", (IEnumerable<Type>) null, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj38 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__38.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__38, obj5, "C2");
      object obj39 = target21((CallSite) p39, obj38, -4108);
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__41 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__41 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "HorizontalAlignment", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target22 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__41.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p41 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__41;
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__40 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__40 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Range", (IEnumerable<Type>) null, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj40 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__40.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__40, obj5, "D2");
      object obj41 = target22((CallSite) p41, obj40, -4108);
      for (int index2 = 0; index2 < this._options.Options.Count; ++index2)
      {
        OptionHolder option = PdmConfiguratorCache.CacheFindOption(this._options.Options[index2]);
        // ISSUE: reference to a compiler-generated field
        if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__43 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__43 = CallSite<Func<CallSite, object, int, int, string, object>>.Create(Binder.SetIndex(CSharpBinderFlags.None, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, int, string, object> target23 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__43.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, int, string, object>> p43 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__43;
        // ISSUE: reference to a compiler-generated field
        if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__42 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__42 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Cells", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj42 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__42.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__42, obj5);
        int num12 = num9;
        string optionCaption = option.OptionCaption;
        object obj43 = target23((CallSite) p43, obj42, num12, 1, optionCaption);
        int num13 = num9;
        string defaultOptionValue = this._options.VisibleOptionValues.GetDefaultOptionValue(option.OptionGuid);
        int num14 = -1;
        for (int index3 = 0; index3 < option.OptionValues.Count; ++index3)
        {
          OptionValue optionValue = option.OptionValues[index3];
          if (this._options.VisibleOptionValues.GetVisibleOptionValue(option.OptionGuid, optionValue.ID))
          {
            // ISSUE: reference to a compiler-generated field
            if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__45 == null)
            {
              // ISSUE: reference to a compiler-generated field
              ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__45 = CallSite<Func<CallSite, object, int, int, string, object>>.Create(Binder.SetIndex(CSharpBinderFlags.None, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, int, int, string, object> target24 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__45.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, int, int, string, object>> p45 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__45;
            // ISSUE: reference to a compiler-generated field
            if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__44 == null)
            {
              // ISSUE: reference to a compiler-generated field
              ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__44 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Cells", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj44 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__44.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__44, obj5);
            int num15 = num9;
            string str = optionValue.Value;
            object obj45 = target24((CallSite) p45, obj44, num15, 2, str);
            // ISSUE: reference to a compiler-generated field
            if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__47 == null)
            {
              // ISSUE: reference to a compiler-generated field
              ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__47 = CallSite<Func<CallSite, object, int, int, string, object>>.Create(Binder.SetIndex(CSharpBinderFlags.None, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, int, int, string, object> target25 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__47.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, int, int, string, object>> p47 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__47;
            // ISSUE: reference to a compiler-generated field
            if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__46 == null)
            {
              // ISSUE: reference to a compiler-generated field
              ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__46 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Cells", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj46 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__46.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__46, obj5);
            int num16 = num9;
            string code = optionValue.Code;
            object obj47 = target25((CallSite) p47, obj46, num16, 3, code);
            // ISSUE: reference to a compiler-generated field
            if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__50 == null)
            {
              // ISSUE: reference to a compiler-generated field
              ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__50 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "HorizontalAlignment", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, int, object> target26 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__50.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, int, object>> p50 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__50;
            // ISSUE: reference to a compiler-generated field
            if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__49 == null)
            {
              // ISSUE: reference to a compiler-generated field
              ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__49 = CallSite<Func<CallSite, object, int, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, int, int, object> target27 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__49.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, int, int, object>> p49 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__49;
            // ISSUE: reference to a compiler-generated field
            if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__48 == null)
            {
              // ISSUE: reference to a compiler-generated field
              ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__48 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Cells", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj48 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__48.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__48, obj5);
            int num17 = num9;
            object obj49 = target27((CallSite) p49, obj48, num17, 3);
            object obj50 = target26((CallSite) p50, obj49, -4108);
            if (optionValue.ID == defaultOptionValue)
              num14 = num9;
            // ISSUE: reference to a compiler-generated field
            if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__52 == null)
            {
              // ISSUE: reference to a compiler-generated field
              ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__52 = CallSite<Func<CallSite, object, int, int, string, object>>.Create(Binder.SetIndex(CSharpBinderFlags.None, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, int, int, string, object> target28 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__52.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, int, int, string, object>> p52 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__52;
            // ISSUE: reference to a compiler-generated field
            if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__51 == null)
            {
              // ISSUE: reference to a compiler-generated field
              ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__51 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Cells", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj51 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__51.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__51, obj5);
            int num18 = num9++;
            string description = optionValue.Description;
            object obj52 = target28((CallSite) p52, obj51, num18, 4, description);
          }
        }
        if (num13 == num9)
          ++num9;
        // ISSUE: reference to a compiler-generated field
        if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__54 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__54 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Select", (IEnumerable<Type>) null, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, object> target29 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__54.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, object>> p54 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__54;
        // ISSUE: reference to a compiler-generated field
        if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__53 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__53 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Range", (IEnumerable<Type>) null, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj53 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__53.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__53, obj5, $"A{num13}:A{num9 - 1}");
        target29((CallSite) p54, obj53);
        // ISSUE: reference to a compiler-generated field
        if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__56 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__56 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Merge", (IEnumerable<Type>) null, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, object> target30 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__56.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, object>> p56 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__56;
        // ISSUE: reference to a compiler-generated field
        if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__55 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__55 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Selection", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj54 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__55.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__55, obj1);
        target30((CallSite) p56, obj54);
        // ISSUE: reference to a compiler-generated field
        if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__58 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__58 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "HorizontalAlignment", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target31 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__58.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p58 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__58;
        // ISSUE: reference to a compiler-generated field
        if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__57 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__57 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Selection", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj55 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__57.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__57, obj1);
        object obj56 = target31((CallSite) p58, obj55, -4108);
        // ISSUE: reference to a compiler-generated field
        if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__60 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__60 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "VerticalAlignment", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target32 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__60.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p60 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__60;
        // ISSUE: reference to a compiler-generated field
        if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__59 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__59 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Selection", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj57 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__59.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__59, obj1);
        object obj58 = target32((CallSite) p60, obj57, -4108);
        // ISSUE: reference to a compiler-generated field
        if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__62 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__62 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "WrapText", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool, object> target33 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__62.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool, object>> p62 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__62;
        // ISSUE: reference to a compiler-generated field
        if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__61 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__61 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Selection", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj59 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__61.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__61, obj1);
        object obj60 = target33((CallSite) p62, obj59, true);
        // ISSUE: reference to a compiler-generated field
        if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__65 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__65 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Color", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target34 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__65.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p65 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__65;
        // ISSUE: reference to a compiler-generated field
        if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__64 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__64 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Interior", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target35 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__64.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p64 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__64;
        // ISSUE: reference to a compiler-generated field
        if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__63 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__63 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Range", (IEnumerable<Type>) null, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj61 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__63.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__63, obj5, $"A{num13}:D{num9 - 1}");
        object obj62 = target35((CallSite) p64, obj61);
        int num19 = numArray1[num10++ % 2];
        object obj63 = target34((CallSite) p65, obj62, num19);
        if (num14 != -1)
        {
          // ISSUE: reference to a compiler-generated field
          if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__67 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__67 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Interior", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, object> target36 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__67.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, object>> p67 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__67;
          // ISSUE: reference to a compiler-generated field
          if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__66 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__66 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Range", (IEnumerable<Type>) null, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj64 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__66.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__66, obj5, $"C{num14}");
          object obj65 = target36((CallSite) p67, obj64);
          // ISSUE: reference to a compiler-generated field
          if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__68 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__68 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Color", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj66 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__68.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__68, obj65, 5296274);
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__70 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__70 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Select", (IEnumerable<Type>) null, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object> target37 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__70.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object>> p70 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__70;
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__69 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__69 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Range", (IEnumerable<Type>) null, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj67 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__69.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__69, obj5, $"A1:A{num9 - 1}");
      target37((CallSite) p70, obj67);
      for (int index4 = 7; index4 < 13; ++index4)
      {
        // ISSUE: reference to a compiler-generated field
        if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__72 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__72 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Borders", (IEnumerable<Type>) null, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target38 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__72.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p72 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__72;
        // ISSUE: reference to a compiler-generated field
        if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__71 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__71 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Selection", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj68 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__71.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__71, obj1);
        int num20 = index4;
        object obj69 = target38((CallSite) p72, obj68, num20);
        // ISSUE: reference to a compiler-generated field
        if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__73 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__73 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "LineStyle", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj70 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__73.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__73, obj69, 1);
        // ISSUE: reference to a compiler-generated field
        if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__74 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__74 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ColorIndex", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj71 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__74.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__74, obj69, 0);
        // ISSUE: reference to a compiler-generated field
        if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__75 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__75 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Weight", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj72 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__75.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__75, obj69, 2);
      }
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__77 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__77 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Select", (IEnumerable<Type>) null, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object> target39 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__77.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object>> p77 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__77;
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__76 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__76 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Range", (IEnumerable<Type>) null, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj73 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__76.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__76, obj5, "A1");
      target39((CallSite) p77, obj73);
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__78 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__78 = CallSite<Action<CallSite, ISimpleExcelReports, object, bool>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "SetVisible", (IEnumerable<Type>) null, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__78.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__78, service, obj1, true);
      ForegroundWindowHelper.Default.AllowActionToAnyProcess();
      ForegroundWindowHelper foregroundWindowHelper = ForegroundWindowHelper.Default;
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__80 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__80 = CallSite<Func<CallSite, Type, object, IntPtr>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, IntPtr> target40 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__80.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, IntPtr>> p80 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__80;
      Type type = typeof (IntPtr);
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__79 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__79 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "hwnd", typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj74 = ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__79.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__79, obj1);
      IntPtr windowHandle = target40((CallSite) p80, type, obj74);
      foregroundWindowHelper.TrySetWindow(windowHandle);
    }
    finally
    {
      // ISSUE: reference to a compiler-generated field
      if (ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__81 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__81 = CallSite<Action<CallSite, ISimpleExcelReports, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "ReleaseExcelInstance", (IEnumerable<Type>) null, typeof (ObjectOptionsEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__81.Target((CallSite) ObjectOptionsEditor.\u003C\u003Eo__190.\u003C\u003Ep__81, service, obj1);
    }
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing)
    {
      if (ServicesManager.GetService(typeof (BarManager)) is BarManager service)
      {
        this.toolBarTree.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
        this.toolBarGrid.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
        this.menuBarTree.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
        this.menuBarGridValues.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
        service.RendererChanged -= new EventHandler(this.BarManager_RendererChanged);
      }
      if (this._notifications != null && this._notifyHandler != null)
      {
        this._notifications.Unsubscribe(this._notifyHandler);
        this._notifyHandler = (NotificationEventHandler) null;
        this._notifications = (INotificationService) null;
      }
    }
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ObjectOptionsEditor));
    this.headerControl = new HeaderControl();
    this.imagesTabs = new ImageList(this.components);
    this.toolTip = new ToolTip(this.components);
    this.menuBarTree = new MenuBar();
    this.imagesToolbars = new ImageList(this.components);
    this.contextMenuBarTree = new ContextMenuBarItem();
    this.mnpAddOptions = new MenuButtonItem();
    this.mnpDeleteOptions = new MenuButtonItem();
    this.mnpObligatoryOption = new MenuButtonItem();
    this.mnpDefaultImport = new MenuButtonItem();
    this.mnpInCompositions = new MenuButtonItem();
    this.mnpRecursiveImport = new MenuButtonItem();
    this.mnpCard = new MenuButtonItem();
    this.mnpOpenInNewWindow = new MenuButtonItem();
    this.mnpExpand = new MenuButtonItem();
    this.mnpCollapse = new MenuButtonItem();
    this.panelTree = new Panel();
    this.treeOptions = new Intermech.VirtualTreeView.VirtualTreeView();
    this.columnMain = new Column();
    this.toolBarTree = new Intermech.Bars.ToolBar();
    this.btnAddOptions = new ButtonItem();
    this.btnDeleteOptions = new ButtonItem();
    this.btnImport = new DropDownMenuItem();
    this.btnInObjectComposition = new MenuButtonItem();
    this.btnObjectRecursiveImport = new MenuButtonItem();
    this.btnDefaultImport = new MenuButtonItem();
    this.btnInCompositions = new MenuButtonItem();
    this.btnRecursiveImport = new MenuButtonItem();
    this.btnCard = new ButtonItem();
    this.btnOpenInNewWindow = new ButtonItem();
    this.btnExpand = new ButtonItem();
    this.btnExcelReport = new ButtonItem();
    this.btnCollapse = new ButtonItem();
    this.panelHint = new Panel();
    this.labelWarning = new Label();
    this.pictureHint = new PictureBox();
    this.panelGrid = new Panel();
    this._splitContainer = new SplitContainer();
    this.gridValues = new iGrid();
    this.tabs = new PageControl();
    this.tabPageIncomps = new Intermech.Docking.TabPage();
    this.tabPageLinked = new Intermech.Docking.TabPage();
    this.tabPagePicture = new Intermech.Docking.TabPage();
    this.picture = new PictureBox();
    this.toolBarGrid = new Intermech.Bars.ToolBar();
    this.btAdvPanels = new ButtonItem();
    this.btnMode = new DropDownMenuItem();
    this.btnDefault = new MenuButtonItem();
    this.btnThumbnails = new MenuButtonItem();
    this.btnHideInvisibles = new ButtonItem();
    this.btnGridExpand = new ButtonItem();
    this.btnGridCollapse = new ButtonItem();
    this.cbObligatoryOption = new ButtonItem();
    this.lbWarning = new LabelItem();
    this.splitter = new CollapsibleSplitter();
    this.menuBarGridValues = new MenuBar();
    this.contextMenuBarGrid = new ContextMenuBarItem();
    this.mnpDefaultValue = new MenuButtonItem();
    this.mnpGridExpand = new MenuButtonItem();
    this.mnpGridCollapse = new MenuButtonItem();
    this.pagesMain = new PageControl();
    this.pageObjectOptions = new Intermech.Docking.TabPage();
    this.splitterV = new CollapsibleSplitter();
    this.pageAppls = new Intermech.Docking.TabPage();
    this.labelAppls = new Label();
    this.pageContext = new Intermech.Docking.TabPage();
    this.lbPageContext = new Label();
    this.pageCode = new Intermech.Docking.TabPage();
    this.incompEditor = new IncompatibilityEditor();
    this.linkedEditor = new LinkedOptionsEditor();
    this.appEditor = new AppConditionsEditor();
    this.contextEditor = new ObjectContextEditor();
    this.codeEditor = new ConfigurationCodeEditor();
    this.panelTree.SuspendLayout();
    this.treeOptions.BeginInit();
    this.panelHint.SuspendLayout();
    ((ISupportInitialize) this.pictureHint).BeginInit();
    this.panelGrid.SuspendLayout();
    this._splitContainer.BeginInit();
    this._splitContainer.Panel1.SuspendLayout();
    this._splitContainer.Panel2.SuspendLayout();
    this._splitContainer.SuspendLayout();
    ((ISupportInitialize) this.gridValues).BeginInit();
    this.tabs.SuspendLayout();
    this.tabPageIncomps.SuspendLayout();
    this.tabPageLinked.SuspendLayout();
    this.tabPagePicture.SuspendLayout();
    ((ISupportInitialize) this.picture).BeginInit();
    this.pagesMain.SuspendLayout();
    this.pageObjectOptions.SuspendLayout();
    this.pageAppls.SuspendLayout();
    this.pageContext.SuspendLayout();
    this.pageCode.SuspendLayout();
    this.SuspendLayout();
    this.headerControl.BackColor = SystemColors.Control;
    componentResourceManager.ApplyResources((object) this.headerControl, "headerControl");
    this.headerControl.ForeColor = SystemColors.ControlText;
    this.headerControl.HeaderFont = new Font("Tahoma", 12f, FontStyle.Bold);
    this.headerControl.Name = "headerControl";
    this.imagesTabs.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imagesTabs.ImageStream");
    this.imagesTabs.TransparentColor = Color.Transparent;
    this.imagesTabs.Images.SetKeyName(0, "gears_stop.png");
    this.imagesTabs.Images.SetKeyName(1, "image.ico");
    this.imagesTabs.Images.SetKeyName(2, "gears_run.png");
    this.imagesTabs.Images.SetKeyName(3, "gear_ok.png");
    this.imagesTabs.Images.SetKeyName(4, "gears.png");
    this.imagesTabs.Images.SetKeyName(5, "gear_forbidden.png");
    this.imagesTabs.Images.SetKeyName(6, "Опции.ico");
    this.imagesTabs.Images.SetKeyName(7, "settings.ico");
    this.imagesTabs.Images.SetKeyName(8, "Опции.ico");
    componentResourceManager.ApplyResources((object) this.menuBarTree, "menuBarTree");
    this.menuBarTree.Guid = new Guid("0909a734-928b-4c5d-9a6d-05be64690c06");
    this.menuBarTree.Hidden = false;
    this.menuBarTree.ImageList = this.imagesToolbars;
    this.menuBarTree.Items.AddRange(new ToolbarItemBase[1]
    {
      (ToolbarItemBase) this.contextMenuBarTree
    });
    this.menuBarTree.Name = "menuBarTree";
    this.menuBarTree.OwnerForm = (Form) null;
    this.imagesToolbars.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imagesToolbars.ImageStream");
    this.imagesToolbars.TransparentColor = Color.Transparent;
    this.imagesToolbars.Images.SetKeyName(0, "gear_add.png");
    this.imagesToolbars.Images.SetKeyName(1, "gear_forbidden.png");
    this.imagesToolbars.Images.SetKeyName(2, "Collapse.ico");
    this.imagesToolbars.Images.SetKeyName(3, "Expand.ico");
    this.imagesToolbars.Images.SetKeyName(4, "EventLog2.ico");
    this.imagesToolbars.Images.SetKeyName(5, "image.ico");
    this.imagesToolbars.Images.SetKeyName(6, "EventLogFiltered2.ico");
    this.imagesToolbars.Images.SetKeyName(7, "cb_unckecked.ico");
    this.imagesToolbars.Images.SetKeyName(8, "cb_checked.ico");
    this.imagesToolbars.Images.SetKeyName(9, "garbage.png");
    this.imagesToolbars.Images.SetKeyName(10, "recycle.png");
    this.imagesToolbars.Images.SetKeyName(11, "window_split_ver.png");
    this.imagesToolbars.Images.SetKeyName(12, "gear_find.png");
    this.imagesToolbars.Images.SetKeyName(13, "Комплектации.ico");
    this.imagesToolbars.Images.SetKeyName(14, "Объекты конфигуратора составов.ico");
    this.imagesToolbars.Images.SetKeyName(15, "Развернутый состав.ico");
    this.imagesToolbars.Images.SetKeyName(16 /*0x10*/, "gear.png");
    this.imagesToolbars.Images.SetKeyName(17, "rb_unchecked.ico");
    this.imagesToolbars.Images.SetKeyName(18, "rb_checked.ico");
    this.imagesToolbars.Images.SetKeyName(19, "ball_green_plus.ico");
    this.imagesToolbars.Images.SetKeyName(20, "delete.ico");
    this.imagesToolbars.Images.SetKeyName(21, "gear_add.png");
    this.imagesToolbars.Images.SetKeyName(22, "export_excel.png");
    componentResourceManager.ApplyResources((object) this.contextMenuBarTree, "contextMenuBarTree");
    this.contextMenuBarTree.Items.AddRange(new ToolbarItemBase[10]
    {
      (ToolbarItemBase) this.mnpAddOptions,
      (ToolbarItemBase) this.mnpDeleteOptions,
      (ToolbarItemBase) this.mnpObligatoryOption,
      (ToolbarItemBase) this.mnpDefaultImport,
      (ToolbarItemBase) this.mnpInCompositions,
      (ToolbarItemBase) this.mnpRecursiveImport,
      (ToolbarItemBase) this.mnpCard,
      (ToolbarItemBase) this.mnpOpenInNewWindow,
      (ToolbarItemBase) this.mnpExpand,
      (ToolbarItemBase) this.mnpCollapse
    });
    this.contextMenuBarTree.ShowText = true;
    componentResourceManager.ApplyResources((object) this.mnpAddOptions, "mnpAddOptions");
    this.mnpAddOptions.ImageIndex = 0;
    this.mnpAddOptions.ShowText = true;
    this.mnpAddOptions.Click += new EventHandler(this.DoAddOptions);
    componentResourceManager.ApplyResources((object) this.mnpDeleteOptions, "mnpDeleteOptions");
    this.mnpDeleteOptions.ImageIndex = 1;
    this.mnpDeleteOptions.ShowText = true;
    this.mnpDeleteOptions.Click += new EventHandler(this.DoDeleteOptions);
    this.mnpObligatoryOption.AutoToggle = AutoToggleType.Single;
    this.mnpObligatoryOption.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.mnpObligatoryOption, "mnpObligatoryOption");
    this.mnpObligatoryOption.ImageIndex = 7;
    this.mnpObligatoryOption.ShowText = true;
    this.mnpObligatoryOption.Click += new EventHandler(this.DoSwitchOptionObligatory);
    this.mnpDefaultImport.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.mnpDefaultImport, "mnpDefaultImport");
    this.mnpDefaultImport.ImageIndex = 13;
    this.mnpDefaultImport.ShowText = true;
    this.mnpDefaultImport.Click += new EventHandler(this.btnDefaultImport_Click);
    componentResourceManager.ApplyResources((object) this.mnpInCompositions, "mnpInCompositions");
    this.mnpInCompositions.ImageIndex = 14;
    this.mnpInCompositions.ShowText = true;
    this.mnpInCompositions.Click += new EventHandler(this.btnInCompositions_Click);
    componentResourceManager.ApplyResources((object) this.mnpRecursiveImport, "mnpRecursiveImport");
    this.mnpRecursiveImport.ImageIndex = 15;
    this.mnpRecursiveImport.ShowText = true;
    this.mnpRecursiveImport.Click += new EventHandler(this.btnRecursiveImport_Click);
    this.mnpCard.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.mnpCard, "mnpCard");
    this.mnpCard.ShowText = true;
    this.mnpCard.Click += new EventHandler(this.DoCard);
    componentResourceManager.ApplyResources((object) this.mnpOpenInNewWindow, "mnpOpenInNewWindow");
    this.mnpOpenInNewWindow.ShowText = true;
    this.mnpOpenInNewWindow.Click += new EventHandler(this.DoOpen);
    this.mnpExpand.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.mnpExpand, "mnpExpand");
    this.mnpExpand.ImageIndex = 3;
    this.mnpExpand.ShowText = true;
    this.mnpExpand.Click += new EventHandler(this.DoExpand);
    componentResourceManager.ApplyResources((object) this.mnpCollapse, "mnpCollapse");
    this.mnpCollapse.ImageIndex = 2;
    this.mnpCollapse.ShowText = true;
    this.mnpCollapse.Click += new EventHandler(this.DoCollapse);
    this.panelTree.Controls.Add((Control) this.treeOptions);
    this.panelTree.Controls.Add((Control) this.toolBarTree);
    this.panelTree.Controls.Add((Control) this.panelHint);
    componentResourceManager.ApplyResources((object) this.panelTree, "panelTree");
    this.panelTree.Name = "panelTree";
    this.treeOptions.AllowDrop = true;
    this.treeOptions.AllowUserPinnedColumns = false;
    this.treeOptions.AutoFitColumns = true;
    this.treeOptions.BackColor = SystemColors.Control;
    this.treeOptions.Columns.Add(this.columnMain);
    this.treeOptions.DisableHeaderContextMenu = true;
    componentResourceManager.ApplyResources((object) this.treeOptions, "treeOptions");
    this.treeOptions.EnableRowCaching = false;
    this.treeOptions.HeaderStyle.HorzAlignment = (StringAlignment) componentResourceManager.GetObject("treeOptions.HeaderStyle.HorzAlignment");
    this.treeOptions.ImageList = (ImageList) null;
    this.treeOptions.LineStyle = LineStyle.Dot;
    this.treeOptions.MainColumn = this.columnMain;
    this.treeOptions.Name = "treeOptions";
    this.treeOptions.RowEvenStyle.WordWrap = (bool) componentResourceManager.GetObject("treeOptions.RowEvenStyle.WordWrap");
    this.treeOptions.RowOddStyle.BackColor = Color.WhiteSmoke;
    this.treeOptions.RowOddStyle.WordWrap = (bool) componentResourceManager.GetObject("treeOptions.RowOddStyle.WordWrap");
    this.treeOptions.RowSelectedStyle.WordWrap = (bool) componentResourceManager.GetObject("treeOptions.RowSelectedStyle.WordWrap");
    this.treeOptions.RowStyle.BorderColor = SystemColors.Control;
    this.treeOptions.RowStyle.BorderStyle = Border3DStyle.Flat;
    this.treeOptions.RowStyle.BorderWidth = 0;
    this.treeOptions.RowStyle.WordWrap = (bool) componentResourceManager.GetObject("treeOptions.RowStyle.WordWrap");
    this.treeOptions.SelectBeforeEdit = true;
    this.treeOptions.SuppressErrorMessages = true;
    this.treeOptions.ShowContextMenu += new MouseEventHandler(this.treeOptions_ShowContextMenu);
    this.treeOptions.GetCellData += new GetCellDataHandler(this.treeOptions_GetCellData);
    this.treeOptions.GetChildren += new GetChildrenHandler(this.treeOptions_GetChildren);
    this.treeOptions.GetRowData += new GetRowDataHandler(this.treeOptions_GetRowData);
    this.treeOptions.GetRowDropEffect += new GetRowDropEffectHandler(this.treeOptions_GetRowDropEffect);
    this.treeOptions.SelectionChanged += new EventHandler(this.treeOptions_SelectionChanged);
    this.treeOptions.DragDrop += new DragEventHandler(this.treeOptions_DragDrop);
    this.treeOptions.DragEnter += new DragEventHandler(this.treeOptions_DragEnter);
    this.treeOptions.DragOver += new DragEventHandler(this.treeOptions_DragOver);
    this.columnMain.AutoSizePolicy = ColumnAutoSizePolicy.AutoSize;
    componentResourceManager.ApplyResources((object) this.columnMain, "columnMain");
    this.columnMain.HeaderStyle.HorzAlignment = (StringAlignment) componentResourceManager.GetObject("columnMain.HeaderStyle.HorzAlignment");
    this.columnMain.Movable = false;
    this.columnMain.Name = "columnMain";
    this.columnMain.Sortable = false;
    this.toolBarTree.AddRemoveButtonsVisible = false;
    this.toolBarTree.AllowHorizontalDock = false;
    this.toolBarTree.DockLine = 3;
    this.toolBarTree.DrawActionsButton = false;
    this.toolBarTree.FullMenus = true;
    this.toolBarTree.Guid = new Guid("ba855ba6-35ae-4775-b979-b76ac70a54e0");
    this.toolBarTree.Hidden = false;
    this.toolBarTree.ImageList = this.imagesToolbars;
    this.toolBarTree.Items.AddRange(new ToolbarItemBase[8]
    {
      (ToolbarItemBase) this.btnAddOptions,
      (ToolbarItemBase) this.btnDeleteOptions,
      (ToolbarItemBase) this.btnImport,
      (ToolbarItemBase) this.btnCard,
      (ToolbarItemBase) this.btnOpenInNewWindow,
      (ToolbarItemBase) this.btnExpand,
      (ToolbarItemBase) this.btnExcelReport,
      (ToolbarItemBase) this.btnCollapse
    });
    componentResourceManager.ApplyResources((object) this.toolBarTree, "toolBarTree");
    this.toolBarTree.MinimumFloatingSize = new Size(250, 30);
    this.toolBarTree.Name = "toolBarTree";
    this.toolBarTree.Overflow = ToolBarOverflow.Wrap;
    this.toolBarTree.Stretch = true;
    this.toolBarTree.Tearable = false;
    componentResourceManager.ApplyResources((object) this.btnAddOptions, "btnAddOptions");
    this.btnAddOptions.ImageIndex = 0;
    this.btnAddOptions.Click += new EventHandler(this.DoAddOptions);
    componentResourceManager.ApplyResources((object) this.btnDeleteOptions, "btnDeleteOptions");
    this.btnDeleteOptions.ImageIndex = 1;
    this.btnDeleteOptions.Click += new EventHandler(this.DoDeleteOptions);
    this.btnImport.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.btnImport, "btnImport");
    this.btnImport.ImageIndex = 12;
    this.btnImport.Items.AddRange(new ToolbarItemBase[5]
    {
      (ToolbarItemBase) this.btnInObjectComposition,
      (ToolbarItemBase) this.btnObjectRecursiveImport,
      (ToolbarItemBase) this.btnDefaultImport,
      (ToolbarItemBase) this.btnInCompositions,
      (ToolbarItemBase) this.btnRecursiveImport
    });
    this.btnImport.ShowText = true;
    this.btnImport.Click += new EventHandler(this.btnDefaultImport_Click);
    componentResourceManager.ApplyResources((object) this.btnInObjectComposition, "btnInObjectComposition");
    this.btnInObjectComposition.ImageIndex = 14;
    this.btnInObjectComposition.ShowText = true;
    this.btnInObjectComposition.Click += new EventHandler(this.btnInObjectComposition_Click);
    componentResourceManager.ApplyResources((object) this.btnObjectRecursiveImport, "btnObjectRecursiveImport");
    this.btnObjectRecursiveImport.ImageIndex = 15;
    this.btnObjectRecursiveImport.ShowText = true;
    this.btnObjectRecursiveImport.Click += new EventHandler(this.btnObjectRecursiveImport_Click);
    this.btnDefaultImport.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.btnDefaultImport, "btnDefaultImport");
    this.btnDefaultImport.ImageIndex = 13;
    this.btnDefaultImport.ShowText = true;
    this.btnDefaultImport.Click += new EventHandler(this.btnDefaultImport_Click);
    componentResourceManager.ApplyResources((object) this.btnInCompositions, "btnInCompositions");
    this.btnInCompositions.ImageIndex = 14;
    this.btnInCompositions.ShowText = true;
    this.btnInCompositions.Click += new EventHandler(this.btnInCompositions_Click);
    componentResourceManager.ApplyResources((object) this.btnRecursiveImport, "btnRecursiveImport");
    this.btnRecursiveImport.ImageIndex = 15;
    this.btnRecursiveImport.ShowText = true;
    this.btnRecursiveImport.Click += new EventHandler(this.btnRecursiveImport_Click);
    this.btnCard.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.btnCard, "btnCard");
    this.btnCard.Click += new EventHandler(this.DoCard);
    componentResourceManager.ApplyResources((object) this.btnOpenInNewWindow, "btnOpenInNewWindow");
    this.btnOpenInNewWindow.Click += new EventHandler(this.DoOpen);
    this.btnExpand.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.btnExpand, "btnExpand");
    this.btnExpand.ImageIndex = 3;
    this.btnExpand.Click += new EventHandler(this.DoExpand);
    componentResourceManager.ApplyResources((object) this.btnExcelReport, "btnExcelReport");
    this.btnExcelReport.ImageIndex = 22;
    this.btnExcelReport.Click += new EventHandler(this.btnExcelReport_Click);
    componentResourceManager.ApplyResources((object) this.btnCollapse, "btnCollapse");
    this.btnCollapse.ImageIndex = 2;
    this.btnCollapse.Click += new EventHandler(this.DoCollapse);
    this.panelHint.BorderStyle = BorderStyle.Fixed3D;
    this.panelHint.Controls.Add((Control) this.labelWarning);
    this.panelHint.Controls.Add((Control) this.pictureHint);
    componentResourceManager.ApplyResources((object) this.panelHint, "panelHint");
    this.panelHint.Name = "panelHint";
    componentResourceManager.ApplyResources((object) this.labelWarning, "labelWarning");
    this.labelWarning.Name = "labelWarning";
    componentResourceManager.ApplyResources((object) this.pictureHint, "pictureHint");
    this.pictureHint.Name = "pictureHint";
    this.pictureHint.TabStop = false;
    this.panelGrid.Controls.Add((Control) this._splitContainer);
    this.panelGrid.Controls.Add((Control) this.toolBarGrid);
    this.panelGrid.Controls.Add((Control) this.splitter);
    this.panelGrid.Controls.Add((Control) this.menuBarGridValues);
    componentResourceManager.ApplyResources((object) this.panelGrid, "panelGrid");
    this.panelGrid.Name = "panelGrid";
    componentResourceManager.ApplyResources((object) this._splitContainer, "_splitContainer");
    this._splitContainer.Name = "_splitContainer";
    this._splitContainer.Panel1.Controls.Add((Control) this.gridValues);
    this._splitContainer.Panel2.Controls.Add((Control) this.tabs);
    this.gridValues.DefaultAutoGroupRow.Height = 20;
    this.gridValues.DefaultRow.Height = (int) componentResourceManager.GetObject("resource.Height");
    this.gridValues.DefaultRow.NormalCellHeight = (int) componentResourceManager.GetObject("resource.NormalCellHeight");
    componentResourceManager.ApplyResources((object) this.gridValues, "gridValues");
    this.gridValues.GridLines.GroupRows = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this.gridValues.GridLines.Horizontal = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this.gridValues.GridLines.HorizontalExtended = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this.gridValues.GridLines.HorizontalLastRow = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this.gridValues.GridLines.Vertical = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this.gridValues.GridLines.VerticalExtended = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this.gridValues.GridLines.VerticalLastCol = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this.gridValues.GroupBox.Text = componentResourceManager.GetString("gridValues.GroupBox.Text");
    this.gridValues.Header.Height = (int) componentResourceManager.GetObject("gridValues.Header.Height");
    this.gridValues.HighlightBackColorNoFocus = SystemColors.Highlight;
    this.gridValues.HotTracking = false;
    this.gridValues.Name = "gridValues";
    this.menuBarGridValues.SetPopupMenu((Control) this.gridValues, (MenuBarItem) this.contextMenuBarGrid);
    this.gridValues.PressedMouseMoveMode = iGPressedMouseMoveMode.Normal;
    this.gridValues.ProcessTab = false;
    this.gridValues.RowMode = true;
    this.gridValues.RowModeHasCurCell = true;
    this.gridValues.ShowControlsInAllCells = false;
    this.gridValues.SilentValidation = true;
    this.gridValues.CellMouseDown += new iGCellMouseDownEventHandler(this.DoCellMouseDown);
    this.gridValues.CustomDrawCellForeground += new iGCustomDrawCellEventHandler(this.gridValues_CustomDrawCellForeground);
    this.gridValues.CustomDrawCellGetHeight += new iGCustomDrawCellGetHeightEventHandler(this.gridValues_CustomDrawCellGetHeight);
    this.gridValues.ColWidthEndChange += new iGColWidthEventHandler(this.DoChangeColWidth);
    this.gridValues.ColWidthChanging += new iGColWidthEventHandler(this.gridValues_ColWidthChanging);
    this.gridValues.SelectionChanged += new EventHandler(this.gridValues_SelectionChanged);
    this.gridValues.BeforeCommitEdit += new iGBeforeCommitEditEventHandler(this.gridValues_BeforeCommitEdit);
    this.gridValues.AfterCommitEdit += new iGAfterCommitEditEventHandler(this.gridValues_AfterCommitEdit);
    this.gridValues.Resize += new EventHandler(this.gridValues_Resize);
    this.tabs.CausesValidation = false;
    this.tabs.Controls.Add((Control) this.tabPageIncomps);
    this.tabs.Controls.Add((Control) this.tabPageLinked);
    this.tabs.Controls.Add((Control) this.tabPagePicture);
    componentResourceManager.ApplyResources((object) this.tabs, "tabs");
    this.tabs.Flat = false;
    this.tabs.ImageList = this.imagesTabs;
    this.tabs.Name = "tabs";
    this.tabs.SelectedPageChanged += new EventHandler(this.tabs_SelectedIndexChanged);
    this.tabs.SelectedPageChanging += new PageControlCancelEventHandler(this.tabs_SelectedPageChanging);
    this.tabPageIncomps.Controls.Add((Control) this.incompEditor);
    this.tabPageIncomps.Index = 0;
    componentResourceManager.ApplyResources((object) this.tabPageIncomps, "tabPageIncomps");
    this.tabPageIncomps.Name = "tabPageIncomps";
    this.tabPageIncomps.TabImage = (Image) componentResourceManager.GetObject("tabPageIncomps.TabImage");
    this.tabPageIncomps.TabImageIndex = 0;
    this.tabPageLinked.Controls.Add((Control) this.linkedEditor);
    this.tabPageLinked.Index = 1;
    componentResourceManager.ApplyResources((object) this.tabPageLinked, "tabPageLinked");
    this.tabPageLinked.Name = "tabPageLinked";
    this.tabPageLinked.TabImage = (Image) componentResourceManager.GetObject("tabPageLinked.TabImage");
    this.tabPageLinked.TabImageIndex = 4;
    this.tabPagePicture.Controls.Add((Control) this.picture);
    this.tabPagePicture.Index = 2;
    componentResourceManager.ApplyResources((object) this.tabPagePicture, "tabPagePicture");
    this.tabPagePicture.Name = "tabPagePicture";
    this.tabPagePicture.TabImage = (Image) componentResourceManager.GetObject("tabPagePicture.TabImage");
    this.tabPagePicture.TabImageIndex = 1;
    componentResourceManager.ApplyResources((object) this.picture, "picture");
    this.picture.Name = "picture";
    this.picture.TabStop = false;
    this.toolBarGrid.AddRemoveButtonsVisible = false;
    this.toolBarGrid.AllowHorizontalDock = false;
    this.toolBarGrid.DockLine = 3;
    this.toolBarGrid.DrawActionsButton = false;
    this.toolBarGrid.FullMenus = true;
    this.toolBarGrid.Guid = new Guid("ba855ba6-35ae-4775-b979-b76ac70a54e0");
    this.toolBarGrid.Hidden = false;
    this.toolBarGrid.ImageList = this.imagesToolbars;
    this.toolBarGrid.Items.AddRange(new ToolbarItemBase[7]
    {
      (ToolbarItemBase) this.btAdvPanels,
      (ToolbarItemBase) this.btnMode,
      (ToolbarItemBase) this.btnHideInvisibles,
      (ToolbarItemBase) this.btnGridExpand,
      (ToolbarItemBase) this.btnGridCollapse,
      (ToolbarItemBase) this.cbObligatoryOption,
      (ToolbarItemBase) this.lbWarning
    });
    componentResourceManager.ApplyResources((object) this.toolBarGrid, "toolBarGrid");
    this.toolBarGrid.MinimumFloatingSize = new Size(250, 30);
    this.toolBarGrid.Name = "toolBarGrid";
    this.toolBarGrid.Overflow = ToolBarOverflow.Wrap;
    this.toolBarGrid.Stretch = true;
    this.toolBarGrid.Tearable = false;
    this.btAdvPanels.AutoToggle = AutoToggleType.Single;
    componentResourceManager.ApplyResources((object) this.btAdvPanels, "btAdvPanels");
    this.btAdvPanels.ImageIndex = 11;
    this.btAdvPanels.ShowText = true;
    this.btAdvPanels.Click += new EventHandler(this.DoAdvPanelsShow);
    this.btnMode.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.btnMode, "btnMode");
    this.btnMode.ImageIndex = 4;
    this.btnMode.Items.AddRange(new ToolbarItemBase[2]
    {
      (ToolbarItemBase) this.btnDefault,
      (ToolbarItemBase) this.btnThumbnails
    });
    this.btnMode.MenuImageList = this.imagesToolbars;
    this.btnMode.ShowText = true;
    this.btnDefault.Checked = true;
    componentResourceManager.ApplyResources((object) this.btnDefault, "btnDefault");
    this.btnDefault.ImageIndex = 4;
    this.btnDefault.ShowText = true;
    this.btnDefault.Click += new EventHandler(this.DoDefaultView);
    componentResourceManager.ApplyResources((object) this.btnThumbnails, "btnThumbnails");
    this.btnThumbnails.ImageIndex = 5;
    this.btnThumbnails.ShowText = true;
    this.btnThumbnails.Click += new EventHandler(this.DoThumbnailsView);
    this.btnHideInvisibles.AutoToggle = AutoToggleType.Single;
    this.btnHideInvisibles.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.btnHideInvisibles, "btnHideInvisibles");
    this.btnHideInvisibles.ImageIndex = 6;
    this.btnHideInvisibles.Click += new EventHandler(this.DoShowHideInvisibles);
    this.btnGridExpand.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.btnGridExpand, "btnGridExpand");
    this.btnGridExpand.ImageIndex = 3;
    this.btnGridExpand.Click += new EventHandler(this.DoExpandGrid);
    componentResourceManager.ApplyResources((object) this.btnGridCollapse, "btnGridCollapse");
    this.btnGridCollapse.ImageIndex = 2;
    this.btnGridCollapse.Click += new EventHandler(this.DoCollapseGrid);
    this.cbObligatoryOption.AutoToggle = AutoToggleType.Single;
    this.cbObligatoryOption.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.cbObligatoryOption, "cbObligatoryOption");
    this.cbObligatoryOption.ImageIndex = 7;
    this.cbObligatoryOption.ShowText = true;
    this.cbObligatoryOption.Click += new EventHandler(this.DoSwitchOptionObligatory);
    this.lbWarning.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.lbWarning, "lbWarning");
    this.lbWarning.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
    this.lbWarning.ForeColor = Color.Red;
    this.lbWarning.Visible = false;
    this.splitter.AnimationDelay = 20;
    this.splitter.AnimationStep = 20;
    this.splitter.BorderStyle3D = Border3DStyle.Etched;
    this.splitter.ControlToHide = (Control) null;
    componentResourceManager.ApplyResources((object) this.splitter, "splitter");
    this.splitter.ExpandParentForm = false;
    this.splitter.Name = "splitter";
    this.splitter.TabStop = false;
    this.splitter.UseAnimations = false;
    this.splitter.VisualStyle = VisualStyles.Mozilla;
    this.splitter.SplitterMoved += new SplitterEventHandler(this.splitter_SplitterMoved);
    this.splitter.Click += new EventHandler(this.DoAdvPanelsShow);
    componentResourceManager.ApplyResources((object) this.menuBarGridValues, "menuBarGridValues");
    this.menuBarGridValues.Guid = new Guid("0909a734-928b-4c5d-9a6d-05be64690c06");
    this.menuBarGridValues.Hidden = true;
    this.menuBarGridValues.ImageList = this.imagesToolbars;
    this.menuBarGridValues.Items.AddRange(new ToolbarItemBase[1]
    {
      (ToolbarItemBase) this.contextMenuBarGrid
    });
    this.menuBarGridValues.Name = "menuBarGridValues";
    this.menuBarGridValues.OwnerForm = (Form) null;
    componentResourceManager.ApplyResources((object) this.contextMenuBarGrid, "contextMenuBarGrid");
    this.contextMenuBarGrid.Items.AddRange(new ToolbarItemBase[3]
    {
      (ToolbarItemBase) this.mnpDefaultValue,
      (ToolbarItemBase) this.mnpGridExpand,
      (ToolbarItemBase) this.mnpGridCollapse
    });
    this.contextMenuBarGrid.ShowText = true;
    this.contextMenuBarGrid.BeforePopup += new MenuItemBase.BeforePopupEventHandler(this.contextMenuBarGrid_BeforePopup);
    this.mnpDefaultValue.AutoToggle = AutoToggleType.Single;
    this.mnpDefaultValue.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.mnpDefaultValue, "mnpDefaultValue");
    this.mnpDefaultValue.ImageIndex = 7;
    this.mnpDefaultValue.ShowText = true;
    this.mnpDefaultValue.Click += new EventHandler(this.DoSetDefaultValue);
    this.mnpGridExpand.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.mnpGridExpand, "mnpGridExpand");
    this.mnpGridExpand.ImageIndex = 3;
    this.mnpGridExpand.ShowText = true;
    this.mnpGridExpand.Click += new EventHandler(this.DoExpandGrid);
    componentResourceManager.ApplyResources((object) this.mnpGridCollapse, "mnpGridCollapse");
    this.mnpGridCollapse.ImageIndex = 2;
    this.mnpGridCollapse.ShowText = true;
    this.mnpGridCollapse.Click += new EventHandler(this.DoCollapseGrid);
    this.pagesMain.CausesValidation = false;
    this.pagesMain.Controls.Add((Control) this.pageObjectOptions);
    this.pagesMain.Controls.Add((Control) this.pageAppls);
    this.pagesMain.Controls.Add((Control) this.pageContext);
    this.pagesMain.Controls.Add((Control) this.pageCode);
    componentResourceManager.ApplyResources((object) this.pagesMain, "pagesMain");
    this.pagesMain.Flat = false;
    this.pagesMain.ImageList = this.imagesTabs;
    this.pagesMain.Name = "pagesMain";
    this.pagesMain.SelectedPageChanged += new EventHandler(this.pagesMain_SelectedPageChanged);
    this.pagesMain.SelectedPageChanging += new PageControlCancelEventHandler(this.pagesMain_SelectedPageChanging);
    this.pageObjectOptions.Controls.Add((Control) this.panelGrid);
    this.pageObjectOptions.Controls.Add((Control) this.splitterV);
    this.pageObjectOptions.Controls.Add((Control) this.panelTree);
    this.pageObjectOptions.Index = 0;
    componentResourceManager.ApplyResources((object) this.pageObjectOptions, "pageObjectOptions");
    this.pageObjectOptions.Name = "pageObjectOptions";
    this.pageObjectOptions.TabImage = (Image) componentResourceManager.GetObject("pageObjectOptions.TabImage");
    this.pageObjectOptions.TabImageIndex = 6;
    this.splitterV.AnimationDelay = 20;
    this.splitterV.AnimationStep = 20;
    this.splitterV.BorderStyle3D = Border3DStyle.Etched;
    this.splitterV.ControlToHide = (Control) this.panelTree;
    this.splitterV.ExpandParentForm = false;
    componentResourceManager.ApplyResources((object) this.splitterV, "splitterV");
    this.splitterV.Name = "splitter";
    this.splitterV.TabStop = false;
    this.splitterV.UseAnimations = false;
    this.splitterV.VisualStyle = VisualStyles.Mozilla;
    this.pageAppls.Controls.Add((Control) this.appEditor);
    this.pageAppls.Controls.Add((Control) this.labelAppls);
    this.pageAppls.Index = 1;
    componentResourceManager.ApplyResources((object) this.pageAppls, "pageAppls");
    this.pageAppls.Name = "pageAppls";
    this.pageAppls.TabImage = (Image) componentResourceManager.GetObject("pageAppls.TabImage");
    this.pageAppls.TabImageIndex = 2;
    componentResourceManager.ApplyResources((object) this.labelAppls, "labelAppls");
    this.labelAppls.Name = "labelAppls";
    this.pageContext.Controls.Add((Control) this.contextEditor);
    this.pageContext.Controls.Add((Control) this.lbPageContext);
    this.pageContext.Index = 2;
    componentResourceManager.ApplyResources((object) this.pageContext, "pageContext");
    this.pageContext.Name = "pageContext";
    this.pageContext.TabImage = (Image) componentResourceManager.GetObject("pageContext.TabImage");
    this.pageContext.TabImageIndex = 3;
    componentResourceManager.ApplyResources((object) this.lbPageContext, "lbPageContext");
    this.lbPageContext.Name = "lbPageContext";
    this.pageCode.Controls.Add((Control) this.codeEditor);
    this.pageCode.Index = 3;
    componentResourceManager.ApplyResources((object) this.pageCode, "pageCode");
    this.pageCode.Name = "pageCode";
    this.pageCode.TabImage = (Image) componentResourceManager.GetObject("pageCode.TabImage");
    this.pageCode.TabImageIndex = 7;
    componentResourceManager.ApplyResources((object) this.incompEditor, "incompEditor");
    this.incompEditor.IsChanged = false;
    this.incompEditor.Name = "incompEditor";
    this.incompEditor.Changed += new EventHandler(this.IncompEditor_Changed);
    componentResourceManager.ApplyResources((object) this.linkedEditor, "linkedEditor");
    this.linkedEditor.IsChanged = false;
    this.linkedEditor.Name = "linkedEditor";
    this.linkedEditor.OnChanged += new LinkedOptionsEditor.ObjectOptionsChangedEventHandler(this.linkedEditor_OnChanged);
    componentResourceManager.ApplyResources((object) this.appEditor, "appEditor");
    this.appEditor.IsChanged = false;
    this.appEditor.Name = "appEditor";
    this.appEditor.Changed += new EventHandler(this.appEditor_Changed);
    componentResourceManager.ApplyResources((object) this.contextEditor, "contextEditor");
    this.contextEditor.IsChanged = false;
    this.contextEditor.IsOptionValueStatus = false;
    this.contextEditor.Name = "contextEditor";
    this.contextEditor.OnChanged += new ObjectContextEditor.ContextChangedEventHandler(this.contextEditor_OnChanged);
    componentResourceManager.ApplyResources((object) this.codeEditor, "codeEditor");
    this.codeEditor.IsChanged = false;
    this.codeEditor.Name = "codeEditor";
    this.codeEditor.OnChanged += new ConfigurationCodeEditor.ObjectOptionsChangedEventHandler(this.codeEditor_OnChanged);
    this.AutoScaleMode = AutoScaleMode.Inherit;
    this.Controls.Add((Control) this.pagesMain);
    this.Controls.Add((Control) this.headerControl);
    this.Controls.Add((Control) this.menuBarTree);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.Name = nameof (ObjectOptionsEditor);
    this.panelTree.ResumeLayout(false);
    this.treeOptions.EndInit();
    this.panelHint.ResumeLayout(false);
    ((ISupportInitialize) this.pictureHint).EndInit();
    this.panelGrid.ResumeLayout(false);
    this._splitContainer.Panel1.ResumeLayout(false);
    this._splitContainer.Panel2.ResumeLayout(false);
    this._splitContainer.EndInit();
    this._splitContainer.ResumeLayout(false);
    ((ISupportInitialize) this.gridValues).EndInit();
    this.tabs.ResumeLayout(false);
    this.tabPageIncomps.ResumeLayout(false);
    this.tabPageLinked.ResumeLayout(false);
    this.tabPagePicture.ResumeLayout(false);
    ((ISupportInitialize) this.picture).EndInit();
    this.pagesMain.ResumeLayout(false);
    this.pageObjectOptions.ResumeLayout(false);
    this.pageAppls.ResumeLayout(false);
    this.pageContext.ResumeLayout(false);
    this.pageCode.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  public delegate void ObjectOptionsChangedEventHandler(object sender, EventArgs e);

  public sealed class State
  {
    public object TreeFocusItem { get; set; }

    public object[] TreeSelectedItems { get; set; }

    public object[] GridSelectedItems { get; set; }

    public IncompatibilityEditor.PathPart[] IncompEditorFocusPath { get; set; }

    public IncompatibilityEditor.PathPart LinkedEditorSelectedItem { get; set; }
  }
}
