// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.EditCoatingPropertiesForm
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Client.Core;
using Intermech.Imbase;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Imbase;
using Intermech.Interfaces.MaterialsHandbook;
using Intermech.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class EditCoatingPropertiesForm : Form
{
  private string _coatingKeyID = string.Empty;
  private string _coatingKeyGuid = string.Empty;
  private DataTable _dtCoatingsProps;
  private string _coatingsColumnGuid = string.Empty;
  private string _materialsColumnGuid = string.Empty;
  private string _purposeColumnGuid = string.Empty;
  private string _instructionsColumnGuid = string.Empty;
  private DataTable _dtParams;
  private DataRow _currentRow;
  private IContainer components;
  private Panel _pnlBottom;
  private Button _btnAdd;
  private Button _btnDel;
  private Button _btnOK;
  private Button _btnCancel;
  private ToolStrip toolStrip1;
  private ToolStripLabel _tsLbName;
  private ToolStripComboBox _tsComboMaterials;
  private Label _lb1;
  private TextBox _txtPurpose;
  private Label _lb2;
  private Label _lb3;
  private TextBox _txtInstructions;
  private DataGridView _dgw;
  private DataGridViewTextBoxColumn _colCondition;
  private DataGridViewTextBoxColumn _colCoating;
  private DataGridViewTextBoxColumn _colThickness;

  public EditCoatingPropertiesForm(
    string coatingKeyID,
    DataTable dtCoatingsProps,
    string coatingsColGuid,
    string materialsColGuid,
    string purposeColGuid,
    string instructionsColGuid,
    DataTable dtParams,
    Dictionary<string, string> materials)
  {
    this.InitializeComponent();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      bool isGuidKey = false;
      string str = ImbaseHelper.ConvertImbaseKey(sessionKeeper.Session, coatingKeyID, out isGuidKey);
      if (isGuidKey)
      {
        this._coatingKeyID = coatingKeyID;
        this._coatingKeyGuid = str;
      }
      else
      {
        this._coatingKeyID = str;
        this._coatingKeyGuid = coatingKeyID;
      }
    }
    this._dtCoatingsProps = dtCoatingsProps;
    this._coatingsColumnGuid = coatingsColGuid;
    this._materialsColumnGuid = materialsColGuid;
    this._purposeColumnGuid = purposeColGuid;
    this._instructionsColumnGuid = instructionsColGuid;
    this._dtParams = dtParams;
    for (int index = 0; index < 8; ++index)
      this._dgw.Rows[this._dgw.Rows.Add()].Cells[nameof (_colCondition)].Value = (object) (index + 1);
    this.LoadMaterialsData(materials);
    this._tsComboMaterials.SelectedIndex = 0;
  }

  private void On_btnAdd_Click(object sender, EventArgs e)
  {
    using (EditMaterialsTableForm materialsTableForm = new EditMaterialsTableForm())
    {
      if (materialsTableForm.ShowDialog() != DialogResult.OK)
        return;
      string selectedItemKey = materialsTableForm.SelectedItemKey;
      string selectedItemText = materialsTableForm.SelectedItemText;
      if (string.IsNullOrEmpty(selectedItemKey))
        return;
      string key2 = string.Empty;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
        key2 = ImbaseHelper.ConvertImbaseKey(sessionKeeper.Session, selectedItemKey);
      EditCoatingPropertiesForm.ComboItem comboItem = new EditCoatingPropertiesForm.ComboItem(selectedItemKey, key2, selectedItemText);
      if (!this._tsComboMaterials.Items.Contains((object) comboItem))
      {
        this.ClearAll();
        this._tsComboMaterials.Items.Add((object) comboItem);
        DataRow row = this._dtCoatingsProps.NewRow();
        row["F_GUID"] = (object) Guid.NewGuid();
        row[this._coatingsColumnGuid] = (object) this._coatingKeyGuid;
        row[this._materialsColumnGuid] = (object) selectedItemKey;
        this._dtCoatingsProps.Rows.Add(row);
      }
      this._tsComboMaterials.SelectedItem = (object) comboItem;
    }
  }

  private void On_btnDel_Click(object sender, EventArgs e)
  {
    if (this._tsComboMaterials.SelectedItem == null)
      return;
    string caption = LocalizationHolder.rm.GetString("IMH_DeleteData_Caption");
    if (MessageBox.Show(LocalizationHolder.rm.GetString("IMH_DeleteData_Msg"), caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
      return;
    this._tsComboMaterials.Items.Remove((object) (this._tsComboMaterials.SelectedItem as EditCoatingPropertiesForm.ComboItem));
    this._currentRow.Delete();
    this._currentRow = (DataRow) null;
    if (this._tsComboMaterials.Items.Count > 0)
      this._tsComboMaterials.SelectedIndex = 0;
    else
      this.ClearAll();
  }

  private void On_dgw_CellEndEdit(object sender, DataGridViewCellEventArgs e)
  {
    object tag = this._dgw.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag;
    if (tag == null || tag == DBNull.Value)
      return;
    this._currentRow[Convert.ToString(tag)] = this._dgw.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
  }

  private void On_tsComboMaterials_SelectedIndexChanged(object sender, EventArgs e)
  {
    this.ClearAll();
    if (this._tsComboMaterials.SelectedItem == null)
      return;
    EditCoatingPropertiesForm.ComboItem selectedItem = this._tsComboMaterials.SelectedItem as EditCoatingPropertiesForm.ComboItem;
    DataRow[] dataRowArray = this._dtCoatingsProps.Select(string.Format("([{0}]='{1}' OR [{0}]='{2}') AND ([{3}]='{4}' OR [{3}]='{5}')", (object) this._coatingsColumnGuid, (object) this._coatingKeyID, (object) this._coatingKeyGuid, (object) this._materialsColumnGuid, (object) selectedItem.Key1, (object) selectedItem.Key2));
    if (dataRowArray == null || dataRowArray.Length == 0)
      return;
    this._currentRow = dataRowArray[0];
    this._txtPurpose.Text = Convert.ToString(this._currentRow[this._purposeColumnGuid]);
    this._txtInstructions.Text = Convert.ToString(this._currentRow[this._instructionsColumnGuid]);
    int num = this._dtParams.Rows.Count <= 8 ? this._dtParams.Rows.Count : 8;
    string empty = string.Empty;
    for (int index = 0; index < num; ++index)
    {
      DataRow row = this._dtParams.Rows[index];
      string str1 = Convert.ToString(row[0]);
      if (this._dtCoatingsProps.Columns.Contains(str1))
      {
        this._dgw.Rows[index].Cells["_colCoating"].Value = this._currentRow[str1];
        this._dgw.Rows[index].Cells["_colCoating"].Tag = (object) str1;
      }
      string str2 = Convert.ToString(row[1]);
      if (this._dtCoatingsProps.Columns.Contains(str2))
      {
        this._dgw.Rows[index].Cells["_colThickness"].Value = this._currentRow[str2];
        this._dgw.Rows[index].Cells["_colThickness"].Tag = (object) str2;
      }
    }
  }

  private void On_txtInstructions_Leave(object sender, EventArgs e)
  {
    if (this._tsComboMaterials.SelectedItem == null)
      return;
    this._currentRow[this._instructionsColumnGuid] = (object) this._txtInstructions.Text;
  }

  private void On_txtPurpose_Leave(object sender, EventArgs e)
  {
    if (this._tsComboMaterials.SelectedItem == null)
      return;
    this._currentRow[this._purposeColumnGuid] = (object) this._txtPurpose.Text;
  }

  protected override void OnClosed(EventArgs e)
  {
    base.OnClosed(e);
    FormStorage.SaveLayout((Control) this);
  }

  protected override void OnClosing(CancelEventArgs e)
  {
    if (!this._dtCoatingsProps.DataSet.HasChanges())
      return;
    if (this.DialogResult != DialogResult.OK)
    {
      string caption = LocalizationHolder.rm.GetString("IMH_SaveChanges_Caption");
      switch (MessageBox.Show(LocalizationHolder.rm.GetString("IMH_SaveChangedData_Msg"), caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
      {
        case DialogResult.Cancel:
          e.Cancel = true;
          break;
        case DialogResult.Yes:
          this.Save();
          break;
        default:
          this._dtCoatingsProps.RejectChanges();
          break;
      }
    }
    else
      this.Save();
  }

  protected override void OnLoad(EventArgs e)
  {
    base.OnLoad(e);
    FormStorage.LoadLayout((Control) this);
  }

  private void ClearAll()
  {
    this._currentRow = (DataRow) null;
    this._txtInstructions.Text = string.Empty;
    this._txtPurpose.Text = string.Empty;
    foreach (DataGridViewRow row in (IEnumerable) this._dgw.Rows)
    {
      row.Cells["_colCoating"].Value = (object) string.Empty;
      row.Cells["_colThickness"].Value = (object) string.Empty;
    }
  }

  private void LoadMaterialsData(Dictionary<string, string> materialsData)
  {
    if (materialsData == null)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      string empty = string.Empty;
      foreach (KeyValuePair<string, string> keyValuePair in materialsData)
      {
        string key2 = ImbaseHelper.ConvertImbaseKey(sessionKeeper.Session, keyValuePair.Key);
        this._tsComboMaterials.Items.Add((object) new EditCoatingPropertiesForm.ComboItem(keyValuePair.Key, key2, keyValuePair.Value));
      }
    }
  }

  private void Save()
  {
    this._dtCoatingsProps.AcceptChanges();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      long num = 0;
      if (sessionKeeper.Session.GetCustomService(typeof (IIMHSystemSettingsService)) is IIMHSystemSettingsService customService)
      {
        Guid objectGuidByName = customService.GetObjectGuidByName("COATING_PROPERTIES_TABLE_NAME");
        num = IMHHelper.GetTableIDByTableRefID(sessionKeeper.Session.GetObjectInfo(objectGuidByName).ObjectID);
      }
      if (num == 0L)
        return;
      TableLoadHelper.StoreData(sessionKeeper.Session, num, this._dtCoatingsProps.DataSet, sessionKeeper.Session.GetCustomService(typeof (ITablesIndexer)) as ITablesIndexer);
      ServiceUtils.GetService<INotificationService>((object) ApplicationServices.Container, false)?.FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", num));
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (EditCoatingPropertiesForm));
    this._pnlBottom = new Panel();
    this._btnAdd = new Button();
    this._btnDel = new Button();
    this._btnOK = new Button();
    this._btnCancel = new Button();
    this.toolStrip1 = new ToolStrip();
    this._tsLbName = new ToolStripLabel();
    this._tsComboMaterials = new ToolStripComboBox();
    this._lb1 = new Label();
    this._txtPurpose = new TextBox();
    this._lb2 = new Label();
    this._lb3 = new Label();
    this._txtInstructions = new TextBox();
    this._dgw = new DataGridView();
    this._colCondition = new DataGridViewTextBoxColumn();
    this._colCoating = new DataGridViewTextBoxColumn();
    this._colThickness = new DataGridViewTextBoxColumn();
    this._pnlBottom.SuspendLayout();
    this.toolStrip1.SuspendLayout();
    ((ISupportInitialize) this._dgw).BeginInit();
    this.SuspendLayout();
    this._pnlBottom.Controls.Add((Control) this._btnAdd);
    this._pnlBottom.Controls.Add((Control) this._btnDel);
    this._pnlBottom.Controls.Add((Control) this._btnOK);
    this._pnlBottom.Controls.Add((Control) this._btnCancel);
    componentResourceManager.ApplyResources((object) this._pnlBottom, "_pnlBottom");
    this._pnlBottom.Name = "_pnlBottom";
    componentResourceManager.ApplyResources((object) this._btnAdd, "_btnAdd");
    this._btnAdd.Name = "_btnAdd";
    this._btnAdd.UseVisualStyleBackColor = true;
    this._btnAdd.Click += new EventHandler(this.On_btnAdd_Click);
    componentResourceManager.ApplyResources((object) this._btnDel, "_btnDel");
    this._btnDel.Name = "_btnDel";
    this._btnDel.UseVisualStyleBackColor = true;
    this._btnDel.Click += new EventHandler(this.On_btnDel_Click);
    componentResourceManager.ApplyResources((object) this._btnOK, "_btnOK");
    this._btnOK.DialogResult = DialogResult.OK;
    this._btnOK.Name = "_btnOK";
    this._btnOK.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this._btnCancel, "_btnCancel");
    this._btnCancel.DialogResult = DialogResult.Cancel;
    this._btnCancel.Name = "_btnCancel";
    this._btnCancel.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.toolStrip1, "toolStrip1");
    this.toolStrip1.Items.AddRange(new ToolStripItem[2]
    {
      (ToolStripItem) this._tsLbName,
      (ToolStripItem) this._tsComboMaterials
    });
    this.toolStrip1.Name = "toolStrip1";
    componentResourceManager.ApplyResources((object) this._tsLbName, "_tsLbName");
    this._tsLbName.Name = "_tsLbName";
    this._tsComboMaterials.DropDownStyle = ComboBoxStyle.DropDownList;
    componentResourceManager.ApplyResources((object) this._tsComboMaterials, "_tsComboMaterials");
    this._tsComboMaterials.Name = "_tsComboMaterials";
    this._tsComboMaterials.SelectedIndexChanged += new EventHandler(this.On_tsComboMaterials_SelectedIndexChanged);
    componentResourceManager.ApplyResources((object) this._lb1, "_lb1");
    this._lb1.Name = "_lb1";
    this._txtPurpose.AcceptsReturn = true;
    componentResourceManager.ApplyResources((object) this._txtPurpose, "_txtPurpose");
    this._txtPurpose.HideSelection = false;
    this._txtPurpose.Name = "_txtPurpose";
    this._txtPurpose.Leave += new EventHandler(this.On_txtPurpose_Leave);
    componentResourceManager.ApplyResources((object) this._lb2, "_lb2");
    this._lb2.Name = "_lb2";
    componentResourceManager.ApplyResources((object) this._lb3, "_lb3");
    this._lb3.Name = "_lb3";
    componentResourceManager.ApplyResources((object) this._txtInstructions, "_txtInstructions");
    this._txtInstructions.HideSelection = false;
    this._txtInstructions.Name = "_txtInstructions";
    this._txtInstructions.Leave += new EventHandler(this.On_txtInstructions_Leave);
    this._dgw.AllowUserToAddRows = false;
    this._dgw.AllowUserToDeleteRows = false;
    this._dgw.AllowUserToOrderColumns = true;
    this._dgw.AllowUserToResizeRows = false;
    componentResourceManager.ApplyResources((object) this._dgw, "_dgw");
    this._dgw.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    this._dgw.Columns.AddRange((DataGridViewColumn) this._colCondition, (DataGridViewColumn) this._colCoating, (DataGridViewColumn) this._colThickness);
    this._dgw.MultiSelect = false;
    this._dgw.Name = "_dgw";
    this._dgw.RowHeadersVisible = false;
    this._dgw.SelectionMode = DataGridViewSelectionMode.CellSelect;
    this._dgw.CellEndEdit += new DataGridViewCellEventHandler(this.On_dgw_CellEndEdit);
    componentResourceManager.ApplyResources((object) this._colCondition, "_colCondition");
    this._colCondition.Name = "_colCondition";
    this._colCondition.ReadOnly = true;
    componentResourceManager.ApplyResources((object) this._colCoating, "_colCoating");
    this._colCoating.Name = "_colCoating";
    componentResourceManager.ApplyResources((object) this._colThickness, "_colThickness");
    this._colThickness.Name = "_colThickness";
    this.AcceptButton = (IButtonControl) this._btnOK;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this._btnCancel;
    this.Controls.Add((Control) this._dgw);
    this.Controls.Add((Control) this._txtInstructions);
    this.Controls.Add((Control) this._lb3);
    this.Controls.Add((Control) this._lb2);
    this.Controls.Add((Control) this._txtPurpose);
    this.Controls.Add((Control) this._lb1);
    this.Controls.Add((Control) this.toolStrip1);
    this.Controls.Add((Control) this._pnlBottom);
    this.DoubleBuffered = true;
    this.FormBorderStyle = FormBorderStyle.FixedSingle;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (EditCoatingPropertiesForm);
    this.ShowInTaskbar = false;
    this._pnlBottom.ResumeLayout(false);
    this.toolStrip1.ResumeLayout(false);
    this.toolStrip1.PerformLayout();
    ((ISupportInitialize) this._dgw).EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  private class ComboItem
  {
    internal string Key1 = string.Empty;
    internal string Key2 = string.Empty;
    internal string Text = string.Empty;

    internal ComboItem(string key1, string key2, string text)
    {
      this.Key1 = key1;
      this.Key2 = key2;
      this.Text = text;
    }

    public override bool Equals(object obj)
    {
      bool flag = false;
      if (obj != null && obj.GetType() == this.GetType())
      {
        EditCoatingPropertiesForm.ComboItem comboItem = obj as EditCoatingPropertiesForm.ComboItem;
        flag = obj == this || this.Key1 == comboItem.Key1 && this.Key2 == comboItem.Key2 || this.Key1 == comboItem.Key2 && this.Key2 == comboItem.Key1;
      }
      return flag;
    }

    public override int GetHashCode() => this.Text.GetHashCode();

    public override string ToString() => this.Text;
  }
}
