// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.ExPCBApplication
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Interfaces;
using Intermech.Tools.Integrators;
using MGCPCB;
using MGCPCBAutomationLicensing;
using System;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.MG.Integrator;

internal class ExPCBApplication : MGApplication<Application>
{
  public ExPCBApplication(
    IIntegrator integrator,
    MGIntegratorSettings integratorSettings,
    Application cadObject)
    : base((IServiceProvider) integrator, integratorSettings, cadObject)
  {
    this.closeSchemaComponent = false;
    // ISSUE: variable of a compiler-generated type
    IApplication instance = LicenseServer.Instance;
  }

  protected override string FockedFile(string fullPath) => fullPath;

  public override bool IsAlive
  {
    get
    {
      if (this.cadObject == null)
        return false;
      try
      {
        if (this.cadObject.Visible)
          return true;
        Marshal.FinalReleaseComObject((object) this.cadObject);
        this.cadObject = (Application) null;
        return false;
      }
      catch
      {
        return false;
      }
    }
  }

  protected override IMGProject OnOpenProject(string fullName, bool openVisible)
  {
    string empty = string.Empty;
    FileInfo fileInfo = new FileInfo(fullName);
    if (!fileInfo.Extension.ToLower().Equals(MGConsts.ProjectFileExtension))
      throw new Exception("Тип файлов интегратором не поддерживается.");
    string pcbPath = (string) null;
    using (Stream stream = (Stream) new FileStream(fullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
    {
      int num = (int) MGProjectHelper.DefineProjectType(stream, out pcbPath);
    }
    if (string.IsNullOrEmpty(pcbPath))
      throw new Exception("В проекте не найден файл печатной платы *.pcb");
    string str = Path.Combine(fileInfo.DirectoryName, pcbPath);
    // ISSUE: reference to a compiler-generated method
    return (IMGProject) new ExPCBProject(this.cadObject.OpenDocument(str), this.integratorSettings, ServiceUtils.GetService<IIntegratorOutput>((object) this.integrator, true), this.cadObject, str);
  }

  protected override void OnSwitchToApp()
  {
    if (this.cadObject.Visible)
      return;
    this.cadObject.Visible = true;
  }

  protected override bool OnCloseProject() => false;

  public override string Name => "Expedition PCB";
}
