// Decompiled with JetBrains decompiler
// Type: InventorApprentice.RunoffSurfaceDefinition
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[InterfaceType(2)]
[Guid("6D78B55D-0148-442F-9EF5-E00611FCD8FF")]
[TypeLibType(4112)]
[ComImport]
public interface RunoffSurfaceDefinition
{
  [DispId(50417665)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int AddExtrusionPiece([MarshalAs(UnmanagedType.Interface), In] ObjectCollection EdgeUses, [MarshalAs(UnmanagedType.Interface), In] UnitVector Direction);

  [DispId(50417666)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int AddRadiatePiece(
    [MarshalAs(UnmanagedType.IDispatch), In] object EdgeUse,
    [MarshalAs(UnmanagedType.Interface), In] ObjectCollection RulingPoints,
    [MarshalAs(UnmanagedType.Interface), In] ObjectCollection RulingVectors);

  [DispId(50417670)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int AddSurfaceExtensionPiece([MarshalAs(UnmanagedType.IDispatch), In] object EdgeUse);

  [DispId(50417667)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int AddCornerPiece([MarshalAs(UnmanagedType.Interface), In] Vertex Corner, [MarshalAs(UnmanagedType.Interface), In] UnitVector StartVector, [MarshalAs(UnmanagedType.Interface), In] UnitVector EndVector);

  [DispId(50417668)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  SurfaceBody CreateSingleRunoffPiece([In] int index);

  [DispId(50417669)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  SurfaceBody CreateRunoff();

  [DispId(50417671)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int AddTangentExtensionPiece([MarshalAs(UnmanagedType.Interface), In] ObjectCollection EdgeUses);
}
