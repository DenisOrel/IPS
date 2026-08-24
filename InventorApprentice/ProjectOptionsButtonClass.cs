// Decompiled with JetBrains decompiler
// Type: InventorApprentice.ProjectOptionsButtonClass
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[ComSourceInterfaces("InventorApprentice.ProjectOptionsButtonSink\0\0")]
[Guid("A4791D9E-EEA1-4524-8543-174DA9CC42B3")]
[ClassInterface(0)]
[DefaultMember("Type")]
[ComImport]
public class ProjectOptionsButtonClass : 
  ProjectOptionsButtonObject,
  ProjectOptionsButton,
  ProjectOptionsButtonSink_Event
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  internal extern ProjectOptionsButtonClass();

  [DispId(0)]
  public virtual extern ObjectTypeEnum Type { [DispId(0), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(2130706433 /*0x7F000001*/)]
  public virtual extern Application Application { [DispId(2130706433 /*0x7F000001*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(50445057)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void FireOnClick([MarshalAs(UnmanagedType.Interface), In] NameValueMap Context);

  [DispId(50445058)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void Delete();

  [DispId(50445059)]
  public virtual extern string DisplayName { [DispId(50445059), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(50445059), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.BStr), In] set; }

  public virtual extern event ProjectOptionsButtonSink_OnClickEventHandler OnClick;

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void add_OnClick([In] ProjectOptionsButtonSink_OnClickEventHandler obj0);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void remove_OnClick([In] ProjectOptionsButtonSink_OnClickEventHandler obj0);
}
