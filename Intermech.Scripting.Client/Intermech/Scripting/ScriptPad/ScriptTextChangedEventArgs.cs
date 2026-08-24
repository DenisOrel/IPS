// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.ScriptTextChangedEventArgs
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common;
using System;

#nullable disable
namespace Intermech.Scripting.ScriptPad;

public class ScriptTextChangedEventArgs : EventArgs
{
  public ScriptTextChangedEventArgs(ScriptTextChange textChange)
  {
    this.TextChange = textChange != null ? textChange : throw new ArgumentNullException(nameof (textChange));
  }

  public ScriptTextChange TextChange { get; }
}
