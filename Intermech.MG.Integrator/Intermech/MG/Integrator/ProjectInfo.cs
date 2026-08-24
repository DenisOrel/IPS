// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.ProjectInfo
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class ProjectInfo
{
  public bool NeedOpenAfterSave { get; private set; }

  public string ProjectFile { get; private set; }

  public ProjectInfo(string projectFile, bool needOpenAfterSave)
  {
    this.ProjectFile = projectFile;
    this.NeedOpenAfterSave = needOpenAfterSave;
  }

  public ProjectInfo()
    : this((string) null, false)
  {
  }
}
