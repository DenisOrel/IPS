// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxFaceShell
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[InterfaceType(1)]
[Guid("5DF86068-6B16-11D3-B794-0060B0F159EF")]
[TypeLibType(16 /*0x10*/)]
[ComImport]
public interface IRxFaceShell
{
  [DispId(67119105)]
  IRxEnumFaces Faces { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(67119106)]
  IRxEnumEdges Edges { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(67119107)]
  IRxSurfaceBody SurfaceBody { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(67119108)]
  sbyte IsClosed { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(67119109)]
  sbyte IsVoid { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  sbyte get_IsPointInside([MarshalAs(UnmanagedType.LPArray, SizeConst = 3), In] double[] Point);

  [DispId(67119111)]
  IRxBox RangeBox { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  double get_Volume([In] double PrecisionPercent);
}
