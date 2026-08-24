// Decompiled with JetBrains decompiler
// Type: OxyPlot.PlotModel
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#nullable disable
namespace OxyPlot;

public class PlotModel : Model, IPlotModel
{
  private WeakReference plotViewReference;
  private int currentColorIndex;
  private Exception lastPlotException;
  private bool _isLegendVisible;

  public PlotModel()
  {
    this.Axes = new ElementCollection<Axis>((Model) this);
    this.Series = new ElementCollection<OxyPlot.Series.Series>((Model) this);
    this.Annotations = new ElementCollection<Annotation>((Model) this);
    this.PlotType = PlotType.XY;
    this.PlotMargins = new OxyThickness(double.NaN);
    this.Padding = new OxyThickness(8.0);
    this.Background = OxyColors.Undefined;
    this.PlotAreaBackground = OxyColors.Undefined;
    this.TextColor = OxyColors.Black;
    this.TitleColor = OxyColors.Automatic;
    this.SubtitleColor = OxyColors.Automatic;
    this.DefaultFont = "Segoe UI";
    this.DefaultFontSize = 12.0;
    this.TitleToolTip = (string) null;
    this.TitleFont = (string) null;
    this.TitleFontSize = 18.0;
    this.TitleFontWeight = 700.0;
    this.SubtitleFont = (string) null;
    this.SubtitleFontSize = 14.0;
    this.SubtitleFontWeight = 400.0;
    this.TitlePadding = 6.0;
    this.PlotAreaBorderColor = OxyColors.Black;
    this.PlotAreaBorderThickness = new OxyThickness(1.0);
    this.IsLegendVisible = true;
    this.RenderLegendsInsidePrint = false;
    this.LegendTitleFont = (string) null;
    this.LegendTitleFontSize = 12.0;
    this.LegendTitleFontWeight = 700.0;
    this.LegendFont = (string) null;
    this.LegendFontSize = 12.0;
    this.LegendFontWeight = 400.0;
    this.LegendSymbolLength = 16.0;
    this.LegendSymbolMargin = 4.0;
    this.LegendPadding = 8.0;
    this.LegendColumnSpacing = 8.0;
    this.LegendItemSpacing = 24.0;
    this.LegendLineSpacing = 0.0;
    this.LegendMargin = 8.0;
    this.LegendBackground = OxyColors.Undefined;
    this.LegendBorder = OxyColors.Undefined;
    this.LegendBorderThickness = 1.0;
    this.LegendTextColor = OxyColors.Automatic;
    this.LegendTitleColor = OxyColors.Automatic;
    this.LegendMaxWidth = double.NaN;
    this.LegendMaxHeight = double.NaN;
    this.LegendPlacement = LegendPlacement.Outside;
    this.LegendPosition = LegendPosition.RightTop;
    this.LegendOrientation = LegendOrientation.Vertical;
    this.LegendItemOrder = LegendItemOrder.Normal;
    this.LegendItemAlignment = HorizontalAlignment.Left;
    this.LegendSymbolPlacement = LegendSymbolPlacement.Left;
    this.DefaultColors = (IList<OxyColor>) new List<OxyColor>()
    {
      OxyColor.FromRgb((byte) 78, (byte) 154, (byte) 6),
      OxyColor.FromRgb((byte) 200, (byte) 141, (byte) 0),
      OxyColor.FromRgb((byte) 204, (byte) 0, (byte) 0),
      OxyColor.FromRgb((byte) 32 /*0x20*/, (byte) 74, (byte) 135),
      OxyColors.Red,
      OxyColors.Orange,
      OxyColors.Yellow,
      OxyColors.Green,
      OxyColors.Blue,
      OxyColors.Indigo,
      OxyColors.Violet
    };
    this.AxisTierDistance = 4.0;
  }

  public event EventHandler<TrackerEventArgs> TrackerChanged;

  public event EventHandler Updated;

  public event EventHandler Updating;

  public string DefaultFont { get; set; }

  public double DefaultFontSize { get; set; }

  public CultureInfo ActualCulture => this.Culture ?? CultureInfo.CurrentCulture;

  public OxyThickness ActualPlotMargins { get; private set; }

  public IPlotView PlotView
  {
    get
    {
      return this.plotViewReference == null ? (IPlotView) null : (IPlotView) this.plotViewReference.Target;
    }
  }

  public ElementCollection<Annotation> Annotations { get; private set; }

  public ElementCollection<Axis> Axes { get; private set; }

  public OxyColor Background { get; set; }

  public CultureInfo Culture { get; set; }

  public IList<OxyColor> DefaultColors { get; set; }

  public bool IsLegendVisible
  {
    get => this._isLegendVisible;
    set => this._isLegendVisible = value;
  }

  public OxyRect LegendArea { get; private set; }

  public OxyColor LegendBackground { get; set; }

  public OxyColor LegendBorder { get; set; }

  public double LegendBorderThickness { get; set; }

  public double LegendColumnSpacing { get; set; }

  public string LegendFont { get; set; }

  public double LegendFontSize { get; set; }

  public OxyColor LegendTextColor { get; set; }

  public double LegendFontWeight { get; set; }

  public HorizontalAlignment LegendItemAlignment { get; set; }

  public LegendItemOrder LegendItemOrder { get; set; }

  public double LegendItemSpacing { get; set; }

  public double LegendLineSpacing { get; set; }

  public double LegendMargin { get; set; }

  public double LegendMaxWidth { get; set; }

  public double LegendMaxHeight { get; set; }

  public LegendOrientation LegendOrientation { get; set; }

  public double LegendPadding { get; set; }

  public LegendPlacement LegendPlacement { get; set; }

  public LegendPosition LegendPosition { get; set; }

  public double LegendSymbolLength { get; set; }

  public double LegendSymbolMargin { get; set; }

  public LegendSymbolPlacement LegendSymbolPlacement { get; set; }

  public string LegendTitle { get; set; }

  public OxyColor LegendTitleColor { get; set; }

  public string LegendTitleFont { get; set; }

  public double LegendTitleFontSize { get; set; }

  public double LegendTitleFontWeight { get; set; }

  public OxyThickness Padding { get; set; }

  public double Width { get; private set; }

  public double Height { get; private set; }

  public OxyRect PlotAndAxisArea { get; private set; }

  public OxyRect PlotArea { get; private set; }

  public double AxisTierDistance { get; set; }

  public OxyColor PlotAreaBackground { get; set; }

  public OxyColor PlotAreaBorderColor { get; set; }

  public OxyThickness PlotAreaBorderThickness { get; set; }

  public OxyThickness PlotMargins { get; set; }

  public PlotType PlotType { get; set; }

  public ElementCollection<OxyPlot.Series.Series> Series { get; private set; }

  public Func<IRenderContext, IRenderContext> RenderingDecorator { get; set; }

  public string Subtitle { get; set; }

  public string SubtitleFont { get; set; }

  public double SubtitleFontSize { get; set; }

  public double SubtitleFontWeight { get; set; }

  public OxyColor TextColor { get; set; }

  public string Title { get; set; }

  public string TitleToolTip { get; set; }

  public OxyColor TitleColor { get; set; }

  public OxyColor SubtitleColor { get; set; }

  public TitleHorizontalAlignment TitleHorizontalAlignment { get; set; }

  public OxyRect TitleArea { get; private set; }

  public string TitleFont { get; set; }

  public double TitleFontSize { get; set; }

  public double TitleFontWeight { get; set; }

  public double TitlePadding { get; set; }

  public AngleAxis DefaultAngleAxis { get; private set; }

  public MagnitudeAxis DefaultMagnitudeAxis { get; private set; }

  public Axis DefaultXAxis { get; private set; }

  public Axis DefaultYAxis { get; private set; }

  public IColorAxis DefaultColorAxis { get; private set; }

  protected string ActualTitleFont => this.TitleFont ?? this.DefaultFont;

  protected string ActualSubtitleFont => this.SubtitleFont ?? this.DefaultFont;

  void IPlotModel.AttachPlotView(IPlotView plotView)
  {
    IPlotView plotView1 = this.PlotView;
    if (plotView1 != null && plotView != null && plotView1 != plotView)
      throw new InvalidOperationException("This PlotModel is already in use by some other PlotView control.");
    this.plotViewReference = plotView == null ? (WeakReference) null : new WeakReference((object) plotView);
  }

  public void InvalidatePlot(bool updateData) => this.PlotView?.InvalidatePlot(updateData);

  public void GetAxesFromPoint(ScreenPoint pt, out Axis xaxis, out Axis yaxis)
  {
    xaxis = yaxis = (Axis) null;
    AxisPosition? nullable1 = new AxisPosition?();
    double num1 = 0.0;
    if (pt.X < this.PlotArea.Left)
    {
      nullable1 = new AxisPosition?(AxisPosition.Left);
      num1 = this.PlotArea.Left;
    }
    if (pt.X > this.PlotArea.Right)
    {
      nullable1 = new AxisPosition?(AxisPosition.Right);
      num1 = this.PlotArea.Right;
    }
    if (pt.Y < this.PlotArea.Top)
    {
      nullable1 = new AxisPosition?(AxisPosition.Top);
      num1 = this.PlotArea.Top;
    }
    if (pt.Y > this.PlotArea.Bottom)
    {
      nullable1 = new AxisPosition?(AxisPosition.Bottom);
      num1 = this.PlotArea.Bottom;
    }
    foreach (Axis ax in this.Axes)
    {
      switch (ax)
      {
        case IColorAxis _:
          continue;
        case MagnitudeAxis _:
          xaxis = ax;
          continue;
        case AngleAxis _:
          yaxis = ax;
          continue;
        default:
          double num2 = double.NaN;
          if (ax.IsHorizontal())
            num2 = ax.InverseTransform(pt.X);
          if (ax.IsVertical())
            num2 = ax.InverseTransform(pt.Y);
          if (num2 >= ax.ActualMinimum && num2 <= ax.ActualMaximum)
          {
            if (!nullable1.HasValue)
            {
              if (ax.IsHorizontal())
              {
                if (xaxis == null)
                {
                  xaxis = ax;
                  continue;
                }
                continue;
              }
              if (ax.IsVertical() && yaxis == null)
              {
                yaxis = ax;
                continue;
              }
              continue;
            }
            AxisPosition? nullable2 = nullable1;
            AxisPosition position = ax.Position;
            if (nullable2.GetValueOrDefault() == position & nullable2.HasValue)
            {
              double positionTierMinShift = ax.PositionTierMinShift;
              double positionTierMaxShift = ax.PositionTierMaxShift;
              double num3 = ax.IsHorizontal() ? pt.Y : pt.X;
              nullable2 = nullable1;
              AxisPosition axisPosition1 = AxisPosition.Top;
              int num4;
              if (!(nullable2.GetValueOrDefault() == axisPosition1 & nullable2.HasValue))
              {
                nullable2 = nullable1;
                AxisPosition axisPosition2 = AxisPosition.Left;
                num4 = nullable2.GetValueOrDefault() == axisPosition2 & nullable2.HasValue ? 1 : 0;
              }
              else
                num4 = 1;
              bool flag = num4 != 0;
              if (num3 >= num1 + positionTierMinShift && num3 < num1 + positionTierMaxShift && !flag || ((num3 > num1 - positionTierMinShift ? 0 : (num3 > num1 - positionTierMaxShift ? 1 : 0)) & (flag ? 1 : 0)) != 0)
              {
                if (ax.IsHorizontal())
                {
                  if (xaxis == null)
                  {
                    xaxis = ax;
                    continue;
                  }
                  continue;
                }
                if (ax.IsVertical() && yaxis == null)
                {
                  yaxis = ax;
                  continue;
                }
                continue;
              }
              continue;
            }
            continue;
          }
          continue;
      }
    }
  }

  public OxyColor GetDefaultColor()
  {
    return this.DefaultColors[this.currentColorIndex++ % this.DefaultColors.Count];
  }

  public LineStyle GetDefaultLineStyle()
  {
    return (LineStyle) (this.currentColorIndex / this.DefaultColors.Count % 10);
  }

  public OxyPlot.Series.Series GetSeriesFromPoint(ScreenPoint point, double limit = 100.0)
  {
    double num1 = double.MaxValue;
    OxyPlot.Series.Series series1 = (OxyPlot.Series.Series) null;
    foreach (OxyPlot.Series.Series series2 in this.Series.Reverse<OxyPlot.Series.Series>().Where<OxyPlot.Series.Series>((Func<OxyPlot.Series.Series, bool>) (s => s.IsVisible)))
    {
      TrackerHitResult trackerHitResult = series2.GetNearestPoint(point, true) ?? series2.GetNearestPoint(point, false);
      if (trackerHitResult != null)
      {
        double num2 = point.DistanceTo(trackerHitResult.Position);
        if (num2 < num1)
        {
          series1 = series2;
          num1 = num2;
        }
      }
    }
    return num1 < limit ? series1 : (OxyPlot.Series.Series) null;
  }

  public string ToCode() => new CodeGenerator(this).ToCode();

  public override string ToString() => this.Title;

  public Exception GetLastPlotException() => this.lastPlotException;

  void IPlotModel.Update(bool updateData)
  {
    lock (this.SyncRoot)
    {
      try
      {
        this.lastPlotException = (Exception) null;
        this.OnUpdating();
        this.EnsureDefaultAxes();
        OxyPlot.Series.Series[] array = this.Series.Where<OxyPlot.Series.Series>((Func<OxyPlot.Series.Series, bool>) (s => s.IsVisible)).ToArray<OxyPlot.Series.Series>();
        if (updateData)
        {
          foreach (OxyPlot.Series.Series series in array)
            series.UpdateData();
        }
        foreach (Axis ax in this.Axes)
        {
          ax.UpdateFromSeries(array);
          ax.ResetCurrentValues();
        }
        if (updateData)
        {
          foreach (OxyPlot.Series.Series series in array)
            series.UpdateValidData();
        }
        this.UpdateMaxMin(updateData);
        this.ResetDefaultColor();
        foreach (OxyPlot.Series.Series series in array)
          series.SetDefaultValues();
        this.OnUpdated();
      }
      catch (Exception ex)
      {
        this.lastPlotException = ex;
      }
    }
  }

  public Axis GetAxisOrDefault(string key, Axis defaultAxis)
  {
    if (key == null)
      return defaultAxis;
    return this.Axes.FirstOrDefault<Axis>((Func<Axis, bool>) (a => a.Key == key)) ?? throw new InvalidOperationException($"Cannot find axis with Key = \"{key}\"");
  }

  public void ResetAllAxes()
  {
    foreach (Axis ax in this.Axes)
      ax.Reset();
  }

  public void PanAllAxes(double dx, double dy)
  {
    foreach (Axis ax in this.Axes)
      ax.Pan(ax.IsHorizontal() ? dx : dy);
  }

  public void ZoomAllAxes(double factor)
  {
    foreach (Axis ax in this.Axes)
      ax.ZoomAtCenter(factor);
  }

  protected internal virtual void OnTrackerChanged(TrackerHitResult result)
  {
    EventHandler<TrackerEventArgs> trackerChanged = this.TrackerChanged;
    if (trackerChanged == null)
      return;
    TrackerEventArgs e = new TrackerEventArgs()
    {
      HitResult = result
    };
    trackerChanged((object) this, e);
  }

  protected override IEnumerable<PlotElement> GetHitTestElements()
  {
    foreach (PlotElement hitTestElement in this.Axes.Reverse<Axis>().Where<Axis>((Func<Axis, bool>) (a => a.IsAxisVisible && a.Layer == AxisLayer.AboveSeries)))
      yield return hitTestElement;
    foreach (PlotElement hitTestElement in this.Annotations.Reverse<Annotation>().Where<Annotation>((Func<Annotation, bool>) (a => a.Layer == AnnotationLayer.AboveSeries)))
      yield return hitTestElement;
    foreach (PlotElement hitTestElement in this.Series.Reverse<OxyPlot.Series.Series>().Where<OxyPlot.Series.Series>((Func<OxyPlot.Series.Series, bool>) (s => s.IsVisible)))
      yield return hitTestElement;
    foreach (PlotElement hitTestElement in this.Annotations.Reverse<Annotation>().Where<Annotation>((Func<Annotation, bool>) (a => a.Layer == AnnotationLayer.BelowSeries)))
      yield return hitTestElement;
    foreach (PlotElement hitTestElement in this.Axes.Reverse<Axis>().Where<Axis>((Func<Axis, bool>) (a => a.IsAxisVisible && a.Layer == AxisLayer.BelowSeries)))
      yield return hitTestElement;
    foreach (PlotElement hitTestElement in this.Annotations.Reverse<Annotation>().Where<Annotation>((Func<Annotation, bool>) (a => a.Layer == AnnotationLayer.BelowAxes)))
      yield return hitTestElement;
  }

  protected virtual void OnUpdated()
  {
    EventHandler updated = this.Updated;
    if (updated == null)
      return;
    EventArgs e = new EventArgs();
    updated((object) this, e);
  }

  protected virtual void OnUpdating()
  {
    EventHandler updating = this.Updating;
    if (updating == null)
      return;
    EventArgs e = new EventArgs();
    updating((object) this, e);
  }

  private void UpdateAxisTransforms()
  {
    foreach (Axis ax in this.Axes)
      ax.UpdateTransform(this.PlotArea);
  }

  private void EnforceCartesianTransforms()
  {
    Axis[] array = this.Axes.Where<Axis>((Func<Axis, bool>) (a => !(a is IColorAxis))).ToArray<Axis>();
    double newScale1 = ((IEnumerable<Axis>) array).Min<Axis>((Func<Axis, double>) (a => Math.Abs(a.Scale)));
    foreach (Axis axis in array)
      axis.Zoom(newScale1);
    double newScale2 = ((IEnumerable<Axis>) array).Max<Axis>((Func<Axis, double>) (a => Math.Abs(a.Scale)));
    foreach (Axis axis in array)
      axis.Zoom(newScale2);
    foreach (Axis axis in array)
      axis.UpdateTransform(this.PlotArea);
  }

  private void UpdateIntervals()
  {
    foreach (Axis ax in this.Axes)
      ax.UpdateIntervals(this.PlotArea);
  }

  private void EnsureDefaultAxes()
  {
    this.DefaultXAxis = this.Axes.FirstOrDefault<Axis>((Func<Axis, bool>) (a => a.IsHorizontal() && a.IsXyAxis()));
    this.DefaultYAxis = this.Axes.FirstOrDefault<Axis>((Func<Axis, bool>) (a => a.IsVertical() && a.IsXyAxis()));
    this.DefaultMagnitudeAxis = this.Axes.FirstOrDefault<Axis>((Func<Axis, bool>) (a => a is MagnitudeAxis)) as MagnitudeAxis;
    this.DefaultAngleAxis = this.Axes.FirstOrDefault<Axis>((Func<Axis, bool>) (a => a is AngleAxis)) as AngleAxis;
    this.DefaultColorAxis = this.Axes.FirstOrDefault<Axis>((Func<Axis, bool>) (a => a is IColorAxis)) as IColorAxis;
    if (this.DefaultXAxis == null)
      this.DefaultXAxis = (Axis) this.DefaultMagnitudeAxis;
    if (this.DefaultYAxis == null)
      this.DefaultYAxis = (Axis) this.DefaultAngleAxis;
    if (this.PlotType == PlotType.Polar)
    {
      if (this.DefaultXAxis == null)
        this.DefaultXAxis = (Axis) (this.DefaultMagnitudeAxis = new MagnitudeAxis());
      if (this.DefaultYAxis == null)
        this.DefaultYAxis = (Axis) (this.DefaultAngleAxis = new AngleAxis());
    }
    else
    {
      bool flag1 = false;
      bool flag2 = false;
      if (this.DefaultXAxis == null)
      {
        if (this.Series.Any<OxyPlot.Series.Series>((Func<OxyPlot.Series.Series, bool>) (s => s.IsVisible && s is ColumnSeries)))
        {
          CategoryAxis categoryAxis = new CategoryAxis();
          categoryAxis.Position = AxisPosition.Bottom;
          this.DefaultXAxis = (Axis) categoryAxis;
        }
        else
        {
          LinearAxis linearAxis = new LinearAxis();
          linearAxis.Position = AxisPosition.Bottom;
          this.DefaultXAxis = (Axis) linearAxis;
          flag1 = true;
        }
      }
      if (this.DefaultYAxis == null)
      {
        if (this.Series.Any<OxyPlot.Series.Series>((Func<OxyPlot.Series.Series, bool>) (s => s.IsVisible && s is BarSeries)))
        {
          CategoryAxis categoryAxis = new CategoryAxis();
          categoryAxis.Position = AxisPosition.Left;
          this.DefaultYAxis = (Axis) categoryAxis;
        }
        else
        {
          LinearAxis linearAxis = new LinearAxis();
          linearAxis.Position = AxisPosition.Left;
          this.DefaultYAxis = (Axis) linearAxis;
          flag2 = true;
        }
      }
      if (flag1 && this.DefaultYAxis is CategoryAxis)
        this.DefaultXAxis.MinimumPadding = 0.0;
      if (flag2 && this.DefaultXAxis is CategoryAxis)
        this.DefaultYAxis.MinimumPadding = 0.0;
    }
    if (this.Series.Any<OxyPlot.Series.Series>((Func<OxyPlot.Series.Series, bool>) (s => s.IsVisible && s.AreAxesRequired())))
    {
      if (!this.Axes.Contains(this.DefaultXAxis) && this.DefaultXAxis != null)
        this.Axes.Add(this.DefaultXAxis);
      if (!this.Axes.Contains(this.DefaultYAxis) && this.DefaultYAxis != null)
        this.Axes.Add(this.DefaultYAxis);
    }
    foreach (OxyPlot.Series.Series series in this.Series)
    {
      if (series.IsVisible && series.AreAxesRequired())
        series.EnsureAxes();
    }
    foreach (Annotation annotation in this.Annotations)
      annotation.EnsureAxes();
  }

  private void ResetDefaultColor() => this.currentColorIndex = 0;

  private void UpdateMaxMin(bool isDataUpdated)
  {
    if (isDataUpdated)
    {
      foreach (Axis ax in this.Axes)
        ax.ResetDataMaxMin();
      foreach (OxyPlot.Series.Series series in this.Series.Where<OxyPlot.Series.Series>((Func<OxyPlot.Series.Series, bool>) (s => s.IsVisible)))
        series.UpdateMaxMin();
    }
    foreach (OxyPlot.Series.Series series in this.Series.Where<OxyPlot.Series.Series>((Func<OxyPlot.Series.Series, bool>) (s => s.IsVisible)))
      series.UpdateAxisMaxMin();
    foreach (Axis ax in this.Axes)
      ax.UpdateActualMaxMin();
  }

  private void EnsureLegendProperties()
  {
    switch (this.LegendPosition)
    {
      case LegendPosition.LeftTop:
      case LegendPosition.LeftMiddle:
      case LegendPosition.LeftBottom:
      case LegendPosition.RightTop:
      case LegendPosition.RightMiddle:
      case LegendPosition.RightBottom:
        if (this.LegendOrientation != LegendOrientation.Horizontal)
          break;
        this.LegendOrientation = LegendOrientation.Vertical;
        break;
    }
  }

  private OxyRect GetLegendRectangle(OxySize legendSize)
  {
    double top1 = 0.0;
    double left1 = 0.0;
    if (this.LegendPlacement == LegendPlacement.Outside)
    {
      OxyRect oxyRect;
      switch (this.LegendPosition)
      {
        case LegendPosition.TopLeft:
        case LegendPosition.TopCenter:
        case LegendPosition.TopRight:
          oxyRect = this.PlotAndAxisArea;
          top1 = oxyRect.Top - legendSize.Height - this.LegendMargin;
          break;
        case LegendPosition.BottomLeft:
        case LegendPosition.BottomCenter:
        case LegendPosition.BottomRight:
          oxyRect = this.PlotAndAxisArea;
          top1 = oxyRect.Bottom + this.LegendMargin;
          break;
        case LegendPosition.LeftTop:
        case LegendPosition.LeftMiddle:
        case LegendPosition.LeftBottom:
          oxyRect = this.PlotAndAxisArea;
          left1 = oxyRect.Left - legendSize.Width - this.LegendMargin;
          break;
        case LegendPosition.RightTop:
        case LegendPosition.RightMiddle:
        case LegendPosition.RightBottom:
          oxyRect = this.PlotAndAxisArea;
          left1 = oxyRect.Right + this.LegendMargin;
          break;
      }
      switch (this.LegendPosition)
      {
        case LegendPosition.TopLeft:
        case LegendPosition.BottomLeft:
          oxyRect = this.PlotArea;
          left1 = oxyRect.Left;
          break;
        case LegendPosition.TopCenter:
        case LegendPosition.BottomCenter:
          oxyRect = this.PlotArea;
          double left2 = oxyRect.Left;
          oxyRect = this.PlotArea;
          double right = oxyRect.Right;
          left1 = (left2 + right - legendSize.Width) * 0.5;
          break;
        case LegendPosition.TopRight:
        case LegendPosition.BottomRight:
          oxyRect = this.PlotArea;
          left1 = oxyRect.Right - legendSize.Width;
          break;
        case LegendPosition.LeftTop:
        case LegendPosition.RightTop:
          oxyRect = this.PlotArea;
          top1 = oxyRect.Top;
          break;
        case LegendPosition.LeftMiddle:
        case LegendPosition.RightMiddle:
          oxyRect = this.PlotArea;
          double top2 = oxyRect.Top;
          oxyRect = this.PlotArea;
          double bottom = oxyRect.Bottom;
          top1 = (top2 + bottom - legendSize.Height) * 0.5;
          break;
        case LegendPosition.LeftBottom:
        case LegendPosition.RightBottom:
          oxyRect = this.PlotArea;
          top1 = oxyRect.Bottom - legendSize.Height;
          break;
      }
    }
    else
    {
      OxyRect plotArea;
      switch (this.LegendPosition)
      {
        case LegendPosition.TopLeft:
        case LegendPosition.TopCenter:
        case LegendPosition.TopRight:
          plotArea = this.PlotArea;
          top1 = plotArea.Top + this.LegendMargin;
          break;
        case LegendPosition.BottomLeft:
        case LegendPosition.BottomCenter:
        case LegendPosition.BottomRight:
          plotArea = this.PlotArea;
          top1 = plotArea.Bottom - legendSize.Height - this.LegendMargin;
          break;
        case LegendPosition.LeftTop:
        case LegendPosition.LeftMiddle:
        case LegendPosition.LeftBottom:
          plotArea = this.PlotArea;
          left1 = plotArea.Left + this.LegendMargin;
          break;
        case LegendPosition.RightTop:
        case LegendPosition.RightMiddle:
        case LegendPosition.RightBottom:
          plotArea = this.PlotArea;
          left1 = plotArea.Right - legendSize.Width - this.LegendMargin;
          break;
      }
      switch (this.LegendPosition)
      {
        case LegendPosition.TopLeft:
        case LegendPosition.BottomLeft:
          plotArea = this.PlotArea;
          left1 = plotArea.Left + this.LegendMargin;
          break;
        case LegendPosition.TopCenter:
        case LegendPosition.BottomCenter:
          plotArea = this.PlotArea;
          double left3 = plotArea.Left;
          plotArea = this.PlotArea;
          double right = plotArea.Right;
          left1 = (left3 + right - legendSize.Width) * 0.5;
          break;
        case LegendPosition.TopRight:
        case LegendPosition.BottomRight:
          plotArea = this.PlotArea;
          left1 = plotArea.Right - legendSize.Width - this.LegendMargin;
          break;
        case LegendPosition.LeftTop:
        case LegendPosition.RightTop:
          plotArea = this.PlotArea;
          top1 = plotArea.Top + this.LegendMargin;
          break;
        case LegendPosition.LeftMiddle:
        case LegendPosition.RightMiddle:
          plotArea = this.PlotArea;
          double top3 = plotArea.Top;
          plotArea = this.PlotArea;
          double bottom = plotArea.Bottom;
          top1 = (top3 + bottom - legendSize.Height) * 0.5;
          break;
        case LegendPosition.LeftBottom:
        case LegendPosition.RightBottom:
          plotArea = this.PlotArea;
          top1 = plotArea.Bottom - legendSize.Height - this.LegendMargin;
          break;
      }
    }
    return new OxyRect(left1, top1, legendSize.Width, legendSize.Height);
  }

  private void RenderLegend(IRenderContext rc, OxyPlot.Series.Series s, OxyRect rect)
  {
    HorizontalAlignment ha = this.LegendItemAlignment;
    if (this.LegendOrientation == LegendOrientation.Horizontal)
      ha = HorizontalAlignment.Left;
    double x = rect.Left;
    switch (ha)
    {
      case HorizontalAlignment.Center:
        double num1 = (rect.Left + rect.Right) / 2.0;
        x = this.LegendSymbolPlacement != LegendSymbolPlacement.Left ? num1 + (this.LegendSymbolLength + this.LegendSymbolMargin) / 2.0 : num1 - (this.LegendSymbolLength + this.LegendSymbolMargin) / 2.0;
        break;
      case HorizontalAlignment.Right:
        x = rect.Right - (this.LegendSymbolLength + this.LegendSymbolMargin);
        break;
    }
    if (this.LegendSymbolPlacement == LegendSymbolPlacement.Left)
      x += this.LegendSymbolLength + this.LegendSymbolMargin;
    double top = rect.Top;
    OxySize oxySize1 = new OxySize(Math.Max(rect.Width - this.LegendSymbolLength - this.LegendSymbolMargin, 0.0), rect.Height);
    rc.SetToolTip(s.ToolTip);
    OxySize oxySize2 = rc.DrawMathText(new ScreenPoint(x, top), s.Title, this.LegendTextColor.GetActualColor(this.TextColor), this.LegendFont ?? this.DefaultFont, this.LegendFontSize, this.LegendFontWeight, 0.0, ha, VerticalAlignment.Top, new OxySize?(oxySize1), true);
    double num2 = x;
    switch (ha)
    {
      case HorizontalAlignment.Center:
        num2 = x - oxySize2.Width * 0.5;
        break;
      case HorizontalAlignment.Right:
        num2 = x - oxySize2.Width;
        break;
    }
    OxyRect legendBox = new OxyRect(this.LegendSymbolPlacement == LegendSymbolPlacement.Right ? num2 + oxySize2.Width + this.LegendSymbolMargin : num2 - this.LegendSymbolMargin - this.LegendSymbolLength, rect.Top, this.LegendSymbolLength, oxySize2.Height);
    s.RenderLegend(rc, legendBox);
    rc.SetToolTip((string) null);
  }

  private OxySize MeasureLegends(IRenderContext rc, OxySize availableSize)
  {
    return this.RenderOrMeasureLegends(rc, new OxyRect(0.0, 0.0, availableSize.Width, availableSize.Height), true);
  }

  private void RenderLegends(IRenderContext rc, OxyRect rect)
  {
    this.RenderOrMeasureLegends(rc, rect);
  }

  private OxySize RenderOrMeasureLegends(IRenderContext rc, OxyRect rect, bool measureOnly = false)
  {
    if (this.Series.Count > 0 && !(this.Series[0] is PieSeriesExt))
    {
      if (!measureOnly && rect.Width > 0.0 && rect.Height > 0.0)
        rc.DrawRectangleAsPolygon(rect, this.LegendBackground, this.LegendBorder, this.LegendBorderThickness);
      double availableWidth = rect.Width;
      double availableHeight = rect.Height;
      double legendPadding1 = this.LegendPadding;
      double legendPadding2 = this.LegendPadding;
      OxySize oxySize1 = new OxySize();
      if (!string.IsNullOrEmpty(this.LegendTitle))
      {
        OxySize oxySize2 = !measureOnly ? rc.DrawMathText(new ScreenPoint(rect.Left + legendPadding1, rect.Top + legendPadding2), this.LegendTitle, this.LegendTitleColor.GetActualColor(this.TextColor), this.LegendTitleFont ?? this.DefaultFont, this.LegendTitleFontSize, this.LegendTitleFontWeight, 0.0, HorizontalAlignment.Left, VerticalAlignment.Top, new OxySize?(), true) : rc.MeasureMathText(this.LegendTitle, this.LegendTitleFont ?? this.DefaultFont, this.LegendTitleFontSize, this.LegendTitleFontWeight);
        legendPadding2 += oxySize2.Height;
        oxySize1 = new OxySize(legendPadding1 + oxySize2.Width + this.LegendPadding, legendPadding2 + oxySize2.Height);
      }
      double val2 = legendPadding2;
      double val1_1 = 0.0;
      double val1_2 = 0.0;
      IEnumerable<OxyPlot.Series.Series> series = this.LegendItemOrder == LegendItemOrder.Reverse ? this.Series.Reverse<OxyPlot.Series.Series>().Where<OxyPlot.Series.Series>((Func<OxyPlot.Series.Series, bool>) (s => s.IsVisible)) : this.Series.Where<OxyPlot.Series.Series>((Func<OxyPlot.Series.Series, bool>) (s => s.IsVisible));
      Dictionary<OxyPlot.Series.Series, OxyRect> seriesToRender = new Dictionary<OxyPlot.Series.Series, OxyRect>();
      Action action = (Action) (() =>
      {
        foreach (KeyValuePair<OxyPlot.Series.Series, OxyRect> keyValuePair in seriesToRender)
        {
          OxyRect oxyRect = keyValuePair.Value;
          OxyPlot.Series.Series key = keyValuePair.Key;
          double val1_3 = availableWidth;
          if (oxyRect.Left + val1_3 + this.LegendPadding > rect.Left + availableWidth)
            val1_3 = rect.Left + availableWidth - oxyRect.Left - this.LegendPadding;
          double val1_4 = oxyRect.Height;
          if (rect.Top + val1_4 + this.LegendPadding > rect.Top + availableHeight)
            val1_4 = rect.Top + availableHeight - rect.Top - this.LegendPadding;
          OxyRect rect1 = new OxyRect(oxyRect.Left, oxyRect.Top, Math.Max(val1_3, 0.0), Math.Max(val1_4, 0.0));
          this.RenderLegend(rc, key, rect1);
        }
        seriesToRender.Clear();
      });
      foreach (OxyPlot.Series.Series key in series)
      {
        if (!string.IsNullOrEmpty(key.Title))
        {
          OxySize oxySize3 = rc.MeasureMathText(key.Title, this.LegendFont ?? this.DefaultFont, this.LegendFontSize, this.LegendFontWeight);
          double num = this.LegendSymbolLength + this.LegendSymbolMargin + oxySize3.Width;
          double height = oxySize3.Height;
          if (this.LegendOrientation == LegendOrientation.Horizontal)
          {
            if (legendPadding1 > this.LegendPadding)
              legendPadding1 += this.LegendItemSpacing;
            if (legendPadding1 + num > availableWidth - this.LegendPadding + 0.001)
            {
              legendPadding1 = this.LegendPadding;
              val2 += val1_1 + this.LegendLineSpacing;
              val1_1 = 0.0;
            }
            val1_1 = Math.Max(val1_1, oxySize3.Height);
            if (!measureOnly)
              seriesToRender.Add(key, new OxyRect(rect.Left + legendPadding1, rect.Top + val2, num, height));
            legendPadding1 += num;
            oxySize1 = new OxySize(Math.Max(oxySize1.Width, legendPadding1), Math.Max(oxySize1.Height, val2 + oxySize3.Height));
          }
          else
          {
            if (val2 + height > availableHeight - this.LegendPadding + 0.001)
            {
              action();
              val2 = legendPadding2;
              legendPadding1 += val1_2 + this.LegendColumnSpacing;
              val1_2 = 0.0;
            }
            if (!measureOnly)
              seriesToRender.Add(key, new OxyRect(rect.Left + legendPadding1, rect.Top + val2, num, height));
            val2 += height + this.LegendLineSpacing;
            val1_2 = Math.Max(val1_2, num);
            oxySize1 = new OxySize(Math.Max(oxySize1.Width, legendPadding1 + num), Math.Max(oxySize1.Height, val2));
          }
        }
      }
      action();
      if (oxySize1.Width > 0.0)
        oxySize1 = new OxySize(oxySize1.Width + this.LegendPadding, oxySize1.Height);
      if (oxySize1.Height > 0.0)
        oxySize1 = new OxySize(oxySize1.Width, oxySize1.Height + this.LegendPadding);
      if (oxySize1.Width > availableWidth)
        oxySize1 = new OxySize(availableWidth, oxySize1.Height);
      if (oxySize1.Height > availableHeight)
        oxySize1 = new OxySize(oxySize1.Width, availableHeight);
      if (!double.IsNaN(this.LegendMaxWidth) && oxySize1.Width > this.LegendMaxWidth)
        oxySize1 = new OxySize(this.LegendMaxWidth, oxySize1.Height);
      if (!double.IsNaN(this.LegendMaxHeight) && oxySize1.Height > this.LegendMaxHeight)
        oxySize1 = new OxySize(oxySize1.Width, this.LegendMaxHeight);
      return oxySize1;
    }
    double availableWidth1 = rect.Width;
    double availableHeight1 = rect.Height;
    double legendPadding3 = this.LegendPadding;
    double legendPadding4 = this.LegendPadding;
    OxySize oxySize4 = new OxySize();
    if (!string.IsNullOrEmpty(this.LegendTitle))
    {
      OxySize oxySize5 = rc.MeasureMathText(this.LegendTitle, this.LegendTitleFont ?? this.DefaultFont, this.LegendTitleFontSize, this.LegendTitleFontWeight);
      legendPadding4 += oxySize5.Height;
      oxySize4 = new OxySize(legendPadding3 + oxySize5.Width + this.LegendPadding, legendPadding4 + oxySize5.Height);
    }
    double val2_1 = legendPadding4;
    double val1_5 = 0.0;
    double val1_6 = 0.0;
    IEnumerable<OxyPlot.Series.Series> series1 = this.LegendItemOrder == LegendItemOrder.Reverse ? this.Series.Reverse<OxyPlot.Series.Series>().Where<OxyPlot.Series.Series>((Func<OxyPlot.Series.Series, bool>) (s => s.IsVisible)) : (IEnumerable<OxyPlot.Series.Series>) this.Series.Where<OxyPlot.Series.Series>((Func<OxyPlot.Series.Series, bool>) (s => s.IsVisible)).ToList<OxyPlot.Series.Series>();
    Dictionary<PieSlice, OxyRect> seriesToRender1 = new Dictionary<PieSlice, OxyRect>();
    Action action1 = (Action) (() =>
    {
      foreach (KeyValuePair<PieSlice, OxyRect> keyValuePair in seriesToRender1)
      {
        OxyRect oxyRect = keyValuePair.Value;
        PieSlice key = keyValuePair.Key;
        double val1_7 = availableWidth1;
        if (oxyRect.Left + val1_7 + this.LegendPadding > rect.Left + availableWidth1)
          val1_7 = rect.Left + availableWidth1 - oxyRect.Left - this.LegendPadding;
        double val1_8 = oxyRect.Height;
        if (rect.Top + val1_8 + this.LegendPadding > rect.Top + availableHeight1)
          val1_8 = rect.Top + availableHeight1 - rect.Top - this.LegendPadding;
        OxyRect rect2 = new OxyRect(oxyRect.Left, oxyRect.Top, Math.Max(val1_7, 0.0), Math.Max(val1_8, 0.0));
        this.RenderLegendForPieSeries(rc, key, rect2);
      }
      seriesToRender1.Clear();
    });
    foreach (OxyPlot.Series.Series series2 in series1)
    {
      if (series2 is PieSeriesExt pieSeriesExt)
      {
        foreach (PieSlice slice in (IEnumerable<PieSlice>) pieSeriesExt.Slices)
        {
          if (!string.IsNullOrEmpty(slice.Label))
          {
            OxySize oxySize6 = rc.MeasureMathText(slice.Label, this.LegendFont ?? this.DefaultFont, this.LegendFontSize, this.LegendFontWeight);
            double num = this.LegendSymbolLength + this.LegendSymbolMargin + oxySize6.Width;
            double height = oxySize6.Height;
            if (this.LegendOrientation == LegendOrientation.Horizontal)
            {
              if (legendPadding3 > this.LegendPadding)
                legendPadding3 += this.LegendItemSpacing;
              if (legendPadding3 + num > availableWidth1 - this.LegendPadding + 0.001)
              {
                legendPadding3 = this.LegendPadding;
                val2_1 += val1_5 + this.LegendLineSpacing;
                val1_5 = 0.0;
              }
              val1_5 = Math.Max(val1_5, oxySize6.Height);
              if (!measureOnly)
                seriesToRender1.Add(slice, new OxyRect(rect.Left + legendPadding3, rect.Top + val2_1, num, height));
              legendPadding3 += num;
              oxySize4 = new OxySize(Math.Max(oxySize4.Width, legendPadding3), Math.Max(oxySize4.Height, val2_1 + oxySize6.Height));
            }
            else
            {
              if (val2_1 + height > availableHeight1 - this.LegendPadding + 0.001)
              {
                val2_1 = legendPadding4;
                legendPadding3 += val1_6 + this.LegendColumnSpacing;
                val1_6 = 0.0;
              }
              if (!measureOnly)
                seriesToRender1.Add(slice, new OxyRect(rect.Left + legendPadding3, rect.Top + val2_1, num, height));
              val2_1 += height + this.LegendLineSpacing;
              val1_6 = Math.Max(val1_6, num);
              oxySize4 = new OxySize(Math.Max(oxySize4.Width, legendPadding3 + num), Math.Max(oxySize4.Height, val2_1));
            }
          }
        }
      }
    }
    if (!measureOnly && rect.Width > 0.0 && rect.Height > 0.0)
      rc.DrawRectangleAsPolygon(rect, this.LegendBackground, this.LegendBorder, this.LegendBorderThickness);
    action1();
    if (oxySize4.Width > 0.0)
      oxySize4 = new OxySize(oxySize4.Width + this.LegendPadding, oxySize4.Height);
    if (oxySize4.Height > 0.0)
      oxySize4 = new OxySize(oxySize4.Width, oxySize4.Height + this.LegendPadding);
    if (oxySize4.Width > availableWidth1)
      oxySize4 = new OxySize(availableWidth1, oxySize4.Height);
    if (oxySize4.Height > availableHeight1)
      oxySize4 = new OxySize(oxySize4.Width, availableHeight1);
    if (!double.IsNaN(this.LegendMaxWidth) && oxySize4.Width > this.LegendMaxWidth)
      oxySize4 = new OxySize(this.LegendMaxWidth, oxySize4.Height);
    if (!double.IsNaN(this.LegendMaxHeight) && oxySize4.Height > this.LegendMaxHeight)
      oxySize4 = new OxySize(oxySize4.Width, this.LegendMaxHeight);
    return oxySize4;
  }

  private void RenderLegendForPieSeries(IRenderContext rc, PieSlice slice, OxyRect rect)
  {
    HorizontalAlignment ha = this.LegendItemAlignment;
    if (this.LegendOrientation == LegendOrientation.Horizontal)
      ha = HorizontalAlignment.Left;
    double x = rect.Left;
    switch (ha)
    {
      case HorizontalAlignment.Center:
        double num1 = (rect.Left + rect.Right) / 2.0;
        x = this.LegendSymbolPlacement != LegendSymbolPlacement.Left ? num1 + (this.LegendSymbolLength + this.LegendSymbolMargin) / 2.0 : num1 - (this.LegendSymbolLength + this.LegendSymbolMargin) / 2.0;
        break;
      case HorizontalAlignment.Right:
        x = rect.Right - (this.LegendSymbolLength + this.LegendSymbolMargin);
        break;
    }
    if (this.LegendSymbolPlacement == LegendSymbolPlacement.Left)
      x += this.LegendSymbolLength + this.LegendSymbolMargin;
    double top = rect.Top;
    OxySize oxySize1 = new OxySize(Math.Max(rect.Width - this.LegendSymbolLength - this.LegendSymbolMargin, 0.0), rect.Height);
    OxySize oxySize2 = rc.DrawMathText(new ScreenPoint(x, top), slice.Label, this.LegendTextColor.GetActualColor(this.TextColor), this.LegendFont ?? this.DefaultFont, this.LegendFontSize, this.LegendFontWeight, 0.0, ha, VerticalAlignment.Top, new OxySize?(oxySize1), true);
    double num2 = x;
    switch (ha)
    {
      case HorizontalAlignment.Center:
        num2 = x - oxySize2.Width * 0.5;
        break;
      case HorizontalAlignment.Right:
        num2 = x - oxySize2.Width;
        break;
    }
    OxyRect legendBox = new OxyRect(this.LegendSymbolPlacement == LegendSymbolPlacement.Right ? num2 + oxySize2.Width + this.LegendSymbolMargin : num2 - this.LegendSymbolMargin - this.LegendSymbolLength, rect.Top, this.LegendSymbolLength, oxySize2.Height);
    this.Series[0].RenderLegend(rc, legendBox);
    double num3 = (legendBox.Left + legendBox.Right) / 2.0;
    double num4 = (legendBox.Top + legendBox.Bottom) / 2.0;
    double height = (legendBox.Bottom - legendBox.Top) * 0.8;
    double width = height;
    rc.DrawRectangleAsPolygon(new OxyRect(num3 - 0.5 * width, num4 - 0.5 * height, width, height), slice.ActualFillColor, OxyColors.Black, 1.0);
    rc.SetToolTip((string) null);
  }

  public bool RenderLegendsInsidePrint { get; set; }

  void IPlotModel.Render(IRenderContext rc, double width, double height)
  {
    this.RenderOverride(rc, width, height);
  }

  protected virtual void RenderOverride(IRenderContext rc, double width, double height)
  {
    lock (this.SyncRoot)
    {
      try
      {
        if (this.lastPlotException != null)
        {
          string errorMessage = $"An exception of type {this.lastPlotException.GetType()} was thrown when updating the plot model.\r\n{this.lastPlotException.GetBaseException().StackTrace}";
          this.RenderErrorMessage(rc, $"OxyPlot exception: {this.lastPlotException.Message}", errorMessage);
        }
        else
        {
          double num1 = this.Padding.Left + this.Padding.Right;
          OxyThickness oxyThickness = this.Padding;
          double top1 = oxyThickness.Top;
          oxyThickness = this.Padding;
          double bottom1 = oxyThickness.Bottom;
          double num2 = top1 + bottom1;
          if (width <= num1 || height <= num2)
            return;
          if (this.RenderingDecorator != null)
            rc = this.RenderingDecorator(rc);
          this.Width = width;
          this.Height = height;
          oxyThickness = this.PlotMargins;
          double left;
          if (!double.IsNaN(oxyThickness.Left))
          {
            oxyThickness = this.PlotMargins;
            left = oxyThickness.Left;
          }
          else
            left = 0.0;
          oxyThickness = this.PlotMargins;
          double top2;
          if (!double.IsNaN(oxyThickness.Top))
          {
            oxyThickness = this.PlotMargins;
            top2 = oxyThickness.Top;
          }
          else
            top2 = 0.0;
          oxyThickness = this.PlotMargins;
          double right;
          if (!double.IsNaN(oxyThickness.Right))
          {
            oxyThickness = this.PlotMargins;
            right = oxyThickness.Right;
          }
          else
            right = 0.0;
          oxyThickness = this.PlotMargins;
          double bottom2;
          if (!double.IsNaN(oxyThickness.Bottom))
          {
            oxyThickness = this.PlotMargins;
            bottom2 = oxyThickness.Bottom;
          }
          else
            bottom2 = 0.0;
          this.ActualPlotMargins = new OxyThickness(left, top2, right, bottom2);
          this.EnsureLegendProperties();
          do
          {
            this.UpdatePlotArea(rc);
            this.UpdateAxisTransforms();
            this.UpdateIntervals();
          }
          while (this.AdjustPlotMargins(rc));
          if (this.PlotType == PlotType.Cartesian)
          {
            this.EnforceCartesianTransforms();
            this.UpdateIntervals();
          }
          foreach (Axis ax in this.Axes)
            ax.ResetCurrentValues();
          this.RenderBackgrounds(rc);
          this.RenderAnnotations(rc, AnnotationLayer.BelowAxes);
          this.RenderAxes(rc, AxisLayer.BelowSeries);
          this.RenderAnnotations(rc, AnnotationLayer.BelowSeries);
          this.RenderSeries(rc);
          this.RenderAnnotations(rc, AnnotationLayer.AboveSeries);
          this.RenderTitle(rc);
          this.RenderBox(rc);
          this.RenderAxes(rc, AxisLayer.AboveSeries);
          if (!this.IsLegendVisible && !this.RenderLegendsInsidePrint)
            return;
          this.RenderLegends(rc, this.LegendArea);
        }
      }
      catch (Exception ex)
      {
        string errorMessage = $"An exception of type {ex.GetType()} was thrown when rendering the plot model.\r\n{ex.GetBaseException().StackTrace}";
        this.lastPlotException = ex;
        this.RenderErrorMessage(rc, $"OxyPlot exception: {ex.Message}", errorMessage);
      }
      finally
      {
        rc.CleanUp();
      }
    }
  }

  private static void EnsureMarginIsBigEnough(
    ref OxyThickness currentMargin,
    double minBorderSize,
    AxisPosition borderPosition)
  {
    switch (borderPosition)
    {
      case AxisPosition.Left:
        currentMargin = new OxyThickness(Math.Max(currentMargin.Left, minBorderSize), currentMargin.Top, currentMargin.Right, currentMargin.Bottom);
        break;
      case AxisPosition.Right:
        currentMargin = new OxyThickness(currentMargin.Left, currentMargin.Top, Math.Max(currentMargin.Right, minBorderSize), currentMargin.Bottom);
        break;
      case AxisPosition.Top:
        currentMargin = new OxyThickness(currentMargin.Left, Math.Max(currentMargin.Top, minBorderSize), currentMargin.Right, currentMargin.Bottom);
        break;
      case AxisPosition.Bottom:
        currentMargin = new OxyThickness(currentMargin.Left, currentMargin.Top, currentMargin.Right, Math.Max(currentMargin.Bottom, minBorderSize));
        break;
      default:
        throw new NotImplementedException();
    }
  }

  private static double MaxSizeOfPositionTier(
    IRenderContext rc,
    IEnumerable<Axis> axesOfPositionTier)
  {
    double num = 0.0;
    foreach (Axis axis in axesOfPositionTier)
    {
      OxySize oxySize = axis.Measure(rc);
      if (axis.IsVertical())
      {
        if (oxySize.Width > num)
          num = oxySize.Width;
      }
      else if (oxySize.Height > num)
        num = oxySize.Height;
    }
    return num;
  }

  private void RenderErrorMessage(
    IRenderContext rc,
    string title,
    string errorMessage,
    double fontSize = 12.0)
  {
    ScreenPoint p = new ScreenPoint(10.0, 10.0);
    rc.DrawText(p, title, this.TextColor, fontSize: fontSize, fontWeight: 700.0);
    rc.DrawMultilineText(p + new ScreenVector(0.0, fontSize * 1.5), errorMessage, this.TextColor, fontSize: fontSize, dy: fontSize * 1.25);
  }

  private bool IsPlotMarginAutoSized(AxisPosition position)
  {
    switch (position)
    {
      case AxisPosition.Left:
        return double.IsNaN(this.PlotMargins.Left);
      case AxisPosition.Right:
        return double.IsNaN(this.PlotMargins.Right);
      case AxisPosition.Top:
        return double.IsNaN(this.PlotMargins.Top);
      case AxisPosition.Bottom:
        return double.IsNaN(this.PlotMargins.Bottom);
      default:
        return false;
    }
  }

  private bool AdjustPlotMargins(IRenderContext rc)
  {
    OxyThickness actualPlotMargins = this.ActualPlotMargins;
    for (AxisPosition position = AxisPosition.Left; position <= AxisPosition.Bottom; position++)
    {
      List<Axis> list = this.Axes.Where<Axis>((Func<Axis, bool>) (a => a.IsAxisVisible && a.Position == position)).ToList<Axis>();
      double minBorderSize = this.AdjustAxesPositions(rc, (IList<Axis>) list);
      if (this.IsPlotMarginAutoSized(position))
        PlotModel.EnsureMarginIsBigEnough(ref actualPlotMargins, minBorderSize, position);
    }
    List<Axis> list1 = this.Axes.Where<Axis>((Func<Axis, bool>) (a => a.IsAxisVisible)).OfType<AngleAxis>().Cast<Axis>().ToList<Axis>();
    if (list1.Any<Axis>())
    {
      double minBorderSize = this.AdjustAxesPositions(rc, (IList<Axis>) list1);
      for (AxisPosition axisPosition = AxisPosition.Left; axisPosition <= AxisPosition.Bottom; ++axisPosition)
      {
        if (this.IsPlotMarginAutoSized(axisPosition))
          PlotModel.EnsureMarginIsBigEnough(ref actualPlotMargins, minBorderSize, axisPosition);
      }
    }
    if (actualPlotMargins.Equals(this.ActualPlotMargins))
      return false;
    this.ActualPlotMargins = actualPlotMargins;
    return true;
  }

  private double AdjustAxesPositions(IRenderContext rc, IList<Axis> parallelAxes)
  {
    double num1 = 0.0;
    foreach (int num2 in (IEnumerable<int>) parallelAxes.Select<Axis, int>((Func<Axis, int>) (a => a.PositionTier)).Distinct<int>().OrderBy<int, int>((Func<int, int>) (l => l)))
    {
      int positionTier = num2;
      List<Axis> list = parallelAxes.Where<Axis>((Func<Axis, bool>) (a => a.PositionTier == positionTier)).ToList<Axis>();
      double num3 = PlotModel.MaxSizeOfPositionTier(rc, (IEnumerable<Axis>) list);
      double num4 = num1;
      if (Math.Abs(num1) > 1E-05)
        num1 += this.AxisTierDistance;
      num1 += num3;
      foreach (Axis axis in list)
      {
        axis.PositionTierSize = num3;
        axis.PositionTierMinShift = num4;
        axis.PositionTierMaxShift = num1;
      }
    }
    return num1;
  }

  private OxySize MeasureTitles(IRenderContext rc)
  {
    OxySize oxySize1 = rc.MeasureText(this.Title, this.ActualTitleFont, this.TitleFontSize, this.TitleFontWeight);
    OxySize oxySize2 = rc.MeasureText(this.Subtitle, this.SubtitleFont ?? this.ActualSubtitleFont, this.SubtitleFontSize, this.SubtitleFontWeight);
    double height = oxySize1.Height + oxySize2.Height;
    return new OxySize(Math.Max(oxySize1.Width, oxySize2.Width), height);
  }

  private void RenderAnnotations(IRenderContext rc, AnnotationLayer layer)
  {
    foreach (Annotation annotation in this.Annotations.Where<Annotation>((Func<Annotation, bool>) (a => a.Layer == layer)))
    {
      rc.SetToolTip(annotation.ToolTip);
      annotation.Render(rc);
    }
    rc.SetToolTip((string) null);
  }

  private void RenderAxes(IRenderContext rc, AxisLayer layer)
  {
    foreach (Axis axis in this.Axes.Where<Axis>((Func<Axis, bool>) (a => a.IsAxisVisible && a.Layer == layer)))
    {
      rc.SetToolTip(axis.ToolTip);
      axis.Render(rc, 0);
    }
    foreach (Axis axis in this.Axes.Where<Axis>((Func<Axis, bool>) (a => a.IsAxisVisible && a.Layer == layer)))
    {
      rc.SetToolTip(axis.ToolTip);
      axis.Render(rc, 1);
    }
    rc.SetToolTip((string) null);
  }

  private void RenderBackgrounds(IRenderContext rc)
  {
    if (this.Axes.Count > 0 && this.PlotAreaBackground.IsVisible())
      rc.DrawRectangleAsPolygon(this.PlotArea, this.PlotAreaBackground, OxyColors.Undefined, 0.0);
    foreach (XYAxisSeries xyAxisSeries in this.Series.Where<OxyPlot.Series.Series>((Func<OxyPlot.Series.Series, bool>) (s => s.IsVisible && s is XYAxisSeries && s.Background.IsVisible())).Cast<XYAxisSeries>())
      rc.DrawRectangle(xyAxisSeries.GetScreenRectangle(), xyAxisSeries.Background, OxyColors.Undefined, 0.0);
  }

  private void RenderBox(IRenderContext rc)
  {
    if (this.Axes.Count <= 0)
      return;
    rc.DrawRectangleAsPolygon(this.PlotArea, OxyColors.Undefined, this.PlotAreaBorderColor, this.PlotAreaBorderThickness);
  }

  private void RenderSeries(IRenderContext rc)
  {
    foreach (OxyPlot.Series.Series series in this.Series.Where<OxyPlot.Series.Series>((Func<OxyPlot.Series.Series, bool>) (s => s.IsVisible)))
    {
      rc.SetToolTip(series.ToolTip);
      series.Render(rc);
    }
    rc.SetToolTip((string) null);
  }

  private void RenderTitle(IRenderContext rc)
  {
    OxySize oxySize = rc.MeasureText(this.Title, this.ActualTitleFont, this.TitleFontSize, this.TitleFontWeight);
    OxyRect titleArea = this.TitleArea;
    double left = titleArea.Left;
    titleArea = this.TitleArea;
    double right = titleArea.Right;
    double x = (left + right) * 0.5;
    double top = this.TitleArea.Top;
    OxyColor oxyColor;
    if (!string.IsNullOrEmpty(this.Title))
    {
      rc.SetToolTip(this.TitleToolTip);
      IRenderContext rc1 = rc;
      ScreenPoint pt = new ScreenPoint(x, top);
      string title = this.Title;
      oxyColor = this.TitleColor;
      OxyColor actualColor = oxyColor.GetActualColor(this.TextColor);
      string actualTitleFont = this.ActualTitleFont;
      double titleFontSize = this.TitleFontSize;
      double titleFontWeight = this.TitleFontWeight;
      OxySize? maxSize = new OxySize?();
      rc1.DrawMathText(pt, title, actualColor, actualTitleFont, titleFontSize, titleFontWeight, 0.0, HorizontalAlignment.Center, VerticalAlignment.Top, maxSize);
      top += oxySize.Height;
      rc.SetToolTip((string) null);
    }
    if (string.IsNullOrEmpty(this.Subtitle))
      return;
    IRenderContext rc2 = rc;
    ScreenPoint pt1 = new ScreenPoint(x, top);
    string subtitle = this.Subtitle;
    oxyColor = this.SubtitleColor;
    OxyColor actualColor1 = oxyColor.GetActualColor(this.TextColor);
    string actualSubtitleFont = this.ActualSubtitleFont;
    double subtitleFontSize = this.SubtitleFontSize;
    double subtitleFontWeight = this.SubtitleFontWeight;
    OxySize? maxSize1 = new OxySize?();
    rc2.DrawMathText(pt1, subtitle, actualColor1, actualSubtitleFont, subtitleFontSize, subtitleFontWeight, 0.0, HorizontalAlignment.Center, VerticalAlignment.Top, maxSize1);
  }

  private void UpdatePlotArea(IRenderContext rc)
  {
    OxyRect oxyRect;
    ref OxyRect local = ref oxyRect;
    OxyThickness padding = this.Padding;
    double left1 = padding.Left;
    padding = this.Padding;
    double top1 = padding.Top;
    double width1 = this.Width;
    padding = this.Padding;
    double left2 = padding.Left;
    double num1 = width1 - left2;
    padding = this.Padding;
    double right = padding.Right;
    double width2 = num1 - right;
    double height1 = this.Height;
    padding = this.Padding;
    double top2 = padding.Top;
    double num2 = height1 - top2;
    padding = this.Padding;
    double bottom = padding.Bottom;
    double height2 = num2 - bottom;
    local = new OxyRect(left1, top1, width2, height2);
    OxySize oxySize = this.MeasureTitles(rc);
    if (oxySize.Height > 0.0)
    {
      double num3 = oxySize.Height + this.TitlePadding;
      oxyRect = new OxyRect(oxyRect.Left, oxyRect.Top + num3, oxyRect.Width, oxyRect.Height - num3);
    }
    oxyRect = oxyRect.Deflate(this.ActualPlotMargins);
    double width3 = oxyRect.Width;
    double height3 = double.IsNaN(this.LegendMaxHeight) ? oxyRect.Height : Math.Min(oxyRect.Height, this.LegendMaxHeight);
    if (this.LegendPlacement == LegendPlacement.Inside)
    {
      width3 -= this.LegendMargin * 2.0;
      height3 -= this.LegendMargin * 2.0;
    }
    if (width3 < 0.0)
      width3 = 0.0;
    if (height3 < 0.0)
      height3 = 0.0;
    OxySize legendSize = this.MeasureLegends(rc, new OxySize(width3, height3));
    if (this.IsLegendVisible && this.LegendPlacement == LegendPlacement.Outside)
    {
      switch (this.LegendPosition)
      {
        case LegendPosition.TopLeft:
        case LegendPosition.TopCenter:
        case LegendPosition.TopRight:
          oxyRect = new OxyRect(oxyRect.Left, oxyRect.Top + legendSize.Height + this.LegendMargin, oxyRect.Width, oxyRect.Height - (legendSize.Height + this.LegendMargin));
          break;
        case LegendPosition.BottomLeft:
        case LegendPosition.BottomCenter:
        case LegendPosition.BottomRight:
          oxyRect = new OxyRect(oxyRect.Left, oxyRect.Top, oxyRect.Width, oxyRect.Height - (legendSize.Height + this.LegendMargin));
          break;
        case LegendPosition.LeftTop:
        case LegendPosition.LeftMiddle:
        case LegendPosition.LeftBottom:
          oxyRect = new OxyRect(oxyRect.Left + legendSize.Width + this.LegendMargin, oxyRect.Top, oxyRect.Width - (legendSize.Width + this.LegendMargin), oxyRect.Height);
          break;
        case LegendPosition.RightTop:
        case LegendPosition.RightMiddle:
        case LegendPosition.RightBottom:
          oxyRect = new OxyRect(oxyRect.Left, oxyRect.Top, oxyRect.Width - (legendSize.Width + this.LegendMargin), oxyRect.Height);
          break;
      }
    }
    if (oxyRect.Height < 0.0)
      oxyRect = new OxyRect(oxyRect.Left, oxyRect.Top, oxyRect.Width, 1.0);
    if (oxyRect.Width < 0.0)
      oxyRect = new OxyRect(oxyRect.Left, oxyRect.Top, 1.0, oxyRect.Height);
    this.PlotArea = oxyRect;
    this.PlotAndAxisArea = oxyRect.Inflate(this.ActualPlotMargins);
    if (this.TitleHorizontalAlignment == TitleHorizontalAlignment.CenteredWithinView)
    {
      this.TitleArea = new OxyRect(0.0, this.Padding.Top, this.Width, oxySize.Height + this.TitlePadding * 2.0);
    }
    else
    {
      OxyRect plotArea = this.PlotArea;
      double left3 = plotArea.Left;
      double top3 = this.Padding.Top;
      plotArea = this.PlotArea;
      double width4 = plotArea.Width;
      double height4 = oxySize.Height + this.TitlePadding * 2.0;
      this.TitleArea = new OxyRect(left3, top3, width4, height4);
    }
    this.LegendArea = this.GetLegendRectangle(legendSize);
  }
}
