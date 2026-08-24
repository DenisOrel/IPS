// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Services.DiskScriptSystemService
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common.DesignTime;
using Intermech.Scripting.CSharp.DesignTime;
using Intermech.Scripting.Projects.DiskScripts;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Intermech.Scripting.Services;

internal sealed class DiskScriptSystemService : IScriptSystemService
{
  private LanguageRegistry languageRegistry;
  private DiskScriptProjectFactory scriptFactory;
  private DiskScriptRepository scriptRepository;
  private OpenDiskScriptDialogService openScriptService;
  private string scriptDirectory;

  public DiskScriptSystemService(LanguageRegistry languageRegistry, string scriptDirectory)
  {
    if (languageRegistry == null)
      throw new ArgumentNullException(nameof (languageRegistry));
    if (scriptDirectory == null)
      throw new ArgumentNullException(nameof (scriptDirectory));
    this.languageRegistry = languageRegistry;
    this.scriptDirectory = scriptDirectory;
    this.scriptFactory = new DiskScriptProjectFactory((ICollection<LanguageInfo>) languageRegistry.Languages);
    this.scriptRepository = new DiskScriptRepository((IScriptProjectFactory) this.scriptFactory);
    this.openScriptService = new OpenDiskScriptDialogService(this.scriptRepository, this.scriptDirectory);
  }

  public ScriptProject CreateEmptyProject(LanguageInfo languageInfo)
  {
    ScriptProject emptyProject = this.scriptFactory.CreateEmptyProject(languageInfo);
    emptyProject.Behaviors.AddRepository((IScriptProjectRepository) this.scriptRepository);
    this.InitializeScriptProjectBehaviors(emptyProject);
    this.InitializeScriptProjectWithTemplate(emptyProject);
    return emptyProject;
  }

  public ScriptProject TryOpenScript(ICollection<LanguageInfo> languageFilter)
  {
    ScriptProject scriptProject = this.openScriptService.TryOpenScript(languageFilter);
    if (scriptProject == null)
      return (ScriptProject) null;
    this.InitializeScriptProjectBehaviors(scriptProject);
    return scriptProject;
  }

  private void InitializeScriptProjectBehaviors(ScriptProject scriptProject)
  {
    scriptProject.Behaviors.AddSaveChangesBehavior((IScriptSaveChangesBehavior) new DiskScriptSaveChangesBehavior(scriptProject, new SaveDiskScriptDialogService(this.scriptDirectory)));
    if (scriptProject.LanguageInfo.Name == "C#")
      scriptProject.Behaviors.AddTextEditorBehavior((IScriptTextEditorBehavior) new CSharpTextEditorBehavior(scriptProject));
    scriptProject.Behaviors.AddDebugBehavior((IScriptDebugBehavior) new DiskScriptDebugBehavior(scriptProject));
  }

  private void InitializeScriptProjectWithTemplate(ScriptProject scriptProject)
  {
    Encoding utF8 = Encoding.UTF8;
    LanguageDescriptor byLanguageName = this.languageRegistry.GetByLanguageName(scriptProject.LanguageInfo.Name, false);
    if (byLanguageName == null)
      return;
    ILanguageExtension customService = (ILanguageExtension) byLanguageName.Services.GetCustomService(typeof (ILanguageExtension), false);
    if (customService == null)
      return;
    scriptProject.File.SetContentAsText(customService.CreateAdministrativeScriptTemplate(), utF8);
  }
}
