// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.TornadoBarSeries
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OxyPlot.Series;

public class TornadoBarSeries : CategorizedSeries
{
  public new const string DefaultTrackerFormatString = "{0}\n{1}: {2}\n{3}: {4}";
  private OxyColor defaultMaximumFillColor;
  private OxyColor defaultMinimumFillColor;

  public TornadoBarSeries()
  {
    this.Items = (IList<TornadoBarItem>) new List<TornadoBarItem>();
    this.MaximumFillColor = OxyColor.FromRgb((byte) 216, (byte) 82, (byte) 85);
    this.MinimumFillColor = OxyColor.FromRgb((byte) 84, (byte) 138, (byte) 209);
    this.LabelColor = OxyColors.Automatic;
    this.StrokeColor = OxyColors.Black;
    this.StrokeThickness = 1.0;
    this.BarWidth = 1.0;
    this.TrackerFormatString = "{0}\n{1}: {2}\n{3}: {4}";
    this.LabelMargin = 4.0;
    this.MinimumLabelFormatString = "{0}";
    this.MaximumLabelFormatString = "{0}";
  }

  public double BarWidth { get; set; }

  public double BaseValue { get; set; }

  public IList<TornadoBarItem> Items { get; private set; }

  public OxyColor LabelColor { get; set; }

  public string LabelField { get; set; }

  public double LabelMargin { get; set; }

  public string MaximumField { get; set; }

  public OxyColor MaximumFillColor { get; set; }

  public OxyColor ActualMaximumFillColor
  {
    get => this.MaximumFillColor.GetActualColor(this.defaultMaximumFillColor);
  }

  public string MaximumLabelFormatString { get; set; }

  public string MinimumField { get; set; }

  public OxyColor MinimumFillColor { get; set; }

  public OxyColor ActualMinimumFillColor
  {
    get => this.MinimumFillColor.GetActualColor(this.defaultMinimumFillColor);
  }

  public string MinimumLabelFormatString { get; set; }

  public OxyColor StrokeColor { get; set; }

  public double StrokeThickness { get; set; }

  protected internal IList<OxyRect> ActualMaximumBarRectangles { get; set; }

  protected internal IList<OxyRect> ActualMinimumBarRectangles { get; set; }

  protected internal IList<TornadoBarItem> ValidItems { get; set; }

  protected internal Dictionary<int, int> ValidItemsIndexInversion { get; set; }

  public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
  {
    for (int index = 0; index < this.ActualMinimumBarRectangles.Count; ++index)
    {
      int num1 = this.ActualMinimumBarRectangles[index].Contains(point) ? 1 : 0;
      bool flag = this.ActualMaximumBarRectangles[index].Contains(point);
      int num2 = flag ? 1 : 0;
      if ((num1 | num2) != 0)
      {
        TornadoBarItem tornadoBarItem = (TornadoBarItem) this.GetItem(this.ValidItemsIndexInversion[index]);
        int categoryIndex = tornadoBarItem.GetCategoryIndex(index);
        double num3 = flag ? this.ValidItems[index].Maximum : this.ValidItems[index].Minimum;
        DataPoint dataPoint = new DataPoint((double) categoryIndex, num3);
        CategoryAxis categoryAxis = this.GetCategoryAxis();
        Axis valueAxis = this.GetValueAxis();
        return new TrackerHitResult()
        {
          Series = (OxyPlot.Series.Series) this,
          DataPoint = dataPoint,
          Position = point,
          Item = (object) tornadoBarItem,
          Index = (double) index,
          Text = StringHelper.Format((IFormatProvider) this.ActualCulture, this.TrackerFormatString, (object) tornadoBarItem, (object) this.Title, (object) (categoryAxis.Title ?? "Category"), (object) categoryAxis.FormatValue((double) categoryIndex), (object) (valueAxis.Title ?? "Value"), valueAxis.GetValue(num3))
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
    this.ActualMinimumBarRectangles = (IList<OxyRect>) new List<OxyRect>();
    this.ActualMaximumBarRectangles = (IList<OxyRect>) new List<OxyRect>();
    if (this.ValidItems.Count == 0)
      return;
    OxyRect clippingRect = this.GetClippingRect();
    CategoryAxis categoryAxis = this.GetCategoryAxis();
    double actualBarWidth = this.GetActualBarWidth();
    for (int index = 0; index < this.ValidItems.Count; ++index)
    {
      TornadoBarItem validItem = this.ValidItems[index];
      int categoryIndex = validItem.GetCategoryIndex(index);
      double x = double.IsNaN(validItem.BaseValue) ? this.BaseValue : validItem.BaseValue;
      double currentBarOffset = categoryAxis.GetCurrentBarOffset(categoryIndex);
      ScreenPoint screenPoint1 = this.Transform(validItem.Minimum, (double) categoryIndex - 0.5 + currentBarOffset);
      ScreenPoint screenPoint2 = this.Transform(validItem.Maximum, (double) categoryIndex - 0.5 + currentBarOffset + actualBarWidth);
      ScreenPoint screenPoint3 = this.Transform(x, (double) categoryIndex - 0.5 + currentBarOffset);
      screenPoint3 = new ScreenPoint((double) (int) screenPoint3.X, screenPoint3.Y);
      OxyRect rect1 = OxyRect.Create(screenPoint1.X, screenPoint1.Y, screenPoint3.X, screenPoint2.Y);
      OxyRect rect2 = OxyRect.Create(screenPoint3.X, screenPoint1.Y, screenPoint2.X, screenPoint2.Y);
      this.ActualMinimumBarRectangles.Add(rect1);
      this.ActualMaximumBarRectangles.Add(rect2);
      rc.DrawClippedRectangleAsPolygon(clippingRect, rect1, validItem.MinimumColor.GetActualColor(this.ActualMinimumFillColor), this.StrokeColor, this.StrokeThickness);
      rc.DrawClippedRectangleAsPolygon(clippingRect, rect2, validItem.MaximumColor.GetActualColor(this.ActualMaximumFillColor), this.StrokeColor, this.StrokeThickness);
      if (this.MinimumLabelFormatString != null)
      {
        string text = StringHelper.Format((IFormatProvider) this.ActualCulture, this.MinimumLabelFormatString, this.GetItem(this.ValidItemsIndexInversion[index]), (object) validItem.Minimum);
        ScreenPoint p = new ScreenPoint(rect1.Left - this.LabelMargin, (rect1.Top + rect1.Bottom) / 2.0);
        rc.DrawClippedText(clippingRect, p, text, this.ActualTextColor, this.ActualFont, this.ActualFontSize, this.ActualFontWeight, horizontalAlignment: HorizontalAlignment.Right, verticalAlignment: VerticalAlignment.Middle);
      }
      if (this.MaximumLabelFormatString != null)
      {
        string text = StringHelper.Format((IFormatProvider) this.ActualCulture, this.MaximumLabelFormatString, this.GetItem(this.ValidItemsIndexInversion[index]), (object) validItem.Maximum);
        ScreenPoint p = new ScreenPoint(rect2.Right + this.LabelMargin, (rect2.Top + rect2.Bottom) / 2.0);
        rc.DrawClippedText(clippingRect, p, text, this.ActualTextColor, this.ActualFont, this.ActualFontSize, this.ActualFontWeight, verticalAlignment: VerticalAlignment.Middle);
      }
    }
  }

  public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
  {
    double left = (legendBox.Left + legendBox.Right) / 2.0;
    double num1 = (legendBox.Top + legendBox.Bottom) / 2.0;
    double height = (legendBox.Bottom - legendBox.Top) * 0.8;
    double num2 = height;
    rc.DrawRectangleAsPolygon(new OxyRect(left - 0.5 * num2, num1 - 0.5 * height, 0.5 * num2, height), this.ActualMinimumFillColor, this.StrokeColor, this.StrokeThickness);
    rc.DrawRectangleAsPolygon(new OxyRect(left, num1 - 0.5 * height, 0.5 * num2, height), this.ActualMaximumFillColor, this.StrokeColor, this.StrokeThickness);
  }

  internal override double GetBarWidth() => this.BarWidth;

  protected internal override IList<CategorizedItem> GetItems()
  {
    return (IList<CategorizedItem>) this.Items.Cast<CategorizedItem>().ToList<CategorizedItem>();
  }

  protected internal override bool IsUsing(Axis axis) => this.XAxis == axis || this.YAxis == axis;

  protected internal override void SetDefaultValues()
  {
    if (this.MaximumFillColor.IsAutomatic())
      this.defaultMaximumFillColor = this.PlotModel.GetDefaultColor();
    if (!this.MinimumFillColor.IsAutomatic())
      return;
    this.defaultMinimumFillColor = this.PlotModel.GetDefaultColor();
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
    OxyPlot.ListBuilder<TornadoBarItem> listBuilder = new OxyPlot.ListBuilder<TornadoBarItem>();
    listBuilder.Add<double>(this.MinimumField, double.NaN);
    listBuilder.Add<double>(this.MaximumField, double.NaN);
    listBuilder.FillT(this.Items, this.ItemsSource, (Func<IList<object>, TornadoBarItem>) (args => new TornadoBarItem()
    {
      Minimum = Convert.ToDouble(args[0]),
      Maximum = Convert.ToDouble(args[1])
    }));
  }

  protected internal override void UpdateMaxMin()
  {
    base.UpdateMaxMin();
    if (this.ValidItems == null || this.ValidItems.Count == 0)
      return;
    double val1_1 = double.MaxValue;
    double val1_2 = double.MinValue;
    foreach (TornadoBarItem validItem in (IEnumerable<TornadoBarItem>) this.ValidItems)
    {
      val1_1 = Math.Min(val1_1, validItem.Minimum);
      val1_2 = Math.Max(val1_2, validItem.Maximum);
    }
    this.MinX = val1_1;
    this.MaxX = val1_2;
  }

  protected internal override void UpdateValidData()
  {
    this.ValidItems = (IList<TornadoBarItem>) new List<TornadoBarItem>();
    this.ValidItemsIndexInversion = new Dictionary<int, int>();
    Axis valueAxis = this.GetValueAxis();
    for (int index = 0; index < this.Items.Count; ++index)
    {
      TornadoBarItem tornadoBarItem = this.Items[index];
      if (valueAxis.IsValidValue(tornadoBarItem.Minimum) && valueAxis.IsValidValue(tornadoBarItem.Maximum))
      {
        this.ValidItemsIndexInversion.Add(this.ValidItems.Count, index);
        this.ValidItems.Add(tornadoBarItem);
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
