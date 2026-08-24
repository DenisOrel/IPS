// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.Settings.TreeNodes.RootTypeNode
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client.Settings.TreeNodes;

internal abstract class RootTypeNode
{
  public RootTypeNode(string caption, int category)
  {
    this.caption = caption;
    this.categoryID = category;
  }

  public abstract TreeNode BuildTree(IUserSession session);

  public abstract void SaveTree(IUserSession session, TreeNode rootNode);

  protected TreeNode CreateRootNode(ICategoryTypeIconService iconService)
  {
    TreeNode rootNode = new TreeNode(this.caption);
    int num1;
    int num2 = num1 = iconService.IndexOf(this.categoryID, 0);
    rootNode.SelectedImageIndex = num1;
    rootNode.ImageIndex = num2;
    rootNode.Tag = (object) this;
    return rootNode;
  }

  protected string caption { get; private set; }

  protected int categoryID { get; private set; }
}
