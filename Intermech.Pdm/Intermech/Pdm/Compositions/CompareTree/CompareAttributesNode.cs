// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.CompareAttributesNode
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal abstract class CompareAttributesNode : ICompareAttributesNode
{
  protected ICategoryTypeIconService iconService;

  public CompareAttributesNode(ICategoryTypeIconService iconService)
  {
    this.iconService = iconService;
  }

  public static void RefreshNode(
    TreeNode treeNode,
    CompoitionSettings settings,
    ICategoryTypeIconService iconService)
  {
    ICompareAttributesNode compareAttributesNode = (ICompareAttributesNode) null;
    List<Tuple<int, List<int>>> items = (List<Tuple<int, List<int>>>) null;
    switch ((RootNodeTypes) Convert.ToInt32(treeNode.Tag))
    {
      case RootNodeTypes.ObjectTypesList:
        compareAttributesNode = (ICompareAttributesNode) new CompareObjectAttributesNode(iconService);
        items = settings.ObjectCompareAttributes;
        break;
      case RootNodeTypes.RelationTypesList:
        compareAttributesNode = (ICompareAttributesNode) new CompareRelationAttributesNode(iconService);
        items = settings.RelationCompareAttributes;
        break;
    }
    treeNode.Nodes.Clear();
    if (items == null)
      return;
    compareAttributesNode.OnRefreshNode(treeNode, items);
  }

  public static List<int> OpenAttributeDialog(
    RootNodeTypes nodeType,
    int id,
    ICategoryTypeIconService iconService)
  {
    ICompareAttributesNode compareAttributesNode = (ICompareAttributesNode) null;
    if (RootNodeTypes.ObjectTypesList.Equals((object) nodeType))
      compareAttributesNode = (ICompareAttributesNode) new CompareObjectAttributesNode(iconService);
    else if (RootNodeTypes.RelationTypesList.Equals((object) nodeType))
      compareAttributesNode = (ICompareAttributesNode) new CompareRelationAttributesNode(iconService);
    return compareAttributesNode?.OnOpenAttributeDialog(id);
  }

  public abstract List<int> OnOpenAttributeDialog(int id);

  public abstract void OnRefreshNode(TreeNode node, List<Tuple<int, List<int>>> items);
}
