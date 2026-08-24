// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.IScriptWindowCollection
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Mvp.Components;
using System;

#nullable disable
namespace Intermech.Scripting.ScriptPad;

internal interface IScriptWindowCollection : IWindowCollection<IScriptWindow>
{
  bool CloseableWindows { get; set; }

  event EventHandler WindowClosing;

  event EventHandler WindowClosed;
}
