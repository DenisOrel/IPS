// Decompiled with JetBrains decompiler
// Type: OxyPlot.HitTestResult
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot;

public class HitTestResult
{
  public HitTestResult(UIElement element, ScreenPoint nearestHitPoint, object item = null, double index = 0.0)
  {
    this.Element = element;
    this.NearestHitPoint = nearestHitPoint;
    this.Item = item;
    this.Index = index;
  }

  public double Index { get; private set; }

  public object Item { get; private set; }

  public UIElement Element { get; private set; }

  public ScreenPoint NearestHitPoint { get; private set; }
}
