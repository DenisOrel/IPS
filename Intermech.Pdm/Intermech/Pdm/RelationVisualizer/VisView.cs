// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.VisView
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Bars;
using Intermech.Client.Core;
using Intermech.DataFormats;
using Intermech.Docking;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Configuration;
using Intermech.Interfaces.Pdm;
using Intermech.Interfaces.Pdm.RelationVisualizer;
using Intermech.Localization;
using Intermech.Map;
using Intermech.Map.Layout;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Pdm.VisDialogs;
using Intermech.PropertyEditors;
using NJFLib.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class VisView : DockControl, IFiltrationClass, ICommandTarget
{
  private static readonly Guid _persistStateGuid = new Guid("{3688A28E-DCEE-458D-8EDE-3A18BC27A518}");
  public const string RelVisWindowName = "newRelationVisualizerWindow";
  public static readonly string RelVisName = LocalizationHolder.rm.GetString("Pdm_rv_1");
  public static readonly string Preview_None = LocalizationHolder.rm.GetString("Pdm_rv_52");
  public static readonly string Preview_Sel = LocalizationHolder.rm.GetString("Pdm_rv_53");
  public static readonly string Preview_All = LocalizationHolder.rm.GetString("Pdm_rv_54");
  private DrawSettings settings = new DrawSettings();
  private UserSettings userSettings;
  private PreviewMode previewMode = PreviewMode.SelPreview;
  private SchemeList schemeList;
  private VisScheme curScheme;
  protected bool _activated;
  protected bool _loaded;
  protected Intermech.Client.Core.NotificationService _notificationService;
  private ICommandManager _cmdMngr;
  private INamedImageList _images;
  protected AdvancedServiceContainer _services;
  private VisEventList dbEvents = new VisEventList();
  private IElementStatusesClientService _elementStatusesClientService;
  private ICurrentUserAndRole _CurrentUserAndRole;
  private IVisualizerService serverService;
  private VisLink currentSelectedRelation;
  private ISelectedItems currentSelectedObjects;
  private ILayoutAlgorithm currentLayoutAlgorithm = (ILayoutAlgorithm) new NormalLayout();
  private VisLayout STANDARD_LAYOUT = (VisLayout) new StandardLayout();
  private VisLayout HIER_LAYOUT = (VisLayout) new HierarchLayout();
  private VisLayout VERT_STANDARD_LAYOUT = (VisLayout) new StandardVertLayout();
  private VisLayout VERT_HIER_LAYOUT = (VisLayout) new HierarchVertLayout();
  private VisLayout QUALITY_LAYOUT = (VisLayout) new QualityLayout();
  private IVisLayout visLayout;
  private Thread backgrThread;
  private BackgroundWorker newTreeWorker;
  private BackgroundWorker updateTreeWorker;
  private BackgroundWorker expandWorker;
  private BackgroundWorker layoutWorker;
  private BackgroundWorker previewWorker;
  private SchemeInfo curSchemeInfo;
  private SchemeInfo defSchemeInfo;
  private MapDocument backDocument;
  private int attrCountTypeID = -1;
  internal bool ShowStructLinks = true;
  internal bool ShowAssociativeLinks = true;
  private IFiltrationService _filtrationService;
  [NonSerialized]
  private IFiltrationClass _FiltrationClass;
  [NonSerialized]
  private string _FiltrationOwnerID = string.Empty;
  private MeasuresConvertor measureConvertor;
  private List<VisNode> objsToDelete = new List<VisNode>();
  private bool lockUpdateStatus;
  private bool setCenterNode;
  private static readonly string visSchemeGuid = "cadd9aa6-306c-11d8-b4e9-00304f19f545";
  private static readonly string visStylesGuid = "cadd9aa7-306c-11d8-b4e9-00304f19f545";
  private StyleData defStyle;
  private IContainer components;
  private StatusStrip statusBar;
  private PropPanel propertyGridPanel;
  private CollapsibleSplitter collapsibleSplitter;
  private ToolStripStatusLabel toolStripStatus;
  private ContextMenuStrip contextMenuStrip_map;
  private ToolStripMenuItem toolStripMenuItem_props;
  private ToolStripMenuItem toolStripMenuItem_delete;
  private ToolStripStatusLabel toolStripStatus_Stop;
  private Intermech.Bars.ToolBar toolBar;
  private ButtonItem buttonItem_ZoomIn;
  private ButtonItem buttonItem_ZoomOut;
  private ButtonItem buttonItem_WidthIn;
  private ButtonItem buttonItem_WidthOut;
  private ButtonItem buttonItem_HeigthIn;
  private ButtonItem buttonItem_HeigthOut;
  private VisControl map;
  private ButtonItem buttonItem_ZoomOnce;
  private ButtonItem buttonItem_AllShema;
  private ButtonItem buttonItem_IfNeedParentTree;
  private ButtonItem buttonItem_IfNeedChildTree;
  private ButtonItem buttonItem_FindObject;
  private DropDownMenuItem dropDownMenu_FromHistory;
  private DropDownMenuItem menuSelectLayout;
  private ButtonItem buttonItem_LevelP;
  private ButtonItem buttonItem_Status;
  private MenuButtonItem mbiLayoutNormal;
  private MenuButtonItem mbiLayoutKulon;
  private MenuButtonItem mbiLayoutHierarch;
  private ButtonItem bStructureLinks;
  private ButtonItem bAssociateLinks;
  private ButtonItem btnLoadStyle;
  private DropDownMenuItem ddMenu_Preview;
  private MenuButtonItem visPreviewNone;
  private MenuButtonItem visPreviewFromScheme;
  private MenuButtonItem visPreviewAll;
  private MenuButtonItem mbiLayoutVertNorm;
  private MenuButtonItem mbiLayoutVertHier;
  private MenuButtonItem mbiLayoutSupreme;

  public VisView()
  {
    this.InitializeComponent();
    this.mbiLayoutKulon.Visible = false;
    this.Guid = VisView._persistStateGuid;
    if (this.DesignMode)
      return;
    this.InitializeServices();
    this.InitializeWindow();
    this.InitLayoutAlgoritm();
    this.TabImage = (Image) Intermech.Pdm.Properties.Resources.rv1;
    this.ShowImageInDocumentTab = true;
    this._activated = false;
    this.HideOnClose = true;
    this.PrimaryControl = (Control) this.map;
    this.ReadMetadata4DB();
    this.map.NoFocusSelectionColor = this.map.PrimarySelectionColor;
    this.defStyle = this.CreateDefaultStyle();
    this.map.AllowCopy = false;
    this.map.MaximumSelectionCount = 10;
    MapViewManager mapViewManager = new MapViewManager((MapView) this.map);
    mapViewManager.InitializeServices();
    this.map.DefaultTool = (IMapTool) mapViewManager;
  }

  public long PrevMode
  {
    get => (long) this.previewMode;
    set
    {
      this.previewMode = (PreviewMode) value;
      this.UpdatePreviewMenu(this.previewMode);
    }
  }

  public bool Execute(ICommandState commandState)
  {
    switch (commandState.CommandName)
    {
      case "CancelChanges":
      case "CheckIn":
      case "CheckOut":
      case "Copy":
      case "Cut":
      case "Delete":
      case "Exclude":
      case "ParametersCard":
      case "Paste":
      case "SaveChanges":
        int num = this.ExecuteMenuCommand(commandState.CommandName) ? 1 : 0;
        if (num == 0)
          return num != 0;
        this.UpdateCurSchemeInfo(DBEvent.DBEventType.All);
        return num != 0;
      case "Print":
        this.map.Print();
        return true;
      case "PrintPreview":
        this.map.PrintPreview();
        return true;
      case "Refresh":
        this.BarCommand_Refresh();
        return true;
      default:
        return false;
    }
  }

  private bool ExecuteMenuCommand(string command)
  {
    if (command == string.Empty || this.currentSelectedObjects == null)
      return false;
    CommandsTable commandsTable = Intermech.Navigator.ContextMenu.Services.GetCommandsTable(this.currentSelectedObjects, (IServiceProvider) this._services, false);
    if (!commandsTable.Contains(command))
      return false;
    Intermech.Navigator.ContextMenu.Services.InvokeCommand(command, commandsTable, (IServiceProvider) this._services);
    return true;
  }

  public bool QueryStatus(ICommandState commandState)
  {
    switch (commandState.CommandName)
    {
      case "CancelChanges":
      case "CheckIn":
      case "CheckOut":
      case "Copy":
      case "Delete":
      case "Exclude":
      case "ParametersCard":
      case "SaveChanges":
        commandState.Enabled = this.BarCommandCheckStatus_baseComands(commandState.CommandName);
        return commandState.Enabled;
      case "Cut":
      case "Paste":
        commandState.Enabled = false;
        return false;
      case "Print":
        commandState.Enabled = true;
        return true;
      case "Refresh":
        commandState.Enabled = true;
        return true;
      default:
        return false;
    }
  }

  protected string Get_FiltrationOwnerID()
  {
    if (this._FiltrationOwnerID.Length <= 0)
      this._FiltrationOwnerID = Convert.ToString((object) Guid.NewGuid());
    return this._FiltrationOwnerID;
  }

  protected HybridDictionary GetFiltrationParms()
  {
    IFiltrationSettings filtration = this._filtrationService.Filtration;
    HybridDictionary filtrationParms = new HybridDictionary();
    foreach (object key in (IEnumerable) filtration.Tags.Keys)
      filtrationParms.Add(key, filtration.Tags[key]);
    filtrationParms.Add((object) "RULE_ID", (object) filtration.RuleID);
    filtrationParms.Add((object) "OWNER_ID", (object) filtration.OwnerID);
    return filtrationParms;
  }

  public string FiltrationOwnerID => this.Get_FiltrationOwnerID();

  protected virtual IFiltrationService InitializeFiltrationService()
  {
    this.Get_FiltrationOwnerID();
    return (IFiltrationService) ServicesManager.GetService(typeof (IFiltrationService));
  }

  protected virtual void DisposeFiltrationService(IFiltrationService filtrationService)
  {
  }

  public IFiltrationService FiltrationService => this._filtrationService;

  private void FiltrationInitToolbar()
  {
    if (this.FiltrationService == null)
      return;
    string filtrationOwnerId = this.Get_FiltrationOwnerID();
    if (filtrationOwnerId != this.FiltrationService.FiltrationServiceOwnerID)
      this.FiltrationService.FiltrationServiceOwnerID = filtrationOwnerId;
    this.FiltrationService.Enabled = true;
    if (!this.FiltrationService.FiltrationToolbarHidden)
      this.FiltrationService.FiltrationToolbarVisible = true;
    this.FiltrationService.FiltrationApplyUpdates(true);
  }

  private void FiltrationClearToolbar()
  {
    if (this.FiltrationService == null)
      return;
    this.FiltrationService.FiltrationServiceOwnerID = string.Empty;
  }

  private void _filtrationService_OnFiltrationChanged(
    IFiltrationSettings NewFiltration,
    bool FiltrationValid)
  {
    if (NewFiltration != null && NewFiltration.OwnerID != this.Get_FiltrationOwnerID())
      return;
    this.setCenterNode = true;
    this.BuildThread(VisView.BuildFlags.CreateTree);
  }

  protected virtual void Do_DeleteFiltrationSettings()
  {
    if (string.IsNullOrEmpty(this._FiltrationOwnerID))
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      (sessionKeeper.Session.GetCustomService(typeof (IVersionRulesCacheService)) as IVersionRulesCacheService).DeleteRuleTuning((object) sessionKeeper.Session.SessionGUID, this._FiltrationOwnerID);
      this._FiltrationOwnerID = string.Empty;
    }
  }

  public event VisView.ThreadFinishEventHandler ThreadFinish;

  public event VisView.ThreadBuildSchemaUpdateInfo BuildSchemaUpdate;

  public bool IsThreadBusy
  {
    get
    {
      return this.newTreeWorker.IsBusy || this.updateTreeWorker.IsBusy || this.expandWorker.IsBusy || this.layoutWorker.IsBusy || this.previewWorker.IsBusy;
    }
  }

  public bool StopThread()
  {
    bool flag = false;
    try
    {
      if (this.newTreeWorker.IsBusy)
        this.newTreeWorker.CancelAsync();
      if (this.updateTreeWorker.IsBusy)
        this.updateTreeWorker.CancelAsync();
      if (this.expandWorker.IsBusy)
        this.expandWorker.CancelAsync();
      if (this.layoutWorker.IsBusy)
        this.layoutWorker.CancelAsync();
      if (this.previewWorker.IsBusy)
        this.previewWorker.CancelAsync();
    }
    catch (Exception ex)
    {
      ExceptionHelper.ExceptionService.ShowException(new Exception(LocalizationHolder.rm.GetString("Pdm_532") + ex.Message, ex));
    }
    return flag;
  }

  protected void OnBuildStatusUpdate(string text, float percent)
  {
    if (this.lockUpdateStatus)
      return;
    VisView.ThreadBuildSchemaUpdateInfo buildSchemaUpdate = this.BuildSchemaUpdate;
    if (buildSchemaUpdate == null)
      return;
    buildSchemaUpdate(text, percent);
  }

  private void InitializeWindow()
  {
    this.schemeList = new SchemeList();
    this.curScheme = (VisScheme) null;
    this.map.NewLinkClass = typeof (VisLink);
    this.backDocument = new MapDocument();
    this.map.ObjectContextClicked += new MapObjectEventHandler(this.map_ObjectContextClicked);
    this.map.ClipboardPasted += new EventHandler(this.map_ClipboardPasted);
    this.map.SelectionDeleted += new EventHandler(this.map_SelectionDeleted);
    this.map.OnRelationCreated += new VisControl.CreateRelation(this.map_OnRelationCreated);
    this.map.ObjectGotSelection += new MapSelectionEventHandler(this.map_ObjectGotSelection);
    this.map.BackgroundSingleClicked += new MapInputEventHandler(this.map_BackgroundSingleClicked);
    this.map.PortDoubleClicked += new PortDoubleClickedHandler(this.map_PortDoubleClicked);
    this.Closed += new EventHandler(this.RelationVisualiserWindow_Closed);
    if (this._images != null)
    {
      this.toolBar.ImageList = this._images.ImageList;
      this.buttonItem_FindObject.ImageIndex = this._images.ImageIndex("imgFind");
      this.contextMenuStrip_map.ImageList = this._images.ImageList;
      this.toolStripMenuItem_delete.ImageIndex = this._images.ImageIndex("imgDelete");
      this.toolStripMenuItem_props.ImageIndex = this._images.ImageIndex("imgProp");
      this.buttonItem_Status.ImageIndex = this._images.ImageIndex("imgContextComposition.PDM");
      this.statusBar.ImageList = this._images.ImageList;
      this.toolStripStatus_Stop.ImageIndex = this._images.ImageIndex("imgStop2");
    }
    this.measureConvertor = new MeasuresConvertor();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this.measureConvertor.Init(sessionKeeper.Session.GetMeasuresList());
    this.propertyGridPanel.AttrsUpdated += new EventHandler(this.propertyGridPanel_AttrsUpdated);
    ServicesManager.GetService(typeof (IRelVisSettings));
    BarManager service = ServicesManager.GetService(typeof (BarManager)) as BarManager;
    this.toolBar.Renderer = service.Renderer;
    service.RendererChanged += new EventHandler(this.barRender_RendererChanged);
    (ServicesManager.ServiceContainer.GetService(typeof (IPropertyPagesService)) as IPropertyPagesService).Changed += new EventHandler(this.PageService_Changed);
    this.UpdateUserSettings();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      StatusCollection.InitializeStatusCollection(sessionKeeper.Session, this._elementStatusesClientService);
      LifecycleLevelInfo.InitLifecycleLevel(sessionKeeper.Session);
    }
    this.map.AllowLink = this.userSettings.allowCreatingRelations;
    this.InitWorkers();
    if (this.BuildSchemaUpdate != null)
      return;
    this.BuildSchemaUpdate += new VisView.ThreadBuildSchemaUpdateInfo(this.RW_BuildSсhemeUpdate);
  }

  private void barRender_RendererChanged(object sender, EventArgs e)
  {
    this.toolBar.Renderer = (sender as BarManager).Renderer;
  }

  private void UpdateUserSettings()
  {
    if (!(ServicesManager.GetService(typeof (IRelVisSettings)) is IRelVisSettings service) || service.Settings == null)
      return;
    UserSettings settings = service.Settings;
    this.userSettings = settings;
    this.settings.MaxCaptionLength = Convert.ToInt32(settings.MaxCaptionLength);
    this.settings.NoCaptionFormula = settings.NoCaptionFormula;
  }

  private void InitWorkers()
  {
    this.newTreeWorker = new BackgroundWorker();
    this.newTreeWorker.WorkerSupportsCancellation = true;
    this.newTreeWorker.WorkerReportsProgress = true;
    this.newTreeWorker.DoWork += new DoWorkEventHandler(this.BuildNewTree);
    this.newTreeWorker.ProgressChanged += new ProgressChangedEventHandler(this.WorkerProgressChanged);
    this.newTreeWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.NewTreeCompleted);
    this.updateTreeWorker = new BackgroundWorker();
    this.updateTreeWorker.WorkerSupportsCancellation = true;
    this.updateTreeWorker.WorkerReportsProgress = true;
    this.updateTreeWorker.DoWork += new DoWorkEventHandler(this.UpdateTree);
    this.updateTreeWorker.ProgressChanged += new ProgressChangedEventHandler(this.WorkerProgressChanged);
    this.updateTreeWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.UpdateTreeCompleted);
    this.expandWorker = new BackgroundWorker();
    this.expandWorker.WorkerSupportsCancellation = true;
    this.expandWorker.WorkerReportsProgress = true;
    this.expandWorker.DoWork += new DoWorkEventHandler(this.ExpandObjStru);
    this.expandWorker.ProgressChanged += new ProgressChangedEventHandler(this.WorkerProgressChanged);
    this.expandWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.ExpandCompleted);
    this.layoutWorker = new BackgroundWorker();
    this.layoutWorker.WorkerSupportsCancellation = true;
    this.layoutWorker.WorkerReportsProgress = true;
    this.layoutWorker.DoWork += new DoWorkEventHandler(this.ReLayout);
    this.layoutWorker.ProgressChanged += new ProgressChangedEventHandler(this.WorkerProgressChanged);
    this.layoutWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.LayoutCompleted);
    this.previewWorker = new BackgroundWorker();
    this.previewWorker.WorkerSupportsCancellation = true;
    this.previewWorker.WorkerReportsProgress = true;
    this.previewWorker.DoWork += new DoWorkEventHandler(this.LoadPreviews);
    this.previewWorker.ProgressChanged += new ProgressChangedEventHandler(this.WorkerProgressChanged);
    this.previewWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.LoadPreviewsCompleted);
  }

  private void LoadPreviews(object sender, DoWorkEventArgs e)
  {
    MapDocument newDocument = this.CreateNewDocument();
    newDocument.UserObject = (object) this.settings;
    if (!(sender is BackgroundWorker bw))
      return;
    if (this.curScheme.ParentLevels != null && this.curScheme.ParentLevels.Count > 0)
    {
      bw.ReportProgress(0, (object) LocalizationHolder.rm.GetString("Pdm_rv_49"));
      this.curScheme.UpdatePreviews(bw, true, (int) this.previewMode);
    }
    if (this.curScheme.ChildLevels != null && this.curScheme.ChildLevels.Count > 0)
    {
      bw.ReportProgress(50, (object) LocalizationHolder.rm.GetString("Pdm_rv_51"));
      this.curScheme.UpdatePreviews(bw, false, (int) this.previewMode);
    }
    bw.ReportProgress(100, (object) LocalizationHolder.rm.GetString("Pdm_rv_50"));
    e.Result = (object) new SchemCreator(this.curScheme, this.map, newDocument, this.previewMode);
  }

  private void LoadPreviewsCompleted(object sender, RunWorkerCompletedEventArgs e)
  {
    if (!(e.Result is SchemCreator result))
      return;
    MapDocument document = result.document;
    this.curScheme.UpdatePreviewMode((int) this.previewMode);
    result?.CreateFullScheme();
    this.dbEvents.InitIdents(document);
    this.map.Document = document;
    this.ShowLayers(this.NeedParentTree, this.NeedChildTree);
  }

  private void ReadMetadata4DB()
  {
    this.attrCountTypeID = MetaDataHelper.GetAttributeTypeID(new Guid("cad00267-306c-11d8-b4e9-00304f19f545"));
  }

  protected override void OnClosing(CancelEventArgs e)
  {
    base.OnClosing(e);
    try
    {
      IConfigurationManager service = ServicesManager.ServiceContainer.GetService<IConfigurationManager>();
      IConfiguration configuration = service.Open("RelationVisualizerView") ?? service.Create("RelationVisualizerView");
      configuration.SetProperty("ShowParent", this.buttonItem_IfNeedParentTree.Checked.ToString());
      configuration.SetProperty("ShowChild", this.buttonItem_IfNeedChildTree.Checked.ToString());
      configuration.SetProperty("ShowLevelP", this.buttonItem_LevelP.Checked.ToString());
      configuration.SetProperty("ShowStatus", this.buttonItem_Status.Checked.ToString());
      this.propertyGridPanel.PropertyGrid.SelectedObject = (object) null;
      this.currentLayoutAlgorithm = (ILayoutAlgorithm) null;
      this.currentSelectedObjects = (ISelectedItems) null;
      this.currentSelectedRelation = (VisLink) null;
    }
    catch
    {
    }
  }

  public override void OnClosed(EventArgs e)
  {
    this.StopThread();
    base.OnClosed(e);
  }

  protected override void OnLoad(EventArgs e)
  {
    base.OnLoad(e);
    try
    {
      IConfiguration configuration = ServicesManager.ServiceContainer.GetService<IConfigurationManager>().Open("RelationVisualizerView");
      if (configuration != null)
      {
        string property1 = configuration.GetProperty("ShowParent");
        if (!string.IsNullOrEmpty(property1))
          this.buttonItem_IfNeedParentTree.Checked = bool.Parse(property1);
        string property2 = configuration.GetProperty("ShowChild");
        if (!string.IsNullOrEmpty(property2))
          this.buttonItem_IfNeedChildTree.Checked = bool.Parse(property2);
        string property3 = configuration.GetProperty("ShowLevelP");
        if (!string.IsNullOrEmpty(property3))
        {
          this.buttonItem_LevelP.Checked = bool.Parse(property3);
          this.settings.ShowLifecycleLevel = this.buttonItem_LevelP.Checked;
        }
        string property4 = configuration.GetProperty("ShowStatus");
        if (!string.IsNullOrEmpty(property4))
        {
          this.buttonItem_Status.Checked = bool.Parse(property4);
          this.settings.ShowStatuses = this.buttonItem_Status.Checked;
          if (this.curSchemeInfo.UserSettings != null)
            this.curSchemeInfo.UserSettings.ShowStatuses = this.settings.ShowStatuses;
          else
            this.curSchemeInfo.UserSettings = this.settings;
        }
      }
      this.visLayout = (IVisLayout) this.STANDARD_LAYOUT;
    }
    catch (FormatException ex)
    {
    }
  }

  private bool NeedChildTree
  {
    get => this.buttonItem_IfNeedChildTree.Checked;
    set => this.buttonItem_IfNeedChildTree.Checked = value;
  }

  private bool NeedParentTree
  {
    get => this.buttonItem_IfNeedParentTree.Checked;
    set => this.buttonItem_IfNeedParentTree.Checked = value;
  }

  private void InitLayoutAlgoritm()
  {
    this.currentLayoutAlgorithm.LayoutProgress += new MapLayoutProgressEventHandler(this.shLayout_Progress);
  }

  private void DisposeLayoutAlgoritm()
  {
    this.currentLayoutAlgorithm.LayoutProgress -= new MapLayoutProgressEventHandler(this.shLayout_Progress);
  }

  private void PageService_Changed(object sender, EventArgs e) => this.UpdateUserSettings();

  private void propertyGridPanel_AttrsUpdated(object sender, EventArgs e)
  {
    this.UpdateCurSchemeInfo(DBEvent.DBEventType.RelationChanged);
  }

  private void UpdateCurSchemeInfo(DBEvent.DBEventType eType)
  {
    MapDocument document = this.map.Document;
    try
    {
      MapLayerCollectionObjectEnumerator enumerator = document.GetEnumerator();
      MeasuredValue mv = new MeasuredValue(0.0, 0L);
      if ((eType == DBEvent.DBEventType.CreateRelation || eType == DBEvent.DBEventType.All) && this.dbEvents.WasRelationCreated())
      {
        this.BuildThread(VisView.BuildFlags.UpdateTree);
      }
      else
      {
        foreach (MapObject mapObject in enumerator)
        {
          if (mapObject is VisNode)
          {
            IEnumerable<VisDBEvent> eventsForId = this.dbEvents.GetEventsForId((mapObject as VisNode).ObjId);
            if (eventsForId != null)
            {
              if ((eType == DBEvent.DBEventType.RemoveObject || eType == DBEvent.DBEventType.All) && VisEventList.WasDeleted(eventsForId))
              {
                foreach (IMapLink link in (mapObject as VisNode).Links)
                  document.Remove(link.MapObject);
                document.Remove(mapObject);
              }
              else
              {
                long objId = (mapObject as VisNode).ObjId;
                if ((eType == DBEvent.DBEventType.ObjVerIdChanged || eType == DBEvent.DBEventType.All) && VisEventList.WasIdChanged(eventsForId))
                  (mapObject as VisNode).ReflectCheckInOrOut();
              }
            }
          }
          else if (mapObject is VisLink)
          {
            VisLink visLink = mapObject as VisLink;
            IEnumerable<VisDBEvent> eventsForId = this.dbEvents.GetEventsForId(visLink.RelId);
            if (eventsForId != null)
            {
              if ((eType == DBEvent.DBEventType.RemoveRelation || eType == DBEvent.DBEventType.All) && VisEventList.WasDeleted(eventsForId))
                document.Remove(mapObject);
              else if (eType == DBEvent.DBEventType.RelationChanged || eType == DBEvent.DBEventType.All)
              {
                bool flag = false;
                foreach (VisDBEvent visDbEvent in eventsForId)
                {
                  if ((visDbEvent.EventCode & EvCode.RelationModified) != EvCode.NoEvent)
                  {
                    mv = visDbEvent.Quantity;
                    flag = true;
                    break;
                  }
                }
                if (flag)
                  visLink.SetCount(mv);
              }
            }
          }
        }
      }
    }
    finally
    {
      this.dbEvents.Clear();
    }
  }

  private MapLayerCollectionObjectEnumerator SetDeletableFlag2Document(
    MapDocument document,
    bool flag)
  {
    MapLayerCollectionObjectEnumerator enumerator = document.GetEnumerator();
    int num = 0;
    this.objsToDelete.Clear();
    foreach (MapObject mapObject in enumerator)
    {
      if (mapObject is VisNode visNode)
      {
        this.objsToDelete.Add(visNode);
        visNode.MarkForDelete = flag;
        ++num;
      }
    }
    return enumerator;
  }

  private void DeleteRelObjsAtDocument(MapDocument document)
  {
    document.GetEnumerator();
    int num = 0;
    foreach (MapObject mapObject in this.objsToDelete)
    {
      VisNode visNode = mapObject as VisNode;
      if (mapObject.Layer == null || mapObject.Layer.Identifier == null || !mapObject.Layer.Identifier.Equals((object) 0))
      {
        if (visNode != null)
        {
          ++num;
          if (visNode.MarkForDelete)
          {
            if (mapObject.Layer != null)
              mapObject.Layer.Remove(mapObject);
            document.Remove(mapObject);
          }
        }
      }
    }
    document.UpdateViews();
  }

  private void UpdateLayers(MapDocument document)
  {
    MapLayer mapLayer1 = document.Layers.Find((object) 2);
    MapLayer mapLayer2 = document.Layers.Find((object) 1);
    if (mapLayer1 == null || mapLayer2 == null)
      return;
    mapLayer1.AllowView = this.NeedChildTree;
    mapLayer2.AllowView = this.NeedParentTree;
  }

  private void SetVisibleAllLayers(MapDocument document)
  {
    MapLayer mapLayer1 = document.Layers.Find((object) 2);
    MapLayer mapLayer2 = document.Layers.Find((object) 1);
    if (mapLayer1 == null || mapLayer2 == null)
      return;
    mapLayer1.AllowView = true;
    mapLayer2.AllowView = true;
  }

  private void BuildNewTree(object sender, DoWorkEventArgs e)
  {
    if (this.curSchemeInfo.IsEmpty || this.curSchemeInfo.ObjectVersionId == 0L)
      return;
    this.curScheme = new VisScheme(this.previewMode);
    this.curScheme.Init(this.serverService, this.Style);
    this.curScheme.FiltrationOwnerID = this.Get_FiltrationOwnerID();
    VisStatusKeeper.UpdateDisabledPlugins();
    MapDocument newDocument = this.CreateNewDocument();
    newDocument.UserObject = (object) this.settings;
    BackgroundWorker bw = sender as BackgroundWorker;
    bw.ReportProgress(0, (object) LocalizationHolder.rm.GetString("Pdm_rv_9"));
    this.curScheme.BuildRoot(this.curSchemeInfo.ObjectVersionId);
    bw.ReportProgress(5, (object) LocalizationHolder.rm.GetString("Pdm_rv_8"));
    if (this.NeedCancel(bw, e))
      return;
    if (this.NeedChildTree)
      this.curScheme.BuildChilds(bw, this.ShowStructLinks, this.ShowAssociativeLinks);
    else
      this.curScheme.ChildLevels = new List<VisLevel>();
    bw.ReportProgress(10, (object) LocalizationHolder.rm.GetString("Pdm_rv_7"));
    if (this.NeedCancel(bw, e))
      return;
    if (this.NeedParentTree)
      this.curScheme.BuildParents(bw, this.ShowStructLinks, this.ShowAssociativeLinks);
    else
      this.curScheme.ParentLevels = new List<VisLevel>();
    bw.ReportProgress(15, (object) LocalizationHolder.rm.GetString("Pdm_rv_6"));
    if (this.NeedCancel(bw, e))
      return;
    this.curScheme.MaxPreviewMode = (int) this.previewMode;
    BasePredicate continuePredicate = this.CreateContinuePredicate(bw, e);
    Size size = this.map.Size;
    this.visLayout.InitLayout(size, this.curScheme);
    this.visLayout.BeforeLayout(continuePredicate, this.curScheme);
    this.visLayout.SetInitialCoords(continuePredicate, this.curScheme, new Point(size.Width / 2, size.Height / 2));
    this.visLayout.DoLayout(continuePredicate, this.curScheme);
    bw.ReportProgress(100, (object) LocalizationHolder.rm.GetString("Pdm_rv_5"));
    e.Result = (object) new SchemCreator(this.curScheme, this.map, newDocument, this.previewMode);
  }

  internal void NewTreeCompleted(object sender, RunWorkerCompletedEventArgs e)
  {
    try
    {
      if (e.Cancelled)
      {
        VisView.ThreadBuildSchemaUpdateInfo buildSchemaUpdate = this.BuildSchemaUpdate;
        if (buildSchemaUpdate == null)
          return;
        buildSchemaUpdate(LocalizationHolder.rm.GetString("Pdm_rv_32"), 0.0f);
      }
      else
      {
        SchemCreator schemCreator = e.Error == null ? (SchemCreator) e.Result : throw e.Error;
        MapDocument document = schemCreator.document;
        if (document != null)
        {
          schemCreator.CreateFullScheme();
          this.dbEvents.InitIdents(document);
          this.map.Document = document;
          this.ShowLayers(this.NeedParentTree, this.NeedChildTree);
          this.curScheme.Loaded.UpdateSettings(this.NeedParentTree, this.NeedChildTree, this.ShowStructLinks, this.ShowAssociativeLinks);
        }
        if (this.setCenterNode)
        {
          MapLayer mapLayer = this.map.Document.Layers.Find((object) 0);
          if (mapLayer != null)
          {
            VisNode firstObject = (VisNode) mapLayer.FirstObject;
            this.map.DocPosition = new PointF(firstObject.Center.X - this.map.DocExtentSize.Width / 2f, firstObject.Center.Y - this.map.DocExtentSize.Height / 2f);
          }
          this.CentralizeScheme();
          this.setCenterNode = false;
        }
        long objectCount = (long) this.curScheme.ObjectCount;
        string text = this.curSchemeInfo.Caption + LocalizationHolder.rm.GetString("Pdm_rv_25") + (object) objectCount;
        VisView.ThreadBuildSchemaUpdateInfo buildSchemaUpdate = this.BuildSchemaUpdate;
        if (buildSchemaUpdate == null)
          return;
        buildSchemaUpdate(text, 100f);
      }
    }
    finally
    {
      this.LockControls(false);
      this.toolStripStatus_Stop.Visible = false;
    }
  }

  private void UpdateTree(object sender, DoWorkEventArgs e)
  {
    if (this.curSchemeInfo.IsEmpty || this.curSchemeInfo.ObjectVersionId == 0L)
      return;
    MapDocument newDocument = this.CreateNewDocument();
    newDocument.UserObject = (object) this.settings;
    VisStatusKeeper.UpdateDisabledPlugins();
    BackgroundWorker bw = sender as BackgroundWorker;
    bw.ReportProgress(0, (object) LocalizationHolder.rm.GetString("Pdm_rv_10"));
    if (this.NeedChildTree && !this.curScheme.Loaded.ChildsLoaded)
    {
      bw.ReportProgress(10, (object) LocalizationHolder.rm.GetString("Pdm_rv_8"));
      this.curScheme.BuildChilds(bw, this.ShowStructLinks, this.ShowAssociativeLinks);
      if (this.NeedCancel(bw, e))
        return;
    }
    if (this.NeedParentTree && !this.curScheme.Loaded.ParentsLoaded)
    {
      bw.ReportProgress(10, (object) LocalizationHolder.rm.GetString("Pdm_rv_7"));
      this.curScheme.BuildParents(bw, this.ShowStructLinks, this.ShowAssociativeLinks);
      if (this.NeedCancel(bw, e))
        return;
    }
    bw.ReportProgress(50, (object) LocalizationHolder.rm.GetString("Pdm_rv_4"));
    VisScheme curScheme = this.curScheme;
    BasePredicate continuePredicate = this.CreateContinuePredicate(bw, e);
    Size size = this.map.Size;
    this.visLayout.InitLayout(size, curScheme);
    this.visLayout.BeforeLayout(continuePredicate, curScheme);
    this.visLayout.SetInitialCoords(continuePredicate, curScheme, new Point(size.Width / 2, size.Height / 2));
    this.visLayout.DoLayout(continuePredicate, curScheme);
    bw.ReportProgress(100, (object) LocalizationHolder.rm.GetString("Pdm_rv_5"));
    e.Result = (object) new SchemCreator(curScheme, this.map, newDocument, this.previewMode);
  }

  internal void UpdateTreeCompleted(object sender, RunWorkerCompletedEventArgs e)
  {
    try
    {
      if (e.Cancelled)
      {
        VisView.ThreadBuildSchemaUpdateInfo buildSchemaUpdate = this.BuildSchemaUpdate;
        if (buildSchemaUpdate == null)
          return;
        buildSchemaUpdate(LocalizationHolder.rm.GetString("Pdm_rv_32"), 0.0f);
      }
      else
      {
        SchemCreator schemCreator = e.Error == null ? (SchemCreator) e.Result : throw e.Error;
        MapDocument document = schemCreator.document;
        if (document != null)
        {
          schemCreator.CreateFullScheme();
          this.dbEvents.InitIdents(document);
          this.map.Document = document;
          this.ShowLayers(this.NeedParentTree, this.NeedChildTree);
          this.curScheme.Loaded.UpdateSettings(this.NeedParentTree, this.NeedChildTree, this.ShowStructLinks, this.ShowAssociativeLinks);
        }
        if (this.setCenterNode)
        {
          MapLayer mapLayer = this.map.Document.Layers.Find((object) 0);
          if (mapLayer != null)
            this.map.ScrollToControl(mapLayer.FirstObject);
          this.setCenterNode = false;
        }
        long objectCount = (long) schemCreator.scheme.ObjectCount;
        string text = this.curSchemeInfo.Caption + LocalizationHolder.rm.GetString("Pdm_rv_25") + (object) objectCount;
        VisView.ThreadBuildSchemaUpdateInfo buildSchemaUpdate = this.BuildSchemaUpdate;
        if (buildSchemaUpdate == null)
          return;
        buildSchemaUpdate(text, 100f);
      }
    }
    finally
    {
      this.LockControls(false);
      this.toolStripStatus_Stop.Visible = false;
    }
  }

  internal void WorkerProgressChanged(object sender, ProgressChangedEventArgs e)
  {
    if (this.lockUpdateStatus)
      return;
    string text = e.UserState != null ? e.UserState.ToString() : string.Empty;
    VisView.ThreadBuildSchemaUpdateInfo buildSchemaUpdate = this.BuildSchemaUpdate;
    if (buildSchemaUpdate == null)
      return;
    buildSchemaUpdate(text, (float) e.ProgressPercentage);
  }

  private bool NeedCancel(BackgroundWorker bw, DoWorkEventArgs e)
  {
    if (!bw.CancellationPending)
      return false;
    e.Cancel = true;
    return true;
  }

  internal BasePredicate CreateContinuePredicate(BackgroundWorker bw, DoWorkEventArgs e)
  {
    return (BasePredicate) (() => this.NeedCancel(bw, e));
  }

  private MapDocument CreateNewDocument()
  {
    MapDocument newDocument = new MapDocument();
    MapLayer newLayerAfter1 = newDocument.Layers.CreateNewLayerAfter(newDocument.Layers.Bottom);
    newLayerAfter1.Identifier = (object) 2;
    MapLayer newLayerAfter2 = newDocument.Layers.CreateNewLayerAfter(newDocument.Layers.Bottom);
    newLayerAfter2.Identifier = (object) 1;
    newLayerAfter1.AllowView = this.NeedChildTree;
    newLayerAfter2.AllowView = this.NeedParentTree;
    return newDocument;
  }

  internal void ShowLayers(bool needParents, bool needChilds)
  {
    foreach (MapLayer layer in this.map.Document.Layers)
    {
      if (layer.Identifier.Equals((object) 1))
        layer.AllowView = needParents;
      if (layer.Identifier.Equals((object) 2))
        layer.AllowView = needChilds;
    }
  }

  private void CentralizeScheme()
  {
    RectangleF documentBounds = this.map.ComputeDocumentBounds();
    PointF pointF1 = new PointF(documentBounds.Left + documentBounds.Width / 2f, documentBounds.Top + documentBounds.Height / 2f);
    RectangleF displayRectangle = (RectangleF) this.map.DisplayRectangle;
    PointF pointF2 = new PointF(displayRectangle.Left + displayRectangle.Width / 2f, displayRectangle.Top + displayRectangle.Height / 2f);
    SizeF shiftF = new SizeF(pointF2.X - pointF1.X, pointF2.Y - pointF1.Y);
    Size shift = new Size((int) Math.Round((double) shiftF.Width), (int) Math.Round((double) shiftF.Height));
    this.ShiftObject(this.curScheme.RootObj, shift, shiftF);
    if (this.curScheme.ParentLevels != null)
    {
      foreach (List<VisObject> parentLevel in this.curScheme.ParentLevels)
      {
        foreach (VisObject vo in parentLevel)
          this.ShiftObject(vo, shift, shiftF);
      }
    }
    if (this.curScheme.ChildLevels == null)
      return;
    foreach (List<VisObject> childLevel in this.curScheme.ChildLevels)
    {
      foreach (VisObject vo in childLevel)
        this.ShiftObject(vo, shift, shiftF);
    }
  }

  private void ShiftObject(VisObject vo, Size shift, SizeF shiftF)
  {
    vo.Org += shift;
    vo.Node.Location = vo.Node.Location + shiftF;
  }

  private void BuildThread(VisView.BuildFlags blf)
  {
    if (this.IsThreadBusy)
      return;
    this.LockControls(true);
    this.toolStripStatus_Stop.Visible = true;
    VisNode.Vertical = this.visLayout.Vertical;
    switch (blf)
    {
      case VisView.BuildFlags.CreateTree:
        this.newTreeWorker.RunWorkerAsync((object) this.newTreeWorker);
        break;
      case VisView.BuildFlags.UpdateTree:
        this.newTreeWorker.RunWorkerAsync((object) this.newTreeWorker);
        break;
      case VisView.BuildFlags.ReLayout:
        this.layoutWorker.RunWorkerAsync((object) this.layoutWorker);
        break;
    }
  }

  private void SetStatusInfoString(string statusInfo, float percent)
  {
    if (!statusInfo.Equals(string.Empty))
    {
      if ((double) percent >= 100.0)
        this.toolStripStatus.Text = statusInfo;
      else
        this.toolStripStatus.Text = $"{statusInfo}   {percent,1:F}%";
    }
    else
      this.toolStripStatus.Text = string.Empty;
  }

  private void SetFinishStatusTitle()
  {
    if (this.curSchemeInfo.IsEmpty || this.curScheme == null)
      return;
    long objectCount = (long) this.curScheme.ObjectCount;
    this.OnBuildStatusUpdate(this.curSchemeInfo.Caption + LocalizationHolder.rm.GetString("Pdm_rv_25") + (object) objectCount, 100f);
  }

  private void ReLayout(object sender, DoWorkEventArgs e)
  {
    if (this.curSchemeInfo.IsEmpty || this.curSchemeInfo.ObjectVersionId == 0L)
      return;
    MapDocument newDocument = this.CreateNewDocument();
    newDocument.UserObject = (object) this.settings;
    BackgroundWorker bw = sender as BackgroundWorker;
    bw.ReportProgress(0, (object) LocalizationHolder.rm.GetString("Pdm_rv_4"));
    if (this.NeedCancel(bw, e))
      return;
    BasePredicate continuePredicate = this.CreateContinuePredicate(bw, e);
    Size size = this.map.Size;
    this.visLayout.InitLayout(size, this.curScheme);
    this.visLayout.RestoreLevels(this.curScheme);
    this.visLayout.BeforeLayout(continuePredicate, this.curScheme);
    this.visLayout.SetInitialCoords(continuePredicate, this.curScheme, new Point(size.Width / 2, size.Height / 2));
    this.visLayout.DoLayout(continuePredicate, this.curScheme);
    bw.ReportProgress(100, (object) LocalizationHolder.rm.GetString("Pdm_rv_5"));
    e.Result = (object) new SchemCreator(this.curScheme, this.map, newDocument, this.previewMode);
  }

  internal void LayoutCompleted(object sender, RunWorkerCompletedEventArgs e)
  {
    try
    {
      if (e.Cancelled)
      {
        VisView.ThreadBuildSchemaUpdateInfo buildSchemaUpdate = this.BuildSchemaUpdate;
        if (buildSchemaUpdate == null)
          return;
        buildSchemaUpdate(LocalizationHolder.rm.GetString("Pdm_rv_32"), 0.0f);
      }
      else
      {
        SchemCreator schemCreator = e.Error == null ? (SchemCreator) e.Result : throw e.Error;
        MapDocument document = schemCreator.document;
        if (document != null)
        {
          schemCreator.CreateFullScheme();
          this.dbEvents.InitIdents(document);
          this.map.Document = document;
          this.ShowLayers(this.NeedParentTree, this.NeedChildTree);
          this.curScheme.Loaded.UpdateSettings(this.NeedParentTree, this.NeedChildTree, this.ShowStructLinks, this.ShowAssociativeLinks);
        }
        if (this.setCenterNode)
        {
          MapLayer mapLayer = this.map.Document.Layers.Find((object) 0);
          if (mapLayer != null)
          {
            VisNode firstObject = (VisNode) mapLayer.FirstObject;
            this.map.DocPosition = new PointF(firstObject.Center.X - this.map.DocExtentSize.Width / 2f, firstObject.Center.Y - this.map.DocExtentSize.Height / 2f);
          }
          this.setCenterNode = false;
        }
        long objectCount = (long) this.curScheme.ObjectCount;
        string text = this.curSchemeInfo.Caption + LocalizationHolder.rm.GetString("Pdm_rv_25") + (object) objectCount;
        VisView.ThreadBuildSchemaUpdateInfo buildSchemaUpdate = this.BuildSchemaUpdate;
        if (buildSchemaUpdate == null)
          return;
        buildSchemaUpdate(text, 100f);
      }
    }
    finally
    {
      this.LockControls(false);
      this.toolStripStatus_Stop.Visible = false;
    }
  }

  private void LayoutDocument(MapDocument document)
  {
    this.UpdateLayers(document);
    if (this.currentLayoutAlgorithm == null)
      return;
    this.currentLayoutAlgorithm.LayoutDocument(document, this.map.Size);
  }

  private void shLayout_Progress(object sender, MapLayoutProgressEventArgs e)
  {
    float percent = e.Progress * 100f;
    this.OnBuildStatusUpdate(LocalizationHolder.rm.GetString("Pdm_rv_4"), percent);
  }

  private void RelationVisualiserWindow_Closed(object sender, EventArgs e)
  {
    if (this.HideOnClose)
      return;
    this.Do_DeleteFiltrationSettings();
  }

  private void RW_BuildSсhemeUpdate(string text, float percent)
  {
    if (this.InvokeRequired)
      this.Invoke((Delegate) this.BuildSchemaUpdate, (object) text, (object) percent);
    else
      this.SetStatusInfoString(text, percent);
  }

  private void RW_ThreadFinish(MapDocument doc, Exception exception)
  {
    if (exception is ThreadAbortException)
      return;
    if (this.InvokeRequired)
    {
      this.Invoke((Delegate) this.ThreadFinish, (object) doc, (object) exception);
    }
    else
    {
      if (exception != null)
        ExceptionHelper.ExceptionService.ShowException(new Exception(LocalizationHolder.rm.GetString("Pdm_rv_19") + exception.Message, exception));
      if (doc == null)
        return;
      this.map.Document = doc;
      if (this.setCenterNode)
      {
        MapLayer mapLayer = this.map.Document.Layers.Find((object) 0);
        if (mapLayer != null)
          this.map.ScrollToControl(mapLayer.FirstObject);
        this.setCenterNode = false;
      }
      this.SetFinishStatusTitle();
    }
  }

  private void InitializeServices()
  {
    this._services = new AdvancedServiceContainer();
    this._notificationService = this.InitializeNotificationService();
    if (this._notificationService != null)
      this._services.AddService(typeof (INotificationService), (object) this._notificationService);
    this._images = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
    this._filtrationService = this.InitializeFiltrationService();
    this._FiltrationClass = (IFiltrationClass) this;
    this._services.AddService(typeof (IFiltrationClass), (object) this._FiltrationClass);
    if (ServicesManager.GetService(typeof (IConfigurationManager)) is IConfigurationManager service)
      service.ConfigurationBeforeSave += new ConfigurationBeforeSaveEventHandler(this.confManager_ConfigurationBeforeSave);
    this._cmdMngr = ServicesManager.GetService(typeof (ICommandManager)) as ICommandManager;
    if (this._cmdMngr != null)
      this._services.AddService(typeof (ICommandManager), (object) this._cmdMngr);
    this._elementStatusesClientService = ServicesManager.GetService(typeof (IElementStatusesClientService)) as IElementStatusesClientService;
    if (this._elementStatusesClientService != null)
      this._services.AddService(typeof (IElementStatusesClientService), (object) this._elementStatusesClientService);
    this._CurrentUserAndRole = ServicesManager.GetService(typeof (ICurrentUserAndRole)) as ICurrentUserAndRole;
    if (this._CurrentUserAndRole != null)
      this._services.AddService(typeof (ICurrentUserAndRole), (object) this._CurrentUserAndRole);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this.serverService = sessionKeeper.Session.GetCustomService(typeof (IVisualizerService)) as IVisualizerService;
  }

  private void confManager_ConfigurationBeforeSave(IConfigurationManager configurationManager)
  {
    if (this.IsInContainer)
      return;
    this.Do_DeleteFiltrationSettings();
  }

  private void DisposeServices()
  {
    this._services.RemoveService(typeof (INotificationService));
    this._notificationService = (Intermech.Client.Core.NotificationService) null;
    this._images = (INamedImageList) null;
    this._FiltrationClass = (IFiltrationClass) null;
    this._services = (AdvancedServiceContainer) null;
  }

  public override void Activated()
  {
    base.Activated();
    if (!this._activated)
      this.EnableNotifications(this.NotificationService, this.IsOpen | UISettings.AutoupdateNonActiveWindows);
    if (!this._loaded)
      this._loaded = true;
    this._filtrationService.OnFiltrationChanged -= new FiltrationChanged(this._filtrationService_OnFiltrationChanged);
    this.FiltrationInitToolbar();
    this._filtrationService.OnFiltrationChanged += new FiltrationChanged(this._filtrationService_OnFiltrationChanged);
    this.UpdateControls();
    this.UpdateCurSchemeInfo(DBEvent.DBEventType.All);
    this._activated = true;
  }

  public override void Deactivated()
  {
    base.Deactivated();
    if (this._activated)
      this.EnableNotifications(this.NotificationService, this.IsOpen | UISettings.AutoupdateNonActiveWindows);
    this._filtrationService.OnFiltrationChanged -= new FiltrationChanged(this._filtrationService_OnFiltrationChanged);
    this.UpdateControls();
    this.FiltrationClearToolbar();
    this._activated = false;
  }

  protected virtual Intermech.Client.Core.NotificationService InitializeNotificationService()
  {
    SwitchedNotificationService notificationService = new SwitchedNotificationService();
    notificationService.Parent = ServicesManager.GetService(typeof (INotificationService)) as Intermech.Client.Core.NotificationService;
    notificationService.Parent.Subscribe(new NotificationEventHandler(this.OnGetNotificationEvent));
    return (Intermech.Client.Core.NotificationService) notificationService;
  }

  private void OnGetNotificationEvent(object sender, NotificationEventArgs args)
  {
    try
    {
      string eventName = args.EventName;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(eventName))
      {
        case 1430399058:
          if (!(eventName == "RelationsChanged") || !(args is DBRelationsExtendedEventArgs extendedEventArgs))
            return;
          for (int index = 0; index < extendedEventArgs.AttributeValuesArray.Length; ++index)
          {
            AttributeValues attributeValues = extendedEventArgs.AttributeValuesArray[index];
            if (attributeValues.AttributeID == this.attrCountTypeID && !attributeValues.IsNew && attributeValues.Values.Length != 0)
            {
              long relationId = extendedEventArgs.RelationIDs[index];
              object quan = attributeValues.Values[0];
              if (!(quan is MeasuredValue))
                quan = (object) this.ConvertAttrCountValue(quan.ToString(), attributeValues.AttributeType);
              this.dbEvents.Add(new VisDBEvent(relationId, (MeasuredValue) quan));
            }
          }
          return;
        case 1484628706:
          if (!(eventName == "RelationsCreated") || !(args is DBRelationsEventArgs relationsEventArgs1))
            return;
          using (IEnumerator<long> enumerator = relationsEventArgs1.RelationIDs.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              long current = enumerator.Current;
              this.dbEvents.Add(new VisDBEvent(EvCode.RelationCreated, relationsEventArgs1.GetProjID(current), current));
            }
            return;
          }
        case 1868964354:
          if (!(eventName == "RelationsRemoved") || !(args is DBRelationsEventArgs relationsEventArgs2))
            return;
          using (IEnumerator<long> enumerator = relationsEventArgs2.RelationIDs.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              long current = enumerator.Current;
              this.dbEvents.Add(new VisDBEvent(EvCode.RelationDeleted, relationsEventArgs2.GetProjID(current), current));
            }
            return;
          }
        case 2108022063:
          if (!(eventName == "ObjectsChangesCancelled"))
            return;
          break;
        case 2621053161:
          if (!(eventName == "ObjectsRemoved") || !(args is DBObjectsEventArgs objectsEventArgs1))
            return;
          using (IEnumerator<long> enumerator = objectsEventArgs1.ObjectIDs.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.dbEvents.Add(new VisDBEvent(EvCode.ObjectDeleted, enumerator.Current));
            return;
          }
        case 2691487867:
          if (!(eventName == "ObjectsCheckedIn"))
            return;
          break;
        case 3096070312:
          if (!(eventName == "ObjectsCheckedOut"))
            return;
          break;
        case 3837095985:
          if (!(eventName == "ObjectsChanged") || !(args is DBObjectsEventArgs objectsEventArgs2))
            return;
          using (IEnumerator<long> enumerator = objectsEventArgs2.ObjectIDs.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.dbEvents.Add(new VisDBEvent(EvCode.ObjectModified, enumerator.Current));
            return;
          }
        default:
          return;
      }
      if (!(args is DBObjectsEventArgs objectsEventArgs3))
        return;
      foreach (long objectId in (IEnumerable<long>) objectsEventArgs3.ObjectIDs)
        this.dbEvents.Add(new VisDBEvent(EvCode.ObjectIdChanged, objectId));
    }
    catch
    {
    }
  }

  private MeasuredValue ConvertAttrCountValue(string countStr, FieldTypes fType)
  {
    long measureId = MeasureHelper.GetMeasureID(SystemGUIDs.objectShtukiGuid);
    try
    {
      if (fType == FieldTypes.ftMeasured)
      {
        MeasuredValue measuredValue = this.measureConvertor.ConvertToMeasuredValue(countStr);
        if (measuredValue != null)
          return measuredValue;
      }
      else
      {
        double result = 0.0;
        if (double.TryParse(countStr, out result))
          return new MeasuredValue(result, measureId);
      }
    }
    catch (Exception ex)
    {
      throw new Exception(LocalizationHolder.rm.GetString("Pdm_rv_18"), ex);
    }
    return new MeasuredValue(0.0, measureId);
  }

  protected virtual void DisposeNotificationService(INotificationService notificationService)
  {
    ((IDisposable) notificationService).Dispose();
  }

  protected virtual void EnableNotifications(INotificationService notificationService, bool enabled)
  {
    if (!(notificationService is SwitchedNotificationService notificationService1))
      return;
    notificationService1.Enabled = enabled;
  }

  protected INotificationService NotificationService
  {
    get => (INotificationService) this._notificationService;
  }

  public virtual void UpdateControls()
  {
    if (this.bAssociateLinks.Checked != this.ShowAssociativeLinks)
      this.bAssociateLinks.Checked = this.ShowAssociativeLinks;
    if (this.bStructureLinks.Checked == this.ShowStructLinks)
      return;
    this.bStructureLinks.Checked = this.ShowStructLinks;
  }

  protected override string GetPersistString()
  {
    try
    {
      XmlDocument state = this.GetState();
      using (TextWriter w1 = (TextWriter) new StringWriter())
      {
        XmlWriter w2 = (XmlWriter) new XmlTextWriter(w1);
        state.WriteTo(w2);
        w2.Flush();
        w2.Close();
        return w1.ToString();
      }
    }
    catch (Exception ex)
    {
      ExceptionHelper.ExceptionService.ShowException(ex);
      return (string) null;
    }
  }

  protected virtual XmlDocument GetState()
  {
    XmlDocument state = new XmlDocument();
    XmlNode element = (XmlNode) state.CreateElement("RelationVisualizerView");
    state.AppendChild((XmlNode) state.CreateXmlDeclaration("1.0", (string) null, (string) null));
    if (!this.curSchemeInfo.IsEmpty)
    {
      XmlAttribute attribute = state.CreateAttribute("ObjId");
      attribute.Value = this.curSchemeInfo.ObjectVersionId.ToString();
      element.Attributes.Append(attribute);
    }
    if (this._FiltrationOwnerID.Length > 0)
    {
      XmlAttribute attribute = state.CreateAttribute("FiltrationOwnerID");
      attribute.Value = Convert.ToString(this.Get_FiltrationOwnerID());
      element.Attributes.Append(attribute);
    }
    XmlAttribute attribute1 = state.CreateAttribute("ShowParentTree");
    attribute1.Value = Convert.ToString(this.NeedParentTree);
    element.Attributes.Append(attribute1);
    XmlAttribute attribute2 = state.CreateAttribute("ShowChildTree");
    attribute2.Value = Convert.ToString(this.NeedChildTree);
    element.Attributes.Append(attribute2);
    state.AppendChild(element);
    return state;
  }

  protected virtual SchemeInfo RestoreState(XmlDocument xmlDoc)
  {
    SchemeInfo schemeInfo = SchemeInfo.Empty;
    if (xmlDoc != null && xmlDoc.ChildNodes.Count > 1)
    {
      XmlAttributeCollection attributes = xmlDoc.ChildNodes[1].Attributes;
      if (attributes != null)
      {
        XmlAttribute xmlAttribute1 = attributes["ObjId"];
        if (xmlAttribute1 != null)
        {
          string s = xmlAttribute1.Value;
          long objectID = 0;
          ref long local = ref objectID;
          if (long.TryParse(s, out local))
          {
            using (SessionKeeper sessionKeeper = new SessionKeeper())
            {
              IDBObject dbObject = sessionKeeper.Session.GetObject(objectID);
              if (dbObject != null)
              {
                if (this.visLayout == null)
                  this.visLayout = (IVisLayout) this.STANDARD_LAYOUT;
                schemeInfo = new SchemeInfo(dbObject.ObjectID, dbObject.ID, dbObject.Caption, dbObject.ObjectType, this.visLayout.GetLayoutKind(), this.settings);
              }
            }
          }
        }
        if (attributes != null)
        {
          XmlAttribute xmlAttribute2 = attributes["FiltrationOwnerID"];
          if (xmlAttribute2 != null)
          {
            string str = xmlAttribute2.Value;
            if (!str.Equals(string.Empty) && this._FiltrationOwnerID != str)
            {
              using (SessionKeeper sessionKeeper = new SessionKeeper())
                (sessionKeeper.Session.GetCustomService(typeof (IVersionRulesCacheService)) as IVersionRulesCacheService).SetFiltrationSettings((object) sessionKeeper.Session.SessionGUID, this._FiltrationOwnerID, (FiltrationSettings) null);
              this._FiltrationOwnerID = str;
            }
          }
        }
        XmlAttribute xmlAttribute3 = attributes["ShowParentTree"];
        if (xmlAttribute3 != null)
        {
          bool result = false;
          if (bool.TryParse(xmlAttribute3.Value, out result))
            this.NeedParentTree = result;
        }
        XmlAttribute xmlAttribute4 = attributes["ShowChildTree"];
        if (xmlAttribute4 != null)
        {
          bool result = false;
          if (bool.TryParse(xmlAttribute4.Value, out result))
            this.NeedChildTree = result;
        }
      }
    }
    return schemeInfo;
  }

  public static DockControl RestoreWindowCallback(Guid guid, string persistString)
  {
    if (!guid.Equals(VisView._persistStateGuid))
      return (DockControl) null;
    try
    {
      XmlDocument xmlDoc = new XmlDocument();
      xmlDoc.LoadXml(persistString);
      VisView window = new VisView();
      SchemeInfo newScheme = window.RestoreState(xmlDoc);
      if (newScheme.IsEmpty)
        return (DockControl) null;
      ((IWellKnownNavigators) ServicesManager.GetService(typeof (IWellKnownNavigators)))?.Register("newRelationVisualizerWindow", (Control) window);
      VisStatusKeeper.Init();
      using (SessionKeeper sessionKeeper = new SessionKeeper())
        window._DoLoadStyle(sessionKeeper.Session, PDMPlugin._visStylesId);
      window.SetCurrentObject(newScheme);
      return (DockControl) window;
    }
    catch (Exception ex)
    {
      IOutputView service = ServicesManager.GetService(typeof (IOutputView)) as IOutputView;
      service.WriteString("Navigator", LocalizationHolder.rm.GetString("Client.Core_326"));
      service.WriteString("Navigator", ex.Message);
      return (DockControl) null;
    }
  }

  private void AddScheme2HistoryList(SchemeInfo oldSchema)
  {
    if (oldSchema.IsEmpty)
      return;
    foreach (MenuItemBase menuItemBase in (CollectionBase) this.dropDownMenu_FromHistory.Items)
    {
      if (menuItemBase.Tag != null && menuItemBase.Tag.Equals((object) oldSchema))
        return;
    }
    MenuButtonItem menuButtonItem = new MenuButtonItem(oldSchema.ToString(), new EventHandler(this.HistoryPressed));
    menuButtonItem.Tag = (object) oldSchema;
    this.dropDownMenu_FromHistory.Items.Add((ToolbarItemBase) menuButtonItem);
  }

  private void ShowCurrentSchemeInHistoryMenu(SchemeInfo newSchema)
  {
    foreach (MenuItemBase menuItemBase in (CollectionBase) this.dropDownMenu_FromHistory.Items)
    {
      if (menuItemBase.Tag != null && menuItemBase.Tag.Equals((object) newSchema))
        menuItemBase.Checked = true;
      else
        menuItemBase.Checked = false;
    }
  }

  protected override void OnParentChanged(EventArgs e)
  {
    base.OnParentChanged(e);
    if (this.Parent == null || this.curSchemeInfo.IsEmpty || this.defSchemeInfo.IsEmpty)
      return;
    this.SetCurrentObject(this.defSchemeInfo);
  }

  public void SetCurrentObject(SchemeInfo newScheme)
  {
    if (!this.curSchemeInfo.IsEmpty && this.Parent == null)
    {
      this.defSchemeInfo = newScheme;
    }
    else
    {
      if (!this.curSchemeInfo.IsEmpty && newScheme.Equals((object) this.curSchemeInfo))
        return;
      if (!this.curSchemeInfo.IsEmpty && !this.schemeList.ContainsKey(this.curSchemeInfo))
        this.schemeList.Add(this.curSchemeInfo, new SchemePackage(this.curScheme, this.map.Document));
      if (!this.curSchemeInfo.IsEmpty)
        this.AddScheme2HistoryList(this.curSchemeInfo);
      this.ShowCurrentSchemeInHistoryMenu(newScheme);
      this.curSchemeInfo = newScheme;
      string caption = this.curSchemeInfo.Caption;
      this.TabText = caption;
      this.Text = caption;
      this.setCenterNode = true;
      this.BuildThread(VisView.BuildFlags.CreateTree);
    }
  }

  public void SetCurrentObject(IDBTypedObjectID ObjID)
  {
    if (ObjID == null || ObjID.ObjectID == 0L)
      return;
    this.SetCurrentObject(new SchemeInfo(ObjID.ObjectID, ObjID.ID, ObjID.Caption, ObjID.ObjectType, this.visLayout.GetLayoutKind(), this.settings));
  }

  public void LaunchForObject(IDBTypedObjectID objId, bool showChilds, bool showParents)
  {
    if (objId == null || objId.ObjectID == 0L)
      return;
    SchemeInfo newScheme = new SchemeInfo(objId.ObjectID, objId.ID, objId.Caption, objId.ObjectType, this.visLayout.GetLayoutKind(), this.settings);
    this.NeedChildTree = showChilds;
    this.NeedParentTree = showParents;
    this.SetCurrentObject(newScheme);
  }

  public static string ChooseScheme(bool сomposition, ref long schemeId)
  {
    long[] numArray = Intermech.Navigator.SelectionWindow.SelectObjects(LocalizationHolder.rm.GetString("Pdm_rv_40"), LocalizationHolder.rm.GetString(сomposition ? "Pdm_rv_41" : "Pdm_rv_42"), ObjectTypesHelper.GetObjTypeID(VisView.visSchemeGuid), SelectionOptions.SelectObjects | SelectionOptions.DisableMultiselect);
    if (numArray == null || numArray.Length == 0)
      return string.Empty;
    schemeId = numArray[0];
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(schemeId);
      return objectInfo.Empty ? string.Empty : objectInfo.Caption;
    }
  }

  private bool ChangeLayoutAlgorithm(LayoutKind layKind)
  {
    if (this.visLayout.GetLayoutKind() == layKind)
      return false;
    if (HierarchLayout.GetKind() == layKind)
    {
      this.visLayout = (IVisLayout) this.STANDARD_LAYOUT;
      this.mbiLayoutHierarch.Checked = true;
      this.mbiLayoutKulon.Checked = false;
      this.mbiLayoutNormal.Checked = false;
      return true;
    }
    if (StandardLayout.GetKind() != layKind)
      return false;
    this.visLayout = (IVisLayout) this.HIER_LAYOUT;
    this.mbiLayoutHierarch.Checked = false;
    this.mbiLayoutKulon.Checked = false;
    this.mbiLayoutNormal.Checked = true;
    return true;
  }

  private void DeleteRelation()
  {
    if (this.currentSelectedRelation == null || this.currentSelectedRelation.RelId == 0L)
      return;
    string caption = LocalizationHolder.rm.GetString("Pdm_rv_57");
    string str1 = "";
    string str2 = "";
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBRelation relation = sessionKeeper.Session.GetRelation(this.currentSelectedRelation.RelId, false);
      if (relation == null)
        return;
      str1 = $"[{(object) relation.ProjObject.ObjectID}] {relation.ProjObject.Caption}";
      if (relation.PartObject != null)
        str2 = $"[{(object) relation.PartObject.ObjectID}] {relation.PartObject.Caption}";
      else
        str2 = $"[{(object) relation.PartID}]";
    }
    string str3 = string.Format(LocalizationHolder.rm.GetString("Pdm_rv_55"), (object) str1, (object) str2);
    if (MessageBox.Show($"{LocalizationHolder.rm.GetString("Pdm_rv_56")}\n\n{str3}", caption, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(-1, this.Get_FiltrationOwnerID());
      if (relationCollection == null)
        return;
      if (relationCollection.Delete(new long[1]
      {
        this.currentSelectedRelation.RelId
      }, true, 0L) != 1)
        return;
      this.DeleteRelation(this.currentSelectedRelation);
      this.NotificationService.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsRemoved", this.currentSelectedRelation.RelId));
      this.currentSelectedRelation = (VisLink) null;
    }
  }

  internal void DeleteRelation(VisLink rel) => rel.Remove();

  private void map_OnRelationCreated(VisNode from, VisNode to, IMapLink il)
  {
    bool flag = false;
    try
    {
      if (il == null || !(il is VisLink))
        return;
      VisLink visLink = il as VisLink;
      if (from == null || to == null || from.ObjId == 0L || to.ObjId == 0L)
        return;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        int typeBetweenObjTypes = VisRelation.GetRelTypeBetweenObjTypes(from.ObjTypeId, to.ObjTypeId, sessionKeeper.Session);
        if (typeBetweenObjTypes == -1)
          throw new Exception(LocalizationHolder.rm.GetString("Pdm_rv_2"));
        IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(typeBetweenObjTypes);
        if (relationCollection == null)
          return;
        IDBRelation dbRelation = relationCollection.Create(to.ObjId, from.ObjId);
        if (dbRelation == null)
          return;
        flag = true;
        long relationId = dbRelation.RelationID;
        visLink.RelId = relationId;
        this.NotificationService.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsCreated", relationId));
      }
    }
    finally
    {
      il.MapObject.Remove();
      if (flag)
        this.BuildThread(VisView.BuildFlags.UpdateTree);
    }
  }

  private void map_SelectionDeleted(object sender, EventArgs e) => this.DeleteRelation();

  private void map_ClipboardPasted(object sender, EventArgs e) => this.ExecuteMenuCommand("Paste");

  private void map_ObjectContextClicked(object sender, MapObjectEventArgs e)
  {
    if (e.MapObject != null && e.MapObject.ParentNode != null && e.MapObject.ParentNode is VisNode)
    {
      if (this.currentSelectedObjects == null)
        return;
      this.GetContextMenu4Object(this.currentSelectedObjects).Show((Control) this.map, e.ViewPoint);
      this.UpdateCurSchemeInfo(DBEvent.DBEventType.All);
    }
    else
    {
      if (e.MapObject == null || e.MapObject.ParentNode == null || !(e.MapObject.ParentNode is VisLink))
        return;
      this.currentSelectedRelation = e.MapObject.ParentNode as VisLink;
      this.contextMenuStrip_map.Show((Control) this.map, e.ViewPoint);
    }
  }

  private MenuBarItem GetContextMenu4Object(ISelectedItems items)
  {
    AdvancedServiceContainer viewServices = new AdvancedServiceContainer();
    viewServices.AddService(typeof (IViewState), (object) new ViewStateService());
    MenuBarItem menu = Intermech.Navigator.ContextMenu.Services.GetMenu(items, (IServiceProvider) viewServices);
    MenuItemBase menuItemBase1 = menu.FindItem("PDM.VisualizerRoot");
    if (menuItemBase1 != null)
    {
      menu.Items.Remove((ToolbarItemBase) menuItemBase1);
      MenuItemBase menuItemBase2 = (MenuItemBase) new MenuButtonItem(LocalizationHolder.rm.GetString("Pdm_rv_17"), new EventHandler(this.newRoot_Click));
      menu.Items.Insert(0, (ToolbarItemBase) menuItemBase2);
      MenuItemBase menuItemBase3 = (MenuItemBase) new MenuButtonItem(LocalizationHolder.rm.GetString("Pdm_rv_43"));
      MenuItemBase menuItemBase4 = (MenuItemBase) new MenuButtonItem(LocalizationHolder.rm.GetString("Pdm_rv_44"), new EventHandler(this.expandStru_Click));
      menuItemBase4.Tag = (object) 5;
      MenuItemBase menuItemBase5 = (MenuItemBase) new MenuButtonItem(LocalizationHolder.rm.GetString("Pdm_rv_45"), new EventHandler(this.expandStru_Click));
      menuItemBase5.Tag = (object) 3;
      MenuItemBase menuItemBase6 = (MenuItemBase) new MenuButtonItem(LocalizationHolder.rm.GetString("Pdm_rv_46"), new EventHandler(this.expandStru_Click));
      menuItemBase6.Tag = (object) 1;
      menuItemBase3.Items.Add((ToolbarItemBase) menuItemBase4);
      menuItemBase3.Items.Add((ToolbarItemBase) menuItemBase5);
      menuItemBase3.Items.Add((ToolbarItemBase) menuItemBase6);
      menu.Items.Insert(1, (ToolbarItemBase) menuItemBase3);
    }
    MenuItemBase menuItemBase7 = menu.FindItem("Cut");
    if (menuItemBase7 != null)
      menuItemBase7.Enabled = false;
    MenuItemBase menuItemBase8 = menu.FindItem("Paste");
    if (menuItemBase8 != null)
      menuItemBase8.Enabled = false;
    return menu;
  }

  private void newRoot_Click(object sender, EventArgs e)
  {
    if (this.currentSelectedObjects == null || this.currentSelectedObjects.Count == 0 || !(this.currentSelectedObjects.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData))
      return;
    this.SetCurrentObject(itemData);
  }

  private void expandStru_Click(object sender, EventArgs e)
  {
    if (this.currentSelectedObjects == null || this.currentSelectedObjects.Count == 0 || !(this.currentSelectedObjects.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData) || sender == null)
      return;
    int tag = (int) (sender as MenuButtonItem).Tag;
    this.LockControls(true);
    this.toolStripStatus_Stop.Visible = true;
    this.expandWorker.RunWorkerAsync((object) new Tuple<long, int>(itemData.ObjectID, tag));
  }

  private void ExpandObjStru(object sender, DoWorkEventArgs e)
  {
    if (!(e.Argument is Tuple<long, int> tuple1))
      return;
    long objId = Math.Abs(tuple1.Item1);
    int maxLevels = tuple1.Item2;
    (bool, bool) tuple2 = this.curScheme.NeedExpandChildren(objId);
    if (tuple2.Item2)
      objId = tuple1.Item1;
    MapDocument newDocument = this.CreateNewDocument();
    newDocument.UserObject = (object) this.settings;
    VisStatusKeeper.UpdateDisabledPlugins();
    if (!(sender is BackgroundWorker bw))
      return;
    if (tuple2.Item2)
    {
      bw.ReportProgress(10, (object) LocalizationHolder.rm.GetString("Pdm_rv_8"));
      this.curScheme.ExpandChilds(bw, objId, maxLevels);
      if (this.NeedCancel(bw, e))
        return;
    }
    if (tuple2.Item1)
    {
      bw.ReportProgress(10, (object) LocalizationHolder.rm.GetString("Pdm_rv_7"));
      this.curScheme.ExpandParents(bw, objId, maxLevels);
      if (this.NeedCancel(bw, e))
        return;
    }
    bw.ReportProgress(50, (object) LocalizationHolder.rm.GetString("Pdm_rv_4"));
    BasePredicate continuePredicate = this.CreateContinuePredicate(bw, e);
    Size size = this.map.Size;
    this.visLayout.InitLayout(size, this.curScheme);
    this.visLayout.BeforeLayout(continuePredicate, this.curScheme);
    this.visLayout.SetInitialCoords(continuePredicate, this.curScheme, new Point(size.Width / 2, size.Height / 2));
    this.visLayout.DoLayout(continuePredicate, this.curScheme);
    bw.ReportProgress(100, (object) LocalizationHolder.rm.GetString("Pdm_rv_5"));
    e.Result = (object) (new SchemCreator(this.curScheme, this.map, newDocument, this.previewMode), objId, tuple2.Item2);
  }

  internal void ExpandCompleted(object sender, RunWorkerCompletedEventArgs e)
  {
    try
    {
      if (e.Cancelled)
      {
        VisView.ThreadBuildSchemaUpdateInfo buildSchemaUpdate = this.BuildSchemaUpdate;
        if (buildSchemaUpdate == null)
          return;
        buildSchemaUpdate(LocalizationHolder.rm.GetString("Pdm_rv_32"), 0.0f);
      }
      else
      {
        (SchemCreator schemCreator, long objId, bool childs) = e.Error == null ? ((SchemCreator, long, bool)) e.Result : throw e.Error;
        MapDocument document = schemCreator.document;
        if (document != null)
        {
          schemCreator.CreateFullScheme();
          this.visLayout.ProcessInvisible(schemCreator.scheme);
          SchemCreator.MarkInvisibleChanged(schemCreator.scheme);
          SchemCreator.ProcessInvisible(schemCreator.scheme);
          this.dbEvents.InitIdents(document);
          this.map.Document = document;
          this.ShowLayers(this.NeedParentTree, this.NeedChildTree);
          this.curScheme.Loaded.UpdateSettings(this.NeedParentTree, this.NeedChildTree, this.ShowStructLinks, this.ShowAssociativeLinks);
        }
        this.map.ScrollToControl((MapObject) schemCreator.scheme.FindVisNode(objId, childs));
        int num = schemCreator.scheme.CalcObjectCount();
        schemCreator.scheme.ObjectCount = num;
        string text = this.curSchemeInfo.Caption + LocalizationHolder.rm.GetString("Pdm_rv_25") + (object) num;
        VisView.ThreadBuildSchemaUpdateInfo buildSchemaUpdate = this.BuildSchemaUpdate;
        if (buildSchemaUpdate == null)
          return;
        buildSchemaUpdate(text, 100f);
      }
    }
    finally
    {
      this.LockControls(false);
      this.toolStripStatus_Stop.Visible = false;
    }
  }

  private void map_BackgroundSingleClicked(object sender, MapInputEventArgs e)
  {
    this.currentSelectedObjects = (ISelectedItems) null;
    this._cmdMngr.QueryStatus();
  }

  private void map_ObjectGotSelection(object sender, MapSelectionEventArgs e)
  {
    if (e.MapObject != null)
    {
      if (e.MapObject is VisNode)
      {
        VisNode mapObject = e.MapObject as VisNode;
        ISelectedItems items = Intermech.Navigator.ContextMenu.Services.GetItems(mapObject.ObjId);
        if (items.Count == 0)
          items = Intermech.Navigator.ContextMenu.Services.GetItems(-mapObject.ObjId);
        this.ShowObjectProps(mapObject, false);
        this.currentSelectedObjects = items;
      }
      else
      {
        if (e.MapObject is VisLink)
        {
          this.currentSelectedRelation = e.MapObject as VisLink;
          this.ShowRelationProps(this.currentSelectedRelation, false);
        }
        else
          this.ShowObjectProps((VisNode) null, false);
        this.currentSelectedObjects = (ISelectedItems) null;
      }
      this._cmdMngr.QueryStatus();
    }
    else
      this.currentSelectedObjects = (ISelectedItems) null;
  }

  private void map_PortDoubleClicked(VisNodePort sender, MapInputEventArgs evt)
  {
    if (this.IsThreadBusy)
      return;
    this.LockControls(true);
    this.map.BeginUpdate();
    try
    {
      if (sender.LeftSide)
        sender.Obj.ParentsOpen = !sender.Obj.ParentsOpen;
      else
        sender.Obj.ChildsOpen = !sender.Obj.ChildsOpen;
      this.visLayout.ProcessInvisible(this.curScheme);
      SchemCreator.ProcessInvisible(this.curScheme);
      bool? open1 = sender.Open;
      bool flag1 = true;
      if (open1.GetValueOrDefault() == flag1 & open1.HasValue)
      {
        sender.Open = new bool?(false);
      }
      else
      {
        bool? open2 = sender.Open;
        bool flag2 = false;
        if (!(open2.GetValueOrDefault() == flag2 & open2.HasValue))
          return;
        sender.Open = new bool?(true);
      }
    }
    finally
    {
      this.map.EndUpdate();
      this.LockControls(false);
    }
  }

  private void toolStripButton_ZoomIn_Click(object sender, EventArgs e) => this.map.ZoomIn();

  private void toolStripButton_ZoomOut_Click(object sender, EventArgs e) => this.map.ZoomOut();

  private void toolStripButton_WidthHeigth_Click(object sender, EventArgs e)
  {
    int xCoef = 100;
    int yCoef = 100;
    if (sender == this.buttonItem_HeigthIn)
      yCoef = 80 /*0x50*/;
    else if (sender == this.buttonItem_HeigthOut)
      yCoef = 125;
    else if (sender == this.buttonItem_WidthIn)
      xCoef = 80 /*0x50*/;
    else if (sender == this.buttonItem_WidthOut)
      xCoef = 125;
    try
    {
      this.lockUpdateStatus = true;
      this.visLayout.ChangeSizes(this.curScheme, xCoef, yCoef);
      if (this.map.Selection.First == null)
        return;
      this.map.ScrollToControl(this.map.Selection.First);
    }
    finally
    {
      this.lockUpdateStatus = false;
    }
  }

  private void toolStripButton_LevelP_Click(object sender, EventArgs e)
  {
    this.settings.ShowLifecycleLevel = this.buttonItem_LevelP.Checked;
    DrawSettings ds = this.map.Document.UserObject as DrawSettings;
    if (ds != null)
      ds.ShowLifecycleLevel = this.buttonItem_LevelP.Checked;
    this.curScheme.ForEachObject((Action<VisObject>) (visObj => visObj.Node.UpdateAllSettings((IDrawSettings) ds)));
    this.map.Invalidate();
  }

  private void toolStripButton_Status_Click(object sender, EventArgs e)
  {
    this.settings.ShowStatuses = this.buttonItem_Status.Checked;
    DrawSettings ds = this.map.Document.UserObject as DrawSettings;
    if (ds != null)
      ds.ShowStatuses = this.buttonItem_Status.Checked;
    this.curScheme.ForEachObject((Action<VisObject>) (visObj => visObj.Node.UpdateAllSettings((IDrawSettings) ds)));
    this.map.Invalidate();
  }

  private void ToFitCurDocument(object sender, EventArgs e) => this.map.ZoomToFit();

  private void toolStripButton_ZoomOnce_Click(object sender, EventArgs e)
  {
    this.map.ZoomOnceCurDocument();
    this.map.ScrollToControl((MapObject) this.curScheme.RootObj.Node);
  }

  private void LockControls(bool locked) => this.toolBar.Enabled = !locked;

  private ReloadDecision MakeUpdateDecision()
  {
    ReloadDecision reloadDecision = this.curSchemeInfo.NeedUpdateTree(this.curScheme.Loaded, this.NeedParentTree, this.NeedChildTree, this.ShowStructLinks, this.ShowAssociativeLinks);
    switch (reloadDecision)
    {
      case ReloadDecision.NoReload:
        this.LayoutDocument(this.map.Document);
        break;
      case ReloadDecision.PartReload:
        this.BuildThread(VisView.BuildFlags.UpdateTree);
        break;
      case ReloadDecision.FullReload:
        this.BuildThread(VisView.BuildFlags.CreateTree);
        break;
    }
    return reloadDecision;
  }

  private void toolStripButton_ifNeedChildTree_Click(object sender, EventArgs e)
  {
    this.NeedChildTree = !this.NeedParentTree || !this.buttonItem_IfNeedChildTree.Checked;
    this.map.Document.Layers.Find((object) 2).AllowView = this.NeedChildTree;
    int num = (int) this.MakeUpdateDecision();
  }

  private void toolStripButton_ifNeedParentTree_Click(object sender, EventArgs e)
  {
    this.NeedParentTree = !this.NeedChildTree || !this.buttonItem_IfNeedParentTree.Checked;
    this.map.Document.Layers.Find((object) 1).AllowView = this.NeedParentTree;
    int num = (int) this.MakeUpdateDecision();
  }

  private VisRelation _AddRel(long relId, VisObject parObj, VisObject childObj, CADRelType crType)
  {
    VisRelation visRelation = new VisRelation((IVisRelationData) new VisRelData(relId, 1, parObj.ObjVerId, childObj.ObjVerId, (MeasuredValue) null, crType));
    visRelation.Parent = parObj;
    visRelation.Child = childObj;
    parObj.ChildRels.Add(visRelation);
    childObj.ParentRels.Add(visRelation);
    return visRelation;
  }

  private void FindObjectInScheme(object sender, EventArgs e)
  {
    using (FindDialog findDialog = new FindDialog((MapView) this.map))
    {
      int num = (int) findDialog.ShowDialog();
    }
  }

  private void HistoryPressed(object sender, EventArgs e)
  {
    if (!(sender is MenuButtonItem menuButtonItem) || menuButtonItem.Tag == null || !(menuButtonItem.Tag is SchemeInfo))
      return;
    this.ReturnToScheme((SchemeInfo) menuButtonItem.Tag);
  }

  private void ReturnToScheme(SchemeInfo selectedScheme)
  {
    this.ChangeLayoutAlgorithm(selectedScheme.SchemeLayoutKind);
    this.SetCurrentObject(selectedScheme);
  }

  private void ShowRelationProps(VisLink relation, bool needOpenPanel)
  {
    if (needOpenPanel)
      this.collapsibleSplitter.ControlToHide.Show();
    else if (!this.collapsibleSplitter.ControlToHide.Visible)
      return;
    this.propertyGridPanel.LoadNode((MapObject) relation);
  }

  private void ShowObjectProps(VisNode obj, bool needOpenPanel)
  {
    if (needOpenPanel)
      this.collapsibleSplitter.ControlToHide.Show();
    else if (!this.collapsibleSplitter.ControlToHide.Visible)
      return;
    this.propertyGridPanel.LoadNode((MapObject) obj);
  }

  private void bStructureLinks_Click(object sender, EventArgs e)
  {
    this.ShowStructLinks = !this.ShowStructLinks;
    this.UpdateControls();
    int num = (int) this.MakeUpdateDecision();
  }

  private void bAssociateLinks_Click(object sender, EventArgs e)
  {
    this.ShowAssociativeLinks = !this.ShowAssociativeLinks;
    this.UpdateControls();
    int num = (int) this.MakeUpdateDecision();
  }

  private void toolStripMenuItem_RelProps_Click(object sender, EventArgs e)
  {
    if (this.currentSelectedRelation == null || this.currentSelectedRelation.RelId == 0L)
      return;
    this.ShowRelationProps(this.currentSelectedRelation, true);
  }

  private void toolStripMenuItem_delete_Click(object sender, EventArgs e) => this.DeleteRelation();

  private void toolStripStatus_Stop_Click(object sender, EventArgs e)
  {
    if (MessageBox.Show(LocalizationHolder.rm.GetString("Pdm_rv_15"), LocalizationHolder.rm.GetString("Pdm_rv_16"), MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.Yes)
      this.StopThread();
  }

  private void BarCommand_Refresh() => this.BuildThread(VisView.BuildFlags.UpdateTree);

  private bool BarCommandCheckStatus_baseComands(string command)
  {
    return this.currentSelectedObjects != null && this.QueryStatusCurrentContext(command);
  }

  private bool QueryStatusCurrentContext(string CommandName)
  {
    return !(CommandName == string.Empty) && this.currentSelectedObjects != null && Intermech.Navigator.ContextMenu.Services.GetCommandsTable(this.currentSelectedObjects, (IServiceProvider) this._services, false).Contains(CommandName);
  }

  private void toolStripMenuItem_LA_Normal_Click(object sender, EventArgs e)
  {
    if (!this.mbiLayoutNormal.Checked)
      return;
    this.visLayout = (IVisLayout) this.STANDARD_LAYOUT;
    this.mbiLayoutKulon.Checked = false;
    this.mbiLayoutHierarch.Checked = false;
    this.mbiLayoutVertNorm.Checked = false;
    this.mbiLayoutVertHier.Checked = false;
    this.mbiLayoutSupreme.Checked = false;
    this.setCenterNode = true;
    this.BuildThread(VisView.BuildFlags.ReLayout);
  }

  private void toolStripMenuItem_LA_Kulon_Click(object sender, EventArgs e)
  {
    if (!this.mbiLayoutKulon.Checked)
      return;
    this.mbiLayoutNormal.Checked = false;
    this.mbiLayoutHierarch.Checked = false;
    this.mbiLayoutVertNorm.Checked = false;
    this.mbiLayoutVertHier.Checked = false;
    this.mbiLayoutSupreme.Checked = false;
    this.setCenterNode = true;
    this.BuildThread(VisView.BuildFlags.ReLayout);
  }

  private void toolStripMenuItem_LA_Hier_Click(object sender, EventArgs e)
  {
    if (!this.mbiLayoutHierarch.Checked)
      return;
    this.visLayout = (IVisLayout) this.HIER_LAYOUT;
    this.mbiLayoutNormal.Checked = false;
    this.mbiLayoutKulon.Checked = false;
    this.mbiLayoutVertNorm.Checked = false;
    this.mbiLayoutVertHier.Checked = false;
    this.mbiLayoutSupreme.Checked = false;
    this.setCenterNode = true;
    this.BuildThread(VisView.BuildFlags.ReLayout);
  }

  private void mbiLayoutVertNorm_Click(object sender, EventArgs e)
  {
    if (!this.mbiLayoutVertNorm.Checked)
      return;
    this.visLayout = (IVisLayout) this.VERT_STANDARD_LAYOUT;
    this.mbiLayoutKulon.Checked = false;
    this.mbiLayoutHierarch.Checked = false;
    this.mbiLayoutNormal.Checked = false;
    this.mbiLayoutVertHier.Checked = false;
    this.mbiLayoutSupreme.Checked = false;
    this.setCenterNode = true;
    this.BuildThread(VisView.BuildFlags.ReLayout);
  }

  private void mbiLayoutVertHier_Click(object sender, EventArgs e)
  {
    if (!this.mbiLayoutVertHier.Checked)
      return;
    this.visLayout = (IVisLayout) this.VERT_HIER_LAYOUT;
    this.mbiLayoutKulon.Checked = false;
    this.mbiLayoutHierarch.Checked = false;
    this.mbiLayoutVertNorm.Checked = false;
    this.mbiLayoutNormal.Checked = false;
    this.mbiLayoutSupreme.Checked = false;
    this.setCenterNode = true;
    this.BuildThread(VisView.BuildFlags.ReLayout);
  }

  private void mbiLayoutSupreme_Click(object sender, EventArgs e)
  {
    if (!this.mbiLayoutSupreme.Checked)
      return;
    this.visLayout = (IVisLayout) this.QUALITY_LAYOUT;
    this.mbiLayoutKulon.Checked = false;
    this.mbiLayoutHierarch.Checked = false;
    this.mbiLayoutVertNorm.Checked = false;
    this.mbiLayoutNormal.Checked = false;
    this.mbiLayoutVertHier.Checked = false;
    this.setCenterNode = true;
    this.BuildThread(VisView.BuildFlags.ReLayout);
  }

  private void dropDownMenu_SelectLO_Alg_Click(object sender, EventArgs e)
  {
    if (this.visLayout != this.STANDARD_LAYOUT)
    {
      this.mbiLayoutNormal.Checked = true;
      this.toolStripMenuItem_LA_Normal_Click((object) null, (EventArgs) null);
    }
    else
    {
      this.mbiLayoutHierarch.Checked = true;
      this.toolStripMenuItem_LA_Hier_Click((object) null, (EventArgs) null);
    }
  }

  private void dropDownMenu_FromHistory_Click(object sender, EventArgs e)
  {
    MenuItemBase menuItemBase1 = (MenuItemBase) null;
    foreach (MenuItemBase menuItemBase2 in (CollectionBase) this.dropDownMenu_FromHistory.Items)
    {
      if (!menuItemBase2.Checked && menuItemBase2.Tag != null)
      {
        menuItemBase1 = menuItemBase2;
        break;
      }
    }
    if (menuItemBase1 == null)
      return;
    this.SetCurrentObject((SchemeInfo) menuItemBase1.Tag);
  }

  public override string HelpID => string.Empty;

  public static string ChooseStyles(ref long schemeId)
  {
    int objectTypeId = MetaDataHelper.GetObjectTypeID(new Guid(VisView.visStylesGuid));
    if (objectTypeId == -1)
    {
      int num = (int) MessageBox.Show("Error!");
    }
    long[] numArray = Intermech.Navigator.SelectionWindow.SelectObjects(LocalizationHolder.rm.GetString("Pdm_rv_47"), LocalizationHolder.rm.GetString("Pdm_rv_48"), objectTypeId, SelectionOptions.SelectObjects | SelectionOptions.DisableMultiselect);
    if (numArray == null || numArray.Length == 0)
      return string.Empty;
    schemeId = numArray[0];
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(schemeId);
      return objectInfo.Empty ? string.Empty : objectInfo.Caption;
    }
  }

  public StyleData Style { get; set; }

  private void btnLoadStyle_Click(object sender, EventArgs e)
  {
    string str = VisView.ChooseStyles(ref PDMPlugin._visStylesId);
    if (str.Equals(string.Empty))
      return;
    PDMPlugin.VisStylesName = str;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this.LoadStyles(sessionKeeper.Session, PDMPlugin._visStylesId);
  }

  public void LoadStyles(IUserSession ius, long objId)
  {
    VisStyle visStyle = this._DoLoadStyle(ius, objId);
    if (visStyle != null)
    {
      PDMPlugin.VisStylesId = objId;
      PDMPlugin.VisStylesName = visStyle.Name;
    }
    MapDocument newDocument = this.CreateNewDocument();
    newDocument.UserObject = (object) this.settings;
    this.curScheme.StyleData = this.Style;
    this.curScheme.UpdateStyle();
    new SchemCreator(this.curScheme, this.map, newDocument, this.previewMode).CreateFullScheme();
    this.map.Document = newDocument;
    MapLayer mapLayer = newDocument.Layers.Find((object) 0);
    if (mapLayer == null)
      return;
    this.map.ScrollToControl(mapLayer.FirstObject);
  }

  public VisStyle _DoLoadStyle(IUserSession ius, long objId)
  {
    VisStyle visStyle = new VisStyle();
    if (!visStyle.LoadFromObject(ius, objId) && objId < 0L && !visStyle.LoadFromObject(ius, -objId))
    {
      this.SetDefaultStyle();
      return (VisStyle) null;
    }
    this.Style = new StyleData();
    foreach (VisStyleNode styleNode in visStyle.StyleNodes)
      this.Style.AddStyleNode(styleNode);
    return visStyle;
  }

  public void SetDefaultStyle() => this.Style = this.defStyle;

  public StyleData CreateDefaultStyle() => new StyleData();

  private void visPreviewNone_Click(object sender, EventArgs e)
  {
    PreviewMode int32 = (PreviewMode) Convert.ToInt32((sender as MenuButtonItem).Tag);
    if (this.previewMode == int32)
      return;
    this.SetPreviewMode(int32);
  }

  private void ddMenu_Preview_Click(object sender, EventArgs e)
  {
    this.SetPreviewMode((PreviewMode) ((int) (this.previewMode + 1) % 3));
  }

  private void SetPreviewMode(PreviewMode newMode)
  {
    this.previewMode = newMode;
    this.UpdatePreviewMenu(newMode);
    if (newMode > (PreviewMode) this.curScheme.MaxPreviewMode)
    {
      this.previewWorker.RunWorkerAsync((object) this.previewWorker);
    }
    else
    {
      MapDocument newDocument = this.CreateNewDocument();
      newDocument.UserObject = (object) this.settings;
      SchemCreator schemCreator = new SchemCreator(this.curScheme, this.map, newDocument, this.previewMode);
      this.curScheme.UpdatePreviewMode((int) newMode);
      schemCreator.CreateFullScheme();
      this.map.Document = newDocument;
    }
    PDMPlugin.PreviewMode = (long) newMode;
  }

  private void UpdatePreviewMenu(PreviewMode newMode)
  {
    switch (newMode)
    {
      case PreviewMode.NoPreview:
        this.visPreviewNone.Checked = true;
        this.visPreviewFromScheme.Checked = false;
        this.visPreviewAll.Checked = false;
        this.ddMenu_Preview.Image = (Image) Intermech.Pdm.Properties.Resources.Vis_PreviewNone;
        this.ddMenu_Preview.ToolTipText = VisView.Preview_None;
        break;
      case PreviewMode.SelPreview:
        this.visPreviewNone.Checked = false;
        this.visPreviewFromScheme.Checked = true;
        this.visPreviewAll.Checked = false;
        this.ddMenu_Preview.Image = (Image) Intermech.Pdm.Properties.Resources.Vis_PreviewSelected;
        this.ddMenu_Preview.ToolTipText = VisView.Preview_Sel;
        break;
      case PreviewMode.FullPreview:
        this.visPreviewNone.Checked = false;
        this.visPreviewFromScheme.Checked = false;
        this.visPreviewAll.Checked = true;
        this.ddMenu_Preview.Image = (Image) Intermech.Pdm.Properties.Resources.Vis_PreviewAll;
        this.ddMenu_Preview.ToolTipText = VisView.Preview_All;
        break;
    }
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing)
    {
      if (ServicesManager.GetService(typeof (BarManager)) is BarManager service)
      {
        this.toolBar.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
        service.RendererChanged -= new EventHandler(this.barRender_RendererChanged);
      }
      if (this.components != null)
        this.components.Dispose();
    }
    this.propertyGridPanel.PropertyGrid.SelectedObject = (object) null;
    this.currentLayoutAlgorithm = (ILayoutAlgorithm) null;
    this.currentSelectedObjects = (ISelectedItems) null;
    this.currentSelectedRelation = (VisLink) null;
    this.curSchemeInfo = SchemeInfo.Empty;
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (VisView));
    this.statusBar = new StatusStrip();
    this.toolStripStatus_Stop = new ToolStripStatusLabel();
    this.toolStripStatus = new ToolStripStatusLabel();
    this.contextMenuStrip_map = new ContextMenuStrip(this.components);
    this.toolStripMenuItem_props = new ToolStripMenuItem();
    this.toolStripMenuItem_delete = new ToolStripMenuItem();
    this.collapsibleSplitter = new CollapsibleSplitter();
    this.propertyGridPanel = new PropPanel();
    this.toolBar = new Intermech.Bars.ToolBar();
    this.buttonItem_ZoomIn = new ButtonItem();
    this.buttonItem_ZoomOut = new ButtonItem();
    this.buttonItem_ZoomOnce = new ButtonItem();
    this.buttonItem_AllShema = new ButtonItem();
    this.buttonItem_WidthIn = new ButtonItem();
    this.buttonItem_WidthOut = new ButtonItem();
    this.buttonItem_HeigthIn = new ButtonItem();
    this.buttonItem_HeigthOut = new ButtonItem();
    this.buttonItem_IfNeedParentTree = new ButtonItem();
    this.buttonItem_IfNeedChildTree = new ButtonItem();
    this.buttonItem_FindObject = new ButtonItem();
    this.dropDownMenu_FromHistory = new DropDownMenuItem();
    this.menuSelectLayout = new DropDownMenuItem();
    this.mbiLayoutNormal = new MenuButtonItem();
    this.mbiLayoutKulon = new MenuButtonItem();
    this.mbiLayoutHierarch = new MenuButtonItem();
    this.mbiLayoutVertNorm = new MenuButtonItem();
    this.mbiLayoutVertHier = new MenuButtonItem();
    this.buttonItem_LevelP = new ButtonItem();
    this.buttonItem_Status = new ButtonItem();
    this.bStructureLinks = new ButtonItem();
    this.bAssociateLinks = new ButtonItem();
    this.btnLoadStyle = new ButtonItem();
    this.ddMenu_Preview = new DropDownMenuItem();
    this.visPreviewNone = new MenuButtonItem();
    this.visPreviewFromScheme = new MenuButtonItem();
    this.visPreviewAll = new MenuButtonItem();
    this.map = new VisControl(this.components);
    this.mbiLayoutSupreme = new MenuButtonItem();
    this.statusBar.SuspendLayout();
    this.contextMenuStrip_map.SuspendLayout();
    this.SuspendLayout();
    this.statusBar.ImageScalingSize = new Size(24, 24);
    this.statusBar.Items.AddRange(new ToolStripItem[2]
    {
      (ToolStripItem) this.toolStripStatus_Stop,
      (ToolStripItem) this.toolStripStatus
    });
    componentResourceManager.ApplyResources((object) this.statusBar, "statusBar");
    this.statusBar.Name = "statusBar";
    this.toolStripStatus_Stop.AutoToolTip = true;
    this.toolStripStatus_Stop.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.toolStripStatus_Stop.Image = (Image) Intermech.Pdm.Properties.Resources.About;
    this.toolStripStatus_Stop.Name = "toolStripStatus_Stop";
    componentResourceManager.ApplyResources((object) this.toolStripStatus_Stop, "toolStripStatus_Stop");
    this.toolStripStatus_Stop.Click += new EventHandler(this.toolStripStatus_Stop_Click);
    this.toolStripStatus.Name = "toolStripStatus";
    componentResourceManager.ApplyResources((object) this.toolStripStatus, "toolStripStatus");
    this.contextMenuStrip_map.ImageScalingSize = new Size(24, 24);
    this.contextMenuStrip_map.Items.AddRange(new ToolStripItem[2]
    {
      (ToolStripItem) this.toolStripMenuItem_props,
      (ToolStripItem) this.toolStripMenuItem_delete
    });
    this.contextMenuStrip_map.Name = "contextMenuStrip1";
    componentResourceManager.ApplyResources((object) this.contextMenuStrip_map, "contextMenuStrip_map");
    this.toolStripMenuItem_props.Name = "toolStripMenuItem_props";
    componentResourceManager.ApplyResources((object) this.toolStripMenuItem_props, "toolStripMenuItem_props");
    this.toolStripMenuItem_props.Click += new EventHandler(this.toolStripMenuItem_RelProps_Click);
    this.toolStripMenuItem_delete.Name = "toolStripMenuItem_delete";
    componentResourceManager.ApplyResources((object) this.toolStripMenuItem_delete, "toolStripMenuItem_delete");
    this.toolStripMenuItem_delete.Click += new EventHandler(this.toolStripMenuItem_delete_Click);
    this.collapsibleSplitter.AnimationDelay = 20;
    this.collapsibleSplitter.AnimationStep = 20;
    this.collapsibleSplitter.BorderStyle3D = Border3DStyle.Flat;
    this.collapsibleSplitter.ControlToHide = (Control) this.propertyGridPanel;
    this.collapsibleSplitter.ExpandParentForm = true;
    componentResourceManager.ApplyResources((object) this.collapsibleSplitter, "collapsibleSplitter");
    this.collapsibleSplitter.Name = "collapsibleSplitter1";
    this.collapsibleSplitter.TabStop = false;
    this.collapsibleSplitter.UseAnimations = false;
    this.collapsibleSplitter.VisualStyle = VisualStyles.Mozilla;
    componentResourceManager.ApplyResources((object) this.propertyGridPanel, "propertyGridPanel");
    this.propertyGridPanel.IsModified = false;
    this.propertyGridPanel.IsRelation = false;
    this.propertyGridPanel.Name = "propertyGridPanel";
    componentResourceManager.ApplyResources((object) this.toolBar, "toolBar");
    this.toolBar.Flow = ToolBarLayout.Vertical;
    this.toolBar.FullMenus = true;
    this.toolBar.Guid = new Guid("feffc6ee-eb37-47cd-ae3d-9e2f2c8a3e3a");
    this.toolBar.Hidden = false;
    this.toolBar.Items.AddRange(new ToolbarItemBase[19]
    {
      (ToolbarItemBase) this.buttonItem_ZoomIn,
      (ToolbarItemBase) this.buttonItem_ZoomOut,
      (ToolbarItemBase) this.buttonItem_ZoomOnce,
      (ToolbarItemBase) this.buttonItem_AllShema,
      (ToolbarItemBase) this.buttonItem_WidthIn,
      (ToolbarItemBase) this.buttonItem_WidthOut,
      (ToolbarItemBase) this.buttonItem_HeigthIn,
      (ToolbarItemBase) this.buttonItem_HeigthOut,
      (ToolbarItemBase) this.buttonItem_IfNeedParentTree,
      (ToolbarItemBase) this.buttonItem_IfNeedChildTree,
      (ToolbarItemBase) this.buttonItem_FindObject,
      (ToolbarItemBase) this.dropDownMenu_FromHistory,
      (ToolbarItemBase) this.menuSelectLayout,
      (ToolbarItemBase) this.buttonItem_LevelP,
      (ToolbarItemBase) this.buttonItem_Status,
      (ToolbarItemBase) this.bStructureLinks,
      (ToolbarItemBase) this.bAssociateLinks,
      (ToolbarItemBase) this.btnLoadStyle,
      (ToolbarItemBase) this.ddMenu_Preview
    });
    this.toolBar.Name = "toolBar";
    componentResourceManager.ApplyResources((object) this.buttonItem_ZoomIn, "buttonItem_ZoomIn");
    this.buttonItem_ZoomIn.Image = (Image) Intermech.Pdm.Properties.Resources.Vis_ZoomIn;
    this.buttonItem_ZoomIn.Click += new EventHandler(this.toolStripButton_ZoomIn_Click);
    componentResourceManager.ApplyResources((object) this.buttonItem_ZoomOut, "buttonItem_ZoomOut");
    this.buttonItem_ZoomOut.Image = (Image) Intermech.Pdm.Properties.Resources.Vis_ZoomOut;
    this.buttonItem_ZoomOut.Click += new EventHandler(this.toolStripButton_ZoomOut_Click);
    componentResourceManager.ApplyResources((object) this.buttonItem_ZoomOnce, "buttonItem_ZoomOnce");
    this.buttonItem_ZoomOnce.Image = (Image) Intermech.Pdm.Properties.Resources.Vis_1to1;
    this.buttonItem_ZoomOnce.Click += new EventHandler(this.toolStripButton_ZoomOnce_Click);
    componentResourceManager.ApplyResources((object) this.buttonItem_AllShema, "buttonItem_AllShema");
    this.buttonItem_AllShema.Image = (Image) Intermech.Pdm.Properties.Resources.Vis_ShowAll;
    this.buttonItem_AllShema.Click += new EventHandler(this.ToFitCurDocument);
    componentResourceManager.ApplyResources((object) this.buttonItem_WidthIn, "buttonItem_WidthIn");
    this.buttonItem_WidthIn.Image = (Image) Intermech.Pdm.Properties.Resources.Vis_MinusWidth;
    this.buttonItem_WidthIn.ToolTipText = this.buttonItem_WidthIn.Text;
    this.buttonItem_WidthIn.Click += new EventHandler(this.toolStripButton_WidthHeigth_Click);
    componentResourceManager.ApplyResources((object) this.buttonItem_WidthOut, "buttonItem_WidthOut");
    this.buttonItem_WidthOut.Image = (Image) Intermech.Pdm.Properties.Resources.Vis_PlusWidth;
    this.buttonItem_WidthOut.ToolTipText = this.buttonItem_WidthOut.Text;
    this.buttonItem_WidthOut.Click += new EventHandler(this.toolStripButton_WidthHeigth_Click);
    componentResourceManager.ApplyResources((object) this.buttonItem_HeigthIn, "buttonItem_HeigthIn");
    this.buttonItem_HeigthIn.Image = (Image) Intermech.Pdm.Properties.Resources.Vis_MinusHeight;
    this.buttonItem_HeigthIn.ToolTipText = this.buttonItem_HeigthIn.Text;
    this.buttonItem_HeigthIn.Click += new EventHandler(this.toolStripButton_WidthHeigth_Click);
    componentResourceManager.ApplyResources((object) this.buttonItem_HeigthOut, "buttonItem_HeigthOut");
    this.buttonItem_HeigthOut.Image = (Image) Intermech.Pdm.Properties.Resources.Vis_PlusHeight;
    this.buttonItem_HeigthOut.ToolTipText = this.buttonItem_HeigthOut.Text;
    this.buttonItem_HeigthOut.Click += new EventHandler(this.toolStripButton_WidthHeigth_Click);
    componentResourceManager.ApplyResources((object) this.buttonItem_IfNeedParentTree, "buttonItem_IfNeedParentTree");
    this.buttonItem_IfNeedParentTree.Image = (Image) Intermech.Pdm.Properties.Resources.Vis_Usability;
    this.buttonItem_IfNeedParentTree.Click += new EventHandler(this.toolStripButton_ifNeedParentTree_Click);
    this.buttonItem_IfNeedChildTree.Checked = true;
    componentResourceManager.ApplyResources((object) this.buttonItem_IfNeedChildTree, "buttonItem_IfNeedChildTree");
    this.buttonItem_IfNeedChildTree.Image = (Image) Intermech.Pdm.Properties.Resources.Vis_Contents;
    this.buttonItem_IfNeedChildTree.Click += new EventHandler(this.toolStripButton_ifNeedChildTree_Click);
    componentResourceManager.ApplyResources((object) this.buttonItem_FindObject, "buttonItem_FindObject");
    this.buttonItem_FindObject.Click += new EventHandler(this.FindObjectInScheme);
    componentResourceManager.ApplyResources((object) this.dropDownMenu_FromHistory, "dropDownMenu_FromHistory");
    this.dropDownMenu_FromHistory.Image = (Image) Intermech.Pdm.Properties.Resources.Vis_History;
    this.dropDownMenu_FromHistory.ShowText = true;
    this.dropDownMenu_FromHistory.Click += new EventHandler(this.dropDownMenu_FromHistory_Click);
    componentResourceManager.ApplyResources((object) this.menuSelectLayout, "menuSelectLayout");
    this.menuSelectLayout.Image = (Image) Intermech.Pdm.Properties.Resources.Vis_Mode;
    this.menuSelectLayout.Items.AddRange(new ToolbarItemBase[6]
    {
      (ToolbarItemBase) this.mbiLayoutNormal,
      (ToolbarItemBase) this.mbiLayoutKulon,
      (ToolbarItemBase) this.mbiLayoutHierarch,
      (ToolbarItemBase) this.mbiLayoutVertNorm,
      (ToolbarItemBase) this.mbiLayoutVertHier,
      (ToolbarItemBase) this.mbiLayoutSupreme
    });
    this.menuSelectLayout.ShowText = true;
    this.menuSelectLayout.Click += new EventHandler(this.dropDownMenu_SelectLO_Alg_Click);
    this.mbiLayoutNormal.AutoToggle = AutoToggleType.Single;
    this.mbiLayoutNormal.Checked = true;
    componentResourceManager.ApplyResources((object) this.mbiLayoutNormal, "mbiLayoutNormal");
    this.mbiLayoutNormal.ShowText = true;
    this.mbiLayoutNormal.Click += new EventHandler(this.toolStripMenuItem_LA_Normal_Click);
    this.mbiLayoutKulon.AutoToggle = AutoToggleType.Single;
    componentResourceManager.ApplyResources((object) this.mbiLayoutKulon, "mbiLayoutKulon");
    this.mbiLayoutKulon.ShowText = true;
    this.mbiLayoutKulon.Click += new EventHandler(this.toolStripMenuItem_LA_Kulon_Click);
    this.mbiLayoutHierarch.AutoToggle = AutoToggleType.Single;
    componentResourceManager.ApplyResources((object) this.mbiLayoutHierarch, "mbiLayoutHierarch");
    this.mbiLayoutHierarch.ShowText = true;
    this.mbiLayoutHierarch.Click += new EventHandler(this.toolStripMenuItem_LA_Hier_Click);
    this.mbiLayoutVertNorm.AutoToggle = AutoToggleType.Single;
    componentResourceManager.ApplyResources((object) this.mbiLayoutVertNorm, "mbiLayoutVertNorm");
    this.mbiLayoutVertNorm.ShowText = true;
    this.mbiLayoutVertNorm.Click += new EventHandler(this.mbiLayoutVertNorm_Click);
    this.mbiLayoutVertHier.AutoToggle = AutoToggleType.Single;
    componentResourceManager.ApplyResources((object) this.mbiLayoutVertHier, "mbiLayoutVertHier");
    this.mbiLayoutVertHier.ShowText = true;
    this.mbiLayoutVertHier.Click += new EventHandler(this.mbiLayoutVertHier_Click);
    this.buttonItem_LevelP.AutoToggle = AutoToggleType.Single;
    componentResourceManager.ApplyResources((object) this.buttonItem_LevelP, "buttonItem_LevelP");
    this.buttonItem_LevelP.Image = (Image) Intermech.Pdm.Properties.Resources.Vis_LCLevels;
    this.buttonItem_LevelP.Click += new EventHandler(this.toolStripButton_LevelP_Click);
    this.buttonItem_Status.AutoToggle = AutoToggleType.Single;
    componentResourceManager.ApplyResources((object) this.buttonItem_Status, "buttonItem_Status");
    this.buttonItem_Status.Click += new EventHandler(this.toolStripButton_Status_Click);
    this.bStructureLinks.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.bStructureLinks, "bStructureLinks");
    this.bStructureLinks.Image = (Image) Intermech.Pdm.Properties.Resources.Vis_Struct;
    this.bStructureLinks.Click += new EventHandler(this.bStructureLinks_Click);
    componentResourceManager.ApplyResources((object) this.bAssociateLinks, "bAssociateLinks");
    this.bAssociateLinks.Image = (Image) Intermech.Pdm.Properties.Resources.Vis_Assoc;
    this.bAssociateLinks.Click += new EventHandler(this.bAssociateLinks_Click);
    this.btnLoadStyle.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.btnLoadStyle, "btnLoadStyle");
    this.btnLoadStyle.Image = (Image) Intermech.Pdm.Properties.Resources.Vis_Styles;
    this.btnLoadStyle.Click += new EventHandler(this.btnLoadStyle_Click);
    componentResourceManager.ApplyResources((object) this.ddMenu_Preview, "ddMenu_Preview");
    this.ddMenu_Preview.Image = (Image) Intermech.Pdm.Properties.Resources.Vis_PreviewSelected;
    this.ddMenu_Preview.Items.AddRange(new ToolbarItemBase[3]
    {
      (ToolbarItemBase) this.visPreviewNone,
      (ToolbarItemBase) this.visPreviewFromScheme,
      (ToolbarItemBase) this.visPreviewAll
    });
    this.ddMenu_Preview.ShowText = true;
    this.ddMenu_Preview.Click += new EventHandler(this.ddMenu_Preview_Click);
    componentResourceManager.ApplyResources((object) this.visPreviewNone, "visPreviewNone");
    this.visPreviewNone.ShowText = true;
    this.visPreviewNone.Tag = (object) "0";
    this.visPreviewNone.Click += new EventHandler(this.visPreviewNone_Click);
    this.visPreviewFromScheme.Checked = true;
    componentResourceManager.ApplyResources((object) this.visPreviewFromScheme, "visPreviewFromScheme");
    this.visPreviewFromScheme.ShowText = true;
    this.visPreviewFromScheme.Tag = (object) "1";
    this.visPreviewFromScheme.Click += new EventHandler(this.visPreviewNone_Click);
    componentResourceManager.ApplyResources((object) this.visPreviewAll, "visPreviewAll");
    this.visPreviewAll.ShowText = true;
    this.visPreviewAll.Tag = (object) "2";
    this.visPreviewAll.Click += new EventHandler(this.visPreviewNone_Click);
    this.map.AllowDrop = true;
    this.map.BackColor = Color.White;
    componentResourceManager.ApplyResources((object) this.map, "map");
    this.map.MaximumSelectionCount = 1;
    this.map.Name = "map";
    this.mbiLayoutSupreme.AutoToggle = AutoToggleType.Single;
    componentResourceManager.ApplyResources((object) this.mbiLayoutSupreme, "mbiLayoutSupreme");
    this.mbiLayoutSupreme.ShowText = true;
    this.mbiLayoutSupreme.Click += new EventHandler(this.mbiLayoutSupreme_Click);
    this.AutoScaleMode = AutoScaleMode.Inherit;
    this.BackColor = SystemColors.Control;
    this.Controls.Add((Control) this.map);
    this.Controls.Add((Control) this.toolBar);
    this.Controls.Add((Control) this.collapsibleSplitter);
    this.Controls.Add((Control) this.propertyGridPanel);
    this.Controls.Add((Control) this.statusBar);
    this.Name = nameof (VisView);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.statusBar.ResumeLayout(false);
    this.statusBar.PerformLayout();
    this.contextMenuStrip_map.ResumeLayout(false);
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  [Serializable]
  public enum BuildFlags
  {
    CreateTree,
    UpdateTree,
    ReLayout,
  }

  public delegate void ThreadFinishEventHandler(MapDocument result, Exception exception);

  public delegate void ThreadBuildSchemaUpdateInfo(string text, float percent);
}
