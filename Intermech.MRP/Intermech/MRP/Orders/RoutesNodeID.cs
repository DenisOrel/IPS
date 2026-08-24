// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.RoutesNodeID
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.Kernel.Search;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Intermech.MRP.Orders;

/// <summary>Описание корневого узла состава</summary>
internal class RoutesNodeID : AdvRelationsNodeID
{
  /// <summary>Контейнер сервисов</summary>
  private IServiceProvider _services;

  /// <summary>Контейнер сервисов</summary>
  public IServiceProvider Services
  {
    [DebuggerStepThrough] get => this._services;
  }

  /// <summary>Входимость - Сборка</summary>
  public long EntersInArticle
  {
    [DebuggerStepThrough] get => (this.pars as CreateRoutesNodeParams).EntersInArticle;
  }

  /// <summary>Маршрут по умолчанию</summary>
  public string IsDefaultRoute
  {
    [DebuggerStepThrough] get => (this.pars as CreateRoutesNodeParams).IsDefaultRoute;
  }

  /// <summary>
  /// Список дополнительных идентификаторов атрибутов, которые будут загружаться в узел независимо от видимых колонок
  /// </summary>
  public List<NodeColumnID> Attributes
  {
    [DebuggerStepThrough] get => (this.pars as CreateRoutesNodeParams).Attributes;
  }

  /// <summary>
  /// Список дополнительных идентификаторов атрибутов, которые будут загружаться в узел независимо от видимых колонок
  /// </summary>
  public new object[] Values
  {
    [DebuggerStepThrough] get => (this.pars as CreateRoutesNodeParams).Values;
  }

  /// <summary>Значение указанного атрибута</summary>
  /// <param name="attributeID">Идентификатор атрибута</param>
  /// <returns>null, если значение атрибута не найдено</returns>
  public new object this[int attributeID]
  {
    get
    {
      for (int index = 0; index < (this.pars as CreateRoutesNodeParams).Attributes.Count; ++index)
      {
        if ((this.pars as CreateRoutesNodeParams).Attributes[index].ID.Equals((object) attributeID))
          return (this.pars as CreateRoutesNodeParams).Values[index];
      }
      return (object) null;
    }
  }

  /// <summary>Создать новый узел</summary>
  /// <param name="e">Параметры</param>
  /// <param name="services">Контейнер сервисов</param>
  public RoutesNodeID(CreateObjectNodeParams e, IServiceProvider services)
    : base(e)
  {
    this._services = services;
    this.pars = (CreateObjectNodeParams) new CreateRoutesNodeParams((object) e);
  }

  /// <summary>Выполнить сравнение с указанным объектом</summary>
  /// <param name="obj">Объект для сравнения</param>
  /// <returns>true, если объекты равны</returns>
  public override bool Equals(object obj)
  {
    return !(obj is RoutesNodeID routesNodeId) ? base.Equals(obj) : routesNodeId.PrjLinkID == this.PrjLinkID;
  }

  /// <summary>Вернуть 32-битный хэш-код экземпляра класса</summary>
  /// <returns>32-битный хэш-код экземпляра класса</returns>
  [DebuggerStepThrough]
  public override int GetHashCode() => this.PrjLinkID.GetHashCode();
}
