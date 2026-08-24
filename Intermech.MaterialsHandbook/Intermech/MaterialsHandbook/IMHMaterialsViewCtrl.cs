// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.IMHMaterialsViewCtrl
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Client.Core;
using Intermech.Imbase;
using Intermech.Imbase.Controls;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
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
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class IMHMaterialsViewCtrl : IMHViewCtrl
{
  private TreeBuilder _treeBuilder = new TreeBuilder();
  private DataTable _imbaseTableTree;
  private List<long> _tableRefIDs;
  private int _indexAssortmentCategory;
  private Guid _baseMaterialAttr;
  private bool _activated;
  private List<ListViewItem> _items = new List<ListViewItem>();
  private IContainer components;
  private TreeView _trv;
  private ListView _lv;
  private System.Windows.Forms.ColumnHeader _colCaption;
  private SplitContainer _spltAssortment;

  public IMHMaterialsViewCtrl()
  {
    this.InitializeComponent();
    this.CustomizeMenu();
    this._indexAssortmentCategory = TreeBuilder.GetIconIndex(Consts.IMHAssortmentNodeCategoryID, -1);
    this._treeBuilder.TreeView = this._trv;
    this._treeBuilder.Selected += new SelectEventHandler(this.On_treeBuilder_Selected);
    this._baseMaterialAttr = (ApplicationServices.Container.GetService(typeof (IMServerService)) is IMServerService service ? service.GetCustomService(typeof (IIMHSystemSettingsService)) : (object) null) is IIMHSystemSettingsService customService ? customService.GetObjectGuidByName("BASE_MATERIAL_ATTR") : Guid.Empty;
    this._categoryNodeGuid = Consts.IMHMaterialsNodeGuid;
  }

  private void On_lv_Enter(object sender, EventArgs e)
  {
    this._isMaterial = true;
    this._formulaText = this._mCaption;
    this._pnlFormula.Invalidate();
    bool selectable = true;
    if (this._lv.SelectedItems.Count > 0)
      selectable = ((LvItem) this._lv.SelectedItems[0].Tag).Selectable;
    this.OnIMHMaterialChanged(this._mTableRefID, this._mRecID, selectable);
  }

  private void On_lv_SelectedIndexChanged(object sender, EventArgs e)
  {
    bool selectable = false;
    if (this._lv.SelectedItems.Count > 0)
    {
      if (this._lv.SelectedItems[0].Tag is LvItem tag)
      {
        this._mTableRefID = tag.M_TableID;
        this._mRecID = tag.RecID;
        this._mCaption = tag.Caption;
        selectable = tag.Selectable;
      }
    }
    else
    {
      this._mTableRefID = 0L;
      this._mRecID = -1L;
      this._mCaption = string.Empty;
      this._trv.SelectedNode = (TreeNode) null;
      this.On_treeBuilder_Selected((object) null, (TreeViewSelectEventArgs) null);
    }
    this._formulaText = this._mCaption;
    this._pnlFormula.Invalidate();
    this.LoadAssortment();
    this.OnIMHMaterialChanged(this._mTableRefID, this._mRecID, selectable);
  }

  private void On_lv_SizeChanged(object sender, EventArgs e)
  {
    if (this._lock || this._lv == null || this._lv.Columns.Count == 0 || this._lv.Columns[0] == null)
      return;
    this._lock = true;
    this._lv.Columns[0].Width = -2;
    this._lock = false;
  }

  private void On_trv_Enter(object sender, EventArgs e) => this._isMaterial = false;

  private void On_treeBuilder_Selected(object sender, TreeViewSelectEventArgs e)
  {
    if (this._lock)
      return;
    long tableRefID = 0;
    if (this._trv.SelectedNode?.Tag is Intermech.Imbase.Controls.NodeInfo tag && tag.IsTableReference)
      tableRefID = tag.ObjectId;
    if (tableRefID == 0L)
      this._aRecID = -1L;
    this.AssortmentTableRefID(tableRefID, -1L, true);
    this.ClearFilter();
  }

  private void _lv_MouseDoubleClick(object sender, MouseEventArgs e)
  {
    if (this._lv.SelectedItems.Count == 0 || this._services == null || !(this._services.GetService(typeof (ISelectionWindow)) is ISelectionWindow service))
      return;
    service.OkButton.PerformClick();
  }

  public override void Initialize(
    ISelectedItems items,
    IServiceProvider provider,
    NavigatorTreeNode parentINode)
  {
    this._activated = false;
    this.ClearData();
    base.Initialize(items, provider, parentINode);
    this._lv.Sorting = SortOrder.Ascending;
    if (!(items?.GetItemData(0, typeof (FolderNode)) is FolderNode itemData))
      return;
    this._mTableRefID = itemData.SelectedMaterialTableRefID;
    this._mRecID = itemData.SelectedMaterialRecID;
    this._aTableRefID = itemData.SelectedAssortmentTableRefID;
    this._aRecID = itemData.SelectedAssortmentRecID;
    itemData.SelectedMaterialTableRefID = 0L;
    itemData.SelectedMaterialRecID = -1L;
    itemData.SelectedAssortmentTableRefID = 0L;
    itemData.SelectedAssortmentRecID = -1L;
    if (items.GetItemID(0) is StandartFolderNodeID itemId)
    {
      this._tableRefIDs = this.GetStandart(itemId.Standart);
      this._tsBtnMaterialSubstitutes.Visible = this._tsBtnFavourites.Visible = this._tsBtnAddFavourite.Visible = this._tsSeparator1.Visible = false;
      this._cmMaterialSubstitutes.Visible = this._cmFavourites.Visible = this._cmAddFavourite.Visible = this._cmSeparator1.Visible = false;
      this._isStandart = true;
    }
    else
    {
      this._tableRefIDs = itemData.TableRefIDs;
      this._tsBtnMaterialSubstitutes.Visible = this._tsBtnFavourites.Visible = this._tsBtnAddFavourite.Visible = this._tsSeparator1.Visible = true;
      this._cmMaterialSubstitutes.Visible = this._cmFavourites.Visible = this._cmAddFavourite.Visible = this._cmSeparator1.Visible = true;
      this._isStandart = false;
    }
  }

  public override void Activate(IView previousView)
  {
    base.Activate(previousView);
    this.SubcribeEvents();
    if (previousView != PageViewsManager.BlackHoleView && !this._activated)
    {
      this._lv.SmallImageList = this._lv.LargeImageList = Statics.IconSrv.ImageList;
      this.LoadMaterials();
      if (this._lv.SelectedItems.Count == 0 && this._lv.Items.Count > 0)
        this._lv.Items[0].Selected = this._lv.Items.Count > 0;
      this._pnlFormula.Invalidate();
      this._activated = true;
    }
    if (this._isMaterial)
      this.OnIMHMaterialChanged(this._mTableRefID, this._mRecID);
    else
      this.OnIMHMaterialChanged(this._aTableRefID, this._aRecID);
  }

  public override void Deactivate(IView nextView)
  {
    this.UnsubscribeEvents();
    base.Deactivate(nextView);
  }

  protected new void ClearData()
  {
    base.ClearData();
    this._imbaseTableTree = (DataTable) null;
    this._trv.Nodes.Clear();
    this.On_treeBuilder_Selected((object) null, (TreeViewSelectEventArgs) null);
    this._lock = true;
    this._lv.Items.Clear();
    this._items.Clear();
    this._pnlFormula.Invalidate();
    this._lock = false;
  }

  protected override void TreeListViewEnter(EventArgs e)
  {
    this._isMaterial = false;
    this._formulaText = this._aCaption;
    this._pnlFormula.Invalidate();
    base.TreeListViewEnter(e);
  }

  protected override void SubstitutesClick(object sender, EventArgs e)
  {
    if (this._mTableRefID == 0L || this._mRecID <= -1L || this._treeView == null)
      return;
    using (MaterialSubstitutes materialSubstitutes = new MaterialSubstitutes(ImbaseHelper.MakeInternalImbaseKey(this._mTableRefID, this._mRecID), this._mCaption))
    {
      if (materialSubstitutes.ShowDialog() != DialogResult.OK)
        return;
      this.GoToNode(materialSubstitutes.TableRefID, materialSubstitutes.TableRefID, materialSubstitutes.RecID);
    }
  }

  protected override void FavouritesClick(object sender, EventArgs e)
  {
    base.FavouritesClick(sender, e);
    this.ViewFavourites(0L, -1L, string.Empty, this._isMaterial);
  }

  protected override void AddFavouriteClick(object sender, EventArgs e)
  {
    base.AddFavouriteClick(sender, e);
    if (this._isMaterial)
      this.ViewFavourites(this._mTableRefID, this._mRecID, this._mCaption, true);
    else
      this.ViewFavourites(this._aTableRefID, this._aRecID, this._aCaption, false);
  }

  protected override void SearchClick(object sender, EventArgs e)
  {
    if (this._isStandart)
    {
      using (StandartSearchForm standartSearchForm = new StandartSearchForm())
      {
        if (standartSearchForm.ShowDialog() != DialogResult.OK)
          return;
        string standartText = standartSearchForm.StandartText;
        NavigatorTreeNode folderNode = standartSearchForm.IsMaterial ? this.SearchMaterialFolderNode(standartText) : this.SearchAssortmentFolderNode(standartText);
        if (!folderNode.HasFocus)
        {
          this.SaveIdsForRestore(standartSearchForm.TableRefID, standartSearchForm.RecID, standartSearchForm.aTableRefID, -1L);
          this.BrowseNode(folderNode);
        }
        else
          this.RestoreSelection(standartSearchForm.TableRefID, standartSearchForm.RecID, standartSearchForm.aTableRefID, -1L);
      }
    }
    else
    {
      using (MaterialSearchForm materialSearchForm = new MaterialSearchForm(true))
      {
        if (materialSearchForm.ShowDialog() != DialogResult.OK)
          return;
        this.GoToNode(materialSearchForm.TableRefID, materialSearchForm.TableRefID, materialSearchForm.RecID, materialSearchForm.aTableRefID);
      }
    }
  }

  protected override void SortClick(object sender, EventArgs e)
  {
    base.SortClick(sender, e);
    this._lv.Sorting = this._lv.Sorting == SortOrder.Descending ? SortOrder.Ascending : SortOrder.Descending;
    if (this._lv.SelectedItems.Count <= 0)
      return;
    this._lv.SelectedItems[0].EnsureVisible();
  }

  protected override void ApplicabilityFilterClick(object sender, EventArgs e)
  {
    this._tsBtnApplicabilityFilter.Checked = !this._tsBtnApplicabilityFilter.Checked;
    this.CheckApplicabilityFilterState();
  }

  protected override void FormulaPaint(PaintEventArgs e, string text)
  {
    base.FormulaPaint(e, this._formulaText);
  }

  protected override void RestoreSelection(
    long mTableRefID,
    long mRecID,
    long aTableRefID,
    long aRecID)
  {
    base.RestoreSelection(mTableRefID, mRecID, aTableRefID, aRecID);
    bool flag = true;
    if (this._lv.SelectedItems.Count > 0)
    {
      if (this._lv.SelectedItems[0].Tag is LvItem tag1 && tag1.M_TableID == mTableRefID && tag1.RecID == mRecID)
      {
        if (this._treeBuilder.NodeCache != null && this._treeBuilder.NodeCache.ContainsKey(aTableRefID))
        {
          this._lock = true;
          this._trv.SelectedNode = this._treeBuilder.NodeCache[aTableRefID];
          this._lock = false;
          this.AssortmentTableRefID(aTableRefID, aRecID, true);
          flag = false;
        }
      }
      else
        this._lv.SelectedItems[0].Selected = false;
    }
    if (!flag)
      return;
    this._aTableRefID = aTableRefID;
    foreach (ListViewItem listViewItem in this._lv.Items)
    {
      if (!(listViewItem.Tag is LvItem tag2) || tag2.RecID == mRecID && tag2.M_TableID == mTableRefID)
      {
        this._lock = true;
        listViewItem.Selected = true;
        listViewItem.EnsureVisible();
        this._lock = false;
        this.AssortmentTableRefID(aTableRefID, aRecID, true);
        break;
      }
    }
  }

  private void CustomizeMenu()
  {
    this._lv.ContextMenuStrip = this._contextMenuBase;
    this._tsBtnSearch.ToolTipText = this._cmSearch.Text = LocalizationHolder.rm.GetString("IMH_Search_Material_Caption");
    this._tsBtnCoatingProperties.Visible = this._tsBtnGluedMaterials.Visible = false;
    this._cmCoatingProperties.Visible = this._cmGluedMaterials.Visible = false;
  }

  private void CheckApplicabilityFilterState()
  {
    List<ListViewItem> listViewItemList;
    if (this._tsBtnApplicabilityFilter.Checked)
    {
      this._tsBtnApplicabilityFilter.ImageIndex = this._tsBtnApplicabilityFilter.Owner.ImageList.Images.IndexOfKey("Filter_On.png");
      listViewItemList = this._items.Where<ListViewItem>((System.Func<ListViewItem, bool>) (x => ((LvItem) x.Tag).Selectable)).ToList<ListViewItem>();
    }
    else
    {
      this._tsBtnApplicabilityFilter.ImageIndex = this._tsBtnApplicabilityFilter.Owner.ImageList.Images.IndexOfKey("Filter.png");
      listViewItemList = this._items;
    }
    this._lv.BeginUpdate();
    try
    {
      this._lv.Items.Clear();
      this._lv.Items.AddRange(listViewItemList.ToArray());
    }
    finally
    {
      this._lv.EndUpdate();
    }
  }

  private void CorrectAssortment(Dictionary<long, TreeNode> dict)
  {
    if (dict == null)
      return;
    foreach (KeyValuePair<long, TreeNode> keyValuePair in dict)
    {
      TreeNode node = keyValuePair.Value;
      if (node.Nodes.Count <= 0 && node.Tag is Intermech.Imbase.Controls.NodeInfo tag && tag.TypeId == Intermech.Imbase.Consts.ImbaseTableRefTypeID)
      {
        TreeNode parent1 = node.Parent;
        if (parent1 != null && parent1.Nodes.Count == 1)
        {
          node.Text = parent1.Text;
          if (parent1.Parent != null)
          {
            TreeNode parent2 = parent1.Parent;
            node.Remove();
            parent1.Remove();
            parent2.Nodes.Add(node);
          }
          else
          {
            TreeView treeView = parent1.TreeView;
            node.Remove();
            parent1.Remove();
            treeView.Nodes.Add(node);
          }
        }
        node.ImageIndex = node.SelectedImageIndex = this._indexAssortmentCategory;
      }
    }
  }

  private DataTable GetAssortments()
  {
    DataTable assortments = (DataTable) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObjectCollection objectCollection = sessionKeeper.Session.GetObjectCollection(Intermech.Imbase.Consts.ImbaseTableRefTypeGUID);
      if (objectCollection != null)
      {
        if (this._baseMaterialAttr != Guid.Empty)
        {
          string str = ImbaseHelper.MakeInternalImbaseKey(this._mTableRefID, this._mRecID);
          int attributeTypeId = MetaDataHelper.GetAttributeTypeID(this._baseMaterialAttr);
          string conditionValue = ImbaseHelper.ConvertImbaseKey(sessionKeeper.Session, str);
          DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[2]
          {
            new ConditionStructure(attributeTypeId, RelationalOperators.Equal, (object) str, LogicalOperators.OR, 0, false),
            new ConditionStructure(attributeTypeId, RelationalOperators.Equal, (object) conditionValue, LogicalOperators.NONE, 0, false)
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
          assortments = objectCollection.Select(paramSet);
          if (assortments != null)
          {
            if (assortments.Rows.Count == 0)
              assortments = (DataTable) null;
          }
        }
      }
    }
    return assortments;
  }

  private List<long> GetStandart(string standart)
  {
    List<long> standart1 = (List<long>) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObjectCollection objectCollection = sessionKeeper.Session.GetObjectCollection(Intermech.Imbase.Consts.ImbaseTableRefTypeID);
      if (objectCollection != null)
      {
        string classifFolderKey = IMHHelper.GetClassifFolderKey("BASE_MATERIALS_CTL");
        DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[2]
        {
          new ConditionStructure(Intermech.Imbase.Consts.ClassifFolderKeyAttId, RelationalOperators.StartString, (object) classifFolderKey, LogicalOperators.AND, 0, false),
          new ConditionStructure(Intermech.Imbase.Consts.StandartAttrID, RelationalOperators.Equal, (object) standart, LogicalOperators.NONE, 0, false)
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
        DataTable dataTable = objectCollection.Select(paramSet);
        if (dataTable != null)
        {
          standart1 = dataTable.Rows.Count > 0 ? new List<long>(dataTable.Rows.Count) : (List<long>) null;
          foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
          {
            long int64 = Convert.ToInt64(row[0]);
            if ((standart1 == null || !standart1.Contains(int64)) && standart1 != null)
            {
              // ISSUE: explicit non-virtual call
              __nonvirtual (standart1.Add(int64));
            }
          }
        }
      }
    }
    return standart1;
  }

  private void LoadAssortment()
  {
    DataTable assortments = this.GetAssortments();
    if (assortments != null)
    {
      List<long> longList = new List<long>(assortments.Rows.Count);
      foreach (DataRow row in (InternalDataCollectionBase) assortments.Rows)
        longList.Add(Convert.ToInt64(row[0]));
      if (longList.Count == 0)
        longList.Add(0L);
      this._trv.BeginUpdate();
      try
      {
        Dictionary<long, TreeNode> dictionary = new Dictionary<long, TreeNode>();
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          if (sessionKeeper.Session.GetCustomService(typeof (IImbaseServer)) is IImbaseServer customService)
          {
            this._imbaseTableTree = customService.GetFoldersForObjects(sessionKeeper.Session.SessionGUID, longList.ToArray(), (long[]) null);
            int columnIndex = this._imbaseTableTree.Columns.IndexOf("F_PATH");
            if (columnIndex > -1)
            {
              string classifFolderKey = IMHHelper.GetClassifFolderKey("ASSORTMENT_FOLDER_NAME");
              int index = 0;
              while (index < this._imbaseTableTree.Rows.Count)
              {
                string str = this._imbaseTableTree.Rows[index][columnIndex].ToString();
                if (classifFolderKey.Contains(str))
                  this._imbaseTableTree.Rows.RemoveAt(index);
                else
                  ++index;
              }
            }
            this._treeBuilder.CreateTree(this._imbaseTableTree, (IDictionary<long, TreeNode>) dictionary);
          }
        }
        this.CorrectAssortment(dictionary);
        this._trv.CollapseAll();
        foreach (TreeNode node in this._trv.Nodes)
          node.Expand();
        if (this._aTableRefID == 0L || !dictionary.ContainsKey(this._aTableRefID))
          return;
        this._trv.SelectedNode = dictionary[this._aTableRefID];
      }
      finally
      {
        this._trv.EndUpdate();
      }
    }
    else
      this._trv.Nodes.Clear();
  }

  private void LoadMaterials()
  {
    bool flag1 = false;
    this._items.Clear();
    if (this._tableRefIDs != null)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        if (sessionKeeper.Session.GetCustomService(typeof (IImbaseServer)) is IImbaseServer customService)
        {
          int imageIndex = Statics.IconSrv.IndexOf(1, Consts.MaterialObjTypeID);
          foreach (long tableRefId in this._tableRefIDs)
          {
            DataTable recordsTable;
            customService.LoadRecords(sessionKeeper.Session.SessionGUID, tableRefId, string.Empty, Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator, out recordsTable, out AttributeTypeProperties[] _, out ImbaseKeyInfo _);
            if (recordsTable != null && recordsTable.Rows.Count != 0)
            {
              List<string> keyValues = new List<string>(recordsTable.Rows.Count);
              List<string> stringList = new List<string>(recordsTable.Rows.Count);
              string str1 = Intermech.Imbase.Consts.ImbaseUsingAttID.ToString();
              bool flag2 = recordsTable.Columns.Contains(str1);
              bool flag3 = !flag2 && !this.CheckEnabledItemFromUsingAttr(tableRefId);
              foreach (DataRow row in (InternalDataCollectionBase) recordsTable.Rows)
              {
                string str2 = ImbaseHelper.MakeInternalImbaseKey(tableRefId, Convert.ToInt64(row["-2"]));
                keyValues.Add(str2);
                if (flag2)
                {
                  if (!(Convert.ToString(row[str1]).Trim() != "-"))
                    stringList.Add(str2);
                  else
                    continue;
                }
                else if (!flag3)
                  continue;
                stringList.Add(str2);
              }
              Dictionary<string, string> dictionary = customService.NameRecordReferences(sessionKeeper.Session.SessionGUID, keyValues);
              if (dictionary != null)
              {
                foreach (KeyValuePair<string, string> keyValuePair in dictionary)
                {
                  long linkId;
                  long recordId;
                  ImbaseHelper.TryParseRecordReference(sessionKeeper.Session, keyValuePair.Key, out linkId, out recordId);
                  ListViewItem listViewItem = new ListViewItem(keyValuePair.Value, imageIndex);
                  bool flag4 = stringList.Contains(keyValuePair.Key);
                  listViewItem.Tag = (object) new LvItem(0L, linkId, (long) Convert.ToInt32(recordId), keyValuePair.Value, !flag4);
                  if (flag4)
                  {
                    listViewItem.ForeColor = SystemColors.GrayText;
                    flag1 = true;
                  }
                  this._lv.Items.Add(listViewItem);
                  if (recordId == this._mRecID && linkId == this._mTableRefID)
                  {
                    listViewItem.Selected = true;
                    listViewItem.EnsureVisible();
                  }
                }
              }
            }
          }
        }
      }
    }
    this._tsBtnSort.Visible = this._lv.Items.Count > 0;
    this._tsBtnApplicabilityFilter.Visible = this._cmApplicabilityFilter.Visible = flag1;
    this._items = this._lv.Items.Cast<ListViewItem>().ToList<ListViewItem>();
    this.CheckApplicabilityFilterState();
  }

  private NavigatorTreeNode SearchAssortmentFolderNode(string standartText)
  {
    NavigatorTreeNode navigatorTreeNode = (NavigatorTreeNode) null;
    NavigatorTreeNode lastNode = (NavigatorTreeNode) null;
    if (this._treeView != null && !this._treeView.TryFind(this._parentNodePath, out lastNode))
      lastNode = (NavigatorTreeNode) null;
    NavigatorTreeNode parent = lastNode?.Parent;
    if (parent != null)
    {
      foreach (NavigatorTreeNode child in (List<NavigatorTreeNode>) parent.Children)
      {
        if (!child.Equals((object) lastNode) && child.NodeID.CategoryID == Consts.IMHAssortmentNodeCategoryID)
        {
          this._treeView.PopulateNode(child);
          navigatorTreeNode = this.SearchStandart(child, standartText);
          if (navigatorTreeNode != null)
            break;
        }
      }
    }
    return navigatorTreeNode;
  }

  private NavigatorTreeNode SearchMaterialFolderNode(string standartText)
  {
    if (this._treeView == null)
      return (NavigatorTreeNode) null;
    NavigatorTreeNode lastNode;
    return !this._treeView.TryFind(this._parentNodePath, out lastNode) ? (NavigatorTreeNode) null : this.SearchStandart(lastNode, standartText);
  }

  private NavigatorTreeNode SearchStandart(NavigatorTreeNode node, string standartText)
  {
    NavigatorTreeNodes children = node?.Children;
    if (children == null)
      return (NavigatorTreeNode) null;
    NavigatorTreeNode navigatorTreeNode1 = (NavigatorTreeNode) null;
    foreach (NavigatorTreeNode navigatorTreeNode2 in (List<NavigatorTreeNode>) children)
    {
      if (!(navigatorTreeNode2.GetDisplayText(0) != standartText))
      {
        navigatorTreeNode1 = navigatorTreeNode2;
        break;
      }
    }
    return navigatorTreeNode1;
  }

  private void ViewFavourites(long tblRefID, long recID, string caption, bool isMaterial)
  {
    using (MaterialFavourites materialFavourites = new MaterialFavourites(Consts.IMHMaterialsNodeGuid, tblRefID, recID, caption, isMaterial))
    {
      if (materialFavourites.ShowDialog() != DialogResult.OK)
        return;
      FavouriteData data = materialFavourites.Data;
      long linkId = 0;
      long recordId = -1;
      long num = 0;
      long aRecID = -1;
      if (!materialFavourites.IsMaterial)
      {
        num = data.TableRefID;
        aRecID = data.RecordID;
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          IDBObject objectActualCopy = sessionKeeper.Session.GetObjectActualCopy(num, false);
          if (objectActualCopy != null)
          {
            if (this._baseMaterialAttr != Guid.Empty)
            {
              IDBAttribute attributeByGuid = objectActualCopy.GetAttributeByGuid(this._baseMaterialAttr);
              if (attributeByGuid != null)
              {
                string asString = attributeByGuid.AsString;
                ImbaseHelper.TryParseRecordReference(sessionKeeper.Session, asString, out linkId, out recordId);
              }
            }
          }
        }
      }
      else
      {
        linkId = data.TableRefID;
        recordId = data.RecordID;
      }
      this.GoToNode(linkId, linkId, recordId, num, aRecID);
    }
  }

  private void SubcribeEvents()
  {
    this._lv.SelectedIndexChanged += new EventHandler(this.On_lv_SelectedIndexChanged);
  }

  private void UnsubscribeEvents()
  {
    this._lv.SelectedIndexChanged -= new EventHandler(this.On_lv_SelectedIndexChanged);
  }

  protected override void Dispose(bool disposing)
  {
    this._treeBuilder.Dispose();
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this._trv = new TreeView();
    this._lv = new ListView();
    this._colCaption = new System.Windows.Forms.ColumnHeader();
    this._spltAssortment = new SplitContainer();
    this._splt.BeginInit();
    this._splt.Panel1.SuspendLayout();
    this._splt.Panel2.SuspendLayout();
    this._splt.SuspendLayout();
    this._spltAssortment.BeginInit();
    this._spltAssortment.Panel1.SuspendLayout();
    this._spltAssortment.Panel2.SuspendLayout();
    this._spltAssortment.SuspendLayout();
    this.SuspendLayout();
    this._tlv.Size = new Size(357, 490);
    this._splt.Panel1.Controls.Add((Control) this._spltAssortment);
    this._splt.SplitterDistance = 439;
    this._trv.Dock = DockStyle.Fill;
    this._trv.HideSelection = false;
    this._trv.Location = new Point(0, 0);
    this._trv.Name = "_trv";
    this._trv.Size = new Size(235, 515);
    this._trv.TabIndex = 0;
    this._trv.Enter += new EventHandler(this.On_trv_Enter);
    this._lv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[1]
    {
      this._colCaption
    });
    this._lv.Dock = DockStyle.Fill;
    this._lv.FullRowSelect = true;
    this._lv.HeaderStyle = ColumnHeaderStyle.None;
    this._lv.HideSelection = false;
    this._lv.Location = new Point(0, 0);
    this._lv.MultiSelect = false;
    this._lv.Name = "_lv";
    this._lv.Size = new Size(200, 515);
    this._lv.TabIndex = 1;
    this._lv.UseCompatibleStateImageBehavior = false;
    this._lv.View = View.Details;
    this._lv.SizeChanged += new EventHandler(this.On_lv_SizeChanged);
    this._lv.Enter += new EventHandler(this.On_lv_Enter);
    this._lv.MouseDoubleClick += new MouseEventHandler(this._lv_MouseDoubleClick);
    this._colCaption.Width = 100;
    this._spltAssortment.Dock = DockStyle.Fill;
    this._spltAssortment.Location = new Point(0, 0);
    this._spltAssortment.Name = "_spltAssortment";
    this._spltAssortment.Panel1.Controls.Add((Control) this._lv);
    this._spltAssortment.Panel2.Controls.Add((Control) this._trv);
    this._spltAssortment.Size = new Size(439, 515);
    this._spltAssortment.SplitterDistance = 200;
    this._spltAssortment.TabIndex = 2;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.Name = nameof (IMHMaterialsViewCtrl);
    this.Controls.SetChildIndex((Control) this._pnlFormula, 0);
    this.Controls.SetChildIndex((Control) this._splt, 0);
    this._splt.Panel1.ResumeLayout(false);
    this._splt.Panel2.ResumeLayout(false);
    this._splt.Panel2.PerformLayout();
    this._splt.EndInit();
    this._splt.ResumeLayout(false);
    this._spltAssortment.Panel1.ResumeLayout(false);
    this._spltAssortment.Panel2.ResumeLayout(false);
    this._spltAssortment.EndInit();
    this._spltAssortment.ResumeLayout(false);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
