// Decompiled with JetBrains decompiler
// Type: InventorApprentice.FileManagementEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("AEC63B24-113F-46FC-B85D-A50AA362BC02")]
public enum FileManagementEnum
{
  kNoForceFile = 0,
  kDeleteFileMask = 1,
  kForceFile = 1,
  kOverwriteExistingFile = 32, // 0x00000020
  kOverwriteReservedFile = 64, // 0x00000040
  kOverwriteReadOnlyFile = 128, // 0x00000080
  kCopyFileMask = 224, // 0x000000E0
  kMoveFileMask = 225, // 0x000000E1
}
