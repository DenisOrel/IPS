// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ImbaseMeasuresSettings.ImbaseMeasureSelectDialog
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Imbase.Properties;
using Intermech.ImpExp.Interface.CommonData;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Imbase.ImbaseMeasuresSettings;

internal class ImbaseMeasureSelectDialog : Form
{
  private IDataWriter idw;
  private IContainer components;
  private GroupBox groupBox1;
  private SplitContainer splitContainer1;
  private TableLayoutPanel tableLayoutPanel1;
  private Button buttonAccept;
  private Button buttonCancel;
  private TreeView treeViewPV;
  private PropertyGrid propertyGrid;
  private ContextMenuStrip contextMenuStrip1;
  private ToolStripMenuItem miNewPhysValue;
  private ToolStripMenuItem miNewMeasure;
  private ToolStrip toolStrip1;
  private ToolStripDropDownButton toolStripDropDownButton1;
  private ToolStripMenuItem создатьToolStripMenuItem;
  private ToolStripMenuItem создатьЕдиницуИзмеренияToolStripMenuItem;
  private ImageList imageList1;

  public long SelectedMeasureID
  {
    get
    {
      TreeNode selectedNode = this.treeViewPV.SelectedNode;
      return selectedNode != null && selectedNode.Tag is ImbaseMeasureSelectDialog.localMeasutreItem ? (selectedNode.Tag as ImbaseMeasureSelectDialog.localMeasutreItem).ID : 0L;
    }
    set
    {
      TreeNode treeNodeByMeasureId = this.getTreeNodeByMeasureId(this.treeViewPV.Nodes, value);
      if (treeNodeByMeasureId == null)
        return;
      this.treeViewPV.SelectedNode = treeNodeByMeasureId;
      treeNodeByMeasureId.EnsureVisible();
    }
  }

  private void miNewPhysValue_Click(object sender, EventArgs e)
  {
    NewPhysValueForm newPhysValueForm = new NewPhysValueForm();
    if (newPhysValueForm.ShowDialog() != DialogResult.OK)
      return;
    this.RefreshTree();
    foreach (TreeNode node in this.treeViewPV.Nodes)
    {
      if (node.Tag is ImbaseMeasureSelectDialog.localPhysicalValueItem && (node.Tag as ImbaseMeasureSelectDialog.localPhysicalValueItem).ID == newPhysValueForm.newPhysicalValueID)
      {
        this.treeViewPV.SelectedNode = node;
        break;
      }
    }
  }

  private void miNewMeasure_Click(object sender, EventArgs e)
  {
    long currentValue = 0;
    TreeNode selectedNode = this.treeViewPV.SelectedNode;
    if (selectedNode != null)
    {
      if (selectedNode.Tag is ImbaseMeasureSelectDialog.localMeasutreItem)
      {
        TreeNode parent = selectedNode.Parent;
        if (parent != null)
          currentValue = (parent.Tag as ImbaseMeasureSelectDialog.localPhysicalValueItem).ID;
      }
      else if (selectedNode.Tag is ImbaseMeasureSelectDialog.localPhysicalValueItem)
        currentValue = (selectedNode.Tag as ImbaseMeasureSelectDialog.localPhysicalValueItem).ID;
    }
    NewMeasureDialog newMeasureDialog = new NewMeasureDialog();
    newMeasureDialog.LoadData(currentValue);
    if (newMeasureDialog.ShowDialog() != DialogResult.OK || newMeasureDialog.NewMeasureID == 0L)
      return;
    this.RefreshTree();
    this.SelectedMeasureID = newMeasureDialog.NewMeasureID;
  }

  public ImbaseMeasureSelectDialog(IDataWriter dataWriter)
  {
    this.InitializeComponent();
    this.idw = dataWriter;
    this.RefreshTree();
  }

  private void RefreshTree()
  {
    this.treeViewPV.Nodes.Clear();
    if (ServicesManager.GetService(typeof (IPhysicalValues)) is IPhysicalValues service)
    {
      foreach (IPhysicalValueItem allPhysicalValue in service.GetAllPhysicalValues())
      {
        TreeNode node = new TreeNode();
        node.Text = allPhysicalValue.Name;
        node.Tag = (object) new ImbaseMeasureSelectDialog.localPhysicalValueItem(allPhysicalValue);
        node.StateImageIndex = 0;
        foreach (IMeasureItem measure in (IEnumerable<IMeasureItem>) ((IDictionary<long, IMeasureItem>) allPhysicalValue.Measures).Values)
          node.Nodes.Add(new TreeNode()
          {
            Text = $"{measure.LongName} ({measure.ShortName})",
            Tag = (object) new ImbaseMeasureSelectDialog.localMeasutreItem(measure),
            StateImageIndex = 1
          });
        this.treeViewPV.Nodes.Add(node);
      }
    }
    this.treeViewPV.TreeViewNodeSorter = (IComparer) new ImbaseMeasureSelectDialog.NodeSorter();
  }

  private TreeNode getTreeNodeByMeasureId(TreeNodeCollection nodes, long measureID)
  {
    foreach (TreeNode node in nodes)
    {
      if (node.Tag is ImbaseMeasureSelectDialog.localMeasutreItem && (node.Tag as ImbaseMeasureSelectDialog.localMeasutreItem).ID == measureID)
        return node;
      if (node.Nodes.Count > 0)
      {
        TreeNode treeNodeByMeasureId = this.getTreeNodeByMeasureId(node.Nodes, measureID);
        if (treeNodeByMeasureId != null)
          return treeNodeByMeasureId;
      }
    }
    return (TreeNode) null;
  }

  private void treeViewPV_AfterSelect(object sender, TreeViewEventArgs e)
  {
    TreeNode selectedNode = this.treeViewPV.SelectedNode;
    this.propertyGrid.SelectedObject = selectedNode?.Tag;
    this.buttonAccept.Enabled = selectedNode != null && selectedNode.Tag is ImbaseMeasureSelectDialog.localMeasutreItem;
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ImbaseMeasureSelectDialog));
    this.splitContainer1 = new SplitContainer();
    this.toolStrip1 = new ToolStrip();
    this.toolStripDropDownButton1 = new ToolStripDropDownButton();
    this.создатьToolStripMenuItem = new ToolStripMenuItem();
    this.создатьЕдиницуИзмеренияToolStripMenuItem = new ToolStripMenuItem();
    this.treeViewPV = new TreeView();
    this.contextMenuStrip1 = new ContextMenuStrip(this.components);
    this.miNewPhysValue = new ToolStripMenuItem();
    this.miNewMeasure = new ToolStripMenuItem();
    this.imageList1 = new ImageList(this.components);
    this.propertyGrid = new PropertyGrid();
    this.groupBox1 = new GroupBox();
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this.buttonCancel = new Button();
    this.buttonAccept = new Button();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.Panel2.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    this.toolStrip1.SuspendLayout();
    this.contextMenuStrip1.SuspendLayout();
    this.groupBox1.SuspendLayout();
    this.tableLayoutPanel1.SuspendLayout();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.splitContainer1, "splitContainer1");
    this.splitContainer1.Name = "splitContainer1";
    this.splitContainer1.Panel1.Controls.Add((Control) this.treeViewPV);
    this.splitContainer1.Panel1.Controls.Add((Control) this.toolStrip1);
    this.splitContainer1.Panel2.Controls.Add((Control) this.propertyGrid);
    this.toolStrip1.Items.AddRange(new ToolStripItem[1]
    {
      (ToolStripItem) this.toolStripDropDownButton1
    });
    componentResourceManager.ApplyResources((object) this.toolStrip1, "toolStrip1");
    this.toolStrip1.Name = "toolStrip1";
    this.toolStripDropDownButton1.DropDownItems.AddRange(new ToolStripItem[2]
    {
      (ToolStripItem) this.создатьToolStripMenuItem,
      (ToolStripItem) this.создатьЕдиницуИзмеренияToolStripMenuItem
    });
    componentResourceManager.ApplyResources((object) this.toolStripDropDownButton1, "toolStripDropDownButton1");
    this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
    this.создатьToolStripMenuItem.Image = (Image) Resources.Физические_величины;
    this.создатьToolStripMenuItem.Name = "создатьToolStripMenuItem";
    componentResourceManager.ApplyResources((object) this.создатьToolStripMenuItem, "создатьToolStripMenuItem");
    this.создатьToolStripMenuItem.Click += new EventHandler(this.miNewPhysValue_Click);
    this.создатьЕдиницуИзмеренияToolStripMenuItem.Image = (Image) Resources.Единицы_измерения;
    this.создатьЕдиницуИзмеренияToolStripMenuItem.Name = "создатьЕдиницуИзмеренияToolStripMenuItem";
    componentResourceManager.ApplyResources((object) this.создатьЕдиницуИзмеренияToolStripMenuItem, "создатьЕдиницуИзмеренияToolStripMenuItem");
    this.создатьЕдиницуИзмеренияToolStripMenuItem.Click += new EventHandler(this.miNewMeasure_Click);
    this.treeViewPV.ContextMenuStrip = this.contextMenuStrip1;
    componentResourceManager.ApplyResources((object) this.treeViewPV, "treeViewPV");
    this.treeViewPV.FullRowSelect = true;
    this.treeViewPV.HideSelection = false;
    this.treeViewPV.Name = "treeViewPV";
    this.treeViewPV.StateImageList = this.imageList1;
    this.treeViewPV.AfterSelect += new TreeViewEventHandler(this.treeViewPV_AfterSelect);
    this.contextMenuStrip1.Items.AddRange(new ToolStripItem[2]
    {
      (ToolStripItem) this.miNewPhysValue,
      (ToolStripItem) this.miNewMeasure
    });
    this.contextMenuStrip1.Name = "contextMenuStrip1";
    componentResourceManager.ApplyResources((object) this.contextMenuStrip1, "contextMenuStrip1");
    this.miNewPhysValue.Image = (Image) Resources.Физические_величины;
    this.miNewPhysValue.Name = "miNewPhysValue";
    componentResourceManager.ApplyResources((object) this.miNewPhysValue, "miNewPhysValue");
    this.miNewPhysValue.Click += new EventHandler(this.miNewPhysValue_Click);
    this.miNewMeasure.Image = (Image) Resources.Единицы_измерения;
    this.miNewMeasure.Name = "miNewMeasure";
    componentResourceManager.ApplyResources((object) this.miNewMeasure, "miNewMeasure");
    this.miNewMeasure.Click += new EventHandler(this.miNewMeasure_Click);
    this.imageList1.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList1.ImageStream");
    this.imageList1.TransparentColor = Color.Transparent;
    this.imageList1.Images.SetKeyName(0, "Физические величины.ico");
    this.imageList1.Images.SetKeyName(1, "Единицы измерения.ico");
    componentResourceManager.ApplyResources((object) this.propertyGrid, "propertyGrid");
    this.propertyGrid.Name = "propertyGrid";
    this.tableLayoutPanel1.SetColumnSpan((Control) this.groupBox1, 3);
    this.groupBox1.Controls.Add((Control) this.splitContainer1);
    componentResourceManager.ApplyResources((object) this.groupBox1, "groupBox1");
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.TabStop = false;
    componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
    this.tableLayoutPanel1.Controls.Add((Control) this.groupBox1, 0, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this.buttonCancel, 2, 1);
    this.tableLayoutPanel1.Controls.Add((Control) this.buttonAccept, 1, 1);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.buttonCancel.DialogResult = DialogResult.Cancel;
    componentResourceManager.ApplyResources((object) this.buttonCancel, "buttonCancel");
    this.buttonCancel.Name = "buttonCancel";
    this.buttonCancel.UseVisualStyleBackColor = true;
    this.buttonAccept.DialogResult = DialogResult.OK;
    componentResourceManager.ApplyResources((object) this.buttonAccept, "buttonAccept");
    this.buttonAccept.Name = "buttonAccept";
    this.buttonAccept.UseVisualStyleBackColor = true;
    this.AcceptButton = (IButtonControl) this.buttonAccept;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.buttonCancel;
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.Name = nameof (ImbaseMeasureSelectDialog);
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.Panel1.PerformLayout();
    this.splitContainer1.Panel2.ResumeLayout(false);
    this.splitContainer1.ResumeLayout(false);
    this.toolStrip1.ResumeLayout(false);
    this.toolStrip1.PerformLayout();
    this.contextMenuStrip1.ResumeLayout(false);
    this.groupBox1.ResumeLayout(false);
    this.tableLayoutPanel1.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  private class NodeSorter : IComparer
  {
    public int Compare(object x, object y)
    {
      return string.Compare((x as TreeNode).Text, (y as TreeNode).Text);
    }
  }

  private class localPhysicalValueItem
  {
    protected long id;
    protected string name = string.Empty;

    [DisplayName("Идентификатор")]
    public long ID => this.id;

    [DisplayName("Наименование")]
    public string Name => this.name;

    public localPhysicalValueItem(IPhysicalValueItem physicalValue)
    {
      this.id = physicalValue.Id;
      this.name = physicalValue.Name;
    }
  }

  internal class localMeasutreItem
  {
    private long id;
    private double koef;
    private string longName = string.Empty;
    private string shortName = string.Empty;

    [DisplayName("Идентификатор")]
    public long ID => this.id;

    [DisplayName("Коэффициент приведения")]
    public double Koef => this.koef;

    [DisplayName("Наименование")]
    public string LongName => this.longName;

    [DisplayName("Краткое имя")]
    public string ShortName => this.shortName;

    public localMeasutreItem(IMeasureItem measure)
    {
      this.id = measure.Id;
      this.koef = measure.Koef;
      this.longName = measure.LongName;
      this.shortName = measure.ShortName;
    }
  }
}
