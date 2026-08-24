// Decompiled with JetBrains decompiler
// Type: InventorApprentice.ISequentialStream
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("0C733A30-2A1C-11CE-ADE5-00AA0044773D")]
[InterfaceType(1)]
[ComImport]
public interface ISequentialStream
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void RemoteRead(out byte pv, [In] uint cb, out uint pcbRead);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void RemoteWrite([In] ref byte pv, [In] uint cb, out uint pcbWritten);
}
