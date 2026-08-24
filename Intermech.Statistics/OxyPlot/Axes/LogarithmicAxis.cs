// Decompiled with JetBrains decompiler
// Type: OxyPlot.Axes.LogarithmicAxis
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Axes;

public class LogarithmicAxis : Axis
{
  public LogarithmicAxis()
  {
    this.PowerPadding = true;
    this.Base = 10.0;
    this.FilterMinValue = 0.0;
  }

  public double Base { get; set; }

  public bool PowerPadding { get; set; }

  public override void CoerceActualMaxMin()
  {
    if (double.IsNaN(this.ActualMinimum) || double.IsInfinity(this.ActualMinimum))
      this.ActualMinimum = 1.0;
    if (this.ActualMinimum <= 0.0)
      this.ActualMinimum = 1.0;
    if (this.ActualMaximum <= this.ActualMinimum)
      this.ActualMaximum = this.ActualMinimum * 100.0;
    base.CoerceActualMaxMin();
  }

  public override void GetTickValues(
    out IList<double> majorLabelValues,
    out IList<double> majorTickValues,
    out IList<double> minorTickValues)
  {
    if (this.ActualMinimum <= 0.0)
      this.ActualMinimum = 0.1;
    double num1 = Math.Log(this.Base);
    int y1 = (int) Math.Floor(Math.Log(this.ActualMinimum) / num1);
    int y2 = (int) Math.Ceiling(Math.Log(this.ActualMaximum) / num1);
    double num2 = Math.Pow(this.Base, (double) y1);
    double num3 = Math.Pow(this.Base, (double) y2);
    double num4 = Math.Round(num2, 10);
    double num5 = Math.Round(num3, 10);
    if (num4 <= 0.0)
      num4 = num2;
    double d = num4;
    majorTickValues = (IList<double>) new List<double>();
    minorTickValues = (IList<double>) new List<double>();
    double num6 = this.ActualMinimum * 1E-06;
    double num7 = this.ActualMaximum * 1E-06;
    while (d <= num5 + num7)
    {
      if (d >= this.ActualMinimum - num6 && d <= this.ActualMaximum + num7)
        majorTickValues.Add(d);
      for (int index = 1; (double) index < this.Base; ++index)
      {
        double num8 = d * (double) (index + 1);
        if (num8 <= num5 + double.Epsilon && num8 <= this.ActualMaximum)
        {
          if (num8 >= this.ActualMinimum && num8 <= this.ActualMaximum)
            minorTickValues.Add(num8);
        }
        else
          break;
      }
      d *= this.Base;
      if (double.IsInfinity(d) || d < double.Epsilon || double.IsNaN(d))
        break;
    }
    if (majorTickValues.Count < 2)
      base.GetTickValues(out majorLabelValues, out majorTickValues, out minorTickValues);
    else
      majorLabelValues = majorTickValues;
  }

  public override bool IsXyAxis() => true;

  public override void Pan(ScreenPoint ppt, ScreenPoint cpt)
  {
    if (!this.IsPanEnabled)
      return;
    bool flag = this.IsHorizontal();
    double num1 = this.InverseTransform(flag ? ppt.X : ppt.Y);
    double num2 = this.InverseTransform(flag ? cpt.X : cpt.Y);
    if (Math.Abs(num2) < double.Epsilon)
      return;
    double actualMinimum = this.ActualMinimum;
    double actualMaximum = this.ActualMaximum;
    double num3 = num1 / num2;
    double num4 = this.ActualMinimum * num3;
    double num5 = this.ActualMaximum * num3;
    if (num4 < this.AbsoluteMinimum)
    {
      num4 = this.AbsoluteMinimum;
      num5 = num4 * this.ActualMaximum / this.ActualMinimum;
    }
    if (num5 > this.AbsoluteMaximum)
    {
      num5 = this.AbsoluteMaximum;
      num4 = num5 * this.ActualMinimum / this.ActualMaximum;
    }
    this.ViewMinimum = num4;
    this.ViewMaximum = num5;
    this.OnAxisChanged(new AxisChangedEventArgs(AxisChangeTypes.Pan, this.ActualMinimum - actualMinimum, this.ActualMaximum - actualMaximum));
  }

  public override double InverseTransform(double sx) => Math.Exp(sx / this.Scale + this.Offset);

  public override double Transform(double x)
  {
    return x <= 0.0 ? -1.0 : (Math.Log(x) - this.Offset) * this.Scale;
  }

  public override void ZoomAt(double factor, double x)
  {
    if (!this.IsZoomEnabled)
      return;
    double actualMinimum = this.ActualMinimum;
    double actualMaximum = this.ActualMaximum;
    double num1 = this.PreTransform(x);
    double num2 = this.PreTransform(this.ActualMinimum) - num1;
    double num3 = this.PreTransform(this.ActualMaximum) - num1;
    double val1_1 = this.PostInverseTransform(num2 / factor + num1);
    double val1_2 = this.PostInverseTransform(num3 / factor + num1);
    double num4 = Math.Max(val1_1, this.AbsoluteMinimum);
    double absoluteMaximum = this.AbsoluteMaximum;
    double num5 = Math.Min(val1_2, absoluteMaximum);
    this.ViewMinimum = num4;
    this.ViewMaximum = num5;
    this.UpdateActualMaxMin();
    this.OnAxisChanged(new AxisChangedEventArgs(AxisChangeTypes.Zoom, this.ActualMinimum - actualMinimum, this.ActualMaximum - actualMaximum));
  }

  internal override void UpdateActualMaxMin()
  {
    if (this.PowerPadding)
    {
      double num1 = Math.Log(this.Base);
      int num2 = (int) Math.Floor(Math.Log(this.ActualMinimum) / num1);
      int num3 = (int) Math.Ceiling(Math.Log(this.ActualMaximum) / num1);
      if (!double.IsNaN(this.ActualMinimum))
        this.ActualMinimum = Math.Round(Math.Exp((double) num2 * num1), 14);
      if (!double.IsNaN(this.ActualMaximum))
        this.ActualMaximum = Math.Round(Math.Exp((double) num3 * num1), 14);
    }
    base.UpdateActualMaxMin();
  }

  protected override double PostInverseTransform(double x) => Math.Exp(x);

  protected override double PreTransform(double x) => x <= 0.0 ? 0.0 : Math.Log(x);
}
