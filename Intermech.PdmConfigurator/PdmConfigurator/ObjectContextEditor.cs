// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.ObjectContextEditor
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
using Intermech.Navigator;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.PdmConfigurator;

public sealed class ObjectContextEditor : UserControl
{
  private const string OptionValueCodeColumnKey = "OPTION_VALUE_CODE";
  private static bool _thumbnail = false;
  private static Font _boldFont;
  private ObjectOptionsHolder _holder;
  private PdmConfiguratorContext _context = new PdmConfiguratorContext((PdmConfiguratorContextsCache) null);
  private PdmConfiguratorContext _contextSource = new PdmConfiguratorContext((PdmConfiguratorContextsCache) null);
  private bool _readOnly;
  private bool _inEvents;
  private PdmContextAccessRights _pdmContextAccessRights;
  private INamedImageList _namedImageList;
  private ICategoryTypeIconService _categoryTypeIconService;
  private INavGraphicsCache _navGraphicsCache;
  private ICurrentUserAndRole _currentUserAndRole;
  private IUserNamesCache _userNamesCache;
  private IPicturesCache _picturesCache;
  private NotificationEventHandler _notifyHandler;
  private INotificationService _notificationService;
  private IServiceProvider _serviceProvider;
  private bool _isChanged;
  private static Dictionary<string, int> _colWidthsStd = new Dictionary<string, int>();
  private static Dictionary<string, int> _colWidthsSmall = new Dictionary<string, int>();
  private Dictionary<long, iGRow> _objectRows = new Dictionary<long, iGRow>();
  private ViewStateFlags _state;
  private bool _isOptionValueStatus;
  private Dictionary<long, OptionValueState> _valuesStates = new Dictionary<long, OptionValueState>();
  private static iGCellStyle _cellStyle;
  private static iGCellStyle _cellObligatoryStyle;
  private static iGCellStyle _cellCategoryStyle;
  private static iGCellStyle _cellImage;
  private static iGCellStyle _cellStyleStatus;
  private static iGCellStyle _cellComboBox;
  private static iGCellStyle _cellComboBoxRo;
  private static iGCellStyle _cellValueStatus;
  private static iGColHdrStyle _headerStyle;
  private EventHandler _handlerDoDefaultView;
  private EventHandler _handlerDoThumbnailsView;
  private static StringFormat _imageStringFormat = new StringFormat();
  private IContainer components;
  private Panel panelGrid;
  private iGrid _grid;
  private Intermech.Bars.ToolBar toolBarGrid;
  private DropDownMenuItem btnMode;
  private MenuButtonItem _setViewWithoutThumbnailsMenuButtonItem;
  private MenuButtonItem _setViewWithThumbnailsMenuButtonItem;
  private ToolTip toolTip;
  private ImageList imagesToolbars;
  private Panel panelHint;
  private Label labelWarning;
  private PictureBox pictureHint;
  private ImageList ilError;
  private Panel errorPanel;
  private PictureBox pictureBox1;
  private Label label1;
  private ImageList ilValueStatus;

  public ObjectContextEditor()
  {
    this.InitializeComponent();
    if (ServicesManager.GetService(typeof (BarManager)) is BarManager service)
    {
      service.RendererChanged += new EventHandler(this.BarManager_RendererChanged);
      this.BarManager_RendererChanged((object) service, EventArgs.Empty);
    }
    if (!(ServicesManager.GetService(typeof (IGuidMapper)) is IGuidMapper))
      return;
    this.Init();
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public Dictionary<long, OptionValueState> ValuesStates
  {
    set => this._valuesStates = value;
    get => this._valuesStates;
  }

  public event ObjectContextEditor.ContextChangedEventHandler OnChanged;

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
  public PdmContextAccessRights AccessRights
  {
    [DebuggerStepThrough] get => this._pdmContextAccessRights;
    set
    {
      this._pdmContextAccessRights = value;
      this.UpdateControls();
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public RelationPair ParentKey
  {
    [DebuggerStepThrough] get => this._context.ParentKey;
    set => this._context.ParentKey = value;
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public ObjectOptionsHolder Holder
  {
    [DebuggerStepThrough] get => this._holder;
    set => this._holder = value;
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public PdmConfiguratorContext Context
  {
    get => this._context.Clone() as PdmConfiguratorContext;
    set
    {
      this._context.Assign((object) value);
      this.FillEditor();
      this.Fix();
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public PdmContextType ContextType
  {
    [DebuggerStepThrough] get => this._context.ContextType;
    set
    {
      this._context.ContextType = value;
      this.UpdateControls();
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public IServiceProvider Services
  {
    [DebuggerStepThrough] get => this._serviceProvider;
    set
    {
      this._serviceProvider = value;
      this.InitServices();
    }
  }

  [Category("Appearance")]
  [Browsable(true)]
  public bool IsOptionValueStatus
  {
    get => this._isOptionValueStatus;
    set => this._isOptionValueStatus = value;
  }

  [Category("Appearance")]
  [Browsable(true)]
  public bool IsChanged
  {
    [DebuggerStepThrough] get => this._isChanged;
    set
    {
      this._isChanged = value;
      this.RaiseOnChanged();
      this.UpdateControls();
    }
  }

  public void Init()
  {
    this._namedImageList = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
    this._categoryTypeIconService = ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
    this._navGraphicsCache = ServicesManager.GetService(typeof (INavGraphicsCache)) as INavGraphicsCache;
    this._currentUserAndRole = ServicesManager.GetService(typeof (ICurrentUserAndRole)) as ICurrentUserAndRole;
    this._userNamesCache = CacheManager.Cache("UserNamesCache") as IUserNamesCache;
    this._picturesCache = ServicesManager.GetService(typeof (IPicturesCache)) as IPicturesCache;
    this._notificationService = ServicesManager.GetService(typeof (INotificationService)) as INotificationService;
    if (this._notificationService != null && this._notifyHandler == null)
    {
      this._notifyHandler = new NotificationEventHandler(this.NotificationService_NotificationEventFired);
      this._notificationService.Subscribe(this._notifyHandler);
    }
    INamedImageList namedImageList = this._namedImageList;
    this._isChanged = false;
    this._context.ContextsCache = ServicesManager.GetService(typeof (PdmConfiguratorContextsCache)) as PdmConfiguratorContextsCache;
    this._contextSource.ContextsCache = this._context.ContextsCache;
    bool inEvents = this._inEvents;
    try
    {
      this._inEvents = true;
      this.FillEditor();
      this.SetHandlers();
    }
    finally
    {
      this._inEvents = inEvents;
    }
    this.UpdateControls();
  }

  public PdmContextAccessRights CheckAccessRights(IDBAttributable item)
  {
    PdmContextAccessRights contextAccessRights1 = PdmContextAccessRights.ReadOnly;
    if (item == null)
      return contextAccessRights1;
    IDBAttribute byId = item.Attributes.FindByID(Intermech.Interfaces.PdmConfigurator.Consts.attributeConfiguratorContextID);
    PdmContextAccessRights contextAccessRights2 = byId == null || byId.ReadOnly ? PdmContextAccessRights.ReadOnly : PdmContextAccessRights.FullAccess;
    if (byId != null)
      return contextAccessRights2;
    switch (item)
    {
      case IDBRelation dbRelation:
        IDBObject dbObject1 = dbRelation.Session.GetObject(dbRelation.ProjID, false);
        if (dbObject1 != null)
        {
          try
          {
            dbObject1.CheckRelationsEdit();
            contextAccessRights2 = PdmContextAccessRights.FullAccess;
            break;
          }
          catch
          {
            break;
          }
        }
        else
          break;
      case IDBObject dbObject2:
        if (MetaDataHelper.GetAttribute4ObjectType(dbObject2.ObjectType, Intermech.Interfaces.PdmConfigurator.Consts.attributeConfiguratorContextID) != null)
        {
          try
          {
            dbObject2.CheckEdit();
            contextAccessRights2 = PdmContextAccessRights.FullAccess;
            break;
          }
          catch
          {
            break;
          }
        }
        else
          break;
    }
    return contextAccessRights2;
  }

  public void UpdateControls()
  {
    bool flag = !this._readOnly && (this._pdmContextAccessRights & PdmContextAccessRights.FullAccess) != 0;
    this.btnMode.ShowText = true;
    this._setViewWithoutThumbnailsMenuButtonItem.ShowText = true;
    this._setViewWithThumbnailsMenuButtonItem.ShowText = true;
    this.panelHint.Visible = !flag;
    iGRow curRow = this._grid.CurRow;
    if (curRow == null)
    {
      this.errorPanel.Visible = false;
    }
    else
    {
      object obj = curRow.Cells["ERROR_STATE"].Value;
      this.errorPanel.Visible = obj != null && (ErrorState) obj != ErrorState.None;
    }
  }

  public void Clear()
  {
    this._context.Clear();
    this.FillEditor();
  }

  public void ClearKeys() => this._context.ClearKeys();

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

  public void LoadInfo(
    IServiceProvider services,
    RelationPair key,
    RelationPair parentKey,
    IDBObject obj,
    IDBRelation rel)
  {
    this.Clear();
    if (key == null || key.F_PROJ_ID == 0L || key.Empty)
      return;
    this._serviceProvider = services;
    IViewState service = this._serviceProvider != null ? this._serviceProvider.GetService(typeof (IViewState)) as IViewState : (IViewState) null;
    this._state = service != null ? service.ViewState : ViewStateFlags.None;
    this._grid.AutoResizeCols = false;
    this._grid.AutoWidthColMode = iGAutoWidthColMode.HeaderAndCells;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (key.F_PRJLINK_ID != 0L)
      {
        rel = rel ?? sessionKeeper.Session.GetRelation(key.F_PRJLINK_ID, false);
        this._pdmContextAccessRights = this.CheckAccessRights((IDBAttributable) rel);
        this._context.Key = key;
        this._context.ParentKey = parentKey;
        (this._context.ObjectsOptions.Count > 0 ? this._context.ObjectsOptions[0] : (ObjectOptionsHolder) null)?.LoadOptionsToCache(sessionKeeper.Session);
        this._context.LoadFromObject((IDBAttributable) rel);
        this._context.SyncOptionsList(true);
      }
      else
      {
        PdmConfiguratorContext source = this._context.ContextsCache != null ? this._context.ContextsCache[key] : (PdmConfiguratorContext) null;
        if (source == null && MetaDataHelper.IsPdmConfigurableObjectType(key.F_OBJECT_TYPE))
        {
          source = new PdmConfiguratorContext(ServicesManager.GetService(typeof (PdmConfiguratorContextsCache)) as PdmConfiguratorContextsCache);
          source.Key = key;
          source.ParentKey = parentKey;
          source.Assign((object) (obj ?? sessionKeeper.Session.GetObject(key.F_PROJ_ID, false)));
        }
        this._context.Assign((object) source);
        this._pdmContextAccessRights = PdmContextAccessRights.FullAccess;
      }
    }
    this._context.Key = key;
    this._context.ParentKey = parentKey;
    this.FillEditor();
    this.Fix();
  }

  private void BarManager_RendererChanged(object sender, EventArgs e)
  {
    this.toolBarGrid.Renderer = (sender as BarManager).Renderer;
  }

  private void NotificationService_NotificationEventFired(object sender, NotificationEventArgs e)
  {
  }

  private void SetViewWithoutThumbnailsMenuButtonItem_Click(object sender, EventArgs e)
  {
    if (this._inEvents)
      return;
    ObjectContextEditor._thumbnail = false;
    this.SetHandlers();
    this.FillGrid();
    this.UpdateControls();
  }

  private void SetViewWithThumbnailsMenuButtonItem_Click(object sender, EventArgs e)
  {
    if (this._inEvents)
      return;
    ObjectContextEditor._thumbnail = true;
    this.SetHandlers();
    this.FillGrid();
    this.UpdateControls();
  }

  private void Grid_AfterCommitEdit(object sender, iGAfterCommitEditEventArgs e)
  {
    iGRow row1 = this._grid.Rows[e.RowIndex];
    iGCol col = this._grid.Cols[e.ColIndex];
    if (this._grid.Cols[e.ColIndex].Key != "VALUE")
      return;
    MyElement myElement1 = row1.Cells[e.ColIndex].Value as MyElement;
    if (row1.Cells[e.ColIndex].ColKey == "VALUE")
      row1.Cells["OPTION_VALUE_CODE"].Value = myElement1 == null || !(myElement1.Value is OptionValue) ? (object) (string) null : (object) ((OptionValue) myElement1.Value).Code;
    OptionHolder optionHolder1 = row1.Cells["TAG"].Value as OptionHolder;
    OptionValue optionValue1 = myElement1 != null ? myElement1.Value as OptionValue : (OptionValue) null;
    if (optionValue1 != null)
    {
      this.FillLinkedOptions(optionHolder1.OptionGuid, optionValue1.ID);
      foreach (iGRow row2 in (IEnumerable) this._grid.Rows)
      {
        if (row2.Level != 0)
        {
          OptionHolder optionHolder2 = row2.Cells["TAG"].Value as OptionHolder;
          OptionValue optionValue2 = row2.Cells["VALUE"].Value is MyElement myElement2 ? myElement2.Value as OptionValue : (OptionValue) null;
          if (optionValue2 != null)
          {
            ErrorState errorState = this.IsErrorExists(optionHolder2.OptionGuid, optionValue2.ID);
            row2.Cells["ERROR_STATE"].Value = (object) errorState;
            if (errorState != ErrorState.None)
            {
              row2.Cells["VALUE"].ImageList = this.ilError;
              row2.Cells["VALUE"].ImageIndex = 0;
            }
            else
              row2.Cells["VALUE"].ImageIndex = -1;
          }
        }
      }
    }
    if (this._isOptionValueStatus)
    {
      if (this._valuesStates.ContainsKey(optionHolder1.OptionObjectID))
        this._valuesStates[optionHolder1.OptionObjectID] = OptionValueState.Custom;
      else
        this._valuesStates.Add(optionHolder1.OptionObjectID, OptionValueState.Custom);
      row1.Cells["OPTVALUESTATUS"].ImageIndex = -1;
    }
    this.UpdateControls();
    this.IsChanged = true;
  }

  private void Grid_BeforeCommitEdit(object sender, iGBeforeCommitEditEventArgs e)
  {
    iGRow row = this._grid.Rows[e.RowIndex];
    if (this._grid.Cols[e.ColIndex].Key != "VALUE")
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
      row.Cells["OPTION"].Style = flag ? (optionValue == null ? ObjectContextEditor._cellObligatoryStyle : ObjectContextEditor._cellStyle) : ObjectContextEditor._cellStyle;
      row.Cells["OPTION"].Value = !flag || optionValue != null ? (object) optionHolder.OptionCaption : (object) ("* " + optionHolder.OptionCaption);
      row.AutoHeight();
    }
  }

  private void Grid_CellClick(object sender, iGCellClickEventArgs e)
  {
  }

  private void Grid_CellMouseDown(object sender, iGCellMouseDownEventArgs e)
  {
    if (!(sender is iGrid iGrid) || e.Button != MouseButtons.Right)
      return;
    iGRow row = iGrid.Rows[e.RowIndex];
    iGrid.PerformAction(iGActions.DeselectAll);
    this.iGridSelectRowCells(row, true);
    iGrid.CurRow = row;
  }

  private void Grid_CurRowChanged(object sender, EventArgs e) => this.UpdateControls();

  private void Grid_CustomDrawCellForeground(object sender, iGCustomDrawCellEventArgs e)
  {
    iGCol col = this._grid.Cols[e.ColIndex];
    if (col.Key == "STATUS" || !ObjectContextEditor._thumbnail || col.Key != "IMAGE")
      return;
    object picture = !(this._grid.Rows[e.RowIndex].Cells["OPTVALUE"].Value is OptionValue optionValue) || !(optionValue.Image != Guid.Empty) ? (object) null : this._picturesCache.GetPicture(optionValue.Image);
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
    ThumbnailRenderer.DrawImageObjectAdv(e.Graphics, picture, imageBounds, this._grid.Font, ObjectContextEditor._imageStringFormat);
  }

  private void Grid_CustomDrawCellGetHeight(object sender, iGCustomDrawCellGetHeightEventArgs e)
  {
    iGCol col = this._grid.Cols[e.ColIndex];
    if (col.Key == "STATUS")
    {
      e.Height = ObjectContextEditor._thumbnail ? this._grid.DefaultRow.Height : 20;
    }
    else
    {
      if (!ObjectContextEditor._thumbnail)
        return;
      e.Height = this._grid.DefaultRow.Height;
      if (col.Key != "IMAGE")
        return;
      iGRow row = this._grid.Rows[e.RowIndex];
      OptionValue optionValue = row.Cells["OPTVALUE"].Value as OptionValue;
      object obj = row.Cells["TAG"].Value;
      object picture = optionValue == null || !(optionValue.Image != Guid.Empty) ? (object) null : this._picturesCache.GetPicture(optionValue.Image);
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

  private void Grid_RequestEdit(object sender, iGRequestEditEventArgs e)
  {
    if (this._grid.Cols["VALUE"].Index != e.ColIndex)
      return;
    OptionHolder option = this._grid.Cells[e.RowIndex, "TAG"].Value as OptionHolder;
    IiGDropDownControl dropDownControl = this._grid.Cells[e.RowIndex, e.ColIndex].DropDownControl;
    iGDropDownList iGdropDownList = new iGDropDownList();
    PdmConfiguratorContext context = new PdmConfiguratorContext(ServicesManager.GetService(typeof (PdmConfiguratorContextsCache)) as PdmConfiguratorContextsCache);
    ObjectIncompatibilitiesCollection incompatibilities = this._context.ObjectsOptions[0].Incompatibilities;
    foreach (iGRow row in (IEnumerable) this._grid.Rows)
    {
      if (row.Level != 0 && row.Index != e.RowIndex)
      {
        OptionHolder optionHolder = row.Cells["TAG"].Value as OptionHolder;
        OptionValue optionValue = !(row.Cells["VALUE"].Value is MyElement myElement) ? (OptionValue) null : myElement.Value as OptionValue;
        string str = optionValue == null ? string.Empty : optionValue.ID;
        context[optionHolder.OptionGuid] = str;
      }
    }
    foreach (string optionVisibleValue in this._context.GetOptionVisibleValues(option.OptionGuid))
    {
      context[option.OptionGuid] = optionVisibleValue;
      if ((incompatibilities == null ? 0 : (int) incompatibilities.Evalute(context)) != 9)
      {
        OptionValue optionValue = option.OptionValues.FindValue(optionVisibleValue);
        MyElement myElement = new MyElement((object) optionValue, optionValue.GetDisplayValue(option), (object) -1);
        iGdropDownList.Items.Add((object) myElement);
      }
    }
    if (!this._context.IsObligatoryOption(option.OptionGuid))
    {
      MyElement myElement = new MyElement((object) null, "", (object) string.Empty);
      iGdropDownList.Items.Insert(0, (object) myElement);
    }
    this._grid.Cells[e.RowIndex, e.ColIndex].DropDownControl = (IiGDropDownControl) iGdropDownList;
  }

  private Dictionary<string, int> ColumnWidthDictionary
  {
    get
    {
      return (this._state & ViewStateFlags.NodeUnderTree) == ViewStateFlags.NodeUnderTree ? ObjectContextEditor._colWidthsSmall : ObjectContextEditor._colWidthsStd;
    }
  }

  private void RaiseOnChanged()
  {
    if (this.OnChanged == null)
      return;
    this.OnChanged((object) this, new EventArgs());
  }

  private int GetTypeImageIndex(FieldTypes attrType)
  {
    return this._categoryTypeIconService == null ? -1 : this._categoryTypeIconService.IndexOf(3, -1, (object) attrType);
  }

  private void InitServices()
  {
    IViewState service = this._serviceProvider != null ? this._serviceProvider.GetService(typeof (IViewState)) as IViewState : (IViewState) null;
    this._state = service != null ? service.ViewState : ViewStateFlags.None;
    this._grid.AutoResizeCols = false;
    this._grid.AutoWidthColMode = iGAutoWidthColMode.HeaderAndCells;
    this._grid.Rows.Clear();
    this._grid.Cols.Clear();
    this.PrepareGridsColumns();
    this.UpdateControls();
  }

  private void PrepareGridsStyles()
  {
    if (ObjectContextEditor._boldFont == null)
      ObjectContextEditor._boldFont = new Font(this._grid.Font, FontStyle.Bold);
    if (ObjectContextEditor._cellStyle != null)
      return;
    ObjectContextEditor._cellStyle = new iGCellStyle(true);
    ObjectContextEditor._cellStyle.ReadOnly = iGBool.True;
    ObjectContextEditor._cellStyle.SingleClickEdit = iGBool.False;
    ObjectContextEditor._cellStyle.TextAlign = iGContentAlignment.TopLeft;
    ObjectContextEditor._cellStyle.ImageAlign = iGContentAlignment.TopLeft;
    ObjectContextEditor._cellObligatoryStyle = ObjectContextEditor._cellStyle.Clone();
    ObjectContextEditor._cellObligatoryStyle.ForeColor = Color.Blue;
    ObjectContextEditor._cellCategoryStyle = ObjectContextEditor._cellStyle.Clone();
    ObjectContextEditor._cellCategoryStyle.Font = ObjectContextEditor._boldFont;
    ObjectContextEditor._cellImage = new iGCellStyle(true);
    ObjectContextEditor._cellImage.CustomDrawFlags = iGCustomDrawFlags.Foreground;
    ObjectContextEditor._cellImage.EmptyStringAs = iGEmptyStringAs.EmptyString;
    ObjectContextEditor._cellImage.ReadOnly = iGBool.True;
    ObjectContextEditor._cellImage.SingleClickEdit = iGBool.False;
    ObjectContextEditor._cellImage.TextAlign = iGContentAlignment.TopCenter;
    ObjectContextEditor._cellImage.ValueType = typeof (Image);
    ObjectContextEditor._cellStyleStatus = ObjectContextEditor._cellImage.Clone();
    ObjectContextEditor._cellStyleStatus.ImageAlign = iGContentAlignment.TopCenter;
    ObjectContextEditor._cellComboBox = new iGCellStyle(true);
    ObjectContextEditor._cellComboBox.EmptyStringAs = iGEmptyStringAs.EmptyString;
    ObjectContextEditor._cellComboBox.ReadOnly = iGBool.False;
    ObjectContextEditor._cellComboBox.SingleClickEdit = iGBool.True;
    ObjectContextEditor._cellComboBox.TextAlign = iGContentAlignment.TopLeft;
    ObjectContextEditor._cellComboBox.ValueType = typeof (string);
    ObjectContextEditor._cellComboBox.Type = iGCellType.Combo;
    ObjectContextEditor._cellComboBox.TypeFlags = iGCellTypeFlags.ComboPreferValue;
    ObjectContextEditor._cellComboBoxRo = ObjectContextEditor._cellComboBox.Clone();
    ObjectContextEditor._cellComboBoxRo.ReadOnly = iGBool.True;
    ObjectContextEditor._cellComboBoxRo.SingleClickEdit = iGBool.False;
    ObjectContextEditor._cellComboBoxRo.ForeColor = SystemColors.GrayText;
    ObjectContextEditor._cellValueStatus = new iGCellStyle(true);
    ObjectContextEditor._cellValueStatus.ReadOnly = iGBool.True;
    ObjectContextEditor._cellValueStatus.ImageList = this.ilValueStatus;
    ObjectContextEditor._headerStyle = new iGColHdrStyle(true);
    ObjectContextEditor._headerStyle.TextAlign = iGContentAlignment.TopLeft;
  }

  private void PrepareGridsColumns()
  {
    int num1 = this._context.Key.Empty ? 1 : 0;
    int num2 = this._readOnly ? 0 : ((this._pdmContextAccessRights & PdmContextAccessRights.FullAccess) != 0 ? 1 : 0);
    this.PrepareGridsStyles();
    this._grid.Header.ImageList = this._categoryTypeIconService.ImageList;
    if (ObjectContextEditor._colWidthsStd.Count == 0)
    {
      ObjectContextEditor._colWidthsStd.Add("OPTION", 256 /*0x0100*/);
      ObjectContextEditor._colWidthsStd.Add("IMAGE", 48 /*0x30*/);
      ObjectContextEditor._colWidthsStd.Add("VALUE", 256 /*0x0100*/);
      ObjectContextEditor._colWidthsStd.Add("OPTION_VALUE_CODE", 128 /*0x80*/);
      ObjectContextEditor._colWidthsStd.Add("NOTE", 128 /*0x80*/);
      ObjectContextEditor._colWidthsStd.Add("OPTVALUESTATUS", 18);
      ObjectContextEditor._colWidthsStd.Add("STATUS", 48 /*0x30*/);
      ObjectContextEditor._colWidthsStd.Add("TAG", 0);
      ObjectContextEditor._colWidthsStd.Add("OPTVALUE", 0);
      ObjectContextEditor._colWidthsStd.Add("CATEGORY", 0);
      ObjectContextEditor._colWidthsStd.Add("OBLIGATORY", 0);
    }
    if (ObjectContextEditor._colWidthsSmall.Count == 0)
    {
      ObjectContextEditor._colWidthsSmall.Add("OPTION", 128 /*0x80*/);
      ObjectContextEditor._colWidthsSmall.Add("IMAGE", 32 /*0x20*/);
      ObjectContextEditor._colWidthsSmall.Add("VALUE", 128 /*0x80*/);
      ObjectContextEditor._colWidthsSmall.Add("OPTION_VALUE_CODE", 128 /*0x80*/);
      ObjectContextEditor._colWidthsSmall.Add("NOTE", 0);
      ObjectContextEditor._colWidthsSmall.Add("STATUS", 0);
      ObjectContextEditor._colWidthsSmall.Add("TAG", 0);
      ObjectContextEditor._colWidthsSmall.Add("OPTVALUE", 0);
      ObjectContextEditor._colWidthsSmall.Add("CATEGORY", 0);
      ObjectContextEditor._colWidthsSmall.Add("OBLIGATORY", 0);
    }
    this.ColumnWidthDictionary["OPTION"] = Math.Max(this.ColumnWidthDictionary["OPTION"], 32 /*0x20*/);
    iGCol iGcol1 = this._grid.Cols["OPTION"] ?? this._grid.Cols.Add(new iGColPattern(this.ColumnWidthDictionary["OPTION"], true, true, 32 /*0x20*/, -1, true, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_37"), "OPTION", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol1.Width = this.ColumnWidthDictionary["OPTION"];
    iGcol1.ColHdrStyle = ObjectContextEditor._headerStyle;
    iGcol1.CellStyle = ObjectContextEditor._cellStyle;
    this.ColumnWidthDictionary["IMAGE"] = Math.Min(Math.Max(this.ColumnWidthDictionary["IMAGE"], 32 /*0x20*/), 1024 /*0x0400*/);
    iGCol iGcol2 = this._grid.Cols["IMAGE"] ?? this._grid.Cols.Add(new iGColPattern(this.ColumnWidthDictionary["IMAGE"], true, true, 32 /*0x20*/, 1024 /*0x0400*/, true, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_38"), "IMAGE", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol2.CellStyle = ObjectContextEditor._cellImage;
    iGcol2.Width = this.ColumnWidthDictionary["IMAGE"];
    iGcol2.ColHdrStyle = ObjectContextEditor._headerStyle;
    iGcol2.Visible = ObjectContextEditor._thumbnail;
    this.ColumnWidthDictionary["OPTVALUESTATUS"] = Math.Min(Math.Max(this.ColumnWidthDictionary["OPTVALUESTATUS"], 0), 18);
    if (this._isOptionValueStatus)
    {
      iGCol iGcol3 = this._grid.Cols["OPTVALUESTATUS"] ?? this._grid.Cols.Add(new iGColPattern(this.ColumnWidthDictionary["OPTVALUESTATUS"], true, false, 0, 18, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) "", "OPTVALUESTATUS", -1, (object) null, (object) null, -1));
      iGcol3.Width = this.ColumnWidthDictionary["OPTVALUESTATUS"];
      iGcol3.CellStyle = ObjectContextEditor._cellValueStatus;
    }
    this.ColumnWidthDictionary["VALUE"] = Math.Max(this.ColumnWidthDictionary["VALUE"], 32 /*0x20*/);
    iGCol iGcol4 = this._grid.Cols["VALUE"] ?? this._grid.Cols.Add(new iGColPattern(this.ColumnWidthDictionary["VALUE"], true, true, 32 /*0x20*/, -1, !ObjectContextEditor._thumbnail, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_39"), "VALUE", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol4.CellStyle = ObjectContextEditor._cellStyle;
    iGcol4.Width = this.ColumnWidthDictionary["VALUE"];
    iGcol4.ColHdrStyle = ObjectContextEditor._headerStyle;
    this.ColumnWidthDictionary["OPTION_VALUE_CODE"] = Math.Max(this.ColumnWidthDictionary["OPTION_VALUE_CODE"], 0);
    iGCol iGcol5 = this._grid.Cols["OPTION_VALUE_CODE"] ?? this._grid.Cols.Add(new iGColPattern(this.ColumnWidthDictionary["OPTION_VALUE_CODE"], true, true, 0, -1, true, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) "Код значения опции", "OPTION_VALUE_CODE", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol5.CellStyle = ObjectContextEditor._cellStyle;
    iGcol5.Width = this.ColumnWidthDictionary["OPTION_VALUE_CODE"];
    iGcol5.ColHdrStyle = ObjectContextEditor._headerStyle;
    this.ColumnWidthDictionary["NOTE"] = Math.Max(this.ColumnWidthDictionary["NOTE"], 0);
    iGCol iGcol6 = this._grid.Cols["NOTE"] ?? this._grid.Cols.Add(new iGColPattern(this.ColumnWidthDictionary["NOTE"], (this._state & ViewStateFlags.NodeUnderTree) == ViewStateFlags.None, true, 0, -1, true, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_40"), "NOTE", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol6.Visible = (this._state & ViewStateFlags.NodeUnderTree) == ViewStateFlags.None;
    iGcol6.CellStyle = ObjectContextEditor._cellStyle;
    iGcol6.Width = this.ColumnWidthDictionary["NOTE"];
    iGcol6.ColHdrStyle = ObjectContextEditor._headerStyle;
    this.ColumnWidthDictionary["STATUS"] = Math.Max(this.ColumnWidthDictionary["STATUS"], 0);
    iGCol iGcol7 = this._grid.Cols["STATUS"] ?? this._grid.Cols.Add(new iGColPattern(this.ColumnWidthDictionary["STATUS"], true, false, 0, -1, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_41"), "STATUS", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol7.Visible = (this._state & ViewStateFlags.NodeUnderTree) == ViewStateFlags.None;
    iGcol7.CellStyle = ObjectContextEditor._cellStyleStatus;
    iGcol7.Width = this.ColumnWidthDictionary["STATUS"];
    iGcol7.ColHdrStyle = ObjectContextEditor._headerStyle;
    (this._grid.Cols["TAG"] ?? this._grid.Cols.Add(new iGColPattern(this.ColumnWidthDictionary["TAG"], false, false, 0, 1024 /*0x0400*/, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) "", "TAG", -1, (object) null, (object) null, -1))).Width = this.ColumnWidthDictionary["TAG"];
    (this._grid.Cols["OPTVALUE"] ?? this._grid.Cols.Add(new iGColPattern(this.ColumnWidthDictionary["OPTVALUE"], false, false, 0, 1024 /*0x0400*/, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) "", "OPTVALUE", -1, (object) null, (object) null, -1))).Width = this.ColumnWidthDictionary["OPTVALUE"];
    (this._grid.Cols["CATEGORY"] ?? this._grid.Cols.Add(new iGColPattern(this.ColumnWidthDictionary["CATEGORY"], false, false, 0, 1024 /*0x0400*/, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) "", "CATEGORY", -1, (object) null, (object) null, -1))).Width = this.ColumnWidthDictionary["CATEGORY"];
    (this._grid.Cols["OBLIGATORY"] ?? this._grid.Cols.Add(new iGColPattern(this.ColumnWidthDictionary["OBLIGATORY"], false, false, 0, 1024 /*0x0400*/, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) "", "OBLIGATORY", -1, (object) null, (object) null, -1))).Width = this.ColumnWidthDictionary["OBLIGATORY"];
    if (this._grid.Cols["ERROR_STATE"] != null)
      return;
    this._grid.Cols.Add(new iGColPattern(10, false, false, 0, 10, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) "", "ERROR_STATE", -1, (object) null, (object) null, -1));
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
    iGRow iGrow = this._grid.Rows.Add();
    this._objectRows.Add(category, iGrow);
    iGrow.Cells["CATEGORY"].ValueType = typeof (OptionObjectDescription);
    iGrow.Cells["CATEGORY"].Value = (object) category1;
    iGrow.Cells["OPTION"].Value = (object) category1.CAPTION;
    iGrow.Cells["OPTION"].ImageList = this._categoryTypeIconService.ImageList;
    iGrow.Cells["OPTION"].ImageIndex = this._categoryTypeIconService.IndexOf(4, Intermech.Interfaces.PdmConfigurator.Consts.objtypeOptionsGroupID);
    iGrow.Cells["OPTION"].Style = ObjectContextEditor._cellCategoryStyle;
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
    if (index1 < this._grid.Rows.Count - 1)
    {
      while (index1 < this._grid.Rows.Count - 1)
      {
        ++index1;
        iGRow row = this._grid.Rows[index1];
        if (row.Cells["CATEGORY"].Value is OptionObjectDescription objectDescription2 && objectDescription1 != null && objectDescription1.F_OBJECT_ID == objectDescription2.F_OBJECT_ID)
          iGrow2 = row;
        else
          break;
      }
    }
    iGRow iGrow3 = this._grid.Rows.Insert(iGrow2.Index + 1);
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
    iGrow3.Cells["OPTION"].Style = flag ? (myElement1 == null ? ObjectContextEditor._cellObligatoryStyle : ObjectContextEditor._cellStyle) : ObjectContextEditor._cellStyle;
    iGrow3.Cells["OPTION"].ImageList = this._categoryTypeIconService.ImageList;
    iGrow3.Cells["OPTION"].ImageIndex = this._categoryTypeIconService.IndexOf(4, Intermech.Interfaces.PdmConfigurator.Consts.objtypeOptionID);
    iGrow3.Cells["IMAGE"].Value = (object) (optionValue1 != null ? optionValue1.Image : Guid.Empty);
    iGrow3.Cells["VALUE"].Style = this._pdmContextAccessRights == PdmContextAccessRights.FullAccess ? ObjectContextEditor._cellComboBox.Clone() : ObjectContextEditor._cellComboBoxRo.Clone();
    iGrow3.Cells["VALUE"].Value = (object) (myElement1 ?? (!flag ? myElement2 : (MyElement) null));
    iGrow3.Cells["VALUE"].DropDownControl = (IiGDropDownControl) iGdropDownList;
    ErrorState errorState = this.IsErrorExists(option, id);
    iGrow3.Cells["ERROR_STATE"].Value = (object) errorState;
    if (errorState != ErrorState.None)
    {
      iGrow3.Cells["VALUE"].ImageList = this.ilError;
      iGrow3.Cells["VALUE"].ImageIndex = 0;
    }
    else
      iGrow3.Cells["VALUE"].ImageIndex = -1;
    iGrow3.Cells["VALUE"].DropDownControl.SelectedItem = (object) (myElement1 ?? (!flag ? myElement2 : (MyElement) null));
    iGrow3.Cells["OPTION_VALUE_CODE"].Value = myElement1 == null || !(myElement1.Value is OptionValue) ? (object) (string) null : (object) ((OptionValue) myElement1.Value).Code;
    iGrow3.Cells["NOTE"].Value = optionValue1 != null ? (object) optionValue1.Description : (object) string.Empty;
    iGrow3.Cells["TAG"].Value = (object) option1;
    iGrow3.Cells["OPTVALUE"].Value = (object) optionValue1;
    iGrow3.Cells["CATEGORY"].Value = (object) objectDescription1;
    iGrow3.Cells["OBLIGATORY"].Value = (object) flag;
    if (this._isOptionValueStatus)
    {
      int num = -1;
      if (this._valuesStates.ContainsKey(option1.OptionObjectID))
        num = (int) this._valuesStates[option1.OptionObjectID];
      else if (this._context.ObjectsOptions[0].VisibleOptionValues.DefaultValues.ContainsKey(option1.OptionGuid))
      {
        string defaultValue = this._context.ObjectsOptions[0].VisibleOptionValues.DefaultValues[option1.OptionGuid];
        if (id == defaultValue)
        {
          num = 1;
          if (this._valuesStates.ContainsKey(option1.OptionObjectID))
            this._valuesStates[option1.OptionObjectID] = OptionValueState.Default;
          else
            this._valuesStates.Add(option1.OptionObjectID, OptionValueState.Default);
        }
      }
      iGrow3.Cells["OPTVALUESTATUS"].ImageIndex = num;
    }
    return iGrow3;
  }

  private void FillGrid()
  {
    bool inEvents = this._inEvents;
    try
    {
      this._inEvents = true;
      this._objectRows.Clear();
      this._grid.Rows.Clear();
      this.PrepareGridsColumns();
      if (this._context.OptionsValues.Count == 0)
        return;
      List<Guid> sortedOptionsList = this._context.GetSortedOptionsList();
      for (int index = 0; index < sortedOptionsList.Count; ++index)
        this.AddOptionValue(sortedOptionsList[index], this._context.OptionsValues[sortedOptionsList[index]]);
      this._grid.Rows.AutoHeight();
    }
    finally
    {
      this._inEvents = inEvents;
    }
  }

  private void FillEditor()
  {
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
    this.UpdateControls();
    this.RaiseOnChanged();
  }

  private void SetHandlers()
  {
    if (this._handlerDoDefaultView == null)
    {
      this._handlerDoDefaultView = new EventHandler(this.SetViewWithoutThumbnailsMenuButtonItem_Click);
      this._handlerDoThumbnailsView = new EventHandler(this.SetViewWithThumbnailsMenuButtonItem_Click);
    }
    if (ObjectContextEditor._thumbnail)
    {
      this.btnMode.Text = this._setViewWithThumbnailsMenuButtonItem.Text;
      this.btnMode.ToolTipText = this._setViewWithThumbnailsMenuButtonItem.ToolTipText;
      this.btnMode.ImageIndex = this._setViewWithThumbnailsMenuButtonItem.ImageIndex;
      this.btnMode.Click -= this._handlerDoThumbnailsView;
      this.btnMode.Click -= this._handlerDoDefaultView;
      this.btnMode.Click += this._handlerDoDefaultView;
      this._setViewWithoutThumbnailsMenuButtonItem.Checked = false;
      this._setViewWithThumbnailsMenuButtonItem.Checked = true;
    }
    else
    {
      this.btnMode.Text = this._setViewWithoutThumbnailsMenuButtonItem.Text;
      this.btnMode.ToolTipText = this._setViewWithoutThumbnailsMenuButtonItem.ToolTipText;
      this.btnMode.ImageIndex = this._setViewWithoutThumbnailsMenuButtonItem.ImageIndex;
      this.btnMode.Click -= this._handlerDoDefaultView;
      this.btnMode.Click -= this._handlerDoThumbnailsView;
      this.btnMode.Click += this._handlerDoThumbnailsView;
      this._setViewWithoutThumbnailsMenuButtonItem.Checked = true;
      this._setViewWithThumbnailsMenuButtonItem.Checked = false;
    }
  }

  private void iGridSelectRowCells(iGRow row, bool select)
  {
    if (row == null)
      return;
    for (int colIndex = 0; colIndex < row.Cells.Count; ++colIndex)
      row.Cells[colIndex].Selected = select;
  }

  private void FillLinkedOptions(Guid optionGuid, string valueID)
  {
    Dictionary<OptionValuePair, List<OptionValuePair>> linearList = this._context.ObjectsOptions[0].Incompatibilities.LinkedOptions.CreateLinearList(new OptionValuePair(optionGuid, valueID));
    int level = 0;
    while (true)
    {
      Dictionary<OptionValuePair, List<OptionValuePair>> dictionary = this.SameLevelOption(linearList, level);
      if (dictionary.Count != 0)
      {
        foreach (OptionValuePair key in dictionary.Keys)
        {
          OptionValuePair sameOption = this.FindSameOption(linearList, key, level);
          if (sameOption != null)
          {
            List<OptionValuePair> currentPath1 = linearList[key];
            currentPath1.Add(key);
            List<OptionValuePair> currentPath2 = linearList[sameOption];
            currentPath2.Add(sameOption);
            string errorMessage = string.Empty;
            using (SessionKeeper sessionKeeper = new SessionKeeper())
              errorMessage = LinkedOptions.FormingPathString(sessionKeeper.Session, currentPath1) + Environment.NewLine + LinkedOptions.FormingPathString(sessionKeeper.Session, currentPath2);
            int num = (int) ConflictForm.ShowErrorDialog(errorMessage);
            return;
          }
        }
        foreach (OptionValuePair key in dictionary.Keys)
        {
          foreach (iGRow row in (IEnumerable) this._grid.Rows)
          {
            if (row.Level != 0)
            {
              OptionHolder option = row.Cells["TAG"].Value as OptionHolder;
              if (option.OptionGuid == key.Option)
              {
                OptionValue optionValue = option.OptionValues.FindValue(key.ID);
                if (optionValue != null)
                {
                  MyElement myElement = new MyElement((object) optionValue, optionValue.GetDisplayValue(option), (object) -1);
                  row.Cells["VALUE"].Value = (object) myElement;
                  row.Cells["OPTVALUE"].Value = (object) optionValue;
                  row.Cells["NOTE"].Value = (object) optionValue.Description;
                  this._context.OptionsValues[option.OptionGuid] = optionValue.ID;
                  if (this._isOptionValueStatus)
                  {
                    if (optionGuid != option.OptionGuid)
                    {
                      if (this._valuesStates.ContainsKey(option.OptionObjectID))
                        this._valuesStates[option.OptionObjectID] = OptionValueState.Linked;
                      else
                        this._valuesStates.Add(option.OptionObjectID, OptionValueState.Linked);
                      row.Cells["OPTVALUESTATUS"].ImageIndex = 0;
                      break;
                    }
                    break;
                  }
                  break;
                }
                break;
              }
            }
          }
        }
        ++level;
      }
      else
        break;
    }
  }

  private Dictionary<OptionValuePair, List<OptionValuePair>> SameLevelOption(
    Dictionary<OptionValuePair, List<OptionValuePair>> allDict,
    int level)
  {
    Dictionary<OptionValuePair, List<OptionValuePair>> dictionary = new Dictionary<OptionValuePair, List<OptionValuePair>>();
    foreach (OptionValuePair key in allDict.Keys)
    {
      List<OptionValuePair> optionValuePairList = allDict[key];
      if (optionValuePairList.Count == level)
        dictionary.Add(key, optionValuePairList);
    }
    return dictionary;
  }

  private OptionValuePair FindSameOption(
    Dictionary<OptionValuePair, List<OptionValuePair>> dict,
    OptionValuePair pair,
    int level)
  {
    foreach (OptionValuePair key in dict.Keys)
    {
      if (key.Option == pair.Option && key.ID != pair.ID && dict[key].Count <= level)
        return key;
    }
    return (OptionValuePair) null;
  }

  private ErrorState IsErrorExists(Guid optionGuid, string id)
  {
    ObjectIncompatibilitiesCollection incompatibilities = this._context.ObjectsOptions[0].Incompatibilities;
    return incompatibilities != null && incompatibilities.Evalute(this._context) == PdmConfiguratorResult.Incompatibles ? ErrorState.IncompConflict : ErrorState.None;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && ServicesManager.GetService(typeof (BarManager)) is BarManager service)
    {
      this.toolBarGrid.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
      service.RendererChanged -= new EventHandler(this.BarManager_RendererChanged);
    }
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ObjectContextEditor));
    this.panelGrid = new Panel();
    this._grid = new iGrid();
    this.errorPanel = new Panel();
    this.label1 = new Label();
    this.pictureBox1 = new PictureBox();
    this.panelHint = new Panel();
    this.labelWarning = new Label();
    this.pictureHint = new PictureBox();
    this.toolBarGrid = new Intermech.Bars.ToolBar();
    this.imagesToolbars = new ImageList(this.components);
    this.btnMode = new DropDownMenuItem();
    this._setViewWithoutThumbnailsMenuButtonItem = new MenuButtonItem();
    this._setViewWithThumbnailsMenuButtonItem = new MenuButtonItem();
    this.toolTip = new ToolTip(this.components);
    this.ilError = new ImageList(this.components);
    this.ilValueStatus = new ImageList(this.components);
    this.panelGrid.SuspendLayout();
    ((ISupportInitialize) this._grid).BeginInit();
    this.errorPanel.SuspendLayout();
    ((ISupportInitialize) this.pictureBox1).BeginInit();
    this.panelHint.SuspendLayout();
    ((ISupportInitialize) this.pictureHint).BeginInit();
    this.SuspendLayout();
    this.panelGrid.Controls.Add((Control) this._grid);
    this.panelGrid.Controls.Add((Control) this.errorPanel);
    this.panelGrid.Controls.Add((Control) this.panelHint);
    this.panelGrid.Controls.Add((Control) this.toolBarGrid);
    componentResourceManager.ApplyResources((object) this.panelGrid, "panelGrid");
    this.panelGrid.Name = "panelGrid";
    this._grid.DefaultAutoGroupRow.Height = 20;
    this._grid.DefaultRow.Height = (int) componentResourceManager.GetObject("resource.Height");
    this._grid.DefaultRow.NormalCellHeight = (int) componentResourceManager.GetObject("resource.NormalCellHeight");
    componentResourceManager.ApplyResources((object) this._grid, "_grid");
    this._grid.GridLines.GroupRows = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this._grid.GridLines.Horizontal = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this._grid.GridLines.HorizontalExtended = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this._grid.GridLines.HorizontalLastRow = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this._grid.GridLines.Vertical = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this._grid.GridLines.VerticalExtended = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this._grid.GridLines.VerticalLastCol = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this._grid.GroupBox.Text = componentResourceManager.GetString("gridValues.GroupBox.Text");
    this._grid.Header.Height = (int) componentResourceManager.GetObject("gridValues.Header.Height");
    this._grid.HighlightBackColorNoFocus = SystemColors.Highlight;
    this._grid.HotTracking = false;
    this._grid.Name = "_grid";
    this._grid.PressedMouseMoveMode = iGPressedMouseMoveMode.Normal;
    this._grid.ProcessTab = false;
    this._grid.RowModeHasCurCell = true;
    this._grid.ShowControlsInAllCells = false;
    this._grid.SilentValidation = true;
    this._grid.CellMouseDown += new iGCellMouseDownEventHandler(this.Grid_CellMouseDown);
    this._grid.CellClick += new iGCellClickEventHandler(this.Grid_CellClick);
    this._grid.CustomDrawCellForeground += new iGCustomDrawCellEventHandler(this.Grid_CustomDrawCellForeground);
    this._grid.CustomDrawCellGetHeight += new iGCustomDrawCellGetHeightEventHandler(this.Grid_CustomDrawCellGetHeight);
    this._grid.CurRowChanged += new EventHandler(this.Grid_CurRowChanged);
    this._grid.RequestEdit += new iGRequestEditEventHandler(this.Grid_RequestEdit);
    this._grid.BeforeCommitEdit += new iGBeforeCommitEditEventHandler(this.Grid_BeforeCommitEdit);
    this._grid.AfterCommitEdit += new iGAfterCommitEditEventHandler(this.Grid_AfterCommitEdit);
    this.errorPanel.Controls.Add((Control) this.label1);
    this.errorPanel.Controls.Add((Control) this.pictureBox1);
    componentResourceManager.ApplyResources((object) this.errorPanel, "errorPanel");
    this.errorPanel.Name = "errorPanel";
    componentResourceManager.ApplyResources((object) this.label1, "label1");
    this.label1.Name = "label1";
    componentResourceManager.ApplyResources((object) this.pictureBox1, "pictureBox1");
    this.pictureBox1.Name = "pictureBox1";
    this.pictureBox1.TabStop = false;
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
    this.toolBarGrid.AddRemoveButtonsVisible = false;
    this.toolBarGrid.AllowHorizontalDock = false;
    this.toolBarGrid.DockLine = 3;
    this.toolBarGrid.DrawActionsButton = false;
    this.toolBarGrid.FullMenus = true;
    this.toolBarGrid.Guid = new Guid("ba855ba6-35ae-4775-b979-b76ac70a54e0");
    this.toolBarGrid.Hidden = false;
    this.toolBarGrid.ImageList = this.imagesToolbars;
    this.toolBarGrid.Items.AddRange(new ToolbarItemBase[1]
    {
      (ToolbarItemBase) this.btnMode
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
      (ToolbarItemBase) this._setViewWithoutThumbnailsMenuButtonItem,
      (ToolbarItemBase) this._setViewWithThumbnailsMenuButtonItem
    });
    this.btnMode.ShowText = true;
    this._setViewWithoutThumbnailsMenuButtonItem.Checked = true;
    componentResourceManager.ApplyResources((object) this._setViewWithoutThumbnailsMenuButtonItem, "_setViewWithoutThumbnailsMenuButtonItem");
    this._setViewWithoutThumbnailsMenuButtonItem.ImageIndex = 5;
    this._setViewWithoutThumbnailsMenuButtonItem.ShowText = true;
    this._setViewWithoutThumbnailsMenuButtonItem.Click += new EventHandler(this.SetViewWithoutThumbnailsMenuButtonItem_Click);
    componentResourceManager.ApplyResources((object) this._setViewWithThumbnailsMenuButtonItem, "_setViewWithThumbnailsMenuButtonItem");
    this._setViewWithThumbnailsMenuButtonItem.ImageIndex = 6;
    this._setViewWithThumbnailsMenuButtonItem.ShowText = true;
    this._setViewWithThumbnailsMenuButtonItem.Click += new EventHandler(this.SetViewWithThumbnailsMenuButtonItem_Click);
    this.ilError.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("ilError.ImageStream");
    this.ilError.TransparentColor = Color.Transparent;
    this.ilError.Images.SetKeyName(0, "error.gif");
    this.ilError.Images.SetKeyName(1, "garbage.png");
    this.ilError.Images.SetKeyName(2, "gear_warning.png");
    this.ilValueStatus.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("ilValueStatus.ImageStream");
    this.ilValueStatus.TransparentColor = Color.Transparent;
    this.ilValueStatus.Images.SetKeyName(0, "gears.png");
    this.ilValueStatus.Images.SetKeyName(1, "checked.ico");
    this.AutoScaleMode = AutoScaleMode.Inherit;
    this.Controls.Add((Control) this.panelGrid);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.Name = nameof (ObjectContextEditor);
    this.panelGrid.ResumeLayout(false);
    ((ISupportInitialize) this._grid).EndInit();
    this.errorPanel.ResumeLayout(false);
    ((ISupportInitialize) this.pictureBox1).EndInit();
    this.panelHint.ResumeLayout(false);
    ((ISupportInitialize) this.pictureHint).EndInit();
    this.ResumeLayout(false);
  }

  public delegate void ContextChangedEventHandler(object sender, EventArgs e);
}
