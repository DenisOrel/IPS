// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.DataPointSeries
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace OxyPlot.Series;

public abstract class DataPointSeries : XYAxisSeries
{
  private readonly List<DataPoint> points = new List<DataPoint>();
  private List<DataPoint> itemsSourcePoints;
  private bool ownsItemsSourcePoints;

  public bool CanTrackerInterpolatePoints { get; set; }

  public string DataFieldX { get; set; }

  public string DataFieldY { get; set; }

  public Func<object, DataPoint> Mapping { get; set; }

  public List<DataPoint> Points => this.points;

  protected List<DataPoint> ActualPoints
  {
    get => this.ItemsSource == null ? this.points : this.itemsSourcePoints;
  }

  public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
  {
    if (interpolate && !this.CanTrackerInterpolatePoints)
      return (TrackerHitResult) null;
    TrackerHitResult nearestPoint = (TrackerHitResult) null;
    if (interpolate)
      nearestPoint = this.GetNearestInterpolatedPointInternal(this.ActualPoints, point);
    if (nearestPoint == null)
      nearestPoint = this.GetNearestPointInternal((IEnumerable<DataPoint>) this.ActualPoints, point);
    if (nearestPoint != null)
    {
      TrackerHitResult trackerHitResult = nearestPoint;
      CultureInfo actualCulture = this.ActualCulture;
      string trackerFormatString = this.TrackerFormatString;
      object obj = nearestPoint.Item;
      object[] objArray = new object[5]
      {
        (object) this.Title,
        (object) (this.XAxis.Title ?? "X"),
        null,
        null,
        null
      };
      Axis xaxis = this.XAxis;
      DataPoint dataPoint = nearestPoint.DataPoint;
      double x = dataPoint.X;
      objArray[2] = xaxis.GetValue(x);
      objArray[3] = (object) (this.YAxis.Title ?? "Y");
      Axis yaxis = this.YAxis;
      dataPoint = nearestPoint.DataPoint;
      double y = dataPoint.Y;
      objArray[4] = yaxis.GetValue(y);
      string str = StringHelper.Format((IFormatProvider) actualCulture, trackerFormatString, obj, objArray);
      trackerHitResult.Text = str;
    }
    return nearestPoint;
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
    this.InternalUpdateMaxMin(this.ActualPoints);
  }

  protected override object GetItem(int i)
  {
    List<DataPoint> actualPoints = this.ActualPoints;
    return this.ItemsSource == null && actualPoints != null && i < actualPoints.Count ? (object) actualPoints[i] : base.GetItem(i);
  }

  private void ClearItemsSourcePoints()
  {
    if (!this.ownsItemsSourcePoints || this.itemsSourcePoints == null)
      this.itemsSourcePoints = new List<DataPoint>();
    else
      this.itemsSourcePoints.Clear();
    this.ownsItemsSourcePoints = true;
  }

  private void UpdateItemsSourcePoints()
  {
    if (this.Mapping != null)
    {
      this.ClearItemsSourcePoints();
      foreach (object obj in this.ItemsSource)
        this.itemsSourcePoints.Add(this.Mapping(obj));
    }
    else if (this.ItemsSource is List<DataPoint> itemsSource1)
    {
      this.itemsSourcePoints = itemsSource1;
      this.ownsItemsSourcePoints = false;
    }
    else
    {
      this.ClearItemsSourcePoints();
      if (this.ItemsSource is IEnumerable<DataPoint> itemsSource)
        this.itemsSourcePoints.AddRange(itemsSource);
      else if (this.DataFieldX == null || this.DataFieldY == null)
      {
        foreach (object obj in this.ItemsSource)
        {
          if (obj is DataPoint dataPoint)
            this.itemsSourcePoints.Add(dataPoint);
          else if (obj is IDataPointProvider dataPointProvider)
            this.itemsSourcePoints.Add(dataPointProvider.GetDataPoint());
        }
      }
      else
        this.itemsSourcePoints.AddRange(this.ItemsSource, this.DataFieldX, this.DataFieldY);
    }
  }
}
