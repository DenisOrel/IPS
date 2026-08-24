// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.AddSostavCommand
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.DataFormats;
using Intermech.Imbase;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Imbase;
using Intermech.Navigator;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MRP2;

internal class AddSostavCommand
{
  /// <summary>Команда меню добавить в состав</summary>
  /// <param name="items"></param>
  /// <param name="viewServices"></param>
  /// <param name="additionalInfo"></param>
  internal static void Handler(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    INodeID itemId = items.GetItemID(0);
    if (itemId.CategoryID != 1 || !MetaDataHelper.IsObjectTypeChildOf(itemId.TypeID, MRP2Consts.objtypeIdProductionLists) && !MetaDataHelper.IsObjectTypeChildOf(itemId.TypeID, MRP2Consts.objtypeIdProductionCopy))
      return;
    IDBTypedObjectID itemData = (IDBTypedObjectID) items.GetItemData(0, typeof (IDBTypedObjectID));
    long version = MRP2Service.GetPLNodeID(items).Version;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      sessionKeeper.Session.GetObject(itemData.ObjectID).CheckEdit();
      List<Tuple<IDBObject, MeasuredValue>> tupleList = AddSostavCommand.SelectProductionCopy(sessionKeeper.Session, itemData.ObjectType, version);
      if (tupleList == null)
        return;
      IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(MRP2Consts.reltypeIdProductComposition);
      INotificationService service = (INotificationService) ServicesManager.GetService(typeof (INotificationService));
      foreach (Tuple<IDBObject, MeasuredValue> tuple in tupleList)
      {
        MeasuredValue initValue = tuple.Item2 ?? new MeasuredValue(1.0, PDMPluginIDs.measureShtuk);
        AttributeValues[] vals = new AttributeValues[2]
        {
          new AttributeValues(MRP2Consts.attrIdCount, (object) initValue),
          new AttributeValues(MRP2Consts.attrIdVersionNumberPL, (object) version)
        };
        IDBRelation dbRelation = relationCollection.Create(itemData.ObjectID, tuple.Item1.ObjectID, vals);
        service?.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsCreated", dbRelation.RelationID, dbRelation.ProjID, dbRelation.RelationType));
      }
    }
  }

  /// <summary>
  /// Выбрать копю для вставки или изделие и создать по нем копию для вставки
  /// </summary>
  /// <param name="Session"></param>
  /// <param name="parentObjectType"></param>
  /// <param name="Count"></param>
  /// <returns></returns>
  internal static List<Tuple<IDBObject, MeasuredValue>> SelectProductionCopy(
    IUserSession Session,
    int parentObjectType,
    long versionPL,
    SelectionOptions selOptions = SelectionOptions.SelectObjects,
    MeasuredValue cnt = null)
  {
    List<Tuple<IDBObject, MeasuredValue>> tupleList = (List<Tuple<IDBObject, MeasuredValue>>) null;
    IDBObjectID[] dbObjectIdArray = AddSostavCommand.MRP2SelectDialog(Session, selOptions, true);
    if (dbObjectIdArray != null && dbObjectIdArray.Length != 0)
    {
      tupleList = new List<Tuple<IDBObject, MeasuredValue>>();
      foreach (IDBObjectID dbObjectId in dbObjectIdArray)
      {
        if (dbObjectId != null)
        {
          Tuple<IDBObject, MeasuredValue> tuple = AddSostavCommand.MakeCopyForAdding(Session, dbObjectId.Value, parentObjectType, versionPL, cnt);
          if (tuple != null)
            tupleList.Add(tuple);
        }
      }
    }
    return tupleList;
  }

  internal static Tuple<IDBObject, MeasuredValue> MakeCopyForAdding(
    IUserSession Session,
    long objectID,
    int parentObjectType,
    long versionPL,
    MeasuredValue cnt = null)
  {
    INotificationService service = (INotificationService) ServicesManager.GetService(typeof (INotificationService));
    IDBObject dbObject = (IDBObject) null;
    IDBObject dbObj = Session.GetObject(objectID);
    MeasuredValue measuredValue = (MeasuredValue) null;
    int objectType = dbObj.ObjectType;
    if (dbObj.isParentType(new Guid("cad00268-306c-11d8-b4e9-00304f19f545")) || dbObj.isParentType(new Guid("cad00170-306c-11d8-b4e9-00304f19f545")))
    {
      CompositionOptionsWizard compositionOptionsWizard = new CompositionOptionsWizard();
      try
      {
        if (cnt != null)
          compositionOptionsWizard.Count = cnt;
        string str = "";
        int num;
        if (MetaDataHelper.IsObjectTypeChildOf(parentObjectType, MRP2Consts.objtypeIdProductionLists) && (dbObj.isParentType(new Guid("cad00132-306c-11d8-b4e9-00304f19f545")) || dbObj.isParentType(new Guid("cad0025f-306c-11d8-b4e9-00304f19f545")) || dbObj.isParentType(new Guid("cad0025e-306c-11d8-b4e9-00304f19f545"))) && dbObj.ObjectType != MRP2Consts.objtypeIdParts)
        {
          num = MRP2Consts.objtypeIdExitAssembly;
          if (compositionOptionsWizard.Execute((long) num, objectID) != DialogResult.OK)
            return (Tuple<IDBObject, MeasuredValue>) null;
          str = compositionOptionsWizard.GetSupplyMethod();
        }
        else
        {
          num = MRP2Consts.GetCopyType(Session, objectType);
          if (compositionOptionsWizard.Execute((long) num, objectID) != DialogResult.OK)
            return (Tuple<IDBObject, MeasuredValue>) null;
        }
        MRP2Consts.ArticleSupplyMethod? articleSupplyMethod1 = MRP2Consts.StringToArticleSupplyMethod(str);
        MRP2Consts.ArticleSupplyMethod? nullable = articleSupplyMethod1;
        MRP2Consts.ArticleSupplyMethod articleSupplyMethod2 = MRP2Consts.ArticleSupplyMethod.Production;
        long objectID1;
        if (nullable.GetValueOrDefault() == articleSupplyMethod2 & nullable.HasValue || !articleSupplyMethod1.HasValue)
        {
          Dictionary<NavigatorTreeNode, string> hashDict = new Dictionary<NavigatorTreeNode, string>();
          MRP2Service.CalculateHashForTree(Session, num, compositionOptionsWizard.navigatorTreeView1.TreeView.RootNode, articleSupplyMethod1, hashDict);
          objectID1 = MRP2Service.CreateObjectCopy4Production(Session, num, compositionOptionsWizard.navigatorTreeView1.TreeView.RootNode, articleSupplyMethod1, hashDict);
        }
        else
        {
          Dictionary<long, string> hashDict = new Dictionary<long, string>();
          MRP2Service.CalculateHashForObject(dbObj, num, articleSupplyMethod1, false, hashDict);
          objectID1 = MRP2Consts.CreateObjectCopy(dbObj, 0L, num, versionPL, articleSupplyMethod1, false, hashDict, (AttributeValues[]) null);
        }
        dbObject = Session.GetObject(objectID1);
        service?.FireEvent((object) null, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsCreated", dbObject.ObjectID, dbObject.ObjectType));
        measuredValue = compositionOptionsWizard.Count;
      }
      finally
      {
        compositionOptionsWizard.RemoveFiltrationSettings();
      }
    }
    else if (dbObj.isParentType(new Guid("cadd9a5d-306c-11d8-b4e9-00304f19f545")))
      dbObject = dbObj;
    return dbObject != null ? new Tuple<IDBObject, MeasuredValue>(dbObject, measuredValue) : (Tuple<IDBObject, MeasuredValue>) null;
  }

  internal static IDBObjectID[] MRP2SelectDialog(
    IUserSession session,
    SelectionOptions selOptions,
    bool withImbase)
  {
    DescriptorCollection descriptors = new DescriptorCollection()
    {
      (IDescriptor) new Intermech.Navigator.DBObjectTypes.Descriptor(MetaDataHelper.GetObjectTypeID("cad00268-306c-11d8-b4e9-00304f19f545")),
      (IDescriptor) new Intermech.Navigator.DBObjectTypes.Descriptor(MRP2Consts.objtypeIdProductionCopy)
    };
    IImbaseServer customService = session.GetCustomService(typeof (IImbaseServer)) as IImbaseServer;
    if (withImbase && customService != null && ServicesManager.GetService(typeof (IImbaseSelector)) is IImbaseSelector service1)
    {
      long[] catalogsForCreateType = customService.GetCatalogsForCreateType(session.SessionGUID, (object) new string[2]
      {
        "cad00268-306c-11d8-b4e9-00304f19f545",
        "cad00170-306c-11d8-b4e9-00304f19f545"
      }, true);
      DescriptorCollection descriptorCollection = descriptors;
      List<long> list = catalogsForCreateType != null ? ((IEnumerable<long>) catalogsForCreateType).ToList<long>() : (List<long>) null;
      IDescriptor rootDescriptor = service1.GetRootDescriptor(list);
      descriptorCollection.Add(rootDescriptor);
    }
    IDescriptor rootDescriptor1 = (IDescriptor) new Intermech.Navigator.CustomNode.Descriptor("Выбор объекта", descriptors);
    SelectionOptions options = SelectionOptions.SelectObjects | SelectionOptions.DisableSelectFromTree | SelectionOptions.DisableSelectAbstractTypes | selOptions;
    Intermech.Navigator.SelectionWindow.RegisterAnalyze((ISelectedItemsAnalyzer) new MRP2SelectedItemsAnalyzer(), true);
    object[] objArray = Intermech.Navigator.SelectionWindow.Select("Выберите объект", "", rootDescriptor1, typeof (IDBObjectID), (DynamicSelectionEventHandler) null, (IServiceProvider) null, options, (int[]) null);
    if (objArray == null || objArray.Length == 0)
      return (IDBObjectID[]) null;
    IDBObjectID[] dbObjectIdArray = new IDBObjectID[objArray.Length];
    for (int index = 0; index < objArray.Length; ++index)
    {
      dbObjectIdArray[index] = (IDBObjectID) null;
      if (objArray[index] is IImbaseTableRecordID imbaseTableRecordId)
      {
        IImbaseSelector service2;
        if ((service2 = ServiceUtils.GetService<IImbaseSelector>((object) ApplicationServices.Container, false)) != null && customService != null && service2.ContextObjectId != 0L && service2.ContextObjectId != -1L)
        {
          long objID = customService.CreateObject(session.SessionGUID, 0L, service2.ContextObjectId, imbaseTableRecordId.Value, true, -1);
          dbObjectIdArray[index] = (IDBObjectID) new DBObjectID(objID, -1L, "", 0L);
        }
      }
      else
        dbObjectIdArray[index] = objArray[index] as IDBObjectID;
    }
    return dbObjectIdArray;
  }
}
