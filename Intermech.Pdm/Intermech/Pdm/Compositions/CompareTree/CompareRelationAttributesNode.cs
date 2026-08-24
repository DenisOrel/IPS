// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.CompareRelationAttributesNode
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class CompareRelationAttributesNode(ICategoryTypeIconService iconService) : 
  CompareAttributesNode(iconService)
{
  public override List<int> OnOpenAttributeDialog(int id)
  {
    return ControlsHelper.SelectAttributes4RelationType(id);
  }

  public override void OnRefreshNode(TreeNode node, List<Tuple<int, List<int>>> items)
  {
    ControlsHelper.SetImageIndex4RootNode(node, 6, this.iconService);
    foreach (Tuple<int, List<int>> tag in items)
      ControlsHelper.CreateRelationTypeNode(tag.Item1, (object) tag, node.Nodes, this.iconService);
  }
}
