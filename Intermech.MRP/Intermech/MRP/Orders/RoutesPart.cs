// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.RoutesPart
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Queries;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MRP.Orders;

/// <summary>
/// Реализует часть элемента навигации, работающую со списком объектов,
/// входящих в состав указанного объекта. Для чтения объектов используется
/// коллекция связей объектов, что позволяет получать значения как атрибутов
/// объектов, так и атрибутов связей.
/// </summary>
internal class RoutesPart : RelatedObjectsPart
{
  /// <summary>Составное значение "Входимость - Сборка"</summary>
  private NodeColumnID ncEntersInArticle;
  /// <summary>Составное значение "Маршрут по умолчанию"</summary>
  private NodeColumnID ncIsDefaultRoute;
  /// <summary>Составные описания дополнительных атрибутов</summary>
  private List<NodeColumnID> ncAdvAttributes;
  /// <summary>Контейнер сервисов</summary>
  private IServiceProvider _services;
  /// <summary>
  /// Уникальный ключ настроек фильтрации состава.
  /// Если фильтрация состава не требуется, можно
  /// указать константу Intermech.SystemGUIDs.filtrationAllVersions.
  /// </summary>
  private string _filtrationOwnerID;
  /// <summary>Контексты, в рамках которых будет получен состав</summary>
  private List<long> _contexts;
  /// <summary>
  /// Список дополнительных идентификаторов атрибутов, которые будут загружаться в узел независимо от видимых колонок
  /// </summary>
  private List<NodeColumnID> _attributes = new List<NodeColumnID>();

  /// <summary>
  /// Конструктор части, позволяющий указать обрабатываемый объект и роль связанных
  /// с ним объектов. Созданная часть будет возвращать все объекты из
  /// состава/применяемости обрабатываемого объекта, связанные с ним указанным типом связи.
  /// </summary>
  /// <param name="services">Контейнер сервисов</param>
  /// <param name="projObjTypeID">Идентификатор типа родительского объекта.</param>
  /// <param name="projID">Идентификатор версии родительского объекта.</param>
  /// <param name="relationTypeID">Тип связи, по которому надо получить состав</param>
  /// <param name="filtrationOwnerID">Уникальный ключ настроек фильтрации состава.
  /// Если фильтрация состава не требуется, можно указать константу Intermech.SystemGUIDs.filtrationAllVersions.</param>
  /// <param name="contexts">Список контекстов, в рамках которых будет считываться состав</param>
  /// <param name="attributes">Список дополнительных идентификаторов атрибутов, которые будут загружаться в узел независимо от видимых колонок</param>
  public RoutesPart(
    IServiceProvider services,
    int projObjTypeID,
    long projID,
    int relationTypeID,
    string filtrationOwnerID,
    List<long> contexts,
    List<NodeColumnID> attributes)
    : base(projObjTypeID, projID, RelatedObjectsRole.Composition, relationTypeID, services)
  {
    this._services = services;
    this._filtrationOwnerID = filtrationOwnerID;
    this._contexts = contexts;
    this.ncAdvAttributes = attributes == null || attributes.Count <= 0 ? (List<NodeColumnID>) null : new List<NodeColumnID>(attributes.Count);
    this._attributes = attributes;
    if (attributes == null)
      return;
    for (int index = 0; index < attributes.Count; ++index)
      this.ncAdvAttributes.Add(attributes[index].Clone() as NodeColumnID);
  }

  /// <summary>
  /// Создать описание корневого узла на основании данных, полученных из запроса
  /// </summary>
  /// <param name="fieldValues">Значения атрибутов</param>
  /// <param name="adapter">Преобразователь</param>
  /// <returns>Описание корневого узла</returns>
  public override INodeID CreateNodeId(object[] fieldValues, RecordAdapter adapter)
  {
    int int32_1 = Convert.ToInt32(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncF_OBJECT_TYPE)]);
    long int64_1 = Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncF_OBJECT_ID)]);
    long int64_2 = Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncF_ID)]);
    long int64_3 = Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) RelatedPartBase.ncF_PRJLINK_ID)]);
    long int64_4 = Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncMODIFICATION_ID)]);
    int int32_2 = Convert.ToInt32(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncF_LC_STEP)]);
    string caption = Convert.ToString(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncCAPTION)]);
    long int64Value = fieldValues[adapter.GetFieldIndex((object) this.ncEntersInArticle)] != DBNull.Value ? DataSetProcessor.GetInt64Value(fieldValues[adapter.GetFieldIndex((object) this.ncEntersInArticle)], 0L) : 0L;
    string isDefaultRoute = fieldValues[adapter.GetFieldIndex((object) this.ncIsDefaultRoute)] != DBNull.Value ? Convert.ToString(fieldValues[adapter.GetFieldIndex((object) this.ncIsDefaultRoute)]) : string.Empty;
    long int64_5 = Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncF_CHKOUT_BY)]);
    long int64_6 = Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncOWNER)]);
    long int64_7 = adapter.GetFieldIndex((object) ObjectsPartBase.ncSORTING) < 0 || fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncSORTING)] == DBNull.Value ? 0L : Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncSORTING)]);
    long int64_8 = adapter.GetFieldIndex((object) ObjectsPartBase.ncVERSION) < 0 || fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncVERSION)] == DBNull.Value ? 0L : Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncVERSION)]);
    long int64_9 = adapter.GetFieldIndex((object) ObjectsPartBase.ncBASE_VERSION) < 0 || fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncBASE_VERSION)] == DBNull.Value ? 0L : Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncBASE_VERSION)]);
    string siteID = adapter.GetFieldIndex((object) ObjectsPartBase.ncSITE_ID) < 0 || fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncSITE_ID)] == DBNull.Value ? string.Empty : Convert.ToString(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncSITE_ID)]);
    object[] values = this.ncAdvAttributes != null ? new object[this.ncAdvAttributes.Count] : (object[]) null;
    if (this.ncAdvAttributes != null)
    {
      for (int index = 0; index < this.ncAdvAttributes.Count; ++index)
      {
        int fieldIndex = adapter.GetFieldIndex((object) this.ncAdvAttributes[index]);
        values[index] = fieldIndex >= 0 ? fieldValues[fieldIndex] : (object) null;
        values[index] = values[index] != DBNull.Value ? values[index] : (object) null;
      }
    }
    return (INodeID) new RoutesNodeID((CreateObjectNodeParams) new CreateRoutesNodeParams(int32_1, int64_1, int64_2, int64_5, int64_3, int32_2, caption, this._relTypeID, int64_6, int64_7, ObjectFiltrationState.fsNotRequired, int64_8, int64_9, siteID, int64_4, this._filtrationOwnerID, this._contexts, this._objTypeID, this._objID, this._attributes, values, int64Value, isDefaultRoute), this._services);
  }

  /// <summary>
  /// Возвращает коллекцию колонок, которые должны отображаться в гриде
  /// для данного элемента. Используется только в том случае, если для
  /// данного элемента нет сохраненных в конфиграции пользователя
  /// настроек отображения грида.
  /// </summary>
  /// <returns>Коллекция виртуальных колонок навигатора</returns>
  public override NodeColumnCollection GetDefaultColumns()
  {
    NodeColumnCollection defaultColumns = new NodeColumnCollection();
    Guid columnSchemeGuid = Intermech.Navigator.Consts.ObjectObligatoryColumnSchemeGuid;
    IColumnSchemes service = (IColumnSchemes) ServicesManager.GetService(typeof (IColumnSchemes));
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      int attributeId1 = sessionKeeper.Session.IdentHelper.GetAttributeID("cad0001f-306c-11d8-b4e9-00304f19f545");
      defaultColumns.Add(service.CreateColumn(Intermech.Navigator.Consts.ObjectColumnSchemeGuid, (object) attributeId1, NodeColumnSortOrder.None, -1), 300);
      int attributeId2 = sessionKeeper.Session.IdentHelper.GetAttributeID("cad00020-306c-11d8-b4e9-00304f19f545");
      defaultColumns.Add(service.CreateColumn(Intermech.Navigator.Consts.ObjectColumnSchemeGuid, (object) attributeId2, NodeColumnSortOrder.None, -1), 300);
      int attributeId3 = sessionKeeper.Session.IdentHelper.GetAttributeID("cad005b9-306c-11d8-b4e9-00304f19f545");
      defaultColumns.Add(service.CreateColumn(Intermech.Navigator.Consts.ObjectColumnSchemeGuid, (object) attributeId3, NodeColumnSortOrder.None, -1), 250);
    }
    defaultColumns.Add(service.CreateColumn(Intermech.Navigator.Consts.NavigatorColumnSchemeGuid, (object) "F_STATUSES"), 100);
    return defaultColumns;
  }

  /// <summary>
  /// Возвращает коллекцию всех поддерживаемых данным элементом
  /// виртуальных колонок навигатора. Этот метод используется диалогом
  /// настройки отображения грида.
  /// </summary>
  /// <param name="ColumnSetName">Название набора колонок.
  /// Intermech.Navigator.Consts.NavigatorDefaultColumnSetName - набор колонок по умолчанию</param>
  /// <returns>Коллекция виртуальных колонок навигатора</returns>
  public override NodeColumnCollection GetSupportedColumns(string ColumnSetName)
  {
    NodeColumnCollection columns = new NodeColumnCollection();
    Helper.AddObjectTypeColumns(columns, this._objTypeID);
    Helper.AddRelationTypeColumns(columns, this._relTypeID);
    Helper.AddObligatoryColumns(columns, true, true);
    Helper.AddObligatoryColumnsAdv(columns);
    Helper.AddObligatoryColumnsRelation(columns);
    Helper.AddObligatoryColumnsRelationAdv(columns);
    Helper.AddAllColumns(columns);
    Helper.AddAllColumnsRelation(columns);
    return columns;
  }

  /// <summary>
  /// Возвращает интерфейс объекта-запроса, с помощью которого эта часть
  /// читает список обрабатываемых ею объектов.
  /// </summary>
  /// <param name="conditions">Массив условий, которым должны удовлетворять объекты.</param>
  /// <returns>Ссылка на интерфейс объекта-запроса.</returns>
  protected override INodeQuery GetQuery(ConditionStructure[] conditions)
  {
    if (this._relTypeID != -1)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        if (sessionKeeper.Session.GetRelationsApplicabilityCollection().GetApplicabilitiesList(this._relTypeID, -1, this._objTypeID).Rows.Count == 0)
          return (INodeQuery) null;
      }
    }
    return (INodeQuery) new RoutesQuery(this._services, (INodeQuerySupport) this, this._objID, this._objTypeID, this._role, this._relTypeID, conditions, this._filtrationOwnerID, this._contexts);
  }

  /// <summary>
  /// Вернуть список служебных полей, которые всегда загружаются вместе с составом
  /// </summary>
  /// <returns>Список служебных полей, которые всегда загружаются вместе с составом</returns>
  public override List<object> GetSpecialFields()
  {
    List<object> specialFields = base.GetSpecialFields();
    if (!specialFields.Contains((object) ObjectsPartBase.ncF_ID))
      specialFields.Add((object) ObjectsPartBase.ncF_ID);
    if (!specialFields.Contains((object) ObjectsPartBase.ncCAPTION))
      specialFields.Add((object) ObjectsPartBase.ncCAPTION);
    if (!specialFields.Contains((object) ObjectsPartBase.ncOWNER))
      specialFields.Add((object) ObjectsPartBase.ncOWNER);
    if (!specialFields.Contains((object) ObjectsPartBase.ncSORTING))
      specialFields.Add((object) ObjectsPartBase.ncSORTING);
    if (!specialFields.Contains((object) ObjectsPartBase.ncVERSION))
      specialFields.Add((object) ObjectsPartBase.ncVERSION);
    if (!specialFields.Contains((object) ObjectsPartBase.ncBASE_VERSION))
      specialFields.Add((object) ObjectsPartBase.ncBASE_VERSION);
    if (!specialFields.Contains((object) ObjectsPartBase.ncSITE_ID))
      specialFields.Add((object) ObjectsPartBase.ncSITE_ID);
    if (!specialFields.Contains((object) ObjectsPartBase.ncMODIFICATION_ID))
      specialFields.Add((object) ObjectsPartBase.ncMODIFICATION_ID);
    if (this.ncEntersInArticle == null)
    {
      using (new SessionKeeper())
      {
        this.ncEntersInArticle = new NodeColumnID((object) MetaDataHelper.GetAttributeTypeID("cad001d5-306c-11d8-b4e9-00304f19f545"), AttributeSourceTypes.Object);
        this.ncIsDefaultRoute = new NodeColumnID((object) MetaDataHelper.GetAttributeTypeID("cad005b9-306c-11d8-b4e9-00304f19f545"), AttributeSourceTypes.Relation);
      }
    }
    if (!specialFields.Contains((object) this.ncEntersInArticle))
      specialFields.Add((object) this.ncEntersInArticle);
    if (!specialFields.Contains((object) this.ncIsDefaultRoute))
      specialFields.Add((object) this.ncIsDefaultRoute);
    if (!specialFields.Contains((object) ObjectsPartBase.ncF_LC_STEP))
      specialFields.Add((object) ObjectsPartBase.ncF_LC_STEP);
    if (this.ncAdvAttributes != null)
    {
      for (int index = 0; index < this.ncAdvAttributes.Count; ++index)
      {
        if (!specialFields.Contains((object) this.ncAdvAttributes[index]))
          specialFields.Add((object) this.ncAdvAttributes[index]);
      }
    }
    return specialFields;
  }

  /// <summary>Вернуть дочерний узел на основании его описания</summary>
  /// <param name="nodeID">Описание дочернего узла</param>
  /// <returns>Дочерний узел на основании его описания или null</returns>
  public override INode GetChild(INodeID nodeID)
  {
    return nodeID is RoutesNodeID routesNodeId ? (INode) new RoutesNode(routesNodeId.Services, routesNodeId.FiltrationOwnerID, routesNodeId.Contexts, routesNodeId.ProjObjType, routesNodeId.ProjID, routesNodeId.ObjectID, routesNodeId.ObjectTypeID, routesNodeId.RelationTypeID, routesNodeId.PrjLinkID, routesNodeId.LCStepID, routesNodeId.Caption, routesNodeId.EntersInArticle, routesNodeId.IsDefaultRoute, routesNodeId.CheckedOutBy, routesNodeId.Owner, routesNodeId.Sorting, routesNodeId.Version, routesNodeId.BaseVersion, routesNodeId.Attributes, routesNodeId.Values) : (INode) null;
  }
}
