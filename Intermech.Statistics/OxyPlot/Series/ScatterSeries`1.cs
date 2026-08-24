// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.ScatterSeries`1
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#nullable disable
namespace OxyPlot.Series;

public abstract class ScatterSeries<T> : XYAxisSeries where T : ScatterPoint
{
  private const string DefaultColorAxisTitle = "Value";
  private readonly List<T> points = new List<T>();
  private OxyColor defaultMarkerFillColor;

  protected ScatterSeries()
  {
    this.MarkerFill = OxyColors.Automatic;
    this.MarkerSize = 5.0;
    this.MarkerType = MarkerType.Square;
    this.MarkerStroke = OxyColors.Automatic;
    this.MarkerStrokeThickness = 1.0;
    this.LabelMargin = 6.0;
  }

  public List<T> Points => this.points;

  public string LabelFormatString { get; set; }

  public double LabelMargin { get; set; }

  public Func<object, T> Mapping { get; set; }

  public int BinSize { get; set; }

  public IColorAxis ColorAxis { get; private set; }

  public string ColorAxisKey { get; set; }

  public string DataFieldX { get; set; }

  public string DataFieldY { get; set; }

  public string DataFieldSize { get; set; }

  public string DataFieldTag { get; set; }

  public string DataFieldValue { get; set; }

  public OxyColor MarkerFill { get; set; }

  public OxyColor ActualMarkerFillColor
  {
    get => this.MarkerFill.GetActualColor(this.defaultMarkerFillColor);
  }

  public ScreenPoint[] MarkerOutline { get; set; }

  public double MarkerSize { get; set; }

  public OxyColor MarkerStroke { get; set; }

  public double MarkerStrokeThickness { get; set; }

  public MarkerType MarkerType { get; set; }

  public double MaxValue { get; private set; }

  public double MinValue { get; private set; }

  public ReadOnlyCollection<T> ActualPoints
  {
    get
    {
      return this.ActualPointsList == null ? (ReadOnlyCollection<T>) null : new ReadOnlyCollection<T>((IList<T>) this.ActualPointsList);
    }
  }

  protected List<T> ActualPointsList
  {
    get => this.ItemsSource == null ? this.points : this.ItemsSourcePoints;
  }

  protected List<T> ItemsSourcePoints { get; set; }

  protected bool OwnsItemsSourcePoints { get; set; }

  public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
  {
    if (this.XAxis == null || this.YAxis == null)
      return (TrackerHitResult) null;
    if (interpolate)
      return (TrackerHitResult) null;
    List<T> actualPointsList = this.ActualPointsList;
    if (actualPointsList == null || actualPointsList.Count == 0)
      return (TrackerHitResult) null;
    TrackerHitResult nearestPoint = (TrackerHitResult) null;
    double num1 = double.MaxValue;
    int i = 0;
    string str1 = this.XAxis.Title ?? "X";
    string str2 = this.YAxis.Title ?? "Y";
    string str3 = (this.ColorAxis != null ? ((Axis) this.ColorAxis).Title : (string) null) ?? "Value";
    foreach (T obj1 in actualPointsList)
    {
      if (obj1.X < this.XAxis.ActualMinimum || obj1.X > this.XAxis.ActualMaximum || obj1.Y < this.YAxis.ActualMinimum || obj1.Y > this.YAxis.ActualMaximum)
      {
        ++i;
      }
      else
      {
        ScreenPoint screenPoint = this.XAxis.Transform(obj1.X, obj1.Y, this.YAxis);
        double num2 = screenPoint.x - point.x;
        double num3 = screenPoint.y - point.y;
        double num4 = num2 * num2 + num3 * num3;
        if (num4 < num1)
        {
          object obj2 = this.GetItem(i) ?? (object) obj1;
          object obj3 = (object) null;
          if (!double.IsNaN(obj1.Value) && !double.IsInfinity(obj1.Value))
            obj3 = (object) obj1.Value;
          nearestPoint = new TrackerHitResult()
          {
            Series = (OxyPlot.Series.Series) this,
            DataPoint = new DataPoint(obj1.X, obj1.Y),
            Position = screenPoint,
            Item = obj2,
            Index = (double) i,
            Text = StringHelper.Format((IFormatProvider) this.ActualCulture, this.TrackerFormatString, obj2, (object) this.Title, (object) str1, this.XAxis.GetValue(obj1.X), (object) str2, this.YAxis.GetValue(obj1.Y), (object) str3, obj3)
          };
          num1 = num4;
        }
        ++i;
      }
    }
    return nearestPoint;
  }

  public override void Render(IRenderContext rc)
  {
    List<T> actualPointsList = this.ActualPointsList;
    if (actualPointsList == null || actualPointsList.Count == 0)
      return;
    OxyRect clippingRect = this.GetClippingRect();
    int count = actualPointsList.Count;
    List<ScreenPoint> markerPoints1 = new List<ScreenPoint>(count);
    List<double> markerSize1 = new List<double>(count);
    List<ScreenPoint> markerPoints2 = new List<ScreenPoint>();
    List<double> markerSize2 = new List<double>(count);
    Dictionary<int, IList<ScreenPoint>> dictionary1 = new Dictionary<int, IList<ScreenPoint>>();
    Dictionary<int, IList<double>> dictionary2 = new Dictionary<int, IList<double>>();
    bool flag1 = this.IsSelected();
    for (int index = 0; index < count; ++index)
    {
      DataPoint pt = new DataPoint(actualPointsList[index].X, actualPointsList[index].Y);
      if (this.IsValidPoint(pt))
      {
        double d1 = double.NaN;
        double d2 = double.NaN;
        T obj = actualPointsList[index];
        if ((object) obj != null)
        {
          d1 = obj.Size;
          d2 = obj.Value;
        }
        if (double.IsNaN(d1))
          d1 = this.MarkerSize;
        ScreenPoint screenPoint = this.XAxis.Transform(pt.X, pt.Y, this.YAxis);
        if (flag1 && this.IsItemSelected(index))
        {
          markerPoints2.Add(screenPoint);
          markerSize2.Add(d1);
        }
        else if (this.ColorAxis != null)
        {
          if (!double.IsNaN(d2))
          {
            int paletteIndex = this.ColorAxis.GetPaletteIndex(d2);
            if (!dictionary1.ContainsKey(paletteIndex))
            {
              dictionary1.Add(paletteIndex, (IList<ScreenPoint>) new List<ScreenPoint>());
              dictionary2.Add(paletteIndex, (IList<double>) new List<double>());
            }
            dictionary1[paletteIndex].Add(screenPoint);
            dictionary2[paletteIndex].Add(d1);
          }
        }
        else
        {
          markerPoints1.Add(screenPoint);
          markerSize1.Add(d1);
        }
      }
    }
    ScreenPoint binOffset = this.XAxis.Transform(this.MinX, this.MaxY, this.YAxis);
    rc.SetClip(clippingRect);
    if (this.ColorAxis != null)
    {
      bool flag2 = this.MarkerType == MarkerType.Plus || this.MarkerType == MarkerType.Star || this.MarkerType == MarkerType.Cross;
      foreach (KeyValuePair<int, IList<ScreenPoint>> keyValuePair in dictionary1)
      {
        OxyColor color = this.ColorAxis.GetColor(keyValuePair.Key);
        rc.DrawMarkers(clippingRect, keyValuePair.Value, this.MarkerType, (IList<ScreenPoint>) this.MarkerOutline, dictionary2[keyValuePair.Key], this.MarkerFill.GetActualColor(color), flag2 ? color : this.MarkerStroke, this.MarkerStrokeThickness, this.BinSize, binOffset);
      }
    }
    rc.DrawMarkers(clippingRect, (IList<ScreenPoint>) markerPoints1, this.MarkerType, (IList<ScreenPoint>) this.MarkerOutline, (IList<double>) markerSize1, this.ActualMarkerFillColor, this.MarkerStroke, this.MarkerStrokeThickness, this.BinSize, binOffset);
    rc.DrawMarkers(clippingRect, (IList<ScreenPoint>) markerPoints2, this.MarkerType, (IList<ScreenPoint>) this.MarkerOutline, (IList<double>) markerSize2, this.PlotModel.SelectionColor, this.PlotModel.SelectionColor, this.MarkerStrokeThickness, this.BinSize, binOffset);
    if (this.LabelFormatString != null)
      this.RenderPointLabels(rc, clippingRect);
    rc.ResetClip();
  }

  public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
  {
    ScreenPoint p = new ScreenPoint((legendBox.Left + legendBox.Right) / 2.0, (legendBox.Top + legendBox.Bottom) / 2.0);
    rc.DrawMarker(legendBox, p, this.MarkerType, (IList<ScreenPoint>) this.MarkerOutline, this.MarkerSize, this.IsSelected() ? this.PlotModel.SelectionColor : this.ActualMarkerFillColor, this.IsSelected() ? this.PlotModel.SelectionColor : this.MarkerStroke, this.MarkerStrokeThickness);
  }

  protected internal override void EnsureAxes()
  {
    base.EnsureAxes();
    this.ColorAxis = this.PlotModel.GetAxisOrDefault(this.ColorAxisKey, (Axis) this.PlotModel.DefaultColorAxis) as IColorAxis;
  }

  protected internal override void SetDefaultValues()
  {
    if (!this.MarkerFill.IsAutomatic())
      return;
    this.defaultMarkerFillColor = this.PlotModel.GetDefaultColor();
  }

  protected internal override void UpdateData()
  {
    if (this.ItemsSource == null)
      return;
    this.UpdateItemsSourcePoints();
  }

  protected internal override void UpdateMaxMin()
  {
    base.UpdateMaxMin();
    this.InternalUpdateMaxMinValue(this.ActualPointsList);
  }

  protected void RenderPointLabels(IRenderContext rc, OxyRect clippingRect)
  {
    List<T> actualPointsList = this.ActualPointsList;
    if (actualPointsList == null || actualPointsList.Count == 0)
      return;
    int i = -1;
    foreach (T obj in actualPointsList)
    {
      ++i;
      DataPoint dataPoint = new DataPoint(obj.X, obj.Y);
      if (this.IsValidPoint(dataPoint))
      {
        ScreenPoint p = this.Transform(dataPoint) + new ScreenVector(0.0, -this.LabelMargin);
        if (clippingRect.Contains(p))
        {
          string text = StringHelper.Format((IFormatProvider) this.ActualCulture, this.LabelFormatString, this.GetItem(i), (object) obj.X, (object) obj.Y);
          rc.DrawClippedText(clippingRect, p, text, this.ActualTextColor, this.ActualFont, this.ActualFontSize, this.ActualFontWeight, horizontalAlignment: HorizontalAlignment.Center, verticalAlignment: VerticalAlignment.Bottom);
        }
      }
    }
  }

  protected void InternalUpdateMaxMinValue(List<T> pts)
  {
    if (pts == null || pts.Count == 0)
      return;
    double d1 = double.MaxValue;
    double d2 = double.MaxValue;
    double d3 = double.MaxValue;
    double d4 = double.MinValue;
    double d5 = double.MinValue;
    double d6 = double.MinValue;
    if (double.IsNaN(d1))
      d1 = double.MaxValue;
    if (double.IsNaN(d2))
      d2 = double.MaxValue;
    if (double.IsNaN(d4))
      d4 = double.MinValue;
    if (double.IsNaN(d5))
      d5 = double.MinValue;
    if (double.IsNaN(d3))
      d3 = double.MinValue;
    if (double.IsNaN(d6))
      d6 = double.MinValue;
    foreach (T pt in pts)
    {
      double x = pt.X;
      double y = pt.Y;
      if (!double.IsNaN(x) && !double.IsNaN(y))
      {
        double num = pt.Value;
        if (x < d1)
          d1 = x;
        if (x > d4)
          d4 = x;
        if (y < d2)
          d2 = y;
        if (y > d5)
          d5 = y;
        if (num < d3)
          d3 = num;
        if (num > d6)
          d6 = num;
      }
    }
    if (d1 < double.MaxValue)
      this.MinX = d1;
    if (d2 < double.MaxValue)
      this.MinY = d2;
    if (d4 > double.MinValue)
      this.MaxX = d4;
    if (d5 > double.MinValue)
      this.MaxY = d5;
    if (d3 < double.MaxValue)
      this.MinValue = d3;
    if (d6 > double.MinValue)
      this.MaxValue = d6;
    if (!(this.ColorAxis is Axis colorAxis))
      return;
    colorAxis.Include(this.MinValue);
    colorAxis.Include(this.MaxValue);
  }

  protected void InternalUpdateMaxMinValue(IList<ScatterPoint> pts)
  {
    if (pts == null || pts.Count == 0)
      return;
    double d1 = double.NaN;
    double d2 = double.NaN;
    foreach (ScatterPoint pt in (IEnumerable<ScatterPoint>) pts)
    {
      double num = pt.Value;
      if (num < d1 || double.IsNaN(d1))
        d1 = num;
      if (num > d2 || double.IsNaN(d2))
        d2 = num;
    }
    this.MinValue = d1;
    this.MaxValue = d2;
    if (!(this.ColorAxis is Axis colorAxis))
      return;
    colorAxis.Include(this.MinValue);
    colorAxis.Include(this.MaxValue);
  }

  protected void ClearItemsSourcePoints()
  {
    if (!this.OwnsItemsSourcePoints || this.ItemsSourcePoints == null)
      this.ItemsSourcePoints = new List<T>();
    else
      this.ItemsSourcePoints.Clear();
    this.OwnsItemsSourcePoints = true;
  }

  private void UpdateItemsSourcePoints()
  {
    if (this.Mapping != null)
    {
      this.ClearItemsSourcePoints();
      foreach (object obj in this.ItemsSource)
        this.ItemsSourcePoints.Add(this.Mapping(obj));
    }
    else if (this.ItemsSource is List<T> itemsSource1)
    {
      this.ItemsSourcePoints = itemsSource1;
      this.OwnsItemsSourcePoints = false;
    }
    else
    {
      this.ClearItemsSourcePoints();
      if (this.ItemsSource is IEnumerable<T> itemsSource)
        this.ItemsSourcePoints.AddRange(itemsSource);
      else if (this.DataFieldX == null || this.DataFieldY == null)
      {
        foreach (object obj1 in this.ItemsSource)
        {
          if (obj1 is T obj2)
            this.ItemsSourcePoints.Add(obj2);
          else if (obj1 is IScatterPointProvider scatterPointProvider)
            this.ItemsSourcePoints.Add((T) scatterPointProvider.GetScatterPoint());
        }
      }
      else
        this.UpdateFromDataFields();
    }
  }

  protected abstract void UpdateFromDataFields();
}
