// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Server.SignsServerStartup
// Assembly: Intermech.Signs.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3ABC2C25-9F6B-4AA7-A176-FCB28F816B8C
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Signs.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Briefcase;
using Intermech.Interfaces.Plugins;
using Intermech.Interfaces.Server;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Signs.Interfaces;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;

#nullable disable
namespace Intermech.Signs.Server;

public class SignsServerStartup : IPackage
{
  private EventLogHelper eventLogHelper;
  internal static SignsService Server = new SignsService();

  public void Unload()
  {
    this.eventLogHelper.RemoveAttributeWriteHandler((object) SignsHolder.ArchAttrTypeID, new WriteAttributeValueHandler(this.ArchiveAttributeWrite));
    this.eventLogHelper.AfterChangeObjectTypeEvent -= new ObjectTypeChangeHandler(this.eventLogHelper_AfterChangeObjectTypeEvent);
    this.eventLogHelper.AfterCreateObjectTypeEvent -= new AfterCreateObjectTypeHandler(this.eventLogHelper_AfterCreateObjectTypeEvent);
    this.eventLogHelper.AfterDeleteObjectTypeEvent -= new DeleteObjectTypeHandler(this.eventLogHelper_AfterDeleteObjectTypeEvent);
    this.eventLogHelper.BeforeNextLCStepEvent -= new NextLCStepHandler(this.eventLogHelper_BeforeNextLCStepEvent);
    this.eventLogHelper.AfterCreateApplicability -= new RelationsApplicabilityHandler(this.eventLogHelper_AfterCreateApplicability);
    this.eventLogHelper.AfterDeleteApplicability -= new RelationsApplicabilityHandler(this.eventLogHelper_AfterDeleteApplicability);
    this.eventLogHelper.CommitCreationObjectEvent -= new ObjectEventHandler(this.eventLogHelper_CommitCreationObjectEvent);
    this.eventLogHelper.CreateObjectEvent -= new ObjectEventHandler(this.eventLogHelper_CreateObjectEvent);
    this.eventLogHelper.DeleteAttributePossibleValueEvent -= new DeleteAttributePossibleValueHandler(this.eventLogHelper_DeleteAttributePossibleValueEvent);
    this.eventLogHelper.AfterCacheReload -= new CacheReloadHandler(this.eventLogHelper_AfterCacheReload);
  }

  public string Name => LocalizationHolder.rm.GetString("Signs.Server_7");

  public void Load(IServiceProvider serviceProvider)
  {
    (serviceProvider.GetService(typeof (ICustomServices)) as ICustomServices).AddService(typeof (ISignsService), (object) SignsServerStartup.Server);
    ServerServices.AddService(typeof (ISignsService), (object) SignsServerStartup.Server);
    IDBTimedEvents service1 = serviceProvider.GetService(typeof (IDBTimedEvents)) as IDBTimedEvents;
    this.eventLogHelper = serviceProvider.GetService(typeof (IEventLogHelper)) as EventLogHelper;
    IUserSession sessionTemporaryClone = service1.GetSystemSessionTemporaryClone("SignsServer.Load");
    try
    {
      SignsHolder.Init(sessionTemporaryClone, (IServiceProvider) ServerServices.ServiceContainer);
      Assembly.GetExecutingAssembly();
      if (ServerServices.GetService(typeof (IDBObjectService)) is ICreatorContainer service2)
      {
        service2.AddCreator((object) SignsHolder.SignObjectTypeGuid, (object) new SignDBObjectCreator());
        service2.AddCreator((object) SignsHolder.CryptoSignObjectTypeGuid, (object) new CryptoSignDBObjectCreator());
      }
      if (ServerServices.GetService(typeof (IDBRelationService)) is ICreatorContainer service3)
        service3.AddCreator((object) SignsHolder.SignRelationTypeGuid, (object) new SignDBRelationCreator());
      if (ServerServices.GetService(typeof (IDBRelationCollectionService)) is ICreatorContainer service4)
        service4.AddCreator((object) SignsHolder.SignRelationTypeGuid, (object) new SignDBRelationCollectionCreator());
      this.eventLogHelper.AddAttributeWriteHandler((object) SignsHolder.ArchAttrTypeID, new WriteAttributeValueHandler(this.ArchiveAttributeWrite));
      this.eventLogHelper.AfterChangeObjectTypeEvent += new ObjectTypeChangeHandler(this.eventLogHelper_AfterChangeObjectTypeEvent);
      this.eventLogHelper.AfterCreateObjectTypeEvent += new AfterCreateObjectTypeHandler(this.eventLogHelper_AfterCreateObjectTypeEvent);
      this.eventLogHelper.AfterDeleteObjectTypeEvent += new DeleteObjectTypeHandler(this.eventLogHelper_AfterDeleteObjectTypeEvent);
      this.eventLogHelper.BeforeNextLCStepEvent += new NextLCStepHandler(this.eventLogHelper_BeforeNextLCStepEvent);
      this.eventLogHelper.AfterCreateApplicability += new RelationsApplicabilityHandler(this.eventLogHelper_AfterCreateApplicability);
      this.eventLogHelper.AfterDeleteApplicability += new RelationsApplicabilityHandler(this.eventLogHelper_AfterDeleteApplicability);
      this.eventLogHelper.CommitCreationObjectEvent += new ObjectEventHandler(this.eventLogHelper_CommitCreationObjectEvent);
      this.eventLogHelper.CreateObjectEvent += new ObjectEventHandler(this.eventLogHelper_CreateObjectEvent);
      this.eventLogHelper.DeleteAttributePossibleValueEvent += new DeleteAttributePossibleValueHandler(this.eventLogHelper_DeleteAttributePossibleValueEvent);
      this.eventLogHelper.AfterCacheReload += new CacheReloadHandler(this.eventLogHelper_AfterCacheReload);
      if (ServerServices.GetService(typeof (ILinkedObjectsService)) is ILinkedObjectsService service5)
        service5.RegisterHandler((ILinkedObjectsHandler) new SignLinkedObjectsHandler());
      SignsServerCache.LoadObjectTypesForSignRelation();
      SignsServerUsersCache.LoadUsersInfo(sessionTemporaryClone);
      this.RegisterExportAttribute();
      SignsServerCache.LoadPossibleGraphs(sessionTemporaryClone);
      if (ServerServices.GetService(typeof (IPortalEventsService)) is IPortalEventsService service6)
      {
        service6.ImportTaskCompletedEvent += new ImportTaskCompletedEventHandler(this.OnSignImported);
        service6.RelationImportedEvent += new RelationImportedEventHandler(this.PortalEvents_RelationImportedEvent);
      }
      if (!(ServerServices.GetService(typeof (IPublishCompositionService)) is IPublishCompositionService service7))
        return;
      service7.RegisterIncludeObjectsAlwaysObjectType(SignsHolder.SignObjectTypeID);
      service7.RegisterIncludeObjectsAlwaysObjectType(SignsHolder.CryptoSignObjectTypeID);
    }
    finally
    {
      sessionTemporaryClone?.Logout("SignsServer.Load");
    }
  }

  private void PortalEvents_RelationImportedEvent(object sender, RelationImportedEventArgs e)
  {
    if (e.RelationType != SignsHolder.SignRelationTypeID)
      return;
    IDBObject objectById = e.Session.GetObjectByID(e.PartID, false);
    if (objectById == null || string.IsNullOrEmpty(objectById.SiteID))
      return;
    SiteInfo site = ((ISitesCacheService) e.Session.GetCustomService(typeof (ISitesCacheService))).GetSite(objectById.SiteID[0]);
    if (site == null || site.SystemType != SystemTypes.Search)
      return;
    int modifyContentDateId = e.Session.IdentHelper.ModifyContentDateID;
    IDBAttribute dbAttribute = objectById.GetAttributeByID(modifyContentDateId);
    if (dbAttribute != null && dbAttribute.AsDateTime.Equals(PortalConsts.SearchNullDate + e.Session.TimeZoneOffset))
      return;
    IDBAttribute attributeById = e.Session.GetObject(e.ProjectID).GetAttributeByID(modifyContentDateId);
    if (attributeById == null)
      return;
    if (dbAttribute == null)
      dbAttribute = objectById.Attributes.AddAttribute(modifyContentDateId, false);
    dbAttribute.AsDateTime = attributeById.AsDateTime;
    this.ReSignImported(objectById);
  }

  private void ReSignImported(IDBObject signObject)
  {
    byte[] inArray = HashPack.CalcHash(signObject);
    (signObject.GetAttributeByID(SignsHolder.HashProtectionAttrTypeID) ?? signObject.Attributes.AddAttribute(SignsHolder.HashProtectionAttrTypeID, false)).AsString = Convert.ToBase64String(inArray);
  }

  private void OnSignImported(object sender, ImportTaskCompletedEventArgs e)
  {
    foreach (Tuple<long, int> objectId in e.ObjectIDs)
    {
      if (objectId.Item2 == SignsHolder.SignObjectTypeID || objectId.Item2 == SignsHolder.CryptoSignObjectTypeID)
        this.ReSignImported(e.Session.GetObject(objectId.Item1));
    }
  }

  private void RegisterExportAttribute()
  {
    if (!(ServerServices.GetService(typeof (ICategoryExportManager)) is ICategoryExportManager service))
      return;
    ICategoryExport iCategoryExport = (ICategoryExport) new CryptoExporter();
    service.RegisterCategoryExport(1, iCategoryExport);
  }

  private void eventLogHelper_DeleteAttributePossibleValueEvent(
    IDBAttributeType attrType,
    object oldValue)
  {
    if (attrType.AttributeID != SignsHolder.GraphAttrTypeID)
      return;
    IUserSession userSession = (IUserSession) (attrType as DBSessionable).UserSession;
    StringBuilder stringBuilder = new StringBuilder();
    DataTable dataTable1 = userSession.GetObjectCollection(SignsHolder.RankTypeID).Select(new DBRecordSetParams((ConditionStructure[]) null, new object[2]
    {
      (object) -2,
      (object) -50
    }));
    for (int index = 0; index < dataTable1.Rows.Count; ++index)
    {
      byte[] rankSignsSetup = SignsServerStartup.Server.GetRankSignsSetup(Convert.ToInt64(dataTable1.Rows[index][0]), userSession.SessionGUID);
      if (rankSignsSetup != null)
      {
        Graphs4Type graphs4Type = new Graphs4Type((Stream) new MemoryStream(rankSignsSetup), SignsServerCache.PossibleGraphs);
        foreach (int objectType in graphs4Type)
        {
          if (graphs4Type.GetGraphs4ObjectType(userSession, objectType, true).Graphs.Contains(oldValue.ToString()))
            stringBuilder.Append(dataTable1.Rows[index][1].ToString() + ", ");
        }
      }
    }
    if (stringBuilder.Length > 0)
    {
      stringBuilder.Length -= 2;
      throw new KernelException($"Нельзя удалять графу для подписей номер {oldValue}, т.к. она используется в настройках подписей для следующих должностей: {stringBuilder.ToString()}.");
    }
    DataTable dataTable2 = userSession.GetObjectTypeCollection(-2).Select(string.Empty);
    for (int index1 = 0; index1 < dataTable2.Rows.Count; ++index1)
    {
      int int32 = Convert.ToInt32(dataTable2.Rows[index1]["F_OBJECT_TYPE"]);
      if (SignsServerCache.HasSignApp(int32))
      {
        DataTable table = userSession.GetLifecycleStepCollection(int32).GetSchema().Tables["IMS_LC_STEPS"];
        for (int index2 = 0; index2 < table.Rows.Count; ++index2)
        {
          GraphsSet graphsSet = SignsServerStartup.Server.LoadGraphsSet(new Guid(dataTable2.Rows[index1]["F_GUID"].ToString()), new Guid(table.Rows[index2]["F_GUID"].ToString()), userSession);
          if (graphsSet != null && graphsSet.Count > 0)
          {
            foreach (string key in (IEnumerable) graphsSet.Keys)
            {
              if (graphsSet[key].Contains(oldValue.ToString()))
                stringBuilder.AppendFormat("шаг '{0}' для типа объектов '{1}', ", table.Rows[index2]["F_LC_NAME"], dataTable2.Rows[index1]["F_OBJ_TYPE_NAME"]);
            }
          }
        }
      }
    }
    if (stringBuilder.Length > 0)
    {
      stringBuilder.Length -= 2;
      throw new KernelException($"Нельзя удалять графу для подписей номер {oldValue}, т.к. она используется в настройках подписей для следующих шагов ЖЦ: {stringBuilder.ToString()}.");
    }
    DataTable dataTable3 = userSession.GetLifecycleLevelCollection().Select(string.Empty);
    for (int index = 0; index < dataTable3.Rows.Count; ++index)
    {
      GraphsSet graphsSet = SignsServerStartup.Server.LoadGraphsSet(new Guid(dataTable3.Rows[index]["F_GUID"].ToString()), userSession);
      if (graphsSet != null && graphsSet.Count > 0)
      {
        foreach (string key in (IEnumerable) graphsSet.Keys)
        {
          if (graphsSet[key].Contains(oldValue.ToString()))
            stringBuilder.AppendFormat("'{0}', ", dataTable3.Rows[index]["F_LEVEL_NAME"]);
        }
      }
    }
    if (stringBuilder.Length > 0)
    {
      stringBuilder.Length -= 2;
      throw new KernelException($"Нельзя удалять графу для подписей номер {oldValue}, т.к. она используется в настройках подписей для следующих уровней продвижения: {stringBuilder.ToString()}.");
    }
    DataTable dataTable4 = userSession.GetObjectCollection(SignsHolder.ArchTypeID).Select(new DBRecordSetParams((ConditionStructure[]) null, new object[2]
    {
      (object) -2,
      (object) -50
    }));
    for (int index = 0; index < dataTable4.Rows.Count; ++index)
    {
      GraphsSet graphsSet = SignsServerStartup.Server.LoadArchiveGraphs(Convert.ToInt64(dataTable4.Rows[index][0]), userSession);
      if (graphsSet != null && graphsSet.Count > 0)
      {
        foreach (string key in (IEnumerable) graphsSet.Keys)
        {
          if (graphsSet[key].Contains(oldValue.ToString()))
            stringBuilder.AppendFormat("'{0}', ", dataTable4.Rows[index][1]);
        }
      }
    }
    if (stringBuilder.Length > 0)
    {
      stringBuilder.Length -= 2;
      throw new KernelException($"Нельзя удалять графу для подписей номер {oldValue}, т.к. она используется в настройках подписей для следующих архивов: {stringBuilder.ToString()}.");
    }
  }

  private void eventLogHelper_CommitCreationObjectEvent(IDBObject sender, IUserSession session)
  {
    if (sender.ObjectType == MetaDataHelper.GetObjectTypeID("cad00002-306c-11d8-b4e9-00304f19f545"))
    {
      SignsServerUsersCache.AddUser(sender.ObjectID, sender.ObjectGUID);
    }
    else
    {
      if (!MetaDataHelper.IsObjectTypeChildOf(sender.ObjectType, SignsHolder.DocumentObjectTypeID))
        return;
      IDBAttribute attributeById = sender.GetAttributeByID(SignsHolder.ArchAttrTypeID);
      if (attributeById == null || !attributeById.Value.GetType().Equals(typeof (long)))
        return;
      long asInteger = attributeById.AsInteger;
      switch (asInteger)
      {
        case -1:
          break;
        case 0:
          break;
        default:
          string errorMessage = (string) null;
          object[] additionalInfo = (object[]) null;
          if ((session.GetCustomService(typeof (ISignsService)) as ISignsService).CheckSigns(new long[1]
          {
            attributeById.DBObjectID
          }, asInteger, (GraphsSet) null, session.SessionGUID, true, false, out errorMessage, out additionalInfo))
            break;
          throw new KernelException(errorMessage == null ? LocalizationHolder.rm.GetString("Signs.Server_SignsCheckNegative") : errorMessage);
      }
    }
  }

  private void eventLogHelper_AfterCacheReload(IDbManager db)
  {
    if (!(ServerServices.GetService(typeof (ISignsService)) is ISignsService service))
      return;
    service.CleanCache();
  }

  private static void AttributesPatch(IUserSession session)
  {
    IMSAttribute4ObjectType attribute4ObjectType = MetaDataHelper.GetAttribute4ObjectType(SignsHolder.SignObjectTypeGuid, SignsHolder.HashProtectionAttrTypeGuid);
    IDBObjectType objectType = session.GetObjectType(SignsHolder.SignObjectTypeID, false);
    Attribute4ObjectTypeProperties attrProperties;
    if (attribute4ObjectType != null && attribute4ObjectType.FieldType != FieldTypes.ftString)
    {
      IDBAttribute4ObjectTypeCollection attributes = objectType.Attributes as IDBAttribute4ObjectTypeCollection;
      IDBAttributeType attributeType = session.GetAttributeType(SignsHolder.HashProtectionAttrTypeGuid, false);
      if (attributeType != null)
      {
        AttributeTypeProperties propertiesStructure = attributeType.PropertiesStructure with
        {
          AttributeGuid = new Guid("e1ca1473-90d0-42b3-81fa-b2e06fec32e7")
        };
        attributeType.PropertiesStructure = propertiesStructure;
        attributes.GetAttributeByID(attributeType.AttributeID).Required = RequiredModes.Manual;
        DataTable dataTable = session.GetObjectCollection(SignsHolder.SignObjectTypeID).Select(new DBRecordSetParams(new ConditionStructure[1]
        {
          new ConditionStructure(SignsHolder.HashProtectionAttrTypeID, RelationalOperators.AttributeExists, (object) null, LogicalOperators.AND, 0, false)
        })
        {
          Columns = new object[1]
          {
            (object) ObligatoryObjectAttributes.F_OBJECT_ID
          }
        });
        if (dataTable != null && dataTable.Rows.Count > 0)
        {
          foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
          {
            long int64 = Convert.ToInt64(row[0]);
            session.GetObject(int64, true).GetAttributeByID(SignsHolder.HashProtectionAttrTypeID).Delete(0L);
          }
        }
        attributes.GetAttributeByID(SignsHolder.HashProtectionAttrTypeID).Delete(1L);
        attributeType.Delete(1L);
      }
      SignsHolder.HashProtectionAttrTypeID = session.GetAttributeTypeCollection(-1).Create(new AttributeTypeProperties("Защита объекта", FieldTypes.ftString)
      {
        AttributeGuid = SignsHolder.HashProtectionAttrTypeGuid,
        FieldType = FieldTypes.ftString,
        SizeType = 64L /*0x40*/,
        ShortName = "Hash-Code",
        OptimizationMode = OptimizationModes.Read
      });
      SignsHolder.HashProtectionAttrTypeName = MetaDataHelper.GetAttributeTypeName(SignsHolder.HashProtectionAttrTypeID);
      attrProperties = new Attribute4ObjectTypeProperties(SignsHolder.HashProtectionAttrTypeID, SignsHolder.SignObjectTypeID, InheritModes.Public, RequiredModes.AutoRequired, string.Empty, ComputeValueModes.NotComputableValue, string.Empty, UniqueValueModes.NotUnique, 0, (object) string.Empty, OptimizationModes.Read, false, AttributeOptions.DisableManualEdit, string.Empty, 0, 0);
      attributes.Create(attrProperties);
    }
    if (MetaDataHelper.GetAttributeType(SignsHolder.ResolutionAttrTypeGuid) != null)
      return;
    IDBAttribute4ObjectTypeCollection attributes1 = objectType.Attributes as IDBAttribute4ObjectTypeCollection;
    SignsHolder.ResolutionAttrTypeID = session.GetAttributeTypeCollection(-1).Create(new AttributeTypeProperties("Резолюция", FieldTypes.ftString)
    {
      AttributeGuid = SignsHolder.ResolutionAttrTypeGuid,
      FieldType = FieldTypes.ftString,
      SizeType = 60L,
      OptimizationMode = OptimizationModes.Read
    });
    attrProperties = new Attribute4ObjectTypeProperties(SignsHolder.ResolutionAttrTypeID, SignsHolder.SignObjectTypeID, InheritModes.Public, RequiredModes.AutoRequired, string.Empty, ComputeValueModes.NotComputableValue, string.Empty, UniqueValueModes.NotUnique, 0, (object) string.Empty, OptimizationModes.Write, false, AttributeOptions.None, string.Empty, 0, 0);
    attributes1.Create(attrProperties);
  }

  private static void CopySignDate(IUserSession session)
  {
    DataTable dataTable = session.GetObjectCollection(SignsHolder.SignObjectTypeID).Select(new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(SignsHolder.DateOfSignatureID, RelationalOperators.Empty, (object) null, LogicalOperators.NONE, 0, false)
    })
    {
      Columns = new object[1]
      {
        (object) ObligatoryObjectAttributes.F_OBJECT_ID
      }
    });
    if (dataTable == null || dataTable.Rows.Count <= 0)
      return;
    foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
    {
      long int64 = Convert.ToInt64(row[0]);
      IDBObject dbObject = session.GetObject(int64, true);
      IDBAttribute attributeById = dbObject.GetAttributeByID(SignsHolder.DateOfSignatureID);
      if (attributeById != null)
      {
        DateTime createDate = dbObject.CreateDate;
        attributeById.Value = (object) createDate;
      }
    }
  }

  private static void ConteinerCorrection(IUserSession session)
  {
    string oldValue = "Контейнер для ЖЦ и типы объектов \"";
    DataTable dataTable = session.GetObjectCollection(SignsHolder.ContainerObjectTypeGuid).Select(new DBRecordSetParams(new ConditionStructure[2]
    {
      new ConditionStructure(SignsHolder.LCStepObjectTypeID, RelationalOperators.NotEmpty, (object) null, LogicalOperators.AND, 0, false),
      new ConditionStructure(SignsHolder.LCStepObjectTypeID, RelationalOperators.StartString, (object) "cad00922-306c-11d8-b4e9-00304f19f545", LogicalOperators.AND, 0, false)
    })
    {
      Columns = new object[1]
      {
        (object) ObligatoryObjectAttributes.F_OBJECT_ID
      }
    });
    if (dataTable == null || dataTable.Rows.Count <= 0)
      return;
    foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
    {
      long int64 = Convert.ToInt64(row[0]);
      IDBObject dbObject = session.GetObject(int64, true);
      IDBAttribute attributeById = dbObject.GetAttributeByID(SignsHolder.LCStepObjectTypeID);
      string g = dbObject.GetAttributeByID(SignsHolder.LCStepObjectTypeID).AsString.Replace("cad00922-306c-11d8-b4e9-00304f19f545", string.Empty);
      IDBObjectType objectType = session.GetObjectType(new Guid(g));
      if (objectType != null)
      {
        IDBLifecycleStepCollection lifecycleStepCollection = session.GetLifecycleStepCollection(objectType.ObjectType);
        string str1 = dbObject.Caption.Replace(oldValue, string.Empty).TrimEnd('"');
        DataSet schema = lifecycleStepCollection.GetSchema();
        if (schema != null)
        {
          DataRow[] dataRowArray = schema.Tables[0].Select($"F_LC_NAME='{str1}'");
          if (dataRowArray != null && dataRowArray.Length == 1)
          {
            string str2 = Convert.ToString(dataRowArray[0]["F_GUID"]) + g;
            attributeById.Value = (object) str2;
          }
        }
      }
    }
  }

  public void ArchiveAttributeWrite(IDBAttribute attribute, AttributeValueEventArgs e)
  {
    if (e.Value == null || !e.Value.GetType().Equals(typeof (long)) || Convert.ToInt64(e.Value) == 0L || Convert.ToInt64(e.Value) == -1L)
      return;
    if (!((attribute as DBAttribute).ParentObject is IDBObject parentObject))
      parentObject = e.Session.GetObject(attribute.DBObjectID);
    if (!parentObject.isParentType(SignsHolder.DocumentObjectTypeGuid) || parentObject.IsCreationMode)
      return;
    long int64 = Convert.ToInt64(e.Value);
    string errorMessage = (string) null;
    object[] additionalInfo = (object[]) null;
    if (!(e.Session.GetCustomService(typeof (ISignsService)) as ISignsService).CheckSigns(new long[1]
    {
      attribute.DBObjectID
    }, int64, (GraphsSet) null, e.Session.SessionGUID, true, false, out errorMessage, out additionalInfo))
      throw new KernelException(errorMessage == null ? LocalizationHolder.rm.GetString("Signs.Server_SignsCheckNegative") : errorMessage);
  }

  private void eventLogHelper_AfterChangeObjectTypeEvent(
    IDBObject sender,
    int objectTypeID,
    IUserSession session)
  {
    ISignsService customService = session.GetCustomService(typeof (ISignsService)) as ISignsService;
    string str = (string) null;
    object[] objArray = (object[]) null;
    long[] objectIDs = new long[1]{ sender.ObjectID };
    Guid sessionGuid = session.SessionGUID;
    ref string local1 = ref str;
    ref object[] local2 = ref objArray;
    if (!customService.CheckSigns(objectIDs, (GraphsSet) null, sessionGuid, true, out local1, out local2))
      throw new KernelException(str == null ? LocalizationHolder.rm.GetString("Signs.Server_SignsCheckNegative") : str);
  }

  private void eventLogHelper_BeforeNextLCStepEvent(
    IDBObject sender,
    IDBLifecycleStep nextstep,
    IUserSession session)
  {
    string errorMessage = (string) null;
    object[] additionalInfo = (object[]) null;
    if (!SignsServerStartup.Server.CheckSignsForNextStep(new IDBObject[1]
    {
      sender
    }, session, nextstep, out errorMessage, out additionalInfo))
      throw new KernelException(errorMessage == null ? LocalizationHolder.rm.GetString("Signs.Server_SignsCheckNegative") : errorMessage);
  }

  private void eventLogHelper_AfterCreateApplicability(
    IUserSession session,
    RelationsApplicabilityProperties applicabilityProperties)
  {
    if (!applicabilityProperties.RelationType.Equals(SignsHolder.SignRelationTypeID))
      return;
    SignsServerCache.AddObjectTypeForSign(applicabilityProperties.InObjectType);
  }

  private void eventLogHelper_AfterDeleteApplicability(
    IUserSession session,
    RelationsApplicabilityProperties applicabilityProperties)
  {
    if (!applicabilityProperties.RelationType.Equals(SignsHolder.SignRelationTypeID))
      return;
    SignsServerCache.RemoveObjectTypeForSign(applicabilityProperties.InObjectType);
  }

  private void eventLogHelper_AfterCreateObjectTypeEvent(IDBObjectType sender, IUserSession session)
  {
    DataTable applicabilitiesList = session.GetRelationsApplicabilityCollection().GetApplicabilitiesList(SignsHolder.SignRelationTypeID, SignsHolder.SignObjectTypeID, sender.ObjectType);
    if (applicabilitiesList == null && applicabilitiesList.Rows.Count <= 0)
      return;
    SignsServerCache.AddObjectTypeForSign(sender.ObjectType);
  }

  private void eventLogHelper_AfterDeleteObjectTypeEvent(IDBObjectType sender, IUserSession session)
  {
    if (!MetaDataHelper.HasApplicability(sender.ObjectType, SignsHolder.SignObjectTypeID, SignsHolder.SignRelationTypeID))
      return;
    SignsServerCache.RemoveObjectTypeForSign(sender.ObjectType);
  }

  private void eventLogHelper_CreateObjectEvent(IDBObject sender, IUserSession session)
  {
    if (sender == null || sender.ParentVersionID == -1L || !MetaDataHelper.HasApplicability(sender.ObjectType, SignsHolder.SignObjectTypeID, SignsHolder.SignRelationTypeID) || !(session.GetCustomService(typeof (ISignsService)) is ISignsService customService))
      return;
    customService.CreateCopySigns(sender, session);
  }
}
