// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.IMHDetailsMaterialViewCtrl
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using ImSSP;
using Intermech.Imbase;
using Intermech.Interfaces;
using Intermech.Interfaces.Imbase;
using Intermech.Interfaces.MaterialsHandbook;
using Intermech.Localization;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class IMHDetailsMaterialViewCtrl : IMHViewCtrlBase
{
  private DataTable _dtCoatingsVarieties;
  private string _coatingsColumnGuid = string.Empty;
  private string _materialsColumnGuid = string.Empty;
  private string _purposeColumnGuid = string.Empty;
  private string _instructionsColumnGuid = string.Empty;
  private string _formula = string.Empty;
  private DataTable _dtParams;
  private List<object> _params;
  private string _imbaseKey = string.Empty;
  private string _text = string.Empty;
  private IContainer components;
  private TreeView _trv;
  private ContextMenuStrip _contextMenu;
  private ToolStripMenuItem _miCollapse;
  private ToolStripMenuItem _miExpand;
  private MaterialPropertiesPage _propsPage;

  public IMHDetailsMaterialViewCtrl()
  {
    this.InitializeComponent();
    this.CustomizeMenu();
    this._propsPage.SetRibbonInvisible();
  }

  private void On_miClick(object sender, EventArgs e)
  {
    this._propsPage.ExpandAll((int) Convert.ToInt16(((ToolStripItem) sender).Tag) == sc_14604.ssp_imbase_14605(220145895));
  }

  private void On_trv_AfterSelect(object sender, TreeViewEventArgs e)
  {
    this._propsPage.Clear(false);
    string designation = string.Empty;
    if (this._trv.SelectedNode != null)
    {
      string keyValue = string.Empty;
      if (this._trv.SelectedNode.Parent != null)
      {
        if (this._trv.SelectedNode.Tag is TrvNode tag)
        {
          this._text = tag.Caption;
          designation = tag.Designation;
          keyValue = tag.TrvNodeData;
          Dictionary<string, DataTable> pages = this.GetPages(this._trv.SelectedNode.Parent.Tag as TrvParentNode, tag.Index);
          if (pages != null)
          {
            foreach (KeyValuePair<string, DataTable> keyValuePair in pages)
              this._propsPage.AddPage(keyValuePair.Key, new List<DataTable>((IEnumerable<DataTable>) new DataTable[1]
              {
                keyValuePair.Value
              }), false, false);
          }
        }
      }
      else
      {
        this._text = string.Empty;
        keyValue = this._trv.SelectedNode.Name;
      }
      using (SessionKeeper sessionKeeper = new SessionKeeper())
        ImbaseHelper.TryParseRecordReference(sessionKeeper.Session, keyValue, out this._mTableRefID, out this._mRecID);
    }
    else
    {
      this._text = string.Empty;
      this._mTableRefID = 0L;
      this._mRecID = -1L;
    }
    this._pnlFormula.Invalidate();
    this.OnIMHMaterialChanged(this._mTableRefID, this._mRecID, designation: designation);
  }

  private void _trv_MouseDoubleClick(object sender, MouseEventArgs e)
  {
    if (this._trv.SelectedNode == null || this._services == null || !(this._services.GetService(typeof (ISelectionWindow)) is ISelectionWindow service))
      return;
    service.OkButton.PerformClick();
  }

  public override void Initialize(
    ISelectedItems items,
    IServiceProvider provider,
    NavigatorTreeNode parentINode)
  {
    this.ClearData();
    base.Initialize(items, provider, parentINode);
    if (items == null)
      return;
    if (items.GetItemID(0) is StandartFolderNodeID itemId)
    {
      this._imbaseKey = itemId.ImbaseKey;
      if (items.GetItemData(0, typeof (FolderNode)) is FolderNode itemData)
      {
        this._mTableRefID = itemData.SelectedMaterialTableRefID;
        this._mRecID = itemData.SelectedMaterialRecID;
        this._aTableRefID = itemData.SelectedAssortmentTableRefID;
        this._aRecID = itemData.SelectedAssortmentRecID;
        itemData.SelectedMaterialTableRefID = 0L;
        itemData.SelectedMaterialRecID = -1L;
        itemData.SelectedAssortmentTableRefID = 0L;
        itemData.SelectedAssortmentRecID = -1L;
      }
    }
    this.LoadData();
    this.LoatTreeData(this._imbaseKey);
  }

  protected new void ClearData()
  {
    base.ClearData();
    this._trv.Nodes.Clear();
  }

  protected override void FavouritesClick(object sender, EventArgs e)
  {
    this.ViewFavourites(string.Empty, string.Empty, (List<object>) null);
  }

  protected override void AddFavouriteClick(object sender, EventArgs e)
  {
    base.AddFavouriteClick(sender, e);
    if (this._trv.SelectedNode?.Parent == null || !(this._trv.SelectedNode.Tag is TrvNode tag))
      return;
    this.ViewFavourites(this._imbaseKey, tag.TrvNodeData, tag.Params);
  }

  protected override void FormulaPaint(PaintEventArgs e, string text)
  {
    base.FormulaPaint(e, this._text);
  }

  protected override void RestoreSelection(
    long mTableRefID,
    long mRecID,
    long aTableRefID,
    long aRecID)
  {
    base.RestoreSelection(mTableRefID, mRecID, aTableRefID, aRecID);
    this.SelectTreeNode();
  }

  private void CustomizeMenu()
  {
    this._trv.ContextMenuStrip = this._contextMenuBase;
    this._tsBtnCoatingProperties.Visible = this._tsBtnGluedMaterials.Visible = this._tsBtnMaterialProperties.Visible = this._tsBtnMaterialSubstitutes.Visible = this._tsSeparator2.Visible = false;
    this._tsBtnSearch.Visible = this._tsBtnSort.Visible = this._tsBtnApplicabilityFilter.Visible = this._tsSeparator1.Visible = false;
    this._cmCoatingProperties.Visible = this._cmGluedMaterials.Visible = this._cmMaterialProperties.Visible = this._cmMaterialSubstitutes.Visible = this._cmSeparator2.Visible = false;
    this._cmSearch.Visible = this._cmSort.Visible = this._cmApplicabilityFilter.Visible = this._cmSeparator1.Visible = false;
  }

  private List<TreeNode> CreateNodes(List<DataRow> rows)
  {
    List<TreeNode> source = (List<TreeNode>) null;
    if (rows.Count > 0)
    {
      source = new List<TreeNode>(rows.Count);
      List<string> keyValues = new List<string>(rows.Count);
      foreach (DataRow row in rows)
      {
        string str = Convert.ToString(row[this._coatingsColumnGuid]);
        if (!string.IsNullOrEmpty(str) && !keyValues.Contains(str))
        {
          keyValues.Add(str);
          TreeNode treeNode = new TreeNode()
          {
            Name = str,
            Tag = (object) new TrvParentNode(Convert.ToString(row[this._purposeColumnGuid]), Convert.ToString(row[this._instructionsColumnGuid]))
          };
          source.Add(treeNode);
          List<TreeNode> subNodes = this.CreateSubNodes(row);
          if (subNodes != null)
            treeNode.Nodes.AddRange(subNodes.ToArray());
        }
      }
      if (keyValues.Count > 0)
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          if (sessionKeeper.Session.GetCustomService(typeof (IImbaseServer)) is IImbaseServer customService)
          {
            Dictionary<string, string> dict = customService.NameRecordReferences(sessionKeeper.Session.SessionGUID, keyValues);
            if (dict != null)
              source.Where<TreeNode>((System.Func<TreeNode, bool>) (x => dict.ContainsKey(x.Name))).ToList<TreeNode>().ForEach((Action<TreeNode>) (x => x.Text = dict[x.Name]));
          }
        }
      }
    }
    return source;
  }

  private List<TreeNode> CreateSubNodes(DataRow row)
  {
    List<TreeNode> subNodes = (List<TreeNode>) null;
    if (row != null && this._dtParams != null && this._dtParams.Rows.Count > 0)
    {
      subNodes = new List<TreeNode>(this._dtParams.Rows.Count);
      int index = 0;
      foreach (DataRow row1 in (InternalDataCollectionBase) this._dtParams.Rows)
      {
        ++index;
        string str1 = this._formula;
        bool flag1 = false;
        List<object> parameters = new List<object>(this._dtParams.Columns.Count);
        bool flag2 = true;
        string caption = string.Empty;
        foreach (DataColumn column in (InternalDataCollectionBase) this._dtParams.Columns)
        {
          string empty = string.Empty;
          object obj1 = row1[column.ColumnName];
          if (obj1 != null && obj1 != DBNull.Value)
          {
            string str2 = Convert.ToString(obj1);
            if (row.Table.Columns.Contains(str2))
            {
              if (!parameters.Contains(obj1))
                parameters.Add(obj1);
              object obj2 = row[str2];
              if (obj2 != null && obj2 != DBNull.Value)
              {
                empty = Convert.ToString(obj2);
                if (!string.IsNullOrEmpty(empty))
                  flag1 = true;
              }
            }
          }
          str1 = str1.Replace($"[{column.ColumnName}]", empty);
          if (flag2)
          {
            caption = empty;
            flag2 = false;
          }
        }
        string text = str1.Replace("[№пп]", Convert.ToString(index));
        if (flag1)
        {
          TreeNode treeNode = new TreeNode(text)
          {
            Tag = (object) new TrvNode(this._imbaseKey, Convert.ToString(row[this._coatingsColumnGuid]), parameters, caption, index)
          };
          subNodes.Add(treeNode);
        }
      }
    }
    return subNodes;
  }

  private Dictionary<string, DataTable> GetPages(TrvParentNode parent, int index)
  {
    Dictionary<string, DataTable> pages = (Dictionary<string, DataTable>) null;
    if (parent != null)
    {
      pages = new Dictionary<string, DataTable>(3);
      DataTable dataTable1 = new DataTable();
      dataTable1.Columns.Add(new DataColumn("colPurpose"));
      DataRow row1 = dataTable1.NewRow();
      row1["colPurpose"] = (object) parent.Purpose;
      dataTable1.Rows.Add(row1);
      pages.Add(LocalizationHolder.rm.GetString("IMH_Coating_Purpose"), dataTable1);
      DataTable dataTable2 = new DataTable();
      dataTable2.Columns.Add(new DataColumn("colInstructions"));
      DataRow row2 = dataTable2.NewRow();
      row2["colInstructions"] = (object) parent.Instructions;
      dataTable2.Rows.Add(row2);
      pages.Add(LocalizationHolder.rm.GetString("IMH_Coating_Instructions"), dataTable2);
      DataTable dataTable3 = new DataTable();
      dataTable3.Columns.Add(new DataColumn("colConditions"));
      switch (index)
      {
        case 1:
          DataRow row3 = dataTable3.NewRow();
          row3["colConditions"] = (object) "У, УХЛ (ХЛ), 2.1; 3; 3.1";
          dataTable3.Rows.Add(row3);
          DataRow row4 = dataTable3.NewRow();
          row4["colConditions"] = (object) "ТС 3; 3.1";
          dataTable3.Rows.Add(row4);
          DataRow row5 = dataTable3.NewRow();
          row5["colConditions"] = (object) "УХЛ (ХЛ). ТС 4; 4.2";
          dataTable3.Rows.Add(row5);
          DataRow row6 = dataTable3.NewRow();
          row6["colConditions"] = (object) "УХЛ (ХЛ). ТВ. ТС. О";
          dataTable3.Rows.Add(row6);
          DataRow row7 = dataTable3.NewRow();
          row7["colConditions"] = (object) "М, ТМ, ОМ, В 4.1";
          dataTable3.Rows.Add(row7);
          break;
        case 2:
          DataRow row8 = dataTable3.NewRow();
          row8["colConditions"] = (object) "ТС 1.1; 2; 3";
          dataTable3.Rows.Add(row8);
          DataRow row9 = dataTable3.NewRow();
          row9["colConditions"] = (object) "ТВ, Т, О 2.1";
          dataTable3.Rows.Add(row9);
          DataRow row10 = dataTable3.NewRow();
          row10["colConditions"] = (object) "ТВ, Т 3; 3.1";
          dataTable3.Rows.Add(row10);
          DataRow row11 = dataTable3.NewRow();
          row11["colConditions"] = (object) "ТВ, О, М, ТМ, ОМ, В 4";
          dataTable3.Rows.Add(row11);
          break;
        case 3:
          DataRow row12 = dataTable3.NewRow();
          row12["colConditions"] = (object) "ТС 1";
          dataTable3.Rows.Add(row12);
          DataRow row13 = dataTable3.NewRow();
          row13["colConditions"] = (object) "У, УХЛ (ХЛ) 1, 1.1; 2; 3";
          dataTable3.Rows.Add(row13);
          break;
        case 4:
          DataRow row14 = dataTable3.NewRow();
          row14["colConditions"] = (object) "ТВ, Т, О, М, ТМ, ОМ, В 1.1";
          dataTable3.Rows.Add(row14);
          break;
        case 5:
          DataRow row15 = dataTable3.NewRow();
          row15["colConditions"] = (object) "У, УХЛ (ХЛ) 1";
          dataTable3.Rows.Add(row15);
          DataRow row16 = dataTable3.NewRow();
          row16["colConditions"] = (object) "ТВ, Т, О 1; 2";
          dataTable3.Rows.Add(row16);
          DataRow row17 = dataTable3.NewRow();
          row17["colConditions"] = (object) "ТВ, Т 3";
          dataTable3.Rows.Add(row17);
          break;
        case 6:
          DataRow row18 = dataTable3.NewRow();
          row18["colConditions"] = (object) "М, ТМ, ОМ, В 1; 2; 2.1; 3; 3.1";
          dataTable3.Rows.Add(row18);
          break;
        case 7:
          DataRow row19 = dataTable3.NewRow();
          row19["colConditions"] = (object) "ТВ, Т, О 1";
          dataTable3.Rows.Add(row19);
          DataRow row20 = dataTable3.NewRow();
          row20["colConditions"] = (object) "УХЛ (ХЛ), ТВ, ТС, О, М, ТМ, ОМ, В 5; 5.1";
          dataTable3.Rows.Add(row20);
          break;
        default:
          DataRow row21 = dataTable3.NewRow();
          row21["colConditions"] = (object) "М, ТМ, ОМ, В 1; 2";
          dataTable3.Rows.Add(row21);
          break;
      }
      pages.Add($"{LocalizationHolder.rm.GetString("IMH_Coating_Conditions")} {Convert.ToString(index)}", dataTable3);
    }
    return pages;
  }

  private void LoadData()
  {
    DataSet imbaseDs = IMHHelper.GetImbaseDS("COATING_PROPERTIES_TABLE_NAME");
    if (imbaseDs == null || !imbaseDs.Tables.Contains("IMS_DATA"))
      return;
    this._dtCoatingsVarieties = imbaseDs.Tables["IMS_DATA"];
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetCustomService(typeof (IIMHSystemSettingsService)) is IIMHSystemSettingsService customService))
        return;
      IMHSystemSettings systemSettings = customService.GetSystemSettings();
      if (systemSettings?.CoatingsSettings == null)
        return;
      string text1 = systemSettings.Dict.ContainsKey("COATING_PROPERTIES_COLUMN_COATING") ? systemSettings.Dict["COATING_PROPERTIES_COLUMN_COATING"] : string.Empty;
      this._coatingsColumnGuid = GuidHelper.IsGuid(text1) ? text1 : string.Empty;
      string text2 = systemSettings.Dict.ContainsKey("COATING_PROPERTIES_COLUMN_MATERIAL") ? systemSettings.Dict["COATING_PROPERTIES_COLUMN_MATERIAL"] : string.Empty;
      this._materialsColumnGuid = GuidHelper.IsGuid(text2) ? text2 : string.Empty;
      string text3 = systemSettings.Dict.ContainsKey("COATING_PROPERTIES_COLUMN_PURPOSE") ? systemSettings.Dict["COATING_PROPERTIES_COLUMN_PURPOSE"] : string.Empty;
      this._purposeColumnGuid = GuidHelper.IsGuid(text3) ? text3 : string.Empty;
      string text4 = systemSettings.Dict.ContainsKey("COATING_PROPERTIES_COLUMN_INSTRUCTIONS") ? systemSettings.Dict["COATING_PROPERTIES_COLUMN_INSTRUCTIONS"] : string.Empty;
      this._instructionsColumnGuid = GuidHelper.IsGuid(text4) ? text4 : string.Empty;
      this._formula = systemSettings.CoatingsSettings.Formula;
      this._dtParams = systemSettings.CoatingsSettings.Params;
    }
  }

  private void LoatTreeData(string imbaseKey)
  {
    if (this._dtCoatingsVarieties == null || !this._dtCoatingsVarieties.Columns.Contains(this._coatingsColumnGuid))
      return;
    List<TreeNode> nodes = this.CreateNodes(this._dtCoatingsVarieties.AsEnumerable().Where<DataRow>((System.Func<DataRow, bool>) (x => Convert.ToString(x[this._materialsColumnGuid]) == imbaseKey)).ToList<DataRow>());
    if (nodes == null)
      return;
    this._trv.Nodes.AddRange(nodes.ToArray());
    this.SelectTreeNode();
  }

  private NavigatorTreeNode SearchFolderNode(string imbaseKey)
  {
    if (this._parentNodePath == null || this._treeView == null)
      return (NavigatorTreeNode) null;
    NavigatorTreeNode lastNode;
    if (!this._treeView.TryFind(this._parentNodePath, out lastNode))
      lastNode = (NavigatorTreeNode) null;
    NavigatorTreeNodes children = lastNode?.Children;
    if (children == null)
      return (NavigatorTreeNode) null;
    NavigatorTreeNode navigatorTreeNode1 = (NavigatorTreeNode) null;
    foreach (NavigatorTreeNode navigatorTreeNode2 in (List<NavigatorTreeNode>) children)
    {
      if (navigatorTreeNode2.NodeID is StandartFolderNodeID nodeId && !(nodeId.ImbaseKey != imbaseKey))
      {
        navigatorTreeNode1 = navigatorTreeNode2;
        break;
      }
    }
    return navigatorTreeNode1;
  }

  private void SelectTreeNode()
  {
    bool flag1 = false;
    if (this._aTableRefID != 0L && this._aRecID != -1L && this._params != null && this._params.Count > 0)
    {
      string str = ImbaseHelper.MakeInternalImbaseKey(this._aTableRefID, this._aRecID);
      foreach (TreeNode node1 in this._trv.Nodes)
      {
        if (!(node1.Name != str))
        {
          node1.Expand();
          foreach (TreeNode node2 in node1.Nodes)
          {
            List<object> objectList = node2.Tag is TrvNode tag ? tag.Params : (List<object>) null;
            if (objectList != null && this._params.Count == objectList.Count)
            {
              bool flag2 = true;
              for (int index = 0; index < this._params.Count; ++index)
              {
                if (!(this._params[index].ToString() == objectList[index].ToString()))
                {
                  flag2 = false;
                  break;
                }
              }
              if (flag2)
              {
                this._trv.SelectedNode = node2;
                flag1 = true;
                this._params = (List<object>) null;
                break;
              }
            }
          }
          if (flag1)
            break;
        }
      }
    }
    else
    {
      if (this._trv.Nodes[0].Nodes.Count <= 0)
        return;
      this._trv.SelectedNode = this._trv.Nodes[0].Nodes[0];
    }
  }

  private void ViewFavourites(string coatingKey, string materialKey, List<object> parameters)
  {
    string caption = string.Empty;
    if (!string.IsNullOrEmpty(coatingKey) && !string.IsNullOrEmpty(materialKey) && this._trv.SelectedNode?.Parent != null)
      caption = $"{this._trv.SelectedNode.Parent.Text} - {this._trv.SelectedNode.Text}";
    using (CoatingFavouritesForm coatingFavouritesForm = new CoatingFavouritesForm(Consts.IMHDetailsMaterialNodeGuid, coatingKey, materialKey, parameters, caption))
    {
      if (coatingFavouritesForm.ShowDialog() != DialogResult.OK)
        return;
      CoatingsFavouriteData data = coatingFavouritesForm.Data;
      this._params = data.Params;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        if (this._imbaseKey == data.CoatingsKey)
        {
          long linkId;
          long recordId;
          if (!ImbaseHelper.TryParseRecordReference(sessionKeeper.Session, data.MaterialsKey, out linkId, out recordId))
            return;
          this._aTableRefID = linkId;
          this._aRecID = recordId;
          this.SelectTreeNode();
        }
        else
        {
          long linkId1;
          long recordId1;
          long linkId2;
          long recordId2;
          if (!ImbaseHelper.TryParseRecordReference(sessionKeeper.Session, data.CoatingsKey, out linkId1, out recordId1) || !ImbaseHelper.TryParseRecordReference(sessionKeeper.Session, data.MaterialsKey, out linkId2, out recordId2))
            return;
          this.SaveIdsForRestore(linkId1, recordId1, linkId2, recordId2);
          this.SearchFolderNode(data.CoatingsKey)?.Focus();
        }
      }
    }
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (IMHDetailsMaterialViewCtrl));
    this._trv = new TreeView();
    this._contextMenu = new ContextMenuStrip(this.components);
    this._miCollapse = new ToolStripMenuItem();
    this._miExpand = new ToolStripMenuItem();
    this._propsPage = new MaterialPropertiesPage();
    this._splt.BeginInit();
    this._splt.Panel1.SuspendLayout();
    this._splt.Panel2.SuspendLayout();
    this._splt.SuspendLayout();
    this._contextMenu.SuspendLayout();
    this.SuspendLayout();
    this._splt.Panel1.Controls.Add((Control) this._trv);
    this._splt.Panel2.Controls.Add((Control) this._propsPage);
    componentResourceManager.ApplyResources((object) this._splt, "_splt");
    componentResourceManager.ApplyResources((object) this._trv, "_trv");
    this._trv.HideSelection = false;
    this._trv.Name = "_trv";
    this._trv.AfterSelect += new TreeViewEventHandler(this.On_trv_AfterSelect);
    this._trv.MouseDoubleClick += new MouseEventHandler(this._trv_MouseDoubleClick);
    this._contextMenu.Items.AddRange(new ToolStripItem[2]
    {
      (ToolStripItem) this._miCollapse,
      (ToolStripItem) this._miExpand
    });
    this._contextMenu.Name = "_contextMenu";
    componentResourceManager.ApplyResources((object) this._contextMenu, "_contextMenu");
    this._miCollapse.DisplayStyle = ToolStripItemDisplayStyle.Text;
    this._miCollapse.Name = "_miCollapse";
    componentResourceManager.ApplyResources((object) this._miCollapse, "_miCollapse");
    this._miCollapse.Tag = (object) "0";
    this._miCollapse.Click += new EventHandler(this.On_miClick);
    this._miExpand.DisplayStyle = ToolStripItemDisplayStyle.Text;
    this._miExpand.Name = "_miExpand";
    componentResourceManager.ApplyResources((object) this._miExpand, "_miExpand");
    this._miExpand.Tag = (object) "1";
    this._miExpand.Click += new EventHandler(this.On_miClick);
    this._propsPage.ContextMenuStrip = this._contextMenu;
    componentResourceManager.ApplyResources((object) this._propsPage, "_propsPage");
    this._propsPage.ImbaseKey = "";
    this._propsPage.Name = "_propsPage";
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Name = nameof (IMHDetailsMaterialViewCtrl);
    this.Controls.SetChildIndex((Control) this._pnlFormula, 0);
    this.Controls.SetChildIndex((Control) this._splt, 0);
    this._splt.Panel1.ResumeLayout(false);
    this._splt.Panel2.ResumeLayout(false);
    this._splt.EndInit();
    this._splt.ResumeLayout(false);
    this._contextMenu.ResumeLayout(false);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
