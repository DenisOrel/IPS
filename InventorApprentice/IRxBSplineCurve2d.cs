// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxBSplineCurve2d
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("5DF86033-6B16-11D3-B794-0060B0F159EF")]
[InterfaceType(1)]
[ComImport]
public interface IRxBSplineCurve2d
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetBSplineInfo(
    out uint pnOrder,
    out uint pnNumPoles,
    out uint pnNumKnots,
    out sbyte pIsRational,
    out sbyte pIsPeriodic,
    out sbyte pIsClosed);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetBSplineData(
    [In] uint nPoles,
    [In] uint nKnots,
    [In] uint nWeights,
    out double pPoles,
    out double pKnots,
    [In, Out] ref double pWeights);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void PutBSplineInfoAndData(
    [In] uint nOrder,
    [In] uint nPoles,
    [In] uint nKnots,
    [In] uint nWeights,
    [In] sbyte bIsPeriodic,
    [In] ref double pPoles,
    [In] ref double pKnots,
    [In, Out] ref double pWeights);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  IRxPoint2d get_PoleAtIndex([In] uint nIndex);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void set_PoleAtIndex([In] uint nIndex, [MarshalAs(UnmanagedType.Interface), In] IRxPoint2d ppPoint);

  [DispId(67124613)]
  IRxCurve2dEvaluator Evaluator { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }
}
