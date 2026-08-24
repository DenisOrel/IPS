// Decompiled with JetBrains decompiler
// Type: OxyPlot.Axes.AxisChangedEventArgs
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;

#nullable disable
namespace OxyPlot.Axes;

public class AxisChangedEventArgs : EventArgs
{
  public AxisChangedEventArgs(AxisChangeTypes changeType, double deltaMinimum, double deltaMaximum)
  {
    this.ChangeType = changeType;
    this.DeltaMinimum = deltaMinimum;
    this.DeltaMaximum = deltaMaximum;
  }

  public AxisChangeTypes ChangeType { get; private set; }

  public double DeltaMinimum { get; private set; }

  public double DeltaMaximum { get; private set; }
}
