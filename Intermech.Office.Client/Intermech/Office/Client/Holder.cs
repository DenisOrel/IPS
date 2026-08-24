// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.Holder
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Bars;
using Intermech.Diagnostics;
using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Configuration;
using Intermech.Interfaces.Plugins;
using Intermech.Navigator;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.Office.Client;

internal static class Holder
{
  private static bool _isInit;
  [NotNull]
  private static readonly object _lockObj = new object();

  [NotNull]
  internal static IPackage Plugin { get; private set; }

  [NotNull]
  internal static ICategoryTypeIconService IconService { get; private set; }

  [NotNull]
  internal static IGuidMapper GuidMapper { get; private set; }

  [NotNull]
  internal static IFactory Factory { get; private set; }

  [NotNull]
  internal static INotificationService NotificationService { get; private set; }

  [NotNull]
  internal static INamedImageList NamedList { get; private set; }

  [NotNull]
  internal static IPopupMenuHost PopupHost { get; private set; }

  [NotNull]
  internal static IHotKeysManager HotKeysManager { get; private set; }

  [NotNull]
  internal static ICommandManager CommandManager { get; private set; }

  [CanBeNull]
  internal static IConfigurationManager ConfigurationManager { get; private set; }

  [NotNull]
  internal static IObjectCreatorService ObjectCreatorService { get; private set; }

  [NotNull]
  internal static IUserNamesCache UserNamesCache { get; private set; }

  internal static void Init([NotNull] IPackage plugin, [NotNull] IServiceProvider serviceProvider)
  {
    if (Holder._isInit)
      return;
    lock (Holder._lockObj)
    {
      if (Holder._isInit)
        return;
      Holder.Plugin = plugin;
      Holder.IconService = serviceProvider.GetService<ICategoryTypeIconService>();
      Holder.GuidMapper = serviceProvider.GetService<IGuidMapper>();
      Holder.Factory = serviceProvider.GetService<IFactory>();
      Holder.NotificationService = serviceProvider.GetService<INotificationService>();
      Holder.NamedList = serviceProvider.GetService<INamedImageList>();
      Holder.PopupHost = serviceProvider.GetService<IPopupMenuHost>();
      Holder.HotKeysManager = serviceProvider.GetService<IHotKeysManager>();
      Holder.CommandManager = serviceProvider.GetService<ICommandManager>();
      Holder.ConfigurationManager = serviceProvider.GetService<IConfigurationManager>(false);
      Holder.ObjectCreatorService = serviceProvider.GetService<IObjectCreatorService>();
      Holder.UserNamesCache = CacheManager.Cache("UserNamesCache").As<IUserNamesCache>();
      Holder._isInit = true;
    }
  }
}
