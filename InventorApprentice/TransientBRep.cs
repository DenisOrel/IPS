// Decompiled with JetBrains decompiler
// Type: InventorApprentice.TransientBRep
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("2BFE4397-C369-4CEF-90C9-D5C8AE90BC9F")]
[InterfaceType(2)]
[TypeLibType(4096 /*0x1000*/)]
[ComImport]
public interface TransientBRep
{
  [DispId(50417409)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  SurfaceBody CreateSilhouetteCurve(
    [MarshalAs(UnmanagedType.Interface), In] Face Face,
    [MarshalAs(UnmanagedType.Interface), In] UnitVector ViewDirection,
    [In] bool ReturnCoincidentSilhouettes);

  [DispId(50417412)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  SurfaceBody CreateSolidCylinderCone(
    [MarshalAs(UnmanagedType.Interface), In] Point BottomPoint,
    [MarshalAs(UnmanagedType.Interface), In] Point TopPoint,
    [In] double BottomMajorRadius,
    [In] double BottomMinorRadius,
    [In] double TopMajorRadius,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object MajorAxisPosition);

  [DispId(50417413)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  SurfaceBody CreateSolidSphere([MarshalAs(UnmanagedType.Interface), In] Point Center, [In] double Radius);

  [DispId(50417420)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  SurfaceBody CreateSolidTorus([MarshalAs(UnmanagedType.Interface), In] Point Center, [In] double MajorRadius, [In] double MinorRadius);

  [DispId(50417414)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  SurfaceBody CreateSolidBlock([MarshalAs(UnmanagedType.Interface), In] Box Box);

  [DispId(50417415)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void DoBoolean([MarshalAs(UnmanagedType.Interface), In] SurfaceBody BlankBody, [MarshalAs(UnmanagedType.Interface), In] SurfaceBody ToolBody, [In] BooleanTypeEnum BooleanType);

  [DispId(50417416)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Transform([MarshalAs(UnmanagedType.Interface), In] SurfaceBody SurfaceBody, [MarshalAs(UnmanagedType.Interface), In] Matrix Transform);

  [DispId(50417417)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  SurfaceBody Copy([MarshalAs(UnmanagedType.IDispatch), In] object Entity);

  [DispId(50417418)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  SurfaceBody CreateIntersectionWithPlane([MarshalAs(UnmanagedType.Interface), In] SurfaceBody Body, [MarshalAs(UnmanagedType.Interface), In] Plane Plane);

  [DispId(50417419)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void DeleteFaces([MarshalAs(UnmanagedType.IDispatch), In] object Faces, [In] bool DeleteSpecifiedFaces);

  [DispId(50417422)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  SurfaceBodyDefinition CreateSurfaceBodyDefinition();

  [DispId(50417425)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  SurfaceBodies ReadFromFile([MarshalAs(UnmanagedType.BStr), In] string FileName);

  [DispId(50417426)]
  ApplicationUtilities ApplicationUtilities { [DispId(50417426), TypeLibFunc(64 /*0x40*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(50417427)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  SurfaceBody CreateRuledSurface([MarshalAs(UnmanagedType.Interface), In] Wire SectionOne, [MarshalAs(UnmanagedType.Interface), In] Wire SectionTwo);

  [DispId(50417428)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void WriteToFile([MarshalAs(UnmanagedType.Interface), In] ObjectCollection Bodies, [MarshalAs(UnmanagedType.BStr), In] string FileName, [MarshalAs(UnmanagedType.BStr), In] string Format = "");

  [DispId(50417429)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void ImprintBodies(
    [MarshalAs(UnmanagedType.Interface), In] SurfaceBody InputBodyOne,
    [MarshalAs(UnmanagedType.Interface), In] SurfaceBody InputBodyTwo,
    [In] bool ImprintCoincidentEdges,
    [MarshalAs(UnmanagedType.Interface)] out SurfaceBody OutputBodyOne,
    [MarshalAs(UnmanagedType.Interface)] out SurfaceBody OutputBodyTwo,
    [MarshalAs(UnmanagedType.Interface)] out Faces BodyOneOverlappingFaces,
    [MarshalAs(UnmanagedType.Interface)] out Faces BodyTwoOverlappingFaces,
    [MarshalAs(UnmanagedType.Interface)] out Edges BodyOneOverlappingEdges,
    [MarshalAs(UnmanagedType.Interface)] out Edges BodyTwoOverlappingEdges,
    [In] double Tolerance = 0.0);
}
