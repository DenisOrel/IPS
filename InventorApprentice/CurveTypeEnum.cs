// Decompiled with JetBrains decompiler
// Type: InventorApprentice.CurveTypeEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("5DF86044-6B16-11D3-B794-0060B0F159EF")]
public enum CurveTypeEnum
{
  kUnknownCurve = 5121, // 0x00001401
  kLineCurve = 5122, // 0x00001402
  kLineSegmentCurve = 5123, // 0x00001403
  kCircleCurve = 5124, // 0x00001404
  kCircularArcCurve = 5125, // 0x00001405
  kEllipseFullCurve = 5126, // 0x00001406
  kEllipticalArcCurve = 5127, // 0x00001407
  kBSplineCurve = 5128, // 0x00001408
  kPolylineCurve = 5129, // 0x00001409
}
