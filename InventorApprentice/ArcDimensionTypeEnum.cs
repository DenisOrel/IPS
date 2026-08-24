// Decompiled with JetBrains decompiler
// Type: InventorApprentice.ArcDimensionTypeEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("2CC2CD2E-CCD5-4749-BC7E-A3312F29C1A9")]
public enum ArcDimensionTypeEnum
{
  kRadialArcDimension = 65537, // 0x00010001
  kDiametricArcDimension = 65538, // 0x00010002
  kAngleArcDimension = 65539, // 0x00010003
  kArcLengthArcDimension = 65540, // 0x00010004
  kChordLengthArcDimension = 65541, // 0x00010005
}
