// Decompiled with JetBrains decompiler
// Type: InventorApprentice.FileManagerEventsSink
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("CA8E18AF-5EA2-4A45-BA43-FF3914C5C200")]
[TypeLibType(4096 /*0x1000*/)]
[InterfaceType(2)]
[ComImport]
public interface FileManagerEventsSink
{
  [DispId(50378753)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void OnFileDelete([MarshalAs(UnmanagedType.BStr), In] string FullFileName, [MarshalAs(UnmanagedType.Interface), In] NameValueMap Context, out HandlingCodeEnum HandlingCode);

  [DispId(50378754)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void OnFileCopy(
    [MarshalAs(UnmanagedType.BStr), In] string SourceFullFileName,
    [MarshalAs(UnmanagedType.BStr), In] string DestinationFullFileName,
    [In] bool Copy,
    [MarshalAs(UnmanagedType.Interface), In] NameValueMap Context,
    out HandlingCodeEnum HandlingCode);
}
