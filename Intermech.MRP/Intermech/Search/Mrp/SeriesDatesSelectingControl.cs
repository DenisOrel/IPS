// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Mrp.SeriesDatesSelectingControl
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.Interfaces.Compositions;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Mrp;

public sealed class SeriesDatesSelectingControl : UserControl
{
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private TableLayoutPanel tableLayoutPanel1;
  private PictureBox pictureBox2;
  private Intermech.Search.Pdm.SeriesDates.SeriesDatesSelectingControl _seriesDatesSelectingControl;

  public SeriesDatesSelectingControl() => this.InitializeComponent();

  public event EventHandler Changed;

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public SeriesDateSettingsHolder SeriesDateSettingsHolder
  {
    get => this._seriesDatesSelectingControl.SeriesDateSettingsHolder;
  }

  private void SeriesDatesSelectingControl_Changed(object sender, EventArgs e) => this.OnChanged();

  private void OnChanged()
  {
    EventHandler changed = this.Changed;
    if (changed == null)
      return;
    changed((object) this, EventArgs.Empty);
  }

  public int PictureBoxWidth
  {
    get => this.pictureBox2.Width;
    set
    {
      this.pictureBox2.SizeMode = PictureBoxSizeMode.CenterImage;
      this.pictureBox2.Width = value;
    }
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SeriesDatesSelectingControl));
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this.pictureBox2 = new PictureBox();
    this._seriesDatesSelectingControl = new Intermech.Search.Pdm.SeriesDates.SeriesDatesSelectingControl();
    this.tableLayoutPanel1.SuspendLayout();
    ((ISupportInitialize) this.pictureBox2).BeginInit();
    this.SuspendLayout();
    this.tableLayoutPanel1.ColumnCount = 2;
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20f));
    this.tableLayoutPanel1.Controls.Add((Control) this.pictureBox2, 0, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this._seriesDatesSelectingControl, 1, 0);
    this.tableLayoutPanel1.Dock = DockStyle.Fill;
    this.tableLayoutPanel1.Location = new Point(0, 0);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.RowCount = 1;
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.Size = new Size(471, 118);
    this.tableLayoutPanel1.TabIndex = 1;
    this.pictureBox2.Dock = DockStyle.Fill;
    this.pictureBox2.Image = (Image) componentResourceManager.GetObject("pictureBox2.Image");
    this.pictureBox2.ImeMode = ImeMode.NoControl;
    this.pictureBox2.Location = new Point(3, 3);
    this.pictureBox2.Name = "pictureBox2";
    this.pictureBox2.Size = new Size(48 /*0x30*/, 112 /*0x70*/);
    this.pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
    this.pictureBox2.TabIndex = 0;
    this.pictureBox2.TabStop = false;
    this._seriesDatesSelectingControl.AutoSize = true;
    this._seriesDatesSelectingControl.BackColor = SystemColors.ControlLightLight;
    this._seriesDatesSelectingControl.BorderStyle = BorderStyle.FixedSingle;
    this._seriesDatesSelectingControl.Dock = DockStyle.Fill;
    this._seriesDatesSelectingControl.Location = new Point(57, 3);
    this._seriesDatesSelectingControl.Name = "_seriesDatesSelectingControl";
    this._seriesDatesSelectingControl.Size = new Size(411, 112 /*0x70*/);
    this._seriesDatesSelectingControl.TabIndex = 2;
    this._seriesDatesSelectingControl.Changed += new EventHandler(this.SeriesDatesSelectingControl_Changed);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.Name = nameof (SeriesDatesSelectingControl);
    this.Size = new Size(471, 118);
    this.tableLayoutPanel1.ResumeLayout(false);
    this.tableLayoutPanel1.PerformLayout();
    ((ISupportInitialize) this.pictureBox2).EndInit();
    this.ResumeLayout(false);
  }
}
