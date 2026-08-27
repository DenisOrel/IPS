// Decompiled with JetBrains decompiler
// Type: SolidWorks.Interop.swpublished.ISwAddin
// Assembly: SWCommandBar, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3a41272d57eb390d
// MVID: 36416389-E9A4-4D69-A4C2-00DF30178B13
// Assembly location: D:\Projects\IPS Code\IPS\CADSystem\CAD\SolidWorks\Bin\SWCommandBar.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace SolidWorks.Interop.swpublished;

[TypeIdentifier]
[Guid("DA306A0D-EAC5-4406-8610-B1DA805D9270")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[CompilerGenerated]
[ComImport]
public interface ISwAddin
{
  bool ConnectToSW([MarshalAs(UnmanagedType.IDispatch), In] object ThisSW, [In] int Cookie);

  bool DisconnectFromSW();
}
