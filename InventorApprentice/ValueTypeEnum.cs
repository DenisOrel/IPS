// Decompiled with JetBrains decompiler
// Type: InventorApprentice.ValueTypeEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("E5AFD5AD-2FFA-4700-95CB-4B5FEB1C80DA")]
public enum ValueTypeEnum
{
  kIntegerType = 14593, // 0x00003901
  kDoubleType = 14594, // 0x00003902
  kStringType = 14595, // 0x00003903
  kByteArrayType = 14596, // 0x00003904
  [TypeLibVar(64 /*0x40*/)] kBooleanType = 14597, // 0x00003905
}
