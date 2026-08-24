// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor.CodeFoldingStrategy
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using ICSharpCode.AvalonEdit.Folding;
using Intermech.Scripting.Common.DesignTime;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor;

internal sealed class CodeFoldingStrategy
{
  private readonly FoldingManager foldingManager;
  private readonly CodeFoldingProvider codeFoldingProvider;

  public CodeFoldingStrategy(FoldingManager foldingManager, CodeFoldingProvider codeFoldingProvider)
  {
    this.foldingManager = foldingManager;
    this.codeFoldingProvider = codeFoldingProvider;
  }

  public void UpdateFoldings()
  {
    IList<FoldingRegionItem> regionsIfPossible = this.codeFoldingProvider.TryGetFoldingRegionsIfPossible();
    if (regionsIfPossible == null)
      return;
    this.foldingManager.UpdateFoldings(this.CreateNewFoldings(regionsIfPossible), -1);
  }

  private IEnumerable<NewFolding> CreateNewFoldings(IList<FoldingRegionItem> regionItems)
  {
    List<NewFolding> newFoldings = new List<NewFolding>();
    foreach (FoldingRegionItem regionItem in (IEnumerable<FoldingRegionItem>) regionItems)
    {
      NewFolding newFolding = new NewFolding(regionItem.StartOffset, regionItem.EndOffset)
      {
        Name = regionItem.RegionName
      };
      newFoldings.Add(newFolding);
    }
    newFoldings.Sort((Comparison<NewFolding>) ((a, b) => a.StartOffset.CompareTo(b.StartOffset)));
    return (IEnumerable<NewFolding>) newFoldings;
  }
}
