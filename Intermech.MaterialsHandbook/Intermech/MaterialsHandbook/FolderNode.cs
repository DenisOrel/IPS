// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.FolderNode
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class FolderNode : Intermech.Imbase.FolderNode
{
  private int _parentVirtualCategoryID;
  private Dictionary<long, INode> _nodes = new Dictionary<long, INode>();

  public long FolderID => this._objID;

  public List<object> Params { get; set; }

  public long SelectedAssortmentRecID { get; set; }

  public long SelectedAssortmentTableRefID { get; set; }

  public long SelectedMaterialRecID { get; set; }

  public long SelectedMaterialTableRefID { get; set; }

  public List<long> TableRefIDs { get; set; }

  public FolderNode(int parentVirtualCategoryID, long objID)
    : base(Intermech.Imbase.Consts.ImbaseFolderTypeID, objID)
  {
    this._parentVirtualCategoryID = parentVirtualCategoryID;
    this.SelectedAssortmentTableRefID = this.SelectedMaterialTableRefID = 0L;
    this.SelectedAssortmentRecID = this.SelectedMaterialRecID = -1L;
  }

  protected override List<PartSlot> CreateFolderSlots()
  {
    List<PartSlot> folderSlots = new List<PartSlot>(1);
    if (this.FolderID != 0L)
    {
      ICurrentUserAndRole service = ServiceUtils.GetService<ICurrentUserAndRole>((object) ApplicationServices.Container, false);
      List<Guid> guidList1 = (List<Guid>) null;
      if (service != null)
        guidList1 = service.Rule.GetObjectTypeVisibleRelationsGuids(Intermech.Imbase.Consts.ImbaseFolderTypeID, true);
      List<Guid> guidList2 = guidList1 ?? new List<Guid>(1);
      if (guidList2.Count == 0)
      {
        Guid relationTypeGuid = MetaDataHelper.GetDefaultRelationTypeGuid(Intermech.Imbase.Consts.ImbaseFolderTypeID);
        if (relationTypeGuid != Guid.Empty)
          guidList2.Add(relationTypeGuid);
      }
      if (guidList2.Count > 0)
      {
        foreach (Guid relTypeGuid in guidList2)
        {
          FolderObjectPart part = new FolderObjectPart(this, this._parentVirtualCategoryID, this.FolderID, RelatedObjectsRole.Composition, MetaDataHelper.GetRelationTypeID(relTypeGuid), this.Services);
          folderSlots.Add(new PartSlot(Intermech.Imbase.Consts.ImbaseFolderTypeGUID, (INodePart) part));
        }
      }
    }
    return folderSlots;
  }

  public override INode GetChild(INodeID nodeID)
  {
    INode node = (INode) null;
    if (nodeID is NodeID nodeId && nodeID.TypeID == Intermech.Imbase.Consts.ImbaseFolderTypeID)
    {
      if (this._nodes.ContainsKey(nodeId.ObjectID))
      {
        node = this._nodes[nodeId.ObjectID];
      }
      else
      {
        FolderNode folderNode = new FolderNode(this._parentVirtualCategoryID, nodeId.ObjectID);
        this._nodes[nodeId.ObjectID] = (INode) folderNode;
        node = (INode) folderNode;
      }
    }
    return node ?? base.GetChild(nodeID);
  }

  public override object GetData(INodeID nodeID, Type dataFormat)
  {
    object obj = (object) null;
    if (nodeID != null)
    {
      if (dataFormat == typeof (IIMHNode))
      {
        if (nodeID is NodeID nodeId1)
        {
          List<long> tableRefIds = this._nodes != null ? (this._nodes.ContainsKey(nodeId1.ObjectID) ? (this._nodes[nodeId1.ObjectID] is FolderNode node ? node.TableRefIDs : (List<long>) null) : (List<long>) null) : (List<long>) null;
          obj = (object) new IMHNode(this._parentVirtualCategoryID, Intermech.Imbase.Consts.ImbaseFolderTypeID, tableRefIds);
        }
      }
      else if (dataFormat == typeof (ICanOpenInNewWindow) && nodeID is NodeID nodeId2 && nodeId2.ObjectTypeID == Intermech.Imbase.Consts.ImbaseFolderTypeID)
        return (object) null;
      if (dataFormat == typeof (IDescriptor) && nodeID is NodeID)
        obj = (object) new FolderNodeDescriptor(this._parentVirtualCategoryID, ((NodeID) nodeID).ObjectID);
      else if (dataFormat == typeof (FolderNode) && nodeID is NodeID)
      {
        long objectId = ((NodeID) nodeID).ObjectID;
        obj = this._nodes == null || !this._nodes.ContainsKey(objectId) ? (object) new FolderNode(this._parentVirtualCategoryID, ((NodeID) nodeID).ObjectID) : (object) this._nodes[objectId];
      }
    }
    return obj ?? base.GetData(nodeID, dataFormat);
  }
}
