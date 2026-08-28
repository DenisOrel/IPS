// Decompiled with JetBrains decompiler
// Type: Intermech.Vault.Service.Intermech_Vault_Service
// Assembly: Intermech.Vault.Service, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 24B0DA67-997A-4E40-A745-DAEE647016D5
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Vault.Service.exe

using Intermech.Vault.Interfaces;
using Intermech.Vault.Interfaces.Server;
using System.ComponentModel;
using System.ServiceProcess;

#nullable disable
namespace Intermech.Vault.Service;

internal class Intermech_Vault_Service : ServiceBase
{
  internal ServiceHandler Handler;
  private IContainer components;

  public Intermech_Vault_Service()
  {
    this.InitializeComponent();
    this.Handler = new ServiceHandler();
  }

  protected override void OnStart(string[] args)
  {
    ApplicationEventLog.InitLogger();
    ApplicationEventLog.LoggingTypeChange(CommonVariables.FullLogging);
    base.OnStart(args);
    this.Handler.OnStart(args);
  }

  protected override void OnStop()
  {
    this.Handler.OnStop();
    base.OnStop();
  }

  protected override void OnShutdown()
  {
    this.Handler.OnShutdown();
    base.OnShutdown();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.AutoLog = false;
    this.ServiceName = CommonVariables.SERVICE_NAME;
  }
}
