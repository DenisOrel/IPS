// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.ConfigDimensionTypeForm
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using ImSSP;
using Intermech.Expressions;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.MaterialsHandbook;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class ConfigDimensionTypeForm : Form
{
  private ExpressionTree _expressionTree;
  private string _formulaText = string.Empty;
  private Dictionary<string, DataGridViewRow> _rows = new Dictionary<string, DataGridViewRow>();
  private DataRow _sourceRow;
  private FormulaRenderer _renderer = new FormulaRenderer();
  private IContainer components;
  private Panel _pnlBottom;
  private Button _btnOK;
  private Button _btnCancel;
  private Panel _pnlFormula;
  private DataGridView _dgv;
  private DataGridViewTextBoxColumn _colCaption;
  private DataGridViewComboBoxColumn _colValue;

  internal Dictionary<string, object> Values { get; private set; }

  internal string ClassAttrValue { get; set; }

  public ConfigDimensionTypeForm(
    List<string> attrGuids,
    Dictionary<string, AttributeTypeProperties> attrTypeProps,
    TreeListNode node,
    ExpressionTree expressionTree,
    bool isNew)
  {
    this.InitializeComponent();
    this.ClassAttrValue = string.Empty;
    DataTable table = node.Row.Table;
    if (attrGuids.Count > sc_14489.ssp_imbase_14490(2030682882))
    {
      object defValue = (object) null;
      if (isNew)
      {
        foreach (string attrGuid in attrGuids)
        {
          if (attrTypeProps.ContainsKey(attrGuid))
          {
            DataColumn column = table.Columns[attrGuid];
            AttributeTypeProperties attrTypeProp = attrTypeProps[attrGuid];
            long measureID = 0;
            if (attrTypeProp.FieldType == FieldTypes.ftMeasured && column.ExtendedProperties.Contains((object) "F_MEASURE"))
            {
              object extendedProperty = column.ExtendedProperties[(object) "F_MEASURE"];
              if (extendedProperty != null && extendedProperty != DBNull.Value)
                measureID = Convert.ToInt64(extendedProperty);
            }
            object extendedProperty1 = column.ExtendedProperties[(object) "F_FILTERED_POSSIBLE_VALUES"];
            DataGridViewComboBoxCell viewComboBoxCell = this.AddRow(attrGuid, attrTypeProp, measureID, out defValue, extendedProperty1);
            if (viewComboBoxCell != null && defValue != null)
              viewComboBoxCell.Value = defValue;
          }
        }
      }
      else
      {
        foreach (string attrGuid in attrGuids)
        {
          if (attrTypeProps.ContainsKey(attrGuid))
          {
            DataColumn column = table.Columns[attrGuid];
            AttributeTypeProperties attrTypeProp = attrTypeProps[attrGuid];
            DataGridViewComboBoxCell viewComboBoxCell;
            if (attrTypeProp.FieldType == FieldTypes.ftMeasured && column.ExtendedProperties.Contains((object) "F_MEASURE"))
            {
              object extendedProperty = column.ExtendedProperties[(object) "F_MEASURE"];
              viewComboBoxCell = extendedProperty == null || extendedProperty == DBNull.Value ? this.AddRow(attrGuid, attrTypeProp, 0L, out defValue) : this.AddRow(attrGuid, attrTypeProp, Convert.ToInt64(extendedProperty), out defValue);
            }
            else
            {
              object extendedProperty = column.ExtendedProperties[(object) "F_FILTERED_POSSIBLE_VALUES"];
              viewComboBoxCell = this.AddRow(attrGuid, attrTypeProp, 0L, out defValue, extendedProperty);
            }
            if (viewComboBoxCell != null)
            {
              object additionalValue = node.AdditionalValues[attrGuid];
              if (additionalValue != null && additionalValue != DBNull.Value)
              {
                if (viewComboBoxCell.DataSource is DataTable dataSource && dataSource.Columns.Contains("NUM_VALUE"))
                {
                  DataRow[] dataRowArray = dataSource.Select($"{"NUM_VALUE"}='{additionalValue}'");
                  if (dataRowArray.Length != 0)
                    viewComboBoxCell.Value = dataRowArray[0][viewComboBoxCell.ValueMember];
                }
                else
                  viewComboBoxCell.Value = node.AdditionalValues[attrGuid];
              }
            }
          }
        }
      }
    }
    this._sourceRow = node.Row;
    this._expressionTree = expressionTree;
    this.ParsedFormula(Guid.Empty.ToString(), (object) null);
    this._dgv.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.On_dgv_EditingControlShowing);
  }

  private void On_cmb_SelectedValueChanged(object sender, EventArgs e)
  {
    DataGridViewComboBoxEditingControl boxEditingControl = sender as DataGridViewComboBoxEditingControl;
    this.ParsedFormula(Convert.ToString(this._dgv.Rows[boxEditingControl.EditingControlRowIndex].Tag), boxEditingControl.SelectedValue);
  }

  private void On_dgv_EditingControlShowing(
    object sender,
    DataGridViewEditingControlShowingEventArgs e)
  {
    if (!(e.Control is ComboBox control))
      return;
    control.SelectedValueChanged -= new EventHandler(this.On_cmb_SelectedValueChanged);
    control.SelectedValueChanged += new EventHandler(this.On_cmb_SelectedValueChanged);
  }

  private void On_pnl_Paint(object sender, PaintEventArgs e)
  {
    this._renderer.SetData(this.ClassAttrValue, this._formulaText);
    this._renderer.Draw(e.Graphics, this._pnlFormula.Font, this._pnlFormula.ClientSize);
  }

  protected override void OnClosed(EventArgs e)
  {
    base.OnClosed(e);
    this.Values = new Dictionary<string, object>(this._dgv.Rows.Count);
    foreach (DataGridViewRow row in (IEnumerable) this._dgv.Rows)
    {
      if (row.Cells[1] is DataGridViewComboBoxCell cell && cell.DataSource is DataTable dataSource && dataSource.Columns.Contains("NUM_VALUE"))
      {
        DataRow[] dataRowArray = dataSource.Select($"{cell.ValueMember}='{cell.Value}'");
        if (dataRowArray.Length != 0)
        {
          this.Values.Add(row.Tag.ToString(), dataRowArray[0]["NUM_VALUE"] ?? (object) DBNull.Value);
          continue;
        }
      }
      this.Values.Add(row.Tag.ToString(), row.Cells[1].Value ?? (object) DBNull.Value);
    }
  }

  private DataGridViewComboBoxCell AddRow(
    string strGuid,
    AttributeTypeProperties props,
    long measureID,
    out object defValue,
    object extendedProps = null)
  {
    defValue = (object) null;
    IDBAttributeTypeInfo attributeType = ApplicationServices.Container.GetService<IClientMetadataCache>().GetAttributeType(props.AttributeGuid, false);
    if (attributeType == null)
      return (DataGridViewComboBoxCell) null;
    DataTable dataTable1 = measureID != 0L ? IMHHelper.GetPossibleValues(attributeType, measureID) : IMHHelper.GetPossibleValues(attributeType);
    if (dataTable1 == null)
      return (DataGridViewComboBoxCell) null;
    int index = this._dgv.Rows.Add(new object[1]
    {
      (object) props.Name
    });
    this._dgv.Rows[index].Tag = (object) strGuid;
    this._rows.Add(strGuid, this._dgv.Rows[index]);
    DataGridViewComboBoxCell cell = this._dgv.Rows[index].Cells[1] as DataGridViewComboBoxCell;
    if (extendedProps != null)
    {
      List<object> objectList = new List<object>((IEnumerable<object>) (extendedProps as object[]));
      DataTable dataTable2 = dataTable1.Clone();
      string empty = string.Empty;
      foreach (DataRow row1 in (InternalDataCollectionBase) dataTable1.Rows)
      {
        object obj = row1[attributeType.PossibleValueFieldName];
        if (string.IsNullOrEmpty(Convert.ToString(obj)) || objectList.Contains(obj))
        {
          DataRow row2 = dataTable2.NewRow();
          row2.ItemArray = row1.ItemArray;
          dataTable2.Rows.Add(row2);
        }
      }
      dataTable1 = dataTable2;
    }
    cell.DataSource = (object) dataTable1;
    cell.DisplayMember = "F_DESCRIPTION";
    cell.ValueMember = attributeType.PossibleValueFieldName;
    if (attributeType.DefaultValue != null && attributeType.DefaultValue != DBNull.Value)
      defValue = attributeType.DefaultValue;
    return cell;
  }

  private void ParsedFormula(string strGuid, object value)
  {
    if (this._sourceRow == null)
      return;
    VariableValuesCollection usedVariables = this._expressionTree.UsedVariables;
    if (usedVariables != null)
    {
      foreach (VariableValue variableValue in (ReadOnlyCollectionBase) usedVariables)
        variableValue.Value = !(variableValue.Name == strGuid) ? (!this._rows.ContainsKey(variableValue.Name) ? this._sourceRow[variableValue.Name] : this._rows[variableValue.Name].Cells[1].Value ?? (object) string.Empty) : (object) Convert.ToString(value);
      this._formulaText = this._expressionTree.Evaluate(usedVariables).ToString();
    }
    this._pnlFormula.Invalidate();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ConfigDimensionTypeForm));
    this._pnlBottom = new Panel();
    this._btnOK = new Button();
    this._btnCancel = new Button();
    this._pnlFormula = new Panel();
    this._dgv = new DataGridView();
    this._colCaption = new DataGridViewTextBoxColumn();
    this._colValue = new DataGridViewComboBoxColumn();
    this._pnlBottom.SuspendLayout();
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
    this._pnlFormula.BorderStyle = BorderStyle.FixedSingle;
    componentResourceManager.ApplyResources((object) this._pnlFormula, "_pnlFormula");
    this._pnlFormula.Name = "_pnlFormula";
    this._pnlFormula.Paint += new PaintEventHandler(this.On_pnl_Paint);
    this._dgv.AllowUserToAddRows = false;
    this._dgv.AllowUserToDeleteRows = false;
    this._dgv.AllowUserToOrderColumns = true;
    this._dgv.AllowUserToResizeRows = false;
    this._dgv.BorderStyle = BorderStyle.Fixed3D;
    this._dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    this._dgv.ColumnHeadersVisible = false;
    this._dgv.Columns.AddRange((DataGridViewColumn) this._colCaption, (DataGridViewColumn) this._colValue);
    componentResourceManager.ApplyResources((object) this._dgv, "_dgv");
    this._dgv.EditMode = DataGridViewEditMode.EditOnEnter;
    this._dgv.MultiSelect = false;
    this._dgv.Name = "_dgv";
    this._dgv.RowHeadersVisible = false;
    this._dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;
    componentResourceManager.ApplyResources((object) this._colCaption, "_colCaption");
    this._colCaption.Name = "_colCaption";
    this._colCaption.ReadOnly = true;
    this._colCaption.SortMode = DataGridViewColumnSortMode.NotSortable;
    this._colValue.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
    componentResourceManager.ApplyResources((object) this._colValue, "_colValue");
    this._colValue.Name = "_colValue";
    this._colValue.Resizable = DataGridViewTriState.True;
    this.AcceptButton = (IButtonControl) this._btnOK;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this._btnCancel;
    this.Controls.Add((Control) this._dgv);
    this.Controls.Add((Control) this._pnlFormula);
    this.Controls.Add((Control) this._pnlBottom);
    this.DoubleBuffered = true;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (ConfigDimensionTypeForm);
    this.ShowInTaskbar = false;
    this._pnlBottom.ResumeLayout(false);
    ((ISupportInitialize) this._dgv).EndInit();
    this.ResumeLayout(false);
  }
}
