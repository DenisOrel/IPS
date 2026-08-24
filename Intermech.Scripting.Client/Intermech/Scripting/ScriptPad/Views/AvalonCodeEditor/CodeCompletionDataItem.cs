// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor.CodeCompletionDataItem
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using Intermech.UI;
using System;
using System.Diagnostics;
using System.Windows.Media;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor;

internal sealed class CodeCompletionDataItem : ViewModel, ICompletionData
{
  private ImageSource image;
  private string text;
  private string content;
  private bool descriptionCreated;
  private Lazy<string> descriptionProvider;
  private string descriptionCache;
  private double priority;

  public CodeCompletionDataItem(
    ImageSource image,
    string text,
    string content,
    Lazy<string> descriptionProvider,
    double priority)
  {
    if (descriptionProvider == null)
      throw new ArgumentNullException(nameof (descriptionProvider));
    this.image = image;
    this.text = text;
    this.content = content;
    this.descriptionProvider = descriptionProvider;
    this.priority = priority;
  }

  public ImageSource Image => this.image;

  public string Text => this.text;

  public object Content => (object) this.content;

  public object Description
  {
    [DebuggerStepThrough] get
    {
      if (!this.descriptionCreated)
      {
        this.descriptionCache = this.CreateDescription();
        this.descriptionCreated = true;
      }
      return (object) this.descriptionCache;
    }
  }

  private string CreateDescription()
  {
    string description = this.descriptionProvider.Value;
    if (string.IsNullOrEmpty(description))
      description = (string) null;
    return description;
  }

  public double Priority => this.priority;

  public void Complete(
    TextArea textArea,
    ISegment completionSegment,
    EventArgs insertionRequestEventArgs)
  {
    textArea.Document.Replace(completionSegment, this.Text);
  }
}
