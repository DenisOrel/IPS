// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.RoutesNode
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.Kernel.Search;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Intermech.MRP.Orders;

/// <summary>
/// Узел для получения состава указанного объекта по определённому типу связи
/// </summary>
internal class RoutesNode : CompositeNode
{
  /// <summary>Контейнер сервисов</summary>
  private IServiceProvider _services;
  /// <summary>Категория</summary>
  private int _categoryID;
  /// <summary>Тип</summary>
  private int _typeID;
  /// <summary>
  /// Уникальный ключ настроек фильтрации состава.
  /// Если фильтрация состава не требуется, можно
  /// указать константу Intermech.SystemGUIDs.filtrationAllVersions.
  /// </summary>
  private string _filtrationOwnerID;
  /// <summary>Контексты, в рамках которых будет получен состав</summary>
  private List<long> _contexts;
  /// <summary>Идентификатор типа родительского объекта</summary>
  private int _projObjType;
  /// <summary>Идентификатор версии родительского объекта</summary>
  private long _projID;
  /// <summary>Идентификатор типа объекта</summary>
  private int _objType;
  /// <summary>Идентификатор версии объекта</summary>
  private long _objID;
  /// <summary>
  /// Идентификатор типа связи, по которой связаны объект и его родительский объект
  /// </summary>
  private int _relationTypeID;
  /// <summary>Идентификатор связи</summary>
  private long _prjLinkID;
  /// <summary>Шаг ЖЦ</summary>
  private int _lcStepID;
  /// <summary>Заголовок объекта</summary>
  private string _caption;
  /// <summary>Входимость - Сборка</summary>
  private long _entersInArticle;
  /// <summary>Маршрут по умолчанию</summary>
  private string _isDefaultRoute;
  /// <summary>Кем объект взят на изменение</summary>
  private long _checkedOutBy;
  /// <summary>Владелец объекта</summary>
  private long _owner;
  /// <summary>Значение атрибута "Сортировка"</summary>
  private long _sorting;
  /// <summary>Версия объекта</summary>
  private long _version;
  /// <summary>Признак базовой версии</summary>
  private long _baseVersion;
  /// <summary>
  /// Список дополнительных идентификаторов атрибутов, которые будут загружаться в узел независимо от видимых колонок.
  /// ВНИМАНИЕ!!! В качестве ID можно использовать только Int32 !!!
  /// </summary>
  private List<NodeColumnID> _attributes = new List<NodeColumnID>();
  /// <summary>Список значений дополнительных атрибутов</summary>
  private object[] _values = new object[0];

  /// <summary>Категория</summary>
  internal int CategoryID
  {
    [DebuggerStepThrough] get => this._categoryID;
  }

  /// <summary>Тип</summary>
  internal int TypeID
  {
    [DebuggerStepThrough] get => this._typeID;
  }

  /// <summary>
  /// Уникальный ключ настроек фильтрации состава.
  /// Если фильтрация состава не требуется, можно
  /// указать константу Intermech.SystemGUIDs.filtrationAllVersions.
  /// </summary>
  internal string FiltrationOwnerID
  {
    [DebuggerStepThrough] get => this._filtrationOwnerID;
  }

  /// <summary>Контексты, в рамках которых будет получен состав</summary>
  internal List<long> Contexts
  {
    [DebuggerStepThrough] get => this._contexts;
  }

  /// <summary>Идентификатор типа родительского объекта</summary>
  internal int ProjObjType
  {
    [DebuggerStepThrough] get => this._projObjType;
  }

  /// <summary>Идентификатор версии родительского объекта</summary>
  internal long ProjID
  {
    [DebuggerStepThrough] get => this._projID;
  }

  /// <summary>Идентификатор версии объекта</summary>
  internal long ObjID
  {
    [DebuggerStepThrough] get => this._objID;
  }

  /// <summary>Идентификатор типа объекта</summary>
  internal int ObjType
  {
    [DebuggerStepThrough] get => this._objType;
  }

  /// <summary>
  /// Идентификатор типа связи, по которой будет получен состав
  /// </summary>
  internal int RelationTypeID
  {
    [DebuggerStepThrough] get => this._relationTypeID;
  }

  /// <summary>Идентификатор связи</summary>
  internal long PrjLinkID
  {
    [DebuggerStepThrough] get => this._prjLinkID;
  }

  /// <summary>Шаг ЖЦ</summary>
  internal int LCStepID
  {
    [DebuggerStepThrough] get => this._lcStepID;
  }

  /// <summary>Заголовок объекта</summary>
  internal string Caption
  {
    [DebuggerStepThrough] get => this._caption;
  }

  /// <summary>Входимость - Сборка</summary>
  internal long EntersInArticle
  {
    [DebuggerStepThrough] get => this._entersInArticle;
  }

  /// <summary>Маршрут по умолчанию</summary>
  internal string IsDefaultRoute
  {
    [DebuggerStepThrough] get => this._isDefaultRoute;
  }

  /// <summary>Кем объект взят на изменение</summary>
  internal long CheckedOutBy
  {
    [DebuggerStepThrough] get => this._checkedOutBy;
  }

  /// <summary>Владелец объекта</summary>
  internal long Owner
  {
    [DebuggerStepThrough] get => this._owner;
  }

  /// <summary>Значение атрибута "Сортировка"</summary>
  internal long Sorting
  {
    [DebuggerStepThrough] get => this._sorting;
  }

  /// <summary>Версия объекта</summary>
  internal long Version
  {
    [DebuggerStepThrough] get => this._version;
  }

  /// <summary>Признак базовой версии</summary>
  internal long BaseVersion
  {
    [DebuggerStepThrough] get => this._baseVersion;
  }

  /// <summary>
  /// Список дополнительных идентификаторов атрибутов, которые будут загружаться в узел независимо от видимых колонок
  /// </summary>
  internal List<NodeColumnID> Attributes
  {
    [DebuggerStepThrough] get => this._attributes;
  }

  /// <summary>Список значений дополнительных атрибутов</summary>
  internal object[] Values
  {
    [DebuggerStepThrough] get => this._values;
  }

  /// <summary>Значение указанного атрибута</summary>
  /// <param name="attributeID">Идентификатор атрибута</param>
  /// <returns>null, если значение атрибута не найдено</returns>
  internal object this[int attributeID]
  {
    get
    {
      for (int index = 0; index < this._attributes.Count; ++index)
      {
        if (this._attributes[index].ID.Equals((object) attributeID))
          return this._values[index];
      }
      return (object) null;
    }
  }

  /// <summary>Базовый конструктор</summary>
  /// <param name="services">Контейнер сервисов</param>
  /// <param name="filtrationOwnerID">Уникальный ключ настроек фильтрации состава</param>
  /// <param name="contexts">Список контекстов, в рамках которых будет считываться состав</param>
  /// <param name="projObjType">Тип родительского объекта</param>
  /// <param name="projID">Идентификатор родительского объекта</param>
  /// <param name="objID">Идентификатор версии обрабатываемого объекта.</param>
  /// <param name="objType">Тип обрабатываемого объекта</param>
  /// <param name="relationTypeID">Тип связи, по которому связаны объект и его родительский объект</param>
  /// <param name="prjLinkID">Идентификатор связи между объектом и его родительским объектом</param>
  /// <param name="lcStepID">Шаг ЖЦ</param>
  /// <param name="caption">Заголовок объекта</param>
  /// <param name="entersInArticle">Входимость - Сборка</param>
  /// <param name="isDefaultRoute">Маршрут по умолчанию</param>
  /// <param name="checkedOutBy">Кем объект взят на изменение</param>
  /// <param name="owner">Владелец объекта</param>
  /// <param name="sorting">Значение атрибута "Сортировка"</param>
  /// <param name="version">Версия объекта</param>
  /// <param name="baseVersion">Признак базовой версии</param>
  /// <param name="attributes">Список дополнительных идентификаторов атрибутов, которые будут загружаться в узел независимо от видимых колонок</param>
  /// <param name="values">Список значений дополнительных атрибутов</param>
  public RoutesNode(
    IServiceProvider services,
    string filtrationOwnerID,
    List<long> contexts,
    int projObjType,
    long projID,
    long objID,
    int objType,
    int relationTypeID,
    long prjLinkID,
    int lcStepID,
    string caption,
    long entersInArticle,
    string isDefaultRoute,
    long checkedOutBy,
    long owner,
    long sorting,
    long version,
    long baseVersion,
    List<NodeColumnID> attributes,
    object[] values)
  {
    this._services = services;
    this._filtrationOwnerID = filtrationOwnerID;
    this._contexts = contexts;
    this._projObjType = projObjType;
    this._projID = projID;
    this._objID = objID;
    this._objType = objType;
    this._relationTypeID = relationTypeID;
    this._prjLinkID = prjLinkID;
    this._lcStepID = lcStepID;
    this._caption = caption;
    this._entersInArticle = entersInArticle;
    this._isDefaultRoute = isDefaultRoute;
    this._checkedOutBy = checkedOutBy;
    this._owner = owner;
    this._sorting = sorting;
    this._version = version;
    this._baseVersion = baseVersion;
    this._attributes = attributes;
    this._values = values;
    this.options = NodeOptions.CanContainsComposition;
  }

  /// <summary>
  /// Создает и возвращает часть, которая отвечает за дочерние элементы-папки.
  /// </summary>
  /// <returns>Ссылка на интерфейс части</returns>
  protected override List<PartSlot> CreateFolderSlots()
  {
    if (this._projID != -1L)
      return this.SlotsFromSinglePart((INodePart) new RoutesPart(this._services, this._projObjType, this._projID, this._relationTypeID, this._filtrationOwnerID, this._contexts, this._attributes));
    return this._objID != -1L ? this.SlotsFromSinglePart((INodePart) new RoutesPart(this._services, this._objType, this._objID, this._relationTypeID, this._filtrationOwnerID, this._contexts, this._attributes)) : (List<PartSlot>) null;
  }
}
