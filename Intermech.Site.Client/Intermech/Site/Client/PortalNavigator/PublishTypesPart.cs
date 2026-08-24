// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PublishTypesPart
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.DataFormats;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using Intermech.Navigator.Persistence;
using Intermech.Navigator.Queries;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class PublishTypesPart : IContextAware, INodeItems, INodePart, INodeQuerySupport
{
  protected IServiceProvider services;
  protected object owner;
  private int _typeID;

  public PublishTypesPart(IServiceProvider services, int typeID)
  {
    this.services = services;
    this._typeID = typeID;
  }

  public IServiceProvider Services
  {
    [DebuggerStepThrough] get => this.services;
    [DebuggerStepThrough] set => this.services = value;
  }

  public ContentAttributes GetAttributesOf(INodeID nodeID) => ContentAttributes.Folder;

  public INode GetChild(INodeID nodeID)
  {
    return !(nodeID is PublishTypeNodeID publishTypeNodeId) ? (INode) null : (INode) new PublishTypeNode(publishTypeNodeId.id);
  }

  public string GetAddress(INodeID nodeID)
  {
    return nodeID is PublishTypeNodeID publishTypeNodeId ? publishTypeNodeId.caption : string.Empty;
  }

  public INodeID ParseAddress(string address)
  {
    IPortalMetadata service = (IPortalMetadata) ServicesManager.GetService(typeof (IPortalMetadata));
    if (service != null)
    {
      int publishObjectTypeId = service.GetPublishObjectTypeID(address);
      if (publishObjectTypeId != -1)
        return (INodeID) new PublishTypeNodeID(publishObjectTypeId, address);
    }
    return (INodeID) null;
  }

  public PersistentState Serialize(INodeID nodeID)
  {
    if (!(nodeID is PublishTypeNodeID publishTypeNodeId))
      return (PersistentState) null;
    PersistentState persistentState = new PersistentState();
    persistentState.AddValue("id", (object) publishTypeNodeId.id);
    return persistentState;
  }

  public INodeID Deserialize(PersistentState persistNodeID)
  {
    if (persistNodeID == null)
      return (INodeID) null;
    object obj = persistNodeID.GetValue("id");
    if (obj == null || !(obj is int))
      return (INodeID) null;
    string name = string.Empty;
    IPortalMetadata service = (IPortalMetadata) ServicesManager.GetService(typeof (IPortalMetadata));
    if (service != null)
      name = service.GetPublishObjectTypeName((int) obj);
    return (INodeID) new PublishTypeNodeID((int) obj, name);
  }

  public object GetData(INodeID nodeID, Type dataFormat)
  {
    return nodeID is PublishTypeNodeID publishTypeNodeId && dataFormat == typeof (IDBObjectTypeID) ? (object) new DBObjectTypeID(publishTypeNodeId.id) : (object) null;
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

  public object Owner
  {
    [DebuggerStepThrough] get => this.owner;
    [DebuggerStepThrough] set => this.owner = value;
  }

  public INodeQuery GetQuery()
  {
    if (this.Owner is IContextAware owner)
    {
      IServiceProvider services = owner.Services;
    }
    return (INodeQuery) new PortalTypesQuery((INodeQuerySupport) this, this._typeID);
  }

  public NodeColumnCollection GetDefaultColumns()
  {
    return new NodeColumnCollection()
    {
      (ServicesManager.GetService(typeof (IColumnSchemes)) as IColumnSchemes).CreateColumn(Intermech.Navigator.Consts.NameColumnSchemeGuid, (object) "F_CAPTION")
    };
  }

  public NodeColumnCollection GetSupportedColumns(string ColumnSetName) => this.GetDefaultColumns();

  public List<string> GetSupportedColumnSetNames() => (List<string>) null;

  public object MapColumnToField(NodeColumn column)
  {
    return column.SchemeGuid == Intermech.Navigator.Consts.NavigatorColumnSchemeGuid && (column.ID.Equals((object) "F_CAPTION") || column.ID.Equals((object) "F_OBJECT_TYPE")) || column.SchemeGuid == Intermech.Navigator.Consts.NameColumnSchemeGuid ? column.ID : (object) null;
  }

  public List<object> GetSpecialFields()
  {
    return new List<object>() { (object) "F_OBJECT_TYPE" };
  }

  public INodeID CreateNodeId(object[] fieldValues, RecordAdapter adapter)
  {
    PortalObjectType fieldValue = fieldValues[1] as PortalObjectType;
    return (INodeID) new PublishTypeNodeID(fieldValue.ID, fieldValue.Name);
  }

  public object CreateRecordId(INodeID nodeId) => (object) (nodeId as PublishTypeNodeID);
}
