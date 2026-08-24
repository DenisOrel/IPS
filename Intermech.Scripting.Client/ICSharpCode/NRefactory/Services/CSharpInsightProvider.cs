// Decompiled with JetBrains decompiler
// Type: ICSharpCode.NRefactory.Services.CSharpInsightProvider
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using ICSharpCode.NRefactory.Completion;
using System;
using System.Collections.Generic;

#nullable disable
namespace ICSharpCode.NRefactory.Services;

internal sealed class CSharpInsightProvider : IParameterDataProvider
{
  private int startOffset;
  private ICollection<CSharpInsightItem> items;

  public CSharpInsightProvider()
  {
    this.startOffset = 0;
    this.items = (ICollection<CSharpInsightItem>) new CSharpInsightItem[0];
  }

  public CSharpInsightProvider(int startOffset, List<CSharpInsightItem> items)
  {
    if (startOffset < 0)
      throw new ArgumentOutOfRangeException(nameof (startOffset));
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    this.startOffset = 0;
    this.items = (ICollection<CSharpInsightItem>) items;
  }

  public int StartOffset => this.startOffset;

  public ICollection<CSharpInsightItem> Items => this.items;

  int IParameterDataProvider.Count => this.items.Count;

  int IParameterDataProvider.StartOffset => this.startOffset;

  string IParameterDataProvider.GetHeading(
    int overload,
    string[] parameterDescription,
    int currentParameter)
  {
    throw new NotSupportedException();
  }

  string IParameterDataProvider.GetDescription(int overload, int currentParameter)
  {
    throw new NotSupportedException();
  }

  string IParameterDataProvider.GetParameterDescription(int overload, int paramIndex)
  {
    throw new NotSupportedException();
  }

  string IParameterDataProvider.GetParameterName(int overload, int currentParameter)
  {
    throw new NotSupportedException();
  }

  int IParameterDataProvider.GetParameterCount(int overload) => throw new NotSupportedException();

  bool IParameterDataProvider.AllowParameterList(int overload) => throw new NotSupportedException();
}
