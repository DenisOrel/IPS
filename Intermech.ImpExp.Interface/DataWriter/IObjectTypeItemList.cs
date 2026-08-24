// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.DataWriter.IObjectTypeItemList
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Interface.DataWriter;

/// <summary>Интерфейс для списка типов объектов</summary>
public interface IObjectTypeItemList : 
  ITypeItemList<IObjectTypeItem>,
  IList<IObjectTypeItem>,
  ICollection<IObjectTypeItem>,
  IEnumerable<IObjectTypeItem>,
  IEnumerable,
  IList,
  ICollection
{
  /// <summary>
  /// Проверка наличия типа объекта по краткому наименованию
  /// </summary>
  /// <param name="shortName">Краткое наименование типа объекта</param>
  /// <returns>Если тип объекта с заданным кратким наименованием существует - true, иначе - false</returns>
  bool ExistsByShortName(string shortName);

  /// <summary>Получение типа объекта по краткому наименованию</summary>
  /// <param name="shortName">Краткое наименование типа объекта</param>
  /// <returns>Тип объекта (или null, если типа объекта с таким кратким наименованием нет)</returns>
  IObjectTypeItem GetByShortName(string shortName);

  /// <summary>Создание нового типа объекта</summary>
  /// <param name="parentID">Глобальный дентификатор родительского типа объекта (если такого нет, то Guid.Empty)</param>
  /// <param name="name">Наименование типа объектов (например, Детали)</param>
  /// <param name="objectName">Наименование объекта данного типа (например, Деталь).</param>
  /// <param name="shortName">Краткое наименование типа объектов</param>
  /// <param name="versionable">Тип версионности(абстрактный/неверсионный/версионный)</param>
  /// <param name="note">Комментарии</param>
  /// <param name="defRelId">Идентификатор вида связи, показываемой в клиенте по умолчанию.</param>
  /// <param name="guid">Глобальный уникальный идентификатор типа объектов.</param>
  /// <param name="area">Предметная область</param>
  /// <param name="captionAttribute">Ид. атрибута, который используется для отображения объектов данного типа в списках и деревьях.</param>
  /// <param name="anyAttributes">Допускается ли добавлять к объектам данного типа любые атрибуты</param>
  /// <param name="publicLc">Наследование жизненного цикла(собственная схема/общаяунаследованная)</param>
  /// <param name="delTime">Количество дней, в течение которых нельзя чистить удаленные объекты данного типа.</param>
  /// <param name="shemaId">Идентификатор схемы ЖЦ</param>
  /// <returns>Интерфейс объекта с параметрами типа объекта</returns>
  IObjectTypeItem Add(
    Guid parentID,
    string name,
    string objectName,
    string shortName,
    ObjectVersionModes versionable,
    string note,
    Guid defRelId,
    Guid guid,
    string area,
    int captionAttribute,
    bool anyAttributes,
    lcType publicLc,
    int delTime,
    Guid shemaId,
    byte[] icon);

  /// <summary>Привязка типа атрибута к типу объекта</summary>
  /// <param name="attrTypeId">Идентификатор типа атрибута</param>
  /// <param name="objTypeId">Идентификатор типа объекта</param>
  /// <param name="isPublic">Передается ли параметр подтипам данного типа объектов</param>
  /// <param name="requiredMod">Режим добавления параметра</param>
  /// <param name="validationRule">Правило валидации значений, попадающих в этот атрибут в контексте данного типа объекта.</param>
  /// <param name="computeMode">Вычисляемый параметр или нет (обычный/с хранением в базе/вычисляемый "на лету)".</param>
  /// <param name="formula">Формула вычисления значения</param>
  /// <param name="uniqueMode">Режим контроля уникальности поля в пределах базы</param>
  /// <param name="level">Уровень продвижения, на котором может существовать атрибут.</param>
  /// <param name="defaultValue">Значение по умолчанию.</param>
  /// <param name="inViewMode">Управляет наличием данного атрибута в общих таблицах представления атрибута на чтение</param>
  /// <param name="isContent">Влияет ли данный атрибут на содержимое объекта</param>
  /// <param name="options">Флаги с настройками (Опции, регулирующие поведение атрибутов)</param>
  /// <param name="mask">Маска ввода значения атрибута</param>
  /// <param name="masterId">Идентификатор мастер-атрибута. Если  &gt; 0, то это подчиненный атрибут (его значение зависит от значения мастер-атрибута).</param>
  /// <param name="sourceId">Ид. атрибута, из которого подчиненный атрибут выбирает данные у объекта-источника.</param>
  void LinkAttributeTypeToObjectType(
    int attrTypeId,
    int objTypeId,
    bool isPublic,
    RequiredModes requiredMod,
    string validationRule,
    ComputeValueModes computeMode,
    string formula,
    UniqueValueModes uniqueMode,
    int level,
    string defaultValue,
    OptimizationModes inViewMode,
    bool isContent,
    AttributeOptions options,
    string mask,
    int masterId,
    int sourceId);

  /// <summary>Получить список дочерних типов для типов</summary>
  List<int> GetChildTypesRecursive(params int[] parentTypeIDs);
}
