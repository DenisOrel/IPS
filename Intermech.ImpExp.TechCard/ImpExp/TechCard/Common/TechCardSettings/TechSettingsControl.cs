// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.TechCardSettings.TechSettingsControl
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.Controls;
using Intermech.ImpExp.SearchData;
using Intermech.ImpExp.TechCard.Controls;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common.TechCardSettings;

public class TechSettingsControl : StepControl
{
  private IDataBase _searchConnection;
  private Image _image;
  private bool _settingsLoaded;
  private bool _archiveNodeUpdate;
  private bool _archivesLoaded;
  private Dictionary<int, TreeNode> _archiveNodeCache;
  private readonly List<int> _docList = new List<int>();
  private readonly List<ArtInfoLight> _artList = new List<ArtInfoLight>();
  private readonly List<ArtInfoLight> _prodZakList = new List<ArtInfoLight>();
  private IContainer components;
  private GroupBox grbMain;
  private GroupBox grbTechProc;
  private Label lblComplectFolder;
  private Button btnComplectFolder;
  private TextBox tbxComplectFolder;
  private CheckBox chbCompectPump;
  private ErrorProvider errorProvider;
  private FolderBrowserDialog folderBrowserDialog;
  private GroupBox grbPumpMode;
  private RadioButton rbtnPumpTpList;
  private RadioButton rbtnPumpByArchive;
  private RadioButton rbtnPumpAllData;
  private ExTabControl tctlPumpModeSettings;
  private TabPage tpagePumpAll;
  private TabPage tpagePumpArchive;
  private TabPage tpagePumpTpList;
  private TreeView tvArchives;
  private ListView lvTpList;
  private ColumnHeader chDesignation;
  private ColumnHeader chName;
  private ContextMenuStrip cmsArchives;
  private ContextMenuStrip cmsTpList;
  private ToolStripMenuItem tsmiArchSelectAll;
  private ToolStripMenuItem tsmiArchClearAll;
  private ToolStripMenuItem tsmiArchInvertAll;
  private ToolStripSeparator tsmiArchSep1;
  private ToolStripMenuItem tsmiDocAdd;
  private ToolStripMenuItem tsmiDocDelete;
  private ToolStripMenuItem tsmiDocDeleteAll;
  private ToolStripSeparator tsmiDocSep1;
  private ToolStripMenuItem tsmiDocSelectAll;
  private ToolStripMenuItem tsmiDocClearAll;
  private ToolStripMenuItem tsmiDocInvertAll;
  private GroupBox grbTechData;
  private CheckBox chbTechDataZagot;
  private CheckBox chbTechDataMat;
  private CheckBox chbTechDataTP;
  private CheckBox chbTechDataRoute;
  private GroupBox grbTechMetaData;
  private CheckBox chbTechMetaDataAutoSelection;
  private CheckBox chbTechMetaDataScriptForms;
  private CheckBox chbTechMetaDataExpertFormulas;
  private CheckBox chbTechMetaDataExpertTables;
  private CheckBox chbTechMetaDataDocumentSettings;
  private RadioButton rbtnPumpProdZakList;
  private RadioButton rbtnPumpArtList;
  private TabPage tpagePumpArtList;
  private TabPage tpagePumpProdZakList;
  private ListView lvArtList;
  private ColumnHeader columnHeader1;
  private ColumnHeader columnHeader2;
  private ContextMenuStrip cmsArtList;
  private ToolStripMenuItem tsmiArtAdd;
  private ToolStripMenuItem tsmiArtDelete;
  private ToolStripMenuItem tsmiArtDeleteAll;
  private ToolStripSeparator toolStripSeparator1;
  private ToolStripMenuItem tsmiArtSelectAll;
  private ToolStripMenuItem tsmiArtClearAll;
  private ToolStripMenuItem tsmiArtInvertAll;
  private ListView lvProdZakList;
  private ColumnHeader columnHeader3;
  private ColumnHeader columnHeader4;
  private ContextMenuStrip cmsProdZakList;
  private ToolStripMenuItem tsmiProdZakAdd;
  private ToolStripMenuItem tsmiProdZakDelete;
  private ToolStripMenuItem tsmiProdZakDeleteAll;
  private ToolStripSeparator toolStripSeparator2;
  private ToolStripMenuItem tsmiProdZakSelectAll;
  private ToolStripMenuItem tsmiProdZakClearAll;
  private ToolStripMenuItem tsmiProdZakInvertAll;
  private ColumnHeader columnHeader5;
  private ColumnHeader columnHeader6;
  private CheckBox chbIgnoreRouteTemplate;
  private ToolTip toolTipHelper;
  private ToolStripMenuItem tsmiAddFromSelectedToImportFromSearch;
  private CheckBox chbPumpLinksOnlyWithActual;

  private static void SelectListViewItems(
    ListView targetListView,
    bool select,
    bool invertSelection)
  {
    foreach (ListViewItem listViewItem in targetListView.Items)
      listViewItem.Selected = invertSelection ? !listViewItem.Selected : select;
  }

  private static void DeleteSelectedItemsFromListView<TListViewData>(
    ListView targetListView,
    Action<TListViewData> onRemoveItem)
  {
    for (int index = targetListView.SelectedItems.Count - 1; index >= 0; --index)
    {
      ListViewItem selectedItem = targetListView.SelectedItems[index];
      TListViewData tag = (TListViewData) selectedItem.Tag;
      if (onRemoveItem != null)
        onRemoveItem(tag);
      selectedItem.Remove();
    }
  }

  public override bool isMetadataSettingsStep => false;

  private void InitializeData() => this.InitializeCustomControls();

  private void InitializeCustomControls()
  {
    this.tctlPumpModeSettings.ShowTabHeader = false;
    this.toolTipHelper.SetToolTip((Control) this.chbIgnoreRouteTemplate, "Связывать РМ и элементы РМ напрямую, игнорируя шаблоны.");
    this.toolTipHelper.SetToolTip((Control) this.chbPumpLinksOnlyWithActual, "Перекачиваются все версии объектов, но связываются только актуальные.");
  }

  private void MakeArtLVColumns(ListView listView)
  {
    ColumnHeader columnHeader1 = new ColumnHeader()
    {
      Name = "ART_ID",
      Text = "Идентификатор объекта",
      Width = 130
    };
    ColumnHeader columnHeader2 = new ColumnHeader()
    {
      Name = "DESIGNATIO",
      Text = "Обозначение",
      Width = 140
    };
    ColumnHeader columnHeader3 = new ColumnHeader()
    {
      Name = "NAME",
      Text = "Наименование",
      Width = 140
    };
    listView.Columns.Clear();
    listView.Columns.AddRange(new ColumnHeader[3]
    {
      columnHeader1,
      columnHeader2,
      columnHeader3
    });
    if (!PluginSettings.PumpArtVersions)
      return;
    ColumnHeader columnHeader4 = new ColumnHeader()
    {
      Name = "ART_VER_ID",
      Text = "Номер версии",
      Width = 90
    };
    listView.Columns.Insert(1, columnHeader4);
  }

  private void FillArtListView(ListView targetListView, List<ArtInfoLight> source)
  {
    List<ArtInfoLight> list = targetListView.SelectedItems.Cast<ListViewItem>().Where<ListViewItem>((System.Func<ListViewItem, bool>) (item => item.Tag is ArtInfoLight)).Select<ListViewItem, ArtInfoLight>((System.Func<ListViewItem, ArtInfoLight>) (item => (ArtInfoLight) item.Tag)).ToList<ArtInfoLight>();
    targetListView.BeginUpdate();
    try
    {
      targetListView.Items.Clear();
      if (source.Count <= 0)
        return;
      List<ArtInfoLight>[] artInfoLightListArray = GenericListHelper.SplitByChanks<ArtInfoLight>((IList<ArtInfoLight>) source, 1000);
      if (PluginSettings.PumpArtVersions)
      {
        foreach (IEnumerable<ArtInfoLight> source1 in artInfoLightListArray)
        {
          using (IDataReader dataReader = this._searchConnection.GetDataReader($"select ART_ID, ART_VER_ID, VART_ID, DESIGNATIO, NAME from V_ARTICLES where VART_ID in({source1.ToList<ArtInfoLight>().Select<ArtInfoLight, string>((Func<ArtInfoLight, int, string>) ((artInfo, idx) => artInfo.VArtId.ToString())).Aggregate<string>((Func<string, string, string>) ((ids, nextId) => string.IsNullOrEmpty(ids) ? nextId : $"{ids},{nextId}"))})"))
          {
            int ordinal1 = dataReader.GetOrdinal("ART_ID");
            int ordinal2 = dataReader.GetOrdinal("ART_VER_ID");
            int ordinal3 = dataReader.GetOrdinal("VART_ID");
            int ordinal4 = dataReader.GetOrdinal("DESIGNATIO");
            int ordinal5 = dataReader.GetOrdinal("NAME");
            while (dataReader.Read())
            {
              int int32Value1 = DataSetProcessor.GetInt32Value(dataReader[ordinal1], 0);
              int int32Value2 = DataSetProcessor.GetInt32Value(dataReader[ordinal2], -1);
              int int32Value3 = DataSetProcessor.GetInt32Value(dataReader[ordinal3], -1);
              string stringValue1 = DataSetProcessor.GetStringValue(dataReader[ordinal4], string.Empty);
              string stringValue2 = DataSetProcessor.GetStringValue(dataReader[ordinal5], string.Empty);
              ArtInfoLight artInfoLight = new ArtInfoLight(int32Value1, int32Value2, int32Value3);
              ListViewItem listViewItem = new ListViewItem()
              {
                Text = int32Value1.ToString()
              };
              listViewItem.SubItems.Add(int32Value2.ToString());
              listViewItem.SubItems.Add(stringValue1);
              listViewItem.SubItems.Add(stringValue2);
              listViewItem.Tag = (object) artInfoLight;
              targetListView.Items.Add(listViewItem);
            }
          }
        }
      }
      else
      {
        foreach (IEnumerable<ArtInfoLight> source2 in artInfoLightListArray)
        {
          using (IDataReader dataReader = this._searchConnection.GetDataReader($"select ART_ID, DESIGNATIO, NAME from ARTICLES where ART_ID in({source2.ToList<ArtInfoLight>().Select<ArtInfoLight, string>((Func<ArtInfoLight, int, string>) ((artInfo, idx) => artInfo.ArtId.ToString())).Aggregate<string>((Func<string, string, string>) ((ids, nextId) => string.IsNullOrEmpty(ids) ? nextId : $"{ids},{nextId}"))})"))
          {
            int ordinal6 = dataReader.GetOrdinal("ART_ID");
            int ordinal7 = dataReader.GetOrdinal("DESIGNATIO");
            int ordinal8 = dataReader.GetOrdinal("NAME");
            while (dataReader.Read())
            {
              int int32Value = DataSetProcessor.GetInt32Value(dataReader[ordinal6], 0);
              string stringValue3 = DataSetProcessor.GetStringValue(dataReader[ordinal7], string.Empty);
              string stringValue4 = DataSetProcessor.GetStringValue(dataReader[ordinal8], string.Empty);
              ArtInfoLight artInfoLight = new ArtInfoLight(int32Value);
              ListViewItem listViewItem = new ListViewItem()
              {
                Text = int32Value.ToString()
              };
              listViewItem.SubItems.Add(stringValue3);
              listViewItem.SubItems.Add(stringValue4);
              listViewItem.Tag = (object) artInfoLight;
              targetListView.Items.Add(listViewItem);
            }
          }
        }
      }
      if (list.Count <= 0)
        return;
      Comparer<ArtInfoLight> comparer = Comparer<ArtInfoLight>.Create((Comparison<ArtInfoLight>) ((left, right) =>
      {
        int num = left.ArtId.CompareTo(right.ArtId);
        return num == 0 ? left.ArtVer.CompareTo(right.ArtVer) : num;
      }));
      list.Sort((IComparer<ArtInfoLight>) comparer);
      foreach (ListViewItem listViewItem in targetListView.Items)
      {
        ArtInfoLight tag = (ArtInfoLight) listViewItem.Tag;
        if (list.BinarySearch(tag, (IComparer<ArtInfoLight>) comparer) >= 0)
          listViewItem.Selected = true;
      }
    }
    finally
    {
      targetListView.EndUpdate();
    }
  }

  protected virtual void UpdateControls()
  {
    this.tbxComplectFolder.Enabled = this.btnComplectFolder.Enabled = this.chbCompectPump.Checked;
    this.ValidateData();
  }

  protected virtual void UpdateTabPages()
  {
    switch (this.tctlPumpModeSettings.TabIndex)
    {
      case 1:
        this.UpdateTabArchiveMode();
        break;
      case 2:
        this.UpdateTabTpListMode();
        break;
      case 3:
        this.UpdateTabArtListMode();
        break;
      case 4:
        this.UpdateTabProdZakListMode();
        break;
      default:
        this.UpdateTabAllMode();
        break;
    }
  }

  protected virtual void UpdateTabAllMode()
  {
  }

  protected virtual void UpdateTabArchiveMode() => this.FillDataArchives();

  protected virtual void UpdateTabTpListMode()
  {
  }

  protected virtual void UpdateTabArtListMode()
  {
  }

  protected virtual void UpdateTabProdZakListMode()
  {
  }

  protected void UpdateArchiveNodeState(TreeNode node, bool checkState)
  {
    if (node == null)
      return;
    if (node.Checked != checkState)
      node.Checked = checkState;
    foreach (TreeNode node1 in node.Nodes)
    {
      if (node1 != node)
        this.UpdateArchiveNodeState(node1, checkState);
    }
  }

  protected virtual void ValidateData()
  {
    this.errorProvider.SetError((Control) this.tbxComplectFolder, string.Empty);
    if (!this.chbCompectPump.Checked)
      return;
    string str = string.Empty;
    string text = this.tbxComplectFolder.Text;
    if (text == string.Empty)
      str = "Директория не задана";
    else if (!Directory.Exists(text))
      str = $"Директория '{text}' не найдена";
    this.errorProvider.SetError((Control) this.tbxComplectFolder, str);
  }

  protected virtual void FillControls()
  {
    this.chbCompectPump.Checked = TechSettingsHelper.TPComplectPumpMode;
    this.tbxComplectFolder.Text = TechSettingsHelper.TPComplectPumpDir;
    switch (TechSettingsHelper.PumpMode)
    {
      case TechPumpMode.tpmArchive:
        this.rbtnPumpByArchive.Checked = true;
        this.tctlPumpModeSettings.TabIndex = 1;
        break;
      case TechPumpMode.tpmTpList:
        this.rbtnPumpTpList.Checked = true;
        this.tctlPumpModeSettings.TabIndex = 2;
        break;
      case TechPumpMode.tpmArtList:
        this.rbtnPumpArtList.Checked = true;
        this.tctlPumpModeSettings.TabIndex = 3;
        break;
      case TechPumpMode.tpmProdZakList:
        this.rbtnPumpProdZakList.Checked = true;
        this.tctlPumpModeSettings.TabIndex = 4;
        break;
      default:
        this.rbtnPumpAllData.Checked = true;
        this.tctlPumpModeSettings.TabIndex = 0;
        break;
    }
    this.chbTechMetaDataAutoSelection.Checked = TechSettingsHelper.PumpMetaDataType.HasFlag((Enum) TechPumpMetaDataType.AutoSelection);
    this.chbTechMetaDataScriptForms.Checked = TechSettingsHelper.PumpMetaDataType.HasFlag((Enum) TechPumpMetaDataType.ScriptForms);
    this.chbTechMetaDataExpertTables.Checked = TechSettingsHelper.PumpMetaDataType.HasFlag((Enum) TechPumpMetaDataType.ExpertTables);
    this.chbTechMetaDataExpertFormulas.Checked = TechSettingsHelper.PumpMetaDataType.HasFlag((Enum) TechPumpMetaDataType.ExpertFormula);
    this.chbTechMetaDataDocumentSettings.Checked = TechSettingsHelper.PumpMetaDataType.HasFlag((Enum) TechPumpMetaDataType.DocumentSettings);
    this.chbTechDataRoute.Checked = TechSettingsHelper.PumpDataType.HasFlag((Enum) TechPumpDataType.Route);
    this.chbTechDataZagot.Checked = TechSettingsHelper.PumpDataType.HasFlag((Enum) TechPumpDataType.Zagot);
    this.chbTechDataMat.Checked = TechSettingsHelper.PumpDataType.HasFlag((Enum) TechPumpDataType.MatGroup);
    this.chbTechDataTP.Checked = TechSettingsHelper.PumpDataType.HasFlag((Enum) TechPumpDataType.TechProc);
    this.chbIgnoreRouteTemplate.Checked = TechSettingsHelper.IgnoreRouteTemplates;
    this.chbPumpLinksOnlyWithActual.Checked = TechSettingsHelper.PumpLinksOnlyWithActual;
    using (StepControlProgress stepControlProgress = new StepControlProgress())
    {
      stepControlProgress.Text = "Подготовка данных";
      stepControlProgress.SetCenterParentLocation(TechcardConsts.Plugin.appManager as Control);
      stepControlProgress.SetProgress("Загрузка информации по архивам", 0);
      stepControlProgress.Visible = true;
      this.FillDataArchives();
      stepControlProgress.SetProgress("Загрузка информации по документам", 30);
      this.FillDataTpList();
      stepControlProgress.SetProgress("Загрузка информации по изделиям", 50);
      this.FillDataArtList();
      stepControlProgress.SetProgress("Загрузка информации по производственным заказам", 75);
      this.FillDataProdZakList();
      stepControlProgress.SetProgress("Подготовка данных завершена", 100);
    }
    this.ValidateData();
  }

  protected virtual void FillDataArchives()
  {
    if (this._archivesLoaded)
      return;
    this.LoadDataArchives();
    if (!this._archivesLoaded)
      return;
    this.FillControl_Archives();
  }

  protected virtual void FillControl_Archives()
  {
    this._archiveNodeUpdate = true;
    try
    {
      List<int> intList = new List<int>((IEnumerable<int>) TechSettingsHelper.PumpArchiveIDS);
      intList.Sort();
      foreach (KeyValuePair<int, TreeNode> keyValuePair in this._archiveNodeCache)
      {
        if (intList.BinarySearch(keyValuePair.Key) >= 0)
          keyValuePair.Value.Checked = true;
      }
    }
    finally
    {
      this._archiveNodeUpdate = false;
    }
  }

  protected virtual void FillDataTpList()
  {
    this.LoadDataTpList();
    this.FillControl_TPList();
  }

  protected virtual void FillDataArtList()
  {
    this.LoadDataArtList();
    this.FillControl_ArtList();
  }

  protected virtual void FillDataProdZakList()
  {
    this.LoadDataProdZakList();
    this.FillControl_ProdZakList();
  }

  protected virtual void FillControl_TPList()
  {
    List<int> intList = new List<int>(this.lvTpList.SelectedItems.Count);
    foreach (ListViewItem selectedItem in this.lvTpList.SelectedItems)
      intList.Add(Convert.ToInt32(selectedItem.Tag));
    DataTable toTable = (DataTable) null;
    if (this._docList.Count > 0)
    {
      foreach (List<int> splitByChank in GenericListHelper.SplitByChanks<int>((IList<int>) this._docList, 1000))
      {
        DataTable docTable = TechSettingsDocList.GetDocTable($"A.F_KEY IN ({string.Join(",", Array.ConvertAll<int, string>(splitByChank.ToArray(), new Converter<int, string>(Convert.ToString)))})");
        if (toTable == null)
          toTable = docTable;
        else
          DataSetProcessor.AddTable(toTable, docTable, false);
      }
    }
    this.lvTpList.BeginUpdate();
    try
    {
      this.lvTpList.Items.Clear();
      if (toTable != null && toTable.Rows.Count > 0)
      {
        int columnIndex1 = toTable.Columns.IndexOf("F_KEY");
        int columnIndex2 = toTable.Columns.IndexOf("F_DESIGNATION");
        int columnIndex3 = toTable.Columns.IndexOf("F_NAME");
        int columnIndex4 = toTable.Columns.IndexOf("F_VERSION");
        foreach (DataRow row in (InternalDataCollectionBase) toTable.Rows)
        {
          if (row != null)
          {
            int int32 = Convert.ToInt32(row[columnIndex1]);
            string str1 = row[columnIndex2].ToString();
            string text = row[columnIndex3].ToString();
            string str2 = row[columnIndex4].ToString();
            ListViewItem listViewItem = new ListViewItem()
            {
              Text = $"{str1}[{str2}]"
            };
            listViewItem.SubItems.Add(text);
            listViewItem.Tag = (object) int32;
            this.lvTpList.Items.Add(listViewItem);
          }
        }
      }
      if (intList.Count <= 0)
        return;
      intList.Sort();
      foreach (ListViewItem listViewItem in this.lvTpList.Items)
      {
        int int32 = Convert.ToInt32(listViewItem.Tag);
        if (intList.BinarySearch(int32) >= 0)
          listViewItem.Selected = true;
      }
    }
    finally
    {
      this.lvTpList.EndUpdate();
    }
  }

  protected virtual void FillControl_ArtList()
  {
    this.FillArtListView(this.lvArtList, this._artList);
  }

  protected virtual void FillControl_ProdZakList()
  {
    this.FillArtListView(this.lvProdZakList, this._prodZakList);
  }

  protected virtual void LoadDataArchives()
  {
    if (this._archivesLoaded)
      return;
    this.tvArchives.BeginUpdate();
    try
    {
      this.tvArchives.Nodes.Clear();
      using (IDataReader dataReader = this._searchConnection.GetDataReader("SELECT * FROM ARCHIVES ORDER BY ARCHIVE_ID"))
      {
        int ordinal1 = dataReader.GetOrdinal("ARCHIVE_ID");
        int ordinal2 = dataReader.GetOrdinal("PARENT_ID");
        int ordinal3 = dataReader.GetOrdinal("DESCRIPTIO");
        this._archiveNodeCache = new Dictionary<int, TreeNode>();
        try
        {
          while (dataReader.Read())
          {
            int int32_1 = Convert.ToInt32(dataReader[ordinal1]);
            int int32_2 = Convert.ToInt32(dataReader[ordinal2]);
            TreeNode node = new TreeNode(dataReader[ordinal3].ToString())
            {
              Tag = (object) int32_1
            };
            this._archiveNodeCache.Add(int32_1, node);
            TreeNode treeNode;
            if (int32_1 != int32_2 && this._archiveNodeCache.TryGetValue(int32_2, out treeNode))
              treeNode.Nodes.Add(node);
            else
              this.tvArchives.Nodes.Add(node);
          }
        }
        finally
        {
          dataReader.Close();
        }
      }
    }
    finally
    {
      this.tvArchives.EndUpdate();
    }
    this._archivesLoaded = true;
  }

  protected virtual void LoadDataTpList()
  {
    this._docList.Clear();
    this._docList.AddRange((IEnumerable<int>) TechSettingsHelper.PumpDocList);
  }

  protected virtual void LoadDataArtList()
  {
    this._artList.Clear();
    this._artList.AddRange((IEnumerable<ArtInfoLight>) TechSettingsHelper.PumpArtList);
  }

  protected virtual void LoadDataProdZakList()
  {
    this._prodZakList.Clear();
    this._prodZakList.AddRange((IEnumerable<ArtInfoLight>) TechSettingsHelper.PumpProdZakList);
  }

  protected virtual void SaveDataArchives()
  {
    List<int> intList1 = new List<int>();
    List<int> intList2 = new List<int>();
    if (this._archiveNodeCache != null)
    {
      foreach (KeyValuePair<int, TreeNode> keyValuePair in this._archiveNodeCache)
      {
        if (keyValuePair.Value.Checked)
          intList1.Add(keyValuePair.Key);
      }
      TechSettingsHelper.PumpArchiveIDS = intList1;
    }
    if (intList1.Count == 0)
      TechSettingsHelper.PumpArchiveDocIDS = intList1;
    else if (this._searchConnection == null)
    {
      TechcardConsts.Plugin.appManager.AddErrorMessage($"Подключение к базе Search \"{"SEARCH PLUGIN CONNECTION"}\" не найдено ");
    }
    else
    {
      using (IDataReader dataReader = this._searchConnection.GetDataReader($"SELECT DOC_ID FROM DOCLIST WHERE DOC_ID > 0 AND ARCHIVE_ID IN ({string.Join(",", Array.ConvertAll<int, string>(intList1.ToArray(), new Converter<int, string>(Convert.ToString)))})"))
      {
        int ordinal = dataReader.GetOrdinal("DOC_ID");
        try
        {
          while (dataReader.Read())
            intList2.Add(Convert.ToInt32(dataReader[ordinal]));
        }
        finally
        {
          dataReader.Close();
        }
      }
      TechSettingsHelper.PumpArchiveDocIDS = intList2;
    }
  }

  protected virtual void SaveDataTpList() => TechSettingsHelper.PumpDocList = this._docList;

  protected virtual void SaveDataArtList() => TechSettingsHelper.PumpArtList = this._artList;

  protected virtual void SaveDataProdZakList()
  {
    TechSettingsHelper.PumpProdZakList = this._prodZakList;
  }

  protected virtual void LoadTechSettings()
  {
    if (this._settingsLoaded)
      return;
    this._searchConnection = SearchConnectionsManager.GetConnection();
    if (this._searchConnection == null)
      return;
    this.MakeArtLVColumns(this.lvArtList);
    this.MakeArtLVColumns(this.lvProdZakList);
    this.FillControls();
    this._settingsLoaded = true;
  }

  protected virtual void SaveTechSettings()
  {
    if (!this._settingsLoaded)
      return;
    TechSettingsHelper.TPComplectPumpMode = this.chbCompectPump.Checked;
    TechSettingsHelper.TPComplectPumpDir = this.tbxComplectFolder.Text;
    TechSettingsHelper.PumpMode = (TechPumpMode) this.tctlPumpModeSettings.SelectedIndex;
    TechPumpMetaDataType pumpMetaDataType = TechPumpMetaDataType.None;
    if (this.chbTechMetaDataAutoSelection.Checked)
      pumpMetaDataType |= TechPumpMetaDataType.AutoSelection;
    if (this.chbTechMetaDataScriptForms.Checked)
      pumpMetaDataType |= TechPumpMetaDataType.ScriptForms;
    if (this.chbTechMetaDataExpertTables.Checked)
      pumpMetaDataType |= TechPumpMetaDataType.ExpertTables;
    if (this.chbTechMetaDataExpertFormulas.Checked)
      pumpMetaDataType |= TechPumpMetaDataType.ExpertFormula;
    if (this.chbTechMetaDataDocumentSettings.Checked)
      pumpMetaDataType |= TechPumpMetaDataType.DocumentSettings;
    TechSettingsHelper.PumpMetaDataType = pumpMetaDataType;
    TechPumpDataType techPumpDataType = TechPumpDataType.None;
    if (this.chbTechDataRoute.Checked)
      techPumpDataType |= TechPumpDataType.Route;
    if (this.chbTechDataZagot.Checked)
      techPumpDataType |= TechPumpDataType.Zagot;
    if (this.chbTechDataMat.Checked)
      techPumpDataType |= TechPumpDataType.MatGroup;
    if (this.chbTechDataTP.Checked)
      techPumpDataType |= TechPumpDataType.TechProc;
    TechSettingsHelper.PumpDataType = techPumpDataType;
    TechSettingsHelper.IgnoreRouteTemplates = this.chbIgnoreRouteTemplate.Checked;
    TechSettingsHelper.PumpLinksOnlyWithActual = this.chbPumpLinksOnlyWithActual.Checked;
    this.SaveDataArchives();
    this.SaveDataTpList();
    this.SaveDataArtList();
    this.SaveDataProdZakList();
    TechSettingsHelper.SaveSettings();
  }

  public TechSettingsControl(object owner)
    : base(owner)
  {
    this.InitializeComponent();
    this.stepPrevAllowed = true;
    this.stepRepumpble = true;
    this.InitializeData();
  }

  protected override string getCaption() => "Настройка перекачки технологических данных";

  public override SaveSettingsResult SaveSettings()
  {
    this.SaveTechSettings();
    return base.SaveSettings();
  }

  protected override Image getImage()
  {
    if (this._image == null && ServicesManager.GetService(typeof (IBigImageList)) is IBigImageList service)
    {
      int index = service.ImageIndex("imgTechParams");
      if (index != -1)
        this._image = service.ImageList.Images[index];
    }
    return this._image;
  }

  public override bool LeaveControl()
  {
    base.LeaveControl();
    this.SaveTechSettings();
    return true;
  }

  public override void RefreshControl()
  {
    base.RefreshControl();
    this.LoadTechSettings();
  }

  private void chbCompectPump_CheckedChanged(object sender, EventArgs e) => this.UpdateControls();

  private void tbxComplectFolder_TextChanged(object sender, EventArgs e) => this.ValidateData();

  private void btnComplectFolder_Click(object sender, EventArgs e)
  {
    this.folderBrowserDialog.SelectedPath = this.tbxComplectFolder.Text;
    if (this.folderBrowserDialog.ShowDialog() != DialogResult.OK)
      return;
    this.tbxComplectFolder.Text = this.folderBrowserDialog.SelectedPath;
    this.ValidateData();
  }

  private void rbtnPumpAllData_CheckedChanged(object sender, EventArgs e)
  {
    if (!(sender is RadioButton radioButton) || !radioButton.Checked)
      return;
    this.tctlPumpModeSettings.SelectedIndex = Convert.ToInt32(radioButton.Tag);
  }

  private void tvArchives_AfterCheck(object sender, TreeViewEventArgs e)
  {
    if (this._archiveNodeUpdate || e.Action == TreeViewAction.Unknown || !e.Node.Checked || e.Node.Nodes.Count == 0)
      return;
    this.UpdateArchiveNodeState(e.Node, e.Node.Checked);
  }

  private void tvArchives_BeforeCheck(object sender, TreeViewCancelEventArgs e)
  {
  }

  private void cmsArchives_Opening(object sender, CancelEventArgs e)
  {
    this.tsmiArchClearAll.Enabled = this.tsmiArchSelectAll.Enabled = this.tsmiArchInvertAll.Enabled = this.tvArchives.Nodes.Count > 0;
  }

  private void tsmiArchInvert(object sender, EventArgs e)
  {
    if (this._archiveNodeCache == null)
      return;
    this._archiveNodeUpdate = true;
    try
    {
      foreach (TreeNode treeNode in this._archiveNodeCache.Values)
        treeNode.Checked = !treeNode.Checked;
    }
    finally
    {
      this._archiveNodeUpdate = false;
    }
  }

  private void tsmiArchSelectAll_Click(object sender, EventArgs e)
  {
    if (this._archiveNodeCache == null)
      return;
    this._archiveNodeUpdate = true;
    try
    {
      foreach (TreeNode treeNode in this._archiveNodeCache.Values)
        treeNode.Checked = true;
    }
    finally
    {
      this._archiveNodeUpdate = false;
    }
  }

  private void tsmiArchClearAll_Click(object sender, EventArgs e)
  {
    if (this._archiveNodeCache == null)
      return;
    this._archiveNodeUpdate = true;
    try
    {
      foreach (TreeNode treeNode in this._archiveNodeCache.Values)
        treeNode.Checked = false;
    }
    finally
    {
      this._archiveNodeUpdate = false;
    }
  }

  private void tsmiDocAdd_Click(object sender, EventArgs e)
  {
    TechSettingsDocList techSettingsDocList = new TechSettingsDocList();
    if (techSettingsDocList.ShowDialog() != DialogResult.OK)
      return;
    this._docList.AddRange((IEnumerable<int>) techSettingsDocList.DocList);
    GenericListHelper.MakeUnique<int>(this._docList);
    this.FillControl_TPList();
  }

  private void tsmiDocDelete_Click(object sender, EventArgs e)
  {
    TechSettingsControl.DeleteSelectedItemsFromListView<int>(this.lvTpList, (Action<int>) (prodZakiInfo => this._docList.Remove(prodZakiInfo)));
  }

  private void tsmiDocDeleteAll_Click(object sender, EventArgs e)
  {
    this.lvTpList.Items.Clear();
    this._docList.Clear();
  }

  private void tsmiDocSelectAll_Click(object sender, EventArgs e)
  {
    TechSettingsControl.SelectListViewItems(this.lvTpList, true, false);
  }

  private void tsmiDocClearAll_Click(object sender, EventArgs e)
  {
    TechSettingsControl.SelectListViewItems(this.lvTpList, false, false);
  }

  private void tsmiDocInvertAll_Click(object sender, EventArgs e)
  {
    TechSettingsControl.SelectListViewItems(this.lvTpList, false, true);
  }

  private void cmsTpList_Opening(object sender, CancelEventArgs e)
  {
    this.tsmiDocDeleteAll.Enabled = this.tsmiDocClearAll.Enabled = this.tsmiDocSelectAll.Enabled = this.tsmiDocInvertAll.Enabled = this.lvTpList.Items.Count > 0;
    this.tsmiDocDelete.Enabled = this.lvTpList.SelectedItems.Count > 0;
  }

  private void tctlPumpModeSettings_TabIndexChanged(object sender, EventArgs e)
  {
    this.UpdateTabPages();
  }

  private void tsmiArtAdd_Click(object sender, EventArgs e)
  {
    ArtSelectionDialog artSelectionDialog = new ArtSelectionDialog();
    artSelectionDialog.LoadArtsTable(string.Empty);
    if (artSelectionDialog.ShowDialog() != DialogResult.OK)
      return;
    this._artList.AddRange(artSelectionDialog.ArtList);
    GenericListHelper.MakeUnique<ArtInfoLight>(this._artList, (Comparison<ArtInfoLight>) ((left, right) =>
    {
      int num = left.ArtId - right.ArtId;
      return num == 0 ? left.ArtVer - right.ArtVer : num;
    }));
    this.FillControl_ArtList();
  }

  private void tsmiArtDelete_Click(object sender, EventArgs e)
  {
    TechSettingsControl.DeleteSelectedItemsFromListView<ArtInfoLight>(this.lvArtList, (Action<ArtInfoLight>) (artiInfo => this._artList.Remove(artiInfo)));
  }

  private void tsmiArtDeleteAll_Click(object sender, EventArgs e)
  {
    this.lvArtList.Items.Clear();
    this._artList.Clear();
  }

  private void tsmiArtSelectAll_Click(object sender, EventArgs e)
  {
    TechSettingsControl.SelectListViewItems(this.lvArtList, true, false);
  }

  private void tsmiArtClearAll_Click(object sender, EventArgs e)
  {
    TechSettingsControl.SelectListViewItems(this.lvArtList, false, false);
  }

  private void tsmiArtInvertAll_Click(object sender, EventArgs e)
  {
    TechSettingsControl.SelectListViewItems(this.lvArtList, false, true);
  }

  private void tsmiProdZakAdd_Click(object sender, EventArgs e)
  {
    ArtSelectionDialog artSelectionDialog = new ArtSelectionDialog();
    artSelectionDialog.LoadArtsTable($"{"SECTION_ID"} = {99999990}");
    if (artSelectionDialog.ShowDialog() != DialogResult.OK)
      return;
    this._prodZakList.AddRange(artSelectionDialog.ArtList);
    GenericListHelper.MakeUnique<ArtInfoLight>(this._artList, (Comparison<ArtInfoLight>) ((left, right) =>
    {
      int num = left.ArtId - right.ArtId;
      return num == 0 ? left.ArtVer - right.ArtVer : num;
    }));
    this.FillControl_ProdZakList();
  }

  private void tsmiProdZakAddFromSelectedToImportFromSearch_Click(object sender, EventArgs e)
  {
    List<ArtInfoLight> artInfoLightList = (ServicesManager.GetService(typeof (ICache)) as ICache).GetCache(ImportingCategory.EnabledProductionLists).GetCategory().Keys.Select<object, ArtInfoLight>((System.Func<object, ArtInfoLight>) (zakData => new ArtInfoLight(Convert.ToInt32(zakData)))).ToList<ArtInfoLight>();
    if (PluginSettings.PumpArtVersions)
      artInfoLightList = this.SelectZaksVersions(artInfoLightList);
    this._prodZakList.AddRange((IEnumerable<ArtInfoLight>) artInfoLightList);
    GenericListHelper.MakeUnique<ArtInfoLight>(this._artList, (Comparison<ArtInfoLight>) ((left, right) =>
    {
      int num = left.ArtId - right.ArtId;
      return num == 0 ? left.ArtVer - right.ArtVer : num;
    }));
    this.FillControl_ProdZakList();
  }

  private List<ArtInfoLight> SelectZaksVersions(List<ArtInfoLight> selectedZaks)
  {
    List<ArtInfoLight> artInfoLightList = new List<ArtInfoLight>();
    foreach (IEnumerable<ArtInfoLight> splitByChank in GenericListHelper.SplitByChanks<ArtInfoLight>((IList<ArtInfoLight>) selectedZaks, 1000))
    {
      using (IDataReader dataReader = this._searchConnection.GetDataReader($"select ART_ID, ART_VER_ID, VART_ID from V_ARTICLES where ART_ID in ({splitByChank.Select<ArtInfoLight, string>((System.Func<ArtInfoLight, string>) (zakInfo => zakInfo.ArtId.ToString())).Aggregate<string>((Func<string, string, string>) ((ids, nextId) => string.IsNullOrEmpty(ids) ? nextId : $"{ids},{nextId}"))})"))
      {
        int ordinal1 = dataReader.GetOrdinal("ART_ID");
        int ordinal2 = dataReader.GetOrdinal("ART_VER_ID");
        int ordinal3 = dataReader.GetOrdinal("VART_ID");
        while (dataReader.Read())
        {
          ArtInfoLight artInfoLight = new ArtInfoLight(DataSetProcessor.GetInt32Value(dataReader[ordinal1], 0), DataSetProcessor.GetInt32Value(dataReader[ordinal2], -1), DataSetProcessor.GetInt32Value(dataReader[ordinal3], -1));
          artInfoLightList.Add(artInfoLight);
        }
      }
    }
    return artInfoLightList;
  }

  private void tsmiProdZakDelete_Click(object sender, EventArgs e)
  {
    TechSettingsControl.DeleteSelectedItemsFromListView<ArtInfoLight>(this.lvProdZakList, (Action<ArtInfoLight>) (prodZakInfo => this._prodZakList.Remove(prodZakInfo)));
  }

  private void tsmiProdZakDeleteAll_Click(object sender, EventArgs e)
  {
    this.lvProdZakList.Items.Clear();
    this._prodZakList.Clear();
  }

  private void tsmiProdZakSelectAll_Click(object sender, EventArgs e)
  {
    TechSettingsControl.SelectListViewItems(this.lvProdZakList, true, false);
  }

  private void tsmiProdZakClearAll_Click(object sender, EventArgs e)
  {
    TechSettingsControl.SelectListViewItems(this.lvProdZakList, false, false);
  }

  private void tsmiProdZakInvertAll_Click(object sender, EventArgs e)
  {
    TechSettingsControl.SelectListViewItems(this.lvProdZakList, false, true);
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
    TreeNode treeNode1 = new TreeNode("Node3");
    TreeNode treeNode2 = new TreeNode("Node4");
    TreeNode treeNode3 = new TreeNode("Node1", new TreeNode[2]
    {
      treeNode1,
      treeNode2
    });
    TreeNode treeNode4 = new TreeNode("Node2");
    TreeNode treeNode5 = new TreeNode("Node0", new TreeNode[2]
    {
      treeNode3,
      treeNode4
    });
    ListViewItem listViewItem = new ListViewItem(new string[2]
    {
      "Документ 1",
      "Док"
    }, -1);
    this.grbMain = new GroupBox();
    this.grbTechData = new GroupBox();
    this.chbIgnoreRouteTemplate = new CheckBox();
    this.chbTechDataZagot = new CheckBox();
    this.chbTechDataMat = new CheckBox();
    this.chbTechDataTP = new CheckBox();
    this.chbTechDataRoute = new CheckBox();
    this.grbTechMetaData = new GroupBox();
    this.chbTechMetaDataDocumentSettings = new CheckBox();
    this.chbTechMetaDataAutoSelection = new CheckBox();
    this.chbTechMetaDataScriptForms = new CheckBox();
    this.chbTechMetaDataExpertFormulas = new CheckBox();
    this.chbTechMetaDataExpertTables = new CheckBox();
    this.grbPumpMode = new GroupBox();
    this.rbtnPumpProdZakList = new RadioButton();
    this.rbtnPumpArtList = new RadioButton();
    this.tctlPumpModeSettings = new ExTabControl();
    this.tpagePumpAll = new TabPage();
    this.tpagePumpArchive = new TabPage();
    this.tvArchives = new TreeView();
    this.cmsArchives = new ContextMenuStrip(this.components);
    this.tsmiArchSelectAll = new ToolStripMenuItem();
    this.tsmiArchClearAll = new ToolStripMenuItem();
    this.tsmiArchSep1 = new ToolStripSeparator();
    this.tsmiArchInvertAll = new ToolStripMenuItem();
    this.tpagePumpTpList = new TabPage();
    this.lvTpList = new ListView();
    this.chDesignation = new ColumnHeader();
    this.chName = new ColumnHeader();
    this.cmsTpList = new ContextMenuStrip(this.components);
    this.tsmiDocAdd = new ToolStripMenuItem();
    this.tsmiDocDelete = new ToolStripMenuItem();
    this.tsmiDocDeleteAll = new ToolStripMenuItem();
    this.tsmiDocSep1 = new ToolStripSeparator();
    this.tsmiDocSelectAll = new ToolStripMenuItem();
    this.tsmiDocClearAll = new ToolStripMenuItem();
    this.tsmiDocInvertAll = new ToolStripMenuItem();
    this.tpagePumpArtList = new TabPage();
    this.lvArtList = new ListView();
    this.columnHeader5 = new ColumnHeader();
    this.columnHeader1 = new ColumnHeader();
    this.columnHeader2 = new ColumnHeader();
    this.cmsArtList = new ContextMenuStrip(this.components);
    this.tsmiArtAdd = new ToolStripMenuItem();
    this.tsmiArtDelete = new ToolStripMenuItem();
    this.tsmiArtDeleteAll = new ToolStripMenuItem();
    this.toolStripSeparator1 = new ToolStripSeparator();
    this.tsmiArtSelectAll = new ToolStripMenuItem();
    this.tsmiArtClearAll = new ToolStripMenuItem();
    this.tsmiArtInvertAll = new ToolStripMenuItem();
    this.tpagePumpProdZakList = new TabPage();
    this.lvProdZakList = new ListView();
    this.columnHeader6 = new ColumnHeader();
    this.columnHeader3 = new ColumnHeader();
    this.columnHeader4 = new ColumnHeader();
    this.cmsProdZakList = new ContextMenuStrip(this.components);
    this.tsmiProdZakAdd = new ToolStripMenuItem();
    this.tsmiAddFromSelectedToImportFromSearch = new ToolStripMenuItem();
    this.tsmiProdZakDelete = new ToolStripMenuItem();
    this.tsmiProdZakDeleteAll = new ToolStripMenuItem();
    this.toolStripSeparator2 = new ToolStripSeparator();
    this.tsmiProdZakSelectAll = new ToolStripMenuItem();
    this.tsmiProdZakClearAll = new ToolStripMenuItem();
    this.tsmiProdZakInvertAll = new ToolStripMenuItem();
    this.rbtnPumpTpList = new RadioButton();
    this.rbtnPumpByArchive = new RadioButton();
    this.rbtnPumpAllData = new RadioButton();
    this.grbTechProc = new GroupBox();
    this.lblComplectFolder = new Label();
    this.btnComplectFolder = new Button();
    this.tbxComplectFolder = new TextBox();
    this.chbCompectPump = new CheckBox();
    this.errorProvider = new ErrorProvider(this.components);
    this.folderBrowserDialog = new FolderBrowserDialog();
    this.toolTipHelper = new ToolTip(this.components);
    this.chbPumpLinksOnlyWithActual = new CheckBox();
    this.grbMain.SuspendLayout();
    this.grbTechData.SuspendLayout();
    this.grbTechMetaData.SuspendLayout();
    this.grbPumpMode.SuspendLayout();
    this.tctlPumpModeSettings.SuspendLayout();
    this.tpagePumpArchive.SuspendLayout();
    this.cmsArchives.SuspendLayout();
    this.tpagePumpTpList.SuspendLayout();
    this.cmsTpList.SuspendLayout();
    this.tpagePumpArtList.SuspendLayout();
    this.cmsArtList.SuspendLayout();
    this.tpagePumpProdZakList.SuspendLayout();
    this.cmsProdZakList.SuspendLayout();
    this.grbTechProc.SuspendLayout();
    ((ISupportInitialize) this.errorProvider).BeginInit();
    this.SuspendLayout();
    this.grbMain.Controls.Add((Control) this.grbTechData);
    this.grbMain.Controls.Add((Control) this.grbTechMetaData);
    this.grbMain.Controls.Add((Control) this.grbPumpMode);
    this.grbMain.Controls.Add((Control) this.grbTechProc);
    this.grbMain.Dock = DockStyle.Fill;
    this.grbMain.Location = new Point(0, 0);
    this.grbMain.Name = "grbMain";
    this.grbMain.Padding = new Padding(10);
    this.grbMain.Size = new Size(594, 586);
    this.grbMain.TabIndex = 4;
    this.grbMain.TabStop = false;
    this.grbMain.Text = "Настройки перекачки TechCard";
    this.grbTechData.Controls.Add((Control) this.chbPumpLinksOnlyWithActual);
    this.grbTechData.Controls.Add((Control) this.chbIgnoreRouteTemplate);
    this.grbTechData.Controls.Add((Control) this.chbTechDataZagot);
    this.grbTechData.Controls.Add((Control) this.chbTechDataMat);
    this.grbTechData.Controls.Add((Control) this.chbTechDataTP);
    this.grbTechData.Controls.Add((Control) this.chbTechDataRoute);
    this.grbTechData.Dock = DockStyle.Top;
    this.grbTechData.Location = new Point(10, 175);
    this.grbTechData.Name = "grbTechData";
    this.grbTechData.Size = new Size(574, 110);
    this.grbTechData.TabIndex = 12;
    this.grbTechData.TabStop = false;
    this.grbTechData.Text = "Перекачиваемые данные";
    this.chbIgnoreRouteTemplate.AutoSize = true;
    this.chbIgnoreRouteTemplate.Location = new Point(213, 19);
    this.chbIgnoreRouteTemplate.Name = "chbIgnoreRouteTemplate";
    this.chbIgnoreRouteTemplate.Size = new Size(252, 17);
    this.chbIgnoreRouteTemplate.TabIndex = 4;
    this.chbIgnoreRouteTemplate.Text = " Связывать РМ с элементами РМ напрямую";
    this.chbIgnoreRouteTemplate.UseVisualStyleBackColor = true;
    this.chbTechDataZagot.AutoSize = true;
    this.chbTechDataZagot.Location = new Point(33, 65);
    this.chbTechDataZagot.Name = "chbTechDataZagot";
    this.chbTechDataZagot.Size = new Size(79, 17);
    this.chbTechDataZagot.TabIndex = 3;
    this.chbTechDataZagot.Text = "Заготовки";
    this.chbTechDataZagot.UseVisualStyleBackColor = true;
    this.chbTechDataMat.AutoSize = true;
    this.chbTechDataMat.Location = new Point(33, 88);
    this.chbTechDataMat.Name = "chbTechDataMat";
    this.chbTechDataMat.Size = new Size(145, 17);
    this.chbTechDataMat.TabIndex = 2;
    this.chbTechDataMat.Text = "Наборы ВМ на изделие";
    this.chbTechDataMat.UseVisualStyleBackColor = true;
    this.chbTechDataTP.AutoSize = true;
    this.chbTechDataTP.Location = new Point(33, 42);
    this.chbTechDataTP.Name = "chbTechDataTP";
    this.chbTechDataTP.Size = new Size(94, 17);
    this.chbTechDataTP.TabIndex = 1;
    this.chbTechDataTP.Text = "Техпроцессы";
    this.chbTechDataTP.UseVisualStyleBackColor = true;
    this.chbTechDataRoute.AutoSize = true;
    this.chbTechDataRoute.Location = new Point(33, 19);
    this.chbTechDataRoute.Name = "chbTechDataRoute";
    this.chbTechDataRoute.Size = new Size(160 /*0xA0*/, 17);
    this.chbTechDataRoute.TabIndex = 0;
    this.chbTechDataRoute.Text = "Расцеховочные маршруты";
    this.chbTechDataRoute.UseVisualStyleBackColor = true;
    this.grbTechMetaData.Controls.Add((Control) this.chbTechMetaDataDocumentSettings);
    this.grbTechMetaData.Controls.Add((Control) this.chbTechMetaDataAutoSelection);
    this.grbTechMetaData.Controls.Add((Control) this.chbTechMetaDataScriptForms);
    this.grbTechMetaData.Controls.Add((Control) this.chbTechMetaDataExpertFormulas);
    this.grbTechMetaData.Controls.Add((Control) this.chbTechMetaDataExpertTables);
    this.grbTechMetaData.Dock = DockStyle.Top;
    this.grbTechMetaData.Location = new Point(10, 99);
    this.grbTechMetaData.Name = "grbTechMetaData";
    this.grbTechMetaData.Size = new Size(574, 76);
    this.grbTechMetaData.TabIndex = 11;
    this.grbTechMetaData.TabStop = false;
    this.grbTechMetaData.Text = "Перекачиваемые метаданные";
    this.chbTechMetaDataDocumentSettings.AutoSize = true;
    this.chbTechMetaDataDocumentSettings.Location = new Point(213, 42);
    this.chbTechMetaDataDocumentSettings.Name = "chbTechMetaDataDocumentSettings";
    this.chbTechMetaDataDocumentSettings.Size = new Size(144 /*0x90*/, 17);
    this.chbTechMetaDataDocumentSettings.TabIndex = 4;
    this.chbTechMetaDataDocumentSettings.Text = "Настройки документов";
    this.chbTechMetaDataDocumentSettings.UseVisualStyleBackColor = true;
    this.chbTechMetaDataAutoSelection.AutoSize = true;
    this.chbTechMetaDataAutoSelection.Location = new Point(213, 19);
    this.chbTechMetaDataAutoSelection.Name = "chbTechMetaDataAutoSelection";
    this.chbTechMetaDataAutoSelection.Size = new Size(135, 17);
    this.chbTechMetaDataAutoSelection.TabIndex = 3;
    this.chbTechMetaDataAutoSelection.Text = "Данные автоподбора";
    this.chbTechMetaDataAutoSelection.UseVisualStyleBackColor = true;
    this.chbTechMetaDataScriptForms.AutoSize = true;
    this.chbTechMetaDataScriptForms.Location = new Point(368, 19);
    this.chbTechMetaDataScriptForms.Name = "chbTechMetaDataScriptForms";
    this.chbTechMetaDataScriptForms.Size = new Size(153, 17);
    this.chbTechMetaDataScriptForms.TabIndex = 2;
    this.chbTechMetaDataScriptForms.Text = "Сценарии (формы ввода)";
    this.chbTechMetaDataScriptForms.UseVisualStyleBackColor = true;
    this.chbTechMetaDataExpertFormulas.AutoSize = true;
    this.chbTechMetaDataExpertFormulas.Location = new Point(33, 42);
    this.chbTechMetaDataExpertFormulas.Name = "chbTechMetaDataExpertFormulas";
    this.chbTechMetaDataExpertFormulas.Size = new Size(93, 17);
    this.chbTechMetaDataExpertFormulas.TabIndex = 1;
    this.chbTechMetaDataExpertFormulas.Text = "Формулы ЭС";
    this.chbTechMetaDataExpertFormulas.UseVisualStyleBackColor = true;
    this.chbTechMetaDataExpertTables.AutoSize = true;
    this.chbTechMetaDataExpertTables.Location = new Point(33, 19);
    this.chbTechMetaDataExpertTables.Name = "chbTechMetaDataExpertTables";
    this.chbTechMetaDataExpertTables.Size = new Size(88, 17);
    this.chbTechMetaDataExpertTables.TabIndex = 0;
    this.chbTechMetaDataExpertTables.Text = "Таблицы ЭС";
    this.chbTechMetaDataExpertTables.UseVisualStyleBackColor = true;
    this.grbPumpMode.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.grbPumpMode.Controls.Add((Control) this.rbtnPumpProdZakList);
    this.grbPumpMode.Controls.Add((Control) this.rbtnPumpArtList);
    this.grbPumpMode.Controls.Add((Control) this.tctlPumpModeSettings);
    this.grbPumpMode.Controls.Add((Control) this.rbtnPumpTpList);
    this.grbPumpMode.Controls.Add((Control) this.rbtnPumpByArchive);
    this.grbPumpMode.Controls.Add((Control) this.rbtnPumpAllData);
    this.grbPumpMode.Location = new Point(10, 291);
    this.grbPumpMode.Name = "grbPumpMode";
    this.grbPumpMode.Size = new Size(574, 286);
    this.grbPumpMode.TabIndex = 7;
    this.grbPumpMode.TabStop = false;
    this.grbPumpMode.Text = "Режим закачки";
    this.rbtnPumpProdZakList.AutoSize = true;
    this.rbtnPumpProdZakList.Location = new Point(213, 42);
    this.rbtnPumpProdZakList.Name = "rbtnPumpProdZakList";
    this.rbtnPumpProdZakList.Size = new Size(220, 17);
    this.rbtnPumpProdZakList.TabIndex = 5;
    this.rbtnPumpProdZakList.Tag = (object) "4";
    this.rbtnPumpProdZakList.Text = "По составу прозводственных заказов";
    this.rbtnPumpProdZakList.UseVisualStyleBackColor = true;
    this.rbtnPumpProdZakList.CheckedChanged += new EventHandler(this.rbtnPumpAllData_CheckedChanged);
    this.rbtnPumpArtList.AutoSize = true;
    this.rbtnPumpArtList.Location = new Point(36, 42);
    this.rbtnPumpArtList.Name = "rbtnPumpArtList";
    this.rbtnPumpArtList.Size = new Size((int) sbyte.MaxValue, 17);
    this.rbtnPumpArtList.TabIndex = 4;
    this.rbtnPumpArtList.Tag = (object) "3";
    this.rbtnPumpArtList.Text = "По составу изделий";
    this.rbtnPumpArtList.UseVisualStyleBackColor = true;
    this.rbtnPumpArtList.CheckedChanged += new EventHandler(this.rbtnPumpAllData_CheckedChanged);
    this.tctlPumpModeSettings.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.tctlPumpModeSettings.Controls.Add((Control) this.tpagePumpAll);
    this.tctlPumpModeSettings.Controls.Add((Control) this.tpagePumpArchive);
    this.tctlPumpModeSettings.Controls.Add((Control) this.tpagePumpTpList);
    this.tctlPumpModeSettings.Controls.Add((Control) this.tpagePumpArtList);
    this.tctlPumpModeSettings.Controls.Add((Control) this.tpagePumpProdZakList);
    this.tctlPumpModeSettings.Location = new Point(10, 72);
    this.tctlPumpModeSettings.Name = "tctlPumpModeSettings";
    this.tctlPumpModeSettings.SelectedIndex = 0;
    this.tctlPumpModeSettings.ShowTabHeader = true;
    this.tctlPumpModeSettings.Size = new Size(558, 208 /*0xD0*/);
    this.tctlPumpModeSettings.TabIndex = 3;
    this.tctlPumpModeSettings.TabIndexChanged += new EventHandler(this.tctlPumpModeSettings_TabIndexChanged);
    this.tpagePumpAll.BackColor = SystemColors.Control;
    this.tpagePumpAll.Location = new Point(4, 22);
    this.tpagePumpAll.Name = "tpagePumpAll";
    this.tpagePumpAll.Padding = new Padding(3);
    this.tpagePumpAll.Size = new Size(550, 182);
    this.tpagePumpAll.TabIndex = 0;
    this.tpagePumpAll.Text = "Pump All";
    this.tpagePumpArchive.Controls.Add((Control) this.tvArchives);
    this.tpagePumpArchive.Location = new Point(4, 22);
    this.tpagePumpArchive.Name = "tpagePumpArchive";
    this.tpagePumpArchive.Padding = new Padding(3);
    this.tpagePumpArchive.Size = new Size(513, 182);
    this.tpagePumpArchive.TabIndex = 1;
    this.tpagePumpArchive.Text = "Список архивов";
    this.tpagePumpArchive.UseVisualStyleBackColor = true;
    this.tvArchives.CheckBoxes = true;
    this.tvArchives.ContextMenuStrip = this.cmsArchives;
    this.tvArchives.Dock = DockStyle.Fill;
    this.tvArchives.FullRowSelect = true;
    this.tvArchives.Location = new Point(3, 3);
    this.tvArchives.Name = "tvArchives";
    treeNode1.Name = "Node3";
    treeNode1.Text = "Node3";
    treeNode2.Name = "Node4";
    treeNode2.Text = "Node4";
    treeNode3.Name = "Node1";
    treeNode3.Text = "Node1";
    treeNode4.Name = "Node2";
    treeNode4.Text = "Node2";
    treeNode5.Name = "Node0";
    treeNode5.Text = "Node0";
    this.tvArchives.Nodes.AddRange(new TreeNode[1]
    {
      treeNode5
    });
    this.tvArchives.Size = new Size(507, 176 /*0xB0*/);
    this.tvArchives.TabIndex = 0;
    this.tvArchives.BeforeCheck += new TreeViewCancelEventHandler(this.tvArchives_BeforeCheck);
    this.tvArchives.AfterCheck += new TreeViewEventHandler(this.tvArchives_AfterCheck);
    this.cmsArchives.Items.AddRange(new ToolStripItem[4]
    {
      (ToolStripItem) this.tsmiArchSelectAll,
      (ToolStripItem) this.tsmiArchClearAll,
      (ToolStripItem) this.tsmiArchSep1,
      (ToolStripItem) this.tsmiArchInvertAll
    });
    this.cmsArchives.Name = "cmsArchives";
    this.cmsArchives.Size = new Size(222, 76);
    this.cmsArchives.Opening += new CancelEventHandler(this.cmsArchives_Opening);
    this.tsmiArchSelectAll.Name = "tsmiArchSelectAll";
    this.tsmiArchSelectAll.Size = new Size(221, 22);
    this.tsmiArchSelectAll.Text = "Выделить все";
    this.tsmiArchSelectAll.Click += new EventHandler(this.tsmiArchSelectAll_Click);
    this.tsmiArchClearAll.Name = "tsmiArchClearAll";
    this.tsmiArchClearAll.Size = new Size(221, 22);
    this.tsmiArchClearAll.Text = "Очистить все";
    this.tsmiArchClearAll.Click += new EventHandler(this.tsmiArchClearAll_Click);
    this.tsmiArchSep1.Name = "tsmiArchSep1";
    this.tsmiArchSep1.Size = new Size(218, 6);
    this.tsmiArchInvertAll.Name = "tsmiArchInvertAll";
    this.tsmiArchInvertAll.Size = new Size(221, 22);
    this.tsmiArchInvertAll.Text = "Инвертировать выделение";
    this.tsmiArchInvertAll.Click += new EventHandler(this.tsmiArchInvert);
    this.tpagePumpTpList.Controls.Add((Control) this.lvTpList);
    this.tpagePumpTpList.Location = new Point(4, 22);
    this.tpagePumpTpList.Name = "tpagePumpTpList";
    this.tpagePumpTpList.Padding = new Padding(3);
    this.tpagePumpTpList.Size = new Size(513, 182);
    this.tpagePumpTpList.TabIndex = 2;
    this.tpagePumpTpList.Text = "Список ТП";
    this.tpagePumpTpList.UseVisualStyleBackColor = true;
    this.lvTpList.Columns.AddRange(new ColumnHeader[2]
    {
      this.chDesignation,
      this.chName
    });
    this.lvTpList.ContextMenuStrip = this.cmsTpList;
    this.lvTpList.Dock = DockStyle.Fill;
    this.lvTpList.FullRowSelect = true;
    this.lvTpList.HeaderStyle = ColumnHeaderStyle.Nonclickable;
    this.lvTpList.HideSelection = false;
    this.lvTpList.Items.AddRange(new ListViewItem[1]
    {
      listViewItem
    });
    this.lvTpList.Location = new Point(3, 3);
    this.lvTpList.Name = "lvTpList";
    this.lvTpList.Size = new Size(507, 176 /*0xB0*/);
    this.lvTpList.TabIndex = 0;
    this.lvTpList.UseCompatibleStateImageBehavior = false;
    this.lvTpList.View = View.Details;
    this.chDesignation.Text = "Обозначение";
    this.chDesignation.Width = 300;
    this.chName.Text = "Наименование";
    this.chName.Width = 200;
    this.cmsTpList.Items.AddRange(new ToolStripItem[7]
    {
      (ToolStripItem) this.tsmiDocAdd,
      (ToolStripItem) this.tsmiDocDelete,
      (ToolStripItem) this.tsmiDocDeleteAll,
      (ToolStripItem) this.tsmiDocSep1,
      (ToolStripItem) this.tsmiDocSelectAll,
      (ToolStripItem) this.tsmiDocClearAll,
      (ToolStripItem) this.tsmiDocInvertAll
    });
    this.cmsTpList.Name = "cmsTpList";
    this.cmsTpList.Size = new Size(248, 142);
    this.cmsTpList.Opening += new CancelEventHandler(this.cmsTpList_Opening);
    this.tsmiDocAdd.Name = "tsmiDocAdd";
    this.tsmiDocAdd.Size = new Size(247, 22);
    this.tsmiDocAdd.Text = "Добавить документы";
    this.tsmiDocAdd.Click += new EventHandler(this.tsmiDocAdd_Click);
    this.tsmiDocDelete.Name = "tsmiDocDelete";
    this.tsmiDocDelete.Size = new Size(247, 22);
    this.tsmiDocDelete.Text = "Удалить выделенные докуметы";
    this.tsmiDocDelete.Click += new EventHandler(this.tsmiDocDelete_Click);
    this.tsmiDocDeleteAll.Name = "tsmiDocDeleteAll";
    this.tsmiDocDeleteAll.Size = new Size(247, 22);
    this.tsmiDocDeleteAll.Text = "Удалить все документы";
    this.tsmiDocDeleteAll.Click += new EventHandler(this.tsmiDocDeleteAll_Click);
    this.tsmiDocSep1.Name = "tsmiDocSep1";
    this.tsmiDocSep1.Size = new Size(244, 6);
    this.tsmiDocSelectAll.Name = "tsmiDocSelectAll";
    this.tsmiDocSelectAll.Size = new Size(247, 22);
    this.tsmiDocSelectAll.Text = "Выбрать все";
    this.tsmiDocSelectAll.Click += new EventHandler(this.tsmiDocSelectAll_Click);
    this.tsmiDocClearAll.Name = "tsmiDocClearAll";
    this.tsmiDocClearAll.Size = new Size(247, 22);
    this.tsmiDocClearAll.Text = "Очистить все";
    this.tsmiDocClearAll.Click += new EventHandler(this.tsmiDocClearAll_Click);
    this.tsmiDocInvertAll.Name = "tsmiDocInvertAll";
    this.tsmiDocInvertAll.Size = new Size(247, 22);
    this.tsmiDocInvertAll.Text = "Инвертировать выделения";
    this.tsmiDocInvertAll.Click += new EventHandler(this.tsmiDocInvertAll_Click);
    this.tpagePumpArtList.Controls.Add((Control) this.lvArtList);
    this.tpagePumpArtList.Location = new Point(4, 22);
    this.tpagePumpArtList.Name = "tpagePumpArtList";
    this.tpagePumpArtList.Padding = new Padding(3);
    this.tpagePumpArtList.Size = new Size(513, 182);
    this.tpagePumpArtList.TabIndex = 3;
    this.tpagePumpArtList.Text = "Список изделий";
    this.tpagePumpArtList.UseVisualStyleBackColor = true;
    this.lvArtList.Columns.AddRange(new ColumnHeader[3]
    {
      this.columnHeader5,
      this.columnHeader1,
      this.columnHeader2
    });
    this.lvArtList.ContextMenuStrip = this.cmsArtList;
    this.lvArtList.Dock = DockStyle.Fill;
    this.lvArtList.FullRowSelect = true;
    this.lvArtList.HeaderStyle = ColumnHeaderStyle.Nonclickable;
    this.lvArtList.HideSelection = false;
    this.lvArtList.Location = new Point(3, 3);
    this.lvArtList.Name = "lvArtList";
    this.lvArtList.Size = new Size(507, 176 /*0xB0*/);
    this.lvArtList.TabIndex = 1;
    this.lvArtList.UseCompatibleStateImageBehavior = false;
    this.lvArtList.View = View.Details;
    this.columnHeader5.Text = "Идентификатор";
    this.columnHeader5.Width = 100;
    this.columnHeader1.Text = "Обозначение";
    this.columnHeader1.Width = 130;
    this.columnHeader2.Text = "Наименование";
    this.columnHeader2.Width = 200;
    this.cmsArtList.Items.AddRange(new ToolStripItem[7]
    {
      (ToolStripItem) this.tsmiArtAdd,
      (ToolStripItem) this.tsmiArtDelete,
      (ToolStripItem) this.tsmiArtDeleteAll,
      (ToolStripItem) this.toolStripSeparator1,
      (ToolStripItem) this.tsmiArtSelectAll,
      (ToolStripItem) this.tsmiArtClearAll,
      (ToolStripItem) this.tsmiArtInvertAll
    });
    this.cmsArtList.Name = "cmsTpList";
    this.cmsArtList.Size = new Size(238, 142);
    this.tsmiArtAdd.Name = "tsmiArtAdd";
    this.tsmiArtAdd.Size = new Size(237, 22);
    this.tsmiArtAdd.Text = "Добавить изделия";
    this.tsmiArtAdd.Click += new EventHandler(this.tsmiArtAdd_Click);
    this.tsmiArtDelete.Name = "tsmiArtDelete";
    this.tsmiArtDelete.Size = new Size(237, 22);
    this.tsmiArtDelete.Text = "Удалить выделенные изделия";
    this.tsmiArtDelete.Click += new EventHandler(this.tsmiArtDelete_Click);
    this.tsmiArtDeleteAll.Name = "tsmiArtDeleteAll";
    this.tsmiArtDeleteAll.Size = new Size(237, 22);
    this.tsmiArtDeleteAll.Text = "Удалить все изделия";
    this.tsmiArtDeleteAll.Click += new EventHandler(this.tsmiArtDeleteAll_Click);
    this.toolStripSeparator1.Name = "toolStripSeparator1";
    this.toolStripSeparator1.Size = new Size(234, 6);
    this.tsmiArtSelectAll.Name = "tsmiArtSelectAll";
    this.tsmiArtSelectAll.Size = new Size(237, 22);
    this.tsmiArtSelectAll.Text = "Выбрать все";
    this.tsmiArtSelectAll.Click += new EventHandler(this.tsmiArtSelectAll_Click);
    this.tsmiArtClearAll.Name = "tsmiArtClearAll";
    this.tsmiArtClearAll.Size = new Size(237, 22);
    this.tsmiArtClearAll.Text = "Очистить все";
    this.tsmiArtClearAll.Click += new EventHandler(this.tsmiArtClearAll_Click);
    this.tsmiArtInvertAll.Name = "tsmiArtInvertAll";
    this.tsmiArtInvertAll.Size = new Size(237, 22);
    this.tsmiArtInvertAll.Text = "Инвертировать выделения";
    this.tsmiArtInvertAll.Click += new EventHandler(this.tsmiArtInvertAll_Click);
    this.tpagePumpProdZakList.Controls.Add((Control) this.lvProdZakList);
    this.tpagePumpProdZakList.Location = new Point(4, 22);
    this.tpagePumpProdZakList.Name = "tpagePumpProdZakList";
    this.tpagePumpProdZakList.Padding = new Padding(3);
    this.tpagePumpProdZakList.Size = new Size(513, 182);
    this.tpagePumpProdZakList.TabIndex = 4;
    this.tpagePumpProdZakList.Text = "Список ПЗ";
    this.tpagePumpProdZakList.UseVisualStyleBackColor = true;
    this.lvProdZakList.Columns.AddRange(new ColumnHeader[3]
    {
      this.columnHeader6,
      this.columnHeader3,
      this.columnHeader4
    });
    this.lvProdZakList.ContextMenuStrip = this.cmsProdZakList;
    this.lvProdZakList.Dock = DockStyle.Fill;
    this.lvProdZakList.FullRowSelect = true;
    this.lvProdZakList.HeaderStyle = ColumnHeaderStyle.Nonclickable;
    this.lvProdZakList.HideSelection = false;
    this.lvProdZakList.Location = new Point(3, 3);
    this.lvProdZakList.Name = "lvProdZakList";
    this.lvProdZakList.Size = new Size(507, 176 /*0xB0*/);
    this.lvProdZakList.TabIndex = 2;
    this.lvProdZakList.UseCompatibleStateImageBehavior = false;
    this.lvProdZakList.View = View.Details;
    this.columnHeader6.Text = "Идентификатор";
    this.columnHeader6.Width = 100;
    this.columnHeader3.Text = "Обозначение";
    this.columnHeader3.Width = 130;
    this.columnHeader4.Text = "Наименование";
    this.columnHeader4.Width = 200;
    this.cmsProdZakList.Items.AddRange(new ToolStripItem[8]
    {
      (ToolStripItem) this.tsmiProdZakAdd,
      (ToolStripItem) this.tsmiAddFromSelectedToImportFromSearch,
      (ToolStripItem) this.tsmiProdZakDelete,
      (ToolStripItem) this.tsmiProdZakDeleteAll,
      (ToolStripItem) this.toolStripSeparator2,
      (ToolStripItem) this.tsmiProdZakSelectAll,
      (ToolStripItem) this.tsmiProdZakClearAll,
      (ToolStripItem) this.tsmiProdZakInvertAll
    });
    this.cmsProdZakList.Name = "cmsTpList";
    this.cmsProdZakList.Size = new Size(331, 164);
    this.tsmiProdZakAdd.Name = "tsmiProdZakAdd";
    this.tsmiProdZakAdd.Size = new Size(330, 22);
    this.tsmiProdZakAdd.Text = "Добавить ПЗ";
    this.tsmiProdZakAdd.Click += new EventHandler(this.tsmiProdZakAdd_Click);
    this.tsmiAddFromSelectedToImportFromSearch.Name = "tsmiAddFromSelectedToImportFromSearch";
    this.tsmiAddFromSelectedToImportFromSearch.Size = new Size(330, 22);
    this.tsmiAddFromSelectedToImportFromSearch.Text = "Добавить из списка импортируемых из Search";
    this.tsmiAddFromSelectedToImportFromSearch.Click += new EventHandler(this.tsmiProdZakAddFromSelectedToImportFromSearch_Click);
    this.tsmiProdZakDelete.Name = "tsmiProdZakDelete";
    this.tsmiProdZakDelete.Size = new Size(330, 22);
    this.tsmiProdZakDelete.Text = "Удалить выделенные ПЗ";
    this.tsmiProdZakDelete.Click += new EventHandler(this.tsmiProdZakDelete_Click);
    this.tsmiProdZakDeleteAll.Name = "tsmiProdZakDeleteAll";
    this.tsmiProdZakDeleteAll.Size = new Size(330, 22);
    this.tsmiProdZakDeleteAll.Text = "Удалить все ПЗ";
    this.tsmiProdZakDeleteAll.Click += new EventHandler(this.tsmiProdZakDeleteAll_Click);
    this.toolStripSeparator2.Name = "toolStripSeparator2";
    this.toolStripSeparator2.Size = new Size(327, 6);
    this.tsmiProdZakSelectAll.Name = "tsmiProdZakSelectAll";
    this.tsmiProdZakSelectAll.Size = new Size(330, 22);
    this.tsmiProdZakSelectAll.Text = "Выбрать все";
    this.tsmiProdZakSelectAll.Click += new EventHandler(this.tsmiProdZakSelectAll_Click);
    this.tsmiProdZakClearAll.Name = "tsmiProdZakClearAll";
    this.tsmiProdZakClearAll.Size = new Size(330, 22);
    this.tsmiProdZakClearAll.Text = "Очистить все";
    this.tsmiProdZakClearAll.Click += new EventHandler(this.tsmiProdZakClearAll_Click);
    this.tsmiProdZakInvertAll.Name = "tsmiProdZakInvertAll";
    this.tsmiProdZakInvertAll.Size = new Size(330, 22);
    this.tsmiProdZakInvertAll.Text = "Инвертировать выделения";
    this.tsmiProdZakInvertAll.Click += new EventHandler(this.tsmiProdZakInvertAll_Click);
    this.rbtnPumpTpList.AutoSize = true;
    this.rbtnPumpTpList.Location = new Point(368, 19);
    this.rbtnPumpTpList.Name = "rbtnPumpTpList";
    this.rbtnPumpTpList.Size = new Size(95, 17);
    this.rbtnPumpTpList.TabIndex = 2;
    this.rbtnPumpTpList.Tag = (object) "2";
    this.rbtnPumpTpList.Text = "По списку ТП";
    this.rbtnPumpTpList.UseVisualStyleBackColor = true;
    this.rbtnPumpTpList.CheckedChanged += new EventHandler(this.rbtnPumpAllData_CheckedChanged);
    this.rbtnPumpByArchive.AutoSize = true;
    this.rbtnPumpByArchive.Location = new Point(213, 19);
    this.rbtnPumpByArchive.Name = "rbtnPumpByArchive";
    this.rbtnPumpByArchive.Size = new Size(85, 17);
    this.rbtnPumpByArchive.TabIndex = 1;
    this.rbtnPumpByArchive.Tag = (object) "1";
    this.rbtnPumpByArchive.Text = "По архивам";
    this.rbtnPumpByArchive.UseVisualStyleBackColor = true;
    this.rbtnPumpByArchive.CheckedChanged += new EventHandler(this.rbtnPumpAllData_CheckedChanged);
    this.rbtnPumpAllData.AutoSize = true;
    this.rbtnPumpAllData.Checked = true;
    this.rbtnPumpAllData.Location = new Point(36, 19);
    this.rbtnPumpAllData.Name = "rbtnPumpAllData";
    this.rbtnPumpAllData.Size = new Size(85, 17);
    this.rbtnPumpAllData.TabIndex = 0;
    this.rbtnPumpAllData.TabStop = true;
    this.rbtnPumpAllData.Tag = (object) "0";
    this.rbtnPumpAllData.Text = "Все данные";
    this.rbtnPumpAllData.UseVisualStyleBackColor = true;
    this.rbtnPumpAllData.CheckedChanged += new EventHandler(this.rbtnPumpAllData_CheckedChanged);
    this.grbTechProc.Controls.Add((Control) this.lblComplectFolder);
    this.grbTechProc.Controls.Add((Control) this.btnComplectFolder);
    this.grbTechProc.Controls.Add((Control) this.tbxComplectFolder);
    this.grbTechProc.Controls.Add((Control) this.chbCompectPump);
    this.grbTechProc.Dock = DockStyle.Top;
    this.grbTechProc.Location = new Point(10, 23);
    this.grbTechProc.Name = "grbTechProc";
    this.grbTechProc.Size = new Size(574, 76);
    this.grbTechProc.TabIndex = 6;
    this.grbTechProc.TabStop = false;
    this.grbTechProc.Text = "Настройки ТП";
    this.lblComplectFolder.AutoSize = true;
    this.lblComplectFolder.Location = new Point(33, 45);
    this.lblComplectFolder.Name = "lblComplectFolder";
    this.lblComplectFolder.Size = new Size(136, 13);
    this.lblComplectFolder.TabIndex = 7;
    this.lblComplectFolder.Text = "Директория  комплектов";
    this.btnComplectFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.btnComplectFolder.Location = new Point(546, 39);
    this.btnComplectFolder.Name = "btnComplectFolder";
    this.btnComplectFolder.Size = new Size(21, 23);
    this.btnComplectFolder.TabIndex = 7;
    this.btnComplectFolder.Text = "...";
    this.btnComplectFolder.UseVisualStyleBackColor = true;
    this.btnComplectFolder.Click += new EventHandler(this.btnComplectFolder_Click);
    this.tbxComplectFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.tbxComplectFolder.Location = new Point(175, 42);
    this.tbxComplectFolder.Name = "tbxComplectFolder";
    this.tbxComplectFolder.Size = new Size(373, 20);
    this.tbxComplectFolder.TabIndex = 1;
    this.tbxComplectFolder.TextChanged += new EventHandler(this.tbxComplectFolder_TextChanged);
    this.chbCompectPump.AutoSize = true;
    this.chbCompectPump.Location = new Point(10, 19);
    this.chbCompectPump.Name = "chbCompectPump";
    this.chbCompectPump.Size = new Size(221, 17);
    this.chbCompectPump.TabIndex = 0;
    this.chbCompectPump.Text = "Перекачивать комплекты документов";
    this.chbCompectPump.UseVisualStyleBackColor = true;
    this.chbCompectPump.CheckedChanged += new EventHandler(this.chbCompectPump_CheckedChanged);
    this.errorProvider.ContainerControl = (ContainerControl) this;
    this.errorProvider.RightToLeft = true;
    this.chbPumpLinksOnlyWithActual.AutoSize = true;
    this.chbPumpLinksOnlyWithActual.Location = new Point(213, 42);
    this.chbPumpLinksOnlyWithActual.Name = "chbPumpLinksOnlyWithActual";
    this.chbPumpLinksOnlyWithActual.Size = new Size(353, 17);
    this.chbPumpLinksOnlyWithActual.TabIndex = 5;
    this.chbPumpLinksOnlyWithActual.Text = "Перекачивать связи только с актуальными версиями объектов";
    this.chbPumpLinksOnlyWithActual.UseVisualStyleBackColor = true;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.grbMain);
    this.Name = nameof (TechSettingsControl);
    this.Size = new Size(594, 586);
    this.grbMain.ResumeLayout(false);
    this.grbTechData.ResumeLayout(false);
    this.grbTechData.PerformLayout();
    this.grbTechMetaData.ResumeLayout(false);
    this.grbTechMetaData.PerformLayout();
    this.grbPumpMode.ResumeLayout(false);
    this.grbPumpMode.PerformLayout();
    this.tctlPumpModeSettings.ResumeLayout(false);
    this.tpagePumpArchive.ResumeLayout(false);
    this.cmsArchives.ResumeLayout(false);
    this.tpagePumpTpList.ResumeLayout(false);
    this.cmsTpList.ResumeLayout(false);
    this.tpagePumpArtList.ResumeLayout(false);
    this.cmsArtList.ResumeLayout(false);
    this.tpagePumpProdZakList.ResumeLayout(false);
    this.cmsProdZakList.ResumeLayout(false);
    this.grbTechProc.ResumeLayout(false);
    this.grbTechProc.PerformLayout();
    ((ISupportInitialize) this.errorProvider).EndInit();
    this.ResumeLayout(false);
  }
}
