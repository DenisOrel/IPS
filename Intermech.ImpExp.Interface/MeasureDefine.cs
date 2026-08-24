// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.MeasureDefine
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>
/// Поиск значения единицы измерения исходя из значения записанного в старом Imbase.
/// Базовый класс для различных задач импорта.
/// </summary>
public class MeasureDefine
{
  protected virtual Guid FindMeasureGuid(string unit) => Guid.Empty;

  protected virtual Guid FindDefaultMeasureGuid(long physicalValueID) => Guid.Empty;

  public Guid GetMeasure(long physicalValueID, string unit)
  {
    Guid measure = Guid.Empty;
    if (unit != null && unit != string.Empty)
      measure = this.FindMeasureGuid(unit);
    if (measure == Guid.Empty && physicalValueID != 0L)
      measure = this.FindDefaultMeasureGuid(physicalValueID);
    return measure;
  }
}
