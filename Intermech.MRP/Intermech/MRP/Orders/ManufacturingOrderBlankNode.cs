// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.ManufacturingOrderBlankNode
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.Interfaces;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Parts;
using Intermech.Search.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.MRP.Orders;

public sealed class ManufacturingOrderBlankNode : ObjectNode
{
  private long _orderVersionID;

  public ManufacturingOrderBlankNode(long orderVersionID)
    : base(ManufacturingOrderBlankNode.GetObjectTypeID(orderVersionID), orderVersionID)
  {
    this._orderVersionID = !ObjectHelper.IsUnknownObjectVersionID(orderVersionID) ? orderVersionID : throw new ArgumentException();
  }

  protected override List<PartSlot> CreateFolderSlots()
  {
    List<PartSlot> folderSlots = new List<PartSlot>();
    foreach (int relTypeID in MetaDataHelper.GetObjectTypeApplicabilities(this._objTypeID).Select<IMSApplicability, int>((Func<IMSApplicability, int>) (o => o.RelationTypeID)).Distinct<int>().ToArray<int>())
      folderSlots.Add(new PartSlot(MetaDataHelper.GetRelationTypeGuid(relTypeID), (INodePart) new RelatedObjectsPart(this._objTypeID, this._orderVersionID, RelatedObjectsRole.Composition, relTypeID, this.Services)));
    return folderSlots;
  }

  protected override List<PartSlot> CreateNonFolderSlots() => new List<PartSlot>(0);

  private static int GetObjectTypeID(long objectVersionID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      return sessionKeeper.Session.GetObject(objectVersionID).ObjectType;
  }
}
