// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.ProductionCopyQuery
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.MRP2;

internal class ProductionCopyQuery : INodeQuery
{
  private readonly ProductionCopyNodePart productionCopyNodePart;

  public ProductionCopyQuery(ProductionCopyNodePart productionCopyNodePart)
  {
    this.productionCopyNodePart = productionCopyNodePart;
  }

  public object Bookmark => throw new NotImplementedException();

  public int RecordCount => throw new NotImplementedException();

  public NodeQueryOptions Options
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public long TotalRecordCount => throw new NotImplementedException();

  public void AddColumn(NodeColumn column, INodeColumnTransform transform)
  {
    throw new NotImplementedException();
  }

  public void Execute(object bookmark, int count) => throw new NotImplementedException();

  public void Execute(NodeIDCollection nodeIDs) => throw new NotImplementedException();

  public object[] GetRawRecordValues(int index) => throw new NotImplementedException();

  public INodeID GetRecordNodeID(int index) => throw new NotImplementedException();

  public object[] GetRecordValues(int index) => throw new NotImplementedException();
}
