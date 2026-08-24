// Decompiled with JetBrains decompiler
// Type: OxyPlot.PlotElement
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

#nullable disable
namespace OxyPlot;

public abstract class PlotElement : UIElement, IPlotElement
{
  protected PlotElement()
  {
    this.Font = (string) null;
    this.FontSize = double.NaN;
    this.FontWeight = 400.0;
    this.TextColor = OxyColors.Automatic;
  }

  public string Font { get; set; }

  public double FontSize { get; set; }

  public double FontWeight { get; set; }

  public PlotModel PlotModel => (PlotModel) this.Parent;

  public object Tag { get; set; }

  public OxyColor TextColor { get; set; }

  public string ToolTip { get; set; }

  protected internal string ActualFont => this.Font ?? this.PlotModel.DefaultFont;

  protected internal double ActualFontSize
  {
    get => double.IsNaN(this.FontSize) ? this.PlotModel.DefaultFontSize : this.FontSize;
  }

  protected internal double ActualFontWeight => this.FontWeight;

  protected internal OxyColor ActualTextColor
  {
    get => this.TextColor.GetActualColor(this.PlotModel.TextColor);
  }

  protected CultureInfo ActualCulture
  {
    get => this.PlotModel == null ? CultureInfo.CurrentCulture : this.PlotModel.ActualCulture;
  }

  public virtual int GetElementHashCode()
  {
    return HashCodeBuilder.GetHashCode(((IEnumerable<PropertyInfo>) this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)).Select<PropertyInfo, object>((Func<PropertyInfo, object>) (pi => pi.GetValue((object) this, (object[]) null))));
  }
}
