// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.PumpItemFactory
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>
/// Базовый класс для создания на его основе вспомогательных классов для закачки данных из таблиц
/// </summary>
public class PumpItemFactory
{
  /// <summary>Список полей в таблице ()</summary>
  protected Dictionary<string, ITableFieldInfo> nameFieldsInfo = new Dictionary<string, ITableFieldInfo>();
  protected Dictionary<int, ITableFieldInfo> indexFieldsInfo = new Dictionary<int, ITableFieldInfo>();
  /// <summary>
  /// кодирвка базы - для корректной закачки текста из blob-ов
  /// </summary>
  protected Encoding dataBaseEncoding = Encoding.GetEncoding(1251);
  /// <summary>
  /// Имя таблицы по записям которой создаются объекты (нужно для формирования сообщения при ошибках)
  /// </summary>
  protected string tableName = "";
  /// <summary>Ссылка на менеджер приложения</summary>
  protected IAppManager appMngr;

  protected int getFieldIndex(string fieldName)
  {
    string upper = fieldName.ToUpper();
    return !this.nameFieldsInfo.ContainsKey(upper) ? -1 : this.nameFieldsInfo[upper].ColumnOrdinal;
  }

  /// <summary>Конструктор</summary>
  /// <param name="tableName">Наименование таблицы</param>
  /// <param name="dataReader">Набор данных, на основе записей которого будут создаваться новые объекты</param>
  /// <param name="appManager">Интерфейс менеджера приложения</param>
  public PumpItemFactory(string tableName, IDataReader dataReader, IAppManager appManager)
  {
    this.tableName = tableName;
    this.appMngr = appManager;
    DataTable schemaTable = dataReader.GetSchemaTable();
    TableFieldInfoCreator fieldInfoCreator = new TableFieldInfoCreator(schemaTable);
    foreach (DataRow row in (InternalDataCollectionBase) schemaTable.Rows)
    {
      ITableFieldInfo tableFieldInfo = fieldInfoCreator.CreateTableFieldInfo(row);
      this.nameFieldsInfo.Add(tableFieldInfo.ColumnName.ToUpper(), tableFieldInfo);
      this.indexFieldsInfo.Add(tableFieldInfo.ColumnOrdinal, tableFieldInfo);
    }
  }

  public static ITableFieldInfo[] GetFieldsInfo(IDataReader dataReader)
  {
    List<ITableFieldInfo> tableFieldInfoList = new List<ITableFieldInfo>();
    DataTable schemaTable = dataReader.GetSchemaTable();
    TableFieldInfoCreator fieldInfoCreator = new TableFieldInfoCreator(schemaTable);
    foreach (DataRow row in (InternalDataCollectionBase) schemaTable.Rows)
      tableFieldInfoList.Add(fieldInfoCreator.CreateTableFieldInfo(row));
    return tableFieldInfoList.ToArray();
  }

  /// <summary>Создание новой записи</summary>
  /// <param name="idr">Набор данных, по текущей записи которого создается новый объект</param>
  /// <returns>Экземпляр созданной записи</returns>
  public virtual object NewItem(IDataReader idr) => new object();

  /// <summary>
  /// вспомогательная функция для формирования сообщения об ошибке получения данных
  /// </summary>
  /// <param name="errMessage">Сообщение об ошибке (сгенерированное системой)</param>
  /// <param name="index">Индекс поля, при получении данных из которого возникла ошибка</param>
  /// <returns>Преобразованное сообщение об ошибке</returns>
  private string CreateErrorMessage(string errMessage, object id)
  {
    string empty = string.Empty;
    return $"Ошибка при получении значения поля {(!(id is int key) ? Convert.ToString(id) : (this.indexFieldsInfo.ContainsKey(key) ? this.indexFieldsInfo[(int) id].ColumnName.ToUpper() : "не определено"))} из таблицы {this.tableName}: {errMessage}";
  }

  /// <summary>
  /// Получение целочисленного значения для текущей записи из поля с заданным индексом
  /// </summary>
  /// <param name="dataReader">Набор данных, из текущей записи еоторого производится получение значения</param>
  /// <param name="index">Индекс поля, из которого надо получить значение</param>
  /// <returns>Полученное значение для текущей записи набора данных из поля с заданным индексом</returns>
  protected int getInt32(IDataReader dataReader, int index)
  {
    int int32 = 0;
    try
    {
      int32 = ReaderHelper.GetInt32(dataReader, index);
    }
    catch (Exception ex)
    {
      this.appMngr.AddWarningMessage(this.CreateErrorMessage(ex.Message, (object) index));
    }
    return int32;
  }

  /// <summary>
  /// Получение целочисленного значения для текущей записи из поля с заданным индексом
  /// </summary>
  /// <param name="dataReader">Набор данных, из текущей записи еоторого производится получение значения</param>
  /// <param name="fieldName">Имя поля, из которого надо получить значение</param>
  /// <returns>Полученное значение для текущей записи набора данных из поля с заданным индексом</returns>
  protected int getInt32(IDataReader dataReader, string fieldName)
  {
    int int32 = 0;
    try
    {
      int32 = ReaderHelper.GetInt32(dataReader, fieldName.ToUpper());
    }
    catch (Exception ex)
    {
      this.appMngr.AddWarningMessage(this.CreateErrorMessage(ex.Message, (object) fieldName));
    }
    return int32;
  }

  /// <summary>
  /// Получение вещественного значения для текущей записи из поля с заданным индексом
  /// </summary>
  /// <param name="dataReader">Набор данных, из текущей записи еоторого производится получение значения</param>
  /// <param name="index">Индекс поля, из которого надо получить значение</param>
  /// <returns>Полученное значение для текущей записи набора данных из поля с заданным индексом</returns>
  protected double getDouble(IDataReader dataReader, int index)
  {
    double num = 0.0;
    try
    {
      num = ReaderHelper.GetDouble(dataReader, index);
    }
    catch (Exception ex)
    {
      this.appMngr.AddWarningMessage(this.CreateErrorMessage(ex.Message, (object) index));
    }
    return num;
  }

  /// <summary>
  /// Получение вещественного значения для текущей записи из поля с заданным индексом
  /// </summary>
  /// <param name="dataReader">Набор данных, из текущей записи еоторого производится получение значения</param>
  /// <param name="fieldName">Имя поля, из которого надо получить значение</param>
  /// <returns>Полученное значение для текущей записи набора данных из поля с заданным индексом</returns>
  protected double getDouble(IDataReader dataReader, string fieldName)
  {
    double num = 0.0;
    try
    {
      num = ReaderHelper.GetDouble(dataReader, fieldName.ToUpper());
    }
    catch (Exception ex)
    {
      this.appMngr.AddWarningMessage(this.CreateErrorMessage(ex.Message, (object) fieldName));
    }
    return num;
  }

  /// <summary>
  /// Получение вещественного значения для текущей записи из поля с заданным индексом
  /// </summary>
  /// <param name="dataReader">Набор данных, из текущей записи еоторого производится получение значения</param>
  /// <param name="index">Индекс поля, из которого надо получить значение</param>
  /// <returns>Полученное значение для текущей записи набора данных из поля с заданным индексом</returns>
  protected float getFloat(IDataReader dataReader, int index)
  {
    float num = 0.0f;
    try
    {
      num = ReaderHelper.GetFloat(dataReader, index);
    }
    catch (Exception ex)
    {
      this.appMngr.AddWarningMessage(this.CreateErrorMessage(ex.Message, (object) index));
    }
    return num;
  }

  /// <summary>
  /// Получение строкового значения для текущей записи из поля с заданным индексом
  /// </summary>
  /// <param name="dataReader">Набор данных, из текущей записи еоторого производится получение значения</param>
  /// <param name="index">Индекс поля, из которого надо получить значение</param>
  /// <returns>Полученное значение для текущей записи набора данных из поля с заданным индексом</returns>
  protected string getString(IDataReader dataReader, int index)
  {
    string empty = string.Empty;
    try
    {
      empty = ReaderHelper.GetString(dataReader, index);
    }
    catch (Exception ex)
    {
      this.appMngr.AddWarningMessage(this.CreateErrorMessage(ex.Message, (object) index));
    }
    return empty;
  }

  /// <summary>
  /// Получение строкового значения для текущей записи из поля с заданным индексом
  /// </summary>
  /// <param name="dataReader">Набор данных, из текущей записи еоторого производится получение значения</param>
  /// <param name="fieldName">Имя поля, из которого надо получить значение</param>
  /// <returns>Полученное значение для текущей записи набора данных из поля с заданным индексом</returns>
  protected string getString(IDataReader dataReader, string fieldName)
  {
    string empty = string.Empty;
    try
    {
      empty = ReaderHelper.GetString(dataReader, fieldName.ToUpper());
    }
    catch (Exception ex)
    {
      this.appMngr.AddWarningMessage(this.CreateErrorMessage(ex.Message, (object) fieldName));
    }
    return empty;
  }

  /// <summary>
  /// Получение значения даты (и времени) для текущей записи из поля с заданным индексом
  /// </summary>
  /// <param name="dataReader">Набор данных, из текущей записи еоторого производится получение значения</param>
  /// <param name="index">Индекс поля, из которого надо получить значение</param>
  /// <returns>Полученное значение для текущей записи набора данных из поля с заданным индексом</returns>
  protected DateTime getDateTime(IDataReader dataReader, int index)
  {
    DateTime dateTime = DateTime.Now;
    try
    {
      dateTime = ReaderHelper.GetDateTime(dataReader, index);
    }
    catch (Exception ex)
    {
      this.appMngr.AddWarningMessage(this.CreateErrorMessage(ex.Message, (object) index));
    }
    return dateTime;
  }

  protected object getObject(IDataReader dataReader, int index)
  {
    object obj = (object) null;
    if (!dataReader.IsDBNull(index))
      obj = !this.indexFieldsInfo[index].DataType.Equals(typeof (byte)) ? (!this.indexFieldsInfo[index].DataType.Equals(typeof (short)) ? (!this.indexFieldsInfo[index].DataType.Equals(typeof (int)) ? (!this.indexFieldsInfo[index].DataType.Equals(typeof (long)) ? (!this.indexFieldsInfo[index].DataType.Equals(typeof (float)) ? (!this.indexFieldsInfo[index].DataType.Equals(typeof (Decimal)) ? (!this.indexFieldsInfo[index].DataType.Equals(typeof (double)) ? (!this.indexFieldsInfo[index].DataType.Equals(typeof (DateTime)) ? (!this.indexFieldsInfo[index].DataType.Equals(typeof (bool)) ? (!this.indexFieldsInfo[index].DataType.Equals(typeof (string)) ? (!this.indexFieldsInfo[index].DataType.Equals(typeof (char)) ? dataReader.GetValue(index) : (object) dataReader.GetChar(index)) : (object) dataReader.GetString(index)) : (object) dataReader.GetBoolean(index)) : (object) dataReader.GetDateTime(index)) : (object) dataReader.GetDouble(index)) : (object) dataReader.GetDecimal(index)) : (object) dataReader.GetFloat(index)) : (object) dataReader.GetInt64(index)) : (object) dataReader.GetInt32(index)) : (object) dataReader.GetInt16(index)) : (object) dataReader.GetByte(index);
    return obj;
  }

  protected string getObjectString(IDataReader dataReader, int index)
  {
    string empty = string.Empty;
    try
    {
      object obj = this.getObject(dataReader, index);
      if (obj != null)
        empty = Convert.ToString(obj);
    }
    catch (Exception ex)
    {
      this.appMngr.AddWarningMessage(this.CreateErrorMessage(ex.Message, (object) index));
    }
    return empty;
  }
}
