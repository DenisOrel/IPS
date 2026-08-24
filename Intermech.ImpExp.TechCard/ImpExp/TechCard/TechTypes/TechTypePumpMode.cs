// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechTypes.TechTypePumpMode
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechTypes;

[Serializable]
public enum TechTypePumpMode
{
  [Description("Новый тип объекта")] NewObjType,
  [Description("Существующий тип объекта")] ExistObjType,
  [Description("Настроено программно")] LockedType,
  [Description("Не качается")] NotPumpType,
}
