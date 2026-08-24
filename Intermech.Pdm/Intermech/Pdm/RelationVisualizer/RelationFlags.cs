// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.RelationFlags
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

[Flags]
public enum RelationFlags
{
  None = 0,
  StructLinks = 1,
  AssocLinks = 2,
  AllLinks = AssocLinks | StructLinks, // 0x00000003
}
