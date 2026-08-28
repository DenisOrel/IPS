// Decompiled with JetBrains decompiler
// Type: Intermech.Vault.Service.Program
// Assembly: Intermech.Vault.Service, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 24B0DA67-997A-4E40-A745-DAEE647016D5
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Vault.Service.exe

using System.ServiceProcess;

#nullable disable
namespace Intermech.Vault.Service;

internal static class Program
{
  private static void Main(string[] args)
  {
    ServiceBase.Run(new ServiceBase[1]
    {
      (ServiceBase) new Intermech_Vault_Service()
    });
  }
}
