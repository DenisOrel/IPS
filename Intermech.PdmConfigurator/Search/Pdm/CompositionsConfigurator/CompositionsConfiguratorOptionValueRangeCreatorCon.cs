// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.CompositionsConfigurator.CompositionsConfiguratorOptionValueRangeCreatorControl
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Pdm.CompositionsConfigurator;

public class CompositionsConfiguratorOptionValueRangeCreatorControl : UserControl
{
  private const string Arithmetic = "Арифметическая";
  private const string Geometry = "Геометрическая";
  private CompositionsConfiguratorOptionValueRangeCreatorControl.CompositionsConfiguratorOptionValueRangeCreatorControlProgressionType _progressionType;
  private double _startValue;
  private bool _isValidStartValue;
  private double _step;
  private bool _isValidStep;
  private IContainer components;
  private ComboBox _progressionTypeComboBox;
  private Label label1;
  private Label label2;
  private Label label3;
  private Label label4;
  private Button _cancelButton;
  private Button _okButton;
  private NumericUpDown _countNumericUpDown;
  private TextBox _stepTextBox;
  private TextBox _startValueTextBox;
  private ErrorProvider _startValueTextBoxErrorProvider;
  private ErrorProvider _stepTextBoxErrorProvider;

  public CompositionsConfiguratorOptionValueRangeCreatorControl()
  {
    this.InitializeComponent();
    this._progressionTypeComboBox.Items.Add((object) "Арифметическая");
    this._progressionTypeComboBox.Items.Add((object) "Геометрическая");
    this.SetProgressionType();
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public CompositionsConfiguratorOptionValueRangeCreatorControl.CompositionsConfiguratorOptionValueRangeCreatorControlProgressionType ProgressionType
  {
    get => this._progressionType;
    set
    {
      if (this._progressionType == value)
        return;
      this._progressionType = value;
      this.SetProgressionType();
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public double StartValue
  {
    get => this._startValue;
    set
    {
      if (this._startValue == value)
        return;
      this._startValue = value;
      this.SetStartValue();
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public double Step
  {
    get => this._step;
    set
    {
      if (this._step == value)
        return;
      this._step = value;
      this.SetStep();
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public int Count
  {
    get => (int) this._countNumericUpDown.Value;
    set => this._countNumericUpDown.Value = (Decimal) value;
  }

  public double[] CreateValueRange()
  {
    List<double> doubleList = new List<double>();
    for (int y = 0; y < this.Count; ++y)
    {
      if (this.ProgressionType == CompositionsConfiguratorOptionValueRangeCreatorControl.CompositionsConfiguratorOptionValueRangeCreatorControlProgressionType.Arithmetic)
      {
        double num = this.StartValue + this.Step * (double) y;
        doubleList.Add(num);
      }
      else
      {
        double num = this.StartValue * Math.Pow(this.Step, (double) y);
        doubleList.Add(num);
      }
    }
    return doubleList.ToArray();
  }

  private void ProgressionTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
  {
    if (object.Equals(this._progressionTypeComboBox.SelectedItem, (object) "Арифметическая"))
      this._progressionType = CompositionsConfiguratorOptionValueRangeCreatorControl.CompositionsConfiguratorOptionValueRangeCreatorControlProgressionType.Arithmetic;
    else
      this._progressionType = CompositionsConfiguratorOptionValueRangeCreatorControl.CompositionsConfiguratorOptionValueRangeCreatorControlProgressionType.Geometry;
  }

  private void StartValueTextBox_TextChanged(object sender, EventArgs e)
  {
    this._isValidStartValue = double.TryParse(this._startValueTextBox.Text, out this._startValue);
    this.SetStartValueTextBoxError();
    this.SetOKButtonEnabled();
  }

  private void StepTextBox_TextChanged(object sender, EventArgs e)
  {
    this._isValidStep = double.TryParse(this._stepTextBox.Text, out this._step);
    this.SetStepTextBoxError();
    this.SetOKButtonEnabled();
  }

  private void SetProgressionType()
  {
    if (this._progressionType == CompositionsConfiguratorOptionValueRangeCreatorControl.CompositionsConfiguratorOptionValueRangeCreatorControlProgressionType.Arithmetic)
      this._progressionTypeComboBox.SelectedText = "Арифметическая";
    else
      this._progressionTypeComboBox.SelectedText = "Геометрическая";
  }

  private void SetStartValue() => this._startValueTextBox.Text = this._startValue.ToString();

  private void SetStep() => this._stepTextBox.Text = this._step.ToString();

  private void SetStartValueTextBoxError()
  {
    this._startValueTextBoxErrorProvider.Clear();
    if (this._isValidStartValue)
      return;
    this._startValueTextBoxErrorProvider.SetError((Control) this._startValueTextBox, "Не число");
  }

  private void SetStepTextBoxError()
  {
    this._stepTextBoxErrorProvider.Clear();
    if (this._isValidStep)
      return;
    this._stepTextBoxErrorProvider.SetError((Control) this._stepTextBox, "Не число");
  }

  private void SetOKButtonEnabled()
  {
    this._okButton.Enabled = this._isValidStartValue && this._isValidStep;
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
    this._progressionTypeComboBox = new ComboBox();
    this.label1 = new Label();
    this.label2 = new Label();
    this.label3 = new Label();
    this.label4 = new Label();
    this._cancelButton = new Button();
    this._okButton = new Button();
    this._countNumericUpDown = new NumericUpDown();
    this._stepTextBox = new TextBox();
    this._startValueTextBox = new TextBox();
    this._startValueTextBoxErrorProvider = new ErrorProvider(this.components);
    this._stepTextBoxErrorProvider = new ErrorProvider(this.components);
    this._countNumericUpDown.BeginInit();
    ((ISupportInitialize) this._startValueTextBoxErrorProvider).BeginInit();
    ((ISupportInitialize) this._stepTextBoxErrorProvider).BeginInit();
    this.SuspendLayout();
    this._progressionTypeComboBox.FormattingEnabled = true;
    this._progressionTypeComboBox.Location = new Point(100, 11);
    this._progressionTypeComboBox.Name = "_progressionTypeComboBox";
    this._progressionTypeComboBox.Size = new Size(121, 21);
    this._progressionTypeComboBox.TabIndex = 0;
    this._progressionTypeComboBox.SelectedIndexChanged += new EventHandler(this.ProgressionTypeComboBox_SelectedIndexChanged);
    this.label1.AutoSize = true;
    this.label1.Location = new Point(3, 14);
    this.label1.Name = "label1";
    this.label1.Size = new Size(91, 13);
    this.label1.TabIndex = 1;
    this.label1.Text = "Тип прогрессии:";
    this.label2.AutoSize = true;
    this.label2.Location = new Point(3, 43);
    this.label2.Name = "label2";
    this.label2.Size = new Size(115, 13);
    this.label2.TabIndex = 2;
    this.label2.Text = "Начальное значение:";
    this.label3.AutoSize = true;
    this.label3.Location = new Point(287, 43);
    this.label3.Name = "label3";
    this.label3.Size = new Size(30, 13);
    this.label3.TabIndex = 2;
    this.label3.Text = "Шаг:";
    this.label4.AutoSize = true;
    this.label4.Location = new Point(490, 43);
    this.label4.Name = "label4";
    this.label4.Size = new Size(69, 13);
    this.label4.TabIndex = 2;
    this.label4.Text = "Количество:";
    this._cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this._cancelButton.DialogResult = DialogResult.Cancel;
    this._cancelButton.Location = new Point(610, 95);
    this._cancelButton.Name = "_cancelButton";
    this._cancelButton.Size = new Size(75, 23);
    this._cancelButton.TabIndex = 4;
    this._cancelButton.Text = "Отмена";
    this._cancelButton.UseVisualStyleBackColor = true;
    this._okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this._okButton.DialogResult = DialogResult.OK;
    this._okButton.Location = new Point(529, 95);
    this._okButton.Name = "_okButton";
    this._okButton.Size = new Size(75, 23);
    this._okButton.TabIndex = 5;
    this._okButton.Text = "ОК";
    this._okButton.UseVisualStyleBackColor = true;
    this._countNumericUpDown.Location = new Point(565, 41);
    this._countNumericUpDown.Minimum = new Decimal(new int[4]
    {
      1,
      0,
      0,
      0
    });
    this._countNumericUpDown.Name = "_countNumericUpDown";
    this._countNumericUpDown.Size = new Size(120, 20);
    this._countNumericUpDown.TabIndex = 6;
    this._countNumericUpDown.Value = new Decimal(new int[4]
    {
      1,
      0,
      0,
      0
    });
    this._stepTextBox.Location = new Point(323, 40);
    this._stepTextBox.Name = "_stepTextBox";
    this._stepTextBox.Size = new Size(120, 20);
    this._stepTextBox.TabIndex = 7;
    this._stepTextBox.TextChanged += new EventHandler(this.StepTextBox_TextChanged);
    this._startValueTextBox.Location = new Point(124, 40);
    this._startValueTextBox.Name = "_startValueTextBox";
    this._startValueTextBox.Size = new Size(129, 20);
    this._startValueTextBox.TabIndex = 7;
    this._startValueTextBox.TextChanged += new EventHandler(this.StartValueTextBox_TextChanged);
    this._startValueTextBoxErrorProvider.ContainerControl = (ContainerControl) this;
    this._stepTextBoxErrorProvider.ContainerControl = (ContainerControl) this;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this._startValueTextBox);
    this.Controls.Add((Control) this._stepTextBox);
    this.Controls.Add((Control) this._countNumericUpDown);
    this.Controls.Add((Control) this._okButton);
    this.Controls.Add((Control) this._cancelButton);
    this.Controls.Add((Control) this.label4);
    this.Controls.Add((Control) this.label3);
    this.Controls.Add((Control) this.label2);
    this.Controls.Add((Control) this.label1);
    this.Controls.Add((Control) this._progressionTypeComboBox);
    this.Name = nameof (CompositionsConfiguratorOptionValueRangeCreatorControl);
    this.Size = new Size(693, 121);
    this._countNumericUpDown.EndInit();
    ((ISupportInitialize) this._startValueTextBoxErrorProvider).EndInit();
    ((ISupportInitialize) this._stepTextBoxErrorProvider).EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  public enum CompositionsConfiguratorOptionValueRangeCreatorControlProgressionType
  {
    Arithmetic,
    Geometry,
  }
}
