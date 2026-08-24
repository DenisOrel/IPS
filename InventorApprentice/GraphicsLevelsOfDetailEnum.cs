// Decompiled with JetBrains decompiler
// Type: InventorApprentice.GraphicsLevelsOfDetailEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("39849E70-E5AA-11D3-B7A1-0060B0F159EF")]
public enum GraphicsLevelsOfDetailEnum
{
  kMereDotRes = 8,
  kVeryCoarseRes = 32, // 0x00000020
  kCoarseRes = 128, // 0x00000080
  kMediumRes = 512, // 0x00000200
  kFullScreenHighRes = 2048, // 0x00000800
  kZoomedInHighRes = 8192, // 0x00002000
}
