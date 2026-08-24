// Decompiled with JetBrains decompiler
// Type: OxyPlot.CanonicalCode
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OxyPlot;

internal class CanonicalCode
{
  private readonly int[] codeLengths;

  public CanonicalCode(int[] codeLengths)
  {
    this.codeLengths = codeLengths != null ? new int[codeLengths.Length] : throw new Exception("Argument is null");
    Array.Copy((Array) codeLengths, (Array) this.codeLengths, codeLengths.Length);
    foreach (int codeLength in codeLengths)
    {
      if (codeLength < 0)
        throw new Exception("Illegal code length");
    }
  }

  public CanonicalCode(CodeTree tree, int symbolLimit)
  {
    this.codeLengths = new int[symbolLimit];
    this.BuildCodeLengths((Node) tree.Root, 0);
  }

  public int GetSymbolLimit() => this.codeLengths.Length;

  public int GetCodeLength(int symbol)
  {
    if (symbol < 0 || symbol >= this.codeLengths.Length)
      throw new Exception("Symbol out of range");
    return this.codeLengths[symbol];
  }

  public CodeTree ToCodeTree()
  {
    List<Node> nodeList1 = new List<Node>();
    for (int index1 = ((IEnumerable<int>) this.codeLengths).Max(); index1 >= 1; --index1)
    {
      List<Node> nodeList2 = new List<Node>();
      for (int symbol = 0; symbol < this.codeLengths.Length; ++symbol)
      {
        if (this.codeLengths[symbol] == index1)
          nodeList2.Add((Node) new Leaf(symbol));
      }
      for (int index2 = 0; index2 < nodeList1.Count; index2 += 2)
        nodeList2.Add((Node) new InternalNode(nodeList1[index2], nodeList1[index2 + 1]));
      nodeList1 = nodeList2;
      if (nodeList1.Count % 2 != 0)
        throw new Exception("This canonical code does not represent a Huffman code tree");
    }
    if (nodeList1.Count != 2)
      throw new Exception("This canonical code does not represent a Huffman code tree");
    return new CodeTree(new InternalNode(nodeList1[0], nodeList1[1]), this.codeLengths.Length);
  }

  private void BuildCodeLengths(Node node, int depth)
  {
    switch (node)
    {
      case InternalNode _:
        InternalNode internalNode = (InternalNode) node;
        this.BuildCodeLengths(internalNode.LeftChild, depth + 1);
        this.BuildCodeLengths(internalNode.RightChild, depth + 1);
        break;
      case Leaf _:
        int symbol = ((Leaf) node).Symbol;
        if (this.codeLengths[symbol] != 0)
          throw new Exception("Symbol has more than one code");
        if (symbol >= this.codeLengths.Length)
          throw new Exception("Symbol exceeds symbol limit");
        this.codeLengths[symbol] = depth;
        break;
      default:
        throw new Exception("Illegal node type");
    }
  }
}
