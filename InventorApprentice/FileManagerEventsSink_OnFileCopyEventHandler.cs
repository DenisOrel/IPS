// Decompiled with JetBrains decompiler
// Type: InventorApprentice.FileManagerEventsSink_OnFileCopyEventHandler
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(16 /*0x10*/)]
[ComVisible(false)]
public delegate void FileManagerEventsSink_OnFileCopyEventHandler(
  [MarshalAs(UnmanagedType.BStr), In] string SourceFullFileName,
  [MarshalAs(UnmanagedType.BStr), In] string DestinationFullFileName,
  [In] bool Copy,
  [MarshalAs(UnmanagedType.Interface), In] NameValueMap Context,
  out HandlingCodeEnum HandlingCode);
