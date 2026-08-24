// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.PumpClass
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>
/// Базовый класс для создания на его основе задач перекачки определенных категорий объектов
/// Для создания своего класса на базе данного надо перекрыть методы Exam() и Pump()
/// и реализовать внутри них логику инициализации закачки и закачку.
/// Также следует установить атрибут TaskDescriptionAttribute с нужными аргументами и переопределить GUID
/// </summary>
[TaskDescription("Инициализация данных для перекачки", "Перекачка данных")]
public abstract class PumpClass
{
  protected PumpTask taskExam;
  protected PumpTask taskPump;
  /// <summary>
  /// Ссылка на экземпляр класса модуля расширения в котором находится данный экземпляр
  /// </summary>
  protected PluginClass plugin;
  /// <summary>
  /// кодирвка базы - для корректной закачки текста из blob-ов
  /// </summary>
  protected Encoding dataBaseEncoding = Encoding.GetEncoding(1251);

  /// <summary>
  /// Событие, которое генерится при изменении статуса задачи
  /// </summary>
  public event CheckPointDelegate OnCheckPoint;

  /// <summary>Вид пампера: метаданные, обычный, или в конце</summary>
  public PumperType PumperType
  {
    get
    {
      TaskType[] customAttributes = (TaskType[]) this.GetType().GetCustomAttributes(typeof (TaskType), true);
      return customAttributes.Length != 0 ? customAttributes[0].PumperType : PumperType.Standard;
    }
  }

  /// <summary>Признак того, что пампер качает метаданные</summary>
  protected bool IsMetadataPumper => this.PumperType == PumperType.MetaData;

  protected abstract Guid GUID { get; }

  /// <summary>Глобальный идентификатор класса миграции</summary>
  public Guid PumperGuid => this.GUID;

  public IPumpTask TaskExam => (IPumpTask) this.taskExam;

  public IPumpTask TaskPump => (IPumpTask) this.taskPump;

  /// <summary>
  /// Ссылка на экземпляр класса модуля расширения в котором находится данный экземпляр
  /// </summary>
  public PluginClass Plugin => this.plugin;

  /// <summary>Конструктор</summary>
  /// <param name="plugin">Модуль расширения, в котором должна проводиться перекачка</param>
  public PumpClass(PluginClass plugin)
  {
    this.plugin = plugin;
    this.ReadTaskAttr();
    this.taskExam = new PumpTask(this.GUID, new MethodInvoker(this.Exam), this.ExamDescription, this.IsMetadataPumper ? PumpTaskType.ExamMetadata : PumpTaskType.ExamData);
    this.taskPump = new PumpTask(this.GUID, new MethodInvoker(this.Pump), this.PumpDescription, this.IsMetadataPumper ? PumpTaskType.PumpMetadata : PumpTaskType.PumpData);
  }

  /// <summary>
  /// Поле для хранения описания задачи инициализации перекачки
  /// </summary>
  public string ExamDescription { get; private set; } = "Инициализация данных для перекачки";

  /// <summary>Поле для хранения описания задачи перекачки</summary>
  public string PumpDescription { get; private set; } = "Перекачка данных";

  /// <summary>
  /// Чтение опсания задачи перекачки из атрибутов класса в метаданных
  /// </summary>
  private void ReadTaskAttr()
  {
    Type attributeType = typeof (TaskDescriptionAttribute);
    object[] objArray = (object[]) null;
    if (this.GetType().IsDefined(attributeType, true))
    {
      objArray = this.GetType().GetCustomAttributes(attributeType, false);
      if (objArray == null || objArray.Length == 0)
        objArray = this.GetType().GetCustomAttributes(attributeType, true);
    }
    TaskDescriptionAttribute descriptionAttribute = objArray == null || objArray.Length == 0 ? (TaskDescriptionAttribute) null : objArray[0] as TaskDescriptionAttribute;
    if (descriptionAttribute == null)
      return;
    this.ExamDescription = descriptionAttribute.ExamDescription;
    this.PumpDescription = descriptionAttribute.PumpDescription;
  }

  /// <summary>
  /// Функция закачки данных (для реализации процесса перекачки)
  /// </summary>
  public virtual void Pump()
  {
  }

  /// <summary>
  /// Функция предварительной закачки данных для анализа и инициализации процесса перекачки
  /// </summary>
  public virtual void Exam()
  {
  }

  /// <summary>Получение полей и их индексов в наборе данных</summary>
  /// <param name="dataReader">Набор данных</param>
  /// <returns>Dictionary с полями набора данных и их индексами (fieldName =&gt; fieldIndex)</returns>
  protected Dictionary<string, int> GetTableColumns(IDataReader dataReader)
  {
    Dictionary<string, int> tableColumns = new Dictionary<string, int>();
    foreach (ITableFieldInfo tableFieldInfo in PumpItemFactory.GetFieldsInfo(dataReader))
      tableColumns.Add(tableFieldInfo.ColumnName, tableFieldInfo.ColumnOrdinal);
    return tableColumns;
  }

  protected virtual bool TableExists(string tableName) => this.plugin.idb.TableExists(tableName);

  /// <summary>Получение количества записей в таблице</summary>
  /// <param name="tableName">Запрос вида "SELECT COUNT(*) FROM"</param>
  /// <returns>Количество записей в таблице</returns>
  protected virtual int GetRecordsCount(string sqlText)
  {
    using (IDbCommand command = this.plugin.idb.DbConnection.CreateCommand())
    {
      command.CommandText = sqlText.ToUpper();
      return Convert.ToInt32(command.ExecuteScalar());
    }
  }

  /// <summary>Получение количества записей в таблице</summary>
  /// <param name="tableName">Имя таблицы</param>
  /// <returns>Количество записей в таблице</returns>
  protected virtual int GetTableRecordsCount(string tableName)
  {
    using (IDbCommand command = this.plugin.idb.DbConnection.CreateCommand())
    {
      command.CommandText = "SELECT COUNT(*) FROM " + tableName.ToUpper();
      return Convert.ToInt32(command.ExecuteScalar());
    }
  }

  /// <summary>
  /// Вспомогательная функция для получения нового объекта IDataReader
  /// </summary>
  /// <param name="tableName">Имя таблицы</param>
  /// <param name="tableColumns">Перечень столбцов таблицы (через запятую - для select)</param>
  /// <param name="commandBehavior">"Поведение" команды (какие данные извлекать - все, ключи, схему и т.д.)</param>
  /// <returns>Интерфейс IDataReader</returns>
  protected virtual IDataReader GetBehaviorDataReader(
    string tableName,
    string tableColumns,
    CommandBehavior commandBehavior)
  {
    if (!this.TableExists(tableName))
      return (IDataReader) null;
    IDbCommand command = this.plugin.idb.DbConnection.CreateCommand();
    command.CommandText = $"SELECT {tableColumns} FROM {tableName.ToUpper()}";
    if (commandBehavior == CommandBehavior.SchemaOnly)
      command.CommandText += " WHERE 1=0";
    return command.ExecuteReader(commandBehavior);
  }

  /// <summary>
  /// Вспомогательная функция для получения нового объекта IDataReader по произвольному запросу
  /// </summary>
  /// <param name="sqlText">Текст запроса по которому надо получить данные</param>
  /// <returns>Интерфейс IDataReader</returns>
  protected virtual IDataReader GetDataReader(string sqlText)
  {
    return this.GetDataReader(sqlText, CommandBehavior.Default);
  }

  /// <summary>
  /// Вспомогательная функция для получения нового объекта IDataReader по произвольному запросу
  /// </summary>
  /// <param name="sqlText"></param>
  /// <param name="commandBehavior">"Поведение" команды (какие данные извлекать - все, ключи, схему и т.д.)</param>
  /// <returns>Интерфейс IDataReader</returns>
  protected virtual IDataReader GetDataReader(string sqlText, CommandBehavior commandBehavior)
  {
    IDbCommand command = this.plugin.idb.DbConnection.CreateCommand();
    command.CommandText = sqlText;
    return command.ExecuteReader(commandBehavior);
  }

  /// <summary>
  /// Вспомогательная функция для получения нового объекта IDataReader по произвольному запросу
  /// </summary>
  /// <param name="sqlText"></param>
  /// <param name="dbType">Тип БД, к которой выполнить подключение</param>
  /// <returns>Интерфейс IDataReader</returns>
  /// <remarks>Если dbType совпадает с БД плагина, то дополнительное соединение не создается, а возвращается уже существующее</remarks>
  protected virtual IDataReader GetDataReader(string sqlText, ConnStrType dbType)
  {
    return this.GetDataReader(sqlText, CommandBehavior.Default, dbType);
  }

  /// <summary>
  /// Вспомогательная функция для получения нового объекта IDataReader по произвольному запросу
  /// </summary>
  /// <param name="sqlText"></param>
  /// <param name="commandBehavior">"Поведение" команды (какие данные извлекать - все, ключи, схему и т.д.)</param>
  /// <param name="dbType">Тип БД, к которой выполнить подключение</param>
  /// <returns>Интерфейс IDataReader</returns>
  /// <remarks>Если dbType совпадает с БД плагина, то дополнительное соединение не создается, а возвращается уже существующее</remarks>
  protected virtual IDataReader GetDataReader(
    string sqlText,
    CommandBehavior commandBehavior,
    ConnStrType dbType)
  {
    IDbConnection dbConnection = this.plugin.CustomDbConnection(dbType);
    if (dbConnection == null || dbConnection.State != ConnectionState.Open)
      return (IDataReader) null;
    IDbCommand command = dbConnection.CreateCommand();
    command.CommandText = sqlText;
    command.CommandTimeout = 0;
    return command.ExecuteReader(commandBehavior);
  }

  protected virtual object GetCustomExecuteScalar(string sqlText)
  {
    IDbCommand command = this.plugin.idb.DbConnection.CreateCommand();
    command.CommandText = sqlText;
    return command.ExecuteScalar();
  }

  /// <summary>Получение данных для работы с блобами</summary>
  /// <param name="tableName">Имя таблицы</param>
  /// <param name="tableColumns">Перечень столбцов таблицы (через запятую - для select)</param>
  /// <returns>Интерфейс IDataReader</returns>
  protected IDataReader GetSequentialDataReader(string tableName, string tableColumns)
  {
    return this.GetBehaviorDataReader(tableName, tableColumns, CommandBehavior.SequentialAccess);
  }

  /// <summary>Получение данных для работы с блобами</summary>
  /// <param name="tableName">Имя таблицы</param>
  /// <returns>Интерфейс IDataReader</returns>
  protected IDataReader GetSequentialDataReader(string tableName)
  {
    return this.GetSequentialDataReader(tableName, "*");
  }

  /// <summary>
  /// Получение данных для работы со схемой таблицы (т.е. для получения метаданных)
  /// </summary>
  /// <param name="tableName">Имя таблицы</param>
  /// <returns>Интерфейс IDataReader</returns>
  protected IDataReader GetShemaDataReader(string tableName)
  {
    return this.GetBehaviorDataReader(tableName, "*", CommandBehavior.SchemaOnly);
  }

  /// <summary>Получение набора данных</summary>
  /// <param name="tableName">Имя таблицы</param>
  /// <returns>Интерфейс IDataReader</returns>
  protected virtual IDataReader GetDefaultDataReader(string tableName)
  {
    return this.GetDefaultDataReader(tableName, "*");
  }

  /// <summary>Получение набора данных</summary>
  /// <param name="tableName">Имя таблицы</param>
  /// <param name="tableColumns">Перечень столбцов таблицы (через запятую - для select)</param>
  /// <returns>Интерфейс IDataReader</returns>
  protected virtual IDataReader GetDefaultDataReader(string tableName, string tableColumns)
  {
    return this.GetBehaviorDataReader(tableName, tableColumns, CommandBehavior.Default);
  }

  /// <summary>
  /// Установка значения строки статуса и значения прогресса для задачи подготовки к закачке
  /// </summary>
  /// <param name="status">Значение строки статуса</param>
  /// <param name="progress">Значение процента выполнения</param>
  protected void ExamCheckPoint(string status, int progress)
  {
    this.taskExam.OnCheckPointEvent(status, progress);
  }

  /// <summary>
  /// Установка значения строки статуса и значения прогресса для задачи закачки данных
  /// </summary>
  /// <param name="status">Значение строки статуса</param>
  /// <param name="progress">Значение процента выполнения</param>
  protected void PumpCheckPoint(string status, int progress)
  {
    this.taskPump.OnCheckPointEvent(status, progress);
  }

  protected void SetCountPumpRecords(int count)
  {
    this.taskPump.OnReadCountRecordsEvent((long) count);
  }

  /// <summary>
  /// Вычисление процента на основании диапазона выполнения, числа итераций и текущего индекса
  /// </summary>
  /// <param name="count">Число итераций в заданном диапазоне</param>
  /// <param name="index">Индекс ткущей итерации</param>
  /// <param name="startPercent">Начальное значение процента для диапазона выполнения итераций</param>
  /// <returns>Вичисленное значение процента выполнения</returns>
  protected int CalculatePercent(int count, int index, int startPercent)
  {
    return this.CalculatePercent(count, index, startPercent, 100);
  }

  /// <summary>
  /// Вычисление процента на основании диапазона выполнения, числа итераций и текущего индекса
  /// </summary>
  /// <param name="count">Число итераций в заданном диапазоне</param>
  /// <param name="index">Индекс ткущей итерации</param>
  /// <param name="startPercent">Начальное значение процента для диапазона выполнения итераций</param>
  /// <param name="endPercent">Конечное значение процента для диапазона выполнения итераций</param>
  /// <returns>Вичисленное значение процента выполнения</returns>
  protected int CalculatePercent(int count, int index, int startPercent, int endPercent)
  {
    if (count == 0)
      return endPercent;
    if (endPercent > 100)
      endPercent = 100;
    else if (endPercent < 0)
      endPercent = 0;
    if (startPercent > endPercent)
      startPercent = endPercent;
    if (startPercent < 0)
      startPercent = 0;
    return Math.Min(startPercent + (endPercent - startPercent) * (index - 1) / count, endPercent);
  }
}
