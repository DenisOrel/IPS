// Decompiled with JetBrains decompiler
// Type: InventorApprentice.FileAccessEventsSink_OnFileResolutionEventHandler
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(16 /*0x10*/)]
[ComVisible(false)]
public delegate void FileAccessEventsSink_OnFileResolutionEventHandler(
  [MarshalAs(UnmanagedType.BStr), In] string RelativeFileName,
  [MarshalAs(UnmanagedType.BStr), In] string LibraryName,
  [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UI1), In, Out] ref byte[] CustomLogicalName,
  [In] EventTimingEnum BeforeOrAfter,
  [MarshalAs(UnmanagedType.Interface), In] NameValueMap Context,
  [MarshalAs(UnmanagedType.BStr)] out string FullFileName,
  out HandlingCodeEnum HandlingCode);
