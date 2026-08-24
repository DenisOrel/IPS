// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.IMHProfilesViewCtrl
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Imbase;
using Intermech.Imbase.Controls;
using Intermech.Interfaces;
using Intermech.Interfaces.Imbase;
using Intermech.Interfaces.MaterialsHandbook;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class IMHProfilesViewCtrl : IMHViewCtrl
{
  private TreeBuilder _treeBuilder = new TreeBuilder();
  private DataTable _imbaseTableTree;
  private long _folderID;
  private string _assortmentFolderClassifKey;
  private IContainer components;
  private TreeView _trv;

  public IMHProfilesViewCtrl()
  {
    this.InitializeComponent();
    this.CustomizeMenu();
    this._assortmentFolderClassifKey = IMHHelper.GetClassifFolderKey("ASSORTMENT_FOLDER_NAME");
    this._treeBuilder.TreeView = this._trv;
    this._treeBuilder.Selected += new SelectEventHandler(this.On_treeBuilder_Selected);
  }

  private void On_treeBuilder_Selected(object sender, TreeViewSelectEventArgs e)
  {
    long num = 0;
    TreeNode treeNode = this._trv.SelectedNode;
    if (treeNode != null)
    {
      while (treeNode.Parent != null)
      {
        treeNode = treeNode.Parent;
        treeNode.Expand();
      }
    }
    if (this._lock)
      return;
    if (this._trv.SelectedNode?.Tag is Intermech.Imbase.Controls.NodeInfo tag && tag.IsTableReference)
      num = tag.ObjectId;
    this.AssortmentTableRefID(num, -1L, true);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject objectActualCopy = sessionKeeper.Session.GetObjectActualCopy(num, false);
      if (objectActualCopy == null || !(sessionKeeper.Session.GetCustomService(typeof (IIMHSystemSettingsService)) is IIMHSystemSettingsService customService))
        return;
      Guid objectGuidByName = customService.GetObjectGuidByName("BASE_MATERIAL_ATTR");
      if (!(objectGuidByName != Guid.Empty))
        return;
      IDBAttribute attributeByGuid = objectActualCopy.GetAttributeByGuid(objectGuidByName);
      if (attributeByGuid == null)
        return;
      string asString = attributeByGuid.AsString;
      ImbaseHelper.TryParseRecordReference(sessionKeeper.Session, asString, out this._mTableRefID, out this._mRecID);
    }
  }

  public override void Initialize(
    ISelectedItems items,
    IServiceProvider provider,
    NavigatorTreeNode parentINode)
  {
    this.ClearData();
    base.Initialize(items, provider, parentINode);
    if (!(items.GetItemData(0, typeof (FolderNode)) is FolderNode itemData))
      return;
    this._folderID = itemData.FolderID;
    this._aTableRefID = itemData.SelectedAssortmentTableRefID;
    this._aRecID = itemData.SelectedAssortmentRecID;
    itemData.SelectedAssortmentTableRefID = 0L;
    itemData.SelectedAssortmentRecID = -1L;
  }

  public override void Activate(IView previousView)
  {
    this.LoadData();
    base.Activate(previousView);
  }

  protected new void ClearData()
  {
    base.ClearData();
    this._imbaseTableTree = (DataTable) null;
    this._trv.Nodes.Clear();
    this.On_treeBuilder_Selected((object) null, (TreeViewSelectEventArgs) null);
    this._pnlFormula.Invalidate();
  }

  protected override void FavouritesClick(object sender, EventArgs e)
  {
    base.FavouritesClick(sender, e);
    this.ViewFavourites(0L, 0L, -1L, string.Empty);
  }

  protected override void AddFavouriteClick(object sender, EventArgs e)
  {
    base.AddFavouriteClick(sender, e);
    this.ViewFavourites(this._folderID, this._aTableRefID, this._aRecID, this._aCaption);
  }

  protected override void SearchClick(object sender, EventArgs e)
  {
    using (ProfileSearchForm profileSearchForm = new ProfileSearchForm())
    {
      int num = (int) profileSearchForm.ShowDialog();
      if (profileSearchForm.FolderID == 0L || this._treeView == null)
        return;
      this.GoToNode(profileSearchForm.FolderID, profileSearchForm.FolderID, 0L);
    }
  }

  protected override void RestoreSelection(
    long mTableRefID,
    long mRecID,
    long aTableRefID,
    long aRecID)
  {
    base.RestoreSelection(mTableRefID, mRecID, aTableRefID, aRecID);
    if (this._treeBuilder == null || aTableRefID == 0L)
      return;
    if (!this._treeBuilder.NodeCache.ContainsKey(aTableRefID))
    {
      foreach (TreeNode treeNode in this._treeBuilder.NodeCache.Values.ToArray<TreeNode>())
      {
        if (!treeNode.IsExpanded)
        {
          treeNode.Expand();
          if (this._treeBuilder.NodeCache.ContainsKey(aTableRefID))
          {
            this._trv.SelectedNode = this._treeBuilder.NodeCache[aTableRefID];
            this.AssortmentTableRefID(aTableRefID, aRecID, false);
            break;
          }
        }
      }
    }
    else
    {
      this._trv.SelectedNode = this._treeBuilder.NodeCache[aTableRefID];
      this.AssortmentTableRefID(aTableRefID, aRecID, false);
    }
  }

  protected override void FormulaPaint(PaintEventArgs e, string text)
  {
    base.FormulaPaint(e, this._formulaText);
  }

  private void CustomizeMenu()
  {
    this._tsBtnSearch.ToolTipText = LocalizationHolder.rm.GetString("IMH_SearchProfileNode_Caption");
    this._tsBtnCoatingProperties.Visible = this._tsBtnGluedMaterials.Visible = this._tsBtnMaterialSubstitutes.Visible = this._tsBtnSort.Visible = this._tsBtnApplicabilityFilter.Visible = false;
  }

  private void CorrectProfiles(List<long> objIDs, Dictionary<long, TreeNode> dict)
  {
    if (dict == null || dict.Count <= 0)
      return;
    foreach (KeyValuePair<long, TreeNode> keyValuePair in dict)
    {
      if (objIDs.Contains(keyValuePair.Key))
        keyValuePair.Value.Collapse();
    }
  }

  private DataTable GetProfiles()
  {
    DataTable profiles = (DataTable) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObjectCollection objectCollection = sessionKeeper.Session.GetObjectCollection(Intermech.Imbase.Consts.ImbaseFolderTypeGUID);
      if (objectCollection != null)
      {
        DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[2]
        {
          new ConditionStructure(Intermech.Imbase.Consts.ClassifFolderKeyAttId, RelationalOperators.StartString, (object) this._assortmentFolderClassifKey, LogicalOperators.AND, 0, false),
          new ConditionStructure(Intermech.Imbase.Consts.BlankCodeAttrID, RelationalOperators.Equal, (object) this._folderID, LogicalOperators.NONE, 0, false)
        }, new object[1]
        {
          (object) ObligatoryObjectAttributes.F_OBJECT_ID
        })
        {
          Contents = new ColumnContents[1]
          {
            ColumnContents.ID
          }
        };
        profiles = objectCollection.Select(paramSet);
        if (profiles != null)
        {
          if (profiles.Rows.Count == 0)
            profiles = (DataTable) null;
        }
      }
    }
    return profiles;
  }

  private void LoadData()
  {
    DataTable profiles = this.GetProfiles();
    if (profiles != null)
    {
      List<long> objIDs = new List<long>(profiles.Rows.Count);
      foreach (DataRow row in (InternalDataCollectionBase) profiles.Rows)
      {
        long int64 = Convert.ToInt64(row[0]);
        if (!objIDs.Contains(int64))
          objIDs.Add(int64);
      }
      if (objIDs.Count == 0)
        objIDs.Add(0L);
      this._trv.BeginUpdate();
      try
      {
        Dictionary<long, TreeNode> dictionary = new Dictionary<long, TreeNode>();
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          if (sessionKeeper.Session.GetCustomService(typeof (IImbaseServer)) is IImbaseServer customService)
          {
            this._imbaseTableTree = customService.GetFoldersForObjects(sessionKeeper.Session.SessionGUID, objIDs.ToArray(), (long[]) null);
            this._treeBuilder.CreateTree(this._imbaseTableTree, (IDictionary<long, TreeNode>) dictionary);
          }
        }
        this.CorrectProfiles(objIDs, dictionary);
        TreeNode treeNode = this.SearchNode(this._aTableRefID);
        if (treeNode == null)
          return;
        this._lock = true;
        this._trv.SelectedNode = treeNode;
        this._lock = false;
        this.AssortmentTableRefID(this._aTableRefID, this._aRecID, true);
      }
      finally
      {
        this._trv.EndUpdate();
      }
    }
    else
      this._trv.Nodes.Clear();
  }

  private TreeNode Search(TreeNodeCollection nodes, List<long> IDs, long id)
  {
    TreeNode treeNode = (TreeNode) null;
    if (nodes != null)
    {
      foreach (TreeNode node in nodes)
      {
        if (node.Tag is Intermech.Imbase.Controls.NodeInfo tag)
        {
          if (tag.ObjectId == id)
          {
            treeNode = node;
            break;
          }
          if (IDs.Contains(tag.ObjectId))
          {
            node.Expand();
            treeNode = this.Search(node.Nodes, IDs, id);
          }
        }
      }
    }
    return treeNode;
  }

  private TreeNode SearchNode(long tableRefID)
  {
    TreeNode treeNode = (TreeNode) null;
    if (tableRefID != 0L)
    {
      DataTable imbaseTableTree = IMHHelper.GetImbaseTableTree(this._aTableRefID);
      if (imbaseTableTree != null && imbaseTableTree.Rows.Count > 0 && imbaseTableTree.Columns.Contains("F_PATH"))
      {
        imbaseTableTree.DefaultView.Sort = "F_PATH";
        DataTable table = imbaseTableTree.DefaultView.ToTable();
        List<long> IDs = new List<long>(table.Rows.Count);
        foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
          IDs.Add(Convert.ToInt64(row["F_OBJECT_ID"]));
        treeNode = this.Search(this._trv.Nodes, IDs, this._aTableRefID);
      }
    }
    return treeNode;
  }

  private void ViewFavourites(long folderID, long tblRefID, long recID, string caption)
  {
    using (MaterialFavourites materialFavourites = new MaterialFavourites(Consts.IMHProfilesNodeGuid, folderID, tblRefID, recID, caption))
    {
      if (materialFavourites.ShowDialog() != DialogResult.OK)
        return;
      FavouriteData data = materialFavourites.Data;
      this.GoToNode(data.FolderID, 0L, -1L, data.TableRefID, data.RecordID);
    }
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing)
      this._treeBuilder?.Dispose();
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this._trv = new TreeView();
    this._splt.BeginInit();
    this._splt.Panel1.SuspendLayout();
    this._splt.Panel2.SuspendLayout();
    this._splt.SuspendLayout();
    this.SuspendLayout();
    this._splt.Panel1.Controls.Add((Control) this._trv);
    this._trv.Dock = DockStyle.Fill;
    this._trv.HideSelection = false;
    this._trv.ItemHeight = 18;
    this._trv.Location = new Point(0, 0);
    this._trv.Name = "_trv";
    this._trv.Size = new Size(299, 515);
    this._trv.TabIndex = 1;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Name = nameof (IMHProfilesViewCtrl);
    this.Controls.SetChildIndex((Control) this._pnlFormula, 0);
    this.Controls.SetChildIndex((Control) this._splt, 0);
    this._splt.Panel1.ResumeLayout(false);
    this._splt.Panel2.ResumeLayout(false);
    this._splt.Panel2.PerformLayout();
    this._splt.EndInit();
    this._splt.ResumeLayout(false);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
