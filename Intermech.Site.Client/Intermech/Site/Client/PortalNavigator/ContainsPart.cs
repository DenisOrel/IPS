// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.ContainsPart
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel.Search;
using Intermech.Navigator.DB;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using Intermech.Navigator.Persistence;
using Intermech.Navigator.Queries;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class ContainsPart : IContextAware, INodePart, INodeItems, INodeQuerySupport
{
  protected int objTypeID;
  private IServiceProvider _services;
  private IConditionsProvider _conditionsProvider;
  private ConditionStructure[] _conditionsCache;
  protected object owner;

  protected ConditionStructure[] Conditions
  {
    get
    {
      if (this._conditionsProvider != null && this._conditionsProvider.ConditionsChanged)
        this._conditionsCache = (ConditionStructure[]) null;
      if (this._conditionsCache == null && this._conditionsProvider != null)
        this._conditionsCache = this._conditionsProvider.GetConditions();
      return this._conditionsCache;
    }
  }

  public ContainsPart(IServiceProvider services, int objTypeID)
  {
    this.objTypeID = objTypeID;
    this._services = services;
  }

  public ContainsPart(
    IServiceProvider services,
    IConditionsProvider conditionsProvider,
    int objTypeID)
    : this(services, objTypeID)
  {
    this._conditionsProvider = conditionsProvider;
    this._conditionsCache = (ConditionStructure[]) null;
  }

  public virtual IServiceProvider Services
  {
    [DebuggerStepThrough] get => this._services;
    set => this._services = value;
  }

  public object Owner
  {
    [DebuggerStepThrough] get => this.owner;
    [DebuggerStepThrough] set => this.owner = value;
  }

  public virtual INodeQuery GetQuery()
  {
    return (INodeQuery) new ContainsQuery((INodeQuerySupport) this, this.objTypeID, this.Conditions, this._services);
  }

  public virtual NodeColumnCollection GetDefaultColumns() => Helper.GetPublishedObjectColumns();

  public virtual NodeColumnCollection GetSupportedColumns(string ColumnSetName)
  {
    NodeColumnCollection defaultColumns = this.GetDefaultColumns();
    IColumnSchemes service1 = (IColumnSchemes) ServicesManager.GetService(typeof (IColumnSchemes));
    NodeColumn column1 = service1.CreateColumn(SiteClientConsts.PublishObjectObligatoryColumnSchemeGuid, (object) ObligatoryObjectAttributes.F_OBJ_CREATE);
    defaultColumns.Add(column1);
    NodeColumn column2 = service1.CreateColumn(SiteClientConsts.PublishObjectObligatoryColumnSchemeGuid, (object) new Guid("cad0013a-306c-11d8-b4e9-00304f19f545"));
    defaultColumns.Add(column2);
    IPortalMetadata service2 = (IPortalMetadata) ServicesManager.GetService(typeof (IPortalMetadata));
    if (service2 != null)
    {
      PortalObjectType publishObjectType = service2.GetPublishObjectType(this.objTypeID);
      if (publishObjectType.Attributes != null && publishObjectType.Attributes.Length != 0)
      {
        for (int index = 0; index < publishObjectType.Attributes.Length; ++index)
        {
          PortalAttributeType attribute = publishObjectType.Attributes[index];
          if (attribute.Type != FieldTypes.ftBlob && attribute.Type != FieldTypes.ftMemo && attribute.Type != FieldTypes.ftShortBlob && attribute.Type != FieldTypes.ftFile)
          {
            NodeColumn column3 = service1.CreateColumn(SiteClientConsts.PublishObjectTypeColumnSchemeGuid, (object) attribute);
            defaultColumns.Add(column3);
          }
        }
      }
    }
    return defaultColumns;
  }

  public virtual object MapColumnToField(NodeColumn column)
  {
    if (column.SchemeGuid == Intermech.Navigator.Consts.NavigatorColumnSchemeGuid && column.ID.Equals((object) "F_CAPTION"))
      return (object) new NodeColumnID((object) ObligatoryObjectAttributes.CAPTION, AttributeSourceTypes.Object);
    if (column.SchemeGuid == Intermech.Navigator.Consts.ObjectColumnSchemeGuid || column.SchemeGuid == Intermech.Navigator.Consts.CurrentObjectColumnSchemeGuid || column.SchemeGuid == Intermech.Navigator.Consts.ObjectObligatoryColumnSchemeGuid || column.SchemeGuid == SiteClientConsts.PublishObjectObligatoryColumnSchemeGuid)
      return (object) new NodeColumnID(column.ID, AttributeSourceTypes.Object);
    return column.SchemeGuid == SiteClientConsts.PublishObjectTypeColumnSchemeGuid ? (object) new NodeColumnID((object) ((PortalAttributeType) column.ID).ID, AttributeSourceTypes.Object) : (object) null;
  }

  public virtual List<object> GetSpecialFields()
  {
    return new List<object>()
    {
      (object) new NodeColumnID((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object),
      (object) new NodeColumnID((object) ObligatoryObjectAttributes.F_OBJECT_TYPE, AttributeSourceTypes.Object),
      (object) new NodeColumnID((object) PortalConsts.attributePublishObjectGUID, AttributeSourceTypes.Object),
      (object) new NodeColumnID((object) PortalConsts.attributeOwner, AttributeSourceTypes.Object),
      (object) new NodeColumnID((object) PortalConsts.attributeCopyKeepers, AttributeSourceTypes.Object)
    };
  }

  public virtual INodeID CreateNodeId(object[] fieldValues, RecordAdapter adapter)
  {
    long int64 = Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) new NodeColumnID((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object))]);
    int int32 = Convert.ToInt32(fieldValues[adapter.GetFieldIndex((object) new NodeColumnID((object) ObligatoryObjectAttributes.F_OBJECT_TYPE, AttributeSourceTypes.Object))]);
    string str1 = Convert.ToString(fieldValues[adapter.GetFieldIndex((object) new NodeColumnID((object) PortalConsts.attributeOwner, AttributeSourceTypes.Object))]);
    Guid guid = new Guid(Convert.ToString(fieldValues[adapter.GetFieldIndex((object) new NodeColumnID((object) PortalConsts.attributePublishObjectGUID, AttributeSourceTypes.Object))]));
    long num = 0;
    if (str1 != string.Empty)
    {
      SiteInfo site = ((ISitesCacheService) (ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService).GetCustomService(typeof (ISitesCacheService))).GetSite(str1[0]);
      num = site != null ? site.ID : 0L;
    }
    int fieldIndex = adapter.GetFieldIndex((object) new NodeColumnID((object) ObligatoryObjectAttributes.CAPTION, AttributeSourceTypes.Object));
    string str2 = fieldIndex >= 0 ? Convert.ToString(fieldValues[fieldIndex]) : string.Empty;
    string str3 = Convert.ToString(fieldValues[adapter.GetFieldIndex((object) new NodeColumnID((object) PortalConsts.attributeCopyKeepers, AttributeSourceTypes.Object))]);
    long objectID = int64;
    Guid objectGuid = guid;
    string copyKeepers = str3;
    long ownerID = num;
    string name = str2;
    return (INodeID) new PublishedObjectNodeID(int32, objectID, objectGuid, copyKeepers, ownerID, name);
  }

  public virtual object CreateRecordId(INodeID nodeId) => (object) (nodeId as PublishTypeNodeID);

  public List<string> GetSupportedColumnSetNames() => (List<string>) null;

  public virtual ContentAttributes GetAttributesOf(INodeID nodeID)
  {
    return ContentAttributes.HasChildren | ContentAttributes.Slow | ContentAttributes.Large;
  }

  public virtual INode GetChild(INodeID nodeID)
  {
    return nodeID is PublishedObjectNodeID publishedObjectNodeId ? (INode) new PublishedObjectNode(publishedObjectNodeId.TypeID, publishedObjectNodeId.ObjectID) : (INode) null;
  }

  public string GetAddress(INodeID nodeID)
  {
    return !(nodeID is PublishedObjectNodeID publishedObjectNodeId) ? Intermech.Navigator.DBObjects.Helper.GetAddress(nodeID) : publishedObjectNodeId.Caption;
  }

  public INodeID ParseAddress(string address) => (INodeID) null;

  public PersistentState Serialize(INodeID nodeID)
  {
    if (!(nodeID is PublishedObjectNodeID publishedObjectNodeId))
      return (PersistentState) null;
    PersistentState persistentState = new PersistentState();
    persistentState.AddValue("PropTypeID", (object) publishedObjectNodeId.TypeID);
    persistentState.AddValue("PropObjectID", (object) publishedObjectNodeId.ObjectID);
    persistentState.AddValue("PropObjectGuid", (object) publishedObjectNodeId.ObjectGuid);
    persistentState.AddValue("PropCaption", (object) publishedObjectNodeId.Caption);
    persistentState.AddValue("PropOwnerID", (object) publishedObjectNodeId.OwnerID);
    persistentState.AddValue("PropCopyKeepers", (object) publishedObjectNodeId.CopyKeepers);
    return persistentState;
  }

  public INodeID Deserialize(PersistentState persistNodeID)
  {
    int objectType = (int) persistNodeID.GetValue("PropTypeID");
    long num1 = (long) persistNodeID.GetValue("PropObjectID");
    string str1 = (string) persistNodeID.GetValue("PropCaption");
    long num2 = (long) persistNodeID.GetValue("PropOwnerID");
    Guid guid = (Guid) persistNodeID.GetValue("PropObjectGuid");
    string str2 = (string) persistNodeID.GetValue("PropCopyKeepers");
    long objectID = num1;
    Guid objectGuid = guid;
    string copyKeepers = str2;
    long ownerID = num2;
    string name = str1;
    return (INodeID) new PublishedObjectNodeID(objectType, objectID, objectGuid, copyKeepers, ownerID, name);
  }

  public virtual object GetData(INodeID nodeID, Type dataFormat)
  {
    PublishedObjectNodeID data = nodeID as PublishedObjectNodeID;
    if (dataFormat == typeof (IPublishObjectID) && data != null)
      return (object) data;
    if (dataFormat == typeof (IPublishTypedID) && data != null)
      return (object) data;
    if (dataFormat == typeof (INode))
      return (object) this.GetChild(nodeID);
    if (dataFormat == typeof (IDescriptor))
      return (object) new PublishedObjectDescriptor(data.ObjectID, data.ObjectGuid, data.TypeID, data.CopyKeepers, data.OwnerID, data.Caption);
    return dataFormat == typeof (ICanOpenInNewWindow) ? (object) new CanOpenInNewWindow() : (object) null;
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
}
