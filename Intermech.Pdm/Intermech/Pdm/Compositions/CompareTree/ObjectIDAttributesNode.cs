// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.ObjectIDAttributesNode
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class ObjectIDAttributesNode
{
  private ICategoryTypeIconService _iconService;

  public ObjectIDAttributesNode(ICategoryTypeIconService iconService)
  {
    this._iconService = iconService;
  }

  public bool IsChildNode(TreeNode node) => node.Tag != null && node.Tag is Tuple<int, List<int>>;

  public void RefreshNode(TreeNode node, List<Tuple<int, List<int>>> items)
  {
    node.Nodes.Clear();
    ControlsHelper.SetImageIndex4RootNode(node, 4, this._iconService);
    List<Tuple<int, TreeNode>> tupleList = new List<Tuple<int, TreeNode>>();
    foreach (Tuple<int, List<int>> tag in items)
    {
      int parentType = MetaDataHelper.GetObjectTypeParentID(tag.Item1);
      Tuple<int, TreeNode> tuple = tupleList.Find((Predicate<Tuple<int, TreeNode>>) (x => x.Item1 == parentType));
      TreeNodeCollection nodes = tuple == null ? node.Nodes : tuple.Item2.Nodes;
      TreeNode objectTypeNode = ControlsHelper.CreateObjectTypeNode(tag.Item1, (object) tag, nodes, this._iconService);
      tupleList.Add(new Tuple<int, TreeNode>(tag.Item1, objectTypeNode));
    }
  }

  public List<int> GetAttributes(TreeNode node)
  {
    return node.Tag is Tuple<int, List<int>> tag ? tag.Item2 : (List<int>) null;
  }

  public List<int> AddAttribute(TreeNode node)
  {
    return node.Tag is Tuple<int, List<int>> tag ? ControlsHelper.SelectAttributes4ObjectType(tag.Item1) : (List<int>) null;
  }

  public int GetObjectType(TreeNode node) => ((Tuple<int, List<int>>) node.Tag).Item1;
}
