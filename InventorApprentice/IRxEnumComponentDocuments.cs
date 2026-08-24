// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxEnumComponentDocuments
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("59D3FA3B-ACE0-11D3-B79A-0060B0F159EF")]
[InterfaceType(1)]
[TypeLibType(16 /*0x10*/)]
[ComImport]
public interface IRxEnumComponentDocuments
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Next(
    [In] uint nCompDefDocs,
    [MarshalAs(UnmanagedType.Interface)] out IRxComponentDocument ppCompDefDocs,
    out uint pnCompDefDocsFetched);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Skip([In] uint nCompDefs);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Reset();

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Clone([MarshalAs(UnmanagedType.Interface)] out IRxEnumComponentDocuments ppResult);
}
