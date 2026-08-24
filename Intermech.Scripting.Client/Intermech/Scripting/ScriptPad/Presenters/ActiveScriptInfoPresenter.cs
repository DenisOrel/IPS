// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Presenters.ActiveScriptInfoPresenter
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Presenters;

internal sealed class ActiveScriptInfoPresenter : ActiveScriptChildPresenter
{
  private Func<OpenScriptData, string, string> createWindowCaption;

  public ActiveScriptInfoPresenter(
    IDEPresenter idePresenter,
    OpenScriptData script,
    Func<OpenScriptData, string, string> createWindowCaption)
    : base(idePresenter, script)
  {
    this.createWindowCaption = createWindowCaption;
  }

  private string GetOriginalWindowText() => this.Script.Window.Text;

  protected override void OnAttachView()
  {
    base.OnAttachView();
    this.Script.ModifiedChanged += new EventHandler(this.OnDocumentModifiedChanged);
    this.CodeEditorControl.CaretPositionChanged += new EventHandler(this.OnCodeEditorCaretPositionChanged);
    this.UpdateLanguageInfo();
    this.UpdateWindowCaption();
    this.UpdateCaretPosition();
  }

  protected override void OnDetachView(bool fullDetach)
  {
    this.Script.ModifiedChanged -= new EventHandler(this.OnDocumentModifiedChanged);
    this.CodeEditorControl.CaretPositionChanged -= new EventHandler(this.OnCodeEditorCaretPositionChanged);
    if (fullDetach)
      this.ClearViewState();
    base.OnDetachView(fullDetach);
  }

  private void ClearViewState()
  {
    this.IDEView.ShowScriptLanguage(string.Empty);
    this.IDEView.ShowScriptEncoding(string.Empty);
    this.IDEView.ShowScriptCodeEditorCaretPosition(string.Empty, string.Empty);
  }

  private void UpdateLanguageInfo()
  {
    this.IDEView.ShowScriptLanguage(this.Script.LanguageInfo.Name);
    this.IDEView.ShowScriptEncoding(this.Script.Encoding.EncodingName);
  }

  private void UpdateCaretPosition()
  {
    TextCaretPosition caretPosition = this.CodeEditorControl.GetCaretPosition();
    IIDEView ideView = this.IDEView;
    int num = caretPosition.Line;
    string line = num.ToString();
    num = caretPosition.Column;
    string column = num.ToString();
    ideView.ShowScriptCodeEditorCaretPosition(line, column);
  }

  private void UpdateWindowCaption()
  {
    this.Script.Window.Text = this.createWindowCaption(this.Script, string.Empty);
  }

  private void OnCodeEditorCaretPositionChanged(object sender, EventArgs e)
  {
    this.UpdateCaretPosition();
  }

  private void OnDocumentModifiedChanged(object sender, EventArgs e) => this.UpdateWindowCaption();
}
