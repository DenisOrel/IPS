// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.MRP2ClientPlugin
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Bars;
using Intermech.ECO.Client;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Configuration;
using Intermech.Interfaces.MRP;
using Intermech.Interfaces.Plugins;
using Intermech.MRP2.Commands;
using Intermech.MRP2.Menu;
using Intermech.MRP2.Resources;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using Intermech.Protection;
using System;
using System.Drawing;

#nullable disable
namespace Intermech.MRP2;

public class MRP2ClientPlugin : IPackage, IConfigurable, ICommandTarget
{
  private static HideDeletedPluginTransfer _ClientPluginsDataTransfer;
  private static IClientPluginsService _clientPluginsService;
  private ICommandManager _commandManager;
  private static bool _isUserAdmin;

  public string Name => "Модуль для работы с производственными ведомостями";

  public void Load(IServiceProvider serviceProvider)
  {
    int appId = 347;
    byte[][] numArray1 = new byte[32 /*0x20*/][]
    {
      new byte[16 /*0x10*/]
      {
        (byte) 180,
        (byte) 99,
        (byte) 173,
        byte.MaxValue,
        (byte) 31 /*0x1F*/,
        (byte) 88,
        (byte) 220,
        (byte) 66,
        (byte) 67,
        (byte) 54,
        (byte) 244,
        (byte) 209,
        (byte) 236,
        (byte) 230,
        (byte) 17,
        (byte) 206
      },
      new byte[16 /*0x10*/]
      {
        (byte) 103,
        (byte) 163,
        (byte) 117,
        (byte) 100,
        (byte) 186,
        (byte) 69,
        (byte) 133,
        (byte) 28,
        (byte) 171,
        (byte) 0,
        (byte) 233,
        (byte) 19,
        (byte) 177,
        (byte) 242,
        (byte) 22,
        (byte) 229
      },
      new byte[16 /*0x10*/]
      {
        (byte) 77,
        (byte) 37,
        (byte) 166,
        (byte) 3,
        (byte) 42,
        (byte) 152,
        (byte) 43,
        (byte) 200,
        (byte) 92,
        (byte) 173,
        (byte) 89,
        (byte) 237,
        (byte) 86,
        (byte) 167,
        (byte) 47,
        (byte) 151
      },
      new byte[16 /*0x10*/]
      {
        (byte) 228,
        (byte) 20,
        (byte) 17,
        (byte) 199,
        (byte) 221,
        (byte) 28,
        (byte) 32 /*0x20*/,
        (byte) 15,
        (byte) 11,
        (byte) 68,
        (byte) 22,
        (byte) 61,
        (byte) 134,
        (byte) 77,
        (byte) 202,
        (byte) 151
      },
      new byte[16 /*0x10*/]
      {
        (byte) 224 /*0xE0*/,
        (byte) 208 /*0xD0*/,
        (byte) 16 /*0x10*/,
        (byte) 112 /*0x70*/,
        (byte) 138,
        (byte) 34,
        (byte) 145,
        (byte) 184,
        (byte) 220,
        (byte) 31 /*0x1F*/,
        (byte) 170,
        (byte) 244,
        (byte) 91,
        (byte) 45,
        (byte) 14,
        (byte) 125
      },
      new byte[16 /*0x10*/]
      {
        (byte) 206,
        (byte) 78,
        (byte) 161,
        (byte) 1,
        (byte) 130,
        (byte) 149,
        (byte) 140,
        (byte) 48 /*0x30*/,
        (byte) 100,
        (byte) 47,
        (byte) 135,
        (byte) 81,
        (byte) 79,
        (byte) 10,
        (byte) 59,
        (byte) 101
      },
      new byte[16 /*0x10*/]
      {
        (byte) 113,
        (byte) 206,
        (byte) 133,
        (byte) 150,
        (byte) 240 /*0xF0*/,
        (byte) 189,
        (byte) 25,
        (byte) 27,
        (byte) 56,
        (byte) 229,
        (byte) 162,
        (byte) 113,
        (byte) 254,
        (byte) 238,
        (byte) 235,
        (byte) 156
      },
      new byte[16 /*0x10*/]
      {
        (byte) 91,
        (byte) 50,
        (byte) 63 /*0x3F*/,
        (byte) 121,
        (byte) 203,
        (byte) 179,
        (byte) 102,
        (byte) 196,
        (byte) 122,
        (byte) 65,
        (byte) 128 /*0x80*/,
        (byte) 159,
        (byte) 136,
        (byte) 86,
        (byte) 35,
        (byte) 48 /*0x30*/
      },
      new byte[16 /*0x10*/]
      {
        (byte) 7,
        (byte) 92,
        (byte) 241,
        (byte) 27,
        (byte) 214,
        (byte) 161,
        (byte) 68,
        (byte) 42,
        (byte) 147,
        (byte) 238,
        (byte) 38,
        (byte) 10,
        (byte) 238,
        (byte) 55,
        (byte) 108,
        (byte) 47
      },
      new byte[16 /*0x10*/]
      {
        (byte) 63 /*0x3F*/,
        (byte) 27,
        (byte) 231,
        (byte) 171,
        (byte) 187,
        (byte) 203,
        (byte) 50,
        (byte) 166,
        (byte) 137,
        (byte) 52,
        (byte) 43,
        (byte) 224 /*0xE0*/,
        (byte) 17,
        (byte) 122,
        (byte) 48 /*0x30*/,
        (byte) 13
      },
      new byte[16 /*0x10*/]
      {
        (byte) 159,
        (byte) 220,
        (byte) 124,
        (byte) 135,
        (byte) 33,
        (byte) 153,
        (byte) 167,
        (byte) 35,
        (byte) 182,
        (byte) 244,
        (byte) 55,
        (byte) 164,
        (byte) 127 /*0x7F*/,
        (byte) 31 /*0x1F*/,
        (byte) 185,
        (byte) 238
      },
      new byte[16 /*0x10*/]
      {
        (byte) 218,
        (byte) 86,
        (byte) 14,
        (byte) 210,
        (byte) 249,
        (byte) 34,
        (byte) 211,
        (byte) 233,
        (byte) 240 /*0xF0*/,
        (byte) 205,
        (byte) 21,
        (byte) 26,
        (byte) 103,
        (byte) 241,
        (byte) 229,
        (byte) 86
      },
      new byte[16 /*0x10*/]
      {
        (byte) 52,
        (byte) 14,
        (byte) 236,
        byte.MaxValue,
        (byte) 60,
        (byte) 242,
        (byte) 166,
        (byte) 208 /*0xD0*/,
        (byte) 138,
        (byte) 90,
        (byte) 2,
        (byte) 2,
        (byte) 150,
        (byte) 14,
        (byte) 44,
        (byte) 0
      },
      new byte[16 /*0x10*/]
      {
        (byte) 127 /*0x7F*/,
        (byte) 90,
        (byte) 81,
        (byte) 30,
        (byte) 219,
        (byte) 252,
        (byte) 240 /*0xF0*/,
        (byte) 193,
        (byte) 203,
        (byte) 79,
        (byte) 195,
        (byte) 190,
        (byte) 27,
        (byte) 144 /*0x90*/,
        (byte) 143,
        (byte) 101
      },
      new byte[16 /*0x10*/]
      {
        (byte) 121,
        (byte) 115,
        (byte) 210,
        (byte) 34,
        (byte) 113,
        (byte) 99,
        (byte) 122,
        (byte) 110,
        (byte) 181,
        (byte) 205,
        (byte) 101,
        (byte) 225,
        (byte) 105,
        (byte) 160 /*0xA0*/,
        (byte) 170,
        (byte) 116
      },
      new byte[16 /*0x10*/]
      {
        (byte) 202,
        (byte) 184,
        (byte) 50,
        (byte) 181,
        (byte) 34,
        (byte) 178,
        (byte) 46,
        (byte) 42,
        (byte) 5,
        (byte) 80 /*0x50*/,
        (byte) 236,
        (byte) 119,
        (byte) 102,
        (byte) 86,
        (byte) 167,
        (byte) 222
      },
      new byte[16 /*0x10*/]
      {
        (byte) 52,
        (byte) 56,
        (byte) 214,
        (byte) 224 /*0xE0*/,
        (byte) 74,
        (byte) 109,
        (byte) 243,
        (byte) 172,
        (byte) 53,
        (byte) 172,
        (byte) 253,
        (byte) 199,
        (byte) 150,
        (byte) 164,
        (byte) 140,
        (byte) 168
      },
      new byte[16 /*0x10*/]
      {
        (byte) 41,
        (byte) 250,
        (byte) 23,
        (byte) 20,
        (byte) 234,
        (byte) 58,
        (byte) 251,
        (byte) 187,
        (byte) 237,
        (byte) 166,
        (byte) 163,
        (byte) 206,
        (byte) 249,
        (byte) 22,
        (byte) 200,
        (byte) 212
      },
      new byte[16 /*0x10*/]
      {
        (byte) 199,
        (byte) 55,
        (byte) 43,
        (byte) 12,
        (byte) 76,
        (byte) 150,
        (byte) 124,
        (byte) 208 /*0xD0*/,
        (byte) 147,
        (byte) 126,
        (byte) 73,
        (byte) 110,
        (byte) 141,
        (byte) 197,
        (byte) 103,
        (byte) 233
      },
      new byte[16 /*0x10*/]
      {
        (byte) 22,
        (byte) 71,
        (byte) 233,
        (byte) 95,
        (byte) 68,
        (byte) 107,
        (byte) 172,
        (byte) 227,
        (byte) 42,
        (byte) 78,
        (byte) 238,
        (byte) 95,
        (byte) 127 /*0x7F*/,
        (byte) 147,
        (byte) 123,
        (byte) 40
      },
      new byte[16 /*0x10*/]
      {
        (byte) 176 /*0xB0*/,
        (byte) 141,
        (byte) 205,
        (byte) 93,
        (byte) 55,
        (byte) 76,
        (byte) 182,
        (byte) 11,
        (byte) 127 /*0x7F*/,
        (byte) 38,
        (byte) 237,
        (byte) 197,
        (byte) 102,
        (byte) 179,
        (byte) 215,
        (byte) 17
      },
      new byte[16 /*0x10*/]
      {
        (byte) 58,
        (byte) 186,
        (byte) 102,
        (byte) 126,
        (byte) 2,
        (byte) 197,
        (byte) 33,
        (byte) 166,
        (byte) 185,
        (byte) 53,
        (byte) 253,
        (byte) 179,
        (byte) 92,
        (byte) 70,
        (byte) 167,
        (byte) 17
      },
      new byte[16 /*0x10*/]
      {
        (byte) 64 /*0x40*/,
        (byte) 188,
        (byte) 25,
        (byte) 49,
        (byte) 116,
        (byte) 157,
        (byte) 39,
        (byte) 158,
        (byte) 188,
        (byte) 6,
        (byte) 76,
        (byte) 203,
        (byte) 35,
        (byte) 132,
        (byte) 30,
        (byte) 112 /*0x70*/
      },
      new byte[16 /*0x10*/]
      {
        (byte) 224 /*0xE0*/,
        (byte) 165,
        (byte) 252,
        (byte) 184,
        (byte) 197,
        (byte) 228,
        (byte) 241,
        (byte) 152,
        (byte) 241,
        (byte) 34,
        (byte) 30,
        (byte) 221,
        (byte) 137,
        (byte) 93,
        (byte) 136,
        (byte) 138
      },
      new byte[16 /*0x10*/]
      {
        (byte) 69,
        (byte) 19,
        (byte) 23,
        (byte) 9,
        (byte) 214,
        (byte) 46,
        (byte) 112 /*0x70*/,
        (byte) 123,
        (byte) 129,
        (byte) 31 /*0x1F*/,
        (byte) 140,
        (byte) 38,
        (byte) 169,
        (byte) 182,
        (byte) 105,
        (byte) 224 /*0xE0*/
      },
      new byte[16 /*0x10*/]
      {
        (byte) 39,
        (byte) 170,
        (byte) 156,
        (byte) 56,
        (byte) 41,
        (byte) 123,
        (byte) 232,
        (byte) 210,
        (byte) 147,
        (byte) 189,
        (byte) 137,
        (byte) 215,
        (byte) 51,
        (byte) 187,
        (byte) 203,
        (byte) 103
      },
      new byte[16 /*0x10*/]
      {
        (byte) 167,
        (byte) 56,
        (byte) 45,
        (byte) 208 /*0xD0*/,
        (byte) 171,
        (byte) 53,
        (byte) 116,
        (byte) 18,
        (byte) 101,
        (byte) 47,
        (byte) 161,
        (byte) 49,
        (byte) 0,
        (byte) 234,
        (byte) 232,
        (byte) 251
      },
      new byte[16 /*0x10*/]
      {
        (byte) 45,
        (byte) 55,
        (byte) 66,
        (byte) 53,
        (byte) 108,
        (byte) 240 /*0xF0*/,
        (byte) 199,
        (byte) 25,
        (byte) 26,
        (byte) 246,
        (byte) 166,
        (byte) 35,
        (byte) 42,
        (byte) 28,
        (byte) 136,
        (byte) 93
      },
      new byte[16 /*0x10*/]
      {
        (byte) 7,
        (byte) 124,
        (byte) 128 /*0x80*/,
        (byte) 82,
        (byte) 155,
        (byte) 206,
        (byte) 58,
        (byte) 17,
        (byte) 76,
        (byte) 158,
        (byte) 176 /*0xB0*/,
        (byte) 85,
        (byte) 17,
        (byte) 1,
        (byte) 245,
        (byte) 5
      },
      new byte[16 /*0x10*/]
      {
        (byte) 111,
        (byte) 215,
        (byte) 158,
        (byte) 73,
        (byte) 77,
        (byte) 117,
        (byte) 43,
        (byte) 167,
        (byte) 220,
        (byte) 0,
        (byte) 100,
        (byte) 81,
        (byte) 97,
        (byte) 107,
        (byte) 1,
        (byte) 50
      },
      new byte[16 /*0x10*/]
      {
        (byte) 156,
        (byte) 108,
        (byte) 36,
        (byte) 234,
        (byte) 86,
        (byte) 77,
        (byte) 196,
        (byte) 15,
        (byte) 158,
        (byte) 5,
        (byte) 163,
        (byte) 166,
        (byte) 141,
        (byte) 62,
        (byte) 123,
        (byte) 151
      },
      new byte[16 /*0x10*/]
      {
        (byte) 96 /*0x60*/,
        (byte) 201,
        (byte) 55,
        (byte) 235,
        (byte) 79,
        (byte) 233,
        (byte) 138,
        (byte) 160 /*0xA0*/,
        (byte) 83,
        (byte) 90,
        (byte) 231,
        (byte) 176 /*0xB0*/,
        (byte) 176 /*0xB0*/,
        (byte) 190,
        (byte) 70,
        (byte) 5
      }
    };
    IProtectionKey service1 = serviceProvider.GetService(typeof (IProtectionKey)) as IProtectionKey;
    ((ILicenser) ServicesManager.GetService(typeof (ILicenser))).AllocateLicense(appId);
    if (service1 == null)
      return;
    int index1 = (Environment.TickCount & 15) * 2;
    byte[] queryData = numArray1[index1];
    byte[] numArray2 = numArray1[index1 + 1];
    byte[] response = new byte[numArray2.Length];
    service1.Query(true, appId, queryData, response);
    int length = queryData.Length;
    for (int index2 = 0; index2 < length; ++index2)
    {
      if ((int) numArray2[index2] != (int) response[index2])
        return;
    }
    IObjectCreatorService service2 = ServicesManager.GetService(typeof (IObjectCreatorService)) as IObjectCreatorService;
    if (-1 != MRP2Consts.objtypeIdProductionLists)
    {
      service2.RegisterCreatorCustomService(MRP2Consts.objtypeIdProductionLists, typeof (ProductionListCreator));
      this.InitializeCommands(serviceProvider);
    }
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      MRP2ClientPlugin._isUserAdmin = sessionKeeper.Session.IsAdmin;
      if (MRP2ClientPlugin._isUserAdmin)
      {
        MRP2PropertyPage page = new MRP2PropertyPage();
        IPropertyPagesService service3 = ServicesManager.GetService<IPropertyPagesService>();
        service3.AddPage("Система\\Подготовка производства (MRP)\\Настройки производственных ведомостей", (IPropertyPage) page);
        service3.AddPage("Система\\Подготовка производства (MRP)\\Настройки производственных ведомостей\\Настройка соответствия типов копий", (IPropertyPage) new MRP2ObjectSettings());
      }
      HideViewsCommand.Hide = sessionKeeper.Session.Configurations.ReadBool("MRP2", "MRP2", "HideViewsManager", false, DBConfigMode.UserAndGlobal);
      TechRouteFilter.InitFilterState(sessionKeeper.Session);
      ((INotificationService) ServicesManager.GetService(typeof (INotificationService))).Subscribe("ApplicationClosed", new NotificationEventHandler(this.OnApplicationClosed));
    }
  }

  private void OnApplicationClosed(object sender, NotificationEventArgs e)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      sessionKeeper.Session.Configurations.WriteBool("MRP2", "MRP2", "HideViewsManager", HideViewsCommand.Hide);
    if (MRP2ClientPlugin._clientPluginsService != null)
      MRP2ClientPlugin._clientPluginsService.UnregisterClientPlugin(MRP2ClientPlugin._ClientPluginsDataTransfer.PluginGuid);
    if (this._commandManager == null)
      return;
    this._commandManager.RemoveTarget((ICommandTarget) this);
  }

  private void InitializeCommands(IServiceProvider serviceProvider)
  {
    if (MRP2Consts.attrIdDeleteTag <= 0)
      return;
    IPluginManager service = (IPluginManager) serviceProvider.GetService(typeof (IPluginManager));
    if (service == null)
      return;
    if (service.IsLoadComplete)
      this.PluginManager_LoadComplete((object) null, (EventArgs) null);
    else
      service.LoadComplete += new EventHandler(this.PluginManager_LoadComplete);
  }

  private void InitCommandsProviders(IFactory factory, INamedImageList _images)
  {
    ProductionObjectsMenuProvider provider = new ProductionObjectsMenuProvider(factory, _images);
    factory.AddCommandsProvider(1, MRP2Consts.objtypeIdProductionObjects, (ICommandsProvider) provider);
    factory.AddCommandsProvider(1, MRP2Consts.objtypeIdProductionCopy, (ICommandsProvider) new ProductionCopyMenuProvider(factory, _images));
    factory.AddCommandsProvider(1, (ICommandsProvider) new ECOCommandProvider(factory));
    factory.AddCommandsProvider(1, MRP2Consts.objtypeIdProductionLists, (ICommandsProvider) new ProductionListMenuProvider());
    factory.AddCommandsProvider(1, MetaDataHelper.GetObjectTypeID("cad00132-306c-11d8-b4e9-00304f19f545"), (ICommandsProvider) new ArticlesCommandProvider(factory));
    factory.AddCommandsProvider(1, MRP2Consts.objtypeIdDocument, (ICommandsProvider) new DocumentsMenuProvider());
    factory.AddCommandsProvider(1, (ICommandsProvider) new FiltrationCompositionCommandProvider());
    if (!(ServicesManager.GetService(typeof (INotificationService)) is INotificationService service))
      return;
    service.Subscribe("NavigatorWindowOpening", new NotificationEventHandler(provider.OnNavigatorNewWindowOpening));
  }

  private void PluginManager_LoadComplete(object sender, EventArgs e)
  {
    ServicesManager.AddService(typeof (IProductionListReportService), (object) new ProductionListReportService());
    INamedImageList service1 = ServiceUtils.GetService<INamedImageList>((object) ServicesManager.ServiceContainer, false);
    service1.Add((Image) MRP2Resource.MRP2_HideDeletedBtn, "MRP2.HideDeleted");
    service1.Add((Image) MRP2Resource.MRP2_RecalcCounts, "MRP2.RecalcCounts");
    service1.Add((Image) MRP2Resource.MRP2_StartPLCheck, "MRP2.StartPLCheck");
    service1.Add((Image) MRP2Resource.MRP2_CreateProto, "MRP2.CreateProto");
    service1.Add((Image) MRP2Resource.MRP2_SelectPL, "MRP2.SelectPL");
    service1.Add((Image) MRP2Resource.MRP2_HideViews, "MRP2.HideViews");
    service1.Add((Image) MRP2Resource.MRP2_ReplacePart, "MRP2.ReplacePart");
    service1.Add((Image) MRP2Resource.MRP2_AddFromPL, "MRP2.AddFromPL");
    service1.Add((Image) MRP2Resource.MRP2_Add, "MRP2.Add");
    service1.Add((Image) MRP2Resource.MRP2_ExpandAll, "MRP2.ExpandAll");
    service1.Add((Image) MRP2Resource.MRP2_Collapse, "MRP2.Collapse");
    service1.Add((Image) MRP2Resource.MRP2_TechFilterBtn1, "MRP2.TechFilterDisabled");
    service1.Add((Image) MRP2Resource.MRP2_TechFilterBtn2, "MRP2.TechFilterEnabled");
    service1.Add((Image) MRP2Resource.MRP2_TechFilterBtn3, "MRP2.TechFilterDefault");
    IFactory service2 = ServicesManager.GetService(typeof (IFactory)) as IFactory;
    this.InitCommandsProviders(service2, service1);
    AdjustableViewsHelper.RegisterView("MRP2.ProductionListReportView", "Отчет о проверке ПВ", "", "Intermech.MRP2", "", true, 20);
    service2.AddViewsProvider(1, MRP2Consts.objtypeIdProductionLists, (IViewsProvider) new ProductionListViewProvider());
    AdjustableViewsHelper.RegisterView("MRP2.ProductionCopyEntersInView", "Применяемость в ПВ", "imgEntersTo", "Intermech.MRP2", "", true, 20);
    service2.AddViewsProvider(1, MRP2Consts.objtypeIdProductionCopy, (IViewsProvider) new ProductionCopyViewProvider());
    service2.AddViewsProvider(1, MetaDataHelper.GetObjectTypeID("cad00268-306c-11d8-b4e9-00304f19f545"), (IViewsProvider) new ArticlesInProductListProvider());
    MRP2ClientPlugin._ClientPluginsDataTransfer = new HideDeletedPluginTransfer();
    MRP2ClientPlugin._clientPluginsService = ServicesManager.GetService(typeof (IClientPluginsService)) as IClientPluginsService;
    MRP2ClientPlugin._clientPluginsService.RegisterClientPlugin(MRP2ClientPlugin._ClientPluginsDataTransfer.PluginGuid, (IClientPluginsDataTransfer) MRP2ClientPlugin._ClientPluginsDataTransfer);
    this._commandManager = ServicesManager.GetService<ICommandManager>();
    this._commandManager.AddTarget((ICommandTarget) this);
    ECOPlugin plugin = ECOPlugin.FindPlugin();
    if (plugin == null)
      return;
    plugin.DoSetPLForAll = new ECOPlugin.SetPLForAll(IndicateApplicabilityCommand.SetPLForAll);
  }

  public void Unload()
  {
    (ServicesManager.GetService(typeof (ILicenser)) as ILicenser).ReleaseLicense(347);
  }

  void IConfigurable.LoadConfiguration(IConfigurationManager configurationManager)
  {
  }

  void IConfigurable.SaveConfiguration(IConfigurationManager configurationManager)
  {
  }

  bool ICommandTarget.Execute(ICommandState commandState) => false;

  bool ICommandTarget.QueryStatus(ICommandState commandState)
  {
    switch (commandState.CommandName)
    {
      case "MRP2.AddFromPL":
        commandState.Enabled = false;
        NavigatorTreeView activeTarget = this._commandManager.ActiveTarget as NavigatorTreeView;
        return true;
      case "MRP2.ReplacePart":
        return true;
      case "MRP2.HideDeleted":
        commandState.Enabled = false;
        commandState.Checked = HideDeletePositionsCommand.checkedState;
        return true;
      case "MRP2.FilterByDateMenu":
        commandState.Checked = FilterDateAttributesCommand.FilterByDateInCompositionEnabled;
        return true;
      default:
        return false;
    }
  }
}
