// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxFileAccessEvents
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(384)]
[InterfaceType(1)]
[Guid("32E4A318-C5E8-11D2-B77F-0060B0F159EF")]
[ComImport]
public interface IRxFileAccessEvents
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void OnFileResolution(
    [MarshalAs(UnmanagedType.BStr), In] string RelativeFileName,
    [MarshalAs(UnmanagedType.BStr), In] string LibraryName,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UI1), In] ref byte[] CustomLogicalName,
    [MarshalAs(UnmanagedType.BStr)] out string FullFileName,
    out HandlingCodeEnum HandlingCode);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void OnFileDirty(
    [MarshalAs(UnmanagedType.BStr), In] string RelativeFileName,
    [MarshalAs(UnmanagedType.BStr), In] string LibraryName,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UI1), In] ref byte[] CustomLogicalName,
    [MarshalAs(UnmanagedType.BStr), In] string FullFileName,
    [MarshalAs(UnmanagedType.Interface), In] Document DocumentObject,
    out HandlingCodeEnum HandlingCode);
}
