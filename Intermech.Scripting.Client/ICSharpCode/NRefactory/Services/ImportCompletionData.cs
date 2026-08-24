// Decompiled with JetBrains decompiler
// Type: ICSharpCode.NRefactory.Services.ImportCompletionData
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Refactoring;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.CSharp.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem;
using System;

#nullable disable
namespace ICSharpCode.NRefactory.Services;

internal class ImportCompletionData : EntityCompletionData
{
  private string insertUsing;
  private string insertionText;

  public ImportCompletionData(
    ITypeDefinition typeDef,
    ConversionHelper textConversionHelper,
    CSharpTypeResolveContext contextAtCaret,
    bool useFullName)
    : base((IEntity) typeDef, textConversionHelper)
  {
    string name = ((IEntity) typeDef).Name;
    string namespaceText = typeDef.Namespace;
    this.DescriptionProvider = new Lazy<string>((Func<string>) (() => $"using {namespaceText};"));
    if (useFullName)
    {
      this.insertionText = new TypeSystemAstBuilder(new CSharpResolver(contextAtCaret)).ConvertType((IType) typeDef).ToString((CSharpFormattingOptions) null);
    }
    else
    {
      this.insertionText = name;
      this.insertUsing = namespaceText;
    }
  }
}
