// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Interfaces.Logger.Configuration.LogMessageTypes
// Assembly: Intermech.XmlExchange.IpsXml.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 741EAC98-7C4B-42E4-B0B7-F40794536EF7
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.xml

using System;
using System.ComponentModel;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Interfaces.Logger.Configuration;

/// <summary>Типы сообщений логгирования.</summary>
[Flags]
public enum LogMessageTypes
{
  [Description("Информация")] Info = 1,
  [Description("Предупреждение")] Warn = 2,
  [Description("Ошибка")] Error = 4,
}
