// Decompiled with JetBrains decompiler
// Type: InventorApprentice.FileManagerEventsSink_Event
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[ComVisible(false)]
[ComEventInterface(typeof (FileManagerEventsSink), typeof (FileManagerEventsSink_EventProvider))]
[TypeLibType(16 /*0x10*/)]
public interface FileManagerEventsSink_Event
{
  event FileManagerEventsSink_OnFileDeleteEventHandler OnFileDelete;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void add_OnFileDelete(
    [In] FileManagerEventsSink_OnFileDeleteEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void remove_OnFileDelete(
    [In] FileManagerEventsSink_OnFileDeleteEventHandler obj0);

  event FileManagerEventsSink_OnFileCopyEventHandler OnFileCopy;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void add_OnFileCopy([In] FileManagerEventsSink_OnFileCopyEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void remove_OnFileCopy([In] FileManagerEventsSink_OnFileCopyEventHandler obj0);
}
