// Decompiled with JetBrains decompiler
// Type: InventorApprentice.ReferenceKeyEventsClass
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[ComSourceInterfaces("InventorApprentice.ReferenceKeyEventsSink\0\0")]
[Guid("D893A325-547B-4DE2-8F8B-BD9594025979")]
[ClassInterface(0)]
[DefaultMember("Type")]
[ComImport]
public class ReferenceKeyEventsClass : 
  ReferenceKeyEventsObject,
  ReferenceKeyEvents,
  ReferenceKeyEventsSink_Event
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  internal extern ReferenceKeyEventsClass();

  [DispId(2130706433 /*0x7F000001*/)]
  public virtual extern object Application { [DispId(2130706433 /*0x7F000001*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IDispatch)] get; }

  [DispId(0)]
  public virtual extern ObjectTypeEnum Type { [DispId(0), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  public virtual extern event ReferenceKeyEventsSink_OnBindKeyToObjectEventHandler OnBindKeyToObject;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void add_OnBindKeyToObject(
    [In] ReferenceKeyEventsSink_OnBindKeyToObjectEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void remove_OnBindKeyToObject(
    [In] ReferenceKeyEventsSink_OnBindKeyToObjectEventHandler obj0);
}
