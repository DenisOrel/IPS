// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxReferencedFileDescriptor
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("00C8476D-E79F-11D2-B785-0060B0F159EF")]
[InterfaceType(1)]
[ComConversionLoss]
[TypeLibType(16 /*0x10*/)]
[ComImport]
public interface IRxReferencedFileDescriptor
{
  [DispId(50337665)]
  DocumentTypeEnum DocumentType { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetLogicalFileName([MarshalAs(UnmanagedType.BStr)] out string pbstrRelativeFileName, [MarshalAs(UnmanagedType.BStr)] out string pbstrLibraryName);

  [TypeLibFunc(64 /*0x40*/)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void _PutLogicalFileName([MarshalAs(UnmanagedType.BStr), In] string bstrRelativeFileName, [MarshalAs(UnmanagedType.BStr), In] string bstrLibraryName);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void PutLogicalFileNameUsingFull([MarshalAs(UnmanagedType.BStr), In] string bstrFullFileName);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetCustomLogicalFileName([In, Out] ref int pnSize, [Out] IntPtr ppCustomLogicalFileName);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void PutCustomLogicalFileName([In] int nSize, [In] ref byte pCustomLogicalFileName);

  [DispId(50337671)]
  string DisplayName { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }

  [DispId(50337672)]
  string FullFileName { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }

  [DispId(50337673)]
  int FileSaveCounter { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(50337674)]
  object FileVersion { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IUnknown)] get; }

  [DispId(50337675)]
  IRxFileAndReferences ReferencedDocument { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }
}
