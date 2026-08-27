// Decompiled with JetBrains decompiler
// Type: SolidWorks.Interop.sldworks.ICommandTabBox
// Assembly: SWCommandBar, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3a41272d57eb390d
// MVID: 36416389-E9A4-4D69-A4C2-00DF30178B13
// Assembly location: D:\Projects\IPS Code\IPS\CADSystem\CAD\SolidWorks\Bin\SWCommandBar.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace SolidWorks.Interop.sldworks;

[TypeIdentifier]
[Guid("1DBAAB20-9626-4CB8-A275-C346AC425362")]
[CompilerGenerated]
[ComImport]
public interface ICommandTabBox
{
  [DispId(1)]
  bool AddCommands([MarshalAs(UnmanagedType.Struct), In] object CommandIDs, [MarshalAs(UnmanagedType.Struct), In] object TextDisplayStyles);
}
