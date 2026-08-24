// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump.ImDataTypeEx
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump;

internal enum ImDataTypeEx
{
  [Description("Не поддерживается")] Unknown,
  [Description("Строковое")] String,
  [Description("Целое")] Integer,
  [Description("Вещественное")] Float,
  [Description("Логическое")] Bool,
  [Description("Ссылка")] Reference,
  [Description("Набор")] Set,
  [Description("")] Adt,
}
