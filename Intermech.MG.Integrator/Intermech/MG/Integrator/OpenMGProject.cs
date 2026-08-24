// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.OpenMGProject
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Data;
using Intermech.Tools.Integrators;
using System;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class OpenMGProject : IOpenDocument
{
  private readonly IMGProject project;
  private string fullPath;

  public OpenMGProject(IMGProject project, string fullPath)
  {
    this.project = project;
    this.fullPath = fullPath;
  }

  public IValueBagContainer Properties
  {
    get => this.project.Properties ?? throw new Exception("Проект не содержит параметров.");
  }

  public string FullPath => this.fullPath;
}
