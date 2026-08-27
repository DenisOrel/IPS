// Decompiled with JetBrains decompiler
// Type: SWCommandBar.ICommandBarCallback
// Assembly: SWCommandBar, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3a41272d57eb390d
// MVID: 36416389-E9A4-4D69-A4C2-00DF30178B13
// Assembly location: D:\Projects\IPS Code\IPS\CADSystem\CAD\SolidWorks\Bin\SWCommandBar.dll

using System.Runtime.InteropServices;

#nullable disable
namespace SWCommandBar;

[Guid("10FD7D12-F9E1-45fe-9D99-FA4837525493")]
[ComVisible(true)]
public interface ICommandBarCallback
{
  bool OnCommandEnable(int iCmd);

  void OnCommand(int iCmd);
}
