// Decompiled with JetBrains decompiler
// Type: ICSharpCode.NRefactory.Services.OverrideCompletionData
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.TypeSystem;

#nullable disable
namespace ICSharpCode.NRefactory.Services;

internal sealed class OverrideCompletionData : EntityCompletionData
{
  public OverrideCompletionData(IMember entity, ConversionHelper textConversionHelper)
    : base((IEntity) entity, textConversionHelper)
  {
    CSharpAmbience overridesAmbiance = textConversionHelper.OverridesAmbiance;
    this.CompletionText = textConversionHelper.ConvertSymbolToPlainText((ISymbol) entity, overridesAmbiance);
  }
}
