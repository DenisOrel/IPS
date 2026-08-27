// Decompiled with JetBrains decompiler
// Type: SolidWorks.Interop.sldworks.ICommandTab
// Assembly: SWCommandBar, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3a41272d57eb390d
// MVID: 36416389-E9A4-4D69-A4C2-00DF30178B13
// Assembly location: D:\Projects\IPS Code\IPS\CADSystem\CAD\SolidWorks\Bin\SWCommandBar.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace SolidWorks.Interop.sldworks;

[TypeIdentifier]
[Guid("FC248E07-607D-4429-960B-E4CE20AB55AB")]
[CompilerGenerated]
[ComImport]
public interface ICommandTab
{
  [SpecialName]
  sealed extern void _VtblGap1_3();

  [DispId(4)]
  [return: MarshalAs(UnmanagedType.Interface)]
  CommandTabBox AddCommandTabBox();
}
