// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor.CodeCompletionTextComparer
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using ICSharpCode.AvalonEdit.CodeCompletion;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor;

internal sealed class CodeCompletionTextComparer : IComparer<ICompletionData>
{
  public int Compare(ICompletionData x, ICompletionData y) => x.Text.CompareTo(y.Text);
}
