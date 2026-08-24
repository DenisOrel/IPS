// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.IMGApplication
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using System.Collections.Generic;

#nullable disable
namespace Intermech.MG.Integrator;

internal interface IMGApplication
{
  void OpenProject(string fullName, bool openVisible);

  bool CloseProject();

  bool CloseProjectBeforeSave();

  bool FileLocked(string fullPath);

  List<string> GetSatelliteFiles(string fullPath);

  string ProjectFile { get; }

  IMGProject CurrentProject { get; }
}
