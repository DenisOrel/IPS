// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PublishTypesSettingsForm
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using ImSSP;
using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.Localization;
using Intermech.Site.Client.Settings;
using Intermech.Site.Client.Settings.TreeNodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client;

public class PublishTypesSettingsForm : Form
{
  private bool _changed;
  private List<Tuple<Guid, string, int>> _publishTypesCache = new List<Tuple<Guid, string, int>>();
  private IContainer components;
  private TreeView treeView1;
  private Button bCancel;
  private PropertyGrid propertyGrid1;
  private Button bApply;
  private SplitContainer splitContainer1;

  public PublishTypesSettingsForm()
  {
    this.InitializeComponent();
    this.Text = SiteClientConsts.PublishTypesSettingsCaption;
    HelpProvidersClass.SetHelpOptionForControl((Control) this, 1732);
  }

  public void RebuildTree()
  {
    this.treeView1.Nodes.Clear();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      this.treeView1.ImageList = (ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService).ImageList;
      TreeNode node1 = new ObjectTypeRootNode().BuildTree(sessionKeeper.Session);
      this.treeView1.Nodes.Add(node1);
      node1.Expand();
      TreeNode node2 = new RelationTypeRootNode().BuildTree(sessionKeeper.Session);
      this.treeView1.Nodes.Add(node2);
      node2.Expand();
    }
    this.treeView1.SelectedNode = this.treeView1.Nodes[0];
  }

  private void SetChanged(bool changed)
  {
    this._changed = changed;
    this.RefreshControls(this.treeView1.SelectedNode);
    if (!(this.treeView1.SelectedNode.Tag is ITypeNode tag))
      return;
    using (new SessionKeeper())
      tag.Redraw(this.treeView1.SelectedNode);
    tag.Changed = changed;
  }

  private void TreeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
  {
    if (e.Node.Nodes == null || e.Node.Nodes.Count != 1 || e.Node.Nodes[0].Tag != null || !(e.Node.Tag is ITypeNode))
      return;
    e.Node.Nodes.Clear();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      TreeNode[] nodes = ((ITypeNode) e.Node.Tag).Expand(sessionKeeper.Session);
      if (nodes == null)
        return;
      e.Node.Nodes.AddRange(nodes);
    }
  }

  private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
  {
    if (e.Node.Tag != null && e.Node.Tag is ITypeNode)
      this.propertyGrid1.SelectedObject = ((ITypeNode) e.Node.Tag).Parameters;
    else
      this.propertyGrid1.SelectedObject = (object) null;
  }

  private void PropertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
  {
    this.SetChanged(true);
  }

  private void RefreshControls(TreeNode node) => this.bApply.Enabled = this._changed;

  private void Cancel_Click(object sender, EventArgs e)
  {
    if (this._changed)
    {
      if (MessageBox.Show("Остались несохраненные данные! В действительно желаете закрыть форму?", this.bCancel.Text, MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      this.Close();
    }
    else
      this.Close();
  }

  private void Apply_Click(object sender, EventArgs e)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetCustomService(typeof (IPublishTypesConfiguration)) is IPublishTypesConfiguration customService1))
        throw new Exception(LocalizationHolder.rm.GetString(sc_18676.ssp_webportal_18677()));
      IDBTransactions customService2 = (IDBTransactions) sessionKeeper.Session.GetCustomService(typeof (IDBTransactions));
      customService2.StartTransaction();
      try
      {
        foreach (TreeNode node in this.treeView1.Nodes)
          (node.Tag as RootTypeNode).SaveTree(sessionKeeper.Session, node);
        customService1.Save();
        this._changed = false;
        this.RebuildTree();
        customService2.Commit();
      }
      catch
      {
        customService2.Rollback();
        throw;
      }
    }
  }

  private void PublishTypesSettingsForm_Shown(object sender, EventArgs e)
  {
    FormStorage.LoadLayout((Control) this);
  }

  private void PublishTypesSettingsForm_FormClosing(object sender, FormClosingEventArgs e)
  {
    FormStorage.SaveLayout((Control) this);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (PublishTypesSettingsForm));
    this.bApply = new Button();
    this.bCancel = new Button();
    this.treeView1 = new TreeView();
    this.propertyGrid1 = new PropertyGrid();
    this.splitContainer1 = new SplitContainer();
    this.splitContainer1.BeginInit();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.Panel2.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.bApply, "bApply");
    this.bApply.Name = "bApply";
    this.bApply.UseVisualStyleBackColor = true;
    this.bApply.Click += new EventHandler(this.Apply_Click);
    componentResourceManager.ApplyResources((object) this.bCancel, "bCancel");
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Name = "bCancel";
    this.bCancel.UseVisualStyleBackColor = true;
    this.bCancel.Click += new EventHandler(this.Cancel_Click);
    componentResourceManager.ApplyResources((object) this.treeView1, "treeView1");
    this.treeView1.HideSelection = false;
    this.treeView1.Name = "treeView1";
    this.treeView1.BeforeExpand += new TreeViewCancelEventHandler(this.TreeView1_BeforeExpand);
    this.treeView1.AfterSelect += new TreeViewEventHandler(this.TreeView1_AfterSelect);
    componentResourceManager.ApplyResources((object) this.propertyGrid1, "propertyGrid1");
    this.propertyGrid1.Name = "propertyGrid1";
    this.propertyGrid1.PropertyValueChanged += new PropertyValueChangedEventHandler(this.PropertyGrid1_PropertyValueChanged);
    componentResourceManager.ApplyResources((object) this.splitContainer1, "splitContainer1");
    this.splitContainer1.Name = "splitContainer1";
    this.splitContainer1.Panel1.Controls.Add((Control) this.treeView1);
    this.splitContainer1.Panel2.Controls.Add((Control) this.propertyGrid1);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.bCancel;
    this.Controls.Add((Control) this.splitContainer1);
    this.Controls.Add((Control) this.bApply);
    this.Controls.Add((Control) this.bCancel);
    this.Name = nameof (PublishTypesSettingsForm);
    this.FormClosing += new FormClosingEventHandler(this.PublishTypesSettingsForm_FormClosing);
    this.Shown += new EventHandler(this.PublishTypesSettingsForm_Shown);
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.Panel2.ResumeLayout(false);
    this.splitContainer1.EndInit();
    this.splitContainer1.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
