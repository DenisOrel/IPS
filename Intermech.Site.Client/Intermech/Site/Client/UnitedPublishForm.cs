// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.UnitedPublishForm
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using ImSSP;
using Intermech.Client.Core;
using Intermech.DataFormats;
using Intermech.Files;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.Localization;
using Intermech.Navigator;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.Site.Client;

public class UnitedPublishForm : Form
{
  private HybridDictionary _loadSettings;
  private ServiceContainer _services;
  private ObjectTypesFilterForm _objectTypesFilterForm;
  private RelationTypesFilterForm _relationTypesFilterForm;
  private List<long> _rootObjectIDs;
  private PleaseWaitFormManager _waitformManager;
  private ExtendedPublishOptions _saved;
  private PublishComposition _composition;
  private bool _compositionControlsChanged = true;
  private bool _optionsMode;
  private bool _selCheck;
  private List<long> _enableTrueTaskForSites;
  private bool _defaultEnableTrueTaskForSites;
  private IContainer components;
  private Button bOK;
  private Button bCancel;
  private iGCellStyle iGrid1Col0CellStyle;
  private iGColHdrStyle iGrid1Col0ColHdrStyle;
  private iGCellStyle iGrid1Col1CellStyle;
  private iGColHdrStyle iGrid1Col1ColHdrStyle;
  private iGCellStyle iGrid1Col2CellStyle;
  private iGColHdrStyle iGrid1Col2ColHdrStyle;
  private iGCellStyle iGrid1Col3CellStyle;
  private iGColHdrStyle iGrid1Col3ColHdrStyle;
  private iGCellStyle iGrid1DefaultCellStyle;
  private iGColHdrStyle iGrid1DefaultColHdrStyle;
  private iGCellStyle iGrid1RowTextColCellStyle;
  private GroupBox EnabledSitesBox;
  private CheckedListBox cbEnabledSites;
  private GroupBox groupBox2;
  private ComboBox cbOwners;
  private GroupBox groupBox3;
  private ComboBox cbComposition;
  private Button bObjectTypesFilter;
  private Button bRelationTypesFilter;
  private Button bObjectsList;
  private CheckBox cbAutoPublish;
  private CheckBox cbStartImmediately;
  private GroupBox PacketBox;
  private CheckBox cbMakeCheckList;
  private RichTextBox tbPackeNote;
  private Label label3;
  private Label label2;
  private Label label1;
  private TextBox tbPacketName;
  private Button bClassification;
  private TextBox tbPacketDesignation;
  private CheckBox cbMakePacket;
  private ObjectsListControl viewObjectsList;
  private ContextMenuStrip contextMenuStrip1;
  private ToolStripMenuItem miSettings;
  private CheckBox GiveOwnershipCheckBox;
  private Panel MidPanel;
  private Panel panel1;
  private Button bLoadOptions;
  private Button bSaveOptions;
  private OpenFileDialog openFileDialog1;
  private SaveFileDialog saveFileDialog1;
  private GroupBox groupBox1;
  private ComboBox cbPriority;

  public UnitedPublishForm()
  {
    this.InitializeComponent();
    this._services = new ServiceContainer();
    this._loadSettings = new HybridDictionary();
    this._services.AddService(typeof (IViewState), (object) new ViewStateService(ViewStateFlags.ReadOnly));
    this.viewObjectsList.StateStreamPrefix = "PublishObjectsList_";
    INamedImageList service = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
    this.contextMenuStrip1.ImageList = service.ImageList;
    this.miSettings.ImageIndex = service.ImageIndex("imgViewSettings");
  }

  public bool OptionsMode
  {
    get => this._optionsMode;
    set
    {
      if (value == this._optionsMode)
        return;
      this._optionsMode = value;
      this.PacketBox.Visible = !value;
      int height = this.Height + (value ? -1 : 1) * this.PacketBox.Height;
      if (this.MinimumSize.Height > height)
        this.MinimumSize = new Size(this.MinimumSize.Width, height);
      this.Height = height;
      this.cbOwners.Visible = !value;
      this.GiveOwnershipCheckBox.Visible = value;
      this.cbEnabledSites.Enabled = !value;
      this.cbAutoPublish.Visible = !value;
      this.cbStartImmediately.Visible = !value;
    }
  }

  private void InitializeData(List<Tuple<long, int>> objects, ExtendedPublishOptions options = null)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      bool isOffline = ((IPortalConnector) sessionKeeper.Session.GetCustomService(typeof (IPortalConnector))).IsOffline;
      List<SiteInfo> otherSites = this.GetOtherSites(sessionKeeper.Session);
      otherSites.Sort();
      this.InitializeObjectsList(sessionKeeper.Session, objects);
      if (options != null)
        this._saved = options;
      this._selCheck = true;
      try
      {
        this.InitializeEnabledSites(otherSites, options != null ? options.EnableSites : (this._loadSettings.Contains((object) SiteClientConsts.CfgUserEnableSites) ? Convert.ToString(this._loadSettings[(object) SiteClientConsts.CfgUserEnableSites]) : string.Empty));
      }
      finally
      {
        this._selCheck = false;
      }
      this.cbAutoPublish.Checked = options != null ? options.AutoReplication : this._loadSettings.Contains((object) SiteClientConsts.CfgAutoPublish) && Convert.ToBoolean(this._loadSettings[(object) SiteClientConsts.CfgAutoPublish]);
      IMSAttribute4ObjectType attribute4ObjectType = MetaDataHelper.GetAttribute4ObjectType(PortalConsts.objtypeUpdateTasks, PortalConsts.attributeTaskTransferEnabled);
      this._defaultEnableTrueTaskForSites = attribute4ObjectType == null || Convert.ToBoolean(attribute4ObjectType.DefaultValue);
      this.cbStartImmediately.Checked = this._defaultEnableTrueTaskForSites && this._loadSettings.Contains((object) SiteClientConsts.CfgStartImmediately) && Convert.ToBoolean(this._loadSettings[(object) SiteClientConsts.CfgStartImmediately]);
      this.InitializeTaskPriority(this._loadSettings.Contains((object) SiteClientConsts.CfgUserPriority) ? (TaskPriority) this._loadSettings[(object) SiteClientConsts.CfgUserPriority] : TaskPriority.Normal);
      this.InitializeComposition(options);
      if (options == null)
      {
        this.InitializeOwnerSites(otherSites);
      }
      else
      {
        CheckBox ownershipCheckBox = this.GiveOwnershipCheckBox;
        char? ownerSite = options.OwnerSite;
        int? nullable = ownerSite.HasValue ? new int?((int) ownerSite.GetValueOrDefault()) : new int?();
        int num1 = 89;
        int num2 = nullable.GetValueOrDefault() == num1 & nullable.HasValue ? 1 : 0;
        ownershipCheckBox.Checked = num2 != 0;
      }
      if (this.GiveOwnershipCheckBox.Visible)
        this.cbOwners.Enabled = this.GiveOwnershipCheckBox.Enabled = false;
      if (isOffline)
      {
        this.Text += " (Офлайн режим)";
        this.cbMakePacket.Checked = true;
        this.cbMakePacket.Enabled = false;
      }
      this._enableTrueTaskForSites = (sessionKeeper.Session.GetCustomService(typeof (IPublishRulesService)) as IPublishRulesService).EnableTrueTaskForSites;
      this.SetStartImmediately(-1, CheckState.Indeterminate);
    }
    this.MakePacket_CheckStateChanged((object) this, new EventArgs());
  }

  private void SetStartImmediately(int checkedIndex, CheckState newState)
  {
    if (this._selCheck || this._defaultEnableTrueTaskForSites)
      return;
    if (this._enableTrueTaskForSites != null && this._enableTrueTaskForSites.Count > 0)
    {
      bool flag = true;
      for (int index = 0; index < this.cbEnabledSites.Items.Count; ++index)
      {
        if ((index == checkedIndex ? (newState == CheckState.Checked ? 1 : 0) : (this.cbEnabledSites.GetItemChecked(index) ? 1 : 0)) != 0 && !this._enableTrueTaskForSites.Contains(((SiteInfo) this.cbEnabledSites.Items[index]).ID))
        {
          flag = false;
          break;
        }
      }
      this.cbStartImmediately.Enabled = flag;
    }
    else
      this.cbStartImmediately.Enabled = this._defaultEnableTrueTaskForSites;
    if (this.cbStartImmediately.Enabled)
      return;
    this.cbStartImmediately.Checked = false;
  }

  private void UnitedPublishForm_FormClosing(object sender, FormClosingEventArgs e)
  {
    HybridDictionary hybridDictionary = new HybridDictionary()
    {
      {
        (object) SiteClientConsts.CfgUserEnableSites,
        (object) this.SitesForUpdate
      },
      {
        (object) SiteClientConsts.CfgStartImmediately,
        (object) this.cbStartImmediately.Checked
      },
      {
        (object) SiteClientConsts.CfgAutoPublish,
        (object) this.cbAutoPublish.Checked
      },
      {
        (object) SiteClientConsts.CfgUserPriority,
        (object) ((UnitedPublishForm.PriorityItem) this.cbPriority.SelectedItem).Value
      }
    };
    if (this.OptionsMode)
      this.Height += this.PacketBox.Height;
    FormStorage.SaveLayout((Control) this, (IDictionary) hybridDictionary);
  }

  public static DialogResult ShowForm(List<Tuple<long, int>> items, ExtendedPublishOptions options = null)
  {
    using (UnitedPublishForm unitedPublishForm = new UnitedPublishForm())
    {
      FormStorage.LoadLayout((Control) unitedPublishForm, (IDictionary) unitedPublishForm._loadSettings);
      unitedPublishForm.OptionsMode = options != null;
      unitedPublishForm.InitializeData(items, options);
      DialogResult dialogResult = unitedPublishForm.ShowDialog();
      if (options != null && dialogResult == DialogResult.OK)
      {
        ExtendedPublishOptions currentOptions = unitedPublishForm.CurrentOptions;
        options.CountLevels = currentOptions.CountLevels;
        options.EnableTypes = currentOptions.EnableTypes;
        options.EnableRelationTypes = currentOptions.EnableRelationTypes;
        options.OwnerSite = currentOptions.OwnerSite;
        options.TaskPriority = currentOptions.TaskPriority;
      }
      return dialogResult;
    }
  }

  public static DialogResult ShowForm(ISelectedItems items)
  {
    using (UnitedPublishForm unitedPublishForm = new UnitedPublishForm())
    {
      FormStorage.LoadLayout((Control) unitedPublishForm, (IDictionary) unitedPublishForm._loadSettings);
      List<Tuple<long, int>> objects = new List<Tuple<long, int>>(items.Count);
      for (int index = 0; index < items.Count; ++index)
      {
        IDBTypedObjectID itemData = (IDBTypedObjectID) items.GetItemData(index, typeof (IDBTypedObjectID));
        objects.Add(new Tuple<long, int>(itemData.ObjectID, itemData.ObjectType));
      }
      unitedPublishForm.InitializeData(objects);
      return unitedPublishForm.ShowDialog();
    }
  }

  private void InitializeComposition(ExtendedPublishOptions options)
  {
    this.cbComposition.Items.Clear();
    int num = 0;
    if (options != null)
    {
      switch (options.CountLevels)
      {
        case -1:
          num = 0;
          break;
        case 0:
          num = 2;
          break;
        case 1:
          num = 1;
          break;
      }
    }
    foreach (CompositionType compositionType in Enum.GetValues(typeof (CompositionType)))
      this.cbComposition.Items.Add((object) EnumDescConverter.GetEnumDescription((Enum) compositionType));
    this.cbComposition.SelectedIndex = num;
  }

  private void InitializeObjectsList(IUserSession session, List<Tuple<long, int>> objects)
  {
    this._rootObjectIDs = new List<long>(objects.Count);
    Dictionary<int, List<long>> objectIDs = new Dictionary<int, List<long>>();
    IPublishTypesConfiguration customService = session.GetCustomService(typeof (IPublishTypesConfiguration)) as IPublishTypesConfiguration;
    foreach (Tuple<long, int> tuple in objects)
    {
      if (!customService.IsPublishObjectType(tuple.Item2))
        throw new Exception($"Публикация объектов типа \"{MetaDataHelper.GetObjectTypeName(tuple.Item2)}\" запрещена");
      if (!this._rootObjectIDs.Contains(tuple.Item1))
      {
        this._rootObjectIDs.Add(tuple.Item1);
        List<long> longList;
        if (!objectIDs.TryGetValue(tuple.Item2, out longList))
        {
          longList = new List<long>();
          objectIDs.Add(tuple.Item2, longList);
        }
        longList.Add(tuple.Item1);
      }
    }
    if (!this.OptionsMode && this._rootObjectIDs.Count == 1)
      this._saved = PublishOptionsHelper.Deserialize(session.GetObject(this._rootObjectIDs[0]));
    this.viewObjectsList.Initialize((IDescriptor) new ListPublishObjectsDescriptor(1, 0, "Выбранные объекты для публикации", objectIDs), (IServiceProvider) this._services);
    this.viewObjectsList.SetColumns(Utils.DefaultColumnsObjects(), false);
    this.viewObjectsList.Activate((IView) null);
  }

  private void InitializeTaskPriority(TaskPriority priority = TaskPriority.Normal)
  {
    this.cbPriority.Items.Clear();
    int num = 0;
    foreach (TaskPriority taskPriority in Enum.GetValues(typeof (TaskPriority)))
    {
      this.cbPriority.Items.Add((object) new UnitedPublishForm.PriorityItem(taskPriority));
      if (taskPriority.Equals((object) priority))
        num = this.cbPriority.Items.Count - 1;
    }
    this.cbPriority.SelectedIndex = num;
  }

  private void InitializeEnabledSites(List<SiteInfo> sites, string enableString)
  {
    this.cbEnabledSites.Items.Clear();
    Guid g = Guid.Empty;
    if (enableString.Length == 36)
      g = new Guid(enableString);
    foreach (SiteInfo site in sites)
      this.cbEnabledSites.Items.Add((object) SiteInfoItem.NewItem(site), enableString.Contains<char>(site.Code) || site.GUID.Equals(g));
  }

  private List<SiteInfo> GetOtherSites(IUserSession session)
  {
    List<SiteInfo> otherSites = new List<SiteInfo>();
    ISitesCacheService customService = (ISitesCacheService) session.GetCustomService(typeof (ISitesCacheService));
    foreach (SiteInfo site in customService.Sites)
    {
      if ((int) site.Code != (int) customService.Info.Code)
        otherSites.Add((SiteInfo) SiteInfoItem.NewItem(site));
    }
    return otherSites;
  }

  private void InitializeOwnerSites(List<SiteInfo> sites)
  {
    this.cbOwners.Items.Clear();
    this.cbOwners.Items.Add((object) new NotToGiveItem());
    foreach (SiteInfo site in sites)
      this.cbOwners.Items.Add((object) SiteInfoItem.NewItem(site));
    this.cbOwners.SelectedIndex = 0;
  }

  private void SetOwner(ExtendedPublishOptions options)
  {
    if (this.GiveOwnershipCheckBox.Visible)
    {
      CheckBox ownershipCheckBox = this.GiveOwnershipCheckBox;
      char? ownerSite = options.OwnerSite;
      int? nullable = ownerSite.HasValue ? new int?((int) ownerSite.GetValueOrDefault()) : new int?();
      int num1 = 89;
      int num2 = nullable.GetValueOrDefault() == num1 & nullable.HasValue ? 1 : 0;
      ownershipCheckBox.Checked = num2 != 0;
    }
    else
    {
      if (!options.OwnerSite.HasValue)
        return;
      foreach (object obj in this.cbOwners.Items)
      {
        if (obj is SiteInfoItem siteInfoItem && siteInfoItem.Code.Equals((object) options.OwnerSite))
        {
          this.cbOwners.SelectedItem = obj;
          break;
        }
      }
    }
  }

  private void Settings_Click(object sender, EventArgs e)
  {
    this.viewObjectsList.ChangeGridColumnsMenuButtonItem_Click(sender, e);
    this.viewObjectsList.DataLoaded = false;
    this.viewObjectsList.Activate((IView) null);
  }

  private void CreateObjectTypesFilterForm(ExtendedPublishOptions options)
  {
    this._objectTypesFilterForm = new ObjectTypesFilterForm();
    this._objectTypesFilterForm.LoadData(((ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService).GetCustomService(typeof (IPublishTypesConfiguration)) as IPublishTypesConfiguration).PublishObjectTypes, options?.EnableTypes, (List<int>) null, options != null ? options.AccessLevel : 0);
  }

  private void CreateRelationTypesFilterForm(ExtendedPublishOptions options)
  {
    this._relationTypesFilterForm = new RelationTypesFilterForm();
    this._relationTypesFilterForm.LoadData(options?.EnableRelationTypes);
  }

  private void ObjectTypesFilter_Click(object sender, EventArgs e)
  {
    if (this._objectTypesFilterForm == null)
      this.CreateObjectTypesFilterForm(this._saved);
    if (this._objectTypesFilterForm.ShowDialog() != DialogResult.OK)
      return;
    this._compositionControlsChanged = true;
  }

  private void RelationTypesFilter_Click(object sender, EventArgs e)
  {
    if (this._relationTypesFilterForm == null)
      this.CreateRelationTypesFilterForm(this._saved);
    if (this._relationTypesFilterForm.ShowDialog() != DialogResult.OK)
      return;
    this._compositionControlsChanged = true;
  }

  private List<int> EnableRelationTypes
  {
    get
    {
      List<int> publishRelationTypes = ((ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService).GetCustomService(typeof (IPublishTypesConfiguration)) as IPublishTypesConfiguration).PublishRelationTypes;
      if (this._relationTypesFilterForm != null)
      {
        List<int> filteredRelationTypes = this._relationTypesFilterForm.FilteredRelationTypes;
        if (filteredRelationTypes != null)
          return publishRelationTypes.Except<int>((IEnumerable<int>) filteredRelationTypes).ToList<int>();
      }
      return publishRelationTypes;
    }
  }

  private int AccessLevel
  {
    get => this._objectTypesFilterForm != null ? this._objectTypesFilterForm.AccessLevel : 0;
  }

  private List<int> EnableObjectTypes
  {
    get
    {
      IPublishTypesConfiguration customService = (ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService).GetCustomService(typeof (IPublishTypesConfiguration)) as IPublishTypesConfiguration;
      List<int> publishObjectTypes = customService.PublishObjectTypes;
      if (publishObjectTypes == null || publishObjectTypes.Count <= 0)
        return (List<int>) null;
      if (this._objectTypesFilterForm != null)
      {
        List<int> filteredObjectTypes = this._objectTypesFilterForm.FilteredObjectTypes;
        if (filteredObjectTypes != null)
          return publishObjectTypes.Except<int>((IEnumerable<int>) filteredObjectTypes).ToList<int>();
      }
      return customService.PublishObjectTypes;
    }
  }

  private void SelectComposition(bool throwCheckException)
  {
    if (this._composition != null && !this._compositionControlsChanged)
      return;
    this._waitformManager = new PleaseWaitFormManager();
    this._waitformManager.ShowForm();
    try
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        if (!(sessionKeeper.Session.GetCustomService(typeof (IPublishCompositionService)) is IPublishCompositionService customService))
          throw new Exception(LocalizationHolder.rm.GetString(sc_18649.ssp_webportal_18650()));
        Guid selectGUID = Guid.NewGuid();
        customService.Select(sessionKeeper.Session.SessionGUID, selectGUID, this._rootObjectIDs, this.CurrentOptions, PublishType.Simple, throwCheckException);
        this._composition = (PublishComposition) null;
        CompositionInfo info;
        for (info = customService.GetInfo(selectGUID); info != null && !info.ErrorPresent && info.Percent < 100; info = customService.GetInfo(selectGUID))
          Thread.Sleep(25);
        if (info.ErrorPresent)
          throw info.ErrorException;
        if (info.ErrorException != null)
        {
          int num = (int) MessageBox.Show(info.ErrorException.Message, "Ошибка проверки", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        if (info.Result == null)
          return;
        this._composition = info.Result as PublishComposition;
        this._compositionControlsChanged = false;
      }
    }
    finally
    {
      this._waitformManager.Close();
      this._waitformManager = (PleaseWaitFormManager) null;
    }
  }

  private ExtendedPublishOptions CurrentOptions
  {
    get
    {
      int countLevels = -1;
      switch (this.cbComposition.SelectedIndex)
      {
        case 0:
          countLevels = -1;
          break;
        case 1:
          countLevels = 1;
          break;
        case 2:
          countLevels = 0;
          break;
      }
      char? nullable = new char?();
      if (this.OptionsMode)
      {
        if (this.GiveOwnershipCheckBox.Checked)
          nullable = new char?('Y');
      }
      else if (this.cbOwners.SelectedItem is SiteInfoItem)
        nullable = new char?(((SiteInfo) this.cbOwners.SelectedItem).Code);
      List<int> enableTypes = this.OptionsMode ? this._objectTypesFilterForm?.FilteredObjectTypes : this.EnableObjectTypes;
      List<int> enableRelationTypes = this.OptionsMode ? this._relationTypesFilterForm?.FilteredRelationTypes : this.EnableRelationTypes;
      PublishCompositionOptions options = PublishCompositionOptions.WithLinkedObjects | PublishCompositionOptions.IncludeFreeChangeAttributes | PublishCompositionOptions.InfoRequired | PublishCompositionOptions.ForcedPublication;
      if (this.cbMakePacket.Checked)
        options |= PublishCompositionOptions.IncludeObjectsAlways;
      return new ExtendedPublishOptions(options, countLevels, enableRelationTypes, enableTypes, (FiltrationSettings) null, this.SitesForUpdate, this.cbAutoPublish.Checked, nullable, nullable, ((UnitedPublishForm.PriorityItem) this.cbPriority.SelectedItem).Value, this.AccessLevel);
    }
    set
    {
      switch (value.CountLevels)
      {
        case -1:
          this.cbComposition.SelectedIndex = 0;
          break;
        case 0:
          this.cbComposition.SelectedIndex = 2;
          break;
        case 1:
          this.cbComposition.SelectedIndex = 1;
          break;
      }
      for (int index = 0; index < this.cbEnabledSites.Items.Count; ++index)
      {
        SiteInfoItem siteInfoItem = (SiteInfoItem) this.cbEnabledSites.Items[index];
        this.cbEnabledSites.SetItemChecked(index, value.EnableSites.Contains<char>(siteInfoItem.Code));
      }
      this.cbAutoPublish.Checked = value.AutoReplication;
      IPublishTypesConfiguration customService = (ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService).GetCustomService(typeof (IPublishTypesConfiguration)) as IPublishTypesConfiguration;
      if (this._objectTypesFilterForm == null)
      {
        this.CreateObjectTypesFilterForm(value);
      }
      else
      {
        this._objectTypesFilterForm.AccessLevel = value.AccessLevel;
        this._objectTypesFilterForm.RefreshObjectTypesTree(customService.PublishObjectTypes, value?.EnableTypes, (List<int>) null);
      }
      if (this._relationTypesFilterForm == null)
        this.CreateRelationTypesFilterForm(value);
      else
        this._relationTypesFilterForm.SetEnabledRelationTypes(value.EnableRelationTypes);
      foreach (UnitedPublishForm.PriorityItem priorityItem in this.cbPriority.Items)
      {
        if (priorityItem.Value == value.TaskPriority)
        {
          this.cbPriority.SelectedItem = (object) priorityItem;
          break;
        }
      }
      this.SetOwner(value);
    }
  }

  private void ObjectsList_Click(object sender, EventArgs e)
  {
    this.SelectComposition(false);
    int num = (int) ObjectsList.ShowDialog(this._composition.Objects.Where<PublishCompositionObject>((Func<PublishCompositionObject, bool>) (x => PublishOptionsHelper.NormalPublish(x.Include))).ToList<PublishCompositionObject>());
  }

  private void EnabledSites_ItemCheck(object sender, ItemCheckEventArgs e)
  {
    this._compositionControlsChanged = true;
    this.SetStartImmediately(e.Index, e.NewValue);
  }

  private void MakePacket_CheckStateChanged(object sender, EventArgs e)
  {
    this.tbPacketDesignation.Enabled = this.bClassification.Enabled = this.tbPacketName.Enabled = this.tbPackeNote.Enabled = this.cbMakeCheckList.Enabled = this.cbMakePacket.Checked;
    if (!this.cbMakePacket.Checked || !(this.tbPacketDesignation.Text == string.Empty))
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBAttribute attributeByGuid = sessionKeeper.Session.GetObject(this._rootObjectIDs[0]).GetAttributeByGuid(new Guid("cad0001f-306c-11d8-b4e9-00304f19f545"));
      if (attributeByGuid == null)
        return;
      this.tbPacketDesignation.Text = attributeByGuid.AsString;
    }
  }

  public string SitesForUpdate
  {
    get
    {
      string empty = string.Empty;
      for (int index = 0; index < this.cbEnabledSites.Items.Count; ++index)
      {
        if (this.cbEnabledSites.GetItemChecked(index))
        {
          SiteInfoItem siteInfoItem = (SiteInfoItem) this.cbEnabledSites.Items[index];
          empty += siteInfoItem.Code.ToString();
        }
      }
      return empty;
    }
  }

  private void OK_Click(object sender, EventArgs e)
  {
    if (this._optionsMode)
    {
      this.DialogResult = DialogResult.OK;
    }
    else
    {
      this.DialogResult = DialogResult.None;
      if (string.IsNullOrEmpty(this.CurrentOptions.EnableSites))
      {
        int num = (int) MessageBox.Show("Не выбраны разрешенные узлы!", "Ошибка публикации", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.DialogResult = DialogResult.None;
      }
      else
      {
        this.SelectComposition(true);
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          IPortalTasksQueue customService = sessionKeeper.Session.GetCustomService(typeof (IPortalTasksQueue)) as IPortalTasksQueue;
          Packet4Publish packet = (Packet4Publish) null;
          if (this.cbMakePacket.Checked)
            packet = new Packet4Publish(this.tbPacketDesignation.Text, this.tbPacketName.Text, this.tbPackeNote.Text);
          Dictionary<int, bool> dictionary = new Dictionary<int, bool>(this._composition.Objects.Count);
          foreach (PublishCompositionObject compositionObject in this._composition.Objects)
          {
            bool flag;
            if (!dictionary.TryGetValue(compositionObject.ObjectType, out flag))
            {
              flag = ServiceUtils.GetService<IFileAttributeEditorService>((object) ApplicationServices.Container, true).HasFileAttribute(compositionObject.ObjectType);
              dictionary.Add(compositionObject.ObjectType, flag);
            }
            if (flag && ClientContext.FileVault.WorkArea.Save(compositionObject.ObjectID))
              ClientContext.FileVault.WorkArea.Unpublish(compositionObject.ObjectID);
          }
          ExtendedPublishOptions currentOptions = this.CurrentOptions;
          if (!this.cbMakePacket.Checked)
            currentOptions.CompositionOptions &= ~PublishCompositionOptions.ForcedPublication;
          long taskID = customService.PublishObjects(sessionKeeper.Session.SessionGUID, $"Публикация объектов пользователем {sessionKeeper.Session.UserName}", ((UnitedPublishForm.PriorityItem) this.cbPriority.SelectedItem).Value, this._composition, currentOptions, packet, this.cbMakeCheckList.Checked);
          if (this.cbStartImmediately.Checked)
          {
            if (taskID != 0L)
              customService.StartTask(taskID);
          }
        }
        this.DialogResult = DialogResult.OK;
      }
    }
  }

  private void Composition_SelectedIndexChanged(object sender, EventArgs e)
  {
    this._compositionControlsChanged = true;
  }

  private void SaveOptions_Click(object sender, EventArgs e)
  {
    ServicesManager.GetService<ISaveDiskPublishOptionsDialogService>().SaveOptions(this.CurrentOptions, true);
  }

  private void LoadOptions_Click(object sender, EventArgs e)
  {
    ExtendedPublishOptions extendedPublishOptions = ServicesManager.GetService<ISaveDiskPublishOptionsDialogService>().LoadOptions();
    if (extendedPublishOptions == null)
      return;
    this.CurrentOptions = extendedPublishOptions;
  }

  protected override void Dispose(bool disposing)
  {
    if (this._objectTypesFilterForm != null)
      this._objectTypesFilterForm.Dispose();
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    this.iGrid1Col0CellStyle = new iGCellStyle(true);
    this.iGrid1Col0ColHdrStyle = new iGColHdrStyle(true);
    this.iGrid1Col1CellStyle = new iGCellStyle(true);
    this.iGrid1Col1ColHdrStyle = new iGColHdrStyle(true);
    this.iGrid1Col2CellStyle = new iGCellStyle(true);
    this.iGrid1Col2ColHdrStyle = new iGColHdrStyle(true);
    this.iGrid1Col3CellStyle = new iGCellStyle(true);
    this.iGrid1Col3ColHdrStyle = new iGColHdrStyle(true);
    this.bOK = new Button();
    this.bCancel = new Button();
    this.iGrid1DefaultCellStyle = new iGCellStyle(true);
    this.iGrid1DefaultColHdrStyle = new iGColHdrStyle(true);
    this.iGrid1RowTextColCellStyle = new iGCellStyle(true);
    this.EnabledSitesBox = new GroupBox();
    this.cbEnabledSites = new CheckedListBox();
    this.groupBox2 = new GroupBox();
    this.GiveOwnershipCheckBox = new CheckBox();
    this.cbOwners = new ComboBox();
    this.groupBox3 = new GroupBox();
    this.cbComposition = new ComboBox();
    this.bObjectTypesFilter = new Button();
    this.bRelationTypesFilter = new Button();
    this.bObjectsList = new Button();
    this.cbAutoPublish = new CheckBox();
    this.cbStartImmediately = new CheckBox();
    this.PacketBox = new GroupBox();
    this.cbMakeCheckList = new CheckBox();
    this.tbPackeNote = new RichTextBox();
    this.label3 = new Label();
    this.label2 = new Label();
    this.label1 = new Label();
    this.tbPacketName = new TextBox();
    this.bClassification = new Button();
    this.tbPacketDesignation = new TextBox();
    this.cbMakePacket = new CheckBox();
    this.contextMenuStrip1 = new ContextMenuStrip(this.components);
    this.miSettings = new ToolStripMenuItem();
    this.MidPanel = new Panel();
    this.bLoadOptions = new Button();
    this.bSaveOptions = new Button();
    this.panel1 = new Panel();
    this.viewObjectsList = new ObjectsListControl();
    this.openFileDialog1 = new OpenFileDialog();
    this.saveFileDialog1 = new SaveFileDialog();
    this.groupBox1 = new GroupBox();
    this.cbPriority = new ComboBox();
    this.EnabledSitesBox.SuspendLayout();
    this.groupBox2.SuspendLayout();
    this.groupBox3.SuspendLayout();
    this.PacketBox.SuspendLayout();
    this.contextMenuStrip1.SuspendLayout();
    this.MidPanel.SuspendLayout();
    this.panel1.SuspendLayout();
    this.groupBox1.SuspendLayout();
    this.SuspendLayout();
    this.bOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bOK.DialogResult = DialogResult.OK;
    this.bOK.Location = new Point(342, 9);
    this.bOK.Name = "bOK";
    this.bOK.Size = new Size(121, 27);
    this.bOK.TabIndex = 0;
    this.bOK.Text = "ОК";
    this.bOK.UseVisualStyleBackColor = true;
    this.bOK.Click += new EventHandler(this.OK_Click);
    this.bCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Location = new Point(469, 9);
    this.bCancel.Name = "bCancel";
    this.bCancel.Size = new Size(121, 27);
    this.bCancel.TabIndex = 1;
    this.bCancel.Text = "Отмена";
    this.bCancel.UseVisualStyleBackColor = true;
    this.EnabledSitesBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.EnabledSitesBox.Controls.Add((Control) this.cbEnabledSites);
    this.EnabledSitesBox.Location = new Point(0, 12);
    this.EnabledSitesBox.Name = "EnabledSitesBox";
    this.EnabledSitesBox.Padding = new Padding(10, 7, 10, 10);
    this.EnabledSitesBox.Size = new Size(232, 263);
    this.EnabledSitesBox.TabIndex = 3;
    this.EnabledSitesBox.TabStop = false;
    this.EnabledSitesBox.Text = "Разрешенные узлы";
    this.cbEnabledSites.CheckOnClick = true;
    this.cbEnabledSites.Dock = DockStyle.Fill;
    this.cbEnabledSites.FormattingEnabled = true;
    this.cbEnabledSites.Location = new Point(10, 20);
    this.cbEnabledSites.Name = "cbEnabledSites";
    this.cbEnabledSites.Size = new Size(212, 233);
    this.cbEnabledSites.TabIndex = 0;
    this.cbEnabledSites.ItemCheck += new ItemCheckEventHandler(this.EnabledSites_ItemCheck);
    this.groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.groupBox2.Controls.Add((Control) this.GiveOwnershipCheckBox);
    this.groupBox2.Controls.Add((Control) this.cbOwners);
    this.groupBox2.Location = new Point(238, 12);
    this.groupBox2.Name = "groupBox2";
    this.groupBox2.Size = new Size(343, 55);
    this.groupBox2.TabIndex = 4;
    this.groupBox2.TabStop = false;
    this.groupBox2.Text = "Права владения";
    this.GiveOwnershipCheckBox.AutoSize = true;
    this.GiveOwnershipCheckBox.Location = new Point(17, 24);
    this.GiveOwnershipCheckBox.Name = "GiveOwnershipCheckBox";
    this.GiveOwnershipCheckBox.Size = new Size(171, 17);
    this.GiveOwnershipCheckBox.TabIndex = 1;
    this.GiveOwnershipCheckBox.Text = "Передавать права владения";
    this.GiveOwnershipCheckBox.UseVisualStyleBackColor = true;
    this.GiveOwnershipCheckBox.Visible = false;
    this.cbOwners.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbOwners.FormattingEnabled = true;
    this.cbOwners.Location = new Point(17, 22);
    this.cbOwners.Name = "cbOwners";
    this.cbOwners.Size = new Size(313, 21);
    this.cbOwners.TabIndex = 0;
    this.groupBox3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.groupBox3.Controls.Add((Control) this.cbComposition);
    this.groupBox3.Location = new Point(238, 75);
    this.groupBox3.Name = "groupBox3";
    this.groupBox3.Size = new Size(343, 55);
    this.groupBox3.TabIndex = 5;
    this.groupBox3.TabStop = false;
    this.groupBox3.Text = "Публикация состава";
    this.cbComposition.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbComposition.FormattingEnabled = true;
    this.cbComposition.Items.AddRange(new object[3]
    {
      (object) "Полный состав",
      (object) "Первый уровень",
      (object) "Без состава"
    });
    this.cbComposition.Location = new Point(17, 21);
    this.cbComposition.Name = "cbComposition";
    this.cbComposition.Size = new Size(313, 21);
    this.cbComposition.TabIndex = 0;
    this.cbComposition.SelectedIndexChanged += new EventHandler(this.Composition_SelectedIndexChanged);
    this.bObjectTypesFilter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.bObjectTypesFilter.Location = new Point(238, 138);
    this.bObjectTypesFilter.Name = "bObjectTypesFilter";
    this.bObjectTypesFilter.Size = new Size(110, 23);
    this.bObjectTypesFilter.TabIndex = 6;
    this.bObjectTypesFilter.Text = "Типы объектов";
    this.bObjectTypesFilter.UseVisualStyleBackColor = true;
    this.bObjectTypesFilter.Click += new EventHandler(this.ObjectTypesFilter_Click);
    this.bRelationTypesFilter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.bRelationTypesFilter.Location = new Point(354, 138);
    this.bRelationTypesFilter.Name = "bRelationTypesFilter";
    this.bRelationTypesFilter.Size = new Size(110, 23);
    this.bRelationTypesFilter.TabIndex = 7;
    this.bRelationTypesFilter.Text = "Типы связей";
    this.bRelationTypesFilter.UseVisualStyleBackColor = true;
    this.bRelationTypesFilter.Click += new EventHandler(this.RelationTypesFilter_Click);
    this.bObjectsList.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.bObjectsList.Location = new Point(471, 138);
    this.bObjectsList.Name = "bObjectsList";
    this.bObjectsList.Size = new Size(110, 23);
    this.bObjectsList.TabIndex = 8;
    this.bObjectsList.Text = "Полный список";
    this.bObjectsList.UseVisualStyleBackColor = true;
    this.bObjectsList.Click += new EventHandler(this.ObjectsList_Click);
    this.cbAutoPublish.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.cbAutoPublish.AutoSize = true;
    this.cbAutoPublish.Location = new Point(254, 168);
    this.cbAutoPublish.Name = "cbAutoPublish";
    this.cbAutoPublish.Size = new Size(172, 17);
    this.cbAutoPublish.TabIndex = 9;
    this.cbAutoPublish.Text = "Автопубликация обновлений";
    this.cbAutoPublish.UseVisualStyleBackColor = true;
    this.cbStartImmediately.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.cbStartImmediately.AutoSize = true;
    this.cbStartImmediately.Location = new Point(254, 193);
    this.cbStartImmediately.Name = "cbStartImmediately";
    this.cbStartImmediately.Size = new Size(174, 17);
    this.cbStartImmediately.TabIndex = 10;
    this.cbStartImmediately.Text = "Запустить публикацию сразу";
    this.cbStartImmediately.UseVisualStyleBackColor = true;
    this.PacketBox.BackColor = SystemColors.Control;
    this.PacketBox.Controls.Add((Control) this.cbMakeCheckList);
    this.PacketBox.Controls.Add((Control) this.tbPackeNote);
    this.PacketBox.Controls.Add((Control) this.label3);
    this.PacketBox.Controls.Add((Control) this.label2);
    this.PacketBox.Controls.Add((Control) this.label1);
    this.PacketBox.Controls.Add((Control) this.tbPacketName);
    this.PacketBox.Controls.Add((Control) this.bClassification);
    this.PacketBox.Controls.Add((Control) this.tbPacketDesignation);
    this.PacketBox.Controls.Add((Control) this.cbMakePacket);
    this.PacketBox.Dock = DockStyle.Bottom;
    this.PacketBox.Location = new Point(10, 365);
    this.PacketBox.Name = "PacketBox";
    this.PacketBox.Size = new Size(590, 161);
    this.PacketBox.TabIndex = 11;
    this.PacketBox.TabStop = false;
    this.cbMakeCheckList.AutoSize = true;
    this.cbMakeCheckList.Location = new Point(363, 19);
    this.cbMakeCheckList.Name = "cbMakeCheckList";
    this.cbMakeCheckList.Size = new Size(156, 17);
    this.cbMakeCheckList.TabIndex = 8;
    this.cbMakeCheckList.Text = "Формировать квитанцию";
    this.cbMakeCheckList.UseVisualStyleBackColor = true;
    this.tbPackeNote.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.tbPackeNote.Location = new Point(10, 105);
    this.tbPackeNote.Name = "tbPackeNote";
    this.tbPackeNote.Size = new Size(571, 42);
    this.tbPackeNote.TabIndex = 7;
    this.tbPackeNote.Text = "";
    this.label3.AutoSize = true;
    this.label3.Location = new Point(7, 89);
    this.label3.Name = "label3";
    this.label3.Size = new Size(115, 13);
    this.label3.TabIndex = 6;
    this.label3.Text = "Комментарий пакета";
    this.label2.AutoSize = true;
    this.label2.Location = new Point(360, 43);
    this.label2.Name = "label2";
    this.label2.Size = new Size(121, 13);
    this.label2.TabIndex = 5;
    this.label2.Text = "Наименование пакета";
    this.label1.AutoSize = true;
    this.label1.Location = new Point(7, 43);
    this.label1.Name = "label1";
    this.label1.Size = new Size(112 /*0x70*/, 13);
    this.label1.TabIndex = 4;
    this.label1.Text = "Обозначение пакета";
    this.tbPacketName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.tbPacketName.Location = new Point(363, 59);
    this.tbPacketName.Name = "tbPacketName";
    this.tbPacketName.Size = new Size(214, 20);
    this.tbPacketName.TabIndex = 3;
    this.bClassification.Image = (Image) Intermech.Site.Client.Properties.Resources.Classify;
    this.bClassification.Location = new Point(316, 58);
    this.bClassification.Name = "bClassification";
    this.bClassification.Size = new Size(27, 23);
    this.bClassification.TabIndex = 2;
    this.bClassification.TabStop = false;
    this.bClassification.UseVisualStyleBackColor = true;
    this.tbPacketDesignation.Location = new Point(10, 59);
    this.tbPacketDesignation.Name = "tbPacketDesignation";
    this.tbPacketDesignation.Size = new Size(298, 20);
    this.tbPacketDesignation.TabIndex = 1;
    this.cbMakePacket.AutoSize = true;
    this.cbMakePacket.Location = new Point(10, 19);
    this.cbMakePacket.Name = "cbMakePacket";
    this.cbMakePacket.Size = new Size(134, 17);
    this.cbMakePacket.TabIndex = 0;
    this.cbMakePacket.Text = "Сформировать пакет";
    this.cbMakePacket.UseVisualStyleBackColor = true;
    this.cbMakePacket.CheckStateChanged += new EventHandler(this.MakePacket_CheckStateChanged);
    this.contextMenuStrip1.Items.AddRange(new ToolStripItem[1]
    {
      (ToolStripItem) this.miSettings
    });
    this.contextMenuStrip1.Name = "contextMenuStrip1";
    this.contextMenuStrip1.Size = new Size(223, 26);
    this.miSettings.Name = "miSettings";
    this.miSettings.Size = new Size(222, 22);
    this.miSettings.Text = "Настройка отображения ...";
    this.miSettings.Click += new EventHandler(this.Settings_Click);
    this.MidPanel.Controls.Add((Control) this.groupBox1);
    this.MidPanel.Controls.Add((Control) this.bLoadOptions);
    this.MidPanel.Controls.Add((Control) this.bSaveOptions);
    this.MidPanel.Controls.Add((Control) this.EnabledSitesBox);
    this.MidPanel.Controls.Add((Control) this.groupBox2);
    this.MidPanel.Controls.Add((Control) this.groupBox3);
    this.MidPanel.Controls.Add((Control) this.bObjectTypesFilter);
    this.MidPanel.Controls.Add((Control) this.cbStartImmediately);
    this.MidPanel.Controls.Add((Control) this.bRelationTypesFilter);
    this.MidPanel.Controls.Add((Control) this.bObjectsList);
    this.MidPanel.Controls.Add((Control) this.cbAutoPublish);
    this.MidPanel.Dock = DockStyle.Bottom;
    this.MidPanel.Location = new Point(10, 85);
    this.MidPanel.Name = "MidPanel";
    this.MidPanel.Size = new Size(590, 280);
    this.MidPanel.TabIndex = 13;
    this.bLoadOptions.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.bLoadOptions.Location = new Point(471, 193);
    this.bLoadOptions.Name = "bLoadOptions";
    this.bLoadOptions.Size = new Size(110, 23);
    this.bLoadOptions.TabIndex = 12;
    this.bLoadOptions.Text = "Загрузить настройки публикации";
    this.bLoadOptions.UseVisualStyleBackColor = true;
    this.bLoadOptions.Click += new EventHandler(this.LoadOptions_Click);
    this.bSaveOptions.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.bSaveOptions.Location = new Point(471, 167);
    this.bSaveOptions.Name = "bSaveOptions";
    this.bSaveOptions.Size = new Size(110, 23);
    this.bSaveOptions.TabIndex = 11;
    this.bSaveOptions.Text = "Сохранить";
    this.bSaveOptions.UseVisualStyleBackColor = true;
    this.bSaveOptions.Click += new EventHandler(this.SaveOptions_Click);
    this.panel1.Controls.Add((Control) this.bCancel);
    this.panel1.Controls.Add((Control) this.bOK);
    this.panel1.Dock = DockStyle.Bottom;
    this.panel1.Location = new Point(10, 526);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(590, 39);
    this.panel1.TabIndex = 14;
    this.viewObjectsList.AllowCustomGroupValues = true;
    this.viewObjectsList.ContextMenuStrip = this.contextMenuStrip1;
    this.viewObjectsList.Control = (object) this.viewObjectsList;
    this.viewObjectsList.DataLoaded = false;
    this.viewObjectsList.DisableColumnsGrouping = true;
    this.viewObjectsList.DisableGroupBox = true;
    this.viewObjectsList.DisableIMContextMenu = true;
    this.viewObjectsList.DisableKeyDownEvents = false;
    this.viewObjectsList.DisableStatusBar = true;
    this.viewObjectsList.DisableToolBar = true;
    this.viewObjectsList.Dock = DockStyle.Fill;
    this.viewObjectsList.EmbeddedFocusAndSelection = (iFocusAndSelection) null;
    this.viewObjectsList.Font = new Font("Tahoma", 8.25f);
    this.viewObjectsList.Location = new Point(10, 10);
    this.viewObjectsList.Name = "viewObjectsList";
    this.viewObjectsList.Size = new Size(590, 75);
    this.viewObjectsList.TabIndex = 12;
    this.viewObjectsList.ViewContentType = ContentType.Folders | ContentType.NonFolders;
    this.openFileDialog1.DefaultExt = "po";
    this.openFileDialog1.RestoreDirectory = true;
    this.saveFileDialog1.DefaultExt = "po";
    this.saveFileDialog1.FileName = "options1";
    this.saveFileDialog1.Filter = "Файлы с настройками публикации|*.po|Все файлы|*.*";
    this.saveFileDialog1.RestoreDirectory = true;
    this.groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.groupBox1.Controls.Add((Control) this.cbPriority);
    this.groupBox1.Location = new Point(238, 220);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(224 /*0xE0*/, 55);
    this.groupBox1.TabIndex = 13;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "Приоритет задачи";
    this.cbPriority.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbPriority.FormattingEnabled = true;
    this.cbPriority.Items.AddRange(new object[3]
    {
      (object) "Полный состав",
      (object) "Первый уровень",
      (object) "Без состава"
    });
    this.cbPriority.Location = new Point(17, 21);
    this.cbPriority.Name = "cbPriority";
    this.cbPriority.Size = new Size(190, 21);
    this.cbPriority.TabIndex = 0;
    this.AcceptButton = (IButtonControl) this.bOK;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(610, 575);
    this.Controls.Add((Control) this.viewObjectsList);
    this.Controls.Add((Control) this.MidPanel);
    this.Controls.Add((Control) this.PacketBox);
    this.Controls.Add((Control) this.panel1);
    this.MinimumSize = new Size(625, 390);
    this.Name = nameof (UnitedPublishForm);
    this.Padding = new Padding(10);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Параметры публикации";
    this.FormClosing += new FormClosingEventHandler(this.UnitedPublishForm_FormClosing);
    this.EnabledSitesBox.ResumeLayout(false);
    this.groupBox2.ResumeLayout(false);
    this.groupBox2.PerformLayout();
    this.groupBox3.ResumeLayout(false);
    this.PacketBox.ResumeLayout(false);
    this.PacketBox.PerformLayout();
    this.contextMenuStrip1.ResumeLayout(false);
    this.MidPanel.ResumeLayout(false);
    this.MidPanel.PerformLayout();
    this.panel1.ResumeLayout(false);
    this.groupBox1.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  private class PriorityItem
  {
    public TaskPriority Value { get; private set; }

    public PriorityItem(TaskPriority value) => this.Value = value;

    public override string ToString() => EnumDescConverter.GetEnumDescription((Enum) this.Value);
  }
}
