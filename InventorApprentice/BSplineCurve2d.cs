// Decompiled with JetBrains decompiler
// Type: InventorApprentice.BSplineCurve2d
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[InterfaceType(2)]
[Guid("49CB4BBA-872A-11D3-8524-0060B0F0B5B7")]
[TypeLibType(4096 /*0x1000*/)]
[ComImport]
public interface BSplineCurve2d
{
  [DispId(67124657)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetBSplineInfo(
    out int Order,
    out int NumPoles,
    out int NumKnots,
    out bool IsRational,
    out bool IsPeriodic,
    out bool IsClosed);

  [DispId(67124658)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetBSplineData([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In, Out] ref double[] Poles, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In, Out] ref double[] Knots, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In, Out] ref double[] Weights);

  [DispId(67124659)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void PutBSplineInfoAndData(
    [In] int Order,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In, Out] ref double[] Poles,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In, Out] ref double[] Knots,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In, Out] ref double[] Weights,
    [In] bool IsPeriodic);

  [DispId(67124660)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Point2d get_PoleAtIndex([In] int index);

  [DispId(67124660)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void set_PoleAtIndex([In] int index, [MarshalAs(UnmanagedType.Interface), In] Point2d _param2);

  [DispId(67124661)]
  Curve2dEvaluator Evaluator { [DispId(67124661), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(67124662)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  BSplineCurve2d Copy();

  [DispId(67124663)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  BSplineCurve2d ExtractPartial([In] double StartParam, [In] double EndParam);

  [DispId(67124664)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Split([In] double SplitParam, [MarshalAs(UnmanagedType.Interface)] out BSplineCurve2d CurveOne, [MarshalAs(UnmanagedType.Interface)] out BSplineCurve2d CurveTwo);
}
