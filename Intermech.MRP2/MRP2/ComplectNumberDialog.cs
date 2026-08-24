// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.ComplectNumberDialog
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MRP2;

public class ComplectNumberDialog : Form
{
  private int max_cnt;
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private Button okBtn;
  private Button cancelBtn;
  private Label label1;
  private TextBox fromBox;
  private Label label2;
  private TextBox toBox;
  private Label maxCountLabel;

  public ComplectNumberDialog() => this.InitializeComponent();

  public static DialogResult Execute(ref string from, ref string to, string max_count)
  {
    ComplectNumberDialog complectNumberDialog = new ComplectNumberDialog();
    complectNumberDialog.fromBox.Text = from;
    complectNumberDialog.toBox.Text = to;
    if (!string.IsNullOrEmpty(max_count))
    {
      complectNumberDialog.maxCountLabel.Text += max_count;
      complectNumberDialog.maxCountLabel.Visible = true;
      MeasuredValue measuredValue = MeasureHelper.ConvertToMeasuredValue(max_count, "шт", false);
      if (measuredValue != null)
        complectNumberDialog.max_cnt = Convert.ToInt32(measuredValue.Value);
    }
    else
      complectNumberDialog.maxCountLabel.Visible = false;
    int num = (int) complectNumberDialog.ShowDialog();
    if (num != 1)
      return (DialogResult) num;
    from = complectNumberDialog.fromBox.Text;
    to = complectNumberDialog.toBox.Text;
    return (DialogResult) num;
  }

  private void ComplectNumberDialog_FormClosing(object sender, FormClosingEventArgs e)
  {
    int result1;
    int result2;
    if (this.DialogResult != DialogResult.OK || this.max_cnt == 0 || (!int.TryParse(this.toBox.Text, out result1) || result1 <= this.max_cnt) && (!int.TryParse(this.fromBox.Text, out result2) || result2 <= this.max_cnt))
      return;
    e.Cancel = true;
    int num = (int) MessageBox.Show("Комплект не должен превышать количество в сборке");
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
    this.okBtn = new Button();
    this.cancelBtn = new Button();
    this.label1 = new Label();
    this.fromBox = new TextBox();
    this.label2 = new Label();
    this.toBox = new TextBox();
    this.maxCountLabel = new Label();
    this.SuspendLayout();
    this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.okBtn.DialogResult = DialogResult.OK;
    this.okBtn.Location = new Point(125, 139);
    this.okBtn.Name = "okBtn";
    this.okBtn.Size = new Size(75, 23);
    this.okBtn.TabIndex = 2;
    this.okBtn.Text = "&OK";
    this.okBtn.UseVisualStyleBackColor = true;
    this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.cancelBtn.DialogResult = DialogResult.Cancel;
    this.cancelBtn.Location = new Point(206, 139);
    this.cancelBtn.Name = "cancelBtn";
    this.cancelBtn.Size = new Size(75, 23);
    this.cancelBtn.TabIndex = 3;
    this.cancelBtn.Text = "Отмена";
    this.cancelBtn.UseVisualStyleBackColor = true;
    this.label1.AutoSize = true;
    this.label1.Location = new Point(12, 61);
    this.label1.Name = "label1";
    this.label1.Size = new Size(75, 13);
    this.label1.TabIndex = 5;
    this.label1.Text = "С комплекта:";
    this.fromBox.Location = new Point(93, 58);
    this.fromBox.Name = "fromBox";
    this.fromBox.Size = new Size(58, 20);
    this.fromBox.TabIndex = 0;
    this.label2.AutoSize = true;
    this.label2.Location = new Point(185, 61);
    this.label2.Name = "label2";
    this.label2.Size = new Size(22, 13);
    this.label2.TabIndex = 6;
    this.label2.Text = "по:";
    this.toBox.Location = new Point(213, 58);
    this.toBox.Name = "toBox";
    this.toBox.Size = new Size(58, 20);
    this.toBox.TabIndex = 1;
    this.maxCountLabel.AutoSize = true;
    this.maxCountLabel.Location = new Point(12, 96 /*0x60*/);
    this.maxCountLabel.Name = "maxCountLabel";
    this.maxCountLabel.Size = new Size(139, 13);
    this.maxCountLabel.TabIndex = 7;
    this.maxCountLabel.Text = "Количество в ведомости: ";
    this.maxCountLabel.Visible = false;
    this.AcceptButton = (IButtonControl) this.okBtn;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.cancelBtn;
    this.ClientSize = new Size(293, 174);
    this.Controls.Add((Control) this.maxCountLabel);
    this.Controls.Add((Control) this.toBox);
    this.Controls.Add((Control) this.label2);
    this.Controls.Add((Control) this.label1);
    this.Controls.Add((Control) this.fromBox);
    this.Controls.Add((Control) this.okBtn);
    this.Controls.Add((Control) this.cancelBtn);
    this.FormBorderStyle = FormBorderStyle.FixedDialog;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (ComplectNumberDialog);
    this.ShowIcon = false;
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Введите номера комплектов";
    this.FormClosing += new FormClosingEventHandler(this.ComplectNumberDialog_FormClosing);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
