// Decompiled with JetBrains decompiler
// Type: InventorApprentice.DimensionDisplayTypeEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("E7F22274-B6DF-4DD9-8ABD-18ECEC9AC3DF")]
public enum DimensionDisplayTypeEnum
{
  kDimensionDisplayAsValue = 34817, // 0x00008801
  kDimensionDisplayAsName = 34818, // 0x00008802
  [TypeLibVar(64 /*0x40*/)] kDimensionDisplayAsExpession = 34819, // 0x00008803
  kDimensionDisplayAsExpression = 34819, // 0x00008803
  [TypeLibVar(64 /*0x40*/)] kDimensionDisplayAsToerance = 34820, // 0x00008804
  kDimensionDisplayAsTolerance = 34820, // 0x00008804
  kDimensionDisplayAsPreciseValue = 34821, // 0x00008805
}
