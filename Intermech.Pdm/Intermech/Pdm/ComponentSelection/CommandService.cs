// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.ComponentSelection.CommandService
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.DataFormats;
using Intermech.Imbase;
using Intermech.Imbase.Common;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Imbase;
using Intermech.Interfaces.Pdm;
using Intermech.Navigator;
using Intermech.Navigator.Descriptos;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

#nullable disable
namespace Intermech.Pdm.ComponentSelection;

internal sealed class CommandService : IComponentSelectionCommandService
{
  public static List<int> EnabledTypes = new List<int>()
  {
    MetaDataHelper.GetObjectTypeID(new Guid("cad00132-306c-11d8-b4e9-00304f19f545")),
    MetaDataHelper.GetObjectTypeID(new Guid("cad00250-306c-11d8-b4e9-00304f19f545")),
    MetaDataHelper.GetObjectTypeID(new Guid("cad0038d-306c-11d8-b4e9-00304f19f545")),
    MetaDataHelper.GetObjectTypeID(new Guid("cad00252-306c-11d8-b4e9-00304f19f545"))
  };
  private int relationTypeComponentSelection;
  private int attributeSelectionForPosDesignation;
  private int attributeCountOnRegulation;

  public CommandService(IServiceProvider serviceProvider)
  {
    this.relationTypeComponentSelection = MetaDataHelper.GetRelationTypeID(ComponentSelectionConsts.relationTypeComponentSelection);
    this.attributeCountOnRegulation = MetaDataHelper.GetAttributeTypeID(ComponentSelectionConsts.attributeCountOnRegulation);
    this.attributeSelectionForPosDesignation = MetaDataHelper.GetAttributeTypeID(ComponentSelectionConsts.attributeSelectionForPosDesignation);
  }

  public long CreateNew(IUserSession session, long[] projectIDs, Guid[] relationGuids)
  {
    List<ObjectRelationLink> objectRelationLinkList = new List<ObjectRelationLink>(projectIDs.Length);
    IObjectCreatorService service = ServicesManager.GetService(typeof (IObjectCreatorService)) as IObjectCreatorService;
    int selectedTypeID = -1;
    for (int index = 0; index < projectIDs.Length; ++index)
    {
      IDBRelation relation = session.GetRelation(relationGuids[index], projectIDs[index]);
      if (index == 0)
        selectedTypeID = session.GetObjectInfo(relation.PartObjectID).ObjectTypeID;
      string posDesignation;
      MeasuredValue count;
      this.CheckRelation(relation, out posDesignation, out count);
      ObjectRelationLink objectRelationLink = new ObjectRelationLink(projectIDs[index], this.relationTypeComponentSelection)
      {
        Attributes = new Dictionary<int, object>()
      };
      objectRelationLink.Attributes.Add(this.attributeSelectionForPosDesignation, (object) posDesignation);
      if (count != null)
        objectRelationLink.Attributes.Add(this.attributeCountOnRegulation, (object) count);
      objectRelationLinkList.Add(objectRelationLink);
    }
    long objectByTypeDialog = service.CreateObjectByTypeDialog(CommandService.EnabledTypes.ToArray(), objectRelationLinkList.ToArray(), selectedTypeID);
    if (objectByTypeDialog != -1L)
      (ServicesManager.GetService(typeof (INotificationService)) as INotificationService).FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsCreated", objectByTypeDialog));
    return objectByTypeDialog;
  }

  public long[] AddExisting(IUserSession session, long[] projectIDs, Guid[] relationGuids)
  {
    IServiceContainer nodesContext = (IServiceContainer) new ServiceContainer();
    IObjectTypeNodeFilter serviceInstance = (IObjectTypeNodeFilter) new ObjectTypeNodeFilter();
    nodesContext.AddService(typeof (IObjectTypeNodeFilter), (object) serviceInstance);
    IDBTypedObjectID[] dbTypedObjectIdArray = (IDBTypedObjectID[]) SelectionWindow.Select("Выберите подборный компонент", (IDescriptor) new ObjectTypesDescriptor(CommandService.EnabledTypes.ToArray(), "Объекты"), typeof (IDBTypedObjectID), (IServiceProvider) nodesContext, SelectionOptions.Default | SelectionOptions.DisableMultiselect | SelectionOptions.ForceFilterObjectsByRule);
    return dbTypedObjectIdArray != null && dbTypedObjectIdArray.Length != 0 ? this.CreateRelationsWithExistingObject(session, projectIDs, relationGuids, dbTypedObjectIdArray[0].ObjectID) : (long[]) null;
  }

  public long[] AddFromImbase(IUserSession session, long[] projectIDs, Guid[] relationGuids)
  {
    IDBRelation relation = session.GetRelation(relationGuids[0], projectIDs[0]);
    IImbaseSelector service = ServicesManager.GetService(typeof (IImbaseSelector)) as IImbaseSelector;
    List<long> catalogIdForObjType = ImbaseUtils.GetCatalogIDForObjType(new int[1]
    {
      session.GetObjectInfo(relation.PartObjectID).ObjectTypeID
    }, session);
    object catalogId = catalogIdForObjType.Count <= 0 ? (object) new ImbaseRootNodeDescriptor() : (object) catalogIdForObjType;
    long objectID = service.SelectFromCatalog("Выберите подборный компонент", string.Empty, catalogId, false, true, (int[]) null, -1);
    return objectID != -1L ? this.CreateRelationsWithExistingObject(session, projectIDs, relationGuids, objectID) : (long[]) null;
  }

  private long[] CreateRelationsWithExistingObject(
    IUserSession session,
    long[] projectIDs,
    Guid[] relationGuids,
    long objectID)
  {
    IDBTransactions customService = (IDBTransactions) session.GetCustomService(typeof (IDBTransactions));
    try
    {
      customService.StartTransaction();
      List<long> longList = new List<long>();
      for (int index = 0; index < projectIDs.Length; ++index)
      {
        string posDesignation;
        MeasuredValue count;
        this.CheckRelation(session.GetRelation(relationGuids[index], projectIDs[index]), out posDesignation, out count);
        longList.Add(this.CreateRelationWithExistingObject(session, projectIDs[index], objectID, posDesignation, count));
      }
      customService.Commit();
      return longList.ToArray();
    }
    catch
    {
      customService.Rollback();
      throw;
    }
  }

  private long CreateRelationWithExistingObject(
    IUserSession session,
    long projectID,
    long objectID,
    string posDesignation,
    MeasuredValue count)
  {
    IList<long> longList = ((IObjectsCheckOutService) ServicesManager.GetService(typeof (IObjectsCheckOutService))).CheckOut(session, (IList<long>) new long[1]
    {
      projectID
    }, true);
    long componentSelection = (session.GetCustomService(typeof (IComponentSelectionService)) as IComponentSelectionService).CreateComponentSelection(session.SessionGUID, longList[0], objectID, posDesignation, count);
    if (componentSelection != 0L)
      (ServicesManager.GetService(typeof (INotificationService)) as INotificationService).FireEvent((object) this, (NotificationEventArgs) new DBRelationsEventArgs("RelationsCreated", componentSelection));
    return componentSelection;
  }

  public void Reset(IUserSession session, long[] projectIDs, Guid[] relationGuids)
  {
    IComponentSelectionService customService1 = session.GetCustomService(typeof (IComponentSelectionService)) as IComponentSelectionService;
    INotificationService service = ServicesManager.GetService(typeof (INotificationService)) as INotificationService;
    IDBTransactions customService2 = (IDBTransactions) session.GetCustomService(typeof (IDBTransactions));
    try
    {
      customService2.StartTransaction();
      for (int index = 0; index < projectIDs.Length; ++index)
      {
        List<long> removedRelationIds = (List<long>) null;
        long changedRelationId;
        customService1.ResetComponentSelection(session.SessionGUID, projectIDs[index], relationGuids[index], out changedRelationId, out removedRelationIds);
        if (removedRelationIds != null)
          service.FireEvent((object) this, (NotificationEventArgs) new DBRelationsEventArgs("RelationsRemoved", (IList<long>) removedRelationIds));
        service.FireEvent((object) this, (NotificationEventArgs) new DBRelationsEventArgs("RelationsChanged", changedRelationId));
      }
      customService2.Commit();
    }
    catch
    {
      customService2.Rollback();
      throw;
    }
  }

  private void CheckRelation(
    IDBRelation relation,
    out string posDesignation,
    out MeasuredValue count)
  {
    if (!ComponentSelectionHelper.IsMainComponent(relation, out posDesignation))
      throw new Exception("Выбранный объект не является основным компонентом для подбора. Создание подборного компонета невозможно.");
    count = (MeasuredValue) null;
    IDBAttribute attributeByGuid = relation.GetAttributeByGuid(new Guid("cad00267-306c-11d8-b4e9-00304f19f545"));
    if (attributeByGuid == null || attributeByGuid.IsNull)
      return;
    count = (MeasuredValue) attributeByGuid.Value;
  }
}
