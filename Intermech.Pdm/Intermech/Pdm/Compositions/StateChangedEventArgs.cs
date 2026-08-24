// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.StateChangedEventArgs
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Pdm;

#nullable disable
namespace Intermech.Pdm.Compositions;

public class StateChangedEventArgs
{
  public BackgroundState State;
  public long PartIdCompleted;
  public int Percent;

  public StateChangedEventArgs(BackgroundState state) => this.State = state;

  public StateChangedEventArgs(BackgroundState state, int percent)
    : this(state)
  {
    this.Percent = percent;
  }

  public StateChangedEventArgs(BackgroundState state, int percent, long partIdCompleted)
    : this(state, percent)
  {
    this.PartIdCompleted = partIdCompleted;
  }
}
