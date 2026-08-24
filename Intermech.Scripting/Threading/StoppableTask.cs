// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Threading.StoppableTask
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Diagnostics;
using System.Threading;

#nullable disable
namespace Intermech.Scripting.Threading;

/// <summary>
/// Позволяет выполнить задачу в фоновом потоке с возможностью прервать выполнение. Класс не является thread-safe.
/// </summary>
public sealed class StoppableTask : IDisposable
{
  private ApartmentState apartmentState;
  private Thread workerThread;
  private StoppableTaskData taskData;
  private bool isDisposed;
  private EventHandler onCompleted;

  public StoppableTask(ApartmentState apartmentState = ApartmentState.STA)
  {
    this.apartmentState = apartmentState;
  }

  public void Dispose()
  {
    if (this.isDisposed)
      return;
    try
    {
      this.Abort();
      if (this.taskData == null)
        return;
      this.taskData.Dispose();
    }
    finally
    {
      this.isDisposed = true;
    }
  }

  private void RequireNotDisposed()
  {
    if (this.isDisposed)
      throw new ObjectDisposedException(this.GetType().FullName);
  }

  public void StartAction(Action taskMethod)
  {
    if (taskMethod == null)
      throw new ArgumentNullException(nameof (taskMethod));
    this.RequireNotDisposed();
    this.Start((Func<object>) (() =>
    {
      taskMethod();
      return (object) null;
    }));
  }

  public void Start(Func<object> taskMethod)
  {
    if (taskMethod == null)
      throw new ArgumentNullException(nameof (taskMethod));
    this.RequireNotDisposed();
    if (this.taskData != null)
    {
      lock (this.taskData)
      {
        if (!this.taskData.IsCompleted)
          throw new InvalidOperationException("Another task is already started.");
      }
      this.taskData.Dispose();
      this.taskData = (StoppableTaskData) null;
    }
    this.taskData = new StoppableTaskData(this, taskMethod);
    this.taskData.OnCompleted = this.onCompleted;
    this.workerThread = new Thread(new ParameterizedThreadStart(StoppableTask.WorkerRoutine));
    this.workerThread.IsBackground = true;
    this.workerThread.Name = "Stoppable task worker";
    this.workerThread.SetApartmentState(this.apartmentState);
    try
    {
      this.workerThread.Start((object) this.taskData);
    }
    catch
    {
      this.workerThread = (Thread) null;
      this.taskData.Dispose();
      this.taskData = (StoppableTaskData) null;
      throw;
    }
  }

  public bool Wait(TimeSpan timeout)
  {
    this.RequireNotDisposed();
    return this.Wait((int) Math.Round(timeout.TotalMilliseconds));
  }

  public bool Wait(int timeout)
  {
    if (timeout < 0 && timeout != -1)
      throw new ArgumentOutOfRangeException(nameof (timeout));
    this.RequireNotDisposed();
    if (this.taskData == null)
      throw new InvalidOperationException("A task is not started.");
    ManualResetEventSlim completedWaitEvent;
    lock (this.taskData)
    {
      if (this.taskData.IsCompleted)
        return true;
      completedWaitEvent = this.taskData.CompletedWaitEvent;
    }
    return completedWaitEvent.Wait(timeout);
  }

  public void Abort()
  {
    this.RequireNotDisposed();
    if (this.taskData == null)
      return;
    lock (this.taskData)
    {
      if (this.taskData.IsCompleted)
        return;
    }
    this.workerThread.Abort();
    if (!this.workerThread.Join(1000))
    {
      lock (this.taskData)
      {
        StoppableTaskData stoppableTaskData = this.taskData.ForkAbortedState();
        this.taskData.OnCompleted = (EventHandler) null;
        this.taskData = stoppableTaskData;
      }
    }
    this.taskData.ReportCompleted();
  }

  public bool IsDisposed
  {
    [DebuggerStepThrough] get => this.isDisposed;
  }

  public bool IsCompleted
  {
    [DebuggerStepThrough] get
    {
      if (this.taskData == null)
        return false;
      lock (this.taskData)
        return this.taskData.IsCompleted;
    }
  }

  public StoppableTaskState State
  {
    [DebuggerStepThrough] get
    {
      if (this.taskData == null)
        return StoppableTaskState.NotRunning;
      lock (this.taskData)
        return this.taskData.State;
    }
  }

  public object Result
  {
    [DebuggerStepThrough] get
    {
      if (this.taskData == null)
        return (object) null;
      lock (this.taskData)
        return this.taskData.Result;
    }
  }

  public Exception Exception
  {
    [DebuggerStepThrough] get
    {
      if (this.taskData == null)
        return (Exception) null;
      lock (this.taskData)
        return this.taskData.Exception;
    }
  }

  public ApartmentState ApartmentState
  {
    [DebuggerStepThrough] get => this.apartmentState;
  }

  public event EventHandler OnCompleted
  {
    add
    {
      this.onCompleted += value;
      this.UpdateOnCompletedHandler();
    }
    remove
    {
      this.onCompleted -= value;
      this.UpdateOnCompletedHandler();
    }
  }

  private void UpdateOnCompletedHandler()
  {
    if (this.taskData == null)
      return;
    lock (this.taskData)
      this.taskData.OnCompleted = this.onCompleted;
  }

  private static void WorkerRoutine(object state)
  {
    StoppableTaskData stoppableTaskData = (StoppableTaskData) state;
    try
    {
      lock (stoppableTaskData)
        stoppableTaskData.State = StoppableTaskState.Running;
      object result = stoppableTaskData.Method();
      lock (stoppableTaskData)
        stoppableTaskData.SetFinishedState(result);
      stoppableTaskData.ReportCompleted();
    }
    catch (ThreadAbortException ex)
    {
      lock (stoppableTaskData)
        stoppableTaskData.SetAbortedState();
      throw;
    }
    catch (Exception ex)
    {
      lock (stoppableTaskData)
        stoppableTaskData.SetFailedState(ex);
      stoppableTaskData.ReportCompleted();
    }
  }
}
