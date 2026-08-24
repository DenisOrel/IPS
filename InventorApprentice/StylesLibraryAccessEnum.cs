// Decompiled with JetBrains decompiler
// Type: InventorApprentice.StylesLibraryAccessEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("62A2561B-3438-4D1D-B2F0-79DF541C21D8")]
public enum StylesLibraryAccessEnum
{
  kReadOnlyStylesLibraryAccess = 54017, // 0x0000D301
  kReadWriteStylesLibraryAccess = 54018, // 0x0000D302
  [TypeLibVar(64 /*0x40*/)] kNoStylesLibraryAccess = 54019, // 0x0000D303
}
