// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxPoint2d
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("CB69F15E-558E-11D3-B793-0060B0F159EF")]
[TypeLibType(16 /*0x10*/)]
[InterfaceType(1)]
[ComImport]
public interface IRxPoint2d
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetPointData([MarshalAs(UnmanagedType.LPArray, SizeConst = 2), Out] double[] pCoords);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void PutPointData([MarshalAs(UnmanagedType.LPArray, SizeConst = 2), In] double[] pCoords);

  [DispId(67111299)]
  double X { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(67111300)]
  double Y { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }
}
