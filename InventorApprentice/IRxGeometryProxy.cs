// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxGeometryProxy
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(16 /*0x10*/)]
[Guid("5DF86010-6B16-11D3-B794-0060B0F159EF")]
[InterfaceType(1)]
[ComImport]
public interface IRxGeometryProxy
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  IntPtr get_NativeObject([In] ref Guid riid);

  [DispId(67113987)]
  IRxComponentOccurrence ContainingOccurrence { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }
}
