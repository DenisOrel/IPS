// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ItemFactories.ImDataMode
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.Imbase.ItemFactories;

internal enum ImDataMode
{
  [Description("Обычные данные")] IDM_DATA,
  [Description("Имя другой таблицы")] IDM_TABLE,
  [Description("Ссылка на рисунок")] IDM_IMAGE,
  [Description("Ссылка на описание")] IDM_TEXT,
}
