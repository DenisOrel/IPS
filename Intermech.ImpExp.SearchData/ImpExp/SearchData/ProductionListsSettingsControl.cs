// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.ProductionListsSettingsControl
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.Controls;
using Intermech.ImpExp.SearchData.Controls;
using Intermech.ImpExp.SearchData.Properties;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.ImpExp.SearchData;

internal class ProductionListsSettingsControl : StepControl
{
  private const string _enabledPLList = "PLLIST";
  private const string _settingsPLList = "SETTINGS_PLLIST";
  public static string BlockPLListInSearchConfigName = "BLOCK";
  private RelationTypesComparison _relationTypesComparison;
  private Dictionary<int, bool> _settings;
  private bool _selfCheck;
  public List<Tuple<int, string, string, string>> PList = new List<Tuple<int, string, string, string>>();
  private IContainer components;
  private Label label1;
  private CheckBox cbSelectAll;
  private GroupBox groupBox1;
  private PropertyGrid pgRelationTypes;
  private TextBox tbF5;
  private GroupBox gbF5;
  private Button bF5;
  private CheckBox cbBlockSearchData;
  private iGrid iGrid1;
  private iGCellStyle iGrid1Col1CellStyle;
  private iGColHdrStyle iGrid1Col1ColHdrStyle;
  private iGCellStyle iGrid1Col2CellStyle;
  private iGColHdrStyle iGrid1Col2ColHdrStyle;
  private iGCellStyle iGrid1Col3CellStyle;
  private iGColHdrStyle iGrid1Col3ColHdrStyle;
  private ImageList imagesState;
  private iGCellStyle iGrid1Col4CellStyle;
  private iGColHdrStyle iGrid1Col4ColHdrStyle;
  private ComboBox cbSearch;
  private Button bSearch;

  public ProductionListsSettingsControl(bool specialModePL)
  {
    this.InitializeComponent();
    this.stepRepumpble = true;
    this.stepPrevAllowed = true;
    ISaveSettings service = ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings;
    Dictionary<string, SaveSettingsAttribute[]> settings1 = service.GetSettings("PLLIST");
    if (settings1 != null)
    {
      this._settings = new Dictionary<int, bool>();
      foreach (KeyValuePair<string, SaveSettingsAttribute[]> keyValuePair in settings1)
        this._settings.Add(Convert.ToInt32(keyValuePair.Key), Convert.ToBoolean(keyValuePair.Value[0].AttributeValue, (IFormatProvider) CultureInfo.InvariantCulture));
    }
    Dictionary<string, SaveSettingsAttribute[]> settings2 = service.GetSettings("SETTINGS_PLLIST");
    SaveSettingsAttribute[] settingsAttributeArray;
    if (settings2 != null && settings2.TryGetValue(ProductionListsSettingsControl.BlockPLListInSearchConfigName, out settingsAttributeArray))
      this.cbBlockSearchData.Checked = Convert.ToBoolean(settingsAttributeArray[0].AttributeValue, (IFormatProvider) CultureInfo.InvariantCulture);
    this.gbF5.Visible = specialModePL;
    this._relationTypesComparison = RelationTypesComparison.Instance;
    this.pgRelationTypes.SelectedObject = (object) this._relationTypesComparison;
    if (specialModePL)
      this.tbF5.Text = this._relationTypesComparison.F5Type.ToString();
    this.iGrid1.Cols[0].CellStyle.ImageList = this.imagesState;
  }

  private Guid GetRelationTypeGuidFromSettings(
    Dictionary<string, SaveSettingsAttribute[]> settings,
    string name)
  {
    SaveSettingsAttribute[] settingsAttributeArray;
    return !settings.TryGetValue(name, out settingsAttributeArray) || !GuidHelper.IsGuid(settingsAttributeArray[0].AttributeValue) ? Guid.Empty : new Guid(settingsAttributeArray[0].AttributeValue);
  }

  private void AddPLToList(int artID, string designatio, string name, string author)
  {
    iGRow iGrow = this.iGrid1.Rows.Add();
    iGrow.Key = artID.ToString();
    bool flag;
    iGrow.Cells[0].ImageIndex = this._settings == null || !this._settings.TryGetValue(artID, out flag) ? 1 : (flag ? 1 : 0);
    iGrow.Cells[1].Value = (object) artID.ToString();
    iGrow.Cells[2].Value = (object) designatio;
    iGrow.Cells[3].Value = (object) name;
    iGrow.Cells[4].Value = (object) author;
    iGrow.Tag = (object) artID;
  }

  private void SelectAll_CheckedChanged(object sender, EventArgs e)
  {
    if (this._selfCheck)
      return;
    try
    {
      this.iGrid1.BeginUpdate();
      this.iGrid1.Redraw = false;
      foreach (iGRow row in (IEnumerable) this.iGrid1.Rows)
        row.Cells[0].ImageIndex = this.cbSelectAll.Checked ? 1 : 0;
    }
    finally
    {
      this.iGrid1.Redraw = true;
      this.iGrid1.EndUpdate();
    }
  }

  public override void RefreshControl() => this.InitControl();

  private void InitControl()
  {
    PluginSettings.PumpArtVersions = true;
    PluginSettings.PumpSysArtVersions = true;
    this.iGrid1.Rows.Clear();
    int relationTypeId = MetaDataHelper.GetRelationTypeID("cadd9a57-306c-11d8-b4e9-00304f19f545");
    int objectTypeId = MetaDataHelper.GetObjectTypeID("cadd9a5c-306c-11d8-b4e9-00304f19f545");
    MetaDataHelper.GetAttributeID((object) new Guid("cad0132b-306c-11d8-b4e9-00304f19f545"));
    BasePumpHelper.Session.GetRelationCollection(relationTypeId);
    BasePumpHelper.Session.GetObjectCollection(objectTypeId);
    try
    {
      this.iGrid1.BeginUpdate();
      this.iGrid1.Redraw = false;
      foreach (Tuple<int, string, string, string> tuple in this.PList)
        this.AddPLToList(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
    }
    finally
    {
      this.iGrid1.Redraw = true;
      this.iGrid1.EndUpdate();
    }
    this.UpdateCheckBox();
  }

  private void UpdateCheckBox()
  {
    this._selfCheck = true;
    try
    {
      int num = 0;
      foreach (iGRow row in (IEnumerable) this.iGrid1.Rows)
      {
        if (row.Cells[0].ImageIndex == 1)
          ++num;
      }
      this.cbSelectAll.Checked = this.iGrid1.Rows.Count == num;
    }
    finally
    {
      this._selfCheck = false;
    }
  }

  public override bool LeaveControl()
  {
    ICache service1 = ServicesManager.GetService(typeof (ICache)) as ICache;
    ISaveSettings service2 = ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings;
    Dictionary<string, SaveSettingsAttribute[]> settings1 = new Dictionary<string, SaveSettingsAttribute[]>();
    Dictionary<string, SaveSettingsAttribute[]> settings2 = new Dictionary<string, SaveSettingsAttribute[]>();
    this._settings = new Dictionary<int, bool>();
    try
    {
      service2.ClearSettings("PLLIST");
      service1.DeleteCache(ImportingCategory.EnabledProductionLists);
      IImportingData cache = service1.GetCache(ImportingCategory.EnabledProductionLists);
      foreach (iGRow row in (IEnumerable) this.iGrid1.Rows)
      {
        bool flag = row.Cells[0].ImageIndex == 1;
        settings1[Convert.ToString(row.Tag)] = new SaveSettingsAttribute[1]
        {
          new SaveSettingsAttribute("ENABLED", flag.ToString((IFormatProvider) CultureInfo.InvariantCulture))
        };
        if (flag)
          cache.AddValue((object) (int) row.Tag, 0L);
        this._settings.Add((int) row.Tag, flag);
      }
      this._relationTypesComparison.Save();
      service2.ClearSettings("SETTINGS_PLLIST");
      settings2.Add(ProductionListsSettingsControl.BlockPLListInSearchConfigName, new SaveSettingsAttribute[1]
      {
        new SaveSettingsAttribute("ENABLED", this.cbBlockSearchData.Checked.ToString((IFormatProvider) CultureInfo.InvariantCulture))
      });
      service1.DeleteCache(ImportingCategory.SettingsProductionLists);
      service1.GetCache(ImportingCategory.SettingsProductionLists).AddValue((object) ProductionListsSettingsControl.BlockPLListInSearchConfigName, Convert.ToInt64(this.cbBlockSearchData.Checked));
    }
    finally
    {
      service1.ReleaseCache(ImportingCategory.EnabledProductionLists);
      service1.ReleaseCache(ImportingCategory.SettingsProductionLists);
      service2.SetSettings("PLLIST", settings1);
      service2.SetSettings("SETTINGS_PLLIST", settings2);
    }
    return true;
  }

  protected override string getCaption() => "Производственные заказы";

  private void F5_Click(object sender, EventArgs e)
  {
    SelectionWindow selectionWindow = new SelectionWindow(6);
    if (selectionWindow.ShowDialog() != DialogResult.OK || !(selectionWindow.SelectedGuid != Guid.Empty))
      return;
    this._relationTypesComparison.F5Type = new RelationTypeAttProxy(selectionWindow.SelectedGuid, selectionWindow.SelectedText);
    this.tbF5.Text = selectionWindow.SelectedText;
  }

  private void Grid1_CellMouseUp(object sender, iGCellMouseUpEventArgs e)
  {
    if (e.RowIndex >= this.iGrid1.Rows.Count || e.ColIndex != 0 || e.Button != MouseButtons.Left)
      return;
    iGRow row = this.iGrid1.Rows[e.RowIndex];
    if (row == null || row.Tag == null)
      return;
    iGCell cell = e.ColIndex < 0 || row == null ? (iGCell) null : row.Cells[e.ColIndex];
    int left = e.Bounds.Left;
    Rectangle bounds = e.Bounds;
    int num1 = (bounds.Width - this.imagesState.ImageSize.Width) / 2;
    int num2 = left + num1;
    bounds = e.Bounds;
    int top = bounds.Top;
    bounds = e.Bounds;
    int num3 = (bounds.Height - this.imagesState.ImageSize.Height) / 2;
    int num4 = top + num3;
    Rectangle rectangle;
    ref Rectangle local = ref rectangle;
    int x = num2;
    int y = num4;
    Size imageSize = this.imagesState.ImageSize;
    int width = imageSize.Width;
    imageSize = this.imagesState.ImageSize;
    int height = imageSize.Height;
    local = new Rectangle(x, y, width, height);
    if (!rectangle.Contains(e.MousePos))
      return;
    cell.ImageIndex = cell.ImageIndex == 0 ? 1 : 0;
    this.iGrid1.Invalidate(e.Bounds);
    this.UpdateCheckBox();
  }

  private void Search_Click(object sender, EventArgs e)
  {
    if (this.iGrid1.Rows.Count == 0 || string.IsNullOrEmpty(this.cbSearch.Text))
      return;
    string searchText = this.cbSearch.Text;
    int colIndex = 2;
    if (searchText.StartsWith("n"))
    {
      searchText = searchText.Remove(0, 1);
      colIndex = 1;
    }
    iGSelectedCellsCollection selectedCells = this.iGrid1.SelectedCells;
    this.SearchMethod(selectedCells == null || selectedCells.Count <= 0 ? 0 : selectedCells[0].RowIndex, colIndex, searchText);
  }

  private void AddTextToHistory()
  {
    bool flag = false;
    for (int index = 0; index < this.cbSearch.Items.Count; ++index)
    {
      if (Convert.ToString(this.cbSearch.Items[index]).Equals(this.cbSearch.Text))
      {
        flag = true;
        break;
      }
    }
    if (flag)
      return;
    this.cbSearch.Items.Insert(0, (object) this.cbSearch.Text);
  }

  private void SearchMethod(int startIndex, int colIndex, string searchText)
  {
    bool flag = false;
    for (int index = startIndex + 1; index < this.iGrid1.Rows.Count; ++index)
    {
      if (this.iGrid1.Rows[index].Cells[colIndex].Text.Contains(searchText))
      {
        this.iGrid1.SetCurRow(index);
        flag = true;
        break;
      }
    }
    if (!flag)
    {
      if (MessageBox.Show($"Строка '{searchText}' не найдена! Начать поиск с начала?", "Поиск", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.SearchMethod(-1, colIndex, searchText);
    }
    else
      this.AddTextToHistory();
  }

  private void Search_KeyUp(object sender, KeyEventArgs e)
  {
    if (e.KeyCode != System.Windows.Forms.Keys.Return)
      return;
    this.Search_Click((object) this, new EventArgs());
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
    iGColPattern iGcolPattern1 = new iGColPattern();
    iGColPattern iGcolPattern2 = new iGColPattern();
    iGColPattern iGcolPattern3 = new iGColPattern();
    iGColPattern iGcolPattern4 = new iGColPattern();
    iGColPattern iGcolPattern5 = new iGColPattern();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ProductionListsSettingsControl));
    this.iGrid1Col4CellStyle = new iGCellStyle(true);
    this.iGrid1Col4ColHdrStyle = new iGColHdrStyle(true);
    this.iGrid1Col1CellStyle = new iGCellStyle(true);
    this.iGrid1Col1ColHdrStyle = new iGColHdrStyle(true);
    this.iGrid1Col2CellStyle = new iGCellStyle(true);
    this.iGrid1Col2ColHdrStyle = new iGColHdrStyle(true);
    this.iGrid1Col3CellStyle = new iGCellStyle(true);
    this.iGrid1Col3ColHdrStyle = new iGColHdrStyle(true);
    this.label1 = new Label();
    this.cbSelectAll = new CheckBox();
    this.groupBox1 = new GroupBox();
    this.pgRelationTypes = new PropertyGrid();
    this.tbF5 = new TextBox();
    this.gbF5 = new GroupBox();
    this.bF5 = new Button();
    this.cbBlockSearchData = new CheckBox();
    this.iGrid1 = new iGrid();
    this.imagesState = new ImageList(this.components);
    this.cbSearch = new ComboBox();
    this.bSearch = new Button();
    this.groupBox1.SuspendLayout();
    this.gbF5.SuspendLayout();
    ((ISupportInitialize) this.iGrid1).BeginInit();
    this.SuspendLayout();
    this.label1.AutoSize = true;
    this.label1.Location = new Point(10, 20);
    this.label1.Name = "label1";
    this.label1.Size = new Size(274, 13);
    this.label1.TabIndex = 1;
    this.label1.Text = "Выберите производственные заказы для миграции:";
    this.cbSelectAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
    this.cbSelectAll.AutoSize = true;
    this.cbSelectAll.Checked = true;
    this.cbSelectAll.CheckState = CheckState.Checked;
    this.cbSelectAll.Location = new Point(13, 464);
    this.cbSelectAll.Name = "cbSelectAll";
    this.cbSelectAll.Size = new Size(91, 17);
    this.cbSelectAll.TabIndex = 2;
    this.cbSelectAll.Text = "Выбрать все";
    this.cbSelectAll.UseVisualStyleBackColor = true;
    this.cbSelectAll.CheckedChanged += new EventHandler(this.SelectAll_CheckedChanged);
    this.groupBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.groupBox1.Controls.Add((Control) this.pgRelationTypes);
    this.groupBox1.Location = new Point(13, 500);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(1045, 140);
    this.groupBox1.TabIndex = 4;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "Сопоставление типов связей в Search и в IPS";
    this.pgRelationTypes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.pgRelationTypes.HelpVisible = false;
    this.pgRelationTypes.Location = new Point(6, 19);
    this.pgRelationTypes.Name = "pgRelationTypes";
    this.pgRelationTypes.PropertySort = PropertySort.Alphabetical;
    this.pgRelationTypes.Size = new Size(1033, 115);
    this.pgRelationTypes.TabIndex = 0;
    this.pgRelationTypes.ToolbarVisible = false;
    this.tbF5.BackColor = SystemColors.Window;
    this.tbF5.Location = new Point(6, 19);
    this.tbF5.Name = "tbF5";
    this.tbF5.ReadOnly = true;
    this.tbF5.Size = new Size(490, 20);
    this.tbF5.TabIndex = 6;
    this.gbF5.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.gbF5.Controls.Add((Control) this.bF5);
    this.gbF5.Controls.Add((Control) this.tbF5);
    this.gbF5.Location = new Point(13, 646);
    this.gbF5.Name = "gbF5";
    this.gbF5.Size = new Size(1045, 50);
    this.gbF5.TabIndex = 7;
    this.gbF5.TabStop = false;
    this.gbF5.Text = "Тип связи для состава документации (по F5)";
    this.bF5.Location = new Point(497, 18);
    this.bF5.Name = "bF5";
    this.bF5.Size = new Size(24, 22);
    this.bF5.TabIndex = 7;
    this.bF5.Text = "...";
    this.bF5.UseVisualStyleBackColor = true;
    this.bF5.Click += new EventHandler(this.F5_Click);
    this.cbBlockSearchData.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
    this.cbBlockSearchData.AutoSize = true;
    this.cbBlockSearchData.Location = new Point(13, 702);
    this.cbBlockSearchData.Name = "cbBlockSearchData";
    this.cbBlockSearchData.Size = new Size(232, 17);
    this.cbBlockSearchData.TabIndex = 8;
    this.cbBlockSearchData.Text = "Блокировать перекачанные ПЗ в Search";
    this.cbBlockSearchData.UseVisualStyleBackColor = true;
    this.iGrid1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.iGrid1.AutoResizeCols = true;
    this.iGrid1.BorderStyle = iGBorderStyle.Flat;
    iGcolPattern1.AllowGrouping = false;
    iGcolPattern1.AllowMoving = false;
    iGcolPattern1.AllowSizing = false;
    iGcolPattern1.CellStyle = this.iGrid1Col4CellStyle;
    iGcolPattern1.ColHdrStyle = this.iGrid1Col4ColHdrStyle;
    iGcolPattern1.Key = "check";
    iGcolPattern1.SortOrder = iGSortOrder.None;
    iGcolPattern1.SortType = iGSortType.None;
    iGcolPattern1.Width = 32 /*0x20*/;
    iGcolPattern2.AllowGrouping = false;
    iGcolPattern2.AllowMoving = false;
    iGcolPattern2.Key = "id";
    iGcolPattern2.Text = (object) "Идентификатор";
    iGcolPattern2.Width = 229;
    iGcolPattern3.AllowGrouping = false;
    iGcolPattern3.AllowMoving = false;
    iGcolPattern3.CellStyle = this.iGrid1Col1CellStyle;
    iGcolPattern3.ColHdrStyle = this.iGrid1Col1ColHdrStyle;
    iGcolPattern3.Key = "designation";
    iGcolPattern3.Text = (object) "Обозначение";
    iGcolPattern3.Width = 285;
    iGcolPattern4.AllowGrouping = false;
    iGcolPattern4.AllowMoving = false;
    iGcolPattern4.CellStyle = this.iGrid1Col2CellStyle;
    iGcolPattern4.ColHdrStyle = this.iGrid1Col2ColHdrStyle;
    iGcolPattern4.Key = "name";
    iGcolPattern4.Text = (object) "Наименование";
    iGcolPattern4.Width = 261;
    iGcolPattern5.AllowGrouping = false;
    iGcolPattern5.AllowMoving = false;
    iGcolPattern5.CellStyle = this.iGrid1Col3CellStyle;
    iGcolPattern5.ColHdrStyle = this.iGrid1Col3ColHdrStyle;
    iGcolPattern5.Key = "createBy";
    iGcolPattern5.Text = (object) "Создал";
    iGcolPattern5.Width = 235;
    this.iGrid1.Cols.AddRange(new iGColPattern[5]
    {
      iGcolPattern1,
      iGcolPattern2,
      iGcolPattern3,
      iGcolPattern4,
      iGcolPattern5
    });
    this.iGrid1.Header.Height = 19;
    this.iGrid1.Location = new Point(13, 36);
    this.iGrid1.Name = "iGrid1";
    this.iGrid1.ReadOnly = true;
    this.iGrid1.RowMode = true;
    this.iGrid1.RowModeHasCurCell = true;
    this.iGrid1.Size = new Size(1044, 420);
    this.iGrid1.TabIndex = 9;
    this.iGrid1.CellMouseUp += new iGCellMouseUpEventHandler(this.Grid1_CellMouseUp);
    this.imagesState.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imagesState.ImageStream");
    this.imagesState.TransparentColor = Color.Transparent;
    this.imagesState.Images.SetKeyName(0, "unchecked.ico");
    this.imagesState.Images.SetKeyName(1, "checked.ico");
    this.cbSearch.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.cbSearch.FormattingEnabled = true;
    this.cbSearch.Location = new Point(736, 471);
    this.cbSearch.Name = "cbSearch";
    this.cbSearch.Size = new Size(295, 21);
    this.cbSearch.TabIndex = 10;
    this.cbSearch.KeyUp += new KeyEventHandler(this.Search_KeyUp);
    this.bSearch.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bSearch.Image = (Image) Resources.Search;
    this.bSearch.Location = new Point(1032, 470);
    this.bSearch.Name = "bSearch";
    this.bSearch.Size = new Size(24, 23);
    this.bSearch.TabIndex = 12;
    this.bSearch.UseVisualStyleBackColor = true;
    this.bSearch.Click += new EventHandler(this.Search_Click);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.bSearch);
    this.Controls.Add((Control) this.cbSearch);
    this.Controls.Add((Control) this.iGrid1);
    this.Controls.Add((Control) this.cbBlockSearchData);
    this.Controls.Add((Control) this.gbF5);
    this.Controls.Add((Control) this.groupBox1);
    this.Controls.Add((Control) this.cbSelectAll);
    this.Controls.Add((Control) this.label1);
    this.MinimumSize = new Size(565, 400);
    this.Name = nameof (ProductionListsSettingsControl);
    this.Size = new Size(1070, 725);
    this.groupBox1.ResumeLayout(false);
    this.gbF5.ResumeLayout(false);
    this.gbF5.PerformLayout();
    ((ISupportInitialize) this.iGrid1).EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  private class ListViewItemComparer : IComparer
  {
    private readonly int col;
    private readonly SortOrder order;

    public ListViewItemComparer()
    {
      this.col = 0;
      this.order = SortOrder.Ascending;
    }

    public ListViewItemComparer(int column, SortOrder order)
    {
      this.col = column;
      this.order = order;
    }

    public int Compare(object x, object y)
    {
      int num = string.Compare(((ListViewItem) x).SubItems[this.col].Text, ((ListViewItem) y).SubItems[this.col].Text);
      if (this.order == SortOrder.Descending)
        num *= -1;
      return num;
    }
  }
}
