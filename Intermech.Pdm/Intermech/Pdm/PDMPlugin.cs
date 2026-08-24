// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.PDMPlugin
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using ImSSP;
using Intermech.Bars;
using Intermech.Client.Core;
using Intermech.Controls;
using Intermech.DatabaseConfigurator;
using Intermech.DataFormats;
using Intermech.Docking;
using Intermech.Interfaces;
using Intermech.Interfaces.AVS;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.Configuration;
using Intermech.Interfaces.Pdm;
using Intermech.Interfaces.Pdm.RelationVisualizer;
using Intermech.Interfaces.Plugins;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Navigator;
using Intermech.Navigator.ContextCommands;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Controls;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Descriptos;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.ListInstances;
using Intermech.Navigator.Views;
using Intermech.Pdm.ArticlesView;
using Intermech.Pdm.ComponentSelection;
using Intermech.Pdm.Compositions;
using Intermech.Pdm.Compositions.CompareTree;
using Intermech.Pdm.Compositions.SearchScheme;
using Intermech.Pdm.ContextComposition;
using Intermech.Pdm.IDAttributesSyncronizer;
using Intermech.Pdm.ListInstancesWindow;
using Intermech.Pdm.OrderPointSelection;
using Intermech.Pdm.RelationVisualizer;
using Intermech.Pdm.SearchScheme;
using Intermech.Pdm.SettingsSyncAttributes;
using Intermech.Pdm.Substitutes;
using Intermech.Pdm.VirtualExemplars;
using Intermech.Pdm.VisDialogs;
using Intermech.PropertyEditors;
using Intermech.Protection;
using Intermech.Search;
using Intermech.Search.CompositionContexts;
using Intermech.Search.Pdm.Analogs;
using Intermech.Search.Pdm.CompositionCopying;
using Intermech.Search.Pdm.Instances;
using Intermech.Search.Pdm.PreciseProducts;
using Intermech.Search.Pdm.SeriesDates;
using Intermech.Search.Pdm.Substitutes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm;

public class PDMPlugin : 
  IPackage,
  IConfigurable,
  ICommandTarget,
  ICommandsProvider,
  IPDMSpecificationsService
{
  internal static DataTable _contextStatuses = (DataTable) null;
  internal static List<long> _hiddenCompositionObjects = new List<long>();
  private static bool _isUserAdmin = false;
  internal static bool PluginLocked = false;
  private static ICommandManager _commandManager = (ICommandManager) null;
  private static INamedImageList _namedImageList = (INamedImageList) null;
  private static INotificationService _notificationService = (INotificationService) null;
  internal static IFiltrationService _filtrationService = (IFiltrationService) null;
  internal static IClientPluginsService _clientPluginsService = (IClientPluginsService) null;
  internal static IElementStatusesClientService _elementStatusesClientService = (IElementStatusesClientService) null;
  internal static IDescriptorElementStatusesService _descriptorElementStatusesService = (IDescriptorElementStatusesService) null;
  internal static IArticleService _artService = (IArticleService) null;
  private static bool _IsInEvent = false;
  private static Guid _pluginGuid = new Guid("cad005f3-306c-11d8-b4e9-00304f19f545");
  private IServiceProvider _serviceProvider;
  internal IOutputView outputView;
  private bool _changeRelationSelf;
  private readonly string PluginName = "PdmPlugin";
  private readonly string VisConfig = "Visualizer";
  private readonly string CompositionId = nameof (CompositionId);
  private readonly string CompositionName = nameof (CompositionName);
  private readonly string ApplicabilityId = nameof (ApplicabilityId);
  private readonly string ApplicabilityName = nameof (ApplicabilityName);
  private readonly string StylesId = nameof (StylesId);
  private readonly string StylesName = nameof (StylesName);
  private readonly string PreviewModeName = nameof (PreviewMode);
  private static long _compositionSchemeId;
  private static long _applicabilitySchemeId;
  public static long _visStylesId;
  private static readonly string[] LineSeparators = new string[2]
  {
    "\r\n",
    "\n"
  };
  internal static readonly TraceSwitch TraceContains = new TraceSwitch("PDM.Contains", string.Empty, "0");
  private LazyService<IMainMenuService> _mainMenuService = new LazyService<IMainMenuService>();
  private PreciseProductsClientModule _preciseProductsClientModule = new PreciseProductsClientModule();
  private CompositionCopyingClientModule _compositionCopyingClientModule;
  private SeriesDatesClientModule _seriesDatesClientModule;

  public void Load(IServiceProvider serviceProvider)
  {
    int appId = 351;
    byte[][] numArray1 = new byte[32 /*0x20*/][]
    {
      new byte[16 /*0x10*/]
      {
        (byte) 246,
        (byte) 223,
        (byte) 134,
        (byte) 240 /*0xF0*/,
        (byte) 103,
        (byte) 49,
        (byte) 226,
        (byte) 199,
        (byte) 183,
        (byte) 107,
        (byte) 9,
        (byte) 10,
        (byte) 153,
        (byte) 38,
        (byte) 167,
        (byte) 178
      },
      new byte[16 /*0x10*/]
      {
        (byte) 33,
        (byte) 11,
        (byte) 161,
        (byte) 81,
        (byte) 205,
        (byte) 144 /*0x90*/,
        (byte) 128 /*0x80*/,
        (byte) 134,
        (byte) 105,
        (byte) 218,
        (byte) 111,
        (byte) 228,
        (byte) 6,
        (byte) 129,
        (byte) 80 /*0x50*/,
        (byte) 149
      },
      new byte[16 /*0x10*/]
      {
        (byte) 245,
        (byte) 30,
        (byte) 64 /*0x40*/,
        (byte) 56,
        (byte) 44,
        (byte) 67,
        (byte) 45,
        (byte) 217,
        (byte) 170,
        (byte) 219,
        (byte) 31 /*0x1F*/,
        (byte) 61,
        (byte) 9,
        (byte) 166,
        (byte) 217,
        (byte) 89
      },
      new byte[16 /*0x10*/]
      {
        (byte) 178,
        (byte) 3,
        (byte) 114,
        (byte) 132,
        (byte) 12,
        (byte) 143,
        (byte) 86,
        (byte) 46,
        (byte) 16 /*0x10*/,
        (byte) 189,
        (byte) 214,
        (byte) 48 /*0x30*/,
        (byte) 5,
        (byte) 139,
        (byte) 198,
        (byte) 80 /*0x50*/
      },
      new byte[16 /*0x10*/]
      {
        (byte) 110,
        (byte) 165,
        (byte) 46,
        (byte) 176 /*0xB0*/,
        (byte) 74,
        (byte) 145,
        (byte) 115,
        (byte) 214,
        (byte) 43,
        (byte) 59,
        (byte) 15,
        (byte) 77,
        (byte) 227,
        (byte) 21,
        (byte) 226,
        (byte) 206
      },
      new byte[16 /*0x10*/]
      {
        (byte) 147,
        (byte) 211,
        (byte) 213,
        (byte) 229,
        (byte) 245,
        (byte) 9,
        (byte) 72,
        (byte) 133,
        (byte) 198,
        (byte) 75,
        (byte) 154,
        (byte) 19,
        (byte) 165,
        (byte) 33,
        (byte) 178,
        (byte) 144 /*0x90*/
      },
      new byte[16 /*0x10*/]
      {
        (byte) 159,
        (byte) 95,
        (byte) 197,
        (byte) 35,
        (byte) 31 /*0x1F*/,
        (byte) 145,
        (byte) 167,
        (byte) 178,
        (byte) 177,
        (byte) 172,
        (byte) 70,
        (byte) 139,
        (byte) 17,
        (byte) 165,
        (byte) 189,
        (byte) 16 /*0x10*/
      },
      new byte[16 /*0x10*/]
      {
        (byte) 229,
        (byte) 74,
        (byte) 140,
        (byte) 139,
        (byte) 142,
        (byte) 150,
        (byte) 54,
        (byte) 12,
        (byte) 200,
        (byte) 171,
        (byte) 59,
        (byte) 207,
        (byte) 31 /*0x1F*/,
        (byte) 146,
        (byte) 43,
        (byte) 216
      },
      new byte[16 /*0x10*/]
      {
        (byte) 145,
        (byte) 176 /*0xB0*/,
        (byte) 156,
        (byte) 243,
        (byte) 248,
        (byte) 246,
        (byte) 103,
        (byte) 100,
        (byte) 171,
        (byte) 105,
        (byte) 13,
        (byte) 241,
        (byte) 52,
        (byte) 224 /*0xE0*/,
        (byte) 246,
        (byte) 149
      },
      new byte[16 /*0x10*/]
      {
        (byte) 21,
        (byte) 239,
        (byte) 211,
        (byte) 73,
        (byte) 110,
        (byte) 237,
        (byte) 20,
        (byte) 3,
        (byte) 92,
        (byte) 152,
        (byte) 68,
        (byte) 49,
        (byte) 152,
        (byte) 203,
        (byte) 218,
        (byte) 185
      },
      new byte[16 /*0x10*/]
      {
        (byte) 249,
        (byte) 80 /*0x50*/,
        (byte) 157,
        (byte) 170,
        (byte) 225,
        (byte) 182,
        (byte) 57,
        (byte) 220,
        (byte) 254,
        (byte) 123,
        byte.MaxValue,
        (byte) 158,
        (byte) 75,
        (byte) 231,
        (byte) 196,
        (byte) 134
      },
      new byte[16 /*0x10*/]
      {
        (byte) 156,
        (byte) 243,
        (byte) 12,
        (byte) 159,
        (byte) 109,
        (byte) 91,
        (byte) 173,
        (byte) 72,
        (byte) 180,
        (byte) 93,
        (byte) 39,
        (byte) 214,
        (byte) 128 /*0x80*/,
        (byte) 24,
        (byte) 197,
        (byte) 72
      },
      new byte[16 /*0x10*/]
      {
        (byte) 125,
        (byte) 254,
        (byte) 188,
        (byte) 50,
        (byte) 121,
        (byte) 238,
        (byte) 214,
        (byte) 168,
        (byte) 247,
        (byte) 97,
        (byte) 120,
        (byte) 181,
        (byte) 132,
        (byte) 181,
        (byte) 126,
        (byte) 209
      },
      new byte[16 /*0x10*/]
      {
        (byte) 48 /*0x30*/,
        (byte) 64 /*0x40*/,
        (byte) 46,
        (byte) 249,
        (byte) 231,
        (byte) 222,
        (byte) 79,
        (byte) 250,
        (byte) 6,
        (byte) 169,
        (byte) 60,
        (byte) 57,
        (byte) 228,
        (byte) 168,
        (byte) 178,
        (byte) 180
      },
      new byte[16 /*0x10*/]
      {
        (byte) 181,
        (byte) 177,
        (byte) 146,
        (byte) 192 /*0xC0*/,
        (byte) 7,
        (byte) 19,
        (byte) 47,
        (byte) 218,
        (byte) 119,
        (byte) 191,
        (byte) 244,
        (byte) 124,
        (byte) 247,
        (byte) 201,
        (byte) 24,
        (byte) 195
      },
      new byte[16 /*0x10*/]
      {
        (byte) 2,
        (byte) 115,
        (byte) 63 /*0x3F*/,
        (byte) 68,
        (byte) 218,
        (byte) 133,
        (byte) 143,
        (byte) 41,
        (byte) 188,
        (byte) 19,
        (byte) 61,
        (byte) 198,
        (byte) 30,
        (byte) 115,
        (byte) 98,
        (byte) 166
      },
      new byte[16 /*0x10*/]
      {
        (byte) 164,
        (byte) 134,
        (byte) 115,
        (byte) 53,
        (byte) 35,
        (byte) 196,
        (byte) 28,
        (byte) 10,
        (byte) 122,
        (byte) 244,
        (byte) 179,
        (byte) 128 /*0x80*/,
        (byte) 62,
        (byte) 131,
        (byte) 87,
        (byte) 73
      },
      new byte[16 /*0x10*/]
      {
        (byte) 245,
        (byte) 93,
        (byte) 89,
        (byte) 31 /*0x1F*/,
        (byte) 95,
        (byte) 203,
        (byte) 205,
        (byte) 212,
        (byte) 3,
        (byte) 204,
        (byte) 138,
        (byte) 221,
        (byte) 226,
        (byte) 65,
        (byte) 88,
        (byte) 229
      },
      new byte[16 /*0x10*/]
      {
        (byte) 253,
        (byte) 45,
        (byte) 233,
        (byte) 154,
        (byte) 85,
        (byte) 4,
        (byte) 127 /*0x7F*/,
        (byte) 106,
        (byte) 207,
        (byte) 225,
        (byte) 187,
        (byte) 185,
        (byte) 249,
        (byte) 89,
        (byte) 177,
        (byte) 45
      },
      new byte[16 /*0x10*/]
      {
        (byte) 205,
        (byte) 93,
        (byte) 115,
        (byte) 167,
        (byte) 201,
        (byte) 34,
        (byte) 193,
        (byte) 176 /*0xB0*/,
        (byte) 178,
        (byte) 57,
        (byte) 64 /*0x40*/,
        (byte) 21,
        (byte) 1,
        (byte) 107,
        (byte) 82,
        (byte) 151
      },
      new byte[16 /*0x10*/]
      {
        (byte) 72,
        (byte) 180,
        (byte) 17,
        (byte) 254,
        (byte) 66,
        (byte) 72,
        (byte) 173,
        (byte) 241,
        (byte) 148,
        (byte) 135,
        (byte) 70,
        (byte) 18,
        (byte) 96 /*0x60*/,
        (byte) 175,
        (byte) 156,
        (byte) 243
      },
      new byte[16 /*0x10*/]
      {
        (byte) 149,
        (byte) 204,
        (byte) 192 /*0xC0*/,
        (byte) 185,
        (byte) 123,
        (byte) 208 /*0xD0*/,
        (byte) 54,
        (byte) 59,
        (byte) 147,
        (byte) 133,
        (byte) 106,
        (byte) 113,
        (byte) 87,
        (byte) 169,
        (byte) 141,
        (byte) 8
      },
      new byte[16 /*0x10*/]
      {
        (byte) 103,
        (byte) 171,
        (byte) 127 /*0x7F*/,
        (byte) 201,
        (byte) 224 /*0xE0*/,
        (byte) 126,
        (byte) 109,
        (byte) 160 /*0xA0*/,
        (byte) 7,
        (byte) 9,
        (byte) 101,
        (byte) 65,
        (byte) 8,
        (byte) 98,
        (byte) 16 /*0x10*/,
        (byte) 32 /*0x20*/
      },
      new byte[16 /*0x10*/]
      {
        (byte) 181,
        (byte) 72,
        (byte) 186,
        (byte) 122,
        (byte) 3,
        (byte) 101,
        (byte) 245,
        (byte) 113,
        (byte) 138,
        (byte) 72,
        (byte) 124,
        (byte) 102,
        (byte) 209,
        (byte) 185,
        (byte) 26,
        (byte) 42
      },
      new byte[16 /*0x10*/]
      {
        (byte) 67,
        (byte) 133,
        (byte) 55,
        (byte) 137,
        (byte) 114,
        (byte) 89,
        (byte) 6,
        (byte) 180,
        (byte) 127 /*0x7F*/,
        (byte) 248,
        (byte) 5,
        (byte) 206,
        (byte) 189,
        (byte) 108,
        (byte) 9,
        (byte) 32 /*0x20*/
      },
      new byte[16 /*0x10*/]
      {
        (byte) 119,
        (byte) 248,
        (byte) 91,
        (byte) 94,
        (byte) 155,
        (byte) 235,
        (byte) 242,
        (byte) 239,
        (byte) 91,
        (byte) 135,
        (byte) 251,
        (byte) 11,
        (byte) 142,
        (byte) 61,
        (byte) 85,
        (byte) 16 /*0x10*/
      },
      new byte[16 /*0x10*/]
      {
        (byte) 69,
        (byte) 199,
        (byte) 81,
        (byte) 77,
        (byte) 218,
        (byte) 231,
        (byte) 77,
        (byte) 201,
        (byte) 71,
        (byte) 112 /*0x70*/,
        (byte) 247,
        (byte) 56,
        (byte) 227,
        (byte) 108,
        (byte) 232,
        (byte) 50
      },
      new byte[16 /*0x10*/]
      {
        (byte) 52,
        (byte) 108,
        (byte) 192 /*0xC0*/,
        (byte) 208 /*0xD0*/,
        (byte) 68,
        (byte) 180,
        (byte) 41,
        (byte) 147,
        (byte) 119,
        (byte) 141,
        (byte) 245,
        (byte) 193,
        (byte) 57,
        (byte) 193,
        (byte) 100,
        (byte) 58
      },
      new byte[16 /*0x10*/]
      {
        (byte) 118,
        (byte) 38,
        (byte) 207,
        (byte) 219,
        (byte) 119,
        (byte) 18,
        (byte) 129,
        (byte) 14,
        (byte) 164,
        (byte) 203,
        (byte) 74,
        (byte) 231,
        (byte) 121,
        (byte) 159,
        (byte) 226,
        (byte) 155
      },
      new byte[16 /*0x10*/]
      {
        (byte) 171,
        (byte) 33,
        (byte) 223,
        (byte) 189,
        (byte) 178,
        (byte) 22,
        (byte) 239,
        (byte) 54,
        (byte) 125,
        (byte) 98,
        (byte) 85,
        (byte) 36,
        (byte) 16 /*0x10*/,
        (byte) 124,
        (byte) 24,
        (byte) 192 /*0xC0*/
      },
      new byte[16 /*0x10*/]
      {
        byte.MaxValue,
        (byte) 68,
        (byte) 252,
        (byte) 65,
        (byte) 16 /*0x10*/,
        (byte) 194,
        (byte) 69,
        (byte) 91,
        (byte) 55,
        (byte) 110,
        (byte) 66,
        (byte) 1,
        (byte) 97,
        (byte) 206,
        (byte) 208 /*0xD0*/,
        (byte) 19
      },
      new byte[16 /*0x10*/]
      {
        (byte) 91,
        (byte) 0,
        (byte) 175,
        (byte) 155,
        (byte) 121,
        (byte) 200,
        (byte) 184,
        (byte) 74,
        (byte) 195,
        (byte) 18,
        (byte) 46,
        (byte) 82,
        (byte) 70,
        (byte) 171,
        (byte) 183,
        (byte) 106
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
    this.outputView = (IOutputView) serviceProvider.GetService(typeof (IOutputView));
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      PDMPlugin._isUserAdmin = sessionKeeper.Session.IsAdmin;
      IPdmServerPlugin pdmServerPlugin1 = (IPdmServerPlugin) null;
      IPdmServerPlugin pdmServerPlugin2;
      try
      {
        pdmServerPlugin2 = sessionKeeper.Session.GetCustomService(typeof (IPdmServerPlugin)) as IPdmServerPlugin;
      }
      catch
      {
        pdmServerPlugin2 = (IPdmServerPlugin) null;
      }
      PDMPlugin.PluginLocked = pdmServerPlugin2 == null;
      pdmServerPlugin1 = (IPdmServerPlugin) null;
      PDMPlugin._filtrationService = ServicesManager.GetService(typeof (IFiltrationService)) as IFiltrationService;
      if (!PDMPlugin.PluginLocked)
      {
        if (sessionKeeper.Session.GetCustomService(typeof (IPluginStatusesTable)) is IPluginStatusesTable customService)
          PDMPlugin._contextStatuses = customService.GetPluginStatusesTable("cad005fc-306c-11d8-b4e9-00304f19f545", true);
        ServicesManager.AddService(typeof (IPDMSpecificationsService), (object) this);
        ServicesManager.AddService(typeof (ISubstitutesSettings), (object) new SubstitutesSettings(sessionKeeper.Session));
        ServicesManager.AddService(typeof (IRelVisSettings), (object) new RelVisSettings(sessionKeeper.Session));
        ServicesManager.AddService(typeof (ISubstitutesRemarksService), (object) new SubstitutesRemarksService());
        ServicesManager.AddService(typeof (IComponentSelectionCommandService), (object) new CommandService(serviceProvider));
        RelVisPropertyPage relVisPropertyPage = new RelVisPropertyPage((IServiceProvider) ServicesManager.ServiceContainer);
        if (PDMPlugin._isUserAdmin)
        {
          SubstitutesSettingsPropertiesPage settingsPropertiesPage = new SubstitutesSettingsPropertiesPage((IServiceProvider) ServicesManager.ServiceContainer);
        }
        if (PDMPlugin._isUserAdmin)
        {
          ArticleAttributesSettings attributesSettings = new ArticleAttributesSettings((IServiceProvider) ServicesManager.ServiceContainer);
        }
        SeriesToolbar serviceInstance = new SeriesToolbar(PDMPlugin._filtrationService);
        serviceInstance.Initialize(true);
        ServicesManager.AddService(typeof (SeriesToolbar), (object) serviceInstance);
      }
      if (ServicesManager.GetService(typeof (IAttributePropertyDescriberService)) is IAttributePropertyDescriberService service2)
      {
        int attributeId1 = sessionKeeper.Session.GetAttributeType(PDMHelper.attributeGuidInstance).AttributeID;
        if (service2.GetDescriber(attributeId1) == null)
          service2.RegisterDescriber(attributeId1, (IAttributePropertyDescriber) new ObjectTypeAttDescriber());
        int attributeId2 = sessionKeeper.Session.GetAttributeType(PDMHelper.attributeGuidParty).AttributeID;
        if (service2.GetDescriber(attributeId2) == null)
          service2.RegisterDescriber(attributeId2, (IAttributePropertyDescriber) new ObjectTypeAttDescriber());
        int attributeId3 = sessionKeeper.Session.GetAttributeType(new Guid("cad00d18-306c-11d8-b4e9-00304f19f545")).AttributeID;
        if (service2.GetDescriber(attributeId3) == null)
          service2.RegisterDescriber(attributeId3, (IAttributePropertyDescriber) new RolePropertyDescriber());
        int attributeId4 = sessionKeeper.Session.GetAttributeType(new Guid(SearchConsts.attributeScheme4Types)).AttributeID;
        if (service2.GetDescriber(attributeId4) == null)
          service2.RegisterDescriber(attributeId4, (IAttributePropertyDescriber) new ObjectTypeAttDescriber());
      }
    }
    if (PDMPlugin.PluginLocked)
    {
      int num = (int) MessageBox.Show(LocalizationHolder.rm.GetString(sc_16458.ssp_pdm_16459()), PDMPluginConsts.Dialog1, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }
    else
    {
      PDMPlugin._commandManager = serviceProvider.GetService(typeof (ICommandManager)) as ICommandManager;
      if (PDMPlugin._commandManager == null)
        PDMPlugin._commandManager = (ICommandManager) new CommandManager();
      PDMPlugin._commandManager.AddTarget((ICommandTarget) this);
      PDMPlugin._namedImageList = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
      PDMPlugin._notificationService = ServicesManager.GetService(typeof (INotificationService)) as INotificationService;
      PDMPlugin._elementStatusesClientService = ServicesManager.GetService(typeof (IElementStatusesClientService)) as IElementStatusesClientService;
      PDMPlugin._descriptorElementStatusesService = ServicesManager.GetService(typeof (IDescriptorElementStatusesService)) as IDescriptorElementStatusesService;
      if (PDMPlugin._descriptorElementStatusesService != null)
        PDMPlugin._descriptorElementStatusesService.SetDescriptorStatuses += new Intermech.Navigator.Interfaces.SetDescriptorStatuses(this.SetDescriptorStatuses);
      if (PDMPlugin._notificationService != null)
      {
        SyncronizerService.Initialize();
        PDMPlugin._notificationService.Subscribe("ObjectsChanged", new NotificationEventHandler(SyncronizerService.ObjectChangedEvent));
        PDMPlugin._notificationService.Subscribe("RelationsChanged", new NotificationEventHandler(this.RelationsChangedEvent));
      }
      IObjectCreatorService service3 = (IObjectCreatorService) ServicesManager.GetService(typeof (IObjectCreatorService));
      if (service3 != null)
        service3.AfterEntersInCreatedEvent += new AfterEntersInCreatedEventHandler(this.iobjCr_EntersInCreatedEvent);
      IContentProvider service4 = (IContentProvider) serviceProvider.GetService(typeof (IContentProvider));
      if (service4 != null)
      {
        service4.ContentCallback += new GetContentCallback(CompareNavWindow.RestoreCompareNavWindowCallback);
        service4.ContentCallback += new GetContentCallback(ListInstancesNavWindow.RestoreWindowCallback);
        service4.ContentCallback += new GetContentCallback(CompareTreeWindow.RestoreWindowCallback);
        service4.ContentCallback += new GetContentCallback(this.RestoreTCEEditor);
      }
      ServicesManager.AddService(typeof (IAdditionalCompositionFiltrationService), (object) new AdditionalFiltrationService(PDMPlugin._namedImageList, this._mainMenuService.Value, PDMPlugin._notificationService));
      this.LoadPluginResources(serviceProvider);
      this.LoadPluginControls(serviceProvider);
      ServicesManager.AddService(typeof (IPDMSubstitutesService), (object) new PDMSubstitutesService((IServiceProvider) ServicesManager.ServiceContainer));
      ServicesManager.AddService(typeof (ISubstitutesClientService), (object) new SubstitutesClientService());
      ServiceLocator.Get<IFactory>().AddCommandsProvider((ICommandsProvider) new SubstitutesCommandsProvider());
      ServiceLocator.Register<IInstancesClientService>((IInstancesClientService) new InstancesClientService());
      ServiceLocator.Get<IFactory>().AddCommandsProvider((ICommandsProvider) new InstancesCommandsProvider());
      ServiceLocator.Get<IFactory>().AddViewsProvider((IViewsProvider) new AnalogsViewsProvider());
      this._preciseProductsClientModule.Load();
      this._compositionCopyingClientModule = new CompositionCopyingClientModule(ServiceLocator.Get<ICompositionCopyingDispatcherService>(), new CompositionCopyingDispatcherHandler((ICompositionCopyingClientService) new CompositionCopyingClientService(ServiceLocator.Get<ICurrentUserAndRole>(), ServiceLocator.Get<IFiltrationService>(), ServiceLocator.Get<INavigatorClientService>())));
      this._compositionCopyingClientModule.Load();
      this._seriesDatesClientModule = new SeriesDatesClientModule(ServiceLocator.Get<IFactory>());
      this._seriesDatesClientModule.Load();
      PDMPlugin._namedImageList.Add((Image) Intermech.Pdm.Properties.Resources.column_delete, "imgColumnsReset");
      PDMPlugin._namedImageList.Add((Image) Intermech.Pdm.Properties.Resources.exchange, "imgExchange");
      PDMPlugin._namedImageList.Add((Image) Intermech.Pdm.Properties.Resources.add2, "imgNewCompareRule");
      if (ServicesManager.GetService(typeof (IPluginManager)) is IPluginManager service5)
      {
        FileInfo fileInfo = new FileInfo(Path.Combine(new FileInfo(typeof (PDMPlugin).Assembly.Location).Directory.FullName, "Intermech.PdmConfigurator.dll"));
        if (fileInfo.Exists)
          service5.Load(fileInfo.FullName, false);
      }
      ServicesManager.AddService(typeof (ICompareTreeSettingsService), (object) new CompareTreeSettingsService());
      if (!(serviceProvider.GetService(typeof (IPropertyPagesService)) is IPropertyPagesService service6))
        return;
      service6.AddPage("Система\\Схемы поиска объектов", (IPropertyPage) new SearchSchemeSettingsPage());
    }
  }

  private DockControl RestoreTCEEditor(Guid guid, string persiststring)
  {
    if (!(guid == ContextCompositionEditor.ContextCompositionControlGuid))
      return (DockControl) null;
    ContextCompositionEditor compositionEditor = new ContextCompositionEditor();
    (long prototypeObject, long contextCompositionObject, long selectedContext, string contextName, long relationID)? nullable = compositionEditor.RestoreState(persiststring);
    if (!nullable.HasValue)
      return (DockControl) null;
    compositionEditor.Init(nullable.Value.prototypeObject, nullable.Value.contextCompositionObject, nullable.Value.selectedContext, nullable.Value.contextName, nullable.Value.relationID);
    return (DockControl) compositionEditor;
  }

  private void RelationsChangedEvent(object sender, NotificationEventArgs e)
  {
    if (this._changeRelationSelf || !(e is DBRelationsExtendedEventArgs extendedEventArgs))
      return;
    int attributePosDesignationID = MetaDataHelper.GetAttributeTypeID("cad01478-306c-11d8-b4e9-00304f19f545");
    if (!Array.Exists<AttributeValues>(extendedEventArgs.AttributeValuesArray, (Predicate<AttributeValues>) (x => x.AttributeID == attributePosDesignationID)))
      return;
    string str = Convert.ToString(Array.Find<AttributeValues>(extendedEventArgs.OrigAttributeValuesArray, (Predicate<AttributeValues>) (x => x.AttributeID == attributePosDesignationID)).Values[0]);
    if (string.IsNullOrEmpty(str))
      return;
    string initValue = Convert.ToString(Array.Find<AttributeValues>(extendedEventArgs.AttributeValuesArray, (Predicate<AttributeValues>) (x => x.AttributeID == attributePosDesignationID)).Values[0]);
    int attributeTypeId = MetaDataHelper.GetAttributeTypeID(ComponentSelectionConsts.attributeSelectionForPosDesignation);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(MetaDataHelper.GetRelationTypeID(ComponentSelectionConsts.relationTypeComponentSelection));
      IDBTransactions service1 = ServiceUtils.GetService<IDBTransactions>((object) sessionKeeper.Session, true);
      service1.StartTransaction();
      List<DBRelationsExtendedEventArgs> extendedEventArgsList = new List<DBRelationsExtendedEventArgs>();
      try
      {
        foreach (long relationId in (IEnumerable<long>) extendedEventArgs.RelationIDs)
        {
          IDBRelation relation1 = sessionKeeper.Session.GetRelation(relationId);
          foreach (DataRow row in (InternalDataCollectionBase) relationCollection.ConsistFrom(new DBRecordSetParams(new ConditionStructure[1]
          {
            new ConditionStructure(attributeTypeId, RelationalOperators.Equal, (object) str, LogicalOperators.AND, 0, false)
          }, new object[1]{ (object) -20 }), relation1.ProjID).Rows)
          {
            IDBRelation relation2 = sessionKeeper.Session.GetRelation(Convert.ToInt64(row[0]));
            IDBAttribute attributeById = relation2.GetAttributeByID(attributeTypeId);
            if (attributeById != null)
              attributeById.Value = (object) initValue;
            extendedEventArgsList.Add(new DBRelationsExtendedEventArgs("RelationsChanged", relation2.RelationID, relation2.RelationType, new AttributeValues[1]
            {
              new AttributeValues(attributeTypeId, (object) str)
            }, new AttributeValues[1]
            {
              new AttributeValues(attributeTypeId, (object) initValue)
            }));
          }
        }
        service1.Commit();
      }
      catch
      {
        service1.Rollback();
        throw;
      }
      if (extendedEventArgsList.Count <= 0)
        return;
      this._changeRelationSelf = true;
      try
      {
        INotificationService service2 = ServiceUtils.GetService<INotificationService>((object) ServicesManager.ServiceContainer, true);
        foreach (DBRelationsExtendedEventArgs e1 in extendedEventArgsList)
          service2.FireEvent((object) this, (NotificationEventArgs) e1);
      }
      finally
      {
        this._changeRelationSelf = false;
      }
    }
  }

  private void iobjCr_EntersInCreatedEvent(object sender, AfterEntersInCreatedEventArgs e)
  {
    if (!MetaDataHelper.GetObjectTypeChildrenIDRecursive(new Guid("cad0057f-306c-11d8-b4e9-00304f19f545")).Contains(e.ObjectType))
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject1 = sessionKeeper.Session.GetObject(e.ProjectID);
      if (!MetaDataHelper.GetObjectTypeChildrenIDRecursive(new Guid("cad00268-306c-11d8-b4e9-00304f19f545")).Contains(dbObject1.ObjectType))
        return;
      IDBObject dbObject2 = sessionKeeper.Session.GetObject(e.ObjectID);
      int attributeTypeId1 = MetaDataHelper.GetAttributeTypeID("cad0001f-306c-11d8-b4e9-00304f19f545");
      IDBAttribute byId1 = dbObject1.Attributes.FindByID(attributeTypeId1);
      if (byId1 != null && byId1.AsString != string.Empty)
      {
        string str = byId1.AsString;
        IDocumentTypeSettingsService customService = (IDocumentTypeSettingsService) sessionKeeper.Session.GetCustomService(typeof (IDocumentTypeSettingsService));
        if (customService != null)
        {
          DocumentTypeSettings settings = customService.GetSettings(sessionKeeper.Session.SessionGUID, e.ObjectType);
          if (settings.DocumentTypeCodeInDesignation && settings.DocumentTypeCode != string.Empty)
            str = DocumentsHelper.AppendDocCode(sessionKeeper.Session, byId1.AsString, settings.DocumentTypeCode);
        }
        IDBAttribute byId2 = dbObject2.Attributes.FindByID(attributeTypeId1);
        if (byId2 != null)
          byId2.AsString = str;
      }
      int attributeTypeId2 = MetaDataHelper.GetAttributeTypeID("cad00020-306c-11d8-b4e9-00304f19f545");
      IDBAttribute byId3 = dbObject1.Attributes.FindByID(attributeTypeId2);
      if (byId3 != null && byId3.AsString != string.Empty)
      {
        IDBAttribute byId4 = dbObject2.Attributes.FindByID(attributeTypeId2);
        if (byId4 != null)
          byId4.AsString = byId3.AsString;
      }
      int attributeTypeId3 = MetaDataHelper.GetAttributeTypeID("cad0038a-306c-11d8-b4e9-00304f19f545");
      IDBAttribute byId5 = dbObject1.Attributes.FindByID(attributeTypeId3);
      if (byId5 == null || !(byId5.AsString != string.Empty))
        return;
      IDBAttribute byId6 = dbObject2.Attributes.FindByID(attributeTypeId3);
      if (byId6 == null)
        return;
      byId6.AsString = byId5.AsString;
    }
  }

  public void Unload()
  {
    this._preciseProductsClientModule.Unload();
    this._compositionCopyingClientModule.Unload();
    this._seriesDatesClientModule.Unload();
    IObjectCreatorService service1 = (IObjectCreatorService) ServicesManager.GetService(typeof (IObjectCreatorService));
    if (service1 != null)
    {
      SearchSchemeCreatorForm.Detach(service1);
      VisSchemeCreatorForm.Detach(service1);
      VisStylesCreatorForm.Detach(service1);
    }
    IPreviewExtender service2 = (IPreviewExtender) ServicesManager.GetService(typeof (IPreviewExtender));
    if (service2 != null)
      service2.Extend -= new ExtendEventHandler(this.previewExtender_Extend);
    ((ILicenser) ServicesManager.GetService(typeof (ILicenser))).ReleaseLicense(15);
  }

  public string Name
  {
    [DebuggerStepThrough] get => PDMPluginConsts.PDMPluginName;
  }

  public void LoadConfiguration(IConfigurationManager configurationManager) => this.LoadVisConfig();

  public void SaveConfiguration(IConfigurationManager configurationManager) => this.SaveVisConfig();

  private void LoadVisConfig()
  {
    if (!(ServicesManager.GetService(typeof (IDBConfigurations)) is IDBConfigurations service))
      return;
    string DefaultValue = LocalizationHolder.rm.GetString("Pdm_rv_39");
    PDMPlugin._compositionSchemeId = service.ReadInteger(this.PluginName, this.VisConfig, this.CompositionId, 0L, DBConfigMode.UserAndGlobal);
    PDMPlugin.CompositionSchemeName = service.ReadString(this.PluginName, this.VisConfig, this.CompositionName, DefaultValue, DBConfigMode.UserAndGlobal);
    PDMPlugin._applicabilitySchemeId = service.ReadInteger(this.PluginName, this.VisConfig, this.ApplicabilityId, 0L, DBConfigMode.UserAndGlobal);
    PDMPlugin.ApplicabilitySchemeName = service.ReadString(this.PluginName, this.VisConfig, this.ApplicabilityName, DefaultValue, DBConfigMode.UserAndGlobal);
    PDMPlugin._visStylesId = service.ReadInteger(this.PluginName, this.VisConfig, this.StylesId, 0L, DBConfigMode.UserAndGlobal);
    PDMPlugin.VisStylesName = service.ReadString(this.PluginName, this.VisConfig, this.StylesName, DefaultValue, DBConfigMode.UserAndGlobal);
    PDMPlugin.PreviewMode = service.ReadInteger(this.PluginName, this.VisConfig, this.PreviewModeName, 1L, DBConfigMode.UserOnly);
  }

  private void SaveVisConfig()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBConfigurations configurations = sessionKeeper.Session.Configurations;
      if (configurations == null)
        return;
      long userId = sessionKeeper.Session.UserID;
      configurations.WriteInteger(this.PluginName, this.VisConfig, this.CompositionId, PDMPlugin._compositionSchemeId, userId);
      configurations.WriteString(this.PluginName, this.VisConfig, this.CompositionName, PDMPlugin.CompositionSchemeName, userId);
      configurations.WriteInteger(this.PluginName, this.VisConfig, this.ApplicabilityId, PDMPlugin._applicabilitySchemeId, userId);
      configurations.WriteString(this.PluginName, this.VisConfig, this.ApplicabilityName, PDMPlugin.ApplicabilitySchemeName, userId);
      configurations.WriteInteger(this.PluginName, this.VisConfig, this.StylesId, PDMPlugin._visStylesId, userId);
      configurations.WriteString(this.PluginName, this.VisConfig, this.StylesName, PDMPlugin.ApplicabilitySchemeName, userId);
      configurations.WriteInteger(this.PluginName, this.VisConfig, this.PreviewModeName, PDMPlugin.PreviewMode, userId);
    }
  }

  public bool Execute(ICommandState commandState) => false;

  public bool QueryStatus(ICommandState commandState) => false;

  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return new CommandsInfo();
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    if (PDMPlugin.PluginLocked || items == null || items.Count == 0)
      return CommandsInfo.Empty;
    INodeID itemId = items.GetItemID(0);
    if (itemId == null || itemId.CategoryID != 1)
      return CommandsInfo.Empty;
    IDBTypedObjectID itemData1 = items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
    CommandsInfo groupCommands = new CommandsInfo();
    bool flag1 = ((viewServices.GetService(typeof (IViewState)) is IViewState service1 ? (long) service1.ViewState : 0L) & 128L /*0x80*/) == 128L /*0x80*/;
    switch (PDMPlugin.CheckHiddenCompositionItems(items, viewServices))
    {
      case 0:
        groupCommands.Add("PDM.HideComposition", new CommandInfo(4, new ClickEventHandler(PDMPlugin.HideComposition), ContextMenuItemState.Unchecked));
        break;
      case 1:
        groupCommands.Add("PDM.HideComposition", new CommandInfo(4, new ClickEventHandler(PDMPlugin.HideComposition), ContextMenuItemState.Checked));
        break;
    }
    if (itemData1 != null)
    {
      Guid objectTypeGuid = MetaDataHelper.GetObjectTypeGuid(itemData1.ObjectType);
      Guid guid = new Guid("cad00132-306c-11d8-b4e9-00304f19f545");
      Guid parentType = new Guid("cad00650-306c-11d8-b4e9-00304f19f545");
      if (MetaDataHelper.HasObjectTypeDesignedRelType(itemData1.ObjectType) && !MetaDataHelper.IsObjectTypeChildOf(objectTypeGuid, parentType) && !PDMPluginConsts.DisableCreateTauCommand)
        groupCommands.Add("PDM.CreateContext", new CommandInfo(0, new ClickEventHandler(PDMPlugin.CreateContext)));
      if (items.Count == 1)
      {
        groupCommands.Add("PDM.CompareVersionComposition", new CommandInfo(0, new ClickEventHandler(this.CompareVersionComposition)));
        groupCommands.Add("PDM.TreeCompareForCompareVersionObjectsMenu", new CommandInfo(0, new ClickEventHandler(CompareTreeCommandHandler.CompareTreeVersion)));
        groupCommands.Add("PDM.InsertTechInComposition", new CommandInfo(0, new ClickEventHandler(PDMPlugin.InsertTechInComposition)));
        int relationTypeId = MetaDataHelper.GetRelationTypeID(new Guid("cadd99d9-306c-11d8-b4e9-00304f19f545"));
        List<int> childObjectTypesId = MetaDataHelper.GetApplicabilityChildObjectTypesID(itemData1.ObjectType, relationTypeId);
        if (childObjectTypesId != null && childObjectTypesId.Count > 0)
          groupCommands.Add("PDM.InsertAdditionalComplect", new CommandInfo(0, new ClickEventHandler(PDMPlugin.InsertAdditionalComplect)));
        groupCommands.Add("PDM.RelationVisualizer", new CommandInfo(0, new ClickEventHandler(PDMPlugin.RelationVisualizer)));
        groupCommands.Add("PDM.VisComplete", new CommandInfo(0, new ClickEventHandler(PDMPlugin.LaunchVisualizerComplete)));
        groupCommands.Add("PDM.VisComposition", new CommandInfo(0, new ClickEventHandler(PDMPlugin.LaunchVisualizerChilds)));
        groupCommands.Add("PDM.VisApplicability", new CommandInfo(0, new ClickEventHandler(PDMPlugin.LaunchVisualizerParents)));
        groupCommands.Add("PDM.VisCompScheme", new CommandInfo(0, new ClickEventHandler(PDMPlugin.ChooseCompScheme)));
        groupCommands.Add("PDM.VisAppScheme", new CommandInfo(0, new ClickEventHandler(PDMPlugin.ChooseAppScheme)));
        if (MetaDataHelper.GetObjectTypeID(new Guid("cad00268-306c-11d8-b4e9-00304f19f545")) == ObjectTypesCacheHelper.GetRootType(itemData1.ObjectType))
        {
          groupCommands.Add("PDM.CreateInstance", new CommandInfo(0, new ClickEventHandler(PDMPlugin.CreateInstance)));
          groupCommands.Add("PDM.ListInstance", new CommandInfo(0, new ClickEventHandler(PDMPlugin.ListInstance)));
          IObjectLCStepsCache objectLcStepsCache = CacheManager.Cache("ObjectLCStepsCache") as IObjectLCStepsCache;
          IDBLCStepID itemData2 = items.GetItemData(0, typeof (IDBLCStepID)) as IDBLCStepID;
          if (objectLcStepsCache != null && itemData2 != null)
          {
            IMSLifeCycleLevel lcLevel = MetaDataHelper.GetLCLevel(objectLcStepsCache.GetLevelID(itemData2.LCStepID));
            if (lcLevel != null && lcLevel.Guid.Equals(new Guid("cad00011-306c-11d8-b4e9-00304f19f545")))
              groupCommands.Add("PDM.CreateExemplar", new CommandInfo(0, new ClickEventHandler(PDMPlugin.CreateExemplar)));
          }
          if (MetaDataHelper.IsObjectTypeChildOf(itemData1.ObjectType, new Guid("cad00132-306c-11d8-b4e9-00304f19f545")))
            groupCommands.Add("PDM.FillFirstEntersTo", new CommandInfo(0, new ClickEventHandler(PDMPlugin.FillFirstEntersTo)));
        }
        if (flag1)
        {
          int objectTypeId = MetaDataHelper.GetObjectTypeID(new Guid("cad00250-306c-11d8-b4e9-00304f19f545"));
          if ((itemData1.ObjectType == objectTypeId || MetaDataHelper.GetObjectTypeChildrenID(new Guid("cad00250-306c-11d8-b4e9-00304f19f545")).Contains(itemData1.ObjectType)) && items.GetParentData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID parentData && MetaDataHelper.GetObjectTypeChildrenIDRecursive(new Guid("cad00268-306c-11d8-b4e9-00304f19f545")).Contains(parentData.ObjectType) | MetaDataHelper.GetObjectTypeChildrenIDRecursive(new Guid("cad00580-306c-11d8-b4e9-00304f19f545")).Contains(parentData.ObjectType))
            groupCommands.Add("PDM.AddZagotovkaForPart", new CommandInfo(8, new ClickEventHandler(this.AddZagotovkaForPart)));
        }
      }
    }
    if (items.Count > 1)
    {
      int objectTypeId1 = MetaDataHelper.GetObjectTypeID("cad00132-306c-11d8-b4e9-00304f19f545");
      int objectTypeId2 = MetaDataHelper.GetObjectTypeID("cad00250-306c-11d8-b4e9-00304f19f545");
      bool flag2 = false;
      int num = 0;
      ICurrentUserAndRole service2 = ServicesManager.GetService(typeof (ICurrentUserAndRole)) as ICurrentUserAndRole;
      for (int index = 0; index < items.Count; ++index)
      {
        IDBTypedObjectID itemData3 = (IDBTypedObjectID) items.GetItemData(index, typeof (IDBTypedObjectID));
        IDBCheckedOutByID itemData4 = (IDBCheckedOutByID) items.GetItemData(index, typeof (IDBCheckedOutByID));
        if (itemData3 == null || itemData4 == null)
        {
          flag2 = true;
          break;
        }
        if (index == 0)
        {
          if (itemData3.ObjectType != objectTypeId1 && itemData3.ObjectType != objectTypeId2)
          {
            flag2 = true;
            break;
          }
          num = itemData3.ObjectType;
        }
        else if (num != itemData3.ObjectType)
        {
          flag2 = true;
          break;
        }
        if (itemData4.CheckedOutBy != service2.UserID)
        {
          flag2 = true;
          break;
        }
      }
      if (!flag2)
        groupCommands.Add("PDM.MadeInstance", new CommandInfo(0, new ClickEventHandler(PDMPlugin.MakeInstance)));
    }
    bool flag3 = true;
    if (itemData1 == null)
      flag3 = false;
    else if (items.Count == 0 || items.Count == 1 && itemData1.ObjectID >= 0L)
      flag3 = false;
    if (flag3)
    {
      int num = -1;
      for (int index = 0; index < items.Count; ++index)
      {
        if (items.GetItemData(index, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData5)
        {
          if (index == 0)
            num = itemData5.ObjectType;
          if (num != itemData5.ObjectType)
          {
            num = -1;
            break;
          }
        }
      }
      if (num != -1)
      {
        groupCommands.Add("PDM.CompareComposition", new CommandInfo(0, new ClickEventHandler(PDMPlugin.CompareComposition)));
        groupCommands.Add("PDM.CompareCompositionForCompareObjectsMenu", new CommandInfo(0, new ClickEventHandler(PDMPlugin.CompareComposition)));
        if (items.Count <= 2)
        {
          groupCommands.Add("PDM.TreeCompare", new CommandInfo(0, new ClickEventHandler(CompareTreeCommandHandler.CompareMethod)));
          groupCommands.Add("PDM.TreeCompareForCompareObjectsMenu", new CommandInfo(0, new ClickEventHandler(CompareTreeCommandHandler.CompareMethod)));
        }
      }
    }
    return groupCommands;
  }

  private void CompareVersionComposition(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (!(items.GetItemData(0, typeof (IDBObjectID)) is IDBObjectID itemData))
      return;
    long versionForCompareId = VersionComparison.GetVersionForCompareId(viewServices, itemData);
    if (versionForCompareId == 0L)
      return;
    if (versionForCompareId == Math.Abs(itemData.Value))
      PDMPlugin.CompareComposition(Intermech.Navigator.ContextMenu.Services.GetItems(itemData.Value), viewServices, additionalInfo);
    else
      PDMPlugin.CompareComposition(Intermech.Navigator.ContextMenu.Services.GetItems(itemData.Value, versionForCompareId), viewServices, additionalInfo);
  }

  private void AddZagotovkaForPart(
    ISelectedItems items,
    IServiceProvider viewservices,
    object additionalinfo)
  {
    IDBTypedObjectID dbTypedObjectId = this.ChooseZagot();
    if (dbTypedObjectId == null || !(this._serviceProvider.GetService(typeof (IArticleService)) is IArticleService service))
      return;
    int relationTypeId = MetaDataHelper.GetRelationTypeID(PDMPluginGuids.linkZagotRelationGuid);
    using (SessionKeeper keeper = new SessionKeeper())
    {
      IDBTypedObjectID itemData = (IDBTypedObjectID) items.GetItemData(0, typeof (IDBTypedObjectID));
      if (itemData == null || !(items.GetParentData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID parentData))
        return;
      long[] articles = service.FindArticlesByGroupIDWithoutFiltration(parentData.ObjectID, (object) keeper.Session);
      if (articles.Length == 0)
        articles = new long[1]{ parentData.ObjectID };
      if (!this.IsArticlesCheckoutByCurrentUser(keeper, articles))
        return;
      this.WriteZagotAsMaterial(keeper, itemData, dbTypedObjectId.ObjectID);
      PDMPlugin.CreateLinkZagotRelation(keeper, relationTypeId, articles, parentData.ObjectType, dbTypedObjectId.ObjectID, itemData.ObjectID);
    }
  }

  private IDBTypedObjectID ChooseZagot()
  {
    object[] objArray = Intermech.Navigator.SelectionWindow.Select("Выберите объект", new DescriptorCollection()
    {
      (IDescriptor) new Intermech.Navigator.DBObjectTypes.Descriptor(MetaDataHelper.GetObjectTypeID(new Guid("cad00268-306c-11d8-b4e9-00304f19f545")))
    }[0], typeof (IDBTypedObjectID), SelectionOptions.Default | SelectionOptions.DisableMultiselect | SelectionOptions.ForceFilterObjectsByRule);
    return objArray == null || objArray.Length < 1 ? (IDBTypedObjectID) null : objArray[0] as IDBTypedObjectID;
  }

  private bool IsLinkExist(
    SessionKeeper keeper,
    long[] articles,
    IDBTypedObjectID zagotTypedObjID,
    int linkZagotRelationType)
  {
    IDBRelationCollection relationCollection = keeper.Session.GetRelationCollection(linkZagotRelationType);
    relationCollection.ObjectTypeID = MetaDataHelper.GetObjectTypeID(new Guid("cad00268-306c-11d8-b4e9-00304f19f545"));
    ColumnDescriptor[] columns = new ColumnDescriptor[3]
    {
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_PROJ_ID, AttributeSourceTypes.Relation, ColumnContents.Value, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object, ColumnContents.Value, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.CAPTION, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0)
    };
    DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[2]
    {
      new ConditionStructure(-21, RelationalOperators.In, (object) articles, LogicalOperators.AND, 0, true),
      new ConditionStructure(-2, RelationalOperators.Equal, (object) zagotTypedObjID.ObjectID, LogicalOperators.NONE, 0, true)
    }, columns);
    DataTable dataTable = relationCollection.Select(paramSet);
    if (dataTable == null || dataTable.Rows.Count <= 0)
      return false;
    long int64 = Convert.ToInt64(dataTable.Rows[0][0]);
    IDBObject dbObject = keeper.Session.GetObject(int64);
    int num = (int) MessageBox.Show(string.Format(LocalizationHolder.rm.GetString("Pdm_716"), (object) zagotTypedObjID.Caption, (object) dbObject.Caption), LocalizationHolder.rm.GetString("Pdm_715"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
    return true;
  }

  private bool IsArticlesCheckoutByCurrentUser(SessionKeeper keeper, long[] articles)
  {
    IDBObjectCollection objectCollection = keeper.Session.GetObjectCollection(MetaDataHelper.GetObjectTypeID(new Guid("cad00268-306c-11d8-b4e9-00304f19f545")));
    ColumnDescriptor[] columns = new ColumnDescriptor[3]
    {
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object, ColumnContents.Value, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.CAPTION, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_CHKOUT_BY, AttributeSourceTypes.Object, ColumnContents.ID, ColumnNameMapping.ID, SortOrders.NONE, 0)
    };
    DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(-2, RelationalOperators.In, (object) articles, LogicalOperators.NONE, 0, true)
    }, columns);
    DataTable dataTable = objectCollection.Select(paramSet);
    if (dataTable != null && dataTable.Rows.Count > 0)
    {
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        long userId = keeper.Session.UserID;
        if (Convert.ToInt64(row[2]) != userId)
        {
          long int64 = Convert.ToInt64(row[0]);
          string str = Convert.ToString(row[1]);
          int num = (int) MessageBox.Show(string.Format(LocalizationHolder.rm.GetString("Pdm_717"), (object) str, (object) int64), LocalizationHolder.rm.GetString("Pdm_715"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
      }
    }
    return true;
  }

  private void WriteZagotAsMaterial(
    SessionKeeper keeper,
    IDBTypedObjectID partTypedObjID,
    long zagotObjID)
  {
    int attributeTypeId = MetaDataHelper.GetAttributeTypeID(new Guid("cad0038c-306c-11d8-b4e9-00304f19f545"));
    IDBObject objectActualCopy = keeper.Session.GetObjectActualCopy(partTypedObjID.ObjectID, false);
    IDBAttribute attributeById = objectActualCopy.GetAttributeByID(attributeTypeId);
    AttributeValues newAttrValues = new AttributeValues(attributeTypeId, (object) zagotObjID);
    long num = -1;
    if (attributeById != null && attributeById.Value != null && attributeById.Value != DBNull.Value)
      num = Convert.ToInt64(attributeById.Value);
    if (num != -1L && Math.Abs(num) == Math.Abs(zagotObjID))
      return;
    objectActualCopy.SetAttributesValues(new AttributeValues[1]
    {
      newAttrValues
    });
    if (PDMPlugin._notificationService == null)
      return;
    PDMPlugin._notificationService.FireEvent((object) this, (NotificationEventArgs) new DBObjectsExtendedEventArgs(objectActualCopy.ObjectID, objectActualCopy.ObjectType, new AttributeValues(attributeTypeId, attributeById?.Value), newAttrValues));
  }

  private static void CreateLinkZagotRelation(
    SessionKeeper keeper,
    int linkZagotRelationType,
    long[] articles,
    int parentObjTypeID,
    long zagotObjID,
    long partObjID)
  {
    IDBRelationCollection relationCollection = keeper.Session.GetRelationCollection(linkZagotRelationType);
    List<long> relationIDs = new List<long>();
    List<long> projIDs = new List<long>();
    List<int> projTypeIDs = new List<int>();
    List<int> relTypeIDs = new List<int>();
    foreach (long article in articles)
    {
      IDBRelation dbRelation = relationCollection.Create(article, zagotObjID);
      if (dbRelation != null)
      {
        dbRelation.SetAttributesValues(new AttributeValues[1]
        {
          new AttributeValues(AvsIDCache.Attr_ArticleID, (object) Math.Abs(partObjID))
        });
        relationIDs.Add(dbRelation.RelationID);
        projIDs.Add(article);
        projTypeIDs.Add(parentObjTypeID);
        relTypeIDs.Add(linkZagotRelationType);
      }
    }
    ((INotificationService) ServicesManager.GetService(typeof (INotificationService)))?.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsCreated", (IList<long>) relationIDs, (IList<long>) projIDs, (IList<int>) projTypeIDs, (IList<int>) relTypeIDs));
  }

  public List<long> GetSpecifyingObjects(long specID)
  {
    List<long> specifyingObjects = new List<long>();
    if (specID == 0L || specID == 0L || specID == -1L)
      return specifyingObjects;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject baseArticle = PDMPlugin._artService.FindBaseArticle(specID, string.Empty, (object) sessionKeeper.Session.SessionGUID);
      if (baseArticle != null)
        specifyingObjects.Insert(0, baseArticle.ObjectID);
      DataTable dataTable = sessionKeeper.Session.GetRelationCollection(MetaDataHelper.GetRelationTypeID("cad00154-306c-11d8-b4e9-00304f19f545")).EntersInVersion(new DBRecordSetParams((ConditionStructure[]) null, new ColumnDescriptor[1]
      {
        new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, -1)
      }), specID);
      if (dataTable == null)
        return specifyingObjects;
      for (int index = 0; index < dataTable.Rows.Count; ++index)
      {
        long int64 = Convert.ToInt64(dataTable.Rows[index][0]);
        if (!specifyingObjects.Contains(int64))
          specifyingObjects.Add(int64);
      }
      dataTable.Dispose();
      if (specifyingObjects.Count > 0)
      {
        List<long> listInstances = PDMPlugin._artService.GetListInstances(specifyingObjects[0], (object) sessionKeeper.Session.SessionGUID);
        if (listInstances != null)
        {
          if (listInstances.Count > 0)
          {
            for (int index = 0; index < listInstances.Count; ++index)
            {
              if (!specifyingObjects.Contains(listInstances[index]))
                specifyingObjects.Add(listInstances[index]);
            }
          }
        }
      }
    }
    return specifyingObjects;
  }

  public long GetObjectWithDesignation(int objectType, string designation)
  {
    if (!MetaDataHelper.ExistsObjectType(objectType))
      return 0;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      DBRecordSetParams dbRecordSetParams = new DBRecordSetParams(new ConditionStructure[1]
      {
        new ConditionStructure(MetaDataHelper.GetAttributeTypeID("cad0001f-306c-11d8-b4e9-00304f19f545"), RelationalOperators.Equal, (object) designation, LogicalOperators.NONE, 0, false)
      }, new ColumnDescriptor[1]
      {
        new ColumnDescriptor((object) -2, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, -1)
      }, recordCount: 1);
      DataTable dataTable = sessionKeeper.Session.ObjectsSelect(objectType, dbRecordSetParams);
      if (dataTable == null)
        return 0;
      try
      {
        return dataTable.Rows.Count == 0 ? 0L : Convert.ToInt64(dataTable.Rows[0][0]);
      }
      finally
      {
        dataTable.Dispose();
      }
    }
  }

  public long GetObjectSpecification(long objectID)
  {
    if (objectID == -1L || objectID == 0L || objectID == 0L)
      return 0;
    DBRecordSetParams paramSet = new DBRecordSetParams((ConditionStructure[]) null, new ColumnDescriptor[1]
    {
      new ColumnDescriptor((object) -2)
    }, recordCount: 1);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(MetaDataHelper.GetRelationTypeID("cad00154-306c-11d8-b4e9-00304f19f545"));
      relationCollection.ObjectTypeID = MetaDataHelper.GetObjectTypeID("cad00133-306c-11d8-b4e9-00304f19f545");
      DataTable dataTable = relationCollection.ConsistFrom(paramSet, objectID);
      if (dataTable == null)
        return 0;
      try
      {
        return dataTable.Rows.Count == 0 ? 0L : Convert.ToInt64(dataTable.Rows[0][0]);
      }
      finally
      {
        dataTable.Dispose();
      }
    }
  }

  public DialogResult CreateArticlesForm(
    long prototypeID,
    List<long> newObjects,
    string defMainDesign,
    string articlesName)
  {
    return ArticlesCreatorForm.Execute(prototypeID, newObjects, defMainDesign, articlesName);
  }

  private void LoadPluginResources(IServiceProvider serviceProvider)
  {
    if (PDMPlugin.PluginLocked || PDMPlugin._namedImageList == null)
      return;
    Stream manifestResourceStream = this.GetType().Assembly.GetManifestResourceStream("Intermech.Pdm.Resources.SubstitutionsBitmaps.bmp");
    if (manifestResourceStream != null)
    {
      using (Bitmap images = new Bitmap(manifestResourceStream))
      {
        images.MakeTransparent();
        PDMPlugin._namedImageList.AddStrip((Image) images, new string[16 /*0x10*/]
        {
          "imgSubstitutes.PDM",
          "imgCreateSubstitutesGroup.PDM",
          "imgMakeActualSubstitute.PDM",
          "imgEditSubstitutesGroup.PDM",
          "imgDeleteSubstitutesGroup.PDM",
          "imgSubstitutes.PDM",
          "imgContextComposition.PDM",
          "imgDesignContext.PDM",
          "imgHiddenChilds.PDM",
          "imgHiddenComposition.PDM",
          "imgComposition.PDM",
          "imgHideComposition.PDM",
          "imgObjects.PDM",
          "imgObjects.ActualSubstitute",
          "imgObjects.Substitute",
          "imgObjects.DesignVariant"
        });
      }
      manifestResourceStream.Close();
    }
    PDMPlugin._namedImageList.Add(Intermech.Pdm.Properties.Resources.add_dopzam, "icoCreateSubstitutesGroup.PDM");
    PDMPlugin._namedImageList.Add(Intermech.Pdm.Properties.Resources.delete_dopzam, "icoDeleteSubstitutesGroup.PDM");
    PDMPlugin._namedImageList.Add(Intermech.Pdm.Properties.Resources.edit_dopzam, "icoEditSubstitutesGroup.PDM");
    PDMPlugin._namedImageList.Add(Intermech.Pdm.Properties.Resources.make_actual_dopzam, "icoMakeActualSubstitute.PDM");
  }

  private void LoadPluginControls(IServiceProvider serviceProvider)
  {
    if (PDMPlugin.PluginLocked || serviceProvider == null)
      return;
    PDMPlugin._artService = (IArticleService) new ArticleService();
    this._serviceProvider = serviceProvider;
    IPluginManager service1 = (IPluginManager) serviceProvider.GetService(typeof (IPluginManager));
    if (service1 != null)
      service1.LoadComplete += new EventHandler(this.pluginManager_LoadComplete);
    ServicesManager.AddService(typeof (IArticleService), (object) PDMPlugin._artService);
    IFactory service2 = (IFactory) serviceProvider.GetService(typeof (IFactory));
    service2.AddCommandsProvider((ICommandsProvider) this);
    service2.AddCommandsProvider((ICommandsProvider) new Intermech.Pdm.ComponentSelection.CommandsProvider());
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObjectType objectType1 = sessionKeeper.Session.GetObjectType(new Guid("cad00129-306c-11d8-b4e9-00304f19f545"), false);
      if (objectType1 != null)
        service2.AddCommandsProvider(1, objectType1.ObjectType, (ICommandsProvider) new SearchSchemeCommandProvider());
      IDBObjectType objectType2 = sessionKeeper.Session.GetObjectType(new Guid("cadd9aa6-306c-11d8-b4e9-00304f19f545"), false);
      if (objectType2 != null)
        service2.AddCommandsProvider(1, objectType2.ObjectType, (ICommandsProvider) new VisSchemeCommandProvider());
      IDBObjectType objectType3 = sessionKeeper.Session.GetObjectType(new Guid("cadd9aa7-306c-11d8-b4e9-00304f19f545"), false);
      if (objectType3 != null)
        service2.AddCommandsProvider(1, objectType3.ObjectType, (ICommandsProvider) new VisStyleCommandProvider());
      if (sessionKeeper.Session.GetCustomService(typeof (IVersionRulesCacheService)) is IVersionRulesCacheService customService)
      {
        PDMPlugin._hiddenCompositionObjects = customService[sessionKeeper.Session.UserID, (object) "{9D621C68-0820-47EC-9ABB-CC7D2EF820F6}"] as List<long>;
        if (PDMPlugin._hiddenCompositionObjects == null)
          PDMPlugin._hiddenCompositionObjects = new List<long>(0);
      }
    }
    service2.AddCommandsProvider(1, MetaDataHelper.GetObjectTypeID("cad00650-306c-11d8-b4e9-00304f19f545"), (ICommandsProvider) new ContextCompositionCommandProvider());
    if (ServicesManager.GetService(typeof (IContentProvider)) is IContentProvider service3)
      service3.ContentCallback += new GetContentCallback(VisView.RestoreWindowCallback);
    MenuTemplate contextMenuTemplate = service2.ContextMenuTemplate;
    try
    {
      contextMenuTemplate.BeginUpdate();
      int[] numArray = new int[12]
      {
        -1,
        -1,
        -1,
        -1,
        -1,
        -1,
        -1,
        -1,
        -1,
        -1,
        -1,
        -1
      };
      if (PDMPlugin._namedImageList != null)
      {
        numArray[0] = PDMPlugin._namedImageList.ImageIndex("imgSubstitutes.PDM");
        numArray[1] = PDMPlugin._namedImageList.ImageIndex("icoCreateSubstitutesGroup.PDM");
        numArray[2] = PDMPlugin._namedImageList.ImageIndex("icoMakeActualSubstitute.PDM");
        numArray[3] = PDMPlugin._namedImageList.ImageIndex("icoEditSubstitutesGroup.PDM");
        numArray[4] = PDMPlugin._namedImageList.ImageIndex("icoDeleteSubstitutesGroup.PDM");
        numArray[5] = PDMPlugin._namedImageList.ImageIndex("imgObjects.PDM");
        numArray[6] = PDMPlugin._namedImageList.ImageIndex("imgSubstitutes.PDM");
        numArray[7] = PDMPlugin._namedImageList.ImageIndex("imgContextComposition.PDM");
        numArray[8] = PDMPlugin._namedImageList.ImageIndex("imgHiddenChilds.PDM");
        numArray[9] = PDMPlugin._namedImageList.ImageIndex("imgHiddenComposition.PDM");
        numArray[10] = PDMPlugin._namedImageList.ImageIndex("imgComposition.PDM");
        numArray[11] = PDMPlugin._namedImageList.ImageIndex("imgHideComposition.PDM");
      }
      if (MetaDataHelper.GetObjectType(PDMPluginGuids.orderPointGuid) != null)
      {
        contextMenuTemplate.Nodes.Add(new MenuTemplateNode(PDMPluginConsts.cmdAddToOrderPoint, PDMPluginConsts.menuAddToOrderPoint, -1, 90, 10));
        contextMenuTemplate.Nodes.Add(new MenuTemplateNode(PDMPluginConsts.cmdUpdateSpecificationNotes, PDMPluginConsts.menuUpdateSpecificationNotes, -1, 100, 10));
      }
      MenuTemplateNode menuTemplateNode1 = service2.ContextMenuTemplate["ObjectComposition"];
      menuTemplateNode1.Nodes.Add(new MenuTemplateNode("PDM.CreateSubstitutesGroup", PDMPluginConsts.menuCreateSubstitutesGroup, numArray[1], 20, 10));
      menuTemplateNode1.Nodes.Add(new MenuTemplateNode("PDM.MakeActualSubstitute", PDMPluginConsts.menuMakeActualSubstitute, numArray[2], 20, 20));
      menuTemplateNode1.Nodes.Add(new MenuTemplateNode("PDM.EditSubstitutesGroup", PDMPluginConsts.menuEditSubstitutesGroup, numArray[3], 20, 30));
      menuTemplateNode1.Nodes.Add(new MenuTemplateNode("PDM.DeleteSubstitutesGroup", PDMPluginConsts.menuDeleteSubstitutesGroup, numArray[4], 20, 40));
      menuTemplateNode1.Nodes.Add(new MenuTemplateNode("PDM.HideComposition", PDMPluginConsts.menuHideComposition, numArray[11], 30, 10));
      menuTemplateNode1.Nodes.Add(new MenuTemplateNode("PDM.InsertTechInComposition", PDMPluginConsts.menuInsertTechInComposition, -1, 31 /*0x1F*/, 10));
      menuTemplateNode1.Nodes.Add(new MenuTemplateNode("PDM.InsertAdditionalComplect", PDMPluginConsts.menuInsertAdditionalComplect, -1, 31 /*0x1F*/, 11));
      menuTemplateNode1.Nodes.Add(new MenuTemplateNode("PDM.AddZagotovkaForPart", PDMPluginConsts.menuAddZagotovkaForPart, -1, 32 /*0x20*/, 10));
      INamedImageList service4 = (INamedImageList) serviceProvider.GetService(typeof (INamedImageList));
      int imageIndex1 = service4 != null ? service4.ImageIndex(sc_16458.ssp_pdm_16460()) : -1;
      menuTemplateNode1.Nodes.Add(new MenuTemplateNode("PDM.CompareComposition", PDMPluginConsts.menuCompareComposition, imageIndex1, 15, 10));
      MenuTemplateNode menuTemplateNode2 = service2.ContextMenuTemplate["CompareObjects"];
      if (menuTemplateNode2 != null)
      {
        menuTemplateNode2.Nodes.Add(new MenuTemplateNode("PDM.CompareCompositionForCompareObjectsMenu", PDMPluginConsts.menuCompareCompositionForCompareObjectsMenu, imageIndex1, 10, 10));
        menuTemplateNode2.Nodes.Add(new MenuTemplateNode("PDM.TreeCompareForCompareObjectsMenu", PDMPluginConsts.menuTreeCompare, imageIndex1, 10, 20));
      }
      MenuTemplateNode menuTemplateNode3 = service2.ContextMenuTemplate["CompareVersionObjects"];
      if (menuTemplateNode3 != null)
      {
        menuTemplateNode3.Nodes.Add(new MenuTemplateNode("PDM.CompareVersionComposition", PDMPluginConsts.menuCompareCompositionForCompareObjectsMenu, imageIndex1, 10, 10));
        menuTemplateNode3.Nodes.Add(new MenuTemplateNode("PDM.TreeCompareForCompareVersionObjectsMenu", PDMPluginConsts.menuTreeCompare, imageIndex1, 10, 20));
      }
      menuTemplateNode1.Nodes.Add(new MenuTemplateNode("PDM.TreeCompare", PDMPluginConsts.menuTreeCompare, imageIndex1, 15, 20));
      contextMenuTemplate.Nodes.Add(new MenuTemplateNode("PDM.FillFirstEntersTo", PDMPluginConsts.menuFillFirstEntersTo, -1, 110, 10));
      MenuTemplateNode menuTemplateNode4 = service2.ContextMenuTemplate[sc_16458.ssp_pdm_16461()];
      if (menuTemplateNode4 != null)
      {
        menuTemplateNode4.Nodes.Add(new MenuTemplateNode("PDM.CreateContext", PDMPluginConsts.menuCreateContext, numArray[7], 10, 100));
        menuTemplateNode4.Nodes.Add(new MenuTemplateNode("PDM.CreateInstance", PDMPluginConsts.menuInstance, -1, 10, 110));
        menuTemplateNode4.Nodes.Add(new MenuTemplateNode("Create.Instances", "Группу исполнений", -1, 10, 120));
        menuTemplateNode4.Nodes.Add(new MenuTemplateNode("PDM.CreateExemplar", PDMPluginConsts.menuCreateExemplar, -1, 10, 130));
      }
      contextMenuTemplate.Nodes.Add(new MenuTemplateNode("PDM.Exemplar", PDMPluginConsts.menuExemplar, -1, 75, 10)
      {
        Nodes = {
          new MenuTemplateNode("PDM.TreeExemplars", PDMPluginConsts.menuTreeExemplars, -1, 10, 10)
        }
      });
      IGuidMapper service5 = (IGuidMapper) serviceProvider.GetService(typeof (IGuidMapper));
      contextMenuTemplate.Nodes.Add(new MenuTemplateNode("PDM.ListInstance", PDMPluginConsts.menuListInstance, numArray[5], 30, 20));
      PDMPlugin.CompositionSchemeId = 0L;
      PDMPlugin.ApplicabilitySchemeId = 0L;
      PDMPlugin.CompositionSchemeName = LocalizationHolder.rm.GetString("Pdm_rv_39");
      PDMPlugin.ApplicabilitySchemeName = PDMPlugin.CompositionSchemeName;
      Bitmap rv1 = Intermech.Pdm.Properties.Resources.rv1;
      int imageIndex2 = rv1 != null ? PDMPlugin._namedImageList.Add((Image) rv1, "imgVisComplete") : -1;
      Bitmap child = Intermech.Pdm.Properties.Resources.child;
      int imageIndex3 = child != null ? PDMPlugin._namedImageList.Add((Image) child, "imgVisComposition") : -1;
      Bitmap rarent = Intermech.Pdm.Properties.Resources.rarent;
      int imageIndex4 = rarent != null ? PDMPlugin._namedImageList.Add((Image) rarent, "imgVisApplicability") : -1;
      MenuTemplateNode node1 = new MenuTemplateNode("PDM.VisualizerRoot", LocalizationHolder.rm.GetString("Pdm_rv_33"), -1, 30, 21);
      MenuTemplateNode node2 = new MenuTemplateNode("PDM.VisComplete", LocalizationHolder.rm.GetString("Pdm_rv_34"), imageIndex2, 0, 1);
      node1.Nodes.Add(node2);
      MenuTemplateNode node3 = new MenuTemplateNode("PDM.VisComposition", LocalizationHolder.rm.GetString("Pdm_rv_35"), imageIndex3, 0, 2);
      node1.Nodes.Add(node3);
      MenuTemplateNode node4 = new MenuTemplateNode("PDM.VisApplicability", LocalizationHolder.rm.GetString("Pdm_rv_36"), imageIndex4, 0, 3);
      node1.Nodes.Add(node4);
      node1.Nodes.Add(new MenuTemplateNode("PDM.VisCompScheme", LocalizationHolder.rm.GetString("Pdm_rv_37") + PDMPlugin.CompositionSchemeName, -1, 1, 4));
      node1.Nodes.Add(new MenuTemplateNode("PDM.VisAppScheme", LocalizationHolder.rm.GetString("Pdm_rv_38") + PDMPlugin.ApplicabilitySchemeName, -1, 1, 5));
      contextMenuTemplate.Nodes.Add(node1);
      service2.OnMenuTemplateNodeTransformEventHandler += new MenuTemplateNodeTransformEventHandler(this.FactoryOnMenuTemplateNodeTransformEventHandler);
      contextMenuTemplate.Nodes.Add(new MenuTemplateNode("PDM.AddInstance", PDMPluginConsts.menuAddInstance, -1, 30, 22));
      contextMenuTemplate.Nodes.Add(new MenuTemplateNode("PDM.MadeInstance", PDMPluginConsts.menuMakeInstance, -1, 30, 23));
      contextMenuTemplate.Nodes.Add(new MenuTemplateNode("PDM.Exclude", PDMPluginConsts.menuExcludeInstance, -1, 30, 24));
      Intermech.Pdm.ComponentSelection.ContextMenu.CreateTemplate(contextMenuTemplate);
      PDMPluginConsts.CategoryInstance = service5.Register(PDMPluginGuids.CategoryInstanceGuid);
      service2.AddNodeType(PDMPluginConsts.CategoryInstance, typeof (ListInstancesNode));
      service2.AddViewsProvider(PDMPluginConsts.CategoryInstance, (IViewsProvider) new InstanceViewProvider());
      service2.AddCommandsProvider(PDMPluginConsts.CategoryInstance, (ICommandsProvider) new ListInstancesCommandProvider());
      service2.AddCommandsProvider((ICommandsProvider) new ExcludeInstancesProvider());
      PDMPluginConsts.CategoryCompareObject = service5.Register(PDMPluginGuids.CategoryCompareObjectGuid);
      service2.AddNodeType(PDMPluginConsts.CategoryCompareObject, typeof (CompareObjectNode));
      service2.AddViewsProvider(1, (IViewsProvider) new CompareObjectViewProvider());
      PDMPluginConsts.CategoryCompareObjectsRoot = service5.Register(PDMPluginGuids.CategoryCompareObjectsRootGuid);
      service2.AddNodeType(PDMPluginConsts.CategoryCompareObjectsRoot, typeof (CompareObjectsListNode));
      service2.AddViewsProvider(PDMPluginConsts.CategoryCompareObjectsRoot, (IViewsProvider) new CompareObjectsRootViewProvider());
      this.RegisterIconForNode(service4, "imgObjects.PDM", PDMPluginConsts.CategoryInstance, 0);
      this.RegisterIconForNode(service4, "imgCompCompare", PDMPluginConsts.CategoryCompareObjectsRoot, 0);
      PDMPluginConsts.CategoryContains = service5.Register(PDMPluginGuids.CategoryContainsGuid);
      service2.AddNodeType(PDMPluginConsts.CategoryContains, typeof (ContainsNode));
      PDMPluginConsts.CategorySubstitutes = service5.Register(PDMPluginGuids.CategorySubstitutesGuid);
      service2.AddNodeType(PDMPluginConsts.CategorySubstitutes, typeof (SubstitutesNode));
      service2.AddViewsProvider(1, (IViewsProvider) new ContainsViewProvider());
      SearchSchemeProvider provider1 = new SearchSchemeProvider();
      service2.AddViewsProvider(1, MetaDataHelper.GetObjectTypeID("cad0012b-306c-11d8-b4e9-00304f19f545"), (IViewsProvider) provider1);
      service2.AddViewsProvider(1, MetaDataHelper.GetObjectTypeID("cad0012a-306c-11d8-b4e9-00304f19f545"), (IViewsProvider) provider1);
      VisSchemeProvider provider2 = new VisSchemeProvider();
      service2.AddViewsProvider(1, MetaDataHelper.GetObjectTypeID("cadd9aa6-306c-11d8-b4e9-00304f19f545"), (IViewsProvider) provider2);
      VisStyleProvider provider3 = new VisStyleProvider();
      service2.AddViewsProvider(1, MetaDataHelper.GetObjectTypeID("cadd9aa7-306c-11d8-b4e9-00304f19f545"), (IViewsProvider) provider3);
      CompareRulesViewProvider provider4 = new CompareRulesViewProvider();
      service2.AddViewsProvider(1, MetaDataHelper.GetObjectTypeID(PDMHelper.objtypeCommonCompositionRules), (IViewsProvider) provider4);
      service2.AddViewsProvider(1, MetaDataHelper.GetObjectTypeID(PDMHelper.objtypePersonalCompositionRules), (IViewsProvider) provider4);
      IObjectCreatorService service6 = (IObjectCreatorService) ServicesManager.GetService(typeof (IObjectCreatorService));
      if (service6 != null)
      {
        SearchSchemeCreatorForm.Attach(service6);
        CompareRulesCreatorForm.Attach(service6);
        VisSchemeCreatorForm.Attach(service6);
        VisStylesCreatorForm.Attach(service6);
      }
      IPreviewExtender service7 = (IPreviewExtender) serviceProvider.GetService(typeof (IPreviewExtender));
      if (service7 != null)
        service7.Extend += new ExtendEventHandler(this.previewExtender_Extend);
      (ServicesManager.GetService(typeof (IViewsManagerService)) as IViewsManagerService).OnActivateView += new Intermech.Interfaces.Client.ActivateViewEventHandler(this.ActivateViewEventHandler);
      service2.AddViewsProvider(1, (IViewsProvider) new ArticlesViewProvider());
      PDMPlugin._filtrationService.OnFiltrationChanged += new FiltrationChanged(this.OnFiltrationChanged);
      AdjustableViewsHelper.RegisterView("PDM.SearchSchemeView", LocalizationHolder.rm.GetString("Pdm_453"), LocalizationHolder.rm.GetString("Pdm_454"), "Intermech.PDM", "imgEditScheme", true, 5);
      AdjustableViewsHelper.RegisterView("PDM.ArticlesView", LocalizationHolder.rm.GetString("Pdm_455"), LocalizationHolder.rm.GetString("Pdm_456"), "Intermech.PDM", "imgObject", true, 12);
      AdjustableViewsHelper.RegisterView("PDM.InstanceView", LocalizationHolder.rm.GetString("Pdm_457"), LocalizationHolder.rm.GetString("Pdm_458"), "Intermech.PDM", "", true, 20);
      AdjustableViewsHelper.RegisterView("PDM.ContainsView", LocalizationHolder.rm.GetString("Pdm_459"), LocalizationHolder.rm.GetString("Pdm_460"), "Intermech.PDM", "imgContains", true, 25);
      AdjustableViewsHelper.RegisterView("PDM.ApplicabilityView", LocalizationHolder.rm.GetString("Pdm_461"), LocalizationHolder.rm.GetString("Pdm_462"), "Intermech.PDM", "imgEntersTo", true, 26);
      AdjustableViewsHelper.RegisterView("PDM.CompareCompositionView", LocalizationHolder.rm.GetString("Pdm_552"), LocalizationHolder.rm.GetString("Pdm_553"), "Intermech.PDM", "imgObjects.PDM", true, 5);
    }
    finally
    {
      contextMenuTemplate.EndUpdate();
    }
  }

  private void FactoryOnMenuTemplateNodeTransformEventHandler(
    object sender,
    MenuTemplateNodeTransformEventArgs e)
  {
    if (e.MenuTemplateNode.Name == "PDM.VisCompScheme")
      e.MenuTemplateNode.Text = LocalizationHolder.rm.GetString("Pdm_rv_37") + PDMPlugin.CompositionSchemeName;
    if (!(e.MenuTemplateNode.Name == "PDM.VisAppScheme"))
      return;
    e.MenuTemplateNode.Text = LocalizationHolder.rm.GetString("Pdm_rv_38") + PDMPlugin.ApplicabilitySchemeName;
  }

  public static long CompositionSchemeId
  {
    get => PDMPlugin._compositionSchemeId;
    set => PDMPlugin._compositionSchemeId = value;
  }

  public static long ApplicabilitySchemeId
  {
    get => PDMPlugin._applicabilitySchemeId;
    set => PDMPlugin._applicabilitySchemeId = value;
  }

  public static string CompositionSchemeName { get; set; }

  public static string ApplicabilitySchemeName { get; set; }

  public static long VisStylesId
  {
    get => PDMPlugin._visStylesId;
    set => PDMPlugin._visStylesId = value;
  }

  public static string VisStylesName { get; set; }

  public static long PreviewMode { get; set; }

  private void pluginManager_LoadComplete(object sender, EventArgs e)
  {
    if (this._serviceProvider.GetService(typeof (IDatabaseConfiguratorService)) is IDatabaseConfiguratorService service)
      PDMPluginConsts.ObjectTypesCategoryID = service.RegisterCategoryProps(4, (ICategoryProps) new ReleaseArticlesEnabledProp());
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      PDMPluginIDs.measureShtuk = sessionKeeper.Session.GetObject(new Guid("cad002e8-306c-11d8-b4e9-00304f19f545")).ObjectID;
      PDMPluginIDs.assemblyUnitTypeID = sessionKeeper.Session.GetObjectType(new Guid("cad00132-306c-11d8-b4e9-00304f19f545")).ObjectType;
      PDMPluginIDs.partTypeID = sessionKeeper.Session.GetObjectType(new Guid("cad00250-306c-11d8-b4e9-00304f19f545")).ObjectType;
      PDMPluginIDs.otherProductsTypeID = sessionKeeper.Session.GetObjectType(new Guid("cad0038d-306c-11d8-b4e9-00304f19f545")).ObjectType;
      PDMPluginIDs.standartProductsTypeID = sessionKeeper.Session.GetObjectType(new Guid("cad00252-306c-11d8-b4e9-00304f19f545")).ObjectType;
      PDMPluginIDs.linkZagotRelaionID = sessionKeeper.Session.GetRelationType(PDMPluginGuids.linkZagotRelationGuid).RelationType;
      IDBObjectType objectType = sessionKeeper.Session.GetObjectType(PDMPluginGuids.orderPointGuid, false);
      if (objectType != null)
        PDMPluginIDs.orderPointTypeID = objectType.ObjectType;
      IDBRelationType relationType = sessionKeeper.Session.GetRelationType(PDMPluginGuids.orderPointCompositionRelationGuid, false);
      if (relationType != null)
        PDMPluginIDs.orderPointCompositionRelationTypeID = relationType.RelationType;
      IDBAttributeType attributeType = sessionKeeper.Session.GetAttributeType(PDMPluginGuids.assemblyUnitRefAttrGuid, false);
      if (attributeType != null)
        PDMPluginIDs.assemblyUnitRefAttrID = attributeType.AttributeID;
    }
    (ServicesManager.GetService(typeof (IAdditionalCompositionFiltrationService)) as IAdditionalCompositionFiltrationService).CreateCommands(PDMPlugin._filtrationService, AdditionalFiltrationToolBarOptions.WithMainMenu | AdditionalFiltrationToolBarOptions.WithNotificationServiceUsing, new Guid("cad005f3-306c-11d8-b4e9-00304f19f545"));
  }

  private void ActivateViewEventHandler(object sender, ActivateViewEventArgs e)
  {
    if (e == null || e.NewSelectedNodes == null || e.NewSelectedNodes.Count != 1 || e.NewSelectedNodes.Count != 1 || !(e.NewSelectedNodes[0] is CompareObjectNodeID) || (e.OldSelectedNodes == null || e.OldSelectedNodes.Count <= 0 ? 0 : (e.OldSelectedNodes[0] is CompareObjectNodeID ? 1 : 0)) != 0)
      return;
    e.NewViewName = "PDM.CompareCompositionView";
  }

  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  public static extern bool DestroyIcon(IntPtr handle);

  private void RegisterIconForNode(INamedImageList nil, string imageName, int category, int type)
  {
    Icon icon = nil?.ImageList.Images[nil.ImageIndex(imageName)] is Bitmap image ? Icon.FromHandle(image.GetHicon()) : (Icon) null;
    if (icon == null)
      return;
    Statics.IconSrv.AddIcon(icon, category, type);
    PDMPlugin.DestroyIcon(icon.Handle);
    icon.Dispose();
  }

  public static void CreateContext(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    long num1 = (items.GetItemData(0, typeof (IDBObjectID)) as IDBObjectID).Value;
    int num2 = (int) ContextCompositionCreatorForm.Execute(items, viewServices);
  }

  public static void CreateExemplar(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    using (ExemplarCreatorDialog exemplarCreatorDialog = new ExemplarCreatorDialog())
    {
      if (!(items.GetItemData(0, typeof (IDBObjectID)) is IDBObjectID itemData))
        return;
      using (SessionKeeper sk = new SessionKeeper())
      {
        IDBObject dbObject = sk.Session.GetObject(itemData.Value, true);
        IDBAttribute attributeByGuid = dbObject.GetAttributeByGuid(PDMHelper.attributeStorageArticle);
        if (attributeByGuid == null || CompareValuesHelper.NormalizedValue(attributeByGuid.Value) == null)
        {
          int num1 = (int) IMMessageBox.Show(LocalizationHolder.rm.GetString("Pdm_550"), string.Format(LocalizationHolder.rm.GetString("Pdm_554"), (object) dbObject.NameInMessages, (object) MetaDataHelper.GetAttributeTypeName(PDMHelper.attributeStorageArticle)), MessageBoxButtons.OK, IMMessageBoxImage.Error);
        }
        else
        {
          if (!exemplarCreatorDialog.SetFormData(sk, itemData.Value, dbObject.ObjectType, (ArticlesInManufacture) Convert.ToInt32(attributeByGuid.Value)))
            return;
          int num2 = (int) exemplarCreatorDialog.ShowDialog();
        }
      }
    }
  }

  public static void CreateInstance(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (PDMPlugin.PluginLocked || !(ServicesManager.GetService(typeof (IObjectCreatorService)) is IObjectCreatorService service))
      return;
    long num = items.GetItemData(0, typeof (IDBObjectID)) is IDBObjectID itemData ? itemData.Value : -1L;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (PDMHelper.Validation3DModelInComposition(sessionKeeper.Session, num))
        throw new Exception(LocalizationHolder.rm.GetString("Pdm_555"));
    }
    service.AfterObjectCreatedEvent += new AfterObjectCreatedEventHandler(PDMPlugin.PerformActionAfterObjectCreated);
    try
    {
      service.CreateObjectByTemplateDialog(num);
    }
    finally
    {
      service.AfterObjectCreatedEvent -= new AfterObjectCreatedEventHandler(PDMPlugin.PerformActionAfterObjectCreated);
    }
  }

  private static void PerformActionAfterObjectCreated(object sender, AfterObjectCreatedEventArgs e)
  {
    int attributeTypeId = MetaDataHelper.GetAttributeTypeID("cad001f9-306c-11d8-b4e9-00304f19f545");
    long objectId = e.ObjectID;
    if (objectId == -1L)
      return;
    List<NotificationEventArgs> notificationEventArgsList = new List<NotificationEventArgs>();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject1 = sessionKeeper.Session.GetObject(e.PrototypeId);
      IDBAttribute attributeByGuid = dbObject1.GetAttributeByGuid(new Guid("cad001f9-306c-11d8-b4e9-00304f19f545"), false);
      Guid numGroupInstance = Guid.Empty;
      if (attributeByGuid == null || !GuidHelper.IsGuid(attributeByGuid.AsString))
      {
        numGroupInstance = Guid.NewGuid();
        Intermech.Pdm.GroupInstances.Helper.AddNumInstance(sessionKeeper.Session, dbObject1, attributeTypeId, numGroupInstance);
      }
      else
        numGroupInstance = new Guid(attributeByGuid.AsString);
      IDBObject dbObject2 = sessionKeeper.Session.GetObject(objectId);
      Intermech.Pdm.GroupInstances.Helper.AddNumInstance(sessionKeeper.Session, dbObject2, attributeTypeId, numGroupInstance);
      notificationEventArgsList.Add((NotificationEventArgs) new DBObjectsEventArgs("ObjectsCreated", objectId));
      IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(MetaDataHelper.GetRelationTypeID(new Guid("cad00154-306c-11d8-b4e9-00304f19f545")));
      DBRecordSetParams paramSet = new DBRecordSetParams((ConditionStructure[]) null, new object[2]
      {
        (object) -20,
        (object) -22
      });
      DataTable dataTable = relationCollection.ConsistFrom(paramSet, e.PrototypeId);
      if (dataTable.Rows.Count > 0)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        {
          long int64_1 = Convert.ToInt64(row[0]);
          long int64_2 = Convert.ToInt64(row[1]);
          if (sessionKeeper.Session.GetRelation(dbObject2.ObjectID, int64_2, relationCollection.RelationTypeID) == null)
          {
            NewRelationProperties properties = new NewRelationProperties(int64_1, dbObject2.ObjectID, int64_2);
            IDBRelation dbRelation = relationCollection.Create(properties);
            notificationEventArgsList.Add((NotificationEventArgs) new DBRelationsEventArgs("RelationsCreated", dbRelation.RelationID));
          }
        }
      }
    }
    foreach (NotificationEventArgs e1 in notificationEventArgsList)
      PDMPlugin._notificationService.FireEvent((object) null, e1);
  }

  public static void RelationVisualizer(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    try
    {
      if (!(items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData))
        return;
      IWellKnownNavigators service = (IWellKnownNavigators) ServicesManager.GetService(typeof (IWellKnownNavigators));
      RelationVisualiserWindow window = (RelationVisualiserWindow) null;
      if (window == null)
      {
        window = new RelationVisualiserWindow();
        service.Register("desktopRelationVisualiserWindow", (Control) window);
      }
      if (window.IsThreadBusy)
      {
        switch (MessageBox.Show(LocalizationHolder.rm.GetString("Pdm_rv_23"), LocalizationHolder.rm.GetString("Pdm_rv_16"), MessageBoxButtons.YesNo, MessageBoxIcon.Question))
        {
          case DialogResult.OK:
            return;
          case DialogResult.Yes:
            if (!window.StopThread())
              return;
            break;
        }
      }
      window.SetCurentObject(itemData);
      window.Show((DockManager) ServicesManager.GetService(typeof (DockManager)));
      window.Activate();
    }
    catch (Exception ex)
    {
      int num = (int) MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }
  }

  public static void LaunchVisualizerComplete(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    PDMPlugin.LaunchVisualizer(items, viewServices, additionalInfo, true, true);
  }

  public static void LaunchVisualizerChilds(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    PDMPlugin.LaunchVisualizer(items, viewServices, additionalInfo, true, false);
  }

  public static void LaunchVisualizerParents(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    PDMPlugin.LaunchVisualizer(items, viewServices, additionalInfo, false, true);
  }

  public static void LaunchVisualizer(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo,
    bool showChilds,
    bool showParents)
  {
    try
    {
      if (!(items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData))
        return;
      IWellKnownNavigators service = (IWellKnownNavigators) ServicesManager.GetService(typeof (IWellKnownNavigators));
      VisView window = (VisView) null;
      if (window == null)
      {
        window = new VisView();
        LCLevelInfoKeeper.Init();
        VisStatusKeeper.Init();
        service.Register("newRelationVisualizerWindow", (Control) window);
        window.PrevMode = PDMPlugin.PreviewMode;
      }
      if (window.IsThreadBusy)
      {
        switch (MessageBox.Show(LocalizationHolder.rm.GetString("Pdm_rv_23"), LocalizationHolder.rm.GetString("Pdm_rv_16"), MessageBoxButtons.YesNo, MessageBoxIcon.Question))
        {
          case DialogResult.OK:
            return;
          case DialogResult.Yes:
            if (!window.StopThread())
              return;
            break;
        }
      }
      if (PDMPlugin.ApplicabilitySchemeId != 0L || PDMPlugin.CompositionSchemeId != 0L)
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          if (PDMPlugin.ApplicabilitySchemeId != 0L && sessionKeeper.Session.GetObjectInfo(PDMPlugin.ApplicabilitySchemeId).Empty)
          {
            PDMPlugin.ApplicabilitySchemeId = 0L;
            PDMPlugin.ApplicabilitySchemeName = LocalizationHolder.rm.GetString("Pdm_rv_39");
          }
          if (PDMPlugin.CompositionSchemeId != 0L)
          {
            if (sessionKeeper.Session.GetObjectInfo(PDMPlugin.CompositionSchemeId).Empty)
            {
              PDMPlugin.CompositionSchemeId = 0L;
              PDMPlugin.CompositionSchemeName = LocalizationHolder.rm.GetString("Pdm_rv_39");
            }
          }
        }
      }
      window.Show((DockManager) ServicesManager.GetService(typeof (DockManager)));
      if (PDMPlugin.VisStylesId != 0L)
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
          window._DoLoadStyle(sessionKeeper.Session, PDMPlugin.VisStylesId);
      }
      else
        window.SetDefaultStyle();
      window.Activate();
      window.LaunchForObject(itemData, showChilds, showParents);
    }
    catch (Exception ex)
    {
      int num = (int) MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }
  }

  public static void ChooseCompScheme(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    string str = VisView.ChooseScheme(true, ref PDMPlugin._compositionSchemeId);
    if (str.Equals(string.Empty))
      return;
    PDMPlugin.CompositionSchemeName = str;
  }

  public static void ChooseAppScheme(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    string str = VisView.ChooseScheme(false, ref PDMPlugin._applicabilitySchemeId);
    if (str.Equals(string.Empty))
      return;
    PDMPlugin.ApplicabilitySchemeName = str;
  }

  public static void ListInstance(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (PDMPlugin.PluginLocked)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBTypedObjectID itemData = items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
      IDBObject dbObject = sessionKeeper.Session.GetObject(itemData.ObjectID, false);
      if (dbObject == null)
        return;
      IDBAttribute attributeByGuid = dbObject.GetAttributeByGuid(new Guid("cad001f9-306c-11d8-b4e9-00304f19f545"));
      Guid numGroupInstance = attributeByGuid == null || !GuidHelper.IsGuid(attributeByGuid.AsString) ? Guid.Empty : new Guid(attributeByGuid.AsString);
      ListInstancesNavWindow instancesNavWindow = new ListInstancesNavWindow();
      instancesNavWindow.Text = PDMPluginConsts.ListInstancesWindow;
      instancesNavWindow.TabImage = PDMPlugin._namedImageList?.ImageList.Images[PDMPlugin._namedImageList.ImageIndex("imgObjects.PDM")];
      instancesNavWindow.TreeView.OnGetSupportedColumnsEventHandler += new GetSupportedColumnsEventHandler(Intermech.Navigator.Utils.GetNavigatorColumns);
      instancesNavWindow.SetDescriptor((IDescriptor) new ListInstancesDescriptor(numGroupInstance, dbObject.ObjectGUID));
      instancesNavWindow.Show((DockManager) ServicesManager.GetService(typeof (DockManager)));
      instancesNavWindow.Activate();
    }
  }

  public static bool ConvertFromItems(
    IUserSession session,
    ISelectedItems items,
    out List<Tuple<long, int>> objIDs,
    out List<Guid> objGuids,
    out List<int> objectTypes,
    out Dictionary<int, bool> relationTypes)
  {
    objIDs = new List<Tuple<long, int>>(items.Count);
    objGuids = new List<Guid>(items.Count);
    objectTypes = new List<int>(items.Count);
    relationTypes = (Dictionary<int, bool>) null;
    if (items.Count == 1)
    {
      IDBTypedObjectID itemData = items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
      QuickObjectInfo objectInfo = session.GetObjectInfo(itemData.ObjectID);
      if (itemData.ObjectID < 0L)
      {
        objIDs.Add(new Tuple<long, int>(itemData.ObjectID, itemData.ObjectType));
        objIDs.Add(new Tuple<long, int>(Math.Abs(itemData.ObjectID), itemData.ObjectType));
        objGuids.Add(objectInfo.VersionGuid);
        objGuids.Add(objectInfo.VersionGuid);
        objectTypes.Add(objectInfo.ObjectTypeID);
      }
      else
      {
        int num = (int) IMMessageBox.Show(MessageDialogs.msgWarning, LocalizationHolder.rm.GetString(sc_16458.ssp_pdm_16462()), MessageBoxButtons.OK, IMMessageBoxImage.Warning);
        return false;
      }
    }
    else
    {
      for (int index = 0; index < items.Count; ++index)
      {
        IDBTypedObjectID itemData = items.GetItemData(index, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
        IDBObject dbObject = session.GetObject(itemData.ObjectID);
        objIDs.Add(new Tuple<long, int>(itemData.ObjectID, itemData.ObjectType));
        objGuids.Add(dbObject.ObjectGUID);
        if (!objectTypes.Contains(itemData.ObjectType))
          objectTypes.Add(itemData.ObjectType);
      }
    }
    relationTypes = CompareHelper.GetOwnRelationTypes(session, objectTypes);
    if (relationTypes.Count != 0)
      return true;
    int num1 = (int) IMMessageBox.Show(MessageDialogs.msgWarning, LocalizationHolder.rm.GetString(sc_16458.ssp_pdm_16463()), MessageBoxButtons.OK, IMMessageBoxImage.Warning);
    return false;
  }

  public static void CompareComposition(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (PDMPlugin.PluginLocked || (DockManager) ServicesManager.GetService(typeof (DockManager)) == null)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      List<Tuple<long, int>> objIDs;
      List<Guid> objGuids;
      Dictionary<int, bool> relationTypes;
      if (!PDMPlugin.ConvertFromItems(sessionKeeper.Session, items, out objIDs, out objGuids, out List<int> _, out relationTypes))
        return;
      CompareObjectsDescriptor rootDescriptor = new CompareObjectsDescriptor(viewServices, objGuids, objIDs, relationTypes);
      CompareNavWindow compareNavWindow = CompareNavWindow.Create();
      compareNavWindow.TabImage = PDMPlugin._namedImageList != null ? PDMPlugin._namedImageList.ImageList.Images[PDMPlugin._namedImageList.ImageIndex("imgObjects.PDM")] : (Image) null;
      compareNavWindow.TreeView.OnGetSupportedColumnsEventHandler += new GetSupportedColumnsEventHandler(Intermech.Navigator.Utils.GetNavigatorColumns);
      compareNavWindow.TreeView.SetColumns(Intermech.Navigator.Utils.CaptionColumnOnly(NodeColumnSortOrder.Ascending));
      compareNavWindow.TreeView.Build((IDescriptor) rootDescriptor);
      compareNavWindow.Show((DockManager) ServicesManager.GetService(typeof (DockManager)));
      compareNavWindow.Activate();
    }
  }

  public static void FillFirstEntersTo(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items.Count != 1)
      return;
    FirstApplicability.FillInComposition((items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID).ObjectID);
  }

  public static void MakeInstance(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      List<long> objectIDs = new List<long>(items.Count);
      int num = -1;
      for (int index = 0; index < items.Count; ++index)
      {
        IDBTypedObjectID itemData = (IDBTypedObjectID) items.GetItemData(index, typeof (IDBTypedObjectID));
        objectIDs.Add(itemData.ObjectID);
        if (index == 0)
          num = itemData.ObjectType;
      }
      int attributeTypeId = MetaDataHelper.GetAttributeTypeID("cad001f9-306c-11d8-b4e9-00304f19f545");
      Guid numGroupInstance = Guid.Empty;
      if (sessionKeeper.Session.GetObjectCollection(num).Select(new DBRecordSetParams(new ConditionStructure[2]
      {
        new ConditionStructure(-2, RelationalOperators.In, (object) objectIDs.ToArray(), LogicalOperators.AND, 0, false),
        new ConditionStructure(attributeTypeId, RelationalOperators.NotEmpty, (object) null, LogicalOperators.AND, 0, false)
      }, new object[1]{ (object) -2 })).Rows.Count > 0)
      {
        object[] objArray = Intermech.Navigator.SelectionWindow.Select("Одно или несколько выделенных изделий уже имеют исполнения. Выберите, исполнениями какого из изделий сделать выделенные изделия.", (IDescriptor) new ListDescriptor(Intermech.Navigator.Consts.CategoryVersionsObjectNode, num, "Выделенные изделия", (IList) objectIDs), typeof (IDBTypedObjectID), SelectionOptions.Default | SelectionOptions.DisableMultiselect);
        if (objArray == null)
          return;
        IDBObject dbObject = sessionKeeper.Session.GetObject(((IDBTypedObjectID) objArray[0]).ObjectID);
        IDBAttribute attributeById = dbObject.GetAttributeByID(attributeTypeId);
        if (!GuidHelper.IsGuid(attributeById.AsString))
        {
          numGroupInstance = Guid.NewGuid();
          Intermech.Pdm.GroupInstances.Helper.AddNumInstance(sessionKeeper.Session, dbObject, attributeTypeId, numGroupInstance);
        }
        else
          numGroupInstance = new Guid(attributeById.AsString);
      }
      else
        numGroupInstance = Guid.NewGuid();
      IDBTransactions customService = (IDBTransactions) sessionKeeper.Session.GetCustomService(typeof (IDBTransactions));
      customService.StartTransaction();
      try
      {
        for (int index = 0; index < objectIDs.Count; ++index)
        {
          IDBObject dbObject = sessionKeeper.Session.GetObject(objectIDs[index]);
          try
          {
            Intermech.Pdm.GroupInstances.Helper.AddNumInstance(sessionKeeper.Session, dbObject, attributeTypeId, numGroupInstance);
          }
          catch (Exception ex)
          {
            customService.Rollback();
            throw new Exception($"Нельзя сделать исполнением {dbObject.NameInMessages}: {ex.Message}");
          }
        }
        customService.Commit();
      }
      catch
      {
        customService.Rollback();
        throw;
      }
      ((INotificationService) ServicesManager.GetService(typeof (INotificationService))).FireEvent((object) null, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", (IList<long>) objectIDs));
    }
  }

  private static void HideComposition(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (PDMPlugin.PluginLocked || items == null || items.Count == 0)
      return;
    List<long> longList1 = new List<long>(0);
    List<long> longList2 = new List<long>(0);
    List<long> objectIDs = new List<long>(0);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!sessionKeeper.Session.Configurations.ReadBool("KERNEL", "PERFORMANCE", "UseHiddenComposition", true, DBConfigMode.GlobalOnly))
      {
        int num = (int) MessageBox.Show("Невозможно выполнить команду. Опция конфиграции 'Использовать функцию скрытия состава' установлена в значение Нет.", "Intermech Professional Solution", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return;
      }
      if (!(sessionKeeper.Session.GetCustomService(typeof (IVersionRulesCacheService)) is IVersionRulesCacheService customService))
        return;
      PDMPlugin._hiddenCompositionObjects = customService[sessionKeeper.Session.UserID, (object) "{9D621C68-0820-47EC-9ABB-CC7D2EF820F6}"] as List<long>;
      if (PDMPlugin._hiddenCompositionObjects == null)
        PDMPlugin._hiddenCompositionObjects = new List<long>(0);
      for (int index = 0; index < items.Count; ++index)
      {
        IDBObjectID itemData1 = items.GetItemData(index, typeof (IDBObjectID)) as IDBObjectID;
        IDBRelationID itemData2 = items.GetItemData(index, typeof (IDBRelationID)) as IDBRelationID;
        if (itemData1 != null && itemData2 != null)
        {
          IDBObject dbObject = sessionKeeper.Session.GetObject(itemData1.Value, false);
          if (dbObject != null)
          {
            long id = dbObject.ID;
            if (!longList2.Contains(id))
            {
              if (PDMPlugin._hiddenCompositionObjects.Contains(id))
                PDMPlugin._hiddenCompositionObjects.Remove(id);
              else
                PDMPlugin._hiddenCompositionObjects.Add(id);
              longList2.Add(id);
              objectIDs.Add(dbObject.ObjectID);
            }
            if (itemData2.Value > 0L && !longList1.Contains(itemData2.Value))
              longList1.Add(itemData2.Value);
          }
        }
      }
      customService[sessionKeeper.Session.UserID, (object) "{9D621C68-0820-47EC-9ABB-CC7D2EF820F6}"] = (object) PDMPlugin._hiddenCompositionObjects;
    }
    if (objectIDs.Count > 0)
    {
      DBObjectsEventArgs e = new DBObjectsEventArgs("ObjectsChanged", (IList<long>) objectIDs);
      if (PDMPlugin._notificationService != null)
        PDMPlugin._notificationService.FireEvent((object) null, (NotificationEventArgs) e);
    }
    if (longList1.Count <= 0)
      return;
    DBRelationsEventArgs e1 = new DBRelationsEventArgs("RelationsChanged", (IList<long>) longList1.ToArray());
    if (PDMPlugin._notificationService == null)
      return;
    PDMPlugin._notificationService.FireEvent((object) null, (NotificationEventArgs) e1);
  }

  private static void InsertTechInComposition(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    try
    {
      AdvancedServiceContainer viewServices1 = new AdvancedServiceContainer(viewServices);
      if (ServicesManager.GetService(typeof (CompositionContextsHolder)) != null)
        ServicesManager.RemoveService(typeof (CompositionContextsHolder));
      if (SingleContextSelectionForm.Execute() != DialogResult.OK)
        return;
      CompositionContextsHolder serviceInstance = new CompositionContextsHolder((IList<long>) new long[1]
      {
        SingleContextSelectionForm.DefaultContext
      });
      ServicesManager.AddService(typeof (CompositionContextsHolder), (object) serviceInstance);
      viewServices1.AddService(typeof (CompositionContextsHolder), (object) serviceInstance);
      ObjectCommands.AddCommand(items, (IServiceProvider) viewServices1, additionalInfo);
    }
    finally
    {
      if (ServicesManager.GetService(typeof (CompositionContextsHolder)) != null)
        ServicesManager.RemoveService(typeof (CompositionContextsHolder));
    }
  }

  private static void InsertAdditionalComplect(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (viewServices != null && viewServices.GetService(typeof (INavigatorTreeViewContextMenuHelper)) is INavigatorTreeViewContextMenuHelper service)
      service.CanRestoreFocusedNode = false;
    IDBTypedObjectID itemData = items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
    IServiceContainer nodesContext = (IServiceContainer) new ServiceContainer();
    IObjectTypeNodeFilter serviceInstance = (IObjectTypeNodeFilter) new ObjectTypeNodeFilter();
    nodesContext.AddService(typeof (IObjectTypeNodeFilter), (object) serviceInstance);
    Hashtable linkTypes = new Hashtable();
    List<int> intList = new List<int>(0);
    int relationTypeId = MetaDataHelper.GetRelationTypeID(new Guid("cadd99d9-306c-11d8-b4e9-00304f19f545"));
    List<int> childObjectTypesId = MetaDataHelper.GetApplicabilityChildObjectTypesID(itemData.ObjectType, relationTypeId);
    linkTypes[(object) itemData.ObjectType] = (object) relationTypeId;
    foreach (int key in childObjectTypesId)
      linkTypes[(object) key] = (object) relationTypeId;
    DescriptorCollection descriptors = new DescriptorCollection()
    {
      (IDescriptor) new ObjectTypesDescriptor(childObjectTypesId.ToArray(), LocalizationHolder.rm.GetString("Pdm_722"))
    };
    Intermech.Navigator.CustomNode.Descriptor rootDescriptor = new Intermech.Navigator.CustomNode.Descriptor(LocalizationHolder.rm.GetString("Pdm_720"), descriptors);
    IDBTypedObjectID[] objectIDs = (IDBTypedObjectID[]) Intermech.Navigator.SelectionWindow.Select(LocalizationHolder.rm.GetString("Pdm_721"), (IDescriptor) rootDescriptor, typeof (IDBTypedObjectID), (IServiceProvider) nodesContext, SelectionOptions.Default | SelectionOptions.ForceFilterObjectsByRule);
    if (objectIDs == null)
      return;
    ObjectCommands.DoInsertIntoObject(items.GetParentPath(0), itemData, objectIDs, (IDBRelationID[]) null, linkTypes, viewServices, NavigatorRelationCommand.InsertIn);
  }

  private static void UpdateSpecificationNotes(
    ISelectedItems items,
    IServiceProvider viewservices,
    object additionalinfo)
  {
    if (!(items.GetItemData(0, typeof (IDBObjectID)) is IDBObjectID itemData))
      return;
    using (SessionKeeper keeper = new SessionKeeper())
    {
      if (!(keeper.Session.GetCustomService(typeof (IOrderPointService)) is IOrderPointService customService))
        return;
      List<long> orderPoints = customService.GetOrderPoints(keeper.Session.SessionGUID, itemData.Value);
      if (orderPoints == null)
      {
        int num = (int) MessageBox.Show(LocalizationHolder.rm.GetString("Pdm_714"), LocalizationHolder.rm.GetString("Pdm_119"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        Dictionary<long, long> deployedCompositionInfo = customService.GetDeployedCompositionInfo(keeper.Session.SessionGUID, itemData.Value);
        if (deployedCompositionInfo == null)
          return;
        PDMPlugin.ClearNotes(keeper, deployedCompositionInfo);
        PDMPlugin.AddNotes(keeper, customService, deployedCompositionInfo, orderPoints);
      }
    }
  }

  private static void AddNotes(
    SessionKeeper keeper,
    IOrderPointService orderPointService,
    Dictionary<long, long> compositionInfo,
    List<long> assemblyUnitPoints)
  {
    foreach (long assemblyUnitPoint in assemblyUnitPoints)
    {
      List<long> pointComposition = orderPointService.GetPointComposition(keeper.Session.SessionGUID, assemblyUnitPoint);
      if (pointComposition != null)
      {
        foreach (long num in pointComposition)
        {
          if (compositionInfo.Values.Contains<long>(num))
          {
            string asString = keeper.Session.GetObject(assemblyUnitPoint).GetAttributeByGuid(new Guid("cad0001f-306c-11d8-b4e9-00304f19f545")).AsString;
            foreach (KeyValuePair<long, long> keyValuePair in compositionInfo)
            {
              if (keyValuePair.Value == num)
              {
                long key = keyValuePair.Key;
                IDBAttribute attributeByGuid = keeper.Session.GetRelation(key).GetAttributeByGuid(new Guid("cad00021-306c-11d8-b4e9-00304f19f545"));
                if (attributeByGuid != null)
                  attributeByGuid.AsString = PDMPlugin.FormNote(attributeByGuid.AsString, asString);
              }
            }
          }
        }
      }
    }
  }

  private static string FormNote(string oldNote, string pointDesignation)
  {
    string str1 = "По заказу \"";
    string str2 = "\". ";
    string str3;
    if (string.IsNullOrWhiteSpace(oldNote))
      str3 = str1 + pointDesignation + str2;
    else if (!oldNote.Contains(str1))
    {
      str3 = str1 + pointDesignation + str2 + oldNote;
    }
    else
    {
      int startIndex = oldNote.IndexOf(str1) + str1.Length;
      str3 = oldNote.Insert(startIndex, pointDesignation + ", ");
    }
    return str3;
  }

  private static void ClearNotes(SessionKeeper keeper, Dictionary<long, long> compositionInfo)
  {
    foreach (KeyValuePair<long, long> keyValuePair in compositionInfo)
    {
      long key = keyValuePair.Key;
      IDBAttribute attributeByGuid = keeper.Session.GetRelation(key).GetAttributeByGuid(new Guid("cad00021-306c-11d8-b4e9-00304f19f545"));
      keeper.Session.GetRelation(key).GetAttributeByGuid(new Guid("cad00021-306c-11d8-b4e9-00304f19f545"));
      string asString = attributeByGuid.AsString;
      if (asString.Contains("По заказу \""))
      {
        string str1 = asString.Remove(asString.IndexOf("По заказу \""));
        string str2 = asString.Substring(asString.IndexOf("По заказу \""));
        string str3 = str2.Remove(0, str2.IndexOf("\". ") + "\". ".Length);
        attributeByGuid.AsString = str1 + str3;
      }
    }
  }

  private static void AddToOrderPoint(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalinfo)
  {
    long rootObjectId = PDMPlugin.FindRootObjectID(viewServices);
    if (rootObjectId == -1L || !(items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData))
      return;
    using (SessionKeeper keeper = new SessionKeeper())
    {
      long orderPointId = PDMPlugin.GetOrderPointID(keeper, rootObjectId);
      if (orderPointId == -1L)
        return;
      PDMPlugin.AddItemToOrderPoint(keeper.Session, viewServices, orderPointId, itemData);
    }
  }

  private static void AddMaterialToOrderPoint(
    IUserSession session,
    long orderPointID,
    IDBTypedObjectID selectedTypedObj,
    NotificationService notificationService)
  {
    long asInteger = session.GetObject(selectedTypedObj.ObjectID).GetAttributeByGuid(new Guid("cad0038c-306c-11d8-b4e9-00304f19f545")).AsInteger;
    if (asInteger == 0L)
      return;
    IDBRelation dbRelation = session.GetRelationCollection(PDMPluginIDs.linkZagotRelaionID).Create(orderPointID, asInteger);
    DBRelationsEventArgs e = new DBRelationsEventArgs("RelationsCreated", dbRelation.RelationID, dbRelation.ProjID, PDMPluginIDs.orderPointTypeID, dbRelation.RelationType, NavigatorRelationCommand.CreateIn);
    notificationService?.FireEvent((object) null, (NotificationEventArgs) e);
  }

  private static bool IsAddToOrderPointCommandNeedToBeShown(
    IServiceProvider viewServices,
    IDBTypedObjectID selectedObj,
    bool isInTree)
  {
    return viewServices.GetService(typeof (NavigatorTreeView)) is NavigatorTreeView service && isInTree && !service.FocusedNode.Equals((object) service.RootNode) && service.RootNodeID.TypeID != PDMPluginIDs.orderPointTypeID && PDMPlugin.IsTypeAppropriateToBeAddToOrderPoint(selectedObj.ObjectType) && !PDMPlugin.IsNodeHasOrderPointInParents(service.FocusedNode, service.RootNode);
  }

  private static long GetOrderPointID(SessionKeeper keeper, long rootObjectID)
  {
    if (!(keeper.Session.GetCustomService(typeof (IOrderPointService)) is IOrderPointService customService))
      return -1;
    List<long> orderPoints = customService.GetOrderPoints(keeper.Session.SessionGUID, rootObjectID);
    if (orderPoints == null)
    {
      int num = (int) MessageBox.Show(LocalizationHolder.rm.GetString("Pdm_714"), LocalizationHolder.rm.GetString("Pdm_119"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      return -1;
    }
    long selectedPointId;
    using (OrderPointSelectionForm pointSelectionForm = new OrderPointSelectionForm(orderPoints))
    {
      int num = (int) pointSelectionForm.ShowDialog();
      selectedPointId = pointSelectionForm.SelectedPointID;
    }
    if (selectedPointId == 0L)
      return -1;
    if (keeper.Session.GetObject(selectedPointId, false).CheckoutBy == keeper.Session.UserID)
      return selectedPointId;
    int num1 = (int) MessageBox.Show("Для добавления объекта в точку заказа необходимо взять её на изменение.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    return -1;
  }

  private static void AddItemToOrderPoint(
    IUserSession session,
    IServiceProvider viewServices,
    long chosedOrderPointID,
    IDBTypedObjectID selectedObject)
  {
    if (!(viewServices.GetService(typeof (NavigatorTreeView)) is NavigatorTreeView service1))
      return;
    IDBRelation relation = session.GetRelationCollection(PDMPluginIDs.orderPointCompositionRelationTypeID).Create(chosedOrderPointID, selectedObject.ObjectID);
    if (relation == null)
      return;
    PDMPlugin.WriteAssemblyUnitRefAttr(relation, service1);
    PDMPlugin.WriteCountAttr(session, relation, service1);
    NotificationService service2 = viewServices.GetService(typeof (INotificationService)) as NotificationService;
    DBRelationsEventArgs e = new DBRelationsEventArgs("RelationsCreated", relation.RelationID, relation.ProjID, PDMPluginIDs.orderPointTypeID, relation.RelationType, NavigatorRelationCommand.CreateIn);
    service2?.FireEvent((object) null, (NotificationEventArgs) e);
    if (selectedObject.ObjectType != PDMPluginIDs.partTypeID && !MetaDataHelper.GetObjectTypeChildrenID(PDMPluginIDs.partTypeID).Contains(selectedObject.ObjectType))
      return;
    PDMPlugin.AddMaterialToOrderPoint(session, chosedOrderPointID, selectedObject, service2);
  }

  private static void WriteCountAttr(
    IUserSession session,
    IDBRelation relation,
    NavigatorTreeView currentNavigatorTree)
  {
    IDBAttribute attributeByGuid1 = relation.GetAttributeByGuid(new Guid("cad00267-306c-11d8-b4e9-00304f19f545"));
    NavigatorTreeNode navigatorTreeNode = currentNavigatorTree.FocusedNode;
    double aValue = 1.0;
    NavigatorTreeNode parent;
    for (; !navigatorTreeNode.Equals((object) currentNavigatorTree.RootNode); navigatorTreeNode = parent)
    {
      parent = navigatorTreeNode.Parent;
      IDBTypedObjectID itemData1 = parent.NodeAsSelectedItem.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
      IDBTypedObjectID itemData2 = navigatorTreeNode.NodeAsSelectedItem.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
      if (itemData1 == null || itemData2 == null)
      {
        aValue = 0.0;
        break;
      }
      IDBAttribute attributeByGuid2 = session.GetRelation(itemData1.ObjectID, itemData2.ID, session.GetRelationType(new Guid("cad00023-306c-11d8-b4e9-00304f19f545")).RelationType).GetAttributeByGuid(new Guid("cad00267-306c-11d8-b4e9-00304f19f545"));
      if (attributeByGuid2 == null)
      {
        aValue = 0.0;
        break;
      }
      aValue *= attributeByGuid2.AsDouble;
    }
    attributeByGuid1.Value = (object) new MeasuredValue(aValue, PDMPluginIDs.measureShtuk);
  }

  private static void WriteAssemblyUnitRefAttr(
    IDBRelation relation,
    NavigatorTreeView currentNavigatorTree)
  {
    IDBAttribute attributeByGuid = relation.GetAttributeByGuid(PDMPluginGuids.assemblyUnitRefAttrGuid);
    if (!(currentNavigatorTree.FocusedNode.Parent.NodeAsSelectedItem.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData) || attributeByGuid == null)
      return;
    if (itemData.ObjectID < 0L)
      attributeByGuid.Value = (object) -itemData.ObjectID;
    else
      attributeByGuid.Value = (object) itemData.ObjectID;
  }

  private static long FindRootObjectID(IServiceProvider viewServices)
  {
    return !(viewServices.GetService(typeof (NavigatorTreeView)) is NavigatorTreeView service) || !(service.RootNode.NodeID is NodeID nodeId) ? -1L : nodeId.ObjectID;
  }

  private static bool IsTypeAppropriateToBeAddToOrderPoint(int selectedObjectType)
  {
    return selectedObjectType == PDMPluginIDs.assemblyUnitTypeID || MetaDataHelper.GetObjectTypeChildrenID(PDMPluginIDs.assemblyUnitTypeID).Contains(selectedObjectType) || selectedObjectType == PDMPluginIDs.partTypeID || MetaDataHelper.GetObjectTypeChildrenID(PDMPluginIDs.partTypeID).Contains(selectedObjectType) || selectedObjectType == PDMPluginIDs.otherProductsTypeID || MetaDataHelper.GetObjectTypeChildrenID(PDMPluginIDs.otherProductsTypeID).Contains(selectedObjectType) || selectedObjectType == PDMPluginIDs.standartProductsTypeID || MetaDataHelper.GetObjectTypeChildrenID(PDMPluginIDs.standartProductsTypeID).Contains(selectedObjectType);
  }

  private static bool IsNodeHasOrderPointInParents(
    NavigatorTreeNode focusedNode,
    NavigatorTreeNode rootNode)
  {
    if (focusedNode.Equals((object) rootNode) || !(focusedNode.Parent.NodeAsSelectedItem.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData))
      return false;
    return itemData.ObjectType == PDMPluginIDs.orderPointTypeID || PDMPlugin.IsNodeHasOrderPointInParents(focusedNode.Parent, rootNode);
  }

  private static void FireObjectsCreated(long objectID)
  {
    if (PDMPlugin._notificationService == null)
      return;
    PDMPlugin._notificationService.FireEvent((object) null, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsCreated", objectID));
  }

  public void SetDescriptorStatuses(object sender, SetDescriptorStatusesEventArgs e)
  {
    if (!(e.RootDescriptor.RootDescriptor is Intermech.Navigator.DBObjects.Descriptor rootDescriptor) || PDMPlugin._elementStatusesClientService == null)
      return;
    if (PDMPlugin._hiddenCompositionObjects.Count == 0)
    {
      PDMPlugin._elementStatusesClientService.SetElementStatuses16("cad005fe-306c-11d8-b4e9-00304f19f545", e.RootDescriptor.Statuses, (short) 0);
    }
    else
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(rootDescriptor.ObjectID);
        if (objectInfo.Empty)
          return;
        if (PDMPlugin._hiddenCompositionObjects.Contains(objectInfo.ID))
          PDMPlugin._elementStatusesClientService.SetElementStatuses16("cad005fe-306c-11d8-b4e9-00304f19f545", e.RootDescriptor.Statuses, (short) 1);
        else
          PDMPlugin._elementStatusesClientService.SetElementStatuses16("cad005fe-306c-11d8-b4e9-00304f19f545", e.RootDescriptor.Statuses, (short) 0);
      }
    }
  }

  internal static int CheckHiddenCompositionItems(
    ISelectedItems items,
    IServiceProvider viewServices)
  {
    bool flag1 = false;
    bool flag2 = false;
    if (!PDMPlugin.CheckSelectedItemsAppls(items, viewServices) || items == null || items.Count == 0 || PDMPlugin._hiddenCompositionObjects == null)
      return -1;
    if (PDMPlugin._hiddenCompositionObjects.Count == 0)
      return 0;
    for (int index = 0; index < items.Count; ++index)
    {
      if (!(items.GetItemData(index, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData) || MetaDataHelper.GetApplicabilityRelationTypesID(itemData.ObjectType).Count == 0)
        return -1;
      bool flag3 = PDMPlugin._hiddenCompositionObjects.Contains(itemData.ID);
      if (index == 0)
      {
        flag1 = flag3;
        flag2 = !flag3;
      }
      flag1 = flag3 & flag1;
      flag2 = !flag3 & flag2;
      if (flag1 == flag2)
        return -1;
    }
    return flag1 ? 1 : 0;
  }

  internal static bool CheckSelectedItemsAppls(ISelectedItems items, IServiceProvider viewServices)
  {
    if (items == null || items.Count == 0)
      return false;
    for (int index = 0; index < items.Count; ++index)
    {
      if (!(items.GetItemData(index, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData) || MetaDataHelper.GetObjectType(itemData.ObjectType) == null)
        return false;
    }
    return true;
  }

  internal static int CheckSelectedItems(ISelectedItems items, IServiceProvider viewServices)
  {
    if (items == null || items.Count == 0)
      return -4;
    if (!(items.GetParentData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID parentData))
      return -3;
    if (!MetaDataHelper.HasObjectTypeSubstRelTypes(parentData.ObjectType))
      return -2;
    if (!(items.GetItemData(0, typeof (IDBRelationID)) is IDBRelationID itemData))
      return -1;
    string empty = string.Empty;
    int relationType = itemData.RelationType;
    if (relationType == -1 || !MetaDataHelper.HasRelationTypeSubstitutes(relationType))
      return -1;
    int count = items.Count;
    return 2;
  }

  internal static void UpdateHiddenCompositions(bool fireEvent = true)
  {
    if (PDMPlugin.PluginLocked || PDMPlugin._filtrationService == null || PDMPlugin._IsInEvent)
      return;
    PDMPlugin._filtrationService.FiltrationApplyUpdates(fireEvent);
  }

  internal void OnFiltrationChanged(IFiltrationSettings NewFiltration, bool FiltrationValid)
  {
    if (PDMPlugin.PluginLocked || PDMPlugin._IsInEvent)
      return;
    bool isInEvent = PDMPlugin._IsInEvent;
    if (PDMPlugin._filtrationService == null)
      return;
    try
    {
      PDMPlugin._IsInEvent = true;
      if (PDMPlugin._filtrationService.Filtration.Tags[(object) "{AB419A02-DE8A-4A8E-905A-D782F5B720E5}"] is List<long>)
        return;
      List<long> list = ((IEnumerable<CompositionContext>) CompositionContextClientHelper.GetDefaultCompositionContexts().CompositionContexts).Select<CompositionContext, long>((System.Func<CompositionContext, long>) (o => o.Value)).ToList<long>();
      PDMPlugin._filtrationService.Filtration.Tags[(object) "{AB419A02-DE8A-4A8E-905A-D782F5B720E5}"] = (object) list;
    }
    finally
    {
      PDMPlugin._IsInEvent = isInEvent;
    }
  }

  private void previewExtender_Extend(ExtendEventArgs eventArgs)
  {
    if (eventArgs == null || eventArgs.ObjectID == -1L)
      return;
    int rootType = ObjectTypesCacheHelper.GetRootType(eventArgs.ObjectType);
    IDBObjectTypeInfo objectType = (ServicesManager.GetService(typeof (IClientMetadataCache)) as IClientMetadataCache).GetObjectType(new Guid("cad00268-306c-11d8-b4e9-00304f19f545"), false);
    if (objectType == null || rootType != objectType.ObjectType)
      return;
    IArticleService service = (IArticleService) ServicesManager.GetService(typeof (IArticleService));
    if (service == null)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject mainDocument = service.FindMainDocument(eventArgs.ObjectID, PDMPlugin._filtrationService.FiltrationServiceOwnerID, (object) sessionKeeper.Session);
      if (mainDocument == null)
        return;
      IDBAttributeType attributeType = sessionKeeper.Session.GetAttributeType(new Guid("cad0004b-306c-11d8-b4e9-00304f19f545"), false);
      if (attributeType == null)
        return;
      IDBAttribute attributeById = mainDocument.GetAttributeByID(attributeType.AttributeID);
      if (attributeById == null)
        return;
      FileBlobItem fileBlobItem1 = new FileBlobItem(mainDocument.ObjectID, attributeType.AttributeID, 0);
      eventArgs.Items.Add(fileBlobItem1);
      eventArgs.PreferedBlobID = attributeById.AsInteger;
      long[] mainDocuments = service.FindMainDocuments(eventArgs.ObjectID, PDMPlugin._filtrationService.FiltrationServiceOwnerID, (object) sessionKeeper.Session);
      if (mainDocuments == null || mainDocuments.Length == 0)
        return;
      foreach (long objectId in mainDocuments)
      {
        if (objectId != mainDocument.ObjectID)
        {
          FileBlobItem fileBlobItem2 = new FileBlobItem(objectId, attributeType.AttributeID, 0);
          eventArgs.Items.Add(fileBlobItem2);
        }
      }
    }
  }

  private void LogMessage(string category, string message)
  {
    if (this.outputView == null)
      return;
    this.LogMessageCore(category, message);
  }

  private void LogError(string category, string errorMessage)
  {
    if (this.outputView == null)
      return;
    this.LogMessageCore(category, errorMessage);
    this.outputView.ShowView();
    this.outputView.Activate(category);
  }

  private void LogMessageCore(string category, string message)
  {
    foreach (string text in message.Split(PDMPlugin.LineSeparators, StringSplitOptions.None))
      this.outputView.WriteString(category, text);
  }
}
