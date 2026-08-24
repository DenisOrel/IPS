// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.IDataBase
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.Data;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>Интерфейс подключения к базе данных</summary>
public interface IDataBase
{
  /// <summary>
  /// Строка с наименованием (а точнее псевдонимом) типа базы к которой
  /// производится подключение
  /// </summary>
  string DataBaseType { get; }

  /// <summary>Интерфейс объекта подключения к базе данных</summary>
  IDbConnection DbConnection { get; }

  /// <summary>Создание нового объекта SQL-запроса к базе данных</summary>
  /// <returns>инрефейс созданного объекта запроса</returns>
  IDbCommand CreateCommand();

  /// <summary>Создание и выполнение запроса к базе данных</summary>
  /// <param name="sqlText">Текст запроса</param>
  /// <returns>интерфейс результирующего набора данных</returns>
  IDataReader GetDataReader(string sqlText);

  /// <summary>Создание объекта для обновления данных в базе</summary>
  /// <param name="sqlText">Строка для получения набора данных</param>
  /// <returns>интерфейс созданного объекта</returns>
  IDbDataAdapter GetDataAdapter(string sqlText);

  /// <summary>
  /// Проверка наличия в базе данных таблицы с указанным наименованием
  /// </summary>
  /// <param name="tableName">Название таблицы</param>
  /// <returns>Если таблица с указанным названием существует - возвращается true, иначе - false</returns>
  bool TableExists(string tableName);

  string GetIntField(string fieldName, string asFieldName);

  /// <summary>
  /// Метод вызывается сразу после успешного соединения с БД
  /// </summary>
  void OnAfterConnect();

  int MaxInOperator { get; }
}
