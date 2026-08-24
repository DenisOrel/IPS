// Decompiled with JetBrains decompiler
// Type: OxyPlot.OxyTouchEventArgs
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot;

public class OxyTouchEventArgs : OxyInputEventArgs
{
  public OxyTouchEventArgs()
  {
  }

  public OxyTouchEventArgs(ScreenPoint[] currentTouches, ScreenPoint[] previousTouches)
  {
    this.Position = currentTouches[0];
    if (currentTouches.Length == previousTouches.Length)
      this.DeltaTranslation = currentTouches[0] - previousTouches[0];
    double num = 1.0;
    if (currentTouches.Length > 1 && currentTouches.Length == previousTouches.Length)
    {
      num = (currentTouches[1] - currentTouches[0]).Length / (previousTouches[1] - previousTouches[0]).Length;
      if (num < 0.5)
        num = 0.5;
      if (num > 2.0)
        num = 2.0;
    }
    this.DeltaScale = new ScreenVector(num, num);
  }

  public ScreenPoint Position { get; set; }

  public ScreenVector DeltaScale { get; set; }

  public ScreenVector DeltaTranslation { get; set; }
}
