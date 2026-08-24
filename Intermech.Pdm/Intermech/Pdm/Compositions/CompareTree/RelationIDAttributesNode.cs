// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.RelationIDAttributesNode
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class RelationIDAttributesNode
{
  private ICategoryTypeIconService _iconService;

  public RelationIDAttributesNode(ICategoryTypeIconService iconService)
  {
    this._iconService = iconService;
  }

  public bool IsChildNode(TreeNode node)
  {
    return node.Tag != null && node.Tag is Tuple<int, int, List<int>>;
  }

  public void RefreshNode(TreeNode node, List<Tuple<int, int, List<int>>> items)
  {
    node.Nodes.Clear();
    ControlsHelper.SetImageIndex4RootNode(node, 6, this._iconService);
    List<Tuple<int, TreeNode>> tupleList = new List<Tuple<int, TreeNode>>();
    foreach (Tuple<int, int, List<int>> tuple1 in items)
    {
      Tuple<int, int, List<int>> item = tuple1;
      Tuple<int, TreeNode> tuple2 = tupleList.Find((Predicate<Tuple<int, TreeNode>>) (x => x.Item1 == item.Item1));
      TreeNode objectTypeNode;
      if (tuple2 == null)
      {
        objectTypeNode = ControlsHelper.CreateObjectTypeNode(item.Item1, (object) null, node.Nodes, this._iconService);
        tupleList.Add(new Tuple<int, TreeNode>(item.Item1, objectTypeNode));
      }
      else
        objectTypeNode = tuple2.Item2;
      ControlsHelper.CreateRelationTypeNode(item.Item2, (object) item, objectTypeNode.Nodes, this._iconService);
    }
  }

  public List<int> GetAttributes(TreeNode node)
  {
    return node.Tag is Tuple<int, int, List<int>> tag ? tag.Item3 : (List<int>) null;
  }

  public List<int> AddAttribute(TreeNode node)
  {
    return node.Tag is Tuple<int, int, List<int>> tag ? ControlsHelper.SelectAttributes4RelationType(tag.Item2) : (List<int>) null;
  }

  public void GetIDs(TreeNode node, out int parentTypeID, out int relationTypeID)
  {
    Tuple<int, int, List<int>> tag = (Tuple<int, int, List<int>>) node.Tag;
    parentTypeID = tag.Item1;
    relationTypeID = tag.Item2;
  }
}
