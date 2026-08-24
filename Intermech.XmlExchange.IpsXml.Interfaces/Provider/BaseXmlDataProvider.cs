// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Interfaces.Provider.BaseXmlDataProvider
// Assembly: Intermech.XmlExchange.IpsXml.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 741EAC98-7C4B-42E4-B0B7-F40794536EF7
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.xml

using System.Collections.Generic;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Interfaces.Provider;

/// <summary>
/// Базовый провайдер данных XML реализующий обобщенный алгоритм обхода дерева объектов.
/// </summary>
public abstract class BaseXmlDataProvider : IXmlDataProvider
{
  public abstract IReadOnlyCollection<IXmlObject> RootObjects { get; }

  public abstract IReadOnlyCollection<IXmlObject> GetAllObjects();

  public abstract IXmlObject GetRelChildObj(IXmlRelation rel);

  public abstract IXmlObject GetRelParentObj(IXmlRelation rel);

  public void Traverse(
    IXmlObject startFromObj,
    OnFilterObject filter = null,
    OnVisitObject action = null,
    bool recursive = false)
  {
    bool stopTraversing = false;
    if (filter == null || filter((IXmlObject) null, startFromObj, (IXmlRelation) null, ref stopTraversing))
    {
      if (stopTraversing)
        return;
      if (action != null)
        action((IXmlObject) null, startFromObj, (IXmlRelation) null, ref stopTraversing);
    }
    if (stopTraversing)
      return;
    IReadOnlyCollection<IXmlRelation> objChildRelations = this.GetObjChildRelations(startFromObj);
    if (objChildRelations == null)
      return;
    foreach (IXmlRelation rel in (IEnumerable<IXmlRelation>) objChildRelations)
    {
      this.InternalTraverse(this.GetRelParentObj(rel), this.GetRelChildObj(rel), rel, filter, action, recursive, out stopTraversing);
      if (stopTraversing)
        break;
    }
  }

  public void Traverse<TBundle>(
    IXmlObject startFromObj,
    OnFilterObject<TBundle> filter = null,
    OnVisitObject<TBundle> action = null,
    bool recursive = false,
    TBundle bundle = null)
  {
    bool stopTraversing = false;
    if (filter == null || filter((IXmlObject) null, startFromObj, (IXmlRelation) null, ref stopTraversing, ref bundle))
    {
      if (stopTraversing)
        return;
      if (action != null)
        action((IXmlObject) null, startFromObj, (IXmlRelation) null, ref stopTraversing, ref bundle);
    }
    if (stopTraversing)
      return;
    IReadOnlyCollection<IXmlRelation> objChildRelations = this.GetObjChildRelations(startFromObj);
    if (objChildRelations == null)
      return;
    foreach (IXmlRelation rel in (IEnumerable<IXmlRelation>) objChildRelations)
    {
      this.InternalTraverse<TBundle>(this.GetRelParentObj(rel), this.GetRelChildObj(rel), rel, filter, action, recursive, out stopTraversing, bundle);
      if (stopTraversing)
        break;
    }
  }

  /// <summary>Внутренняя функция прохода по дереву связей объекта.</summary>
  /// <param name="parentObj">Родительский объект по связи.</param>
  /// <param name="childObj">Дочерний объект по связи.</param>
  /// <param name="rel">Связь которую необходимо посетить.</param>
  /// <param name="filter">Фильтр связей.Если null - обход всех связей.</param>
  /// <param name="action">Действие при посещении очередной связи.</param>
  /// <param name="recursive">Проход по всем уровням вложенности.Если false, то будет осуществлен проход только по текущей связи.</param>
  /// <param name="stopTraversing">Был ли остановлен обход.</param>
  private void InternalTraverse(
    IXmlObject parentObj,
    IXmlObject childObj,
    IXmlRelation rel,
    OnFilterObject filter,
    OnVisitObject action,
    bool recursive,
    out bool stopTraversing)
  {
    stopTraversing = false;
    if (filter == null || filter(parentObj, childObj, rel, ref stopTraversing))
    {
      if (stopTraversing)
        return;
      if (action != null)
        action(parentObj, childObj, rel, ref stopTraversing);
    }
    if (stopTraversing || !recursive)
      return;
    IReadOnlyCollection<IXmlRelation> objChildRelations = this.GetObjChildRelations(childObj);
    if (objChildRelations == null)
      return;
    foreach (IXmlRelation rel1 in (IEnumerable<IXmlRelation>) objChildRelations)
    {
      this.InternalTraverse(this.GetRelParentObj(rel1), this.GetRelChildObj(rel1), rel1, filter, action, true, out stopTraversing);
      if (stopTraversing)
        break;
    }
  }

  /// <summary>Внутренняя функция прохода по дереву связей объекта.</summary>
  /// <param name="parentObj">Родительский объект по связи.</param>
  /// <param name="childObj">Дочерний объект по связи.</param>
  /// <param name="rel">Связь которую необходимо посетить.</param>
  /// <param name="filter">Фильтр связей.Если null - обход всех связей.</param>
  /// <param name="action">Действие при посещении очередной связи.</param>
  /// <param name="recursive">Проход по всем уровням вложенности.Если false, то будет осуществлен проход только по текущей связи.</param>
  /// <param name="stopTraversing">Был ли остановлен обход.</param>
  /// <param name="bundle">Дополнительные пользовательские данные.</param>
  private void InternalTraverse<TBundle>(
    IXmlObject parentObj,
    IXmlObject childObj,
    IXmlRelation rel,
    OnFilterObject<TBundle> filter,
    OnVisitObject<TBundle> action,
    bool recursive,
    out bool stopTraversing,
    TBundle bundle = null)
  {
    stopTraversing = false;
    if (filter == null || filter(parentObj, childObj, rel, ref stopTraversing, ref bundle))
    {
      if (stopTraversing)
        return;
      if (action != null)
        action(parentObj, childObj, rel, ref stopTraversing, ref bundle);
    }
    if (stopTraversing || !recursive)
      return;
    IReadOnlyCollection<IXmlRelation> objChildRelations = this.GetObjChildRelations(childObj);
    if (objChildRelations == null)
      return;
    foreach (IXmlRelation rel1 in (IEnumerable<IXmlRelation>) objChildRelations)
    {
      this.InternalTraverse<TBundle>(this.GetRelParentObj(rel1), this.GetRelChildObj(rel1), rel1, filter, action, true, out stopTraversing, bundle);
      if (stopTraversing)
        break;
    }
  }

  /// <summary>Получить коллекцию связей с дочерними объектами.</summary>
  /// <param name="obj">Родительский объект</param>
  /// <returns>Коллекция связей с дочерними объектами</returns>
  public abstract IReadOnlyCollection<IXmlRelation> GetObjChildRelations(IXmlObject obj);

  /// <summary>Получение связей с родительскими объектами</summary>
  /// <param name="obj">Объект</param>
  /// <returns>Коллекция связей с родительскими объектами</returns>
  public abstract IReadOnlyCollection<IXmlRelation> GetObjParentRelations(IXmlObject obj);
}
