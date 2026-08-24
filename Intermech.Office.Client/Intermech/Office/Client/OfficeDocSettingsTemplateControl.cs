// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.OfficeDocSettingsTemplateControl
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

public class OfficeDocSettingsTemplateControl : UserControl
{
  [NotNull]
  private readonly EventHandler _modifyMethod;
  private readonly OfficeDocumentTypes _officeType;
  internal int _ObjectTypeID;
  private IContainer components;
  private RadioButton rbNotReset;
  private RadioButton rbPerMonth;
  private RadioButton rbPerYear;
  private CheckBox cbTypeNumering;
  private TextBox tbTemplateString;
  private Label label1;
  private CheckBox cbGenerate;
  private CheckBox cbDesignationEqualRegNumber;
  private CheckBox cbUnitNumering;
  private CheckBox cbEnableEmptyRegNum;
  private Button bReset;

  public bool EnableEmpty
  {
    get => this.cbEnableEmptyRegNum.Checked;
    set => this.cbEnableEmptyRegNum.Checked = value;
  }

  public void SetEnableEmpty(bool enable) => this.cbEnableEmptyRegNum.Enabled = enable;

  public bool Generate
  {
    get => this.cbGenerate.Checked;
    set => this.cbGenerate.Checked = value;
  }

  [NotNull]
  public string TemplateString
  {
    get => this.tbTemplateString.Text;
    set => this.tbTemplateString.Text = value;
  }

  public CountResetTypes ResetType
  {
    get
    {
      if (this.rbNotReset.Checked)
        return CountResetTypes.None;
      if (this.rbPerYear.Checked)
        return CountResetTypes.PerYear;
      return this.rbPerMonth.Checked ? CountResetTypes.PerMonth : CountResetTypes.None;
    }
    set
    {
      switch (value)
      {
        case CountResetTypes.None:
          this.rbNotReset.Checked = true;
          break;
        case CountResetTypes.PerYear:
          this.rbPerYear.Checked = true;
          break;
        case CountResetTypes.PerMonth:
          this.rbPerMonth.Checked = true;
          break;
      }
    }
  }

  public bool TypeNumbering
  {
    get => this.cbTypeNumering.Checked;
    set => this.cbTypeNumering.Checked = value;
  }

  public bool UnitNumbering
  {
    get => this.cbUnitNumering.Checked;
    set => this.cbUnitNumering.Checked = value;
  }

  public bool DesignationEqualRegNumber
  {
    get => this.cbDesignationEqualRegNumber.Checked;
    set => this.cbDesignationEqualRegNumber.Checked = value;
  }

  public OfficeDocSettingsTemplateControl([NotNull] EventHandler modifyMethod, OfficeDocumentTypes officeType)
  {
    this.InitializeComponent();
    this._modifyMethod = modifyMethod;
    this._officeType = officeType;
  }

  private void OnChanged([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    this._modifyMethod(sender, e);
  }

  private void cbTypeNumbering_CheckedChanged([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    this.rbNotReset.Enabled = this.cbTypeNumering.Checked || this.cbUnitNumering.Checked;
    this.rbPerMonth.Enabled = this.cbTypeNumering.Checked || this.cbUnitNumering.Checked;
    this.rbPerYear.Enabled = this.cbTypeNumering.Checked || this.cbUnitNumering.Checked;
    this.OnChanged(sender, e);
  }

  public void ResetControl()
  {
    this.EnableEmpty = false;
    this.Generate = false;
    this.TemplateString = string.Empty;
    this.ResetType = CountResetTypes.PerYear;
    this.TypeNumbering = false;
    this.UnitNumbering = false;
    this.DesignationEqualRegNumber = false;
    this.cbUnitNumbering_CheckedChanged((object) this, EventArgs.Empty);
  }

  private void cbGenerate_CheckedChanged([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    this.OnChanged(sender, e);
  }

  private void cbUnitNumbering_CheckedChanged([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    this.rbNotReset.Enabled = this.cbTypeNumering.Checked || this.cbUnitNumering.Checked;
    this.rbPerMonth.Enabled = this.cbTypeNumering.Checked || this.cbUnitNumering.Checked;
    this.rbPerYear.Enabled = this.cbTypeNumering.Checked || this.cbUnitNumering.Checked;
    this.OnChanged(sender, e);
  }

  private void bReset_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    if (MessageBox.Show("Сброс счетчика может привести к появлению одинаковых регистрационных номеров. Продолжить?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!sessionKeeper.Session.GetCustomService<IRegistrationNumberGenerator>().ResetCounter(sessionKeeper.Session.SessionGUID, this._ObjectTypeID, this._officeType))
        return;
      int num = (int) MessageBox.Show("Счетчик успешно обнулен!");
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (OfficeDocSettingsTemplateControl));
    this.rbNotReset = new RadioButton();
    this.rbPerMonth = new RadioButton();
    this.rbPerYear = new RadioButton();
    this.cbTypeNumering = new CheckBox();
    this.tbTemplateString = new TextBox();
    this.label1 = new Label();
    this.cbGenerate = new CheckBox();
    this.cbDesignationEqualRegNumber = new CheckBox();
    this.cbUnitNumering = new CheckBox();
    this.cbEnableEmptyRegNum = new CheckBox();
    this.bReset = new Button();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.rbNotReset, "rbNotReset");
    this.rbNotReset.ForeColor = SystemColors.ControlText;
    this.rbNotReset.Name = "rbNotReset";
    this.rbNotReset.UseVisualStyleBackColor = true;
    this.rbNotReset.CheckedChanged += new EventHandler(this.OnChanged);
    componentResourceManager.ApplyResources((object) this.rbPerMonth, "rbPerMonth");
    this.rbPerMonth.ForeColor = SystemColors.ControlText;
    this.rbPerMonth.Name = "rbPerMonth";
    this.rbPerMonth.UseVisualStyleBackColor = true;
    this.rbPerMonth.CheckedChanged += new EventHandler(this.OnChanged);
    this.rbPerMonth.TextChanged += new EventHandler(this.OnChanged);
    componentResourceManager.ApplyResources((object) this.rbPerYear, "rbPerYear");
    this.rbPerYear.Checked = true;
    this.rbPerYear.ForeColor = SystemColors.ControlText;
    this.rbPerYear.Name = "rbPerYear";
    this.rbPerYear.TabStop = true;
    this.rbPerYear.UseVisualStyleBackColor = true;
    this.rbPerYear.CheckedChanged += new EventHandler(this.OnChanged);
    this.rbPerYear.TextChanged += new EventHandler(this.OnChanged);
    componentResourceManager.ApplyResources((object) this.cbTypeNumering, "cbTypeNumering");
    this.cbTypeNumering.ForeColor = SystemColors.ControlText;
    this.cbTypeNumering.Name = "cbTypeNumering";
    this.cbTypeNumering.UseVisualStyleBackColor = true;
    this.cbTypeNumering.CheckedChanged += new EventHandler(this.cbTypeNumbering_CheckedChanged);
    componentResourceManager.ApplyResources((object) this.tbTemplateString, "tbTemplateString");
    this.tbTemplateString.Name = "tbTemplateString";
    this.tbTemplateString.TextChanged += new EventHandler(this.OnChanged);
    componentResourceManager.ApplyResources((object) this.label1, "label1");
    this.label1.ForeColor = SystemColors.ControlText;
    this.label1.Name = "label1";
    componentResourceManager.ApplyResources((object) this.cbGenerate, "cbGenerate");
    this.cbGenerate.Checked = true;
    this.cbGenerate.CheckState = CheckState.Checked;
    this.cbGenerate.Name = "cbGenerate";
    this.cbGenerate.UseVisualStyleBackColor = true;
    this.cbGenerate.CheckedChanged += new EventHandler(this.cbGenerate_CheckedChanged);
    componentResourceManager.ApplyResources((object) this.cbDesignationEqualRegNumber, "cbDesignationEqualRegNumber");
    this.cbDesignationEqualRegNumber.Name = "cbDesignationEqualRegNumber";
    this.cbDesignationEqualRegNumber.UseVisualStyleBackColor = true;
    this.cbDesignationEqualRegNumber.CheckedChanged += new EventHandler(this.OnChanged);
    componentResourceManager.ApplyResources((object) this.cbUnitNumering, "cbUnitNumering");
    this.cbUnitNumering.ForeColor = SystemColors.ControlText;
    this.cbUnitNumering.Name = "cbUnitNumering";
    this.cbUnitNumering.UseVisualStyleBackColor = true;
    this.cbUnitNumering.CheckedChanged += new EventHandler(this.cbUnitNumbering_CheckedChanged);
    componentResourceManager.ApplyResources((object) this.cbEnableEmptyRegNum, "cbEnableEmptyRegNum");
    this.cbEnableEmptyRegNum.Name = "cbEnableEmptyRegNum";
    this.cbEnableEmptyRegNum.UseVisualStyleBackColor = true;
    this.cbEnableEmptyRegNum.CheckedChanged += new EventHandler(this.OnChanged);
    componentResourceManager.ApplyResources((object) this.bReset, "bReset");
    this.bReset.Name = "bReset";
    this.bReset.UseVisualStyleBackColor = true;
    this.bReset.Click += new EventHandler(this.bReset_Click);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.bReset);
    this.Controls.Add((Control) this.cbEnableEmptyRegNum);
    this.Controls.Add((Control) this.cbUnitNumering);
    this.Controls.Add((Control) this.cbDesignationEqualRegNumber);
    this.Controls.Add((Control) this.cbGenerate);
    this.Controls.Add((Control) this.rbNotReset);
    this.Controls.Add((Control) this.rbPerMonth);
    this.Controls.Add((Control) this.rbPerYear);
    this.Controls.Add((Control) this.cbTypeNumering);
    this.Controls.Add((Control) this.tbTemplateString);
    this.Controls.Add((Control) this.label1);
    this.Name = nameof (OfficeDocSettingsTemplateControl);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
