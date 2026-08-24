// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxFileAccessEvents_Event
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[ComEventInterface(typeof (IRxFileAccessEvents), typeof (IRxFileAccessEvents_EventProvider))]
[TypeLibType(16 /*0x10*/)]
[ComVisible(false)]
public interface IRxFileAccessEvents_Event
{
  event IRxFileAccessEvents_OnFileResolutionEventHandler OnFileResolution;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void add_OnFileResolution(
    [In] IRxFileAccessEvents_OnFileResolutionEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void remove_OnFileResolution(
    [In] IRxFileAccessEvents_OnFileResolutionEventHandler obj0);

  event IRxFileAccessEvents_OnFileDirtyEventHandler OnFileDirty;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void add_OnFileDirty([In] IRxFileAccessEvents_OnFileDirtyEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void remove_OnFileDirty([In] IRxFileAccessEvents_OnFileDirtyEventHandler obj0);
}
