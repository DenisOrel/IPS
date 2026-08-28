// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.MetadataInfoAction
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

#nullable disable
namespace Intermech.Portal.Server;

internal class MetadataInfoAction : PortalAction
{
  public PortalObjectType[] GetObjectTypesTree(Guid sessionGuid)
  {
    IUserSession userSession = this.GetUserSession(sessionGuid);
    IDBObjectType objectType1 = userSession.GetObjectType(PortalConsts.objtypePublishObjects, true);
    DataTable dataTable = userSession.GetObjectTypeCollection(objectType1.ObjectType, true).SelectRecursive(string.Empty);
    List<PortalObjectType> portalObjectTypeList = new List<PortalObjectType>(dataTable.Rows.Count + 1)
    {
      this.GetPortalObjectType(objectType1)
    };
    for (int index = 0; index < dataTable.Rows.Count; ++index)
    {
      IDBObjectType objectType2 = userSession.GetObjectType(Convert.ToInt32(dataTable.Rows[index]["F_OBJECT_TYPE"]));
      portalObjectTypeList.Add(this.GetPortalObjectType(objectType2));
    }
    IDBObjectType objectType3 = userSession.GetObjectType(PortalConsts.objtypePacket);
    portalObjectTypeList.Add(this.GetPortalObjectType(objectType3));
    return portalObjectTypeList.ToArray();
  }

  public string[][] GetPublishObjectTypes(Guid sessionGuid)
  {
    IUserSession userSession = this.GetUserSession(sessionGuid);
    IDBObjectType objectType1 = userSession.GetObjectType(PortalConsts.objtypePublishObjects, true);
    List<string[]> strArrayList = new List<string[]>(1)
    {
      new string[5]
      {
        Convert.ToString(objectType1.ObjectType),
        Convert.ToString(objectType1.ParentTypeID),
        objectType1.ObjectTypeName,
        (objectType1 as IDBGuid).GUID.ToString(),
        objectType1.Icon == null || objectType1.Icon.Length == 0 ? string.Empty : Convert.ToBase64String(objectType1.Icon)
      }
    };
    DataTable dataTable = userSession.GetObjectTypeCollection(objectType1.ObjectType, true).SelectRecursive(string.Empty);
    for (int index = 0; index < dataTable.Rows.Count; ++index)
    {
      IDBObjectType objectType2 = userSession.GetObjectType(Convert.ToInt32(dataTable.Rows[index]["F_OBJECT_TYPE"]));
      strArrayList.Add(new string[5]
      {
        Convert.ToString(objectType2.ObjectType),
        Convert.ToString(objectType2.ParentTypeID),
        objectType2.ObjectTypeName,
        (objectType2 as IDBGuid).GUID.ToString(),
        objectType2.Icon == null || objectType2.Icon.Length == 0 ? string.Empty : Convert.ToBase64String(objectType2.Icon)
      });
    }
    return strArrayList.ToArray();
  }

  public string[][] GetAttributesForPublishObjectType(Guid sessionGuid, int objectTypeID)
  {
    IDBObjectType objectType = this.GetUserSession(sessionGuid).GetObjectType(objectTypeID, true);
    List<string[]> strArrayList = new List<string[]>();
    DataTable dataTable = objectType.Attributes.Select("F_ATTRIBUTE_ID");
    if (dataTable != null && dataTable.Rows.Count > 0)
    {
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        int int32 = Convert.ToInt32(row["F_ATTRIBUTE_ID"]);
        IDBAttributeType4 attributeById = objectType.Attributes.GetAttributeByID(int32);
        if (attributeById != null && (attributeById.Options & AttributeOptions.Internal) == AttributeOptions.None)
          strArrayList.Add(new string[4]
          {
            Convert.ToString(attributeById.AttributeID),
            attributeById.Name,
            (attributeById as IDBGuid).GUID.ToString(),
            Convert.ToString((int) attributeById.AttributeType)
          });
      }
    }
    return strArrayList.Count <= 0 ? (string[][]) null : strArrayList.ToArray();
  }

  public PortalAttributeType[] GetPublishRelationAttributes(Guid sessionGuid)
  {
    IDBRelationType relationType = this.GetUserSession(sessionGuid).GetRelationType(PortalConsts.reltypePublish);
    DataTable dataTable = relationType.Attributes.Select("F_ATTRIBUTE_ID");
    List<PortalAttributeType> portalAttributeTypeList = new List<PortalAttributeType>(dataTable.Rows.Count);
    if (dataTable.Rows.Count > 0)
    {
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        int int32 = Convert.ToInt32(row["F_ATTRIBUTE_ID"]);
        IDBAttributeType4 attributeById = relationType.Attributes.GetAttributeByID(int32);
        if (attributeById != null && (attributeById.Options & AttributeOptions.Internal) == AttributeOptions.None)
          portalAttributeTypeList.Add(new PortalAttributeType(attributeById.AttributeID, attributeById.Name, (attributeById as IDBGuid).GUID.ToString(), attributeById.AttributeType));
      }
    }
    return portalAttributeTypeList.ToArray();
  }

  public AttributePossibleValues[] GetAttributePossibleValues(Guid sessionGuid)
  {
    IUserSession userSession = this.GetUserSession(sessionGuid);
    List<AttributePossibleValues> attributePossibleValuesList = new List<AttributePossibleValues>();
    List<int> addedAttributes = new List<int>();
    List<int> publishObjectTypes = this.GetPublishObjectTypes(userSession);
    for (int index = 0; index < publishObjectTypes.Count; ++index)
    {
      IDBObjectType objectType = userSession.GetObjectType(publishObjectTypes[index]);
      AttributePossibleValues[] posibleValues4Type = this.GetAttributePosibleValues4Type(userSession, (IDBAttributableType) objectType, addedAttributes);
      if (posibleValues4Type.Length != 0)
        attributePossibleValuesList.AddRange((IEnumerable<AttributePossibleValues>) posibleValues4Type);
    }
    IDBRelationType relationType = userSession.GetRelationType(PortalConsts.reltypePublish);
    AttributePossibleValues[] posibleValues4Type1 = this.GetAttributePosibleValues4Type(userSession, (IDBAttributableType) relationType, addedAttributes);
    if (posibleValues4Type1.Length != 0)
      attributePossibleValuesList.AddRange((IEnumerable<AttributePossibleValues>) posibleValues4Type1);
    return attributePossibleValuesList.ToArray();
  }

  public DateTime GetLasModifyMetadata(Guid sessionGuid)
  {
    IUserSession userSession = this.GetUserSession(sessionGuid);
    DateTime lasModifyMetadata = DateTime.MinValue;
    DataTable tablesModifyTime = userSession.ServerCache.GetTablesModifyTime();
    for (int index = 0; index < tablesModifyTime.Rows.Count; ++index)
    {
      string str = Convert.ToString(tablesModifyTime.Rows[index]["F_TABLE_NAME"]);
      if (str == "IMS_OBJECT_TYPES" || str == "IMS_ATTRIBUTES" || str == "IMS_OBJTYPES_TREE" || str == "IMS_ATTR4OBJ_TYPES" || str == "IMS_ATTR4RELATION_TYPES" || str == "IMS_POSSIBLE_VALUES")
      {
        DateTime dateTime = Convert.ToDateTime(tablesModifyTime.Rows[index]["F_MODIFY_DATE"], (IFormatProvider) CultureInfo.InvariantCulture);
        if (dateTime > lasModifyMetadata)
          lasModifyMetadata = dateTime;
      }
    }
    return lasModifyMetadata;
  }

  private PortalObjectType GetPortalObjectType(IDBObjectType objType)
  {
    PortalObjectType portalObjectType = new PortalObjectType(objType.ObjectType, objType.ParentTypeID, objType.ObjectTypeName, (objType as IDBGuid).GUID.ToString(), objType.Icon);
    List<PortalAttributeType> portalAttributeTypeList = new List<PortalAttributeType>();
    DataTable dataTable = objType.Attributes.Select("F_ATTRIBUTE_ID");
    if (dataTable != null && dataTable.Rows.Count > 0)
    {
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        int int32 = Convert.ToInt32(row["F_ATTRIBUTE_ID"]);
        IDBAttributeType4 attributeById = objType.Attributes.GetAttributeByID(int32);
        if (attributeById != null && (attributeById.Options & AttributeOptions.Internal) == AttributeOptions.None)
          portalAttributeTypeList.Add(new PortalAttributeType(attributeById.AttributeID, attributeById.Name, (attributeById as IDBGuid).GUID.ToString(), attributeById.AttributeType));
      }
    }
    portalObjectType.Attributes = portalAttributeTypeList.ToArray();
    return portalObjectType;
  }

  public List<int> GetPublishObjectTypes(IUserSession session)
  {
    List<int> publishObjectTypes = new List<int>();
    IDBObjectType objectType1 = session.GetObjectType(PortalConsts.objtypePublishObjects);
    publishObjectTypes.Add(objectType1.ObjectType);
    DataTable dataTable = session.GetObjectTypeCollection(objectType1.ObjectType).SelectRecursive(string.Empty);
    for (int index = 0; index < dataTable.Rows.Count; ++index)
      publishObjectTypes.Add(Convert.ToInt32(dataTable.Rows[index]["F_OBJECT_TYPE"]));
    IDBObjectType objectType2 = session.GetObjectType(PortalConsts.objtypePacket);
    publishObjectTypes.Add(objectType2.ObjectType);
    return publishObjectTypes;
  }

  private AttributePossibleValues[] GetAttributePosibleValues4Type(
    IUserSession session,
    IDBAttributableType type,
    List<int> addedAttributes)
  {
    List<AttributePossibleValues> attributePossibleValuesList = new List<AttributePossibleValues>();
    DataTable dataTable = type.Attributes.Select(string.Empty);
    for (int index1 = 0; index1 < dataTable.Rows.Count; ++index1)
    {
      IDBAttributeType attributeType = session.GetAttributeType(Convert.ToInt32(dataTable.Rows[index1]["F_ATTRIBUTE_ID"]));
      if ((attributeType.MultipleValued == MultiValueModes.MultiValuesFromList || attributeType.MultipleValued == MultiValueModes.SingleValueFromList) && !addedAttributes.Contains(attributeType.AttributeID))
      {
        DataTable possibleValues = attributeType.GetPossibleValues();
        List<PossibleValue> possibleValueList = new List<PossibleValue>(possibleValues.Rows.Count);
        for (int index2 = 0; index2 < possibleValues.Rows.Count; ++index2)
        {
          PossibleValue possibleValue = new PossibleValue();
          switch (attributeType.ValueFieldName)
          {
            case "F_STRING_VALUE":
              possibleValue.StringValue = Convert.ToString(possibleValues.Rows[index2][1]);
              break;
            case "F_INTEGER_VALUE":
              possibleValue.IntegerValue = Convert.ToInt64(possibleValues.Rows[index2][1]);
              break;
            case "F_DATE_VALUE":
              possibleValue.DateTimeValue = Convert.ToString(possibleValues.Rows[index2][1]);
              break;
            case "F_DOUBLE_VALUE":
              possibleValue.DoubleValue = Convert.ToDouble(possibleValues.Rows[index2][1], (IFormatProvider) CultureInfo.InvariantCulture);
              break;
          }
          possibleValue.ValueFieldName = attributeType.ValueFieldName;
          possibleValue.Description = Convert.ToString(possibleValues.Rows[index2][2]);
          possibleValueList.Add(possibleValue);
        }
        if (possibleValueList.Count > 0)
          attributePossibleValuesList.Add(new AttributePossibleValues(attributeType.AttributeID, possibleValueList.ToArray()));
        addedAttributes.Add(attributeType.AttributeID);
      }
    }
    return attributePossibleValuesList.ToArray();
  }
}
