// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxEnumFileAndReferences
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(16 /*0x10*/)]
[InterfaceType(1)]
[Guid("2C9F9B60-8967-11D2-86B1-080009DB6864")]
[ComImport]
public interface IRxEnumFileAndReferences
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Next([In] uint nElems, [MarshalAs(UnmanagedType.Interface)] out IRxFileAndReferences ppElems, out uint pnElemsFetched);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Skip([In] uint nElems);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Reset();

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Clone([MarshalAs(UnmanagedType.Interface)] out IRxEnumFileAndReferences ppEnum);
}
