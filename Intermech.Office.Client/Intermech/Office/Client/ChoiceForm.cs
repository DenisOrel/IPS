// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.ChoiceForm
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.PropertyEditors;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

public class ChoiceForm : Form
{
  private IContainer components;
  private TreeView treeView1;
  private Panel panel1;
  private Button bCancel;
  private Button bOK;

  public ChoiceForm() => this.InitializeComponent();

  [CanBeNull]
  public object SelectedValue => this.treeView1.SelectedNode?.Tag;

  [NotNull]
  public string SelectedCaption => this.treeView1.SelectedNode?.Text ?? string.Empty;

  public void Init([NotNull] string text, [NotNull, ItemNotNull] List<string> captions, [NotNull, ItemNotNull] List<object> values)
  {
    this.treeView1.Nodes.Clear();
    this.treeView1.ImageList = FileAttributeStatics.imageList;
    for (int index = 0; index < captions.Count; ++index)
    {
      TreeNode node = new TreeNode(captions[index]);
      node.Tag = values[index];
      TreeNode treeNode1 = node;
      TreeNode treeNode2 = node;
      string ext = Path.GetExtension(captions[index]) ?? string.Empty.ToLower();
      int extImageIndex;
      int num1 = extImageIndex = FileAttributeStatics.GetExtImageIndex(ext);
      treeNode2.SelectedImageIndex = extImageIndex;
      int num2 = num1;
      treeNode1.ImageIndex = num2;
      this.treeView1.Nodes.Add(node);
    }
    this.RefreshButtons();
  }

  private void treeView1_AfterSelect([CanBeNull] object sender, [NotNull] TreeViewEventArgs e)
  {
    this.RefreshButtons();
  }

  private void RefreshButtons() => this.bOK.Enabled = this.treeView1.SelectedNode != null;

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ChoiceForm));
    this.treeView1 = new TreeView();
    this.panel1 = new Panel();
    this.bCancel = new Button();
    this.bOK = new Button();
    this.panel1.SuspendLayout();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.treeView1, "treeView1");
    this.treeView1.Name = "treeView1";
    this.treeView1.ShowLines = false;
    this.treeView1.ShowPlusMinus = false;
    this.treeView1.ShowRootLines = false;
    this.treeView1.AfterSelect += new TreeViewEventHandler(this.treeView1_AfterSelect);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Controls.Add((Control) this.bCancel);
    this.panel1.Controls.Add((Control) this.bOK);
    this.panel1.Name = "panel1";
    componentResourceManager.ApplyResources((object) this.bCancel, "bCancel");
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Name = "bCancel";
    this.bCancel.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.bOK, "bOK");
    this.bOK.DialogResult = DialogResult.OK;
    this.bOK.Name = "bOK";
    this.bOK.UseVisualStyleBackColor = true;
    this.AcceptButton = (IButtonControl) this.bOK;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.bCancel;
    this.Controls.Add((Control) this.treeView1);
    this.Controls.Add((Control) this.panel1);
    this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
    this.Name = nameof (ChoiceForm);
    this.panel1.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
