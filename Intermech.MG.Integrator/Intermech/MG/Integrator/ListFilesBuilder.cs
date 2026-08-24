// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.ListFilesBuilder
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class ListFilesBuilder
{
  private string _masterfileFulPath;
  private MGIntegratorSettings _integratorSettings;

  public ListFilesBuilder(MGIntegratorSettings integratorSettings, string masterfileFulPath)
  {
    this._integratorSettings = integratorSettings;
    this._masterfileFulPath = masterfileFulPath;
  }

  public List<string> GetProjectFiles()
  {
    List<string> projectFiles = new List<string>();
    List<string> listFiles = this.GetListFiles(new FileInfo(this._masterfileFulPath).DirectoryName, this._integratorSettings.NotImportingDir);
    for (int index = 0; index < listFiles.Count; ++index)
    {
      if (!listFiles[index].Contains(this._masterfileFulPath))
        projectFiles.Add(listFiles[index]);
    }
    return projectFiles;
  }

  private List<string> GetListFiles(string dir, List<string> filter)
  {
    List<string> listFiles1 = new List<string>();
    foreach (string file in Directory.GetFiles(dir))
      listFiles1.Add(file);
    foreach (string directory in Directory.GetDirectories(dir))
    {
      DirectoryInfo directoryInfo = new DirectoryInfo(directory);
      if (filter.IndexOf(directoryInfo.Name) < 0)
      {
        List<string> listFiles2 = this.GetListFiles(directory, filter);
        if (listFiles2.Count > 0)
          listFiles1.AddRange((IEnumerable<string>) listFiles2);
      }
    }
    return listFiles1;
  }
}
