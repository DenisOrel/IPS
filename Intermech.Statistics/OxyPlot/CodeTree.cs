// Decompiled with JetBrains decompiler
// Type: OxyPlot.CodeTree
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace OxyPlot;

internal class CodeTree
{
  private readonly List<List<int>> codes;

  public CodeTree(InternalNode root, int symbolLimit)
  {
    this.Root = root != null ? root : throw new Exception("Argument is null");
    this.codes = new List<List<int>>();
    for (int index = 0; index < symbolLimit; ++index)
      this.codes.Add((List<int>) null);
    this.BuildCodeList((Node) root, new List<int>());
  }

  public InternalNode Root { get; private set; }

  public List<int> GetCode(int symbol)
  {
    if (symbol < 0)
      throw new Exception("Illegal symbol");
    return this.codes[symbol] != null ? this.codes[symbol] : throw new Exception("No code for given symbol");
  }

  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    CodeTree.NodeString(string.Empty, (Node) this.Root, sb);
    return sb.ToString();
  }

  private static void NodeString(string prefix, Node node, StringBuilder sb)
  {
    switch (node)
    {
      case InternalNode internalNode:
        CodeTree.NodeString(prefix + "0", internalNode.LeftChild, sb);
        CodeTree.NodeString(prefix + "1", internalNode.RightChild, sb);
        break;
      case Leaf leaf:
        sb.Append($"Code {prefix}: Symbol {leaf.Symbol}");
        break;
      default:
        throw new Exception("Illegal node type");
    }
  }

  private void BuildCodeList(Node node, List<int> prefix)
  {
    switch (node)
    {
      case InternalNode _:
        InternalNode internalNode = (InternalNode) node;
        prefix.Add(0);
        this.BuildCodeList(internalNode.LeftChild, prefix);
        prefix.RemoveAt(prefix.Count - 1);
        prefix.Add(1);
        this.BuildCodeList(internalNode.RightChild, prefix);
        prefix.RemoveAt(prefix.Count - 1);
        break;
      case Leaf _:
        Leaf leaf = (Leaf) node;
        if (leaf.Symbol >= this.codes.Count)
          throw new Exception("Symbol exceeds symbol limit");
        if (this.codes[leaf.Symbol] != null)
          throw new Exception("Symbol has more than one code");
        this.codes[leaf.Symbol] = new List<int>((IEnumerable<int>) prefix);
        break;
      default:
        throw new Exception("Illegal node type");
    }
  }
}
