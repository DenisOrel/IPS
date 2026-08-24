// Decompiled with JetBrains decompiler
// Type: Intermech.Search.MSOfficeReviews.MSOfficeReviewObjectTypesEditor
// Assembly: Intermech.Search.MSOfficeReviews.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 4AB1E446-C278-4B7C-8A5E-DB94EF37D83B
// Assembly location: D:\IPS\Client\Intermech.Search.MSOfficeReviews.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.MSOfficeReviews;

public sealed class MSOfficeReviewObjectTypesEditor : UITypeEditor
{
  public override object EditValue(
    ITypeDescriptorContext context,
    IServiceProvider provider,
    object value)
  {
    using (TreeViewWithButtonsForm viewWithButtonsForm = new TreeViewWithButtonsForm())
    {
      viewWithButtonsForm.DisableGroupCheckedNodes = true;
      TreeNode nodeForObjectType = this.CreateTreeNodeForObjectType(MSOfficeReviewsConstants.DocumentObjectTypeID);
      viewWithButtonsForm.Nodes.Add(nodeForObjectType);
      if (ServicesManager.GetService(typeof (ICategoryTypeIconService)) is ICategoryTypeIconService service)
        viewWithButtonsForm.ImageList = service.ImageList;
      if (!(value is int[] numArray))
        numArray = new int[0];
      int[] source = numArray;
      viewWithButtonsForm.CheckedTags = source.Cast<object>().ToList<object>();
      viewWithButtonsForm.ShowCheckedNodes();
      return viewWithButtonsForm.ShowDialog() == DialogResult.OK ? (object) viewWithButtonsForm.CheckedTags.Cast<int>().ToArray<int>() : (object) source;
    }
  }

  public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
  {
    return UITypeEditorEditStyle.Modal;
  }

  private TreeNode CreateTreeNodeForObjectType(int objectTypeID)
  {
    return this.CreateTreeNodeForObjectType(MetaDataHelper.GetObjectType(objectTypeID));
  }

  private TreeNode CreateTreeNodeForObjectType(IMSObjectType objectType)
  {
    TreeNode nodeForObjectType = new TreeNode(objectType.ObjectTypeName)
    {
      Tag = (object) objectType.ObjectTypeID
    };
    if (ServicesManager.GetService(typeof (ICategoryTypeIconService)) is ICategoryTypeIconService service)
      nodeForObjectType.ImageIndex = nodeForObjectType.SelectedImageIndex = service.IndexOf(4, objectType.ObjectTypeID);
    foreach (IMSObjectType objectType1 in MetaDataHelper.GetObjectTypeChildrenID(objectType.ObjectTypeID).Select<int, IMSObjectType>((Func<int, IMSObjectType>) (o => MetaDataHelper.GetObjectType(o))).OrderBy<IMSObjectType, string>((Func<IMSObjectType, string>) (o => o.ObjectTypeName)).ToArray<IMSObjectType>())
    {
      if (objectType1.ObjectTypeID != MSOfficeReviewsConstants.ReviewObjectTypeID)
        nodeForObjectType.Nodes.Add(this.CreateTreeNodeForObjectType(objectType1));
    }
    return nodeForObjectType;
  }
}
