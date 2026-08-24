// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.IErrorsView
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.ScriptPad;

internal interface IErrorsView
{
  void SetErrors(ICollection<ScriptProjectErrorRecord> errors);

  ScriptProjectErrorRecord TryGetSelectedError();

  event EventHandler ShowSelectedError;
}
