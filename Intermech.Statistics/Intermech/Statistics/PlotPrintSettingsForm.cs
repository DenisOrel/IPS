// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.PlotPrintSettingsForm
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Statistics;

public class PlotPrintSettingsForm : Form
{
  private IContainer components;
  private Label label1;
  private NumericUpDown printPageCount;
  private Button btnOK;
  private Button btnCancel;
  private CheckBox checkBox1;

  public int PrintPageCount { get; private set; } = 1;

  public bool NeedToPrintLegend { get; private set; }

  public PlotPrintSettingsForm() => this.InitializeComponent();

  private void GetPageCountesForChart_Load(object sender, EventArgs e)
  {
    this.printPageCount.Maximum = 2147483646M;
  }

  private void btnOK_Click(object sender, EventArgs e)
  {
    this.PrintPageCount = (int) this.printPageCount.Value;
    this.NeedToPrintLegend = this.checkBox1.Checked;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.label1 = new Label();
    this.printPageCount = new NumericUpDown();
    this.btnOK = new Button();
    this.btnCancel = new Button();
    this.checkBox1 = new CheckBox();
    this.printPageCount.BeginInit();
    this.SuspendLayout();
    this.label1.AutoSize = true;
    this.label1.Location = new Point(13, 13);
    this.label1.Name = "label1";
    this.label1.Size = new Size(240 /*0xF0*/, 13);
    this.label1.TabIndex = 0;
    this.label1.Text = "Количество страниц для разбиения графика: ";
    this.printPageCount.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.printPageCount.Location = new Point(259, 11);
    this.printPageCount.Minimum = new Decimal(new int[4]
    {
      1,
      0,
      0,
      0
    });
    this.printPageCount.Name = "printPageCount";
    this.printPageCount.Size = new Size(116, 20);
    this.printPageCount.TabIndex = 1;
    this.printPageCount.Value = new Decimal(new int[4]
    {
      1,
      0,
      0,
      0
    });
    this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnOK.DialogResult = DialogResult.OK;
    this.btnOK.Location = new Point(186, 72);
    this.btnOK.Name = "btnOK";
    this.btnOK.Size = new Size(97, 23);
    this.btnOK.TabIndex = 2;
    this.btnOK.Text = "OK";
    this.btnOK.UseVisualStyleBackColor = true;
    this.btnOK.Click += new EventHandler(this.btnOK_Click);
    this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.Location = new Point(289, 72);
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.Size = new Size(99, 23);
    this.btnCancel.TabIndex = 3;
    this.btnCancel.Text = "Отменить";
    this.btnCancel.UseVisualStyleBackColor = true;
    this.checkBox1.AutoSize = true;
    this.checkBox1.Checked = true;
    this.checkBox1.CheckState = CheckState.Checked;
    this.checkBox1.Location = new Point(16 /*0x10*/, 41);
    this.checkBox1.Name = "checkBox1";
    this.checkBox1.Size = new Size(116, 17);
    this.checkBox1.TabIndex = 4;
    this.checkBox1.Text = "Печатать легенду";
    this.checkBox1.UseVisualStyleBackColor = true;
    this.AcceptButton = (IButtonControl) this.btnOK;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.btnCancel;
    this.ClientSize = new Size(400, 107);
    this.Controls.Add((Control) this.checkBox1);
    this.Controls.Add((Control) this.btnCancel);
    this.Controls.Add((Control) this.btnOK);
    this.Controls.Add((Control) this.printPageCount);
    this.Controls.Add((Control) this.label1);
    this.MaximizeBox = false;
    this.MaximumSize = new Size(416, 146);
    this.MinimumSize = new Size(416, 146);
    this.Name = nameof (PlotPrintSettingsForm);
    this.StartPosition = FormStartPosition.CenterScreen;
    this.Text = "Количество страниц для печати графика";
    this.Load += new EventHandler(this.GetPageCountesForChart_Load);
    this.printPageCount.EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
