// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.SiteUsersPart
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using Intermech.Navigator.Persistence;
using Intermech.Navigator.Queries;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class SiteUsersPart : IContextAware, INodePart, INodeItems, INodeQuerySupport
{
  private IServiceProvider _services;
  private object _owner;
  private Guid _siteGuid;

  public SiteUsersPart(IServiceProvider services, Guid siteGuid)
  {
    this._siteGuid = siteGuid;
    this._services = services;
  }

  public virtual IServiceProvider Services
  {
    [DebuggerStepThrough] get => this._services;
    set => this._services = value;
  }

  public object Owner
  {
    [DebuggerStepThrough] get => this._owner;
    [DebuggerStepThrough] set => this._owner = value;
  }

  public INodeQuery GetQuery()
  {
    return (INodeQuery) new SiteUsersQuery((INodeQuerySupport) this, this._siteGuid, this._services);
  }

  public NodeColumnCollection GetDefaultColumns() => Helper.GetPublicUserColumns();

  public NodeColumnCollection GetSupportedColumns(string ColumnSetName)
  {
    NodeColumnCollection publicUserColumns = Helper.GetPublicUserColumns();
    publicUserColumns.Add((ServicesManager.GetService(typeof (IColumnSchemes)) as IColumnSchemes).CreateColumn(SiteClientConsts.PublishUserObligatoryColumnSchemeGuid, (object) ObligatoryObjectAttributes.F_OBJ_GUID));
    return publicUserColumns;
  }

  public List<string> GetSupportedColumnSetNames() => (List<string>) null;

  public ContentAttributes GetAttributesOf(INodeID nodeID) => ContentAttributes.Folder;

  public INode GetChild(INodeID nodeID)
  {
    return !(nodeID is UserNodeID userNodeId) ? (INode) null : (INode) new UserNode(userNodeId.UserID);
  }

  public string GetAddress(INodeID nodeID) => string.Empty;

  public INodeID ParseAddress(string address) => (INodeID) null;

  public PersistentState Serialize(INodeID nodeID)
  {
    if (!(nodeID is UserNodeID userNodeId))
      return (PersistentState) null;
    PersistentState persistentState = new PersistentState();
    persistentState.AddValue("id", (object) userNodeId.UserID);
    persistentState.AddValue("name", (object) userNodeId.UserName);
    return persistentState;
  }

  public INodeID Deserialize(PersistentState persistNodeID)
  {
    if (persistNodeID == null)
      return (INodeID) null;
    object obj = persistNodeID.GetValue("id");
    return obj != null && obj is long userId ? (INodeID) new UserNodeID(userId, (string) persistNodeID.GetValue("name"), this._siteGuid) : (INodeID) null;
  }

  public object GetData(INodeID nodeID, Type dataFormat)
  {
    return nodeID is UserNodeID userNodeId && dataFormat == typeof (IUserNodeID) ? (object) userNodeId : (object) null;
  }

  public object[] GetData(NodeIDCollection nodeIDs, Type dataFormat)
  {
    object[] data = new object[nodeIDs.Count];
    for (int index = 0; index < data.Length; ++index)
      data[index] = this.GetData(nodeIDs[index], dataFormat);
    return data;
  }

  public IUpdateAnalyser GetAnalyser(
    NodeViewCapabilities capabilities,
    object sender,
    NotificationEventArgs e)
  {
    return (IUpdateAnalyser) null;
  }

  public object GetService(Type service) => (object) null;

  public virtual object MapColumnToField(NodeColumn column)
  {
    if (column.SchemeGuid == Intermech.Navigator.Consts.ObjectObligatoryColumnSchemeGuid)
      return (object) new NodeColumnID(column.ID, AttributeSourceTypes.Object);
    return column.SchemeGuid == SiteClientConsts.PublishUserObligatoryColumnSchemeGuid ? (object) new NodeColumnID(column.ID, AttributeSourceTypes.Object) : (object) null;
  }

  public List<object> GetSpecialFields()
  {
    return new List<object>()
    {
      (object) new NodeColumnID((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object),
      (object) new NodeColumnID((object) new Guid("cad0001d-306c-11d8-b4e9-00304f19f545"), AttributeSourceTypes.Object)
    };
  }

  public INodeID CreateNodeId(object[] fieldValues, RecordAdapter adapter)
  {
    return (INodeID) new UserNodeID(Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) new NodeColumnID((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object))]), Convert.ToString(fieldValues[adapter.GetFieldIndex((object) new NodeColumnID((object) new Guid("cad0001d-306c-11d8-b4e9-00304f19f545"), AttributeSourceTypes.Object))]), this._siteGuid);
  }

  public object CreateRecordId(INodeID nodeId) => (object) (nodeId as UserNodeID);
}
