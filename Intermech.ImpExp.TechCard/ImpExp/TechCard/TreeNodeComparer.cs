// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TreeNodeComparer
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard;

internal class TreeNodeComparer : IComparer
{
  public int Compare(object x, object y)
  {
    if (x == y)
      return 0;
    TreeNode treeNode1 = x as TreeNode;
    TreeNode treeNode2 = y as TreeNode;
    if (treeNode1 == null)
      return -1;
    return treeNode2 == null ? 1 : string.CompareOrdinal(treeNode1.Text, treeNode2.Text);
  }
}
