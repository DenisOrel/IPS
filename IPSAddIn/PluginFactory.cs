// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.PluginFactory
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using DXP;
using Intermech.AltiumDesigner.Interfaces;
using Intermech.Win32;
using Microsoft.Win32;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#nullable disable
namespace CSharpPlugin;

[ClassInterface(ClassInterfaceType.None)]
public class PluginFactory : IPluginFactory
{
  public object InvokePluginFactory(IClient a)
  {
    this.CheckPathInRegistry();
    FileInfo fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
    return (object) new PluginServerModule(a, Consts.ModuleName, fileInfo.DirectoryName);
  }

  private void CheckPathInRegistry()
  {
    RegistryKey exePathRegistryKey = RegistryHelper.GetAppExePathRegistryKey(AltiumConsts.ApplicationName, true);
    string str = (string) exePathRegistryKey.GetValue(string.Empty);
    if (!(str == string.Empty) && !(str != Application.ExecutablePath))
      return;
    exePathRegistryKey.SetValue(string.Empty, (object) Application.ExecutablePath);
  }
}
