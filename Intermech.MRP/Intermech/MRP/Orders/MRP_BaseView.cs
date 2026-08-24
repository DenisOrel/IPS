// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.MRP_BaseView
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.Bars;
using Intermech.Client.Core;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.PdmConfigurator;
using Intermech.Localization;
using Intermech.Navigator;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MRP.Orders;

/// <summary>
/// Базовая закладка для редакторов в мастере по созданию производственных заказов
/// </summary>
/// <summary>Базовая закладка</summary>
internal class MRP_BaseView : UserControl, IView
{
  /// <summary>Объект для синхронизации</summary>
  protected object syncRoot = new object();
  /// <summary>Есть ли изменения в редакторе</summary>
  protected bool isChanged;
  /// <summary>Запрет на обработку некоторых событий</summary>
  protected bool inEvents;
  /// <summary>Сервис именованных значков</summary>
  protected INamedImageList _images;
  /// <summary>Текущий пользователь</summary>
  protected ICurrentUserAndRole _userAndRole;
  /// <summary>Сервис значков для категорий и типов</summary>
  protected ICategoryTypeIconService _categoryImages;
  /// <summary>Кэш графических элементов Навигатора</summary>
  protected INavGraphicsCache _navGraphicsCache;
  /// <summary>Индекс изображения закладка</summary>
  protected int _imgView = -1;
  /// <summary>Обработчик событий от службы уведомлений</summary>
  protected NotificationEventHandler _notifyHandler;
  /// <summary>
  /// Коллекция выделенных элементов пространства навигации, на основании данных которых работает закладка
  /// </summary>
  protected ISelectedItems _items;
  /// <summary>Служба уведомлений</summary>
  protected INotificationService _notifications;
  /// <summary>
  /// Контейнер сервисов (контекст) для выделенных элементов пространства навигации
  /// </summary>
  protected IServiceProvider _services;
  /// <summary>Required designer variable.</summary>
  private IContainer components;

  /// <summary>Контейнер сервисов</summary>
  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public virtual IServiceProvider Services
  {
    [DebuggerStepThrough] get => this._services;
    set => this._services = value;
  }

  /// <summary>Объект для синхронизации</summary>
  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public virtual object SyncRoot
  {
    [DebuggerStepThrough] get => this.syncRoot;
  }

  /// <summary>Есть ли изменения в редакторе настройке</summary>
  [Category("Appearance")]
  [Browsable(true)]
  public virtual bool IsChanged
  {
    [DebuggerStepThrough] get => this.isChanged;
  }

  /// <summary>
  /// Событие возникает, если в редакторе произошли изменения
  /// </summary>
  [CustomDescription("Attribute.MRP_1")]
  public virtual event EventHandler OnChanged;

  /// <summary>Сгенерировать событие "OnChanged"</summary>
  public virtual void RaiseOnChanged()
  {
    if (!this.inEvents && this.OnChanged != null)
      this.OnChanged((object) this, EventArgs.Empty);
    if (!(this.Services.GetService(typeof (ManufactOrdersEditor)) is ManufactOrdersEditor service))
      return;
    service.RaiseOnChanged();
  }

  /// <summary>Создать экземпляр класса</summary>
  public MRP_BaseView()
  {
    this.InitializeComponent();
    this.InitViewResources();
    if (!(ServicesManager.GetService(typeof (BarManager)) is BarManager service))
      return;
    service.RendererChanged += new EventHandler(this.ToolbarRendererChanged);
  }

  /// <summary>Инициализация ресурсов закладки</summary>
  public virtual void InitViewResources()
  {
    if (!(ServicesManager.GetService(typeof (IGuidMapper)) is IGuidMapper) || this.DesignMode)
      return;
    this._images = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
    this._userAndRole = ServicesManager.GetService(typeof (ICurrentUserAndRole)) as ICurrentUserAndRole;
    this._navGraphicsCache = ServicesManager.GetService(typeof (INavGraphicsCache)) as INavGraphicsCache;
    this._categoryImages = ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
    this._notifications = ServicesManager.GetService(typeof (INotificationService)) as INotificationService;
    this._imgView = -1;
  }

  /// <summary>Освобождение ресурсов закладки</summary>
  public virtual void DisposeViewResources()
  {
    this._images = (INamedImageList) null;
    this._categoryImages = (ICategoryTypeIconService) null;
    this._items = (ISelectedItems) null;
    this._services = (IServiceProvider) null;
    this._notifications = (INotificationService) null;
  }

  /// <summary>
  /// Пришло событие "Изменился рендерер панелей инструментов"
  /// </summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  protected virtual void ToolbarRendererChanged(object sender, EventArgs e)
  {
  }

  /// <summary>Заголовок закладки</summary>
  public virtual string Caption
  {
    [DebuggerStepThrough] get => string.Empty;
  }

  /// <summary>Индекс изображения</summary>
  public virtual int ImageIndex => this._imgView;

  /// <summary>Порядковый номер закладки</summary>
  public virtual int OrderID => 0;

  /// <summary>Инициализировать закладку</summary>
  /// <param name="items">Коллекция выделенных элементов пространства навигации</param>
  /// <param name="provider">Контейнер сервисов</param>
  public virtual void Initialize(ISelectedItems items, IServiceProvider provider)
  {
    this._items = items;
    this._services = provider;
  }

  /// <summary>
  /// Активировать закладку (чтение из базы данных, загрузка информации и т.п.)
  /// </summary>
  /// <param name="previousView">Предыдущая закладка</param>
  public virtual void Activate(IView previousView)
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
  public virtual void Deactivate(IView nextView)
  {
  }

  /// <summary>
  /// Заполнить элементы управления закладки данными, полученными в методе Initialize
  /// </summary>
  protected virtual void LoadViewData()
  {
    this.Clear();
    if (this._items == null || this._items.Count == 0)
      return;
    this._items.GetItemData(0, typeof (IDBTypedObjectID));
    this.UpdateControls();
  }

  /// <summary>Выполнить очистку элементов управления в закладке</summary>
  protected virtual void Clear() => this.UpdateControls();

  /// <summary>Управление контролами на закладке</summary>
  protected virtual void UpdateControls()
  {
  }

  /// <summary>
  /// Создать и заполнить контекст для указанного дочернего объекта
  /// </summary>
  /// <param name="session">Сессия</param>
  /// <param name="rootObj">Корневой объект состава</param>
  /// <param name="relation">Связь</param>
  /// <param name="obj">Объект</param>
  /// <param name="partObject">Дочерний объект</param>
  /// <returns>Контекст конфигуратора составов для указанного дочернего объекта</returns>
  public virtual PdmConfiguratorContext CreateConfiguratorContext(
    IUserSession session,
    IDBTypedObjectID rootObj,
    IDBRelation relation,
    IDBObject obj,
    IDBTypedObjectID partObject)
  {
    PdmConfiguratorContext configuratorContext = new PdmConfiguratorContext((object) relation);
    RelationPair key = PdmConfiguratorHelper.CreateKey(rootObj.ObjectID, rootObj.ObjectType, relation != null ? relation.RelationID : 0L, relation != null ? relation.RelationType : -1, obj != null ? obj.ObjectID : partObject.ObjectID, obj != null ? obj.ObjectType : partObject.ObjectType);
    try
    {
      configuratorContext.Services.AddService(typeof (IUserSession), (object) session);
      configuratorContext.Services.AddService(typeof (object), relation != null ? (object) relation : (object) obj);
      configuratorContext.Key = key;
      configuratorContext.ObjectsOptions.Clear();
      ObjectOptionsHolder objectOptionsHolder = new ObjectOptionsHolder(obj != null ? (object) obj : (object) session.GetObject(partObject.ObjectID, false));
      objectOptionsHolder.LoadOptionsToCache(session);
      configuratorContext.ObjectsOptions.Add(objectOptionsHolder);
      configuratorContext.SyncOptionsList(true);
      configuratorContext.Key = key;
    }
    finally
    {
      configuratorContext.Services.RemoveService(typeof (object));
      configuratorContext.Services.RemoveService(typeof (IUserSession));
    }
    return configuratorContext;
  }

  /// <summary>
  /// Получить контекст конфигуратора составов для первого элемента из указанной коллекции
  /// </summary>
  /// <param name="items">Коллекция выделенных элементов пространства Навигатора</param>
  /// <param name="services">Контейнер сервисов</param>
  /// <returns>Контекст или null</returns>
  public virtual PdmConfiguratorContext GetConfiguratorContext(
    ISelectedItems items,
    IServiceProvider services)
  {
    if (items == null || items.Count == 0 || services == null)
      return (PdmConfiguratorContext) null;
    IDBTypedObjectID rootObj = services.GetService(typeof (IDBTypedObjectID)) as IDBTypedObjectID;
    IDBTypedObjectID itemData1 = items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
    IDBRelationID itemData2 = items.GetItemData(0, typeof (IDBRelationID)) as IDBRelationID;
    if (rootObj == null)
      rootObj = itemData1;
    if (rootObj == null || itemData1 == null)
      return (PdmConfiguratorContext) null;
    if (rootObj.ObjectID == itemData1.ObjectID && (itemData2 == null || itemData2.Value == 0L) && MetaDataHelper.IsPdmRootObjectType(itemData1.ObjectType))
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(itemData1.ObjectID, false);
        return dbObject != null ? this.CreateConfiguratorContext(sessionKeeper.Session, rootObj, (IDBRelation) null, dbObject, itemData1) : (PdmConfiguratorContext) null;
      }
    }
    if (itemData2 == null || itemData2.Value == 0L)
      return (PdmConfiguratorContext) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBRelation relation = sessionKeeper.Session.GetRelation(itemData2.Value, false);
      return relation != null ? this.CreateConfiguratorContext(sessionKeeper.Session, rootObj, relation, (IDBObject) null, itemData1) : (PdmConfiguratorContext) null;
    }
  }

  /// <summary>Забрать изменения из закладки в контейнер настроек</summary>
  protected virtual void CaptureChanges()
  {
  }

  /// <summary>Удаление используемых ресурсов</summary>
  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
  protected override void Dispose(bool disposing)
  {
    if (disposing && ServicesManager.GetService(typeof (BarManager)) is BarManager service)
      service.RendererChanged -= new EventHandler(this.ToolbarRendererChanged);
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MRP_BaseView));
    this.SuspendLayout();
    this.AutoScaleMode = AutoScaleMode.Inherit;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.Name = nameof (MRP_BaseView);
    this.ResumeLayout(false);
  }
}
