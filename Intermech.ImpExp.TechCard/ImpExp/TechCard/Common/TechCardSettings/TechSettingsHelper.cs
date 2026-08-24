// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.TechCardSettings.TechSettingsHelper
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.Interfaces.Client;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common.TechCardSettings;

internal static class TechSettingsHelper
{
  private static TechPumpMode _pumpMode = TechPumpMode.tpmAll;
  private static TechPumpMetaDataType _pumpMetaDataType = TechPumpMetaDataType.AutoSelection | TechPumpMetaDataType.ScriptForms | TechPumpMetaDataType.ExpertTables | TechPumpMetaDataType.ExpertFormula | TechPumpMetaDataType.DocumentSettings;
  private static TechPumpDataType _pumpDataType = TechPumpDataType.Route | TechPumpDataType.Zagot | TechPumpDataType.MatGroup | TechPumpDataType.TechProc;
  private static readonly List<int> _pumpArchiveIDS = new List<int>();
  private static readonly List<int> _pumpArchiveDocIDS = new List<int>();
  private static readonly List<int> _pumpDocList = new List<int>();
  private static readonly List<ArtInfoLight> _pumpArtList = new List<ArtInfoLight>();
  private static readonly List<ArtInfoLight> _pumpProdZakList = new List<ArtInfoLight>();
  private static bool _tpComplectPumpMode;
  private static string _tpComplectPumpDir = string.Empty;
  private static bool _settingsChanged;
  private const string _settingsSection = "TechCardSettingsData";

  private static void InitDefaultComplectSettings()
  {
    if (!(TechSettingsHelper._tpComplectPumpDir == string.Empty))
      return;
    object obj = Registry.GetValue("HKEY_CURRENT_USER\\Software\\InterMech\\TechCard\\3.5\\Placement\\TC_PREPARE_TO_EXPORT\\ComplectSettingsFrm", "KTD_DIRECTORY", (object) null);
    if (obj == null)
      return;
    string path = obj.ToString();
    if (!Directory.Exists(path))
      return;
    TechSettingsHelper.TPComplectPumpDir = path;
    TechSettingsHelper.TPComplectPumpMode = true;
  }

  static TechSettingsHelper() => TechSettingsHelper.LoadSettings();

  public static bool LoadSettings()
  {
    bool flag = true;
    Dictionary<string, SaveSettingsAttribute[]> settings = (ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings).GetSettings("TechCardSettingsData");
    if (settings == null)
      return false;
    SaveSettingsAttribute[] settingsAttributeArray = (SaveSettingsAttribute[]) null;
    List<int> list1;
    List<ArtInfoLight> list2;
    if (settings.TryGetValue("TechPumpMode", out settingsAttributeArray))
    {
      foreach (SaveSettingsAttribute settingsAttribute in settingsAttributeArray)
      {
        switch (settingsAttribute.AttributeName)
        {
          case "ArchiveDocIDS":
            if (TechUtils.String2GenericList<int>(settingsAttribute.AttributeValue, out list1))
            {
              TechSettingsHelper._pumpArchiveDocIDS.Clear();
              TechSettingsHelper._pumpArchiveDocIDS.AddRange((IEnumerable<int>) list1);
              break;
            }
            break;
          case "ArchiveIDS":
            if (TechUtils.String2GenericList<int>(settingsAttribute.AttributeValue, out list1))
            {
              TechSettingsHelper._pumpArchiveIDS.Clear();
              TechSettingsHelper._pumpArchiveIDS.AddRange((IEnumerable<int>) list1);
              break;
            }
            break;
          case "ArtList":
            if (TechUtils.String2GenericList<ArtInfoLight>(settingsAttribute.AttributeValue, out list2))
            {
              TechSettingsHelper._pumpArtList.Clear();
              TechSettingsHelper._pumpArtList.AddRange((IEnumerable<ArtInfoLight>) list2);
              break;
            }
            break;
          case "DocList":
            if (TechUtils.String2GenericList<int>(settingsAttribute.AttributeValue, out list1))
            {
              TechSettingsHelper._pumpDocList.Clear();
              TechSettingsHelper._pumpDocList.AddRange((IEnumerable<int>) list1);
              break;
            }
            break;
          case "IgnoreRouteTemplates":
            TechSettingsHelper.IgnoreRouteTemplates = Convert.ToBoolean(Convert.ToInt32(settingsAttribute.AttributeValue));
            break;
          case "ProdZakList":
            if (TechUtils.String2GenericList<ArtInfoLight>(settingsAttribute.AttributeValue, out list2))
            {
              TechSettingsHelper._pumpProdZakList.Clear();
              TechSettingsHelper._pumpProdZakList.AddRange((IEnumerable<ArtInfoLight>) list2);
              break;
            }
            break;
          case "PumpDataType":
            TechSettingsHelper._pumpDataType = (TechPumpDataType) Convert.ToInt32(Convert.ToInt32(settingsAttribute.AttributeValue));
            break;
          case "PumpLinksOnlyWithActual":
            TechSettingsHelper.PumpLinksOnlyWithActual = Convert.ToBoolean(Convert.ToInt32(settingsAttribute.AttributeValue));
            break;
          case "PumpMetaDataType":
            TechSettingsHelper._pumpMetaDataType = (TechPumpMetaDataType) Convert.ToInt32(Convert.ToInt32(settingsAttribute.AttributeValue));
            break;
          case "PumpMode":
            TechSettingsHelper._pumpMode = (TechPumpMode) Convert.ToInt32(Convert.ToInt32(settingsAttribute.AttributeValue));
            break;
        }
      }
    }
    else
      flag = false;
    if (settings.TryGetValue("TechTpComplect", out settingsAttributeArray))
    {
      foreach (SaveSettingsAttribute settingsAttribute in settingsAttributeArray)
      {
        switch (settingsAttribute.AttributeName)
        {
          case "PumpMode":
            TechSettingsHelper._tpComplectPumpMode = Convert.ToBoolean(Convert.ToInt32(settingsAttribute.AttributeValue));
            break;
          case "PumpDir":
            TechSettingsHelper._tpComplectPumpDir = settingsAttribute.AttributeValue;
            break;
        }
      }
    }
    else
    {
      TechSettingsHelper.InitDefaultComplectSettings();
      flag = false;
    }
    return flag;
  }

  public static void SaveSettings()
  {
    if (!TechSettingsHelper._settingsChanged)
      return;
    ISaveSettings service = ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings;
    Dictionary<string, SaveSettingsAttribute[]> dictionary = new Dictionary<string, SaveSettingsAttribute[]>();
    List<SaveSettingsAttribute> settingsAttributeList = new List<SaveSettingsAttribute>();
    settingsAttributeList.Add(new SaveSettingsAttribute("PumpMetaDataType", Convert.ToInt32((object) TechSettingsHelper.PumpMetaDataType).ToString()));
    settingsAttributeList.Add(new SaveSettingsAttribute("PumpMode", Convert.ToInt32((object) TechSettingsHelper.PumpMode).ToString()));
    settingsAttributeList.Add(new SaveSettingsAttribute("PumpDataType", Convert.ToInt32((object) TechSettingsHelper.PumpDataType).ToString()));
    settingsAttributeList.Add(new SaveSettingsAttribute("ArchiveIDS", TechUtils.GenericList2String<int>(TechSettingsHelper.PumpArchiveIDS)));
    settingsAttributeList.Add(new SaveSettingsAttribute("ArchiveDocIDS", TechUtils.GenericList2String<int>(TechSettingsHelper.PumpArchiveDocIDS)));
    settingsAttributeList.Add(new SaveSettingsAttribute("DocList", TechUtils.GenericList2String<int>(TechSettingsHelper.PumpDocList)));
    settingsAttributeList.Add(new SaveSettingsAttribute("ArtList", TechUtils.GenericList2String<ArtInfoLight>(TechSettingsHelper.PumpArtList)));
    settingsAttributeList.Add(new SaveSettingsAttribute("ProdZakList", TechUtils.GenericList2String<ArtInfoLight>(TechSettingsHelper.PumpProdZakList)));
    settingsAttributeList.Add(new SaveSettingsAttribute("IgnoreRouteTemplates", Convert.ToInt32(TechSettingsHelper.IgnoreRouteTemplates).ToString()));
    settingsAttributeList.Add(new SaveSettingsAttribute("PumpLinksOnlyWithActual", Convert.ToInt32(TechSettingsHelper.PumpLinksOnlyWithActual).ToString()));
    dictionary.Add("TechPumpMode", settingsAttributeList.ToArray());
    settingsAttributeList.Clear();
    settingsAttributeList.Add(new SaveSettingsAttribute("PumpMode", Convert.ToInt32(TechSettingsHelper.TPComplectPumpMode).ToString()));
    settingsAttributeList.Add(new SaveSettingsAttribute("PumpDir", TechSettingsHelper.TPComplectPumpDir));
    dictionary.Add("TechTpComplect", settingsAttributeList.ToArray());
    Dictionary<string, SaveSettingsAttribute[]> settings = dictionary;
    service.SetSettings("TechCardSettingsData", settings);
    TechSettingsHelper._settingsChanged = false;
  }

  public static TechPumpMode PumpMode
  {
    get => TechSettingsHelper._pumpMode;
    set
    {
      if (TechSettingsHelper._pumpMode == value)
        return;
      TechSettingsHelper._pumpMode = value;
      TechSettingsHelper._settingsChanged = true;
    }
  }

  public static TechPumpMetaDataType PumpMetaDataType
  {
    [DebuggerStepThrough] get => TechSettingsHelper._pumpMetaDataType;
    [DebuggerStepThrough] set
    {
      if (TechSettingsHelper._pumpMetaDataType == value)
        return;
      TechSettingsHelper._pumpMetaDataType = value;
      TechSettingsHelper._settingsChanged = true;
    }
  }

  public static TechPumpDataType PumpDataType
  {
    [DebuggerStepThrough] get => TechSettingsHelper._pumpDataType;
    [DebuggerStepThrough] set
    {
      if (TechSettingsHelper._pumpDataType == value)
        return;
      TechSettingsHelper._pumpDataType = value;
      TechSettingsHelper._settingsChanged = true;
    }
  }

  public static List<int> PumpArchiveIDS
  {
    get => TechSettingsHelper._pumpArchiveIDS;
    set
    {
      TechSettingsHelper._pumpArchiveIDS.Clear();
      if (value != null)
        TechSettingsHelper._pumpArchiveIDS.AddRange((IEnumerable<int>) value);
      TechSettingsHelper._settingsChanged = true;
    }
  }

  public static List<int> PumpArchiveDocIDS
  {
    get => TechSettingsHelper._pumpArchiveDocIDS;
    set
    {
      TechSettingsHelper._pumpArchiveDocIDS.Clear();
      if (value != null)
        TechSettingsHelper._pumpArchiveDocIDS.AddRange((IEnumerable<int>) value);
      TechSettingsHelper._settingsChanged = true;
    }
  }

  public static List<int> PumpDocList
  {
    get => TechSettingsHelper._pumpDocList;
    set
    {
      TechSettingsHelper._pumpDocList.Clear();
      if (value != null)
        TechSettingsHelper._pumpDocList.AddRange((IEnumerable<int>) value);
      TechSettingsHelper._settingsChanged = true;
    }
  }

  public static List<ArtInfoLight> PumpArtList
  {
    get => TechSettingsHelper._pumpArtList;
    set
    {
      TechSettingsHelper._pumpArtList.Clear();
      if (value != null)
        TechSettingsHelper._pumpArtList.AddRange((IEnumerable<ArtInfoLight>) value);
      TechSettingsHelper._settingsChanged = true;
    }
  }

  public static List<ArtInfoLight> PumpProdZakList
  {
    get => TechSettingsHelper._pumpProdZakList;
    set
    {
      TechSettingsHelper._pumpProdZakList.Clear();
      if (value != null)
        TechSettingsHelper._pumpProdZakList.AddRange((IEnumerable<ArtInfoLight>) value);
      TechSettingsHelper._settingsChanged = true;
    }
  }

  public static bool TPComplectPumpMode
  {
    get => TechSettingsHelper._tpComplectPumpMode;
    set
    {
      if (TechSettingsHelper._tpComplectPumpMode == value)
        return;
      TechSettingsHelper._tpComplectPumpMode = value;
      TechSettingsHelper._settingsChanged = true;
    }
  }

  public static string TPComplectPumpDir
  {
    get => TechSettingsHelper._tpComplectPumpDir;
    set
    {
      if (TechSettingsHelper._tpComplectPumpDir == value)
        return;
      TechSettingsHelper._tpComplectPumpDir = value;
      TechSettingsHelper._settingsChanged = true;
    }
  }

  public static bool IgnoreRouteTemplates { get; set; }

  public static bool PumpLinksOnlyWithActual { get; set; }
}
