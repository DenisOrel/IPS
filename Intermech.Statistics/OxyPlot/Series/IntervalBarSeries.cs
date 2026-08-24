// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.IntervalBarSeries
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OxyPlot.Series;

public class IntervalBarSeries : CategorizedSeries, IStackableSeries
{
  public new const string DefaultTrackerFormatString = "{0}\n{1}: {2}\n{3}: {4}";
  private OxyColor defaultFillColor;

  public IntervalBarSeries()
  {
    this.Items = (IList<IntervalBarItem>) new List<IntervalBarItem>();
    this.FillColor = OxyColors.Automatic;
    this.LabelColor = OxyColors.Automatic;
    this.StrokeColor = OxyColors.Black;
    this.StrokeThickness = 1.0;
    this.BarWidth = 1.0;
    this.TrackerFormatString = "{0}\n{1}: {2}\n{3}: {4}";
    this.LabelMargin = 4.0;
    this.LabelFormatString = "{2}";
  }

  public double BarWidth { get; set; }

  public OxyColor FillColor { get; set; }

  public OxyColor ActualFillColor => this.FillColor.GetActualColor(this.defaultFillColor);

  public bool IsStacked => true;

  public IList<IntervalBarItem> Items { get; private set; }

  public OxyColor LabelColor { get; set; }

  public string LabelField { get; set; }

  public string LabelFormatString { get; set; }

  public double LabelMargin { get; set; }

  public string MaximumField { get; set; }

  public string MinimumField { get; set; }

  public string StackGroup => string.Empty;

  public OxyColor StrokeColor { get; set; }

  public double StrokeThickness { get; set; }

  protected internal IList<OxyRect> ActualBarRectangles { get; set; }

  protected internal IList<IntervalBarItem> ValidItems { get; set; }

  protected internal Dictionary<int, int> ValidItemsIndexInversion { get; set; }

  public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
  {
    for (int index = 0; index < this.ActualBarRectangles.Count; ++index)
    {
      if (this.ActualBarRectangles[index].Contains(point))
      {
        IntervalBarItem intervalBarItem = (IntervalBarItem) this.GetItem(this.ValidItemsIndexInversion[index]);
        int categoryIndex = intervalBarItem.GetCategoryIndex(index);
        double y = (this.ValidItems[index].Start + this.ValidItems[index].End) / 2.0;
        DataPoint dataPoint = new DataPoint((double) categoryIndex, y);
        CategoryAxis categoryAxis = this.GetCategoryAxis();
        Axis valueAxis = this.GetValueAxis();
        return new TrackerHitResult()
        {
          Series = (OxyPlot.Series.Series) this,
          DataPoint = dataPoint,
          Position = point,
          Item = (object) intervalBarItem,
          Index = (double) index,
          Text = StringHelper.Format((IFormatProvider) this.ActualCulture, this.TrackerFormatString, (object) intervalBarItem, (object) this.Title, (object) (categoryAxis.Title ?? "Category"), (object) categoryAxis.FormatValue((double) categoryIndex), (object) (valueAxis.Title ?? "Value"), valueAxis.GetValue(this.Items[index].Start), valueAxis.GetValue(this.Items[index].End), (object) this.Items[index].Title)
        };
      }
    }
    return (TrackerHitResult) null;
  }

  public virtual bool IsValidPoint(double v, Axis yaxis)
  {
    return !double.IsNaN(v) && !double.IsInfinity(v);
  }

  public override void Render(IRenderContext rc)
  {
    this.ActualBarRectangles = (IList<OxyRect>) new List<OxyRect>();
    if (this.ValidItems.Count == 0)
      return;
    OxyRect clippingRect = this.GetClippingRect();
    CategoryAxis categoryAxis = this.GetCategoryAxis();
    double actualBarWidth = this.GetActualBarWidth();
    int stackIndex = categoryAxis.GetStackIndex(this.StackGroup);
    for (int index = 0; index < this.ValidItems.Count; ++index)
    {
      IntervalBarItem validItem = this.ValidItems[index];
      int categoryIndex = validItem.GetCategoryIndex(index);
      double categoryValue = categoryAxis.GetCategoryValue(categoryIndex, stackIndex, actualBarWidth);
      ScreenPoint screenPoint1 = this.Transform(validItem.Start, categoryValue);
      ScreenPoint screenPoint2 = this.Transform(validItem.End, categoryValue + actualBarWidth);
      OxyRect rect = OxyRect.Create(screenPoint1.X, screenPoint1.Y, screenPoint2.X, screenPoint2.Y);
      this.ActualBarRectangles.Add(rect);
      rc.DrawClippedRectangleAsPolygon(clippingRect, rect, this.GetSelectableFillColor(validItem.Color.GetActualColor(this.ActualFillColor)), this.StrokeColor, this.StrokeThickness);
      if (this.LabelFormatString != null)
      {
        string text = StringHelper.Format((IFormatProvider) this.ActualCulture, this.LabelFormatString, this.GetItem(index), (object) validItem.Start, (object) validItem.End, (object) validItem.Title);
        ScreenPoint p = new ScreenPoint((rect.Left + rect.Right) / 2.0, (rect.Top + rect.Bottom) / 2.0);
        rc.DrawClippedText(clippingRect, p, text, this.ActualTextColor, this.ActualFont, this.ActualFontSize, this.ActualFontWeight, horizontalAlignment: HorizontalAlignment.Center, verticalAlignment: VerticalAlignment.Middle);
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

  internal override double GetBarWidth() => this.BarWidth;

  protected internal override IList<CategorizedItem> GetItems()
  {
    return (IList<CategorizedItem>) this.Items.Cast<CategorizedItem>().ToList<CategorizedItem>();
  }

  protected internal override bool IsUsing(Axis axis) => this.XAxis == axis || this.YAxis == axis;

  protected internal override void SetDefaultValues()
  {
    if (!this.FillColor.IsAutomatic())
      return;
    this.defaultFillColor = this.PlotModel.GetDefaultColor();
  }

  protected internal override void UpdateAxisMaxMin()
  {
    this.XAxis.Include(this.MinX);
    this.XAxis.Include(this.MaxX);
  }

  protected internal override void UpdateData()
  {
    if (this.ItemsSource == null)
      return;
    this.Items.Clear();
    OxyPlot.ListBuilder<IntervalBarItem> listBuilder = new OxyPlot.ListBuilder<IntervalBarItem>();
    listBuilder.Add<double>(this.MinimumField, double.NaN);
    listBuilder.Add<double>(this.MaximumField, double.NaN);
    listBuilder.FillT(this.Items, this.ItemsSource, (Func<IList<object>, IntervalBarItem>) (args => new IntervalBarItem()
    {
      Start = Convert.ToDouble(args[0]),
      End = Convert.ToDouble(args[1])
    }));
  }

  protected internal override void UpdateMaxMin()
  {
    base.UpdateMaxMin();
    if (this.ValidItems == null || this.ValidItems.Count == 0)
      return;
    double val1_1 = double.MaxValue;
    double val1_2 = double.MinValue;
    foreach (IntervalBarItem validItem in (IEnumerable<IntervalBarItem>) this.ValidItems)
    {
      val1_1 = Math.Min(val1_1, validItem.Start);
      val1_1 = Math.Min(val1_1, validItem.End);
      val1_2 = Math.Max(val1_2, validItem.Start);
      val1_2 = Math.Max(val1_2, validItem.End);
    }
    this.MinX = val1_1;
    this.MaxX = val1_2;
  }

  protected internal override void UpdateValidData()
  {
    this.ValidItems = (IList<IntervalBarItem>) new List<IntervalBarItem>();
    this.ValidItemsIndexInversion = new Dictionary<int, int>();
    Axis valueAxis = this.GetValueAxis();
    for (int index = 0; index < this.Items.Count; ++index)
    {
      IntervalBarItem intervalBarItem = this.Items[index];
      if (valueAxis.IsValidValue(intervalBarItem.Start) && valueAxis.IsValidValue(intervalBarItem.End))
      {
        this.ValidItemsIndexInversion.Add(this.ValidItems.Count, index);
        this.ValidItems.Add(intervalBarItem);
      }
    }
  }

  protected override double GetActualBarWidth()
  {
    CategoryAxis categoryAxis = this.GetCategoryAxis();
    return this.BarWidth / (1.0 + categoryAxis.GapWidth) / categoryAxis.GetMaxWidth();
  }

  protected override CategoryAxis GetCategoryAxis()
  {
    return this.YAxis is CategoryAxis yaxis ? yaxis : throw new InvalidOperationException("No category axis defined.");
  }

  protected override object GetItem(int i)
  {
    return this.ItemsSource != null || this.Items == null || this.Items.Count == 0 ? base.GetItem(i) : (object) this.Items[i];
  }

  private Axis GetValueAxis() => this.XAxis;
}
