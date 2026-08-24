// Decompiled with JetBrains decompiler
// Type: OxyPlot.Conrec
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;

#nullable disable
namespace OxyPlot;

public static class Conrec
{
  public static void Contour(
    double[,] d,
    double[] x,
    double[] y,
    double[] z,
    Conrec.RendererDelegate renderer)
  {
    double x1 = 0.0;
    double x2 = 0.0;
    double y1 = 0.0;
    double y2 = 0.0;
    double[] h = new double[5];
    int[] numArray1 = new int[5];
    double[] xh = new double[5];
    double[] yh = new double[5];
    int lowerBound1 = d.GetLowerBound(0);
    int upperBound1 = d.GetUpperBound(0);
    int lowerBound2 = d.GetLowerBound(1);
    int upperBound2 = d.GetUpperBound(1);
    int length = z.Length;
    int[] numArray2 = new int[4]{ 0, 1, 1, 0 };
    int[] numArray3 = new int[4]{ 0, 0, 1, 1 };
    int[,,] numArray4 = new int[3, 3, 3]
    {
      {
        {
          0,
          0,
          8
        },
        {
          0,
          2,
          5
        },
        {
          7,
          6,
          9
        }
      },
      {
        {
          0,
          3,
          4
        },
        {
          1,
          3,
          1
        },
        {
          4,
          3,
          0
        }
      },
      {
        {
          9,
          6,
          7
        },
        {
          5,
          2,
          0
        },
        {
          8,
          0,
          0
        }
      }
    };
    Func<int, int, double> func1 = (Func<int, int, double>) ((p1, p2) => (h[p2] * xh[p1] - h[p1] * xh[p2]) / (h[p2] - h[p1]));
    Func<int, int, double> func2 = (Func<int, int, double>) ((p1, p2) => (h[p2] * yh[p1] - h[p1] * yh[p2]) / (h[p2] - h[p1]));
    for (int index1 = upperBound2 - 1; index1 >= lowerBound2; --index1)
    {
      for (int index2 = lowerBound1; index2 <= upperBound1 - 1; ++index2)
      {
        double num1 = Math.Min(Math.Min(d[index2, index1], d[index2, index1 + 1]), Math.Min(d[index2 + 1, index1], d[index2 + 1, index1 + 1]));
        double num2 = Math.Max(Math.Max(d[index2, index1], d[index2, index1 + 1]), Math.Max(d[index2 + 1, index1], d[index2 + 1, index1 + 1]));
        if (num2 >= z[0] && num1 <= z[length - 1])
        {
          for (int index3 = 0; index3 < length; ++index3)
          {
            if (z[index3] >= num1 && z[index3] <= num2)
            {
              for (int index4 = 4; index4 >= 0; --index4)
              {
                if (index4 > 0)
                {
                  h[index4] = d[index2 + numArray2[index4 - 1], index1 + numArray3[index4 - 1]] - z[index3];
                  xh[index4] = x[index2 + numArray2[index4 - 1]];
                  yh[index4] = y[index1 + numArray3[index4 - 1]];
                }
                else
                {
                  h[0] = 0.25 * (h[1] + h[2] + h[3] + h[4]);
                  xh[0] = 0.5 * (x[index2] + x[index2 + 1]);
                  yh[0] = 0.5 * (y[index1] + y[index1 + 1]);
                }
                numArray1[index4] = h[index4] <= 0.0 ? (h[index4] >= 0.0 ? 0 : -1) : 1;
              }
              for (int index5 = 1; index5 <= 4; ++index5)
              {
                int index6 = index5;
                int index7 = 0;
                int index8 = index5 == 4 ? 1 : index5 + 1;
                switch (numArray4[numArray1[index6] + 1, numArray1[index7] + 1, numArray1[index8] + 1])
                {
                  case 0:
                    continue;
                  case 1:
                    x1 = xh[index6];
                    y1 = yh[index6];
                    x2 = xh[index7];
                    y2 = yh[index7];
                    break;
                  case 2:
                    x1 = xh[index7];
                    y1 = yh[index7];
                    x2 = xh[index8];
                    y2 = yh[index8];
                    break;
                  case 3:
                    x1 = xh[index8];
                    y1 = yh[index8];
                    x2 = xh[index6];
                    y2 = yh[index6];
                    break;
                  case 4:
                    x1 = xh[index6];
                    y1 = yh[index6];
                    x2 = func1(index7, index8);
                    y2 = func2(index7, index8);
                    break;
                  case 5:
                    x1 = xh[index7];
                    y1 = yh[index7];
                    x2 = func1(index8, index6);
                    y2 = func2(index8, index6);
                    break;
                  case 6:
                    x1 = xh[index8];
                    y1 = yh[index8];
                    x2 = func1(index6, index7);
                    y2 = func2(index6, index7);
                    break;
                  case 7:
                    x1 = func1(index6, index7);
                    y1 = func2(index6, index7);
                    x2 = func1(index7, index8);
                    y2 = func2(index7, index8);
                    break;
                  case 8:
                    x1 = func1(index7, index8);
                    y1 = func2(index7, index8);
                    x2 = func1(index8, index6);
                    y2 = func2(index8, index6);
                    break;
                  case 9:
                    x1 = func1(index8, index6);
                    y1 = func2(index8, index6);
                    x2 = func1(index6, index7);
                    y2 = func2(index6, index7);
                    break;
                }
                renderer(x1, y1, x2, y2, z[index3]);
              }
            }
          }
        }
      }
    }
  }

  public delegate void RendererDelegate(double x1, double y1, double x2, double y2, double z);
}
