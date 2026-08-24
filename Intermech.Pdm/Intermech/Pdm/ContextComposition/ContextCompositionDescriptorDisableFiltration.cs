// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.ContextComposition.ContextCompositionDescriptorDisableFiltration
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Navigator.Interfaces;

#nullable disable
namespace Intermech.Pdm.ContextComposition;

public class ContextCompositionDescriptorDisableFiltration(long objectVersionID) : Intermech.Navigator.DBObjects.Descriptor(objectVersionID)
{
  public override INode GetChild(INodeID nodeID)
  {
    return (INode) new ObjectContextCompositionNode(this._realObjID, ContextCompositionDescriptorDisableFiltration.GetObjectTypeID(this._realObjID));
  }

  private static int GetObjectTypeID(long objectVersionID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      return sessionKeeper.Session.GetObjectInfo(objectVersionID).ObjectTypeID;
  }
}
