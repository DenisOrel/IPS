// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.RibbonRenderer
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class RibbonRenderer
{
  private static ColorMatrix _disabledImageColorMatrix;
  private ColorTable _colorTable = new ColorTable();
  private Size arrowSize = new Size(5, 3);

  private static Image CreateDisabledImage(Image normalImage)
  {
    Size size = normalImage.Size;
    Bitmap disabledImage = new Bitmap(size.Width, size.Height);
    using (ImageAttributes imageAttr = new ImageAttributes())
    {
      imageAttr.ClearColorKey();
      imageAttr.SetColorMatrix(RibbonRenderer.DisabledImageColorMatrix);
      using (Graphics graphics = Graphics.FromImage((Image) disabledImage))
        graphics.DrawImage(normalImage, new Rectangle(0, 0, size.Width, size.Height), 0, 0, size.Width, size.Height, GraphicsUnit.Pixel, imageAttr);
    }
    return (Image) disabledImage;
  }

  private static ColorMatrix DisabledImageColorMatrix
  {
    get
    {
      if (RibbonRenderer._disabledImageColorMatrix == null)
        RibbonRenderer._disabledImageColorMatrix = RibbonRenderer.MultiplyColorMatrix(new float[5][]
        {
          new float[5]{ 1f, 0.0f, 0.0f, 0.0f, 0.0f },
          new float[5]{ 0.0f, 1f, 0.0f, 0.0f, 0.0f },
          new float[5]{ 0.0f, 0.0f, 1f, 0.0f, 0.0f },
          new float[5]{ 0.0f, 0.0f, 0.0f, 0.7f, 0.0f },
          new float[5]
        }, new float[5][]
        {
          new float[5]{ 0.2125f, 0.2125f, 0.2125f, 0.0f, 0.0f },
          new float[5]{ 0.2577f, 0.2577f, 0.2577f, 0.0f, 0.0f },
          new float[5]{ 0.0361f, 0.0361f, 0.0361f, 0.0f, 0.0f },
          new float[5]{ 0.0f, 0.0f, 0.0f, 1f, 0.0f },
          new float[5]{ 0.38f, 0.38f, 0.38f, 0.0f, 1f }
        });
      return RibbonRenderer._disabledImageColorMatrix;
    }
  }

  internal static ColorMatrix MultiplyColorMatrix(float[][] matrix1, float[][] matrix2)
  {
    int length = 5;
    float[][] newColorMatrix = new float[length][];
    float[] numArray1 = new float[length];
    for (int index = 0; index < length; ++index)
      newColorMatrix[index] = new float[length];
    for (int index1 = 0; index1 < length; ++index1)
    {
      for (int index2 = 0; index2 < length; ++index2)
        numArray1[index2] = matrix1[index2][index1];
      for (int index3 = 0; index3 < length; ++index3)
      {
        float[] numArray2 = matrix2[index3];
        float num = 0.0f;
        for (int index4 = 0; index4 < length; ++index4)
          num += numArray2[index4] * numArray1[index4];
        newColorMatrix[index3][index1] = num;
      }
    }
    return new ColorMatrix(newColorMatrix);
  }

  public static GraphicsPath RoundRectangle(
    Rectangle r,
    int radius,
    RibbonRenderer.Corners corners)
  {
    GraphicsPath graphicsPath = new GraphicsPath();
    int num1 = radius * 2;
    int num2 = (corners & RibbonRenderer.Corners.NorthWest) == RibbonRenderer.Corners.NorthWest ? num1 : 0;
    int num3 = (corners & RibbonRenderer.Corners.NorthEast) == RibbonRenderer.Corners.NorthEast ? num1 : 0;
    int num4 = (corners & RibbonRenderer.Corners.SouthEast) == RibbonRenderer.Corners.SouthEast ? num1 : 0;
    int num5 = (corners & RibbonRenderer.Corners.SouthWest) == RibbonRenderer.Corners.SouthWest ? num1 : 0;
    graphicsPath.AddLine(r.Left + num2, r.Top, r.Right - num3, r.Top);
    if (num3 > 0)
      graphicsPath.AddArc(Rectangle.FromLTRB(r.Right - num3, r.Top, r.Right, r.Top + num3), -90f, 90f);
    graphicsPath.AddLine(r.Right, r.Top + num3, r.Right, r.Bottom - num4);
    if (num4 > 0)
      graphicsPath.AddArc(Rectangle.FromLTRB(r.Right - num4, r.Bottom - num4, r.Right, r.Bottom), 0.0f, 90f);
    graphicsPath.AddLine(r.Right - num4, r.Bottom, r.Left + num5, r.Bottom);
    if (num5 > 0)
      graphicsPath.AddArc(Rectangle.FromLTRB(r.Left, r.Bottom - num5, r.Left + num5, r.Bottom), 90f, 90f);
    graphicsPath.AddLine(r.Left, r.Bottom - num5, r.Left, r.Top + num2);
    if (num2 > 0)
      graphicsPath.AddArc(Rectangle.FromLTRB(r.Left, r.Top, r.Left + num2, r.Top + num2), 180f, 90f);
    graphicsPath.CloseFigure();
    return graphicsPath;
  }

  private RibbonRenderer.Corners ButtonCorners(RibbonButton button) => RibbonRenderer.Corners.All;

  private RibbonRenderer.Corners ButtonDdRounding(RibbonButton button)
  {
    return button.SizeMode != RibbonElementSizeMode.Large ? RibbonRenderer.Corners.East : RibbonRenderer.Corners.South;
  }

  private RibbonRenderer.Corners ButtonFaceRounding(RibbonButton button)
  {
    return button.SizeMode != RibbonElementSizeMode.Large ? RibbonRenderer.Corners.West : RibbonRenderer.Corners.North;
  }

  public GraphicsPath CreateCompleteTabPath(RibbonTab t)
  {
    GraphicsPath completeTabPath = new GraphicsPath();
    int num = 6;
    Rectangle rectangle;
    if (t.Bounds.Width > 0)
    {
      GraphicsPath graphicsPath1 = completeTabPath;
      int x1 = t.TabBounds.Left + num;
      Rectangle tabBounds1 = t.TabBounds;
      int top1 = tabBounds1.Top;
      tabBounds1 = t.TabBounds;
      int x2 = tabBounds1.Right - num;
      tabBounds1 = t.TabBounds;
      int top2 = tabBounds1.Top;
      graphicsPath1.AddLine(x1, top1, x2, top2);
      GraphicsPath graphicsPath2 = completeTabPath;
      tabBounds1 = t.TabBounds;
      int left = tabBounds1.Right - num;
      tabBounds1 = t.TabBounds;
      int top3 = tabBounds1.Top;
      tabBounds1 = t.TabBounds;
      int right1 = tabBounds1.Right;
      tabBounds1 = t.TabBounds;
      int bottom1 = tabBounds1.Top + num;
      Rectangle rect1 = Rectangle.FromLTRB(left, top3, right1, bottom1);
      graphicsPath2.AddArc(rect1, -90f, 90f);
      GraphicsPath graphicsPath3 = completeTabPath;
      tabBounds1 = t.TabBounds;
      int right2 = tabBounds1.Right;
      tabBounds1 = t.TabBounds;
      int y1 = tabBounds1.Top + num;
      Rectangle tabBounds2 = t.TabBounds;
      int right3 = tabBounds2.Right;
      tabBounds2 = t.TabBounds;
      int y2 = tabBounds2.Bottom - num;
      graphicsPath3.AddLine(right2, y1, right3, y2);
      GraphicsPath graphicsPath4 = completeTabPath;
      tabBounds2 = t.TabBounds;
      int right4 = tabBounds2.Right;
      tabBounds2 = t.TabBounds;
      int top4 = tabBounds2.Bottom - num;
      rectangle = t.TabBounds;
      int right5 = rectangle.Right + num;
      rectangle = t.TabBounds;
      int bottom2 = rectangle.Bottom;
      Rectangle rect2 = Rectangle.FromLTRB(right4, top4, right5, bottom2);
      graphicsPath4.AddArc(rect2, -180f, -90f);
    }
    GraphicsPath graphicsPath5 = completeTabPath;
    rectangle = t.TabBounds;
    int x1_1 = rectangle.Right + num;
    rectangle = t.TabBounds;
    int bottom3 = rectangle.Bottom;
    rectangle = t.TabContentBounds;
    int x2_1 = rectangle.Right - num;
    rectangle = t.TabBounds;
    int bottom4 = rectangle.Bottom;
    graphicsPath5.AddLine(x1_1, bottom3, x2_1, bottom4);
    GraphicsPath graphicsPath6 = completeTabPath;
    rectangle = t.TabContentBounds;
    int left1 = rectangle.Right - num;
    rectangle = t.TabBounds;
    int bottom5 = rectangle.Bottom;
    rectangle = t.TabContentBounds;
    int right6 = rectangle.Right;
    rectangle = t.TabBounds;
    int bottom6 = rectangle.Bottom + num;
    Rectangle rect3 = Rectangle.FromLTRB(left1, bottom5, right6, bottom6);
    graphicsPath6.AddArc(rect3, -90f, 90f);
    GraphicsPath graphicsPath7 = completeTabPath;
    rectangle = t.TabContentBounds;
    int right7 = rectangle.Right;
    rectangle = t.TabContentBounds;
    int y1_1 = rectangle.Top + num;
    rectangle = t.TabContentBounds;
    int right8 = rectangle.Right;
    rectangle = t.TabContentBounds;
    int y2_1 = rectangle.Bottom - num;
    graphicsPath7.AddLine(right7, y1_1, right8, y2_1);
    GraphicsPath graphicsPath8 = completeTabPath;
    rectangle = t.TabContentBounds;
    int left2 = rectangle.Right - num;
    rectangle = t.TabContentBounds;
    int top5 = rectangle.Bottom - num;
    rectangle = t.TabContentBounds;
    int right9 = rectangle.Right;
    rectangle = t.TabContentBounds;
    int bottom7 = rectangle.Bottom;
    Rectangle rect4 = Rectangle.FromLTRB(left2, top5, right9, bottom7);
    graphicsPath8.AddArc(rect4, 0.0f, 90f);
    GraphicsPath graphicsPath9 = completeTabPath;
    rectangle = t.TabContentBounds;
    int x1_2 = rectangle.Right - num;
    rectangle = t.TabContentBounds;
    int bottom8 = rectangle.Bottom;
    rectangle = t.TabContentBounds;
    int x2_2 = rectangle.Left + num;
    rectangle = t.TabContentBounds;
    int bottom9 = rectangle.Bottom;
    graphicsPath9.AddLine(x1_2, bottom8, x2_2, bottom9);
    GraphicsPath graphicsPath10 = completeTabPath;
    rectangle = t.TabContentBounds;
    int left3 = rectangle.Left;
    rectangle = t.TabContentBounds;
    int top6 = rectangle.Bottom - num;
    rectangle = t.TabContentBounds;
    int right10 = rectangle.Left + num;
    rectangle = t.TabContentBounds;
    int bottom10 = rectangle.Bottom;
    Rectangle rect5 = Rectangle.FromLTRB(left3, top6, right10, bottom10);
    graphicsPath10.AddArc(rect5, 90f, 90f);
    GraphicsPath graphicsPath11 = completeTabPath;
    rectangle = t.TabContentBounds;
    int left4 = rectangle.Left;
    rectangle = t.TabContentBounds;
    int y1_2 = rectangle.Bottom - num;
    rectangle = t.TabContentBounds;
    int left5 = rectangle.Left;
    rectangle = t.TabBounds;
    int y2_2 = rectangle.Bottom + num;
    graphicsPath11.AddLine(left4, y1_2, left5, y2_2);
    GraphicsPath graphicsPath12 = completeTabPath;
    rectangle = t.TabContentBounds;
    int left6 = rectangle.Left;
    rectangle = t.TabBounds;
    int bottom11 = rectangle.Bottom;
    rectangle = t.TabContentBounds;
    int right11 = rectangle.Left + num;
    rectangle = t.TabBounds;
    int bottom12 = rectangle.Bottom + num;
    Rectangle rect6 = Rectangle.FromLTRB(left6, bottom11, right11, bottom12);
    graphicsPath12.AddArc(rect6, 180f, 90f);
    rectangle = t.Bounds;
    if (rectangle.Width > 0)
    {
      GraphicsPath graphicsPath13 = completeTabPath;
      rectangle = t.TabContentBounds;
      int x1_3 = rectangle.Left + num;
      rectangle = t.TabContentBounds;
      int top7 = rectangle.Top;
      rectangle = t.TabBounds;
      int x2_3 = rectangle.Left - num;
      rectangle = t.TabBounds;
      int bottom13 = rectangle.Bottom;
      graphicsPath13.AddLine(x1_3, top7, x2_3, bottom13);
      GraphicsPath graphicsPath14 = completeTabPath;
      rectangle = t.TabBounds;
      int left7 = rectangle.Left - num;
      rectangle = t.TabBounds;
      int top8 = rectangle.Bottom - num;
      rectangle = t.TabBounds;
      int left8 = rectangle.Left;
      rectangle = t.TabBounds;
      int bottom14 = rectangle.Bottom;
      Rectangle rect7 = Rectangle.FromLTRB(left7, top8, left8, bottom14);
      graphicsPath14.AddArc(rect7, 90f, -90f);
      GraphicsPath graphicsPath15 = completeTabPath;
      rectangle = t.TabBounds;
      int left9 = rectangle.Left;
      rectangle = t.TabBounds;
      int y1_3 = rectangle.Bottom - num;
      rectangle = t.TabBounds;
      int left10 = rectangle.Left;
      rectangle = t.TabBounds;
      int y2_3 = rectangle.Top + num;
      graphicsPath15.AddLine(left9, y1_3, left10, y2_3);
      GraphicsPath graphicsPath16 = completeTabPath;
      rectangle = t.TabBounds;
      int left11 = rectangle.Left;
      rectangle = t.TabBounds;
      int top9 = rectangle.Top;
      rectangle = t.TabBounds;
      int right12 = rectangle.Left + num;
      rectangle = t.TabBounds;
      int bottom15 = rectangle.Top + num;
      Rectangle rect8 = Rectangle.FromLTRB(left11, top9, right12, bottom15);
      graphicsPath16.AddArc(rect8, 180f, 90f);
    }
    completeTabPath.CloseFigure();
    return completeTabPath;
  }

  public GraphicsPath CreateTabPath(RibbonTab t)
  {
    GraphicsPath tabPath = new GraphicsPath();
    int num1 = 6;
    int num2 = 1;
    Rectangle tabBounds1 = t.TabBounds;
    int left1 = tabBounds1.Left;
    tabBounds1 = t.TabBounds;
    int bottom1 = tabBounds1.Bottom;
    tabBounds1 = t.TabBounds;
    int left2 = tabBounds1.Left;
    tabBounds1 = t.TabBounds;
    int y2 = tabBounds1.Top + num1;
    tabPath.AddLine(left1, bottom1, left2, y2);
    Rectangle tabBounds2 = t.TabBounds;
    int left3 = tabBounds2.Left;
    tabBounds2 = t.TabBounds;
    int top1 = tabBounds2.Top;
    int width1 = num1;
    int height1 = num1;
    tabPath.AddArc(new Rectangle(left3, top1, width1, height1), 180f, 90f);
    Rectangle tabBounds3 = t.TabBounds;
    int x1_1 = tabBounds3.Left + num1;
    tabBounds3 = t.TabBounds;
    int top2 = tabBounds3.Top;
    tabBounds3 = t.TabBounds;
    int x2_1 = tabBounds3.Right - num1 - num2;
    tabBounds3 = t.TabBounds;
    int top3 = tabBounds3.Top;
    tabPath.AddLine(x1_1, top2, x2_1, top3);
    Rectangle tabBounds4 = t.TabBounds;
    int x = tabBounds4.Right - num1 - num2;
    tabBounds4 = t.TabBounds;
    int top4 = tabBounds4.Top;
    int width2 = num1;
    int height2 = num1;
    tabPath.AddArc(new Rectangle(x, top4, width2, height2), -90f, 90f);
    Rectangle tabBounds5 = t.TabBounds;
    int x1_2 = tabBounds5.Right - num2;
    tabBounds5 = t.TabBounds;
    int y1 = tabBounds5.Top + num1;
    tabBounds5 = t.TabBounds;
    int x2_2 = tabBounds5.Right - num2;
    tabBounds5 = t.TabBounds;
    int bottom2 = tabBounds5.Bottom;
    tabPath.AddLine(x1_2, y1, x2_2, bottom2);
    return tabPath;
  }

  public void DrawArrow(Graphics g, Rectangle b, Color c, RibbonArrowDirection d)
  {
    using (GraphicsPath path = new GraphicsPath())
    {
      Rectangle rectangle = b;
      if (b.Width % 2 != 0 && d == RibbonArrowDirection.Up)
        rectangle = new Rectangle(new Point(b.Left - 1, b.Top - 1), new Size(b.Width + 1, b.Height + 1));
      switch (d)
      {
        case RibbonArrowDirection.Up:
          path.AddLine(rectangle.Left, rectangle.Bottom, rectangle.Right, rectangle.Bottom);
          path.AddLine(rectangle.Right, rectangle.Bottom, rectangle.Left + rectangle.Width / 2, rectangle.Top);
          break;
        case RibbonArrowDirection.Down:
          path.AddLine(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Top);
          path.AddLine(rectangle.Right, rectangle.Top, rectangle.Left + rectangle.Width / 2, rectangle.Bottom);
          break;
        case RibbonArrowDirection.Left:
          path.AddLine(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Top + rectangle.Height / 2);
          path.AddLine(rectangle.Right, rectangle.Top + rectangle.Height / 2, rectangle.Left, rectangle.Bottom);
          break;
        default:
          path.AddLine(rectangle.Right, rectangle.Top, rectangle.Left, rectangle.Top + rectangle.Height / 2);
          path.AddLine(rectangle.Left, rectangle.Top + rectangle.Height / 2, rectangle.Right, rectangle.Bottom);
          break;
      }
      path.CloseFigure();
      using (SolidBrush solidBrush = new SolidBrush(c))
      {
        SmoothingMode smoothingMode = g.SmoothingMode;
        g.SmoothingMode = SmoothingMode.None;
        g.FillPath((Brush) solidBrush, path);
        g.SmoothingMode = smoothingMode;
      }
    }
  }

  private void DrawArrowShaded(Graphics g, Rectangle b, RibbonArrowDirection d, bool enabled)
  {
    Size size = this.arrowSize;
    if (d == RibbonArrowDirection.Left || d == RibbonArrowDirection.Right)
      size = new Size(this.arrowSize.Height, this.arrowSize.Width);
    Rectangle b1 = new Rectangle(new Point(b.Left + (b.Width - size.Width) / 2, b.Top + (b.Height - size.Height) / 2), size);
    Rectangle b2 = b1;
    b2.Offset(0, 1);
    this.DrawArrow(g, b2, enabled ? ColorTable.ArrowLight : Color.Transparent, d);
    this.DrawArrow(g, b1, enabled ? ColorTable.Arrow : ColorTable.ArrowDisabled, d);
  }

  public void DrawButton(Graphics g, Rectangle bounds, RibbonRenderer.Corners corners)
  {
    if (bounds.Height <= 0 || bounds.Width <= 0)
      return;
    Rectangle r1 = Rectangle.FromLTRB(bounds.Left, bounds.Top, bounds.Right - 1, bounds.Bottom - 1);
    Rectangle r2 = Rectangle.FromLTRB(bounds.Left + 1, bounds.Top + 1, bounds.Right - 2, bounds.Bottom - 2);
    Rectangle rectangle = Rectangle.FromLTRB(bounds.Left + 1, bounds.Top + 1, bounds.Right - 2, bounds.Top + Convert.ToInt32((double) bounds.Height * 0.36));
    int num = (int) corners;
    using (GraphicsPath path1 = RibbonRenderer.RoundRectangle(r1, 3, (RibbonRenderer.Corners) num))
    {
      using (SolidBrush solidBrush = new SolidBrush(ColorTable.ButtonBgOut))
        g.FillPath((Brush) solidBrush, path1);
      using (GraphicsPath path2 = new GraphicsPath())
      {
        path2.AddEllipse(new Rectangle(bounds.Left, bounds.Top, bounds.Width, bounds.Height * 2));
        path2.CloseFigure();
        using (PathGradientBrush pathGradientBrush = new PathGradientBrush(path2))
        {
          pathGradientBrush.WrapMode = WrapMode.Clamp;
          pathGradientBrush.CenterPoint = new PointF(Convert.ToSingle(bounds.Left + bounds.Width / 2), Convert.ToSingle(bounds.Bottom));
          pathGradientBrush.CenterColor = ColorTable.ButtonBgCenter;
          pathGradientBrush.SurroundColors = new Color[1]
          {
            ColorTable.ButtonBgOut
          };
          Blend blend = new Blend(3)
          {
            Factors = new float[3]{ 0.0f, 0.8f, 0.0f },
            Positions = new float[3]{ 0.0f, 0.3f, 1f }
          };
          Region clip = g.Clip;
          Region region = new Region(path1);
          region.Intersect(clip);
          g.SetClip(region.GetBounds(g));
          g.FillPath((Brush) pathGradientBrush, path2);
          g.Clip = clip;
        }
      }
      using (Pen pen = new Pen(ColorTable.ButtonBorderOut))
        g.DrawPath(pen, path1);
      using (GraphicsPath path3 = RibbonRenderer.RoundRectangle(r2, 3, corners))
      {
        using (Pen pen = new Pen(ColorTable.ButtonBorderIn))
          g.DrawPath(pen, path3);
      }
      using (GraphicsPath path4 = RibbonRenderer.RoundRectangle(rectangle, 3, corners & RibbonRenderer.Corners.NorthWest | corners & RibbonRenderer.Corners.NorthEast))
      {
        if (rectangle.Width <= 0 || rectangle.Height <= 0)
          return;
        using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle, ColorTable.ButtonGlossyNorth, ColorTable.ButtonGlossySouth, 90f))
        {
          linearGradientBrush.WrapMode = WrapMode.TileFlipXY;
          g.FillPath((Brush) linearGradientBrush, path4);
        }
      }
    }
  }

  private void DrawButtonDropDownArrow(Graphics g, RibbonButton button, Rectangle textLayout)
  {
    Rectangle empty = Rectangle.Empty;
    Rectangle b = button.SizeMode == RibbonElementSizeMode.Large || button.SizeMode == RibbonElementSizeMode.Overflow ? this.LargeButtonDropDownArrowBounds(g, button.Owner.Font, button.Text, textLayout) : textLayout;
    this.DrawArrowShaded(g, b, RibbonArrowDirection.Down, button.Enabled);
  }

  private void DrawButtonPressed(Graphics g, Rectangle bounds, RibbonRenderer.Corners corners)
  {
    Rectangle r1 = Rectangle.FromLTRB(bounds.Left, bounds.Top, bounds.Right - 1, bounds.Bottom - 1);
    Rectangle r2 = Rectangle.FromLTRB(bounds.Left + 1, bounds.Top + 1, bounds.Right - 2, bounds.Bottom - 2);
    Rectangle rectangle = Rectangle.FromLTRB(bounds.Left + 1, bounds.Top + 1, bounds.Right - 2, bounds.Top + Convert.ToInt32((double) bounds.Height * 0.36));
    using (GraphicsPath path1 = RibbonRenderer.RoundRectangle(r1, 4, corners))
    {
      using (SolidBrush solidBrush = new SolidBrush(ColorTable.ButtonPressedBgOut))
        g.FillPath((Brush) solidBrush, path1);
      using (GraphicsPath path2 = new GraphicsPath())
      {
        path2.AddEllipse(new Rectangle(bounds.Left, bounds.Top, bounds.Width, bounds.Height * 2));
        path2.CloseFigure();
        using (PathGradientBrush pathGradientBrush = new PathGradientBrush(path2))
        {
          pathGradientBrush.WrapMode = WrapMode.Clamp;
          pathGradientBrush.CenterPoint = new PointF(Convert.ToSingle(bounds.Left + bounds.Width / 2), Convert.ToSingle(bounds.Bottom));
          pathGradientBrush.CenterColor = ColorTable.ButtonPressedBgCenter;
          pathGradientBrush.SurroundColors = new Color[1]
          {
            ColorTable.ButtonPressedBgOut
          };
          Blend blend = new Blend(3)
          {
            Factors = new float[3]{ 0.0f, 0.8f, 0.0f },
            Positions = new float[3]{ 0.0f, 0.3f, 1f }
          };
          Region clip = g.Clip;
          Region region = new Region(path1);
          region.Intersect(clip);
          g.SetClip(region.GetBounds(g));
          g.FillPath((Brush) pathGradientBrush, path2);
          g.Clip = clip;
        }
      }
      using (Pen pen = new Pen(ColorTable.ButtonPressedBorderOut))
        g.DrawPath(pen, path1);
      using (GraphicsPath path3 = RibbonRenderer.RoundRectangle(r2, 4, corners))
      {
        using (Pen pen = new Pen(ColorTable.ButtonPressedBorderIn))
          g.DrawPath(pen, path3);
      }
      using (GraphicsPath path4 = RibbonRenderer.RoundRectangle(rectangle, 4, corners & RibbonRenderer.Corners.NorthWest | corners & RibbonRenderer.Corners.NorthEast))
      {
        using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle, ColorTable.ButtonPressedGlossyNorth, ColorTable.ButtonPressedGlossySouth, 90f))
        {
          linearGradientBrush.WrapMode = WrapMode.TileFlipXY;
          g.FillPath((Brush) linearGradientBrush, path4);
        }
      }
    }
    this.DrawPressedShadow(g, r1);
  }

  private void DrawButtonSelected(Graphics g, Rectangle bounds, RibbonRenderer.Corners corners)
  {
    Rectangle r1 = Rectangle.FromLTRB(bounds.Left, bounds.Top, bounds.Right - 1, bounds.Bottom - 1);
    Rectangle r2 = Rectangle.FromLTRB(bounds.Left + 1, bounds.Top + 1, bounds.Right - 2, bounds.Bottom - 2);
    Rectangle rectangle = Rectangle.FromLTRB(bounds.Left + 1, bounds.Top + 1, bounds.Right - 2, bounds.Top + Convert.ToInt32((double) bounds.Height * 0.36));
    int num = (int) corners;
    using (GraphicsPath path1 = RibbonRenderer.RoundRectangle(r1, 4, (RibbonRenderer.Corners) num))
    {
      using (SolidBrush solidBrush = new SolidBrush(ColorTable.ButtonSelectedBgOut))
        g.FillPath((Brush) solidBrush, path1);
      using (GraphicsPath path2 = new GraphicsPath())
      {
        path2.AddEllipse(new Rectangle(bounds.Left, bounds.Top, bounds.Width, bounds.Height * 2));
        path2.CloseFigure();
        using (PathGradientBrush pathGradientBrush = new PathGradientBrush(path2))
        {
          pathGradientBrush.WrapMode = WrapMode.Clamp;
          pathGradientBrush.CenterPoint = new PointF(Convert.ToSingle(bounds.Left + bounds.Width / 2), Convert.ToSingle(bounds.Bottom));
          pathGradientBrush.CenterColor = ColorTable.ButtonSelectedBgCenter;
          pathGradientBrush.SurroundColors = new Color[1]
          {
            ColorTable.ButtonSelectedBgOut
          };
          Blend blend = new Blend(3)
          {
            Factors = new float[3]{ 0.0f, 0.8f, 0.0f },
            Positions = new float[3]{ 0.0f, 0.3f, 1f }
          };
          Region clip = g.Clip;
          Region region = new Region(path1);
          region.Intersect(clip);
          g.SetClip(region.GetBounds(g));
          g.FillPath((Brush) pathGradientBrush, path2);
          g.Clip = clip;
        }
      }
      using (Pen pen = new Pen(ColorTable.ButtonSelectedBorderOut))
        g.DrawPath(pen, path1);
      using (GraphicsPath path3 = RibbonRenderer.RoundRectangle(r2, 4, corners))
      {
        using (Pen pen = new Pen(ColorTable.ButtonSelectedBorderIn))
          g.DrawPath(pen, path3);
      }
      using (GraphicsPath path4 = RibbonRenderer.RoundRectangle(rectangle, 4, corners & RibbonRenderer.Corners.NorthWest | corners & RibbonRenderer.Corners.NorthEast))
      {
        using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle, ColorTable.ButtonSelectedGlossyNorth, ColorTable.ButtonSelectedGlossySouth, 90f))
        {
          linearGradientBrush.WrapMode = WrapMode.TileFlipXY;
          g.FillPath((Brush) linearGradientBrush, path4);
        }
      }
    }
  }

  public void DrawCompleteTab(RibbonTabRenderEventArgs e)
  {
    if (e.Tab.Bounds.Width > 0)
      this.DrawTabActive(e);
    using (GraphicsPath path = RibbonRenderer.RoundRectangle(e.Tab.TabContentBounds, 4, RibbonRenderer.Corners.All))
    {
      Color tabContentNorth = ColorTable.TabContentNorth;
      Color tabContentSouth = ColorTable.TabContentSouth;
      using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(new Point(0, e.Tab.TabContentBounds.Top + 30), new Point(0, e.Tab.TabContentBounds.Bottom - 10), tabContentNorth, tabContentSouth))
      {
        linearGradientBrush.WrapMode = WrapMode.TileFlipXY;
        e.Graphics.FillPath((Brush) linearGradientBrush, path);
      }
    }
    int left = e.Tab.TabContentBounds.Left;
    Rectangle tabContentBounds = e.Tab.TabContentBounds;
    int top = tabContentBounds.Top;
    tabContentBounds = e.Tab.TabContentBounds;
    int right = tabContentBounds.Right;
    tabContentBounds = e.Tab.TabContentBounds;
    int bottom = tabContentBounds.Top + 18;
    using (GraphicsPath path = RibbonRenderer.RoundRectangle(Rectangle.FromLTRB(left, top, right, bottom), 6, RibbonRenderer.Corners.North))
    {
      using (Brush brush = (Brush) new SolidBrush(Color.FromArgb(30, Color.White)))
        e.Graphics.FillPath(brush, path);
    }
    using (GraphicsPath completeTabPath = this.CreateCompleteTabPath(e.Tab))
    {
      using (Pen pen = new Pen(ColorTable.TabBorder))
      {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        e.Graphics.DrawPath(pen, completeTabPath);
      }
    }
    if (!e.Tab.Selected)
      return;
    using (GraphicsPath tabPath = this.CreateTabPath(e.Tab))
    {
      Pen pen = new Pen(Color.FromArgb(150, Color.Gold))
      {
        Width = 2f
      };
      e.Graphics.DrawPath(pen, tabPath);
      pen.Dispose();
    }
  }

  private void DrawPanelNormal(RibbonPanelRenderEventArgs e)
  {
    Rectangle bounds1 = e.Panel.Bounds;
    int left1 = bounds1.Left;
    bounds1 = e.Panel.Bounds;
    int top1 = bounds1.Top;
    bounds1 = e.Panel.Bounds;
    int right1 = bounds1.Right;
    bounds1 = e.Panel.Bounds;
    int bottom1 = bounds1.Bottom;
    Rectangle r1 = Rectangle.FromLTRB(left1, top1, right1, bottom1);
    Rectangle bounds2 = e.Panel.Bounds;
    int left2 = bounds2.Left + 1;
    bounds2 = e.Panel.Bounds;
    int top2 = bounds2.Top + 1;
    bounds2 = e.Panel.Bounds;
    int right2 = bounds2.Right + 1;
    bounds2 = e.Panel.Bounds;
    int bottom2 = bounds2.Bottom;
    Rectangle r2 = Rectangle.FromLTRB(left2, top2, right2, bottom2);
    Rectangle rectangle = e.Panel.Bounds;
    int left3 = rectangle.Left + 1;
    rectangle = e.Panel.ContentBounds;
    int bottom3 = rectangle.Bottom;
    rectangle = e.Panel.Bounds;
    int right3 = rectangle.Right - 1;
    rectangle = e.Panel.Bounds;
    int bottom4 = rectangle.Bottom - 1;
    Rectangle r3 = Rectangle.FromLTRB(left3, bottom3, right3, bottom4);
    GraphicsPath path1 = RibbonRenderer.RoundRectangle(r1, 3, RibbonRenderer.Corners.All);
    GraphicsPath path2 = RibbonRenderer.RoundRectangle(r2, 3, RibbonRenderer.Corners.All);
    GraphicsPath path3 = RibbonRenderer.RoundRectangle(r3, 3, RibbonRenderer.Corners.South);
    using (Pen pen = new Pen(ColorTable.PanelLightBorder))
      e.Graphics.DrawPath(pen, path2);
    using (Pen pen = new Pen(ColorTable.PanelDarkBorder))
      e.Graphics.DrawPath(pen, path1);
    using (SolidBrush solidBrush = new SolidBrush(ColorTable.PanelTextBackground))
      e.Graphics.FillPath((Brush) solidBrush, path3);
    path3.Dispose();
    path1.Dispose();
    path2.Dispose();
  }

  private void DrawPanelOverflowImage(RibbonPanelRenderEventArgs e)
  {
    int num1 = 3;
    Size size1 = new Size(32 /*0x20*/, 32 /*0x20*/);
    Rectangle rectangle1;
    ref Rectangle local = ref rectangle1;
    Rectangle bounds1 = e.Panel.Bounds;
    int left1 = bounds1.Left;
    bounds1 = e.Panel.Bounds;
    int num2 = (bounds1.Width - size1.Width) / 2;
    int x = left1 + num2;
    bounds1 = e.Panel.Bounds;
    int y = bounds1.Top + 5;
    Point location = new Point(x, y);
    Size size2 = size1;
    local = new Rectangle(location, size2);
    Rectangle r = Rectangle.FromLTRB(rectangle1.Left, rectangle1.Bottom - 10, rectangle1.Right, rectangle1.Bottom);
    Rectangle bounds2 = e.Panel.Bounds;
    int left2 = bounds2.Left + num1;
    int top = rectangle1.Bottom + num1;
    bounds2 = e.Panel.Bounds;
    int right = bounds2.Right - num1;
    bounds2 = e.Panel.Bounds;
    int bottom = bounds2.Bottom - num1;
    Rectangle rectangle2 = Rectangle.FromLTRB(left2, top, right, bottom);
    GraphicsPath path1 = RibbonRenderer.RoundRectangle(rectangle1, 5, RibbonRenderer.Corners.All);
    GraphicsPath path2 = RibbonRenderer.RoundRectangle(r, 5, RibbonRenderer.Corners.South);
    using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle1, ColorTable.TabContentNorth, ColorTable.TabContentSouth, 90f))
      e.Graphics.FillPath((Brush) linearGradientBrush, path1);
    using (SolidBrush solidBrush = new SolidBrush(ColorTable.PanelTextBackground))
      e.Graphics.FillPath((Brush) solidBrush, path2);
    using (Pen pen = new Pen(ColorTable.PanelDarkBorder))
      e.Graphics.DrawPath(pen, path1);
    if (e.Panel.Image != null)
      e.Graphics.DrawImage(e.Panel.Image, rectangle1.Left + (rectangle1.Width - e.Panel.Image.Width) / 2, rectangle1.Top + (rectangle1.Height - r.Height - e.Panel.Image.Height) / 2, e.Panel.Image.Width, e.Panel.Image.Height);
    using (SolidBrush solidBrush = new SolidBrush(this.GetTextColor(e.Panel.Enabled, ColorTable.Text)))
    {
      using (StringFormat format = new StringFormat())
      {
        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Near;
        format.Trimming = StringTrimming.Character;
        e.Graphics.DrawString(e.Panel.Text, e.Ribbon.Font, (Brush) solidBrush, (RectangleF) rectangle2, format);
      }
    }
    Rectangle b1 = this.LargeButtonDropDownArrowBounds(e.Graphics, e.Panel.Owner.Font, e.Panel.Text, rectangle2);
    if (b1.Right < e.Panel.Bounds.Right)
    {
      Rectangle b2 = b1;
      b2.Offset(0, 1);
      Color arrowLight = ColorTable.ArrowLight;
      Color arrow = ColorTable.Arrow;
      this.DrawArrow(e.Graphics, b2, arrowLight, RibbonArrowDirection.Down);
      this.DrawArrow(e.Graphics, b1, arrow, RibbonArrowDirection.Down);
    }
    path1.Dispose();
    path2.Dispose();
  }

  private void DrawPanelOverflowNormal(RibbonPanelRenderEventArgs e)
  {
    Rectangle bounds1 = e.Panel.Bounds;
    int left1 = bounds1.Left;
    bounds1 = e.Panel.Bounds;
    int top1 = bounds1.Top;
    bounds1 = e.Panel.Bounds;
    int right1 = bounds1.Right;
    bounds1 = e.Panel.Bounds;
    int bottom1 = bounds1.Bottom;
    Rectangle r1 = Rectangle.FromLTRB(left1, top1, right1, bottom1);
    Rectangle bounds2 = e.Panel.Bounds;
    int left2 = bounds2.Left + 1;
    bounds2 = e.Panel.Bounds;
    int top2 = bounds2.Top + 1;
    bounds2 = e.Panel.Bounds;
    int right2 = bounds2.Right - 1;
    bounds2 = e.Panel.Bounds;
    int bottom2 = bounds2.Bottom - 1;
    Rectangle r2 = Rectangle.FromLTRB(left2, top2, right2, bottom2);
    GraphicsPath path1 = RibbonRenderer.RoundRectangle(r1, 3, RibbonRenderer.Corners.All);
    GraphicsPath path2 = RibbonRenderer.RoundRectangle(r2, 3, RibbonRenderer.Corners.All);
    using (Pen pen = new Pen(ColorTable.PanelLightBorder))
      e.Graphics.DrawPath(pen, path2);
    using (Pen pen = new Pen(ColorTable.PanelDarkBorder))
      e.Graphics.DrawPath(pen, path1);
    this.DrawPanelOverflowImage(e);
    path1.Dispose();
    path2.Dispose();
  }

  private void DrawPanelOverflowPressed(RibbonPanelRenderEventArgs e)
  {
    Rectangle bounds1 = e.Panel.Bounds;
    int left1 = bounds1.Left;
    bounds1 = e.Panel.Bounds;
    int top1 = bounds1.Top;
    bounds1 = e.Panel.Bounds;
    int right1 = bounds1.Right;
    bounds1 = e.Panel.Bounds;
    int bottom1 = bounds1.Bottom;
    Rectangle r = Rectangle.FromLTRB(left1, top1, right1, bottom1);
    Rectangle bounds2 = e.Panel.Bounds;
    int left2 = bounds2.Left + 1;
    bounds2 = e.Panel.Bounds;
    int top2 = bounds2.Top + 1;
    bounds2 = e.Panel.Bounds;
    int right2 = bounds2.Right - 1;
    bounds2 = e.Panel.Bounds;
    int bottom2 = bounds2.Bottom - 1;
    Rectangle rectangle1 = Rectangle.FromLTRB(left2, top2, right2, bottom2);
    bounds2 = e.Panel.Bounds;
    int left3 = bounds2.Left;
    bounds2 = e.Panel.Bounds;
    int top3 = bounds2.Top;
    bounds2 = e.Panel.Bounds;
    int right3 = bounds2.Right;
    bounds2 = e.Panel.Bounds;
    int bottom3 = bounds2.Top + 17;
    Rectangle rectangle2 = Rectangle.FromLTRB(left3, top3, right3, bottom3);
    GraphicsPath path1 = RibbonRenderer.RoundRectangle(r, 3, RibbonRenderer.Corners.All);
    GraphicsPath path2 = RibbonRenderer.RoundRectangle(rectangle1, 3, RibbonRenderer.Corners.All);
    using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle1, ColorTable.PanelOverflowBackgroundPressed, ColorTable.PanelOverflowBackgroundSelectedSouth, 90f))
    {
      linearGradientBrush.WrapMode = WrapMode.TileFlipXY;
      e.Graphics.FillPath((Brush) linearGradientBrush, path1);
    }
    using (GraphicsPath path3 = RibbonRenderer.RoundRectangle(rectangle2, 3, RibbonRenderer.Corners.North))
    {
      using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle2, Color.FromArgb(150, Color.White), Color.FromArgb(50, Color.White), 90f))
      {
        linearGradientBrush.WrapMode = WrapMode.TileFlipXY;
        e.Graphics.FillPath((Brush) linearGradientBrush, path3);
      }
    }
    using (Pen pen = new Pen(Color.FromArgb(40, Color.White)))
      e.Graphics.DrawPath(pen, path2);
    using (Pen pen = new Pen(ColorTable.PanelDarkBorder))
      e.Graphics.DrawPath(pen, path1);
    this.DrawPanelOverflowImage(e);
    this.DrawPressedShadow(e.Graphics, rectangle2);
    path1.Dispose();
    path2.Dispose();
  }

  private void DrawPanelOveflowSelected(RibbonPanelRenderEventArgs e)
  {
    Rectangle bounds1 = e.Panel.Bounds;
    int left1 = bounds1.Left;
    bounds1 = e.Panel.Bounds;
    int top1 = bounds1.Top;
    bounds1 = e.Panel.Bounds;
    int right1 = bounds1.Right;
    bounds1 = e.Panel.Bounds;
    int bottom1 = bounds1.Bottom;
    Rectangle r = Rectangle.FromLTRB(left1, top1, right1, bottom1);
    Rectangle bounds2 = e.Panel.Bounds;
    int left2 = bounds2.Left + 1;
    bounds2 = e.Panel.Bounds;
    int top2 = bounds2.Top + 1;
    bounds2 = e.Panel.Bounds;
    int right2 = bounds2.Right - 1;
    bounds2 = e.Panel.Bounds;
    int bottom2 = bounds2.Bottom - 1;
    Rectangle rectangle = Rectangle.FromLTRB(left2, top2, right2, bottom2);
    GraphicsPath path1 = RibbonRenderer.RoundRectangle(r, 3, RibbonRenderer.Corners.All);
    GraphicsPath path2 = RibbonRenderer.RoundRectangle(rectangle, 3, RibbonRenderer.Corners.All);
    using (Pen pen = new Pen(ColorTable.PanelLightBorder))
      e.Graphics.DrawPath(pen, path2);
    using (Pen pen = new Pen(ColorTable.PanelDarkBorder))
      e.Graphics.DrawPath(pen, path1);
    using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle, ColorTable.PanelOverflowBackgroundSelectedNorth, Color.Transparent, 90f))
      e.Graphics.FillPath((Brush) linearGradientBrush, path2);
    this.DrawPanelOverflowImage(e);
    path1.Dispose();
    path2.Dispose();
  }

  private void DrawPanelSelected(RibbonPanelRenderEventArgs e)
  {
    Rectangle bounds1 = e.Panel.Bounds;
    int left1 = bounds1.Left;
    bounds1 = e.Panel.Bounds;
    int top1 = bounds1.Top;
    bounds1 = e.Panel.Bounds;
    int right1 = bounds1.Right;
    bounds1 = e.Panel.Bounds;
    int bottom1 = bounds1.Bottom;
    Rectangle r1 = Rectangle.FromLTRB(left1, top1, right1, bottom1);
    Rectangle bounds2 = e.Panel.Bounds;
    int left2 = bounds2.Left + 1;
    bounds2 = e.Panel.Bounds;
    int top2 = bounds2.Top + 1;
    bounds2 = e.Panel.Bounds;
    int right2 = bounds2.Right - 1;
    bounds2 = e.Panel.Bounds;
    int bottom2 = bounds2.Bottom - 1;
    Rectangle r2 = Rectangle.FromLTRB(left2, top2, right2, bottom2);
    Rectangle rectangle = e.Panel.Bounds;
    int left3 = rectangle.Left + 1;
    rectangle = e.Panel.ContentBounds;
    int bottom3 = rectangle.Bottom;
    rectangle = e.Panel.Bounds;
    int right3 = rectangle.Right - 1;
    rectangle = e.Panel.Bounds;
    int bottom4 = rectangle.Bottom - 1;
    Rectangle r3 = Rectangle.FromLTRB(left3, bottom3, right3, bottom4);
    GraphicsPath path1 = RibbonRenderer.RoundRectangle(r1, 3, RibbonRenderer.Corners.All);
    GraphicsPath path2 = RibbonRenderer.RoundRectangle(r2, 3, RibbonRenderer.Corners.All);
    GraphicsPath path3 = RibbonRenderer.RoundRectangle(r3, 3, RibbonRenderer.Corners.South);
    using (Pen pen = new Pen(ColorTable.PanelLightBorder))
      e.Graphics.DrawPath(pen, path2);
    using (Pen pen = new Pen(ColorTable.PanelDarkBorder))
      e.Graphics.DrawPath(pen, path1);
    using (SolidBrush solidBrush = new SolidBrush(ColorTable.PanelBackgroundSelected))
      e.Graphics.FillPath((Brush) solidBrush, path2);
    using (SolidBrush solidBrush = new SolidBrush(ColorTable.PanelTextBackgroundSelected))
      e.Graphics.FillPath((Brush) solidBrush, path3);
    path3.Dispose();
    path1.Dispose();
    path2.Dispose();
  }

  private void DrawPressedShadow(Graphics g, Rectangle r)
  {
    Rectangle rectangle = Rectangle.FromLTRB(r.Left, r.Top, r.Right, r.Top + 4);
    using (GraphicsPath path = RibbonRenderer.RoundRectangle(rectangle, 3, RibbonRenderer.Corners.North))
    {
      using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle, Color.FromArgb(50, Color.Black), Color.FromArgb(0, Color.Black), 90f))
      {
        linearGradientBrush.WrapMode = WrapMode.TileFlipXY;
        g.FillPath((Brush) linearGradientBrush, path);
      }
    }
  }

  private void DrawSplitButton(RibbonItemRenderEventArgs e, RibbonButton button)
  {
  }

  private void DrawSplitButtonDropDownSelected(RibbonItemRenderEventArgs e, RibbonButton button)
  {
    if (button.DropDownBounds.IsEmpty)
      return;
    int left1 = button.DropDownBounds.Left;
    Rectangle rectangle = button.DropDownBounds;
    int top1 = rectangle.Top;
    rectangle = button.DropDownBounds;
    int right1 = rectangle.Right - 1;
    rectangle = button.DropDownBounds;
    int bottom1 = rectangle.Bottom - 1;
    Rectangle r1 = Rectangle.FromLTRB(left1, top1, right1, bottom1);
    Rectangle r2 = Rectangle.FromLTRB(r1.Left + 1, r1.Top + (button.SizeMode == RibbonElementSizeMode.Large ? 1 : 0), r1.Right - 1, r1.Bottom - 1);
    rectangle = button.ButtonFaceBounds;
    int left2 = rectangle.Left;
    rectangle = button.ButtonFaceBounds;
    int top2 = rectangle.Top;
    rectangle = button.ButtonFaceBounds;
    int right2 = rectangle.Right - 1;
    rectangle = button.ButtonFaceBounds;
    int bottom2 = rectangle.Bottom - 1;
    Rectangle r3 = Rectangle.FromLTRB(left2, top2, right2, bottom2);
    Rectangle r4 = Rectangle.FromLTRB(r3.Left + 1, r3.Top + 1, r3.Right + (button.SizeMode == RibbonElementSizeMode.Large ? -1 : 0), r3.Bottom + (button.SizeMode == RibbonElementSizeMode.Large ? 0 : -1));
    RibbonRenderer.Corners corners1 = this.ButtonFaceRounding(button);
    RibbonRenderer.Corners corners2 = this.ButtonDdRounding(button);
    GraphicsPath graphicsPath1 = RibbonRenderer.RoundRectangle(r1, 3, corners2);
    GraphicsPath graphicsPath2 = RibbonRenderer.RoundRectangle(r2, 2, corners2);
    GraphicsPath path1 = RibbonRenderer.RoundRectangle(r3, 3, corners1);
    int num = (int) corners1;
    GraphicsPath path2 = RibbonRenderer.RoundRectangle(r4, 2, (RibbonRenderer.Corners) num);
    using (SolidBrush solidBrush = new SolidBrush(Color.FromArgb(150, Color.White)))
      e.Graphics.FillPath((Brush) solidBrush, path2);
    using (Pen pen = new Pen(!button.Pressed || button.SizeMode == RibbonElementSizeMode.DropDown ? ColorTable.ButtonSelectedBorderIn : ColorTable.ButtonPressedBorderIn))
      e.Graphics.DrawPath(pen, path2);
    using (Pen pen = new Pen(!button.Pressed || button.SizeMode == RibbonElementSizeMode.DropDown ? ColorTable.ButtonSelectedBorderOut : ColorTable.ButtonPressedBorderOut))
      e.Graphics.DrawPath(pen, path1);
    graphicsPath1.Dispose();
    graphicsPath2.Dispose();
    path1.Dispose();
    path2.Dispose();
  }

  private void DrawSplitButtonSelected(RibbonItemRenderEventArgs e, RibbonButton button)
  {
    Rectangle dropDownBounds = button.DropDownBounds;
    int left1 = dropDownBounds.Left;
    dropDownBounds = button.DropDownBounds;
    int top1 = dropDownBounds.Top;
    dropDownBounds = button.DropDownBounds;
    int right1 = dropDownBounds.Right - 1;
    dropDownBounds = button.DropDownBounds;
    int bottom1 = dropDownBounds.Bottom - 1;
    Rectangle r1 = Rectangle.FromLTRB(left1, top1, right1, bottom1);
    Rectangle r2 = Rectangle.FromLTRB(r1.Left + 1, r1.Top + 1, r1.Right - 1, r1.Bottom - 1);
    Rectangle buttonFaceBounds = button.ButtonFaceBounds;
    int left2 = buttonFaceBounds.Left;
    buttonFaceBounds = button.ButtonFaceBounds;
    int top2 = buttonFaceBounds.Top;
    buttonFaceBounds = button.ButtonFaceBounds;
    int right2 = buttonFaceBounds.Right - 1;
    buttonFaceBounds = button.ButtonFaceBounds;
    int bottom2 = buttonFaceBounds.Bottom - 1;
    Rectangle r3 = Rectangle.FromLTRB(left2, top2, right2, bottom2);
    Rectangle r4 = Rectangle.FromLTRB(r3.Left + 1, r3.Top + 1, r3.Right + (button.SizeMode == RibbonElementSizeMode.Large ? -1 : 0), r3.Bottom + (button.SizeMode == RibbonElementSizeMode.Large ? 0 : -1));
    RibbonRenderer.Corners corners1 = this.ButtonFaceRounding(button);
    RibbonRenderer.Corners corners2 = this.ButtonDdRounding(button);
    GraphicsPath path1 = RibbonRenderer.RoundRectangle(r1, 3, corners2);
    GraphicsPath path2 = RibbonRenderer.RoundRectangle(r2, 2, corners2);
    GraphicsPath graphicsPath = RibbonRenderer.RoundRectangle(r3, 3, corners1);
    int num = (int) corners1;
    GraphicsPath path3 = RibbonRenderer.RoundRectangle(r4, 2, (RibbonRenderer.Corners) num);
    using (SolidBrush solidBrush = new SolidBrush(Color.FromArgb(150, Color.White)))
      e.Graphics.FillPath((Brush) solidBrush, path2);
    using (Pen pen = new Pen(!button.Pressed || button.SizeMode == RibbonElementSizeMode.DropDown ? ColorTable.ButtonSelectedBorderOut : ColorTable.ButtonPressedBorderOut))
      e.Graphics.DrawPath(pen, path1);
    using (Pen pen = new Pen(!button.Pressed || button.SizeMode == RibbonElementSizeMode.DropDown ? ColorTable.ButtonSelectedBorderIn : ColorTable.ButtonPressedBorderIn))
      e.Graphics.DrawPath(pen, path3);
    path1.Dispose();
    path2.Dispose();
    graphicsPath.Dispose();
    path3.Dispose();
  }

  public void DrawTabActive(RibbonTabRenderEventArgs e)
  {
    Rectangle r;
    ref Rectangle local = ref r;
    Rectangle tabBounds1 = e.Tab.TabBounds;
    int left = tabBounds1.Left;
    tabBounds1 = e.Tab.TabBounds;
    int top = tabBounds1.Top;
    tabBounds1 = e.Tab.TabBounds;
    int width = tabBounds1.Width;
    local = new Rectangle(left, top, width, 4);
    Rectangle tabBounds2 = e.Tab.TabBounds;
    tabBounds2.Offset(2, 1);
    Rectangle tabBounds3 = e.Tab.TabBounds;
    using (GraphicsPath path = RibbonRenderer.RoundRectangle(tabBounds2, 6, RibbonRenderer.Corners.North))
    {
      using (PathGradientBrush pathGradientBrush = new PathGradientBrush(path))
      {
        pathGradientBrush.WrapMode = WrapMode.Clamp;
        pathGradientBrush.InterpolationColors = new ColorBlend(3)
        {
          Colors = new Color[3]
          {
            Color.Transparent,
            Color.FromArgb(50, Color.Black),
            Color.FromArgb(100, Color.Black)
          },
          Positions = new float[3]{ 0.0f, 0.1f, 1f }
        };
        e.Graphics.FillPath((Brush) pathGradientBrush, path);
      }
    }
    using (GraphicsPath path = RibbonRenderer.RoundRectangle(tabBounds3, 6, RibbonRenderer.Corners.North))
    {
      Color tabNorth = ColorTable.TabNorth;
      Color tabSouth = ColorTable.TabSouth;
      using (Pen pen = new Pen(ColorTable.TabNorth, 1.6f))
        e.Graphics.DrawPath(pen, path);
      using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(e.Tab.TabBounds, ColorTable.TabNorth, ColorTable.TabSouth, 90f))
        e.Graphics.FillPath((Brush) linearGradientBrush, path);
    }
    using (GraphicsPath path = RibbonRenderer.RoundRectangle(r, 6, RibbonRenderer.Corners.North))
    {
      using (Brush brush = (Brush) new SolidBrush(Color.FromArgb(180, Color.White)))
        e.Graphics.FillPath(brush, path);
    }
  }

  public void DrawTabNormal(RibbonTabRenderEventArgs e)
  {
    RectangleF clipBounds = e.Graphics.ClipBounds;
    Rectangle tabBounds1 = e.Tab.TabBounds;
    int left1 = tabBounds1.Left;
    tabBounds1 = e.Tab.TabBounds;
    int top1 = tabBounds1.Top;
    tabBounds1 = e.Tab.TabBounds;
    int right1 = tabBounds1.Right;
    tabBounds1 = e.Tab.TabBounds;
    int bottom1 = tabBounds1.Bottom;
    Rectangle rect1 = Rectangle.FromLTRB(left1, top1, right1, bottom1);
    Rectangle tabBounds2 = e.Tab.TabBounds;
    int left2 = tabBounds2.Left - 1;
    tabBounds2 = e.Tab.TabBounds;
    int top2 = tabBounds2.Top - 1;
    tabBounds2 = e.Tab.TabBounds;
    int right2 = tabBounds2.Right;
    tabBounds2 = e.Tab.TabBounds;
    int bottom2 = tabBounds2.Bottom;
    Rectangle rect2 = Rectangle.FromLTRB(left2, top2, right2, bottom2);
    e.Graphics.SetClip(rect1);
    using (Brush brush = (Brush) new SolidBrush(ColorTable.RibbonBackground))
      e.Graphics.FillRectangle(brush, rect2);
    e.Graphics.SetClip(clipBounds);
  }

  public void DrawTabSelected(RibbonTabRenderEventArgs e)
  {
    Rectangle tabBounds = e.Tab.TabBounds;
    int left = tabBounds.Left;
    tabBounds = e.Tab.TabBounds;
    int top = tabBounds.Top;
    tabBounds = e.Tab.TabBounds;
    int right = tabBounds.Right - 1;
    tabBounds = e.Tab.TabBounds;
    int bottom = tabBounds.Bottom;
    Rectangle r1 = Rectangle.FromLTRB(left, top, right, bottom);
    Rectangle rectangle = Rectangle.FromLTRB(r1.Left + 1, r1.Top + 1, r1.Right - 1, r1.Bottom);
    Rectangle r2 = Rectangle.FromLTRB(rectangle.Left + 1, rectangle.Top + 1, rectangle.Right - 1, rectangle.Top + e.Tab.TabBounds.Height / 2);
    GraphicsPath path1 = RibbonRenderer.RoundRectangle(r1, 3, RibbonRenderer.Corners.North);
    GraphicsPath path2 = RibbonRenderer.RoundRectangle(rectangle, 3, RibbonRenderer.Corners.North);
    GraphicsPath path3 = RibbonRenderer.RoundRectangle(r2, 3, RibbonRenderer.Corners.North);
    using (Pen pen = new Pen(ColorTable.TabBorder))
      e.Graphics.DrawPath(pen, path1);
    using (Pen pen = new Pen(Color.FromArgb(200, Color.White)))
      e.Graphics.DrawPath(pen, path2);
    using (GraphicsPath path4 = new GraphicsPath())
    {
      path4.AddRectangle(rectangle);
      path4.CloseFigure();
      Blend blend = new Blend(3);
      blend.Factors = new float[3]{ 0.0f, 0.9f, 0.0f };
      blend.Positions = new float[3]{ 0.0f, 0.8f, 1f };
      using (PathGradientBrush pathGradientBrush = new PathGradientBrush(path4))
      {
        pathGradientBrush.CenterPoint = new PointF(Convert.ToSingle(rectangle.Left + rectangle.Width / 2), Convert.ToSingle(rectangle.Top - 5));
        pathGradientBrush.CenterColor = Color.Transparent;
        pathGradientBrush.SurroundColors = new Color[1]
        {
          ColorTable.TabSelectedGlow
        };
        pathGradientBrush.Blend = blend;
        e.Graphics.FillPath((Brush) pathGradientBrush, path4);
      }
    }
    using (SolidBrush solidBrush = new SolidBrush(Color.FromArgb(100, Color.White)))
      e.Graphics.FillPath((Brush) solidBrush, path3);
    path1.Dispose();
    path2.Dispose();
    path3.Dispose();
  }

  private Color GetTextColor(bool enabled, Color alternative)
  {
    return !enabled ? ColorTable.ArrowDisabled : alternative;
  }

  private Rectangle LargeButtonDropDownArrowBounds(
    Graphics g,
    Font font,
    string text,
    Rectangle textLayout)
  {
    Rectangle rectangle1 = Rectangle.Empty;
    bool flag = text.Contains(" ");
    using (StringFormat stringFormat = new StringFormat())
    {
      stringFormat.Alignment = StringAlignment.Center;
      stringFormat.LineAlignment = flag ? StringAlignment.Center : StringAlignment.Near;
      stringFormat.Trimming = StringTrimming.EllipsisCharacter;
      stringFormat.SetMeasurableCharacterRanges(new CharacterRange[1]
      {
        new CharacterRange(0, text.Length)
      });
      Region[] regionArray = g.MeasureCharacterRanges(text, font, (RectangleF) textLayout, stringFormat);
      if (regionArray.Length != 0)
      {
        Rectangle rectangle2 = Rectangle.Round(regionArray[regionArray.Length - 1].GetBounds(g));
        rectangle1 = !flag ? new Rectangle(textLayout.Left + (textLayout.Width - this.arrowSize.Width) / 2, rectangle2.Bottom + (textLayout.Bottom - rectangle2.Bottom - this.arrowSize.Height) / 2, this.arrowSize.Width, this.arrowSize.Height) : new Rectangle(rectangle2.Right + 3, rectangle2.Top + (rectangle2.Height - this.arrowSize.Height) / 2, this.arrowSize.Width, this.arrowSize.Height);
      }
    }
    return rectangle1;
  }

  public void OnRenderDropDownBackground(RibbonCanvasEventArgs e)
  {
    Rectangle rect1;
    ref Rectangle local = ref rect1;
    Rectangle bounds = e.Bounds;
    int width = bounds.Width - 1;
    bounds = e.Bounds;
    int height = bounds.Height - 1;
    local = new Rectangle(0, 0, width, height);
    Rectangle rect2 = new Rectangle(0, 0, 26, e.Bounds.Height);
    RibbonDropDown canvas = e.Canvas as RibbonDropDown;
    using (SolidBrush solidBrush = new SolidBrush(ColorTable.DropDownBg))
    {
      e.Graphics.Clear(Color.Transparent);
      SmoothingMode smoothingMode = e.Graphics.SmoothingMode;
      e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
      e.Graphics.FillRectangle((Brush) solidBrush, rect1);
      e.Graphics.SmoothingMode = smoothingMode;
    }
    if (canvas != null && canvas.DrawIconsBar)
    {
      using (SolidBrush solidBrush = new SolidBrush(ColorTable.DropDownImageBg))
        e.Graphics.FillRectangle((Brush) solidBrush, rect2);
      using (Pen pen = new Pen(ColorTable.DropDownImageSeparator))
        e.Graphics.DrawLine(pen, new Point(rect2.Right, rect2.Top), new Point(rect2.Right, rect2.Bottom));
    }
    using (Pen pen = new Pen(ColorTable.DropDownBorder))
    {
      if (canvas != null)
      {
        using (GraphicsPath path = RibbonRenderer.RoundRectangle(new Rectangle(Point.Empty, new Size(canvas.Size.Width - 1, canvas.Size.Height - 1)), canvas.BorderRoundness, RibbonRenderer.Corners.All))
        {
          SmoothingMode smoothingMode = e.Graphics.SmoothingMode;
          e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
          e.Graphics.DrawPath(pen, path);
          e.Graphics.SmoothingMode = smoothingMode;
        }
      }
      else
        e.Graphics.DrawRectangle(pen, rect1);
    }
  }

  public void OnRenderPanelPopupBackground(RibbonCanvasEventArgs e)
  {
    if (!(e.RelatedObject is RibbonPanel relatedObject))
      return;
    Rectangle rectangle = e.Bounds;
    int left1 = rectangle.Left;
    rectangle = e.Bounds;
    int top1 = rectangle.Top;
    rectangle = e.Bounds;
    int right1 = rectangle.Right;
    rectangle = e.Bounds;
    int bottom1 = rectangle.Bottom;
    Rectangle r1 = Rectangle.FromLTRB(left1, top1, right1, bottom1);
    rectangle = e.Bounds;
    int left2 = rectangle.Left + 1;
    rectangle = e.Bounds;
    int top2 = rectangle.Top + 1;
    rectangle = e.Bounds;
    int right2 = rectangle.Right - 1;
    rectangle = e.Bounds;
    int bottom2 = rectangle.Bottom - 1;
    Rectangle r2 = Rectangle.FromLTRB(left2, top2, right2, bottom2);
    rectangle = e.Bounds;
    int left3 = rectangle.Left + 1;
    rectangle = relatedObject.ContentBounds;
    int bottom3 = rectangle.Bottom;
    rectangle = e.Bounds;
    int right3 = rectangle.Right - 1;
    rectangle = e.Bounds;
    int bottom4 = rectangle.Bottom - 1;
    Rectangle r3 = Rectangle.FromLTRB(left3, bottom3, right3, bottom4);
    GraphicsPath path1 = RibbonRenderer.RoundRectangle(r1, 3, RibbonRenderer.Corners.All);
    GraphicsPath path2 = RibbonRenderer.RoundRectangle(r2, 3, RibbonRenderer.Corners.All);
    GraphicsPath path3 = RibbonRenderer.RoundRectangle(r3, 3, RibbonRenderer.Corners.South);
    using (Pen pen = new Pen(ColorTable.PanelLightBorder))
      e.Graphics.DrawPath(pen, path2);
    using (Pen pen = new Pen(ColorTable.PanelDarkBorder))
      e.Graphics.DrawPath(pen, path1);
    using (SolidBrush solidBrush = new SolidBrush(ColorTable.PanelBackgroundSelected))
      e.Graphics.FillPath((Brush) solidBrush, path2);
    using (SolidBrush solidBrush = new SolidBrush(ColorTable.PanelTextBackground))
      e.Graphics.FillPath((Brush) solidBrush, path3);
    path3.Dispose();
    path1.Dispose();
    path2.Dispose();
  }

  public void OnRenderRibbonBackground(RibbonRenderEventArgs e)
  {
    e.Graphics.Clear(ColorTable.RibbonBackground);
  }

  public void OnRenderRibbonItem(RibbonItemRenderEventArgs e)
  {
    if (!(e.Item is RibbonButton))
      return;
    RibbonButton button = e.Item as RibbonButton;
    if (button.Enabled)
    {
      if (button.Style == RibbonButtonStyle.Normal)
      {
        if (button.Pressed && button.SizeMode != RibbonElementSizeMode.DropDown)
          this.DrawButtonPressed(e.Graphics, button.Bounds, this.ButtonCorners(button));
        else if (button.Selected)
          this.DrawButtonSelected(e.Graphics, button.Bounds, this.ButtonCorners(button));
      }
      else if (button.DropDownPressed && button.SizeMode != RibbonElementSizeMode.DropDown)
      {
        this.DrawButtonPressed(e.Graphics, button.Bounds, this.ButtonCorners(button));
        this.DrawSplitButtonDropDownSelected(e, button);
      }
      else if (button.Pressed && button.SizeMode != RibbonElementSizeMode.DropDown)
      {
        this.DrawButtonPressed(e.Graphics, button.Bounds, this.ButtonCorners(button));
        this.DrawSplitButtonSelected(e, button);
      }
      else if (button.DropDownSelected)
      {
        this.DrawButtonSelected(e.Graphics, button.Bounds, this.ButtonCorners(button));
        this.DrawSplitButtonDropDownSelected(e, button);
      }
      else if (button.Selected)
      {
        this.DrawButtonSelected(e.Graphics, button.Bounds, this.ButtonCorners(button));
        this.DrawSplitButtonSelected(e, button);
      }
      else
        this.DrawSplitButton(e, button);
    }
    if (button.Style == RibbonButtonStyle.Normal || button.Style == RibbonButtonStyle.DropDown && button.SizeMode == RibbonElementSizeMode.Large)
      return;
    if (button.Style == RibbonButtonStyle.DropDown)
      this.DrawButtonDropDownArrow(e.Graphics, button, button.OnGetDropDownBounds(button.SizeMode, button.Bounds));
    else
      this.DrawButtonDropDownArrow(e.Graphics, button, button.DropDownBounds);
  }

  public void OnRenderRibbonItemImage(RibbonItemBoundsEventArgs e)
  {
    Image image = e.Item.Image;
    if (e.Item is RibbonButton && e.Item.SizeMode != RibbonElementSizeMode.Large && e.Item.SizeMode != RibbonElementSizeMode.Overflow)
      image = (e.Item as RibbonButton).SmallImage;
    if (image == null)
      return;
    if (!e.Item.Enabled)
      image = RibbonRenderer.CreateDisabledImage(image);
    e.Graphics.DrawImage(image, e.Bounds);
  }

  public void OnRenderRibbonItemText(RibbonTextEventArgs e)
  {
    Color color = e.Color;
    StringFormat format = e.Format;
    Font font1 = e.Ribbon.Font;
    if (e.Item is RibbonButton button && button.Style == RibbonButtonStyle.DropDown && button.SizeMode == RibbonElementSizeMode.Large)
      this.DrawButtonDropDownArrow(e.Graphics, button, e.Bounds);
    if (!e.Item.Enabled)
    {
      Rectangle bounds = e.Bounds;
      ++bounds.Y;
      using (SolidBrush solidBrush = new SolidBrush(ColorTable.ArrowLight))
      {
        using (Font font2 = new Font(font1, e.Style))
          e.Graphics.DrawString(e.Text, font2, (Brush) solidBrush, (RectangleF) bounds, format);
      }
    }
    if (color.Equals((object) Color.Empty))
      color = this.GetTextColor(e.Item.Enabled, ColorTable.Text);
    using (SolidBrush solidBrush = new SolidBrush(color))
      e.Graphics.DrawString(e.Text, new Font(font1, e.Style), (Brush) solidBrush, (RectangleF) e.Bounds, format);
  }

  public void OnRenderRibbonPanelBackground(RibbonPanelRenderEventArgs e)
  {
    if (e.Panel.OverflowMode && !(e.Canvas is RibbonPanelPopup))
    {
      if (e.Panel.Pressed)
        this.DrawPanelOverflowPressed(e);
      else if (e.Panel.Selected)
        this.DrawPanelOveflowSelected(e);
      else
        this.DrawPanelOverflowNormal(e);
    }
    else if (e.Panel.Selected)
      this.DrawPanelSelected(e);
    else
      this.DrawPanelNormal(e);
  }

  public void OnRenderRibbonPanelText(RibbonPanelRenderEventArgs e)
  {
    if (e.Panel.OverflowMode && !(e.Canvas is RibbonPanelPopup) || string.IsNullOrEmpty(e.Panel.Text))
      return;
    int left = e.Panel.Bounds.Left + 1;
    Rectangle rectangle = e.Panel.ContentBounds;
    int bottom1 = rectangle.Bottom;
    rectangle = e.Panel.Bounds;
    int right = rectangle.Right - 1;
    rectangle = e.Panel.Bounds;
    int bottom2 = rectangle.Bottom - 1;
    Rectangle layoutRectangle = Rectangle.FromLTRB(left, bottom1, right, bottom2);
    using (StringFormat format = new StringFormat())
    {
      format.Alignment = StringAlignment.Center;
      format.LineAlignment = StringAlignment.Center;
      using (Brush brush = (Brush) new SolidBrush(this.GetTextColor(e.Panel.Enabled, ColorTable.PanelText)))
        e.Graphics.DrawString(e.Panel.Text, e.Ribbon.Font, brush, (RectangleF) layoutRectangle, format);
    }
  }

  public void OnRenderRibbonTab(RibbonTabRenderEventArgs e)
  {
    if (e.Tab.Active)
      this.DrawCompleteTab(e);
    else if (e.Tab.Selected)
      this.DrawTabSelected(e);
    else
      this.DrawTabNormal(e);
  }

  public void OnRenderRibbonTabText(RibbonTabRenderEventArgs e)
  {
    using (StringFormat format = new StringFormat())
    {
      format.Alignment = StringAlignment.Center;
      format.Trimming = StringTrimming.EllipsisCharacter;
      format.LineAlignment = StringAlignment.Center;
      format.FormatFlags |= StringFormatFlags.NoWrap;
      Rectangle bounds = e.Tab.Bounds;
      if (bounds.Width <= 0)
        return;
      Ribbon ribbon = e.Ribbon;
      int left = bounds.Left + ribbon.TabTextMargin.Left;
      int top1 = bounds.Top;
      Padding tabTextMargin = ribbon.TabTextMargin;
      int top2 = tabTextMargin.Top;
      int top3 = top1 + top2;
      int right1 = bounds.Right;
      tabTextMargin = ribbon.TabTextMargin;
      int right2 = tabTextMargin.Right;
      int right3 = right1 - right2;
      int bottom1 = bounds.Bottom;
      tabTextMargin = ribbon.TabTextMargin;
      int bottom2 = tabTextMargin.Bottom;
      int bottom3 = bottom1 - bottom2;
      Rectangle layoutRectangle = Rectangle.FromLTRB(left, top3, right3, bottom3);
      using (Brush brush = (Brush) new SolidBrush(this.GetTextColor(true, e.Tab.Active ? ColorTable.TabActiveText : ColorTable.TabText)))
      {
        e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        e.Graphics.DrawString(e.Tab.Text, ribbon.Font, brush, (RectangleF) layoutRectangle, format);
      }
    }
  }

  public void OnRenderTabScrollButtons(RibbonTabRenderEventArgs e)
  {
    if (e.Tab.ScrollLeftVisible)
    {
      if (e.Tab.ScrollLeftSelected)
        this.DrawButtonSelected(e.Graphics, e.Tab.ScrollLeftBounds, RibbonRenderer.Corners.West);
      else
        this.DrawButton(e.Graphics, e.Tab.ScrollLeftBounds, RibbonRenderer.Corners.West);
      this.DrawArrowShaded(e.Graphics, e.Tab.ScrollLeftBounds, RibbonArrowDirection.Right, true);
    }
    if (!e.Tab.ScrollRightVisible)
      return;
    if (e.Tab.ScrollRightSelected)
      this.DrawButtonSelected(e.Graphics, e.Tab.ScrollRightBounds, RibbonRenderer.Corners.East);
    else
      this.DrawButton(e.Graphics, e.Tab.ScrollRightBounds, RibbonRenderer.Corners.East);
    this.DrawArrowShaded(e.Graphics, e.Tab.ScrollRightBounds, RibbonArrowDirection.Left, true);
  }

  public enum Corners
  {
    None = 0,
    NorthWest = 2,
    NorthEast = 4,
    North = 6,
    SouthEast = 8,
    East = 12, // 0x0000000C
    SouthWest = 16, // 0x00000010
    West = 18, // 0x00000012
    South = 24, // 0x00000018
    All = 30, // 0x0000001E
  }
}
