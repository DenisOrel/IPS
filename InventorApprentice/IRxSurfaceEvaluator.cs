// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxSurfaceEvaluator
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(16 /*0x10*/)]
[InterfaceType(1)]
[Guid("5DF8606E-6B16-11D3-B794-0060B0F159EF")]
[ComImport]
public interface IRxSurfaceEvaluator
{
  [DispId(67120642)]
  IRxBox RangeBox { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(67120643)]
  IRxBox2d ParamRangeRect { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetParamAtPoint(
    [In] uint nPoints,
    [In] ref double pPoints,
    [In, Out] ref double pGuessParams,
    [In, Out] ref double pMaxDeviations,
    [In, Out] ref double pParams,
    [In, Out] ref SolutionNatureEnum pSolTypes);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetPointAtParam([In] uint nParams, [In] ref double pParams, out double pPoints);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetNormal([In] uint nParams, [In] ref double pParams, out double pNormals);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetTangents([In] uint nParams, [In] ref double pParams, out double pUTangents, out double pVTangents);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetCurvatures(
    [In] uint nParams,
    [In] ref double pParams,
    [In, Out] ref double pMaxTangents,
    [In, Out] ref double pMaxCurvatures,
    [In, Out] ref double pMinCurvatures);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetDerivatives(
    [In] uint nParams,
    [In] ref double pParams,
    [In, Out] ref double pUPartials,
    [In, Out] ref double pVPartials,
    [In, Out] ref double pUUPartials,
    [In, Out] ref double pUVPartials,
    [In, Out] ref double pVVPartials,
    [In, Out] ref double pUUUPartials,
    [In, Out] ref double pVVVPartials);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  sbyte get_IsParamOnFace([MarshalAs(UnmanagedType.LPArray, SizeConst = 2), In] double[] pParams);

  [DispId(67120651)]
  double Area { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(67120652)]
  uint Continuity { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetParamAnomaly(
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 2), Out] double[] pPeriodicityU,
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 2), Out] double[] pPeriodicityV,
    out uint pnEndSingularityU,
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 2), Out] double[] pSingularityU,
    out uint pnEndSingularityV,
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 2), Out] double[] pSingularityV,
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 2), Out] sbyte[] pUnboundedParam);
}
