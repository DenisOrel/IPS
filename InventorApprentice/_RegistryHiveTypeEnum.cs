// Decompiled with JetBrains decompiler
// Type: InventorApprentice._RegistryHiveTypeEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(16 /*0x10*/)]
[Guid("207D3495-FF0C-11D2-B785-0060B0F159EF")]
public enum _RegistryHiveTypeEnum
{
  kInventorHiveLM = 2560, // 0x00000A00
  kInventorHiveCU = 2561, // 0x00000A01
  kHKEY_CLASSES_ROOT = 2562, // 0x00000A02
  kHKEY_LOCAL_MACHINE = 2563, // 0x00000A03
  kHKEY_CURRENT_USER = 2564, // 0x00000A04
  kCurrentInventorVersionHiveCU = 2565, // 0x00000A05
}
