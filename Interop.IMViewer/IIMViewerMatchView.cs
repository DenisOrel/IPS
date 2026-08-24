// Decompiled with JetBrains decompiler
// Type: Interop.IMViewer.IIMViewerMatchView
// Assembly: Interop.IMViewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=8d2bc20ab69e4bb4
// MVID: 1C0DF326-5EF4-4829-91C2-C30ABF7D8DD6
// Assembly location: D:\IPS\Client\Interop.IMViewer.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Interop.IMViewer;

[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FNonExtensible | TypeLibTypeFlags.FDispatchable)]
[Guid("A35A3AD7-2C13-463C-AF3E-8A5F205E2893")]
[ComImport]
public interface IIMViewerMatchView
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
  void ActiveDocuments(
    [MarshalAs(UnmanagedType.BStr)] out string ViewerFilePath1,
    [MarshalAs(UnmanagedType.BStr)] out string ConfigName1,
    [MarshalAs(UnmanagedType.BStr)] out string ViewerFilePath2,
    [MarshalAs(UnmanagedType.BStr)] out string ConfigName2);
}
