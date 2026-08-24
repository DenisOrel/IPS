// Decompiled with JetBrains decompiler
// Type: InventorApprentice.DesignProjectManager
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(4096 /*0x1000*/)]
[InterfaceType(2)]
[Guid("4A60CB5E-1EE8-4180-A801-194704F3021E")]
[DefaultMember("Type")]
[ComImport]
public interface DesignProjectManager
{
  [DispId(0)]
  ObjectTypeEnum Type { [DispId(0), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(2130706433 /*0x7F000001*/)]
  object Application { [DispId(2130706433 /*0x7F000001*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IDispatch)] get; }

  [DispId(50401025)]
  DesignProjects DesignProjects { [DispId(50401025), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(50401026)]
  DesignProject ActiveDesignProject { [DispId(50401026), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(2130706434 /*0x7F000002*/)]
  object Parent { [DispId(2130706434 /*0x7F000002*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IDispatch)] get; }

  [DispId(50401027)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  bool IsFileInActiveProject(
    [MarshalAs(UnmanagedType.BStr), In] string FileName,
    out LocationTypeEnum ProjectPathType,
    [MarshalAs(UnmanagedType.BStr)] out string ProjectPathName);

  [DispId(50401028)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.BStr)]
  string ResolveFile([MarshalAs(UnmanagedType.BStr), In] string SourcePath, [MarshalAs(UnmanagedType.BStr), In] string DestinationFileName, [MarshalAs(UnmanagedType.Struct), Optional] object Options);

  [DispId(50401029)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  ProjectOptionsButton AddOptionsButton(
    [MarshalAs(UnmanagedType.BStr), In] string ClientId,
    [MarshalAs(UnmanagedType.BStr), In] string DisplayName,
    [MarshalAs(UnmanagedType.BStr), In] string TooltipText,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object StandardIcon);
}
