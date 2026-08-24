// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.Controls.ImbaseAttributesControl
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Imbase.ItemFactories;
using Intermech.ImpExp.Imbase.Properties;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData;
using Intermech.ImpExp.Interface.CommonData.ItemsToCreate;
using Intermech.ImpExp.Interface.CommonData.SettingsItems;
using Intermech.ImpExp.Interface.Controls;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.ImpExp.Imbase.Controls;

internal class ImbaseAttributesControl : StepControl
{
  private ImbasePlugin _plugin;
  private string _sColumnWidthName = "ColumnWidthName";
  private string _sColumnWidthShortName = "ColumnWidthShortName";
  private string _sListViewWidth = "ListViewWidth";
  private string _sSplitterDistance = "SplitterDistance";
  private string _sSplitterWidth = "SplitterWidth";
  private IAttributeImageList _attrImageList;
  private ImageList _attributesCheckImages;
  private IAttributeTypeToCreateList _attrService;
  private List<ImbaseAttribute> _attributes;
  private ImbaseAttributesControl.ItemComparer itemComparer = new ImbaseAttributesControl.ItemComparer();
  private readonly string _colName = "NAME";
  private readonly string _colShortName = "SHORT_NAME";
  private readonly string _colCheckResult = "CHECK_RESULT";
  private readonly string _colDataType = "DATA_TYPE";
  private readonly string _colSize = "SIZE";
  private readonly string _colUnit = "UNIT";
  private Image _image;
  private IContainer components;
  private GroupBox groupBox1;
  private GroupBox groupBox2;
  private PropertyGrid propertyGrid1;
  private Panel panel3;
  private TextBox tbBindingAttr;
  private Panel panel2;
  private Button bChangeBinding;
  private SplitContainer splitContainer1;
  private GroupBox groupBox3;
  private Label label1;
  private PictureBox pictureBox1;
  private Panel panel1;
  private GroupBox groupBox4;
  private Label label4;
  private Label label3;
  private Label label2;
  private TextBox tbLength;
  private TextBox tbShortName;
  private TextBox tbLongName;
  private ListBox lbTables;
  private Label label5;
  private RadioButton rbCalculate;
  private RadioButton rbData;
  private Label label6;
  private TextBox tbDataType;
  private ContextMenuStrip cmenuAttrs;
  private ToolStripMenuItem miFindErrorNode;
  private ToolStrip toolStrip1;
  private ToolStripButton toolStripButton1;
  private Panel panel4;
  private ToolStripLabel toolStripLabel1;
  private ToolStripTextBox tsbSearch;
  private ToolStripButton bSearch;
  private ToolStripSeparator toolStripSeparator1;
  private iGrid iGrid1;
  private iGCellStyle iGrid1Col0CellStyle;
  private iGColHdrStyle iGrid1Col0ColHdrStyle;
  private iGCellStyle iGrid1Col1CellStyle;
  private iGColHdrStyle iGrid1Col1ColHdrStyle;
  private iGCellStyle iGrid1Col2CellStyle;
  private iGColHdrStyle iGrid1Col2ColHdrStyle;
  private iGCellStyle iGrid1Col3CellStyle;
  private iGColHdrStyle iGrid1Col3ColHdrStyle;
  private iGCellStyle iGrid1Col4CellStyle;
  private iGColHdrStyle iGrid1Col4ColHdrStyle;
  private iGCellStyle iGrid1DefaultCellStyle;
  private iGColHdrStyle iGrid1DefaultColHdrStyle;
  private iGCellStyle iGrid1RowTextColCellStyle;
  private iGCellStyle iGrid1Col5CellStyle;
  private iGColHdrStyle iGrid1Col5ColHdrStyle;
  private TextBox tbUnit;
  private Label label7;
  private Label label8;
  private TextBox tbList;
  private ToolStripSeparator toolStripSeparator2;
  private ToolStripButton bExcelReport;

  public ImbaseAttributesControl(ImbasePlugin plugin)
  {
    this.InitializeComponent();
    this._attrImageList = ServicesManager.GetService(typeof (IAttributeImageList)) as IAttributeImageList;
    this._attrService = ServicesManager.GetService(typeof (IAttributeTypeToCreateList)) as IAttributeTypeToCreateList;
    this._plugin = plugin;
    this._attributes = new List<ImbaseAttribute>();
    this._attributesCheckImages = new ImageList()
    {
      ImageSize = new Size(48 /*0x30*/, 48 /*0x30*/),
      ColorDepth = ColorDepth.Depth24Bit
    };
    IBigImageList service = ServicesManager.GetService(typeof (IBigImageList)) as IBigImageList;
    this._attributesCheckImages.Images.Add(service.ImageList.Images[service.ImageIndex("imgOK")]);
    this._attributesCheckImages.Images.Add(service.ImageList.Images[service.ImageIndex("imgCut")]);
    this._attributesCheckImages.Images.Add(service.ImageList.Images[service.ImageIndex("imgConvert")]);
    this._attributesCheckImages.Images.Add(service.ImageList.Images[service.ImageIndex("imgCancel")]);
    this._attributesCheckImages.Images.Add(service.ImageList.Images[service.ImageIndex("imgLostData")]);
    this._attributesCheckImages.Images.Add(service.ImageList.Images[service.ImageIndex("imgPhysError")]);
  }

  public override SaveSettingsResult SaveSettings() => SaveSettingsResult.ssrOk;

  public override void Cancel()
  {
    base.Cancel();
    this.SaveControl();
  }

  public override bool LeaveControl()
  {
    try
    {
      foreach (iGRow row in (IEnumerable) this.iGrid1.Rows)
      {
        if (((ImbaseAttribute) row.Tag).CheckResult == AttributeCheckResult.cresError)
          return MessageBox.Show("Существуют поля с недопустимой конвертацией данных при импорте в атрибуты IPS. Продолжить ?", "Ошибка", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Hand) == DialogResult.Yes;
      }
      return true;
    }
    finally
    {
      this.SaveControl();
    }
  }

  private void SaveControl()
  {
    ISaveSettings service1 = ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings;
    service1.ClearSettings("IMBASEFIELD");
    Dictionary<string, SaveSettingsAttribute[]> settings = new Dictionary<string, SaveSettingsAttribute[]>();
    ICache service2 = ServicesManager.GetService(typeof (ICache)) as ICache;
    service2.DeleteCache(ImportingCategory.AttributeTypesToCreate);
    IImportingData cache = service2.GetCache(ImportingCategory.AttributeTypesToCreate);
    IMeasures service3 = ServicesManager.GetService(typeof (IMeasures)) as IMeasures;
    try
    {
      foreach (ISettingsGroupItem settingsGroupItem in ImbaseGroups.imTablesDict.Values)
      {
        foreach (IImFieldsItem settingsItem in settingsGroupItem.SettingsItems)
        {
          IAttributeTypeToCreate byGuid = this._attrService.GetByGuid(settingsItem.AttrGuid);
          if (byGuid.IsNew)
            AttributesHelper.SaveAttributeToCreate(byGuid, service3, cache);
          List<SaveSettingsAttribute> settingsAttributeList = new List<SaveSettingsAttribute>()
          {
            new SaveSettingsAttribute("GUID", settingsItem.AttrGuid.ToString())
          };
          settings.Add(settingsItem.UniqueKey, settingsAttributeList.ToArray());
        }
      }
    }
    finally
    {
      service1.SetSettings("IMBASEFIELD", settings);
      service2?.ReleaseCache(ImportingCategory.AttributeTypesToCreate);
    }
    FormStorageEx.SaveSettings((Control) this);
    FormStorageEx.AddAttribute((Control) this, this._sColumnWidthName, Convert.ToString(this.iGrid1.Cols[this._colName].Width));
    FormStorageEx.AddAttribute((Control) this, this._sColumnWidthShortName, Convert.ToString(this.iGrid1.Cols[this._colShortName].Width));
    FormStorageEx.AddAttribute((Control) this, this._sListViewWidth, Convert.ToString(this.iGrid1.Width));
    FormStorageEx.AddAttribute((Control) this, this._sSplitterDistance, Convert.ToString(this.splitContainer1.SplitterDistance));
    FormStorageEx.AddAttribute((Control) this, this._sSplitterWidth, Convert.ToString(this.splitContainer1.Width));
  }

  public override void RefreshControl()
  {
    if (this.iGrid1.Rows.Count <= 0)
      return;
    this.iGrid1.CurRow = this.iGrid1.Rows[0];
  }

  protected override string getCaption()
  {
    return "Привязка атрибутов Imbase к уже существующим в новой системе";
  }

  protected override Image getImage()
  {
    if (this._image == null && ServicesManager.GetService(typeof (IBigImageList)) is IBigImageList service)
      this._image = service.ImageList.Images[service.ImageIndex("imgImbaseFieldsBinding")];
    return this._image;
  }

  public event AttributeBindingChange BindingChange;

  private void AddItem(ImbaseAttribute attr)
  {
    iGRow iGrow = this.iGrid1.Rows.Add();
    iGrow.Cells[this._colName].Value = (object) attr.Name;
    iGrow.Cells[this._colShortName].Value = (object) attr.ShortName;
    iGrow.Cells[this._colCheckResult].Value = (object) this.GetResultShortCaption(attr.CheckResult);
    iGrow.Cells[this._colDataType].Value = (object) EnumDescConverter.GetEnumDescription((Enum) attr.AttributeType);
    iGrow.Cells[this._colSize].Value = attr.AttributeType == ImDataTypeEx.IEX_STRING ? (object) attr.Size.ToString() : (object) string.Empty;
    iGrow.Cells[this._colUnit].Value = (object) attr.Unit;
    iGrow.Tag = (object) attr;
  }

  private string GetResultShortCaption(AttributeCheckResult res)
  {
    switch (res)
    {
      case AttributeCheckResult.cresCut:
        return "Усечение данных";
      case AttributeCheckResult.cresConvert:
        return "Конвертация данных";
      case AttributeCheckResult.cresError:
      case AttributeCheckResult.cresPhysVal:
        return "Не переносится";
      case AttributeCheckResult.cresLost:
        return "Потеря данных";
      default:
        return string.Empty;
    }
  }

  public void AddAtribute(ImbaseAttribute attr)
  {
    if (this.iGrid1.InvokeRequired)
      this.iGrid1.BeginInvoke((Delegate) new ImbaseAttributesControl.AddItemDelegate(this.AddItem), (object) attr);
    else
      this.AddItem(attr);
    this._attributes.Add(attr);
  }

  private void SetControlsData(ImbaseAttribute attr)
  {
    this.tbLongName.Text = attr != null ? attr.Name : string.Empty;
    this.tbShortName.Text = attr != null ? attr.ShortName : string.Empty;
    this.tbLength.Text = attr == null || attr.AttributeType != ImDataTypeEx.IEX_STRING ? string.Empty : attr.Size.ToString();
    this.tbUnit.Text = attr != null ? attr.Unit : string.Empty;
    this.tbList.Text = attr != null ? EnumDescConverter.GetEnumDescription((Enum) attr.MultiValueMode) : string.Empty;
    if (attr != null)
    {
      if (attr.ExistInBase)
        this.rbData.Checked = true;
      else
        this.rbCalculate.Checked = true;
    }
    else
      this.rbData.Checked = true;
    this.tbDataType.Text = attr != null ? EnumDescConverter.GetEnumDescription((Enum) attr.AttributeType) : string.Empty;
    this.lbTables.Items.Clear();
    if (attr != null)
    {
      for (int index = 0; index < attr.PresentInTables.Count; ++index)
        this.lbTables.Items.Add((object) attr.PresentInTables[index]);
    }
    this.pictureBox1.Image = attr != null ? this._attributesCheckImages.Images[(int) attr.CheckResult] : (Image) null;
    this.label1.Text = attr != null ? attr.CheckResultString : string.Empty;
    if (attr != null)
    {
      IAttributeTypeToCreate byGuid = this._attrService.GetByGuid(attr.BindingAttribute.AttributeType);
      this.tbBindingAttr.Text = byGuid.Name;
      this.propertyGrid1.SelectedObject = (object) byGuid;
    }
    else
    {
      this.tbBindingAttr.Text = string.Empty;
      this.propertyGrid1.SelectedObject = (object) null;
    }
  }

  private void Grid1_CurRowChanged(object sender, EventArgs e)
  {
    if (this.iGrid1.CurRow != null && this.iGrid1.CurRow.Tag is ImbaseAttribute)
    {
      if (!(this.iGrid1.CurRow.Tag is ImbaseAttribute tag))
        return;
      this.SetControlsData(tag);
    }
    else
      this.SetControlsData((ImbaseAttribute) null);
  }

  private void ChangeBinding_Click(object sender, EventArgs e)
  {
    if (this.iGrid1.CurRow == null || !(this.iGrid1.CurRow.Tag is ImbaseAttribute))
      return;
    ImbaseAttribute tag = this.iGrid1.CurRow.Tag as ImbaseAttribute;
    this._attrService.SelectDialog.SelectedItemGUID = tag.BindingAttribute.AttributeType;
    if (this._attrService.SelectDialog.ShowDialog() != DialogResult.OK)
      return;
    IAttributeTypeToCreate byGuid = this._attrService.GetByGuid(this._attrService.SelectDialog.SelectedItemGUID);
    bool flag = true;
    if ((tag.NewType == FieldTypes.ftDouble || tag.NewType == FieldTypes.ftInteger) && byGuid.FieldType == FieldTypes.ftMeasured || tag.NewType == FieldTypes.ftMeasured && byGuid.FieldType == FieldTypes.ftDouble)
      flag = false;
    tag.CheckResult = flag ? AttributesHelper.CheckTypes(tag.NewType, byGuid.FieldType, (long) tag.Size, (long) Convert.ToInt32(byGuid.Size), tag.MultiValueMode, byGuid.MultiValueMode) : AttributeCheckResult.cresOk;
    if (tag.CheckResult == AttributeCheckResult.cresError)
    {
      int num = (int) MessageBox.Show("Недопустимое преобразование типов данных", "Преобразование типов данных", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      tag.CheckResult = AttributeCheckResult.cresOk;
    }
    else if (tag.CheckResult == AttributeCheckResult.cresPhysVal)
    {
      int num = (int) MessageBox.Show("Недопустимый перенос значений в текущей физической величине", "Преобразование типов данных", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      tag.CheckResult = AttributeCheckResult.cresOk;
    }
    else if (tag.CheckResult == AttributeCheckResult.cresLost && MessageBox.Show("Возможна потеря данных в связи с неконвертируемыми типами атрибутов. Продолжить ?", "Преобразование типов данных", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
    {
      tag.CheckResult = AttributeCheckResult.cresOk;
    }
    else
    {
      if (tag.CheckResult == AttributeCheckResult.cresCut)
      {
        switch (MessageBox.Show("Возможна потеря данных в связи с разными длинами значений атрибутов. Увеличить длину атрибута в IPS?", "Преобразование типов данных", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
        {
          case DialogResult.Cancel:
            tag.CheckResult = AttributeCheckResult.cresOk;
            return;
          case DialogResult.Yes:
            byGuid.Size = tag.Size > Consts.MaxStringSize ? (long) Consts.MaxStringSize : (long) tag.Size;
            if (tag.Size <= Consts.MaxStringSize)
            {
              tag.CheckResult = AttributeCheckResult.cresOk;
              break;
            }
            break;
        }
      }
      tag.BindingAttribute = new AttributeTypeAttProxy(this._attrService.SelectDialog.SelectedItemGUID, this._attrService.SelectDialog.SelectedItemName);
      this.iGrid1.CurRow.Cells[this._colCheckResult].Value = (object) tag.CheckResultString;
      this.SetControlsData(tag);
      AttributeBindingChange bindingChange = this.BindingChange;
      if (bindingChange == null)
        return;
      bindingChange((object) this, new AttributeBindingEventArgs(tag.Keys, tag.TableIDs, tag.BindingAttribute.AttributeType, tag.CheckResult));
    }
  }

  private void FindErrorNode_Click(object sender, EventArgs e)
  {
    for (int index = this.iGrid1.CurRow != null ? this.iGrid1.CurRow.Index + 1 : 0; index < this.iGrid1.Rows.Count; ++index)
    {
      iGRow row = this.iGrid1.Rows[index];
      if ((row.Tag as ImbaseAttribute).CheckResult == AttributeCheckResult.cresError)
      {
        this.iGrid1.CurRow = row;
        return;
      }
    }
    int num = (int) MessageBox.Show("Атрибутов с ошибкой привязки не найдено", "Результат поиска", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
  }

  private bool FindRow(string searchText, int startIndex)
  {
    for (int index = startIndex; index < this.iGrid1.Rows.Count; ++index)
    {
      iGRow row = this.iGrid1.Rows[index];
      if (row.Cells[this._colName].Text.IndexOf(this.tsbSearch.Text, StringComparison.CurrentCultureIgnoreCase) >= 0)
      {
        this.iGrid1.CurRow = row;
        return true;
      }
    }
    return false;
  }

  private void Search_Click(object sender, EventArgs e)
  {
    if (this.tsbSearch.Text.Trim() == string.Empty || this.iGrid1.Rows.Count == 0)
      return;
    if (this.iGrid1.CurRow != null)
    {
      int index = this.iGrid1.CurRow.Index;
    }
    if (this.FindRow(this.tsbSearch.Text, this.iGrid1.CurRow != null ? this.iGrid1.CurRow.Index + 1 : 0) || MessageBox.Show($"Атрибутов с подстрокой '{this.tsbSearch.Text}' в названии не найдено. Начать поиск с начала ?", "Результат поиска", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes || this.FindRow(this.tsbSearch.Text, 0))
      return;
    int num = (int) MessageBox.Show($"Атрибутов с подстрокой '{this.tsbSearch.Text}' в названии не найдено", "Результат поиска", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
  }

  private void Grid1_CustomDrawCellBackground(object sender, iGCustomDrawCellEventArgs e)
  {
    if (!((e.RowIndex >= 0 ? this.iGrid1.Rows[e.RowIndex] : (iGRow) null).Tag is ImbaseAttribute tag))
      return;
    Brush brush;
    switch (tag.CheckResult)
    {
      case AttributeCheckResult.cresOk:
        brush = (Brush) new SolidBrush(this.iGrid1.BackColor);
        break;
      case AttributeCheckResult.cresError:
      case AttributeCheckResult.cresPhysVal:
        brush = (Brush) new SolidBrush(Color.MistyRose);
        break;
      default:
        brush = (Brush) new SolidBrush(Color.LightYellow);
        break;
    }
    Rectangle bounds = e.Bounds;
    try
    {
      e.Graphics.FillRectangle(brush, bounds);
    }
    finally
    {
      brush.Dispose();
    }
  }

  private void ExcelReport_Click(object sender, EventArgs e)
  {
    List<ImbaseAttribute> attributes = new List<ImbaseAttribute>();
    for (int index = 0; index < this.iGrid1.Rows.Count; ++index)
    {
      iGRow row = this.iGrid1.Rows[index];
      if ((row.Tag as ImbaseAttribute).CheckResult == AttributeCheckResult.cresError)
        attributes.Add(row.Tag as ImbaseAttribute);
    }
    if (attributes.Count == 0)
    {
      int num1 = (int) MessageBox.Show("Атрибутов с ошибкой привязки не найдено", "Генерация отчета", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }
    else
    {
      using (SaveFileDialog saveFileDialog = new SaveFileDialog())
      {
        saveFileDialog.DefaultExt = "*.xlsx";
        saveFileDialog.Filter = "Книга Excel|*.xlsx";
        saveFileDialog.RestoreDirectory = true;
        if (saveFileDialog.ShowDialog() != DialogResult.OK || string.IsNullOrEmpty(saveFileDialog.FileName))
          return;
        new InvalidAttributesReport().Create(saveFileDialog.FileName, this._attrService, attributes);
        int num2 = (int) MessageBox.Show("Отчет успешно создан по пути " + saveFileDialog.FileName, "Генерация отчета", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
    iGColPattern iGcolPattern1 = new iGColPattern();
    iGColPattern iGcolPattern2 = new iGColPattern();
    iGColPattern iGcolPattern3 = new iGColPattern();
    iGColPattern iGcolPattern4 = new iGColPattern();
    iGColPattern iGcolPattern5 = new iGColPattern();
    iGColPattern iGcolPattern6 = new iGColPattern();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ImbaseAttributesControl));
    this.iGrid1Col0CellStyle = new iGCellStyle(true);
    this.iGrid1Col0ColHdrStyle = new iGColHdrStyle(true);
    this.iGrid1Col1CellStyle = new iGCellStyle(true);
    this.iGrid1Col1ColHdrStyle = new iGColHdrStyle(true);
    this.iGrid1Col2CellStyle = new iGCellStyle(true);
    this.iGrid1Col2ColHdrStyle = new iGColHdrStyle(true);
    this.iGrid1Col3CellStyle = new iGCellStyle(true);
    this.iGrid1Col3ColHdrStyle = new iGColHdrStyle(true);
    this.iGrid1Col4CellStyle = new iGCellStyle(true);
    this.iGrid1Col4ColHdrStyle = new iGColHdrStyle(true);
    this.iGrid1Col5CellStyle = new iGCellStyle(true);
    this.iGrid1Col5ColHdrStyle = new iGColHdrStyle(true);
    this.groupBox1 = new GroupBox();
    this.panel4 = new Panel();
    this.iGrid1 = new iGrid();
    this.iGrid1DefaultCellStyle = new iGCellStyle(true);
    this.iGrid1DefaultColHdrStyle = new iGColHdrStyle(true);
    this.iGrid1RowTextColCellStyle = new iGCellStyle(true);
    this.toolStrip1 = new ToolStrip();
    this.toolStripButton1 = new ToolStripButton();
    this.toolStripLabel1 = new ToolStripLabel();
    this.tsbSearch = new ToolStripTextBox();
    this.bSearch = new ToolStripButton();
    this.panel1 = new Panel();
    this.groupBox4 = new GroupBox();
    this.tbList = new TextBox();
    this.label8 = new Label();
    this.tbUnit = new TextBox();
    this.label7 = new Label();
    this.tbDataType = new TextBox();
    this.label6 = new Label();
    this.lbTables = new ListBox();
    this.label5 = new Label();
    this.rbCalculate = new RadioButton();
    this.rbData = new RadioButton();
    this.tbLength = new TextBox();
    this.tbShortName = new TextBox();
    this.tbLongName = new TextBox();
    this.label4 = new Label();
    this.label3 = new Label();
    this.label2 = new Label();
    this.cmenuAttrs = new ContextMenuStrip(this.components);
    this.miFindErrorNode = new ToolStripMenuItem();
    this.toolStripSeparator1 = new ToolStripSeparator();
    this.groupBox2 = new GroupBox();
    this.panel2 = new Panel();
    this.propertyGrid1 = new PropertyGrid();
    this.groupBox3 = new GroupBox();
    this.label1 = new Label();
    this.pictureBox1 = new PictureBox();
    this.panel3 = new Panel();
    this.bChangeBinding = new Button();
    this.tbBindingAttr = new TextBox();
    this.splitContainer1 = new SplitContainer();
    this.bExcelReport = new ToolStripButton();
    this.toolStripSeparator2 = new ToolStripSeparator();
    this.groupBox1.SuspendLayout();
    this.panel4.SuspendLayout();
    ((ISupportInitialize) this.iGrid1).BeginInit();
    this.toolStrip1.SuspendLayout();
    this.panel1.SuspendLayout();
    this.groupBox4.SuspendLayout();
    this.cmenuAttrs.SuspendLayout();
    this.groupBox2.SuspendLayout();
    this.panel2.SuspendLayout();
    this.groupBox3.SuspendLayout();
    ((ISupportInitialize) this.pictureBox1).BeginInit();
    this.panel3.SuspendLayout();
    this.splitContainer1.BeginInit();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.Panel2.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    this.SuspendLayout();
    this.iGrid1Col0CellStyle.CustomDrawFlags = iGCustomDrawFlags.Background;
    this.iGrid1Col1CellStyle.CustomDrawFlags = iGCustomDrawFlags.Background;
    this.iGrid1Col2CellStyle.CustomDrawFlags = iGCustomDrawFlags.Background;
    this.iGrid1Col3CellStyle.CustomDrawFlags = iGCustomDrawFlags.Background;
    this.iGrid1Col4CellStyle.CustomDrawFlags = iGCustomDrawFlags.Background;
    this.groupBox1.Controls.Add((Control) this.panel4);
    this.groupBox1.Controls.Add((Control) this.toolStrip1);
    this.groupBox1.Controls.Add((Control) this.panel1);
    this.groupBox1.Dock = DockStyle.Fill;
    this.groupBox1.Location = new Point(0, 0);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(395, 460);
    this.groupBox1.TabIndex = 0;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "Поле таблицы Imbase";
    this.panel4.Controls.Add((Control) this.iGrid1);
    this.panel4.Dock = DockStyle.Fill;
    this.panel4.Location = new Point(3, 41);
    this.panel4.Name = "panel4";
    this.panel4.Size = new Size(389, 154);
    this.panel4.TabIndex = 3;
    this.iGrid1.AutoResizeCols = true;
    this.iGrid1.BackColorEvenRows = SystemColors.Window;
    this.iGrid1.BackColorOddRows = SystemColors.Window;
    iGcolPattern1.CellStyle = this.iGrid1Col0CellStyle;
    iGcolPattern1.ColHdrStyle = this.iGrid1Col0ColHdrStyle;
    iGcolPattern1.Key = "NAME";
    iGcolPattern1.Text = (object) "Длинное имя";
    iGcolPattern1.Width = 83;
    iGcolPattern2.CellStyle = this.iGrid1Col1CellStyle;
    iGcolPattern2.ColHdrStyle = this.iGrid1Col1ColHdrStyle;
    iGcolPattern2.Key = "SHORT_NAME";
    iGcolPattern2.Text = (object) "Короткое имя";
    iGcolPattern2.Width = 65;
    iGcolPattern3.CellStyle = this.iGrid1Col2CellStyle;
    iGcolPattern3.ColHdrStyle = this.iGrid1Col2ColHdrStyle;
    iGcolPattern3.Key = "CHECK_RESULT";
    iGcolPattern3.Text = (object) "Проверка";
    iGcolPattern3.Width = 52;
    iGcolPattern4.CellStyle = this.iGrid1Col3CellStyle;
    iGcolPattern4.ColHdrStyle = this.iGrid1Col3ColHdrStyle;
    iGcolPattern4.Key = "DATA_TYPE";
    iGcolPattern4.Text = (object) "Тип данных";
    iGcolPattern4.Width = 57;
    iGcolPattern5.CellStyle = this.iGrid1Col4CellStyle;
    iGcolPattern5.ColHdrStyle = this.iGrid1Col4ColHdrStyle;
    iGcolPattern5.Key = "SIZE";
    iGcolPattern5.Text = (object) "Ширина";
    iGcolPattern5.Width = 61;
    iGcolPattern6.CellStyle = this.iGrid1Col5CellStyle;
    iGcolPattern6.ColHdrStyle = this.iGrid1Col5ColHdrStyle;
    iGcolPattern6.Key = "UNIT";
    iGcolPattern6.Text = (object) "Ед.измерения";
    iGcolPattern6.Width = 67;
    this.iGrid1.Cols.AddRange(new iGColPattern[6]
    {
      iGcolPattern1,
      iGcolPattern2,
      iGcolPattern3,
      iGcolPattern4,
      iGcolPattern5,
      iGcolPattern6
    });
    this.iGrid1.DefaultCol.CellStyle = this.iGrid1DefaultCellStyle;
    this.iGrid1.DefaultCol.ColHdrStyle = this.iGrid1DefaultColHdrStyle;
    this.iGrid1.Dock = DockStyle.Fill;
    this.iGrid1.GroupBox.BackColor = SystemColors.AppWorkspace;
    this.iGrid1.GroupBox.HintBackColor = SystemColors.AppWorkspace;
    this.iGrid1.GroupBox.HintForeColor = SystemColors.ControlText;
    this.iGrid1.GroupBox.Text = "Перетащите заголовок колонки в эту область для группировки по значениям этой колонки";
    this.iGrid1.GroupBox.Visible = true;
    this.iGrid1.Header.Height = 19;
    this.iGrid1.HotTracking = false;
    this.iGrid1.Location = new Point(0, 0);
    this.iGrid1.Name = "iGrid1";
    this.iGrid1.ProcessTab = false;
    this.iGrid1.ReadOnly = true;
    this.iGrid1.RowMode = true;
    this.iGrid1.RowModeHasCurCell = true;
    this.iGrid1.RowTextCol.CellStyle = this.iGrid1RowTextColCellStyle;
    this.iGrid1.SilentValidation = true;
    this.iGrid1.Size = new Size(389, 154);
    this.iGrid1.TabIndex = 2;
    this.iGrid1.CustomDrawCellBackground += new iGCustomDrawCellEventHandler(this.Grid1_CustomDrawCellBackground);
    this.iGrid1.CurRowChanged += new EventHandler(this.Grid1_CurRowChanged);
    this.toolStrip1.Items.AddRange(new ToolStripItem[6]
    {
      (ToolStripItem) this.toolStripButton1,
      (ToolStripItem) this.toolStripLabel1,
      (ToolStripItem) this.tsbSearch,
      (ToolStripItem) this.bSearch,
      (ToolStripItem) this.toolStripSeparator2,
      (ToolStripItem) this.bExcelReport
    });
    this.toolStrip1.Location = new Point(3, 16 /*0x10*/);
    this.toolStrip1.Name = "toolStrip1";
    this.toolStrip1.Size = new Size(389, 25);
    this.toolStrip1.TabIndex = 2;
    this.toolStrip1.Text = "toolStrip1";
    this.toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.toolStripButton1.Image = (Image) componentResourceManager.GetObject("toolStripButton1.Image");
    this.toolStripButton1.ImageTransparentColor = Color.Magenta;
    this.toolStripButton1.Name = "toolStripButton1";
    this.toolStripButton1.Size = new Size(23, 22);
    this.toolStripButton1.Text = "Найти атрибут с ошибкой в привязке";
    this.toolStripButton1.Click += new EventHandler(this.FindErrorNode_Click);
    this.toolStripLabel1.Name = "toolStripLabel1";
    this.toolStripLabel1.Size = new Size(44, 22);
    this.toolStripLabel1.Text = "Найти:";
    this.tsbSearch.Name = "tsbSearch";
    this.tsbSearch.Size = new Size(200, 25);
    this.bSearch.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.bSearch.Image = (Image) componentResourceManager.GetObject("bSearch.Image");
    this.bSearch.ImageTransparentColor = Color.Magenta;
    this.bSearch.Name = "bSearch";
    this.bSearch.Size = new Size(23, 22);
    this.bSearch.Text = "Поиск";
    this.bSearch.Click += new EventHandler(this.Search_Click);
    this.panel1.Controls.Add((Control) this.groupBox4);
    this.panel1.Dock = DockStyle.Bottom;
    this.panel1.Location = new Point(3, 195);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(389, 262);
    this.panel1.TabIndex = 0;
    this.groupBox4.Controls.Add((Control) this.tbList);
    this.groupBox4.Controls.Add((Control) this.label8);
    this.groupBox4.Controls.Add((Control) this.tbUnit);
    this.groupBox4.Controls.Add((Control) this.label7);
    this.groupBox4.Controls.Add((Control) this.tbDataType);
    this.groupBox4.Controls.Add((Control) this.label6);
    this.groupBox4.Controls.Add((Control) this.lbTables);
    this.groupBox4.Controls.Add((Control) this.label5);
    this.groupBox4.Controls.Add((Control) this.rbCalculate);
    this.groupBox4.Controls.Add((Control) this.rbData);
    this.groupBox4.Controls.Add((Control) this.tbLength);
    this.groupBox4.Controls.Add((Control) this.tbShortName);
    this.groupBox4.Controls.Add((Control) this.tbLongName);
    this.groupBox4.Controls.Add((Control) this.label4);
    this.groupBox4.Controls.Add((Control) this.label3);
    this.groupBox4.Controls.Add((Control) this.label2);
    this.groupBox4.Dock = DockStyle.Fill;
    this.groupBox4.Location = new Point(0, 0);
    this.groupBox4.Name = "groupBox4";
    this.groupBox4.Size = new Size(389, 262);
    this.groupBox4.TabIndex = 0;
    this.groupBox4.TabStop = false;
    this.groupBox4.Text = "Свойства поля таблицы Imbase";
    this.tbList.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.tbList.BackColor = SystemColors.Window;
    this.tbList.Location = new Point(20, 108);
    this.tbList.Name = "tbList";
    this.tbList.ReadOnly = true;
    this.tbList.Size = new Size(355, 20);
    this.tbList.TabIndex = 8;
    this.label8.AutoSize = true;
    this.label8.Location = new Point(17, 92);
    this.label8.Name = "label8";
    this.label8.Size = new Size(44, 13);
    this.label8.TabIndex = 15;
    this.label8.Text = "Список";
    this.tbUnit.AcceptsReturn = true;
    this.tbUnit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.tbUnit.BackColor = SystemColors.Window;
    this.tbUnit.Location = new Point(294, 69);
    this.tbUnit.Name = "tbUnit";
    this.tbUnit.ReadOnly = true;
    this.tbUnit.Size = new Size(81, 20);
    this.tbUnit.TabIndex = 7;
    this.tbUnit.TextAlign = HorizontalAlignment.Right;
    this.label7.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.label7.AutoSize = true;
    this.label7.Location = new Point(291, 55);
    this.label7.Name = "label7";
    this.label7.Size = new Size(79, 13);
    this.label7.TabIndex = 13;
    this.label7.Text = "Ед.измерения";
    this.tbDataType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.tbDataType.BackColor = SystemColors.Window;
    this.tbDataType.Location = new Point(20, 69);
    this.tbDataType.Name = "tbDataType";
    this.tbDataType.ReadOnly = true;
    this.tbDataType.Size = new Size(181, 20);
    this.tbDataType.TabIndex = 5;
    this.label6.AutoSize = true;
    this.label6.Location = new Point(17, 55);
    this.label6.Name = "label6";
    this.label6.Size = new Size(66, 13);
    this.label6.TabIndex = 11;
    this.label6.Text = "Тип данных";
    this.lbTables.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.lbTables.FormattingEnabled = true;
    this.lbTables.Location = new Point(20, 174);
    this.lbTables.Name = "lbTables";
    this.lbTables.Size = new Size(355, 82);
    this.lbTables.TabIndex = 11;
    this.label5.AutoSize = true;
    this.label5.Location = new Point(16 /*0x10*/, 154);
    this.label5.Name = "label5";
    this.label5.Size = new Size(137, 13);
    this.label5.TabIndex = 8;
    this.label5.Text = "Используется в таблицах";
    this.rbCalculate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.rbCalculate.AutoSize = true;
    this.rbCalculate.Location = new Point(280, 134);
    this.rbCalculate.Name = "rbCalculate";
    this.rbCalculate.Size = new Size(95, 17);
    this.rbCalculate.TabIndex = 10;
    this.rbCalculate.Text = "Вычисляемое";
    this.rbCalculate.UseVisualStyleBackColor = true;
    this.rbData.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.rbData.AutoSize = true;
    this.rbData.Checked = true;
    this.rbData.Location = new Point(158, 134);
    this.rbData.Name = "rbData";
    this.rbData.Size = new Size(116, 17);
    this.rbData.TabIndex = 9;
    this.rbData.TabStop = true;
    this.rbData.Text = "Содержит данные";
    this.rbData.UseVisualStyleBackColor = true;
    this.tbLength.AcceptsReturn = true;
    this.tbLength.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.tbLength.BackColor = SystemColors.Window;
    this.tbLength.Location = new Point(207, 69);
    this.tbLength.Name = "tbLength";
    this.tbLength.ReadOnly = true;
    this.tbLength.Size = new Size(81, 20);
    this.tbLength.TabIndex = 6;
    this.tbLength.TextAlign = HorizontalAlignment.Right;
    this.tbShortName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.tbShortName.BackColor = SystemColors.Window;
    this.tbShortName.Location = new Point(292, 32 /*0x20*/);
    this.tbShortName.Name = "tbShortName";
    this.tbShortName.ReadOnly = true;
    this.tbShortName.Size = new Size(83, 20);
    this.tbShortName.TabIndex = 4;
    this.tbLongName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.tbLongName.BackColor = SystemColors.Window;
    this.tbLongName.Location = new Point(20, 32 /*0x20*/);
    this.tbLongName.Name = "tbLongName";
    this.tbLongName.ReadOnly = true;
    this.tbLongName.Size = new Size(266, 20);
    this.tbLongName.TabIndex = 3;
    this.label4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.label4.AutoSize = true;
    this.label4.Location = new Point(204, 55);
    this.label4.Name = "label4";
    this.label4.Size = new Size(46, 13);
    this.label4.TabIndex = 2;
    this.label4.Text = "Ширина";
    this.label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.label3.AutoSize = true;
    this.label3.Location = new Point(288, 18);
    this.label3.Name = "label3";
    this.label3.Size = new Size(78, 13);
    this.label3.TabIndex = 1;
    this.label3.Text = "Короткое имя";
    this.label2.AutoSize = true;
    this.label2.Location = new Point(16 /*0x10*/, 18);
    this.label2.Name = "label2";
    this.label2.Size = new Size(75, 13);
    this.label2.TabIndex = 0;
    this.label2.Text = "Длинное имя";
    this.cmenuAttrs.Items.AddRange(new ToolStripItem[2]
    {
      (ToolStripItem) this.miFindErrorNode,
      (ToolStripItem) this.toolStripSeparator1
    });
    this.cmenuAttrs.Name = "cmenuAttrs";
    this.cmenuAttrs.Size = new Size(281, 32 /*0x20*/);
    this.miFindErrorNode.Name = "miFindErrorNode";
    this.miFindErrorNode.Size = new Size(280, 22);
    this.miFindErrorNode.Text = "Найти атрибут с ошибкой в привязке";
    this.miFindErrorNode.Click += new EventHandler(this.FindErrorNode_Click);
    this.toolStripSeparator1.Name = "toolStripSeparator1";
    this.toolStripSeparator1.Size = new Size(277, 6);
    this.groupBox2.Controls.Add((Control) this.panel2);
    this.groupBox2.Controls.Add((Control) this.panel3);
    this.groupBox2.Dock = DockStyle.Fill;
    this.groupBox2.Location = new Point(0, 0);
    this.groupBox2.Name = "groupBox2";
    this.groupBox2.Size = new Size(433, 460);
    this.groupBox2.TabIndex = 1;
    this.groupBox2.TabStop = false;
    this.groupBox2.Text = "Соответствующий атрибут в IPS";
    this.panel2.Controls.Add((Control) this.propertyGrid1);
    this.panel2.Controls.Add((Control) this.groupBox3);
    this.panel2.Dock = DockStyle.Fill;
    this.panel2.Location = new Point(3, 59);
    this.panel2.Name = "panel2";
    this.panel2.Size = new Size(427, 398);
    this.panel2.TabIndex = 0;
    this.propertyGrid1.Dock = DockStyle.Fill;
    this.propertyGrid1.HelpVisible = false;
    this.propertyGrid1.Location = new Point(0, 100);
    this.propertyGrid1.Name = "propertyGrid1";
    this.propertyGrid1.Size = new Size(427, 298);
    this.propertyGrid1.TabIndex = 1;
    this.propertyGrid1.ToolbarVisible = false;
    this.groupBox3.Controls.Add((Control) this.label1);
    this.groupBox3.Controls.Add((Control) this.pictureBox1);
    this.groupBox3.Dock = DockStyle.Top;
    this.groupBox3.Location = new Point(0, 0);
    this.groupBox3.Name = "groupBox3";
    this.groupBox3.Size = new Size(427, 100);
    this.groupBox3.TabIndex = 0;
    this.groupBox3.TabStop = false;
    this.groupBox3.Text = "Результат привязки атрибута";
    this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.label1.Location = new Point(60, 27);
    this.label1.Name = "label1";
    this.label1.Size = new Size(361, 48 /*0x30*/);
    this.label1.TabIndex = 1;
    this.label1.TextAlign = ContentAlignment.MiddleLeft;
    this.pictureBox1.Anchor = AnchorStyles.Left;
    this.pictureBox1.Location = new Point(6, 27);
    this.pictureBox1.Name = "pictureBox1";
    this.pictureBox1.Size = new Size(48 /*0x30*/, 48 /*0x30*/);
    this.pictureBox1.TabIndex = 0;
    this.pictureBox1.TabStop = false;
    this.panel3.Controls.Add((Control) this.bChangeBinding);
    this.panel3.Controls.Add((Control) this.tbBindingAttr);
    this.panel3.Dock = DockStyle.Top;
    this.panel3.Location = new Point(3, 16 /*0x10*/);
    this.panel3.Name = "panel3";
    this.panel3.Size = new Size(427, 43);
    this.panel3.TabIndex = 1;
    this.bChangeBinding.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bChangeBinding.Image = (Image) Resources._3point;
    this.bChangeBinding.Location = new Point(395, 9);
    this.bChangeBinding.Name = "bChangeBinding";
    this.bChangeBinding.Size = new Size(23, 23);
    this.bChangeBinding.TabIndex = 1;
    this.bChangeBinding.TextImageRelation = TextImageRelation.ImageBeforeText;
    this.bChangeBinding.UseVisualStyleBackColor = true;
    this.bChangeBinding.Click += new EventHandler(this.ChangeBinding_Click);
    this.tbBindingAttr.AcceptsTab = true;
    this.tbBindingAttr.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.tbBindingAttr.BackColor = SystemColors.Window;
    this.tbBindingAttr.Location = new Point(17, 11);
    this.tbBindingAttr.Name = "tbBindingAttr";
    this.tbBindingAttr.ReadOnly = true;
    this.tbBindingAttr.Size = new Size(372, 20);
    this.tbBindingAttr.TabIndex = 0;
    this.splitContainer1.Dock = DockStyle.Fill;
    this.splitContainer1.Location = new Point(0, 0);
    this.splitContainer1.Name = "splitContainer1";
    this.splitContainer1.Panel1.Controls.Add((Control) this.groupBox1);
    this.splitContainer1.Panel2.Controls.Add((Control) this.groupBox2);
    this.splitContainer1.Size = new Size(832, 460);
    this.splitContainer1.SplitterDistance = 395;
    this.splitContainer1.TabIndex = 2;
    this.bExcelReport.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.bExcelReport.Image = (Image) componentResourceManager.GetObject("bExcelReport.Image");
    this.bExcelReport.ImageTransparentColor = Color.Magenta;
    this.bExcelReport.Name = "bExcelReport";
    this.bExcelReport.Size = new Size(23, 22);
    this.bExcelReport.ToolTipText = "Сохранить атрибуты с ошибкой привязки в Excel";
    this.bExcelReport.Click += new EventHandler(this.ExcelReport_Click);
    this.toolStripSeparator2.Name = "toolStripSeparator2";
    this.toolStripSeparator2.Size = new Size(6, 25);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.splitContainer1);
    this.MinimumSize = new Size(751, 383);
    this.Name = nameof (ImbaseAttributesControl);
    this.Size = new Size(832, 460);
    this.groupBox1.ResumeLayout(false);
    this.groupBox1.PerformLayout();
    this.panel4.ResumeLayout(false);
    ((ISupportInitialize) this.iGrid1).EndInit();
    this.toolStrip1.ResumeLayout(false);
    this.toolStrip1.PerformLayout();
    this.panel1.ResumeLayout(false);
    this.groupBox4.ResumeLayout(false);
    this.groupBox4.PerformLayout();
    this.cmenuAttrs.ResumeLayout(false);
    this.groupBox2.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.groupBox3.ResumeLayout(false);
    ((ISupportInitialize) this.pictureBox1).EndInit();
    this.panel3.ResumeLayout(false);
    this.panel3.PerformLayout();
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.Panel2.ResumeLayout(false);
    this.splitContainer1.EndInit();
    this.splitContainer1.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  private delegate void AddItemDelegate(ImbaseAttribute attr);

  private class ItemComparer : IComparer
  {
    private int columnIndex;
    private bool sortAscending = true;

    public int ColumnIndex
    {
      set
      {
        if (this.columnIndex == value)
        {
          this.sortAscending = !this.sortAscending;
        }
        else
        {
          this.columnIndex = value;
          this.sortAscending = true;
        }
      }
    }

    public int Compare(object x, object y)
    {
      return string.Compare(((ListViewItem) x).SubItems[this.columnIndex].Text, ((ListViewItem) y).SubItems[this.columnIndex].Text) * (this.sortAscending ? 1 : -1);
    }
  }
}
