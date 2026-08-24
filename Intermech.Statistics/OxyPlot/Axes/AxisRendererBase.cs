// Decompiled with JetBrains decompiler
// Type: OxyPlot.Axes.AxisRendererBase
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Axes;

public abstract class AxisRendererBase
{
  private readonly PlotModel plot;
  private readonly IRenderContext rc;
  private IList<double> majorLabelValues;
  private IList<double> majorTickValues;
  private IList<double> minorTickValues;

  protected AxisRendererBase(IRenderContext rc, PlotModel plot)
  {
    this.plot = plot;
    this.rc = rc;
  }

  protected PlotModel Plot => this.plot;

  protected IRenderContext RenderContext => this.rc;

  protected OxyPen AxislinePen { get; set; }

  protected OxyPen ExtraPen { get; set; }

  protected IList<double> MajorLabelValues
  {
    get => this.majorLabelValues;
    set => this.majorLabelValues = value;
  }

  protected OxyPen MajorPen { get; set; }

  protected OxyPen MajorTickPen { get; set; }

  protected IList<double> MajorTickValues
  {
    get => this.majorTickValues;
    set => this.majorTickValues = value;
  }

  protected OxyPen MinorPen { get; set; }

  protected OxyPen MinorTickPen { get; set; }

  protected IList<double> MinorTickValues
  {
    get => this.minorTickValues;
    set => this.minorTickValues = value;
  }

  protected OxyPen ZeroPen { get; set; }

  public virtual void Render(Axis axis, int pass)
  {
    if (axis == null)
      return;
    axis.GetTickValues(out this.majorLabelValues, out this.majorTickValues, out this.minorTickValues);
    this.CreatePens(axis);
  }

  protected virtual void CreatePens(Axis axis)
  {
    this.MinorPen = OxyPen.Create(axis.MinorGridlineColor, axis.MinorGridlineThickness, axis.MinorGridlineStyle);
    this.MajorPen = OxyPen.Create(axis.MajorGridlineColor, axis.MajorGridlineThickness, axis.MajorGridlineStyle);
    this.MinorTickPen = OxyPen.Create(axis.TicklineColor, axis.MinorGridlineThickness);
    this.MajorTickPen = OxyPen.Create(axis.TicklineColor, axis.MajorGridlineThickness);
    this.ZeroPen = OxyPen.Create(axis.TicklineColor, axis.MajorGridlineThickness);
    this.ExtraPen = OxyPen.Create(axis.ExtraGridlineColor, axis.ExtraGridlineThickness, axis.ExtraGridlineStyle);
    this.AxislinePen = OxyPen.Create(axis.AxislineColor, axis.AxislineThickness, axis.AxislineStyle);
  }

  protected virtual void GetTickPositions(
    Axis axis,
    TickStyle tickStyle,
    double tickSize,
    AxisPosition position,
    out double x0,
    out double x1)
  {
    x0 = 0.0;
    x1 = 0.0;
    double num = (position == AxisPosition.Top ? 1 : (position == AxisPosition.Left ? 1 : 0)) != 0 ? -1.0 : 1.0;
    switch (tickStyle)
    {
      case TickStyle.Crossing:
        x0 = -tickSize * num * 0.75;
        x1 = tickSize * num * 0.75;
        break;
      case TickStyle.Inside:
        x0 = -tickSize * num;
        break;
      case TickStyle.Outside:
        x1 = tickSize * num;
        break;
    }
  }

  protected bool IsWithin(double d, double min, double max) => d >= min && d <= max;
}
