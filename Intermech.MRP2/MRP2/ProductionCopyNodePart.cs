// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.ProductionCopyNodePart
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Interfaces.Client;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using Intermech.Navigator.Persistence;
using Intermech.Navigator.Queries;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MRP2;

internal class ProductionCopyNodePart : INodeItems, INodePart, INodeQuerySupport
{
  private readonly long _objectID;

  public ProductionCopyNodePart(long objectID) => this._objectID = objectID;

  public object Owner
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public INodeID CreateNodeId(object[] fieldValues, RecordAdapter adapter)
  {
    throw new NotImplementedException();
  }

  public object CreateRecordId(INodeID nodeId) => throw new NotImplementedException();

  public INodeID Deserialize(PersistentState persistNodeID) => (INodeID) null;

  public string GetAddress(INodeID nodeID) => (string) null;

  public IUpdateAnalyser GetAnalyser(
    NodeViewCapabilities capabilities,
    object sender,
    NotificationEventArgs e)
  {
    return (IUpdateAnalyser) null;
  }

  public ContentAttributes GetAttributesOf(INodeID nodeID) => ContentAttributes.None;

  public INode GetChild(INodeID nodeID) => (INode) null;

  public object GetData(INodeID nodeID, Type dataFormat)
  {
    if ((dataFormat == typeof (IDescriptor) || dataFormat == typeof (ICanOpenInNewWindow)) && !Intermech.Consts.IsUndefinedObjectId(this._objectID))
    {
      if (dataFormat == typeof (IDescriptor))
        return (object) new Descriptor(this._objectID);
      if (dataFormat == typeof (ICanOpenInNewWindow))
        return (object) new CanOpenInNewWindow();
    }
    return (object) null;
  }

  public object[] GetData(NodeIDCollection nodeIDs, Type dataFormat) => (object[]) null;

  public NodeColumnCollection GetDefaultColumns() => throw new NotImplementedException();

  public INodeQuery GetQuery() => (INodeQuery) new ProductionCopyQuery(this);

  public object GetService(Type service) => throw new NotImplementedException();

  public List<object> GetSpecialFields() => throw new NotImplementedException();

  public NodeColumnCollection GetSupportedColumns(string ColumnSetName)
  {
    throw new NotImplementedException();
  }

  public List<string> GetSupportedColumnSetNames() => throw new NotImplementedException();

  public object MapColumnToField(NodeColumn column) => throw new NotImplementedException();

  public INodeID ParseAddress(string address) => (INodeID) null;

  public PersistentState Serialize(INodeID nodeID) => (PersistentState) null;
}
