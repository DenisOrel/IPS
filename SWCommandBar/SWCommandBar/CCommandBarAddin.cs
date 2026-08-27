// Decompiled with JetBrains decompiler
// Type: SWCommandBar.CCommandBarAddin
// Assembly: SWCommandBar, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3a41272d57eb390d
// MVID: 36416389-E9A4-4D69-A4C2-00DF30178B13
// Assembly location: D:\Projects\IPS Code\IPS\CADSystem\CAD\SolidWorks\Bin\SWCommandBar.dll

using Microsoft.Win32;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using SolidWorksTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

#nullable disable
namespace SWCommandBar;

[Guid("000b1897-c0ef-403b-bbfa-bfe53553f833")]
[SwAddin(Description = "Intermech Command Bar", Title = "Intermech Command Bar Add-In", LoadAtStartup = true)]
[ComVisible(true)]
public class CCommandBarAddin : ICommandBarAddin, ISwAddin
{
  private ISldWorks m_pSldWorks;
  private ICommandManager m_pCmdMgr;
  private ICommandBarCallback[] m_CallbacksArray;
  private static CCommandBarAddin s_pMainInstance;

  [ComRegisterFunction]
  public static void RegisterFunction(Type t)
  {
    SwAddinAttribute swAddinAttribute = (SwAddinAttribute) null;
    foreach (Attribute customAttribute in typeof (CCommandBarAddin).GetCustomAttributes(false))
    {
      if (customAttribute is SwAddinAttribute)
      {
        swAddinAttribute = customAttribute as SwAddinAttribute;
        break;
      }
    }
    RegistryKey localMachine = Registry.LocalMachine;
    RegistryKey currentUser = Registry.CurrentUser;
    string subkey1 = $"SOFTWARE\\SolidWorks\\Addins\\{{{t.GUID.ToString()}}}";
    RegistryKey subKey = localMachine.CreateSubKey(subkey1);
    subKey.SetValue((string) null, (object) 0);
    subKey.SetValue("Description", (object) swAddinAttribute.Description);
    subKey.SetValue("Title", (object) swAddinAttribute.Title);
    string subkey2 = $"Software\\SolidWorks\\AddInsStartup\\{{{t.GUID.ToString()}}}";
    currentUser.CreateSubKey(subkey2).SetValue((string) null, (object) Convert.ToInt32(swAddinAttribute.LoadAtStartup), RegistryValueKind.DWord);
  }

  [ComUnregisterFunction]
  public static void UnregisterFunction(Type t)
  {
    RegistryKey localMachine = Registry.LocalMachine;
    RegistryKey currentUser = Registry.CurrentUser;
    string subkey1 = $"SOFTWARE\\SolidWorks\\Addins\\{{{t.GUID.ToString()}}}";
    localMachine.DeleteSubKey(subkey1);
    string subkey2 = $"Software\\SolidWorks\\AddInsStartup\\{{{t.GUID.ToString()}}}";
    currentUser.DeleteSubKey(subkey2);
  }

  [DllImport("ole32.dll")]
  private static extern int GetRunningObjectTable(int reserved, out IRunningObjectTable prot);

  [DllImport("ole32.dll")]
  private static extern int CreateClassMoniker(Guid rclsid, out IMoniker pMoniker);

  public CCommandBarAddin()
  {
    this.m_pSldWorks = (ISldWorks) null;
    this.m_pCmdMgr = (ICommandManager) null;
    this.m_CallbacksArray = (ICommandBarCallback[]) null;
  }

  public bool ConnectToSW(object ThisSW, int iCookie)
  {
    this.m_pSldWorks = (ISldWorks) ThisSW;
    // ISSUE: reference to a compiler-generated method
    this.m_pSldWorks.SetAddinCallbackInfo(0, (object) this, iCookie);
    // ISSUE: reference to a compiler-generated method
    this.m_pCmdMgr = (ICommandManager) this.m_pSldWorks.GetCommandManager(iCookie);
    CCommandBarAddin.s_pMainInstance = this;
    return true;
  }

  public bool DisconnectFromSW()
  {
    for (int index = 0; index < this.m_CallbacksArray.Length; ++index)
      this.m_CallbacksArray[index] = (ICommandBarCallback) null;
    this.m_pSldWorks = (ISldWorks) null;
    this.m_pCmdMgr = (ICommandManager) null;
    CCommandBarAddin.s_pMainInstance = (CCommandBarAddin) null;
    GC.Collect();
    return true;
  }

  public void GetMainObject(out ICommandBarAddin pMainObj)
  {
    pMainObj = (ICommandBarAddin) CCommandBarAddin.s_pMainInstance;
  }

  public void RegisterCallback(ICommandBarCallback pCallback, out int iCookie)
  {
    if (this.m_CallbacksArray == null)
    {
      this.m_CallbacksArray = new ICommandBarCallback[1];
      this.m_CallbacksArray[0] = pCallback;
    }
    else
    {
      List<ICommandBarCallback> list = ((IEnumerable<ICommandBarCallback>) this.m_CallbacksArray).ToList<ICommandBarCallback>();
      list.Add(pCallback);
      this.m_CallbacksArray = list.ToArray();
    }
    iCookie = this.m_CallbacksArray.Length - 1;
  }

  public void UnregisterCallback(int iCookie)
  {
    if (this.m_CallbacksArray == null || iCookie >= this.m_CallbacksArray.Length)
      return;
    this.m_CallbacksArray[iCookie] = (ICommandBarCallback) null;
  }

  public void AddToolBar(
    int iCookie,
    int iToolbarID,
    string strTitle,
    string strToolTip,
    string strHint,
    int iDocType,
    out IToolbarInfo pToolbarInfo)
  {
    pToolbarInfo = (IToolbarInfo) null;
    if (this.m_pCmdMgr == null)
      return;
    object UserIDs;
    // ISSUE: reference to a compiler-generated method
    this.m_pCmdMgr.GetGroupDataFromRegistry(iToolbarID, out UserIDs);
    Array array = (Array) UserIDs;
    bool bNewToolbar = true;
    if (array != null)
      bNewToolbar = false;
    int Errors = 0;
    // ISSUE: reference to a compiler-generated method
    // ISSUE: variable of a compiler-generated type
    ICommandGroup commandGroup2 = (ICommandGroup) this.m_pCmdMgr.CreateCommandGroup2(iToolbarID, strTitle, strToolTip, strHint, -1, false, ref Errors);
    if (commandGroup2 == null)
      return;
    pToolbarInfo = (IToolbarInfo) new CToolBarInfo(commandGroup2, iDocType, this, iCookie, iToolbarID, bNewToolbar);
  }

  public bool ActivateToolBar(CToolBarInfo pToolBarInfo)
  {
    if (pToolBarInfo == null || this.m_pCmdMgr == null)
      return false;
    // ISSUE: variable of a compiler-generated type
    ICommandGroup commandGroup = pToolBarInfo.GetCommandGroup();
    if (commandGroup == null)
      return false;
    commandGroup.HasToolbar = true;
    commandGroup.HasMenu = false;
    // ISSUE: reference to a compiler-generated method
    if (!commandGroup.Activate())
      return false;
    int docType = pToolBarInfo.GetDocType();
    string name = commandGroup.Name;
    // ISSUE: reference to a compiler-generated method
    // ISSUE: variable of a compiler-generated type
    ICommandTab TabToRemove = (ICommandTab) this.m_pCmdMgr.GetCommandTab(docType, name);
    if (TabToRemove != null && pToolBarInfo.IsNewToolbar())
    {
      // ISSUE: reference to a compiler-generated method
      this.m_pCmdMgr.RemoveCommandTab((CommandTab) TabToRemove);
      TabToRemove = (ICommandTab) null;
    }
    if (TabToRemove != null)
      return true;
    // ISSUE: reference to a compiler-generated method
    // ISSUE: variable of a compiler-generated type
    ICommandTab commandTab = (ICommandTab) this.m_pCmdMgr.AddCommandTab(docType, name);
    if (commandTab == null)
      return false;
    // ISSUE: reference to a compiler-generated method
    // ISSUE: variable of a compiler-generated type
    ICommandTabBox commandTabBox = (ICommandTabBox) commandTab.AddCommandTabBox();
    if (commandTabBox == null)
      return false;
    int numberOfGroupItems = commandGroup.NumberOfGroupItems;
    int[] CommandIDs = new int[numberOfGroupItems];
    int[] TextDisplayStyles = new int[numberOfGroupItems];
    for (int CommandIndex = 0; CommandIndex < numberOfGroupItems; ++CommandIndex)
    {
      // ISSUE: reference to a compiler-generated method
      CommandIDs[CommandIndex] = commandGroup.get_CommandID(CommandIndex);
      TextDisplayStyles[CommandIndex] = 4;
    }
    // ISSUE: reference to a compiler-generated method
    commandTabBox.AddCommands((object) CommandIDs, (object) TextDisplayStyles);
    return true;
  }

  public bool RemoveToolBar(CToolBarInfo pToolBarInfo)
  {
    if (pToolBarInfo == null || this.m_pCmdMgr == null)
      return false;
    // ISSUE: reference to a compiler-generated method
    this.m_pCmdMgr.RemoveCommandGroup2(pToolBarInfo.GetGroupID(), true);
    return true;
  }

  public int OnEnable(string strID)
  {
    int length1 = strID.IndexOf(',');
    int index = int.Parse(strID.Substring(0, length1));
    if (index >= this.m_CallbacksArray.Length)
      return 0;
    ICommandBarCallback callbacks = this.m_CallbacksArray[index];
    if (callbacks == null)
      return 0;
    string str = strID.Substring(length1 + 1);
    int num = 0;
    // ISSUE: variable of a compiler-generated type
    IModelDoc2 iactiveDoc2 = (IModelDoc2) this.m_pSldWorks.IActiveDoc2;
    if (iactiveDoc2 != null)
    {
      // ISSUE: reference to a compiler-generated method
      num = iactiveDoc2.GetType();
    }
    int length2 = str.IndexOf(',');
    if (int.Parse(str.Substring(length2 + 1)) != num)
      return 0;
    string s = str.Substring(0, length2);
    return !callbacks.OnCommandEnable(int.Parse(s)) ? 0 : 1;
  }

  public void OnCommand(string strID)
  {
    int length = strID.IndexOf(',');
    int index = int.Parse(strID.Substring(0, length));
    if (index >= this.m_CallbacksArray.Length)
      return;
    ICommandBarCallback callbacks = this.m_CallbacksArray[index];
    if (callbacks == null)
      return;
    string s = strID.Substring(length + 1);
    callbacks.OnCommand(int.Parse(s));
  }
}
