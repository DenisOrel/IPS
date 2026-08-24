// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.CompositionOptionsWizard
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Infralution.Controls.VirtualTree;
using Intermech.Bars;
using Intermech.Client.Core;
using Intermech.Client.Core.FormDesigner.Controls;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Contexts;
using Intermech.Interfaces.MRP;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Navigator;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Search.Mrp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace Intermech.MRP2;

public class CompositionOptionsWizard : Form
{
  private Stack<Panel> PreviosSteps = new Stack<Panel>();
  private Panel CurrentStep;
  private Panel[] Steps = new Panel[3];
  private long _newObjectType;
  private long _ObjectID;
  public FiltrationSettings FiltrationSettings = new FiltrationSettings(Guid.NewGuid());
  private ServiceContainer _services;
  private ICurrentUserAndRole _currentUserAndRole;
  private IMRPSettings _mrpSettings;
  private CompositionOptionsWizard.MOClientPluginsDataTransfer _moClientPluginsDataTransfer;
  private bool _useEvents;
  private List<long> _selectedContexts = new List<long>();
  private MyAttributeMetadata _contextAttr = new MyAttributeMetadata();
  /// <summary>
  /// Сервис, позволяющий клиентским плагинам передавать какую-о информацию на сторону сервера
  /// </summary>
  private IClientPluginsService _clientPluginsService;
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private Button btnNext;
  private Button btnCancel;
  private Button btnBack;
  private GroupBox groupBox1;
  private PictureBox picture;
  private TextBox edCount;
  private Label lbBought;
  internal Panel pageSupplyMethod;
  internal Panel pageVersionRule;
  internal Panel pageCount;
  private Panel panel2;
  private SplitContainer splitContainer1;
  private Panel panel3;
  private PictureBox pictureContext;
  private LinkLabel _selectEditingContextLinkLabel;
  private Button _clearEditingContextButton;
  private Button _setCurrentEditingContextButton;
  private Button _selectEditingContextButton;
  private Panel panel1;
  private PictureBox pictureRule;
  private LinkLabel _changeVersionsRuleLinkLabel;
  private Button _setDefaultVersionsRuleButton;
  private Button _setCurrentVersionsRuleButton;
  private Button _changeVersionsRuleButton;
  private IMGroupBox imGroupBox1;
  private SeriesDatesSelectingControl seriesDatesSelectingControl;
  private ContextMenuStrip contextMenuStrip1;
  private ToolStripMenuItem toolStripMenuItem1;
  internal NavigatorTreeViewWithObjectTypeFiltration navigatorTreeView1;
  private Panel panel4;
  private Label lbPromt;
  private CheckedListBox cbContexts;

  public MeasuredValue Count
  {
    get
    {
      return MeasureHelper.ConvertToMeasuredValue(this.edCount.Text, "шт", false) ?? new MeasuredValue(1.0, PDMPluginIDs.measureShtuk);
    }
    set => this.edCount.Text = value.ToString();
  }

  public CompositionOptionsWizard() => this.InitializeComponent();

  private void CompositionOptionsDialog_Load(object sender, EventArgs e)
  {
    this.SetupCollumns();
    FormStorage.LoadLayout((Control) this);
    INamedImageList service = ServiceUtils.GetService<INamedImageList>((object) ServicesManager.ServiceContainer, false);
    ButtonItem buttonItem1 = new ButtonItem();
    buttonItem1.CommandName = "";
    buttonItem1.ShowText = false;
    buttonItem1.ToolTipText = LocalizationHolder.rm.GetString("btnDocFilterHint");
    buttonItem1.ImageIndex = service.ImageIndex("imgNewItem");
    buttonItem1.BeginGroup = false;
    buttonItem1.AutoToggle = AutoToggleType.Single;
    ButtonItem buttonItem2 = buttonItem1;
    buttonItem2.Click += new EventHandler(this.btnDocFilterClick);
    buttonItem2.Checked = !PLRelationsNode.ShowDocuments;
    this.navigatorTreeView1.TreeToolbar.Items.Insert(0, (ToolbarItemBase) buttonItem2);
  }

  private void btnDocFilterClick(object sender, EventArgs e)
  {
    PLRelationsNode.ShowDocuments = !(sender as ButtonItem).Checked;
    this.UpdateRulePage();
  }

  private void SetupCollumns()
  {
    NodeColumnCollection nodeColumnCollection = Intermech.Navigator.Utils.CaptionAndStatesesColumns(NodeColumnSortOrder.Ascending);
    nodeColumnCollection[0].Width = 250;
    nodeColumnCollection[1].Width = 50;
    int columnID = 0;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      columnID = sessionKeeper.Session.IdentHelper.GetAttributeID("cad00267-306c-11d8-b4e9-00304f19f545");
    IColumnSchemes service = (IColumnSchemes) ServicesManager.GetService(typeof (IColumnSchemes));
    NodeColumn column1 = service.CreateColumn(Intermech.Navigator.Consts.RelationColumnSchemeGuid, (object) columnID);
    column1.SortOrder = NodeColumnSortOrder.None;
    nodeColumnCollection.Add(column1, 50);
    NodeColumn column2 = service.CreateColumn(Intermech.Navigator.Consts.ObjectObligatoryColumnSchemeGuid, (object) ObligatoryObjectAttributes.F_VERSION_ID);
    column2.SortOrder = NodeColumnSortOrder.None;
    nodeColumnCollection.Add(column2, 50);
    this.navigatorTreeView1.TreeView.SetColumns(nodeColumnCollection);
  }

  private bool IsLastStep() => this.Steps[this.Steps.Length - 1] == this.CurrentStep;

  private void UpdateControls()
  {
    for (int index = 0; index < this.Steps.Length; ++index)
    {
      this.Steps[index].Visible = this.Steps[index] == this.CurrentStep;
      this.Steps[index].Dock = DockStyle.Fill;
    }
    this.btnBack.Enabled = this.PreviosSteps.Count > 0;
    if (this.IsLastStep())
    {
      this.btnNext.Text = "Гото&во";
      this.btnNext.DialogResult = DialogResult.OK;
    }
    else
    {
      this.btnNext.Text = "&Далее";
      this.btnNext.DialogResult = DialogResult.None;
    }
    if (this.CurrentStep != this.pageVersionRule)
      return;
    this.UpdateRulePage();
  }

  private void UpdateRulePage()
  {
    if (this.CurrentStep != this.pageVersionRule)
      return;
    if (this.FiltrationSettings.CurrentRule == null)
      this._changeVersionsRuleLinkLabel.Text = "[Выберите правило подбора версий]";
    else
      this._changeVersionsRuleLinkLabel.Text = $"[{this.FiltrationSettings.CurrentRule.RuleObjectCaption}]";
    this.SetupFiltrationSettings();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(this.FiltrationSettings.EditingContext.ContextID);
      this._selectEditingContextLinkLabel.Text = !objectInfo.Empty ? $"[{objectInfo.Caption}]" : "[Выберите контекст редактирования]";
      IDBObject dbObject = sessionKeeper.Session.GetObject(this._ObjectID);
      this.navigatorTreeView1.TreeView.Build((IDescriptor) new PLRelationsDescriptor(Intermech.Navigator.Consts.CategoryAdvRelationsNode, 0, this.FiltrationSettings.OwnerID, this._selectedContexts, this._ObjectID, dbObject.ObjectType, MRP2Consts.reltypeIdSP, string.Empty, 0L, dbObject.OwnerID, 0L, 0, (List<int>) null, (long) dbObject.VersionID, dbObject.IsBaseVersion ? 1L : 0L));
    }
  }

  private void btnNext_Click(object sender, EventArgs e) => this.NextStep();

  private void NextStep()
  {
    if (this.CurrentStep == null)
    {
      this.CurrentStep = this._newObjectType != (long) MRP2Consts.objtypeIdExitAssembly ? this.pageVersionRule : this.pageSupplyMethod;
    }
    else
    {
      Panel currentStep = this.CurrentStep;
      if (this.CurrentStep == this.pageSupplyMethod)
      {
        MRP2Consts.ArticleSupplyMethod? articleSupplyMethod1 = MRP2Consts.StringToArticleSupplyMethod(this.GetSupplyMethod());
        MRP2Consts.ArticleSupplyMethod articleSupplyMethod2 = MRP2Consts.ArticleSupplyMethod.Production;
        this.CurrentStep = !(articleSupplyMethod1.GetValueOrDefault() == articleSupplyMethod2 & articleSupplyMethod1.HasValue) ? this.pageCount : this.pageVersionRule;
      }
      else if (this.CurrentStep == this.pageVersionRule)
        this.CurrentStep = this.pageCount;
      this.PreviosSteps.Push(currentStep);
    }
    this.UpdateControls();
  }

  private void PrevStep()
  {
    if (this.PreviosSteps.Count <= 0)
      return;
    this.CurrentStep = this.PreviosSteps.Pop();
    this.UpdateControls();
  }

  private void btnBack_Click(object sender, EventArgs e) => this.PrevStep();

  internal string GetSupplyMethod()
  {
    foreach (Control control in (ArrangedElementCollection) this.groupBox1.Controls)
    {
      if (control is RadioButton && (control as RadioButton).Checked)
        return (control as RadioButton).Text;
    }
    return "";
  }

  private void _changeVersionsRuleButton_Click(object sender, EventArgs e)
  {
    this.SelectVersionRule();
  }

  private void SelectVersionRule()
  {
    long[] numArray = VersionRulesSelectionForm.Execute(VersionRulesSelectFilter.vrfExcludeAllVersionsRule, false, "Выберите правило подбора версий для формирования состава");
    if (numArray == null || numArray.Length == 0)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this.FiltrationSettings.CurrentRule = (sessionKeeper.Session.GetCustomService(typeof (IVersionRulesCacheService)) as IVersionRulesCacheService)[(object) sessionKeeper.Session.SessionGUID, numArray[0]];
    this.UpdateRulePage();
  }

  private void _setCurrentVersionsRuleButton_Click(object sender, EventArgs e)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IFiltrationService service = ServicesManager.GetService(typeof (IFiltrationService)) as IFiltrationService;
      this.FiltrationSettings.CurrentRule = (sessionKeeper.Session.GetCustomService(typeof (IVersionRulesCacheService)) as IVersionRulesCacheService)[(object) sessionKeeper.Session.SessionGUID, service.FiltrationRuleID];
    }
    this.UpdateRulePage();
  }

  private void _setDefaultVersionsRuleButton_Click(object sender, EventArgs e)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this.FiltrationSettings.CurrentRule = (sessionKeeper.Session.GetCustomService(typeof (IVersionRulesCacheService)) as IVersionRulesCacheService).GetDefaultVersionRule(sessionKeeper.Session.SessionGUID);
    this.UpdateRulePage();
  }

  private void _changeVersionsRuleLinkLabel_LinkClicked(
    object sender,
    LinkLabelLinkClickedEventArgs e)
  {
    this.SelectVersionRule();
  }

  private void Init(long objectType, long ObjectID)
  {
    this._services = new ServiceContainer();
    this._services.AddService(typeof (IViewState), (object) new ViewStateService());
    this.navigatorTreeView1.Services = (IServiceProvider) this._services;
    this.navigatorTreeView1.TreeView.Services = (IServiceProvider) this._services;
    this._currentUserAndRole = ServicesManager.GetService(typeof (ICurrentUserAndRole)) as ICurrentUserAndRole;
    this._mrpSettings = ServicesManager.GetService(typeof (IMRPSettings)) as IMRPSettings;
    this._moClientPluginsDataTransfer = new CompositionOptionsWizard.MOClientPluginsDataTransfer(this);
    this._clientPluginsService = ServicesManager.GetService(typeof (IClientPluginsService)) as IClientPluginsService;
    if (this._currentUserAndRole != null)
    {
      this._useEvents = this._currentUserAndRole.UseRuleEvents;
      this._currentUserAndRole.UseRuleEvents = true;
    }
    this.Steps[0] = this.pageSupplyMethod;
    this.Steps[1] = this.pageVersionRule;
    this.Steps[2] = this.pageCount;
    this.CurrentStep = (Panel) null;
    this._newObjectType = objectType;
    this._ObjectID = ObjectID;
    RadioGroupDialog.FillGroupBoxControl(this.groupBox1, MRP2Consts.SupplyMethods);
    this._setCurrentVersionsRuleButton_Click((object) null, (EventArgs) null);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(ObjectID);
      this.Text = "Выберите параметры объекта";
      if (!objectInfo.Empty)
        this.Text = $"{this.Text} {objectInfo.Caption}";
      this.seriesDatesSelectingControl.Visible = sessionKeeper.Session.EnabledSeriesDates;
    }
    if ((ServicesManager.GetService(typeof (IFiltrationService)) as IFiltrationService).Filtration.Tags[(object) "{AB419A02-DE8A-4A8E-905A-D782F5B720E5}"] is List<long> tag)
      this._selectedContexts.AddRange((IEnumerable<long>) tag);
    this._contextAttr.SetByGUID("cad00651-306c-11d8-b4e9-00304f19f545");
    this.cbContexts.Items.Clear();
    if (this._contextAttr.AttrPossibleValues != null)
    {
      for (int index = 0; index < this._contextAttr.AttrPossibleValues.Count; ++index)
      {
        MyElement attrPossibleValue = this._contextAttr.AttrPossibleValues[index] as MyElement;
        this.cbContexts.Items.Add((object) attrPossibleValue, this._selectedContexts.Contains(Convert.ToInt64(attrPossibleValue.Value)));
      }
    }
    this.NextStep();
  }

  public DialogResult Execute(long objectType, long ObjectID)
  {
    this.Init(objectType, ObjectID);
    return this.ShowDialog();
  }

  private void SelectEditingContextButton_Click(object sender, EventArgs e)
  {
    DescriptorCollection descriptors = new DescriptorCollection();
    List<int> contextTopObjectsIds = MetaDataHelper.GetEditingContextTopObjectsIDs();
    for (int index = 0; index < contextTopObjectsIds.Count; ++index)
      descriptors.Add((IDescriptor) new Intermech.Navigator.DBObjectTypes.Descriptor(contextTopObjectsIds[index]));
    long[] numArray = Intermech.Navigator.SelectionWindow.SelectObjects("Выберите контекст редактирования", "Выберите контекст редактирования", (IDescriptor) new Intermech.Navigator.CustomNode.Descriptor("Контексты редактирования", descriptors), SelectionOptions.Default | SelectionOptions.DisableSelectAbstractTypes | SelectionOptions.DisableMultiselect);
    if (numArray == null || numArray.Length == 0)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBEditingContextsObject editingContextsObject = sessionKeeper.Session.GetObject(numArray[0]) as IDBEditingContextsObject;
      this.FiltrationSettings.EditingContext = new CurrentEditingContext(numArray[0], editingContextsObject.LinkedContextNumber, EditingContextMode.Default);
    }
    this.UpdateRulePage();
  }

  private void ClearEditingContextButton_Click(object sender, EventArgs e)
  {
    this.FiltrationSettings.EditingContext = CurrentEditingContext.Empty;
    this.UpdateRulePage();
  }

  private void SetCurrentEditingContextButton_Click(object sender, EventArgs e)
  {
    if (this.FiltrationSettings.EditingContext.ContextID == this._currentUserAndRole.CachedEditingContextID)
      return;
    if (this._currentUserAndRole.CachedEditingContextID == 0L)
    {
      this.ClearEditingContextButton_Click(sender, e);
    }
    else
    {
      this.FiltrationSettings.EditingContext = new CurrentEditingContext(this._currentUserAndRole.CachedEditingContextID, this._currentUserAndRole.CachedEditingContextModificationID, EditingContextMode.Default);
      this.UpdateRulePage();
    }
  }

  private void _selectEditingContextLinkLabel_LinkClicked(
    object sender,
    LinkLabelLinkClickedEventArgs e)
  {
    this.SelectEditingContextButton_Click(sender, (EventArgs) e);
  }

  private void CompositionOptionsWizard_FormClosed(object sender, FormClosedEventArgs e)
  {
    FormStorage.SaveLayout((Control) this);
  }

  public void SetupFiltrationSettings()
  {
    this.FiltrationSettings.Tags[(object) "{76094280-391F-44AC-8B7B-9B6DEA501110}"] = (object) this.FiltrationSettings.EditingContext;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      (sessionKeeper.Session.GetCustomService(typeof (IVersionRulesCacheService)) as IVersionRulesCacheService).SetFiltrationSettings((object) sessionKeeper.Session.SessionGUID, this.FiltrationSettings.OwnerID, this.FiltrationSettings);
    this._clientPluginsService.RegisterClientPlugin(this._moClientPluginsDataTransfer.PluginGuid, (IClientPluginsDataTransfer) this._moClientPluginsDataTransfer);
  }

  public void RemoveFiltrationSettings()
  {
    if (this._currentUserAndRole != null)
      this._currentUserAndRole.UseRuleEvents = this._useEvents;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      (sessionKeeper.Session.GetCustomService(typeof (IVersionRulesCacheService)) as IVersionRulesCacheService).SetFiltrationSettings((object) sessionKeeper.Session.SessionGUID, this.FiltrationSettings.OwnerID, (FiltrationSettings) null);
    if (this._moClientPluginsDataTransfer != null)
    {
      this._clientPluginsService.UnregisterClientPlugin(this._moClientPluginsDataTransfer.PluginGuid);
      if (ServicesManager.GetService(typeof (IFiltrationService)) is IFiltrationService service && service.Filtration.Tags != null)
      {
        service.Filtration.Tags.Remove((object) "{E2390B62-E0BA-4F7E-89CC-1E9E33F0BB5C}");
        service.Filtration.Tags.Remove((object) "{529FFE92-FDA7-48B8-AADF-ADB1EE6EF584}");
        service.Filtration.Tags.Remove((object) "{7196FEC5-A048-4118-AF15-73BEEAA63A87}");
        service.Filtration.Tags.Remove((object) "{76094280-391F-44AC-8B7B-9B6DEA501110}");
      }
    }
    this._moClientPluginsDataTransfer = (CompositionOptionsWizard.MOClientPluginsDataTransfer) null;
  }

  private void toolStripMenuItem1_Click(object sender, EventArgs e)
  {
    ServiceContainer viewServices = new ServiceContainer();
    viewServices.AddService(typeof (IViewState), (object) new ViewStateService(ViewStateFlags.None));
    ISelectedItems focusedItems = this.navigatorTreeView1.TreeView.FocusedItems;
    (ServicesManager.GetService(typeof (IPDMSubstitutesService)) as IPDMSubstitutesService).EditSubstitutesGroup((object) focusedItems, (IServiceProvider) viewServices, (object) null);
    this.navigatorTreeView1.TreeView.Build(this.navigatorTreeView1.TreeView.FocusedPath);
  }

  private void navigatorTreeView1_TreeView_ShowContextMenu(object sender, MouseEventArgs e)
  {
    if (e.Button != MouseButtons.Right || e.Y < this.navigatorTreeView1.TreeView.HeaderHeight || this.navigatorTreeView1.TreeView.SelectedRow == null || this.navigatorTreeView1.TreeView.SelectedRow.Level == 1 || this.navigatorTreeView1.TreeView.FocusedItem?.GetItemData(typeof (IDBRelationID)) is IDBRelationID itemData && itemData.RelationType != MRP2Consts.reltypeIdSP)
      return;
    ContextMenuStrip contextMenuStrip1 = this.contextMenuStrip1;
    if (contextMenuStrip1 == null || contextMenuStrip1.Visible)
      return;
    contextMenuStrip1.Show((Control) this.navigatorTreeView1.TreeView, e.Location);
  }

  private void navigatorTreeView1_TreeView_AfterCreateNode(object sender, NodeEventArgs e)
  {
    this.navigatorTreeView1.TreeView.CheckStateChanging -= new EventHandler<CheckStateEventArgs>(this.Node_CheckStateChangingEvent);
    try
    {
      NavigatorTreeNode node = e.Node;
      if (node.Level <= 1)
      {
        node.CheckState = CheckState.Checked;
      }
      else
      {
        if (node.Level <= 1)
          return;
        node.CheckState = node.Parent.CheckState;
      }
    }
    finally
    {
      this.navigatorTreeView1.TreeView.CheckStateChanging += new EventHandler<CheckStateEventArgs>(this.Node_CheckStateChangingEvent);
    }
  }

  private void Node_CheckStateChangingEvent(object sender, CheckStateEventArgs e)
  {
    if (e.OldValue == e.NewValue)
      return;
    if (e.Node.Level == 1)
    {
      e.NewValue = CheckState.Checked;
    }
    else
    {
      if (e.NewValue != CheckState.Checked)
        return;
      for (NavigatorTreeNode parent = e.Node.Parent; parent != null && parent.CheckState == CheckState.Unchecked; parent = parent.Parent)
        parent.CheckState = CheckState.Checked;
    }
  }

  private void cbContexts_ItemCheck(object sender, ItemCheckEventArgs e)
  {
    if (this._contextAttr.AttrPossibleValues == null || this._contextAttr.AttrPossibleValues.Count <= e.Index)
      return;
    long int64 = Convert.ToInt64((this._contextAttr.AttrPossibleValues[e.Index] as MyElement).Value);
    this._selectedContexts.Remove(int64);
    if (e.NewValue == CheckState.Checked)
      this._selectedContexts.Add(int64);
    this.UpdateRulePage();
  }

  /// <summary>Clean up any resources being used.</summary>
  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  /// <summary>
  /// Required method for Designer support - do not modify
  /// the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (CompositionOptionsWizard));
    this.btnNext = new Button();
    this.btnCancel = new Button();
    this.btnBack = new Button();
    this.pageSupplyMethod = new Panel();
    this.groupBox1 = new GroupBox();
    this.pageVersionRule = new Panel();
    this.splitContainer1 = new SplitContainer();
    this.imGroupBox1 = new IMGroupBox();
    this.panel4 = new Panel();
    this.cbContexts = new CheckedListBox();
    this.lbPromt = new Label();
    this.panel3 = new Panel();
    this.pictureContext = new PictureBox();
    this._selectEditingContextLinkLabel = new LinkLabel();
    this._clearEditingContextButton = new Button();
    this._setCurrentEditingContextButton = new Button();
    this._selectEditingContextButton = new Button();
    this.panel1 = new Panel();
    this.pictureRule = new PictureBox();
    this._changeVersionsRuleLinkLabel = new LinkLabel();
    this._setDefaultVersionsRuleButton = new Button();
    this._setCurrentVersionsRuleButton = new Button();
    this._changeVersionsRuleButton = new Button();
    this.navigatorTreeView1 = new NavigatorTreeViewWithObjectTypeFiltration();
    this.contextMenuStrip1 = new ContextMenuStrip(this.components);
    this.toolStripMenuItem1 = new ToolStripMenuItem();
    this.pageCount = new Panel();
    this.picture = new PictureBox();
    this.edCount = new TextBox();
    this.lbBought = new Label();
    this.panel2 = new Panel();
    this.seriesDatesSelectingControl = new SeriesDatesSelectingControl();
    this.pageSupplyMethod.SuspendLayout();
    this.pageVersionRule.SuspendLayout();
    this.splitContainer1.BeginInit();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.Panel2.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    this.imGroupBox1.SuspendLayout();
    this.panel4.SuspendLayout();
    this.panel3.SuspendLayout();
    ((ISupportInitialize) this.pictureContext).BeginInit();
    this.panel1.SuspendLayout();
    ((ISupportInitialize) this.pictureRule).BeginInit();
    this.navigatorTreeView1.TreeView.BeginInit();
    this.navigatorTreeView1.SuspendLayout();
    this.contextMenuStrip1.SuspendLayout();
    this.pageCount.SuspendLayout();
    ((ISupportInitialize) this.picture).BeginInit();
    this.panel2.SuspendLayout();
    this.SuspendLayout();
    this.btnNext.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnNext.Location = new Point(635, 7);
    this.btnNext.Name = "btnNext";
    this.btnNext.Size = new Size(75, 23);
    this.btnNext.TabIndex = 3;
    this.btnNext.Text = "&Далее";
    this.btnNext.UseVisualStyleBackColor = true;
    this.btnNext.Click += new EventHandler(this.btnNext_Click);
    this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.Location = new Point(716, 7);
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.Size = new Size(75, 23);
    this.btnCancel.TabIndex = 2;
    this.btnCancel.Text = "Отмена";
    this.btnCancel.UseVisualStyleBackColor = true;
    this.btnBack.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnBack.Location = new Point(554, 7);
    this.btnBack.Name = "btnBack";
    this.btnBack.Size = new Size(75, 23);
    this.btnBack.TabIndex = 4;
    this.btnBack.Text = "&Назад";
    this.btnBack.UseVisualStyleBackColor = true;
    this.btnBack.Click += new EventHandler(this.btnBack_Click);
    this.pageSupplyMethod.Controls.Add((Control) this.groupBox1);
    this.pageSupplyMethod.Location = new Point(12, 12);
    this.pageSupplyMethod.Name = "pageSupplyMethod";
    this.pageSupplyMethod.Size = new Size(224 /*0xE0*/, 58);
    this.pageSupplyMethod.TabIndex = 5;
    this.pageSupplyMethod.Visible = false;
    this.groupBox1.Dock = DockStyle.Fill;
    this.groupBox1.Location = new Point(0, 0);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(224 /*0xE0*/, 58);
    this.groupBox1.TabIndex = 1;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "Метод обработки/поставки";
    this.pageVersionRule.Controls.Add((Control) this.splitContainer1);
    this.pageVersionRule.Location = new Point(6, 76);
    this.pageVersionRule.Name = "pageVersionRule";
    this.pageVersionRule.Size = new Size(785, 374);
    this.pageVersionRule.TabIndex = 6;
    this.splitContainer1.BorderStyle = BorderStyle.FixedSingle;
    this.splitContainer1.Dock = DockStyle.Fill;
    this.splitContainer1.Location = new Point(0, 0);
    this.splitContainer1.Name = "splitContainer1";
    this.splitContainer1.Panel1.Controls.Add((Control) this.imGroupBox1);
    this.splitContainer1.Panel1MinSize = 400;
    this.splitContainer1.Panel2.Controls.Add((Control) this.navigatorTreeView1);
    this.splitContainer1.Size = new Size(785, 374);
    this.splitContainer1.SplitterDistance = 400;
    this.splitContainer1.TabIndex = 2;
    this.imGroupBox1.Controls.Add((Control) this.panel4);
    this.imGroupBox1.Controls.Add((Control) this.seriesDatesSelectingControl);
    this.imGroupBox1.Controls.Add((Control) this.panel3);
    this.imGroupBox1.Controls.Add((Control) this.panel1);
    this.imGroupBox1.Dock = DockStyle.Fill;
    this.imGroupBox1.Location = new Point(0, 0);
    this.imGroupBox1.Name = "imGroupBox1";
    this.imGroupBox1.Size = new Size(398, 372);
    this.imGroupBox1.TabIndex = 4;
    this.imGroupBox1.Text = "Параметры подбора состава объекта";
    this.panel4.Controls.Add((Control) this.cbContexts);
    this.panel4.Controls.Add((Control) this.lbPromt);
    this.panel4.Dock = DockStyle.Top;
    this.panel4.Location = new Point(3, 270);
    this.panel4.Name = "panel4";
    this.panel4.Size = new Size(392, 100);
    this.panel4.TabIndex = 5;
    this.cbContexts.CheckOnClick = true;
    this.cbContexts.Dock = DockStyle.Fill;
    this.cbContexts.FormattingEnabled = true;
    this.cbContexts.IntegralHeight = false;
    this.cbContexts.Location = new Point(0, 27);
    this.cbContexts.Name = "cbContexts";
    this.cbContexts.Size = new Size(392, 73);
    this.cbContexts.TabIndex = 2;
    this.cbContexts.ItemCheck += new ItemCheckEventHandler(this.cbContexts_ItemCheck);
    this.lbPromt.Dock = DockStyle.Top;
    this.lbPromt.ImeMode = ImeMode.NoControl;
    this.lbPromt.Location = new Point(0, 0);
    this.lbPromt.Name = "lbPromt";
    this.lbPromt.Size = new Size(392, 27);
    this.lbPromt.TabIndex = 1;
    this.lbPromt.Text = "Выберите контексты, в рамках которых будут отображаться составы:";
    this.lbPromt.TextAlign = ContentAlignment.MiddleLeft;
    this.panel3.Controls.Add((Control) this.pictureContext);
    this.panel3.Controls.Add((Control) this._selectEditingContextLinkLabel);
    this.panel3.Controls.Add((Control) this._clearEditingContextButton);
    this.panel3.Controls.Add((Control) this._setCurrentEditingContextButton);
    this.panel3.Controls.Add((Control) this._selectEditingContextButton);
    this.panel3.Dock = DockStyle.Top;
    this.panel3.Location = new Point(3, 84);
    this.panel3.Name = "panel3";
    this.panel3.Size = new Size(392, 68);
    this.panel3.TabIndex = 3;
    this.pictureContext.Cursor = Cursors.Hand;
    this.pictureContext.Dock = DockStyle.Left;
    this.pictureContext.Image = (Image) componentResourceManager.GetObject("pictureContext.Image");
    this.pictureContext.ImeMode = ImeMode.NoControl;
    this.pictureContext.InitialImage = (Image) null;
    this.pictureContext.Location = new Point(0, 0);
    this.pictureContext.Name = "pictureContext";
    this.pictureContext.Size = new Size(61, 68);
    this.pictureContext.SizeMode = PictureBoxSizeMode.CenterImage;
    this.pictureContext.TabIndex = 26;
    this.pictureContext.TabStop = false;
    this._selectEditingContextLinkLabel.ActiveLinkColor = Color.Blue;
    this._selectEditingContextLinkLabel.AutoSize = true;
    this._selectEditingContextLinkLabel.Cursor = Cursors.Hand;
    this._selectEditingContextLinkLabel.ImeMode = ImeMode.NoControl;
    this._selectEditingContextLinkLabel.Location = new Point(82, 11);
    this._selectEditingContextLinkLabel.Name = "_selectEditingContextLinkLabel";
    this._selectEditingContextLinkLabel.Size = new Size(198, 13);
    this._selectEditingContextLinkLabel.TabIndex = 22;
    this._selectEditingContextLinkLabel.TabStop = true;
    this._selectEditingContextLinkLabel.Text = "[Выберите контекст редактирования]";
    this._selectEditingContextLinkLabel.VisitedLinkColor = Color.Blue;
    this._selectEditingContextLinkLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(this._selectEditingContextLinkLabel_LinkClicked);
    this._clearEditingContextButton.ImeMode = ImeMode.NoControl;
    this._clearEditingContextButton.Location = new Point(243, 38);
    this._clearEditingContextButton.Name = "_clearEditingContextButton";
    this._clearEditingContextButton.Size = new Size(73, 23);
    this._clearEditingContextButton.TabIndex = 25;
    this._clearEditingContextButton.Text = "Очистить";
    this._clearEditingContextButton.UseVisualStyleBackColor = true;
    this._clearEditingContextButton.Click += new EventHandler(this.ClearEditingContextButton_Click);
    this._setCurrentEditingContextButton.ImeMode = ImeMode.NoControl;
    this._setCurrentEditingContextButton.Location = new Point(164, 38);
    this._setCurrentEditingContextButton.Name = "_setCurrentEditingContextButton";
    this._setCurrentEditingContextButton.Size = new Size(73, 23);
    this._setCurrentEditingContextButton.TabIndex = 24;
    this._setCurrentEditingContextButton.Text = "Текущий";
    this._setCurrentEditingContextButton.UseVisualStyleBackColor = true;
    this._setCurrentEditingContextButton.Click += new EventHandler(this.SetCurrentEditingContextButton_Click);
    this._selectEditingContextButton.ImeMode = ImeMode.NoControl;
    this._selectEditingContextButton.Location = new Point(85, 38);
    this._selectEditingContextButton.Name = "_selectEditingContextButton";
    this._selectEditingContextButton.Size = new Size(73, 23);
    this._selectEditingContextButton.TabIndex = 23;
    this._selectEditingContextButton.Text = "Выбрать...";
    this._selectEditingContextButton.UseVisualStyleBackColor = true;
    this._selectEditingContextButton.Click += new EventHandler(this.SelectEditingContextButton_Click);
    this.panel1.Controls.Add((Control) this.pictureRule);
    this.panel1.Controls.Add((Control) this._changeVersionsRuleLinkLabel);
    this.panel1.Controls.Add((Control) this._setDefaultVersionsRuleButton);
    this.panel1.Controls.Add((Control) this._setCurrentVersionsRuleButton);
    this.panel1.Controls.Add((Control) this._changeVersionsRuleButton);
    this.panel1.Dock = DockStyle.Top;
    this.panel1.Location = new Point(3, 16 /*0x10*/);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(392, 68);
    this.panel1.TabIndex = 2;
    this.pictureRule.Cursor = Cursors.Hand;
    this.pictureRule.Dock = DockStyle.Left;
    this.pictureRule.Image = (Image) componentResourceManager.GetObject("pictureRule.Image");
    this.pictureRule.ImeMode = ImeMode.NoControl;
    this.pictureRule.InitialImage = (Image) null;
    this.pictureRule.Location = new Point(0, 0);
    this.pictureRule.Name = "pictureRule";
    this.pictureRule.Size = new Size(61, 68);
    this.pictureRule.SizeMode = PictureBoxSizeMode.CenterImage;
    this.pictureRule.TabIndex = 21;
    this.pictureRule.TabStop = false;
    this._changeVersionsRuleLinkLabel.ActiveLinkColor = Color.Blue;
    this._changeVersionsRuleLinkLabel.AutoSize = true;
    this._changeVersionsRuleLinkLabel.Cursor = Cursors.Hand;
    this._changeVersionsRuleLinkLabel.ImeMode = ImeMode.NoControl;
    this._changeVersionsRuleLinkLabel.Location = new Point(82, 4);
    this._changeVersionsRuleLinkLabel.Name = "_changeVersionsRuleLinkLabel";
    this._changeVersionsRuleLinkLabel.Size = new Size(192 /*0xC0*/, 13);
    this._changeVersionsRuleLinkLabel.TabIndex = 17;
    this._changeVersionsRuleLinkLabel.TabStop = true;
    this._changeVersionsRuleLinkLabel.Text = "[Выберите правило подбора версий]";
    this._changeVersionsRuleLinkLabel.VisitedLinkColor = Color.Blue;
    this._changeVersionsRuleLinkLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(this._changeVersionsRuleLinkLabel_LinkClicked);
    this._setDefaultVersionsRuleButton.ImeMode = ImeMode.NoControl;
    this._setDefaultVersionsRuleButton.Location = new Point(243, 31 /*0x1F*/);
    this._setDefaultVersionsRuleButton.Name = "_setDefaultVersionsRuleButton";
    this._setDefaultVersionsRuleButton.Size = new Size(90, 23);
    this._setDefaultVersionsRuleButton.TabIndex = 20;
    this._setDefaultVersionsRuleButton.Text = "По умолчанию";
    this._setDefaultVersionsRuleButton.UseVisualStyleBackColor = true;
    this._setDefaultVersionsRuleButton.Click += new EventHandler(this._setDefaultVersionsRuleButton_Click);
    this._setCurrentVersionsRuleButton.ImeMode = ImeMode.NoControl;
    this._setCurrentVersionsRuleButton.Location = new Point(164, 31 /*0x1F*/);
    this._setCurrentVersionsRuleButton.Name = "_setCurrentVersionsRuleButton";
    this._setCurrentVersionsRuleButton.Size = new Size(73, 23);
    this._setCurrentVersionsRuleButton.TabIndex = 19;
    this._setCurrentVersionsRuleButton.Text = "Текущее";
    this._setCurrentVersionsRuleButton.UseVisualStyleBackColor = true;
    this._setCurrentVersionsRuleButton.Click += new EventHandler(this._setCurrentVersionsRuleButton_Click);
    this._changeVersionsRuleButton.ImeMode = ImeMode.NoControl;
    this._changeVersionsRuleButton.Location = new Point(85, 31 /*0x1F*/);
    this._changeVersionsRuleButton.Name = "_changeVersionsRuleButton";
    this._changeVersionsRuleButton.Size = new Size(73, 23);
    this._changeVersionsRuleButton.TabIndex = 18;
    this._changeVersionsRuleButton.Text = "Выбрать...";
    this._changeVersionsRuleButton.UseVisualStyleBackColor = true;
    this._changeVersionsRuleButton.Click += new EventHandler(this._changeVersionsRuleButton_Click);
    this.navigatorTreeView1.BtnClearSorting.AutoToggle = AutoToggleType.Single;
    this.navigatorTreeView1.BtnClearSorting.CommandName = "btCancelSort";
    this.navigatorTreeView1.BtnClearSorting.ImageIndex = 9;
    this.navigatorTreeView1.BtnClearSorting.ToolTipText = "Режим ручной сортировки";
    this.navigatorTreeView1.BtnSetupSorting.CommandName = "btSetupSorting";
    this.navigatorTreeView1.BtnSetupSorting.ImageIndex = 10;
    this.navigatorTreeView1.BtnSetupSorting.ToolTipText = "Выполнить настройку ручной сортировки";
    this.navigatorTreeView1.Dock = DockStyle.Fill;
    this.navigatorTreeView1.ForceIsReadOnly = true;
    this.navigatorTreeView1.ImagesToolbar.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("navigatorTreeView1.ImagesToolbar.ImageStream");
    this.navigatorTreeView1.ImagesToolbar.TransparentColor = Color.Transparent;
    this.navigatorTreeView1.ImagesToolbar.Images.SetKeyName(0, "");
    this.navigatorTreeView1.ImagesToolbar.Images.SetKeyName(1, "");
    this.navigatorTreeView1.ImagesToolbar.Images.SetKeyName(2, "");
    this.navigatorTreeView1.ImagesToolbar.Images.SetKeyName(3, "");
    this.navigatorTreeView1.ImagesToolbar.Images.SetKeyName(4, "ручная_сортировка.png");
    this.navigatorTreeView1.ImagesToolbar.Images.SetKeyName(5, "настройка_ручной_сортировки.png");
    this.navigatorTreeView1.IsReadOnly = true;
    this.navigatorTreeView1.LabelSpace.BeginGroup = true;
    this.navigatorTreeView1.LabelSpace.CommandName = "labelSpace";
    this.navigatorTreeView1.LabelSpace.Enabled = false;
    this.navigatorTreeView1.LabelSpace.Stretch = true;
    this.navigatorTreeView1.LabelSpace.Text = " ";
    this.navigatorTreeView1.LabelSpace.ToolTipText = " ";
    this.navigatorTreeView1.Location = new Point(0, 0);
    this.navigatorTreeView1.Name = "navigatorTreeView1";
    this.navigatorTreeView1.Size = new Size(379, 372);
    this.navigatorTreeView1.TabIndex = 0;
    this.navigatorTreeView1.TreeToolbar.FlipLastItem = true;
    this.navigatorTreeView1.TreeToolbar.FullMenus = true;
    this.navigatorTreeView1.TreeToolbar.Guid = new Guid("3fb71a02-4b93-44ea-84a6-db6e9ca5869f");
    this.navigatorTreeView1.TreeToolbar.Hidden = false;
    this.navigatorTreeView1.TreeToolbar.Items.AddRange(new ToolbarItemBase[3]
    {
      (ToolbarItemBase) this.navigatorTreeView1.BtnClearSorting,
      (ToolbarItemBase) this.navigatorTreeView1.BtnSetupSorting,
      (ToolbarItemBase) this.navigatorTreeView1.LabelSpace
    });
    this.navigatorTreeView1.TreeToolbar.Location = new Point(0, 0);
    this.navigatorTreeView1.TreeToolbar.Name = "_tbTreePanel";
    this.navigatorTreeView1.TreeToolbar.Size = new Size(379, 22);
    this.navigatorTreeView1.TreeToolbar.TabIndex = 8;
    this.navigatorTreeView1.TreeToolbar.Text = "";
    this.navigatorTreeView1.TreeView.AllowDrop = true;
    this.navigatorTreeView1.TreeView.AllowMultiSelect = false;
    this.navigatorTreeView1.TreeView.AllowUserPinnedColumns = false;
    this.navigatorTreeView1.TreeView.CheckBoxStyle = NavigatorTreeViewCheckBoxStyle.TwoState;
    this.navigatorTreeView1.TreeView.DisableCheckedOutColumn = true;
    this.navigatorTreeView1.TreeView.DisableDragAndDrop = true;
    this.navigatorTreeView1.TreeView.DisableIMContextMenu = true;
    this.navigatorTreeView1.TreeView.Dock = DockStyle.Fill;
    this.navigatorTreeView1.TreeView.HeaderStyle.HorzAlignment = StringAlignment.Near;
    this.navigatorTreeView1.TreeView.ImageList = (ImageList) null;
    this.navigatorTreeView1.TreeView.ItemsMode = SelectedItemsMode.Default;
    this.navigatorTreeView1.TreeView.LineStyle = LineStyle.Dot;
    this.navigatorTreeView1.TreeView.Location = new Point(0, 22);
    this.navigatorTreeView1.TreeView.Name = "TreeView";
    this.navigatorTreeView1.TreeView.RowEvenStyle.WordWrap = false;
    this.navigatorTreeView1.TreeView.RowOddStyle.WordWrap = false;
    this.navigatorTreeView1.TreeView.RowSelectedStyle.WordWrap = false;
    this.navigatorTreeView1.TreeView.RowStyle.BorderColor = SystemColors.Control;
    this.navigatorTreeView1.TreeView.RowStyle.BorderStyle = Border3DStyle.Adjust;
    this.navigatorTreeView1.TreeView.RowStyle.BorderWidth = 1;
    this.navigatorTreeView1.TreeView.RowStyle.WordWrap = false;
    this.navigatorTreeView1.TreeView.SelectBeforeEdit = true;
    this.navigatorTreeView1.TreeView.ShowRootRow = false;
    this.navigatorTreeView1.TreeView.Size = new Size(379, 350);
    this.navigatorTreeView1.TreeView.SuppressErrorMessages = true;
    this.navigatorTreeView1.TreeView.TabIndex = 0;
    this.navigatorTreeView1.TreeView.AfterCreateNode += new EventHandler<NodeEventArgs>(this.navigatorTreeView1_TreeView_AfterCreateNode);
    this.navigatorTreeView1.TreeView.ShowContextMenu += new MouseEventHandler(this.navigatorTreeView1_TreeView_ShowContextMenu);
    this.contextMenuStrip1.Items.AddRange(new ToolStripItem[1]
    {
      (ToolStripItem) this.toolStripMenuItem1
    });
    this.contextMenuStrip1.Name = "contextMenuStrip1";
    this.contextMenuStrip1.Size = new Size(258, 26);
    this.toolStripMenuItem1.Name = "toolStripMenuItem1";
    this.toolStripMenuItem1.Size = new Size(257, 22);
    this.toolStripMenuItem1.Text = "Настроить допустимые замены...";
    this.toolStripMenuItem1.Click += new EventHandler(this.toolStripMenuItem1_Click);
    this.pageCount.Controls.Add((Control) this.picture);
    this.pageCount.Controls.Add((Control) this.edCount);
    this.pageCount.Controls.Add((Control) this.lbBought);
    this.pageCount.Location = new Point(304, 0);
    this.pageCount.Name = "pageCount";
    this.pageCount.Size = new Size(325, 70);
    this.pageCount.TabIndex = 9;
    this.picture.Image = (Image) componentResourceManager.GetObject("picture.Image");
    this.picture.ImeMode = ImeMode.NoControl;
    this.picture.Location = new Point(16 /*0x10*/, 19);
    this.picture.Margin = new Padding(2);
    this.picture.Name = "picture";
    this.picture.Size = new Size(47, 39);
    this.picture.SizeMode = PictureBoxSizeMode.CenterImage;
    this.picture.TabIndex = 11;
    this.picture.TabStop = false;
    this.edCount.Location = new Point(67, 40);
    this.edCount.Margin = new Padding(2);
    this.edCount.MaxLength = 256 /*0x0100*/;
    this.edCount.Name = "edCount";
    this.edCount.Size = new Size(249, 20);
    this.edCount.TabIndex = 9;
    this.edCount.Text = "1";
    this.edCount.WordWrap = false;
    this.lbBought.AutoSize = true;
    this.lbBought.ImeMode = ImeMode.NoControl;
    this.lbBought.Location = new Point(67, 20);
    this.lbBought.Margin = new Padding(2, 0, 2, 0);
    this.lbBought.Name = "lbBought";
    this.lbBought.Size = new Size(171, 13);
    this.lbBought.TabIndex = 10;
    this.lbBought.Text = "Количество (по умолчанию - шт):";
    this.panel2.Controls.Add((Control) this.btnBack);
    this.panel2.Controls.Add((Control) this.btnNext);
    this.panel2.Controls.Add((Control) this.btnCancel);
    this.panel2.Dock = DockStyle.Bottom;
    this.panel2.Location = new Point(0, 456);
    this.panel2.Name = "panel2";
    this.panel2.Size = new Size(803, 42);
    this.panel2.TabIndex = 10;
    this.seriesDatesSelectingControl.Dock = DockStyle.Top;
    this.seriesDatesSelectingControl.Location = new Point(3, 152);
    this.seriesDatesSelectingControl.Name = "seriesDatesSelectingControl";
    this.seriesDatesSelectingControl.PictureBoxWidth = 51;
    this.seriesDatesSelectingControl.Size = new Size(392, 118);
    this.seriesDatesSelectingControl.TabIndex = 4;
    this.AcceptButton = (IButtonControl) this.btnNext;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.btnCancel;
    this.ClientSize = new Size(803, 498);
    this.Controls.Add((Control) this.pageCount);
    this.Controls.Add((Control) this.pageVersionRule);
    this.Controls.Add((Control) this.pageSupplyMethod);
    this.Controls.Add((Control) this.panel2);
    this.MinimumSize = new Size(640, 480);
    this.Name = nameof (CompositionOptionsWizard);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Выберите параметры объекта";
    this.FormClosed += new FormClosedEventHandler(this.CompositionOptionsWizard_FormClosed);
    this.Load += new EventHandler(this.CompositionOptionsDialog_Load);
    this.pageSupplyMethod.ResumeLayout(false);
    this.pageVersionRule.ResumeLayout(false);
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.Panel2.ResumeLayout(false);
    this.splitContainer1.EndInit();
    this.splitContainer1.ResumeLayout(false);
    this.imGroupBox1.ResumeLayout(false);
    this.panel4.ResumeLayout(false);
    this.panel3.ResumeLayout(false);
    this.panel3.PerformLayout();
    ((ISupportInitialize) this.pictureContext).EndInit();
    this.panel1.ResumeLayout(false);
    this.panel1.PerformLayout();
    ((ISupportInitialize) this.pictureRule).EndInit();
    this.navigatorTreeView1.TreeView.EndInit();
    this.navigatorTreeView1.ResumeLayout(false);
    this.contextMenuStrip1.ResumeLayout(false);
    this.pageCount.ResumeLayout(false);
    this.pageCount.PerformLayout();
    ((ISupportInitialize) this.picture).EndInit();
    this.panel2.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  /// <summary>Класс-затычка для передачи своих параметров в запросы</summary>
  internal class MOClientPluginsDataTransfer : ClientPluginsDataTransfer
  {
    private CompositionOptionsWizard _owner;

    /// <summary>Создать экземпляр класса</summary>
    public MOClientPluginsDataTransfer(CompositionOptionsWizard owner) => this._owner = owner;

    /// <summary>
    /// Метод вызывается ядром клиентской части для сбора информации у плагинов.
    /// Плагины, подписавшиеся в коллекции IClientPluginsService, должны записать в словарик
    /// PluginsData свою информацию в виде сериализуемых пар значений [Ключ] = [Значение].
    /// Указанная информация будет передана на серверную сторону.
    /// </summary>
    /// <param name="PluginsData">Коллекция сериализуемых пар значений для передачи
    /// дополнительной информации на серверную сторону</param>
    public override void GetPluginData(HybridDictionary PluginsData)
    {
      base.GetPluginData(PluginsData);
      if (PluginsData == null)
        return;
      FiltrationHelper.BlockPluginFiltrations(PluginsData);
      FiltrationHelper.UnlockConfigurator(PluginsData);
      PluginsData[(object) "{529FFE92-FDA7-48B8-AADF-ADB1EE6EF584}"] = (object) false;
      if (this._owner.seriesDatesSelectingControl.SeriesDateSettingsHolder != null)
        PluginsData[(object) "{E2390B62-E0BA-4F7E-89CC-1E9E33F0BB5C}"] = (object) this._owner.seriesDatesSelectingControl.SeriesDateSettingsHolder;
      else
        PluginsData.Remove((object) "{E2390B62-E0BA-4F7E-89CC-1E9E33F0BB5C}");
      PluginsData[(object) "{7196FEC5-A048-4118-AF15-73BEEAA63A87}"] = (object) this._owner.FiltrationSettings.OwnerID;
      PluginsData[(object) "{76094280-391F-44AC-8B7B-9B6DEA501110}"] = (object) this._owner.FiltrationSettings.EditingContext;
    }
  }
}
