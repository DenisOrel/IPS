// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxUnitVector2d
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[InterfaceType(1)]
[Guid("CB69F162-558E-11D3-B793-0060B0F159EF")]
[TypeLibType(16 /*0x10*/)]
[ComImport]
public interface IRxUnitVector2d
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetUnitVectorData([MarshalAs(UnmanagedType.LPArray, SizeConst = 2), Out] double[] ppCoords);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void PutUnitVectorData([MarshalAs(UnmanagedType.LPArray, SizeConst = 2), In] double[] pCoords);

  [DispId(67111811)]
  double X { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(67111812)]
  double Y { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }
}
