// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IOMechanismEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("6BA435D1-BBD6-11D4-8DE6-0010B541CAA8")]
public enum IOMechanismEnum
{
  kUnspecifiedIOMechanism = 13057, // 0x00003301
  kDataDropIOMechanism = 13058, // 0x00003302
  kFileBrowseIOMechanism = 13059, // 0x00003303
  kPasteSpecialIOMechanism = 13060, // 0x00003304
}
