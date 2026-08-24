// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.DataWriter.IAttributeTypeItem
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

#nullable disable
namespace Intermech.ImpExp.Interface.DataWriter;

/// <summary>Интерфейс для типа атрибута</summary>
public interface IAttributeTypeItem : ITypeItem
{
  /// <summary>Идентификатор типа значения атрибута</summary>
  int AttrValueType { get; set; }

  /// <summary>
  /// Тип значения (одно, одно из списка, несколько, несколько из списка)
  /// </summary>
  MultiValueModes MultiValueMode { get; set; }

  /// <summary>Максимальная длина значения атрибута</summary>
  int MaxSize { get; set; }

  /// <summary>Короткое имя типа атрибута</summary>
  string ShortName { get; set; }

  /// <summary>Псевдоним для типа атрибута</summary>
  string Alias { get; set; }

  /// <summary>
  /// Признак того, что данный тип атрибута существует в базе
  /// </summary>
  bool ExistsInBase { get; set; }

  object DefaultValue { get; set; }

  /// <summary>
  /// Получить список возможных значений атрибута (если атрибут содержит значения из списка)
  /// </summary>
  /// <returns>Список значений</returns>
  IAttributePossibleValue[] GetPossibleValues();

  /// <summary>Добавление нового значения в список допустимых</summary>
  /// <param name="possibleValue">новое значение</param>
  void AddPossibleValue(IAttributePossibleValue possibleValue);

  /// <summary>Добавление нового значения в список допустимых</summary>
  /// <param name="value">Новое значение</param>
  /// <param name="description">Описание</param>
  bool AddPossibleValue(int inListID, object value, string description);

  /// <summary>
  /// Проверка наличия значения в списке (если атрибут содержит значения из списка)
  /// Если атрибут не может содержать значения из списка - то функция будет возвращать false
  /// </summary>
  /// <param name="possibleValue"></param>
  /// <returns>Если значение есть в списке - true, иначе - false</returns>
  bool IsExistsPossibleValue(IAttributePossibleValue possibleValue);
}
