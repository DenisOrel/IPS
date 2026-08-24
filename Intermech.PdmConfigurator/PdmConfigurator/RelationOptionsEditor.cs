// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.RelationOptionsEditor
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.Bars;
using Intermech.Client.Core.Thumbnail;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.PdmConfigurator;
using Intermech.Localization;
using Intermech.NavBars;
using Intermech.Navigator;
using Intermech.Navigator.Interfaces;
using NJFLib.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.PdmConfigurator;

public class RelationOptionsEditor : UserControl
{
  private static Font _boldFont;
  private PdmConfiguratorContext _context = new PdmConfiguratorContext((PdmConfiguratorContextsCache) null);
  private PdmConfiguratorContext _contextSource = new PdmConfiguratorContext((PdmConfiguratorContextsCache) null);
  private bool _readOnly;
  private bool _inEvents;
  private PdmContextAccessRights _accessRights;
  private INamedImageList _images;
  private ICategoryTypeIconService _objtypesIcons;
  private INavGraphicsCache _navGraphicsCache;
  private ICurrentUserAndRole _userRole;
  private IUserNamesCache _userNamesCache;
  private IPicturesCache _cache;
  private NotificationEventHandler _notifyHandler;
  private INotificationService _notifications;
  private IServiceProvider _services;
  private bool _isChanged;
  private bool _smallWidthMode = true;
  private static Dictionary<bool, bool> _thumbnailModeStatic = new Dictionary<bool, bool>();
  private static Dictionary<bool, Dictionary<string, int>> _colWidthsStatic = new Dictionary<bool, Dictionary<string, int>>();
  private Dictionary<long, iGRow> _objectRows = new Dictionary<long, iGRow>();
  private ViewStateFlags _state;
  private static iGCellStyle _cellStyle;
  private static iGCellStyle _cellObligatoryStyle;
  private static iGCellStyle _cellCategoryStyle;
  private static iGCellStyle _cellImage;
  private static iGCellStyle _cellStyleStatus;
  private static iGCellStyle _cellComboBox;
  private static iGCellStyle _cellComboBoxRo;
  private static iGColHdrStyle _headerStyle;
  private EventHandler handlerDoDefaultView;
  private EventHandler handlerDoThumbnailsView;
  private static StringFormat _imageStringFormat = new StringFormat();
  private IContainer components;
  private HeaderControl headerControl;
  private Panel panelGrid;
  private iGrid gridValues;
  private Intermech.Bars.ToolBar toolBarGrid;
  private DropDownMenuItem btnMode;
  private MenuButtonItem btnDefault;
  private MenuButtonItem btnThumbnails;
  private ButtonItem btnGridExpand;
  private ButtonItem btnGridCollapse;
  private ToolTip toolTip;
  private ImageList imagesToolbars;
  private ButtonItem btnAddOption;
  private ButtonItem btnImport;
  private ButtonItem btnDelete;
  private MenuBar menuBar;
  private ContextMenuBarItem contextMenuBarItem;
  private MenuButtonItem mnpAddOption;
  private MenuButtonItem mnpImport;
  private MenuButtonItem mnpDelete;
  private MenuButtonItem mnpExpand;
  private MenuButtonItem mnpCollapse;
  private CollapsibleSplitter splitter;
  private Panel panelAppEditor;
  private Label labelDeleteMe;

  private bool _thumbnailMode
  {
    [DebuggerStepThrough] get => RelationOptionsEditor._thumbnailModeStatic[this._smallWidthMode];
    set => RelationOptionsEditor._thumbnailModeStatic[this._smallWidthMode] = value;
  }

  private Dictionary<string, int> _colWidths
  {
    get => RelationOptionsEditor._colWidthsStatic[this._smallWidthMode];
  }

  static RelationOptionsEditor()
  {
    RelationOptionsEditor._colWidthsStatic.Add(false, new Dictionary<string, int>());
    RelationOptionsEditor._colWidthsStatic.Add(true, new Dictionary<string, int>());
    RelationOptionsEditor._thumbnailModeStatic.Add(false, false);
    RelationOptionsEditor._thumbnailModeStatic.Add(true, false);
  }

  public RelationOptionsEditor()
  {
    this.InitializeComponent();
    if (ServicesManager.GetService(typeof (BarManager)) is BarManager service)
    {
      service.RendererChanged += new EventHandler(this.ToolbarRendererChanged);
      this.ToolbarRendererChanged((object) service, EventArgs.Empty);
    }
    if (!(ServicesManager.GetService(typeof (IGuidMapper)) is IGuidMapper))
      return;
    this.Init();
  }

  public event RelationOptionsEditor.ContextChangedEventHandler OnChanged;

  protected virtual void RaiseOnChanged()
  {
    if (this.OnChanged == null)
      return;
    this.OnChanged((object) this, new EventArgs());
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public virtual bool ReadOnly
  {
    [DebuggerStepThrough] get => this._readOnly;
    set => this._readOnly = value;
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public virtual PdmContextAccessRights AccessRights
  {
    [DebuggerStepThrough] get => this._accessRights;
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public virtual RelationPair ParentKey
  {
    get => this._context.ParentKey;
    set => this._context.ParentKey = value;
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public virtual RelationPair Key
  {
    get => this._context.Key;
    set
    {
      this.Clear();
      if (value != null && !value.Empty)
      {
        this._context.Key = new RelationPair((object) value);
        if (value.F_PRJLINK_ID != 0L)
        {
          using (SessionKeeper sessionKeeper = new SessionKeeper())
          {
            try
            {
              this._context.Services.AddService(typeof (IUserSession), (object) sessionKeeper.Session);
              IDBRelation relation = sessionKeeper.Session.GetRelation(value.F_PRJLINK_ID, false);
              this._context.Assign((object) relation);
              this.FillEditor(false);
              this.CheckAccessRights((IDBAttributable) relation);
            }
            finally
            {
              this._context.Services.RemoveService(typeof (IUserSession));
            }
          }
        }
        else
        {
          using (SessionKeeper sessionKeeper = new SessionKeeper())
          {
            try
            {
              this._context.Services.AddService(typeof (IUserSession), (object) sessionKeeper.Session);
              IDBObject source = sessionKeeper.Session.GetObject(value.F_PROJ_ID, false);
              if (source != null)
              {
                this._context.Assign((object) source);
              }
              else
              {
                PdmConfiguratorContext context = this._context;
                if (context.ContextsCache != null)
                  this._context.Assign((object) context.ContextsCache[value]);
              }
              this.CheckAccessRights((IDBAttributable) source);
              this.FillEditor(false);
            }
            finally
            {
              this._context.Services.RemoveService(typeof (IUserSession));
            }
          }
        }
      }
      this.Fix();
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public virtual PdmConfiguratorContext Context
  {
    get => this._context.Clone() as PdmConfiguratorContext;
    set
    {
      this._context.Assign((object) value);
      this.FillEditor(true);
      this.Fix();
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public virtual PdmContextType ContextType
  {
    [DebuggerStepThrough] get => this._context.ContextType;
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public virtual IServiceProvider Services
  {
    [DebuggerStepThrough] get => this._services;
    set
    {
      this._services = value;
      this.InitServices();
    }
  }

  [Category("Appearance")]
  [Browsable(true)]
  public virtual bool IsChanged
  {
    [DebuggerStepThrough] get => this._isChanged;
    set
    {
      this._isChanged = value;
      this.RaiseOnChanged();
      this.UpdateControls();
    }
  }

  [Category("Appearance")]
  [Browsable(true)]
  public virtual bool DisableHeader
  {
    [DebuggerStepThrough] get => !this.headerControl.Visible;
    set => this.headerControl.Visible = !value;
  }

  protected virtual void ToolbarRendererChanged(object sender, EventArgs e)
  {
    IToolBarRenderer renderer = (sender as BarManager).Renderer;
    this.toolBarGrid.Renderer = renderer;
    this.menuBar.Renderer = renderer;
  }

  private int GetTypeImageIndex(FieldTypes attrType)
  {
    return this._objtypesIcons == null ? -1 : this._objtypesIcons.IndexOf(3, -1, (object) attrType);
  }

  public virtual void Init()
  {
    this._images = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
    this._objtypesIcons = ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
    this._navGraphicsCache = ServicesManager.GetService(typeof (INavGraphicsCache)) as INavGraphicsCache;
    this._userRole = ServicesManager.GetService(typeof (ICurrentUserAndRole)) as ICurrentUserAndRole;
    this._userNamesCache = CacheManager.Cache("UserNamesCache") as IUserNamesCache;
    this._cache = ServicesManager.GetService(typeof (IPicturesCache)) as IPicturesCache;
    this._notifications = ServicesManager.GetService(typeof (INotificationService)) as INotificationService;
    if (this._notifications != null && this._notifyHandler == null)
    {
      this._notifyHandler = new NotificationEventHandler(this.NotificationEventFired);
      this._notifications.Subscribe(this._notifyHandler);
    }
    INamedImageList images = this._images;
    this._isChanged = false;
    this._context.ContextsCache = ServicesManager.GetService(typeof (PdmConfiguratorContextsCache)) as PdmConfiguratorContextsCache;
    this._contextSource.ContextsCache = this._context.ContextsCache;
    bool inEvents = this._inEvents;
    try
    {
      this._inEvents = true;
      this.FillEditor(false);
      this.SetHandlers();
    }
    finally
    {
      this._inEvents = inEvents;
    }
    this.UpdateControls();
  }

  private void InitServices()
  {
    IViewState service = this._services != null ? this._services.GetService(typeof (IViewState)) as IViewState : (IViewState) null;
    this._state = service != null ? service.ViewState : ViewStateFlags.None;
    this._smallWidthMode = (this._state & ViewStateFlags.NodeUnderTree) == ViewStateFlags.NodeUnderTree;
    this.gridValues.AutoResizeCols = false;
    this.gridValues.AutoWidthColMode = iGAutoWidthColMode.HeaderAndCells;
    this.gridValues.Rows.Clear();
    this.gridValues.Cols.Clear();
    this.PrepareGridsColumns();
    this.CorrectGridColsWidth();
    this.UpdateControls();
  }

  public virtual void CheckAccessRights()
  {
    this._accessRights = PdmContextAccessRights.ReadOnly;
    if (this._context.Key.Empty)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBAttributable dbAttributable = (IDBAttributable) null;
      if (this.Key.F_PRJLINK_ID != 0L)
        dbAttributable = (IDBAttributable) sessionKeeper.Session.GetRelation(this._context.Key.F_PRJLINK_ID, false);
      else if (this.Key.F_PROJ_ID != 0L)
        dbAttributable = (IDBAttributable) sessionKeeper.Session.GetObject(this._context.Key.F_PROJ_ID, false);
      this.CheckAccessRights(dbAttributable);
    }
  }

  public virtual void CheckAccessRights(IDBAttributable item)
  {
    this._accessRights = PdmContextAccessRights.ReadOnly;
    if (item == null)
      return;
    IDBAttribute byId = item.Attributes.FindByID(Intermech.Interfaces.PdmConfigurator.Consts.attributeConfiguratorContextID);
    this._accessRights = byId == null || byId.ReadOnly ? PdmContextAccessRights.ReadOnly : PdmContextAccessRights.FullAccess;
    if (byId != null)
      return;
    switch (item)
    {
      case IDBRelation dbRelation:
        IDBObject dbObject1 = dbRelation.Session.GetObject(dbRelation.ProjID, false);
        if (dbObject1 == null)
          break;
        try
        {
          dbObject1.CheckRelationsEdit();
          this._accessRights = PdmContextAccessRights.FullAccess;
          break;
        }
        catch
        {
          break;
        }
      case IDBObject dbObject2:
        if (MetaDataHelper.IsPdmContextableObjectType(dbObject2.ObjectType))
        {
          try
          {
            dbObject2.CheckEdit();
            this._accessRights = PdmContextAccessRights.FullAccess;
            break;
          }
          catch
          {
            break;
          }
        }
        else
        {
          if (!MetaDataHelper.IsPdmConfigurableObjectType(dbObject2.ObjectType))
            break;
          this._accessRights = PdmContextAccessRights.FullAccess;
          break;
        }
    }
  }

  public virtual void UpdateControls()
  {
    int num = this._readOnly ? 0 : ((this._accessRights & PdmContextAccessRights.FullAccess) != 0 ? 1 : 0);
    bool smallWidthMode = this._smallWidthMode;
    this.btnMode.ShowText = !smallWidthMode;
    this.btnDefault.ShowText = !smallWidthMode;
    this.btnThumbnails.ShowText = !smallWidthMode;
    this.btnAddOption.ShowText = !smallWidthMode;
    this.btnImport.ShowText = !smallWidthMode;
    this.btnDelete.ShowText = !smallWidthMode;
    this.btnGridExpand.ShowText = !smallWidthMode;
    this.btnGridCollapse.ShowText = !smallWidthMode;
  }

  public virtual void Clear()
  {
    this._context.Clear();
    this.FillEditor(false);
  }

  protected virtual void PrepareGridsStyles()
  {
    if (RelationOptionsEditor._boldFont == null)
      RelationOptionsEditor._boldFont = new Font(this.gridValues.Font, FontStyle.Bold);
    if (RelationOptionsEditor._cellStyle != null)
      return;
    RelationOptionsEditor._cellStyle = new iGCellStyle(true);
    RelationOptionsEditor._cellStyle.ReadOnly = iGBool.True;
    RelationOptionsEditor._cellStyle.SingleClickEdit = iGBool.False;
    RelationOptionsEditor._cellStyle.TextAlign = iGContentAlignment.TopLeft;
    RelationOptionsEditor._cellStyle.ImageAlign = iGContentAlignment.TopLeft;
    RelationOptionsEditor._cellObligatoryStyle = RelationOptionsEditor._cellStyle.Clone();
    RelationOptionsEditor._cellObligatoryStyle.ForeColor = Color.Blue;
    RelationOptionsEditor._cellCategoryStyle = RelationOptionsEditor._cellStyle.Clone();
    RelationOptionsEditor._cellCategoryStyle.Font = RelationOptionsEditor._boldFont;
    RelationOptionsEditor._cellImage = new iGCellStyle(true);
    RelationOptionsEditor._cellImage.CustomDrawFlags = iGCustomDrawFlags.Foreground;
    RelationOptionsEditor._cellImage.EmptyStringAs = iGEmptyStringAs.EmptyString;
    RelationOptionsEditor._cellImage.ReadOnly = iGBool.True;
    RelationOptionsEditor._cellImage.SingleClickEdit = iGBool.False;
    RelationOptionsEditor._cellImage.TextAlign = iGContentAlignment.TopCenter;
    RelationOptionsEditor._cellImage.ValueType = typeof (Image);
    RelationOptionsEditor._cellStyleStatus = RelationOptionsEditor._cellImage.Clone();
    RelationOptionsEditor._cellStyleStatus.ImageAlign = iGContentAlignment.TopCenter;
    RelationOptionsEditor._cellComboBox = new iGCellStyle(true);
    RelationOptionsEditor._cellComboBox.EmptyStringAs = iGEmptyStringAs.EmptyString;
    RelationOptionsEditor._cellComboBox.ReadOnly = iGBool.False;
    RelationOptionsEditor._cellComboBox.SingleClickEdit = iGBool.True;
    RelationOptionsEditor._cellComboBox.TextAlign = iGContentAlignment.TopLeft;
    RelationOptionsEditor._cellComboBox.ValueType = typeof (string);
    RelationOptionsEditor._cellComboBox.Type = iGCellType.Combo;
    RelationOptionsEditor._cellComboBox.TypeFlags = iGCellTypeFlags.ComboPreferValue;
    RelationOptionsEditor._cellComboBoxRo = RelationOptionsEditor._cellComboBox.Clone();
    RelationOptionsEditor._cellComboBoxRo.ReadOnly = iGBool.True;
    RelationOptionsEditor._cellComboBoxRo.SingleClickEdit = iGBool.False;
    RelationOptionsEditor._cellComboBoxRo.BackColor = Color.Orange;
    RelationOptionsEditor._headerStyle = new iGColHdrStyle(true);
    RelationOptionsEditor._headerStyle.TextAlign = iGContentAlignment.TopLeft;
  }

  protected virtual void PrepareGridsColumns()
  {
    int num1 = this._context.Key.Empty ? 1 : 0;
    int num2 = this._readOnly ? 0 : ((this._accessRights & PdmContextAccessRights.FullAccess) != 0 ? 1 : 0);
    this.PrepareGridsStyles();
    this.gridValues.Header.ImageList = this._objtypesIcons.ImageList;
    if (RelationOptionsEditor._colWidthsStatic[false].Count == 0)
    {
      RelationOptionsEditor._colWidthsStatic[false].Add("OPTION", 256 /*0x0100*/);
      RelationOptionsEditor._colWidthsStatic[false].Add("IMAGE", 48 /*0x30*/);
      RelationOptionsEditor._colWidthsStatic[false].Add("VALUE", 128 /*0x80*/);
      RelationOptionsEditor._colWidthsStatic[false].Add("NOTE", 128 /*0x80*/);
      RelationOptionsEditor._colWidthsStatic[false].Add("STATUS", 48 /*0x30*/);
      RelationOptionsEditor._colWidthsStatic[false].Add("TAG", 0);
      RelationOptionsEditor._colWidthsStatic[false].Add("OPTVALUE", 0);
      RelationOptionsEditor._colWidthsStatic[false].Add("CATEGORY", 0);
      RelationOptionsEditor._colWidthsStatic[false].Add("OBLIGATORY", 0);
    }
    if (RelationOptionsEditor._colWidthsStatic[true].Count == 0)
    {
      RelationOptionsEditor._colWidthsStatic[true].Add("OPTION", 150);
      RelationOptionsEditor._colWidthsStatic[true].Add("IMAGE", 32 /*0x20*/);
      RelationOptionsEditor._colWidthsStatic[true].Add("VALUE", 50);
      RelationOptionsEditor._colWidthsStatic[true].Add("NOTE", 0);
      RelationOptionsEditor._colWidthsStatic[true].Add("STATUS", 0);
      RelationOptionsEditor._colWidthsStatic[true].Add("TAG", 0);
      RelationOptionsEditor._colWidthsStatic[true].Add("OPTVALUE", 0);
      RelationOptionsEditor._colWidthsStatic[true].Add("CATEGORY", 0);
      RelationOptionsEditor._colWidthsStatic[true].Add("OBLIGATORY", 0);
    }
    iGCol iGcol1 = this.gridValues.Cols["OPTION"] ?? this.gridValues.Cols.Add(new iGColPattern(Math.Max(32 /*0x20*/, this._colWidths["OPTION"]), true, true, 32 /*0x20*/, -1, true, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_37"), "OPTION", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol1.Width = this._colWidths["OPTION"];
    iGcol1.ColHdrStyle = RelationOptionsEditor._headerStyle;
    iGcol1.CellStyle = RelationOptionsEditor._cellStyle;
    iGCol iGcol2 = this.gridValues.Cols["IMAGE"] ?? this.gridValues.Cols.Add(new iGColPattern(Math.Max(32 /*0x20*/, this._colWidths["IMAGE"]), true, true, 32 /*0x20*/, -1, true, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_38"), "IMAGE", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol2.CellStyle = RelationOptionsEditor._cellImage;
    iGcol2.Width = this._colWidths["IMAGE"];
    iGcol2.ColHdrStyle = RelationOptionsEditor._headerStyle;
    iGcol2.Visible = this._thumbnailMode;
    iGCol iGcol3 = this.gridValues.Cols["VALUE"] ?? this.gridValues.Cols.Add(new iGColPattern(Math.Max(32 /*0x20*/, this._colWidths["VALUE"]), true, true, 32 /*0x20*/, -1, !this._smallWidthMode, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_39"), "VALUE", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol3.CellStyle = RelationOptionsEditor._cellStyle;
    iGcol3.Width = this._colWidths["VALUE"];
    iGcol3.ColHdrStyle = RelationOptionsEditor._headerStyle;
    iGCol iGcol4 = this.gridValues.Cols["NOTE"] ?? this.gridValues.Cols.Add(new iGColPattern(Math.Max(0, this._colWidths["NOTE"]), true, true, 0, -1, true, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_40"), "NOTE", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol4.CellStyle = RelationOptionsEditor._cellStyle;
    iGcol4.Width = this._colWidths["NOTE"];
    iGcol4.ColHdrStyle = RelationOptionsEditor._headerStyle;
    iGCol iGcol5 = this.gridValues.Cols["STATUS"] ?? this.gridValues.Cols.Add(new iGColPattern(Math.Max(0, this._colWidths["STATUS"]), true, false, 0, -1, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_41"), "STATUS", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol5.CellStyle = RelationOptionsEditor._cellStyleStatus;
    iGcol5.Width = this._colWidths["STATUS"];
    iGcol5.ColHdrStyle = RelationOptionsEditor._headerStyle;
    (this.gridValues.Cols["TAG"] ?? this.gridValues.Cols.Add(new iGColPattern(this._colWidths["TAG"], false, false, 0, 0, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) "", "TAG", -1, (object) null, (object) null, -1))).Width = this._colWidths["TAG"];
    (this.gridValues.Cols["OPTVALUE"] ?? this.gridValues.Cols.Add(new iGColPattern(this._colWidths["OPTVALUE"], false, false, 0, 0, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) "", "OPTVALUE", -1, (object) null, (object) null, -1))).Width = this._colWidths["OPTVALUE"];
    (this.gridValues.Cols["CATEGORY"] ?? this.gridValues.Cols.Add(new iGColPattern(this._colWidths["CATEGORY"], false, false, 0, 0, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) "", "CATEGORY", -1, (object) null, (object) null, -1))).Width = this._colWidths["CATEGORY"];
    (this.gridValues.Cols["OBLIGATORY"] ?? this.gridValues.Cols.Add(new iGColPattern(this._colWidths["OBLIGATORY"], false, false, 0, 0, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) "", "OBLIGATORY", -1, (object) null, (object) null, -1))).Width = this._colWidths["OBLIGATORY"];
    this.CorrectGridColsWidth();
  }

  private void CorrectGridColsWidth()
  {
    if (this.gridValues.AutoResizeCols || this._colWidths.Count == 0)
      return;
    int num = this.gridValues.ClientRectangle.Width - 30 - this._colWidths["OPTION"] - this._colWidths["NOTE"] - this._colWidths["STATUS"] - this._colWidths["TAG"] - this._colWidths["OPTVALUE"] - this._colWidths["CATEGORY"] - this._colWidths["OBLIGATORY"];
    if (this._thumbnailMode)
      num -= this._colWidths["IMAGE"];
    if (this.gridValues.Cols.Count == 0)
      return;
    this.gridValues.Cols["OPTION"].Width = this._colWidths["OPTION"];
    this.gridValues.Cols["NOTE"].Width = this._colWidths["NOTE"];
    this.gridValues.Cols["STATUS"].Width = this._colWidths["STATUS"];
    if (num > 32 /*0x20*/)
    {
      this._colWidths["VALUE"] = num;
      this.gridValues.Cols["VALUE"].Width = num;
    }
    this.gridValues.Rows.AutoHeight();
  }

  private iGRow AddCategory(long category)
  {
    if (this._objectRows.ContainsKey(category))
      return this._objectRows[category];
    IUserSession service = this._context.Services.GetService(typeof (IUserSession)) as IUserSession;
    OptionObjectDescription category1 = PdmConfiguratorCache.CacheFindCategory(category);
    if (category1 == null)
    {
      if (service != null)
        PdmConfiguratorCache.CacheLoadCategories(service);
      category1 = PdmConfiguratorCache.CacheFindCategory(category);
      if (category1 == null)
        return (iGRow) null;
    }
    iGRow iGrow = this.gridValues.Rows.Add();
    this._objectRows.Add(category, iGrow);
    iGrow.Cells["CATEGORY"].ValueType = typeof (OptionObjectDescription);
    iGrow.Cells["CATEGORY"].Value = (object) category1;
    iGrow.Cells["OPTION"].Value = (object) category1.CAPTION;
    iGrow.Cells["OPTION"].ImageList = this._objtypesIcons.ImageList;
    iGrow.Cells["OPTION"].ImageIndex = this._objtypesIcons.IndexOf(4, Intermech.Interfaces.PdmConfigurator.Consts.objtypeOptionsGroupID);
    iGrow.Cells["OPTION"].Style = RelationOptionsEditor._cellCategoryStyle;
    iGrow.Level = 0;
    return iGrow;
  }

  private iGRow AddOptionValue(Guid option, string id)
  {
    List<string> optionVisibleValues = this._context.GetOptionVisibleValues(option);
    if (optionVisibleValues == null || optionVisibleValues.Count == 0)
      return (iGRow) null;
    bool flag = this._context.IsObligatoryOption(option);
    if (optionVisibleValues.IndexOf(id) < 0)
      id = string.Empty;
    IUserSession service = this._context.Services.GetService(typeof (IUserSession)) as IUserSession;
    OptionHolder option1 = PdmConfiguratorCache.CacheFindOption(option);
    if (option1 == null)
    {
      if (service != null)
        PdmConfiguratorCache.CacheAddOption(service, option);
      option1 = PdmConfiguratorCache.CacheFindOption(option);
      if (option1 == null)
        return (iGRow) null;
    }
    if (this._objectRows.ContainsKey(option1.OptionObjectID))
      return this._objectRows[option1.OptionObjectID];
    iGRow iGrow1 = this.AddCategory(option1.OptionCategory);
    if (iGrow1 == null)
      return (iGRow) null;
    OptionObjectDescription objectDescription1 = iGrow1.Cells["CATEGORY"].Value as OptionObjectDescription;
    OptionValue optionValue1 = option1.OptionValues.FindValue(id);
    int index1 = iGrow1.Index;
    iGRow iGrow2 = iGrow1;
    if (index1 < this.gridValues.Rows.Count - 1)
    {
      while (index1 < this.gridValues.Rows.Count - 1)
      {
        ++index1;
        iGRow row = this.gridValues.Rows[index1];
        if (row.Cells["CATEGORY"].Value is OptionObjectDescription objectDescription2 && objectDescription1 != null && objectDescription1.F_OBJECT_ID == objectDescription2.F_OBJECT_ID)
          iGrow2 = row;
        else
          break;
      }
    }
    iGRow iGrow3 = this.gridValues.Rows.Insert(iGrow2.Index + 1);
    this._objectRows.Add(option1.OptionObjectID, iGrow3);
    iGDropDownList iGdropDownList = new iGDropDownList();
    MyElement myElement1 = (MyElement) null;
    MyElement myElement2 = new MyElement((object) null, "", (object) string.Empty);
    if (option1 != null)
    {
      for (int index2 = 0; index2 < optionVisibleValues.Count; ++index2)
      {
        OptionValue optionValue2 = option1.OptionValues.FindValue(optionVisibleValues[index2]);
        if (optionValue2 != null)
        {
          MyElement myElement3 = new MyElement((object) optionValue2, optionValue2.GetDisplayValue(option1), (object) -1);
          if (optionValue2.ID == id)
            myElement1 = myElement3;
          iGdropDownList.Items.Add((object) myElement3);
        }
      }
      if (!flag)
        iGdropDownList.Items.Insert(0, (object) myElement2);
    }
    iGrow3.Level = 1;
    iGrow3.Cells["OPTION"].Value = !flag || myElement1 != null ? (object) option1.OptionCaption : (object) ("* " + option1.OptionCaption);
    iGrow3.Cells["OPTION"].Style = flag ? (myElement1 == null ? RelationOptionsEditor._cellObligatoryStyle : RelationOptionsEditor._cellStyle) : RelationOptionsEditor._cellStyle;
    iGrow3.Cells["OPTION"].ImageList = this._objtypesIcons.ImageList;
    iGrow3.Cells["OPTION"].ImageIndex = this._objtypesIcons.IndexOf(4, Intermech.Interfaces.PdmConfigurator.Consts.objtypeOptionID);
    iGrow3.Cells["IMAGE"].Value = (object) (optionValue1 != null ? optionValue1.Image : Guid.Empty);
    iGrow3.Cells["VALUE"].Style = this._accessRights == PdmContextAccessRights.FullAccess ? RelationOptionsEditor._cellComboBox.Clone() : RelationOptionsEditor._cellComboBoxRo.Clone();
    iGrow3.Cells["VALUE"].Value = (object) (myElement1 ?? (!flag ? myElement2 : (MyElement) null));
    iGrow3.Cells["VALUE"].DropDownControl = (IiGDropDownControl) iGdropDownList;
    iGrow3.Cells["VALUE"].DropDownControl.SelectedItem = (object) (myElement1 ?? (!flag ? myElement2 : (MyElement) null));
    iGrow3.Cells["NOTE"].Value = optionValue1 != null ? (object) optionValue1.Description : (object) string.Empty;
    iGrow3.Cells["TAG"].Value = (object) option1;
    iGrow3.Cells["OPTVALUE"].Value = (object) optionValue1;
    iGrow3.Cells["CATEGORY"].Value = (object) objectDescription1;
    iGrow3.Cells["OBLIGATORY"].Value = (object) flag;
    return iGrow3;
  }

  private void FillGrid()
  {
    bool inEvents = this._inEvents;
    try
    {
      this._inEvents = true;
      this._objectRows.Clear();
      this.gridValues.Rows.Clear();
      this.PrepareGridsColumns();
      if (this._context.OptionsValues.Count == 0)
        return;
      List<Guid> sortedOptionsList = this._context.GetSortedOptionsList();
      for (int index = 0; index < sortedOptionsList.Count; ++index)
        this.AddOptionValue(sortedOptionsList[index], this._context.OptionsValues[sortedOptionsList[index]]);
      this.gridValues.Rows.AutoHeight();
    }
    finally
    {
      this._inEvents = inEvents;
    }
    this.CorrectGridColsWidth();
  }

  private void FillEditor(bool checkAccess)
  {
    if (checkAccess)
      this.CheckAccessRights();
    bool inEvents = this._inEvents;
    try
    {
      this._inEvents = true;
      this.FillGrid();
    }
    finally
    {
      this._inEvents = inEvents;
    }
    this.CorrectGridColsWidth();
    this.UpdateControls();
    this.RaiseOnChanged();
  }

  public void Fix()
  {
    this._contextSource.Assign((object) this._context);
    this._isChanged = false;
    this.UpdateControls();
    this.RaiseOnChanged();
  }

  public void Undo()
  {
    this.Context = this._contextSource;
    this.RaiseOnChanged();
  }

  protected virtual void NotificationEventFired(object sender, NotificationEventArgs e)
  {
  }

  private void SetHandlers()
  {
    if (this.handlerDoDefaultView == null)
    {
      this.handlerDoDefaultView = new EventHandler(this.DoDefaultView);
      this.handlerDoThumbnailsView = new EventHandler(this.DoThumbnailsView);
    }
    if (this._thumbnailMode)
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

  private void DoDefaultView(object sender, EventArgs e)
  {
    if (this._inEvents)
      return;
    this._thumbnailMode = false;
    this.SetHandlers();
    this.FillGrid();
    this.CorrectGridColsWidth();
    this.UpdateControls();
  }

  private void DoThumbnailsView(object sender, EventArgs e)
  {
    if (this._inEvents)
      return;
    this._thumbnailMode = true;
    this.SetHandlers();
    this.FillGrid();
    this.CorrectGridColsWidth();
    this.UpdateControls();
  }

  private void DoAdd(object sender, EventArgs e)
  {
  }

  private void DoImport(object sender, EventArgs e)
  {
  }

  private void DoDelete(object sender, EventArgs e)
  {
  }

  private void DoExpand(object sender, EventArgs e)
  {
  }

  private void DoCollapse(object sender, EventArgs e)
  {
  }

  private void gridValues_ColWidthChanging(object sender, iGColWidthEventArgs e)
  {
    if (this._inEvents)
      return;
    if (this.gridValues.AutoResizeCols)
      return;
    try
    {
      this._inEvents = true;
      string key1 = e.ColIndex < this.gridValues.Cols.Count - 1 ? this.gridValues.Cols[e.ColIndex + 1].Key : string.Empty;
      if (!string.IsNullOrEmpty(key1) && !this.gridValues.Cols[key1].Visible && this.gridValues.Cols[key1].Index < this.gridValues.Cols.Count - 1)
        key1 = this.gridValues.Cols[e.ColIndex + 2].Key;
      string key2 = this.gridValues.Cols[e.ColIndex].Key;
      if (!string.IsNullOrEmpty(key1))
      {
        int num = e.Width - this._colWidths[key2];
        this._colWidths[key1] = Math.Max(this._colWidths[key1] - num, this.gridValues.Cols[key1].MinWidth);
        this._colWidths[key2] = Math.Max(this._colWidths[key2] + num, this.gridValues.Cols[key2].MinWidth);
        this.gridValues.Cols[key1].Width = this._colWidths[key1];
        this.gridValues.Cols[key2].Width = this._colWidths[key2];
      }
      else
      {
        this._colWidths[this.gridValues.Cols[e.ColIndex].Key] = Math.Max(e.Width, this.gridValues.Cols[e.ColIndex].MinWidth);
        this.gridValues.Cols[e.ColIndex].Width = e.Width;
      }
      this.CorrectGridColsWidth();
    }
    finally
    {
      this._inEvents = false;
    }
  }

  private void DoResize(object sender, EventArgs e) => this.CorrectGridColsWidth();

  private void gridValues_BeforeCommitEdit(object sender, iGBeforeCommitEditEventArgs e)
  {
    iGRow row = this.gridValues.Rows[e.RowIndex];
    if (this.gridValues.Cols[e.ColIndex].Key != "VALUE")
      return;
    MyElement newValue = e.NewValue as MyElement;
    OptionHolder optionHolder = row.Cells["TAG"].Value as OptionHolder;
    OptionValue optionValue = newValue != null ? newValue.Value as OptionValue : (OptionValue) null;
    if (newValue == null || optionHolder == null)
    {
      e.Result = iGEditResult.Cancel;
    }
    else
    {
      bool flag = this._context.IsObligatoryOption(optionHolder.OptionGuid);
      if (optionValue != null)
      {
        this._context.OptionsValues[optionHolder.OptionGuid] = optionValue.ID;
      }
      else
      {
        if (flag)
        {
          e.Result = iGEditResult.Cancel;
          return;
        }
        if (this._context.OptionsValues.ContainsKey(optionHolder.OptionGuid))
          this._context.OptionsValues.Remove(optionHolder.OptionGuid);
      }
      row.Cells["IMAGE"].Value = (object) (optionValue != null ? optionValue.Image : Guid.Empty);
      row.Cells["OPTVALUE"].Value = (object) optionValue;
      row.Cells["NOTE"].Value = optionValue != null ? (object) optionValue.Description : (object) string.Empty;
      row.Cells["OPTION"].Style = flag ? (optionValue == null ? RelationOptionsEditor._cellObligatoryStyle : RelationOptionsEditor._cellStyle) : RelationOptionsEditor._cellStyle;
      row.Cells["OPTION"].Value = !flag || optionValue != null ? (object) optionHolder.OptionCaption : (object) ("* " + optionHolder.OptionCaption);
      row.AutoHeight();
    }
  }

  private void gridValues_AfterCommitEdit(object sender, iGAfterCommitEditEventArgs e)
  {
    this.IsChanged = true;
  }

  private void gridValues_CellClick(object sender, iGCellClickEventArgs e)
  {
  }

  private void gridValues_CustomDrawCellForeground(object sender, iGCustomDrawCellEventArgs e)
  {
    iGCol col = this.gridValues.Cols[e.ColIndex];
    if (col.Key == "STATUS" || !this._thumbnailMode || col.Key != "IMAGE")
      return;
    object picture = !(this.gridValues.Rows[e.RowIndex].Cells["OPTVALUE"].Value is OptionValue optionValue) || !(optionValue.Image != Guid.Empty) ? (object) null : this._cache.GetPicture(optionValue.Image);
    if (picture == null)
      return;
    Rectangle imageBounds;
    ref Rectangle local = ref imageBounds;
    int x = e.Bounds.Left + 1;
    Rectangle bounds = e.Bounds;
    int y = bounds.Top + 1;
    bounds = e.Bounds;
    int width = bounds.Width - 2;
    bounds = e.Bounds;
    int height = bounds.Height - 2;
    local = new Rectangle(x, y, width, height);
    ThumbnailRenderer.GetImageObjectSizeAdv(picture, imageBounds);
    ThumbnailRenderer.DrawImageObjectAdv(e.Graphics, picture, imageBounds, this.gridValues.Font, RelationOptionsEditor._imageStringFormat);
  }

  private void gridValues_CustomDrawCellGetHeight(
    object sender,
    iGCustomDrawCellGetHeightEventArgs e)
  {
    iGCol col = this.gridValues.Cols[e.ColIndex];
    if (col.Key == "STATUS")
    {
      e.Height = this._smallWidthMode ? this.gridValues.DefaultRow.Height : 20;
    }
    else
    {
      if (!this._thumbnailMode)
        return;
      e.Height = this.gridValues.DefaultRow.Height;
      if (col.Key != "IMAGE")
        return;
      iGRow row = this.gridValues.Rows[e.RowIndex];
      OptionValue optionValue = row.Cells["OPTVALUE"].Value as OptionValue;
      object obj = row.Cells["TAG"].Value;
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

  protected override void Dispose(bool disposing)
  {
    if (disposing && ServicesManager.GetService(typeof (BarManager)) is BarManager service)
    {
      this.toolBarGrid.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
      this.menuBar.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
      service.RendererChanged -= new EventHandler(this.ToolbarRendererChanged);
    }
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (RelationOptionsEditor));
    this.headerControl = new HeaderControl();
    this.panelGrid = new Panel();
    this.gridValues = new iGrid();
    this.toolBarGrid = new Intermech.Bars.ToolBar();
    this.imagesToolbars = new ImageList(this.components);
    this.btnMode = new DropDownMenuItem();
    this.btnDefault = new MenuButtonItem();
    this.btnThumbnails = new MenuButtonItem();
    this.btnAddOption = new ButtonItem();
    this.btnImport = new ButtonItem();
    this.btnDelete = new ButtonItem();
    this.btnGridExpand = new ButtonItem();
    this.btnGridCollapse = new ButtonItem();
    this.menuBar = new MenuBar();
    this.contextMenuBarItem = new ContextMenuBarItem();
    this.mnpAddOption = new MenuButtonItem();
    this.mnpImport = new MenuButtonItem();
    this.mnpDelete = new MenuButtonItem();
    this.mnpExpand = new MenuButtonItem();
    this.mnpCollapse = new MenuButtonItem();
    this.toolTip = new ToolTip(this.components);
    this.splitter = new CollapsibleSplitter();
    this.panelAppEditor = new Panel();
    this.labelDeleteMe = new Label();
    this.panelGrid.SuspendLayout();
    ((ISupportInitialize) this.gridValues).BeginInit();
    this.panelAppEditor.SuspendLayout();
    this.SuspendLayout();
    this.headerControl.BackColor = SystemColors.Control;
    componentResourceManager.ApplyResources((object) this.headerControl, "headerControl");
    this.headerControl.ForeColor = SystemColors.ControlText;
    this.headerControl.HeaderFont = new Font("Tahoma", 12f, FontStyle.Bold);
    this.headerControl.Name = "headerControl";
    this.panelGrid.Controls.Add((Control) this.gridValues);
    this.panelGrid.Controls.Add((Control) this.toolBarGrid);
    this.panelGrid.Controls.Add((Control) this.menuBar);
    componentResourceManager.ApplyResources((object) this.panelGrid, "panelGrid");
    this.panelGrid.Name = "panelGrid";
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
    this.gridValues.PressedMouseMoveMode = iGPressedMouseMoveMode.Normal;
    this.gridValues.ProcessTab = false;
    this.gridValues.RowModeHasCurCell = true;
    this.gridValues.ShowControlsInAllCells = false;
    this.gridValues.SilentValidation = true;
    this.gridValues.CellClick += new iGCellClickEventHandler(this.gridValues_CellClick);
    this.gridValues.CustomDrawCellForeground += new iGCustomDrawCellEventHandler(this.gridValues_CustomDrawCellForeground);
    this.gridValues.CustomDrawCellGetHeight += new iGCustomDrawCellGetHeightEventHandler(this.gridValues_CustomDrawCellGetHeight);
    this.gridValues.ColWidthEndChange += new iGColWidthEventHandler(this.gridValues_ColWidthChanging);
    this.gridValues.ColWidthChanging += new iGColWidthEventHandler(this.gridValues_ColWidthChanging);
    this.gridValues.BeforeCommitEdit += new iGBeforeCommitEditEventHandler(this.gridValues_BeforeCommitEdit);
    this.gridValues.AfterCommitEdit += new iGAfterCommitEditEventHandler(this.gridValues_AfterCommitEdit);
    this.gridValues.Resize += new EventHandler(this.DoResize);
    this.toolBarGrid.AddRemoveButtonsVisible = false;
    this.toolBarGrid.AllowHorizontalDock = false;
    this.toolBarGrid.DockLine = 3;
    this.toolBarGrid.DrawActionsButton = false;
    this.toolBarGrid.FullMenus = true;
    this.toolBarGrid.Guid = new Guid("ba855ba6-35ae-4775-b979-b76ac70a54e0");
    this.toolBarGrid.Hidden = false;
    this.toolBarGrid.ImageList = this.imagesToolbars;
    this.toolBarGrid.Items.AddRange(new ToolbarItemBase[6]
    {
      (ToolbarItemBase) this.btnMode,
      (ToolbarItemBase) this.btnAddOption,
      (ToolbarItemBase) this.btnImport,
      (ToolbarItemBase) this.btnDelete,
      (ToolbarItemBase) this.btnGridExpand,
      (ToolbarItemBase) this.btnGridCollapse
    });
    componentResourceManager.ApplyResources((object) this.toolBarGrid, "toolBarGrid");
    this.toolBarGrid.MinimumFloatingSize = new Size(250, 30);
    this.toolBarGrid.Name = "toolBarGrid";
    this.toolBarGrid.Overflow = ToolBarOverflow.Wrap;
    this.toolBarGrid.Stretch = true;
    this.toolBarGrid.Tearable = false;
    this.imagesToolbars.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imagesToolbars.ImageStream");
    this.imagesToolbars.TransparentColor = Color.Transparent;
    this.imagesToolbars.Images.SetKeyName(0, "ball_green_plus.ico");
    this.imagesToolbars.Images.SetKeyName(1, "down_plus.png");
    this.imagesToolbars.Images.SetKeyName(2, "delete.ico");
    this.imagesToolbars.Images.SetKeyName(3, "Expand.ico");
    this.imagesToolbars.Images.SetKeyName(4, "Collapse.ico");
    this.imagesToolbars.Images.SetKeyName(5, "EventLog2.ico");
    this.imagesToolbars.Images.SetKeyName(6, "image.ico");
    this.imagesToolbars.Images.SetKeyName(7, "unchecked.ico");
    this.imagesToolbars.Images.SetKeyName(8, "checked.ico");
    this.imagesToolbars.Images.SetKeyName(9, "garbage.png");
    this.imagesToolbars.Images.SetKeyName(10, "recycle.png");
    componentResourceManager.ApplyResources((object) this.btnMode, "btnMode");
    this.btnMode.ImageIndex = 5;
    this.btnMode.Items.AddRange(new ToolbarItemBase[2]
    {
      (ToolbarItemBase) this.btnDefault,
      (ToolbarItemBase) this.btnThumbnails
    });
    this.btnMode.ShowText = true;
    this.btnDefault.Checked = true;
    componentResourceManager.ApplyResources((object) this.btnDefault, "btnDefault");
    this.btnDefault.ImageIndex = 5;
    this.btnDefault.ShowText = true;
    this.btnDefault.Click += new EventHandler(this.DoDefaultView);
    componentResourceManager.ApplyResources((object) this.btnThumbnails, "btnThumbnails");
    this.btnThumbnails.ImageIndex = 6;
    this.btnThumbnails.ShowText = true;
    this.btnThumbnails.Click += new EventHandler(this.DoThumbnailsView);
    this.btnAddOption.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.btnAddOption, "btnAddOption");
    this.btnAddOption.ImageIndex = 0;
    this.btnAddOption.Visible = false;
    this.btnAddOption.Click += new EventHandler(this.DoAdd);
    componentResourceManager.ApplyResources((object) this.btnImport, "btnImport");
    this.btnImport.ImageIndex = 1;
    this.btnImport.Visible = false;
    this.btnImport.Click += new EventHandler(this.DoImport);
    componentResourceManager.ApplyResources((object) this.btnDelete, "btnDelete");
    this.btnDelete.ImageIndex = 2;
    this.btnDelete.Visible = false;
    this.btnDelete.Click += new EventHandler(this.DoDelete);
    this.btnGridExpand.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.btnGridExpand, "btnGridExpand");
    this.btnGridExpand.ImageIndex = 3;
    this.btnGridExpand.Visible = false;
    this.btnGridExpand.Click += new EventHandler(this.DoExpand);
    componentResourceManager.ApplyResources((object) this.btnGridCollapse, "btnGridCollapse");
    this.btnGridCollapse.ImageIndex = 4;
    this.btnGridCollapse.Visible = false;
    this.btnGridCollapse.Click += new EventHandler(this.DoCollapse);
    componentResourceManager.ApplyResources((object) this.menuBar, "menuBar");
    this.menuBar.Guid = new Guid("0909a734-928b-4c5d-9a6d-05be64690c06");
    this.menuBar.Hidden = false;
    this.menuBar.ImageList = this.imagesToolbars;
    this.menuBar.Items.AddRange(new ToolbarItemBase[1]
    {
      (ToolbarItemBase) this.contextMenuBarItem
    });
    this.menuBar.Name = "menuBar";
    this.menuBar.OwnerForm = (Form) null;
    componentResourceManager.ApplyResources((object) this.contextMenuBarItem, "contextMenuBarItem");
    this.contextMenuBarItem.Items.AddRange(new ToolbarItemBase[5]
    {
      (ToolbarItemBase) this.mnpAddOption,
      (ToolbarItemBase) this.mnpImport,
      (ToolbarItemBase) this.mnpDelete,
      (ToolbarItemBase) this.mnpExpand,
      (ToolbarItemBase) this.mnpCollapse
    });
    this.contextMenuBarItem.ShowText = true;
    this.mnpAddOption.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.mnpAddOption, "mnpAddOption");
    this.mnpAddOption.ImageIndex = 0;
    this.mnpAddOption.ShowText = true;
    this.mnpAddOption.Click += new EventHandler(this.DoAdd);
    componentResourceManager.ApplyResources((object) this.mnpImport, "mnpImport");
    this.mnpImport.ImageIndex = 1;
    this.mnpImport.ShowText = true;
    this.mnpImport.Click += new EventHandler(this.DoImport);
    componentResourceManager.ApplyResources((object) this.mnpDelete, "mnpDelete");
    this.mnpDelete.ImageIndex = 2;
    this.mnpDelete.ShowText = true;
    this.mnpDelete.Click += new EventHandler(this.DoDelete);
    this.mnpExpand.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.mnpExpand, "mnpExpand");
    this.mnpExpand.ImageIndex = 3;
    this.mnpExpand.ShowText = true;
    this.mnpExpand.Click += new EventHandler(this.DoExpand);
    componentResourceManager.ApplyResources((object) this.mnpCollapse, "mnpCollapse");
    this.mnpCollapse.ImageIndex = 4;
    this.mnpCollapse.ShowText = true;
    this.mnpCollapse.Click += new EventHandler(this.DoCollapse);
    this.splitter.AnimationDelay = 20;
    this.splitter.AnimationStep = 20;
    this.splitter.BorderStyle3D = Border3DStyle.Etched;
    this.splitter.ControlToHide = (Control) this.panelGrid;
    componentResourceManager.ApplyResources((object) this.splitter, "splitter");
    this.splitter.ExpandParentForm = false;
    this.splitter.Name = "splitter";
    this.splitter.TabStop = false;
    this.splitter.UseAnimations = false;
    this.splitter.VisualStyle = VisualStyles.Mozilla;
    this.panelAppEditor.Controls.Add((Control) this.labelDeleteMe);
    componentResourceManager.ApplyResources((object) this.panelAppEditor, "panelAppEditor");
    this.panelAppEditor.Name = "panelAppEditor";
    componentResourceManager.ApplyResources((object) this.labelDeleteMe, "labelDeleteMe");
    this.labelDeleteMe.Name = "labelDeleteMe";
    this.AutoScaleMode = AutoScaleMode.Inherit;
    this.Controls.Add((Control) this.panelAppEditor);
    this.Controls.Add((Control) this.splitter);
    this.Controls.Add((Control) this.panelGrid);
    this.Controls.Add((Control) this.headerControl);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.Name = nameof (RelationOptionsEditor);
    this.panelGrid.ResumeLayout(false);
    ((ISupportInitialize) this.gridValues).EndInit();
    this.panelAppEditor.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  public delegate void ContextChangedEventHandler(object sender, EventArgs e);
}
