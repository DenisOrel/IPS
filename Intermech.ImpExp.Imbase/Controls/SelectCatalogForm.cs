// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.Controls.SelectCatalogForm
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Imbase.ItemFactories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Imbase.Controls;

public class SelectCatalogForm : Form
{
  private IContainer components;
  private Panel panel1;
  private Button bCancel;
  private Button bOK;
  private Panel panel2;
  private TreeView treeView1;
  private ImageList imageList1;

  public SelectCatalogForm() => this.InitializeComponent();

  internal void AddCatalogs(Dictionary<Guid, CatalogPres> catalogsPres)
  {
    foreach (CatalogPres catalogPres in catalogsPres.Values)
    {
      TreeNode node = new TreeNode(catalogPres.Name);
      switch (catalogPres.Type)
      {
        case ImTablesType.IMTT_CATALOG:
          this.treeView1.Nodes["NodeCatalogs"].Nodes.Add(node);
          break;
        case ImTablesType.IMTT_CTLREF:
          this.treeView1.Nodes["NodeCtlRefs"].Nodes.Add(node);
          break;
        case ImTablesType.IMTT_TECHREF:
          this.treeView1.Nodes["NodeTechRefs"].Nodes.Add(node);
          break;
      }
      node.StateImageIndex = 1;
      node.Tag = (object) catalogPres;
    }
  }

  internal CatalogPres SelectedCatalog
  {
    get
    {
      TreeNode selectedNode = this.treeView1.SelectedNode;
      return selectedNode != null && selectedNode.Tag != null ? selectedNode.Tag as CatalogPres : (CatalogPres) null;
    }
  }

  private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
  {
    this.bOK.Enabled = e.Node.Tag != null;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    TreeNode treeNode1 = new TreeNode("Каталоги");
    TreeNode treeNode2 = new TreeNode("Справочники");
    TreeNode treeNode3 = new TreeNode("Технологические справочники");
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SelectCatalogForm));
    this.panel1 = new Panel();
    this.bCancel = new Button();
    this.bOK = new Button();
    this.panel2 = new Panel();
    this.treeView1 = new TreeView();
    this.imageList1 = new ImageList(this.components);
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.SuspendLayout();
    this.panel1.Controls.Add((Control) this.bCancel);
    this.panel1.Controls.Add((Control) this.bOK);
    this.panel1.Dock = DockStyle.Bottom;
    this.panel1.Location = new Point(0, 344);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(357, 36);
    this.panel1.TabIndex = 0;
    this.bCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Location = new Point(270, 7);
    this.bCancel.Name = "bCancel";
    this.bCancel.Size = new Size(75, 23);
    this.bCancel.TabIndex = 1;
    this.bCancel.Text = "Отмена";
    this.bCancel.UseVisualStyleBackColor = true;
    this.bOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bOK.DialogResult = DialogResult.OK;
    this.bOK.Enabled = false;
    this.bOK.Location = new Point(189, 7);
    this.bOK.Name = "bOK";
    this.bOK.Size = new Size(75, 23);
    this.bOK.TabIndex = 0;
    this.bOK.Text = "ОК";
    this.bOK.UseVisualStyleBackColor = true;
    this.panel2.Controls.Add((Control) this.treeView1);
    this.panel2.Dock = DockStyle.Fill;
    this.panel2.Location = new Point(0, 0);
    this.panel2.Name = "panel2";
    this.panel2.Size = new Size(357, 344);
    this.panel2.TabIndex = 1;
    this.treeView1.Dock = DockStyle.Fill;
    this.treeView1.Location = new Point(0, 0);
    this.treeView1.Name = "treeView1";
    treeNode1.Name = "NodeCatalogs";
    treeNode1.StateImageIndex = 0;
    treeNode1.Text = "Каталоги";
    treeNode2.Name = "NodeCtlRefs";
    treeNode2.StateImageIndex = 0;
    treeNode2.Text = "Справочники";
    treeNode3.Name = "NodeTechRefs";
    treeNode3.StateImageIndex = 0;
    treeNode3.Text = "Технологические справочники";
    this.treeView1.Nodes.AddRange(new TreeNode[3]
    {
      treeNode1,
      treeNode2,
      treeNode3
    });
    this.treeView1.Size = new Size(357, 344);
    this.treeView1.StateImageList = this.imageList1;
    this.treeView1.TabIndex = 0;
    this.treeView1.AfterSelect += new TreeViewEventHandler(this.treeView1_AfterSelect);
    this.imageList1.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList1.ImageStream");
    this.imageList1.TransparentColor = Color.Transparent;
    this.imageList1.Images.SetKeyName(0, "folder2.ico");
    this.imageList1.Images.SetKeyName(1, "catalogs2.ico");
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(357, 380);
    this.Controls.Add((Control) this.panel2);
    this.Controls.Add((Control) this.panel1);
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (SelectCatalogForm);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Выбор каталога";
    this.panel1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
