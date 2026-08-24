// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.CommonData.ItemsToCreate.IObjectTypeToCreate
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.ImpExp.Interface.DataWriter;
using System;
using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.Interface.CommonData.ItemsToCreate;

public interface IObjectTypeToCreate : IItemToCreate, ICustomTypeDescriptor
{
  /// <summary>Короткое имя</summary>
  string ShortName { get; set; }

  /// <summary>Наименование объекта данного типа (например, Деталь).</summary>
  string InstanceName { get; set; }

  /// <summary>
  /// Глобальный дентификатор родительского типа объекта (если такого нет, то Guid.Empty)
  /// </summary>
  Guid ParentTypeId { get; set; }

  /// <summary>Тип версионности</summary>
  ObjectVersionModes VersionMode { get; set; }

  /// <summary>Комментарии</summary>
  string Note { get; set; }

  /// <summary>
  /// Идентификатор вида связи, показываемой в клиенте по умолчанию.
  /// </summary>
  Guid DefaultRelationId { get; set; }

  /// <summary>Предметная область</summary>
  string Area { get; set; }

  /// <summary>
  /// Ид. атрибута, который используется для отображения объектов данного типа в списках и деревьях.
  /// </summary>
  int CaptionAttrId { get; set; }

  /// <summary>
  /// Допускается ли добавлять к объектам данного типа любые атрибуты
  /// </summary>
  bool AnyAttributes { get; set; }

  /// <summary>
  /// Наследование жизненного цикла(собственная схема/общая/унаследованная)
  /// </summary>
  lcType LcMode { get; set; }

  /// <summary>
  /// Количество дней, в течение которых нельзя чистить удаленные объекты данного типа
  /// </summary>
  int DaysToDelete { get; set; }

  /// <summary>Идентификатор схемы ЖЦ</summary>
  Guid LcShemaId { get; set; }

  /// <summary>Иконка типа объектов</summary>
  byte[] Icon { get; set; }
}
