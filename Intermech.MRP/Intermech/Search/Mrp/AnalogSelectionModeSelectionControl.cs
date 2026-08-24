// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Mrp.AnalogSelectionModeSelectionControl
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.Search.Pdm.Analogs;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Mrp;

public sealed class AnalogSelectionModeSelectionControl : UserControl
{
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private Intermech.Search.Pdm.Analogs.AnalogSelectionModeSelectionControl _analogSelectionModeSelectionControl;
  private TableLayoutPanel tableLayoutPanel1;
  private PictureBox pictureBox1;

  public AnalogSelectionModeSelectionControl() => this.InitializeComponent();

  public event EventHandler AnalogSelectionModeChanged;

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public AnalogSelectionMode AnalogSelectionMode
  {
    get => this._analogSelectionModeSelectionControl.AnalogSelectionMode;
  }

  private void AnalogSelectionModeSelectionControl_AnalogSelectionModeChanged(
    object sender,
    EventArgs e)
  {
    EventHandler selectionModeChanged = this.AnalogSelectionModeChanged;
    if (selectionModeChanged == null)
      return;
    selectionModeChanged((object) this, EventArgs.Empty);
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AnalogSelectionModeSelectionControl));
    this._analogSelectionModeSelectionControl = new Intermech.Search.Pdm.Analogs.AnalogSelectionModeSelectionControl();
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this.pictureBox1 = new PictureBox();
    this.tableLayoutPanel1.SuspendLayout();
    ((ISupportInitialize) this.pictureBox1).BeginInit();
    this.SuspendLayout();
    this._analogSelectionModeSelectionControl.Dock = DockStyle.Fill;
    this._analogSelectionModeSelectionControl.Location = new Point(57, 3);
    this._analogSelectionModeSelectionControl.Name = "_analogSelectionModeSelectionControl";
    this._analogSelectionModeSelectionControl.Size = new Size(389, 243);
    this._analogSelectionModeSelectionControl.TabIndex = 0;
    this._analogSelectionModeSelectionControl.AnalogSelectionModeChanged += new EventHandler(this.AnalogSelectionModeSelectionControl_AnalogSelectionModeChanged);
    this.tableLayoutPanel1.ColumnCount = 2;
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.Controls.Add((Control) this._analogSelectionModeSelectionControl, 1, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this.pictureBox1, 0, 0);
    this.tableLayoutPanel1.Dock = DockStyle.Fill;
    this.tableLayoutPanel1.Location = new Point(0, 0);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.RowCount = 1;
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.Size = new Size(449, 249);
    this.tableLayoutPanel1.TabIndex = 1;
    this.pictureBox1.Dock = DockStyle.Fill;
    this.pictureBox1.Image = (Image) componentResourceManager.GetObject("pictureBox1.Image");
    this.pictureBox1.Location = new Point(3, 3);
    this.pictureBox1.Name = "pictureBox1";
    this.pictureBox1.Size = new Size(48 /*0x30*/, 243);
    this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
    this.pictureBox1.TabIndex = 1;
    this.pictureBox1.TabStop = false;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.Name = nameof (AnalogSelectionModeSelectionControl);
    this.Size = new Size(449, 249);
    this.tableLayoutPanel1.ResumeLayout(false);
    this.tableLayoutPanel1.PerformLayout();
    ((ISupportInitialize) this.pictureBox1).EndInit();
    this.ResumeLayout(false);
  }
}
