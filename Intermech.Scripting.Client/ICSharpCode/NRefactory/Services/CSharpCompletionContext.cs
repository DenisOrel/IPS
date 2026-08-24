// Decompiled with JetBrains decompiler
// Type: ICSharpCode.NRefactory.Services.CSharpCompletionContext
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Completion;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.CSharp.TypeSystem;
using ICSharpCode.NRefactory.Editor;
using ICSharpCode.NRefactory.TypeSystem;

#nullable disable
namespace ICSharpCode.NRefactory.Services;

internal sealed class CSharpCompletionContext
{
  public CSharpCompletionContext(IDocument document, int offset, IProjectContent projectContent)
  {
    this.Document = document;
    this.Offset = offset;
    SyntaxTree syntaxTree = new CSharpParser().Parse((ITextSource) this.Document, this.Document.FileName);
    syntaxTree.Freeze();
    CSharpUnresolvedFile typeSystem = syntaxTree.ToTypeSystem();
    this.ProjectContent = projectContent.AddOrUpdateFiles((IUnresolvedFile) typeSystem);
    this.Compilation = this.ProjectContent.CreateCompilation();
    TextLocation location = this.Document.GetLocation(this.Offset);
    this.Resolver = typeSystem.GetResolver(this.Compilation, location);
    this.TypeResolveContextAtCaret = typeSystem.GetTypeResolveContext(this.Compilation, location);
    this.CompletionContextProvider = (ICompletionContextProvider) new DefaultCompletionContextProvider(this.Document, typeSystem);
    this.TextConversionHelper = new ConversionHelper();
  }

  public int Offset { get; private set; }

  public IDocument Document { get; private set; }

  public ICompilation Compilation { get; private set; }

  public IProjectContent ProjectContent { get; private set; }

  public CSharpResolver Resolver { get; private set; }

  public CSharpTypeResolveContext TypeResolveContextAtCaret { get; private set; }

  public ICompletionContextProvider CompletionContextProvider { get; private set; }

  public ConversionHelper TextConversionHelper { get; private set; }
}
