// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxBox
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(16 /*0x10*/)]
[Guid("5DF8602A-6B16-11D3-B794-0060B0F159EF")]
[InterfaceType(1)]
[ComImport]
public interface IRxBox
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetBoxData([MarshalAs(UnmanagedType.LPArray, SizeConst = 3), Out] double[] pMinPoint, [MarshalAs(UnmanagedType.LPArray, SizeConst = 3), Out] double[] pMaxPoint);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void PutBoxData([MarshalAs(UnmanagedType.LPArray, SizeConst = 3), In] double[] pMinPoint, [MarshalAs(UnmanagedType.LPArray, SizeConst = 3), In] double[] pMaxPoint);

  [DispId(67115267)]
  IRxPoint MinPoint { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.Interface), In] set; }

  [DispId(67115268)]
  IRxPoint MaxPoint { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.Interface), In] set; }
}
