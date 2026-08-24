// Decompiled with JetBrains decompiler
// Type: SldWorks.ISldWorks
// Assembly: Intermech.SolidWorks.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C58B767B-0480-4923-A6B5-4C5307770AFD
// Assembly location: D:\IPS\Client\Intermech.SolidWorks.Integrator.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace SldWorks;

[CompilerGenerated]
[Guid("83A33D22-27C5-11CE-BFD4-00400513BB57")]
[TypeIdentifier]
[ComImport]
public interface ISldWorks
{
  [SpecialName]
  [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
  sealed extern void _VtblGap1_43();

  [DispId(42)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.BStr)]
  string GetSearchFolders([In] int FolderType);

  [DispId(43)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  bool SetSearchFolders([In] int FolderType, [MarshalAs(UnmanagedType.BStr), In] string Folders);
}
