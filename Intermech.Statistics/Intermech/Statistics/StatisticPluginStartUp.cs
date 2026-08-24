// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.StatisticPluginStartUp
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Bars;
using Intermech.Docking;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Plugins;
using Intermech.NavBars;
using Intermech.Protection;
using Intermech.Search;
using Intermech.Statistics.Interfaces;
using Intermech.Statistics.Properties;
using System;
using System.Drawing;

#nullable disable
namespace Intermech.Statistics;

public class StatisticPluginStartUp : IPackage
{
  private IServiceProvider _serviceProvider;
  private StatisticsMainForm _statisticsMainForm;

  public void Load(IServiceProvider serviceProvider)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IStatisticsService statisticsService = (IStatisticsService) null;
      try
      {
        statisticsService = (IStatisticsService) sessionKeeper.Session.GetCustomService(typeof (IStatisticsService));
      }
      catch
      {
      }
      if (statisticsService == null)
        throw new KernelException($"Невозможно загрузить плагин '{this.Name}': серверная часть модуля не загружена!");
      StatisticsConst.Init(sessionKeeper.Session);
      ((IPropertyPagesService) ServicesManager.GetService(typeof (IPropertyPagesService)))?.AddPage("Система\\Статистика", (IPropertyPage) new StatisticsPropertyPage(sessionKeeper.Session));
    }
    ApplicationServices.Container.RemoveService(typeof (IStatisticsClientService));
    ApplicationServices.Container.AddService(typeof (IStatisticsClientService), (object) new StatisticsClientService());
    if (!(serviceProvider.GetService(typeof (ILicenser)) is ILicenser service1))
      throw new ProtectionException("ILicenser not found");
    service1.AllocateLicense(356);
    if (!(serviceProvider.GetService(typeof (IPluginManager)) is IPluginManager service2))
      return;
    this._serviceProvider = serviceProvider;
    if (service2.IsLoadComplete)
    {
      this.RegisterMenus();
      this.RegisterObjectsCreator();
    }
    else
      service2.LoadComplete += new EventHandler(this.pluginManager_LoadComplete);
  }

  public void Unload()
  {
    ApplicationServices.Container.RemoveService(typeof (IStatisticsClientService));
    if (!(ServicesManager.GetService(typeof (IObjectCreatorService)) is IObjectCreatorService service))
      return;
    service.UnregisterCreatorCustomService(MetaDataHelper.GetObjectTypeID(StatisticsConst.StatisticsCommandTypeGuid), typeof (StatisticsObjectsCreatorService));
  }

  public string Name => "Модуль статистики IPS";

  private void pluginManager_LoadComplete(object sender, EventArgs e)
  {
    this.RegisterMenus();
    this.RegisterObjectsCreator();
    IContentProvider contentProvider = this._serviceProvider == null ? (IContentProvider) ServicesManager.GetService(typeof (IContentProvider)) : (IContentProvider) this._serviceProvider.GetService(typeof (IContentProvider));
    if (contentProvider == null)
      return;
    contentProvider.ContentCallback += new GetContentCallback(this.cp_ContentCallback);
  }

  public DockControl cp_ContentCallback(Guid guid, string persistString)
  {
    return guid == StatisticsConst.StatisticsDockControlGuid ? (DockControl) new StatisticsMainForm() : (DockControl) null;
  }

  private void RegisterObjectsCreator()
  {
    if (!(ServicesManager.GetService(typeof (IObjectCreatorService)) is IObjectCreatorService service))
      return;
    service.RegisterCreatorCustomService(MetaDataHelper.GetObjectTypeID(StatisticsConst.StatisticsCommandTypeGuid), typeof (StatisticsObjectsCreatorService));
  }

  private void RegisterMenus()
  {
    INamedImageList service1 = (INamedImageList) this._serviceProvider.GetService(typeof (INamedImageList));
    Bitmap statistics = Resources._statistics;
    if (statistics != null && service1 != null && service1.ImageIndex(StatisticsConst.ImageKeyName) == -1)
    {
      service1.Add((Image) statistics, StatisticsConst.ImageKeyName);
      statistics.Dispose();
    }
    int imageIndex = service1 == null ? -1 : service1.ImageIndex(StatisticsConst.ImageKeyName);
    if ((BarManager) this._serviceProvider.GetService(typeof (BarManager)) != null && this._serviceProvider.GetService(typeof (IMainMenuService)) is IMainMenuService service2)
    {
      MenuButtonItem menuButtonItem1 = new MenuButtonItem(StatisticsConst.ModuleName);
      menuButtonItem1.CommandName = StatisticsConst.CommandName;
      menuButtonItem1.ImageIndex = imageIndex;
      MenuButtonItem menuButtonItem2 = menuButtonItem1;
      menuButtonItem2.Click += new EventHandler(this.Statistics_Click);
      MenuButtonItem[] menuButtonItemArray = new MenuButtonItem[1]
      {
        menuButtonItem2
      };
      service2.RegisterMenuItems(MainMenuItemSite.Applications, MainMenuItemPosition.Default, menuButtonItemArray);
    }
    if (!(this._serviceProvider.GetService(typeof (INavigationBar)) is INavigationBar service3) || !(service3.FindPane("appPane") is IAppPane pane))
      return;
    pane.Add(StatisticsConst.ModuleName, new EventHandler(this.Statistics_Click), imageIndex);
  }

  private void Statistics_Click(object sender, EventArgs e)
  {
    DockManager service = (DockManager) this._serviceProvider.GetService(typeof (DockManager));
    StatisticsMainForm statisticsMainForm = new StatisticsMainForm();
    statisticsMainForm.Name = StatisticsConst.ModuleName;
    this._statisticsMainForm = statisticsMainForm;
    if (service == null)
      return;
    this._statisticsMainForm.Show(service);
    this._statisticsMainForm.Activate();
  }
}
