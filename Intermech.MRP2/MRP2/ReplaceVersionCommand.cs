// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.ReplaceVersionCommand
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using Intermech.MRP2.Dialogs;
using Intermech.Navigator;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MRP2;

internal class ReplaceVersionCommand
{
  /// <summary>Команда меню заменить версию в составе</summary>
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
    NodeID plNodeId = MRP2Service.GetPLNodeID(items);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(items.GetItemData(0, typeof (IDBRelationID)) is IDBRelationID itemData1) || Intermech.Consts.IsUndefinedRelationId(itemData1.Value))
        return;
      IDBTypedObjectID itemData2 = (IDBTypedObjectID) items.GetItemData(0, typeof (IDBTypedObjectID));
      sessionKeeper.Session.GetObject(itemData1.ProjID).CheckEdit();
      IDBObject dbObject = sessionKeeper.Session.GetObject(itemData1.PartID);
      IDBAttribute byId1;
      for (; MetaDataHelper.IsObjectTypeChildOf(dbObject.ObjectType, MRP2Consts.objtypeIdProductionCopy); dbObject = sessionKeeper.Session.GetObject(byId1.AsInteger))
      {
        byId1 = dbObject.Attributes.FindByID(MRP2Consts.attrIdArticleLink);
        if (byId1 == null)
          throw new Exception("Не найден атрибут 'Ссылка на изделие' - нельзя узнать по какому объекту выпущена копия");
      }
      IDBAttribute byId2 = sessionKeeper.Session.GetObject(plNodeId.ObjectID).Attributes.FindByID(MRP2Consts.attrIdChangeBase);
      List<long> longList = new List<long>();
      if (byId2 != null)
      {
        foreach (object obj in byId2.Values)
        {
          if (obj != DBNull.Value)
            longList.Add(Convert.ToInt64(obj));
        }
      }
      long num1;
      if (longList.Count == 0)
      {
        num1 = ObjectVersionSelection.SelectVersion(dbObject.ID, true, (List<long>) null, dbObject.ObjectID, -dbObject.ObjectID);
      }
      else
      {
        ICompositionLoadService customService = sessionKeeper.Session.GetCustomService(typeof (ICompositionLoadService)) as ICompositionLoadService;
        List<ColumnDescriptor> columnDescriptorList = new List<ColumnDescriptor>()
        {
          new ColumnDescriptor((object) ObligatoryObjectAttributes.F_PROJ_ID, AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.FieldName, SortOrders.NONE, 0),
          new ColumnDescriptor((object) ObligatoryObjectAttributes.F_PART_ID, AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.FieldName, SortOrders.NONE, 0),
          new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.FieldName, SortOrders.NONE, 0)
        };
        IUserSession session = sessionKeeper.Session;
        List<long> projIDs = longList;
        int relationTypeId = MetaDataHelper.GetRelationTypeID("cad0036b-306c-11d8-b4e9-00304f19f545");
        List<ColumnDescriptor> columns = columnDescriptorList;
        int[] numArray = new int[1]{ dbObject.ObjectType };
        DataTable dataTable = customService.LoadComplexCompositions((object) session, (IEnumerable<long>) projIDs, relationTypeId, (IEnumerable<ColumnDescriptor>) columns, "cad001e2-306c-11d8-b4e9-00304f19f545", numArray);
        num1 = 0L;
        if (dataTable != null && dataTable.Rows.Count > 0)
        {
          foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
          {
            if (DataSetProcessor.GetInt64Value(row, "F_PART_ID", 0L) == dbObject.ID)
            {
              num1 = DataSetProcessor.GetInt64Value(row, "F_OBJECT_ID", 0L);
              break;
            }
          }
        }
        if (num1 == 0L)
          num1 = ObjectVersionSelection.SelectVersion(dbObject.ID, true, (List<long>) null, dbObject.ObjectID);
      }
      if (Intermech.Consts.IsUndefinedObjectId(num1))
        return;
      MRP2Consts.ArticleSupplyMethod? sMethod = new MRP2Consts.ArticleSupplyMethod?();
      AttributeValues[] attributesValues = sessionKeeper.Session.GetObjectAttributesValues(itemData1.PartID, new int[1]
      {
        MRP2Consts.attrIdSupplyMethod
      }, GetAttributeValuesModes.None, false);
      if (attributesValues != null && attributesValues[0] != null)
        sMethod = MRP2Consts.StringToArticleSupplyMethod(attributesValues[0].AsString);
      long num2 = ReplaceVersionCommand.ReplacePartVersionDialog(sessionKeeper.Session, itemData1.PartID, num1, plNodeId.Version, sMethod);
      IDBRelation relation = sessionKeeper.Session.GetRelation(itemData1.Value);
      if (Intermech.Consts.IsUndefinedObjectId(num2))
        return;
      relation.ReplacePartObject(num2);
      INotificationService service = (INotificationService) ServicesManager.GetService(typeof (INotificationService));
      if (service == null)
        return;
      service.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsRemoved", relation.RelationID, relation.ProjID, relation.RelationType));
      service.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsCreated", relation.RelationID, relation.ProjID, relation.RelationType));
    }
  }

  /// <summary>Команда меню заменить версию в составе для документов</summary>
  /// <param name="items"></param>
  /// <param name="viewServices"></param>
  /// <param name="additionalInfo"></param>
  internal static void DocHandler(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    INodeID itemId = items.GetItemID(0);
    if (itemId.CategoryID != 1 || !MetaDataHelper.IsObjectTypeChildOf(itemId.TypeID, MRP2Consts.objtypeIdDocument))
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(items.GetItemData(0, typeof (IDBRelationID)) is IDBRelationID itemData1) || Intermech.Consts.IsUndefinedRelationId(itemData1.Value))
        return;
      IDBTypedObjectID itemData2 = (IDBTypedObjectID) items.GetItemData(0, typeof (IDBTypedObjectID));
      sessionKeeper.Session.GetObject(itemData1.ProjID).CheckEdit();
      IDBObject dbObject = sessionKeeper.Session.GetObject(itemData1.PartID);
      long num = ObjectVersionSelection.SelectVersion(dbObject.ID, true, (List<long>) null, dbObject.ObjectID);
      IDBRelation relation = sessionKeeper.Session.GetRelation(itemData1.Value);
      if (Intermech.Consts.IsUndefinedObjectId(num))
        return;
      relation.ReplacePartObject(num);
      relation.SetAttributesValues(new AttributeValues[1]
      {
        new AttributeValues(MRP2Consts.attrIdCompositionVersionID, (object) Math.Abs(num))
      });
      INotificationService service = (INotificationService) ServicesManager.GetService(typeof (INotificationService));
      if (service == null)
        return;
      service.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsRemoved", relation.RelationID, relation.ProjID, relation.RelationType));
      service.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsCreated", relation.RelationID, relation.ProjID, relation.RelationType));
    }
  }

  /// <summary>
  /// Ф-ия покажет диалог сравнения состав изделия и копии и предложит перенести изменения из состава изделия в состав копии
  /// чтобы создать новую копию для замены в составе ПВ
  /// </summary>
  /// <param name="session">Сессия</param>
  /// <param name="copyObjectID">Идентификатор копии</param>
  /// <param name="newverObjectID">Идентификатор новой версии изделия</param>
  /// <param name="plVersion">Номер версии ПВ</param>
  /// <param name="sMethod">Метол обработки (null, пока не обрабатываем)</param>
  /// <returns></returns>
  internal static long ReplacePartVersionDialog(
    IUserSession session,
    long copyObjectID,
    long newverObjectID,
    long plVersion,
    MRP2Consts.ArticleSupplyMethod? sMethod)
  {
    // ISSUE: variable of a compiler-generated type
    ReplaceVersionCommand.\u003C\u003Ec__DisplayClass2_0 cDisplayClass20;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass20.session = session;
    // ISSUE: reference to a compiler-generated field
    ProductionListComparer plc = new ProductionListComparer(copyObjectID, newverObjectID, cDisplayClass20.session);
    if (plc.HasSostav)
    {
      if (DialogResult.OK != new ReplaceVersionDialog(plc).ShowDialog() || DialogResult.OK != AttributesCompareDialog.Execute(plc))
        return 0;
      // ISSUE: variable of a compiler-generated type
      ReplaceVersionCommand.\u003C\u003Ec__DisplayClass2_1 cDisplayClass21;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      cDisplayClass21.zrc = cDisplayClass20.session.GetRelationCollection(MRP2Consts.reltypeIdProductComposition);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      cDisplayClass21.drc = cDisplayClass20.session.GetRelationCollection(MRP2Consts.reltypeIdDocumentComposition);
      Dictionary<long, string> hashDict = new Dictionary<long, string>();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass20.session.GetObject(plc.artInfo.ObjectID, true);
      // ISSUE: reference to a compiler-generated field
      IDBObject objectCopy1 = MRP2Consts.CreateObjectCopy(cDisplayClass20.session, copyObjectID, plc.copyInfo.ObjectTypeID);
      objectCopy1.SetAttributesValues(plc.NewAttributesValues());
      objectCopy1.SetAttributesValues(new AttributeValues[1]
      {
        new AttributeValues(MRP2Consts.attrIdArticleLink, (object) plc.artInfo.ObjectID)
      });
      // ISSUE: reference to a compiler-generated field
      AttributeValues[] attributesValues = cDisplayClass20.session.GetObjectAttributesValues(plc.copyInfo.ObjectID, new int[1]
      {
        MRP2Consts.attrIdPKDSE_Id
      }, GetAttributeValuesModes.None, false);
      if (attributesValues != null)
        objectCopy1.SetAttributesValues(attributesValues);
      AttributeValues attributeValues = new AttributeValues(MRP2Consts.attrIdVersionNumberPL, (object) plVersion);
      // ISSUE: reference to a compiler-generated field
      cDisplayClass21.nrp = new NewRelationProperties()
      {
        ProjectObjectID = objectCopy1.ObjectID
      };
      foreach (CompositionItem compositionItem in (List<CompositionItem>) plc.rightItem)
      {
        if (!compositionItem.Empty)
        {
          if (compositionItem.CompositionItemFlag.HasFlag((Enum) CompositionItemFlags.CreateNewCopy))
          {
            if (compositionItem.CompositionItemFlag.HasFlag((Enum) CompositionItemFlags.AnotherVersion))
            {
              if (compositionItem.RelationTypeID == MRP2Consts.reltypeIdProductComposition)
              {
                // ISSUE: reference to a compiler-generated field
                IDBObject dbObject = cDisplayClass20.session.GetObject(compositionItem.ObjectID, true);
                // ISSUE: reference to a compiler-generated field
                IDBObject withReplacedPart2 = MRP2Service.CreateProductionCopyWithReplacedPart2(cDisplayClass20.session, dbObject.ObjectID, dbObject.ObjectType, Guid.Empty, 0L, true, out Guid _, out Dictionary<Guid, Guid> _);
                IList<AttributeValues> source = plc.CompositionAttributeValues(compositionItem, AttributeSourceTypes.Object);
                source.Add(new AttributeValues(MRP2Consts.attrIdArticleLink, (object) compositionItem.ReplacedObjectID()));
                MRP2Consts.SafeSetAttributeValues((IDBAttributable) withReplacedPart2, source.ToArray<AttributeValues>());
                IList<AttributeValues> aValues = plc.CompositionAttributeValues(compositionItem, AttributeSourceTypes.Relation);
                aValues.Add(attributeValues);
                ReplaceVersionCommand.\u003CReplacePartVersionDialog\u003Eg___makenewrelation2\u007C2_1(withReplacedPart2.ObjectID, compositionItem.PrjLinkID, aValues, ref cDisplayClass20, ref cDisplayClass21);
              }
              else
              {
                IList<AttributeValues> aValues = plc.CompositionAttributeValues(compositionItem, AttributeSourceTypes.Relation);
                aValues.Add(attributeValues);
                ReplaceVersionCommand.\u003CReplacePartVersionDialog\u003Eg___makenewrelation2\u007C2_1(compositionItem.ReplacedObjectID(), compositionItem.PrjLinkID, aValues, ref cDisplayClass20, ref cDisplayClass21);
              }
            }
            else if (compositionItem.CompositionItemFlag.HasFlag((Enum) CompositionItemFlags.AttributesChanged))
            {
              // ISSUE: reference to a compiler-generated field
              IDBObject dbObject = cDisplayClass20.session.GetObject(compositionItem.ObjectID, true);
              // ISSUE: reference to a compiler-generated field
              IDBObject withReplacedPart2 = MRP2Service.CreateProductionCopyWithReplacedPart2(cDisplayClass20.session, dbObject.ObjectID, dbObject.ObjectType, Guid.Empty, 0L, true, out Guid _, out Dictionary<Guid, Guid> _);
              MRP2Consts.SafeSetAttributeValues((IDBAttributable) withReplacedPart2, plc.CompositionAttributeValues(compositionItem, AttributeSourceTypes.Object).ToArray<AttributeValues>());
              IList<AttributeValues> aValues = plc.CompositionAttributeValues(compositionItem, AttributeSourceTypes.Relation);
              aValues.Add(attributeValues);
              ReplaceVersionCommand.\u003CReplacePartVersionDialog\u003Eg___makenewrelation2\u007C2_1(withReplacedPart2.ObjectID, compositionItem.PrjLinkID, aValues, ref cDisplayClass20, ref cDisplayClass21);
            }
            else if (compositionItem.CompositionItemFlag.HasFlag((Enum) CompositionItemFlags.AttributesChangedInCompositionObject))
            {
              IList<AttributeValues> aValues = plc.CompositionAttributeValues(compositionItem, AttributeSourceTypes.Relation);
              aValues.Add(attributeValues);
              ReplaceVersionCommand.\u003CReplacePartVersionDialog\u003Eg___makenewrelation2\u007C2_1(compositionItem.ObjectID, compositionItem.PrjLinkID, aValues, ref cDisplayClass20, ref cDisplayClass21);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              IDBRelation relation = cDisplayClass20.session.GetRelation(compositionItem.PrjLinkID, true);
              if (relation.RelationType == MRP2Consts.reltypeIdSP)
              {
                hashDict.Clear();
                // ISSUE: reference to a compiler-generated field
                IDBObject dbObj = cDisplayClass20.session.GetObject(compositionItem.ObjectID, true);
                // ISSUE: reference to a compiler-generated field
                int copyType = MRP2Consts.GetCopyType(cDisplayClass20.session, dbObj.ObjectType);
                MRP2Service.CalculateHashForObject(dbObj, copyType, new MRP2Consts.ArticleSupplyMethod?(), true, hashDict);
                long objectCopy2 = MRP2Consts.CreateObjectCopy(dbObj, 0L, copyType, plVersion, new MRP2Consts.ArticleSupplyMethod?(), true, hashDict, plc.CompositionAttributeValues(compositionItem, AttributeSourceTypes.Object).ToArray<AttributeValues>());
                IList<AttributeValues> attributeValuesList = plc.CompositionAttributeValues(compositionItem, AttributeSourceTypes.Relation);
                attributeValuesList.Add(attributeValues);
                IDBRelation relpart = relation;
                IList<AttributeValues> aValues = attributeValuesList;
                ref ReplaceVersionCommand.\u003C\u003Ec__DisplayClass2_1 local = ref cDisplayClass21;
                ReplaceVersionCommand.\u003CReplacePartVersionDialog\u003Eg___makenewrelation1\u007C2_0(objectCopy2, relpart, aValues, ref local);
              }
              else
              {
                IList<AttributeValues> aValues = plc.CompositionAttributeValues(compositionItem, AttributeSourceTypes.Relation);
                aValues.Add(new AttributeValues(MRP2Consts.attrIdCompositionVersionID, (object) Math.Abs(compositionItem.ObjectID)));
                aValues.Add(attributeValues);
                ReplaceVersionCommand.\u003CReplacePartVersionDialog\u003Eg___makenewrelation1\u007C2_0(compositionItem.ObjectID, relation, aValues, ref cDisplayClass21);
              }
            }
          }
          else
            ReplaceVersionCommand.\u003CReplacePartVersionDialog\u003Eg___makenewrelation2\u007C2_1(compositionItem.ObjectID, compositionItem.PrjLinkID, (IList<AttributeValues>) null, ref cDisplayClass20, ref cDisplayClass21);
        }
      }
      objectCopy1.CommitCreation(true, false);
      return objectCopy1.ObjectID;
    }
    if (DialogResult.OK != AttributesCompareDialog.Execute(plc))
      return 0;
    Dictionary<long, string> hashDict1 = new Dictionary<long, string>();
    // ISSUE: reference to a compiler-generated field
    IDBObject dbObj1 = cDisplayClass20.session.GetObject(newverObjectID, true);
    int objectTypeId = plc.copyInfo.ObjectTypeID;
    MRP2Service.CalculateHashForObject(dbObj1, objectTypeId, sMethod, false, hashDict1);
    long objectCopy = MRP2Consts.CreateObjectCopy(dbObj1, copyObjectID, objectTypeId, plVersion, sMethod, false, hashDict1, plc.NewAttributesValues());
    // ISSUE: reference to a compiler-generated field
    AttributeValues[] attributesValues1 = cDisplayClass20.session.GetObjectAttributesValues(plc.copyInfo.ObjectID, new int[1]
    {
      MRP2Consts.attrIdPKDSE_Id
    }, GetAttributeValuesModes.None, false);
    if (attributesValues1 != null)
    {
      // ISSUE: reference to a compiler-generated field
      cDisplayClass20.session.SetObjectAttributesValues(objectCopy, false, attributesValues1);
    }
    return objectCopy;
  }
}
