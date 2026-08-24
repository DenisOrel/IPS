// Decompiled with JetBrains decompiler
// Type: Interop.IMShape.PdmModelsListClass
// Assembly: Interop.IMShape, Version=1.0.0.0, Culture=neutral, PublicKeyToken=8d2bc20ab69e4bb4
// MVID: D89360AE-CA24-4DA7-8C37-DC22263AF86B
// Assembly location: D:\IPS\Client\Interop.IMShape.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Interop.IMShape;

[ClassInterface(ClassInterfaceType.None)]
[Guid("F7DD1278-54DA-43B4-8A0B-BE7418A1FE12")]
[TypeLibType(TypeLibTypeFlags.FCanCreate)]
[ComImport]
public class PdmModelsListClass : IPdmModelsList, PdmModelsList
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public extern PdmModelsListClass();

  [DispId(1)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void AddDoc(
    [MarshalAs(UnmanagedType.BStr), In] string bstrDocID,
    [MarshalAs(UnmanagedType.BStr), In] string bstrDesignation,
    [MarshalAs(UnmanagedType.BStr), In] string bstrName,
    [MarshalAs(UnmanagedType.BStr), In] string bstrPathToFile,
    [MarshalAs(UnmanagedType.BStr), In] string bstrCadGuid);

  [DispId(2)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void AddConfiguration(
    [MarshalAs(UnmanagedType.BStr), In] string bstrObjectID,
    [MarshalAs(UnmanagedType.BStr), In] string bstrPathToFile,
    [MarshalAs(UnmanagedType.BStr), In] string bstrCadGuid);
}
