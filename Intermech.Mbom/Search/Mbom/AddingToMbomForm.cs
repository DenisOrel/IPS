// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Mbom.AddingToMbomForm
// Assembly: Intermech.Mbom, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 13559C9A-4DBC-479B-BA71-AFEA0247DEC7
// Assembly location: D:\IPS\Client\Intermech.Mbom.dll
// XML documentation location: D:\IPS\Client\Intermech.Mbom.xml

using Intermech.Client.Core;
using Intermech.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Mbom;

public sealed class AddingToMbomForm : Form
{
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private TableLayoutPanel tableLayoutPanel1;
  private FlowLayoutPanel flowLayoutPanel1;
  private Button _cancelButton;
  private Button _addAllButton;
  private Button _addButton;
  private AddingToMbomControl _addingToMbomControl;

  public AddingToMbomForm()
  {
    this.InitializeComponent();
    this.UpdateForm();
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public MeasuredValue Count
  {
    get => this._addingToMbomControl.Count;
    set => this._addingToMbomControl.Count = value;
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public MeasuredValue RemainingCount
  {
    get => this._addingToMbomControl.RemainingCount;
    set => this._addingToMbomControl.RemainingCount = value;
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public MeasuredValue TotalCount
  {
    get => this._addingToMbomControl.TotalCount;
    set => this._addingToMbomControl.TotalCount = value;
  }

  private void AddingToMbomControl_Changed(object sender, EventArgs e) => this.UpdateForm();

  private void AddingToMbomForm_Load(object sender, EventArgs e)
  {
    FormStorage.LoadLayout((Control) this);
  }

  private void AddingToMbomForm_FormClosing(object sender, FormClosingEventArgs e)
  {
    FormStorage.SaveLayout((Control) this);
  }

  private void AddAllButton_Click(object sender, EventArgs e)
  {
    this._addingToMbomControl.Count = this._addingToMbomControl.RemainingCount;
    this.Close();
  }

  private void UpdateForm()
  {
    this._addButton.Enabled = this._addingToMbomControl.Count != null && this._addingToMbomControl.Count.Value > 0.0 && !this._addingToMbomControl.HasErrors;
    this._addAllButton.Enabled = !this._addingToMbomControl.HasErrors;
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
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this.flowLayoutPanel1 = new FlowLayoutPanel();
    this._cancelButton = new Button();
    this._addAllButton = new Button();
    this._addButton = new Button();
    this._addingToMbomControl = new AddingToMbomControl();
    this.tableLayoutPanel1.SuspendLayout();
    this.flowLayoutPanel1.SuspendLayout();
    this.SuspendLayout();
    this.tableLayoutPanel1.ColumnCount = 1;
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.Controls.Add((Control) this.flowLayoutPanel1, 0, 1);
    this.tableLayoutPanel1.Controls.Add((Control) this._addingToMbomControl, 0, 0);
    this.tableLayoutPanel1.Dock = DockStyle.Fill;
    this.tableLayoutPanel1.Location = new Point(0, 0);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.RowCount = 2;
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40f));
    this.tableLayoutPanel1.Size = new Size(471, 125);
    this.tableLayoutPanel1.TabIndex = 0;
    this.flowLayoutPanel1.Controls.Add((Control) this._cancelButton);
    this.flowLayoutPanel1.Controls.Add((Control) this._addAllButton);
    this.flowLayoutPanel1.Controls.Add((Control) this._addButton);
    this.flowLayoutPanel1.Dock = DockStyle.Fill;
    this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
    this.flowLayoutPanel1.Location = new Point(3, 88);
    this.flowLayoutPanel1.Name = "flowLayoutPanel1";
    this.flowLayoutPanel1.Size = new Size(465, 34);
    this.flowLayoutPanel1.TabIndex = 0;
    this._cancelButton.DialogResult = DialogResult.Cancel;
    this._cancelButton.Location = new Point(387, 3);
    this._cancelButton.Name = "_cancelButton";
    this._cancelButton.Size = new Size(75, 23);
    this._cancelButton.TabIndex = 0;
    this._cancelButton.Text = "Отмена";
    this._cancelButton.UseVisualStyleBackColor = true;
    this._addAllButton.DialogResult = DialogResult.OK;
    this._addAllButton.Location = new Point(306, 3);
    this._addAllButton.Name = "_addAllButton";
    this._addAllButton.Size = new Size(75, 23);
    this._addAllButton.TabIndex = 0;
    this._addAllButton.Text = "Все кол-во";
    this._addAllButton.UseVisualStyleBackColor = true;
    this._addAllButton.Click += new EventHandler(this.AddAllButton_Click);
    this._addButton.DialogResult = DialogResult.OK;
    this._addButton.Location = new Point(225, 3);
    this._addButton.Name = "_addButton";
    this._addButton.Size = new Size(75, 23);
    this._addButton.TabIndex = 0;
    this._addButton.Text = "Добавить";
    this._addButton.UseVisualStyleBackColor = true;
    this._addingToMbomControl.Dock = DockStyle.Fill;
    this._addingToMbomControl.Location = new Point(3, 3);
    this._addingToMbomControl.Name = "_addingToMbomControl";
    this._addingToMbomControl.Size = new Size(465, 79);
    this._addingToMbomControl.TabIndex = 1;
    this._addingToMbomControl.Changed += new EventHandler(this.AddingToMbomControl_Changed);
    this.AcceptButton = (IButtonControl) this._addButton;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this._cancelButton;
    this.ClientSize = new Size(471, 125);
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.Name = nameof (AddingToMbomForm);
    this.FormClosing += new FormClosingEventHandler(this.AddingToMbomForm_FormClosing);
    this.Load += new EventHandler(this.AddingToMbomForm_Load);
    this.tableLayoutPanel1.ResumeLayout(false);
    this.flowLayoutPanel1.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
