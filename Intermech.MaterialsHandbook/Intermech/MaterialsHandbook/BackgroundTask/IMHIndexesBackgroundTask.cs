// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.BackgroundTask.IMHIndexesBackgroundTask
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Imbase.Indexes;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.MaterialsHandbook;
using Intermech.Localization;
using System;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook.BackgroundTask;

public class IMHIndexesBackgroundTask : Control, IBackgroundTask
{
  protected Thread _thread;
  protected int _minValue;
  protected int _maxValue;
  protected int _value;
  protected string _category;
  protected BackgroundTaskState _state;
  protected object _result;
  protected EventWaitHandle _event;
  protected bool _paused = true;
  private IMHIndexesHelper _helper;
  private System.Timers.Timer _timer = new System.Timers.Timer();
  private IIMHIndexingService _indexingService;
  private bool _correctClosed = true;

  public IMHIndexesBackgroundTask(IMHIndexesHelper helper)
  {
    this.Name = LocalizationHolder.rm.GetString("IMH_Indexing");
    this._category = LocalizationHolder.rm.GetString("IMH_Indexing_Caption");
    this.CreateHandle();
    this._event = new EventWaitHandle(true, EventResetMode.ManualReset);
    if (helper != null)
    {
      this._helper = helper;
      this.ImageIndex = this._helper.ImageIndex;
    }
    this._timer.Elapsed += new ElapsedEventHandler(this.On_timer_Elapsed);
    this._timer.Interval = 3000.0;
    this._thread = new Thread(new ThreadStart(this.ThreadProc));
    this.Resume();
    this._minValue = 0;
    this._maxValue = 100;
    this._value = 0;
  }

  private void On_timer_Elapsed(object sender, ElapsedEventArgs e)
  {
    this.Value = (object) this._indexingService.Completed;
  }

  public event BackgroundTaskChangedEventHandler Changed;

  public int ImageIndex { get; }

  public int MaximumValue
  {
    get => this._maxValue;
    set
    {
      if (value == this._maxValue)
        return;
      this._maxValue = value;
      this.OnChanged(BackgroundTaskChangedType.Value);
    }
  }

  public int MinimumValue
  {
    get => this._minValue;
    set
    {
      if (this._minValue == value)
        return;
      this._minValue = value;
      this.OnChanged(BackgroundTaskChangedType.Value);
    }
  }

  public object Value
  {
    get => (object) this._value;
    set
    {
      int result;
      if (value == null || !int.TryParse(Convert.ToString(value), out result) || this._value == result)
        return;
      this._value = result;
      this.OnChanged(BackgroundTaskChangedType.Value);
    }
  }

  public new string Name { get; }

  public object Result
  {
    get => this._result;
    set
    {
      if (this._result == value)
        return;
      this._result = value;
      this.OnChanged(BackgroundTaskChangedType.Result);
    }
  }

  public BackgroundTaskState State
  {
    get => this._state;
    set
    {
      if (this._state == value)
        return;
      this._state = value;
      this.OnChanged(BackgroundTaskChangedType.State);
    }
  }

  public BackgroundTaskShowMode ShowMode => BackgroundTaskShowMode.Progress;

  public bool Active
  {
    get => this._state == BackgroundTaskState.Running || this._state == BackgroundTaskState.Paused;
  }

  public void SetMaxMin(int max, int min)
  {
    this._minValue = max >= min ? min : throw new ArgumentException(LocalizationHolder.rm.GetString("IMH_MinMaxValue_Error"));
    this._maxValue = max;
    this.OnChanged(BackgroundTaskChangedType.Value);
  }

  public bool CanStop() => true;

  public bool CanPause() => false;

  public bool CanResume() => false;

  public bool CanTerminate() => false;

  public void Stop()
  {
    if (this._correctClosed)
      this._indexingService.MarkAsFree();
    this.OnChanged(BackgroundTaskChangedType.Dispose);
  }

  public void Pause()
  {
    this.State = BackgroundTaskState.Paused;
    this._paused = true;
    this._event.Reset();
  }

  public void Resume()
  {
    this.State = BackgroundTaskState.Running;
    switch (this._thread.ThreadState)
    {
      case ThreadState.Unstarted:
        this._thread.Start();
        this._paused = false;
        break;
      case ThreadState.Stopped:
        break;
      default:
        if (!this._paused)
          break;
        this._event.Set();
        this._paused = false;
        break;
    }
  }

  public void Terminate()
  {
  }

  protected void ThreadProc()
  {
    this._correctClosed = true;
    try
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        this._indexingService = sessionKeeper.Session.GetCustomService(typeof (IIMHIndexingService)) as IIMHIndexingService;
        if (this._indexingService == null)
          throw new Exception(LocalizationHolder.rm.GetString("IMH_Null_Indexing_Service"));
        if (!this._indexingService.IsBusy)
        {
          this._timer.Start();
          if (this._helper.NeedIndexindMaterials)
          {
            long objectIdByConstName = IMHHelper.GetObjectIDByConstName("BASE_MATERIALS_CTL");
            this._indexingService.IndexingMaterial(sessionKeeper.Session.SessionGUID, objectIdByConstName);
          }
          if ((this._helper.Actions & IndexesStatus.Added) == IndexesStatus.Added)
            this._indexingService.Add(sessionKeeper.Session.SessionGUID, this._helper.SourceID, this._helper.AddedIndexes);
          else if ((this._helper.Actions & IndexesStatus.Removed) == IndexesStatus.Removed)
            this._indexingService.RemoveIndexes(sessionKeeper.Session.SessionGUID, this._helper.SourceID, this._helper.RemovedIndexes);
          else if ((this._helper.Actions & IndexesStatus.Changed) == IndexesStatus.Changed)
            this._indexingService.UpdateIndexes(sessionKeeper.Session.SessionGUID, this._helper.SourceID, this._helper.AddedIndexes, this._helper.RemovedIndexes);
          this._timer.Stop();
        }
        else
          this._correctClosed = false;
      }
    }
    catch (Exception ex)
    {
      this.SetState(BackgroundTaskState.Error);
      this.SetThrow(ex);
    }
    finally
    {
      this._timer.Elapsed -= new ElapsedEventHandler(this.On_timer_Elapsed);
      this.Stop();
    }
  }

  protected void OnChanged(BackgroundTaskChangedType type)
  {
    if (this.InvokeRequired)
      this.Invoke((Delegate) new IMHIndexesBackgroundTask.SetChangedCallback(this.OnChanged), (object) type);
    BackgroundTaskChangedEventHandler changed = this.Changed;
    if (changed == null)
      return;
    changed((object) this, type);
  }

  private void SetState(BackgroundTaskState state)
  {
    if (this.InvokeRequired)
      this.Invoke((Delegate) new IMHIndexesBackgroundTask.SetStateCallback(this.SetState), (object) state);
    else
      this.State = state;
  }

  private void SetThrow(Exception e)
  {
    if (this.InvokeRequired)
    {
      this.Invoke((Delegate) new IMHIndexesBackgroundTask.SetThrowCallback(this.SetThrow), (object) e);
    }
    else
    {
      IOutputView service = ServiceUtils.GetService<IOutputView>((object) ApplicationServices.Container, false);
      if (service != null)
      {
        string text1 = $"{LocalizationHolder.rm.GetString("IMH_Object")}: {this.Name}";
        service.WriteString(this._category, text1);
        string text2 = $"{LocalizationHolder.rm.GetString("IMH_Error")}: {e.Message}";
        service.WriteString(this._category, text2);
        service.Activate(this._category);
        service.ShowView();
      }
      int num = (int) MessageBox.Show(LocalizationHolder.rm.GetString("IMH_BackgroundTask_Error"), this.Name, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }
  }

  public delegate void SetStateCallback(BackgroundTaskState state);

  public delegate void SetValueCallback(object value);

  public delegate void SetThrowCallback(Exception e);

  public delegate void SetChangedCallback(BackgroundTaskChangedType type);
}
