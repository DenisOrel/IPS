// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.Configurations.TaskConfigsForm
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.DataFormats;
using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator;
using Intermech.Navigator.Controls;
using Intermech.Navigator.DBObjectTypes;
using Intermech.Navigator.Interfaces;
using Intermech.Statistics.Interfaces;
using Intermech.Statistics.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Statistics.Configurations;

public class TaskConfigsForm : Form, IStatisticSettingsForm
{
  private CommandSettings _settings = new CommandSettings();
  private CommandSettings _tempSettings = new CommandSettings();
  private List<SchemeFilter> _filterObjectsList = new List<SchemeFilter>();
  private bool _usingAsControl;
  private IContainer components;
  private Button btnCancel;
  private Button btnOK;
  private TabPage taskCommandSettings;
  private GroupBox groupBox1;
  private Label label3;
  private ComboBox comboBox1;
  private TabControl tabControl1;
  private TabControl tabControl2;
  private TabPage generalSettings;
  private TabControl tabControl3;
  private TabPage objectsTypes;
  private TabPage filterObjects;
  private Label delFilter;
  private Label addFilter;
  public CustomListBox filterListBox;
  private TabPage searchShemeRoot;
  private Label delRootObjectForFilter;
  private Label addRootObjectForFilter;
  public CustomListBox rootObjectsLB;
  private AnalyzeObjectTypeControl analyzeObjectTypeControl1;
  private Label labelEdit;
  private ToolTip toolTip1;

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

  public TaskConfigsForm(CommandSettings commandSettings = null)
  {
    this.InitializeComponent();
    this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
    this.InitForm(commandSettings);
    this.analyzeObjectTypeControl1.ItemsChanged += new AnalyzeObjectTypeControl.ModifyItems(this.AnalyzeObjectTypeItemsChanged);
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
    this.analyzeObjectTypeControl1.Init(commandSettings.AnalizedObjectsTypes, commandSettings.CommandType);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      this.filterListBox.BeginUpdate();
      this.filterListBox.Items.Clear();
      foreach (ListItem filterObject in commandSettings.FilterObjects)
      {
        if (sessionKeeper.Session.GetObject(filterObject.ObjID, false) != null)
          this.filterListBox.Items.Add((object) filterObject);
      }
      this.filterListBox.EndUpdate();
      this._filterObjectsList = commandSettings.SchemeFilters;
      this.rootObjectsLB.BeginUpdate();
      this.rootObjectsLB.Items.Clear();
      foreach (SchemeFilter filterObjects in this._filterObjectsList)
      {
        if (sessionKeeper.Session.GetObject(filterObjects.FilterObject.ObjID, false) != null)
        {
          foreach (ListItem rootObject in filterObjects.RootObjects)
          {
            if (sessionKeeper.Session.GetObject(rootObject.ObjID, false) != null)
              this.rootObjectsLB.Items.Add((object) new FilterObject()
              {
                Filter = filterObjects.FilterObject,
                IsSheme = true,
                RootObject = rootObject
              });
          }
        }
      }
      this.rootObjectsLB.EndUpdate();
    }
    try
    {
      this.comboBox1.DataSource = (object) Enum.GetValues(typeof (CollectPeriodsEnum)).Cast<Enum>().Select(value => new
      {
        Description = (Attribute.GetCustomAttribute((MemberInfo) value.GetType().GetField(value.ToString()), typeof (DescriptionAttribute)) as DescriptionAttribute).Description,
        value = value
      }).OrderBy(item => item.value).ToList();
      this.comboBox1.DisplayMember = "Description";
      this.comboBox1.ValueMember = "value";
    }
    catch
    {
      this.comboBox1.DataSource = (object) Enum.GetValues(typeof (CollectPeriodsEnum));
    }
    if (this.comboBox1.Items.Count <= 0)
      return;
    this.comboBox1.SelectedIndex = commandSettings.CollectPeriodIndex;
  }

  private void InitFormByDefault()
  {
    try
    {
      this.comboBox1.DataSource = (object) Enum.GetValues(typeof (CollectPeriodsEnum)).Cast<Enum>().Select(value => new
      {
        Description = (Attribute.GetCustomAttribute((MemberInfo) value.GetType().GetField(value.ToString()), typeof (DescriptionAttribute)) as DescriptionAttribute).Description,
        value = value
      }).OrderBy(item => item.value).ToList();
      this.comboBox1.DisplayMember = "Description";
      this.comboBox1.ValueMember = "value";
    }
    catch
    {
      this.comboBox1.DataSource = (object) Enum.GetValues(typeof (CollectPeriodsEnum));
    }
    if (this.comboBox1.Items.Count <= 0)
      return;
    this.comboBox1.SelectedIndex = 1;
  }

  private void addFilter_Click(object sender, EventArgs e)
  {
    object[] objArray = Intermech.Navigator.SelectionWindow.Select("Выберите фильтрующие объекты", "", (IDescriptor) new Intermech.Navigator.CustomNode.Descriptor("Фильтрующие объекты", new DescriptorCollection()
    {
      (IDescriptor) new Intermech.Navigator.DBObjectTypes.Descriptor(MetaDataHelper.GetObjectTypeID("cad00156-306c-11d8-b4e9-00304f19f545")),
      (IDescriptor) new Intermech.Navigator.DBObjectTypes.Descriptor(MetaDataHelper.GetObjectTypeID("cad00129-306c-11d8-b4e9-00304f19f545"))
    }), typeof (IDBObjectID), SelectionOptions.SelectObjects);
    if (objArray != null && objArray.Length >= 1)
    {
      foreach (object obj in objArray)
      {
        IDBObjectID objType = obj as IDBObjectID;
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          int typeId = sessionKeeper.Session.GetObject(objType.Value).TypeID;
          if (this.filterListBox.Items.IndexOfFirst((Predicate<object>) (x => ((ListItem) x).ID == objType.ID)) == -1)
            this.filterListBox.Items.Add((object) new ListItem(objType.Caption, objType.ID, objType.Value));
        }
      }
    }
    this.Modify();
  }

  private void delFilter_Click(object sender, EventArgs e)
  {
    for (int i = 0; i < this.filterListBox.SelectedItems.Count; i++)
    {
      List<SchemeFilter> list = this._filterObjectsList.Where<SchemeFilter>((Func<SchemeFilter, bool>) (x => x.FilterObject.Equals(this.filterListBox.SelectedItems[i]))).ToList<SchemeFilter>();
      if (list.Count > 0)
      {
        for (int index = 0; index < this.rootObjectsLB.Items.Count; ++index)
        {
          FilterObject filterObject = this.rootObjectsLB.Items[index] as FilterObject;
          if (filterObject.Filter.Equals(this.filterListBox.SelectedItems[i]))
          {
            this.rootObjectsLB.Items.Remove((object) filterObject);
            --index;
          }
        }
        this._filterObjectsList.RemoveRange<SchemeFilter>((IEnumerable<SchemeFilter>) list);
      }
      this.filterListBox.Items.Remove(this.filterListBox.SelectedItems[i]);
      i--;
    }
    this.Modify();
  }

  private void delRootObjectForFilter_Click(object sender, EventArgs e)
  {
    for (int index = 0; index < this.rootObjectsLB.SelectedItems.Count; index = index - 1 + 1)
    {
      FilterObject rootItem = this.rootObjectsLB.SelectedItems[index] as FilterObject;
      List<SchemeFilter> list = this._filterObjectsList.Where<SchemeFilter>((Func<SchemeFilter, bool>) (x => x.FilterObject.Equals(rootItem.Filter))).ToList<SchemeFilter>();
      if (list.Count > 0)
      {
        ListItem listItem = list[0].RootObjects.FirstOrDefault<ListItem>((Func<ListItem, bool>) (x => x.ID.Equals(rootItem.RootObject.ID) && x.ObjID.Equals(rootItem.RootObject.ObjID)));
        list[0].RootObjects.Remove(listItem);
      }
      this.rootObjectsLB.Items.Remove(this.rootObjectsLB.SelectedItems[index]);
    }
    this.Modify();
  }

  private void addRootObjectForFilter_Click(object sender, EventArgs e)
  {
    SelectSchemeObject schemeForm = new SelectSchemeObject();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      foreach (object obj in this.filterListBox.Items)
      {
        ListItem listItem = obj as ListItem;
        IDBObject dbObject = sessionKeeper.Session.GetObject(listItem.ObjID);
        if (StatisticsConst.AllSchemeTypes.Contains(dbObject.TypeID))
          schemeForm.listBox1.Items.Add((object) listItem);
      }
    }
    if (schemeForm.listBox1.Items.Count == 0)
    {
      int num = (int) MessageBox.Show("Для выбора корневого объекта необходимо сначала добавить схему поиска объектов на вкладке Фильтрующие объекты.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      schemeForm.Dispose();
    }
    else
    {
      object[] rootObj = Intermech.Navigator.SelectionWindow.Select("Выберите корневые объекты", "", (IDescriptor) new AllObjectTypesDescriptor(), typeof (IDBObjectID), SelectionOptions.SelectObjects);
      if (rootObj != null && rootObj.Length != 0)
      {
        if (schemeForm.listBox1.Items.Count > 1)
        {
          schemeForm.listBox1.SelectedIndex = 0;
          int num = (int) schemeForm.ShowDialog();
          if (this._filterObjectsList.Contains<SchemeFilter>((Predicate<SchemeFilter>) (x => x.FilterObject.Equals(schemeForm.listBox1.SelectedItem))))
          {
            int index = this._filterObjectsList.IndexOf(this._filterObjectsList.FirstOrDefault<SchemeFilter>((Func<SchemeFilter, bool>) (x => x.FilterObject.Equals(schemeForm.listBox1.SelectedItem))));
            if (this._filterObjectsList[index].RootObjects != null)
            {
              this._filterObjectsList[index].RootObjects = this.CreateRootAndAddIcon(rootObj, this._filterObjectsList[index].RootObjects);
            }
            else
            {
              this._filterObjectsList[index].RootObjects = new List<ListItem>();
              this._filterObjectsList[index].RootObjects = this.CreateRootAndAddIcon(rootObj, this._filterObjectsList[index].RootObjects);
            }
          }
          else
          {
            SchemeFilter schemeFilter = new SchemeFilter()
            {
              FilterObject = schemeForm.listBox1.SelectedItem as ListItem,
              IsSheme = true,
              RootObjects = new List<ListItem>()
            };
            schemeFilter.RootObjects = this.CreateRootAndAddIcon(rootObj, schemeFilter.RootObjects);
            this._filterObjectsList.Add(schemeFilter);
          }
          FilterObject objects = new FilterObject()
          {
            Filter = schemeForm.listBox1.SelectedItem as ListItem,
            IsSheme = true
          };
          this.AddItemToList(rootObj, objects);
        }
        else
        {
          if (this._filterObjectsList.Contains<SchemeFilter>((Predicate<SchemeFilter>) (x => x.FilterObject.Equals(schemeForm.listBox1.Items[0]))))
          {
            int index = this._filterObjectsList.IndexOf(this._filterObjectsList.FirstOrDefault<SchemeFilter>((Func<SchemeFilter, bool>) (x => x.FilterObject.Equals(schemeForm.listBox1.Items[0]))));
            if (this._filterObjectsList[index].RootObjects != null)
            {
              this._filterObjectsList[index].RootObjects = this.CreateRootAndAddIcon(rootObj, this._filterObjectsList[index].RootObjects);
            }
            else
            {
              this._filterObjectsList[index].RootObjects = new List<ListItem>();
              this._filterObjectsList[index].RootObjects = this.CreateRootAndAddIcon(rootObj, this._filterObjectsList[index].RootObjects);
            }
          }
          else
          {
            SchemeFilter schemeFilter = new SchemeFilter()
            {
              FilterObject = schemeForm.listBox1.Items[0] as ListItem,
              IsSheme = true,
              RootObjects = new List<ListItem>()
            };
            schemeFilter.RootObjects = this.CreateRootAndAddIcon(rootObj, schemeFilter.RootObjects);
            this._filterObjectsList.Add(schemeFilter);
          }
          FilterObject objects = new FilterObject()
          {
            Filter = schemeForm.listBox1.Items[0] as ListItem,
            IsSheme = true
          };
          this.AddItemToList(rootObj, objects);
        }
      }
      schemeForm.Dispose();
      this.Modify();
    }
  }

  private void AddItemToList(object[] rootObj, FilterObject objects)
  {
    foreach (object obj in rootObj)
    {
      FilterObject tempObj = new FilterObject()
      {
        Filter = objects.Filter,
        IsSheme = true
      };
      IDBObjectID dbObjectId = obj as IDBObjectID;
      tempObj.RootObject = new ListItem(dbObjectId.Caption, dbObjectId.ID, dbObjectId.Value);
      if (this.rootObjectsLB.Items.IndexOfFirst((Predicate<object>) (x => ((FilterObject) x).Filter.ID == tempObj.Filter.ID && ((FilterObject) x).RootObject.ID == tempObj.RootObject.ID)) == -1)
        this.rootObjectsLB.Items.Add((object) tempObj);
    }
  }

  private List<ListItem> CreateRootAndAddIcon(object[] rootObj, List<ListItem> rootObjects)
  {
    foreach (object obj in rootObj)
    {
      IDBObjectID rObj = obj as IDBObjectID;
      if (rootObjects.IndexOfFirst<ListItem>((Predicate<ListItem>) (x => x.ID == rObj.ID)) == -1)
        rootObjects.Add(new ListItem(rObj.Caption, rObj.ID, rObj.Value));
    }
    return rootObjects;
  }

  private void btnOK_Click(object sender, EventArgs e)
  {
    CommandSettings commandSettings = new CommandSettings()
    {
      ObjectID = this._settings.ObjectID,
      FilterObjects = new List<ListItem>(),
      AnalizedObjectsTypes = new List<ObjectTypesListItem>(),
      StatisticsObjectType = StatisticsObjectsTypeEnum.TaskStatisticsObject,
      CollectPeriod = (CollectPeriodsEnum) this.comboBox1.SelectedIndex,
      CollectPeriodIndex = this.comboBox1.SelectedIndex,
      CommandType = CommandStatisticsTypesEnum.None,
      LCStep = this._tempSettings.LCStep,
      LCLevel = this._tempSettings.LCLevel,
      AttrData = this._tempSettings.AttrData,
      SchemeFilters = this._filterObjectsList,
      ListUsers = this._tempSettings.ListUsers,
      StatisticsUsersType = this._tempSettings.StatisticsUsersType,
      EndDateTime = this._tempSettings.EndDateTime,
      StartDateTime = this._tempSettings.StartDateTime
    };
    foreach (object obj in this.filterListBox.Items)
    {
      ListItem listItem = obj as ListItem;
      commandSettings.FilterObjects.Add(listItem);
    }
    foreach (ObjectTypesListItem typesListItem in this.analyzeObjectTypeControl1.TypesListItems)
      commandSettings.AnalizedObjectsTypes.Add(typesListItem);
    this._settings = commandSettings;
    if (!this._usingAsControl)
      this.Close();
    this.btnOK.Enabled = false;
    this.btnCancel.Enabled = false;
    this.Applied();
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

  private void ConfigsForm_FormClosed(object sender, FormClosedEventArgs e)
  {
    Intermech.Client.Core.FormStorage.SaveLayout((Control) this);
  }

  private void ConfigsForm_Load(object sender, EventArgs e)
  {
    Intermech.Client.Core.FormStorage.LoadLayout((Control) this);
  }

  private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) => this.Modify();

  private void AnalyzeObjectTypeItemsChanged(bool message) => this.Modify();

  private void filterListBox_SelectedIndexChanged(object sender, EventArgs e)
  {
    this.labelEdit.Enabled = this.filterListBox.SelectedItems.Count == 1;
    this.delFilter.Enabled = this.filterListBox.SelectedItems.Count > 0;
  }

  private void labelEdit_Click(object sender, EventArgs e)
  {
    if (this.filterListBox.SelectedItems.Count != 1 || !(this.filterListBox.SelectedItems[0] is ListItem selectedItem))
      return;
    int num = (int) PropertiesWindow.Execute(string.Empty, string.Empty, selectedItem.ObjID, "ObjectProperties");
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
    this.taskCommandSettings = new TabPage();
    this.tabControl2 = new TabControl();
    this.generalSettings = new TabPage();
    this.tabControl3 = new TabControl();
    this.objectsTypes = new TabPage();
    this.analyzeObjectTypeControl1 = new AnalyzeObjectTypeControl();
    this.filterObjects = new TabPage();
    this.labelEdit = new Label();
    this.delFilter = new Label();
    this.addFilter = new Label();
    this.filterListBox = new CustomListBox();
    this.searchShemeRoot = new TabPage();
    this.delRootObjectForFilter = new Label();
    this.addRootObjectForFilter = new Label();
    this.rootObjectsLB = new CustomListBox();
    this.groupBox1 = new GroupBox();
    this.label3 = new Label();
    this.comboBox1 = new ComboBox();
    this.tabControl1 = new TabControl();
    this.toolTip1 = new ToolTip(this.components);
    this.taskCommandSettings.SuspendLayout();
    this.tabControl2.SuspendLayout();
    this.generalSettings.SuspendLayout();
    this.tabControl3.SuspendLayout();
    this.objectsTypes.SuspendLayout();
    this.filterObjects.SuspendLayout();
    this.searchShemeRoot.SuspendLayout();
    this.groupBox1.SuspendLayout();
    this.tabControl1.SuspendLayout();
    this.SuspendLayout();
    this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.Location = new Point(458, 535);
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.Size = new Size(89, 23);
    this.btnCancel.TabIndex = 9;
    this.btnCancel.Text = "Отмена";
    this.btnCancel.UseVisualStyleBackColor = true;
    this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
    this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnOK.DialogResult = DialogResult.OK;
    this.btnOK.Location = new Point(363, 535);
    this.btnOK.Name = "btnOK";
    this.btnOK.Size = new Size(89, 23);
    this.btnOK.TabIndex = 10;
    this.btnOK.Text = "ОК";
    this.btnOK.UseVisualStyleBackColor = true;
    this.btnOK.Click += new EventHandler(this.btnOK_Click);
    this.taskCommandSettings.Controls.Add((Control) this.tabControl2);
    this.taskCommandSettings.Controls.Add((Control) this.groupBox1);
    this.taskCommandSettings.Location = new Point(4, 22);
    this.taskCommandSettings.Name = "taskCommandSettings";
    this.taskCommandSettings.Padding = new Padding(3);
    this.taskCommandSettings.Size = new Size(529, 491);
    this.taskCommandSettings.TabIndex = 0;
    this.taskCommandSettings.Text = "Настройки задачи сбора статистики";
    this.taskCommandSettings.UseVisualStyleBackColor = true;
    this.tabControl2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.tabControl2.Controls.Add((Control) this.generalSettings);
    this.tabControl2.Location = new Point(9, 54);
    this.tabControl2.Name = "tabControl2";
    this.tabControl2.SelectedIndex = 0;
    this.tabControl2.Size = new Size(507, 431);
    this.tabControl2.TabIndex = 6;
    this.generalSettings.Controls.Add((Control) this.tabControl3);
    this.generalSettings.Location = new Point(4, 22);
    this.generalSettings.Name = "generalSettings";
    this.generalSettings.Padding = new Padding(3);
    this.generalSettings.Size = new Size(499, 405);
    this.generalSettings.TabIndex = 3;
    this.generalSettings.Text = "Дополнительные настройки";
    this.generalSettings.UseVisualStyleBackColor = true;
    this.tabControl3.Controls.Add((Control) this.objectsTypes);
    this.tabControl3.Controls.Add((Control) this.filterObjects);
    this.tabControl3.Controls.Add((Control) this.searchShemeRoot);
    this.tabControl3.Dock = DockStyle.Fill;
    this.tabControl3.Location = new Point(3, 3);
    this.tabControl3.Name = "tabControl3";
    this.tabControl3.SelectedIndex = 0;
    this.tabControl3.Size = new Size(493, 399);
    this.tabControl3.TabIndex = 0;
    this.objectsTypes.Controls.Add((Control) this.analyzeObjectTypeControl1);
    this.objectsTypes.Location = new Point(4, 22);
    this.objectsTypes.Name = "objectsTypes";
    this.objectsTypes.Padding = new Padding(3);
    this.objectsTypes.Size = new Size(485, 373);
    this.objectsTypes.TabIndex = 0;
    this.objectsTypes.Text = "Типы анализируемых объектов";
    this.objectsTypes.UseVisualStyleBackColor = true;
    this.analyzeObjectTypeControl1.AutoScroll = true;
    this.analyzeObjectTypeControl1.Dock = DockStyle.Fill;
    this.analyzeObjectTypeControl1.Location = new Point(3, 3);
    this.analyzeObjectTypeControl1.Name = "analyzeObjectTypeControl1";
    this.analyzeObjectTypeControl1.Size = new Size(479, 367);
    this.analyzeObjectTypeControl1.TabIndex = 0;
    this.filterObjects.Controls.Add((Control) this.labelEdit);
    this.filterObjects.Controls.Add((Control) this.delFilter);
    this.filterObjects.Controls.Add((Control) this.addFilter);
    this.filterObjects.Controls.Add((Control) this.filterListBox);
    this.filterObjects.Location = new Point(4, 22);
    this.filterObjects.Name = "filterObjects";
    this.filterObjects.Padding = new Padding(3);
    this.filterObjects.Size = new Size(485, 373);
    this.filterObjects.TabIndex = 1;
    this.filterObjects.Text = "Фильтрующие объекты";
    this.filterObjects.UseVisualStyleBackColor = true;
    this.labelEdit.Enabled = false;
    this.labelEdit.Image = (Image) Resources.EditStandart;
    this.labelEdit.Location = new Point(50, 8);
    this.labelEdit.Name = "labelEdit";
    this.labelEdit.Size = new Size(16 /*0x10*/, 16 /*0x10*/);
    this.labelEdit.TabIndex = 9;
    this.labelEdit.Text = "    ";
    this.toolTip1.SetToolTip((Control) this.labelEdit, "Редактировать фильтр");
    this.labelEdit.Click += new EventHandler(this.labelEdit_Click);
    this.delFilter.Enabled = false;
    this.delFilter.Image = (Image) Resources.minus;
    this.delFilter.Location = new Point(28, 8);
    this.delFilter.Name = "delFilter";
    this.delFilter.Size = new Size(16 /*0x10*/, 16 /*0x10*/);
    this.delFilter.TabIndex = 5;
    this.delFilter.Text = "    ";
    this.toolTip1.SetToolTip((Control) this.delFilter, "Удалить фильтр");
    this.delFilter.Click += new EventHandler(this.delFilter_Click);
    this.addFilter.Image = (Image) Resources.add;
    this.addFilter.Location = new Point(6, 8);
    this.addFilter.Name = "addFilter";
    this.addFilter.Size = new Size(16 /*0x10*/, 16 /*0x10*/);
    this.addFilter.TabIndex = 6;
    this.addFilter.Text = "        ";
    this.toolTip1.SetToolTip((Control) this.addFilter, "Добавить фильтр");
    this.addFilter.Click += new EventHandler(this.addFilter_Click);
    this.filterListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.filterListBox.DrawMode = DrawMode.OwnerDrawFixed;
    this.filterListBox.FormattingEnabled = true;
    this.filterListBox.ItemHeight = 21;
    this.filterListBox.Location = new Point(6, 27);
    this.filterListBox.Name = "filterListBox";
    this.filterListBox.SelectionMode = SelectionMode.MultiExtended;
    this.filterListBox.Size = new Size(473, 340);
    this.filterListBox.TabIndex = 4;
    this.filterListBox.SelectedIndexChanged += new EventHandler(this.filterListBox_SelectedIndexChanged);
    this.searchShemeRoot.Controls.Add((Control) this.delRootObjectForFilter);
    this.searchShemeRoot.Controls.Add((Control) this.addRootObjectForFilter);
    this.searchShemeRoot.Controls.Add((Control) this.rootObjectsLB);
    this.searchShemeRoot.Location = new Point(4, 22);
    this.searchShemeRoot.Name = "searchShemeRoot";
    this.searchShemeRoot.Size = new Size(485, 373);
    this.searchShemeRoot.TabIndex = 3;
    this.searchShemeRoot.Text = "Корневые объекты для схемы поиска данных";
    this.searchShemeRoot.UseVisualStyleBackColor = true;
    this.delRootObjectForFilter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.delRootObjectForFilter.Image = (Image) Resources.minus;
    this.delRootObjectForFilter.Location = new Point(466, 30);
    this.delRootObjectForFilter.Name = "delRootObjectForFilter";
    this.delRootObjectForFilter.Size = new Size(16 /*0x10*/, 16 /*0x10*/);
    this.delRootObjectForFilter.TabIndex = 7;
    this.delRootObjectForFilter.Text = "    ";
    this.delRootObjectForFilter.Click += new EventHandler(this.delRootObjectForFilter_Click);
    this.addRootObjectForFilter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.addRootObjectForFilter.Image = (Image) Resources.add;
    this.addRootObjectForFilter.Location = new Point(466, 4);
    this.addRootObjectForFilter.Name = "addRootObjectForFilter";
    this.addRootObjectForFilter.Size = new Size(16 /*0x10*/, 16 /*0x10*/);
    this.addRootObjectForFilter.TabIndex = 8;
    this.addRootObjectForFilter.Text = "        ";
    this.addRootObjectForFilter.Click += new EventHandler(this.addRootObjectForFilter_Click);
    this.rootObjectsLB.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.rootObjectsLB.DrawMode = DrawMode.OwnerDrawFixed;
    this.rootObjectsLB.FormattingEnabled = true;
    this.rootObjectsLB.HorizontalScrollbar = true;
    this.rootObjectsLB.ItemHeight = 21;
    this.rootObjectsLB.Location = new Point(4, 4);
    this.rootObjectsLB.Name = "rootObjectsLB";
    this.rootObjectsLB.SelectionMode = SelectionMode.MultiExtended;
    this.rootObjectsLB.Size = new Size(456, 361);
    this.rootObjectsLB.TabIndex = 0;
    this.groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.groupBox1.Controls.Add((Control) this.label3);
    this.groupBox1.Controls.Add((Control) this.comboBox1);
    this.groupBox1.Location = new Point(9, 6);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(513, 41);
    this.groupBox1.TabIndex = 5;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "Данные для графика";
    this.label3.AutoSize = true;
    this.label3.Location = new Point(3, 16 /*0x10*/);
    this.label3.Name = "label3";
    this.label3.Size = new Size(150, 13);
    this.label3.TabIndex = 4;
    this.label3.Text = "Период показа статистики: ";
    this.comboBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
    this.comboBox1.FormattingEnabled = true;
    this.comboBox1.Location = new Point(173, 13);
    this.comboBox1.Name = "comboBox1";
    this.comboBox1.Size = new Size(334, 21);
    this.comboBox1.TabIndex = 3;
    this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
    this.tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.tabControl1.Controls.Add((Control) this.taskCommandSettings);
    this.tabControl1.Location = new Point(12, 12);
    this.tabControl1.Name = "tabControl1";
    this.tabControl1.SelectedIndex = 0;
    this.tabControl1.Size = new Size(537, 517);
    this.tabControl1.TabIndex = 11;
    this.AcceptButton = (IButtonControl) this.btnOK;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.btnCancel;
    this.ClientSize = new Size(558, 570);
    this.Controls.Add((Control) this.tabControl1);
    this.Controls.Add((Control) this.btnCancel);
    this.Controls.Add((Control) this.btnOK);
    this.MinimumSize = new Size(450, 440);
    this.Name = nameof (TaskConfigsForm);
    this.StartPosition = FormStartPosition.CenterScreen;
    this.Text = "Конфигуратор задачи статистики";
    this.FormClosed += new FormClosedEventHandler(this.ConfigsForm_FormClosed);
    this.Load += new EventHandler(this.ConfigsForm_Load);
    this.taskCommandSettings.ResumeLayout(false);
    this.tabControl2.ResumeLayout(false);
    this.generalSettings.ResumeLayout(false);
    this.tabControl3.ResumeLayout(false);
    this.objectsTypes.ResumeLayout(false);
    this.filterObjects.ResumeLayout(false);
    this.searchShemeRoot.ResumeLayout(false);
    this.groupBox1.ResumeLayout(false);
    this.groupBox1.PerformLayout();
    this.tabControl1.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
