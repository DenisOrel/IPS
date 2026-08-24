// Decompiled with JetBrains decompiler
// Type: InventorApprentice._SegPerformanceMonitor
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("AE621339-6CA8-486C-BF06-E683D2EE3A8E")]
[InterfaceType(2)]
[TypeLibType(4112)]
[ComImport]
public interface _SegPerformanceMonitor
{
  [DispId(50377473)]
  string Name { [DispId(50377473), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }

  [DispId(50377474)]
  _SegmentLoadState State { [DispId(50377474), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(50377475)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetMemoryInUse(out int Committed, out int Reserved, out int RSeManagement);

  [DispId(50377476)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetMemoryInUseFor64Bit(out long Committed, out long Reserved, out long RSeManagement);
}
