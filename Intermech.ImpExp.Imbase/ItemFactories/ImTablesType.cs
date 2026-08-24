// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ItemFactories.ImTablesType
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.Imbase.ItemFactories;

internal enum ImTablesType
{
  [Description("Неизвестный тип")] IMTT_UNKNOWN,
  [Description("Таблица Каталога")] IMTT_CATALOG,
  [Description("Справочник")] IMTT_CTLREF,
  [Description("Технологический справочник")] IMTT_TECHREF,
  [Description("Пользовательская таблица")] IMTT_TABLE,
  [Description("Таблица индекса Каталога")] IMTT_INDEX,
  [Description("Таблица записей каталога")] IMTT_CTLREC,
}
