// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.CheckPointArgs
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>
/// Аргументы передаваемые с событием изменения статуса задачи
/// </summary>
public class CheckPointArgs
{
  /// <summary>Строка статуса</summary>
  public string Status;
  /// <summary>Процент выполнения</summary>
  public int Progress;
  /// <summary>Время работы</summary>
  public TimeSpan WorkTime;

  public CheckPointArgs(string status, int progress, TimeSpan workTime)
  {
    this.Status = status;
    this.Progress = progress;
    this.WorkTime = workTime;
  }
}
