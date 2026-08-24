// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor.FindReplaceTextEditorAdapter
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using Intermech.UI.Wpf.Controls;
using System;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor;

internal sealed class FindReplaceTextEditorAdapter : 
  Intermech.UI.Wpf.Controls.FindReplaceTextEditorAdapter,
  IFindReplaceTextEditor
{
  private TextEditor editorControl;

  public FindReplaceTextEditorAdapter(TextEditor editorControl)
  {
    this.editorControl = editorControl != null ? editorControl : throw new ArgumentNullException(nameof (editorControl));
  }

  public string Text => this.editorControl.Text;

  public int SelectionStart => this.editorControl.SelectionStart;

  public int SelectionLength => this.editorControl.SelectionLength;

  public void BeginChange() => this.editorControl.BeginChange();

  public void EndChange() => this.editorControl.EndChange();

  public void Select(int start, int length)
  {
    this.editorControl.Select(start, length);
    TextLocation location = this.editorControl.Document.GetLocation(start);
    this.editorControl.ScrollTo(location.Line, location.Column);
  }

  public void Replace(int start, int length, string ReplaceWith)
  {
    this.editorControl.Document.Replace(start, length, ReplaceWith);
  }
}
