// Decompiled with JetBrains decompiler
// Type: ICSharpCode.NRefactory.Services.VariableCompletionData
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using ICSharpCode.NRefactory.Completion;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.TypeSystem;
using Intermech.Scripting.Common.DesignTime;
using System;
using System.Diagnostics;

#nullable disable
namespace ICSharpCode.NRefactory.Services;

internal sealed class VariableCompletionData : 
  CompletionData,
  IVariableCompletionData,
  ICompletionData
{
  private readonly IVariable variable;

  public VariableCompletionData(IVariable variable, ConversionHelper textConversionHelper)
  {
    if (variable == null)
      throw new ArgumentNullException(nameof (variable));
    if (textConversionHelper == null)
      throw new ArgumentNullException(nameof (textConversionHelper));
    this.variable = variable;
    this.ItemType = CodeCompletionItemType.Field;
    this.CompletionText = variable.Name;
    this.DisplayText = variable.Name;
    CSharpAmbience ambience = textConversionHelper.NameOnlyAmbiance;
    this.DescriptionProvider = new Lazy<string>((Func<string>) (() => textConversionHelper.ConvertSymbolToPlainText((ISymbol) variable, ambience)));
  }

  public IVariable Variable
  {
    [DebuggerStepThrough] get => this.variable;
  }
}
