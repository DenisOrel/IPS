// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.MaterialSearchForm
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using ImSSP;
using Intermech.Imbase;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Imbase;
using Intermech.Interfaces.MaterialsHandbook;
using Intermech.Kernel.Search;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class MaterialSearchForm : BaseSearchForm
{
  private bool _isMaterialPart = true;
  private long _tableRefID;
  private long _recID = -1;
  private Guid _baseMaterialAttr = Guid.Empty;
  private IContainer components;
  private Panel _pnlAssortmentSearch;
  private DataGridView _dgv;
  private Label _lbConditions;
  private ComboBox _cmbClass;
  private Label _lbCkass;
  private DataSet _ds;
  private DataTable conditions;
  private DataColumn dataColumn2;
  private DataColumn dataColumn3;
  private DataColumn dataColumn4;
  private DataTable condsMap;
  private DataColumn dataColumn5;
  private DataColumn dataColumn6;
  private DataGridViewTextBoxColumn colField;
  private DataGridViewComboBoxColumn colCondition;
  private DataGridViewTextBoxColumn colData;

  public long aTableRefID { get; private set; }

  public bool IsMaterial { get; private set; }

  public long RecID => this._recID;

  public long TableRefID => this._tableRefID;

  public MaterialSearchForm(bool isMaterialPart)
  {
    this.InitializeComponent();
    this.aTableRefID = 0L;
    this._isMaterialPart = isMaterialPart;
    this.IsMaterial = true;
    this._cmbSearchIn.Items.Add((object) LocalizationHolder.rm.GetString("IMH_Search_Material"));
    this._cmbSearchIn.Items.Add((object) LocalizationHolder.rm.GetString("IMH_Search_Assortment"));
    if ((ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService).GetCustomService(typeof (IIMHSystemSettingsService)) is IIMHSystemSettingsService customService)
      this._baseMaterialAttr = customService.GetObjectGuidByName("BASE_MATERIAL_ATTR");
    this.FillConditionsMap();
    this.LoadAssortmentSearchData();
    this._pnl.Controls.Add((Control) this._pnlAssortmentSearch);
    if (isMaterialPart)
    {
      this._cmbSearchIn.SelectedIndex = 0;
      this.Text = LocalizationHolder.rm.GetString("IMH_Search_Material_Caption");
    }
    else
    {
      this._cmbSearchIn.SelectedIndex = 1;
      this.Text = LocalizationHolder.rm.GetString("IMH_Search_Assortment_Caption");
    }
    this.splitContainer1.SuspendLayout();
    this._pnlAssortmentSearch.Location = this._pnlMaterialSearch.Location;
    this._pnlAssortmentSearch.Visible = !(this._pnlMaterialSearch.Visible = isMaterialPart);
    this.splitContainer1.ResumeLayout();
    this.IsMaterial = this._cmbSearchIn.SelectedIndex == 0;
  }

  private void On_cmbClass_SelectedIndexChanged(object sender, EventArgs e)
  {
    if (this._cmbClass.SelectedItem == null)
      return;
    this._ds.Tables["Conditions"].Clear();
    DataTable data = (this._cmbClass.SelectedItem as MaterialSearchForm.ComboBoxItem).Data;
    if (data == null)
      return;
    foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
      this._ds.Tables["Conditions"].Rows.Add(row.ItemArray);
  }

  private void On_dgv_DataError(object sender, DataGridViewDataErrorEventArgs e)
  {
    if (this._dgv.Focused)
    {
      int num = (int) MessageBox.Show((IWin32Window) this, e.Exception.Message, LocalizationHolder.rm.GetString("IMH_Error"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }
    else
      e.ThrowException = false;
  }

  protected override void On_btnSearch_Click(object sender, EventArgs e)
  {
    if (this._isMaterialPart)
    {
      if (this.IsMaterial)
        this.SearchMaterialInMaterialPart();
      else
        this.SearchAssortmentInMaterialPart();
    }
    else if (this.IsMaterial)
      this.SearchMaterialInAssortmentPart();
    else
      this.SearchAssortmentInAssortmentPart();
  }

  protected override void On_lvResult_DoubleClick(object sender, EventArgs e)
  {
    if (this._lvResult.SelectedItems.Count <= 0)
      return;
    MaterialSearchForm.LvItem tag = this._lvResult.SelectedItems[0].Tag as MaterialSearchForm.LvItem;
    this._tableRefID = tag.TableRefID;
    this._recID = tag.RecID;
    this.aTableRefID = tag.A_TableRefID;
    if (!this._isMaterialPart && !this.IsMaterial)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject objectActualCopy = sessionKeeper.Session.GetObjectActualCopy(this.aTableRefID, false);
        if (objectActualCopy != null)
        {
          if (this._baseMaterialAttr != Guid.Empty)
          {
            IDBAttribute attributeByGuid = objectActualCopy.GetAttributeByGuid(this._baseMaterialAttr);
            if (attributeByGuid != null)
            {
              string asString = attributeByGuid.AsString;
              ImbaseHelper.TryParseRecordReference(sessionKeeper.Session, asString, out this._tableRefID, out this._recID);
            }
          }
        }
      }
    }
    this.DialogResult = DialogResult.OK;
    this.Close();
  }

  protected override void On_cmbSearchIn_SelectedIndexChanged(object sender, EventArgs e)
  {
    int selectedIndex = (sender as ComboBox).SelectedIndex;
    this.splitContainer1.SuspendLayout();
    if (selectedIndex == 0)
    {
      this._pnlAssortmentSearch.Visible = false;
      this._pnlMaterialSearch.Visible = true;
    }
    else
    {
      this._pnlAssortmentSearch.Visible = true;
      this._pnlMaterialSearch.Visible = false;
    }
    this.splitContainer1.ResumeLayout();
    this.IsMaterial = selectedIndex == 0;
  }

  private void CreateResultItems(List<MaterialSearchForm.LvItem> items, bool isNew)
  {
    if (isNew)
      this._lvResult.Items.Clear();
    if (items == null || items.Count <= 0)
      return;
    this._lvResult.SuspendLayout();
    try
    {
      foreach (MaterialSearchForm.LvItem lvItem in items)
        this._lvResult.Items.Add(new ListViewItem(new string[2]
        {
          lvItem.Text,
          lvItem.Path
        })
        {
          Tag = (object) lvItem
        });
    }
    finally
    {
      this._lvResult.ResumeLayout();
    }
  }

  private void FillConditionsMap()
  {
    this.condsMap.Clear();
    this.condsMap.Rows.Add((object) RelationalOperators.None, (object) "");
    this.condsMap.Rows.Add((object) RelationalOperators.Equal, (object) LocalizationHolder.rm.GetString(sc_14564.ssp_imbase_14565()));
    this.condsMap.Rows.Add((object) RelationalOperators.NotEqual, (object) LocalizationHolder.rm.GetString("IMH.NotEqual"));
    this.condsMap.Rows.Add((object) RelationalOperators.Greater, (object) LocalizationHolder.rm.GetString("IMH.Great"));
    this.condsMap.Rows.Add((object) RelationalOperators.GreaterOrEqual, (object) LocalizationHolder.rm.GetString("IMH.GreatOrEqual"));
    this.condsMap.Rows.Add((object) RelationalOperators.Less, (object) LocalizationHolder.rm.GetString("IMH.Less"));
    this.condsMap.Rows.Add((object) RelationalOperators.LessOrEqual, (object) LocalizationHolder.rm.GetString("IMH.LessOrEqual"));
  }

  private DataTable GetImbaseHierarchyTable(long[] IDs)
  {
    DataTable imbaseHierarchyTable = (DataTable) null;
    if (IDs != null && IDs.Length != 0)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        if (sessionKeeper.Session.GetCustomService(typeof (IImbaseServer)) is IImbaseServer customService)
        {
          imbaseHierarchyTable = customService.GetFoldersForObjects(sessionKeeper.Session.SessionGUID, IDs, (long[]) null);
          imbaseHierarchyTable.DefaultView.Sort = "F_PATH ASC";
          imbaseHierarchyTable = imbaseHierarchyTable.DefaultView.ToTable();
        }
      }
    }
    return imbaseHierarchyTable;
  }

  private Dictionary<long, string> GetMaterialNamesForAssortmentTables(
    List<long> assortmentTableRefIDs)
  {
    Dictionary<long, string> assortmentTables = (Dictionary<long, string>) null;
    if (this._baseMaterialAttr != Guid.Empty && assortmentTableRefIDs != null && assortmentTableRefIDs.Count > 0)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObjectCollection objectCollection = sessionKeeper.Session.GetObjectCollection(Intermech.Imbase.Consts.ImbaseTableRefTypeGUID);
        if (objectCollection != null)
        {
          int attributeTypeId = MetaDataHelper.GetAttributeTypeID(this._baseMaterialAttr);
          DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[2]
          {
            new ConditionStructure(attributeTypeId, RelationalOperators.NotEmpty, (object) null, LogicalOperators.AND, 0, false),
            new ConditionStructure(-2, RelationalOperators.In, (object) assortmentTableRefIDs.ToArray(), LogicalOperators.NONE, 0, false)
          }, new List<ColumnDescriptor>()
          {
            new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
            new ColumnDescriptor((object) attributeTypeId, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0)
          }.ToArray());
          DataTable dataTable = objectCollection.Select(paramSet);
          dataTable.Columns[Convert.ToString(-2)].ColumnName = "TableRefID";
          dataTable.Columns[Convert.ToString(attributeTypeId)].ColumnName = "Keys";
          if (dataTable.Rows.Count > 0)
          {
            string empty = string.Empty;
            Dictionary<long, string> dictionary1 = new Dictionary<long, string>(dataTable.Rows.Count);
            List<string> keys = new List<string>(dataTable.Rows.Count);
            foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
            {
              long int64 = Convert.ToInt64(row["TableRefID"]);
              if (!dictionary1.ContainsKey(int64))
              {
                string str = Convert.ToString(row["Keys"]);
                dictionary1.Add(int64, str);
                if (!keys.Contains(str))
                  keys.Add(str);
              }
            }
            Dictionary<string, string> dictionary2 = this.RenameKeys(keys);
            if (dictionary2 != null)
            {
              long[] array = new long[dictionary1.Count];
              dictionary1.Keys.CopyTo(array, 0);
              foreach (long key1 in array)
              {
                string key2 = dictionary1[key1];
                if (dictionary2.ContainsKey(key2))
                  dictionary1[key1] = dictionary2[key2];
              }
              assortmentTables = dictionary1;
            }
          }
        }
      }
    }
    return assortmentTables;
  }

  private SearchesAccuracy GetSearchesAccuracy()
  {
    SearchesAccuracy searchesAccuracy = SearchesAccuracy.Exact;
    if (this._rbBeg.Checked)
      searchesAccuracy = SearchesAccuracy.Start;
    else if (this._rbEntry.Checked)
      searchesAccuracy = SearchesAccuracy.Сontain;
    else if (this._rbEnd.Checked)
      searchesAccuracy = SearchesAccuracy.End;
    else if (this._rbTemplate.Checked)
      searchesAccuracy = SearchesAccuracy.Template;
    return searchesAccuracy;
  }

  private void LoadAssortmentSearchData()
  {
    if ((ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService).GetCustomService(typeof (IIMHSystemSettingsService)) is IIMHSystemSettingsService customService)
    {
      List<IMHAssortmentClass> assortmentSearchSettings = customService.GetSystemSettings().AssortmentSearchSettings;
      if (assortmentSearchSettings != null && assortmentSearchSettings.Count > 0)
      {
        foreach (IMHAssortmentClass imhAssortmentClass in assortmentSearchSettings)
        {
          DataTable dt = this._ds.Tables["Conditions"].Clone();
          this._cmbClass.Items.Add((object) new MaterialSearchForm.ComboBoxItem(imhAssortmentClass.Name, imhAssortmentClass.Parameters, dt));
        }
      }
    }
    if (this._cmbClass.Items.Count <= 0)
      return;
    this._cmbClass.SelectedIndex = 0;
  }

  private Dictionary<string, string> RenameKeys(List<string> keys)
  {
    Dictionary<string, string> dictionary = (Dictionary<string, string>) null;
    if (keys != null)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
        dictionary = sessionKeeper.Session.GetCustomService(typeof (IImbaseServer)) is IImbaseServer customService ? customService.NameRecordReferences(sessionKeeper.Session.SessionGUID, keys) : (Dictionary<string, string>) null;
    }
    return dictionary == null || dictionary.Count <= 0 ? (Dictionary<string, string>) null : dictionary;
  }

  private List<long> SearchAssortment()
  {
    List<long> longList = (List<long>) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetCustomService(typeof (IIMHIndexingService)) is IIMHIndexingService customService))
      {
        ExceptionHelper.ExceptionService.ShowException(new Exception(LocalizationHolder.rm.GetString("IMH_Search_GetIndexes_Error")));
      }
      else
      {
        DataTable table = this._ds.Tables["Conditions"];
        List<ConditionClass> conditions = new List<ConditionClass>(table.Rows.Count);
        foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
        {
          object obj = row["F_COND"];
          if (obj != DBNull.Value && obj != null && row["F_DATA"] != DBNull.Value && row["F_DATA"] != null)
          {
            RelationalOperators relOperator = (RelationalOperators) obj;
            if (relOperator != RelationalOperators.None)
              conditions.Add(new ConditionClass(Convert.ToString(row["F_NAME"]), relOperator, row["F_DATA"]));
          }
        }
        if (conditions.Count > 0)
        {
          long objectIdByConstName = IMHHelper.GetObjectIDByConstName("ASSORTMENT_FOLDER_NAME");
          longList = customService.SearchAssortmentData(sessionKeeper.Session.SessionGUID, objectIdByConstName, (this._cmbClass.SelectedItem as MaterialSearchForm.ComboBoxItem).ClassName, conditions);
        }
      }
    }
    return longList;
  }

  private void SearchAssortmentInAssortmentPart()
  {
    List<long> assortmentTableRefIDs = this.SearchAssortment();
    bool flag = true;
    if (assortmentTableRefIDs != null && assortmentTableRefIDs.Count > 0)
    {
      Dictionary<long, string> assortmentTables = this.GetMaterialNamesForAssortmentTables(assortmentTableRefIDs);
      if (assortmentTables != null && assortmentTables.Count > 0)
      {
        Dictionary<long, MaterialSearchForm.LvItem> dictionary1 = new Dictionary<long, MaterialSearchForm.LvItem>(assortmentTables.Count);
        foreach (KeyValuePair<long, string> keyValuePair in assortmentTables)
          dictionary1.Add(keyValuePair.Key, new MaterialSearchForm.LvItem(0L, -1L, keyValuePair.Key, keyValuePair.Value, string.Empty));
        long[] numArray = new long[dictionary1.Count];
        dictionary1.Keys.CopyTo(numArray, 0);
        DataTable imbaseHierarchyTable = this.GetImbaseHierarchyTable(numArray);
        if (imbaseHierarchyTable != null)
        {
          Dictionary<string, string> dictionary2 = new Dictionary<string, string>(imbaseHierarchyTable.Rows.Count);
          string classifFolderKey = IMHHelper.GetClassifFolderKey("ASSORTMENT_FOLDER_NAME");
          dictionary2.Add(classifFolderKey, LocalizationHolder.rm.GetString("IMH_Search_Assortment_RootFolder"));
          string empty1 = string.Empty;
          string empty2 = string.Empty;
          string empty3 = string.Empty;
          foreach (DataRow row in (InternalDataCollectionBase) imbaseHierarchyTable.Rows)
          {
            string key1 = Convert.ToString(row["F_PATH"]);
            if (key1.Length > classifFolderKey.Length)
            {
              string key2 = key1.Substring(0, key1.Length - 2);
              long int64 = Convert.ToInt64(row["F_OBJECT_ID"]);
              if (dictionary1.ContainsKey(int64))
                dictionary1[int64].Path = $"{dictionary2[key2]}\\";
              string str = $"{dictionary2[key2]}\\{Convert.ToString(row["CAPTION"])}";
              dictionary2.Add(key1, str);
            }
          }
          MaterialSearchForm.LvItem[] lvItemArray = new MaterialSearchForm.LvItem[dictionary1.Count];
          dictionary1.Values.CopyTo(lvItemArray, 0);
          this.CreateResultItems(new List<MaterialSearchForm.LvItem>((IEnumerable<MaterialSearchForm.LvItem>) lvItemArray), true);
          flag = false;
        }
      }
    }
    if (!flag)
      return;
    this._lvResult.Items.Clear();
  }

  private void SearchAssortmentInMaterialPart()
  {
    List<long> tableRefIDs = this.SearchAssortment();
    if (tableRefIDs == null || tableRefIDs.Count <= 0)
      return;
    Dictionary<long, List<MaterialSearchForm.LvItem>> dictionary1 = this.SearchMaterialsTable(IMHHelper.GetClassifFolderKey("ASSORTMENT_FOLDER_NAME"), tableRefIDs);
    if (dictionary1 == null || dictionary1.Count <= 0)
      return;
    long[] numArray = new long[dictionary1.Count];
    dictionary1.Keys.CopyTo(numArray, 0);
    DataTable imbaseHierarchyTable = this.GetImbaseHierarchyTable(numArray);
    if (imbaseHierarchyTable == null)
      return;
    Dictionary<string, string> dictionary2 = new Dictionary<string, string>(imbaseHierarchyTable.Rows.Count);
    string classifFolderKey = IMHHelper.GetClassifFolderKey("BASE_MATERIALS_CTL");
    dictionary2.Add(classifFolderKey, LocalizationHolder.rm.GetString("IMH_Search_Materials_RootFolder"));
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    string empty3 = string.Empty;
    foreach (DataRow row in (InternalDataCollectionBase) imbaseHierarchyTable.Rows)
    {
      string key1 = Convert.ToString(row["F_PATH"]);
      if (key1.Length > classifFolderKey.Length)
      {
        string key2 = key1.Substring(0, key1.Length - 2);
        long int64 = Convert.ToInt64(row["F_OBJECT_ID"]);
        if (dictionary1.ContainsKey(int64))
        {
          foreach (MaterialSearchForm.LvItem lvItem in dictionary1[int64])
            lvItem.Path = $"{dictionary2[key2]}\\";
        }
        string str = $"{dictionary2[key2]}\\{Convert.ToString(row["CAPTION"])}";
        dictionary2.Add(key1, str);
      }
    }
    int num = 0;
    foreach (KeyValuePair<long, List<MaterialSearchForm.LvItem>> keyValuePair in dictionary1)
      this.CreateResultItems(keyValuePair.Value, num++ == 0);
  }

  private DataTable SearchAssortmentTables(string classifKeys, string[] keys)
  {
    DataTable dataTable = (DataTable) null;
    if (keys != null && keys.Length != 0 && !string.IsNullOrEmpty(classifKeys))
    {
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
              new ConditionStructure(Intermech.Imbase.Consts.ClassifFolderKeyAttId, RelationalOperators.StartString, (object) classifKeys, LogicalOperators.AND, 0, false),
              new ConditionStructure(attributeTypeId, RelationalOperators.In, (object) keys, LogicalOperators.NONE, 0, false)
            }, new List<ColumnDescriptor>()
            {
              new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
              new ColumnDescriptor((object) attributeTypeId, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0)
            }.ToArray());
            dataTable = objectCollection.Select(paramSet);
            dataTable.Columns[Convert.ToString(-2)].ColumnName = "TableRefID";
            dataTable.Columns[Convert.ToString(attributeTypeId)].ColumnName = "Keys";
          }
        }
      }
    }
    return dataTable;
  }

  private DataTable SearchMaterials()
  {
    DataTable dataTable = (DataTable) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetCustomService(typeof (IIMHIndexingService)) is IIMHIndexingService customService1))
      {
        ExceptionHelper.ExceptionService.ShowException(new Exception(LocalizationHolder.rm.GetString("IMH_Search_GetIndexes_Error")));
      }
      else
      {
        long sourceID = 0;
        if (sessionKeeper.Session.GetCustomService(typeof (IIMHSystemSettingsService)) is IIMHSystemSettingsService customService)
        {
          Guid objectGuidByName = customService.GetObjectGuidByName("BASE_MATERIALS_CTL");
          sourceID = sessionKeeper.Session.GetObjectInfo(objectGuidByName).ObjectID;
        }
        Guid attrGuid = new Guid("cad00020-306c-11d8-b4e9-00304f19f545");
        string[] colsNames = new string[3]
        {
          IndexesField.F_TEXT,
          IndexesField.F_LINK_ID,
          IndexesField.F_TABKEY
        };
        SearchesAccuracy searchesAccuracy = this.GetSearchesAccuracy();
        if (sourceID != 0L)
          dataTable = customService1.Search(sessionKeeper.Session.SessionGUID, sourceID, attrGuid, colsNames, this._txtSearch.Text, searchesAccuracy);
      }
    }
    return dataTable;
  }

  private void SearchMaterialInAssortmentPart()
  {
    DataTable dataTable1 = this.SearchMaterials();
    if (dataTable1 == null)
      return;
    Dictionary<string, string> dictionary1 = new Dictionary<string, string>(dataTable1.Rows.Count);
    string empty1 = string.Empty;
    foreach (DataRow row in (InternalDataCollectionBase) dataTable1.Rows)
    {
      long int64_1 = Convert.ToInt64(row[IndexesField.F_LINK_ID]);
      long int64_2 = Convert.ToInt64(row[IndexesField.F_TABKEY]);
      if (int64_2 != -1L)
      {
        string key = ImbaseHelper.MakeInternalImbaseKey(int64_1, int64_2);
        if (dictionary1.ContainsKey(key))
          dictionary1[key] = Convert.ToString(row[IndexesField.F_TEXT]);
        else
          dictionary1.Add(ImbaseHelper.MakeInternalImbaseKey(int64_1, int64_2), Convert.ToString(row[IndexesField.F_TEXT]));
      }
    }
    if (dictionary1.Count <= 0)
      return;
    string[] strArray = new string[dictionary1.Count];
    dictionary1.Keys.CopyTo(strArray, 0);
    string classifFolderKey = IMHHelper.GetClassifFolderKey("ASSORTMENT_FOLDER_NAME");
    DataTable dataTable2 = this.SearchAssortmentTables(classifFolderKey, strArray);
    if (dataTable2 == null)
      return;
    Dictionary<long, MaterialSearchForm.LvItem> dictionary2 = new Dictionary<long, MaterialSearchForm.LvItem>(dataTable2.Rows.Count);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      string empty2 = string.Empty;
      foreach (DataRow row in (InternalDataCollectionBase) dataTable2.Rows)
      {
        long int64 = Convert.ToInt64(row["TableRefID"]);
        string str = Convert.ToString(row["Keys"]);
        if (!string.IsNullOrEmpty(str))
        {
          long linkId = 0;
          long recordId = -1;
          if (ImbaseHelper.TryParseRecordReference(sessionKeeper.Session, str, out linkId, out recordId))
            dictionary2.Add(int64, new MaterialSearchForm.LvItem(linkId, recordId, int64, dictionary1[str], string.Empty));
        }
      }
    }
    if (dictionary2.Count <= 0)
      return;
    long[] numArray = new long[dictionary2.Count];
    dictionary2.Keys.CopyTo(numArray, 0);
    DataTable imbaseHierarchyTable = this.GetImbaseHierarchyTable(numArray);
    if (imbaseHierarchyTable == null)
      return;
    Dictionary<string, string> dictionary3 = new Dictionary<string, string>(imbaseHierarchyTable.Rows.Count);
    dictionary3.Add(classifFolderKey, LocalizationHolder.rm.GetString("IMH_Search_Assortment_RootFolder"));
    string empty3 = string.Empty;
    string empty4 = string.Empty;
    string empty5 = string.Empty;
    foreach (DataRow row in (InternalDataCollectionBase) imbaseHierarchyTable.Rows)
    {
      string key1 = Convert.ToString(row["F_PATH"]);
      if (key1.Length > classifFolderKey.Length)
      {
        string key2 = key1.Substring(0, key1.Length - 2);
        long int64 = Convert.ToInt64(row["F_OBJECT_ID"]);
        if (dictionary2.ContainsKey(int64))
          dictionary2[int64].Path = $"{dictionary3[key2]}\\";
        string str = $"{dictionary3[key2]}\\{Convert.ToString(row["CAPTION"])}";
        dictionary3.Add(key1, str);
      }
    }
    MaterialSearchForm.LvItem[] lvItemArray = new MaterialSearchForm.LvItem[dictionary2.Count];
    dictionary2.Values.CopyTo(lvItemArray, 0);
    this.CreateResultItems(new List<MaterialSearchForm.LvItem>((IEnumerable<MaterialSearchForm.LvItem>) lvItemArray), true);
  }

  private void SearchMaterialInMaterialPart()
  {
    DataTable dataTable = this.SearchMaterials();
    if (dataTable == null)
      return;
    List<MaterialSearchForm.LvItem> items = new List<MaterialSearchForm.LvItem>(dataTable.Rows.Count);
    Dictionary<long, Dictionary<long, string>> dictionary1 = new Dictionary<long, Dictionary<long, string>>(dataTable.Rows.Count);
    foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
    {
      long int64_1 = Convert.ToInt64(row[IndexesField.F_LINK_ID]);
      long int64_2 = Convert.ToInt64(row[IndexesField.F_TABKEY]);
      if (int64_2 != -1L)
      {
        if (dictionary1.ContainsKey(int64_1))
        {
          if (dictionary1[int64_1].ContainsKey(int64_2))
            dictionary1[int64_1][int64_2] = Convert.ToString(row[IndexesField.F_TEXT]);
          else
            dictionary1[int64_1].Add(int64_2, Convert.ToString(row[IndexesField.F_TEXT]));
        }
        else
        {
          Dictionary<long, string> dictionary2 = new Dictionary<long, string>()
          {
            {
              int64_2,
              Convert.ToString(row[IndexesField.F_TEXT])
            }
          };
          dictionary1.Add(int64_1, dictionary2);
        }
      }
    }
    if (dictionary1.Count > 0)
    {
      long[] numArray = new long[dictionary1.Count];
      dictionary1.Keys.CopyTo(numArray, 0);
      DataTable imbaseHierarchyTable = this.GetImbaseHierarchyTable(numArray);
      if (imbaseHierarchyTable != null)
      {
        Dictionary<string, string> dictionary3 = new Dictionary<string, string>(imbaseHierarchyTable.Rows.Count);
        string classifFolderKey = IMHHelper.GetClassifFolderKey("BASE_MATERIALS_CTL");
        dictionary3.Add(classifFolderKey, LocalizationHolder.rm.GetString("IMH_Search_Materials_RootFolder"));
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        string empty3 = string.Empty;
        foreach (DataRow row in (InternalDataCollectionBase) imbaseHierarchyTable.Rows)
        {
          string key1 = Convert.ToString(row["F_PATH"]);
          if (key1.Length > classifFolderKey.Length)
          {
            string key2 = key1.Substring(0, key1.Length - 2);
            long int64 = Convert.ToInt64(row["F_OBJECT_ID"]);
            if (dictionary1.ContainsKey(int64))
            {
              foreach (KeyValuePair<long, string> keyValuePair in dictionary1[int64])
                items.Add(new MaterialSearchForm.LvItem(int64, keyValuePair.Key, 0L, keyValuePair.Value, $"{dictionary3[key2]}\\"));
            }
            string str = $"{dictionary3[key2]}\\{Convert.ToString(row["CAPTION"])}";
            dictionary3.Add(key1, str);
          }
        }
      }
    }
    this.CreateResultItems(items, true);
  }

  private Dictionary<long, List<MaterialSearchForm.LvItem>> SearchMaterialsTable(
    string sourceCassifKey,
    List<long> tableRefIDs)
  {
    Dictionary<long, List<MaterialSearchForm.LvItem>> dictionary1 = (Dictionary<long, List<MaterialSearchForm.LvItem>>) null;
    if (tableRefIDs != null && tableRefIDs.Count > 0)
    {
      DataTable baseMaterialAttr = this.GetTableRefWithNotEmptyBaseMaterialAttr(sourceCassifKey, tableRefIDs.ToArray());
      if (baseMaterialAttr != null && baseMaterialAttr.Rows.Count > 0)
      {
        Dictionary<long, List<MaterialSearchForm.LvItem>> dictionary2 = new Dictionary<long, List<MaterialSearchForm.LvItem>>(baseMaterialAttr.Rows.Count);
        List<long> longList = new List<long>();
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          string empty = string.Empty;
          foreach (DataRow row in (InternalDataCollectionBase) baseMaterialAttr.Rows)
          {
            long int64 = Convert.ToInt64(row["TableRefID"]);
            object obj = row["Keys"];
            if (obj != null && obj != DBNull.Value)
            {
              string str = Convert.ToString(obj);
              long linkId = 0;
              long recordId = -1;
              if (ImbaseHelper.TryParseRecordReference(sessionKeeper.Session, str, out linkId, out recordId))
              {
                if (dictionary2.ContainsKey(linkId))
                {
                  dictionary2[linkId].Add(new MaterialSearchForm.LvItem(str, linkId, recordId, int64, string.Empty, string.Empty));
                }
                else
                {
                  List<MaterialSearchForm.LvItem> lvItemList = new List<MaterialSearchForm.LvItem>()
                  {
                    new MaterialSearchForm.LvItem(str, linkId, recordId, int64, string.Empty, string.Empty)
                  };
                  dictionary2.Add(linkId, lvItemList);
                  longList.Add(linkId);
                }
              }
            }
          }
        }
        if (dictionary2.Count > 0 && longList.Count > 0)
        {
          DataTable tableRefFromCatalog = this.GetTableRefFromCatalog(IMHHelper.GetClassifFolderKey("BASE_MATERIALS_CTL"), longList.ToArray());
          if (tableRefFromCatalog != null && tableRefFromCatalog.Rows.Count > 0)
          {
            tableRefIDs.Clear();
            foreach (DataRow row in (InternalDataCollectionBase) tableRefFromCatalog.Rows)
              tableRefIDs.Add(Convert.ToInt64(row["TableRefID"]));
            dictionary1 = new Dictionary<long, List<MaterialSearchForm.LvItem>>(tableRefIDs.Count);
            Dictionary<string, List<MaterialSearchForm.LvItem>> dictionary3 = new Dictionary<string, List<MaterialSearchForm.LvItem>>();
            foreach (KeyValuePair<long, List<MaterialSearchForm.LvItem>> keyValuePair in dictionary2)
            {
              if (tableRefIDs.Contains(keyValuePair.Key))
              {
                dictionary1.Add(keyValuePair.Key, keyValuePair.Value);
                foreach (MaterialSearchForm.LvItem lvItem in keyValuePair.Value)
                {
                  if (dictionary3.ContainsKey(lvItem.Key))
                  {
                    dictionary3[lvItem.Key].Add(lvItem);
                  }
                  else
                  {
                    List<MaterialSearchForm.LvItem> lvItemList = new List<MaterialSearchForm.LvItem>()
                    {
                      lvItem
                    };
                    dictionary3.Add(lvItem.Key, lvItemList);
                  }
                }
              }
            }
            string[] strArray = new string[dictionary3.Count];
            dictionary3.Keys.CopyTo(strArray, 0);
            Dictionary<string, string> dictionary4 = this.RenameKeys(new List<string>((IEnumerable<string>) strArray));
            if (dictionary4 != null)
            {
              foreach (KeyValuePair<string, string> keyValuePair in dictionary4)
              {
                if (dictionary3.ContainsKey(keyValuePair.Key))
                {
                  foreach (MaterialSearchForm.LvItem lvItem in dictionary3[keyValuePair.Key])
                    lvItem.Text = keyValuePair.Value;
                }
              }
            }
          }
        }
      }
    }
    return dictionary1;
  }

  private DataTable GetTableRefWithNotEmptyBaseMaterialAttr(
    string sourceCassifKey,
    long[] tableRefIDs)
  {
    DataTable baseMaterialAttr = (DataTable) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObjectCollection objectCollection = sessionKeeper.Session.GetObjectCollection(Intermech.Imbase.Consts.ImbaseTableRefTypeGUID);
      if (objectCollection != null)
      {
        if (this._baseMaterialAttr != Guid.Empty)
        {
          int attributeTypeId = MetaDataHelper.GetAttributeTypeID(this._baseMaterialAttr);
          DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[3]
          {
            new ConditionStructure(Intermech.Imbase.Consts.ClassifFolderKeyAttId, RelationalOperators.StartString, (object) sourceCassifKey, LogicalOperators.AND, 0, false),
            new ConditionStructure(attributeTypeId, RelationalOperators.NotEmpty, (object) null, LogicalOperators.AND, 0, false),
            new ConditionStructure(-2, RelationalOperators.In, (object) tableRefIDs, LogicalOperators.NONE, 0, false)
          }, new List<ColumnDescriptor>()
          {
            new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
            new ColumnDescriptor((object) attributeTypeId, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0)
          }.ToArray());
          baseMaterialAttr = objectCollection.Select(paramSet);
          baseMaterialAttr.Columns[Convert.ToString(-2)].ColumnName = "TableRefID";
          baseMaterialAttr.Columns[Convert.ToString(attributeTypeId)].ColumnName = "Keys";
        }
      }
    }
    return baseMaterialAttr;
  }

  private DataTable GetTableRefFromCatalog(string sourceCassifKey, long[] tableRefIDs)
  {
    DataTable tableRefFromCatalog = (DataTable) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObjectCollection objectCollection = sessionKeeper.Session.GetObjectCollection(Intermech.Imbase.Consts.ImbaseTableRefTypeGUID);
      if (objectCollection != null)
      {
        DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[2]
        {
          new ConditionStructure(Intermech.Imbase.Consts.ClassifFolderKeyAttId, RelationalOperators.StartString, (object) sourceCassifKey, LogicalOperators.AND, 0, false),
          new ConditionStructure(-2, RelationalOperators.In, (object) tableRefIDs, LogicalOperators.NONE, 0, false)
        }, new List<ColumnDescriptor>()
        {
          new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0)
        }.ToArray());
        tableRefFromCatalog = objectCollection.Select(paramSet);
        tableRefFromCatalog.Columns[Convert.ToString(-2)].ColumnName = "TableRefID";
      }
    }
    return tableRefFromCatalog;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MaterialSearchForm));
    this._pnlAssortmentSearch = new Panel();
    this._dgv = new DataGridView();
    this.colField = new DataGridViewTextBoxColumn();
    this.colCondition = new DataGridViewComboBoxColumn();
    this._ds = new DataSet();
    this.conditions = new DataTable();
    this.dataColumn2 = new DataColumn();
    this.dataColumn3 = new DataColumn();
    this.dataColumn4 = new DataColumn();
    this.condsMap = new DataTable();
    this.dataColumn5 = new DataColumn();
    this.dataColumn6 = new DataColumn();
    this.colData = new DataGridViewTextBoxColumn();
    this._lbConditions = new Label();
    this._cmbClass = new ComboBox();
    this._lbCkass = new Label();
    this.splitContainer1.BeginInit();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.Panel2.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    this._pnlMaterialSearch.SuspendLayout();
    this._pnl.SuspendLayout();
    this._pnlAssortmentSearch.SuspendLayout();
    ((ISupportInitialize) this._dgv).BeginInit();
    this._ds.BeginInit();
    this.conditions.BeginInit();
    this.condsMap.BeginInit();
    this.SuspendLayout();
    this.splitContainer1.Panel1.Controls.Add((Control) this._pnlAssortmentSearch);
    this._pnlAssortmentSearch.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this._pnlAssortmentSearch.Controls.Add((Control) this._dgv);
    this._pnlAssortmentSearch.Controls.Add((Control) this._lbConditions);
    this._pnlAssortmentSearch.Controls.Add((Control) this._cmbClass);
    this._pnlAssortmentSearch.Controls.Add((Control) this._lbCkass);
    this._pnlAssortmentSearch.Location = new Point(0, 266);
    this._pnlAssortmentSearch.Name = "_pnlAssortmentSearch";
    this._pnlAssortmentSearch.Size = new Size(267, 176 /*0xB0*/);
    this._pnlAssortmentSearch.TabIndex = 7;
    this._pnlAssortmentSearch.Visible = false;
    this._dgv.AllowUserToAddRows = false;
    this._dgv.AllowUserToDeleteRows = false;
    this._dgv.AllowUserToResizeRows = false;
    this._dgv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this._dgv.AutoGenerateColumns = false;
    this._dgv.BackgroundColor = SystemColors.Window;
    this._dgv.BorderStyle = BorderStyle.Fixed3D;
    this._dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    this._dgv.Columns.AddRange((DataGridViewColumn) this.colField, (DataGridViewColumn) this.colCondition, (DataGridViewColumn) this.colData);
    this._dgv.DataMember = "Conditions";
    this._dgv.DataSource = (object) this._ds;
    this._dgv.Location = new Point(11, 53);
    this._dgv.Name = "_dgv";
    this._dgv.RowHeadersVisible = false;
    this._dgv.Size = new Size(247, 120);
    this._dgv.TabIndex = 3;
    this._dgv.DataError += new DataGridViewDataErrorEventHandler(this.On_dgv_DataError);
    this.colField.DataPropertyName = "F_NAME";
    this.colField.HeaderText = "Поле";
    this.colField.Name = "colField";
    this.colField.ReadOnly = true;
    this.colCondition.DataPropertyName = "F_COND";
    this.colCondition.DataSource = (object) this._ds;
    this.colCondition.DisplayMember = "CondsMap.F_NAME";
    this.colCondition.HeaderText = "Условие";
    this.colCondition.Name = "colCondition";
    this.colCondition.ValueMember = "CondsMap.F_COND";
    this.colCondition.Width = 60;
    this._ds.DataSetName = "NewDataSet";
    this._ds.Tables.AddRange(new DataTable[2]
    {
      this.conditions,
      this.condsMap
    });
    this.conditions.Columns.AddRange(new DataColumn[3]
    {
      this.dataColumn2,
      this.dataColumn3,
      this.dataColumn4
    });
    this.conditions.TableName = "Conditions";
    this.dataColumn2.ColumnName = "F_NAME";
    this.dataColumn2.DataType = typeof (object);
    this.dataColumn3.ColumnName = "F_COND";
    this.dataColumn3.DataType = typeof (object);
    this.dataColumn4.ColumnName = "F_DATA";
    this.dataColumn4.DataType = typeof (double);
    this.condsMap.Columns.AddRange(new DataColumn[2]
    {
      this.dataColumn5,
      this.dataColumn6
    });
    this.condsMap.Constraints.AddRange(new Constraint[1]
    {
      (Constraint) new UniqueConstraint("Constraint1", new string[1]
      {
        "F_COND"
      }, true)
    });
    this.condsMap.PrimaryKey = new DataColumn[1]
    {
      this.dataColumn5
    };
    this.condsMap.TableName = "CondsMap";
    this.dataColumn5.AllowDBNull = false;
    this.dataColumn5.ColumnName = "F_COND";
    this.dataColumn5.DataType = typeof (object);
    this.dataColumn6.ColumnName = "F_NAME";
    this.colData.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
    this.colData.DataPropertyName = "F_DATA";
    this.colData.HeaderText = "Данные";
    this.colData.Name = "colData";
    this._lbConditions.AutoSize = true;
    this._lbConditions.ImeMode = ImeMode.NoControl;
    this._lbConditions.Location = new Point(8, 37);
    this._lbConditions.Name = "_lbConditions";
    this._lbConditions.Size = new Size(93, 13);
    this._lbConditions.TabIndex = 2;
    this._lbConditions.Text = "Условия поиска:";
    this._cmbClass.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this._cmbClass.DropDownStyle = ComboBoxStyle.DropDownList;
    this._cmbClass.FormattingEnabled = true;
    this._cmbClass.Location = new Point(51, 6);
    this._cmbClass.Name = "_cmbClass";
    this._cmbClass.Size = new Size(207, 21);
    this._cmbClass.TabIndex = 1;
    this._cmbClass.SelectedIndexChanged += new EventHandler(this.On_cmbClass_SelectedIndexChanged);
    this._lbCkass.AutoSize = true;
    this._lbCkass.ImeMode = ImeMode.NoControl;
    this._lbCkass.Location = new Point(8, 9);
    this._lbCkass.Name = "_lbCkass";
    this._lbCkass.Size = new Size(38, 13);
    this._lbCkass.TabIndex = 0;
    this._lbCkass.Text = "Класс";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(804, 482);
    this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
    this.Name = nameof (MaterialSearchForm);
    this.Text = nameof (MaterialSearchForm);
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.Panel2.ResumeLayout(false);
    this.splitContainer1.EndInit();
    this.splitContainer1.ResumeLayout(false);
    this._pnlMaterialSearch.ResumeLayout(false);
    this._pnlMaterialSearch.PerformLayout();
    this._pnl.ResumeLayout(false);
    this._pnl.PerformLayout();
    this._pnlAssortmentSearch.ResumeLayout(false);
    this._pnlAssortmentSearch.PerformLayout();
    ((ISupportInitialize) this._dgv).EndInit();
    this._ds.EndInit();
    this.conditions.EndInit();
    this.condsMap.EndInit();
    this.ResumeLayout(false);
  }

  private new class ComboBoxItem
  {
    internal string ClassName = string.Empty;
    internal DataTable Data;

    internal ComboBoxItem(
      string className,
      Dictionary<string, List<string>> attrAliases,
      DataTable dt)
    {
      this.ClassName = className;
      this.Data = dt;
      this.LoadData(attrAliases);
    }

    public override string ToString() => this.ClassName;

    private void LoadData(Dictionary<string, List<string>> attrAliases)
    {
      if (attrAliases == null)
        return;
      foreach (string key in attrAliases.Keys)
      {
        DataRow row = this.Data.NewRow();
        row["F_NAME"] = (object) key;
        row["F_COND"] = (object) RelationalOperators.None;
        row["F_DATA"] = (object) DBNull.Value;
        this.Data.Rows.Add(row);
      }
    }
  }

  private class LvItem
  {
    internal long TableRefID;
    internal long RecID = -1;
    internal string Text = string.Empty;
    internal string Path = string.Empty;
    internal long A_TableRefID;
    internal string Key = string.Empty;

    internal LvItem(long tableRefID, long recID, long aTableRefID, string text, string path)
    {
      this.TableRefID = tableRefID;
      this.RecID = recID;
      this.A_TableRefID = aTableRefID;
      this.Text = text;
      this.Path = path;
    }

    internal LvItem(
      string key,
      long tableRefID,
      long recID,
      long aTableRefID,
      string text,
      string path)
      : this(tableRefID, recID, aTableRefID, text, path)
    {
      this.Key = key;
    }
  }
}
