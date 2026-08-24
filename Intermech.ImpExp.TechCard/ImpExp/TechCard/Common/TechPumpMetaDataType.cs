// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.TechPumpMetaDataType
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common;

[Flags]
public enum TechPumpMetaDataType
{
  None = 0,
  AutoSelection = 1,
  ScriptForms = 2,
  ExpertTables = 4,
  ExpertFormula = 8,
  DocumentSettings = 16, // 0x00000010
}
