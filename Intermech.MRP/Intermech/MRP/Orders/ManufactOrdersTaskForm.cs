// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.ManufactOrdersTaskForm
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using ImSSP;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.MRP;
using Intermech.Localization;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MRP.Orders;

/// <summary>
/// Форма, отслеживающая работу службы, создающей состав производственного заказа
/// </summary>
public sealed class ManufactOrdersTaskForm : Form
{
  /// <summary>Фоновый поток для отслеживания состояния задания</summary>
  private volatile Thread _thread;
  /// <summary>
  /// Уникальный идентификатор задания по созданию состава производственного заказа
  /// </summary>
  private Guid _jobID;
  /// <summary>
  /// Состояние задания по созданию производственного заказа
  /// </summary>
  private volatile MRPTasksQueueState _mrpTasksQueueState;
  /// <summary>
  /// Объект для потокобезопасного доступа к переменным при фоновом обращении к статусу задания
  /// </summary>
  private object _lockForm = new object();
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private PictureBox pictureInfo;
  private ProgressBar progressBar;
  private Label labelWait;
  private Button btnCancel;
  private System.Windows.Forms.Timer timerRefresh;
  private Label lbInQueue;
  private Label lbCompleted;
  private Label lbCompletedCount;
  private Label lbInQueueCount;
  private Label labelInfo;

  /// <summary>Создать пустой экземпляр формы</summary>
  public ManufactOrdersTaskForm() => this.InitializeComponent();

  /// <summary>Создать заполненный экземпляр формы</summary>
  /// <param name="jobID">Уникальный идентификатор задания, которое запущено на сервере приложений</param>
  public ManufactOrdersTaskForm(Guid jobID)
    : this()
  {
    this._jobID = jobID;
    this.progressBar.Value = 0;
    this.progressBar.Maximum = sc_14795.ssp_mrp_14796(2087695045);
    Rectangle primaryWorkingArea = MultiscreenHelper.PrimaryWorkingArea;
    this.Location = new Point((primaryWorkingArea.Width - this.Size.Width) / 2 + primaryWorkingArea.Left, (primaryWorkingArea.Height - this.Size.Height) / 2 + primaryWorkingArea.Top);
    this.StartThread();
    this.UpdateControls();
  }

  /// <summary>
  /// Вызвать форму, отслеживающую состояние задания по формированию состава производственного заказа
  /// </summary>
  /// <param name="jobID">Уникальный идентификатор задания</param>
  /// <returns>Состояние задания</returns>
  public static MRPTasksQueueState Execute(Guid jobID)
  {
    using (ManufactOrdersTaskForm manufactOrdersTaskForm = new ManufactOrdersTaskForm(jobID))
    {
      int num = (int) manufactOrdersTaskForm.ShowDialog();
      ((ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService).GetCustomService(typeof (IMRPCompositionsBrowser)) as IMRPCompositionsBrowser).CancelJob(jobID);
      return manufactOrdersTaskForm._mrpTasksQueueState;
    }
  }

  private void ManufactOrdersTaskForm_FormClosing(object sender, FormClosingEventArgs e)
  {
    if (this.DialogResult == DialogResult.OK)
      return;
    this.CancelButton_Click(sender, (EventArgs) null);
  }

  private void CancelButton_Click(object sender, EventArgs e)
  {
    if (this._jobID == Guid.Empty)
      return;
    if (MessageBox.Show(LocalizationHolder.rm.GetString("MRP_65"), LocalizationHolder.rm.GetString("MRP_52"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
    {
      this.DialogResult = DialogResult.None;
    }
    else
    {
      lock (this._lockForm)
      {
        this.StopThread();
        if ((ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService).GetCustomService(typeof (IMRPCompositionsBrowser)) is IMRPCompositionsBrowser customService)
        {
          this._mrpTasksQueueState = customService.GetJobState(this._jobID);
          if (this._mrpTasksQueueState != null && this._mrpTasksQueueState.CancelledTasks <= 0)
            this._mrpTasksQueueState.CancelledTasks = 1;
          this._mrpTasksQueueState.IsBreaked = true;
          customService.CancelJob(this._jobID);
        }
        this._jobID = Guid.Empty;
        if (e == null)
          return;
        this.DialogResult = DialogResult.Cancel;
      }
    }
  }

  private void RefreshTimer_Tick(object sender, EventArgs e)
  {
    this.timerRefresh.Enabled = false;
    lock (this._lockForm)
    {
      if (this._thread == null || this._mrpTasksQueueState != null && this._mrpTasksQueueState.IsBreaked)
      {
        this.StopThread();
        this.DialogResult = DialogResult.OK;
        return;
      }
      if (this._mrpTasksQueueState != null)
      {
        this.UpdateControls();
        this.progressBar.Update();
      }
    }
    this.timerRefresh.Enabled = true;
  }

  /// <summary>Установить статус всех контролов формы</summary>
  private void UpdateControls()
  {
    try
    {
      this.labelInfo.Text = this._mrpTasksQueueState != null ? this._mrpTasksQueueState.TaskOperation : string.Empty;
      this.lbCompletedCount.Text = this._mrpTasksQueueState != null ? this._mrpTasksQueueState.ProcessedTasks.ToString() : "0";
      this.lbInQueueCount.Text = this._mrpTasksQueueState != null ? (this._mrpTasksQueueState.InProcess + this._mrpTasksQueueState.InQueue).ToString() : "0";
      this.progressBar.Minimum = Math.Max(this._mrpTasksQueueState != null ? this._mrpTasksQueueState.MinProgress : 0, 0);
      this.progressBar.Value = Math.Min(this._mrpTasksQueueState != null ? this._mrpTasksQueueState.Progress : 0, this.progressBar.Maximum);
      this.progressBar.Maximum = Math.Max(this._mrpTasksQueueState != null ? this._mrpTasksQueueState.MaxProgress : 100, this.progressBar.Maximum);
    }
    catch
    {
    }
  }

  /// <summary>
  /// Остановить фоновый поток, обращающийся к серверу приложений
  /// </summary>
  private void StopThread()
  {
    if (this._thread == null)
      return;
    this._thread = (Thread) null;
  }

  /// <summary>
  /// Запустить фоновый поток, обращающийся к серверу приложений
  /// </summary>
  private void StartThread()
  {
    this.StopThread();
    this._thread = new Thread(new ThreadStart(this.ThreadMethod));
    this._thread.IsBackground = true;
    this._thread.Priority = ThreadPriority.Lowest;
    this._thread.Name = "MRP.ManufactOrdersTaskForm";
    this._thread.Start();
    this.timerRefresh.Enabled = true;
  }

  /// <summary>Фоновое обращение к серверу приложений</summary>
  private void ThreadMethod()
  {
    lock (this._lockForm)
      this._mrpTasksQueueState = (MRPTasksQueueState) null;
    if (!((ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService).GetCustomService(typeof (IMRPCompositionsBrowser)) is IMRPCompositionsBrowser customService))
    {
      lock (this._lockForm)
        this._mrpTasksQueueState = (MRPTasksQueueState) null;
      this._thread = (Thread) null;
    }
    else
    {
      while (!(this._jobID == Guid.Empty))
      {
        this._mrpTasksQueueState = customService.GetJobState(this._jobID);
        if (this._mrpTasksQueueState != null)
        {
          if (this._mrpTasksQueueState.IsBreaked)
            ServicesManager.GetService(typeof (IInvokeService));
          if (!this._mrpTasksQueueState.IsBreaked)
            Thread.Sleep(1000);
          else
            break;
        }
        else
          break;
      }
      this._thread = (Thread) null;
    }
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
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ManufactOrdersTaskForm));
    this.pictureInfo = new PictureBox();
    this.progressBar = new ProgressBar();
    this.labelWait = new Label();
    this.btnCancel = new Button();
    this.timerRefresh = new System.Windows.Forms.Timer(this.components);
    this.lbInQueue = new Label();
    this.lbCompleted = new Label();
    this.lbCompletedCount = new Label();
    this.lbInQueueCount = new Label();
    this.labelInfo = new Label();
    ((ISupportInitialize) this.pictureInfo).BeginInit();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.pictureInfo, "pictureInfo");
    this.pictureInfo.Name = "pictureInfo";
    this.pictureInfo.TabStop = false;
    componentResourceManager.ApplyResources((object) this.progressBar, "progressBar");
    this.progressBar.Name = "progressBar";
    this.progressBar.Step = 1;
    this.progressBar.Style = ProgressBarStyle.Continuous;
    componentResourceManager.ApplyResources((object) this.labelWait, "labelWait");
    this.labelWait.Name = "labelWait";
    this.btnCancel.Cursor = Cursors.Default;
    this.btnCancel.DialogResult = DialogResult.Cancel;
    componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.Click += new EventHandler(this.CancelButton_Click);
    this.timerRefresh.Interval = 1000;
    this.timerRefresh.Tick += new EventHandler(this.RefreshTimer_Tick);
    componentResourceManager.ApplyResources((object) this.lbInQueue, "lbInQueue");
    this.lbInQueue.Name = "lbInQueue";
    componentResourceManager.ApplyResources((object) this.lbCompleted, "lbCompleted");
    this.lbCompleted.Name = "lbCompleted";
    componentResourceManager.ApplyResources((object) this.lbCompletedCount, "lbCompletedCount");
    this.lbCompletedCount.Name = "lbCompletedCount";
    componentResourceManager.ApplyResources((object) this.lbInQueueCount, "lbInQueueCount");
    this.lbInQueueCount.Name = "lbInQueueCount";
    componentResourceManager.ApplyResources((object) this.labelInfo, "labelInfo");
    this.labelInfo.Name = "labelInfo";
    this.CancelButton = (IButtonControl) this.btnCancel;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.Controls.Add((Control) this.labelInfo);
    this.Controls.Add((Control) this.lbCompletedCount);
    this.Controls.Add((Control) this.lbInQueueCount);
    this.Controls.Add((Control) this.lbCompleted);
    this.Controls.Add((Control) this.lbInQueue);
    this.Controls.Add((Control) this.pictureInfo);
    this.Controls.Add((Control) this.progressBar);
    this.Controls.Add((Control) this.labelWait);
    this.Controls.Add((Control) this.btnCancel);
    this.FormBorderStyle = FormBorderStyle.FixedDialog;
    this.MaximizeBox = false;
    this.Name = nameof (ManufactOrdersTaskForm);
    this.SizeGripStyle = SizeGripStyle.Hide;
    ((ISupportInitialize) this.pictureInfo).EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
