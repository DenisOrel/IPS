// Decompiled with JetBrains decompiler
// Type: OxyPlot.TrackerManipulator
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot;

public class TrackerManipulator : MouseManipulator
{
  private OxyPlot.Series.Series currentSeries;

  public TrackerManipulator(IPlotView plotView)
    : base(plotView)
  {
    this.Snap = true;
    this.PointsOnly = false;
    this.LockToInitialSeries = true;
  }

  public bool PointsOnly { get; set; }

  public bool Snap { get; set; }

  public bool LockToInitialSeries { get; set; }

  public override void Completed(OxyMouseEventArgs e)
  {
    base.Completed(e);
    e.Handled = true;
    this.currentSeries = (OxyPlot.Series.Series) null;
    this.PlotView.HideTracker();
    if (this.PlotView.ActualModel == null)
      return;
    this.PlotView.ActualModel.OnTrackerChanged((TrackerHitResult) null);
  }

  public override void Delta(OxyMouseEventArgs e)
  {
    base.Delta(e);
    e.Handled = true;
    if (this.currentSeries == null || !this.LockToInitialSeries)
      this.currentSeries = this.PlotView.ActualModel != null ? this.PlotView.ActualModel.GetSeriesFromPoint(e.Position, 20.0) : (OxyPlot.Series.Series) null;
    if (this.currentSeries == null)
    {
      if (this.LockToInitialSeries)
        return;
      this.PlotView.HideTracker();
    }
    else
    {
      PlotModel actualModel = this.PlotView.ActualModel;
      if (actualModel == null || !actualModel.PlotArea.Contains(e.Position.X, e.Position.Y))
        return;
      TrackerHitResult nearestHit = TrackerManipulator.GetNearestHit(this.currentSeries, e.Position, this.Snap, this.PointsOnly);
      if (nearestHit == null)
        return;
      nearestHit.PlotModel = this.PlotView.ActualModel;
      this.PlotView.ShowTracker(nearestHit);
      this.PlotView.ActualModel.OnTrackerChanged(nearestHit);
    }
  }

  public override void Started(OxyMouseEventArgs e)
  {
    base.Started(e);
    this.currentSeries = this.PlotView.ActualModel != null ? this.PlotView.ActualModel.GetSeriesFromPoint(e.Position) : (OxyPlot.Series.Series) null;
    this.Delta(e);
  }

  private static TrackerHitResult GetNearestHit(
    OxyPlot.Series.Series series,
    ScreenPoint point,
    bool snap,
    bool pointsOnly)
  {
    if (series == null)
      return (TrackerHitResult) null;
    if (snap | pointsOnly)
    {
      TrackerHitResult nearestPoint = series.GetNearestPoint(point, false);
      if (nearestPoint != null && nearestPoint.Position.DistanceTo(point) < 20.0)
        return nearestPoint;
    }
    return !pointsOnly ? series.GetNearestPoint(point, true) : (TrackerHitResult) null;
  }
}
