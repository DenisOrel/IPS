// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.Controls.MaterialProperties.TableDescription
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Imbase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook.Controls.MaterialProperties;

[DesignTimeVisible(false)]
public class TableDescription : ControlWithEditor
{
  private bool _isExpanded = true;
  private TableDescription.CellData[,] _data;
  private int _textHeight;
  private int _headerDelta;
  private int _bottomDelta;
  private bool _selected;
  private int _columnClicked = -1;
  private int _rowClicked = -1;
  private Dictionary<int, TypeConverter> _convs = new Dictionary<int, TypeConverter>();
  private Dictionary<int, Dictionary<string, string>> _recordLinksDicts = new Dictionary<int, Dictionary<string, string>>();
  private IContainer components;

  public TableDescription()
  {
    this.InitializeComponent();
    this.ColumnsCount = 0;
    this.ColumnWidth = 0;
    this.PreRowBounds = Rectangle.Empty;
    this.RowsCount = 0;
    this.UnitedRowIndex = new List<int>(0);
    this.Header = new HeaderDescription()
    {
      Visible = false
    };
  }

  public bool DrawTablesHeader { get; set; } = true;

  public bool CanRemoveColumn => this.Table != null && this.Table.Columns.Count > 1;

  public bool CanRemoveRows => this.Table != null && this.Table.Rows.Count > 1;

  public Padding CellPadding { get; private set; }

  public int ColumnClicked
  {
    get => this._columnClicked;
    set
    {
      if (value > -1)
        this._columnClicked = value < this.Table.Columns.Count ? value : this.Table.Columns.Count - 1;
      else
        this._columnClicked = -1;
    }
  }

  public int ColumnsCount { get; private set; }

  public int ColumnWidth { get; private set; }

  public HeaderDescription Header { get; }

  public bool IsExpanded
  {
    get => this._isExpanded;
    set
    {
      if (value)
      {
        this._isExpanded = true;
        int h = this.Header.Visible ? this.Header.Bounds.Height + this._headerDelta : 0;
        this.RowsHeight.ForEach((Action<int>) (x => h += x));
        this.Height = h + this._bottomDelta;
      }
      else if (this.Header.Visible && !string.IsNullOrEmpty(this.Header.Text))
      {
        this._isExpanded = false;
        this.Height = this.Header.Bounds.Height + this._headerDelta;
      }
      this.OnTableSizeChanged();
    }
  }

  public int PreRowBoundOffset { get; set; } = 15;

  public Rectangle PreRowBounds { get; private set; }

  public int RowClicked
  {
    get => this._rowClicked;
    set
    {
      if (value > -1)
      {
        if (this.DrawTablesHeader)
        {
          this._rowClicked = value <= this.Table.Rows.Count ? value : this.Table.Rows.Count;
        }
        else
        {
          if (value >= this.Table.Rows.Count)
            return;
          this._rowClicked = value;
        }
      }
      else
        this._rowClicked = -1;
    }
  }

  public int RowsCount { get; private set; }

  public List<int> RowsHeight { get; private set; }

  public bool Selected
  {
    get => this._selected;
    set
    {
      this._selected = value;
      if (!value)
      {
        this.ClearClicked();
        this.Header.Visible = !string.IsNullOrEmpty(this.Header.Text);
        if (this.Header.Visible || this._isExpanded)
          return;
        this._isExpanded = true;
      }
      else
      {
        if (!string.IsNullOrEmpty(this.Header.Text))
          return;
        this.Header.Visible = this._rowClicked == -1 && this._columnClicked == -1;
      }
    }
  }

  public SelectedElement SelectedElement
  {
    get
    {
      return this._columnClicked != -1 ? (this._rowClicked > -1 ? SelectedElement.Cell : SelectedElement.Column) : (this._rowClicked > -1 ? SelectedElement.Row : SelectedElement.Table);
    }
  }

  public DataTable Table { get; private set; }

  public Rectangle TableBounds
  {
    get
    {
      int num = 2;
      int height = this.Header.Visible ? this.Header.Bounds.Height : 0;
      return new Rectangle(this.PreRowBoundOffset, height, this.Width - (this.PreRowBoundOffset + 2 * num), this.Height - (height + num));
    }
  }

  public List<int> UnitedRowIndex { get; }

  public bool ForbiddenColumnAdd { get; set; }

  public bool ForbiddenRowAdd { get; set; }

  protected override Color EditorColor()
  {
    return this.RowClicked <= -1 ? this.HeaderColor : this.BackColor;
  }

  public void EditValue(bool startEdit = false)
  {
    object text;
    if (this.ColumnClicked > -1)
    {
      if (this._convs.ContainsKey(this.ColumnClicked) && this.RowClicked != -1)
      {
        this._Editor.Converter = (TypeConverter) new TableRefTypeConverter();
        this._Editor.Editor = (UITypeEditor) new TableRefTypeEditor();
        text = this.GetValue(this.ColumnClicked, this.RowClicked);
      }
      else
      {
        if (this.DrawTablesHeader && this.Header.ReadOnly)
          return;
        this._Editor.UseDefaultConverter = true;
        this._Editor.UseDefaultEditor = true;
        this._Editor.ValueType = typeof (string);
        text = this.GetValue(this.ColumnClicked, this.RowClicked == -1 ? 0 : this.RowClicked);
      }
    }
    else if (this.RowClicked > -1)
    {
      if (this._convs.ContainsKey(this.ColumnClicked))
      {
        this._Editor.Converter = (TypeConverter) new TableRefTypeConverter();
        this._Editor.Editor = (UITypeEditor) new TableRefTypeEditor();
        text = this.GetValue(this.ColumnClicked, this.RowClicked);
      }
      else
      {
        this._Editor.UseDefaultConverter = true;
        this._Editor.UseDefaultEditor = true;
        this._Editor.ValueType = typeof (string);
        text = this.GetValue(this.ColumnClicked, this.RowClicked);
      }
    }
    else
    {
      if (this.Header.ReadOnly)
        return;
      this._Editor.UseDefaultConverter = true;
      this._Editor.UseDefaultEditor = true;
      this._Editor.ValueType = typeof (string);
      text = (object) this.Header.Text;
    }
    this.OnBeginEdit(text, startEdit);
  }

  protected override Rectangle CalcEditorBounds()
  {
    int int32_1 = Convert.ToInt32(Math.Ceiling(3.0 * (double) this.AutoScaleFactor.Width));
    int int32_2 = Convert.ToInt32(Math.Ceiling(3.0 * (double) this.AutoScaleFactor.Height));
    Rectangle rectangle;
    if (this.ColumnClicked > -1)
    {
      int x = this.TableBounds.X + this.ColumnWidth * this.ColumnClicked + int32_1;
      if (this.RowClicked == -1)
      {
        rectangle = new Rectangle(x, this.TableBounds.Y + int32_2, this.ColumnWidth - int32_1, this.RowsHeight[0] - int32_2);
      }
      else
      {
        int y = this.TableBounds.Y + int32_2;
        for (int index = 0; index < this.RowClicked; ++index)
          y += this.RowsHeight[index];
        rectangle = new Rectangle(x, y, this.ColumnWidth - int32_1, this.RowsHeight[this.RowClicked] - int32_2);
      }
    }
    else if (this.RowClicked > -1)
    {
      Rectangle tableBounds = this.TableBounds;
      int num1 = tableBounds.X + int32_1;
      tableBounds = this.TableBounds;
      int num2 = tableBounds.Y + int32_2;
      for (int index = 0; index < this.RowClicked; ++index)
        num2 += this.RowsHeight[index];
      ref Rectangle local = ref rectangle;
      int x = num1;
      int y = num2;
      tableBounds = this.TableBounds;
      int width = tableBounds.Width - int32_1;
      int height = this.RowsHeight[this.RowClicked] - int32_2;
      local = new Rectangle(x, y, width, height);
    }
    else
    {
      ref Rectangle local = ref rectangle;
      int left = this.Header.TextBounds.Left;
      Rectangle textBounds = this.Header.TextBounds;
      int top = textBounds.Top;
      textBounds = this.Header.TextBounds;
      int width = textBounds.Width;
      int height = this.Header.TextBounds.Height;
      local = new Rectangle(left, top, width, height);
    }
    return rectangle;
  }

  public Padding TablePadding { get; set; }

  public bool DrawLines { get; set; } = true;

  public Color ExpandArrowColor { get; set; } = Color.Black;

  public Color HeaderColor { get; set; } = SystemColors.ControlLight;

  public Color SelectedElementBorderColor { get; set; } = Color.DodgerBlue;

  private int CalcControlHeight(int width)
  {
    int num1 = 0;
    int num2 = 2;
    if (this.Header != null && this.Header.Visible)
    {
      this.Header.Bounds = new Rectangle(this.PreRowBoundOffset + num2, 0, width - this.PreRowBoundOffset - 2 * num2, 0);
      num1 = this.Header.Bounds.Height;
    }
    if (this._isExpanded && this._data != null)
    {
      this.RowsHeight.Clear();
      this.ColumnWidth = (width - this.PreRowBoundOffset - 2 * num2) / this.ColumnsCount;
      for (int index1 = 0; index1 < this.RowsCount; ++index1)
      {
        int num3 = 1;
        if (this.UnitedRowIndex.BinarySearch(index1) > -1)
        {
          TableDescription.CellData cellData = this._data[0, index1];
          if (!cellData.IsEmpty)
          {
            int num4 = 0;
            foreach (int num5 in cellData.Widht)
              num4 += Convert.ToInt32(Math.Ceiling((double) num5 / ((double) width - (double) this.PreRowBoundOffset - (double) this.CellPadding.Horizontal - (double) (2 * num2))));
            num3 = num4 != 0 ? num4 : 1;
          }
        }
        else
        {
          for (int index2 = 0; index2 < this.ColumnsCount; ++index2)
          {
            TableDescription.CellData cellData = this._data[index2, index1];
            if (!cellData.IsEmpty)
            {
              int num6 = 0;
              foreach (int num7 in cellData.Widht)
                num6 += Convert.ToInt32(Math.Ceiling((double) num7 / ((double) this.ColumnWidth - (double) this.CellPadding.Horizontal)));
              if (num3 < num6)
                num3 = num6;
            }
          }
        }
        int num8 = num3 * this._textHeight + this.CellPadding.Vertical;
        this.RowsHeight.Add(num8);
        num1 += num8;
      }
      num1 += num2;
    }
    return num1;
  }

  private void ClearClicked()
  {
    this._columnClicked = -1;
    this._rowClicked = -1;
  }

  private void LoadData(DataTable dt)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetCustomService(typeof (IImbaseServer)) is IImbaseServer customService))
        return;
      this._convs.Clear();
      this._recordLinksDicts.Clear();
      this.ColumnsCount = dt.Columns.Count;
      this.RowsCount = this.DrawTablesHeader ? dt.Rows.Count + 1 : dt.Rows.Count;
      this.RowsHeight = new List<int>(this.RowsCount);
      this._data = new TableDescription.CellData[this.ColumnsCount, this.RowsCount];
      for (int index = 0; index < this.ColumnsCount; ++index)
      {
        if (this.DrawTablesHeader)
          this._data[index, 0] = new TableDescription.CellData(this.Font, (object) dt.Columns[index].Caption, dt.Columns[index].Caption);
        object extendedProperty;
        if ((extendedProperty = dt.Columns[index].ExtendedProperties[(object) "F_OPTIONS"]) is AttributeOptions && ((AttributeOptions) extendedProperty).HasFlag((Enum) AttributeOptions.ImbaseFlag_TableRecordRef))
        {
          TypeConverter typeConverter = (TypeConverter) new TableRefTypeConverter();
          this._convs.Add(index, typeConverter);
          int colIndx = index;
          this._recordLinksDicts.Add(index, customService.NameRecordReferences(sessionKeeper.Session.SessionGUID, dt.AsEnumerable().Select<DataRow, string>((System.Func<DataRow, string>) (row => row[colIndx].ToString())).Distinct<string>().ToList<string>()));
        }
      }
      int count = dt.Rows.Count;
      this.UnitedRowIndex.Clear();
      int num = this.DrawTablesHeader ? 1 : 0;
      for (int index1 = 0; index1 < count; ++index1)
      {
        DataRow row = dt.Rows[index1];
        if (row.RowError == "United")
        {
          this.UnitedRowIndex.Add(index1 + num);
          this._data[0, index1 + num] = new TableDescription.CellData(this.Font, row[0], row[0].ToString());
        }
        else
        {
          for (int index2 = 0; index2 < this.ColumnsCount; ++index2)
          {
            string text = string.Empty;
            Dictionary<string, string> dictionary;
            if (this._recordLinksDicts.TryGetValue(index2, out dictionary))
            {
              string str;
              if (dictionary.TryGetValue(row[index2].ToString(), out str))
                text = str;
            }
            else
              text = row[index2].ToString();
            this._data[index2, index1 + num] = new TableDescription.CellData(this.Font, row[index2], text);
          }
        }
      }
      this.UnitedRowIndex.Sort();
      this.Height = 0;
      this.Invalidate(true);
    }
  }

  private int InsertColumn(DataColumn column, int index)
  {
    int num = -1;
    if (this.Table != null && column != null && index > -1)
    {
      this.Table.Columns.Add(column);
      num = this.Table.Columns.Count - 1;
      if (index < this.Table.Columns.Count)
      {
        column.SetOrdinal(index);
        num = index;
      }
      this.LoadData(this.Table);
      this.OnValueChanged();
    }
    return num;
  }

  private void RemoveColumn(int index)
  {
    if (index <= -1 || index >= this.Table.Columns.Count)
      return;
    this.Table.Columns.RemoveAt(index);
    this.LoadData(this.Table);
    this.OnValueChanged();
    this.OnTableSizeChanged();
  }

  private int InsertEmptyRow(int index, bool unitedRow)
  {
    int num = -1;
    if (this.Table != null && index > -1)
    {
      DataRow row = this.Table.NewRow();
      row.RowError = unitedRow ? "United" : string.Empty;
      this.Table.Rows.InsertAt(row, index);
      num = index < this.Table.Rows.Count ? index : this.Table.Rows.Count - 1;
      this.LoadData(this.Table);
      this.OnValueChanged();
      this.OnTableSizeChanged();
    }
    return num;
  }

  private void RemoveRow(int index)
  {
    if (index <= -1 || index >= this.Table.Rows.Count)
      return;
    this.Table.Rows.RemoveAt(index);
    this.LoadData(this.Table);
    this.OnValueChanged();
    this.OnTableSizeChanged();
  }

  private void MoveColumn(int sourceIndex, int destIndex)
  {
    if (destIndex <= -1 || sourceIndex <= -1 || sourceIndex >= this.Table.Columns.Count)
      return;
    if (destIndex == 0)
    {
      if (this.UnitedRowIndex != null && this.UnitedRowIndex.Count > 0)
      {
        foreach (int num in this.UnitedRowIndex)
        {
          this.Table.Rows[num - 1][sourceIndex] = this.Table.Rows[num - 1][0];
          this.Table.Rows[num - 1][0] = (object) string.Empty;
        }
      }
    }
    else if (sourceIndex == 0 && this.UnitedRowIndex != null && this.UnitedRowIndex.Count > 0)
    {
      foreach (int num in this.UnitedRowIndex)
      {
        this.Table.Rows[num - 1][destIndex] = this.Table.Rows[num - 1][0];
        this.Table.Rows[num - 1][0] = (object) string.Empty;
      }
    }
    this.Table.Columns[sourceIndex].SetOrdinal(destIndex);
    this.LoadData(this.Table);
    this.OnValueChanged();
  }

  private void MoveRow(int sourceIndex, int destIndex)
  {
    if (destIndex <= -1 || sourceIndex <= -1 || sourceIndex >= this.Table.Rows.Count)
      return;
    DataRow row = this.Table.NewRow();
    row.ItemArray = this.Table.Rows[sourceIndex].ItemArray;
    row.RowError = this.Table.Rows[sourceIndex].RowError;
    this.Table.Rows.RemoveAt(sourceIndex);
    this.Table.Rows.InsertAt(row, destIndex);
    this.LoadData(this.Table);
    this.OnValueChanged();
  }

  private void SetPreRowArea(Point p)
  {
    this.PreRowBounds = Rectangle.Empty;
    Rectangle tableBounds = this.TableBounds;
    int int32 = Convert.ToInt32(Math.Ceiling((double) this.PreRowBoundOffset * (double) this.AutoScaleFactor.Width));
    if (!new Rectangle(tableBounds.X - int32, tableBounds.Y, int32, tableBounds.Height).Contains(p))
      return;
    int y = tableBounds.Y;
    foreach (int height in this.RowsHeight)
    {
      y += height;
      if (y >= p.Y)
      {
        this.PreRowBounds = new Rectangle(tableBounds.X - int32, y - height, int32, height);
        break;
      }
    }
  }

  private void DrawSelectedRectangle(Graphics g, Rectangle bounds)
  {
    using (Pen pen = new Pen(this.SelectedElementBorderColor, 1.8f))
      g.DrawRectangle(pen, bounds);
  }

  private void TableCtrl_MouseDoubleClick(object sender, MouseEventArgs e)
  {
    if (!this.TableBounds.Contains(e.Location))
      return;
    this.EditValue(true);
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

  public void FillData(DataTable dt)
  {
    this.Table = dt;
    string tableName = dt.TableName;
    this._textHeight = TextRenderer.MeasureText("A", this.Font).Height;
    this._headerDelta = Convert.ToInt32(5f * this.AutoScaleFactor.Height);
    this._bottomDelta = Convert.ToInt32(5f * this.AutoScaleFactor.Height);
    this.CellPadding = new Padding(Convert.ToInt32(3f * this.AutoScaleFactor.Height));
    this.Header.Font = this.Font;
    this.Header.SetText(tableName);
    this.Header.CalcPadding(this.AutoScaleFactor);
    this.Header.Visible = !string.IsNullOrEmpty(tableName);
    this.LoadData(dt);
  }

  public void CombineRow(int index, bool isCombine)
  {
    int num = this.DrawTablesHeader ? 0 : -1;
    int index1 = num == 0 ? index - 1 : index;
    if (index <= num)
      return;
    if (isCombine)
    {
      if (this.UnitedRowIndex.Contains(index))
        return;
      DataRow row = this.Table.Rows[index1];
      row.RowError = "United";
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(row[0]);
      stringBuilder.Append(" ");
      for (int columnIndex = 1; columnIndex < this.Table.Columns.Count; ++columnIndex)
      {
        if (row[columnIndex] != DBNull.Value)
        {
          stringBuilder.Append(row[columnIndex]);
          stringBuilder.Append(" ");
          row[columnIndex] = (object) DBNull.Value;
        }
      }
      row[0] = (object) stringBuilder.ToString().Trim();
      this.LoadData(this.Table);
      this.OnValueChanged();
    }
    else
    {
      if (!this.UnitedRowIndex.Contains(index))
        return;
      this.Table.Rows[index1].RowError = string.Empty;
      this.LoadData(this.Table);
      this.OnValueChanged();
    }
  }

  private object GetValue(int colIndex, int rowIndex)
  {
    object obj = (object) null;
    if (colIndex > -1 && this.ColumnsCount > colIndex && rowIndex > -1 && this.RowsCount > rowIndex)
      obj = this._data[colIndex, rowIndex].Value;
    else if (this.RowsCount > rowIndex && this.UnitedRowIndex.Contains(rowIndex))
      obj = this._data[0, rowIndex].Value;
    return obj;
  }

  private string GetText(int colIndex, int rowIndex)
  {
    string text = string.Empty;
    if (colIndex > -1 && this.ColumnsCount > colIndex && rowIndex > -1 && this.RowsCount > rowIndex)
      text = this._data[colIndex, rowIndex].Text;
    else if (this.RowsCount > rowIndex && this.UnitedRowIndex.Contains(rowIndex))
      text = this._data[0, rowIndex].Text;
    return text;
  }

  public void AddColumn()
  {
    DataColumn column = new DataColumn()
    {
      Caption = string.Empty
    };
    int columnClicked = this.ColumnClicked;
    int num1;
    int num2 = this.InsertColumn(column, num1 = columnClicked + 1);
    if (num2 <= -1)
      return;
    this.ColumnClicked = num2;
    this.OnTableSizeChanged();
  }

  public void AddRow()
  {
    int num = this.InsertEmptyRow(this.RowClicked, false) + 1;
    if (num > -1)
      this.RowClicked = num;
    this.ColumnClicked = 0;
    this.OnTableSizeChanged();
  }

  public void RemoveClickedColumn()
  {
    int columnClicked = this.ColumnClicked;
    this.RemoveColumn(columnClicked);
    this.ColumnClicked = columnClicked;
  }

  public void RemoveClickedRow()
  {
    int rowClicked = this.RowClicked;
    this.RemoveRow(rowClicked - 1);
    this.RowClicked = rowClicked;
  }

  public void MoveClickedColumnBegin()
  {
    if (this.ColumnClicked <= 0)
      return;
    this.MoveColumn(this.ColumnClicked, 0);
    this.ColumnClicked = 0;
  }

  public void MoveClickedColumnEnd()
  {
    if (this.ColumnClicked <= -1 || this.ColumnClicked >= this.ColumnsCount - 1)
      return;
    this.MoveColumn(this.ColumnClicked, this.ColumnsCount - 1);
    this.ColumnClicked = this.ColumnsCount - 1;
  }

  public void MoveClickedColumnLeft()
  {
    if (this.ColumnClicked <= 0)
      return;
    this.MoveColumn(this.ColumnClicked, this.ColumnClicked - 1);
    --this.ColumnClicked;
  }

  public void MoveClickedColumnRight()
  {
    if (this.ColumnClicked >= this.ColumnsCount - 1)
      return;
    this.MoveColumn(this.ColumnClicked, this.ColumnClicked + 1);
    ++this.ColumnClicked;
  }

  public void MoveClickedRowBegin()
  {
    if (this.RowClicked <= 0)
      return;
    this.MoveRow(this.RowClicked - 1, 0);
    this.RowClicked = 1;
  }

  public void MoveClickedRowUp()
  {
    if (this.RowClicked <= 0)
      return;
    int sourceIndex = this.RowClicked - 1;
    int num = this.RowClicked - 2;
    int destIndex = num < 0 ? 0 : num;
    if (sourceIndex <= destIndex)
      return;
    this.MoveRow(sourceIndex, destIndex);
    this.RowClicked = destIndex + 1;
  }

  public void MoveClickedRowDown()
  {
    if (this.RowClicked <= 0 || this.RowClicked >= this.RowsCount)
      return;
    int sourceIndex = this.RowClicked - 1;
    int rowClicked = this.RowClicked;
    if (sourceIndex >= rowClicked)
      return;
    this.MoveRow(sourceIndex, rowClicked);
    this.RowClicked = rowClicked + 1;
  }

  public void MoveClickedRowEnd()
  {
    if (this.RowClicked <= 0 || this.RowClicked >= this.RowsCount)
      return;
    this.MoveRow(this.RowClicked - 1, this.RowsCount - 1);
    this.RowClicked = this.RowsCount - 1;
  }

  public void SetText(int colIndex, int rowIndex, string text)
  {
    if (colIndex <= -1 || colIndex >= this.ColumnsCount || rowIndex <= -1 || rowIndex >= this.RowsCount)
      return;
    if (rowIndex == 0)
      this.Table.Columns[colIndex].Caption = text;
    else if (rowIndex - 1 > -1)
      this.Table.Rows[rowIndex - 1][colIndex] = (object) text;
    this.LoadData(this.Table);
  }

  protected override void SetBoundsCore(
    int x,
    int y,
    int width,
    int height,
    BoundsSpecified specified)
  {
    height = height >= 0 ? this.CalcControlHeight(width) : throw new ArgumentOutOfRangeException(nameof (height));
    base.SetBoundsCore(x, y, width, height, specified);
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    base.OnMouseMove(e);
    this.SetPreRowArea(e.Location);
    this.Invalidate(true);
  }

  protected override void OnMouseClick(MouseEventArgs e)
  {
    base.OnMouseClick(e);
    this.OnBeforeTableSelected();
    Rectangle rectangle1;
    if (!this.PreRowBounds.Contains(e.Location) && !this.TableBounds.Contains(e.Location))
    {
      rectangle1 = this.Header.Bounds;
      if (!rectangle1.Contains(e.Location))
        return;
    }
    this.ClearClicked();
    rectangle1 = this.TableBounds;
    if (rectangle1.Contains(e.Location))
    {
      Point location;
      if (this.DrawTablesHeader && this.RowsHeight != null && this.RowsHeight.Count > 0)
      {
        Rectangle rectangle2;
        ref Rectangle local = ref rectangle2;
        rectangle1 = this.TableBounds;
        int x1 = rectangle1.X;
        rectangle1 = this.TableBounds;
        int y = rectangle1.Y;
        rectangle1 = this.TableBounds;
        int width = rectangle1.Width;
        int height = this.RowsHeight[0];
        local = new Rectangle(x1, y, width, height);
        if (rectangle2.Contains(e.Location))
        {
          int x2 = rectangle2.X;
          for (int index = 0; index < this.ColumnsCount; ++index)
          {
            x2 += this.ColumnWidth;
            location = e.Location;
            if (location.X <= x2)
            {
              this._columnClicked = index;
              break;
            }
          }
        }
      }
      if (this._columnClicked == -1)
      {
        rectangle1 = this.TableBounds;
        int y1 = rectangle1.Y;
        for (int index = 0; index < this.RowsCount; ++index)
        {
          if (this.RowsHeight != null)
            y1 += this.RowsHeight[index];
          int num = y1;
          location = e.Location;
          int y2 = location.Y;
          if (num >= y2)
          {
            this._rowClicked = index;
            break;
          }
        }
        if (!this.UnitedRowIndex.Contains(this._rowClicked))
        {
          rectangle1 = this.TableBounds;
          int x = rectangle1.X;
          for (int index = 0; index < this.ColumnsCount; ++index)
          {
            x += this.ColumnWidth;
            location = e.Location;
            if (location.X <= x)
            {
              this._columnClicked = index;
              break;
            }
          }
        }
      }
    }
    else
    {
      rectangle1 = this.TableBounds;
      int y3 = rectangle1.Y;
      int num1 = y3 + this.RowsHeight[0];
      Point location = e.Location;
      int y4 = location.Y;
      if (num1 < y4)
      {
        for (int index = 0; index < this.RowsCount; ++index)
        {
          y3 += this.RowsHeight[index];
          int num2 = y3;
          location = e.Location;
          int y5 = location.Y;
          if (num2 >= y5)
          {
            this._rowClicked = index;
            break;
          }
        }
      }
      if (this._rowClicked == -1 && this._columnClicked == -1)
      {
        this.Header.Visible = true;
        this.Height = 0;
      }
    }
    if (this.Header.Visible)
    {
      rectangle1 = this.Header.Bounds;
      if (rectangle1.Contains(e.Location))
        this.IsExpanded = !this.IsExpanded;
    }
    this.Selected = true;
    this.OnAfterTableSelected();
  }

  protected override void OnCompleteEdit(object value)
  {
    base.OnCompleteEdit(value);
    if (this.ColumnClicked > -1)
      this.SetText(this.ColumnClicked, this.RowClicked == -1 ? 0 : this.RowClicked, value?.ToString());
    else if (this.RowClicked > -1)
    {
      this.SetText(0, this.RowClicked, value?.ToString());
    }
    else
    {
      this.Header.SetText(value?.ToString());
      this.Height = 0;
    }
    this.OnTableSizeChanged();
  }

  protected override void OnPaint(PaintEventArgs e)
  {
    base.OnPaint(e);
    if (this.Header != null && this.Header.Visible && !this.Header.Bounds.IsEmpty)
      this.DrawHeader(e.Graphics, this.Header.Bounds, this.Font, this.Header.TextBounds, this.Header.Text, this.Header.ArrowWidth, this.IsExpanded);
    Rectangle rectangle = Rectangle.Empty;
    List<int> intList = (List<int>) null;
    if (this.IsExpanded)
    {
      rectangle = this.TableBounds;
      intList = this.RowsHeight;
      int x1 = rectangle.X;
      int y1 = rectangle.Y;
      if (this.DrawTablesHeader)
      {
        using (Brush brush = (Brush) new SolidBrush(this.HeaderColor))
          e.Graphics.FillRectangle(brush, x1, y1, rectangle.Width, intList[0]);
      }
      using (Pen pen = new Pen(SystemColors.ActiveBorder))
      {
        int num = x1 + this.ColumnWidth;
        for (int index = 1; index < this.ColumnsCount; ++index)
        {
          e.Graphics.DrawLine(pen, num, rectangle.Y, num, rectangle.Bottom);
          num += this.ColumnWidth;
        }
        if (this.DrawLines)
        {
          e.Graphics.DrawRectangle(pen, rectangle.X, y1, rectangle.Width, intList[0]);
          int y2 = y1 + intList[0];
          using (Brush brush = (Brush) new SolidBrush(this.BackColor))
          {
            for (int index = 1; index < intList.Count; ++index)
            {
              if (this.UnitedRowIndex.BinarySearch(index) > -1)
                e.Graphics.FillRectangle(brush, rectangle.X, y2, rectangle.Width, intList[index]);
              e.Graphics.DrawRectangle(pen, rectangle.X, y2, rectangle.Width, intList[index]);
              y2 += intList[index];
            }
          }
        }
      }
      if (this.PreRowBounds != Rectangle.Empty)
      {
        using (Brush brush = (Brush) new SolidBrush(this.BackColor))
          e.Graphics.FillRectangle(brush, this.PreRowBounds);
        using (Pen pen = new Pen(SystemColors.ActiveBorder))
          e.Graphics.DrawRectangle(pen, this.PreRowBounds);
      }
      int x2 = rectangle.X;
      Padding cellPadding = this.CellPadding;
      int left1 = cellPadding.Left;
      int num1 = x2 + left1;
      int y3 = rectangle.Y;
      cellPadding = this.CellPadding;
      int top = cellPadding.Top;
      int num2 = y3 + top;
      for (int index = 0; index < this.RowsCount; ++index)
      {
        int num3 = this.ColumnsCount;
        int num4 = this.ColumnWidth;
        if (this.UnitedRowIndex.BinarySearch(index) > -1)
        {
          num3 = 1;
          num4 = rectangle.Width;
        }
        for (int colIndex = 0; colIndex < num3; ++colIndex)
        {
          Rectangle bounds;
          ref Rectangle local = ref bounds;
          int x3 = num1;
          int y4 = num2;
          int num5 = num4;
          cellPadding = this.CellPadding;
          int horizontal = cellPadding.Horizontal;
          int width = num5 - horizontal;
          int num6 = this.RowsHeight[index];
          cellPadding = this.CellPadding;
          int vertical = cellPadding.Vertical;
          int height = num6 - vertical;
          local = new Rectangle(x3, y4, width, height);
          TextRenderer.DrawText((IDeviceContext) e.Graphics, this.GetText(colIndex, index), this.Font, bounds, SystemColors.ControlText, TextFormatFlags.WordBreak);
          num1 += this.ColumnWidth;
        }
        int x4 = rectangle.X;
        cellPadding = this.CellPadding;
        int left2 = cellPadding.Left;
        num1 = x4 + left2;
        num2 += this.RowsHeight[index];
      }
    }
    if (!this.Selected)
      return;
    Rectangle bounds1;
    if (rectangle == Rectangle.Empty && this.Header != null)
    {
      ref Rectangle local = ref rectangle;
      bounds1 = this.Header.Bounds;
      int x = bounds1.X;
      bounds1 = this.Header.Bounds;
      int y = bounds1.Y;
      bounds1 = this.Header.Bounds;
      int width = bounds1.Width;
      bounds1 = this.Header.Bounds;
      int height = bounds1.Height;
      local = new Rectangle(x, y, width, height);
    }
    Rectangle bounds2 = rectangle;
    if (this.ColumnClicked > -1)
    {
      if (this.RowClicked == -1)
      {
        bounds2 = new Rectangle(rectangle.X + this.ColumnWidth * this.ColumnClicked, rectangle.Y, this.ColumnWidth, rectangle.Height);
      }
      else
      {
        int x = rectangle.X + this.ColumnWidth * this.ColumnClicked;
        int y = rectangle.Y;
        for (int index = 0; index < this.RowClicked; ++index)
        {
          if (intList != null)
            y += intList[index];
        }
        if (intList != null)
          bounds2 = new Rectangle(x, y, this.ColumnWidth, intList[this.RowClicked]);
      }
    }
    else if (this.RowClicked > -1)
    {
      int y = rectangle.Y;
      for (int index = 0; index < this.RowClicked; ++index)
      {
        if (intList != null)
          y += intList[index];
      }
      if (intList != null)
        bounds2 = new Rectangle(rectangle.X, y, rectangle.Width, intList[this.RowClicked]);
    }
    else if (this.Header != null)
    {
      ref Rectangle local = ref bounds2;
      int x = bounds2.X;
      bounds1 = this.Header.Bounds;
      int y5 = bounds1.Y;
      int width = bounds2.Width;
      int bottom = bounds2.Bottom;
      bounds1 = this.Header.Bounds;
      int y6 = bounds1.Y;
      int height = bottom - y6;
      local = new Rectangle(x, y5, width, height);
    }
    this.DrawSelectedRectangle(e.Graphics, bounds2);
  }

  protected override void OnMouseLeave(EventArgs e)
  {
    this.PreRowBounds = Rectangle.Empty;
    this.Invalidate(true);
    base.OnMouseLeave(e);
  }

  protected virtual void OnAfterTableSelected()
  {
    EventHandler afterTableSelected = this.AfterTableSelected;
    if (afterTableSelected == null)
      return;
    afterTableSelected((object) this, EventArgs.Empty);
  }

  protected virtual void OnBeforeTableSelected()
  {
    EventHandler beforeTableSelected = this.BeforeTableSelected;
    if (beforeTableSelected == null)
      return;
    beforeTableSelected((object) this, EventArgs.Empty);
  }

  protected virtual void OnTableSizeChanged()
  {
    EventHandler tableSizeChanged = this.TableSizeChanged;
    if (tableSizeChanged == null)
      return;
    tableSizeChanged((object) this, EventArgs.Empty);
  }

  public event EventHandler AfterTableSelected;

  public event EventHandler BeforeTableSelected;

  public event EventHandler TableSizeChanged;

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
    this.Name = nameof (TableDescription);
    this.Size = new Size(281, 163);
    this.MouseDoubleClick += new MouseEventHandler(this.TableCtrl_MouseDoubleClick);
    this.ResumeLayout(false);
  }

  private class CellData
  {
    internal string Text;
    internal List<int> Widht = new List<int>(0);
    internal object Value;

    internal bool IsEmpty => string.IsNullOrEmpty(this.Text) && this.Widht.Count == 0;

    public CellData(Font f, object value, string text)
    {
      this.Value = value;
      this.Text = text;
      string str = text;
      string[] separator = new string[1]{ "\r\n" };
      foreach (string text1 in str.Split(separator, StringSplitOptions.None))
        this.Widht.Add(TextRenderer.MeasureText(text1, f).Width);
    }
  }
}
