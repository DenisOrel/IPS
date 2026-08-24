// Decompiled with JetBrains decompiler
// Type: Intermech.ProEngineer.Integrator.PEPackAndGoService
// Assembly: Intermech.ProEngineer.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 19987673-5EB5-4BB3-AE60-6A96614A14F3
// Assembly location: D:\IPS\Client\Intermech.ProEngineer.Integrator.dll

using Intermech.IO;
using Intermech.Tools.Integrators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

#nullable disable
namespace Intermech.ProEngineer.Integrator;

internal sealed class PEPackAndGoService(IIntegrator owner) : IntegratorService(owner), IPackAndGoService
{
  public void AdaptDocumentCopy(string directoryPath, bool recursive)
  {
    if (string.IsNullOrEmpty(directoryPath))
      throw new ArgumentException();
    this.RequireReadyState();
    lock (this.Integrator.SyncRoot)
      this.DoAdaptDocumentCopy(directoryPath, recursive);
  }

  private void DoAdaptDocumentCopy(string directoryPath, bool recursive)
  {
    string[] files = Directory.GetFiles(directoryPath, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
    PathDictionary<List<PEPackAndGoService.VersionedFileName>> pathDictionary = new PathDictionary<List<PEPackAndGoService.VersionedFileName>>(files.Length);
    foreach (string originalName in files)
    {
      PEPackAndGoService.VersionedFileName versionedFileName = new PEPackAndGoService.VersionedFileName(originalName);
      if (!versionedFileName.IsInformational)
      {
        List<PEPackAndGoService.VersionedFileName> versionedFileNameList;
        if (!pathDictionary.TryGetValue(versionedFileName.StrippedName, out versionedFileNameList))
        {
          versionedFileNameList = new List<PEPackAndGoService.VersionedFileName>(8);
          pathDictionary.Add(versionedFileName.StrippedName, versionedFileNameList);
        }
        versionedFileNameList.Add(versionedFileName);
      }
    }
    foreach (KeyValuePair<string, List<PEPackAndGoService.VersionedFileName>> keyValuePair in (Dictionary<string, List<PEPackAndGoService.VersionedFileName>>) pathDictionary)
    {
      if (keyValuePair.Value.Count > 1)
      {
        keyValuePair.Value.Sort((Comparison<PEPackAndGoService.VersionedFileName>) ((x, y) => x.Version != 0 && y.Version != 0 ? x.Version.CompareTo(y.Version) : x.LastWriteTime.CompareTo(y.LastWriteTime)));
        for (int index = 0; index < keyValuePair.Value.Count - 1; ++index)
        {
          PEPackAndGoService.VersionedFileName versionedFileName = keyValuePair.Value[index];
          File.SetAttributes(versionedFileName.OriginalName, FileAttributes.Normal);
          File.Delete(versionedFileName.OriginalName);
        }
      }
      PEPackAndGoService.VersionedFileName versionedFileName1 = keyValuePair.Value[keyValuePair.Value.Count - 1];
      if (versionedFileName1.Version != 0)
        File.Move(versionedFileName1.OriginalName, versionedFileName1.StrippedName);
    }
  }

  private sealed class VersionedFileName
  {
    public readonly string OriginalName;
    public readonly string StrippedName;
    public readonly int Version;
    private DateTime? lastWriteTime;
    private static readonly Regex versionPattern = new Regex(".+(\\.(?<ver>\\d{1,3}))$", RegexOptions.Compiled | RegexOptions.Singleline);

    public VersionedFileName(string originalName)
    {
      this.OriginalName = originalName;
      Match match = PEPackAndGoService.VersionedFileName.versionPattern.Match(originalName);
      if (match.Success)
      {
        Group group = match.Groups["ver"];
        this.StrippedName = originalName.Substring(0, originalName.Length - group.Length - 1);
        this.Version = Convert.ToInt32(group.Value);
      }
      else
      {
        this.StrippedName = originalName;
        this.Version = 0;
      }
    }

    public bool IsInformational
    {
      get
      {
        return this.StrippedName.EndsWith(".txt", StringComparison.CurrentCultureIgnoreCase) || this.StrippedName.EndsWith(".inf", StringComparison.CurrentCultureIgnoreCase);
      }
    }

    public DateTime LastWriteTime
    {
      get
      {
        if (!this.lastWriteTime.HasValue)
          this.lastWriteTime = new DateTime?(File.GetLastWriteTime(this.OriginalName));
        return this.lastWriteTime.Value;
      }
    }
  }
}
