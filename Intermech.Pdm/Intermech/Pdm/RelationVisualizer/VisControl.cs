// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.VisControl
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Map;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class VisControl : MapView
{
  private VisControl.PrintInfo printInfo;
  private IContainer components;

  public VisControl()
  {
    this.InitializeComponent();
    this.InitTools();
  }

  public VisControl(IContainer container)
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
    this.MouseMoveTools.Insert(0, (object) new VisToolZooming((MapView) this));
    this.MouseDownTools.Insert(0, (object) new VisToolPanningAcad((MapView) this));
    this.MouseUpTools.Insert(0, (object) new VisToolSelecting((MapView) this));
  }

  internal event VisControl.CreateRelation OnRelationCreated;

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
        this.OnRelationCreated(from.Node as VisNode, to.Node as VisNode, il);
    }
  }

  internal event PortDoubleClickedHandler PortDoubleClicked;

  internal void OnPortDoubleClicked(VisNodePort sender, MapInputEventArgs evt)
  {
    PortDoubleClickedHandler portDoubleClicked = this.PortDoubleClicked;
    if (portDoubleClicked == null)
      return;
    portDoubleClicked(sender, evt);
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
    this.ZoomToScale_NoLimit(0.85f);
  }

  public override void ZoomOut()
  {
    if ((double) this.DocScale <= 1.0 / 1000.0)
      return;
    this.ZoomToScale_NoLimit(1.15f);
  }

  public void ScrollToControl(MapObject obj)
  {
    if (obj == null || this.DocExtent.Contains(obj.Center))
      return;
    this.DocPosition = new PointF(obj.Center.X - this.DocExtentSize.Width / 2f, obj.Center.Y - this.DocExtentSize.Height / 2f);
  }

  public virtual void ZoomToScale_NoLimit(float scale)
  {
    PointF docPosition = this.DocPosition;
    Rectangle displayRectangle = this.DisplayRectangle;
    PointF doc = this.ConvertViewToDoc(new Point((displayRectangle.Left + displayRectangle.Right) / 2, (displayRectangle.Top + displayRectangle.Bottom) / 2));
    this.ZoomToBox_NoLimit(new RectangleF(0.0f, 0.0f, (float) (((double) doc.X - (double) docPosition.X) * 2.0) * scale, (float) (((double) doc.Y - (double) docPosition.Y) * 2.0) * scale)
    {
      X = (float) ((double) doc.X * (1.0 - (double) scale) + (double) docPosition.X * (double) scale),
      Y = (float) ((double) doc.Y * (1.0 - (double) scale) + (double) docPosition.Y * (double) scale)
    });
  }

  public virtual void ZoomToBox_NoLimit(RectangleF docBox)
  {
    this.OnViewChanging();
    float num = this.DocScale;
    if ((double) docBox.Width > 0.0 && (double) docBox.Height > 0.0)
    {
      Size size = this.DisplayRectangle.Size;
      num = Math.Min((float) size.Width / docBox.Width, (float) size.Height / docBox.Height);
    }
    this.DocScale = num;
    this.myOrigin = new PointF(docBox.X, docBox.Y);
    this.RaisePropertyChangedEvent("DocPosition");
    this.OnViewChanging();
    this.UpdateView();
  }

  public override void Print()
  {
    try
    {
      PrintDocument pd = new PrintDocument();
      pd.PrintPage += new PrintPageEventHandler(((MapView) this).PrintDocumentPage);
      pd.DocumentName = this.Document.Name;
      if (this.PrintShowDialog(pd) == DialogResult.Cancel)
        return;
      pd.Print();
    }
    finally
    {
      this.printInfo = (VisControl.PrintInfo) null;
    }
  }

  protected override void PrintDocumentPage(object sender, PrintPageEventArgs e)
  {
    RectangleF docRect;
    if (this.printInfo == null)
    {
      this.printInfo = new VisControl.PrintInfo();
      this.printInfo.DocRect = this.PrintDocumentRect;
      this.printInfo.HorizScale = this.PrintScale;
      this.printInfo.VertScale = this.printInfo.HorizScale;
      RectangleF printableArea = e.PageSettings.PrintableArea;
      this.printInfo.PrintSize = !e.PageSettings.Landscape ? new SizeF(printableArea.Width / this.printInfo.HorizScale, printableArea.Height / this.printInfo.VertScale) : new SizeF(printableArea.Height / this.printInfo.VertScale, printableArea.Width / this.printInfo.HorizScale);
      if ((double) this.printInfo.PrintSize.Width > 0.0 && (double) this.printInfo.PrintSize.Height > 0.0)
      {
        this.printInfo.NumPagesAcross = (int) Math.Ceiling((double) this.printInfo.DocRect.Width / (double) this.printInfo.PrintSize.Width);
        VisControl.PrintInfo printInfo = this.printInfo;
        docRect = this.printInfo.DocRect;
        int num = (int) Math.Ceiling((double) docRect.Height / (double) this.printInfo.PrintSize.Height);
        printInfo.NumPagesDown = num;
        switch (e.PageSettings.PrinterSettings.PrintRange)
        {
          case PrintRange.Selection:
            this.printInfo.CurPage = 0;
            break;
          case PrintRange.SomePages:
            this.printInfo.CurPage = e.PageSettings.PrinterSettings.FromPage;
            break;
          default:
            this.printInfo.CurPage = 0;
            break;
        }
      }
    }
    if (this.printInfo.NumPagesAcross <= 0 || this.printInfo.NumPagesDown <= 0)
      return;
    int num1 = this.printInfo.CurPage % this.printInfo.NumPagesAcross;
    int num2 = this.printInfo.CurPage / this.printInfo.NumPagesAcross;
    PointF origin = this.myOrigin;
    float horizScale = this.myHorizScale;
    float vertScale = this.myVertScale;
    Size borderSize = this.myBorderSize;
    docRect = this.printInfo.DocRect;
    double x1 = (double) docRect.X + (double) num1 * (double) this.printInfo.PrintSize.Width;
    docRect = this.printInfo.DocRect;
    double y1 = (double) docRect.Y + (double) num2 * (double) this.printInfo.PrintSize.Height;
    this.myOrigin = new PointF((float) x1, (float) y1);
    this.myHorizScale = this.printInfo.HorizScale;
    this.myVertScale = this.printInfo.VertScale;
    this.myBorderSize = new Size(0, 0);
    RectangleF clipRect;
    ref RectangleF local = ref clipRect;
    double x2 = (double) this.myOrigin.X;
    double y2 = (double) this.myOrigin.Y;
    double width1 = (double) this.printInfo.PrintSize.Width;
    docRect = this.printInfo.DocRect;
    double width2 = (double) docRect.Width;
    double width3 = (double) Math.Min((float) width1, (float) width2);
    double height1 = (double) this.printInfo.PrintSize.Height;
    docRect = this.printInfo.DocRect;
    double height2 = (double) docRect.Height;
    double height3 = (double) Math.Min((float) height1, (float) height2);
    local = new RectangleF((float) x2, (float) y2, (float) width3, (float) height3);
    try
    {
      Graphics graphics = e.Graphics;
      graphics.ScaleTransform(this.myHorizScale, this.myVertScale);
      float dx = (float) ((double) this.printInfo.PrintSize.Width / 2.0 - ((double) clipRect.X + (double) clipRect.Width / 2.0));
      float dy = (float) ((double) this.printInfo.PrintSize.Height / 2.0 - ((double) clipRect.Y + (double) clipRect.Height / 2.0));
      graphics.TranslateTransform(dx, dy);
      this.PrintView(graphics, clipRect);
    }
    finally
    {
      this.myOrigin = origin;
      this.myHorizScale = horizScale;
      this.myVertScale = vertScale;
      this.myBorderSize = borderSize;
    }
    int num3;
    switch (e.PageSettings.PrinterSettings.PrintRange)
    {
      case PrintRange.Selection:
        num3 = this.printInfo.NumPagesAcross * this.printInfo.NumPagesDown - 1;
        break;
      case PrintRange.SomePages:
        num3 = e.PageSettings.PrinterSettings.ToPage;
        break;
      default:
        num3 = this.printInfo.NumPagesAcross * this.printInfo.NumPagesDown - 1;
        break;
    }
    e.HasMorePages = this.printInfo.CurPage < num3;
    if (e.HasMorePages)
      ++this.printInfo.CurPage;
    else
      this.printInfo = (VisControl.PrintInfo) null;
  }

  protected override void PrintDecoration(
    Graphics g,
    PrintPageEventArgs e,
    int horPageNum,
    int horPageCount,
    int vertPageNum,
    int vertPageCount)
  {
    float left = e.PageSettings.PrintableArea.Left;
    float top = e.PageSettings.PrintableArea.Top;
    float num1 = (float) ((double) left + (double) e.PageSettings.PrintableArea.Width - 1.0);
    float num2 = (float) ((double) top + (double) e.PageSettings.PrintableArea.Height - 1.0);
    if (e.PageSettings.Landscape)
    {
      double num3 = (double) num1;
      num1 = num2;
      num2 = (float) num3;
    }
    g.DrawLine(Pens.Black, left, top, left + 10f, top);
    g.DrawLine(Pens.Black, left, top, left, top + 10f);
    g.DrawLine(Pens.Black, num1, top, num1 - 10f, top);
    g.DrawLine(Pens.Black, num1, top, num1, top + 10f);
    g.DrawLine(Pens.Black, left, num2, left + 10f, num2);
    g.DrawLine(Pens.Black, left, num2, left, num2 - 10f);
    g.DrawLine(Pens.Black, num1, num2, num1 - 10f, num2);
    g.DrawLine(Pens.Black, num1, num2, num1, num2 - 10f);
    g.DrawLine(Pens.Black, (float) (((double) left + (double) num1) / 2.0), top, (float) (((double) left + (double) num1) / 2.0), num2);
    g.DrawLine(Pens.Black, left, (float) (((double) top + (double) num2) / 2.0), num1, (float) (((double) top + (double) num2) / 2.0));
  }

  private void DrawDocRectangle(Graphics g, RectangleF rect, Pen pen)
  {
    float left = this.printInfo.DocRect.Left;
    float top = this.printInfo.DocRect.Top;
    float width = this.printInfo.DocRect.Width;
    float height = this.printInfo.DocRect.Height;
    float num1 = left + width / 2f;
    float num2 = top + height / 2f;
    g.DrawRectangle(pen, left, top, width, height);
    g.DrawLine(pen, num1, top, num1, (float) this.Bottom);
    g.DrawLine(pen, left, num2, (float) this.Right, num2);
  }

  public override void PrintPreview()
  {
    try
    {
      PrintDocument pd = new PrintDocument();
      pd.PrintPage += new PrintPageEventHandler(((MapView) this).PrintDocumentPage);
      pd.DocumentName = this.Document.Name;
      this.PrintPreviewShowDialog(pd);
    }
    finally
    {
      this.printInfo = (VisControl.PrintInfo) null;
    }
  }

  protected override void PrintPreviewShowDialog(PrintDocument pd)
  {
    int num = (int) new PrintPreviewDialog()
    {
      UseAntiAlias = true,
      Document = pd
    }.ShowDialog();
  }

  protected override DialogResult PrintShowDialog(PrintDocument pd)
  {
    return new PrintDialog()
    {
      AllowSomePages = true,
      Document = pd
    }.ShowDialog();
  }

  protected override void PrintView(Graphics g, RectangleF clipRect)
  {
    this.PaintBackgroundDecoration(g, clipRect);
    g.SmoothingMode = this.SmoothingMode;
    g.TextRenderingHint = this.TextRenderingHint;
    g.InterpolationMode = this.InterpolationMode;
    this.PaintObjects(true, false, g, clipRect);
  }

  public override SizeF PrintDocumentSize
  {
    get
    {
      RectangleF documentBounds = this.ComputeDocumentBounds();
      SizeF printDocumentSize = MapTool.SubtractPoints(new PointF(documentBounds.X + documentBounds.Width, documentBounds.Y + documentBounds.Height), this.PrintDocumentTopLeft);
      printDocumentSize.Width += Math.Abs(this.ShadowOffset.Width);
      printDocumentSize.Height += Math.Abs(this.ShadowOffset.Height);
      return printDocumentSize;
    }
  }

  public RectangleF PrintDocumentRect
  {
    get
    {
      RectangleF documentBounds = this.ComputeDocumentBounds();
      return new RectangleF(documentBounds.Left - 10f, documentBounds.Top - 10f, documentBounds.Width + 20f, documentBounds.Height + 20f);
    }
  }

  public override PointF PrintDocumentTopLeft
  {
    get
    {
      PointF topLeft = this.Document.TopLeft;
      SizeF shadowOffset = this.ShadowOffset;
      if ((double) shadowOffset.Width < 0.0)
        topLeft.X += shadowOffset.Width;
      if ((double) shadowOffset.Height < 0.0)
        topLeft.Y += shadowOffset.Height;
      return topLeft;
    }
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent() => this.components = (IContainer) new System.ComponentModel.Container();

  internal delegate void CreateRelation(VisNode from, VisNode to, IMapLink il);

  [Serializable]
  internal new sealed class PrintInfo
  {
    internal int CurPage { get; set; }

    internal RectangleF DocRect { get; set; }

    internal float HorizScale { get; set; }

    internal int NumPagesAcross { get; set; }

    internal int NumPagesDown { get; set; }

    internal SizeF PrintSize { get; set; }

    internal float VertScale { get; set; }
  }
}
