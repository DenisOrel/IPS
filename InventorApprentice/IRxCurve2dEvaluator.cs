// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxCurve2dEvaluator
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(16 /*0x10*/)]
[Guid("5DF8603D-6B16-11D3-B794-0060B0F159EF")]
[InterfaceType(1)]
[ComConversionLoss]
[ComImport]
public interface IRxCurve2dEvaluator
{
  [DispId(67116930)]
  IRxBox2d RangeBox { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetEndPoints([MarshalAs(UnmanagedType.LPArray, SizeConst = 2), Out] double[] pStartPoint, [MarshalAs(UnmanagedType.LPArray, SizeConst = 2), Out] double[] pEndPoint);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetParamExtents(out double pMinParam, out double pMaxParam);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetParamAtPoint(
    [In] uint nPoints,
    [In] ref double pPoints,
    [In, Out] ref double pGuessParams,
    [In, Out] ref double pMaxDeviations,
    [In, Out] ref double pParams,
    [In, Out] ref SolutionNatureEnum pSolutionNatures);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetPointAtParam([In] uint nParams, [In] ref double pParams, out double pPoints);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetTangent([In] uint nParams, [In] ref double pParams, out double pTangents);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetCurvature(
    [In] uint nParams,
    [In] ref double pParams,
    [In, Out] ref double pDirections,
    [In, Out] ref double pCurvatures);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetDerivatives(
    [In] uint nParams,
    [In] ref double pParams,
    [In, Out] ref double pFirstDerivs,
    [In, Out] ref double pSecondDerivs,
    [In, Out] ref double pThirdDerivs);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetParamAtLength([In] double FromParam, [In] double Length, out double pParam);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetLengthAtParam([In] double FromParam, [In] double ToParam, out double pLength);

  [DispId(67116940)]
  uint Continuity { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetParamAnomaly([MarshalAs(UnmanagedType.LPArray, SizeConst = 2), Out] double[] pPeriodicity, out sbyte pIsSingular, out sbyte pUnboundedParam);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetStrokes(
    [In] double FromParam,
    [In] double ToParam,
    [In] double Tolerance,
    out uint nVertexCount,
    [Out] IntPtr ppVertexCoordinates);
}
