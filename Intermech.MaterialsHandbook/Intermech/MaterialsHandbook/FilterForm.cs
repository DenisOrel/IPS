// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.FilterForm
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
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class FilterForm : Form
{
  private Dictionary<DataRow, Guid> _rows;
  private IContainer components;
  private Panel _pnlBottom;
  private Button _btnOK;
  private Button _btnCancel;
  private DataSet _ds;
  private DataTable conditions;
  private DataColumn dataColumn2;
  private DataColumn dataColumn3;
  private DataColumn dataColumn4;
  private DataTable condsMap;
  private DataColumn dataColumn5;
  private DataColumn dataColumn6;
  private DataGridView _dgv;
  private DataGridViewTextBoxColumn F_NAME;
  private DataGridViewComboBoxColumn F_COND;
  private DataGridViewTextBoxColumn F_DATA;

  public FilterList Filter { get; private set; }

  public FilterForm(List<string> attrGuids, FilterList filter)
  {
    this.InitializeComponent();
    this.Filter = filter;
    this.FillConditionsMap();
    this.LoadData(attrGuids);
  }

  protected override void OnClosing(CancelEventArgs e)
  {
    base.OnClosing(e);
    if (this.DialogResult != DialogResult.OK)
      return;
    DataTable table = this._ds.Tables[sc_14467.ssp_imbase_14468()];
    this.Filter.Clear();
    foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
    {
      object obj = row[1];
      Condition cond = obj is DBNull ? Condition.None : (Condition) obj;
      this.Filter.SetValue(this._rows[row], cond, row[2]);
    }
  }

  private int AddRow(string strGuid)
  {
    int num = -1;
    Guid guid = new Guid(strGuid);
    IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(guid);
    if (attributeType == null)
      return num;
    DataTable table = this._ds.Tables["Conditions"];
    SortamentFilter sortamentFilter = this.Filter[guid];
    DataRow dataRow = table.NewRow();
    dataRow["F_NAME"] = (object) attributeType.Name;
    dataRow["F_COND"] = (object) sortamentFilter.Cond;
    dataRow["F_DATA"] = sortamentFilter.Value;
    table.Rows.Add(dataRow);
    this._rows.Add(dataRow, guid);
    return num;
  }

  private void FillConditionsMap()
  {
    this.condsMap.Clear();
    this.condsMap.Rows.Add((object) Condition.None, (object) "");
    this.condsMap.Rows.Add((object) Condition.Equal, (object) LocalizationHolder.rm.GetString(sc_14467.ssp_imbase_14469()));
    this.condsMap.Rows.Add((object) Condition.NotEqual, (object) LocalizationHolder.rm.GetString("IMH.NotEqual"));
    this.condsMap.Rows.Add((object) Condition.Great, (object) LocalizationHolder.rm.GetString("IMH.Great"));
    this.condsMap.Rows.Add((object) Condition.GreatOrEqual, (object) LocalizationHolder.rm.GetString("IMH.GreatOrEqual"));
    this.condsMap.Rows.Add((object) Condition.Less, (object) LocalizationHolder.rm.GetString("IMH.Less"));
    this.condsMap.Rows.Add((object) Condition.LessOrEqual, (object) LocalizationHolder.rm.GetString("IMH.LessOrEqual"));
  }

  private void LoadData(List<string> attrGuids)
  {
    if (attrGuids == null)
      return;
    this._rows = new Dictionary<DataRow, Guid>(attrGuids.Count);
    attrGuids.ForEach((Action<string>) (x => this.AddRow(x)));
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FilterForm));
    this._pnlBottom = new Panel();
    this._btnOK = new Button();
    this._btnCancel = new Button();
    this._ds = new DataSet();
    this.conditions = new DataTable();
    this.dataColumn2 = new DataColumn();
    this.dataColumn3 = new DataColumn();
    this.dataColumn4 = new DataColumn();
    this.condsMap = new DataTable();
    this.dataColumn5 = new DataColumn();
    this.dataColumn6 = new DataColumn();
    this._dgv = new DataGridView();
    this.F_NAME = new DataGridViewTextBoxColumn();
    this.F_COND = new DataGridViewComboBoxColumn();
    this.F_DATA = new DataGridViewTextBoxColumn();
    this._pnlBottom.SuspendLayout();
    this._ds.BeginInit();
    this.conditions.BeginInit();
    this.condsMap.BeginInit();
    ((ISupportInitialize) this._dgv).BeginInit();
    this.SuspendLayout();
    this._pnlBottom.Controls.Add((Control) this._btnOK);
    this._pnlBottom.Controls.Add((Control) this._btnCancel);
    componentResourceManager.ApplyResources((object) this._pnlBottom, "_pnlBottom");
    this._pnlBottom.Name = "_pnlBottom";
    componentResourceManager.ApplyResources((object) this._btnOK, "_btnOK");
    this._btnOK.DialogResult = DialogResult.OK;
    this._btnOK.Name = "_btnOK";
    this._btnOK.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this._btnCancel, "_btnCancel");
    this._btnCancel.DialogResult = DialogResult.Cancel;
    this._btnCancel.Name = "_btnCancel";
    this._btnCancel.UseVisualStyleBackColor = true;
    this._ds.DataSetName = "NewDataSet";
    this._ds.Tables.AddRange(new DataTable[2]
    {
      this.conditions,
      this.condsMap
    });
    this.conditions.Columns.AddRange(new DataColumn[3]
    {
      this.dataColumn2,
      this.dataColumn3,
      this.dataColumn4
    });
    this.conditions.TableName = "Conditions";
    this.dataColumn2.ColumnName = "F_NAME";
    this.dataColumn2.DataType = typeof (object);
    this.dataColumn3.ColumnName = "F_COND";
    this.dataColumn3.DataType = typeof (object);
    this.dataColumn4.ColumnName = "F_DATA";
    this.condsMap.Columns.AddRange(new DataColumn[2]
    {
      this.dataColumn5,
      this.dataColumn6
    });
    this.condsMap.Constraints.AddRange(new Constraint[1]
    {
      (Constraint) new UniqueConstraint("Constraint1", new string[1]
      {
        "F_COND"
      }, true)
    });
    this.condsMap.PrimaryKey = new DataColumn[1]
    {
      this.dataColumn5
    };
    this.condsMap.TableName = "CondsMap";
    this.dataColumn5.AllowDBNull = false;
    this.dataColumn5.ColumnName = "F_COND";
    this.dataColumn5.DataType = typeof (object);
    this.dataColumn6.ColumnName = "F_NAME";
    this._dgv.AllowUserToAddRows = false;
    this._dgv.AllowUserToDeleteRows = false;
    this._dgv.AllowUserToResizeRows = false;
    this._dgv.AutoGenerateColumns = false;
    this._dgv.BackgroundColor = SystemColors.Window;
    this._dgv.BorderStyle = BorderStyle.Fixed3D;
    this._dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    this._dgv.Columns.AddRange((DataGridViewColumn) this.F_NAME, (DataGridViewColumn) this.F_COND, (DataGridViewColumn) this.F_DATA);
    this._dgv.DataMember = "Conditions";
    this._dgv.DataSource = (object) this._ds;
    componentResourceManager.ApplyResources((object) this._dgv, "_dgv");
    this._dgv.EditMode = DataGridViewEditMode.EditOnEnter;
    this._dgv.Name = "_dgv";
    this._dgv.RowHeadersVisible = false;
    this.F_NAME.DataPropertyName = "F_NAME";
    componentResourceManager.ApplyResources((object) this.F_NAME, "F_NAME");
    this.F_NAME.Name = "F_NAME";
    this.F_NAME.ReadOnly = true;
    this.F_COND.DataPropertyName = "F_COND";
    this.F_COND.DataSource = (object) this._ds;
    this.F_COND.DisplayMember = "CondsMap.F_NAME";
    componentResourceManager.ApplyResources((object) this.F_COND, "F_COND");
    this.F_COND.MaxDropDownItems = 13;
    this.F_COND.Name = "F_COND";
    this.F_COND.Resizable = DataGridViewTriState.True;
    this.F_COND.ValueMember = "CondsMap.F_COND";
    this.F_DATA.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
    this.F_DATA.DataPropertyName = "F_DATA";
    componentResourceManager.ApplyResources((object) this.F_DATA, "F_DATA");
    this.F_DATA.Name = "F_DATA";
    this.F_DATA.Resizable = DataGridViewTriState.True;
    this.F_DATA.SortMode = DataGridViewColumnSortMode.NotSortable;
    this.AcceptButton = (IButtonControl) this._btnOK;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this._btnCancel;
    this.Controls.Add((Control) this._dgv);
    this.Controls.Add((Control) this._pnlBottom);
    this.DoubleBuffered = true;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (FilterForm);
    this.ShowInTaskbar = false;
    this._pnlBottom.ResumeLayout(false);
    this._ds.EndInit();
    this.conditions.EndInit();
    this.condsMap.EndInit();
    ((ISupportInitialize) this._dgv).EndInit();
    this.ResumeLayout(false);
  }
}
