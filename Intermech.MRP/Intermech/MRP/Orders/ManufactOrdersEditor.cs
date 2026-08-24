// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.ManufactOrdersEditor
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using ImSSP;
using Infralution.Controls.VirtualTree;
using Intermech.Bars;
using Intermech.Client.Core;
using Intermech.Client.Core.ObjectCreator;
using Intermech.DataFormats;
using Intermech.Docking;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.Contexts;
using Intermech.Interfaces.MRP;
using Intermech.Interfaces.PdmConfigurator;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.NavBars;
using Intermech.Navigator;
using Intermech.Navigator.ContextCommands;
using Intermech.Navigator.Controls;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using Intermech.Search.Mrp;
using Intermech.Search.Pdm.Analogs;
using Intermech.Search.Utilities;
using Intermech.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MRP.Orders;

/// <summary>
/// Редактор состава производственного заказа. Страничка мастера, позволяющая формировать
/// состав заказа, проверять его на наличие ошибок, управлять допустимыми заменами,
/// выбирать маршруты обработки, т.п.
/// </summary>
internal class ManufactOrdersEditor : UserControl, IDisposable
{
  /// <summary>Режим работы - только чтение</summary>
  private bool _readOnly;
  /// <summary>Запрет на обработку некоторых событий</summary>
  private bool _inEvents;
  /// <summary>
  /// Надо ли использовать события в текущем правиле отображения и сортировки составов
  /// </summary>
  private bool _useEvents;
  /// <summary>
  /// Сервис, позволяющий клиентским плагинам передавать какую-о информацию на сторону сервера
  /// </summary>
  private IClientPluginsService _clientPluginsService;
  /// <summary>
  /// Интерфейс сервиса по управлению тулбаром "Фильтрация состава" в главной форме приложения
  /// </summary>
  private IFiltrationService _filtrationService;
  /// <summary>Текущий пользователь и его роль</summary>
  private ICurrentUserAndRole _currentUserAndRole;
  /// <summary>Контейнер сервисов</summary>
  private AdvancedServiceContainer _services = new AdvancedServiceContainer();
  /// <summary>Контейнер сервисов для дерева</summary>
  private AdvancedServiceContainer _navigatorTreeViewServiceContainer;
  /// <summary>Контейнер сервисов для менеджера закладок</summary>
  private AdvancedServiceContainer _pagesViewManagerServiceContainer;
  /// <summary>
  /// Заготовка вновь созданного объекта, для которого вызвана форма
  /// </summary>
  private CreatedObjectItem _createdObjectItem;
  /// <summary>
  /// Коллекция выделенных элементов, для которых вызвана форма
  /// </summary>
  private ISelectedItems _selectedItems;
  /// <summary>
  /// Сервис локальной службы уведомлений (для текущего окна)
  /// </summary>
  private NotificationService _notificationService;
  /// <summary>
  /// Сервис глобальной службы уведомлений (для всего IMClient)
  /// </summary>
  private INotificationService _mainNotificationService;
  /// <summary>Настройки MRP</summary>
  private IMRPSettings _mrpSettings;
  /// <summary>Есть ли изменения в редакторе</summary>
  private bool _isChanged;
  /// <summary>Есть ли ошибки в редакторе</summary>
  private bool _errorsInEditor;
  /// <summary>Родительский объект (производственный заказ)</summary>
  private IDBTypedObjectID _projObject;
  /// <summary>
  /// Класс, который управляет дополнительными параметрами запросов
  /// </summary>
  private ManufactOrdersEditor.MOClientPluginsDataTransfer _moClientPluginsDataTransfer;
  /// <summary>
  /// Контейнер с информацией о редактируемом производственном заказе
  /// </summary>
  internal ManufactureOrderHolder _manufactureOrderHolder = new ManufactureOrderHolder();
  /// <summary>
  /// Контейнер с информацией о редактируемом производственном заказе: исходные данные
  /// </summary>
  private ManufactureOrderHolder _sourceManufactureOrderHolder = new ManufactureOrderHolder();
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private ImageList imagesToolbars;
  private ImageList imagesTabs;
  private ToolTip toolTips;
  private ImageList ilState;
  private SplitContainer splitContainerMain;
  private SplitContainer splitMain;
  private ManufactOrdersEditorNavigatorTreeView _tree;
  private Intermech.Bars.ToolBar toolBarTree;
  private ButtonItem _addButton;
  private ButtonItem _deleteButton;
  private HeaderControl headerControl;
  private HeaderControl headerControlPages;
  private HeaderControl _topHeaderControl;
  private PageControl pagesTop;
  private Intermech.Docking.TabPage tabVersionsRule;
  private PictureBox pictureRule;
  private LinkLabel _changeVersionsRuleLinkLabel;
  private Button _setDefaultVersionsRuleButton;
  private Button _setCurrentVersionsRuleButton;
  private Button _changeVersionsRuleButton;
  private Intermech.Docking.TabPage tabEditingContext;
  private PictureBox pictureContext;
  private LinkLabel _selectEditingContextLinkLabel;
  private Button _clearEditingContextButton;
  private Button _setCurrentEditingContextButton;
  private Button _selectEditingContextButton;
  private Intermech.Docking.TabPage tabCompositionContexts;
  private PictureBox pictureCompositionContexts;
  private CheckedListBox _compositionContextsCheckedListBox;
  private Button _setDefaultCompositionContextsButton;
  private ButtonItem _refreshButton;
  private Bevel bevelSettings;
  private PageViewsManager _pageViewsManager;
  internal TreeViewsBridge treeViewsBridge;
  private HeaderControl _compositionTracingHeaderControl;
  private CompositionTracing _compositionTracing;
  private Panel panelInfo;
  private PictureBox pictureBox1;
  private Label labelInfo;
  private Intermech.Docking.TabPage tabPage1;
  private SeriesDatesSelectingControl _seriesDatesSelectingControl;
  private Intermech.Docking.TabPage tabPage2;
  private Intermech.Search.Mrp.AnalogSelectionModeSelectionControl _analogSelectionModeSelectionControl;

  /// <summary>Создать экземпляр класса</summary>
  public ManufactOrdersEditor()
  {
    this.InitializeComponent();
    if (ServicesManager.GetService(typeof (ManufactOrdersEditor)) is ManufactOrdersEditor)
      ServicesManager.RemoveService(typeof (ManufactOrdersEditor));
    ServicesManager.AddService(typeof (ManufactOrdersEditor), (object) this);
    if (ServicesManager.GetService(typeof (IColumnSchemes)) == null)
      return;
    this._tree.OnGetSupportedColumnsEventHandler += new GetSupportedColumnsEventHandler(Intermech.Navigator.Utils.GetNavigatorColumns);
  }

  /// <summary>Создать экземпляр класса</summary>
  /// <param name="caption">Заголовок формы</param>
  /// <param name="createdObject">Заготовка созданного объекта, для которого вызвана форма (вместо selectedItems)</param>
  /// <param name="selectedItems">Коллекция выделенных элементов, для которых вызвана форма (вместо createdObject)</param>
  /// <param name="viewServices">Контейнер сервисов</param>
  public ManufactOrdersEditor(
    string caption,
    CreatedObjectItem createdObject,
    ISelectedItems selectedItems,
    IServiceProvider viewServices)
    : this()
  {
    if (ServicesManager.GetService(typeof (BarManager)) is BarManager service)
    {
      service.RendererChanged += new EventHandler(this.BarManager_RendererChanged);
      this.BarManager_RendererChanged((object) service, EventArgs.Empty);
    }
    if (ServicesManager.GetService(typeof (IGuidMapper)) is IGuidMapper)
      this.Init(caption, createdObject, selectedItems, viewServices);
    else
      this.UpdateControls();
  }

  /// <summary>
  /// Событие возникает, если в редакторе происходят изменения
  /// </summary>
  public event ManufactOrdersChangedEventHandler Changed;

  /// <summary>Сгенерировать событие "OnChanged"</summary>
  internal virtual void RaiseOnChanged()
  {
    if (this.Changed == null)
      return;
    this.Changed((object) this, EventArgs.Empty);
  }

  /// <summary>Событие возникает, если в редакторе возникла ошибка</summary>
  public event ManufactOrdersErrorsInEditorEventHandler ErrorsInEditor;

  /// <summary>Есть ли изменения в редакторе</summary>
  [Category("Appearance")]
  [Browsable(true)]
  public virtual bool IsChanged
  {
    [DebuggerStepThrough] get => this._isChanged;
    set
    {
      this._isChanged = value;
      this.UpdateControls();
    }
  }

  /// <summary>Есть ли ошибки в редакторе</summary>
  [Category("Appearance")]
  [Browsable(true)]
  public virtual bool HasErrorsInEditor
  {
    get
    {
      bool flag = false;
      if (this._compositionTracing.TaskStatuses != null)
      {
        foreach (KeyValuePair<CompositionObject, PdmCompositionBrowserJobStatus> taskStatuse in this._compositionTracing.TaskStatuses)
        {
          flag = taskStatuse.Value == null;
          if (flag)
            break;
        }
      }
      if (this._errorsInEditor || this._compositionTracing.InProgress || this._compositionTracing.ErrorsInEditor)
        return true;
      return this.HasComposition && ((this._compositionTracing.TaskStatuses == null || this._compositionTracing.TaskStatuses.Count == 0 ? 1 : (this._compositionTracing.IsTerminated ? 1 : 0)) | (flag ? 1 : 0)) != 0;
    }
    set
    {
      this._errorsInEditor = value;
      this.UpdateControls();
    }
  }

  /// <summary>Отображать ли заголовок в редакторе</summary>
  [Category("Appearance")]
  [Browsable(true)]
  public virtual bool HeaderVisiblity
  {
    [DebuggerStepThrough] get => this._topHeaderControl.Visible;
    set => this._topHeaderControl.Visible = value;
  }

  /// <summary>Первый выделенный в дереве объект</summary>
  [Browsable(false)]
  public IDBTypedObjectID SelectedObject
  {
    get
    {
      ISelectedItems selectedItems = this._tree.SelectedItems;
      return selectedItems != null && selectedItems.Count > 0 ? selectedItems.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID : (IDBTypedObjectID) null;
    }
  }

  /// <summary>Первая выделенная в дереве связь</summary>
  [Browsable(false)]
  public IDBRelationID SelectedRelation
  {
    get
    {
      ISelectedItems selectedItems = this._tree.SelectedItems;
      return selectedItems != null && selectedItems.Count > 0 ? selectedItems.GetItemData(0, typeof (IDBRelationID)) as IDBRelationID : (IDBRelationID) null;
    }
  }

  /// <summary>Выделенная связь как элемент состава</summary>
  [Browsable(false)]
  [Obsolete("После перехода на использование ядра Навигатора данное свойство использовать не рекомендуется")]
  public OrderItem SelectedOrderItem
  {
    get
    {
      IDBRelationID selectedRelation = this.SelectedRelation;
      IDBTypedObjectID selectedObject = this.SelectedObject;
      if (selectedRelation == null || selectedRelation.Value == 0L || selectedRelation.Value == -1L || selectedObject == null)
        return (OrderItem) null;
      if (this._manufactureOrderHolder.RelSettings.ContainsKey(selectedRelation.Value))
        return this._manufactureOrderHolder.RelSettings[selectedRelation.Value];
      using (SessionKeeper sessionKeeper = new SessionKeeper())
        return CompositionObjectsDescriptionsHelper.LoadDescription(sessionKeeper.Session, typeof (OrderItem), selectedObject.ObjectID, selectedRelation.Value) as OrderItem;
    }
  }

  /// <summary>Проверить наличие состава у редактируемого заказа</summary>
  [Browsable(false)]
  public bool HasComposition
  {
    get
    {
      return this._tree.RootNode != null && this._tree.RootNode.Children != null && this._tree.RootNode.Children.Count > 0;
    }
  }

  /// <summary>Выполнить инициализацию компонента</summary>
  /// <param name="caption">Заголовок формы</param>
  /// <param name="createdObject">Заготовка созданного объекта, для которого вызвана форма (вместо selectedItems)</param>
  /// <param name="selectedItems">Коллекция выделенных элементов, для которых вызвана форма (вместо createdObject)</param>
  /// <param name="viewServices">Контейнер сервисов</param>
  public void Init(
    string caption,
    CreatedObjectItem createdObject,
    ISelectedItems selectedItems,
    IServiceProvider viewServices)
  {
    this._mainNotificationService = ServicesManager.GetService(typeof (INotificationService)) as INotificationService;
    this._mrpSettings = ServicesManager.GetService(typeof (IMRPSettings)) as IMRPSettings;
    this._notificationService = this.InitializeNotificationService();
    this._currentUserAndRole = ServicesManager.GetService(typeof (ICurrentUserAndRole)) as ICurrentUserAndRole;
    if (this._currentUserAndRole != null)
    {
      this._useEvents = this._currentUserAndRole.UseRuleEvents;
      this._currentUserAndRole.UseRuleEvents = true;
    }
    this._manufactureOrderHolder.Merge(viewServices != null ? viewServices.GetService(typeof (MRPOrderItemsSettingsHolder)) as MRPOrderItemsSettingsHolder : (MRPOrderItemsSettingsHolder) null);
    this._createdObjectItem = createdObject;
    this._selectedItems = selectedItems;
    this._services.AddService(typeof (ManufactOrdersEditor), (object) this);
    this._services.AddService(typeof (ManufactureOrderHolder), (object) this._manufactureOrderHolder);
    this._services.AddService(typeof (INotificationService), (object) this._notificationService);
    this._services.AdvancedProvider = viewServices;
    this._services.AddService(typeof (NavigatorTreeView), (object) this._tree);
    this._services.AddService(typeof (IViewsManager), (object) this._pageViewsManager);
    this._navigatorTreeViewServiceContainer = new AdvancedServiceContainer((IServiceProvider) this._services);
    this._navigatorTreeViewServiceContainer.AddService(typeof (IViewState), (object) new ViewStateService(ViewStateFlags.InDialog | ViewStateFlags.NodeInTree));
    this._pagesViewManagerServiceContainer = new AdvancedServiceContainer((IServiceProvider) this._services);
    this._pagesViewManagerServiceContainer.AddService(typeof (IViewState), (object) new ViewStateService(ViewStateFlags.InDialog | ViewStateFlags.NodeInViews));
    this.EnableNotifications((INotificationService) this._notificationService, true);
    this._clientPluginsService = ServicesManager.GetService(typeof (IClientPluginsService)) as IClientPluginsService;
    this._filtrationService = ServicesManager.GetService(typeof (IFiltrationService)) as IFiltrationService;
    this.InitPluginsDataPlugin();
    this._tree.Services = (IServiceProvider) this._navigatorTreeViewServiceContainer;
    this._pageViewsManager.Services = (IServiceProvider) this._pagesViewManagerServiceContainer;
    this._isChanged = false;
    this._projObject = this._createdObjectItem != null ? (IDBTypedObjectID) new DBTypedObjectID(this._createdObjectItem.ObjectTypeID, this._createdObjectItem.ObjectID, 0L, string.Empty, 0L, 0L, 0L, string.Empty, 0L) : (IDBTypedObjectID) null;
    if (this._projObject == null)
      this._projObject = selectedItems == null || selectedItems.Count <= 0 ? (IDBTypedObjectID) null : selectedItems.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
    if (this._projObject != null)
      this._services.AddService(typeof (IDBTypedObjectID), (object) this._projObject);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      PdmConfiguratorCache.CacheLoadCategories(sessionKeeper.Session);
      PdmConfiguratorCache.CacheLoadOptions(sessionKeeper.Session);
    }
    this.CreateFiltrationSettings();
    this.Reload();
  }

  /// <summary>Обновить информацию в редакторе</summary>
  public void RefreshEditor(bool initializeCompositionTracing = true)
  {
    IViewPage activeViewPage = this._pageViewsManager.ActiveViewPage;
    try
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        PdmConfiguratorObjectOptionsCache.ResetExpired();
        if (sessionKeeper.Session.GetCustomService(typeof (IPdmConfiguratorService)) is IPdmConfiguratorService customService)
          customService.ResetSessionCache((object) sessionKeeper.Session.SessionGUID);
      }
      this.CreateFiltrationSettings();
      this.FillVersionsRule();
      this.FillEditingContext();
      this.FillCompositionContexts();
      KeyValuePair<long, OrderItem> keyValuePair = this._manufactureOrderHolder.RelSettings.FirstOrDefault<KeyValuePair<long, OrderItem>>((Func<KeyValuePair<long, OrderItem>, bool>) (o => o.Value.Settings.Any<IOrderItemSetting>((Func<IOrderItemSetting, bool>) (oo => oo is MovingItemSettings))));
      IDescriptor descriptor = this._projObject == null || RelationHelper.IsUnknownRelationID(keyValuePair.Key) ? (this._projObject != null ? (IDescriptor) new ManufacturingOrderBlankDescriptor(this._projObject.ObjectID) : (IDescriptor) new Intermech.Navigator.DBObjects.Descriptor()) : (IDescriptor) new ManufactOrdersEditor.ReplaceProductDescriptor(this._projObject.ObjectID, keyValuePair.Key);
      bool backgroundTreeTasks = OptimizationSettings.BackgroundTreeTasks;
      try
      {
        OptimizationSettings.BackgroundTreeTasks = false;
        NodeIDPath focusedPath = this._tree.FocusedPath;
        this._tree.BuildWithPath(descriptor, focusedPath);
        this.CreateFiltrationSettings();
        if (initializeCompositionTracing)
          this.InitializeCompositionTracing();
      }
      finally
      {
        OptimizationSettings.BackgroundTreeTasks = backgroundTreeTasks;
      }
      this.UpdateControls();
    }
    finally
    {
      if (activeViewPage != null)
        this._pageViewsManager.ActiveViewPage = activeViewPage;
    }
  }

  /// <summary>Очистить редактор</summary>
  public void Clear()
  {
    this._manufactureOrderHolder.Clear();
    this.Reload();
    this.Fix();
  }

  /// <summary>
  /// Зафиксировать изменения в редакторе
  /// (в базу данных при этом ничего не вносится)
  /// </summary>
  public void Fix()
  {
    this._sourceManufactureOrderHolder.Assign((object) this._manufactureOrderHolder);
    this._isChanged = false;
    this.UpdateControls();
    this.RaiseOnChanged();
  }

  /// <summary>
  /// Отменить изменения в редакторе
  /// (в базу данных при этом ничего не вносится)
  /// </summary>
  public void Undo()
  {
    this._manufactureOrderHolder.Assign((object) this._sourceManufactureOrderHolder);
    this._isChanged = false;
    this.Reload();
    this.RaiseOnChanged();
  }

  /// <summary>Начать трассировку составов</summary>
  internal void TraceStart()
  {
    this._compositionTracing.TraceStart();
    this._errorsInEditor = true;
    this.UpdateControls();
  }

  private void BarManager_RendererChanged(object sender, EventArgs e)
  {
    this.toolBarTree.Renderer = (sender as BarManager).Renderer;
  }

  private void TopHeaderControl_Click(object sender, EventArgs e)
  {
    this.pagesTop.Visible = !this.pagesTop.Visible;
    this.bevelSettings.Visible = !this.pagesTop.Visible;
    if (this.bevelSettings.Visible)
      this.bevelSettings.BringToFront();
    this.UpdateControls();
  }

  private void ChangeVersionsRuleLinkLabel_Click(object sender, EventArgs e)
  {
    if (this._inEvents || this._readOnly)
      return;
    if (this._manufactureOrderHolder == null || this._manufactureOrderHolder.FiltrationSettings == null || this._manufactureOrderHolder.FiltrationSettings.RuleID == 0L || this._manufactureOrderHolder.FiltrationSettings.CurrentRule == null)
    {
      this.ChangeVersionsRuleButton_Click(sender, e);
    }
    else
    {
      int num = (int) PropertiesWindow.Execute(this._manufactureOrderHolder.FiltrationSettings.CurrentRule.RuleObjectCaption, this._manufactureOrderHolder.FiltrationSettings.CurrentRule.RuleObjectCaption, this._manufactureOrderHolder.FiltrationSettings.CurrentRule.RuleObjectID);
    }
  }

  private void ChangeVersionsRuleButton_Click(object sender, EventArgs e)
  {
    if (this._inEvents || this._readOnly)
      return;
    long[] numArray = VersionRulesSelectionForm.Execute(VersionRulesSelectFilter.vrfExcludeVariableRules | VersionRulesSelectFilter.vrfExcludeAllVersionsRule, false, LocalizationHolder.rm.GetString("MRP_12"));
    if (numArray == null || numArray.Length == 0)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this._manufactureOrderHolder.FiltrationSettings.CurrentRule = (sessionKeeper.Session.GetCustomService(typeof (IVersionRulesCacheService)) as IVersionRulesCacheService)[(object) sessionKeeper.Session.SessionGUID, numArray[0]];
    this.RefreshEditor();
    this.IsChanged = true;
  }

  private void SetCurrentVersionsRuleButton_Click(object sender, EventArgs e)
  {
    if (this._inEvents || this._readOnly)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this._manufactureOrderHolder.FiltrationSettings.CurrentRule = (sessionKeeper.Session.GetCustomService(typeof (IVersionRulesCacheService)) as IVersionRulesCacheService)[(object) sessionKeeper.Session.SessionGUID, this._filtrationService.FiltrationRuleID];
    this.RefreshEditor();
    this.IsChanged = true;
  }

  private void SetDefaultVersionsRuleButton_Click(object sender, EventArgs e)
  {
    if (this._inEvents || this._readOnly)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this._manufactureOrderHolder.FiltrationSettings.CurrentRule = (sessionKeeper.Session.GetCustomService(typeof (IVersionRulesCacheService)) as IVersionRulesCacheService).GetDefaultVersionRule(sessionKeeper.Session.SessionGUID);
    this.RefreshEditor();
    this.IsChanged = true;
  }

  private void SelectEditingContextLinkLabel_Click(object sender, EventArgs e)
  {
    if (this._inEvents || this._readOnly)
      return;
    if (this._manufactureOrderHolder == null || this._manufactureOrderHolder.FiltrationSettings == null || this._manufactureOrderHolder.FiltrationSettings.EditingContext == null || this._manufactureOrderHolder.FiltrationSettings.EditingContext.ContextID == 0L)
    {
      this.SelectEditingContextButton_Click(sender, e);
    }
    else
    {
      string str = string.Empty;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(this._manufactureOrderHolder.FiltrationSettings.EditingContext.ContextID);
        if (objectInfo.Empty)
          return;
        str = objectInfo.Caption;
      }
      int num = (int) PropertiesWindow.Execute(str, str, this._manufactureOrderHolder.FiltrationSettings.EditingContext.ContextID);
    }
  }

  private void SelectEditingContextButton_Click(object sender, EventArgs e)
  {
    if (this._inEvents || this._readOnly)
      return;
    DescriptorCollection descriptors = new DescriptorCollection();
    List<int> contextTopObjectsIds = MetaDataHelper.GetEditingContextTopObjectsIDs();
    for (int index = 0; index < contextTopObjectsIds.Count; ++index)
      descriptors.Add((IDescriptor) new Intermech.Navigator.DBObjectTypes.Descriptor(contextTopObjectsIds[index]));
    long[] numArray = Intermech.Navigator.SelectionWindow.SelectObjects(LocalizationHolder.rm.GetString("MRP_14"), LocalizationHolder.rm.GetString("MRP_15"), (IDescriptor) new Intermech.Navigator.CustomNode.Descriptor(LocalizationHolder.rm.GetString("MRP_16"), descriptors), SelectionOptions.Default | SelectionOptions.DisableSelectAbstractTypes | SelectionOptions.DisableMultiselect);
    if (numArray == null || numArray.Length == 0)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBEditingContextsObject editingContextsObject = sessionKeeper.Session.GetObject(numArray[0]) as IDBEditingContextsObject;
      this._manufactureOrderHolder.FiltrationSettings.EditingContext = new CurrentEditingContext(numArray[0], editingContextsObject.LinkedContextNumber, EditingContextMode.Default);
    }
    this.RefreshEditor();
    this.IsChanged = true;
  }

  private void SetCurrentEditingContextButton_Click(object sender, EventArgs e)
  {
    if (this._inEvents || this._readOnly || this._manufactureOrderHolder.FiltrationSettings.EditingContext.ContextID == this._currentUserAndRole.CachedEditingContextID)
      return;
    if (this._currentUserAndRole.CachedEditingContextID == 0L)
    {
      this.ClearEditingContextButton_Click(sender, e);
    }
    else
    {
      this._manufactureOrderHolder.FiltrationSettings.EditingContext = new CurrentEditingContext(this._currentUserAndRole.CachedEditingContextID, this._currentUserAndRole.CachedEditingContextModificationID, EditingContextMode.Default);
      this.RefreshEditor();
      this.IsChanged = true;
    }
  }

  private void ClearEditingContextButton_Click(object sender, EventArgs e)
  {
    if (this._inEvents || this._readOnly)
      return;
    this._manufactureOrderHolder.FiltrationSettings.EditingContext = CurrentEditingContext.Empty;
    this.RefreshEditor();
    this.IsChanged = true;
  }

  private void CompositionContextsCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
  {
    if (this._inEvents || this._readOnly || this._manufactureOrderHolder == null)
      return;
    IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(new Guid("cad00651-306c-11d8-b4e9-00304f19f545"));
    if (attributeType == null || attributeType.PossibleValues == null || attributeType.PossibleValues.Count <= e.Index)
      return;
    long int64Value = DataSetProcessor.GetInt64Value(attributeType.PossibleValues[e.Index], -1L);
    this._manufactureOrderHolder.CompositionContexts.Remove(int64Value);
    if (e.NewValue == CheckState.Checked)
      this._manufactureOrderHolder.CompositionContexts.Add(int64Value);
    this.RefreshEditor();
    this.IsChanged = true;
  }

  private void SetDefaultCompositionContextButton_Click(object sender, EventArgs e)
  {
    if (this._inEvents || this._readOnly || this._manufactureOrderHolder == null)
      return;
    this._manufactureOrderHolder.CompositionContexts = new List<long>((IEnumerable<long>) new long[2]
    {
      0L,
      3L
    });
    this.RefreshEditor();
    this.IsChanged = true;
  }

  private void SeriesDatesSelectingControl_Changed(object sender, EventArgs e)
  {
    if (this._inEvents || this._readOnly || this._manufactureOrderHolder == null)
      return;
    this._manufactureOrderHolder.SeriesDateSettingsHolder = this._seriesDatesSelectingControl.SeriesDateSettingsHolder;
    this.RefreshEditor();
    this.IsChanged = true;
  }

  private void AnalogSelectionModeSelectionControl_AnalogSelectionModeChanged(
    object sender,
    EventArgs e)
  {
    if (this._inEvents || this._readOnly || this._manufactureOrderHolder == null)
      return;
    this._manufactureOrderHolder.AnalogSelectionMode = this._analogSelectionModeSelectionControl.AnalogSelectionMode;
    this.RefreshEditor();
    this.IsChanged = true;
  }

  private void AddButton_Click(object sender, EventArgs e)
  {
    if (this._readOnly || this._projObject == null || this._projObject.ObjectID == 0L)
      return;
    List<int> typesForComposition = ObjectCommands.GetObjectTypesForComposition(this._projObject.ObjectID, (IServiceProvider) this._services);
    if (typesForComposition == null || typesForComposition.Count == 0)
      return;
    int objectTypeId = MetaDataHelper.GetObjectTypeID(new Guid("cad00583-306c-11d8-b4e9-00304f19f545"));
    typesForComposition.Remove(objectTypeId);
    DescriptorCollection descriptors = new DescriptorCollection();
    typesForComposition.ForEach((Action<int>) (type => descriptors.Add((IDescriptor) new Intermech.Navigator.DBObjectTypes.Descriptor(type))));
    object[] objArray = Intermech.Navigator.SelectionWindow.Select(LocalizationHolder.rm.GetString("MRP_17"), LocalizationHolder.rm.GetString(sc_14789.ssp_mrp_14790()), (IDescriptor) new Intermech.Navigator.CustomNode.Descriptor(Intermech.Navigator.Consts.CategoryCustomNode, 1, LocalizationHolder.rm.GetString("MRP_19"), descriptors), typeof (IDBTypedObjectID), SelectionOptions.Default | SelectionOptions.ForceFilterObjectsByRule);
    if (objArray == null || objArray.Length == 0)
      return;
    List<IDBTypedObjectID> dbTypedObjectIdList = new List<IDBTypedObjectID>(objArray.Length);
    for (int index = 0; index < objArray.Length; ++index)
      dbTypedObjectIdList.Add(objArray[index] as IDBTypedObjectID);
    ObjectCommands.DoInsertIntoObject(this._tree.GetNodeIDPath(this._tree.RootNode), this._projObject, dbTypedObjectIdList.ToArray(), (IDBRelationID[]) null, (Hashtable) null, false, (IServiceProvider) this._services, NavigatorRelationCommand.InsertIn);
    this.InitializeCompositionTracing();
    this.IsChanged = true;
    if (ObjectCommands.insertIncluded.Count <= 7)
    {
      for (int index = 0; index < ObjectCommands.insertIncluded.Count; ++index)
      {
        long prjLinkID = ObjectCommands.insertIncluded[index];
        long objectId = dbTypedObjectIdList[index].ObjectID;
        string caption = string.Empty;
        using (SessionKeeper sessionKeeper = new SessionKeeper())
          caption = sessionKeeper.Session.GetObjectInfo(dbTypedObjectIdList[index].ObjectID).Caption;
        if (prjLinkID != 0L)
        {
          int num = (int) QuantityFm.Execute(prjLinkID, caption);
        }
      }
    }
    this.UpdateControls();
    if (this.IsChanged)
      this.RaiseOnChanged();
    if (!this.HasErrorsInEditor)
      return;
    this.RaiseOnErrorsInEditor();
  }

  private void DeleteButton_Click(object sender, EventArgs e)
  {
    if (this._readOnly || this._projObject == null || this._projObject.ObjectID == 0L)
      return;
    ISelectedItems selectedItems = this._tree.SelectedItems;
    if (selectedItems == null || selectedItems.Count == 0)
      return;
    List<long> longList = new List<long>(selectedItems.Count);
    for (int index = 0; index < selectedItems.Count; ++index)
    {
      if (selectedItems.GetItemData(index, typeof (IDBRelationID)) is IDBRelationID itemData && itemData.ProjID == this._projObject.ObjectID && itemData.Value != 0L && longList.IndexOf(itemData.Value) < 0)
        longList.Add(itemData.Value);
    }
    if (longList.Count == 0)
      return;
    List<long> relationIDs = new List<long>(longList.Count);
    List<long> projIDs = new List<long>(longList.Count);
    List<int> relTypeIDs = new List<int>(longList.Count);
    List<long> objectIDs = new List<long>();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      try
      {
        sessionKeeper.Session.StartLogHistory();
        for (int index = 0; index < longList.Count; ++index)
        {
          IDBRelation relation = sessionKeeper.Session.GetRelation(longList[index], false);
          if (relation != null)
          {
            relation.Delete(0L);
            relationIDs.Add(relation.RelationID);
            projIDs.Add(relation.ProjID);
            relTypeIDs.Add(relation.RelationType);
          }
        }
      }
      finally
      {
        foreach (CategoryValue modificationsHistory in sessionKeeper.Session.GetModificationsHistoryList())
        {
          if (modificationsHistory.CategoryType == 1 && (modificationsHistory.ActionID == ActionType.Delete || modificationsHistory.ActionID == ActionType.Purge) && objectIDs.IndexOf(modificationsHistory.CategoryID) < 0)
            objectIDs.Add(modificationsHistory.CategoryID);
        }
        sessionKeeper.Session.StopLogHistory();
        if (relationIDs.Count > sc_14789.ssp_mrp_14791(421175914))
          this._notificationService.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsRemoved", (IList<long>) relationIDs, (IList<long>) projIDs, (IList<int>) null, (IList<int>) relTypeIDs));
        if (objectIDs.Count > 0)
          this._notificationService.FireEvent((object) null, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsRemoved", (IList<long>) objectIDs));
      }
    }
    this.InitializeCompositionTracing();
    this.IsChanged = true;
    this.UpdateControls();
    if (this.IsChanged)
      this.RaiseOnChanged();
    if (!this.HasErrorsInEditor)
      return;
    this.RaiseOnErrorsInEditor();
  }

  private void RefreshButton_Click(object sender, EventArgs e)
  {
    this.RefreshEditor();
    if (this.IsChanged)
      this.RaiseOnChanged();
    if (!this.HasErrorsInEditor)
      return;
    this.RaiseOnErrorsInEditor();
  }

  private void Tree_BuildTree(object sender, EventArgs e)
  {
    long ruleId = this._manufactureOrderHolder == null || this._manufactureOrderHolder.FiltrationSettings == null ? 0L : this._manufactureOrderHolder.FiltrationSettings.RuleID;
    long contextId = this._manufactureOrderHolder == null || this._manufactureOrderHolder.FiltrationSettings == null ? 0L : this._manufactureOrderHolder.FiltrationSettings.EditingContext.ContextID;
    ISelectedItems selectedItems = this._tree.SelectedItems;
    List<long> longList = new List<long>();
    if (selectedItems != null && selectedItems.Count > 0)
    {
      for (int index = 0; index < selectedItems.Count; ++index)
      {
        if (selectedItems.GetItemData(index, typeof (IDBRelationID)) is IDBRelationID itemData && itemData.ProjID == this._projObject.ObjectID && itemData.Value != 0L && longList.IndexOf(itemData.Value) < 0)
          longList.Add(itemData.Value);
      }
    }
    this._topHeaderControl.Text = this.pagesTop.Visible ? LocalizationHolder.rm.GetString("MRP_25") : LocalizationHolder.rm.GetString("MRP_24");
    this._setCurrentVersionsRuleButton.Enabled = !this._readOnly && this._filtrationService != null && this._filtrationService.FiltrationRuleID != ruleId;
    this._setDefaultVersionsRuleButton.Enabled = !this._readOnly;
    this._changeVersionsRuleButton.Enabled = this._setDefaultVersionsRuleButton.Enabled;
    this._changeVersionsRuleLinkLabel.Enabled = this._changeVersionsRuleButton.Enabled;
    this.pictureRule.Enabled = this._changeVersionsRuleButton.Enabled;
    this._setCurrentEditingContextButton.Enabled = !this._readOnly && this._currentUserAndRole != null && this._currentUserAndRole.CachedEditingContextID != contextId;
    this._clearEditingContextButton.Enabled = !this._readOnly;
    this._selectEditingContextButton.Enabled = this._clearEditingContextButton.Enabled;
    this._selectEditingContextLinkLabel.Enabled = this._clearEditingContextButton.Enabled;
    this.pictureContext.Enabled = this._clearEditingContextButton.Enabled;
    this.tabCompositionContexts.Enabled = this._mrpSettings != null && this._mrpSettings.UseCompositionContext;
    this._compositionContextsCheckedListBox.Enabled = !this._readOnly && this._mrpSettings != null && this._mrpSettings.UseCompositionContext;
    this._setDefaultCompositionContextsButton.Enabled = this._compositionContextsCheckedListBox.Enabled && this._mrpSettings != null && this._mrpSettings.UseCompositionContext;
    this._addButton.Enabled = !this._readOnly && this._projObject != null && this._projObject.ObjectID != 0L;
    this._deleteButton.Enabled = this._addButton.Enabled && longList.Count > 0;
    this._refreshButton.Enabled = true;
    this._compositionTracingHeaderControl.Text = this.splitContainerMain.Panel2Collapsed ? LocalizationHolder.rm.GetString("MRP_21") : LocalizationHolder.rm.GetString("MRP_22");
    this.panelInfo.Visible = this.splitContainerMain.Panel2Collapsed;
    this._pageViewsManager.UpdateViews(this._tree.SelectedItems, true);
  }

  private void Tree_SelectedItemsChanged(object sender, EventArgs e)
  {
    this.Tree_BuildTree(sender, e);
  }

  private void Tree_SelectionChanging(object sender, SelectionChangingEventArgs e)
  {
  }

  private void CompositionTracing_TracingStarted(object sender, CompositionTracingEventArgs args)
  {
    this.DoShowTrace(sender, (EventArgs) args);
  }

  private void CompositionTracing_TracingStopped(object sender, CompositionTracingEventArgs args)
  {
    this.DoShowTrace(sender, (EventArgs) args);
    if (!this.HasErrorsInEditor)
      return;
    this.RaiseOnErrorsInEditor();
  }

  private void CompositionTracing_TracingObjectComplete(
    object sender,
    TracingObjectCompleteEventArgs args)
  {
    this.UpdateControls();
    this._errorsInEditor = this._compositionTracing.ErrorsInEditor;
    if (!this.HasErrorsInEditor)
      return;
    this.RaiseOnErrorsInEditor();
  }

  private void CompositionTracing_TracingAllObjectsComplete(
    object sender,
    TracingAllObjectsCompleteEventArgs args)
  {
    this._errorsInEditor = this._compositionTracing.ErrorsInEditor;
    this.DoShowTrace(sender, (EventArgs) args);
    if (this.HasErrorsInEditor)
      this.RaiseOnErrorsInEditor();
    else
      this.RaiseOnChanged();
  }

  private void CompositionTracing_TracingObjectSelected(
    object sender,
    TracingObjectSelectedEventArgs args)
  {
    try
    {
      this.SelectNode(args.Path);
    }
    catch
    {
      int num = (int) MessageBox.Show("При выделении узла в дереве произошла ошибка. Вероятно узел отсутствует в дереве. Попробуйте выбрать другие контексты состава и обновить дерево производственного заказа", "Intermech Professional Solution", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }
    this.UpdateControls();
  }

  private void SelectNode(RelationPath path)
  {
    Row row = this._tree.RootRow;
    int index = 0;
    for (int count = path.Items.Count; index < count; ++index)
    {
      SimpleRelationPair pathPart = path.Items[index];
      row = this.GetChildrenAndSelf(row).Where<Row>((Func<Row, bool>) (o => this.CompareNodeAndNodePathPart(o.Item as NavigatorTreeNode, pathPart))).FirstOrDefault<Row>();
      row.ExpandChildren(false);
    }
    this._tree.TopRow = row;
    this._tree.SelectedRow = row;
  }

  private IEnumerable<Row> GetChildrenAndSelf(Row row)
  {
    yield return row;
    int i = 0;
    for (int max = int.MaxValue; i < max; ++i)
    {
      Row row1 = row.ChildRowByIndex(i);
      if (row1 == null)
        break;
      yield return row1;
    }
  }

  private bool CompareNodeAndNodePathPart(NavigatorTreeNode node, SimpleRelationPair pathPart)
  {
    if (!(node.NodeID is NodeID nodeId))
      return false;
    return pathPart.F_PRJLINK_ID == 0L ? nodeId.ObjectID == pathPart.F_PART_ID : nodeId.PrjLinkID == pathPart.F_PRJLINK_ID;
  }

  /// <summary>Сгенерировать событие "OnErrorsInEditor"</summary>
  private void RaiseOnErrorsInEditor()
  {
    if (this.ErrorsInEditor == null)
      return;
    this.ErrorsInEditor((object) this, EventArgs.Empty);
  }

  /// <summary>Инициализировать службу уведомлений</summary>
  /// <returns>Ссылка на службу уведомлений для текущего окна</returns>
  private NotificationService InitializeNotificationService()
  {
    SwitchedNotificationService notificationService = new SwitchedNotificationService();
    notificationService.Parent = (NotificationService) ServicesManager.GetService(typeof (INotificationService));
    return (NotificationService) notificationService;
  }

  /// <summary>Удалить службу уведомлений</summary>
  /// <param name="notificationService">Удаляемая служба уведомлений</param>
  private void DisposeNotificationService(INotificationService notificationService)
  {
    this._navigatorTreeViewServiceContainer.Dispose();
    this._navigatorTreeViewServiceContainer = (AdvancedServiceContainer) null;
    this._services.RemoveService(typeof (INotificationService));
    ((IDisposable) notificationService).Dispose();
  }

  /// <summary>Управление службой уведомлений</summary>
  /// <param name="notificationService">Служба уведомлений</param>
  /// <param name="enabled">true - служба работает</param>
  private void EnableNotifications(INotificationService notificationService, bool enabled)
  {
    SwitchedNotificationService notificationService1 = (SwitchedNotificationService) notificationService;
    if (notificationService1 == null)
      return;
    notificationService1.Enabled = enabled;
  }

  /// <summary>Обновить состояние элементов управления в редакторе</summary>
  private void UpdateControls() => this.Tree_BuildTree((object) this, EventArgs.Empty);

  /// <summary>Создаём (обновляем) настройки фильтрации составов</summary>
  private void CreateFiltrationSettings()
  {
    if (this._manufactureOrderHolder.FiltrationSettings != null && this._manufactureOrderHolder.FiltrationSettings.Tags != null)
    {
      this._manufactureOrderHolder.FiltrationSettings.Tags[(object) "{0422E069-0A1D-4235-85E8-C52C3516CFC1}"] = (object) false;
      this._manufactureOrderHolder.FiltrationSettings.Tags[(object) "{76094280-391F-44AC-8B7B-9B6DEA501110}"] = (object) this._manufactureOrderHolder.FiltrationSettings.EditingContext;
    }
    NavigatorTreeNode focusedNode = this._tree.FocusedNode;
    INode nodeHandler = focusedNode != null ? this._tree.GetNodeHandler(focusedNode) : (INode) null;
    IDBRelationID data1 = nodeHandler != null ? nodeHandler.GetData(focusedNode.NodeID, typeof (IDBRelationID)) as IDBRelationID : (IDBRelationID) null;
    IDBTypedObjectID data2 = nodeHandler != null ? nodeHandler.GetData(focusedNode.NodeID, typeof (IDBTypedObjectID)) as IDBTypedObjectID : (IDBTypedObjectID) null;
    if ((data1 != null || data2 != null) && this._manufactureOrderHolder.FiltrationSettings.Tags != null)
    {
      IDBTypedObjectID compositionObject = this._tree.GetTopCompositionObject(focusedNode);
      ICurrentUserAndRole service = ServicesManager.GetService(typeof (ICurrentUserAndRole)) as ICurrentUserAndRole;
      RelationPair relationPair = new RelationPair(0L, compositionObject != null ? compositionObject.ObjectID : 0L, compositionObject != null ? compositionObject.ObjectType : -1, data1 == null || !MetaDataHelper.IsPdmPartiallyConfigurableRelationType(data1.RelationType) ? 0L : data1.Value, service.UserID, data2 != null ? data2.ObjectID : 0L, data1 == null || !MetaDataHelper.IsPdmPartiallyConfigurableRelationType(data1.RelationType) ? -1 : data1.RelationType, data2 != null ? data2.ObjectType : -1);
      if (!relationPair.Empty && relationPair.TOP_OBJECT_ID != 0L)
        this._manufactureOrderHolder.FiltrationSettings.Tags[(object) "{78D53C74-3CF7-4F48-94FC-80C4FCB0BA77}"] = (object) relationPair;
    }
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      (sessionKeeper.Session.GetCustomService(typeof (IVersionRulesCacheService)) as IVersionRulesCacheService).SetFiltrationSettings((object) sessionKeeper.Session.SessionGUID, this._manufactureOrderHolder.FiltrationSettings.OwnerID, this._manufactureOrderHolder.FiltrationSettings);
  }

  /// <summary>Удаляем настройки фильтрации составов</summary>
  private void RemoveFiltrationSettings()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      (sessionKeeper.Session.GetCustomService(typeof (IVersionRulesCacheService)) as IVersionRulesCacheService).SetFiltrationSettings((object) sessionKeeper.Session.SessionGUID, this._manufactureOrderHolder.FiltrationSettings.OwnerID, (FiltrationSettings) null);
  }

  /// <summary>Инициализировать закладку "Трассировка составов"</summary>
  private void InitializeCompositionTracing()
  {
    NavigatorTreeNode rootNode = this._tree.RootNode;
    RelationPath configuredNodePath = NavigatorTreeViewHelper.GetConfiguredNodePath(rootNode);
    this._compositionTracing.Services = (IServiceProvider) this._services;
    rootNode.Fetch();
    this._compositionTracing.Initialize(this._manufactureOrderHolder.FiltrationSettings != null ? this._manufactureOrderHolder.FiltrationSettings.OwnerID : string.Empty, configuredNodePath, rootNode.NodesAsSelectedItems);
  }

  /// <summary>Загрузить информацию в редактор</summary>
  private void Reload()
  {
    if (this._projObject == null || this._projObject.ObjectID == 0L)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      this._manufactureOrderHolder.Assign((object) sessionKeeper.Session.GetObjectInfo(this._projObject.ObjectID));
      this._manufactureOrderHolder.Merge(this._services.GetService(typeof (MRPOrderItemsSettingsHolder)) as MRPOrderItemsSettingsHolder);
    }
    this._tree.SetColumns(Intermech.Navigator.Utils.CaptionAndStatesesColumns(NodeColumnSortOrder.Ascending));
    this.RefreshEditor();
  }

  /// <summary>
  /// Подписаться на глобальное событие по управлению дополнительными параметрами запросов
  /// </summary>
  private void InitPluginsDataPlugin()
  {
    if (this._moClientPluginsDataTransfer != null)
      return;
    this._moClientPluginsDataTransfer = new ManufactOrdersEditor.MOClientPluginsDataTransfer(this);
    this._clientPluginsService.RegisterClientPlugin(this._moClientPluginsDataTransfer.PluginGuid, (IClientPluginsDataTransfer) this._moClientPluginsDataTransfer);
  }

  /// <summary>
  /// Отписаться от глобального события по управлению дополнительными параметрами запросов
  /// </summary>
  private void ReleasePluginsDataPlugin()
  {
    if (this._moClientPluginsDataTransfer == null)
      return;
    this._clientPluginsService.UnregisterClientPlugin(this._moClientPluginsDataTransfer.PluginGuid);
    this._moClientPluginsDataTransfer = (ManufactOrdersEditor.MOClientPluginsDataTransfer) null;
  }

  /// <summary>
  /// Заполнить все поля, отвечающие за правило подбора версий
  /// </summary>
  private void FillVersionsRule()
  {
    if (this._inEvents)
      return;
    if (this._manufactureOrderHolder == null || this._manufactureOrderHolder.FiltrationSettings == null || this._manufactureOrderHolder.FiltrationSettings.RuleID == 0L || this._manufactureOrderHolder.FiltrationSettings.CurrentRule == null)
      this._changeVersionsRuleLinkLabel.Text = LocalizationHolder.rm.GetString("MRP_11");
    else
      this._changeVersionsRuleLinkLabel.Text = $"[{this._manufactureOrderHolder.FiltrationSettings.CurrentRule.RuleObjectCaption}]";
  }

  /// <summary>Заполнить поля, связанные с контекстом редактирования</summary>
  private void FillEditingContext()
  {
    if (this._inEvents)
      return;
    if (this._manufactureOrderHolder == null || this._manufactureOrderHolder.FiltrationSettings == null || this._manufactureOrderHolder.FiltrationSettings.EditingContext.ContextID == 0L)
    {
      this._selectEditingContextLinkLabel.Text = LocalizationHolder.rm.GetString("MRP_13");
    }
    else
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(this._manufactureOrderHolder.FiltrationSettings.EditingContext.ContextID);
        this._selectEditingContextLinkLabel.Text = !objectInfo.Empty ? $"[{objectInfo.Caption}]" : LocalizationHolder.rm.GetString("MRP_13");
      }
    }
  }

  /// <summary>Обновить список контекстов составов</summary>
  private void FillCompositionContexts()
  {
    if (this._inEvents)
      return;
    try
    {
      this._inEvents = true;
      IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(new Guid("cad00651-306c-11d8-b4e9-00304f19f545"));
      this._compositionContextsCheckedListBox.Items.Clear();
      if (attributeType == null || attributeType.PossibleValues == null || attributeType.PossibleValuesDescriptions == null || attributeType.PossibleValues.Count <= 0 || attributeType.PossibleValuesDescriptions.Count != attributeType.PossibleValues.Count)
        return;
      for (int index = 0; index < attributeType.PossibleValues.Count; ++index)
        this._compositionContextsCheckedListBox.Items.Add(attributeType.PossibleValuesDescriptions[index] ?? attributeType.PossibleValues[index], this._manufactureOrderHolder != null && this._manufactureOrderHolder.CompositionContexts.IndexOf(DataSetProcessor.GetInt64Value(attributeType.PossibleValues[index], -1L)) >= 0);
    }
    finally
    {
      this._inEvents = false;
      this.UpdateControls();
    }
  }

  /// <summary>Принудительно открыть панель "Трассировка состава"</summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  private void DoShowTrace(object sender, EventArgs e)
  {
    this.splitContainerMain.Panel2Collapsed = false;
    this.UpdateControls();
  }

  /// <summary>Управление видимостью панели "Трассировка состава"</summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  private void DoCollapseTrace(object sender, EventArgs e)
  {
    this.splitContainerMain.Panel2Collapsed = !this.splitContainerMain.Panel2Collapsed;
    this.UpdateControls();
  }

  /// <summary>Clean up any resources being used.</summary>
  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
  protected override void Dispose(bool disposing)
  {
    if (disposing)
    {
      if (ServicesManager.GetService(typeof (ManufactOrdersEditor)) is ManufactOrdersEditor service1 && service1 == this)
        ServicesManager.RemoveService(typeof (ManufactOrdersEditor));
      if (this._currentUserAndRole != null)
        this._currentUserAndRole.UseRuleEvents = this._useEvents;
      this.DisposeNotificationService((INotificationService) this._notificationService);
      this.ReleasePluginsDataPlugin();
      if (ServicesManager.GetService(typeof (BarManager)) is BarManager service2)
      {
        this.toolBarTree.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
        service2.RendererChanged -= new EventHandler(this.BarManager_RendererChanged);
      }
    }
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ManufactOrdersEditor));
    this.imagesToolbars = new ImageList();
    this.imagesTabs = new ImageList();
    this.toolTips = new ToolTip();
    this._changeVersionsRuleLinkLabel = new LinkLabel();
    this._setDefaultVersionsRuleButton = new Button();
    this._setCurrentVersionsRuleButton = new Button();
    this._changeVersionsRuleButton = new Button();
    this._selectEditingContextLinkLabel = new LinkLabel();
    this._clearEditingContextButton = new Button();
    this._setCurrentEditingContextButton = new Button();
    this._selectEditingContextButton = new Button();
    this._setDefaultCompositionContextsButton = new Button();
    this.ilState = new ImageList();
    this.splitContainerMain = new SplitContainer();
    this.splitMain = new SplitContainer();
    this._tree = new ManufactOrdersEditorNavigatorTreeView();
    this.toolBarTree = new Intermech.Bars.ToolBar();
    this._addButton = new ButtonItem();
    this._deleteButton = new ButtonItem();
    this._refreshButton = new ButtonItem();
    this.headerControl = new HeaderControl();
    this._pageViewsManager = new PageViewsManager();
    this.headerControlPages = new HeaderControl();
    this._compositionTracingHeaderControl = new HeaderControl();
    this._compositionTracing = new CompositionTracing();
    this._topHeaderControl = new HeaderControl();
    this.pagesTop = new PageControl();
    this.tabVersionsRule = new Intermech.Docking.TabPage();
    this.pictureRule = new PictureBox();
    this.tabEditingContext = new Intermech.Docking.TabPage();
    this.pictureContext = new PictureBox();
    this.tabCompositionContexts = new Intermech.Docking.TabPage();
    this._compositionContextsCheckedListBox = new CheckedListBox();
    this.pictureCompositionContexts = new PictureBox();
    this.tabPage1 = new Intermech.Docking.TabPage();
    this._seriesDatesSelectingControl = new SeriesDatesSelectingControl();
    this.tabPage2 = new Intermech.Docking.TabPage();
    this._analogSelectionModeSelectionControl = new Intermech.Search.Mrp.AnalogSelectionModeSelectionControl();
    this.bevelSettings = new Bevel();
    this.treeViewsBridge = new TreeViewsBridge();
    this.panelInfo = new Panel();
    this.labelInfo = new Label();
    this.pictureBox1 = new PictureBox();
    this.splitContainerMain.BeginInit();
    this.splitContainerMain.Panel1.SuspendLayout();
    this.splitContainerMain.Panel2.SuspendLayout();
    this.splitContainerMain.SuspendLayout();
    this.splitMain.BeginInit();
    this.splitMain.Panel1.SuspendLayout();
    this.splitMain.Panel2.SuspendLayout();
    this.splitMain.SuspendLayout();
    this._tree.BeginInit();
    this.pagesTop.SuspendLayout();
    this.tabVersionsRule.SuspendLayout();
    ((ISupportInitialize) this.pictureRule).BeginInit();
    this.tabEditingContext.SuspendLayout();
    ((ISupportInitialize) this.pictureContext).BeginInit();
    this.tabCompositionContexts.SuspendLayout();
    ((ISupportInitialize) this.pictureCompositionContexts).BeginInit();
    this.tabPage1.SuspendLayout();
    this.tabPage2.SuspendLayout();
    this.panelInfo.SuspendLayout();
    ((ISupportInitialize) this.pictureBox1).BeginInit();
    this.SuspendLayout();
    this.imagesToolbars.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imagesToolbars.ImageStream");
    this.imagesToolbars.TransparentColor = Color.Transparent;
    this.imagesToolbars.Images.SetKeyName(0, "add.png");
    this.imagesToolbars.Images.SetKeyName(1, "delete.png");
    this.imagesToolbars.Images.SetKeyName(2, "refresh.png");
    this.imagesTabs.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imagesTabs.ImageStream");
    this.imagesTabs.TransparentColor = Color.Transparent;
    this.imagesTabs.Images.SetKeyName(0, "Правила подбора версий 2.ico");
    this.imagesTabs.Images.SetKeyName(1, "substitutes_16x16.ico");
    this.imagesTabs.Images.SetKeyName(2, "gears.png");
    this.imagesTabs.Images.SetKeyName(3, "Объекты конфигуратора составов.ico");
    this.imagesTabs.Images.SetKeyName(4, "substsico.ico");
    this.imagesTabs.Images.SetKeyName(5, "Маршрут обработки.ico");
    this.toolTips.AutomaticDelay = 1000;
    this._changeVersionsRuleLinkLabel.ActiveLinkColor = Color.Blue;
    componentResourceManager.ApplyResources((object) this._changeVersionsRuleLinkLabel, "_changeVersionsRuleLinkLabel");
    this._changeVersionsRuleLinkLabel.Name = "_changeVersionsRuleLinkLabel";
    this._changeVersionsRuleLinkLabel.TabStop = true;
    this.toolTips.SetToolTip((Control) this._changeVersionsRuleLinkLabel, componentResourceManager.GetString("_changeVersionsRuleLinkLabel.ToolTip"));
    this._changeVersionsRuleLinkLabel.VisitedLinkColor = Color.Blue;
    this._changeVersionsRuleLinkLabel.Click += new EventHandler(this.ChangeVersionsRuleLinkLabel_Click);
    componentResourceManager.ApplyResources((object) this._setDefaultVersionsRuleButton, "_setDefaultVersionsRuleButton");
    this._setDefaultVersionsRuleButton.Name = "_setDefaultVersionsRuleButton";
    this.toolTips.SetToolTip((Control) this._setDefaultVersionsRuleButton, componentResourceManager.GetString("_setDefaultVersionsRuleButton.ToolTip"));
    this._setDefaultVersionsRuleButton.UseVisualStyleBackColor = true;
    this._setDefaultVersionsRuleButton.Click += new EventHandler(this.SetDefaultVersionsRuleButton_Click);
    componentResourceManager.ApplyResources((object) this._setCurrentVersionsRuleButton, "_setCurrentVersionsRuleButton");
    this._setCurrentVersionsRuleButton.Name = "_setCurrentVersionsRuleButton";
    this.toolTips.SetToolTip((Control) this._setCurrentVersionsRuleButton, componentResourceManager.GetString("_setCurrentVersionsRuleButton.ToolTip"));
    this._setCurrentVersionsRuleButton.UseVisualStyleBackColor = true;
    this._setCurrentVersionsRuleButton.Click += new EventHandler(this.SetCurrentVersionsRuleButton_Click);
    componentResourceManager.ApplyResources((object) this._changeVersionsRuleButton, "_changeVersionsRuleButton");
    this._changeVersionsRuleButton.Name = "_changeVersionsRuleButton";
    this.toolTips.SetToolTip((Control) this._changeVersionsRuleButton, componentResourceManager.GetString("_changeVersionsRuleButton.ToolTip"));
    this._changeVersionsRuleButton.UseVisualStyleBackColor = true;
    this._changeVersionsRuleButton.Click += new EventHandler(this.ChangeVersionsRuleButton_Click);
    this._selectEditingContextLinkLabel.ActiveLinkColor = Color.Blue;
    componentResourceManager.ApplyResources((object) this._selectEditingContextLinkLabel, "_selectEditingContextLinkLabel");
    this._selectEditingContextLinkLabel.Name = "_selectEditingContextLinkLabel";
    this._selectEditingContextLinkLabel.TabStop = true;
    this.toolTips.SetToolTip((Control) this._selectEditingContextLinkLabel, componentResourceManager.GetString("_selectEditingContextLinkLabel.ToolTip"));
    this._selectEditingContextLinkLabel.VisitedLinkColor = Color.Blue;
    this._selectEditingContextLinkLabel.Click += new EventHandler(this.SelectEditingContextLinkLabel_Click);
    componentResourceManager.ApplyResources((object) this._clearEditingContextButton, "_clearEditingContextButton");
    this._clearEditingContextButton.Name = "_clearEditingContextButton";
    this.toolTips.SetToolTip((Control) this._clearEditingContextButton, componentResourceManager.GetString("_clearEditingContextButton.ToolTip"));
    this._clearEditingContextButton.UseVisualStyleBackColor = true;
    this._clearEditingContextButton.Click += new EventHandler(this.ClearEditingContextButton_Click);
    componentResourceManager.ApplyResources((object) this._setCurrentEditingContextButton, "_setCurrentEditingContextButton");
    this._setCurrentEditingContextButton.Name = "_setCurrentEditingContextButton";
    this.toolTips.SetToolTip((Control) this._setCurrentEditingContextButton, componentResourceManager.GetString("_setCurrentEditingContextButton.ToolTip"));
    this._setCurrentEditingContextButton.UseVisualStyleBackColor = true;
    this._setCurrentEditingContextButton.Click += new EventHandler(this.SetCurrentEditingContextButton_Click);
    componentResourceManager.ApplyResources((object) this._selectEditingContextButton, "_selectEditingContextButton");
    this._selectEditingContextButton.Name = "_selectEditingContextButton";
    this.toolTips.SetToolTip((Control) this._selectEditingContextButton, componentResourceManager.GetString("_selectEditingContextButton.ToolTip"));
    this._selectEditingContextButton.UseVisualStyleBackColor = true;
    this._selectEditingContextButton.Click += new EventHandler(this.SelectEditingContextButton_Click);
    componentResourceManager.ApplyResources((object) this._setDefaultCompositionContextsButton, "_setDefaultCompositionContextsButton");
    this._setDefaultCompositionContextsButton.Name = "_setDefaultCompositionContextsButton";
    this.toolTips.SetToolTip((Control) this._setDefaultCompositionContextsButton, componentResourceManager.GetString("_setDefaultCompositionContextsButton.ToolTip"));
    this._setDefaultCompositionContextsButton.UseVisualStyleBackColor = true;
    this._setDefaultCompositionContextsButton.Click += new EventHandler(this.SetDefaultCompositionContextButton_Click);
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
    componentResourceManager.ApplyResources((object) this.splitContainerMain, "splitContainerMain");
    this.splitContainerMain.Name = "splitContainerMain";
    this.splitContainerMain.Panel1.Controls.Add((Control) this.splitMain);
    this.splitContainerMain.Panel1.Controls.Add((Control) this._compositionTracingHeaderControl);
    this.splitContainerMain.Panel2.Controls.Add((Control) this._compositionTracing);
    componentResourceManager.ApplyResources((object) this.splitMain, "splitMain");
    this.splitMain.FixedPanel = FixedPanel.Panel1;
    this.splitMain.Name = "splitMain";
    this.splitMain.Panel1.Controls.Add((Control) this._tree);
    this.splitMain.Panel1.Controls.Add((Control) this.toolBarTree);
    this.splitMain.Panel1.Controls.Add((Control) this.headerControl);
    this.splitMain.Panel2.Controls.Add((Control) this._pageViewsManager);
    this.splitMain.Panel2.Controls.Add((Control) this.headerControlPages);
    this._tree.AllowDrop = true;
    this._tree.AllowMultiSelect = false;
    this._tree.AllowUserPinnedColumns = false;
    this._tree.DisableCheckedOutColumn = true;
    this._tree.DisableIMContextMenu = true;
    this._tree.DisableKeyDownEvents = true;
    this._tree.DisableKeyUpEvents = true;
    componentResourceManager.ApplyResources((object) this._tree, "_tree");
    this._tree.HeaderStyle.HorzAlignment = (StringAlignment) componentResourceManager.GetObject("_tree.HeaderStyle.HorzAlignment");
    this._tree.ImageList = (ImageList) null;
    this._tree.LineStyle = LineStyle.Dot;
    this._tree.Name = "_tree";
    this._tree.RowEvenStyle.WordWrap = (bool) componentResourceManager.GetObject("_tree.RowEvenStyle.WordWrap");
    this._tree.RowOddStyle.WordWrap = (bool) componentResourceManager.GetObject("_tree.RowOddStyle.WordWrap");
    this._tree.RowSelectedStyle.WordWrap = (bool) componentResourceManager.GetObject("_tree.RowSelectedStyle.WordWrap");
    this._tree.RowStyle.BorderColor = SystemColors.Control;
    this._tree.RowStyle.BorderStyle = Border3DStyle.Adjust;
    this._tree.RowStyle.BorderWidth = 1;
    this._tree.RowStyle.WordWrap = (bool) componentResourceManager.GetObject("_tree.RowStyle.WordWrap");
    this._tree.SelectBeforeEdit = true;
    this._tree.ShowRootRow = false;
    this._tree.SuppressErrorMessages = true;
    this._tree.BuildTree += new EventHandler(this.Tree_BuildTree);
    this._tree.SelectedItemsChanged += new EventHandler(this.Tree_SelectedItemsChanged);
    this._tree.SelectionChanging += new SelectionChangingHandler(this.Tree_SelectionChanging);
    this.toolBarTree.AddRemoveButtonsVisible = false;
    this.toolBarTree.AllowHorizontalDock = false;
    this.toolBarTree.DockLine = 3;
    this.toolBarTree.DrawActionsButton = false;
    this.toolBarTree.FullMenus = true;
    this.toolBarTree.Guid = new Guid("ba855ba6-35ae-4775-b979-b76ac70a54e0");
    this.toolBarTree.Hidden = false;
    this.toolBarTree.ImageList = this.imagesToolbars;
    this.toolBarTree.Items.AddRange(new ToolbarItemBase[3]
    {
      (ToolbarItemBase) this._addButton,
      (ToolbarItemBase) this._deleteButton,
      (ToolbarItemBase) this._refreshButton
    });
    componentResourceManager.ApplyResources((object) this.toolBarTree, "toolBarTree");
    this.toolBarTree.MinimumFloatingSize = new Size(250, 30);
    this.toolBarTree.Name = "toolBarTree";
    this.toolBarTree.Overflow = ToolBarOverflow.Wrap;
    this.toolBarTree.Stretch = true;
    this.toolBarTree.Tearable = false;
    this._addButton.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this._addButton, "_addButton");
    this._addButton.ImageIndex = 0;
    this._addButton.ShowText = true;
    this._addButton.Click += new EventHandler(this.AddButton_Click);
    componentResourceManager.ApplyResources((object) this._deleteButton, "_deleteButton");
    this._deleteButton.ImageIndex = 1;
    this._deleteButton.ShowText = true;
    this._deleteButton.Click += new EventHandler(this.DeleteButton_Click);
    this._refreshButton.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this._refreshButton, "_refreshButton");
    this._refreshButton.ImageIndex = 2;
    this._refreshButton.ShowText = true;
    this._refreshButton.Click += new EventHandler(this.RefreshButton_Click);
    this.headerControl.BackColor = SystemColors.Control;
    componentResourceManager.ApplyResources((object) this.headerControl, "headerControl");
    this.headerControl.ForeColor = SystemColors.ControlText;
    this.headerControl.HeaderFont = new Font("Tahoma", 10f, FontStyle.Bold);
    this.headerControl.Name = "headerControl";
    this._pageViewsManager.ActiveViewPage = (IViewPage) null;
    this._pageViewsManager.AllowedViews = new string[9]
    {
      "PdmObjectOptionsEditorView",
      "PdmRelationOptionsEditorView",
      "MRP_PDMConfigurator",
      "MRP_PDMSubstitutes",
      "MRP_TechcardRoutes",
      "MRP_BoughtArticles",
      "ObjectProperties",
      "RelationProperties",
      "VersionsApplicabilitiesView"
    };
    this._pageViewsManager.CausesValidation = false;
    componentResourceManager.ApplyResources((object) this._pageViewsManager, "_pageViewsManager");
    this._pageViewsManager.Name = "_pageViewsManager";
    this.headerControlPages.BackColor = SystemColors.Control;
    componentResourceManager.ApplyResources((object) this.headerControlPages, "headerControlPages");
    this.headerControlPages.ForeColor = SystemColors.ControlText;
    this.headerControlPages.HeaderFont = new Font("Tahoma", 10f, FontStyle.Bold);
    this.headerControlPages.Name = "headerControlPages";
    this._compositionTracingHeaderControl.BackColor = SystemColors.Control;
    componentResourceManager.ApplyResources((object) this._compositionTracingHeaderControl, "_compositionTracingHeaderControl");
    this._compositionTracingHeaderControl.ForeColor = SystemColors.ControlText;
    this._compositionTracingHeaderControl.HeaderFont = new Font("Tahoma", 10f, FontStyle.Bold);
    this._compositionTracingHeaderControl.Name = "_compositionTracingHeaderControl";
    this._compositionTracingHeaderControl.Click += new EventHandler(this.DoCollapseTrace);
    componentResourceManager.ApplyResources((object) this._compositionTracing, "_compositionTracing");
    this._compositionTracing.Name = "_compositionTracing";
    this._compositionTracing.TracingStarted += new CompositionTracingEventHandler(this.CompositionTracing_TracingStarted);
    this._compositionTracing.TracingObjectComplete += new TracingObjectCompleteEventHandler(this.CompositionTracing_TracingObjectComplete);
    this._compositionTracing.TracingStopped += new CompositionTracingEventHandler(this.CompositionTracing_TracingStopped);
    this._compositionTracing.TracingAllObjectsComplete += new TracingAllObjectsCompleteEventHandler(this.CompositionTracing_TracingAllObjectsComplete);
    this._compositionTracing.TracingObjectSelected += new TracingObjectSelectedEventHandler(this.CompositionTracing_TracingObjectSelected);
    this._topHeaderControl.BackColor = SystemColors.Control;
    componentResourceManager.ApplyResources((object) this._topHeaderControl, "_topHeaderControl");
    this._topHeaderControl.ForeColor = SystemColors.ControlText;
    this._topHeaderControl.HeaderFont = new Font("Tahoma", 10f, FontStyle.Bold);
    this._topHeaderControl.Name = "_topHeaderControl";
    this._topHeaderControl.Click += new EventHandler(this.TopHeaderControl_Click);
    this.pagesTop.Controls.Add((Control) this.tabVersionsRule);
    this.pagesTop.Controls.Add((Control) this.tabEditingContext);
    this.pagesTop.Controls.Add((Control) this.tabCompositionContexts);
    this.pagesTop.Controls.Add((Control) this.tabPage1);
    this.pagesTop.Controls.Add((Control) this.tabPage2);
    componentResourceManager.ApplyResources((object) this.pagesTop, "pagesTop");
    this.pagesTop.ImageList = this.imagesTabs;
    this.pagesTop.Name = "pagesTop";
    this.pagesTop.TabLayout = TabLayout.SingleLineFixed;
    this.tabVersionsRule.Controls.Add((Control) this.pictureRule);
    this.tabVersionsRule.Controls.Add((Control) this._changeVersionsRuleLinkLabel);
    this.tabVersionsRule.Controls.Add((Control) this._setDefaultVersionsRuleButton);
    this.tabVersionsRule.Controls.Add((Control) this._setCurrentVersionsRuleButton);
    this.tabVersionsRule.Controls.Add((Control) this._changeVersionsRuleButton);
    this.tabVersionsRule.Index = 0;
    componentResourceManager.ApplyResources((object) this.tabVersionsRule, "tabVersionsRule");
    this.tabVersionsRule.Name = "tabVersionsRule";
    this.tabVersionsRule.TabImage = (Image) componentResourceManager.GetObject("tabVersionsRule.TabImage");
    this.tabVersionsRule.TabImageIndex = 0;
    componentResourceManager.ApplyResources((object) this.pictureRule, "pictureRule");
    this.pictureRule.Name = "pictureRule";
    this.pictureRule.TabStop = false;
    this.pictureRule.Click += new EventHandler(this.ChangeVersionsRuleButton_Click);
    this.tabEditingContext.Controls.Add((Control) this.pictureContext);
    this.tabEditingContext.Controls.Add((Control) this._selectEditingContextLinkLabel);
    this.tabEditingContext.Controls.Add((Control) this._clearEditingContextButton);
    this.tabEditingContext.Controls.Add((Control) this._setCurrentEditingContextButton);
    this.tabEditingContext.Controls.Add((Control) this._selectEditingContextButton);
    this.tabEditingContext.Index = 1;
    componentResourceManager.ApplyResources((object) this.tabEditingContext, "tabEditingContext");
    this.tabEditingContext.Name = "tabEditingContext";
    this.tabEditingContext.TabImage = (Image) componentResourceManager.GetObject("tabEditingContext.TabImage");
    this.tabEditingContext.TabImageIndex = 1;
    componentResourceManager.ApplyResources((object) this.pictureContext, "pictureContext");
    this.pictureContext.Name = "pictureContext";
    this.pictureContext.TabStop = false;
    this.pictureContext.Click += new EventHandler(this.SelectEditingContextButton_Click);
    this.tabCompositionContexts.Controls.Add((Control) this._setDefaultCompositionContextsButton);
    this.tabCompositionContexts.Controls.Add((Control) this._compositionContextsCheckedListBox);
    this.tabCompositionContexts.Controls.Add((Control) this.pictureCompositionContexts);
    this.tabCompositionContexts.Index = 2;
    componentResourceManager.ApplyResources((object) this.tabCompositionContexts, "tabCompositionContexts");
    this.tabCompositionContexts.Name = "tabCompositionContexts";
    this.tabCompositionContexts.TabImage = (Image) componentResourceManager.GetObject("tabCompositionContexts.TabImage");
    this.tabCompositionContexts.TabImageIndex = 2;
    componentResourceManager.ApplyResources((object) this._compositionContextsCheckedListBox, "_compositionContextsCheckedListBox");
    this._compositionContextsCheckedListBox.BackColor = Color.White;
    this._compositionContextsCheckedListBox.CheckOnClick = true;
    this._compositionContextsCheckedListBox.FormattingEnabled = true;
    this._compositionContextsCheckedListBox.MultiColumn = true;
    this._compositionContextsCheckedListBox.Name = "_compositionContextsCheckedListBox";
    this._compositionContextsCheckedListBox.ItemCheck += new ItemCheckEventHandler(this.CompositionContextsCheckedListBox_ItemCheck);
    componentResourceManager.ApplyResources((object) this.pictureCompositionContexts, "pictureCompositionContexts");
    this.pictureCompositionContexts.Name = "pictureCompositionContexts";
    this.pictureCompositionContexts.TabStop = false;
    this.tabPage1.Controls.Add((Control) this._seriesDatesSelectingControl);
    this.tabPage1.Index = 3;
    componentResourceManager.ApplyResources((object) this.tabPage1, "tabPage1");
    this.tabPage1.Name = "tabPage1";
    this.tabPage1.TabImage = (Image) componentResourceManager.GetObject("tabPage1.TabImage");
    componentResourceManager.ApplyResources((object) this._seriesDatesSelectingControl, "_seriesDatesSelectingControl");
    this._seriesDatesSelectingControl.Name = "_seriesDatesSelectingControl";
    this._seriesDatesSelectingControl.Changed += new EventHandler(this.SeriesDatesSelectingControl_Changed);
    this.tabPage2.Controls.Add((Control) this._analogSelectionModeSelectionControl);
    this.tabPage2.Index = 4;
    componentResourceManager.ApplyResources((object) this.tabPage2, "tabPage2");
    this.tabPage2.Name = "tabPage2";
    this.tabPage2.TabImage = (Image) componentResourceManager.GetObject("tabPage2.TabImage");
    componentResourceManager.ApplyResources((object) this._analogSelectionModeSelectionControl, "_analogSelectionModeSelectionControl");
    this._analogSelectionModeSelectionControl.Name = "_analogSelectionModeSelectionControl";
    this._analogSelectionModeSelectionControl.AnalogSelectionModeChanged += new EventHandler(this.AnalogSelectionModeSelectionControl_AnalogSelectionModeChanged);
    componentResourceManager.ApplyResources((object) this.bevelSettings, "bevelSettings");
    this.bevelSettings.Name = "bevelSettings";
    this.treeViewsBridge.ViewsManager = (IViewsManager) this._pageViewsManager;
    this.panelInfo.BorderStyle = BorderStyle.Fixed3D;
    this.panelInfo.Controls.Add((Control) this.labelInfo);
    this.panelInfo.Controls.Add((Control) this.pictureBox1);
    componentResourceManager.ApplyResources((object) this.panelInfo, "panelInfo");
    this.panelInfo.Name = "panelInfo";
    componentResourceManager.ApplyResources((object) this.labelInfo, "labelInfo");
    this.labelInfo.Name = "labelInfo";
    componentResourceManager.ApplyResources((object) this.pictureBox1, "pictureBox1");
    this.pictureBox1.Name = "pictureBox1";
    this.pictureBox1.TabStop = false;
    this.AutoScaleMode = AutoScaleMode.Inherit;
    this.Controls.Add((Control) this.splitContainerMain);
    this.Controls.Add((Control) this.panelInfo);
    this.Controls.Add((Control) this.pagesTop);
    this.Controls.Add((Control) this.bevelSettings);
    this.Controls.Add((Control) this._topHeaderControl);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.Name = nameof (ManufactOrdersEditor);
    this.splitContainerMain.Panel1.ResumeLayout(false);
    this.splitContainerMain.Panel2.ResumeLayout(false);
    this.splitContainerMain.EndInit();
    this.splitContainerMain.ResumeLayout(false);
    this.splitMain.Panel1.ResumeLayout(false);
    this.splitMain.Panel2.ResumeLayout(false);
    this.splitMain.EndInit();
    this.splitMain.ResumeLayout(false);
    this._tree.EndInit();
    this.pagesTop.ResumeLayout(false);
    this.tabVersionsRule.ResumeLayout(false);
    this.tabVersionsRule.PerformLayout();
    ((ISupportInitialize) this.pictureRule).EndInit();
    this.tabEditingContext.ResumeLayout(false);
    this.tabEditingContext.PerformLayout();
    ((ISupportInitialize) this.pictureContext).EndInit();
    this.tabCompositionContexts.ResumeLayout(false);
    ((ISupportInitialize) this.pictureCompositionContexts).EndInit();
    this.tabPage1.ResumeLayout(false);
    this.tabPage2.ResumeLayout(false);
    this.panelInfo.ResumeLayout(false);
    ((ISupportInitialize) this.pictureBox1).EndInit();
    this.ResumeLayout(false);
  }

  /// <summary>Класс-затычка для передачи своих параметров в запросы</summary>
  protected class MOClientPluginsDataTransfer : ClientPluginsDataTransfer
  {
    /// <summary>Владелец</summary>
    private ManufactOrdersEditor _owner;

    /// <summary>Создать экземпляр класса</summary>
    public MOClientPluginsDataTransfer(ManufactOrdersEditor owner) => this._owner = owner;

    /// <summary>
    /// Метод вызывается ядром клиентской части для сбора информации у плагинов.
    /// Плагины, подписавшиеся в коллекции IClientPluginsService, должны записать в словарик
    /// PluginsData свою информацию в виде сериализуемых пар значений [Ключ] = [Значение].
    /// Указанная информация будет передана на серверную сторону.
    /// </summary>
    /// <param name="PluginsData">Коллекция сериализуемых пар значений для передачи
    /// дополнительной информации на серверную сторону</param>
    public override void GetPluginData(HybridDictionary PluginsData)
    {
      base.GetPluginData(PluginsData);
      if (PluginsData == null)
        return;
      FiltrationHelper.BlockPluginFiltrations(PluginsData);
      FiltrationHelper.UnlockConfigurator(PluginsData);
      if (this._owner != null && this._owner._mrpSettings != null)
      {
        if (this._owner._mrpSettings.UseCompositionContext)
        {
          PluginsData[(object) "{529FFE92-FDA7-48B8-AADF-ADB1EE6EF584}"] = (object) false;
          HybridDictionary hybridDictionary = PluginsData;
          List<long> longList;
          if (this._owner == null || this._owner._manufactureOrderHolder == null)
            longList = new List<long>((IEnumerable<long>) new long[2]
            {
              0L,
              3L
            });
          else
            longList = this._owner._manufactureOrderHolder.CompositionContexts;
          hybridDictionary[(object) "{AB419A02-DE8A-4A8E-905A-D782F5B720E5}"] = (object) longList;
        }
        else
          PluginsData[(object) "{529FFE92-FDA7-48B8-AADF-ADB1EE6EF584}"] = (object) true;
        if (this._owner._seriesDatesSelectingControl.SeriesDateSettingsHolder != null)
          PluginsData[(object) "{E2390B62-E0BA-4F7E-89CC-1E9E33F0BB5C}"] = (object) this._owner._seriesDatesSelectingControl.SeriesDateSettingsHolder;
        else
          PluginsData.Remove((object) "{E2390B62-E0BA-4F7E-89CC-1E9E33F0BB5C}");
        AnalogsHelper.SetAnalogSelectionModeToRecordSetParamsTags(PluginsData, this._owner._analogSelectionModeSelectionControl.AnalogSelectionMode);
      }
      PluginsData[(object) "{7196FEC5-A048-4118-AF15-73BEEAA63A87}"] = (object) this._owner._manufactureOrderHolder.FiltrationSettings.OwnerID;
      PluginsData[(object) "{76094280-391F-44AC-8B7B-9B6DEA501110}"] = (object) this._owner._manufactureOrderHolder.FiltrationSettings.EditingContext;
    }
  }

  public sealed class ReplaceProductDescriptor : Intermech.Navigator.DBObjects.Descriptor
  {
    private long _newRelationID;

    public ReplaceProductDescriptor(long objectVersionID, long newRelationID)
      : base(objectVersionID)
    {
      this._newRelationID = !RelationHelper.IsUnknownRelationID(newRelationID) ? newRelationID : throw new ArgumentException();
    }

    public override INode GetChild(INodeID nodeID)
    {
      return (INode) new ManufactOrdersEditor.ReplaceProductNode(this._objID, this._newRelationID);
    }
  }

  public sealed class ReplaceProductNode : ObjectNode
  {
    private long _newRelationID;

    public ReplaceProductNode(long objectVersionID, long newRelationID)
      : base(ManufactOrdersEditor.ReplaceProductNode.GetObjectTypeID(objectVersionID), objectVersionID)
    {
      this._newRelationID = !RelationHelper.IsUnknownRelationID(newRelationID) ? newRelationID : throw new ArgumentException();
    }

    protected override List<PartSlot> CreateFolderSlots()
    {
      return new List<PartSlot>()
      {
        new PartSlot(this.GetRelationGuid(this._newRelationID), (INodePart) new RelatedObjectsPart(this._objTypeID, this._objID, RelatedObjectsRole.Composition, new ConditionStructure()
        {
          Attribute = (object) ObligatoryObjectAttributes.F_PRJLINK_ID,
          RelationalOperator = RelationalOperators.Equal,
          Value = (object) this._newRelationID,
          SQL = string.Empty
        }, this.Services))
      };
    }

    private static int GetObjectTypeID(long objectVersionID)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
        return sessionKeeper.Session.GetObject(objectVersionID).ObjectType;
    }

    private Guid GetRelationGuid(long relationID)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
        return sessionKeeper.Session.GetRelation(relationID).GUID;
    }
  }
}
