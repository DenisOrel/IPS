// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.Controls.MaterialProperties.BasePropertiesCtrl
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using ImSSP;
using Intermech.Interfaces;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook.Controls.MaterialProperties;

public class BasePropertiesCtrl : UserControl
{
  private string _imbaseKey = string.Empty;
  private IContainer components;
  private SelectablePanel Surface;

  public BasePropertiesCtrl() => this.InitializeComponent();

  public bool ReadOnly { get; set; }

  public DataProvider SettingsDriver { get; set; }

  public string ImbaseKey
  {
    get => this._imbaseKey;
    set
    {
      this._imbaseKey = value;
      this.Clear(false);
      this.LoadSettings();
      this.OnSelectedElementChanged();
    }
  }

  public List<Page> Pages => this.Surface.Controls.OfType<Page>().ToList<Page>();

  public Page ActivePage { get; private set; }

  public int BetweenPagesDistance { get; set; } = 10;

  public event SelectedRibbonElementEventHandler SelectedElementChanged;

  public event EventHandler DataChanged;

  public event EventHandler EditorEnter;

  protected virtual void OnDataChanged()
  {
    EventHandler dataChanged = this.DataChanged;
    if (dataChanged == null)
      return;
    dataChanged((object) this, EventArgs.Empty);
  }

  protected virtual void OnSelectedElementChanged()
  {
    if (this.SelectedElementChanged == null)
      return;
    SelectedElement element = SelectedElement.None;
    int index = -1;
    int elementsCount = 0;
    bool flag = false;
    int num1 = -1;
    int num2 = -1;
    if (this.ActivePage != null)
    {
      if (!this.ActivePage.IsTableClicked)
      {
        element = SelectedElement.Page;
        index = this.Pages.IndexOf(this.ActivePage);
        elementsCount = this.Pages.Count;
      }
      else if (this.ActivePage.ClickedTable != null)
      {
        TableDescription clickedTable = this.ActivePage.ClickedTable;
        element = clickedTable.SelectedElement;
        switch (element)
        {
          case SelectedElement.Table:
            index = this.ActivePage.Tables.IndexOf(clickedTable);
            elementsCount = this.ActivePage.Tables.Count;
            break;
          case SelectedElement.Column:
            index = clickedTable.ColumnClicked;
            elementsCount = clickedTable.ColumnsCount;
            break;
          case SelectedElement.Row:
            index = clickedTable.RowClicked - 1;
            elementsCount = clickedTable.RowsCount - 1;
            flag = clickedTable.UnitedRowIndex.Contains(clickedTable.RowClicked);
            num1 = clickedTable.ColumnsCount;
            break;
          case SelectedElement.Cell:
            index = clickedTable.RowClicked - 1;
            num1 = clickedTable.ColumnsCount;
            num2 = clickedTable.RowsCount - 1;
            break;
        }
      }
    }
    SelectedRibbonElementEventArgs e = new SelectedRibbonElementEventArgs(element, index, elementsCount)
    {
      IsUnitedRow = flag,
      ColumnCount = num1,
      RowCount = num2
    };
    SelectedRibbonElementEventHandler selectedElementChanged = this.SelectedElementChanged;
    if (selectedElementChanged == null)
      return;
    selectedElementChanged((object) this, e);
  }

  protected override void OnLayout(LayoutEventArgs e)
  {
    this.CalcPagesLocation(this.Surface.ClientSize.Width);
    base.OnLayout(e);
  }

  private void CalcPagesLocation(int width)
  {
    if (this.Pages.Count <= 0)
      return;
    int y = this.Pages.Select<Page, int>((System.Func<Page, int>) (x => x.Top)).Min();
    int width1 = width;
    foreach (Page page in this.Pages)
    {
      page.Bounds = new Rectangle(0, y, width1, page.Height);
      y = page.Bounds.Bottom + this.BetweenPagesDistance;
    }
  }

  private void BasePropertiesCtrl_SizeChanged(object sender, EventArgs e) => this.PerformLayout();

  private void BasePropertiesCtrl_PageSelected(object sender, EventArgs e)
  {
    if (this.ActivePage != (Page) sender)
    {
      this.ActivePage?.LostSelection();
      this.ActivePage = (Page) sender;
    }
    this.Invalidate(true);
    this.OnSelectedElementChanged();
  }

  private void BasePropertiesCtrl_ValueChanged(object sender, EventArgs e) => this.OnDataChanged();

  private void Surface_ControlAdded(object sender, ControlEventArgs e)
  {
    if (!(e.Control is Page control))
      return;
    control.PageSelected += new EventHandler(this.BasePropertiesCtrl_PageSelected);
    control.BeforePageClicked += new EventHandler(this.BasePropertiesCtrl_BeforePageClicked);
    control.PageSizeChanged += new EventHandler(this.BasePropertiesCtrl_SizeChanged);
    control.ValueChanged += new EventHandler(this.BasePropertiesCtrl_ValueChanged);
    control.EditorEnter += new EventHandler(this.BasePropertiesCtrl_EditorEnter);
    control.Enabled = !this.ReadOnly;
  }

  private void BasePropertiesCtrl_EditorEnter(object sender, EventArgs e) => this.OnEditorEnter();

  private void BasePropertiesCtrl_BeforePageClicked(object sender, EventArgs e)
  {
    this.Pages.ForEach((Action<Page>) (x => x.LostSelection()));
  }

  private void Surface_ControlRemoved(object sender, ControlEventArgs e)
  {
    if (!(e.Control is Page))
      return;
    ((Page) e.Control).PageSelected -= new EventHandler(this.BasePropertiesCtrl_PageSelected);
    ((Page) e.Control).BeforePageClicked -= new EventHandler(this.BasePropertiesCtrl_BeforePageClicked);
    ((Page) e.Control).PageSizeChanged -= new EventHandler(this.BasePropertiesCtrl_SizeChanged);
    ((ControlWithEditor) e.Control).ValueChanged -= new EventHandler(this.BasePropertiesCtrl_ValueChanged);
    ((ControlWithEditor) e.Control).EditorEnter -= new EventHandler(this.BasePropertiesCtrl_EditorEnter);
  }

  private string ReadBlob(IDBAttribute attr)
  {
    string str = string.Empty;
    if (attr != null)
    {
      try
      {
        if (attr is IBlobReader blobReader)
        {
          BlobInformation blobInformation = blobReader.OpenBlob(0);
          if (blobInformation.RealFileSize != 0L)
          {
            byte[] buffer = blobReader.ReadDataBlock();
            blobReader.CloseBlob();
            if (buffer != null)
            {
              if (buffer.Length > sc_14485.ssp_imbase_14486(1355766346))
              {
                IPackedStream service = ServiceUtils.GetService<IPackedStream>((object) ApplicationServices.Container, true);
                using (MemoryStream inStream = new MemoryStream(buffer))
                {
                  inStream.Position = 0L;
                  using (MemoryStream memoryStream = new MemoryStream((int) blobInformation.RealFileSize))
                  {
                    service.UnpackStream((Stream) memoryStream, (Stream) inStream);
                    memoryStream.Position = 0L;
                    using (BinaryReader binaryReader = new BinaryReader((Stream) memoryStream, Encoding.UTF8))
                      str = binaryReader.ReadString();
                  }
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }
    return str;
  }

  private void WriteBlob(IDBAttribute attr, string strProperties)
  {
    IPackedStream service = ServiceUtils.GetService<IPackedStream>((object) ApplicationServices.Container, true);
    using (MemoryStream memoryStream = new MemoryStream(strProperties.Length))
    {
      using (BinaryWriter binaryWriter = new BinaryWriter((Stream) memoryStream))
      {
        binaryWriter.Write(strProperties);
        binaryWriter.Flush();
        memoryStream.Position = 0L;
        using (MemoryStream outStream = new MemoryStream((int) memoryStream.Length / 2))
        {
          service.PackStream((Stream) outStream, (Stream) memoryStream, 9);
          outStream.Position = 0L;
          byte[] buffer = outStream.GetBuffer();
          byte[] data = new byte[outStream.Length];
          byte[] dst = data;
          int length = (int) outStream.Length;
          Buffer.BlockCopy((Array) buffer, 0, (Array) dst, 0, length);
          try
          {
            BlobInformation blobInfo = new BlobInformation(outStream.Length, outStream.Length, DateTime.Now, string.Empty, ArcMethods.ZLibPacked, string.Empty);
            if (attr is IBlobWriter blobWriter)
              blobWriter.OpenBlob(blobInfo, false);
            blobWriter?.WriteDataBlock(data);
          }
          catch (Exception ex)
          {
            throw;
          }
        }
      }
    }
  }

  public virtual void LoadSettings()
  {
    if (string.IsNullOrEmpty(this._imbaseKey))
      return;
    List<Tuple<string, IEnumerable<DataTable>>> parms = this.SettingsDriver?.LoadData(this._imbaseKey);
    if (parms == null || parms.Count <= 0)
      return;
    this.AddPages((IEnumerable<Tuple<string, IEnumerable<DataTable>>>) parms);
  }

  public virtual void SaveSettings()
  {
    this.SettingsDriver?.SaveData(this._imbaseKey, this.Pages.Select<Page, Tuple<string, IEnumerable<DataTable>>>((System.Func<Page, Tuple<string, IEnumerable<DataTable>>>) (x => new Tuple<string, IEnumerable<DataTable>>(x.Header.Text, x.Tables.Select<TableDescription, DataTable>((System.Func<TableDescription, DataTable>) (t => t.Table))))).ToList<Tuple<string, IEnumerable<DataTable>>>());
  }

  public Page AddPage(
    string caption,
    IEnumerable<DataTable> tables = null,
    bool drawLines = true,
    bool drawTablesHeader = true,
    bool forbiddenColumnsAdd = false)
  {
    Page page = new Page(caption, tables, drawLines, drawTablesHeader, forbiddenColumnsAdd);
    this.Surface.Controls.Add((Control) page);
    this.Surface.PerformLayout();
    return page;
  }

  public Page[] AddPages(
    IEnumerable<Tuple<string, IEnumerable<DataTable>>> parms)
  {
    Page[] array = parms.ToList<Tuple<string, IEnumerable<DataTable>>>().Select<Tuple<string, IEnumerable<DataTable>>, Page>((System.Func<Tuple<string, IEnumerable<DataTable>>, Page>) (x => new Page(x.Item1, x.Item2))).ToArray<Page>();
    if (array.Length != 0)
      this.Surface.Controls.AddRange((Control[]) array);
    this.Surface.PerformLayout();
    return array;
  }

  public virtual void Clear(bool bInvalidate)
  {
    this.ActivePage = (Page) null;
    this.SuspendLayout();
    this.Surface.Controls.Clear();
    this.ResumeLayout();
    if (!bInvalidate)
      return;
    this.Invalidate(true);
  }

  public virtual void ExpandAll(bool isExpand)
  {
    foreach (Page page in this.Pages)
    {
      page.IsExpanded = isExpand;
      page.ExpandAllTables(page.IsExpanded);
    }
    this.Invalidate();
  }

  public void AddAction()
  {
    if (this.ActivePage != null)
    {
      if (!this.ActivePage.IsTableClicked)
      {
        int childIndex = this.Surface.Controls.GetChildIndex((Control) this.ActivePage);
        Page child = this.AddPage(string.Empty);
        int num;
        this.Surface.Controls.SetChildIndex((Control) child, num = childIndex + 1);
        child.Selected = true;
        child.Focus();
      }
      else if (this.ActivePage.ClickedTable != null)
      {
        switch (this.ActivePage.ClickedTable.SelectedElement)
        {
          case SelectedElement.Table:
            TableDescription tableDescription = this.ActivePage.AddTable();
            this.ActivePage.ClickedTable = tableDescription;
            tableDescription.Selected = true;
            tableDescription.Focus();
            break;
          case SelectedElement.Column:
            this.ActivePage.ClickedTable.AddColumn();
            break;
          case SelectedElement.Row:
            this.ActivePage.ClickedTable.AddRow();
            break;
          case SelectedElement.Cell:
            if (this.ActivePage.ClickedTable.ColumnsCount == 1)
            {
              this.ActivePage.ClickedTable.AddRow();
              break;
            }
            break;
        }
      }
    }
    else
    {
      Page page = this.AddPage(string.Empty);
      page.Selected = true;
      page.Focus();
    }
    this.OnSelectedElementChanged();
  }

  public void EditAction()
  {
    if (this.ActivePage == null)
      return;
    TableDescription clickedTable = this.ActivePage.ClickedTable;
    if (clickedTable != null)
      clickedTable.EditValue();
    else
      this.ActivePage.EditValue();
  }

  public void MoveBeginAction()
  {
    if (this.ActivePage != null)
    {
      if (!this.ActivePage.IsTableClicked)
      {
        this.Surface.Controls.SetChildIndex((Control) this.ActivePage, 0);
        this.ActivePage.Focus();
      }
      else if (this.ActivePage.ClickedTable != null)
      {
        switch (this.ActivePage.ClickedTable.SelectedElement)
        {
          case SelectedElement.Table:
            this.ActivePage.MoveClickedTableBegin();
            break;
          case SelectedElement.Column:
            this.ActivePage.ClickedTable.MoveClickedColumnBegin();
            break;
          case SelectedElement.Row:
          case SelectedElement.Cell:
            this.ActivePage.ClickedTable.MoveClickedRowBegin();
            break;
        }
      }
    }
    this.OnSelectedElementChanged();
  }

  public void MoveUpAction()
  {
    if (this.ActivePage != null)
    {
      if (!this.ActivePage.IsTableClicked)
      {
        int childIndex = this.Surface.Controls.GetChildIndex((Control) this.ActivePage);
        if (childIndex > 0)
          this.Surface.Controls.SetChildIndex((Control) this.ActivePage, childIndex - 1);
      }
      else if (this.ActivePage.ClickedTable != null)
      {
        switch (this.ActivePage.ClickedTable.SelectedElement)
        {
          case SelectedElement.Table:
            this.ActivePage.MoveClickedTableUp();
            break;
          case SelectedElement.Row:
          case SelectedElement.Cell:
            this.ActivePage.ClickedTable.MoveClickedRowUp();
            break;
        }
      }
    }
    this.OnSelectedElementChanged();
  }

  public void MoveDownAction()
  {
    if (this.ActivePage != null)
    {
      if (!this.ActivePage.IsTableClicked)
      {
        int childIndex = this.Surface.Controls.GetChildIndex((Control) this.ActivePage);
        if (childIndex < this.Pages.Count - 1)
          this.Surface.Controls.SetChildIndex((Control) this.ActivePage, childIndex + 1);
      }
      else if (this.ActivePage.ClickedTable != null)
      {
        switch (this.ActivePage.ClickedTable.SelectedElement)
        {
          case SelectedElement.Table:
            this.ActivePage.MoveClickedTableDown();
            break;
          case SelectedElement.Row:
          case SelectedElement.Cell:
            this.ActivePage.ClickedTable.MoveClickedRowDown();
            break;
        }
      }
    }
    this.OnSelectedElementChanged();
  }

  public void MoveEndAction()
  {
    if (this.ActivePage != null)
    {
      if (!this.ActivePage.IsTableClicked)
      {
        this.Surface.Controls.SetChildIndex((Control) this.ActivePage, this.Pages.Count - 1);
        this.ActivePage.Focus();
      }
      else if (this.ActivePage.ClickedTable != null)
      {
        switch (this.ActivePage.ClickedTable.SelectedElement)
        {
          case SelectedElement.Table:
            this.ActivePage.MoveClickedTableEnd();
            break;
          case SelectedElement.Column:
            this.ActivePage.ClickedTable.MoveClickedColumnEnd();
            break;
          case SelectedElement.Row:
          case SelectedElement.Cell:
            this.ActivePage.ClickedTable.MoveClickedRowEnd();
            break;
        }
      }
    }
    this.OnSelectedElementChanged();
  }

  public void MoveLeftAction()
  {
    if (this.ActivePage != null && this.ActivePage.IsTableClicked)
    {
      TableDescription clickedTable = this.ActivePage.ClickedTable;
      if (clickedTable != null && clickedTable.SelectedElement == SelectedElement.Column)
        clickedTable.MoveClickedColumnLeft();
    }
    this.OnSelectedElementChanged();
  }

  public void MoveRightAction()
  {
    if (this.ActivePage != null && this.ActivePage.IsTableClicked)
    {
      TableDescription clickedTable = this.ActivePage.ClickedTable;
      if (clickedTable != null && clickedTable.SelectedElement == SelectedElement.Column)
        clickedTable.MoveClickedColumnRight();
    }
    this.OnSelectedElementChanged();
  }

  public void RemoveAction()
  {
    if (this.ActivePage != null)
    {
      Form form = this.FindForm();
      string caption = LocalizationHolder.rm.GetString("IMH_DeleteData_Caption");
      if (!this.ActivePage.IsTableClicked)
      {
        string text = LocalizationHolder.rm.GetString("IMH_DeletePage_Msg");
        if (MessageBox.Show((IWin32Window) form, text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
          int index = this.Surface.Controls.IndexOf((Control) this.ActivePage);
          this.Surface.Controls.Remove((Control) this.ActivePage);
          if (index != 0)
          {
            this.ActivePage = index < this.Pages.Count ? this.Pages[index] : this.Pages[index - 1];
            this.ActivePage.Selected = true;
            this.ActivePage.Focus();
          }
          else
            this.ActivePage = (Page) null;
        }
      }
      else if (this.ActivePage.ClickedTable != null)
      {
        switch (this.ActivePage.ClickedTable.SelectedElement)
        {
          case SelectedElement.Table:
            if (this.ActivePage.Tables.Count > 1)
            {
              string text = LocalizationHolder.rm.GetString("IMH_DeleteTable_Msg");
              if (MessageBox.Show((IWin32Window) form, text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
              {
                this.ActivePage.RemoveClickedTable();
                break;
              }
              break;
            }
            break;
          case SelectedElement.Column:
            if (this.ActivePage.ClickedTable.CanRemoveColumn)
            {
              string text = LocalizationHolder.rm.GetString("IMH_DeleteColumn_Msg");
              if (MessageBox.Show((IWin32Window) form, text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
              {
                this.ActivePage.ClickedTable.RemoveClickedColumn();
                break;
              }
              break;
            }
            break;
          case SelectedElement.Row:
          case SelectedElement.Cell:
            if (this.ActivePage.ClickedTable.CanRemoveRows)
            {
              string text = LocalizationHolder.rm.GetString("IMH_DeleteString_Msg");
              if (MessageBox.Show((IWin32Window) form, text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
              {
                this.ActivePage.ClickedTable.RemoveClickedRow();
                break;
              }
              break;
            }
            break;
        }
      }
    }
    this.OnSelectedElementChanged();
  }

  public void UnionAction(bool isCombine)
  {
    if (this.ActivePage?.ClickedTable != null && this.ActivePage.ClickedTable.SelectedElement == SelectedElement.Row)
      this.ActivePage.ClickedTable.CombineRow(this.ActivePage.ClickedTable.RowClicked, isCombine);
    this.OnSelectedElementChanged();
  }

  public virtual void ReloadSettingsData() => this.ImbaseKey = this._imbaseKey;

  private void Surface_SizeChanged(object sender, EventArgs e) => this.Surface.PerformLayout();

  private void Surface_MouseClick(object sender, MouseEventArgs e)
  {
    this.BasePropertiesCtrl_PageSelected((object) null, new EventArgs());
    this.OnSelectedElementChanged();
  }

  private void Surface_Layout(object sender, LayoutEventArgs e) => this.OnLayout(e);

  protected virtual void OnEditorEnter()
  {
    EventHandler editorEnter = this.EditorEnter;
    if (editorEnter == null)
      return;
    editorEnter((object) this, EventArgs.Empty);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.Surface = new SelectablePanel();
    this.SuspendLayout();
    this.Surface.AutoScroll = true;
    this.Surface.Dock = DockStyle.Fill;
    this.Surface.Location = new Point(0, 0);
    this.Surface.Margin = new Padding(0);
    this.Surface.Name = "Surface";
    this.Surface.Padding = new Padding(10);
    this.Surface.Size = new Size(602, 369);
    this.Surface.TabIndex = 0;
    this.Surface.TabStop = true;
    this.Surface.SizeChanged += new EventHandler(this.Surface_SizeChanged);
    this.Surface.ControlAdded += new ControlEventHandler(this.Surface_ControlAdded);
    this.Surface.ControlRemoved += new ControlEventHandler(this.Surface_ControlRemoved);
    this.Surface.Layout += new LayoutEventHandler(this.Surface_Layout);
    this.Surface.MouseClick += new MouseEventHandler(this.Surface_MouseClick);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.BackColor = SystemColors.Window;
    this.BorderStyle = BorderStyle.FixedSingle;
    this.Controls.Add((Control) this.Surface);
    this.Name = nameof (BasePropertiesCtrl);
    this.Size = new Size(602, 369);
    this.ResumeLayout(false);
  }
}
