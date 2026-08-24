// Decompiled with JetBrains decompiler
// Type: InventorApprentice.PromptMessageRestrictionsEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("FC30104D-6D4D-4C05-9760-CBBEF00F9701")]
public enum PromptMessageRestrictionsEnum
{
  kNoRestrictions = 0,
  kDontAllowNeverAgain = 1,
  kDontAllowNoMoreThisSession = 2,
  kDontAllowButton1NeverAgain = 8,
  kDontAllowButton1NoMoreThisSession = 16, // 0x00000010
  kDontAllowButton2NeverAgain = 64, // 0x00000040
  kDontAllowButton2NoMoreThisSession = 128, // 0x00000080
  kDontAllowButton3NeverAgain = 512, // 0x00000200
  kDontAllowButton3NoMoreThisSession = 1024, // 0x00000400
}
