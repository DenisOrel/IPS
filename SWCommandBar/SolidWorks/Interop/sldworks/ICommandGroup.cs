// Decompiled with JetBrains decompiler
// Type: SolidWorks.Interop.sldworks.ICommandGroup
// Assembly: SWCommandBar, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3a41272d57eb390d
// MVID: 36416389-E9A4-4D69-A4C2-00DF30178B13
// Assembly location: D:\Projects\IPS Code\IPS\CADSystem\CAD\SolidWorks\Bin\SWCommandBar.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace SolidWorks.Interop.sldworks;

[CompilerGenerated]
[Guid("FF545450-B559-400D-964C-A3811F209148")]
[TypeIdentifier]
[ComImport]
public interface ICommandGroup
{
  [SpecialName]
  sealed extern void _VtblGap1_2();

  int NumberOfGroupItems { [DispId(3)] get; }

  [DispId(4)]
  bool Activate();

  string LargeMainIcon { [DispId(5)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(5)] [param: MarshalAs(UnmanagedType.BStr), In] set; }

  string SmallMainIcon { [DispId(6)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(6)] [param: MarshalAs(UnmanagedType.BStr), In] set; }

  string LargeIconList { [DispId(7)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(7)] [param: MarshalAs(UnmanagedType.BStr), In] set; }

  string SmallIconList { [DispId(8)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(8)] [param: MarshalAs(UnmanagedType.BStr), In] set; }

  [SpecialName]
  sealed extern void _VtblGap2_7();

  bool HasToolbar { [DispId(13)] get; [DispId(13)] [param: In] set; }

  bool HasMenu { [DispId(14)] get; [DispId(14)] [param: In] set; }

  string Name { [DispId(15)] [return: MarshalAs(UnmanagedType.BStr)] get; }

  [SpecialName]
  sealed extern void _VtblGap3_3();

  [DispId(18)]
  int AddCommandItem2(
    [MarshalAs(UnmanagedType.BStr), In] string Name,
    [In] int Position,
    [MarshalAs(UnmanagedType.BStr), In] string HintString,
    [MarshalAs(UnmanagedType.BStr), In] string ToolTip,
    [In] int ImageListIndex,
    [MarshalAs(UnmanagedType.BStr), In] string CallbackFunction,
    [MarshalAs(UnmanagedType.BStr), In] string EnableMethod,
    [In] int UserID,
    [In] int MenuTBOption);

  [SpecialName]
  sealed extern void _VtblGap4_1();

  [DispId(20)]
  int get_CommandID([In] int CommandIndex);
}
