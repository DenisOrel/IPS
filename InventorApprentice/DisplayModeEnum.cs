// Decompiled with JetBrains decompiler
// Type: InventorApprentice.DisplayModeEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("9C88D3AB-C3EB-11D3-B79E-0060B0F159EF")]
public enum DisplayModeEnum
{
  kWireframeRendering = 8706, // 0x00002202
  [TypeLibVar(64 /*0x40*/)] kHiddenEdgeRendering = 8707, // 0x00002203
  kShadedWithHiddenEdgesRendering = 8707, // 0x00002203
  kShadedRendering = 8708, // 0x00002204
  kRealisticRendering = 8709, // 0x00002205
  kShadedWithEdgesRendering = 8710, // 0x00002206
  kWireframeNoHiddenEdges = 8711, // 0x00002207
  kWireframeWithHiddenEdgesRendering = 8712, // 0x00002208
  kMonochromeRendering = 8713, // 0x00002209
  kWatercolorRendering = 8714, // 0x0000220A
  kIllustrationRendering = 8715, // 0x0000220B
}
