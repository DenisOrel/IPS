// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.IMHCoatingsVarietiesViewCtrl
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
using Intermech.Navigator.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class IMHCoatingsVarietiesViewCtrl : IMHViewCtrlBase
{
  private DataTable _dtCoatingsVarieties;
  private Dictionary<string, List<TreeNode>> _nodes = new Dictionary<string, List<TreeNode>>();
  private string _coatingsColumnGuid = string.Empty;
  private string _materialsColumnGuid = string.Empty;
  private string _purposeColumnGuid = string.Empty;
  private string _instructionsColumnGuid = string.Empty;
  private string _formula = string.Empty;
  private DataTable _dtParams;
  private List<object> _params;
  private bool _lock;
  private string _text = string.Empty;
  private int _imgIndex = -1;
  private IContainer components;
  private SplitContainer _spltAssortment;
  private TreeView _trv;
  private ContextMenuStrip _contextMenu;
  private ToolStripMenuItem _miCollapse;
  private ToolStripMenuItem _miExpand;
  private MaterialPropertiesPage _propsPage;
  private ListView _lv;
  private System.Windows.Forms.ColumnHeader _colCaption;

  public IMHCoatingsVarietiesViewCtrl()
  {
    this.InitializeComponent();
    this.CustomizeMenu();
    this._propsPage.SetRibbonInvisible();
    INamedImageList service = ServiceUtils.GetService<INamedImageList>((object) ApplicationServices.Container, false);
    if (service == null)
      return;
    this._lv.SmallImageList = this._lv.LargeImageList = service.ImageList;
    this._imgIndex = service.ImageIndex("icoCoating");
  }

  private void On_lv_SelectedIndexChanged(object sender, EventArgs e)
  {
    if (this._lv.SelectedItems.Count > 0)
    {
      if (this._lv.SelectedItems[0].Tag is IMHCoatingsVarietiesViewCtrl.LvItem tag)
      {
        this._mTableRefID = tag.M_TableID;
        this._mRecID = tag.RecID;
        this._mCaption = tag.Caption;
        this.LoadTreeData(tag.ImbaseKey);
      }
    }
    else
    {
      this._mTableRefID = 0L;
      this._mRecID = -1L;
      this._mCaption = string.Empty;
      this._trv.Nodes.Clear();
      this._propsPage.Clear(true);
    }
    this.OnIMHMaterialChanged(this._mTableRefID, this._mRecID);
  }

  private void On_lv_SizeChanged(object sender, EventArgs e)
  {
    if (this._lock || this._lv == null || this._lv.Columns.Count == 0 || this._lv.Columns[0] == null)
      return;
    this._lock = true;
    this._lv.Columns[0].Width = -2;
    this._lock = false;
  }

  private void On_miClick(object sender, EventArgs e)
  {
    this._propsPage.ExpandAll((int) Convert.ToInt16(sender is ToolStripMenuItem toolStripMenuItem ? toolStripMenuItem.Tag : (object) null) == sc_14601.ssp_imbase_14602(330771591));
  }

  private void On_trv_AfterSelect(object sender, TreeViewEventArgs e)
  {
    trvNode = (TrvNode) null;
    this._propsPage.Clear(false);
    string designation = string.Empty;
    if (this._trv.SelectedNode?.Parent != null && this._trv.SelectedNode.Tag is TrvNode trvNode)
    {
      designation = trvNode.Designation;
      Dictionary<string, DataTable> pages = this.GetPages(this._trv.SelectedNode.Parent.Tag as TrvParentNode, trvNode.Index);
      if (pages != null)
      {
        foreach (KeyValuePair<string, DataTable> keyValuePair in pages)
          this._propsPage.AddPage(keyValuePair.Key, new List<DataTable>((IEnumerable<DataTable>) new DataTable[1]
          {
            keyValuePair.Value
          }), false, false);
      }
    }
    this._text = trvNode != null ? trvNode.Caption : string.Empty;
    this._pnlFormula.Invalidate();
    this.OnIMHMaterialChanged(this._mTableRefID, this._mRecID, designation: designation);
  }

  private void On_trv_Enter(object sender, EventArgs e)
  {
    string designation = string.Empty;
    if (this._trv.SelectedNode?.Parent != null && this._trv.SelectedNode.Tag is TrvNode tag)
      designation = tag.Designation;
    this.OnIMHMaterialChanged(this._mTableRefID, this._mRecID, designation: designation);
  }

  private void On_trv_Leave(object sender, EventArgs e)
  {
    string designation = string.Empty;
    if (this._trv.SelectedNode?.Parent != null && this._trv.SelectedNode.Tag is TrvNode tag)
      designation = tag.Designation;
    this.OnIMHMaterialChanged(this._mTableRefID, this._mRecID, designation: designation);
  }

  private void _lv_MouseDoubleClick(object sender, MouseEventArgs e)
  {
    if (this._lv.SelectedItems.Count == 0 || this._services == null || !(this._services.GetService(typeof (ISelectionWindow)) is ISelectionWindow service))
      return;
    service.OkButton.PerformClick();
  }

  private void _trv_MouseDoubleClick(object sender, MouseEventArgs e)
  {
    if (this._trv.SelectedNode == null || this._services == null || !(this._services.GetService(typeof (ISelectionWindow)) is ISelectionWindow service))
      return;
    service.OkButton.PerformClick();
  }

  public override void Activate(IView previousView)
  {
    base.Activate(previousView);
    this._lv.Items[0].Selected = this._lv.Items.Count > 0 && this._lv.SelectedItems.Count == 0;
  }

  public override void Initialize(
    ISelectedItems items,
    IServiceProvider provider,
    NavigatorTreeNode parentINode)
  {
    this.CleanData();
    base.Initialize(items, provider, parentINode);
    this._lv.Sorting = SortOrder.Ascending;
    if (items == null)
      return;
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
    else
    {
      this._mTableRefID = this._aTableRefID = 0L;
      this._mRecID = this._aRecID = -1L;
      this._params = (List<object>) null;
    }
    this.CreateItems(this.LoadData(itemData != null ? itemData.FolderID : 0L));
  }

  protected void CleanData()
  {
    this.ClearData();
    this._lock = true;
    this._lv.Items.Clear();
    this._trv.Nodes.Clear();
    this._nodes.Clear();
    this._lock = false;
  }

  protected override void CoatingPropertiesClick(object sender, EventArgs e)
  {
    if (this._lv.SelectedItems.Count <= 0)
      return;
    string str = (this._lv.SelectedItems[0].Tag is IMHCoatingsVarietiesViewCtrl.LvItem tag ? tag.ImbaseKey : (string) null) ?? string.Empty;
    if (!this._nodes.ContainsKey(str))
      return;
    List<TreeNode> node = this._nodes[str];
    Dictionary<string, string> materials = new Dictionary<string, string>(node.Count);
    foreach (TreeNode treeNode in node)
    {
      if (!materials.ContainsKey(treeNode.Name))
        materials.Add(treeNode.Name, treeNode.Text);
    }
    using (EditCoatingPropertiesForm coatingPropertiesForm = new EditCoatingPropertiesForm(str, this._dtCoatingsVarieties, this._coatingsColumnGuid, this._materialsColumnGuid, this._purposeColumnGuid, this._instructionsColumnGuid, this._dtParams, materials))
    {
      int num = (int) coatingPropertiesForm.ShowDialog();
      this.ReloadTreeDataForSelectedItem(str);
    }
  }

  protected override void FavouritesClick(object sender, EventArgs e)
  {
    this.ViewFavourites(string.Empty, string.Empty, (List<object>) null);
  }

  protected override void AddFavouriteClick(object sender, EventArgs e)
  {
    if (this._trv.SelectedNode?.Parent == null || !(this._trv.SelectedNode.Tag is TrvNode tag))
      return;
    this.ViewFavourites(tag.LvItemData, tag.TrvNodeData, tag.Params);
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
    base.FormulaPaint(e, this._text);
  }

  protected override void RestoreSelection(
    long mTableRefID,
    long mRecID,
    long aTableRefID,
    long aRecID)
  {
    base.RestoreSelection(mTableRefID, mRecID, aTableRefID, aRecID);
    bool flag = false;
    if (this._lv.SelectedItems.Count > 0 && this._lv.SelectedItems[0].Tag is IMHCoatingsVarietiesViewCtrl.LvItem tag1 && tag1.RecID == mRecID && tag1.M_TableID == mTableRefID)
      flag = true;
    if (flag)
      return;
    foreach (ListViewItem listViewItem in this._lv.Items)
    {
      if (!(listViewItem.Tag is IMHCoatingsVarietiesViewCtrl.LvItem tag2) || tag2.RecID == mRecID && tag2.M_TableID == mTableRefID)
      {
        listViewItem.Selected = true;
        break;
      }
    }
  }

  private void CustomizeMenu()
  {
    this._lv.ContextMenuStrip = this._contextMenuBase;
    this._tsBtnGluedMaterials.Visible = this._tsBtnMaterialProperties.Visible = this._tsBtnMaterialSubstitutes.Visible = this._tsBtnSearch.Visible = this._tsBtnApplicabilityFilter.Visible = false;
    this._cmGluedMaterials.Visible = this._cmMaterialProperties.Visible = this._cmMaterialSubstitutes.Visible = this._cmSearch.Visible = this._cmApplicabilityFilter.Visible = false;
  }

  private void CreateItems(List<long> linkIDs)
  {
    if (linkIDs == null)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetCustomService(typeof (IImbaseServer)) is IImbaseServer customService))
        return;
      foreach (long linkId1 in linkIDs)
      {
        long objID = linkId1;
        DataTable recordsTable;
        customService.LoadRecords(sessionKeeper.Session.SessionGUID, objID, string.Empty, Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator, out recordsTable, out AttributeTypeProperties[] _, out ImbaseKeyInfo _);
        if (recordsTable != null && recordsTable.Rows.Count != 0)
        {
          List<string> list = recordsTable.AsEnumerable().Select<DataRow, string>((System.Func<DataRow, string>) (x => ImbaseHelper.MakeInternalImbaseKey(objID, Convert.ToInt64(x["-2"])))).ToList<string>();
          Dictionary<string, string> dictionary = customService.NameRecordReferences(sessionKeeper.Session.SessionGUID, list);
          if (dictionary != null)
          {
            foreach (KeyValuePair<string, string> keyValuePair in dictionary)
            {
              long linkId2;
              long recordId;
              ImbaseHelper.TryParseRecordReference(sessionKeeper.Session, keyValuePair.Key, out linkId2, out recordId);
              ListViewItem listViewItem = new ListViewItem(keyValuePair.Value, this._imgIndex)
              {
                Tag = (object) new IMHCoatingsVarietiesViewCtrl.LvItem(keyValuePair.Key, linkId2, recordId, keyValuePair.Value)
              };
              this._lv.Items.Add(listViewItem);
              if (recordId == this._mRecID && linkId2 == this._mTableRefID)
                listViewItem.Selected = true;
            }
          }
        }
      }
    }
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
        string str = Convert.ToString(row[this._materialsColumnGuid]);
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
            Tag = (object) new TrvNode(row[this._coatingsColumnGuid].ToString(), row[this._materialsColumnGuid].ToString(), parameters, caption, index)
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
      pages.Add($"{LocalizationHolder.rm.GetString("IMH_Coating_Conditions")} {index}", dataTable3);
    }
    return pages;
  }

  private List<long> LoadData(long folderID)
  {
    List<long> linksEntersInFolder;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      linksEntersInFolder = ImbaseHelper.GetLinksEntersInFolder(sessionKeeper.Session, folderID);
      if (linksEntersInFolder != null)
      {
        DataSet imbaseDs = IMHHelper.GetImbaseDS("COATING_PROPERTIES_TABLE_NAME");
        if (imbaseDs != null)
        {
          if (imbaseDs.Tables.Contains("IMS_DATA"))
          {
            this._dtCoatingsVarieties = imbaseDs.Tables["IMS_DATA"];
            if (sessionKeeper.Session.GetCustomService(typeof (IIMHSystemSettingsService)) is IIMHSystemSettingsService customService)
            {
              IMHSystemSettings systemSettings = customService.GetSystemSettings();
              if (systemSettings?.CoatingsSettings != null)
              {
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
          }
        }
      }
    }
    return linksEntersInFolder;
  }

  private void LoadTreeData(string imbaseKey)
  {
    this._trv.BeginUpdate();
    try
    {
      this._trv.Nodes.Clear();
      List<TreeNode> treeNodeList = (List<TreeNode>) null;
      if (this._nodes.ContainsKey(imbaseKey))
        treeNodeList = this._nodes[imbaseKey];
      else if (this._dtCoatingsVarieties != null && this._dtCoatingsVarieties.Rows.Count > 0 && this._dtCoatingsVarieties.Columns.Contains(this._materialsColumnGuid))
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          string str1 = ImbaseHelper.ConvertImbaseKey(sessionKeeper.Session, imbaseKey);
          List<DataRow> rows = new List<DataRow>();
          foreach (DataRow row in (InternalDataCollectionBase) this._dtCoatingsVarieties.Rows)
          {
            string str2 = Convert.ToString(row[this._coatingsColumnGuid]);
            if (!(str2 != imbaseKey) || !(str2 != str1))
              rows.Add(row);
          }
          treeNodeList = this.CreateNodes(rows);
        }
      }
      if (treeNodeList == null)
        return;
      this._trv.Nodes.AddRange(treeNodeList.ToArray());
      this._nodes[imbaseKey] = treeNodeList;
      this.SelectTreeNode();
    }
    finally
    {
      this._trv.EndUpdate();
    }
  }

  private void ReloadTreeDataForSelectedItem(string itemKey)
  {
    if (this._nodes != null && this._nodes.ContainsKey(itemKey))
      this._nodes.Remove(itemKey);
    this.LoadTreeData(itemKey);
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
    using (CoatingFavouritesForm coatingFavouritesForm = new CoatingFavouritesForm(Consts.IMHCoatingsVarietiesNodeGuid, coatingKey, materialKey, parameters, caption))
    {
      if (coatingFavouritesForm.ShowDialog() != DialogResult.OK)
        return;
      CoatingsFavouriteData data = coatingFavouritesForm.Data;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        long linkId1;
        long recordId1;
        long linkId2;
        long recordId2;
        if (!ImbaseHelper.TryParseRecordReference(sessionKeeper.Session, data.CoatingsKey, out linkId1, out recordId1) || !ImbaseHelper.TryParseRecordReference(sessionKeeper.Session, data.MaterialsKey, out linkId2, out recordId2))
          return;
        this._params = data.Params;
        this.GoToNode(linkId1, linkId1, recordId1, linkId2, recordId2);
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (IMHCoatingsVarietiesViewCtrl));
    this._spltAssortment = new SplitContainer();
    this._trv = new TreeView();
    this._contextMenu = new ContextMenuStrip(this.components);
    this._miCollapse = new ToolStripMenuItem();
    this._miExpand = new ToolStripMenuItem();
    this._propsPage = new MaterialPropertiesPage();
    this._colCaption = new System.Windows.Forms.ColumnHeader();
    this._lv = new ListView();
    this._splt.BeginInit();
    this._splt.Panel1.SuspendLayout();
    this._splt.Panel2.SuspendLayout();
    this._splt.SuspendLayout();
    this._spltAssortment.BeginInit();
    this._spltAssortment.Panel1.SuspendLayout();
    this._spltAssortment.Panel2.SuspendLayout();
    this._spltAssortment.SuspendLayout();
    this._contextMenu.SuspendLayout();
    this.SuspendLayout();
    this._splt.Panel1.Controls.Add((Control) this._spltAssortment);
    this._splt.Panel2.Controls.Add((Control) this._propsPage);
    componentResourceManager.ApplyResources((object) this._splt, "_splt");
    componentResourceManager.ApplyResources((object) this._spltAssortment, "_spltAssortment");
    this._spltAssortment.Name = "_spltAssortment";
    this._spltAssortment.Panel1.Controls.Add((Control) this._lv);
    this._spltAssortment.Panel2.Controls.Add((Control) this._trv);
    componentResourceManager.ApplyResources((object) this._trv, "_trv");
    this._trv.HideSelection = false;
    this._trv.Name = "_trv";
    this._trv.AfterSelect += new TreeViewEventHandler(this.On_trv_AfterSelect);
    this._trv.Enter += new EventHandler(this.On_trv_Enter);
    this._trv.Leave += new EventHandler(this.On_trv_Leave);
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
    componentResourceManager.ApplyResources((object) this._colCaption, "_colCaption");
    this._lv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[1]
    {
      this._colCaption
    });
    componentResourceManager.ApplyResources((object) this._lv, "_lv");
    this._lv.FullRowSelect = true;
    this._lv.HeaderStyle = ColumnHeaderStyle.None;
    this._lv.HideSelection = false;
    this._lv.MultiSelect = false;
    this._lv.Name = "_lv";
    this._lv.UseCompatibleStateImageBehavior = false;
    this._lv.View = View.Details;
    this._lv.SelectedIndexChanged += new EventHandler(this.On_lv_SelectedIndexChanged);
    this._lv.SizeChanged += new EventHandler(this.On_lv_SizeChanged);
    this._lv.MouseDoubleClick += new MouseEventHandler(this._lv_MouseDoubleClick);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Name = nameof (IMHCoatingsVarietiesViewCtrl);
    this.Controls.SetChildIndex((Control) this._pnlFormula, 0);
    this.Controls.SetChildIndex((Control) this._splt, 0);
    this._splt.Panel1.ResumeLayout(false);
    this._splt.Panel2.ResumeLayout(false);
    this._splt.EndInit();
    this._splt.ResumeLayout(false);
    this._spltAssortment.Panel1.ResumeLayout(false);
    this._spltAssortment.Panel2.ResumeLayout(false);
    this._spltAssortment.EndInit();
    this._spltAssortment.ResumeLayout(false);
    this._contextMenu.ResumeLayout(false);
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  private class LvItem
  {
    internal long M_TableID { get; }

    internal long RecID { get; }

    internal string Caption { get; }

    internal string ImbaseKey { get; }

    public LvItem(string imbaseKey, long mTableID, long recID, string caption)
    {
      this.ImbaseKey = imbaseKey;
      this.M_TableID = mTableID;
      this.RecID = recID;
      this.Caption = caption;
    }
  }
}
