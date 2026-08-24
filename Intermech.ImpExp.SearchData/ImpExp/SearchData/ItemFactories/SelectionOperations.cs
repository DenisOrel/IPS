// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.ItemFactories.SelectionOperations
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.SearchData.ItemFactories;

[TypeConverter(typeof (EnumDescConverter))]
[Description("Операторы отношений в условиях выборки")]
[Category("SQL")]
public enum SelectionOperations
{
  [Description("")] None = -1, // 0xFFFFFFFF
  [Description("Равно")] Equal = 0,
  [Description("Не равно")] NotEqual = 1,
  [Description("Содержит строку")] Substring = 2,
  [Description("Начинается со строки")] StartString = 3,
  [Description("Заканчивается строкой")] EndString = 4,
  [Description("Меньше")] Less = 5,
  [Description("Меньше или равно")] LessOrEqual = 6,
  [Description("Больше")] Greater = 7,
  [Description("Больше или равно")] GreaterOrEqual = 8,
  [Description("Не содержит строку")] NotSubstring = 9,
}
