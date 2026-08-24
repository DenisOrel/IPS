// Decompiled with JetBrains decompiler
// Type: InventorApprentice.ReferencedFileDescriptor
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(4112)]
[Guid("9E0BA9CA-E916-11D2-B785-0060B0F159EF")]
[InterfaceType(2)]
[ComImport]
public interface ReferencedFileDescriptor
{
  [DispId(50337537)]
  DocumentTypeEnum DocumentType { [DispId(50337537), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(50337538)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetLogicalFileName([MarshalAs(UnmanagedType.BStr)] out string RelativeFileName, [MarshalAs(UnmanagedType.BStr)] out string LibraryName);

  [DispId(50337540)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void PutLogicalFileNameUsingFull([MarshalAs(UnmanagedType.BStr), In] string FullFileName);

  [DispId(50337541)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UI1)]
  byte[] GetCustomLogicalFileName();

  [DispId(50337542)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void PutCustomLogicalFileName([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UI1), In, Out] ref byte[] CustomLogicalFileName);

  [DispId(50337543)]
  string DisplayName { [DispId(50337543), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }

  [DispId(50337544)]
  string FullFileName { [DispId(50337544), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }

  [DispId(50337545)]
  int FileSaveCounter { [DispId(50337545), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(50337547)]
  object ReferencedDocument { [DispId(50337547), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IUnknown)] get; }

  [DispId(50342413)]
  ReferenceStatusEnum ReferenceStatus { [DispId(50342413), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(50337551)]
  DocumentDescriptor DocumentDescriptor { [DispId(50337551), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(50337539)]
  [TypeLibFunc(64 /*0x40*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void _PutLogicalFileName([MarshalAs(UnmanagedType.BStr), In] string RelativeFileName, [MarshalAs(UnmanagedType.BStr), In] string LibraryName);

  [DispId(50337548)]
  [TypeLibFunc(64 /*0x40*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void PutLogicalFileNameUsingFullSpl([MarshalAs(UnmanagedType.BStr), In] string FullFileName);

  [DispId(50337549)]
  bool DifferentDocument { [DispId(50337549), TypeLibFunc(64 /*0x40*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(50337550)]
  bool DocumentFound { [TypeLibFunc(64 /*0x40*/), DispId(50337550), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }
}
