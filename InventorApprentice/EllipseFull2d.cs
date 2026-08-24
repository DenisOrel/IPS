// Decompiled with JetBrains decompiler
// Type: InventorApprentice.EllipseFull2d
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(4096 /*0x1000*/)]
[Guid("49CB4BB8-872A-11D3-8524-0060B0F0B5B7")]
[InterfaceType(2)]
[ComImport]
public interface EllipseFull2d
{
  [DispId(67115953)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetEllipseFullData([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In, Out] ref double[] Center, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In, Out] ref double[] MajorAxis, out double MinorMajorRatio);

  [DispId(67115954)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void PutEllipseFullData([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In, Out] ref double[] Center, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In, Out] ref double[] MajorAxis, [In] double MinorMajorRatio);

  [DispId(67115955)]
  Point2d Center { [DispId(67115955), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; [DispId(67115955), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.Interface), In] set; }

  [DispId(67115956)]
  Vector2d MajorAxisVector { [DispId(67115956), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; [DispId(67115956), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.Interface), In] set; }

  [DispId(67115957)]
  double MinorMajorRatio { [DispId(67115957), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(67115957), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(67115958)]
  Curve2dEvaluator Evaluator { [DispId(67115958), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(67115959)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  EllipseFull2d Copy();
}
