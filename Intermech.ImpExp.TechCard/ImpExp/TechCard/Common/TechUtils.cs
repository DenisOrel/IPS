// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.TechUtils
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common;

internal static class TechUtils
{
  public static string GenericList2String<T>(List<T> list)
  {
    string empty = string.Empty;
    if (list != null)
    {
      if (list.Count != 0)
      {
        try
        {
          BinaryFormatter binaryFormatter = new BinaryFormatter();
          MemoryStream memoryStream = new MemoryStream();
          MemoryStream serializationStream = memoryStream;
          List<T> graph = list;
          binaryFormatter.Serialize((Stream) serializationStream, (object) graph);
          return Convert.ToBase64String(memoryStream.ToArray());
        }
        catch
        {
          throw;
        }
      }
    }
    return empty;
  }

  public static bool String2GenericList<T>(string data, out List<T> list)
  {
    list = (List<T>) null;
    if (string.IsNullOrEmpty(data))
      return false;
    BinaryFormatter binaryFormatter = new BinaryFormatter();
    MemoryStream serializationStream = new MemoryStream(Convert.FromBase64String(data));
    list = binaryFormatter.Deserialize((Stream) serializationStream) as List<T>;
    return true;
  }

  public static class File
  {
    public static void DeleteTmpFiles(Guid pumperGuid)
    {
      try
      {
        string path = Path.Combine(Path.GetTempPath(), pumperGuid.ToString());
        if (!Directory.Exists(path))
          return;
        Directory.Delete(path, true);
      }
      catch (Exception ex)
      {
        TechcardConsts.Plugin.appManager.AddWarningMessage($"невозможно удалить папку со временными файлами, по причине: {ex.Message}");
        if (!(ex is OutOfMemoryException))
          return;
        throw;
      }
    }

    public static string GetTmpFileName(Guid pumperGuid)
    {
      string str = string.Empty;
      try
      {
        str = Path.Combine(Path.GetTempPath(), pumperGuid.ToString());
        if (!Directory.Exists(str))
          Directory.CreateDirectory(str);
        string path2 = $"{Guid.NewGuid()}.tmp";
        return Path.Combine(str, path2);
      }
      catch (Exception ex)
      {
        TechcardConsts.Plugin.appManager.AddWarningMessage(string.Format("Ошибка создания временной директории {0}, по причине: {2}. Все временные файлы будут созданы в директории {1}", (object) str, (object) Path.GetTempPath(), (object) ex.Message));
        if (ex is OutOfMemoryException)
          throw;
      }
      return Path.GetTempFileName();
    }
  }
}
