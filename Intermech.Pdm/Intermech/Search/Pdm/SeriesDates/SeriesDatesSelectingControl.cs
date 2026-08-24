// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.SeriesDates.SeriesDatesSelectingControl
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Compositions;
using Intermech.Search.Utilities;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Pdm.SeriesDates;

public sealed class SeriesDatesSelectingControl : UserControl
{
  private IContainer components;
  private FlowLayoutPanel flowLayoutPanel1;
  private TableLayoutPanel tableLayoutPanel1;
  private Label label1;
  private ObjectLinkComboBox _objectLinkComboBox;
  private TableLayoutPanel tableLayoutPanel2;
  private Label label2;
  private Int32ComboBox _int32ComboBox;
  private TableLayoutPanel tableLayoutPanel3;
  private Label label3;
  private DateTimeComboBox _dateTimeComboBox;

  public SeriesDatesSelectingControl()
  {
    this.InitializeComponent();
    this._objectLinkComboBox.AllowEmpty = true;
    this._objectLinkComboBox.ObjectTypeID = SeriesDatesConstants.HeadProductObjectTypeID;
    this._int32ComboBox.AllowEmpty = true;
    this._dateTimeComboBox.AllowEmpty = true;
  }

  public event EventHandler Changed;

  public long HeadProduct => this._objectLinkComboBox.TypedValue;

  public bool HasSeries => !this._int32ComboBox.IsEmpty;

  public int Series => this._int32ComboBox.TypedValue;

  public bool HasDate => !this._dateTimeComboBox.IsEmpty;

  public DateTime Date => this._dateTimeComboBox.TypedValue.Date;

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public SeriesDateSettingsHolder SeriesDateSettingsHolder { get; private set; }

  private void ObjectLinkComboBox_ValueChanged(object sender, EventArgs e)
  {
    this.SetSeriesDateSettingsHolder();
    this.OnChanged();
  }

  private void Int32ComboBox_ValueChanged(object sender, EventArgs e)
  {
    this.SetSeriesDateSettingsHolder();
    this.OnChanged();
  }

  private void DateTimeComboBox_ValueChanged(object sender, EventArgs e)
  {
    this.SetSeriesDateSettingsHolder();
    this.OnChanged();
  }

  private void SetSeriesDateSettingsHolder()
  {
    if (!ObjectHelper.IsUnknownObjectVersionID(this.HeadProduct))
      this.SeriesDateSettingsHolder = new SeriesDateSettingsHolder(true, this.HeadProduct, this.HasDate ? this.Date : DateTime.MinValue, this.HasSeries ? this.Series : int.MinValue);
    else
      this.SeriesDateSettingsHolder = (SeriesDateSettingsHolder) null;
  }

  private void OnChanged()
  {
    EventHandler changed = this.Changed;
    if (changed == null)
      return;
    changed((object) this, EventArgs.Empty);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.flowLayoutPanel1 = new FlowLayoutPanel();
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this.label1 = new Label();
    this._objectLinkComboBox = new ObjectLinkComboBox();
    this.tableLayoutPanel2 = new TableLayoutPanel();
    this.label2 = new Label();
    this._int32ComboBox = new Int32ComboBox();
    this.tableLayoutPanel3 = new TableLayoutPanel();
    this.label3 = new Label();
    this._dateTimeComboBox = new DateTimeComboBox();
    this.flowLayoutPanel1.SuspendLayout();
    this.tableLayoutPanel1.SuspendLayout();
    this.tableLayoutPanel2.SuspendLayout();
    this.tableLayoutPanel3.SuspendLayout();
    this.SuspendLayout();
    this.flowLayoutPanel1.Controls.Add((Control) this.tableLayoutPanel1);
    this.flowLayoutPanel1.Controls.Add((Control) this.tableLayoutPanel2);
    this.flowLayoutPanel1.Controls.Add((Control) this.tableLayoutPanel3);
    this.flowLayoutPanel1.Dock = DockStyle.Fill;
    this.flowLayoutPanel1.Location = new Point(0, 0);
    this.flowLayoutPanel1.Name = "flowLayoutPanel1";
    this.flowLayoutPanel1.Size = new Size(677, 81);
    this.flowLayoutPanel1.TabIndex = 0;
    this.tableLayoutPanel1.AutoSize = true;
    this.tableLayoutPanel1.ColumnCount = 2;
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.Controls.Add((Control) this.label1, 0, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this._objectLinkComboBox, 1, 0);
    this.tableLayoutPanel1.Location = new Point(3, 3);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.RowCount = 1;
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.Size = new Size(315, 27);
    this.tableLayoutPanel1.TabIndex = 0;
    this.label1.AutoSize = true;
    this.label1.Dock = DockStyle.Fill;
    this.label1.Location = new Point(3, 0);
    this.label1.Name = "label1";
    this.label1.Size = new Size(103, 27);
    this.label1.TabIndex = 0;
    this.label1.Text = "Головное изделие:";
    this._objectLinkComboBox.Location = new Point(112 /*0x70*/, 3);
    this._objectLinkComboBox.MinimumSize = new Size(200, 0);
    this._objectLinkComboBox.Name = "_objectLinkComboBox";
    this._objectLinkComboBox.Size = new Size(200, 21);
    this._objectLinkComboBox.TabIndex = 1;
    this._objectLinkComboBox.ValueChanged += new EventHandler(this.ObjectLinkComboBox_ValueChanged);
    this.tableLayoutPanel2.AutoSize = true;
    this.tableLayoutPanel2.ColumnCount = 2;
    this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
    this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel2.Controls.Add((Control) this.label2, 0, 0);
    this.tableLayoutPanel2.Controls.Add((Control) this._int32ComboBox, 1, 0);
    this.tableLayoutPanel2.Location = new Point(324, 3);
    this.tableLayoutPanel2.Name = "tableLayoutPanel2";
    this.tableLayoutPanel2.RowCount = 1;
    this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel2.Size = new Size(253, 27);
    this.tableLayoutPanel2.TabIndex = 1;
    this.label2.AutoSize = true;
    this.label2.Dock = DockStyle.Fill;
    this.label2.Location = new Point(3, 0);
    this.label2.Name = "label2";
    this.label2.Size = new Size(41, 27);
    this.label2.TabIndex = 0;
    this.label2.Text = "Серия:";
    this._int32ComboBox.Location = new Point(50, 3);
    this._int32ComboBox.MinimumSize = new Size(100, 0);
    this._int32ComboBox.Name = "_int32ComboBox";
    this._int32ComboBox.Size = new Size(200, 21);
    this._int32ComboBox.TabIndex = 1;
    this._int32ComboBox.ValueChanged += new EventHandler(this.Int32ComboBox_ValueChanged);
    this.tableLayoutPanel3.AutoSize = true;
    this.tableLayoutPanel3.ColumnCount = 2;
    this.tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle());
    this.tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel3.Controls.Add((Control) this.label3, 0, 0);
    this.tableLayoutPanel3.Controls.Add((Control) this._dateTimeComboBox, 1, 0);
    this.tableLayoutPanel3.Location = new Point(3, 36);
    this.tableLayoutPanel3.Name = "tableLayoutPanel3";
    this.tableLayoutPanel3.RowCount = 1;
    this.tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel3.Size = new Size(248, 27);
    this.tableLayoutPanel3.TabIndex = 2;
    this.label3.AutoSize = true;
    this.label3.Dock = DockStyle.Fill;
    this.label3.Location = new Point(3, 0);
    this.label3.Name = "label3";
    this.label3.Size = new Size(36, 27);
    this.label3.TabIndex = 0;
    this.label3.Text = "Дата:";
    this._dateTimeComboBox.Location = new Point(45, 3);
    this._dateTimeComboBox.MinimumSize = new Size(200, 0);
    this._dateTimeComboBox.Name = "_dateTimeComboBox";
    this._dateTimeComboBox.Size = new Size(200, 21);
    this._dateTimeComboBox.TabIndex = 1;
    this._dateTimeComboBox.ValueChanged += new EventHandler(this.DateTimeComboBox_ValueChanged);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.flowLayoutPanel1);
    this.Name = nameof (SeriesDatesSelectingControl);
    this.Size = new Size(677, 81);
    this.flowLayoutPanel1.ResumeLayout(false);
    this.flowLayoutPanel1.PerformLayout();
    this.tableLayoutPanel1.ResumeLayout(false);
    this.tableLayoutPanel1.PerformLayout();
    this.tableLayoutPanel2.ResumeLayout(false);
    this.tableLayoutPanel2.PerformLayout();
    this.tableLayoutPanel3.ResumeLayout(false);
    this.tableLayoutPanel3.PerformLayout();
    this.ResumeLayout(false);
  }
}
