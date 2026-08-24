// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.FunctionSeries
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;

#nullable disable
namespace OxyPlot.Series;

public class FunctionSeries : LineSeries
{
  public FunctionSeries()
  {
  }

  public FunctionSeries(Func<double, double> f, double x0, double x1, double dx, string title = null)
  {
    this.Title = title;
    for (double x = x0; x <= x1 + dx * 0.5; x += dx)
      this.Points.Add(new DataPoint(x, f(x)));
  }

  public FunctionSeries(Func<double, double> f, double x0, double x1, int n, string title = null)
    : this(f, x0, x1, (x1 - x0) / (double) (n - 1), title)
  {
  }

  public FunctionSeries(
    Func<double, double> fx,
    Func<double, double> fy,
    double t0,
    double t1,
    double dt,
    string title = null)
  {
    this.Title = title;
    for (double num = t0; num <= t1 + dt * 0.5; num += dt)
      this.Points.Add(new DataPoint(fx(num), fy(num)));
  }

  public FunctionSeries(
    Func<double, double> fx,
    Func<double, double> fy,
    double t0,
    double t1,
    int n,
    string title = null)
    : this(fx, fy, t0, t1, (t1 - t0) / (double) (n - 1), title)
  {
  }
}
