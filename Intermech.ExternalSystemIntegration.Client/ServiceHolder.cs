// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.ServiceHolder
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using Intermech.Bars;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator.Interfaces;
using System.Reflection;
using System.Resources;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client;

public class ServiceHolder
{
  public static IFactory Factory;
  public static IGuidMapper GuidMapper;
  public static ICategoryTypeIconService CategoryTypeIconService;
  public static BarManager BarManager;
  public static INamedImageList NamedImageList;
  public static INotificationService NotificationService;
  public static IObjectCreatorService ObjectCreatorService;
  public static IPropertyPagesService PropertyPagesService;
  public static ResourceManager rm = new ResourceManager("Intermech.ExternalSystemIntegration.Client.ExtSystIntegrationResources", Assembly.GetExecutingAssembly());
}
