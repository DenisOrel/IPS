// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.ICompareAttributesNode
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal interface ICompareAttributesNode
{
  List<int> OnOpenAttributeDialog(int id);

  void OnRefreshNode(TreeNode node, List<Tuple<int, List<int>>> items);
}
