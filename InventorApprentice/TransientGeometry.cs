// Decompiled with JetBrains decompiler
// Type: InventorApprentice.TransientGeometry
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(4096 /*0x1000*/)]
[InterfaceType(2)]
[Guid("97ECB3AE-6C6E-4D8A-A91E-564314494EB8")]
[ComImport]
public interface TransientGeometry
{
  [DispId(2130706435 /*0x7F000003*/)]
  ObjectTypeEnum Type { [DispId(2130706435 /*0x7F000003*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(67126944)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Arc2d CreateArc2d([MarshalAs(UnmanagedType.Interface), In] Point2d Center, [In] double Radius, [In] double StartAngle, [In] double SweepAngle);

  [DispId(67126952)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Arc2d CreateArc2dByThreePoints([MarshalAs(UnmanagedType.Interface), In] Point2d PointOne, [MarshalAs(UnmanagedType.Interface), In] Point2d PointTwo, [MarshalAs(UnmanagedType.Interface), In] Point2d PointThree);

  [DispId(67126945)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Arc3d CreateArc3d(
    [MarshalAs(UnmanagedType.Interface), In] Point Center,
    [MarshalAs(UnmanagedType.Interface), In] UnitVector Normal,
    [MarshalAs(UnmanagedType.Interface), In] UnitVector ReferenceVector,
    [In] double Radius,
    [In] double StartAngle,
    [In] double SweepAngle);

  [DispId(67126953)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Arc3d CreateArc3dByThreePoints([MarshalAs(UnmanagedType.Interface), In] Point PointOne, [MarshalAs(UnmanagedType.Interface), In] Point PointTwo, [MarshalAs(UnmanagedType.Interface), In] Point PointThree);

  [DispId(67126946)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  EllipseFull CreateEllipseFull(
    [MarshalAs(UnmanagedType.Interface), In] Point Center,
    [MarshalAs(UnmanagedType.Interface), In] UnitVector Normal,
    [MarshalAs(UnmanagedType.Interface), In] Vector MajorAxisVector,
    [In] double MinorMajorRatio);

  [DispId(67126950)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  EllipticalArc CreateEllipticalArc(
    [MarshalAs(UnmanagedType.Interface), In] Point Center,
    [MarshalAs(UnmanagedType.Interface), In] UnitVector MajorAxis,
    [MarshalAs(UnmanagedType.Interface), In] UnitVector MinorAxis,
    [In] double MajorRadius,
    [In] double MinorRadius,
    [In] double StartAngle,
    [In] double SweepAngle);

  [DispId(67126951)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  EllipticalArc2d CreateEllipticalArc2d(
    [MarshalAs(UnmanagedType.Interface), In] Point2d Center,
    [MarshalAs(UnmanagedType.Interface), In] UnitVector2d MajorAxis,
    [In] double MajorRadius,
    [In] double MinorRadius,
    [In] double StartAngle,
    [In] double SweepAngle);

  [DispId(67126949)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  EllipseFull2d CreateEllipseFull2d(
    [MarshalAs(UnmanagedType.Interface), In] Point2d Center,
    [MarshalAs(UnmanagedType.Interface), In] Vector2d MajorAxisVector,
    [In] double MinorMajorRatio);

  [DispId(67126947)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  LineSegment CreateLineSegment([MarshalAs(UnmanagedType.Interface), In] Point StartPoint, [MarshalAs(UnmanagedType.Interface), In] Point EndPoint);

  [DispId(67126948)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  LineSegment2d CreateLineSegment2d([MarshalAs(UnmanagedType.Interface), In] Point2d StartPoint, [MarshalAs(UnmanagedType.Interface), In] Point2d EndPoint);

  [DispId(67126913)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Matrix CreateMatrix();

  [DispId(67126914)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Matrix2d CreateMatrix2d();

  [DispId(67126915)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Box CreateBox();

  [DispId(67126916)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Box2d CreateBox2d();

  [DispId(67126917)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Point CreatePoint([In] double XCoord = 0.0, [In] double YCoord = 0.0, [In] double ZCoord = 0.0);

  [DispId(67126918)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Point2d CreatePoint2d([In] double XCoord = 0.0, [In] double YCoord = 0.0);

  [DispId(67126919)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Vector CreateVector([In] double XCoord = 0.0, [In] double YCoord = 0.0, [In] double ZCoord = 0.0);

  [DispId(67126920)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Vector2d CreateVector2d([In] double XCoord = 0.0, [In] double YCoord = 0.0);

  [DispId(67126921)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  UnitVector CreateUnitVector([In] double XCoord = 0.0, [In] double YCoord = 0.0, [In] double ZCoord = 1.0);

  [DispId(67126922)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  UnitVector2d CreateUnitVector2d([In] double XCoord = 0.0, [In] double YCoord = 1.0);

  [DispId(67126923)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Line CreateLine([MarshalAs(UnmanagedType.Interface), In] Point RootPoint, [MarshalAs(UnmanagedType.Interface), In] Vector Direction);

  [DispId(67126924)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Line2d CreateLine2d([MarshalAs(UnmanagedType.Interface), In] Point2d RootPoint, [MarshalAs(UnmanagedType.Interface), In] UnitVector2d Direction);

  [DispId(67126925)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Circle CreateCircle([MarshalAs(UnmanagedType.Interface), In] Point Center, [MarshalAs(UnmanagedType.Interface), In] UnitVector Normal, [In] double Radius);

  [DispId(67126955)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Circle CreateCircleByThreePoints([MarshalAs(UnmanagedType.Interface), In] Point PointOne, [MarshalAs(UnmanagedType.Interface), In] Point PointTwo, [MarshalAs(UnmanagedType.Interface), In] Point PointThree);

  [DispId(67126926)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Circle2d CreateCircle2d([MarshalAs(UnmanagedType.Interface), In] Point2d Center, [In] double Radius);

  [DispId(67126954)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Circle2d CreateCircle2dByThreePoints([MarshalAs(UnmanagedType.Interface), In] Point2d PointOne, [MarshalAs(UnmanagedType.Interface), In] Point2d PointTwo, [MarshalAs(UnmanagedType.Interface), In] Point2d PointThree);

  [DispId(67126927)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  BSplineCurve CreateBSplineCurve(
    [In] int Order,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In, Out] ref double[] Poles,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In, Out] ref double[] Knots,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In, Out] ref double[] Weights,
    [In] bool IsPeriodic);

  [DispId(67126928)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  BSplineCurve2d CreateBSplineCurve2d(
    [In] int Order,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In, Out] ref double[] Poles,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In, Out] ref double[] Knots,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In, Out] ref double[] Weights,
    [In] bool IsPeriodic);

  [DispId(67126929)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Plane CreatePlane([MarshalAs(UnmanagedType.Interface), In] Point RootPoint, [MarshalAs(UnmanagedType.Interface), In] Vector Normal);

  [DispId(67126956)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Plane CreatePlaneByThreePoints([MarshalAs(UnmanagedType.Interface), In] Point PointOne, [MarshalAs(UnmanagedType.Interface), In] Point PointTwo, [MarshalAs(UnmanagedType.Interface), In] Point PointThree);

  [DispId(67126930)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Cylinder CreateCylinder([MarshalAs(UnmanagedType.Interface), In] Point RootPoint, [MarshalAs(UnmanagedType.Interface), In] UnitVector Axis, [In] double Radius);

  [DispId(67126931)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  EllipticalCylinder CreateEllipticalCylinder(
    [MarshalAs(UnmanagedType.Interface), In] Point BasePoint,
    [MarshalAs(UnmanagedType.Interface), In] UnitVector AxisVector,
    [MarshalAs(UnmanagedType.Interface), In] Vector MajorAxisVector,
    [In] double MinorMajorRatio);

  [DispId(67126932)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Cone CreateCone(
    [MarshalAs(UnmanagedType.Interface), In] Point RootPoint,
    [MarshalAs(UnmanagedType.Interface), In] UnitVector Axis,
    [In] double Radius,
    [In] double HalfAngle,
    [In] bool IsExpanding);

  [DispId(67126933)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  EllipticalCone CreateEllipticalCone(
    [MarshalAs(UnmanagedType.Interface), In] Point BasePoint,
    [MarshalAs(UnmanagedType.Interface), In] UnitVector AxisVector,
    [MarshalAs(UnmanagedType.Interface), In] Vector MajorAxisVector,
    [In] double MinorMajorRatio,
    [In] double HalfAngle,
    [In] bool IsExpanding);

  [DispId(67126934)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Sphere CreateSphere([MarshalAs(UnmanagedType.Interface), In] Point CenterPoint, [In] double Radius);

  [DispId(67126935)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Torus CreateTorus(
    [MarshalAs(UnmanagedType.Interface), In] Point CenterPoint,
    [MarshalAs(UnmanagedType.Interface), In] UnitVector AxisVector,
    [In] double MajorRadius,
    [In] double MinorRadius);

  [DispId(67126936)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  BSplineSurface CreateBSplineSurface(
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_I4), In] ref int[] Order,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In] ref double[] Poles,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In] ref double[] KnotsU,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In] ref double[] KnotsV,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In] ref double[] Weights,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BOOL), In] ref bool[] IsPeriodic);

  [DispId(67126941)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  BSplineCurve2dDefinition CreateBSplineCurve2dDefinition();

  [DispId(67126943)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  BSplineCurve2d CreateFittedBSplineCurve2d([MarshalAs(UnmanagedType.Interface), In] BSplineCurve2dDefinition Definition);

  [DispId(67126940)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  BSplineCurveDefinition CreateBSplineCurveDefinition();

  [DispId(67126942)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  BSplineCurve CreateFittedBSplineCurve([MarshalAs(UnmanagedType.Interface), In] BSplineCurveDefinition Definition);

  [DispId(67126957)]
  double PointTolerance { [DispId(67126957), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(67126958)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Polyline3d CreatePolyline3d([MarshalAs(UnmanagedType.Struct), In] object Points);

  [DispId(67126960)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Polyline3d CreatePolyline3dFromCurve([MarshalAs(UnmanagedType.IDispatch), In] object Curve, [In] double Tolerance);

  [DispId(67126959)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Polyline2d CreatePolyline2d([MarshalAs(UnmanagedType.Struct), In] object Points);

  [DispId(67126961)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Polyline2d CreatePolyline2dFromCurve([MarshalAs(UnmanagedType.IDispatch), In] object Curve, [In] double Tolerance);

  [DispId(67126962)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  ObjectsEnumerator CurveCurveIntersection([MarshalAs(UnmanagedType.IDispatch), In] object CurveOne, [MarshalAs(UnmanagedType.IDispatch), In] object CurveTwo, [In] double Tolerance = 0.0);

  [DispId(67126963)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  ObjectsEnumerator CurveSurfaceIntersection([MarshalAs(UnmanagedType.IDispatch), In] object Curve, [MarshalAs(UnmanagedType.IDispatch), In] object Surface, [In] double Tolerance = 0.0);

  [DispId(67126964)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  ObjectsEnumerator SurfaceSurfaceIntersection(
    [MarshalAs(UnmanagedType.IDispatch), In] object SurfaceOne,
    [MarshalAs(UnmanagedType.IDispatch), In] object SurfaceTwo,
    [In] double Tolerance = 0.0);

  [DispId(67126966)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Point GetFarmostPoint([MarshalAs(UnmanagedType.IDispatch), In] object Entity, [MarshalAs(UnmanagedType.Interface), In] UnitVector Direction);
}
