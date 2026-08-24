// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.TerminateType
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>Операция на которой произошло падение</summary>
[Serializable]
public enum TerminateType
{
  /// <summary>Падения не было</summary>
  None,
  /// <summary>На импорте метаданных</summary>
  SaveMetadata,
  /// <summary>На одной из задач закачки</summary>
  Pump,
  /// <summary>Закачка завершена</summary>
  Complete,
}
