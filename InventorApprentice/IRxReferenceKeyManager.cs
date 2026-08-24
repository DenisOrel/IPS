// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxReferenceKeyManager
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("5DF86028-6B16-11D3-B794-0060B0F159EF")]
[TypeLibType(16 /*0x10*/)]
[ComConversionLoss]
[InterfaceType(1)]
[ComImport]
public interface IRxReferenceKeyManager
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void CreateKeyContext(out uint phKeyContext);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SaveKeyContext([In] uint hKeyContext, [MarshalAs(UnmanagedType.Interface), In] IStream pKeyContextStream);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void LoadKeyContext(out uint phKeyContext, [MarshalAs(UnmanagedType.Interface), In] IStream pKeyContextStream);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void BindKeyToInterface(
    [In] uint hKeyContext,
    [In] ref Guid ObjectInterfaceIID,
    [In] uint dwKeySize,
    [In] ref byte pKey,
    out IntPtr ppPrimaryMatch,
    out SolutionNatureEnum pMatchType,
    [MarshalAs(UnmanagedType.Interface), In, Out] ref IEnumUnknown ppAllMatches);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void BindTransientKeyToInterface(
    [In] ref Guid ObjectInterfaceIID,
    [In] int TransientKey,
    out IntPtr ppMatch);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetTransientKeyFromKey(
    [In] uint hKeyContext,
    [In] uint dwKeySize,
    [In] ref byte pKey,
    out int pPrimaryTransientKey,
    out SolutionNatureEnum pMatchType,
    [In, Out] ref uint pdwNumMatches,
    [In, Out] IntPtr ppAllMatches);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  IntPtr GetKeyFromTransientKey([In] int TransientKey, [In] uint hKeyContext, [In, Out] ref uint pdwKeySize);
}
