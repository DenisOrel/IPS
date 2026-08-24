// Decompiled with JetBrains decompiler
// Type: ICSharpCode.NRefactory.Services.CSharpCompletionService
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using ICSharpCode.NRefactory.Completion;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Completion;
using ICSharpCode.NRefactory.Editor;
using ICSharpCode.NRefactory.TypeSystem;
using Intermech.Scripting.Common.DesignTime;
using System;
using System.Collections.Generic;

#nullable disable
namespace ICSharpCode.NRefactory.Services;

internal sealed class CSharpCompletionService
{
  private ICSharpCompletionAssemblyLoader assemblyLoader;
  private IProjectContent projectContent;

  public CSharpCompletionService(ICSharpCompletionAssemblyLoader assemblyLoader)
  {
    this.assemblyLoader = assemblyLoader != null ? assemblyLoader : throw new ArgumentNullException(nameof (assemblyLoader));
    this.projectContent = (IProjectContent) new CSharpProjectContent();
  }

  public void AddAssembly(string filePath)
  {
    if (string.IsNullOrEmpty(filePath))
      return;
    IUnresolvedAssembly unresolvedAssembly = this.assemblyLoader.TryLoadAssembly(filePath);
    if (unresolvedAssembly == null)
      return;
    this.projectContent = this.projectContent.AddAssemblyReferences((IAssemblyReference) unresolvedAssembly);
  }

  public void GetCompletions(
    IDocument document,
    int offset,
    bool manualMode,
    bool includeInsightItems,
    ICodeCompletionResultBuilder resultBuilder)
  {
    if (string.IsNullOrEmpty(document.FileName))
      return;
    CSharpCompletionContext context = new CSharpCompletionContext(document, offset, this.projectContent);
    CSharpCompletionDataFactory factory = new CSharpCompletionDataFactory(context.TypeResolveContextAtCaret, context);
    CSharpCompletionEngine completionEngine = new CSharpCompletionEngine(context.Document, context.CompletionContextProvider, (ICompletionDataFactory) factory, context.ProjectContent, context.TypeResolveContextAtCaret);
    completionEngine.EolMarker = Environment.NewLine;
    completionEngine.FormattingPolicy = context.TextConversionHelper.FormattingOptions;
    char charAt = context.Document.GetCharAt(context.Offset - 1);
    int wordLength;
    IEnumerable<ICompletionData> completionData1;
    if (manualMode)
    {
      int startPos;
      if (!completionEngine.TryGetCompletionWord(context.Offset, out startPos, out wordLength))
      {
        startPos = context.Offset;
        wordLength = 0;
      }
      completionData1 = completionEngine.GetCompletionData(startPos, true);
    }
    else
    {
      int offset1 = context.Offset;
      if (char.IsLetterOrDigit(charAt) || charAt == '_')
      {
        if (offset1 > 1 && char.IsLetterOrDigit(context.Document.GetCharAt(offset1 - 2)))
          return;
        completionData1 = completionEngine.GetCompletionData(offset1, false);
        int num = offset1 - 1;
        wordLength = 1;
      }
      else
      {
        completionData1 = completionEngine.GetCompletionData(offset1, false);
        wordLength = 0;
      }
    }
    foreach (CompletionData completionData2 in completionData1)
      resultBuilder.AddCompletionItem(completionData2.ItemType, completionData2.Text, completionData2.DescriptionProvider, completionData2.Priority);
    string triggerWord = wordLength != 0 ? context.Document.GetText(context.Offset - wordLength, wordLength) : string.Empty;
    resultBuilder.SetCompletionTriggerWord(triggerWord);
    if (!includeInsightItems)
      return;
    CSharpInsightProvider parameterDataProvider = (CSharpInsightProvider) new CSharpParameterCompletionEngine(context.Document, context.CompletionContextProvider, (IParameterCompletionDataFactory) factory, context.ProjectContent, context.TypeResolveContextAtCaret).GetParameterDataProvider(context.Offset, charAt);
    if (parameterDataProvider == null || parameterDataProvider.Items.Count == 0)
      return;
    foreach (CSharpInsightItem csharpInsightItem in (IEnumerable<CSharpInsightItem>) parameterDataProvider.Items)
      resultBuilder.AddOverloadInsightItem(csharpInsightItem.Text, csharpInsightItem.DescriptionProvider);
  }
}
