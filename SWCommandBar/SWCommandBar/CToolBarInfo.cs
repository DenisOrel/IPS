// Decompiled with JetBrains decompiler
// Type: SWCommandBar.CToolBarInfo
// Assembly: SWCommandBar, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3a41272d57eb390d
// MVID: 36416389-E9A4-4D69-A4C2-00DF30178B13
// Assembly location: D:\Projects\IPS Code\IPS\CADSystem\CAD\SolidWorks\Bin\SWCommandBar.dll

using SolidWorks.Interop.sldworks;
using System.Runtime.InteropServices;

#nullable disable
namespace SWCommandBar;

[ComVisible(true)]
[Guid("A3C0A4A4-D5DE-4481-A7CD-764933EA56B9")]
public class CToolBarInfo : IToolbarInfo
{
  private ICommandGroup m_pCmdGroup;
  private CCommandBarAddin m_pSwAddin;
  private int m_iDocType;
  private int m_iGroupID;
  private int m_iCookie;
  private bool m_bNewToolbar;

  public CToolBarInfo(
    ICommandGroup pCmdGroup,
    int iDocType,
    CCommandBarAddin pSwAddin,
    int iCookie,
    int iGroupID,
    bool bNewToolbar)
  {
    this.m_pCmdGroup = pCmdGroup;
    this.m_pSwAddin = pSwAddin;
    this.m_iDocType = iDocType;
    this.m_iGroupID = iGroupID;
    this.m_iCookie = iCookie;
    this.m_bNewToolbar = bNewToolbar;
  }

  public void SetImages(
    string strSmallIconList,
    string strLargeIconList,
    string strSmallMainIcon,
    string strLargeMainIcon)
  {
    if (this.m_pCmdGroup == null)
      return;
    this.m_pCmdGroup.SmallIconList = strSmallIconList;
    this.m_pCmdGroup.LargeIconList = strLargeIconList;
    this.m_pCmdGroup.SmallMainIcon = strSmallMainIcon;
    this.m_pCmdGroup.LargeMainIcon = strLargeMainIcon;
  }

  public bool AddItem(string strName, string strHint, string strToolTip, int iImage, int iCmd)
  {
    if (this.m_pCmdGroup == null)
      return false;
    string CallbackFunction = $"OnCommand({this.m_iCookie.ToString()},{iCmd.ToString()})";
    string EnableMethod = $"OnEnable({this.m_iCookie.ToString()},{iCmd.ToString()},{this.m_iDocType.ToString()})";
    // ISSUE: reference to a compiler-generated method
    this.m_pCmdGroup.AddCommandItem2(strName, -1, strHint, strToolTip, iImage, CallbackFunction, EnableMethod, 0, 2);
    return true;
  }

  public bool Activate() => this.m_pSwAddin != null && this.m_pSwAddin.ActivateToolBar(this);

  public bool Remove() => this.m_pSwAddin != null && this.m_pSwAddin.RemoveToolBar(this);

  public int GetDocType() => this.m_iDocType;

  public ICommandGroup GetCommandGroup() => this.m_pCmdGroup;

  public int GetGroupID() => this.m_iGroupID;

  public bool IsNewToolbar() => this.m_bNewToolbar;
}
