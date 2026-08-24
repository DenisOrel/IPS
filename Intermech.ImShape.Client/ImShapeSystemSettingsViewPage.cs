// Decompiled with JetBrains decompiler
// Type: Intermech.ImShape.Client.ImShapeSystemSettingsViewPage
// Assembly: Intermech.ImShape.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: EAEE73DE-1C1F-4401-8BB6-D181BFA32870
// Assembly location: D:\IPS\Client\Intermech.ImShape.Client.dll

using ImSSP;
using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImShape.Client;

public class ImShapeSystemSettingsViewPage : UserControl, IPropertyPage
{
  private Dictionary<int, bool> _typeIDs = new Dictionary<int, bool>();
  private IContainer components;
  private Label _lbTypeNames;
  private Panel _pnlRight;
  private Panel _pnlGeneral;
  private Button _btnSelect;
  private DataGridView _dgv;
  private DataGridViewImageColumn colIco;
  private DataGridViewTextBoxColumn colObjType;
  private DataGridViewCheckBoxColumn colAuto;

  public ImShapeSystemSettingsViewPage()
  {
    this.InitializeComponent();
    this.Type = PropertyPageType.Control;
    this.PageName = LocalizationHolder.rm.GetString("ImShape.SystemPage.Name");
    this.HelpTopicID = sc_9105.ssp_imbase_9106();
    this.ReadConfigSettings();
    this.LoadItems();
  }

  private void On_btnSelect_Click(object sender, EventArgs e)
  {
  }

  private void On_dgv_CellClick(object sender, DataGridViewCellEventArgs e)
  {
    if (e.ColumnIndex != 2)
      return;
    EventHandler changed = this.Changed;
    if (changed == null)
      return;
    changed((object) this, new EventArgs());
  }

  public event EventHandler Changed;

  public PropertyPageType Type { get; private set; }

  public object Control => (object) this;

  public string PageName { get; private set; }

  public void Apply()
  {
    ImShapeSystemSettingsService service = ServiceUtils.GetService<ImShapeSystemSettingsService>((object) ServicesManager.ServiceContainer, false);
    if (service == null)
      return;
    this.GetData();
    service.SaveSistemSettings(this._typeIDs);
  }

  public void Cancel()
  {
    this.ReadConfigSettings();
    this.LoadItems();
  }

  public string HelpTopicID { get; private set; }

  public string HeaderText
  {
    [DebuggerStepThrough] get => this.PageName;
  }

  private void LoadItems()
  {
    ICurrentUserAndRole service = ServicesManager.GetService(typeof (ICurrentUserAndRole)) as ICurrentUserAndRole;
    this._dgv.Rows.Clear();
    foreach (KeyValuePair<int, bool> typeId in this._typeIDs)
    {
      int index = Statics.IconSrv.IndexOf(4, typeId.Key);
      this._dgv.Rows[this._dgv.Rows.Add((object) (index > -1 ? Statics.IconSrv.ImageList.Images[index] : (Image) null), (object) MetaDataHelper.GetObjectTypeName(typeId.Key), (object) typeId.Value)].Tag = (object) typeId.Key;
    }
    this._dgv.Enabled = service.IsAdmin;
  }

  private void ReadConfigSettings()
  {
    ImShapeSystemSettingsService service = ServiceUtils.GetService<ImShapeSystemSettingsService>((object) ServicesManager.ServiceContainer, false);
    if (service == null)
      return;
    this._typeIDs = service.TypeIDs;
  }

  private void GetData()
  {
    this._typeIDs.Clear();
    foreach (DataGridViewRow row in (IEnumerable) this._dgv.Rows)
      this._typeIDs[Convert.ToInt32(row.Tag)] = Convert.ToBoolean(row.Cells[this.colAuto.Name].Value);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ImShapeSystemSettingsViewPage));
    DataGridViewCellStyle gridViewCellStyle = new DataGridViewCellStyle();
    this._lbTypeNames = new Label();
    this._pnlRight = new Panel();
    this._btnSelect = new Button();
    this._pnlGeneral = new Panel();
    this._dgv = new DataGridView();
    this.colIco = new DataGridViewImageColumn();
    this.colObjType = new DataGridViewTextBoxColumn();
    this.colAuto = new DataGridViewCheckBoxColumn();
    this._pnlRight.SuspendLayout();
    this._pnlGeneral.SuspendLayout();
    ((ISupportInitialize) this._dgv).BeginInit();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this._lbTypeNames, "_lbTypeNames");
    this._lbTypeNames.Name = "_lbTypeNames";
    this._pnlRight.Controls.Add((System.Windows.Forms.Control) this._btnSelect);
    componentResourceManager.ApplyResources((object) this._pnlRight, "_pnlRight");
    this._pnlRight.Name = "_pnlRight";
    componentResourceManager.ApplyResources((object) this._btnSelect, "_btnSelect");
    this._btnSelect.Name = "_btnSelect";
    this._btnSelect.UseVisualStyleBackColor = true;
    this._pnlGeneral.Controls.Add((System.Windows.Forms.Control) this._dgv);
    componentResourceManager.ApplyResources((object) this._pnlGeneral, "_pnlGeneral");
    this._pnlGeneral.Name = "_pnlGeneral";
    this._dgv.AllowUserToAddRows = false;
    this._dgv.AllowUserToDeleteRows = false;
    this._dgv.AllowUserToResizeColumns = false;
    this._dgv.AllowUserToResizeRows = false;
    this._dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
    this._dgv.BackgroundColor = SystemColors.Window;
    this._dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    this._dgv.Columns.AddRange((DataGridViewColumn) this.colIco, (DataGridViewColumn) this.colObjType, (DataGridViewColumn) this.colAuto);
    componentResourceManager.ApplyResources((object) this._dgv, "_dgv");
    this._dgv.Name = "_dgv";
    this._dgv.RowHeadersVisible = false;
    this._dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
    this._dgv.CellClick += new DataGridViewCellEventHandler(this.On_dgv_CellClick);
    this.colIco.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
    gridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
    gridViewCellStyle.NullValue = componentResourceManager.GetObject("dataGridViewCellStyle1.NullValue");
    gridViewCellStyle.Padding = new Padding(3, 0, 0, 0);
    this.colIco.DefaultCellStyle = gridViewCellStyle;
    this.colIco.Frozen = true;
    componentResourceManager.ApplyResources((object) this.colIco, "colIco");
    this.colIco.Name = "colIco";
    this.colIco.ReadOnly = true;
    this.colObjType.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
    this.colObjType.FillWeight = 50f;
    componentResourceManager.ApplyResources((object) this.colObjType, "colObjType");
    this.colObjType.Name = "colObjType";
    this.colObjType.ReadOnly = true;
    this.colAuto.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
    componentResourceManager.ApplyResources((object) this.colAuto, "colAuto");
    this.colAuto.Name = "colAuto";
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((System.Windows.Forms.Control) this._pnlGeneral);
    this.Controls.Add((System.Windows.Forms.Control) this._pnlRight);
    this.Controls.Add((System.Windows.Forms.Control) this._lbTypeNames);
    this.DoubleBuffered = true;
    this.Name = nameof (ImShapeSystemSettingsViewPage);
    this._pnlRight.ResumeLayout(false);
    this._pnlGeneral.ResumeLayout(false);
    ((ISupportInitialize) this._dgv).EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
