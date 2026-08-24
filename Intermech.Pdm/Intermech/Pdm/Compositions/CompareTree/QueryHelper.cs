// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.QueryHelper
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal static class QueryHelper
{
  public static void GetSettings4Query(
    Guid ruleID,
    int parentTypeID,
    out List<int> relationTypeIDs,
    out List<int> objectTypeIDs,
    out List<ColumnDescriptor> columns)
  {
    ICompareTreeSettingsService service = (ICompareTreeSettingsService) ServicesManager.GetService(typeof (ICompareTreeSettingsService));
    columns = new List<ColumnDescriptor>();
    columns.AddRange((IEnumerable<ColumnDescriptor>) QueryHelper.ObligatoryColumns);
    relationTypeIDs = service.GetRelationTypes(ruleID, parentTypeID);
    objectTypeIDs = new List<int>();
    foreach (int relationTypeID in relationTypeIDs)
    {
      foreach (int childobjectTypeId in service.GetChildobjectTypeIDs(ruleID, parentTypeID, relationTypeID))
      {
        if (!objectTypeIDs.Contains(childobjectTypeId))
        {
          objectTypeIDs.Add(childobjectTypeId);
          List<int> objectAttributes = service.GetIDObjectAttributes(ruleID, childobjectTypeId);
          if (objectAttributes != null && objectAttributes.Count > 0)
          {
            foreach (int attributeID in objectAttributes)
              QueryHelper.AddColumn(new ColumnDescriptor((object) attributeID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0), columns);
          }
          QueryHelper.AddAttributes(service.GetObjectCompareAttributes(ruleID, childobjectTypeId), AttributeSourceTypes.Object, columns);
          QueryHelper.AddAttributes(service.GetRelationCompareAttributes(ruleID, relationTypeID), AttributeSourceTypes.Relation, columns);
        }
      }
      List<int> relationAttributes = service.GetIDRelationAttributes(ruleID, parentTypeID, relationTypeID);
      if (relationAttributes != null && relationAttributes.Count > 0)
      {
        foreach (int attributeID in relationAttributes)
          QueryHelper.AddColumn(new ColumnDescriptor((object) attributeID, AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0), columns);
      }
    }
    List<Tuple<int, AttributeSourceTypes>> sortedAttributes = service.GetSortedAttributes(ruleID, parentTypeID);
    if (sortedAttributes == null || sortedAttributes.Count <= 0)
      return;
    foreach (Tuple<int, AttributeSourceTypes> tuple in sortedAttributes)
      QueryHelper.AddColumn(new ColumnDescriptor((object) tuple.Item1, tuple.Item2, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0), columns);
  }

  private static bool FilterAttribute(IMSAttributeType attribute)
  {
    return attribute.MultiValueMode != MultiValueModes.MultiValues && attribute.MultiValueMode != MultiValueModes.MultiValuesFromList && attribute.FieldType != FieldTypes.ftMemo && attribute.FieldType != FieldTypes.ftBlob && attribute.FieldType != FieldTypes.ftFile && attribute.FieldType != FieldTypes.ftShortBlob && attribute.FieldType != FieldTypes.ftPassword;
  }

  private static void AddAttributes(
    List<int> compareAttributes,
    AttributeSourceTypes sourceType,
    List<ColumnDescriptor> columns)
  {
    if (compareAttributes == null || compareAttributes.Count == 0)
      return;
    foreach (int compareAttribute in compareAttributes)
    {
      IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(compareAttribute);
      if (QueryHelper.FilterAttribute(attributeType))
      {
        ColumnContents contents = ColumnContents.Text;
        if (attributeType.RealFieldType == FieldTypes.ftObjectLink)
          contents = ColumnContents.ID;
        QueryHelper.AddColumn(new ColumnDescriptor((object) compareAttribute, sourceType, contents, ColumnNameMapping.Index, SortOrders.NONE, 0), columns);
      }
    }
  }

  private static List<ColumnDescriptor> ObligatoryColumns
  {
    get
    {
      return new List<ColumnDescriptor>()
      {
        new ColumnDescriptor((object) -2, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
        new ColumnDescriptor((object) -3, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
        new ColumnDescriptor((object) -7, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.ASC, 1),
        new ColumnDescriptor((object) -5, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
        new ColumnDescriptor((object) -8, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
        new ColumnDescriptor((object) -6, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
        new ColumnDescriptor((object) -9, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
        new ColumnDescriptor((object) -14, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
        new ColumnDescriptor((object) -20, AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
        new ColumnDescriptor((object) -23, AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.ASC, 0),
        new ColumnDescriptor((object) -26, AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
        new ColumnDescriptor((object) -50, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
        new ColumnDescriptor((object) -16, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
        new ColumnDescriptor((object) -17, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
        new ColumnDescriptor((object) -15, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0)
      };
    }
  }

  private static void AddColumn(ColumnDescriptor column, List<ColumnDescriptor> columns)
  {
    int index = columns.FindIndex((Predicate<ColumnDescriptor>) (x => x.AttributeID.Equals(column.AttributeID) && x.AttributeSource.Equals((object) column.AttributeSource)));
    if (index < 0)
    {
      columns.Add(column);
    }
    else
    {
      if (column.Sort == SortOrders.NONE)
        return;
      columns[index] = column;
    }
  }
}
