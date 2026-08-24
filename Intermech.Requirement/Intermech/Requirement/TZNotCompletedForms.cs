// Decompiled with JetBrains decompiler
// Type: Intermech.Requirement.TZNotCompletedForms
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Requirement;

public class TZNotCompletedForms : Form
{
  private readonly List<object> _notCompletedList;
  private IContainer components;
  private ListBox notCompletedTT;
  private Button button1;
  private Label label1;

  public TZNotCompletedForms(List<object> notCompletedList)
  {
    this.InitializeComponent();
    this._notCompletedList = notCompletedList;
  }

  private void TZNotCompletedForms_Load(object sender, EventArgs e)
  {
    if (this._notCompletedList == null)
      return;
    this.notCompletedTT.Items.AddRange(this._notCompletedList.ToArray());
  }

  private void button1_Click(object sender, EventArgs e) => this.Close();

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.notCompletedTT = new ListBox();
    this.button1 = new Button();
    this.label1 = new Label();
    this.SuspendLayout();
    this.notCompletedTT.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.notCompletedTT.FormattingEnabled = true;
    this.notCompletedTT.Location = new Point(12, 25);
    this.notCompletedTT.Name = "notCompletedTT";
    this.notCompletedTT.Size = new Size(585, 225);
    this.notCompletedTT.TabIndex = 0;
    this.button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.button1.DialogResult = DialogResult.OK;
    this.button1.Location = new Point(522, 256 /*0x0100*/);
    this.button1.Name = "button1";
    this.button1.Size = new Size(75, 23);
    this.button1.TabIndex = 1;
    this.button1.Text = "OK";
    this.button1.UseVisualStyleBackColor = true;
    this.button1.Click += new EventHandler(this.button1_Click);
    this.label1.AutoSize = true;
    this.label1.Location = new Point(9, 9);
    this.label1.Name = "label1";
    this.label1.Size = new Size(194, 13);
    this.label1.TabIndex = 2;
    this.label1.Text = "Пункты которые ещё не выполнены:";
    this.AcceptButton = (IButtonControl) this.button1;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(609, 286);
    this.Controls.Add((Control) this.label1);
    this.Controls.Add((Control) this.button1);
    this.Controls.Add((Control) this.notCompletedTT);
    this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.MinimumSize = new Size(625, 320);
    this.Name = nameof (TZNotCompletedForms);
    this.ShowIcon = false;
    this.Text = "Техническое задание не выполнено";
    this.Load += new EventHandler(this.TZNotCompletedForms_Load);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
