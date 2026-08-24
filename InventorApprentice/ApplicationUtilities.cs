// Decompiled with JetBrains decompiler
// Type: InventorApprentice.ApplicationUtilities
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("DB93184E-4A45-4C38-96B4-42051502413D")]
[InterfaceType(2)]
[TypeLibType(4112)]
[ComImport]
public interface ApplicationUtilities
{
  [DispId(50434561)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  MoldDefinition CreateMoldDefinition(
    [MarshalAs(UnmanagedType.Interface), In] SurfaceBody PartBody,
    [MarshalAs(UnmanagedType.Interface), In] SurfaceBody WorkpieceBody,
    [MarshalAs(UnmanagedType.Interface), In] ObjectCollection RunoffSurfaces,
    [MarshalAs(UnmanagedType.Interface), In] UnitVector PullDirection,
    [In] double Tolerance = 0.0010000000474974513,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object HolePatches,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object Inserts);

  [DispId(50434562)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void CreateHolePatch([MarshalAs(UnmanagedType.Interface), In] EdgeCollection HoleEdges, [In] bool PatchSpecifiedHoles);

  [DispId(50434563)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  SurfaceBody CreateHolePatch2([MarshalAs(UnmanagedType.Interface), In] ObjectCollection EdgeUseLoop);

  [DispId(50434564)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  SurfaceBody CreateHolePatch3([MarshalAs(UnmanagedType.Interface), In] Edge Edge);

  [TypeLibFunc(64 /*0x40*/)]
  [DispId(50434565)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  RunoffSurfaceDefinition CreateRunoffSurfaceDefinition([MarshalAs(UnmanagedType.Interface), In] Box RangeBox);

  [TypeLibFunc(64 /*0x40*/)]
  [DispId(50434566)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void ExportMoldCoolingAnalysisData(
    [MarshalAs(UnmanagedType.Interface), In] ObjectCollection MoldBlocks,
    [MarshalAs(UnmanagedType.Interface), In] ObjectCollection BlockAttributes,
    [MarshalAs(UnmanagedType.Interface), In] ObjectCollection CoolingCurves,
    [MarshalAs(UnmanagedType.Interface), In] ObjectCollection CoolingAttributes,
    [MarshalAs(UnmanagedType.Interface), In] ObjectCollection RunnerCurves,
    [MarshalAs(UnmanagedType.Interface), In] ObjectCollection RunnerAttributes,
    [MarshalAs(UnmanagedType.BStr), In] string FileName);
}
