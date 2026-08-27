// Decompiled with JetBrains decompiler
// Type: SWCommandBar.IToolbarInfo
// Assembly: SWCommandBar, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3a41272d57eb390d
// MVID: 36416389-E9A4-4D69-A4C2-00DF30178B13
// Assembly location: D:\Projects\IPS Code\IPS\CADSystem\CAD\SolidWorks\Bin\SWCommandBar.dll

using System.Runtime.InteropServices;

#nullable disable
namespace SWCommandBar;

[ComVisible(true)]
[Guid("29B76EB2-F6D9-4357-AC49-08A5DBF7E642")]
public interface IToolbarInfo
{
  void SetImages(
    string strSmallIconList,
    string strLargeIconList,
    string strSmallMainIcon,
    string strLargeMainIcon);

  bool AddItem(string strName, string strHint, string strToolTip, int iImage, int iCmd);

  bool Activate();

  bool Remove();
}
