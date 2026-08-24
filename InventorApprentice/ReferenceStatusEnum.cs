// Decompiled with JetBrains decompiler
// Type: InventorApprentice.ReferenceStatusEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("5899FDCD-113A-4643-9A86-CA8D12E232AF")]
public enum ReferenceStatusEnum
{
  kUnknownReference = 49665, // 0x0000C201
  kUpToDateReference = 49666, // 0x0000C202
  kOutOfDateReference = 49667, // 0x0000C203
  kMissingReference = 49668, // 0x0000C204
  kReplacedReference = 49669, // 0x0000C205
}
