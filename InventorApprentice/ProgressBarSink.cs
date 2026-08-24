// Decompiled with JetBrains decompiler
// Type: InventorApprentice.ProgressBarSink
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[InterfaceType(2)]
[Guid("6322C608-F92C-4CBB-9C17-B1661DA641AB")]
[TypeLibType(4096 /*0x1000*/)]
[ComImport]
public interface ProgressBarSink
{
  [DispId(50420097)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void OnCancel();
}
