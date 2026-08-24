// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.Creator.OrderCreatorProvider
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using Intermech.Search.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.PdmConfigurator.Creator;

public sealed class OrderCreatorProvider : ICommandsProvider
{
  private static readonly int orderTypeID = MetaDataHelper.GetObjectTypeID("cad00580-306c-11d8-b4e9-00304f19f545");
  private static readonly int complementsTypeID = MetaDataHelper.GetObjectTypeID("cad015b1-306c-11d8-b4e9-00304f19f545");
  private static List<IDBTypedObjectID> objectIDs = new List<IDBTypedObjectID>();

  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    if (((viewServices.GetService(typeof (IViewState)) is IViewState service ? (long) service.ViewState : 0L) & 2L) != 0L)
      return CommandsInfo.Empty;
    bool flag1 = true;
    bool flag2 = true;
    for (int index = 0; index < items.Count; ++index)
    {
      if (!MetaDataHelper.IsPdmConfigurableObjectType(items.GetItemData(index, typeof (IDBObjectTypeID)) is IDBObjectTypeID itemData ? itemData.Value : -1))
      {
        flag1 = flag2 = false;
        break;
      }
    }
    CommandsInfo groupCommands = new CommandsInfo();
    if (flag1)
      groupCommands.Add("CreateOrder", new CommandInfo(0, new ClickEventHandler(OrderCreatorProvider.CreateOrderOrComplements), (object) OrderCreatorProvider.orderTypeID));
    if (flag2)
      groupCommands.Add("CreateComplements", new CommandInfo(0, new ClickEventHandler(OrderCreatorProvider.CreateOrderOrComplements), (object) OrderCreatorProvider.complementsTypeID));
    return groupCommands;
  }

  public static void CreateOrderOrComplements(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    int int32 = Convert.ToInt32(additionalInfo);
    OrderCreatorProvider.objectIDs.Clear();
    for (int index = 0; index < items.Count; ++index)
    {
      IDBTypedObjectID itemData = items.GetItemData(index, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
      OrderCreatorProvider.objectIDs.Add(itemData);
    }
    IObjectCreatorService service = ServicesManager.GetService(typeof (IObjectCreatorService)) as IObjectCreatorService;
    service.AfterDraftCreatedEvent += new AfterDraftCreatedEventHandler(OrderCreatorProvider.cDlg_ObjectCreatorDraftCreatedEvent);
    try
    {
      long objectByTypeDialog = service.CreateObjectByTypeDialog(int32);
      switch (objectByTypeDialog)
      {
        case -1:
          break;
        case 0:
          break;
        default:
          DBObjectsEventArgs e = new DBObjectsEventArgs("ObjectsCreated", objectByTypeDialog);
          (viewServices.GetService(typeof (INotificationService)) as INotificationService).FireEvent((object) null, (NotificationEventArgs) e);
          break;
      }
    }
    finally
    {
      service.AfterDraftCreatedEvent -= new AfterDraftCreatedEventHandler(OrderCreatorProvider.cDlg_ObjectCreatorDraftCreatedEvent);
      OrderCreatorProvider.objectIDs.Clear();
    }
  }

  private static void cDlg_ObjectCreatorDraftCreatedEvent(
    object sender,
    AfterDraftCreatedEventArgs e)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(e.ObjectID, false);
      if (dbObject == null)
        return;
      foreach (IDBTypedObjectID objectId in OrderCreatorProvider.objectIDs)
      {
        int suitableRelationType = OrderCreatorProvider.GetSuitableRelationType(dbObject.ObjectType, objectId.ObjectType);
        if (!RelationTypeHelper.IsUnknownRelationTypeID(suitableRelationType))
          sessionKeeper.Session.GetRelationCollection(suitableRelationType).Create(e.ObjectID, objectId.ObjectID);
      }
    }
  }

  private static int GetSuitableRelationType(int projectType, int partType)
  {
    List<int> source = MetaDataHelper.GetObjectTypeParentsID(partType) ?? new List<int>();
    source.Insert(0, partType);
    List<IMSApplicability> applicabilities = MetaDataHelper.GetObjectTypeApplicabilities(projectType) ?? new List<IMSApplicability>();
    IMSApplicability imsApplicability = source.Select<int, IMSApplicability>((Func<int, IMSApplicability>) (partOrAscenderType => applicabilities.FirstOrDefault<IMSApplicability>((Func<IMSApplicability, bool>) (applicability => applicability.ChildObjectTypeID == partOrAscenderType && applicability.Options.HasFlag((Enum) ApplicabilityOptions.DefaultRelation))))).Where<IMSApplicability>((Func<IMSApplicability, bool>) (applicability => applicability != null)).FirstOrDefault<IMSApplicability>() ?? source.Select<int, IMSApplicability>((Func<int, IMSApplicability>) (partOrAscenderType => applicabilities.FirstOrDefault<IMSApplicability>((Func<IMSApplicability, bool>) (applicability => applicability.ChildObjectTypeID == partOrAscenderType)))).Where<IMSApplicability>((Func<IMSApplicability, bool>) (applicability => applicability != null)).FirstOrDefault<IMSApplicability>();
    return imsApplicability == null ? -1 : imsApplicability.RelationTypeID;
  }
}
