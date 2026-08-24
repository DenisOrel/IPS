// Decompiled with JetBrains decompiler
// Type: InventorApprentice.DataIO
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(4096 /*0x1000*/)]
[Guid("FBCADB33-9CBE-11D3-B799-0060B0F159EF")]
[InterfaceType(2)]
[ComImport]
public interface DataIO
{
  [DispId(6401)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void WriteDataToFile([MarshalAs(UnmanagedType.BStr), In] string Format, [MarshalAs(UnmanagedType.BStr), In] string FileName);

  [DispId(6402)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void WriteDataToStream([MarshalAs(UnmanagedType.BStr), In] string Format, [MarshalAs(UnmanagedType.IUnknown), In, Out] ref object Stream);

  [DispId(6403)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void ReadDataFromFile([MarshalAs(UnmanagedType.BStr), In] string Format, [MarshalAs(UnmanagedType.BStr), In] string FileName);

  [DispId(6404)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void ReadDataFromStream([MarshalAs(UnmanagedType.BStr), In] string Format, [MarshalAs(UnmanagedType.IUnknown), In] object Stream);

  [DispId(6405)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetOutputFormats([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR), In, Out] ref string[] Formats, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_I4), In, Out] ref StorageTypeEnum[] StorageTypes);

  [DispId(6406)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetInputFormats([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR), In, Out] ref string[] Formats, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_I4), In, Out] ref StorageTypeEnum[] StorageTypes);
}
