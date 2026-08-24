// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump.EntityExistsStatus
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;

[TypeConverter(typeof (EnumDescConverter))]
[Serializable]
internal enum EntityExistsStatus
{
  [Description("Не найдено")] None,
  [Description("Имя атрибута")] ByName,
  [Description("Краткое наименование")] ByShortName,
  [Description("Псевдоним")] ByAlias,
}
