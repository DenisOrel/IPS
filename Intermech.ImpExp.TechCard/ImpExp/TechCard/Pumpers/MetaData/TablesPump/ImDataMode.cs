// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump.ImDataMode
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump;

internal enum ImDataMode
{
  [Description("Обычные данные")] Data,
  [Description("Имя другой таблицы")] Table,
  [Description("Ссылка на рисунок")] Image,
  [Description("Ссылка на описание")] Text,
}
