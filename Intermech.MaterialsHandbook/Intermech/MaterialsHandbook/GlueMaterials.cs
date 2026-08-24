// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.GlueMaterials
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
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class GlueMaterials : Form
{
  private string _glueKeyID = string.Empty;
  private string _glueKeyGuid = string.Empty;
  private DataTable _dt;
  private string _materialColName_1 = string.Empty;
  private string _materialColName_2 = string.Empty;
  private string _glueColName = string.Empty;
  private Dictionary<string, string> _materials;
  private List<GlueMaterials.GlueItem> _lbItems = new List<GlueMaterials.GlueItem>();
  private IContainer components;
  private Panel _pnlBottom;
  private Button _btnAdd;
  private Button _btnDel;
  private Button _btnOK;
  private Button _btnCancel;
  private SplitContainer splitContainer1;
  private TableLayoutPanel _tlp;
  private ListView _lvMaterial1;
  private System.Windows.Forms.ColumnHeader _colName1;
  private ListView _lvMaterial2;
  private System.Windows.Forms.ColumnHeader _colName2;
  private ListBox _lb;

  private bool IsDataLoad
  {
    get
    {
      return this._dt != null && !string.IsNullOrEmpty(this._materialColName_1) && this._dt.Columns.Contains(this._materialColName_1) && !string.IsNullOrEmpty(this._materialColName_2) && this._dt.Columns.Contains(this._materialColName_2) && !string.IsNullOrEmpty(this._glueColName) && this._dt.Columns.Contains(this._glueColName);
    }
  }

  public GlueMaterials(string glueKey, string glueCaption)
  {
    this.InitializeComponent();
    this._glueKeyID = glueKey;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this._glueKeyGuid = ImbaseHelper.ConvertImbaseKey(sessionKeeper.Session, this._glueKeyID);
    this.Text = glueCaption;
  }

  private void On_btnAdd_Click(object sender, EventArgs e)
  {
    if (this._lvMaterial1.SelectedItems.Count <= 0 || this._lvMaterial2.SelectedItems.Count <= 0)
      return;
    this.AddItem(this._lvMaterial1.SelectedItems[0].Name, this._lvMaterial2.SelectedItems[0].Name, (DataRow) null);
    this._btnDel.Enabled = true;
  }

  private void On_btnDel_Click(object sender, EventArgs e)
  {
    object selectedItem = this._lb.SelectedItem;
    if (selectedItem == null)
      return;
    int selectedIndex = this._lb.SelectedIndex;
    this._lb.Items.Remove(selectedItem);
    (selectedItem as GlueMaterials.GlueItem).Visible = false;
    if (this._lb.Items.Count > 0)
      this._lb.SelectedIndex = selectedIndex >= this._lb.Items.Count ? this._lb.Items.Count - 1 : selectedIndex;
    else
      this._btnDel.Enabled = false;
  }

  protected override void OnClosed(EventArgs e)
  {
    base.OnClosed(e);
    FormStorage.SaveLayout((Control) this);
  }

  protected override void OnClosing(CancelEventArgs e)
  {
    base.OnClosing(e);
    bool flag = false;
    foreach (GlueMaterials.GlueItem lbItem in this._lbItems)
    {
      if (lbItem.Row == null || !lbItem.Visible)
      {
        flag = true;
        break;
      }
    }
    if (!flag)
      return;
    string caption = LocalizationHolder.rm.GetString("IMH_SaveChanges_Caption");
    switch (MessageBox.Show(LocalizationHolder.rm.GetString("IMH_SaveChangedData_Msg"), caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
    {
      case DialogResult.Cancel:
        e.Cancel = true;
        break;
      case DialogResult.Yes:
        if (!this.IsDataLoad)
          break;
        foreach (GlueMaterials.GlueItem lbItem in this._lbItems)
        {
          if (lbItem.Row == null)
          {
            if (lbItem.Visible)
            {
              DataRow row = this._dt.NewRow();
              row["F_GUID"] = (object) Guid.NewGuid();
              row[this._glueColName] = (object) this._glueKeyGuid;
              row[this._materialColName_1] = (object) lbItem.Key1;
              row[this._materialColName_2] = (object) lbItem.Key2;
              this._dt.Rows.Add(row);
            }
          }
          else if (!lbItem.Visible)
            this._dt.Rows.Remove(lbItem.Row);
        }
        this._dt.AcceptChanges();
        long tableIdByTableRefId = IMHHelper.GetTableIDByTableRefID(IMHHelper.GetObjectIDByConstName("GLUE_TABLE_NAME"));
        using (SessionKeeper sessionKeeper = new SessionKeeper())
          TableLoadHelper.StoreData(sessionKeeper.Session, tableIdByTableRefId, this._dt.DataSet, sessionKeeper.Session.GetCustomService(typeof (ITablesIndexer)) as ITablesIndexer);
        ServiceUtils.GetService<INotificationService>((object) ApplicationServices.Container, false)?.FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", tableIdByTableRefId));
        break;
    }
  }

  protected override void OnLoad(EventArgs e)
  {
    base.OnLoad(e);
    FormStorage.LoadLayout((Control) this);
    if (!this.IsDataLoad)
      return;
    DataRow[] dataRowArray = this._dt.Select(string.Format("[{0}]='{1}' OR [{0}]='{2}'", (object) this._glueColName, (object) this._glueKeyID, (object) this._glueKeyGuid));
    if (dataRowArray.Length == 0)
      return;
    foreach (DataRow row in dataRowArray)
    {
      string materialKeyGuid1 = this.GetMaterialKeyGuid(row[this._materialColName_1]);
      if (!string.IsNullOrEmpty(materialKeyGuid1))
      {
        string materialKeyGuid2 = this.GetMaterialKeyGuid(row[this._materialColName_2]);
        if (!string.IsNullOrEmpty(materialKeyGuid2))
          this.AddItem(materialKeyGuid1, materialKeyGuid2, row);
      }
    }
  }

  private void AddItem(string key1, string key2, DataRow row)
  {
    if (!this._materials.ContainsKey(key1) || !this._materials.ContainsKey(key2))
      return;
    string material1 = this._materials[key1];
    string material2 = this._materials[key2];
    GlueMaterials.GlueItem selectedItem = (GlueMaterials.GlueItem) null;
    if (!this.lbContainsPairMaterials(key1, key2, out selectedItem))
    {
      selectedItem = new GlueMaterials.GlueItem(key1, material1, key2, material2, row);
      this._lbItems.Add(selectedItem);
      this._lb.Items.Add((object) selectedItem);
    }
    else if (!selectedItem.Visible)
    {
      this._lb.Items.Add((object) selectedItem);
      selectedItem.Visible = true;
    }
    this._lb.SelectedItem = (object) selectedItem;
  }

  private bool lbContainsPairMaterials(
    string key1,
    string key2,
    out GlueMaterials.GlueItem selectedItem)
  {
    bool flag = false;
    selectedItem = (GlueMaterials.GlueItem) null;
    foreach (GlueMaterials.GlueItem lbItem in this._lbItems)
    {
      if (lbItem.IsEqual(key1, key2))
      {
        selectedItem = lbItem;
        flag = true;
        break;
      }
    }
    return flag;
  }

  private string GetMaterialKeyGuid(object objKey)
  {
    string materialKeyGuid = string.Empty;
    string str1 = Convert.ToString(objKey);
    if (str1.StartsWith("IK", StringComparison.InvariantCultureIgnoreCase))
    {
      int num = str1.IndexOf('.');
      if (num > 3)
      {
        string str2 = str1.Substring(2, num - 2);
        if (GuidHelper.IsGuid(str2))
        {
          materialKeyGuid = str1;
        }
        else
        {
          long result = 0;
          if (long.TryParse(str2, out result))
          {
            using (SessionKeeper sessionKeeper = new SessionKeeper())
            {
              QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(result);
              if (!objectInfo.Empty)
                materialKeyGuid = str1.Replace(str2, objectInfo.VersionGuid.ToString());
            }
          }
        }
      }
    }
    return materialKeyGuid;
  }

  public void GluesData(
    DataTable dt,
    string materialColName_1,
    string materialColName_2,
    string glueColName)
  {
    this._dt = dt;
    this._materialColName_1 = materialColName_1;
    this._materialColName_2 = materialColName_2;
    this._glueColName = glueColName;
  }

  public void MaterialsData(long tableRefID, DataTable dt, string colName)
  {
    if (dt != null && dt.Columns.Contains(colName))
    {
      this._lvMaterial1.SuspendLayout();
      this._lvMaterial2.SuspendLayout();
      try
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          this._materials = new Dictionary<string, string>(dt.Rows.Count);
          string empty1 = string.Empty;
          string empty2 = string.Empty;
          string empty3 = string.Empty;
          foreach (DataRow row in (InternalDataCollectionBase) dt.Rows)
          {
            object obj = row[colName];
            if (obj != null && obj != DBNull.Value)
            {
              string text = obj.ToString();
              string keyValue = ImbaseHelper.MakeInternalImbaseKey(tableRefID, Convert.ToInt64(row["F_KEY"]));
              string key = ImbaseHelper.ConvertImbaseKey(sessionKeeper.Session, keyValue);
              this._materials.Add(key, text);
              ListViewItem listViewItem1 = new ListViewItem(text);
              ListViewItem listViewItem2 = new ListViewItem(text);
              listViewItem1.Name = listViewItem2.Name = key;
              this._lvMaterial1.Items.Add(listViewItem1);
              this._lvMaterial2.Items.Add(listViewItem2);
            }
          }
        }
        if (this._lvMaterial1.Items.Count > 0)
        {
          this._lvMaterial1.Items[0].Selected = true;
          this._lvMaterial2.Items[0].Selected = true;
        }
        else
          this._btnAdd.Enabled = this._btnDel.Enabled = this._btnOK.Enabled = false;
      }
      finally
      {
        this._lvMaterial1.ResumeLayout();
        this._lvMaterial2.ResumeLayout();
      }
    }
    else
      this._btnAdd.Enabled = this._btnDel.Enabled = this._btnOK.Enabled = false;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (GlueMaterials));
    this._pnlBottom = new Panel();
    this._btnAdd = new Button();
    this._btnDel = new Button();
    this._btnOK = new Button();
    this._btnCancel = new Button();
    this.splitContainer1 = new SplitContainer();
    this._lb = new ListBox();
    this._tlp = new TableLayoutPanel();
    this._lvMaterial2 = new ListView();
    this._colName2 = new System.Windows.Forms.ColumnHeader();
    this._lvMaterial1 = new ListView();
    this._colName1 = new System.Windows.Forms.ColumnHeader();
    this._pnlBottom.SuspendLayout();
    this.splitContainer1.BeginInit();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.Panel2.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    this._tlp.SuspendLayout();
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
    componentResourceManager.ApplyResources((object) this.splitContainer1, "splitContainer1");
    this.splitContainer1.Name = "splitContainer1";
    this.splitContainer1.Panel1.Controls.Add((Control) this._lb);
    this.splitContainer1.Panel2.Controls.Add((Control) this._tlp);
    componentResourceManager.ApplyResources((object) this._lb, "_lb");
    this._lb.FormattingEnabled = true;
    this._lb.Name = "_lb";
    componentResourceManager.ApplyResources((object) this._tlp, "_tlp");
    this._tlp.Controls.Add((Control) this._lvMaterial2, 1, 0);
    this._tlp.Controls.Add((Control) this._lvMaterial1, 0, 0);
    this._tlp.Name = "_tlp";
    this._lvMaterial2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[1]
    {
      this._colName2
    });
    componentResourceManager.ApplyResources((object) this._lvMaterial2, "_lvMaterial2");
    this._lvMaterial2.FullRowSelect = true;
    this._lvMaterial2.HideSelection = false;
    this._lvMaterial2.MultiSelect = false;
    this._lvMaterial2.Name = "_lvMaterial2";
    this._lvMaterial2.UseCompatibleStateImageBehavior = false;
    this._lvMaterial2.View = View.Details;
    componentResourceManager.ApplyResources((object) this._colName2, "_colName2");
    this._lvMaterial1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[1]
    {
      this._colName1
    });
    componentResourceManager.ApplyResources((object) this._lvMaterial1, "_lvMaterial1");
    this._lvMaterial1.FullRowSelect = true;
    this._lvMaterial1.HideSelection = false;
    this._lvMaterial1.MultiSelect = false;
    this._lvMaterial1.Name = "_lvMaterial1";
    this._lvMaterial1.UseCompatibleStateImageBehavior = false;
    this._lvMaterial1.View = View.Details;
    componentResourceManager.ApplyResources((object) this._colName1, "_colName1");
    this.AcceptButton = (IButtonControl) this._btnOK;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this._btnCancel;
    this.Controls.Add((Control) this.splitContainer1);
    this.Controls.Add((Control) this._pnlBottom);
    this.DoubleBuffered = true;
    this.Name = nameof (GlueMaterials);
    this.ShowInTaskbar = false;
    this._pnlBottom.ResumeLayout(false);
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.Panel2.ResumeLayout(false);
    this.splitContainer1.EndInit();
    this.splitContainer1.ResumeLayout(false);
    this._tlp.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  private class GlueItem
  {
    internal string Key1 = string.Empty;
    internal string Key2 = string.Empty;
    internal string Caption = string.Empty;
    internal DataRow Row;
    internal bool Visible = true;

    internal GlueItem(string key1, string caption1, string key2, string caption2, DataRow row)
    {
      this.Key1 = key1;
      this.Key2 = key2;
      this.Caption = $"{caption1} - {caption2}";
      this.Row = row;
    }

    internal bool IsEqual(string key1, string key2)
    {
      if (this.Key1 == key1 && this.Key2 == key2)
        return true;
      return this.Key1 == key2 && this.Key2 == key1;
    }

    public override bool Equals(object obj)
    {
      if (!(obj is GlueMaterials.GlueItem glueItem))
        return false;
      if (this.Key1 == glueItem.Key1 && this.Key2 == glueItem.Key2)
        return true;
      return this.Key1 == glueItem.Key2 && this.Key2 == glueItem.Key1;
    }

    public override int GetHashCode() => this.Caption.GetHashCode();

    public override string ToString() => this.Caption;
  }
}
