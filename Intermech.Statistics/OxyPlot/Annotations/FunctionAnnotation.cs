// Decompiled with JetBrains decompiler
// Type: OxyPlot.Annotations.FunctionAnnotation
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OxyPlot.Annotations;

public class FunctionAnnotation : PathAnnotation
{
  public FunctionAnnotation()
  {
    this.Resolution = 400;
    this.Type = FunctionAnnotationType.EquationX;
  }

  public FunctionAnnotationType Type { get; set; }

  public Func<double, double> Equation { get; set; }

  public int Resolution { get; set; }

  protected override IList<ScreenPoint> GetScreenPoints()
  {
    Func<double, double> func1 = (Func<double, double>) null;
    Func<double, double> func2 = (Func<double, double>) null;
    switch (this.Type)
    {
      case FunctionAnnotationType.EquationX:
        func1 = this.Equation;
        break;
      case FunctionAnnotationType.EquationY:
        func2 = this.Equation;
        break;
    }
    List<DataPoint> source = new List<DataPoint>();
    if (func1 != null)
    {
      double actualMinimumX = this.ActualMinimumX;
      double num = (this.ActualMaximumX - this.ActualMinimumX) / (double) this.Resolution;
      while (true)
      {
        source.Add(new DataPoint(actualMinimumX, func1(actualMinimumX)));
        if (actualMinimumX <= this.ActualMaximumX)
          actualMinimumX += num;
        else
          break;
      }
    }
    else if (func2 != null)
    {
      double actualMinimumY = this.ActualMinimumY;
      double num = (this.ActualMaximumY - this.ActualMinimumY) / (double) this.Resolution;
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
