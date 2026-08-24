// Decompiled with JetBrains decompiler
// Type: InventorApprentice.FileDialogEventsClass
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[ComSourceInterfaces("InventorApprentice.FileDialogEventsSink\0\0")]
[Guid("8CBEA17E-7987-4C21-B3AA-429602F6BAA8")]
[ClassInterface(0)]
[DefaultMember("Type")]
[ComImport]
public class FileDialogEventsClass : 
  FileDialogEventsObject,
  FileDialogEvents,
  FileDialogEventsSink_Event
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  internal extern FileDialogEventsClass();

  [DispId(0)]
  public virtual extern ObjectTypeEnum Type { [DispId(0), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(2130706434 /*0x7F000002*/)]
  public virtual extern FileDialog Parent { [DispId(2130706434 /*0x7F000002*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(2130706433 /*0x7F000001*/)]
  public virtual extern object Application { [DispId(2130706433 /*0x7F000001*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IDispatch)] get; }

  public virtual extern event FileDialogEventsSink_OnOptionsEventHandler OnOptions;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void add_OnOptions([In] FileDialogEventsSink_OnOptionsEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void remove_OnOptions([In] FileDialogEventsSink_OnOptionsEventHandler obj0);
}
