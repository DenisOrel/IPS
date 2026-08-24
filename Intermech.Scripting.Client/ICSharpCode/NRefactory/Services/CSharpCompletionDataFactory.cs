// Decompiled with JetBrains decompiler
// Type: ICSharpCode.NRefactory.Services.CSharpCompletionDataFactory
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using ICSharpCode.NRefactory.Completion;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Completion;
using ICSharpCode.NRefactory.CSharp.TypeSystem;
using ICSharpCode.NRefactory.Editor;
using ICSharpCode.NRefactory.TypeSystem;
using Intermech.Scripting.Common.DesignTime;
using System;
using System.Collections.Generic;

#nullable disable
namespace ICSharpCode.NRefactory.Services;

internal sealed class CSharpCompletionDataFactory : 
  ICompletionDataFactory,
  IParameterCompletionDataFactory
{
  private readonly CSharpTypeResolveContext contextAtCaret;
  private readonly CSharpCompletionContext context;

  public CSharpCompletionDataFactory(
    CSharpTypeResolveContext contextAtCaret,
    CSharpCompletionContext context)
  {
    this.contextAtCaret = contextAtCaret;
    this.context = context;
  }

  ICompletionData ICompletionDataFactory.CreateEntityCompletionData(IEntity entity)
  {
    return (ICompletionData) new EntityCompletionData(entity, this.context.TextConversionHelper);
  }

  ICompletionData ICompletionDataFactory.CreateEntityCompletionData(IEntity entity, string text)
  {
    EntityCompletionData entityCompletionData = new EntityCompletionData(entity, this.context.TextConversionHelper);
    entityCompletionData.CompletionText = text;
    entityCompletionData.DisplayText = text;
    return (ICompletionData) entityCompletionData;
  }

  ICompletionData ICompletionDataFactory.CreateTypeCompletionData(
    IType type,
    bool showFullName,
    bool isInAttributeContext,
    bool addForTypeCreation)
  {
    ITypeDefinition definition = type.GetDefinition();
    if (definition != null)
      return (ICompletionData) new EntityCompletionData((IEntity) definition, this.context.TextConversionHelper);
    string text = showFullName ? type.FullName : type.Name;
    if (isInAttributeContext && text.EndsWith("Attribute") && text.Length > "Attribute".Length)
      text = text.Substring(0, text.Length - "Attribute".Length);
    return (ICompletionData) new CompletionData(text);
  }

  ICompletionData ICompletionDataFactory.CreateMemberCompletionData(IType type, IEntity member)
  {
    return (ICompletionData) new CompletionData($"{type.Name}.{member.Name}")
    {
      ItemType = CompletionItemTypes.GetItemType(member)
    };
  }

  ICompletionData ICompletionDataFactory.CreateLiteralCompletionData(
    string title,
    string description,
    string insertText)
  {
    return (ICompletionData) new CompletionData(title)
    {
      DescriptionProvider = new Lazy<string>((Func<string>) (() => description)),
      CompletionText = (insertText ?? title),
      ItemType = CodeCompletionItemType.Literal,
      Priority = 2.0
    };
  }

  ICompletionData ICompletionDataFactory.CreateNamespaceCompletionData(INamespace name)
  {
    return (ICompletionData) new CompletionData(name.Name)
    {
      ItemType = CodeCompletionItemType.Namespace
    };
  }

  ICompletionData ICompletionDataFactory.CreateVariableCompletionData(IVariable variable)
  {
    return (ICompletionData) new VariableCompletionData(variable, this.context.TextConversionHelper);
  }

  ICompletionData ICompletionDataFactory.CreateVariableCompletionData(ITypeParameter parameter)
  {
    return (ICompletionData) new CompletionData(parameter.Name);
  }

  ICompletionData ICompletionDataFactory.CreateEventCreationCompletionData(
    string varName,
    IType delegateType,
    IEvent evt,
    string parameterDefinition,
    IUnresolvedMember currentMember,
    IUnresolvedTypeDefinition currentType)
  {
    return (ICompletionData) new CompletionData("TODO: event creation");
  }

  ICompletionData ICompletionDataFactory.CreateNewOverrideCompletionData(
    int declarationBegin,
    IUnresolvedTypeDefinition type,
    IMember member)
  {
    return (ICompletionData) new OverrideCompletionData(member, this.context.TextConversionHelper);
  }

  ICompletionData ICompletionDataFactory.CreateNewPartialCompletionData(
    int declarationBegin,
    IUnresolvedTypeDefinition type,
    IUnresolvedMember m)
  {
    return (ICompletionData) new CompletionData("TODO: partial completion");
  }

  IEnumerable<ICompletionData> ICompletionDataFactory.CreateCodeTemplateCompletionData()
  {
    yield break;
  }

  IEnumerable<ICompletionData> ICompletionDataFactory.CreatePreProcessorDefinesCompletionData()
  {
    yield return (ICompletionData) new CompletionData("DEBUG");
    yield return (ICompletionData) new CompletionData("TEST");
  }

  ICompletionData ICompletionDataFactory.CreateImportCompletionData(
    IType type,
    bool useFullName,
    bool addForTypeCreation)
  {
    return (ICompletionData) new ImportCompletionData(type.GetDefinition() ?? throw new InvalidOperationException("Should never happen"), this.context.TextConversionHelper, this.contextAtCaret, useFullName);
  }

  ICompletionData ICompletionDataFactory.CreateFormatItemCompletionData(
    string format,
    string description,
    object example)
  {
    throw new NotImplementedException();
  }

  ICompletionData ICompletionDataFactory.CreateXmlDocCompletionData(
    string tag,
    string description,
    string tagInsertionText)
  {
    throw new NotImplementedException();
  }

  private IParameterDataProvider CreateMethodDataProvider(
    int startOffset,
    IEnumerable<IParameterizedMember> methods)
  {
    List<CSharpInsightItem> items = new List<CSharpInsightItem>();
    foreach (IParameterizedMember method in methods)
    {
      if (method != null)
        items.Add(new CSharpInsightItem(this.CreateMemberInsightText(method), this.CreateMemberInsightDescriptionProvider(method)));
    }
    return (IParameterDataProvider) new CSharpInsightProvider(startOffset, items);
  }

  IParameterDataProvider IParameterCompletionDataFactory.CreateConstructorProvider(
    int startOffset,
    IType type)
  {
    return this.CreateMethodDataProvider(startOffset, (IEnumerable<IParameterizedMember>) type.GetConstructors());
  }

  IParameterDataProvider IParameterCompletionDataFactory.CreateConstructorProvider(
    int startOffset,
    IType type,
    AstNode thisInitializer)
  {
    return this.CreateMethodDataProvider(startOffset, (IEnumerable<IParameterizedMember>) type.GetConstructors());
  }

  IParameterDataProvider IParameterCompletionDataFactory.CreateMethodDataProvider(
    int startOffset,
    IEnumerable<IMethod> methods)
  {
    return this.CreateMethodDataProvider(startOffset, (IEnumerable<IParameterizedMember>) methods);
  }

  IParameterDataProvider IParameterCompletionDataFactory.CreateDelegateDataProvider(
    int startOffset,
    IType type)
  {
    return this.CreateMethodDataProvider(startOffset, (IEnumerable<IParameterizedMember>) new IMethod[1]
    {
      type.GetDelegateInvokeMethod()
    });
  }

  public IParameterDataProvider CreateIndexerParameterDataProvider(
    int startOffset,
    IType type,
    IEnumerable<IProperty> accessibleIndexers,
    AstNode resolvedNode)
  {
    return this.CreateMethodDataProvider(startOffset, (IEnumerable<IParameterizedMember>) accessibleIndexers);
  }

  IParameterDataProvider IParameterCompletionDataFactory.CreateTypeParameterDataProvider(
    int startOffset,
    IEnumerable<IType> types)
  {
    return (IParameterDataProvider) null;
  }

  public IParameterDataProvider CreateTypeParameterDataProvider(
    int startOffset,
    IEnumerable<IMethod> methods)
  {
    return this.CreateMethodDataProvider(startOffset, (IEnumerable<IParameterizedMember>) methods);
  }

  private string CreateMemberInsightText(IParameterizedMember member)
  {
    CSharpAmbience standardAmbiance = this.context.TextConversionHelper.StandardAmbiance;
    return this.context.TextConversionHelper.ConvertSymbolToPlainText((ISymbol) member, standardAmbiance);
  }

  private Lazy<string> CreateMemberInsightDescriptionProvider(IParameterizedMember member)
  {
    if (member.Documentation == null || member.Documentation.Xml.TextLength == 0)
      return CSharpCompletionConsts.EmptyStringProvider;
    ConversionHelper textConversionHelper = this.context.TextConversionHelper;
    ITextSource memberXmlDoc = member.Documentation.Xml;
    return new Lazy<string>((Func<string>) (() => textConversionHelper.ConvertDocumentationToPlainText(memberXmlDoc)));
  }
}
