// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompositionReaderChangeStateEventArgs
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Pdm;
using System;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal sealed class CompositionReaderChangeStateEventArgs : EventArgs
{
  public BackgroundState State { get; private set; }

  public CompositionItem[] Result { get; private set; }

  public Exception ErrorException { get; private set; }

  public CompositionReaderChangeStateEventArgs(BackgroundState state)
    : this(state, (CompositionItem[]) null)
  {
  }

  public CompositionReaderChangeStateEventArgs(BackgroundState state, CompositionItem[] result)
  {
    this.State = state;
    this.Result = result;
  }

  public CompositionReaderChangeStateEventArgs(Exception error)
  {
    this.State = BackgroundState.Error;
    this.ErrorException = error;
  }
}
