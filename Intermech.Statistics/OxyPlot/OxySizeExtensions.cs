// Decompiled with JetBrains decompiler
// Type: OxyPlot.OxySizeExtensions
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot;

public static class OxySizeExtensions
{
  public static OxyRect GetBounds(
    this OxySize bounds,
    double angle,
    HorizontalAlignment horizontalAlignment,
    VerticalAlignment verticalAlignment)
  {
    double num1;
    switch (horizontalAlignment)
    {
      case HorizontalAlignment.Left:
        num1 = 0.0;
        break;
      case HorizontalAlignment.Center:
        num1 = 0.5;
        break;
      default:
        num1 = 1.0;
        break;
    }
    double num2 = num1;
    double num3;
    switch (verticalAlignment)
    {
      case VerticalAlignment.Top:
        num3 = 0.0;
        break;
      case VerticalAlignment.Middle:
        num3 = 0.5;
        break;
      default:
        num3 = 1.0;
        break;
    }
    double num4 = num3;
    ScreenVector screenVector1 = new ScreenVector(num2 * bounds.Width, num4 * bounds.Height);
    if (angle == 0.0)
      return new OxyRect(-screenVector1.X, -screenVector1.Y, bounds.Width, bounds.Height);
    ScreenVector screenVector2 = new ScreenVector(0.0, 0.0) - screenVector1;
    ScreenVector screenVector3 = new ScreenVector(bounds.Width, 0.0) - screenVector1;
    ScreenVector screenVector4 = new ScreenVector(bounds.Width, bounds.Height) - screenVector1;
    ScreenVector screenVector5 = new ScreenVector(0.0, bounds.Height) - screenVector1;
    double num5 = angle * Math.PI / 180.0;
    double costh = Math.Cos(num5);
    double sinth = Math.Sin(num5);
    Func<ScreenVector, ScreenVector> func = (Func<ScreenVector, ScreenVector>) (p => new ScreenVector(costh * p.X - sinth * p.Y, sinth * p.X + costh * p.Y));
    ScreenVector screenVector6 = func(screenVector2);
    ScreenVector screenVector7 = func(screenVector3);
    ScreenVector screenVector8 = func(screenVector4);
    ScreenVector screenVector9 = func(screenVector5);
    double left = Math.Min(Math.Min(screenVector6.X, screenVector7.X), Math.Min(screenVector8.X, screenVector9.X));
    double top = Math.Min(Math.Min(screenVector6.Y, screenVector7.Y), Math.Min(screenVector8.Y, screenVector9.Y));
    double width = Math.Max(Math.Max(screenVector6.X - left, screenVector7.X - left), Math.Max(screenVector8.X - left, screenVector9.X - left));
    double height = Math.Max(Math.Max(screenVector6.Y - top, screenVector7.Y - top), Math.Max(screenVector8.Y - top, screenVector9.Y - top));
    return new OxyRect(left, top, width, height);
  }

  public static IEnumerable<ScreenPoint> GetPolygon(
    this OxySize size,
    ScreenPoint origin,
    double angle,
    HorizontalAlignment horizontalAlignment,
    VerticalAlignment verticalAlignment)
  {
    ScreenVector screenVector1 = new ScreenVector((horizontalAlignment == HorizontalAlignment.Left ? 0.0 : (horizontalAlignment == HorizontalAlignment.Center ? 0.5 : 1.0)) * size.Width, (verticalAlignment == VerticalAlignment.Top ? 0.0 : (verticalAlignment == VerticalAlignment.Middle ? 0.5 : 1.0)) * size.Height);
    ScreenVector screenVector2 = new ScreenVector(0.0, 0.0) - screenVector1;
    ScreenVector p1 = new ScreenVector(size.Width, 0.0) - screenVector1;
    ScreenVector p2 = new ScreenVector(size.Width, size.Height) - screenVector1;
    ScreenVector p3 = new ScreenVector(0.0, size.Height) - screenVector1;
    if (angle != 0.0)
    {
      double num = angle * Math.PI / 180.0;
      double costh = Math.Cos(num);
      double sinth = Math.Sin(num);
      Func<ScreenVector, ScreenVector> func = (Func<ScreenVector, ScreenVector>) (p => new ScreenVector(costh * p.X - sinth * p.Y, sinth * p.X + costh * p.Y));
      screenVector2 = func(screenVector2);
      p1 = func(p1);
      p2 = func(p2);
      p3 = func(p3);
    }
    yield return origin + screenVector2;
    yield return origin + p1;
    yield return origin + p2;
    yield return origin + p3;
  }
}
