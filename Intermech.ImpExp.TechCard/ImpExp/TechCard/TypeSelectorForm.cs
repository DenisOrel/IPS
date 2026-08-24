// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TypeSelectorForm
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Advanced;
using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard;

public class TypeSelectorForm : Form, ISelectorForm, IDisposable
{
  private IContainer components;
  private Panel panel1;
  private Button bCancel;
  private Button bOk;
  protected TreeView tvType;
  private TextBox tbxType;
  private Panel panel2;
  private Panel panel3;
  protected Label lblInfo;

  private void InitializeData()
  {
    this.tvType.TreeViewNodeSorter = (IComparer) new TreeNodeComparer();
    this.FormClosed += new FormClosedEventHandler(this.TypeSelectorForm_FormClosed);
    this.Load += new EventHandler(this.TypeSelectorForm_Load);
    this.bOk.Enabled = false;
  }

  protected virtual void SetParams(object[] data, string caption)
  {
    if (caption == null)
      return;
    this.Text = caption;
  }

  protected TreeNode FindNode(TreeNodeCollection nodes, string text)
  {
    if (nodes == null || nodes.Count == 0)
      return (TreeNode) null;
    TreeNode node1 = (TreeNode) null;
    foreach (TreeNode node2 in nodes)
    {
      if (node2.Text.StartsWith(text, StringComparison.CurrentCultureIgnoreCase))
      {
        node1 = node2;
        break;
      }
      node1 = this.FindNode(node2.Nodes, text);
      if (node1 != null)
        break;
    }
    return node1;
  }

  protected void LoadData()
  {
    this.LoadTypesTree();
    this.tvType.Sort();
  }

  protected virtual void LoadTypesTree()
  {
  }

  public TypeSelectorForm()
    : this((object[]) null)
  {
  }

  public TypeSelectorForm(object[] data)
    : this(data, (string) null)
  {
  }

  public TypeSelectorForm(object[] data, string caption)
  {
    this.InitializeComponent();
    if (this.DesignMode)
      return;
    this.SetParams(data, caption);
    this.InitializeData();
    this.LoadData();
  }

  private void tbxType_TextChanged(object sender, EventArgs e)
  {
    this.tvType.AfterSelect -= new TreeViewEventHandler(this.tvType_AfterSelect);
    try
    {
      this.tvType.SelectedNode = this.FindNode(this.tvType.Nodes, this.tbxType.Text);
      this.bOk.Enabled = this.tvType.SelectedNode != null;
    }
    finally
    {
      this.tvType.AfterSelect += new TreeViewEventHandler(this.tvType_AfterSelect);
    }
  }

  private void tvType_AfterSelect(object sender, TreeViewEventArgs e)
  {
    this.bOk.Enabled = e.Node != null;
    this.tbxType.TextChanged -= new EventHandler(this.tbxType_TextChanged);
    try
    {
      if (e.Node == null)
        return;
      this.tbxType.Text = e.Node.Text;
    }
    finally
    {
      this.tbxType.TextChanged += new EventHandler(this.tbxType_TextChanged);
    }
  }

  private void tvType_MouseDoubleClick(object sender, MouseEventArgs e)
  {
    TreeNode selectedNode = this.tvType.SelectedNode;
    if (selectedNode == null || selectedNode.Nodes.Count != 0)
      return;
    this.DialogResult = DialogResult.OK;
  }

  private void TypeSelectorForm_Load(object sender, EventArgs e)
  {
    FormStorageEx.LoadSettings((Control) this);
  }

  private void TypeSelectorForm_FormClosed(object sender, FormClosedEventArgs e)
  {
    FormStorageEx.SaveSettings((Control) this);
  }

  public virtual object SelectedItem
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (TypeSelectorForm));
    this.panel1 = new Panel();
    this.bCancel = new Button();
    this.bOk = new Button();
    this.tvType = new TreeView();
    this.lblInfo = new Label();
    this.tbxType = new TextBox();
    this.panel2 = new Panel();
    this.panel3 = new Panel();
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.panel3.SuspendLayout();
    this.SuspendLayout();
    this.panel1.Controls.Add((Control) this.bCancel);
    this.panel1.Controls.Add((Control) this.bOk);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    componentResourceManager.ApplyResources((object) this.bCancel, "bCancel");
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Name = "bCancel";
    this.bCancel.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.bOk, "bOk");
    this.bOk.DialogResult = DialogResult.OK;
    this.bOk.Name = "bOk";
    this.bOk.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.tvType, "tvType");
    this.tvType.HideSelection = false;
    this.tvType.Name = "tvType";
    this.tvType.MouseDoubleClick += new MouseEventHandler(this.tvType_MouseDoubleClick);
    this.tvType.AfterSelect += new TreeViewEventHandler(this.tvType_AfterSelect);
    componentResourceManager.ApplyResources((object) this.lblInfo, "lblInfo");
    this.lblInfo.Name = "lblInfo";
    componentResourceManager.ApplyResources((object) this.tbxType, "tbxType");
    this.tbxType.Name = "tbxType";
    this.tbxType.TextChanged += new EventHandler(this.tbxType_TextChanged);
    this.panel2.Controls.Add((Control) this.tvType);
    componentResourceManager.ApplyResources((object) this.panel2, "panel2");
    this.panel2.Name = "panel2";
    this.panel3.Controls.Add((Control) this.tbxType);
    this.panel3.Controls.Add((Control) this.lblInfo);
    componentResourceManager.ApplyResources((object) this.panel3, "panel3");
    this.panel3.Name = "panel3";
    this.AcceptButton = (IButtonControl) this.bOk;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.bCancel;
    this.Controls.Add((Control) this.panel2);
    this.Controls.Add((Control) this.panel1);
    this.Controls.Add((Control) this.panel3);
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (TypeSelectorForm);
    this.panel1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.panel3.ResumeLayout(false);
    this.panel3.PerformLayout();
    this.ResumeLayout(false);
  }

  DialogResult ISelectorForm.ShowDialog() => this.ShowDialog();
}
