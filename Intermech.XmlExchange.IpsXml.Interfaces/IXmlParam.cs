// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Interfaces.IXmlParam
// Assembly: Intermech.XmlExchange.IpsXml.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 741EAC98-7C4B-42E4-B0B7-F40794536EF7
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.xml

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Interfaces;

/// <summary>Отдельный параметр.</summary>
public interface IXmlParam : IXmlEntity
{
  /// <summary>Уникальный идентификатор параметра.</summary>
  string Id { get; }

  /// <summary>Наименование параметра.</summary>
  string Name { get; }

  /// <summary>Значение параметра.</summary>
  string Value { get; }
}
