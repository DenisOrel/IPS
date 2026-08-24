// Decompiled with JetBrains decompiler
// Type: OxyPlot.Axes.Axis
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Axes;

public abstract class Axis : PlotElement
{
  protected static readonly Func<double, double> Exponent = (Func<double, double>) (x => Math.Floor(Math.Log(Math.Abs(x), 10.0)));
  protected static readonly Func<double, double> Mantissa = (Func<double, double>) (x => x / Math.Pow(10.0, Axis.Exponent(x)));
  private double offset;
  private double scale;
  private AxisPosition position;

  protected Axis()
  {
    this.Position = AxisPosition.Left;
    this.PositionTier = 0;
    this.IsAxisVisible = true;
    this.Layer = AxisLayer.BelowSeries;
    this.ViewMaximum = double.NaN;
    this.ViewMinimum = double.NaN;
    this.AbsoluteMaximum = double.MaxValue;
    this.AbsoluteMinimum = double.MinValue;
    this.Minimum = double.NaN;
    this.Maximum = double.NaN;
    this.MinorStep = double.NaN;
    this.MajorStep = double.NaN;
    this.MinimumPadding = 0.01;
    this.MaximumPadding = 0.01;
    this.MinimumRange = 0.0;
    this.MaximumRange = double.PositiveInfinity;
    this.TickStyle = TickStyle.Outside;
    this.TicklineColor = OxyColors.Black;
    this.AxislineStyle = LineStyle.None;
    this.AxislineColor = OxyColors.Black;
    this.AxislineThickness = 1.0;
    this.MajorGridlineStyle = LineStyle.None;
    this.MajorGridlineColor = OxyColor.FromArgb((byte) 64 /*0x40*/, (byte) 0, (byte) 0, (byte) 0);
    this.MajorGridlineThickness = 1.0;
    this.MinorGridlineStyle = LineStyle.None;
    this.MinorGridlineColor = OxyColor.FromArgb((byte) 32 /*0x20*/, (byte) 0, (byte) 0, (byte) 0);
    this.MinorGridlineThickness = 1.0;
    this.ExtraGridlineStyle = LineStyle.Solid;
    this.ExtraGridlineColor = OxyColors.Black;
    this.ExtraGridlineThickness = 1.0;
    this.MinorTickSize = 4.0;
    this.MajorTickSize = 7.0;
    this.StartPosition = 0.0;
    this.EndPosition = 1.0;
    this.TitlePosition = 0.5;
    this.TitleFormatString = "{0} [{1}]";
    this.TitleClippingLength = 0.9;
    this.TitleColor = OxyColors.Automatic;
    this.TitleFontSize = double.NaN;
    this.TitleFontWeight = 400.0;
    this.ClipTitle = true;
    this.Angle = 0.0;
    this.IsZoomEnabled = true;
    this.IsPanEnabled = true;
    this.FilterMinValue = double.MinValue;
    this.FilterMaxValue = double.MaxValue;
    this.FilterFunction = (Func<double, bool>) null;
    this.IntervalLength = 60.0;
    this.AxisDistance = 0.0;
    this.AxisTitleDistance = 4.0;
    this.AxisTickToLabelDistance = 4.0;
  }

  public event EventHandler<AxisChangedEventArgs> AxisChanged;

  public event EventHandler TransformChanged;

  public double AbsoluteMaximum { get; set; }

  public double AbsoluteMinimum { get; set; }

  public double ActualMajorStep { get; protected set; }

  public double ActualMaximum { get; protected set; }

  public double ActualMinimum { get; protected set; }

  public double ActualMinorStep { get; protected set; }

  public string ActualStringFormat { get; protected set; }

  public string ActualTitle
  {
    get
    {
      return this.Unit != null ? string.Format(this.TitleFormatString, (object) this.Title, (object) this.Unit) : this.Title;
    }
  }

  public double Angle { get; set; }

  public double AxisTickToLabelDistance { get; set; }

  public double AxisTitleDistance { get; set; }

  public double AxisDistance { get; set; }

  public OxyColor AxislineColor { get; set; }

  public LineStyle AxislineStyle { get; set; }

  public double AxislineThickness { get; set; }

  public bool ClipTitle { get; set; }

  public double DataMaximum { get; protected set; }

  public double DataMinimum { get; protected set; }

  public double EndPosition { get; set; }

  public OxyColor ExtraGridlineColor { get; set; }

  public LineStyle ExtraGridlineStyle { get; set; }

  public double ExtraGridlineThickness { get; set; }

  public double[] ExtraGridlines { get; set; }

  public Func<double, bool> FilterFunction { get; set; }

  public double FilterMaxValue { get; set; }

  public double FilterMinValue { get; set; }

  public double IntervalLength { get; set; }

  public bool IsAxisVisible { get; set; }

  public bool IsPanEnabled { get; set; }

  public bool IsReversed => this.StartPosition > this.EndPosition;

  public bool IsZoomEnabled { get; set; }

  public string Key { get; set; }

  public Func<double, string> LabelFormatter { get; set; }

  public AxisLayer Layer { get; set; }

  public OxyColor MajorGridlineColor { get; set; }

  public LineStyle MajorGridlineStyle { get; set; }

  public double MajorGridlineThickness { get; set; }

  public double MajorStep { get; set; }

  public double MajorTickSize { get; set; }

  public double Maximum { get; set; }

  public double MaximumPadding { get; set; }

  public double MaximumRange { get; set; }

  public double Minimum { get; set; }

  public double MinimumPadding { get; set; }

  public double MinimumRange { get; set; }

  public OxyColor MinorGridlineColor { get; set; }

  public LineStyle MinorGridlineStyle { get; set; }

  public double MinorGridlineThickness { get; set; }

  public double MinorStep { get; set; }

  public double MinorTickSize { get; set; }

  public double Offset => this.offset;

  public AxisPosition Position
  {
    get => this.position;
    set => this.position = value;
  }

  public bool PositionAtZeroCrossing { get; set; }

  public int PositionTier { get; set; }

  public double Scale => this.scale;

  public ScreenPoint ScreenMax { get; protected set; }

  public ScreenPoint ScreenMin { get; protected set; }

  public double StartPosition { get; set; }

  public string StringFormat { get; set; }

  public TickStyle TickStyle { get; set; }

  public OxyColor TicklineColor { get; set; }

  public string Title { get; set; }

  public double TitleClippingLength { get; set; }

  public OxyColor TitleColor { get; set; }

  public string TitleFont { get; set; }

  public double TitleFontSize { get; set; }

  public double TitleFontWeight { get; set; }

  public string TitleFormatString { get; set; }

  public double TitlePosition { get; set; }

  public string Unit { get; set; }

  public bool UseSuperExponentialFormat { get; set; }

  public OxySize DesiredSize { get; protected set; }

  internal double PositionTierMaxShift { get; set; }

  internal double PositionTierMinShift { get; set; }

  internal double PositionTierSize { get; set; }

  protected internal OxyColor ActualTitleColor
  {
    get => this.TitleColor.GetActualColor(this.PlotModel.TextColor);
  }

  protected internal string ActualTitleFont => this.TitleFont ?? this.PlotModel.DefaultFont;

  protected internal double ActualTitleFontSize
  {
    get => double.IsNaN(this.TitleFontSize) ? this.ActualFontSize : this.TitleFontSize;
  }

  protected internal double ActualTitleFontWeight
  {
    get => double.IsNaN(this.TitleFontWeight) ? this.ActualFontWeight : this.TitleFontWeight;
  }

  protected double ViewMaximum { get; set; }

  protected double ViewMinimum { get; set; }

  public static IList<double> CreateTickValues(double from, double to, double step, int maxTicks = 1000)
  {
    if (step <= 0.0)
      throw new ArgumentException("Step cannot be zero or negative.", nameof (step));
    if (to <= from && step > 0.0)
      step *= -1.0;
    double num1 = Math.Round(from / step) * step;
    int capacity = Math.Max((int) ((to - from) / step), 1);
    double num2 = step * 0.001 * (double) Math.Sign(step);
    List<double> tickValues = new List<double>(capacity);
    for (int index = 0; index < maxTicks; ++index)
    {
      double num3 = num1 + step * (double) index;
      if (num3 <= to + num2)
      {
        double num4 = Math.Round(num3 / step, 14) * step;
        tickValues.Add(num4);
      }
      else
        break;
    }
    return (IList<double>) tickValues;
  }

  public static double ToDouble(object value)
  {
    switch (value)
    {
      case DateTime dateTime:
        return DateTimeAxis.ToDouble(dateTime);
      case TimeSpan s:
        return TimeSpanAxis.ToDouble(s);
      default:
        return Convert.ToDouble(value);
    }
  }

  public static DataPoint InverseTransform(ScreenPoint p, Axis xaxis, Axis yaxis)
  {
    return xaxis.InverseTransform(p.x, p.y, yaxis);
  }

  public virtual void CoerceActualMaxMin()
  {
    if (double.IsNaN(this.ActualMinimum) || double.IsInfinity(this.ActualMinimum))
      this.ActualMinimum = 0.0;
    if (double.IsNaN(this.ActualMaximum) || double.IsInfinity(this.ActualMaximum))
      this.ActualMaximum = 100.0;
    if (this.AbsoluteMaximum - this.AbsoluteMinimum < this.MinimumRange)
      throw new InvalidOperationException("MinimumRange should be larger than AbsoluteMaximum-AbsoluteMinimum.");
    if (this.ActualMaximum - this.ActualMinimum < this.MinimumRange)
    {
      if (this.ActualMinimum + this.MinimumRange < this.AbsoluteMaximum)
      {
        if (this.ActualMinimum < this.AbsoluteMinimum)
          this.ActualMinimum = this.AbsoluteMinimum;
        this.ActualMaximum = this.ActualMinimum + this.MinimumRange;
      }
      else if (this.AbsoluteMaximum - this.MinimumRange > this.AbsoluteMinimum)
      {
        this.ActualMinimum = this.AbsoluteMaximum - this.MinimumRange;
        this.ActualMaximum = this.AbsoluteMaximum;
      }
      else
      {
        this.ActualMaximum = this.AbsoluteMaximum;
        this.ActualMinimum = this.AbsoluteMinimum;
      }
    }
    if (this.ActualMaximum - this.ActualMinimum > this.MaximumRange)
    {
      if (this.ActualMinimum + this.MaximumRange < this.AbsoluteMaximum)
      {
        if (this.ActualMinimum < this.AbsoluteMinimum)
          this.ActualMinimum = this.AbsoluteMinimum;
        this.ActualMaximum = this.ActualMinimum + this.MaximumRange;
      }
      else if (this.AbsoluteMaximum - this.MaximumRange > this.AbsoluteMinimum)
      {
        this.ActualMinimum = this.AbsoluteMaximum - this.MaximumRange;
        this.ActualMaximum = this.AbsoluteMaximum;
      }
      else
      {
        this.ActualMaximum = this.AbsoluteMaximum;
        this.ActualMinimum = this.AbsoluteMinimum;
      }
    }
    if (this.AbsoluteMaximum <= this.AbsoluteMinimum)
      throw new InvalidOperationException("AbsoluteMaximum should be larger than AbsoluteMinimum.");
    if (this.ActualMaximum <= this.ActualMinimum)
      this.ActualMaximum = this.ActualMinimum + 100.0;
    if (this.ActualMinimum < this.AbsoluteMinimum)
      this.ActualMinimum = this.AbsoluteMinimum;
    if (this.ActualMinimum > this.AbsoluteMaximum)
      this.ActualMinimum = this.AbsoluteMaximum;
    if (this.ActualMaximum < this.AbsoluteMinimum)
      this.ActualMaximum = this.AbsoluteMinimum;
    if (this.ActualMaximum <= this.AbsoluteMaximum)
      return;
    this.ActualMaximum = this.AbsoluteMaximum;
  }

  public string FormatValue(double x)
  {
    return this.LabelFormatter != null ? this.LabelFormatter(x) : this.FormatValueOverride(x);
  }

  public virtual void GetTickValues(
    out IList<double> majorLabelValues,
    out IList<double> majorTickValues,
    out IList<double> minorTickValues)
  {
    minorTickValues = Axis.CreateTickValues(this.ActualMinimum, this.ActualMaximum, this.ActualMinorStep);
    majorTickValues = Axis.CreateTickValues(this.ActualMinimum, this.ActualMaximum, this.ActualMajorStep);
    majorLabelValues = majorTickValues;
  }

  public virtual object GetValue(double x) => (object) x;

  public virtual DataPoint InverseTransform(double x, double y, Axis yaxis)
  {
    return new DataPoint(this.InverseTransform(x), yaxis != null ? yaxis.InverseTransform(y) : 0.0);
  }

  public virtual double InverseTransform(double sx) => sx / this.scale + this.offset;

  public bool IsHorizontal()
  {
    return this.position == AxisPosition.Top || this.position == AxisPosition.Bottom;
  }

  public bool IsValidValue(double value)
  {
    if (value != value || value == double.PositiveInfinity || value == double.NegativeInfinity || value >= this.FilterMaxValue || value <= this.FilterMinValue)
      return false;
    return this.FilterFunction == null || this.FilterFunction(value);
  }

  public bool IsVertical()
  {
    return this.position == AxisPosition.Left || this.position == AxisPosition.Right;
  }

  public abstract bool IsXyAxis();

  public virtual OxySize Measure(IRenderContext rc)
  {
    IList<double> majorLabelValues;
    this.GetTickValues(out majorLabelValues, out IList<double> _, out IList<double> _);
    OxySize oxySize1 = new OxySize();
    foreach (double x in (IEnumerable<double>) majorLabelValues)
    {
      string text = this.FormatValue(x);
      OxySize oxySize2 = rc.MeasureText(text, this.ActualFont, this.ActualFontSize, this.ActualFontWeight, this.Angle);
      if (oxySize2.Width > oxySize1.Width)
        oxySize1 = new OxySize(oxySize2.Width, oxySize1.Height);
      if (oxySize2.Height > oxySize1.Height)
        oxySize1 = new OxySize(oxySize1.Width, oxySize2.Height);
    }
    OxySize oxySize3 = rc.MeasureText(this.ActualTitle, this.ActualFont, this.ActualFontSize, this.ActualFontWeight);
    double width = 0.0;
    double height = 0.0;
    if (this.IsVertical())
    {
      switch (this.TickStyle)
      {
        case TickStyle.Crossing:
          width += this.MajorTickSize * 0.75;
          break;
        case TickStyle.Outside:
          width += this.MajorTickSize;
          break;
      }
      width = width + this.AxisDistance + this.AxisTickToLabelDistance + oxySize1.Width;
      if (oxySize3.Height > 0.0)
        width = width + this.AxisTitleDistance + oxySize3.Height;
    }
    else
    {
      switch (this.TickStyle)
      {
        case TickStyle.Crossing:
          height += this.MajorTickSize * 0.75;
          break;
        case TickStyle.Outside:
          height += this.MajorTickSize;
          break;
      }
      height = height + this.AxisDistance + this.AxisTickToLabelDistance + oxySize1.Height;
      if (oxySize3.Height > 0.0)
        height = height + this.AxisTitleDistance + oxySize3.Height;
    }
    OxySize oxySize4 = new OxySize(width, height);
    this.DesiredSize = oxySize4;
    return oxySize4;
  }

  public virtual void Pan(ScreenPoint ppt, ScreenPoint cpt)
  {
    if (!this.IsPanEnabled)
      return;
    this.Pan(this.IsHorizontal() ? cpt.X - ppt.X : cpt.Y - ppt.Y);
  }

  public virtual void Pan(double delta)
  {
    if (!this.IsPanEnabled)
      return;
    double actualMinimum = this.ActualMinimum;
    double actualMaximum = this.ActualMaximum;
    double num1 = delta / this.Scale;
    double num2 = this.ActualMinimum - num1;
    double num3 = this.ActualMaximum - num1;
    if (num2 < this.AbsoluteMinimum)
    {
      num2 = this.AbsoluteMinimum;
      num3 = Math.Min(num2 + this.ActualMaximum - this.ActualMinimum, this.AbsoluteMaximum);
    }
    if (num3 > this.AbsoluteMaximum)
    {
      num3 = this.AbsoluteMaximum;
      num2 = Math.Max(num3 - (this.ActualMaximum - this.ActualMinimum), this.AbsoluteMinimum);
    }
    this.ViewMinimum = num2;
    this.ViewMaximum = num3;
    this.UpdateActualMaxMin();
    this.OnAxisChanged(new AxisChangedEventArgs(AxisChangeTypes.Pan, this.ActualMinimum - actualMinimum, this.ActualMaximum - actualMaximum));
  }

  public virtual void Render(IRenderContext rc, int pass)
  {
    new HorizontalAndVerticalAxisRenderer(rc, this.PlotModel).Render(this, pass);
  }

  public virtual void Reset()
  {
    double actualMinimum1 = this.ActualMinimum;
    double actualMinimum2 = this.ActualMinimum;
    this.ViewMinimum = double.NaN;
    this.ViewMaximum = double.NaN;
    this.UpdateActualMaxMin();
    this.OnAxisChanged(new AxisChangedEventArgs(AxisChangeTypes.Reset, this.ActualMinimum - actualMinimum1, this.ActualMaximum - actualMinimum2));
  }

  public override string ToString()
  {
    return string.Format((IFormatProvider) this.ActualCulture, "{0}({1}, {2}, {3}, {4})", (object) this.GetType().Name, (object) this.Position, (object) this.ActualMinimum, (object) this.ActualMaximum, (object) this.ActualMajorStep);
  }

  public virtual ScreenPoint Transform(double x, double y, Axis yaxis)
  {
    if (yaxis == null)
      throw new NullReferenceException("Y axis should not be null when transforming.");
    return new ScreenPoint(this.Transform(x), yaxis.Transform(y));
  }

  public virtual double Transform(double x) => (x - this.offset) * this.scale;

  public virtual void Zoom(double newScale)
  {
    double actualMinimum = this.ActualMinimum;
    double actualMaximum = this.ActualMaximum;
    double sx1 = this.Transform(this.ActualMaximum);
    double sx2 = this.Transform(this.ActualMinimum);
    double num1 = (double) Math.Sign(this.scale);
    double num2 = (this.ActualMaximum + this.ActualMinimum) / 2.0;
    double newOffset = (this.offset - num2) * this.scale / (num1 * newScale) + num2;
    this.SetTransform(num1 * newScale, newOffset);
    double num3 = this.InverseTransform(sx1);
    double num4 = this.InverseTransform(sx2);
    if (num4 < this.AbsoluteMinimum && num3 > this.AbsoluteMaximum)
    {
      num4 = this.AbsoluteMinimum;
      num3 = this.AbsoluteMaximum;
    }
    else if (num4 < this.AbsoluteMinimum)
    {
      double num5 = num3 - num4;
      num4 = this.AbsoluteMinimum;
      num3 = this.AbsoluteMinimum + num5;
      if (num3 > this.AbsoluteMaximum)
        num3 = this.AbsoluteMaximum;
    }
    else if (num3 > this.AbsoluteMaximum)
    {
      double num6 = num3 - num4;
      num3 = this.AbsoluteMaximum;
      num4 = this.AbsoluteMaximum - num6;
      if (num4 < this.AbsoluteMinimum)
        num4 = this.AbsoluteMinimum;
    }
    this.ViewMaximum = num3;
    this.ViewMinimum = num4;
    this.UpdateActualMaxMin();
    this.OnAxisChanged(new AxisChangedEventArgs(AxisChangeTypes.Zoom, this.ActualMinimum - actualMinimum, this.ActualMaximum - actualMaximum));
  }

  public virtual void Zoom(double x0, double x1)
  {
    if (!this.IsZoomEnabled)
      return;
    double actualMinimum = this.ActualMinimum;
    double actualMaximum = this.ActualMaximum;
    double num1 = Math.Max(Math.Min(x0, x1), this.AbsoluteMinimum);
    double num2 = Math.Min(Math.Max(x0, x1), this.AbsoluteMaximum);
    this.ViewMinimum = num1;
    this.ViewMaximum = num2;
    this.UpdateActualMaxMin();
    this.OnAxisChanged(new AxisChangedEventArgs(AxisChangeTypes.Zoom, this.ActualMinimum - actualMinimum, this.ActualMaximum - actualMaximum));
  }

  public virtual void ZoomAt(double factor, double x)
  {
    if (!this.IsZoomEnabled)
      return;
    double actualMinimum = this.ActualMinimum;
    double actualMaximum = this.ActualMaximum;
    double num1 = (this.ActualMinimum - x) * this.scale;
    double num2 = (this.ActualMaximum - x) * this.scale;
    this.scale *= factor;
    double val1_1 = num1 / this.scale + x;
    double scale = this.scale;
    double val1_2 = num2 / scale + x;
    if (val1_2 - val1_1 > this.MaximumRange)
    {
      double num3 = (val1_1 + val1_2) * 0.5;
      val1_2 = num3 + this.MaximumRange * 0.5;
      val1_1 = num3 - this.MaximumRange * 0.5;
    }
    if (val1_2 - val1_1 < this.MinimumRange)
    {
      double num4 = (val1_1 + val1_2) * 0.5;
      val1_2 = num4 + this.MinimumRange * 0.5;
      val1_1 = num4 - this.MinimumRange * 0.5;
    }
    double num5 = Math.Max(val1_1, this.AbsoluteMinimum);
    double num6 = Math.Min(val1_2, this.AbsoluteMaximum);
    this.ViewMinimum = num5;
    this.ViewMaximum = num6;
    this.UpdateActualMaxMin();
    this.OnAxisChanged(new AxisChangedEventArgs(AxisChangeTypes.Zoom, this.ActualMinimum - actualMinimum, this.ActualMaximum - actualMaximum));
  }

  public virtual void ZoomAtCenter(double factor)
  {
    double x = this.InverseTransform((this.Transform(this.ActualMaximum) + this.Transform(this.ActualMinimum)) * 0.5);
    this.ZoomAt(factor, x);
  }

  public virtual void Include(double value)
  {
    if (!this.IsValidValue(value))
      return;
    this.DataMinimum = double.IsNaN(this.DataMinimum) ? value : Math.Min(this.DataMinimum, value);
    this.DataMaximum = double.IsNaN(this.DataMaximum) ? value : Math.Max(this.DataMaximum, value);
  }

  internal virtual void ResetDataMaxMin()
  {
    this.DataMaximum = this.DataMinimum = this.ActualMaximum = this.ActualMinimum = double.NaN;
  }

  internal virtual void UpdateActualMaxMin()
  {
    this.ActualMaximum = double.IsNaN(this.ViewMaximum) ? (double.IsNaN(this.Maximum) ? this.CalculateActualMaximum() : this.Maximum) : this.ViewMaximum;
    this.ActualMinimum = double.IsNaN(this.ViewMinimum) ? (double.IsNaN(this.Minimum) ? this.CalculateActualMinimum() : this.Minimum) : this.ViewMinimum;
    this.CoerceActualMaxMin();
  }

  internal virtual void UpdateFromSeries(OxyPlot.Series.Series[] series)
  {
  }

  internal virtual void UpdateIntervals(OxyRect plotArea)
  {
    double intervalLength = this.IntervalLength;
    double availableSize = (this.IsHorizontal() ? plotArea.Width : plotArea.Height) * Math.Abs(this.EndPosition - this.StartPosition);
    this.ActualMajorStep = !double.IsNaN(this.MajorStep) ? this.MajorStep : this.CalculateActualInterval(availableSize, intervalLength);
    this.ActualMinorStep = !double.IsNaN(this.MinorStep) ? this.MinorStep : this.CalculateMinorInterval(this.ActualMajorStep);
    if (double.IsNaN(this.ActualMinorStep))
      this.ActualMinorStep = 2.0;
    if (double.IsNaN(this.ActualMajorStep))
      this.ActualMajorStep = 10.0;
    this.ActualStringFormat = this.StringFormat;
  }

  internal virtual void UpdateTransform(OxyRect bounds)
  {
    double left = bounds.Left;
    double right = bounds.Right;
    double bottom = bounds.Bottom;
    double top = bounds.Top;
    this.ScreenMin = new ScreenPoint(left, top);
    this.ScreenMax = new ScreenPoint(right, bottom);
    double num1 = this.IsHorizontal() ? left : bottom;
    double num2 = (this.IsHorizontal() ? right : top) - num1;
    double num3 = num1 + this.EndPosition * num2;
    double num4 = num1 + this.StartPosition * num2;
    this.ScreenMin = new ScreenPoint(num4, num3);
    this.ScreenMax = new ScreenPoint(num3, num4);
    if (this.ActualMaximum - this.ActualMinimum < double.Epsilon)
      this.ActualMaximum = this.ActualMinimum + 1.0;
    double num5 = this.PreTransform(this.ActualMaximum);
    double num6 = this.PreTransform(this.ActualMinimum);
    double num7 = num4 - num3;
    double newOffset = Math.Abs(num7) <= double.Epsilon ? 0.0 : num4 / num7 * num5 - num3 / num7 * num6;
    double num8 = num5 - num6;
    this.SetTransform(Math.Abs(num8) <= double.Epsilon ? 1.0 : (num3 - num4) / num8, newOffset);
  }

  protected internal virtual void ResetCurrentValues()
  {
  }

  protected virtual double PostInverseTransform(double x) => x;

  protected virtual double PreTransform(double x) => x;

  protected virtual string FormatValueOverride(double x)
  {
    if (!this.UseSuperExponentialFormat || x.Equals(0.0))
      return string.Format((IFormatProvider) this.ActualCulture, $"{{0:{this.ActualStringFormat ?? this.StringFormat ?? string.Empty}}}", (object) x);
    double num1 = Axis.Exponent(x);
    double num2 = Axis.Mantissa(x);
    return string.Format((IFormatProvider) this.ActualCulture, this.StringFormat != null ? $"{{0:{this.StringFormat}}}·10^{{{{{{1:0}}}}}}" : (Math.Abs(num2 - 1.0) < 1E-06 ? "10^{{{1:0}}}" : "{0}·10^{{{1:0}}}"), (object) num2, (object) num1);
  }

  protected virtual double CalculateActualMaximum()
  {
    double dataMaximum = this.DataMaximum;
    if (this.DataMaximum - this.DataMinimum < double.Epsilon)
    {
      double num = this.DataMaximum > 0.0 ? this.DataMaximum : 1.0;
      dataMaximum += num * 0.5;
    }
    if (double.IsNaN(this.DataMinimum) || double.IsNaN(dataMaximum))
      return dataMaximum;
    double num1 = this.PreTransform(dataMaximum);
    double num2 = this.PreTransform(this.DataMinimum);
    double num3 = this.MaximumPadding * (num1 - num2);
    return this.PostInverseTransform(num1 + num3);
  }

  protected virtual double CalculateActualMinimum()
  {
    double dataMinimum = this.DataMinimum;
    if (this.DataMaximum - this.DataMinimum < double.Epsilon)
    {
      double num = this.DataMaximum > 0.0 ? this.DataMaximum : 1.0;
      dataMinimum -= num * 0.5;
    }
    if (double.IsNaN(this.ActualMaximum))
      return dataMinimum;
    double num1 = this.PreTransform(this.ActualMaximum);
    double num2 = this.PreTransform(dataMinimum);
    double num3 = this.MinimumPadding * (num1 - num2);
    return this.PostInverseTransform(num2 - num3);
  }

  protected void SetTransform(double newScale, double newOffset)
  {
    this.scale = newScale;
    this.offset = newOffset;
    this.OnTransformChanged(new EventArgs());
  }

  protected virtual double CalculateActualInterval(double availableSize, double maxIntervalSize)
  {
    return this.CalculateActualInterval(availableSize, maxIntervalSize, this.ActualMaximum - this.ActualMinimum);
  }

  protected double CalculateActualInterval(
    double availableSize,
    double maxIntervalSize,
    double range)
  {
    if (availableSize <= 0.0)
      return maxIntervalSize;
    Func<double, double> exponent = (Func<double, double>) (x => Math.Ceiling(Math.Log(x, 10.0)));
    Func<double, double> func1 = (Func<double, double>) (x => x / Math.Pow(10.0, exponent(x) - 1.0));
    double num = availableSize / maxIntervalSize;
    range = Math.Abs(range);
    double actualInterval = Math.Pow(10.0, exponent(range));
    double d = actualInterval;
    Func<double, double> func2 = (Func<double, double>) (x => double.Parse(x.ToString("e14")));
    while (true)
    {
      switch ((int) func1(d))
      {
        case 1:
        case 2:
        case 10:
          d = func2(d / 2.0);
          break;
        case 5:
          d = func2(d / 2.5);
          break;
        default:
          d = func2(d / 2.0);
          break;
      }
      if (range / d <= num && !double.IsNaN(d) && !double.IsInfinity(d))
        actualInterval = d;
      else
        break;
    }
    return actualInterval;
  }

  protected double CalculateMinorInterval(double majorInterval) => majorInterval / 5.0;

  protected virtual void OnAxisChanged(AxisChangedEventArgs args)
  {
    this.UpdateActualMaxMin();
    EventHandler<AxisChangedEventArgs> axisChanged = this.AxisChanged;
    if (axisChanged == null)
      return;
    axisChanged((object) this, args);
  }

  protected virtual void OnTransformChanged(EventArgs args)
  {
    EventHandler transformChanged = this.TransformChanged;
    if (transformChanged == null)
      return;
    transformChanged((object) this, args);
  }
}
