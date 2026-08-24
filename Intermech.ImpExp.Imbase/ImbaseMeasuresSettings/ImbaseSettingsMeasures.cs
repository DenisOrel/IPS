// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ImbaseMeasuresSettings.ImbaseSettingsMeasures
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData;
using Intermech.ImpExp.Interface.Controls;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Imbase.ImbaseMeasuresSettings;

internal sealed class ImbaseSettingsMeasures : StepControl
{
  private ImbasePlugin imPlugin;
  private Dictionary<string, ImbaseMeasureItem> imbaseMeasures = new Dictionary<string, ImbaseMeasureItem>();
  private Dictionary<string, string> savedMeasures = new Dictionary<string, string>();
  private ImbaseMeasureSelectDialog selectMeasureDlg;
  private int deltaColumn;
  private Image _image;
  private const string _settingsDeltaColumnWidth = "deltaColumnWidth";
  private const string _settingsSpliterDistance = "spliterDistance";
  private IContainer components;
  private TextBox textBox;
  private Button buttonSelect;
  private SplitContainer splitContainer1;
  private TableLayoutPanel tableLayoutPanel1;
  private Label label1;
  private PropertyGrid propertyGrid;
  private ImageList imageList1;
  private ListView lvMeasures;
  private ColumnHeader columnHeader1;

  public ImbaseSettingsMeasures(ImbasePlugin plugin)
  {
    this.InitializeComponent();
    this.imPlugin = plugin;
    this.stepPrevAllowed = false;
  }

  protected override string getCaption() => "Настройка единиц измерения из Imbase";

  protected override Image getImage()
  {
    if (this._image == null && ServicesManager.GetService(typeof (IBigImageList)) is IBigImageList service)
      this._image = service.ImageList.Images[service.ImageIndex("imgMeasure")];
    return this._image;
  }

  public override void RefreshControl()
  {
    this.loadFromCfgXml();
    if (ServicesManager.GetService(typeof (IMeasures)) is IMeasures service)
    {
      this.lvMeasures.Items.Clear();
      foreach (ImbaseMeasureItem imbaseMeasureItem in this.imbaseMeasures.Values)
      {
        IMeasureItem measureItem = (IMeasureItem) null;
        if (this.savedMeasures.ContainsKey(imbaseMeasureItem.Name))
        {
          string savedMeasure = this.savedMeasures[imbaseMeasureItem.Name];
          if (service.MeasureExists(savedMeasure))
            measureItem = service.GetMeasure(savedMeasure);
        }
        if (measureItem == null)
        {
          measureItem = service.GetMeasure(imbaseMeasureItem.Name);
          if (measureItem == null)
          {
            string measureShortName = imbaseMeasureItem.Name.Replace("**", "^").Replace("k", "к").Replace("K", "К").Replace("H", "Н").Replace("c", "с").Replace("C", "С").Replace("m", "м").Replace("M", "М");
            measureItem = service.GetMeasure(measureShortName);
          }
        }
        if (measureItem != null)
          imbaseMeasureItem.MeasureID = measureItem.Id;
        this.lvMeasures.Items.Add(new ListViewItem(imbaseMeasureItem.ToString())
        {
          Tag = (object) imbaseMeasureItem,
          StateImageIndex = imbaseMeasureItem.MeasureID == 0L ? 0 : 1
        });
      }
    }
    FormStorageEx.LoadSettings((Control) this);
    string attribute1 = FormStorageEx.GetAttribute((Control) this, "deltaColumnWidth");
    if (attribute1 != string.Empty)
      this.deltaColumn = Convert.ToInt32(attribute1);
    string attribute2 = FormStorageEx.GetAttribute((Control) this, "spliterDistance");
    if (attribute2 != string.Empty)
    {
      double num = Convert.ToDouble(attribute2, (IFormatProvider) CultureInfo.InvariantCulture);
      if (num > 0.0)
        this.splitContainer1.SplitterDistance = Convert.ToInt32((double) this.splitContainer1.Width / num);
    }
    this.ResizeColumn();
    base.RefreshControl();
  }

  public override SaveSettingsResult SaveSettings()
  {
    if (!this.IsEnabled)
    {
      int num = (int) MessageBox.Show("Выполните привязку всех единиц измерения Imbase к существующим единицам измерения", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return SaveSettingsResult.ssrRetry;
    }
    ICache service = ServicesManager.GetService(typeof (ICache)) as ICache;
    service.DeleteCache(ImportingCategory.ImbaseBindedMeasures);
    IImportingData cache = service.GetCache(ImportingCategory.ImbaseBindedMeasures);
    try
    {
      foreach (ListViewItem listViewItem in this.lvMeasures.Items)
      {
        ImbaseMeasureItem tag = listViewItem.Tag as ImbaseMeasureItem;
        cache.AddValue(ImportingCategory.ImbaseBindedMeasures, (object) tag.Name, tag.MeasureID);
      }
    }
    finally
    {
      service.ReleaseCache(ImportingCategory.ImbaseBindedMeasures);
    }
    this.imPlugin.imbaseFields.UpdateMeasureFields();
    return SaveSettingsResult.ssrOk;
  }

  private bool SaveControlSettings()
  {
    try
    {
      this.saveToCfgXml();
      FormStorageEx.SaveSettings((Control) this);
      FormStorageEx.AddAttribute((Control) this, "deltaColumnWidth", Convert.ToString(this.deltaColumn));
      FormStorageEx.AddAttribute((Control) this, "spliterDistance", Convert.ToString(this.splitContainer1.Width / this.splitContainer1.SplitterDistance, (IFormatProvider) CultureInfo.InvariantCulture));
      return true;
    }
    catch (Exception ex)
    {
      int num = (int) MessageBox.Show($"Ошибка при сохранении параметров шага: {ex.Message}");
      return false;
    }
  }

  public override bool LeaveControl() => this.SaveControlSettings() && base.LeaveControl();

  public override void Cancel()
  {
    this.SaveControlSettings();
    base.Cancel();
  }

  private void loadFromCfgXml()
  {
    this.savedMeasures.Clear();
    Dictionary<string, SaveSettingsAttribute[]> settings = (ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings).GetSettings("MEASURES");
    if (settings == null)
      return;
    foreach (KeyValuePair<string, SaveSettingsAttribute[]> keyValuePair in settings)
    {
      string key = keyValuePair.Key;
      string str = string.Empty;
      foreach (SaveSettingsAttribute settingsAttribute in keyValuePair.Value)
      {
        if (settingsAttribute.AttributeName.Equals("NEW_NAME"))
        {
          str = settingsAttribute.AttributeValue;
          break;
        }
      }
      if (str != string.Empty)
        this.savedMeasures.Add(key, str);
    }
  }

  private void saveToCfgXml()
  {
    ISaveSettings service1 = ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings;
    Dictionary<string, SaveSettingsAttribute[]> settings = new Dictionary<string, SaveSettingsAttribute[]>();
    IMeasures service2 = ServicesManager.GetService(typeof (IMeasures)) as IMeasures;
    foreach (ListViewItem listViewItem in this.lvMeasures.Items)
    {
      ImbaseMeasureItem tag = listViewItem.Tag as ImbaseMeasureItem;
      List<SaveSettingsAttribute> settingsAttributeList = new List<SaveSettingsAttribute>();
      IMeasureItem measure = service2.GetMeasure(tag.MeasureID);
      if (measure != null)
      {
        settingsAttributeList.Add(new SaveSettingsAttribute("NEW_NAME", measure.ShortName));
        settings.Add(tag.Name, settingsAttributeList.ToArray());
      }
    }
    if (settings.Count > 0)
      service1.SetSettings("MEASURES", settings);
    else
      service1.ClearSettings("MEASURES");
  }

  internal ImbaseMeasureItem GetMeasure(string measureName)
  {
    ImbaseMeasureItem imbaseMeasureItem = (ImbaseMeasureItem) null;
    return this.imbaseMeasures.TryGetValue(measureName, out imbaseMeasureItem) ? imbaseMeasureItem : (ImbaseMeasureItem) null;
  }

  internal ImbaseMeasureItem AddMeasure(string MeasureName)
  {
    ImbaseMeasureItem imbaseMeasureItem = this.GetMeasure(MeasureName);
    if (imbaseMeasureItem == null)
    {
      imbaseMeasureItem = new ImbaseMeasureItem(MeasureName);
      this.imbaseMeasures.Add(imbaseMeasureItem.Name, imbaseMeasureItem);
    }
    return imbaseMeasureItem;
  }

  private void updateMeasureView()
  {
    long currentMeasureId = this.getCurrentMeasureId();
    IMeasures service1 = ServicesManager.GetService(typeof (IMeasures)) as IMeasures;
    IPhysicalValues service2 = ServicesManager.GetService(typeof (IPhysicalValues)) as IPhysicalValues;
    if (currentMeasureId != 0L && service1 != null && service2 != null)
    {
      IMeasureItem measure = service1.GetMeasure(currentMeasureId);
      IPhysicalValueItem physicalValue = service2.GetPhysicalValue(measure.PhysicalValueID);
      ImbaseSettingsMeasures.linkedMeasure linkedMeasure = new ImbaseSettingsMeasures.linkedMeasure(measure, physicalValue);
      this.propertyGrid.SelectedObject = (object) linkedMeasure;
      this.textBox.Text = linkedMeasure.MeasureLongName;
    }
    else
    {
      this.propertyGrid.SelectedObject = (object) null;
      this.textBox.Text = string.Empty;
    }
  }

  private long getCurrentMeasureId()
  {
    return this.lvMeasures.FocusedItem != null && this.lvMeasures.FocusedItem.Tag is ImbaseMeasureItem tag ? tag.MeasureID : 0L;
  }

  private bool IsEnabled
  {
    get
    {
      foreach (ListViewItem listViewItem in this.lvMeasures.Items)
      {
        if ((listViewItem.Tag as ImbaseMeasureItem).MeasureID == 0L)
          return false;
      }
      return true;
    }
  }

  private void saveMeasureID(ListViewItem item, long newMeasureId)
  {
    if (item == null)
      return;
    (item.Tag as ImbaseMeasureItem).MeasureID = newMeasureId;
    item.StateImageIndex = newMeasureId == 0L ? 0 : 1;
  }

  private void buttonSelect_Click(object sender, EventArgs e)
  {
    if (this.selectMeasureDlg == null)
      this.selectMeasureDlg = new ImbaseMeasureSelectDialog(this.imPlugin.Idw);
    this.selectMeasureDlg.SelectedMeasureID = this.getCurrentMeasureId();
    if (this.selectMeasureDlg.ShowDialog() != DialogResult.OK)
      return;
    long selectedMeasureId = this.selectMeasureDlg.SelectedMeasureID;
    foreach (ListViewItem selectedItem in this.lvMeasures.SelectedItems)
    {
      this.saveMeasureID(selectedItem, selectedMeasureId);
      if (selectedItem.Focused)
        this.updateMeasureView();
    }
  }

  private void textBox_KeyDown(object sender, KeyEventArgs e)
  {
    if (e.KeyCode == Keys.Delete && this.lvMeasures.FocusedItem != null)
      (this.lvMeasures.FocusedItem.Tag as ImbaseMeasureItem).MeasureID = 0L;
    this.updateMeasureView();
  }

  private void lvMeasures_SelectedIndexChanged(object sender, EventArgs e)
  {
    this.updateMeasureView();
  }

  private void lvMeasures_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
  {
    this.deltaColumn = this.splitContainer1.Panel1.Width - this.lvMeasures.Columns[0].Width;
  }

  private void ResizeColumn()
  {
    if (this.lvMeasures.Columns.Count == 0)
      return;
    this.lvMeasures.ColumnWidthChanged += new ColumnWidthChangedEventHandler(this.lvMeasures_ColumnWidthChanged);
    try
    {
      this.lvMeasures.Columns[0].Width = this.splitContainer1.Panel1.Width - this.deltaColumn;
    }
    finally
    {
      this.lvMeasures.ColumnWidthChanged += new ColumnWidthChangedEventHandler(this.lvMeasures_ColumnWidthChanged);
    }
  }

  private void splitContainer1_Panel1_SizeChanged(object sender, EventArgs e)
  {
    this.ResizeColumn();
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
    ListViewItem listViewItem = new ListViewItem("рпопропр");
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ImbaseSettingsMeasures));
    this.textBox = new TextBox();
    this.buttonSelect = new Button();
    this.splitContainer1 = new SplitContainer();
    this.lvMeasures = new ListView();
    this.columnHeader1 = new ColumnHeader();
    this.imageList1 = new ImageList(this.components);
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this.label1 = new Label();
    this.propertyGrid = new PropertyGrid();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.Panel2.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    this.tableLayoutPanel1.SuspendLayout();
    this.SuspendLayout();
    this.tableLayoutPanel1.SetColumnSpan((Control) this.textBox, 3);
    this.textBox.Dock = DockStyle.Fill;
    this.textBox.Location = new Point(3, 25);
    this.textBox.Margin = new Padding(3, 5, 3, 3);
    this.textBox.Name = "textBox";
    this.textBox.ReadOnly = true;
    this.textBox.Size = new Size(276, 20);
    this.textBox.TabIndex = 1;
    this.textBox.KeyDown += new KeyEventHandler(this.textBox_KeyDown);
    this.buttonSelect.Location = new Point(285, 23);
    this.buttonSelect.Name = "buttonSelect";
    this.buttonSelect.Size = new Size(24, 23);
    this.buttonSelect.TabIndex = 2;
    this.buttonSelect.Text = "...";
    this.buttonSelect.UseVisualStyleBackColor = true;
    this.buttonSelect.Click += new EventHandler(this.buttonSelect_Click);
    this.splitContainer1.Dock = DockStyle.Fill;
    this.splitContainer1.Location = new Point(0, 0);
    this.splitContainer1.Name = "splitContainer1";
    this.splitContainer1.Panel1.Controls.Add((Control) this.lvMeasures);
    this.splitContainer1.Panel1.Padding = new Padding(3);
    this.splitContainer1.Panel1.SizeChanged += new EventHandler(this.splitContainer1_Panel1_SizeChanged);
    this.splitContainer1.Panel2.Controls.Add((Control) this.tableLayoutPanel1);
    this.splitContainer1.Size = new Size(626, 509);
    this.splitContainer1.SplitterDistance = 310;
    this.splitContainer1.TabIndex = 3;
    this.lvMeasures.Columns.AddRange(new ColumnHeader[1]
    {
      this.columnHeader1
    });
    this.lvMeasures.Dock = DockStyle.Fill;
    listViewItem.Checked = true;
    listViewItem.StateImageIndex = 1;
    this.lvMeasures.Items.AddRange(new ListViewItem[1]
    {
      listViewItem
    });
    this.lvMeasures.Location = new Point(3, 3);
    this.lvMeasures.Name = "lvMeasures";
    this.lvMeasures.Size = new Size(304, 503);
    this.lvMeasures.StateImageList = this.imageList1;
    this.lvMeasures.TabIndex = 7;
    this.lvMeasures.UseCompatibleStateImageBehavior = false;
    this.lvMeasures.View = View.Details;
    this.lvMeasures.ColumnWidthChanged += new ColumnWidthChangedEventHandler(this.lvMeasures_ColumnWidthChanged);
    this.lvMeasures.SelectedIndexChanged += new EventHandler(this.lvMeasures_SelectedIndexChanged);
    this.columnHeader1.Text = "Единицы измерения, имеющиеся в IMBASE";
    this.columnHeader1.Width = 300;
    this.imageList1.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList1.ImageStream");
    this.imageList1.TransparentColor = Color.Transparent;
    this.imageList1.Images.SetKeyName(0, "blank.gif");
    this.imageList1.Images.SetKeyName(1, "Единицы измерения.ico");
    this.tableLayoutPanel1.ColumnCount = 4;
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
    this.tableLayoutPanel1.Controls.Add((Control) this.buttonSelect, 3, 1);
    this.tableLayoutPanel1.Controls.Add((Control) this.textBox, 0, 1);
    this.tableLayoutPanel1.Controls.Add((Control) this.label1, 0, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this.propertyGrid, 0, 2);
    this.tableLayoutPanel1.Dock = DockStyle.Fill;
    this.tableLayoutPanel1.Location = new Point(0, 0);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.RowCount = 4;
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 55f));
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.Size = new Size(312, 509);
    this.tableLayoutPanel1.TabIndex = 0;
    this.label1.AutoSize = true;
    this.tableLayoutPanel1.SetColumnSpan((Control) this.label1, 4);
    this.label1.Dock = DockStyle.Fill;
    this.label1.Location = new Point(3, 0);
    this.label1.Name = "label1";
    this.label1.Size = new Size(306, 20);
    this.label1.TabIndex = 3;
    this.label1.Text = "Единица измерения";
    this.label1.TextAlign = ContentAlignment.MiddleCenter;
    this.tableLayoutPanel1.SetColumnSpan((Control) this.propertyGrid, 4);
    this.propertyGrid.Dock = DockStyle.Fill;
    this.propertyGrid.Location = new Point(3, 52);
    this.propertyGrid.Name = "propertyGrid";
    this.propertyGrid.Size = new Size(306, 454);
    this.propertyGrid.TabIndex = 4;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.splitContainer1);
    this.Name = nameof (ImbaseSettingsMeasures);
    this.Size = new Size(626, 509);
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.Panel2.ResumeLayout(false);
    this.splitContainer1.ResumeLayout(false);
    this.tableLayoutPanel1.ResumeLayout(false);
    this.tableLayoutPanel1.PerformLayout();
    this.ResumeLayout(false);
  }

  protected class linkedMeasure
  {
    private long measureID;
    private double measureKoef;
    private string measureLongName = string.Empty;
    private string measureShortName = string.Empty;
    private long physValueID;
    private string physValueName = string.Empty;

    [DisplayName("Идентификатор единицы измерения")]
    public long MeasureID => this.measureID;

    [DisplayName("Коэффициент приведения единицы измерения")]
    public double MeasureKoef => this.measureKoef;

    [DisplayName("Наименование единицы измерения")]
    public string MeasureLongName => this.measureLongName;

    [DisplayName("Краткое имя единицы измерения")]
    public string MeasureShortName => this.measureShortName;

    [DisplayName("Идентификатор физичекой величины")]
    public long PhysValueID => this.physValueID;

    [DisplayName("Наименование физичекой величины")]
    public string PhysValueName => this.physValueName;

    public linkedMeasure(IMeasureItem measure, IPhysicalValueItem physValue)
    {
      this.measureID = measure.Id;
      this.measureKoef = measure.Koef;
      this.measureLongName = measure.LongName;
      this.measureShortName = measure.ShortName;
      this.physValueID = physValue.Id;
      this.physValueName = physValue.Name;
    }
  }
}
