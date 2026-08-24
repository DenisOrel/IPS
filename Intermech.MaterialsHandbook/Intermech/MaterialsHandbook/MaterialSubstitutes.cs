// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.MaterialSubstitutes
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class MaterialSubstitutes : Form
{
  private string _baseIDKey = string.Empty;
  private string _baseGuidKey = string.Empty;
  private string _colMaterialGuid = string.Empty;
  private string _colNameSubstitute = string.Empty;
  private List<string> _legacyKeys = new List<string>();
  private List<string> _addedKeys = new List<string>();
  private List<string> _removedKeys = new List<string>();
  private IContainer components;
  private Panel _pnlBottom;
  private Button _btnCancel;
  private Label _lbMaterialCaption;
  private TextBox _txtMaterialCaption;
  private Label _lbSubstitutes;
  private ListBox _lbMaterials;
  private Button _btnApply;
  private Button _btnAdd;
  private Button _btnDel;
  private Button _btnClose;
  private Button _btnGoTo;

  public long RecID { get; private set; }

  public long TableRefID { get; private set; }

  public MaterialSubstitutes(string imbaseKey, string caption)
  {
    this.InitializeComponent();
    this.RecID = -1L;
    this.TableRefID = 0L;
    this._txtMaterialCaption.Text = caption;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      this.LoadBaseKeys(sessionKeeper.Session, imbaseKey);
      this.LoadColumnNames(sessionKeeper.Session);
    }
    this.LoadSubstitutes();
  }

  private void On_btnAdd_Click(object sender, EventArgs e)
  {
    IIMHSelector service = ServiceUtils.GetService<IIMHSelector>((object) ApplicationServices.Container, false);
    if (service != null)
      this.CreateItems(service.SelectMaterial(false, true), true);
    this.CheckEnableButtons();
  }

  private void On_btnDel_Click(object sender, EventArgs e)
  {
    if (this._lbMaterials.SelectedItem != null)
    {
      int selectedIndex = this._lbMaterials.SelectedIndex;
      if (this._lbMaterials.SelectedItem is FavouriteData selectedItem)
      {
        if (this._addedKeys.Contains(selectedItem.ImbaseKey))
          this._addedKeys.Remove(selectedItem.ImbaseKey);
        else if (this._legacyKeys.Contains(selectedItem.ImbaseKey))
        {
          this._legacyKeys.Remove(selectedItem.ImbaseKey);
          if (!this._removedKeys.Contains(selectedItem.ImbaseKey))
            this._removedKeys.Add(selectedItem.ImbaseKey);
        }
      }
      this._lbMaterials.Items.Remove(this._lbMaterials.SelectedItem);
      if (this._lbMaterials.Items.Count > selectedIndex)
        this._lbMaterials.SelectedIndex = selectedIndex;
      else if (this._lbMaterials.Items.Count > 0)
        this._lbMaterials.SelectedIndex = this._lbMaterials.Items.Count - 1;
    }
    this.CheckEnableButtons();
  }

  private void On_btnGoTo_Click(object sender, EventArgs e)
  {
    if (this._lbMaterials.SelectedItems.Count > 0)
    {
      FavouriteData selectedItem = this._lbMaterials.SelectedItem as FavouriteData;
      this.TableRefID = selectedItem.TableRefID;
      this.RecID = selectedItem.RecordID;
    }
    this.DialogResult = DialogResult.OK;
    this.Close();
  }

  private void On_btnApply_Click(object sender, EventArgs e)
  {
    this.Save();
    this.CheckEnableButtons();
  }

  private void On_btnCancel_Click(object sender, EventArgs e)
  {
    this.Cancel();
    this.CheckEnableButtons();
  }

  private void On_lbMaterials_SelectedIndexChanged(object sender, EventArgs e)
  {
    this.CheckEnableButtons();
  }

  private void LoadBaseKeys(IUserSession session, string imbaseKey)
  {
    bool isGuidKey = false;
    string str = ImbaseHelper.ConvertImbaseKey(session, imbaseKey, out isGuidKey);
    if (isGuidKey)
    {
      this._baseGuidKey = str;
      this._baseIDKey = imbaseKey;
    }
    else
    {
      this._baseGuidKey = imbaseKey;
      this._baseIDKey = str;
    }
  }

  private void LoadColumnNames(IUserSession session)
  {
    if (!(session.GetCustomService(typeof (IIMHSystemSettingsService)) is IIMHSystemSettingsService customService))
      return;
    List<string> names = new List<string>()
    {
      "MATERIAL_SUBSTITUTES_COLUMN_MATERIAL",
      "MATERIAL_SUBSTITUTES_COLUMN_SUBSTITUTES"
    };
    Dictionary<string, Guid> objectGuidsByNames = customService.GetObjectGuidsByNames(names);
    if (objectGuidsByNames == null)
      return;
    if (objectGuidsByNames.ContainsKey("MATERIAL_SUBSTITUTES_COLUMN_MATERIAL"))
      this._colMaterialGuid = Convert.ToString((object) objectGuidsByNames["MATERIAL_SUBSTITUTES_COLUMN_MATERIAL"]);
    if (!objectGuidsByNames.ContainsKey("MATERIAL_SUBSTITUTES_COLUMN_SUBSTITUTES"))
      return;
    this._colNameSubstitute = Convert.ToString((object) objectGuidsByNames["MATERIAL_SUBSTITUTES_COLUMN_SUBSTITUTES"]);
  }

  private void LoadSubstitutes()
  {
    DataTable table = this.GetTable();
    if (table == null)
      return;
    List<DataRow> list = table.AsEnumerable().Where<DataRow>((System.Func<DataRow, bool>) (x => Convert.ToString(x[this._colMaterialGuid]) == this._baseIDKey || Convert.ToString(x[this._colMaterialGuid]) == this._baseGuidKey)).ToList<DataRow>();
    if (list.Count <= 0)
      return;
    List<string> keys = new List<string>(list.Count);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      foreach (DataRow dataRow in list)
      {
        string key1 = Convert.ToString(dataRow[this._colNameSubstitute]);
        if (!string.IsNullOrEmpty(key1))
        {
          string key2 = this.GetKey(sessionKeeper.Session, key1);
          if (!keys.Contains(key2))
            keys.Add(key2);
        }
      }
    }
    this.CreateItems(keys, false);
  }

  private FavouriteData CreateItem(string imbaseKey, string caption)
  {
    FavouriteData favouriteData = (FavouriteData) null;
    long linkId = 0;
    long recordId = -1;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (ImbaseHelper.TryParseRecordReference(sessionKeeper.Session, imbaseKey, out linkId, out recordId))
      {
        if (linkId != 0L)
        {
          if (recordId != -1L)
            favouriteData = new FavouriteData(linkId, recordId, caption, imbaseKey);
        }
      }
    }
    return favouriteData;
  }

  private void CreateItems(List<string> keys, bool newItems)
  {
    if (keys == null || keys.Count <= 0)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetCustomService(typeof (IImbaseServer)) is IImbaseServer customService))
        return;
      Dictionary<string, string> dictionary = customService.NameRecordReferences(sessionKeeper.Session.SessionGUID, keys);
      if (dictionary == null)
        return;
      int num = -1;
      this._lbMaterials.BeginUpdate();
      try
      {
        foreach (KeyValuePair<string, string> keyValuePair in dictionary)
        {
          if (!this._legacyKeys.Contains(keyValuePair.Key) && !this._addedKeys.Contains(keyValuePair.Key))
          {
            FavouriteData favouriteData = this.CreateItem(keyValuePair.Key, keyValuePair.Value);
            if (favouriteData != null)
            {
              num = this._lbMaterials.Items.Add((object) favouriteData);
              if (newItems)
              {
                if (this._removedKeys.Contains(keyValuePair.Key))
                  this._removedKeys.Remove(keyValuePair.Key);
                else
                  this._addedKeys.Add(keyValuePair.Key);
              }
              else
                this._legacyKeys.Add(keyValuePair.Key);
            }
          }
        }
        if (num <= -1 || this._lbMaterials.Items.Count <= num)
          return;
        this._lbMaterials.SelectedIndex = num;
      }
      finally
      {
        this._lbMaterials.EndUpdate();
      }
    }
  }

  private void CheckEnableButtons()
  {
    this._btnGoTo.Enabled = this._lbMaterials.SelectedItems.Count > 0;
    this._btnApply.Enabled = this._btnCancel.Enabled = this._addedKeys.Count > 0 || this._removedKeys.Count > 0;
  }

  private DataTable GetTable()
  {
    DataTable table1 = (DataTable) null;
    DataSet imbaseDs = IMHHelper.GetImbaseDS("MATERIAL_SUBSTITUTES_TABLE_NAME");
    if (imbaseDs != null && imbaseDs.Tables.Contains("IMS_DATA"))
    {
      DataTable table2 = imbaseDs.Tables["IMS_DATA"];
      table1 = !table2.Columns.Contains(this._colMaterialGuid) || !table2.Columns.Contains(this._colNameSubstitute) ? (DataTable) null : table2;
    }
    return table1;
  }

  private string GetKey(IUserSession session, string key)
  {
    bool isGuidKey = false;
    string str = ImbaseHelper.ConvertImbaseKey(session, key, out isGuidKey);
    return !isGuidKey ? str : key;
  }

  private void Save()
  {
    DataTable dt = this.GetTable();
    if (dt == null)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      foreach (string addedKey in this._addedKeys)
      {
        string key = addedKey;
        key2 = ImbaseHelper.ConvertImbaseKey(sessionKeeper.Session, key);
        if (dt.AsEnumerable().FirstOrDefault<DataRow>((System.Func<DataRow, bool>) (x =>
        {
          if (!(Convert.ToString(x[this._colMaterialGuid]) == this._baseIDKey) && !(Convert.ToString(x[this._colMaterialGuid]) == this._baseGuidKey))
            return false;
          return Convert.ToString(x[this._colNameSubstitute]) == key || Convert.ToString(x[this._colNameSubstitute]) == key2;
        })) == null)
        {
          DataRow row = dt.NewRow();
          row["F_GUID"] = (object) Guid.NewGuid();
          row[this._colMaterialGuid] = (object) this._baseGuidKey;
          row[this._colNameSubstitute] = (object) key2;
          dt.Rows.Add(row);
        }
      }
      foreach (string removedKey in this._removedKeys)
      {
        string key = removedKey;
        key2 = ImbaseHelper.ConvertImbaseKey(sessionKeeper.Session, key);
        List<DataRow> list = dt.AsEnumerable().Where<DataRow>((System.Func<DataRow, bool>) (x =>
        {
          if (!(Convert.ToString(x[this._colMaterialGuid]) == this._baseIDKey) && !(Convert.ToString(x[this._colMaterialGuid]) == this._baseGuidKey))
            return false;
          return Convert.ToString(x[this._colNameSubstitute]) == key || Convert.ToString(x[this._colNameSubstitute]) == key2;
        })).ToList<DataRow>();
        if (list.Count != 0)
          list.ForEach((Action<DataRow>) (x => dt.Rows.Remove(x)));
      }
      dt.AcceptChanges();
      long tableIdByTableRefId = IMHHelper.GetTableIDByTableRefID(IMHHelper.GetObjectIDByConstName("MATERIAL_SUBSTITUTES_TABLE_NAME"));
      TableLoadHelper.StoreData(sessionKeeper.Session, tableIdByTableRefId, dt.DataSet, sessionKeeper.Session.GetCustomService(typeof (ITablesIndexer)) as ITablesIndexer);
      ServiceUtils.GetService<INotificationService>((object) ApplicationServices.Container, false)?.FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", tableIdByTableRefId));
      this._legacyKeys = this._legacyKeys.Union<string>((IEnumerable<string>) this._addedKeys).Except<string>((IEnumerable<string>) this._removedKeys).ToList<string>();
      this._addedKeys.Clear();
      this._removedKeys.Clear();
    }
  }

  private void Cancel()
  {
    this._legacyKeys.Clear();
    this._addedKeys.Clear();
    this._removedKeys.Clear();
    this._lbMaterials.BeginUpdate();
    this._lbMaterials.Items.Clear();
    this._lbMaterials.EndUpdate();
    this.LoadSubstitutes();
    this.CheckEnableButtons();
  }

  protected override void OnLoad(EventArgs e)
  {
    base.OnLoad(e);
    FormStorage.LoadLayout((Control) this);
    string text = string.Empty;
    string str = Guid.Empty.ToString();
    if (string.IsNullOrEmpty(this._colMaterialGuid) || this._colMaterialGuid == str)
      text = LocalizationHolder.rm.GetString("IMH_SystemSettings_MaterialColumn_Empty");
    if (string.IsNullOrEmpty(this._colNameSubstitute) || this._colNameSubstitute == str)
      text = $"{text}{(string.IsNullOrEmpty(text) ? (object) string.Empty : (object) "\n ")}{LocalizationHolder.rm.GetString("IMH_SystemSettings_SubstitutesColumn_Empty")}";
    if (string.IsNullOrEmpty(text))
      return;
    string caption = LocalizationHolder.rm.GetString("IMH_Substitutes_Settings");
    int num = (int) MessageBox.Show((IWin32Window) this, text, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    this._btnAdd.Enabled = this._btnDel.Enabled = false;
  }

  protected override void OnClosing(CancelEventArgs e)
  {
    base.OnClosing(e);
    if (this._addedKeys.Count <= 0 && this._removedKeys.Count <= 0)
      return;
    string caption = LocalizationHolder.rm.GetString("IMH_MaterialSubstitutes");
    switch (MessageBox.Show(LocalizationHolder.rm.GetString("IMH_MaterialSubstitutes_SaveDialog_Msg"), caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
    {
      case DialogResult.Cancel:
        e.Cancel = true;
        break;
      case DialogResult.Yes:
        this.Save();
        break;
    }
  }

  protected override void OnClosed(EventArgs e)
  {
    base.OnClosed(e);
    FormStorage.SaveLayout((Control) this);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MaterialSubstitutes));
    this._pnlBottom = new Panel();
    this._btnClose = new Button();
    this._btnGoTo = new Button();
    this._btnAdd = new Button();
    this._btnDel = new Button();
    this._btnApply = new Button();
    this._btnCancel = new Button();
    this._lbMaterialCaption = new Label();
    this._txtMaterialCaption = new TextBox();
    this._lbSubstitutes = new Label();
    this._lbMaterials = new ListBox();
    this._pnlBottom.SuspendLayout();
    this.SuspendLayout();
    this._pnlBottom.Controls.Add((Control) this._btnClose);
    this._pnlBottom.Controls.Add((Control) this._btnGoTo);
    this._pnlBottom.Controls.Add((Control) this._btnAdd);
    this._pnlBottom.Controls.Add((Control) this._btnDel);
    this._pnlBottom.Controls.Add((Control) this._btnApply);
    this._pnlBottom.Controls.Add((Control) this._btnCancel);
    componentResourceManager.ApplyResources((object) this._pnlBottom, "_pnlBottom");
    this._pnlBottom.Name = "_pnlBottom";
    componentResourceManager.ApplyResources((object) this._btnClose, "_btnClose");
    this._btnClose.DialogResult = DialogResult.Cancel;
    this._btnClose.Name = "_btnClose";
    this._btnClose.UseVisualStyleBackColor = true;
    this._btnGoTo.DialogResult = DialogResult.OK;
    componentResourceManager.ApplyResources((object) this._btnGoTo, "_btnGoTo");
    this._btnGoTo.Name = "_btnGoTo";
    this._btnGoTo.UseVisualStyleBackColor = true;
    this._btnGoTo.Click += new EventHandler(this.On_btnGoTo_Click);
    componentResourceManager.ApplyResources((object) this._btnAdd, "_btnAdd");
    this._btnAdd.Name = "_btnAdd";
    this._btnAdd.UseVisualStyleBackColor = true;
    this._btnAdd.Click += new EventHandler(this.On_btnAdd_Click);
    componentResourceManager.ApplyResources((object) this._btnDel, "_btnDel");
    this._btnDel.Name = "_btnDel";
    this._btnDel.UseVisualStyleBackColor = true;
    this._btnDel.Click += new EventHandler(this.On_btnDel_Click);
    componentResourceManager.ApplyResources((object) this._btnApply, "_btnApply");
    this._btnApply.Name = "_btnApply";
    this._btnApply.UseVisualStyleBackColor = true;
    this._btnApply.Click += new EventHandler(this.On_btnApply_Click);
    componentResourceManager.ApplyResources((object) this._btnCancel, "_btnCancel");
    this._btnCancel.Name = "_btnCancel";
    this._btnCancel.UseVisualStyleBackColor = true;
    this._btnCancel.Click += new EventHandler(this.On_btnCancel_Click);
    componentResourceManager.ApplyResources((object) this._lbMaterialCaption, "_lbMaterialCaption");
    this._lbMaterialCaption.Name = "_lbMaterialCaption";
    componentResourceManager.ApplyResources((object) this._txtMaterialCaption, "_txtMaterialCaption");
    this._txtMaterialCaption.Name = "_txtMaterialCaption";
    this._txtMaterialCaption.ReadOnly = true;
    componentResourceManager.ApplyResources((object) this._lbSubstitutes, "_lbSubstitutes");
    this._lbSubstitutes.Name = "_lbSubstitutes";
    componentResourceManager.ApplyResources((object) this._lbMaterials, "_lbMaterials");
    this._lbMaterials.FormattingEnabled = true;
    this._lbMaterials.Name = "_lbMaterials";
    this._lbMaterials.Sorted = true;
    this._lbMaterials.SelectedIndexChanged += new EventHandler(this.On_lbMaterials_SelectedIndexChanged);
    this._lbMaterials.DoubleClick += new EventHandler(this.On_btnGoTo_Click);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this._btnClose;
    this.Controls.Add((Control) this._lbMaterials);
    this.Controls.Add((Control) this._lbSubstitutes);
    this.Controls.Add((Control) this._txtMaterialCaption);
    this.Controls.Add((Control) this._lbMaterialCaption);
    this.Controls.Add((Control) this._pnlBottom);
    this.DoubleBuffered = true;
    this.Name = nameof (MaterialSubstitutes);
    this.ShowInTaskbar = false;
    this._pnlBottom.ResumeLayout(false);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
