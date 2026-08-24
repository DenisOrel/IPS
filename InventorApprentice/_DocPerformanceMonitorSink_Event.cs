// Decompiled with JetBrains decompiler
// Type: InventorApprentice._DocPerformanceMonitorSink_Event
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[ComEventInterface(typeof (_DocPerformanceMonitorSink), typeof (_DocPerformanceMonitorSink_EventProvider))]
[TypeLibType(16 /*0x10*/)]
[ComVisible(false)]
public interface _DocPerformanceMonitorSink_Event
{
  event _DocPerformanceMonitorSink_OnSegmentLoadEventHandler OnSegmentLoad;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void add_OnSegmentLoad(
    [In] _DocPerformanceMonitorSink_OnSegmentLoadEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void remove_OnSegmentLoad(
    [In] _DocPerformanceMonitorSink_OnSegmentLoadEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void add_OnDatabaseClose(
    [In] _DocPerformanceMonitorSink_OnDatabaseCloseEventHandler obj0);

  event _DocPerformanceMonitorSink_OnDatabaseCloseEventHandler OnDatabaseClose;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void remove_OnDatabaseClose(
    [In] _DocPerformanceMonitorSink_OnDatabaseCloseEventHandler obj0);

  event _DocPerformanceMonitorSink_OnIStorageOpenEventHandler OnIStorageOpen;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void add_OnIStorageOpen(
    [In] _DocPerformanceMonitorSink_OnIStorageOpenEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void remove_OnIStorageOpen(
    [In] _DocPerformanceMonitorSink_OnIStorageOpenEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void add_OnIStorageClose(
    [In] _DocPerformanceMonitorSink_OnIStorageCloseEventHandler obj0);

  event _DocPerformanceMonitorSink_OnIStorageCloseEventHandler OnIStorageClose;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void remove_OnIStorageClose(
    [In] _DocPerformanceMonitorSink_OnIStorageCloseEventHandler obj0);

  event _DocPerformanceMonitorSink_OnViewUpdateEventHandler OnViewUpdate;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void add_OnViewUpdate(
    [In] _DocPerformanceMonitorSink_OnViewUpdateEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void remove_OnViewUpdate(
    [In] _DocPerformanceMonitorSink_OnViewUpdateEventHandler obj0);

  event _DocPerformanceMonitorSink_OnIdleEventHandler OnIdle;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void add_OnIdle([In] _DocPerformanceMonitorSink_OnIdleEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void remove_OnIdle([In] _DocPerformanceMonitorSink_OnIdleEventHandler obj0);
}
