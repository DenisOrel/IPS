// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.ItemFactories.ThematicParamsType
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.Search.ItemFactories;

[TypeConverter(typeof (EnumDescConverter))]
[Description("Типы тематических параметров")]
[Category("Misc")]
internal enum ThematicParamsType
{
  [Description("Неизвестный тип")] ptUnknown = -1, // 0xFFFFFFFF
  [Description("Строка")] ptString = 0,
  [Description("Целое число")] ptInteger = 1,
  [Description("Дробное число")] ptDouble = 2,
  [Description("Дата")] ptDateTime = 3,
  [Description("Текст")] ptText = 4,
}
