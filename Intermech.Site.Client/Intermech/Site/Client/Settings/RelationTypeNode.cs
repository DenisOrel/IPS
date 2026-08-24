// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.Settings.RelationTypeNode
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using System;
using System.Data;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client.Settings;

internal sealed class RelationTypeNode(
  int typeID,
  RelationTypeProrerties properties,
  TreeNode node) : TypeNode<RelationTypeProrerties>(typeID, 4, properties, node)
{
  public static TreeNode Create(IUserSession session, DataRow row)
  {
    IPublishTypesConfiguration customService = session.GetCustomService(typeof (IPublishTypesConfiguration)) as IPublishTypesConfiguration;
    ICategoryTypeIconService service = ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
    TreeNode node = new TreeNode(Convert.ToString(row["F_DESCRIPTION"]));
    int int32 = Convert.ToInt32(row["F_RELATION_TYPE"]);
    Guid relationType = new Guid(Convert.ToString(row["F_GUID"]));
    RelationMigrateType relationMigrateType = customService.GetRelationMigrateType(relationType);
    node.Tag = (object) new RelationTypeNode(int32, new RelationTypeProrerties(relationMigrateType), node);
    node.ImageIndex = node.SelectedImageIndex = service.IndexOf(6, int32);
    return node;
  }

  public override void Save(IUserSession session)
  {
    if (!this.Changed)
      return;
    (session.GetCustomService(typeof (IPublishTypesConfiguration)) as IPublishTypesConfiguration).SetRelationMigrateType(MetaDataHelper.GetRelationTypeGuid(this.typeID), this.properties.MigrateType, false);
  }
}
