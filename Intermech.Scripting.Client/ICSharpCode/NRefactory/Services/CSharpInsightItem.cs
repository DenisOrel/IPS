// Decompiled with JetBrains decompiler
// Type: ICSharpCode.NRefactory.Services.CSharpInsightItem
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System;
using System.Diagnostics;

#nullable disable
namespace ICSharpCode.NRefactory.Services;

internal sealed class CSharpInsightItem
{
  private string text;
  private Lazy<string> descriptionProvider;

  public CSharpInsightItem(string text, Lazy<string> descriptionProvider)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    if (descriptionProvider == null)
      throw new ArgumentNullException(nameof (descriptionProvider));
    this.text = text;
    this.descriptionProvider = descriptionProvider;
  }

  public string Text
  {
    [DebuggerStepThrough] get => this.text;
  }

  public Lazy<string> DescriptionProvider
  {
    [DebuggerStepThrough] get => this.descriptionProvider;
  }
}
