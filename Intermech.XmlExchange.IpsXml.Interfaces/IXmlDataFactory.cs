// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Interfaces.IXmlDataFactory
// Assembly: Intermech.XmlExchange.IpsXml.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 741EAC98-7C4B-42E4-B0B7-F40794536EF7
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.xml

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Interfaces;

/// <summary>Фабрика по загрузке данных из любого XML.</summary>
/// <remarks>См. реализации конкретных классов.</remarks>
public interface IXmlDataFactory
{
  /// <summary>
  /// Загрузить Файл/Файлы и предоставить доступ к их содержимому через провайдер.
  /// </summary>
  /// <param name="files">Файл/Файлы которые необходимо загрузить</param>
  /// <returns>Провайдер, обеспечивающий доступ к загруженным данных из файлов.</returns>
  IXmlDataProvider GetDataProvider(params string[] files);
}
