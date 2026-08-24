// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Interfaces.Logger.Configuration.LogMessageTypesExtension
// Assembly: Intermech.XmlExchange.IpsXml.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 741EAC98-7C4B-42E4-B0B7-F40794536EF7
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.xml

using System;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Interfaces.Logger.Configuration;

/// <summary>Вспомогательные функции к AttrType.</summary>
public static class LogMessageTypesExtension
{
  /// <summary>Буфер для хранения заголовков</summary>
  private static readonly string[] CaptionBuffer;

  /// <summary>
  /// 
  /// </summary>
  static LogMessageTypesExtension()
  {
    int val2 = 0;
    Array values = Enum.GetValues(typeof (LogMessageTypes));
    foreach (int val1 in values)
      val2 = Math.Max(val1, val2);
    LogMessageTypesExtension.CaptionBuffer = new string[val2 + 1];
    foreach (object index in values)
      LogMessageTypesExtension.CaptionBuffer[(int) index] = EnumTypeHelper.GetCaption((Enum) (LogMessageTypes) index);
  }

  /// <summary>
  /// Преобразование в Строковое представление типа для лога.
  /// </summary>
  /// <param name="target">Тип сообщения.</param>
  /// <returns>Строковое представление типа для лога.</returns>
  public static string ToLogString(this LogMessageTypes target)
  {
    return LogMessageTypesExtension.CaptionBuffer[(int) target];
  }
}
