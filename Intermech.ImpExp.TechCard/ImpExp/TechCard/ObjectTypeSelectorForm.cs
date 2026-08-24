// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.ObjectTypeSelectorForm
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard;

public class ObjectTypeSelectorForm : TypeSelectorForm
{
  private bool _anyObjType = true;

  private void InitializeData() => this.Name = nameof (ObjectTypeSelectorForm);

  private TreeNode LoadObjectType(int objTypeId, IMetadataInfo metaInfo)
  {
    if (metaInfo == null)
      return (TreeNode) null;
    IObjectTypeItem byId = metaInfo.ObjectTypes.GetByID(objTypeId);
    if (byId == null)
      return (TreeNode) null;
    TreeNode treeNode = new TreeNode(byId.Name);
    treeNode.Tag = (object) byId;
    foreach (int childId in byId.ChildIDs)
    {
      TreeNode node = this.LoadObjectType(childId, metaInfo);
      if (node != null)
        treeNode.Nodes.Add(node);
    }
    return treeNode;
  }

  public ObjectTypeSelectorForm(string caption)
    : this(caption, true)
  {
  }

  public ObjectTypeSelectorForm(string caption, bool anyObjType)
    : base(new object[1]{ (object) anyObjType }, caption)
  {
    this.InitializeData();
  }

  public int ObjType
  {
    get
    {
      TreeNode selectedNode = this.tvType.SelectedNode;
      return selectedNode != null && selectedNode.Tag is IObjectTypeItem ? (selectedNode.Tag as IObjectTypeItem).ID : -1;
    }
  }

  public override object SelectedItem => (object) this.ObjType;

  protected override void SetParams(object[] data, string caption)
  {
    base.SetParams(data, caption);
    if (data == null || data.Length == 0)
      return;
    this._anyObjType = (bool) data[0];
  }

  protected override void LoadTypesTree()
  {
    this.tvType.BeginUpdate();
    try
    {
      this.tvType.Nodes.Clear();
      IMetadataInfo service = (IMetadataInfo) ServicesManager.GetService(typeof (IMetadataInfo));
      if (service == null)
        return;
      if (this._anyObjType)
      {
        TreeNode node = new TreeNode("Любой тип объекта");
        foreach (IObjectTypeItem objectType in (IEnumerable<IObjectTypeItem>) service.ObjectTypes)
        {
          if (objectType.ParentID == Guid.Empty)
            node.Nodes.Add(this.LoadObjectType(objectType.ID, service));
        }
        node.Expand();
        this.tvType.Nodes.Add(node);
      }
      else
      {
        foreach (IObjectTypeItem objectType in (IEnumerable<IObjectTypeItem>) service.ObjectTypes)
        {
          if (objectType.ParentID == Guid.Empty)
            this.tvType.Nodes.Add(this.LoadObjectType(objectType.ID, service));
        }
      }
    }
    finally
    {
      this.tvType.EndUpdate();
    }
  }
}
