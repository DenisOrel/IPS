// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Projects.DiskScripts.DiskScriptSaveChangesBehavior
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common.DesignTime;
using System;

#nullable disable
namespace Intermech.Scripting.Projects.DiskScripts;

internal sealed class DiskScriptSaveChangesBehavior : IScriptSaveChangesBehavior
{
  private ScriptProject scriptProject;
  private SaveDiskScriptDialogService saveDialogService;

  public DiskScriptSaveChangesBehavior(
    ScriptProject scriptProject,
    SaveDiskScriptDialogService saveDialogService)
  {
    if (scriptProject == null)
      throw new ArgumentNullException(nameof (scriptProject));
    if (saveDialogService == null)
      throw new ArgumentNullException(nameof (saveDialogService));
    this.scriptProject = scriptProject;
    this.saveDialogService = saveDialogService;
  }

  public ScriptSaveAsParameters TrySaveAs()
  {
    string path = this.saveDialogService.TrySelectFilePath(this.scriptProject);
    return path != null ? (ScriptSaveAsParameters) new DiskScriptSaveAsParameters(path) : (ScriptSaveAsParameters) null;
  }

  public void BeforeSave(ScriptBeforeSaveEventArgs e)
  {
  }

  public void AfterSave(ScriptAfterSaveEventArgs e)
  {
  }
}
