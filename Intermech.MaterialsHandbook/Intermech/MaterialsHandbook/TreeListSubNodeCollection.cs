// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.TreeListSubNodeCollection
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System.Collections;
using System.ComponentModel;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class TreeListSubNodeCollection : CollectionBase
{
  private TreeListView _ctrl;

  [Browsable(false)]
  public TreeListView Control
  {
    get => this._ctrl;
    set
    {
      this._ctrl = value;
      if (this._ctrl?.Columns != null)
      {
        for (int index = 0; index < this.List.Count && index < this._ctrl.Columns.Count - 1; ++index)
        {
          if (this.List[index] is TreeListSubNode treeListSubNode)
            treeListSubNode.Column = this._ctrl.Columns[index + 1];
        }
      }
      else
      {
        foreach (object obj in (IEnumerable) this.List)
        {
          if (obj is TreeListSubNode treeListSubNode)
            treeListSubNode.Column = (ColumnHeader) null;
        }
      }
    }
  }

  public TreeListSubNode this[int index]
  {
    get
    {
      try
      {
        return this.List[index] as TreeListSubNode;
      }
      catch
      {
        return (TreeListSubNode) null;
      }
    }
    set => this.List[index] = (object) value;
  }

  public int Add(TreeListSubNode node)
  {
    int index;
    lock (this.List.SyncRoot)
      index = this.List.Add((object) node) + 1;
    if (this._ctrl?.Columns != null && index < this._ctrl.Columns.Count)
      node.Column = this._ctrl.Columns[index];
    return index;
  }

  public TreeListSubNode Add(FieldTypes type, string name, string text)
  {
    TreeListSubNode node = new TreeListSubNode(type, name, text);
    this.Add(node);
    return node;
  }
}
