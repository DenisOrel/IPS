// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.SplashLoadingForm
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Manager;

public class SplashLoadingForm : Form
{
  private Thread _thread;
  private IContainer components;
  private Label lText;

  public SplashLoadingForm() => this.InitializeComponent();

  public void Start()
  {
    this._thread = new Thread(new ThreadStart(this.ThreadMethod));
    this._thread.IsBackground = true;
    this._thread.Name = "SplashLoadingForm_Thread";
    this._thread.Start();
  }

  public void SetProgressText(string text)
  {
    this.Invoke((Delegate) new SplashLoadingForm.SetText(this.setText), (object) text);
  }

  private void setText(string text) => this.lText.Text = text;

  public void CloseForm() => this.Invoke((Delegate) new MethodInvoker(this.closeForm));

  private void closeForm() => this.Close();

  private void ThreadMethod()
  {
    int num = (int) this.ShowDialog();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.lText = new Label();
    this.SuspendLayout();
    this.lText.Dock = DockStyle.Fill;
    this.lText.Location = new Point(0, 0);
    this.lText.Name = "lText";
    this.lText.Size = new Size(443, 57);
    this.lText.TabIndex = 0;
    this.lText.TextAlign = ContentAlignment.MiddleCenter;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(443, 57);
    this.ControlBox = false;
    this.Controls.Add((Control) this.lText);
    this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (SplashLoadingForm);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Загрузка сервисов";
    this.ResumeLayout(false);
  }

  private delegate void SetText(string text);
}
