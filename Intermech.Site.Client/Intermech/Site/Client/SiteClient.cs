// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.SiteClient
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using ImSSP;
using Intermech.Bars;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Client.WebPortal;
using Intermech.Interfaces.Configuration;
using Intermech.Interfaces.Plugins;
using Intermech.Interfaces.SelectionService;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.NavBars;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using Intermech.PropertyEditors;
using Intermech.Protection;
using Intermech.Site.Client.Cache;
using Intermech.Site.Client.PortalNavigator;
using Intermech.Site.Client.Selections;
using Intermech.Site.Client.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client;

public class SiteClient : IPackage, IConfigurable
{
  private MetaDataCache _cache;
  private IServiceProvider _serviceProvider;
  private int _objtypeRemoteSelections = -1;

  public void Load(IServiceProvider serviceProvider)
  {
    int appId = 363;
    byte[][] numArray = new byte[32 /*0x20*/][]
    {
      new byte[16 /*0x10*/]
      {
        (byte) 126,
        (byte) 53,
        (byte) 155,
        (byte) 22,
        (byte) 25,
        (byte) 76,
        (byte) 83,
        (byte) 15,
        (byte) 215,
        (byte) 56,
        (byte) 22,
        (byte) 175,
        (byte) 232,
        (byte) 221,
        (byte) 76,
        (byte) 115
      },
      new byte[16 /*0x10*/]
      {
        (byte) 79,
        (byte) 161,
        (byte) 106,
        (byte) 132,
        (byte) 131,
        (byte) 230,
        (byte) 70,
        (byte) 254,
        (byte) 246,
        (byte) 169,
        (byte) 196,
        (byte) 254,
        (byte) 184,
        (byte) 79,
        (byte) 73,
        (byte) 70
      },
      new byte[16 /*0x10*/]
      {
        (byte) 20,
        (byte) 143,
        (byte) 27,
        (byte) 204,
        (byte) 81,
        (byte) 176 /*0xB0*/,
        (byte) 192 /*0xC0*/,
        (byte) 187,
        (byte) 13,
        (byte) 33,
        (byte) 135,
        (byte) 138,
        (byte) 90,
        (byte) 90,
        (byte) 222,
        (byte) 152
      },
      new byte[16 /*0x10*/]
      {
        (byte) 206,
        (byte) 11,
        (byte) 94,
        (byte) 81,
        (byte) 112 /*0x70*/,
        (byte) 235,
        (byte) 176 /*0xB0*/,
        (byte) 71,
        (byte) 12,
        (byte) 33,
        (byte) 186,
        (byte) 248,
        (byte) 214,
        (byte) 60,
        (byte) 190,
        (byte) 242
      },
      new byte[16 /*0x10*/]
      {
        (byte) 113,
        (byte) 244,
        (byte) 22,
        (byte) 172,
        (byte) 21,
        (byte) 100,
        (byte) 166,
        (byte) 56,
        (byte) 192 /*0xC0*/,
        (byte) 1,
        (byte) 221,
        (byte) 57,
        (byte) 23,
        (byte) 169,
        (byte) 228,
        (byte) 109
      },
      new byte[16 /*0x10*/]
      {
        (byte) 62,
        (byte) 231,
        (byte) 68,
        (byte) 37,
        (byte) 97,
        (byte) 67,
        (byte) 44,
        (byte) 162,
        (byte) 34,
        (byte) 243,
        (byte) 180,
        (byte) 132,
        (byte) 155,
        (byte) 237,
        (byte) 5,
        (byte) 210
      },
      new byte[16 /*0x10*/]
      {
        (byte) 165,
        (byte) 211,
        (byte) 114,
        (byte) 134,
        (byte) 109,
        (byte) 103,
        (byte) 97,
        (byte) 205,
        (byte) 116,
        (byte) 224 /*0xE0*/,
        (byte) 9,
        (byte) 248,
        (byte) 197,
        (byte) 201,
        (byte) 180,
        (byte) 125
      },
      new byte[16 /*0x10*/]
      {
        (byte) 62,
        (byte) 119,
        (byte) 206,
        (byte) 204,
        (byte) 31 /*0x1F*/,
        (byte) 43,
        (byte) 100,
        (byte) 142,
        (byte) 91,
        (byte) 143,
        (byte) 207,
        (byte) 141,
        (byte) 179,
        (byte) 185,
        (byte) 27,
        (byte) 150
      },
      new byte[16 /*0x10*/]
      {
        (byte) 90,
        (byte) 126,
        (byte) 61,
        (byte) 243,
        (byte) 199,
        (byte) 160 /*0xA0*/,
        (byte) 94,
        (byte) 176 /*0xB0*/,
        (byte) 232,
        (byte) 227,
        (byte) 12,
        (byte) 79,
        (byte) 211,
        (byte) 134,
        (byte) 163,
        (byte) 100
      },
      new byte[16 /*0x10*/]
      {
        (byte) 106,
        (byte) 55,
        (byte) 46,
        (byte) 183,
        (byte) 50,
        (byte) 36,
        (byte) 44,
        (byte) 71,
        (byte) 181,
        (byte) 27,
        (byte) 245,
        (byte) 53,
        (byte) 137,
        (byte) 194,
        (byte) 93,
        (byte) 204
      },
      new byte[16 /*0x10*/]
      {
        (byte) 60,
        (byte) 25,
        (byte) 229,
        (byte) 135,
        (byte) 231,
        (byte) 87,
        (byte) 42,
        (byte) 9,
        (byte) 146,
        (byte) 73,
        (byte) 55,
        (byte) 53,
        (byte) 91,
        (byte) 174,
        (byte) 115,
        (byte) 246
      },
      new byte[16 /*0x10*/]
      {
        (byte) 39,
        (byte) 31 /*0x1F*/,
        (byte) 81,
        (byte) 213,
        (byte) 54,
        (byte) 148,
        (byte) 193,
        (byte) 186,
        (byte) 192 /*0xC0*/,
        (byte) 216,
        (byte) 247,
        (byte) 233,
        (byte) 128 /*0x80*/,
        (byte) 247,
        (byte) 56,
        (byte) 38
      },
      new byte[16 /*0x10*/]
      {
        (byte) 77,
        (byte) 96 /*0x60*/,
        (byte) 223,
        (byte) 152,
        (byte) 108,
        (byte) 240 /*0xF0*/,
        (byte) 166,
        (byte) 4,
        (byte) 117,
        (byte) 194,
        (byte) 158,
        (byte) 138,
        (byte) 91,
        (byte) 18,
        (byte) 249,
        (byte) 238
      },
      new byte[16 /*0x10*/]
      {
        (byte) 146,
        (byte) 121,
        (byte) 114,
        (byte) 189,
        (byte) 208 /*0xD0*/,
        (byte) 183,
        (byte) 199,
        (byte) 248,
        (byte) 42,
        (byte) 69,
        (byte) 92,
        (byte) 157,
        (byte) 243,
        (byte) 153,
        (byte) 12,
        (byte) 17
      },
      new byte[16 /*0x10*/]
      {
        (byte) 60,
        (byte) 165,
        (byte) 187,
        (byte) 171,
        (byte) 227,
        (byte) 45,
        (byte) 150,
        (byte) 2,
        (byte) 83,
        (byte) 51,
        (byte) 96 /*0x60*/,
        (byte) 126,
        (byte) 105,
        (byte) 41,
        (byte) 84,
        (byte) 57
      },
      new byte[16 /*0x10*/]
      {
        (byte) 213,
        (byte) 150,
        (byte) 121,
        (byte) 224 /*0xE0*/,
        (byte) 20,
        (byte) 79,
        (byte) 181,
        (byte) 127 /*0x7F*/,
        (byte) 70,
        (byte) 55,
        (byte) 30,
        (byte) 156,
        (byte) 170,
        (byte) 113,
        (byte) 134,
        (byte) 191
      },
      new byte[16 /*0x10*/]
      {
        (byte) 111,
        (byte) 21,
        (byte) 2,
        (byte) 115,
        (byte) 166,
        (byte) 11,
        (byte) 248,
        (byte) 76,
        (byte) 115,
        (byte) 224 /*0xE0*/,
        (byte) 17,
        (byte) 36,
        (byte) 185,
        (byte) 159,
        (byte) 174,
        (byte) 128 /*0x80*/
      },
      new byte[16 /*0x10*/]
      {
        (byte) 20,
        (byte) 70,
        (byte) 230,
        (byte) 235,
        (byte) 187,
        (byte) 191,
        (byte) 184,
        (byte) 48 /*0x30*/,
        (byte) 69,
        (byte) 37,
        (byte) 101,
        (byte) 222,
        (byte) 147,
        (byte) 186,
        (byte) 127 /*0x7F*/,
        (byte) 197
      },
      new byte[16 /*0x10*/]
      {
        (byte) 79,
        (byte) 72,
        (byte) 249,
        (byte) 182,
        (byte) 242,
        (byte) 199,
        (byte) 19,
        (byte) 31 /*0x1F*/,
        (byte) 202,
        (byte) 221,
        (byte) 69,
        (byte) 56,
        (byte) 143,
        (byte) 39,
        (byte) 155,
        (byte) 76
      },
      new byte[16 /*0x10*/]
      {
        (byte) 135,
        (byte) 195,
        (byte) 95,
        (byte) 10,
        (byte) 141,
        (byte) 252,
        (byte) 42,
        (byte) 68,
        (byte) 10,
        (byte) 202,
        (byte) 170,
        (byte) 130,
        (byte) 5,
        (byte) 205,
        (byte) 228,
        (byte) 233
      },
      new byte[16 /*0x10*/]
      {
        (byte) 110,
        (byte) 100,
        (byte) 26,
        (byte) 70,
        (byte) 16 /*0x10*/,
        (byte) 134,
        (byte) 71,
        (byte) 49,
        (byte) 172,
        (byte) 111,
        (byte) 56,
        (byte) 3,
        (byte) 28,
        (byte) 113,
        (byte) 72,
        (byte) 179
      },
      new byte[16 /*0x10*/]
      {
        (byte) 156,
        (byte) 6,
        (byte) 170,
        (byte) 3,
        (byte) 11,
        (byte) 103,
        (byte) 194,
        (byte) 233,
        (byte) 100,
        (byte) 61,
        (byte) 59,
        (byte) 94,
        (byte) 63 /*0x3F*/,
        (byte) 222,
        (byte) 240 /*0xF0*/,
        (byte) 81
      },
      new byte[16 /*0x10*/]
      {
        (byte) 133,
        (byte) 224 /*0xE0*/,
        (byte) 47,
        (byte) 32 /*0x20*/,
        (byte) 47,
        (byte) 158,
        (byte) 71,
        (byte) 168,
        (byte) 2,
        (byte) 226,
        (byte) 169,
        (byte) 201,
        (byte) 197,
        (byte) 191,
        (byte) 188,
        (byte) 75
      },
      new byte[16 /*0x10*/]
      {
        (byte) 81,
        (byte) 183,
        (byte) 252,
        (byte) 96 /*0x60*/,
        (byte) 125,
        (byte) 10,
        (byte) 143,
        (byte) 13,
        (byte) 99,
        (byte) 165,
        (byte) 110,
        (byte) 143,
        (byte) 98,
        (byte) 243,
        (byte) 197,
        (byte) 180
      },
      new byte[16 /*0x10*/]
      {
        (byte) 68,
        (byte) 196,
        (byte) 182,
        (byte) 84,
        (byte) 132,
        (byte) 104,
        (byte) 111,
        (byte) 200,
        (byte) 0,
        (byte) 200,
        (byte) 9,
        (byte) 130,
        (byte) 142,
        (byte) 15,
        (byte) 2,
        (byte) 88
      },
      new byte[16 /*0x10*/]
      {
        (byte) 243,
        (byte) 50,
        (byte) 137,
        (byte) 73,
        (byte) 131,
        (byte) 226,
        (byte) 252,
        (byte) 127 /*0x7F*/,
        (byte) 61,
        (byte) 196,
        (byte) 102,
        (byte) 109,
        (byte) 205,
        (byte) 192 /*0xC0*/,
        (byte) 74,
        (byte) 169
      },
      new byte[16 /*0x10*/]
      {
        (byte) 148,
        (byte) 145,
        (byte) 178,
        (byte) 175,
        (byte) 248,
        (byte) 123,
        (byte) 28,
        (byte) 208 /*0xD0*/,
        (byte) 29,
        (byte) 114,
        (byte) 68,
        (byte) 45,
        (byte) 221,
        (byte) 125,
        (byte) 107,
        (byte) 237
      },
      new byte[16 /*0x10*/]
      {
        (byte) 232,
        (byte) 241,
        (byte) 156,
        (byte) 22,
        (byte) 152,
        (byte) 158,
        (byte) 155,
        (byte) 217,
        (byte) 51,
        (byte) 224 /*0xE0*/,
        (byte) 118,
        (byte) 126,
        (byte) 54,
        (byte) 139,
        (byte) 93,
        (byte) 199
      },
      new byte[16 /*0x10*/]
      {
        (byte) 179,
        (byte) 237,
        (byte) 190,
        (byte) 87,
        (byte) 187,
        (byte) 244,
        (byte) 47,
        (byte) 29,
        (byte) 112 /*0x70*/,
        (byte) 102,
        (byte) 31 /*0x1F*/,
        (byte) 103,
        (byte) 148,
        (byte) 196,
        (byte) 25,
        (byte) 242
      },
      new byte[16 /*0x10*/]
      {
        (byte) 242,
        (byte) 86,
        (byte) 239,
        (byte) 11,
        (byte) 8,
        (byte) 204,
        (byte) 197,
        (byte) 8,
        (byte) 247,
        (byte) 0,
        (byte) 228,
        (byte) 137,
        (byte) 60,
        (byte) 119,
        (byte) 161,
        (byte) 36
      },
      new byte[16 /*0x10*/]
      {
        (byte) 223,
        (byte) 80 /*0x50*/,
        (byte) 53,
        (byte) 52,
        (byte) 13,
        (byte) 12,
        (byte) 82,
        (byte) 201,
        (byte) 168,
        (byte) 230,
        (byte) 55,
        (byte) 18,
        (byte) 36,
        (byte) 66,
        (byte) 234,
        (byte) 130
      },
      new byte[16 /*0x10*/]
      {
        (byte) 53,
        (byte) 79,
        (byte) 62,
        (byte) 135,
        (byte) 253,
        (byte) 21,
        (byte) 197,
        (byte) 178,
        (byte) 92,
        (byte) 219,
        (byte) 75,
        (byte) 94,
        (byte) 73,
        (byte) 130,
        (byte) 16 /*0x10*/,
        (byte) 180
      }
    };
    ((ILicenser) ServicesManager.GetService(typeof (ILicenser))).AllocateLicense(appId);
    if (!(serviceProvider.GetService(typeof (IProtectionKey)) is IProtectionKey))
      return;
    this._serviceProvider = serviceProvider;
    MenuTemplate contextMenuTemplate = ((IFactory) serviceProvider.GetService(typeof (IFactory))).ContextMenuTemplate;
    ServicesManager.GetService(typeof (BarManager));
    this._cache = new MetaDataCache();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!((ISiteServerService) sessionKeeper.Session.GetCustomService(typeof (ISiteServerService))).Initialized)
      {
        string text = LocalizationHolder.rm.GetString("Site.Client_92");
        int num = (int) MessageBox.Show(text, this.Name, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        if (!(ServicesManager.GetService(typeof (IOutputView)) is IOutputView service))
          return;
        service.WriteString(LocalizationHolder.rm.GetString("Site.Client_93"), text);
      }
      else
      {
        ISitesCacheService customService = (ISitesCacheService) sessionKeeper.Session.GetCustomService(typeof (ISitesCacheService));
        IDBObject dbObject = sessionKeeper.Session.GetObject(sessionKeeper.Session.UserID);
        if (sessionKeeper.Session.GetRelationCollection(sessionKeeper.Session.IdentHelper.SimpleRelationTypeID).Select(new DBRecordSetParams((ConditionStructure[]) null, new object[1]
        {
          (object) -20
        }), customService.Info.ID, dbObject.ID, DateTime.Now).Rows.Count == 0)
        {
          string text = string.Format(LocalizationHolder.rm.GetString("Site.Client_94"), (object) sessionKeeper.Session.UserName, (object) customService.Info.Caption);
          int num = (int) MessageBox.Show(text, this.Name, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          if (!(ServicesManager.GetService(typeof (IOutputView)) is IOutputView service))
            return;
          service.WriteString(LocalizationHolder.rm.GetString(sc_18497.ssp_webportal_18498()), text);
        }
        else
        {
          ServicesManager.AddService(typeof (IPublicationService), (object) new PublicationService());
          ServicesManager.AddService(typeof (ISaveDiskPublishOptionsDialogService), (object) new SaveDiskPublishOptionsDialogService(Environment.GetFolderPath(Environment.SpecialFolder.Personal)));
          this.LoadServices(sessionKeeper.Session, serviceProvider);
        }
      }
    }
  }

  private void LoadServices(IUserSession session, IServiceProvider serviceProvider)
  {
    IFactory service1 = (IFactory) this._serviceProvider.GetService(typeof (IFactory));
    MenuTemplate contextMenuTemplate = service1.ContextMenuTemplate;
    BarManager service2 = ServicesManager.GetService(typeof (BarManager)) as BarManager;
    Helper.Init(session);
    IPortalConnector customService1 = (IPortalConnector) session.GetCustomService(typeof (IPortalConnector));
    IGuidMapper service3 = (IGuidMapper) this._serviceProvider.GetService(typeof (IGuidMapper));
    SiteClientConsts.CategoryPublishType = service3.Register(SiteClientConsts.CategoryPublishTypeGuid);
    SiteClientConsts.CategoryPublishObject = service3.Register(SiteClientConsts.CategoryPublishObjectGuid);
    SiteClientConsts.CategoryPortalSelection = service3.Register(SiteClientConsts.CategoryPortalSelectionGuid);
    SiteClientConsts.CategoryPublishPacket = service3.Register(SiteClientConsts.CategoryPublishPacketGuid);
    this._cache.Load(session);
    ServicesManager.AddService(typeof (IPortalMetadata), (object) this._cache);
    IColumnSchemes service4 = (IColumnSchemes) ServicesManager.GetService(typeof (IColumnSchemes));
    service4.Register(SiteClientConsts.PublishObjectTypeColumnSchemeGuid, (INodeColumnScheme) new PublishObjectTypeColumnScheme());
    service4.Register(SiteClientConsts.PublishObjectObligatoryColumnSchemeGuid, (INodeColumnScheme) new PublishedObjectObligatoryColumnScheme());
    service4.Register(SiteClientConsts.PublishRelationColumnSchemeGuid, (INodeColumnScheme) new PublishRelationColumnScheme());
    service4.Register(SiteClientConsts.PublishUserObligatoryColumnSchemeGuid, (INodeColumnScheme) new PublishedUserObligatoryColumnScheme());
    service4.Register(SiteClientConsts.PublishPacketsObligatoryColumnSchemeGuid, (INodeColumnScheme) new PublishedPacketObligatoryColumnScheme());
    SiteClientConsts.CategoryPortal = service3.Register(SiteClientConsts.CategoryPortalGuid);
    service1.AddNodeType(SiteClientConsts.CategoryPortal, typeof (PortalRootNode));
    service1.AddViewsProvider(SiteClientConsts.CategoryPortal, (IViewsProvider) new PortalViewProvider());
    SiteClientConsts.CategoryRootPublishType = service3.Register(SiteClientConsts.CategoryRootPublishTypeGuid);
    RootNodeChildren.Descriptors.Add(SiteClientConsts.CategoryRootPublishTypeGuid, (IDescriptor) new PublishTypeRootDescriptor(SiteClientConsts.CategoryRootPublishType, SiteClientConsts.objtypeToPublishGuid));
    SiteClientConsts.CategoryRootPacketType = service3.Register(SiteClientConsts.CategoryRootPacketTypeGuid);
    RootNodeChildren.Descriptors.Add(SiteClientConsts.CategoryRootPacketTypeGuid, (IDescriptor) new PacketTypeRootDescriptor());
    service1.AddNodeType(SiteClientConsts.CategoryPublishType, typeof (PublishTypeNode));
    service1.AddViewsProvider(SiteClientConsts.CategoryPublishType, (IViewsProvider) new PublishTypeViewProvider());
    SiteClientConsts.CategoryListSites = service3.Register(SiteClientConsts.CategoryListSitesGuid);
    RootNodeChildren.AdminDescriptors.Add(SiteClientConsts.CategoryListSitesGuid, (IDescriptor) new ListSitesDescriptor());
    service1.AddNodeType(SiteClientConsts.CategoryListSites, typeof (ListSitesNode));
    service1.AddViewsProvider(SiteClientConsts.CategoryListSites, (IViewsProvider) new ListSitesViewProvider());
    SiteClientConsts.CategorySiteNode = service3.Register(SiteClientConsts.CategorySiteNodeGuid);
    service1.AddNodeType(SiteClientConsts.CategorySiteNode, typeof (SiteNode));
    service1.AddViewsProvider(SiteClientConsts.CategorySiteNode, (IViewsProvider) new SiteViewProvider());
    SiteClientConsts.CategoryUserNode = service3.Register(SiteClientConsts.CategoryUserNodeGuid);
    service1.AddNodeType(SiteClientConsts.CategoryUserNode, typeof (UserNode));
    service1.AddCommandsProvider(SiteClientConsts.CategoryUserNode, (ICommandsProvider) new UserCommandsProvider());
    SiteClientConsts.CategoryRootListPublishObjects = service3.Register(SiteClientConsts.CategoryRootListPublishObjectsGuid);
    service1.AddNodeType(SiteClientConsts.CategoryRootListPublishObjects, typeof (PublishCompositionNode));
    ICategoryTypeIconService service5 = (ICategoryTypeIconService) ServicesManager.GetService(typeof (ICategoryTypeIconService));
    if (service5 != null)
    {
      service5.AddIcon(Intermech.Site.Client.Properties.Resources.portal, SiteClientConsts.CategoryPortal);
      Icon icon = service5.GetIcon(4, MetaDataHelper.GetObjectTypeID(PortalConsts.objtypeSites));
      service5.AddIcon(icon, SiteClientConsts.CategoryListSites, 0);
      service5.AddIcon(icon, SiteClientConsts.CategorySiteNode, 0);
      service5.AddIcon(service5.GetIcon(4, MetaDataHelper.GetObjectTypeID("cad00002-306c-11d8-b4e9-00304f19f545")), SiteClientConsts.CategoryUserNode, 0);
    }
    service1.AddNodeType(SiteClientConsts.CategoryPublishObject, typeof (PublishedObjectNode));
    service1.AddViewsProvider(SiteClientConsts.CategoryPublishObject, (IViewsProvider) new PublishedObjectViewProvider());
    service1.AddViewsProvider(1, MetaDataHelper.GetObjectTypeID(PortalConsts.objtypePortalSelections), (IViewsProvider) new PublishSelectionViewProvider());
    service1.AddViewsProvider(1, MetaDataHelper.GetObjectTypeID(PortalConsts.objtypeReceipt), (IViewsProvider) new ReceiptViewProvider());
    service1.AddNodeType(SiteClientConsts.CategoryPublishPacket, typeof (PacketNode));
    service1.AddViewsProvider(SiteClientConsts.CategoryPublishPacket, (IViewsProvider) new PacketViewProvider());
    SiteClientConsts.CategoryContains = service3.Register(SiteClientConsts.CategoryContainsGuid);
    service1.AddNodeType(SiteClientConsts.CategoryContains, typeof (CompositionNode));
    contextMenuTemplate.BeginUpdate();
    INamedImageList service6 = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
    QuickObjectInfo objectInfo = session.GetObjectInfo(PortalConsts.objectPortalAdminRole);
    bool flag = session.RoleID == objectInfo.ObjectID || session.IsAdmin;
    bool isOffline = customService1.IsOffline;
    try
    {
      IWellKnownWindowsOpenService service7 = ServicesManager.GetService(typeof (IWellKnownWindowsOpenService)) as IWellKnownWindowsOpenService;
      INavigationBar service8 = (INavigationBar) this._serviceProvider.GetService(typeof (INavigationBar));
      if (!isOffline)
      {
        MenuBarItem menuBar = service2.MenuBar.FindMenuBar(sc_18497.ssp_imclient_18499());
        int imageIndex = service6 != null ? service6.ImageIndex("imgPortal") : -1;
        if (menuBar != null)
        {
          MenuButtonItem menuButtonItem1 = new MenuButtonItem(SiteClientConsts.PortalCaption);
          menuButtonItem1.CommandName = SiteClientConsts.CommandShowPortalName;
          menuButtonItem1.ImageIndex = imageIndex;
          MenuButtonItem menuButtonItem2 = menuButtonItem1;
          menuButtonItem2.Click += new EventHandler(this.ShowPortal);
          menuBar.Items.Add((ToolbarItemBase) menuButtonItem2);
        }
        if (service8 != null && service8.FindPane("appPane") is IAppPane pane)
        {
          pane.Add(SiteClientConsts.PortalCaption, new EventHandler(this.ShowPortal), imageIndex);
          service7?.RegisterWindowOpeningHandler("portalWindow", new EventHandler(this.ShowPortal));
        }
      }
      MenuBarItem menuBar1 = service2.MenuBar.FindMenuBar("mnService");
      if (menuBar1 != null)
      {
        if (flag)
        {
          MenuButtonItem menuButtonItem3 = new MenuButtonItem(SiteClientConsts.PublishTypesSettingsCaption);
          menuButtonItem3.CommandName = SiteClientConsts.CommandPublishTypesSettings;
          MenuButtonItem menuButtonItem4 = menuButtonItem3;
          menuButtonItem4.Click += new EventHandler(this.PublishTypesSettings);
          menuBar1.Items.Add((ToolbarItemBase) menuButtonItem4);
        }
        MenuButtonItem menuButtonItem5 = new MenuButtonItem(SiteClientConsts.AutoPublishOblectsListCaption);
        menuButtonItem5.CommandName = SiteClientConsts.AutoPublishOblectsListCommand;
        MenuButtonItem menuButtonItem6 = menuButtonItem5;
        menuButtonItem6.Click += new EventHandler(this.ShowAutoPublishOblectsList);
        menuBar1.Items.Add((ToolbarItemBase) menuButtonItem6);
        if (service8 != null && service8.FindPane("appPane") is IAppPane pane)
        {
          pane.Add(SiteClientConsts.AutoPublishOblectsListCaption, new EventHandler(this.ShowAutoPublishOblectsList), -1);
          service7?.RegisterWindowOpeningHandler(Commands.AutoPublishList, new EventHandler(this.ShowAutoPublishOblectsList));
        }
        if (isOffline)
        {
          MenuButtonItem menuButtonItem7 = new MenuButtonItem("Офлайн импорт");
          menuButtonItem7.CommandName = SiteClientConsts.CommandOfflineImport;
          menuButtonItem7.Click += new EventHandler(OfflineImport.OnImport);
          menuBar1.Items.Add((ToolbarItemBase) menuButtonItem7);
        }
      }
      if (service6 != null)
      {
        service6.Add(Intermech.Site.Client.Properties.Resources.export, SiteClientConsts.ImageExportName);
        service6.Add(Intermech.Site.Client.Properties.Resources.import, SiteClientConsts.ImageImportName);
      }
      contextMenuTemplate.Nodes.Add(new MenuTemplateNode(SiteClientConsts.CommandStartTask, SiteClientConsts.CommandStartTaskCaption, -1, 20, 60));
      contextMenuTemplate.Nodes.Add(new MenuTemplateNode(SiteClientConsts.CommandTaskIncludes, SiteClientConsts.CommandTaskIncludesCaption, -1, 20, 61));
      contextMenuTemplate.Nodes.Add(new MenuTemplateNode(SiteClientConsts.CommandToPublishName, SiteClientConsts.CommandToPublishCaption, service6 != null ? service6.ImageIndex(SiteClientConsts.ImageExportName) : -1, 20, 40));
      contextMenuTemplate.Nodes.Add(new MenuTemplateNode(SiteClientConsts.CommandSetEnterPoint, SiteClientConsts.CommandSetEnterPointCaption, -1, 20, 42));
      contextMenuTemplate.Nodes.Add(new MenuTemplateNode(SiteClientConsts.CommandPublishTableLinks, SiteClientConsts.CommandPublishTableLinksCaption, -1, 20, 45));
      contextMenuTemplate.Nodes.Add(new MenuTemplateNode(SiteClientConsts.CommandEndAutoPublish, SiteClientConsts.CommandEndAutoPublishCaption, -1, 20, 47));
      if (!isOffline)
      {
        contextMenuTemplate.Nodes.Add(new MenuTemplateNode(SiteClientConsts.CommandOwnComplete, SiteClientConsts.CommandOwnCompleteCaption, -1, 20, 50));
        contextMenuTemplate.Nodes.Add(new MenuTemplateNode(SiteClientConsts.CommandImport, SiteClientConsts.CommandImportCaption, service6 != null ? service6.ImageIndex(SiteClientConsts.ImageImportName) : -1, 29991, 10));
        contextMenuTemplate.Nodes.Add(new MenuTemplateNode(SiteClientConsts.CommandDelete, SiteClientConsts.CommandDeleteCaption, -1, 29991, 20));
        contextMenuTemplate.Nodes.Add(new MenuTemplateNode(SiteClientConsts.CommandAutoImportComplete, SiteClientConsts.CommandAutoImportCompleteCaption, -1, 29991, 30));
        service1.AddCommandsProvider(SiteClientConsts.CategoryPublishObject, (ICommandsProvider) new PublishObjectsCommandsProvider());
        service1.AddCommandsProvider(SiteClientConsts.CategoryPublishPacket, (ICommandsProvider) new PacketCommandsProvider());
      }
      service1.AddCommandsProvider(1, (ICommandsProvider) new ObjectsCommandsProvider());
      service1.AddCommandsProvider(1, Intermech.Imbase.Consts.ImbaseCatalogTypeID, (ICommandsProvider) new ImbaseCatalogCommandsProvider());
      service1.AddCommandsProvider(1, Intermech.Imbase.Consts.ImbaseFolderTypeID, (ICommandsProvider) new ImbaseCatalogCommandsProvider());
      int objectTypeId = MetaDataHelper.GetObjectTypeID(new Guid("cad0149e-306c-11d8-b4e9-00304f19f545"));
      if (objectTypeId != -1)
      {
        service1.AddNodeType(4, objectTypeId, typeof (TaskNode));
        service1.AddCommandsProvider(1, objectTypeId, (ICommandsProvider) new TasksCommandsProvider());
      }
    }
    finally
    {
      contextMenuTemplate.EndUpdate();
    }
    if (ServicesManager.GetService(typeof (IAttributePropertyDescriberService)) is IAttributePropertyDescriberService service9)
    {
      PublishTypeAttrDescriber typeAttrDescriber = new PublishTypeAttrDescriber();
      int attributeId1 = session.GetAttributeType(PortalConsts.attributePublishObjTypeGuid).AttributeID;
      if (service9.GetDescriber(attributeId1) == null)
        service9.RegisterDescriber(attributeId1, (IAttributePropertyDescriber) typeAttrDescriber);
      int attributeId2 = session.GetAttributeType(PortalConsts.attributePortalObjectTypes).AttributeID;
      if (service9.GetDescriber(attributeId2) == null)
        service9.RegisterDescriber(attributeId2, (IAttributePropertyDescriber) typeAttrDescriber);
    }
    if (!isOffline)
      PortalSelectionCreator.Attach((IObjectCreatorService) ServicesManager.GetService(typeof (IObjectCreatorService)));
    (ServicesManager.GetService(typeof (IViewsManagerService)) as IViewsManagerService).OnActivateView += new Intermech.Interfaces.Client.ActivateViewEventHandler(this.ActivateViewEventHandler);
    IPropertyPagesService service10 = (IPropertyPagesService) this._serviceProvider.GetService(typeof (IPropertyPagesService));
    if (service10 != null)
    {
      if (!isOffline)
        service10.AddPage(LocalizationHolder.rm.GetString("Site.Client_100"), (IPropertyPage) new PortalPropertiesPage(this._serviceProvider));
      if (flag)
      {
        if (!isOffline)
        {
          service10.AddPage(LocalizationHolder.rm.GetString("Site.Client_107"), (IPropertyPage) new ImportSettingsPropertiesPage());
          service10.AddPage("Сервисы портала\\Настройки публикации", (IPropertyPage) new ExportSettingsPropertiesPage());
        }
        service10.AddPage(LocalizationHolder.rm.GetString("Site.Client_99"), (IPropertyPage) new NotificationsSettings());
      }
    }
    IConditionEditorAttributeService service11 = (IConditionEditorAttributeService) ServicesManager.GetService(typeof (IConditionEditorAttributeService));
    SiteClient.ConditionEditorSiteID handler = new SiteClient.ConditionEditorSiteID();
    foreach (Guid siteCodeAttribute in PortalConsts.SiteCodeAttributes)
      service11.Register(siteCodeAttribute, (IConditionEditorAttribute) handler);
    IStatusBar service12 = (IStatusBar) serviceProvider.GetService(typeof (IStatusBar));
    ISitesCacheService customService2 = (ISitesCacheService) session.GetCustomService(typeof (ISitesCacheService));
    if (customService2 != null && customService2.Info != null)
    {
      StatusBarPanel statusBarPanel = new StatusBarPanel()
      {
        Text = isOffline ? customService2.Info.Caption + " (Офлайн режим)" : customService2.Info.Caption,
        AutoSize = StatusBarPanelAutoSize.Contents,
        ToolTipText = LocalizationHolder.rm.GetString("Site.Client_101")
      };
      int num = service12.StatusBar.Panels.IndexOfKey("sbpRole");
      service12.StatusBar.Panels.Insert(num + 1, statusBarPanel);
    }
    ((ICurrentUserAndRole) ServicesManager.GetService(typeof (ICurrentUserAndRole))).PortalClient = true;
    ServicesManager.GetService<IConditionControllersService>().RegisterController((IConditionController) new PortalAttributeConditionController());
    ServicesManager.GetService<IConditionDataProviderService>().Register(SelectionDataSource.Portal, (IConditionDataProvider) new PortalConditionDataProvider());
  }

  private void ShowPortal(object sender, EventArgs e) => Commands.ShowPortal(this._serviceProvider);

  private void ShowAutoPublishOblectsList(object sender, EventArgs e)
  {
    Commands.ShowAutoPublishOblectsList(this._serviceProvider);
  }

  private void ActivateViewEventHandler(object sender, ActivateViewEventArgs e)
  {
    if (this._objtypeRemoteSelections == -1)
      this._objtypeRemoteSelections = MetaDataHelper.GetObjectTypeID(PortalConsts.objtypePortalSelections);
    if (e == null || e.NewSelectedNodes == null || e.NewSelectedNodes.Count == 0)
      return;
    bool flag = e.OldSelectedNodes != null && e.OldSelectedNodes.Count > 0 && e.OldSelectedNodes[0].CategoryID == 1 && e.OldSelectedNodes[0].TypeID == this._objtypeRemoteSelections;
    int num = e.NewSelectedNodes[0].CategoryID != 1 ? 0 : (e.NewSelectedNodes[0].TypeID == this._objtypeRemoteSelections ? 1 : 0);
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

  public void Unload()
  {
  }

  public string Name => SiteClientConsts.PluginName;

  private void Test(object sender, EventArgs e)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      ((IPortalConnector) sessionKeeper.Session.GetCustomService(typeof (IPortalConnector))).SelectPublishObjectsFlt(sessionKeeper.Session.SessionGUID, 1510, new string[5]
      {
        "cad00029-306c-11d8-b4e9-00304f19f545",
        "cad0001f-306c-11d8-b4e9-00304f19f545",
        "cad00020-306c-11d8-b4e9-00304f19f545",
        "cad014cf-306c-11d8-b4e9-00304f19f545",
        "-5"
      }, 10000, new string[1]
      {
        "cad0001f-306c-11d8-b4e9-00304f19f545"
      }, new int[1]{ 9 }, new string[1]{ "TEST" }, (string[]) null, new int[1]
      {
        2
      }, (int[]) null, new bool[1]);
  }

  private void PublishTypesSettings(object sender, EventArgs e)
  {
    using (PublishTypesSettingsForm typesSettingsForm = new PublishTypesSettingsForm())
    {
      typesSettingsForm.RebuildTree();
      int num = (int) typesSettingsForm.ShowDialog();
    }
  }

  public void LoadConfiguration(IConfigurationManager configurationManager)
  {
  }

  public void SaveConfiguration(IConfigurationManager configurationManager)
  {
  }

  private class ConditionEditorSiteID : IConditionEditorAttribute
  {
    public SelectionParameterTypes NodeValueType => SelectionParameterTypes.sptSiteID;

    public RelationalOperators[] Operators
    {
      get
      {
        return new RelationalOperators[7]
        {
          RelationalOperators.Empty,
          RelationalOperators.NotExistsOrEmpty,
          RelationalOperators.Equal,
          RelationalOperators.NotEmpty,
          RelationalOperators.NotEqual,
          RelationalOperators.AttributeExists,
          RelationalOperators.Substring
        };
      }
    }
  }
}
