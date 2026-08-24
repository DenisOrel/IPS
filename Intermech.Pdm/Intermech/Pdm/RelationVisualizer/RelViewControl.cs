// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.RelViewControl
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Map;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class RelViewControl : MapView
{
  private IContainer components;

  public event RelViewControl.CreateRelation OnRelationCreated;

  public RelViewControl()
  {
    this.InitializeComponent();
    this.InitTools();
  }

  public RelViewControl(IContainer container)
  {
    container.Add((IComponent) this);
    this.InitializeComponent();
    this.InitTools();
  }

  private void InitTools()
  {
    this.DoubleBuffered = true;
    this.SetStyle(ControlStyles.UserPaint, true);
    this.SetStyle(ControlStyles.DoubleBuffer, true);
    this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
    this.MouseMoveTools.Insert(0, (object) new RelZoomingTool((MapView) this));
    this.MouseDownTools.Insert(0, (object) new RelToolPanningAcad((MapView) this));
    this.MouseUpTools.Insert(0, (object) new RelToolSelecting((MapView) this));
  }

  public void ZoomOnceCurDocument() => this.DocScale = 1f;

  public void ZoomFit()
  {
    RectangleF rectangleF = this.ComputeDocumentBounds();
    SizeF size = (SizeF) this.DisplayRectangle.Size;
    if ((double) size.Height / (double) size.Width < (double) rectangleF.Height / (double) rectangleF.Width)
    {
      float num = size.Height / size.Width;
      rectangleF = new RectangleF(rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Width * num);
    }
    this.ZoomToFit();
  }

  public override void ZoomIn()
  {
    if ((double) this.DocScale >= 10.0)
      return;
    base.ZoomIn();
  }

  public void ScrollToControl(MapObject obj)
  {
    if (obj == null || this.DocExtent.Contains(obj.Center))
      return;
    this.DocPosition = new PointF(obj.Center.X - this.DocExtentSize.Width / 2f, obj.Center.Y - this.DocExtentSize.Height / 2f);
  }

  public override void ZoomOut()
  {
    if ((double) this.DocScale <= 1.0 / 1000.0)
      return;
    base.ZoomOut();
  }

  public override void DoToolTipObject(MapObject obj) => base.DoToolTipObject(obj);

  public override IMapLink CreateLink(IMapPort from, IMapPort to)
  {
    IMapLink il = (IMapLink) null;
    try
    {
      il = base.CreateLink(from, to);
      return il;
    }
    finally
    {
      if (this.OnRelationCreated != null)
        this.OnRelationCreated(from.Node as VisObjectNode, to.Node as VisObjectNode, il);
    }
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent() => this.components = (IContainer) new System.ComponentModel.Container();

  public delegate void CreateRelation(VisObjectNode from, VisObjectNode to, IMapLink il);
}
