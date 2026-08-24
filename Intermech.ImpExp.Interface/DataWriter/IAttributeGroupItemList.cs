// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.DataWriter.IAttributeGroupItemList
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Interface.DataWriter;

public interface IAttributeGroupItemList : 
  ITypeItemList<IAttributeGroupItem>,
  IList<IAttributeGroupItem>,
  ICollection<IAttributeGroupItem>,
  IEnumerable<IAttributeGroupItem>,
  IEnumerable,
  IList,
  ICollection
{
  /// <summary>Привязка типа атрибута к группе атрибутов</summary>
  /// <param name="attrTypeId">Идентификатор типа атрибута</param>
  /// <param name="attrGroupId">Идентификатор группы атрибутов</param>
  void LinkAttributeTypeToGroup(int attrTypeId, Guid attrTypeGuid, int attrGroupId);

  /// <summary>Добавление группы атрибутов</summary>
  /// <param name="groupName">Наименование группы атрибутов</param>
  /// <returns>Интерфейс объекта с параметрами группы атрибутов</returns>
  IAttributeGroupItem Add(string groupName);

  /// <summary>Добавление группы атрибутов</summary>
  /// <param name="groupName">Наименование группы атрибутов</param>
  /// <param name="note">Комментарии</param>
  /// <returns>Интерфейс объекта с параметрами группы атрибутов</returns>
  IAttributeGroupItem Add(string groupName, string note);

  /// <summary>Добавление группы атрибутов</summary>
  /// <param name="groupName">Наименование группы атрибутов</param>
  /// <param name="groupGuid">Глобальный ид.</param>
  /// <param name="note">Комментарии</param>
  /// <param name="area">Предметная область</param>
  /// <param name="lang">Язык</param>
  /// <returns>Интерфейс объекта с параметрами группы атрибутов</returns>
  IAttributeGroupItem Add(
    string groupName,
    Guid groupGuid,
    string note,
    string area,
    string lang);
}
