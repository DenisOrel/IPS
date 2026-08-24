// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PleaseWaitForm
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client;

internal class PleaseWaitForm : Form
{
  private IContainer components;
  private Label label1;
  private Label label2;
  private ProgressBar progressBar1;

  public PleaseWaitForm() => this.InitializeComponent();

  public void CloseForm() => this.Invoke((Delegate) new MethodInvoker(((Form) this).Close));

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.label1 = new Label();
    this.label2 = new Label();
    this.progressBar1 = new ProgressBar();
    this.SuspendLayout();
    this.label1.Dock = DockStyle.Top;
    this.label1.Location = new Point(0, 0);
    this.label1.Name = "label1";
    this.label1.Size = new Size(433, 23);
    this.label1.TabIndex = 0;
    this.label1.Text = "Производится формирование списка публикуемого состава.";
    this.label1.TextAlign = ContentAlignment.MiddleCenter;
    this.label2.Dock = DockStyle.Bottom;
    this.label2.Location = new Point(0, 56);
    this.label2.Name = "label2";
    this.label2.Size = new Size(433, 23);
    this.label2.TabIndex = 1;
    this.label2.Text = "Пожалуйста подождите.";
    this.label2.TextAlign = ContentAlignment.MiddleCenter;
    this.progressBar1.Location = new Point(60, 29);
    this.progressBar1.MarqueeAnimationSpeed = 30;
    this.progressBar1.Name = "progressBar1";
    this.progressBar1.Size = new Size(311, 23);
    this.progressBar1.Style = ProgressBarStyle.Marquee;
    this.progressBar1.TabIndex = 2;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(433, 79);
    this.ControlBox = false;
    this.Controls.Add((Control) this.progressBar1);
    this.Controls.Add((Control) this.label2);
    this.Controls.Add((Control) this.label1);
    this.FormBorderStyle = FormBorderStyle.None;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (PleaseWaitForm);
    this.ShowIcon = false;
    this.ShowInTaskbar = false;
    this.StartPosition = FormStartPosition.CenterScreen;
    this.Text = nameof (PleaseWaitForm);
    this.ResumeLayout(false);
  }
}
