// Decompiled with JetBrains decompiler
// Type: InventorApprentice.FileAccessEventsSink
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("E51041E7-5DB6-4951-9F76-3ACA9B2E2A66")]
[TypeLibType(4096 /*0x1000*/)]
[InterfaceType(2)]
[ComImport]
public interface FileAccessEventsSink
{
  [DispId(50335665)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void OnFileResolution(
    [MarshalAs(UnmanagedType.BStr), In] string RelativeFileName,
    [MarshalAs(UnmanagedType.BStr), In] string LibraryName,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UI1), In, Out] ref byte[] CustomLogicalName,
    [In] EventTimingEnum BeforeOrAfter,
    [MarshalAs(UnmanagedType.Interface), In] NameValueMap Context,
    [MarshalAs(UnmanagedType.BStr)] out string FullFileName,
    out HandlingCodeEnum HandlingCode);

  [DispId(50335666)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void OnFileDirty(
    [MarshalAs(UnmanagedType.BStr), In] string RelativeFileName,
    [MarshalAs(UnmanagedType.BStr), In] string LibraryName,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UI1), In, Out] ref byte[] CustomLogicalName,
    [MarshalAs(UnmanagedType.BStr), In] string FullFileName,
    [MarshalAs(UnmanagedType.Interface), In] Document DocumentObject,
    [In] EventTimingEnum BeforeOrAfter,
    [MarshalAs(UnmanagedType.Interface), In] NameValueMap Context,
    out HandlingCodeEnum HandlingCode);
}
