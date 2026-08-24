// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.MRP_TechcardRoutesView
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.Bars;
using Intermech.DataFormats;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.MRP;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using Intermech.Navigator.Views;
using Intermech.Navigator.VirtualNodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.MRP.Orders;

/// <summary>Закладка "Маршрут обработки"</summary>
/// <summary>Закладка выбора маршрута обработки</summary>
internal class MRP_TechcardRoutesView : MRP_BaseView
{
  /// <summary>Для регистрации своих категорий;</summary>
  private IGuidMapper guidMapper;
  /// <summary>Дерево Навигатора</summary>
  private NavigatorTreeView tree;
  /// <summary>Полный путь к редактируемому элементу</summary>
  private RelationPath selItem;
  /// <summary>Контейнер редактируемых настроек</summary>
  private ManufactureOrderHolder holder;
  /// <summary>Редактируемые данные</summary>
  private TechnologicalItemSettings settings;
  /// <summary>Пустой корневой дескриптор</summary>
  private IDescriptor emptyDescriptor = (IDescriptor) new HiveDescriptor(0, 0, string.Empty);
  /// <summary>Скрыта ли панель подсказки</summary>
  internal static bool hiddenHintPanel;
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private Intermech.Bars.ToolBar toolBarGrid;
  private ImageList imagesToolbars;
  private ButtonItem btActualize;
  private ButtonItem btRefresh;
  private ChildrenView view;
  private Panel panelHint;
  private RichTextBox edHint;
  private Button btnHideHint;

  /// <summary>Создать экземпляр класса</summary>
  public MRP_TechcardRoutesView()
  {
    this.InitializeComponent();
    this.guidMapper = ServicesManager.GetService(typeof (IGuidMapper)) as IGuidMapper;
    this.ToolbarRendererChanged((object) (ServicesManager.GetService(typeof (BarManager)) as BarManager), EventArgs.Empty);
    this.view.Grid.DynamicFont += new iGDynamicFontEventHandler(this.GridDynamicFontEventHandler);
    this.view.Grid.CellDoubleClick += new iGCellDoubleClickEventHandler(this.GridCellDoubleClickEventHandler);
  }

  /// <summary>
  /// Пришло событие "Изменился рендерер панелей инструментов"
  /// </summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  protected override void ToolbarRendererChanged(object sender, EventArgs e)
  {
    this.toolBarGrid.Renderer = (sender as BarManager).Renderer;
  }

  /// <summary>Заголовок закладки</summary>
  public override string Caption
  {
    [DebuggerStepThrough] get => LocalizationHolder.rm.GetString("MRP_7");
  }

  /// <summary>Порядковый номер закладки</summary>
  public override int OrderID => -6;

  /// <summary>Инициализировать закладку</summary>
  /// <param name="items">Коллекция выделенных элементов пространства навигации</param>
  /// <param name="provider">Контейнер сервисов</param>
  public override void Initialize(ISelectedItems items, IServiceProvider provider)
  {
    base.Initialize(items, provider);
    this.selItem = (RelationPath) null;
  }

  /// <summary>
  /// Активировать закладку (чтение из базы данных, загрузка информации и т.п.)
  /// </summary>
  /// <param name="previousView">Предыдущая закладка</param>
  public override void Activate(IView previousView)
  {
    IViewState service = this._services != null ? this._services.GetService(typeof (IViewState)) as IViewState : (IViewState) null;
    if (service != null)
    {
      long viewState = (long) service.ViewState;
    }
    this.LoadViewData();
  }

  /// <summary>Деактивировать закладку</summary>
  /// <param name="nextView">Следующая закладка</param>
  public override void Deactivate(IView nextView) => base.Deactivate(nextView);

  /// <summary>Инициализация ресурсов закладки</summary>
  public override void InitViewResources()
  {
    base.InitViewResources();
    this._imgView = this._images != null ? this._images.ImageIndex("MRP.imgTechRoutes") : -1;
  }

  /// <summary>Забрать изменения из закладки в контейнер настроек</summary>
  protected override void CaptureChanges()
  {
    RelationPath configuredNodePath = this.tree != null ? NavigatorTreeViewHelper.GetConfiguredNodePath(this.tree.FocusedNode) : (RelationPath) null;
    IDBTypedObjectID itemData1 = this.view.SelectedItems.Count > 0 ? this.view.SelectedItems.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID : (IDBTypedObjectID) null;
    IDBRelationID itemData2 = this.view.SelectedItems.Count > 0 ? this.view.SelectedItems.GetItemData(0, typeof (IDBRelationID)) as IDBRelationID : (IDBRelationID) null;
    TechnologicalItemSettings pathSetting = this.holder == null || configuredNodePath == null || configuredNodePath.Empty ? (TechnologicalItemSettings) null : this.holder.GetPathSetting(configuredNodePath, typeof (TechnologicalItemSettings)) as TechnologicalItemSettings;
    this.settings = this.settings ?? new TechnologicalItemSettings();
    this.settings.RouteObjID = itemData1 != null ? itemData1.ObjectID : 0L;
    this.settings.RouteLinkID = itemData2 != null ? itemData2.Value : 0L;
    if (pathSetting != null)
      pathSetting.Assign((object) this.settings);
    else if (itemData2.Value != 0L && itemData2.Value != -1L)
    {
      TechnologicalItemSettings setting = new TechnologicalItemSettings((object) this.settings);
      this.holder.SetPathSetting(configuredNodePath, (IOrderItemSetting) setting);
    }
    this.RaiseOnChanged();
  }

  /// <summary>
  /// Заполнить содержимое грида на основании указанного дескриптора
  /// </summary>
  /// <param name="descriptor">Дескриптор</param>
  private void LoadFromDescriptor(IDescriptor descriptor)
  {
    descriptor = descriptor ?? this.emptyDescriptor;
    NodeIDPath path = new NodeIDPath(descriptor);
    INode parentNode = (INode) new EtherealNode(descriptor);
    INodeQuery query = parentNode.GetQuery(ContentType.Folders);
    query.Execute((object) null, 1);
    INodeID recordNodeId = query.GetRecordNodeID(0);
    INodeID nodeID = recordNodeId;
    NodeIDPath parentPath = new NodeIDPath(path, nodeID);
    if (parentNode.GetChild(recordNodeId) is IContextAware child)
      child.Services = this.Services;
    this.view.Initialize(parentPath, parentNode, recordNodeId, this.Services);
    this.view.Activate((IView) null);
  }

  /// <summary>
  /// Заполнить элементы управления закладки данными, полученными в методе Initialize
  /// </summary>
  protected override void LoadViewData()
  {
    this.tree = this.Services.GetService(typeof (NavigatorTreeView)) as NavigatorTreeView;
    RelationPath configuredNodePath = this.tree != null ? NavigatorTreeViewHelper.GetConfiguredNodePath(this.tree.FocusedNode) : (RelationPath) null;
    if (this.selItem != null && configuredNodePath != null && this.selItem.Equals((object) configuredNodePath))
      return;
    this.Clear();
    if (this._items == null || this._items.Count == 0)
      return;
    this.tree = this.Services.GetService(typeof (NavigatorTreeView)) as NavigatorTreeView;
    this.selItem = configuredNodePath;
    IDBTypedObjectID itemData = this._items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
    this._items.GetItemData(0, typeof (IDBRelationID));
    this.holder = this.Services != null ? this.Services.GetService(typeof (ManufactureOrderHolder)) as ManufactureOrderHolder : (ManufactureOrderHolder) null;
    TechnologicalItemSettings pathSetting = this.holder == null || this.selItem == null || this.selItem.Empty ? (TechnologicalItemSettings) null : this.holder.GetPathSetting(this.selItem, typeof (TechnologicalItemSettings)) as TechnologicalItemSettings;
    if (this.settings == null)
      this.settings = new TechnologicalItemSettings((object) pathSetting);
    else
      this.settings.Assign((object) pathSetting);
    RoutesDescriptor.CorrectStatics();
    this.LoadFromDescriptor((IDescriptor) new RoutesDescriptor(0, 0, this.Services, this.holder.FiltrationSettings.OwnerID, (List<long>) null, itemData.ObjectID, itemData.ObjectType, RoutesDescriptor.DefaultRelationTypeID, string.Empty, 0L, itemData.Version, itemData.BaseVersion, (List<NodeColumnID>) null));
    this.UpdateControls();
  }

  /// <summary>Выполнить очистку элементов управления в закладке</summary>
  protected override void Clear()
  {
    base.Clear();
    this.tree = (NavigatorTreeView) null;
    this.selItem = (RelationPath) null;
    this.holder = (ManufactureOrderHolder) null;
    this.settings = (TechnologicalItemSettings) null;
  }

  /// <summary>Нажата кнопка "Выбрать маршрут"</summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  private void DoActualize(object sender, EventArgs e)
  {
    this.CaptureChanges();
    this.UpdateControls();
    this.view.Grid.Invalidate();
  }

  /// <summary>Нажата кнопка "Обновить"</summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  private void DoRefresh(object sender, EventArgs e) => this.LoadViewData();

  /// <summary>Управление контролами на закладке</summary>
  protected override void UpdateControls()
  {
    base.UpdateControls();
    iGRow row = this.view.Grid.SelectedCells.Count > 0 ? this.view.Grid.Rows[this.view.Grid.SelectedCells[0].RowIndex] : (iGRow) null;
    RoutesNodeID nodeIdForRow = row != null ? this.view.GetNodeIDForRow(row) as RoutesNodeID : (RoutesNodeID) null;
    this.btActualize.Visible = true;
    this.btActualize.Enabled = nodeIdForRow != null && this.settings != null && this.settings.RouteObjID != nodeIdForRow.ObjectID && this.settings.RouteLinkID != nodeIdForRow.PrjLinkID;
    this.btRefresh.Enabled = true;
    this.btRefresh.Visible = true;
    this.panelHint.Visible = !MRP_TechcardRoutesView.hiddenHintPanel;
  }

  /// <summary>Отпущена клавиша в гриде</summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  private void view_KeyUp(object sender, KeyEventArgs e)
  {
    if (e.KeyCode != Keys.Space)
      return;
    this.DoActualize(sender, (EventArgs) e);
    e.Handled = true;
  }

  /// <summary>Изменились выделенные элементы в гриде</summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  private void view_SelectedItemsChanged(object sender, EventArgs e) => this.UpdateControls();

  /// <summary>
  /// Событие по динамическому изменению шрифта в ячейках грида
  /// </summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  private void GridDynamicFontEventHandler(object sender, iGDynamicFontEventArgs e)
  {
    iGRow row = e.RowIndex >= 0 ? this.view.Grid.Rows[e.RowIndex] : (iGRow) null;
    iGCell cell = e.ColIndex < 0 || row == null ? (iGCell) null : row.Cells[e.ColIndex];
    RoutesNodeID nodeIdForRow = row != null ? this.view.GetNodeIDForRow(row) as RoutesNodeID : (RoutesNodeID) null;
    Font font = (Font) null;
    if (this.settings != null && nodeIdForRow != null && this.settings.RouteObjID == nodeIdForRow.ObjectID && this.settings.RouteLinkID == nodeIdForRow.PrjLinkID)
      font = new Font(e.Font != null ? e.Font : cell.EffectiveFont, FontStyle.Bold);
    e.Font = font;
  }

  /// <summary>Событие по двойному клику в ячейке грида</summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  private void GridCellDoubleClickEventHandler(object sender, iGCellDoubleClickEventArgs e)
  {
    this.DoActualize(sender, (EventArgs) e);
  }

  /// <summary>Нажата кнопка "Скрыть подсказку"</summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы отправителя</param>
  private void DoHideHint(object sender, EventArgs e)
  {
    this.panelHint.Visible = false;
    MRP_TechcardRoutesView.hiddenHintPanel = true;
  }

  /// <summary>Удаление используемых ресурсов</summary>
  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
  protected override void Dispose(bool disposing)
  {
    if (disposing && ServicesManager.GetService(typeof (BarManager)) is BarManager)
      this.toolBarGrid.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  /// <summary>
  /// Required method for Designer support - do not modify
  /// the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MRP_TechcardRoutesView));
    this.toolBarGrid = new Intermech.Bars.ToolBar();
    this.imagesToolbars = new ImageList(this.components);
    this.btActualize = new ButtonItem();
    this.btRefresh = new ButtonItem();
    this.view = new ChildrenView();
    this.panelHint = new Panel();
    this.edHint = new RichTextBox();
    this.btnHideHint = new Button();
    this.panelHint.SuspendLayout();
    this.SuspendLayout();
    this.toolBarGrid.AddRemoveButtonsVisible = false;
    this.toolBarGrid.AllowHorizontalDock = false;
    this.toolBarGrid.DockLine = 3;
    this.toolBarGrid.DrawActionsButton = false;
    this.toolBarGrid.FullMenus = true;
    this.toolBarGrid.Guid = new Guid("ba855ba6-35ae-4775-b979-b76ac70a54e0");
    this.toolBarGrid.Hidden = false;
    this.toolBarGrid.ImageList = this.imagesToolbars;
    this.toolBarGrid.Items.AddRange(new ToolbarItemBase[2]
    {
      (ToolbarItemBase) this.btActualize,
      (ToolbarItemBase) this.btRefresh
    });
    componentResourceManager.ApplyResources((object) this.toolBarGrid, "toolBarGrid");
    this.toolBarGrid.MinimumFloatingSize = new Size(250, 30);
    this.toolBarGrid.Name = "toolBarGrid";
    this.toolBarGrid.Overflow = ToolBarOverflow.Wrap;
    this.toolBarGrid.Stretch = true;
    this.toolBarGrid.Tearable = false;
    this.imagesToolbars.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imagesToolbars.ImageStream");
    this.imagesToolbars.TransparentColor = Color.Transparent;
    this.imagesToolbars.Images.SetKeyName(0, "Маршрут обработки.ico");
    this.imagesToolbars.Images.SetKeyName(1, "refresh.png");
    this.btActualize.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.btActualize, "btActualize");
    this.btActualize.ImageIndex = 0;
    this.btActualize.ShowText = true;
    this.btActualize.Click += new EventHandler(this.DoActualize);
    this.btRefresh.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.btRefresh, "btRefresh");
    this.btRefresh.ImageIndex = 1;
    this.btRefresh.ShowText = true;
    this.btRefresh.Visible = false;
    this.btRefresh.Click += new EventHandler(this.DoRefresh);
    this.view.AllowCustomGroupValues = true;
    this.view.Control = (object) this.view;
    this.view.DisableAutoselectFirstRow = true;
    this.view.DisableColumnsGrouping = true;
    this.view.DisableColumnsSettings = true;
    this.view.DisableContextSearch = true;
    this.view.DisableDelayedUpdates = true;
    this.view.DisableDoubleClicks = true;
    this.view.DisableFiltration = true;
    this.view.DisableGroupBox = true;
    this.view.DisableHeaderContextMenu = true;
    this.view.DisableIMContextMenu = true;
    this.view.DisableKeyDownEvents = false;
    this.view.DisableMultiValuesAttrButton = true;
    this.view.DisablePacketsReading = true;
    this.view.DisableParentSelectedItems = true;
    this.view.DisableStatusBar = true;
    this.view.DisableToolBar = true;
    componentResourceManager.ApplyResources((object) this.view, "view");
    this.view.EmbeddedFocusAndSelection = (iFocusAndSelection) null;
    this.view.Name = "view";
    this.view.SelectedItemsChanged += new EventHandler(this.view_SelectedItemsChanged);
    this.view.KeyUp += new KeyEventHandler(this.view_KeyUp);
    this.panelHint.Controls.Add((Control) this.edHint);
    this.panelHint.Controls.Add((Control) this.btnHideHint);
    componentResourceManager.ApplyResources((object) this.panelHint, "panelHint");
    this.panelHint.Name = "panelHint";
    componentResourceManager.ApplyResources((object) this.edHint, "edHint");
    this.edHint.BackColor = SystemColors.Control;
    this.edHint.Cursor = Cursors.Arrow;
    this.edHint.DetectUrls = false;
    this.edHint.Name = "edHint";
    this.edHint.ReadOnly = true;
    this.edHint.ShortcutsEnabled = false;
    componentResourceManager.ApplyResources((object) this.btnHideHint, "btnHideHint");
    this.btnHideHint.Name = "btnHideHint";
    this.btnHideHint.Tag = (object) "0";
    this.btnHideHint.UseVisualStyleBackColor = true;
    this.btnHideHint.Click += new EventHandler(this.DoHideHint);
    this.AutoScaleMode = AutoScaleMode.Inherit;
    this.Controls.Add((Control) this.view);
    this.Controls.Add((Control) this.panelHint);
    this.Controls.Add((Control) this.toolBarGrid);
    this.MinimumSize = new Size(450, 300);
    this.Name = nameof (MRP_TechcardRoutesView);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.panelHint.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
