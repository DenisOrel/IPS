// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ItemFactories.ImLookupDataType
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using System;
using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.Imbase.ItemFactories;

[Flags]
internal enum ImLookupDataType
{
  [Description("Определяется полем")] ldtNone = 0,
  [Description("Строковое значение")] ldtStr = 1,
  [Description("Целочисленное значение")] ldtInt = 2,
  [Description("Вещественное значение")] ldtDbl = 4,
  [Description("Именованное значение")] ldtNmd = 8,
}
