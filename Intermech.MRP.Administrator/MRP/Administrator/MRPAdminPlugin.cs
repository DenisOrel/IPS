// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Administrator.MRPAdminPlugin
// Assembly: Intermech.MRP.Administrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 6B87B3A6-A601-4A16-AA63-05D1A823449F
// Assembly location: D:\IPS\Client\Intermech.MRP.Administrator.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.Administrator.xml

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.MRP;
using Intermech.Interfaces.Plugins;
using Intermech.Localization;
using System;
using System.Diagnostics;

#nullable disable
namespace Intermech.MRP.Administrator;

/// <summary>Плагин Intermech.MRP.Admin</summary>
internal class MRPAdminPlugin : IPackage
{
  /// <summary>Guid плагина</summary>
  private static Guid _pluginGuid = new Guid("{7A9A5E6B-EFB6-4098-B904-32B2A32E7B76}");
  /// <summary>Является ли текущий пользователь администратором</summary>
  private static bool _isUserAdmin = false;
  /// <summary>
  /// Если данное свойство равно true, все механизмы плагина должны быть заблокированы
  /// </summary>
  internal static bool PluginLocked = false;

  /// <summary>Guid плагина</summary>
  internal static Guid PluginGuid => MRPAdminPlugin._pluginGuid;

  /// <summary>Имя плагина</summary>
  public string Name
  {
    [DebuggerStepThrough] get => LocalizationHolder.rm.GetString("MRP_ADMIN_4");
  }

  /// <summary>Выполнить инициализацию плагина</summary>
  /// <param name="serviceProvider">Контейнер сервисов</param>
  public void Load(IServiceProvider serviceProvider)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      MRPAdminPlugin._isUserAdmin = sessionKeeper.Session.IsAdmin;
      MRPAdminPlugin.PluginLocked = !MRPAdminPlugin._isUserAdmin;
      if (!MRPAdminPlugin.PluginLocked)
      {
        MRPSettings serviceInstance = new MRPSettings(sessionKeeper.Session);
        if (!(ServicesManager.GetService(typeof (IMRPSettings)) is IMRPSettings))
          ServicesManager.AddService(typeof (IMRPSettings), (object) serviceInstance);
        MRPSettingsPropertiesPage settingsPropertiesPage = new MRPSettingsPropertiesPage((IServiceProvider) ServicesManager.ServiceContainer);
      }
      else
        MRPAdminPlugin.PluginLocked = true;
    }
  }

  /// <summary>Выгрузка модуля расширения</summary>
  public void Unload()
  {
  }
}
