// Decompiled with JetBrains decompiler
// Type: InventorApprentice.Matrix2d
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("DA33F19F-7C3F-11D3-B794-0060B0F159EF")]
[DefaultMember("GetMatrixData")]
[TypeLibType(4096 /*0x1000*/)]
[InterfaceType(2)]
[ComImport]
public interface Matrix2d
{
  [DispId(0)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetMatrixData([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In, Out] ref double[] Cells);

  [DispId(67111090)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void PutMatrixData([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In, Out] ref double[] Cells);

  [DispId(67111091)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  double get_Cell([In] int Row, [In] int Col);

  [DispId(67111091)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void set_Cell([In] int Row, [In] int Col, [In] double _param3);

  [DispId(67111093)]
  double Determinant { [DispId(67111093), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(67111094)]
  Vector2d Translation { [DispId(67111094), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(67111092)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Invert();

  [DispId(67111095)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetCoordinateSystem([MarshalAs(UnmanagedType.Interface)] out Point2d Origin, [MarshalAs(UnmanagedType.Interface)] out Vector2d XAxis, [MarshalAs(UnmanagedType.Interface)] out Vector2d YAxis);

  [DispId(67111096)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetCoordinateSystem([MarshalAs(UnmanagedType.Interface), In] Point2d Origin, [MarshalAs(UnmanagedType.Interface), In] Vector2d XAxis, [MarshalAs(UnmanagedType.Interface), In] Vector2d YAxis);

  [DispId(67111097)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetToAlignCoordinateSystems(
    [MarshalAs(UnmanagedType.Interface), In] Point2d FromOrigin,
    [MarshalAs(UnmanagedType.Interface), In] Vector2d FromXAxis,
    [MarshalAs(UnmanagedType.Interface), In] Vector2d FromYAxis,
    [MarshalAs(UnmanagedType.Interface), In] Point2d ToOrigin,
    [MarshalAs(UnmanagedType.Interface), In] Vector2d ToXAxis,
    [MarshalAs(UnmanagedType.Interface), In] Vector2d ToYAxis);

  [DispId(67111098)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetToIdentity();

  [DispId(67111099)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetToRotation([In] double Angle, [MarshalAs(UnmanagedType.Interface), In] Point2d Center);

  [DispId(67111100)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetToRotateTo([MarshalAs(UnmanagedType.Interface), In] Vector2d From, [MarshalAs(UnmanagedType.Interface), In] Vector2d To);

  [DispId(67111101)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetTranslation([MarshalAs(UnmanagedType.Interface), In] Vector2d Translation, [In] bool ResetRotation = false);

  [DispId(67111102)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  bool IsEqualTo([MarshalAs(UnmanagedType.Interface), In] Matrix2d Matrix2d, [In] double Tolerance = 0.0);

  [DispId(67111103)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void TransformBy([MarshalAs(UnmanagedType.Interface), In] Matrix2d Matrix2d);

  [DispId(67111104 /*0x040008C0*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void PreMultiplyBy([MarshalAs(UnmanagedType.Interface), In] Matrix2d Matrix2d);

  [DispId(67111105)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void PostMultiplyBy([MarshalAs(UnmanagedType.Interface), In] Matrix2d Matrix2d);

  [DispId(67111106)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Matrix2d Copy();
}
