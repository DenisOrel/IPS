// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.InMemoryIDESettingsService
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.ScriptPad;

internal sealed class InMemoryIDESettingsService : IDESettingsService
{
  private Dictionary<Tuple<string, string>, Tuple<Type, object>> storage;

  public InMemoryIDESettingsService()
  {
    this.storage = new Dictionary<Tuple<string, string>, Tuple<Type, object>>();
  }

  protected override Tuple<Type, object> DoTryReadParameter(Tuple<string, string> key)
  {
    Tuple<Type, object> tuple;
    return this.storage.TryGetValue(key, out tuple) ? tuple : (Tuple<Type, object>) null;
  }

  protected override void DoWriteParameter(
    Tuple<string, string> key,
    Tuple<Type, object> typeAndValue)
  {
    this.storage[key] = typeAndValue;
  }

  protected override void DoFlush()
  {
  }
}
