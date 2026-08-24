// Decompiled with JetBrains decompiler
// Type: InventorApprentice.LevelOfDetailEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("4F3F1B37-4207-4890-A873-95F0616EB85D")]
public enum LevelOfDetailEnum
{
  kMasterLevelOfDetail = 56065, // 0x0000DB01
  kAllComponentsSuppressedLevelOfDetail = 56066, // 0x0000DB02
  kAllPartsSuppressedLevelOfDetail = 56067, // 0x0000DB03
  kAllContentSuppressedLevelOfDetail = 56068, // 0x0000DB04
  kSandboxLevelOfDetail = 56069, // 0x0000DB05
  kTransientLevelOfDetail = 56070, // 0x0000DB06
  kSubstituteLevelOfDetail = 56071, // 0x0000DB07
  kCustomLevelOfDetail = 56072, // 0x0000DB08
  kLastActiveLevelOfDetail = 56073, // 0x0000DB09
}
