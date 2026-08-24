// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.TreeListView
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class TreeListView : Control
{
  private HScrollBar _hScroll;
  private VScrollBar _vScroll;
  private Rectangle _headerRect = Rectangle.Empty;
  private Rectangle _rowsRect = Rectangle.Empty;
  private List<Rectangle> _columnRects;
  private List<Rectangle> _columnSizeRects;
  private ListDictionary _pmRects = new ListDictionary();
  private ListDictionary _nodeRowRects = new ListDictionary();
  private int _headerHeight = 20;
  private int _rowHeight = 20;
  private int _borderWidth = 2;
  private int _totalColumnsWidth;
  private Color _selectedRowColor = SystemColors.Highlight;
  private Point _lastClickedPoint = Point.Empty;
  private bool _colScaleMode;
  private int _colScaleWidth;
  private int _colScaleIndex = -1;
  private int _rendcnt;
  private Bitmap _bmpMinus;
  private Bitmap _bmpPlus;
  private bool _selectedChanging;

  [Category("Behavior")]
  [Description("The lists column headers.")]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
  [Editor(typeof (CollectionEditor), typeof (UITypeEditor))]
  public ColumnHeaderCollection Columns { get; }

  [Category("Behavior")]
  [Description("The indentation of child nodes in pixels.")]
  [DefaultValue(20)]
  public int Indent { get; set; }

  [Category("Data")]
  [Description("The collection of root nodes in the treelist.")]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
  [Editor(typeof (CollectionEditor), typeof (UITypeEditor))]
  public TreeListNodeCollection Nodes { get; }

  [Browsable(false)]
  public TreeListNode SelectedNode { get; private set; }

  public TreeListView()
  {
    this.Columns = new ColumnHeaderCollection();
    this.Indent = 20;
    this.SetStyle(ControlStyles.UserPaint | ControlStyles.Opaque | ControlStyles.ResizeRedraw | ControlStyles.Selectable | ControlStyles.UserMouse | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
    this.BackColor = SystemColors.Window;
    HScrollBar hscrollBar = new HScrollBar();
    hscrollBar.Parent = (Control) this;
    hscrollBar.Minimum = 0;
    hscrollBar.Maximum = 0;
    hscrollBar.SmallChange = 10;
    this._hScroll = hscrollBar;
    this._hScroll.Hide();
    VScrollBar vscrollBar = new VScrollBar();
    vscrollBar.Parent = (Control) this;
    vscrollBar.Minimum = 0;
    vscrollBar.Maximum = 0;
    vscrollBar.SmallChange = this._rowHeight;
    this._vScroll = vscrollBar;
    this._vScroll.Hide();
    this.Nodes = new TreeListNode()
    {
      Visible = false,
      Control = this
    }.Nodes;
    this.Nodes.NodesChanged += new EventHandler(this.OnNodesChanged);
    this.Nodes.SelectedChanged += new EventHandler(this.On_nodes_SelectedChanged);
    Assembly assembly = Assembly.GetAssembly(Type.GetType("Intermech.MaterialsHandbook.TreeListView") ?? throw new InvalidOperationException());
    this._bmpMinus = ResourceHelper.GetResourceData<Bitmap>(assembly, "Intermech.MaterialsHandbook.Resources.Minus.bmp");
    this._bmpPlus = ResourceHelper.GetResourceData<Bitmap>(assembly, "Intermech.MaterialsHandbook.Resources.Plus.bmp");
    this.Attach();
    this.GenerateBaseRects();
  }

  public event EventHandler SelectedChanged;

  private void OnColumnWidthResize(object sender, EventArgs e) => this.GenerateColumnRects();

  private void OnNodesChanged(object sender, EventArgs e)
  {
    this.AdjustScrollbars();
    this.Invalidate();
  }

  private void OnScroll(object sender, EventArgs e)
  {
    this.GenerateColumnRects();
    this.Invalidate();
  }

  private void On_nodes_SelectedChanged(object sender, EventArgs e)
  {
    if (sender is TreeListNode treeListNode && treeListNode != this.SelectedNode)
    {
      this._selectedChanging = true;
      if (this.SelectedNode != null)
        this.SelectedNode.Selected = false;
      this.SelectedNode = treeListNode;
      this._selectedChanging = false;
      this.OnSelectedChanged(sender, e);
    }
    else if (!this._selectedChanging)
    {
      this.SelectedNode = (TreeListNode) null;
      this.OnSelectedChanged(sender, e);
    }
    this.Invalidate();
  }

  protected override void Dispose(bool disposing)
  {
    base.Dispose(disposing);
    this._bmpMinus?.Dispose();
    this._bmpPlus?.Dispose();
  }

  protected override void OnMouseDown(MouseEventArgs e)
  {
    base.OnMouseDown(e);
    this._lastClickedPoint = new Point(e.X, e.Y);
    if (this._headerRect.Contains(e.Location) && e.Button == MouseButtons.Left)
    {
      Rectangle columnRect;
      for (int index = 0; index < this._columnSizeRects.Count; ++index)
      {
        if (this._columnSizeRects[index].Contains(e.Location))
        {
          this._colScaleMode = true;
          columnRect = this._columnRects[index];
          this._colScaleWidth = columnRect.Width;
          this._colScaleIndex = index;
          break;
        }
      }
      if (this._colScaleMode)
        return;
      SortMode sortMode = SortMode.None;
      int index1 = 0;
      for (int index2 = 0; index2 < this._columnRects.Count; ++index2)
      {
        columnRect = this._columnRects[index2];
        if (columnRect.Contains(e.Location))
        {
          this.Columns[index2].SetSortMode();
          sortMode = this.Columns[index2].SortedMode;
          index1 = index2;
        }
        else
          this.Columns[index2].ClearSort();
      }
      this.Nodes.Sort(sortMode == SortMode.Asc, index1);
      this.AdjustScrollbars();
      this.Invalidate();
    }
    else
    {
      if (!this._rowsRect.Contains(e.Location) || e.Button != MouseButtons.Left)
        return;
      TreeListNode treeListNode1 = this.NodePlusClicked(e);
      if (treeListNode1 != null)
      {
        treeListNode1.Expanded = !treeListNode1.Expanded;
        this.AdjustScrollbars();
        this.Invalidate(this.ClientRectangle);
      }
      else
      {
        TreeListNode treeListNode2 = this.NodeInNodeRow(e);
        if (treeListNode2 == null)
          return;
        if (e.Clicks == 2)
        {
          treeListNode2.Expanded = !treeListNode2.Expanded;
          this.AdjustScrollbars();
          this.Invalidate();
        }
        treeListNode2.Selected = true;
      }
    }
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    base.OnMouseMove(e);
    Cursor.Current = Cursors.Default;
    if (this._colScaleMode)
    {
      Cursor.Current = Cursors.VSplit;
      int num = e.X - this._lastClickedPoint.X;
      this.Columns[this._colScaleIndex].Width = num + this._colScaleWidth <= 0 ? 1 : num + this._colScaleWidth;
      this.Invalidate();
    }
    else
    {
      if (!this._headerRect.Contains(e.Location) || this._columnSizeRects == null)
        return;
      foreach (Rectangle columnSizeRect in this._columnSizeRects)
      {
        if (columnSizeRect.Contains(e.Location))
        {
          Cursor.Current = Cursors.VSplit;
          break;
        }
      }
    }
  }

  protected override void OnMouseUp(MouseEventArgs e)
  {
    base.OnMouseUp(e);
    this._lastClickedPoint = Point.Empty;
    if (!this._colScaleMode)
      return;
    this._colScaleMode = false;
    this._colScaleWidth = 0;
    this._colScaleIndex = -1;
    this.AdjustScrollbars();
  }

  protected override void OnMouseWheel(MouseEventArgs e)
  {
    ScrollBar scrollBar = this._vScroll.Visible ? (ScrollBar) this._vScroll : (this._hScroll.Visible ? (ScrollBar) this._hScroll : (ScrollBar) null);
    if (scrollBar == null)
      return;
    if (e.Delta > 0)
    {
      scrollBar.Value = scrollBar.Value - scrollBar.SmallChange * (e.Delta / 100) < 0 ? 0 : scrollBar.Value - scrollBar.SmallChange * (e.Delta / 100);
    }
    else
    {
      if (e.Delta >= 0)
        return;
      scrollBar.Value = scrollBar.Value - scrollBar.SmallChange * (e.Delta / 100) > scrollBar.Maximum - scrollBar.LargeChange ? scrollBar.Maximum - scrollBar.LargeChange : scrollBar.Value - scrollBar.SmallChange * (e.Delta / 100);
    }
  }

  protected override void OnPaint(PaintEventArgs e)
  {
    Rectangle clientRectangle = this.ClientRectangle;
    Graphics graphics = e.Graphics;
    this.DrawBackground(graphics, clientRectangle);
    this.DrawRows(graphics, clientRectangle);
    this.DrawHeader(graphics, clientRectangle);
    this.DrawExtra(graphics, clientRectangle);
    this.DrawBorder(graphics, clientRectangle);
  }

  protected override void OnResize(EventArgs e)
  {
    base.OnResize(e);
    this.GenerateBaseRects();
    this.AdjustScrollbars();
  }

  private void AdjustScrollbars()
  {
    if (this.Nodes.Count <= 0 && this.Columns.Count <= 0)
      return;
    this._totalColumnsWidth = 0;
    for (int index = 0; index < this.Columns.Count; ++index)
      this._totalColumnsWidth += this.Columns[index].Width;
    int num1 = 0;
    foreach (TreeListNode node in (List<TreeListNode>) this.Nodes)
      num1 += this._rowHeight + this._rowHeight * node.GetVisibleNodeCount(true);
    int width = this._vScroll.Width;
    int num2 = this._hScroll.Height;
    this._hScroll.Left = this.ClientRectangle.Left + this._borderWidth;
    this._hScroll.Top = this.ClientRectangle.Height - this._hScroll.Height - this._borderWidth;
    this._hScroll.Width = this.ClientRectangle.Width - width - this._borderWidth * 2;
    this._hScroll.Maximum = this._totalColumnsWidth;
    int totalColumnsWidth1 = this._totalColumnsWidth;
    Rectangle clientRectangle = this.ClientRectangle;
    int num3 = clientRectangle.Width - width - this._borderWidth * 2;
    if (totalColumnsWidth1 > num3)
    {
      this._hScroll.Show();
    }
    else
    {
      this._hScroll.Hide();
      this._hScroll.Value = 0;
      num2 = 0;
    }
    VScrollBar vScroll1 = this._vScroll;
    clientRectangle = this.ClientRectangle;
    int num4 = clientRectangle.Width - this._vScroll.Width - this._borderWidth;
    vScroll1.Left = num4;
    VScrollBar vScroll2 = this._vScroll;
    clientRectangle = this.ClientRectangle;
    int num5 = clientRectangle.Top + this._headerHeight + this._borderWidth;
    vScroll2.Top = num5;
    VScrollBar vScroll3 = this._vScroll;
    clientRectangle = this.ClientRectangle;
    int num6 = clientRectangle.Height - this._headerHeight - num2 - this._borderWidth * 2;
    vScroll3.Height = num6;
    VScrollBar vScroll4 = this._vScroll;
    clientRectangle = this.ClientRectangle;
    int num7;
    if (clientRectangle.Height - this._headerHeight - num2 - this._borderWidth * 2 <= 0)
    {
      num7 = 0;
    }
    else
    {
      clientRectangle = this.ClientRectangle;
      num7 = clientRectangle.Height - this._headerHeight - num2 - this._borderWidth * 2;
    }
    vScroll4.LargeChange = num7;
    this._vScroll.Maximum = num1;
    int num8 = num1;
    clientRectangle = this.ClientRectangle;
    int num9 = clientRectangle.Height - num2 - this._headerHeight - this._borderWidth * 2;
    int num10;
    if (num8 > num9)
    {
      this._vScroll.Show();
      num10 = this._vScroll.Width;
    }
    else
    {
      this._vScroll.Hide();
      this._vScroll.Value = 0;
      num10 = 0;
    }
    HScrollBar hScroll1 = this._hScroll;
    clientRectangle = this.ClientRectangle;
    int num11 = clientRectangle.Width - num10 - this._borderWidth * 2;
    hScroll1.Width = num11;
    HScrollBar hScroll2 = this._hScroll;
    clientRectangle = this.ClientRectangle;
    int num12;
    if (clientRectangle.Width - num10 - this._borderWidth * 2 <= 0)
    {
      num12 = 0;
    }
    else
    {
      clientRectangle = this.ClientRectangle;
      num12 = clientRectangle.Width - num10 - this._borderWidth * 2;
    }
    hScroll2.LargeChange = num12;
    int totalColumnsWidth2 = this._totalColumnsWidth;
    clientRectangle = this.ClientRectangle;
    int num13 = clientRectangle.Width - num10 - this._borderWidth * 2;
    if (totalColumnsWidth2 > num13)
    {
      this._hScroll.Show();
    }
    else
    {
      this._hScroll.Hide();
      this._hScroll.Value = 0;
    }
  }

  private void Attach()
  {
    this.Columns.WidthResized += new EventHandler(this.OnColumnWidthResize);
    this._hScroll.ValueChanged += new EventHandler(this.OnScroll);
    this._vScroll.ValueChanged += new EventHandler(this.OnScroll);
  }

  private void Detach()
  {
    this.Columns.WidthResized -= new EventHandler(this.OnColumnWidthResize);
    this._hScroll.ValueChanged -= new EventHandler(this.OnScroll);
    this._vScroll.ValueChanged -= new EventHandler(this.OnScroll);
  }

  private void DrawBackground(Graphics g, Rectangle r)
  {
    using (SolidBrush solidBrush = new SolidBrush(this.BackColor))
      g.FillRectangle((Brush) solidBrush, r);
  }

  private void DrawBorder(Graphics g, Rectangle r)
  {
    g.ResetClip();
    ControlPaint.DrawBorder3D(g, r.Left, r.Top, r.Width, r.Height, Border3DStyle.Sunken);
  }

  private void DrawExtra(Graphics g, Rectangle r)
  {
    if (!this._hScroll.Visible || !this._vScroll.Visible)
      return;
    g.ResetClip();
    g.FillRectangle(SystemBrushes.Control, r.Width - this._vScroll.Width - this._borderWidth, r.Height - this._hScroll.Height - this._borderWidth, this._vScroll.Width, this._hScroll.Height);
  }

  private void DrawHeader(Graphics g, Rectangle r)
  {
    int x = r.Left + this._borderWidth;
    int y = r.Top + this._borderWidth;
    using (SolidBrush solidBrush = new SolidBrush(SystemColors.Control))
      g.FillRectangle((Brush) solidBrush, x, y, r.Width, this._headerHeight);
    Rectangle rectangle = new Rectangle(x, y, r.Width - this._borderWidth * 2, this._headerHeight);
    g.Clip = new Region(rectangle);
    if (this.Columns.Count == 0)
    {
      ControlPaint.DrawButton(g, rectangle, ButtonState.Normal);
    }
    else
    {
      int num1 = r.Left - this._hScroll.Value;
      int borderWidth = this._borderWidth;
      for (int index = 0; index < this.Columns.Count; ++index)
      {
        ColumnHeader column = this.Columns[index];
        int num2 = num1 + borderWidth;
        if (num2 + column.Width > x && num2 < r.Left + r.Width - this._borderWidth)
        {
          ControlPaint.DrawButton(g, num2, y, column.Width, this._headerHeight, ButtonState.Normal);
          int num3 = this.DrawSortSymbol(g, num2, column.Width, column.SortedMode) ? 14 : 0;
          g.DrawString(this.TruncatedString(column.Text, column.Width - num3, 0, g), this.Font, SystemBrushes.ControlText, (float) (num2 + 4), (float) (r.Top + this._borderWidth + 3));
        }
        borderWidth += this.Columns[index].Width;
      }
    }
  }

  private bool DrawSortSymbol(Graphics g, int startX, int columnWidth, SortMode mode)
  {
    bool flag = false;
    float x = (float) (startX + columnWidth - 14);
    int num1;
    switch (mode)
    {
      case SortMode.None:
label_9:
        return flag;
      case SortMode.Asc:
        num1 = 1;
        break;
      default:
        num1 = -1;
        break;
    }
    int num2 = num1;
    float y = (float) this._borderWidth + (float) this._headerHeight / 2f + (float) (2 * num2);
    PointF pointF1 = new PointF(x, y);
    PointF pointF2 = new PointF(x + 5f, y - (float) (5 * num2));
    PointF pointF3 = new PointF(x + 10f, y);
    PointF pointF4 = new PointF(x, y);
    using (Brush brush = (Brush) new SolidBrush(Color.Black))
      g.FillPolygon(brush, new PointF[4]
      {
        pointF1,
        pointF2,
        pointF3,
        pointF4
      });
    flag = true;
    goto label_9;
  }

  private void DrawRows(Graphics g, Rectangle r)
  {
    int childCount = 0;
    int num1 = r.Height / this._rowHeight + 1;
    int num2 = this._vScroll.Value / this._rowHeight - 1;
    int totalRend = 0;
    int num3 = 0;
    if (num2 > 0)
    {
      for (int index = 0; index < this.Nodes.Count; ++index)
      {
        int num4 = 0;
        if (this.Nodes[index].Expanded)
          num4 = this.Nodes[index].GetVisibleNodeCount(true);
        totalRend += num4 + 1;
        if (totalRend > num2)
        {
          totalRend -= num4 + 1;
          num3 = index;
          break;
        }
      }
    }
    this._rendcnt = 0;
    this._nodeRowRects.Clear();
    this._pmRects.Clear();
    for (int index = num3; index < this.Nodes.Count && this._rendcnt < num1; ++index)
      this.RenderNodeRows(this.Nodes[index], g, r, 0, index, ref totalRend, ref childCount, this.Nodes.Count);
  }

  private void GenerateBaseRects()
  {
    int x = this.ClientRectangle.Left + this._borderWidth - this._hScroll.Value;
    int y = this.ClientRectangle.Top + this._borderWidth;
    int width = this.ClientRectangle.Width - this._borderWidth * 2;
    this._headerRect = new Rectangle(x, y, width, this._headerHeight);
    this._rowsRect = new Rectangle(x, y + this._headerHeight, width, this.ClientRectangle.Height - this._borderWidth * 2 - this._headerHeight);
  }

  private void GenerateColumnRects()
  {
    this._columnRects = new List<Rectangle>(this.Columns.Count);
    this._columnSizeRects = new List<Rectangle>(this.Columns.Count);
    int x = this._borderWidth - this._hScroll.Value;
    this._totalColumnsWidth = 0;
    for (int index = 0; index < this.Columns.Count; ++index)
    {
      int width = this.Columns[index].Width;
      this._columnRects.Add(new Rectangle(x, this._borderWidth, width, this._headerHeight));
      this._columnSizeRects.Add(new Rectangle(x + width - 4, this._borderWidth, 4, this._headerHeight));
      int num;
      x += num = width + 1;
      this._totalColumnsWidth += num;
    }
  }

  private TreeListNode NodeInNodeRow(MouseEventArgs e)
  {
    TreeListNode treeListNode = (TreeListNode) null;
    IEnumerator enumerator1 = this._nodeRowRects.Keys.GetEnumerator();
    IEnumerator enumerator2 = this._nodeRowRects.Values.GetEnumerator();
    while (enumerator1.MoveNext() && enumerator2.MoveNext())
    {
      if (enumerator1.Current != null && ((Rectangle) enumerator1.Current).Contains(e.Location))
        treeListNode = (TreeListNode) enumerator2.Current;
    }
    return treeListNode;
  }

  private TreeListNode NodePlusClicked(MouseEventArgs e)
  {
    TreeListNode treeListNode = (TreeListNode) null;
    IEnumerator enumerator1 = this._pmRects.Keys.GetEnumerator();
    IEnumerator enumerator2 = this._pmRects.Values.GetEnumerator();
    while (enumerator1.MoveNext() && enumerator2.MoveNext())
    {
      if (enumerator1.Current != null && ((Rectangle) enumerator1.Current).Contains(e.Location))
      {
        treeListNode = (TreeListNode) enumerator2.Current;
        break;
      }
    }
    return treeListNode;
  }

  private void OnSelectedChanged(object sender, EventArgs e)
  {
    EventHandler selectedChanged = this.SelectedChanged;
    if (selectedChanged == null)
      return;
    selectedChanged(sender, e);
  }

  private void RenderNodeRows(
    TreeListNode node,
    Graphics g,
    Rectangle r,
    int level,
    int index,
    ref int totalRend,
    ref int childCount,
    int count)
  {
    if (node.Visible)
    {
      int num1 = this._rowHeight * totalRend;
      if (this._borderWidth + num1 - this._vScroll.Value + this._rowHeight > 0 && this._borderWidth + num1 - this._vScroll.Value < r.Height)
      {
        ++this._rendcnt;
        int num2 = this.Indent * level + this._borderWidth;
        int num3 = 20;
        int num4 = r.Left + num2;
        Rectangle rectangle = new Rectangle(num4 + num3 - this._hScroll.Value, r.Top + this._headerHeight + this._borderWidth + num1 - this._vScroll.Value, this._totalColumnsWidth - (num2 + num3), this._rowHeight);
        this._nodeRowRects.Add((object) rectangle, (object) node);
        if (node.BackColor != this.BackColor)
        {
          using (SolidBrush solidBrush = new SolidBrush(node.BackColor))
            g.FillRectangle((Brush) solidBrush, rectangle);
        }
        using (Region region = new Region(rectangle))
        {
          g.Clip = region;
          if (node.Selected)
          {
            using (SolidBrush solidBrush = new SolidBrush(this._selectedRowColor))
              g.FillRectangle((Brush) solidBrush, rectangle);
          }
        }
        using (Region region = new Region(new Rectangle(r.Left + this._borderWidth - this._hScroll.Value, r.Top + this._borderWidth + this._headerHeight, this.Columns[0].Width, r.Height - this._headerHeight - 4)))
        {
          g.Clip = region;
          if (num4 + num3 - this._hScroll.Value > r.Left && level > 0)
          {
            using (Pen pen = new Pen(SystemBrushes.ControlDark, 1f))
            {
              pen.DashStyle = DashStyle.Dot;
              int x = num4 + num3 / 2 - this._hScroll.Value;
              int y = r.Top + num3 / 2 + this._headerHeight + num1 - this._vScroll.Value;
              Point point = new Point(x, r.Top + this._headerHeight + num1 - this._vScroll.Value);
              Point pt1 = new Point(x, y);
              g.DrawLine(pen, point, index == count - 1 ? pt1 : new Point(x, r.Top + num3 + this._headerHeight + num1 - this._vScroll.Value));
              g.DrawLine(pen, pt1, new Point(num4 + num3 - this._hScroll.Value, y));
              if (childCount > 0)
                g.DrawLine(pen, new Point(x, r.Top + this._headerHeight + this._rowHeight * (totalRend - childCount) - this._vScroll.Value), point);
            }
          }
          if (num4 + num3 / 2 + 5 - this._hScroll.Value > r.Left && node.GetNodeCount(false) > 0)
          {
            int x = num4 + num3 / 2 - 4 - this._hScroll.Value;
            int y = r.Top + this._headerHeight + num3 / 2 - 4 - this._vScroll.Value + (index != 0 || level != 0 ? num1 : 0);
            g.DrawImage(node.Expanded ? (Image) this._bmpMinus : (Image) this._bmpPlus, x, y);
            this._pmRects.Add((object) new Rectangle(x, y, 8, 8), (object) node);
          }
          if (this.Columns[0].Width - this._hScroll.Value > 0)
          {
            string s = this.TruncatedString(node.Text, this.Columns[0].Width, num2 + num3 + 6, g);
            float x = (float) (num4 + num3 + 4 - this._hScroll.Value);
            float y = (float) ((double) r.Top + (double) this._headerHeight + (double) num1 + (double) num3 / 4.0) - (float) this._vScroll.Value;
            if (node.Selected)
            {
              g.DrawString(s, this.Font, SystemBrushes.HighlightText, x, y);
            }
            else
            {
              using (SolidBrush solidBrush = new SolidBrush(node.ForeColor))
                g.DrawString(s, this.Font, (Brush) solidBrush, x, y);
            }
          }
        }
        if (this.Columns.Count > 0)
        {
          int num5 = 0;
          for (int index1 = 0; index1 < node.SubNodes.Count && index1 < this.Columns.Count - 1; ++index1)
          {
            num5 += this.Columns[index1].Width;
            using (Region region = new Region(new Rectangle(num5 + 6 - this._hScroll.Value, r.Top + this._headerHeight + this._borderWidth, num5 + this.Columns[index1 + 1].Width > r.Width - 6 ? r.Width - 6 : this.Columns[index1 + 1].Width - 6, r.Height - 5)))
            {
              g.Clip = region;
              string s = this.TruncatedString(node.SubNodes[index1].Text, this.Columns[index1 + 1].Width, 9, g);
              float x = (float) (num5 + 6 - this._hScroll.Value);
              float y = (float) (r.Top + num1 + this._headerHeight + 4 - this._vScroll.Value);
              using (SolidBrush solidBrush = new SolidBrush(node.ForeColor))
                g.DrawString(s, this.Font, node.Selected ? SystemBrushes.HighlightText : (Brush) solidBrush, x, y);
            }
          }
        }
      }
      ++totalRend;
      if (node.Expanded)
      {
        childCount = 0;
        for (int index2 = 0; index2 < node.GetNodeCount(false); ++index2)
          this.RenderNodeRows(node.Nodes[index2], g, r, level + 1, index2, ref totalRend, ref childCount, node.Nodes.Count);
      }
      childCount = node.GetVisibleNodeCount(true);
    }
    else
      childCount = 0;
  }

  private string TruncatedString(string text, int width, int offset, Graphics g)
  {
    string str = string.Empty;
    try
    {
      int width1 = (int) g.MeasureString(text, this.Font).Width;
      int length;
      for (length = text.Length; length > 0 && width1 > width - offset; --length)
        width1 = (int) g.MeasureString(text.Substring(0, length), this.Font).Width;
      str = length < text.Length ? text.Substring(0, length - 3 <= 0 ? 1 : length - 3) + "..." : text.Substring(0, length);
    }
    catch
    {
    }
    return str;
  }

  public void CollapseAll()
  {
    foreach (TreeListNode node in (List<TreeListNode>) this.Nodes)
      node.CollapseAll();
    this.AdjustScrollbars();
    this.Invalidate();
  }

  public void ExpandAll()
  {
    foreach (TreeListNode node in (List<TreeListNode>) this.Nodes)
      node.ExpandAll();
    this.AdjustScrollbars();
    this.Invalidate();
  }
}
