// Decompiled with JetBrains decompiler
// Type: Intermech.Navigator.Plugin
// Assembly: Intermech.Navigator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FA68CCDA-C8AC-453D-A97D-7A56D5366A1E
// Assembly location: D:\IPS\Client\Intermech.Navigator.dll

using Intermech.Bars;
using Intermech.Client.Core;
using Intermech.DataFormats;
using Intermech.Docking;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Plugins;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.NavBars;
using Intermech.Navigator.ContextCommands;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Controls;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Redlining;
using Intermech.Navigator.Views;
using Intermech.Search;
using Intermech.Search.RecentObjects;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace Intermech.Navigator;

internal class Plugin : 
  IPackage,
  ICommandTarget,
  ICurrentNavWindow,
  ICommandsProvider,
  IDescriptorElementStatusesService,
  IEnableTreeMultiSelect,
  IEnableTreeColumnsSorting,
  IEnableTreeCollapse,
  IIODestination
{
  private static readonly string _pluginName = LocalizationHolder.rm.GetString("Navigator_1");
  private static readonly string _navName = LocalizationHolder.rm.GetString("Navigator_2");
  private const string _navImageName = "imgNavigator";
  private const string _navWindowName = "mainNavigator";
  private static readonly string _desktopName = LocalizationHolder.rm.GetString("Navigator_3");
  private static readonly Guid _desktopGuid = new Guid("{CA6135C4-40F3-4fad-83B7-DDE417ECB457}");
  private const string _desktopImageName = "imgDesktopObjectType";
  private const string _desktopWindowName = "desktopNavigator";
  private static readonly string _recentObjectsName = LocalizationHolder.rm.GetString("Navigator_4");
  private const string _recentObjectsImageName = "imgRecentObjects";
  private const string _recentObjectsWindowName = "desktopRecentObjects";
  private static readonly string _errorCaption = LocalizationHolder.rm.GetString("Navigator_5");
  private static readonly string _errorDesktopNotFound = LocalizationHolder.rm.GetString("Navigator_6");
  private static readonly object[] _desktopsColumns = new object[1]
  {
    (object) ObligatoryObjectAttributes.F_OBJECT_ID
  };
  private static readonly SortOrders[] _desktopSortOrders = new SortOrders[1]
  {
    SortOrders.ASC
  };
  private static readonly Guid _Guid = new Guid("{7B3A3DEC-70C3-4AFD-AECD-B26C21A1CC8E}");
  private static ICategoryTypeIconService _categoryImages;
  private static INavigatorColumnsService _navigatorColumnsService;
  internal static int _objtypeSelections = -1;
  internal static int _objtypeClassifyers = -1;
  internal static IIODispatcher _IODispatcher;
  public static readonly Guid CategoryFavoritesRootNodeGuid = new Guid("{54DC89C7-75BA-4759-95EE-40D09E6D9E64}");
  private static readonly string _favoritesName = LocalizationHolder.rm.GetString("Navigator_16");
  private const string _favoritesImageName = "imgFavorites";
  private static readonly Guid _favoritesWindowGuid = new Guid("{ADB04943-7E2A-47A4-A1E8-23672AAEFD2A}");
  private const string _favoritesWindowName = "favoritesNavigator";
  internal object navWindow;
  internal object treeView;
  internal object viewsManagers;
  private LazyService<IMainMenuService> _mainMenuService = new LazyService<IMainMenuService>();

  string IPackage.Name
  {
    [DebuggerStepThrough] get => Plugin._pluginName;
  }

  void IPackage.Load(IServiceProvider serviceProvider)
  {
    int imageIndex1 = -1;
    int imageIndex2 = -1;
    int imageIndex3 = -1;
    int imageIndex4 = -1;
    int imageIndex5 = -1;
    Plugin._IODispatcher = ServicesManager.GetService(typeof (IIODispatcher)) as IIODispatcher;
    if (Plugin._IODispatcher != null)
      Plugin._IODispatcher.RegisterDestination((IIODestination) this);
    INamedImageList service1 = (INamedImageList) serviceProvider.GetService(typeof (INamedImageList));
    Plugin._categoryImages = ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
    Plugin._navigatorColumnsService = ServicesManager.GetService(typeof (INavigatorColumnsService)) as INavigatorColumnsService;
    if (Plugin._navigatorColumnsService != null)
      Plugin._navigatorColumnsService.OnGetCategoryTypeParentEventHandler += new Intermech.Navigator.Interfaces.GetCategoryTypeParentEventHandler(this.GetCategoryTypeParentEventHandler);
    if (service1 != null)
    {
      imageIndex1 = service1.ImageIndex("imgNavigator");
      Icon icon = Plugin._categoryImages.GetIcon(4, MetaDataHelper.GetObjectTypeID(new Guid("cad0004a-306c-11d8-b4e9-00304f19f545")));
      service1.Add(icon, "imgDesktopObjectType");
      imageIndex2 = service1.ImageIndex("imgDesktopObjectType");
      imageIndex3 = service1.ImageIndex("imgRecentObjects");
      imageIndex4 = service1.ImageIndex("imgObject");
      imageIndex5 = service1.ImageIndex("imgFavorites");
    }
    IWellKnownWindowsOpenService service2 = ServicesManager.GetService(typeof (IWellKnownWindowsOpenService)) as IWellKnownWindowsOpenService;
    (ServicesManager.GetService(typeof (ICommandManager)) as ICommandManager).AddTarget((ICommandTarget) this);
    ICurrentUserAndRole service3 = ServicesManager.GetService(typeof (ICurrentUserAndRole)) as ICurrentUserAndRole;
    MenuButtonItem menuButtonItem1 = new MenuButtonItem(Plugin._navName);
    menuButtonItem1.CommandName = "ShowNavigator";
    menuButtonItem1.ImageIndex = imageIndex1;
    menuButtonItem1.Click += new EventHandler(this.ShowNavigatorClick);
    menuButtonItem1.ShortcutActive = true;
    menuButtonItem1.Shortcut = Shortcut.CtrlShiftN;
    this._mainMenuService.Value.RegisterMenuItems(MainMenuItemSite.Applications, MainMenuItemPosition.First, menuButtonItem1);
    service2?.RegisterWindowOpeningHandler("mainNavigator", new EventHandler(this.ShowNavigatorClick));
    MenuButtonItem menuButtonItem2 = new MenuButtonItem(Plugin._desktopName);
    menuButtonItem2.CommandName = "ShowDesktop";
    menuButtonItem2.ImageIndex = imageIndex2;
    menuButtonItem2.Click += new EventHandler(this.ShowDesktopClick);
    menuButtonItem2.ShortcutActive = true;
    menuButtonItem2.Shortcut = Shortcut.CtrlShiftD;
    this._mainMenuService.Value.RegisterMenuItems(MainMenuItemSite.Applications, MainMenuItemPosition.Default, menuButtonItem2);
    service2?.RegisterWindowOpeningHandler("desktopNavigator", new EventHandler(this.ShowDesktopClick));
    MenuButtonItem menuButtonItem3 = new MenuButtonItem(Plugin._recentObjectsName);
    menuButtonItem3.CommandName = "ShowRecentObjects";
    menuButtonItem3.ImageIndex = imageIndex3;
    menuButtonItem3.Click += new EventHandler(this.ShowRecentObjectsClick);
    menuButtonItem3.ShortcutActive = true;
    menuButtonItem3.Shortcut = Shortcut.CtrlShiftR;
    this._mainMenuService.Value.RegisterMenuItems(MainMenuItemSite.Applications, MainMenuItemPosition.Default, menuButtonItem3);
    service2?.RegisterWindowOpeningHandler("desktopRecentObjects", new EventHandler(this.ShowRecentObjectsClick));
    MenuButtonItem menuButtonItem4 = new MenuButtonItem(Plugin._favoritesName);
    menuButtonItem4.CommandName = "ShowFavorites";
    menuButtonItem4.ImageIndex = imageIndex5;
    menuButtonItem4.Click += new EventHandler(this.ShowFavoritesClick);
    this._mainMenuService.Value.RegisterMenuItems(MainMenuItemSite.Applications, MainMenuItemPosition.Default, menuButtonItem4);
    service2?.RegisterWindowOpeningHandler("favoritesNavigator", new EventHandler(this.ShowFavoritesClick));
    if (service3 != null && service3.IsAdmin)
    {
      MenuButtonItem menuButtonItem5 = new MenuButtonItem(CompositionsAutosortRulesWindow.AutosortName);
      menuButtonItem5.CommandName = "ShowAutosortRulesEditor";
      menuButtonItem5.BeginGroup = false;
      menuButtonItem5.ImageIndex = imageIndex4;
      menuButtonItem5.Click += new EventHandler(this.ShowAutosortRulesEditorClick);
      this._mainMenuService.Value.RegisterMenuItems(MainMenuItemSite.TuningTop, MainMenuItemPosition.Third, menuButtonItem5);
      service2?.RegisterWindowOpeningHandler("desktopAutosortWindow", new EventHandler(this.ShowAutosortRulesEditorClick));
    }
    INavigationBar service4 = (INavigationBar) serviceProvider.GetService(typeof (INavigationBar));
    if (service4 != null)
    {
      if (service4.FindPane("appPane") is IAppPane pane1)
      {
        pane1.Add(Plugin._navName, new EventHandler(this.ShowNavigatorClick), imageIndex1);
        pane1.Add(Plugin._desktopName, new EventHandler(this.ShowDesktopClick), imageIndex2);
        pane1.Add(Plugin._recentObjectsName, new EventHandler(this.ShowRecentObjectsClick), imageIndex3);
        pane1.Add(Plugin._favoritesName, new EventHandler(this.ShowFavoritesClick), imageIndex5);
      }
      if (service4.FindPane("adminPane") is IAppPane pane2 && service3 != null && service3.IsAdmin)
        pane2.Add(CompositionsAutosortRulesWindow.AutosortName, new EventHandler(this.ShowAutosortRulesEditorClick), imageIndex4);
    }
    ServicesManager.AddService(typeof (ISimpleExcelReports), (object) new SimpleExcelReports());
    ServicesManager.AddService(typeof (ICurrentNavWindow), (object) this);
    ServicesManager.AddService(typeof (IDescriptorElementStatusesService), (object) this);
    if (ServicesManager.GetService(typeof (IEnableTreeMultiSelectService)) is IEnableTreeMultiSelectService service5)
      service5.Register((IEnableTreeMultiSelect) this);
    if (ServicesManager.GetService(typeof (IEnableTreeColumnsSortingService)) is IEnableTreeColumnsSortingService service6)
      service6.Register((IEnableTreeColumnsSorting) this);
    if (ServicesManager.GetService(typeof (INavigatorTreeCollapseService)) is INavigatorTreeCollapseService service7)
      service7.Register((IEnableTreeCollapse) this);
    (ServicesManager.GetService(typeof (IViewsManagerService)) as IViewsManagerService).OnActivateView += new Intermech.Interfaces.Client.ActivateViewEventHandler(this.ActivateViewEventHandler);
    NavigatorWindowCaptionsHelper.OnGetNavigatorWindowCaption += new Intermech.Interfaces.Client.NavigatorWindowCaptionEventHandler(this.NavigatorWindowCaptionEventHandler);
    IFactory service8 = ServicesManager.GetService(typeof (IFactory)) as IFactory;
    MenuTemplate contextMenuTemplate = service8.ContextMenuTemplate;
    try
    {
      contextMenuTemplate.BeginUpdate();
      MenuTemplateNode menuTemplateNode = service8.ContextMenuTemplate["Create"];
      if (menuTemplateNode != null)
      {
        menuTemplateNode.Nodes.Add(new MenuTemplateNode("Navigator.CreateObjectType", LocalizationHolder.rm.GetString("Navigator_11"), -1, 10, 9));
        menuTemplateNode.Nodes.Add(new MenuTemplateNode("Navigator.CreateObjectTypeInclude", LocalizationHolder.rm.GetString("Navigator_12"), -1, 10, 10));
      }
      MenuTemplateNode createInComposition = service8.ContextMenuTemplate["CreateInComposition"];
      if (createInComposition != null)
      {
        if (!(ServicesManager.GetService(typeof (ICreateObjByTypeMRU)) is ICreateObjByTypeMRU service9))
          return;
        for (int index = 0; index < service9.Count; ++index)
        {
          IMSObjectType objectType = MetaDataHelper.GetObjectType((int) service9[index].Value);
          Plugin.AddObjectTypeToMenu(createInComposition, objectType, 11 + index);
        }
      }
    }
    finally
    {
      contextMenuTemplate.EndUpdate();
    }
    service8.AddCommandsProvider(1, (ICommandsProvider) this);
    service8.OnMenuTemplateNodeTransformEventHandler += new Intermech.Navigator.ContextMenu.MenuTemplateNodeTransformEventHandler(this.MenuTemplateNodeTransformEventHandler);
    ClientRedliningService redliningService = new ClientRedliningService();
    MetaDataHelperService.Instance.OnCacheReloaded += new MetaDataHelperEventHandler(this.MetaDataHelper_OnCacheReloaded);
    IContentProvider service10 = (IContentProvider) serviceProvider.GetService(typeof (IContentProvider));
    if (service10 != null)
      service10.ContentCallback += new GetContentCallback(this.ContentCallback);
    if (!(ServicesManager.GetService(typeof (IObjectCreatorService)) is IObjectCreatorService service11))
      return;
    service11.AfterObjectCreatedEvent += new AfterObjectCreatedEventHandler(this.NewObjectCreated);
  }

  void IPackage.Unload()
  {
  }

  public bool QueryStatus(ICommandState commandState)
  {
    if (!(commandState.CommandName == "CreateNew"))
      return false;
    commandState.Enabled = true;
    return true;
  }

  public bool Execute(ICommandState commandState)
  {
    if (!(commandState.CommandName == "CreateNew"))
      return false;
    IObjectCreatorService service = ServicesManager.GetService(typeof (IObjectCreatorService)) as IObjectCreatorService;
    (ServicesManager.GetService(typeof (ICreateObjectButton)) as ICreateObjectButton).ResetIcon();
    long objectDialog = service.CreateObjectDialog();
    switch (objectDialog)
    {
      case -1:
      case 0:
        return false;
      default:
        DBObjectsEventArgs e = new DBObjectsEventArgs("ObjectsCreated", objectDialog);
        (ServicesManager.GetService(typeof (INotificationService)) as INotificationService).FireEvent((object) null, (NotificationEventArgs) e);
        return true;
    }
  }

  public object NavWindow
  {
    [DebuggerStepThrough] get => this.navWindow;
    [DebuggerStepThrough] set => this.navWindow = value;
  }

  public object TreeView
  {
    [DebuggerStepThrough] get => this.treeView;
    [DebuggerStepThrough] set => this.treeView = value;
  }

  public object ViewsManagers
  {
    [DebuggerStepThrough] get => this.viewsManagers;
    [DebuggerStepThrough] set => this.viewsManagers = value;
  }

  public event Intermech.Navigator.Interfaces.SetDescriptorStatuses SetDescriptorStatuses;

  public void FireSetDescriptorStatuses(IDescriptorElementStatuses descriptor)
  {
    if (this.SetDescriptorStatuses == null || descriptor == null)
      return;
    this.SetDescriptorStatuses((object) this, new SetDescriptorStatusesEventArgs(descriptor));
  }

  public Guid Guid
  {
    [DebuggerStepThrough] get => Plugin._Guid;
  }

  public YesNoUnknownEnum EnableTreeMultiSelect(
    IDescriptor rootDescriptor,
    IServiceProvider viewServices)
  {
    return YesNoUnknownEnum.Unknown;
  }

  public YesNoUnknownEnum EnableTreeColumnsSorting(
    IDescriptor rootDescriptor,
    IServiceProvider viewServices)
  {
    return YesNoUnknownEnum.Unknown;
  }

  public YesNoUnknownEnum EnableTreeCollapse(
    IDescriptor rootDescriptor,
    IServiceProvider viewServices)
  {
    Intermech.Navigator.DBObjects.Descriptor descriptor = rootDescriptor as Intermech.Navigator.DBObjects.Descriptor;
    int num = -1;
    bool flag = false;
    if (descriptor == null)
      return YesNoUnknownEnum.Unknown;
    long objectId = descriptor.ObjectID;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(objectId);
      flag = objectInfo.Empty;
      num = objectInfo.ObjectTypeID;
    }
    if (flag)
      return YesNoUnknownEnum.No;
    return MetaDataHelper.GetObjectTypeChildrenIDRecursive(new Guid("cad0011b-306c-11d8-b4e9-00304f19f545")).Contains(num) ? YesNoUnknownEnum.Yes : YesNoUnknownEnum.Unknown;
  }

  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    if (items.Count != 1)
      return CommandsInfo.Empty;
    CommandsInfo groupCommands = new CommandsInfo();
    long viewState = viewServices.GetService(typeof (IViewState)) is IViewState service ? (long) service.ViewState : 0L;
    IDBTypedObjectID itemData1 = items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
    IDBRelationID itemData2 = items.GetItemData(0, typeof (IDBRelationID)) as IDBRelationID;
    if (itemData1 != null)
    {
      if (Utils.CreateFreeObject(itemData1.ObjectType))
        groupCommands.Add("Navigator.CreateObjectType", new CommandInfo(0, new ClickEventHandler(ObjectCommands.CreateCommandType)));
      else if (itemData2 != null && Utils.CreateFreeObject(itemData1.ObjectType))
        groupCommands.Add("Navigator.CreateObjectTypeInclude", new CommandInfo(0, new ClickEventHandler(ObjectCommands.CreateIncludeInParentCommand)));
    }
    return groupCommands;
  }

  public IOEventTypes SupportedEvents
  {
    [DebuggerStepThrough] get => IOEventTypes.evKeyUp;
    [DebuggerStepThrough] set
    {
    }
  }

  public bool ProcessEvent(IIOEvent Event) => false;

  private void ShowFavoritesClick(object sender, EventArgs e)
  {
    try
    {
      WellKnownNavWindow wellKnownNavWindow = (WellKnownNavWindow) ((IWellKnownNavigators) ServicesManager.GetService(typeof (IWellKnownNavigators))).Get("favoritesNavigator");
      if (wellKnownNavWindow == null)
      {
        DockManager service = (DockManager) ServicesManager.GetService(typeof (DockManager));
        DockControl dockControl = service.FindDockControl(Plugin._favoritesWindowGuid);
        if (dockControl != null)
        {
          dockControl.Activate();
          wellKnownNavWindow = service.FindDockControl(Plugin._favoritesWindowGuid) as WellKnownNavWindow;
        }
      }
      FavoritesRootNodeDescriptor favoritesDescriptor = this.GetFavoritesDescriptor() as FavoritesRootNodeDescriptor;
      if (wellKnownNavWindow == null)
        wellKnownNavWindow = this.CreateFavoritesWindow(favoritesDescriptor, string.Empty);
      wellKnownNavWindow.Show((DockManager) ServicesManager.GetService(typeof (DockManager)));
      wellKnownNavWindow.Activate();
    }
    catch (Exception ex)
    {
      int num = (int) MessageBox.Show(ex.Message, Plugin._errorCaption, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }
  }

  private IDescriptor GetFavoritesDescriptor()
  {
    ApplicationServices.Container.GetService(typeof (ICurrentUserAndRole));
    return (IDescriptor) new FavoritesRootNodeDescriptor();
  }

  private WellKnownNavWindow CreateFavoritesWindow(
    FavoritesRootNodeDescriptor rootDescriptor,
    string persistString)
  {
    WellKnownNavWindow favoritesWindow = (WellKnownNavWindow) new FavoritesWindow();
    favoritesWindow.WellKnownName = "favoritesNavigator";
    favoritesWindow.Text = Plugin._favoritesName;
    favoritesWindow.Guid = Plugin._favoritesWindowGuid;
    int num = ((INamedImageList) ServicesManager.GetService(typeof (INamedImageList))).ImageIndex("imgFavorites");
    favoritesWindow.RootDescriptor = (IDescriptor) rootDescriptor;
    if (num >= 0)
      favoritesWindow.TabImageIndex = num;
    string.IsNullOrEmpty(persistString);
    return favoritesWindow;
  }

  private void MenuTemplateNodeTransformEventHandler(
    object sender,
    MenuTemplateNodeTransformEventArgs e)
  {
    if (e.Items == null || e.Items.Count != 1 || e.MenuTemplateNode == null || !(e.MenuTemplateNode.Name == "Navigator.CreateObjectType") && !(e.MenuTemplateNode.Name == "Navigator.CreateObjectTypeInclude") || !(e.Items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData))
      return;
    IMSObjectType objectType = MetaDataHelper.GetObjectType(itemData.ObjectType);
    if (objectType == null || objectType.IsDisableManualCreate)
      return;
    if (e.MenuTemplateNode.Name == "Navigator.CreateObjectType")
      e.MenuTemplateNode.Text = string.Format(LocalizationHolder.rm.GetString("Navigator_13"), (object) objectType.ObjectName);
    else if (e.MenuTemplateNode.Name == "Navigator.CreateObjectTypeInclude")
      e.MenuTemplateNode.Text = string.Format(LocalizationHolder.rm.GetString("Navigator_14"), (object) objectType.ObjectName);
    int index = Plugin._categoryImages.IndexOf(4, objectType.ObjectTypeID);
    if (index >= 0)
      e.MenuTemplateNode.Image = Plugin._categoryImages.ImageList.Images[index];
    else
      e.MenuTemplateNode.Image = (Image) null;
  }

  private void NewObjectCreated(object sender, AfterObjectCreatedEventArgs e)
  {
    IFactory service = ServicesManager.GetService(typeof (IFactory)) as IFactory;
    MenuTemplate contextMenuTemplate = service.ContextMenuTemplate;
    try
    {
      contextMenuTemplate.BeginUpdate();
      MenuTemplateNode createInComposition = service.ContextMenuTemplate["CreateInComposition"];
      if (createInComposition == null)
        return;
      IMSObjectType objectType = MetaDataHelper.GetObjectType(e.ObjectTypeID);
      string str = "CreateTypeInComposition" + objectType.ObjectName;
      foreach (MenuTemplateNode node in createInComposition.Nodes)
      {
        if (node.Name == str)
          return;
      }
      Plugin.AddObjectTypeToMenu(createInComposition, objectType, 11);
    }
    finally
    {
      contextMenuTemplate.EndUpdate();
    }
  }

  private static void AddObjectTypeToMenu(
    MenuTemplateNode createInComposition,
    IMSObjectType imsObjectType,
    int indexInGroup)
  {
    int imageIndex = ServicesManager.GetService(typeof (ICategoryTypeIconService)) is ICategoryTypeIconService service ? service.IndexOf(4, imsObjectType.ObjectTypeID) : -1;
    MenuTemplateNode node = new MenuTemplateNode("CreateTypeInComposition" + imsObjectType.ObjectName, imsObjectType.ObjectName, imageIndex, 20, indexInGroup, Keys.None, true, ImageListSource.CategoryImageList);
    createInComposition.Nodes.Add(node);
  }

  private void GetCategoryTypeParentEventHandler(object sender, GetCategoryTypeParentEventArgs e)
  {
    if (e.Category != 4 && e.Category != 1)
      return;
    int objectTypeParentId = MetaDataHelper.GetObjectTypeParentID(e.Type);
    if (objectTypeParentId != -1)
    {
      e.ParentCategory = e.Category;
      e.ParentType = objectTypeParentId;
      e.ParentSuffix = e.Suffix;
      e.Processed = true;
    }
    else
    {
      if (e.Category != 4)
        return;
      e.ParentCategory = Consts.CategoryObjectTypes;
      e.ParentType = 0;
      e.ParentSuffix = string.Empty;
      e.Processed = true;
    }
  }

  private void NavigatorWindowCaptionEventHandler(object sender, NavigatorWindowCaptionEventArgs e)
  {
    if (e == null || e.RootDescriptor == null || UISettings.NavigatorWindowCaptionsMode == NavigatorWindowCaptionsMode.Default || !(e.RootDescriptor is Intermech.Navigator.DBObjects.Descriptor rootDescriptor) || rootDescriptor.Version <= 0L)
      return;
    e.ExtraText = $"[{rootDescriptor.Version}]";
    e.TextHint = string.Format(LocalizationHolder.rm.GetString("Navigator_15"), (object) e.Text, (object) rootDescriptor.Version);
  }

  private void ShowNavigatorClick(object sender, EventArgs e)
  {
    try
    {
      WellKnownNavWindow wellKnownNavWindow = (WellKnownNavWindow) ((IWellKnownNavigators) ServicesManager.GetService(typeof (IWellKnownNavigators))).Get("mainNavigator");
      if (wellKnownNavWindow == null)
      {
        DockManager service = (DockManager) ServicesManager.GetService(typeof (DockManager));
        DockControl dockControl = service.FindDockControl(WellKnownNavWindow._persistStateGuid);
        if (dockControl != null)
        {
          dockControl.Activate();
          wellKnownNavWindow = service.FindDockControl(WellKnownNavWindow._persistStateGuid) as WellKnownNavWindow;
        }
      }
      int num = ((INamedImageList) ServicesManager.GetService(typeof (INamedImageList))).ImageIndex("imgNavigator");
      if (wellKnownNavWindow == null)
      {
        wellKnownNavWindow = new WellKnownNavWindow();
        wellKnownNavWindow.WellKnownName = "mainNavigator";
        wellKnownNavWindow.Text = Plugin._navName;
        wellKnownNavWindow.TreeView.OnGetSupportedColumnsEventHandler += new GetSupportedColumnsEventHandler(Utils.GetNavigatorColumns);
        wellKnownNavWindow.TreeView.SetColumns(Utils.CaptionColumnOnly(NodeColumnSortOrder.Ascending));
        wellKnownNavWindow.TreeView.Build((IDescriptor) new Intermech.Navigator.GlobalNode.Descriptor());
      }
      if (num >= 0)
        wellKnownNavWindow.TabImageIndex = num;
      wellKnownNavWindow.Show((DockManager) ServicesManager.GetService(typeof (DockManager)));
      wellKnownNavWindow.Activate();
    }
    catch (Exception ex)
    {
      int num = (int) MessageBox.Show(ex.Message, Plugin._errorCaption, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }
  }

  private IDescriptor GetDesktopDescriptor()
  {
    return (IDescriptor) new DesktopNodeDescriptor(DesktopObjectNode.DesktopObjectID);
  }

  private void ShowDesktopClick(object sender, EventArgs e)
  {
    try
    {
      WellKnownNavWindow wellKnownNavWindow = (WellKnownNavWindow) ((IWellKnownNavigators) ServicesManager.GetService(typeof (IWellKnownNavigators))).Get("desktopNavigator");
      if (wellKnownNavWindow == null)
      {
        DockManager service = (DockManager) ServicesManager.GetService(typeof (DockManager));
        DockControl dockControl = service.FindDockControl(Plugin._desktopGuid);
        if (dockControl != null)
        {
          dockControl.Activate();
          wellKnownNavWindow = service.FindDockControl(Plugin._desktopGuid) as WellKnownNavWindow;
        }
      }
      DesktopNodeDescriptor desktopDescriptor = this.GetDesktopDescriptor() as DesktopNodeDescriptor;
      if (wellKnownNavWindow != null && (!(wellKnownNavWindow.RootDescriptor is DesktopNodeDescriptor rootDescriptor) || rootDescriptor.InvalidDescriptor || rootDescriptor.ObjectID != desktopDescriptor.ObjectID))
      {
        wellKnownNavWindow.Close();
        wellKnownNavWindow = (WellKnownNavWindow) null;
      }
      if (wellKnownNavWindow == null)
        wellKnownNavWindow = this.CreateDekstopWindow(desktopDescriptor, string.Empty);
      wellKnownNavWindow.Show((DockManager) ServicesManager.GetService(typeof (DockManager)));
      wellKnownNavWindow.Activate();
    }
    catch (Exception ex)
    {
      int num = (int) MessageBox.Show(ex.Message, Plugin._errorCaption, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }
  }

  private WellKnownNavWindow CreateDekstopWindow(
    DesktopNodeDescriptor rootDescriptor,
    string persistString)
  {
    WellKnownNavWindow dekstopWindow = new WellKnownNavWindow();
    dekstopWindow.WellKnownName = "desktopNavigator";
    dekstopWindow.Text = Plugin._desktopName;
    dekstopWindow.Guid = Plugin._desktopGuid;
    int num = ((INamedImageList) ServicesManager.GetService(typeof (INamedImageList))).ImageIndex("imgDesktopObjectType");
    dekstopWindow.TreeView.OnGetSupportedColumnsEventHandler += new GetSupportedColumnsEventHandler(Utils.GetNavigatorColumns);
    dekstopWindow.TreeView.SetColumns(Utils.CaptionAndStatesesColumns(NodeColumnSortOrder.Ascending));
    dekstopWindow.RootDescriptor = (IDescriptor) rootDescriptor;
    if (num >= 0)
      dekstopWindow.TabImageIndex = num;
    if (string.IsNullOrEmpty(persistString))
      return dekstopWindow;
    try
    {
      XmlDocument xmlDoc = new XmlDocument();
      xmlDoc.LoadXml(persistString);
      dekstopWindow.RestoreState(xmlDoc);
    }
    catch
    {
      dekstopWindow.WellKnownName = string.Empty;
      dekstopWindow.HideOnClose = false;
      dekstopWindow.Close();
      dekstopWindow.Dispose();
      dekstopWindow = (WellKnownNavWindow) null;
    }
    return dekstopWindow;
  }

  private void ShowRecentObjectsClick(object sender, EventArgs e)
  {
    try
    {
      RecentObjectsWindow recentObjectsWindow = (RecentObjectsWindow) ((IWellKnownNavigators) ServicesManager.GetService(typeof (IWellKnownNavigators))).Get("desktopRecentObjects");
      int num = ((INamedImageList) ServicesManager.GetService(typeof (INamedImageList))).ImageIndex("imgRecentObjects");
      if (recentObjectsWindow == null)
      {
        DockManager service = (DockManager) ServicesManager.GetService(typeof (DockManager));
        DockControl dockControl = service.FindDockControl(WellKnownNavWindow._persistStateGuid);
        if (dockControl != null)
        {
          dockControl.Activate();
          recentObjectsWindow = service.FindDockControl(WellKnownNavWindow._persistStateGuid) as RecentObjectsWindow;
        }
      }
      if (recentObjectsWindow == null)
      {
        IDescriptor rootDescriptor = (IDescriptor) new CurrentUserRecentObjectsDescriptor();
        recentObjectsWindow = new RecentObjectsWindow();
        recentObjectsWindow.WellKnownName = "desktopRecentObjects";
        recentObjectsWindow.Text = Plugin._recentObjectsName;
        recentObjectsWindow.TreeView.SupportedColumns = Utils.DefaultSupportedColumnsObjects();
        recentObjectsWindow.TreeView.OnGetSupportedColumnsEventHandler += new GetSupportedColumnsEventHandler(Utils.GetObjectsColumns);
        recentObjectsWindow.TreeView.SetColumns(Utils.CaptionColumnOnly(NodeColumnSortOrder.Ascending));
        recentObjectsWindow.TreeView.Build(rootDescriptor);
      }
      if (num >= 0)
        recentObjectsWindow.TabImageIndex = num;
      recentObjectsWindow.Show((DockManager) ServicesManager.GetService(typeof (DockManager)));
      recentObjectsWindow.Activate();
    }
    catch (Exception ex)
    {
      int num = (int) MessageBox.Show(ex.Message, Plugin._errorCaption, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }
  }

  private void ShowAutosortRulesEditorClick(object sender, EventArgs e)
  {
    try
    {
      IWellKnownNavigators service = (IWellKnownNavigators) ServicesManager.GetService(typeof (IWellKnownNavigators));
      if (!(service.Get("desktopAutosortWindow") is CompositionsAutosortRulesWindow window))
      {
        window = new CompositionsAutosortRulesWindow();
        service.Register("desktopAutosortWindow", (Control) window);
      }
      window.Show((DockManager) ServicesManager.GetService(typeof (DockManager)));
      window.Activate();
    }
    catch (Exception ex)
    {
      int num = (int) MessageBox.Show(ex.Message, Plugin._errorCaption, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }
  }

  private void ActivateViewEventHandler(object sender, ActivateViewEventArgs e)
  {
    if (Plugin._objtypeSelections == -1)
    {
      Plugin._objtypeSelections = MetaDataHelper.GetObjectTypeID("cad00156-306c-11d8-b4e9-00304f19f545");
      Plugin._objtypeClassifyers = MetaDataHelper.GetObjectTypeID("cad00157-306c-11d8-b4e9-00304f19f545");
    }
    if (e == null || e.NewSelectedNodes == null || e.NewSelectedNodes.Count == 0)
      return;
    int categoryId1 = e.OldSelectedNodes == null || e.OldSelectedNodes.Count <= 0 ? 0 : e.OldSelectedNodes[0].CategoryID;
    int typeId1 = e.OldSelectedNodes == null || e.OldSelectedNodes.Count <= 0 ? 0 : e.OldSelectedNodes[0].TypeID;
    int categoryId2 = e.NewSelectedNodes == null || e.NewSelectedNodes.Count <= 0 ? 0 : e.NewSelectedNodes[0].CategoryID;
    int typeId2 = e.NewSelectedNodes == null || e.NewSelectedNodes.Count <= 0 ? 0 : e.NewSelectedNodes[0].TypeID;
    bool flag = e.OldSelectedNodes != null && e.OldSelectedNodes.Count > 0 && categoryId1 == 1 && (MetaDataHelper.IsObjectTypeChildOf(typeId1, Plugin._objtypeSelections) || MetaDataHelper.IsObjectTypeChildOf(typeId1, Plugin._objtypeClassifyers));
    int num = categoryId2 != 1 ? 0 : (MetaDataHelper.IsObjectTypeChildOf(typeId2, Plugin._objtypeSelections) ? 1 : (MetaDataHelper.IsObjectTypeChildOf(typeId2, Plugin._objtypeClassifyers) ? 1 : 0));
    IFoldersView currActiveView = e.CurrActiveView as IFoldersView;
    if (num == 0)
      return;
    if (currActiveView != null)
    {
      if (currActiveView.RemainActiveView)
        return;
      e.NewViewName = "ChildrenView";
    }
    else
    {
      if (flag)
        return;
      e.NewViewName = "ChildrenView";
    }
  }

  private void MetaDataHelper_OnCacheReloaded(object sender, EventArgs e)
  {
    Helper.ClearNodeColumnsCache();
  }

  internal DockControl ContentCallback(Guid guid, string persistString)
  {
    IWellKnownNavigators service = (IWellKnownNavigators) ServicesManager.GetService(typeof (IWellKnownNavigators));
    if (guid == WellKnownNavWindow._persistStateGuid)
      return (DockControl) service.Get("desktopRecentObjects");
    if (guid == WellKnownNavWindow._persistStateGuid)
      return (DockControl) service.Get("mainNavigator");
    if (guid == Plugin._desktopGuid)
      return (DockControl) service.Get("desktopNavigator") ?? (DockControl) this.CreateDekstopWindow(this.GetDesktopDescriptor() as DesktopNodeDescriptor, persistString);
    return guid == Plugin._favoritesWindowGuid ? (DockControl) service.Get("favoritesNavigator") ?? (DockControl) this.CreateFavoritesWindow(this.GetFavoritesDescriptor() as FavoritesRootNodeDescriptor, persistString) : (DockControl) null;
  }
}
