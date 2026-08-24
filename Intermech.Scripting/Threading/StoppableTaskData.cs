// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Threading.StoppableTaskData
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Threading;

#nullable disable
namespace Intermech.Scripting.Threading;

internal sealed class StoppableTaskData : IDisposable
{
  private StoppableTask task;
  private bool isDisposed;

  public StoppableTaskData(StoppableTask task, Func<object> taskMethod)
  {
    this.task = task;
    this.Method = taskMethod;
    this.State = StoppableTaskState.NotRunning;
    this.CompletedWaitEvent = new ManualResetEventSlim(false);
  }

  ~StoppableTaskData() => this.DoDispose(false);

  public void Dispose()
  {
    GC.SuppressFinalize((object) this);
    this.DoDispose(true);
  }

  private void DoDispose(bool disposing)
  {
    if (this.isDisposed)
      return;
    this.isDisposed = true;
    this.CompletedWaitEvent.Dispose();
  }

  public StoppableTask Task => this.task;

  public Func<object> Method { get; private set; }

  public StoppableTaskState State { get; set; }

  public object Result { get; set; }

  public Exception Exception { get; set; }

  public ManualResetEventSlim CompletedWaitEvent { get; private set; }

  public EventHandler OnCompleted { get; set; }

  public bool IsCompleted
  {
    get
    {
      return this.State == StoppableTaskState.Finished || this.State == StoppableTaskState.Failed || this.State == StoppableTaskState.Aborted;
    }
  }

  public void SetFinishedState(object result)
  {
    this.Result = result;
    this.Exception = (Exception) null;
    this.State = StoppableTaskState.Finished;
  }

  public void SetFailedState(Exception x)
  {
    this.Result = (object) null;
    this.Exception = x;
    this.State = StoppableTaskState.Failed;
  }

  public void SetAbortedState()
  {
    this.Result = (object) null;
    this.Exception = (Exception) null;
    this.State = StoppableTaskState.Aborted;
  }

  public void ReportCompleted()
  {
    this.RaiseOnCompleted();
    this.CompletedWaitEvent.Set();
  }

  private void RaiseOnCompleted()
  {
    StoppableTask task = this.Task;
    EventHandler onCompleted = this.OnCompleted;
    if (onCompleted == null)
      return;
    onCompleted((object) task, EventArgs.Empty);
  }

  public StoppableTaskData ForkAbortedState()
  {
    StoppableTaskData stoppableTaskData = new StoppableTaskData(this.Task, this.Method);
    stoppableTaskData.OnCompleted = this.OnCompleted;
    stoppableTaskData.SetAbortedState();
    ManualResetEventSlim completedWaitEvent = stoppableTaskData.CompletedWaitEvent;
    stoppableTaskData.CompletedWaitEvent = this.CompletedWaitEvent;
    this.CompletedWaitEvent = completedWaitEvent;
    return stoppableTaskData;
  }
}
