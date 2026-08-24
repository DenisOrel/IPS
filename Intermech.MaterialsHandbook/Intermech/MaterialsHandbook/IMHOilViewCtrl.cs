// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.IMHOilViewCtrl
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
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class IMHOilViewCtrl : IMHViewCtrlBase
{
  private bool _lock;
  private bool _readOnly;
  private int _imgIndex = -1;
  private OilHelper _helper = new OilHelper();
  private IContainer components;
  private ContextMenuStrip _contextMenu;
  private ToolStripMenuItem _miCollapse;
  private ToolStripMenuItem _miExpand;
  private ListView _lv;
  private System.Windows.Forms.ColumnHeader _caption;
  private Panel _pnl;
  private Button _btnDelProperty;
  private Button _btnAddProperty;
  private MaterialPropertiesPage _propsPage;

  public IMHOilViewCtrl()
  {
    this.InitializeComponent();
    this.CustomizeMenu();
    INamedImageList service = ServiceUtils.GetService<INamedImageList>((object) ApplicationServices.Container, false);
    if (service == null)
      return;
    this._lv.SmallImageList = this._lv.LargeImageList = service.ImageList;
    this._imgIndex = service.ImageIndex("icoOils");
  }

  private void On_miClick(object sender, EventArgs e)
  {
    this._propsPage.ExpandAll(Convert.ToInt32(sender is ToolStripMenuItem toolStripMenuItem ? toolStripMenuItem.Tag : (object) null) == sc_14612.ssp_imbase_14613(1970570371));
  }

  private void On_lv_SelectedIndexChanged(object sender, EventArgs e)
  {
    bool flag = false;
    string str = string.Empty;
    this._mTableRefID = 0L;
    this._mRecID = -1L;
    this._mCaption = string.Empty;
    if (this._lv.SelectedItems.Count > 0)
    {
      if (this._lv.SelectedItems[0].Tag is LvItem tag)
      {
        this._mTableRefID = tag.M_TableID;
        this._mRecID = tag.RecID;
        this._mCaption = tag.Caption;
      }
      str = ImbaseHelper.MakeInternalImbaseKey(this._mTableRefID, this._mRecID);
      flag = true;
    }
    this._propsPage.ImbaseKey = str;
    if (this._readOnly)
      flag = false;
    this._btnAddProperty.Enabled = this._btnDelProperty.Enabled = flag;
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

  private void OnAddProperty_Click(object sender, EventArgs e)
  {
    IDescriptor rootDescriptor = (IDescriptor) new Intermech.Navigator.DBObjectTypes.Descriptor(MetaDataHelper.GetObjectTypeID(Intermech.Imbase.Consts.MaterialPropertiesObjTypeGuid));
    long[] numArray = Intermech.Navigator.SelectionWindow.SelectObjects(LocalizationHolder.rm.GetString("IMH_SelectObject"), LocalizationHolder.rm.GetString("IMH_SelectOilProperties"), rootDescriptor, SelectionOptions.SelectObjects | SelectionOptions.DisableSelectFromTree | SelectionOptions.DisableSelectAbstractTypes | SelectionOptions.DisableMultiselect);
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

  private void OnDelProperty_Click(object sender, EventArgs e)
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
      for (int index = 0; index < dataRowList.Count; ++index)
        settingsTable.Rows.Remove(dataRowList[index]);
      settingsTable.AcceptChanges();
      long tableIdByTableRefId = IMHHelper.GetTableIDByTableRefID(IMHHelper.GetObjectIDByConstName("MATERIAL_PROPERTIES_TABLE_NAME"));
      TableLoadHelper.StoreData(sessionKeeper.Session, tableIdByTableRefId, settingsTable.DataSet, sessionKeeper.Session.GetCustomService(typeof (ITablesIndexer)) as ITablesIndexer);
      this._propsPage.Clear(true);
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
    this._btnAddProperty.Enabled = this._btnDelProperty.Enabled = canEdit;
    if (!(items?.GetItemData(0, typeof (FolderNode)) is FolderNode itemData))
      return;
    this._mTableRefID = itemData.SelectedMaterialTableRefID;
    this._mRecID = itemData.SelectedMaterialRecID;
    itemData.SelectedMaterialTableRefID = 0L;
    itemData.SelectedMaterialRecID = -1L;
    this._helper.LoadData(itemData.FolderID);
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
    this._tsBtnSearch.Visible = this._tsBtnApplicabilityFilter.Visible = false;
    this._cmCoatingProperties.Visible = this._cmGluedMaterials.Visible = this._cmMaterialProperties.Visible = this._cmMaterialSubstitutes.Visible = this._cmSeparator1.Visible = false;
    this._cmSearch.Visible = this._cmApplicabilityFilter.Visible = false;
  }

  private new void ClearData()
  {
    base.ClearData();
    this._lock = true;
    this._lv.Items.Clear();
    this._lock = false;
    this._propsPage.ImbaseKey = string.Empty;
    this._btnAddProperty.Enabled = this._btnDelProperty.Enabled = false;
  }

  private ListViewItem[] CreateListViewItems()
  {
    ListViewItem[] listViewItems = (ListViewItem[]) null;
    if (this._helper.IsDataLoaded)
    {
      Dictionary<string, string> keys = this._helper.Keys;
      listViewItems = new ListViewItem[keys.Count];
      int num = 0;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        foreach (KeyValuePair<string, string> keyValuePair in keys)
        {
          long linkId;
          long recordId;
          ImbaseHelper.TryParseRecordReference(sessionKeeper.Session, keyValuePair.Key, out linkId, out recordId);
          bool flag = this._mRecID == recordId && this._mTableRefID == linkId;
          LvItem lvItem = new LvItem(linkId, recordId, keyValuePair.Value);
          ListViewItem listViewItem = new ListViewItem(keyValuePair.Value, this._imgIndex)
          {
            Selected = flag,
            Tag = (object) lvItem
          };
          listViewItems[num++] = listViewItem;
        }
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

  private void ViewFavourites(long tblRefID, long recID, string caption)
  {
    using (MaterialFavourites materialFavourites = new MaterialFavourites(this._helper.NodeGuid, 0L, tblRefID, recID, caption))
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

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (IMHOilViewCtrl));
    this._contextMenu = new ContextMenuStrip(this.components);
    this._miCollapse = new ToolStripMenuItem();
    this._miExpand = new ToolStripMenuItem();
    this._lv = new ListView();
    this._caption = new System.Windows.Forms.ColumnHeader();
    this._pnl = new Panel();
    this._btnDelProperty = new Button();
    this._btnAddProperty = new Button();
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
    this._pnl.Controls.Add((Control) this._btnDelProperty);
    this._pnl.Controls.Add((Control) this._btnAddProperty);
    componentResourceManager.ApplyResources((object) this._pnl, "_pnl");
    this._pnl.Name = "_pnl";
    componentResourceManager.ApplyResources((object) this._btnDelProperty, "_btnDelProperty");
    this._btnDelProperty.Name = "_btnDelProperty";
    this._btnDelProperty.UseVisualStyleBackColor = true;
    this._btnDelProperty.Click += new EventHandler(this.OnDelProperty_Click);
    componentResourceManager.ApplyResources((object) this._btnAddProperty, "_btnAddProperty");
    this._btnAddProperty.Name = "_btnAddProperty";
    this._btnAddProperty.UseVisualStyleBackColor = true;
    this._btnAddProperty.Click += new EventHandler(this.OnAddProperty_Click);
    this._propsPage.ContextMenuStrip = this._contextMenu;
    componentResourceManager.ApplyResources((object) this._propsPage, "_propsPage");
    this._propsPage.ImbaseKey = "";
    this._propsPage.Name = "_propsPage";
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Name = nameof (IMHOilViewCtrl);
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
