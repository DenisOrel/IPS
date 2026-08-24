// Decompiled with JetBrains decompiler
// Type: InventorApprentice.HelpEventsClass
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[DefaultMember("Type")]
[ComSourceInterfaces("InventorApprentice.HelpEventsSink\0\0")]
[Guid("285898F7-E731-44FA-B327-540394EBE313")]
[ClassInterface(0)]
[ComImport]
public class HelpEventsClass : HelpEventsObject, HelpEvents, HelpEventsSink_Event
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  internal extern HelpEventsClass();

  [DispId(2130706433 /*0x7F000001*/)]
  public virtual extern object Application { [DispId(2130706433 /*0x7F000001*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IDispatch)] get; }

  [DispId(0)]
  public virtual extern ObjectTypeEnum Type { [DispId(0), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  public virtual extern event HelpEventsSink_OnApplicationHelpEventHandler OnApplicationHelp;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void add_OnApplicationHelp([In] HelpEventsSink_OnApplicationHelpEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void remove_OnApplicationHelp(
    [In] HelpEventsSink_OnApplicationHelpEventHandler obj0);
}
