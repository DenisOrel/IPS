// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.PreciseProducts.PreciseProductsClientService
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Search.Utilities;
using System;

#nullable disable
namespace Intermech.Search.Pdm.PreciseProducts;

public sealed class PreciseProductsClientService : IPreciseProductsClientService
{
  public void CreatePreciseProduct(long relationID, long productVersionID)
  {
    if (RelationHelper.IsUnknownRelationID(relationID))
      throw new ArgumentException();
    if (ObjectHelper.IsUnknownObjectVersionID(productVersionID))
      throw new ArgumentException();
    using (CreatePreciseProductForm preciseProductForm = new CreatePreciseProductForm())
    {
      preciseProductForm.CompositionPartID = new Tuple<long, long>(relationID, productVersionID);
      int num = (int) preciseProductForm.ShowDialog();
    }
  }
}
