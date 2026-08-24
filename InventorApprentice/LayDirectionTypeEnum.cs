// Decompiled with JetBrains decompiler
// Type: InventorApprentice.LayDirectionTypeEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("E10A058E-0B40-40FB-8019-6565A272CF38")]
public enum LayDirectionTypeEnum
{
  kParallelToPlaneOfProjection = 1,
  kPerpendicularToPlaneOfProjection = 2,
  kAngularInBothDirections = 4,
  kMultidirectional = 8,
  kCircularRelativeToCenter = 16, // 0x00000010
  kRadialRelativeToCenter = 32, // 0x00000020
  kParticulateNondirectional = 64, // 0x00000040
}
