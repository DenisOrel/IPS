// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.MRP2Service
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using Intermech.Navigator.Controls;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MRP2;

internal class MRP2Service
{
  internal static bool DisabledEvents;

  internal static IDBRelation ReplaceLink(
    IUserSession session,
    long masterObjectID,
    Guid replacedRelation,
    long newPartID)
  {
    if (newPartID == 0L)
      return (IDBRelation) null;
    IDBRelation relation = session.GetRelation(replacedRelation, masterObjectID, false);
    if (relation == null)
      return (IDBRelation) null;
    relation.ReplacePartObject(newPartID);
    MRP2Service.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsChanged", relation.RelationID, relation.ProjID, relation.RelationType));
    return relation;
  }

  internal static long CreateProductionCopy(
    IUserSession session,
    long masterObjectID,
    int objectTypeID,
    List<Guid> replacedRelations,
    List<long> replacePartIDs,
    bool autoCheckout,
    out List<Guid> new_relations)
  {
    IDBRelation rel_new = (IDBRelation) null;
    List<Guid> x_relations = new List<Guid>();
    DBRecordSetParams ps = new DBRecordSetParams(new ConditionStructure[0], new object[3]
    {
      (object) ObligatoryObjectAttributes.F_OBJECT_ID,
      (object) ObligatoryObjectAttributes.F_PRJLINK_ID,
      (object) ObligatoryObjectAttributes.F_PRJ_GUID
    });
    IDBObject newInstance = MRP2Consts.CreateObjectCopy(session, masterObjectID, objectTypeID);
    _copypasterelations(MRP2Consts.reltypeIdProductComposition);
    _copypasterelations(MRP2Consts.reltypeIdDocumentComposition);
    new_relations = x_relations;
    newInstance.CommitCreation(true, autoCheckout);
    MRP2Service.FireEvent((object) null, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsCreated", newInstance.ObjectID, objectTypeID));
    return newInstance.ObjectID;

    void _copypasterelations(int relationType)
    {
      IDBRelationCollection relationCollection = session.GetRelationCollection(relationType);
      DataTable dataTable = relationCollection.ConsistFrom(ps, masterObjectID);
      NewRelationProperties properties = new NewRelationProperties()
      {
        ProjectObjectID = newInstance.ObjectID
      };
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        long int64_1 = Convert.ToInt64(row[0]);
        long int64_2 = Convert.ToInt64(row[1]);
        Guid guid = new Guid(Convert.ToString(row[2]));
        int index = replacedRelations.IndexOf(guid);
        properties.PrototypeRelationID = int64_2;
        if (index < 0)
        {
          properties.PartObjectID = int64_1;
          rel_new = relationCollection.Create(properties);
        }
        else
        {
          long replacePartId = replacePartIDs[index];
          properties.PartObjectID = replacePartId;
          rel_new = relationCollection.Create(properties);
          x_relations.Add(rel_new.GUID);
        }
      }
    }
  }

  /// <summary>
  /// Создать производственную копию на основе другой производственной копии
  /// </summary>
  /// <param name="keeper"></param>
  /// <param name="masterObjectID"></param>
  /// <param name="objectTypeID"></param>
  /// <param name="replacedRelation"></param>
  /// <param name="replacePartID"></param>
  /// <param name="autoCheckout"></param>
  /// <returns></returns>
  internal static long CreateProductionCopyWithReplacedPart(
    IUserSession session,
    long masterObjectID,
    int objectTypeID,
    Guid replacedRelation,
    long replacePartID,
    bool autoCheckout,
    out Guid new_relation,
    out Dictionary<Guid, Guid> newGuids)
  {
    return MRP2Service.CreateProductionCopyWithReplacedPart2(session, masterObjectID, objectTypeID, replacedRelation, replacePartID, autoCheckout, out new_relation, out newGuids).ObjectID;
  }

  internal static IDBObject CreateProductionCopyWithReplacedPart2(
    IUserSession session,
    long masterObjectID,
    int objectTypeID,
    Guid replacedRelation,
    long replacePartID,
    bool autoCheckout,
    out Guid new_relation,
    out Dictionary<Guid, Guid> newGuids)
  {
    new_relation = Guid.Empty;
    IDBRelationCollection relationCollection1 = session.GetRelationCollection(MRP2Consts.reltypeIdProductComposition);
    DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[0], new object[3]
    {
      (object) ObligatoryObjectAttributes.F_OBJECT_ID,
      (object) ObligatoryObjectAttributes.F_PRJLINK_ID,
      (object) ObligatoryObjectAttributes.F_PRJ_GUID
    });
    IDBObject objectCopy = MRP2Consts.CreateObjectCopy(session, masterObjectID, objectTypeID);
    newGuids = new Dictionary<Guid, Guid>();
    NewRelationProperties properties = new NewRelationProperties()
    {
      ProjectObjectID = objectCopy.ObjectID
    };
    foreach (DataRow row in (InternalDataCollectionBase) relationCollection1.ConsistFrom(paramSet, masterObjectID).Rows)
    {
      long int64_1 = Convert.ToInt64(row[0]);
      long int64_2 = Convert.ToInt64(row[1]);
      Guid key = new Guid(Convert.ToString(row[2]));
      properties.PrototypeRelationID = int64_2;
      IDBRelation dbRelation;
      if (key != replacedRelation)
      {
        properties.PartObjectID = int64_1;
        dbRelation = relationCollection1.Create(properties);
      }
      else
      {
        properties.PartObjectID = replacePartID;
        dbRelation = relationCollection1.Create(properties);
        new_relation = dbRelation.GUID;
      }
      newGuids[key] = dbRelation.GUID;
    }
    AttributeValues[] attributeValuesArray = new AttributeValues[1]
    {
      new AttributeValues(MRP2Consts.attrIdCompositionVersionID, (object) 0)
    };
    IDBRelationCollection relationCollection2 = session.GetRelationCollection(MRP2Consts.reltypeIdDocumentComposition);
    foreach (DataRow row in (InternalDataCollectionBase) relationCollection2.ConsistFrom(paramSet, masterObjectID).Rows)
    {
      long int64_3 = Convert.ToInt64(row[0]);
      long int64_4 = Convert.ToInt64(row[1]);
      Guid key = new Guid(Convert.ToString(row[2]));
      properties.PrototypeRelationID = int64_4;
      IDBRelation dbRelation;
      if (key != replacedRelation)
      {
        properties.PartObjectID = int64_3;
        properties.ValuesList = (AttributeValues[]) null;
        dbRelation = relationCollection2.Create(properties);
      }
      else
      {
        attributeValuesArray[0].Values[0] = (object) Math.Abs(replacePartID);
        properties.PartObjectID = replacePartID;
        properties.ValuesList = attributeValuesArray;
        dbRelation = relationCollection2.Create(properties);
        new_relation = dbRelation.GUID;
      }
      newGuids[key] = dbRelation.GUID;
    }
    objectCopy.CommitCreation(true, autoCheckout);
    MRP2Service.FireEvent((object) null, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsCreated", objectCopy.ObjectID, objectTypeID));
    return objectCopy;
  }

  internal static IDBObject CheckOutTreeCopy(
    IDBObject parent,
    MRP2Service.DoOperationDelegate doOperation)
  {
    DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[0], new object[2]
    {
      (object) ObligatoryObjectAttributes.F_OBJECT_ID,
      (object) ObligatoryObjectAttributes.F_PRJLINK_ID
    });
    bool flag = false;
    IDBObject dbObject1;
    if (parent.CheckoutBy == parent.Session.UserID)
    {
      dbObject1 = parent;
    }
    else
    {
      dbObject1 = MRP2Consts.CreateObjectCopy(parent.Session, parent.ObjectID, parent.ObjectType);
      flag = true;
    }
    IDBRelationCollection relationCollection1 = parent.Session.GetRelationCollection(MRP2Consts.reltypeIdProductComposition);
    foreach (DataRow row in (InternalDataCollectionBase) relationCollection1.ConsistFrom(paramSet, parent.ObjectID).Rows)
    {
      long int64_1 = Convert.ToInt64(row[0]);
      long int64_2 = Convert.ToInt64(row[1]);
      IDBRelation relation = parent.Session.GetRelation(int64_2);
      IDBObject parent1 = parent.Session.GetObject(int64_1);
      IDBObject dbObject2 = MRP2Service.CheckOutTreeCopy(parent1, doOperation);
      if (!flag)
      {
        if (parent1.ObjectID != dbObject2.ObjectID)
          relation.ReplacePartObject(dbObject2.ObjectID);
      }
      else
      {
        NewRelationProperties properties = new NewRelationProperties()
        {
          ProjectObjectID = dbObject1.ObjectID,
          PartObjectID = dbObject2.ObjectID,
          PrototypeRelationID = int64_2
        };
        relation = relationCollection1.Create(properties);
      }
      if (dbObject2.IsCreationMode)
        dbObject2.CommitCreation(true, true);
      if (doOperation != null)
        doOperation(dbObject2, relation);
    }
    IDBRelationCollection relationCollection2 = parent.Session.GetRelationCollection(MRP2Consts.reltypeIdDocumentComposition);
    foreach (DataRow row in (InternalDataCollectionBase) relationCollection2.ConsistFrom(paramSet, parent.ObjectID).Rows)
    {
      long int64_3 = Convert.ToInt64(row[0]);
      long int64_4 = Convert.ToInt64(row[1]);
      IDBRelation relation;
      if (!flag)
      {
        relation = parent.Session.GetRelation(int64_4);
      }
      else
      {
        NewRelationProperties properties = new NewRelationProperties()
        {
          ProjectObjectID = dbObject1.ObjectID,
          PartObjectID = int64_3,
          PrototypeRelationID = int64_4
        };
        relation = relationCollection2.Create(properties);
      }
      if (doOperation != null)
        doOperation((IDBObject) null, relation);
    }
    return dbObject1;
  }

  /// <summary>
  /// Создаем дерево производственных копий по дереву навигатора с составом изделий
  /// </summary>
  /// <param name="Session"></param>
  /// <param name="newType">Тип созданной копии</param>
  /// <param name="Node">Нод дерева на основе которой создать копию</param>
  /// <param name="supplyMethod">Метод обработки (для выходных сборок)</param>
  /// <returns></returns>
  internal static long CreateObjectCopy4Production(
    IUserSession Session,
    int newType,
    NavigatorTreeNode Node,
    MRP2Consts.ArticleSupplyMethod? supplyMethod,
    Dictionary<NavigatorTreeNode, string> hashDict,
    IDBRelationCollection zrc = null,
    IDBRelationCollection drc = null)
  {
    string hash = hashDict[Node];
    long objectCopy = MRP2Service.FindObjectCopy(Session, newType, hash);
    if (!Consts.IsUndefinedObjectId(objectCopy))
      return objectCopy;
    IDBObjectID data = Node.Handler.GetData(Node.NodeID, typeof (IDBObjectID)) as IDBObjectID;
    IDBObject dbObj = Session.GetObject(data.Value);
    if (newType == -1)
      newType = MRP2Consts.GetCopyType(Session, dbObj.ObjectType);
    IDBObject newInstance = Session.GetObjectCollection(newType).Create();
    newInstance.Attributes.AssignPossibleAttributes(dbObj.Attributes, Consts.CreateMode);
    MRP2Consts.FillCopyProperties(newInstance, newType, supplyMethod, dbObj, hash);
    if (zrc == null)
      zrc = dbObj.Session.GetRelationCollection(MRP2Consts.reltypeIdProductComposition);
    if (drc == null)
      drc = dbObj.Session.GetRelationCollection(MRP2Consts.reltypeIdDocumentComposition);
    Node.Fetch();
    AttributeValues[] attributeValuesArray = new AttributeValues[2];
    foreach (NavigatorTreeNode child in (List<NavigatorTreeNode>) Node.Children)
    {
      if (child.CheckState != CheckState.Unchecked)
      {
        INode handler = child.Handler;
        long aRelationID = (handler.GetData(child.NodeID, typeof (IDBRelationID)) as IDBRelationID).Value;
        IDBRelation relation = Session.GetRelation(aRelationID);
        NewRelationProperties relationProperties;
        if (relation.RelationType == MRP2Consts.reltypeIdDocumentation)
        {
          long num = (handler.GetData(child.NodeID, typeof (IDBObjectID)) as IDBObjectID).Value;
          attributeValuesArray[0] = new AttributeValues(MRP2Consts.attrIdCreatedByRelation, (object) relation.GUID);
          attributeValuesArray[1] = new AttributeValues(MRP2Consts.attrIdCompositionVersionID, (object) Math.Abs(num));
          relationProperties = new NewRelationProperties();
          relationProperties.ProjectObjectID = newInstance.ObjectID;
          relationProperties.PartObjectID = num;
          relationProperties.PrototypeRelationID = relation.RelationID;
          relationProperties.ValuesList = attributeValuesArray;
          NewRelationProperties properties = relationProperties;
          drc.Create(properties);
        }
        else
        {
          long objectCopy4Production = MRP2Service.CreateObjectCopy4Production(Session, -1, child, new MRP2Consts.ArticleSupplyMethod?(), hashDict, zrc, drc);
          attributeValuesArray[0] = new AttributeValues(MRP2Consts.attrIdCreatedByRelation, (object) relation.GUID);
          attributeValuesArray[1] = new AttributeValues(MRP2Consts.attrIdCompositionVersionID, (object) DeleteModesEnum.None);
          relationProperties = new NewRelationProperties();
          relationProperties.ProjectObjectID = newInstance.ObjectID;
          relationProperties.PartObjectID = objectCopy4Production;
          relationProperties.PrototypeRelationID = relation.RelationID;
          relationProperties.ValuesList = attributeValuesArray;
          NewRelationProperties properties = relationProperties;
          zrc.Create(properties);
        }
      }
    }
    newInstance.CommitCreation(true, false);
    return newInstance.ObjectID;
  }

  internal static string CalculateHashForObject(
    IDBObject dbObj,
    int newType,
    MRP2Consts.ArticleSupplyMethod? supplyMethod,
    bool IgnoreSupplyMethod,
    Dictionary<long, string> hashDict)
  {
    string dbObjectHash = MRP2Service.GetDBObjectHash(dbObj, newType, supplyMethod);
    List<string> stringList = new List<string>();
    if (supplyMethod.HasValue | IgnoreSupplyMethod)
    {
      foreach (DataRow row in (InternalDataCollectionBase) dbObj.Session.GetRelationCollection(MRP2Consts.reltypeIdSP).ConsistFrom(new DBRecordSetParams(new ConditionStructure[0], new ColumnDescriptor[2]
      {
        new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object, ColumnContents.Value, ColumnNameMapping.ID, SortOrders.NONE, 0),
        new ColumnDescriptor((object) MRP2Consts.attrIdCount, AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0)
      }), dbObj.ObjectID).Rows)
      {
        long int64 = Convert.ToInt64(row[0]);
        IDBObject dbObj1 = dbObj.Session.GetObject(int64);
        string hashForObject = MRP2Service.CalculateHashForObject(dbObj1, MRP2Consts.GetCopyType(dbObj.Session, dbObj1.ObjectType), new MRP2Consts.ArticleSupplyMethod?(), IgnoreSupplyMethod, hashDict);
        string stringValue = DataSetProcessor.GetStringValue(row, 1, "");
        stringList.Add($"{hashForObject}-{stringValue}");
      }
    }
    stringList.Sort();
    stringList.Add(dbObjectHash);
    string hashForObject1 = MRP2Service.HashData(string.Join("\r\n", stringList.ToArray()));
    hashDict[dbObj.ObjectID] = hashForObject1;
    return hashForObject1;
  }

  internal static string CalculateHashForTree(
    IUserSession Session,
    int newType,
    NavigatorTreeNode Node,
    MRP2Consts.ArticleSupplyMethod? supplyMethod,
    Dictionary<NavigatorTreeNode, string> hashDict)
  {
    IDBObjectID data1 = Node.Handler.GetData(Node.NodeID, typeof (IDBObjectID)) as IDBObjectID;
    IDBObject o = Session.GetObject(data1.Value);
    switch (newType)
    {
      case -2:
        newType = o.ObjectType;
        break;
      case -1:
        newType = MRP2Consts.GetCopyType(Session, o.ObjectType);
        break;
    }
    string dbObjectHash = MRP2Service.GetDBObjectHash(o, newType, supplyMethod);
    List<string> stringList = new List<string>();
    Node.Fetch();
    foreach (NavigatorTreeNode child in (List<NavigatorTreeNode>) Node.Children)
    {
      if (child.CheckState != CheckState.Unchecked)
      {
        IDBRelationID data2 = child.Handler.GetData(child.NodeID, typeof (IDBRelationID)) as IDBRelationID;
        IDBRelation relation = Session.GetRelation(data2.Value);
        newType = relation.RelationType == MRP2Consts.reltypeIdDocumentation ? -2 : -1;
        string hashForTree = MRP2Service.CalculateHashForTree(Session, newType, child, new MRP2Consts.ArticleSupplyMethod?(), hashDict);
        IDBAttribute attributeById = relation.GetAttributeByID(MRP2Consts.attrIdCount);
        string asString = attributeById == null ? "" : attributeById.AsString;
        stringList.Add($"{hashForTree}-{asString}");
      }
    }
    stringList.Sort();
    stringList.Add(dbObjectHash);
    string hashForTree1 = MRP2Service.HashData(string.Join("\r\n", stringList.ToArray()));
    hashDict[Node] = hashForTree1;
    return hashForTree1;
  }

  internal static string HashData(string data)
  {
    using (SHA256 shA256 = SHA256.Create())
      return BitConverter.ToString(shA256.ComputeHash(Encoding.UTF8.GetBytes(data))).Replace("-", "").ToLower();
  }

  internal static string GetDBObjectHash(
    IDBObject o,
    int newType,
    MRP2Consts.ArticleSupplyMethod? supplyMethod)
  {
    string caption;
    try
    {
      DateTime dateTime = o.ModifyDate;
      dateTime = dateTime.ToUniversalTime();
      caption = dateTime.ToString("ddMMyyyyHHmmssfff");
    }
    catch (KernelException ex)
    {
      caption = o.Caption;
    }
    return MRP2Service.HashData($"{o.ObjectGUID.ToString().ToLower()}-{caption}-{newType}-{supplyMethod.ToString()}");
  }

  internal static NodeID GetPLNodeID(ISelectedItems items)
  {
    NavigatorTreeView navigatorTreeView = items.GetItemData(0, typeof (NavigatorTreeNode)) is NavigatorTreeNode itemData ? itemData.Tree : throw new ApplicationException("Эту команду можно выполнить только в отдельном окне в контексте состава производственной ведомости");
    INodeID[] path = itemData.GetPath();
    string[] strArray = new string[path.Length];
    if (!(path[0] is NodeID plNodeId) || !MetaDataHelper.IsObjectTypeChildOf(plNodeId.ObjectTypeID, MRP2Consts.objtypeIdProductionLists))
      throw new ApplicationException("Эту команду можно выполнить только в отдельном окне в контексте состава производственной ведомости");
    return plNodeId;
  }

  internal static void FireEvent(object sender, NotificationEventArgs e)
  {
    if (MRP2Service.DisabledEvents)
      return;
    INotificationService service = (INotificationService) ServicesManager.GetService(typeof (INotificationService));
    if (service == null)
      return;
    bool flag = false;
    try
    {
      if (e.EventName == "RelationsChanged" && !NotificationEventNames.CriticalEventNames.Contains("RelationsChanged"))
      {
        NotificationEventNames.CriticalEventNames.Add("RelationsChanged");
        flag = true;
      }
      service.FireEvent(sender, e);
    }
    finally
    {
      if (flag)
        NotificationEventNames.CriticalEventNames.Remove("RelationsChanged");
    }
  }

  /// <summary>unused</summary>
  /// <param name="dbObj"></param>
  /// <param name="newType"></param>
  /// <param name="supplyMethod"></param>
  /// <returns></returns>
  internal static long FindObjectCopy(IUserSession Session, int newType, string hash)
  {
    DataTable dataTable = Session.GetObjectCollection(newType).Select(new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(MRP2Consts.attrIdHash, RelationalOperators.Equal, (object) hash, LogicalOperators.NONE, 0, false)
    }, new object[2]
    {
      (object) ObligatoryObjectAttributes.F_OBJECT_ID,
      (object) ObligatoryObjectAttributes.F_MODIFY_DATE
    }, new object[1]
    {
      (object) ObligatoryObjectAttributes.F_MODIFY_DATE
    }, new SortOrders[1]{ SortOrders.DESC })
    {
      Contents = new ColumnContents[2]
      {
        ColumnContents.ID,
        ColumnContents.ID
      }
    });
    return dataTable.Rows.Count > 0 ? Convert.ToInt64(dataTable.Rows[0][0]) : 0L;
  }

  internal static MRP2Consts.ArticleSupplyMethod? SelectArticleMethod()
  {
    object obj;
    return RadioGroupDialog.ExecuteDialog("Выберите метод обработки/поставки", "", MRP2Consts.ArticleSupplyMethod.Production.GetType(), out obj) == DialogResult.OK ? new MRP2Consts.ArticleSupplyMethod?((MRP2Consts.ArticleSupplyMethod) obj) : new MRP2Consts.ArticleSupplyMethod?();
  }

  internal delegate void DoOperationDelegate(IDBObject obj, IDBRelation rel);
}
