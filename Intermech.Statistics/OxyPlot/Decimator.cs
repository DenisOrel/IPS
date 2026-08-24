// Decompiled with JetBrains decompiler
// Type: OxyPlot.Decimator
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot;

public class Decimator
{
  public static void Decimate(List<ScreenPoint> input, List<ScreenPoint> output)
  {
    if (input == null || input.Count == 0)
      return;
    ScreenPoint screenPoint = input[0];
    double x = Math.Round(screenPoint.X);
    double minY = Math.Round(screenPoint.Y);
    double maxY = minY;
    double firstY = minY;
    double lastY1 = minY;
    for (int index = 1; index < input.Count; ++index)
    {
      screenPoint = input[index];
      double num1 = Math.Round(screenPoint.X);
      double num2 = Math.Round(screenPoint.Y);
      if (num1 != x)
      {
        Decimator.AddVerticalPoints(output, x, firstY, lastY1, minY, maxY);
        double num3;
        maxY = num3 = num2;
        minY = num3;
        lastY1 = num3;
        firstY = num3;
        x = num1;
      }
      else
      {
        if (num2 < minY)
          minY = num2;
        if (num2 > maxY)
          maxY = num2;
        lastY1 = num2;
      }
    }
    double lastY2 = firstY == minY ? maxY : minY;
    Decimator.AddVerticalPoints(output, x, firstY, lastY2, minY, maxY);
  }

  private static void AddVerticalPoints(
    List<ScreenPoint> result,
    double x,
    double firstY,
    double lastY,
    double minY,
    double maxY)
  {
    result.Add(new ScreenPoint(x, firstY));
    if (firstY == minY)
    {
      if (minY != maxY)
        result.Add(new ScreenPoint(x, maxY));
      if (maxY == lastY)
        return;
      result.Add(new ScreenPoint(x, lastY));
    }
    else if (firstY == maxY)
    {
      if (maxY != minY)
        result.Add(new ScreenPoint(x, minY));
      if (minY == lastY)
        return;
      result.Add(new ScreenPoint(x, lastY));
    }
    else
    {
      if (lastY == minY)
      {
        if (minY != maxY)
          result.Add(new ScreenPoint(x, maxY));
      }
      else if (lastY == maxY)
      {
        if (maxY != minY)
          result.Add(new ScreenPoint(x, minY));
      }
      else
      {
        result.Add(new ScreenPoint(x, minY));
        result.Add(new ScreenPoint(x, maxY));
      }
      result.Add(new ScreenPoint(x, lastY));
    }
  }
}
