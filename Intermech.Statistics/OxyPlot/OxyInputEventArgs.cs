// Decompiled with JetBrains decompiler
// Type: OxyPlot.OxyInputEventArgs
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;

#nullable disable
namespace OxyPlot;

public abstract class OxyInputEventArgs : EventArgs
{
  public bool Handled { get; set; }

  public OxyModifierKeys ModifierKeys { get; set; }

  public bool IsAltDown => (this.ModifierKeys & OxyModifierKeys.Alt) == OxyModifierKeys.Alt;

  public bool IsControlDown
  {
    get => (this.ModifierKeys & OxyModifierKeys.Control) == OxyModifierKeys.Control;
  }

  public bool IsShiftDown => (this.ModifierKeys & OxyModifierKeys.Shift) == OxyModifierKeys.Shift;
}
