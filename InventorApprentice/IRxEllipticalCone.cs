// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxEllipticalCone
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("97ED8AED-EF9D-11D3-B7A2-0060B0F159EF")]
[TypeLibType(16 /*0x10*/)]
[InterfaceType(1)]
[ComImport]
public interface IRxEllipticalCone
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetEllipticalConeData(
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 3), Out] double[] pBasePoint,
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 3), Out] double[] pAxisVector,
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 3), Out] double[] pMajorAxis,
    out double pMinorMajorRatio,
    out double pHalfAngle,
    out sbyte pbIsExpanding);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void PutEllipticalConeData(
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 3), In] double[] pBasePoint,
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 3), In] double[] pAxisVector,
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 3), In] double[] pMajorAxis,
    [In] double MinorMajorRatio,
    [In] double HalfAngle,
    [In] sbyte bIsExpanding);

  [DispId(67126275)]
  IRxPoint BasePoint { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.Interface), In] set; }

  [DispId(67126276)]
  IRxUnitVector AxisVector { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.Interface), In] set; }

  [DispId(67126277)]
  IRxVector MajorAxisVector { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.Interface), In] set; }

  [DispId(67126278)]
  double MinorMajorRatio { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(67126279)]
  double HalfAngle { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(67126280)]
  sbyte IsExpanding { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(67126281)]
  IRxSurfaceEvaluator Evaluator { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }
}
