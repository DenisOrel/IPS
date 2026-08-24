// Decompiled with JetBrains decompiler
// Type: InventorApprentice.Point
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("CB69F172-558E-11D3-B793-0060B0F159EF")]
[DefaultMember("GetPointData")]
[InterfaceType(2)]
[TypeLibType(4096 /*0x1000*/)]
[ComImport]
public interface Point
{
  [DispId(0)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetPointData([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In, Out] ref double[] Coords);

  [DispId(67111218)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void PutPointData([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In, Out] ref double[] Coords);

  [DispId(67111219)]
  double X { [DispId(67111219), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(67111219), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(67111220)]
  double Y { [DispId(67111220), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(67111220), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(67111221)]
  double Z { [DispId(67111221), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(67111221), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(67111222)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void TransformBy([MarshalAs(UnmanagedType.Interface), In] Matrix Matrix);

  [DispId(67111223)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void TranslateBy([MarshalAs(UnmanagedType.Interface), In] Vector Vector);

  [DispId(67111224)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  double DistanceTo([MarshalAs(UnmanagedType.Interface), In] Point Point);

  [DispId(67111225)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Vector VectorTo([MarshalAs(UnmanagedType.Interface), In] Point Point);

  [DispId(67111226)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  bool IsEqualTo([MarshalAs(UnmanagedType.Interface), In] Point Point, [In] double Tolerance = 0.0);

  [DispId(67111227)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Point Copy();
}
