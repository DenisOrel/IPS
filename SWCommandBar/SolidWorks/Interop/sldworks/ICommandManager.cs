// Decompiled with JetBrains decompiler
// Type: SolidWorks.Interop.sldworks.ICommandManager
// Assembly: SWCommandBar, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3a41272d57eb390d
// MVID: 36416389-E9A4-4D69-A4C2-00DF30178B13
// Assembly location: D:\Projects\IPS Code\IPS\CADSystem\CAD\SolidWorks\Bin\SWCommandBar.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace SolidWorks.Interop.sldworks;

[TypeIdentifier]
[CompilerGenerated]
[Guid("F61069CF-2E42-4AC4-A517-6A95B79E45EE")]
[ComImport]
public interface ICommandManager
{
  [SpecialName]
  sealed extern void _VtblGap1_10();

  [DispId(11)]
  [return: MarshalAs(UnmanagedType.Interface)]
  CommandTab GetCommandTab([In] int DocumentType, [MarshalAs(UnmanagedType.BStr), In] string TabName);

  [DispId(12)]
  [return: MarshalAs(UnmanagedType.Interface)]
  CommandTab AddCommandTab([In] int DocumentType, [MarshalAs(UnmanagedType.BStr), In] string TabName);

  [DispId(13)]
  bool RemoveCommandTab([MarshalAs(UnmanagedType.Interface), In] CommandTab TabToRemove);

  [DispId(14)]
  [return: MarshalAs(UnmanagedType.Interface)]
  CommandGroup CreateCommandGroup2(
    [In] int UserID,
    [MarshalAs(UnmanagedType.BStr), In] string Title,
    [MarshalAs(UnmanagedType.BStr), In] string ToolTip,
    [MarshalAs(UnmanagedType.BStr), In] string Hint,
    [In] int Position,
    [In] bool IgnorePreviousVersion,
    [In, Out] ref int Errors);

  [DispId(15)]
  int RemoveCommandGroup2([In] int UserID, [In] bool RuntimeOnly);

  [SpecialName]
  sealed extern void _VtblGap2_5();

  [DispId(21)]
  bool GetGroupDataFromRegistry([In] int UserGroupId, [MarshalAs(UnmanagedType.Struct)] out object UserIDs);
}
