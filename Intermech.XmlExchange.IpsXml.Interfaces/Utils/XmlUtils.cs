// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Interfaces.Utils.XmlUtils
// Assembly: Intermech.XmlExchange.IpsXml.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 741EAC98-7C4B-42E4-B0B7-F40794536EF7
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.xml

using System.IO;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Interfaces.Utils;

/// <summary>Вспомогательные функции.</summary>
public class XmlUtils
{
  /// <summary>Заменить запрещенные символы для имен файлов</summary>
  /// <param name="fileName">Имя файла</param>
  /// <returns></returns>
  public static string ReplaceForbiddenSymbols(string fileName)
  {
    foreach (char invalidPathChar in Path.GetInvalidPathChars())
      fileName = fileName.Replace(invalidPathChar, '_');
    return fileName;
  }

  /// <summary>Пересоздать директорию.</summary>
  /// <param name="dirName">Имя директории</param>
  /// <remarks>Если директория существует, то она будет удалена со всем содержимым и создана заново.</remarks>
  public static void RecreateDirectory(string dirName)
  {
    if (Directory.Exists(dirName))
      Directory.Delete(dirName, true);
    Directory.CreateDirectory(dirName);
  }
}
