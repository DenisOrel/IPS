// Decompiled with JetBrains decompiler
// Type: InventorApprentice.Curve2dTypeEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("5DF86045-6B16-11D3-B794-0060B0F159EF")]
public enum Curve2dTypeEnum
{
  kUnknownCurve2d = 5249, // 0x00001481
  kLineCurve2d = 5250, // 0x00001482
  kLineSegmentCurve2d = 5251, // 0x00001483
  kCircleCurve2d = 5252, // 0x00001484
  kCircularArcCurve2d = 5253, // 0x00001485
  kEllipseFullCurve2d = 5254, // 0x00001486
  kEllipticalArcCurve2d = 5255, // 0x00001487
  kBSplineCurve2d = 5256, // 0x00001488
  kPolylineCurve2d = 5257, // 0x00001489
}
