// Decompiled with JetBrains decompiler
// Type: InventorApprentice.FileManager
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("B00506C6-BEB7-47F6-8B1B-A5CB5DCD09B3")]
[TypeLibType(4096 /*0x1000*/)]
[InterfaceType(2)]
[DefaultMember("Type")]
[ComImport]
public interface FileManager
{
  [DispId(0)]
  ObjectTypeEnum Type { [DispId(0), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(2130706433 /*0x7F000001*/)]
  object Application { [DispId(2130706433 /*0x7F000001*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IDispatch)] get; }

  [DispId(50378241)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void DeleteFile([MarshalAs(UnmanagedType.BStr), In] string FullFileName, [In] FileManagementEnum FileManagementOption = FileManagementEnum.kNoForceFile);

  [DispId(50378242)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void CopyFile(
    [MarshalAs(UnmanagedType.BStr), In] string SourceFullFileName,
    [MarshalAs(UnmanagedType.BStr), In] string DestinationFullFileName,
    [In] FileManagementEnum FileManagementOption = FileManagementEnum.kNoForceFile);

  [DispId(50378243)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void MoveFile(
    [MarshalAs(UnmanagedType.BStr), In] string SourceFullFileName,
    [MarshalAs(UnmanagedType.BStr), In] string DestinationFullFileName,
    [In] FileManagementEnum FileManagementOption = FileManagementEnum.kNoForceFile);

  [DispId(50378244)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.BStr)]
  string GetTemplateFile(
    [In] DocumentTypeEnum DocumentType,
    [In] SystemOfMeasureEnum SystemOfMeasure = SystemOfMeasureEnum.kDefaultSystemOfMeasure,
    [In] DraftingStandardEnum DraftingStandard = DraftingStandardEnum.kDefault_DraftingStandard,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object DocumentSubType);

  [DispId(50378245)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetIdentifierFromFileName([MarshalAs(UnmanagedType.BStr), In] string FullFileName, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UI1), In, Out] ref byte[] Identifier, [MarshalAs(UnmanagedType.BStr), In] string AbsolutePath = "");

  [DispId(50378246)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetFileNameFromIdentifier(
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UI1), In, Out] ref byte[] Identifier,
    [MarshalAs(UnmanagedType.BStr)] out string FullFileName,
    [MarshalAs(UnmanagedType.BStr), In] string AbsolutePath = "");

  [DispId(50378247)]
  FileManagerEvents FileManagerEvents { [DispId(50378247), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(50378248)]
  FilesEnumerator Files { [DispId(50378248), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(50378249)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.BStr)]
  string GetFullDocumentName([MarshalAs(UnmanagedType.BStr), In] string FullFileName, [MarshalAs(UnmanagedType.BStr), In] string LevelOfDetailName = "");

  [DispId(50378250)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.BStr)]
  string GetLevelOfDetailName([MarshalAs(UnmanagedType.BStr), In] string FullDocumentName);

  [DispId(50378251)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.BStr)]
  string GetFullFileName([MarshalAs(UnmanagedType.BStr), In] string FullDocumentName);

  [DispId(50378252)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)]
  string[] GetDesignViewRepresentations([MarshalAs(UnmanagedType.BStr), In] string FullFileName);

  [DispId(50378253)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)]
  string[] GetPositionalRepresentations([MarshalAs(UnmanagedType.BStr), In] string FullFileName);

  [DispId(50378254)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)]
  string[] GetLevelOfDetailRepresentations([MarshalAs(UnmanagedType.BStr), In] string FullFileName);

  [DispId(50378259)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.BStr)]
  string GetLastActiveLevelOfDetailRepresentation([MarshalAs(UnmanagedType.BStr), In] string FullFileName);

  [DispId(50378255)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  bool IsFileNameValid([MarshalAs(UnmanagedType.BStr), In] string FileName, [MarshalAs(UnmanagedType.BStr)] out string ValidFileName);

  [DispId(50378256)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)]
  string[] GetDWGDocumentReferences([MarshalAs(UnmanagedType.Struct), In] object DocumentOrFileName);

  [DispId(50378257)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  bool IsInventorDWG([MarshalAs(UnmanagedType.BStr), In] string FullFileName);

  [DispId(50378258)]
  object FileSystemObject { [DispId(50378258), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IDispatch)] get; }

  [DispId(50378260)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void RefreshAllDocuments();

  [DispId(50378261)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.BStr)]
  string GetLastActiveDesignViewRepresentation([MarshalAs(UnmanagedType.BStr), In] string FullFileName);

  [DispId(50378262)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  SoftwareVersion SoftwareVersionSaved([MarshalAs(UnmanagedType.BStr), In] string FullFileName);

  [DispId(50378263)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)]
  string[] GetAutoCADBlockDefinitions([MarshalAs(UnmanagedType.BStr), In] string FullFileName);

  [DispId(50378264)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  CachedGraphicsStatusEnum GetExpressGraphicsStatus([MarshalAs(UnmanagedType.BStr), In] string AssemblyFullFilename);

  [DispId(50378265)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int ReferencedDocumentCount([MarshalAs(UnmanagedType.BStr), In] string FullFileName);

  [DispId(50378266)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  bool WillOpenExpressDefault([MarshalAs(UnmanagedType.BStr), In] string FullFileName);
}
