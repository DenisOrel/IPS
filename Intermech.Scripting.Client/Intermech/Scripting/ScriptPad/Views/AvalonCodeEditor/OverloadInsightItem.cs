// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor.OverloadInsightItem
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.UI;
using System;
using System.Diagnostics;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor;

internal sealed class OverloadInsightItem : ViewModel
{
  private string text;
  private bool descriptionCreated;
  private Lazy<string> descriptionProvider;
  private string descriptionCache;

  public OverloadInsightItem(string text, Lazy<string> descriptionProvider)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    if (descriptionProvider == null)
      throw new ArgumentNullException(nameof (descriptionProvider));
    this.text = text;
    this.descriptionProvider = descriptionProvider;
  }

  public string Text => this.text;

  public string Description
  {
    [DebuggerStepThrough] get
    {
      if (!this.descriptionCreated)
      {
        this.descriptionCache = this.CreateDescription();
        this.descriptionCreated = true;
      }
      return this.descriptionCache;
    }
  }

  private string CreateDescription()
  {
    string str = this.descriptionProvider.Value;
    return !string.IsNullOrEmpty(str) ? Environment.NewLine + str : (string) null;
  }
}
