// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.ContourSeries
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#nullable disable
namespace OxyPlot.Series;

public class ContourSeries : XYAxisSeries
{
  public new const string DefaultTrackerFormatString = "{0}\n{1}: {2}\n{3}: {4}\n{5}: {6}";
  private List<ContourSeries.Contour> contours;
  private List<ContourSeries.ContourSegment> segments;
  private OxyColor defaultColor;

  public ContourSeries()
  {
    this.ContourLevelStep = double.NaN;
    this.LabelSpacing = double.NaN;
    this.LabelStep = 1;
    this.LabelBackground = OxyColor.FromAColor((byte) 220, OxyColors.White);
    this.Color = OxyColors.Automatic;
    this.StrokeThickness = 1.0;
    this.LineStyle = LineStyle.Solid;
    this.TrackerFormatString = "{0}\n{1}: {2}\n{3}: {4}\n{5}: {6}";
  }

  public OxyColor Color { get; set; }

  public OxyColor ActualColor => this.Color.GetActualColor(this.defaultColor);

  public double[] ColumnCoordinates { get; set; }

  public double ContourLevelStep { get; set; }

  public double[] ContourLevels { get; set; }

  public OxyColor[] ContourColors { get; set; }

  public double[,] Data { get; set; }

  public OxyColor LabelBackground { get; set; }

  public string LabelFormatString { get; set; }

  public double LabelSpacing { get; set; }

  public int LabelStep { get; set; }

  public LineStyle LineStyle { get; set; }

  public double[] RowCoordinates { get; set; }

  public double StrokeThickness { get; set; }

  public void CalculateContours()
  {
    if (this.Data == null)
      return;
    double[] numArray = this.ContourLevels;
    this.segments = new List<ContourSeries.ContourSegment>();
    Conrec.RendererDelegate renderer = (Conrec.RendererDelegate) ((startX, startY, endX, endY, contourLevel) => this.segments.Add(new ContourSeries.ContourSegment(new DataPoint(startX, startY), new DataPoint(endX, endY), contourLevel)));
    if (numArray == null)
    {
      double val1_1 = this.Data[0, 0];
      double val1_2 = this.Data[0, 0];
      for (int index1 = 0; index1 < this.Data.GetUpperBound(0); ++index1)
      {
        for (int index2 = 0; index2 < this.Data.GetUpperBound(1); ++index2)
        {
          val1_1 = Math.Max(val1_1, this.Data[index1, index2]);
          val1_2 = Math.Min(val1_2, this.Data[index1, index2]);
        }
      }
      double num = this.ContourLevelStep;
      if (double.IsNaN(num))
        num = Math.Pow(10.0, Math.Floor(Math.Round(Math.Log(Math.Abs((val1_1 - val1_2) / 20.0), 10.0))));
      double x1 = Math.Round(num * (double) (int) Math.Ceiling(val1_1 / num), 14);
      numArray = ArrayBuilder.CreateVector(Math.Round(num * (double) (int) Math.Floor(val1_2 / num), 14), x1, num);
    }
    Conrec.Contour(this.Data, this.ColumnCoordinates, this.RowCoordinates, numArray, renderer);
    this.JoinContourSegments();
    if (this.ContourColors == null || this.ContourColors.Length == 0)
      return;
    foreach (ContourSeries.Contour contour in this.contours)
    {
      int num = ContourSeries.IndexOf((IList<double>) numArray, contour.ContourLevel);
      if (num >= 0)
      {
        int index = num % this.ContourColors.Length;
        contour.Color = this.ContourColors[index];
      }
    }
  }

  public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
  {
    TrackerHitResult nearestPoint = (TrackerHitResult) null;
    string str1 = this.XAxis.Title ?? "X";
    string str2 = this.YAxis.Title ?? "Y";
    string str3 = "Z";
    foreach (ContourSeries.Contour contour in this.contours)
    {
      TrackerHitResult trackerHitResult1 = interpolate ? this.GetNearestInterpolatedPointInternal(contour.Points, point) : this.GetNearestPointInternal((IEnumerable<DataPoint>) contour.Points, point);
      if (trackerHitResult1 != null)
      {
        if (nearestPoint != null)
        {
          ScreenPoint position = nearestPoint.Position;
          double squared1 = position.DistanceToSquared(point);
          position = trackerHitResult1.Position;
          double squared2 = position.DistanceToSquared(point);
          if (squared1 <= squared2)
            continue;
        }
        nearestPoint = trackerHitResult1;
        TrackerHitResult trackerHitResult2 = nearestPoint;
        CultureInfo actualCulture = this.ActualCulture;
        string trackerFormatString = this.TrackerFormatString;
        object[] objArray = new object[7];
        objArray[0] = (object) this.Title;
        objArray[1] = (object) str1;
        Axis xaxis = this.XAxis;
        DataPoint dataPoint = trackerHitResult1.DataPoint;
        double x = dataPoint.X;
        objArray[2] = xaxis.GetValue(x);
        objArray[3] = (object) str2;
        Axis yaxis = this.YAxis;
        dataPoint = trackerHitResult1.DataPoint;
        double y = dataPoint.Y;
        objArray[4] = yaxis.GetValue(y);
        objArray[5] = (object) str3;
        objArray[6] = (object) contour.ContourLevel;
        string str4 = StringHelper.Format((IFormatProvider) actualCulture, trackerFormatString, (object) null, objArray);
        trackerHitResult2.Text = str4;
      }
    }
    return nearestPoint;
  }

  public override void Render(IRenderContext rc)
  {
    if (this.contours == null)
      this.CalculateContours();
    if (this.contours.Count == 0)
      return;
    this.VerifyAxes();
    OxyRect clippingRect = this.GetClippingRect();
    rc.SetClip(clippingRect);
    List<ContourSeries.ContourLabel> contourLabelList = new List<ContourSeries.ContourLabel>();
    double[] dashArray = this.LineStyle.GetDashArray();
    foreach (ContourSeries.Contour contour in this.contours)
    {
      if (this.StrokeThickness > 0.0 && this.LineStyle != LineStyle.None)
      {
        ScreenPoint[] array = contour.Points.Select<DataPoint, ScreenPoint>(new Func<DataPoint, ScreenPoint>(((XYAxisSeries) this).Transform)).ToArray<ScreenPoint>();
        OxyColor actualColor = contour.Color.GetActualColor(this.ActualColor);
        rc.DrawClippedLine(clippingRect, (IList<ScreenPoint>) array, 4.0, this.GetSelectableColor(actualColor), this.StrokeThickness, dashArray, LineJoin.Miter, false);
        if (array.Length > 10)
          this.AddContourLabels(contour, array, clippingRect, (ICollection<ContourSeries.ContourLabel>) contourLabelList);
      }
    }
    foreach (ContourSeries.ContourLabel cl in contourLabelList)
      this.RenderLabelBackground(rc, cl);
    foreach (ContourSeries.ContourLabel cl in contourLabelList)
      this.RenderLabel(rc, cl);
    rc.ResetClip();
  }

  protected internal override void SetDefaultValues()
  {
    if (!this.Color.IsAutomatic())
      return;
    this.LineStyle = this.PlotModel.GetDefaultLineStyle();
    this.defaultColor = this.PlotModel.GetDefaultColor();
  }

  protected internal override void UpdateMaxMin()
  {
    this.MinX = ((IEnumerable<double>) this.ColumnCoordinates).Min();
    this.MaxX = ((IEnumerable<double>) this.ColumnCoordinates).Max();
    this.MinY = ((IEnumerable<double>) this.RowCoordinates).Min();
    this.MaxY = ((IEnumerable<double>) this.RowCoordinates).Max();
  }

  private static bool AreClose(double x1, double x2, double eps = 1E-06)
  {
    double num = x1 - x2;
    return num * num < eps;
  }

  private static bool AreClose(DataPoint p0, DataPoint p1, double eps = 1E-06)
  {
    double num1 = p0.X - p1.X;
    double num2 = p0.Y - p1.Y;
    return num1 * num1 + num2 * num2 < eps;
  }

  private static int IndexOf(IList<double> values, double value)
  {
    double num1 = double.MaxValue;
    int num2 = -1;
    for (int index = 0; index < values.Count; ++index)
    {
      double num3 = Math.Abs(values[index] - value);
      if (num3 < num1)
      {
        num1 = num3;
        num2 = index;
      }
    }
    return num2;
  }

  private void AddContourLabels(
    ContourSeries.Contour contour,
    ScreenPoint[] pts,
    OxyRect clippingRect,
    ICollection<ContourSeries.ContourLabel> contourLabels)
  {
    if (pts.Length < 2)
      return;
    double num1 = (double) (pts.Length - 1) * 0.5;
    int index1 = (int) num1;
    int index2 = index1 + 1;
    double x1 = pts[index2].X - pts[index1].X;
    double y1 = pts[index2].Y - pts[index1].Y;
    double x2 = pts[index1].X + x1 * (num1 - (double) index1);
    double y2 = pts[index1].Y + y1 * (num1 - (double) index1);
    if (!clippingRect.Contains(x2, y2))
      return;
    ScreenPoint screenPoint = new ScreenPoint(x2, y2);
    double num2 = Math.Atan2(y1, x1) * 180.0 / Math.PI;
    if (num2 > 90.0)
      num2 -= 180.0;
    if (num2 < -90.0)
      num2 += 180.0;
    string str = string.Format((IFormatProvider) this.ActualCulture, $"{{0:{this.LabelFormatString}}}", (object) contour.ContourLevel);
    contourLabels.Add(new ContourSeries.ContourLabel()
    {
      Position = screenPoint,
      Angle = num2,
      Text = str
    });
  }

  private ContourSeries.ContourSegment FindConnectedSegment(
    DataPoint point,
    double contourLevel,
    double eps,
    out bool reverse)
  {
    reverse = false;
    foreach (ContourSeries.ContourSegment segment in this.segments)
    {
      if (ContourSeries.AreClose(segment.ContourLevel, contourLevel, eps))
      {
        if (ContourSeries.AreClose(point, segment.StartPoint, eps))
          return segment;
        if (ContourSeries.AreClose(point, segment.EndPoint, eps))
        {
          reverse = true;
          return segment;
        }
      }
    }
    return (ContourSeries.ContourSegment) null;
  }

  private void JoinContourSegments(double eps = 1E-10)
  {
    this.contours = new List<ContourSeries.Contour>();
    List<DataPoint> points = new List<DataPoint>();
    int num = 0;
    ContourSeries.ContourSegment contourSegment1 = (ContourSeries.ContourSegment) null;
    int count = this.segments.Count;
    while (count > 0)
    {
      ContourSeries.ContourSegment contourSegment2 = (ContourSeries.ContourSegment) null;
      ContourSeries.ContourSegment contourSegment3 = (ContourSeries.ContourSegment) null;
      if (contourSegment1 != null)
      {
        bool reverse;
        contourSegment2 = this.FindConnectedSegment(points[0], contourSegment1.ContourLevel, eps, out reverse);
        if (contourSegment2 != null)
        {
          points.Insert(0, reverse ? contourSegment2.StartPoint : contourSegment2.EndPoint);
          ++num;
          this.segments.Remove(contourSegment2);
          --count;
        }
        contourSegment3 = this.FindConnectedSegment(points[num - 1], contourSegment1.ContourLevel, eps, out reverse);
        if (contourSegment3 != null)
        {
          points.Add(reverse ? contourSegment3.StartPoint : contourSegment3.EndPoint);
          ++num;
          this.segments.Remove(contourSegment3);
          --count;
        }
      }
      if (contourSegment2 == null && contourSegment3 == null || count == 0)
      {
        if (num > 0 && contourSegment1 != null)
        {
          this.contours.Add(new ContourSeries.Contour(points, contourSegment1.ContourLevel));
          points = new List<DataPoint>();
          num = 0;
        }
        if (count > 0)
        {
          contourSegment1 = this.segments.First<ContourSeries.ContourSegment>();
          points.Add(contourSegment1.StartPoint);
          points.Add(contourSegment1.EndPoint);
          num += 2;
          this.segments.Remove(contourSegment1);
          --count;
        }
      }
    }
  }

  private void RenderLabel(IRenderContext rc, ContourSeries.ContourLabel cl)
  {
    if (this.ActualFontSize <= 0.0)
      return;
    rc.DrawText(cl.Position, cl.Text, this.ActualTextColor, this.ActualFont, this.ActualFontSize, this.ActualFontWeight, cl.Angle, HorizontalAlignment.Center, VerticalAlignment.Middle);
  }

  private void RenderLabelBackground(IRenderContext rc, ContourSeries.ContourLabel cl)
  {
    if (this.LabelBackground.IsInvisible())
      return;
    OxySize oxySize = rc.MeasureText(cl.Text, this.ActualFont, this.ActualFontSize, this.ActualFontWeight);
    double num1 = cl.Angle / 180.0 * Math.PI;
    double num2 = Math.Cos(num1);
    double num3 = Math.Sin(num1);
    double num4 = num2 * 0.6;
    double num5 = num3 * 0.6;
    double num6 = -num3 * 0.5;
    double num7 = num2 * 0.5;
    double x = cl.Position.X;
    double y = cl.Position.Y;
    ScreenPoint[] points = new ScreenPoint[4]
    {
      new ScreenPoint(x - oxySize.Width * num4 - oxySize.Height * num6, y - oxySize.Width * num5 - oxySize.Height * num7),
      new ScreenPoint(x + oxySize.Width * num4 - oxySize.Height * num6, y + oxySize.Width * num5 - oxySize.Height * num7),
      new ScreenPoint(x + oxySize.Width * num4 + oxySize.Height * num6, y + oxySize.Width * num5 + oxySize.Height * num7),
      new ScreenPoint(x - oxySize.Width * num4 + oxySize.Height * num6, y - oxySize.Width * num5 + oxySize.Height * num7)
    };
    rc.DrawPolygon((IList<ScreenPoint>) points, this.LabelBackground, OxyColors.Undefined);
  }

  private class Contour
  {
    internal readonly double ContourLevel;
    internal readonly List<DataPoint> Points;

    public Contour(List<DataPoint> points, double contourLevel)
    {
      this.Points = points;
      this.ContourLevel = contourLevel;
      this.Color = OxyColors.Automatic;
    }

    public OxyColor Color { get; set; }
  }

  private class ContourLabel
  {
    public double Angle { get; set; }

    public ScreenPoint Position { get; set; }

    public string Text { get; set; }
  }

  private class ContourSegment
  {
    internal readonly double ContourLevel;
    internal readonly DataPoint EndPoint;
    internal readonly DataPoint StartPoint;

    public ContourSegment(DataPoint startPoint, DataPoint endPoint, double contourLevel)
    {
      this.ContourLevel = contourLevel;
      this.StartPoint = startPoint;
      this.EndPoint = endPoint;
    }
  }
}
