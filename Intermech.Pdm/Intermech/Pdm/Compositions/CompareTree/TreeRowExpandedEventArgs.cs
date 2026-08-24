// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.TreeRowExpandedEventArgs
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Infralution.Controls.VirtualTree;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

public class TreeRowExpandedEventArgs
{
  public Row Row1 { get; }

  public Row Row2 { get; }

  public TreeRowExpandedEventArgs(Row row1, Row row2)
  {
    this.Row1 = row1;
    this.Row2 = row2;
  }
}
