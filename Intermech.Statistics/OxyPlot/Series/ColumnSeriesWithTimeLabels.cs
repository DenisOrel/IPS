// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.ColumnSeriesWithTimeLabels
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Axes;
using System;

#nullable disable
namespace OxyPlot.Series;

public class ColumnSeriesWithTimeLabels : ColumnSeries
{
  protected override void RenderLabel(
    IRenderContext rc,
    OxyRect clippingRect,
    OxyRect rect,
    double value,
    int i)
  {
    TimeSpan timeSpan = TimeSpanAxis.ToTimeSpan(value);
    string text = StringHelper.Format((IFormatProvider) this.ActualCulture, this.LabelFormatString, this.GetItem(this.ValidItemsIndexInversion[i]), (object) timeSpan);
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
}
