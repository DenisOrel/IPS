// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.Settings.ObjectTypeNode
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client.Settings;

internal sealed class ObjectTypeNode : TypeNode<ObjectType4PublicationProrerties>
{
  private Guid _guid;
  private Guid _nodeGuid = Guid.NewGuid();

  public ObjectTypeNode(
    int typeID,
    Guid guid,
    ObjectType4PublicationProrerties properties,
    TreeNode node)
    : base(typeID, 4, properties, node)
  {
    this._guid = guid;
  }

  public override TreeNode[] Expand(IUserSession session)
  {
    DataTable dataTable = session.GetObjectTypeCollection(this.typeID).Select("F_OBJ_TYPE_NAME");
    List<TreeNode> treeNodeList = new List<TreeNode>(dataTable.Rows.Count);
    try
    {
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        treeNodeList.Add(ObjectTypeNode.Create(session, row));
    }
    finally
    {
      PublishTypesHelper.ClearCache();
    }
    return treeNodeList.Count <= 0 ? (TreeNode[]) null : treeNodeList.ToArray();
  }

  public static TreeNode Create(IUserSession session, DataRow row)
  {
    ICategoryTypeIconService service = ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
    TreeNode node = new TreeNode(Convert.ToString(row["F_OBJ_TYPE_NAME"]));
    int int32 = Convert.ToInt32(row["F_OBJECT_TYPE"]);
    IPublishTypesConfiguration customService1 = session.GetCustomService(typeof (IPublishTypesConfiguration)) as IPublishTypesConfiguration;
    bool isPublish = customService1.IsPublishObjectType(int32);
    bool flag = customService1.ObjectWithLink(int32);
    IPortalConnector customService2 = (IPortalConnector) session.GetCustomService(typeof (IPortalConnector));
    IDBObjectType objectType = session.GetObjectType(int32);
    ObjectType4PublicationProrerties properties = !customService2.IsOffline ? (ObjectType4PublicationProrerties) new ObjectType4PublicationProrertiesEx(isPublish, flag, PublishTypesHelper.GetPublishType(session, objectType)) : new ObjectType4PublicationProrerties(isPublish, flag);
    node.Tag = (object) new ObjectTypeNode(int32, (objectType as IDBGuid).GUID, properties, node);
    node.ImageIndex = node.SelectedImageIndex = service.IndexOf(4, int32);
    node.Nodes.Add(new TreeNode());
    ObjectTypeNode.SetNodeColor(node, ((TypeNode<ObjectType4PublicationProrerties>) node.Tag).properties.IsPublish);
    return node;
  }

  private static void SetNodeColor(TreeNode node, bool isPublish)
  {
    node.ForeColor = isPublish ? SystemColors.WindowText : Color.DarkGray;
  }

  public override void Redraw(TreeNode node)
  {
    ObjectTypeNode tag = node.Tag as ObjectTypeNode;
    ObjectTypeNode.SetNodeColor(node, tag.properties.IsPublish);
  }

  public override void Save(IUserSession session)
  {
    if (this.Changed)
    {
      IPublishTypesConfiguration customService = session.GetCustomService(typeof (IPublishTypesConfiguration)) as IPublishTypesConfiguration;
      if (!this.properties.IsPublish && customService.IsPublishObjectType(this.typeID))
        customService.RemovePublishObjectType(this.typeID, false);
      else if (this.properties.IsPublish && !customService.IsPublishObjectType(this.typeID))
        customService.AddPublishObjectType(this.typeID, false);
      customService.SetObjectWithLink(this.typeID, this.properties.ObjectWithLink);
      PublishTypesHelper.SetPublishType(session, this._guid, this.properties);
    }
    ObjectTypeNode.SaveChild(session, this.node.Nodes);
  }

  protected override void OnChanged()
  {
    TreeNodeCollection childNodes = this.GetChildNodes(this.node);
    if (childNodes == null || childNodes.Count <= 0 || MessageBox.Show("Установить значение всем дочерним типам?", SiteClientConsts.PublishTypesSettingsCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
      return;
    this.SetParametersChildRecursive(this);
  }

  private TreeNodeCollection GetChildNodes(TreeNode parentNode)
  {
    if (parentNode.Nodes.Count == 1 && parentNode.Nodes[0].Tag == null)
    {
      parentNode.Nodes.Clear();
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        TreeNode[] nodes = ((TypeNode<ObjectType4PublicationProrerties>) parentNode.Tag).Expand(sessionKeeper.Session);
        if (nodes != null)
          parentNode.Nodes.AddRange(nodes);
      }
    }
    return parentNode.Nodes;
  }

  private void SetParametersChildRecursive(ObjectTypeNode parentNode)
  {
    TreeNodeCollection childNodes = this.GetChildNodes(parentNode.node);
    if (childNodes == null || childNodes.Count <= 0)
      return;
    foreach (TreeNode node in childNodes)
    {
      ObjectTypeNode tag = node.Tag as ObjectTypeNode;
      tag.properties.IsPublish = parentNode.properties.IsPublish;
      tag.properties.ObjectWithLink = parentNode.properties.ObjectWithLink;
      if (parentNode.properties is ObjectType4PublicationProrertiesEx properties)
        ((ObjectType4PublicationProrertiesEx) tag.properties).PublishType = (PublishTypeAttProxy) properties.PublishType.Clone();
      tag.changed = true;
      ObjectTypeNode.SetNodeColor(node, tag.properties.IsPublish);
      this.SetParametersChildRecursive(tag);
    }
  }

  public static void SaveChild(IUserSession session, TreeNodeCollection nodes)
  {
    if (nodes == null || nodes.Count <= 0)
      return;
    foreach (TreeNode node in nodes)
    {
      if (node.Tag != null)
        ((TypeNode<ObjectType4PublicationProrerties>) node.Tag).Save(session);
    }
  }
}
