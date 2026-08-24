// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.SortamentFilter
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;

#nullable disable
namespace Intermech.MaterialsHandbook;

internal class SortamentFilter
{
  internal Guid AttrGuid { get; set; }

  internal Condition Cond { get; set; }

  internal object Value { get; set; }

  public SortamentFilter(Guid g, Condition cond, object value)
  {
    this.AttrGuid = g;
    this.Cond = cond;
    this.Value = value;
  }
}
