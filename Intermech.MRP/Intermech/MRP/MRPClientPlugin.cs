// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.MRPClientPlugin
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using ImSSP;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Configuration;
using Intermech.Interfaces.MRP;
using Intermech.Interfaces.Plugins;
using Intermech.Localization;
using Intermech.MRP.Orders;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using Intermech.Protection;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MRP;

/// <summary>Плагин Intermech.MRP</summary>
internal class MRPClientPlugin : IPackage, IConfigurable
{
  /// <summary>Guid плагина</summary>
  private static Guid _pluginGuid = new Guid("{7E1F8750-8BC0-4EA0-B238-1B40EE26C243}");
  /// <summary>
  /// Может ли плагин предоставлять возможности, доступные в русскоязычных странах
  /// (например, отображать странички и колонки, связанные с расшифровкой допустимых замен, т.п.)
  /// </summary>
  private static bool _canUseRussianFeatures = false;
  /// <summary>Является ли текущий пользователь администратором</summary>
  private static bool _isUserAdmin = false;
  /// <summary>
  /// Если данное свойство равно true, все механизмы плагина должны быть заблокированы
  /// </summary>
  internal static bool PluginLocked = false;
  /// <summary>Коллекция именованных значков</summary>
  private static INamedImageList _namedImageList = (INamedImageList) null;
  /// <summary>Коллекция изображений для разных категорий</summary>
  private static ICategoryTypeIconService _objtypesIcons = (ICategoryTypeIconService) null;
  /// <summary>Служба уведомлений Навигатора</summary>
  private static INotificationService _notifySvc = (INotificationService) null;

  /// <summary>Guid плагина</summary>
  internal static Guid PluginGuid => MRPClientPlugin._pluginGuid;

  /// <summary>Имя плагина</summary>
  public string Name
  {
    [DebuggerStepThrough] get => LocalizationHolder.rm.GetString("MRP_20");
  }

  /// <summary>Выполнить инициализацию плагина</summary>
  /// <param name="serviceProvider">Контейнер сервисов</param>
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
    if (service1 != null)
    {
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
      MRPClientPlugin._namedImageList = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
      MRPClientPlugin._objtypesIcons = ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
      MRPClientPlugin._notifySvc = ServicesManager.GetService(typeof (INotificationService)) as INotificationService;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        MRPClientPlugin._isUserAdmin = sessionKeeper.Session.IsAdmin;
        MRPClientPlugin._canUseRussianFeatures = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName == "ru";
        IMRPServerPlugin mrpServerPlugin1 = (IMRPServerPlugin) null;
        IMRPServerPlugin mrpServerPlugin2;
        try
        {
          mrpServerPlugin2 = sessionKeeper.Session.GetCustomService(typeof (IMRPServerPlugin)) as IMRPServerPlugin;
        }
        catch
        {
          mrpServerPlugin2 = (IMRPServerPlugin) null;
        }
        MRPClientPlugin.PluginLocked = mrpServerPlugin2 == null;
        mrpServerPlugin1 = (IMRPServerPlugin) null;
        if (!MRPClientPlugin.PluginLocked)
        {
          MRPSettings serviceInstance = new MRPSettings(sessionKeeper.Session);
          if (!(ServicesManager.GetService(typeof (IMRPSettings)) is IMRPSettings))
            ServicesManager.AddService(typeof (IMRPSettings), (object) serviceInstance);
          CompositionsAutosortRule.OnGetVisibleRelations += new CompositionsGetVisibleRelationsEventHandler(ClientAutosortRuleEvents.CompositionsGetVisibleRelationsEventHandler);
          CompositionsAutosortRule.OnGetVisibleRelationsGuids += new CompositionsGetVisibleRelationsGuidEventHandler(ClientAutosortRuleEvents.CompositionsGetVisibleRelationsGuidEventHandler);
          IObjectCreatorService service2 = ServicesManager.GetService(typeof (IObjectCreatorService)) as IObjectCreatorService;
          int objectTypeId = MetaDataHelper.GetObjectTypeID("cadd92e9-306c-11d8-b4e9-00304f19f545");
          IFactory service3 = ServicesManager.GetService(typeof (IFactory)) as IFactory;
          service3.AddViewsProvider(1, (IViewsProvider) new OrdersViewProvider());
          service2.RegisterCreatorCustomService(objectTypeId, typeof (ManufactOrdersCreator));
          service2.AfterObjectCreatedEvent += new AfterObjectCreatedEventHandler(ManufactOrdersAPI.OnObjectCreatorCompleatedEventHandler);
          MenuTemplate contextMenuTemplate = service3.ContextMenuTemplate;
          contextMenuTemplate.BeginUpdate();
          try
          {
            int imageIndex1 = MRPClientPlugin._objtypesIcons != null ? MRPClientPlugin._objtypesIcons.IndexOf(4, MetaDataHelper.GetObjectTypeID("cadd92e9-306c-11d8-b4e9-00304f19f545")) : -1;
            int imageIndex2 = MRPClientPlugin._objtypesIcons != null ? MRPClientPlugin._objtypesIcons.IndexOf(4, MetaDataHelper.GetObjectTypeID("cad00583-306c-11d8-b4e9-00304f19f545")) : -1;
            int imageIndex3 = MRPClientPlugin._objtypesIcons != null ? MRPClientPlugin._objtypesIcons.IndexOf(4, MetaDataHelper.GetObjectTypeID("cad00268-306c-11d8-b4e9-00304f19f545")) : -1;
            int imageIndex4 = MRPClientPlugin._objtypesIcons != null ? MRPClientPlugin._objtypesIcons.IndexOf(4, MetaDataHelper.GetObjectTypeID("cad0016f-306c-11d8-b4e9-00304f19f545")) : -1;
            int imageIndex5 = MRPClientPlugin._namedImageList != null ? MRPClientPlugin._namedImageList.ImageIndex("MRP.imgBoughtArticle") : -1;
            MenuTemplateNode node = new MenuTemplateNode("[MRP]", LocalizationHolder.rm.GetString("MRP_53"), imageIndex1, 19000, 10, Keys.None, true, ImageListSource.CategoryImageList);
            contextMenuTemplate.Nodes.Add(node);
            node.Nodes.Add(new MenuTemplateNode("MRP.ProcessOrder", LocalizationHolder.rm.GetString("MRP_54"), imageIndex1, 10, 10, Keys.None, true, ImageListSource.CategoryImageList));
            node.Nodes.Add(new MenuTemplateNode("MRP.ChangeInstanceVersion", LocalizationHolder.rm.GetString("MRP_55"), imageIndex2, 20, 10, Keys.None, true, ImageListSource.CategoryImageList));
            node.Nodes.Add(new MenuTemplateNode("MRP.ChangeInstance", LocalizationHolder.rm.GetString("MRP_56"), imageIndex3, 20, 20, Keys.None, true, ImageListSource.CategoryImageList));
            node.Nodes.Add(new MenuTemplateNode("MRP.ChangeTechRoute", LocalizationHolder.rm.GetString("MRP_57"), imageIndex4, 20, 30, Keys.None, true, ImageListSource.CategoryImageList));
            node.Nodes.Add(new MenuTemplateNode("MRP.MakeBought", LocalizationHolder.rm.GetString("MRP_58"), imageIndex5, 30, 10, Keys.None, true, ImageListSource.NamedImageList));
            node.Nodes.Add(new MenuTemplateNode("MRP.ChangeVersion", LocalizationHolder.rm.GetString("MRP_59"), -1, 40, 10, Keys.None));
          }
          finally
          {
            contextMenuTemplate.EndUpdate();
          }
          MenuTemplateNode menuTemplateNode = service3.ContextMenuTemplate[sc_14777.ssp_mrp_14778()];
          if (menuTemplateNode != null)
          {
            contextMenuTemplate.BeginUpdate();
            try
            {
              int imageIndex = MRPClientPlugin._objtypesIcons != null ? MRPClientPlugin._objtypesIcons.IndexOf(4, MetaDataHelper.GetObjectTypeID("cadd92e9-306c-11d8-b4e9-00304f19f545")) : -1;
              menuTemplateNode.Nodes.Add(new MenuTemplateNode("MRP.CreateManufactOrder", LocalizationHolder.rm.GetString("MRP_49"), imageIndex, 15, 25, Keys.None, true, ImageListSource.CategoryImageList));
            }
            finally
            {
              contextMenuTemplate.EndUpdate();
            }
          }
          ICommandsProvider provider1 = (ICommandsProvider) new ManufactOrdersCommandsProvider();
          service3.AddCommandsProvider(1, MetaDataHelper.GetObjectTypeID("cad00268-306c-11d8-b4e9-00304f19f545"), provider1);
          service3.AddCommandsProvider(1, MetaDataHelper.GetObjectTypeID("cad015b1-306c-11d8-b4e9-00304f19f545"), provider1);
          service3.AddCommandsProvider(1, MetaDataHelper.GetObjectTypeID("cad00580-306c-11d8-b4e9-00304f19f545"), provider1);
          ProcessManufactOrdersCommandsProvider provider2 = new ProcessManufactOrdersCommandsProvider();
          service3.AddCommandsProvider(1, MetaDataHelper.GetObjectTypeID("cadd92e9-306c-11d8-b4e9-00304f19f545"), (ICommandsProvider) provider2);
          service3.AddCommandsProvider(1, MetaDataHelper.GetObjectTypeID("cad00583-306c-11d8-b4e9-00304f19f545"), (ICommandsProvider) provider2);
          service3.AddCommandsProvider(1, MetaDataHelper.GetObjectTypeID("cad0016f-306c-11d8-b4e9-00304f19f545"), (ICommandsProvider) provider2);
          service3.AddCommandsProvider(1, MetaDataHelper.GetObjectTypeID("cad00163-306c-11d8-b4e9-00304f19f545"), (ICommandsProvider) provider2);
          service3.AddCommandsProvider(1, MetaDataHelper.GetObjectTypeID("cad00268-306c-11d8-b4e9-00304f19f545"), (ICommandsProvider) provider2);
          service3.AddCommandsProvider(1, MetaDataHelper.GetObjectTypeID("cad015b1-306c-11d8-b4e9-00304f19f545"), (ICommandsProvider) provider2);
          if (MRPClientPlugin._notifySvc != null)
            MRPClientPlugin._notifySvc.Subscribe("RelationsCreated", new NotificationEventHandler(ManufactOrdersAPI.NotificationEventFired));
        }
      }
      this.LoadPluginResources(serviceProvider);
    }
    else
      MRPClientPlugin.PluginLocked = true;
  }

  /// <summary>Выгрузка модуля расширения</summary>
  public void Unload()
  {
    (ServicesManager.GetService(typeof (ILicenser)) as ILicenser).ReleaseLicense(347);
  }

  /// <summary>Выполнить загрузку настроек плагина</summary>
  /// <param name="configurationManager">Менеджер настроек</param>
  public void LoadConfiguration(IConfigurationManager configurationManager)
  {
  }

  /// <summary>Выполнить сохранение настроек плагина</summary>
  /// <param name="configurationManager">Менеджер настроек</param>
  public void SaveConfiguration(IConfigurationManager configurationManager)
  {
  }

  /// <summary>Считать ресурс в массив байт</summary>
  /// <param name="ResourceName">Имя ресурса</param>
  /// <returns>Массив байт</returns>
  internal static byte[] LoadResource(string ResourceName)
  {
    Stream stream = (Stream) null;
    try
    {
      stream = typeof (MRPClientPlugin).Assembly.GetManifestResourceStream(ResourceName);
      if (stream == null)
        return new byte[0];
      byte[] buffer = new byte[stream.Length];
      stream.Read(buffer, 0, buffer.Length);
      return buffer;
    }
    finally
    {
      stream?.Close();
    }
  }

  /// <summary>Загрузить ресурсы плагина</summary>
  /// <param name="serviceProvider">Коллекция сервисов</param>
  private void LoadPluginResources(IServiceProvider serviceProvider)
  {
    if (MRPClientPlugin.PluginLocked || MRPClientPlugin._namedImageList == null)
      return;
    Stream manifestResourceStream = this.GetType().Assembly.GetManifestResourceStream("Intermech.MRP.Resources.MRPBitmaps.bmp");
    if (manifestResourceStream == null)
      return;
    using (Bitmap images = new Bitmap(manifestResourceStream))
    {
      images.MakeTransparent();
      MRPClientPlugin._namedImageList.AddStrip((Image) images, new string[4]
      {
        "MRP.imgConfigurator",
        "MRP.imgSubstitutes",
        "MRP.imgTechRoutes",
        "MRP.imgBoughtArticle"
      });
    }
    manifestResourceStream.Close();
  }
}
