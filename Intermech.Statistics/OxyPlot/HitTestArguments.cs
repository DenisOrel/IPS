// Decompiled with JetBrains decompiler
// Type: OxyPlot.HitTestArguments
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot;

public class HitTestArguments
{
  public HitTestArguments(ScreenPoint point, double tolerance)
  {
    this.Point = point;
    this.Tolerance = tolerance;
  }

  public ScreenPoint Point { get; private set; }

  public double Tolerance { get; private set; }
}
