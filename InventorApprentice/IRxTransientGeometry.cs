// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxTransientGeometry
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[InterfaceType(1)]
[Guid("C1B42715-92E9-4278-BD5F-6DCE4B25FEBE")]
[TypeLibType(16 /*0x10*/)]
[ComImport]
public interface IRxTransientGeometry
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  IRxMatrix CreateMatrix();

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  IRxMatrix2d CreateMatrix2d();

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  IRxBox CreateBox();

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  IRxBox2d CreateBox2d();

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  IRxPoint CreatePoint([In] double XCoord = 0.0, [In] double YCoord = 0.0, [In] double ZCoord = 0.0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  IRxPoint2d CreatePoint2d([In] double XCoord = 0.0, [In] double YCoord = 0.0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  IRxVector CreateVector([In] double XCoord = 0.0, [In] double YCoord = 0.0, [In] double ZCoord = 0.0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  IRxVector2d CreateVector2d([In] double XCoord = 0.0, [In] double YCoord = 0.0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  IRxUnitVector CreateUnitVector([In] double XCoord = 0.0, [In] double YCoord = 0.0, [In] double ZCoord = 1.0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  IRxUnitVector2d CreateUnitVector2d([In] double XCoord = 0.0, [In] double YCoord = 1.0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  IRxLine CreateLine();

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  IRxLine2d CreateLine2d();

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  IRxCircle CreateCircle();

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  IRxCircle2d CreateCircle2d();

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  IRxBSplineCurve CreateBSplineCurve(
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
  IRxBSplineCurve2d CreateBSplineCurve2d(
    [In] uint nOrder,
    [In] uint nPoles,
    [In] uint nKnots,
    [In] uint nWeights,
    [In] sbyte bIsPeriodic,
    out double pPoles,
    out double pKnots,
    [In, Out] ref double pWeights);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  IRxPlane CreatePlane();

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  IRxCylinder CreateCylinder();

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  IRxEllipticalCylinder CreateEllipticalCylinder();

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  IRxCone CreateCone();

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  IRxEllipticalCone CreateEllipticalCone();

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  IRxSphere CreateSphere();

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  IRxTorus CreateTorus();

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  IRxBSplineSurface CreateBSplineSurface(
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
}
