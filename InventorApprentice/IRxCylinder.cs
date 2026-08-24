// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxCylinder
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(16 /*0x10*/)]
[InterfaceType(1)]
[Guid("5DF86098-6B16-11D3-B794-0060B0F159EF")]
[ComImport]
public interface IRxCylinder
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetCylinderData([MarshalAs(UnmanagedType.LPArray, SizeConst = 3), Out] double[] pBasePoint, [MarshalAs(UnmanagedType.LPArray, SizeConst = 3), Out] double[] pAxisVector, out double pRadius);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void PutCylinderData([MarshalAs(UnmanagedType.LPArray, SizeConst = 3), In] double[] pBasePoint, [MarshalAs(UnmanagedType.LPArray, SizeConst = 3), In] double[] pAxisVector, [In] double Radius);

  [DispId(67123251)]
  IRxPoint BasePoint { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.Interface), In] set; }

  [DispId(67123252)]
  IRxUnitVector AxisVector { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.Interface), In] set; }

  [DispId(67123253)]
  double Radius { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(67123254)]
  IRxSurfaceEvaluator Evaluator { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }
}
