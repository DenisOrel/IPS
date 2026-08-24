// Decompiled with JetBrains decompiler
// Type: Microsoft.Office.Interop.Word.Documents
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.Office.Interop.Word;

[CompilerGenerated]
[DefaultMember("Item")]
[Guid("0002096C-0000-0000-C000-000000000046")]
[TypeIdentifier]
[ComImport]
public interface Documents : IEnumerable
{
  [SpecialName]
  [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
  sealed extern void _VtblGap1_15();

  [DispId(19)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Document Open(
    [MarshalAs(UnmanagedType.Struct), In] ref object FileName,
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object ConfirmConversions,
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object ReadOnly,
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object AddToRecentFiles,
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object PasswordDocument,
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object PasswordTemplate,
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object Revert,
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object WritePasswordDocument,
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object WritePasswordTemplate,
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object Format,
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object Encoding,
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object Visible,
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object OpenAndRepair,
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object DocumentDirection,
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object NoEncodingDialog,
    [MarshalAs(UnmanagedType.Struct), In, Optional] ref object XMLTransform);
}
