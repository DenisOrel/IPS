// Decompiled with JetBrains decompiler
// Type: InventorApprentice.ProgressBarSink_Event
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(16 /*0x10*/)]
[ComEventInterface(typeof (ProgressBarSink), typeof (ProgressBarSink_EventProvider))]
[ComVisible(false)]
public interface ProgressBarSink_Event
{
  event ProgressBarSink_OnCancelEventHandler OnCancel;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void add_OnCancel([In] ProgressBarSink_OnCancelEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void remove_OnCancel([In] ProgressBarSink_OnCancelEventHandler obj0);
}
