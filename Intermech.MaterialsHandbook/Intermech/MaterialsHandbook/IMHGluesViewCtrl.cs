// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.IMHGluesViewCtrl
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using ImSSP;
using Intermech.Imbase;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
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

[ToolboxItem(false)]
public class IMHGluesViewCtrl : IMHViewCtrlBase
{
  private long _materialsTableRefObjectID;
  private DataTable _dtMaterials;
  private string _materialColName = string.Empty;
  private DataTable _dtGlues;
  private string _materialColName_1 = string.Empty;
  private string _materialColName_2 = string.Empty;
  private string _glueColName = string.Empty;
  private bool _dataLoaded;
  private bool _lock;
  private bool _readOnly;
  private Dictionary<string, string> _dictMaterials = new Dictionary<string, string>();
  private int _imgIndex = -1;
  private IContainer components;
  private ListView _lv;
  private System.Windows.Forms.ColumnHeader _caption;
  private ContextMenuStrip _contextMenu;
  private ToolStripMenuItem _miCollapse;
  private ToolStripMenuItem _miExpand;
  private Panel _pnl;
  private Button _btnDel;
  private Button _btnAdd;
  private MaterialPropertiesPage _propsPage;
  private ToolStripSeparator _tsHSeparator1;
  private new ToolStripMenuItem _cmFavourites;
  private new ToolStripMenuItem _cmAddFavourite;
  private ToolStripSeparator _tsHSeparator2;
  private new ToolStripMenuItem _cmSearch;

  public IMHGluesViewCtrl()
  {
    this.InitializeComponent();
    this.CustomizeMenu();
    this._propsPage.ReloadAdditionalPage += new EventHandler(this.On_propsPage_ReloadAdditionalPage);
    INamedImageList service = ServiceUtils.GetService<INamedImageList>((object) ApplicationServices.Container, false);
    if (service == null)
      return;
    this._lv.SmallImageList = this._lv.LargeImageList = service.ImageList;
    this._imgIndex = service.ImageIndex("imgGlue");
  }

  private void On_AddProps_Click(object sender, EventArgs e)
  {
    IDescriptor rootDescriptor = (IDescriptor) new Intermech.Navigator.DBObjectTypes.Descriptor(MetaDataHelper.GetObjectTypeID(Intermech.Imbase.Consts.MaterialPropertiesObjTypeGuid));
    long[] numArray = Intermech.Navigator.SelectionWindow.SelectObjects(LocalizationHolder.rm.GetString("IMH_SelectObject"), LocalizationHolder.rm.GetString("IMH_SelectGluesProperties"), rootDescriptor, SelectionOptions.SelectObjects | SelectionOptions.DisableSelectFromTree | SelectionOptions.DisableSelectAbstractTypes | SelectionOptions.DisableMultiselect);
    if (numArray == null || numArray.Length == 0)
      return;
    if (!this._propsPage.IsSettingsLoaded)
      this._propsPage.ReloadSettingsData();
    if (!this._propsPage.IsSettingsLoaded)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      DataTable settingsTable = this._propsPage.SettingsTable;
      string imbaseKey = this._propsPage.ImbaseKey;
      bool isGuidKey;
      string str1 = ImbaseHelper.ConvertImbaseKey(sessionKeeper.Session, imbaseKey, out isGuidKey);
      DataRow dataRow = (DataRow) null;
      foreach (DataRow row in (InternalDataCollectionBase) settingsTable.Rows)
      {
        string str2 = Convert.ToString(row[this._propsPage.ColMaterial]);
        if (!(str2 != imbaseKey) || !(str2 != str1))
        {
          dataRow = row;
          break;
        }
      }
      if (dataRow != null)
      {
        dataRow[this._propsPage.ColObject] = (object) numArray[0];
      }
      else
      {
        DataRow row = settingsTable.NewRow();
        row["F_GUID"] = (object) Guid.NewGuid();
        row[this._propsPage.ColMaterial] = isGuidKey ? (object) str1 : (object) imbaseKey;
        row[this._propsPage.ColObject] = (object) numArray[0];
        settingsTable.Rows.Add(row);
      }
      settingsTable.AcceptChanges();
      long tableIdByTableRefId = IMHHelper.GetTableIDByTableRefID(IMHHelper.GetObjectIDByConstName("MATERIAL_PROPERTIES_TABLE_NAME"));
      TableLoadHelper.StoreData(sessionKeeper.Session, tableIdByTableRefId, settingsTable.DataSet, sessionKeeper.Session.GetCustomService(typeof (ITablesIndexer)) as ITablesIndexer);
      this._propsPage.ImbaseKey = imbaseKey;
      ServiceUtils.GetService<INotificationService>((object) ApplicationServices.Container, false)?.FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", tableIdByTableRefId));
    }
  }

  private void On_DelProps_Click(object sender, EventArgs e)
  {
    string caption = LocalizationHolder.rm.GetString("IMH_DeleteProperties");
    if (MessageBox.Show(LocalizationHolder.rm.GetString("IMH_DeleteProperties_Msg"), caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
      return;
    if (!this._propsPage.IsSettingsLoaded)
      this._propsPage.ReloadSettingsData();
    if (!this._propsPage.IsSettingsLoaded)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      DataTable settingsTable = this._propsPage.SettingsTable;
      string imbaseKey = this._propsPage.ImbaseKey;
      string str1 = ImbaseHelper.ConvertImbaseKey(sessionKeeper.Session, imbaseKey);
      List<DataRow> dataRowList = new List<DataRow>();
      foreach (DataRow row in (InternalDataCollectionBase) settingsTable.Rows)
      {
        string str2 = Convert.ToString(row[this._propsPage.ColMaterial]);
        if (!(str2 != imbaseKey) || !(str2 != str1))
          dataRowList.Add(row);
      }
      if (dataRowList.Count <= 0)
        return;
      foreach (DataRow row in dataRowList)
        settingsTable.Rows.Remove(row);
      settingsTable.AcceptChanges();
      long tableIdByTableRefId = IMHHelper.GetTableIDByTableRefID(IMHHelper.GetObjectIDByConstName("MATERIAL_PROPERTIES_TABLE_NAME"));
      TableLoadHelper.StoreData(sessionKeeper.Session, tableIdByTableRefId, settingsTable.DataSet, sessionKeeper.Session.GetCustomService(typeof (ITablesIndexer)) as ITablesIndexer);
      this._propsPage.Clear(true);
      ServiceUtils.GetService<INotificationService>((object) ApplicationServices.Container, false)?.FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", tableIdByTableRefId));
    }
  }

  private void On_lv_SelectedIndexChanged(object sender, EventArgs e)
  {
    bool flag = false;
    if (this._lv.SelectedItems.Count > 0)
    {
      if (this._lv.SelectedItems[0].Tag is LvItem tag)
      {
        this._mTableRefID = tag.M_TableID;
        this._mRecID = tag.RecID;
        this._mCaption = tag.Caption;
      }
      string imbaseKey = ImbaseHelper.MakeInternalImbaseKey(this._mTableRefID, this._mRecID);
      this._propsPage.ImbaseKey = imbaseKey;
      flag = true;
      this.AddPropertiesPage(imbaseKey);
    }
    else
    {
      this._mTableRefID = 0L;
      this._mRecID = -1L;
      this._mCaption = string.Empty;
      this._propsPage.ImbaseKey = string.Empty;
    }
    if (this._readOnly)
      flag = false;
    this._btnAdd.Enabled = this._btnDel.Enabled = flag;
    this._tsBtnGluedMaterials.Enabled = this._cmGluedMaterials.Enabled = flag;
    this._pnlFormula.Invalidate();
    this.OnIMHMaterialChanged(this._mTableRefID, this._mRecID, designation: this._mCaption);
  }

  private void On_lv_SizeChanged(object sender, EventArgs e)
  {
    if (this._lock || this._lv == null || this._lv.Columns.Count <= 0 || this._lv.Columns[0] == null)
      return;
    this._lock = true;
    this._lv.Columns[0].Width = -2;
    this._lock = false;
  }

  private void On_miClick(object sender, EventArgs e)
  {
    this._propsPage.ExpandAll((int) Convert.ToInt16(sender is ToolStripMenuItem toolStripMenuItem ? toolStripMenuItem.Tag : (object) null) == sc_14607.ssp_imbase_14608(334669196));
  }

  private void On_propsPage_ReloadAdditionalPage(object sender, EventArgs e)
  {
    this.AddPropertiesPage(sender is MaterialPropertiesPage materialPropertiesPage ? materialPropertiesPage.ImbaseKey : (string) null);
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
    bool canEdit = IMHViewCtrlBase.ExtractCanEdit(parentINode);
    this._readOnly = !canEdit;
    this._propsPage.Initialize(canEdit);
    this._btnAdd.Enabled = this._btnDel.Enabled = canEdit;
    if (!(items?.GetItemData(0, typeof (FolderNode)) is FolderNode itemData))
      return;
    this._mTableRefID = itemData.SelectedMaterialTableRefID;
    this._mRecID = itemData.SelectedMaterialRecID;
    itemData.SelectedMaterialTableRefID = 0L;
    itemData.SelectedMaterialRecID = -1L;
    this.CreateItems(this.LoadData(itemData.FolderID));
  }

  protected new void ClearData()
  {
    base.ClearData();
    this._lock = true;
    this._lv.Items.Clear();
    this._lock = false;
    this._propsPage.ImbaseKey = string.Empty;
    this._btnAdd.Enabled = this._btnDel.Enabled = false;
    this._tsBtnGluedMaterials.Enabled = this._cmGluedMaterials.Enabled = false;
  }

  protected override void MaterialsClick(object sender, EventArgs e)
  {
    using (GlueMaterials glueMaterials = new GlueMaterials(ImbaseHelper.MakeInternalImbaseKey(this._mTableRefID, this._mRecID), this._mCaption))
    {
      glueMaterials.MaterialsData(this._materialsTableRefObjectID, this._dtMaterials, this._materialColName);
      glueMaterials.GluesData(this._dtGlues, this._materialColName_1, this._materialColName_2, this._glueColName);
      int num = (int) glueMaterials.ShowDialog();
    }
  }

  protected override void FavouritesClick(object sender, EventArgs e)
  {
    this.ViewFavourites(0L, -1L, string.Empty);
  }

  protected override void AddFavouriteClick(object sender, EventArgs e)
  {
    this.ViewFavourites(this._mTableRefID, this._mRecID, this._mCaption);
  }

  protected override void SearchClick(object sender, EventArgs e)
  {
    if (this._dataLoaded)
    {
      Dictionary<string, string> materials = this.GetMaterials();
      using (GlueSearchForm glueSearchForm = new GlueSearchForm(materials, materials, new Func<string, string, Dictionary<string, string>>(this.Search)))
      {
        string captionForm = LocalizationHolder.rm.GetString("IMH_Glue_Search");
        string caption1 = LocalizationHolder.rm.GetString("IMH_Glue_Material_First");
        string caption2 = LocalizationHolder.rm.GetString("IMH_Glue_Material_Second");
        string captionResult = LocalizationHolder.rm.GetString("IMH_Glues");
        glueSearchForm.SetCaptions(captionForm, caption1, caption2, captionResult);
        int num = (int) glueSearchForm.ShowDialog();
        if (string.IsNullOrEmpty(glueSearchForm.ImbaseKey) || this._services == null)
          return;
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          long linkId;
          long recordId;
          if (!ImbaseHelper.TryParseRecordReference(sessionKeeper.Session, glueSearchForm.ImbaseKey, out linkId, out recordId))
            return;
          this.GoToNode(linkId, linkId, recordId);
        }
      }
    }
    else
    {
      string caption = LocalizationHolder.rm.GetString("IMH_Glues_EmptyData_Caption");
      int num = (int) MessageBox.Show(LocalizationHolder.rm.GetString("IMH_Glues_EmptyData_Msg"), caption, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }
  }

  protected override void RestoreSelection(
    long mTableRefID,
    long mRecID,
    long aTableRefID,
    long aRecID)
  {
    base.RestoreSelection(mTableRefID, mRecID, aTableRefID, aRecID);
    foreach (ListViewItem listViewItem in this._lv.Items)
    {
      if (!(listViewItem.Tag is LvItem tag) || tag.RecID == mRecID && tag.M_TableID == mTableRefID)
      {
        listViewItem.Selected = true;
        break;
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
    base.FormulaPaint(e, this._mCaption);
  }

  private void CustomizeMenu()
  {
    this._lv.ContextMenuStrip = this._contextMenuBase;
    this._tsBtnSearch.ToolTipText = this._cmSearch.Text = LocalizationHolder.rm.GetString("IMH_Search_Glues");
    this._tsBtnCoatingProperties.Visible = this._tsBtnMaterialProperties.Visible = this._tsBtnMaterialSubstitutes.Visible = this._tsBtnApplicabilityFilter.Visible = false;
    this._cmCoatingProperties.Visible = this._cmMaterialProperties.Visible = this._cmMaterialSubstitutes.Visible = this._cmApplicabilityFilter.Visible = false;
  }

  private void CreateItems(List<long> linkIDs)
  {
    if (linkIDs == null)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetCustomService(typeof (IImbaseServer)) is IImbaseServer customService))
        return;
      List<string> keyValues = new List<string>();
      foreach (long linkId in linkIDs)
      {
        long objID = linkId;
        DataTable recordsTable;
        customService.LoadRecords(sessionKeeper.Session.SessionGUID, objID, string.Empty, Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator, out recordsTable, out AttributeTypeProperties[] _, out ImbaseKeyInfo _);
        if (recordsTable != null && recordsTable.Rows.Count != 0)
          keyValues.AddRange((IEnumerable<string>) recordsTable.AsEnumerable().Select<DataRow, string>((System.Func<DataRow, string>) (x => ImbaseHelper.MakeInternalImbaseKey(objID, Convert.ToInt64(x["-2"])))));
      }
      Dictionary<string, string> dictionary = customService.NameRecordReferences(sessionKeeper.Session.SessionGUID, keyValues);
      if (dictionary == null)
        return;
      ListViewItem[] items = new ListViewItem[dictionary.Count];
      int num = 0;
      foreach (KeyValuePair<string, string> keyValuePair in dictionary)
      {
        long linkId;
        long recordId;
        ImbaseHelper.TryParseRecordReference(sessionKeeper.Session, keyValuePair.Key, out linkId, out recordId);
        ListViewItem listViewItem = new ListViewItem(keyValuePair.Value, this._imgIndex)
        {
          Tag = (object) new LvItem(linkId, (long) Convert.ToInt32(recordId), keyValuePair.Value)
        };
        items[num++] = listViewItem;
        if (this._mRecID == recordId && this._mTableRefID == linkId)
          listViewItem.Selected = true;
      }
      this._lv.BeginUpdate();
      try
      {
        this._lv.Items.AddRange(items);
      }
      finally
      {
        this._lv.EndUpdate();
      }
    }
  }

  private void AddPropertiesPage(string imbaseKey)
  {
    DataTable additionalPage = this.GetAdditionalPage(imbaseKey);
    if (additionalPage == null)
      return;
    this._propsPage.AddPage(LocalizationHolder.rm.GetString("IMH_Glues_AdditionalPage_Caption"), new List<DataTable>((IEnumerable<DataTable>) new DataTable[1]
    {
      additionalPage
    }));
  }

  private DataTable GetAdditionalPage(string imbaseKey)
  {
    DataTable additionalPage = (DataTable) null;
    if (this._dataLoaded)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        string str1 = ImbaseHelper.ConvertImbaseKey(sessionKeeper.Session, imbaseKey);
        List<DataRow> dataRowList = new List<DataRow>();
        foreach (DataRow row in (InternalDataCollectionBase) this._dtGlues.Rows)
        {
          string str2 = Convert.ToString(row[this._glueColName]);
          if (!(str2 != imbaseKey) || !(str2 != str1))
            dataRowList.Add(row);
        }
        if (dataRowList.Count > 0)
        {
          additionalPage = new DataTable();
          additionalPage.Columns.Add(new DataColumn(LocalizationHolder.rm.GetString("IMH_MaterialsNode_Caption")));
          foreach (DataRow dataRow in dataRowList)
          {
            string imbaseKey1 = Convert.ToString(dataRow[this._materialColName_1]);
            string imbaseKey2 = Convert.ToString(dataRow[this._materialColName_2]);
            if (!string.IsNullOrEmpty(imbaseKey1) && !string.IsNullOrEmpty(imbaseKey2))
            {
              DataRow row = additionalPage.NewRow();
              row[0] = (object) $"{this.GetMaterialCaption(imbaseKey1)} - {this.GetMaterialCaption(imbaseKey2)}";
              additionalPage.Rows.Add(row);
            }
          }
        }
      }
    }
    return additionalPage;
  }

  private string GetMaterialCaption(string imbaseKey)
  {
    string materialCaption = string.Empty;
    if (this._dictMaterials.ContainsKey(imbaseKey))
    {
      materialCaption = this._dictMaterials[imbaseKey];
    }
    else
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        long linkId;
        long recID;
        if (ImbaseHelper.TryParseRecordReference(sessionKeeper.Session, imbaseKey, out linkId, out recID))
        {
          if (this._materialsTableRefObjectID == linkId)
          {
            DataRow dataRow = this._dtMaterials.AsEnumerable().FirstOrDefault<DataRow>((System.Func<DataRow, bool>) (x => Convert.ToInt64(x["F_KEY"]) == recID));
            if (dataRow != null)
            {
              materialCaption = Convert.ToString(dataRow[this._materialColName]);
              if (!string.IsNullOrEmpty(materialCaption))
                this._dictMaterials[imbaseKey] = materialCaption;
            }
          }
        }
      }
    }
    return materialCaption;
  }

  private List<long> LoadData(long folderID)
  {
    List<long> linksEntersInFolder;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      linksEntersInFolder = ImbaseHelper.GetLinksEntersInFolder(sessionKeeper.Session, folderID);
    this.LoadAdditionalData();
    return linksEntersInFolder;
  }

  private void LoadAdditionalData()
  {
    List<string> names = new List<string>()
    {
      "GLUE_MATERIAL_GROUPS_TABLE_NAME",
      "GLUE_MATERIAL_GROUPS_COLUMN_NAME",
      "GLUE_TABLE_NAME",
      "GLUE_COLUMN_MATERIAL1",
      "GLUE_COLUMN_MATERIAL2",
      "GLUE_COLUMN_GLUE"
    };
    Dictionary<string, Guid> dictionary = (Dictionary<string, Guid>) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (sessionKeeper.Session.GetCustomService(typeof (IIMHSystemSettingsService)) is IIMHSystemSettingsService customService)
        dictionary = customService.GetObjectGuidsByNames(names);
      if (dictionary != null)
      {
        QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(dictionary["GLUE_MATERIAL_GROUPS_TABLE_NAME"]);
        long tableIdByTableRefId1 = IMHHelper.GetTableIDByTableRefID(objectInfo.ObjectID);
        if (tableIdByTableRefId1 != 0L)
        {
          DataSet tables = TableLoadHelper.GetTables(sessionKeeper.Session, tableIdByTableRefId1, true);
          if (tables != null && tables.Tables.Contains("IMS_DATA"))
          {
            this._materialsTableRefObjectID = objectInfo.ObjectID;
            this._dtMaterials = tables.Tables["IMS_DATA"];
            string name = Convert.ToString((object) dictionary["GLUE_MATERIAL_GROUPS_COLUMN_NAME"]);
            this._materialColName = this._dtMaterials.Columns.Contains(name) ? name : string.Empty;
          }
        }
        long tableIdByTableRefId2 = IMHHelper.GetTableIDByTableRefID(sessionKeeper.Session.GetObjectInfo(dictionary["GLUE_TABLE_NAME"]).ObjectID);
        if (tableIdByTableRefId2 != 0L)
        {
          DataSet tables = TableLoadHelper.GetTables(sessionKeeper.Session, tableIdByTableRefId2, true);
          if (tables != null)
          {
            if (tables.Tables.Contains("IMS_DATA"))
            {
              this._dtGlues = tables.Tables["IMS_DATA"];
              string name1 = Convert.ToString((object) dictionary["GLUE_COLUMN_MATERIAL1"]);
              this._materialColName_1 = this._dtGlues.Columns.Contains(name1) ? name1 : string.Empty;
              string name2 = Convert.ToString((object) dictionary["GLUE_COLUMN_MATERIAL2"]);
              this._materialColName_2 = this._dtGlues.Columns.Contains(name2) ? name2 : string.Empty;
              string name3 = Convert.ToString((object) dictionary["GLUE_COLUMN_GLUE"]);
              this._glueColName = this._dtGlues.Columns.Contains(name3) ? name3 : string.Empty;
            }
          }
        }
      }
    }
    this._dataLoaded = this._dtMaterials != null && this._dtGlues != null && !string.IsNullOrEmpty(this._materialColName) && !string.IsNullOrEmpty(this._materialColName_1) && !string.IsNullOrEmpty(this._materialColName_2) && !string.IsNullOrEmpty(this._glueColName);
  }

  private void ViewFavourites(long tblRefID, long recID, string caption)
  {
    using (MaterialFavourites materialFavourites = new MaterialFavourites(Consts.IMHGluesHandbookNodeGuid, 0L, tblRefID, recID, caption))
    {
      if (materialFavourites.ShowDialog() != DialogResult.OK)
        return;
      FavouriteData data = materialFavourites.Data;
      this.GoToNode(data.TableRefID, data.TableRefID, data.RecordID);
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

  private Dictionary<string, string> GetMaterials()
  {
    Dictionary<string, string> materials = (Dictionary<string, string>) null;
    if (this._dtMaterials.Rows.Count > 0)
    {
      materials = new Dictionary<string, string>(this._dtMaterials.Rows.Count);
      foreach (DataRow row in (InternalDataCollectionBase) this._dtMaterials.Rows)
      {
        long int64 = Convert.ToInt64(row["F_KEY"]);
        string str = Convert.ToString(row[this._materialColName]);
        string key = ImbaseHelper.MakeInternalImbaseKey(this._materialsTableRefObjectID, int64);
        materials.Add(key, str);
      }
    }
    return materials;
  }

  private Dictionary<string, string> Search(string materialKey1, string materialKey2)
  {
    if (!this._dtGlues.Columns.Contains(this._glueColName))
      return (Dictionary<string, string>) null;
    Dictionary<string, string> dictionary;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      bool isGuidKey1;
      string str1 = ImbaseHelper.ConvertImbaseKey(sessionKeeper.Session, materialKey1, out isGuidKey1);
      string str2;
      string str3;
      if (isGuidKey1)
      {
        str2 = materialKey1;
        str3 = str1;
      }
      else
      {
        str2 = str1;
        str3 = materialKey1;
      }
      bool isGuidKey2;
      string str4 = ImbaseHelper.ConvertImbaseKey(sessionKeeper.Session, materialKey2, out isGuidKey2);
      string str5;
      string str6;
      if (isGuidKey2)
      {
        str5 = materialKey2;
        str6 = str4;
      }
      else
      {
        str5 = str1;
        str6 = materialKey2;
      }
      List<DataRow> dataRowList = new List<DataRow>();
      dataRowList.AddRange((IEnumerable<DataRow>) this._dtGlues.Select($"([{this._materialColName_1}]='{str2}' OR [{this._materialColName_1}]='{str3}') AND ([{this._materialColName_2}]='{str5}' OR [{this._materialColName_2}]='{str6}')"));
      dataRowList.AddRange((IEnumerable<DataRow>) this._dtGlues.Select($"([{this._materialColName_2}]='{str2}' OR [{this._materialColName_2}]='{str3}') AND ([{this._materialColName_1}]='{str5}' OR [{this._materialColName_1}]='{str6}')"));
      if (dataRowList.Count <= 0)
        return (Dictionary<string, string>) null;
      dictionary = new Dictionary<string, string>(dataRowList.Count);
      List<string> keyValues = new List<string>(dataRowList.Count);
      foreach (DataRow dataRow in dataRowList)
      {
        string str7 = Convert.ToString(dataRow[this._glueColName]);
        if (!keyValues.Contains(str7))
          keyValues.Add(str7);
      }
      if (sessionKeeper.Session.GetCustomService(typeof (IImbaseServer)) is IImbaseServer customService)
        dictionary = customService.NameRecordReferences(sessionKeeper.Session.SessionGUID, keyValues);
    }
    return dictionary;
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (IMHGluesViewCtrl));
    this._lv = new ListView();
    this._caption = new System.Windows.Forms.ColumnHeader();
    this._tsHSeparator1 = new ToolStripSeparator();
    this._cmFavourites = new ToolStripMenuItem();
    this._cmAddFavourite = new ToolStripMenuItem();
    this._tsHSeparator2 = new ToolStripSeparator();
    this._cmSearch = new ToolStripMenuItem();
    this._contextMenu = new ContextMenuStrip(this.components);
    this._miCollapse = new ToolStripMenuItem();
    this._miExpand = new ToolStripMenuItem();
    this._pnl = new Panel();
    this._btnDel = new Button();
    this._btnAdd = new Button();
    this._propsPage = new MaterialPropertiesPage();
    this._splt.BeginInit();
    this._splt.Panel1.SuspendLayout();
    this._splt.Panel2.SuspendLayout();
    this._splt.SuspendLayout();
    this._contextMenu.SuspendLayout();
    this._pnl.SuspendLayout();
    this.SuspendLayout();
    this._splt.Panel1.Controls.Add((Control) this._lv);
    this._splt.Panel2.Controls.Add((Control) this._propsPage);
    this._splt.Panel2.Controls.Add((Control) this._pnl);
    this._lv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[1]
    {
      this._caption
    });
    componentResourceManager.ApplyResources((object) this._lv, "_lv");
    this._lv.FullRowSelect = true;
    this._lv.HeaderStyle = ColumnHeaderStyle.None;
    this._lv.HideSelection = false;
    this._lv.MultiSelect = false;
    this._lv.Name = "_lv";
    this._lv.UseCompatibleStateImageBehavior = false;
    this._lv.View = View.Details;
    this._lv.SizeChanged += new EventHandler(this.On_lv_SizeChanged);
    this._lv.MouseDoubleClick += new MouseEventHandler(this._lv_MouseDoubleClick);
    componentResourceManager.ApplyResources((object) this._caption, "_caption");
    this._tsHSeparator1.Name = "_tsHSeparator1";
    componentResourceManager.ApplyResources((object) this._tsHSeparator1, "_tsHSeparator1");
    componentResourceManager.ApplyResources((object) this._cmFavourites, "_cmFavourites");
    this._cmFavourites.Name = "_cmFavourites";
    this._cmFavourites.Click += new EventHandler(((IMHViewCtrlBase) this).FavouritesClick);
    componentResourceManager.ApplyResources((object) this._cmAddFavourite, "_cmAddFavourite");
    this._cmAddFavourite.Name = "_cmAddFavourite";
    this._cmAddFavourite.Click += new EventHandler(((IMHViewCtrlBase) this).AddFavouriteClick);
    this._tsHSeparator2.Name = "_tsHSeparator2";
    componentResourceManager.ApplyResources((object) this._tsHSeparator2, "_tsHSeparator2");
    componentResourceManager.ApplyResources((object) this._cmSearch, "_cmSearch");
    this._cmSearch.Name = "_cmSearch";
    this._cmSearch.Click += new EventHandler(((IMHViewCtrlBase) this).SearchClick);
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
    this._pnl.Controls.Add((Control) this._btnDel);
    this._pnl.Controls.Add((Control) this._btnAdd);
    componentResourceManager.ApplyResources((object) this._pnl, "_pnl");
    this._pnl.Name = "_pnl";
    componentResourceManager.ApplyResources((object) this._btnDel, "_btnDel");
    this._btnDel.Name = "_btnDel";
    this._btnDel.UseVisualStyleBackColor = true;
    this._btnDel.Click += new EventHandler(this.On_DelProps_Click);
    componentResourceManager.ApplyResources((object) this._btnAdd, "_btnAdd");
    this._btnAdd.Name = "_btnAdd";
    this._btnAdd.UseVisualStyleBackColor = true;
    this._btnAdd.Click += new EventHandler(this.On_AddProps_Click);
    this._propsPage.ContextMenuStrip = this._contextMenu;
    componentResourceManager.ApplyResources((object) this._propsPage, "_propsPage");
    this._propsPage.ImbaseKey = "";
    this._propsPage.Name = "_propsPage";
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Name = nameof (IMHGluesViewCtrl);
    this.Controls.SetChildIndex((Control) this._pnlFormula, 0);
    this.Controls.SetChildIndex((Control) this._splt, 0);
    this._splt.Panel1.ResumeLayout(false);
    this._splt.Panel2.ResumeLayout(false);
    this._splt.EndInit();
    this._splt.ResumeLayout(false);
    this._contextMenu.ResumeLayout(false);
    this._pnl.ResumeLayout(false);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
