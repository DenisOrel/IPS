// Decompiled with JetBrains decompiler
// Type: Interop.IMViewer.IIMViewerDocView
// Assembly: Interop.IMViewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=8d2bc20ab69e4bb4
// MVID: 1C0DF326-5EF4-4829-91C2-C30ABF7D8DD6
// Assembly location: D:\IPS\Client\Interop.IMViewer.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Interop.IMViewer;

[Guid("61743A24-F752-44B5-84D1-563ED586CE1B")]
[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FNonExtensible | TypeLibTypeFlags.FDispatchable)]
[ComImport]
public interface IIMViewerDocView
{
  [DispId(1)]
  int ViewType { [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(2)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetCamera([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)] out double[] CameraMatrix, out double Zoom);

  [DispId(3)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetCamera([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In] double[] CameraMatrix, [In] double Zoom);

  [DispId(20)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void ActiveDocument([MarshalAs(UnmanagedType.BStr)] out string ViewerFilePath, [MarshalAs(UnmanagedType.BStr)] out string ConfigName);
}
