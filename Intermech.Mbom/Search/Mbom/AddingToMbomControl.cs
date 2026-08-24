// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Mbom.AddingToMbomControl
// Assembly: Intermech.Mbom, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 13559C9A-4DBC-479B-BA71-AFEA0247DEC7
// Assembly location: D:\IPS\Client\Intermech.Mbom.dll
// XML documentation location: D:\IPS\Client\Intermech.Mbom.xml

using Intermech.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Mbom;

public sealed class AddingToMbomControl : UserControl
{
  private MeasuredValue _count;
  private MeasuredValue _remainingCount;
  private MeasuredValue _totalCount;
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private TableLayoutPanel tableLayoutPanel1;
  private TableLayoutPanel tableLayoutPanel2;
  private Label label4;
  private TableLayoutPanel tableLayoutPanel4;
  private Label label2;
  private TextBox _remainingCountTextBox;
  private TableLayoutPanel tableLayoutPanel5;
  private Label label1;
  private TextBox _totalCountTextBox;
  private TableLayoutPanel tableLayoutPanel3;
  private TextBox _countTextBox;
  private Label _measureLabel;
  private ErrorProvider _errorProvider;

  public AddingToMbomControl() => this.InitializeComponent();

  public event EventHandler Changed;

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public MeasuredValue Count
  {
    get => this._count;
    set
    {
      if (this._count == value)
        return;
      this._count = value;
      this._countTextBox.TextChanged -= new EventHandler(this.CountTextBox_TextChanged);
      try
      {
        this._countTextBox.Text = this.ConvertMeasuredValueToString(this._count);
      }
      finally
      {
        this._countTextBox.TextChanged += new EventHandler(this.CountTextBox_TextChanged);
      }
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public MeasuredValue RemainingCount
  {
    get => this._remainingCount;
    set
    {
      if (this._remainingCount == value)
        return;
      this._remainingCount = value;
      this._remainingCountTextBox.Text = this.ConvertMeasuredValueToString(this._remainingCount);
      this.SetMeasureLabel();
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public MeasuredValue TotalCount
  {
    get => this._totalCount;
    set
    {
      if (this._totalCount == value)
        return;
      this._totalCount = value;
      this._totalCountTextBox.Text = this.ConvertMeasuredValueToString(this._totalCount);
      this.SetMeasureLabel();
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public bool HasErrors { get; private set; }

  private void CountTextBox_TextChanged(object sender, EventArgs e)
  {
    double result = 0.0;
    double.TryParse(this._countTextBox.Text, out result);
    long measureID = this._count != null ? this._count.MeasureID : (this._remainingCount != null ? this._remainingCount.MeasureID : (this._totalCount != null ? this._totalCount.MeasureID : 0L));
    this._count = new MeasuredValue(result, measureID);
    this.HasErrors = false;
    this._errorProvider.Clear();
    if (this._count.Value <= 0.0)
    {
      this.HasErrors = true;
      this._errorProvider.SetError((Control) this._countTextBox, "Количество должно быть больше нуля.");
    }
    if (this.IsCountGreatThanRemainingCount())
    {
      this.HasErrors = true;
      this._errorProvider.SetError((Control) this._countTextBox, "Количество не может быть больше оставшегося количества.");
    }
    this.OnChanged();
  }

  private string ConvertMeasuredValueToString(MeasuredValue measuredValue)
  {
    return measuredValue == null ? string.Empty : measuredValue.ToString();
  }

  private bool IsCountGreatThanRemainingCount()
  {
    return this._count != null && this._remainingCount != null && this._count.Value > this._remainingCount.Value;
  }

  private void OnChanged()
  {
    EventHandler changed = this.Changed;
    if (changed == null)
      return;
    changed((object) this, EventArgs.Empty);
  }

  private void SetMeasureLabel()
  {
    if (this._totalCount != null)
    {
      this._measureLabel.Text = this.GetMeasureShortName(this._totalCount);
    }
    else
    {
      if (this._remainingCount == null)
        return;
      this._measureLabel.Text = this.GetMeasureShortName(this._remainingCount);
    }
  }

  private string GetMeasureShortName(MeasuredValue measuredValue)
  {
    return MeasureHelper.FindDescriptor(measuredValue)?.ShortName;
  }

  /// <summary>Clean up any resources being used.</summary>
  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  /// <summary>
  /// Required method for Designer support - do not modify
  /// the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this.tableLayoutPanel2 = new TableLayoutPanel();
    this.label4 = new Label();
    this.tableLayoutPanel3 = new TableLayoutPanel();
    this._countTextBox = new TextBox();
    this._measureLabel = new Label();
    this.tableLayoutPanel4 = new TableLayoutPanel();
    this.label2 = new Label();
    this._remainingCountTextBox = new TextBox();
    this.tableLayoutPanel5 = new TableLayoutPanel();
    this.label1 = new Label();
    this._totalCountTextBox = new TextBox();
    this._errorProvider = new ErrorProvider(this.components);
    this.tableLayoutPanel1.SuspendLayout();
    this.tableLayoutPanel2.SuspendLayout();
    this.tableLayoutPanel3.SuspendLayout();
    this.tableLayoutPanel4.SuspendLayout();
    this.tableLayoutPanel5.SuspendLayout();
    ((ISupportInitialize) this._errorProvider).BeginInit();
    this.SuspendLayout();
    this.tableLayoutPanel1.ColumnCount = 3;
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333f));
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333f));
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333f));
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20f));
    this.tableLayoutPanel1.Controls.Add((Control) this.tableLayoutPanel2, 0, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this.tableLayoutPanel4, 1, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this.tableLayoutPanel5, 2, 0);
    this.tableLayoutPanel1.Dock = DockStyle.Fill;
    this.tableLayoutPanel1.Location = new Point(0, 0);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.RowCount = 1;
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.Size = new Size(481, 150);
    this.tableLayoutPanel1.TabIndex = 0;
    this.tableLayoutPanel2.ColumnCount = 1;
    this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
    this.tableLayoutPanel2.Controls.Add((Control) this.label4, 0, 0);
    this.tableLayoutPanel2.Controls.Add((Control) this.tableLayoutPanel3, 0, 1);
    this.tableLayoutPanel2.Dock = DockStyle.Fill;
    this.tableLayoutPanel2.Location = new Point(3, 3);
    this.tableLayoutPanel2.Name = "tableLayoutPanel2";
    this.tableLayoutPanel2.RowCount = 2;
    this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
    this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
    this.tableLayoutPanel2.Size = new Size(154, 144 /*0x90*/);
    this.tableLayoutPanel2.TabIndex = 0;
    this.label4.AutoSize = true;
    this.label4.Location = new Point(3, 0);
    this.label4.Name = "label4";
    this.label4.Size = new Size(108, 13);
    this.label4.TabIndex = 0;
    this.label4.Text = "Объектов в сборке:";
    this.tableLayoutPanel3.ColumnCount = 2;
    this.tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40f));
    this.tableLayoutPanel3.Controls.Add((Control) this._countTextBox, 0, 0);
    this.tableLayoutPanel3.Controls.Add((Control) this._measureLabel, 1, 0);
    this.tableLayoutPanel3.Location = new Point(3, 75);
    this.tableLayoutPanel3.Name = "tableLayoutPanel3";
    this.tableLayoutPanel3.RowCount = 1;
    this.tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel3.Size = new Size(148, 66);
    this.tableLayoutPanel3.TabIndex = 1;
    this._countTextBox.Dock = DockStyle.Fill;
    this._countTextBox.Location = new Point(3, 3);
    this._countTextBox.Name = "_countTextBox";
    this._countTextBox.Size = new Size(102, 20);
    this._countTextBox.TabIndex = 0;
    this._countTextBox.TextChanged += new EventHandler(this.CountTextBox_TextChanged);
    this._measureLabel.AutoSize = true;
    this._measureLabel.Location = new Point(111, 0);
    this._measureLabel.Name = "_measureLabel";
    this._measureLabel.Size = new Size(29, 26);
    this._measureLabel.TabIndex = 1;
    this._measureLabel.Text = "label3";
    this.tableLayoutPanel4.ColumnCount = 1;
    this.tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
    this.tableLayoutPanel4.Controls.Add((Control) this.label2, 0, 0);
    this.tableLayoutPanel4.Controls.Add((Control) this._remainingCountTextBox, 0, 1);
    this.tableLayoutPanel4.Dock = DockStyle.Fill;
    this.tableLayoutPanel4.Location = new Point(163, 3);
    this.tableLayoutPanel4.Name = "tableLayoutPanel4";
    this.tableLayoutPanel4.RowCount = 2;
    this.tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
    this.tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
    this.tableLayoutPanel4.Size = new Size(154, 144 /*0x90*/);
    this.tableLayoutPanel4.TabIndex = 0;
    this.label2.AutoSize = true;
    this.label2.Location = new Point(3, 0);
    this.label2.Name = "label2";
    this.label2.Size = new Size(109, 13);
    this.label2.TabIndex = 0;
    this.label2.Text = "Оставшееся кол-во:";
    this._remainingCountTextBox.Dock = DockStyle.Fill;
    this._remainingCountTextBox.Location = new Point(3, 75);
    this._remainingCountTextBox.Name = "_remainingCountTextBox";
    this._remainingCountTextBox.ReadOnly = true;
    this._remainingCountTextBox.Size = new Size(148, 20);
    this._remainingCountTextBox.TabIndex = 1;
    this.tableLayoutPanel5.ColumnCount = 1;
    this.tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
    this.tableLayoutPanel5.Controls.Add((Control) this.label1, 0, 0);
    this.tableLayoutPanel5.Controls.Add((Control) this._totalCountTextBox, 0, 1);
    this.tableLayoutPanel5.Dock = DockStyle.Fill;
    this.tableLayoutPanel5.Location = new Point(323, 3);
    this.tableLayoutPanel5.Name = "tableLayoutPanel5";
    this.tableLayoutPanel5.RowCount = 2;
    this.tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
    this.tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
    this.tableLayoutPanel5.Size = new Size(155, 144 /*0x90*/);
    this.tableLayoutPanel5.TabIndex = 0;
    this.label1.AutoSize = true;
    this.label1.Location = new Point(3, 0);
    this.label1.Name = "label1";
    this.label1.Size = new Size(81, 13);
    this.label1.TabIndex = 0;
    this.label1.Text = "Общее кол-во:";
    this._totalCountTextBox.Dock = DockStyle.Fill;
    this._totalCountTextBox.Location = new Point(3, 75);
    this._totalCountTextBox.Name = "_totalCountTextBox";
    this._totalCountTextBox.ReadOnly = true;
    this._totalCountTextBox.Size = new Size(149, 20);
    this._totalCountTextBox.TabIndex = 1;
    this._errorProvider.ContainerControl = (ContainerControl) this;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.Name = nameof (AddingToMbomControl);
    this.Size = new Size(481, 150);
    this.tableLayoutPanel1.ResumeLayout(false);
    this.tableLayoutPanel2.ResumeLayout(false);
    this.tableLayoutPanel2.PerformLayout();
    this.tableLayoutPanel3.ResumeLayout(false);
    this.tableLayoutPanel3.PerformLayout();
    this.tableLayoutPanel4.ResumeLayout(false);
    this.tableLayoutPanel4.PerformLayout();
    this.tableLayoutPanel5.ResumeLayout(false);
    this.tableLayoutPanel5.PerformLayout();
    ((ISupportInitialize) this._errorProvider).EndInit();
    this.ResumeLayout(false);
  }
}
