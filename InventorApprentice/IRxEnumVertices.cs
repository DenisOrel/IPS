// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxEnumVertices
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[InterfaceType(1)]
[TypeLibType(16 /*0x10*/)]
[Guid("5DF86076-6B16-11D3-B794-0060B0F159EF")]
[ComImport]
public interface IRxEnumVertices
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Next([In] uint nBodies, [MarshalAs(UnmanagedType.Interface)] out IRxVertex ppBodies, out uint pnBodiesFetched);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Skip([In] uint nCurves);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Reset();

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Clone([MarshalAs(UnmanagedType.Interface)] out IRxEnumVertices ppEnum);
}
