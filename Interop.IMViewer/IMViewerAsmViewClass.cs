// Decompiled with JetBrains decompiler
// Type: Interop.IMViewer.IMViewerAsmViewClass
// Assembly: Interop.IMViewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=8d2bc20ab69e4bb4
// MVID: 1C0DF326-5EF4-4829-91C2-C30ABF7D8DD6
// Assembly location: D:\IPS\Client\Interop.IMViewer.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Interop.IMViewer;

[TypeLibType(TypeLibTypeFlags.FCanCreate)]
[Guid("ECE1A9F4-AF69-47D0-86F2-61054F9486CE")]
[ClassInterface(ClassInterfaceType.None)]
[ComImport]
public class IMViewerAsmViewClass : IIMViewerMatchView, IMViewerAsmView, IIMViewerView
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public extern IMViewerAsmViewClass();

  [DispId(1)]
  public virtual extern int ViewType { [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(2)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void GetCamera([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)] out double[] CameraMatrix, out double Zoom);

  [DispId(3)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void SetCamera([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In] double[] CameraMatrix, [In] double Zoom);

  [DispId(20)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void ActiveDocuments(
    [MarshalAs(UnmanagedType.BStr)] out string ViewerFilePath1,
    [MarshalAs(UnmanagedType.BStr)] out string ConfigName1,
    [MarshalAs(UnmanagedType.BStr)] out string ViewerFilePath2,
    [MarshalAs(UnmanagedType.BStr)] out string ConfigName2);

  public virtual extern int IIMViewerView_ViewType { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void IIMViewerView_GetCamera([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)] out double[] CameraMatrix, out double Zoom);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void IIMViewerView_SetCamera([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In] double[] CameraMatrix, [In] double Zoom);
}
