// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.EntityPumpStatus
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.TechCard;

[TypeConverter(typeof (EnumDescConverter))]
[Serializable]
public enum EntityPumpStatus
{
  [Description("Не настроен")] None,
  [Description("Присутствует в базе")] Exists,
  [Description("Новый атрибут")] New,
  [Description("Настроен для закачки")] Commited,
  [Description("Не закачивается")] NotPump,
}
