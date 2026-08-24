// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.Settings.TreeNodes.RelationTypeRootNode
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System.Data;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client.Settings.TreeNodes;

internal sealed class RelationTypeRootNode : RootTypeNode
{
  public RelationTypeRootNode()
    : base("Типы связей публикуемого состава", 6)
  {
  }

  public override TreeNode BuildTree(IUserSession session)
  {
    TreeNode rootNode = this.CreateRootNode(ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService);
    foreach (DataRow row in (InternalDataCollectionBase) session.GetRelationTypeCollection().Select("F_DESCRIPTION").Rows)
      rootNode.Nodes.Add(RelationTypeNode.Create(session, row));
    return rootNode;
  }

  public override void SaveTree(IUserSession session, TreeNode rootNode)
  {
    foreach (TreeNode node in rootNode.Nodes)
      (node.Tag as RelationTypeNode).Save(session);
  }
}
