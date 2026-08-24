// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ItemFactories.ImLookupRecordType
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.Imbase.ItemFactories;

internal enum ImLookupRecordType
{
  [Description("Элемент (для поддержки старой версии)")] lrtItemOld,
  [Description("Папка")] lrtFolder,
  [Description("Список")] lrtList,
  [Description("Элемент")] lrtItem,
  [Description("Выборка из папок")] lrtTreeSel,
  [Description("Выборка из таблиц")] lrtTableSel,
  [Description("Выборка из каталога")] lrtCtlSel,
}
