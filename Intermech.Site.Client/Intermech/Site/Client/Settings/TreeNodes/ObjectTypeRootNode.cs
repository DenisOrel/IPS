// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.Settings.TreeNodes.ObjectTypeRootNode
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using System.Data;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client.Settings.TreeNodes;

internal sealed class ObjectTypeRootNode : RootTypeNode
{
  public ObjectTypeRootNode()
    : base("Типы публикуемых объектов", 4)
  {
  }

  public override TreeNode BuildTree(IUserSession session)
  {
    session.GetCustomService(typeof (IPublishTypesConfiguration));
    TreeNode rootNode = this.CreateRootNode(ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService);
    DataTable dataTable = session.GetObjectTypeCollection(-1).Select("F_OBJ_TYPE_NAME");
    try
    {
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        rootNode.Nodes.Add(ObjectTypeNode.Create(session, row));
    }
    finally
    {
      PublishTypesHelper.ClearCache();
    }
    return rootNode;
  }

  public override void SaveTree(IUserSession session, TreeNode rootNode)
  {
    ObjectTypeNode.SaveChild(session, rootNode.Nodes);
  }
}
