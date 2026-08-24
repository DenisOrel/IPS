// Decompiled with JetBrains decompiler
// Type: OxyPlot.Annotations.LineAnnotation
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OxyPlot.Annotations;

public class LineAnnotation : PathAnnotation
{
  public LineAnnotation() => this.Type = LineAnnotationType.LinearEquation;

  public double Intercept { get; set; }

  public double Slope { get; set; }

  public LineAnnotationType Type { get; set; }

  public double X { get; set; }

  public double Y { get; set; }

  protected override IList<ScreenPoint> GetScreenPoints()
  {
    this.Aliased = false;
    Func<double, double> func1 = (Func<double, double>) null;
    Func<double, double> func2 = (Func<double, double>) null;
    switch (this.Type)
    {
      case LineAnnotationType.Horizontal:
        func1 = (Func<double, double>) (x => this.Y);
        break;
      case LineAnnotationType.Vertical:
        func2 = (Func<double, double>) (y => this.X);
        break;
      default:
        func1 = (Func<double, double>) (x => this.Slope * x + this.Intercept);
        break;
    }
    List<DataPoint> source = new List<DataPoint>();
    if ((!(this.XAxis is LinearAxis) ? 1 : (!(this.YAxis is LinearAxis) ? 1 : 0)) == 0)
    {
      if (func1 != null)
      {
        source.Add(new DataPoint(this.ActualMinimumX, func1(this.ActualMinimumX)));
        source.Add(new DataPoint(this.ActualMaximumX, func1(this.ActualMaximumX)));
      }
      else
      {
        source.Add(new DataPoint(func2(this.ActualMinimumY), this.ActualMinimumY));
        source.Add(new DataPoint(func2(this.ActualMaximumY), this.ActualMaximumY));
      }
      if (this.Type == LineAnnotationType.Horizontal || this.Type == LineAnnotationType.Vertical)
        this.Aliased = true;
    }
    else if (func1 != null)
    {
      double actualMinimumX = this.ActualMinimumX;
      double num = (this.ActualMaximumX - this.ActualMinimumX) / 100.0;
      while (true)
      {
        source.Add(new DataPoint(actualMinimumX, func1(actualMinimumX)));
        if (actualMinimumX <= this.ActualMaximumX)
          actualMinimumX += num;
        else
          break;
      }
    }
    else
    {
      double actualMinimumY = this.ActualMinimumY;
      double num = (this.ActualMaximumY - this.ActualMinimumY) / 100.0;
      while (true)
      {
        source.Add(new DataPoint(func2(actualMinimumY), actualMinimumY));
        if (actualMinimumY <= this.ActualMaximumY)
          actualMinimumY += num;
        else
          break;
      }
    }
    return (IList<ScreenPoint>) source.Select<DataPoint, ScreenPoint>(new Func<DataPoint, ScreenPoint>(((Annotation) this).Transform)).ToList<ScreenPoint>();
  }
}
