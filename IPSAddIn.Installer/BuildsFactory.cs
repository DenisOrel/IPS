// Decompiled with JetBrains decompiler
// Type: IPSAddIn.Installer.BuildsFactory
// Assembly: IPSAddIn.Installer, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: 0B42B756-5F54-4959-820D-851B2C3E0C84
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn.Installer.exe

using Microsoft.Win32;
using System;

#nullable disable
namespace IPSAddIn.Installer;

internal sealed class BuildsFactory
{
  public AltiumBuild Create(RegistryKey buildKey)
  {
    return new AltiumBuild(this.ReadValue(buildKey, "Application"), this.ReadValue(buildKey, "UniqueID"), this.ReadValue(buildKey, "Version"));
  }

  private string ReadValue(RegistryKey buildKey, string parameterName)
  {
    return Convert.ToString(buildKey.GetValue(parameterName) ?? throw new Exception($"Ключ реестра {buildKey.Name} не содержит параметра {parameterName}."));
  }
}
