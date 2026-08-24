// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.CommandObjectCreator
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Statistics.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Statistics;

public class CommandObjectCreator : Form
{
  public long ObjectID = -1;
  private int _objectTypeID;
  public bool OpenConfig;
  public CommandStatisticsTypesEnum CommandType;
  public UsersEnum UsersType;
  public string Caption;
  private IContainer components;
  private TextBox nameTb;
  private ComboBox collectMethod;
  private Label label1;
  private Label label3;
  private Button okBtn;
  private Button cancelBtn;
  private CheckBox configOpen;
  private GroupBox groupBox2;
  private RadioButton rbUser;
  private RadioButton rbUserGroup;
  private RadioButton rbDepartament;

  public CommandObjectCreator(int ObjectTypeID)
  {
    this.InitializeComponent();
    this._objectTypeID = ObjectTypeID;
  }

  private void CommandObjectCreator_Load(object sender, EventArgs e)
  {
    try
    {
      List<\u003C\u003Ef__AnonymousType0<string, Enum>> list = Enum.GetValues(typeof (CommandStatisticsTypesEnum)).Cast<Enum>().Select(value => new
      {
        Description = (Attribute.GetCustomAttribute((MemberInfo) value.GetType().GetField(value.ToString()), typeof (DescriptionAttribute)) as DescriptionAttribute).Description,
        value = value
      }).OrderBy(item => item.value).ToList();
      int index = list.FindIndex(x => x.value.Equals((object) CommandStatisticsTypesEnum.None));
      list.RemoveAt(index);
      this.collectMethod.DataSource = (object) list;
      this.collectMethod.DisplayMember = "Description";
      this.collectMethod.ValueMember = "value";
      this.SetUserGroupBoxEnable();
    }
    catch
    {
      this.collectMethod.DataSource = (object) Enum.GetValues(typeof (CommandStatisticsTypesEnum));
    }
  }

  private void cancelBtn_Click(object sender, EventArgs e) => this.Close();

  private void okBtn_Click(object sender, EventArgs e)
  {
    if (this.nameTb.Text.Length > 0)
    {
      this.CreateObject();
      this.OpenConfig = this.configOpen.Checked;
      this.Caption = this.nameTb.Text;
      this.CommandType = this.collectMethod.SelectedValue.ToString().ToEnum<CommandStatisticsTypesEnum>(CommandStatisticsTypesEnum.None);
      if (this.rbUser.Checked)
        this.UsersType = UsersEnum.User;
      else if (this.rbUserGroup.Checked)
        this.UsersType = UsersEnum.UserGroup;
      else if (this.rbDepartament.Checked)
        this.UsersType = UsersEnum.Department;
      this.DialogResult = DialogResult.OK;
    }
    else
    {
      int num = (int) MessageBox.Show("Поле 'Наименование' не может быть пустым.");
    }
  }

  private void CommandObjectCreator_FormClosing(object sender, FormClosingEventArgs e)
  {
    if (this.DialogResult == DialogResult.OK && this.DialogResult == DialogResult.Cancel)
      return;
    e.Cancel = false;
  }

  private void CreateObject()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObjectCollection objectCollection = sessionKeeper.Session.GetObjectCollection(this._objectTypeID);
      if (objectCollection == null)
        return;
      IDBTransactions customService = sessionKeeper.Session.GetCustomService(typeof (IDBTransactions)) as IDBTransactions;
      try
      {
        customService?.StartTransaction();
        IDBObject dbObject1 = objectCollection.Create();
        if (dbObject1 == null)
        {
          customService?.Rollback();
        }
        else
        {
          long objectId = dbObject1.ObjectID;
          AttributeValues[] valuesList = new AttributeValues[2];
          int attributeTypeId1 = MetaDataHelper.GetAttributeTypeID(StatisticsConst.CollectMethod);
          valuesList[0] = new AttributeValues(attributeTypeId1, (object) this.collectMethod.SelectedValue.ToString());
          int attributeTypeId2 = MetaDataHelper.GetAttributeTypeID("cad00020-306c-11d8-b4e9-00304f19f545");
          valuesList[1] = new AttributeValues(attributeTypeId2, (object) this.nameTb.Text);
          dbObject1.SetAttributesValues(valuesList);
          IDBObject dbObject2 = sessionKeeper.Session.GetObject(objectId);
          if (dbObject2 != null)
          {
            if (dbObject2.IsCreationMode)
              dbObject2.CommitCreation(true);
            objectId = dbObject2.ObjectID;
          }
          this.ObjectID = objectId;
          customService?.Commit();
        }
      }
      catch
      {
        customService?.Rollback();
        throw;
      }
    }
  }

  private void collectMethod_SelectedIndexChanged(object sender, EventArgs e)
  {
    this.SetUserGroupBoxEnable();
  }

  private void SetUserGroupBoxEnable()
  {
    switch (this.collectMethod.SelectedValue.ToString().ToEnum<CommandStatisticsTypesEnum>(CommandStatisticsTypesEnum.None))
    {
      case CommandStatisticsTypesEnum.CreatedDate:
      case CommandStatisticsTypesEnum.SignDate:
        this.groupBox2.Enabled = true;
        break;
      default:
        this.groupBox2.Enabled = false;
        break;
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
    this.nameTb = new TextBox();
    this.collectMethod = new ComboBox();
    this.label1 = new Label();
    this.label3 = new Label();
    this.okBtn = new Button();
    this.cancelBtn = new Button();
    this.configOpen = new CheckBox();
    this.groupBox2 = new GroupBox();
    this.rbUser = new RadioButton();
    this.rbUserGroup = new RadioButton();
    this.rbDepartament = new RadioButton();
    this.groupBox2.SuspendLayout();
    this.SuspendLayout();
    this.nameTb.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.nameTb.Location = new Point(108, 10);
    this.nameTb.Name = "nameTb";
    this.nameTb.Size = new Size(464, 20);
    this.nameTb.TabIndex = 0;
    this.collectMethod.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.collectMethod.DropDownStyle = ComboBoxStyle.DropDownList;
    this.collectMethod.FormattingEnabled = true;
    this.collectMethod.Location = new Point(157, 36);
    this.collectMethod.Name = "collectMethod";
    this.collectMethod.Size = new Size(415, 21);
    this.collectMethod.TabIndex = 2;
    this.collectMethod.SelectedIndexChanged += new EventHandler(this.collectMethod_SelectedIndexChanged);
    this.label1.AutoSize = true;
    this.label1.Location = new Point(12, 13);
    this.label1.Name = "label1";
    this.label1.Size = new Size(89, 13);
    this.label1.TabIndex = 2;
    this.label1.Text = "Наименование: ";
    this.label3.AutoSize = true;
    this.label3.Location = new Point(12, 39);
    this.label3.Name = "label3";
    this.label3.Size = new Size(138, 13);
    this.label3.TabIndex = 2;
    this.label3.Text = "Метод сбора статистики: ";
    this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.okBtn.Location = new Point(416, 106);
    this.okBtn.Name = "okBtn";
    this.okBtn.Size = new Size(75, 23);
    this.okBtn.TabIndex = 8;
    this.okBtn.Text = "ОК";
    this.okBtn.UseVisualStyleBackColor = true;
    this.okBtn.Click += new EventHandler(this.okBtn_Click);
    this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.cancelBtn.DialogResult = DialogResult.Cancel;
    this.cancelBtn.Location = new Point(497, 106);
    this.cancelBtn.Name = "cancelBtn";
    this.cancelBtn.Size = new Size(75, 23);
    this.cancelBtn.TabIndex = 9;
    this.cancelBtn.Text = "Отмена";
    this.cancelBtn.UseVisualStyleBackColor = true;
    this.cancelBtn.Click += new EventHandler(this.cancelBtn_Click);
    this.configOpen.AutoSize = true;
    this.configOpen.Checked = true;
    this.configOpen.CheckState = CheckState.Checked;
    this.configOpen.Location = new Point(15, 110);
    this.configOpen.Name = "configOpen";
    this.configOpen.Size = new Size(193, 17);
    this.configOpen.TabIndex = 7;
    this.configOpen.Text = "Открыть конфигуратор команды";
    this.configOpen.UseVisualStyleBackColor = true;
    this.groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.groupBox2.Controls.Add((Control) this.rbUser);
    this.groupBox2.Controls.Add((Control) this.rbUserGroup);
    this.groupBox2.Controls.Add((Control) this.rbDepartament);
    this.groupBox2.Location = new Point(12, 63 /*0x3F*/);
    this.groupBox2.Name = "groupBox2";
    this.groupBox2.Size = new Size(560, 37);
    this.groupBox2.TabIndex = 3;
    this.groupBox2.TabStop = false;
    this.groupBox2.Text = "Расчет статистики для:";
    this.rbUser.AutoSize = true;
    this.rbUser.Checked = true;
    this.rbUser.Location = new Point(6, 15);
    this.rbUser.Name = "rbUser";
    this.rbUser.Size = new Size(104, 17);
    this.rbUser.TabIndex = 4;
    this.rbUser.TabStop = true;
    this.rbUser.Text = "Пользователей";
    this.rbUser.UseVisualStyleBackColor = true;
    this.rbUserGroup.AutoSize = true;
    this.rbUserGroup.Location = new Point(110, 15);
    this.rbUserGroup.Name = "rbUserGroup";
    this.rbUserGroup.Size = new Size(134, 17);
    this.rbUserGroup.TabIndex = 5;
    this.rbUserGroup.Text = "Групп пользователей";
    this.rbUserGroup.UseVisualStyleBackColor = true;
    this.rbDepartament.AutoSize = true;
    this.rbDepartament.Location = new Point(254, 15);
    this.rbDepartament.Name = "rbDepartament";
    this.rbDepartament.Size = new Size(105, 17);
    this.rbDepartament.TabIndex = 6;
    this.rbDepartament.Text = "Подразделений";
    this.rbDepartament.UseVisualStyleBackColor = true;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.cancelBtn;
    this.ClientSize = new Size(584, 141);
    this.Controls.Add((Control) this.groupBox2);
    this.Controls.Add((Control) this.configOpen);
    this.Controls.Add((Control) this.cancelBtn);
    this.Controls.Add((Control) this.okBtn);
    this.Controls.Add((Control) this.label3);
    this.Controls.Add((Control) this.label1);
    this.Controls.Add((Control) this.collectMethod);
    this.Controls.Add((Control) this.nameTb);
    this.MaximumSize = new Size(600, 180);
    this.MinimumSize = new Size(16 /*0x10*/, 180);
    this.Name = nameof (CommandObjectCreator);
    this.StartPosition = FormStartPosition.CenterScreen;
    this.Text = "Создание объекта команды статистики";
    this.FormClosing += new FormClosingEventHandler(this.CommandObjectCreator_FormClosing);
    this.Load += new EventHandler(this.CommandObjectCreator_Load);
    this.groupBox2.ResumeLayout(false);
    this.groupBox2.PerformLayout();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
