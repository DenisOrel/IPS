// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.SettingsHelper
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Manager.Caches;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.Manager;

public class SettingsHelper
{
  private static string _folder = string.Empty;
  public const string SettingsFileName = "PumpSettings.xml";

  public static string SettingsFolder
  {
    get
    {
      if (SettingsHelper._folder == string.Empty)
      {
        IConfigurationService service = ServicesManager.GetService(typeof (IConfigurationService)) as IConfigurationService;
        SettingsHelper._folder = service.Configuration.SettingsTempFolder == null || !(service.Configuration.SettingsTempFolder != string.Empty) ? Path.GetTempPath() : Intermech.ImpExp.Interface.PathHelper.Normalize(service.Configuration.SettingsTempFolder);
        if (!Directory.Exists(SettingsHelper._folder))
          Directory.CreateDirectory(SettingsHelper._folder);
      }
      return SettingsHelper._folder;
    }
  }

  public static List<string> GetSettingsFiles()
  {
    return new List<string>(1)
    {
      Path.Combine(SettingsHelper.SettingsFolder, "PumpSettings.xml"),
      Path.Combine(CacheHelper.CacheFolder, $"{(Enum) ImportingCategory.AttributeTypesToCreate}{".dat"}"),
      Path.Combine(CacheHelper.CacheFolder, $"{(Enum) ImportingCategory.ObjectTypesToCreate}{".dat"}"),
      Path.Combine(CacheHelper.CacheFolder, $"{(Enum) ImportingCategory.LCSteps4Archives}{".dat"}"),
      Path.Combine(CacheHelper.CacheFolder, $"{(Enum) ImportingCategory.ImbaseCatalogBindingType}{".dat"}")
    };
  }
}
