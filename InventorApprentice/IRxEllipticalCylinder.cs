// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxEllipticalCylinder
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("FA34A3FE-F063-11D3-B7A2-0060B0F159EF")]
[InterfaceType(1)]
[TypeLibType(16 /*0x10*/)]
[ComImport]
public interface IRxEllipticalCylinder
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetEllipticalCylinderData(
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 3), Out] double[] pBasePoint,
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 3), Out] double[] pAxisVector,
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 3), Out] double[] pMajorAxis,
    out double pMinorMajorRatio);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void PutEllipticalCylinderData(
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 3), In] double[] pBasePoint,
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 3), In] double[] pAxisVector,
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 3), In] double[] pMajorAxis,
    [In] double MinorMajorRatio);

  [DispId(67126019)]
  IRxPoint BasePoint { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.Interface), In] set; }

  [DispId(67126020)]
  IRxUnitVector AxisVector { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.Interface), In] set; }

  [DispId(67126021)]
  IRxVector MajorAxisVector { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.Interface), In] set; }

  [DispId(67126022)]
  double MinorMajorRatio { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(67126023)]
  IRxSurfaceEvaluator Evaluator { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }
}
