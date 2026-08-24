// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Interfaces.Logger.Configuration.LoggerConfig
// Assembly: Intermech.XmlExchange.IpsXml.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 741EAC98-7C4B-42E4-B0B7-F40794536EF7
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.xml

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Interfaces.Logger.Configuration;

/// <summary>Настройки логгирования.</summary>
public class LoggerConfig
{
  /// <summary>Типы сообщений.</summary>
  public LogMessageTypes MessageTypes { get; set; }
}
