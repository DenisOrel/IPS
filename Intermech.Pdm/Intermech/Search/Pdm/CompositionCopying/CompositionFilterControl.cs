// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.CompositionCopying.CompositionFilterControl
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Pdm.CompositionCopying;

public sealed class CompositionFilterControl : UserControl
{
  private IContainer components;
  private TableLayoutPanel tableLayoutPanel11;
  private CheckBox _considerInstancesCheckBox;

  public CompositionFilterControl() => this.InitializeComponent();

  public event EventHandler ConsiderInstancesChanged;

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public bool ConsiderInstances => this._considerInstancesCheckBox.Checked;

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public bool ConsiderInstancesCheckBoxEnabled
  {
    get => this._considerInstancesCheckBox.Enabled;
    set => this._considerInstancesCheckBox.Enabled = value;
  }

  private void ConsiderInstancesCheckBox_CheckedChanged(object sender, EventArgs e)
  {
    EventHandler instancesChanged = this.ConsiderInstancesChanged;
    if (instancesChanged == null)
      return;
    instancesChanged((object) this, EventArgs.Empty);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.tableLayoutPanel11 = new TableLayoutPanel();
    this._considerInstancesCheckBox = new CheckBox();
    this.tableLayoutPanel11.SuspendLayout();
    this.SuspendLayout();
    this.tableLayoutPanel11.AutoSize = true;
    this.tableLayoutPanel11.AutoSizeMode = AutoSizeMode.GrowAndShrink;
    this.tableLayoutPanel11.ColumnCount = 1;
    this.tableLayoutPanel11.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel11.Controls.Add((Control) this._considerInstancesCheckBox, 0, 0);
    this.tableLayoutPanel11.Dock = DockStyle.Fill;
    this.tableLayoutPanel11.Location = new Point(0, 0);
    this.tableLayoutPanel11.Name = "tableLayoutPanel11";
    this.tableLayoutPanel11.RowCount = 2;
    this.tableLayoutPanel11.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel11.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel11.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
    this.tableLayoutPanel11.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
    this.tableLayoutPanel11.Size = new Size(357, 311);
    this.tableLayoutPanel11.TabIndex = 0;
    this._considerInstancesCheckBox.AutoSize = true;
    this._considerInstancesCheckBox.Dock = DockStyle.Fill;
    this._considerInstancesCheckBox.Location = new Point(3, 3);
    this._considerInstancesCheckBox.Name = "_considerInstancesCheckBox";
    this._considerInstancesCheckBox.Size = new Size(351, 17);
    this._considerInstancesCheckBox.TabIndex = 1;
    this._considerInstancesCheckBox.Text = "Копировать исполнения";
    this._considerInstancesCheckBox.UseVisualStyleBackColor = true;
    this._considerInstancesCheckBox.CheckedChanged += new EventHandler(this.ConsiderInstancesCheckBox_CheckedChanged);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.tableLayoutPanel11);
    this.Name = nameof (CompositionFilterControl);
    this.Size = new Size(357, 311);
    this.tableLayoutPanel11.ResumeLayout(false);
    this.tableLayoutPanel11.PerformLayout();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
