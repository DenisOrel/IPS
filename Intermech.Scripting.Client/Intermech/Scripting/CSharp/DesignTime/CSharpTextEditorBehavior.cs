// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.DesignTime.CSharpTextEditorBehavior
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Collections;
using Intermech.Scripting.Common.DesignTime;
using Intermech.Scripting.CSharp.ServiceProcess;
using Intermech.Scripting.Projects.DBScripts;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.CSharp.DesignTime;

internal sealed class CSharpTextEditorBehavior : IScriptTextEditorBehavior
{
  private ScriptProject scriptProject;
  private static volatile NRefactoryCodeCompletionServiceProvider nrefactoryServiceProvider;

  public CSharpTextEditorBehavior(ScriptProject scriptProject)
  {
    this.scriptProject = scriptProject != null ? scriptProject : throw new ArgumentNullException(nameof (scriptProject));
  }

  public Dictionary<string, string> TryCreateCodeModelOptions(
    Dictionary<string, string> scriptProjectOptions,
    Dictionary<string, string> runtimeOptions)
  {
    if (scriptProjectOptions == null)
      throw new ArgumentNullException(nameof (scriptProjectOptions));
    CSharpScriptDebugRuntimeOptions debugRuntimeOptions = runtimeOptions != null ? CSharpScriptDebugRuntimeOptions.FromDictionary(runtimeOptions) : throw new ArgumentNullException(nameof (runtimeOptions));
    ScriptParseOptions parseOptions = new ScriptParseOptions();
    parseOptions.AutoReferencedAssemblies.AddRange((IEnumerable<string>) debugRuntimeOptions.AutoReferencedAssemblies);
    parseOptions.SearchPathList.AddRange((IEnumerable<string>) debugRuntimeOptions.SearchPathList);
    return ScriptParseOptions.ToDictionary(parseOptions);
  }

  public ICodeCompletionProvider TryGetCodeCompletionProvider(ICollection<string> xmlDocPathList)
  {
    if (xmlDocPathList == null)
      throw new ArgumentNullException(nameof (xmlDocPathList));
    bool runAtClientSide = true;
    if (this.scriptProject is DBScriptProject)
      runAtClientSide = ((DBScriptProject) this.scriptProject).RunAtClientSide;
    if (CSharpTextEditorBehavior.nrefactoryServiceProvider == null || !CollectionUtils.ContentEqual<string>(CSharpTextEditorBehavior.nrefactoryServiceProvider.XmlDocPathList, xmlDocPathList))
      CSharpTextEditorBehavior.nrefactoryServiceProvider = new NRefactoryCodeCompletionServiceProvider(xmlDocPathList);
    return (ICodeCompletionProvider) new CSharpCodeCompletionProvider(runAtClientSide, CSharpTextEditorBehavior.nrefactoryServiceProvider);
  }
}
