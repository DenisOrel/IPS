// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.ManufactOrdersAPI
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using ImSSP;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.MRP;
using Intermech.Localization;
using Intermech.Navigator;
using Intermech.Navigator.Controls;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Search.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MRP.Orders;

/// <summary>
/// Вспомогательный статический класс, содержащий функционал
/// для работы с производственными заказами из Навигатора
/// </summary>
internal static class ManufactOrdersAPI
{
  /// <summary>
  /// Идентификаторы версий объектов, которые следует включить в состав производственного заказа
  /// (применяется в методе CreateManufactOrder)
  /// </summary>
  internal static List<IDBTypedObjectID> objectIDs;
  /// <summary>
  /// Настройки, помогающие сохранить информацию об исходном экземпляре/партии, новом изделии, пути в составе
  /// </summary>
  internal static MovingItemSettings settings;

  /// <summary>
  /// Обработчик события, возникающего при успешном завершении создания нового объекта
  /// </summary>
  /// <param name="sender">Ссылка на экземпляр создателя объекта</param>
  /// <param name="ea">Аргументы события</param>
  internal static void OnObjectCreatorCompleatedEventHandler(
    object sender,
    AfterObjectCreatedEventArgs ea)
  {
    if (!(ServicesManager.GetService(typeof (ManufactureOrderHolder)) is ManufactureOrderHolder service) || Math.Abs(service.ObjectID) != Math.Abs(ea.ObjectID))
      return;
    ServicesManager.RemoveService(typeof (ManufactureOrderHolder));
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (ea.ObjectID > (long) sc_14825.ssp_mrp_14826(718995887))
      {
        if (service.ObjectID < 0L)
        {
          IDBObject dbObject = sessionKeeper.Session.GetObjectActualCopy(ea.ObjectID, true);
          if (dbObject.CheckoutBy == 0L && dbObject.ObjectModifyMode == ObjectModifyModes.Checkout)
            dbObject = dbObject.CheckOut();
          service.ObjectID = dbObject.ObjectID;
          service.ObjectType = dbObject.ObjectType;
          service.ReloadOrderNumber(dbObject);
        }
      }
    }
    ManufactOrdersAPI.ExecuteManufactOrderJob(service, (IServiceProvider) null);
  }

  /// <summary>
  /// Создать и вернуть контейнер настроек производственного заказа для указанного состава
  /// </summary>
  /// <param name="items">Коллекция выделенных элементов, входящих в состав производственного заказа</param>
  /// <param name="viewServices">Контейнер сервисов</param>
  /// <param name="additionalInfo">Дополнительная информация</param>
  /// <returns></returns>
  internal static ManufactureOrderHolder GetDefaultHolder(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    ManufactureOrderHolder defaultHolder = new ManufactureOrderHolder();
    if (items == null || items.Count == 0)
      return defaultHolder;
    RelationPath orderItemPath = ManufactOrdersAPI.FindOrderItemPath(items, viewServices, (object) null);
    if (orderItemPath == null || orderItemPath.Items == null || orderItemPath.Items.Count == 0)
      return defaultHolder;
    IFiltrationService service = ServicesManager.GetService(typeof (IFiltrationService)) as IFiltrationService;
    defaultHolder.ObjectID = orderItemPath.Items[0].F_PART_ID;
    defaultHolder.ObjectType = orderItemPath.Items[0].F_OBJECT_TYPE;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      defaultHolder.ReloadOrderNumber(sessionKeeper.Session);
      if (service != null)
      {
        if (!string.IsNullOrEmpty(service.FiltrationServiceOwnerID))
        {
          if (sessionKeeper.Session.GetCustomService(typeof (IVersionRulesCacheService)) is IVersionRulesCacheService customService)
            defaultHolder.FiltrationSettings = customService.GetFiltrationSettings((object) sessionKeeper.Session.SessionGUID, service.FiltrationServiceOwnerID, true);
        }
      }
    }
    return defaultHolder;
  }

  /// <summary>
  /// Отобразить окно с состоянием указанной задачи, следить за ходом её выполнения
  /// </summary>
  /// <param name="viewServices">Контейнер сервисов</param>
  /// <param name="jobID">Уникальный идентификатор задачи</param>
  /// <param name="holder">Контейнер настроек</param>
  internal static void ShowJobStatus(
    IServiceProvider viewServices,
    Guid jobID,
    ManufactureOrderHolder holder)
  {
    if (jobID == Guid.Empty)
      return;
    MRPTasksQueueState mrpTasksQueueState = ManufactOrdersTaskForm.Execute(jobID);
    if (mrpTasksQueueState == null)
      return;
    if (mrpTasksQueueState.Exception != null)
      ExceptionHelper.ExceptionService.ShowException(mrpTasksQueueState.Exception);
    else if (mrpTasksQueueState.CancelledTasks > sc_14825.ssp_mrp_14827(1289298065))
    {
      int num1 = (int) MessageBox.Show(LocalizationHolder.rm.GetString("MRP_51"), LocalizationHolder.rm.GetString("MRP_52"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }
    else
    {
      int num2 = (int) MessageBox.Show(LocalizationHolder.rm.GetString(sc_14825.ssp_mrp_14828()), LocalizationHolder.rm.GetString("MRP_45"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }
    if (mrpTasksQueueState.NavigatorEvents == null || mrpTasksQueueState.Exception != null)
      return;
    INotificationService notificationService = (viewServices != null ? viewServices.GetService(typeof (INotificationService)) as INotificationService : (INotificationService) null) ?? ServicesManager.GetService(typeof (INotificationService)) as INotificationService;
    if (mrpTasksQueueState.NavigatorEvents.RelDeletedIDs.Count > 0)
    {
      DBRelationsEventArgs e = new DBRelationsEventArgs("RelationsRemoved", (IList<long>) mrpTasksQueueState.NavigatorEvents.RelDeletedIDs, (IList<long>) null, (IList<int>) null, (IList<int>) mrpTasksQueueState.NavigatorEvents.RelDeletedTypeIDs);
      notificationService.FireEvent((object) null, (NotificationEventArgs) e);
    }
    if (mrpTasksQueueState.NavigatorEvents.ObjDeletedIDs.Count > 0)
    {
      DBObjectsEventArgs e = new DBObjectsEventArgs("ObjectsRemoved", (IList<long>) mrpTasksQueueState.NavigatorEvents.ObjDeletedIDs);
      notificationService.FireEvent((object) null, (NotificationEventArgs) e);
    }
    if (mrpTasksQueueState.NavigatorEvents.ObjCreatedIDs.Count > 0)
    {
      DBObjectsEventArgs e = new DBObjectsEventArgs("ObjectsCreated", (IList<long>) mrpTasksQueueState.NavigatorEvents.ObjCreatedIDs, (IList<int>) mrpTasksQueueState.NavigatorEvents.ObjCreatedTypeIDs);
      notificationService.FireEvent((object) null, (NotificationEventArgs) e);
    }
    if (mrpTasksQueueState.NavigatorEvents.RelCreatedIDs.Count <= 0)
      return;
    DBRelationsEventArgs e1 = new DBRelationsEventArgs("RelationsCreated", (IList<long>) mrpTasksQueueState.NavigatorEvents.RelCreatedIDs, (IList<long>) mrpTasksQueueState.NavigatorEvents.RelCreatedProjIDs, (IList<int>) mrpTasksQueueState.NavigatorEvents.RelCreatedProjTypeIDs, (IList<int>) mrpTasksQueueState.NavigatorEvents.RelCreatedTypeIDs, NavigatorRelationCommand.Unknown);
    notificationService.FireEvent((object) null, (NotificationEventArgs) e1);
  }

  /// <summary>
  /// Запустить задание по обработке производственного заказа
  /// </summary>
  /// <param name="holder">Контейнер настроек</param>
  /// <param name="viewServices">Контейнер сервисов</param>
  internal static void ExecuteManufactOrderJob(
    ManufactureOrderHolder holder,
    IServiceProvider viewServices)
  {
    if (holder == null || holder.ObjectID == 0L)
      return;
    Guid jobID = Guid.Empty;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObjectActualCopy(holder.ObjectID, true);
      if (dbObject.CheckoutBy == 0L && dbObject.ObjectModifyMode == ObjectModifyModes.Checkout)
        dbObject = dbObject.CheckOut();
      holder.ObjectID = dbObject.ObjectID;
      holder.ObjectType = dbObject.ObjectType;
      holder.ReloadOrderNumber(dbObject);
      jobID = (sessionKeeper.Session.GetCustomService(typeof (IMRPCompositionsBrowser)) as IMRPCompositionsBrowser).StartActionsCreateJob(sessionKeeper.Session.SessionGUID, holder, sessionKeeper.Session.MaxTaskThreadsCount, true);
    }
    ManufactOrdersAPI.ShowJobStatus(viewServices, jobID, holder);
  }

  /// <summary>Создать производственный заказ</summary>
  /// <param name="items">Коллекция выделенных элементов</param>
  /// <param name="viewServices">Контейнер сервисов</param>
  /// <param name="additionalInfo">Дополнительная информация</param>
  internal static void CreateManufactOrder(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    IObjectCreatorService service1 = ServicesManager.GetService(typeof (IObjectCreatorService)) as IObjectCreatorService;
    service1.AfterDraftCreatedEvent += new AfterDraftCreatedEventHandler(ManufactOrdersAPI.cDlg_ObjectCreatorDraftCreatedEvent);
    try
    {
      long objectByTypeDialog = service1.CreateObjectByTypeDialog(MetaDataHelper.GetObjectTypeID("cadd92e9-306c-11d8-b4e9-00304f19f545"));
      if (objectByTypeDialog == 0L || objectByTypeDialog == (long) -sc_14825.ssp_mrp_14829(1158338106))
        return;
      DBObjectsEventArgs e = new DBObjectsEventArgs("ObjectsCreated", objectByTypeDialog);
      if (!(viewServices.GetService(typeof (INotificationService)) is INotificationService service2))
        return;
      service2.FireEvent((object) null, (NotificationEventArgs) e);
    }
    finally
    {
      service1.AfterDraftCreatedEvent -= new AfterDraftCreatedEventHandler(ManufactOrdersAPI.cDlg_ObjectCreatorDraftCreatedEvent);
      ManufactOrdersAPI.objectIDs = (List<IDBTypedObjectID>) null;
    }
  }

  /// <summary>Создана заготовка производственного заказа</summary>
  /// <param name="objectID">Идентификатор версии созданной заготовки</param>
  private static void cDlg_ObjectCreatorDraftCreatedEvent(
    object sender,
    AfterDraftCreatedEventArgs e)
  {
    if (ManufactOrdersAPI.objectIDs == null || ManufactOrdersAPI.objectIDs.Count == 0)
      return;
    MeasuredValue newValue = (MeasuredValue) null;
    try
    {
      MeasureDescriptor measureDescriptor = (MeasureDescriptor) null;
      foreach (MeasureDescriptor measure in MeasureHelper.Measures)
      {
        if (measure.PhysicalQuantityGuid == SystemGUIDs.objectQuantityGuid)
        {
          measureDescriptor = measure;
          break;
        }
      }
      newValue = MeasureHelper.ConvertToMeasuredValue($"1 {measureDescriptor.ShortName}");
    }
    catch
    {
    }
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (sessionKeeper.Session.GetObject(e.ObjectID, false) == null)
        return;
      IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(MetaDataHelper.GetRelationTypeID("cad00023-306c-11d8-b4e9-00304f19f545"));
      for (int index = 0; index < ManufactOrdersAPI.objectIDs.Count; ++index)
      {
        IDBRelation dbRelation = relationCollection.Create(e.ObjectID, ManufactOrdersAPI.objectIDs[index].ObjectID);
        if (newValue != null && newValue.Value == 1.0)
          dbRelation.TryToAddOrDelAttribute(MetaDataHelper.GetAttributeTypeID("cad00267-306c-11d8-b4e9-00304f19f545"), (object) newValue);
      }
    }
  }

  /// <summary>Обработать состав производственного заказа</summary>
  /// <param name="items">Коллекция выделенных элементов</param>
  /// <param name="viewServices">Контейнер сервисов</param>
  /// <param name="additionalInfo">Дополнительная информация</param>
  internal static void ConvertManufactOrder(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (ManufactOrdersEditorForm.Execute(viewServices, items) != DialogResult.OK || !(ServicesManager.GetService(typeof (ManufactureOrderHolder)) is ManufactureOrderHolder service) || service.ObjectID == 0L)
      return;
    ServicesManager.RemoveService(typeof (ManufactureOrderHolder));
    ManufactOrdersAPI.ExecuteManufactOrderJob(service, viewServices);
  }

  /// <summary>
  /// Метод позволяет получить полный путь от указанного выделенного элемента к корневому конфигурируемуму
  /// объекту - производственному заказу, если эта информация доступна в сервисах и исходных данных, в противном
  /// случае будет возвращено значение null
  /// </summary>
  /// <param name="items">Выделенные элементы (изучается только нулевой элемент из коллекции)</param>
  /// <param name="viewServices">Контейнер сервисов</param>
  /// <param name="additionalInfo">Дополнительная информация</param>
  /// <returns>Полный путь от указанного выделенного элемента к корневому конфигурируемуму
  /// объекту - производственному заказу, либо null</returns>
  internal static RelationPath FindOrderItemPath(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items == null || items.Count == 0 || viewServices == null)
      return (RelationPath) null;
    ChildrenView service1 = viewServices.GetService(typeof (ChildrenView)) as ChildrenView;
    NavigatorTreeView service2 = viewServices.GetService(typeof (NavigatorTreeView)) as NavigatorTreeView;
    RelationPath orderItemPath = service1 != null ? service1.GetTypedParentObjectNodePath(MetaDataHelper.GetObjectTypeID("cadd92e9-306c-11d8-b4e9-00304f19f545"), true) : (service2 != null ? NavigatorTreeViewHelper.GetTypedParentObjectNodePath(service2.FocusedNode, MetaDataHelper.GetObjectTypeID("cadd92e9-306c-11d8-b4e9-00304f19f545"), true) : (RelationPath) null);
    if (orderItemPath != null && orderItemPath.Items.Count > 0 && !MetaDataHelper.IsObjectTypeChildOf(orderItemPath.Items[0].F_OBJECT_TYPE, MetaDataHelper.GetObjectTypeID("cadd92e9-306c-11d8-b4e9-00304f19f545")))
      orderItemPath = (RelationPath) null;
    return orderItemPath;
  }

  /// <summary>Получено очередное событие от службы уведомлений</summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  internal static void NotificationEventFired(object sender, NotificationEventArgs e)
  {
    ManufactOrdersEditorForm service1 = ServicesManager.GetService(typeof (ManufactOrdersEditorForm)) as ManufactOrdersEditorForm;
    ManufactOrdersEditor service2 = ServicesManager.GetService(typeof (ManufactOrdersEditor)) as ManufactOrdersEditor;
    if (service1 != null || service2 != null || !(e is DBRelationsEventArgs relationsEventArgs) || relationsEventArgs.RelationCommand == NavigatorRelationCommand.Unknown || relationsEventArgs.RelationIDs == null || relationsEventArgs.RelationIDs.Count == 0 || relationsEventArgs.ProjIDs == null || relationsEventArgs.ProjIDs.Count != 1)
      return;
    int projTypeId = relationsEventArgs.GetProjTypeID(relationsEventArgs.ProjIDs[0]);
    if (projTypeId == -1 || !MetaDataHelper.IsObjectTypeChildOf(projTypeId, MetaDataHelper.GetObjectTypeID("cadd92e9-306c-11d8-b4e9-00304f19f545")))
      return;
    bool flag = false;
    int relationTypeId = MetaDataHelper.GetRelationTypeID("cad00023-306c-11d8-b4e9-00304f19f545");
    for (int index = 0; index < relationsEventArgs.RelationIDs.Count; ++index)
    {
      flag = relationsEventArgs.GetRelationType(relationsEventArgs.RelationIDs[index]) == relationTypeId;
      if (flag)
        break;
    }
    if (!flag)
      return;
    ManufactOrdersAPI.ConvertManufactOrder(Intermech.Navigator.ContextMenu.Services.GetItems(relationsEventArgs.ProjIDs[0]), (IServiceProvider) ServicesManager.ServiceContainer, (object) null);
  }

  /// <summary>Команда "Заменить версию изделия"</summary>
  /// <param name="items">Коллекция выделенных элементов</param>
  /// <param name="viewServices">Контейнер сервисов</param>
  /// <param name="additionalInfo">Дополнительная информация</param>
  internal static void MRPChangeInstanceVersion(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    long newVersion = ManufactOrdersAPI.MRPSelectObject(items, viewServices, true, (IList<int>) null);
    ManufactOrdersAPI.MRPInternalChangeVersion(items, viewServices, additionalInfo, newVersion);
  }

  /// <summary>Команда "Заменить изделие"</summary>
  /// <param name="items">Коллекция выделенных элементов</param>
  /// <param name="viewServices">Контейнер сервисов</param>
  /// <param name="additionalInfo">Дополнительная информация</param>
  internal static void MRPChangeInstance(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    long newVersion = ManufactOrdersAPI.MRPSelectObject(items, viewServices, false, (IList<int>) new List<int>((IEnumerable<int>) new int[2]
    {
      MetaDataHelper.GetObjectTypeID("cad00268-306c-11d8-b4e9-00304f19f545"),
      MetaDataHelper.GetObjectTypeID("cad015b1-306c-11d8-b4e9-00304f19f545")
    }));
    ManufactOrdersAPI.MRPInternalChangeVersion(items, viewServices, additionalInfo, newVersion);
  }

  /// <summary>Команда "Заменить маршрут обработки"</summary>
  /// <param name="items">Коллекция выделенных элементов</param>
  /// <param name="viewServices">Контейнер сервисов</param>
  /// <param name="additionalInfo">Дополнительная информация</param>
  internal static void MRPChangeTechRoute(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (!(items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData) || itemData.ID == 0L || itemData.ObjectID == 0L || itemData.ObjectType == -1)
      return;
    RelationPath orderItemPath = ManufactOrdersAPI.FindOrderItemPath(items, viewServices, (object) null);
    if (orderItemPath == null || orderItemPath.Items == null || orderItemPath.Items.Count == 0)
      return;
    ManufactureOrderHolder defaultHolder = ManufactOrdersAPI.GetDefaultHolder(items, viewServices, additionalInfo);
    RelationPair rootObject = (RelationPair) null;
    MRPTypedObjectRef mrpTypedObjectRef = new MRPTypedObjectRef(viewServices, itemData.ObjectID, Guid.Empty, itemData.ObjectType);
    MRPFindArticleTechRoutesAction action1 = new MRPFindArticleTechRoutesAction(viewServices, (IMRPObjectRef) mrpTypedObjectRef);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      MRPActionsExecutor.Execute(sessionKeeper.Session, (IMRPAction) action1);
      rootObject = new RelationPair(sessionKeeper.Session.ClientConnectionID, defaultHolder.ObjectID, defaultHolder.ObjectType, 0L, sessionKeeper.Session.UserID, defaultHolder.ObjectID, -1, defaultHolder.ObjectType);
    }
    if (action1.TechRoutes.Count == 0)
    {
      int num = (int) MessageBox.Show(LocalizationHolder.rm.GetString("MRP_64"), LocalizationHolder.rm.GetString("MRP_45"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }
    else
    {
      Intermech.Navigator.SelectionWindow.RegisterAnalyze((ISelectedItemsAnalyzer) new TypedObjectsSelectedItemsAnalyzer(MetaDataHelper.GetObjectTypeID("cad0016f-306c-11d8-b4e9-00304f19f545"), true), true);
      if (!(Intermech.Navigator.SelectionWindow.Select(LocalizationHolder.rm.GetString("MRP_62"), (IDescriptor) new ListDescriptor(Intermech.Navigator.Consts.CategoryVersionsObjectNode, MetaDataHelper.GetObjectTypeID("cad0016f-306c-11d8-b4e9-00304f19f545"), LocalizationHolder.rm.GetString("MRP_63"), (IList) action1.TechRoutes), typeof (IDBTypedObjectID), viewServices, SelectionOptions.Default | SelectionOptions.DisableMultiselect | SelectionOptions.ForceRebuildNavTree) is IDBTypedObjectID[] dbTypedObjectIdArray) || dbTypedObjectIdArray.Length == 0)
        return;
      MRPTypedObjectRef techObjRef = new MRPTypedObjectRef(viewServices, dbTypedObjectIdArray[0].ObjectID, Guid.Empty, dbTypedObjectIdArray[0].ObjectType);
      MRPAttachTechRouteAction action2 = new MRPAttachTechRouteAction(viewServices, (IMRPTypedObjectRef) mrpTypedObjectRef, (IMRPTypedObjectRef) techObjRef);
      Guid jobID = Guid.Empty;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        using (NotificationContext.Create(sessionKeeper.Session))
        {
          MRPActionsExecutor.Execute(sessionKeeper.Session, (IMRPAction) action2);
          jobID = (sessionKeeper.Session.GetCustomService(typeof (IMRPCompositionsBrowser)) as IMRPCompositionsBrowser).StartTechRouteChangeJob(sessionKeeper.Session.SessionGUID, rootObject, orderItemPath, mrpTypedObjectRef.ObjectID, defaultHolder, sessionKeeper.Session.MaxTaskThreadsCount, true);
        }
      }
      ManufactOrdersAPI.ShowJobStatus(viewServices, jobID, defaultHolder);
    }
  }

  /// <summary>Команда "Сделать покупным"</summary>
  /// <param name="items">Коллекция выделенных элементов</param>
  /// <param name="viewServices">Контейнер сервисов</param>
  /// <param name="additionalInfo">Дополнительная информация</param>
  internal static void MRPMakeBought(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
  }

  /// <summary>Команда "Заменить версию в заказе"</summary>
  /// <param name="items">Коллекция выделенных элементов</param>
  /// <param name="viewServices">Контейнер сервисов</param>
  /// <param name="additionalInfo">Дополнительная информация</param>
  internal static void MRPChangeVersion(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
  }

  /// <summary>
  /// Выбрать версию первого указанного объекта, либо выбрать объект из списка допустимых типов
  /// </summary>
  /// <param name="items">Коллекция выделенных элементов. Для выбора версии используется нулевой элемент. Коллекция используется для поиска пути в составе</param>
  /// <param name="viewServices">Контейнер сервисов</param>
  /// <param name="versionOnly">true - выбирается одна из версий нулевого объекта. Значение typesToSelect игнорируется</param>
  /// <param name="typesToSelect">При значении versionOnly = false в данном списке должны быть перечислены допустимые типы объектов для выбора</param>
  /// <returns>Идентификатор выбранной версии объекта, либо Intermech.Consts.UnknownObjectId, если выбор отменён</returns>
  internal static long MRPSelectObject(
    ISelectedItems items,
    IServiceProvider viewServices,
    bool versionOnly,
    IList<int> typesToSelect)
  {
    ManufactOrdersAPI.settings = new MovingItemSettings();
    long objectID = 0;
    if (items == null || items.Count == 0)
      return 0;
    IDBRelationID itemData = items.GetItemData(0, typeof (IDBRelationID)) as IDBRelationID;
    IDBTypedObjectID dbTypedObjectId = items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
    if (itemData == null || itemData.ProjID == 0L || itemData.RelationType == -1 || itemData.Value == 0L || dbTypedObjectId == null || dbTypedObjectId.ID == 0L || dbTypedObjectId.ObjectID == 0L || dbTypedObjectId.ObjectType == -1)
      return objectID;
    ManufactOrdersAPI.settings = new MovingItemSettings(itemData.ProjID, itemData.Value, itemData.RelationType, dbTypedObjectId.ObjectID, dbTypedObjectId.ObjectType, 0L, -1, 0L);
    try
    {
      if (versionOnly)
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          MRPFindArticle4InstanceAction action = new MRPFindArticle4InstanceAction(viewServices, (IMRPObjectRef) new MRPObjectRef(viewServices, dbTypedObjectId.ObjectID));
          MRPActionsExecutor.Execute(sessionKeeper.Session, (IMRPAction) action);
          if (action.ObjectID != 0L)
          {
            QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(action.ObjectID);
            dbTypedObjectId = (IDBTypedObjectID) new DBTypedObjectID(objectInfo.ObjectTypeID, action.ObjectID, objectInfo.ID, objectInfo.Caption, dbTypedObjectId.Owner, 0L, 0L, string.Empty, 0L);
          }
        }
        objectID = ObjectVersionSelection.SelectVersion(dbTypedObjectId.ID, true, (List<long>) null, dbTypedObjectId.ObjectID);
        return objectID;
      }
      IServiceContainer nodesContext = (IServiceContainer) new ServiceContainer();
      DescriptorCollection descriptors = new DescriptorCollection();
      IObjectTypeNodeFilter serviceInstance = (IObjectTypeNodeFilter) new ObjectTypeNodeFilter();
      nodesContext.AddService(typeof (IObjectTypeNodeFilter), (object) serviceInstance);
      for (int index = 0; index < typesToSelect.Count; ++index)
        descriptors.Add((IDescriptor) new Intermech.Navigator.DBObjectTypes.Descriptor(typesToSelect[index]));
      if (!(Intermech.Navigator.SelectionWindow.Select(LocalizationHolder.rm.GetString("MRP_60"), (IDescriptor) new Intermech.Navigator.CustomNode.Descriptor(Intermech.Navigator.Consts.CategoryCustomNode, 1, LocalizationHolder.rm.GetString("MRP_61"), descriptors), typeof (IDBTypedObjectID), (IServiceProvider) nodesContext, SelectionOptions.Default | SelectionOptions.DisableMultiselect | SelectionOptions.ForceFilterObjectsByRule) is IDBTypedObjectID[] dbTypedObjectIdArray) || dbTypedObjectIdArray.Length == 0)
        return 0;
      objectID = dbTypedObjectIdArray[0].ObjectID;
      return objectID;
    }
    finally
    {
      if (objectID != 0L)
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(objectID);
          ManufactOrdersAPI.settings.NewArticleID = objectInfo.ObjectID;
          ManufactOrdersAPI.settings.NewArticleTypeID = objectInfo.ObjectTypeID;
        }
      }
    }
  }

  /// <summary>
  /// Выполнить замену выделенного нулевого объекта в составе производственного заказа на другой объект
  /// типа "Экземпляры/Партии", созданного на основе указанной новой версии изделия/комплектации
  /// </summary>
  /// <param name="items">Коллекция выделенных элементов</param>
  /// <param name="viewServices">Контейнер сервисов</param>
  /// <param name="additionalInfo">Дополнительная информация</param>
  /// <param name="newVersion">Новая версия изделия/комплектации</param>
  internal static void MRPInternalChangeVersion(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo,
    long newVersion)
  {
    if (items == null || items.Count == 0 || newVersion == 0L || ManufactOrdersAPI.settings == null || ManufactOrdersAPI.settings.NewArticleID != newVersion)
      return;
    RelationPath orderItemPath = ManufactOrdersAPI.FindOrderItemPath(items, viewServices, (object) null);
    if (orderItemPath == null || orderItemPath.Items == null || orderItemPath.Items.Count == 0)
      return;
    AdvancedServiceContainer viewServices1 = new AdvancedServiceContainer();
    MRPOrderItemsSettingsHolder serviceInstance = new MRPOrderItemsSettingsHolder();
    viewServices1.AddService(typeof (MRPOrderItemsSettingsHolder), (object) serviceInstance);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      MRPTypedObjectRef projID = new MRPTypedObjectRef(viewServices, orderItemPath.Items[0].F_PART_ID, Guid.Empty, orderItemPath.Items[0].F_OBJECT_TYPE);
      QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(newVersion);
      MRPTypedObjectRef mrpTypedObjectRef = new MRPTypedObjectRef(viewServices, newVersion, objectInfo.VersionGuid, objectInfo.ObjectTypeID);
      MRPCreateRelationAction createRelationAction = new MRPCreateRelationAction(viewServices, (IMRPTypedObjectRef) projID, (IMRPTypedObjectRef) mrpTypedObjectRef, MetaDataHelper.GetRelationTypeID("cad00023-306c-11d8-b4e9-00304f19f545"));
      MRPRelationRef sourceRelRef = new MRPRelationRef(viewServices, ManufactOrdersAPI.settings.SourceProjID, ManufactOrdersAPI.settings.SourceLinkID, Guid.Empty, ManufactOrdersAPI.settings.SourceLinkTypeID, false);
      MRPActionsExecutor.Execute(sessionKeeper.Session, (IMRPAction) createRelationAction);
      MRPActionsExecutor.Execute(sessionKeeper.Session, (IMRPAction) new MRPSyncRelationsAttrsAction(viewServices, (IMRPRelationRef) sourceRelRef, (IMRPRelationRef) createRelationAction));
      MRPActionsExecutor.Execute(sessionKeeper.Session, (IMRPAction) new MRPFixRelationPartAction(viewServices, (IMRPRelationRef) createRelationAction, (IMRPObjectRef) mrpTypedObjectRef));
      ManufactOrdersAPI.settings.NewArticleLinkID = createRelationAction.PrjLinkID;
      serviceInstance.SetRelationSetting(ManufactOrdersAPI.settings.NewArticleLinkID, (IOrderItemSetting) ManufactOrdersAPI.settings);
    }
    ManufactOrdersAPI.ConvertManufactOrder(Intermech.Navigator.ContextMenu.Services.GetItems(orderItemPath.Items[0].F_PART_ID), (IServiceProvider) viewServices1, (object) null);
  }
}
