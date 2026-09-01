// Decompiled with JetBrains decompiler
// Type: IPSAddIn.Installer.PlatformVersions
// Assembly: IPSAddIn.Installer, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: 0B42B756-5F54-4959-820D-851B2C3E0C84
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn.Installer.exe

#nullable disable
namespace IPSAddIn.Installer;

internal class PlatformVersions
{
  public string PlatformName { get; private set; }

  public string PlatformVersion { get; private set; }

  public PlatformVersions(string platformName, string platformVersion)
  {
    this.PlatformName = platformName;
    this.PlatformVersion = platformVersion;
  }
}
