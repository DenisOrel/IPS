// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.SelectRank
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using Intermech.Client.Core;
using Intermech.Controls;
using Intermech.DataFormats;
using Intermech.Interfaces.Client;
using Intermech.Signs.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Signs.Client;

public class SelectRank : Form
{
  private Panel _bottom;
  private Panel _buttons;
  private Button _bApply;
  private Button _bCancel;
  private CheckedTreeView _Box;
  private IContainer components;
  private ImageList imageList;
  private GraphsSet hintGraphSet;

  public SelectRank(
    List<long> rankIDs,
    List<IDBTypedObjectID> typedObjectIDs,
    SignsCard card,
    GraphsSet hintGraphSet)
  {
    this.hintGraphSet = hintGraphSet;
    this.InitializeComponent();
    HelpProvidersClass.SetHelpOptionForControl((Control) this, 1267);
    this.LoadList(rankIDs, typedObjectIDs, card);
  }

  private void LoadList(List<long> rankIDs, List<IDBTypedObjectID> typedObjectIDs, SignsCard card)
  {
    this._Box.BeginUpdate();
    try
    {
      this._Box.Nodes.Clear();
      foreach (long rankId in rankIDs)
      {
        UserRankInformation userRankInformation = new UserRankInformation(rankId);
        TreeNode node1 = new TreeNode(userRankInformation.RankCaption);
        this._Box.Nodes.Add(node1);
        List<string> graphs = card.GetGraphs(rankId, typedObjectIDs);
        List<string[]> strArrayList = new List<string[]>();
        foreach (string key in graphs)
        {
          if (SignsCache.PossibleGraphs.ContainsKey(key))
            strArrayList.Add(new string[2]
            {
              SignsCache.PossibleGraphs[key],
              key
            });
        }
        strArrayList.Sort((IComparer<string[]>) new GraphComparer());
        foreach (string[] strArray in strArrayList)
        {
          GraphTreeNode node2 = new GraphTreeNode(strArray[0], strArray[1]);
          node2.Tag = (object) userRankInformation;
          if (this.hintGraphSet != null)
          {
            foreach (GraphsCollection graphsCollection in (IEnumerable) this.hintGraphSet.Values)
            {
              bool flag = false;
              foreach (GraphClass graphClass in graphsCollection)
              {
                if (graphClass.Value.Equals(strArray[1]))
                {
                  node2.ImageIndex = 1;
                  flag = true;
                  break;
                }
              }
              if (flag)
                break;
            }
          }
          node1.Nodes.Add((TreeNode) node2);
        }
      }
    }
    finally
    {
      this._Box.EndUpdate();
    }
    this._Box.ExpandAll();
  }

  public ArrayList SelectedItems
  {
    get
    {
      ArrayList list = new ArrayList();
      foreach (TreeNode node in this._Box.Nodes)
        this.GetSelectedItems(list, node);
      return list;
    }
  }

  private void GetSelectedItems(ArrayList list, TreeNode node)
  {
    if (node != null && node.Tag is UserRankInformation && node.Checked)
    {
      bool flag = false;
      UserRankInformation userRankInformation1 = (UserRankInformation) null;
      UserRankInformation tag = node.Tag as UserRankInformation;
      foreach (UserRankInformation userRankInformation2 in list)
      {
        if (userRankInformation2.Equals((object) tag))
        {
          userRankInformation1 = userRankInformation2;
          flag = true;
          break;
        }
      }
      if (!flag)
        userRankInformation1 = UserRankInformation.Clone(tag);
      string str = string.Empty;
      if (node is GraphTreeNode)
        str = ((GraphTreeNode) node).GraphID;
      if (!userRankInformation1.Graphs.Contains(str) && !str.Equals(string.Empty))
        userRankInformation1.Graphs.Add(str);
      if (!flag)
        list.Add((object) userRankInformation1);
    }
    if (node == null)
      return;
    foreach (TreeNode node1 in node.Nodes)
      this.GetSelectedItems(list, node1);
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SelectRank));
    this._bottom = new Panel();
    this._buttons = new Panel();
    this._bCancel = new Button();
    this._bApply = new Button();
    this.imageList = new ImageList(this.components);
    this._Box = new CheckedTreeView();
    this._bottom.SuspendLayout();
    this._buttons.SuspendLayout();
    this.SuspendLayout();
    this._bottom.Controls.Add((Control) this._buttons);
    componentResourceManager.ApplyResources((object) this._bottom, "_bottom");
    this._bottom.Name = "_bottom";
    this._buttons.Controls.Add((Control) this._bCancel);
    this._buttons.Controls.Add((Control) this._bApply);
    componentResourceManager.ApplyResources((object) this._buttons, "_buttons");
    this._buttons.Name = "_buttons";
    this._bCancel.DialogResult = DialogResult.Cancel;
    componentResourceManager.ApplyResources((object) this._bCancel, "_bCancel");
    this._bCancel.Name = "_bCancel";
    this._bApply.DialogResult = DialogResult.OK;
    componentResourceManager.ApplyResources((object) this._bApply, "_bApply");
    this._bApply.Name = "_bApply";
    this.imageList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList.ImageStream");
    this.imageList.TransparentColor = Color.Fuchsia;
    this.imageList.Images.SetKeyName(0, "ArrowNo.bmp");
    this.imageList.Images.SetKeyName(1, "Arrow.bmp");
    this._Box.CheckBoxes = true;
    componentResourceManager.ApplyResources((object) this._Box, "_Box");
    this._Box.ImageList = this.imageList;
    this._Box.Name = "_Box";
    this._Box.BeforeCheck += new TreeViewCancelEventHandler(this._Box_BeforeCheck);
    this._Box.AfterCheck += new TreeViewEventHandler(this._Box_AfterCheck);
    this._Box.AfterSelect += new TreeViewEventHandler(this._Box_AfterSelect);
    this.AcceptButton = (IButtonControl) this._bApply;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.CancelButton = (IButtonControl) this._bCancel;
    this.Controls.Add((Control) this._Box);
    this.Controls.Add((Control) this._bottom);
    this.HelpButton = true;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (SelectRank);
    this.ShowInTaskbar = false;
    this.Closed += new EventHandler(this.SelectRank_Closed);
    this.Load += new EventHandler(this.SelectRank_Load);
    this._bottom.ResumeLayout(false);
    this._buttons.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  private void SelectRank_Load(object sender, EventArgs e)
  {
    FormStorage.LoadLayout((Control) this);
  }

  private void SelectRank_Closed(object sender, EventArgs e)
  {
    FormStorage.SaveLayout((Control) this);
  }

  private void _Box_BeforeCheck(object sender, TreeViewCancelEventArgs e)
  {
    if (e.Node.Tag is UserRankInformation)
      return;
    e.Cancel = true;
  }

  private void _Box_AfterCheck(object sender, TreeViewEventArgs e)
  {
    if (e.Node.Tag is UserRankInformation)
      return;
    foreach (TreeNode node in e.Node.Nodes)
      node.Checked = e.Node.Checked;
  }

  private void _Box_AfterSelect(object sender, TreeViewEventArgs e)
  {
    this.UpdateNodeStatus(e.Node);
  }

  private void UpdateNodeStatus(TreeNode tn) => tn.SelectedImageIndex = tn.ImageIndex;
}
