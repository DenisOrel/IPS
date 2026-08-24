// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.LevelCompositionReader
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class LevelCompositionReader
{
  private readonly ICompareTreeSettingsService _settingsService;
  private readonly Guid _ruleID;

  public LevelCompositionReader(
    CompositionFiltrationSettings filtration,
    Guid ruleID,
    ICompareTreeSettingsService settingsService)
  {
    this.Filtration = filtration;
    this._settingsService = settingsService;
    this._ruleID = ruleID;
  }

  public CompositionFiltrationSettings Filtration { get; }

  private DataRowCollection Sort(
    int parentTypeID,
    DataTable levelTable,
    List<ColumnDescriptor> columns)
  {
    List<Tuple<int, AttributeSourceTypes>> sortedAttributes = this._settingsService.GetSortedAttributes(this._ruleID, parentTypeID);
    if (levelTable.Rows.Count <= 1)
      return levelTable.Rows;
    StringBuilder sortString = new StringBuilder();
    List<string> needDeleted = new List<string>();
    foreach (Tuple<int, AttributeSourceTypes> tuple in sortedAttributes)
    {
      Tuple<int, AttributeSourceTypes> sortedAttribute = tuple;
      IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(sortedAttribute.Item1);
      int index = columns.FindIndex((Predicate<ColumnDescriptor>) (x => x.AttributeID.Equals((object) sortedAttribute.Item1) && x.AttributeSource.Equals((object) sortedAttribute.Item2)));
      if (attributeType.FieldType == FieldTypes.ftMeasured)
        DataTableSortHelper.GetMeasuredColumnFilter(levelTable, sortString, index.ToString(), needDeleted, SortOrders.ASC);
      else if (attributeType.AttributeGuid.Equals(new Guid("cad00270-306c-11d8-b4e9-00304f19f545")))
        DataTableSortHelper.GetPositionColumnFilter(levelTable, sortString, index.ToString(), needDeleted, SortOrders.ASC);
      else
        sortString.Append($"[{index}] ASC,");
    }
    int index1 = columns.FindIndex((Predicate<ColumnDescriptor>) (x => x.AttributeID.Equals((object) -23) && x.AttributeSource.Equals((object) AttributeSourceTypes.Relation)));
    if (index1 >= 0 && !sortedAttributes.Exists((Predicate<Tuple<int, AttributeSourceTypes>>) (x => x.Item1.Equals(-23) && x.Item2.Equals((object) AttributeSourceTypes.Relation))))
      sortString.Append($"[{index1}] ASC,");
    int index2 = columns.FindIndex((Predicate<ColumnDescriptor>) (x => x.AttributeID.Equals((object) -7) && x.AttributeSource.Equals((object) AttributeSourceTypes.Object)));
    if (index2 >= 0 && !sortedAttributes.Exists((Predicate<Tuple<int, AttributeSourceTypes>>) (x => x.Item1.Equals(-7) && x.Item2.Equals((object) AttributeSourceTypes.Object))))
      sortString.Append($"[{index2}] ASC,");
    sortString.Remove(sortString.Length - 1, 1);
    DataRow[] fromRows = levelTable.Select(string.Empty, sortString.ToString());
    DataTable toTable = levelTable.Clone();
    DataSetProcessor.AssignRows(toTable, (IEnumerable<DataRow>) fromRows);
    foreach (string name in needDeleted)
      toTable.Columns.Remove(name);
    toTable.AcceptChanges();
    return toTable.Rows;
  }

  private void ReadRootObjectAttributes(
    IUserSession session,
    CompositionItem parent,
    List<ColumnDescriptor> columns)
  {
    AttributeValues[] array = (AttributeValues[]) null;
    foreach (ColumnDescriptor columnDescriptor in columns.FindAll((Predicate<ColumnDescriptor>) (x => x.AttributeSource == AttributeSourceTypes.Object)))
    {
      ColumnDescriptor column = columnDescriptor;
      if (!parent.Attributes.Exists((Predicate<CompositionItemAttribute>) (y => y.AttributeID == (int) column.AttributeID)))
      {
        if (array == null)
          array = session.GetObject(parent.ObjectID).GetAttributesValues(GetAttributeValuesModes.IncludeBlobs | GetAttributeValuesModes.IncludeObligatoryAttributes);
        AttributeValues attributeValues = Array.Find<AttributeValues>(array, (Predicate<AttributeValues>) (z => z.AttributeID == (int) column.AttributeID));
        if (attributeValues != null)
        {
          MetaDataHelper.GetAttributeType((int) column.AttributeID);
          parent.AddAttribute(new CompositionItemAttribute((int) column.AttributeID, AttributeSourceTypes.Object, attributeValues.Value, attributeValues.AsString));
        }
      }
    }
  }

  public void Read(IUserSession session, CompositionItem parent, bool recursive)
  {
    this.Read(session, parent, recursive, true);
  }

  private void Read(IUserSession session, CompositionItem parent, bool recursive, bool firstLevel)
  {
    List<int> relationTypeIDs;
    List<int> objectTypeIDs;
    List<ColumnDescriptor> columns;
    QueryHelper.GetSettings4Query(this._ruleID, parent.ObjectTypeID, out relationTypeIDs, out objectTypeIDs, out columns);
    if (firstLevel)
      this.ReadRootObjectAttributes(session, parent, columns);
    DataTable levelTable = ServiceUtils.GetService<ICompositionLoadService>((object) session, true).LoadComposition((object) session.SessionGUID, parent.ObjectID, parent.ObjectTypeID, (IEnumerable<int>) relationTypeIDs, (IEnumerable<int>) objectTypeIDs, (IEnumerable<ColumnDescriptor>) columns, true, false, this.Filtration.VersionsRule, (IEnumerable<ConditionStructure>) null, string.Empty, this.Filtration.Tag, 1, (IEnumerable<int>) null);
    parent.Clear();
    if (levelTable == null)
      return;
    DataRowCollection dataRowCollection = this.Sort(parent.ObjectTypeID, levelTable, columns);
    if (dataRowCollection.Count <= 0)
      return;
    foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
    {
      CompositionItem parent1 = new CompositionItem(parent);
      for (int index = 0; index < columns.Count; ++index)
        parent1.AddAttribute(new CompositionItemAttribute((int) columns[index].AttributeID, columns[index].AttributeSource, dataRow[index], (string) null));
      parent.Add(parent1);
      if (recursive && !this.LinkInCompositionPresent(parent1.PrjLinkID, parent))
        this.Read(session, parent1, recursive, false);
    }
  }

  public bool LinkInCompositionPresent(long prjLinkID, CompositionItem parent)
  {
    if (parent == null)
      return false;
    return parent.PrjLinkID == prjLinkID || this.LinkInCompositionPresent(prjLinkID, parent.Parent);
  }
}
