// Decompiled with JetBrains decompiler
// Type: InventorApprentice.StorageTypeEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("FBCADB34-9CBE-11D3-B799-0060B0F159EF")]
public enum StorageTypeEnum
{
  kUnknownStorage = 6145, // 0x00001801
  kFileStorage = 6146, // 0x00001802
  kStreamStorage = 6147, // 0x00001803
  kFileOrStreamStorage = 6148, // 0x00001804
  kStorageStorage = 6149, // 0x00001805
}
