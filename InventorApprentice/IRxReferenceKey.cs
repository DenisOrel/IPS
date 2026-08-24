// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxReferenceKey
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(16 /*0x10*/)]
[Guid("5DF86026-6B16-11D3-B794-0060B0F159EF")]
[InterfaceType(1)]
[ComConversionLoss]
[ComImport]
public interface IRxReferenceKey
{
  [DispId(67109121 /*0x04000101*/)]
  Guid ObjectType { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  IntPtr get_Key([In] uint hKeyContext, [In, Out] ref uint pdwKeySize);

  [DispId(67109125 /*0x04000105*/)]
  int TransientKey { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(67109126 /*0x04000106*/)]
  Guid RevisionId { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }
}
