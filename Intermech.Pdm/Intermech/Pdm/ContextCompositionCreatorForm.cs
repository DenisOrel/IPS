// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.ContextCompositionCreatorForm
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using DevExpress.IM.Utils;
using DevExpress.IM.XtraEditors.Controls;
using DevExpress.IM.XtraTreeList;
using DevExpress.IM.XtraTreeList.Columns;
using DevExpress.IM.XtraTreeList.Nodes;
using ImSSP;
using Intermech.Bars;
using Intermech.Client.Core;
using Intermech.DataFormats;
using Intermech.Docking;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Navigator;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Pdm.ContextComposition;
using Intermech.PropertyEditors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm;

public class ContextCompositionCreatorForm : Form
{
  private int FPageIndex;
  private const int FMaxPageIndex = 4;
  private long FParentItem;
  private string FParentItemCaption = string.Empty;
  private string FParentItemGuid = string.Empty;
  private int FParentItemType = -1;
  private int FParentItemImage = -1;
  private long FContextComposition;
  private long FContextCompositionID;
  private long FContextCompositionArchive;
  private string FContextCompositionCaption = string.Empty;
  private int FContextCompositionType = -1;
  private int FContextCompositionImage = -1;
  private bool FCCQuantityOK;
  private long FCCPrjLinkID = -1;
  private long FCCProjID;
  private bool stoppedComposition;
  private object lockDesign = (object) new Guid();
  private object lockTech = (object) new Guid();
  private Guid taskGuidDesign = Guid.NewGuid();
  private Guid taskGuidTech = Guid.NewGuid();
  private int percentDesign;
  private int percentTech;
  private Thread threadDesign;
  private Thread threadTech;
  private DataTable tableDesign;
  private DataTable tableTech;
  private bool needTechComposition = true;
  private List<long> _newRelations = new List<long>(0);
  private bool FIsNew;
  private MyAttributeMetadata _contextAttr;
  private ICategoryTypeIconService objtypesIcons;
  private IGuidMapper FGuidMapper;
  private IServiceProvider FViewServices;
  private HybridDictionary FFormSettings = new HybridDictionary(0, true);
  private HybridDictionary FControlsSettings = new HybridDictionary(0, true);
  private bool FIsLoading;
  private bool FError;
  private bool FIsChanged;
  private SortedList<long, MyElement> FRollback = new SortedList<long, MyElement>(0);
  private bool FIsRelPropertiesActive;
  internal static ContextCompositionCreatorForm.MyClientPluginsDataTransfer _pluginData = new ContextCompositionCreatorForm.MyClientPluginsDataTransfer(new long[2]
  {
    0L,
    1L
  }, new long[3]{ 0L, 2L, 3L }, new long[4]
  {
    0L,
    1L,
    2L,
    3L
  });
  internal static IClientPluginsService _pluginsService = (IClientPluginsService) null;
  internal static IFiltrationService _filtrationService = (IFiltrationService) null;
  internal static ICategoryTypeIconService _objtypesIcons = (ICategoryTypeIconService) null;
  internal static int _designRelationTypeID = -1;
  internal static int _attrContext = -10000;
  internal static readonly string OK1 = LocalizationHolder.rm.GetString("Pdm_97");
  internal static readonly string OK2 = LocalizationHolder.rm.GetString("Pdm_98");
  internal static readonly string OK3 = LocalizationHolder.rm.GetString("Pdm_99");
  internal static readonly string Caption0 = LocalizationHolder.rm.GetString("Pdm_100");
  internal static readonly string Caption1 = LocalizationHolder.rm.GetString("Pdm_101");
  internal static readonly string Dialog0 = LocalizationHolder.rm.GetString("Pdm_102");
  internal static readonly string Dialog1 = LocalizationHolder.rm.GetString("Pdm_103");
  private IContainer components;
  private Panel panelBottom;
  private Button btnCancel;
  private Button btnNext;
  private ImageList imagesToolbars;
  private Button btnPrev;
  private Panel panelPage1;
  private SplitContainer scFrame;
  private MenuBar menuComposition;
  private ContextMenuBarItem contextMenuComposition;
  private MenuButtonItem mnpAddToCC;
  private MenuButtonItem mnpRefresh;
  private Intermech.Bars.ToolBar toolBarLeft;
  private ButtonItem btnAdd;
  private ButtonItem btnDelete;
  private Panel panelCompositionTop;
  private Label labelComposition;
  private MenuBar menuContextComposition;
  private ContextMenuBarItem contextMenuContextComposition;
  private MenuButtonItem mnpDelFromCC;
  private Panel panelContextTop;
  private ComboBox cbContext;
  private Panel panelPage0;
  private Button btnBrowseTemplate;
  private TextBox edTemplate;
  private Label labelHint;
  private Label labelCC;
  private Label label1;
  private Panel panelPage3;
  private Label labelHint3;
  private TextBox edContext;
  private Panel panelPage4;
  private Label labelHnt5;
  private Panel panelPage2;
  private MenuButtonItem mnpRefreshTC;
  private SplitContainer TectContainer;
  private Panel panel1;
  private Button btnRelCancel;
  private Button btnRelApply;
  private ObjectPropertyGrid relPropertyGrid;
  private LinkLabel labelRelProperties;
  private TreeList treeAnalyze;
  private TreeListColumn columnDesign;
  private TreeListColumn columnDesignQuantity;
  private TreeListColumn columnTechQuantity;
  private Panel panelTopProgress;
  private NumericUpDown edQuantity;
  private Label labelHint6;
  private PictureBox pictureInfo;
  private Label labelProgress;
  private TreeListColumn columnVersion;
  private Panel panelAnalyze;
  private Label labelAnalyzeInfo;
  private ProgressBar progressBarAnalyze;
  private Panel panelProgress;
  private Label labelProgressTech;
  private ProgressBar progressBarTech;
  private Label labelProgressDesign;
  private ProgressBar progressBarDesign;
  private Label labelAnalyze;
  private Panel panelHint;
  private Label labelHint2;
  private LinkLabel linkLabel;
  private PictureBox pictureObject;
  private NavigatorTreeView treeNewComposition;
  private NavigatorTreeView treeComposition;
  private NavigatorTreeView treeContextComposition;
  private MenuButtonItem _findInTreeMenuButtonItem;

  public ContextCompositionCreatorForm(
    long ParentItem,
    long ContextComposition,
    bool IsNewCC,
    string FormCaption,
    IServiceProvider viewServices)
  {
    this.InitializeComponent();
    HelpProvidersClass.SetHelpOptionForControl((Control) this, 692);
    if (ServicesManager.GetService(typeof (BarManager)) is BarManager service1)
    {
      service1.RendererChanged += new EventHandler(this.ToolbarRendererChanged);
      this.ToolbarRendererChanged((object) service1, EventArgs.Empty);
    }
    this.FParentItem = ParentItem;
    this.FContextCompositionArchive = ContextComposition;
    this.FIsNew = IsNewCC;
    this.FViewServices = viewServices;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      ContextCompositionCreatorForm._designRelationTypeID = ContextCompositionCreatorForm._designRelationTypeID == -1 ? sessionKeeper.Session.IdentHelper.SPRelationTypeID : ContextCompositionCreatorForm._designRelationTypeID;
      IDBObject dbObject = sessionKeeper.Session.GetObject(this.FContextCompositionArchive);
      if (dbObject.CheckoutBy == 0L)
        dbObject = dbObject.CheckOut(false);
      this.FContextComposition = dbObject.ObjectID;
      this.FContextCompositionID = dbObject.ID;
      this.FContextCompositionCaption = dbObject.Caption;
      this.FContextCompositionType = dbObject.ObjectType;
    }
    if (this.FViewServices.GetService(typeof (INotificationService)) is INotificationService service2)
      service2.Subscribe("RelationsChanged", new NotificationEventHandler(this.RelationChangedEvent));
    DBObjectsEventArgs e = new DBObjectsEventArgs("ObjectsCreated", this.FContextComposition);
    service2?.FireEvent((object) null, (NotificationEventArgs) e);
    this.objtypesIcons = ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
    this.FGuidMapper = ServicesManager.GetService(typeof (IGuidMapper)) as IGuidMapper;
    this.FContextCompositionImage = this.objtypesIcons.IndexOf(4, this.FContextCompositionType);
    Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
    this.Size = new Size(workingArea.Width / 100 * 70, workingArea.Height / 100 * 60);
    this.Location = new Point((workingArea.Width - this.Size.Width) / 2, (workingArea.Height - this.Size.Height) / 2);
    FormStorage.LoadLayout((Control) this, (IDictionary) this.FFormSettings);
    if (this.FFormSettings == null)
      this.FFormSettings = new HybridDictionary(0, true);
    this.treeAnalyze.SelectImageList = ContextCompositionCreatorForm._objtypesIcons.ImageList;
    this.treeComposition.DisableColumnsMoving = true;
    this.treeComposition.DisableColumnsSorting = true;
    this.treeContextComposition.DisableColumnsMoving = true;
    this.treeContextComposition.DisableColumnsSorting = true;
    this.panelAnalyze.Height = 0;
    this.panelProgress.Height = 0;
    this.labelAnalyze.Height = 34;
    this.Text = FormCaption != string.Empty ? FormCaption : LocalizationHolder.rm.GetString(sc_16722.ssp_pdm_16723());
    this.treeComposition.AllowDrop = false;
    this.treeContextComposition.AllowDrop = false;
    this.treeNewComposition.AllowDrop = false;
    this.FIsChanged = false;
    this.FError = !this.LoadFormData(ParentItem, viewServices);
    if (this.FError)
      this.Clear();
    this.UpdateControls();
  }

  private void RelationChangedEvent(object sender, NotificationEventArgs e)
  {
    this.tableDesign = (DataTable) null;
    this.needTechComposition = true;
  }

  public static DialogResult Execute(ISelectedItems selectedItems, IServiceProvider viewServices)
  {
    if (selectedItems == null || selectedItems.Count == 0 || !(selectedItems.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData) || !MetaDataHelper.HasObjectTypeDesignedRelType(itemData.ObjectType))
      return DialogResult.Cancel;
    long objectId = itemData.ObjectID;
    int relationTypeId = MetaDataHelper.GetRelationTypeID("cad00023-306c-11d8-b4e9-00304f19f545");
    long num = -1;
    DockManager service = (DockManager) ApplicationServices.Container.GetService(typeof (DockManager));
    long selectedContext = -1;
    string contextNameString = "Общий контекст";
    long relationID = -1;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (SingleContextSelectionForm.Execute() != DialogResult.OK)
        return DialogResult.Cancel;
      selectedContext = SingleContextSelectionForm.DefaultContext;
      num = ContextCompositionCreatorForm.CreateTemplate(objectId, relationTypeId, selectedContext);
      if (num == -1L)
        return DialogResult.Cancel;
      long idByObjectId = sessionKeeper.Session.GetIDByObjectID(num);
      IDBRelation relation = sessionKeeper.Session.GetRelation(objectId, idByObjectId, relationTypeId);
      if (relation == null)
        return DialogResult.Cancel;
      contextNameString = relation.GetAttributeByID(MetaDataHelper.GetAttributeID((object) "cad00651-306c-11d8-b4e9-00304f19f545"))?.Description;
      if (string.IsNullOrEmpty(contextNameString))
        contextNameString = "Общий контекст";
      relationID = relation.RelationID;
    }
    ContextCompositionEditor compositionEditor = new ContextCompositionEditor(objectId, num, selectedContext, contextNameString, relationID);
    if (service == null)
      return DialogResult.Cancel;
    compositionEditor.Show(service);
    compositionEditor.Activate();
    return DialogResult.Cancel;
  }

  public static DialogResult Execute(
    string FormCaption,
    ISelectedItems selectedItems,
    IServiceProvider viewServices)
  {
    ContextCompositionCreatorForm._pluginsService = ContextCompositionCreatorForm._pluginsService == null ? ServicesManager.GetService(typeof (IClientPluginsService)) as IClientPluginsService : ContextCompositionCreatorForm._pluginsService;
    ContextCompositionCreatorForm._filtrationService = ContextCompositionCreatorForm._filtrationService == null ? ServicesManager.GetService(typeof (IFiltrationService)) as IFiltrationService : ContextCompositionCreatorForm._filtrationService;
    ContextCompositionCreatorForm._objtypesIcons = ContextCompositionCreatorForm._objtypesIcons == null ? ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService : ContextCompositionCreatorForm._objtypesIcons;
    if (selectedItems == null || selectedItems.Count == 0 || !(selectedItems.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData) || !MetaDataHelper.HasObjectTypeDesignedRelType(itemData.ObjectType))
      return DialogResult.Cancel;
    long objectId = itemData.ObjectID;
    long template = ContextCompositionCreatorForm.CreateTemplate();
    if (template == -1L)
      return DialogResult.Cancel;
    using (ContextCompositionCreatorForm compositionCreatorForm = new ContextCompositionCreatorForm(objectId, template, true, FormCaption, viewServices))
      return compositionCreatorForm.FError ? DialogResult.Abort : compositionCreatorForm.ShowDialog();
  }

  public static DialogResult Execute(
    string FormCaption,
    long ParentItem,
    IServiceProvider viewServices)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      ContextCompositionCreatorForm._designRelationTypeID = ContextCompositionCreatorForm._designRelationTypeID == -1 ? sessionKeeper.Session.IdentHelper.SPRelationTypeID : ContextCompositionCreatorForm._designRelationTypeID;
      if (!MetaDataHelper.HasObjectTypeDesignedRelType(sessionKeeper.Session.GetObjectInfo(ParentItem).ObjectTypeID))
        return DialogResult.Cancel;
    }
    long template = ContextCompositionCreatorForm.CreateTemplate();
    if (template == -1L)
      return DialogResult.Cancel;
    using (ContextCompositionCreatorForm compositionCreatorForm = new ContextCompositionCreatorForm(ParentItem, template, true, FormCaption, viewServices))
      return compositionCreatorForm.FError ? DialogResult.Abort : compositionCreatorForm.ShowDialog();
  }

  internal void Clear() => this.FError = false;

  private object GetDicValue(HybridDictionary collection, object key, object defaultValue)
  {
    return collection == null || key == null ? defaultValue : collection[key] ?? defaultValue;
  }

  private void ContextCompositionCreatorForm_FormClosed(object sender, FormClosedEventArgs e)
  {
    if (this.FViewServices.GetService(typeof (INotificationService)) is INotificationService service)
      service.Unsubscribe("RelationsChanged", new NotificationEventHandler(this.RelationChangedEvent));
    FormStorage.SaveLayout((Control) this, (IDictionary) this.FFormSettings);
  }

  private void ContextCompositionCreatorForm_Load(object sender, EventArgs e)
  {
    FormStorage.LoadLayout((Control) this, (IDictionary) this.FFormSettings);
    this.DoCalcBarsLabels((object) this, (EventArgs) null);
  }

  private void ShowPanel(Panel panel)
  {
    panel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    panel.Left = 0;
    panel.Top = 0;
    panel.Width = panel.Parent.ClientSize.Width;
    panel.Height = panel.Parent.ClientSize.Height - this.panelBottom.Height;
  }

  private void UpdateControls()
  {
    if (this.FPageIndex == 0 && !this.panelPage0.Visible)
      this.ShowPanel(this.panelPage0);
    if (this.FPageIndex == 1 && !this.panelPage1.Visible)
      this.ShowPanel(this.panelPage1);
    if (this.FPageIndex == 2 && !this.panelPage2.Visible)
      this.ShowPanel(this.panelPage2);
    if (this.FPageIndex == 3 && !this.panelPage3.Visible)
      this.ShowPanel(this.panelPage3);
    if (this.FPageIndex == 4 && !this.panelPage4.Visible)
      this.ShowPanel(this.panelPage4);
    this.panelPage0.Visible = this.FPageIndex == 0;
    this.panelPage1.Visible = this.FPageIndex == 1;
    this.panelPage2.Visible = this.FPageIndex == 2;
    this.panelPage3.Visible = this.FPageIndex == 3;
    this.panelPage4.Visible = this.FPageIndex == 4;
    this.cbContext.Enabled = !this.FIsChanged;
    this.edContext.Text = $"[{this.FContextComposition}] {this.FContextCompositionCaption}";
    this.edTemplate.Text = $"[{this.FParentItem}] {this.FParentItemCaption}";
    bool flag1 = false;
    if (this.FPageIndex == 0)
      flag1 = this.FParentItemGuid != string.Empty && this.FPageIndex < 4;
    if (this.FPageIndex == 1)
      flag1 = this.FParentItemGuid != string.Empty && this.FPageIndex < 4 && this._newRelations.Count > 0;
    if (this.FPageIndex == 2)
      flag1 = this.FParentItemGuid != string.Empty && this.FPageIndex < 4 && this._newRelations.Count > 0;
    if (this.FPageIndex == 3)
      flag1 = this.FParentItemGuid != string.Empty && this.FPageIndex < 4 && this._newRelations.Count > 0 && this.FCCQuantityOK && this.treeNewComposition.SelectedItems != null && this.treeNewComposition.SelectedItems.Count > 0;
    if (this.FPageIndex == 4)
      flag1 = this.FCCPrjLinkID != -1L;
    this.btnCancel.Enabled = true;
    this.btnNext.Enabled = flag1;
    bool flag2 = this.FPageIndex > 0;
    if (this.FPageIndex == 1)
      flag2 = !this.FIsChanged && this.FPageIndex > 0;
    if (this.FPageIndex == 3)
      flag2 = this.FCCPrjLinkID == -1L;
    if (this.FPageIndex > 3)
      flag2 = !this.FIsChanged && this.FPageIndex > 0;
    this.btnPrev.Enabled = flag2;
    if (this.FPageIndex == 4)
      this.btnNext.Text = ContextCompositionCreatorForm.OK3;
    else
      this.btnNext.Text = ContextCompositionCreatorForm.OK1;
    string str = this.FIsRelPropertiesActive ? ContextCompositionCreatorForm.Caption1 : ContextCompositionCreatorForm.Caption0;
    if (this.labelRelProperties.Text != str)
      this.labelRelProperties.Text = str;
    this.TectContainer.IsSplitterFixed = !this.FIsRelPropertiesActive;
    if (this.TectContainer.Panel2Collapsed == this.FIsRelPropertiesActive)
      this.TectContainer.Panel2Collapsed = !this.FIsRelPropertiesActive;
    this.btnRelApply.Enabled = this.FIsRelPropertiesActive && this.relPropertyGrid.IsChanged;
    this.btnRelCancel.Enabled = this.btnRelApply.Enabled;
  }

  public static long CreateTemplate()
  {
    return (ServicesManager.GetService(typeof (IObjectCreatorService)) as IObjectCreatorService).CreateObjectByTypeDialog(new Guid("cad00650-306c-11d8-b4e9-00304f19f545"));
  }

  public static long CreateTemplate(long parentObjectID, int relationTypeID, long selectedContext)
  {
    IObjectCreatorService service = ApplicationServices.Container.GetService(typeof (IObjectCreatorService)) as IObjectCreatorService;
    ObjectRelationLink objectRelationLink = new ObjectRelationLink(parentObjectID, relationTypeID)
    {
      Attributes = new Dictionary<int, object>(1)
      {
        {
          MetaDataHelper.GetAttributeID((object) "cad00651-306c-11d8-b4e9-00304f19f545"),
          (object) selectedContext
        }
      }
    };
    Guid aObjectTypeGuid = new Guid("cad00650-306c-11d8-b4e9-00304f19f545");
    ObjectRelationLink[] aObjRelations = new ObjectRelationLink[1]
    {
      objectRelationLink
    };
    return service.CreateObjectByTypeDialog(aObjectTypeGuid, aObjRelations);
  }

  internal bool LoadFormData(long ParentID, IServiceProvider viewServices)
  {
    this.StopExpandDesign();
    this.Clear();
    this.FIsChanged = false;
    this.UpdateControls();
    if (this.FParentItemGuid == string.Empty)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        this._contextAttr = new MyAttributeMetadata("cad00651-306c-11d8-b4e9-00304f19f545");
        ContextCompositionCreatorForm._designRelationTypeID = ContextCompositionCreatorForm._designRelationTypeID == -1 ? sessionKeeper.Session.IdentHelper.SPRelationTypeID : ContextCompositionCreatorForm._designRelationTypeID;
        QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(ParentID);
        this.FParentItemCaption = objectInfo.Caption;
        this.FParentItemGuid = objectInfo.VersionGuid.ToString();
        this.FParentItemType = objectInfo.ObjectTypeID;
        this.FParentItemImage = this.objtypesIcons.IndexOf(4, this.FParentItemType);
      }
    }
    try
    {
      ContextCompositionCreatorForm._pluginsService.RegisterClientPlugin(ContextCompositionCreatorForm._pluginData.PluginGuid, (IClientPluginsDataTransfer) ContextCompositionCreatorForm._pluginData);
      ContextCompositionCreatorForm._pluginData.CurrentSet = 0;
      IDescriptor rootDescriptor1 = (IDescriptor) new Intermech.Navigator.DBObjects.Descriptor(this.FParentItem);
      this.treeComposition.Services = viewServices;
      this.treeComposition.OnGetSupportedColumnsEventHandler += new GetSupportedColumnsEventHandler(Intermech.Navigator.Utils.GetContextStatusColumns);
      this.treeComposition.SetColumns(Intermech.Navigator.Utils.ContextStatusColumns());
      this.treeComposition.Build(rootDescriptor1);
      ContextCompositionCreatorForm._pluginData.CurrentSet = 1;
      IDescriptor rootDescriptor2 = (IDescriptor) new Intermech.Navigator.DBObjects.Descriptor(this.FContextComposition);
      this.treeContextComposition.Services = viewServices;
      this.treeContextComposition.OnGetSupportedColumnsEventHandler += new GetSupportedColumnsEventHandler(Intermech.Navigator.Utils.GetContextColumns);
      this.treeContextComposition.SetColumns(Intermech.Navigator.Utils.ContextColumns());
      this.treeContextComposition.Build(rootDescriptor2);
      ContextCompositionCreatorForm._pluginData.CurrentSet = 0;
      IDescriptor rootDescriptor3 = (IDescriptor) new Intermech.Navigator.DBObjects.Descriptor(this.FParentItem);
      this.treeNewComposition.Services = viewServices;
      this.treeNewComposition.OnGetSupportedColumnsEventHandler += new GetSupportedColumnsEventHandler(Intermech.Navigator.Utils.GetContextStatusColumns);
      this.treeNewComposition.SetColumns(Intermech.Navigator.Utils.ContextStatusColumns());
      this.treeNewComposition.Build(rootDescriptor3);
    }
    finally
    {
      ContextCompositionCreatorForm._pluginsService.UnregisterClientPlugin(ContextCompositionCreatorForm._pluginData.PluginGuid);
    }
    this.UpdateControls();
    this.cbContext.Items.Clear();
    if (this._contextAttr.AttrPossibleValues != null && this._contextAttr.AttrPossibleValues.Count > 0)
    {
      for (int index = 0; index < this._contextAttr.AttrPossibleValues.Count; ++index)
      {
        if (index >= 2)
          this.cbContext.Items.Add(this._contextAttr.AttrPossibleValues[index]);
      }
    }
    if (this.cbContext.Items.Count > 0)
      this.cbContext.SelectedIndex = 0;
    return true;
  }

  private void StoreComposition()
  {
    if (this.FError || this.FIsLoading || !this.FIsChanged)
      return;
    this.UpdateControls();
  }

  private void DoBrowseTemplate(object sender, EventArgs e)
  {
    long[] numArray = Intermech.Navigator.SelectionWindow.SelectObjects(LocalizationHolder.rm.GetString("Pdm_106"), LocalizationHolder.rm.GetString("Pdm_107"), SelectionOptions.Default);
    if (numArray == null)
      return;
    long objectID = numArray[0];
    if (objectID == this.FParentItem)
      return;
    int num1 = 0;
    try
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(objectID);
        if (!MetaDataHelper.HasObjectTypeDesignedRelType(dbObject.ObjectType))
        {
          num1 = 1;
          return;
        }
        Guid guid = new Guid("cad00650-306c-11d8-b4e9-00304f19f545");
        if (dbObject.isParentType(guid))
        {
          num1 = 2;
          return;
        }
        this.FParentItem = dbObject.ObjectID;
        this.FParentItemCaption = dbObject.Caption;
        this.FParentItemGuid = dbObject.ObjectGUID.ToString();
        this.FParentItemType = dbObject.ObjectType;
      }
    }
    finally
    {
      if (num1 == 1)
      {
        int num2 = (int) MessageBox.Show(LocalizationHolder.rm.GetString(sc_16722.ssp_pdm_16724()), LocalizationHolder.rm.GetString("Pdm_109"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      if (num1 == 2)
      {
        int num3 = (int) MessageBox.Show(LocalizationHolder.rm.GetString(sc_16722.ssp_pdm_16725()), LocalizationHolder.rm.GetString("Pdm_111"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }
    this.LoadFormData(this.FParentItem, this.FViewServices);
    this.UpdateControls();
  }

  private void StopExpandDesign()
  {
    if (this.threadDesign != null)
      this.threadDesign.Abort();
    this.threadDesign = (Thread) null;
    lock (this.lockDesign)
    {
      this.percentDesign = 0;
      this.tableDesign = (DataTable) null;
    }
  }

  private void StartExpandDesign()
  {
    this.StopExpandDesign();
    using (FixEditingContext fixEditingContext = new FixEditingContext())
    {
      this.threadDesign = new Thread(fixEditingContext.SendEditingContextToThread(new ThreadStart(this.ThreadSelectDesign)));
      this.threadDesign.IsBackground = true;
      this.threadDesign.Name = "SelectDesignComposition";
      this.threadDesign.Start();
    }
  }

  private void StopExpandTech()
  {
    if (this.threadTech != null)
      this.threadTech.Abort();
    this.threadTech = (Thread) null;
    lock (this.lockTech)
    {
      this.percentTech = 0;
      this.tableTech = (DataTable) null;
    }
  }

  private void StartExpandTech()
  {
    if (!this.needTechComposition)
      return;
    this.StopExpandTech();
    using (FixEditingContext fixEditingContext = new FixEditingContext())
    {
      this.threadTech = new Thread(fixEditingContext.SendEditingContextToThread(new ThreadStart(this.ThreadSelectTech)));
      this.threadTech.IsBackground = true;
      this.threadTech.Name = "SelectTechComposition";
      this.threadTech.Start();
    }
  }

  private int CompareQuantity()
  {
    int num1 = 0;
    this.panelHint.Visible = true;
    this.labelAnalyze.Height = 0;
    this.panelProgress.Height = 43;
    this.progressBarDesign.Minimum = 0;
    this.progressBarDesign.Maximum = 100;
    this.progressBarDesign.Value = 0;
    this.progressBarTech.Minimum = 0;
    this.progressBarTech.Maximum = 100;
    this.progressBarTech.Value = 0;
    try
    {
      this.btnNext.Enabled = false;
      this.btnPrev.Enabled = false;
      this.btnCancel.Enabled = false;
      this.Invalidate();
      this.Update();
      while (true)
      {
        lock (this.lockDesign)
        {
          lock (this.lockTech)
          {
            this.progressBarDesign.Value = this.threadDesign == null ? 100 : this.percentDesign;
            this.progressBarTech.Value = this.threadTech == null ? 100 : this.percentTech;
            num1 = this.percentDesign == 100 && this.percentTech == 100 || this.threadDesign == null && this.threadTech == null && this.tableDesign != null && this.tableTech != null ? 0 : -1;
          }
        }
        if (num1 != 0 && !this.stoppedComposition)
          Application.DoEvents();
        else
          break;
      }
      if (num1 == 0 || this.stoppedComposition)
      {
        this.progressBarDesign.Value = 100;
        this.progressBarTech.Value = 100;
      }
      this.panelProgress.Height = 0;
      this.panelHint.Visible = false;
      Application.DoEvents();
      this.Invalidate();
      this.Update();
      num1 = this.FillAnalyzeTree();
    }
    finally
    {
      this.btnCancel.Enabled = true;
      this.labelAnalyze.Height = 34;
      this.UpdateControls();
    }
    if (num1 == -3)
    {
      int num2 = (int) MessageBox.Show(LocalizationHolder.rm.GetString("Pdm_113"), LocalizationHolder.rm.GetString("Pdm_119"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }
    return num1;
  }

  private TreeListNode AddAnalyzeTreeNode(DataRow dataDesign, DataRow dataTech)
  {
    return (TreeListNode) null;
  }

  private int FillAnalyzeTree()
  {
    lock (this.lockDesign)
    {
      lock (this.lockTech)
      {
        try
        {
          if (this.tableDesign == null || this.tableTech == null)
            return -1;
          if (this.stoppedComposition)
            return -2;
          this.progressBarAnalyze.Minimum = 0;
          this.progressBarAnalyze.Value = 0;
          this.progressBarAnalyze.Maximum = this.tableDesign.Rows.Count + this.tableTech.Rows.Count;
          this.panelAnalyze.Height = 43;
          this.labelAnalyzeInfo.Text = LocalizationHolder.rm.GetString("Pdm_120");
          Application.DoEvents();
          this.Invalidate();
          this.Update();
          this.treeAnalyze.BeginUpdate();
          this.treeAnalyze.BeginSort();
          this.treeAnalyze.ClearNodes();
          if (this.tableDesign == null || this.tableDesign.Rows.Count == 0 || this.tableTech == null || this.tableTech.Rows.Count == 0)
            return -1;
          SortedList<long, ElementQuantity> sortedList1 = new SortedList<long, ElementQuantity>(0);
          SortedList<long, ElementQuantity> sortedList2 = new SortedList<long, ElementQuantity>(0);
          int num1 = 0;
          foreach (DataRow row in (InternalDataCollectionBase) this.tableDesign.Rows)
          {
            try
            {
              long int64Value = DataSetProcessor.GetInt64Value(row, 2, 0L);
              int int32Value = DataSetProcessor.GetInt32Value(row, 0, -1);
              string stringValue1 = DataSetProcessor.GetStringValue(row, 3, string.Empty);
              string stringValue2 = DataSetProcessor.GetStringValue(row, 4, string.Empty);
              ++num1;
              if (num1 % 15 == 0)
                this.progressBarAnalyze.Value = num1;
              if (num1 % 30 == 0)
                Application.DoEvents();
              if (!sortedList1.ContainsKey(int64Value))
              {
                ElementQuantity elementQuantity = new ElementQuantity(stringValue2, int32Value, stringValue1, string.Empty);
                sortedList1[int64Value] = elementQuantity;
              }
              else
              {
                ElementQuantity elementQuantity = sortedList1[int64Value];
                if (stringValue1 != string.Empty)
                {
                  MeasuredValue baseMeasure = MeasureHelper.ConvertToBaseMeasure(MeasureHelper.ConvertToMeasuredValue(stringValue1));
                  if (elementQuantity.DesignQuantity == null)
                    elementQuantity.DesignQuantity = baseMeasure;
                  else
                    elementQuantity.DesignQuantity.Value += baseMeasure.Value;
                }
              }
            }
            catch
            {
            }
          }
          foreach (DataRow row in (InternalDataCollectionBase) this.tableTech.Rows)
          {
            try
            {
              long int64Value = DataSetProcessor.GetInt64Value(row, 2, 0L);
              string stringValue = DataSetProcessor.GetStringValue(row, 3, string.Empty);
              ++num1;
              if (num1 % 15 == 0)
                this.progressBarAnalyze.Value = num1;
              if (num1 % 30 == 0)
                Application.DoEvents();
              if (sortedList1.ContainsKey(int64Value))
              {
                ElementQuantity elementQuantity = sortedList1[int64Value];
                if (stringValue != string.Empty)
                {
                  MeasuredValue baseMeasure = MeasureHelper.ConvertToBaseMeasure(MeasureHelper.ConvertToMeasuredValue(stringValue));
                  if (elementQuantity.TechQuantity == null)
                    elementQuantity.TechQuantity = baseMeasure;
                  else
                    elementQuantity.TechQuantity.Value += baseMeasure.Value;
                }
              }
            }
            catch
            {
            }
          }
          this.progressBarAnalyze.Value = 0;
          this.progressBarAnalyze.Maximum = sortedList1.Count;
          int num2 = 0;
          this.labelAnalyzeInfo.Text = LocalizationHolder.rm.GetString("Pdm_121");
          this.Invalidate();
          this.Update();
          IEnumerator<KeyValuePair<long, ElementQuantity>> enumerator1 = sortedList1.GetEnumerator();
          KeyValuePair<long, ElementQuantity> current;
          if (enumerator1 != null)
          {
            enumerator1.Reset();
            while (enumerator1.MoveNext())
            {
              ++num2;
              if (num2 % 15 == 0)
                this.progressBarAnalyze.Value = num2;
              if (num2 % 30 == 0)
                Application.DoEvents();
              current = enumerator1.Current;
              if (current.Value.TechQuantity != null)
              {
                current = enumerator1.Current;
                if (current.Value.DesignQuantity != null)
                {
                  current = enumerator1.Current;
                  double num3 = current.Value.TechQuantity.Value * Convert.ToDouble(this.edQuantity.Value);
                  current = enumerator1.Current;
                  double num4 = current.Value.DesignQuantity.Value;
                  if (num3 > num4)
                  {
                    SortedList<long, ElementQuantity> sortedList3 = sortedList2;
                    current = enumerator1.Current;
                    long key = current.Key;
                    current = enumerator1.Current;
                    ElementQuantity elementQuantity = current.Value;
                    sortedList3.Add(key, elementQuantity);
                  }
                }
              }
            }
          }
          this.progressBarAnalyze.Value = 0;
          this.progressBarAnalyze.Maximum = sortedList2.Count;
          int num5 = 0;
          this.labelAnalyzeInfo.Text = LocalizationHolder.rm.GetString("Pdm_122");
          this.Invalidate();
          this.Update();
          IEnumerator<KeyValuePair<long, ElementQuantity>> enumerator2 = sortedList2.GetEnumerator();
          if (enumerator2 != null)
          {
            enumerator2.Reset();
            while (enumerator2.MoveNext())
            {
              ++num5;
              if (num5 % 15 == 0)
                this.progressBarAnalyze.Value = num5;
              if (num5 % 30 == 0)
                Application.DoEvents();
              current = enumerator2.Current;
              double aValue = current.Value.TechQuantity.Value * Convert.ToDouble(this.edQuantity.Value);
              current = enumerator2.Current;
              long measureId = current.Value.TechQuantity.MeasureID;
              MeasuredValue measuredValue = new MeasuredValue(aValue, measureId);
              TreeList treeAnalyze = this.treeAnalyze;
              object[] nodeData = new object[4];
              current = enumerator2.Current;
              nodeData[0] = (object) current.Key;
              current = enumerator2.Current;
              nodeData[1] = (object) current.Value.Caption;
              current = enumerator2.Current;
              nodeData[2] = (object) current.Value.DesignQuantity.Caption;
              nodeData[3] = (object) measuredValue.Caption;
              TreeListNode treeListNode = treeAnalyze.AppendNode((object) nodeData, (TreeListNode) null);
              ICategoryTypeIconService objtypesIcons = ContextCompositionCreatorForm._objtypesIcons;
              current = enumerator2.Current;
              int objectType = current.Value.ObjectType;
              treeListNode.ImageIndex = objtypesIcons.IndexOf(4, objectType);
              treeListNode.SelectImageIndex = treeListNode.ImageIndex;
            }
          }
          return sortedList2.Count == 0 ? 0 : -3;
        }
        finally
        {
          this.treeAnalyze.EndSort();
          this.treeAnalyze.EndUpdate();
          this.panelAnalyze.Height = 0;
        }
      }
    }
  }

  private bool TryAddContextComposition()
  {
    if (this.FCCPrjLinkID != -1L && this.FCCProjID != 0L)
      return true;
    ISelectedItems selectedItems = this.treeNewComposition.SelectedItems;
    if (selectedItems == null || selectedItems.Count == 0 || this.cbContext.SelectedIndex < 0 || this.cbContext.Items.Count == 0)
      return false;
    long num1 = (long) (this.cbContext.Items[this.cbContext.SelectedIndex] as MyElement).Value;
    List<MeasureDescriptor> measureDescriptorList = new List<MeasureDescriptor>(0);
    List<long> relationIDs = new List<long>(0);
    List<long> projIDs = new List<long>(0);
    List<int> relTypeIDs = new List<int>(0);
    int attributeTypeId = MetaDataHelper.GetAttributeTypeID("cad00267-306c-11d8-b4e9-00304f19f545");
    IDBObjectID itemData1 = selectedItems.GetItemData(0, typeof (IDBObjectID)) as IDBObjectID;
    IDBRelationID itemData2 = selectedItems.GetItemData(0, typeof (IDBRelationID)) as IDBRelationID;
    if (itemData1 == null || itemData2 == null)
      return false;
    long num2 = itemData1.Value;
    int relationType = itemData2.RelationType;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (itemData2.RelationType == -1)
        relationType = sessionKeeper.Session.IdentHelper.SPRelationTypeID;
      if (MetaDataHelper.IsObjectTypeChildOf(sessionKeeper.Session.GetObject(num2).ObjectType, this.FContextCompositionType))
        return false;
      IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(relationType);
      IDBRelation dbRelation;
      try
      {
        NewRelationProperties properties = new NewRelationProperties(itemData2.Value, num2, this.FContextCompositionID)
        {
          PartObjectID = this.FContextComposition
        } with
        {
          ValuesList = new AttributeValues[1]
          {
            new AttributeValues(this._contextAttr.AttrID, FieldTypes.ftInteger, MultiValueModes.MultiValuesFromList, new object[1]
            {
              (object) num1
            })
          }
        };
        dbRelation = relationCollection.Create(properties);
      }
      catch (Exception ex)
      {
        int num3 = (int) MessageBox.Show(ex.Message, LocalizationHolder.rm.GetString(sc_16722.ssp_pdm_16726()), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      IDBAttribute byId1 = dbRelation.Attributes.FindByID(this._contextAttr.AttrID);
      if (byId1 == null)
      {
        dbRelation.Delete(0L);
        return false;
      }
      this.FCCProjID = num2;
      this.FCCPrjLinkID = dbRelation.RelationID;
      relationIDs.Add(this.FCCPrjLinkID);
      projIDs.Add(dbRelation.ProjID);
      relTypeIDs.Add(dbRelation.RelationType);
      this._newRelations.Add(this.FCCPrjLinkID);
      byId1.Value = (object) num1;
      IDBAttribute byId2 = dbRelation.Attributes.FindByID(attributeTypeId);
      if (byId2 != null)
      {
        object obj = byId2.Value;
        measureDescriptorList.Clear();
        foreach (MeasureDescriptor measure in MeasureHelper.Measures)
        {
          if (measure.PhysicalQuantityGuid == SystemGUIDs.objectQuantityGuid)
            measureDescriptorList.Add(measure);
        }
        byId2.Value = (object) MeasureHelper.ConvertToMeasuredValue($"{this.edQuantity.Value.ToString()} {measureDescriptorList[0].ShortName}");
      }
      IDBObject dbObject = sessionKeeper.Session.GetObject(this.FContextComposition);
      try
      {
        long objectId = dbObject.ObjectID;
        dbObject.CheckIn();
        (this.FViewServices.GetService(typeof (INotificationService)) as INotificationService).FireEvent((object) null, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsCheckedIn", objectId));
      }
      catch
      {
      }
      this.FContextComposition = dbObject.ObjectID;
      this.FContextCompositionCaption = dbObject.Caption;
      this.FContextCompositionID = dbObject.ID;
      this.FContextCompositionType = dbObject.TypeID;
    }
    try
    {
      ContextCompositionCreatorForm._pluginsService.RegisterClientPlugin(ContextCompositionCreatorForm._pluginData.PluginGuid, (IClientPluginsDataTransfer) ContextCompositionCreatorForm._pluginData);
      ContextCompositionCreatorForm._pluginData.CurrentSet = 2;
      DBRelationsEventArgs e = new DBRelationsEventArgs("RelationsCreated", (IList<long>) relationIDs, (IList<long>) projIDs, (IList<int>) null, (IList<int>) relTypeIDs);
      if (this.FViewServices.GetService(typeof (INotificationService)) is INotificationService service)
        service.FireEvent((object) null, (NotificationEventArgs) e);
    }
    finally
    {
      ContextCompositionCreatorForm._pluginsService.UnregisterClientPlugin(ContextCompositionCreatorForm._pluginData.PluginGuid);
    }
    this.FIsChanged = true;
    this.UpdateControls();
    return true;
  }

  private void Goto_PrevPage(object sender, EventArgs e)
  {
    if (this.FPageIndex <= 0)
      return;
    --this.FPageIndex;
    this.UpdateControls();
  }

  private void Goto_NextPage(object sender, EventArgs e)
  {
    if (this.FPageIndex == 1)
    {
      this.treeAnalyze.BeginUpdate();
      this.treeAnalyze.BeginSort();
      this.treeAnalyze.ClearNodes();
      this.treeAnalyze.EndSort();
      this.treeAnalyze.EndUpdate();
      ++this.FPageIndex;
      this.UpdateControls();
      this.stoppedComposition = false;
      if (this.tableDesign == null || this.tableDesign.Rows.Count == 0)
        this.StartExpandDesign();
      this.StartExpandTech();
      this.FCCQuantityOK = this.CompareQuantity() == 0;
      this.UpdateControls();
    }
    else if (this.FPageIndex == 2)
    {
      this.UpdateControls();
      this.stoppedComposition = false;
      if (this.tableDesign == null || this.tableDesign.Rows.Count == 0)
        this.StartExpandDesign();
      this.StartExpandTech();
      this.FCCQuantityOK = this.CompareQuantity() == 0;
      if (this.FCCQuantityOK)
        ++this.FPageIndex;
      this.UpdateControls();
    }
    else
    {
      if (this.FPageIndex == 3 && !this.TryAddContextComposition())
        return;
      if (this.FPageIndex == 4 && this.FCCPrjLinkID != -1L && this.FCCQuantityOK)
      {
        this.DialogResult = DialogResult.OK;
      }
      else
      {
        ++this.FPageIndex;
        this.UpdateControls();
      }
    }
  }

  private void DoCancelChanges(object sender, EventArgs e)
  {
    this.StopExpandDesign();
    this.StopExpandTech();
    List<long> relationIDs = new List<long>(0);
    List<long> longList = new List<long>(0);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject objectActualCopy1 = sessionKeeper.Session.GetObjectActualCopy(Math.Abs(this.FContextComposition), true);
      try
      {
        if (objectActualCopy1.CheckoutBy == sessionKeeper.Session.UserID)
          objectActualCopy1.CancelChanges();
      }
      catch
      {
      }
      try
      {
        objectActualCopy1 = sessionKeeper.Session.GetObjectActualCopy(Math.Abs(this.FContextComposition), true);
        if (this.FCCPrjLinkID != -1L)
        {
          sessionKeeper.Session.GetRelation(this.FCCPrjLinkID, false)?.Delete(0L);
          relationIDs.Add(this.FCCPrjLinkID);
        }
      }
      catch
      {
      }
      this.FCCPrjLinkID = -1L;
      try
      {
        if (this.FIsNew)
        {
          longList.Add(objectActualCopy1.ObjectID);
          longList.Add(-objectActualCopy1.ObjectID);
          objectActualCopy1.Delete(0L);
        }
      }
      catch
      {
      }
      if (this.FIsNew)
      {
        try
        {
          IDBObject objectActualCopy2 = sessionKeeper.Session.GetObjectActualCopy(Math.Abs(this.FContextCompositionArchive), false);
          if (objectActualCopy2 != null)
          {
            if (!longList.Contains(objectActualCopy2.ObjectID))
              longList.Add(objectActualCopy2.ObjectID);
            objectActualCopy2.Delete(0L);
          }
        }
        catch
        {
        }
      }
    }
    try
    {
      ContextCompositionCreatorForm._pluginsService.RegisterClientPlugin(ContextCompositionCreatorForm._pluginData.PluginGuid, (IClientPluginsDataTransfer) ContextCompositionCreatorForm._pluginData);
      ContextCompositionCreatorForm._pluginData.CurrentSet = 2;
      DBRelationsEventArgs e1 = new DBRelationsEventArgs("RelationsRemoved", (IList<long>) relationIDs);
      if (this.FViewServices.GetService(typeof (INotificationService)) is INotificationService service1)
        service1.FireEvent((object) null, (NotificationEventArgs) e1);
      DBObjectsEventArgs e2 = new DBObjectsEventArgs("ObjectsRemoved", (IList<long>) longList.ToArray());
      if (!(this.FViewServices.GetService(typeof (INotificationService)) is INotificationService service2))
        return;
      service2.FireEvent((object) null, (NotificationEventArgs) e2);
    }
    catch
    {
    }
    finally
    {
      ContextCompositionCreatorForm._pluginsService.UnregisterClientPlugin(ContextCompositionCreatorForm._pluginData.PluginGuid);
    }
  }

  private void DoUpdateControls(object sender, EventArgs e) => this.UpdateControls();

  private void DoAddToContext(object sender, EventArgs e)
  {
    ISelectedItems selectedItems = this.treeComposition.SelectedItems;
    if (selectedItems == null || selectedItems.Count == 0 || this.cbContext.SelectedIndex < 0 || this.cbContext.Items.Count == 0)
      return;
    long num1 = (long) (this.cbContext.Items[this.cbContext.SelectedIndex] as MyElement).Value;
    List<MeasureDescriptor> measureDescriptorList = new List<MeasureDescriptor>(0);
    List<long> relationIDs1 = new List<long>(0);
    List<long> projIDs = new List<long>(0);
    List<int> relTypeIDs = new List<int>(0);
    List<long> relationIDs2 = new List<long>(0);
    MeasureForm measureForm = new MeasureForm();
    int attributeId = MetaDataHelper.GetAttributeID((object) "cad00267-306c-11d8-b4e9-00304f19f545");
    ProgressForm progressForm = (ProgressForm) null;
    try
    {
      this.btnNext.Enabled = false;
      this.btnPrev.Enabled = false;
      this.btnCancel.Enabled = false;
      this.Enabled = false;
      this.Invalidate();
      this.Update();
      if (selectedItems.Count > 1)
        progressForm = ProgressForm.Execute(LocalizationHolder.rm.GetString("Pdm_124"), LocalizationHolder.rm.GetString("Pdm_125"), 0, selectedItems.Count, false, string.Empty, (EventHandler) null);
      for (int index = 0; index < selectedItems.Count; ++index)
      {
        if (progressForm != null)
          progressForm.ProgressValue = index;
        IDBObjectID itemData1 = selectedItems.GetItemData(index, typeof (IDBObjectID)) as IDBObjectID;
        IDBRelationID itemData2 = selectedItems.GetItemData(index, typeof (IDBRelationID)) as IDBRelationID;
        if (itemData1 != null && itemData2 != null && itemData1.Value != this.FParentItem)
        {
          aMeasureValue = (MeasuredValue) null;
          using (SessionKeeper sessionKeeper = new SessionKeeper())
          {
            QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(itemData1.Value);
            if (!MetaDataHelper.IsObjectTypeChildOf(objectInfo.ObjectTypeID, this.FContextCompositionType))
            {
              string str = $"[{objectInfo.ObjectID}] {objectInfo.Caption}";
              IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(itemData2.RelationType);
              IDBRelation relation = sessionKeeper.Session.GetRelation(itemData2.Value);
              IDBAttribute byId1 = relation.Attributes.FindByID(this._contextAttr.AttrID);
              if (byId1 != null)
              {
                if ((long) byId1.Value != 0L)
                {
                  if ((long) byId1.Value != 1L)
                    continue;
                }
                bool flag1 = false;
                if (selectedItems.Count == 1)
                {
                  IDBAttribute byId2 = relation.Attributes.FindByID(attributeId);
                  if (byId2 != null && byId2.Value is MeasuredValue aMeasureValue)
                  {
                    measureDescriptorList.Clear();
                    foreach (MeasureDescriptor measure in MeasureHelper.Measures)
                    {
                      if (measure.MeasureID == aMeasureValue.MeasureID)
                        measureDescriptorList.Add(measure);
                    }
                    measureForm.Text = string.Format(LocalizationHolder.rm.GetString("Pdm_127"), (object) str);
                    flag1 = measureForm.ExecuteDialog(ref aMeasureValue, measureDescriptorList.ToArray()) == DialogResult.OK;
                    if (!flag1)
                      return;
                  }
                }
                bool flag2 = false;
                IDBRelation dbRelation = (IDBRelation) null;
                try
                {
                  NewRelationProperties properties = new NewRelationProperties(itemData2.Value, this.FContextComposition, objectInfo.ID)
                  {
                    PartObjectID = objectInfo.ObjectID
                  } with
                  {
                    ValuesList = new AttributeValues[1]
                    {
                      new AttributeValues(this._contextAttr.AttrID, FieldTypes.ftInteger, MultiValueModes.MultiValuesFromList, new object[1]
                      {
                        (object) num1
                      })
                    }
                  };
                  dbRelation = relationCollection.Create(properties);
                }
                catch (Exception ex)
                {
                  flag2 = true;
                  int num2 = (int) MessageBox.Show(ex.Message, LocalizationHolder.rm.GetString(sc_16722.ssp_pdm_16727()), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                Application.DoEvents();
                Thread.Sleep(10);
                if (!flag2)
                {
                  if (flag1)
                    dbRelation.Attributes.FindByID(attributeId).Value = (object) aMeasureValue;
                  MyElement myElement = new MyElement((object) relation.RelationID, string.Empty, byId1.Value);
                  this.FRollback.Add(dbRelation.RelationID, myElement);
                  byId1.Value = (object) 1L;
                  relationIDs2.Add(itemData2.Value);
                  relationIDs1.Add(dbRelation.RelationID);
                  projIDs.Add(dbRelation.ProjID);
                  relTypeIDs.Add(dbRelation.RelationType);
                  this._newRelations.Add(dbRelation.RelationID);
                  this.needTechComposition = true;
                  IDBAttribute byId3 = dbRelation.Attributes.FindByID(sessionKeeper.Session.IdentHelper.SubstitutesGroupNoID);
                  if (byId3 != null)
                    byId3.Value = (object) 0;
                  IDBAttribute byId4 = dbRelation.Attributes.FindByID(sessionKeeper.Session.IdentHelper.SubstituteInGroup);
                  if (byId4 != null)
                    byId4.Value = (object) 0;
                  MeasuredValue measuredValue = (MeasuredValue) null;
                  Application.DoEvents();
                  Thread.Sleep(10);
                  if (selectedItems.Count == 1)
                  {
                    IDBAttribute byId5 = dbRelation.Attributes.FindByID(attributeId);
                    if (byId5 != null)
                      measuredValue = byId5.Value as MeasuredValue;
                  }
                }
              }
            }
          }
        }
      }
      if (progressForm != null)
        progressForm.ProgressValue = progressForm.Maximum;
    }
    finally
    {
      this.Enabled = true;
      this.btnCancel.Enabled = true;
      if (progressForm != null)
      {
        progressForm.CanCloseForm = true;
        progressForm.Close();
        progressForm.Dispose();
        progressForm = (ProgressForm) null;
      }
    }
    try
    {
      this.Enabled = false;
      this.Invalidate();
      this.Update();
      Application.DoEvents();
      Thread.Sleep(100);
      progressForm = ProgressForm.Execute(LocalizationHolder.rm.GetString("Pdm_128"), LocalizationHolder.rm.GetString("Pdm_129"), 0, 2, false, string.Empty, (EventHandler) null);
      ContextCompositionCreatorForm._pluginsService.RegisterClientPlugin(ContextCompositionCreatorForm._pluginData.PluginGuid, (IClientPluginsDataTransfer) ContextCompositionCreatorForm._pluginData);
      ContextCompositionCreatorForm._pluginData.CurrentSet = 2;
      this.treeContextComposition.FocusedNode = this.treeContextComposition.RootNode;
      DBRelationsEventArgs e1 = new DBRelationsEventArgs("RelationsChanged", (IList<long>) relationIDs2);
      if (this.FViewServices.GetService(typeof (INotificationService)) is INotificationService service1)
        service1.FireEvent((object) null, (NotificationEventArgs) e1);
      progressForm.ProgressValue = 1;
      DBRelationsEventArgs e2 = new DBRelationsEventArgs("RelationsCreated", (IList<long>) relationIDs1, (IList<long>) projIDs, (IList<int>) null, (IList<int>) relTypeIDs);
      if (this.FViewServices.GetService(typeof (INotificationService)) is INotificationService service2)
        service2.FireEvent((object) null, (NotificationEventArgs) e2);
      progressForm.ProgressValue = 2;
    }
    finally
    {
      this.Enabled = true;
      ContextCompositionCreatorForm._pluginData.CurrentSet = 0;
      ContextCompositionCreatorForm._pluginsService.UnregisterClientPlugin(ContextCompositionCreatorForm._pluginData.PluginGuid);
      if (progressForm != null)
      {
        progressForm.CanCloseForm = true;
        progressForm.Close();
        progressForm.Dispose();
      }
      this.FIsChanged = true;
      this.UpdateControls();
    }
  }

  private MyElement FindSingleRelation(long newPrjLinkID)
  {
    if (!this.FRollback.ContainsKey(newPrjLinkID))
      return (MyElement) null;
    MyElement singleRelation = this.FRollback[newPrjLinkID];
    if (singleRelation == null)
      return (MyElement) null;
    IEnumerator<KeyValuePair<long, MyElement>> enumerator = this.FRollback.GetEnumerator();
    if (enumerator == null)
      return (MyElement) null;
    enumerator.Reset();
    while (enumerator.MoveNext())
    {
      if (enumerator.Current.Value != singleRelation && (long) enumerator.Current.Value.Value == (long) singleRelation.Value)
        return (MyElement) null;
    }
    return singleRelation;
  }

  private long PerformRollBack(long newPrjLinkID)
  {
    long num = 0;
    MyElement singleRelation = this.FindSingleRelation(newPrjLinkID);
    try
    {
      if (singleRelation == null)
        return num;
      long aRelationID = (long) singleRelation.Value;
      long tag = (long) singleRelation.Tag;
      num = aRelationID;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
        sessionKeeper.Session.GetRelation(aRelationID).GetAttributeByID(this._contextAttr.AttrID).Value = singleRelation.Tag;
    }
    finally
    {
      if (this.FRollback.ContainsKey(newPrjLinkID))
        this.FRollback.Remove(newPrjLinkID);
    }
    return num;
  }

  private void DoRemoveFromContext(object sender, EventArgs e)
  {
    ISelectedItems selectedItems = this.treeContextComposition.SelectedItems;
    if (selectedItems == null || selectedItems.Count == 0)
      return;
    List<long> longList1 = new List<long>(0);
    List<long> longList2 = new List<long>(0);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      for (int index = 0; index < selectedItems.Count; ++index)
      {
        if (!(selectedItems.GetItemData(index, typeof (IDBObjectID)) is IDBObjectID itemData1) || itemData1.Value != this.FContextComposition)
        {
          IDBRelationID itemData = selectedItems.GetItemData(index, typeof (IDBRelationID)) as IDBRelationID;
          if (itemData1 != null && itemData != null)
          {
            long num1 = itemData.Value;
            if (this._newRelations.Contains(num1))
            {
              sessionKeeper.Session.GetRelation(num1).Delete(0L);
              this.needTechComposition = true;
              long num2 = this.PerformRollBack(num1);
              if (num2 != 0L)
                longList2.Add(num2);
              this._newRelations.Remove(num1);
              longList1.Add(num1);
            }
          }
        }
      }
    }
    ProgressForm progressForm = (ProgressForm) null;
    try
    {
      progressForm = ProgressForm.Execute(LocalizationHolder.rm.GetString("Pdm_130"), LocalizationHolder.rm.GetString("Pdm_131"), 0, 2, false, string.Empty, (EventHandler) null);
      ContextCompositionCreatorForm._pluginsService.RegisterClientPlugin(ContextCompositionCreatorForm._pluginData.PluginGuid, (IClientPluginsDataTransfer) ContextCompositionCreatorForm._pluginData);
      ContextCompositionCreatorForm._pluginData.CurrentSet = 2;
      DBRelationsEventArgs e1 = new DBRelationsEventArgs("RelationsRemoved", (IList<long>) longList1.ToArray());
      if (this.FViewServices.GetService(typeof (INotificationService)) is INotificationService service1)
        service1.FireEvent((object) null, (NotificationEventArgs) e1);
      progressForm.ProgressValue = 1;
      DBRelationsEventArgs e2 = new DBRelationsEventArgs("RelationsChanged", (IList<long>) longList2.ToArray());
      if (this.FViewServices.GetService(typeof (INotificationService)) is INotificationService service2)
        service2.FireEvent((object) null, (NotificationEventArgs) e2);
      progressForm.ProgressValue = 2;
    }
    finally
    {
      ContextCompositionCreatorForm._pluginsService.UnregisterClientPlugin(ContextCompositionCreatorForm._pluginData.PluginGuid);
      if (progressForm != null)
      {
        progressForm.CanCloseForm = true;
        progressForm.Close();
        progressForm.Dispose();
      }
    }
    this.FIsChanged = true;
    this.UpdateControls();
  }

  private void DoRefreshComposition(object sender, EventArgs e)
  {
    try
    {
      ContextCompositionCreatorForm._pluginsService.RegisterClientPlugin(ContextCompositionCreatorForm._pluginData.PluginGuid, (IClientPluginsDataTransfer) ContextCompositionCreatorForm._pluginData);
      ContextCompositionCreatorForm._pluginData.CurrentSet = 0;
      this.treeComposition.Build((IDescriptor) new Intermech.Navigator.DBObjects.Descriptor(this.FParentItem));
      this.UpdateControls();
    }
    finally
    {
      ContextCompositionCreatorForm._pluginsService.UnregisterClientPlugin(ContextCompositionCreatorForm._pluginData.PluginGuid);
    }
  }

  private void DoRefreshContextComposition(object sender, EventArgs e)
  {
    try
    {
      ContextCompositionCreatorForm._pluginsService.RegisterClientPlugin(ContextCompositionCreatorForm._pluginData.PluginGuid, (IClientPluginsDataTransfer) ContextCompositionCreatorForm._pluginData);
      ContextCompositionCreatorForm._pluginData.CurrentSet = 1;
      this.treeContextComposition.Build((IDescriptor) new Intermech.Navigator.DBObjects.Descriptor(this.FContextComposition));
      this.UpdateControls();
    }
    finally
    {
      ContextCompositionCreatorForm._pluginsService.UnregisterClientPlugin(ContextCompositionCreatorForm._pluginData.PluginGuid);
    }
  }

  private void cbContext_SelectedIndexChanged(object sender, EventArgs e)
  {
    if (this.cbContext.SelectedIndex < 0 || !(this.cbContext.Items[this.cbContext.SelectedIndex] is MyElement myElement))
      return;
    ContextCompositionCreatorForm._pluginData._addContexts2[1] = (long) myElement.Value;
    this.DoRefreshContextComposition((object) this, (EventArgs) null);
  }

  private void ThreadSelectDesign()
  {
    lock (this.lockDesign)
    {
      if (this.tableDesign != null)
        this.tableDesign.Dispose();
      this.tableDesign = (DataTable) null;
      this.percentDesign = 0;
      if (this.stoppedComposition)
      {
        this.threadDesign = (Thread) null;
        Thread.CurrentThread.Abort();
        return;
      }
    }
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      ICompositionService customService;
      try
      {
        customService = (ICompositionService) sessionKeeper.Session.GetCustomService(typeof (ICompositionService));
      }
      catch
      {
        lock (this.lockDesign)
          this.percentDesign = 100;
        this.threadDesign = (Thread) null;
        Thread.CurrentThread.Abort();
        return;
      }
      HybridDictionary Tags = new HybridDictionary(0, true);
      Tags[(object) "{82E381A1-8952-416A-B303-F81BA2945F8F}"] = (object) true;
      Tags[(object) "{AB419A02-DE8A-4A8E-905A-D782F5B720E5}"] = (object) ContextCompositionCreatorForm._pluginData._addContexts;
      RuntimeSearchScheme compositionQuantityScheme = RuntimeSearchScheme.GetCompositionQuantityScheme(sessionKeeper.Session, (int[]) null, new int[1]
      {
        sessionKeeper.Session.IdentHelper.SPRelationTypeID
      });
      List<ColumnDescriptor> schemeDescriptors = RuntimeSearchScheme.GetCompositionQuantitySchemeDescriptors(sessionKeeper.Session);
      customService.CancelSelect(this.taskGuidDesign);
      customService.Select(sessionKeeper.Session.SessionGUID, this.FParentItem, compositionQuantityScheme, schemeDescriptors, this.taskGuidDesign, string.Empty, Tags);
      CompositionInfo info = customService.GetInfo(this.taskGuidDesign);
      while (info != null && !info.ErrorPresent && info.Percent < 100)
      {
        Thread.Sleep(25);
        lock (this.lockDesign)
        {
          this.percentDesign = info.Percent;
          if (this.stoppedComposition)
          {
            this.percentDesign = 0;
            customService.CancelSelect(this.taskGuidDesign);
            this.threadDesign = (Thread) null;
            Thread.CurrentThread.Abort();
            return;
          }
        }
        info = customService.GetInfo(this.taskGuidDesign);
        if (info != null)
        {
          lock (this.lockDesign)
          {
            this.tableDesign = info.Result as DataTable;
            this.percentDesign = info.Percent;
          }
        }
      }
      lock (this.lockDesign)
        this.percentDesign = 100;
    }
    this.threadDesign = (Thread) null;
    Thread.CurrentThread.Abort();
  }

  private void ThreadSelectTech()
  {
    lock (this.lockTech)
    {
      if (this.tableTech != null)
        this.tableTech.Dispose();
      this.tableTech = (DataTable) null;
      this.percentTech = 0;
      if (this.stoppedComposition)
      {
        this.threadTech = (Thread) null;
        Thread.CurrentThread.Abort();
        return;
      }
    }
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      ICompositionService customService;
      try
      {
        customService = (ICompositionService) sessionKeeper.Session.GetCustomService(typeof (ICompositionService));
      }
      catch
      {
        lock (this.lockDesign)
          this.percentDesign = 100;
        this.threadDesign = (Thread) null;
        Thread.CurrentThread.Abort();
        return;
      }
      HybridDictionary Tags = new HybridDictionary(0, true);
      Tags[(object) "{82E381A1-8952-416A-B303-F81BA2945F8F}"] = (object) true;
      Tags[(object) "{AB419A02-DE8A-4A8E-905A-D782F5B720E5}"] = (object) ContextCompositionCreatorForm._pluginData._addContexts2;
      RuntimeSearchScheme compositionQuantityScheme = RuntimeSearchScheme.GetCompositionQuantityScheme(sessionKeeper.Session, (int[]) null, new int[1]
      {
        sessionKeeper.Session.IdentHelper.SPRelationTypeID
      });
      List<ColumnDescriptor> schemeDescriptors = RuntimeSearchScheme.GetCompositionQuantitySchemeDescriptors(sessionKeeper.Session);
      customService.CancelSelect(this.taskGuidTech);
      customService.Select(sessionKeeper.Session.SessionGUID, this.FContextComposition, compositionQuantityScheme, schemeDescriptors, this.taskGuidTech, string.Empty, Tags);
      CompositionInfo info = customService.GetInfo(this.taskGuidTech);
      while (info != null && !info.ErrorPresent && info.Percent < 100)
      {
        Thread.Sleep(25);
        lock (this.lockTech)
        {
          this.percentDesign = info.Percent;
          if (this.stoppedComposition)
          {
            this.percentDesign = 0;
            customService.CancelSelect(this.taskGuidTech);
            this.threadTech = (Thread) null;
            Thread.CurrentThread.Abort();
            return;
          }
        }
        info = customService.GetInfo(this.taskGuidTech);
        if (info != null)
        {
          lock (this.lockTech)
          {
            this.tableTech = info.Result as DataTable;
            this.percentTech = info.Percent;
            if (info.Percent == 100)
            {
              if (!info.ErrorPresent)
              {
                if (info.Result != null)
                  this.needTechComposition = false;
              }
            }
          }
        }
      }
      lock (this.lockTech)
        this.percentTech = 100;
    }
    this.threadTech = (Thread) null;
    Thread.CurrentThread.Abort();
  }

  private void ContextCompositionCreatorForm_FormClosing(object sender, FormClosingEventArgs e)
  {
    if (e.CloseReason != CloseReason.UserClosing)
      return;
    this.DoCancelChanges((object) this, (EventArgs) null);
  }

  private void DoCalcBarsLabels(object sender, EventArgs e)
  {
    int num = this.panelProgress.ClientRectangle.Width / 2 - 24;
    this.progressBarDesign.Left = 8;
    this.progressBarDesign.Width = num;
    this.labelProgressDesign.Left = 8;
    this.labelProgressDesign.Width = num;
    this.progressBarTech.Left = num + 16 /*0x10*/;
    this.progressBarTech.Width = num;
    this.labelProgressTech.Left = num + 16 /*0x10*/;
    this.labelProgressTech.Width = num;
  }

  private void DoOpenCloseRelPropertyGrid(object sender, LinkLabelLinkClickedEventArgs e)
  {
    this.FIsRelPropertiesActive = !this.FIsRelPropertiesActive;
    this.UpdateControls();
  }

  private void relPropertyGrid_SelectedGridItemChanged(
    object sender,
    SelectedGridItemChangedEventArgs e)
  {
    this.UpdateControls();
  }

  private void relPropertyGrid_SelectedObjectsChanged(object sender, EventArgs e)
  {
    this.UpdateControls();
  }

  private void DoCheckChangedRelation(object sender, NavigatorTreeNodeEventArgs e)
  {
    if (!this.FIsRelPropertiesActive || !this.relPropertyGrid.IsChanged)
      return;
    NavigatorTreeNode node = e.Node;
    if (node == null || node.Tag == null || MessageBox.Show(ContextCompositionCreatorForm.Dialog1, ContextCompositionCreatorForm.Dialog0, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
      return;
    this.btnRelApply_Click((object) this, (EventArgs) null);
  }

  private void DoInsertRelationInGrid(object sender, NavigatorTreeNodeEventArgs e)
  {
    bool flag = true;
    try
    {
      NavigatorTreeNode focusedNode = this.treeContextComposition.FocusedNode;
      if (focusedNode == null)
        return;
      NavigatorTreeNode navigatorTreeNode = focusedNode;
      if (navigatorTreeNode == null || navigatorTreeNode.NodeID == null)
        return;
      INode nodeHandler = this.treeContextComposition.GetNodeHandler(focusedNode);
      if (nodeHandler == null || !(nodeHandler.GetData(navigatorTreeNode.NodeID, typeof (IDBRelationID)) is IDBRelationID data) || data.Value == -1L)
        return;
      this.relPropertyGrid.Tag = (object) data.Value;
      this.relPropertyGrid.Load(data.Value, AttributableElements.Relation, GetAttributeValuesModes.IncludeName | GetAttributeValuesModes.IncludeGroupName | GetAttributeValuesModes.IncludeDescriptions | GetAttributeValuesModes.CheckVisibility | GetAttributeValuesModes.IncludeOnlyInvisible, false, typeof (ObjectMainAttributesGridTab));
      flag = false;
    }
    finally
    {
      if (flag)
      {
        this.relPropertyGrid.Tag = (object) null;
        this.relPropertyGrid.Load(-1L, AttributableElements.Relation, GetAttributeValuesModes.IncludeName | GetAttributeValuesModes.IncludeGroupName | GetAttributeValuesModes.IncludeDescriptions | GetAttributeValuesModes.CheckVisibility | GetAttributeValuesModes.IncludeOnlyInvisible, false, typeof (ObjectMainAttributesGridTab));
      }
    }
  }

  private void btnRelApply_Click(object sender, EventArgs e)
  {
    this.UpdateControls();
    if (!this.FIsRelPropertiesActive || !this.relPropertyGrid.IsChanged)
      return;
    this.relPropertyGrid.Save();
    try
    {
      ContextCompositionCreatorForm._pluginsService.RegisterClientPlugin(ContextCompositionCreatorForm._pluginData.PluginGuid, (IClientPluginsDataTransfer) ContextCompositionCreatorForm._pluginData);
      ContextCompositionCreatorForm._pluginData.CurrentSet = 2;
      DBRelationsEventArgs e1 = new DBRelationsEventArgs("RelationsChanged", this.relPropertyGrid.Id);
      if (!(this.FViewServices.GetService(typeof (INotificationService)) is INotificationService service))
        return;
      service.FireEvent((object) null, (NotificationEventArgs) e1);
    }
    finally
    {
      ContextCompositionCreatorForm._pluginData.CurrentSet = 0;
      ContextCompositionCreatorForm._pluginsService.UnregisterClientPlugin(ContextCompositionCreatorForm._pluginData.PluginGuid);
      this.UpdateControls();
    }
  }

  private void btnRelCancel_Click(object sender, EventArgs e)
  {
    this.UpdateControls();
    if (!this.FIsRelPropertiesActive || !this.relPropertyGrid.IsChanged)
      return;
    if (this.relPropertyGrid.Tag != null)
      this.relPropertyGrid.Load((long) this.relPropertyGrid.Tag, AttributableElements.Relation, GetAttributeValuesModes.IncludeName | GetAttributeValuesModes.IncludeGroupName | GetAttributeValuesModes.IncludeDescriptions | GetAttributeValuesModes.CheckVisibility | GetAttributeValuesModes.IncludeOnlyInvisible, false, typeof (ObjectMainAttributesGridTab));
    this.UpdateControls();
  }

  private void DoCancel(object sender, LinkLabelLinkClickedEventArgs e)
  {
    this.StopExpandDesign();
    this.StopExpandTech();
    this.stoppedComposition = true;
  }

  protected virtual void ToolbarRendererChanged(object sender, EventArgs e)
  {
    this.toolBarLeft.Renderer = (sender as BarManager).Renderer;
  }

  private void FindInTreeMenuButtonItem_Click(object sender, EventArgs e)
  {
    if (this.treeComposition.FocusedNode == null)
      return;
    TreeViewSearchForm.ShowFor(this.treeComposition);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && ServicesManager.GetService(typeof (BarManager)) is BarManager service)
    {
      this.toolBarLeft.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
      service.RendererChanged -= new EventHandler(this.ToolbarRendererChanged);
    }
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ContextCompositionCreatorForm));
    this.scFrame = new SplitContainer();
    this.treeComposition = new NavigatorTreeView();
    this.toolBarLeft = new Intermech.Bars.ToolBar();
    this.imagesToolbars = new ImageList(this.components);
    this.btnAdd = new ButtonItem();
    this.btnDelete = new ButtonItem();
    this.panelCompositionTop = new Panel();
    this.labelComposition = new Label();
    this.menuComposition = new MenuBar();
    this.contextMenuComposition = new ContextMenuBarItem();
    this.mnpAddToCC = new MenuButtonItem();
    this.mnpRefresh = new MenuButtonItem();
    this.treeNewComposition = new NavigatorTreeView();
    this.panel1 = new Panel();
    this.btnRelCancel = new Button();
    this.btnRelApply = new Button();
    this.relPropertyGrid = new ObjectPropertyGrid();
    this.TectContainer = new SplitContainer();
    this.treeContextComposition = new NavigatorTreeView();
    this.labelRelProperties = new LinkLabel();
    this.menuContextComposition = new MenuBar();
    this.contextMenuContextComposition = new ContextMenuBarItem();
    this.mnpDelFromCC = new MenuButtonItem();
    this.mnpRefreshTC = new MenuButtonItem();
    this.panelContextTop = new Panel();
    this.labelCC = new Label();
    this.cbContext = new ComboBox();
    this.panelBottom = new Panel();
    this.btnPrev = new Button();
    this.btnCancel = new Button();
    this.btnNext = new Button();
    this.panelPage1 = new Panel();
    this.panelPage0 = new Panel();
    this.edContext = new TextBox();
    this.label1 = new Label();
    this.labelHint = new Label();
    this.btnBrowseTemplate = new Button();
    this.edTemplate = new TextBox();
    this.panelPage3 = new Panel();
    this.labelHint3 = new Label();
    this.panelPage4 = new Panel();
    this.pictureInfo = new PictureBox();
    this.labelHnt5 = new Label();
    this.panelPage2 = new Panel();
    this.panelHint = new Panel();
    this.labelHint2 = new Label();
    this.linkLabel = new LinkLabel();
    this.pictureObject = new PictureBox();
    this.panelAnalyze = new Panel();
    this.labelAnalyzeInfo = new Label();
    this.progressBarAnalyze = new ProgressBar();
    this.panelProgress = new Panel();
    this.labelProgressTech = new Label();
    this.progressBarTech = new ProgressBar();
    this.labelProgressDesign = new Label();
    this.progressBarDesign = new ProgressBar();
    this.labelAnalyze = new Label();
    this.treeAnalyze = new TreeList();
    this.columnVersion = new TreeListColumn();
    this.columnDesign = new TreeListColumn();
    this.columnDesignQuantity = new TreeListColumn();
    this.columnTechQuantity = new TreeListColumn();
    this.panelTopProgress = new Panel();
    this.labelProgress = new Label();
    this.edQuantity = new NumericUpDown();
    this.labelHint6 = new Label();
    this._findInTreeMenuButtonItem = new MenuButtonItem();
    this.scFrame.BeginInit();
    this.scFrame.Panel1.SuspendLayout();
    this.scFrame.Panel2.SuspendLayout();
    this.scFrame.SuspendLayout();
    this.treeComposition.BeginInit();
    this.panelCompositionTop.SuspendLayout();
    this.treeNewComposition.BeginInit();
    this.panel1.SuspendLayout();
    this.TectContainer.BeginInit();
    this.TectContainer.Panel1.SuspendLayout();
    this.TectContainer.Panel2.SuspendLayout();
    this.TectContainer.SuspendLayout();
    this.treeContextComposition.BeginInit();
    this.panelContextTop.SuspendLayout();
    this.panelBottom.SuspendLayout();
    this.panelPage1.SuspendLayout();
    this.panelPage0.SuspendLayout();
    this.panelPage3.SuspendLayout();
    this.panelPage4.SuspendLayout();
    ((ISupportInitialize) this.pictureInfo).BeginInit();
    this.panelPage2.SuspendLayout();
    this.panelHint.SuspendLayout();
    ((ISupportInitialize) this.pictureObject).BeginInit();
    this.panelAnalyze.SuspendLayout();
    this.panelProgress.SuspendLayout();
    this.treeAnalyze.BeginInit();
    this.panelTopProgress.SuspendLayout();
    this.edQuantity.BeginInit();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.scFrame, "scFrame");
    this.scFrame.Name = "scFrame";
    this.scFrame.Panel1.Controls.Add((Control) this.treeComposition);
    this.scFrame.Panel1.Controls.Add((Control) this.toolBarLeft);
    this.scFrame.Panel1.Controls.Add((Control) this.panelCompositionTop);
    this.scFrame.Panel1.Controls.Add((Control) this.menuComposition);
    componentResourceManager.ApplyResources((object) this.scFrame.Panel1, "scFrame.Panel1");
    this.scFrame.Panel2.Controls.Add((Control) this.TectContainer);
    this.scFrame.Panel2.Controls.Add((Control) this.panelContextTop);
    componentResourceManager.ApplyResources((object) this.scFrame.Panel2, "scFrame.Panel2");
    this.treeComposition.AllowDrop = true;
    this.treeComposition.AllowUserPinnedColumns = false;
    this.treeComposition.DisableCheckedOutColumn = true;
    this.treeComposition.DisableIMContextMenu = true;
    this.treeComposition.DisableKeyDownEvents = true;
    this.treeComposition.DisableKeyUpEvents = true;
    componentResourceManager.ApplyResources((object) this.treeComposition, "treeComposition");
    this.treeComposition.HeaderStyle.HorzAlignment = (StringAlignment) componentResourceManager.GetObject("treeComposition.HeaderStyle.HorzAlignment");
    this.treeComposition.ImageList = (ImageList) null;
    this.treeComposition.LineStyle = Infralution.Controls.VirtualTree.LineStyle.Dot;
    this.treeComposition.MultiSelect = true;
    this.treeComposition.Name = "treeComposition";
    this.menuComposition.SetPopupMenu((Control) this.treeComposition, (MenuBarItem) this.contextMenuComposition);
    this.treeComposition.RowEvenStyle.WordWrap = (bool) componentResourceManager.GetObject("treeComposition.RowEvenStyle.WordWrap");
    this.treeComposition.RowOddStyle.WordWrap = (bool) componentResourceManager.GetObject("treeComposition.RowOddStyle.WordWrap");
    this.treeComposition.RowSelectedStyle.BackColor = SystemColors.Highlight;
    this.treeComposition.RowSelectedStyle.WordWrap = (bool) componentResourceManager.GetObject("treeComposition.RowSelectedStyle.WordWrap");
    this.treeComposition.RowSelectedUnfocusedStyle.BackColor = SystemColors.Highlight;
    this.treeComposition.RowStyle.BorderColor = SystemColors.Control;
    this.treeComposition.RowStyle.BorderStyle = Border3DStyle.Adjust;
    this.treeComposition.RowStyle.BorderWidth = 1;
    this.treeComposition.RowStyle.WordWrap = (bool) componentResourceManager.GetObject("treeComposition.RowStyle.WordWrap");
    this.treeComposition.SelectBeforeEdit = true;
    this.treeComposition.ShowRootRow = false;
    this.treeComposition.SuppressErrorMessages = true;
    this.treeComposition.SelectedItemsChanged += new EventHandler(this.DoUpdateControls);
    this.toolBarLeft.AddRemoveButtonsVisible = false;
    this.toolBarLeft.AllowHorizontalDock = false;
    componentResourceManager.ApplyResources((object) this.toolBarLeft, "toolBarLeft");
    this.toolBarLeft.DockLine = 3;
    this.toolBarLeft.DrawActionsButton = false;
    this.toolBarLeft.Flow = ToolBarLayout.Vertical;
    this.toolBarLeft.FullMenus = true;
    this.toolBarLeft.Guid = new Guid("ba855ba6-35ae-4775-b979-b76ac70a54e0");
    this.toolBarLeft.Hidden = false;
    this.toolBarLeft.ImageList = this.imagesToolbars;
    this.toolBarLeft.Items.AddRange(new ToolbarItemBase[2]
    {
      (ToolbarItemBase) this.btnAdd,
      (ToolbarItemBase) this.btnDelete
    });
    this.toolBarLeft.MinimumFloatingSize = new Size(250, 30);
    this.toolBarLeft.Name = "toolBarLeft";
    this.toolBarLeft.Overflow = ToolBarOverflow.Wrap;
    this.toolBarLeft.Stretch = true;
    this.toolBarLeft.Tearable = false;
    this.imagesToolbars.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imagesToolbars.ImageStream");
    this.imagesToolbars.TransparentColor = Color.Transparent;
    this.imagesToolbars.Images.SetKeyName(0, "arrow_right_blue.ico");
    this.imagesToolbars.Images.SetKeyName(1, "arrow_left_blue.ico");
    this.imagesToolbars.Images.SetKeyName(2, "refresh.ico");
    componentResourceManager.ApplyResources((object) this.btnAdd, "btnAdd");
    this.btnAdd.ImageIndex = 0;
    this.btnAdd.Click += new EventHandler(this.DoAddToContext);
    componentResourceManager.ApplyResources((object) this.btnDelete, "btnDelete");
    this.btnDelete.ImageIndex = 1;
    this.btnDelete.Click += new EventHandler(this.DoRemoveFromContext);
    this.panelCompositionTop.Controls.Add((Control) this.labelComposition);
    componentResourceManager.ApplyResources((object) this.panelCompositionTop, "panelCompositionTop");
    this.panelCompositionTop.Name = "panelCompositionTop";
    componentResourceManager.ApplyResources((object) this.labelComposition, "labelComposition");
    this.labelComposition.Name = "labelComposition";
    componentResourceManager.ApplyResources((object) this.menuComposition, "menuComposition");
    this.menuComposition.Guid = new Guid("0909a734-928b-4c5d-9a6d-05be64690c06");
    this.menuComposition.Hidden = false;
    this.menuComposition.ImageList = this.imagesToolbars;
    this.menuComposition.Items.AddRange(new ToolbarItemBase[1]
    {
      (ToolbarItemBase) this.contextMenuComposition
    });
    this.menuComposition.Name = "menuComposition";
    this.menuComposition.OwnerForm = (Form) this;
    componentResourceManager.ApplyResources((object) this.contextMenuComposition, "contextMenuComposition");
    this.contextMenuComposition.Items.AddRange(new ToolbarItemBase[3]
    {
      (ToolbarItemBase) this.mnpAddToCC,
      (ToolbarItemBase) this.mnpRefresh,
      (ToolbarItemBase) this._findInTreeMenuButtonItem
    });
    this.contextMenuComposition.ShowText = true;
    componentResourceManager.ApplyResources((object) this.mnpAddToCC, "mnpAddToCC");
    this.mnpAddToCC.ImageIndex = 0;
    this.mnpAddToCC.ShowText = true;
    this.mnpAddToCC.Click += new EventHandler(this.DoAddToContext);
    this.mnpRefresh.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.mnpRefresh, "mnpRefresh");
    this.mnpRefresh.ImageIndex = 2;
    this.mnpRefresh.ShowText = true;
    this.mnpRefresh.Click += new EventHandler(this.DoRefreshComposition);
    this.treeNewComposition.AllowDrop = true;
    this.treeNewComposition.AllowMultiSelect = false;
    this.treeNewComposition.AllowUserPinnedColumns = false;
    this.treeNewComposition.DisableCheckedOutColumn = true;
    this.treeNewComposition.DisableIMContextMenu = true;
    this.treeNewComposition.DisableKeyDownEvents = true;
    this.treeNewComposition.DisableKeyUpEvents = true;
    componentResourceManager.ApplyResources((object) this.treeNewComposition, "treeNewComposition");
    this.treeNewComposition.HeaderStyle.HorzAlignment = (StringAlignment) componentResourceManager.GetObject("treeNewComposition.HeaderStyle.HorzAlignment");
    this.treeNewComposition.ImageList = (ImageList) null;
    this.treeNewComposition.LineStyle = Infralution.Controls.VirtualTree.LineStyle.Dot;
    this.treeNewComposition.Name = "treeNewComposition";
    this.menuComposition.SetPopupMenu((Control) this.treeNewComposition, (MenuBarItem) this.contextMenuComposition);
    this.treeNewComposition.RowEvenStyle.WordWrap = (bool) componentResourceManager.GetObject("treeNewComposition.RowEvenStyle.WordWrap");
    this.treeNewComposition.RowOddStyle.WordWrap = (bool) componentResourceManager.GetObject("treeNewComposition.RowOddStyle.WordWrap");
    this.treeNewComposition.RowSelectedStyle.BackColor = SystemColors.Highlight;
    this.treeNewComposition.RowSelectedStyle.WordWrap = (bool) componentResourceManager.GetObject("treeNewComposition.RowSelectedStyle.WordWrap");
    this.treeNewComposition.RowSelectedUnfocusedStyle.BackColor = SystemColors.Highlight;
    this.treeNewComposition.RowStyle.BorderColor = SystemColors.Control;
    this.treeNewComposition.RowStyle.BorderStyle = Border3DStyle.Adjust;
    this.treeNewComposition.RowStyle.BorderWidth = 1;
    this.treeNewComposition.RowStyle.WordWrap = (bool) componentResourceManager.GetObject("treeNewComposition.RowStyle.WordWrap");
    this.treeNewComposition.SelectBeforeEdit = true;
    this.treeNewComposition.ShowRootRow = false;
    this.treeNewComposition.SuppressErrorMessages = true;
    this.treeNewComposition.SelectedItemsChanged += new EventHandler(this.DoUpdateControls);
    this.panel1.BorderStyle = BorderStyle.Fixed3D;
    this.panel1.Controls.Add((Control) this.btnRelCancel);
    this.panel1.Controls.Add((Control) this.btnRelApply);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    this.menuComposition.SetPopupMenu((Control) this.panel1, (MenuBarItem) this.contextMenuComposition);
    componentResourceManager.ApplyResources((object) this.btnRelCancel, "btnRelCancel");
    this.btnRelCancel.Cursor = Cursors.Default;
    this.btnRelCancel.Name = "btnRelCancel";
    this.menuComposition.SetPopupMenu((Control) this.btnRelCancel, (MenuBarItem) this.contextMenuComposition);
    this.btnRelCancel.Click += new EventHandler(this.btnRelCancel_Click);
    componentResourceManager.ApplyResources((object) this.btnRelApply, "btnRelApply");
    this.btnRelApply.Cursor = Cursors.Default;
    this.btnRelApply.Name = "btnRelApply";
    this.menuComposition.SetPopupMenu((Control) this.btnRelApply, (MenuBarItem) this.contextMenuComposition);
    this.btnRelApply.Click += new EventHandler(this.btnRelApply_Click);
    this.relPropertyGrid.CommandsActiveLinkColor = SystemColors.ActiveCaption;
    this.relPropertyGrid.CommandsDisabledLinkColor = SystemColors.ControlDark;
    this.relPropertyGrid.CommandsLinkColor = SystemColors.ActiveCaption;
    componentResourceManager.ApplyResources((object) this.relPropertyGrid, "relPropertyGrid");
    this.relPropertyGrid.InternalMenuEnabled = true;
    this.relPropertyGrid.LockTypeChange = true;
    this.relPropertyGrid.Name = "relPropertyGrid";
    this.menuComposition.SetPopupMenu((Control) this.relPropertyGrid, (MenuBarItem) this.contextMenuComposition);
    this.relPropertyGrid.PropertySort = PropertySort.Alphabetical;
    this.relPropertyGrid.ToolbarVisible = false;
    this.relPropertyGrid.SelectedGridItemChanged += new SelectedGridItemChangedEventHandler(this.relPropertyGrid_SelectedGridItemChanged);
    this.relPropertyGrid.SelectedObjectsChanged += new EventHandler(this.relPropertyGrid_SelectedObjectsChanged);
    componentResourceManager.ApplyResources((object) this.TectContainer, "TectContainer");
    this.TectContainer.Name = "TectContainer";
    this.TectContainer.Panel1.Controls.Add((Control) this.treeContextComposition);
    this.TectContainer.Panel1.Controls.Add((Control) this.labelRelProperties);
    this.TectContainer.Panel1.Controls.Add((Control) this.menuContextComposition);
    this.TectContainer.Panel2.Controls.Add((Control) this.relPropertyGrid);
    this.TectContainer.Panel2.Controls.Add((Control) this.panel1);
    this.treeContextComposition.AllowDrop = true;
    this.treeContextComposition.AllowUserPinnedColumns = false;
    this.treeContextComposition.DisableCheckedOutColumn = true;
    this.treeContextComposition.DisableIMContextMenu = true;
    this.treeContextComposition.DisableKeyDownEvents = true;
    this.treeContextComposition.DisableKeyUpEvents = true;
    componentResourceManager.ApplyResources((object) this.treeContextComposition, "treeContextComposition");
    this.treeContextComposition.HeaderStyle.HorzAlignment = (StringAlignment) componentResourceManager.GetObject("treeContextComposition.HeaderStyle.HorzAlignment");
    this.treeContextComposition.ImageList = (ImageList) null;
    this.treeContextComposition.LineStyle = Infralution.Controls.VirtualTree.LineStyle.Dot;
    this.treeContextComposition.MultiSelect = true;
    this.treeContextComposition.Name = "treeContextComposition";
    this.menuContextComposition.SetPopupMenu((Control) this.treeContextComposition, (MenuBarItem) this.contextMenuContextComposition);
    this.treeContextComposition.RowEvenStyle.WordWrap = (bool) componentResourceManager.GetObject("treeContextComposition.RowEvenStyle.WordWrap");
    this.treeContextComposition.RowOddStyle.WordWrap = (bool) componentResourceManager.GetObject("treeContextComposition.RowOddStyle.WordWrap");
    this.treeContextComposition.RowSelectedStyle.BackColor = SystemColors.Highlight;
    this.treeContextComposition.RowSelectedStyle.WordWrap = (bool) componentResourceManager.GetObject("treeContextComposition.RowSelectedStyle.WordWrap");
    this.treeContextComposition.RowSelectedUnfocusedStyle.BackColor = SystemColors.Highlight;
    this.treeContextComposition.RowStyle.BorderColor = SystemColors.Control;
    this.treeContextComposition.RowStyle.BorderStyle = Border3DStyle.Adjust;
    this.treeContextComposition.RowStyle.BorderWidth = 1;
    this.treeContextComposition.RowStyle.WordWrap = (bool) componentResourceManager.GetObject("treeContextComposition.RowStyle.WordWrap");
    this.treeContextComposition.SelectBeforeEdit = true;
    this.treeContextComposition.ShowRootRow = false;
    this.treeContextComposition.SuppressErrorMessages = true;
    this.treeContextComposition.SelectedItemsChanged += new EventHandler(this.DoUpdateControls);
    this.labelRelProperties.ActiveLinkColor = Color.Crimson;
    this.labelRelProperties.BackColor = SystemColors.Control;
    this.labelRelProperties.BorderStyle = BorderStyle.Fixed3D;
    this.labelRelProperties.Cursor = Cursors.Default;
    componentResourceManager.ApplyResources((object) this.labelRelProperties, "labelRelProperties");
    this.labelRelProperties.ForeColor = SystemColors.Control;
    this.labelRelProperties.LinkColor = Color.Blue;
    this.labelRelProperties.Name = "labelRelProperties";
    this.labelRelProperties.VisitedLinkColor = Color.Blue;
    this.labelRelProperties.LinkClicked += new LinkLabelLinkClickedEventHandler(this.DoOpenCloseRelPropertyGrid);
    componentResourceManager.ApplyResources((object) this.menuContextComposition, "menuContextComposition");
    this.menuContextComposition.Guid = new Guid("0909a734-928b-4c5d-9a6d-05be64690c06");
    this.menuContextComposition.Hidden = false;
    this.menuContextComposition.ImageList = this.imagesToolbars;
    this.menuContextComposition.Items.AddRange(new ToolbarItemBase[1]
    {
      (ToolbarItemBase) this.contextMenuContextComposition
    });
    this.menuContextComposition.Name = "menuContextComposition";
    this.menuContextComposition.OwnerForm = (Form) this;
    componentResourceManager.ApplyResources((object) this.contextMenuContextComposition, "contextMenuContextComposition");
    this.contextMenuContextComposition.Items.AddRange(new ToolbarItemBase[2]
    {
      (ToolbarItemBase) this.mnpDelFromCC,
      (ToolbarItemBase) this.mnpRefreshTC
    });
    this.contextMenuContextComposition.ShowText = true;
    componentResourceManager.ApplyResources((object) this.mnpDelFromCC, "mnpDelFromCC");
    this.mnpDelFromCC.ImageIndex = 1;
    this.mnpDelFromCC.ShowText = true;
    this.mnpDelFromCC.Click += new EventHandler(this.DoRemoveFromContext);
    componentResourceManager.ApplyResources((object) this.mnpRefreshTC, "mnpRefreshTC");
    this.mnpRefreshTC.ImageIndex = 2;
    this.mnpRefreshTC.ShowText = true;
    this.mnpRefreshTC.Click += new EventHandler(this.DoRefreshContextComposition);
    this.panelContextTop.Controls.Add((Control) this.labelCC);
    this.panelContextTop.Controls.Add((Control) this.cbContext);
    componentResourceManager.ApplyResources((object) this.panelContextTop, "panelContextTop");
    this.panelContextTop.Name = "panelContextTop";
    componentResourceManager.ApplyResources((object) this.labelCC, "labelCC");
    this.labelCC.Name = "labelCC";
    componentResourceManager.ApplyResources((object) this.cbContext, "cbContext");
    this.cbContext.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbContext.FormattingEnabled = true;
    this.cbContext.Name = "cbContext";
    this.cbContext.SelectedIndexChanged += new EventHandler(this.cbContext_SelectedIndexChanged);
    this.panelBottom.BorderStyle = BorderStyle.Fixed3D;
    this.panelBottom.Controls.Add((Control) this.btnPrev);
    this.panelBottom.Controls.Add((Control) this.btnCancel);
    this.panelBottom.Controls.Add((Control) this.btnNext);
    componentResourceManager.ApplyResources((object) this.panelBottom, "panelBottom");
    this.panelBottom.Name = "panelBottom";
    componentResourceManager.ApplyResources((object) this.btnPrev, "btnPrev");
    this.btnPrev.Cursor = Cursors.Default;
    this.btnPrev.Name = "btnPrev";
    this.btnPrev.Click += new EventHandler(this.Goto_PrevPage);
    componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
    this.btnCancel.Cursor = Cursors.Default;
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.Click += new EventHandler(this.DoCancelChanges);
    componentResourceManager.ApplyResources((object) this.btnNext, "btnNext");
    this.btnNext.Cursor = Cursors.Default;
    this.btnNext.Name = "btnNext";
    this.btnNext.Click += new EventHandler(this.Goto_NextPage);
    this.panelPage1.Controls.Add((Control) this.scFrame);
    componentResourceManager.ApplyResources((object) this.panelPage1, "panelPage1");
    this.panelPage1.Name = "panelPage1";
    this.panelPage0.Controls.Add((Control) this.edContext);
    this.panelPage0.Controls.Add((Control) this.label1);
    this.panelPage0.Controls.Add((Control) this.labelHint);
    this.panelPage0.Controls.Add((Control) this.btnBrowseTemplate);
    this.panelPage0.Controls.Add((Control) this.edTemplate);
    componentResourceManager.ApplyResources((object) this.panelPage0, "panelPage0");
    this.panelPage0.Name = "panelPage0";
    componentResourceManager.ApplyResources((object) this.edContext, "edContext");
    this.edContext.BackColor = SystemColors.Window;
    this.edContext.Name = "edContext";
    this.edContext.ReadOnly = true;
    componentResourceManager.ApplyResources((object) this.label1, "label1");
    this.label1.BackColor = Color.Transparent;
    this.label1.Name = "label1";
    componentResourceManager.ApplyResources((object) this.labelHint, "labelHint");
    this.labelHint.BackColor = Color.Transparent;
    this.labelHint.Name = "labelHint";
    componentResourceManager.ApplyResources((object) this.btnBrowseTemplate, "btnBrowseTemplate");
    this.btnBrowseTemplate.Cursor = Cursors.Default;
    this.btnBrowseTemplate.Name = "btnBrowseTemplate";
    this.btnBrowseTemplate.Click += new EventHandler(this.DoBrowseTemplate);
    componentResourceManager.ApplyResources((object) this.edTemplate, "edTemplate");
    this.edTemplate.BackColor = SystemColors.Window;
    this.edTemplate.Name = "edTemplate";
    this.edTemplate.ReadOnly = true;
    this.panelPage3.Controls.Add((Control) this.treeNewComposition);
    this.panelPage3.Controls.Add((Control) this.labelHint3);
    componentResourceManager.ApplyResources((object) this.panelPage3, "panelPage3");
    this.panelPage3.Name = "panelPage3";
    this.labelHint3.BackColor = Color.Transparent;
    componentResourceManager.ApplyResources((object) this.labelHint3, "labelHint3");
    this.labelHint3.Name = "labelHint3";
    this.panelPage4.Controls.Add((Control) this.pictureInfo);
    this.panelPage4.Controls.Add((Control) this.labelHnt5);
    componentResourceManager.ApplyResources((object) this.panelPage4, "panelPage4");
    this.panelPage4.Name = "panelPage4";
    componentResourceManager.ApplyResources((object) this.pictureInfo, "pictureInfo");
    this.pictureInfo.Name = "pictureInfo";
    this.pictureInfo.TabStop = false;
    componentResourceManager.ApplyResources((object) this.labelHnt5, "labelHnt5");
    this.labelHnt5.BackColor = Color.Transparent;
    this.labelHnt5.Name = "labelHnt5";
    this.panelPage2.Controls.Add((Control) this.panelHint);
    this.panelPage2.Controls.Add((Control) this.panelAnalyze);
    this.panelPage2.Controls.Add((Control) this.panelProgress);
    this.panelPage2.Controls.Add((Control) this.labelAnalyze);
    this.panelPage2.Controls.Add((Control) this.treeAnalyze);
    this.panelPage2.Controls.Add((Control) this.panelTopProgress);
    componentResourceManager.ApplyResources((object) this.panelPage2, "panelPage2");
    this.panelPage2.Name = "panelPage2";
    this.panelPage2.Resize += new EventHandler(this.DoCalcBarsLabels);
    componentResourceManager.ApplyResources((object) this.panelHint, "panelHint");
    this.panelHint.BorderStyle = BorderStyle.FixedSingle;
    this.panelHint.Controls.Add((Control) this.labelHint2);
    this.panelHint.Controls.Add((Control) this.linkLabel);
    this.panelHint.Controls.Add((Control) this.pictureObject);
    this.panelHint.ForeColor = SystemColors.ControlText;
    this.panelHint.Name = "panelHint";
    componentResourceManager.ApplyResources((object) this.labelHint2, "labelHint2");
    this.labelHint2.Name = "labelHint2";
    this.linkLabel.ActiveLinkColor = Color.Crimson;
    this.linkLabel.Cursor = Cursors.Default;
    componentResourceManager.ApplyResources((object) this.linkLabel, "linkLabel");
    this.linkLabel.LinkColor = Color.Blue;
    this.linkLabel.Name = "linkLabel";
    this.linkLabel.TabStop = true;
    this.linkLabel.VisitedLinkColor = Color.Blue;
    this.linkLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(this.DoCancel);
    componentResourceManager.ApplyResources((object) this.pictureObject, "pictureObject");
    this.pictureObject.Name = "pictureObject";
    this.pictureObject.TabStop = false;
    this.panelAnalyze.Controls.Add((Control) this.labelAnalyzeInfo);
    this.panelAnalyze.Controls.Add((Control) this.progressBarAnalyze);
    componentResourceManager.ApplyResources((object) this.panelAnalyze, "panelAnalyze");
    this.panelAnalyze.Name = "panelAnalyze";
    componentResourceManager.ApplyResources((object) this.labelAnalyzeInfo, "labelAnalyzeInfo");
    this.labelAnalyzeInfo.Name = "labelAnalyzeInfo";
    componentResourceManager.ApplyResources((object) this.progressBarAnalyze, "progressBarAnalyze");
    this.progressBarAnalyze.Name = "progressBarAnalyze";
    this.panelProgress.Controls.Add((Control) this.labelProgressTech);
    this.panelProgress.Controls.Add((Control) this.progressBarTech);
    this.panelProgress.Controls.Add((Control) this.labelProgressDesign);
    this.panelProgress.Controls.Add((Control) this.progressBarDesign);
    componentResourceManager.ApplyResources((object) this.panelProgress, "panelProgress");
    this.panelProgress.Name = "panelProgress";
    componentResourceManager.ApplyResources((object) this.labelProgressTech, "labelProgressTech");
    this.labelProgressTech.Name = "labelProgressTech";
    componentResourceManager.ApplyResources((object) this.progressBarTech, "progressBarTech");
    this.progressBarTech.Name = "progressBarTech";
    componentResourceManager.ApplyResources((object) this.labelProgressDesign, "labelProgressDesign");
    this.labelProgressDesign.Name = "labelProgressDesign";
    componentResourceManager.ApplyResources((object) this.progressBarDesign, "progressBarDesign");
    this.progressBarDesign.Name = "progressBarDesign";
    componentResourceManager.ApplyResources((object) this.labelAnalyze, "labelAnalyze");
    this.labelAnalyze.Name = "labelAnalyze";
    componentResourceManager.ApplyResources((object) this.treeAnalyze, "treeAnalyze");
    this.treeAnalyze.BorderStyle = BorderStyles.UltraFlat;
    this.treeAnalyze.Columns.AddRange(new TreeListColumn[4]
    {
      this.columnVersion,
      this.columnDesign,
      this.columnDesignQuantity,
      this.columnTechQuantity
    });
    this.treeAnalyze.Name = "treeAnalyze";
    this.treeAnalyze.Styles.AddReplace("HideSelectionRow", (object) new ViewStyle("HideSelectionRow", "TreeList", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204), "", StyleOptions.StyleEnabled | StyleOptions.UseBackColor | StyleOptions.UseDrawFocusRect | StyleOptions.UseFont | StyleOptions.UseForeColor | StyleOptions.UseImage, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, SystemColors.MenuHighlight, SystemColors.HighlightText));
    this.treeAnalyze.Styles.AddReplace("HorzLine", (object) new ViewStyle("HorzLine", "TreeList", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204), "", StyleOptions.StyleEnabled, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, SystemColors.ControlLight, SystemColors.ControlLight));
    this.treeAnalyze.Styles.AddReplace("FocusedRow", (object) new ViewStyle("FocusedRow", "TreeList", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204), "", StyleOptions.StyleEnabled | StyleOptions.UseBackColor | StyleOptions.UseDrawFocusRect | StyleOptions.UseFont | StyleOptions.UseForeColor | StyleOptions.UseImage, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, SystemColors.MenuHighlight, SystemColors.HighlightText));
    this.treeAnalyze.Styles.AddReplace("TreeLine", (object) new ViewStyle("TreeLine", "TreeList", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204), "", StyleOptions.StyleEnabled, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, SystemColors.Window, SystemColors.ControlLight));
    this.treeAnalyze.Styles.AddReplace("FocusedCell", (object) new ViewStyle("FocusedCell", "TreeList", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204), "", StyleOptions.StyleEnabled | StyleOptions.UseBackColor | StyleOptions.UseDrawFocusRect | StyleOptions.UseFont | StyleOptions.UseForeColor | StyleOptions.UseImage, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, SystemColors.MenuHighlight, SystemColors.HighlightText));
    this.treeAnalyze.Styles.AddReplace("SelectedRow", (object) new ViewStyle("SelectedRow", "TreeList", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204), "", StyleOptions.StyleEnabled | StyleOptions.UseBackColor | StyleOptions.UseDrawFocusRect | StyleOptions.UseFont | StyleOptions.UseForeColor | StyleOptions.UseImage, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, SystemColors.MenuHighlight, SystemColors.HighlightText));
    componentResourceManager.ApplyResources((object) this.columnVersion, "columnVersion");
    this.columnVersion.Name = "columnVersion";
    componentResourceManager.ApplyResources((object) this.columnDesign, "columnDesign");
    this.columnDesign.Name = "columnDesign";
    componentResourceManager.ApplyResources((object) this.columnDesignQuantity, "columnDesignQuantity");
    this.columnDesignQuantity.Name = "columnDesignQuantity";
    componentResourceManager.ApplyResources((object) this.columnTechQuantity, "columnTechQuantity");
    this.columnTechQuantity.Name = "columnTechQuantity";
    this.panelTopProgress.Controls.Add((Control) this.labelProgress);
    this.panelTopProgress.Controls.Add((Control) this.edQuantity);
    this.panelTopProgress.Controls.Add((Control) this.labelHint6);
    componentResourceManager.ApplyResources((object) this.panelTopProgress, "panelTopProgress");
    this.panelTopProgress.Name = "panelTopProgress";
    componentResourceManager.ApplyResources((object) this.labelProgress, "labelProgress");
    this.labelProgress.Name = "labelProgress";
    componentResourceManager.ApplyResources((object) this.edQuantity, "edQuantity");
    this.edQuantity.Maximum = new Decimal(new int[4]
    {
      999999999,
      0,
      0,
      0
    });
    this.edQuantity.Minimum = new Decimal(new int[4]
    {
      1,
      0,
      0,
      0
    });
    this.edQuantity.Name = "edQuantity";
    this.edQuantity.Value = new Decimal(new int[4]
    {
      1,
      0,
      0,
      0
    });
    componentResourceManager.ApplyResources((object) this.labelHint6, "labelHint6");
    this.labelHint6.Name = "labelHint6";
    componentResourceManager.ApplyResources((object) this._findInTreeMenuButtonItem, "_findInTreeMenuButtonItem");
    this._findInTreeMenuButtonItem.Shortcut = Shortcut.CtrlF;
    this._findInTreeMenuButtonItem.ShowText = true;
    this._findInTreeMenuButtonItem.Click += new EventHandler(this.FindInTreeMenuButtonItem_Click);
    this.AutoScaleMode = AutoScaleMode.Inherit;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.Controls.Add((Control) this.panelBottom);
    this.Controls.Add((Control) this.panelPage2);
    this.Controls.Add((Control) this.panelPage4);
    this.Controls.Add((Control) this.panelPage3);
    this.Controls.Add((Control) this.panelPage0);
    this.Controls.Add((Control) this.panelPage1);
    this.HelpButton = true;
    this.KeyPreview = true;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (ContextCompositionCreatorForm);
    this.ShowInTaskbar = false;
    this.Tag = (object) " ";
    this.FormClosing += new FormClosingEventHandler(this.ContextCompositionCreatorForm_FormClosing);
    this.FormClosed += new FormClosedEventHandler(this.ContextCompositionCreatorForm_FormClosed);
    this.Load += new EventHandler(this.ContextCompositionCreatorForm_Load);
    this.scFrame.Panel1.ResumeLayout(false);
    this.scFrame.Panel2.ResumeLayout(false);
    this.scFrame.EndInit();
    this.scFrame.ResumeLayout(false);
    this.treeComposition.EndInit();
    this.panelCompositionTop.ResumeLayout(false);
    this.treeNewComposition.EndInit();
    this.panel1.ResumeLayout(false);
    this.TectContainer.Panel1.ResumeLayout(false);
    this.TectContainer.Panel2.ResumeLayout(false);
    this.TectContainer.EndInit();
    this.TectContainer.ResumeLayout(false);
    this.treeContextComposition.EndInit();
    this.panelContextTop.ResumeLayout(false);
    this.panelBottom.ResumeLayout(false);
    this.panelPage1.ResumeLayout(false);
    this.panelPage0.ResumeLayout(false);
    this.panelPage0.PerformLayout();
    this.panelPage3.ResumeLayout(false);
    this.panelPage4.ResumeLayout(false);
    this.panelPage4.PerformLayout();
    ((ISupportInitialize) this.pictureInfo).EndInit();
    this.panelPage2.ResumeLayout(false);
    this.panelHint.ResumeLayout(false);
    ((ISupportInitialize) this.pictureObject).EndInit();
    this.panelAnalyze.ResumeLayout(false);
    this.panelProgress.ResumeLayout(false);
    this.treeAnalyze.EndInit();
    this.panelTopProgress.ResumeLayout(false);
    this.edQuantity.EndInit();
    this.ResumeLayout(false);
  }

  internal class MyClientPluginsDataTransfer : ClientPluginsDataTransfer
  {
    internal int _currSet;
    internal List<long> _addContexts = new List<long>(0);
    internal List<long> _addContexts2 = new List<long>(0);
    internal List<long> _addContexts3 = new List<long>(0);

    public MyClientPluginsDataTransfer(long[] firstSet, long[] secondSet, long[] thirdSet)
    {
      this._addContexts.Clear();
      this._addContexts2.Clear();
      this._addContexts3.Clear();
      if (firstSet != null)
      {
        for (int index = 0; index < firstSet.Length; ++index)
        {
          if (!this._addContexts.Contains(firstSet[index]))
            this._addContexts.Add(firstSet[index]);
        }
      }
      if (secondSet != null)
      {
        for (int index = 0; index < secondSet.Length; ++index)
        {
          if (!this._addContexts2.Contains(secondSet[index]))
            this._addContexts2.Add(secondSet[index]);
        }
      }
      if (thirdSet == null)
        return;
      for (int index = 0; index < thirdSet.Length; ++index)
      {
        if (!this._addContexts3.Contains(thirdSet[index]))
          this._addContexts3.Add(thirdSet[index]);
      }
    }

    internal List<long> CurrentContexts
    {
      get
      {
        switch (this._currSet)
        {
          case 1:
            return this._addContexts2;
          case 2:
            return this._addContexts3;
          default:
            return this._addContexts;
        }
      }
    }

    internal int CurrentSet
    {
      get => this._currSet;
      set
      {
        if (this._currSet == value)
          return;
        switch (value)
        {
          case 1:
          case 2:
            this._currSet = value;
            break;
          default:
            this._currSet = 0;
            break;
        }
      }
    }

    public override void GetPluginData(HybridDictionary PluginsData)
    {
      base.GetPluginData(PluginsData);
      if (PluginsData == null)
        return;
      PluginsData[(object) "{82E381A1-8952-416A-B303-F81BA2945F8F}"] = (object) true;
      List<long> longList = new List<long>(this.CurrentContexts.Count);
      for (int index = 0; index < this.CurrentContexts.Count; ++index)
        longList.Add(this.CurrentContexts[index]);
      PluginsData[(object) "{AB419A02-DE8A-4A8E-905A-D782F5B720E5}"] = (object) longList;
    }
  }
}
