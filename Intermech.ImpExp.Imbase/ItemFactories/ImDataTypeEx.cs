// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ItemFactories.ImDataTypeEx
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.Imbase.ItemFactories;

internal enum ImDataTypeEx
{
  [Description("Не поддерживается")] IEX_UNKNOWN,
  [Description("Строковое")] IEX_STRING,
  [Description("Целое")] IEX_INTEGER,
  [Description("Вещественное")] IEX_FLOAT,
  [Description("Логическое")] IEX_BOOL,
  [Description("Ссылка")] IEX_REF,
  [Description("Набор")] IEX_SET,
  [Description("")] IEX_ADT,
  [Description("Пользователь")] IEX_USER,
}
