// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.ScatterErrorSeries
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OxyPlot.Series;

public class ScatterErrorSeries : ScatterSeries<ScatterErrorPoint>
{
  public ScatterErrorSeries()
  {
    this.ErrorBarColor = OxyColors.Black;
    this.ErrorBarStrokeThickness = 1.0;
    this.ErrorBarStopWidth = 4.0;
    this.MinimumErrorSize = 0.0;
  }

  public string DataFieldErrorX { get; set; }

  public string DataFieldErrorY { get; set; }

  public OxyColor ErrorBarColor { get; set; }

  public double ErrorBarStopWidth { get; set; }

  public double ErrorBarStrokeThickness { get; set; }

  public double MinimumErrorSize { get; set; }

  public override void Render(IRenderContext rc)
  {
    base.Render(rc);
    List<ScatterErrorPoint> actualPointsList = this.ActualPointsList;
    if (actualPointsList == null || actualPointsList.Count == 0)
      return;
    OxyRect clippingRect = this.GetClippingRect();
    List<ScreenPoint> points = new List<ScreenPoint>();
    foreach (ScatterErrorPoint scatterErrorPoint in actualPointsList)
    {
      if (scatterErrorPoint != null)
      {
        if (scatterErrorPoint.ErrorX > 0.0)
        {
          ScreenPoint screenPoint1 = this.XAxis.Transform(scatterErrorPoint.X - scatterErrorPoint.ErrorX * 0.5, scatterErrorPoint.Y, this.YAxis);
          ScreenPoint screenPoint2 = this.XAxis.Transform(scatterErrorPoint.X + scatterErrorPoint.ErrorX * 0.5, scatterErrorPoint.Y, this.YAxis);
          if (Math.Abs(screenPoint2.X - screenPoint1.X) > this.MarkerSize * this.MinimumErrorSize)
          {
            points.Add(screenPoint1);
            points.Add(screenPoint2);
            points.Add(new ScreenPoint(screenPoint1.X, screenPoint1.Y - this.ErrorBarStopWidth));
            points.Add(new ScreenPoint(screenPoint1.X, screenPoint1.Y + this.ErrorBarStopWidth));
            points.Add(new ScreenPoint(screenPoint2.X, screenPoint2.Y - this.ErrorBarStopWidth));
            points.Add(new ScreenPoint(screenPoint2.X, screenPoint2.Y + this.ErrorBarStopWidth));
          }
        }
        if (scatterErrorPoint.ErrorY > 0.0)
        {
          ScreenPoint screenPoint3 = this.XAxis.Transform(scatterErrorPoint.X, scatterErrorPoint.Y - scatterErrorPoint.ErrorY * 0.5, this.YAxis);
          ScreenPoint screenPoint4 = this.XAxis.Transform(scatterErrorPoint.X, scatterErrorPoint.Y + scatterErrorPoint.ErrorY * 0.5, this.YAxis);
          if (Math.Abs(screenPoint3.Y - screenPoint4.Y) > this.MarkerSize * this.MinimumErrorSize)
          {
            points.Add(screenPoint3);
            points.Add(screenPoint4);
            points.Add(new ScreenPoint(screenPoint3.X - this.ErrorBarStopWidth, screenPoint3.Y));
            points.Add(new ScreenPoint(screenPoint3.X + this.ErrorBarStopWidth, screenPoint3.Y));
            points.Add(new ScreenPoint(screenPoint4.X - this.ErrorBarStopWidth, screenPoint4.Y));
            points.Add(new ScreenPoint(screenPoint4.X + this.ErrorBarStopWidth, screenPoint4.Y));
          }
        }
      }
    }
    rc.DrawClippedLineSegments(clippingRect, (IList<ScreenPoint>) points, this.GetSelectableColor(this.ErrorBarColor), this.ErrorBarStrokeThickness, (double[]) null, LineJoin.Bevel, true);
  }

  protected override void UpdateFromDataFields()
  {
    OxyPlot.ListBuilder<ScatterErrorPoint> listBuilder = new OxyPlot.ListBuilder<ScatterErrorPoint>();
    listBuilder.Add<double>(this.DataFieldX, double.NaN);
    listBuilder.Add<double>(this.DataFieldY, double.NaN);
    listBuilder.Add<double>(this.DataFieldErrorX, double.NaN);
    listBuilder.Add<double>(this.DataFieldErrorY, double.NaN);
    listBuilder.Add<double>(this.DataFieldSize, double.NaN);
    listBuilder.Add<double>(this.DataFieldValue, double.NaN);
    listBuilder.Add<object>(this.DataFieldTag, (object) null);
    listBuilder.FillT((IList<ScatterErrorPoint>) this.ItemsSourcePoints, this.ItemsSource, (Func<IList<object>, ScatterErrorPoint>) (args => new ScatterErrorPoint(Convert.ToDouble(args[0]), Convert.ToDouble(args[1]), Convert.ToDouble(args[2]), Convert.ToDouble(args[3]), Convert.ToDouble(args[4]), Convert.ToDouble(args[5]), args[6])));
  }

  public void SelectAll(Func<ScatterErrorPoint, bool> func)
  {
    foreach (ScatterErrorPoint scatterErrorPoint in this.Points.Where<ScatterErrorPoint>(func))
      this.SelectItem(this.Points.IndexOf(scatterErrorPoint));
  }
}
