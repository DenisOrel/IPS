// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxVector
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[InterfaceType(1)]
[Guid("CB69F15F-558E-11D3-B793-0060B0F159EF")]
[TypeLibType(16 /*0x10*/)]
[ComImport]
public interface IRxVector
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetVectorData([MarshalAs(UnmanagedType.LPArray, SizeConst = 3), Out] double[] pCoords);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void PutVectorData([MarshalAs(UnmanagedType.LPArray, SizeConst = 3), In] double[] pCoords);

  [DispId(67111427 /*0x04000A03*/)]
  double X { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(67111428 /*0x04000A04*/)]
  double Y { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(67111429 /*0x04000A05*/)]
  double Z { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }
}
