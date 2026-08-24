// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.Options.ImportAnalyzeForm
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.PdmConfigurator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace Intermech.PdmConfigurator.Options;

public class ImportAnalyzeForm : Form
{
  private PdmAnalyzedOptionObjects analyzedObjects;
  private Guid jobID;
  private PdmAnalyzerFlags flag;
  private PdmOptionsAnalyzerJobStatus jobStatus;
  private object lockForm = (object) new Guid();
  private Thread thread;
  private List<long> excludedObjects;
  private IList<long> excludedOptions;
  private IContainer components;
  private System.Windows.Forms.Timer timerRefresh;
  private Button btnCancel;
  private Label labelInfo;
  private PictureBox pictureInfo;

  public ImportAnalyzeForm(
    PdmAnalyzedOptionObjects selObjects,
    PdmAnalyzerFlags analyzerFlag,
    List<long> excludedObjects,
    IList<long> excludedOptions)
  {
    this.InitializeComponent();
    this.Init(selObjects, analyzerFlag, excludedObjects, excludedOptions);
  }

  protected virtual void Init(
    PdmAnalyzedOptionObjects selObjects,
    PdmAnalyzerFlags analyzerFlag,
    List<long> excludedObjects,
    IList<long> excludedOptions)
  {
    this.analyzedObjects = selObjects;
    this.jobID = Guid.Empty;
    this.flag = analyzerFlag;
    this.excludedObjects = excludedObjects;
    this.excludedOptions = excludedOptions;
    Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
    this.Location = new Point((workingArea.Width - this.Size.Width) / 2, (workingArea.Height - this.Size.Height) / 2);
    this.StartThread();
    this.UpdateControls();
  }

  public static PdmOptionsAnalyzerJobStatus Execute(
    PdmAnalyzedOptionObjects selObjects,
    PdmAnalyzerFlags analyzerFlag,
    List<long> excludedObjects,
    IList<long> excludedOptions)
  {
    using (ImportAnalyzeForm importAnalyzeForm = new ImportAnalyzeForm(selObjects, analyzerFlag, excludedObjects, excludedOptions))
      return importAnalyzeForm.ShowDialog() != DialogResult.OK ? (PdmOptionsAnalyzerJobStatus) null : importAnalyzeForm.jobStatus;
  }

  public virtual void UpdateControls()
  {
  }

  private void ChangingAnalyzerForm_Load(object sender, EventArgs e)
  {
  }

  private void ChangingAnalyzerForm_FormClosed(object sender, FormClosedEventArgs e)
  {
  }

  private void ChangingAnalyzerForm_FormClosing(object sender, FormClosingEventArgs e)
  {
    if (this.DialogResult == DialogResult.OK)
      return;
    this.DoCancelAnalyze(sender, (EventArgs) null);
  }

  private void StopThread()
  {
    if (this.thread != null)
      this.thread.Abort();
    this.thread = (Thread) null;
  }

  private void StartThread()
  {
    this.StopThread();
    this.thread = new Thread(new ThreadStart(this.ThreadMethod));
    this.thread.IsBackground = true;
    this.thread.Name = "PdmConfigurator.ImportAnalyzeForm";
    this.thread.Start();
    this.timerRefresh.Enabled = true;
  }

  protected virtual void ThreadMethod()
  {
    lock (this.lockForm)
      this.jobStatus = (PdmOptionsAnalyzerJobStatus) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetCustomService(typeof (IPdmConfiguratorService)) is IPdmConfiguratorService customService))
      {
        lock (this.lockForm)
          this.jobStatus = (PdmOptionsAnalyzerJobStatus) null;
        this.thread = (Thread) null;
        return;
      }
      this.jobID = customService.Analyze(sessionKeeper.Session.SessionGUID, this.analyzedObjects, this.flag, (IList<long>) this.excludedObjects, this.excludedOptions);
      while (!(this.jobID == Guid.Empty))
      {
        PdmOptionsAnalyzerJobStatus analyzerJobStatus = customService.QueryJobStatus(this.jobID);
        lock (this.lockForm)
          this.jobStatus = analyzerJobStatus;
        if (analyzerJobStatus != null)
        {
          if (analyzerJobStatus.Progress != PdmOptionsAnalyzerJobProgress.NotStarted)
          {
            if (analyzerJobStatus.Progress != PdmOptionsAnalyzerJobProgress.Working)
              break;
          }
          Thread.Sleep(1000);
        }
        else
          break;
      }
    }
    this.thread = (Thread) null;
  }

  private void timerRefresh_Tick(object sender, EventArgs e)
  {
    this.timerRefresh.Enabled = false;
    lock (this.lockForm)
    {
      if (this.thread != null)
      {
        if (this.jobStatus != null)
        {
          if (this.jobStatus.Progress != PdmOptionsAnalyzerJobProgress.NotStarted)
          {
            if (this.jobStatus.Progress == PdmOptionsAnalyzerJobProgress.Working)
              goto label_9;
          }
          else
            goto label_9;
        }
        else
          goto label_9;
      }
      this.StopThread();
      this.DialogResult = DialogResult.OK;
      return;
    }
label_9:
    this.timerRefresh.Enabled = true;
  }

  private void DoCancelAnalyze(object sender, EventArgs e)
  {
    if (this.jobID == Guid.Empty)
      return;
    lock (this.lockForm)
    {
      this.StopThread();
      if ((ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService).GetCustomService(typeof (IPdmConfiguratorService)) is IPdmConfiguratorService customService)
        customService.CancelJob(this.jobID);
      this.jobStatus = (PdmOptionsAnalyzerJobStatus) null;
      this.jobID = Guid.Empty;
      if (e == null)
        return;
      this.DialogResult = DialogResult.Cancel;
    }
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ImportAnalyzeForm));
    this.timerRefresh = new System.Windows.Forms.Timer(this.components);
    this.btnCancel = new Button();
    this.labelInfo = new Label();
    this.pictureInfo = new PictureBox();
    ((ISupportInitialize) this.pictureInfo).BeginInit();
    this.SuspendLayout();
    this.timerRefresh.Interval = 1000;
    this.timerRefresh.Tick += new EventHandler(this.timerRefresh_Tick);
    this.btnCancel.Cursor = Cursors.Default;
    this.btnCancel.DialogResult = DialogResult.Cancel;
    componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.Click += new EventHandler(this.DoCancelAnalyze);
    componentResourceManager.ApplyResources((object) this.labelInfo, "labelInfo");
    this.labelInfo.Name = "labelInfo";
    componentResourceManager.ApplyResources((object) this.pictureInfo, "pictureInfo");
    this.pictureInfo.Name = "pictureInfo";
    this.pictureInfo.TabStop = false;
    this.AutoScaleMode = AutoScaleMode.Inherit;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.Controls.Add((Control) this.pictureInfo);
    this.Controls.Add((Control) this.labelInfo);
    this.Controls.Add((Control) this.btnCancel);
    this.FormBorderStyle = FormBorderStyle.FixedDialog;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (ImportAnalyzeForm);
    this.SizeGripStyle = SizeGripStyle.Hide;
    this.Load += new EventHandler(this.ChangingAnalyzerForm_Load);
    this.FormClosed += new FormClosedEventHandler(this.ChangingAnalyzerForm_FormClosed);
    this.FormClosing += new FormClosingEventHandler(this.ChangingAnalyzerForm_FormClosing);
    ((ISupportInitialize) this.pictureInfo).EndInit();
    this.ResumeLayout(false);
  }
}
