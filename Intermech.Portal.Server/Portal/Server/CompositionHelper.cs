// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.CompositionHelper
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Server;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

#nullable disable
namespace Intermech.Portal.Server;

internal static class CompositionHelper
{
  private static readonly int attributeLinkedID = MetaDataHelper.GetAttributeTypeID(PortalConsts.attributeLinkedGuid);
  private static readonly int attributeObjectTypeNameID = MetaDataHelper.GetAttributeTypeID("cad014cf-306c-11d8-b4e9-00304f19f545");
  private static readonly int attributeObjectTypeGuidID = MetaDataHelper.GetAttributeTypeID("cad001a0-306c-11d8-b4e9-00304f19f545");
  private static readonly int objtypePublishObjectsID = MetaDataHelper.GetObjectTypeID(PortalConsts.objtypePublishObjects);

  public static DataTable GetCompositionTable(
    IUserSession session,
    long rootObjectID,
    string[] filteredTypes,
    DBQueryParams queryParams,
    int countLevels)
  {
    int addedColumnsCount = 0;
    IDBObject dbObject = session.GetObject(rootObjectID, true);
    IDBAttribute attributeByGuid = dbObject.GetAttributeByGuid(PortalConsts.attributeLinkedGuid);
    IDBAttribute attributeById1 = dbObject.GetAttributeByID(MetaDataHelper.GetAttributeTypeID("cad001a0-306c-11d8-b4e9-00304f19f545"));
    IDBAttribute attributeById2 = dbObject.GetAttributeByID(MetaDataHelper.GetAttributeTypeID("cad014cf-306c-11d8-b4e9-00304f19f545"));
    List<CompositionObjectInfo> rootObjects = new List<CompositionObjectInfo>()
    {
      new CompositionObjectInfo(dbObject.ObjectID, dbObject.ID, attributeById2 != null ? attributeById2.AsString : string.Empty, attributeById1 != null ? attributeById1.AsString : string.Empty, attributeByGuid != null ? attributeByGuid.AsString : string.Empty, rootObjectID)
    };
    ConditionStructure[] onEnabledObjects = ActionsHelper.GetConditionOnEnabledObjects(session);
    queryParams.Conditions = queryParams.Conditions == null || queryParams.Conditions.Length == 0 ? onEnabledObjects : ConditionStructure.Join(onEnabledObjects, queryParams.Conditions);
    IDBRelationCollection relationCollection = session.GetRelationCollection(MetaDataHelper.GetRelationTypeID(PortalConsts.reltypePublish));
    List<long> relations = new List<long>();
    DataTable tableComposition = CompositionHelper.GetLevelTableComposition(session, relationCollection, CompositionHelper.AddSpecialColumns(queryParams, ref addedColumnsCount), filteredTypes, rootObjects, relations, 1, countLevels);
    for (int index = 0; index < addedColumnsCount; ++index)
      tableComposition.Columns.RemoveAt(0);
    if (queryParams.ColumnNames[0] == ColumnNameMapping.Index)
    {
      for (int index = 0; index < tableComposition.Columns.Count; ++index)
        tableComposition.Columns[index].ColumnName = index.ToString();
    }
    return tableComposition;
  }

  private static ConditionStructure GetFilterTypesCondition(string[] filteredTypes)
  {
    if (filteredTypes == null || filteredTypes.Length == 0)
      return ConditionStructure.Empty;
    Guid attributeGuid = GuidHelper.IsGuid(filteredTypes[0]) ? new Guid("cad001a0-306c-11d8-b4e9-00304f19f545") : PortalConsts.attributeObjTypeName;
    return filteredTypes.Length != 1 ? new ConditionStructure(attributeGuid, RelationalOperators.In, (object) filteredTypes, LogicalOperators.AND, 0) : new ConditionStructure(attributeGuid, RelationalOperators.Equal, (object) filteredTypes[0], LogicalOperators.AND, 0);
  }

  private static DataTable GetLevelTableComposition(
    IUserSession session,
    IDBRelationCollection relCollection,
    DBRecordSetParams dbParams,
    string[] filteredTypes,
    List<CompositionObjectInfo> rootObjects,
    List<long> relations,
    int level,
    int countLevels)
  {
    List<long> longList = rootObjects.ConvertAll<long>((Converter<CompositionObjectInfo, long>) (x => x.ObjectID));
    dbParams.Conditions = ConditionStructure.Join(longList.Count == 1 ? new ConditionStructure(-21, RelationalOperators.Equal, (object) longList[0], LogicalOperators.AND, 0, true) : new ConditionStructure(-21, RelationalOperators.In, (object) longList.ToArray(), LogicalOperators.AND, 0, true), dbParams.Conditions);
    ConditionStructure filterTypesCondition = CompositionHelper.GetFilterTypesCondition(filteredTypes);
    if (!filterTypesCondition.Equals((object) ConditionStructure.Empty))
      dbParams.Conditions = ConditionStructure.Join(filterTypesCondition, dbParams.Conditions);
    DataTable tableComposition1 = relCollection.Select(dbParams);
    if (tableComposition1.Rows.Count == 0)
      return tableComposition1;
    DataTable toTable = tableComposition1.Clone();
    List<CompositionObjectInfo> rootObjects1 = new List<CompositionObjectInfo>();
    List<int> intList = new List<int>();
    List<ColumnDescriptor> columnDescriptorList = new List<ColumnDescriptor>();
    for (int index = 0; index < dbParams.ColumnsInfo.Length; ++index)
    {
      if (dbParams.ColumnsInfo[index].AttributeSource == AttributeSourceTypes.Object)
      {
        intList.Add(index);
        columnDescriptorList.Add(new ColumnDescriptor(dbParams.Columns[index], dbParams.Contents != null ? dbParams.Contents[index] : ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0));
      }
    }
    foreach (DataRow row1 in (InternalDataCollectionBase) tableComposition1.Rows)
    {
      Convert.ToInt64(row1[6]);
      long int64_1 = Convert.ToInt64(row1[0]);
      long objectID = 0;
      if (CompareValuesHelper.NormalizedValue(row1[5]) != null)
        objectID = Convert.ToInt64(row1[5]);
      if (objectID != 0L)
      {
        if (!relations.Contains(int64_1))
          rootObjects1.Add(new CompositionObjectInfo(objectID, Convert.ToInt64(row1[7]), Convert.ToString(row1[10]), Convert.ToString(row1[9]), Convert.ToString(row1[8]), Convert.ToInt64(row1[4])));
        DataSetProcessor.AddRow(toTable, row1, false);
      }
      else
      {
        long int64_2 = Convert.ToInt64(row1[7]);
        IDbManager dataManager = (session as UserSession).DataManager;
        DataTable dataTable = session.GetObjectCollection(PortalConsts.objtypePublishObjects).Select(new DBRecordSetParams(new ConditionStructure[1]
        {
          new ConditionStructure(-3, RelationalOperators.Equal, (object) int64_2, LogicalOperators.NONE, 0, false)
        }, columnDescriptorList.ToArray()));
        for (int index = 0; index < dataTable.Rows.Count; ++index)
        {
          DataRow row2 = toTable.NewRow();
          for (int columnIndex = 0; columnIndex < toTable.Columns.Count; ++columnIndex)
            row2[columnIndex] = !intList.Contains(columnIndex) ? row1[columnIndex] : dataTable.Rows[index][intList.IndexOf(columnIndex)];
          toTable.Rows.Add(row2);
          if (!relations.Contains(int64_1))
            rootObjects1.Add(new CompositionObjectInfo(Convert.ToInt64(row2[6]), Convert.ToInt64(row2[7]), Convert.ToString(row2[10]), Convert.ToString(row2[9]), Convert.ToString(row2[8]), Convert.ToInt64(row1[4])));
        }
      }
      if (!relations.Contains(int64_1))
        relations.Add(int64_1);
    }
    if (rootObjects1.Count > 0 && (level < countLevels || countLevels == -1))
    {
      DataTable tableComposition2 = CompositionHelper.GetLevelTableComposition(session, relCollection, dbParams, filteredTypes, rootObjects1, relations, level + 1, countLevels);
      if (tableComposition2.Rows.Count > 0)
        DataSetProcessor.AddTable(toTable, tableComposition2, false);
    }
    toTable.AcceptChanges();
    return toTable;
  }

  public static void GetComposition(
    IUserSession session,
    long[] rootObjectIDs,
    string[] filteredTypes,
    List<Tuple<long, bool>> objects,
    List<Tuple<Guid, long>> relations,
    int countLevels)
  {
    List<CompositionObjectInfo> rootObjects = new List<CompositionObjectInfo>();
    StringBuilder stringBuilder = (StringBuilder) null;
    if (TraceLog.Enabled)
      stringBuilder = new StringBuilder();
    for (int index = 0; index < rootObjectIDs.Length; ++index)
    {
      IDBObject dbObject = session.GetObject(rootObjectIDs[index], true);
      IDBAttribute attributeByGuid = dbObject.GetAttributeByGuid(PortalConsts.attributeLinkedGuid);
      string linkedGuid = attributeByGuid != null ? attributeByGuid.AsString : string.Empty;
      IDBAttribute attributeById1 = dbObject.GetAttributeByID(MetaDataHelper.GetAttributeTypeID("cad001a0-306c-11d8-b4e9-00304f19f545"));
      string str1 = attributeById1 != null ? attributeById1.AsString : string.Empty;
      IDBAttribute attributeById2 = dbObject.GetAttributeByID(MetaDataHelper.GetAttributeTypeID("cad014cf-306c-11d8-b4e9-00304f19f545"));
      string str2 = attributeById2 != null ? attributeById2.AsString : string.Empty;
      if (CompositionHelper.IsEnableObjectType(filteredTypes, str1, str2))
      {
        rootObjects.Add(new CompositionObjectInfo(dbObject.ObjectID, dbObject.ID, str2, str1, linkedGuid));
        if (TraceLog.Enabled)
        {
          if (index > 0)
            stringBuilder.Append(';');
          stringBuilder.Append(dbObject.ObjectID);
        }
      }
    }
    if (TraceLog.Enabled)
      TraceLog.Write($"...start GetComposition for {stringBuilder.ToString()}");
    IDBRelationCollection relationCollection = session.GetRelationCollection(MetaDataHelper.GetRelationTypeID(PortalConsts.reltypePublish));
    List<CompositionObjectInfo> levelLinkedObjects = CompositionHelper.GetLevelLinkedObjects(session, rootObjects, filteredTypes);
    foreach (CompositionObjectInfo compositionObjectInfo in levelLinkedObjects)
      objects.Add(new Tuple<long, bool>(compositionObjectInfo.ObjectID, countLevels != 0));
    CompositionHelper.GetLevelComposition(session, relationCollection, levelLinkedObjects, objects, relations, filteredTypes, 1, countLevels);
    if (!TraceLog.Enabled)
      return;
    TraceLog.Write("...end GetComposition");
  }

  private static DBRecordSetParams AddSpecialColumns(
    DBQueryParams dbParams,
    ref int addedColumnsCount)
  {
    List<ColumnDescriptor> columnDescriptorList = new List<ColumnDescriptor>((IEnumerable<ColumnDescriptor>) QueryColumnsHelper.RelationsColumns);
    addedColumnsCount = columnDescriptorList.Count;
    for (int index = 0; index < dbParams.Columns.Length; ++index)
    {
      object column = dbParams.Columns[index];
      AttributeSourceTypes attributeSource = AttributeSourceTypes.Auto;
      if (dbParams.ColumnsInfo != null && dbParams.ColumnsInfo.Length != 0)
        attributeSource = dbParams.ColumnsInfo[index].AttributeSource;
      SortOrders sort = SortOrders.NONE;
      int orderByID = 0;
      if (dbParams.SortColumns != null && dbParams.SortColumns.Length != 0)
      {
        orderByID = Array.IndexOf<object>(dbParams.SortColumns, column);
        if (orderByID >= 0)
          sort = dbParams.Orders[orderByID];
        else
          orderByID = 0;
      }
      ColumnContents contents = ColumnContents.Text;
      if (dbParams.Contents != null)
        contents = dbParams.Contents[index];
      columnDescriptorList.Add(new ColumnDescriptor(column, attributeSource, contents, dbParams.ColumnNames[index], sort, orderByID));
    }
    DBRecordSetParams dbRecordSetParams = new DBRecordSetParams(dbParams.Conditions, columnDescriptorList.ToArray(), dbParams.LastKeyValue, recordCount: dbParams.RecordCount);
    if (dbParams.LastOrderValue != null && dbParams.LastOrderValue is object[])
      dbRecordSetParams.LastOrderValue = (object) new List<object>((IEnumerable<object>) (object[]) dbParams.LastOrderValue);
    dbRecordSetParams.FailIfNotFound = dbParams.FailIfNotFound;
    dbRecordSetParams.TableName = dbParams.TableName;
    return dbRecordSetParams;
  }

  private static void GetLevelComposition(
    IUserSession session,
    IDBRelationCollection relCollection,
    List<CompositionObjectInfo> rootObjects,
    List<Tuple<long, bool>> objects,
    List<Tuple<Guid, long>> relations,
    string[] filteredTypes,
    int level,
    int countLevels)
  {
    List<long> longList = rootObjects.ConvertAll<long>((Converter<CompositionObjectInfo, long>) (x => x.ObjectID));
    DBRecordSetParams paramSet = new DBRecordSetParams((ConditionStructure[]) null, QueryColumnsHelper.RelationsColumns)
    {
      Conditions = ActionsHelper.GetConditionOnEnabledObjects(session)
    };
    ConditionStructure joinedCondition = longList.Count == 1 ? new ConditionStructure(-21, RelationalOperators.Equal, (object) longList[0], LogicalOperators.NONE, 0, true) : new ConditionStructure(-21, RelationalOperators.In, (object) longList.ToArray(), LogicalOperators.NONE, 0, true);
    paramSet.Conditions = ConditionStructure.Join(joinedCondition, paramSet.Conditions);
    ConditionStructure filterTypesCondition = CompositionHelper.GetFilterTypesCondition(filteredTypes);
    if (!filterTypesCondition.Equals((object) ConditionStructure.Empty))
      paramSet.Conditions = ConditionStructure.Join(filterTypesCondition, paramSet.Conditions);
    DataTable dataTable = relCollection.Select(paramSet);
    if (dataTable.Rows.Count == 0)
      return;
    List<CompositionObjectInfo> rootObjects1 = new List<CompositionObjectInfo>();
    List<Tuple<Guid, long, List<CompositionObjectInfo>>> tupleList = new List<Tuple<Guid, long, List<CompositionObjectInfo>>>();
    foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
    {
      Guid relationGuid = new Guid(Convert.ToString(row[1]));
      long projObjectID = Convert.ToInt64(row[4]);
      if (!relations.Exists((Predicate<Tuple<Guid, long>>) (x => x.Item1.Equals(relationGuid) && x.Item2 == projObjectID)))
      {
        List<CompositionObjectInfo> partObjectIds = CompositionHelper.GetPartObjectIDs(session, row, projObjectID);
        tupleList.Add(new Tuple<Guid, long, List<CompositionObjectInfo>>(relationGuid, projObjectID, partObjectIds));
        foreach (CompositionObjectInfo compositionObjectInfo in partObjectIds)
        {
          CompositionObjectInfo part = compositionObjectInfo;
          if (!rootObjects1.Exists((Predicate<CompositionObjectInfo>) (x => x.ObjectID == part.ObjectID)))
            rootObjects1.Add(part);
        }
      }
    }
    if (rootObjects1.Count <= 0 || level > countLevels && countLevels != -1)
      return;
    List<CompositionObjectInfo> levelLinkedObjects = CompositionHelper.GetLevelLinkedObjects(session, rootObjects1, filteredTypes);
    foreach (CompositionObjectInfo compositionObjectInfo in levelLinkedObjects)
      CompositionHelper.AddObject(compositionObjectInfo.ObjectID, level < countLevels || countLevels == -1, objects);
    foreach (Tuple<Guid, long, List<CompositionObjectInfo>> tuple in tupleList)
    {
      List<CompositionObjectInfo> compositionObjectInfoList = tuple.Item3;
      if (compositionObjectInfoList.Count > 0)
      {
        bool flag = false;
        foreach (CompositionObjectInfo compositionObjectInfo in compositionObjectInfoList)
        {
          CompositionObjectInfo partInfo = compositionObjectInfo;
          if (objects.Exists((Predicate<Tuple<long, bool>>) (_ => _.Item1.Equals(partInfo.ObjectID))))
          {
            flag = true;
            break;
          }
        }
        if (flag)
          relations.Add(new Tuple<Guid, long>(tuple.Item1, tuple.Item2));
      }
    }
    CompositionHelper.GetLevelComposition(session, relCollection, levelLinkedObjects, objects, relations, filteredTypes, level + 1, countLevels);
  }

  private static List<CompositionObjectInfo> GetPartObjectIDs(
    IUserSession session,
    DataRow row,
    long projObjectID)
  {
    List<CompositionObjectInfo> partObjectIds = new List<CompositionObjectInfo>();
    long int64_1 = Convert.ToInt64(row[7]);
    long objectID = 0;
    if (CompareValuesHelper.NormalizedValue(row[5]) != null)
      objectID = Convert.ToInt64(row[5]);
    if (objectID != 0L)
    {
      if (TraceLog.Enabled)
        TraceLog.Write($"... concrete={objectID}");
      partObjectIds.Add(new CompositionObjectInfo(objectID, int64_1, Convert.ToString(row[10]), Convert.ToString(row[9]), Convert.ToString(row[8]), projObjectID));
      if (TraceLog.Enabled)
        TraceLog.Write($"... add version={objectID}");
    }
    else
    {
      StringBuilder stringBuilder = (StringBuilder) null;
      if (TraceLog.Enabled)
        stringBuilder = new StringBuilder();
      IDbManager dataManager = (session as UserSession).DataManager;
      DataTable dataTable = dataManager.ExecuteDataTable($"SELECT F_OBJECT_ID, F_ID,  F{CompositionHelper.attributeObjectTypeNameID}, F{CompositionHelper.attributeObjectTypeGuidID}, F{CompositionHelper.attributeLinkedID} FROM IMV_O{CompositionHelper.objtypePublishObjectsID} WHERE F_ID = :f_id AND F_LEVEL_ID <>:f_del_level", dataManager.Parameter("f_id", (object) Convert.ToInt64(row[7])), dataManager.Parameter("f_del_level", (object) session.IdentHelper.DeletedID));
      for (int index = 0; index < dataTable.Rows.Count; ++index)
      {
        long int64_2 = Convert.ToInt64(dataTable.Rows[index][0]);
        partObjectIds.Add(new CompositionObjectInfo(int64_2, Convert.ToInt64(dataTable.Rows[index][1]), Convert.ToString(dataTable.Rows[index][2]), Convert.ToString(dataTable.Rows[index][3]), Convert.ToString(dataTable.Rows[index][4]), Convert.ToInt64(row[4])));
        if (TraceLog.Enabled)
          TraceLog.Write($"... add={int64_2}");
        if (TraceLog.Enabled)
        {
          if (index > 0)
            stringBuilder.Append(';');
          stringBuilder.Append(int64_2);
        }
      }
      if (TraceLog.Enabled)
        TraceLog.Write($"... all versions ({dataTable.Rows.Count}): {stringBuilder.ToString()}");
    }
    return partObjectIds;
  }

  private static bool IsEnableObjectType(string[] filteredTypes, string guid, string name)
  {
    if (filteredTypes != null && filteredTypes.Length != 0)
    {
      if (GuidHelper.IsGuid(filteredTypes[0]))
      {
        if (!string.IsNullOrEmpty(guid) && Array.IndexOf<string>(filteredTypes, guid) >= 0)
          return false;
      }
      else if (!string.IsNullOrEmpty(name) && Array.IndexOf<string>(filteredTypes, name) >= 0)
        return false;
    }
    return true;
  }

  private static List<CompositionObjectInfo> GetLevelLinkedObjects(
    IUserSession session,
    List<CompositionObjectInfo> rootObjects,
    string[] filteredTypes)
  {
    List<CompositionObjectInfo> levelLinkedObjects = new List<CompositionObjectInfo>();
    foreach (CompositionObjectInfo rootObject in rootObjects)
    {
      foreach (CompositionObjectInfo linkedObject in CompositionHelper.GetLinkedObjects(session, rootObject))
      {
        CompositionObjectInfo item = linkedObject;
        if (CompositionHelper.IsEnableObjectType(filteredTypes, item.ObjectTypeGuid, item.ObjectTypeName))
        {
          CompositionObjectInfo compositionObjectInfo = levelLinkedObjects.Find((Predicate<CompositionObjectInfo>) (x => x.ObjectID == item.ObjectID));
          if (compositionObjectInfo == null)
            levelLinkedObjects.Add(item);
          else if (compositionObjectInfo.ProjID != 0L && item.ProjID == 0L)
            compositionObjectInfo.ClearProjID();
        }
      }
    }
    return levelLinkedObjects;
  }

  private static List<CompositionObjectInfo> GetLinkedObjects(
    IUserSession session,
    CompositionObjectInfo rootObjectInfo)
  {
    List<CompositionObjectInfo> linkedObjects = new List<CompositionObjectInfo>()
    {
      rootObjectInfo
    };
    if (rootObjectInfo.LinkedGuid != string.Empty)
    {
      foreach (DataRow row in (InternalDataCollectionBase) session.GetObjectCollection(PortalConsts.objtypePublishObjects).Select(new DBRecordSetParams(ConditionStructure.Join(new ConditionStructure[2]
      {
        new ConditionStructure(PortalConsts.attributeLinkedGuid, RelationalOperators.Equal, (object) rootObjectInfo.LinkedGuid, LogicalOperators.AND, 0),
        new ConditionStructure(-2, RelationalOperators.NotEqual, (object) rootObjectInfo.ObjectID, LogicalOperators.AND, 0, false)
      }, ActionsHelper.GetConditionOnEnabledObjects(session)), QueryColumnsHelper.VersionsColumns)).Rows)
      {
        long linkedObjectID = Convert.ToInt64(row[0]);
        if (!linkedObjects.Exists((Predicate<CompositionObjectInfo>) (x => x.ObjectID == linkedObjectID)))
          linkedObjects.Add(new CompositionObjectInfo(linkedObjectID, Convert.ToInt64(row[1]), Convert.ToString(row[4]), Convert.ToString(row[3]), Convert.ToString(row[2])));
      }
    }
    return linkedObjects;
  }

  public static long[] GetLinkedObjectsArray(IUserSession session, long[] objects)
  {
    int attributeTypeId = MetaDataHelper.GetAttributeTypeID(PortalConsts.attributeLinkedGuid);
    IDBObjectCollection objectCollection = session.GetObjectCollection(PortalConsts.objtypePublishObjects);
    DataTable dataTable = objectCollection.Select(new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(-2, RelationalOperators.In, (object) objects, LogicalOperators.AND, 0, false)
    }, new object[1]{ (object) attributeTypeId }, 0L, (object) null, -1));
    List<Guid> guidList = new List<Guid>();
    foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
    {
      string str = Convert.ToString(row[0]);
      if (!string.IsNullOrEmpty(str) && GuidHelper.IsGuid(str))
      {
        Guid guid = new Guid(str);
        if (!guidList.Contains(guid))
          guidList.Add(guid);
      }
    }
    if (guidList.Count == 0)
      return objects;
    List<long> longList = new List<long>();
    foreach (DataRow row in (InternalDataCollectionBase) objectCollection.Select(new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(attributeTypeId, RelationalOperators.In, (object) guidList.ToArray(), LogicalOperators.AND, 0, false)
    }, new object[1]{ (object) -2 }, 0L, (object) null, -1)).Rows)
      longList.Add(Convert.ToInt64(row[0]));
    foreach (long num in objects)
    {
      if (!longList.Contains(num))
        longList.Add(num);
    }
    return longList.ToArray();
  }

  private static DataTable GetChildVersions(
    IDBObjectCollection objectsCollection,
    ColumnDescriptor[] columns,
    long parentVersionID)
  {
    DataTable toTable = objectsCollection.Select(new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(0, RelationalOperators.ParentVersionID, (object) parentVersionID, LogicalOperators.AND, 0, false)
    }, columns));
    if (toTable.Rows.Count > 0)
    {
      for (int index = 0; index < toTable.Rows.Count; ++index)
      {
        DataTable childVersions = CompositionHelper.GetChildVersions(objectsCollection, columns, Convert.ToInt64(toTable.Rows[index][0]));
        if (childVersions.Rows.Count > 0)
          DataSetProcessor.AddTable(toTable, childVersions, true);
      }
    }
    return toTable;
  }

  private static void AddObject(
    long objectID,
    bool withComposition,
    List<Tuple<long, bool>> objects)
  {
    if (objects.Exists((Predicate<Tuple<long, bool>>) (_ => _.Item1.Equals(objectID))))
      return;
    objects.Add(new Tuple<long, bool>(objectID, withComposition));
  }
}
