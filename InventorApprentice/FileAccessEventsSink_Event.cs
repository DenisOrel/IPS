// Decompiled with JetBrains decompiler
// Type: InventorApprentice.FileAccessEventsSink_Event
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(16 /*0x10*/)]
[ComVisible(false)]
[ComEventInterface(typeof (FileAccessEventsSink), typeof (FileAccessEventsSink_EventProvider))]
public interface FileAccessEventsSink_Event
{
  event FileAccessEventsSink_OnFileResolutionEventHandler OnFileResolution;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void add_OnFileResolution(
    [In] FileAccessEventsSink_OnFileResolutionEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void remove_OnFileResolution(
    [In] FileAccessEventsSink_OnFileResolutionEventHandler obj0);

  event FileAccessEventsSink_OnFileDirtyEventHandler OnFileDirty;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void add_OnFileDirty([In] FileAccessEventsSink_OnFileDirtyEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void remove_OnFileDirty([In] FileAccessEventsSink_OnFileDirtyEventHandler obj0);
}
