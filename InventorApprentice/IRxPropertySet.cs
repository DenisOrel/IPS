// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxPropertySet
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(16 /*0x10*/)]
[Guid("73F35CCD-ED6E-11D2-B785-0060B0F159EF")]
[InterfaceType(1)]
[ComImport]
public interface IRxPropertySet
{
  [DispId(50338689)]
  string DisplayName { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.BStr), In] set; }

  [DispId(50338690)]
  Guid InternalName { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  IRxProperty Create([In] int PropId, [MarshalAs(UnmanagedType.Struct), In] object PropValue, [MarshalAs(UnmanagedType.BStr), In] string Name);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  IRxProperty CreateByName([MarshalAs(UnmanagedType.Struct), In] object PropValue, [MarshalAs(UnmanagedType.BStr), In] string Name);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Delete();

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  IRxProperty GetByName([MarshalAs(UnmanagedType.BStr), In] string Name);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  IRxProperty GetByPropId([In] int PropId);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  IRxProperty GetByIndex([In] int index);

  [DispId(2130706438 /*0x7F000006*/)]
  int Count { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(50338697)]
  sbyte Dirty { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }
}
