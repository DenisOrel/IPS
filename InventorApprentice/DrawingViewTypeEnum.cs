// Decompiled with JetBrains decompiler
// Type: InventorApprentice.DrawingViewTypeEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("4F589650-207C-11D4-B7A5-0060B0F159EF")]
public enum DrawingViewTypeEnum
{
  [TypeLibVar(64 /*0x40*/)] kDefaultDrawingViewType = 10497, // 0x00002901
  [TypeLibVar(64 /*0x40*/)] kCustomDrawingViewType = 10498, // 0x00002902
  kAuxiliaryDrawingViewType = 10499, // 0x00002903
  [TypeLibVar(64 /*0x40*/)] kOLEAttachmentDrawingViewType = 10500, // 0x00002904
  kStandardDrawingViewType = 10501, // 0x00002905
  kDetailDrawingViewType = 10502, // 0x00002906
  kSectionDrawingViewType = 10503, // 0x00002907
  kProjectedDrawingViewType = 10504, // 0x00002908
  kDraftDrawingViewType = 10505, // 0x00002909
  kAssociativeDraftDrawingViewType = 10506, // 0x0000290A
  kOverlayDrawingViewType = 10507, // 0x0000290B
}
