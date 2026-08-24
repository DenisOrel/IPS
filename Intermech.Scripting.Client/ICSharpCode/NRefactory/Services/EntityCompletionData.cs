// Decompiled with JetBrains decompiler
// Type: ICSharpCode.NRefactory.Services.EntityCompletionData
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using ICSharpCode.NRefactory.Completion;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Editor;
using ICSharpCode.NRefactory.TypeSystem;
using System;
using System.Diagnostics;

#nullable disable
namespace ICSharpCode.NRefactory.Services;

internal class EntityCompletionData : CompletionData, IEntityCompletionData, ICompletionData
{
  private readonly IEntity entity;
  private readonly ConversionHelper textConversionHelper;

  public EntityCompletionData(IEntity entity, ConversionHelper textConversionHelper)
  {
    if (entity == null)
      throw new ArgumentNullException(nameof (entity));
    if (textConversionHelper == null)
      throw new ArgumentNullException(nameof (textConversionHelper));
    this.entity = entity;
    this.textConversionHelper = textConversionHelper;
    this.ItemType = CompletionItemTypes.GetItemType(entity);
    this.DisplayText = entity.Name;
    CSharpAmbience ambience = entity is ITypeDefinition ? textConversionHelper.TypeAmbiance : textConversionHelper.NameOnlyAmbiance;
    this.CompletionText = textConversionHelper.ConvertSymbolToPlainText((ISymbol) entity, ambience);
    this.DescriptionProvider = this.CreateDescriptionProvider();
  }

  public IEntity Entity
  {
    [DebuggerStepThrough] get => this.entity;
  }

  private Lazy<string> CreateDescriptionProvider()
  {
    return new Lazy<string>((Func<string>) (() => EntityCompletionData.CreateDescription(this.entity, this.GetOverloadsCount(), this.textConversionHelper)));
  }

  private static string CreateDescription(
    IEntity entity,
    int overloadsCount,
    ConversionHelper textConversionHelper)
  {
    TextBuilder textBuilder = new TextBuilder();
    textBuilder.Append(EntityCompletionData.GetText(entity, textConversionHelper));
    if (overloadsCount != 0)
    {
      textBuilder.Append(" ");
      textBuilder.Append($"(+{overloadsCount} overloads)");
    }
    if (entity.Documentation != null && entity.Documentation.Xml.TextLength != 0)
    {
      ITextSource xml = entity.Documentation.Xml;
      string plainText = textConversionHelper.ConvertDocumentationToPlainText(xml);
      if (!string.IsNullOrEmpty(plainText))
      {
        textBuilder.AppendLine();
        textBuilder.AppendLine();
        textBuilder.Append(plainText);
      }
    }
    return textBuilder.ToString();
  }

  private static string GetText(IEntity entity, ConversionHelper textConversionHelper)
  {
    CSharpAmbience ambience = entity is ITypeDefinition ? textConversionHelper.TypeAmbiance : textConversionHelper.StandardAmbiance;
    if (entity is IMethod)
    {
      IMethod reducedFrom = ((IMethod) entity).ReducedFrom;
      if (reducedFrom != null)
        entity = (IEntity) reducedFrom;
    }
    return textConversionHelper.ConvertSymbolToPlainText((ISymbol) entity, ambience);
  }
}
