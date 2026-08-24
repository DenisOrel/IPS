// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.LinearBarSeries
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Series;

public class LinearBarSeries : DataPointSeries
{
  private readonly List<OxyRect> rectangles = new List<OxyRect>();
  private readonly List<int> rectanglesPointIndexes = new List<int>();
  private OxyColor defaultColor;

  public LinearBarSeries()
  {
    this.FillColor = OxyColors.Automatic;
    this.BarWidth = 5.0;
    this.StrokeColor = OxyColors.Black;
    this.StrokeThickness = 0.0;
    this.TrackerFormatString = "{0}\n{1}: {2}\n{3}: {4}";
    this.NegativeFillColor = OxyColors.Undefined;
    this.NegativeStrokeColor = OxyColors.Undefined;
  }

  public OxyColor FillColor { get; set; }

  public double BarWidth { get; set; }

  public double StrokeThickness { get; set; }

  public OxyColor StrokeColor { get; set; }

  public OxyColor NegativeFillColor { get; set; }

  public OxyColor NegativeStrokeColor { get; set; }

  public OxyColor ActualColor => this.FillColor.GetActualColor(this.defaultColor);

  public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
  {
    int rectangleIndex = this.FindRectangleIndex(point);
    if (rectangleIndex < 0)
      return (TrackerHitResult) null;
    if (!this.rectangles[rectangleIndex].Contains(point))
      return (TrackerHitResult) null;
    int rectanglesPointIndex = this.rectanglesPointIndexes[rectangleIndex];
    DataPoint actualPoint = this.ActualPoints[rectanglesPointIndex];
    object obj = this.GetItem(rectanglesPointIndex);
    object[] objArray = new object[5]
    {
      (object) this.Title,
      (object) (this.XAxis.Title ?? "X"),
      this.XAxis.GetValue(actualPoint.X),
      (object) (this.YAxis.Title ?? "Y"),
      this.YAxis.GetValue(actualPoint.Y)
    };
    string str = StringHelper.Format((IFormatProvider) this.ActualCulture, this.TrackerFormatString, obj, objArray);
    return new TrackerHitResult()
    {
      Series = (OxyPlot.Series.Series) this,
      DataPoint = actualPoint,
      Position = point,
      Item = obj,
      Index = (double) rectanglesPointIndex,
      Text = str
    };
  }

  public override void Render(IRenderContext rc)
  {
    this.rectangles.Clear();
    this.rectanglesPointIndexes.Clear();
    List<DataPoint> actualPoints = this.ActualPoints;
    if (actualPoints == null || actualPoints.Count == 0)
      return;
    this.VerifyAxes();
    OxyRect clippingRect = this.GetClippingRect();
    rc.SetClip(clippingRect);
    this.RenderBars(rc, clippingRect, actualPoints);
    rc.ResetClip();
  }

  public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
  {
    double num1 = (legendBox.Left + legendBox.Right) / 2.0;
    double num2 = (legendBox.Top + legendBox.Bottom) / 2.0;
    double height = (legendBox.Bottom - legendBox.Top) * 0.8;
    double width = height;
    rc.DrawRectangleAsPolygon(new OxyRect(num1 - 0.5 * width, num2 - 0.5 * height, width, height), this.GetSelectableColor(this.ActualColor), this.StrokeColor, this.StrokeThickness);
  }

  protected internal override void SetDefaultValues()
  {
    if (!this.FillColor.IsAutomatic())
      return;
    this.defaultColor = this.PlotModel.GetDefaultColor();
  }

  protected internal override void UpdateAxisMaxMin()
  {
    base.UpdateAxisMaxMin();
    this.YAxis.Include(0.0);
  }

  private static ScreenPoint Translate(ScreenPoint screenPoint, double offset)
  {
    return new ScreenPoint(screenPoint.X + offset, screenPoint.Y);
  }

  private int FindRectangleIndex(ScreenPoint point)
  {
    return this.rectangles.BinarySearch(0, this.rectangles.Count, new OxyRect(), ComparerHelper.CreateComparer<OxyRect>((Comparison<OxyRect>) ((x, y) =>
    {
      if (x.Right < point.X)
        return -1;
      return x.Left > point.X ? 1 : 0;
    })));
  }

  private void RenderBars(IRenderContext rc, OxyRect clippingRect, List<DataPoint> actualPoints)
  {
    double offset = this.GetBarWidth(actualPoints) / 2.0;
    for (int index = 0; index < actualPoints.Count; ++index)
    {
      DataPoint actualPoint = actualPoints[index];
      if (this.IsValidPoint(actualPoint))
      {
        ScreenPoint p1 = LinearBarSeries.Translate(this.Transform(actualPoint), -offset);
        OxyRect rect = new OxyRect(LinearBarSeries.Translate(this.Transform(new DataPoint(actualPoint.X, 0.0)), offset), p1);
        this.rectangles.Add(rect);
        this.rectanglesPointIndexes.Add(index);
        LinearBarSeries.BarColors barColors = this.GetBarColors(actualPoint.Y);
        rc.DrawClippedRectangleAsPolygon(clippingRect, rect, barColors.FillColor, barColors.StrokeColor, this.StrokeThickness);
      }
    }
  }

  private double GetBarWidth(List<DataPoint> actualPoints)
  {
    double num1 = this.BarWidth / this.XAxis.Scale;
    for (int index = 1; index < actualPoints.Count; ++index)
    {
      double num2 = actualPoints[index].X - actualPoints[index - 1].X;
      if (num2 < num1)
        num1 = num2;
    }
    return num1 * this.XAxis.Scale;
  }

  private LinearBarSeries.BarColors GetBarColors(double y)
  {
    int num = y >= 0.0 ? 1 : 0;
    return new LinearBarSeries.BarColors(num != 0 || this.NegativeFillColor.IsUndefined() ? this.GetSelectableFillColor(this.ActualColor) : this.NegativeFillColor, num != 0 || this.NegativeStrokeColor.IsUndefined() ? this.StrokeColor : this.NegativeStrokeColor);
  }

  private struct BarColors
  {
    public BarColors(OxyColor fillColor, OxyColor strokeColor)
      : this()
    {
      this.FillColor = fillColor;
      this.StrokeColor = strokeColor;
    }

    public OxyColor FillColor { get; private set; }

    public OxyColor StrokeColor { get; private set; }
  }
}
