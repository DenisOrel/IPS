// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.EditMaterialsTableForm
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

public class EditMaterialsTableForm : Form
{
  private Guid _tableRefGuid = Guid.Empty;
  private DataTable _dt;
  private long _tableID;
  private string _strColumnGuid = string.Empty;
  private Dictionary<string, EditMaterialsTableForm.lbItem> _items = new Dictionary<string, EditMaterialsTableForm.lbItem>();
  private bool _infoLoaded;
  private IContainer components;
  private Panel _pnlBottom;
  private Button _btnOK;
  private Button _btnCancel;
  private ToolStrip _ts;
  private ToolStripButton _tsBtnAdd;
  private ToolStripButton _tsBtnEdit;
  private ToolStripButton _tsBtnDel;
  private Label _lbName;
  private TextBox _txtName;
  private ListBox _lb;
  private ContextMenuStrip _contextMenu;
  private ToolStripMenuItem _cmAdd;
  private ToolStripMenuItem _cmEdit;
  private ToolStripMenuItem _cmDel;

  public string SelectedItemKey
  {
    get
    {
      string selectedItemKey = string.Empty;
      if (this._lb.SelectedItem != null)
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(this._tableRefGuid);
          if (!objectInfo.Empty)
          {
            string keyValue = ImbaseHelper.MakeInternalImbaseKey(objectInfo.ObjectID, (this._lb.SelectedItem as EditMaterialsTableForm.lbItem).RecID);
            selectedItemKey = ImbaseHelper.ConvertImbaseKey(sessionKeeper.Session, keyValue);
          }
        }
      }
      return selectedItemKey;
    }
  }

  public string SelectedItemText
  {
    get
    {
      return this._lb.SelectedItem == null ? string.Empty : (this._lb.SelectedItem as EditMaterialsTableForm.lbItem).Text;
    }
  }

  public EditMaterialsTableForm()
  {
    this.InitializeComponent();
    this._infoLoaded = this.LoadInfo();
    if (!this._infoLoaded)
      return;
    this.CreateItems();
  }

  private void On_lb_DoubleClick(object sender, EventArgs e)
  {
    if (this._lb.SelectedItem == null)
      return;
    this.DialogResult = DialogResult.OK;
    this.OnClosed(e);
  }

  private void On_lb_SelectedValueChanged(object sender, EventArgs e)
  {
    if (this._lb.SelectedItem == null)
      return;
    this._txtName.Text = this._lb.Text;
  }

  private void On_tsBtnAdd_Click(object sender, EventArgs e)
  {
    if (string.IsNullOrEmpty(this._txtName.Text))
      return;
    if (!this._items.ContainsKey(this._txtName.Text))
    {
      EditMaterialsTableForm.lbItem lbItem = new EditMaterialsTableForm.lbItem(-1L, this._txtName.Text, (DataRow) null);
      this._items.Add(this._txtName.Text, lbItem);
      this._lb.SelectedIndex = this._lb.Items.Add((object) lbItem);
    }
    else if (!this._items[this._txtName.Text].Visible)
    {
      EditMaterialsTableForm.lbItem lbItem = this._items[this._txtName.Text];
      this._lb.SelectedIndex = this._lb.Items.Add((object) lbItem);
      lbItem.Visible = true;
    }
    else
      this._lb.SelectedItem = (object) this._items[this._txtName.Text];
  }

  private void On_tsBtnDel_Click(object sender, EventArgs e)
  {
    if (string.IsNullOrEmpty(this._txtName.Text) || !this._items.ContainsKey(this._txtName.Text))
      return;
    this._lb.Items.Remove((object) this._items[this._txtName.Text]);
    this._items[this._txtName.Text].Visible = false;
  }

  private void On_tsBtnEdit_Click(object sender, EventArgs e)
  {
    if (string.IsNullOrEmpty(this._txtName.Text) || this._lb.SelectedItem == null)
      return;
    if (this._items.ContainsKey(this._txtName.Text))
    {
      string caption = LocalizationHolder.rm.GetString("IMH_DataChanged");
      int num = (int) MessageBox.Show(LocalizationHolder.rm.GetString("IMH_RepetitionItem_Msg"), caption, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }
    else
    {
      int selectedIndex = this._lb.SelectedIndex;
      EditMaterialsTableForm.lbItem selectedItem = this._lb.SelectedItem as EditMaterialsTableForm.lbItem;
      this._items.Remove(selectedItem.Text);
      this._lb.Items.Remove((object) selectedItem);
      selectedItem.Text = this._txtName.Text;
      this._items.Add(this._txtName.Text, selectedItem);
      this._lb.Items.Insert(selectedIndex, (object) selectedItem);
      this._lb.SelectedItem = (object) selectedItem;
    }
  }

  protected override void OnClosed(EventArgs e)
  {
    base.OnClosed(e);
    FormStorage.SaveLayout((Control) this);
  }

  protected override void OnClosing(CancelEventArgs e)
  {
    bool flag = false;
    foreach (KeyValuePair<string, EditMaterialsTableForm.lbItem> keyValuePair in this._items)
    {
      if (keyValuePair.Value.Row == null || !keyValuePair.Value.Visible)
      {
        flag = true;
        break;
      }
    }
    if (!flag)
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

  private void CreateItems()
  {
    string empty = string.Empty;
    foreach (DataRow row in (InternalDataCollectionBase) this._dt.Rows)
    {
      long int64 = Convert.ToInt64(row["F_KEY"]);
      string str = Convert.ToString(row[this._strColumnGuid]);
      if (!this._items.ContainsKey(str))
      {
        EditMaterialsTableForm.lbItem lbItem = new EditMaterialsTableForm.lbItem(int64, str, row);
        this._items.Add(str, lbItem);
        this._lb.Items.Add((object) lbItem);
      }
    }
  }

  private bool LoadInfo()
  {
    bool flag = false;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (sessionKeeper.Session.GetCustomService(typeof (IIMHSystemSettingsService)) is IIMHSystemSettingsService customService)
      {
        List<string> names = new List<string>((IEnumerable<string>) new string[2]
        {
          "MATERIAL_GROUPS_TABLE_NAME",
          "MATERIAL_GROUPS_COLUMN_NAME"
        });
        Dictionary<string, Guid> objectGuidsByNames = customService.GetObjectGuidsByNames(names);
        if (objectGuidsByNames != null)
        {
          this._tableRefGuid = objectGuidsByNames["MATERIAL_GROUPS_TABLE_NAME"];
          this._strColumnGuid = objectGuidsByNames["MATERIAL_GROUPS_COLUMN_NAME"].ToString();
          QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(this._tableRefGuid);
          if (!objectInfo.Empty)
          {
            DataSet tables = TableLoadHelper.GetTables(sessionKeeper.Session, objectInfo.ObjectID, true);
            this._tableID = IMHHelper.GetTableIDByTableRefID(objectInfo.ObjectID);
            if (tables == null)
              tables = TableLoadHelper.GetTables(sessionKeeper.Session, this._tableID, true);
            this._dt = tables == null || !tables.Tables.Contains("IMS_DATA") ? (DataTable) null : tables.Tables["IMS_DATA"];
            flag = this._dt != null && this._dt.Columns.Contains(this._strColumnGuid);
          }
        }
      }
    }
    return flag;
  }

  private void Save()
  {
    if (!this._infoLoaded)
      return;
    foreach (KeyValuePair<string, EditMaterialsTableForm.lbItem> keyValuePair in this._items)
    {
      EditMaterialsTableForm.lbItem lbItem = keyValuePair.Value;
      if (lbItem.Row == null)
      {
        if (lbItem.Visible)
        {
          DataRow row = this._dt.NewRow();
          row["F_GUID"] = (object) Guid.NewGuid();
          row[this._strColumnGuid] = (object) lbItem.Text;
          this._dt.Rows.Add(row);
          lbItem.RecID = Convert.ToInt64(row["F_KEY"]);
        }
      }
      else if (!lbItem.Visible)
        this._dt.Rows.Remove(lbItem.Row);
    }
    this._dt.AcceptChanges();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      TableLoadHelper.StoreData(sessionKeeper.Session, this._tableID, this._dt.DataSet, sessionKeeper.Session.GetCustomService(typeof (ITablesIndexer)) as ITablesIndexer);
    ServiceUtils.GetService<INotificationService>((object) ApplicationServices.Container, false)?.FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", this._tableID));
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (EditMaterialsTableForm));
    this._pnlBottom = new Panel();
    this._btnOK = new Button();
    this._btnCancel = new Button();
    this._ts = new ToolStrip();
    this._tsBtnAdd = new ToolStripButton();
    this._tsBtnEdit = new ToolStripButton();
    this._tsBtnDel = new ToolStripButton();
    this._lbName = new Label();
    this._txtName = new TextBox();
    this._lb = new ListBox();
    this._contextMenu = new ContextMenuStrip(this.components);
    this._cmAdd = new ToolStripMenuItem();
    this._cmEdit = new ToolStripMenuItem();
    this._cmDel = new ToolStripMenuItem();
    this._pnlBottom.SuspendLayout();
    this._ts.SuspendLayout();
    this._contextMenu.SuspendLayout();
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
    this._ts.Items.AddRange(new ToolStripItem[3]
    {
      (ToolStripItem) this._tsBtnAdd,
      (ToolStripItem) this._tsBtnEdit,
      (ToolStripItem) this._tsBtnDel
    });
    componentResourceManager.ApplyResources((object) this._ts, "_ts");
    this._ts.Name = "_ts";
    this._tsBtnAdd.DisplayStyle = ToolStripItemDisplayStyle.Image;
    componentResourceManager.ApplyResources((object) this._tsBtnAdd, "_tsBtnAdd");
    this._tsBtnAdd.Name = "_tsBtnAdd";
    this._tsBtnAdd.Click += new EventHandler(this.On_tsBtnAdd_Click);
    this._tsBtnEdit.DisplayStyle = ToolStripItemDisplayStyle.Image;
    componentResourceManager.ApplyResources((object) this._tsBtnEdit, "_tsBtnEdit");
    this._tsBtnEdit.Name = "_tsBtnEdit";
    this._tsBtnEdit.Click += new EventHandler(this.On_tsBtnEdit_Click);
    this._tsBtnDel.DisplayStyle = ToolStripItemDisplayStyle.Image;
    componentResourceManager.ApplyResources((object) this._tsBtnDel, "_tsBtnDel");
    this._tsBtnDel.Name = "_tsBtnDel";
    this._tsBtnDel.Click += new EventHandler(this.On_tsBtnDel_Click);
    componentResourceManager.ApplyResources((object) this._lbName, "_lbName");
    this._lbName.Name = "_lbName";
    componentResourceManager.ApplyResources((object) this._txtName, "_txtName");
    this._txtName.Name = "_txtName";
    componentResourceManager.ApplyResources((object) this._lb, "_lb");
    this._lb.ContextMenuStrip = this._contextMenu;
    this._lb.FormattingEnabled = true;
    this._lb.Name = "_lb";
    this._lb.SelectedValueChanged += new EventHandler(this.On_lb_SelectedValueChanged);
    this._lb.DoubleClick += new EventHandler(this.On_lb_DoubleClick);
    this._contextMenu.Items.AddRange(new ToolStripItem[3]
    {
      (ToolStripItem) this._cmAdd,
      (ToolStripItem) this._cmEdit,
      (ToolStripItem) this._cmDel
    });
    this._contextMenu.Name = "_contextMenu";
    componentResourceManager.ApplyResources((object) this._contextMenu, "_contextMenu");
    componentResourceManager.ApplyResources((object) this._cmAdd, "_cmAdd");
    this._cmAdd.Name = "_cmAdd";
    this._cmAdd.Click += new EventHandler(this.On_tsBtnAdd_Click);
    componentResourceManager.ApplyResources((object) this._cmEdit, "_cmEdit");
    this._cmEdit.Name = "_cmEdit";
    this._cmEdit.Click += new EventHandler(this.On_tsBtnEdit_Click);
    componentResourceManager.ApplyResources((object) this._cmDel, "_cmDel");
    this._cmDel.Name = "_cmDel";
    this._cmDel.Click += new EventHandler(this.On_tsBtnDel_Click);
    this.AcceptButton = (IButtonControl) this._btnOK;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this._btnCancel;
    this.Controls.Add((Control) this._lb);
    this.Controls.Add((Control) this._txtName);
    this.Controls.Add((Control) this._lbName);
    this.Controls.Add((Control) this._ts);
    this.Controls.Add((Control) this._pnlBottom);
    this.DoubleBuffered = true;
    this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (EditMaterialsTableForm);
    this.ShowIcon = false;
    this.ShowInTaskbar = false;
    this._pnlBottom.ResumeLayout(false);
    this._ts.ResumeLayout(false);
    this._ts.PerformLayout();
    this._contextMenu.ResumeLayout(false);
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  private class lbItem
  {
    internal long RecID = -1;
    internal string Text = string.Empty;
    internal DataRow Row;
    internal bool Visible = true;

    internal lbItem(long recID, string caption, DataRow row)
    {
      this.RecID = recID;
      this.Text = caption;
      this.Row = row;
    }

    public override bool Equals(object obj)
    {
      bool flag = false;
      if (obj != null && this.GetType() == obj.GetType())
        flag = (obj as EditMaterialsTableForm.lbItem).Text == this.Text;
      return flag;
    }

    public override int GetHashCode() => this.Text.GetHashCode();

    public override string ToString() => this.Text;
  }
}
