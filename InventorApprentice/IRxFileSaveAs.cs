// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxFileSaveAs
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("42C7E0BE-FDCF-11D2-B785-0060B0F159EF")]
[InterfaceType(1)]
[ComConversionLoss]
[TypeLibType(16 /*0x10*/)]
[ComImport]
public interface IRxFileSaveAs
{
  [TypeLibFunc(64 /*0x40*/)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void _WhereUsed([MarshalAs(UnmanagedType.IUnknown), In] object pDocument, [In, Out] ref int pnOwningDocuments, [Out] IntPtr pppOwningDocuments);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void AddFileToSave([MarshalAs(UnmanagedType.IUnknown), In] object pDocument, [MarshalAs(UnmanagedType.BStr), In] string pTargetFileName);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void ExecuteSaveAs();

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void ExecuteSaveCopyAs();

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void ExecuteSave();
}
