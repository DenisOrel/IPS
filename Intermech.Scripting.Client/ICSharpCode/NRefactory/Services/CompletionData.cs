// Decompiled with JetBrains decompiler
// Type: ICSharpCode.NRefactory.Services.CompletionData
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using ICSharpCode.NRefactory.Completion;
using Intermech.Scripting.Common.DesignTime;
using System;
using System.Collections.Generic;

#nullable disable
namespace ICSharpCode.NRefactory.Services;

internal class CompletionData : ICompletionData
{
  private Lazy<string> descriptionProvider;
  private static readonly IList<ICompletionData> emptyOverloads = (IList<ICompletionData>) new ICompletionData[0];
  private IList<ICompletionData> overloads = CompletionData.emptyOverloads;
  private double priority = 1.0;

  protected CompletionData()
    : this(string.Empty)
  {
  }

  public CompletionData(string text)
  {
    this.DisplayText = this.CompletionText = text;
    this.descriptionProvider = CSharpCompletionConsts.EmptyStringProvider;
  }

  public CompletionCategory CompletionCategory { get; set; }

  public string DisplayText { get; set; }

  public Lazy<string> DescriptionProvider
  {
    get => this.descriptionProvider;
    set
    {
      this.descriptionProvider = value != null ? value : throw new ArgumentNullException(nameof (value));
    }
  }

  public string Description
  {
    get => this.DescriptionProvider.Value;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      this.DescriptionProvider = value != string.Empty ? new Lazy<string>((Func<string>) (() => value)) : CSharpCompletionConsts.EmptyStringProvider;
    }
  }

  public string CompletionText { get; set; }

  public DisplayFlags DisplayFlags { get; set; }

  public IEnumerable<ICompletionData> OverloadedData
  {
    get => (IEnumerable<ICompletionData>) this.overloads;
  }

  public void AddOverload(ICompletionData data)
  {
    if (this.overloads.Count == 0)
    {
      if (this.overloads.IsReadOnly)
        this.overloads = (IList<ICompletionData>) new List<ICompletionData>();
      this.overloads.Add((ICompletionData) this);
    }
    this.overloads.Add(data);
  }

  public bool HasOverloads => this.overloads.Count != 0;

  public int GetOverloadsCount() => this.overloads.Count;

  public CodeCompletionItemType ItemType { get; set; }

  public object Content => (object) this.DisplayText;

  public virtual double Priority
  {
    get => this.priority;
    set => this.priority = value;
  }

  public string Text => this.CompletionText;

  public override string ToString() => this.DisplayText;

  public override bool Equals(object obj)
  {
    return obj is CompletionData completionData && this.DisplayText == completionData.DisplayText;
  }

  public override int GetHashCode() => this.DisplayText.GetHashCode();
}
