// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxEllipseFull
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[InterfaceType(1)]
[Guid("5DF86030-6B16-11D3-B794-0060B0F159EF")]
[TypeLibType(16 /*0x10*/)]
[ComImport]
public interface IRxEllipseFull
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetEllipseFullData(
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 3), Out] double[] pCenter,
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 3), Out] double[] pNormal,
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 3), Out] double[] pMajorAxis,
    out double pMinorMajorRatio);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void PutEllipseFullData(
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 3), In] double[] pCenter,
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 3), In] double[] pNormal,
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 3), In] double[] pMajorAxis,
    [In] double MinorMajorRatio);

  [DispId(67115779)]
  IRxPoint Center { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.Interface), In] set; }

  [DispId(67115780)]
  IRxUnitVector Normal { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.Interface), In] set; }

  [DispId(67115781)]
  IRxVector MajorAxisVector { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.Interface), In] set; }

  [DispId(67115782)]
  double MinorMajorRatio { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(67115783)]
  IRxCurveEvaluator Evaluator { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }
}
