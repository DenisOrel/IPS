// Decompiled with JetBrains decompiler
// Type: IPSAddIn.Installer.RegistryReader
// Assembly: IPSAddIn.Installer, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: 0B42B756-5F54-4959-820D-851B2C3E0C84
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn.Installer.exe

using Microsoft.Win32;
using System;
using System.Collections.Generic;

#nullable disable
namespace IPSAddIn.Installer;

internal class RegistryReader
{
  public static List<AltiumBuild> AltiumBuilds
  {
    get
    {
      RegistryKey altiumDesignerRootKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Altium\\Builds") ?? throw new Exception("Не найден установленный Altium Designer.");
      try
      {
        string[] listBuilds = RegistryReader.GetListBuilds(altiumDesignerRootKey);
        BuildsFactory factory = new BuildsFactory();
        List<AltiumBuild> altiumBuilds = new List<AltiumBuild>();
        foreach (string buildName in listBuilds)
          altiumBuilds.Add(RegistryReader.ReadBuildKey(altiumDesignerRootKey, buildName, factory));
        return altiumBuilds;
      }
      finally
      {
        altiumDesignerRootKey.Close();
      }
    }
  }

  private static AltiumBuild ReadBuildKey(
    RegistryKey altiumDesignerRootKey,
    string buildName,
    BuildsFactory factory)
  {
    RegistryKey buildKey = altiumDesignerRootKey.OpenSubKey(buildName);
    try
    {
      return factory.Create(buildKey);
    }
    finally
    {
      buildKey.Close();
    }
  }

  private static string[] GetListBuilds(RegistryKey altiumDesignerRootKey)
  {
    string[] subKeyNames = altiumDesignerRootKey.GetSubKeyNames();
    return subKeyNames != null && subKeyNames.Length != 0 ? subKeyNames : throw new Exception("Не найдены версии установленного Altium Designer.");
  }
}
