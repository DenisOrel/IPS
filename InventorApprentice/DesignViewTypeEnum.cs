// Decompiled with JetBrains decompiler
// Type: InventorApprentice.DesignViewTypeEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("4545E3EA-632E-42B3-A0EB-DF3978A1DC20")]
public enum DesignViewTypeEnum
{
  kMasterDesignViewType = 57345, // 0x0000E001
  [TypeLibVar(64 /*0x40*/)] kPrivateDesignViewType = 57346, // 0x0000E002
  kPublicDesignViewType = 57347, // 0x0000E003
  kTransientDesignViewType = 57348, // 0x0000E004
  kAllVisibleDesignViewType = 57349, // 0x0000E005
  kNothingVisibleDesignViewType = 57350, // 0x0000E006
  kLastActiveDesignViewType = 57351, // 0x0000E007
}
