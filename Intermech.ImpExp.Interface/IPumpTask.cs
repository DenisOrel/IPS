// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.IPumpTask
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>
/// Интерфейс для унификации управления потоком выполнения задач
/// </summary>
public interface IPumpTask
{
  /// <summary>Тип задачи</summary>
  PumpTaskType Type { get; }

  /// <summary>Идентификатор задачи</summary>
  Guid GUID { get; }

  /// <summary>
  /// Событие, которое генериться при изменении статуса задачи
  /// </summary>
  event CheckPointDelegate OnCheckPoint;

  event OnReadCountRecordsDelegate OnReadCountRecords;

  /// <summary>Описание задачи</summary>
  string Description { get; }

  /// <summary>Задача дозакачиваемая</summary>
  bool Repumpble { get; }

  /// <summary>Запуск выполнения задачи</summary>
  void Start();
}
