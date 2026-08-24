// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.ExternalSystemIntegrationClientPlugin
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using Intermech.Bars;
using Intermech.ExternalSystemIntegration.Interfaces;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Plugins;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client;

public class ExternalSystemIntegrationClientPlugin : IPackage
{
  public void Load(IServiceProvider serviceProvider)
  {
    bool isAdmin;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetCustomService(typeof (ICommonSettingsHolder)) is ICommonSettingsHolder))
      {
        int num = (int) MessageBox.Show(ServiceHolder.rm.GetString("ExtInt_22"), ServiceHolder.rm.GetString("ExtInt_23"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return;
      }
      isAdmin = sessionKeeper.Session.IsAdmin;
    }
    ExternalSystemIntegrationClientPlugin.InitializeServiceHolder(serviceProvider);
    ExternalSystemIntegrationClientPlugin.PopulateNamedImageList();
    ExternalSystemIntegrationClientPlugin.AddCustomViews();
    if (ServiceHolder.ObjectCreatorService != null)
    {
      ServiceHolder.ObjectCreatorService.RegisterCreatorCustomService(Const.ResponceSchemeObjTypeID, typeof (ResponceSchemeCreator));
      ServiceHolder.ObjectCreatorService.RegisterCreatorCustomService(Const.RequestSchemeObjTypeID, typeof (RequestSchemeCreator));
      ServiceHolder.ObjectCreatorService.RegisterCreatorCustomService(Const.RequestObjTypeID, typeof (RequestCreatorCustomService));
      ObjectTypeSettingCreator.Attach(ServiceHolder.ObjectCreatorService);
    }
    if (isAdmin)
    {
      ServiceHolder.PropertyPagesService.AddPage(ServiceHolder.rm.GetString("ExtInt_11"), (IPropertyPage) new CommonSettingsControl());
      ServiceHolder.PropertyPagesService.AddPage(ServiceHolder.rm.GetString("ExtInt_12"), (IPropertyPage) new ObjTypeSettingsControl());
      ServiceHolder.PropertyPagesService.AddPage(ServiceHolder.rm.GetString("ExtInt_13"), (IPropertyPage) new SchemeSettingsControl());
    }
    ContextMenuCommandsProvider.AddContextMenuCommands();
  }

  private static void AddCustomViews()
  {
    ServiceHolder.Factory.AddViewsProvider(1, Const.RequestConfigObjTypeID, (IViewsProvider) new RequestConfigObjectViewProvider());
    ServiceHolder.Factory.AddViewsProvider(1, Const.ResponceConfigObjTypeID, (IViewsProvider) new ResponceConfigObjectViewProvider());
    ServiceHolder.Factory.AddViewsProvider(1, Const.TypeSettingItemObjTypeID, (IViewsProvider) new ObjectTypeSettingViewProvider());
    ServiceHolder.Factory.AddViewsProvider(1, Const.RequestSchemeObjTypeID, (IViewsProvider) new RequestSchemeObjectViewProvider());
    ServiceHolder.Factory.AddViewsProvider(1, Const.ResponceSchemeObjTypeID, (IViewsProvider) new ResponceSchemeObjectViewProvider());
  }

  private static void PopulateNamedImageList()
  {
    Icon icon1 = (Icon) ServiceHolder.rm.GetObject(Const.RequestConfigIconName);
    if (icon1 != null)
      ServiceHolder.NamedImageList.Add(icon1, Const.RequestConfigIconName);
    Icon icon2 = (Icon) ServiceHolder.rm.GetObject(Const.ObjectTypeSettingItemIconName);
    if (icon2 != null)
      ServiceHolder.NamedImageList.Add(icon2, Const.ObjectTypeSettingItemIconName);
    Icon icon3 = (Icon) ServiceHolder.rm.GetObject(Const.RequestSchemeIconName);
    if (icon3 != null)
      ServiceHolder.NamedImageList.Add(icon3, Const.RequestSchemeIconName);
    Icon icon4 = (Icon) ServiceHolder.rm.GetObject(Const.ResponceSchemeIconName);
    if (icon3 != null)
      ServiceHolder.NamedImageList.Add(icon4, Const.ResponceSchemeIconName);
    Icon icon5 = (Icon) ServiceHolder.rm.GetObject(Const.RequestCommandImage);
    if (icon5 == null)
      return;
    ServiceHolder.NamedImageList.Add(icon5, Const.RequestCommandImage);
  }

  private static void InitializeServiceHolder(IServiceProvider serviceProvider)
  {
    ServiceHolder.BarManager = serviceProvider.GetService(typeof (BarManager)) as BarManager;
    ServiceHolder.Factory = serviceProvider.GetService(typeof (IFactory)) as IFactory;
    ServiceHolder.GuidMapper = serviceProvider.GetService(typeof (IGuidMapper)) as IGuidMapper;
    ServiceHolder.CategoryTypeIconService = serviceProvider.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
    ServiceHolder.NamedImageList = serviceProvider.GetService(typeof (INamedImageList)) as INamedImageList;
    ServiceHolder.NotificationService = serviceProvider.GetService(typeof (INotificationService)) as INotificationService;
    ServiceHolder.ObjectCreatorService = serviceProvider.GetService(typeof (IObjectCreatorService)) as IObjectCreatorService;
    ServiceHolder.PropertyPagesService = serviceProvider.GetService(typeof (IPropertyPagesService)) as IPropertyPagesService;
  }

  public void Unload()
  {
    if (ServiceHolder.ObjectCreatorService == null)
      return;
    ServiceHolder.ObjectCreatorService.UnregisterCreatorCustomService(Const.RequestSchemeObjTypeID, typeof (RequestSchemeCreator));
    ServiceHolder.ObjectCreatorService.UnregisterCreatorCustomService(Const.ResponceSchemeObjTypeID, typeof (ResponceSchemeCreator));
  }

  public string Name => "Клиентская часть модуля интеграции с внешними системами";
}
