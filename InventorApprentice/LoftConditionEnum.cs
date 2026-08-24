// Decompiled with JetBrains decompiler
// Type: InventorApprentice.LoftConditionEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("1C08C10F-8357-49BD-9E14-C6FA66892012")]
public enum LoftConditionEnum
{
  kFreeLoftCondition = 34305, // 0x00008601
  kTangentLoftCondition = 34306, // 0x00008602
  [TypeLibVar(64 /*0x40*/)] kAngleLoftCondition = 34307, // 0x00008603
  kDirectionLoftCondition = 34307, // 0x00008603
  kSmoothLoftCondition = 34308, // 0x00008604
  kSharpPointLoftCondition = 34309, // 0x00008605
  kTangentToPlaneLoftCondition = 34310, // 0x00008606
}
