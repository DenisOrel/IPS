// Decompiled with JetBrains decompiler
// Type: InventorApprentice.DrawingSheetStatusBits
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("93822436-662D-44EC-ACFB-2D7D0729E9D7")]
public enum DrawingSheetStatusBits
{
  kUpToDateDrawingSheet = 0,
  kGeomOutOfDateDrawingSheet = 1,
  kAssyPositionOutOfDateDrawingSheet = 2,
  kAssyCompositionOutOfDateDrawingSheet = 4,
  kStandardOutOfDateDrawingSheet = 8,
  kResourceTemplateOutOfDateDrawingSheet = 16, // 0x00000010
  kParameterizedTextOutOfDateDrawingSheet = 32, // 0x00000020
  kUnknownOutOfDateDrawingSheet = 64, // 0x00000040
  kNoDataDrawingSheet = 128, // 0x00000080
  kProcessingPreciseDisplayDrawingSheet = 256, // 0x00000100
}
