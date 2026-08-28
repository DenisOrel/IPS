// Decompiled with JetBrains decompiler
// Type: Intermech.Sales.Server.SalesServerStartup
// Assembly: Intermech.Sales.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E52E0A3-6B62-4A54-B7BB-4D37052E1EF5
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Sales.Server.dll

using Intermech.Interfaces.Plugins;
using Intermech.Localization;
using System;

#nullable disable
namespace Intermech.Sales.Server;

public class SalesServerStartup : IPackage
{
  public void Load(IServiceProvider serviceProvider)
  {
  }

  public void Unload()
  {
  }

  public string Name => LocalizationHolder.rm.GetString("Sales.Server_1");
}
