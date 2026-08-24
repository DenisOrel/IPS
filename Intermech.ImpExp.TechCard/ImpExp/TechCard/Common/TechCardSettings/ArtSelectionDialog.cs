// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.TechCardSettings.ArtSelectionDialog
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Extensions;
using Intermech.ImpExp.SearchData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common.TechCardSettings;

public class ArtSelectionDialog : Form
{
  private const string COL_CHECK_BOX = "COL_CHECK_BOX";
  private int _idx_fld_Checked = -1;
  private int _idx_fld_ArtId = -1;
  private int _idx_fld_ArtVerId = -1;
  private int _idx_fld_VArtId = -1;
  private CancellationTokenSource _ctsSearch;
  private IContainer components;
  private DataGridView dgviewArts;
  private ContextMenuStrip cmsArtList;
  private ToolStripMenuItem tsmiArtSelectAll;
  private ToolStripMenuItem tsmiArtInvert;
  private ToolStripSeparator tsmiDocSep1;
  private ToolStripMenuItem tsmiArtClearAll;
  private TableLayoutPanel tableLayoutPanel5;
  private Button btnCancel;
  private Button btnApply;
  private Label lblSearchString;
  private System.Windows.Forms.Timer tmrSearch;
  private Label lblSearch;

  private async Task SearchString(string searchString, CancellationToken token)
  {
    DataGridViewCell currentCell = this.dgviewArts.CurrentCell;
    int searchColumnIdx = (currentCell != null ? (currentCell.ColumnIndex > 0 ? 1 : 0) : 0) != 0 ? this.dgviewArts.CurrentCell.ColumnIndex : 1;
    (DataGridViewCell dataGridViewCell, DataGridViewRow dataGridViewRow) = await Task.Run<(DataGridViewCell, DataGridViewRow)>((Func<(DataGridViewCell, DataGridViewRow)>) (() =>
    {
      foreach (DataGridViewRow row in (IEnumerable) this.dgviewArts.Rows)
      {
        token.ThrowIfCancellationRequested();
        DataGridViewCell cell = row.Cells[searchColumnIdx];
        if (cell.Value.ToString().StartsWith(searchString, true, CultureInfo.InvariantCulture))
          return (cell, row);
      }
      return ((DataGridViewCell) null, (DataGridViewRow) null);
    }), token);
    this.dgviewArts.FirstDisplayedScrollingRowIndex = dataGridViewRow != null ? dataGridViewRow.Index : this.dgviewArts.FirstDisplayedScrollingRowIndex;
    this.dgviewArts.CurrentCell = dataGridViewCell ?? this.dgviewArts.CurrentCell;
  }

  private void SelectRows(bool select, bool invertSelection)
  {
    this.dgviewArts.CurrentCell = (DataGridViewCell) null;
    foreach (DataGridViewRow row in (IEnumerable) this.dgviewArts.Rows)
    {
      bool boolean = Convert.ToBoolean(row.Cells[this._idx_fld_Checked].Value);
      row.Cells[this._idx_fld_Checked].Value = (object) (bool) (invertSelection ? (!boolean ? 1 : 0) : (select ? 1 : 0));
    }
  }

  protected void InitializeCustomControls()
  {
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
    DataGridViewTextBoxColumn viewTextBoxColumn1 = new DataGridViewTextBoxColumn();
    viewTextBoxColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
    viewTextBoxColumn1.DataPropertyName = "ART_ID";
    viewTextBoxColumn1.HeaderText = "Идентификатор объекта";
    viewTextBoxColumn1.Name = "ART_ID";
    viewTextBoxColumn1.ReadOnly = true;
    viewTextBoxColumn1.Width = 90;
    DataGridViewTextBoxColumn viewTextBoxColumn2 = viewTextBoxColumn1;
    DataGridViewTextBoxColumn viewTextBoxColumn3 = new DataGridViewTextBoxColumn();
    viewTextBoxColumn3.DataPropertyName = "DESIGNATIO";
    viewTextBoxColumn3.HeaderText = "Обозначение";
    viewTextBoxColumn3.Name = "DESIGNATIO";
    viewTextBoxColumn3.ReadOnly = true;
    DataGridViewTextBoxColumn viewTextBoxColumn4 = viewTextBoxColumn3;
    DataGridViewTextBoxColumn viewTextBoxColumn5 = new DataGridViewTextBoxColumn();
    viewTextBoxColumn5.DataPropertyName = "NAME";
    viewTextBoxColumn5.HeaderText = "Наименование";
    viewTextBoxColumn5.Name = "NAME";
    viewTextBoxColumn5.ReadOnly = true;
    DataGridViewTextBoxColumn viewTextBoxColumn6 = viewTextBoxColumn5;
    this.dgviewArts.Columns.Clear();
    this.dgviewArts.Columns.AddRange((DataGridViewColumn) viewCheckBoxColumn2, (DataGridViewColumn) viewTextBoxColumn2, (DataGridViewColumn) viewTextBoxColumn4, (DataGridViewColumn) viewTextBoxColumn6);
    if (!PluginSettings.PumpArtVersions)
      return;
    DataGridViewTextBoxColumn viewTextBoxColumn7 = new DataGridViewTextBoxColumn();
    viewTextBoxColumn7.DataPropertyName = "ART_VER_ID";
    viewTextBoxColumn7.HeaderText = "Номер версии";
    viewTextBoxColumn7.Name = "ART_VER_ID";
    viewTextBoxColumn7.ReadOnly = true;
    this.dgviewArts.Columns.Insert(1, (DataGridViewColumn) viewTextBoxColumn7);
    DataGridViewTextBoxColumn viewTextBoxColumn8 = new DataGridViewTextBoxColumn();
    viewTextBoxColumn8.DataPropertyName = "VART_ID";
    viewTextBoxColumn8.HeaderText = "VART_ID";
    viewTextBoxColumn8.Name = "VART_ID";
    viewTextBoxColumn8.ReadOnly = true;
    viewTextBoxColumn8.Visible = false;
    this.dgviewArts.Columns.Add((DataGridViewColumn) viewTextBoxColumn8);
  }

  public ArtSelectionDialog()
  {
    this.InitializeComponent();
    this.InitializeCustomControls();
  }

  private void tsmiArtSelectAll_Click(object sender, EventArgs e) => this.SelectRows(true, false);

  private void tsmiArtInvert_Click(object sender, EventArgs e) => this.SelectRows(false, true);

  private void tsmiArtClearAll_Click(object sender, EventArgs e) => this.SelectRows(false, false);

  private void cmsArtList_Opening(object sender, CancelEventArgs e)
  {
    this.tsmiArtInvert.Enabled = this.tsmiArtSelectAll.Enabled = this.tsmiArtClearAll.Enabled = this.dgviewArts.Rows.Count > 0;
  }

  private void dgviewArts_KeyPress(object sender, KeyPressEventArgs e)
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

  private void dgviewArts_KeyDown(object sender, KeyEventArgs e)
  {
    switch (e.KeyCode)
    {
      case System.Windows.Forms.Keys.Back:
        if (this.lblSearchString.Text.IsEmpty())
          break;
        this.lblSearchString.Text = this.lblSearchString.Text.Remove(this.lblSearchString.Text.Length - 1, 1);
        break;
      case System.Windows.Forms.Keys.Escape:
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

  public IEnumerable<ArtInfoLight> ArtList
  {
    get
    {
      return !PluginSettings.PumpArtVersions ? this.dgviewArts.Rows.Cast<DataGridViewRow>().Where<DataGridViewRow>((System.Func<DataGridViewRow, bool>) (row => Convert.ToBoolean(row.Cells[this._idx_fld_Checked].Value))).Select<DataGridViewRow, ArtInfoLight>((System.Func<DataGridViewRow, ArtInfoLight>) (row => new ArtInfoLight(Convert.ToInt32(row.Cells[this._idx_fld_ArtId].Value)))) : this.dgviewArts.Rows.Cast<DataGridViewRow>().Where<DataGridViewRow>((System.Func<DataGridViewRow, bool>) (row => Convert.ToBoolean(row.Cells[this._idx_fld_Checked].Value))).Select<DataGridViewRow, ArtInfoLight>((System.Func<DataGridViewRow, ArtInfoLight>) (row => new ArtInfoLight(Convert.ToInt32(row.Cells[this._idx_fld_ArtId].Value), Convert.ToInt32(row.Cells[this._idx_fld_ArtVerId].Value), Convert.ToInt32(row.Cells[this._idx_fld_VArtId].Value))));
    }
    set
    {
      this.SelectRows(false, false);
      if (value == null)
        return;
      Comparer<ArtInfoLight> comparer = Comparer<ArtInfoLight>.Create((Comparison<ArtInfoLight>) ((left, right) =>
      {
        int num = left.ArtId.CompareTo(right.ArtId);
        return num == 0 ? left.ArtVer.CompareTo(right.ArtVer) : num;
      }));
      List<ArtInfoLight> list = value.ToList<ArtInfoLight>();
      list.Sort((IComparer<ArtInfoLight>) comparer);
      foreach (DataGridViewRow row in (IEnumerable) this.dgviewArts.Rows)
      {
        int int32 = Convert.ToInt32(row.Cells[this._idx_fld_ArtId].Value);
        (int artVer, int vArtId) = PluginSettings.PumpArtVersions ? (Convert.ToInt32(row.Cells[this._idx_fld_ArtVerId].Value), Convert.ToInt32(row.Cells[this._idx_fld_VArtId].Value)) : (-1, -1);
        if (list.BinarySearch(new ArtInfoLight(int32, artVer, vArtId)) >= 0)
          row.Cells[this._idx_fld_Checked].Value = (object) true;
      }
    }
  }

  public void LoadArtsTable(string sqlCond)
  {
    string sqlText;
    if (PluginSettings.PumpArtVersions)
    {
      sqlText = "select ART_ID, ART_VER_ID, VART_ID, DESIGNATIO, NAME from V_ARTICLES";
      if (!string.IsNullOrEmpty(sqlCond))
        sqlText = $"{sqlText} where {sqlCond}";
    }
    else
    {
      sqlText = "select ART_ID, DESIGNATIO, NAME from ARTICLES where ART_ID > 0 ";
      if (!string.IsNullOrEmpty(sqlCond))
        sqlText = $"{sqlText} and {sqlCond}";
    }
    DataSet dataSet = new DataSet();
    try
    {
      SearchConnectionsManager.GetConnection().GetDataAdapter(sqlText).Fill(dataSet);
    }
    catch (Exception ex)
    {
      TechcardConsts.Plugin.appManager.AddErrorMessage("Ошибка получения списка объектов: " + ex.Message);
    }
    this.dgviewArts.DataSource = dataSet.Tables.Count > 0 ? (object) dataSet.Tables[0] : (object) (DataTable) null;
    foreach (DataGridViewColumn column in (BaseCollection) this.dgviewArts.Columns)
    {
      switch (column.Name)
      {
        case "ART_ID":
          this._idx_fld_ArtId = column.Index;
          continue;
        case "ART_VER_ID":
          this._idx_fld_ArtVerId = column.Index;
          continue;
        case "VART_ID":
          this._idx_fld_VArtId = column.Index;
          continue;
        case "COL_CHECK_BOX":
          this._idx_fld_Checked = column.Index;
          continue;
        default:
          continue;
      }
    }
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
    this.dgviewArts = new DataGridView();
    this.cmsArtList = new ContextMenuStrip(this.components);
    this.tsmiArtSelectAll = new ToolStripMenuItem();
    this.tsmiArtInvert = new ToolStripMenuItem();
    this.tsmiDocSep1 = new ToolStripSeparator();
    this.tsmiArtClearAll = new ToolStripMenuItem();
    this.tableLayoutPanel5 = new TableLayoutPanel();
    this.lblSearch = new Label();
    this.btnCancel = new Button();
    this.btnApply = new Button();
    this.lblSearchString = new Label();
    this.tmrSearch = new System.Windows.Forms.Timer(this.components);
    ((ISupportInitialize) this.dgviewArts).BeginInit();
    this.cmsArtList.SuspendLayout();
    this.tableLayoutPanel5.SuspendLayout();
    this.SuspendLayout();
    this.dgviewArts.AllowUserToAddRows = false;
    this.dgviewArts.AllowUserToDeleteRows = false;
    this.dgviewArts.AllowUserToResizeRows = false;
    this.dgviewArts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
    this.dgviewArts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    this.dgviewArts.ContextMenuStrip = this.cmsArtList;
    this.dgviewArts.Dock = DockStyle.Fill;
    this.dgviewArts.EditMode = DataGridViewEditMode.EditOnKeystroke;
    this.dgviewArts.Location = new Point(0, 0);
    this.dgviewArts.MultiSelect = false;
    this.dgviewArts.Name = "dgviewArts";
    this.dgviewArts.RowHeadersWidth = 25;
    this.dgviewArts.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
    this.dgviewArts.SelectionMode = DataGridViewSelectionMode.CellSelect;
    this.dgviewArts.Size = new Size(692, 338);
    this.dgviewArts.TabIndex = 7;
    this.dgviewArts.KeyDown += new KeyEventHandler(this.dgviewArts_KeyDown);
    this.dgviewArts.KeyPress += new KeyPressEventHandler(this.dgviewArts_KeyPress);
    this.cmsArtList.Items.AddRange(new ToolStripItem[4]
    {
      (ToolStripItem) this.tsmiArtSelectAll,
      (ToolStripItem) this.tsmiArtInvert,
      (ToolStripItem) this.tsmiDocSep1,
      (ToolStripItem) this.tsmiArtClearAll
    });
    this.cmsArtList.Name = "cmsDocList";
    this.cmsArtList.Size = new Size(222, 76);
    this.cmsArtList.Opening += new CancelEventHandler(this.cmsArtList_Opening);
    this.tsmiArtSelectAll.Name = "tsmiArtSelectAll";
    this.tsmiArtSelectAll.Size = new Size(221, 22);
    this.tsmiArtSelectAll.Text = "Выделить все";
    this.tsmiArtSelectAll.Click += new EventHandler(this.tsmiArtSelectAll_Click);
    this.tsmiArtInvert.Name = "tsmiArtInvert";
    this.tsmiArtInvert.Size = new Size(221, 22);
    this.tsmiArtInvert.Text = "Инвертировать выделение";
    this.tsmiArtInvert.Click += new EventHandler(this.tsmiArtInvert_Click);
    this.tsmiDocSep1.Name = "tsmiDocSep1";
    this.tsmiDocSep1.Size = new Size(218, 6);
    this.tsmiArtClearAll.Name = "tsmiArtClearAll";
    this.tsmiArtClearAll.Size = new Size(221, 22);
    this.tsmiArtClearAll.Text = "Очистить все";
    this.tsmiArtClearAll.Click += new EventHandler(this.tsmiArtClearAll_Click);
    this.tableLayoutPanel5.ColumnCount = 5;
    this.tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50f));
    this.tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 158f));
    this.tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80f));
    this.tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 83f));
    this.tableLayoutPanel5.Controls.Add((Control) this.lblSearch, 0, 0);
    this.tableLayoutPanel5.Controls.Add((Control) this.btnCancel, 4, 0);
    this.tableLayoutPanel5.Controls.Add((Control) this.btnApply, 3, 0);
    this.tableLayoutPanel5.Controls.Add((Control) this.lblSearchString, 1, 0);
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
    this.lblSearch.TabIndex = 8;
    this.lblSearch.Text = "Поиск:";
    this.lblSearch.TextAlign = ContentAlignment.MiddleLeft;
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
    this.lblSearchString.AutoSize = true;
    this.lblSearchString.Dock = DockStyle.Fill;
    this.lblSearchString.Location = new Point(53, 0);
    this.lblSearchString.Name = "lblSearchString";
    this.lblSearchString.Size = new Size(152, 32 /*0x20*/);
    this.lblSearchString.TabIndex = 7;
    this.lblSearchString.TextAlign = ContentAlignment.MiddleLeft;
    this.lblSearchString.TextChanged += new EventHandler(this.lblSearchString_TextChanged);
    this.tmrSearch.Interval = 1000;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(692, 370);
    this.Controls.Add((Control) this.dgviewArts);
    this.Controls.Add((Control) this.tableLayoutPanel5);
    this.Name = nameof (ArtSelectionDialog);
    this.Text = "Выберите объекты для закачки";
    ((ISupportInitialize) this.dgviewArts).EndInit();
    this.cmsArtList.ResumeLayout(false);
    this.tableLayoutPanel5.ResumeLayout(false);
    this.tableLayoutPanel5.PerformLayout();
    this.ResumeLayout(false);
  }
}
