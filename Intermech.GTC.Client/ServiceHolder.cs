// Decompiled with JetBrains decompiler
// Type: Intermech.GTC.Client.ServiceHolder
// Assembly: Intermech.GTC.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 539B70F6-18D3-4230-8795-0EE95CBE5B1C
// Assembly location: D:\IPS\Client\Intermech.GTC.Client.dll

using Intermech.Bars;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator.Interfaces;
using System;
using System.Reflection;
using System.Resources;

#nullable disable
namespace Intermech.GTC.Client;

public class ServiceHolder
{
  private static bool _initialized;
  private static IFactory _factory;
  private static ICategoryTypeIconService _categoryTypeIconService;
  private static BarManager _barManager;
  private static INamedImageList _namedImageList;
  private static INotificationService _notificationService;
  private static IPropertyPagesService _propertyPagesService;
  private static IOutputView _outputView;
  public static ResourceManager Rm = new ResourceManager("Intermech.GTC.Client.GtcClientResources", Assembly.GetExecutingAssembly());

  public static IFactory Factory
  {
    get
    {
      if (!ServiceHolder._initialized)
        throw new Exception("Не иницилизирован харнитель сервисов");
      return ServiceHolder._factory;
    }
  }

  public static ICategoryTypeIconService CategoryTypeIconService
  {
    get
    {
      if (!ServiceHolder._initialized)
        throw new Exception("Не иницилизирован харнитель сервисов");
      return ServiceHolder._categoryTypeIconService;
    }
  }

  public static BarManager BarManager
  {
    get
    {
      if (!ServiceHolder._initialized)
        throw new Exception("Не иницилизирован харнитель сервисов");
      return ServiceHolder._barManager;
    }
  }

  public static INamedImageList NamedImageList
  {
    get
    {
      if (!ServiceHolder._initialized)
        throw new Exception("Не иницилизирован харнитель сервисов");
      return ServiceHolder._namedImageList;
    }
  }

  public static INotificationService NotificationService
  {
    get
    {
      if (!ServiceHolder._initialized)
        throw new Exception("Не иницилизирован харнитель сервисов");
      return ServiceHolder._notificationService;
    }
  }

  public static IPropertyPagesService PropertyPagesService
  {
    get
    {
      if (!ServiceHolder._initialized)
        throw new Exception("Не иницилизирован харнитель сервисов");
      return ServiceHolder._propertyPagesService;
    }
  }

  public static IOutputView OutputView
  {
    get
    {
      if (!ServiceHolder._initialized)
        throw new Exception("Не иницилизирован харнитель сервисов");
      return ServiceHolder._outputView;
    }
  }

  public static void Initialize(IServiceProvider serviceProvider)
  {
    ServiceHolder._factory = serviceProvider.GetService(typeof (IFactory)) as IFactory;
    ServiceHolder._categoryTypeIconService = serviceProvider.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
    ServiceHolder._barManager = serviceProvider.GetService(typeof (BarManager)) as BarManager;
    ServiceHolder._namedImageList = serviceProvider.GetService(typeof (INamedImageList)) as INamedImageList;
    ServiceHolder._notificationService = serviceProvider.GetService(typeof (INotificationService)) as INotificationService;
    ServiceHolder._propertyPagesService = serviceProvider.GetService(typeof (IPropertyPagesService)) as IPropertyPagesService;
    ServiceHolder._outputView = serviceProvider.GetService(typeof (IOutputView)) as IOutputView;
    ServiceHolder._initialized = true;
  }
}
