// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor.DesignTimeTextEditorAdapter
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common.DesignTime;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor;

internal sealed class DesignTimeTextEditorAdapter : ITextEditor
{
  private TextEditorViewModel textEditorVM;
  private EditableTextDocumentAdapter documentAdapter;

  public DesignTimeTextEditorAdapter(TextEditorViewModel textEditorVM)
  {
    this.textEditorVM = textEditorVM;
    this.documentAdapter = new EditableTextDocumentAdapter(this.textEditorVM.Document);
  }

  public IEditableTextDocument Document => (IEditableTextDocument) this.documentAdapter;

  public int CaretOffset => this.textEditorVM.CaretOffset;

  public int SelectionStart => this.textEditorVM.SelectionStart;

  public int SelectionLength => this.textEditorVM.SelectionLength;
}
