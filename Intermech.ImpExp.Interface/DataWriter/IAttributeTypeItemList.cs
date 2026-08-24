// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.DataWriter.IAttributeTypeItemList
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Interface.DataWriter;

/// <summary>интерфейс для списка типов атрибутов</summary>
public interface IAttributeTypeItemList : 
  ITypeItemList<IAttributeTypeItem>,
  IList<IAttributeTypeItem>,
  ICollection<IAttributeTypeItem>,
  IEnumerable<IAttributeTypeItem>,
  IEnumerable,
  IList,
  ICollection
{
  /// <summary>Проверка наличия типа атрибута по псевдониму</summary>
  /// <param name="alias">Псевдоним типа атрибута</param>
  /// <returns>Если тип атрибута с заданным псевдонимом существует - true, иначе - false</returns>
  bool ExistsByAlias(string alias);

  /// <summary>Полчение типа атрибута по псевдониму</summary>
  /// <param name="alias">Псевдоним типа атрибута</param>
  /// <returns>Элемент типа атрибута (или null, если типа с таким псевдонимом нет)</returns>
  IAttributeTypeItem GetByAlias(string alias);

  /// <summary>Добавление типа атрибута</summary>
  /// <param name="name">Наименование</param>
  /// <param name="shortName">Краткое наименование</param>
  /// <param name="alias">Идентификатор понятия для использования в Techcard</param>
  /// <param name="note">Примечание</param>
  /// <param name="fieldType">Тип данных</param>
  /// <param name="defVal">Значение по умолчанию</param>
  /// <param name="multiMode">Тип значения</param>
  /// <param name="computeMode">Вычисляемый параметр или нет</param>
  /// <param name="uniqueMode">Контроль уникальности атрибута в пределах базы</param>
  /// <param name="size">Длина (или ссылка) атрибута</param>
  /// <param name="level">Уровень продвижения, на котором может существовать атрибут</param>
  /// <param name="formula">Формула вычисления значения типа атрибута</param>
  /// <param name="language">Язык</param>
  /// <param name="guid">Глобальный идентификатор типа атрибута</param>
  /// <param name="area">Предметная область</param>
  /// <param name="isContent">Влияет ли данный атрибут на содержимое объекта</param>
  /// <param name="inView">Управляет наличием данного атрибута в общих таблицах представления атрибута на чтение</param>
  /// <param name="options">Флаги с настройками</param>
  /// <param name="mask">Маска ввода значения атрибута</param>
  /// <param name="groupID">Группа атрибутов, в которую включить создаваемый атрибут или 0</param>
  /// <returns>Структура с параметрами типа атрибута</returns>
  IAttributeTypeItem Add(
    string name,
    string shortName,
    string alias,
    string note,
    FieldTypes fieldType,
    string defVal,
    MultiValueModes multiMode,
    ComputeValueModes computeMode,
    UniqueValueModes uniqueMode,
    long size,
    int level,
    string formula,
    string language,
    Guid guid,
    string area,
    bool isContent,
    short inView,
    AttributeOptions options,
    string mask,
    int groupID);

  /// <summary>Добавление типа атрибута</summary>
  /// <param name="name">Наименование</param>
  /// <param name="shortName">Краткое наименование</param>
  /// <param name="alias">Идентификатор понятия для использования в Techcard</param>
  /// <param name="fieldType">Тип данных</param>
  /// <param name="multiMode">Тип значения</param>
  /// <param name="size">Длина (или ссылка) атрибута</param>
  /// <param name="guid">Глобальный идентификатор типа атрибута</param>
  /// <returns>Структура с параметрами типа атрибута</returns>
  IAttributeTypeItem Add(
    string name,
    string shortName,
    string alias,
    FieldTypes fieldType,
    MultiValueModes multiMode,
    long size,
    Guid guid);

  /// <summary>Добавление типа атрибута</summary>
  /// <param name="name">Наименование</param>
  /// <param name="shortName">Краткое наименование</param>
  /// <param name="alias">Идентификатор понятия для использования в Techcard</param>
  /// <param name="fieldType">Тип данных</param>
  /// <param name="size">Длина (или ссылка) атрибута</param>
  /// <param name="guid">Глобальный идентификатор типа атрибута</param>
  /// <returns>Структура с параметрами типа атрибута</returns>
  IAttributeTypeItem Add(
    string name,
    string shortName,
    string alias,
    FieldTypes fieldType,
    long size,
    Guid guid);

  /// <summary>Добавление типа атрибута (сокращенная версия)</summary>
  /// <param name="name">Наименование</param>
  /// <param name="fieldType">Тип данных</param>
  /// <param name="size">Размер данных</param>
  /// <params name="guidStr">Строка с глобальным идентификатором типа атрибута</params>
  /// <returns>Структура с параметрами типа атрибута</returns>
  IAttributeTypeItem Add(string name, string guidStr, FieldTypes fieldType, long size);

  /// <summary>Добавление типа атрибута (сокращенная версия)</summary>
  /// <param name="name">Наименование</param>
  /// <param name="fieldType">Тип данных</param>
  /// <param name="size">Размер данных</param>
  /// <returns>Структура с параметрами типа атрибута</returns>
  IAttributeTypeItem Add(string name, FieldTypes fieldType, long size);
}
