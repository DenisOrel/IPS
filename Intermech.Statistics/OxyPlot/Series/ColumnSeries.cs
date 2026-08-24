// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.ColumnSeries
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Axes;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Series;

public class ColumnSeries : BarSeriesBase<ColumnItem>
{
  public ColumnSeries() => this.ColumnWidth = 1.0;

  public double ColumnWidth { get; set; }

  internal override double GetBarWidth() => this.ColumnWidth;

  protected override double GetActualBarWidth()
  {
    CategoryAxis categoryAxis = this.GetCategoryAxis();
    return this.ColumnWidth / (1.0 + categoryAxis.GapWidth) / categoryAxis.GetMaxWidth();
  }

  protected override CategoryAxis GetCategoryAxis()
  {
    return this.XAxis is CategoryAxis ? this.XAxis as CategoryAxis : throw new Exception("A ColumnSeries requires a CategoryAxis on the x-axis. Use a BarSeries if you want horizontal bars.");
  }

  protected override OxyRect GetRectangle(
    double baseValue,
    double topValue,
    double beginValue,
    double endValue)
  {
    return new OxyRect(this.Transform(beginValue, baseValue), this.Transform(endValue, topValue));
  }

  protected override Axis GetValueAxis() => this.YAxis;

  protected override void RenderLabel(
    IRenderContext rc,
    OxyRect clippingRect,
    OxyRect rect,
    double value,
    int i)
  {
    string text = StringHelper.Format((IFormatProvider) this.ActualCulture, this.LabelFormatString, this.GetItem(this.ValidItemsIndexInversion[i]), (object) value);
    ScreenPoint p;
    VerticalAlignment verticalAlignment;
    switch (this.LabelPlacement)
    {
      case LabelPlacement.Inside:
        p = new ScreenPoint((rect.Left + rect.Right) / 2.0, rect.Top + this.LabelMargin);
        verticalAlignment = VerticalAlignment.Top;
        break;
      case LabelPlacement.Middle:
        p = new ScreenPoint((rect.Left + rect.Right) / 2.0, (rect.Bottom + rect.Top) / 2.0);
        verticalAlignment = VerticalAlignment.Middle;
        break;
      case LabelPlacement.Base:
        p = new ScreenPoint((rect.Left + rect.Right) / 2.0, rect.Bottom - this.LabelMargin);
        verticalAlignment = VerticalAlignment.Bottom;
        break;
      default:
        p = new ScreenPoint((rect.Left + rect.Right) / 2.0, rect.Top - this.LabelMargin);
        verticalAlignment = VerticalAlignment.Bottom;
        break;
    }
    rc.DrawClippedText(clippingRect, p, text, this.ActualTextColor, this.ActualFont, this.ActualFontSize, this.ActualFontWeight, horizontalAlignment: HorizontalAlignment.Center, verticalAlignment: verticalAlignment);
  }

  protected override void UpdateFromDataFields()
  {
    OxyPlot.ListBuilder<ColumnItem> listBuilder = new OxyPlot.ListBuilder<ColumnItem>();
    listBuilder.Add<double>(this.ValueField, double.NaN);
    listBuilder.Add<OxyColor>(this.ColorField, OxyColors.Automatic);
    listBuilder.Fill((IList) this.ItemsSourceItems, this.ItemsSource, (Func<IList<object>, object>) (args =>
    {
      return (object) new ColumnItem(Convert.ToDouble(args[0]))
      {
        Color = (OxyColor) args[1]
      };
    }));
  }
}
