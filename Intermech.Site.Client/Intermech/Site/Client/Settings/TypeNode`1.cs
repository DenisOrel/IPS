// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.Settings.TypeNode`1
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client.Settings;

internal abstract class TypeNode<T> : ITypeNode
{
  public TypeNode(int typeID, int categoryID, T properties, TreeNode node)
  {
    this.typeID = typeID;
    this.categoryID = categoryID;
    this.properties = properties;
    this.node = node;
    this.changed = false;
  }

  public virtual TreeNode[] Expand(IUserSession session) => (TreeNode[]) null;

  public virtual void Redraw(TreeNode node)
  {
  }

  public object Parameters => (object) this.properties;

  public abstract void Save(IUserSession session);

  protected virtual void OnChanged()
  {
  }

  public bool Changed
  {
    get => this.changed;
    set
    {
      this.changed = value;
      this.OnChanged();
    }
  }

  protected T properties { get; set; }

  protected int typeID { get; private set; }

  protected int categoryID { get; private set; }

  protected bool changed { get; set; }

  protected TreeNode node { get; private set; }
}
