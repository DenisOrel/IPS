// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.FormulaControl
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Office.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

internal class FormulaControl : UserControl
{
  [NotNull]
  private readonly EventHandler _modifyMethod;
  private bool _autofill;
  [CanBeNull]
  private RegNumberSettings _oldTemplate;
  private bool _changed;
  private readonly long _unitID;
  private readonly OfficeDocumentTypes _officeType;
  private int _objectTypeID;
  private IContainer components;
  private CheckBox cbGenerate;
  private RadioButton rbNotReset;
  private RadioButton rbPerMonth;
  private RadioButton rbPerYear;
  private CheckBox cbTypeNumering;
  private TextBox tbTemplateString;
  private Label label1;
  private Button bReset;
  private CheckBox cbEnableEmptyRegNum;

  public FormulaControl([NotNull] EventHandler modifyMethod, [NotEmpty] long unitID, OfficeDocumentTypes officeType)
  {
    this.InitializeComponent();
    this._modifyMethod = modifyMethod;
    this._unitID = unitID;
    this._officeType = officeType;
  }

  private void OnChanged([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    if (this._autofill || this.Changed || this.Template.Equals((object) this._oldTemplate))
      return;
    this._modifyMethod(sender, e);
    this.Changed = true;
  }

  public void SetData([NotNull] RegNumberSettings template, int objectTypeID)
  {
    this._autofill = true;
    try
    {
      this._oldTemplate = template;
      this._objectTypeID = objectTypeID;
      this.cbEnableEmptyRegNum.Checked = template.EnableEmptyRegNumbers;
      this.cbGenerate.Checked = template.Template != string.Empty;
      this.tbTemplateString.Text = template.Template;
      this.cbTypeNumering.Checked = template.CountWithinType;
      if (template.CountWithinType)
      {
        if (template.CountResetType == CountResetTypes.None)
          this.rbNotReset.Checked = true;
        else if (template.CountResetType == CountResetTypes.PerMonth)
          this.rbPerMonth.Checked = true;
        else if (template.CountResetType == CountResetTypes.PerYear)
          this.rbPerYear.Checked = true;
      }
      this.RefreshControls();
    }
    finally
    {
      this._autofill = false;
    }
  }

  [NotNull]
  public RegNumberSettings Template
  {
    get
    {
      RegNumberSettings template = new RegNumberSettings();
      template.EnableEmptyRegNumbers = this.cbEnableEmptyRegNum.Checked;
      template.AutoGenerateRegNumber = this.cbGenerate.Checked;
      if (this.cbGenerate.Checked)
      {
        template.Template = this.tbTemplateString.Text;
        template.CountWithinType = this.cbTypeNumering.Checked;
        if (this.rbNotReset.Checked)
          template.CountResetType = CountResetTypes.None;
        else if (this.rbPerMonth.Checked)
          template.CountResetType = CountResetTypes.PerMonth;
        else if (this.rbPerYear.Checked)
          template.CountResetType = CountResetTypes.PerYear;
      }
      return template;
    }
  }

  private void RefreshControls()
  {
    this.tbTemplateString.Enabled = this.cbGenerate.Checked;
    this.cbTypeNumering.Enabled = this.cbGenerate.Checked;
    this.bReset.Enabled = this.cbGenerate.Checked;
    this.rbNotReset.Enabled = this.cbGenerate.Checked && this.cbTypeNumering.Checked;
    this.rbPerMonth.Enabled = this.cbGenerate.Checked && this.cbTypeNumering.Checked;
    this.rbPerYear.Enabled = this.cbGenerate.Checked && this.cbTypeNumering.Checked;
  }

  private void cbTypeNumbering_CheckedChanged([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    this.RefreshControls();
    this.OnChanged(sender, e);
  }

  private void cbGenerate_CheckedChanged([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    this.RefreshControls();
    this.OnChanged(sender, e);
  }

  public bool Changed
  {
    get => this._changed;
    set
    {
      if (value)
        return;
      this._oldTemplate = this.Template;
      this._changed = false;
    }
  }

  private void bReset_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    if (MessageBox.Show("Сброс счетчика может привести к появлению одинаковых регистрационных номеров. Продолжить?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!sessionKeeper.Session.GetCustomService<IRegistrationNumberGenerator>().ResetCounter(sessionKeeper.Session.SessionGUID, this._objectTypeID, this._officeType, this._unitID))
        return;
      int num = (int) MessageBox.Show("Счетчик успешно обнулен!");
    }
  }

  private void cbEnableEmptyRegNum_CheckedChanged([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    this.RefreshControls();
    this.OnChanged(sender, e);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.cbGenerate = new CheckBox();
    this.rbNotReset = new RadioButton();
    this.rbPerMonth = new RadioButton();
    this.rbPerYear = new RadioButton();
    this.cbTypeNumering = new CheckBox();
    this.tbTemplateString = new TextBox();
    this.label1 = new Label();
    this.bReset = new Button();
    this.cbEnableEmptyRegNum = new CheckBox();
    this.SuspendLayout();
    this.cbGenerate.AutoSize = true;
    this.cbGenerate.Checked = true;
    this.cbGenerate.CheckState = CheckState.Checked;
    this.cbGenerate.ImeMode = ImeMode.NoControl;
    this.cbGenerate.Location = new Point(17, 29);
    this.cbGenerate.Name = "cbGenerate";
    this.cbGenerate.Size = new Size(303, 17);
    this.cbGenerate.TabIndex = 1;
    this.cbGenerate.Text = "Автоматическая генерация регистрационного номера";
    this.cbGenerate.UseVisualStyleBackColor = true;
    this.cbGenerate.CheckedChanged += new EventHandler(this.cbGenerate_CheckedChanged);
    this.rbNotReset.AutoSize = true;
    this.rbNotReset.ForeColor = SystemColors.ControlText;
    this.rbNotReset.ImeMode = ImeMode.NoControl;
    this.rbNotReset.Location = new Point(217, (int) sbyte.MaxValue);
    this.rbNotReset.Name = "rbNotReset";
    this.rbNotReset.Size = new Size(145, 17);
    this.rbNotReset.TabIndex = 5;
    this.rbNotReset.Text = "Не сбрасывать счетчик";
    this.rbNotReset.UseVisualStyleBackColor = true;
    this.rbNotReset.CheckedChanged += new EventHandler(this.OnChanged);
    this.rbPerMonth.AutoSize = true;
    this.rbPerMonth.ForeColor = SystemColors.ControlText;
    this.rbPerMonth.ImeMode = ImeMode.NoControl;
    this.rbPerMonth.Location = new Point(217, 108);
    this.rbPerMonth.Name = "rbPerMonth";
    this.rbPerMonth.Size = new Size(169, 17);
    this.rbPerMonth.TabIndex = 4;
    this.rbPerMonth.Text = "Сброс счетчика раз в месяц";
    this.rbPerMonth.UseVisualStyleBackColor = true;
    this.rbPerMonth.CheckedChanged += new EventHandler(this.OnChanged);
    this.rbPerYear.AutoSize = true;
    this.rbPerYear.Checked = true;
    this.rbPerYear.ForeColor = SystemColors.ControlText;
    this.rbPerYear.ImeMode = ImeMode.NoControl;
    this.rbPerYear.Location = new Point(217, 89);
    this.rbPerYear.Name = "rbPerYear";
    this.rbPerYear.Size = new Size(154, 17);
    this.rbPerYear.TabIndex = 3;
    this.rbPerYear.TabStop = true;
    this.rbPerYear.Text = "Сброс счетчика раз в год";
    this.rbPerYear.UseVisualStyleBackColor = true;
    this.rbPerYear.CheckedChanged += new EventHandler(this.OnChanged);
    this.cbTypeNumering.AutoSize = true;
    this.cbTypeNumering.ForeColor = SystemColors.ControlText;
    this.cbTypeNumering.ImeMode = ImeMode.NoControl;
    this.cbTypeNumering.Location = new Point(17, 174);
    this.cbTypeNumering.Name = "cbTypeNumering";
    this.cbTypeNumering.Size = new Size(219, 17);
    this.cbTypeNumering.TabIndex = 7;
    this.cbTypeNumering.Text = "Нумерация в пределах типа объектов";
    this.cbTypeNumering.UseVisualStyleBackColor = true;
    this.cbTypeNumering.CheckedChanged += new EventHandler(this.cbTypeNumbering_CheckedChanged);
    this.tbTemplateString.Location = new Point(17, 66);
    this.tbTemplateString.Name = "tbTemplateString";
    this.tbTemplateString.Size = new Size(369, 20);
    this.tbTemplateString.TabIndex = 2;
    this.tbTemplateString.TextChanged += new EventHandler(this.OnChanged);
    this.label1.AutoSize = true;
    this.label1.ForeColor = SystemColors.ControlText;
    this.label1.ImeMode = ImeMode.NoControl;
    this.label1.Location = new Point(14, 50);
    this.label1.Name = "label1";
    this.label1.Size = new Size(46, 13);
    this.label1.TabIndex = 17;
    this.label1.Text = "Шаблон";
    this.bReset.Location = new Point(280, 150);
    this.bReset.Name = "bReset";
    this.bReset.Size = new Size(106, 23);
    this.bReset.TabIndex = 6;
    this.bReset.Text = "Сбросить счетчик";
    this.bReset.UseVisualStyleBackColor = true;
    this.bReset.Click += new EventHandler(this.bReset_Click);
    this.cbEnableEmptyRegNum.AutoSize = true;
    this.cbEnableEmptyRegNum.ImeMode = ImeMode.NoControl;
    this.cbEnableEmptyRegNum.Location = new Point(17, 7);
    this.cbEnableEmptyRegNum.Name = "cbEnableEmptyRegNum";
    this.cbEnableEmptyRegNum.Size = new Size(343, 17);
    this.cbEnableEmptyRegNum.TabIndex = 0;
    this.cbEnableEmptyRegNum.Text = "Регистрационный номер не присваивается новому документу";
    this.cbEnableEmptyRegNum.UseVisualStyleBackColor = true;
    this.cbEnableEmptyRegNum.CheckedChanged += new EventHandler(this.cbEnableEmptyRegNum_CheckedChanged);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.cbEnableEmptyRegNum);
    this.Controls.Add((Control) this.bReset);
    this.Controls.Add((Control) this.cbGenerate);
    this.Controls.Add((Control) this.rbNotReset);
    this.Controls.Add((Control) this.rbPerMonth);
    this.Controls.Add((Control) this.rbPerYear);
    this.Controls.Add((Control) this.cbTypeNumering);
    this.Controls.Add((Control) this.tbTemplateString);
    this.Controls.Add((Control) this.label1);
    this.Name = nameof (FormulaControl);
    this.Size = new Size(429, 199);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
