// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.TechPumpMode
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common;

public enum TechPumpMode
{
  [Description("Закачка всех данных")] tpmAll,
  [Description("Закачка по архивам")] tpmArchive,
  [Description("Закачка по списку ТП")] tpmTpList,
  [Description("Закачка по составу выбранных изделий")] tpmArtList,
  [Description("Закачка по составу производственных заказов(ПЗ)")] tpmProdZakList,
}
