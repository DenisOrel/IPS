// Decompiled with JetBrains decompiler
// Type: InventorApprentice.SurfaceTypeEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("5DF86088-6B16-11D3-B794-0060B0F159EF")]
public enum SurfaceTypeEnum
{
  kUnknownSurface = 5889, // 0x00001701
  kPlaneSurface = 5890, // 0x00001702
  kCylinderSurface = 5891, // 0x00001703
  kEllipticalCylinderSurface = 5892, // 0x00001704
  kConeSurface = 5893, // 0x00001705
  kEllipticalConeSurface = 5894, // 0x00001706
  kTorusSurface = 5895, // 0x00001707
  kSphereSurface = 5896, // 0x00001708
  kBSplineSurface = 5897, // 0x00001709
}
