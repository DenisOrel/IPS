// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Projects.DiskScripts.DiskScriptRepository
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common.DesignTime;
using System;
using System.IO;

#nullable disable
namespace Intermech.Scripting.Projects.DiskScripts;

internal sealed class DiskScriptRepository : IScriptProjectRepository
{
  private IScriptProjectFactory scriptProjectFactory;

  public DiskScriptRepository(IScriptProjectFactory scriptProjectFactory)
  {
    this.scriptProjectFactory = scriptProjectFactory != null ? scriptProjectFactory : throw new ArgumentNullException(nameof (scriptProjectFactory));
  }

  public ScriptProject Get(object key)
  {
    DiskScriptKey diskScriptKey = DiskScriptKey.CastFrom(key);
    ScriptProject emptyProject = this.scriptProjectFactory.CreateEmptyProject(Path.GetExtension(diskScriptKey.Path));
    emptyProject.Name = Path.GetFileName(diskScriptKey.Path);
    emptyProject.File.SetContent(File.ReadAllBytes(diskScriptKey.Path));
    emptyProject.RepositoryKey = (object) diskScriptKey;
    emptyProject.Behaviors.AddRepository((IScriptProjectRepository) this);
    return emptyProject;
  }

  public void Add(ScriptProject scriptProject, ScriptSaveAsParameters parameters)
  {
    if (scriptProject == null)
      throw new ArgumentNullException(nameof (scriptProject));
    DiskScriptKey diskScriptKey = parameters != null ? new DiskScriptKey(((DiskScriptSaveAsParameters) parameters).Path) : throw new ArgumentNullException(nameof (parameters));
    File.WriteAllBytes(diskScriptKey.Path, scriptProject.File.GetContent());
    scriptProject.RepositoryKey = (object) diskScriptKey;
    scriptProject.Name = Path.GetFileName(diskScriptKey.Path);
  }

  public void Update(ScriptProject scriptProject)
  {
    if (scriptProject == null)
      throw new ArgumentNullException(nameof (scriptProject));
    File.WriteAllBytes(DiskScriptKey.CastFrom(scriptProject.RepositoryKey).Path, scriptProject.File.GetContent());
  }
}
