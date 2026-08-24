// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.Controls.ThreadedStepControl
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.ImpExp.Interface.Controls;

public class ThreadedStepControl : StepControl, IThreadedStepControl, IStepControl
{
  /// <summary>Текущий поток</summary>
  private Thread currentThread;
  /// <summary>Таймер</summary>
  private System.Windows.Forms.Timer _timer;
  /// <summary>
  /// Коллекция сгенеренных пампером шагов вида,
  /// GUID пампера -&gt; последний CheckPointArgs пампера
  /// </summary>
  private Dictionary<Guid, ThreadedStepControl.CheckPointArgsEx> _checkPoints = new Dictionary<Guid, ThreadedStepControl.CheckPointArgsEx>();
  /// <summary>Управляющий поток</summary>
  private Thread mainThread;
  /// <summary>Флаг того, что произошла ошибка</summary>
  private bool errorPresent;
  /// <summary>Флаг того, что нажали "Отмена"</summary>
  private bool cancelPresent;
  /// <summary>Текст статуса незапущенной задачи</summary>
  private string textZeroStatus = "Не запущен";
  /// <summary>Количество памперов</summary>
  private int _countPumpers;
  protected bool isPump;
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private Panel panel1;
  protected ProgressBar progressBar1;
  protected ProgressBar progressBar2;
  protected Label label1;
  private Label label2;
  private Panel panel2;
  protected iGrid iGrid1;
  private iGCellStyle iGrid1Col0CellStyle;
  private iGColHdrStyle iGrid1Col0ColHdrStyle;
  private iGCellStyle iGrid1Col1CellStyle;
  private iGColHdrStyle iGrid1Col1ColHdrStyle;
  private iGCellStyle iGrid1Col2CellStyle;
  private iGColHdrStyle iGrid1Col2ColHdrStyle;
  private iGCellStyle iGrid1DefaultCellStyle;
  private iGColHdrStyle iGrid1DefaultColHdrStyle;
  private iGCellStyle iGrid1RowTextColCellStyle;
  private iGCellStyle iGrid1Col3CellStyle;
  private iGColHdrStyle iGrid1Col3ColHdrStyle;

  public ThreadedStepControl() => this.InitializeComponent();

  public ThreadedStepControl(object owner)
    : this()
  {
    this.owner = owner;
    this.stepPrevAllowed = false;
    this.progressBar2.Maximum = 100;
    this.progressBar2.Minimum = this.progressBar2.Value = 0;
    this.progressBar1.Minimum = this.progressBar1.Value = 0;
  }

  /// <summary>Добавить в список item (задачу)</summary>
  /// <param name="tag">GUID задачи</param>
  /// <param name="description">Название задачи</param>
  protected void AddListViewItem(Guid guid, string description)
  {
    iGRow iGrow = this.iGrid1.Rows.Add();
    iGrow.Cells[0].Value = (object) description;
    iGrow.Cells[1].Value = (object) Convert.ToString(0);
    iGrow.Cells[2].Value = (object) this.textZeroStatus;
    iGrow.Tag = (object) guid;
  }

  /// <summary>Стартуем управляющий поток</summary>
  /// <param name="collection">Коллекция памперов</param>
  protected void StartMainThread(List<IPumpTask> collection)
  {
    this.StartTimer();
    this._countPumpers = collection.Count;
    this.mainThread = new Thread(new ParameterizedThreadStart(this.MainThreadMethod));
    this.mainThread.SetApartmentState(ApartmentState.STA);
    this.mainThread.IsBackground = true;
    this.mainThread.Name = "MainMethod_Thread";
    this.mainThread.Start((object) collection);
  }

  /// <summary>Управляющий поток, иво метод</summary>
  private void MainThreadMethod(object obj)
  {
    List<IPumpTask> pumpTaskList = obj as List<IPumpTask>;
    ISavePoint service1 = ServicesManager.GetService(typeof (ISavePoint)) as ISavePoint;
    IDataWriter service2 = ServicesManager.GetService(typeof (IDataWriter)) as IDataWriter;
    bool flag = false;
    try
    {
      PumpTaskType pumpTaskType = PumpTaskType.ExamMetadata;
      for (int index = 0; index < pumpTaskList.Count; ++index)
      {
        IPumpTask parameter = pumpTaskList[index];
        parameter.OnCheckPoint += new CheckPointDelegate(this.Task_OnCheckPoint);
        parameter.OnReadCountRecords += new OnReadCountRecordsDelegate(this.Task_OnReadCountRecords);
        try
        {
          if (parameter.Type == PumpTaskType.PumpData || parameter.Type == PumpTaskType.PumpMetadata)
          {
            SavePoint point = service1.GetSavePoint() ?? new SavePoint();
            point.OperationTerminateType = TerminateType.Pump;
            point.PumpGuid = parameter.GUID;
            service1.SetSavePoint(point);
            service2?.AppManager.AddInfoMessage($"Начало выполнения задачи \"{parameter.Description}\"");
          }
          if (pumpTaskType == PumpTaskType.PumpMetadata && parameter.Type == PumpTaskType.PumpData && !(ServicesManager.GetService(typeof (IConfigurationService)) as IConfigurationService).Configuration.DataMigrate)
          {
            flag = true;
            break;
          }
          this.currentThread = new Thread(new ParameterizedThreadStart(this.TaskMethod));
          this.currentThread.SetApartmentState(ApartmentState.STA);
          this.currentThread.IsBackground = true;
          this.currentThread.Name = $"{parameter.GUID}_Thread";
          this.currentThread.Start((object) parameter);
          this.currentThread.Join();
          if (!this.errorPresent && (parameter.Type == PumpTaskType.PumpMetadata || parameter.Type == PumpTaskType.PumpData))
          {
            SavePoint savePoint = service1.GetSavePoint();
            if (savePoint.PumpCompleted == null)
              savePoint.PumpCompleted = new List<Guid>(pumpTaskList.Count);
            savePoint.PumpCompleted.Add(parameter.GUID);
            service1.SetSavePoint(savePoint);
          }
          if (this.errorPresent)
            break;
          pumpTaskType = parameter.Type;
        }
        finally
        {
          parameter.OnCheckPoint -= new CheckPointDelegate(this.Task_OnCheckPoint);
          parameter.OnReadCountRecords -= new OnReadCountRecordsDelegate(this.Task_OnReadCountRecords);
          if (!this.cancelPresent)
            this.Task_OnEnd(parameter.GUID, index);
        }
      }
    }
    finally
    {
      if (this.InvokeRequired)
        this.BeginInvoke((Delegate) new MethodInvoker(this.StopTimer));
      else
        this.StopTimer();
      if (this.currentThread != null && this.currentThread.IsAlive)
      {
        this.currentThread.Abort();
        this.currentThread.Join();
      }
      if (!this.cancelPresent && this.OnEndSaveSettings != null)
      {
        SaveSettingsResult result = this.errorPresent ? SaveSettingsResult.ssrError : (flag ? SaveSettingsResult.ssrMetadataTerminate : SaveSettingsResult.ssrOk);
        OnEndEventHandler onEndSaveSettings = this.OnEndSaveSettings;
        if (onEndSaveSettings != null)
          onEndSaveSettings((object) this, new OnEndEventArgs(result));
      }
      if (this.isPump && service2 != null && this.errorPresent)
        service2.AppManager.AddInfoMessage("Миграция данных была прервана из-за ошибки. См. лог.");
    }
  }

  private void Task_OnReadCountRecords(Guid sender, OnReadCountRecordsArgs e)
  {
    this.logFile?.WriteMessage($"Количество обрабатываемых записей: {e.Count}");
  }

  /// <summary>Прибивает таймер</summary>
  private void StopTimer()
  {
    this._timer.Enabled = false;
    this._timer.Tick -= new EventHandler(this.Timer_Tick);
    this._timer = (System.Windows.Forms.Timer) null;
  }

  /// <summary>
  /// Стартует таймер по которому прорисовываем статус и процент выполнения задачи
  /// </summary>
  private void StartTimer()
  {
    this._timer = new System.Windows.Forms.Timer() { Interval = 10 };
    this._timer.Tick += new EventHandler(this.Timer_Tick);
    this._timer.Enabled = true;
  }

  /// <summary>Обрабатываем тик таймера</summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void Timer_Tick(object sender, EventArgs e)
  {
    this._timer.Enabled = false;
    try
    {
      lock (this._checkPoints)
      {
        List<Guid> guidList = new List<Guid>();
        foreach (KeyValuePair<Guid, ThreadedStepControl.CheckPointArgsEx> checkPoint in this._checkPoints)
        {
          if (!checkPoint.Value.Handled)
          {
            this.WriteCheckPoint(checkPoint.Key, (CheckPointArgs) checkPoint.Value);
            if (checkPoint.Value.Progress == 100)
              guidList.Add(checkPoint.Key);
            checkPoint.Value.Handled = true;
          }
        }
        if (guidList.Count <= 0)
          return;
        foreach (Guid key in guidList)
          this._checkPoints.Remove(key);
      }
    }
    finally
    {
      this._timer.Enabled = true;
    }
  }

  /// <summary>Метод задачи</summary>
  /// <param name="obj"></param>
  private void TaskMethod(object obj)
  {
    IPumpTask pumpTask = obj as IPumpTask;
    try
    {
      pumpTask.Start();
    }
    catch (Exception ex)
    {
      if (ex is ThreadAbortException)
        return;
      ExceptionHelper.ExceptionService.ShowException(ex);
      this.errorPresent = true;
    }
  }

  /// <summary>Экстренно завершаем работу шага</summary>
  private void OnCancel()
  {
    if (this.mainThread == null || !this.mainThread.IsAlive)
      return;
    this.cancelPresent = true;
    this.mainThread.Abort();
  }

  /// <summary>Обрабатываем событие об завершении текущего потока</summary>
  /// <param name="sender">GUID текущего пампера</param>
  /// <param name="index">Номер закончившегося пампера в коллекции (0, 1, 2, 3 ....)</param>
  private void Task_OnEnd(Guid sender, int index)
  {
    if (this.InvokeRequired)
      this.BeginInvoke((Delegate) new ThreadedStepControl.OnEndDelegate(this.SetEndTask), (object) sender, (object) index);
    else
      this.SetEndTask(sender, index);
  }

  private void SetEndTask(Guid sender, int index)
  {
    this.progressBar2.Value = Convert.ToInt32(100 * (index + 1) / this._countPumpers);
  }

  /// <summary>Пишем на форму очередной CheckPoint</summary>
  /// <param name="sender">GUID текущего пампера</param>
  /// <param name="e">Аргументы передаваемые с событием изменения статуса задачи</param>
  private void WriteCheckPoint(Guid sender, CheckPointArgs e)
  {
    foreach (iGRow row in (IEnumerable) this.iGrid1.Rows)
    {
      if (sender.Equals((Guid) row.Tag))
      {
        row.Cells[1].Value = (object) Convert.ToString(e.Progress);
        row.Cells[2].Value = (object) e.Status;
        row.Cells[3].Value = (object) ThreadedStepControl.ToReadableString(e.WorkTime);
        this.progressBar1.Value = e.Progress;
        this.label1.Text = e.Status;
        break;
      }
    }
  }

  public static string ToReadableString(TimeSpan span)
  {
    StringBuilder stringBuilder = new StringBuilder();
    int totalHours = (int) span.TotalHours;
    if (totalHours > 0)
    {
      stringBuilder.AppendFormat("{0:0} ", (object) totalHours);
      if (totalHours == 1)
        stringBuilder.Append("час");
      else if (totalHours > 1 && totalHours < 5)
        stringBuilder.Append("часа");
      else
        stringBuilder.Append("часов");
      stringBuilder.Append(" ");
    }
    if (span.Minutes > 0)
      stringBuilder.AppendFormat("{0} мин.", (object) span.Minutes);
    else
      stringBuilder.AppendFormat("{0} сек.", (object) span.Seconds);
    return stringBuilder.ToString();
  }

  /// <summary>Обрабатываем событие из текущего пампера</summary>
  /// <param name="sender">GUID текущего пампера</param>
  /// <param name="e">Аргументы передаваемые с событием изменения статуса задачи</param>
  private void SetCheckPoint(Guid sender, CheckPointArgs e)
  {
    ThreadedStepControl.CheckPointArgsEx checkPointArgsEx;
    if (this._checkPoints.TryGetValue(sender, out checkPointArgsEx))
    {
      checkPointArgsEx.Progress = e.Progress;
      checkPointArgsEx.Status = e.Status;
      checkPointArgsEx.WorkTime = e.WorkTime;
      checkPointArgsEx.Handled = false;
    }
    else
      this._checkPoints.Add(sender, new ThreadedStepControl.CheckPointArgsEx(e.Status, e.Progress, e.WorkTime));
  }

  /// <summary>Обрабатываем событие из текущего пампера</summary>
  /// <param name="sender">GUID текущего пампера</param>
  /// <param name="e">Аргументы передаваемые с событием изменения статуса задачи</param>
  private void Task_OnCheckPoint(Guid sender, CheckPointArgs e)
  {
    if (this.InvokeRequired)
      this.BeginInvoke((Delegate) new CheckPointDelegate(this.SetCheckPoint), (object) sender, (object) e);
    else
      this.SetCheckPoint(sender, e);
  }

  /// <summary>Событие</summary>
  public event OnEndEventHandler OnEndSaveSettings;

  /// <summary>
  /// Виртуальный метод для сохранения данных на шаге настроек
  /// </summary>
  /// <returns></returns>
  public override SaveSettingsResult SaveSettings() => SaveSettingsResult.ssrError;

  /// <summary>Нажали клавишу "Отмена"</summary>
  public override void Cancel()
  {
    if (this.InvokeRequired)
      this.Invoke((Delegate) new MethodInvoker(this.OnCancel));
    else
      this.OnCancel();
  }

  public override void RefreshControl() => this.iGrid1.Rows.Clear();

  public void LoadConfiguration()
  {
    FormStorageEx formStorageEx = new FormStorageEx((Control) this);
    formStorageEx.Load();
    foreach (iGCol col in (IEnumerable) this.iGrid1.Cols)
    {
      string name = $"Column{col.Index}_Width";
      if (formStorageEx.HasAttribute(name))
        col.Width = Convert.ToInt32(formStorageEx.GetAttribute(name));
    }
  }

  public void LoadConfiguration(IConfiguration cfg)
  {
  }

  public void SaveConfiguration()
  {
    FormStorageEx formStorageEx = new FormStorageEx((Control) this);
    foreach (iGCol col in (IEnumerable) this.iGrid1.Cols)
    {
      string name = $"Column{col.Index}_Width";
      formStorageEx.AddAttribute(name, col.Width.ToString());
    }
    formStorageEx.Save();
  }

  public void SaveConfiguration(IConfiguration cfg)
  {
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
    iGColPattern iGcolPattern1 = new iGColPattern();
    iGColPattern iGcolPattern2 = new iGColPattern();
    iGColPattern iGcolPattern3 = new iGColPattern();
    iGColPattern iGcolPattern4 = new iGColPattern();
    this.iGrid1Col0CellStyle = new iGCellStyle(true);
    this.iGrid1Col0ColHdrStyle = new iGColHdrStyle(true);
    this.iGrid1Col1CellStyle = new iGCellStyle(true);
    this.iGrid1Col1ColHdrStyle = new iGColHdrStyle(true);
    this.iGrid1Col2CellStyle = new iGCellStyle(true);
    this.iGrid1Col2ColHdrStyle = new iGColHdrStyle(true);
    this.panel1 = new Panel();
    this.progressBar1 = new ProgressBar();
    this.progressBar2 = new ProgressBar();
    this.label1 = new Label();
    this.label2 = new Label();
    this.panel2 = new Panel();
    this.iGrid1 = new iGrid();
    this.iGrid1DefaultCellStyle = new iGCellStyle(true);
    this.iGrid1DefaultColHdrStyle = new iGColHdrStyle(true);
    this.iGrid1RowTextColCellStyle = new iGCellStyle(true);
    this.iGrid1Col3CellStyle = new iGCellStyle(true);
    this.iGrid1Col3ColHdrStyle = new iGColHdrStyle(true);
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    ((ISupportInitialize) this.iGrid1).BeginInit();
    this.SuspendLayout();
    this.iGrid1Col0CellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
    this.panel1.Controls.Add((Control) this.progressBar1);
    this.panel1.Controls.Add((Control) this.progressBar2);
    this.panel1.Controls.Add((Control) this.label1);
    this.panel1.Controls.Add((Control) this.label2);
    this.panel1.Dock = DockStyle.Bottom;
    this.panel1.Location = new Point(0, 294);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(742, 100);
    this.panel1.TabIndex = 0;
    this.progressBar1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.progressBar1.BackColor = SystemColors.Control;
    this.progressBar1.Location = new Point(18, 29);
    this.progressBar1.Name = "progressBar1";
    this.progressBar1.Size = new Size(709, 14);
    this.progressBar1.Step = 100;
    this.progressBar1.TabIndex = 4;
    this.progressBar1.Value = 50;
    this.progressBar2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.progressBar2.Location = new Point(18, 62);
    this.progressBar2.Name = "progressBar2";
    this.progressBar2.Size = new Size(709, 14);
    this.progressBar2.Step = 100;
    this.progressBar2.TabIndex = 5;
    this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.label1.Location = new Point(18, 11);
    this.label1.Name = "label1";
    this.label1.Size = new Size(709, 15);
    this.label1.TabIndex = 6;
    this.label1.Text = "Ход выполнения текущей операции";
    this.label1.TextAlign = ContentAlignment.MiddleCenter;
    this.label2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.label2.Location = new Point(18, 44);
    this.label2.Name = "label2";
    this.label2.Size = new Size(709, 15);
    this.label2.TabIndex = 7;
    this.label2.Text = "Ход выполнения текущего шага";
    this.label2.TextAlign = ContentAlignment.MiddleCenter;
    this.panel2.Controls.Add((Control) this.iGrid1);
    this.panel2.Dock = DockStyle.Fill;
    this.panel2.Location = new Point(0, 0);
    this.panel2.Name = "panel2";
    this.panel2.Size = new Size(742, 294);
    this.panel2.TabIndex = 1;
    this.iGrid1.AutoResizeCols = true;
    iGcolPattern1.AllowGrouping = false;
    iGcolPattern1.AllowMoving = false;
    iGcolPattern1.CellStyle = this.iGrid1Col0CellStyle;
    iGcolPattern1.ColHdrStyle = this.iGrid1Col0ColHdrStyle;
    iGcolPattern1.SortOrder = iGSortOrder.None;
    iGcolPattern1.SortType = iGSortType.None;
    iGcolPattern1.Text = (object) "Наименование операции";
    iGcolPattern1.Width = 251;
    iGcolPattern2.AllowGrouping = false;
    iGcolPattern2.AllowMoving = false;
    iGcolPattern2.CellStyle = this.iGrid1Col1CellStyle;
    iGcolPattern2.ColHdrStyle = this.iGrid1Col1ColHdrStyle;
    iGcolPattern2.SortOrder = iGSortOrder.None;
    iGcolPattern2.SortType = iGSortType.None;
    iGcolPattern2.Text = (object) "Выполнение";
    iGcolPattern2.Width = 78;
    iGcolPattern3.AllowGrouping = false;
    iGcolPattern3.AllowMoving = false;
    iGcolPattern3.CellStyle = this.iGrid1Col2CellStyle;
    iGcolPattern3.ColHdrStyle = this.iGrid1Col2ColHdrStyle;
    iGcolPattern3.SortOrder = iGSortOrder.None;
    iGcolPattern3.SortType = iGSortType.None;
    iGcolPattern3.Text = (object) "Статус";
    iGcolPattern3.Width = 297;
    iGcolPattern4.CellStyle = this.iGrid1Col3CellStyle;
    iGcolPattern4.ColHdrStyle = this.iGrid1Col3ColHdrStyle;
    iGcolPattern4.Text = (object) "Время выполнения";
    iGcolPattern4.Width = 112 /*0x70*/;
    this.iGrid1.Cols.AddRange(new iGColPattern[4]
    {
      iGcolPattern1,
      iGcolPattern2,
      iGcolPattern3,
      iGcolPattern4
    });
    this.iGrid1.DefaultCol.CellStyle = this.iGrid1DefaultCellStyle;
    this.iGrid1.DefaultCol.ColHdrStyle = this.iGrid1DefaultColHdrStyle;
    this.iGrid1.Dock = DockStyle.Fill;
    this.iGrid1.Header.Height = 19;
    this.iGrid1.Location = new Point(0, 0);
    this.iGrid1.Name = "iGrid1";
    this.iGrid1.ReadOnly = true;
    this.iGrid1.RowTextCol.CellStyle = this.iGrid1RowTextColCellStyle;
    this.iGrid1.Size = new Size(742, 294);
    this.iGrid1.TabIndex = 6;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.panel2);
    this.Controls.Add((Control) this.panel1);
    this.Name = nameof (ThreadedStepControl);
    this.Size = new Size(742, 394);
    this.panel1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    ((ISupportInitialize) this.iGrid1).EndInit();
    this.ResumeLayout(false);
  }

  /// <summary>
  /// Расширенный класс CheckPointArgs
  /// Аргументы передаваемые с событием изменения статуса задачи + свойство "Обработано"
  /// </summary>
  private class CheckPointArgsEx : CheckPointArgs
  {
    /// <summary>Обработано</summary>
    public bool Handled;

    public CheckPointArgsEx(string status, int progress, TimeSpan workTime)
      : base(status, progress, workTime)
    {
      this.Handled = false;
    }
  }

  /// <summary>
  /// Делегат для генерации события об завершении текущего потока
  /// </summary>
  /// <param name="sender">GUID текущего пампера</param>
  /// <param name="index">Номер закончившегося пампера в коллекции (0, 1, 2, 3 ....)</param>
  private delegate void OnEndDelegate(Guid sender, int index);
}
