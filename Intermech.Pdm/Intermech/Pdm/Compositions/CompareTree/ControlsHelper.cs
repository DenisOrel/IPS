// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.ControlsHelper
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal static class ControlsHelper
{
  public static Color ChangedColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192 /*0xC0*/);
  public static Color AddedColor = Color.FromArgb(192 /*0xC0*/, (int) byte.MaxValue, 192 /*0xC0*/);
  public static Color RemovedColor = Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 192 /*0xC0*/);
  public static readonly int AttributeChangesID = MetaDataHelper.GetAttributeTypeID(PDMHelper.attributeChanges);

  public static List<int> SelectAttributes4ObjectType(int objectTypeID)
  {
    MetaDataHelper.GetObjectType(objectTypeID);
    AttributesSelectDlg attributesSelectDlg = new AttributesSelectDlg(true);
    attributesSelectDlg.LoadAttrDialogForObjectsTypes(new List<int>()
    {
      objectTypeID
    });
    return attributesSelectDlg.ShowDialog() == DialogResult.OK && attributesSelectDlg.SelectedAttributesID.Count > 0 ? attributesSelectDlg.SelectedAttributesID : (List<int>) null;
  }

  public static List<int> SelectAttributes4RelationType(int relationTypeID)
  {
    MetaDataHelper.GetRelationType(relationTypeID);
    AttributesSelectDlg attributesSelectDlg = new AttributesSelectDlg(true);
    attributesSelectDlg.LoadAttrDialogForRelationsTypes(new List<int>()
    {
      relationTypeID
    });
    return attributesSelectDlg.ShowDialog() == DialogResult.OK && attributesSelectDlg.SelectedAttributesID.Count > 0 ? attributesSelectDlg.SelectedAttributesID : (List<int>) null;
  }

  public static TreeNode CreateObjectTypeNode(
    int objectTypeID,
    object tag,
    TreeNodeCollection nodes,
    ICategoryTypeIconService iconService)
  {
    int num = iconService.IndexOf(4, objectTypeID);
    TreeNode node = new TreeNode(MetaDataHelper.GetObjectTypeName(objectTypeID))
    {
      Tag = tag,
      ImageIndex = num,
      SelectedImageIndex = num
    };
    nodes.Add(node);
    return node;
  }

  public static TreeNode CreateRelationTypeNode(
    int id,
    object tag,
    TreeNodeCollection nodes,
    ICategoryTypeIconService iconService)
  {
    int num = iconService.IndexOf(6, id);
    TreeNode node = new TreeNode(MetaDataHelper.GetRelationTypeName(id))
    {
      Tag = tag,
      ImageIndex = num,
      SelectedImageIndex = num
    };
    nodes.Add(node);
    return node;
  }

  public static void SetImageIndex4RootNode(
    TreeNode node,
    int category,
    ICategoryTypeIconService iconService)
  {
    if (node.ImageIndex != -1)
      return;
    int num = iconService.IndexOf(category, 0);
    node.ImageIndex = num;
    node.SelectedImageIndex = num;
  }

  public static ListViewItem CreateAttributeListViewItem(
    int attributeID,
    ICategoryTypeIconService iconService)
  {
    IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(attributeID);
    string attributeTypeName = MetaDataHelper.GetAttributeTypeName(attributeID);
    return new ListViewItem(attributeTypeName)
    {
      Tag = (object) new MetadataListNode(attributeID, attributeTypeName),
      ImageIndex = iconService.IndexOf(3, -1, (object) attributeType.FieldType)
    };
  }
}
