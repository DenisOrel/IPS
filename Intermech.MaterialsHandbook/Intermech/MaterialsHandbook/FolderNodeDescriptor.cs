// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.FolderNodeDescriptor
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class FolderNodeDescriptor : Descriptor
{
  private string _caption = string.Empty;
  private int _parentVirtualCategoryID;
  private Dictionary<long, INode> _nodes = new Dictionary<long, INode>();

  public FolderNodeDescriptor(int parentVirtualCategoryID, long objID)
    : base(objID)
  {
    this._parentVirtualCategoryID = parentVirtualCategoryID;
  }

  public FolderNodeDescriptor(string caption, int parentVirtualCategoryID)
    : base(0L)
  {
    this._caption = caption;
    this._parentVirtualCategoryID = parentVirtualCategoryID;
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
    if (dataFormat == typeof (IIMHNode))
    {
      if (nodeID is NodeID nodeId1 && nodeId1.ObjectTypeID == Intermech.Imbase.Consts.ImbaseFolderTypeID)
      {
        List<long> tableRefIds = this._nodes.ContainsKey(nodeId1.ObjectID) ? (this._nodes[nodeId1.ObjectID] is FolderNode node ? node.TableRefIDs : (List<long>) null) : (List<long>) null;
        obj = (object) new IMHNode(this._parentVirtualCategoryID, nodeId1.ObjectTypeID, tableRefIds);
      }
      else
        obj = (object) new IMHNode(this._parentVirtualCategoryID, nodeID.CategoryID, (List<long>) null);
    }
    else if (dataFormat == typeof (FolderNode) && nodeID is NodeID)
    {
      NodeID nodeId2 = (NodeID) nodeID;
      FolderNode folderNode = new FolderNode(this._parentVirtualCategoryID, nodeId2.ObjectID);
      if (nodeId2.ObjectTypeID == Intermech.Imbase.Consts.ImbaseFolderTypeID)
      {
        folderNode.TableRefIDs = this._nodes.ContainsKey(nodeId2.ObjectID) ? (this._nodes[nodeId2.ObjectID] is FolderNode node ? node.TableRefIDs : (List<long>) null) : (List<long>) null;
        obj = (object) folderNode;
      }
    }
    return obj ?? base.GetData(nodeID, dataFormat);
  }

  public override INodeID GetRecordNodeID()
  {
    INodeID nodeId = (INodeID) null;
    if (this.ObjectID == 0L)
      nodeId = (INodeID) new StandartFolderNodeID(new CreateObjectNodeParams()
      {
        ObjectTypeID = Intermech.Imbase.Consts.ImbaseFolderTypeID,
        Caption = this._caption
      });
    return nodeId ?? base.GetRecordNodeID();
  }
}
