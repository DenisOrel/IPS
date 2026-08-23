// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Windows.SignConditionsEditor
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using DevExpress.IM.Utils;
using DevExpress.IM.XtraEditors;
using DevExpress.IM.XtraEditors.Controls;
using Intermech.Client.Core;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using Intermech.Navigator;
using Intermech.Navigator.Interfaces;
using Intermech.Security;
using Intermech.Signs.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Signs.Windows;

internal class SignConditionsEditor : Form
{
  private List<IAttributeCondition> conditionWrapper = new List<IAttributeCondition>();
  private IContainer components;
  private GroupBox groupBox1;
  private Button bUserChange;
  private TextBox tbUser;
  private System.Windows.Forms.ComboBox cbGraph;
  private Label label4;
  private System.Windows.Forms.ComboBox cbRanks;
  private Label label3;
  private GroupBox groupBox3;
  private Label label2;
  private Label label1;
  private Label label7;
  private TextBox tbResolution;
  private Label label6;
  private Button bIOUserChange;
  private TextBox tbIOUser;
  private Label label5;
  private Button bOK;
  private Button bCancel;
  private CheckBox cbEndDateCurrent;
  private CheckBox cbStartDateCurrent;
  private DateEdit dtpEndDate;
  private DateEdit dtpStartDate;
  private Panel panel1;
  private CheckBox cbCurrentUser;

  public SignConditionsEditor()
  {
    this.InitializeComponent();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      this.ReloadRanks(sessionKeeper.Session, 0L);
      this.ReloadGraphs(sessionKeeper.Session, string.Empty);
    }
    this.conditionWrapper.AddRange((IEnumerable<IAttributeCondition>) new IAttributeCondition[6]
    {
      (IAttributeCondition) new DateOfSignatureCondition(this.dtpStartDate, this.cbStartDateCurrent, this.dtpEndDate, this.cbEndDateCurrent),
      (IAttributeCondition) new RankCondition(this.cbRanks),
      (IAttributeCondition) new GraphCondition(this.cbGraph),
      (IAttributeCondition) new UserCondition(this.tbUser, this.cbCurrentUser),
      (IAttributeCondition) new IOUserCondition(this.tbIOUser),
      (IAttributeCondition) new ResolutionCondition(this.tbResolution)
    });
  }

  public static bool IsOwnCondition(ConditionStructure cs)
  {
    return DateOfSignatureCondition.IsOwnCondition(cs) || RankCondition.IsOwnCondition(cs) || GraphCondition.IsOwnCondition(cs) || UserCondition.IsOwnCondition(cs) || IOUserCondition.IsOwnCondition(cs) || ResolutionCondition.IsOwnCondition(cs);
  }

  public ConditionStructure Condition
  {
    get
    {
      List<ConditionStructure> conditionStructureList = new List<ConditionStructure>();
      for (int index = 0; index < this.conditionWrapper.Count; ++index)
      {
        ConditionStructure conditionStricture = this.conditionWrapper[index].GetConditionStricture();
        if (conditionStricture.Attribute != null)
          conditionStructureList.Add(conditionStricture);
      }
      ConditionStructure condition = new ConditionStructure((string) null, RelationalOperators.ConsistFromType, (object) SignsHolder.SignObjectTypeID, LogicalOperators.AND, 0, false);
      if (conditionStructureList.Count > 0)
        condition.NestedConditions = conditionStructureList.ToArray();
      return condition;
    }
    set
    {
      bool signed = true;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        for (int index = 0; index < this.conditionWrapper.Count; ++index)
          this.conditionWrapper[index].SetConditionStructure(sessionKeeper.Session, value.NestedConditions, ref signed);
      }
    }
  }

  private void ReloadGraphs(IUserSession session, string selectedID)
  {
    this.cbGraph.Items.Clear();
    DataTable possibleValues = session.GetAttributeType(SignsHolder.GraphAttrTypeID).GetPossibleValues();
    int num = -1;
    string empty = string.Empty;
    DataRow[] dataRowArray = possibleValues.Select(empty, "F_DESCRIPTION");
    for (int index = 0; index < dataRowArray.Length; ++index)
    {
      string id = Convert.ToString(dataRowArray[index]["F_STRING_VALUE"]);
      if (selectedID != string.Empty && id.Equals(selectedID))
        num = index;
      this.cbGraph.Items.Add((object) new SignConditionsEditor.ComboBoxValue((object) id, Convert.ToString(dataRowArray[index]["F_DESCRIPTION"])));
    }
    this.cbGraph.Items.Add((object) new SignConditionsEditor.ComboBoxValue((object) string.Empty, string.Empty));
    if (num == -1)
      this.cbGraph.SelectedIndex = this.cbGraph.Items.Count - 1;
    else
      this.cbGraph.SelectedIndex = num;
  }

  private void ReloadRanks(IUserSession session, long selectedID)
  {
    this.cbRanks.Items.Clear();
    DataTable dataTable = session.ObjectsSelect(new Guid("cad00147-306c-11d8-b4e9-00304f19f545"), new DBRecordSetParams((ConditionStructure[]) null, new object[2]
    {
      (object) -2,
      (object) -50
    }, new object[1]{ (object) -50 }, new SortOrders[1]
    {
      SortOrders.ASC
    }));
    int num = -1;
    for (int index = 0; index < dataTable.Rows.Count; ++index)
    {
      long int64 = Convert.ToInt64(dataTable.Rows[index][0]);
      if (selectedID != 0L && int64 == selectedID)
        num = index;
      this.cbRanks.Items.Add((object) new SignConditionsEditor.ComboBoxValue((object) int64, Convert.ToString(dataTable.Rows[index][1])));
    }
    this.cbRanks.Items.Add((object) new SignConditionsEditor.ComboBoxValue((object) 0L, string.Empty));
    if (num == -1)
      this.cbRanks.SelectedIndex = this.cbRanks.Items.Count - 1;
    else
      this.cbRanks.SelectedIndex = num;
  }

  private void UserChange_Click(object sender, EventArgs e)
  {
    IDBTypedObjectID dbTypedObjectId = this.SelectUser("Выберите пользователя");
    if (dbTypedObjectId == null)
      return;
    this.tbUser.Text = dbTypedObjectId.Caption;
    this.tbUser.Tag = (object) dbTypedObjectId.ObjectID;
  }

  private void IOUserChange_Click(object sender, EventArgs e)
  {
    IDBTypedObjectID dbTypedObjectId = this.SelectUser("Выберите исполняющего обязанности");
    if (dbTypedObjectId == null)
      return;
    this.tbIOUser.Text = dbTypedObjectId.Caption;
    this.tbIOUser.Tag = (object) dbTypedObjectId.ObjectID;
  }

  private IDBTypedObjectID SelectUser(string text)
  {
    return !(SelectionWindow.Select(text, (IDescriptor) new UsersGroupsDescriptor(), typeof (IDBTypedObjectID), SelectionOptions.SelectObjects) is IDBTypedObjectID[] dbTypedObjectIdArray) || dbTypedObjectIdArray.Length != 1 ? (IDBTypedObjectID) null : dbTypedObjectIdArray[0];
  }

  private void SignConditionsEditor_FormClosing(object sender, FormClosingEventArgs e)
  {
    FormStorage.SaveLayout((Control) this);
  }

  private void SignConditionsEditor_Shown(object sender, EventArgs e)
  {
    FormStorage.LoadLayout((Control) this);
  }

  private void StartDateCurrent_CheckedChanged(object sender, EventArgs e)
  {
    this.dtpStartDate.Enabled = !this.cbStartDateCurrent.Checked;
  }

  private void EndDateCurrent_CheckedChanged(object sender, EventArgs e)
  {
    this.dtpEndDate.Enabled = !this.cbEndDateCurrent.Checked;
  }

  private void User_KeyDown(object sender, KeyEventArgs e)
  {
    if (e.KeyCode != Keys.Delete)
      return;
    ((Control) sender).Text = string.Empty;
    ((Control) sender).Tag = (object) 0L;
  }

  private void CurrentUser_CheckedChanged(object sender, EventArgs e)
  {
    this.tbUser.Enabled = this.bUserChange.Enabled = !this.cbCurrentUser.Checked;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.groupBox1 = new GroupBox();
    this.cbCurrentUser = new CheckBox();
    this.label7 = new Label();
    this.tbResolution = new TextBox();
    this.label6 = new Label();
    this.bIOUserChange = new Button();
    this.tbIOUser = new TextBox();
    this.label5 = new Label();
    this.bUserChange = new Button();
    this.tbUser = new TextBox();
    this.cbGraph = new System.Windows.Forms.ComboBox();
    this.label4 = new Label();
    this.cbRanks = new System.Windows.Forms.ComboBox();
    this.label3 = new Label();
    this.groupBox3 = new GroupBox();
    this.dtpEndDate = new DateEdit();
    this.dtpStartDate = new DateEdit();
    this.cbEndDateCurrent = new CheckBox();
    this.cbStartDateCurrent = new CheckBox();
    this.label2 = new Label();
    this.label1 = new Label();
    this.bOK = new Button();
    this.bCancel = new Button();
    this.panel1 = new Panel();
    this.groupBox1.SuspendLayout();
    this.groupBox3.SuspendLayout();
    this.dtpEndDate.Properties.BeginInit();
    this.dtpStartDate.Properties.BeginInit();
    this.panel1.SuspendLayout();
    this.SuspendLayout();
    this.groupBox1.Controls.Add((Control) this.cbCurrentUser);
    this.groupBox1.Controls.Add((Control) this.label7);
    this.groupBox1.Controls.Add((Control) this.tbResolution);
    this.groupBox1.Controls.Add((Control) this.label6);
    this.groupBox1.Controls.Add((Control) this.bIOUserChange);
    this.groupBox1.Controls.Add((Control) this.tbIOUser);
    this.groupBox1.Controls.Add((Control) this.label5);
    this.groupBox1.Controls.Add((Control) this.bUserChange);
    this.groupBox1.Controls.Add((Control) this.tbUser);
    this.groupBox1.Controls.Add((Control) this.cbGraph);
    this.groupBox1.Controls.Add((Control) this.label4);
    this.groupBox1.Controls.Add((Control) this.cbRanks);
    this.groupBox1.Controls.Add((Control) this.label3);
    this.groupBox1.Controls.Add((Control) this.groupBox3);
    this.groupBox1.Location = new Point(12, 12);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(532, 279);
    this.groupBox1.TabIndex = 0;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "Подписанные объекты";
    this.cbCurrentUser.AutoSize = true;
    this.cbCurrentUser.Location = new Point(13, 203);
    this.cbCurrentUser.Name = "cbCurrentUser";
    this.cbCurrentUser.Size = new Size(155, 17);
    this.cbCurrentUser.TabIndex = 15;
    this.cbCurrentUser.Text = "Текущим пользователем";
    this.cbCurrentUser.UseVisualStyleBackColor = true;
    this.cbCurrentUser.CheckedChanged += new EventHandler(this.CurrentUser_CheckedChanged);
    this.label7.AutoSize = true;
    this.label7.Location = new Point(10, 234);
    this.label7.Name = "label7";
    this.label7.Size = new Size(119, 13);
    this.label7.TabIndex = 14;
    this.label7.Text = "Резолюция содержит:";
    this.tbResolution.Location = new Point(12, 250);
    this.tbResolution.Name = "tbResolution";
    this.tbResolution.Size = new Size(508, 20);
    this.tbResolution.TabIndex = 10;
    this.label6.AutoSize = true;
    this.label6.Location = new Point(269, 163);
    this.label6.Name = "label6";
    this.label6.Size = new Size(153, 13);
    this.label6.TabIndex = 12;
    this.label6.Text = "Исполняющим обязанности:";
    this.bIOUserChange.Location = new Point(496, 178);
    this.bIOUserChange.Name = "bIOUserChange";
    this.bIOUserChange.Size = new Size(24, 23);
    this.bIOUserChange.TabIndex = 9;
    this.bIOUserChange.Text = "...";
    this.bIOUserChange.UseVisualStyleBackColor = true;
    this.bIOUserChange.Click += new EventHandler(this.IOUserChange_Click);
    this.tbIOUser.BackColor = SystemColors.Window;
    this.tbIOUser.Location = new Point(272, 179);
    this.tbIOUser.Name = "tbIOUser";
    this.tbIOUser.ReadOnly = true;
    this.tbIOUser.Size = new Size(222, 20);
    this.tbIOUser.TabIndex = 8;
    this.tbIOUser.KeyDown += new KeyEventHandler(this.User_KeyDown);
    this.label5.AutoSize = true;
    this.label5.Location = new Point(9, 161);
    this.label5.Name = "label5";
    this.label5.Size = new Size(91, 13);
    this.label5.TabIndex = 9;
    this.label5.Text = "Пользователем:";
    this.bUserChange.Location = new Point(236, 176 /*0xB0*/);
    this.bUserChange.Name = "bUserChange";
    this.bUserChange.Size = new Size(24, 23);
    this.bUserChange.TabIndex = 7;
    this.bUserChange.Text = "...";
    this.bUserChange.UseVisualStyleBackColor = true;
    this.bUserChange.Click += new EventHandler(this.UserChange_Click);
    this.tbUser.BackColor = SystemColors.Window;
    this.tbUser.Location = new Point(12, 177);
    this.tbUser.Name = "tbUser";
    this.tbUser.ReadOnly = true;
    this.tbUser.Size = new Size(222, 20);
    this.tbUser.TabIndex = 6;
    this.tbUser.KeyDown += new KeyEventHandler(this.User_KeyDown);
    this.cbGraph.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbGraph.FormattingEnabled = true;
    this.cbGraph.Location = new Point(336, 126);
    this.cbGraph.Name = "cbGraph";
    this.cbGraph.Size = new Size(184, 21);
    this.cbGraph.TabIndex = 5;
    this.label4.AutoSize = true;
    this.label4.Location = new Point(333, 110);
    this.label4.Name = "label4";
    this.label4.Size = new Size(51, 13);
    this.label4.TabIndex = 5;
    this.label4.Text = "В графе:";
    this.cbRanks.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbRanks.FormattingEnabled = true;
    this.cbRanks.Location = new Point(12, 126);
    this.cbRanks.Name = "cbRanks";
    this.cbRanks.Size = new Size(306, 21);
    this.cbRanks.TabIndex = 4;
    this.label3.AutoSize = true;
    this.label3.Location = new Point(9, 110);
    this.label3.Name = "label3";
    this.label3.Size = new Size(76, 13);
    this.label3.TabIndex = 3;
    this.label3.Text = "Должностью:";
    this.groupBox3.Controls.Add((Control) this.dtpEndDate);
    this.groupBox3.Controls.Add((Control) this.dtpStartDate);
    this.groupBox3.Controls.Add((Control) this.cbEndDateCurrent);
    this.groupBox3.Controls.Add((Control) this.cbStartDateCurrent);
    this.groupBox3.Controls.Add((Control) this.label2);
    this.groupBox3.Controls.Add((Control) this.label1);
    this.groupBox3.Location = new Point(13, 19);
    this.groupBox3.Name = "groupBox3";
    this.groupBox3.Size = new Size(507, 88);
    this.groupBox3.TabIndex = 2;
    this.groupBox3.TabStop = false;
    this.groupBox3.Text = "В период";
    this.dtpEndDate.EditValue = (object) null;
    this.dtpEndDate.Location = new Point(209, 36);
    this.dtpEndDate.Name = "dtpEndDate";
    this.dtpEndDate.Properties.Buttons.AddRange(new EditorButton[1]
    {
      new EditorButton(ButtonPredefines.Combo)
    });
    this.dtpEndDate.Properties.DisplayFormat.FormatString = "dd.MM.yyyy";
    this.dtpEndDate.Properties.DisplayFormat.FormatType = FormatType.DateTime;
    this.dtpEndDate.Properties.EditFormat.FormatString = "dd.MM.yyyy";
    this.dtpEndDate.Properties.EditFormat.FormatType = FormatType.DateTime;
    this.dtpEndDate.ShowToolTips = false;
    this.dtpEndDate.Size = new Size(165, 23);
    this.dtpEndDate.TabIndex = 16 /*0x10*/;
    this.dtpStartDate.EditValue = (object) null;
    this.dtpStartDate.Location = new Point(21, 36);
    this.dtpStartDate.Name = "dtpStartDate";
    this.dtpStartDate.Properties.Buttons.AddRange(new EditorButton[1]
    {
      new EditorButton(ButtonPredefines.Combo)
    });
    this.dtpStartDate.Properties.DisplayFormat.FormatString = "dd.MM.yyyy";
    this.dtpStartDate.Properties.DisplayFormat.FormatType = FormatType.DateTime;
    this.dtpStartDate.Properties.EditFormat.FormatString = "dd.MM.yyyy";
    this.dtpStartDate.Properties.EditFormat.FormatType = FormatType.DateTime;
    this.dtpStartDate.ShowToolTips = false;
    this.dtpStartDate.Size = new Size(165, 23);
    this.dtpStartDate.TabIndex = 15;
    this.cbEndDateCurrent.AutoSize = true;
    this.cbEndDateCurrent.Location = new Point(209, 62);
    this.cbEndDateCurrent.Name = "cbEndDateCurrent";
    this.cbEndDateCurrent.Size = new Size(97, 17);
    this.cbEndDateCurrent.TabIndex = 5;
    this.cbEndDateCurrent.Text = "Текущая дата";
    this.cbEndDateCurrent.UseVisualStyleBackColor = true;
    this.cbEndDateCurrent.CheckedChanged += new EventHandler(this.EndDateCurrent_CheckedChanged);
    this.cbStartDateCurrent.AutoSize = true;
    this.cbStartDateCurrent.Location = new Point(21, 62);
    this.cbStartDateCurrent.Name = "cbStartDateCurrent";
    this.cbStartDateCurrent.Size = new Size(97, 17);
    this.cbStartDateCurrent.TabIndex = 4;
    this.cbStartDateCurrent.Text = "Текущая дата";
    this.cbStartDateCurrent.UseVisualStyleBackColor = true;
    this.cbStartDateCurrent.CheckedChanged += new EventHandler(this.StartDateCurrent_CheckedChanged);
    this.label2.AutoSize = true;
    this.label2.Location = new Point(206, 22);
    this.label2.Name = "label2";
    this.label2.Size = new Size(49, 13);
    this.label2.TabIndex = 3;
    this.label2.Text = "По дату:";
    this.label1.AutoSize = true;
    this.label1.Location = new Point(18, 21);
    this.label1.Name = "label1";
    this.label1.Size = new Size(45, 13);
    this.label1.TabIndex = 1;
    this.label1.Text = "С даты:";
    this.bOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bOK.DialogResult = DialogResult.OK;
    this.bOK.Location = new Point(293, 3);
    this.bOK.Name = "bOK";
    this.bOK.Size = new Size(121, 27);
    this.bOK.TabIndex = 11;
    this.bOK.Text = "ОК";
    this.bOK.UseVisualStyleBackColor = true;
    this.bCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Location = new Point(420, 3);
    this.bCancel.Name = "bCancel";
    this.bCancel.Size = new Size(121, 27);
    this.bCancel.TabIndex = 12;
    this.bCancel.Text = "Отменить";
    this.bCancel.UseVisualStyleBackColor = true;
    this.panel1.Controls.Add((Control) this.bCancel);
    this.panel1.Controls.Add((Control) this.bOK);
    this.panel1.Dock = DockStyle.Bottom;
    this.panel1.Location = new Point(0, 305);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(553, 45);
    this.panel1.TabIndex = 13;
    this.AcceptButton = (IButtonControl) this.bOK;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.bCancel;
    this.ClientSize = new Size(553, 350);
    this.Controls.Add((Control) this.panel1);
    this.Controls.Add((Control) this.groupBox1);
    this.FormBorderStyle = FormBorderStyle.FixedDialog;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (SignConditionsEditor);
    this.Text = "Подписи";
    this.FormClosing += new FormClosingEventHandler(this.SignConditionsEditor_FormClosing);
    this.Shown += new EventHandler(this.SignConditionsEditor_Shown);
    this.groupBox1.ResumeLayout(false);
    this.groupBox1.PerformLayout();
    this.groupBox3.ResumeLayout(false);
    this.groupBox3.PerformLayout();
    this.dtpEndDate.Properties.EndInit();
    this.dtpStartDate.Properties.EndInit();
    this.panel1.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  internal class ComboBoxValue
  {
    public ComboBoxValue(object id, string caption)
    {
      this.ID = id;
      this.Caption = caption;
    }

    public string Caption { get; private set; }

    public object ID { get; private set; }

    public override string ToString() => this.Caption;
  }
}
