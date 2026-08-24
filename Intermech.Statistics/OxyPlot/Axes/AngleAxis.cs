// Decompiled with JetBrains decompiler
// Type: OxyPlot.Axes.AngleAxis
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Axes;

public class AngleAxis : LinearAxis
{
  public AngleAxis()
  {
    this.Position = AxisPosition.None;
    this.TickStyle = TickStyle.None;
    this.IsPanEnabled = false;
    this.IsZoomEnabled = false;
    this.MajorGridlineStyle = LineStyle.Solid;
    this.MinorGridlineStyle = LineStyle.Solid;
    this.StartAngle = 0.0;
    this.EndAngle = 360.0;
  }

  public double StartAngle { get; set; }

  public double EndAngle { get; set; }

  public override void GetTickValues(
    out IList<double> majorLabelValues,
    out IList<double> majorTickValues,
    out IList<double> minorTickValues)
  {
    double from = this.StartAngle / this.Scale;
    double to = this.EndAngle / this.Scale;
    minorTickValues = Axis.CreateTickValues(from, to, this.ActualMinorStep);
    majorTickValues = Axis.CreateTickValues(from, to, this.ActualMajorStep);
    majorLabelValues = Axis.CreateTickValues(this.Minimum, this.Maximum, this.ActualMajorStep);
  }

  public override DataPoint InverseTransform(double x, double y, Axis yaxis)
  {
    throw new InvalidOperationException("Angle axis should always be the y-axis.");
  }

  public override bool IsXyAxis() => false;

  public override void Render(IRenderContext rc, int pass)
  {
    new AngleAxisRenderer(rc, this.PlotModel).Render((Axis) this, pass);
  }

  public override ScreenPoint Transform(double x, double y, Axis yaxis)
  {
    throw new InvalidOperationException("Angle axis should always be the y-axis.");
  }

  internal override void UpdateTransform(OxyRect bounds)
  {
    double left = bounds.Left;
    double right = bounds.Right;
    double bottom = bounds.Bottom;
    double top = bounds.Top;
    this.ScreenMin = new ScreenPoint(left, top);
    this.ScreenMax = new ScreenPoint(right, bottom);
    double newScale = (this.EndAngle - this.StartAngle) / (this.ActualMaximum - this.ActualMinimum);
    double newOffset = this.ActualMinimum - this.StartAngle / newScale;
    this.SetTransform(newScale, newOffset);
  }
}
