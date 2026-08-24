// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.EntityPumModes
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.TechCard;

[TypeConverter(typeof (EnumDescConverter))]
public enum EntityPumModes
{
  [Description("Новый атрибут")] NewAttr,
  [Description("Существующий атрибут")] ExistAttr,
  [Description("Существующее понятие")] ExistEntity,
  [Description("Найден в настройках Imbase")] FoundInImbase,
}
