// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.PieSeries
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OxyPlot.Series;

public class PieSeries : ItemsSeries
{
  public const string DefaultTrackerFormatString = "{1}: {2:0.###} ({3:P1})";
  private IList<PieSlice> slices;
  private List<IList<ScreenPoint>> slicePoints = new List<IList<ScreenPoint>>();
  private double total;

  public PieSeries()
  {
    this.slices = (IList<PieSlice>) new List<PieSlice>();
    this.Stroke = OxyColors.White;
    this.StrokeThickness = 1.0;
    this.Diameter = 1.0;
    this.InnerDiameter = 0.0;
    this.StartAngle = 0.0;
    this.AngleSpan = 360.0;
    this.AngleIncrement = 1.0;
    this.LegendFormat = (string) null;
    this.OutsideLabelFormat = "{2:0} %";
    this.InsideLabelColor = OxyColors.Automatic;
    this.InsideLabelFormat = "{1}";
    this.TickDistance = 0.0;
    this.TickRadialLength = 6.0;
    this.TickHorizontalLength = 8.0;
    this.TickLabelDistance = 4.0;
    this.InsideLabelPosition = 0.5;
    this.FontSize = 12.0;
    this.TrackerFormatString = "{1}: {2:0.###} ({3:P1})";
  }

  public double AngleIncrement { get; set; }

  public double AngleSpan { get; set; }

  public bool AreInsideLabelsAngled { get; set; }

  public string ColorField { get; set; }

  public double Diameter { get; set; }

  public double ExplodedDistance { get; set; }

  public double InnerDiameter { get; set; }

  public OxyColor InsideLabelColor { get; set; }

  public string InsideLabelFormat { get; set; }

  public double InsideLabelPosition { get; set; }

  public string IsExplodedField { get; set; }

  public string LabelField { get; set; }

  public string LegendFormat { get; set; }

  public string OutsideLabelFormat { get; set; }

  public IList<PieSlice> Slices
  {
    get => this.slices;
    set => this.slices = value;
  }

  public double StartAngle { get; set; }

  public OxyColor Stroke { get; set; }

  public double StrokeThickness { get; set; }

  public double TickDistance { get; set; }

  public double TickHorizontalLength { get; set; }

  public double TickLabelDistance { get; set; }

  public double TickRadialLength { get; set; }

  public string ValueField { get; set; }

  public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
  {
    for (int index = 0; index < this.slicePoints.Count; ++index)
    {
      if (ScreenPointHelper.IsPointInPolygon(point, this.slicePoints[index]))
      {
        PieSlice slice = this.slices[index];
        object obj = this.GetItem(index);
        return new TrackerHitResult()
        {
          Series = (OxyPlot.Series.Series) this,
          Position = point,
          Item = obj,
          Index = (double) index,
          Text = StringHelper.Format((IFormatProvider) this.ActualCulture, this.TrackerFormatString, (object) slice, (object) this.Title, (object) slice.Label, (object) slice.Value, (object) (slice.Value / this.total))
        };
      }
    }
    return (TrackerHitResult) null;
  }

  public override void Render(IRenderContext rc)
  {
    this.slicePoints.Clear();
    if (this.Slices.Count == 0)
      return;
    this.total = this.slices.Sum<PieSlice>((Func<PieSlice, double>) (slice => slice.Value));
    if (Math.Abs(this.total) < double.Epsilon)
      return;
    double num1 = Math.Min(this.PlotModel.PlotArea.Width, this.PlotModel.PlotArea.Height) / 2.0;
    double num2 = num1 * (this.Diameter - this.ExplodedDistance);
    double num3 = num1 * this.InnerDiameter;
    double num4 = this.StartAngle;
    ScreenPoint screenPoint1;
    ref ScreenPoint local = ref screenPoint1;
    OxyRect plotArea = this.PlotModel.PlotArea;
    double left = plotArea.Left;
    plotArea = this.PlotModel.PlotArea;
    double right = plotArea.Right;
    double x = (left + right) * 0.5;
    plotArea = this.PlotModel.PlotArea;
    double top = plotArea.Top;
    plotArea = this.PlotModel.PlotArea;
    double bottom = plotArea.Bottom;
    double y = (top + bottom) * 0.5;
    local = new ScreenPoint(x, y);
    foreach (PieSlice slice in (IEnumerable<PieSlice>) this.slices)
    {
      List<ScreenPoint> screenPointList = new List<ScreenPoint>();
      List<ScreenPoint> collection = new List<ScreenPoint>();
      double num5 = slice.Value / this.total * this.AngleSpan;
      double num6 = num4 + num5;
      double num7 = slice.IsExploded ? this.ExplodedDistance * num1 : 0.0;
      double num8 = num4 + num5 / 2.0;
      double num9 = num8 * Math.PI / 180.0;
      ScreenPoint screenPoint2 = new ScreenPoint(screenPoint1.X + num7 * Math.Cos(num9), screenPoint1.Y + num7 * Math.Sin(num9));
      while (true)
      {
        bool flag = false;
        if (num4 >= num6)
        {
          num4 = num6;
          flag = true;
        }
        double num10 = num4 * Math.PI / 180.0;
        ScreenPoint screenPoint3 = new ScreenPoint(screenPoint2.X + num2 * Math.Cos(num10), screenPoint2.Y + num2 * Math.Sin(num10));
        screenPointList.Add(screenPoint3);
        ScreenPoint screenPoint4 = new ScreenPoint(screenPoint2.X + num3 * Math.Cos(num10), screenPoint2.Y + num3 * Math.Sin(num10));
        if (num3 + num7 > 0.0)
          collection.Add(screenPoint4);
        if (!flag)
          num4 += this.AngleIncrement;
        else
          break;
      }
      collection.Reverse();
      if (collection.Count == 0)
        collection.Add(screenPoint2);
      collection.Add(screenPointList[0]);
      List<ScreenPoint> points = screenPointList;
      points.AddRange((IEnumerable<ScreenPoint>) collection);
      rc.DrawPolygon((IList<ScreenPoint>) points, slice.ActualFillColor, this.Stroke, this.StrokeThickness, lineJoin: LineJoin.Bevel);
      this.slicePoints.Add((IList<ScreenPoint>) points);
      if (this.OutsideLabelFormat != null)
      {
        string text = string.Format(this.OutsideLabelFormat, (object) slice.Value, (object) slice.Label, (object) (slice.Value / this.total * 100.0));
        int num11 = Math.Sign(Math.Cos(num9));
        ScreenPoint screenPoint5 = new ScreenPoint(screenPoint2.X + (num2 + this.TickDistance) * Math.Cos(num9), screenPoint2.Y + (num2 + this.TickDistance) * Math.Sin(num9));
        ScreenPoint screenPoint6 = new ScreenPoint(screenPoint5.X + this.TickRadialLength * Math.Cos(num9), screenPoint5.Y + this.TickRadialLength * Math.Sin(num9));
        ScreenPoint screenPoint7 = new ScreenPoint(screenPoint6.X + this.TickHorizontalLength * (double) num11, screenPoint6.Y);
        rc.DrawLine((IList<ScreenPoint>) new ScreenPoint[3]
        {
          screenPoint5,
          screenPoint6,
          screenPoint7
        }, this.ActualTextColor, lineJoin: LineJoin.Bevel);
        ScreenPoint p = new ScreenPoint(screenPoint7.X + this.TickLabelDistance * (double) num11, screenPoint7.Y);
        rc.DrawText(p, text, this.ActualTextColor, this.ActualFont, this.ActualFontSize, this.ActualFontWeight, horizontalAlignment: num11 > 0 ? HorizontalAlignment.Left : HorizontalAlignment.Right, verticalAlignment: VerticalAlignment.Middle);
      }
      if (this.InsideLabelFormat != null)
      {
        OxyColor insideLabelColor = this.InsideLabelColor;
        if (!insideLabelColor.IsUndefined())
        {
          string text = string.Format(this.InsideLabelFormat, (object) slice.Value, (object) slice.Label, (object) (slice.Value / this.total * 100.0));
          double num12 = num3 * (1.0 - this.InsideLabelPosition) + num2 * this.InsideLabelPosition;
          ScreenPoint p = new ScreenPoint(screenPoint2.X + num12 * Math.Cos(num9), screenPoint2.Y + num12 * Math.Sin(num9));
          double rotation = 0.0;
          if (this.AreInsideLabelsAngled)
          {
            rotation = num8;
            if (Math.Cos(num9) < 0.0)
              rotation += 180.0;
          }
          insideLabelColor = this.InsideLabelColor;
          OxyColor fill = insideLabelColor.IsAutomatic() ? this.ActualTextColor : this.InsideLabelColor;
          rc.DrawText(p, text, fill, this.ActualFont, this.ActualFontSize, this.ActualFontWeight, rotation, HorizontalAlignment.Center, VerticalAlignment.Middle);
        }
      }
    }
  }

  public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
  {
  }

  protected internal override bool AreAxesRequired() => false;

  protected internal override void EnsureAxes()
  {
  }

  protected internal override bool IsUsing(Axis axis) => false;

  protected internal override void SetDefaultValues()
  {
    foreach (PieSlice slice in (IEnumerable<PieSlice>) this.Slices)
    {
      if (slice.Fill.IsAutomatic())
        slice.DefaultFillColor = this.PlotModel.GetDefaultColor();
    }
  }

  protected internal override void UpdateAxisMaxMin()
  {
  }

  protected internal override void UpdateData()
  {
    if (this.ItemsSource == null)
      return;
    this.slices.Clear();
    OxyPlot.ListBuilder<PieSlice> listBuilder = new OxyPlot.ListBuilder<PieSlice>();
    listBuilder.Add<string>(this.LabelField, (string) null);
    listBuilder.Add<double>(this.ValueField, double.NaN);
    listBuilder.Add<OxyColor>(this.ColorField, OxyColors.Automatic);
    listBuilder.Add<bool>(this.IsExplodedField, false);
    listBuilder.FillT(this.slices, this.ItemsSource, (Func<IList<object>, PieSlice>) (args => new PieSlice((string) args[0], Convert.ToDouble(args[1]))
    {
      Fill = (OxyColor) args[2],
      IsExploded = (bool) args[3]
    }));
  }

  protected internal override void UpdateMaxMin()
  {
  }
}
