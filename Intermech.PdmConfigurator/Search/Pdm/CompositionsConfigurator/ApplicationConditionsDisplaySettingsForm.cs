// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.CompositionsConfigurator.ApplicationConditionsDisplaySettingsForm
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.Client.Core;
using Intermech.Extensions;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Pdm.CompositionsConfigurator;

public sealed class ApplicationConditionsDisplaySettingsForm : Form
{
  private ApplicationConditionsDisplaySettings _settings = new ApplicationConditionsDisplaySettings();
  private IContainer components;
  private TableLayoutPanel tableLayoutPanel1;
  private FlowLayoutPanel flowLayoutPanel1;
  private Button _cancelButton;
  private Button _applyButton;
  private TableLayoutPanel tableLayoutPanel2;
  private Label label1;
  private Label label2;
  private ComboBox _optionNameReplacemenetComboBox;
  private ComboBox _optionValueReplacementComboBox;
  private ComboBox _relationOperatorDisplayTypeComboBox;
  private Label label3;

  public ApplicationConditionsDisplaySettingsForm()
  {
    this.InitializeComponent();
    this.InitializeOptionNameReplacementComboBox();
    this.InitializeOptionValueReplacementComboBox();
    this.InitializeRelationOperatorDisplayTypeComboBox();
  }

  public ApplicationConditionsDisplaySettings Settings
  {
    get => this._settings;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      if (this._settings == value)
        return;
      this._settings = value.Clone();
      this.SetComboBoxSelectedValue(this._optionNameReplacemenetComboBox, (object) this._settings.NameReplacement);
      this.SetComboBoxSelectedValue(this._optionValueReplacementComboBox, (object) this._settings.ValueReplacement);
      this.SetComboBoxSelectedValue(this._relationOperatorDisplayTypeComboBox, (object) this._settings.RelationOperatorDisplayType);
    }
  }

  private void ApplicationConditionsDisplaySettingsForm_Load(object sender, EventArgs e)
  {
    FormStorage.LoadLayout((Control) this);
  }

  private void ApplicationConditionsDisplaySettingsForm_FormClosing(
    object sender,
    FormClosingEventArgs e)
  {
    FormStorage.SaveLayout((Control) this);
  }

  private void OptionNameReplacemenetComboBox_SelectedIndexChanged(object sender, EventArgs e)
  {
    this._settings.NameReplacement = (ApplicationConditionsDisplaySettings.OptionNameReplacement) ((Tuple<string, object>) this._optionNameReplacemenetComboBox.SelectedItem).Item2;
  }

  private void OptionValueReplacementComboBox_SelectedIndexChanged(object sender, EventArgs e)
  {
    this._settings.ValueReplacement = (ApplicationConditionsDisplaySettings.OptionValueReplacement) ((Tuple<string, object>) this._optionValueReplacementComboBox.SelectedItem).Item2;
  }

  private void RelationOperatorDisplayTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
  {
    this._settings.RelationOperatorDisplayType = (ApplicationConditionsDisplaySettings.OperatorDisplayType) ((Tuple<string, object>) this._relationOperatorDisplayTypeComboBox.SelectedItem).Item2;
  }

  private void InitializeOptionNameReplacementComboBox()
  {
    this._optionNameReplacemenetComboBox.DisplayMember = "Item1";
    this._optionNameReplacemenetComboBox.ValueMember = "Item2";
    this._optionNameReplacemenetComboBox.BeginUpdate();
    try
    {
      this._optionNameReplacemenetComboBox.Items.Clear();
      this._optionNameReplacemenetComboBox.Items.Add((object) this.CreateDescriptionValueTupleFromEnum((Enum) ApplicationConditionsDisplaySettings.OptionNameReplacement.OptionName));
      this._optionNameReplacemenetComboBox.Items.Add((object) this.CreateDescriptionValueTupleFromEnum((Enum) ApplicationConditionsDisplaySettings.OptionNameReplacement.OptionCode));
    }
    finally
    {
      this._optionNameReplacemenetComboBox.EndUpdate();
    }
    this._optionNameReplacemenetComboBox.SelectedIndex = 0;
  }

  private void InitializeOptionValueReplacementComboBox()
  {
    this._optionValueReplacementComboBox.DisplayMember = "Item1";
    this._optionValueReplacementComboBox.ValueMember = "Item2";
    this._optionValueReplacementComboBox.BeginUpdate();
    try
    {
      this._optionValueReplacementComboBox.Items.Clear();
      this._optionValueReplacementComboBox.Items.Add((object) this.CreateDescriptionValueTupleFromEnum((Enum) ApplicationConditionsDisplaySettings.OptionValueReplacement.OptionValue));
      this._optionValueReplacementComboBox.Items.Add((object) this.CreateDescriptionValueTupleFromEnum((Enum) ApplicationConditionsDisplaySettings.OptionValueReplacement.OptionValueCode));
      this._optionValueReplacementComboBox.Items.Add((object) this.CreateDescriptionValueTupleFromEnum((Enum) ApplicationConditionsDisplaySettings.OptionValueReplacement.OptionValueDescription));
    }
    finally
    {
      this._optionValueReplacementComboBox.EndUpdate();
    }
    this._optionValueReplacementComboBox.SelectedIndex = 0;
  }

  private void InitializeRelationOperatorDisplayTypeComboBox()
  {
    this._relationOperatorDisplayTypeComboBox.DisplayMember = "Item1";
    this._relationOperatorDisplayTypeComboBox.ValueMember = "Item2";
    this._relationOperatorDisplayTypeComboBox.BeginUpdate();
    try
    {
      this._relationOperatorDisplayTypeComboBox.Items.Clear();
      this._relationOperatorDisplayTypeComboBox.Items.Add((object) this.CreateDescriptionValueTupleFromEnum((Enum) ApplicationConditionsDisplaySettings.OperatorDisplayType.Words));
      this._relationOperatorDisplayTypeComboBox.Items.Add((object) this.CreateDescriptionValueTupleFromEnum((Enum) ApplicationConditionsDisplaySettings.OperatorDisplayType.Symbols));
    }
    finally
    {
      this._relationOperatorDisplayTypeComboBox.EndUpdate();
    }
    this._relationOperatorDisplayTypeComboBox.SelectedIndex = 0;
  }

  private Tuple<string, object> CreateDescriptionValueTupleFromEnum(Enum @enum)
  {
    return new Tuple<string, object>(@enum.GetDescription(), (object) @enum);
  }

  private void SetComboBoxSelectedValue(ComboBox comboBox, object selectedValue)
  {
    Tuple<string, object> tuple = comboBox.Items.Cast<Tuple<string, object>>().FirstOrDefault<Tuple<string, object>>((Func<Tuple<string, object>, bool>) (o => object.Equals(o.Item2, selectedValue)));
    comboBox.SelectedItem = (object) tuple;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this.flowLayoutPanel1 = new FlowLayoutPanel();
    this._cancelButton = new Button();
    this._applyButton = new Button();
    this.tableLayoutPanel2 = new TableLayoutPanel();
    this.label1 = new Label();
    this.label2 = new Label();
    this._optionNameReplacemenetComboBox = new ComboBox();
    this._optionValueReplacementComboBox = new ComboBox();
    this._relationOperatorDisplayTypeComboBox = new ComboBox();
    this.label3 = new Label();
    this.tableLayoutPanel1.SuspendLayout();
    this.flowLayoutPanel1.SuspendLayout();
    this.tableLayoutPanel2.SuspendLayout();
    this.SuspendLayout();
    this.tableLayoutPanel1.ColumnCount = 1;
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.Controls.Add((Control) this.flowLayoutPanel1, 0, 1);
    this.tableLayoutPanel1.Controls.Add((Control) this.tableLayoutPanel2, 0, 0);
    this.tableLayoutPanel1.Dock = DockStyle.Fill;
    this.tableLayoutPanel1.Location = new Point(0, 0);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.RowCount = 2;
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40f));
    this.tableLayoutPanel1.Size = new Size(550, 130);
    this.tableLayoutPanel1.TabIndex = 0;
    this.flowLayoutPanel1.Controls.Add((Control) this._cancelButton);
    this.flowLayoutPanel1.Controls.Add((Control) this._applyButton);
    this.flowLayoutPanel1.Dock = DockStyle.Fill;
    this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
    this.flowLayoutPanel1.Location = new Point(3, 93);
    this.flowLayoutPanel1.Name = "flowLayoutPanel1";
    this.flowLayoutPanel1.Size = new Size(544, 34);
    this.flowLayoutPanel1.TabIndex = 0;
    this._cancelButton.DialogResult = DialogResult.Cancel;
    this._cancelButton.Location = new Point(466, 3);
    this._cancelButton.Name = "_cancelButton";
    this._cancelButton.Size = new Size(75, 23);
    this._cancelButton.TabIndex = 0;
    this._cancelButton.Text = "Отмена";
    this._cancelButton.UseVisualStyleBackColor = true;
    this._applyButton.DialogResult = DialogResult.OK;
    this._applyButton.Location = new Point(385, 3);
    this._applyButton.Name = "_applyButton";
    this._applyButton.Size = new Size(75, 23);
    this._applyButton.TabIndex = 1;
    this._applyButton.Text = "OK";
    this._applyButton.UseVisualStyleBackColor = true;
    this.tableLayoutPanel2.ColumnCount = 2;
    this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
    this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
    this.tableLayoutPanel2.Controls.Add((Control) this.label1, 0, 0);
    this.tableLayoutPanel2.Controls.Add((Control) this.label2, 0, 1);
    this.tableLayoutPanel2.Controls.Add((Control) this._optionNameReplacemenetComboBox, 1, 0);
    this.tableLayoutPanel2.Controls.Add((Control) this._optionValueReplacementComboBox, 1, 1);
    this.tableLayoutPanel2.Controls.Add((Control) this._relationOperatorDisplayTypeComboBox, 1, 2);
    this.tableLayoutPanel2.Controls.Add((Control) this.label3, 0, 2);
    this.tableLayoutPanel2.Dock = DockStyle.Fill;
    this.tableLayoutPanel2.Location = new Point(3, 3);
    this.tableLayoutPanel2.Name = "tableLayoutPanel2";
    this.tableLayoutPanel2.RowCount = 3;
    this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333f));
    this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33334f));
    this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33334f));
    this.tableLayoutPanel2.Size = new Size(544, 84);
    this.tableLayoutPanel2.TabIndex = 1;
    this.label1.AutoSize = true;
    this.label1.Location = new Point(3, 0);
    this.label1.Name = "label1";
    this.label1.Size = new Size(207, 13);
    this.label1.TabIndex = 0;
    this.label1.Text = "Вместо наименования опции выводить";
    this.label2.AutoSize = true;
    this.label2.Location = new Point(3, 27);
    this.label2.Name = "label2";
    this.label2.Size = new Size(180, 13);
    this.label2.TabIndex = 0;
    this.label2.Text = "Вместо значения опции выводить";
    this._optionNameReplacemenetComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
    this._optionNameReplacemenetComboBox.FormattingEnabled = true;
    this._optionNameReplacemenetComboBox.Location = new Point(275, 3);
    this._optionNameReplacemenetComboBox.Name = "_optionNameReplacemenetComboBox";
    this._optionNameReplacemenetComboBox.Size = new Size(121, 21);
    this._optionNameReplacemenetComboBox.TabIndex = 1;
    this._optionNameReplacemenetComboBox.SelectedIndexChanged += new EventHandler(this.OptionNameReplacemenetComboBox_SelectedIndexChanged);
    this._optionValueReplacementComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
    this._optionValueReplacementComboBox.FormattingEnabled = true;
    this._optionValueReplacementComboBox.Location = new Point(275, 30);
    this._optionValueReplacementComboBox.Name = "_optionValueReplacementComboBox";
    this._optionValueReplacementComboBox.Size = new Size(121, 21);
    this._optionValueReplacementComboBox.TabIndex = 1;
    this._optionValueReplacementComboBox.SelectedIndexChanged += new EventHandler(this.OptionValueReplacementComboBox_SelectedIndexChanged);
    this._relationOperatorDisplayTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
    this._relationOperatorDisplayTypeComboBox.FormattingEnabled = true;
    this._relationOperatorDisplayTypeComboBox.Location = new Point(275, 58);
    this._relationOperatorDisplayTypeComboBox.Name = "_relationOperatorDisplayTypeComboBox";
    this._relationOperatorDisplayTypeComboBox.Size = new Size(121, 21);
    this._relationOperatorDisplayTypeComboBox.TabIndex = 2;
    this._relationOperatorDisplayTypeComboBox.SelectedIndexChanged += new EventHandler(this.RelationOperatorDisplayTypeComboBox_SelectedIndexChanged);
    this.label3.AutoSize = true;
    this.label3.Location = new Point(3, 55);
    this.label3.Name = "label3";
    this.label3.Size = new Size((int) byte.MaxValue, 13);
    this.label3.TabIndex = 3;
    this.label3.Text = "Вместо значения операции сравнения выводить";
    this.AcceptButton = (IButtonControl) this._applyButton;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this._cancelButton;
    this.ClientSize = new Size(550, 130);
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.Name = nameof (ApplicationConditionsDisplaySettingsForm);
    this.ShowIcon = false;
    this.Text = "Настройки отображения условий применения";
    this.FormClosing += new FormClosingEventHandler(this.ApplicationConditionsDisplaySettingsForm_FormClosing);
    this.Load += new EventHandler(this.ApplicationConditionsDisplaySettingsForm_Load);
    this.tableLayoutPanel1.ResumeLayout(false);
    this.flowLayoutPanel1.ResumeLayout(false);
    this.tableLayoutPanel2.ResumeLayout(false);
    this.tableLayoutPanel2.PerformLayout();
    this.ResumeLayout(false);
  }
}
