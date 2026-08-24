// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.ContextComposition.ObjectContextCompositionNode
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Parts;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm.ContextComposition;

internal sealed class ObjectContextCompositionNode(long objectVersionID, int objectTypeID) : 
  ObjectNode(objectTypeID, objectVersionID)
{
  protected override List<PartSlot> CreateFolderSlots()
  {
    return new List<PartSlot>()
    {
      new PartSlot(MetaDataHelper.GetObjectTypeGuid(this._objTypeID), (INodePart) new ObjectContextCompositionNodePart(this._objID, this._objTypeID, this.Services))
    };
  }

  protected override List<PartSlot> CreateNonFolderSlots()
  {
    return new List<PartSlot>()
    {
      new PartSlot(MetaDataHelper.GetObjectTypeGuid(this._objTypeID), (INodePart) new ObjectContextCompositionNodePart(this._objID, this._objTypeID, this.Services))
    };
  }
}
