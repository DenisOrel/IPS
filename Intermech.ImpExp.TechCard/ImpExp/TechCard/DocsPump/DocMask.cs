// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.DocsPump.DocMask
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.DocsPump;

[Serializable]
public enum DocMask
{
  dmNone = 0,
  dmComplCardStructure = 1,
  dmDontRepeatOsn = 2,
  dmNoPageNumbers = 4,
  dmPlaceOsnIntoEmptyFlds = 8,
  dmNewCehFromNewHeadList = 16, // 0x00000010
  dmNewCehFromNewList = 32, // 0x00000020
  dmAddToOglav = 64, // 0x00000040
  dmDocByDetal = 128, // 0x00000080
  dmDocOutOfComplect = 256, // 0x00000100
  dmVspMatByPlainText = 512, // 0x00000200
}
