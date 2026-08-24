// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.Configurations.DateAttrValueConfigsForm
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Statistics.Controls;
using Intermech.Statistics.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Statistics.Configurations;

public class DateAttrValueConfigsForm : Form, IStatisticSettingsForm
{
  private CommandSettings _settings = new CommandSettings();
  public UsersEnum UserType;
  private bool _usingAsControl;
  private bool _canCloseForm = true;
  private ListItem _dateAttr;
  private IContainer components;
  private Button btnCancel;
  private Button btnOK;
  private TabControl config;
  private TabPage createdDateCommandSettings;
  private GroupBox groupBox1;
  private Label label1;
  private Label label3;
  private Label label2;
  public ComboBox comboBox1;
  public DateTimePicker startDate;
  public DateTimePicker startTime;
  public DateTimePicker endDate;
  public DateTimePicker endTime;
  private TabPage generalSettings;
  private TabControl tabControl2;
  private TabPage objectsTypes;
  private Label dateAttrAdd;
  private AnalyzeObjectTypeControl analyzeObjectTypeControl1;
  private CustomListBox dateAttrCustomListBox;
  private ExcludeValuesForCommandsCntrl excludeValuesCntrl;
  private ToolTip toolTip1;
  private GroupBox groupBox2;
  private Panel panel1;
  private TabPage filters;
  private FiltersControl filtersControl;
  private LaborInputControl laborInputControl;

  private void Modify()
  {
    this.btnCancel.Enabled = true;
    this.btnOK.Enabled = true;
    EventHandler onModified = this.OnModified;
    if (onModified == null)
      return;
    onModified((object) this, EventArgs.Empty);
  }

  protected void Applied()
  {
    EventHandler onApplied = this.OnApplied;
    if (onApplied == null)
      return;
    onApplied((object) this, EventArgs.Empty);
  }

  public DateAttrValueConfigsForm(CommandSettings commandSettings = null)
  {
    this.InitializeComponent();
    this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
    this.InitForm(commandSettings);
    this.analyzeObjectTypeControl1.ItemsChanged += new AnalyzeObjectTypeControl.ModifyItems(this.AnalyzeObjectTypeItemsChanged);
    this.filtersControl.OnModified += new EventHandler(this.FiltersControl_OnModify);
    this.laborInputControl.OnModified += new EventHandler(this.LaborInputControl_OnModify);
  }

  public void InitForm(CommandSettings commandSettings)
  {
    if (commandSettings == null)
      this.InitFormByDefault();
    else
      this.InitFormBySettings(commandSettings);
    if (!this._usingAsControl)
      return;
    this.btnCancel.Enabled = false;
    this.btnOK.Enabled = false;
  }

  private void InitFormBySettings(CommandSettings commandSettings)
  {
    this._settings = (CommandSettings) commandSettings.Clone();
    DateTimePicker startDate = this.startDate;
    DateTime now1 = DateTime.Now;
    int year1 = now1.Year;
    now1 = DateTime.Now;
    int month1 = now1.Month;
    now1 = DateTime.Now;
    int day1 = now1.Day;
    DateTime dateTime1 = new DateTime(year1, month1, day1, 23, 59, 59);
    startDate.MaxDate = dateTime1;
    DateTimePicker endDate = this.endDate;
    DateTime now2 = DateTime.Now;
    int year2 = now2.Year;
    now2 = DateTime.Now;
    int month2 = now2.Month;
    now2 = DateTime.Now;
    int day2 = now2.Day;
    DateTime dateTime2 = new DateTime(year2, month2, day2, 23, 59, 59);
    endDate.MaxDate = dateTime2;
    this.analyzeObjectTypeControl1.Init(commandSettings.AnalizedObjectsTypes, commandSettings.CommandType);
    this.filtersControl.Init(commandSettings.Filters, commandSettings.CommandType);
    if (commandSettings.AttrData != null)
      this.analyzeObjectTypeControl1.SetAttrForFilter(Convert.ToInt32(commandSettings.AttrData.ID));
    this.dateAttrCustomListBox.Items.Clear();
    if (commandSettings.AttrData != null && MetaDataHelper.GetAttributeType((int) commandSettings.AttrData.ID) != null)
    {
      this.dateAttrCustomListBox.Items.Add((object) commandSettings.AttrData);
      this._dateAttr = commandSettings.AttrData;
    }
    this.UserType = commandSettings.StatisticsUsersType;
    this.startDate.Value = commandSettings.StartDateTime;
    this.startTime.Value = commandSettings.StartDateTime;
    this.endDate.Value = commandSettings.EndDateTime.Date;
    this.endTime.Value = commandSettings.EndDateTime;
    this.comboBox1.DataSource = (object) ControlHelper.GetPeriodEnumValueList();
    this.comboBox1.DisplayMember = "Description";
    this.comboBox1.ValueMember = "Value";
    if (this.comboBox1.Items.Count > 0)
      this.comboBox1.SelectedIndex = commandSettings.CollectPeriodIndex;
    this.excludeValuesCntrl.Percent = commandSettings.ExcludeAbnormalValuesSettings.Percentage.ToString();
    this.excludeValuesCntrl.NeedExcludeAbnormalValues = commandSettings.ExcludeAbnormalValuesSettings.NeedExcludeAbnormalValues;
    this.excludeValuesCntrl.IgnoreNotWorkingDays = commandSettings.IgnoreNotWorkingDays;
    List<int> list = this.analyzeObjectTypeControl1.TypesListItems.Select<ObjectTypesListItem, int>((Func<ObjectTypesListItem, int>) (x => x.ObjectTypeID)).ToList<int>();
    this.laborInputControl.Init(commandSettings.LaborInput.Formula, list);
  }

  private void InitFormByDefault()
  {
    this.analyzeObjectTypeControl1.Init(CommandStatisticsTypesEnum.DateAttrValue);
    this.filtersControl.Init(CommandStatisticsTypesEnum.DateAttrValue);
    DateTimePicker startDate1 = this.startDate;
    DateTime now1 = DateTime.Now;
    int year1 = now1.Year;
    now1 = DateTime.Now;
    int month1 = now1.Month;
    now1 = DateTime.Now;
    int day1 = now1.Day;
    DateTime dateTime1 = new DateTime(year1, month1, day1, 23, 59, 59);
    startDate1.MaxDate = dateTime1;
    DateTimePicker startDate2 = this.startDate;
    DateTime now2 = DateTime.Now;
    int year2 = now2.Year;
    now2 = DateTime.Now;
    int month2 = now2.Month;
    now2 = DateTime.Now;
    int day2 = now2.Day;
    DateTime dateTime2 = new DateTime(year2, month2, day2, 0, 0, 0);
    startDate2.Value = dateTime2;
    DateTimePicker endDate1 = this.endDate;
    DateTime now3 = DateTime.Now;
    int year3 = now3.Year;
    now3 = DateTime.Now;
    int month3 = now3.Month;
    now3 = DateTime.Now;
    int day3 = now3.Day;
    DateTime dateTime3 = new DateTime(year3, month3, day3, 23, 59, 59);
    endDate1.MaxDate = dateTime3;
    DateTimePicker endDate2 = this.endDate;
    DateTime now4 = DateTime.Now;
    int year4 = now4.Year;
    now4 = DateTime.Now;
    int month4 = now4.Month;
    now4 = DateTime.Now;
    int day4 = now4.Day;
    DateTime dateTime4 = new DateTime(year4, month4, day4, 23, 59, 59);
    endDate2.Value = dateTime4;
    DateTimePicker endTime = this.endTime;
    DateTime now5 = DateTime.Now;
    int year5 = now5.Year;
    now5 = DateTime.Now;
    int month5 = now5.Month;
    now5 = DateTime.Now;
    int day5 = now5.Day;
    DateTime dateTime5 = new DateTime(year5, month5, day5, 23, 59, 59);
    endTime.Value = dateTime5;
    this.comboBox1.DataSource = (object) ControlHelper.GetPeriodEnumValueList();
    this.comboBox1.DisplayMember = "Description";
    this.comboBox1.ValueMember = "Value";
    if (this.comboBox1.Items.Count > 0)
      this.comboBox1.SelectedIndex = 1;
    this.SetTimeAvailability();
    this.SetIgnoringNotWorkingDaysAvailability();
    this.excludeValuesCntrl.IgnoreNotWorkingDays = false;
    this.excludeValuesCntrl.NeedExcludeAbnormalValues = true;
    this.excludeValuesCntrl.Percent = StatisticsConst.DefaultDeviationPercentage.ToString();
    this.laborInputControl.Init(string.Empty, new List<int>());
  }

  public event EventHandler OnApplied;

  public event EventHandler OnCancelModify;

  public CommandSettings Settings => this._settings;

  public event EventHandler OnModified;

  public void Save(object sender, EventArgs e) => this.btnOK_Click(sender, e);

  public void SetAsControl(Control parentControl)
  {
    this.AcceptButton = (IButtonControl) null;
    this.CancelButton = (IButtonControl) null;
    this.TopLevel = false;
    this.FormBorderStyle = FormBorderStyle.None;
    this.Dock = DockStyle.Fill;
    this.Parent = parentControl;
    this.AutoScroll = true;
    this.btnCancel.Enabled = false;
    this.btnOK.Enabled = false;
    this.MinimumSize = new Size(0, 40);
    this._usingAsControl = true;
  }

  private void dateAttrAdd_Click(object sender, EventArgs e)
  {
    using (AttributesSelectDlg attributesSelectDlg = new AttributesSelectDlg(false))
    {
      attributesSelectDlg.AllowedAttrsTypesFilter.Add(FieldTypes.ftDateTime);
      if (this.analyzeObjectTypeControl1.TypesListItems.Count > 0)
      {
        List<int> list = this.analyzeObjectTypeControl1.TypesListItems.Cast<object>().Select<object, ObjectTypesListItem>((Func<object, ObjectTypesListItem>) (type => type as ObjectTypesListItem)).Select<ObjectTypesListItem, int>((Func<ObjectTypesListItem, int>) (item => item.ObjectTypeID)).ToList<int>();
        attributesSelectDlg.LoadAttrDialogForObjectsTypes(list);
      }
      if (attributesSelectDlg.ShowDialog() == DialogResult.OK)
      {
        foreach (int num in attributesSelectDlg.SelectedAttributesID)
        {
          ListItem listItem = new ListItem(MetaDataHelper.GetAttributeTypeName(num), (long) num);
          this._dateAttr = listItem;
          this.analyzeObjectTypeControl1.ClearTypes();
          this.analyzeObjectTypeControl1.SetAttrForFilter(Convert.ToInt32(this._dateAttr.ID));
          this.dateAttrCustomListBox.BeginUpdate();
          this.dateAttrCustomListBox.Items.Clear();
          this.dateAttrCustomListBox.Items.Add((object) listItem);
          this.dateAttrCustomListBox.EndUpdate();
        }
      }
    }
    this.Modify();
  }

  private void btnOK_Click(object sender, EventArgs e)
  {
    DialogResult dialogResult = DialogResult.Yes;
    if (this._dateAttr == null)
    {
      dialogResult = MessageBox.Show("В настройках не задан атрибут типа 'Дата' для которого должен производиться подсчет статистики. Все равно продолжить?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
      if (dialogResult == DialogResult.Yes && this.analyzeObjectTypeControl1.TypesListItems.Count == 0)
        dialogResult = MessageBox.Show("В настройках не заданы типы объектов для которых должен производиться подсчет статистики. Все равно продолжить?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
    }
    if (dialogResult == DialogResult.Yes)
    {
      CommandSettings commandSettings1 = new CommandSettings()
      {
        ObjectID = this._settings.ObjectID,
        Filters = this.filtersControl.Filters,
        AnalizedObjectsTypes = new List<ObjectTypesListItem>(),
        StatisticsObjectType = StatisticsObjectsTypeEnum.CommandStatisticsObject,
        CollectPeriod = (CollectPeriodsEnum) this.comboBox1.SelectedIndex,
        CollectPeriodIndex = this.comboBox1.SelectedIndex,
        CommandType = CommandStatisticsTypesEnum.DateAttrValue,
        LCStep = this._settings.LCStep,
        LCLevel = this._settings.LCLevel,
        AttrData = this._dateAttr,
        ListUsers = this._settings.ListUsers,
        StatisticsUsersType = this.UserType,
        IgnoreNotWorkingDays = this.excludeValuesCntrl.IgnoreNotWorkingDays,
        ExcludeAbnormalValuesSettings = new ExcludeAbnormalValuesSettings(this.excludeValuesCntrl.NeedExcludeAbnormalValues, Convert.ToUInt32(this.excludeValuesCntrl.Percent)),
        LaborInput = new LaborInput(this.laborInputControl.Formula)
      };
      foreach (ObjectTypesListItem typesListItem in this.analyzeObjectTypeControl1.TypesListItems)
        commandSettings1.AnalizedObjectsTypes.Add(typesListItem);
      CommandSettings commandSettings2 = commandSettings1;
      DateTime dateTime1 = this.startDate.Value;
      int year1 = dateTime1.Year;
      dateTime1 = this.startDate.Value;
      int month1 = dateTime1.Month;
      int day1 = this.startDate.Value.Day;
      int hour1 = this.startTime.Value.Hour;
      int minute1 = this.startTime.Value.Minute;
      int second1 = this.startTime.Value.Second;
      DateTime dateTime2 = new DateTime(year1, month1, day1, hour1, minute1, second1);
      commandSettings2.StartDateTime = dateTime2;
      CommandSettings commandSettings3 = commandSettings1;
      DateTime dateTime3 = this.endDate.Value;
      int year2 = dateTime3.Year;
      dateTime3 = this.endDate.Value;
      int month2 = dateTime3.Month;
      dateTime3 = this.endDate.Value;
      int day2 = dateTime3.Day;
      dateTime3 = this.endTime.Value;
      int hour2 = dateTime3.Hour;
      dateTime3 = this.endTime.Value;
      int minute2 = dateTime3.Minute;
      dateTime3 = this.endTime.Value;
      int second2 = dateTime3.Second;
      DateTime dateTime4 = new DateTime(year2, month2, day2, hour2, minute2, second2);
      commandSettings3.EndDateTime = dateTime4;
      this._settings = commandSettings1;
      this._canCloseForm = true;
      if (!this._usingAsControl)
        this.Close();
      this.btnOK.Enabled = false;
      this.btnCancel.Enabled = false;
      this.Applied();
    }
    else
    {
      this._canCloseForm = false;
      this.btnOK.Enabled = true;
      this.btnCancel.Enabled = true;
    }
  }

  private void FormClose(object sender, FormClosingEventArgs e)
  {
    if (this._canCloseForm)
      return;
    e.Cancel = true;
  }

  private void ConfigsForm_FormClosed(object sender, FormClosedEventArgs e)
  {
    FormStorage.SaveLayout((Control) this);
  }

  private void ConfigsForm_Load(object sender, EventArgs e)
  {
    FormStorage.LoadLayout((Control) this);
  }

  private void btnCancel_Click(object sender, EventArgs e)
  {
    this._canCloseForm = true;
    if (this._usingAsControl)
      this.InitForm(this._settings);
    EventHandler onCancelModify = this.OnCancelModify;
    if (onCancelModify == null)
      return;
    onCancelModify((object) this, EventArgs.Empty);
  }

  private void startDate_ValueChanged(object sender, EventArgs e) => this.Modify();

  private void startTime_ValueChanged(object sender, EventArgs e) => this.Modify();

  private void endDate_ValueChanged(object sender, EventArgs e) => this.Modify();

  private void endTime_ValueChanged(object sender, EventArgs e) => this.Modify();

  private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
  {
    this.SetTimeAvailability();
    this.SetIgnoringNotWorkingDaysAvailability();
    this.Modify();
  }

  private void AnalyzeObjectTypeItemsChanged(bool message)
  {
    this.UpdateFormulaControl();
    this.Modify();
  }

  private void UpdateFormulaControl()
  {
    this.laborInputControl.ClearData();
    List<int> list = this.analyzeObjectTypeControl1.TypesListItems.Select<ObjectTypesListItem, int>((Func<ObjectTypesListItem, int>) (x => x.ObjectTypeID)).ToList<int>();
    this.laborInputControl.Init(string.Empty, list);
  }

  private void excludeAbnormalValuesCntrl1_OnModified(object sender, EventArgs e) => this.Modify();

  private void FiltersControl_OnModify(object sender, EventArgs e) => this.Modify();

  private void LaborInputControl_OnModify(object sender, EventArgs e) => this.Modify();

  private void SetTimeAvailability()
  {
    if (this.comboBox1.SelectedIndex == 0)
    {
      this.startTime.Enabled = this.endTime.Enabled = true;
    }
    else
    {
      this.startTime.Value = StatisticsConst.StartTimeInitial;
      this.endTime.Value = StatisticsConst.EndTimeInitial;
      this.startTime.Enabled = this.endTime.Enabled = false;
    }
  }

  private void SetIgnoringNotWorkingDaysAvailability()
  {
    if (this.comboBox1.SelectedIndex == 1)
    {
      this.excludeValuesCntrl.IgnoreWorkingDaysEnable = true;
    }
    else
    {
      this.excludeValuesCntrl.IgnoreWorkingDaysEnable = false;
      this.excludeValuesCntrl.IgnoreNotWorkingDays = false;
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
    this.btnCancel = new Button();
    this.btnOK = new Button();
    this.config = new TabControl();
    this.createdDateCommandSettings = new TabPage();
    this.groupBox2 = new GroupBox();
    this.dateAttrCustomListBox = new CustomListBox();
    this.dateAttrAdd = new Label();
    this.excludeValuesCntrl = new ExcludeValuesForCommandsCntrl();
    this.groupBox1 = new GroupBox();
    this.label1 = new Label();
    this.label3 = new Label();
    this.label2 = new Label();
    this.comboBox1 = new ComboBox();
    this.startDate = new DateTimePicker();
    this.startTime = new DateTimePicker();
    this.endDate = new DateTimePicker();
    this.endTime = new DateTimePicker();
    this.generalSettings = new TabPage();
    this.tabControl2 = new TabControl();
    this.objectsTypes = new TabPage();
    this.analyzeObjectTypeControl1 = new AnalyzeObjectTypeControl();
    this.filters = new TabPage();
    this.filtersControl = new FiltersControl();
    this.toolTip1 = new ToolTip(this.components);
    this.panel1 = new Panel();
    this.laborInputControl = new LaborInputControl();
    this.config.SuspendLayout();
    this.createdDateCommandSettings.SuspendLayout();
    this.groupBox2.SuspendLayout();
    this.groupBox1.SuspendLayout();
    this.generalSettings.SuspendLayout();
    this.tabControl2.SuspendLayout();
    this.objectsTypes.SuspendLayout();
    this.filters.SuspendLayout();
    this.panel1.SuspendLayout();
    this.SuspendLayout();
    this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.Location = new Point(893, 8);
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.Size = new Size(89, 23);
    this.btnCancel.TabIndex = 19;
    this.btnCancel.Text = "Отмена";
    this.btnCancel.UseVisualStyleBackColor = true;
    this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
    this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnOK.DialogResult = DialogResult.OK;
    this.btnOK.Location = new Point(798, 8);
    this.btnOK.Name = "btnOK";
    this.btnOK.Size = new Size(89, 23);
    this.btnOK.TabIndex = 20;
    this.btnOK.Text = "ОК";
    this.btnOK.UseVisualStyleBackColor = true;
    this.btnOK.Click += new EventHandler(this.btnOK_Click);
    this.config.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.config.Controls.Add((Control) this.createdDateCommandSettings);
    this.config.Controls.Add((Control) this.generalSettings);
    this.config.Location = new Point(5, 1);
    this.config.Name = "config";
    this.config.SelectedIndex = 0;
    this.config.Size = new Size(988, 532);
    this.config.TabIndex = 21;
    this.createdDateCommandSettings.AutoScroll = true;
    this.createdDateCommandSettings.BackColor = SystemColors.Control;
    this.createdDateCommandSettings.Controls.Add((Control) this.laborInputControl);
    this.createdDateCommandSettings.Controls.Add((Control) this.groupBox2);
    this.createdDateCommandSettings.Controls.Add((Control) this.excludeValuesCntrl);
    this.createdDateCommandSettings.Controls.Add((Control) this.groupBox1);
    this.createdDateCommandSettings.Location = new Point(4, 22);
    this.createdDateCommandSettings.Name = "createdDateCommandSettings";
    this.createdDateCommandSettings.Padding = new Padding(3);
    this.createdDateCommandSettings.Size = new Size(980, 506);
    this.createdDateCommandSettings.TabIndex = 0;
    this.createdDateCommandSettings.Text = "Настройки сбора статистики по значению атрибута типа \"Дата\"";
    this.groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.groupBox2.Controls.Add((Control) this.dateAttrCustomListBox);
    this.groupBox2.Controls.Add((Control) this.dateAttrAdd);
    this.groupBox2.Location = new Point(9, 197);
    this.groupBox2.Name = "groupBox2";
    this.groupBox2.Size = new Size(968, 52);
    this.groupBox2.TabIndex = 10;
    this.groupBox2.TabStop = false;
    this.groupBox2.Text = "Атрибут типа Дата";
    this.dateAttrCustomListBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.dateAttrCustomListBox.DrawMode = DrawMode.OwnerDrawFixed;
    this.dateAttrCustomListBox.FormattingEnabled = true;
    this.dateAttrCustomListBox.ItemHeight = 21;
    this.dateAttrCustomListBox.Location = new Point(8, 19);
    this.dateAttrCustomListBox.Name = "dateAttrCustomListBox";
    this.dateAttrCustomListBox.SelectionMode = SelectionMode.MultiExtended;
    this.dateAttrCustomListBox.Size = new Size(923, 25);
    this.dateAttrCustomListBox.TabIndex = 12;
    this.dateAttrAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.dateAttrAdd.BackColor = SystemColors.ControlLight;
    this.dateAttrAdd.BorderStyle = BorderStyle.FixedSingle;
    this.dateAttrAdd.Font = new Font("Tahoma", 6f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
    this.dateAttrAdd.Location = new Point(939, 19);
    this.dateAttrAdd.Name = "dateAttrAdd";
    this.dateAttrAdd.Size = new Size(24, 24);
    this.dateAttrAdd.TabIndex = 10;
    this.dateAttrAdd.Text = "...";
    this.dateAttrAdd.TextAlign = ContentAlignment.MiddleCenter;
    this.dateAttrAdd.Click += new EventHandler(this.dateAttrAdd_Click);
    this.excludeValuesCntrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.excludeValuesCntrl.IgnoreNotWorkingDays = false;
    this.excludeValuesCntrl.Location = new Point(9, 126);
    this.excludeValuesCntrl.Name = "excludeValuesCntrl";
    this.excludeValuesCntrl.NeedExcludeAbnormalValues = true;
    this.excludeValuesCntrl.Percent = "200";
    this.excludeValuesCntrl.Size = new Size(972, 65);
    this.excludeValuesCntrl.TabIndex = 9;
    this.excludeValuesCntrl.OnModified += new EventHandler(this.excludeAbnormalValuesCntrl1_OnModified);
    this.groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.groupBox1.BackColor = SystemColors.Control;
    this.groupBox1.Controls.Add((Control) this.label1);
    this.groupBox1.Controls.Add((Control) this.label3);
    this.groupBox1.Controls.Add((Control) this.label2);
    this.groupBox1.Controls.Add((Control) this.comboBox1);
    this.groupBox1.Controls.Add((Control) this.startDate);
    this.groupBox1.Controls.Add((Control) this.startTime);
    this.groupBox1.Controls.Add((Control) this.endDate);
    this.groupBox1.Controls.Add((Control) this.endTime);
    this.groupBox1.Location = new Point(9, 6);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(972, 114);
    this.groupBox1.TabIndex = 5;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "Данные для графика";
    this.label1.AutoSize = true;
    this.label1.Location = new Point(6, 16 /*0x10*/);
    this.label1.Name = "label1";
    this.label1.Size = new Size(152, 13);
    this.label1.TabIndex = 0;
    this.label1.Text = "Начало отсчета статистики: ";
    this.label3.AutoSize = true;
    this.label3.Location = new Point(5, 86);
    this.label3.Name = "label3";
    this.label3.Size = new Size(144 /*0x90*/, 13);
    this.label3.TabIndex = 4;
    this.label3.Text = "Период сбора статистики: ";
    this.label2.AutoSize = true;
    this.label2.Location = new Point(5, 53);
    this.label2.Name = "label2";
    this.label2.Size = new Size(170, 13);
    this.label2.TabIndex = 0;
    this.label2.Text = "Окончание отсчета статистики: ";
    this.comboBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
    this.comboBox1.FormattingEnabled = true;
    this.comboBox1.Location = new Point(181, 83);
    this.comboBox1.Name = "comboBox1";
    this.comboBox1.Size = new Size(783, 21);
    this.comboBox1.TabIndex = 3;
    this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
    this.startDate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.startDate.Location = new Point(181, 10);
    this.startDate.Name = "startDate";
    this.startDate.Size = new Size(695, 20);
    this.startDate.TabIndex = 1;
    this.startDate.Value = new DateTime(2019, 9, 17, 10, 35, 51, 0);
    this.startDate.ValueChanged += new EventHandler(this.startDate_ValueChanged);
    this.startTime.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.startTime.Format = DateTimePickerFormat.Time;
    this.startTime.Location = new Point(883, 10);
    this.startTime.Name = "startTime";
    this.startTime.ShowUpDown = true;
    this.startTime.Size = new Size(83, 20);
    this.startTime.TabIndex = 1;
    this.startTime.Value = new DateTime(2019, 9, 17, 0, 0, 0, 0);
    this.startTime.ValueChanged += new EventHandler(this.startTime_ValueChanged);
    this.endDate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.endDate.Location = new Point(181, 47);
    this.endDate.Name = "endDate";
    this.endDate.Size = new Size(694, 20);
    this.endDate.TabIndex = 1;
    this.endDate.Value = new DateTime(2019, 9, 17, 0, 0, 0, 0);
    this.endDate.ValueChanged += new EventHandler(this.endDate_ValueChanged);
    this.endTime.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.endTime.Format = DateTimePickerFormat.Time;
    this.endTime.Location = new Point(882, 47);
    this.endTime.Name = "endTime";
    this.endTime.ShowUpDown = true;
    this.endTime.Size = new Size(84, 20);
    this.endTime.TabIndex = 1;
    this.endTime.Value = new DateTime(2019, 9, 17, 0, 0, 0, 0);
    this.endTime.ValueChanged += new EventHandler(this.endTime_ValueChanged);
    this.generalSettings.BackColor = SystemColors.Control;
    this.generalSettings.Controls.Add((Control) this.tabControl2);
    this.generalSettings.Location = new Point(4, 22);
    this.generalSettings.Name = "generalSettings";
    this.generalSettings.Padding = new Padding(3);
    this.generalSettings.Size = new Size(862, 606);
    this.generalSettings.TabIndex = 1;
    this.generalSettings.Text = "Дополнительные настройки";
    this.tabControl2.Controls.Add((Control) this.objectsTypes);
    this.tabControl2.Controls.Add((Control) this.filters);
    this.tabControl2.Dock = DockStyle.Fill;
    this.tabControl2.Location = new Point(3, 3);
    this.tabControl2.Name = "tabControl2";
    this.tabControl2.SelectedIndex = 0;
    this.tabControl2.Size = new Size(856, 600);
    this.tabControl2.TabIndex = 0;
    this.objectsTypes.Controls.Add((Control) this.analyzeObjectTypeControl1);
    this.objectsTypes.Location = new Point(4, 22);
    this.objectsTypes.Name = "objectsTypes";
    this.objectsTypes.Padding = new Padding(3);
    this.objectsTypes.Size = new Size(848, 574);
    this.objectsTypes.TabIndex = 0;
    this.objectsTypes.Text = "Типы анализируемых объектов";
    this.objectsTypes.UseVisualStyleBackColor = true;
    this.analyzeObjectTypeControl1.AutoScroll = true;
    this.analyzeObjectTypeControl1.Dock = DockStyle.Fill;
    this.analyzeObjectTypeControl1.Location = new Point(3, 3);
    this.analyzeObjectTypeControl1.Name = "analyzeObjectTypeControl1";
    this.analyzeObjectTypeControl1.Size = new Size(842, 568);
    this.analyzeObjectTypeControl1.TabIndex = 0;
    this.filters.Controls.Add((Control) this.filtersControl);
    this.filters.Location = new Point(4, 22);
    this.filters.Name = "filters";
    this.filters.Padding = new Padding(3);
    this.filters.Size = new Size(178, 42);
    this.filters.TabIndex = 4;
    this.filters.Text = "Фильтрующие объекты";
    this.filters.UseVisualStyleBackColor = true;
    this.filtersControl.Dock = DockStyle.Fill;
    this.filtersControl.Location = new Point(3, 3);
    this.filtersControl.Margin = new Padding(2);
    this.filtersControl.Name = "filtersControl";
    this.filtersControl.Size = new Size(172, 36);
    this.filtersControl.TabIndex = 0;
    this.panel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.panel1.Controls.Add((Control) this.btnOK);
    this.panel1.Controls.Add((Control) this.btnCancel);
    this.panel1.Location = new Point(9, 535);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(986, 43);
    this.panel1.TabIndex = 22;
    this.laborInputControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.laborInputControl.Location = new Point(9, (int) byte.MaxValue);
    this.laborInputControl.Name = "laborInputControl";
    this.laborInputControl.Size = new Size(968, 132);
    this.laborInputControl.TabIndex = 11;
    this.AcceptButton = (IButtonControl) this.btnOK;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.btnCancel;
    this.ClientSize = new Size(1002, 578);
    this.Controls.Add((Control) this.panel1);
    this.Controls.Add((Control) this.config);
    this.MinimumSize = new Size(660, 440);
    this.Name = nameof (DateAttrValueConfigsForm);
    this.StartPosition = FormStartPosition.CenterScreen;
    this.Text = "Конфигуратор команды статистики";
    this.FormClosing += new FormClosingEventHandler(this.FormClose);
    this.FormClosed += new FormClosedEventHandler(this.ConfigsForm_FormClosed);
    this.Load += new EventHandler(this.ConfigsForm_Load);
    this.config.ResumeLayout(false);
    this.createdDateCommandSettings.ResumeLayout(false);
    this.groupBox2.ResumeLayout(false);
    this.groupBox1.ResumeLayout(false);
    this.groupBox1.PerformLayout();
    this.generalSettings.ResumeLayout(false);
    this.tabControl2.ResumeLayout(false);
    this.objectsTypes.ResumeLayout(false);
    this.filters.ResumeLayout(false);
    this.panel1.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
