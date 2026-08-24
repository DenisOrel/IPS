// Decompiled with JetBrains decompiler
// Type: OxyPlot.CohenSutherlandClipping
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot;

public class CohenSutherlandClipping
{
  private const int Bottom = 4;
  private const int Inside = 0;
  private const int Left = 1;
  private const int Right = 2;
  private const int Top = 8;
  private readonly double xmax;
  private readonly double xmin;
  private readonly double ymax;
  private readonly double ymin;

  public CohenSutherlandClipping(OxyRect rect)
  {
    this.xmin = rect.Left;
    this.xmax = rect.Right;
    this.ymin = rect.Top;
    this.ymax = rect.Bottom;
  }

  public bool ClipLine(ref ScreenPoint p0, ref ScreenPoint p1)
  {
    int num1 = 0;
    if (p0.x < this.xmin)
      num1 |= 1;
    else if (p0.x > this.xmax)
      num1 |= 2;
    if (p0.y < this.ymin)
      num1 |= 4;
    else if (p0.y > this.ymax)
      num1 |= 8;
    int num2 = 0;
    if (p1.x < this.xmin)
      num2 |= 1;
    else if (p1.x > this.xmax)
      num2 |= 2;
    if (p1.y < this.ymin)
      num2 |= 4;
    else if (p1.y > this.ymax)
      num2 |= 8;
    bool flag = false;
    while ((num1 | num2) != 0)
    {
      if ((num1 & num2) == 0)
      {
        double num3 = 0.0;
        double num4 = 0.0;
        int num5 = num1 != 0 ? num1 : num2;
        if ((num5 & 8) != 0)
        {
          num3 = p0.x + (p1.x - p0.x) * (this.ymax - p0.y) / (p1.y - p0.y);
          num4 = this.ymax;
        }
        else if ((num5 & 4) != 0)
        {
          num3 = p0.x + (p1.x - p0.x) * (this.ymin - p0.y) / (p1.y - p0.y);
          num4 = this.ymin;
        }
        else if ((num5 & 2) != 0)
        {
          num4 = p0.y + (p1.y - p0.y) * (this.xmax - p0.x) / (p1.x - p0.x);
          num3 = this.xmax;
        }
        else if ((num5 & 1) != 0)
        {
          num4 = p0.y + (p1.y - p0.y) * (this.xmin - p0.x) / (p1.x - p0.x);
          num3 = this.xmin;
        }
        if (num5 == num1)
        {
          p0.x = num3;
          p0.y = num4;
          num1 = 0;
          if (p0.x < this.xmin)
            num1 |= 1;
          else if (p0.x > this.xmax)
            num1 |= 2;
          if (p0.y < this.ymin)
            num1 |= 4;
          else if (p0.y > this.ymax)
            num1 |= 8;
        }
        else
        {
          p1.x = num3;
          p1.y = num4;
          num2 = 0;
          if (p1.x < this.xmin)
            num2 |= 1;
          else if (p1.x > this.xmax)
            num2 |= 2;
          if (p1.y < this.ymin)
            num2 |= 4;
          else if (p1.y > this.ymax)
            num2 |= 8;
        }
      }
      else
        goto label_45;
    }
    flag = true;
label_45:
    return flag;
  }

  public bool IsInside(ScreenPoint s)
  {
    return s.x >= this.xmin && s.x <= this.xmax && s.y >= this.ymin && s.y <= this.ymax;
  }
}
