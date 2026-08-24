// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.CompositionTracing
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.Bars;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.Contexts;
using Intermech.Interfaces.MRP;
using Intermech.Interfaces.PdmConfigurator;
using Intermech.Localization;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.MRP.Orders;

/// <summary>
/// Компонент, позволяющий выполнять трассировку составов на наличие ошибок и предупреждений,
/// связанных с конфигуратором составов, допустимыми заменами, маршрутами обработки и
/// уровнями продвижений в объектах этих составов
/// </summary>
/// <summary>
/// Компонент, позволяющий выполнять трассировку составов на наличие ошибок и предупреждений,
/// связанных с конфигуратором составов, допустимыми заменами, маршрутами обработки и
/// уровнями продвижений в объектах этих составов
/// </summary>
public sealed class CompositionTracing : UserControl, IDisposable
{
  /// <summary>
  /// Ширина колонки для определённого типа: [KEY колонки] =&gt; [Ширина колонки]
  /// </summary>
  private Dictionary<string, int> _columnWidthDictionaryByKey = new Dictionary<string, int>();
  /// <summary>Колонка "OBJECT_TYPE_ICON" - значок типа объекта</summary>
  private const string ObjectTypeIconColumnKey = "OBJECT_TYPE_ICON";
  /// <summary>Колонка "OBJECT_ID" - идентификатор версии объекта</summary>
  private const string ObjectVersionIDColumnKey = "OBJECT_ID";
  /// <summary>Колонка "RELATION_ID" - идентификатор связи</summary>
  private const string RelationIDColumnKey = "RELATION_ID";
  /// <summary>Колонка "OBJECT_CAPTION" - заголовок объекта</summary>
  private const string ObjectCaptionColumnKey = "OBJECT_CAPTION";
  /// <summary>Колонка "MESSAGE" - сообщение</summary>
  private const string MessageColumnKey = "MESSAGE";
  /// <summary>Колонка "RELATION_PATH" - путь к объекту состава</summary>
  private const string RelationPathColumnKey = "RELATION_PATH";
  /// <summary>Колонка "TRACE" - трассировка</summary>
  private const string TraceColumnKey = "TRACE";
  /// <summary>Колонка "PARENT" - родительский объект</summary>
  private const string ParentColumnKey = "PARENT";
  /// <summary>Стиль для группирующих строк</summary>
  private iGCellStyleDesign _groupRowsLevel1 = new iGCellStyleDesign();
  /// <summary>Объект для синхронизации</summary>
  private object _syncRoot = new object();
  /// <summary>Контейнер сервисов</summary>
  private AdvancedServiceContainer _services = new AdvancedServiceContainer();
  /// <summary>
  /// Ссылка на контейнер настроек, если он найден в сервисах
  /// </summary>
  private ManufactureOrderHolder _holder;
  /// <summary>Состояние закладки</summary>
  private bool _inProgress;
  /// <summary>Была ли прервана трассировка</summary>
  private bool _isTerminated;
  /// <summary>Ключ настроек фильтрации составов</summary>
  private string _filtrationOwnerID = string.Empty;
  /// <summary>
  /// Полный путь к корневому конфигурируемому объекту, объекты состава которого будут трассироваться
  /// </summary>
  private RelationPath _rootPath;
  /// <summary>
  /// Коллекция объектов состава, которые требуется протрассировать
  /// </summary>
  private CompositionObjects _compositionObjects;
  /// <summary>Guid задания по трассировке составов</summary>
  private Guid _taskGuid = Guid.Empty;
  /// <summary>
  /// Фоновый поток, в рамках которого выполняется обращение к серверу за статусом задания
  /// </summary>
  private Thread _taskThread;
  /// <summary>
  /// Состояния текущей задачи (для каждого протрассированного объекта - своё состояние)
  /// </summary>
  private Dictionary<CompositionObject, PdmCompositionBrowserJobStatus> _taskStatuses = new Dictionary<CompositionObject, PdmCompositionBrowserJobStatus>();
  /// <summary>Коллекция изображений для разных категорий</summary>
  private ICategoryTypeIconService _objtypesIcons;
  /// <summary>
  /// Есть ли ошибки в редакторе. До проверки считаем, что они есть
  /// </summary>
  private bool _errorsInEditor = true;
  /// <summary>
  /// Ссылка на обработчик события "Изменился главный редактор"
  /// </summary>
  private ManufactOrdersChangedEventHandler _manufactOrdersChangedEventHandler;
  /// <summary>Скрыта ли панель легедны</summary>
  private static bool _hiddenLegendPanel;
  /// <summary>Задержки (мс) для волшебного Sleep</summary>
  private static readonly int[] Delays = new int[46]
  {
    10,
    10,
    10,
    20,
    20,
    20,
    20,
    20,
    30,
    30,
    30,
    30,
    30,
    40,
    40,
    40,
    50,
    50,
    50,
    60,
    60,
    60,
    70,
    70,
    70,
    80 /*0x50*/,
    80 /*0x50*/,
    80 /*0x50*/,
    90,
    90,
    90,
    100,
    100,
    100,
    200,
    200,
    200,
    300,
    300,
    300,
    400,
    400,
    400,
    500,
    500,
    500
  };
  /// <summary>Контейнер компонентов</summary>
  private IContainer components;
  private ImageList ilTracing;
  private Panel panelTrace;
  private iGrid _grid;
  private Intermech.Bars.ToolBar toolBarTrace;
  private ButtonItem _startTracingButton;
  private ButtonItem _stopTracingButton;
  private ImageList ilState;
  private ButtonItem _clearButton;
  private Panel panelStatus;
  private Label labelHint;
  private Label labelQuantity;
  private ProgressBar progressBar;
  private Intermech.Bars.ToolBar toolbarFilter;
  private ButtonItem btErrors;
  private ButtonItem btWarnings;
  private ButtonItem btInformation;
  private LabelItem labelFilter;
  private ButtonItem _expandAllButton;
  private ButtonItem _collapseAllButton;
  private Panel panelLegend;
  private Button btnHideLegend;
  private Label lbLegendError;
  private Label lbLegendWarning;
  private Label lbLegendWarningOK;

  /// <summary>Создать экземпляр класса</summary>
  public CompositionTracing()
  {
    this.InitializeComponent();
    if (ServicesManager.GetService(typeof (BarManager)) is BarManager service)
    {
      service.RendererChanged += new EventHandler(this.BarManager_RendererChanged);
      this.BarManager_RendererChanged((object) service, EventArgs.Empty);
    }
    this._objtypesIcons = ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
    this.PrepareGridsColumns();
    this.UpdateControls();
  }

  /// <summary>
  /// Событие генерируется когда начинается трассировка составов объектов
  /// </summary>
  [CustomDescription("MRP_34")]
  [Description("Событие генерируется когда начинается трассировка составов объектов")]
  public event CompositionTracingEventHandler TracingStarted;

  /// <summary>
  /// Событие генерируется когда завершается трассировка состава очередного объекта
  /// </summary>
  [CustomDescription("MRP_31")]
  [Description("Событие генерируется когда завершается трассировка состава очередного объекта")]
  public event TracingObjectCompleteEventHandler TracingObjectComplete;

  /// <summary>
  /// Событие генерируется когда прерывается трассировка составов объектов
  /// </summary>
  [CustomDescription("MRP_35")]
  [Description("Событие генерируется когда прерывается трассировка составов объектов")]
  public event CompositionTracingEventHandler TracingStopped;

  /// <summary>
  /// Генерируется когда полностью завершается трассировка всех объектов
  /// </summary>
  [CustomDescription("MRP_32")]
  [Description("Генерируется когда полностью завершается трассировка всех объектов")]
  public event TracingAllObjectsCompleteEventHandler TracingAllObjectsComplete;

  /// <summary>
  /// Генерируется когда в гриде выбрана строка, содержащая ссылку на объект состава, который требуется отобразить в дереве Навигатора и на закладках
  /// </summary>
  [CustomDescription("MRP_33")]
  [Description("Генерируется когда в гриде выбрана строка, содержащая ссылку на объект состава, который требуется отобразить в дереве Навигатора и на закладках")]
  public event TracingObjectSelectedEventHandler TracingObjectSelected;

  /// <summary>Контейнер сервисов</summary>
  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  [CustomDescription("MRP_27")]
  [Description("Контейнер сервисов")]
  public IServiceProvider Services
  {
    [DebuggerStepThrough] get => (IServiceProvider) this._services;
    set
    {
      this.ReleaseServices();
      this._services.AdvancedProvider = value;
      this.InitializeServices();
    }
  }

  /// <summary>Есть ли ошибки в редакторе</summary>
  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public bool ErrorsInEditor
  {
    [DebuggerStepThrough] get
    {
      lock (this._syncRoot)
        return this._errorsInEditor;
    }
    set
    {
      lock (this._syncRoot)
        this._errorsInEditor = value;
      this.UpdateControls();
    }
  }

  /// <summary>
  /// Состояния текущей задачи (для каждого протрассированного объекта - своё состояние)
  /// </summary>
  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public Dictionary<CompositionObject, PdmCompositionBrowserJobStatus> TaskStatuses
  {
    [DebuggerStepThrough] get
    {
      if (this.InProgress)
        return (Dictionary<CompositionObject, PdmCompositionBrowserJobStatus>) null;
      lock (this._syncRoot)
        return this._taskStatuses;
    }
  }

  /// <summary>
  /// Осуществляется ли трассировка составов в настоящий момент времени
  /// </summary>
  [Category("Appearance")]
  [Browsable(true)]
  [CustomDescription("MRP_26")]
  [Description("Осуществляется ли трассировка составов в настоящий момент времени")]
  public bool InProgress
  {
    [DebuggerStepThrough] get
    {
      lock (this._syncRoot)
        return this._inProgress || this._taskThread != null;
    }
  }

  /// <summary>
  /// Была ли прервана трассировка составов с момента её начала (true означает то, что трассировка не была завершена до конца)
  /// </summary>
  [Category("Appearance")]
  [Browsable(true)]
  [CustomDescription("MRP_42")]
  [Description("Была ли прервана трассировка составов с момента её начала (true означает то, что трассировка не была завершена до конца)")]
  public bool IsTerminated
  {
    [DebuggerStepThrough] get
    {
      lock (this._syncRoot)
        return this._isTerminated;
    }
  }

  /// <summary>
  /// Инициализировать компонент kисходными данными.
  /// Если выполняется трассировка составов, она будет прервана
  /// </summary>
  /// <param name="filtrationOwnerID">Ключ настроек фильтрации составов</param>
  /// <param name="rootPath">Путь к корневому объекту конфигурируемого состава (без учёта самих трассируемых объектов)</param>
  /// <param name="items">Коллекция выделенных элементов, состав которых надо протрассировать</param>
  public void Initialize(string filtrationOwnerID, RelationPath rootPath, ISelectedItems items)
  {
    if (this.InProgress)
      this.TraceStop();
    this._filtrationOwnerID = filtrationOwnerID;
    this._rootPath = rootPath;
    this._errorsInEditor = false;
    if (rootPath == null)
    {
      if (this.Services == null)
        return;
      this._rootPath = this.Services.GetService(typeof (NavigatorTreeView)) is NavigatorTreeView service ? NavigatorTreeViewHelper.GetCompositionNodePath(service.FocusedNode) : (RelationPath) null;
    }
    this._rootPath = this._rootPath ?? new RelationPath();
    this._compositionObjects = this._compositionObjects ?? new CompositionObjects();
    this._compositionObjects.Clear();
    if (items == null || items.Count == 0)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      for (int index = 0; index < items.Count; ++index)
      {
        IDBTypedObjectID itemData1 = items.GetItemData(index, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
        IDBLCStepID itemData2 = items.GetItemData(index, typeof (IDBLCStepID)) as IDBLCStepID;
        IDBCheckedOutByID itemData3 = items.GetItemData(index, typeof (IDBCheckedOutByID)) as IDBCheckedOutByID;
        IDBRelationID itemData4 = items.GetItemData(index, typeof (IDBRelationID)) as IDBRelationID;
        if (itemData1 != null && itemData4 != null && (MetaDataHelper.IsPdmRootObjectType(itemData1.ObjectType) || MetaDataHelper.IsPdmConfigurableObjectType(itemData1.ObjectType)))
        {
          IDBObject dbObject = itemData4.ProjID != 0L ? sessionKeeper.Session.GetObject(itemData4.ProjID) : sessionKeeper.Session.GetObject(itemData1.ObjectID);
          if (dbObject != null)
          {
            RelationPair relationPair = new RelationPair(sessionKeeper.Session.ClientConnectionID, dbObject.ObjectID, dbObject.ObjectType, 0L, sessionKeeper.Session.UserID, dbObject.ObjectID, -1, dbObject.ObjectType);
            CompositionObject key = new CompositionObject(itemData1.ID, itemData1.ObjectID, itemData1.ObjectType, itemData2 != null ? itemData2.LCStepID : -1, itemData1.Owner, itemData3 != null ? itemData3.CheckedOutBy : 0L, itemData1.Caption, itemData1.Version, itemData1.ModificationID, itemData1.BaseVersion, ObjectVersionDescriptionOptions.None, (CompositionObjects) null, itemData4.Value, itemData4.PartID, itemData4.RelationType, string.Empty);
            key.Tag = (object) relationPair;
            this._compositionObjects.Add(key);
            this._taskStatuses[key] = (PdmCompositionBrowserJobStatus) null;
          }
        }
      }
    }
  }

  /// <summary>Начать трассировку</summary>
  public void TraceStart()
  {
    this.TraceStop();
    this.ClearTracingGrid();
    ManufactureOrderHolder service = this.Services.GetService(typeof (ManufactureOrderHolder)) as ManufactureOrderHolder;
    CurrentEditingContext editingContext = CurrentEditingContext.Dummy;
    if (service != null && service.FiltrationSettings != null && service.FiltrationSettings.EditingContext != null)
      editingContext = service.FiltrationSettings.EditingContext;
    lock (this._syncRoot)
    {
      this._taskThread = new Thread(editingContext.SendToThread(new ThreadStart(this.TracingTask)));
      this._taskThread.IsBackground = true;
      this._taskThread.Priority = ThreadPriority.Lowest;
      this._taskThread.Name = "Intermech.MRP.Orders.TracingTask";
      this._taskThread.Start();
    }
    this.UpdateControls();
  }

  private void BarManager_RendererChanged(object sender, EventArgs e)
  {
    IToolBarRenderer renderer = (sender as BarManager).Renderer;
    this.toolBarTrace.Renderer = renderer;
    this.toolbarFilter.Renderer = renderer;
  }

  private void Grid_ColWidthChanging(object sender, iGColWidthEventArgs e)
  {
    this._columnWidthDictionaryByKey[this._grid.Cols[e.ColIndex].Key] = e.Width;
    this.CorrectColsWidth();
  }

  private void Grid_ColWidthEndChange(object sender, iGColWidthEventArgs e)
  {
    this._columnWidthDictionaryByKey[this._grid.Cols[e.ColIndex].Key] = e.Width;
    this.CorrectColsWidth();
  }

  private void Grid_Resize(object sender, EventArgs e) => this.CorrectColsWidth();

  private void Grid_CellClick(object sender, iGCellClickEventArgs e)
  {
    if (this.InProgress || this._grid.Rows[e.RowIndex].Level != 2)
      return;
    CompositionObject parent = this._grid.Cells[e.RowIndex, "PARENT"].Value as CompositionObject;
    RelationPath path = this._grid.Cells[e.RowIndex, "RELATION_PATH"].Value as RelationPath;
    if (parent == null || path == null)
      return;
    this.OnTracingObjectSelected((CompositionTracingEventArgs) new TracingObjectSelectedEventArgs(parent, path));
  }

  private void Grid_SelectionChanged(object sender, EventArgs e)
  {
    if (this._grid.SelectedCells.Count == 0)
      return;
    this.SetTraceLogRowVisibility(this._grid.SelectedCells[0].Row);
  }

  private void StartTracingButton_Click(object sender, EventArgs e)
  {
    if (this.InProgress)
      return;
    this.OnTracingStarted();
    this.TraceStart();
  }

  private void StopTracingButton_Click(object sender, EventArgs e)
  {
    if (!this.InProgress)
      return;
    this.TraceStop();
    this.OnTracingStopped();
  }

  private void ExpandAllButton_Click(object sender, EventArgs e)
  {
    if (this.InProgress)
      return;
    this._grid.PerformAction(iGActions.ExpandAll);
    this.UpdateControls();
  }

  private void CollapseAllButton_Click(object sender, EventArgs e)
  {
    if (this.InProgress)
      return;
    this._grid.PerformAction(iGActions.CollapseAll);
    this.UpdateControls();
  }

  private void ClearButton_Click(object sender, EventArgs e)
  {
    if (this.InProgress)
      return;
    this.ClearTracingGrid();
  }

  private void HideLegendButton_Click(object sender, EventArgs e)
  {
    this.panelLegend.Visible = false;
    CompositionTracing._hiddenLegendPanel = true;
  }

  private void DoCorrectVisibility(object sender, EventArgs e) => this.SetTraceLogRowVisibility();

  /// <summary>Прервать трассировку</summary>
  private void TraceStop()
  {
    lock (this._syncRoot)
    {
      this._isTerminated = true;
      this._inProgress = false;
      this._taskThread = (Thread) null;
      this._taskStatuses.Clear();
      this._taskGuid = Guid.Empty;
    }
    this.UpdateControls();
  }

  /// <summary>
  /// Получить значение задержки (мс) для указанной итерации
  /// </summary>
  /// <param name="iteration">Итерация</param>
  /// <returns>Задержка для волшебного Sleep, мс</returns>
  private int GetSleepValue(int iteration)
  {
    return iteration < 0 || iteration >= CompositionTracing.Delays.Length ? 1000 : CompositionTracing.Delays[iteration];
  }

  /// <summary>Метод фоновой задачи по трассировке составов</summary>
  private void TracingTask()
  {
    lock (this._syncRoot)
    {
      this._inProgress = true;
      this._isTerminated = true;
      this._taskStatuses.Clear();
      this._taskGuid = Guid.Empty;
    }
    bool flag = false;
    try
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IMRPSettings service1 = ServicesManager.GetService(typeof (IMRPSettings)) as IMRPSettings;
        if (!(sessionKeeper.Session.GetCustomService(typeof (IPdmConfiguratorService)) is IPdmConfiguratorService customService))
          return;
        customService.ResetSessionCache((object) sessionKeeper.Session.SessionGUID);
        CompositionObjects objs = new CompositionObjects();
        this._compositionObjects = this._compositionObjects ?? new CompositionObjects();
        long num = 0;
        try
        {
          this.Invoke((Delegate) new CompositionTracingMethodInvoker3(this.UpdateStatusPanel), (object) num, (object) 0, (object) this._compositionObjects.Count);
        }
        catch
        {
        }
        for (int index = 0; index < this._compositionObjects.Count; ++index)
        {
          lock (this._syncRoot)
          {
            if (this._taskThread == null)
            {
              flag = true;
              break;
            }
          }
          CompositionObject compositionObject1 = this._compositionObjects[index];
          RelationPair tag = compositionObject1.Tag as RelationPair;
          RelationPath rootObjectPath = new RelationPath((object) this._rootPath);
          rootObjectPath.Items.Add(new SimpleRelationPair(compositionObject1.F_PRJLINK_ID, compositionObject1.F_RELATION_TYPE, compositionObject1.F_OBJECT_ID, compositionObject1.F_OBJECT_TYPE));
          compositionObject1.Tag = (object) null;
          objs.Clear();
          objs.Add(compositionObject1);
          ManufactureOrderHolder service2 = this.Services.GetService(typeof (ManufactureOrderHolder)) as ManufactureOrderHolder;
          HybridDictionary tags = new HybridDictionary();
          if (service2 != null && service2.FiltrationSettings != null && service2.FiltrationSettings.Tags != null)
          {
            foreach (object key in (IEnumerable) service2.FiltrationSettings.Tags.Keys)
              tags[key] = service2.FiltrationSettings.Tags[key];
          }
          if (service1 != null)
          {
            if (!service1.UseBoughtArticles)
              tags[(object) "{78C6A7F1-3B57-4CF9-8E3C-B5D308593A6B}"] = (object) true;
            if (!service1.UseSubstitutes)
              tags[(object) "{7C0E9952-C5C7-4505-AA53-2F662A4E9D2B}"] = (object) true;
          }
          this._taskGuid = customService.Browse(sessionKeeper.Session.SessionGUID, tag, rootObjectPath, objs, new PdmCompositionBrowserEventArgs(-1, this._filtrationOwnerID, (VersionsRule) null, tags, true));
          PdmCompositionBrowserJobStatus browserJobStatus = (PdmCompositionBrowserJobStatus) null;
          int iteration = 0;
          while (true)
          {
            lock (this._syncRoot)
            {
              if (this._taskThread == null || this._taskGuid == Guid.Empty)
              {
                flag = true;
                break;
              }
              browserJobStatus = customService.QueryBrowserStatus(this._taskGuid);
            }
            if (browserJobStatus != null)
            {
              lock (this._syncRoot)
              {
                compositionObject1.Tag = (object) tag;
                this._taskStatuses[compositionObject1] = browserJobStatus;
              }
              if (browserJobStatus.Progress == PdmCompositionBrowserJobProgress.NotStarted || browserJobStatus.Progress == PdmCompositionBrowserJobProgress.Working)
              {
                Thread.Sleep(this.GetSleepValue(iteration));
                ++iteration;
              }
              else
                break;
            }
            else
              break;
          }
          if (!flag && browserJobStatus != null && (browserJobStatus.Progress == PdmCompositionBrowserJobProgress.Completed || browserJobStatus.Progress == PdmCompositionBrowserJobProgress.Error || browserJobStatus.Progress == PdmCompositionBrowserJobProgress.Cancelled))
          {
            if (browserJobStatus.Trace != null && browserJobStatus.Trace.Items != null)
              num += (long) browserJobStatus.Trace.Items.Count;
            CompositionObject compositionObject2 = compositionObject1.Clone() as CompositionObject;
            compositionObject2.Tag = (object) null;
            TracingObjectCompleteEventArgs completeEventArgs = new TracingObjectCompleteEventArgs(tag != null ? tag.Clone() as RelationPair : (RelationPair) null, compositionObject2, this._rootPath.Clone() as RelationPath, browserJobStatus.Clone() as PdmCompositionBrowserJobStatus);
            try
            {
              this.Invoke((Delegate) new CompositionTracingMethodInvoker(this.OnTracingObjectComplete), (object) completeEventArgs);
              this.Invoke((Delegate) new CompositionTracingMethodInvoker2(this.FillTracingGrid), (object) compositionObject1, (object) browserJobStatus);
              this.Invoke((Delegate) new CompositionTracingMethodInvoker3(this.UpdateStatusPanel), (object) num, (object) index, (object) this._compositionObjects.Count);
            }
            catch
            {
            }
          }
        }
      }
    }
    finally
    {
      lock (this._syncRoot)
      {
        this._inProgress = false;
        this._isTerminated = false;
        this._taskThread = (Thread) null;
      }
      this._errorsInEditor = this.CheckForErrors();
      try
      {
        this.Invoke((Delegate) new MethodInvoker(this.UpdateControls));
      }
      catch
      {
      }
      if (!flag)
      {
        TracingAllObjectsCompleteEventArgs completeEventArgs = new TracingAllObjectsCompleteEventArgs(this._taskStatuses);
        try
        {
          this.Invoke((Delegate) new CompositionTracingMethodInvoker(this.OnTracingAllObjectsComplete), (object) completeEventArgs);
        }
        catch
        {
        }
      }
    }
  }

  /// <summary>
  /// Проверить на наличие ошибок результаты трассировки.
  /// Если трассировка ещё не завершена, метод вернёт true
  /// </summary>
  /// <returns>true - ошибки найдены</returns>
  private bool CheckForErrors()
  {
    if (this.InProgress)
      return true;
    lock (this._syncRoot)
    {
      if (this._taskStatuses == null || this._taskStatuses.Count == 0)
        return false;
      foreach (KeyValuePair<CompositionObject, PdmCompositionBrowserJobStatus> taskStatuse in this._taskStatuses)
      {
        SortedDictionary<RelationPath, TraceEntry> items = taskStatuse.Value == null || taskStatuse.Value.Trace == null ? (SortedDictionary<RelationPath, TraceEntry>) null : taskStatuse.Value.Trace.Items;
        if (items != null)
        {
          foreach (RelationPath key in items.Keys)
          {
            TraceEntry traceEntry = items[key];
            if (traceEntry.Flags != PdmConfiguratorResult.Unknown)
            {
              bool flag = (traceEntry.Trace & PdmCompositionTraceResult.PdmConfiguratorError) == PdmCompositionTraceResult.PdmConfiguratorError || traceEntry.Flags == PdmConfiguratorResult.ApplOptionNotFound || traceEntry.Flags == PdmConfiguratorResult.ApplOptionValueNotFound || traceEntry.Flags == PdmConfiguratorResult.ConflictOptionNotFound || traceEntry.Flags == PdmConfiguratorResult.ConflictOptionValueNotFound || traceEntry.Flags == PdmConfiguratorResult.ContextNotFound || traceEntry.Flags == PdmConfiguratorResult.Exception || traceEntry.Flags == PdmConfiguratorResult.OptionNotFound || traceEntry.Flags == PdmConfiguratorResult.OptionValueNotFound;
              if (flag)
                return flag;
            }
          }
        }
      }
    }
    return false;
  }

  /// <summary>Создать в гридах колонки</summary>
  private void PrepareGridsColumns()
  {
    this._groupRowsLevel1.BackColor = SystemColors.ControlLight;
    this._groupRowsLevel1.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
    this._groupRowsLevel1.ReadOnly = iGBool.True;
    this._groupRowsLevel1.TextAlign = iGContentAlignment.MiddleLeft;
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
    iGcellStyle3.ImageList = this._objtypesIcons != null ? this._objtypesIcons.ImageList : (ImageList) null;
    if (this._columnWidthDictionaryByKey.Count == 0)
      this._columnWidthDictionaryByKey = new Dictionary<string, int>()
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
          300
        },
        {
          "MESSAGE",
          300
        },
        {
          "RELATION_PATH",
          0
        },
        {
          "TRACE",
          0
        },
        {
          "PARENT",
          0
        }
      };
    iGCol col1 = this._grid.Cols["OBJECT_TYPE_ICON"];
    iGCol iGcol1 = this._grid.Cols["OBJECT_TYPE_ICON"] ?? this._grid.Cols.Add(new iGColPattern(this._columnWidthDictionaryByKey["OBJECT_TYPE_ICON"], true, true, 32 /*0x20*/, 32 /*0x20*/, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) string.Empty, "OBJECT_TYPE_ICON", -1, (object) null, (object) null, -1));
    iGcol1.Width = this._columnWidthDictionaryByKey["OBJECT_TYPE_ICON"];
    iGcol1.CellStyle = iGcellStyle3;
    iGCol col2 = this._grid.Cols["OBJECT_ID"];
    iGCol iGcol2 = this._grid.Cols["OBJECT_ID"] ?? this._grid.Cols.Add(new iGColPattern(this._columnWidthDictionaryByKey["OBJECT_ID"], true, true, 50, 200, true, false, false, iGSortType.ByValue, iGSortOrder.Ascending, false, (object) null, (object) LocalizationHolder.rm.GetString("MRP_36"), "OBJECT_ID", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol2.CellStyle = iGcellStyle2;
    iGcol2.Width = this._columnWidthDictionaryByKey["OBJECT_ID"];
    iGCol col3 = this._grid.Cols["RELATION_ID"];
    iGCol iGcol3 = this._grid.Cols["RELATION_ID"] ?? this._grid.Cols.Add(new iGColPattern(this._columnWidthDictionaryByKey["RELATION_ID"], true, true, 50, 200, true, false, false, iGSortType.ByValue, iGSortOrder.Ascending, false, (object) null, (object) LocalizationHolder.rm.GetString("MRP_37"), "RELATION_ID", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol3.CellStyle = iGcellStyle2;
    iGcol3.Width = this._columnWidthDictionaryByKey["RELATION_ID"];
    iGCol col4 = this._grid.Cols["OBJECT_CAPTION"];
    iGCol iGcol4 = this._grid.Cols["OBJECT_CAPTION"] ?? this._grid.Cols.Add(new iGColPattern(this._columnWidthDictionaryByKey["OBJECT_CAPTION"], true, true, 200, -1, true, false, false, iGSortType.ByValue, iGSortOrder.Ascending, false, (object) null, (object) LocalizationHolder.rm.GetString("MRP_38"), "OBJECT_CAPTION", -1, (object) null, (object) null, -1));
    iGcol4.Width = this._columnWidthDictionaryByKey["OBJECT_CAPTION"];
    iGcol4.CellStyle = iGcellStyle2;
    iGCol col5 = this._grid.Cols["MESSAGE"];
    iGCol iGcol5 = this._grid.Cols["MESSAGE"] ?? this._grid.Cols.Add(new iGColPattern(this._columnWidthDictionaryByKey["MESSAGE"], true, true, 300, -1, true, false, false, iGSortType.ByValue, iGSortOrder.Ascending, false, (object) null, (object) LocalizationHolder.rm.GetString("MRP_39"), "MESSAGE", -1, (object) null, (object) null, -1));
    iGcol5.Width = this._columnWidthDictionaryByKey["MESSAGE"];
    iGcol5.CellStyle = iGcellStyle1;
    iGCol col6 = this._grid.Cols["RELATION_PATH"];
    (this._grid.Cols["RELATION_PATH"] ?? this._grid.Cols.Add(new iGColPattern(this._columnWidthDictionaryByKey["RELATION_PATH"], false, false, 0, 0, true, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) string.Empty, "RELATION_PATH", -1, (object) null, (object) null, -1))).Width = this._columnWidthDictionaryByKey["RELATION_PATH"];
    iGCol col7 = this._grid.Cols["TRACE"];
    (this._grid.Cols["TRACE"] ?? this._grid.Cols.Add(new iGColPattern(this._columnWidthDictionaryByKey["TRACE"], false, false, 0, 0, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) string.Empty, "TRACE", -1, (object) null, (object) null, -1))).Width = this._columnWidthDictionaryByKey["TRACE"];
    iGCol col8 = this._grid.Cols["PARENT"];
    (this._grid.Cols["PARENT"] ?? this._grid.Cols.Add(new iGColPattern(this._columnWidthDictionaryByKey["PARENT"], false, false, 0, 0, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) string.Empty, "PARENT", -1, (object) null, (object) null, -1))).Width = this._columnWidthDictionaryByKey["PARENT"];
    this.CorrectColsWidth();
  }

  /// <summary>Откорректировать ширину колонок в гриде</summary>
  private void CorrectColsWidth()
  {
    if (this._grid.AutoResizeCols || this._columnWidthDictionaryByKey.Count == 0)
      return;
    int num = this._grid.ClientRectangle.Width - 30 - this._columnWidthDictionaryByKey["OBJECT_TYPE_ICON"] - this._columnWidthDictionaryByKey["OBJECT_ID"] - this._columnWidthDictionaryByKey["RELATION_ID"] - this._columnWidthDictionaryByKey["OBJECT_CAPTION"];
    if (this._grid.Cols.Count == 0)
      return;
    this._grid.Cols["OBJECT_CAPTION"].Width = this._columnWidthDictionaryByKey["OBJECT_CAPTION"];
    this._grid.Cols["OBJECT_ID"].Width = this._columnWidthDictionaryByKey["OBJECT_ID"];
    this._grid.Cols["RELATION_ID"].Width = this._columnWidthDictionaryByKey["RELATION_ID"];
    if (num > 200)
      this._grid.Cols["MESSAGE"].Width = this._columnWidthDictionaryByKey["MESSAGE"] = num;
    else
      this._grid.Cols["MESSAGE"].Width = this._columnWidthDictionaryByKey["MESSAGE"];
  }

  /// <summary>
  /// Откорректировать видимость для всех строк согласно указанным настройкам фильтрации
  /// </summary>
  private void SetTraceLogRowVisibility()
  {
    if (this.InProgress)
      return;
    for (int index = 0; index < this._grid.Rows.Count; ++index)
    {
      iGRow row = this._grid.Rows[index];
      if (row.Level == 2 && row.Cells["TRACE"].Value is TraceEntry entry)
        this.SetTraceLogRowVisibility(entry, row);
    }
  }

  /// <summary>Откорректировать видимость для строки</summary>
  /// <param name="row">Строка, содержащая указанную запись</param>
  private void SetTraceLogRowVisibility(iGRow row)
  {
    if (row == null || row.Level != 2 || !(row.Cells["TRACE"].Value is TraceEntry entry))
      return;
    this.SetTraceLogRowVisibility(entry, row);
  }

  /// <summary>
  /// Откорректировать видимость для строки, содержащей указанную запись протокола трассировки
  /// </summary>
  /// <param name="entry">Запись протокола трассировки</param>
  /// <param name="row">Строка, содержащая указанную запись</param>
  private void SetTraceLogRowVisibility(TraceEntry entry, iGRow row)
  {
    if (entry == null || row == null || row.Level != 2)
      return;
    object obj = this._grid.Cells[row.Index, "PARENT"].Value;
    RelationPath path = this._grid.Cells[row.Index, "RELATION_PATH"].Value as RelationPath;
    row.Cells["OBJECT_CAPTION"].ImageList = this.ilState;
    bool hasErrors = entry.HasErrors;
    bool hasWarningsOnly = entry.HasWarningsOnly;
    bool hasInformationOnly = entry.HasInformationOnly;
    if (this._holder == null)
      this._holder = this.Services.GetService(typeof (ManufactureOrderHolder)) as ManufactureOrderHolder;
    if (hasWarningsOnly && this._holder != null)
    {
      long prjLinkID = DataSetProcessor.GetInt64Value(this._grid.Cells[row.Index, "RELATION_ID"].Value, 0L);
      if (path.Items.Count > 1 && path.Items[path.Items.Count - 2].F_PRJLINK_ID != 0L)
        prjLinkID = path.Items[path.Items.Count - 2].F_PRJLINK_ID;
      SubstitutesItemSettings relationSetting = this._holder.GetRelationSetting(prjLinkID, typeof (SubstitutesItemSettings)) as SubstitutesItemSettings;
      TechnologicalItemSettings pathSetting = this._holder.GetPathSetting(path, typeof (TechnologicalItemSettings)) as TechnologicalItemSettings;
      int num1 = 13;
      int num2 = (entry.Trace & PdmCompositionTraceResult.HasSubstitutes) != PdmCompositionTraceResult.HasSubstitutes ? 0 : (relationSetting == null ? 1 : 0);
      bool flag = (entry.Trace & PdmCompositionTraceResult.HasSomeRoutes) == PdmCompositionTraceResult.HasSomeRoutes && pathSetting == null;
      if (num2 == 0 && !flag)
        num1 = 14;
      row.Cells["OBJECT_CAPTION"].ImageIndex = num1;
    }
    if (hasErrors)
      row.Cells["OBJECT_CAPTION"].ImageIndex = 12;
    switch (entry.Flags)
    {
      case PdmConfiguratorResult.True:
        row.Cells["MESSAGE"].ImageIndex = 5;
        break;
      case PdmConfiguratorResult.ContextNotFound:
        row.Cells["MESSAGE"].ImageIndex = 1;
        break;
      case PdmConfiguratorResult.OptionNotFound:
      case PdmConfiguratorResult.ConflictOptionNotFound:
      case PdmConfiguratorResult.ApplOptionNotFound:
        row.Cells["MESSAGE"].ImageIndex = 3;
        break;
      case PdmConfiguratorResult.OptionValueNotFound:
      case PdmConfiguratorResult.ConflictOptionValueNotFound:
      case PdmConfiguratorResult.ApplOptionValueNotFound:
        row.Cells["MESSAGE"].ImageIndex = 4;
        break;
      case PdmConfiguratorResult.Incompatibles:
        row.Cells["MESSAGE"].ImageIndex = 0;
        break;
      case PdmConfiguratorResult.Exception:
        row.Cells["MESSAGE"].ImageIndex = 2;
        break;
      default:
        row.Cells["MESSAGE"].ImageIndex = 6;
        break;
    }
    if (this.btErrors.Checked & hasErrors)
      row.Visible = true;
    else if (this.btWarnings.Checked & hasWarningsOnly)
    {
      row.Visible = true;
      if ((entry.Trace & PdmCompositionTraceResult.HasSomeRoutes) == PdmCompositionTraceResult.HasSomeRoutes)
        row.Cells["MESSAGE"].ImageIndex = 9;
      if ((entry.Trace & PdmCompositionTraceResult.HasSubstitutes) != PdmCompositionTraceResult.HasSubstitutes)
        return;
      row.Cells["MESSAGE"].ImageIndex = 8;
    }
    else
    {
      if (hasInformationOnly)
      {
        row.Cells["MESSAGE"].ImageIndex = 7;
        if ((entry.Trace & PdmCompositionTraceResult.NotManufacturingLevel) == PdmCompositionTraceResult.NotManufacturingLevel)
          row.Cells["MESSAGE"].ImageIndex = 10;
      }
      row.Visible = this.btInformation.Checked & hasInformationOnly;
    }
  }

  /// <summary>
  /// Добавить в грид строчку с информацией об очередном родительском объекте, состав которого трассировался
  /// </summary>
  /// <param name="session">Сессия</param>
  /// <param name="obj">Родительский объект, состав которого трассировался</param>
  /// <returns>Строка грида или null</returns>
  private iGRow AddCompositionObjectRow(IUserSession session, CompositionObject obj)
  {
    if (session == null || obj == null || obj.F_OBJECT_ID == 0L)
      return (iGRow) null;
    iGRow iGrow = this._grid.Rows.Add();
    iGrow.Tag = (object) obj;
    iGrow.Type = iGRowType.AutoGroupRow;
    iGrow.Level = 1;
    iGrow.RowTextCell.Value = (object) obj.CAPTION;
    iGrow.RowTextCell.Style = (iGCellStyle) this._groupRowsLevel1;
    iGrow.RowTextCell.ImageList = this._objtypesIcons != null ? this._objtypesIcons.ImageList : (ImageList) null;
    iGrow.TreeButton = iGTreeButtonState.Visible;
    iGrow.Expanded = true;
    for (int colIndex = 0; colIndex < iGrow.Cells.Count; ++colIndex)
      iGrow.Cells[colIndex].Style = (iGCellStyle) this._groupRowsLevel1;
    return iGrow;
  }

  /// <summary>
  /// Добавить в грид строчку с информацией о трассировке очередного объекта состава
  /// </summary>
  /// <param name="session">Сессия</param>
  /// <param name="obj">Родительский объект, состав которого трассировался</param>
  /// <param name="path">Путь к указанному узлу</param>
  /// <param name="entry">Результат трассировки объекта состава</param>
  /// <returns>Строка грида или null</returns>
  private iGRow AddTraceLogEntryRow(
    IUserSession session,
    CompositionObject obj,
    RelationPath path,
    TraceEntry entry)
  {
    if (session == null || path == null || path.Empty || entry == null || entry.Flags == PdmConfiguratorResult.Unknown)
      return (iGRow) null;
    iGRow row = this._grid.Rows.Add();
    row.Tag = (object) entry;
    row.Type = iGRowType.Normal;
    row.Level = 2;
    row.TreeButton = iGTreeButtonState.Hidden;
    row.Cells["MESSAGE"].Value = !string.IsNullOrEmpty(entry.Message) ? (object) entry.Message : (object) LocalizationHolder.rm.GetString("MRP_40");
    row.Cells["RELATION_PATH"].Value = (object) path;
    row.Cells["PARENT"].Value = (object) obj;
    row.Cells["TRACE"].Value = (object) entry;
    long fPartId = path.Items[path.Items.Count - 1].F_PART_ID;
    int fObjectType = path.Items[path.Items.Count - 1].F_OBJECT_TYPE;
    row.Cells["RELATION_ID"].Value = (object) path.Items[path.Items.Count - 1].F_PRJLINK_ID;
    row.Cells["OBJECT_TYPE_ICON"].ImageIndex = this._objtypesIcons != null ? this._objtypesIcons.IndexOf(4, fObjectType) : -1;
    QuickObjectInfo objectInfo = session.GetObjectInfo(fPartId);
    row.Cells["OBJECT_CAPTION"].Value = !objectInfo.Empty ? (object) objectInfo.Caption : (object) string.Format(LocalizationHolder.rm.GetString("MRP_41"), (object) fPartId);
    row.Cells["OBJECT_ID"].Value = (object) objectInfo.ObjectID;
    this.SetTraceLogRowVisibility(entry, row);
    return row;
  }

  /// <summary>Очистить грид от результатов трассировки</summary>
  private void ClearTracingGrid()
  {
    this._grid.Rows.Clear();
    lock (this._syncRoot)
    {
      Dictionary<CompositionObject, PdmCompositionBrowserJobStatus> dictionary = new Dictionary<CompositionObject, PdmCompositionBrowserJobStatus>(this._taskStatuses.Count);
      foreach (KeyValuePair<CompositionObject, PdmCompositionBrowserJobStatus> taskStatuse in this._taskStatuses)
        dictionary[taskStatuse.Key] = (PdmCompositionBrowserJobStatus) null;
      this._taskStatuses = dictionary;
    }
    this.ErrorsInEditor = false;
    this.UpdateButtons();
    this.UpdateControls();
  }

  /// <summary>
  /// Заполнить грид информацией с результатами трассировки для указанного объекта
  /// </summary>
  /// <param name="obj">Родительский объект состава</param>
  /// <param name="status">Результаты трассировки</param>
  private void FillTracingGrid(CompositionObject obj, PdmCompositionBrowserJobStatus status)
  {
    if (obj == null || status == null)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      this.AddCompositionObjectRow(sessionKeeper.Session, obj);
      SortedDictionary<RelationPath, TraceEntry> items = status.Trace.Items;
      foreach (RelationPath key in items.Keys)
      {
        TraceEntry entry = items[key];
        if (entry.Flags != PdmConfiguratorResult.Unknown)
          this.AddTraceLogEntryRow(sessionKeeper.Session, obj, key, entry);
      }
    }
  }

  /// <summary>Заполнить грид информацией с результатами трассировки</summary>
  private void FillTracingGrid()
  {
    this.ClearTracingGrid();
    if (this._taskStatuses == null || this._taskStatuses.Count == 0)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      foreach (KeyValuePair<CompositionObject, PdmCompositionBrowserJobStatus> taskStatuse in this._taskStatuses)
      {
        this.AddCompositionObjectRow(sessionKeeper.Session, taskStatuse.Key);
        SortedDictionary<RelationPath, TraceEntry> items = taskStatuse.Value.Trace.Items;
        foreach (RelationPath key in items.Keys)
        {
          TraceEntry entry = items[key];
          if (entry.Flags != PdmConfiguratorResult.Unknown)
            this.AddTraceLogEntryRow(sessionKeeper.Session, taskStatuse.Key, key, entry);
        }
      }
    }
  }

  /// <summary>Обновить состояние контролов</summary>
  private void UpdateControls()
  {
    this._startTracingButton.Enabled = !this.InProgress;
    this._stopTracingButton.Enabled = !this._startTracingButton.Enabled;
    this._expandAllButton.Enabled = this._startTracingButton.Enabled && this._grid.Rows.Count > 0;
    this._collapseAllButton.Enabled = this._expandAllButton.Enabled;
    this._clearButton.Enabled = this._startTracingButton.Enabled;
    this.panelStatus.Visible = !this._startTracingButton.Enabled;
    this.toolbarFilter.Visible = true;
    this.btErrors.Enabled = this._startTracingButton.Enabled;
    this.btWarnings.Enabled = this.btErrors.Enabled;
    this.btInformation.Enabled = this.btErrors.Enabled;
    this.panelLegend.Visible = !CompositionTracing._hiddenLegendPanel;
  }

  /// <summary>Заполнить статусную панель</summary>
  /// <param name="totalObjects">Суммарное количество обработанных объектов</param>
  /// <param name="progress">Текущее состояние для индикатора прогресса</param>
  /// <param name="maxProgress">Максимальное значение для индикатора прогресса</param>
  private void UpdateStatusPanel(long totalObjects, int progress, int maxProgress)
  {
    this.labelQuantity.Text = totalObjects.ToString();
    this.progressBar.Maximum = maxProgress;
    this.progressBar.Value = progress;
    this.UpdateButtons();
  }

  /// <summary>
  /// Обновить заголовки кнопок "Ошибки", "Предупреждения", "Информация"
  /// </summary>
  private void UpdateButtons()
  {
    long num1 = 0;
    long num2 = 0;
    long num3 = 0;
    lock (this._syncRoot)
    {
      foreach (KeyValuePair<CompositionObject, PdmCompositionBrowserJobStatus> taskStatuse in this._taskStatuses)
      {
        if (taskStatuse.Value != null && taskStatuse.Value.Trace != null && taskStatuse.Value.Trace.Items != null && taskStatuse.Value.Trace.Items.Count != 0)
        {
          foreach (KeyValuePair<RelationPath, TraceEntry> keyValuePair in taskStatuse.Value.Trace.Items)
          {
            if (keyValuePair.Value.HasErrors)
              ++num1;
            if (keyValuePair.Value.HasWarningsOnly)
              ++num2;
            if (keyValuePair.Value.HasInformationOnly)
              ++num3;
          }
        }
      }
    }
    this.btErrors.Text = num1 == 0L ? LocalizationHolder.rm.GetString("MRP_43") : string.Format(LocalizationHolder.rm.GetString("MRP_46"), (object) num1);
    this.btWarnings.Text = num2 == 0L ? LocalizationHolder.rm.GetString("MRP_44") : string.Format(LocalizationHolder.rm.GetString("MRP_47"), (object) num2);
    this.btInformation.Text = num3 == 0L ? LocalizationHolder.rm.GetString("MRP_45") : string.Format(LocalizationHolder.rm.GetString("MRP_48"), (object) num3);
  }

  /// <summary>Инициализировать сервисы</summary>
  private void InitializeServices()
  {
    if (this._manufactOrdersChangedEventHandler != null || !(this.Services.GetService(typeof (ManufactOrdersEditor)) is ManufactOrdersEditor service))
      return;
    this._manufactOrdersChangedEventHandler = new ManufactOrdersChangedEventHandler(this.DoCorrectVisibility);
    service.Changed += this._manufactOrdersChangedEventHandler;
  }

  /// <summary>Освободить ссылки на сервисы</summary>
  private void ReleaseServices()
  {
    if (this._manufactOrdersChangedEventHandler == null || !(this.Services.GetService(typeof (ManufactOrdersEditor)) is ManufactOrdersEditor service))
      return;
    service.Changed -= this._manufactOrdersChangedEventHandler;
    this._manufactOrdersChangedEventHandler = (ManufactOrdersChangedEventHandler) null;
  }

  /// <summary>Сгенерировать событие "OnTracingStarted"</summary>
  private void OnTracingStarted()
  {
    if (this.TracingStarted == null)
      return;
    this.TracingStarted((object) this, new CompositionTracingEventArgs());
  }

  /// <summary>Сгенерировать событие "OnTracingStopped"</summary>
  private void OnTracingStopped()
  {
    if (this.TracingStopped == null)
      return;
    this.TracingStopped((object) this, new CompositionTracingEventArgs());
  }

  /// <summary>Сгенерировать событие "OnTracingObjectComplete"</summary>
  /// <param name="args">Аргументы события</param>
  private void OnTracingObjectComplete(CompositionTracingEventArgs args)
  {
    if (this.TracingObjectComplete == null)
      return;
    TracingObjectCompleteEventHandler tracingObjectComplete = this.TracingObjectComplete;
    if (!(args is TracingObjectCompleteEventArgs args1))
      args1 = new TracingObjectCompleteEventArgs();
    tracingObjectComplete((object) this, args1);
  }

  /// <summary>Сгенерировать событие "OnTracingAllObjectsComplete"</summary>
  /// <param name="args">Аргументы события</param>
  private void OnTracingAllObjectsComplete(CompositionTracingEventArgs args)
  {
    if (this.TracingAllObjectsComplete == null)
      return;
    TracingAllObjectsCompleteEventHandler allObjectsComplete = this.TracingAllObjectsComplete;
    if (!(args is TracingAllObjectsCompleteEventArgs args1))
      args1 = new TracingAllObjectsCompleteEventArgs();
    allObjectsComplete((object) this, args1);
  }

  /// <summary>Сгенерировать событие "OnTracingAllObjectsComplete"</summary>
  /// <param name="args">Аргументы события</param>
  private void OnTracingObjectSelected(CompositionTracingEventArgs args)
  {
    if (this.TracingObjectSelected == null)
      return;
    TracingObjectSelectedEventHandler tracingObjectSelected = this.TracingObjectSelected;
    if (!(args is TracingObjectSelectedEventArgs args1))
      args1 = new TracingObjectSelectedEventArgs();
    tracingObjectSelected((object) this, args1);
  }

  /// <summary>Освободить используемые ресурсы</summary>
  /// <param name="disposing">true - освободить управляемые ресурсы</param>
  protected override void Dispose(bool disposing)
  {
    if (disposing && ServicesManager.GetService(typeof (BarManager)) is BarManager service)
    {
      this.toolBarTrace.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
      this.toolbarFilter.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
      service.RendererChanged -= new EventHandler(this.BarManager_RendererChanged);
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
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (CompositionTracing));
    this.ilTracing = new ImageList(this.components);
    this.panelTrace = new Panel();
    this._grid = new iGrid();
    this.toolbarFilter = new Intermech.Bars.ToolBar();
    this.ilState = new ImageList(this.components);
    this.labelFilter = new LabelItem();
    this.btErrors = new ButtonItem();
    this.btWarnings = new ButtonItem();
    this.btInformation = new ButtonItem();
    this.panelStatus = new Panel();
    this.progressBar = new ProgressBar();
    this.labelQuantity = new Label();
    this.labelHint = new Label();
    this.toolBarTrace = new Intermech.Bars.ToolBar();
    this._startTracingButton = new ButtonItem();
    this._stopTracingButton = new ButtonItem();
    this._expandAllButton = new ButtonItem();
    this._collapseAllButton = new ButtonItem();
    this._clearButton = new ButtonItem();
    this.panelLegend = new Panel();
    this.lbLegendWarningOK = new Label();
    this.lbLegendWarning = new Label();
    this.lbLegendError = new Label();
    this.btnHideLegend = new Button();
    this.panelTrace.SuspendLayout();
    ((ISupportInitialize) this._grid).BeginInit();
    this.panelStatus.SuspendLayout();
    this.panelLegend.SuspendLayout();
    this.SuspendLayout();
    this.ilTracing.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("ilTracing.ImageStream");
    this.ilTracing.TransparentColor = Color.Transparent;
    this.ilTracing.Images.SetKeyName(0, "gear_stop.png");
    this.ilTracing.Images.SetKeyName(1, "gear_run.png");
    this.ilTracing.Images.SetKeyName(2, "Expand.ico");
    this.ilTracing.Images.SetKeyName(3, "Collapse.ico");
    this.ilTracing.Images.SetKeyName(4, "clean_err.ico");
    this.panelTrace.Controls.Add((Control) this._grid);
    this.panelTrace.Controls.Add((Control) this.toolbarFilter);
    this.panelTrace.Controls.Add((Control) this.panelStatus);
    this.panelTrace.Controls.Add((Control) this.toolBarTrace);
    componentResourceManager.ApplyResources((object) this.panelTrace, "panelTrace");
    this.panelTrace.Name = "panelTrace";
    this._grid.AutoResizeCols = true;
    this._grid.BackColorEvenRows = Color.WhiteSmoke;
    this._grid.DefaultRow.Height = (int) componentResourceManager.GetObject("resource.Height");
    this._grid.DefaultRow.NormalCellHeight = (int) componentResourceManager.GetObject("resource.NormalCellHeight");
    componentResourceManager.ApplyResources((object) this._grid, "_grid");
    this._grid.Header.Height = (int) componentResourceManager.GetObject("gridTrace.Header.Height");
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
    this._grid.SelectionChanged += new EventHandler(this.Grid_SelectionChanged);
    this._grid.Resize += new EventHandler(this.Grid_Resize);
    this.toolbarFilter.AddRemoveButtonsVisible = false;
    this.toolbarFilter.AllowHorizontalDock = false;
    componentResourceManager.ApplyResources((object) this.toolbarFilter, "toolbarFilter");
    this.toolbarFilter.DockLine = 3;
    this.toolbarFilter.DrawActionsButton = false;
    this.toolbarFilter.FullMenus = true;
    this.toolbarFilter.Guid = new Guid("ba855ba6-35ae-4775-b979-b76ac70a54e0");
    this.toolbarFilter.Hidden = false;
    this.toolbarFilter.ImageList = this.ilState;
    this.toolbarFilter.Items.AddRange(new ToolbarItemBase[4]
    {
      (ToolbarItemBase) this.labelFilter,
      (ToolbarItemBase) this.btErrors,
      (ToolbarItemBase) this.btWarnings,
      (ToolbarItemBase) this.btInformation
    });
    this.toolbarFilter.MinimumFloatingSize = new Size(250, 30);
    this.toolbarFilter.Name = "toolbarFilter";
    this.toolbarFilter.Overflow = ToolBarOverflow.Wrap;
    this.toolbarFilter.Stretch = true;
    this.toolbarFilter.Tearable = false;
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
    this.ilState.Images.SetKeyName(8, "make_main.ico");
    this.ilState.Images.SetKeyName(9, "Маршрут обработки.ico");
    this.ilState.Images.SetKeyName(10, "Уровни продвижения.ico");
    this.ilState.Images.SetKeyName(11, "information.png");
    this.ilState.Images.SetKeyName(12, "delete.png");
    this.ilState.Images.SetKeyName(13, "asterisk.ico");
    this.ilState.Images.SetKeyName(14, "ball.ico");
    componentResourceManager.ApplyResources((object) this.labelFilter, "labelFilter");
    this.btErrors.AutoToggle = AutoToggleType.Single;
    this.btErrors.BeginGroup = true;
    this.btErrors.Checked = true;
    componentResourceManager.ApplyResources((object) this.btErrors, "btErrors");
    this.btErrors.Font = new Font("Tahoma", 8.25f);
    this.btErrors.ImageIndex = 12;
    this.btErrors.ShowText = true;
    this.btErrors.Click += new EventHandler(this.DoCorrectVisibility);
    this.btWarnings.AutoToggle = AutoToggleType.Single;
    this.btWarnings.BeginGroup = true;
    this.btWarnings.Checked = true;
    componentResourceManager.ApplyResources((object) this.btWarnings, "btWarnings");
    this.btWarnings.ImageIndex = 13;
    this.btWarnings.ShowText = true;
    this.btWarnings.Click += new EventHandler(this.DoCorrectVisibility);
    this.btInformation.AutoToggle = AutoToggleType.Single;
    this.btInformation.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.btInformation, "btInformation");
    this.btInformation.ImageIndex = 11;
    this.btInformation.ShowText = true;
    this.btInformation.Click += new EventHandler(this.DoCorrectVisibility);
    this.panelStatus.Controls.Add((Control) this.progressBar);
    this.panelStatus.Controls.Add((Control) this.labelQuantity);
    this.panelStatus.Controls.Add((Control) this.labelHint);
    componentResourceManager.ApplyResources((object) this.panelStatus, "panelStatus");
    this.panelStatus.Name = "panelStatus";
    componentResourceManager.ApplyResources((object) this.progressBar, "progressBar");
    this.progressBar.Name = "progressBar";
    componentResourceManager.ApplyResources((object) this.labelQuantity, "labelQuantity");
    this.labelQuantity.Name = "labelQuantity";
    componentResourceManager.ApplyResources((object) this.labelHint, "labelHint");
    this.labelHint.Name = "labelHint";
    this.toolBarTrace.AddRemoveButtonsVisible = false;
    this.toolBarTrace.AllowHorizontalDock = false;
    this.toolBarTrace.DockLine = 3;
    this.toolBarTrace.DrawActionsButton = false;
    this.toolBarTrace.FullMenus = true;
    this.toolBarTrace.Guid = new Guid("ba855ba6-35ae-4775-b979-b76ac70a54e0");
    this.toolBarTrace.Hidden = false;
    this.toolBarTrace.ImageList = this.ilTracing;
    this.toolBarTrace.Items.AddRange(new ToolbarItemBase[5]
    {
      (ToolbarItemBase) this._startTracingButton,
      (ToolbarItemBase) this._stopTracingButton,
      (ToolbarItemBase) this._expandAllButton,
      (ToolbarItemBase) this._collapseAllButton,
      (ToolbarItemBase) this._clearButton
    });
    componentResourceManager.ApplyResources((object) this.toolBarTrace, "toolBarTrace");
    this.toolBarTrace.MinimumFloatingSize = new Size(250, 30);
    this.toolBarTrace.Name = "toolBarTrace";
    this.toolBarTrace.Overflow = ToolBarOverflow.Wrap;
    this.toolBarTrace.Stretch = true;
    this.toolBarTrace.Tearable = false;
    this._startTracingButton.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this._startTracingButton, "_startTracingButton");
    this._startTracingButton.ImageIndex = 1;
    this._startTracingButton.ShowText = true;
    this._startTracingButton.Click += new EventHandler(this.StartTracingButton_Click);
    componentResourceManager.ApplyResources((object) this._stopTracingButton, "_stopTracingButton");
    this._stopTracingButton.ImageIndex = 0;
    this._stopTracingButton.ShowText = true;
    this._stopTracingButton.Click += new EventHandler(this.StopTracingButton_Click);
    this._expandAllButton.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this._expandAllButton, "_expandAllButton");
    this._expandAllButton.ImageIndex = 2;
    this._expandAllButton.ShowText = true;
    this._expandAllButton.Click += new EventHandler(this.ExpandAllButton_Click);
    componentResourceManager.ApplyResources((object) this._collapseAllButton, "_collapseAllButton");
    this._collapseAllButton.ImageIndex = 3;
    this._collapseAllButton.ShowText = true;
    this._collapseAllButton.Click += new EventHandler(this.CollapseAllButton_Click);
    this._clearButton.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this._clearButton, "_clearButton");
    this._clearButton.ImageIndex = 4;
    this._clearButton.ShowText = true;
    this._clearButton.Click += new EventHandler(this.ClearButton_Click);
    this.panelLegend.BorderStyle = BorderStyle.Fixed3D;
    this.panelLegend.Controls.Add((Control) this.lbLegendWarningOK);
    this.panelLegend.Controls.Add((Control) this.lbLegendWarning);
    this.panelLegend.Controls.Add((Control) this.lbLegendError);
    this.panelLegend.Controls.Add((Control) this.btnHideLegend);
    componentResourceManager.ApplyResources((object) this.panelLegend, "panelLegend");
    this.panelLegend.Name = "panelLegend";
    componentResourceManager.ApplyResources((object) this.lbLegendWarningOK, "lbLegendWarningOK");
    this.lbLegendWarningOK.ImageList = this.ilState;
    this.lbLegendWarningOK.Name = "lbLegendWarningOK";
    componentResourceManager.ApplyResources((object) this.lbLegendWarning, "lbLegendWarning");
    this.lbLegendWarning.ImageList = this.ilState;
    this.lbLegendWarning.Name = "lbLegendWarning";
    componentResourceManager.ApplyResources((object) this.lbLegendError, "lbLegendError");
    this.lbLegendError.ImageList = this.ilState;
    this.lbLegendError.Name = "lbLegendError";
    componentResourceManager.ApplyResources((object) this.btnHideLegend, "btnHideLegend");
    this.btnHideLegend.Name = "btnHideLegend";
    this.btnHideLegend.Tag = (object) "0";
    this.btnHideLegend.UseVisualStyleBackColor = true;
    this.btnHideLegend.Click += new EventHandler(this.HideLegendButton_Click);
    this.AutoScaleMode = AutoScaleMode.Inherit;
    this.Controls.Add((Control) this.panelTrace);
    this.Controls.Add((Control) this.panelLegend);
    this.DoubleBuffered = true;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.Name = nameof (CompositionTracing);
    this.panelTrace.ResumeLayout(false);
    ((ISupportInitialize) this._grid).EndInit();
    this.panelStatus.ResumeLayout(false);
    this.panelLegend.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
