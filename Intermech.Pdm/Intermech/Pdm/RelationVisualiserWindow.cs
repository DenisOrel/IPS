// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualiserWindow
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Bars;
using Intermech.Client.Core;
using Intermech.DataFormats;
using Intermech.Docking;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.Configuration;
using Intermech.Interfaces.Pdm;
using Intermech.Interfaces.Pdm.RelationVisualizer;
using Intermech.Localization;
using Intermech.Map;
using Intermech.Map.Layout;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Pdm.RelationVisualizer;
using NJFLib.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace Intermech.Pdm;

public class RelationVisualiserWindow : DockControl, IFiltrationClass, ICommandTarget
{
  private static readonly Guid _persistStateGuid = new Guid("{DF4C6013-2B74-41a4-A489-C6E827468FFF}");
  public const string RelVisWindowName = "desktopRelationVisualiserWindow";
  public static readonly string RelVisName = LocalizationHolder.rm.GetString("Pdm_rv_1");
  public static bool ShowStructLinks = true;
  public static bool ShowAssociativeLinks = true;
  public bool ShowLinkButtons;
  private WinSettings settings = new WinSettings();
  private UserSettings userSettings;
  private SchemaList SchemaList;
  protected bool _activated;
  protected bool _loaded;
  protected Intermech.Client.Core.NotificationService _notificationService;
  private ICommandManager _cmdMngr;
  private INamedImageList _images;
  private ICategoryTypeIconService _categoryObjTypeImages;
  protected AdvancedServiceContainer _services;
  private DBEventList dbEvents = new DBEventList();
  private IElementStatusesClientService _elementStatusesClientService;
  private ICurrentUserAndRole _CurrentUserAndRule;
  private IRelVisObserverService observerService;
  private ICompositionLoadService compositionLoadService;
  private RelMapLink currentSelectedRelation;
  private ISelectedItems currentSelectedObjects;
  private ILayoutAlgorithm currentLayoutAlgorithm = (ILayoutAlgorithm) new NormalLayout();
  private Thread backgrThread;
  private SchemaInfo currentSchema;
  private SchemaInfo defaultSchema;
  private MapDocument backDocument;
  private int attrCountTypeID = -1;
  private IFiltrationService _filtrationService;
  [NonSerialized]
  private IFiltrationClass _FiltrationClass;
  [NonSerialized]
  private string _FiltrationOwnerID = string.Empty;
  private bool isThreadRun;
  private MeasuresConvertor measureConvertor;
  private bool closed;
  private List<VisObjectNode> objsToDelete = new List<VisObjectNode>();
  private bool lockUpdateStatus;
  private bool setCenterNode;
  private IContainer components;
  private StatusStrip statusBar;
  private RelVisualItemsPropPanel propertyGridPanel;
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
  private RelViewControl map;
  private ButtonItem buttonItem_ZoomOnce;
  private ButtonItem buttonItem_AllShema;
  private ButtonItem buttonItem_IfNeedParentTree;
  private ButtonItem buttonItem_IfNeedChildTree;
  private ButtonItem buttonItem_FindObject;
  private DropDownMenuItem dropDownMenu_SelectShema;
  private DropDownMenuItem dropDownMenu_SelectLO_Alg;
  private ButtonItem buttonItem_LevelP;
  private ButtonItem buttonItem_Status;
  private MenuButtonItem menuButtonItem_LA_Normal;
  private MenuButtonItem menuButtonItem_LA_Kulon;
  private MenuButtonItem menuButtonItem_LA_Hier;
  private ButtonItem bStructureLinks;
  private ButtonItem bAssociateLinks;

  public RelationVisualiserWindow()
  {
    this.InitializeComponent();
    this.menuButtonItem_LA_Kulon.Visible = false;
    this.Guid = RelationVisualiserWindow._persistStateGuid;
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
        this.UpdateCurrentSchema(DBEvent.DBEventType.All);
        return num != 0;
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
      case "Cut":
      case "Delete":
      case "Exclude":
      case "ParametersCard":
      case "Paste":
      case "SaveChanges":
        commandState.Enabled = this.BarCommandCheckStatus_baseComands(commandState.CommandName);
        return commandState.Enabled;
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
    this.BuildThread(Observer.BuildFlags.UpdateTree);
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

  public event RelationVisualiserWindow.ThreadFinishEventHandler ThreadFinish;

  public event RelationVisualiserWindow.ThreadBuildShepaUpdaeInfo BuildShemaUpdate;

  public bool IsThreadBusy
  {
    get => this.isThreadRun;
    set
    {
      this.isThreadRun = value;
      this.LockControls(this.isThreadRun);
      this.toolStripStatus_Stop.Visible = value;
    }
  }

  public bool StopThread()
  {
    bool flag = false;
    try
    {
      if (this.isThreadRun)
      {
        this.backgrThread.Abort();
        this.isThreadRun = false;
        this.backgrThread = (Thread) null;
        this.toolStripStatus_Stop.Visible = false;
        flag = true;
      }
    }
    catch (Exception ex)
    {
      ExceptionHelper.ExceptionService.ShowException(new Exception(LocalizationHolder.rm.GetString("Pdm_532") + ex.Message, ex));
    }
    return flag;
  }

  protected void OnThreadFinish(MapDocument result, string message, Exception exception)
  {
    if (this.ThreadFinish == null)
      return;
    this.ThreadFinish(result, (object) message, exception);
  }

  protected void OnBuildStatusUpdate(string text, float percent)
  {
    if (this.lockUpdateStatus || this.BuildShemaUpdate == null)
      return;
    this.BuildShemaUpdate(text, percent);
  }

  private void InitializeWindow()
  {
    this.SchemaList = new SchemaList();
    this.map.NewLinkClass = typeof (RelMapLink);
    this.backDocument = new MapDocument();
    this.map.ObjectContextClicked += new MapObjectEventHandler(this.map_ObjectContextClicked);
    this.map.ClipboardPasted += new EventHandler(this.map_ClipboardPasted);
    this.map.SelectionDeleted += new EventHandler(this.map_SelectionDeleted);
    this.map.OnRelationCreated += new RelViewControl.CreateRelation(this.map_OnRelationCreated);
    this.map.ObjectGotSelection += new MapSelectionEventHandler(this.map_ObjectGotSelection);
    this.map.BackgroundSingleClicked += new MapInputEventHandler(this.map_BackgroundSingleClicked);
    this.Closed += new EventHandler(this.RelationVisualiserWindow_Closed);
    if (this._images != null)
    {
      this.toolBar.ImageList = this._images.ImageList;
      this.buttonItem_AllShema.ImageIndex = this._images.ImageIndex("imgZoomAll");
      this.buttonItem_FindObject.ImageIndex = this._images.ImageIndex("imgFind");
      this.dropDownMenu_SelectShema.ImageIndex = this._images.ImageIndex("imgOpenItem");
      this.contextMenuStrip_map.ImageList = this._images.ImageList;
      this.toolStripMenuItem_delete.ImageIndex = this._images.ImageIndex("imgDelete");
      this.toolStripMenuItem_props.ImageIndex = this._images.ImageIndex("imgProp");
      this.buttonItem_ZoomOnce.ImageIndex = this._images.ImageIndex("imgZoom1to1");
      this.buttonItem_Status.ImageIndex = this._images.ImageIndex("imgContextComposition.PDM");
      this.buttonItem_ZoomOut.ImageIndex = this._images.ImageIndex("imgZoomOut");
      this.buttonItem_ZoomIn.ImageIndex = this._images.ImageIndex("imgZoomIn");
      this.statusBar.ImageList = this._images.ImageList;
      this.toolStripStatus_Stop.ImageIndex = this._images.ImageIndex("imgStop2");
    }
    this.measureConvertor = new MeasuresConvertor();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this.measureConvertor.Init(sessionKeeper.Session.GetMeasuresList());
    this.propertyGridPanel.RelationAttrUpdated += new EventHandler(this.propertyGridPanel_RelationAttrUpdated);
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

  private void ReadMetadata4DB()
  {
    this.attrCountTypeID = MetaDataHelper.GetAttributeTypeID(new Guid("cad00267-306c-11d8-b4e9-00304f19f545"));
  }

  private bool NeedChildTree() => this.buttonItem_IfNeedChildTree.Checked;

  private void NeedChildTree(bool ch) => this.buttonItem_IfNeedChildTree.Checked = ch;

  private bool NeedParentTree() => this.buttonItem_IfNeedParentTree.Checked;

  private void NeedParentTree(bool ch) => this.buttonItem_IfNeedParentTree.Checked = ch;

  protected override void OnClosing(CancelEventArgs e)
  {
    base.OnClosing(e);
    try
    {
      IConfigurationManager service = ServicesManager.ServiceContainer.GetService(typeof (IConfigurationManager)) as IConfigurationManager;
      IConfiguration configuration = service.Open("RelationVisualizerWindow") ?? service.Create("RelationVisualizerWindow");
      configuration.SetProperty("ShowParent", this.buttonItem_IfNeedParentTree.Checked.ToString());
      configuration.SetProperty("ShowChild", this.buttonItem_IfNeedChildTree.Checked.ToString());
      configuration.SetProperty("ShowLevelP", this.buttonItem_LevelP.Checked.ToString());
      configuration.SetProperty("ShowStatus", this.buttonItem_Status.Checked.ToString());
      this.propertyGridPanel.PropertyGrid.SelectedObject = (object) null;
      this.currentLayoutAlgorithm = (ILayoutAlgorithm) null;
      this.currentSelectedObjects = (ISelectedItems) null;
      this.currentSelectedRelation = (RelMapLink) null;
      this.currentSchema = (SchemaInfo) null;
      this.map.Document = new MapDocument();
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
      IConfiguration configuration = (ServicesManager.ServiceContainer.GetService(typeof (IConfigurationManager)) as IConfigurationManager).Open("RelationVisualizerWindow");
      if (configuration == null)
        return;
      string property1 = configuration.GetProperty("ShowParent");
      if (property1 != null && property1 != "")
        this.buttonItem_IfNeedParentTree.Checked = bool.Parse(property1);
      string property2 = configuration.GetProperty("ShowChild");
      if (property2 != null && property2 != "")
        this.buttonItem_IfNeedChildTree.Checked = bool.Parse(property2);
      string property3 = configuration.GetProperty("ShowLevelP");
      if (property3 != null && property3 != "")
      {
        this.buttonItem_LevelP.Checked = bool.Parse(property3);
        this.settings.ShowLifecycleLevel = this.buttonItem_LevelP.Checked;
      }
      string property4 = configuration.GetProperty("ShowStatus");
      if (property4 == null || !(property4 != ""))
        return;
      this.buttonItem_Status.Checked = bool.Parse(property4);
      this.settings.ShowStatuses = this.buttonItem_Status.Checked;
      this.currentSchema.IsLoadStatuses = true;
    }
    catch
    {
    }
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

  private void propertyGridPanel_RelationAttrUpdated(object sender, EventArgs e)
  {
    this.UpdateCurrentSchema(DBEvent.DBEventType.RelationChanged);
  }

  private void UpdateCurrentSchema(DBEvent.DBEventType eType)
  {
    MapDocument document = this.map.Document;
    DateTime dateOflastCheck = this.currentSchema.DateOflastCheck;
    DateTime maxEventTime = this.dbEvents.GetMaxEventTime();
    try
    {
      if (maxEventTime < dateOflastCheck)
        return;
      MapLayerCollectionObjectEnumerator enumerator = document.GetEnumerator();
      double count = 0.0;
      if ((eType == DBEvent.DBEventType.CreateRelation || eType == DBEvent.DBEventType.All) && this.dbEvents.ifContainsRelationCreated(dateOflastCheck))
      {
        this.BuildThread(Observer.BuildFlags.UpdateTree);
      }
      else
      {
        foreach (MapObject mapObject in enumerator)
        {
          if (mapObject is VisObjectNode)
          {
            long objectVerId1 = (mapObject as VisObjectNode).ObjectVerId;
            if ((eType == DBEvent.DBEventType.RemoveObject || eType == DBEvent.DBEventType.All) && this.dbEvents.ifContainsObjectRemoved(objectVerId1, dateOflastCheck))
            {
              foreach (IMapLink link in (mapObject as VisObjectNode).Links)
                document.Remove(link.MapObject);
              document.Remove(mapObject);
            }
            else
            {
              long objectVerId2 = (mapObject as VisObjectNode).ObjectVerId;
              string caption = (string) null;
              if ((eType == DBEvent.DBEventType.ObjVerIdChanged || eType == DBEvent.DBEventType.All) && this.dbEvents.ifContainsObjectVerIdChanged(objectVerId2, dateOflastCheck, out caption))
                (mapObject as VisObjectNode).CheckInOrOut();
            }
          }
          else if (mapObject is RelMapLink)
          {
            RelMapLink relMapLink = mapObject as RelMapLink;
            if ((eType == DBEvent.DBEventType.RemoveRelation || eType == DBEvent.DBEventType.All) && this.dbEvents.ifContainsRelationRemoved(relMapLink.RelId, dateOflastCheck))
              document.Remove(mapObject);
            else if ((eType == DBEvent.DBEventType.RelationChanged || eType == DBEvent.DBEventType.All) && this.dbEvents.ifContainsRelationCountChanged(relMapLink.RelId, dateOflastCheck, out count))
              relMapLink.SetCount(count);
          }
        }
      }
    }
    finally
    {
      this.currentSchema.DateOflastCheck = DateTime.Now;
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
      if (mapObject is VisObjectNode visObjectNode)
      {
        this.objsToDelete.Add(visObjectNode);
        visObjectNode.UseF = flag;
        ++num;
      }
    }
    return enumerator;
  }

  private void DeleteRelObjsAtDocument(MapDocument document, bool flag)
  {
    document.GetEnumerator();
    int num = 0;
    foreach (MapObject mapObject in this.objsToDelete)
    {
      VisObjectNode visObjectNode = mapObject as VisObjectNode;
      if (mapObject.Layer == null || mapObject.Layer.Identifier == null || !mapObject.Layer.Identifier.Equals((object) 0))
      {
        if (visObjectNode != null)
        {
          ++num;
          if (visObjectNode.UseF == flag)
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

  private void BuildExistTree()
  {
    lock (this.currentSchema)
    {
      if (this.currentSchema == null)
        return;
      MapDocument outSchema = (MapDocument) null;
      if (!this.SchemaList.TryGetSchema(this.currentSchema, out outSchema))
        return;
      this.UpdateLayers(outSchema);
      MapLayerCollectionObjectEnumerator enumerator = this.SetDeletableFlag2Document(outSchema, true);
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        long objectVersionId = this.currentSchema.ObjectVersionId;
        long id = this.currentSchema.ID;
        int objectTypeId1 = this.currentSchema.ObjectTypeId;
        if (objectVersionId == 0L)
          return;
        List<VisObjectNode> objectByObjVerId = Observer.GetObjectByObjVerId(enumerator, objectVersionId);
        if (objectByObjVerId.Count == 0)
        {
          this.OnThreadFinish((MapDocument) null, string.Empty, (Exception) null);
        }
        else
        {
          VisObjectNode vobject = objectByObjVerId[0];
          string filtrationOwnerId = this.Get_FiltrationOwnerID();
          this.OnBuildStatusUpdate(LocalizationHolder.rm.GetString("Pdm_rv_10"), 0.0f);
          int objectTypeId2 = this.currentSchema.ObjectTypeId;
          string caption = this.currentSchema.Caption;
          List<long> longList = new List<long>();
          Size size = this.map.Size;
          Statistic statistic = this.currentSchema.Statistic;
          try
          {
            statistic.selectedObjectsCount = 1;
            statistic.isMultiContainsMode = false;
            IElementStatusesClientService statusesClientService = this._elementStatusesClientService;
            ICompositionsAutosortRule rule = (ICompositionsAutosortRule) this._CurrentUserAndRule.Rule;
            Random random = new Random();
            ObjectShape centralShape = new ObjectShape(vobject);
            centralShape.PartID = objectVersionId;
            this.OnBuildStatusUpdate(LocalizationHolder.rm.GetString("Pdm_rv_8"), 5f);
            Observer.BuildChild(objectVersionId, objectTypeId2, this.settings, statistic, sessionKeeper.Session, outSchema, size, centralShape, filtrationOwnerId, Observer.BuildFlags.UpdateTree, this.currentLayoutAlgorithm, statusesClientService, this.observerService, rule);
            this.OnBuildStatusUpdate(LocalizationHolder.rm.GetString("Pdm_rv_7"), 10f);
            Observer.BuildParent(objectVersionId, id, objectTypeId2, this.settings, statistic, sessionKeeper.Session, outSchema, size, centralShape, filtrationOwnerId, Observer.BuildFlags.UpdateTree, this.currentLayoutAlgorithm, statusesClientService, this.observerService, rule);
            this.OnBuildStatusUpdate(LocalizationHolder.rm.GetString("Pdm_rv_6"), 15f);
            this.DeleteRelObjsAtDocument(outSchema, true);
            this.LayoutDocument(outSchema);
          }
          catch (Exception ex)
          {
            this.OnThreadFinish((MapDocument) null, "error", ex);
          }
          finally
          {
            this.OnThreadFinish(outSchema, string.Empty, (Exception) null);
          }
        }
      }
    }
  }

  private MapDocument CreateNewDocument()
  {
    MapDocument newDocument = new MapDocument();
    MapLayer newLayerAfter1 = newDocument.Layers.CreateNewLayerAfter(newDocument.Layers.Bottom);
    newLayerAfter1.Identifier = (object) 2;
    MapLayer newLayerAfter2 = newDocument.Layers.CreateNewLayerAfter(newDocument.Layers.Bottom);
    newLayerAfter2.Identifier = (object) 1;
    newLayerAfter1.AllowView = this.NeedChildTree();
    newLayerAfter2.AllowView = this.NeedParentTree();
    return newDocument;
  }

  private void UpdateLayers(MapDocument document)
  {
    MapLayer mapLayer1 = document.Layers.Find((object) 2);
    MapLayer mapLayer2 = document.Layers.Find((object) 1);
    if (mapLayer1 == null || mapLayer2 == null)
      return;
    mapLayer1.AllowView = this.NeedChildTree();
    mapLayer2.AllowView = this.NeedParentTree();
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

  private void BuildNewTree()
  {
    lock (this.currentSchema)
    {
      if (this.currentSchema == null)
        return;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        long objectVersionId = this.currentSchema.ObjectVersionId;
        long id = this.currentSchema.ID;
        int objectTypeId1 = this.currentSchema.ObjectTypeId;
        IElementStatusesClientService statusesClientService = this._elementStatusesClientService;
        ICompositionsAutosortRule rule = (ICompositionsAutosortRule) this._CurrentUserAndRule.Rule;
        if (objectVersionId == 0L)
          return;
        string filtrationOwnerId = this.Get_FiltrationOwnerID();
        int objectLevel = sessionKeeper.Session.GetObjectLevel(objectVersionId);
        MapDocument newDocument = this.CreateNewDocument();
        this.OnBuildStatusUpdate(LocalizationHolder.rm.GetString("Pdm_rv_9"), 0.0f);
        int objectTypeId2 = this.currentSchema.ObjectTypeId;
        string caption = this.currentSchema.Caption;
        List<long> longList = new List<long>();
        Size size = this.map.Size;
        Statistic statistic = this.currentSchema.Statistic;
        ObjectShape centralShape = (ObjectShape) null;
        try
        {
          statistic.selectedObjectsCount = 0;
          statistic.isMultiContainsMode = false;
          centralShape = Observer.BuildCentralObject(objectVersionId, size, caption, newDocument, objectTypeId2, this.currentLayoutAlgorithm, statusesClientService, this.settings, statistic, objectLevel);
          this.OnBuildStatusUpdate(LocalizationHolder.rm.GetString("Pdm_rv_8"), 5f);
          if (this.userSettings.NeedInvisibleTree || this.NeedChildTree())
            Observer.BuildChild(objectVersionId, objectTypeId2, this.settings, statistic, sessionKeeper.Session, newDocument, size, centralShape, filtrationOwnerId, Observer.BuildFlags.CreateTree, this.currentLayoutAlgorithm, statusesClientService, this.observerService, rule);
          this.OnBuildStatusUpdate(LocalizationHolder.rm.GetString("Pdm_rv_7"), 10f);
          if (this.userSettings.NeedInvisibleTree || this.NeedParentTree())
            Observer.BuildParent(objectVersionId, id, objectTypeId2, this.settings, statistic, sessionKeeper.Session, newDocument, size, centralShape, filtrationOwnerId, Observer.BuildFlags.CreateTree, this.currentLayoutAlgorithm, statusesClientService, this.observerService, rule);
          this.OnBuildStatusUpdate(LocalizationHolder.rm.GetString("Pdm_rv_6"), 15f);
        }
        catch (Exception ex)
        {
          this.OnThreadFinish((MapDocument) null, "error", ex);
        }
        finally
        {
          this.LayoutDocument(newDocument);
          if (centralShape != null)
            this.UpdateCaptions(centralShape.Node);
          this.OnBuildStatusUpdate(LocalizationHolder.rm.GetString("Pdm_rv_5"), 100f);
          this.OnThreadFinish(newDocument, string.Empty, (Exception) null);
        }
      }
    }
  }

  private void UpdateCaptions(VisObjectNode obj)
  {
    if (this.currentLayoutAlgorithm == null || obj == null)
      return;
    string text = obj.Text;
    int distanceBetweenItems = this.currentLayoutAlgorithm.DistanceBetweenItems;
    string shortCaption = ObjectShape.GetShortCaption(text, this.settings, obj.ObjectTypeId, obj.ObjectVerId, distanceBetweenItems);
    obj.Text = shortCaption;
    if (obj.Port == null)
      return;
    foreach (IMapLink sourceLink in obj.SourceLinks)
    {
      if (sourceLink is RelMapLink relMapLink)
      {
        if (relMapLink.ToNode != obj)
          this.UpdateCaptions(relMapLink.ToNode as VisObjectNode);
        if (relMapLink.FromNode != obj)
          this.UpdateCaptions(relMapLink.FromNode as VisObjectNode);
      }
    }
  }

  private void BuildThread(Observer.BuildFlags blf)
  {
    if (this.IsThreadBusy)
      return;
    this.IsThreadBusy = true;
    switch (blf)
    {
      case Observer.BuildFlags.CreateTree:
        this.backgrThread = new Thread(new ThreadStart(this.BuildNewTree));
        this.backgrThread.Name = "RelView_cr_" + this.currentSchema.ToString();
        this.backgrThread.IsBackground = true;
        this.backgrThread.Start();
        if (this.ThreadFinish == null)
          this.ThreadFinish += new RelationVisualiserWindow.ThreadFinishEventHandler(this.RelationVisualiserWindow_ThreadFinish);
        if (this.BuildShemaUpdate != null)
          break;
        this.BuildShemaUpdate += new RelationVisualiserWindow.ThreadBuildShepaUpdaeInfo(this.RelationVisualiserWindow_BuildShemaUpdate);
        break;
      case Observer.BuildFlags.UpdateTree:
        if (!this.SchemaList.ContainsKey(this.currentSchema))
          this.SchemaList.Add(this.currentSchema, this.map.Document);
        this.map.Document = this.backDocument;
        this.backgrThread = new Thread(new ThreadStart(this.BuildExistTree));
        this.backgrThread.Name = "RelView_up_" + this.currentSchema.ToString();
        this.backgrThread.IsBackground = true;
        this.backgrThread.Start();
        if (this.ThreadFinish == null)
          this.ThreadFinish += new RelationVisualiserWindow.ThreadFinishEventHandler(this.RelationVisualiserWindow_ThreadFinish);
        if (this.BuildShemaUpdate != null)
          break;
        this.BuildShemaUpdate += new RelationVisualiserWindow.ThreadBuildShepaUpdaeInfo(this.RelationVisualiserWindow_BuildShemaUpdate);
        break;
    }
  }

  private void SetStatusInfoString(string statusInfo, float precent)
  {
    if (!statusInfo.Equals(string.Empty))
    {
      if ((double) precent >= 100.0)
        this.toolStripStatus.Text = statusInfo;
      else
        this.toolStripStatus.Text = $"{statusInfo}   {precent,1:F}%";
    }
    else
      this.toolStripStatus.Text = string.Empty;
  }

  private void SetFinishStatusTitle()
  {
    if (this.currentSchema == null)
      return;
    long selectedObjectsCount = (long) this.currentSchema.Statistic.selectedObjectsCount;
    this.OnBuildStatusUpdate(this.currentSchema.Caption + LocalizationHolder.rm.GetString("Pdm_rv_25") + (object) selectedObjectsCount, 100f);
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

  private void RelationVisualiserWindow_BuildShemaUpdate(string text, float percent)
  {
    if (this.InvokeRequired)
      this.Invoke((Delegate) this.BuildShemaUpdate, (object) text, (object) percent);
    else
      this.SetStatusInfoString(text, percent);
  }

  private void RelationVisualiserWindow_ThreadFinish(
    MapDocument result,
    object message,
    Exception exception)
  {
    if (this.InvokeRequired)
    {
      this.Invoke((Delegate) this.ThreadFinish, (object) result, message, (object) exception);
    }
    else
    {
      this.IsThreadBusy = false;
      switch (exception)
      {
        case null:
        case ThreadAbortException _:
          if (result == null)
            break;
          this.map.Document = result;
          if (this.setCenterNode)
          {
            MapLayer mapLayer = this.map.Document.Layers.Find((object) 0);
            if (mapLayer != null)
            {
              MapObject[] mapObjectArray = mapLayer.CopyArray();
              if (mapObjectArray != null && mapObjectArray.Length != 0)
                this.map.ScrollToControl(mapObjectArray[0]);
            }
            this.setCenterNode = false;
          }
          this.SetFinishStatusTitle();
          break;
        default:
          ExceptionHelper.ExceptionService.ShowException(new Exception(LocalizationHolder.rm.GetString("Pdm_rv_19") + exception.Message, exception));
          goto case null;
      }
    }
  }

  private void InitializeServices()
  {
    this._services = new AdvancedServiceContainer();
    this._notificationService = this.InitializeNotificationService();
    if (this._notificationService != null)
      this._services.AddService(typeof (INotificationService), (object) this._notificationService);
    this._images = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
    this._categoryObjTypeImages = ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
    Observer.objectTypeImageService = this._categoryObjTypeImages;
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
    this._CurrentUserAndRule = ServicesManager.GetService(typeof (ICurrentUserAndRole)) as ICurrentUserAndRole;
    if (this._CurrentUserAndRule != null)
      this._services.AddService(typeof (ICurrentUserAndRole), (object) this._CurrentUserAndRule);
    try
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        this.observerService = sessionKeeper.Session.GetCustomService(typeof (IRelVisObserverService)) as IRelVisObserverService;
        this.compositionLoadService = sessionKeeper.Session.GetCustomService(typeof (ICompositionLoadService)) as ICompositionLoadService;
      }
    }
    catch (SerializationException ex)
    {
    }
    catch (Exception ex)
    {
      throw ex;
    }
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
    this.UpdateCurrentSchema(DBEvent.DBEventType.All);
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
              this.dbEvents.AddRelationCountChanged(extendedEventArgs.RelationIDs[index], this.ConvertAttrCountValue(attributeValues.Values[0].ToString(), attributeValues.AttributeType));
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
              this.dbEvents.AddRelationCreatedEvent(current, relationsEventArgs1.GetProjID(current));
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
              this.dbEvents.AddRelationRemoveEvent(current, relationsEventArgs2.GetProjID(current));
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
              this.dbEvents.AddObjectRemoveEvent(enumerator.Current);
            return;
          }
        case 2691487867:
          if (!(eventName == "ObjectsCheckedIn"))
            return;
          break;
        case 3096070312:
          if (!(eventName == "ObjectsCheckedOut") || !(args is DBObjectsCheckOutEventArgs checkOutEventArgs))
            return;
          using (IEnumerator<long> enumerator = checkOutEventArgs.ObjectIDs.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.dbEvents.AddObjectInChangedEvent(enumerator.Current, (string) null);
            return;
          }
        case 3837095985:
          if (!(eventName == "ObjectsChanged") || !(args is DBObjectsEventArgs objectsEventArgs2))
            return;
          using (IEnumerator<long> enumerator = objectsEventArgs2.ObjectIDs.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              long current = enumerator.Current;
            }
            return;
          }
        default:
          return;
      }
      if (!(args is DBObjectsEventArgs objectsEventArgs3))
        return;
      foreach (long objectId in (IEnumerable<long>) objectsEventArgs3.ObjectIDs)
        this.dbEvents.AddObjectInChangedEvent(objectId, (string) null);
    }
    catch
    {
    }
  }

  private double ConvertAttrCountValue(string countStr, FieldTypes fType)
  {
    try
    {
      if (fType == FieldTypes.ftMeasured)
      {
        MeasuredValue measuredValue = this.measureConvertor.ConvertToMeasuredValue(countStr);
        if (measuredValue != null)
          return Convert.ToDouble(measuredValue.Value);
      }
      else
      {
        double result = 0.0;
        if (double.TryParse(countStr, out result))
          return result;
      }
    }
    catch (Exception ex)
    {
      throw new Exception(LocalizationHolder.rm.GetString("Pdm_rv_18"), ex);
    }
    return 0.0;
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
    this.bAssociateLinks.Checked = RelationVisualiserWindow.ShowAssociativeLinks;
    this.bStructureLinks.Checked = RelationVisualiserWindow.ShowStructLinks;
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
    XmlNode element = (XmlNode) state.CreateElement(nameof (RelationVisualiserWindow));
    state.AppendChild((XmlNode) state.CreateXmlDeclaration("1.0", (string) null, (string) null));
    if (this.currentSchema != null)
    {
      XmlAttribute attribute = state.CreateAttribute("ObjId");
      attribute.Value = this.currentSchema.ObjectVersionId.ToString();
      element.Attributes.Append(attribute);
    }
    if (this._FiltrationOwnerID.Length > 0)
    {
      XmlAttribute attribute = state.CreateAttribute("FiltrationOwnerID");
      attribute.Value = Convert.ToString(this.Get_FiltrationOwnerID());
      element.Attributes.Append(attribute);
    }
    XmlAttribute attribute1 = state.CreateAttribute("ShowParentTree");
    attribute1.Value = Convert.ToString(this.NeedParentTree());
    element.Attributes.Append(attribute1);
    XmlAttribute attribute2 = state.CreateAttribute("ShowChildTree");
    attribute2.Value = Convert.ToString(this.NeedChildTree());
    element.Attributes.Append(attribute2);
    state.AppendChild(element);
    return state;
  }

  protected virtual SchemaInfo RestoreState(XmlDocument xmlDoc)
  {
    SchemaInfo schemaInfo = (SchemaInfo) null;
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
                schemaInfo = new SchemaInfo(dbObject.ObjectID, dbObject.ID, dbObject.Caption, dbObject.ObjectType, this.currentLayoutAlgorithm.GetAlgorithmName(), this.settings);
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
            this.NeedParentTree(result);
        }
        XmlAttribute xmlAttribute4 = attributes["ShowChildTree"];
        if (xmlAttribute4 != null)
        {
          bool result = false;
          if (bool.TryParse(xmlAttribute4.Value, out result))
            this.NeedChildTree(result);
        }
      }
    }
    return schemaInfo;
  }

  public static DockControl RestoreWindowCallback(Guid guid, string persistString)
  {
    if (!guid.Equals(RelationVisualiserWindow._persistStateGuid))
      return (DockControl) null;
    try
    {
      XmlDocument xmlDoc = new XmlDocument();
      xmlDoc.LoadXml(persistString);
      RelationVisualiserWindow window = new RelationVisualiserWindow();
      try
      {
        SchemaInfo newShema = window.RestoreState(xmlDoc);
        if (newShema == null)
          return (DockControl) null;
        ((IWellKnownNavigators) ServicesManager.GetService(typeof (IWellKnownNavigators)))?.Register("desktopRelationVisualiserWindow", (Control) window);
        window.SetCurrentObject(newShema);
        return (DockControl) window;
      }
      catch
      {
      }
    }
    catch (Exception ex)
    {
      IOutputView service = ServicesManager.GetService(typeof (IOutputView)) as IOutputView;
      service.WriteString("Navigator", LocalizationHolder.rm.GetString("Client.Core_326"));
      service.WriteString("Navigator", ex.Message);
      return (DockControl) null;
    }
    return (DockControl) null;
  }

  private void AddShema2HistoryList(SchemaInfo oldShema)
  {
    if (oldShema == null)
      return;
    foreach (MenuItemBase menuItemBase in (CollectionBase) this.dropDownMenu_SelectShema.Items)
    {
      if (menuItemBase.Tag != null && menuItemBase.Tag.Equals((object) oldShema))
        return;
    }
    MenuButtonItem menuButtonItem = new MenuButtonItem(oldShema.ToString(), new EventHandler(this.toolStripComboBox1_SelectedIndexChanged));
    menuButtonItem.Tag = (object) oldShema;
    this.dropDownMenu_SelectShema.Items.Add((ToolbarItemBase) menuButtonItem);
  }

  private void ShowCurentShemaInHistoryMenu(SchemaInfo newShema)
  {
    foreach (MenuItemBase menuItemBase in (CollectionBase) this.dropDownMenu_SelectShema.Items)
    {
      if (menuItemBase.Tag != null && menuItemBase.Tag.Equals((object) newShema))
        menuItemBase.Checked = true;
      else
        menuItemBase.Checked = false;
    }
  }

  protected override void OnParentChanged(EventArgs e)
  {
    base.OnParentChanged(e);
    if (this.Parent == null || this.currentSchema != null || this.defaultSchema == null)
      return;
    this.SetCurrentObject(this.defaultSchema);
  }

  public void SetCurrentObject(SchemaInfo newShema)
  {
    if (this.currentSchema == null && this.Parent == null)
    {
      this.defaultSchema = newShema;
    }
    else
    {
      if (this.currentSchema != null && newShema.Equals((object) this.currentSchema))
        return;
      if (this.currentSchema != null && !this.SchemaList.ContainsKey(this.currentSchema))
        this.SchemaList.Add(this.currentSchema, this.map.Document);
      if (this.currentSchema != null)
        this.AddShema2HistoryList(this.currentSchema);
      this.ShowCurentShemaInHistoryMenu(newShema);
      this.currentSchema = newShema;
      string caption = this.currentSchema.Caption;
      this.TabText = caption;
      this.Text = caption;
      this.setCenterNode = true;
      this.BuildThread(Observer.BuildFlags.CreateTree);
    }
  }

  public void SetCurentObject(IDBTypedObjectID ObjID)
  {
    if (ObjID == null || ObjID.ObjectID == 0L)
      return;
    this.SetCurrentObject(new SchemaInfo(ObjID.ObjectID, ObjID.ID, ObjID.Caption, ObjID.ObjectType, this.currentLayoutAlgorithm.GetAlgorithmName(), this.settings));
  }

  private bool ChangeLayoutAlgorithm(string layoutAlgoritmName)
  {
    if (this.currentLayoutAlgorithm.GetAlgorithmName() == layoutAlgoritmName)
      return false;
    if (ForceDirected.AlgoritmName() == layoutAlgoritmName)
    {
      this.currentLayoutAlgorithm = (ILayoutAlgorithm) new ForceDirected();
      this.menuButtonItem_LA_Hier.Checked = false;
      this.menuButtonItem_LA_Kulon.Checked = true;
      this.menuButtonItem_LA_Normal.Checked = false;
      return true;
    }
    if (HierarchicalLayout.AlgorithmName() == layoutAlgoritmName)
    {
      this.currentLayoutAlgorithm = (ILayoutAlgorithm) new HierarchicalLayout();
      this.menuButtonItem_LA_Hier.Checked = true;
      this.menuButtonItem_LA_Kulon.Checked = false;
      this.menuButtonItem_LA_Normal.Checked = false;
      return true;
    }
    if (!(NormalLayout.AlgorithmName() == layoutAlgoritmName))
      return false;
    this.currentLayoutAlgorithm = (ILayoutAlgorithm) new NormalLayout();
    this.menuButtonItem_LA_Hier.Checked = false;
    this.menuButtonItem_LA_Kulon.Checked = false;
    this.menuButtonItem_LA_Normal.Checked = true;
    return true;
  }

  private void DeleteRelation()
  {
    if (this.currentSelectedRelation == null || this.currentSelectedRelation.RelId == 0L)
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
      if (this.currentSchema.IsMultiContainsMode)
      {
        foreach (MapObject rel in this.map.Document.GetEnumerator())
        {
          if (rel is RelMapLink && (rel as RelMapLink).RelId == this.currentSelectedRelation.RelId)
            this.DeleteRelation(rel as RelMapLink);
        }
      }
      else
        this.DeleteRelation(this.currentSelectedRelation);
      this.NotificationService.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsRemoved", this.currentSelectedRelation.RelId));
      this.currentSelectedRelation = (RelMapLink) null;
    }
  }

  public void DeleteRelation(RelMapLink rel) => rel.Remove();

  private void map_OnRelationCreated(VisObjectNode from, VisObjectNode to, IMapLink il)
  {
    bool flag = false;
    try
    {
      if (il == null || !(il is RelMapLink))
        return;
      RelMapLink relMapLink = il as RelMapLink;
      if (from == null || to == null || from.ObjectVerId == 0L || to.ObjectVerId == 0L)
        return;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        int relationsTypeByObjTypes = Observer.GetRelationsTypeByObjTypes(from.ObjectTypeId, to.ObjectTypeId, sessionKeeper.Session);
        if (relationsTypeByObjTypes == -1)
          throw new Exception(LocalizationHolder.rm.GetString("Pdm_rv_2"));
        IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(relationsTypeByObjTypes);
        if (relationCollection == null)
          return;
        IDBRelation dbRelation = relationCollection.Create(to.ObjectVerId, from.ObjectVerId);
        if (dbRelation == null)
          return;
        flag = true;
        long relationId = dbRelation.RelationID;
        relMapLink.RelId = relationId;
        this.NotificationService.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsCreated", relationId));
      }
    }
    finally
    {
      il.MapObject.Remove();
      if (flag)
        this.BuildThread(Observer.BuildFlags.UpdateTree);
    }
  }

  private void map_SelectionDeleted(object sender, EventArgs e) => this.DeleteRelation();

  private void map_ClipboardPasted(object sender, EventArgs e) => this.ExecuteMenuCommand("Paste");

  private void map_ObjectContextClicked(object sender, MapObjectEventArgs e)
  {
    if (e.MapObject != null && e.MapObject.ParentNode != null && e.MapObject.ParentNode is VisObjectNode)
    {
      if (this.currentSelectedObjects == null)
        return;
      this.GetContectMenu4Object(this.currentSelectedObjects).Show((Control) this.map, e.ViewPoint);
      this.UpdateCurrentSchema(DBEvent.DBEventType.All);
    }
    else
    {
      if (e.MapObject == null || e.MapObject.ParentNode == null || !(e.MapObject.ParentNode is RelMapLink))
        return;
      this.currentSelectedRelation = e.MapObject.ParentNode as RelMapLink;
      this.contextMenuStrip_map.Show((Control) this.map, e.ViewPoint);
    }
  }

  private MenuBarItem GetContectMenu4Object(ISelectedItems items)
  {
    AdvancedServiceContainer viewServices = new AdvancedServiceContainer();
    viewServices.AddService(typeof (IViewState), (object) new ViewStateService());
    MenuBarItem menu = Intermech.Navigator.ContextMenu.Services.GetMenu(items, (IServiceProvider) viewServices);
    MenuItemBase menuItemBase1 = menu.FindItem("PDM.RelationVisualizer");
    if (menuItemBase1 != null)
    {
      menu.Items.Remove((ToolbarItemBase) menuItemBase1);
      MenuItemBase menuItemBase2 = (MenuItemBase) new MenuButtonItem(LocalizationHolder.rm.GetString("Pdm_rv_17"), new EventHandler(this.relVis_Click));
      int index = menuItemBase1.Index + 1;
      menu.Items.Insert(index, (ToolbarItemBase) menuItemBase2);
    }
    return menu;
  }

  private void relVis_Click(object sender, EventArgs e)
  {
    if (this.currentSelectedObjects == null || this.currentSelectedObjects.Count == 0 || !(this.currentSelectedObjects.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData))
      return;
    this.SetCurentObject(itemData);
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
      if (e.MapObject is VisObjectNode)
      {
        VisObjectNode mapObject = e.MapObject as VisObjectNode;
        ISelectedItems items = Intermech.Navigator.ContextMenu.Services.GetItems(mapObject.ObjectVerId);
        if (items.Count == 0)
          items = Intermech.Navigator.ContextMenu.Services.GetItems(-mapObject.ObjectVerId);
        this.ShowObjectProps(mapObject, false);
        this.currentSelectedObjects = items;
      }
      else
      {
        if (e.MapObject is RelMapLink)
        {
          this.currentSelectedRelation = e.MapObject as RelMapLink;
          this.ShowRelationProps(this.currentSelectedRelation, false);
        }
        else
          this.ShowObjectProps((VisObjectNode) null, false);
        this.currentSelectedObjects = (ISelectedItems) null;
      }
      this._cmdMngr.QueryStatus();
    }
    else
      this.currentSelectedObjects = (ISelectedItems) null;
  }

  private void toolStripButton_ZoomIn_Click(object sender, EventArgs e) => this.map.ZoomIn();

  private void toolStripButton_ZoomOut_Click(object sender, EventArgs e) => this.map.ZoomOut();

  private void toolStripButton_WidthHeigth_Click(object sender, EventArgs e)
  {
    double num = 0.8;
    if (sender == this.buttonItem_HeigthIn)
      HierarchicalLayout.YKoef *= num;
    if (sender == this.buttonItem_HeigthOut)
      HierarchicalLayout.YKoef /= num;
    if (sender == this.buttonItem_WidthIn)
      HierarchicalLayout.XKoef *= num;
    if (sender == this.buttonItem_WidthOut)
      HierarchicalLayout.XKoef /= num;
    try
    {
      this.lockUpdateStatus = true;
      this.LayoutDocument(this.map.Document);
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
    this.map.Invalidate();
  }

  private void toolStripButton_Status_Click(object sender, EventArgs e)
  {
    this.settings.ShowStatuses = this.buttonItem_Status.Checked;
    if (this.currentSchema.IsLoadStatuses)
    {
      this.map.Invalidate();
    }
    else
    {
      this.BuildThread(Observer.BuildFlags.UpdateTree);
      this.currentSchema.IsLoadStatuses = true;
    }
  }

  private void ToFitCurDocument(object sender, EventArgs e) => this.map.ZoomToFit();

  private void toolStripButton_ZoomOnce_Click(object sender, EventArgs e)
  {
    this.map.ZoomOnceCurDocument();
  }

  private void LockControls(bool islock) => this.toolBar.Enabled = !islock;

  private void toolStripButton_ifNeedChildTree_Click(object sender, EventArgs e)
  {
    if (!this.NeedParentTree())
      this.NeedChildTree(true);
    else
      this.NeedChildTree(!this.buttonItem_IfNeedChildTree.Checked);
    this.map.Document.Layers.Find((object) 2).AllowView = this.NeedChildTree();
    if (this.currentSchema.Statistic != null && !this.currentSchema.Statistic.isReadChildTree)
      this.BuildThread(Observer.BuildFlags.UpdateTree);
    else
      this.LayoutDocument(this.map.Document);
  }

  private void toolStripButton_ifNeedParentTree_Click(object sender, EventArgs e)
  {
    if (!this.NeedChildTree())
      this.NeedParentTree(true);
    else
      this.NeedParentTree(!this.buttonItem_IfNeedParentTree.Checked);
    this.map.Document.Layers.Find((object) 1).AllowView = this.NeedParentTree();
    if (this.currentSchema.Statistic != null && !this.currentSchema.Statistic.isReadParentTree)
      this.BuildThread(Observer.BuildFlags.UpdateTree);
    else
      this.LayoutDocument(this.map.Document);
  }

  private void FindObjectbyShema(object sender, EventArgs e)
  {
    using (FindObjectDialog findObjectDialog = new FindObjectDialog((MapView) this.map))
    {
      int num = (int) findObjectDialog.ShowDialog();
    }
  }

  private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
  {
    if (!(sender is MenuButtonItem menuButtonItem) || menuButtonItem.Tag == null && menuButtonItem.Tag is SchemaInfo)
      return;
    SchemaInfo tag = menuButtonItem.Tag as SchemaInfo;
    this.ChangeLayoutAlgorithm(tag.LayoutAlgoritmName);
    this.SetCurrentObject(tag);
  }

  private void ShowRelationProps(RelMapLink relation, bool isOpenPanel)
  {
    if (isOpenPanel)
      this.collapsibleSplitter.ControlToHide.Show();
    else if (!this.collapsibleSplitter.ControlToHide.Visible)
      return;
    this.propertyGridPanel.LoadObject((MapObject) relation);
  }

  private void ShowObjectProps(VisObjectNode obj, bool isOpenPanel)
  {
    if (isOpenPanel)
      this.collapsibleSplitter.ControlToHide.Show();
    else if (!this.collapsibleSplitter.ControlToHide.Visible)
      return;
    this.propertyGridPanel.LoadObject((MapObject) obj);
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

  private void BarCommand_Refresh() => this.BuildThread(Observer.BuildFlags.UpdateTree);

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
    if (!this.menuButtonItem_LA_Normal.Checked)
      return;
    this.DisposeLayoutAlgoritm();
    this.currentLayoutAlgorithm = (ILayoutAlgorithm) new NormalLayout();
    this.InitLayoutAlgoritm();
    this.menuButtonItem_LA_Kulon.Checked = false;
    this.menuButtonItem_LA_Hier.Checked = false;
    this.SetCurrentObject(new SchemaInfo(this.currentSchema, this.currentLayoutAlgorithm.GetAlgorithmName()));
  }

  private void toolStripMenuItem_LA_Kulon_Click(object sender, EventArgs e)
  {
    if (!this.menuButtonItem_LA_Kulon.Checked)
      return;
    this.DisposeLayoutAlgoritm();
    this.currentLayoutAlgorithm = (ILayoutAlgorithm) new ForceDirected();
    this.InitLayoutAlgoritm();
    this.menuButtonItem_LA_Normal.Checked = false;
    this.menuButtonItem_LA_Hier.Checked = false;
    this.SetCurrentObject(new SchemaInfo(this.currentSchema, this.currentLayoutAlgorithm.GetAlgorithmName()));
  }

  private void toolStripMenuItem_LA_Hier_Click(object sender, EventArgs e)
  {
    if (!this.menuButtonItem_LA_Hier.Checked)
      return;
    this.DisposeLayoutAlgoritm();
    this.currentLayoutAlgorithm = (ILayoutAlgorithm) new HierarchicalLayout();
    this.InitLayoutAlgoritm();
    this.menuButtonItem_LA_Normal.Checked = false;
    this.menuButtonItem_LA_Kulon.Checked = false;
    this.SetCurrentObject(new SchemaInfo(this.currentSchema, this.currentLayoutAlgorithm.GetAlgorithmName()));
  }

  public override string HelpID => string.Empty;

  private void bStructureLinks_Click(object sender, EventArgs e)
  {
    RelationVisualiserWindow.ShowStructLinks = !RelationVisualiserWindow.ShowStructLinks;
    this.UpdateControls();
  }

  private void bAssociateLinks_Click(object sender, EventArgs e)
  {
    RelationVisualiserWindow.ShowAssociativeLinks = !RelationVisualiserWindow.ShowAssociativeLinks;
    this.UpdateControls();
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
    this.currentSelectedRelation = (RelMapLink) null;
    this.currentSchema = (SchemaInfo) null;
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (RelationVisualiserWindow));
    this.statusBar = new StatusStrip();
    this.toolStripStatus_Stop = new ToolStripStatusLabel();
    this.toolStripStatus = new ToolStripStatusLabel();
    this.contextMenuStrip_map = new ContextMenuStrip(this.components);
    this.toolStripMenuItem_props = new ToolStripMenuItem();
    this.toolStripMenuItem_delete = new ToolStripMenuItem();
    this.collapsibleSplitter = new CollapsibleSplitter();
    this.propertyGridPanel = new RelVisualItemsPropPanel();
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
    this.dropDownMenu_SelectShema = new DropDownMenuItem();
    this.dropDownMenu_SelectLO_Alg = new DropDownMenuItem();
    this.menuButtonItem_LA_Normal = new MenuButtonItem();
    this.menuButtonItem_LA_Kulon = new MenuButtonItem();
    this.menuButtonItem_LA_Hier = new MenuButtonItem();
    this.buttonItem_LevelP = new ButtonItem();
    this.buttonItem_Status = new ButtonItem();
    this.bStructureLinks = new ButtonItem();
    this.bAssociateLinks = new ButtonItem();
    this.map = new RelViewControl(this.components);
    this.statusBar.SuspendLayout();
    this.contextMenuStrip_map.SuspendLayout();
    this.SuspendLayout();
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
    this.propertyGridPanel.IsModificated = false;
    this.propertyGridPanel.Name = "propertyGridPanel";
    componentResourceManager.ApplyResources((object) this.toolBar, "toolBar");
    this.toolBar.Flow = ToolBarLayout.Vertical;
    this.toolBar.FullMenus = true;
    this.toolBar.Guid = new Guid("feffc6ee-eb37-47cd-ae3d-9e2f2c8a3e3a");
    this.toolBar.Hidden = false;
    this.toolBar.Items.AddRange(new ToolbarItemBase[17]
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
      (ToolbarItemBase) this.dropDownMenu_SelectShema,
      (ToolbarItemBase) this.dropDownMenu_SelectLO_Alg,
      (ToolbarItemBase) this.buttonItem_LevelP,
      (ToolbarItemBase) this.buttonItem_Status,
      (ToolbarItemBase) this.bStructureLinks,
      (ToolbarItemBase) this.bAssociateLinks
    });
    this.toolBar.Name = "toolBar";
    componentResourceManager.ApplyResources((object) this.buttonItem_ZoomIn, "buttonItem_ZoomIn");
    this.buttonItem_ZoomIn.Click += new EventHandler(this.toolStripButton_ZoomIn_Click);
    componentResourceManager.ApplyResources((object) this.buttonItem_ZoomOut, "buttonItem_ZoomOut");
    this.buttonItem_ZoomOut.Click += new EventHandler(this.toolStripButton_ZoomOut_Click);
    componentResourceManager.ApplyResources((object) this.buttonItem_ZoomOnce, "buttonItem_ZoomOnce");
    this.buttonItem_ZoomOnce.Click += new EventHandler(this.toolStripButton_ZoomOnce_Click);
    componentResourceManager.ApplyResources((object) this.buttonItem_AllShema, "buttonItem_AllShema");
    this.buttonItem_AllShema.Click += new EventHandler(this.ToFitCurDocument);
    componentResourceManager.ApplyResources((object) this.buttonItem_WidthIn, "buttonItem_WidthIn");
    this.buttonItem_WidthIn.Image = (Image) Intermech.Pdm.Properties.Resources.Arrow1;
    this.buttonItem_WidthIn.ToolTipText = this.buttonItem_WidthIn.Text;
    this.buttonItem_WidthIn.Click += new EventHandler(this.toolStripButton_WidthHeigth_Click);
    componentResourceManager.ApplyResources((object) this.buttonItem_WidthOut, "buttonItem_WidthOut");
    this.buttonItem_WidthOut.Image = (Image) Intermech.Pdm.Properties.Resources.Arrow_Left_Right;
    this.buttonItem_WidthOut.ToolTipText = this.buttonItem_WidthOut.Text;
    this.buttonItem_WidthOut.Click += new EventHandler(this.toolStripButton_WidthHeigth_Click);
    componentResourceManager.ApplyResources((object) this.buttonItem_HeigthIn, "buttonItem_HeigthIn");
    this.buttonItem_HeigthIn.Image = (Image) Intermech.Pdm.Properties.Resources.Arrow2;
    this.buttonItem_HeigthIn.ToolTipText = this.buttonItem_HeigthIn.Text;
    this.buttonItem_HeigthIn.Click += new EventHandler(this.toolStripButton_WidthHeigth_Click);
    componentResourceManager.ApplyResources((object) this.buttonItem_HeigthOut, "buttonItem_HeigthOut");
    this.buttonItem_HeigthOut.Image = (Image) Intermech.Pdm.Properties.Resources.Arrow_Up_Down;
    this.buttonItem_HeigthOut.ToolTipText = this.buttonItem_HeigthOut.Text;
    this.buttonItem_HeigthOut.Click += new EventHandler(this.toolStripButton_WidthHeigth_Click);
    componentResourceManager.ApplyResources((object) this.buttonItem_IfNeedParentTree, "buttonItem_IfNeedParentTree");
    this.buttonItem_IfNeedParentTree.Image = (Image) Intermech.Pdm.Properties.Resources.rarent;
    this.buttonItem_IfNeedParentTree.Click += new EventHandler(this.toolStripButton_ifNeedParentTree_Click);
    this.buttonItem_IfNeedChildTree.Checked = true;
    componentResourceManager.ApplyResources((object) this.buttonItem_IfNeedChildTree, "buttonItem_IfNeedChildTree");
    this.buttonItem_IfNeedChildTree.Image = (Image) Intermech.Pdm.Properties.Resources.child;
    this.buttonItem_IfNeedChildTree.Click += new EventHandler(this.toolStripButton_ifNeedChildTree_Click);
    componentResourceManager.ApplyResources((object) this.buttonItem_FindObject, "buttonItem_FindObject");
    this.buttonItem_FindObject.Click += new EventHandler(this.FindObjectbyShema);
    componentResourceManager.ApplyResources((object) this.dropDownMenu_SelectShema, "dropDownMenu_SelectShema");
    this.dropDownMenu_SelectShema.ShowText = true;
    componentResourceManager.ApplyResources((object) this.dropDownMenu_SelectLO_Alg, "dropDownMenu_SelectLO_Alg");
    this.dropDownMenu_SelectLO_Alg.Image = (Image) Intermech.Pdm.Properties.Resources.Schema;
    this.dropDownMenu_SelectLO_Alg.Items.AddRange(new ToolbarItemBase[3]
    {
      (ToolbarItemBase) this.menuButtonItem_LA_Normal,
      (ToolbarItemBase) this.menuButtonItem_LA_Kulon,
      (ToolbarItemBase) this.menuButtonItem_LA_Hier
    });
    this.dropDownMenu_SelectLO_Alg.ShowText = true;
    this.menuButtonItem_LA_Normal.AutoToggle = AutoToggleType.Single;
    this.menuButtonItem_LA_Normal.Checked = true;
    componentResourceManager.ApplyResources((object) this.menuButtonItem_LA_Normal, "menuButtonItem_LA_Normal");
    this.menuButtonItem_LA_Normal.ShowText = true;
    this.menuButtonItem_LA_Normal.Click += new EventHandler(this.toolStripMenuItem_LA_Normal_Click);
    this.menuButtonItem_LA_Kulon.AutoToggle = AutoToggleType.Single;
    componentResourceManager.ApplyResources((object) this.menuButtonItem_LA_Kulon, "menuButtonItem_LA_Kulon");
    this.menuButtonItem_LA_Kulon.ShowText = true;
    this.menuButtonItem_LA_Kulon.Click += new EventHandler(this.toolStripMenuItem_LA_Kulon_Click);
    this.menuButtonItem_LA_Hier.AutoToggle = AutoToggleType.Single;
    componentResourceManager.ApplyResources((object) this.menuButtonItem_LA_Hier, "menuButtonItem_LA_Hier");
    this.menuButtonItem_LA_Hier.ShowText = true;
    this.menuButtonItem_LA_Hier.Click += new EventHandler(this.toolStripMenuItem_LA_Hier_Click);
    this.buttonItem_LevelP.AutoToggle = AutoToggleType.Single;
    componentResourceManager.ApplyResources((object) this.buttonItem_LevelP, "buttonItem_LevelP");
    this.buttonItem_LevelP.Image = (Image) Intermech.Pdm.Properties.Resources.lc_levels;
    this.buttonItem_LevelP.Click += new EventHandler(this.toolStripButton_LevelP_Click);
    this.buttonItem_Status.AutoToggle = AutoToggleType.Single;
    componentResourceManager.ApplyResources((object) this.buttonItem_Status, "buttonItem_Status");
    this.buttonItem_Status.Click += new EventHandler(this.toolStripButton_Status_Click);
    this.bStructureLinks.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.bStructureLinks, "bStructureLinks");
    this.bStructureLinks.Image = (Image) componentResourceManager.GetObject("bStructureLinks.Image");
    this.bStructureLinks.Click += new EventHandler(this.bStructureLinks_Click);
    componentResourceManager.ApplyResources((object) this.bAssociateLinks, "bAssociateLinks");
    this.bAssociateLinks.Image = (Image) componentResourceManager.GetObject("bAssociateLinks.Image");
    this.bAssociateLinks.Click += new EventHandler(this.bAssociateLinks_Click);
    this.map.AllowDrop = true;
    this.map.BackColor = Color.White;
    componentResourceManager.ApplyResources((object) this.map, "map");
    this.map.MaximumSelectionCount = 1;
    this.map.Name = "map";
    this.AutoScaleMode = AutoScaleMode.Inherit;
    this.BackColor = SystemColors.Control;
    this.Controls.Add((Control) this.map);
    this.Controls.Add((Control) this.toolBar);
    this.Controls.Add((Control) this.collapsibleSplitter);
    this.Controls.Add((Control) this.propertyGridPanel);
    this.Controls.Add((Control) this.statusBar);
    this.Name = nameof (RelationVisualiserWindow);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.statusBar.ResumeLayout(false);
    this.statusBar.PerformLayout();
    this.contextMenuStrip_map.ResumeLayout(false);
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  public delegate void ThreadFinishEventHandler(
    MapDocument result,
    object message,
    Exception exception);

  public delegate void ThreadBuildShepaUpdaeInfo(string text, float percent);
}
