// Decompiled with JetBrains decompiler
// Type: Intermech.SolidWorks.Integrator.SWFileBehavior
// Assembly: Intermech.SolidWorks.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C58B767B-0480-4923-A6B5-4C5307770AFD
// Assembly location: D:\IPS\Client\Intermech.SolidWorks.Integrator.dll

using Intermech.IO;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface;
using Intermech.UI;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

#nullable disable
namespace Intermech.SolidWorks.Integrator;

internal sealed class SWFileBehavior : IDependencyFilterBehavior
{
  private static readonly Regex virtualComponentPattern = new Regex("(?<asm>.+)\\\\(?<vc>.+)\\^\\k<asm>\\.(?<ext>.+)", RegexOptions.Compiled);

  public void FilterDependencies(List<DocumentFileData> dependencies)
  {
    if (dependencies.Count <= 0)
      return;
    string tempPath = Path.GetTempPath();
    for (int index = dependencies.Count - 1; index >= 0; --index)
    {
      DocumentFileData dependency = dependencies[index];
      if (PathUtils.IsPlacedIn(dependency.DocumentFilePath, tempPath))
      {
        Match match = SWFileBehavior.virtualComponentPattern.Match(dependency.DocumentFilePath);
        if (match.Success && UIReport.Enabled)
          UIReport.ReportEvent(string.Format(Localization.rm.GetString("SolidWorks.Integrator_1"), (object) match.Groups["vc"].Value, (object) match.Groups["ext"].Value));
        dependencies.RemoveAt(index);
      }
    }
  }
}
