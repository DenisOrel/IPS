// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.PieSeriesExt
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OxyPlot.Series;

public class PieSeriesExt : ItemsSeries
{
  public const string DefaultTrackerFormatString = "{1}: {2:0.###} ({3:P1})";
  private IList<PieSlice> slices;
  private List<IList<ScreenPoint>> slicePoints = new List<IList<ScreenPoint>>();
  private double total;
  private Random _random;

  public PieSeriesExt()
  {
    this._random = new Random();
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
    double width = this.PlotModel.PlotArea.Width;
    OxyRect plotArea1 = this.PlotModel.PlotArea;
    double num1 = plotArea1.Left * 2.0;
    double val1 = width - num1;
    plotArea1 = this.PlotModel.PlotArea;
    double val2 = plotArea1.Height - this.PlotModel.PlotArea.Top;
    double num2 = Math.Min(val1, val2) / 2.0;
    double num3 = num2 * (this.Diameter - this.ExplodedDistance);
    double num4 = num2 * this.InnerDiameter;
    double num5 = this.StartAngle;
    ScreenPoint screenPoint1;
    ref ScreenPoint local = ref screenPoint1;
    double left = this.PlotModel.PlotArea.Left;
    OxyRect plotArea2 = this.PlotModel.PlotArea;
    double right = plotArea2.Right;
    double x = (left + right) * 0.5;
    plotArea2 = this.PlotModel.PlotArea;
    double top = plotArea2.Top;
    plotArea2 = this.PlotModel.PlotArea;
    double bottom = plotArea2.Bottom;
    double y = (top + bottom) * 0.5;
    local = new ScreenPoint(x, y);
    foreach (PieSlice slice in (IEnumerable<PieSlice>) this.slices)
    {
      List<ScreenPoint> screenPointList = new List<ScreenPoint>();
      List<ScreenPoint> collection = new List<ScreenPoint>();
      double num6 = slice.Value / this.total * this.AngleSpan;
      double num7 = num5 + num6;
      double num8 = slice.IsExploded ? this.ExplodedDistance * num2 : 0.0;
      double num9 = (num5 + num6 / 2.0) * Math.PI / 180.0;
      ScreenPoint screenPoint2 = new ScreenPoint(screenPoint1.X + num8 * Math.Cos(num9), screenPoint1.Y + num8 * Math.Sin(num9));
      while (true)
      {
        bool flag = false;
        if (num5 >= num7)
        {
          num5 = num7;
          flag = true;
        }
        double num10 = num5 * Math.PI / 180.0;
        ScreenPoint screenPoint3 = new ScreenPoint(screenPoint2.X + num3 * Math.Cos(num10), screenPoint2.Y + num3 * Math.Sin(num10));
        screenPointList.Add(screenPoint3);
        ScreenPoint screenPoint4 = new ScreenPoint(screenPoint2.X + num4 * Math.Cos(num10), screenPoint2.Y + num4 * Math.Sin(num10));
        if (num4 + num8 > 0.0)
          collection.Add(screenPoint4);
        if (!flag)
          num5 += this.AngleIncrement;
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
        ScreenPoint screenPoint5 = new ScreenPoint(screenPoint2.X + (num3 + this.TickDistance) * Math.Cos(num9), screenPoint2.Y + (num3 + this.TickDistance) * Math.Sin(num9));
        ScreenPoint screenPoint6 = new ScreenPoint(screenPoint5.X + this.TickRadialLength * Math.Cos(num9), screenPoint5.Y + this.TickRadialLength * Math.Sin(num9));
        ScreenPoint screenPoint7 = new ScreenPoint(screenPoint6.X + this.TickHorizontalLength * (double) num11, screenPoint6.Y);
        rc.DrawLine((IList<ScreenPoint>) new ScreenPoint[2]
        {
          screenPoint5,
          screenPoint6
        }, this.ActualTextColor, lineJoin: LineJoin.Bevel);
        ScreenPoint p = new ScreenPoint(screenPoint7.X + this.TickLabelDistance * (double) num11, screenPoint7.Y);
        rc.DrawText(p, text, this.ActualTextColor, this.ActualFont, this.ActualFontSize, this.ActualFontWeight, horizontalAlignment: num11 > 0 ? HorizontalAlignment.Left : HorizontalAlignment.Right, verticalAlignment: VerticalAlignment.Middle);
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
