// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.ChangeRelationTypesForm
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using DevExpress.IM.XtraTreeList;
using DevExpress.IM.XtraTreeList.Columns;
using DevExpress.IM.XtraTreeList.Nodes;
using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal class ChangeRelationTypesForm : Form
{
  private List<int> _attributes;
  private Dictionary<int, bool> _relTypes;
  private bool _autoCheck;
  private IContainer components;
  private Panel panel1;
  private Panel panel2;
  private SplitContainer splitContainer1;
  private Button bCancel;
  private Button bOK;
  private GroupBox groupBox1;
  private GroupBox groupBox2;
  private TreeList tlRelTypes;
  private TreeListColumn treeListColumn1;
  private TreeList tlAttributes;
  private TreeListColumn treeListColumn2;
  private ImageList imagesState;

  public ChangeRelationTypesForm(Dictionary<int, bool> relTypes, List<int> attributes)
  {
    this.InitializeComponent();
    FormStorage.LoadLayout((Control) this);
    this._relTypes = relTypes;
    this._attributes = attributes;
  }

  public void Init()
  {
    this.tlRelTypes.BeginUpdate();
    try
    {
      this.tlRelTypes.Nodes.Clear();
      this.tlAttributes.Nodes.Clear();
      ImageList imageList = new ImageList();
      imageList.ColorDepth = ColorDepth.Depth24Bit;
      imageList.ImageSize = new Size(32 /*0x20*/, 16 /*0x10*/);
      this.tlRelTypes.SelectImageList = imageList;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        List<TreeListNode> treeListNodeList = new List<TreeListNode>();
        ICategoryTypeIconService service = ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
        foreach (KeyValuePair<int, bool> relType in this._relTypes)
        {
          IDBRelationType relationType = sessionKeeper.Session.GetRelationType(relType.Key);
          Icon icon = service.GetIcon(6, relationType.RelationType);
          imageList.Images.Add(ImagesResizeHelper.ResizeIconTo32x16(icon, this.tlRelTypes.BackColor));
          TreeListNode treeListNode = this.tlRelTypes.AppendNode((object) new object[1]
          {
            (object) relationType.Description
          }, (TreeListNode) null);
          treeListNode.SelectImageIndex = treeListNode.ImageIndex = imageList.Images.Count - 1;
          List<IMSAttribute4RelationType> relationTypeList = MetaDataHelper.GetAttribute4RelationTypeList(relationType.RelationType);
          List<int> attributes = new List<int>((int) relationType.Attributes.Count);
          for (int index = 0; index < relationTypeList.Count; ++index)
            attributes.Add(relationTypeList[index].AttributeID);
          treeListNode.Tag = (object) new ChangeRelationTypesForm.Relation(relationType.RelationType, attributes);
          if (relType.Value)
            treeListNodeList.Add(treeListNode);
        }
        if (treeListNodeList.Count <= 0)
          return;
        foreach (TreeListNode node in this.tlRelTypes.Nodes)
          node.CheckState = treeListNodeList.Contains(node) ? CheckState.Checked : CheckState.Unchecked;
      }
    }
    finally
    {
      this.tlRelTypes.EndUpdate();
    }
  }

  private void tlRelTypes_CheckStateChanged(object sender, NodeEventArgs e)
  {
    ChangeRelationTypesForm.Relation tag1 = (ChangeRelationTypesForm.Relation) e.Node.Tag;
    this.tlAttributes.BeginUpdate();
    try
    {
      for (int index1 = 0; index1 < tag1.Attributes.Count; ++index1)
      {
        int attribute = tag1.Attributes[index1];
        if (e.Node.CheckState == CheckState.Checked)
        {
          bool flag = false;
          for (int index2 = 0; index2 < this.tlAttributes.Nodes.Count; ++index2)
          {
            if ((int) this.tlAttributes.Nodes[index2].Tag == attribute)
            {
              flag = true;
              break;
            }
          }
          if (!flag)
          {
            TreeListNode treeListNode = this.tlAttributes.AppendNode((object) new object[1]
            {
              (object) MetaDataHelper.GetAttributeTypeName(attribute)
            }, (TreeListNode) null);
            treeListNode.Tag = (object) attribute;
            if (this._attributes.Contains(attribute))
            {
              this._autoCheck = true;
              try
              {
                treeListNode.CheckState = CheckState.Checked;
              }
              finally
              {
                this._autoCheck = false;
              }
            }
          }
        }
        else
        {
          bool flag = false;
          for (int index3 = 0; index3 < this.tlRelTypes.Nodes.Count; ++index3)
          {
            ChangeRelationTypesForm.Relation tag2 = (ChangeRelationTypesForm.Relation) this.tlRelTypes.Nodes[index3].Tag;
            if (tag2.Id != tag1.Id && this.tlRelTypes.Nodes[index3].CheckState == CheckState.Checked && tag2.Attributes.Contains(attribute))
            {
              flag = true;
              break;
            }
          }
          if (!flag)
          {
            for (int index4 = 0; index4 < this.tlAttributes.Nodes.Count; ++index4)
            {
              if ((int) this.tlAttributes.Nodes[index4].Tag == attribute)
              {
                this.tlAttributes.Nodes.RemoveAt(index4);
                break;
              }
            }
            if (this._attributes.Contains(attribute))
              this._attributes.Remove(attribute);
          }
        }
      }
      this._relTypes[tag1.Id] = e.Node.CheckState == CheckState.Checked;
    }
    finally
    {
      this.tlAttributes.EndUpdate();
    }
  }

  private void tlAttributes_CheckStateChanged(object sender, NodeEventArgs e)
  {
    if (this._autoCheck)
      return;
    if (e.Node.CheckState == CheckState.Checked)
    {
      if (this._attributes.Contains((int) e.Node.Tag))
        return;
      this._attributes.Add((int) e.Node.Tag);
    }
    else
    {
      if (!this._attributes.Contains((int) e.Node.Tag))
        return;
      this._attributes.Remove((int) e.Node.Tag);
    }
  }

  private void bOK_Click(object sender, EventArgs e)
  {
    if (!this._relTypes.ContainsValue(true))
    {
      int num = (int) MessageBox.Show(LocalizationHolder.rm.GetString("Pdm_520"), LocalizationHolder.rm.GetString("Pdm_521"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      this.DialogResult = DialogResult.None;
    }
    else
      this.DialogResult = DialogResult.OK;
  }

  protected override void Dispose(bool disposing)
  {
    FormStorage.SaveLayout((Control) this);
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ChangeRelationTypesForm));
    this.splitContainer1 = new SplitContainer();
    this.groupBox1 = new GroupBox();
    this.tlRelTypes = new TreeList();
    this.treeListColumn1 = new TreeListColumn();
    this.imagesState = new ImageList(this.components);
    this.groupBox2 = new GroupBox();
    this.tlAttributes = new TreeList();
    this.treeListColumn2 = new TreeListColumn();
    this.panel1 = new Panel();
    this.bCancel = new Button();
    this.bOK = new Button();
    this.panel2 = new Panel();
    this.splitContainer1.BeginInit();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.Panel2.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    this.groupBox1.SuspendLayout();
    this.tlRelTypes.BeginInit();
    this.groupBox2.SuspendLayout();
    this.tlAttributes.BeginInit();
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.splitContainer1, "splitContainer1");
    this.splitContainer1.Name = "splitContainer1";
    this.splitContainer1.Panel1.Controls.Add((Control) this.groupBox1);
    this.splitContainer1.Panel2.Controls.Add((Control) this.groupBox2);
    this.groupBox1.Controls.Add((Control) this.tlRelTypes);
    componentResourceManager.ApplyResources((object) this.groupBox1, "groupBox1");
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.TabStop = false;
    componentResourceManager.ApplyResources((object) this.tlRelTypes, "tlRelTypes");
    this.tlRelTypes.CheckBoxes = CheckBoxesStyle.TwoState;
    this.tlRelTypes.Columns.AddRange(new TreeListColumn[1]
    {
      this.treeListColumn1
    });
    this.tlRelTypes.Name = "tlRelTypes";
    this.tlRelTypes.StateImageList = this.imagesState;
    this.tlRelTypes.CheckStateChanged += new NodeEventHandler(this.tlRelTypes_CheckStateChanged);
    componentResourceManager.ApplyResources((object) this.treeListColumn1, "treeListColumn1");
    this.treeListColumn1.Name = "treeListColumn1";
    this.imagesState.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imagesState.ImageStream");
    this.imagesState.TransparentColor = Color.Transparent;
    this.imagesState.Images.SetKeyName(0, "unchecked.ico");
    this.imagesState.Images.SetKeyName(1, "checked.ico");
    this.imagesState.Images.SetKeyName(2, "grayed.ico");
    this.groupBox2.Controls.Add((Control) this.tlAttributes);
    componentResourceManager.ApplyResources((object) this.groupBox2, "groupBox2");
    this.groupBox2.Name = "groupBox2";
    this.groupBox2.TabStop = false;
    componentResourceManager.ApplyResources((object) this.tlAttributes, "tlAttributes");
    this.tlAttributes.CheckBoxes = CheckBoxesStyle.TwoState;
    this.tlAttributes.Columns.AddRange(new TreeListColumn[1]
    {
      this.treeListColumn2
    });
    this.tlAttributes.Name = "tlAttributes";
    this.tlAttributes.StateImageList = this.imagesState;
    this.tlAttributes.CheckStateChanged += new NodeEventHandler(this.tlAttributes_CheckStateChanged);
    componentResourceManager.ApplyResources((object) this.treeListColumn2, "treeListColumn2");
    this.treeListColumn2.Name = "treeListColumn2";
    this.panel1.Controls.Add((Control) this.bCancel);
    this.panel1.Controls.Add((Control) this.bOK);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    componentResourceManager.ApplyResources((object) this.bCancel, "bCancel");
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Name = "bCancel";
    this.bCancel.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.bOK, "bOK");
    this.bOK.DialogResult = DialogResult.OK;
    this.bOK.Name = "bOK";
    this.bOK.UseVisualStyleBackColor = true;
    this.bOK.Click += new EventHandler(this.bOK_Click);
    this.panel2.Controls.Add((Control) this.splitContainer1);
    componentResourceManager.ApplyResources((object) this.panel2, "panel2");
    this.panel2.Name = "panel2";
    this.AcceptButton = (IButtonControl) this.bOK;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.bCancel;
    this.Controls.Add((Control) this.panel2);
    this.Controls.Add((Control) this.panel1);
    this.Name = nameof (ChangeRelationTypesForm);
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.Panel2.ResumeLayout(false);
    this.splitContainer1.EndInit();
    this.splitContainer1.ResumeLayout(false);
    this.groupBox1.ResumeLayout(false);
    this.tlRelTypes.EndInit();
    this.groupBox2.ResumeLayout(false);
    this.tlAttributes.EndInit();
    this.panel1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  private class Relation
  {
    public int Id;
    public List<int> Attributes;

    public Relation(int id, List<int> attributes)
    {
      this.Id = id;
      this.Attributes = attributes;
    }
  }
}
