// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.HeatMapSeries
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Axes;
using System;

#nullable disable
namespace OxyPlot.Series;

public class HeatMapSeries : XYAxisSeries
{
  public new const string DefaultTrackerFormatString = "{0}\n{1}: {2}\n{3}: {4}\n{5}: {6}";
  private const string DefaultColorAxisTitle = "Value";
  private int dataHash;
  private int colorAxisHash;
  private OxyImage image;

  public HeatMapSeries()
  {
    this.TrackerFormatString = "{0}\n{1}: {2}\n{3}: {4}\n{5}: {6}";
    this.Interpolate = true;
    this.LabelFormatString = "0.00";
    this.LabelFontSize = 0.0;
  }

  public double X0 { get; set; }

  public double X1 { get; set; }

  public double Y0 { get; set; }

  public double Y1 { get; set; }

  public double[,] Data { get; set; }

  public bool Interpolate { get; set; }

  public double MinValue { get; private set; }

  public double MaxValue { get; private set; }

  public IColorAxis ColorAxis { get; protected set; }

  public string ColorAxisKey { get; set; }

  public HeatMapCoordinateDefinition CoordinateDefinition { get; set; }

  public string LabelFormatString { get; set; }

  public double LabelFontSize { get; set; }

  public void Invalidate() => this.image = (OxyImage) null;

  public override void Render(IRenderContext rc)
  {
    if (this.Data == null)
    {
      this.image = (OxyImage) null;
    }
    else
    {
      if (this.ColorAxis == null)
        throw new InvalidOperationException("Color axis not specified.");
      double x0 = this.X0;
      double x1 = this.X1;
      double y0 = this.Y0;
      double y1 = this.Y1;
      int length1 = this.Data.GetLength(0);
      int length2 = this.Data.GetLength(1);
      double num1 = (this.X1 - this.X0) / (double) (length1 - 1);
      double num2 = (this.Y1 - this.Y0) / (double) (length2 - 1);
      if (this.CoordinateDefinition == HeatMapCoordinateDefinition.Center)
      {
        x0 -= num1 / 2.0;
        x1 += num1 / 2.0;
        y0 -= num2 / 2.0;
        y1 += num2 / 2.0;
      }
      OxyRect rect = new OxyRect(this.Transform(x0, y0), this.Transform(x1, y1));
      int hashCode = this.Data.GetHashCode();
      int elementHashCode = this.ColorAxis.GetElementHashCode();
      if (this.image == null || hashCode != this.dataHash || elementHashCode != this.colorAxisHash)
      {
        this.UpdateImage();
        this.dataHash = hashCode;
        this.colorAxisHash = elementHashCode;
      }
      OxyRect clippingRect = this.GetClippingRect();
      if (this.image != null)
        rc.DrawClippedImage(clippingRect, this.image, rect.Left, rect.Top, rect.Width, rect.Height, 1.0, this.Interpolate);
      if (this.LabelFontSize <= 0.0)
        return;
      this.RenderLabels(rc, rect);
    }
  }

  public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
  {
    if (!this.Interpolate)
      interpolate = false;
    DataPoint p = this.InverseTransform(point);
    if (!this.IsPointInRange(p))
      return (TrackerHitResult) null;
    double num1 = (this.X1 - this.X0) / (double) (this.Data.GetLength(0) - 1);
    double num2 = (this.Y1 - this.Y0) / (double) (this.Data.GetLength(1) - 1);
    double num3 = (p.X - this.X0) / num1;
    double num4 = (p.Y - this.Y0) / num2;
    if (!interpolate)
    {
      num3 = Math.Round(num3);
      num4 = Math.Round(num4);
      p = new DataPoint(num3 * num1 + this.X0, num4 * num2 + this.Y0);
      point = this.Transform(p);
    }
    double num5 = HeatMapSeries.GetValue(this.Data, num3, num4);
    string str = (this.ColorAxis is Axis colorAxis ? colorAxis.Title : (string) null) ?? "Value";
    return new TrackerHitResult()
    {
      Series = (OxyPlot.Series.Series) this,
      DataPoint = p,
      Position = point,
      Item = (object) null,
      Index = -1.0,
      Text = StringHelper.Format((IFormatProvider) this.ActualCulture, this.TrackerFormatString, (object) null, (object) this.Title, (object) (this.XAxis.Title ?? "X"), this.XAxis.GetValue(p.X), (object) (this.YAxis.Title ?? "Y"), this.YAxis.GetValue(p.Y), (object) str, (object) num5)
    };
  }

  protected internal override void EnsureAxes()
  {
    base.EnsureAxes();
    this.ColorAxis = this.PlotModel.GetAxisOrDefault(this.ColorAxisKey, (Axis) this.PlotModel.DefaultColorAxis) as IColorAxis;
  }

  protected internal override void UpdateMaxMin()
  {
    base.UpdateMaxMin();
    int length1 = this.Data.GetLength(0);
    int length2 = this.Data.GetLength(1);
    this.MinX = Math.Min(this.X0, this.X1);
    this.MaxX = Math.Max(this.X0, this.X1);
    this.MinY = Math.Min(this.Y0, this.Y1);
    this.MaxY = Math.Max(this.Y0, this.Y1);
    if (this.CoordinateDefinition == HeatMapCoordinateDefinition.Center)
    {
      double num1 = Math.Abs(this.X1 - this.X0) / (double) (length1 - 1);
      double num2 = Math.Abs(this.Y1 - this.Y0) / (double) (length2 - 1);
      this.MinX -= num1 / 2.0;
      this.MaxX += num1 / 2.0;
      this.MinY -= num2 / 2.0;
      this.MaxY += num2 / 2.0;
    }
    this.MinValue = this.Data.Min2D(true);
    this.MaxValue = this.Data.Max2D();
  }

  protected internal override void UpdateAxisMaxMin()
  {
    base.UpdateAxisMaxMin();
    if (!(this.ColorAxis is Axis colorAxis))
      return;
    colorAxis.Include(this.MinValue);
    colorAxis.Include(this.MaxValue);
  }

  protected virtual void RenderLabels(IRenderContext rc, OxyRect rect)
  {
    OxyRect clippingRect = this.GetClippingRect();
    int length1 = this.Data.GetLength(0);
    int length2 = this.Data.GetLength(1);
    double num1 = (this.X1 - this.X0) / (double) (length1 - 1);
    double num2 = (this.Y1 - this.Y0) / (double) (length2 - 1);
    double fontSize = rect.Height / (double) length2 * this.LabelFontSize;
    for (int i = 0; i < length1; ++i)
    {
      for (int j = 0; j < length2; ++j)
      {
        ScreenPoint p = this.Transform(new DataPoint((double) i * num1 + this.X0, (double) j * num2 + this.Y0));
        double v = HeatMapSeries.GetValue(this.Data, (double) i, (double) j);
        OxyColor fill = this.ColorAxis.GetColor(v).ToHsv()[2] > 0.6 ? OxyColors.Black : OxyColors.White;
        string label = this.GetLabel(v, i, j);
        rc.DrawClippedText(clippingRect, p, label, fill, this.ActualFont, fontSize, horizontalAlignment: HorizontalAlignment.Center, verticalAlignment: VerticalAlignment.Middle);
      }
    }
  }

  protected virtual string GetLabel(double v, int i, int j)
  {
    return v.ToString(this.LabelFormatString, (IFormatProvider) this.ActualCulture);
  }

  private static double GetValue(double[,] data, double i, double j)
  {
    i = Math.Max(i, 0.0);
    j = Math.Max(j, 0.0);
    int index1 = (int) i;
    int val2_1 = index1 + 1 < data.GetLength(0) ? index1 + 1 : index1;
    int index2 = (int) j;
    int val2_2 = index2 + 1 < data.GetLength(1) ? index2 + 1 : index2;
    i = Math.Min(i, (double) val2_1);
    j = Math.Min(j, (double) val2_2);
    if (i == (double) index1 && j == (double) index2)
      return data[index1, index2];
    if (i != (double) index1 && j == (double) index2)
    {
      if (double.IsNaN(data[index1, index2]) || double.IsNaN(data[val2_1, index2]))
        return double.NaN;
      double num = i - (double) index1;
      return index1 != val2_1 ? data[index1, index2] * (1.0 - num) + data[val2_1, index2] * num : data[index1, index2];
    }
    if (i == (double) index1 && j != (double) index2)
    {
      if (double.IsNaN(data[index1, index2]) || double.IsNaN(data[index1, val2_2]))
        return double.NaN;
      double num = j - (double) index2;
      return index2 != val2_2 ? data[index1, index2] * (1.0 - num) + data[index1, val2_2] * num : data[index1, index2];
    }
    if (double.IsNaN(data[index1, index2]) || double.IsNaN(data[val2_1, index2]) || double.IsNaN(data[index1, val2_2]) || double.IsNaN(data[val2_1, val2_2]))
      return double.NaN;
    double num1 = i - (double) index1;
    double num2 = j - (double) index2;
    double num3;
    double num4;
    if (index1 != val2_1)
    {
      num3 = data[index1, index2] * (1.0 - num1) + data[val2_1, index2] * num1;
      num4 = data[index1, val2_2] * (1.0 - num1) + data[val2_1, val2_2] * num1;
    }
    else
    {
      num3 = data[index1, index2];
      num4 = data[index1, val2_2];
    }
    return index2 != val2_2 ? num3 * (1.0 - num2) + num4 * num2 : num3;
  }

  private bool IsPointInRange(DataPoint p)
  {
    double x0 = this.X0;
    double x1 = this.X1;
    double y0 = this.Y0;
    double y1 = this.Y1;
    if (this.CoordinateDefinition == HeatMapCoordinateDefinition.Center)
    {
      double num1 = (this.X1 - this.X0) / (double) (this.Data.GetLength(0) - 1);
      double num2 = (this.Y1 - this.Y0) / (double) (this.Data.GetLength(1) - 1);
      x0 -= num1 / 2.0;
      x1 += num1 / 2.0;
      y0 -= num2 / 2.0;
      y1 += num2 / 2.0;
    }
    return p.X >= x0 && p.X <= x1 && p.Y >= y0 && p.Y <= y1;
  }

  private void UpdateImage()
  {
    bool flag1 = this.XAxis.Transform(this.X0) > this.XAxis.Transform(this.X1);
    bool flag2 = this.YAxis.Transform(this.Y0) > this.YAxis.Transform(this.Y1);
    int length1 = this.Data.GetLength(0);
    int length2 = this.Data.GetLength(1);
    OxyColor[,] pixels = new OxyColor[length1, length2];
    for (int index1 = 0; index1 < length1; ++index1)
    {
      int index2 = flag1 ? length1 - 1 - index1 : index1;
      for (int index3 = 0; index3 < length2; ++index3)
      {
        int index4 = flag2 ? length2 - 1 - index3 : index3;
        pixels[index1, index3] = this.ColorAxis.GetColor(this.Data[index2, index4]);
      }
    }
    this.image = OxyImage.Create(pixels, ImageFormat.Png);
  }
}
