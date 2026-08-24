// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.ProductionListReportService
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Interfaces.MRP;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MRP2;

internal class ProductionListReportService : IProductionListReportService
{
  private Dictionary<long, object> dataSources;

  public ProductionListReportService() => this.dataSources = new Dictionary<long, object>();

  public void AddReportDataSource(long objectID, object dataSource)
  {
    if (this.dataSources.ContainsKey(objectID))
      this.dataSources.Remove(objectID);
    this.dataSources.Add(objectID, dataSource);
  }

  public object GetReportDataSource(long objectID)
  {
    object obj;
    return this.dataSources.TryGetValue(objectID, out obj) ? obj : (object) null;
  }
}
