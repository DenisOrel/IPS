// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.ImportForm
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using Intermech.Site.Client.PortalNavigator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client;

internal class ImportForm : Form
{
  private ObjectTypesFilterForm _objectTypesFilterForm;
  private List<int> _savedUncheckedTypes;
  protected List<long> objectIDs;
  protected IServiceProvider viewServices;
  private IContainer components;
  protected Panel panel1;
  protected Button bImport;
  protected Button bCancel;
  protected Panel panel2;
  protected Button bObjectTypesFilter;
  protected GroupBox groupBox1;
  protected ComboBox cbComposition;
  protected GroupBox groupBox2;
  protected ComboBox cbOwners;
  protected CheckBox cbStartImmediately;
  protected CheckBox cbAutoUpdate;
  protected Button bObjectsList;
  protected ObjectsListControl viewObjectsList;

  public ImportForm()
  {
    this.InitializeComponent();
    HelpProvidersClass.SetHelpOptionForControl((Control) this, 1657);
    INamedImageList service = (INamedImageList) ServicesManager.GetService(typeof (INamedImageList));
    if (service != null)
      this.Icon = ImagesResizeHelper.GetIconFromImage(service.ImageList.Images[service.ImageIndex(SiteClientConsts.ImageImportName)]);
    FormStorage.LoadLayout((Control) this);
  }

  public List<long> ImportedObjectIDs => this.objectIDs;

  public virtual object Options
  {
    get
    {
      return (object) new ImportOptions()
      {
        AutoUpdate = this.cbAutoUpdate.Checked,
        CompositionType = (SelectCompositionType) this.cbComposition.SelectedIndex,
        FilteredTypes = this.FilteredObjectTypes,
        SetOwner = (this.cbOwners.SelectedIndex == 0),
        StartImmediately = this.cbStartImmediately.Checked
      };
    }
  }

  protected List<int> FilteredObjectTypes
  {
    get
    {
      return this._objectTypesFilterForm != null ? this._objectTypesFilterForm.FilteredObjectTypes : (List<int>) null;
    }
  }

  public virtual void Initialize(ISelectedItems items, IServiceProvider viewServices)
  {
    this.viewServices = viewServices;
    if (items.GetItemData(0, typeof (IPublishObjectID)) is IPublishObjectID)
      new ObjectListView(this.viewObjectsList).InitView(items, viewServices, out this.objectIDs);
    IDBConfigurations service1 = ServicesManager.GetService(typeof (IDBConfigurations)) as IDBConfigurations;
    ICurrentUserAndRole service2 = ServicesManager.GetService(typeof (ICurrentUserAndRole)) as ICurrentUserAndRole;
    int int32_1 = Convert.ToInt32(service1.ReadInteger(PortalConsts.PortalClientModuleName, "IMPORT_SETTINGS", SiteClientConsts.CfgOwner, 1L, DBConfigMode.UserOnly));
    this.cbOwners.SelectedIndex = int32_1 < 0 ? 1 : int32_1;
    int int32_2 = Convert.ToInt32(service1.ReadInteger(PortalConsts.PortalClientModuleName, "IMPORT_SETTINGS", SiteClientConsts.CfgUserCompositionType, 2L, DBConfigMode.UserOnly));
    this.cbComposition.SelectedIndex = int32_2 < 0 ? 2 : int32_2;
    this.cbAutoUpdate.Checked = service1.ReadBool(PortalConsts.PortalClientModuleName, "IMPORT_SETTINGS", SiteClientConsts.CfgUserAutoUpdate, false, DBConfigMode.UserOnly);
    this.cbStartImmediately.Checked = service1.ReadBool(PortalConsts.PortalClientModuleName, "IMPORT_SETTINGS", SiteClientConsts.CfgStartImmediately, false, DBConfigMode.UserOnly);
    DataTable dataTable = service1.ReadSection(PortalConsts.PortalClientModuleName, "IMPORT_SETTINGS_UNCH_TYPES", service2.UserID);
    if (dataTable == null || dataTable.Rows.Count <= 0)
      return;
    this._savedUncheckedTypes = new List<int>(dataTable.Rows.Count);
    foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      this._savedUncheckedTypes.Add(Convert.ToInt32(row["F_VALUE"]));
  }

  private void ImportForm_FormClosing(object sender, FormClosingEventArgs e)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBConfigurations configurations = sessionKeeper.Session.Configurations;
      configurations.WriteInteger(PortalConsts.PortalClientModuleName, "IMPORT_SETTINGS", SiteClientConsts.CfgOwner, (long) this.cbOwners.SelectedIndex, sessionKeeper.Session.UserID);
      configurations.WriteInteger(PortalConsts.PortalClientModuleName, "IMPORT_SETTINGS", SiteClientConsts.CfgUserCompositionType, (long) this.cbComposition.SelectedIndex, sessionKeeper.Session.UserID);
      configurations.WriteBool(PortalConsts.PortalClientModuleName, "IMPORT_SETTINGS", SiteClientConsts.CfgUserAutoUpdate, this.cbAutoUpdate.Checked, sessionKeeper.Session.UserID);
      configurations.WriteBool(PortalConsts.PortalClientModuleName, "IMPORT_SETTINGS", SiteClientConsts.CfgStartImmediately, this.cbStartImmediately.Checked, sessionKeeper.Session.UserID);
      if (this._objectTypesFilterForm != null)
      {
        List<int> filteredObjectTypes = this._objectTypesFilterForm.FilteredObjectTypes;
        bool flag = false;
        if (filteredObjectTypes != null)
        {
          if (this._savedUncheckedTypes != null)
          {
            if (filteredObjectTypes.Except<int>((IEnumerable<int>) this._savedUncheckedTypes).ToList<int>().Count > 0)
              flag = true;
          }
          else
            flag = true;
        }
        if (flag)
        {
          DataTable table = new DataTable();
          table.Columns.Add("F_PARAM_NAME", typeof (string));
          table.Columns.Add("F_VALUE", typeof (string));
          int num1 = 0;
          foreach (int num2 in filteredObjectTypes)
          {
            DataRow row = table.NewRow();
            row["F_PARAM_NAME"] = (object) num1.ToString();
            row["F_VALUE"] = (object) num2.ToString();
            table.Rows.Add(row);
            ++num1;
          }
          configurations.WriteSection(PortalConsts.PortalClientModuleName, "IMPORT_SETTINGS_UNCH_TYPES", table, sessionKeeper.Session.UserID);
        }
      }
    }
    FormStorage.SaveLayout((Control) this);
  }

  private void bObjectTypesFilter_Click(object sender, EventArgs e)
  {
    if (this._objectTypesFilterForm == null)
    {
      this._objectTypesFilterForm = new ObjectTypesFilterForm();
      using (new SessionKeeper())
        this._objectTypesFilterForm.LoadData((List<int>) null, (List<int>) null, this._savedUncheckedTypes != null ? new List<int>((IEnumerable<int>) this._savedUncheckedTypes) : (List<int>) null);
    }
    int num = (int) this._objectTypesFilterForm.ShowDialog();
  }

  private void bObjectsList_Click(object sender, EventArgs e)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      Guid connectGuid = Guid.Empty;
      IPortalConnector customService = (IPortalConnector) sessionKeeper.Session.GetCustomService(typeof (IPortalConnector));
      try
      {
        connectGuid = customService.Login(sessionKeeper.Session.SessionGUID);
        this.ShowImportObjectList(sessionKeeper.Session, customService, connectGuid);
      }
      finally
      {
        if (connectGuid != Guid.Empty && customService != null)
          customService.Logout(connectGuid);
      }
    }
  }

  protected virtual void ShowImportObjectList(
    IUserSession session,
    IPortalConnector connection,
    Guid connectGuid)
  {
    List<long> objectIDs = new List<long>();
    List<int> types = new List<int>();
    SelectCompositionType selectedIndex = (SelectCompositionType) this.cbComposition.SelectedIndex;
    int countLevels = selectedIndex == SelectCompositionType.RecursiveComposition ? -1 : (int) selectedIndex;
    if (countLevels == 0)
    {
      objectIDs = this.objectIDs;
    }
    else
    {
      int[] filteredTypes = (int[]) null;
      if (this._objectTypesFilterForm != null)
      {
        List<int> filteredObjectTypes = this._objectTypesFilterForm.FilteredObjectTypes;
        if (filteredObjectTypes != null)
          filteredTypes = filteredObjectTypes.ToArray();
      }
      long[] importComposition = connection.GetImportComposition(session.SessionGUID, this.objectIDs.ToArray(), filteredTypes, countLevels);
      if (importComposition != null && importComposition.Length != 0)
        objectIDs.AddRange((IEnumerable<long>) importComposition);
    }
    using (ObjectListForm objectListForm = new ObjectListForm("Импортируемые объекты"))
    {
      objectListForm.Initialize(this.viewServices, objectIDs, types);
      int num = (int) objectListForm.ShowDialog();
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ImportForm));
    this.panel1 = new Panel();
    this.bImport = new Button();
    this.bCancel = new Button();
    this.panel2 = new Panel();
    this.viewObjectsList = new ObjectsListControl();
    this.cbStartImmediately = new CheckBox();
    this.cbAutoUpdate = new CheckBox();
    this.bObjectsList = new Button();
    this.bObjectTypesFilter = new Button();
    this.groupBox1 = new GroupBox();
    this.cbComposition = new ComboBox();
    this.groupBox2 = new GroupBox();
    this.cbOwners = new ComboBox();
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.groupBox1.SuspendLayout();
    this.groupBox2.SuspendLayout();
    this.SuspendLayout();
    this.panel1.Controls.Add((Control) this.bImport);
    this.panel1.Controls.Add((Control) this.bCancel);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    componentResourceManager.ApplyResources((object) this.bImport, "bImport");
    this.bImport.DialogResult = DialogResult.OK;
    this.bImport.Name = "bImport";
    this.bImport.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.bCancel, "bCancel");
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Name = "bCancel";
    this.bCancel.UseVisualStyleBackColor = true;
    this.panel2.Controls.Add((Control) this.viewObjectsList);
    this.panel2.Controls.Add((Control) this.cbStartImmediately);
    this.panel2.Controls.Add((Control) this.cbAutoUpdate);
    this.panel2.Controls.Add((Control) this.bObjectsList);
    this.panel2.Controls.Add((Control) this.bObjectTypesFilter);
    this.panel2.Controls.Add((Control) this.groupBox1);
    this.panel2.Controls.Add((Control) this.groupBox2);
    componentResourceManager.ApplyResources((object) this.panel2, "panel2");
    this.panel2.Name = "panel2";
    this.viewObjectsList.AllowCustomGroupValues = true;
    componentResourceManager.ApplyResources((object) this.viewObjectsList, "viewObjectsList");
    this.viewObjectsList.Control = (object) this.viewObjectsList;
    this.viewObjectsList.DataLoaded = false;
    this.viewObjectsList.DisableColumnsGrouping = true;
    this.viewObjectsList.DisableGroupBox = true;
    this.viewObjectsList.DisableIMContextMenu = true;
    this.viewObjectsList.DisableKeyDownEvents = false;
    this.viewObjectsList.DisableStatusBar = true;
    this.viewObjectsList.DisableToolBar = true;
    this.viewObjectsList.EmbeddedFocusAndSelection = (iFocusAndSelection) null;
    this.viewObjectsList.Name = "viewObjectsList";
    componentResourceManager.ApplyResources((object) this.cbStartImmediately, "cbStartImmediately");
    this.cbStartImmediately.Name = "cbStartImmediately";
    this.cbStartImmediately.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.cbAutoUpdate, "cbAutoUpdate");
    this.cbAutoUpdate.Name = "cbAutoUpdate";
    this.cbAutoUpdate.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.bObjectsList, "bObjectsList");
    this.bObjectsList.Name = "bObjectsList";
    this.bObjectsList.UseVisualStyleBackColor = true;
    this.bObjectsList.Click += new EventHandler(this.bObjectsList_Click);
    componentResourceManager.ApplyResources((object) this.bObjectTypesFilter, "bObjectTypesFilter");
    this.bObjectTypesFilter.Name = "bObjectTypesFilter";
    this.bObjectTypesFilter.UseVisualStyleBackColor = true;
    this.bObjectTypesFilter.Click += new EventHandler(this.bObjectTypesFilter_Click);
    componentResourceManager.ApplyResources((object) this.groupBox1, "groupBox1");
    this.groupBox1.Controls.Add((Control) this.cbComposition);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.TabStop = false;
    componentResourceManager.ApplyResources((object) this.cbComposition, "cbComposition");
    this.cbComposition.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbComposition.FormattingEnabled = true;
    this.cbComposition.Items.AddRange(new object[3]
    {
      (object) componentResourceManager.GetString("cbComposition.Items"),
      (object) componentResourceManager.GetString("cbComposition.Items1"),
      (object) componentResourceManager.GetString("cbComposition.Items2")
    });
    this.cbComposition.Name = "cbComposition";
    componentResourceManager.ApplyResources((object) this.groupBox2, "groupBox2");
    this.groupBox2.Controls.Add((Control) this.cbOwners);
    this.groupBox2.Name = "groupBox2";
    this.groupBox2.TabStop = false;
    this.cbOwners.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbOwners.FormattingEnabled = true;
    this.cbOwners.Items.AddRange(new object[2]
    {
      (object) componentResourceManager.GetString("cbOwners.Items"),
      (object) componentResourceManager.GetString("cbOwners.Items1")
    });
    componentResourceManager.ApplyResources((object) this.cbOwners, "cbOwners");
    this.cbOwners.Name = "cbOwners";
    this.AcceptButton = (IButtonControl) this.bImport;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.bCancel;
    this.Controls.Add((Control) this.panel2);
    this.Controls.Add((Control) this.panel1);
    this.Name = nameof (ImportForm);
    this.FormClosing += new FormClosingEventHandler(this.ImportForm_FormClosing);
    this.panel1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.panel2.PerformLayout();
    this.groupBox1.ResumeLayout(false);
    this.groupBox2.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
