// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxGeometricLocate
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("5DF86015-6B16-11D3-B794-0060B0F159EF")]
[TypeLibType(16 /*0x10*/)]
[InterfaceType(1)]
[ComImport]
public interface IRxGeometricLocate
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void PointLocate(
    [ComAliasName("InventorApprentice.BoreLineStruct"), In] ref BoreLineStruct pBoreline,
    [In] uint dwNumTypes,
    [In] ref Guid pTypes,
    [MarshalAs(UnmanagedType.Interface)] out IRxEnumReferenceKeys ppEnumReferenceKey);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void ShapeLocate(
    [ComAliasName("InventorApprentice.SelectPrismStruct"), In] ref SelectPrismStruct pShape,
    [In] uint dwNumTypes,
    [In] ref Guid pTypes,
    [MarshalAs(UnmanagedType.Interface)] out IRxEnumReferenceKeys ppEnumReferenceKey);
}
