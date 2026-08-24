// Decompiled with JetBrains decompiler
// Type: Interop.IMViewer.IIMViewerFileConverter2
// Assembly: Interop.IMViewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=8d2bc20ab69e4bb4
// MVID: 1C0DF326-5EF4-4829-91C2-C30ABF7D8DD6
// Assembly location: D:\IPS\Client\Interop.IMViewer.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Interop.IMViewer;

[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FNonExtensible | TypeLibTypeFlags.FDispatchable)]
[Guid("04FF60E1-0C13-4B99-A969-C5A40F4B4198")]
[ComImport]
public interface IIMViewerFileConverter2 : IIMViewerFileConverter
{
  [DispId(1)]
  new string LastError { [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }

  [DispId(2)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void ConvertNativeFile(
    [MarshalAs(UnmanagedType.IUnknown), In] object pCadSystem,
    [MarshalAs(UnmanagedType.BStr), In] string bstrCadFilePath,
    [In] bool bRecursive,
    [In] bool bMakePackage,
    [MarshalAs(UnmanagedType.BStr)] out string psViewerFilePath);

  [DispId(3)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void ConvertNativeFile2(
    [MarshalAs(UnmanagedType.IUnknown), In] object pCadSystem,
    [MarshalAs(UnmanagedType.BStr), In] string bstrCadFilePath,
    [MarshalAs(UnmanagedType.BStr), In] string bstrOutputFolder,
    [MarshalAs(UnmanagedType.BStr)] out string psViewerFilePath);
}
