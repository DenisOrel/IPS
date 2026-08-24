// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.CoatingsSettingsForm
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Imbase;
using Intermech.Interfaces.MaterialsHandbook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class CoatingsSettingsForm : Form
{
  private IMHCoatingsSystemSettings _settings;
  private List<Guid> _attrGuids = new List<Guid>();
  private IContainer components;
  private Panel _pnlBottom;
  private Button _btnCancel;
  private Button _btnOK;
  private DataGridView _dgv;
  private DataSet _ds;
  private DataColumn dataColumn1;
  private DataColumn dataColumn2;
  private DataGridViewComboBoxColumn colP1DataGridViewTextBoxColumn;
  private DataTable dataTable2;
  private DataColumn dataColumn3;
  private DataColumn dataColumn4;
  private DataGridViewComboBoxColumn colP2DataGridViewTextBoxColumn;
  private DataTable dataTable1;

  public IMHCoatingsSystemSettings Settings
  {
    get => new IMHCoatingsSystemSettings(this._ds.Tables["_dtParams"]);
  }

  public CoatingsSettingsForm(Guid tableGuid, IMHCoatingsSystemSettings settings)
  {
    this.InitializeComponent();
    this._settings = settings;
    this.LoadDataSource(tableGuid);
    this.LoadParams();
  }

  private void LoadDataSource(Guid tableGuid)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(tableGuid);
      if (objectInfo.Empty || !(sessionKeeper.Session.GetCustomService(typeof (IImbaseServer)) is IImbaseServer customService))
        return;
      DataTable recordsTable = (DataTable) null;
      AttributeTypeProperties[] columnsAttributes = (AttributeTypeProperties[]) null;
      ImbaseKeyInfo keyInfo = new ImbaseKeyInfo(-1L);
      customService.LoadRecords(sessionKeeper.Session.SessionGUID, objectInfo.ObjectID, string.Empty, Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator, out recordsTable, out columnsAttributes, out keyInfo);
      if (columnsAttributes == null || columnsAttributes.Length == 0)
        return;
      DataTable table = this._ds.Tables["_dtAttrs"];
      table.Rows.Add((object) " ", (object) DBNull.Value);
      foreach (AttributeTypeProperties attributeTypeProperties in columnsAttributes)
      {
        table.Rows.Add((object) attributeTypeProperties.Name, (object) attributeTypeProperties.AttributeGuid);
        if (!this._attrGuids.Contains(attributeTypeProperties.AttributeGuid))
          this._attrGuids.Add(attributeTypeProperties.AttributeGuid);
      }
    }
  }

  private void LoadParams()
  {
    DataTable table = this._ds.Tables["_dtParams"];
    DataTable dataTable = this._settings != null ? this._settings.Params : (DataTable) null;
    for (int index = 0; index < 8; ++index)
      table.Rows.Add(table.NewRow());
    if (dataTable == null || dataTable.Rows.Count <= 0 || !dataTable.Columns.Contains("P1") || !dataTable.Columns.Contains("P2"))
      return;
    string empty = string.Empty;
    for (int index = 0; index < dataTable.Rows.Count && table.Rows.Count != index; ++index)
    {
      object[] objArray = new object[2]
      {
        dataTable.Rows[index]["P1"],
        dataTable.Rows[index]["P2"]
      };
      int num = 1;
      foreach (object obj in objArray)
      {
        string str = Convert.ToString(obj);
        if (GuidHelper.IsGuid(str))
        {
          Guid guid = new Guid(str);
          if (this._attrGuids.Contains(guid))
            table.Rows[index][$"P{num++}"] = (object) guid;
        }
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (CoatingsSettingsForm));
    this._pnlBottom = new Panel();
    this._btnCancel = new Button();
    this._btnOK = new Button();
    this._dgv = new DataGridView();
    this.colP1DataGridViewTextBoxColumn = new DataGridViewComboBoxColumn();
    this._ds = new DataSet();
    this.dataTable1 = new DataTable();
    this.dataColumn1 = new DataColumn();
    this.dataColumn2 = new DataColumn();
    this.dataTable2 = new DataTable();
    this.dataColumn3 = new DataColumn();
    this.dataColumn4 = new DataColumn();
    this.colP2DataGridViewTextBoxColumn = new DataGridViewComboBoxColumn();
    this._pnlBottom.SuspendLayout();
    ((ISupportInitialize) this._dgv).BeginInit();
    this._ds.BeginInit();
    this.dataTable1.BeginInit();
    this.dataTable2.BeginInit();
    this.SuspendLayout();
    this._pnlBottom.Controls.Add((Control) this._btnCancel);
    this._pnlBottom.Controls.Add((Control) this._btnOK);
    componentResourceManager.ApplyResources((object) this._pnlBottom, "_pnlBottom");
    this._pnlBottom.Name = "_pnlBottom";
    componentResourceManager.ApplyResources((object) this._btnCancel, "_btnCancel");
    this._btnCancel.DialogResult = DialogResult.Cancel;
    this._btnCancel.Name = "_btnCancel";
    this._btnCancel.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this._btnOK, "_btnOK");
    this._btnOK.DialogResult = DialogResult.OK;
    this._btnOK.Name = "_btnOK";
    this._btnOK.UseVisualStyleBackColor = true;
    this._dgv.AllowUserToAddRows = false;
    this._dgv.AllowUserToDeleteRows = false;
    this._dgv.AllowUserToResizeRows = false;
    this._dgv.AutoGenerateColumns = false;
    this._dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    this._dgv.Columns.AddRange((DataGridViewColumn) this.colP1DataGridViewTextBoxColumn, (DataGridViewColumn) this.colP2DataGridViewTextBoxColumn);
    this._dgv.DataMember = "_dtParams";
    this._dgv.DataSource = (object) this._ds;
    componentResourceManager.ApplyResources((object) this._dgv, "_dgv");
    this._dgv.MultiSelect = false;
    this._dgv.Name = "_dgv";
    this._dgv.RowHeadersVisible = false;
    this._dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;
    this.colP1DataGridViewTextBoxColumn.DataPropertyName = "P1";
    this.colP1DataGridViewTextBoxColumn.DataSource = (object) this._ds;
    this.colP1DataGridViewTextBoxColumn.DisplayMember = "_dtAttrs._colName";
    componentResourceManager.ApplyResources((object) this.colP1DataGridViewTextBoxColumn, "colP1DataGridViewTextBoxColumn");
    this.colP1DataGridViewTextBoxColumn.Name = "colP1DataGridViewTextBoxColumn";
    this.colP1DataGridViewTextBoxColumn.Resizable = DataGridViewTriState.True;
    this.colP1DataGridViewTextBoxColumn.SortMode = DataGridViewColumnSortMode.Automatic;
    this.colP1DataGridViewTextBoxColumn.ValueMember = "_dtAttrs._colValue";
    this._ds.DataSetName = "NewDataSet";
    this._ds.Tables.AddRange(new DataTable[2]
    {
      this.dataTable1,
      this.dataTable2
    });
    this.dataTable1.Columns.AddRange(new DataColumn[2]
    {
      this.dataColumn1,
      this.dataColumn2
    });
    this.dataTable1.TableName = "_dtAttrs";
    this.dataColumn1.Caption = "Наименование";
    this.dataColumn1.ColumnName = "_colName";
    this.dataColumn2.Caption = "Значение";
    this.dataColumn2.ColumnName = "_colValue";
    this.dataColumn2.DataType = typeof (Guid);
    this.dataTable2.Columns.AddRange(new DataColumn[2]
    {
      this.dataColumn3,
      this.dataColumn4
    });
    this.dataTable2.TableName = "_dtParams";
    this.dataColumn3.Caption = "Параметр 1";
    this.dataColumn3.ColumnName = "P1";
    this.dataColumn3.DataType = typeof (Guid);
    this.dataColumn4.Caption = "Параметр 2";
    this.dataColumn4.ColumnName = "P2";
    this.dataColumn4.DataType = typeof (Guid);
    this.colP2DataGridViewTextBoxColumn.DataPropertyName = "P2";
    this.colP2DataGridViewTextBoxColumn.DataSource = (object) this._ds;
    this.colP2DataGridViewTextBoxColumn.DisplayMember = "_dtAttrs._colName";
    componentResourceManager.ApplyResources((object) this.colP2DataGridViewTextBoxColumn, "colP2DataGridViewTextBoxColumn");
    this.colP2DataGridViewTextBoxColumn.Name = "colP2DataGridViewTextBoxColumn";
    this.colP2DataGridViewTextBoxColumn.Resizable = DataGridViewTriState.True;
    this.colP2DataGridViewTextBoxColumn.SortMode = DataGridViewColumnSortMode.Automatic;
    this.colP2DataGridViewTextBoxColumn.ValueMember = "_dtAttrs._colValue";
    this.AcceptButton = (IButtonControl) this._btnOK;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this._btnCancel;
    this.Controls.Add((Control) this._dgv);
    this.Controls.Add((Control) this._pnlBottom);
    this.DoubleBuffered = true;
    this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
    this.Name = nameof (CoatingsSettingsForm);
    this.ShowIcon = false;
    this.ShowInTaskbar = false;
    this._pnlBottom.ResumeLayout(false);
    ((ISupportInitialize) this._dgv).EndInit();
    this._ds.EndInit();
    this.dataTable1.EndInit();
    this.dataTable2.EndInit();
    this.ResumeLayout(false);
  }
}
