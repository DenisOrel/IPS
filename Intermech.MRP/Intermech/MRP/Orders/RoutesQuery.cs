// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.RoutesQuery
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

#nullable disable
namespace Intermech.MRP.Orders;

/// <summary>
/// Реализует запрос к базе данных на чтение инфрормации об объектах из
/// коллекции связей объектов, т.е. позволяет прочитать значения атрибутов не
/// только объектов, но и связей. Результаты запроса возвращаются в
/// унифицированном формате, воспринимаемом навигатором, т.е. для каждого
/// объекта предоставляется его идентификатор, поддерживающий интерфейс INodeID,
/// и значения указанных виртуальных колонок.
/// </summary>
internal class RoutesQuery : RelatedObjectsQuery, IFiltrationClass
{
  /// <summary>
  /// Идентификатор типа связи, по которой будет получен состав
  /// </summary>
  private int _relationTypeID;
  /// <summary>Идентификатор дочернего типа объектов состава</summary>
  private int _childTypeID;
  /// <summary>
  /// Уникальный ключ настроек фильтрации состава.
  /// Если фильтрация состава не требуется, можно
  /// указать константу Intermech.SystemGUIDs.filtrationAllVersions.
  /// </summary>
  private string _filtrationOwnerID;
  /// <summary>Контексты, в рамках которых будет получен состав</summary>
  private List<long> _contexts;

  /// <summary>
  /// Конструктор запроса, в результате выполнения которого будет прочитана
  /// информация о всех объектах, связанных с указанным объектом заданным
  /// типом связи и удовлетворяющих указанным условиям.
  /// </summary>
  /// <param name="services">Контейнер сервисов</param>
  /// <param name="objTypeID"></param>
  /// <param name="support"></param>
  /// <param name="objId">Идентификатор объекта</param>
  /// <param name="role">Роль связанных объектов</param>
  /// <param name="relTypeId">Идентификатор типа связи</param>
  /// <param name="conditions">Массив условий, которым должны удовлетворять связанные объекты</param>
  /// <param name="filtrationOwnerID">Уникальный ключ настроек фильтрации состава.
  /// Если фильтрация состава не требуется, можно указать константу Intermech.SystemGUIDs.filtrationAllVersions.</param>
  /// <param name="contexts">Список контекстов, в рамках которых будет считываться состав</param>
  public RoutesQuery(
    IServiceProvider services,
    INodeQuerySupport support,
    long objId,
    int objTypeID,
    RelatedObjectsRole role,
    int relTypeId,
    ConditionStructure[] conditions,
    string filtrationOwnerID,
    List<long> contexts)
    : base(support, objId, objTypeID, role, relTypeId, conditions)
  {
    this.Services = services;
    RoutesDescriptor.CorrectStatics();
    this._relationTypeID = relTypeId != -1 ? relTypeId : RoutesDescriptor.DefaultRelationTypeID;
    this._childTypeID = RoutesDescriptor.ChildTypeID;
    this._filtrationOwnerID = filtrationOwnerID != string.Empty ? filtrationOwnerID : "cad001e2-306c-11d8-b4e9-00304f19f545";
    if (contexts == null || contexts.Count <= 0)
      return;
    this._contexts = contexts;
  }

  /// <summary>
  /// Добавляет к параметрам запроса условия, указанные в конструкторе
  /// запроса. Этот метод используется при чтении первой/следующей части
  /// списка объектов.
  /// </summary>
  /// <param name="mapping">Схема отображения виртуальных колонок в поля источника данных</param>
  /// <param name="bookmark">Закладка, определяющая позицию для чтения порции</param>
  /// <param name="count">Количество записей, которое должно быть прочитано</param>
  /// <returns>Параметры запроса к базе данных</returns>
  protected override DBRecordSetParams GetQueryParams(
    object bookmark,
    int count,
    RecordMapping mapping)
  {
    DBRecordSetParams queryParams = base.GetQueryParams(bookmark, count, mapping);
    if (this._contexts != null && queryParams.Tags != null)
      queryParams.Tags[(object) "{AB419A02-DE8A-4A8E-905A-D782F5B720E5}"] = (object) this._contexts;
    return queryParams;
  }

  /// <summary>
  /// Возвращает таблицу, содержащую результаты запроса. Базовый класс
  /// вызывает этот метод, чтобы получить результаты запроса в формате
  /// источника данных, а затем транслирует их в унифицированный формат,
  /// понятный навигатору.
  /// </summary>
  /// <param name="queryParams">Параметры запроса к базе данных</param>
  /// <returns>Таблица с значениями атрибутов объектов</returns>
  protected override DataTable GetDataTable(DBRecordSetParams queryParams)
  {
    this._log[this.relTypeId] = (Dictionary<FiltrateVersionsLogEntryKey, FiltrateVersionsLogEntry>) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(this._relationTypeID, this._filtrationOwnerID);
      relationCollection.ObjectTypeID = this._childTypeID;
      DataTable dataTable = relationCollection.Select(queryParams);
      if (dataTable != null && dataTable.ExtendedProperties.ContainsKey((object) FiltrateVersionsLog.Key))
      {
        this._log.AssignRelTypeLog(dataTable.ExtendedProperties[(object) FiltrateVersionsLog.Key]);
        if (this.Services is AdvancedServiceContainer services)
        {
          if (services.GetService(typeof (FiltrateVersionsLog)) is FiltrateVersionsLog service)
          {
            service.AssignRelTypeLog((object) this._log.ToString(this.relTypeId));
          }
          else
          {
            FiltrateVersionsLog serviceInstance = new FiltrateVersionsLog();
            serviceInstance.AssignRelTypeLog((object) this._log.ToString(this.relTypeId));
            services.AddService(typeof (FiltrateVersionsLog), (object) serviceInstance);
          }
        }
      }
      return dataTable;
    }
  }

  /// <summary>Ключ настроек фильтрации</summary>
  public string FiltrationOwnerID
  {
    [DebuggerStepThrough] get => this._filtrationOwnerID;
  }
}
