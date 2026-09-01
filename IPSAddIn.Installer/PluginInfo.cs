// Decompiled with JetBrains decompiler
// Type: IPSAddIn.Installer.PluginInfo
// Assembly: IPSAddIn.Installer, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: 0B42B756-5F54-4959-820D-851B2C3E0C84
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn.Installer.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace IPSAddIn.Installer;

internal class PluginInfo
{
  public string FolderPath { get; private set; }

  public int Status { get; private set; }

  public string CreatedBy { get; private set; }

  public Guid CategoryGuid { get; private set; } = Consts.PluginCategoryGuid;

  public string Title { get; private set; }

  public string Description { get; private set; }

  public DateTime Date { get; private set; }

  public string Version { get; private set; }

  public Guid VersionGuid { get; private set; }

  public Guid Guid { get; private set; } = Consts.PluginGuid;

  public List<IPSAddIn.Installer.PlatformVersions> PlatformVersions { get; set; }

  public static PluginInfo Create(string pluginPath, System.Version ipsVersion)
  {
    string pluginInfoFilePath = PluginInfo.GetPluginInfoFilePath(pluginPath);
    PluginInfo pluginInfo = new PluginInfo()
    {
      FolderPath = pluginPath,
      VersionGuid = Guid.NewGuid(),
      Title = $"IPS {ipsVersion.Major} Integrator AddIn"
    };
    Encoding encoding = Encoding.Default;
    using (StreamReader streamReader = new StreamReader(pluginInfoFilePath, encoding))
    {
      while (streamReader.Peek() > 0)
      {
        string str1 = streamReader.ReadLine();
        if (!string.IsNullOrEmpty(str1))
        {
          if (!PluginInfo.TrimValue(str1).Equals("End"))
          {
            string key;
            string str2;
            if (PluginInfo.ParceLine(str1, out key, out str2))
            {
              switch (key)
              {
                case "Version":
                  pluginInfo.Version = str2;
                  continue;
                case "EditorDescription":
                  pluginInfo.Description = str2;
                  continue;
                case "Date":
                  pluginInfo.Date = Convert.ToDateTime(str2);
                  continue;
                case "Copyright":
                  pluginInfo.CreatedBy = str2;
                  continue;
                default:
                  continue;
              }
            }
          }
          else
            break;
        }
      }
    }
    return pluginInfo;
  }

  private static bool ParceLine(string line, out string key, out string value)
  {
    key = (string) null;
    value = (string) null;
    int num = line.IndexOf('=');
    if (num <= 0 || num == line.Length - 1)
      return false;
    string[] strArray = line.Split('=');
    key = PluginInfo.TrimValue(strArray[0]);
    value = PluginInfo.TrimValue(strArray[1]);
    return true;
  }

  private static string TrimValue(string input) => input.Trim().Trim('\'');

  private static string GetPluginInfoFilePath(string pluginPath)
  {
    string path = Path.Combine(pluginPath, "IPSAddIn.ins");
    return File.Exists(path) ? path : throw new Exception("Отсутствует файл расширения " + path);
  }
}
