// Decompiled with JetBrains decompiler
// Type: Intermech.ProEngineer.Integrator.CheckNoVersionFilesTask
// Assembly: Intermech.ProEngineer.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 19987673-5EB5-4BB3-AE60-6A96614A14F3
// Assembly location: D:\IPS\Client\Intermech.ProEngineer.Integrator.dll

using Intermech.ControlFlow;
using Intermech.IO;
using System;
using System.IO;
using System.Text.RegularExpressions;

#nullable disable
namespace Intermech.ProEngineer.Integrator;

internal sealed class CheckNoVersionFilesTask : IAction
{
  private readonly string fullPath;
  private static readonly Regex numberRegex = new Regex("^\\d+$", RegexOptions.Compiled);

  public CheckNoVersionFilesTask(string fullPath)
  {
    if (fullPath == null)
      throw new ArgumentNullException(nameof (fullPath));
    this.fullPath = Path.IsPathRooted(fullPath) ? fullPath : throw new ArgumentException("Требуется абсолютный путь к файлу.", nameof (fullPath));
  }

  public void Perform()
  {
    string directoryName = Path.GetDirectoryName(this.fullPath);
    string str = Path.GetFileName(this.fullPath) + ".*";
    string path = (string) null;
    string searchPattern = str;
    foreach (string enumerateFile in Directory.EnumerateFiles(directoryName, searchPattern, SearchOption.TopDirectoryOnly))
    {
      if (!PathUtils.IsSamePath(enumerateFile, this.fullPath) && this.IsVersionFile(enumerateFile))
      {
        path = enumerateFile;
        break;
      }
    }
    if (path != null)
      throw new FaultException(string.Format(Localization.rm.GetString("ProEngineer.Integrator_1"), (object) Path.GetFileName(path)));
  }

  private bool IsVersionFile(string filePath)
  {
    int num = filePath.LastIndexOf('.');
    if (num >= 0)
    {
      int startIndex = num + 1;
      if (startIndex < filePath.Length)
      {
        string input = filePath.Substring(startIndex, filePath.Length - startIndex);
        return CheckNoVersionFilesTask.numberRegex.IsMatch(input);
      }
    }
    return false;
  }
}
