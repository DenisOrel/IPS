// Decompiled with JetBrains decompiler
// Type: InventorApprentice.ProjectOptionsButtonSink_Event
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(16 /*0x10*/)]
[ComVisible(false)]
[ComEventInterface(typeof (ProjectOptionsButtonSink), typeof (ProjectOptionsButtonSink_EventProvider))]
public interface ProjectOptionsButtonSink_Event
{
  event ProjectOptionsButtonSink_OnClickEventHandler OnClick;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void add_OnClick([In] ProjectOptionsButtonSink_OnClickEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void remove_OnClick([In] ProjectOptionsButtonSink_OnClickEventHandler obj0);
}
