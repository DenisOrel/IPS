// Decompiled with JetBrains decompiler
// Type: Intermech.Server.Service.ProjectInstaller
// Assembly: Intermech.Server.Service, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E91FE21E-230A-49EC-A627-5E0B3AE2517E
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Server.Service.exe

using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

#nullable disable
namespace Intermech.Server.Service;

[RunInstaller(true)]
public class ProjectInstaller : Installer
{
  private IContainer components;
  private ServiceProcessInstaller serviceProcessInstaller;
  private ServiceInstaller serviceInstaller;

  public ProjectInstaller()
  {
    this.InitializeComponent();
    this.serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.serviceProcessInstaller = new ServiceProcessInstaller();
    this.serviceInstaller = new ServiceInstaller();
    this.serviceProcessInstaller.Password = (string) null;
    this.serviceProcessInstaller.Username = (string) null;
    this.serviceInstaller.Description = "Обеспечивает доступ к серверу приложений IPS";
    this.serviceInstaller.DisplayName = "Сервер приложений IPS";
    this.serviceInstaller.ServiceName = "IPSserver1";
    this.serviceInstaller.StartType = ServiceStartMode.Automatic;
    this.Installers.AddRange(new Installer[2]
    {
      (Installer) this.serviceProcessInstaller,
      (Installer) this.serviceInstaller
    });
  }
}
