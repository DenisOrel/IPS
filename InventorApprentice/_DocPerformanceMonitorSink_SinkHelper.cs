// Decompiled with JetBrains decompiler
// Type: InventorApprentice._DocPerformanceMonitorSink_SinkHelper
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(TypeLibTypeFlags.FHidden)]
[ClassInterface(ClassInterfaceType.None)]
public sealed class _DocPerformanceMonitorSink_SinkHelper : _DocPerformanceMonitorSink
{
  public _DocPerformanceMonitorSink_OnSegmentLoadEventHandler m_OnSegmentLoadDelegate;
  public _DocPerformanceMonitorSink_OnDatabaseCloseEventHandler m_OnDatabaseCloseDelegate;
  public _DocPerformanceMonitorSink_OnIStorageOpenEventHandler m_OnIStorageOpenDelegate;
  public _DocPerformanceMonitorSink_OnIStorageCloseEventHandler m_OnIStorageCloseDelegate;
  public _DocPerformanceMonitorSink_OnViewUpdateEventHandler m_OnViewUpdateDelegate;
  public _DocPerformanceMonitorSink_OnIdleEventHandler m_OnIdleDelegate;
  public int m_dwCookie;

  public override void OnSegmentLoad([In] string obj0)
  {
    if (this.m_OnSegmentLoadDelegate == null)
      return;
    this.m_OnSegmentLoadDelegate(obj0);
  }

  public override void OnDatabaseClose()
  {
    if (this.m_OnDatabaseCloseDelegate == null)
      return;
    this.m_OnDatabaseCloseDelegate();
  }

  public override void OnIStorageOpen([In] int obj0)
  {
    if (this.m_OnIStorageOpenDelegate == null)
      return;
    this.m_OnIStorageOpenDelegate(obj0);
  }

  public override void OnIStorageClose()
  {
    if (this.m_OnIStorageCloseDelegate == null)
      return;
    this.m_OnIStorageCloseDelegate();
  }

  public override void OnViewUpdate([In] object obj0, [In] int obj1, [In] NameValueMap obj2)
  {
    if (this.m_OnViewUpdateDelegate == null)
      return;
    this.m_OnViewUpdateDelegate(obj0, obj1, obj2);
  }

  public override void OnIdle([In] NameValueMap obj0)
  {
    if (this.m_OnIdleDelegate == null)
      return;
    this.m_OnIdleDelegate(obj0);
  }

  internal _DocPerformanceMonitorSink_SinkHelper()
  {
    this.m_dwCookie = 0;
    this.m_OnSegmentLoadDelegate = (_DocPerformanceMonitorSink_OnSegmentLoadEventHandler) null;
    this.m_OnDatabaseCloseDelegate = (_DocPerformanceMonitorSink_OnDatabaseCloseEventHandler) null;
    this.m_OnIStorageOpenDelegate = (_DocPerformanceMonitorSink_OnIStorageOpenEventHandler) null;
    this.m_OnIStorageCloseDelegate = (_DocPerformanceMonitorSink_OnIStorageCloseEventHandler) null;
    this.m_OnViewUpdateDelegate = (_DocPerformanceMonitorSink_OnViewUpdateEventHandler) null;
    this.m_OnIdleDelegate = (_DocPerformanceMonitorSink_OnIdleEventHandler) null;
  }
}
