// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.IMHVarnishViewCtrl
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
using Intermech.MaterialsHandbook.Controls.MaterialProperties;
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

public class IMHVarnishViewCtrl : IMHViewCtrlBase
{
  private int _imgIndex = -1;
  private bool _readOnly;
  private long _folderId;
  private VarnishHelper _helper = new VarnishHelper();
  private MainSettingsDataProvider _dataProvider = new MainSettingsDataProvider();
  private VarnishItem _currentItem;
  private IContainer components;
  private ContextMenuStrip _contextMenu;
  private ToolStripMenuItem _miCollapse;
  private ToolStripMenuItem _miExpand;
  private TabControl tabControl;
  private TabPage tabPageMainProperties;
  private TabPage tabPageCommonProperties;
  private MaterialPropertiesPage _commonPropsPage;
  private Panel _pnlBottomProperties;
  private Button _btnDelProperty;
  private Button _btnAddProperty;
  private MainPropertiesPage _mainPropertiesPage;
  private SplitContainer splcTree;
  private ListView _lv;
  private System.Windows.Forms.ColumnHeader _caption;
  private System.Windows.Forms.ColumnHeader _color;
  private System.Windows.Forms.ColumnHeader _coatingClass;
  private System.Windows.Forms.ColumnHeader _coatingGroup;
  private System.Windows.Forms.ColumnHeader _termsOfUse;
  private GroupBox gpbConfig;
  private PropertyGrid _pgVarnish;
  private Panel pnlPropsBottom;
  private Button _btnAddVarnish;
  private Splitter splitter1;

  public IMHVarnishViewCtrl()
  {
    this.InitializeComponent();
    this.CustomizeMenu();
    INamedImageList service = ServiceUtils.GetService<INamedImageList>((object) ApplicationServices.Container, false);
    if (service != null)
    {
      this._lv.SmallImageList = this._lv.LargeImageList = service.ImageList;
      this._imgIndex = service.ImageIndex("icoVarnish");
    }
    this._mainPropertiesPage.DataProvider = (DataProvider) this._dataProvider;
  }

  private void On_miClick(object sender, EventArgs e)
  {
    this._commonPropsPage.ExpandAll((int) Convert.ToInt16(((ToolStripItem) sender).Tag) == sc_14617.ssp_imbase_14618(255175687));
  }

  private void On_lv_SelectedIndexChanged(object sender, EventArgs e)
  {
    bool flag = false;
    this._mTableRefID = 0L;
    this._mRecID = -1L;
    this._mCaption = string.Empty;
    if (this._lv.SelectedItems.Count > 0)
    {
      flag = true;
      if (this._lv.SelectedItems[0].Tag is LvItem tag1)
      {
        this._mTableRefID = tag1.M_TableID;
        this._mRecID = tag1.RecID;
        this._mCaption = tag1.Caption;
      }
      string tag2 = (string) this._lv.SelectedItems[0].SubItems[0].Tag;
      this._commonPropsPage.ImbaseKey = this._mainPropertiesPage.ImbaseKey = tag2;
      this._currentItem = new VarnishItem()
      {
        ImbaseKey = tag2,
        Color = this._lv.SelectedItems[0].SubItems[1].Tag as string,
        CoatingClass = this._lv.SelectedItems[0].SubItems[2].Tag as string,
        CoatingGroup = this._lv.SelectedItems[0].SubItems[3].Tag as string,
        TermOfUse = this._lv.SelectedItems[0].SubItems[4].Tag as string
      };
      this._pgVarnish.SelectedObject = (object) new VarnishPropertiesDescriptor(this._currentItem, this._dataProvider);
    }
    else
    {
      this._commonPropsPage.ImbaseKey = this._mainPropertiesPage.ImbaseKey = string.Empty;
      this._currentItem = (VarnishItem) null;
      this._pgVarnish.SelectedObject = (object) null;
    }
    this._btnAddVarnish.Enabled = false;
    if (this._readOnly)
      flag = false;
    this._btnAddProperty.Enabled = this._btnDelProperty.Enabled = flag;
    this._pnlFormula.Invalidate();
    this.OnIMHMaterialChanged(this._mTableRefID, this._mRecID, designation: this._mCaption);
  }

  private void _pgVarnish_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
  {
    if (this._readOnly)
      this._btnAddVarnish.Enabled = false;
    else
      this._btnAddVarnish.Enabled = !this._lv.Items.Cast<ListViewItem>().Any<ListViewItem>((System.Func<ListViewItem, bool>) (x => x.Text == this._lv.SelectedItems[0].Text && x.SubItems[1].Tag as string == this._currentItem.Color && x.SubItems[2].Tag as string == this._currentItem.CoatingClass && x.SubItems[3].Tag as string == this._currentItem.CoatingGroup && x.SubItems[4].Tag as string == this._currentItem.TermOfUse));
  }

  private void _btnAddVarnish_Click(object sender, EventArgs e)
  {
    List<string> stringList = new List<string>()
    {
      this._helper.ColorGuid.ToString(),
      Consts.CoatingClassAttrTypeGuid.ToString(),
      Consts.CoatingGroupAttrTypeGuid.ToString(),
      Consts.TermsOfUseAttrTypeGuid.ToString()
    };
    using (SessionKeeper sk = new SessionKeeper())
    {
      if (!(sk.Session.GetCustomService(typeof (IImbaseServer)) is IImbaseServer customService))
        return;
      List<Tuple<string, IEnumerable<DataTable>>> data = this._dataProvider.LoadData((string) this._lv.SelectedItems[0].SubItems[0].Tag);
      long tableIdByTableRefId = IMHHelper.GetTableIDByTableRefID(this._mTableRefID);
      DataSet tables = TableLoadHelper.GetTables(sk.Session, tableIdByTableRefId, true);
      if (tables == null)
        return;
      DataTable attTable = tables.Tables["IMS_ATTR_TYPES"];
      DataTable dataTable = tables.Tables["IMS_DATA"];
      List<string> existingAttrs = attTable.AsEnumerable().Select<DataRow, string>((System.Func<DataRow, string>) (row => row.Field<string>("F_ATTRIBUTE_GUID"))).ToList<string>();
      stringList.ForEach((Action<string>) (x =>
      {
        if (existingAttrs.IndexOf(x) != -1)
          return;
        this.AddAttrToTable(sk.Session, attTable, dataTable, x);
      }));
      DataRow dataRow = dataTable.AsEnumerable().FirstOrDefault<DataRow>((System.Func<DataRow, bool>) (row => Convert.ToInt64(row["F_KEY"]) == this._mRecID));
      DataRow row1 = dataTable.NewRow();
      long int64 = Convert.ToInt64(row1["F_KEY"]);
      string str1 = ImbaseHelper.MakeInternalImbaseKey(this._mTableRefID, int64);
      if (dataRow != null)
        row1.ItemArray = (object[]) dataRow.ItemArray.Clone();
      row1["F_KEY"] = (object) int64;
      row1["F_GUID"] = (object) Guid.NewGuid();
      row1[this._helper.ColorGuid.ToString()] = (object) this._currentItem.Color;
      row1[Consts.CoatingClassAttrTypeGuid.ToString()] = (object) this._currentItem.CoatingClass;
      row1[Consts.CoatingGroupAttrTypeGuid.ToString()] = (object) this._currentItem.CoatingGroup;
      row1[Consts.TermsOfUseAttrTypeGuid.ToString()] = (object) this._currentItem.TermOfUse;
      dataTable.Rows.Add(row1);
      dataTable.AcceptChanges();
      TableLoadHelper.StoreData(sk.Session, tableIdByTableRefId, tables, sk.Session.GetCustomService(typeof (ITablesIndexer)) as ITablesIndexer);
      this._dataProvider.SaveData(str1, data);
      this.AppendCommonPropsFromCurrentRecord(str1);
      List<string> keyValues = new List<string>()
      {
        str1,
        this._currentItem.Color,
        this._currentItem.CoatingGroup,
        this._currentItem.TermOfUse
      };
      Dictionary<string, string> dictionary = customService.NameRecordReferences(sk.Session.SessionGUID, keyValues);
      Dictionary<string, string> attrValues = this._helper.GetAttrValues(Consts.CoatingClassAttrTypeGuid);
      string str2;
      dictionary.TryGetValue(str1, out str2);
      string str3;
      dictionary.TryGetValue(this._currentItem.Color, out str3);
      string str4;
      attrValues.TryGetValue(this._currentItem.CoatingClass, out str4);
      string str5;
      dictionary.TryGetValue(this._currentItem.CoatingGroup, out str5);
      string str6;
      dictionary.TryGetValue(this._currentItem.TermOfUse, out str6);
      LvItem lvItem = new LvItem(this._mTableRefID, int64, str2);
      ListViewItem listViewItem = new ListViewItem(str2, this._imgIndex)
      {
        Selected = true,
        Tag = (object) lvItem
      };
      listViewItem.SubItems[0].Tag = (object) str1;
      listViewItem.SubItems.Add(new ListViewItem.ListViewSubItem()
      {
        Text = str3,
        Tag = (object) this._currentItem.Color
      });
      listViewItem.SubItems.Add(new ListViewItem.ListViewSubItem()
      {
        Text = str4,
        Tag = (object) this._currentItem.CoatingClass
      });
      listViewItem.SubItems.Add(new ListViewItem.ListViewSubItem()
      {
        Text = str5,
        Tag = (object) this._currentItem.CoatingGroup
      });
      listViewItem.SubItems.Add(new ListViewItem.ListViewSubItem()
      {
        Text = str6,
        Tag = (object) this._currentItem.TermOfUse
      });
      this._lv.Items.Insert(this._lv.SelectedItems[0].Index + 1, listViewItem);
      this._mRecID = int64;
    }
  }

  private void _btnAddProperty_Click(object sender, EventArgs e)
  {
    IDescriptor rootDescriptor = (IDescriptor) new Intermech.Navigator.DBObjectTypes.Descriptor(MetaDataHelper.GetObjectTypeID(Intermech.Imbase.Consts.MaterialPropertiesObjTypeGuid));
    long[] numArray = Intermech.Navigator.SelectionWindow.SelectObjects(LocalizationHolder.rm.GetString("IMH_SelectObject"), LocalizationHolder.rm.GetString("IMH_SelectGluesProperties"), rootDescriptor, SelectionOptions.SelectObjects | SelectionOptions.DisableSelectFromTree | SelectionOptions.DisableSelectAbstractTypes | SelectionOptions.DisableMultiselect);
    if (numArray == null || numArray.Length == 0)
      return;
    if (!this._commonPropsPage.IsSettingsLoaded)
      this._commonPropsPage.ReloadSettingsData();
    if (!this._commonPropsPage.IsSettingsLoaded)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      DataTable settingsTable = this._commonPropsPage.SettingsTable;
      string imbaseKey = this._commonPropsPage.ImbaseKey;
      bool isGuidKey;
      string str1 = ImbaseHelper.ConvertImbaseKey(sessionKeeper.Session, imbaseKey, out isGuidKey);
      DataRow dataRow = (DataRow) null;
      foreach (DataRow row in (InternalDataCollectionBase) settingsTable.Rows)
      {
        string str2 = Convert.ToString(row[this._commonPropsPage.ColMaterial]);
        if (!(str2 != imbaseKey) || !(str2 != str1))
        {
          dataRow = row;
          break;
        }
      }
      if (dataRow != null)
      {
        dataRow[this._commonPropsPage.ColObject] = (object) numArray[0];
      }
      else
      {
        DataRow row = settingsTable.NewRow();
        row["F_GUID"] = (object) Guid.NewGuid();
        row[this._commonPropsPage.ColMaterial] = isGuidKey ? (object) str1 : (object) imbaseKey;
        row[this._commonPropsPage.ColObject] = (object) numArray[0];
        settingsTable.Rows.Add(row);
      }
      settingsTable.AcceptChanges();
      long tableIdByTableRefId = IMHHelper.GetTableIDByTableRefID(IMHHelper.GetObjectIDByConstName("MATERIAL_PROPERTIES_TABLE_NAME"));
      TableLoadHelper.StoreData(sessionKeeper.Session, tableIdByTableRefId, settingsTable.DataSet, sessionKeeper.Session.GetCustomService(typeof (ITablesIndexer)) as ITablesIndexer);
      this._commonPropsPage.ImbaseKey = imbaseKey;
      ServiceUtils.GetService<INotificationService>((object) ApplicationServices.Container, false)?.FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", tableIdByTableRefId));
    }
  }

  private void _btnDelProperty_Click(object sender, EventArgs e)
  {
    string caption = LocalizationHolder.rm.GetString("IMH_DeleteProperties");
    if (MessageBox.Show(LocalizationHolder.rm.GetString("IMH_DeleteProperties_Msg"), caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
      return;
    if (!this._commonPropsPage.IsSettingsLoaded)
      this._commonPropsPage.ReloadSettingsData();
    if (!this._commonPropsPage.IsSettingsLoaded)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      DataTable settingsTable = this._commonPropsPage.SettingsTable;
      string imbaseKey = this._commonPropsPage.ImbaseKey;
      string str1 = ImbaseHelper.ConvertImbaseKey(sessionKeeper.Session, imbaseKey);
      List<DataRow> dataRowList = new List<DataRow>();
      foreach (DataRow row in (InternalDataCollectionBase) settingsTable.Rows)
      {
        string str2 = Convert.ToString(row[this._commonPropsPage.ColMaterial]);
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
      this._commonPropsPage.Clear(true);
      ServiceUtils.GetService<INotificationService>((object) ApplicationServices.Container, false)?.FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", tableIdByTableRefId));
    }
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
    this._lv.Items[0].Selected = this._lv.Items.Count > 0 && this._lv.SelectedItems.Count == 0;
  }

  public override void Deactivate(IView nextView)
  {
    base.Deactivate(nextView);
    this.UnsubscribeEvents();
  }

  public override void Initialize(
    ISelectedItems items,
    IServiceProvider provider,
    NavigatorTreeNode parentINode)
  {
    this.ClearData();
    base.Initialize(items, provider, parentINode);
    bool canEdit = IMHViewCtrlBase.ExtractCanEdit(parentINode);
    this._readOnly = !canEdit;
    this._commonPropsPage.Initialize(canEdit);
    this._mainPropertiesPage.Initialize(canEdit);
    this._btnAddProperty.Enabled = this._btnDelProperty.Enabled = canEdit;
    this._btnAddVarnish.Visible = canEdit;
    if (!(items?.GetItemData(0, typeof (FolderNode)) is FolderNode itemData))
      return;
    this._mTableRefID = itemData.SelectedMaterialTableRefID;
    this._mRecID = itemData.SelectedMaterialRecID;
    itemData.SelectedMaterialTableRefID = 0L;
    itemData.SelectedMaterialRecID = -1L;
    this._folderId = itemData.FolderID;
    this._helper.LoadData(this._folderId);
    this.AddItemsToListView(this.CreateListViewItems());
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
    if (this._helper.IsDataLoaded)
    {
      List<string> names = new List<string>()
      {
        "SURFACE_MATERIALS_TABLE_NAME",
        "TERMS_USE_TABLE_NAME",
        "SURFACE_MATERIALS_COLUMN_NAME",
        "TERMS_USE_COLUMN_NAME"
      };
      Dictionary<string, Guid> dictionary = (Dictionary<string, Guid>) null;
      Dictionary<string, string>[] dictionaryArray = new Dictionary<string, string>[2];
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        if (sessionKeeper.Session.GetCustomService(typeof (IIMHSystemSettingsService)) is IIMHSystemSettingsService customService)
          dictionary = customService.GetObjectGuidsByNames(names);
        if (dictionary != null)
        {
          for (int index = 0; index < 2; ++index)
          {
            string key1 = names[index];
            string key2 = names[index + 2];
            long objectId = sessionKeeper.Session.GetObjectInfo(dictionary[key1]).ObjectID;
            DataTable table = this.GetTable(objectId);
            string columnName = Convert.ToString((object) dictionary[key2]);
            dictionaryArray[index] = this.GetItems(table, columnName, objectId);
          }
        }
      }
      using (GlueSearchForm glueSearchForm = new GlueSearchForm(dictionaryArray[0], dictionaryArray[1], new Func<string, string, Dictionary<string, string>>(this.Search)))
      {
        string captionForm = LocalizationHolder.rm.GetString("IMH_Varnish_Search");
        string caption1 = LocalizationHolder.rm.GetString("IMH_SurfaceMaterial");
        string caption2 = LocalizationHolder.rm.GetString("IMH_TermsOfUseTableName");
        string captionResult = LocalizationHolder.rm.GetString("IMH_CoatingsHandbookNode_Caption");
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
      int num = (int) MessageBox.Show(LocalizationHolder.rm.GetString("IMH_Varnish_EmptyData_Msg"), caption, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }
  }

  private DataTable GetTable(long linkId)
  {
    DataTable table = (DataTable) null;
    long tableIdByTableRefId = IMHHelper.GetTableIDByTableRefID(linkId);
    if (tableIdByTableRefId != 0L)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        DataSet tables = TableLoadHelper.GetTables(sessionKeeper.Session, tableIdByTableRefId, true);
        if (tables?.Tables != null)
        {
          if (tables.Tables.Contains("IMS_DATA"))
            table = tables.Tables["IMS_DATA"];
        }
      }
    }
    return table;
  }

  private Dictionary<string, string> GetItems(DataTable dt, string columnName, long linkId)
  {
    if (dt == null || dt.Rows.Count <= 0)
      return (Dictionary<string, string>) null;
    Dictionary<string, string> items = new Dictionary<string, string>(dt.Rows.Count);
    foreach (DataRow row in (InternalDataCollectionBase) dt.Rows)
    {
      long int64 = Convert.ToInt64(row["F_KEY"]);
      string str = Convert.ToString(row[columnName]);
      string key = ImbaseHelper.MakeInternalImbaseKey(linkId, int64);
      items.Add(key, str);
    }
    return items;
  }

  private Dictionary<string, string> Search(string materialKey, string termsOfUseKey)
  {
    Dictionary<string, string> dictionary1 = (Dictionary<string, string>) null;
    List<string> names = new List<string>()
    {
      "COATING_MATERIALS_TABLE_NAME",
      "COATING_MATERIALS_COLUMN_COATING",
      "COATING_MATERIALS_COLUMN_MATERIALS",
      "COATING_TERMS_USE_TABLE_NAME",
      "COATING_TERMS_USE_COLUMN_COATING",
      "COATING_TERMS_USE_COLUMN_TERMS"
    };
    Dictionary<string, Guid> dictionary2 = (Dictionary<string, Guid>) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (sessionKeeper.Session.GetCustomService(typeof (IIMHSystemSettingsService)) is IIMHSystemSettingsService customService1)
        dictionary2 = customService1.GetObjectGuidsByNames(names);
      if (dictionary2 != null)
      {
        long objectId = sessionKeeper.Session.GetObjectInfo(dictionary2["COATING_MATERIALS_TABLE_NAME"]).ObjectID;
        List<string> stringList1 = (List<string>) null;
        List<string> stringList2 = (List<string>) null;
        DataTable table1 = this.GetTable(objectId);
        if (table1 != null)
        {
          string columnNameCoating = Convert.ToString((object) dictionary2["COATING_MATERIALS_COLUMN_COATING"]);
          string columnNameMaterial = Convert.ToString((object) dictionary2["COATING_MATERIALS_COLUMN_MATERIALS"]);
          stringList1 = table1.AsEnumerable().Where<DataRow>((System.Func<DataRow, bool>) (x => Convert.ToString(x[columnNameCoating]) == materialKey)).Select<DataRow, string>((System.Func<DataRow, string>) (x => Convert.ToString(x[columnNameMaterial]))).ToList<string>();
        }
        DataTable table2 = this.GetTable(sessionKeeper.Session.GetObjectInfo(dictionary2["COATING_TERMS_USE_TABLE_NAME"]).ObjectID);
        if (table2 != null)
        {
          string columnNameCoating = Convert.ToString((object) dictionary2["COATING_TERMS_USE_COLUMN_COATING"]);
          string columnNameTermsOfUse = Convert.ToString((object) dictionary2["COATING_TERMS_USE_COLUMN_TERMS"]);
          stringList2 = table2.AsEnumerable().Where<DataRow>((System.Func<DataRow, bool>) (x => Convert.ToString(x[columnNameTermsOfUse]) == termsOfUseKey)).Select<DataRow, string>((System.Func<DataRow, string>) (x => Convert.ToString(x[columnNameCoating]))).ToList<string>();
        }
        if (stringList1 != null)
        {
          if (stringList1.Any<string>())
          {
            if (stringList2 != null)
            {
              if (stringList2.Any<string>())
              {
                List<string> list = stringList1.Intersect<string>((IEnumerable<string>) stringList2).ToList<string>();
                if (list.Any<string>())
                {
                  if (sessionKeeper.Session.GetCustomService(typeof (IImbaseServer)) is IImbaseServer customService2)
                    dictionary1 = customService2.NameRecordReferences(sessionKeeper.Session.SessionGUID, list);
                }
              }
            }
          }
        }
      }
    }
    return dictionary1;
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

  private void CustomizeMenu()
  {
    this._lv.ContextMenuStrip = this._contextMenuBase;
    this._tsBtnCoatingProperties.Visible = this._tsBtnGluedMaterials.Visible = this._tsBtnMaterialProperties.Visible = this._tsBtnMaterialSubstitutes.Visible = this._tsSeparator1.Visible = false;
    this._tsBtnApplicabilityFilter.Visible = false;
    this._cmCoatingProperties.Visible = this._cmGluedMaterials.Visible = this._cmMaterialProperties.Visible = this._cmMaterialSubstitutes.Visible = this._cmSeparator1.Visible = false;
    this._cmApplicabilityFilter.Visible = false;
  }

  private new void ClearData()
  {
    base.ClearData();
    this._lv.Items.Clear();
    this._pgVarnish.SelectedObject = (object) null;
    this._currentItem = (VarnishItem) null;
    this._commonPropsPage.ImbaseKey = string.Empty;
    this._btnAddProperty.Enabled = this._btnDelProperty.Enabled = false;
  }

  private ListViewItem[] CreateListViewItems()
  {
    if (!this._helper.IsDataLoaded)
      return (ListViewItem[]) null;
    List<Tuple<Tuple<string, string>, Tuple<string, string>, Tuple<string, string>, Tuple<string, string>, Tuple<string, string>>> keys = this._helper.Keys;
    if (keys == null)
      return (ListViewItem[]) null;
    ListViewItem[] listViewItems = new ListViewItem[keys.Count];
    int num = 0;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      foreach (Tuple<Tuple<string, string>, Tuple<string, string>, Tuple<string, string>, Tuple<string, string>, Tuple<string, string>> tuple in keys)
      {
        long linkId;
        long recordId;
        ImbaseHelper.TryParseRecordReference(sessionKeeper.Session, tuple.Item1.Item1, out linkId, out recordId);
        bool flag = this._mRecID == recordId && this._mTableRefID == linkId;
        LvItem lvItem = new LvItem(linkId, recordId, tuple.Item1.Item2);
        ListViewItem listViewItem = new ListViewItem(tuple.Item1.Item2, this._imgIndex)
        {
          Selected = flag,
          Tag = (object) lvItem
        };
        listViewItem.SubItems[0].Tag = (object) tuple.Item1.Item1;
        listViewItem.SubItems.Add(new ListViewItem.ListViewSubItem()
        {
          Text = tuple.Item2.Item2,
          Tag = (object) tuple.Item2.Item1
        });
        listViewItem.SubItems.Add(new ListViewItem.ListViewSubItem()
        {
          Text = tuple.Item3.Item2,
          Tag = (object) tuple.Item3.Item1
        });
        listViewItem.SubItems.Add(new ListViewItem.ListViewSubItem()
        {
          Text = tuple.Item4.Item2,
          Tag = (object) tuple.Item4.Item1
        });
        listViewItem.SubItems.Add(new ListViewItem.ListViewSubItem()
        {
          Text = tuple.Item5.Item2,
          Tag = (object) tuple.Item5.Item1
        });
        listViewItems[num++] = listViewItem;
      }
    }
    return listViewItems;
  }

  private void AddItemsToListView(ListViewItem[] items)
  {
    if (items == null)
      return;
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

  private void ViewFavourites(long tblRefId, long recId, string caption)
  {
    using (MaterialFavourites materialFavourites = new MaterialFavourites(this._helper.NodeGuid, 0L, tblRefId, recId, caption))
    {
      if (materialFavourites.ShowDialog() != DialogResult.OK)
        return;
      FavouriteData data = materialFavourites.Data;
      this.GoToNode(data.TableRefID, data.TableRefID, data.RecordID);
    }
  }

  private void AddAttrToTable(
    IUserSession session,
    DataTable dtAttrs,
    DataTable dtData,
    string strAttrGuid)
  {
    Guid anAttributeGuid = new Guid(strAttrGuid);
    IDBAttributeType attributeType = session.GetAttributeType(anAttributeGuid);
    DataRow row1 = dtAttrs.NewRow();
    row1["F_ATTRIBUTE_GUID"] = (object) anAttributeGuid;
    row1["F_REQUIRED"] = (object) 2;
    row1["F_COMPUTED"] = (object) 0;
    row1["F_FORMULA"] = (object) string.Empty;
    row1["F_UNIQUE"] = (object) 0;
    row1["F_DEFAULT_VALUE"] = attributeType.DefaultValue;
    row1["F_OPTIONS"] = (object) attributeType.Options;
    row1["F_UNITS"] = (object) string.Empty;
    if (attributeType.AttributeType == FieldTypes.ftMeasured)
    {
      long baseMeasureId = MeasureHelper.GetBaseMeasureID(attributeType.PropertiesStructure.SizeType);
      QuickObjectInfo objectInfo = session.GetObjectInfo(baseMeasureId);
      if (!objectInfo.Empty)
        row1["F_UNITS"] = (object) objectInfo.VersionGuid;
    }
    dtAttrs.Rows.Add(row1);
    if (TableLoadHelper.CreateDataColumn(dtData, attributeType) != null && attributeType.DefaultValue != null && attributeType.DefaultValue != DBNull.Value)
    {
      foreach (DataRow row2 in (InternalDataCollectionBase) dtData.Rows)
        row2[strAttrGuid] = attributeType.DefaultValue;
    }
    dtAttrs.AcceptChanges();
    dtData.AcceptChanges();
  }

  private void AppendCommonPropsFromCurrentRecord(string newImbaseKey)
  {
    if (!this._commonPropsPage.IsSettingsLoaded)
      this._commonPropsPage.ReloadSettingsData();
    if (!this._commonPropsPage.IsSettingsLoaded)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      DataTable settingsTable = this._commonPropsPage.SettingsTable;
      string key = this._commonPropsPage.ImbaseKey;
      bool isGuidKey;
      string key2 = ImbaseHelper.ConvertImbaseKey(sessionKeeper.Session, key, out isGuidKey);
      string str = ImbaseHelper.ConvertImbaseKey(sessionKeeper.Session, newImbaseKey, out isGuidKey);
      DataRow dataRow = settingsTable.AsEnumerable().FirstOrDefault<DataRow>((System.Func<DataRow, bool>) (rw => key == rw[this._commonPropsPage.ColMaterial].ToString() || key2 == rw[this._commonPropsPage.ColMaterial].ToString()));
      if (dataRow == null)
        return;
      DataRow row = settingsTable.NewRow();
      row["F_GUID"] = (object) Guid.NewGuid();
      row[this._commonPropsPage.ColMaterial] = isGuidKey ? (object) str : (object) newImbaseKey;
      row[this._commonPropsPage.ColObject] = dataRow[this._commonPropsPage.ColObject];
      settingsTable.Rows.Add(row);
      settingsTable.AcceptChanges();
      long tableIdByTableRefId = IMHHelper.GetTableIDByTableRefID(IMHHelper.GetObjectIDByConstName("MATERIAL_PROPERTIES_TABLE_NAME"));
      TableLoadHelper.StoreData(sessionKeeper.Session, tableIdByTableRefId, settingsTable.DataSet, sessionKeeper.Session.GetCustomService(typeof (ITablesIndexer)) as ITablesIndexer);
      ServiceUtils.GetService<INotificationService>((object) ApplicationServices.Container, false)?.FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", tableIdByTableRefId));
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
    this._contextMenu = new ContextMenuStrip(this.components);
    this._miCollapse = new ToolStripMenuItem();
    this._miExpand = new ToolStripMenuItem();
    this.tabControl = new TabControl();
    this.tabPageMainProperties = new TabPage();
    this._mainPropertiesPage = new MainPropertiesPage();
    this.tabPageCommonProperties = new TabPage();
    this._commonPropsPage = new MaterialPropertiesPage();
    this._pnlBottomProperties = new Panel();
    this._btnDelProperty = new Button();
    this._btnAddProperty = new Button();
    this.splitter1 = new Splitter();
    this.splcTree = new SplitContainer();
    this._lv = new ListView();
    this._caption = new System.Windows.Forms.ColumnHeader();
    this._color = new System.Windows.Forms.ColumnHeader();
    this._coatingClass = new System.Windows.Forms.ColumnHeader();
    this._coatingGroup = new System.Windows.Forms.ColumnHeader();
    this._termsOfUse = new System.Windows.Forms.ColumnHeader();
    this.gpbConfig = new GroupBox();
    this._pgVarnish = new PropertyGrid();
    this.pnlPropsBottom = new Panel();
    this._btnAddVarnish = new Button();
    this._splt.BeginInit();
    this._splt.Panel1.SuspendLayout();
    this._splt.Panel2.SuspendLayout();
    this._splt.SuspendLayout();
    this._contextMenu.SuspendLayout();
    this.tabControl.SuspendLayout();
    this.tabPageMainProperties.SuspendLayout();
    this.tabPageCommonProperties.SuspendLayout();
    this._pnlBottomProperties.SuspendLayout();
    this.splcTree.BeginInit();
    this.splcTree.Panel1.SuspendLayout();
    this.splcTree.Panel2.SuspendLayout();
    this.splcTree.SuspendLayout();
    this.gpbConfig.SuspendLayout();
    this.pnlPropsBottom.SuspendLayout();
    this.SuspendLayout();
    this._splt.Panel1.Controls.Add((Control) this.splcTree);
    this._splt.Panel1.Controls.Add((Control) this.splitter1);
    this._splt.Panel2.Controls.Add((Control) this.tabControl);
    this._contextMenu.Items.AddRange(new ToolStripItem[2]
    {
      (ToolStripItem) this._miCollapse,
      (ToolStripItem) this._miExpand
    });
    this._contextMenu.Name = "_contextMenu";
    this._contextMenu.Size = new Size(157, 48 /*0x30*/);
    this._miCollapse.DisplayStyle = ToolStripItemDisplayStyle.Text;
    this._miCollapse.Name = "_miCollapse";
    this._miCollapse.Size = new Size(156, 22);
    this._miCollapse.Tag = (object) "0";
    this._miCollapse.Text = "Свернуть все";
    this._miCollapse.Click += new EventHandler(this.On_miClick);
    this._miExpand.DisplayStyle = ToolStripItemDisplayStyle.Text;
    this._miExpand.Name = "_miExpand";
    this._miExpand.Size = new Size(156, 22);
    this._miExpand.Tag = (object) "1";
    this._miExpand.Text = "Развернуть все";
    this._miExpand.Click += new EventHandler(this.On_miClick);
    this.tabControl.Controls.Add((Control) this.tabPageMainProperties);
    this.tabControl.Controls.Add((Control) this.tabPageCommonProperties);
    this.tabControl.Dock = DockStyle.Fill;
    this.tabControl.Location = new Point(0, 0);
    this.tabControl.Name = "tabControl";
    this.tabControl.SelectedIndex = 0;
    this.tabControl.Size = new Size(497, 515);
    this.tabControl.TabIndex = 8;
    this.tabPageMainProperties.Controls.Add((Control) this._mainPropertiesPage);
    this.tabPageMainProperties.Location = new Point(4, 22);
    this.tabPageMainProperties.Name = "tabPageMainProperties";
    this.tabPageMainProperties.Padding = new Padding(3);
    this.tabPageMainProperties.Size = new Size(489, 489);
    this.tabPageMainProperties.TabIndex = 0;
    this.tabPageMainProperties.Text = "Основные свойства";
    this.tabPageMainProperties.UseVisualStyleBackColor = true;
    this._mainPropertiesPage.DataProvider = (DataProvider) null;
    this._mainPropertiesPage.Dock = DockStyle.Fill;
    this._mainPropertiesPage.ImbaseKey = "";
    this._mainPropertiesPage.Location = new Point(3, 3);
    this._mainPropertiesPage.Name = "_mainPropertiesPage";
    this._mainPropertiesPage.Size = new Size(483, 483);
    this._mainPropertiesPage.TabIndex = 0;
    this.tabPageCommonProperties.Controls.Add((Control) this._commonPropsPage);
    this.tabPageCommonProperties.Controls.Add((Control) this._pnlBottomProperties);
    this.tabPageCommonProperties.Location = new Point(4, 22);
    this.tabPageCommonProperties.Name = "tabPageCommonProperties";
    this.tabPageCommonProperties.Padding = new Padding(3);
    this.tabPageCommonProperties.Size = new Size(489, 489);
    this.tabPageCommonProperties.TabIndex = 1;
    this.tabPageCommonProperties.Text = "Общие свойства";
    this.tabPageCommonProperties.UseVisualStyleBackColor = true;
    this._commonPropsPage.ContextMenuStrip = this._contextMenu;
    this._commonPropsPage.Dock = DockStyle.Fill;
    this._commonPropsPage.ImbaseKey = "";
    this._commonPropsPage.Location = new Point(3, 3);
    this._commonPropsPage.Name = "_commonPropsPage";
    this._commonPropsPage.Size = new Size(483, 447);
    this._commonPropsPage.TabIndex = 9;
    this._pnlBottomProperties.BackColor = SystemColors.Control;
    this._pnlBottomProperties.Controls.Add((Control) this._btnDelProperty);
    this._pnlBottomProperties.Controls.Add((Control) this._btnAddProperty);
    this._pnlBottomProperties.Dock = DockStyle.Bottom;
    this._pnlBottomProperties.Location = new Point(3, 450);
    this._pnlBottomProperties.Name = "_pnlBottomProperties";
    this._pnlBottomProperties.Size = new Size(483, 36);
    this._pnlBottomProperties.TabIndex = 10;
    this._btnDelProperty.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this._btnDelProperty.Enabled = false;
    this._btnDelProperty.ImeMode = ImeMode.NoControl;
    this._btnDelProperty.Location = new Point(393, 6);
    this._btnDelProperty.Name = "_btnDelProperty";
    this._btnDelProperty.Size = new Size(75, 23);
    this._btnDelProperty.TabIndex = 1;
    this._btnDelProperty.Text = "Удалить";
    this._btnDelProperty.UseVisualStyleBackColor = true;
    this._btnDelProperty.Click += new EventHandler(this._btnDelProperty_Click);
    this._btnAddProperty.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this._btnAddProperty.Enabled = false;
    this._btnAddProperty.ImeMode = ImeMode.NoControl;
    this._btnAddProperty.Location = new Point(312, 6);
    this._btnAddProperty.Name = "_btnAddProperty";
    this._btnAddProperty.Size = new Size(75, 23);
    this._btnAddProperty.TabIndex = 0;
    this._btnAddProperty.Text = "Добавить";
    this._btnAddProperty.UseVisualStyleBackColor = true;
    this._btnAddProperty.Click += new EventHandler(this._btnAddProperty_Click);
    this.splitter1.Location = new Point(0, 0);
    this.splitter1.Name = "splitter1";
    this.splitter1.Size = new Size(3, 515);
    this.splitter1.TabIndex = 12;
    this.splitter1.TabStop = false;
    this.splcTree.Dock = DockStyle.Fill;
    this.splcTree.Location = new Point(3, 0);
    this.splcTree.Name = "splcTree";
    this.splcTree.Orientation = Orientation.Horizontal;
    this.splcTree.Panel1.Controls.Add((Control) this._lv);
    this.splcTree.Panel2.Controls.Add((Control) this.gpbConfig);
    this.splcTree.Size = new Size(296, 515);
    this.splcTree.SplitterDistance = 308;
    this.splcTree.TabIndex = 13;
    this._lv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[5]
    {
      this._caption,
      this._color,
      this._coatingClass,
      this._coatingGroup,
      this._termsOfUse
    });
    this._lv.Dock = DockStyle.Fill;
    this._lv.FullRowSelect = true;
    this._lv.HeaderStyle = ColumnHeaderStyle.Nonclickable;
    this._lv.HideSelection = false;
    this._lv.Location = new Point(0, 0);
    this._lv.MultiSelect = false;
    this._lv.Name = "_lv";
    this._lv.Size = new Size(296, 308);
    this._lv.TabIndex = 9;
    this._lv.UseCompatibleStateImageBehavior = false;
    this._lv.View = View.Details;
    this._lv.MouseDoubleClick += new MouseEventHandler(this._lv_MouseDoubleClick);
    this._caption.Text = "Наименование";
    this._caption.Width = 225;
    this._color.Text = "Цвет";
    this._color.Width = 97;
    this._coatingClass.Text = "Класс покрытий";
    this._coatingGroup.Text = "Группа покрытий";
    this._termsOfUse.Text = "Условия эксплуатации";
    this.gpbConfig.Controls.Add((Control) this._pgVarnish);
    this.gpbConfig.Controls.Add((Control) this.pnlPropsBottom);
    this.gpbConfig.Dock = DockStyle.Fill;
    this.gpbConfig.Location = new Point(0, 0);
    this.gpbConfig.Name = "gpbConfig";
    this.gpbConfig.Padding = new Padding(5);
    this.gpbConfig.Size = new Size(296, 203);
    this.gpbConfig.TabIndex = 12;
    this.gpbConfig.TabStop = false;
    this.gpbConfig.Text = "Конфигурация:";
    this._pgVarnish.Dock = DockStyle.Fill;
    this._pgVarnish.Location = new Point(5, 18);
    this._pgVarnish.Name = "_pgVarnish";
    this._pgVarnish.PropertySort = PropertySort.NoSort;
    this._pgVarnish.Size = new Size(286, 144 /*0x90*/);
    this._pgVarnish.TabIndex = 11;
    this._pgVarnish.ToolbarVisible = false;
    this._pgVarnish.PropertyValueChanged += new PropertyValueChangedEventHandler(this._pgVarnish_PropertyValueChanged);
    this.pnlPropsBottom.Controls.Add((Control) this._btnAddVarnish);
    this.pnlPropsBottom.Dock = DockStyle.Bottom;
    this.pnlPropsBottom.Location = new Point(5, 162);
    this.pnlPropsBottom.Name = "pnlPropsBottom";
    this.pnlPropsBottom.Size = new Size(286, 36);
    this.pnlPropsBottom.TabIndex = 10;
    this._btnAddVarnish.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this._btnAddVarnish.Enabled = false;
    this._btnAddVarnish.ImeMode = ImeMode.NoControl;
    this._btnAddVarnish.Location = new Point(205, 6);
    this._btnAddVarnish.Name = "_btnAddVarnish";
    this._btnAddVarnish.Size = new Size(75, 23);
    this._btnAddVarnish.TabIndex = 0;
    this._btnAddVarnish.Text = "Добавить";
    this._btnAddVarnish.UseVisualStyleBackColor = true;
    this._btnAddVarnish.Click += new EventHandler(this._btnAddVarnish_Click);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Name = nameof (IMHVarnishViewCtrl);
    this.Controls.SetChildIndex((Control) this._pnlFormula, 0);
    this.Controls.SetChildIndex((Control) this._splt, 0);
    this._splt.Panel1.ResumeLayout(false);
    this._splt.Panel2.ResumeLayout(false);
    this._splt.EndInit();
    this._splt.ResumeLayout(false);
    this._contextMenu.ResumeLayout(false);
    this.tabControl.ResumeLayout(false);
    this.tabPageMainProperties.ResumeLayout(false);
    this.tabPageCommonProperties.ResumeLayout(false);
    this._pnlBottomProperties.ResumeLayout(false);
    this.splcTree.Panel1.ResumeLayout(false);
    this.splcTree.Panel2.ResumeLayout(false);
    this.splcTree.EndInit();
    this.splcTree.ResumeLayout(false);
    this.gpbConfig.ResumeLayout(false);
    this.pnlPropsBottom.ResumeLayout(false);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
