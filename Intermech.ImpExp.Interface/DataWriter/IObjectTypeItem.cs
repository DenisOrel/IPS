// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.DataWriter.IObjectTypeItem
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;

#nullable disable
namespace Intermech.ImpExp.Interface.DataWriter;

/// <summary>Интерфейс для типа объекта</summary>
public interface IObjectTypeItem : ITypeItem, IAttributableTypeItem
{
  /// <summary>Краткое наименование типа объекта</summary>
  string ShortName { get; set; }

  /// <summary>Название созданных объектов данного типа</summary>
  string ObjectName { get; set; }

  /// <summary>Комментарий</summary>
  string Note { get; set; }

  /// <summary>Идентификатор типа связи, используемой поумолчанию</summary>
  Guid RelationID { get; set; }

  /// <summary>
  /// Режим версионности типа объекта
  /// 0 - абстрактный тип объекта (контейнер для группировки других типов объектов);
  /// 1 - объекты данного типа не могут иметь версий;
  /// 2 - объекты данного типа могут иметь версии.
  /// </summary>
  ObjectVersionModes VersionableMode { get; set; }

  /// <summary>Предметная область</summary>
  string Area { get; set; }

  /// <summary>
  /// Ид. атрибута, который используется для отображения объектов данного типа в списках и деревьях.
  /// </summary>
  int CaptionAttributeID { get; set; }

  /// <summary>
  /// Наследование жизненного цикла: 0 - собственная схема,1 - общая,2 - унаследованная.
  /// </summary>
  lcType PublicLifeCycle { get; set; }

  /// <summary>
  /// Количество дней, в течение которых нельзя чистить удаленные объекты данного типа.
  /// </summary>
  int DaysBeforeDelete { get; set; }

  /// <summary>Иденитификатор схемы жизненного цикла</summary>
  Guid ShemaId { get; set; }

  /// <summary>Глобальный идентификатор родительского типа объекта</summary>
  Guid ParentID { get; set; }

  /// <summary>
  /// Можно ли данному типу объектов назначить любой атрибут
  /// </summary>
  bool AnyAttribute { get; }

  /// <summary>Массив идентификаторов дочерних типов</summary>
  int[] ChildIDs { get; }

  /// <summary>Иконка типа объектов</summary>
  byte[] Icon { get; set; }

  /// <summary>
  /// Проверка наличия дочернего типа с заданным идентификатором у данного типа объекта
  /// </summary>
  /// <param name="childID">Идентификатор дочернего типа объекта</param>
  /// <returns>Если дочерноий тип с заданным идентификатором уже есть, то возвращается - true, иначе false</returns>
  bool ChildExists(int childID);
}
