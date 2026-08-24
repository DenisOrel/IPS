// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.ManufacturingOrderBlankDescriptor
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Search.Utilities;
using System;

#nullable disable
namespace Intermech.MRP.Orders;

public sealed class ManufacturingOrderBlankDescriptor : Descriptor
{
  private long _orderVersionID;

  public ManufacturingOrderBlankDescriptor(long orderVersionID)
    : base(orderVersionID)
  {
    this._orderVersionID = !ObjectHelper.IsUnknownObjectVersionID(orderVersionID) ? orderVersionID : throw new ArgumentException();
  }

  public override INode GetChild(INodeID nodeID)
  {
    return (INode) new ManufacturingOrderBlankNode(this._orderVersionID);
  }
}
