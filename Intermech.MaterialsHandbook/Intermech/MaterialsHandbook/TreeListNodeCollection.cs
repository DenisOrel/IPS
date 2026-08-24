// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.TreeListNodeCollection
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class TreeListNodeCollection : List<TreeListNode>
{
  private TreeListView _ctrl;

  [Browsable(false)]
  public TreeListView Control
  {
    get => this._ctrl;
    set
    {
      this._ctrl = value;
      this.ForEach((Action<TreeListNode>) (x => x.Control = value));
    }
  }

  [Browsable(false)]
  public TreeListNode Owner { get; set; }

  public event EventHandler NodesChanged;

  public event EventHandler SelectedChanged;

  private void OnNodesChanged(object sender, EventArgs e)
  {
    EventHandler nodesChanged = this.NodesChanged;
    if (nodesChanged == null)
      return;
    nodesChanged(sender, e);
  }

  private void OnNode_SelectedChanged(object sender, EventArgs e)
  {
    EventHandler selectedChanged = this.SelectedChanged;
    if (selectedChanged == null)
      return;
    selectedChanged(sender, e);
  }

  public new TreeListNode this[int index]
  {
    get => this.Count <= 0 || index >= this.Count ? (TreeListNode) null : base[index];
    set
    {
      value.Parent = this.Owner;
      value.Nodes.NodesChanged += new EventHandler(this.OnNodesChanged);
      value.SelectedChanged += new EventHandler(this.OnNode_SelectedChanged);
      base[index] = value;
      this.OnNodesChanged((object) this, new EventArgs());
    }
  }

  public int Add(TreeListNode node)
  {
    node.Parent = this.Owner;
    node.Control = this._ctrl;
    node.Nodes.NodesChanged += new EventHandler(this.OnNodesChanged);
    node.SelectedChanged += new EventHandler(this.OnNode_SelectedChanged);
    base.Add(node);
    int num = this.Count - 1;
    this.OnNodesChanged((object) this, EventArgs.Empty);
    return num;
  }

  public new void Insert(int index, TreeListNode node)
  {
    node.Parent = this.Owner;
    node.Control = this._ctrl;
    node.Nodes.NodesChanged += new EventHandler(this.OnNodesChanged);
    node.SelectedChanged += new EventHandler(this.OnNode_SelectedChanged);
    base.Insert(index, node);
    this.OnNodesChanged((object) this, EventArgs.Empty);
  }

  public void Remove(TreeListNode node)
  {
    if (node.Selected)
      node.Selected = false;
    node.Nodes.NodesChanged -= new EventHandler(this.OnNodesChanged);
    node.SelectedChanged -= new EventHandler(this.OnNode_SelectedChanged);
    base.Remove(node);
    this.OnNodesChanged((object) this, EventArgs.Empty);
  }

  public new void Clear()
  {
    for (int index = 0; index < this.Count; ++index)
    {
      TreeListNode treeListNode = this[index];
      treeListNode.Nodes.Clear();
      treeListNode.Selected = false;
    }
    base.Clear();
    this.OnNodesChanged((object) this, EventArgs.Empty);
  }

  public void Sort(bool asc, int index)
  {
    if (index == 0)
      this.Sort(new Comparison<TreeListNode>(ComparerHelper.Compare));
    else
      this.Sort((Comparison<TreeListNode>) ((x, y) => ComparerHelper.Compare((ITreeListNode) x.SubNodes[index - 1], (ITreeListNode) y.SubNodes[index - 1])));
    if (asc)
      return;
    this.Reverse();
  }
}
