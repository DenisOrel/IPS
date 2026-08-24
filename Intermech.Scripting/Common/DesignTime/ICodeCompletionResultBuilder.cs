// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.ICodeCompletionResultBuilder
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

public interface ICodeCompletionResultBuilder
{
  void AddCompletionItem(
    CodeCompletionItemType itemType,
    string text,
    Lazy<string> descriptionProvider,
    double priority);

  void SetCompletionTriggerWord(string triggerWord);

  void AddOverloadInsightItem(string text, Lazy<string> descriptionProvider);
}
