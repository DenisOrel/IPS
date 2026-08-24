// Decompiled with JetBrains decompiler
// Type: InventorApprentice._DocPerformanceMonitorClass
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[ClassInterface(0)]
[Guid("89832854-67B3-4DBF-B8E6-715435D51FE2")]
[TypeLibType(16 /*0x10*/)]
[ComSourceInterfaces("InventorApprentice._DocPerformanceMonitorSink\0\0")]
[ComImport]
public class _DocPerformanceMonitorClass : 
  _DocPerformanceMonitorObject,
  _DocPerformanceMonitor,
  _DocPerformanceMonitorSink_Event
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  internal extern _DocPerformanceMonitorClass();

  [DispId(50377217)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void GetGraphicsLOD(out int NumberOfLODs, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In, Out] ref double[] LODTolerances);

  [DispId(50377218)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void SetGraphicsLOD([In] int NumberOfLODs);

  [DispId(50377219)]
  public virtual extern int SegmentCount { [DispId(50377219), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(50377220)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  public virtual extern _SegPerformanceMonitor get_SegmentItem([MarshalAs(UnmanagedType.Struct), In] object index);

  [DispId(50377221)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void GetMemoryInUse(
    out int Committed,
    out int Reserved,
    out int InActiveUse);

  [DispId(50377222)]
  public virtual extern int NodeCount { [DispId(50377222), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(50377223)]
  public virtual extern int ASMHistoryMemory { [DispId(50377223), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(50377224)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void SetDetectLeaks([In] bool detect);

  [DispId(50377225)]
  public virtual extern bool LeakDetected { [DispId(50377225), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(50377226)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void DontSaveIdentifiedMTLeaks([In] bool dontSave);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void add_OnSegmentLoad(
    [In] _DocPerformanceMonitorSink_OnSegmentLoadEventHandler obj0);

  public virtual extern event _DocPerformanceMonitorSink_OnSegmentLoadEventHandler OnSegmentLoad;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void remove_OnSegmentLoad(
    [In] _DocPerformanceMonitorSink_OnSegmentLoadEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void add_OnDatabaseClose(
    [In] _DocPerformanceMonitorSink_OnDatabaseCloseEventHandler obj0);

  public virtual extern event _DocPerformanceMonitorSink_OnDatabaseCloseEventHandler OnDatabaseClose;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void remove_OnDatabaseClose(
    [In] _DocPerformanceMonitorSink_OnDatabaseCloseEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void add_OnIStorageOpen(
    [In] _DocPerformanceMonitorSink_OnIStorageOpenEventHandler obj0);

  public virtual extern event _DocPerformanceMonitorSink_OnIStorageOpenEventHandler OnIStorageOpen;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void remove_OnIStorageOpen(
    [In] _DocPerformanceMonitorSink_OnIStorageOpenEventHandler obj0);

  public virtual extern event _DocPerformanceMonitorSink_OnIStorageCloseEventHandler OnIStorageClose;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void add_OnIStorageClose(
    [In] _DocPerformanceMonitorSink_OnIStorageCloseEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void remove_OnIStorageClose(
    [In] _DocPerformanceMonitorSink_OnIStorageCloseEventHandler obj0);

  public virtual extern event _DocPerformanceMonitorSink_OnViewUpdateEventHandler OnViewUpdate;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void add_OnViewUpdate(
    [In] _DocPerformanceMonitorSink_OnViewUpdateEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void remove_OnViewUpdate(
    [In] _DocPerformanceMonitorSink_OnViewUpdateEventHandler obj0);

  public virtual extern event _DocPerformanceMonitorSink_OnIdleEventHandler OnIdle;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void add_OnIdle([In] _DocPerformanceMonitorSink_OnIdleEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void remove_OnIdle([In] _DocPerformanceMonitorSink_OnIdleEventHandler obj0);
}
