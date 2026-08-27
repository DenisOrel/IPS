// Decompiled with JetBrains decompiler
// Type: SWCommandBar.ICommandBarAddin
// Assembly: SWCommandBar, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3a41272d57eb390d
// MVID: 36416389-E9A4-4D69-A4C2-00DF30178B13
// Assembly location: D:\Projects\IPS Code\IPS\CADSystem\CAD\SolidWorks\Bin\SWCommandBar.dll

using SolidWorks.Interop.swpublished;
using System.Runtime.InteropServices;

#nullable disable
namespace SWCommandBar;

[ComVisible(true)]
[Guid("EE983795-2DB7-4d71-B80B-9318E626ECED")]
public interface ICommandBarAddin : ISwAddin
{
  void GetMainObject(out ICommandBarAddin pMainObj);

  void RegisterCallback(ICommandBarCallback pCallback, out int iCookie);

  void AddToolBar(
    int iCookie,
    int iToolbarID,
    string strTitle,
    string strToolTip,
    string strHint,
    int iDocType,
    out IToolbarInfo pToolbarInfo);

  void UnregisterCallback(int iCookie);
}
