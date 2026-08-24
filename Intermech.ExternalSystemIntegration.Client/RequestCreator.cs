// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.RequestCreator
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using Intermech.ExternalSystemIntegration.Interfaces;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using Intermech.Navigator;
using Intermech.Navigator.CustomNode;
using Intermech.Navigator.Interfaces;
using System;
using System.Data;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client;

public static class RequestCreator
{
  public static long CreateRequest(long objectID)
  {
    long request = 0;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IUserSession session = sessionKeeper.Session;
      IDBObject sourceObject = session.GetObject(objectID);
      IObjTypeSettingItemObject setting = RequestCreator.GetSetting(MetaDataHelper.GetObjectTypeGuid(sourceObject.ObjectType), session);
      if (setting == null)
        return request;
      long[] requestConfigs = setting.RequestConfigs;
      if (requestConfigs.Length > 1)
      {
        IDescriptor fromValue = (IDescriptor) ObjectsSelectionDescriptor.CreateFromValue<string>(Const.RequestConfigObjTypeID, ServiceHolder.rm.GetString("ExtInt_17"), Const.LinkObjectAttrTypeID, setting.LinkObjGuid);
        long[] numArray = SelectionWindow.SelectObjects(ServiceHolder.rm.GetString("ExtInt_16"), ServiceHolder.rm.GetString("ExtInt_15"), fromValue, SelectionOptions.Default);
        if (numArray == null || numArray.Length != 1)
          return request;
        IRequestConfigObject requestConfigObject = session.GetObject(numArray[0], true) as IRequestConfigObject;
        request = RequestCreator.CreateNewRequest(session, sourceObject, requestConfigObject);
      }
      else if (requestConfigs.Length == 1)
      {
        IRequestConfigObject requestConfigObject = session.GetObject(requestConfigs[0], true) as IRequestConfigObject;
        request = RequestCreator.CreateNewRequest(session, sourceObject, requestConfigObject);
      }
    }
    return request;
  }

  private static long CreateNewRequest(
    IUserSession session,
    IDBObject sourceObject,
    IRequestConfigObject requestConfigObject)
  {
    if (!(session.GetCustomService(typeof (IRequestObjectHelperService)) is IRequestObjectHelperService customService))
      throw new Exception(ServiceHolder.rm.GetString("ExtInt_21"));
    long newRequest;
    if (requestConfigObject.ShowCard)
    {
      IObjectCreatorService service = ServiceUtils.GetService<IObjectCreatorService>((object) ServicesManager.ServiceContainer, true);
      IObjectCreatorParams objectCreatorParams = (IObjectCreatorParams) new RequestCreatorParams(sourceObject.ObjectID);
      int requestObjTypeId = Const.RequestObjTypeID;
      OpenEditorMode openEditorMode;
      ref OpenEditorMode local = ref openEditorMode;
      IObjectCreatorParams creatorParams = objectCreatorParams;
      newRequest = service.CreateObjectByTypeDialog(requestObjTypeId, out local, creatorParams);
    }
    else
    {
      IDBObject dbObject = session.GetObjectCollection(Const.RequestObjTypeID).Create();
      customService.AssignAttributes(dbObject.ObjectID, sourceObject.ObjectID, session.SessionGUID);
      if (dbObject.IsCreationMode)
        dbObject.CommitCreation(true);
      newRequest = dbObject.ObjectID;
    }
    return newRequest;
  }

  private static IObjTypeSettingItemObject GetSetting(Guid objectTypeGuid, IUserSession session)
  {
    IObjTypeSettingItemObject setting = (IObjTypeSettingItemObject) null;
    DataTable dataTable = session.GetObjectCollection(Const.TypeSettingItemObjTypeID).Select(new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(Const.ObjectTypeIDAttrTypeID, RelationalOperators.Equal, (object) objectTypeGuid, LogicalOperators.NONE, 0, false)
    }, new ColumnDescriptor[1]
    {
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object, ColumnContents.ID, ColumnNameMapping.Name, SortOrders.NONE, 0)
    }));
    if (dataTable.Rows.Count > 0)
      setting = session.GetObject(Convert.ToInt64(dataTable.Rows[0][0]), false) as IObjTypeSettingItemObject;
    return setting;
  }
}
