// Decompiled with JetBrains decompiler
// Type: OxyPlot.Annotations.PolylineAnnotation
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OxyPlot.Annotations;

public class PolylineAnnotation : PathAnnotation
{
  private readonly List<DataPoint> points = new List<DataPoint>();

  public List<DataPoint> Points => this.points;

  public double MinimumSegmentLength { get; set; }

  public bool Smooth { get; set; }

  protected override IList<ScreenPoint> GetScreenPoints()
  {
    List<ScreenPoint> list = this.Points.Select<DataPoint, ScreenPoint>(new Func<DataPoint, ScreenPoint>(((Annotation) this).Transform)).ToList<ScreenPoint>();
    return this.Smooth ? (IList<ScreenPoint>) CanonicalSplineHelper.CreateSpline(ScreenPointHelper.ResamplePoints((IList<ScreenPoint>) list, this.MinimumSegmentLength), 0.5, (IList<double>) null, false, 0.25) : (IList<ScreenPoint>) this.Points.Select<DataPoint, ScreenPoint>(new Func<DataPoint, ScreenPoint>(((Annotation) this).Transform)).ToList<ScreenPoint>();
  }
}
