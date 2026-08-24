// Decompiled with JetBrains decompiler
// Type: Intermech.ProEngineer.Integrator.StripVersionNumbersTargetWindow
// Assembly: Intermech.ProEngineer.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 19987673-5EB5-4BB3-AE60-6A96614A14F3
// Assembly location: D:\IPS\Client\Intermech.ProEngineer.Integrator.dll

using Intermech.Mvp;
using Intermech.Mvp.Components;
using Intermech.Mvp.Winforms;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ProEngineer.Integrator;

internal sealed class StripVersionNumbersTargetWindow : 
  MvpWindow,
  IStripVersionNumbserTargetView,
  IView,
  IOperationConfirmationView
{
  private IContainer components;
  private TableLayoutPanel tableLayoutPanel1;
  private Label label1;
  private RadioButton rbWorkspace;
  private RadioButton rbEnterpriseArchive;
  private FlowLayoutPanel flowLayoutPanel1;
  private Button btClose;

  public StripVersionNumbersTargetWindow()
  {
    this.InitializeComponent();
    if (this.DesignMode)
      return;
    this.Text = string.Format(Localization.rm.GetString("ProEngineer.Integrator_4"), (object) PEConsts.AppName);
  }

  StripVersionNumbersTarget IStripVersionNumbserTargetView.GetSelectedTarget()
  {
    return this.rbWorkspace.Checked || !this.rbEnterpriseArchive.Checked ? StripVersionNumbersTarget.Workspace : StripVersionNumbersTarget.EnterpriseArchive;
  }

  event EventHandler IOperationConfirmationView.OperationConfirmed
  {
    add => this.btClose.Click += value;
    remove => this.btClose.Click -= value;
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
    this.label1 = new Label();
    this.rbWorkspace = new RadioButton();
    this.rbEnterpriseArchive = new RadioButton();
    this.flowLayoutPanel1 = new FlowLayoutPanel();
    this.btClose = new Button();
    this.tableLayoutPanel1.SuspendLayout();
    this.flowLayoutPanel1.SuspendLayout();
    this.SuspendLayout();
    this.tableLayoutPanel1.AutoSize = true;
    this.tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
    this.tableLayoutPanel1.ColumnCount = 1;
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.Controls.Add((Control) this.label1, 0, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this.rbWorkspace, 0, 1);
    this.tableLayoutPanel1.Controls.Add((Control) this.rbEnterpriseArchive, 0, 2);
    this.tableLayoutPanel1.Controls.Add((Control) this.flowLayoutPanel1, 0, 3);
    this.tableLayoutPanel1.Dock = DockStyle.Fill;
    this.tableLayoutPanel1.Location = new Point(0, 0);
    this.tableLayoutPanel1.MaximumSize = new Size(500, 310);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.RowCount = 4;
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
    this.tableLayoutPanel1.Size = new Size(500, 222);
    this.tableLayoutPanel1.TabIndex = 0;
    this.label1.AutoSize = true;
    this.label1.Dock = DockStyle.Bottom;
    this.label1.Location = new Point(16 /*0x10*/, 16 /*0x10*/);
    this.label1.Margin = new Padding(16 /*0x10*/, 16 /*0x10*/, 16 /*0x10*/, 8);
    this.label1.Name = "label1";
    this.label1.Size = new Size(468, 26);
    this.label1.TabIndex = 0;
    this.label1.Text = "Выберите каталог, в котором следует убрать версии из имен файлов. В большинстве случаев можно использовать предложенный вариант без изменений";
    this.rbWorkspace.AutoSize = true;
    this.rbWorkspace.Checked = true;
    this.rbWorkspace.Location = new Point(32 /*0x20*/, 58);
    this.rbWorkspace.Margin = new Padding(32 /*0x20*/, 8, 3, 8);
    this.rbWorkspace.Name = "rbWorkspace";
    this.rbWorkspace.Size = new Size(422, 17);
    this.rbWorkspace.TabIndex = 1;
    this.rbWorkspace.TabStop = true;
    this.rbWorkspace.Text = "Каталог рабочей области пользователя IPS, а также вложенные подкаталоги";
    this.rbWorkspace.UseVisualStyleBackColor = true;
    this.rbEnterpriseArchive.AutoSize = true;
    this.rbEnterpriseArchive.Location = new Point(32 /*0x20*/, 91);
    this.rbEnterpriseArchive.Margin = new Padding(32 /*0x20*/, 8, 3, 8);
    this.rbEnterpriseArchive.Name = "rbEnterpriseArchive";
    this.rbEnterpriseArchive.Size = new Size(401, 17);
    this.rbEnterpriseArchive.TabIndex = 2;
    this.rbEnterpriseArchive.Text = "Каталог исходного архива предприятия, а также вложенные подкаталоги";
    this.rbEnterpriseArchive.UseVisualStyleBackColor = true;
    this.flowLayoutPanel1.AutoSize = true;
    this.flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
    this.flowLayoutPanel1.Controls.Add((Control) this.btClose);
    this.flowLayoutPanel1.Dock = DockStyle.Bottom;
    this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
    this.flowLayoutPanel1.Location = new Point(16 /*0x10*/, 181);
    this.flowLayoutPanel1.Margin = new Padding(16 /*0x10*/, 32 /*0x20*/, 16 /*0x10*/, 8);
    this.flowLayoutPanel1.Name = "flowLayoutPanel1";
    this.flowLayoutPanel1.Size = new Size(468, 33);
    this.flowLayoutPanel1.TabIndex = 4;
    this.btClose.AutoSize = true;
    this.btClose.DialogResult = DialogResult.OK;
    this.btClose.Location = new Point(393, 3);
    this.btClose.Margin = new Padding(0, 3, 0, 3);
    this.btClose.Name = "btClose";
    this.btClose.Padding = new Padding(2);
    this.btClose.Size = new Size(75, 27);
    this.btClose.TabIndex = 0;
    this.btClose.Text = "OK";
    this.btClose.UseVisualStyleBackColor = true;
    this.AcceptButton = (IButtonControl) this.btClose;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.AutoSize = true;
    this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
    this.ClientSize = new Size(516, 222);
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.FormBorderStyle = FormBorderStyle.FixedDialog;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (StripVersionNumbersTargetWindow);
    this.StartPosition = FormStartPosition.CenterScreen;
    this.Text = "";
    this.tableLayoutPanel1.ResumeLayout(false);
    this.tableLayoutPanel1.PerformLayout();
    this.flowLayoutPanel1.ResumeLayout(false);
    this.flowLayoutPanel1.PerformLayout();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
