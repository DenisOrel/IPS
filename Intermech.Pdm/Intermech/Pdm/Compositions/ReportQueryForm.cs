// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.ReportQueryForm
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Pdm;
using Intermech.Localization;
using System;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal class ReportQueryForm : Form
{
  private BackgroundReader _reader;
  private IContainer components;
  private ProgressBar progressBar1;
  private Label label1;
  private Button button1;

  public ReportQueryForm(BackgroundReader reader)
  {
    this.InitializeComponent();
    this._reader = reader;
    BackgroundReader reader1 = this._reader;
    reader1.StateChangedEvent = reader1.StateChangedEvent + new StateChanged(this.ReaderStateChanged);
  }

  private void ReaderStateChanged(object sender, StateChangedEventArgs arg)
  {
    switch (arg.State)
    {
      case BackgroundState.Empty:
      case BackgroundState.Reading:
      case BackgroundState.SetPersent:
        this.progressBar1.Invoke((Delegate) new ReportQueryForm.SetPersentEventHandler(this.SetPersentProgressBar), (object) arg.Percent);
        break;
      case BackgroundState.Error:
        this.DialogResult = DialogResult.Cancel;
        this.CloseForm();
        break;
      case BackgroundState.Fill:
        this.progressBar1.Invoke((Delegate) new ReportQueryForm.SetPersentEventHandler(this.SetPersentProgressBar), (object) 100);
        this.DialogResult = DialogResult.OK;
        this.CloseForm();
        break;
    }
  }

  private void CloseForm()
  {
    BackgroundReader reader = this._reader;
    reader.StateChangedEvent = reader.StateChangedEvent - new StateChanged(this.ReaderStateChanged);
  }

  private void SetPersentProgressBar(int percent)
  {
    this.progressBar1.Value = percent;
    this.label1.Text = string.Format(LocalizationHolder.rm.GetString("Pdm_58"), (object) percent);
  }

  private void button1_Click(object sender, EventArgs e)
  {
    BackgroundReader reader = this._reader;
    reader.StateChangedEvent = reader.StateChangedEvent - new StateChanged(this.ReaderStateChanged);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ReportQueryForm));
    this.progressBar1 = new ProgressBar();
    this.label1 = new Label();
    this.button1 = new Button();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.progressBar1, "progressBar1");
    this.progressBar1.Name = "progressBar1";
    componentResourceManager.ApplyResources((object) this.label1, "label1");
    this.label1.Name = "label1";
    this.button1.DialogResult = DialogResult.Cancel;
    componentResourceManager.ApplyResources((object) this.button1, "button1");
    this.button1.Name = "button1";
    this.button1.UseVisualStyleBackColor = true;
    this.button1.Click += new EventHandler(this.button1_Click);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.button1;
    this.Controls.Add((Control) this.button1);
    this.Controls.Add((Control) this.label1);
    this.Controls.Add((Control) this.progressBar1);
    this.FormBorderStyle = FormBorderStyle.FixedSingle;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (ReportQueryForm);
    this.ShowInTaskbar = false;
    this.Tag = (object) "";
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  private delegate void SetPersentEventHandler(int persent);
}
