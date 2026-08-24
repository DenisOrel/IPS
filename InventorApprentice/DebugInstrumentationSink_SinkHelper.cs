// Decompiled with JetBrains decompiler
// Type: InventorApprentice.DebugInstrumentationSink_SinkHelper
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[ClassInterface(ClassInterfaceType.None)]
[TypeLibType(TypeLibTypeFlags.FHidden)]
public sealed class DebugInstrumentationSink_SinkHelper : DebugInstrumentationSink
{
  public DebugInstrumentationSink_ObjectCreatedEventHandler m_ObjectCreatedDelegate;
  public DebugInstrumentationSink_ObjectDestroyedEventHandler m_ObjectDestroyedDelegate;
  public DebugInstrumentationSink_ObjectAddRefdEventHandler m_ObjectAddRefdDelegate;
  public DebugInstrumentationSink_ObjectReleasedEventHandler m_ObjectReleasedDelegate;
  public DebugInstrumentationSink_ObjectQueryInterfacedEventHandler m_ObjectQueryInterfacedDelegate;
  public DebugInstrumentationSink_OnMemberInvokeEventHandler m_OnMemberInvokeDelegate;
  public int m_dwCookie;

  public override void ObjectCreated([In] int obj0)
  {
    if (this.m_ObjectCreatedDelegate == null)
      return;
    this.m_ObjectCreatedDelegate(obj0);
  }

  public override void ObjectDestroyed([In] int obj0)
  {
    if (this.m_ObjectDestroyedDelegate == null)
      return;
    this.m_ObjectDestroyedDelegate(obj0);
  }

  public override void ObjectAddRefd([In] int obj0, [In] int obj1)
  {
    if (this.m_ObjectAddRefdDelegate == null)
      return;
    this.m_ObjectAddRefdDelegate(obj0, obj1);
  }

  public override void ObjectReleased([In] int obj0, [In] int obj1)
  {
    if (this.m_ObjectReleasedDelegate == null)
      return;
    this.m_ObjectReleasedDelegate(obj0, obj1);
  }

  public override void ObjectQueryInterfaced([In] int obj0, [In] string obj1, [In] bool obj2)
  {
    if (this.m_ObjectQueryInterfacedDelegate == null)
      return;
    this.m_ObjectQueryInterfacedDelegate(obj0, obj1, obj2);
  }

  public override void OnMemberInvoke([In] int obj0, [In] string obj1, [In] EventTimingEnum obj2, [In] int obj3)
  {
    if (this.m_OnMemberInvokeDelegate == null)
      return;
    this.m_OnMemberInvokeDelegate(obj0, obj1, obj2, obj3);
  }

  internal DebugInstrumentationSink_SinkHelper()
  {
    this.m_dwCookie = 0;
    this.m_ObjectCreatedDelegate = (DebugInstrumentationSink_ObjectCreatedEventHandler) null;
    this.m_ObjectDestroyedDelegate = (DebugInstrumentationSink_ObjectDestroyedEventHandler) null;
    this.m_ObjectAddRefdDelegate = (DebugInstrumentationSink_ObjectAddRefdEventHandler) null;
    this.m_ObjectReleasedDelegate = (DebugInstrumentationSink_ObjectReleasedEventHandler) null;
    this.m_ObjectQueryInterfacedDelegate = (DebugInstrumentationSink_ObjectQueryInterfacedEventHandler) null;
    this.m_OnMemberInvokeDelegate = (DebugInstrumentationSink_OnMemberInvokeEventHandler) null;
  }
}
