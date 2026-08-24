// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor.CodeCompletionResultBuilder
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Collections;
using Intermech.Scripting.Common.DesignTime;
using System;
using System.Collections.Generic;
using System.Windows.Media;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor;

internal sealed class CodeCompletionResultBuilder : ICodeCompletionResultBuilder
{
  private static readonly CodeCompletionTextComparer completionDataTextComparer = new CodeCompletionTextComparer();
  private List<CodeCompletionDataItem> completionDataItems;
  private string completionTriggerWord;
  private List<OverloadInsightItem> overloadInsightItems;

  public CodeCompletionResultBuilder()
  {
    this.completionDataItems = new List<CodeCompletionDataItem>();
    this.completionTriggerWord = string.Empty;
    this.overloadInsightItems = new List<OverloadInsightItem>();
  }

  public List<CodeCompletionDataItem> CompletionDataItems => this.completionDataItems;

  public string CompletionTriggerWord => this.completionTriggerWord;

  public List<OverloadInsightItem> OverloadInsightItems => this.overloadInsightItems;

  public void AddCompletionItem(
    CodeCompletionItemType itemType,
    string text,
    Lazy<string> descriptionProvider,
    double priority)
  {
    if (descriptionProvider == null)
      throw new ArgumentNullException(nameof (descriptionProvider));
    CollectionUtils.AddSorted<CodeCompletionDataItem>(this.completionDataItems, new CodeCompletionDataItem(this.ToImageSource(itemType), text, text, descriptionProvider, priority), (IComparer<CodeCompletionDataItem>) CodeCompletionResultBuilder.completionDataTextComparer);
  }

  public void SetCompletionTriggerWord(string triggerWord)
  {
    this.completionTriggerWord = triggerWord != null ? triggerWord : throw new ArgumentNullException(nameof (triggerWord));
  }

  public void AddOverloadInsightItem(string text, Lazy<string> descriptionProvider)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    if (descriptionProvider == null)
      throw new ArgumentNullException(nameof (descriptionProvider));
    this.overloadInsightItems.Add(new OverloadInsightItem(text, descriptionProvider));
  }

  public void SetOverloadInsightHandler(IOverloadInsightHandler handler)
  {
  }

  private ImageSource ToImageSource(CodeCompletionItemType itemType)
  {
    switch (itemType)
    {
      case CodeCompletionItemType.Literal:
        return CodeCompletionImages.LiteralImage;
      case CodeCompletionItemType.Namespace:
        return CodeCompletionImages.NamespaceImage;
      case CodeCompletionItemType.Field:
        return CodeCompletionImages.FieldImage;
      case CodeCompletionItemType.ReadOnlyField:
        return CodeCompletionImages.ReadOnlyFieldImage;
      case CodeCompletionItemType.Class:
        return CodeCompletionImages.ClassImage;
      case CodeCompletionItemType.StaticClass:
        return CodeCompletionImages.StaticClassImage;
      case CodeCompletionItemType.Struct:
        return CodeCompletionImages.StructImage;
      case CodeCompletionItemType.Interface:
        return CodeCompletionImages.InterfaceImage;
      case CodeCompletionItemType.Delegate:
        return CodeCompletionImages.DelegateImage;
      case CodeCompletionItemType.Enum:
        return CodeCompletionImages.EnumImage;
      case CodeCompletionItemType.EnumValue:
        return CodeCompletionImages.EnumValueImage;
      case CodeCompletionItemType.Constructor:
        return CodeCompletionImages.ConstructorImage;
      case CodeCompletionItemType.Method:
        return CodeCompletionImages.MethodImage;
      case CodeCompletionItemType.VirtualMethod:
        return CodeCompletionImages.VirtualMethodImage;
      case CodeCompletionItemType.ExtensionMethod:
        return CodeCompletionImages.ExtensionMethodImage;
      case CodeCompletionItemType.PInvokeMethod:
        return CodeCompletionImages.PInvokeMethodImage;
      case CodeCompletionItemType.Operator:
        return CodeCompletionImages.OperatorImage;
      case CodeCompletionItemType.Property:
        return CodeCompletionImages.PropertyImage;
      case CodeCompletionItemType.IndexerProperty:
        return CodeCompletionImages.IndexerPropertyImage;
      case CodeCompletionItemType.Event:
        return CodeCompletionImages.EventImage;
      default:
        return (ImageSource) null;
    }
  }
}
