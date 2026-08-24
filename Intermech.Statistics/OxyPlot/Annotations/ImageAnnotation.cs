// Decompiled with JetBrains decompiler
// Type: OxyPlot.Annotations.ImageAnnotation
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;

#nullable disable
namespace OxyPlot.Annotations;

public class ImageAnnotation : Annotation
{
  private OxyRect actualBounds;

  public ImageAnnotation()
  {
    this.X = new PlotLength(0.5, PlotLengthUnit.RelativeToPlotArea);
    this.Y = new PlotLength(0.5, PlotLengthUnit.RelativeToPlotArea);
    this.OffsetX = new PlotLength(0.0, PlotLengthUnit.ScreenUnits);
    this.OffsetY = new PlotLength(0.0, PlotLengthUnit.ScreenUnits);
    this.Width = new PlotLength(double.NaN, PlotLengthUnit.ScreenUnits);
    this.Height = new PlotLength(double.NaN, PlotLengthUnit.ScreenUnits);
    this.Opacity = 1.0;
    this.Interpolate = true;
    this.HorizontalAlignment = HorizontalAlignment.Center;
    this.VerticalAlignment = VerticalAlignment.Middle;
  }

  public OxyImage ImageSource { get; set; }

  public HorizontalAlignment HorizontalAlignment { get; set; }

  public PlotLength X { get; set; }

  public PlotLength Y { get; set; }

  public PlotLength OffsetX { get; set; }

  public PlotLength OffsetY { get; set; }

  public PlotLength Width { get; set; }

  public PlotLength Height { get; set; }

  public double Opacity { get; set; }

  public bool Interpolate { get; set; }

  public VerticalAlignment VerticalAlignment { get; set; }

  public override void Render(IRenderContext rc)
  {
    base.Render(rc);
    ScreenPoint screenPoint = this.GetPoint(this.X, this.Y, rc, this.PlotModel) + this.GetVector(this.OffsetX, this.OffsetY, rc, this.PlotModel);
    OxyRect clippingRect = this.GetClippingRect();
    ScreenVector vector = this.GetVector(this.Width, this.Height, rc, this.PlotModel);
    double d1 = vector.X;
    double d2 = vector.Y;
    if (double.IsNaN(d1) && double.IsNaN(d2))
    {
      d1 = (double) this.ImageSource.Width;
      d2 = (double) this.ImageSource.Height;
    }
    if (double.IsNaN(d1))
      d1 = d2 / (double) this.ImageSource.Height * (double) this.ImageSource.Width;
    if (double.IsNaN(d2))
      d2 = d1 / (double) this.ImageSource.Width * (double) this.ImageSource.Height;
    double num1 = Math.Abs(d1);
    double num2 = Math.Abs(d2);
    double x = screenPoint.X;
    double y = screenPoint.Y;
    if (this.HorizontalAlignment == HorizontalAlignment.Center)
      x -= num1 * 0.5;
    if (this.HorizontalAlignment == HorizontalAlignment.Right)
      x -= num1;
    if (this.VerticalAlignment == VerticalAlignment.Middle)
      y -= num2 * 0.5;
    if (this.VerticalAlignment == VerticalAlignment.Bottom)
      y -= num2;
    this.actualBounds = new OxyRect(x, y, num1, num2);
    if (this.X.Unit == PlotLengthUnit.Data || this.Y.Unit == PlotLengthUnit.Data)
      rc.DrawClippedImage(clippingRect, this.ImageSource, x, y, num1, num2, this.Opacity, this.Interpolate);
    else
      rc.DrawImage(this.ImageSource, x, y, num1, num2, this.Opacity, this.Interpolate);
  }

  protected override HitTestResult HitTestOverride(HitTestArguments args)
  {
    return this.actualBounds.Contains(args.Point) ? new HitTestResult((UIElement) this, args.Point) : (HitTestResult) null;
  }

  protected ScreenPoint GetPoint(PlotLength x, PlotLength y, IRenderContext rc, PlotModel model)
  {
    if (x.Unit == PlotLengthUnit.Data || y.Unit == PlotLengthUnit.Data)
      return this.XAxis.Transform(x.Value, y.Value, this.YAxis);
    double x1;
    switch (x.Unit)
    {
      case PlotLengthUnit.RelativeToViewport:
        x1 = model.Width * x.Value;
        break;
      case PlotLengthUnit.RelativeToPlotArea:
        x1 = model.PlotArea.Left + model.PlotArea.Width * x.Value;
        break;
      default:
        x1 = x.Value;
        break;
    }
    double y1;
    switch (y.Unit)
    {
      case PlotLengthUnit.RelativeToViewport:
        y1 = model.Height * y.Value;
        break;
      case PlotLengthUnit.RelativeToPlotArea:
        y1 = model.PlotArea.Top + model.PlotArea.Height * y.Value;
        break;
      default:
        y1 = y.Value;
        break;
    }
    return new ScreenPoint(x1, y1);
  }

  protected ScreenVector GetVector(PlotLength x, PlotLength y, IRenderContext rc, PlotModel model)
  {
    double x1;
    switch (x.Unit)
    {
      case PlotLengthUnit.Data:
        x1 = this.XAxis.Transform(x.Value) - this.XAxis.Transform(0.0);
        break;
      case PlotLengthUnit.RelativeToViewport:
        x1 = model.Width * x.Value;
        break;
      case PlotLengthUnit.RelativeToPlotArea:
        x1 = model.PlotArea.Width * x.Value;
        break;
      default:
        x1 = x.Value;
        break;
    }
    double y1;
    switch (y.Unit)
    {
      case PlotLengthUnit.Data:
        y1 = -this.YAxis.Transform(y.Value) + this.YAxis.Transform(0.0);
        break;
      case PlotLengthUnit.RelativeToViewport:
        y1 = model.Height * y.Value;
        break;
      case PlotLengthUnit.RelativeToPlotArea:
        y1 = model.PlotArea.Height * y.Value;
        break;
      default:
        y1 = y.Value;
        break;
    }
    return new ScreenVector(x1, y1);
  }
}
