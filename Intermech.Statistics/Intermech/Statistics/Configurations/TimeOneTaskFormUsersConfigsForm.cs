// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.Configurations.TimeOneTaskFormUsersConfigsForm
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Bars;
using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Kernel.Search;
using Intermech.Statistics.Controls;
using Intermech.Statistics.Interfaces;
using Intermech.Statistics.Properties;
using Intermech.Workflow;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Statistics.Configurations;

public class TimeOneTaskFormUsersConfigsForm : Form, IStatisticSettingsForm
{
  private CommandSettings _settings = new CommandSettings();
  private List<ActivityItem> _activity = new List<ActivityItem>();
  private bool _usingAsControl;
  private IContainer components;
  private Button btnCancel;
  private Button btnOK;
  public TabControl optionsTab;
  public TabPage activityTab;
  private GroupBox groupBox1;
  private Label label1;
  private Label label3;
  private Label label2;
  public ComboBox comboBox1;
  public DateTimePicker startDate;
  public DateTimePicker startTime;
  public DateTimePicker endDate;
  public DateTimePicker endTime;
  private Panel panel1;
  private Panel panel2;
  private ExcludeValuesForTasksCntrl excludeAbnormalValuesCntrl1;
  private ListView lbActivityOneTask;
  private ColumnHeader activityNameRevertCountTask;
  private ColumnHeader activityIDRevertCountTask;
  private ColumnHeader activityTemplateName;
  private Intermech.Bars.ToolBar toolBar1;
  private ButtonItem btnAddActivity;
  private ButtonItem btnDeleteActivity;

  public CommandSettings Settings => this._settings;

  public event EventHandler OnApplied;

  public event EventHandler OnModified;

  public event EventHandler OnCancelModify;

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

  public TimeOneTaskFormUsersConfigsForm(CommandSettings commandSettings = null)
  {
    this.InitializeComponent();
    this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
    this.btnDeleteActivity.Enabled = false;
    try
    {
      this.comboBox1.DataSource = (object) Enum.GetValues(typeof (CollectPeriodsEnum)).Cast<Enum>().Select(value => new
      {
        Description = (Attribute.GetCustomAttribute((MemberInfo) value.GetType().GetField(value.ToString()), typeof (DescriptionAttribute)) as DescriptionAttribute).Description,
        value = value
      }).OrderBy(item => item.value).ToList();
      this.comboBox1.DisplayMember = "Description";
      this.comboBox1.ValueMember = "value";
      if (this.comboBox1.Items.Count > 0)
        this.comboBox1.SelectedIndex = 1;
    }
    catch
    {
      this.comboBox1.DataSource = (object) Enum.GetValues(typeof (CollectPeriodsEnum));
    }
    this.InitForm(commandSettings);
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
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      this.lbActivityOneTask.BeginUpdate();
      this.lbActivityOneTask.Items.Clear();
      this._activity.Clear();
      foreach (ActivityItem activity in commandSettings.Activities)
      {
        ListViewItem listViewItem = new ListViewItem()
        {
          Text = activity.Caption,
          Tag = (object) activity
        };
        string activityTemplateCaption = ControlHelper.GetActivityTemplateCaption(activity.ObjID, sessionKeeper.Session);
        listViewItem.SubItems.Add(activityTemplateCaption);
        listViewItem.SubItems.Add(activity.ObjID.ToString());
        this.lbActivityOneTask.Items.Add(listViewItem);
        this._activity.Add(activity);
      }
      this.lbActivityOneTask.EndUpdate();
    }
    ControlHelper.AutoResizeColumns(this.lbActivityOneTask);
    this.startDate.Value = commandSettings.StartDateTime;
    this.startTime.Value = commandSettings.StartDateTime;
    this.endDate.Value = commandSettings.EndDateTime.Date;
    this.endTime.Value = commandSettings.EndDateTime;
    this.excludeAbnormalValuesCntrl1.Percent = commandSettings.ExcludeAbnormalValuesSettings.Percentage.ToString();
    this.excludeAbnormalValuesCntrl1.NeedExcludeAbnormalValues = commandSettings.ExcludeAbnormalValuesSettings.NeedExcludeAbnormalValues;
    if (this.comboBox1.Items.Count <= 0)
      return;
    this.comboBox1.SelectedIndex = commandSettings.CollectPeriodIndex;
  }

  private void InitFormByDefault()
  {
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
    this.SetTimeAvailability();
    this.excludeAbnormalValuesCntrl1.NeedExcludeAbnormalValues = true;
    this.excludeAbnormalValuesCntrl1.Percent = StatisticsConst.DefaultDeviationPercentage.ToString();
  }

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

  private void btnAddActivity_Click(object sender, EventArgs e)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      SchemeChoosingForm schemeChoosingForm = new SchemeChoosingForm(false);
      if (schemeChoosingForm.ShowDialog() != DialogResult.OK || schemeChoosingForm.Schemes.Count == 0)
        return;
      object[] columns = new object[4]
      {
        (object) ObligatoryObjectAttributes.F_OBJECT_ID,
        (object) ObligatoryObjectAttributes.CAPTION,
        (object) ObligatoryObjectAttributes.F_ID,
        (object) ObligatoryObjectAttributes.F_OBJECT_TYPE
      };
      DBRecordSetParams dbRecordSetParams = new DBRecordSetParams(new ConditionStructure[2]
      {
        new ConditionStructure(-9, RelationalOperators.Equal, (object) MetaDataHelper.GetLCLevelID("cad00013-306c-11d8-b4e9-00304f19f545"), LogicalOperators.AND, 0, true),
        new ConditionStructure(wfConsts.AttrProcessID, RelationalOperators.Equal, (object) schemeChoosingForm.Schemes[0], LogicalOperators.AND, 0, true)
      }, columns);
      dbRecordSetParams.AddColumnDescriptors(new ColumnDescriptor[1]
      {
        new ColumnDescriptor((object) wfConsts.AttrProcessID, AttributeSourceTypes.Object, ColumnContents.ID, ColumnNameMapping.ID, SortOrders.NONE, 0)
      }, (List<int>) null);
      LocalTypesList localTypesList = new LocalTypesList(new int[2]
      {
        wfConsts.ApproveTypeID,
        wfConsts.TaskTypeID
      });
      dbRecordSetParams.Tags = new HybridDictionary()
      {
        {
          (object) "LocalTypesSelector",
          (object) localTypesList
        }
      };
      DataRowCollection rows = sessionKeeper.Session.ObjectsSelect(wfConsts.ActivitiesTypeID, dbRecordSetParams).Rows;
      if (rows != null)
      {
        if (rows.Count > 0)
        {
          SelectRootTaskActivity rootTaskActivity = new SelectRootTaskActivity(rows, true);
          if (rootTaskActivity.ShowDialog() == DialogResult.OK)
          {
            if (rootTaskActivity.SelectedActivity.Count > 0)
            {
              RootActivityListBox rootActivityListBox = rootTaskActivity.SelectedActivity[0];
              ActivityItem activityItem = new ActivityItem(rootActivityListBox.ActivityCaption, rootActivityListBox.ID, rootActivityListBox.ActivityObjID);
              this._activity.Clear();
              this.lbActivityOneTask.Items.Clear();
              if (!this._activity.Contains(activityItem))
              {
                ListViewItem listViewItem = new ListViewItem()
                {
                  Text = rootActivityListBox.ActivityCaption,
                  Tag = (object) activityItem
                };
                string activityTemplateCaption = ControlHelper.GetActivityTemplateCaption(rootActivityListBox.ActivityObjID, sessionKeeper.Session);
                listViewItem.SubItems.Add(activityTemplateCaption);
                listViewItem.SubItems.Add(rootActivityListBox.ActivityObjID.ToString());
                this.lbActivityOneTask.Items.Add(listViewItem);
                this._activity.Add(activityItem);
              }
            }
          }
        }
      }
    }
    ControlHelper.AutoResizeColumns(this.lbActivityOneTask);
    this.Modify();
  }

  private void btnDeleteActivity_Click(object sender, EventArgs e)
  {
    if (!ControlHelper.CanRemoveItems(this.lbActivityOneTask.SelectedItems.Count, "задачу", "задачи"))
      return;
    for (int index = 0; index < this.lbActivityOneTask.SelectedItems.Count; index = index - 1 + 1)
    {
      ListViewItem selectedItem = this.lbActivityOneTask.SelectedItems[index];
      this._activity.Remove(selectedItem.Tag as ActivityItem);
      this.lbActivityOneTask.Items.Remove(selectedItem);
    }
    ControlHelper.AutoResizeColumns(this.lbActivityOneTask);
    this.Modify();
  }

  private void btnOK_Click(object sender, EventArgs e)
  {
    CommandSettings commandSettings = new CommandSettings();
    commandSettings.ObjectID = this._settings.ObjectID;
    commandSettings.AnalizedObjectsTypes = new List<ObjectTypesListItem>();
    commandSettings.StatisticsObjectType = StatisticsObjectsTypeEnum.CommandStatisticsObject;
    commandSettings.CollectPeriod = (CollectPeriodsEnum) this.comboBox1.SelectedIndex;
    commandSettings.CollectPeriodIndex = this.comboBox1.SelectedIndex;
    commandSettings.CommandType = CommandStatisticsTypesEnum.TimeOneTaskFormUsers;
    commandSettings.LCStep = this._settings.LCStep;
    commandSettings.LCLevel = this._settings.LCLevel;
    commandSettings.AttrData = this._settings.AttrData;
    commandSettings.ListUsers = this._settings.ListUsers;
    commandSettings.StatisticsUsersType = this._settings.StatisticsUsersType;
    commandSettings.Activities = new List<ActivityItem>((IEnumerable<ActivityItem>) this._activity);
    DateTime dateTime1 = this.startDate.Value;
    int year1 = dateTime1.Year;
    dateTime1 = this.startDate.Value;
    int month1 = dateTime1.Month;
    dateTime1 = this.startDate.Value;
    int day1 = dateTime1.Day;
    dateTime1 = this.startTime.Value;
    int hour1 = dateTime1.Hour;
    dateTime1 = this.startTime.Value;
    int minute1 = dateTime1.Minute;
    dateTime1 = this.startTime.Value;
    int second1 = dateTime1.Second;
    commandSettings.StartDateTime = new DateTime(year1, month1, day1, hour1, minute1, second1);
    DateTime dateTime2 = this.endDate.Value;
    int year2 = dateTime2.Year;
    dateTime2 = this.endDate.Value;
    int month2 = dateTime2.Month;
    dateTime2 = this.endDate.Value;
    int day2 = dateTime2.Day;
    dateTime2 = this.endTime.Value;
    int hour2 = dateTime2.Hour;
    dateTime2 = this.endTime.Value;
    int minute2 = dateTime2.Minute;
    dateTime2 = this.endTime.Value;
    int second2 = dateTime2.Second;
    commandSettings.EndDateTime = new DateTime(year2, month2, day2, hour2, minute2, second2);
    commandSettings.ExcludeAbnormalValuesSettings = new ExcludeAbnormalValuesSettings(this.excludeAbnormalValuesCntrl1.NeedExcludeAbnormalValues, Convert.ToUInt32(this.excludeAbnormalValuesCntrl1.Percent));
    this._settings = commandSettings;
    if (!this._usingAsControl)
      this.Close();
    this.btnOK.Enabled = false;
    this.btnCancel.Enabled = false;
    this.Applied();
  }

  private void ConfigsForm_FormClosed(object sender, FormClosedEventArgs e)
  {
    FormStorage.SaveLayout((Control) this);
  }

  private void ConfigsForm_Load(object sender, EventArgs e)
  {
    FormStorage.LoadLayout((Control) this);
  }

  private void startDate_ValueChanged(object sender, EventArgs e) => this.Modify();

  private void startTime_ValueChanged(object sender, EventArgs e) => this.Modify();

  private void endDate_ValueChanged(object sender, EventArgs e) => this.Modify();

  private void endTime_ValueChanged(object sender, EventArgs e) => this.Modify();

  private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
  {
    this.SetTimeAvailability();
    this.Modify();
  }

  private void btnCancel_Click(object sender, EventArgs e)
  {
    if (this._usingAsControl)
      this.InitForm(this._settings);
    EventHandler onCancelModify = this.OnCancelModify;
    if (onCancelModify == null)
      return;
    onCancelModify((object) this, EventArgs.Empty);
  }

  private void excludeAbnormalValuesCntrl1_OnModified(object sender, EventArgs e) => this.Modify();

  private void lbActivityOneTask_SelectedIndexChanged(object sender, EventArgs e)
  {
    if (this.lbActivityOneTask.SelectedItems.Count == 0)
      this.btnDeleteActivity.Enabled = false;
    else
      this.btnDeleteActivity.Enabled = true;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.btnCancel = new Button();
    this.btnOK = new Button();
    this.optionsTab = new TabControl();
    this.activityTab = new TabPage();
    this.toolBar1 = new Intermech.Bars.ToolBar();
    this.btnAddActivity = new ButtonItem();
    this.btnDeleteActivity = new ButtonItem();
    this.lbActivityOneTask = new ListView();
    this.activityNameRevertCountTask = new ColumnHeader();
    this.activityTemplateName = new ColumnHeader();
    this.activityIDRevertCountTask = new ColumnHeader();
    this.groupBox1 = new GroupBox();
    this.label1 = new Label();
    this.label3 = new Label();
    this.label2 = new Label();
    this.comboBox1 = new ComboBox();
    this.startDate = new DateTimePicker();
    this.startTime = new DateTimePicker();
    this.endDate = new DateTimePicker();
    this.endTime = new DateTimePicker();
    this.panel1 = new Panel();
    this.panel2 = new Panel();
    this.excludeAbnormalValuesCntrl1 = new ExcludeValuesForTasksCntrl();
    this.optionsTab.SuspendLayout();
    this.activityTab.SuspendLayout();
    this.groupBox1.SuspendLayout();
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.SuspendLayout();
    this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.Location = new Point(693, 5);
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.Size = new Size(89, 23);
    this.btnCancel.TabIndex = 14;
    this.btnCancel.Text = "Отмена";
    this.btnCancel.UseVisualStyleBackColor = true;
    this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
    this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnOK.DialogResult = DialogResult.OK;
    this.btnOK.Location = new Point(592, 5);
    this.btnOK.Name = "btnOK";
    this.btnOK.Size = new Size(89, 23);
    this.btnOK.TabIndex = 15;
    this.btnOK.Text = "ОК";
    this.btnOK.UseVisualStyleBackColor = true;
    this.btnOK.Click += new EventHandler(this.btnOK_Click);
    this.optionsTab.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.optionsTab.Controls.Add((Control) this.activityTab);
    this.optionsTab.Location = new Point(9, 183);
    this.optionsTab.Name = "optionsTab";
    this.optionsTab.SelectedIndex = 0;
    this.optionsTab.Size = new Size(777, 334);
    this.optionsTab.TabIndex = 13;
    this.activityTab.Controls.Add((Control) this.toolBar1);
    this.activityTab.Controls.Add((Control) this.lbActivityOneTask);
    this.activityTab.Location = new Point(4, 22);
    this.activityTab.Name = "activityTab";
    this.activityTab.Padding = new Padding(3);
    this.activityTab.Size = new Size(769, 308);
    this.activityTab.TabIndex = 0;
    this.activityTab.Text = "Анализируемая задача";
    this.activityTab.UseVisualStyleBackColor = true;
    this.toolBar1.FullMenus = true;
    this.toolBar1.Guid = new Guid("f93a5cb1-6939-4308-914f-ed53a484499d");
    this.toolBar1.Hidden = false;
    this.toolBar1.Items.AddRange(new ToolbarItemBase[2]
    {
      (ToolbarItemBase) this.btnAddActivity,
      (ToolbarItemBase) this.btnDeleteActivity
    });
    this.toolBar1.Location = new Point(3, 3);
    this.toolBar1.Name = "toolBar1";
    this.toolBar1.Size = new Size(763, 24);
    this.toolBar1.TabIndex = 9;
    this.toolBar1.Text = "toolBar1";
    this.btnAddActivity.CommandName = "btnAddActivity";
    this.btnAddActivity.Image = (Image) Resources.add;
    this.btnAddActivity.ToolTipText = "Добавить задачу";
    this.btnAddActivity.Click += new EventHandler(this.btnAddActivity_Click);
    this.btnDeleteActivity.CommandName = "btnDeleteActivity";
    this.btnDeleteActivity.Image = (Image) Resources.minus;
    this.btnDeleteActivity.ToolTipText = "Удалить задачу";
    this.btnDeleteActivity.Click += new EventHandler(this.btnDeleteActivity_Click);
    this.lbActivityOneTask.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.lbActivityOneTask.Columns.AddRange(new ColumnHeader[3]
    {
      this.activityNameRevertCountTask,
      this.activityTemplateName,
      this.activityIDRevertCountTask
    });
    this.lbActivityOneTask.FullRowSelect = true;
    this.lbActivityOneTask.HideSelection = false;
    this.lbActivityOneTask.Location = new Point(-1, 29);
    this.lbActivityOneTask.Name = "lbActivityOneTask";
    this.lbActivityOneTask.Size = new Size(770, 281);
    this.lbActivityOneTask.TabIndex = 8;
    this.lbActivityOneTask.UseCompatibleStateImageBehavior = false;
    this.lbActivityOneTask.View = View.Details;
    this.lbActivityOneTask.SelectedIndexChanged += new EventHandler(this.lbActivityOneTask_SelectedIndexChanged);
    this.activityNameRevertCountTask.Text = "Наименование";
    this.activityNameRevertCountTask.Width = 116;
    this.activityTemplateName.Text = "Шаблон";
    this.activityTemplateName.Width = 155;
    this.activityIDRevertCountTask.Text = "Идентификатор версии объекта";
    this.activityIDRevertCountTask.Width = 150;
    this.groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.groupBox1.Controls.Add((Control) this.label1);
    this.groupBox1.Controls.Add((Control) this.label3);
    this.groupBox1.Controls.Add((Control) this.label2);
    this.groupBox1.Controls.Add((Control) this.comboBox1);
    this.groupBox1.Controls.Add((Control) this.startDate);
    this.groupBox1.Controls.Add((Control) this.startTime);
    this.groupBox1.Controls.Add((Control) this.endDate);
    this.groupBox1.Controls.Add((Control) this.endTime);
    this.groupBox1.Location = new Point(8, 3);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(777, 116);
    this.groupBox1.TabIndex = 12;
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
    this.comboBox1.Size = new Size(588, 21);
    this.comboBox1.TabIndex = 3;
    this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
    this.startDate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.startDate.Location = new Point(181, 10);
    this.startDate.Name = "startDate";
    this.startDate.Size = new Size(498, 20);
    this.startDate.TabIndex = 1;
    this.startDate.ValueChanged += new EventHandler(this.startDate_ValueChanged);
    this.startTime.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.startTime.Format = DateTimePickerFormat.Time;
    this.startTime.Location = new Point(686, 10);
    this.startTime.Name = "startTime";
    this.startTime.ShowUpDown = true;
    this.startTime.Size = new Size(83, 20);
    this.startTime.TabIndex = 1;
    this.startTime.Value = new DateTime(2019, 9, 17, 0, 0, 0, 0);
    this.startTime.ValueChanged += new EventHandler(this.startTime_ValueChanged);
    this.endDate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.endDate.Location = new Point(181, 47);
    this.endDate.Name = "endDate";
    this.endDate.Size = new Size(498, 20);
    this.endDate.TabIndex = 1;
    this.endDate.ValueChanged += new EventHandler(this.endDate_ValueChanged);
    this.endTime.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.endTime.Format = DateTimePickerFormat.Time;
    this.endTime.Location = new Point(685, 47);
    this.endTime.Name = "endTime";
    this.endTime.ShowUpDown = true;
    this.endTime.Size = new Size(84, 20);
    this.endTime.TabIndex = 1;
    this.endTime.Value = new DateTime(2019, 9, 17, 0, 0, 0, 0);
    this.endTime.ValueChanged += new EventHandler(this.endTime_ValueChanged);
    this.panel1.Controls.Add((Control) this.btnOK);
    this.panel1.Controls.Add((Control) this.btnCancel);
    this.panel1.Dock = DockStyle.Bottom;
    this.panel1.Location = new Point(0, 520);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(789, 31 /*0x1F*/);
    this.panel1.TabIndex = 16 /*0x10*/;
    this.panel2.AutoScroll = true;
    this.panel2.Controls.Add((Control) this.excludeAbnormalValuesCntrl1);
    this.panel2.Controls.Add((Control) this.groupBox1);
    this.panel2.Controls.Add((Control) this.optionsTab);
    this.panel2.Dock = DockStyle.Fill;
    this.panel2.Location = new Point(0, 0);
    this.panel2.Name = "panel2";
    this.panel2.Size = new Size(789, 520);
    this.panel2.TabIndex = 17;
    this.excludeAbnormalValuesCntrl1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.excludeAbnormalValuesCntrl1.Location = new Point(9, 126);
    this.excludeAbnormalValuesCntrl1.Name = "excludeAbnormalValuesCntrl1";
    this.excludeAbnormalValuesCntrl1.NeedExcludeAbnormalValues = true;
    this.excludeAbnormalValuesCntrl1.Percent = "200";
    this.excludeAbnormalValuesCntrl1.Size = new Size(776, 51);
    this.excludeAbnormalValuesCntrl1.TabIndex = 14;
    this.excludeAbnormalValuesCntrl1.OnModified += new EventHandler(this.excludeAbnormalValuesCntrl1_OnModified);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.btnCancel;
    this.ClientSize = new Size(789, 551);
    this.Controls.Add((Control) this.panel2);
    this.Controls.Add((Control) this.panel1);
    this.FormBorderStyle = FormBorderStyle.FixedDialog;
    this.MaximizeBox = false;
    this.MinimumSize = new Size(660, 590);
    this.Name = nameof (TimeOneTaskFormUsersConfigsForm);
    this.StartPosition = FormStartPosition.CenterScreen;
    this.Text = "Конфигуратор анализатора выполнения задачи разными пользователями";
    this.FormClosed += new FormClosedEventHandler(this.ConfigsForm_FormClosed);
    this.Load += new EventHandler(this.ConfigsForm_Load);
    this.optionsTab.ResumeLayout(false);
    this.activityTab.ResumeLayout(false);
    this.groupBox1.ResumeLayout(false);
    this.groupBox1.PerformLayout();
    this.panel1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
