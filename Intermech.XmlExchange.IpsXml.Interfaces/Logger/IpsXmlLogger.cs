// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Interfaces.Logger.IpsXmlLogger
// Assembly: Intermech.XmlExchange.IpsXml.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 741EAC98-7C4B-42E4-B0B7-F40794536EF7
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.xml

using Intermech.XmlExchange.IpsXml.Interfaces.Logger.Configuration;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Interfaces.Logger;

/// <summary>Универсальный логгер в файл.</summary>
public class IpsXmlLogger
{
  /// <summary>
  /// 
  /// </summary>
  private Stream _stream;
  /// <summary>
  /// 
  /// </summary>
  private TextWriter _writer;
  private readonly string _logFileName;
  private LoggerConfig _loggerConfig = new LoggerConfig();

  /// <summary>
  /// 
  /// </summary>
  /// <param name="logFileName">Имя файла, в который будет производится запись</param>
  public IpsXmlLogger(string logFileName) => this._logFileName = logFileName;

  /// <summary>Запись информационного сообщения в файл.</summary>
  /// <param name="message">Сообщение</param>
  public void Info(string message)
  {
    if ((this.LoggerConfig.MessageTypes & LogMessageTypes.Info) == (LogMessageTypes) 0)
      return;
    this.InternalLogMessage(message, LogMessageTypes.Info);
  }

  /// <summary>Запись предупреждения в файл.</summary>
  /// <param name="message">Сообщение</param>
  public void Warn(string message)
  {
    if ((this.LoggerConfig.MessageTypes & LogMessageTypes.Warn) == (LogMessageTypes) 0)
      return;
    this.InternalLogMessage(message, LogMessageTypes.Warn);
  }

  /// <summary>Запись ошибки в файл.</summary>
  /// <param name="message">Сообщение</param>
  public void Error(string message)
  {
    if ((this.LoggerConfig.MessageTypes & LogMessageTypes.Error) == (LogMessageTypes) 0)
      return;
    this.InternalLogMessage(message, LogMessageTypes.Error);
  }

  public void Close()
  {
    try
    {
      if (this._writer != null)
      {
        try
        {
          this._writer.Flush();
          this._writer.Close();
        }
        catch (Exception ex)
        {
        }
      }
      if (this._stream == null)
        return;
      try
      {
        this._stream.Close();
      }
      catch (Exception ex)
      {
      }
    }
    finally
    {
      this._writer = (TextWriter) null;
      this._stream = (Stream) null;
    }
  }

  /// <summary>Очистить лог-файл.</summary>
  public void Clear()
  {
    this.Close();
    if (!File.Exists(this._logFileName))
      return;
    File.Delete(this._logFileName);
  }

  /// <summary>Настройки логирования.</summary>
  public LoggerConfig LoggerConfig => this._loggerConfig;

  /// <summary>Записать сообщение в файл.</summary>
  /// <param name="message">Текст сообщения</param>
  /// <param name="messageType">Тип сообщения</param>
  private void InternalLogMessage(string message, LogMessageTypes messageType)
  {
    this.GetWriter()?.WriteLine($"[{messageType.ToLogString()} {DateTime.Now}]: {message}");
  }

  private TextWriter GetWriter()
  {
    if (this._writer != null)
      return this._writer;
    this._stream = (Stream) new FileStream(this._logFileName, FileMode.Append);
    this._writer = (TextWriter) new StreamWriter(this._stream, Encoding.UTF8, 32768 /*0x8000*/, true);
    return this._writer;
  }
}
