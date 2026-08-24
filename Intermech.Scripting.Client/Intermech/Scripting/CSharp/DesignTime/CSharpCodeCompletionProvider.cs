// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.DesignTime.CSharpCodeCompletionProvider
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using ICSharpCode.NRefactory.Editor;
using ICSharpCode.NRefactory.Services;
using ICSharpCode.NRefactory.TextSources;
using Intermech.Scripting.Common.DesignTime;
using System;

#nullable disable
namespace Intermech.Scripting.CSharp.DesignTime;

internal sealed class CSharpCodeCompletionProvider : ICodeCompletionProvider
{
  private NRefactoryCodeCompletionServiceProvider nrefactoryServiceProvider;
  private bool runAtClientSide;

  public CSharpCodeCompletionProvider(
    bool runAtClientSide,
    NRefactoryCodeCompletionServiceProvider nrefactoryServiceProvider)
  {
    if (nrefactoryServiceProvider == null)
      throw new ArgumentNullException(nameof (nrefactoryServiceProvider));
    this.runAtClientSide = runAtClientSide;
    this.nrefactoryServiceProvider = nrefactoryServiceProvider;
  }

  public void GetResult(
    IReadOnlyTextDocument document,
    int documentOffset,
    bool manualMode,
    bool includeInsightItems,
    ICodeCompletionResultBuilder resultBuilder)
  {
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    if (documentOffset < 0 || documentOffset > document.Length)
      throw new ArgumentOutOfRangeException(nameof (documentOffset));
    if (resultBuilder == null)
      throw new ArgumentNullException(nameof (resultBuilder));
    CSharpCompletionService completionService = this.nrefactoryServiceProvider.TryGetCodeCompletionService(this.runAtClientSide);
    if (completionService != null)
    {
      ReadOnlyDocument readOnlyDocument = new ReadOnlyDocument((ITextSource) new ReadOnlyTextSource(document), "scripts" + ".cs");
      completionService.GetCompletions((IDocument) readOnlyDocument, documentOffset, manualMode, includeInsightItems, resultBuilder);
    }
    else
      this.GetResultForInitializationPending(document, documentOffset, manualMode, includeInsightItems, resultBuilder);
  }

  private void GetResultForInitializationPending(
    IReadOnlyTextDocument document,
    int documentOffset,
    bool manualMode,
    bool includeInsightItems,
    ICodeCompletionResultBuilder resultBuilder)
  {
    if (manualMode & includeInsightItems)
      resultBuilder.AddOverloadInsightItem(this.nrefactoryServiceProvider.InitializationPendingMessage, this.nrefactoryServiceProvider.InitializationPendingDescription);
    else
      resultBuilder.AddCompletionItem(CodeCompletionItemType.Unknown, this.nrefactoryServiceProvider.InitializationPendingMessage, this.nrefactoryServiceProvider.InitializationPendingDescription, 1.0);
  }
}
