// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor.TextEditorViewModel
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using ICSharpCode.AvalonEdit.Document;
using Intermech.UI;
using System;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor;

internal sealed class TextEditorViewModel : ViewModel
{
  private TextDocument document;
  private bool isReadOnly;
  private int fontSize;
  private string fontFamily;
  private int caretOffset;
  private int selectionStart;
  private int selectionLength;
  private int caretLine;
  private int caretColumn;
  private ContextMenuViewModel<CodeEditorMenuItemViewModel> contextMenuVM;
  private WeakReference<DesignTimeTextEditorAdapter> designTimeTextEditor;

  public TextEditorViewModel()
  {
    this.document = new TextDocument();
    this.fontFamily = "Consolas";
    this.fontSize = 16 /*0x10*/;
    this.caretOffset = 0;
    this.caretLine = 1;
    this.caretColumn = 1;
    this.selectionStart = 0;
    this.selectionLength = 0;
    this.contextMenuVM = new ContextMenuViewModel<CodeEditorMenuItemViewModel>();
    this.designTimeTextEditor = new WeakReference<DesignTimeTextEditorAdapter>((DesignTimeTextEditorAdapter) null);
  }

  public TextDocument Document => this.document;

  public bool IsReadOnly
  {
    get => this.isReadOnly;
    set
    {
      if (this.isReadOnly == value)
        return;
      this.isReadOnly = value;
      this.RaisePropertyChanged(nameof (IsReadOnly));
    }
  }

  public int FontSize
  {
    get => this.fontSize;
    set
    {
      if (this.fontSize == value)
        return;
      this.fontSize = value;
      this.RaisePropertyChanged(nameof (FontSize));
    }
  }

  public string FontFamily
  {
    get => this.fontFamily;
    set
    {
      if (!(this.fontFamily != value))
        return;
      this.fontFamily = value;
      this.RaisePropertyChanged(nameof (FontFamily));
    }
  }

  public int CaretOffset => this.caretOffset;

  public int CaretLine => this.caretLine;

  public int CaretColumn => this.caretColumn;

  public int SelectionStart => this.selectionStart;

  public int SelectionLength => this.selectionLength;

  public ContextMenuViewModel<CodeEditorMenuItemViewModel> ContextMenu => this.contextMenuVM;

  public DesignTimeTextEditorAdapter AsDesignTimeTextEditor()
  {
    DesignTimeTextEditorAdapter target;
    if (!this.designTimeTextEditor.TryGetTarget(out target))
    {
      target = new DesignTimeTextEditorAdapter(this);
      this.designTimeTextEditor.SetTarget(target);
    }
    return target;
  }

  public void SetDocumentFileName(string fileName)
  {
    this.document.FileName = fileName != null ? fileName : throw new ArgumentNullException(nameof (fileName));
  }

  internal void UpdateCaretPosition(int offset, int line, int column)
  {
    this.caretOffset = offset;
    this.caretLine = line;
    this.caretColumn = column;
  }

  internal void UpdateSelection(int start, int length)
  {
    this.selectionStart = start;
    this.selectionLength = length;
  }
}
