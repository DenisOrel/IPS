// Decompiled with JetBrains decompiler
// Type: Interop.IMViewer.IIMViewerFileConverter
// Assembly: Interop.IMViewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=8d2bc20ab69e4bb4
// MVID: 1C0DF326-5EF4-4829-91C2-C30ABF7D8DD6
// Assembly location: D:\IPS\Client\Interop.IMViewer.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Interop.IMViewer;

[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FNonExtensible | TypeLibTypeFlags.FDispatchable)]
[Guid("8A8A7899-825F-4C7A-9A0A-FA00626290A2")]
[ComImport]
public interface IIMViewerFileConverter
{
  [DispId(1)]
  string LastError { [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }

  [DispId(2)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void ConvertNativeFile(
    [MarshalAs(UnmanagedType.IUnknown), In] object pCadSystem,
    [MarshalAs(UnmanagedType.BStr), In] string bstrCadFilePath,
    [In] bool bRecursive,
    [In] bool bMakePackage,
    [MarshalAs(UnmanagedType.BStr)] out string psViewerFilePath);
}
