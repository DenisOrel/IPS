// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxLine
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(16 /*0x10*/)]
[Guid("CB69F163-558E-11D3-B793-0060B0F159EF")]
[InterfaceType(1)]
[ComImport]
public interface IRxLine
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetLineData([MarshalAs(UnmanagedType.LPArray, SizeConst = 3), Out] double[] pRootPoint, [MarshalAs(UnmanagedType.LPArray, SizeConst = 3), Out] double[] pDirection);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void PutLineData([MarshalAs(UnmanagedType.LPArray, SizeConst = 3), In] double[] pRootPoint, [MarshalAs(UnmanagedType.LPArray, SizeConst = 3), In] double[] pDirection);

  [DispId(67111939 /*0x04000C03*/)]
  IRxPoint RootPoint { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.Interface), In] set; }

  [DispId(67111940 /*0x04000C04*/)]
  IRxUnitVector Direction { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.Interface), In] set; }

  [DispId(67111941 /*0x04000C05*/)]
  IRxCurveEvaluator Evaluator { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }
}
