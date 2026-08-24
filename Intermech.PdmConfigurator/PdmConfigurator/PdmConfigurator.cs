// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.PdmConfigurator
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.Bars;
using Intermech.Docking;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Configuration;
using Intermech.Interfaces.Pdm;
using Intermech.Interfaces.PdmConfigurator;
using Intermech.Interfaces.Plugins;
using Intermech.Localization;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using Intermech.PdmConfigurator.CompositionTracing;
using Intermech.PdmConfigurator.Creator;
using Intermech.Search;
using Intermech.Search.Pdm.CompositionsConfigurator;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace Intermech.PdmConfigurator;

public class PdmConfigurator : IPackage, IConfigurable
{
  public static string categoryOptionsGroups = "cad00596-306c-11d8-b4e9-00304f19f545";
  public static int categoryOptionsGroupsID = 0;
  public const string CategoryAllCategoryOptionsNodeGuid = "{7F78301F-D7BB-4E85-ADA5-DAB876BCF417}";
  public static int CategoryAllCategoryOptionsNode = -1;
  public static string categoryCategories = "{17E94473-5E2C-4CA9-B9C2-1FCF8A7E6889}";
  public static int categoryCategoriesID = 0;
  public static string categoryCategoriesAndOptions = "{A34C3E9A-0131-4196-B4CE-DA0EE5BE3910}";
  public static int categoryCategoriesAndOptionsID = 0;
  internal static bool PluginLocked = false;
  private Guid _pluginGuid = new Guid("cad005f5-306c-11d8-b4e9-00304f19f545");
  private bool _IsInEvent;
  private CompositionTracingView tracingView;
  internal static INamedImageList _namedImageList = (INamedImageList) null;
  internal static ICategoryTypeIconService _objtypesIcons = (ICategoryTypeIconService) null;
  internal static IFactory _factory = (IFactory) null;
  internal static IPicturesCache _picturesCache = (IPicturesCache) null;
  internal static INotificationService _notificationService = (INotificationService) null;
  internal static IFiltrationService _filtrationService = (IFiltrationService) null;
  internal static IGuidMapper _guidMapper = (IGuidMapper) null;
  internal static DockManager _manager = (DockManager) null;
  internal static ICommandManager commandManager = (ICommandManager) null;
  private LazyService<IMainMenuService> _mainMenuService = new LazyService<IMainMenuService>();
  private CompositionsConfiguratorClientModule _compositionsConfiguratorClientModule = new CompositionsConfiguratorClientModule();

  private void BeforeAppExit(object sender, EventArgs e)
  {
    int num = Intermech.PdmConfigurator.PdmConfigurator.PluginLocked ? 1 : 0;
  }

  public void Load(IServiceProvider serviceProvider)
  {
    (serviceProvider.GetService(typeof (IPluginManager)) as IPluginManager).LoadComplete += new EventHandler(this.pluginManager_LoadComplete);
    IPdmConfiguratorServerPlugin configuratorServerPlugin1 = (IPdmConfiguratorServerPlugin) null;
    IPdmConfiguratorServerPlugin configuratorServerPlugin2;
    try
    {
      configuratorServerPlugin2 = (ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService).GetCustomService(typeof (IPdmConfiguratorServerPlugin)) as IPdmConfiguratorServerPlugin;
    }
    catch
    {
      configuratorServerPlugin2 = (IPdmConfiguratorServerPlugin) null;
    }
    Intermech.PdmConfigurator.PdmConfigurator.PluginLocked = configuratorServerPlugin2 == null;
    configuratorServerPlugin1 = (IPdmConfiguratorServerPlugin) null;
    if (Intermech.PdmConfigurator.PdmConfigurator.PluginLocked)
    {
      int num1 = (int) MessageBox.Show(Intermech.Interfaces.PdmConfigurator.Consts.Dialog2, Intermech.Interfaces.PdmConfigurator.Consts.Dialog1, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }
    else
    {
      IPDMSubstitutesService service1 = ServicesManager.GetService(typeof (IPDMSubstitutesService)) as IPDMSubstitutesService;
      ServiceLocator.Register<ICompositionsConfiguratorClientService>((ICompositionsConfiguratorClientService) new CompositionsConfiguratorClientService());
      this._compositionsConfiguratorClientModule.Load();
      Intermech.PdmConfigurator.PdmConfigurator.PluginLocked = service1 == null;
      if (Intermech.PdmConfigurator.PdmConfigurator.PluginLocked)
      {
        int num2 = (int) MessageBox.Show(LocalizationHolder.rm.GetString("PdmConfigurator_97"), LocalizationHolder.rm.GetString("PdmConfigurator_98"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        Intermech.PdmConfigurator.PdmConfigurator._namedImageList = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
        Intermech.PdmConfigurator.PdmConfigurator._factory = ServicesManager.GetService(typeof (IFactory)) as IFactory;
        Intermech.PdmConfigurator.PdmConfigurator._notificationService = ServicesManager.GetService(typeof (INotificationService)) as INotificationService;
        Intermech.PdmConfigurator.PdmConfigurator._picturesCache = ServicesManager.GetService(typeof (IPicturesCache)) as IPicturesCache;
        Intermech.PdmConfigurator.PdmConfigurator._objtypesIcons = ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
        Intermech.PdmConfigurator.PdmConfigurator._filtrationService = ServicesManager.GetService(typeof (IFiltrationService)) as IFiltrationService;
        Intermech.PdmConfigurator.PdmConfigurator._guidMapper = ServicesManager.GetService(typeof (IGuidMapper)) as IGuidMapper;
        Intermech.PdmConfigurator.PdmConfigurator._manager = ServicesManager.GetService(typeof (DockManager)) as DockManager;
        this.LoadPluginResources(serviceProvider);
        this.LoadPluginControls(serviceProvider);
        IObjectCreatorService service2 = ServicesManager.GetService(typeof (IObjectCreatorService)) as IObjectCreatorService;
        service2.RegisterCreatorCustomService(MetaDataHelper.GetObjectTypeID("cad00580-306c-11d8-b4e9-00304f19f545"), typeof (OrderCreator));
        service2.RegisterCreatorCustomService(MetaDataHelper.GetObjectTypeID("cad015b1-306c-11d8-b4e9-00304f19f545"), typeof (OrderCreator));
        Application.ApplicationExit += new EventHandler(this.BeforeAppExit);
      }
    }
  }

  private void pluginManager_LoadComplete(object sender, EventArgs e)
  {
    PdmConfiguratorTarget target = new PdmConfiguratorTarget();
    Intermech.PdmConfigurator.PdmConfigurator.commandManager.AddTarget((ICommandTarget) target);
  }

  public void Unload()
  {
  }

  public string Name => Intermech.Interfaces.PdmConfigurator.Consts.PDMPluginName;

  public void LoadConfiguration(IConfigurationManager configurationManager)
  {
  }

  public void SaveConfiguration(IConfigurationManager configurationManager)
  {
  }

  private void LoadPluginResources(IServiceProvider serviceProvider)
  {
    if (Intermech.PdmConfigurator.PdmConfigurator.PluginLocked || Intermech.PdmConfigurator.PdmConfigurator._namedImageList == null)
      return;
    Stream manifestResourceStream = this.GetType().Assembly.GetManifestResourceStream("Intermech.PdmConfigurator.Resources.Options.bmp");
    if (manifestResourceStream == null)
      return;
    using (Bitmap images = new Bitmap(manifestResourceStream))
    {
      images.MakeTransparent();
      Intermech.PdmConfigurator.PdmConfigurator._namedImageList.AddStrip((Image) images, new string[2]
      {
        "imgPdmConfigurator.Options",
        "imgPdmConfigurator.Configurator"
      });
    }
    manifestResourceStream.Close();
  }

  private void LoadPluginControls(IServiceProvider serviceProvider)
  {
    if (Intermech.PdmConfigurator.PdmConfigurator.PluginLocked || serviceProvider == null)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      ServicesManager.AddService(typeof (PdmConfiguratorContextsCache), (object) new PdmConfiguratorContextsCache(sessionKeeper.Session.UserID));
      Intermech.Interfaces.PdmConfigurator.Consts.Initialize();
      Intermech.Interfaces.PdmConfigurator.Consts.Initialize(sessionKeeper.Session);
      PdmConfiguratorCache.CacheLoadCategories(sessionKeeper.Session);
      PdmConfiguratorCache.CacheLoadOptions(sessionKeeper.Session);
      Intermech.PdmConfigurator.PdmConfigurator.categoryOptionsGroupsID = Intermech.PdmConfigurator.PdmConfigurator._guidMapper.Register(new Guid(Intermech.PdmConfigurator.PdmConfigurator.categoryOptionsGroups));
      Intermech.PdmConfigurator.PdmConfigurator.CategoryAllCategoryOptionsNode = Intermech.PdmConfigurator.PdmConfigurator._guidMapper.Register(new Guid("{7F78301F-D7BB-4E85-ADA5-DAB876BCF417}"));
      Intermech.PdmConfigurator.PdmConfigurator.categoryCategoriesID = Intermech.PdmConfigurator.PdmConfigurator._guidMapper.Register(new Guid(Intermech.PdmConfigurator.PdmConfigurator.categoryCategories));
      Intermech.PdmConfigurator.PdmConfigurator.categoryCategoriesAndOptionsID = Intermech.PdmConfigurator.PdmConfigurator._guidMapper.Register(new Guid(Intermech.PdmConfigurator.PdmConfigurator.categoryCategoriesAndOptions));
      Image image = Intermech.PdmConfigurator.PdmConfigurator._namedImageList.ImageList.Images[Intermech.PdmConfigurator.PdmConfigurator._namedImageList.ImageIndex("imgPdmConfigurator.Options")];
      if (image != null)
      {
        using (Icon icon = ImageHelper.BitmapToIcon(image as Bitmap))
        {
          Intermech.PdmConfigurator.PdmConfigurator._objtypesIcons.AddIcon(icon, Intermech.PdmConfigurator.PdmConfigurator.categoryOptionsGroupsID, 0);
          Intermech.PdmConfigurator.PdmConfigurator._objtypesIcons.AddIcon(icon, Intermech.PdmConfigurator.PdmConfigurator.CategoryAllCategoryOptionsNode, 0);
          Intermech.PdmConfigurator.PdmConfigurator._objtypesIcons.AddIcon(icon, Intermech.PdmConfigurator.PdmConfigurator.categoryCategoriesID, 0);
          Intermech.PdmConfigurator.PdmConfigurator._objtypesIcons.AddIcon(icon, Intermech.PdmConfigurator.PdmConfigurator.categoryCategoriesAndOptionsID, 0);
        }
      }
      Intermech.PdmConfigurator.PdmConfigurator._factory.AddNodeType(Intermech.PdmConfigurator.PdmConfigurator.categoryOptionsGroupsID, typeof (TopObjectsNode));
      Intermech.PdmConfigurator.PdmConfigurator._factory.AddNodeType(1, Intermech.Interfaces.PdmConfigurator.Consts.objtypeOptionsGroupID, typeof (PdmCategoryObjectNode));
      Intermech.PdmConfigurator.PdmConfigurator._factory.AddNodeType(1, Intermech.Interfaces.PdmConfigurator.Consts.objtypeOptionID, typeof (PdmOptionObjectNode));
      Intermech.PdmConfigurator.PdmConfigurator._factory.AddViewsProvider(1, (IViewsProvider) new OptionEditorViewProvider());
      Intermech.PdmConfigurator.PdmConfigurator._factory.AddViewsProvider(1, (IViewsProvider) new ObjectOptionsEditorViewProvider());
      Intermech.PdmConfigurator.PdmConfigurator._factory.AddViewsProvider(1, (IViewsProvider) new RelationOptionsEditorViewProvider());
      Intermech.PdmConfigurator.PdmConfigurator._factory.AddViewsProvider(1, Intermech.Interfaces.PdmConfigurator.Consts.objtypeOptionsGroupID, (IViewsProvider) new PdmCategoryOptionsViewsProvider());
      Intermech.PdmConfigurator.PdmConfigurator._factory.AddViewsProvider(1, Intermech.Interfaces.PdmConfigurator.Consts.objtypeOptionID, (IViewsProvider) new PdmOptionsViewsProvider());
      Intermech.PdmConfigurator.PdmConfigurator.commandManager = serviceProvider.GetService(typeof (ICommandManager)) as ICommandManager;
      MenuButtonItem menuButtonItem = new MenuButtonItem(LocalizationHolder.rm.GetString("PdmConfigurator_99"), new EventHandler(this.CompositionItemClick));
      menuButtonItem.BeginGroup = true;
      menuButtonItem.CommandName = "CompositionTracing";
      Intermech.PdmConfigurator.PdmConfigurator.commandManager.Add((ButtonItemBase) menuButtonItem);
      this._mainMenuService.Value.RegisterMenuItemsGroup(MainMenuItemSite.Composition, MainMenuItemPosition.Default, false, menuButtonItem);
      if (serviceProvider.GetService(typeof (IContentProvider)) is IContentProvider service)
        service.ContentCallback += new GetContentCallback(this.RestoreTracingView);
      MenuTemplate contextMenuTemplate = Intermech.PdmConfigurator.PdmConfigurator._factory.ContextMenuTemplate;
      MenuTemplateNode menuTemplateNode = Intermech.PdmConfigurator.PdmConfigurator._factory.ContextMenuTemplate["Create"];
      if (menuTemplateNode != null)
      {
        contextMenuTemplate.BeginUpdate();
        try
        {
          int imageIndex1 = Intermech.PdmConfigurator.PdmConfigurator._objtypesIcons.IndexOf(4, MetaDataHelper.GetObjectTypeID("cad00580-306c-11d8-b4e9-00304f19f545"));
          menuTemplateNode.Nodes.Add(new MenuTemplateNode("CreateOrder", "Заказ", imageIndex1, 15, 10, Keys.None, true, ImageListSource.CategoryImageList));
          int imageIndex2 = Intermech.PdmConfigurator.PdmConfigurator._objtypesIcons.IndexOf(4, MetaDataHelper.GetObjectTypeID("cad015b1-306c-11d8-b4e9-00304f19f545"));
          menuTemplateNode.Nodes.Add(new MenuTemplateNode("CreateComplements", "Комплектацию", imageIndex2, 15, 20, Keys.None, true, ImageListSource.CategoryImageList));
          Intermech.PdmConfigurator.PdmConfigurator._factory.ContextMenuTemplate.Nodes.Add(new MenuTemplateNode("CompositionsConfigurator", "Конфигуратор составов", -1, -1, -1)
          {
            Nodes = {
              new MenuTemplateNode("CompositionsConfigurator.CopyApplicationConditions", "Копировать условия применения", -1, -1, -1),
              new MenuTemplateNode("CompositionsConfigurator.PasteApplicationConditions", "Вставить условия применения", -1, -1, -1),
              new MenuTemplateNode("CompositionsConfigurator.PasteApplicationConditionsToAllInstances", "Вставить условия применения во все исполнения", -1, -1, -1)
            }
          });
        }
        finally
        {
          contextMenuTemplate.EndUpdate();
        }
      }
      Intermech.PdmConfigurator.PdmConfigurator._factory.AddCommandsProvider(1, (ICommandsProvider) new OrderCreatorProvider());
      Intermech.PdmConfigurator.PdmConfigurator._factory.AddCommandsProvider((ICommandsProvider) new CompositionsConfiguratorCommandsProvider());
      (ServicesManager.GetService(typeof (IAdditionalCompositionFiltrationService)) as IAdditionalCompositionFiltrationService).GetCompositionFiltrationCommand += new GetCompositionFiltrationCommandEventHandler(this.AddFiltrationService_GetCompositionFiltrationCommand);
    }
  }

  private ICompositionFiltrationCommand AddFiltrationService_GetCompositionFiltrationCommand(
    object sender,
    GetCompositionFiltrationCommandEventArgs e)
  {
    return (ICompositionFiltrationCommand) new ConfiguratorCommand(e.Filtration);
  }

  private void CompositionItemClick(object sender, EventArgs e)
  {
    if (this.tracingView == null)
      this.tracingView = new CompositionTracingView();
    this.tracingView.Show(Intermech.PdmConfigurator.PdmConfigurator._manager);
    this.tracingView.Activate();
  }

  public DockControl RestoreTracingView(Guid guid, string persistString)
  {
    if (!guid.Equals(CompositionTracingView.ViewGuid))
      return (DockControl) null;
    if (this.tracingView == null)
      this.tracingView = new CompositionTracingView();
    this.tracingView.Show(Intermech.PdmConfigurator.PdmConfigurator._manager);
    this.tracingView.Activate();
    return (DockControl) this.tracingView;
  }
}
