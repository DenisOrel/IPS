// Decompiled with JetBrains decompiler
// Type: InventorApprentice.ReferenceKeyEventsSink_Event
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(16 /*0x10*/)]
[ComVisible(false)]
[ComEventInterface(typeof (ReferenceKeyEventsSink), typeof (ReferenceKeyEventsSink_EventProvider))]
public interface ReferenceKeyEventsSink_Event
{
  event ReferenceKeyEventsSink_OnBindKeyToObjectEventHandler OnBindKeyToObject;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void add_OnBindKeyToObject(
    [In] ReferenceKeyEventsSink_OnBindKeyToObjectEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void remove_OnBindKeyToObject(
    [In] ReferenceKeyEventsSink_OnBindKeyToObjectEventHandler obj0);
}
