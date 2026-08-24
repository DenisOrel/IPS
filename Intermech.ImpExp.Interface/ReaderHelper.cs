// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ReaderHelper
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Interface;

public static class ReaderHelper
{
  public static short GetInt16(IDataReader dataReader, int index)
  {
    return !dataReader.IsDBNull(index) ? dataReader.GetInt16(index) : (short) 0;
  }

  /// <summary>
  /// Получение целочисленного значения для текущей записи из поля с заданным индексом
  /// </summary>
  /// <param name="dataReader">Набор данных, из текущей записи еоторого производится получение значения</param>
  /// <param name="index">Индекс поля, из которого надо получить значение</param>
  /// <returns>Полученное значение для текущей записи набора данных из поля с заданным индексом</returns>
  public static int GetInt32(IDataReader dataReader, int index)
  {
    object obj = dataReader[index];
    return !DBNull.Value.Equals(obj) ? Convert.ToInt32(obj) : 0;
  }

  /// <summary>
  /// Получение целочисленного значения для текущей записи из поля с заданным индексом
  /// </summary>
  /// <param name="dataReader">Набор данных, из текущей записи еоторого производится получение значения</param>
  /// <param name="fieldName">Наименование поля, из которого надо получить значение</param>
  /// <returns>Полученное значение для текущей записи набора данных из поля с заданным индексом</returns>
  public static int GetInt32(IDataReader dataReader, string fieldName)
  {
    object obj = dataReader[fieldName];
    return !DBNull.Value.Equals(obj) ? Convert.ToInt32(obj) : 0;
  }

  /// <summary>
  /// Получение вещественного значения для текущей записи из поля с заданным индексом
  /// </summary>
  /// <param name="dataReader">Набор данных, из текущей записи еоторого производится получение значения</param>
  /// <param name="fieldName">Наименование поля, из которого надо получить значение</param>
  /// <returns>Полученное значение для текущей записи набора данных из поля с заданным индексом</returns>
  public static double GetDouble(IDataReader dataReader, string fieldName)
  {
    object obj = dataReader[fieldName];
    return !DBNull.Value.Equals(obj) ? Convert.ToDouble(dataReader[fieldName]) : 0.0;
  }

  /// <summary>
  /// Получение вещественного значения для текущей записи из поля с заданным индексом
  /// </summary>
  /// <param name="dataReader">Набор данных, из текущей записи еоторого производится получение значения</param>
  /// <param name="fieldName">Индекс поля, из которого надо получить значение</param>
  /// <returns>Полученное значение для текущей записи набора данных из поля с заданным индексом</returns>
  public static double GetDouble(IDataReader dataReader, int index)
  {
    return !dataReader.IsDBNull(index) ? Convert.ToDouble(dataReader[index]) : 0.0;
  }

  /// <summary>
  /// Получение вещественного значения для текущей записи из поля с заданным индексом
  /// </summary>
  /// <param name="dataReader">Набор данных, из текущей записи еоторого производится получение значения</param>
  /// <param name="index">Индекс поля, из которого надо получить значение</param>
  /// <returns>Полученное значение для текущей записи набора данных из поля с заданным индексом</returns>
  public static float GetFloat(IDataReader dataReader, int index)
  {
    return !dataReader.IsDBNull(index) ? dataReader.GetFloat(index) : 0.0f;
  }

  /// <summary>
  /// Получение строкового значения для текущей записи из поля с заданным индексом
  /// </summary>
  /// <param name="dataReader">Набор данных, из текущей записи еоторого производится получение значения</param>
  /// <param name="index">Индекс поля, из которого надо получить значение</param>
  /// <returns>Полученное значение для текущей записи набора данных из поля с заданным индексом</returns>
  public static string GetString(IDataReader dataReader, string fieldName)
  {
    object obj = dataReader[fieldName];
    return !DBNull.Value.Equals(obj) ? Convert.ToString(dataReader[fieldName]) : string.Empty;
  }

  /// <summary>
  /// Получение строкового значения для текущей записи из поля с заданным индексом
  /// </summary>
  /// <param name="dataReader">Набор данных, из текущей записи еоторого производится получение значения</param>
  /// <param name="index">Индекс поля, из которого надо получить значение</param>
  /// <returns>Полученное значение для текущей записи набора данных из поля с заданным индексом</returns>
  public static string GetString(IDataReader dataReader, int index)
  {
    return !dataReader.IsDBNull(index) ? dataReader.GetString(index) : "";
  }

  /// <summary>
  /// Получение значения даты (и времени) для текущей записи из поля с заданным индексом
  /// </summary>
  /// <param name="dataReader">Набор данных, из текущей записи еоторого производится получение значения</param>
  /// <param name="index">Индекс поля, из которого надо получить значение</param>
  /// <returns>Полученное значение для текущей записи набора данных из поля с заданным индексом</returns>
  public static DateTime GetDateTime(IDataReader dataReader, int index)
  {
    return !dataReader.IsDBNull(index) ? dataReader.GetDateTime(index) : DateTime.Now;
  }

  /// <summary>
  /// Получение "логического" значения для текущей записи из поля с заданным индексом
  /// </summary>
  /// <param name="dataReader">Набор данных, из текущей записи еоторого производится получение значения</param>
  /// <param name="index">Индекс поля, из которого надо получить значение</param>
  /// <returns>Полученное значение для текущей записи набора данных из поля с заданным индексом</returns>
  public static bool GetBoolean(IDataReader dataReader, int index)
  {
    return !dataReader.IsDBNull(index) && dataReader.GetBoolean(index);
  }
}
