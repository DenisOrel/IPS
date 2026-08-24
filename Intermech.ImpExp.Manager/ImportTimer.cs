// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.ImportTimer
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using System;
using System.Threading;

#nullable disable
namespace Intermech.ImpExp.Manager;

internal class ImportTimer
{
  private long _currentTicks;
  private IImportingData _cacheData;
  private ICache _cache;
  private Thread _timerThread;
  private int _timerKey = 99;
  private bool _started;

  public bool Started => this._started;

  public event OnTickImportTimerHandler OnTickImportTimer;

  public ImportTimer(ICache cache, bool newImport)
  {
    this._cache = cache;
    this._cacheData = cache.GetCache(ImportingCategory.ImportingTimer);
    if (this._cacheData == null)
      return;
    DictionaryValue dictionaryValue = this._cacheData.GetValue(ImportingCategory.ImportingTimer, (object) this._timerKey);
    if (dictionaryValue == null)
      this._cacheData.AddValue(ImportingCategory.ImportingTimer, (object) this._timerKey, 0L);
    this._currentTicks = newImport ? 0L : (dictionaryValue != null ? dictionaryValue.NewObjectID : 0L);
  }

  public void Start()
  {
    if (this._cacheData == null)
      return;
    this._timerThread = new Thread(new ThreadStart(this.TimerMethod));
    this._timerThread.IsBackground = true;
    this._timerThread.Name = "ImportTimer_Thread";
    this._timerThread.Start();
  }

  public void Stop()
  {
    this._started = false;
    if (this._timerThread != null && this._timerThread.IsAlive)
    {
      this._timerThread.Abort();
      this._timerThread.Join();
    }
    if (this._cache == null)
      return;
    this._cache.ReleaseCache(ImportingCategory.ImportingTimer);
  }

  private void OnTick()
  {
    OnTickImportTimerHandler onTickImportTimer = this.OnTickImportTimer;
    if (onTickImportTimer == null)
      return;
    onTickImportTimer((object) this, new OnTickImportEventArgs(this._currentTicks));
  }

  private void SaveToDisk()
  {
    if (this._cacheData == null)
      return;
    this._cacheData.SetNewKey(ImportingCategory.ImportingTimer, (object) this._timerKey, this._currentTicks);
  }

  private void TimerMethod()
  {
    this.OnTick();
    this._started = true;
    try
    {
      int num = 0;
      while (true)
      {
        if (num > 59)
        {
          this.SaveToDisk();
          num = 0;
        }
        DateTime now = DateTime.Now;
        Thread.Sleep(1000);
        this._currentTicks += (DateTime.Now - now).Ticks;
        this.OnTick();
        ++num;
      }
    }
    finally
    {
      this._started = false;
    }
  }
}
