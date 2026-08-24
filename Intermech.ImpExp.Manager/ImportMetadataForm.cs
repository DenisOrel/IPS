// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.ImportMetadataForm
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Manager;

internal class ImportMetadataForm : Form
{
  private const string _labelText = "Выполнено: {0}%";
  private IContainer components;
  private ProgressBar progressBar1;
  private Label label1;

  public ImportMetadataForm()
  {
    this.InitializeComponent();
    this.label1.Text = $"Выполнено: {0}%";
  }

  public void SetPercent(int percent)
  {
    this.Invoke((Delegate) new ImportMetadataForm.SetPercentDelegate(this._setPercent), (object) percent);
  }

  private void _setPercent(int percent)
  {
    if (percent > 100)
      percent = 100;
    this.progressBar1.Value = percent;
    this.label1.Text = $"Выполнено: {percent}%";
  }

  public void CloseForm() => this.Invoke((Delegate) new MethodInvoker(this._closeForm));

  private void _closeForm() => this.Close();

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.progressBar1 = new ProgressBar();
    this.label1 = new Label();
    this.SuspendLayout();
    this.progressBar1.Location = new Point(12, 21);
    this.progressBar1.Name = "progressBar1";
    this.progressBar1.Size = new Size(373, 23);
    this.progressBar1.Step = 1;
    this.progressBar1.TabIndex = 0;
    this.label1.AutoSize = true;
    this.label1.Location = new Point(17, 4);
    this.label1.Name = "label1";
    this.label1.Size = new Size(35, 13);
    this.label1.TabIndex = 1;
    this.label1.Text = "label1";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(397, 54);
    this.ControlBox = false;
    this.Controls.Add((Control) this.label1);
    this.Controls.Add((Control) this.progressBar1);
    this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (ImportMetadataForm);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Импорт метаданных";
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  private delegate void SetPercentDelegate(int percent);
}
