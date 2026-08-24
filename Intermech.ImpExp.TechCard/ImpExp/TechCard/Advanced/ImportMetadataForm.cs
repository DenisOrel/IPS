// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Advanced.ImportMetadataForm
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard.Advanced;

public class ImportMetadataForm : Form
{
  private Thread _thread;
  private IContainer components;
  private ProgressBar progressBar1;
  private Label label1;

  public ImportMetadataForm() => this.InitializeComponent();

  private void setPercent(int percent) => this.progressBar1.Value = percent;

  private void setDescription(string desc) => this.label1.Text = desc;

  public void SetProgressText(string text)
  {
    this.Invoke((Delegate) new ImportMetadataForm.SetText(this.setDescription), (object) text);
  }

  public void SetProgressPercent(int percents)
  {
    this.Invoke((Delegate) new ImportMetadataForm.SetPercent(this.setPercent), (object) percents);
  }

  public void Start()
  {
    this._thread = new Thread(new ThreadStart(this.ThreadMethod));
    this._thread.IsBackground = true;
    this._thread.Name = "TechImportMetadataForm_Thread";
    this._thread.Start();
  }

  private void ThreadMethod()
  {
    int num = (int) this.ShowDialog();
  }

  public void CloseForm() => this.Invoke((Delegate) new MethodInvoker(this.closeForm));

  private void closeForm() => this.Close();

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
    this.progressBar1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.progressBar1.Location = new Point(12, 34);
    this.progressBar1.Name = "progressBar1";
    this.progressBar1.Size = new Size(368, 23);
    this.progressBar1.TabIndex = 0;
    this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.label1.AutoSize = true;
    this.label1.Location = new Point(12, 9);
    this.label1.Name = "label1";
    this.label1.Size = new Size(111, 13);
    this.label1.TabIndex = 1;
    this.label1.Text = "Импорт метаданных";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(392, 73);
    this.ControlBox = false;
    this.Controls.Add((Control) this.label1);
    this.Controls.Add((Control) this.progressBar1);
    this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
    this.Name = nameof (ImportMetadataForm);
    this.Text = "Импорт методанных TechCard";
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  private delegate void SetText(string text);

  private delegate void SetPercent(int percent);
}
