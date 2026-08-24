// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.CommonData.IMeasures
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

#nullable disable
namespace Intermech.ImpExp.Interface.CommonData;

/// <summary>
/// Интерфейс для работы с единицами измерения при закачке (для организации кэша единиц измерения)
/// </summary>
public interface IMeasures
{
  /// <summary>
  /// Проверка наличия в кэше единицы измерения с заданным коротким именем
  /// </summary>
  /// <param name="shortName">Короткое имя единицы измерения</param>
  /// <returns>Если такая ед. измерения есть в кэше возвращается true, иначе - false</returns>
  bool MeasureExists(string shortName);

  /// <summary>
  /// Проверка наличия единицы измерения с указанным идентификатором в котексте текущего сеанса закачки
  /// </summary>
  /// <param name="measureObjId">Идентификатор объекта единицы измерения в котексте текущего сеанса закачки</param>
  /// <returns>Если такая ед. измерения есть в кэше возвращается true, иначе - false</returns>
  bool MeasureExists(long measureObjId);

  /// <summary>
  /// Получение объекта единицы измерения по его идентификатору в сеансе закачки
  /// </summary>
  /// <param name="measureObjId">Идентификатор единицы измерения</param>
  /// <returns>Интерфейс объекта единицы измерения</returns>
  IMeasureItem GetMeasure(long measureObjId);

  /// <summary>
  /// Получение объекта единицы измерения по его короткому имени
  /// </summary>
  /// <param name="measureShortName">Короткое имя единицы измерения</param>
  /// <returns>Интерфейс объекта единицы измерения</returns>
  IMeasureItem GetMeasure(string measureShortName);

  /// <summary>
  /// Добавление новой единицы измерения (для сооздания тех единиц измерения, что еще нет в новой базе)
  /// </summary>
  /// <param name="shortName">Короткое имя единицы измерения (например, "кг")</param>
  /// <param name="longName">Название единицы измерения (например, "килограмм")</param>
  /// <param name="koef">Коэффициент приведения к базовой единице измерения</param>
  /// <param name="physicalValueId">Идентификатор (в контексте текущего сеанса перекачки)
  /// физической величины в состав которой входит данная единица измерения</param>
  /// <returns>Идентификатор единицы измерения в контексте текущего сеанса перекачки</returns>
  long AddMeasure(string shortName, string longName, double koef, long physicalValueId);

  /// <summary>
  /// Загрузка уже имеющихся в базе объектов единиц измерения
  /// </summary>
  void Reload();
}
