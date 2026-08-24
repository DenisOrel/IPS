// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.IScriptCodeEditorCutCopyPaste
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

#nullable disable
namespace Intermech.Scripting.ScriptPad;

internal interface IScriptCodeEditorCutCopyPaste : IScriptCodeEditorControl
{
  void Cut();

  void Copy();

  void Paste();
}
