// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxBSplineSurface
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(16 /*0x10*/)]
[Guid("5DF8609C-6B16-11D3-B794-0060B0F159EF")]
[InterfaceType(1)]
[ComImport]
public interface IRxBSplineSurface
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetBSplineInfo(
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 2), Out] uint[] pnOrder,
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 2), Out] uint[] pnNumPoles,
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 2), Out] uint[] pnNumKnots,
    out sbyte pbIsRational,
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 2), Out] sbyte[] pbIsPeriodic,
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 2), Out] sbyte[] pbIsClosed,
    out sbyte pbIsPlanar,
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 3), Out] double[] pPlaneVector);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetBSplineData(
    [In] uint nPoles,
    [In] uint nKnotsU,
    [In] uint nKnotsV,
    [In] uint nWeights,
    out double pPoles,
    out double pKnotsU,
    out double pKnotsV,
    [In, Out] ref double pWeights);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void PutBSplineInfoAndData(
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 2), In] uint[] pnOrder,
    [In] uint nNumPolesUXV,
    [In] uint nKnotsU,
    [In] uint nKnotsV,
    [In] uint nWeights,
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 2), In] sbyte[] pbIsPeriodic,
    [In] ref double pPoles,
    [In] ref double pKnotsU,
    [In] ref double pKnotsV,
    [In, Out] ref double pWeights);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  IRxPoint get_PoleAtIndex([In] uint nIndexU, [In] uint nIndexV);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void set_PoleAtIndex([In] uint nIndexU, [In] uint nIndexV, [MarshalAs(UnmanagedType.Interface), In] IRxPoint ppPoint);

  [DispId(67124229)]
  IRxSurfaceEvaluator Evaluator { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }
}
