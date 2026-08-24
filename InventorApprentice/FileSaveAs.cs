// Decompiled with JetBrains decompiler
// Type: InventorApprentice.FileSaveAs
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[InterfaceType(2)]
[Guid("9CAF98A3-33EA-11D3-B78F-0060B0F159EF")]
[TypeLibType(4096 /*0x1000*/)]
[ComImport]
public interface FileSaveAs
{
  [DispId(50339334)]
  [TypeLibFunc(64 /*0x40*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void _WhereUsed([MarshalAs(UnmanagedType.IUnknown), In] object Document, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UNKNOWN), In, Out] ref object[] OwningDocuments);

  [DispId(50339335)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void AddFileToSave([MarshalAs(UnmanagedType.IUnknown), In] object Document, [MarshalAs(UnmanagedType.BStr), In] string TargetFileName);

  [DispId(50337032)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void ExecuteSaveAs();

  [DispId(50337033)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void ExecuteSaveCopyAs();

  [DispId(50337034)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void ExecuteSave();
}
