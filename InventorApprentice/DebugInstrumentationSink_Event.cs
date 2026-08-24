// Decompiled with JetBrains decompiler
// Type: InventorApprentice.DebugInstrumentationSink_Event
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[ComEventInterface(typeof (DebugInstrumentationSink), typeof (DebugInstrumentationSink_EventProvider))]
[TypeLibType(16 /*0x10*/)]
[ComVisible(false)]
public interface DebugInstrumentationSink_Event
{
  event DebugInstrumentationSink_ObjectCreatedEventHandler ObjectCreated;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void add_ObjectCreated(
    [In] DebugInstrumentationSink_ObjectCreatedEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void remove_ObjectCreated(
    [In] DebugInstrumentationSink_ObjectCreatedEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void add_ObjectDestroyed(
    [In] DebugInstrumentationSink_ObjectDestroyedEventHandler obj0);

  event DebugInstrumentationSink_ObjectDestroyedEventHandler ObjectDestroyed;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void remove_ObjectDestroyed(
    [In] DebugInstrumentationSink_ObjectDestroyedEventHandler obj0);

  event DebugInstrumentationSink_ObjectAddRefdEventHandler ObjectAddRefd;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void add_ObjectAddRefd(
    [In] DebugInstrumentationSink_ObjectAddRefdEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void remove_ObjectAddRefd(
    [In] DebugInstrumentationSink_ObjectAddRefdEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void add_ObjectReleased(
    [In] DebugInstrumentationSink_ObjectReleasedEventHandler obj0);

  event DebugInstrumentationSink_ObjectReleasedEventHandler ObjectReleased;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void remove_ObjectReleased(
    [In] DebugInstrumentationSink_ObjectReleasedEventHandler obj0);

  event DebugInstrumentationSink_ObjectQueryInterfacedEventHandler ObjectQueryInterfaced;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void add_ObjectQueryInterfaced(
    [In] DebugInstrumentationSink_ObjectQueryInterfacedEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void remove_ObjectQueryInterfaced(
    [In] DebugInstrumentationSink_ObjectQueryInterfacedEventHandler obj0);

  event DebugInstrumentationSink_OnMemberInvokeEventHandler OnMemberInvoke;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void add_OnMemberInvoke(
    [In] DebugInstrumentationSink_OnMemberInvokeEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void remove_OnMemberInvoke(
    [In] DebugInstrumentationSink_OnMemberInvokeEventHandler obj0);
}
