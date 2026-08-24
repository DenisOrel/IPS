// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxFileLocations
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(16 /*0x10*/)]
[ComConversionLoss]
[Guid("42C7E0BF-FDCF-11D2-B785-0060B0F159EF")]
[InterfaceType(1)]
[ComImport]
public interface IRxFileLocations
{
  [DispId(50339713)]
  string Workspace { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.BStr), In] set; }

  [DispId(50339714)]
  sbyte _WorkspaceActive { [TypeLibFunc(64 /*0x40*/), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [TypeLibFunc(64 /*0x40*/), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Locals([In, Out] ref int pnPaths, [Out] IntPtr pppNames, [Out] IntPtr ppbstrPaths);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void RemoveLocal([MarshalAs(UnmanagedType.BStr), In] string bstrName);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void AddLocal([MarshalAs(UnmanagedType.BStr), In] string bstrName, [MarshalAs(UnmanagedType.BStr), In] string bstrPath, [In] int nIndex);

  [DispId(50339718)]
  sbyte _LocalsActive { [TypeLibFunc(64 /*0x40*/), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [TypeLibFunc(64 /*0x40*/), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Workgroups([In, Out] ref int pnPaths, [Out] IntPtr ppbstrNames, [Out] IntPtr ppbstrPaths);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void RemoveWorkgroup([MarshalAs(UnmanagedType.BStr), In] string bstrName);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void AddWorkgroup([MarshalAs(UnmanagedType.BStr), In] string bstrName, [MarshalAs(UnmanagedType.BStr), In] string bstrPath, [In] int nIndex);

  [DispId(50339722)]
  sbyte _WorkgroupsActive { [TypeLibFunc(64 /*0x40*/), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [TypeLibFunc(64 /*0x40*/), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Libraries([In, Out] ref int pnPaths, [Out] IntPtr ppbstrNames, [Out] IntPtr ppbstrPaths);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void RemoveLibrary([MarshalAs(UnmanagedType.BStr), In] string bstrName);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void AddLibrary([MarshalAs(UnmanagedType.BStr), In] string bstrName, [MarshalAs(UnmanagedType.BStr), In] string bstrPath);

  [DispId(50339726)]
  string FileLocationsFilesDir { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.BStr), In] set; }

  [DispId(50339727)]
  string FileLocationsFile { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.BStr), In] set; }

  [DispId(50339728)]
  sbyte _CurrentSettingsDirty { [TypeLibFunc(64 /*0x40*/), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [TypeLibFunc(64 /*0x40*/)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void _ApplyCurrentSettings();

  [DispId(50339730)]
  sbyte _Dirty { [TypeLibFunc(64 /*0x40*/), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [TypeLibFunc(64 /*0x40*/)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void _Save();

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void _SaveAs([MarshalAs(UnmanagedType.BStr), In] string bstrFile);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void FindInLocations(
    [MarshalAs(UnmanagedType.BStr), In] string bstrFullFileName,
    [MarshalAs(UnmanagedType.BStr)] out string pbstrRepositoryName,
    out LocationTypeEnum pType);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void FindLogicalInLocations(
    [MarshalAs(UnmanagedType.BStr), In] string bstrRelativeFileName,
    [MarshalAs(UnmanagedType.BStr), In] string bstrLibraryName,
    [MarshalAs(UnmanagedType.BStr)] out string pbstrRepositoryName,
    out LocationTypeEnum pType);
}
