// Decompiled with JetBrains decompiler
// Type: Intermech.Vault.Service.ServiceHandler
// Assembly: Intermech.Vault.Service, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 24B0DA67-997A-4E40-A745-DAEE647016D5
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Vault.Service.exe

using Intermech.Vault.Interfaces;
using Intermech.Vault.Interfaces.Server;
using System;
using System.Reflection;
using System.Runtime.Remoting;

#nullable disable
namespace Intermech.Vault.Service;

internal class ServiceHandler
{
  internal void OnStart(string[] args)
  {
    ApplicationEventLog.Log.Info((object) Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_3"));
    ApplicationEventLog.Log.Info((object) ("Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString()));
    string environmentVariable = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE", EnvironmentVariableTarget.Machine);
    ApplicationEventLog.Log.Info((object) (Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_4") + environmentVariable));
    string str = !Environment.Is64BitProcess ? "x86" : "AMD64";
    ApplicationEventLog.Log.Info((object) (Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_4x") + str));
    this.StartRemotingCfg();
  }

  internal void OnStop()
  {
    this.StopRemotingCfg();
    ApplicationEventLog.Log.Info((object) Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_6"));
  }

  internal void OnShutdown()
  {
    ApplicationEventLog.Log.Info((object) Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_7"));
    this.StopRemotingCfg();
  }

  internal void StartRemotingCfg()
  {
    try
    {
      CommonVariables.ConfigFileName = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
      RemotingConfiguration.Configure(CommonVariables.ConfigFileName, false);
    }
    catch (Exception ex)
    {
      ApplicationEventLog.Log.Fatal((object) Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_8"), ex);
    }
  }

  internal void StopRemotingCfg()
  {
    try
    {
      DiskFileStorageCollection.RemoveAllConnections();
    }
    catch (Exception ex)
    {
      ApplicationEventLog.Log.Error((object) Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_9"), ex);
    }
  }
}
