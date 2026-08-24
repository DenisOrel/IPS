// Decompiled with JetBrains decompiler
// Type: InventorApprentice.Matrix
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[InterfaceType(2)]
[TypeLibType(4096 /*0x1000*/)]
[DefaultMember("GetMatrixData")]
[Guid("CB69F171-558E-11D3-B793-0060B0F159EF")]
[ComImport]
public interface Matrix
{
  [DispId(0)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetMatrixData([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In, Out] ref double[] Cells);

  [DispId(67110962)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void PutMatrixData([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In, Out] ref double[] Cells);

  [DispId(67110963)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  double get_Cell([In] int Row, [In] int Col);

  [DispId(67110963)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void set_Cell([In] int Row, [In] int Col, [In] double _param3);

  [DispId(67110965)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Invert();

  [DispId(67110966)]
  double Determinant { [DispId(67110966), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(67110967)]
  Vector Translation { [DispId(67110967), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(67110968)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetCoordinateSystem([MarshalAs(UnmanagedType.Interface)] out Point Origin, [MarshalAs(UnmanagedType.Interface)] out Vector XAxis, [MarshalAs(UnmanagedType.Interface)] out Vector YAxis, [MarshalAs(UnmanagedType.Interface)] out Vector ZAxis);

  [DispId(67110969)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetCoordinateSystem([MarshalAs(UnmanagedType.Interface), In] Point Origin, [MarshalAs(UnmanagedType.Interface), In] Vector XAxis, [MarshalAs(UnmanagedType.Interface), In] Vector YAxis, [MarshalAs(UnmanagedType.Interface), In] Vector ZAxis);

  [DispId(67110970)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetToAlignCoordinateSystems(
    [MarshalAs(UnmanagedType.Interface), In] Point FromOrigin,
    [MarshalAs(UnmanagedType.Interface), In] Vector FromXAxis,
    [MarshalAs(UnmanagedType.Interface), In] Vector FromYAxis,
    [MarshalAs(UnmanagedType.Interface), In] Vector FromZAxis,
    [MarshalAs(UnmanagedType.Interface), In] Point ToOrigin,
    [MarshalAs(UnmanagedType.Interface), In] Vector ToXAxis,
    [MarshalAs(UnmanagedType.Interface), In] Vector ToYAxis,
    [MarshalAs(UnmanagedType.Interface), In] Vector ToZAxis);

  [DispId(67110971)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetToIdentity();

  [DispId(67110972)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetToRotation([In] double Angle, [MarshalAs(UnmanagedType.Interface), In] Vector Axis, [MarshalAs(UnmanagedType.Interface), In] Point Center);

  [DispId(67110973)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetToRotateTo([MarshalAs(UnmanagedType.Interface), In] Vector From, [MarshalAs(UnmanagedType.Interface), In] Vector To, [MarshalAs(UnmanagedType.Interface), In] Vector Axis = null);

  [DispId(67110974)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetTranslation([MarshalAs(UnmanagedType.Interface), In] Vector Translation, [In] bool ResetRotation = false);

  [DispId(67110975)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  bool IsEqualTo([MarshalAs(UnmanagedType.Interface), In] Matrix Matrix, [In] double Tolerance = 0.0);

  [DispId(67110976 /*0x04000840*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void TransformBy([MarshalAs(UnmanagedType.Interface), In] Matrix Matrix);

  [DispId(67110977)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void PreMultiplyBy([MarshalAs(UnmanagedType.Interface), In] Matrix Matrix);

  [DispId(67110978)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void PostMultiplyBy([MarshalAs(UnmanagedType.Interface), In] Matrix Matrix);

  [DispId(67110979)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Matrix Copy();

  [TypeLibFunc(64 /*0x40*/)]
  [DispId(67110964)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void MultiplyBy([MarshalAs(UnmanagedType.Interface), In] Matrix Matrix);
}
