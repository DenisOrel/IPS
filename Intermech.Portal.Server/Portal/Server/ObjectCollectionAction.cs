// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.ObjectCollectionAction
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Portal;
using Intermech.Interfaces.Server;
using Intermech.Interfaces.Server.WebPortal;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel;
using Intermech.Kernel.Search;
using Intermech.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Intermech.Portal.Server;

internal sealed class ObjectCollectionAction : PortalAction
{
  private string GetValueToTraceLog(object value)
  {
    if (value == null)
      return "NULL";
    if (!(value is IList list))
      return value.ToString();
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("[");
    for (int index = 0; index < list.Count; ++index)
    {
      if (index > 0)
        stringBuilder.Append(";");
      stringBuilder.Append(list[index]);
    }
    stringBuilder.Append("]");
    return stringBuilder.ToString();
  }

  private string GetAttributeToTraceLog(object id)
  {
    string attributeTypeName;
    switch (id)
    {
      case int attrTypeID:
        attributeTypeName = MetaDataHelper.GetAttributeTypeName(attrTypeID);
        break;
      case Guid attrTypeGuid:
        attributeTypeName = MetaDataHelper.GetAttributeTypeName(attrTypeGuid);
        break;
      default:
        return $"{id}";
    }
    return string.IsNullOrEmpty(attributeTypeName) ? $"{id}?" : attributeTypeName;
  }

  private void WriteConditionToTraceLog(ConditionStructure cs)
  {
    TraceLog.Write($"Condition: attribute={this.GetAttributeToTraceLog(cs.Attribute)}, attributeSource={cs.AttributeSource}, operator={cs.RelationalOperator}, value={this.GetValueToTraceLog(cs.Value)}, value1={this.GetValueToTraceLog(cs.Value2)}, typeID={cs.TypeID}, caseSensitive={cs.CaseSensitive}, logicalOperator={cs.LogicalOperator}, droupID={cs.GroupID}");
    if (cs.NestedConditions == null)
      return;
    TraceLog.Write("NestedConditions:");
    foreach (ConditionStructure nestedCondition in cs.NestedConditions)
      this.WriteConditionToTraceLog(nestedCondition);
  }

  public PublishObjectsTable SelectPublishObjects(
    Guid sessionGuid,
    int objectType,
    DBQueryParams dbParams)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start SelectPublishObjects objectType={objectType} sessionGuid={sessionGuid}");
    IUserSession userSession = this.GetUserSession(sessionGuid);
    IDBObjectCollection objectCollection = new MetadataInfoAction().GetPublishObjectTypes(userSession).Contains(objectType) ? userSession.GetObjectCollection(objectType) : throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_49"), (object) objectType));
    DBRecordSetParams paramSet = DBQueryParams.UnformingParams(dbParams);
    ConditionStructure[] conditionStructureArray = ActionsHelper.GetConditionOnEnabledObjects(userSession);
    if (paramSet.Conditions != null && paramSet.Conditions.Length != 0)
      conditionStructureArray = ConditionStructure.Join(conditionStructureArray, paramSet.Conditions);
    ConditionStructure[] objectTypesFilters = this.GetObjectTypesFilters();
    if (objectTypesFilters != null && objectTypesFilters.Length != 0)
      conditionStructureArray = ConditionStructure.Join(objectTypesFilters, conditionStructureArray);
    paramSet.Conditions = conditionStructureArray;
    if (TraceLog.Enabled)
    {
      foreach (ConditionStructure condition in paramSet.Conditions)
        this.WriteConditionToTraceLog(condition);
    }
    DataTable table = objectCollection.Select(paramSet);
    if (TraceLog.Enabled)
    {
      TraceLog.Write($"Select objects count:{table.Rows.Count}");
      TraceLog.Write($"End SelectPublishObjects objectType={objectType}");
    }
    return new PublishObjectsTable(table);
  }

  public long[] GetImportComposition(
    Guid sessionGuid,
    long[] objectIDs,
    string[] filteredTypes,
    int countLevels)
  {
    IUserSession userSession = this.GetUserSession(sessionGuid);
    List<Tuple<long, bool>> tupleList1 = new List<Tuple<long, bool>>();
    List<Tuple<Guid, long>> tupleList2 = new List<Tuple<Guid, long>>();
    long[] rootObjectIDs = objectIDs;
    string[] filteredTypes1 = filteredTypes;
    List<Tuple<long, bool>> objects = tupleList1;
    List<Tuple<Guid, long>> relations = tupleList2;
    int countLevels1 = countLevels;
    CompositionHelper.GetComposition(userSession, rootObjectIDs, filteredTypes1, objects, relations, countLevels1);
    return tupleList1.Count != 0 ? tupleList1.ConvertAll<long>((Converter<Tuple<long, bool>, long>) (_ => _.Item1)).ToArray() : (long[]) null;
  }

  public PublishObjectsTable SelectComposition(
    Guid sessionGuid,
    long objectID,
    DBQueryParams dbParams,
    int countLevels)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start SelectComposition objectID={objectID} sessionGuid={sessionGuid}");
    IUserSession userSession = this.GetUserSession(sessionGuid);
    if (dbParams.Columns == null || dbParams.Columns.Length == 0)
      throw new Exception(LocalizationHolder.rm.GetString("PortalServer_50"));
    if (countLevels == 0)
      throw new ArgumentOutOfRangeException(nameof (countLevels));
    DataTable compositionTable = CompositionHelper.GetCompositionTable(userSession, objectID, (string[]) null, dbParams, countLevels);
    compositionTable.TableName = dbParams.TableName != string.Empty ? dbParams.TableName : string.Format(LocalizationHolder.rm.GetString("PortalServer_51"), (object) objectID);
    if (compositionTable.Rows.Count > 0 && dbParams.SortColumns != null && dbParams.SortColumns.Length != 0)
    {
      List<string> stringList = new List<string>();
      IDBAttributeType[] attributeTypeList1 = (userSession as UserSession).GetAttributeTypeCollection(0).GetAttributeTypeList(dbParams.Columns, true);
      IDBAttributeType[] attributeTypeList2 = (userSession as UserSession).GetAttributeTypeCollection(0).GetAttributeTypeList(dbParams.SortColumns, true);
      StringBuilder stringBuilder = new StringBuilder();
      for (int index1 = 0; index1 < attributeTypeList2.Length; ++index1)
      {
        AttributeSourceTypes attributeSourceTypes1 = dbParams.SortSources == null || dbParams.SortSources.Length == 0 ? AttributeSourceTypes.Auto : dbParams.SortSources[index1];
        SortOrders sortOrders = dbParams.Orders == null || dbParams.Orders.Length == 0 ? SortOrders.ASC : dbParams.Orders[index1];
        string str = string.Empty;
        for (int index2 = 0; index2 < attributeTypeList1.Length; ++index2)
        {
          AttributeSourceTypes attributeSourceTypes2 = dbParams.ColumnsInfo == null || dbParams.ColumnsInfo.Length == 0 ? AttributeSourceTypes.Auto : dbParams.ColumnsInfo[index2].AttributeSource;
          if (attributeTypeList1[index2].AttributeID == attributeTypeList2[index1].AttributeID && (attributeSourceTypes1 == AttributeSourceTypes.Auto || attributeSourceTypes2 == attributeSourceTypes1))
          {
            if (attributeTypeList2[index1].AttributeType == FieldTypes.ftMeasured)
            {
              string columnName1 = Convert.ToString(compositionTable.Columns.Count);
              stringList.Add(columnName1);
              string columnName2 = index2.ToString();
              DataColumn column = new DataColumn(columnName1, typeof (double));
              compositionTable.Columns.Add(column);
              for (int index3 = 0; index3 < compositionTable.Rows.Count; ++index3)
              {
                double num = 0.0;
                try
                {
                  string mValue = Convert.ToString(compositionTable.Rows[index3][columnName2]);
                  if (mValue != string.Empty)
                  {
                    MeasuredValue measuredValue = MeasureHelper.ConvertToMeasuredValue(mValue);
                    if (measuredValue != null)
                      num = MeasureHelper.ConvertToBaseMeasure(measuredValue).Value;
                  }
                }
                catch
                {
                }
                compositionTable.Rows[index3][columnName1] = (object) num;
              }
              compositionTable.AcceptChanges();
              str = columnName1;
              break;
            }
            str = index2.ToString();
            break;
          }
        }
        if (str != string.Empty)
          stringBuilder.Append($"[{str}] {sortOrders.ToString()},");
      }
      if (stringBuilder.Length > 1)
        stringBuilder.Remove(stringBuilder.Length - 1, 1);
      DataRow[] fromRows = compositionTable.Select(string.Empty, stringBuilder.ToString());
      DataTable dataTable = compositionTable.Clone();
      DataSetProcessor.AssignRows(dataTable, (IEnumerable<DataRow>) fromRows);
      foreach (string name in stringList)
        dataTable.Columns.Remove(name);
      dataTable.AcceptChanges();
      if (TraceLog.Enabled)
        TraceLog.Write($"End SelectCompositionwith sorted objectID={objectID}");
      return new PublishObjectsTable(dataTable);
    }
    if (TraceLog.Enabled)
      TraceLog.Write($"End SelectComposition objectID={objectID}");
    return new PublishObjectsTable(compositionTable);
  }

  public string[][] SelectPublishObjectsEx(
    Guid sessionGuid,
    int objectType,
    string[] columns,
    int recordCount)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start SelectPublishObjectsEx objectType={objectType} sessionGuid={sessionGuid}");
    DBQueryParams dbParams = new DBQueryParams();
    if (columns == null || columns.Length == 0)
      return (string[][]) null;
    dbParams.Columns = this.ConvertColumns(columns);
    dbParams.RecordCount = recordCount;
    PublishObjectsTable table = this.SelectPublishObjects(sessionGuid, objectType, dbParams);
    if (TraceLog.Enabled)
      TraceLog.Write($"End SelectPublishObjectsEx objectType={objectType}");
    return this.FormingStringsArray(table);
  }

  public string[][] SelectPublishObjectsEx(
    Guid sessionGuid,
    int objectType,
    string[] columns,
    int recordCount,
    string[] attributes,
    int[] relationalOperators,
    string[] values,
    string[] values2,
    int[] logicalOperators,
    int[] groupIDs,
    bool[] caseSensitives)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start SelectPublishObjectsEx with conditions objectType={objectType} sessionGuid={sessionGuid}");
    IUserSession userSession = this.GetUserSession(sessionGuid);
    DBQueryParams dbParams = new DBQueryParams();
    if (columns == null || columns.Length == 0)
      return (string[][]) null;
    dbParams.Columns = this.ConvertColumns(columns);
    dbParams.RecordCount = recordCount;
    if (attributes != null && attributes.Length != 0)
    {
      string format = LocalizationHolder.rm.GetString("PortalServer_52");
      if (relationalOperators == null || relationalOperators.Length != attributes.Length)
        throw new ArgumentException(string.Format(format, (object) nameof (relationalOperators)));
      if (values == null || values.Length != attributes.Length)
        throw new ArgumentException(string.Format(format, (object) nameof (values)));
      if (logicalOperators == null || logicalOperators.Length != attributes.Length)
        throw new ArgumentException(string.Format(format, (object) nameof (logicalOperators)));
      if (values2 != null && values2.Length != 0 && values2.Length != attributes.Length)
        throw new ArgumentException(string.Format(format, (object) nameof (values2)));
      if (groupIDs != null && groupIDs.Length != 0 && groupIDs.Length != attributes.Length)
        throw new ArgumentException(string.Format(format, (object) nameof (groupIDs)));
      if (caseSensitives != null && caseSensitives.Length != attributes.Length)
        throw new ArgumentException(string.Format(format, (object) nameof (caseSensitives)));
      ConditionStructure[] conditionStructureArray = new ConditionStructure[attributes.Length];
      for (int index = 0; index < attributes.Length; ++index)
      {
        ActionsHelper.ValuePresentInEnum(typeof (RelationalOperators), relationalOperators[index], nameof (relationalOperators));
        ObjectCollectionAction.AttrType attrType1 = this.CheckAttributeIdentifier(attributes[index]);
        IDBAttributeType attrType2 = (IDBAttributeType) null;
        switch (attrType1)
        {
          case ObjectCollectionAction.AttrType.Integer:
            attrType2 = userSession.GetAttributeType(Convert.ToInt32(attributes[index]), false);
            break;
          case ObjectCollectionAction.AttrType.Guid:
            attrType2 = userSession.GetAttributeType(new Guid(attributes[index]), false);
            break;
          case ObjectCollectionAction.AttrType.Name:
            attrType2 = userSession.GetAttributeType(attributes[index], false);
            break;
        }
        if (attrType2 == null)
          throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_53"), (object) attributes[index]));
        object conditionValue2 = (object) null;
        if (values2 != null && values2.Length != 0 && values2[index] != string.Empty)
          conditionValue2 = this.ConvertValue(userSession, attrType2, values2[index]);
        conditionStructureArray[index] = new ConditionStructure(attrType2.AttributeID, (RelationalOperators) relationalOperators[index], this.ConvertValue(userSession, attrType2, values[index]), conditionValue2, (LogicalOperators) logicalOperators[index], groupIDs == null || groupIDs.Length == 0 ? 0 : groupIDs[index], caseSensitives != null && caseSensitives[index]);
      }
      dbParams.Conditions = conditionStructureArray;
    }
    PublishObjectsTable table = this.SelectPublishObjects(sessionGuid, objectType, dbParams);
    if (TraceLog.Enabled)
      TraceLog.Write($"End SelectPublishObjectsEx with conditions objectType={objectType}");
    return this.FormingStringsArray(table);
  }

  public string GetObjectAttributesEx(Guid sessionGuid, long objectID, params string[] attrIDs)
  {
    return this.GetXMLFromAttributes(this.GetObjectAttributes(sessionGuid, objectID, attrIDs));
  }

  public string GetRelationAttributesEx(Guid sessionGuid, long relationID, params string[] attrIDs)
  {
    return this.GetXMLFromAttributes(this.GetRelationAttributes(sessionGuid, relationID, attrIDs));
  }

  public PublishAttribute[] GetObjectAttributes(Guid sessionGuid, long objectID, string[] attrIDs)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start GetObjectAttributes objectID={objectID} sessionGuid={sessionGuid}");
    IUserSession userSession = this.GetUserSession(sessionGuid);
    if (attrIDs == null || attrIDs.Length == 0)
    {
      PublishAttribute[] objectAttributes = this.GetObjectAttributes(userSession, objectID);
      if (!TraceLog.Enabled)
        return objectAttributes;
      TraceLog.Write($"End GetObjectAttributes objectID={objectID}");
      return objectAttributes;
    }
    IDBObject dbObject = userSession.GetObject(objectID);
    XmlNode rootNode = this.GetRootNode((IDBAttributable) dbObject);
    ObjectInfo objectAttributes1 = AttributesFile.GetObjectAttributes(rootNode);
    PublishAttribute[] objectAttributes2 = new PublishAttribute[attrIDs.Length];
    for (int index = 0; index < attrIDs.Length; ++index)
    {
      List<AttributeValue> attributeValueList = new List<AttributeValue>(1);
      bool flag = false;
      if (GuidHelper.IsGuid(attrIDs[index]))
      {
        flag = true;
      }
      else
      {
        AttributeValue obligatoryValue = this.GetObligatoryValue(dbObject, objectAttributes1, attrIDs[index]);
        if (obligatoryValue != null)
        {
          AttributeInfo info = new AttributeInfo()
          {
            Name = attrIDs[index]
          };
          attributeValueList.Add(obligatoryValue);
          objectAttributes2[index] = new PublishAttribute(info, attributeValueList.ToArray(), PublishAttributeCategory.Object);
          continue;
        }
      }
      for (int i1 = 0; i1 < rootNode.ChildNodes.Count; ++i1)
      {
        XmlNode childNode1 = rootNode.ChildNodes[i1];
        if (childNode1.Name == PortalConsts.XmlNodeAttribute)
        {
          AttributeInfo attributeInfo = AttributesFile.GetAttributeInfo(childNode1);
          if (flag)
          {
            if (attributeInfo.Guid != attrIDs[index])
              continue;
          }
          else if (attributeInfo.Name != attrIDs[index])
            continue;
          for (int i2 = 0; i2 < childNode1.ChildNodes.Count; ++i2)
          {
            XmlNode childNode2 = childNode1.ChildNodes[i2];
            if (childNode2.Name == PortalConsts.XmlNodeValueAttribute)
              attributeValueList.Add(AttributesFile.GetAttributeValue(childNode2));
          }
          objectAttributes2[index] = new PublishAttribute(attributeInfo, attributeValueList.ToArray(), PublishAttributeCategory.Object);
          break;
        }
      }
    }
    if (TraceLog.Enabled)
      TraceLog.Write($"End GetObjectAttributes objectID={objectID}");
    return objectAttributes2;
  }

  public PublishAttribute[] GetRelationAttributes(
    Guid sessionGuid,
    long relationID,
    params string[] attrIDs)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start GetRelationAttributes relationID={relationID} sessionGuid={sessionGuid}");
    IUserSession userSession = this.GetUserSession(sessionGuid);
    if (attrIDs == null || attrIDs.Length == 0)
      return this.GetRelationAttributes(userSession, relationID);
    IDBRelation relation = userSession.GetRelation(relationID);
    XmlNode rootNode = this.GetRootNode((IDBAttributable) relation);
    RelationInfo relationAttributes1 = AttributesFile.GetRelationAttributes(rootNode);
    PublishAttribute[] relationAttributes2 = new PublishAttribute[attrIDs.Length];
    for (int index = 0; index < attrIDs.Length; ++index)
    {
      List<AttributeValue> attributeValueList = new List<AttributeValue>(1);
      bool flag = false;
      if (GuidHelper.IsGuid(attrIDs[index]))
      {
        flag = true;
      }
      else
      {
        AttributeValue obligatoryValue = this.GetObligatoryValue(relation, relationAttributes1, attrIDs[index]);
        if (obligatoryValue != null)
        {
          AttributeInfo info = new AttributeInfo()
          {
            Name = attrIDs[index]
          };
          attributeValueList.Add(obligatoryValue);
          relationAttributes2[index] = new PublishAttribute(info, attributeValueList.ToArray(), PublishAttributeCategory.Relation);
          continue;
        }
      }
      for (int i1 = 0; i1 < rootNode.ChildNodes.Count; ++i1)
      {
        XmlNode childNode1 = rootNode.ChildNodes[i1];
        if (childNode1.Name == PortalConsts.XmlNodeAttribute)
        {
          AttributeInfo attributeInfo = AttributesFile.GetAttributeInfo(childNode1);
          if (flag)
          {
            if (attributeInfo.Guid != attrIDs[index])
              continue;
          }
          else if (attributeInfo.Name != attrIDs[index])
            continue;
          for (int i2 = 0; i2 < childNode1.ChildNodes.Count; ++i2)
          {
            XmlNode childNode2 = childNode1.ChildNodes[i2];
            if (childNode2.Name == PortalConsts.XmlNodeValueAttribute)
              attributeValueList.Add(AttributesFile.GetAttributeValue(childNode2));
          }
          relationAttributes2[index] = new PublishAttribute(attributeInfo, attributeValueList.ToArray(), PublishAttributeCategory.Relation);
          break;
        }
      }
    }
    if (TraceLog.Enabled)
      TraceLog.Write($"End GetRelationAttributes relationID={relationID}");
    return relationAttributes2;
  }

  private ConditionStructure[] GetObjectTypesFilters()
  {
    PortalSettings service = (PortalSettings) ServerServices.GetService(typeof (PortalSettings));
    if (service.DisableSelectTypes == null || service.DisableSelectTypes.Length == 0)
      return (ConditionStructure[]) null;
    List<ConditionStructure> conditionStructureList = new List<ConditionStructure>(service.DisableSelectTypes.Length);
    for (int index = 0; index < service.DisableSelectTypes.Length; ++index)
    {
      string disableSelectType = service.DisableSelectTypes[index];
      if (GuidHelper.IsGuid(disableSelectType))
        conditionStructureList.Add(new ConditionStructure(new Guid("cad001a0-306c-11d8-b4e9-00304f19f545"), RelationalOperators.NotEqual, (object) new Guid(disableSelectType), LogicalOperators.AND, 0));
      else
        conditionStructureList.Add(new ConditionStructure(PortalServerConsts.attributeObjTypeName, RelationalOperators.NotEqual, (object) disableSelectType, LogicalOperators.AND, 0));
    }
    return conditionStructureList.ToArray();
  }

  private AttributeValue GetObligatoryValue(
    IDBRelation relation,
    RelationInfo relInfo,
    string paramName)
  {
    AttributeValue attributeValue = new AttributeValue()
    {
      InListID = -1
    };
    switch (paramName)
    {
      case "F_CREATE_DATE":
        attributeValue.InListID = 0;
        if (relInfo.CreateDate != DateTime.MinValue)
        {
          attributeValue.DateTimeValue = Convert.ToString(relInfo.CreateDate, (IFormatProvider) CultureInfo.InvariantCulture);
          break;
        }
        break;
      case "F_GUID":
        attributeValue.InListID = 0;
        if (relInfo.Guid != Guid.Empty)
        {
          attributeValue.GuidValue = Convert.ToString((object) relInfo.Guid);
          break;
        }
        break;
      case "F_PART_GUID":
        attributeValue.InListID = 0;
        if (relInfo.PartGuid != Guid.Empty)
        {
          attributeValue.GuidValue = Convert.ToString((object) relInfo.PartGuid);
          break;
        }
        break;
      case "F_PRJLINK_ID":
        attributeValue.InListID = 0;
        attributeValue.IntegerValue = relation.RelationID;
        break;
      case "F_PROJECT_GUID":
        attributeValue.InListID = 0;
        if (relInfo.ProjectGuid != Guid.Empty)
        {
          attributeValue.GuidValue = Convert.ToString((object) relInfo.ProjectGuid);
          break;
        }
        break;
      case "F_RELATION_TYPE_GUID":
        attributeValue.InListID = 0;
        if (relInfo.RelationTypeGuid != Guid.Empty)
        {
          attributeValue.GuidValue = Convert.ToString((object) relInfo.RelationTypeGuid);
          break;
        }
        break;
      case "F_RELATION_TYPE_NAME":
        attributeValue.InListID = 0;
        if (relInfo.RelationTypeName != string.Empty)
        {
          attributeValue.StringValue = relInfo.RelationTypeName;
          break;
        }
        break;
    }
    return attributeValue.InListID != -1 ? attributeValue : (AttributeValue) null;
  }

  private AttributeValue GetObligatoryValue(
    IDBObject publishObject,
    ObjectInfo objInfo,
    string paramName)
  {
    AttributeValue attributeValue = new AttributeValue()
    {
      InListID = -1
    };
    switch (paramName)
    {
      case "CAPTION":
        attributeValue.InListID = 0;
        attributeValue.StringValue = publishObject.Caption;
        break;
      case "F_GUID":
        attributeValue.InListID = 0;
        if (objInfo.Guid != Guid.Empty)
        {
          attributeValue.GuidValue = Convert.ToString((object) objInfo.Guid);
          break;
        }
        break;
      case "F_LC_STEP":
        attributeValue.InListID = 0;
        if (objInfo.LCStep != Guid.Empty)
        {
          attributeValue.GuidValue = Convert.ToString((object) objInfo.LCStep);
          break;
        }
        break;
      case "F_LEVEL_ID":
        attributeValue.InListID = 0;
        if (objInfo.LCLevel != Guid.Empty)
        {
          attributeValue.GuidValue = Convert.ToString((object) objInfo.LCLevel);
          break;
        }
        break;
      case "F_OBJECT_GUID":
        attributeValue.InListID = 0;
        if (objInfo.ObjectGuid != Guid.Empty)
        {
          attributeValue.GuidValue = Convert.ToString((object) objInfo.ObjectGuid);
          break;
        }
        break;
      case "F_OBJECT_ID":
        attributeValue.InListID = 0;
        attributeValue.IntegerValue = publishObject.ObjectID;
        break;
      case "F_OBJECT_TYPE":
        attributeValue.InListID = 0;
        attributeValue.IntegerValue = (long) publishObject.ObjectType;
        break;
      case "F_OBJTYPE_GUID":
        attributeValue.InListID = 0;
        if (objInfo.ObjectTypeGuid != Guid.Empty)
        {
          attributeValue.GuidValue = Convert.ToString((object) objInfo.ObjectTypeGuid);
          break;
        }
        break;
      case "F_OBJ_CREATE":
        attributeValue.InListID = 0;
        if (objInfo.CreateDate != DateTime.MinValue)
        {
          attributeValue.DateTimeValue = Convert.ToString(objInfo.CreateDate, (IFormatProvider) CultureInfo.InvariantCulture);
          break;
        }
        break;
      case "F_OBJ_TYPE_NAME":
        attributeValue.InListID = 0;
        if (objInfo.ObjTypeName != string.Empty)
        {
          attributeValue.StringValue = objInfo.ObjTypeName;
          break;
        }
        break;
      case "F_OWNER_ID":
        attributeValue.InListID = 0;
        if (objInfo.OwnerGuid != Guid.Empty)
        {
          attributeValue.GuidValue = Convert.ToString((object) objInfo.OwnerGuid);
          break;
        }
        break;
      case "F_PARENT_GUID":
        attributeValue.InListID = 0;
        if (objInfo.ParentGuid != Guid.Empty)
        {
          attributeValue.GuidValue = Convert.ToString((object) objInfo.ParentGuid);
          break;
        }
        break;
      case "F_PROJECT_ID":
        attributeValue.InListID = 0;
        if (objInfo.ProjectGuid != Guid.Empty)
        {
          attributeValue.DateTimeValue = Convert.ToString(objInfo.CreateDate, (IFormatProvider) CultureInfo.InvariantCulture);
          break;
        }
        break;
    }
    return attributeValue.InListID == -1 ? (AttributeValue) null : attributeValue;
  }

  private PublishAttribute[] GetObjectAttributes(IUserSession session, long objectID)
  {
    IDBObject dbObject = session.GetObject(objectID);
    XmlNode rootNode = this.GetRootNode((IDBAttributable) dbObject);
    ObjectInfo objectAttributes = AttributesFile.GetObjectAttributes(rootNode);
    List<PublishAttribute> publishAttributeList = new List<PublishAttribute>()
    {
      new PublishAttribute(new AttributeInfo("F_OWNER_ID"), new AttributeValue[1]
      {
        this.GetObligatoryValue(dbObject, objectAttributes, "F_OWNER_ID")
      }, PublishAttributeCategory.Object),
      new PublishAttribute(new AttributeInfo("F_LC_STEP"), new AttributeValue[1]
      {
        this.GetObligatoryValue(dbObject, objectAttributes, "F_LC_STEP")
      }, PublishAttributeCategory.Object),
      new PublishAttribute(new AttributeInfo("F_LEVEL_ID"), new AttributeValue[1]
      {
        this.GetObligatoryValue(dbObject, objectAttributes, "F_LEVEL_ID")
      }, PublishAttributeCategory.Object)
    };
    AttributeValue obligatoryValue = this.GetObligatoryValue(dbObject, objectAttributes, "F_OBJ_CREATE");
    if (obligatoryValue.DateTimeValue != string.Empty)
      publishAttributeList.Add(new PublishAttribute(new AttributeInfo("F_OBJ_CREATE"), new AttributeValue[1]
      {
        obligatoryValue
      }, PublishAttributeCategory.Object));
    publishAttributeList.Add(new PublishAttribute(new AttributeInfo("F_PROJECT_ID"), new AttributeValue[1]
    {
      this.GetObligatoryValue(dbObject, objectAttributes, "F_PROJECT_ID")
    }, PublishAttributeCategory.Object));
    publishAttributeList.Add(new PublishAttribute(new AttributeInfo("F_GUID"), new AttributeValue[1]
    {
      this.GetObligatoryValue(dbObject, objectAttributes, "F_GUID")
    }, PublishAttributeCategory.Object));
    publishAttributeList.Add(new PublishAttribute(new AttributeInfo("F_OBJECT_GUID"), new AttributeValue[1]
    {
      this.GetObligatoryValue(dbObject, objectAttributes, "F_OBJECT_GUID")
    }, PublishAttributeCategory.Object));
    publishAttributeList.Add(new PublishAttribute(new AttributeInfo("F_PARENT_GUID"), new AttributeValue[1]
    {
      this.GetObligatoryValue(dbObject, objectAttributes, "F_PARENT_GUID")
    }, PublishAttributeCategory.Object));
    publishAttributeList.Add(new PublishAttribute(new AttributeInfo("CAPTION"), new AttributeValue[1]
    {
      this.GetObligatoryValue(dbObject, objectAttributes, "CAPTION")
    }, PublishAttributeCategory.Object));
    List<string> addedAtributes = new List<string>(rootNode.ChildNodes.Count);
    List<PublishAttribute> attributesFromFile = this.GetAttributesFromFile(rootNode, PublishAttributeCategory.Object, ref addedAtributes);
    if (attributesFromFile.Count > 0)
      publishAttributeList.AddRange((IEnumerable<PublishAttribute>) attributesFromFile);
    AttributeValue attributeValue1 = new AttributeValue()
    {
      IntegerValue = dbObject.ObjectID
    };
    AttributeValue attributeValue2 = new AttributeValue()
    {
      IntegerValue = dbObject.ID
    };
    AttributeValue attributeValue3 = new AttributeValue()
    {
      IntegerValue = (long) dbObject.ObjectType
    };
    AttributeValue attributeValue4 = new AttributeValue()
    {
      DateTimeValue = Convert.ToString(dbObject.CreateDate - session.TimeZoneOffset, (IFormatProvider) CultureInfo.InvariantCulture)
    };
    publishAttributeList.Add(new PublishAttribute(new AttributeInfo("F_OBJECT_ID"), new AttributeValue[1]
    {
      attributeValue1
    }, PublishAttributeCategory.PublishObject));
    publishAttributeList.Add(new PublishAttribute(new AttributeInfo("F_ID"), new AttributeValue[1]
    {
      attributeValue2
    }, PublishAttributeCategory.PublishObject));
    publishAttributeList.Add(new PublishAttribute(new AttributeInfo("F_OBJECT_TYPE"), new AttributeValue[1]
    {
      attributeValue3
    }, PublishAttributeCategory.PublishObject));
    publishAttributeList.Add(new PublishAttribute(new AttributeInfo("F_OBJ_CREATE"), new AttributeValue[1]
    {
      attributeValue4
    }, PublishAttributeCategory.PublishObject));
    List<PublishAttribute> fromAttributable = this.GetAttributesFromAttributable(session, (IDBAttributable) dbObject, PublishAttributeCategory.PublishObject, addedAtributes);
    if (fromAttributable.Count > 0)
      publishAttributeList.AddRange((IEnumerable<PublishAttribute>) fromAttributable);
    return publishAttributeList.ToArray();
  }

  private XmlNode GetRootNode(IDBAttributable publish)
  {
    IDBAttribute attributeByGuid = publish.GetAttributeByGuid(PortalServerConsts.attributeFile);
    XmlDocument xmlDocument = new XmlDocument();
    if (attributeByGuid != null && attributeByGuid.ValuesCount > 0)
    {
      IPackedStream service = ServiceUtils.GetService<IPackedStream>((object) ApplicationServices.Container, true);
      for (int index = 0; index < attributeByGuid.ValuesCount; ++index)
      {
        attributeByGuid.Index = index;
        IBlobReader blobReader = attributeByGuid as IBlobReader;
        BlobInformation blobInformation = blobReader.OpenBlob(0);
        try
        {
          if (blobInformation.FileName == PortalConsts.AttributesXmlFileName)
          {
            byte[] buffer = blobReader.ReadDataBlock(0);
            if (buffer != null)
            {
              using (MemoryStream inStream = new MemoryStream())
              {
                try
                {
                  inStream.Write(buffer, 0, buffer.Length);
                  using (MemoryStream memoryStream = new MemoryStream())
                  {
                    inStream.Position = 0L;
                    service.UnpackStream((Stream) memoryStream, (Stream) inStream);
                    memoryStream.Position = 0L;
                    xmlDocument.Load((Stream) memoryStream);
                    break;
                  }
                }
                finally
                {
                  inStream.Flush();
                  inStream.Close();
                }
              }
            }
            else
              break;
          }
        }
        finally
        {
          blobReader.CloseBlob();
        }
      }
    }
    if (xmlDocument.ChildNodes == null || xmlDocument.ChildNodes.Count == 0)
      throw new Exception(LocalizationHolder.rm.GetString("PortalServer_8"));
    XmlNode xmlNode = (XmlNode) null;
    for (int i = 0; i < xmlDocument.ChildNodes.Count; ++i)
    {
      if (xmlDocument.ChildNodes[i].Name == PortalConsts.XmlRootNodeAttributes)
      {
        xmlNode = xmlDocument.ChildNodes[i];
        break;
      }
    }
    return xmlNode != null ? xmlNode : throw new Exception(LocalizationHolder.rm.GetString("PortalServer_9"));
  }

  private List<PublishAttribute> GetAttributesFromFile(
    XmlNode rootNode,
    PublishAttributeCategory category,
    ref List<string> addedAtributes)
  {
    List<PublishAttribute> attributesFromFile = new List<PublishAttribute>(rootNode.ChildNodes.Count);
    for (int i1 = 0; i1 < rootNode.ChildNodes.Count; ++i1)
    {
      XmlNode childNode1 = rootNode.ChildNodes[i1];
      if (childNode1.Name == PortalConsts.XmlNodeAttribute)
      {
        AttributeInfo attributeInfo = AttributesFile.GetAttributeInfo(childNode1);
        List<AttributeValue> attributeValueList = new List<AttributeValue>(1);
        for (int i2 = 0; i2 < childNode1.ChildNodes.Count; ++i2)
        {
          XmlNode childNode2 = childNode1.ChildNodes[i2];
          if (childNode2.Name == PortalConsts.XmlNodeValueAttribute)
            attributeValueList.Add(AttributesFile.GetAttributeValue(childNode2));
        }
        attributesFromFile.Add(new PublishAttribute(attributeInfo, attributeValueList.ToArray(), category));
        if (attributeInfo.Guid != string.Empty)
          addedAtributes.Add(attributeInfo.Name);
        else if (attributeInfo.Name != string.Empty)
          addedAtributes.Add(attributeInfo.Name);
      }
    }
    return attributesFromFile;
  }

  private List<PublishAttribute> GetAttributesFromAttributable(
    IUserSession session,
    IDBAttributable attributable,
    PublishAttributeCategory category,
    List<string> addedAtributes)
  {
    List<PublishAttribute> fromAttributable = new List<PublishAttribute>(attributable.Attributes.Count);
    ISitesCacheService customService = (ISitesCacheService) session.GetCustomService(typeof (ISitesCacheService));
    for (int AttrIndex = 0; AttrIndex < attributable.Attributes.Count; ++AttrIndex)
    {
      IDBAttribute attribute = attributable.Attributes[AttrIndex];
      if (attribute.AttributeID > 0)
      {
        List<string> stringList = addedAtributes;
        Guid guid = (attribute as IDBGuid).GUID;
        string str = guid.ToString();
        if (!stringList.Contains(str) && !addedAtributes.Contains(attribute.Name))
        {
          IDBAttributeType attributeType = session.GetAttributeType(attribute.AttributeID);
          List<PublishAttribute> publishAttributeList = fromAttributable;
          guid = (attribute as IDBGuid).GUID;
          PublishAttribute publishAttribute = new PublishAttribute(new AttributeInfo(guid.ToString(), attribute.Name, attributeType.ShortName, attributeType.Alias, attributeType.AttributeType), this.GetValues(session, customService, (attributeType as IDBGuid).GUID, attribute), category);
          publishAttributeList.Add(publishAttribute);
        }
      }
    }
    return fromAttributable;
  }

  private AttributeValue[] GetValues(
    IUserSession session,
    ISitesCacheService cacheService,
    Guid attrGuid,
    IDBAttribute attribute)
  {
    List<AttributeValue> attributeValueList = new List<AttributeValue>(attribute.ValuesCount);
    for (int index = 0; index < attribute.ValuesCount; ++index)
    {
      attribute.Index = index;
      AttributeValue av = new AttributeValue()
      {
        InListID = index,
        Description = attribute.Description
      };
      if (!attribute.IsNull)
      {
        switch (attribute.DataType)
        {
          case FieldTypes.ftString:
          case FieldTypes.ftMemo:
            av.StringValue = attribute.AsString;
            if (attrGuid != Guid.Empty && av.StringValue != null && av.StringValue != string.Empty && Array.Exists<Guid>(PortalConsts.SiteCodeAttributes, (Predicate<Guid>) (x => x.Equals(attrGuid))))
            {
              av.Description = string.Empty;
              int num = 0;
              foreach (char code in av.StringValue)
              {
                SiteInfo site = cacheService.GetSite(code);
                if (num > 0)
                  av.Description += ", ";
                av.Description += site == null ? $"{code}" : site.Caption;
                ++num;
              }
              break;
            }
            break;
          case FieldTypes.ftInteger:
          case FieldTypes.ftBoolean:
          case FieldTypes.ftAutoInc:
            av.IntegerValue = attribute.AsInteger;
            break;
          case FieldTypes.ftDouble:
            av.DoubleValue = attribute.AsDouble;
            break;
          case FieldTypes.ftDateTime:
            av.DateTimeValue = attribute.AsDateTime != DateTime.MinValue ? Convert.ToString(attribute.AsDateTime - session.TimeZoneOffset, (IFormatProvider) CultureInfo.InvariantCulture) : string.Empty;
            break;
          case FieldTypes.ftShortBlob:
            this.ReadBlobInfo(session, (IBlobReader) attribute, av);
            break;
          case FieldTypes.ftFile:
            this.ReadBlobInfo(session, (IBlobReader) attribute, av);
            break;
          case FieldTypes.ftObjectLink:
            QuickObjectInfo objectInfo1 = session.GetObjectInfo(attribute.AsInteger);
            av.GuidValue = objectInfo1.VersionGuid.ToString();
            av.StringValue = attribute.AsString;
            break;
          case FieldTypes.ftPassword:
            av.StringValue = attribute.AsString;
            av.DateTimeValue = Convert.ToString(attribute.AsDateTime - session.TimeZoneOffset, (IFormatProvider) CultureInfo.InvariantCulture);
            break;
          case FieldTypes.ftBlob:
            this.ReadBlobInfo(session, (IBlobReader) attribute, av);
            break;
          case FieldTypes.ftMeasured:
            MeasuredValue measuredValue = attribute.Value as MeasuredValue;
            QuickObjectInfo objectInfo2 = session.GetObjectInfo(measuredValue.MeasureID);
            av.GuidValue = objectInfo2.VersionGuid.ToString();
            av.DoubleValue = measuredValue.Value;
            av.StringValue = measuredValue.Caption;
            break;
          case FieldTypes.ftGuid:
            if (attribute.AsString != string.Empty && GuidHelper.IsGuid(attribute.AsString))
            {
              av.GuidValue = attribute.AsString;
              break;
            }
            break;
        }
      }
      attributeValueList.Add(av);
    }
    return attributeValueList.ToArray();
  }

  private void ReadBlobInfo(IUserSession session, IBlobReader reader, AttributeValue av)
  {
    BlobInformation blobInformation = reader.OpenBlob(-1);
    try
    {
      av.StringValue = blobInformation.Note;
      av.DateTimeValue = Convert.ToString(blobInformation.ModifyDate - session.TimeZoneOffset, (IFormatProvider) CultureInfo.InvariantCulture);
      av.IntegerValue = blobInformation.RealFileSize;
      av.ArcMethod = blobInformation.ArcMethod;
      av.FileName = blobInformation.FileName;
    }
    finally
    {
      reader.CloseBlob();
    }
  }

  private PublishAttribute[] GetRelationAttributes(IUserSession session, long relationID)
  {
    IDBRelation relation = session.GetRelation(relationID);
    XmlNode rootNode = this.GetRootNode((IDBAttributable) relation);
    RelationInfo relationAttributes = AttributesFile.GetRelationAttributes(rootNode);
    List<PublishAttribute> publishAttributeList = new List<PublishAttribute>()
    {
      new PublishAttribute(new AttributeInfo("F_CREATE_DATE"), new AttributeValue[1]
      {
        this.GetObligatoryValue(relation, relationAttributes, "F_CREATE_DATE")
      }, PublishAttributeCategory.Relation),
      new PublishAttribute(new AttributeInfo("F_GUID"), new AttributeValue[1]
      {
        this.GetObligatoryValue(relation, relationAttributes, "F_GUID")
      }, PublishAttributeCategory.Relation)
    };
    List<string> addedAtributes = new List<string>(rootNode.ChildNodes.Count);
    List<PublishAttribute> attributesFromFile = this.GetAttributesFromFile(rootNode, PublishAttributeCategory.Relation, ref addedAtributes);
    if (attributesFromFile.Count > 0)
      publishAttributeList.AddRange((IEnumerable<PublishAttribute>) attributesFromFile);
    AttributeValue attributeValue = new AttributeValue()
    {
      IntegerValue = relation.RelationID
    };
    publishAttributeList.Add(new PublishAttribute(new AttributeInfo("F_PRJLINK_ID"), new AttributeValue[1]
    {
      attributeValue
    }, PublishAttributeCategory.PublishRelation));
    List<PublishAttribute> fromAttributable = this.GetAttributesFromAttributable(session, (IDBAttributable) relation, PublishAttributeCategory.PublishRelation, addedAtributes);
    if (fromAttributable.Count > 0)
      publishAttributeList.AddRange((IEnumerable<PublishAttribute>) fromAttributable);
    return publishAttributeList.ToArray();
  }

  private string GetXMLFromAttributes(PublishAttribute[] attrs)
  {
    if (attrs == null || attrs.Length == 0)
      return string.Empty;
    StringBuilder output = new StringBuilder();
    using (XmlWriter xmlWriter = XmlWriter.Create(output))
    {
      xmlWriter.WriteProcessingInstruction("xml", $"version=\"1.0\" encoding=\"{Encoding.Default.HeaderName}\"");
      xmlWriter.WriteStartElement(PortalConsts.XmlRootNodeAttributes);
      for (int index1 = 0; index1 < attrs.Length; ++index1)
      {
        PublishAttribute attr = attrs[index1];
        if (attr != null)
        {
          string empty = string.Empty;
          string str;
          if (attr.Info.Name == "F_GUID")
            str = attr.Category != PublishAttributeCategory.Object ? (attr.Category != PublishAttributeCategory.Relation ? ObligatoryObjectAttributesHelper.GetCaption(attr.Info.Name) : LocalizationHolder.rm.GetString("PortalServer_4")) : LocalizationHolder.rm.GetString("PortalServer_3");
          else if (attr.Info.Name == "F_OBJECT_GUID")
            str = LocalizationHolder.rm.GetString("PortalServer_5");
          else if (attr.Info.Name == "F_PARENT_GUID")
            str = LocalizationHolder.rm.GetString("PortalServer_6");
          else if (ObligatoryObjectAttributesHelper.IsObligatoryAttribute(attr.Info.Name))
          {
            ObligatoryObjectAttributes obligatoryObjectAttribute = ObligatoryObjectAttributesHelper.GetObligatoryObjectAttribute(attr.Info.Name);
            str = obligatoryObjectAttribute != ObligatoryObjectAttributes.None ? ObligatoryObjectAttributesHelper.GetCaption(obligatoryObjectAttribute) : attr.Info.Name;
          }
          else
            str = attr.Info.Name;
          xmlWriter.WriteStartElement(PortalConsts.XmlNodeAttribute);
          xmlWriter.WriteAttributeString("F_GUID", attr.Info.Guid);
          xmlWriter.WriteAttributeString("F_NAME", str);
          xmlWriter.WriteAttributeString("F_SHORT_NAME", attr.Info.ShortName);
          xmlWriter.WriteAttributeString("F_ALIAS", attr.Info.Alias);
          xmlWriter.WriteAttributeString("F_ATTRIBUTE_TYPE", Convert.ToString((int) attr.Info.FieldType));
          xmlWriter.WriteAttributeString("F_CATEGORY", Convert.ToString((int) attr.Category));
          if (attr.Values != null)
          {
            for (int index2 = 0; index2 < attr.Values.Length; ++index2)
            {
              AttributeValue attributeValue = attr.Values[index2];
              xmlWriter.WriteStartElement(PortalConsts.XmlNodeValueAttribute);
              xmlWriter.WriteAttributeString("F_INLIST_ID", attributeValue.InListID.ToString());
              if (!attributeValue.IsEmpty)
              {
                xmlWriter.WriteAttributeString("F_STRING_VALUE", attributeValue.StringValue);
                xmlWriter.WriteAttributeString("F_DATE_VALUE", attributeValue.DateTimeValue);
                xmlWriter.WriteAttributeString("F_INTEGER_VALUE", attributeValue.IntegerValue != long.MinValue ? Convert.ToString(attributeValue.IntegerValue) : string.Empty);
                xmlWriter.WriteAttributeString("F_DOUBLE_VALUE", attributeValue.DoubleValue != double.MinValue ? Convert.ToString(attributeValue.DoubleValue, (IFormatProvider) CultureInfo.InvariantCulture) : string.Empty);
                if (attributeValue.Description != string.Empty)
                  xmlWriter.WriteAttributeString("F_DESCRIPTION", attributeValue.Description);
                if (attributeValue.GuidValue != string.Empty)
                  xmlWriter.WriteAttributeString("F_FILE", attributeValue.GuidValue);
                if (attr.Info.FieldType == FieldTypes.ftMemo || attr.Info.FieldType == FieldTypes.ftFile || attr.Info.FieldType == FieldTypes.ftBlob || attr.Info.FieldType == FieldTypes.ftShortBlob)
                {
                  xmlWriter.WriteAttributeString("F_ARC_METHOD", Convert.ToString((int) attributeValue.ArcMethod));
                  xmlWriter.WriteAttributeString("F_FILE", attributeValue.FileName);
                }
              }
              xmlWriter.WriteEndElement();
            }
          }
          xmlWriter.WriteEndElement();
        }
      }
      xmlWriter.WriteEndElement();
      xmlWriter.Flush();
    }
    return output.ToString();
  }

  private int GetIndexForSpecialColumn(
    DBRecordSetParams pars,
    object attr,
    int countNotSpecCols,
    ref int countDelete)
  {
    int forSpecialColumn = Array.IndexOf<object>(pars.Columns, attr);
    if (forSpecialColumn <= countNotSpecCols - 1)
      return forSpecialColumn;
    ++countDelete;
    return forSpecialColumn;
  }

  private object ConvertValue(IUserSession session, IDBAttributeType attrType, string value)
  {
    object obj = (object) null;
    switch (attrType.AttributeType != FieldTypes.ftSystem ? (int) attrType.AttributeType : (int) ObligatoryObjectAttributesHelper.GetDataType((ObligatoryObjectAttributes) attrType.AttributeID))
    {
      case 1:
      case 5:
      case 6:
      case 10:
      case 11:
        obj = (object) value;
        break;
      case 2:
      case 14:
        long result1;
        if (value != string.Empty && long.TryParse(value, out result1))
        {
          obj = (object) result1;
          break;
        }
        break;
      case 3:
        double result2;
        if (value != string.Empty && double.TryParse(value, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result2))
        {
          obj = (object) result2;
          break;
        }
        break;
      case 4:
        DateTime result3;
        if (value != string.Empty && DateTime.TryParse(value, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result3))
        {
          obj = (object) result3;
          break;
        }
        break;
      case 8:
        if (value != string.Empty)
        {
          if (GuidHelper.IsGuid(value))
          {
            QuickObjectInfo objectInfo = session.GetObjectInfo(new Guid(value));
            if (!objectInfo.Empty)
            {
              obj = (object) objectInfo.ObjectID;
              break;
            }
            break;
          }
          long result4;
          if (long.TryParse(value, out result4))
          {
            obj = (object) result4;
            break;
          }
          break;
        }
        break;
      case 12:
        bool result5;
        if (value != string.Empty && bool.TryParse(value, out result5))
        {
          obj = (object) result5;
          break;
        }
        break;
      case 13:
        if (value != string.Empty)
        {
          MeasuredValue measuredValue = MeasureHelper.ConvertToMeasuredValue(value);
          if (measuredValue != null)
          {
            obj = (object) measuredValue;
            break;
          }
          break;
        }
        break;
      case 16 /*0x10*/:
        if (GuidHelper.IsGuid(value))
        {
          obj = (object) new Guid(value);
          break;
        }
        break;
    }
    return obj;
  }

  private string[][] FormingStringsArray(PublishObjectsTable table)
  {
    if (table.Rows == null || table.Rows.Length == 0 || table.Columns == null || table.Columns.Length == 0)
      return (string[][]) null;
    List<string[]> strArrayList = new List<string[]>(table.Rows.Length);
    for (int index1 = 0; index1 < table.Rows.Length; ++index1)
    {
      PublishObjectsRow row = table.Rows[index1];
      string[] strArray = new string[table.Columns.Length];
      for (int index2 = 0; index2 < table.Columns.Length; ++index2)
      {
        switch (table.Columns[index2].TypeCode)
        {
          case ColumnTypeCode.tcDBNull:
            strArray[index2] = string.Empty;
            break;
          case ColumnTypeCode.tcDouble:
          case ColumnTypeCode.tcDateTime:
            strArray[index2] = Convert.ToString(row.Data[index2], (IFormatProvider) CultureInfo.InvariantCulture);
            break;
          default:
            strArray[index2] = Convert.ToString(row.Data[index2]);
            break;
        }
      }
      strArrayList.Add(strArray);
    }
    return strArrayList.ToArray();
  }

  private ObjectCollectionAction.AttrType CheckAttributeIdentifier(string value)
  {
    if (GuidHelper.IsGuid(value))
      return ObjectCollectionAction.AttrType.Guid;
    return int.TryParse(value, out int _) ? ObjectCollectionAction.AttrType.Integer : ObjectCollectionAction.AttrType.Name;
  }

  private object[] ConvertColumns(string[] columns)
  {
    List<object> objectList = new List<object>();
    for (int index = 0; index < columns.Length; ++index)
    {
      switch (this.CheckAttributeIdentifier(columns[index]))
      {
        case ObjectCollectionAction.AttrType.Integer:
          objectList.Add((object) Convert.ToInt32(columns[index]));
          break;
        case ObjectCollectionAction.AttrType.Guid:
          objectList.Add((object) new Guid(columns[index]));
          break;
        case ObjectCollectionAction.AttrType.Name:
          objectList.Add((object) columns[index]);
          break;
      }
    }
    return objectList.ToArray();
  }

  private enum AttrType
  {
    Integer,
    Guid,
    Name,
  }
}
