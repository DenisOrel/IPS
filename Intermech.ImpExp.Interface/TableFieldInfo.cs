// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.TableFieldInfo
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>Класс с описанием поля из таблицы данных</summary>
internal sealed class TableFieldInfo : ITableFieldInfo
{
  /// <summary>
  /// The name of the column; this might not be unique. If this cannot be determined,
  /// a null value is returned. This name always reflects the most recent renaming of
  /// the column in the current view or command text.
  /// </summary>
  public string ColumnName { get; } = "";

  /// <summary>
  /// The ordinal of the column. This is zero for the bookmark column of the row, if any.
  /// Other columns are numbered starting with one. This column cannot contain a null value.
  /// </summary>
  public int ColumnOrdinal { get; } = -1;

  /// <summary>
  /// The maximum possible length of a value in the column.
  /// For columns that use a fixed-length data type, this is the size of the data type.
  /// </summary>
  public int ColumnSize { get; } = -1;

  /// <summary>
  /// If ProviderType is a numeric data type, this is the maximum precision of the column.
  /// The precision depends on the definition of the column. If ProviderType is not a numeric
  /// data type, this is a null value.
  /// </summary>
  public int NumericPrecision { get; } = -1;

  /// <summary>
  /// If ProviderType is is DBTYPE_DECIMAL or DBTYPE_NUMERIC, the number of digits to the right
  /// of the decimal point. Otherwise, this is a null value.
  /// </summary>
  public int NumericScale { get; } = -1;

  /// <summary>Maps to the .NET Framework type of the column.</summary>
  public Type DataType { get; }

  /// <summary>
  /// The indicator of the column's data type. If the data type of the column varies from row to row,
  /// this must be Object. This column cannot contain a null value.
  /// </summary>
  public string ProviderType { get; } = "";

  /// <summary>
  /// Set if the column contains a Binary Long Object (BLOB) that contains very long data.
  /// The definition of very long data is provider-specific. The setting of this flag
  /// corresponds to the value of the IS_LONG column in the PROVIDER_TYPES rowset for the data type.
  /// </summary>
  public bool IsLong { get; }

  /// <summary>
  /// Set if the consumer can set the column to a null value, or if the provider
  /// cannot determine whether or not the consumer can set the column to a null value.
  /// Otherwise, not set. A column may contain null values, even if it cannot be set to a null value.
  /// </summary>
  public bool AllowDBNull { get; }

  /// <summary>
  /// true if the column cannot be modified; otherwise false.
  /// </summary>
  public bool IsReadOnly { get; }

  /// <summary>
  /// Set if the column contains a persistent row identifier that cannot be written to,
  /// and has no meaningful value except to identity the row.
  /// </summary>
  public bool IsRowVersion { get; }

  /// <summary>
  /// true if no two rows in the base table-the table returned in BaseTableName-can have
  /// the same value in this column. IsUnique is guaranteed to be true if the column constitutes
  /// a key by itself or if there is a constraint of type UNIQUE that applies only to this column.
  /// false if the column can contain duplicate values in the base table. The default of this column is false.
  /// </summary>
  public bool IsUnique { get; }

  /// <summary>
  /// true if the column is one of a set of columns in the rowset that, taken together, uniquely identify the row.
  /// The set of columns with IsKeyColumn set to true must uniquely identify a row in the rowset.
  /// There is no requirement that this set of columns is a minimal set of columns.
  /// This set of columns may be generated from a base table primary key, a unique constraint or a unique index.
  /// false if the column is not required to uniquely identify the row.
  /// </summary>
  public bool IsKeyColumn { get; }

  /// <summary>
  /// VARIANT_TRUE: The column assigns values to new rows in fixed increments.
  /// VARIANT_FALSE: The column does not assign values to new rows in fixed increments.
  /// The default of this column is VARIANT_FALSE.
  /// </summary>
  public bool IsAutoIncrement { get; }

  /// <summary>
  /// The name of the schema in the data store that contains the column.
  /// A null value if the base schema name cannot be determined.
  /// The default of this column is a null value.
  /// </summary>
  public string BaseSchemaName { get; } = "";

  /// <summary>
  /// The name of the catalog in the data store that contains the column.
  /// A null value if the base catalog name cannot be determined.
  /// The default of this column is a null value.
  /// </summary>
  public string BaseCatalogName { get; } = "";

  /// <summary>
  /// The name of the table or view in the data store that contains the column.
  /// A null value if the base table name cannot be determined.
  /// The default of this column is a null value.
  /// </summary>
  public string BaseTableName { get; } = "";

  /// <summary>
  /// The name of the column in the data store. This might be different than the column name
  /// returned in the ColumnName column if an alias was used. A null value if the base column
  /// name cannot be determined or if the rowset column is derived, but not identical to,
  /// a column in the data store. The default of this column is a null value.
  /// </summary>
  public string BaseColumnName { get; } = "";

  /// <summary>Конструктор</summary>
  /// <param name="creator">Объект с данными о полях таблицы с метаданными</param>
  /// <param name="dr">Строка таблицы с метаданными, на основе которой будет создан новый объект</param>
  public TableFieldInfo(TableFieldInfoCreator creator, DataRow dr)
  {
    if (creator.idxColumnName > -1)
      this.ColumnName = Convert.ToString(dr[creator.idxColumnName]).ToUpper();
    if (creator.idxColumnOrdinal > -1)
      this.ColumnOrdinal = Convert.ToInt32(dr[creator.idxColumnOrdinal]);
    if (creator.idxColumnSize > -1)
      this.ColumnSize = Convert.ToInt32(dr[creator.idxColumnSize]);
    if (creator.idxNumericPrecision > -1 && dr[creator.idxNumericPrecision] != DBNull.Value)
      this.NumericPrecision = Convert.ToInt32(dr[creator.idxNumericPrecision]);
    if (creator.idxNumericScale > -1 && dr[creator.idxNumericScale] != DBNull.Value)
      this.NumericScale = Convert.ToInt32(dr[creator.idxNumericScale]);
    if (creator.idxDataType > -1)
      this.DataType = (Type) dr[creator.idxDataType];
    if (creator.idxProviderType > -1)
      this.ProviderType = Convert.ToString(dr[creator.idxProviderType]);
    if (creator.idxIsLong > -1)
      this.IsLong = Convert.ToBoolean(dr[creator.idxIsLong]);
    if (creator.idxAllowDBNull > -1)
      this.AllowDBNull = Convert.ToBoolean(dr[creator.idxAllowDBNull]);
    if (creator.idxIsReadOnly > -1)
      this.IsReadOnly = Convert.ToBoolean(dr[creator.idxIsReadOnly]);
    if (creator.idxIsRowVersion > -1)
      this.IsRowVersion = Convert.ToBoolean(dr[creator.idxIsRowVersion]);
    if (creator.idxIsUnique > -1)
      this.IsUnique = !DBNull.Value.Equals(dr[creator.idxIsUnique]) && Convert.ToBoolean(dr[creator.idxIsUnique]);
    if (creator.idxIsKeyColumn > -1 && dr[creator.idxIsKeyColumn] != DBNull.Value)
      this.IsKeyColumn = Convert.ToBoolean(dr[creator.idxIsKeyColumn]);
    if (creator.idxIsAutoIncrement > -1)
      this.IsAutoIncrement = Convert.ToBoolean(dr[creator.idxIsAutoIncrement]);
    if (creator.idxBaseSchemaName > -1)
      this.BaseSchemaName = Convert.ToString(dr[creator.idxBaseSchemaName]);
    if (creator.idxBaseCatalogName > -1)
      this.BaseCatalogName = Convert.ToString(dr[creator.idxBaseCatalogName]);
    if (creator.idxBaseTableName > -1)
      this.BaseTableName = Convert.ToString(dr[creator.idxBaseTableName]);
    if (creator.idxBaseColumnName <= -1)
      return;
    this.BaseColumnName = Convert.ToString(dr[creator.idxBaseColumnName]);
  }
}
