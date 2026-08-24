// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.RectangleBarSeries
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Series;

public class RectangleBarSeries : XYAxisSeries
{
  public new const string DefaultTrackerFormatString = "{0}\n{1}: {2} {3}\n{4}: {5} {6}";
  private OxyColor defaultFillColor;

  public RectangleBarSeries()
  {
    this.Items = (IList<RectangleBarItem>) new List<RectangleBarItem>();
    this.FillColor = OxyColors.Automatic;
    this.LabelColor = OxyColors.Automatic;
    this.StrokeColor = OxyColors.Black;
    this.StrokeThickness = 1.0;
    this.TrackerFormatString = "{0}\n{1}: {2} {3}\n{4}: {5} {6}";
    this.LabelFormatString = "{4}";
  }

  public OxyColor FillColor { get; set; }

  public OxyColor ActualFillColor => this.FillColor.GetActualColor(this.defaultFillColor);

  public IList<RectangleBarItem> Items { get; private set; }

  public OxyColor LabelColor { get; set; }

  public string LabelFormatString { get; set; }

  public OxyColor StrokeColor { get; set; }

  public double StrokeThickness { get; set; }

  internal IList<OxyRect> ActualBarRectangles { get; set; }

  public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
  {
    if (this.ActualBarRectangles == null)
      return (TrackerHitResult) null;
    for (int index = 0; index < this.ActualBarRectangles.Count; ++index)
    {
      if (this.ActualBarRectangles[index].Contains(point))
      {
        double y = (this.Items[index].Y0 + this.Items[index].Y1) / 2.0;
        ScreenPoint screenPoint = point;
        DataPoint dataPoint = new DataPoint((double) index, y);
        object obj = this.GetItem(index);
        return new TrackerHitResult()
        {
          Series = (OxyPlot.Series.Series) this,
          DataPoint = dataPoint,
          Position = screenPoint,
          Item = obj,
          Index = (double) index,
          Text = StringHelper.Format((IFormatProvider) this.ActualCulture, this.TrackerFormatString, obj, (object) this.Title, (object) (this.XAxis.Title ?? "X"), this.XAxis.GetValue(this.Items[index].X0), this.XAxis.GetValue(this.Items[index].X1), (object) (this.YAxis.Title ?? "Y"), this.YAxis.GetValue(this.Items[index].Y0), this.YAxis.GetValue(this.Items[index].Y1), (object) this.Items[index].Title)
        };
      }
    }
    return (TrackerHitResult) null;
  }

  public override void Render(IRenderContext rc)
  {
    if (this.Items.Count == 0)
      return;
    OxyRect clippingRect = this.GetClippingRect();
    int i = 0;
    this.ActualBarRectangles = (IList<OxyRect>) new List<OxyRect>();
    foreach (RectangleBarItem rectangleBarItem in (IEnumerable<RectangleBarItem>) this.Items)
    {
      if (this.IsValid(rectangleBarItem.X0) && this.IsValid(rectangleBarItem.X1) && this.IsValid(rectangleBarItem.Y0) && this.IsValid(rectangleBarItem.Y1))
      {
        ScreenPoint screenPoint1 = this.Transform(rectangleBarItem.X0, rectangleBarItem.Y0);
        ScreenPoint screenPoint2 = this.Transform(rectangleBarItem.X1, rectangleBarItem.Y1);
        OxyRect rect = OxyRect.Create(screenPoint1.X, screenPoint1.Y, screenPoint2.X, screenPoint2.Y);
        this.ActualBarRectangles.Add(rect);
        rc.DrawClippedRectangleAsPolygon(clippingRect, rect, this.GetSelectableFillColor(rectangleBarItem.Color.GetActualColor(this.ActualFillColor)), this.StrokeColor, this.StrokeThickness);
        if (this.LabelFormatString != null)
        {
          string text = StringHelper.Format((IFormatProvider) this.ActualCulture, this.LabelFormatString, this.GetItem(i), (object) rectangleBarItem.X0, (object) rectangleBarItem.X1, (object) rectangleBarItem.Y0, (object) rectangleBarItem.Y1, (object) rectangleBarItem.Title);
          ScreenPoint p = new ScreenPoint((rect.Left + rect.Right) / 2.0, (rect.Top + rect.Bottom) / 2.0);
          rc.DrawClippedText(clippingRect, p, text, this.ActualTextColor, this.ActualFont, this.ActualFontSize, this.ActualFontWeight, horizontalAlignment: HorizontalAlignment.Center, verticalAlignment: VerticalAlignment.Middle);
        }
        ++i;
      }
    }
  }

  public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
  {
    double num1 = (legendBox.Left + legendBox.Right) / 2.0;
    double num2 = (legendBox.Top + legendBox.Bottom) / 2.0;
    double height = (legendBox.Bottom - legendBox.Top) * 0.8;
    double width = height;
    rc.DrawRectangleAsPolygon(new OxyRect(num1 - 0.5 * width, num2 - 0.5 * height, width, height), this.GetSelectableFillColor(this.ActualFillColor), this.StrokeColor, this.StrokeThickness);
  }

  protected internal override void SetDefaultValues()
  {
    if (!this.FillColor.IsAutomatic())
      return;
    this.defaultFillColor = this.PlotModel.GetDefaultColor();
  }

  protected internal override void UpdateData()
  {
    if (this.ItemsSource != null)
    {
      this.Items.Clear();
      throw new NotImplementedException();
    }
  }

  protected internal override void UpdateMaxMin()
  {
    base.UpdateMaxMin();
    if (this.Items == null || this.Items.Count == 0)
      return;
    double val1_1 = double.MaxValue;
    double val1_2 = double.MinValue;
    double val1_3 = double.MaxValue;
    double val1_4 = double.MinValue;
    foreach (RectangleBarItem rectangleBarItem in (IEnumerable<RectangleBarItem>) this.Items)
    {
      val1_1 = Math.Min(val1_1, Math.Min(rectangleBarItem.X0, rectangleBarItem.X1));
      val1_2 = Math.Max(val1_2, Math.Max(rectangleBarItem.X1, rectangleBarItem.X0));
      val1_3 = Math.Min(val1_3, Math.Min(rectangleBarItem.Y0, rectangleBarItem.Y1));
      val1_4 = Math.Max(val1_4, Math.Max(rectangleBarItem.Y0, rectangleBarItem.Y1));
    }
    this.MinX = val1_1;
    this.MaxX = val1_2;
    this.MinY = val1_3;
    this.MaxY = val1_4;
  }

  protected virtual bool IsValid(double v) => !double.IsNaN(v) && !double.IsInfinity(v);
}
