// Decompiled with JetBrains decompiler
// Type: InventorApprentice._DocPerformanceMonitorSink
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("C2083475-A259-414A-BED9-FC43291F4455")]
[InterfaceType(2)]
[TypeLibType(4112)]
[ComImport]
public interface _DocPerformanceMonitorSink
{
  [DispId(50377457)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void OnSegmentLoad([MarshalAs(UnmanagedType.BStr), In] string SegmentName);

  [DispId(50377458)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void OnDatabaseClose();

  [DispId(50377459)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void OnIStorageOpen([In] int OpenedWithFlags);

  [DispId(50377460)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void OnIStorageClose();

  [DispId(50377461)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void OnViewUpdate([MarshalAs(UnmanagedType.IDispatch), In] object ViewObject, [In] int BeforeOrAfter, [MarshalAs(UnmanagedType.Interface), In] NameValueMap Context);

  [DispId(50377462)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void OnIdle([MarshalAs(UnmanagedType.Interface), In] NameValueMap Context);
}
