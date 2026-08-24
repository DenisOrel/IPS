// Decompiled with JetBrains decompiler
// Type: InventorApprentice.DebugInstrumentationClass
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[ComSourceInterfaces("InventorApprentice.DebugInstrumentationSink\0\0")]
[TypeLibType(16 /*0x10*/)]
[ClassInterface(0)]
[Guid("F6F33559-6984-11D5-8DF3-0010B541CAA8")]
[ComImport]
public class DebugInstrumentationClass : 
  DebugInstrumentationObject,
  DebugInstrumentation,
  DebugInstrumentationSink_Event
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  internal extern DebugInstrumentationClass();

  [DispId(50367233)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void GetLiveObjects([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_I4), In, Out] ref int[] Cookies);

  [DispId(50367234)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.BStr)]
  public virtual extern string get_ObjectDescription([In] int Cookie);

  [DispId(50367235)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.IUnknown)]
  public virtual extern object get_Object([In] int Cookie);

  [DispId(50367236)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern int GetObjectCookie([MarshalAs(UnmanagedType.IUnknown), In] object Object);

  [DispId(50367237)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern int get_ObjectReferenceCount([In] int Cookie);

  [DispId(50367238)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern int get_ObjectInstanceNumber([In] int Cookie);

  [DispId(50367239)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern DebugWatchType get_ObjectWatchType([In] int Cookie);

  [DispId(50367239)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void set_ObjectWatchType([In] int Cookie, [In] DebugWatchType _param2);

  [DispId(50367240)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void SetTrace([In] bool Enable = true, [MarshalAs(UnmanagedType.BStr), In] string TraceFilename = "");

  [DispId(50367241)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void GetTraceInfo(out bool Enabled, [MarshalAs(UnmanagedType.BStr)] out string TraceFilename);

  [DispId(50367242)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void SetProfileInfo([In] bool Enable, [In] bool WriteToFileOnStop = true, [MarshalAs(UnmanagedType.BStr), In] string FileName = "");

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void add_ObjectCreated(
    [In] DebugInstrumentationSink_ObjectCreatedEventHandler obj0);

  public virtual extern event DebugInstrumentationSink_ObjectCreatedEventHandler ObjectCreated;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void remove_ObjectCreated(
    [In] DebugInstrumentationSink_ObjectCreatedEventHandler obj0);

  public virtual extern event DebugInstrumentationSink_ObjectDestroyedEventHandler ObjectDestroyed;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void add_ObjectDestroyed(
    [In] DebugInstrumentationSink_ObjectDestroyedEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void remove_ObjectDestroyed(
    [In] DebugInstrumentationSink_ObjectDestroyedEventHandler obj0);

  public virtual extern event DebugInstrumentationSink_ObjectAddRefdEventHandler ObjectAddRefd;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void add_ObjectAddRefd(
    [In] DebugInstrumentationSink_ObjectAddRefdEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void remove_ObjectAddRefd(
    [In] DebugInstrumentationSink_ObjectAddRefdEventHandler obj0);

  public virtual extern event DebugInstrumentationSink_ObjectReleasedEventHandler ObjectReleased;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void add_ObjectReleased(
    [In] DebugInstrumentationSink_ObjectReleasedEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void remove_ObjectReleased(
    [In] DebugInstrumentationSink_ObjectReleasedEventHandler obj0);

  public virtual extern event DebugInstrumentationSink_ObjectQueryInterfacedEventHandler ObjectQueryInterfaced;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void add_ObjectQueryInterfaced(
    [In] DebugInstrumentationSink_ObjectQueryInterfacedEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void remove_ObjectQueryInterfaced(
    [In] DebugInstrumentationSink_ObjectQueryInterfacedEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void add_OnMemberInvoke(
    [In] DebugInstrumentationSink_OnMemberInvokeEventHandler obj0);

  public virtual extern event DebugInstrumentationSink_OnMemberInvokeEventHandler OnMemberInvoke;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void remove_OnMemberInvoke(
    [In] DebugInstrumentationSink_OnMemberInvokeEventHandler obj0);
}
