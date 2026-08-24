// Decompiled with JetBrains decompiler
// Type: Interop.Viewdraw.VdObjectType
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Interop.Viewdraw;

[CompilerGenerated]
[TypeIdentifier("4ADEF4E1-690A-11CE-9261-0020C5E26659", "Interop.Viewdraw.VdObjectType")]
public enum VdObjectType
{
  VDTS_LINE = 0,
  VDTS_BOX = 1,
  VDTS_TEXT = 2,
  VDTS_CIRCLE = 3,
  VDTS_ARC = 4,
  VDTS_NET = 5,
  VDTS_ATTRIBUTE = 6,
  VDTS_COMPONENT = 7,
  VDTS_LABEL = 8,
  VDTS_PIN = 9,
  VDTS_OATTRIBUTE = 10, // 0x0000000A
  VDTS_BLOCK = 11, // 0x0000000B
  VDTS_COMPPIN = 12, // 0x0000000C
  VDTS_SEGMENT = 13, // 0x0000000D
  VDTS_CONNECTION = 14, // 0x0000000E
  VDTS_LIBRARY = 15, // 0x0000000F
  VDTS_POINT = 16, // 0x00000010
  VDTS_DESIGN = 17, // 0x00000011
  VDTS_RIPPER = 18, // 0x00000012
  VDTL_ANNOTATION = 1036, // 0x0000040C
  VDTL_SELECTION = 1037, // 0x0000040D
  VDTL_BACKGROUND = 1038, // 0x0000040E
  VDTL_VALUE = 1039, // 0x0000040F
  VDTL_BORDER = 1040, // 0x00000410
  VDTL_HIGHLIGHT = 1041, // 0x00000411
}
