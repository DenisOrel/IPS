// Decompiled with JetBrains decompiler
// Type: OxyPlot.XkcdRenderingDecorator
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OxyPlot;

public class XkcdRenderingDecorator : RenderContextBase
{
  private readonly IRenderContext rc;
  private readonly Random r = new Random(0);

  public XkcdRenderingDecorator(IRenderContext rc)
  {
    this.rc = rc;
    this.RendersToScreen = this.rc.RendersToScreen;
    this.DistortionFactor = 7.0;
    this.InterpolationDistance = 10.0;
    this.ThicknessScale = 2.0;
    this.FontFamily = "Humor Sans";
  }

  public double DistortionFactor { get; set; }

  public double InterpolationDistance { get; set; }

  public string FontFamily { get; set; }

  public double ThicknessScale { get; set; }

  public override void DrawLine(
    IList<ScreenPoint> points,
    OxyColor stroke,
    double thickness,
    double[] dashArray,
    LineJoin lineJoin,
    bool aliased)
  {
    this.rc.DrawLine((IList<ScreenPoint>) this.Distort((IEnumerable<ScreenPoint>) points), stroke, thickness * this.ThicknessScale, dashArray, lineJoin);
  }

  public override void DrawPolygon(
    IList<ScreenPoint> points,
    OxyColor fill,
    OxyColor stroke,
    double thickness,
    double[] dashArray,
    LineJoin lineJoin,
    bool aliased)
  {
    List<ScreenPoint> points1 = new List<ScreenPoint>((IEnumerable<ScreenPoint>) points);
    points1.Add(points1[0]);
    this.rc.DrawPolygon((IList<ScreenPoint>) this.Distort((IEnumerable<ScreenPoint>) points1), fill, stroke, thickness * this.ThicknessScale, dashArray, lineJoin);
  }

  public override void DrawText(
    ScreenPoint p,
    string text,
    OxyColor fill,
    string fontFamily,
    double fontSize,
    double fontWeight,
    double rotate,
    HorizontalAlignment halign,
    VerticalAlignment valign,
    OxySize? maxSize)
  {
    this.rc.DrawText(p, text, fill, this.GetFontFamily(fontFamily), fontSize, fontWeight, rotate, halign, valign, maxSize);
  }

  public override OxySize MeasureText(
    string text,
    string fontFamily,
    double fontSize,
    double fontWeight)
  {
    return this.rc.MeasureText(text, this.GetFontFamily(fontFamily), fontSize, fontWeight);
  }

  public override void SetToolTip(string text) => this.rc.SetToolTip(text);

  public override void CleanUp() => this.rc.CleanUp();

  public override void DrawImage(
    OxyImage source,
    double srcX,
    double srcY,
    double srcWidth,
    double srcHeight,
    double destX,
    double destY,
    double destWidth,
    double destHeight,
    double opacity,
    bool interpolate)
  {
    this.rc.DrawImage(source, srcX, srcY, srcWidth, srcHeight, destX, destY, destWidth, destHeight, opacity, interpolate);
  }

  public override bool SetClip(OxyRect clippingRect) => this.rc.SetClip(clippingRect);

  public override void ResetClip() => this.rc.ResetClip();

  private string GetFontFamily(string fontFamily) => this.FontFamily;

  private ScreenPoint[] Distort(IEnumerable<ScreenPoint> points)
  {
    IList<ScreenPoint> array = (IList<ScreenPoint>) this.Interpolate(points, this.InterpolationDistance).ToArray<ScreenPoint>();
    ScreenPoint[] screenPointArray = new ScreenPoint[array.Count];
    double[] numArray = this.ApplyMovingAverage((IList<double>) this.GenerateRandomNumbers(array.Count), 5);
    double distortionFactor = this.DistortionFactor;
    double num = distortionFactor / 2.0;
    for (int index = 0; index < array.Count; ++index)
    {
      if (index == 0 || index == array.Count - 1)
      {
        screenPointArray[index] = array[index];
      }
      else
      {
        ScreenVector screenVector1 = array[index + 1] - array[index - 1];
        screenVector1.Normalize();
        ScreenVector screenVector2 = new ScreenVector(screenVector1.Y, -screenVector1.X) * (numArray[index] * distortionFactor - num);
        screenPointArray[index] = array[index] + screenVector2;
      }
    }
    return screenPointArray;
  }

  private double[] GenerateRandomNumbers(int n)
  {
    double[] randomNumbers = new double[n];
    for (int index = 0; index < n; ++index)
      randomNumbers[index] = this.r.NextDouble();
    return randomNumbers;
  }

  private double[] ApplyMovingAverage(IList<double> input, int m)
  {
    int count = input.Count;
    double[] numArray = new double[count];
    int num1 = m / 2;
    for (int index1 = 0; index1 < count; ++index1)
    {
      int num2 = Math.Max(0, index1 - num1);
      int num3 = Math.Min(count - 1, index1 + num1);
      for (int index2 = num2; index2 <= num3; ++index2)
        numArray[index1] += input[index2];
      numArray[index1] /= (double) m;
    }
    return numArray;
  }

  private IEnumerable<ScreenPoint> Interpolate(IEnumerable<ScreenPoint> input, double dist)
  {
    ScreenPoint p0 = new ScreenPoint();
    double l = -1.0;
    double nl = dist;
    foreach (ScreenPoint p1 in input)
    {
      if (l < 0.0)
      {
        yield return p1;
        p0 = p1;
        l = 0.0;
      }
      else
      {
        double l1 = (p1 - p0).Length;
        if (l1 > 0.0)
        {
          for (; nl >= l && nl <= l + l1; nl += dist)
          {
            double num = (nl - l) / l1;
            yield return new ScreenPoint(p0.X * (1.0 - num) + p1.X * num, p0.Y * (1.0 - num) + p1.Y * num);
          }
        }
        l += l1;
        p0 = p1;
      }
    }
    yield return p0;
  }
}
