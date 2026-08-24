// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.Caches.CacheHelper
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.Interfaces.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.Manager.Caches;

public class CacheHelper
{
  public const string CacheFilesExt = ".dat";
  public const string BakFilesExt = ".bak";
  public const string SavePointFileName = "spimpexp.dat";
  public const string PumpPropertiesFileName = "pumpprop.dat";
  private static string _folder = string.Empty;

  internal static RecordType GetRecordType(object oldKey)
  {
    RecordType recordType;
    switch (oldKey.GetType().FullName)
    {
      case "System.Char":
        recordType = RecordType.Char;
        break;
      case "System.String":
        recordType = RecordType.String;
        break;
      case "System.Int64":
        recordType = RecordType.Int64;
        break;
      default:
        recordType = RecordType.Int;
        break;
    }
    return recordType;
  }

  public static string CacheFolder
  {
    get
    {
      if (CacheHelper._folder == string.Empty)
      {
        CacheHelper._folder = !(ServicesManager.GetService(typeof (IConfigurationService)) is IConfigurationService service) || service.Configuration.CacheTempFolder == null || !(service.Configuration.CacheTempFolder != string.Empty) ? Path.GetTempPath() : Intermech.ImpExp.Interface.PathHelper.Normalize(service.Configuration.CacheTempFolder);
        if (!Directory.Exists(CacheHelper._folder))
          Directory.CreateDirectory(CacheHelper._folder);
      }
      return CacheHelper._folder;
    }
    set => CacheHelper._folder = value;
  }

  public static List<string> GetCacheFiles()
  {
    ArrayList arrayList = new ArrayList((ICollection) Enum.GetValues(typeof (ImportingCategory)));
    List<string> cacheFiles = new List<string>(arrayList.Count + 1);
    for (int index = 0; index < arrayList.Count; ++index)
    {
      if ((ImportingCategory) arrayList[index] != ImportingCategory.AttributeTypesToCreate && (ImportingCategory) arrayList[index] != ImportingCategory.ObjectTypesToCreate && (ImportingCategory) arrayList[index] != ImportingCategory.LCSteps4Archives && (ImportingCategory) arrayList[index] != ImportingCategory.ImportingTimer && (ImportingCategory) arrayList[index] != ImportingCategory.ImbaseCatalogBindingType)
        cacheFiles.Add(Path.Combine(CacheHelper.CacheFolder, $"{(ImportingCategory) arrayList[index]}{".dat"}"));
    }
    cacheFiles.Add(Path.Combine(CacheHelper.CacheFolder, "spimpexp.dat"));
    cacheFiles.Add(Path.Combine(CacheHelper.CacheFolder, "pumpprop.dat"));
    string[] files = Directory.GetFiles(CacheHelper.CacheFolder, "cache_*.dat");
    if (files != null && files.Length != 0)
      cacheFiles.AddRange((IEnumerable<string>) files);
    return cacheFiles;
  }
}
