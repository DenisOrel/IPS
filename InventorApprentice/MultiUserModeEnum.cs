// Decompiled with JetBrains decompiler
// Type: InventorApprentice.MultiUserModeEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("644A3053-564F-4193-90B6-DD8C16B2F5B5")]
public enum MultiUserModeEnum
{
  kSingleUserMode = 36353, // 0x00008E01
  kSharedMode = 36354, // 0x00008E02
  kSemiIsolatedMode = 36355, // 0x00008E03
  [TypeLibVar(64 /*0x40*/)] kSemiIsolatedNoCheckoutMode = 36356, // 0x00008E04
  kVaultMode = 36357, // 0x00008E05
}
