// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.MaterialProperties
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using ImSSP;
using Intermech.Client.Core;
using Intermech.Imbase;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Imbase;
using Intermech.Interfaces.MaterialsHandbook;
using Intermech.Localization;
using Intermech.Navigator;
using Intermech.Navigator.Interfaces;
using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class MaterialProperties : Form
{
  private IContainer components;
  private ContextMenuStrip _contextMenu;
  private ToolStripMenuItem _miCollapse;
  private ToolStripMenuItem _miExpand;
  private Panel _pnl;
  private Button _btnDel;
  private Button _btnAdd;
  private Button _btnCancel;
  private MaterialPropertiesPage _propsPage;

  public MaterialProperties(string imbaseKey, string materialCaption = "")
  {
    this.InitializeComponent();
    this._propsPage.ImbaseKey = imbaseKey;
    if (string.IsNullOrEmpty(materialCaption))
      return;
    this.Text = $"{this.Text} ({materialCaption})";
  }

  private void On_btnAdd_Click(object sender, EventArgs e)
  {
    IDescriptor rootDescriptor = (IDescriptor) new Intermech.Navigator.DBObjectTypes.Descriptor(MetaDataHelper.GetObjectTypeID(Intermech.Imbase.Consts.MaterialPropertiesObjTypeGuid));
    long[] numArray = SelectionWindow.SelectObjects(LocalizationHolder.rm.GetString("IMH_SelectObject"), LocalizationHolder.rm.GetString("IMH_SelectObjectProperties"), rootDescriptor, SelectionOptions.SelectObjects | SelectionOptions.DisableSelectFromTree | SelectionOptions.DisableSelectAbstractTypes | SelectionOptions.DisableMultiselect);
    if (numArray == null || numArray.Length == 0)
      return;
    if (!this._propsPage.IsSettingsLoaded)
      this._propsPage.ReloadSettingsData();
    if (!this._propsPage.IsSettingsLoaded)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      DataTable settingsTable = this._propsPage.SettingsTable;
      bool isGuidKey = false;
      string imbaseKey = this._propsPage.ImbaseKey;
      string str = ImbaseHelper.ConvertImbaseKey(sessionKeeper.Session, imbaseKey, out isGuidKey);
      DataRow[] dataRowArray = settingsTable.Select(string.Format("[{0}]='{1}' or [{0}]='{2}'", (object) this._propsPage.ColMaterial, (object) imbaseKey, (object) str));
      if (dataRowArray.Length != 0)
      {
        dataRowArray[0][this._propsPage.ColObject] = (object) numArray[0];
      }
      else
      {
        DataRow row = settingsTable.NewRow();
        row["F_GUID"] = (object) Guid.NewGuid();
        row[this._propsPage.ColMaterial] = isGuidKey ? (object) str : (object) imbaseKey;
        row[this._propsPage.ColObject] = (object) numArray[0];
        settingsTable.Rows.Add(row);
      }
      settingsTable.AcceptChanges();
      long tableIdByTableRefId = IMHHelper.GetTableIDByTableRefID(IMHHelper.GetObjectIDByConstName("MATERIAL_PROPERTIES_TABLE_NAME"));
      TableLoadHelper.StoreData(sessionKeeper.Session, tableIdByTableRefId, settingsTable.DataSet, sessionKeeper.Session.GetCustomService(typeof (ITablesIndexer)) as ITablesIndexer);
      this._propsPage.ImbaseKey = imbaseKey;
    }
  }

  private void On_btnDel_Click(object sender, EventArgs e)
  {
    string caption = LocalizationHolder.rm.GetString("IMH_DeleteProperties");
    if (MessageBox.Show(LocalizationHolder.rm.GetString("IMH_DeleteProperties_Msg"), caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
      return;
    if (!this._propsPage.IsSettingsLoaded)
      this._propsPage.ReloadSettingsData();
    if (!this._propsPage.IsSettingsLoaded)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      DataTable settingsTable = this._propsPage.SettingsTable;
      string imbaseKey = this._propsPage.ImbaseKey;
      string str = ImbaseHelper.ConvertImbaseKey(sessionKeeper.Session, imbaseKey);
      DataRow[] dataRowArray = settingsTable.Select(string.Format("[{0}]='{1}' or [{0}]='{2}'", (object) this._propsPage.ColMaterial, (object) imbaseKey, (object) str));
      if (dataRowArray.Length == 0)
        return;
      for (int index = 0; index < dataRowArray.Length; ++index)
        settingsTable.Rows.Remove(dataRowArray[index]);
      settingsTable.AcceptChanges();
      long tableIdByTableRefId = IMHHelper.GetTableIDByTableRefID(IMHHelper.GetObjectIDByConstName("MATERIAL_PROPERTIES_TABLE_NAME"));
      TableLoadHelper.StoreData(sessionKeeper.Session, tableIdByTableRefId, settingsTable.DataSet, sessionKeeper.Session.GetCustomService(typeof (ITablesIndexer)) as ITablesIndexer);
      this._propsPage.Clear(true);
    }
  }

  private void On_miClick(object sender, EventArgs e)
  {
    this._propsPage.ExpandAll((int) Convert.ToInt16((sender as ToolStripMenuItem).Tag) == sc_14500.ssp_imbase_14501(1169046531));
  }

  protected override void OnClosed(EventArgs e)
  {
    base.OnClosed(e);
    FormStorage.SaveLayout((Control) this);
  }

  protected override void OnClosing(CancelEventArgs e)
  {
    if (this._propsPage.PropertiesChanged)
    {
      string caption = LocalizationHolder.rm.GetString("IMH_PropertiesChanged_Caption");
      switch (MessageBox.Show((IWin32Window) this, LocalizationHolder.rm.GetString("IMH_PropertiesChanged_Msg"), caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
      {
        case DialogResult.Cancel:
          e.Cancel = true;
          break;
        case DialogResult.Yes:
          this._propsPage.SaveChanged();
          break;
      }
    }
    base.OnClosing(e);
  }

  protected override void OnLoad(EventArgs e)
  {
    base.OnLoad(e);
    FormStorage.LoadLayout((Control) this);
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MaterialProperties));
    this._contextMenu = new ContextMenuStrip(this.components);
    this._miCollapse = new ToolStripMenuItem();
    this._miExpand = new ToolStripMenuItem();
    this._pnl = new Panel();
    this._btnCancel = new Button();
    this._btnDel = new Button();
    this._btnAdd = new Button();
    this._propsPage = new MaterialPropertiesPage();
    this._contextMenu.SuspendLayout();
    this._pnl.SuspendLayout();
    this.SuspendLayout();
    this._contextMenu.Items.AddRange(new ToolStripItem[2]
    {
      (ToolStripItem) this._miCollapse,
      (ToolStripItem) this._miExpand
    });
    this._contextMenu.Name = "_contextMenu";
    componentResourceManager.ApplyResources((object) this._contextMenu, "_contextMenu");
    this._miCollapse.DisplayStyle = ToolStripItemDisplayStyle.Text;
    this._miCollapse.Name = "_miCollapse";
    componentResourceManager.ApplyResources((object) this._miCollapse, "_miCollapse");
    this._miCollapse.Tag = (object) "0";
    this._miCollapse.Click += new EventHandler(this.On_miClick);
    this._miExpand.DisplayStyle = ToolStripItemDisplayStyle.Text;
    this._miExpand.Name = "_miExpand";
    componentResourceManager.ApplyResources((object) this._miExpand, "_miExpand");
    this._miExpand.Tag = (object) "1";
    this._miExpand.Click += new EventHandler(this.On_miClick);
    this._pnl.Controls.Add((Control) this._btnCancel);
    this._pnl.Controls.Add((Control) this._btnDel);
    this._pnl.Controls.Add((Control) this._btnAdd);
    componentResourceManager.ApplyResources((object) this._pnl, "_pnl");
    this._pnl.Name = "_pnl";
    this._btnCancel.DialogResult = DialogResult.Cancel;
    componentResourceManager.ApplyResources((object) this._btnCancel, "_btnCancel");
    this._btnCancel.Name = "_btnCancel";
    this._btnCancel.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this._btnDel, "_btnDel");
    this._btnDel.Name = "_btnDel";
    this._btnDel.UseVisualStyleBackColor = true;
    this._btnDel.Click += new EventHandler(this.On_btnDel_Click);
    componentResourceManager.ApplyResources((object) this._btnAdd, "_btnAdd");
    this._btnAdd.Name = "_btnAdd";
    this._btnAdd.UseVisualStyleBackColor = true;
    this._btnAdd.Click += new EventHandler(this.On_btnAdd_Click);
    this._propsPage.ContextMenuStrip = this._contextMenu;
    componentResourceManager.ApplyResources((object) this._propsPage, "_propsPage");
    this._propsPage.ImbaseKey = "";
    this._propsPage.Name = "_propsPage";
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this._btnCancel;
    this.ContextMenuStrip = this._contextMenu;
    this.Controls.Add((Control) this._propsPage);
    this.Controls.Add((Control) this._pnl);
    this.DoubleBuffered = true;
    this.Name = nameof (MaterialProperties);
    this.ShowInTaskbar = false;
    this._contextMenu.ResumeLayout(false);
    this._pnl.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
