// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.Configurations.SignDateConfigsForm
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Bars;
using Intermech.DataFormats;
using Intermech.Extensions;
using Intermech.Interfaces.Client;
using Intermech.Navigator;
using Intermech.Navigator.DBObjectTypes;
using Intermech.Navigator.Interfaces;
using Intermech.Security;
using Intermech.Statistics.Controls;
using Intermech.Statistics.Interfaces;
using Intermech.Statistics.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Statistics.Configurations;

public class SignDateConfigsForm : Form, IStatisticSettingsForm
{
  private CommandSettings _settings = new CommandSettings();
  private List<StatisticsUsers> _users = new List<StatisticsUsers>();
  private bool _usingAsControl;
  private bool _canCloseForm = true;
  private UsersEnum _userType;
  private IContainer components;
  private Button btnCancel;
  private Button btnOK;
  private TabControl config;
  private TabPage createdDateCommandSettings;
  public TabControl optionsTab;
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
  public TabPage usersTab;
  public ListBox usersListBox;
  private AnalyzeObjectTypeControl analyzeObjectTypeControl1;
  private ExcludeValuesForCommandsCntrl excludeValuesCntrl;
  private ToolTip toolTip1;
  private TabPage Filters;
  private FiltersControl filtersControl;
  private LaborInputControl laborInputControl;
  private Intermech.Bars.ToolBar tbUser;
  private ButtonItem btnAddUser;
  private ButtonItem btnDeleteUser;

  public UsersEnum UserType
  {
    get => this._userType;
    set
    {
      this._userType = value;
      switch (value)
      {
        case UsersEnum.User:
          this.btnAddUser.ToolTipText = "Добавить пользователя";
          this.btnDeleteUser.ToolTipText = "Удалить пользователя";
          break;
        case UsersEnum.UserGroup:
          this.btnAddUser.ToolTipText = "Добавить группу";
          this.btnDeleteUser.ToolTipText = "Удалить группу";
          break;
        case UsersEnum.Department:
          this.btnAddUser.ToolTipText = "Добавить подразделение";
          this.btnDeleteUser.ToolTipText = "Удалить подразделение";
          break;
      }
    }
  }

  public SignDateConfigsForm(CommandSettings commandSettings = null)
  {
    this.InitializeComponent();
    this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
    this.InitForm(commandSettings);
    this.btnDeleteUser.Enabled = false;
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
    this._users.Clear();
    this.usersListBox.BeginUpdate();
    this.usersListBox.Items.Clear();
    foreach (StatisticsUsers listUser in commandSettings.ListUsers)
    {
      this.usersListBox.Items.Add((object) new ListItem(listUser.Caption, listUser.ID, listUser.ObjectID));
      this._users.Add(listUser);
    }
    this.usersListBox.EndUpdate();
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
    List<int> list = this.analyzeObjectTypeControl1.TypesListItems.Select<ObjectTypesListItem, int>((System.Func<ObjectTypesListItem, int>) (x => x.ObjectTypeID)).ToList<int>();
    this.laborInputControl.Init(commandSettings.LaborInput.Formula, list);
  }

  private void InitFormByDefault()
  {
    this.analyzeObjectTypeControl1.Init(CommandStatisticsTypesEnum.SignDate);
    this.filtersControl.Init(CommandStatisticsTypesEnum.SignDate);
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

  public event EventHandler OnApplied;

  public event EventHandler OnModified;

  public event EventHandler OnCancelModify;

  public CommandSettings Settings => this._settings;

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

  private void btnAddUser_Click(object sender, EventArgs e)
  {
    object[] objArray = (object[]) null;
    switch (this.UserType)
    {
      case UsersEnum.User:
        objArray = SelectionWindow.Select("Выберите пользователей", "", (IDescriptor) new UsersGroupsDescriptor(), typeof (IDBObjectID), (DynamicSelectionEventHandler) null, (IServiceProvider) null, SelectionOptions.SelectObjects | SelectionOptions.DisableSelectAbstractTypes, new int[1]
        {
          StatisticsConst.UserTypeID
        });
        break;
      case UsersEnum.UserGroup:
        objArray = SelectionWindow.Select("Выберите группу пользователей для составления статистики", "", (IDescriptor) new Descriptor(StatisticsConst.UserTypeID), typeof (IDBObjectID), (DynamicSelectionEventHandler) null, (IServiceProvider) null, SelectionOptions.SelectObjects | SelectionOptions.DisableSelectAbstractTypes, new int[1]
        {
          StatisticsConst.GroupTypeID
        });
        break;
      case UsersEnum.Department:
        objArray = SelectionWindow.Select("Выберите подразделения для составления статистики", "", (IDescriptor) new Descriptor(StatisticsConst.DepartmentTypeId), typeof (IDBObjectID), (DynamicSelectionEventHandler) null, (IServiceProvider) null, SelectionOptions.SelectObjects | SelectionOptions.DisableSelectAbstractTypes, new int[1]
        {
          StatisticsConst.DepartmentTypeId
        });
        break;
    }
    if (objArray != null && objArray.Length >= 1)
    {
      switch (this.UserType)
      {
        case UsersEnum.User:
          foreach (object obj in objArray)
          {
            IDBObjectID user = obj as IDBObjectID;
            DataTable usersTable;
            Intermech.Statistics.Interfaces.Extensions.GetGroupAndUsersTable((object) user.Value, out DataTable _, out usersTable);
            if (usersTable == null)
            {
              if (this.usersListBox.Items.IndexOfFirst((Predicate<object>) (x => ((ListItem) x).ID == user.ID)) == -1)
              {
                this.usersListBox.Items.Add((object) new ListItem(user.Caption, user.ID, user.Value));
                this._users.Add(new StatisticsUsers()
                {
                  ID = user.ID,
                  Caption = user.Caption,
                  ObjectID = user.Value,
                  UserType = this.UserType
                });
              }
            }
            else
            {
              foreach (DataRow row in (InternalDataCollectionBase) usersTable.Rows)
              {
                long int64 = Convert.ToInt64(row.ItemArray[0]);
                long id = Convert.ToInt64(row.ItemArray[1]);
                string caption = row.ItemArray[2].ToString();
                if (this.usersListBox.Items.IndexOfFirst((Predicate<object>) (x => ((ListItem) x).ID == id)) == -1)
                {
                  this.usersListBox.Items.Add((object) new ListItem(caption, id, int64));
                  this._users.Add(new StatisticsUsers()
                  {
                    ObjectID = int64,
                    ID = id,
                    Caption = caption,
                    UserType = this.UserType
                  });
                }
              }
            }
          }
          break;
        case UsersEnum.UserGroup:
          foreach (object obj in objArray)
          {
            IDBObjectID group = obj as IDBObjectID;
            bool flag = true;
            if (this._users.Count > 0)
            {
              flag = this.checkIfGroupNecessaryToAdd(group);
              this.RemoveRedundantGroupsIfNecessary(group);
            }
            if (flag && this.usersListBox.Items.IndexOfFirst((Predicate<object>) (x => ((ListItem) x).ID == group.ID)) == -1)
            {
              this.usersListBox.Items.Add((object) new ListItem(group.Caption, group.ID, group.Value));
              this._users.Add(new StatisticsUsers()
              {
                ID = group.ID,
                Caption = group.Caption,
                ObjectID = group.Value,
                UserType = this.UserType
              });
            }
          }
          break;
        case UsersEnum.Department:
          foreach (object obj in objArray)
          {
            IDBObjectID department = obj as IDBObjectID;
            bool flag = true;
            if (this._users.Count > 0)
            {
              flag = this.checkIfDepartmentNecessaryToAdd(department);
              this.RemoveRedundantDepartmentsIfNecessary(department);
            }
            if (flag && this.usersListBox.Items.IndexOfFirst((Predicate<object>) (x => ((ListItem) x).ID == department.ID)) == -1)
            {
              this.usersListBox.Items.Add((object) new ListItem(department.Caption, department.ID, department.Value));
              this._users.Add(new StatisticsUsers()
              {
                ID = department.ID,
                Caption = department.Caption,
                ObjectID = department.Value,
                UserType = this.UserType
              });
            }
          }
          break;
      }
    }
    this.Modify();
  }

  private void btnDeleteUser_Click(object sender, EventArgs e)
  {
    if (!ControlHelper.CanRemoveItems(this.usersListBox.SelectedItems.Count, "пользователя", "пользователей"))
      return;
    for (int index1 = 0; index1 < this.usersListBox.SelectedItems.Count; index1 = index1 - 1 + 1)
    {
      ListItem user = (ListItem) this.usersListBox.SelectedItems[index1];
      int index2 = this._users.IndexOfFirst<StatisticsUsers>((Predicate<StatisticsUsers>) (x => x.ID == user.ID));
      if (index2 != -1)
        this._users.RemoveAt(index2);
      this.usersListBox.Items.Remove(this.usersListBox.SelectedItems[index1]);
    }
    if (this.usersListBox.Items.Count > 0)
      this.usersListBox.SelectedIndex = 0;
    this.Modify();
  }

  private void RemoveRedundantGroupsIfNecessary(IDBObjectID user)
  {
    List<long> choosedGroupRecursive = Intermech.Statistics.Interfaces.Extensions.GetGroupsEntersInChoosedGroupRecursive(this._users, user.Value);
    if (choosedGroupRecursive.Count <= 0 || MessageBox.Show($"В списке есть группы, которые входят в добавляемую группу {user.Caption}.  Удалить дочерние группы из списка?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
      return;
    this.DeleteStatisticsUsersFromField(choosedGroupRecursive);
    this.DeleteStatisticsUsersFromListBox(choosedGroupRecursive);
  }

  private void RemoveRedundantDepartmentsIfNecessary(IDBObjectID user)
  {
    List<long> departmentRecursive = Intermech.Statistics.Interfaces.Extensions.GetDepartmentsEntersInChoosedDepartmentRecursive(this._users, user.Value);
    if (departmentRecursive.Count <= 0 || MessageBox.Show($"В списке есть подразделения, которые входят в добавляемое подразделение {user.Caption}.  Удалить дочерние подразделения из списка?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
      return;
    this.DeleteStatisticsUsersFromField(departmentRecursive);
    this.DeleteStatisticsUsersFromListBox(departmentRecursive);
  }

  private bool checkIfGroupNecessaryToAdd(IDBObjectID user)
  {
    bool add = true;
    string name;
    if (Intermech.Statistics.Interfaces.Extensions.CheckInGroupForParents(user.Value, this._users, out name) && MessageBox.Show($"Добавленная ранее группа '{name}' уже содержит в себе группу '{user.Caption}'. Всё равно добавить?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
      add = false;
    return add;
  }

  private bool checkIfDepartmentNecessaryToAdd(IDBObjectID user)
  {
    bool add = true;
    string name;
    if (Intermech.Statistics.Interfaces.Extensions.CheckInDepartmentForParents(user.Value, this._users, out name) && MessageBox.Show($"Добавленное ранее подразделение '{name}' уже содержит в себе подразделение '{user.Caption}'. Всё равно добавить?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
      add = false;
    return add;
  }

  private void DeleteStatisticsUsersFromListBox(List<long> deletingUserIDs)
  {
    for (int index = 0; index < this.usersListBox.Items.Count; ++index)
    {
      ListItem listItem = (ListItem) this.usersListBox.Items[index];
      if (deletingUserIDs.Contains(listItem.ObjID))
        this.usersListBox.Items.Remove((object) listItem);
    }
  }

  private void DeleteStatisticsUsersFromField(List<long> deletingUserIDs)
  {
    List<StatisticsUsers> values = new List<StatisticsUsers>();
    foreach (StatisticsUsers user in this._users)
    {
      if (deletingUserIDs.Contains(user.ObjectID))
        values.Add(user);
    }
    this._users.RemoveRange<StatisticsUsers>((IEnumerable<StatisticsUsers>) values);
  }

  private void btnOK_Click(object sender, EventArgs e)
  {
    DialogResult dialogResult = DialogResult.Yes;
    if (this.analyzeObjectTypeControl1.TypesListItems.Count == 0)
      dialogResult = MessageBox.Show("В настройках не заданы типы объектов для которых должен производиться подсчет статистики. Все равно продолжить?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
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
        CommandType = CommandStatisticsTypesEnum.SignDate,
        LCStep = this._settings.LCStep,
        LCLevel = this._settings.LCLevel,
        AttrData = this._settings.AttrData,
        ListUsers = this._users,
        StatisticsUsersType = this.UserType,
        IgnoreNotWorkingDays = this.excludeValuesCntrl.IgnoreNotWorkingDays,
        ExcludeAbnormalValuesSettings = new ExcludeAbnormalValuesSettings(this.excludeValuesCntrl.NeedExcludeAbnormalValues, Convert.ToUInt32(this.excludeValuesCntrl.Percent)),
        LaborInput = new LaborInput(this.laborInputControl.Formula)
      };
      foreach (ObjectTypesListItem typesListItem in this.analyzeObjectTypeControl1.TypesListItems)
        commandSettings1.AnalizedObjectsTypes.Add(typesListItem);
      CommandSettings commandSettings2 = commandSettings1;
      int year1 = this.startDate.Value.Year;
      int month1 = this.startDate.Value.Month;
      int day1 = this.startDate.Value.Day;
      int hour1 = this.startTime.Value.Hour;
      DateTime dateTime1 = this.startTime.Value;
      int minute1 = dateTime1.Minute;
      dateTime1 = this.startTime.Value;
      int second1 = dateTime1.Second;
      DateTime dateTime2 = new DateTime(year1, month1, day1, hour1, minute1, second1);
      commandSettings2.StartDateTime = dateTime2;
      CommandSettings commandSettings3 = commandSettings1;
      dateTime1 = this.endDate.Value;
      int year2 = dateTime1.Year;
      dateTime1 = this.endDate.Value;
      int month2 = dateTime1.Month;
      dateTime1 = this.endDate.Value;
      int day2 = dateTime1.Day;
      dateTime1 = this.endTime.Value;
      int hour2 = dateTime1.Hour;
      dateTime1 = this.endTime.Value;
      int minute2 = dateTime1.Minute;
      dateTime1 = this.endTime.Value;
      int second2 = dateTime1.Second;
      DateTime dateTime3 = new DateTime(year2, month2, day2, hour2, minute2, second2);
      commandSettings3.EndDateTime = dateTime3;
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

  private void ConfigsForm_FormClosed(object sender, FormClosedEventArgs e)
  {
    Intermech.Client.Core.FormStorage.SaveLayout((Control) this);
  }

  private void ConfigsForm_Load(object sender, EventArgs e)
  {
    Intermech.Client.Core.FormStorage.LoadLayout((Control) this);
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
    List<int> list = this.analyzeObjectTypeControl1.TypesListItems.Select<ObjectTypesListItem, int>((System.Func<ObjectTypesListItem, int>) (x => x.ObjectTypeID)).ToList<int>();
    this.laborInputControl.Init(string.Empty, list);
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

  private void excludeAbnormalValuesCntrl1_OnModified(object sender, EventArgs e) => this.Modify();

  private void FiltersControl_OnModify(object sender, EventArgs e) => this.Modify();

  private void LaborInputControl_OnModify(object sender, EventArgs e) => this.Modify();

  private void usersListBox_SelectedIndexChanged(object sender, EventArgs e)
  {
    if (this.usersListBox.SelectedItem == null)
      this.btnDeleteUser.Enabled = false;
    else
      this.btnDeleteUser.Enabled = true;
  }

  private void SignDateConfigsForm_FormClosing(object sender, FormClosingEventArgs e)
  {
    if (this._canCloseForm)
      return;
    e.Cancel = true;
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
    this.btnCancel = new Button();
    this.btnOK = new Button();
    this.config = new TabControl();
    this.createdDateCommandSettings = new TabPage();
    this.laborInputControl = new LaborInputControl();
    this.excludeValuesCntrl = new ExcludeValuesForCommandsCntrl();
    this.optionsTab = new TabControl();
    this.usersTab = new TabPage();
    this.tbUser = new Intermech.Bars.ToolBar();
    this.btnAddUser = new ButtonItem();
    this.btnDeleteUser = new ButtonItem();
    this.usersListBox = new ListBox();
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
    this.Filters = new TabPage();
    this.filtersControl = new FiltersControl();
    this.toolTip1 = new ToolTip();
    this.config.SuspendLayout();
    this.createdDateCommandSettings.SuspendLayout();
    this.optionsTab.SuspendLayout();
    this.usersTab.SuspendLayout();
    this.groupBox1.SuspendLayout();
    this.generalSettings.SuspendLayout();
    this.tabControl2.SuspendLayout();
    this.objectsTypes.SuspendLayout();
    this.Filters.SuspendLayout();
    this.SuspendLayout();
    this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.Location = new Point(579, 577);
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.Size = new Size(89, 23);
    this.btnCancel.TabIndex = 10;
    this.btnCancel.Text = "Отмена";
    this.btnCancel.UseVisualStyleBackColor = true;
    this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
    this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnOK.DialogResult = DialogResult.OK;
    this.btnOK.Location = new Point(484, 577);
    this.btnOK.Name = "btnOK";
    this.btnOK.Size = new Size(89, 23);
    this.btnOK.TabIndex = 11;
    this.btnOK.Text = "ОК";
    this.btnOK.UseVisualStyleBackColor = true;
    this.btnOK.Click += new EventHandler(this.btnOK_Click);
    this.config.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.config.Controls.Add((Control) this.createdDateCommandSettings);
    this.config.Controls.Add((Control) this.generalSettings);
    this.config.Location = new Point(3, 1);
    this.config.Name = "config";
    this.config.SelectedIndex = 0;
    this.config.Size = new Size(678, 570);
    this.config.TabIndex = 12;
    this.createdDateCommandSettings.AutoScroll = true;
    this.createdDateCommandSettings.BackColor = SystemColors.Control;
    this.createdDateCommandSettings.Controls.Add((Control) this.laborInputControl);
    this.createdDateCommandSettings.Controls.Add((Control) this.excludeValuesCntrl);
    this.createdDateCommandSettings.Controls.Add((Control) this.optionsTab);
    this.createdDateCommandSettings.Controls.Add((Control) this.groupBox1);
    this.createdDateCommandSettings.Location = new Point(4, 22);
    this.createdDateCommandSettings.Name = "createdDateCommandSettings";
    this.createdDateCommandSettings.Padding = new Padding(3);
    this.createdDateCommandSettings.Size = new Size(670, 544);
    this.createdDateCommandSettings.TabIndex = 0;
    this.createdDateCommandSettings.Text = "Настройки сбора статистики по дате подписания объектов";
    this.laborInputControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.laborInputControl.AutoScroll = true;
    this.laborInputControl.Location = new Point(7, 368);
    this.laborInputControl.Name = "laborInputControl";
    this.laborInputControl.Size = new Size(654, 135);
    this.laborInputControl.TabIndex = 9;
    this.excludeValuesCntrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.excludeValuesCntrl.IgnoreNotWorkingDays = false;
    this.excludeValuesCntrl.Location = new Point(6, (int) sbyte.MaxValue);
    this.excludeValuesCntrl.Name = "excludeValuesCntrl";
    this.excludeValuesCntrl.NeedExcludeAbnormalValues = true;
    this.excludeValuesCntrl.Percent = "200";
    this.excludeValuesCntrl.Size = new Size(655, 66);
    this.excludeValuesCntrl.TabIndex = 8;
    this.excludeValuesCntrl.OnModified += new EventHandler(this.excludeAbnormalValuesCntrl1_OnModified);
    this.optionsTab.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.optionsTab.Controls.Add((Control) this.usersTab);
    this.optionsTab.Location = new Point(6, 199);
    this.optionsTab.Name = "optionsTab";
    this.optionsTab.SelectedIndex = 0;
    this.optionsTab.Size = new Size(661, 163);
    this.optionsTab.TabIndex = 7;
    this.usersTab.AutoScroll = true;
    this.usersTab.Controls.Add((Control) this.tbUser);
    this.usersTab.Controls.Add((Control) this.usersListBox);
    this.usersTab.Location = new Point(4, 22);
    this.usersTab.Name = "usersTab";
    this.usersTab.Padding = new Padding(3);
    this.usersTab.Size = new Size(653, 137);
    this.usersTab.TabIndex = 0;
    this.usersTab.Text = "Расчет статистики для:";
    this.usersTab.UseVisualStyleBackColor = true;
    this.tbUser.FullMenus = true;
    this.tbUser.Guid = new Guid("cbfea772-9072-4b58-813c-6de857da40d2");
    this.tbUser.Hidden = false;
    this.tbUser.Items.AddRange(new ToolbarItemBase[2]
    {
      (ToolbarItemBase) this.btnAddUser,
      (ToolbarItemBase) this.btnDeleteUser
    });
    this.tbUser.Location = new Point(3, 3);
    this.tbUser.Name = "tbUser";
    this.tbUser.Size = new Size(647, 24);
    this.tbUser.TabIndex = 17;
    this.tbUser.Text = "toolBar2";
    this.btnAddUser.CommandName = "buttonItem1";
    this.btnAddUser.Image = (Image) Resources.add;
    this.btnAddUser.ToolTipText = "Добавить ";
    this.btnAddUser.Click += new EventHandler(this.btnAddUser_Click);
    this.btnDeleteUser.CommandName = "btnDelete";
    this.btnDeleteUser.Image = (Image) Resources.minus;
    this.btnDeleteUser.ToolTipText = "Удалить";
    this.btnDeleteUser.Click += new EventHandler(this.btnDeleteUser_Click);
    this.usersListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.usersListBox.FormattingEnabled = true;
    this.usersListBox.Location = new Point(0, 28);
    this.usersListBox.Name = "usersListBox";
    this.usersListBox.SelectionMode = SelectionMode.MultiExtended;
    this.usersListBox.Size = new Size(653, 108);
    this.usersListBox.TabIndex = 2;
    this.usersListBox.SelectedIndexChanged += new EventHandler(this.usersListBox_SelectedIndexChanged);
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
    this.groupBox1.Location = new Point(6, 6);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(655, 114);
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
    this.comboBox1.Location = new Point(187, 83);
    this.comboBox1.Name = "comboBox1";
    this.comboBox1.Size = new Size(462, 21);
    this.comboBox1.TabIndex = 3;
    this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
    this.startDate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.startDate.Location = new Point(187, 10);
    this.startDate.Name = "startDate";
    this.startDate.Size = new Size(373, 20);
    this.startDate.TabIndex = 1;
    this.startDate.ValueChanged += new EventHandler(this.startDate_ValueChanged);
    this.startTime.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.startTime.Format = DateTimePickerFormat.Time;
    this.startTime.Location = new Point(566, 11);
    this.startTime.Name = "startTime";
    this.startTime.ShowUpDown = true;
    this.startTime.Size = new Size(83, 20);
    this.startTime.TabIndex = 1;
    this.startTime.Value = new DateTime(2019, 9, 17, 0, 0, 0, 0);
    this.startTime.ValueChanged += new EventHandler(this.startTime_ValueChanged);
    this.endDate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.endDate.Location = new Point(187, 47);
    this.endDate.Name = "endDate";
    this.endDate.Size = new Size(373, 20);
    this.endDate.TabIndex = 1;
    this.endDate.ValueChanged += new EventHandler(this.endDate_ValueChanged);
    this.endTime.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.endTime.Format = DateTimePickerFormat.Time;
    this.endTime.Location = new Point(565, 46);
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
    this.generalSettings.Size = new Size(950, 544);
    this.generalSettings.TabIndex = 1;
    this.generalSettings.Text = "Дополнительные настройки";
    this.tabControl2.Controls.Add((Control) this.objectsTypes);
    this.tabControl2.Controls.Add((Control) this.Filters);
    this.tabControl2.Dock = DockStyle.Fill;
    this.tabControl2.Location = new Point(3, 3);
    this.tabControl2.Name = "tabControl2";
    this.tabControl2.SelectedIndex = 0;
    this.tabControl2.Size = new Size(944, 538);
    this.tabControl2.TabIndex = 0;
    this.objectsTypes.Controls.Add((Control) this.analyzeObjectTypeControl1);
    this.objectsTypes.Location = new Point(4, 22);
    this.objectsTypes.Name = "objectsTypes";
    this.objectsTypes.Padding = new Padding(3);
    this.objectsTypes.Size = new Size(936, 512 /*0x0200*/);
    this.objectsTypes.TabIndex = 0;
    this.objectsTypes.Text = "Типы анализируемых объектов";
    this.objectsTypes.UseVisualStyleBackColor = true;
    this.analyzeObjectTypeControl1.AutoScroll = true;
    this.analyzeObjectTypeControl1.Dock = DockStyle.Fill;
    this.analyzeObjectTypeControl1.Location = new Point(3, 3);
    this.analyzeObjectTypeControl1.Name = "analyzeObjectTypeControl1";
    this.analyzeObjectTypeControl1.Size = new Size(930, 506);
    this.analyzeObjectTypeControl1.TabIndex = 0;
    this.Filters.Controls.Add((Control) this.filtersControl);
    this.Filters.Location = new Point(4, 22);
    this.Filters.Name = "Filters";
    this.Filters.Size = new Size(178, 42);
    this.Filters.TabIndex = 4;
    this.Filters.Text = "Фильтрующие объекты";
    this.Filters.UseVisualStyleBackColor = true;
    this.filtersControl.AutoScroll = true;
    this.filtersControl.Dock = DockStyle.Fill;
    this.filtersControl.Location = new Point(0, 0);
    this.filtersControl.Margin = new Padding(2);
    this.filtersControl.Name = "filtersControl";
    this.filtersControl.Size = new Size(178, 42);
    this.filtersControl.TabIndex = 0;
    this.AcceptButton = (IButtonControl) this.btnOK;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.btnCancel;
    this.ClientSize = new Size(681, 612);
    this.Controls.Add((Control) this.config);
    this.Controls.Add((Control) this.btnCancel);
    this.Controls.Add((Control) this.btnOK);
    this.MinimumSize = new Size(660, 590);
    this.Name = nameof (SignDateConfigsForm);
    this.StartPosition = FormStartPosition.CenterScreen;
    this.Text = "Конфигуратор команды статистики";
    this.FormClosing += new FormClosingEventHandler(this.SignDateConfigsForm_FormClosing);
    this.FormClosed += new FormClosedEventHandler(this.ConfigsForm_FormClosed);
    this.Load += new EventHandler(this.ConfigsForm_Load);
    this.config.ResumeLayout(false);
    this.createdDateCommandSettings.ResumeLayout(false);
    this.optionsTab.ResumeLayout(false);
    this.usersTab.ResumeLayout(false);
    this.groupBox1.ResumeLayout(false);
    this.groupBox1.PerformLayout();
    this.generalSettings.ResumeLayout(false);
    this.tabControl2.ResumeLayout(false);
    this.objectsTypes.ResumeLayout(false);
    this.Filters.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
