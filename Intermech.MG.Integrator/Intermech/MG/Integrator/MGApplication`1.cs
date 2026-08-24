// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.MGApplication`1
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using ImSSP;
using Intermech.Runtime.ComInterop.Proxies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.MG.Integrator;

internal abstract class MGApplication<T> : IMGApplication
{
  protected IMGProject currentProject;
  protected T cadObject;
  protected readonly IServiceProvider integrator;
  protected MGIntegratorSettings integratorSettings;
  protected bool closeSchemaComponent = true;
  protected string path;

  public MGApplication(
    IServiceProvider integrator,
    MGIntegratorSettings integratorSettings,
    T cadObject)
  {
    this.integrator = integrator;
    this.integratorSettings = integratorSettings;
    this.cadObject = cadObject;
  }

  protected abstract string FockedFile(string fullPath);

  public bool FileLocked(string fullPath)
  {
    string path = this.FockedFile(fullPath);
    if (!File.Exists(path))
      throw new Exception($"Не найден файл {fullPath}");
    try
    {
      using (new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        return false;
    }
    catch
    {
      return true;
    }
  }

  public List<string> GetSatelliteFiles(string fullPath)
  {
    return new ListFilesBuilder(this.integratorSettings, fullPath).GetProjectFiles();
  }

  public abstract bool IsAlive { get; }

  protected abstract void OnSwitchToApp();

  public void SwitchToApp()
  {
    try
    {
      this.OnSwitchToApp();
    }
    catch (COMException ex)
    {
      throw new ApplicationProxyException(string.Format(sc_14745.ssp_mentor_14746(), (object) this.Name, (object) ex.Message), (Exception) ex);
    }
  }

  public void OpenProject(string fullName, bool openVisible)
  {
    if (this.path != null && this.CurrentProject != null && this.path.Equals(fullName))
      return;
    this.currentProject = this.OnOpenProject(fullName, openVisible);
    this.path = fullName;
  }

  protected abstract IMGProject OnOpenProject(string fullName, bool openVisible);

  protected void DisposeProject()
  {
    this.path = (string) null;
    if (this.currentProject == null)
      return;
    this.currentProject.Dispose();
    this.currentProject = (IMGProject) null;
  }

  public bool CloseProject()
  {
    this.DisposeProject();
    return this.OnCloseProject();
  }

  public virtual bool CloseProjectBeforeSave() => false;

  public virtual bool BeforeSaveProject() => false;

  protected abstract bool OnCloseProject();

  public abstract string Name { get; }

  public string ProjectFile => this.path;

  public IMGProject CurrentProject
  {
    get
    {
      try
      {
        if (!this.currentProject.IsValid())
        {
          this.currentProject = (IMGProject) null;
          this.OpenProject(this.path, true);
        }
      }
      catch (Exception ex)
      {
        if (ex is COMException)
        {
          this.currentProject = (IMGProject) null;
          this.OpenProject(this.path, true);
        }
      }
      return this.currentProject;
    }
  }
}
