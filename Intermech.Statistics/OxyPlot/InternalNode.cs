// Decompiled with JetBrains decompiler
// Type: OxyPlot.InternalNode
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;

#nullable disable
namespace OxyPlot;

internal sealed class InternalNode : Node
{
  public InternalNode(Node leftChild, Node rightChild)
  {
    if (leftChild == null)
      throw new ArgumentException("Argument is null", nameof (leftChild));
    if (rightChild == null)
      throw new ArgumentException("Argument is null", nameof (rightChild));
    this.LeftChild = leftChild;
    this.RightChild = rightChild;
  }

  public Node LeftChild { get; private set; }

  public Node RightChild { get; private set; }
}
