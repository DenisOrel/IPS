// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.PLRelationsNode
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Interfaces;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MRP2;

public class PLRelationsNode(CreateObjectNodeParams e) : AdvRelationsNode(e)
{
  public static bool ShowDocuments = true;

  protected override List<PartSlot> CreateFolderSlots()
  {
    if (Intermech.Consts.IsUndefinedObjectId(this.ObjID))
      return (List<PartSlot>) null;
    List<PartSlot> folderSlots = this.SlotsFromSinglePart((INodePart) new PLRelationsPart(this.ObjType, this.ObjID, this.RelationTypeID, this.FiltrationOwnerID, this.Contexts, this.Attributes, this.Services));
    if (PLRelationsNode.ShowDocuments)
      folderSlots.Add(new PartSlot(new Guid("cad00070-306c-11d8-b4e9-00304f19f545"), (INodePart) new PLRelationsPart(this.ObjType, this.ObjID, MRP2Consts.reltypeIdDocumentation, this.FiltrationOwnerID, this.Contexts, this.Attributes, this.Services)));
    folderSlots.Add(new PartSlot(PDMPluginGuids.linkZagotRelationGuid, (INodePart) new PLRelationsPart(this.ObjType, this.ObjID, PDMPluginIDs.linkZagotRelaionID, this.FiltrationOwnerID, this.Contexts, this.Attributes, this.Services)));
    return folderSlots;
  }
}
