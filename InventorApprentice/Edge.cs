// Decompiled with JetBrains decompiler
// Type: InventorApprentice.Edge
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("5DF8608E-6B16-11D3-B794-0060B0F159EF")]
[TypeLibType(4096 /*0x1000*/)]
[InterfaceType(2)]
[DefaultMember("Type")]
[ComImport]
public interface Edge
{
  [DispId(0)]
  ObjectTypeEnum Type { [DispId(0), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(2130706433 /*0x7F000001*/)]
  object Application { [DispId(2130706433 /*0x7F000001*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IDispatch)] get; }

  [DispId(2130706434 /*0x7F000002*/)]
  SurfaceBody Parent { [DispId(2130706434 /*0x7F000002*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(67120191)]
  Wire Wire { [DispId(67120191), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(67120177)]
  Vertex StartVertex { [DispId(67120177), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(67120178)]
  Vertex StopVertex { [DispId(67120178), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(67120179)]
  EdgeUses EdgeUses { [DispId(67120179), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(67120180)]
  Faces Faces { [DispId(67120180), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(67120181)]
  bool IsParamReversed { [DispId(67120181), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(67120192)]
  CurveTypeEnum GeometryType { [DispId(67120192), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(67120187)]
  object Geometry { [DispId(67120187), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IDispatch)] get; }

  [DispId(67120184)]
  int GeometryForm { [DispId(67120184), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(67120185)]
  CurveEvaluator Evaluator { [DispId(67120185), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(2130706452)]
  AttributeSets AttributeSets { [DispId(2130706452), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(67120186)]
  EdgeCollection TangentiallyConnectedEdges { [DispId(67120186), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(2130706454)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetReferenceKey([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UI1), In, Out] ref byte[] ReferenceKey, [In] int KeyContext = 0);

  [DispId(2130706474)]
  int TransientKey { [DispId(2130706474), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(2130706467)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void CalculateStrokes(
    [In] double Tolerance,
    out int VertexCount,
    out int SegmentCount,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)] out double[] VertexCoordinates,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_I4)] out int[] VertexIndices);

  [DispId(2130706469)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetExistingStrokes(
    [In] double ToleranceIndex,
    out int VertexCount,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)] out double[] VertexCoordinates,
    out int PolylineCount,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_I4)] out int[] PolylineLengths);

  [DispId(2130706468)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetExistingStrokeTolerances(out int ToleranceCount, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)] out double[] ExistingTolerances);

  [DispId(67120188)]
  Point PointOnEdge { [DispId(67120188), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(67120194)]
  bool IsTolerant { [DispId(67120194), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(67120195)]
  double Tolerance { [DispId(67120195), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(67120189)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Edge GetSourceEdge([In] bool GetLeafSource = true);

  [DispId(67120190)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Point GetClosestPointTo([MarshalAs(UnmanagedType.Interface), In] Point InputPoint);

  [DispId(67120193)]
  int AssociativeID { [DispId(67120193), TypeLibFunc(64 /*0x40*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [TypeLibFunc(64 /*0x40*/), DispId(67120193), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [TypeLibFunc(64 /*0x40*/)]
  [DispId(2130706481)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void CalculateStrokesWithOptions(
    [In] double ChordalTolerance,
    [MarshalAs(UnmanagedType.Interface), In] NameValueMap Options,
    out int VertexCount,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)] out double[] VertexCoordinates,
    out int PolylineCount,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_I4)] out int[] PolylineLengths);

  [DispId(67120182)]
  CurveTypeEnum CurveType { [TypeLibFunc(64 /*0x40*/), DispId(67120182), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [TypeLibFunc(64 /*0x40*/)]
  [DispId(67120183)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.IDispatch)]
  object get_Curve([In] CurveTypeEnum Type);
}
