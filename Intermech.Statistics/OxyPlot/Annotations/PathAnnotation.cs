// Decompiled with JetBrains decompiler
// Type: OxyPlot.Annotations.PathAnnotation
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Annotations;

public abstract class PathAnnotation : TextualAnnotation
{
  private bool aliased;
  private double actualMinimumX;
  private double actualMinimumY;
  private double actualMaximumX;
  private double actualMaximumY;
  private IList<ScreenPoint> screenPoints;

  protected PathAnnotation()
  {
    this.MinimumX = double.MinValue;
    this.MaximumX = double.MaxValue;
    this.MinimumY = double.MinValue;
    this.MaximumY = double.MaxValue;
    this.Color = OxyColors.Blue;
    this.StrokeThickness = 1.0;
    this.LineStyle = LineStyle.Dash;
    this.LineJoin = LineJoin.Miter;
    this.ClipByXAxis = true;
    this.ClipByYAxis = true;
    this.aliased = false;
    this.TextLinePosition = 1.0;
    this.TextOrientation = AnnotationTextOrientation.AlongLine;
    this.TextMargin = 12.0;
    this.ClipText = true;
    this.TextHorizontalAlignment = HorizontalAlignment.Right;
    this.TextVerticalAlignment = VerticalAlignment.Top;
  }

  public OxyColor Color { get; set; }

  public LineJoin LineJoin { get; set; }

  public LineStyle LineStyle { get; set; }

  public double MaximumX { get; set; }

  public double MaximumY { get; set; }

  public double MinimumX { get; set; }

  public double MinimumY { get; set; }

  public double StrokeThickness { get; set; }

  public double TextMargin { get; set; }

  public double TextPadding { get; set; }

  public AnnotationTextOrientation TextOrientation { get; set; }

  public double TextLinePosition { get; set; }

  public bool ClipText { get; set; }

  public bool ClipByXAxis { get; set; }

  public bool ClipByYAxis { get; set; }

  protected bool Aliased
  {
    get => this.aliased;
    set => this.aliased = value;
  }

  protected double ActualMinimumX
  {
    get => this.actualMinimumX;
    set => this.actualMinimumX = value;
  }

  protected double ActualMinimumY
  {
    get => this.actualMinimumY;
    set => this.actualMinimumY = value;
  }

  protected double ActualMaximumX
  {
    get => this.actualMaximumX;
    set => this.actualMaximumX = value;
  }

  protected double ActualMaximumY
  {
    get => this.actualMaximumY;
    set => this.actualMaximumY = value;
  }

  public override void Render(IRenderContext rc)
  {
    base.Render(rc);
    this.CalculateActualMinimumsMaximums();
    this.screenPoints = this.GetScreenPoints();
    OxyRect plotArea;
    ScreenPoint screenPoint;
    double x0;
    if (!this.ClipByXAxis)
    {
      plotArea = this.PlotModel.PlotArea;
      x0 = plotArea.Left;
    }
    else
    {
      screenPoint = this.XAxis.ScreenMin;
      x0 = screenPoint.X;
    }
    double y0;
    if (!this.ClipByYAxis)
    {
      plotArea = this.PlotModel.PlotArea;
      y0 = plotArea.Top;
    }
    else
    {
      screenPoint = this.YAxis.ScreenMin;
      y0 = screenPoint.Y;
    }
    double x1;
    if (!this.ClipByXAxis)
    {
      plotArea = this.PlotModel.PlotArea;
      x1 = plotArea.Right;
    }
    else
    {
      screenPoint = this.XAxis.ScreenMax;
      x1 = screenPoint.X;
    }
    double y1;
    if (!this.ClipByYAxis)
    {
      plotArea = this.PlotModel.PlotArea;
      y1 = plotArea.Bottom;
    }
    else
    {
      screenPoint = this.YAxis.ScreenMax;
      y1 = screenPoint.Y;
    }
    OxyRect oxyRect = OxyRect.Create(x0, y0, x1, y1);
    List<ScreenPoint> pts = new List<ScreenPoint>();
    double[] dashArray = this.LineStyle.GetDashArray();
    rc.DrawClippedLine(oxyRect, this.screenPoints, 16.0, this.GetSelectableColor(this.Color), this.StrokeThickness, dashArray, this.LineJoin, this.aliased, pointsRendered: new Action<IList<ScreenPoint>>(pts.AddRange));
    double textMargin = this.TextMargin;
    double margin = this.TextHorizontalAlignment != HorizontalAlignment.Center ? textMargin * (this.TextLinePosition < 0.5 ? 1.0 : -1.0) : 0.0;
    double angle;
    ScreenPoint position;
    if (!PathAnnotation.GetPointAtRelativeDistance((IList<ScreenPoint>) pts, this.TextLinePosition, margin, out position, out angle))
      return;
    if (angle < -90.0)
      angle += 180.0;
    if (angle > 90.0)
      angle -= 180.0;
    switch (this.TextOrientation)
    {
      case AnnotationTextOrientation.Horizontal:
        angle = 0.0;
        break;
      case AnnotationTextOrientation.Vertical:
        angle = -90.0;
        break;
    }
    double num1 = angle / 180.0 * Math.PI;
    int num2 = 1;
    if (this.TextHorizontalAlignment == HorizontalAlignment.Right)
      num2 = -1;
    if (this.TextHorizontalAlignment == HorizontalAlignment.Center)
      num2 = 0;
    position += new ScreenVector((double) num2 * this.TextPadding * Math.Cos(num1), (double) num2 * this.TextPadding * Math.Sin(num1));
    if (string.IsNullOrEmpty(this.Text))
      return;
    ScreenPoint actualTextPosition = this.GetActualTextPosition((Func<ScreenPoint>) (() => position));
    if (this.TextPosition.IsDefined())
      angle = this.TextRotation;
    if (this.ClipText)
    {
      if (!new CohenSutherlandClipping(oxyRect).IsInside(position))
        return;
      rc.DrawClippedText(oxyRect, actualTextPosition, this.Text, this.ActualTextColor, this.ActualFont, this.ActualFontSize, this.ActualFontWeight, angle, this.TextHorizontalAlignment, this.TextVerticalAlignment);
    }
    else
      rc.DrawText(actualTextPosition, this.Text, this.ActualTextColor, this.ActualFont, this.ActualFontSize, this.ActualFontWeight, angle, this.TextHorizontalAlignment, this.TextVerticalAlignment);
  }

  protected override HitTestResult HitTestOverride(HitTestArguments args)
  {
    if (this.screenPoints == null)
      return (HitTestResult) null;
    ScreenPoint nearestPointOnPolyline = ScreenPointHelper.FindNearestPointOnPolyline(args.Point, this.screenPoints);
    return (args.Point - nearestPointOnPolyline).Length < args.Tolerance ? new HitTestResult((UIElement) this, nearestPointOnPolyline) : (HitTestResult) null;
  }

  protected abstract IList<ScreenPoint> GetScreenPoints();

  protected virtual void CalculateActualMinimumsMaximums()
  {
    this.actualMinimumX = Math.Max(this.MinimumX, this.XAxis.ActualMinimum);
    this.actualMaximumX = Math.Min(this.MaximumX, this.XAxis.ActualMaximum);
    this.actualMinimumY = Math.Max(this.MinimumY, this.YAxis.ActualMinimum);
    this.actualMaximumY = Math.Min(this.MaximumY, this.YAxis.ActualMaximum);
    if (!this.ClipByXAxis)
    {
      double val2 = this.XAxis.InverseTransform(this.PlotModel.PlotArea.Right);
      double val1 = this.XAxis.InverseTransform(this.PlotModel.PlotArea.Left);
      this.actualMaximumX = Math.Max(val1, val2);
      this.actualMinimumX = Math.Min(val1, val2);
    }
    if (this.ClipByYAxis)
      return;
    double val2_1 = this.YAxis.InverseTransform(this.PlotModel.PlotArea.Bottom);
    double val1_1 = this.YAxis.InverseTransform(this.PlotModel.PlotArea.Top);
    this.actualMaximumY = Math.Max(val1_1, val2_1);
    this.actualMinimumY = Math.Min(val1_1, val2_1);
  }

  private static bool GetPointAtRelativeDistance(
    IList<ScreenPoint> pts,
    double p,
    double margin,
    out ScreenPoint position,
    out double angle)
  {
    if (pts == null || pts.Count == 0)
    {
      position = new ScreenPoint();
      angle = 0.0;
      return false;
    }
    double num1 = 0.0;
    ScreenVector screenVector;
    for (int index = 1; index < pts.Count; ++index)
    {
      double num2 = num1;
      screenVector = pts[index] - pts[index - 1];
      double length = screenVector.Length;
      num1 = num2 + length;
    }
    double num3 = num1 * p + margin;
    double num4 = 1E-08;
    double num5 = 0.0;
    for (int index = 1; index < pts.Count; ++index)
    {
      screenVector = pts[index] - pts[index - 1];
      double length = screenVector.Length;
      if (num3 >= num5 - num4 && num3 <= num5 + length + num4)
      {
        double num6 = (num3 - num5) / length;
        double x1 = pts[index].X * num6 + pts[index - 1].X * (1.0 - num6);
        double y1 = pts[index].Y * num6 + pts[index - 1].Y * (1.0 - num6);
        position = new ScreenPoint(x1, y1);
        double x2 = pts[index].X - pts[index - 1].X;
        double y2 = pts[index].Y - pts[index - 1].Y;
        angle = Math.Atan2(y2, x2) / Math.PI * 180.0;
        return true;
      }
      num5 += length;
    }
    position = pts[0];
    angle = 0.0;
    return false;
  }
}
