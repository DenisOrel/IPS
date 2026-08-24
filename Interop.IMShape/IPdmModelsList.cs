// Decompiled with JetBrains decompiler
// Type: Interop.IMShape.IPdmModelsList
// Assembly: Interop.IMShape, Version=1.0.0.0, Culture=neutral, PublicKeyToken=8d2bc20ab69e4bb4
// MVID: D89360AE-CA24-4DA7-8C37-DC22263AF86B
// Assembly location: D:\IPS\Client\Interop.IMShape.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Interop.IMShape;

[Guid("BDB2B624-0D58-4A21-85DB-6C3B428E8CBE")]
[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FNonExtensible | TypeLibTypeFlags.FDispatchable)]
[ComImport]
public interface IPdmModelsList
{
  [DispId(1)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void AddDoc(
    [MarshalAs(UnmanagedType.BStr), In] string bstrDocID,
    [MarshalAs(UnmanagedType.BStr), In] string bstrDesignation,
    [MarshalAs(UnmanagedType.BStr), In] string bstrName,
    [MarshalAs(UnmanagedType.BStr), In] string bstrPathToFile,
    [MarshalAs(UnmanagedType.BStr), In] string bstrCadGuid);

  [DispId(2)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void AddConfiguration([MarshalAs(UnmanagedType.BStr), In] string bstrObjectID, [MarshalAs(UnmanagedType.BStr), In] string bstrPathToFile, [MarshalAs(UnmanagedType.BStr), In] string bstrCadGuid);
}
