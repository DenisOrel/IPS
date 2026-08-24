// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.ProductionListComparer
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Interfaces;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;

#nullable disable
namespace Intermech.MRP2;

internal class ProductionListComparer
{
  private long _copyObjectID;
  private long _articleObjectID;
  private DataTable _copyComposition;
  private DataTable _articleComposition;
  public List<CompositionItemAttribute> NewAttributes = new List<CompositionItemAttribute>();
  public QuickObjectInfo copyInfo;
  public QuickObjectInfo artInfo;
  public CompositionItem leftItem;
  public CompositionItem rightItem;

  public bool HasSostav { get; private set; }

  public ProductionListComparer(long copyObjectID, long articleObjectID, IUserSession userSession)
  {
    this._copyObjectID = copyObjectID;
    this._articleObjectID = articleObjectID;
    this.copyInfo = userSession.GetObjectInfo(copyObjectID);
    this.artInfo = userSession.GetObjectInfo(articleObjectID);
    if (MRP2PropertyPage.cfg_ObjectAttributes == null)
    {
      MRP2PropertyPage.cfg_ObjectAttributes = MRP2PropertyPage.cfg_compareAttrs.Where<Tuple<int, int, AttributableElements>>((System.Func<Tuple<int, int, AttributableElements>, bool>) (x => x.Item3 == AttributableElements.Object)).Select<Tuple<int, int, AttributableElements>, (int, int)>((System.Func<Tuple<int, int, AttributableElements>, (int, int)>) (x => (x.Item1, x.Item2))).ToList<(int, int)>();
      MRP2PropertyPage.cfg_SostavAttributes = MRP2PropertyPage.cfg_compareAttrs.Where<Tuple<int, int, AttributableElements>>((System.Func<Tuple<int, int, AttributableElements>, bool>) (x => x.Item3 == AttributableElements.Relation)).Select<Tuple<int, int, AttributableElements>, (int, int)>((System.Func<Tuple<int, int, AttributableElements>, (int, int)>) (x => (x.Item1, x.Item2))).ToList<(int, int)>();
    }
    ICompositionLoadService customService = userSession.GetCustomService(typeof (ICompositionLoadService)) as ICompositionLoadService;
    List<ColumnDescriptor> _copyCols = new List<ColumnDescriptor>()
    {
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_PROJ_ID, AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_PART_ID, AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_TYPE, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_VERSION_ID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_BASE_VERSION, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.CAPTION, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_ID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJ_GUID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_PRJLINK_ID, AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_RELATION_TYPE, AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) MRP2Consts.attrIdArticleLink, AttributeSourceTypes.Object, ColumnContents.ID, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) MRP2Consts.attrIdCreatedByRelation, AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) MRP2Consts.attrIdDeleteTag, AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) MRP2Consts.attrIdPKDSE_Id, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0)
    };
    MRP2PropertyPage.cfg_ObjectAttributes.ForEach((Action<(int, int)>) (x => this.FillColumns(_copyCols, x.CopyAttributeID, AttributeSourceTypes.Object)));
    MRP2PropertyPage.cfg_SostavAttributes.ForEach((Action<(int, int)>) (x => this.FillColumns(_copyCols, x.CopyAttributeID, AttributeSourceTypes.Relation)));
    int[] searchRelationTypes1 = new int[2]
    {
      MRP2Consts.reltypeIdProductComposition,
      MRP2Consts.reltypeIdDocumentComposition
    };
    this._copyComposition = customService.LoadComposition((object) userSession, copyObjectID, this.copyInfo.ObjectTypeID, (IEnumerable<int>) searchRelationTypes1, (IEnumerable<int>) null, (IEnumerable<ColumnDescriptor>) _copyCols, true, false, (VersionsRule) null, (IEnumerable<ConditionStructure>) null, "cad001e2-306c-11d8-b4e9-00304f19f545", (HybridDictionary) null, 1);
    int aSortID = MetaDataHelper.GetAttributeTypeID("cad00202-306c-11d8-b4e9-00304f19f545");
    List<ColumnDescriptor> _articleCols = new List<ColumnDescriptor>()
    {
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_PROJ_ID, AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_PART_ID, AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_PRJ_GUID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_TYPE, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_VERSION_ID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_BASE_VERSION, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.CAPTION, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_ID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJ_GUID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_PRJLINK_ID, AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_RELATION_TYPE, AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) aSortID, AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0)
    };
    MRP2PropertyPage.cfg_ObjectAttributes.ForEach((Action<(int, int)>) (x => this.FillColumns(_articleCols, x.ArticleAttributeID, AttributeSourceTypes.Object)));
    MRP2PropertyPage.cfg_SostavAttributes.ForEach((Action<(int, int)>) (x => this.FillColumns(_articleCols, x.ArticleAttributeID, AttributeSourceTypes.Relation)));
    ConditionStructure[] conditions = (ConditionStructure[]) null;
    int[] searchRelationTypes2;
    if (MRP2Consts.GetDocConditions(userSession) != null)
    {
      searchRelationTypes2 = new int[2]
      {
        MRP2Consts.reltypeIdSP,
        MRP2Consts.reltypeIdDocumentation
      };
      conditions = ConditionStructure.Join(new ConditionStructure[2]
      {
        new ConditionStructure(-23, RelationalOperators.Equal, (object) MRP2Consts.reltypeIdSP, LogicalOperators.OR, 0, false),
        new ConditionStructure(-23, RelationalOperators.Equal, (object) MRP2Consts.reltypeIdDocumentation, LogicalOperators.OR, 1, false)
      }, MRP2Consts.GetDocConditions(userSession));
      conditions[conditions.Length - 1].GroupID = -1;
    }
    else
      searchRelationTypes2 = new int[1]
      {
        MRP2Consts.reltypeIdSP
      };
    this._articleComposition = customService.LoadComposition((object) userSession, articleObjectID, this.artInfo.ObjectTypeID, (IEnumerable<int>) searchRelationTypes2, (IEnumerable<int>) null, (IEnumerable<ColumnDescriptor>) _articleCols, true, false, (VersionsRule) null, (IEnumerable<ConditionStructure>) conditions, "cad001e2-306c-11d8-b4e9-00304f19f545", (HybridDictionary) null, 1);
    this.HasSostav = this._copyComposition != null && this._copyComposition.Rows.Count > 0 || this._articleComposition != null && this._articleComposition.Rows.Count > 0;
    IDBObject dbObject1 = userSession.GetObject(this._copyObjectID, true);
    this.rightItem = new CompositionItem(dbObject1.ObjectID, dbObject1.ID, dbObject1.ObjectType, (long) dbObject1.VersionID, dbObject1.Caption, dbObject1.OwnerID, dbObject1.IsBaseVersion ? 1L : 0L, dbObject1.SiteID, dbObject1.ModificationID, userSession.GetLifecycleStep(dbObject1.LCStep).LevelID, dbObject1.CheckoutBy, dbObject1.ProjectID);
    this.InitCompositionItem(_copyCols, dbObject1, this.rightItem);
    IDBObject dbObject2 = userSession.GetObject(this._articleObjectID, true);
    this.leftItem = new CompositionItem(dbObject2.ObjectID, dbObject2.ID, dbObject2.ObjectType, (long) dbObject2.VersionID, dbObject2.Caption, dbObject2.OwnerID, dbObject2.IsBaseVersion ? 1L : 0L, dbObject2.SiteID, dbObject2.ModificationID, userSession.GetLifecycleStep(dbObject2.LCStep).LevelID, dbObject2.CheckoutBy, dbObject2.ProjectID);
    this.InitCompositionItem(_articleCols, dbObject2, this.leftItem);
    if (!this.HasSostav)
      return;
    DataColumn column1 = (DataColumn) null;
    DataColumn column2 = (DataColumn) null;
    DataColumn dataColumn = (DataColumn) null;
    DataColumn dcaObjGUID = (DataColumn) null;
    DataColumn column3 = (DataColumn) null;
    DataColumn column4 = (DataColumn) null;
    DataColumn dccPKDSEID = (DataColumn) null;
    DataRow[] source1;
    int index1;
    if (this._articleComposition != null)
    {
      source1 = this._articleComposition.Select("", $"[{-26}] ASC");
      column1 = this._articleComposition.Columns[-26.ToString()];
      dataColumn = this._articleComposition.Columns[-2.ToString()];
      DataColumnCollection columns = this._articleComposition.Columns;
      index1 = -18;
      string name = index1.ToString();
      dcaObjGUID = columns[name];
    }
    else
      source1 = new DataRow[0];
    DataRow[] source2;
    if (this._copyComposition != null)
    {
      source2 = this._copyComposition.Select("", $"[{MRP2Consts.attrIdCreatedByRelation.ToString()}] ASC");
      column2 = this._copyComposition.Columns[MRP2Consts.attrIdCreatedByRelation.ToString()];
      DataColumnCollection columns1 = this._copyComposition.Columns;
      index1 = -2;
      string name1 = index1.ToString();
      column3 = columns1[name1];
      column4 = this._copyComposition.Columns[MRP2Consts.attrIdArticleLink.ToString()];
      DataColumnCollection columns2 = this._copyComposition.Columns;
      index1 = -23;
      string name2 = index1.ToString();
      DataColumn column5 = columns2[name2];
      DataColumnCollection columns3 = this._copyComposition.Columns;
      index1 = -18;
      string name3 = index1.ToString();
      DataColumn column6 = columns3[name3];
      dccPKDSEID = this._copyComposition.Columns[MRP2Consts.attrIdPKDSE_Id.ToString()];
      DataRow[] dataRowArray = source2;
      for (index1 = 0; index1 < dataRowArray.Length; ++index1)
      {
        DataRow dataRow = dataRowArray[index1];
        if (DataSetProcessor.GetInt32Value(dataRow[column5], 0) != MRP2Consts.reltypeIdProductComposition)
        {
          dataRow[column4] = (object) Math.Abs(DataSetProcessor.GetInt64Value(dataRow[column3], 0L));
          dataRow[dccPKDSEID] = dataRow[column6];
        }
      }
    }
    else
      source2 = new DataRow[0];
    int index2 = 0;
    for (int index3 = 0; index2 < source1.Length || index3 < source2.Length; ++index3)
    {
      if (index2 < source1.Length == index3 < source2.Length)
      {
        DataRow dataRow1 = source1[index2];
        DataRow dataRow2 = source2[index3];
        string strA = dataRow1[column1].ToString();
        string str = dataRow2[column2].ToString();
        long int64Value1 = DataSetProcessor.GetInt64Value(dataRow1, dataColumn.Ordinal, 0L);
        DataSetProcessor.GetInt64Value(dataRow2, column3.Ordinal, 0L);
        long int64Value2 = DataSetProcessor.GetInt64Value(dataRow2, column4.Ordinal, 0L);
        string strB = str;
        long num = (long) string.Compare(strA, strB);
        if (num == 0L)
        {
          CompositionItemFlags Flag = this.CompareRows(dataRow1, Math.Abs(int64Value1), dataRow2, int64Value2);
          CompositionItem compositionItem1 = ProductionListComparer.InitCompositionItem(this.leftItem, _articleCols, Flag, dataRow1);
          CompositionItem compositionItem2 = ProductionListComparer.InitCompositionItem(this.rightItem, _copyCols, Flag, dataRow2);
          compositionItem1.LevelIndex = this.leftItem.Count - 1;
          compositionItem2.LevelIndex = this.rightItem.Count - 1;
          source1[index2] = (DataRow) null;
          source2[index3] = (DataRow) null;
        }
        else if (num > 0L)
          --index2;
        else
          --index3;
      }
      ++index2;
    }
    DataRow[] array1 = ((IEnumerable<DataRow>) source1).Where<DataRow>((System.Func<DataRow, bool>) (r => r != null)).OrderBy<DataRow, string>((System.Func<DataRow, string>) (r => DataSetProcessor.GetStringValue(r, dcaObjGUID.Ordinal, ""))).ToArray<DataRow>();
    DataRow[] array2 = ((IEnumerable<DataRow>) source2).Where<DataRow>((System.Func<DataRow, bool>) (r => r != null)).OrderBy<DataRow, string>((System.Func<DataRow, string>) (r => DataSetProcessor.GetStringValue(r, dccPKDSEID.Ordinal, ""))).ToArray<DataRow>();
    int index4 = 0;
    for (int index5 = 0; index4 < array1.Length || index5 < array2.Length; ++index5)
    {
      CompositionItem compositionItem3;
      CompositionItem compositionItem4;
      if (index4 < array1.Length != index5 < array2.Length)
      {
        if (index4 >= array1.Length)
        {
          compositionItem3 = new CompositionItem(this.leftItem, true, 1);
          this.leftItem.Add(compositionItem3);
          DataRow row = array2[index5];
          compositionItem4 = ProductionListComparer.InitCompositionItem(this.rightItem, _copyCols, CompositionItemFlags.Added, row);
        }
        else
        {
          DataRow row = array1[index4];
          compositionItem3 = ProductionListComparer.InitCompositionItem(this.leftItem, _articleCols, CompositionItemFlags.Removed, row);
          compositionItem4 = new CompositionItem(this.rightItem, true, 1);
          this.rightItem.Add(compositionItem4);
        }
      }
      else
      {
        DataRow dataRow3 = array1[index4];
        DataRow dataRow4 = array2[index5];
        string strA = dataRow3[dcaObjGUID].ToString();
        string str = dataRow4[dccPKDSEID].ToString();
        long int64Value3 = DataSetProcessor.GetInt64Value(dataRow3, dataColumn.Ordinal, 0L);
        DataSetProcessor.GetInt64Value(dataRow4, column3.Ordinal, 0L);
        long int64Value4 = DataSetProcessor.GetInt64Value(dataRow4, column4.Ordinal, 0L);
        string strB = str;
        long num = (long) string.Compare(strA, strB);
        if (num == 0L)
        {
          CompositionItemFlags Flag = this.CompareRows(dataRow3, Math.Abs(int64Value3), dataRow4, int64Value4);
          compositionItem3 = ProductionListComparer.InitCompositionItem(this.leftItem, _articleCols, Flag, dataRow3);
          compositionItem4 = ProductionListComparer.InitCompositionItem(this.rightItem, _copyCols, Flag, dataRow4);
        }
        else if (num > 0L)
        {
          compositionItem3 = new CompositionItem(this.leftItem, true, 1);
          this.leftItem.Add(compositionItem3);
          --index4;
          compositionItem4 = ProductionListComparer.InitCompositionItem(this.rightItem, _copyCols, CompositionItemFlags.Added, dataRow4);
        }
        else
        {
          compositionItem3 = ProductionListComparer.InitCompositionItem(this.leftItem, _articleCols, CompositionItemFlags.Removed, dataRow3);
          compositionItem4 = new CompositionItem(this.rightItem, true, 1);
          this.rightItem.Add(compositionItem4);
          --index5;
        }
      }
      compositionItem3.LevelIndex = this.leftItem.Count - 1;
      compositionItem4.LevelIndex = this.rightItem.Count - 1;
      ++index4;
    }
    this.leftItem.Sort((Comparison<CompositionItem>) ((i1, i2) =>
    {
      CompositionItemAttribute compositionItemAttribute1 = i1.Attributes?.Find((Predicate<CompositionItemAttribute>) (x => x.AttributeID == aSortID));
      CompositionItemAttribute compositionItemAttribute2 = i2.Attributes?.Find((Predicate<CompositionItemAttribute>) (x => x.AttributeID == aSortID));
      return (compositionItemAttribute1 == null ? 0L : DataSetProcessor.GetInt64Value(compositionItemAttribute1.Value, 0L)).CompareTo(compositionItemAttribute2 == null ? 0L : DataSetProcessor.GetInt64Value(compositionItemAttribute2.Value, 0L));
    }));
    for (int index6 = 0; index6 < this.leftItem.Count; ++index6)
    {
      CompositionItem compositionItem5 = this.leftItem[index6];
      CompositionItem compositionItem6 = this.rightItem[compositionItem5.LevelIndex];
      compositionItem5.LevelIndex = index6;
      int num = index6;
      compositionItem6.LevelIndex = num;
    }
    this.rightItem.Sort((Comparison<CompositionItem>) ((i1, i2) => i1.LevelIndex.CompareTo(i2.LevelIndex)));
  }

  private void FillColumns(
    List<ColumnDescriptor> list,
    int AttributeID,
    AttributeSourceTypes source)
  {
    if (this.IsComplexAttr(MetaDataHelper.GetAttributeType(AttributeID).FieldType))
    {
      list.Add(new ColumnDescriptor((object) AttributeID, source, ColumnContents.ID, ColumnNameMapping.ID, SortOrders.NONE, 0));
      list.Add(new ColumnDescriptor((object) AttributeID, source, ColumnContents.Text, ColumnNameMapping.Guid, SortOrders.NONE, 0));
    }
    else
      list.Add(new ColumnDescriptor((object) AttributeID, source, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0));
  }

  private bool IsComplexAttr(FieldTypes fieldType)
  {
    return fieldType == FieldTypes.ftObjectLink || fieldType == FieldTypes.ftObjectLinkByID || fieldType == FieldTypes.ftExternalLink;
  }

  private CompositionItemFlags CompareRows(DataRow lr, long lObjectID, DataRow rr, long rObjectID)
  {
    if (lObjectID != rObjectID)
      return CompositionItemFlags.AttributesChanged | CompositionItemFlags.AnotherVersion;
    foreach (Tuple<int, int, AttributableElements> cfgCompareAttr in MRP2PropertyPage.cfg_compareAttrs)
    {
      DataRow row1 = lr;
      int num = cfgCompareAttr.Item1;
      string columnName1 = num.ToString();
      string stringValue1 = DataSetProcessor.GetStringValue(row1, columnName1, "");
      DataRow row2 = rr;
      num = cfgCompareAttr.Item2;
      string columnName2 = num.ToString();
      string stringValue2 = DataSetProcessor.GetStringValue(row2, columnName2, "");
      if (stringValue1 != stringValue2)
        return CompositionItemFlags.AttributesChanged;
    }
    return CompositionItemFlags.Equal;
  }

  private void InitCompositionItem(
    List<ColumnDescriptor> _copyCols,
    IDBObject dbObject,
    CompositionItem item)
  {
    IDBAttribute dbAttribute = (IDBAttribute) null;
    for (int index = 0; index < _copyCols.Count; ++index)
    {
      IDBAttribute byId = dbObject.Attributes.FindByID((int) _copyCols[index].AttributeID);
      if (byId != null && (dbAttribute == null || byId.AttributeID != dbAttribute.AttributeID))
        item.Attributes.Add(new CompositionItemAttribute((int) _copyCols[index].AttributeID, _copyCols[index].AttributeSource, byId.Value, byId.AsString));
      dbAttribute = byId;
    }
  }

  private static CompositionItem InitCompositionItem(
    CompositionItem parent,
    List<ColumnDescriptor> colums,
    CompositionItemFlags Flag,
    DataRow row)
  {
    CompositionItem compositionItem = new CompositionItem(parent);
    compositionItem.CompositionItemFlag = Flag;
    int num = 0;
    for (int index = 0; index < colums.Count; ++index)
    {
      int attributeId = (int) colums[index].AttributeID;
      if (attributeId != num)
        compositionItem.Attributes.Add(new CompositionItemAttribute(attributeId, colums[index].AttributeSource, row[index], (string) null));
      else
        compositionItem.Attributes.Last<CompositionItemAttribute>().Description = DataSetProcessor.GetStringValue(row[index], (string) null);
      num = attributeId;
    }
    parent.Add(compositionItem);
    return compositionItem;
  }

  public List<(CompositionItemAttribute copyAttribute, CompositionItemAttribute art)> CompareAttributes(
    CompositionItem aItem,
    CompositionItem cItem)
  {
    List<(CompositionItemAttribute, CompositionItemAttribute)> valueTupleList = new List<(CompositionItemAttribute, CompositionItemAttribute)>();
    foreach (Tuple<int, int, AttributableElements> cfgCompareAttr in MRP2PropertyPage.cfg_compareAttrs)
    {
      Tuple<int, int, AttributableElements> a = cfgCompareAttr;
      bool flag = false;
      CompositionItemAttribute compositionItemAttribute1 = cItem.Attributes.FirstOrDefault<CompositionItemAttribute>((System.Func<CompositionItemAttribute, bool>) (x => x.AttributeID == a.Item1));
      CompositionItemAttribute compositionItemAttribute2 = aItem.Attributes.FirstOrDefault<CompositionItemAttribute>((System.Func<CompositionItemAttribute, bool>) (x => x.AttributeID == a.Item2));
      if (compositionItemAttribute1 != null && compositionItemAttribute2 != null)
        flag = compositionItemAttribute1.Value.ToString() != compositionItemAttribute2.Value.ToString();
      else if (compositionItemAttribute2 != null)
        flag = true;
      if (flag)
      {
        if (compositionItemAttribute1 == null)
        {
          AttributeSourceTypes sourceType = AttributeSourceTypes.Object;
          if (a.Item3 == AttributableElements.Relation)
            sourceType = AttributeSourceTypes.Relation;
          compositionItemAttribute1 = new CompositionItemAttribute(a.Item1, sourceType, (object) null);
        }
        valueTupleList.Add((compositionItemAttribute1, compositionItemAttribute2));
      }
    }
    return valueTupleList;
  }

  public AttributeValues[] NewAttributesValues()
  {
    return this.NewAttributes.Select<CompositionItemAttribute, AttributeValues>((System.Func<CompositionItemAttribute, AttributeValues>) (x => new AttributeValues(x.AttributeID, x.Value))).ToArray<AttributeValues>();
  }

  public IList<AttributeValues> CompositionAttributeValues(
    CompositionItem item,
    AttributeSourceTypes src)
  {
    return (IList<AttributeValues>) item.Attributes.Where<CompositionItemAttribute>((System.Func<CompositionItemAttribute, bool>) (x => x.SourceType == src)).Join<CompositionItemAttribute, (int, int), int, AttributeValues>(src != AttributeSourceTypes.Object ? (IEnumerable<(int, int)>) MRP2PropertyPage.cfg_SostavAttributes : (IEnumerable<(int, int)>) MRP2PropertyPage.cfg_ObjectAttributes, (System.Func<CompositionItemAttribute, int>) (pl => pl.AttributeID), (System.Func<(int, int), int>) (l => l.CopyAttributeID), (Func<CompositionItemAttribute, (int, int), AttributeValues>) ((pl, l) => new AttributeValues(l.CopyAttributeID, pl.Value))).ToList<AttributeValues>();
  }
}
