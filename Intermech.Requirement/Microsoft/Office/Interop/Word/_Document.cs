// Decompiled with JetBrains decompiler
// Type: Microsoft.Office.Interop.Word._Document
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.Office.Interop.Word;

[CompilerGenerated]
[Guid("0002096B-0000-0000-C000-000000000046")]
[DefaultMember("Name")]
[TypeIdentifier]
[ComImport]
public interface _Document
{
  [DispId(0)]
  string Name { [DispId(0), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }

  [SpecialName]
  [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
  sealed extern void _VtblGap1_21();

  [DispId(16 /*0x10*/)]
  Paragraphs Paragraphs { [DispId(16 /*0x10*/), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [SpecialName]
  [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
  sealed extern void _VtblGap2_137();

  [DispId(1105)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Close([MarshalAs(UnmanagedType.Struct), In, Optional] ref object SaveChanges, [MarshalAs(UnmanagedType.Struct), In, Optional] ref object OriginalFormat, [MarshalAs(UnmanagedType.Struct), In, Optional] ref object RouteDocument);

  [SpecialName]
  [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
  sealed extern void _VtblGap3_129();

  [DispId(376)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SaveAs(
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object FileName,
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object FileFormat,
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object LockComments,
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object Password,
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object AddToRecentFiles,
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object WritePassword,
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object ReadOnlyRecommended,
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object EmbedTrueTypeFonts,
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object SaveNativePictureFormat,
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object SaveFormsData,
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object SaveAsAOCELetter,
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object Encoding,
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object InsertLineBreaks,
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object AllowSubstitutions,
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object LineEnding,
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object AddBiDiMarks);
}
