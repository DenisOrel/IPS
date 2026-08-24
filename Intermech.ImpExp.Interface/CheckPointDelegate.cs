// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.CheckPointDelegate
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>
/// Делегат для генерации события изменения статуса задачи
/// </summary>
/// <param name="sender">Глобальный идентификатор задачи</param>
/// <param name="e">Параметры</param>
public delegate void CheckPointDelegate(Guid sender, CheckPointArgs e);
