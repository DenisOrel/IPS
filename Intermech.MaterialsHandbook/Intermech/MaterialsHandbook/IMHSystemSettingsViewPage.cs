// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.IMHSystemSettingsViewPage
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Imbase.Indexes;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.MaterialsHandbook;
using Intermech.Localization;
using Intermech.MaterialsHandbook.BackgroundTask;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class IMHSystemSettingsViewPage : UserControl, IPropertyPage, IPropertyPageSearchOptionEvents
{
  private ConfigSettingsCustomDescriptor _csCd;
  private bool _modified;
  private IContainer components;
  private TabControl _tc;
  private TabPage _tpConfigFileSettings;
  private TabPage _tpAssortmentSearch;
  private PropertyGrid _pgConfigFileSettings;
  private AssortmentSearchSettingsCtrl _searchSettingsCtrl;

  public IMHSystemSettingsViewPage(IServiceProvider services)
  {
    this.InitializeComponent();
    if (services.GetService(typeof (IPropertyPagesService)) is IPropertyPagesService service)
      service.AddPage(LocalizationHolder.rm.GetString("MaterialsHandbook_2"), (IPropertyPage) this);
    this.ReadConfigSettings();
  }

  private void On_pgConfigFileSettings_PropertyValueChanged(
    object s,
    PropertyValueChangedEventArgs e)
  {
    this.OnChanged();
  }

  private void On_searchSettingsCtrl_Changed(object sender, EventArgs e) => this.OnChanged();

  private void On_tc_SelectedIndexChanged(object sender, EventArgs e)
  {
    if (this._tc.SelectedIndex != 1)
      return;
    this._searchSettingsCtrl.LoadData();
  }

  public event EventHandler Changed;

  public PropertyPageType Type => PropertyPageType.Control;

  public object Control => (object) this;

  public string PageName => LocalizationHolder.rm.GetString("IMH_SystemSettings_PageName");

  public void Apply()
  {
    if (!this._modified)
      return;
    try
    {
      IMServerService service = ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService;
      IIMHSystemSettingsService customService1 = service.GetCustomService(typeof (IIMHSystemSettingsService)) as IIMHSystemSettingsService;
      IIMHIndexingService customService2 = service.GetCustomService(typeof (IIMHIndexingService)) as IIMHIndexingService;
      if (customService1 == null)
        throw new Exception(LocalizationHolder.rm.GetString("IMH_Save_Error_Msg"));
      Dictionary<string, string> dict;
      IMHCoatingsSystemSettings coatingsSettings;
      this._csCd.GetSettings(out dict, out coatingsSettings);
      if (dict.Count > 0)
        customService1.SaveSistemSettings(new IMHSystemSettings(dict, coatingsSettings, this._searchSettingsCtrl.Settings));
      if (customService2 == null)
        throw new Exception(LocalizationHolder.rm.GetString("IMH_IndexingService_NotFound"));
      if (!customService2.IsBusy)
      {
        if (this._searchSettingsCtrl.AddedAttributes.Count > 0 && this._searchSettingsCtrl.RemovedAttributes.Count > 0)
          this.CreateIndex(IndexesStatus.Changed, this._searchSettingsCtrl.AddedAttributes, this._searchSettingsCtrl.RemovedAttributes);
        else if (this._searchSettingsCtrl.AddedAttributes.Count > 0)
          this.CreateIndex(IndexesStatus.Added, this._searchSettingsCtrl.AddedAttributes, (Dictionary<string, List<Guid>>) null);
        else if (this._searchSettingsCtrl.RemovedAttributes.Count > 0)
          this.CreateIndex(IndexesStatus.Removed, (Dictionary<string, List<Guid>>) null, this._searchSettingsCtrl.RemovedAttributes);
        else
          this.CreateIndex(IndexesStatus.None, (Dictionary<string, List<Guid>>) null, (Dictionary<string, List<Guid>>) null);
      }
      else
      {
        string msg = customService2.Msg;
        throw new Exception(msg + (!string.IsNullOrEmpty(msg) ? ". " : string.Empty) + LocalizationHolder.rm.GetString("IMH_RollbackChanges"));
      }
    }
    catch (Exception ex)
    {
      ExceptionHelper.ExceptionService.ShowException(ex);
      this.Cancel();
    }
    finally
    {
      this._searchSettingsCtrl.ClearLists();
      this._modified = false;
    }
  }

  public void Cancel()
  {
    this.ReadConfigSettings();
    this._modified = false;
  }

  public string HelpTopicID => "-1";

  public string HeaderText
  {
    [DebuggerStepThrough] get => this.PageName;
  }

  public List<string> GetOptionNames()
  {
    return !(this.Control is System.Windows.Forms.Control control) ? new List<string>() : IPropertyPageHelper.GetOptionNames(control);
  }

  private void CreateIndex(
    IndexesStatus status,
    Dictionary<string, List<Guid>> addedAttrs,
    Dictionary<string, List<Guid>> removedAttrs)
  {
    IBackgroundTaskView service = ServiceUtils.GetService<IBackgroundTaskView>((object) ApplicationServices.Container, false);
    if (service == null)
      return;
    long objectIdByConstName = IMHHelper.GetObjectIDByConstName("ASSORTMENT_FOLDER_NAME");
    Dictionary<string, List<Guid>> addedIndexes = (Dictionary<string, List<Guid>>) null;
    if (addedAttrs != null)
    {
      addedIndexes = new Dictionary<string, List<Guid>>(addedAttrs.Count);
      foreach (KeyValuePair<string, List<Guid>> addedAttr in addedAttrs)
        addedIndexes.Add(addedAttr.Key, new List<Guid>((IEnumerable<Guid>) addedAttr.Value.ToArray()));
    }
    Dictionary<string, List<Guid>> removedIndexes = (Dictionary<string, List<Guid>>) null;
    if (removedAttrs != null)
    {
      removedIndexes = new Dictionary<string, List<Guid>>(removedAttrs.Count);
      foreach (KeyValuePair<string, List<Guid>> removedAttr in removedAttrs)
        removedIndexes.Add(removedAttr.Key, new List<Guid>((IEnumerable<Guid>) removedAttr.Value.ToArray()));
    }
    IMHIndexesHelper helper = new IMHIndexesHelper(objectIdByConstName, this._searchSettingsCtrl.NeedIndexMaterial, addedIndexes, removedIndexes)
    {
      Actions = status
    };
    service.AddTask((IBackgroundTask) new IMHIndexesBackgroundTask(helper));
  }

  private void ReadConfigSettings()
  {
    IMHSystemSettings systemSettings = (ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService).GetCustomService(typeof (IIMHSystemSettingsService)) is IIMHSystemSettingsService customService ? customService.GetSystemSettings() : (IMHSystemSettings) null;
    if (systemSettings == null)
      return;
    this._csCd = new ConfigSettingsCustomDescriptor(systemSettings.Dict, systemSettings.CoatingsSettings);
    this._pgConfigFileSettings.SelectedObject = (object) this._csCd;
    this._searchSettingsCtrl.Settings = systemSettings.AssortmentSearchSettings;
    this._searchSettingsCtrl.CancelChanged();
  }

  private void OnChanged()
  {
    this._modified = true;
    EventHandler changed = this.Changed;
    if (changed == null)
      return;
    changed((object) this, new EventArgs());
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (IMHSystemSettingsViewPage));
    this._tc = new TabControl();
    this._tpConfigFileSettings = new TabPage();
    this._pgConfigFileSettings = new PropertyGrid();
    this._tpAssortmentSearch = new TabPage();
    this._searchSettingsCtrl = new AssortmentSearchSettingsCtrl();
    this._tc.SuspendLayout();
    this._tpConfigFileSettings.SuspendLayout();
    this._tpAssortmentSearch.SuspendLayout();
    this.SuspendLayout();
    this._tc.Controls.Add((System.Windows.Forms.Control) this._tpConfigFileSettings);
    this._tc.Controls.Add((System.Windows.Forms.Control) this._tpAssortmentSearch);
    componentResourceManager.ApplyResources((object) this._tc, "_tc");
    this._tc.Name = "_tc";
    this._tc.SelectedIndex = 0;
    this._tc.SelectedIndexChanged += new EventHandler(this.On_tc_SelectedIndexChanged);
    this._tpConfigFileSettings.Controls.Add((System.Windows.Forms.Control) this._pgConfigFileSettings);
    componentResourceManager.ApplyResources((object) this._tpConfigFileSettings, "_tpConfigFileSettings");
    this._tpConfigFileSettings.Name = "_tpConfigFileSettings";
    this._tpConfigFileSettings.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this._pgConfigFileSettings, "_pgConfigFileSettings");
    this._pgConfigFileSettings.Name = "_pgConfigFileSettings";
    this._pgConfigFileSettings.PropertySort = PropertySort.Categorized;
    this._pgConfigFileSettings.ToolbarVisible = false;
    this._pgConfigFileSettings.PropertyValueChanged += new PropertyValueChangedEventHandler(this.On_pgConfigFileSettings_PropertyValueChanged);
    this._tpAssortmentSearch.BackColor = SystemColors.Control;
    this._tpAssortmentSearch.Controls.Add((System.Windows.Forms.Control) this._searchSettingsCtrl);
    componentResourceManager.ApplyResources((object) this._tpAssortmentSearch, "_tpAssortmentSearch");
    this._tpAssortmentSearch.Name = "_tpAssortmentSearch";
    componentResourceManager.ApplyResources((object) this._searchSettingsCtrl, "_searchSettingsCtrl");
    this._searchSettingsCtrl.Name = "_searchSettingsCtrl";
    this._searchSettingsCtrl.Settings = (List<IMHAssortmentClass>) componentResourceManager.GetObject("_searchSettingsCtrl.Settings");
    this._searchSettingsCtrl.Changed += new EventHandler(this.On_searchSettingsCtrl_Changed);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((System.Windows.Forms.Control) this._tc);
    this.DoubleBuffered = true;
    this.Name = nameof (IMHSystemSettingsViewPage);
    this._tc.ResumeLayout(false);
    this._tpConfigFileSettings.ResumeLayout(false);
    this._tpAssortmentSearch.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
