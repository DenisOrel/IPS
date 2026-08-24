// Decompiled with JetBrains decompiler
// Type: InventorApprentice.BendTransitionEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("FFD2AEBA-5B42-4053-BFD6-9CBDDC7418DC")]
public enum BendTransitionEnum
{
  kNoBendTransition = 28161, // 0x00006E01
  kIntersectionBendTransition = 28162, // 0x00006E02
  kStraightLineBendTransition = 28163, // 0x00006E03
  kArcBendTransition = 28164, // 0x00006E04
  kDefaultBendTransition = 28165, // 0x00006E05
  kTrimToBendBendTransition = 28166, // 0x00006E06
}
