// Decompiled with JetBrains decompiler
// Type: Interop.IMViewer.IIMViewerApp
// Assembly: Interop.IMViewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=8d2bc20ab69e4bb4
// MVID: 1C0DF326-5EF4-4829-91C2-C30ABF7D8DD6
// Assembly location: D:\IPS\Client\Interop.IMViewer.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Interop.IMViewer;

[Guid("63C98CB5-937B-4739-B0B6-1FB055B1C864")]
[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FNonExtensible | TypeLibTypeFlags.FDispatchable)]
[ComImport]
public interface IIMViewerApp
{
  [DispId(1)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void OpenDocument([MarshalAs(UnmanagedType.BStr), In] string ViewerFilePath, [MarshalAs(UnmanagedType.BStr), In] string ConfigName);

  [DispId(2)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void OpenModelMatch(
    [MarshalAs(UnmanagedType.BStr), In] string ViewerFilePath1,
    [MarshalAs(UnmanagedType.BStr), In] string ConfigName1,
    [MarshalAs(UnmanagedType.BStr), In] string ViewerFilePath2,
    [MarshalAs(UnmanagedType.BStr), In] string ConfigName2);

  [DispId(3)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void OpenAsmAnimation([MarshalAs(UnmanagedType.BStr), In] string ViewerFilePath, [MarshalAs(UnmanagedType.BStr), In] string ConfigName);

  [DispId(4)]
  IIMViewerView ActiveView { [DispId(4), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(5)]
  tagIMViewerViewType ActiveViewType { [DispId(5), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(6)]
  bool EnableMultiTabs { [DispId(6), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(6), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(7)]
  bool EnableOpenFile { [DispId(7), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(7), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }
}
