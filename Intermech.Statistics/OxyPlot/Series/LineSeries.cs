// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.LineSeries
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OxyPlot.Series;

public class LineSeries : DataPointSeries
{
  private const double ToleranceDivisor = 200.0;
  private List<ScreenPoint> outputBuffer;
  private List<ScreenPoint> contiguousScreenPointsBuffer;
  private List<ScreenPoint> decimatorBuffer;
  private OxyColor defaultColor;
  private OxyColor defaultMarkerFill;
  private LineStyle defaultLineStyle;
  private List<DataPoint> smoothedPoints;

  public LineSeries()
  {
    this.StrokeThickness = 2.0;
    this.LineJoin = LineJoin.Bevel;
    this.LineStyle = LineStyle.Automatic;
    this.Color = OxyColors.Automatic;
    this.BrokenLineColor = OxyColors.Undefined;
    this.MarkerFill = OxyColors.Automatic;
    this.MarkerStroke = OxyColors.Automatic;
    this.MarkerResolution = 0;
    this.MarkerSize = 3.0;
    this.MarkerStrokeThickness = 1.0;
    this.MarkerType = MarkerType.None;
    this.MinimumSegmentLength = 2.0;
    this.CanTrackerInterpolatePoints = true;
    this.LabelMargin = 6.0;
    this.smoothedPoints = new List<DataPoint>();
  }

  public OxyColor Color { get; set; }

  public OxyColor BrokenLineColor { get; set; }

  public LineStyle BrokenLineStyle { get; set; }

  public double BrokenLineThickness { get; set; }

  public double[] Dashes { get; set; }

  public Action<List<ScreenPoint>, List<ScreenPoint>> Decimator { get; set; }

  public string LabelFormatString { get; set; }

  public double LabelMargin { get; set; }

  public LineJoin LineJoin { get; set; }

  public LineStyle LineStyle { get; set; }

  public LineLegendPosition LineLegendPosition { get; set; }

  public OxyColor MarkerFill { get; set; }

  public ScreenPoint[] MarkerOutline { get; set; }

  public int MarkerResolution { get; set; }

  public double MarkerSize { get; set; }

  public OxyColor MarkerStroke { get; set; }

  public double MarkerStrokeThickness { get; set; }

  public MarkerType MarkerType { get; set; }

  public double MinimumSegmentLength { get; set; }

  public bool Smooth { get; set; }

  public double StrokeThickness { get; set; }

  public OxyColor ActualColor => this.Color.GetActualColor(this.defaultColor);

  public OxyColor ActualMarkerFill => this.MarkerFill.GetActualColor(this.defaultMarkerFill);

  protected LineStyle ActualLineStyle
  {
    get => this.LineStyle == LineStyle.Automatic ? this.defaultLineStyle : this.LineStyle;
  }

  protected double[] ActualDashArray => this.Dashes ?? this.ActualLineStyle.GetDashArray();

  protected List<DataPoint> SmoothedPoints => this.smoothedPoints;

  public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
  {
    if (interpolate)
    {
      if (this.ActualColor.IsInvisible() || this.StrokeThickness.Equals(0.0))
        return (TrackerHitResult) null;
      if (!this.CanTrackerInterpolatePoints)
        return (TrackerHitResult) null;
    }
    if (!interpolate || !this.Smooth)
      return base.GetNearestPoint(point, interpolate);
    TrackerHitResult interpolatedPointInternal = this.GetNearestInterpolatedPointInternal(this.SmoothedPoints, point);
    if (interpolatedPointInternal != null)
      interpolatedPointInternal.Text = StringHelper.Format((IFormatProvider) this.ActualCulture, this.TrackerFormatString, interpolatedPointInternal.Item, (object) this.Title, (object) (this.XAxis.Title ?? "X"), this.XAxis.GetValue(interpolatedPointInternal.DataPoint.X), (object) (this.YAxis.Title ?? "Y"), this.YAxis.GetValue(interpolatedPointInternal.DataPoint.Y));
    return interpolatedPointInternal;
  }

  public override void Render(IRenderContext rc)
  {
    List<DataPoint> actualPoints = this.ActualPoints;
    if (actualPoints == null || actualPoints.Count == 0)
      return;
    this.VerifyAxes();
    OxyRect clippingRect = this.GetClippingRect();
    rc.SetClip(clippingRect);
    this.RenderPoints(rc, clippingRect, (ICollection<DataPoint>) actualPoints);
    if (this.LabelFormatString != null)
      this.RenderPointLabels(rc, clippingRect);
    rc.ResetClip();
    if (this.LineLegendPosition == LineLegendPosition.None || string.IsNullOrEmpty(this.Title))
      return;
    this.RenderLegendOnLine(rc);
  }

  public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
  {
    double x = (legendBox.Left + legendBox.Right) / 2.0;
    double y = (legendBox.Top + legendBox.Bottom) / 2.0;
    ScreenPoint[] points = new ScreenPoint[2]
    {
      new ScreenPoint(legendBox.Left, y),
      new ScreenPoint(legendBox.Right, y)
    };
    rc.DrawLine((IList<ScreenPoint>) points, this.GetSelectableColor(this.ActualColor), this.StrokeThickness, this.ActualDashArray);
    ScreenPoint p = new ScreenPoint(x, y);
    rc.DrawMarker(legendBox, p, this.MarkerType, (IList<ScreenPoint>) this.MarkerOutline, this.MarkerSize, this.ActualMarkerFill, this.MarkerStroke, this.MarkerStrokeThickness);
  }

  protected internal override void SetDefaultValues()
  {
    if (this.LineStyle == LineStyle.Automatic)
      this.defaultLineStyle = this.PlotModel.GetDefaultLineStyle();
    OxyColor oxyColor = this.Color;
    if (!oxyColor.IsAutomatic())
      return;
    this.defaultColor = this.PlotModel.GetDefaultColor();
    oxyColor = this.MarkerFill;
    if (!oxyColor.IsAutomatic())
      return;
    this.defaultMarkerFill = this.defaultColor;
  }

  protected internal override void UpdateMaxMin()
  {
    if (this.Smooth)
    {
      base.UpdateMaxMin();
      this.ResetSmoothedPoints();
      if (this.SmoothedPoints.Count == 0)
        return;
      this.MinX = this.SmoothedPoints.Where<DataPoint>((Func<DataPoint, bool>) (x => !double.IsNaN(x.X))).Min<DataPoint>((Func<DataPoint, double>) (x => x.X));
      this.MinY = this.SmoothedPoints.Where<DataPoint>((Func<DataPoint, bool>) (x => !double.IsNaN(x.Y))).Min<DataPoint>((Func<DataPoint, double>) (x => x.Y));
      this.MaxX = this.SmoothedPoints.Where<DataPoint>((Func<DataPoint, bool>) (x => !double.IsNaN(x.X))).Max<DataPoint>((Func<DataPoint, double>) (x => x.X));
      this.MaxY = this.SmoothedPoints.Where<DataPoint>((Func<DataPoint, bool>) (x => !double.IsNaN(x.Y))).Max<DataPoint>((Func<DataPoint, double>) (x => x.Y));
    }
    else
      base.UpdateMaxMin();
  }

  protected void RenderPoints(
    IRenderContext rc,
    OxyRect clippingRect,
    ICollection<DataPoint> points)
  {
    IEnumerator<DataPoint> enumerator = points.GetEnumerator();
    ScreenPoint? previousContiguousLineSegmentEndPoint = new ScreenPoint?();
    bool flag = this.BrokenLineThickness > 0.0 && this.BrokenLineStyle != LineStyle.None;
    double[] dashArray = flag ? this.BrokenLineStyle.GetDashArray() : (double[]) null;
    List<ScreenPoint> screenPointList = flag ? new List<ScreenPoint>(2) : (List<ScreenPoint>) null;
    if (this.contiguousScreenPointsBuffer == null)
      this.contiguousScreenPointsBuffer = new List<ScreenPoint>(points.Count);
    while (enumerator.MoveNext() && this.ExtractNextContiguousLineSegment(enumerator, ref previousContiguousLineSegmentEndPoint, screenPointList, this.contiguousScreenPointsBuffer))
    {
      if (flag)
      {
        if (screenPointList.Count > 0)
        {
          OxyColor stroke = this.BrokenLineColor.IsAutomatic() ? this.ActualColor : this.BrokenLineColor;
          rc.DrawClippedLineSegments(clippingRect, (IList<ScreenPoint>) screenPointList, stroke, this.BrokenLineThickness, dashArray, this.LineJoin, false);
          screenPointList.Clear();
        }
      }
      else
        previousContiguousLineSegmentEndPoint = new ScreenPoint?();
      if (this.Decimator != null)
      {
        if (this.decimatorBuffer == null)
          this.decimatorBuffer = new List<ScreenPoint>(this.contiguousScreenPointsBuffer.Count);
        else
          this.decimatorBuffer.Clear();
        this.Decimator(this.contiguousScreenPointsBuffer, this.decimatorBuffer);
        this.RenderLineAndMarkers(rc, clippingRect, (IList<ScreenPoint>) this.decimatorBuffer);
      }
      else
        this.RenderLineAndMarkers(rc, clippingRect, (IList<ScreenPoint>) this.contiguousScreenPointsBuffer);
      this.contiguousScreenPointsBuffer.Clear();
    }
  }

  protected bool ExtractNextContiguousLineSegment(
    IEnumerator<DataPoint> pointEnumerator,
    ref ScreenPoint? previousContiguousLineSegmentEndPoint,
    List<ScreenPoint> broken,
    List<ScreenPoint> contiguous)
  {
    DataPoint current1;
    while (!this.IsValidPoint(current1 = pointEnumerator.Current))
    {
      if (!pointEnumerator.MoveNext())
        return false;
    }
    ScreenPoint screenPoint = this.Transform(current1);
    if (previousContiguousLineSegmentEndPoint.HasValue)
    {
      broken.Add(previousContiguousLineSegmentEndPoint.Value);
      broken.Add(screenPoint);
    }
    contiguous.Add(screenPoint);
    DataPoint current2;
    while (pointEnumerator.MoveNext() && this.IsValidPoint(current2 = pointEnumerator.Current))
    {
      screenPoint = this.Transform(current2);
      contiguous.Add(screenPoint);
    }
    previousContiguousLineSegmentEndPoint = new ScreenPoint?(screenPoint);
    return true;
  }

  protected void RenderPointLabels(IRenderContext rc, OxyRect clippingRect)
  {
    int i = -1;
    foreach (DataPoint actualPoint in this.ActualPoints)
    {
      ++i;
      if (this.IsValidPoint(actualPoint))
      {
        ScreenPoint p = this.Transform(actualPoint) + new ScreenVector(0.0, -this.LabelMargin);
        if (clippingRect.Contains(p))
        {
          string text = StringHelper.Format((IFormatProvider) this.ActualCulture, this.LabelFormatString, this.GetItem(i), (object) actualPoint.X, (object) actualPoint.Y);
          rc.DrawClippedText(clippingRect, p, text, this.ActualTextColor, this.ActualFont, this.ActualFontSize, this.ActualFontWeight, horizontalAlignment: HorizontalAlignment.Center, verticalAlignment: VerticalAlignment.Bottom);
        }
      }
    }
  }

  protected void RenderLegendOnLine(IRenderContext rc)
  {
    HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left;
    DataPoint actualPoint;
    double x;
    if (this.LineLegendPosition == LineLegendPosition.Start)
    {
      actualPoint = this.ActualPoints[0];
      horizontalAlignment = HorizontalAlignment.Right;
      x = -4.0;
    }
    else
    {
      actualPoint = this.ActualPoints[this.ActualPoints.Count - 1];
      x = 4.0;
    }
    ScreenPoint p = this.Transform(actualPoint) + new ScreenVector(x, 0.0);
    rc.DrawText(p, this.Title, this.ActualTextColor, this.ActualFont, this.ActualFontSize, this.ActualFontWeight, horizontalAlignment: horizontalAlignment, verticalAlignment: VerticalAlignment.Middle);
  }

  protected virtual void RenderLineAndMarkers(
    IRenderContext rc,
    OxyRect clippingRect,
    IList<ScreenPoint> pointsToRender)
  {
    IList<ScreenPoint> pointsToRender1 = pointsToRender;
    if (this.Smooth)
      pointsToRender1 = (IList<ScreenPoint>) CanonicalSplineHelper.CreateSpline(ScreenPointHelper.ResamplePoints(pointsToRender, this.MinimumSegmentLength), 0.5, (IList<double>) null, false, 0.25);
    if (this.StrokeThickness > 0.0 && this.ActualLineStyle != LineStyle.None)
      this.RenderLine(rc, clippingRect, pointsToRender1);
    if (this.MarkerType == MarkerType.None)
      return;
    ScreenPoint binOffset = this.MarkerResolution > 0 ? this.Transform(this.MinX, this.MinY) : new ScreenPoint();
    rc.DrawMarkers(clippingRect, pointsToRender, this.MarkerType, (IList<ScreenPoint>) this.MarkerOutline, (IList<double>) new double[1]
    {
      this.MarkerSize
    }, this.ActualMarkerFill, this.MarkerStroke, this.MarkerStrokeThickness, this.MarkerResolution, binOffset);
  }

  protected virtual void RenderLine(
    IRenderContext rc,
    OxyRect clippingRect,
    IList<ScreenPoint> pointsToRender)
  {
    double[] actualDashArray = this.ActualDashArray;
    if (this.outputBuffer == null)
      this.outputBuffer = new List<ScreenPoint>(pointsToRender.Count);
    rc.DrawClippedLine(clippingRect, pointsToRender, this.MinimumSegmentLength * this.MinimumSegmentLength, this.GetSelectableColor(this.ActualColor), this.StrokeThickness, actualDashArray, this.LineJoin, false, this.outputBuffer);
  }

  protected virtual void ResetSmoothedPoints()
  {
    this.smoothedPoints = CanonicalSplineHelper.CreateSpline(this.ActualPoints, 0.5, (IList<double>) null, false, Math.Abs(Math.Max(this.MaxX - this.MinX, this.MaxY - this.MinY) / 200.0));
  }

  protected class Segment
  {
    public Segment(DataPoint point1, DataPoint point2)
    {
      this.Point1 = point1;
      this.Point2 = point2;
    }

    public DataPoint Point1 { get; private set; }

    public DataPoint Point2 { get; private set; }
  }
}
