// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.Page
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.MaterialsHandbook.Controls.MaterialProperties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

[DesignTimeVisible(false)]
public class Page : ControlWithEditor
{
  private IContainer components;
  private bool _isExpanded = true;
  private TableDescription _clickedTable;
  private bool _selected;

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.SuspendLayout();
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.BackColor = SystemColors.Window;
    this.DoubleBuffered = true;
    this.Name = nameof (Page);
    this.ControlAdded += new ControlEventHandler(this.Page_ControlAdded);
    this.ControlRemoved += new ControlEventHandler(this.Page_ControlRemoved);
    this.MouseDoubleClick += new MouseEventHandler(this.Page_MouseDoubleClick);
    this.ResumeLayout(false);
  }

  public Page() => this.InitializeComponent();

  public Page(
    string caption,
    IEnumerable<DataTable> tables,
    bool drawLines = true,
    bool drawTablesHeader = true,
    bool forbiddenColumnsAdd = false)
    : this()
  {
    this.Header = new HeaderDescription(this.Font, caption, this.AutoScaleFactor)
    {
      Bounds = new Rectangle(0, 0, this.Width, 0)
    };
    this.DrawLines = drawLines;
    this.DrawTablesHeader = drawTablesHeader;
    this.ForbiddenColumnsAdd = forbiddenColumnsAdd;
    if (tables == null)
      this.AddTable();
    else
      tables.ToList<DataTable>().ForEach((Action<DataTable>) (x => this.AddTable(x)));
  }

  public Color ExpandArrowColor { get; set; } = SystemColors.ControlText;

  public Color HeaderColor { get; set; } = SystemColors.ControlLight;

  public Color SelectedElementBorderColor { get; set; } = Color.DodgerBlue;

  public int SpaceBetweenTables { get; set; } = 10;

  public TableDescription ClickedTable
  {
    get => this._clickedTable;
    set
    {
      if (this._clickedTable == value)
        return;
      if (this._clickedTable != null)
        this._clickedTable.Selected = false;
      this._clickedTable = value;
    }
  }

  public Rectangle EditorCtrlBounds => Rectangle.Empty;

  public HeaderDescription Header { get; }

  public bool IsExpanded
  {
    get => this._isExpanded;
    set
    {
      this._isExpanded = value;
      this.Height = this.CalcControlHeight(this.Width);
      this.OnPageSizeChanged();
    }
  }

  public bool IsTableClicked => this._clickedTable != null;

  public bool Selected
  {
    get => this._selected;
    set
    {
      this._selected = value;
      this.OnPageSelected();
    }
  }

  public List<TableDescription> Tables
  {
    get => this.ChildControls.OfType<TableDescription>().ToList<TableDescription>();
  }

  public bool DrawLines { get; set; } = true;

  public bool DrawTablesHeader { get; set; } = true;

  public bool ForbiddenColumnsAdd { get; set; }

  private void Page_ControlAdded(object sender, ControlEventArgs e)
  {
    if (!(e.Control is TableDescription))
      return;
    ((TableDescription) e.Control).BeforeTableSelected += new EventHandler(this.Page_BeforeTableSelected);
    ((TableDescription) e.Control).AfterTableSelected += new EventHandler(this.Page_AfterTableSelected);
    ((TableDescription) e.Control).TableSizeChanged += new EventHandler(this.Page_TableSizeChanged);
    ((ControlWithEditor) e.Control).ValueChanged += new EventHandler(this.Page_ValueChanged);
    ((ControlWithEditor) e.Control).EditorEnter += new EventHandler(this.Page_EditorEnter);
    ((ControlWithEditor) e.Control).EditorLeave += new EventHandler(this.Page_EditorLeave);
  }

  private void Page_EditorLeave(object sender, EventArgs e) => this.OnEditorLeave(sender, e);

  private void Page_EditorEnter(object sender, EventArgs e) => this.OnEditorEnter(sender, e);

  private void Page_ControlRemoved(object sender, ControlEventArgs e)
  {
    if (!(e.Control is TableDescription))
      return;
    ((TableDescription) e.Control).BeforeTableSelected -= new EventHandler(this.Page_BeforeTableSelected);
    ((TableDescription) e.Control).AfterTableSelected -= new EventHandler(this.Page_AfterTableSelected);
    ((TableDescription) e.Control).TableSizeChanged -= new EventHandler(this.Page_TableSizeChanged);
    ((ControlWithEditor) e.Control).ValueChanged -= new EventHandler(this.Page_ValueChanged);
    ((ControlWithEditor) e.Control).EditorEnter -= new EventHandler(this.Page_EditorEnter);
    ((ControlWithEditor) e.Control).EditorLeave -= new EventHandler(this.Page_EditorLeave);
  }

  private void Page_AfterTableSelected(object sender, EventArgs e)
  {
    if (sender is TableDescription tableDescription)
      this.ClickedTable = tableDescription;
    this.Selected = false;
  }

  private void Page_TableSizeChanged(object sender, EventArgs e)
  {
    this.Height = 0;
    this.OnPageSizeChanged();
  }

  private void Page_BeforeTableSelected(object sender, EventArgs e) => this.DeselectTables();

  private int CalcControlHeight(int width)
  {
    int num1 = 0;
    int x = 2;
    if (this.Header == null)
      return num1;
    this.Header.Bounds = new Rectangle(0, 0, width, 0);
    int num2;
    if (this.IsExpanded)
    {
      int y = this.Header.Bounds.Height + this.SpaceBetweenTables;
      foreach (TableDescription table in this.Tables)
      {
        table.Bounds = new Rectangle(x, y, width - 2 * x, 0);
        y += table.Bounds.Height + this.SpaceBetweenTables;
      }
      num2 = y - this.SpaceBetweenTables + x;
    }
    else
      num2 = this.Header.Bounds.Height;
    return num2;
  }

  private DataTable CreateEmptyTable()
  {
    return new DataTable()
    {
      Columns = {
        new DataColumn() { Caption = string.Empty }
      },
      Rows = {
        Array.Empty<object>()
      }
    };
  }

  private void DrawHeader(
    Graphics g,
    Rectangle bounds,
    Font font,
    Rectangle textBounds,
    string text,
    int arrowWidth,
    bool bExpanded)
  {
    ColorBlend colorBlend = new ColorBlend()
    {
      Colors = new Color[3]
      {
        this.HeaderColor,
        SystemColors.ControlLightLight,
        this.HeaderColor
      },
      Positions = new float[3]{ 0.0f, 0.62f, 1f }
    };
    using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(new Point(bounds.Left, bounds.Bottom), new Point(bounds.Left, bounds.Top), Color.White, Color.Black))
    {
      linearGradientBrush.InterpolationColors = colorBlend;
      g.FillRectangle((Brush) linearGradientBrush, bounds);
    }
    using (Pen pen = new Pen(this.ExpandArrowColor))
    {
      pen.Width = 1.5f;
      float x = (float) bounds.X + (float) arrowWidth / 2f;
      float y = (float) bounds.Y + (float) arrowWidth / 2f;
      if (bExpanded)
      {
        PointF[] points1 = new PointF[3]
        {
          new PointF(x, y - 3f),
          new PointF(x + 3f, y),
          new PointF((float) ((double) x + 3.0 + 3.0), y - 3f)
        };
        g.DrawLines(pen, points1);
        PointF[] points2 = new PointF[3]
        {
          new PointF(x, y + 1f),
          new PointF(x + 3f, y + 4f),
          new PointF((float) ((double) x + 3.0 + 3.0), y + 1f)
        };
        g.DrawLines(pen, points2);
      }
      else
      {
        PointF[] points3 = new PointF[3]
        {
          new PointF(x, y),
          new PointF(x + 3f, y - 3f),
          new PointF((float) ((double) x + 3.0 + 3.0), y)
        };
        g.DrawLines(pen, points3);
        PointF[] points4 = new PointF[3]
        {
          new PointF(x, y + 4f),
          new PointF(x + 3f, y + 1f),
          new PointF((float) ((double) x + 3.0 + 3.0), y + 4f)
        };
        g.DrawLines(pen, points4);
      }
    }
    using (Font font1 = new Font(font, FontStyle.Regular))
      TextRenderer.DrawText((IDeviceContext) g, text, font1, textBounds, this.ForeColor, TextFormatFlags.WordBreak);
  }

  private void DrawSelectedRectangle(Graphics g, Rectangle bounds)
  {
    using (Pen pen = new Pen(this.SelectedElementBorderColor, 1.8f))
      g.DrawRectangle(pen, bounds);
  }

  private void Page_FinishEdit(object sender, EventArgs e) => this.OnPageSelected();

  private void Page_ValueChanged(object sender, EventArgs e) => this.OnValueChanged();

  public TableDescription AddTable()
  {
    if (this.ClickedTable == null)
      return this.AddTable(this.CreateEmptyTable());
    int num1 = this.ChildControls.IndexOf((Control) this.ClickedTable);
    int num2;
    return this.AddTable(this.CreateEmptyTable(), num2 = num1 + 1);
  }

  public TableDescription AddTable(DataTable table, int controlIndex = -1)
  {
    TableDescription child = new TableDescription();
    this.ChildControls.Add((Control) child);
    if (controlIndex != -1)
      this.ChildControls.SetChildIndex((Control) child, controlIndex);
    child.DrawLines = this.DrawLines;
    child.DrawTablesHeader = this.DrawTablesHeader;
    child.ForbiddenColumnAdd = this.ForbiddenColumnsAdd;
    child.FillData(table);
    return child;
  }

  public void RemoveClickedTable()
  {
    int childIndex = this.ChildControls.GetChildIndex((Control) this.ClickedTable);
    this.ChildControls.Remove((Control) this.ClickedTable);
    this.ClickedTable = childIndex < this.Tables.Count ? this.Tables[childIndex] : this.Tables[this.Tables.Count - 1];
    this.ClickedTable.Selected = true;
    this.ClickedTable.Focus();
  }

  public void ExpandAllTables(bool b)
  {
    this.Tables.ForEach((Action<TableDescription>) (x => x.IsExpanded = b));
  }

  public void EditValue()
  {
    if (this.Header.ReadOnly)
      return;
    this.OnBeginEdit((object) this.Header.Text);
  }

  public void LostSelection()
  {
    this._selected = false;
    this.DeselectTables();
  }

  public void DeselectTables()
  {
    this.Tables.ForEach((Action<TableDescription>) (x => x.Selected = false));
  }

  public void MoveClickedTableBegin()
  {
    if (this.ClickedTable == null || this.Tables.Count <= 1 || this.ChildControls.GetChildIndex((Control) this.ClickedTable) <= 0)
      return;
    this.ChildControls.SetChildIndex((Control) this.ClickedTable, 0);
    this.Height = 0;
  }

  public void MoveClickedTableUp()
  {
    if (this.ClickedTable == null || this.Tables.Count <= 1)
      return;
    int childIndex = this.ChildControls.GetChildIndex((Control) this.ClickedTable);
    if (childIndex <= 0)
      return;
    int num;
    this.ChildControls.SetChildIndex((Control) this.ClickedTable, num = childIndex - 1);
    this.Height = 0;
  }

  public void MoveClickedTableDown()
  {
    if (this.ClickedTable == null || this.Tables.Count <= 1)
      return;
    int childIndex = this.ChildControls.GetChildIndex((Control) this.ClickedTable);
    if (childIndex >= this.ChildControls.Count - 1)
      return;
    int num;
    this.ChildControls.SetChildIndex((Control) this.ClickedTable, num = childIndex + 1);
    this.Height = 0;
  }

  public void MoveClickedTableEnd()
  {
    if (this.ClickedTable == null || this.Tables.Count <= 1 || this.ChildControls.GetChildIndex((Control) this.ClickedTable) >= this.ChildControls.Count - 1)
      return;
    this.ChildControls.SetChildIndex((Control) this.ClickedTable, this.ChildControls.Count - 1);
    this.Height = 0;
  }

  protected override void SetBoundsCore(
    int x,
    int y,
    int width,
    int height,
    BoundsSpecified specified)
  {
    height = this.CalcControlHeight(width);
    base.SetBoundsCore(x, y, width, height, specified);
  }

  protected override void OnMouseClick(MouseEventArgs e)
  {
    base.OnMouseClick(e);
    this.ClickedTable = (TableDescription) null;
    if (this.Header.Bounds.Contains(e.Location))
    {
      this.IsExpanded = !this.IsExpanded;
    }
    else
    {
      this.OnBeforePageClicked();
      this.Selected = true;
    }
  }

  protected override void OnPaint(PaintEventArgs e)
  {
    base.OnPaint(e);
    this.DrawHeader(e.Graphics, this.Header.Bounds, this.Font, this.Header.TextBounds, this.Header.Text, this.Header.ArrowWidth, this.IsExpanded);
    if (!this.Selected)
      return;
    Rectangle bounds1;
    ref Rectangle local = ref bounds1;
    Rectangle bounds2 = this.Bounds;
    int width = bounds2.Width - 1;
    bounds2 = this.Bounds;
    int height = bounds2.Height - 1;
    local = new Rectangle(0, 0, width, height);
    this.DrawSelectedRectangle(e.Graphics, bounds1);
  }

  protected override Rectangle CalcEditorBounds()
  {
    Rectangle textBounds = this.Header.TextBounds;
    int left = textBounds.Left;
    textBounds = this.Header.TextBounds;
    int top = textBounds.Top;
    textBounds = this.Header.TextBounds;
    int width = textBounds.Width;
    textBounds = this.Header.TextBounds;
    int height = textBounds.Height;
    return new Rectangle(left, top, width, height);
  }

  protected override Color EditorColor() => this.HeaderColor;

  protected override void OnCompleteEdit(object value)
  {
    base.OnCompleteEdit(value);
    this.Header.SetText(value.ToString());
    this.Height = 0;
    this.OnPageSizeChanged();
  }

  protected override void OnEditorLeave(object sender, EventArgs e)
  {
    base.OnEditorLeave(sender, e);
    this.OnPageSelected();
  }

  public event EventHandler PageSelected;

  public event EventHandler PageSizeChanged;

  public event EventHandler BeforePageClicked;

  protected virtual void OnPageSelected()
  {
    EventHandler pageSelected = this.PageSelected;
    if (pageSelected == null)
      return;
    pageSelected((object) this, EventArgs.Empty);
  }

  protected virtual void OnPageSizeChanged()
  {
    EventHandler pageSizeChanged = this.PageSizeChanged;
    if (pageSizeChanged == null)
      return;
    pageSizeChanged((object) this, EventArgs.Empty);
  }

  protected virtual void OnBeforePageClicked()
  {
    EventHandler beforePageClicked = this.BeforePageClicked;
    if (beforePageClicked == null)
      return;
    beforePageClicked((object) this, EventArgs.Empty);
  }

  private void Page_MouseDoubleClick(object sender, MouseEventArgs e) => this.EditValue();
}
