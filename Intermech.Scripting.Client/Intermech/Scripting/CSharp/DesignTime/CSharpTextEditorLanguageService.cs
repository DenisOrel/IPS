// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.DesignTime.CSharpTextEditorLanguageService
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common.DesignTime;
using Intermech.Scripting.Common.DesignTime.TextEditorActions;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.CSharp.DesignTime;

internal sealed class CSharpTextEditorLanguageService : ITextEditorLanguageService
{
  private LanguageInfo languageInfo;
  private CSharpLanguageClientHolder languageClientHolder;

  public CSharpTextEditorLanguageService(LanguageInfo languageInfo)
  {
    this.languageInfo = languageInfo != null ? languageInfo : throw new ArgumentNullException(nameof (languageInfo));
    this.languageClientHolder = new CSharpLanguageClientHolder();
  }

  public LanguageInfo LanguageInfo => this.languageInfo;

  public ITextEditorAction TryCreateCommentSelectionAction()
  {
    return (ITextEditorAction) new CommentSelectionAction("//");
  }

  public ITextEditorAction TryCreateUncommentSelectionAction()
  {
    return (ITextEditorAction) new UncommentSelectionAction("//");
  }

  public ITextEditorAction TryCreateFormatIndentsAction()
  {
    return (ITextEditorAction) new CSharpFormatIndentsAction();
  }

  public IList<ITextEditorUIAction> TryCreateContextMenu()
  {
    return (IList<ITextEditorUIAction>) new List<ITextEditorUIAction>()
    {
      (ITextEditorUIAction) new InsertDBAttributeGuidUIAction(),
      (ITextEditorUIAction) new InsertDBObjectTypeGuidUIAction(),
      (ITextEditorUIAction) new InsertDBRelationTypeGuidUIAction()
    };
  }

  public ICodeModel TryCreateCodeModel(Uri scriptId)
  {
    return !(scriptId == (Uri) null) ? (ICodeModel) new CSharpCodeModel(scriptId, this.languageClientHolder) : throw new ArgumentNullException(nameof (scriptId));
  }
}
