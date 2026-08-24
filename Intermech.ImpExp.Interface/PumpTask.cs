// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.PumpTask
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.ImpExp.Interface.PumpStatistics;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Interface;

public class PumpTask : IPumpTask
{
  private MethodInvoker _method;
  private DateTime _startTime = DateTime.MinValue;
  public object Tag;

  /// <summary>Сохранить статистику работы пампера</summary>
  private void LogPumpStatistics()
  {
    PumpStatisticsService service1 = ApplicationServices.Container.GetService<PumpStatisticsService>();
    if (service1 == null)
      return;
    ILogFile service2 = ApplicationServices.Container.GetService<ILogFile>();
    if (service2 == null)
      return;
    Intermech.ImpExp.Interface.PumpStatistics.PumpStatistics pumpStatistics = service1[this.GUID];
    foreach (string statisticsKey in (IEnumerable<string>) pumpStatistics.GetStatisticsKeys())
    {
      int num;
      if (pumpStatistics.IntStat.TryGetValue(statisticsKey, out num))
        service2.WriteMessage($"{statisticsKey}: {num}");
      string str;
      if (pumpStatistics.StringStat.TryGetValue(statisticsKey, out str))
        service2.WriteMessage($"{statisticsKey}: {str}");
    }
  }

  public PumpTask(Guid guid, MethodInvoker method, string description, PumpTaskType type)
  {
    this.GUID = guid;
    this._method = method;
    this.Description = description;
    this.Type = type;
  }

  public Guid GUID { get; }

  public event CheckPointDelegate OnCheckPoint;

  public event OnReadCountRecordsDelegate OnReadCountRecords;

  public string Description { get; }

  public void Start()
  {
    this._startTime = DateTime.Now;
    this._method();
    this.LogPumpStatistics();
  }

  public PumpTaskType Type { get; }

  public bool Repumpble { get; set; }

  /// <summary>Метод генерит событие изменения статуса</summary>
  public void OnCheckPointEvent(string status, int progress)
  {
    CheckPointDelegate onCheckPoint = this.OnCheckPoint;
    if (onCheckPoint == null)
      return;
    onCheckPoint(this.GUID, new CheckPointArgs(status, progress, this._startTime > DateTime.MinValue ? DateTime.Now - this._startTime : new TimeSpan(0L)));
  }

  public void OnReadCountRecordsEvent(long count)
  {
    OnReadCountRecordsDelegate readCountRecords = this.OnReadCountRecords;
    if (readCountRecords == null)
      return;
    readCountRecords(this.GUID, new OnReadCountRecordsArgs(count));
  }
}
