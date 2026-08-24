// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.PluginSettings
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.SearchData;

public static class PluginSettings
{
  public static bool AddDocID = true;
  public static bool AddArtID = true;
  public static bool PumpArtVersions = false;
  public static bool PumpSysArtVersions = false;
  public static List<string> ArtSuffixesToDelete = (List<string>) null;
  public static bool OptimizeReadTParams = true;
  public const string SettingsSection = "SEARCHDATA";

  static PluginSettings()
  {
    Dictionary<string, SaveSettingsAttribute[]> settings = (ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings).GetSettings("SEARCHDATA");
    if (settings == null)
      return;
    if (settings.ContainsKey("Common"))
    {
      foreach (SaveSettingsAttribute settingsAttribute in settings["Common"])
      {
        switch (settingsAttribute.AttributeName)
        {
          case nameof (AddDocID):
            PluginSettings.AddDocID = Convert.ToBoolean(Convert.ToInt32(settingsAttribute.AttributeValue));
            break;
          case nameof (AddArtID):
            PluginSettings.AddArtID = Convert.ToBoolean(Convert.ToInt32(settingsAttribute.AttributeValue));
            break;
          case nameof (PumpArtVersions):
            PluginSettings.PumpArtVersions = Convert.ToBoolean(Convert.ToInt32(settingsAttribute.AttributeValue));
            break;
          case nameof (PumpSysArtVersions):
            PluginSettings.PumpSysArtVersions = Convert.ToBoolean(Convert.ToInt32(settingsAttribute.AttributeValue));
            break;
          case nameof (OptimizeReadTParams):
            PluginSettings.OptimizeReadTParams = Convert.ToBoolean(Convert.ToInt32(settingsAttribute.AttributeValue));
            break;
        }
      }
    }
    if (settings.ContainsKey("ImStores"))
    {
      foreach (SaveSettingsAttribute settingsAttribute in settings["ImStores"])
      {
        if (PumpHelper.Plugin.AliasInfo.ContainsKey(settingsAttribute.AttributeName))
          PumpHelper.Plugin.AliasInfo[settingsAttribute.AttributeName][AliasData.FilePath] = settingsAttribute.AttributeValue;
      }
    }
    if (!settings.ContainsKey("DelSuffixes"))
      return;
    SaveSettingsAttribute[] settingsAttributeArray = settings["DelSuffixes"];
    string str = "";
    if (settingsAttributeArray.Length != 0)
      str = settingsAttributeArray[0].AttributeValue;
    PluginSettings.ArtSuffixesToDelete = new List<string>((IEnumerable<string>) str.Split(','));
  }
}
