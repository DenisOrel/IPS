// Decompiled with JetBrains decompiler
// Type: InventorApprentice.DebugInstrumentationSink
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("F6F3355B-6984-11D5-8DF3-0010B541CAA8")]
[TypeLibType(4112)]
[InterfaceType(2)]
[ComImport]
public interface DebugInstrumentationSink
{
  [DispId(50367361)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void ObjectCreated([In] int Cookie);

  [DispId(50367362)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void ObjectDestroyed([In] int Cookie);

  [DispId(50367363)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void ObjectAddRefd([In] int Cookie, [In] int ToReferenceCount);

  [DispId(50367364)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void ObjectReleased([In] int Cookie, [In] int ToReferenceCount);

  [DispId(50367365)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void ObjectQueryInterfaced([In] int Cookie, [MarshalAs(UnmanagedType.BStr), In] string InterfaceIID, [In] bool Successful);

  [DispId(50367366)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void OnMemberInvoke([In] int Cookie, [MarshalAs(UnmanagedType.BStr), In] string MemberName, [In] EventTimingEnum BeforeOrAfter, [In] int Result);
}
