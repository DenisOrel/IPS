// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PublishTypeSelectForm
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client;

public class PublishTypeSelectForm : Form
{
  private IPortalMetadata _metadata;
  private ImageList _imageList;
  private IContainer components;
  private Panel panel1;
  private Button bOK;
  private Button bCancel;
  private TreeView treeView1;

  public PublishTypeSelectForm(IPortalMetadata metadata)
  {
    this.InitializeComponent();
    this._metadata = metadata;
    this._imageList = new ImageList();
    this._imageList.ColorDepth = ColorDepth.Depth24Bit;
    this._imageList.ImageSize = new Size(32 /*0x20*/, 16 /*0x10*/);
    this.treeView1.ImageList = this._imageList;
    HelpProvidersClass.SetHelpOptionForControl((Control) this, 1733);
  }

  public PublishTypeAttProxy SelectedPublishType
  {
    get
    {
      return this.treeView1.SelectedNode != null && this.treeView1.SelectedNode.Tag is PortalObjectType ? new PublishTypeAttProxy(((PortalObjectType) this.treeView1.SelectedNode.Tag).ID, new Guid(((PortalObjectType) this.treeView1.SelectedNode.Tag).GUID), ((PortalObjectType) this.treeView1.SelectedNode.Tag).Name) : (PublishTypeAttProxy) null;
    }
  }

  public static PublishTypeAttProxy SelectType(IPortalMetadata metadata, Guid selectedType)
  {
    using (PublishTypeSelectForm publishTypeSelectForm = new PublishTypeSelectForm(metadata))
    {
      publishTypeSelectForm.BuildTree(selectedType);
      if (publishTypeSelectForm.ShowDialog() == DialogResult.OK)
        return publishTypeSelectForm.SelectedPublishType;
    }
    return (PublishTypeAttProxy) null;
  }

  private void RefreshControls()
  {
    this.bOK.Enabled = this.treeView1.SelectedNode != null && this.treeView1.SelectedNode.Tag is PortalObjectType;
  }

  private TreeNode AddNodes(TreeNodeCollection nodes, Guid selectedType, int parentType)
  {
    TreeNode treeNode1 = (TreeNode) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (this._metadata != null)
      {
        PortalObjectType[] childObjectTypes = this._metadata.GetChildObjectTypes(sessionKeeper.Session, parentType, false);
        if (childObjectTypes != null)
        {
          for (int index = 0; index < childObjectTypes.Length; ++index)
          {
            TreeNode node = new TreeNode(childObjectTypes[index].Name);
            node.Tag = (object) childObjectTypes[index];
            if (childObjectTypes[index].Icon != null && childObjectTypes[index].Icon.Length != 0)
            {
              using (MemoryStream memoryStream = new MemoryStream(childObjectTypes[index].Icon))
                this._imageList.Images.Add(new Icon((Stream) memoryStream));
              node.ImageIndex = node.SelectedImageIndex = this._imageList.Images.Count - 1;
            }
            nodes.Add(node);
            if (new Guid(childObjectTypes[index].GUID) == selectedType)
              treeNode1 = node;
            TreeNode treeNode2 = this.AddNodes(node.Nodes, selectedType, childObjectTypes[index].ID);
            if (treeNode2 != null)
              treeNode1 = treeNode2;
          }
        }
      }
    }
    return treeNode1;
  }

  private void BuildTree(Guid selectedType)
  {
    this.treeView1.Nodes.Clear();
    TreeNode treeNode = this.AddNodes(this.treeView1.Nodes, selectedType, -1);
    if (treeNode != null)
    {
      this.treeView1.SelectedNode = treeNode;
      this.treeView1.Select();
      for (TreeNode parent = treeNode.Parent; parent != null; parent = parent.Parent)
        parent.Expand();
    }
    this.RefreshControls();
  }

  private void treeView1_AfterSelect(object sender, TreeViewEventArgs e) => this.RefreshControls();

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (PublishTypeSelectForm));
    this.panel1 = new Panel();
    this.bOK = new Button();
    this.bCancel = new Button();
    this.treeView1 = new TreeView();
    this.panel1.SuspendLayout();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Controls.Add((Control) this.bOK);
    this.panel1.Controls.Add((Control) this.bCancel);
    this.panel1.Name = "panel1";
    componentResourceManager.ApplyResources((object) this.bOK, "bOK");
    this.bOK.DialogResult = DialogResult.OK;
    this.bOK.Name = "bOK";
    this.bOK.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.bCancel, "bCancel");
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Name = "bCancel";
    this.bCancel.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.treeView1, "treeView1");
    this.treeView1.Name = "treeView1";
    this.treeView1.AfterSelect += new TreeViewEventHandler(this.treeView1_AfterSelect);
    this.AcceptButton = (IButtonControl) this.bOK;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.bCancel;
    this.Controls.Add((Control) this.treeView1);
    this.Controls.Add((Control) this.panel1);
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (PublishTypeSelectForm);
    this.panel1.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
