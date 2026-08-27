// Decompiled with JetBrains decompiler
// Type: SolidWorks.Interop.sldworks.ISldWorks
// Assembly: SWCommandBar, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3a41272d57eb390d
// MVID: 36416389-E9A4-4D69-A4C2-00DF30178B13
// Assembly location: D:\Projects\IPS Code\IPS\CADSystem\CAD\SolidWorks\Bin\SWCommandBar.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace SolidWorks.Interop.sldworks;

[TypeIdentifier]
[CompilerGenerated]
[Guid("83A33D22-27C5-11CE-BFD4-00400513BB57")]
[ComImport]
public interface ISldWorks
{
  [SpecialName]
  sealed extern void _VtblGap1_148();

  [DispId(146)]
  bool SetAddinCallbackInfo([In] int ModuleHandle, [MarshalAs(UnmanagedType.IDispatch), In] object AddinCallbacks, [In] int Cookie);

  [SpecialName]
  sealed extern void _VtblGap2_9();

  ModelDoc2 IActiveDoc2 { [DispId(156)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [SpecialName]
  sealed extern void _VtblGap3_65();

  [DispId(220)]
  [return: MarshalAs(UnmanagedType.Interface)]
  CommandManager GetCommandManager([In] int Cookie);
}
