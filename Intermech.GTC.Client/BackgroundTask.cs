// Decompiled with JetBrains decompiler
// Type: Intermech.GTC.Client.BackgroundTask
// Assembly: Intermech.GTC.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 539B70F6-18D3-4230-8795-0EE95CBE5B1C
// Assembly location: D:\IPS\Client\Intermech.GTC.Client.dll

using Intermech.Client.Core;
using Intermech.GTC.Interfaces;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Timers;

#nullable disable
namespace Intermech.GTC.Client;

internal class BackgroundTask : CustomBackgroundTask
{
  private IServiceForBackgroundTask _srv;
  private System.Timers.Timer _timer = new System.Timers.Timer(1000.0);
  private Guid _taskGuid = Guid.NewGuid();
  protected BackgroundTaskShowMode TaskShowMode;

  public BackgroundTask(IServiceForBackgroundTask srv)
  {
    this.Result = (object) 1;
    this._srv = srv;
    this._imageIndex = -1;
    this._canStop = true;
    this._canTerminate = true;
    this._canPause = true;
    this._canResume = true;
  }

  public override BackgroundTaskShowMode ShowMode => this.TaskShowMode;

  public override void Stop()
  {
    if (!this._canStop)
      return;
    this._srv.StoppingTask(this._taskGuid);
    this.State = BackgroundTaskState.Stopped;
    while (!this._srv.StoppedTask(this._taskGuid))
      Thread.Sleep(1000);
    this.FinishTask();
  }

  public override void Pause()
  {
    if (!this._canPause)
      return;
    this._timer.Stop();
    this._srv.PauseTask(this._taskGuid);
    this.State = BackgroundTaskState.Paused;
  }

  public override void Resume()
  {
    if (!this._canResume)
      return;
    this._srv.ResumeTask(this._taskGuid);
    this._timer.Start();
    this.State = BackgroundTaskState.Running;
  }

  public override void Terminate()
  {
    if (!this._canTerminate)
      return;
    this._srv.StoppingTask(this._taskGuid);
    this.State = BackgroundTaskState.Terminated;
    this.RemoveTask();
  }

  private void On_timer_Elapsed(object sender, ElapsedEventArgs e)
  {
    int state = 0;
    if (this.State == BackgroundTaskState.Running)
    {
      string text;
      this.Value = (object) this._srv.GetCompleted(this._taskGuid, out state, out text);
      this.Name = text;
    }
    if (state >= 0)
      return;
    this.FinishTask();
  }

  private void FinishTask()
  {
    this.RemoveTask();
    BackgroundTaskResult result = this._srv.GetResult(this._taskGuid);
    if (result == null)
      return;
    this.ShowResult(result);
    this.FireEvent(result);
  }

  private void RemoveTask()
  {
    this._timer.Stop();
    this._timer.Elapsed -= new ElapsedEventHandler(this.On_timer_Elapsed);
    this.OnChanged(BackgroundTaskChangedType.Dispose);
  }

  private void ShowResult(BackgroundTaskResult result)
  {
    if (result.Messages.Count <= 0 || !(ServicesManager.GetService(typeof (IOutputView)) is IOutputView service))
      return;
    StringBuilder stringBuilder = new StringBuilder();
    foreach (BackgroundTaskMessage message in result.Messages)
    {
      stringBuilder.AppendLine(message.Message);
      if (message.Exception != null)
        stringBuilder.AppendLine(message.Exception.Message);
    }
    service.ClearText(this.Name);
    service.WriteString(this.Name, stringBuilder.ToString());
    service.Activate(this.Name);
    service.ShowView();
  }

  private void FireEvent(BackgroundTaskResult result)
  {
    if (result.ChangedObjects.Count <= 0 && result.CreatedObjects.Count <= 0)
      return;
    if (ServicesManager.GetService(typeof (INotificationService)) is INotificationService service && result.ChangedObjects.Count > 0)
      service.FireEvent((object) null, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", (IList<long>) result.ChangedObjects));
    if (service == null || result.CreatedObjects.Count <= 0)
      return;
    service.FireEvent((object) null, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsCreated", (IList<long>) result.CreatedObjects));
  }

  public void StartTask(object inputData)
  {
    try
    {
      if (this._srv == null)
        throw new Exception("Не удалось получить сервис для обновления объектов");
      using (SessionKeeper sessionKeeper = new SessionKeeper())
        this._srv.StartTask(sessionKeeper.Session.SessionGUID, this._taskGuid, this.Name, inputData);
      this._timer.Elapsed += new ElapsedEventHandler(this.On_timer_Elapsed);
      this._timer.Start();
      this.State = BackgroundTaskState.Running;
    }
    catch (Exception ex)
    {
      this.State = BackgroundTaskState.Error;
      this.RemoveTask();
      ExceptionHelper.ExceptionService.ShowException(ex);
    }
  }
}
