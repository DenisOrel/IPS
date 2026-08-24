// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.Series
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Axes;

#nullable disable
namespace OxyPlot.Series;

public abstract class Series : PlotElement
{
  protected Series()
  {
    this.IsVisible = true;
    this.Background = OxyColors.Undefined;
  }

  public OxyColor Background { get; set; }

  public bool IsVisible { get; set; }

  public string Title { get; set; }

  public string TrackerFormatString { get; set; }

  public string TrackerKey { get; set; }

  public virtual TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
  {
    return (TrackerHitResult) null;
  }

  public abstract void Render(IRenderContext rc);

  public abstract void RenderLegend(IRenderContext rc, OxyRect legendBox);

  protected internal abstract bool AreAxesRequired();

  protected internal abstract void EnsureAxes();

  protected internal abstract bool IsUsing(Axis axis);

  protected internal abstract void SetDefaultValues();

  protected internal abstract void UpdateAxisMaxMin();

  protected internal abstract void UpdateData();

  protected internal abstract void UpdateValidData();

  protected internal abstract void UpdateMaxMin();

  protected override HitTestResult HitTestOverride(HitTestArguments args)
  {
    TrackerHitResult trackerHitResult = this.GetNearestPoint(args.Point, true) ?? this.GetNearestPoint(args.Point, false);
    if (trackerHitResult == null)
      return (HitTestResult) null;
    return trackerHitResult.Position.DistanceTo(args.Point) > args.Tolerance ? (HitTestResult) null : new HitTestResult((UIElement) this, trackerHitResult.Position, trackerHitResult.Item, trackerHitResult.Index);
  }
}
