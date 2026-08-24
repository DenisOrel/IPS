// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.TechTypes.Settings.TechTypeListHelper
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.TechTypes;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.TechTypes.Settings;

public class TechTypeListHelper
{
  private const string TechTypesSection = "TechTypes";

  public static bool LoadFromFile(string fileName, out TechTypeList techTypeList)
  {
    techTypeList = (TechTypeList) null;
    if (fileName == string.Empty || !System.IO.File.Exists(fileName))
      return false;
    FileStream fileStream = System.IO.File.OpenRead(fileName);
    try
    {
      byte[] buffer = new byte[fileStream.Length];
      fileStream.Read(buffer, 0, buffer.Length);
      MemoryStream serializationStream = new MemoryStream(buffer);
      IFormatter formatter = (IFormatter) new BinaryFormatter();
      techTypeList = formatter.Deserialize((Stream) serializationStream) as TechTypeList;
    }
    finally
    {
      fileStream.Close();
    }
    return true;
  }

  public static bool SaveToFile(string fileName, TechTypeList techTypeList)
  {
    if (techTypeList == null)
      throw new ArgumentNullException(nameof (techTypeList));
    MemoryStream serializationStream = new MemoryStream();
    new BinaryFormatter().Serialize((Stream) serializationStream, (object) techTypeList);
    byte[] array = serializationStream.ToArray();
    FileStream fileStream = System.IO.File.Create(fileName);
    try
    {
      fileStream.Write(array, 0, array.Length);
      fileStream.Flush();
    }
    finally
    {
      fileStream.Close();
    }
    return true;
  }

  public static bool LoadFromSettings(out TechTypeList techTypeList)
  {
    techTypeList = (TechTypeList) null;
    try
    {
      Dictionary<string, SaveSettingsAttribute[]> settings = ServiceUtils.GetService<ISaveSettings>((object) ServicesManager.ServiceContainer, true).GetSettings("TECHCARDSETTINGS");
      if (settings == null)
        return false;
      SaveSettingsAttribute[] settingsAttributeArray;
      if (!settings.TryGetValue("TechTypes", out settingsAttributeArray))
      {
        string Message = "Настройки типов записей не найдены в конфигурации";
        TechcardConsts.Plugin.appManager.AddWarningMessage(Message);
        return false;
      }
      if (settingsAttributeArray == null || settingsAttributeArray.Length == 0)
        return false;
      using (MemoryStream serializationStream = new MemoryStream(Convert.FromBase64String(settingsAttributeArray[0].AttributeValue)))
      {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        techTypeList = binaryFormatter.Deserialize((Stream) serializationStream) as TechTypeList;
      }
    }
    catch (Exception ex)
    {
      TechcardConsts.Plugin.appManager.AddWarningMessage($"Невозможно загрузить настройки правил перекачки типов: {ex.Message}");
      if (!(ex is OutOfMemoryException))
        return false;
      throw;
    }
    return true;
  }

  public static bool SaveToSettings(TechTypeList techTypeList)
  {
    if (techTypeList == null)
    {
      string str = "TechTypeListHelper.SaveToSettings(TechTypeList techTypeList) ошибка входного параметра";
      TechcardConsts.Plugin.appManager.AddWarningMessage(str);
      throw new ArgumentNullException(nameof (techTypeList), str);
    }
    bool settings1 = true;
    string str1 = string.Empty;
    try
    {
      using (MemoryStream serializationStream = new MemoryStream())
      {
        new BinaryFormatter().Serialize((Stream) serializationStream, (object) techTypeList);
        str1 = Convert.ToBase64String(serializationStream.ToArray());
      }
    }
    catch (Exception ex)
    {
      TechcardConsts.Plugin.appManager.AddWarningMessage($"Невозможно сохранить настройки правил перекачки типов Techcard: {ex.Message}");
      if (ex is OutOfMemoryException)
        throw;
    }
    if (str1 == string.Empty)
    {
      string Message = "TechTypeListHelper.SaveToSettings невозможно получить строковое представление типов";
      TechcardConsts.Plugin.appManager.AddWarningMessage(Message);
    }
    Dictionary<string, SaveSettingsAttribute[]> settings2 = new Dictionary<string, SaveSettingsAttribute[]>();
    try
    {
      settings2.Add("TechTypes", new SaveSettingsAttribute[1]
      {
        new SaveSettingsAttribute("Data", str1)
      });
      ServiceUtils.GetService<ISaveSettings>((object) ServicesManager.ServiceContainer, true).SetSettings("TECHCARDSETTINGS", settings2);
    }
    catch (Exception ex)
    {
      settings1 = false;
      TechcardConsts.Plugin.appManager.AddWarningMessage($"Невозможно сохранить настройки правил перекачки типов: {ex.Message}");
      if (ex is OutOfMemoryException)
        throw;
    }
    return settings1;
  }
}
