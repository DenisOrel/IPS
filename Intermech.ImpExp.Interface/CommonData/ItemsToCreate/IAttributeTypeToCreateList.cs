// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.CommonData.ItemsToCreate.IAttributeTypeToCreateList
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Interface.CommonData.ItemsToCreate;

/// <summary>
/// Интерфейс для доступа к списку элементов типов атрибутов, предназначенных для создания в новой базе
/// </summary>
public interface IAttributeTypeToCreateList : IItemToCreateList<IAttributeTypeToCreate>
{
  IAttributeTypeToCreate AddItem(
    bool isNew,
    string name,
    string shortName,
    string alias,
    FieldTypes fieldType,
    long size,
    Guid guid,
    long sysID,
    bool valueInList);

  IAttributeTypeToCreate AddItem(
    bool isNew,
    string name,
    string shortName,
    string alias,
    FieldTypes fieldType,
    long size,
    Guid guid,
    long sysID,
    bool valueInList,
    int valueListID);

  IAttributeTypeToCreate AddItem(
    bool isNew,
    string name,
    string shortName,
    string alias,
    FieldTypes fieldType,
    long size,
    Guid guid,
    long sysID,
    bool valueInList,
    int valueListID,
    string DefaultValue,
    MultiValueModes multiValueMode);

  IAttributeTypeToCreate AddItem(
    bool isNew,
    string name,
    string shortName,
    string alias,
    FieldTypes fieldType,
    long size,
    Guid guid,
    long sysID,
    bool valueInList,
    List<int> valueListIDs,
    Dictionary<int, string> valuesListMeasureIDs,
    string defaultValue,
    MultiValueModes multiValueMode);

  bool ExistsByAlias(string alias);

  IAttributeTypeToCreate GetByAlias(string alias);

  void UpdateCasheAlias(string alias, IAttributeTypeToCreate item);
}
