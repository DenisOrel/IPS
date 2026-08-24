// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.RelZoomingTool
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Map;
using System;
using System.Drawing;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

[Serializable]
public class RelZoomingTool(MapView v) : MapToolZooming(v)
{
  public override bool CanStart() => base.CanStart();

  public override Rectangle ComputeRubberBandBox()
  {
    Point viewPoint1 = this.FirstInput.ViewPoint;
    Point viewPoint2 = this.LastInput.ViewPoint;
    return new Rectangle(Math.Min(viewPoint2.X, viewPoint1.X), Math.Min(viewPoint2.Y, viewPoint1.Y), Math.Abs(viewPoint2.X - viewPoint1.X), Math.Abs(viewPoint2.Y - viewPoint1.Y));
  }

  public override void DoRubberBand(Rectangle box)
  {
    if (box.Width < 4 || box.Height < 4)
      return;
    MapView zoomedView = this.ZoomedView;
    if (zoomedView == null)
      return;
    RectangleF doc1 = this.View.ConvertViewToDoc(box);
    PointF pointF = new PointF((float) (((double) doc1.Left + (double) doc1.Right) / 2.0), (float) (((double) doc1.Top + (double) doc1.Bottom) / 2.0));
    Size size = zoomedView.DisplayRectangle.Size;
    zoomedView.DocScale = Math.Min((float) size.Width / doc1.Width, (float) size.Height / doc1.Height);
    SizeF doc2 = zoomedView.ConvertViewToDoc(size);
    zoomedView.DocPosition = new PointF(pointF.X - doc2.Width / 2f, pointF.Y - doc2.Height / 2f);
  }

  public override void DoSelect(MapInputEventArgs evt) => base.DoSelect(evt);
}
