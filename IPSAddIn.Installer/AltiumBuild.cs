// Decompiled with JetBrains decompiler
// Type: IPSAddIn.Installer.AltiumBuild
// Assembly: IPSAddIn.Installer, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: 0B42B756-5F54-4959-820D-851B2C3E0C84
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn.Installer.exe

#nullable disable
namespace IPSAddIn.Installer;

internal sealed class AltiumBuild
{
  public string Application { get; private set; }

  public string UniqueID { get; private set; }

  public string Version { get; private set; }

  public AltiumBuild(string application, string uniqueID, string version)
  {
    this.Application = application;
    this.UniqueID = uniqueID;
    this.Version = version;
  }
}
