// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.TableFieldInfoCreator
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.Data;

#nullable disable
namespace Intermech.ImpExp.Interface;

internal sealed class TableFieldInfoCreator
{
  public int idxColumnName = -1;
  public int idxColumnOrdinal = -1;
  public int idxColumnSize = -1;
  public int idxNumericPrecision = -1;
  public int idxNumericScale = -1;
  public int idxDataType = -1;
  public int idxProviderType = -1;
  public int idxIsLong = -1;
  public int idxAllowDBNull = -1;
  public int idxIsReadOnly = -1;
  public int idxIsRowVersion = -1;
  public int idxIsUnique = -1;
  public int idxIsKeyColumn = -1;
  public int idxIsAutoIncrement = -1;
  public int idxBaseSchemaName = -1;
  public int idxBaseCatalogName = -1;
  public int idxBaseTableName = -1;
  public int idxBaseColumnName = -1;

  /// <summary>Конструктор</summary>
  /// <param name="schemaTable">Таблица с метаданными (описание полей требуемой таблицы из схемы базы данных)</param>
  public TableFieldInfoCreator(DataTable schemaTable)
  {
    this.idxColumnName = schemaTable.Columns.IndexOf("ColumnName");
    this.idxColumnOrdinal = schemaTable.Columns.IndexOf("ColumnOrdinal");
    this.idxColumnSize = schemaTable.Columns.IndexOf("ColumnSize");
    this.idxNumericPrecision = schemaTable.Columns.IndexOf("NumericPrecision");
    this.idxNumericScale = schemaTable.Columns.IndexOf("NumericScale");
    this.idxDataType = schemaTable.Columns.IndexOf("DataType");
    this.idxProviderType = schemaTable.Columns.IndexOf("ProviderType");
    this.idxIsLong = schemaTable.Columns.IndexOf("IsLong");
    this.idxAllowDBNull = schemaTable.Columns.IndexOf("AllowDBNull");
    this.idxIsReadOnly = schemaTable.Columns.IndexOf("IsReadOnly");
    this.idxIsRowVersion = schemaTable.Columns.IndexOf("IsRowVersion");
    this.idxIsUnique = schemaTable.Columns.IndexOf("IsUnique");
    this.idxIsKeyColumn = schemaTable.Columns.IndexOf("IsKeyColumn");
    if (this.idxIsKeyColumn == -1)
      this.idxIsKeyColumn = schemaTable.Columns.IndexOf("IsKey");
    this.idxIsAutoIncrement = schemaTable.Columns.IndexOf("IsAutoIncrement");
    this.idxBaseSchemaName = schemaTable.Columns.IndexOf("BaseSchemaName");
    this.idxBaseCatalogName = schemaTable.Columns.IndexOf("BaseCatalogName");
    this.idxBaseTableName = schemaTable.Columns.IndexOf("BaseTableName");
    this.idxBaseColumnName = schemaTable.Columns.IndexOf("BaseColumnName");
  }

  /// <summary>
  /// Создание объекта содержащего информацию о поле таблицы
  /// </summary>
  /// <param name="dataRow">Строка таблицы с метаданными, на основе которой будет создан новый объект</param>
  /// <returns>Объект содержащий информацию о поле таблицы</returns>
  public ITableFieldInfo CreateTableFieldInfo(DataRow dataRow)
  {
    return (ITableFieldInfo) new TableFieldInfo(this, dataRow);
  }
}
