// Decompiled with JetBrains decompiler
// Type: InventorApprentice.FileManagerEventsClass
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[ClassInterface(0)]
[DefaultMember("Type")]
[ComSourceInterfaces("InventorApprentice.FileManagerEventsSink\0\0")]
[Guid("A44AF926-6383-42F0-8B2D-253F82F95ABE")]
[ComImport]
public class FileManagerEventsClass : 
  FileManagerEventsObject,
  FileManagerEvents,
  FileManagerEventsSink_Event
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  internal extern FileManagerEventsClass();

  [DispId(0)]
  public virtual extern ObjectTypeEnum Type { [DispId(0), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(2130706434 /*0x7F000002*/)]
  public virtual extern object Parent { [DispId(2130706434 /*0x7F000002*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IDispatch)] get; }

  [DispId(2130706433 /*0x7F000001*/)]
  public virtual extern object Application { [DispId(2130706433 /*0x7F000001*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IDispatch)] get; }

  public virtual extern event FileManagerEventsSink_OnFileDeleteEventHandler OnFileDelete;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void add_OnFileDelete(
    [In] FileManagerEventsSink_OnFileDeleteEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void remove_OnFileDelete(
    [In] FileManagerEventsSink_OnFileDeleteEventHandler obj0);

  public virtual extern event FileManagerEventsSink_OnFileCopyEventHandler OnFileCopy;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void add_OnFileCopy([In] FileManagerEventsSink_OnFileCopyEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void remove_OnFileCopy([In] FileManagerEventsSink_OnFileCopyEventHandler obj0);
}
