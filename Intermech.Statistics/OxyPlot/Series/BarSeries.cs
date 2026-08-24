// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.BarSeries
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Axes;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Series;

public class BarSeries : BarSeriesBase<BarItem>
{
  public BarSeries() => this.BarWidth = 1.0;

  public double BarWidth { get; set; }

  internal override double GetBarWidth() => this.BarWidth;

  protected override double GetActualBarWidth()
  {
    CategoryAxis categoryAxis = this.GetCategoryAxis();
    return this.BarWidth / (1.0 + categoryAxis.GapWidth) / categoryAxis.GetMaxWidth();
  }

  protected override CategoryAxis GetCategoryAxis()
  {
    return this.YAxis is CategoryAxis ? this.YAxis as CategoryAxis : throw new Exception("A BarSeries requires a CategoryAxis on the y-axis. Use a ColumnSeries if you want vertical bars.");
  }

  protected override OxyRect GetRectangle(
    double baseValue,
    double topValue,
    double beginValue,
    double endValue)
  {
    return new OxyRect(this.Transform(baseValue, beginValue), this.Transform(topValue, endValue));
  }

  protected override Axis GetValueAxis() => this.XAxis;

  protected override void RenderLabel(
    IRenderContext rc,
    OxyRect clippingRect,
    OxyRect rect,
    double value,
    int index)
  {
    string text = StringHelper.Format((IFormatProvider) this.ActualCulture, this.LabelFormatString, this.GetItem(this.ValidItemsIndexInversion[index]), (object) value);
    ScreenPoint p;
    HorizontalAlignment horizontalAlignment;
    switch (this.LabelPlacement)
    {
      case LabelPlacement.Inside:
        p = new ScreenPoint(rect.Right - this.LabelMargin, (rect.Top + rect.Bottom) / 2.0);
        horizontalAlignment = HorizontalAlignment.Right;
        break;
      case LabelPlacement.Middle:
        p = new ScreenPoint((rect.Left + rect.Right) / 2.0, (rect.Top + rect.Bottom) / 2.0);
        horizontalAlignment = HorizontalAlignment.Center;
        break;
      case LabelPlacement.Base:
        p = new ScreenPoint(rect.Left + this.LabelMargin, (rect.Top + rect.Bottom) / 2.0);
        horizontalAlignment = HorizontalAlignment.Left;
        break;
      default:
        p = new ScreenPoint(rect.Right + this.LabelMargin, (rect.Top + rect.Bottom) / 2.0);
        horizontalAlignment = HorizontalAlignment.Left;
        break;
    }
    rc.DrawClippedText(clippingRect, p, text, this.ActualTextColor, this.ActualFont, this.ActualFontSize, this.ActualFontWeight, horizontalAlignment: horizontalAlignment, verticalAlignment: VerticalAlignment.Middle);
  }

  protected override void UpdateFromDataFields()
  {
    OxyPlot.ListBuilder<BarItem> listBuilder = new OxyPlot.ListBuilder<BarItem>();
    listBuilder.Add<double>(this.ValueField, double.NaN);
    listBuilder.Add<OxyColor>(this.ColorField, OxyColors.Automatic);
    listBuilder.Fill((IList) this.ItemsSourceItems, this.ItemsSource, (Func<IList<object>, object>) (args =>
    {
      return (object) new BarItem(Convert.ToDouble(args[0]))
      {
        Color = (OxyColor) args[1]
      };
    }));
  }
}
