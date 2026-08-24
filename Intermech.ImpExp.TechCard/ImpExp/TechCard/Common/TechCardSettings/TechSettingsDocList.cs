// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.TechCardSettings.TechSettingsDocList
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Extensions;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common.TechCardSettings;

public class TechSettingsDocList : Form
{
  private const string COL_CHECK_BOX = "COL_CHECK_BOX";
  private CancellationTokenSource _ctsSearch;
  private int _idx_fld_Key;
  private int _idx_fld_Checked = -1;
  private IContainer components;
  private DataGridView dgviewTP;
  private ContextMenuStrip cmsDocList;
  private ToolStripMenuItem tsmiDocSelectAll;
  private ToolStripMenuItem tsmiDocInvert;
  private ToolStripSeparator tsmiDocSep1;
  private ToolStripMenuItem tsmiDocClearAll;
  private TableLayoutPanel tableLayoutPanel5;
  private Button btnCancel;
  private Button btnApply;
  private Label lblSearchString;
  private Label lblSearch;

  private async Task SearchString(string searchString, CancellationToken token)
  {
    DataGridViewCell currentCell = this.dgviewTP.CurrentCell;
    int searchColumnIdx = (currentCell != null ? (currentCell.ColumnIndex > 0 ? 1 : 0) : 0) != 0 ? this.dgviewTP.CurrentCell.ColumnIndex : 1;
    (DataGridViewCell dataGridViewCell, DataGridViewRow dataGridViewRow) = await Task.Run<(DataGridViewCell, DataGridViewRow)>((Func<(DataGridViewCell, DataGridViewRow)>) (() =>
    {
      foreach (DataGridViewRow row in (IEnumerable) this.dgviewTP.Rows)
      {
        token.ThrowIfCancellationRequested();
        DataGridViewCell cell = row.Cells[searchColumnIdx];
        if (cell.Value.ToString().StartsWith(searchString, true, CultureInfo.InvariantCulture))
          return (cell, row);
      }
      return ((DataGridViewCell) null, (DataGridViewRow) null);
    }), token);
    this.dgviewTP.FirstDisplayedScrollingRowIndex = dataGridViewRow != null ? dataGridViewRow.Index : this.dgviewTP.FirstDisplayedScrollingRowIndex;
    this.dgviewTP.CurrentCell = dataGridViewCell ?? this.dgviewTP.CurrentCell;
  }

  protected void InitializeCustomControls()
  {
    DataGridViewTextBoxColumn viewTextBoxColumn1 = new DataGridViewTextBoxColumn();
    DataGridViewTextBoxColumn viewTextBoxColumn2 = new DataGridViewTextBoxColumn();
    DataGridViewTextBoxColumn viewTextBoxColumn3 = new DataGridViewTextBoxColumn();
    DataGridViewTextBoxColumn viewTextBoxColumn4 = new DataGridViewTextBoxColumn();
    DataGridViewCheckBoxCell viewCheckBoxCell1 = new DataGridViewCheckBoxCell();
    viewCheckBoxCell1.FalseValue = (object) false;
    viewCheckBoxCell1.TrueValue = (object) true;
    viewCheckBoxCell1.Value = (object) false;
    viewCheckBoxCell1.ValueType = typeof (bool);
    viewCheckBoxCell1.ReadOnly = false;
    DataGridViewCheckBoxCell viewCheckBoxCell2 = viewCheckBoxCell1;
    DataGridViewCheckBoxColumn viewCheckBoxColumn1 = new DataGridViewCheckBoxColumn();
    viewCheckBoxColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
    viewCheckBoxColumn1.HeaderText = string.Empty;
    viewCheckBoxColumn1.Name = "COL_CHECK_BOX";
    viewCheckBoxColumn1.ReadOnly = false;
    viewCheckBoxColumn1.Visible = true;
    viewCheckBoxColumn1.CellTemplate = (DataGridViewCell) viewCheckBoxCell2;
    viewCheckBoxColumn1.Width = 20;
    DataGridViewCheckBoxColumn viewCheckBoxColumn2 = viewCheckBoxColumn1;
    viewTextBoxColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
    viewTextBoxColumn1.DataPropertyName = "F_KEY";
    viewTextBoxColumn1.HeaderText = "Код TechCard";
    viewTextBoxColumn1.Name = "F_KEY";
    viewTextBoxColumn1.ReadOnly = true;
    viewTextBoxColumn1.Visible = true;
    viewTextBoxColumn1.Width = 90;
    viewTextBoxColumn2.DataPropertyName = "F_DESIGNATION";
    viewTextBoxColumn2.HeaderText = "Обозначение";
    viewTextBoxColumn2.Name = "F_DESIGNATION";
    viewTextBoxColumn2.ReadOnly = true;
    viewTextBoxColumn3.DataPropertyName = "F_NAME";
    viewTextBoxColumn3.HeaderText = "Наименование";
    viewTextBoxColumn3.Name = "F_NAME";
    viewTextBoxColumn3.ReadOnly = true;
    viewTextBoxColumn4.DataPropertyName = "F_VERSION";
    viewTextBoxColumn4.HeaderText = "Версия";
    viewTextBoxColumn4.Name = "F_VERSION";
    viewTextBoxColumn4.ReadOnly = true;
    this.dgviewTP.Columns.Clear();
    this.dgviewTP.Columns.AddRange((DataGridViewColumn) viewCheckBoxColumn2, (DataGridViewColumn) viewTextBoxColumn1, (DataGridViewColumn) viewTextBoxColumn2, (DataGridViewColumn) viewTextBoxColumn3, (DataGridViewColumn) viewTextBoxColumn4);
    this._idx_fld_Key = viewTextBoxColumn1.Index;
    this._idx_fld_Checked = viewCheckBoxColumn2.Index;
  }

  protected void LoadCustomData()
  {
    DataTable docTable = TechSettingsDocList.GetDocTable();
    if (docTable == null)
      return;
    this.dgviewTP.DataSource = (object) docTable;
  }

  private void SelectRows(bool select, bool invertSelection)
  {
    this.dgviewTP.CurrentCell = (DataGridViewCell) null;
    foreach (DataGridViewRow row in (IEnumerable) this.dgviewTP.Rows)
    {
      bool boolean = Convert.ToBoolean(row.Cells[this._idx_fld_Checked].Value);
      row.Cells[this._idx_fld_Checked].Value = (object) (bool) (invertSelection ? (!boolean ? 1 : 0) : (select ? 1 : 0));
    }
  }

  protected void RowsSelectAll() => this.SelectRows(true, false);

  protected void RowsClearAll() => this.SelectRows(false, false);

  protected void RowsInvertSelection() => this.SelectRows(false, true);

  public TechSettingsDocList()
  {
    this.InitializeComponent();
    this.InitializeCustomControls();
  }

  private void tsmiDocSelectAll_Click(object sender, EventArgs e) => this.RowsSelectAll();

  private void tsmiDocInvert_Click(object sender, EventArgs e) => this.RowsInvertSelection();

  private void tsmiDocClearAll_Click(object sender, EventArgs e) => this.RowsClearAll();

  private void cmsDocList_Opening(object sender, CancelEventArgs e)
  {
    this.tsmiDocInvert.Enabled = this.tsmiDocSelectAll.Enabled = this.tsmiDocClearAll.Enabled = this.dgviewTP.Rows.Count > 0;
  }

  private void TechSettingsDocList_Load(object sender, EventArgs e) => this.LoadCustomData();

  private void dgviewTP_KeyPress(object sender, KeyPressEventArgs e)
  {
    if (!char.IsLetterOrDigit(e.KeyChar))
    {
      if (Array.IndexOf<char>(new char[24]
      {
        '-',
        ' ',
        '/',
        '\\',
        '|',
        '=',
        '_',
        '+',
        ':',
        ';',
        '!',
        '@',
        '^',
        '&',
        '$',
        '#',
        '(',
        ')',
        '[',
        ']',
        '{',
        '}',
        '\'',
        '"'
      }, e.KeyChar) < 0)
        return;
    }
    this.lblSearchString.Text += e.KeyChar.ToString();
  }

  private void dgviewTP_KeyDown(object sender, KeyEventArgs e)
  {
    switch (e.KeyCode)
    {
      case Keys.Back:
        if (this.lblSearchString.Text.IsEmpty())
          break;
        this.lblSearchString.Text = this.lblSearchString.Text.Remove(this.lblSearchString.Text.Length - 1, 1);
        break;
      case Keys.Escape:
        this.lblSearchString.Text = string.Empty;
        break;
    }
  }

  private async void lblSearchString_TextChanged(object sender, EventArgs e)
  {
    if (this._ctsSearch != null && !this._ctsSearch.IsCancellationRequested)
      this._ctsSearch.Cancel();
    if (this.lblSearchString.Text.IsEmpty())
      return;
    this._ctsSearch = new CancellationTokenSource();
    try
    {
      await this.SearchString(this.lblSearchString.Text, this._ctsSearch.Token);
    }
    catch (OperationCanceledException ex)
    {
    }
  }

  public List<int> DocList
  {
    get
    {
      List<int> docList = new List<int>();
      foreach (DataGridViewRow row in (IEnumerable) this.dgviewTP.Rows)
      {
        if (Convert.ToBoolean(row.Cells[this._idx_fld_Checked].Value))
        {
          int int32 = Convert.ToInt32(row.Cells[this._idx_fld_Key].Value);
          docList.Add(int32);
        }
      }
      return docList;
    }
    set
    {
      this.RowsClearAll();
      if (value == null || value.Count == 0 || this._idx_fld_Key == -1)
        return;
      value.Sort();
      foreach (DataGridViewRow row in (IEnumerable) this.dgviewTP.Rows)
      {
        int int32 = Convert.ToInt32(row.Cells[this._idx_fld_Key].Value);
        if (value.BinarySearch(int32) >= 0)
          row.Cells[this._idx_fld_Checked].Value = (object) true;
      }
    }
  }

  public static DataTable GetDocTable() => TechSettingsDocList.GetDocTable(string.Empty);

  public static DataTable GetDocTable(string sqlCond)
  {
    string sqlText = $" SELECT DISTINCT    A.F_KEY,         A.F_DESIGNATION,    A.F_NAME,        A.F_VERSION     FROM             TP_VERSIONS A,   TC_ARCDOCS B    WHERE             A.F_TCKEY = B.F_KEY   AND B.F_KIND IN ({TechProcDataBuilder<TechPumpBase>.GetTechProcKindSqlCond()}) ";
    if (!string.IsNullOrEmpty(sqlCond))
      sqlText = $"{sqlText} AND {sqlCond}";
    DataSet dataSet = new DataSet();
    try
    {
      TechcardConsts.Plugin.idb.GetDataAdapter(sqlText).Fill(dataSet);
    }
    catch (Exception ex)
    {
      TechcardConsts.Plugin.appManager.AddErrorMessage($"Ошибка получения списка ТП: {ex.Message}");
      return (DataTable) null;
    }
    return dataSet.Tables.Count <= 0 ? (DataTable) null : dataSet.Tables[0];
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    this.dgviewTP = new DataGridView();
    this.cmsDocList = new ContextMenuStrip(this.components);
    this.tsmiDocSelectAll = new ToolStripMenuItem();
    this.tsmiDocInvert = new ToolStripMenuItem();
    this.tsmiDocSep1 = new ToolStripSeparator();
    this.tsmiDocClearAll = new ToolStripMenuItem();
    this.tableLayoutPanel5 = new TableLayoutPanel();
    this.lblSearch = new Label();
    this.lblSearchString = new Label();
    this.btnCancel = new Button();
    this.btnApply = new Button();
    ((ISupportInitialize) this.dgviewTP).BeginInit();
    this.cmsDocList.SuspendLayout();
    this.tableLayoutPanel5.SuspendLayout();
    this.SuspendLayout();
    this.dgviewTP.AllowUserToAddRows = false;
    this.dgviewTP.AllowUserToDeleteRows = false;
    this.dgviewTP.AllowUserToResizeRows = false;
    this.dgviewTP.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
    this.dgviewTP.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    this.dgviewTP.ContextMenuStrip = this.cmsDocList;
    this.dgviewTP.Dock = DockStyle.Fill;
    this.dgviewTP.EditMode = DataGridViewEditMode.EditOnKeystroke;
    this.dgviewTP.Location = new Point(0, 0);
    this.dgviewTP.MultiSelect = false;
    this.dgviewTP.Name = "dgviewTP";
    this.dgviewTP.RowHeadersWidth = 25;
    this.dgviewTP.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
    this.dgviewTP.SelectionMode = DataGridViewSelectionMode.CellSelect;
    this.dgviewTP.Size = new Size(692, 338);
    this.dgviewTP.TabIndex = 7;
    this.dgviewTP.KeyDown += new KeyEventHandler(this.dgviewTP_KeyDown);
    this.dgviewTP.KeyPress += new KeyPressEventHandler(this.dgviewTP_KeyPress);
    this.cmsDocList.Items.AddRange(new ToolStripItem[4]
    {
      (ToolStripItem) this.tsmiDocSelectAll,
      (ToolStripItem) this.tsmiDocInvert,
      (ToolStripItem) this.tsmiDocSep1,
      (ToolStripItem) this.tsmiDocClearAll
    });
    this.cmsDocList.Name = "cmsDocList";
    this.cmsDocList.Size = new Size(222, 76);
    this.cmsDocList.Opening += new CancelEventHandler(this.cmsDocList_Opening);
    this.tsmiDocSelectAll.Name = "tsmiDocSelectAll";
    this.tsmiDocSelectAll.Size = new Size(221, 22);
    this.tsmiDocSelectAll.Text = "Выделить все";
    this.tsmiDocSelectAll.Click += new EventHandler(this.tsmiDocSelectAll_Click);
    this.tsmiDocInvert.Name = "tsmiDocInvert";
    this.tsmiDocInvert.Size = new Size(221, 22);
    this.tsmiDocInvert.Text = "Инвертировать выделение";
    this.tsmiDocInvert.Click += new EventHandler(this.tsmiDocInvert_Click);
    this.tsmiDocSep1.Name = "tsmiDocSep1";
    this.tsmiDocSep1.Size = new Size(218, 6);
    this.tsmiDocClearAll.Name = "tsmiDocClearAll";
    this.tsmiDocClearAll.Size = new Size(221, 22);
    this.tsmiDocClearAll.Text = "Очистить все";
    this.tsmiDocClearAll.Click += new EventHandler(this.tsmiDocClearAll_Click);
    this.tableLayoutPanel5.ColumnCount = 5;
    this.tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50f));
    this.tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 158f));
    this.tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80f));
    this.tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 83f));
    this.tableLayoutPanel5.Controls.Add((Control) this.lblSearch, 0, 0);
    this.tableLayoutPanel5.Controls.Add((Control) this.lblSearchString, 1, 0);
    this.tableLayoutPanel5.Controls.Add((Control) this.btnCancel, 4, 0);
    this.tableLayoutPanel5.Controls.Add((Control) this.btnApply, 3, 0);
    this.tableLayoutPanel5.Dock = DockStyle.Bottom;
    this.tableLayoutPanel5.Location = new Point(0, 338);
    this.tableLayoutPanel5.Name = "tableLayoutPanel5";
    this.tableLayoutPanel5.RowCount = 1;
    this.tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
    this.tableLayoutPanel5.Size = new Size(692, 32 /*0x20*/);
    this.tableLayoutPanel5.TabIndex = 8;
    this.lblSearch.AutoSize = true;
    this.lblSearch.Dock = DockStyle.Fill;
    this.lblSearch.Location = new Point(3, 0);
    this.lblSearch.Name = "lblSearch";
    this.lblSearch.Size = new Size(44, 32 /*0x20*/);
    this.lblSearch.TabIndex = 9;
    this.lblSearch.Text = "Поиск:";
    this.lblSearch.TextAlign = ContentAlignment.MiddleLeft;
    this.lblSearchString.AutoSize = true;
    this.lblSearchString.Dock = DockStyle.Fill;
    this.lblSearchString.Location = new Point(53, 0);
    this.lblSearchString.Name = "lblSearchString";
    this.lblSearchString.Size = new Size(152, 32 /*0x20*/);
    this.lblSearchString.TabIndex = 8;
    this.lblSearchString.TextAlign = ContentAlignment.MiddleLeft;
    this.lblSearchString.TextChanged += new EventHandler(this.lblSearchString_TextChanged);
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.ImeMode = ImeMode.NoControl;
    this.btnCancel.Location = new Point(612, 3);
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.Size = new Size(75, 23);
    this.btnCancel.TabIndex = 6;
    this.btnCancel.Text = "Отмена";
    this.btnCancel.UseVisualStyleBackColor = true;
    this.btnApply.DialogResult = DialogResult.OK;
    this.btnApply.ImeMode = ImeMode.NoControl;
    this.btnApply.Location = new Point(532, 3);
    this.btnApply.Name = "btnApply";
    this.btnApply.Size = new Size(74, 23);
    this.btnApply.TabIndex = 5;
    this.btnApply.Text = "Применить";
    this.btnApply.UseVisualStyleBackColor = true;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(692, 370);
    this.Controls.Add((Control) this.dgviewTP);
    this.Controls.Add((Control) this.tableLayoutPanel5);
    this.Name = nameof (TechSettingsDocList);
    this.Text = "Выберите ТП для закачки";
    this.Load += new EventHandler(this.TechSettingsDocList_Load);
    ((ISupportInitialize) this.dgviewTP).EndInit();
    this.cmsDocList.ResumeLayout(false);
    this.tableLayoutPanel5.ResumeLayout(false);
    this.tableLayoutPanel5.PerformLayout();
    this.ResumeLayout(false);
  }
}
