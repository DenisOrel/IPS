// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.VisDialogs.UserPrompt
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.VisDialogs;

public class UserPrompt : Form
{
  private IContainer components;
  private Panel panel1;
  private Button button2;
  private Button button1;
  private Label lblPrompt;
  private TextBox tbResult;

  public UserPrompt() => this.InitializeComponent();

  public string Execute(string capt, string prompt)
  {
    this.Text = capt;
    this.lblPrompt.Text = prompt;
    return this.ShowDialog() == DialogResult.OK ? this.tbResult.Text : "";
  }

  private void button2_Click(object sender, EventArgs e)
  {
    if (!(this.tbResult.Text == ""))
      return;
    this.DialogResult = DialogResult.None;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.panel1 = new Panel();
    this.button2 = new Button();
    this.button1 = new Button();
    this.lblPrompt = new Label();
    this.tbResult = new TextBox();
    this.panel1.SuspendLayout();
    this.SuspendLayout();
    this.panel1.Controls.Add((Control) this.button2);
    this.panel1.Controls.Add((Control) this.button1);
    this.panel1.Dock = DockStyle.Bottom;
    this.panel1.Location = new Point(0, 67);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(587, 30);
    this.panel1.TabIndex = 0;
    this.button2.DialogResult = DialogResult.OK;
    this.button2.Location = new Point(419, 3);
    this.button2.Name = "button2";
    this.button2.Size = new Size(75, 23);
    this.button2.TabIndex = 1;
    this.button2.Text = "OK";
    this.button2.UseVisualStyleBackColor = true;
    this.button2.Click += new EventHandler(this.button2_Click);
    this.button1.DialogResult = DialogResult.Cancel;
    this.button1.Location = new Point(500, 3);
    this.button1.Name = "button1";
    this.button1.Size = new Size(75, 23);
    this.button1.TabIndex = 0;
    this.button1.Text = "Отмена";
    this.button1.UseVisualStyleBackColor = true;
    this.lblPrompt.AutoSize = true;
    this.lblPrompt.Location = new Point(12, 12);
    this.lblPrompt.Name = "lblPrompt";
    this.lblPrompt.Size = new Size(35, 13);
    this.lblPrompt.TabIndex = 1;
    this.lblPrompt.Text = "label1";
    this.tbResult.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.tbResult.Location = new Point(12, 28);
    this.tbResult.Name = "tbResult";
    this.tbResult.Size = new Size(563, 20);
    this.tbResult.TabIndex = 2;
    this.AcceptButton = (IButtonControl) this.button2;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.button1;
    this.ClientSize = new Size(587, 97);
    this.Controls.Add((Control) this.tbResult);
    this.Controls.Add((Control) this.lblPrompt);
    this.Controls.Add((Control) this.panel1);
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (UserPrompt);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = nameof (UserPrompt);
    this.panel1.ResumeLayout(false);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
