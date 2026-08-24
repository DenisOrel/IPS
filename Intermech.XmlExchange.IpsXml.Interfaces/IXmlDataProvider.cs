// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Interfaces.IXmlDataProvider
// Assembly: Intermech.XmlExchange.IpsXml.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 741EAC98-7C4B-42E4-B0B7-F40794536EF7
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.xml

using System.Collections.Generic;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Interfaces;

/// <summary>Универсальный провайдер данных в любом формате XML</summary>
public interface IXmlDataProvider
{
  /// <summary>Доступ к корневым объектам</summary>
  IReadOnlyCollection<IXmlObject> RootObjects { get; }

  /// <summary>Получить список всех объектов из XML</summary>
  /// <returns></returns>
  IReadOnlyCollection<IXmlObject> GetAllObjects();

  /// <summary>Получить родительский объект по связи</summary>
  /// <param name="rel">Связь</param>
  /// <returns>Родительский объект</returns>
  IXmlObject GetRelParentObj(IXmlRelation rel);

  /// <summary>Получить дочерний объект по связи</summary>
  /// <param name="rel">Связь</param>
  /// <returns>Дочерний объект</returns>
  IXmlObject GetRelChildObj(IXmlRelation rel);

  /// <summary>Получение связей с дочерними объектами</summary>
  /// <param name="obj">Объект</param>
  /// <returns>Коллекция связей с дочерними объектами. null, если связей нет</returns>
  IReadOnlyCollection<IXmlRelation> GetObjChildRelations(IXmlObject obj);

  /// <summary>Получение связей с родительскими объектами</summary>
  /// <param name="obj">Объект</param>
  /// <returns>Коллекция связей с родительскими объектами. null, если связей нет</returns>
  IReadOnlyCollection<IXmlRelation> GetObjParentRelations(IXmlObject obj);

  /// <summary>Проход по дереву связей объекта.</summary>
  /// <param name="startFromObj">Объект, с которого начать обход. Not null</param>
  /// <param name="filter">Фильтр связей.Если null - обход всех связей.</param>
  /// <param name="action">Действие при посещении очередного объекта.Если null, то все действие в OnFilterObject().</param>
  /// <param name="recursive">Проход по всем уровням вложенности.Если false, то будет осуществлен проход только по узлу
  /// и его непосредственным дочерним.</param>
  /// <param name="bundle">Дополнительные пользовательские данные.</param>
  void Traverse(
    IXmlObject startFromObj,
    OnFilterObject filter = null,
    OnVisitObject action = null,
    bool recursive = false);

  /// <summary>Проход по дереву связей объекта.</summary>
  /// <param name="startFromObj">Объект, с которого начать обход. Not null</param>
  /// <param name="filter">Фильтр связей.Если null - обход всех связей.</param>
  /// <param name="action">Действие при посещении очередного объекта.Если null, то все действие в OnFilterObject().</param>
  /// <param name="recursive">Проход по всем уровням вложенности.Если false, то будет осуществлен проход только по узлу
  /// и его непосредственным дочерним.</param>
  /// <param name="bundle">Дополнительные пользовательские данные.</param>
  /// <remarks>Параметр bundle может использоваться например для передачи на нижние уровни реккурсии результатов работы функции на более высоких уровнях.</remarks>
  void Traverse<TBundle>(
    IXmlObject startFromObj,
    OnFilterObject<TBundle> filter = null,
    OnVisitObject<TBundle> action = null,
    bool recursive = false,
    TBundle bundle = null);
}
