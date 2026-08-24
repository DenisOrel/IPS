// Decompiled with JetBrains decompiler
// Type: InventorApprentice.CameraEventsSink_Event
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(16 /*0x10*/)]
[ComEventInterface(typeof (CameraEventsSink), typeof (CameraEventsSink_EventProvider))]
[ComVisible(false)]
public interface CameraEventsSink_Event
{
  event CameraEventsSink_OnCameraChangeEventHandler OnCameraChange;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void add_OnCameraChange([In] CameraEventsSink_OnCameraChangeEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void remove_OnCameraChange([In] CameraEventsSink_OnCameraChangeEventHandler obj0);
}
