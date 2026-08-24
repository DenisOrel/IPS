// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.IMHAssortmentViewCtrl
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Client.Core;
using Intermech.Imbase;
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
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class IMHAssortmentViewCtrl : IMHViewCtrl
{
  private Guid _baseMaterialAttr;
  private IContainer components;
  private ListView _lv;
  private System.Windows.Forms.ColumnHeader _caption;
  private ImageList _il;

  public IMHAssortmentViewCtrl()
  {
    this.InitializeComponent();
    this.CustomizeMenu();
    this._lv.SmallImageList = this._lv.LargeImageList = Statics.IconSrv.ImageList;
    if (ApplicationServices.Container.GetService(typeof (IMServerService)) is IMServerService service)
      this._baseMaterialAttr = service.GetCustomService(typeof (IIMHSystemSettingsService)) is IIMHSystemSettingsService customService ? customService.GetObjectGuidByName("BASE_MATERIAL_ATTR") : Guid.Empty;
    this._categoryNodeGuid = Consts.IMHAssortmentNodeGuid;
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
        if (!this._lock)
          this.AssortmentTableRefID(tag.A_TableID, -1L, false);
      }
    }
    else
    {
      this._mTableRefID = 0L;
      this._mRecID = -1L;
      this._mCaption = string.Empty;
    }
    this._formulaText = this._mCaption;
    this._pnlFormula.Invalidate();
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

  private void _lv_MouseDoubleClick(object sender, MouseEventArgs e)
  {
    if (this._lv.SelectedItems.Count == 0 || this._services == null || !(this._services.GetService(typeof (ISelectionWindow)) is ISelectionWindow service))
      return;
    service.OkButton.PerformClick();
  }

  public override void Activate(IView previousView)
  {
    base.Activate(previousView);
    this.SubcribeEvents();
    if (this._lv.Items.Count <= 0 || this._lv.SelectedItems.Count != 0)
      return;
    this._lv.Items[0].Selected = true;
  }

  public override void Deactivate(IView nextView)
  {
    this.UnsubscribeEvents();
    base.Deactivate(nextView);
  }

  public override void Initialize(
    ISelectedItems items,
    IServiceProvider provider,
    NavigatorTreeNode parentINode)
  {
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
    DataTable dt;
    if (items.GetItemID(0) is StandartFolderNodeID itemId)
    {
      dt = this.GetStandart(itemId.Standart);
      this._tsBtnFavourites.Visible = this._tsBtnAddFavourite.Visible = this._tsSeparator1.Visible = false;
      this._cmFavourites.Visible = this._cmAddFavourite.Visible = this._cmSeparator1.Visible = false;
      this._isStandart = true;
    }
    else
    {
      dt = this.LoadMaterialsData(itemData.FolderID);
      this._tsBtnFavourites.Visible = this._tsBtnAddFavourite.Visible = this._tsSeparator1.Visible = true;
      this._cmFavourites.Visible = this._cmAddFavourite.Visible = this._cmSeparator1.Visible = true;
      this._isStandart = false;
    }
    this.CreateItems(dt);
  }

  protected new void ClearData()
  {
    base.ClearData();
    this._lock = true;
    this._lv.Items.Clear();
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

  protected override void FavouritesClick(object sender, EventArgs e)
  {
    base.FavouritesClick(sender, e);
    this.ViewFavourites(0L, -1L, string.Empty, this._isMaterial);
  }

  protected override void AddFavouriteClick(object sender, EventArgs e)
  {
    base.AddFavouriteClick(sender, e);
    if (this._isMaterial)
      this.ViewFavourites(this._aTableRefID, -1L, this._mCaption, true);
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
      using (MaterialSearchForm materialSearchForm = new MaterialSearchForm(false))
      {
        if (materialSearchForm.ShowDialog() != DialogResult.OK)
          return;
        this.GoToNode(materialSearchForm.aTableRefID, materialSearchForm.TableRefID, materialSearchForm.RecID, materialSearchForm.aTableRefID);
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

  protected override void FormulaPaint(PaintEventArgs e, string text)
  {
    base.FormulaPaint(e, this._formulaText);
  }

  private void CustomizeMenu()
  {
    this._lv.ContextMenuStrip = this._contextMenuBase;
    this._tsBtnSearch.ToolTipText = this._cmSearch.Text = LocalizationHolder.rm.GetString("IMH_Search_Assortment_Caption");
    this._tsBtnCoatingProperties.Visible = this._tsBtnGluedMaterials.Visible = this._tsBtnMaterialSubstitutes.Visible = this._tsBtnApplicabilityFilter.Visible = false;
    this._cmCoatingProperties.Visible = this._cmGluedMaterials.Visible = this._cmMaterialSubstitutes.Visible = this._cmApplicabilityFilter.Visible = false;
  }

  private void CreateItems(DataTable dt)
  {
    if (dt != null)
    {
      List<string> keyValues = new List<string>(dt.Rows.Count);
      Dictionary<string, List<long>> dictionary1 = new Dictionary<string, List<long>>(dt.Rows.Count);
      foreach (DataRow row in (InternalDataCollectionBase) dt.Rows)
      {
        string key = Convert.ToString(row["KEY"]);
        if (!keyValues.Contains(key))
          keyValues.Add(key);
        if (dictionary1.ContainsKey(key))
          dictionary1[key].Add(Convert.ToInt64(row["OBJECT_ID"]));
        else
          dictionary1[key] = new List<long>((IEnumerable<long>) new long[1]
          {
            Convert.ToInt64(row["OBJECT_ID"])
          });
      }
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        if (sessionKeeper.Session.GetCustomService(typeof (IImbaseServer)) is IImbaseServer customService)
        {
          Dictionary<string, Tuple<string, bool>> dictionary2 = customService.NameRecordReferencesWithApplicability(sessionKeeper.Session.SessionGUID, keyValues);
          if (dictionary2 != null)
          {
            if (dictionary2.Count > 0)
            {
              int imageIndex = Statics.IconSrv.IndexOf(1, Consts.MaterialObjTypeID);
              foreach (KeyValuePair<string, Tuple<string, bool>> keyValuePair in dictionary2)
              {
                List<long> longList = dictionary1[keyValuePair.Key];
                long linkId;
                long recordId;
                ImbaseHelper.TryParseRecordReference(sessionKeeper.Session, keyValuePair.Key, out linkId, out recordId);
                foreach (long aTableID in longList)
                {
                  ListViewItem listViewItem = new ListViewItem(keyValuePair.Value.Item1, imageIndex)
                  {
                    Tag = (object) new LvItem(aTableID, linkId, (long) Convert.ToInt32(recordId), keyValuePair.Value.Item1, keyValuePair.Value.Item2),
                    ForeColor = keyValuePair.Value.Item2 ? SystemColors.ControlText : SystemColors.GrayText
                  };
                  this._lv.Items.Add(listViewItem);
                  if (linkId == this._mTableRefID && recordId == this._mRecID && aTableID == this._aTableRefID)
                  {
                    listViewItem.Selected = true;
                    this.AssortmentTableRefID(this._aTableRefID, this._aRecID, true);
                  }
                }
              }
            }
          }
        }
      }
    }
    this._tsBtnSort.Visible = this._lv.Items.Count > 0;
  }

  private DataTable GetStandart(string standart)
  {
    DataTable standart1 = (DataTable) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObjectCollection objectCollection = sessionKeeper.Session.GetObjectCollection(Intermech.Imbase.Consts.ImbaseTableRefTypeID);
      if (objectCollection != null)
      {
        if (this._baseMaterialAttr != Guid.Empty)
        {
          int attributeTypeId = MetaDataHelper.GetAttributeTypeID(this._baseMaterialAttr);
          string classifFolderKey = IMHHelper.GetClassifFolderKey("ASSORTMENT_FOLDER_NAME");
          DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[3]
          {
            new ConditionStructure(Intermech.Imbase.Consts.ClassifFolderKeyAttId, RelationalOperators.StartString, (object) classifFolderKey, LogicalOperators.AND, 0, false),
            new ConditionStructure(Intermech.Imbase.Consts.StandartAssortmentAttrID, RelationalOperators.Equal, (object) standart, LogicalOperators.AND, 0, false),
            new ConditionStructure(attributeTypeId, RelationalOperators.NotEmpty, (object) null, LogicalOperators.NONE, 0, false)
          }, new object[2]
          {
            (object) ObligatoryObjectAttributes.F_OBJECT_ID,
            (object) attributeTypeId
          })
          {
            Contents = new ColumnContents[2]
            {
              ColumnContents.ID,
              ColumnContents.String
            }
          };
          standart1 = objectCollection.Select(paramSet);
          if (standart1 != null)
          {
            standart1.Columns[0].ColumnName = "OBJECT_ID";
            standart1.Columns[1].ColumnName = "KEY";
          }
        }
      }
    }
    return standart1;
  }

  private DataTable LoadMaterialsData(long folderID)
  {
    DataTable dataTable = (DataTable) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObjectCollection objectCollection = sessionKeeper.Session.GetObjectCollection(Intermech.Imbase.Consts.ImbaseTableRefTypeGUID);
      if (objectCollection != null)
      {
        if (this._baseMaterialAttr != Guid.Empty)
        {
          int attributeTypeId = MetaDataHelper.GetAttributeTypeID(this._baseMaterialAttr);
          DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[2]
          {
            new ConditionStructure((string) null, RelationalOperators.EntersIn, (object) folderID, LogicalOperators.AND, 0, false),
            new ConditionStructure(attributeTypeId, RelationalOperators.NotEmpty, (object) null, LogicalOperators.NONE, 0, false)
          }, new object[2]
          {
            (object) ObligatoryObjectAttributes.F_OBJECT_ID,
            (object) attributeTypeId
          })
          {
            Contents = new ColumnContents[2]
            {
              ColumnContents.ID,
              ColumnContents.String
            }
          };
          dataTable = objectCollection.Select(paramSet);
          if (dataTable != null)
          {
            dataTable.Columns[0].ColumnName = "OBJECT_ID";
            dataTable.Columns[1].ColumnName = "KEY";
          }
        }
      }
    }
    return dataTable;
  }

  private NavigatorTreeNode SearchAssortmentFolderNode(string standartText)
  {
    if (this._treeView == null)
      return (NavigatorTreeNode) null;
    NavigatorTreeNode lastNode;
    return !this._treeView.TryFind(this._parentNodePath, out lastNode) ? (NavigatorTreeNode) null : this.SearchStandart(lastNode, standartText);
  }

  private NavigatorTreeNode SearchMaterialFolderNode(string standartText)
  {
    NavigatorTreeNode navigatorTreeNode = (NavigatorTreeNode) null;
    if (this._treeView == null)
      return (NavigatorTreeNode) null;
    NavigatorTreeNode lastNode;
    if (!this._treeView.TryFind(this._parentNodePath, out lastNode))
      lastNode = (NavigatorTreeNode) null;
    NavigatorTreeNode parent = lastNode?.Parent;
    if (parent == null)
      return (NavigatorTreeNode) null;
    foreach (NavigatorTreeNode child in (List<NavigatorTreeNode>) parent.Children)
    {
      if (!child.Equals((object) lastNode) && child.NodeID.CategoryID == Consts.IMHMaterialsNodeCategoryID)
      {
        this._treeView.PopulateNode(child);
        navigatorTreeNode = this.SearchStandart(child, standartText);
        if (navigatorTreeNode != null)
          break;
      }
    }
    return navigatorTreeNode;
  }

  private NavigatorTreeNode SearchStandart(NavigatorTreeNode node, string standartText)
  {
    NavigatorTreeNode navigatorTreeNode1 = (NavigatorTreeNode) null;
    NavigatorTreeNodes children = node?.Children;
    if (children == null)
      return (NavigatorTreeNode) null;
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
    using (MaterialFavourites materialFavourites = new MaterialFavourites(Consts.IMHAssortmentNodeGuid, tblRefID, recID, caption, isMaterial))
    {
      if (materialFavourites.ShowDialog() != DialogResult.OK)
        return;
      FavouriteData data = materialFavourites.Data;
      long linkId = 0;
      long recordId1 = -1;
      long tableRefId = data.TableRefID;
      long recordId2 = data.RecordID;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject objectActualCopy = sessionKeeper.Session.GetObjectActualCopy(tableRefId, false);
        if (objectActualCopy != null)
        {
          if (this._baseMaterialAttr != Guid.Empty)
          {
            IDBAttribute attributeByGuid = objectActualCopy.GetAttributeByGuid(this._baseMaterialAttr);
            if (attributeByGuid != null)
            {
              string asString = attributeByGuid.AsString;
              ImbaseHelper.TryParseRecordReference(sessionKeeper.Session, asString, out linkId, out recordId1);
            }
          }
        }
      }
      this.GoToNode(tableRefId, linkId, recordId1, tableRefId, recordId2);
    }
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
        this.AssortmentTableRefID(tag1.A_TableID, aRecID, true);
        flag = false;
      }
      else
        this._lv.SelectedItems[0].Selected = false;
    }
    if (!flag)
      return;
    foreach (ListViewItem listViewItem in this._lv.Items)
    {
      if (!(listViewItem.Tag is LvItem tag2) || tag2.RecID == mRecID && tag2.M_TableID == mTableRefID)
      {
        this._lock = true;
        listViewItem.Selected = true;
        this._lock = false;
        this.AssortmentTableRefID(aTableRefID, aRecID, false);
        break;
      }
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
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    this._lv = new ListView();
    this._caption = new System.Windows.Forms.ColumnHeader();
    this._il = new ImageList(this.components);
    this._splt.BeginInit();
    this._splt.Panel1.SuspendLayout();
    this._splt.Panel2.SuspendLayout();
    this._splt.SuspendLayout();
    this.SuspendLayout();
    this._splt.Panel1.Controls.Add((Control) this._lv);
    this._lv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[1]
    {
      this._caption
    });
    this._lv.Dock = DockStyle.Fill;
    this._lv.FullRowSelect = true;
    this._lv.HeaderStyle = ColumnHeaderStyle.None;
    this._lv.HideSelection = false;
    this._lv.Location = new Point(0, 0);
    this._lv.MultiSelect = false;
    this._lv.Name = "_lv";
    this._lv.Size = new Size(299, 515);
    this._lv.TabIndex = 0;
    this._lv.UseCompatibleStateImageBehavior = false;
    this._lv.View = View.Details;
    this._lv.SizeChanged += new EventHandler(this.On_lv_SizeChanged);
    this._lv.Enter += new EventHandler(this.On_lv_Enter);
    this._lv.MouseDoubleClick += new MouseEventHandler(this._lv_MouseDoubleClick);
    this._caption.Width = 294;
    this._il.ColorDepth = ColorDepth.Depth8Bit;
    this._il.ImageSize = new Size(16 /*0x10*/, 16 /*0x10*/);
    this._il.TransparentColor = Color.Transparent;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Name = nameof (IMHAssortmentViewCtrl);
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
