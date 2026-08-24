// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxEnumApplicationAddIns
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(16 /*0x10*/)]
[Guid("E3571290-DB40-11D2-B783-0060B0F159EF")]
[InterfaceType(1)]
[ComImport]
public interface IRxEnumApplicationAddIns
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Next([In] uint nElems, [MarshalAs(UnmanagedType.Interface)] out IRxApplicationAddIn ppElems, out uint pnElemsFetched);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Skip([In] uint nElems);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Reset();

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Clone([MarshalAs(UnmanagedType.Interface)] out IRxEnumApplicationAddIns ppEnum);
}
