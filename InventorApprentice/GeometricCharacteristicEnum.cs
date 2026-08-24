// Decompiled with JetBrains decompiler
// Type: InventorApprentice.GeometricCharacteristicEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("20A8F412-590D-4BBA-9978-E5F807FAF601")]
public enum GeometricCharacteristicEnum
{
  kStraightness = 1,
  kFlatness = 2,
  kCircularity = 4,
  kProfileOfAnyLine = 8,
  kProfileOfAnySurface = 16, // 0x00000010
  kAngularity = 32, // 0x00000020
  kPerpendicularity = 64, // 0x00000040
  kParallelism = 128, // 0x00000080
  kPosition = 256, // 0x00000100
  kConcentricityAndCoaxiality = 512, // 0x00000200
  kCircularRunout = 1024, // 0x00000400
  kSymmetry = 2048, // 0x00000800
  kTotalRunout = 4096, // 0x00001000
  kCylindricity = 8192, // 0x00002000
  kParallelProfile = 16384, // 0x00004000
  kAxisIntersection = 32768, // 0x00008000
  kCircularRunoutFilled = 65536, // 0x00010000
  kTotalRunoutFilled = 131072, // 0x00020000
}
