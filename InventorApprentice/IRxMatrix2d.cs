// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxMatrix2d
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("CB69F15C-558E-11D3-B793-0060B0F159EF")]
[TypeLibType(16 /*0x10*/)]
[InterfaceType(1)]
[ComImport]
public interface IRxMatrix2d
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetMatrixData([MarshalAs(UnmanagedType.LPArray, SizeConst = 9), Out] double[] pCells);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void PutMatrixData([MarshalAs(UnmanagedType.LPArray, SizeConst = 9), In] double[] pCells);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  double get_Cell([In] uint dwRow, [In] uint dwCol);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void set_Cell([In] uint dwRow, [In] uint dwCol, [In] double pCell);
}
