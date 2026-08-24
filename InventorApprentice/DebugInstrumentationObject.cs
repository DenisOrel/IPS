// Decompiled with JetBrains decompiler
// Type: InventorApprentice.DebugInstrumentationObject
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[InterfaceType(2)]
[Guid("F6F33557-6984-11D5-8DF3-0010B541CAA8")]
[TypeLibType(4112)]
[ComImport]
public interface DebugInstrumentationObject
{
  [DispId(50367233)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetLiveObjects([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_I4), In, Out] ref int[] Cookies);

  [DispId(50367234)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.BStr)]
  string get_ObjectDescription([In] int Cookie);

  [DispId(50367235)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.IUnknown)]
  object get_Object([In] int Cookie);

  [DispId(50367236)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int GetObjectCookie([MarshalAs(UnmanagedType.IUnknown), In] object Object);

  [DispId(50367237)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int get_ObjectReferenceCount([In] int Cookie);

  [DispId(50367238)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int get_ObjectInstanceNumber([In] int Cookie);

  [DispId(50367239)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  DebugWatchType get_ObjectWatchType([In] int Cookie);

  [DispId(50367239)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void set_ObjectWatchType([In] int Cookie, [In] DebugWatchType _param2);

  [DispId(50367240)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetTrace([In] bool Enable = true, [MarshalAs(UnmanagedType.BStr), In] string TraceFilename = "");

  [DispId(50367241)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetTraceInfo(out bool Enabled, [MarshalAs(UnmanagedType.BStr)] out string TraceFilename);

  [DispId(50367242)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetProfileInfo([In] bool Enable, [In] bool WriteToFileOnStop = true, [MarshalAs(UnmanagedType.BStr), In] string FileName = "");
}
