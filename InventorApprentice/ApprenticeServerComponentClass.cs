// Decompiled with JetBrains decompiler
// Type: InventorApprentice.ApprenticeServerComponentClass
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[ClassInterface(0)]
[Guid("C343ED84-A129-11D3-B799-0060B0F159EF")]
[TypeLibType(2)]
[ComImport]
public class ApprenticeServerComponentClass : 
  ApprenticeServer,
  ApprenticeServerComponent,
  IRxApprenticeServer,
  IPersistFile
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public extern ApprenticeServerComponentClass();

  [DispId(2130706435 /*0x7F000003*/)]
  public virtual extern ObjectTypeEnum Type { [DispId(2130706435 /*0x7F000003*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(50341122)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  public virtual extern ApprenticeServerDocument Open([MarshalAs(UnmanagedType.BStr), In] string FullDocumentName);

  [DispId(50341158)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  public virtual extern ApprenticeServerDocument OpenWithOptions(
    [MarshalAs(UnmanagedType.BStr), In] string FullDocumentName,
    [MarshalAs(UnmanagedType.Interface), In] NameValueMap Options);

  [DispId(50341123)]
  public virtual extern ApprenticeServerDocument Document { [DispId(50341123), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(50341124)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void Close();

  [DispId(50341125)]
  public virtual extern FileLocations FileLocations { [DispId(50341125), TypeLibFunc(64 /*0x40*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(50341126)]
  public virtual extern FileSaveAs FileSaveAs { [DispId(50341126), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(50341131)]
  public virtual extern ApplicationAddIns ApplicationAddIns { [DispId(50341131), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(50341127)]
  public virtual extern bool MultiUsersEnabled { [TypeLibFunc(64 /*0x40*/), DispId(50341127), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [TypeLibFunc(64 /*0x40*/), DispId(50341127), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(50341148)]
  public virtual extern MultiUserModeEnum MultiUserMode { [DispId(50341148), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(50341148), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(50341149)]
  public virtual extern bool MultiUserExternallyManaged { [DispId(50341149), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(50341149), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(50341134)]
  public virtual extern SoftwareVersion SoftwareVersion { [DispId(50341134), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(50341137)]
  public virtual extern string UserName { [DispId(50341137), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(50341137), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.BStr), In] set; }

  [DispId(50341135)]
  public virtual extern TransientGeometry TransientGeometry { [DispId(50341135), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(50341136)]
  public virtual extern bool DisplayAffinity { [DispId(50341136), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(50341136), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(50341147)]
  public virtual extern FileManager FileManager { [DispId(50341147), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(50341151)]
  public virtual extern FileOptions FileOptions { [DispId(50341151), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(50341152)]
  public virtual extern int Locale { [DispId(50341152), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(50341156)]
  public virtual extern DisplayOptions DisplayOptions { [DispId(50341156), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(50341157)]
  public virtual extern HardwareOptions HardwareOptions { [DispId(50341157), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(50341159)]
  public virtual extern string InstallPath { [DispId(50341159), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }

  [DispId(50341163)]
  public virtual extern DesignProjectManager DesignProjectManager { [DispId(50341163), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(50341160)]
  public virtual extern string CurrentUserAppDataPath { [DispId(50341160), TypeLibFunc(64 /*0x40*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }

  [DispId(50341161)]
  public virtual extern string AllUsersAppDataPath { [TypeLibFunc(64 /*0x40*/), DispId(50341161), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }

  [DispId(50341162)]
  public virtual extern TransientObjects TransientObjects { [DispId(50341162), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(50341145)]
  public virtual extern FileAccessEvents FileAccessEvents { [TypeLibFunc(64 /*0x40*/), DispId(50341145), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(50341164)]
  public virtual extern ReferenceKeyEvents ReferenceKeyEvents { [DispId(50341164), TypeLibFunc(64 /*0x40*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [TypeLibFunc(64 /*0x40*/)]
  [DispId(50341138)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern bool get__IsRegistryEntry(
    [MarshalAs(UnmanagedType.BStr), In] string SubKey,
    [MarshalAs(UnmanagedType.BStr), In] string ValueName,
    [In] _RegistryHiveTypeEnum RegistryHive);

  [TypeLibFunc(64 /*0x40*/)]
  [DispId(50341139)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Struct)]
  public virtual extern object get__RegistryEntry(
    [MarshalAs(UnmanagedType.BStr), In] string SubKey,
    [MarshalAs(UnmanagedType.BStr), In] string ValueName,
    [In] _RegistryHiveTypeEnum RegistryHive);

  [DispId(50341140)]
  [TypeLibFunc(64 /*0x40*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void _SetRegistryEntry(
    [MarshalAs(UnmanagedType.BStr), In] string SubKey,
    [MarshalAs(UnmanagedType.BStr), In] string ValueName,
    [MarshalAs(UnmanagedType.Struct), In] object Value,
    [In] _RegistryHiveTypeEnum RegistryHive,
    [In] bool RefreshWithEntry);

  [DispId(50341141)]
  [TypeLibFunc(64 /*0x40*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void _DeleteRegistryEntry(
    [MarshalAs(UnmanagedType.BStr), In] string SubKey,
    [MarshalAs(UnmanagedType.BStr), In] string ValueName,
    [In] _RegistryHiveTypeEnum RegistryHive);

  [TypeLibFunc(64 /*0x40*/)]
  [DispId(50341143)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void _DisplayHelpTopic([MarshalAs(UnmanagedType.BStr), In] string FileName, [MarshalAs(UnmanagedType.BStr), In] string TopicName);

  [DispId(50341144)]
  [TypeLibFunc(64 /*0x40*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void _DisplayHelpContext([MarshalAs(UnmanagedType.BStr), In] string FileName, [In] int Context);

  [DispId(50341142)]
  public virtual extern DebugInstrumentation _DebugInstrumentation { [TypeLibFunc(64 /*0x40*/), DispId(50341142), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(50341146)]
  public virtual extern _AppPerformanceMonitor _AppPerformanceMonitor { [DispId(50341146), TypeLibFunc(64 /*0x40*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(50341155)]
  public virtual extern bool IndirectReferences { [DispId(50341155), TypeLibFunc(64 /*0x40*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [TypeLibFunc(64 /*0x40*/), DispId(50341155), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(50341150)]
  public virtual extern TestManager TestManager { [DispId(50341150), TypeLibFunc(64 /*0x40*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [TypeLibFunc(64 /*0x40*/)]
  [DispId(50341153)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern bool _GetStylesLibraryLockStatus([MarshalAs(UnmanagedType.BStr), In] string StylesLibraryLocation);

  [DispId(50341154)]
  [TypeLibFunc(64 /*0x40*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void _SetStylesLibraryLockStatus([MarshalAs(UnmanagedType.BStr), In] string FileName, [In] bool bLock);

  [DispId(50341165)]
  public virtual extern HelpManager HelpManager { [DispId(50341165), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  public virtual extern IRxComponentDocument IRxApprenticeServer_Open([MarshalAs(UnmanagedType.BStr), In] string FullFileName);

  public virtual extern IRxComponentDocument IRxApprenticeServer_Document { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void IRxApprenticeServer_Close();

  public virtual extern IRxFileAndReferences FileAndReferences { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  public virtual extern IRxFileLocations IRxApprenticeServer_FileLocations { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  public virtual extern IRxFileSaveAs IRxApprenticeServer_FileSaveAs { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  public virtual extern sbyte IRxApprenticeServer_MultiUsersEnabled { [TypeLibFunc(64 /*0x40*/), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [TypeLibFunc(64 /*0x40*/), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [TypeLibFunc(64 /*0x40*/)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void _MinimizeFileSize([MarshalAs(UnmanagedType.BStr), In] string FullFileName, [In] int NumVersionsToKeep);

  public virtual extern int _NumberOfVersionsKept { [TypeLibFunc(64 /*0x40*/), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [TypeLibFunc(64 /*0x40*/), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [TypeLibFunc(64 /*0x40*/)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void _FileAlreadyOpen(
    [MarshalAs(UnmanagedType.BStr), In] string FullFileName,
    out sbyte pbAlreadyOpen,
    out sbyte pbOpenInThisProcess);

  public virtual extern int _MajorVersion { [TypeLibFunc(64 /*0x40*/), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  public virtual extern int _MinorVersion { [TypeLibFunc(64 /*0x40*/), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  public virtual extern IRxTransientGeometry IRxApprenticeServer_TransientGeometry { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void GetClassID(out Guid pClassID);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void IsDirty();

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void Load([MarshalAs(UnmanagedType.LPWStr), In] string pszFileName, [In] uint dwMode);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void Save([MarshalAs(UnmanagedType.LPWStr), In] string pszFileName, [In] int fRemember);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void SaveCompleted([MarshalAs(UnmanagedType.LPWStr), In] string pszFileName);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void GetCurFile([MarshalAs(UnmanagedType.LPWStr)] out string ppszFileName);
}
