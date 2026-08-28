// Decompiled with JetBrains decompiler
// Type: Intermech.Vault.Service.ProjectInstaller
// Assembly: Intermech.Vault.Service, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 24B0DA67-997A-4E40-A745-DAEE647016D5
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Vault.Service.exe

using Intermech.Vault.Interfaces;
using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.Runtime.InteropServices;
using System.ServiceProcess;

#nullable disable
namespace Intermech.Vault.Service;

[RunInstaller(true)]
public class ProjectInstaller : Installer
{
  private IContainer components;
  private ServiceProcessInstaller serviceProcessInstaller1;
  private ServiceInstaller Intermech_Vault_Installer;

  [DllImport("advapi32.dll", SetLastError = true)]
  private static extern bool DeleteService(IntPtr hService);

  [DllImport("advapi32.dll", SetLastError = true)]
  private static extern bool CloseServiceHandle(IntPtr hSCObject);

  public ProjectInstaller() => this.InitializeComponent();

  private void Intermech_Vault_Installer_AfterInstall(object sender, InstallEventArgs e)
  {
    ServiceController serviceController = new ServiceController(CommonVariables.SERVICE_NAME, Environment.MachineName);
    if (serviceController.Status == ServiceControllerStatus.Running)
      return;
    serviceController.Start();
  }

  private void Uninstall()
  {
    foreach (ServiceController service in ServiceController.GetServices())
    {
      if (service.DisplayName == CommonVariables.SERVICE_NAME)
      {
        if (service.Status == ServiceControllerStatus.Running)
        {
          service.Stop();
          service.WaitForStatus(ServiceControllerStatus.Stopped);
        }
        try
        {
          IntPtr handle = service.ServiceHandle.DangerousGetHandle();
          ProjectInstaller.DeleteService(handle);
          ProjectInstaller.CloseServiceHandle(handle);
          IntPtr zero = IntPtr.Zero;
          break;
        }
        finally
        {
          service.Close();
          service.Dispose();
        }
      }
    }
  }

  private void serviceProcessInstaller1_BeforeInstall(object sender, InstallEventArgs e)
  {
    this.Uninstall();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.serviceProcessInstaller1 = new ServiceProcessInstaller();
    this.Intermech_Vault_Installer = new ServiceInstaller();
    this.serviceProcessInstaller1.Account = ServiceAccount.LocalSystem;
    this.serviceProcessInstaller1.Password = (string) null;
    this.serviceProcessInstaller1.Username = (string) null;
    this.serviceProcessInstaller1.BeforeInstall += new InstallEventHandler(this.serviceProcessInstaller1_BeforeInstall);
    this.Intermech_Vault_Installer.Description = "Служба хранения файлов и двоичных данных IPS. Data Vault Service";
    this.Intermech_Vault_Installer.ServiceName = "IPS.DVS";
    this.Intermech_Vault_Installer.StartType = ServiceStartMode.Automatic;
    this.Intermech_Vault_Installer.AfterInstall += new InstallEventHandler(this.Intermech_Vault_Installer_AfterInstall);
    this.Installers.AddRange(new Installer[2]
    {
      (Installer) this.serviceProcessInstaller1,
      (Installer) this.Intermech_Vault_Installer
    });
  }
}
