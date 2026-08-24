// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.DXDApplication
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using ImSSP;
using Intermech.Interfaces;
using Intermech.Tools.Integrators;
using Interop.Viewdraw;
using System;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class DXDApplication : MGApplication<IVdApp>
{
  private DXDInterfaceService _parent;

  public DXDApplication(
    IServiceProvider integrator,
    MGIntegratorSettings integratorSettings,
    IVdApp cadObject,
    DXDInterfaceService parent)
    : base(integrator, integratorSettings, cadObject)
  {
    this._parent = parent;
  }

  public override string Name => sc_14695.ssp_mentor_14696();

  protected override IMGProject OnOpenProject(string fullName, bool openVisible)
  {
    fullName.Equals(this.cadObject.CurrentProject);
    // ISSUE: reference to a compiler-generated method
    this.cadObject.OpenProject(fullName);
    // ISSUE: reference to a compiler-generated method
    return (IMGProject) new DXDProject(this.cadObject.GetProjectData(), this.integratorSettings, ServiceUtils.GetService<IIntegratorOutput>((object) this.integrator, true), this.cadObject);
  }

  public override bool IsAlive => this.cadObject != null && this.cadObject.Version != null;

  public override bool CloseProjectBeforeSave()
  {
    this.DisposeProject();
    // ISSUE: reference to a compiler-generated method
    this.cadObject.CloseProject();
    return true;
  }

  protected override bool OnCloseProject()
  {
    // ISSUE: reference to a compiler-generated method
    int num = this.cadObject.CloseProject() ? 1 : 0;
    if (!this._parent.OpenFromIPS)
      return num != 0;
    // ISSUE: reference to a compiler-generated method
    this.cadObject.Quit();
    Marshal.FinalReleaseComObject((object) this.cadObject);
    this.cadObject = (IVdApp) null;
    this._parent.OpenFromIPS = false;
    return num != 0;
  }

  protected override void OnSwitchToApp()
  {
    if (!this.cadObject.Visible)
      this.cadObject.Visible = true;
    // ISSUE: reference to a compiler-generated method
    this.cadObject.Activate();
  }

  protected override string FockedFile(string fullPath)
  {
    return Path.Combine(Path.GetDirectoryName(fullPath), "database\\icdb.dat");
  }
}
