// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.CommonData.ItemsToCreate.IAttributeTypeToCreate
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.Interface.CommonData.ItemsToCreate;

/// <summary>
/// Интерфейс для доступа к элементу типа атрибута, предназначенного для создания в новой базе
/// </summary>
public interface IAttributeTypeToCreate : IItemToCreate, ICustomTypeDescriptor
{
  /// <summary>Короткое имя</summary>
  string ShortName { get; set; }

  /// <summary>Псевдоним</summary>
  string Alias { get; set; }

  /// <summary>Тип данных значения</summary>
  FieldTypes FieldType { get; set; }

  /// <summary>Максимальная длина значения</summary>
  long Size { get; set; }

  bool HasValueInList { get; }

  /// <summary>Идентификаторы наборов значений</summary>
  List<int> ValuesListIds { get; }

  /// <summary>
  /// Тип значения (одно, одно из списка, несколько, несколько из списка)
  /// </summary>
  MultiValueModes MultiValueMode { get; set; }

  /// <summary>Значение "по умолчанию" для данного типа атрибута</summary>
  string DefaultValue { get; set; }

  /// <summary>
  /// Тип создаваемого объекта, используется для атрибутов типа "Ссылка на объект"
  /// </summary>
  Guid CreatedObjectType { get; set; }

  /// <summary>Дополнительные опции атрибуту</summary>
  AttributeOptions Options { get; set; }

  /// <summary>
  /// Идентификаторы ед.измерений для списков допустимых значений
  /// </summary>
  Dictionary<int, string> ValuesListMeasureIDs { get; }

  /// <summary>
  /// Добавить идентификатор списка допустимых значений для атрибута
  /// </summary>
  /// <param name="id"></param>
  void AddValueInListId(int id, string units);

  /// <summary>Локальный атрибут IMBASE</summary>
  bool LocalImbaseAttribute { get; set; }
}
