// Decompiled with JetBrains decompiler
// Type: Intermech.GTC.Client.ExtendedBackgroundTaskBase
// Assembly: Intermech.GTC.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 539B70F6-18D3-4230-8795-0EE95CBE5B1C
// Assembly location: D:\IPS\Client\Intermech.GTC.Client.dll

using Intermech.Interfaces.Client;
using System;
using System.Threading;

#nullable disable
namespace Intermech.GTC.Client;

public class ExtendedBackgroundTaskBase : IExtendedBackgroundTask, IBackgroundTask
{
  protected object LockObject = new object();
  protected IAsyncResult AsyncResult;
  protected bool Terminated;
  protected bool Stopped;
  protected bool Paused;
  protected int _Value;
  protected string _Name;
  protected int _MaximumValue;

  public string Name
  {
    get => this._Name;
    set
    {
      lock (this.LockObject)
        this._Name = value;
      this.OnChanged(BackgroundTaskChangedType.Text);
    }
  }

  public bool IsProcessStoped
  {
    get
    {
      while (this.Paused)
      {
        if (this.Stopped)
          return true;
        Thread.Sleep(1000);
      }
      return this.Stopped;
    }
  }

  public void IncProgress() => this.Value = (object) (Convert.ToInt32(this.Value) + 1);

  public event BackgroundTaskChangedEventHandler Changed;

  public virtual int ImageIndex => -1;

  public int MaximumValue
  {
    get => this._MaximumValue;
    set
    {
      lock (this.LockObject)
        this._MaximumValue = value;
    }
  }

  public int MinimumValue
  {
    get => 0;
    set
    {
    }
  }

  public object Value
  {
    get => (object) this._Value;
    set
    {
      lock (this.LockObject)
        this._Value = Convert.ToInt32(value);
      this.OnChanged(BackgroundTaskChangedType.Value);
    }
  }

  public object Result
  {
    get => (object) 1;
    set
    {
    }
  }

  public BackgroundTaskState State
  {
    get
    {
      switch (this.Terminated ? -2 : (this.AsyncResult == null ? 0 : (this.Paused ? -1 : 1)))
      {
        case -2:
          return BackgroundTaskState.Terminated;
        case -1:
          return BackgroundTaskState.Paused;
        case 0:
          return BackgroundTaskState.Stopped;
        default:
          return BackgroundTaskState.Running;
      }
    }
    set
    {
    }
  }

  public BackgroundTaskShowMode ShowMode => BackgroundTaskShowMode.Progress;

  public bool Active => this.State > BackgroundTaskState.Running;

  public void SetMaxMin(int max, int min)
  {
  }

  public bool CanStop()
  {
    return this.State == BackgroundTaskState.Running || this.State == BackgroundTaskState.Paused;
  }

  public bool CanPause() => this.State == BackgroundTaskState.Running;

  public bool CanResume() => true;

  public bool CanTerminate() => true;

  public void Stop()
  {
    if (this.AsyncResult != null)
    {
      lock (this.LockObject)
        this.Stopped = true;
    }
    this.OnChanged(BackgroundTaskChangedType.Dispose);
  }

  public void Pause()
  {
    lock (this.LockObject)
      this.Paused = true;
    this.OnChanged(BackgroundTaskChangedType.State);
  }

  public virtual void Resume()
  {
  }

  public void Terminate() => this.OnChanged(BackgroundTaskChangedType.Dispose);

  protected void OnChanged(BackgroundTaskChangedType type)
  {
    if (this.Changed == null)
      return;
    this.Changed((object) this, type);
  }
}
