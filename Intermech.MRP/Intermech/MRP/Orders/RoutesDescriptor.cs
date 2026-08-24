// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.RoutesDescriptor
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Kernel.Search;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Persistence;
using Intermech.Navigator.VirtualNodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Intermech.MRP.Orders;

/// <summary>
/// Дескриптор для состава, содержащего только маршруты обработки
/// </summary>
internal class RoutesDescriptor : HiveDescriptor
{
  /// <summary>
  /// Идентификатор типа связи "Технологический состав" - cad0019f-306c-11d8-b4e9-00304f19f545
  /// </summary>
  internal static int DefaultRelationTypeID = -1;
  /// <summary>
  /// Идентификатор типа объекта "Маршрут обработки" - cad0016f-306c-11d8-b4e9-00304f19f545
  /// </summary>
  internal static int ChildTypeID = -1;
  /// <summary>
  /// Уникальный ключ настроек фильтрации состава.
  /// Если фильтрация состава не требуется, можно
  /// указать константу Intermech.SystemGUIDs.filtrationAllVersions.
  /// </summary>
  private string _filtrationOwnerID;
  /// <summary>Контексты, в рамках которых будет получен состав</summary>
  private List<long> _contexts;
  /// <summary>Идентификатор типа корневого объекта</summary>
  private int _objType;
  /// <summary>Идентификатор версии корневого объекта</summary>
  private long _objID;
  /// <summary>Идентификатор корневого объекта</summary>
  private long _ID;
  /// <summary>Идентификатор типа связи по умолчанию</summary>
  private int _relationTypeID;
  /// <summary>Заголовок объекта</summary>
  private new string _caption;
  /// <summary>Кем объект взят на изменение</summary>
  private long _checkedOutBy;
  /// <summary>Версия объекта</summary>
  private long _version;
  /// <summary>Признак базовой версии</summary>
  private long _baseVersion;
  /// <summary>Узлы информационной системы</summary>
  private string _siteID;
  /// <summary>
  /// Список дополнительных идентификаторов атрибутов, которые будут загружаться в узел независимо от видимых колонок.
  /// ВНИМАНИЕ!!! В качестве ID можно использовать ТОЛЬКО Int32 !!!
  /// </summary>
  private List<NodeColumnID> _attributes = new List<NodeColumnID>();
  /// <summary>Список значений дополнительных атрибутов</summary>
  private object[] _values = new object[0];
  /// <summary>Контейнер сервисов</summary>
  private IServiceProvider _services;

  /// <summary>
  /// Установить/откорректировать значения статических полей класса
  /// </summary>
  internal static void CorrectStatics()
  {
    RoutesDescriptor.DefaultRelationTypeID = RoutesDescriptor.DefaultRelationTypeID == -1 ? MetaDataHelper.GetRelationTypeID("cad0019f-306c-11d8-b4e9-00304f19f545") : RoutesDescriptor.DefaultRelationTypeID;
    RoutesDescriptor.ChildTypeID = RoutesDescriptor.ChildTypeID == -1 ? MetaDataHelper.GetObjectTypeID("cad0016f-306c-11d8-b4e9-00304f19f545") : RoutesDescriptor.ChildTypeID;
  }

  /// <summary>
  /// Уникальный ключ настроек фильтрации состава.
  /// Если фильтрация состава не требуется, можно
  /// указать константу Intermech.SystemGUIDs.filtrationAllVersions.
  /// </summary>
  public string FiltrationOwnerID
  {
    [DebuggerStepThrough] get => this._filtrationOwnerID;
    set
    {
      this._filtrationOwnerID = value != string.Empty ? value : "cad001e2-306c-11d8-b4e9-00304f19f545";
    }
  }

  /// <summary>Контексты, в рамках которых будет получен состав</summary>
  public List<long> Contexts
  {
    [DebuggerStepThrough] get => this._contexts;
    set
    {
      if (value == null || value.Count <= 0)
        return;
      this._contexts = new List<long>(value.Count);
      for (int index = 0; index < value.Count; ++index)
        this._contexts.Add(value[index]);
    }
  }

  /// <summary>Идентификатор типа корневого объекта</summary>
  public int ObjType
  {
    [DebuggerStepThrough] get => this._objType;
  }

  /// <summary>Идентификатор версии корневого объекта</summary>
  public long ObjID
  {
    [DebuggerStepThrough] get => this._objID;
    set
    {
      if (this._objID == value)
        return;
      this._objID = value;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(this._objID);
        this._objType = dbObject.ObjectType;
        this._ID = dbObject.ID;
        this._checkedOutBy = dbObject.CheckoutBy;
        this._caption = dbObject.Caption;
        for (int index = 0; index < this._attributes.Count; ++index)
        {
          IDBAttribute byId = dbObject.Attributes.FindByID((int) this._attributes[index].ID);
          this[(int) this._attributes[index].ID] = byId?.Value;
        }
      }
    }
  }

  /// <summary>
  /// Идентификатор типа по умолчанию связи, по которой будет получен состав
  /// </summary>
  public int RelationTypeID
  {
    [DebuggerStepThrough] get => this._relationTypeID;
    set => this._relationTypeID = value >= 0 ? value : RoutesDescriptor.DefaultRelationTypeID;
  }

  /// <summary>Заголовок объекта</summary>
  public new string Caption
  {
    [DebuggerStepThrough] get => this._caption;
    set => this._caption = value;
  }

  /// <summary>Кем объект взят на изменение</summary>
  public long CheckedOutBy
  {
    [DebuggerStepThrough] get => this._checkedOutBy;
    set => this._checkedOutBy = value;
  }

  /// <summary>Версия объекта</summary>
  public long Version
  {
    [DebuggerStepThrough] get => this._version;
    set => this._version = value;
  }

  /// <summary>Признак базовой версии</summary>
  public long BaseVersion
  {
    [DebuggerStepThrough] get => this._baseVersion;
    set => this._baseVersion = value;
  }

  /// <summary>Признак базовой версии</summary>
  public string SiteID
  {
    [DebuggerStepThrough] get => this._siteID;
    set => this._siteID = value;
  }

  /// <summary>
  /// Список дополнительных идентификаторов атрибутов, которые будут загружаться в узел независимо от видимых колонок
  /// </summary>
  public List<NodeColumnID> Attributes
  {
    [DebuggerStepThrough] get => this._attributes;
    set
    {
      this._attributes = value != null ? value : new List<NodeColumnID>();
      if (this._values != null && this._values.Length == this._attributes.Count)
        return;
      this._values = new object[this._attributes.Count];
    }
  }

  /// <summary>Список значений дополнительных атрибутов</summary>
  public object[] Values
  {
    [DebuggerStepThrough] get => this._values;
    set
    {
      this._values = value == null || value.Length != this._attributes.Count ? new object[this._attributes.Count] : value;
    }
  }

  /// <summary>Значение указанного атрибута</summary>
  /// <param name="attributeID">Идентификатор атрибута</param>
  /// <returns>null, если значение атрибута не найдено</returns>
  public object this[int attributeID]
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
    set
    {
      for (int index = 0; index < this._attributes.Count; ++index)
      {
        if (this._attributes[index].ID.Equals((object) attributeID))
          this._values[index] = value;
      }
    }
  }

  /// <summary>
  /// Создает дескриптор элемента навигации состава допустимых замен.
  /// </summary>
  /// <param name="categoryID">Категория</param>
  /// <param name="typeID">Тип</param>
  /// <param name="services">Контейнер сервисом</param>
  /// <param name="filtrationOwnerID">Уникальный ключ настроек фильтрации состава</param>
  /// <param name="contexts">Список контекстов, в рамках которых будет считываться состав</param>
  /// <param name="objID">Идентификатор версии корневого объекта.</param>
  /// <param name="objType">Идентификатор типа корневого объекта</param>
  /// <param name="relationTypeID">Тип связи по умолчанию, по которому надо получать состав</param>
  /// <param name="caption">Заголовок</param>
  /// <param name="checkedOutBy">Кем объект взят на изменение</param>
  /// <param name="version">Версия объекта</param>
  /// <param name="baseVersion">Признак базовой версии</param>
  /// <param name="attributes">Список дополнительных идентификаторов атрибутов, которые будут загружаться в узел независимо от видимых колонок</param>
  public RoutesDescriptor(
    int categoryID,
    int typeID,
    IServiceProvider services,
    string filtrationOwnerID,
    List<long> contexts,
    long objID,
    int objType,
    int relationTypeID,
    string caption,
    long checkedOutBy,
    long version,
    long baseVersion,
    List<NodeColumnID> attributes)
    : base(categoryID, typeID, caption)
  {
    RoutesDescriptor.CorrectStatics();
    this._services = services;
    this.FiltrationOwnerID = filtrationOwnerID;
    this.Contexts = contexts;
    this.CheckedOutBy = checkedOutBy;
    this.Attributes = attributes;
    this.ObjID = objID;
    this.RelationTypeID = relationTypeID;
    this.Version = version;
    this.BaseVersion = baseVersion;
  }

  /// <summary>Cериализовать описание узла</summary>
  /// <param name="nodeID">Описание узла</param>
  /// <returns>Сериализованное представление узла</returns>
  [DebuggerStepThrough]
  public override PersistentState Serialize(INodeID nodeID) => (PersistentState) null;

  /// <summary>Десериализовать описание узла</summary>
  /// <param name="persistNodeID">Сериализованное представление узла</param>
  /// <returns>Описание узла</returns>
  [DebuggerStepThrough]
  public new virtual INodeID Deserialize(PersistentState persistNodeID) => (INodeID) null;

  /// <summary>
  /// Вернуть описание корневого узла на основании данных дескриптора
  /// </summary>
  /// <returns>Описание коревого узла на основании данных дескриптора</returns>
  public override INodeID GetRecordNodeID()
  {
    return (INodeID) new RoutesNodeID((CreateObjectNodeParams) new CreateRoutesNodeParams(this.ObjType, this.ObjID, this._ID, this.CheckedOutBy, 0L, 0, this.Caption, this.RelationTypeID, 0L, 0L, ObjectFiltrationState.fsNotRequired, this.Version, this.BaseVersion, this.SiteID, 0L, this.FiltrationOwnerID, this.Contexts, this.ObjType, this.ObjID, this.Attributes, this.Values, 0L, string.Empty), this._services);
  }

  /// <summary>Вернуть дочерний узел по его описанию</summary>
  /// <param name="nodeID">Описание дочернего узла</param>
  /// <returns>Новый дочерний узел по его описанию</returns>
  public override INode GetChild(INodeID nodeID)
  {
    return !(nodeID is RoutesNodeID routesNodeId) ? base.GetChild(nodeID) : (INode) new RoutesNode(routesNodeId.Services, routesNodeId.FiltrationOwnerID, routesNodeId.Contexts, routesNodeId.ProjObjType, routesNodeId.ProjID, routesNodeId.ObjectID, routesNodeId.ObjectTypeID, routesNodeId.RelationTypeID, routesNodeId.PrjLinkID, routesNodeId.LCStepID, routesNodeId.Caption, routesNodeId.EntersInArticle, routesNodeId.IsDefaultRoute, routesNodeId.CheckedOutBy, routesNodeId.Owner, routesNodeId.Sorting, routesNodeId.Version, routesNodeId.BaseVersion, routesNodeId.Attributes, routesNodeId.Values);
  }

  /// <summary>Вернуть данные по описанию узла</summary>
  /// <param name="nodeID">Описание узла</param>
  /// <param name="dataFormat">Тип запрашиваемых данных</param>
  /// <returns>Запрошенные данные или null</returns>
  public override object GetData(INodeID nodeID, Type dataFormat)
  {
    if (dataFormat == typeof (IDescriptor))
      return (object) new RoutesDescriptor(this._categoryID, this._typeID, this._services, this.FiltrationOwnerID, this.Contexts, this.ObjID, this.ObjType, this.RelationTypeID, this.Caption, this.CheckedOutBy, this.Version, this.BaseVersion, this.Attributes);
    if (dataFormat == typeof (ICanOpenInNewWindow))
      return (object) new CanOpenInNewWindow();
    if (nodeID is RoutesNodeID routesNodeId)
    {
      if (dataFormat == typeof (IDBTypedObjectID))
        return (object) new DBTypedObjectID(routesNodeId.ObjectTypeID, routesNodeId.ObjectID, routesNodeId.ID, routesNodeId.Caption, routesNodeId.Owner, routesNodeId.Version, routesNodeId.BaseVersion, routesNodeId.SiteID, routesNodeId.ModificationID);
      if (dataFormat == typeof (IDBObjectID))
        return (object) new DBObjectID(routesNodeId.ObjectID, routesNodeId.ID, routesNodeId.Caption, routesNodeId.Owner);
      if (dataFormat == typeof (IDBRelationID))
        return (object) new DBRelationID(routesNodeId.PrjLinkID, routesNodeId.ObjectID, routesNodeId.RelationTypeID, routesNodeId.Sorting, routesNodeId.RelGuid, routesNodeId.ProjID);
      if (dataFormat == typeof (IDBObjectTypeID))
        return (object) new DBObjectTypeID(routesNodeId.ObjectTypeID);
      if (dataFormat == typeof (IDBCheckedOutByID))
        return (object) new DBCheckedOutByID(routesNodeId.ObjectID, routesNodeId.CheckedOutBy, routesNodeId.Owner);
    }
    return base.GetData(nodeID, dataFormat);
  }
}
