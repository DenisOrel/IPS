// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.PumpAttributesHelper
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Interface;

public static class PumpAttributesHelper
{
  public const string SchemaColumnName = "ColumnName";
  public const string SchemaColumnOrdinal = "ColumnOrdinal";
  public const string SchemaColumnSize = "ColumnSize";
  public const string SchemaNumericPrecision = "NumericPrecision";
  public const string SchemaNumericScale = "NumericScale";
  public const string SchemaDataType = "DataType";
  public const string SchemaProviderType = "ProviderType";
  public const string SchemaIsLong = "IsLong";
  public const string SchemaAllowDBNull = "AllowDBNull";
  public const string SchemaIsReadOnly = "IsReadOnly";
  public const string SchemaIsRowVersion = "IsRowVersion";
  public const string SchemaIsUnique = "IsUnique";
  public const string SchemaIsKeyColumn = "IsKeyColumn";
  public const string SchemaIsKeyColumn2 = "IsKey";
  public const string SchemaIsAutoIncrement = "IsAutoIncrement";
  public const string SchemaBaseSchemaName = "BaseSchemaName";
  public const string SchemaBaseCatalogName = "BaseCatalogName";
  public const string SchemaBaseTableName = "BaseTableName";
  public const string SchemaBaseColumnName = "BaseColumnName";

  public static FieldTypes GetFieldTypeFromSchemaRow(DataRow dr, out int size)
  {
    string str = Convert.ToString(dr["DataType"]);
    int int32 = !dr["NumericScale"].Equals((object) DBNull.Value) ? Convert.ToInt32(dr["NumericScale"]) : 0;
    bool flag = !dr["IsLong"].Equals((object) DBNull.Value) && Convert.ToBoolean(dr["IsLong"]);
    FieldTypes typeFromSchemaRow;
    switch (str)
    {
      case "System.Decimal":
        typeFromSchemaRow = int32 == 0 ? FieldTypes.ftInteger : FieldTypes.ftDouble;
        break;
      case "System.Int32":
        typeFromSchemaRow = FieldTypes.ftInteger;
        break;
      case "System.String":
        typeFromSchemaRow = flag ? FieldTypes.ftMemo : FieldTypes.ftString;
        break;
      case "System.DateTime":
        typeFromSchemaRow = FieldTypes.ftDateTime;
        break;
      case "System.Double":
        typeFromSchemaRow = FieldTypes.ftDouble;
        break;
      default:
        typeFromSchemaRow = FieldTypes.ftUnknown;
        break;
    }
    size = typeFromSchemaRow == FieldTypes.ftString ? Convert.ToInt32(dr["ColumnSize"]) : 0;
    return typeFromSchemaRow;
  }
}
