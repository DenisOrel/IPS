// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor.SyntaxDefinitions
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor;

internal sealed class SyntaxDefinitions
{
  private object syncRoot;
  private bool isLoaded;

  public SyntaxDefinitions() => this.syncRoot = new object();

  public void LoadSyntaxDefinitions()
  {
    lock (this.syncRoot)
    {
      if (this.isLoaded)
        return;
      this.LoadSyntaxDefinition((ISyntaxModeFileProvider) new CSharpSyntaxModeProvider());
      this.LoadSyntaxDefinition((ISyntaxModeFileProvider) new PowerShellSyntaxModeProvider());
      this.LoadSyntaxDefinition((ISyntaxModeFileProvider) new PythonSyntaxModeProvider());
      this.isLoaded = true;
    }
  }

  private void LoadSyntaxDefinition(ISyntaxModeFileProvider provider)
  {
    foreach (SyntaxMode syntaxMode in (IEnumerable<SyntaxMode>) provider.SyntaxModes)
    {
      IHighlightingDefinition highlighting = HighlightingLoader.Load((XmlReader) provider.GetSyntaxModeFile(syntaxMode), (IHighlightingDefinitionReferenceResolver) HighlightingManager.Instance);
      HighlightingManager.Instance.RegisterHighlighting(syntaxMode.Name, syntaxMode.Extensions, highlighting);
    }
  }
}
