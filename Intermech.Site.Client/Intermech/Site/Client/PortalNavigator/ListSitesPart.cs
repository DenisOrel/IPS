// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.ListSitesPart
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
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

internal sealed class ListSitesPart : INodeItems, IContextAware, INodePart, INodeQuerySupport
{
  private readonly int _sitesObjectTypeId;

  public ListSitesPart(int sitesObjectTypeId, IServiceProvider services)
  {
    this._sitesObjectTypeId = sitesObjectTypeId;
    this.Services = services;
  }

  public object MapColumnToField(NodeColumn column)
  {
    return column.SchemeGuid == Intermech.Navigator.Consts.NavigatorColumnSchemeGuid && (column.ID.Equals((object) "F_CAPTION") || column.ID.Equals((object) "F_OBJECT_ID")) || column.SchemeGuid == Intermech.Navigator.Consts.NameColumnSchemeGuid ? column.ID : (object) null;
  }

  public List<object> GetSpecialFields()
  {
    return new List<object>() { (object) "F_OBJECT_ID" };
  }

  public INodeID CreateNodeId(object[] fieldValues, RecordAdapter adapter)
  {
    if (fieldValues == null || fieldValues.Length != 2)
      return (INodeID) null;
    return !(fieldValues[1] is SiteInfo fieldValue) ? (INodeID) null : (INodeID) new SiteNodeID(fieldValue.ID, this._sitesObjectTypeId, fieldValue.GUID, fieldValue.Code, fieldValue.Caption);
  }

  public object CreateRecordId(INodeID nodeId) => (object) (nodeId as SiteNodeID);

  public object Owner { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

  public INodeQuery GetQuery()
  {
    return (INodeQuery) new ListSitesQuery((INodeQuerySupport) this, this._sitesObjectTypeId, this.Owner is IContextAware owner ? owner.Services : (IServiceProvider) null);
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

  public ContentAttributes GetAttributesOf(INodeID nodeID) => ContentAttributes.Folder;

  public INode GetChild(INodeID nodeID)
  {
    return !(nodeID is SiteNodeID siteNodeId) ? (INode) null : (INode) new SiteNode(siteNodeId.Guid);
  }

  public string GetAddress(INodeID nodeID)
  {
    return nodeID is SiteNodeID siteNodeId ? siteNodeId.Name : string.Empty;
  }

  public INodeID ParseAddress(string address)
  {
    SiteInfo site = ((ISitesCacheService) (ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService).GetCustomService(typeof (ISitesCacheService))).GetSite(address);
    return site != null ? (INodeID) new SiteNodeID(site.ID, this._sitesObjectTypeId, site.GUID, site.Code, site.Caption) : (INodeID) null;
  }

  public PersistentState Serialize(INodeID nodeID)
  {
    if (!(nodeID is SiteNodeID siteNodeId))
      return (PersistentState) null;
    PersistentState persistentState = new PersistentState();
    persistentState.AddValue("id", (object) siteNodeId.ObjectID);
    return persistentState;
  }

  public INodeID Deserialize(PersistentState persistNodeID)
  {
    if (persistNodeID == null)
      return (INodeID) null;
    object obj = persistNodeID.GetValue("id");
    if (obj != null && obj is long id)
    {
      SiteInfo site = ((ISitesCacheService) (ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService).GetCustomService(typeof (ISitesCacheService))).GetSite(id);
      if (site != null)
        return (INodeID) new SiteNodeID(site.ID, this._sitesObjectTypeId, site.GUID, site.Code, site.Caption);
    }
    return (INodeID) null;
  }

  public object GetData(INodeID nodeID, Type dataFormat) => (object) null;

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

  public IServiceProvider Services { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }
}
