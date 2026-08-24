// Decompiled with JetBrains decompiler
// Type: InventorApprentice.LocationTypeEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("AFBBA399-0A51-11D3-B787-0060B0F159EF")]
public enum LocationTypeEnum
{
  kWorkspaceLocation = 45057, // 0x0000B001
  [TypeLibVar(64 /*0x40*/)] kLocalLocation = 45058, // 0x0000B002
  kWorkgroupLocation = 45059, // 0x0000B003
  kLibraryLocation = 45060, // 0x0000B004
  kUnknownLocation = 45061, // 0x0000B005
  [TypeLibVar(64 /*0x40*/)] kOwnerDirectoryLocation = 45062, // 0x0000B006
}
