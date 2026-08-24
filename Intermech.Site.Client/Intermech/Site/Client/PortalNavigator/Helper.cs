// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.Helper
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class Helper
{
  public static Type GetType(TypeCode typeCode)
  {
    switch (typeCode)
    {
      case TypeCode.Empty:
        return typeof (DBNull);
      case TypeCode.Object:
        return typeof (object);
      case TypeCode.DBNull:
        return typeof (DBNull);
      case TypeCode.Boolean:
        return typeof (bool);
      case TypeCode.Char:
        return typeof (char);
      case TypeCode.SByte:
        return typeof (sbyte);
      case TypeCode.Byte:
        return typeof (byte);
      case TypeCode.Int16:
        return typeof (short);
      case TypeCode.UInt16:
        return typeof (ushort);
      case TypeCode.Int32:
        return typeof (int);
      case TypeCode.UInt32:
        return typeof (uint);
      case TypeCode.Int64:
        return typeof (long);
      case TypeCode.UInt64:
        return typeof (ulong);
      case TypeCode.Single:
        return typeof (float);
      case TypeCode.Double:
        return typeof (double);
      case TypeCode.Decimal:
        return typeof (Decimal);
      case TypeCode.DateTime:
        return typeof (DateTime);
      case TypeCode.String:
        return typeof (string);
      default:
        return typeof (DBNull);
    }
  }

  public static NodeColumnCollection GetPublicUserColumns()
  {
    IColumnSchemes service = (IColumnSchemes) ServicesManager.GetService(typeof (IColumnSchemes));
    return new NodeColumnCollection()
    {
      service.CreateColumn(SiteClientConsts.PublishUserObligatoryColumnSchemeGuid, (object) new Guid("cad0001d-306c-11d8-b4e9-00304f19f545")),
      service.CreateColumn(SiteClientConsts.PublishUserObligatoryColumnSchemeGuid, (object) new Guid("cad00018-306c-11d8-b4e9-00304f19f545"))
    };
  }

  public static NodeColumnCollection GetPublishedObjectColumns()
  {
    IColumnSchemes service = (IColumnSchemes) ServicesManager.GetService(typeof (IColumnSchemes));
    return new NodeColumnCollection()
    {
      service.CreateColumn(SiteClientConsts.PublishObjectObligatoryColumnSchemeGuid, (object) ObligatoryObjectAttributes.F_OBJECT_TYPE),
      service.CreateColumn(SiteClientConsts.PublishObjectObligatoryColumnSchemeGuid, (object) ObligatoryObjectAttributes.F_OBJECT_ID),
      service.CreateColumn(SiteClientConsts.PublishObjectObligatoryColumnSchemeGuid, (object) ObligatoryObjectAttributes.CAPTION)
    };
  }

  public static NodeColumnCollection GetPublishedPacketColumns()
  {
    IColumnSchemes service = (IColumnSchemes) ServicesManager.GetService(typeof (IColumnSchemes));
    return new NodeColumnCollection()
    {
      service.CreateColumn(SiteClientConsts.PublishPacketsObligatoryColumnSchemeGuid, (object) ObligatoryObjectAttributes.F_OBJECT_TYPE),
      service.CreateColumn(SiteClientConsts.PublishPacketsObligatoryColumnSchemeGuid, (object) ObligatoryObjectAttributes.F_OBJECT_ID),
      service.CreateColumn(SiteClientConsts.PublishPacketsObligatoryColumnSchemeGuid, (object) ObligatoryObjectAttributes.CAPTION),
      service.CreateColumn(SiteClientConsts.PublishPacketsObligatoryColumnSchemeGuid, (object) ObligatoryObjectAttributes.F_OBJ_CREATE),
      service.CreateColumn(SiteClientConsts.PublishPacketsObligatoryColumnSchemeGuid, (object) new Guid("cad0001f-306c-11d8-b4e9-00304f19f545")),
      service.CreateColumn(SiteClientConsts.PublishPacketsObligatoryColumnSchemeGuid, (object) new Guid("cad00020-306c-11d8-b4e9-00304f19f545")),
      service.CreateColumn(SiteClientConsts.PublishPacketsObligatoryColumnSchemeGuid, (object) PortalConsts.attributeFirstPublishSite),
      service.CreateColumn(SiteClientConsts.PublishPacketsObligatoryColumnSchemeGuid, (object) PortalConsts.attributePacketNote)
    };
  }

  public static NodeColumnCollection GetPublicObjectCaptionOnlyColumns()
  {
    IColumnSchemes service = (IColumnSchemes) ServicesManager.GetService(typeof (IColumnSchemes));
    NodeColumnCollection captionOnlyColumns = new NodeColumnCollection();
    Guid columnSchemeGuid = SiteClientConsts.PublishObjectObligatoryColumnSchemeGuid;
    // ISSUE: variable of a boxed type
    __Boxed<ObligatoryObjectAttributes> columnID = (Enum) ObligatoryObjectAttributes.CAPTION;
    NodeColumn column = service.CreateColumn(columnSchemeGuid, (object) columnID);
    captionOnlyColumns.Add(column);
    return captionOnlyColumns;
  }

  public static NodeColumnCollection GetPublicRelationColumns()
  {
    IColumnSchemes service = (IColumnSchemes) ServicesManager.GetService(typeof (IColumnSchemes));
    NodeColumnCollection publicRelationColumns = new NodeColumnCollection();
    Guid columnSchemeGuid = SiteClientConsts.PublishRelationColumnSchemeGuid;
    // ISSUE: variable of a boxed type
    __Boxed<ObligatoryObjectAttributes> columnID = (Enum) ObligatoryObjectAttributes.F_PRJLINK_ID;
    NodeColumn column = service.CreateColumn(columnSchemeGuid, (object) columnID);
    publicRelationColumns.Add(column);
    return publicRelationColumns;
  }

  internal static object GetAttributeValue(
    IUserSession session,
    IPortalMetadata metadata,
    PublishAttribute attr,
    ref string name,
    ref Type type,
    ref DataErrors errors)
  {
    object attributeValue1 = (object) null;
    if (attr != null && attr.Info != null)
    {
      switch (attr.Info.Name)
      {
        case "CAPTION":
          if (attr.Values != null && attr.Values.Length != 0)
            attributeValue1 = (object) attr.Values[0].StringValue;
          name = EnumDescConverter.GetEnumDescription((Enum) ObligatoryObjectAttributes.CAPTION);
          break;
        case "F_CREATE_DATE":
          if (attr.Values != null && attr.Values.Length != 0 && attr.Values[0].DateTimeValue != string.Empty)
            attributeValue1 = (object) Convert.ToDateTime(attr.Values[0].DateTimeValue, (IFormatProvider) CultureInfo.InvariantCulture);
          name = EnumDescConverter.GetEnumDescription((Enum) ObligatoryObjectAttributes.F_CREATE_DATE);
          type = typeof (DateTime);
          break;
        case "F_GUID":
          if (attr.Values != null && attr.Values.Length != 0 && GuidHelper.IsGuid(attr.Values[0].GuidValue))
            attributeValue1 = (object) attr.Values[0].GuidValue;
          name = LocalizationHolder.rm.GetString("Site.Client_10");
          if (attr.Category == PublishAttributeCategory.Object)
          {
            name += LocalizationHolder.rm.GetString("Site.Client_11");
            break;
          }
          if (attr.Category == PublishAttributeCategory.PublishObject)
          {
            name += LocalizationHolder.rm.GetString("Site.Client_12");
            break;
          }
          if (attr.Category == PublishAttributeCategory.Relation)
          {
            name += LocalizationHolder.rm.GetString("Site.Client_13");
            break;
          }
          if (attr.Category == PublishAttributeCategory.PublishRelation)
          {
            name += LocalizationHolder.rm.GetString("Site.Client_14");
            break;
          }
          break;
        case "F_ID":
          name = LocalizationHolder.rm.GetString("Site.Client_18");
          if (attr.Values != null && attr.Values.Length != 0)
          {
            attributeValue1 = (object) attr.Values[0].IntegerValue;
            break;
          }
          break;
        case "F_LC_STEP":
          if (attr.Values != null && attr.Values.Length != 0 && GuidHelper.IsGuid(attr.Values[0].GuidValue))
          {
            IDBLifecycleStep lifecycleStep = session.GetLifecycleStep(new Guid(attr.Values[0].GuidValue), false);
            attributeValue1 = lifecycleStep != null ? (object) lifecycleStep.LCName : (object) attr.Values[0].GuidValue;
          }
          name = EnumDescConverter.GetEnumDescription((Enum) ObligatoryObjectAttributes.F_LC_STEP);
          break;
        case "F_LEVEL_ID":
          if (attr.Values != null && attr.Values.Length != 0 && GuidHelper.IsGuid(attr.Values[0].GuidValue))
          {
            IDBLifecycleLevelType lifecycleLevel = session.GetLifecycleLevel(new Guid(attr.Values[0].GuidValue), false);
            attributeValue1 = lifecycleLevel != null ? (object) lifecycleLevel.LevelName : (object) attr.Values[0].GuidValue;
          }
          name = EnumDescConverter.GetEnumDescription((Enum) ObligatoryObjectAttributes.F_LEVEL_ID);
          break;
        case "F_OBJECT_GUID":
          if (attr.Values != null && attr.Values.Length != 0 && GuidHelper.IsGuid(attr.Values[0].GuidValue))
            attributeValue1 = (object) attr.Values[0].GuidValue;
          name = LocalizationHolder.rm.GetString("Site.Client_15");
          break;
        case "F_OBJECT_ID":
          name = LocalizationHolder.rm.GetString("Site.Client_17");
          if (attr.Values != null && attr.Values.Length != 0)
          {
            attributeValue1 = (object) attr.Values[0].IntegerValue;
            break;
          }
          break;
        case "F_OBJECT_TYPE":
          name = LocalizationHolder.rm.GetString("Site.Client_19");
          if (attr.Values != null && attr.Values.Length != 0 && metadata != null)
          {
            PortalObjectType publishObjectType = metadata.GetPublishObjectType(Convert.ToInt32(attr.Values[0].IntegerValue));
            if (publishObjectType != null)
            {
              attributeValue1 = (object) publishObjectType.Name;
              break;
            }
            break;
          }
          break;
        case "F_OBJTYPE_GUID":
          if (attr.Values != null && attr.Values.Length != 0 && GuidHelper.IsGuid(attr.Values[0].GuidValue))
            attributeValue1 = (object) attr.Values[0].GuidValue;
          name = LocalizationHolder.rm.GetString("Site.Client_21");
          break;
        case "F_OBJ_CREATE":
          if (attr.Values != null && attr.Values.Length != 0 && attr.Values[0].DateTimeValue != string.Empty)
            attributeValue1 = (object) Convert.ToDateTime(attr.Values[0].DateTimeValue, (IFormatProvider) CultureInfo.InvariantCulture);
          name = EnumDescConverter.GetEnumDescription((Enum) ObligatoryObjectAttributes.F_OBJ_CREATE);
          type = typeof (DateTime);
          break;
        case "F_OBJ_TYPE_NAME":
          if (attr.Values != null && attr.Values.Length != 0 && attr.Values[0].StringValue != string.Empty)
            attributeValue1 = (object) attr.Values[0].StringValue;
          name = LocalizationHolder.rm.GetString("Site.Client_22");
          break;
        case "F_OWNER_ID":
          if (attr.Values != null && attr.Values.Length != 0 && GuidHelper.IsGuid(attr.Values[0].GuidValue))
          {
            IDBObject dbObject = session.GetObject(new Guid(attr.Values[0].GuidValue), false);
            attributeValue1 = dbObject != null ? (object) dbObject.Caption : (object) attr.Values[0].GuidValue;
          }
          name = EnumDescConverter.GetEnumDescription((Enum) ObligatoryObjectAttributes.F_OWNER_ID);
          break;
        case "F_PARENT_GUID":
          if (attr.Values != null && attr.Values.Length != 0 && GuidHelper.IsGuid(attr.Values[0].GuidValue))
            attributeValue1 = (object) attr.Values[0].GuidValue;
          name = LocalizationHolder.rm.GetString("Site.Client_16");
          break;
        case "F_PRJLINK_ID":
          name = LocalizationHolder.rm.GetString("Site.Client_20");
          if (attr.Values != null && attr.Values.Length != 0)
          {
            attributeValue1 = (object) attr.Values[0].IntegerValue;
            break;
          }
          break;
        case "F_PROJECT_ID":
          if (attr.Values != null && attr.Values.Length != 0 && GuidHelper.IsGuid(attr.Values[0].GuidValue))
          {
            IDBObject dbObject = session.GetObject(new Guid(attr.Values[0].GuidValue), false);
            attributeValue1 = dbObject != null ? (object) dbObject.Caption : (object) attr.Values[0].GuidValue;
          }
          name = EnumDescConverter.GetEnumDescription((Enum) ObligatoryObjectAttributes.F_PROJECT_ID);
          break;
        case "F_RELATION_TYPE_GUID":
          if (attr.Values != null && attr.Values.Length != 0 && GuidHelper.IsGuid(attr.Values[0].GuidValue))
            attributeValue1 = (object) attr.Values[0].GuidValue;
          name = LocalizationHolder.rm.GetString("Site.Client_23");
          break;
        case "F_RELATION_TYPE_NAME":
          if (attr.Values != null && attr.Values.Length != 0 && attr.Values[0].StringValue != string.Empty)
            attributeValue1 = (object) attr.Values[0].StringValue;
          name = LocalizationHolder.rm.GetString("Site.Client_24");
          break;
        default:
          bool flag1 = false;
          if (attr.Info.Guid != null && GuidHelper.IsGuid(attr.Info.Guid))
          {
            IDBAttributeType attributeType = session.GetAttributeType(new Guid(attr.Info.Guid), false);
            if (attributeType != null)
            {
              name = attributeType.Name;
              flag1 = true;
            }
          }
          if (!flag1)
          {
            if (attr.Info.Name != null && attr.Info.Name != string.Empty)
              name = attr.Info.Name;
            else if (attr.Info.ShortName != null && attr.Info.ShortName != string.Empty)
              name = attr.Info.ShortName;
            else if (attr.Info.Alias != null && attr.Info.Alias != string.Empty)
              name = attr.Info.Alias;
            else if (attr.Info.Guid != null && GuidHelper.IsGuid(attr.Info.Guid))
            {
              name = attr.Info.Guid;
            }
            else
            {
              name = LocalizationHolder.rm.GetString("Site.Client_25");
              errors |= DataErrors.ErrorAttribute;
            }
          }
          if (attr.Values != null && attr.Values.Length != 0)
          {
            List<object> objectList = new List<object>(attr.Values.Length);
            for (int index = 0; index < attr.Values.Length; ++index)
            {
              AttributeValue attributeValue2 = attr.Values[index];
              if (attributeValue2.Description != string.Empty && attr.Info.FieldType != FieldTypes.ftObjectLinkByID)
                objectList.Add((object) attributeValue2.Description);
              else if (attributeValue2.IsEmpty)
              {
                objectList.Add((object) null);
              }
              else
              {
                switch (attr.Info.FieldType)
                {
                  case FieldTypes.ftString:
                  case FieldTypes.ftMemo:
                    objectList.Add((object) attributeValue2.StringValue);
                    continue;
                  case FieldTypes.ftInteger:
                  case FieldTypes.ftAutoInc:
                    long result1 = long.MinValue;
                    if (attributeValue2.IntegerValue != long.MinValue)
                      result1 = attributeValue2.IntegerValue;
                    else if (attributeValue2.DoubleValue != double.MinValue && Math.Ceiling(attributeValue2.DoubleValue) - Math.Floor(attributeValue2.DoubleValue) == 0.0)
                      result1 = Convert.ToInt64(attributeValue2.DoubleValue);
                    else if (attributeValue2.StringValue != string.Empty)
                      long.TryParse(attributeValue2.StringValue, out result1);
                    if (result1 != long.MinValue)
                      objectList.Add((object) result1);
                    type = typeof (long);
                    continue;
                  case FieldTypes.ftDouble:
                    double result2 = double.MinValue;
                    if (attributeValue2.DoubleValue != double.MinValue)
                      result2 = attributeValue2.DoubleValue;
                    else if (attributeValue2.IntegerValue != long.MinValue)
                      result2 = Convert.ToDouble(attributeValue2.IntegerValue);
                    else if (attributeValue2.StringValue != string.Empty)
                      double.TryParse(attributeValue2.StringValue, out result2);
                    if (result2 != double.MinValue)
                      objectList.Add((object) result2);
                    type = typeof (double);
                    continue;
                  case FieldTypes.ftDateTime:
                    if (attributeValue2.DateTimeValue != string.Empty)
                      objectList.Add((object) Convert.ToDateTime(attr.Values[0].DateTimeValue, (IFormatProvider) CultureInfo.InvariantCulture));
                    type = typeof (DateTime);
                    continue;
                  case FieldTypes.ftShortBlob:
                  case FieldTypes.ftBlob:
                    string empty1 = string.Empty;
                    string str1 = attributeValue2.FileName == null || !(attributeValue2.FileName != string.Empty) ? (attributeValue2.StringValue == null || !(attributeValue2.StringValue != string.Empty) ? LocalizationHolder.rm.GetString("Site.Client_104") : attributeValue2.StringValue) : attributeValue2.FileName;
                    objectList.Add((object) string.Format(LocalizationHolder.rm.GetString("Site.Client_105"), (object) str1, (object) attributeValue2.IntegerValue));
                    continue;
                  case FieldTypes.ftFile:
                    string empty2 = string.Empty;
                    string str2 = attributeValue2.StringValue == null || !(attributeValue2.StringValue != string.Empty) ? (attributeValue2.FileName == null || !(attributeValue2.FileName != string.Empty) ? LocalizationHolder.rm.GetString("Site.Client_30") : attributeValue2.FileName) : attributeValue2.StringValue;
                    objectList.Add((object) string.Format(LocalizationHolder.rm.GetString("Site.Client_31"), (object) str2, (object) attributeValue2.IntegerValue));
                    continue;
                  case FieldTypes.ftObjectLink:
                    if (attributeValue2.GuidValue != null && GuidHelper.IsGuid(attributeValue2.GuidValue))
                    {
                      if (string.IsNullOrEmpty(attributeValue2.StringValue))
                      {
                        IDBObject dbObject = session.GetObject(new Guid(attributeValue2.GuidValue), false);
                        if (dbObject != null)
                        {
                          objectList.Add(dbObject.Caption != string.Empty ? (object) dbObject.Caption : (object) attributeValue2.GuidValue);
                          continue;
                        }
                        objectList.Add((object) attributeValue2.GuidValue);
                        continue;
                      }
                      objectList.Add((object) attributeValue2.StringValue);
                      continue;
                    }
                    continue;
                  case FieldTypes.ftPassword:
                    objectList.Add((object) "**********");
                    continue;
                  case FieldTypes.ftBoolean:
                    long num = long.MinValue;
                    if (attributeValue2.IntegerValue != long.MinValue)
                      num = attributeValue2.IntegerValue;
                    else if (attributeValue2.DoubleValue != double.MinValue && Math.Ceiling(attributeValue2.DoubleValue) - Math.Floor(attributeValue2.DoubleValue) == 0.0)
                      num = Convert.ToInt64(attributeValue2.DoubleValue);
                    if (num == long.MinValue && attributeValue2.StringValue != string.Empty)
                    {
                      string upper = attributeValue2.StringValue.ToUpper();
                      if (upper == LocalizationHolder.rm.GetString("Site.Client_26") || upper == "TRUE" || upper == LocalizationHolder.rm.GetString("Site.Client_27"))
                        num = 1L;
                      else if (upper == LocalizationHolder.rm.GetString("Site.Client_28") || upper == "FALSE" || upper == LocalizationHolder.rm.GetString("Site.Client_29"))
                        num = 0L;
                    }
                    if (num == 0L || num == 1L)
                    {
                      objectList.Add(num == 0L ? (object) Consts.NoValue : (object) Consts.YesValue);
                      continue;
                    }
                    continue;
                  case FieldTypes.ftMeasured:
                    if (MeasureHelper.Measures == null || MeasureHelper.Measures.Length == 0)
                      MeasureHelper.Init(session.GetMeasuresList());
                    MeasureDescriptor measureDescriptor = (MeasureDescriptor) null;
                    bool flag2 = false;
                    if (attributeValue2.GuidValue != string.Empty)
                    {
                      IDBObject dbObject = session.GetObject(new Guid(attributeValue2.GuidValue), false);
                      if (dbObject != null)
                        measureDescriptor = MeasureHelper.FindDescriptor(dbObject.ObjectID);
                    }
                    if (attributeValue2.StringValue != string.Empty)
                    {
                      MeasuredValue measuredValue = MeasureHelper.ConvertToMeasuredValue(attributeValue2.StringValue, false);
                      if (measuredValue != null && measuredValue.MeasureID != 0L && measuredValue.MeasureID != -1L)
                      {
                        MeasureDescriptor descriptor = MeasureHelper.FindDescriptor(measuredValue.MeasureID);
                        if (descriptor != null && !descriptor.Empty)
                        {
                          if (measureDescriptor == null || measureDescriptor.Empty)
                          {
                            measureDescriptor = descriptor;
                            flag2 = true;
                          }
                          else if (measureDescriptor.PhysicalQuantityID == descriptor.PhysicalQuantityID)
                            flag2 = true;
                        }
                      }
                    }
                    if (measureDescriptor != null && !measureDescriptor.Empty && attributeValue2.DoubleValue != double.MinValue)
                    {
                      MeasuredValue measuredValue = new MeasuredValue(attributeValue2.DoubleValue, measureDescriptor.MeasureID);
                      measuredValue.Caption = !flag2 ? MeasureHelper.ConvertToString(measuredValue.Value, measuredValue.MeasureID, false) : attributeValue2.StringValue;
                      objectList.Add((object) measuredValue.Caption);
                      continue;
                    }
                    continue;
                  case FieldTypes.ftGuid:
                    if (GuidHelper.IsGuid(attributeValue2.GuidValue))
                    {
                      objectList.Add((object) attributeValue2.GuidValue);
                      continue;
                    }
                    continue;
                  case FieldTypes.ftObjectLinkByID:
                    if (attributeValue2.GuidValue != null && GuidHelper.IsGuid(attributeValue2.GuidValue))
                    {
                      objectList.Add(string.IsNullOrEmpty(attributeValue2.StringValue) ? (object) attributeValue2.GuidValue : (object) attributeValue2.StringValue);
                      continue;
                    }
                    continue;
                  default:
                    continue;
                }
              }
            }
            attributeValue1 = objectList.Count == 1 ? objectList[0] : (object) objectList.ToArray();
            break;
          }
          break;
      }
    }
    else
      errors |= DataErrors.ErrorAttribute;
    return attributeValue1;
  }

  public static DataTable ConvertToDataTable(PublishObjectsTable table)
  {
    DataTable dataTable = new DataTable(table.Name);
    for (int index = 0; index < table.Columns.Length; ++index)
      dataTable.Columns.Add(new DataColumn(table.Columns[index].Name, Helper.GetType((TypeCode) table.Columns[index].TypeCode)));
    for (int index1 = 0; index1 < table.Rows.Length; ++index1)
    {
      DataRow row = dataTable.NewRow();
      for (int index2 = 0; index2 < table.Columns.Length; ++index2)
      {
        object obj = table[index1][index2];
        row[index2] = obj != null ? table[index1][index2] : (object) DBNull.Value;
      }
      dataTable.Rows.Add(row);
    }
    dataTable.ExtendedProperties[(object) "Eof"] = (object) true;
    dataTable.AcceptChanges();
    return dataTable;
  }
}
