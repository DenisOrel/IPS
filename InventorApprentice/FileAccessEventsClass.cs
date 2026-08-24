// Decompiled with JetBrains decompiler
// Type: InventorApprentice.FileAccessEventsClass
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[DefaultMember("Type")]
[Guid("32E4A319-C5E8-11D2-B77F-0060B0F159EF")]
[ClassInterface(0)]
[ComSourceInterfaces("InventorApprentice.FileAccessEventsSink\0InventorApprentice.IRxFileAccessEvents\0\0")]
[ComImport]
public class FileAccessEventsClass : 
  FileAccessEventsObject,
  FileAccessEvents,
  FileAccessEventsSink_Event,
  IRxFileAccessEvents_Event
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  internal extern FileAccessEventsClass();

  [DispId(0)]
  public virtual extern ObjectTypeEnum Type { [DispId(0), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(2130706434 /*0x7F000002*/)]
  public virtual extern object Parent { [DispId(2130706434 /*0x7F000002*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IDispatch)] get; }

  [DispId(2130706433 /*0x7F000001*/)]
  public virtual extern object Application { [DispId(2130706433 /*0x7F000001*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IDispatch)] get; }

  [DispId(50335489 /*0x03000F01*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void FireOnFileResolution(
    [MarshalAs(UnmanagedType.BStr), In] string RelativeFileName,
    [MarshalAs(UnmanagedType.BStr), In] string LibraryName,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UI1), In, Out] ref byte[] CustomLogicalName,
    [In] EventTimingEnum BeforeOrAfter,
    [MarshalAs(UnmanagedType.Interface), In] NameValueMap Context,
    [MarshalAs(UnmanagedType.BStr)] out string FullFileName,
    out HandlingCodeEnum HandlingCode);

  public virtual extern event FileAccessEventsSink_OnFileResolutionEventHandler OnFileResolution;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void add_OnFileResolution(
    [In] FileAccessEventsSink_OnFileResolutionEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void remove_OnFileResolution(
    [In] FileAccessEventsSink_OnFileResolutionEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void add_OnFileDirty([In] FileAccessEventsSink_OnFileDirtyEventHandler obj0);

  public virtual extern event FileAccessEventsSink_OnFileDirtyEventHandler OnFileDirty;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void remove_OnFileDirty([In] FileAccessEventsSink_OnFileDirtyEventHandler obj0);

  public virtual extern event IRxFileAccessEvents_OnFileResolutionEventHandler IRxFileAccessEvents_Event_OnFileResolution;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void IRxFileAccessEvents_Event_add_OnFileResolution(
    [In] IRxFileAccessEvents_OnFileResolutionEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void IRxFileAccessEvents_Event_remove_OnFileResolution(
    [In] IRxFileAccessEvents_OnFileResolutionEventHandler obj0);

  public virtual extern event IRxFileAccessEvents_OnFileDirtyEventHandler IRxFileAccessEvents_Event_OnFileDirty;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void IRxFileAccessEvents_Event_add_OnFileDirty(
    [In] IRxFileAccessEvents_OnFileDirtyEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void IRxFileAccessEvents_Event_remove_OnFileDirty(
    [In] IRxFileAccessEvents_OnFileDirtyEventHandler obj0);
}
