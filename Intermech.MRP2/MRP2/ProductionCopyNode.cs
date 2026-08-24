// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.ProductionCopyNode
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Navigator.Parts;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MRP2;

/// <summary>
/// не используется (пока?)  - не осилил логику, пока сделал по тупому без этих классов
/// </summary>
internal class ProductionCopyNode : CompositeNode
{
  private readonly long _objectID;

  public ProductionCopyNode(long objectID) => this._objectID = objectID;

  protected override List<PartSlot> CreateNonFolderSlots()
  {
    return this.SlotsFromSinglePart((INodePart) new ProductionCopyNodePart(this._objectID));
  }
}
